Imports System.Reflection
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Schema
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports VDS.RDF
Imports VDS.RDF.Parsing
Imports VDS.RDF.Query
Imports VDS.RDF.Query.Patterns
Imports System.Text.RegularExpressions

Public Class frmFEKLUploader

    Public WithEvents mrModel As FBM.Model

    Private mrFEKL4JSON As New FEKL.FEKL4JSON
    Private mrFEKL4JSONCopy As FEKL.FEKL4JSON

    Private mbProcessingPaused As Boolean = False

    Private miFEKLStraightProcessedUpToLine As Integer = 0

    Public zrScanner As FEKL.Scanner
    Public zrParser As FEKL.Parser
    Public WithEvents zrTextHighlighter As FEKL.TextHighlighter
    Private TextMarker As FEKL.Controls.TextMarker

    Private Sub frmFEKLUploader_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SetupForm()

        Try
            Me.LabelModelName.Text = Me.mrModel.Name

            Call Me.LoadDatabaseTypes()

            '-------------------------------------------------------
            'Setup the Parser etc
            '---------------------
            zrScanner = New FEKL.Scanner
            zrParser = New FEKL.Parser(zrScanner)
            'Set in refresh button
            'Me.zrTextHighlighter = New FEKL.TextHighlighter(
            '                       Me.RichTextBoxFEKLDocument,
            '                       Me.zrScanner,
            '                       Me.zrParser)

            Me.TextMarker = New FEKL.Controls.TextMarker(Me.RichTextBoxFEKLDocument)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadDatabaseTypes()

        Try
            Dim lrComboboxItem = New tComboboxItem(pcenumDatabaseType.None, pcenumDatabaseType.None.ToString, pcenumDatabaseType.None)
            Me.ToolStripComboBoxDatabaseType.Items.Add(lrComboboxItem)
            lrComboboxItem = New tComboboxItem(pcenumDatabaseType.SQLite, pcenumDatabaseType.SQLite.ToString, pcenumDatabaseType.SQLite)
            Me.ToolStripComboBoxDatabaseType.Items.Add(lrComboboxItem)

            Me.ToolStripComboBoxDatabaseType.SelectedIndex = 0

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub ButtonOpenFEKLFile_Click(sender As Object, e As EventArgs) Handles ButtonOpenFEKLFile.Click

        Try
            Dim ofd As New OpenFileDialog()
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"

            If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

                Me.RichTextBoxFEKLDocument.Text = ofd.FileName
                Dim encode As Encoding = Encoding.GetEncoding("GB2312")
                MyData.TheDoc = System.IO.File.ReadAllText(ofd.FileName, encode)
                Me.RichTextBoxFEKLDocument.Text = MyData.TheDoc

                Me.LabelErrorType.Text = "N/A"
                Me.LabelErrorMessage.Text = "N/A"

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadStraightFEKLStatements(ByVal aiStartLineNumber As Integer)

        Try
            Dim lsFEKLStatement As String
            Dim lrDuplexServiceClientError As DuplexServiceClient.DuplexServiceClientError = Nothing

            'CodeSafe
            If prApplication.Brain.Model Is Nothing Then
                If Me.mrModel IsNot Nothing Then
                    prApplication.Brain.Model = Me.mrModel
                    prApplication.WorkingModel = Me.mrModel
                ElseIf prApplication.WorkingModel Is Nothing Then
                    Throw New Exception("The Brain has no Working Model")
                Else
                    '20250902-VM-Consider asking the user to confirm the Working Model.
                    'prApplication.Brain.Model = prApplication.WorkingModel
                    If Me.mrModel IsNot Nothing Then
                        prApplication.Brain.Model = Me.mrModel
                        prApplication.WorkingModel = Me.mrModel
                    End If
                End If
            End If

            'Housekeeping
            Me.LabelErrorType.Text = "N/A"
            Me.LabelErrorMessage.Text = "N/A"

            'Set the Brain's Model
            prApplication.Brain.Model = Me.mrModel
            prApplication.Brain.Page = Nothing

            Dim liInd As Integer

            Dim lbErrorThrown As Boolean = False



            For liInd = aiStartLineNumber To Me.RichTextBoxFEKLDocument.Lines.Length - 1

                'Get the FEKL Statement
                lsFEKLStatement = Me.RichTextBoxFEKLDocument.Lines(liInd)

                Application.DoEvents()

#Region "Pause Processing?"
                If Me.mbProcessingPaused Then
                    'CodeSafe
                    'Pause Processing
                    Me.ButtonFEKLStartStop.BackColor = Color.DarkSeaGreen
                    Me.ButtonFEKLStartStop.Text = "Continue Processing"
                    Me.ButtonFEKLStartStop.Tag = "Paused"
                    Exit Sub
                End If
#End Region

                Me.miFEKLStraightProcessedUpToLine = liInd

                If liInd Mod 500 = 0 Then
                    ' Perform an action on every 500th iteration
                    If Me.mrModel.StoreAsXML Then
                        Call Me.mrModel.Save(,, False)
                    Else
                        Call Me.mrModel.SetStoreAsXML(True, True)
                    End If
                End If

                Dim liPercent = 1
                Try
                    liPercent = Math.Min(liInd / Me.RichTextBoxFEKLDocument.Lines.Length, 1)
                Catch ex As Exception

                End Try
                prApplication.WriteToStatusBar($"Processing: {liInd} of {Me.RichTextBoxFEKLDocument.Lines.Length}", False, liPercent)

                Try
                    With New WaitCursor

                        Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.LightBlue)
                        Me.RichTextBoxFEKLDocument.Refresh()

                        prApplication.Brain.Page = Nothing
                        prApplication.WorkingPage = Nothing

                        Dim lrFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing
                        If Me.TextBoxDefaultDocumentName.Text.Trim <> "" Or
                            Me.TextBoxDefaultDocumentLocation.Text.Trim <> "" Or
                            Me.TextBoxDefaultSectionId.Text.Trim <> "" Or
                            Me.TextBoxDefaultSectionName.Text.Trim <> "" _
                            Or Me.TextBoxDefaultPageNumber.Text.Trim <> "" Then

                            lrFEKLLineageObject = New FEKL.FEKL4JSONObject

                            lrFEKLLineageObject.DocumentName = Me.TextBoxDefaultDocumentName.Text.Trim
                            lrFEKLLineageObject.DocumentLocation = Me.TextBoxDefaultDocumentLocation.Text.Trim
                            lrFEKLLineageObject.SectionId = Me.TextBoxDefaultSectionId.Text.Trim
                            lrFEKLLineageObject.SectionName = Me.TextBoxDefaultSectionName.Text.Trim
                            Try
                                lrFEKLLineageObject.PageNumber = If(Me.TextBoxDefaultPageNumber.Text.Trim = "", 0, Me.TextBoxDefaultPageNumber.Text.Trim)
                            Catch
                                lrFEKLLineageObject.PageNumber = 0
                            End Try
                        End If

                        '==============================================================
                        'Process the FEKL Statement
                        lrDuplexServiceClientError = prApplication.Brain.ProcessFBMInterfaceFEKLStatement(lsFEKLStatement, lrFEKLLineageObject)

                        Dim lbIgnoreDuplicates As Boolean = True

                        If lrDuplexServiceClientError.ErrorType <> [Interface].publicConstants.pcenumErrorType.None Then
                            If (lrDuplexServiceClientError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists And lbIgnoreDuplicates) Then
                                Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.LightCoral)
                            Else
                                lbErrorThrown = True

                                'Report the Error
                                Me.LabelErrorType.Text = lrDuplexServiceClientError.ErrorType.ToString
                                Me.LabelErrorMessage.Text = lrDuplexServiceClientError.ErrorString

                                Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.Orange)

                                Try
                                    Me.RichTextBoxFEKLDocument.GotoLine(liInd)
                                Catch NewEx As Exception
                                    'Not a biggie
                                End Try

#Region "Pause Processing?"
                                Me.mbProcessingPaused = True
                                'CodeSafe
                                'Pause Processing
                                Me.ButtonFEKLStartStop.BackColor = Color.DarkSeaGreen
                                Me.ButtonFEKLStartStop.Text = "Continue Processing"
                                Me.ButtonFEKLStartStop.Tag = "Paused"
#End Region

                                Exit For
                            End If
                        End If

                        Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.White)

                        Try
                            Me.RichTextBoxFEKLDocument.GotoLine(liInd)
                        Catch ex As Exception

                        End Try

                    End With

                Catch ex As Exception

                    lbErrorThrown = True

                    Me.LabelErrorType.Text = "Unknown Error"
                    Me.LabelPromptErrorMessage.Text = ex.Message

                    Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.Orange)

                    Try
                        Me.RichTextBoxFEKLDocument.GotoLine(liInd)
                    Catch NewEx As Exception
                        'Not a biggie
                    End Try

                    Exit For
                End Try
            Next

            miFEKLStraightProcessedUpToLine = 0
            Me.ButtonFEKLStartStop.BackColor = Color.DarkSeaGreen
            Me.ButtonFEKLStartStop.Text = "Start Processing"
            Me.ButtonFEKLStartStop.Tag = "Paused"

            If Not lbErrorThrown Then
                Dim lfrmFlashCard As New frmFlashCard
                lfrmFlashCard.ziIntervalMilliseconds = 2500
                lfrmFlashCard.zsText = "Success. FEKL file loaded."
                lfrmFlashCard.Show(frmMain, "LightGray")
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub RichTextBoxFEKLDocument_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RichTextBoxFEKLDocument.KeyPress

        Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RichTextBoxFEKLDocument_Click(sender As Object, e As EventArgs) Handles RichTextBoxFEKLDocument.Click

        Try
            Dim CaretPosition As Integer = Me.RichTextBoxFEKLDocument.SelectionStart
            'Me.RichTextBoxFEKLDocument.SelectAll()
            'Me.RichTextBoxFEKLDocument.SelectionBackColor = Me.RichTextBoxFEKLDocument.BackColor
            'Me.RichTextBoxFEKLDocument.DeselectAll()
            'Try
            Me.RichTextBoxFEKLDocument.SelectionStart = CaretPosition
            '    Me.RichTextBoxFEKLDocument.SelectionLength = 0
            'Catch ex As Exception
            '    'Not a biggie.
            'End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub mrModel_Deleting() Handles mrModel.Deleting

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButtonDDLExtractFEKL_Click(sender As Object, e As EventArgs) Handles ToolStripButtonDDLExtractFEKL.Click

        Try
            'CodeSafe
            If Me.TextBoxDDL.Text.Trim = "" Then
                MsgBox("Add some DDL to the text box.")
                Exit Sub
            End If

            Dim lrDatabaseConnection As FactEngine.DatabaseConnection = Nothing

            Select Case CType(Me.ToolStripComboBoxDatabaseType.SelectedItem.ItemData, pcenumDatabaseType)
                Case Is = pcenumDatabaseType.SQLite
                    lrDatabaseConnection = New FactEngine.SQLiteConnection
                Case Else
                    Exit Sub
            End Select

            Dim larTable As List(Of RDS.Table) = lrDatabaseConnection.ParseDDL(Me.TextBoxDDL.Text.Trim)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonOpenFEKLJSONFile_Click(sender As Object, e As EventArgs) Handles ButtonOpenFEKLJSONFile.Click

        Try
            Dim openFileDialog As New OpenFileDialog()
            openFileDialog.Filter = "JSON Files|*.json"

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    Me.LabelFEKLJSON.Text = openFileDialog.FileName

                    Dim jsonContent As String = File.ReadAllText(openFileDialog.FileName)

                    ' Load JSON schema
                    Dim schemaJson As String = Boston.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "Boston.FEKLDataLineageSchemaDefinition.json") ' Path to your JSON schema
                    Dim schema As JSchema = JSchema.Parse(schemaJson)


                    ' Parse the JSON content
                    Dim jsonData As JObject = JObject.Parse(jsonContent)

                    ' Validate JSON against schema
                    Dim validationResults As IList(Of ValidationError) = Nothing
                    Dim isValid As Boolean = jsonData.IsValid(schema, validationResults)

                    If isValid Then
                        'Button
                        Me.ButtonOpenFEKLJSONFile.Enabled = False
                        Me.ButtonStopContinueProcessing.Enabled = True
                        Me.ButtonStopContinueProcessing.Visible = True

                        ' Deserialize JSON into RootObject
                        Me.mrFEKL4JSON = JsonConvert.DeserializeObject(Of FEKL.FEKL4JSON)(jsonContent)

                        Me.mrFEKL4JSON.FEKLStatement.ForEach(Sub(item) item.ErrorType = [Interface].publicConstants.pcenumErrorType.None)

                        ' Assign RootObject as DataSource to DataGridView
                        DataGridViewFEKLStatements.DataSource = Me.mrFEKL4JSON.FEKLStatement

                        Me.mrFEKL4JSONCopy = Me.mrFEKL4JSON.Clone

                        ' Find the index of the FEKLStatement column
                        Dim feklStatementColumnIndex As Integer = DataGridViewFEKLStatements.Columns("FEKLStatement").Index

                        ' Move the FEKLStatement column to the first position
                        DataGridViewFEKLStatements.Columns(feklStatementColumnIndex).DisplayIndex = 0

                        ' Reorder the display indices of the remaining columns
                        Dim columnIndex As Integer = 1
                        For Each column As DataGridViewColumn In DataGridViewFEKLStatements.Columns
                            If column.Index <> feklStatementColumnIndex Then
                                column.DisplayIndex = columnIndex
                                columnIndex += 1
                            End If
                        Next

                    Else
                        ' JSON validation failed, show error messages in MessageBox
                        Dim errorMessage As String = "JSON schema validation failed:" & Environment.NewLine
                        For Each validationResult In validationResults
                            errorMessage += validationResult.Path & ": " & validationResult.Message & Environment.NewLine
                        Next
                        MessageBox.Show(errorMessage, "JSON Schema Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                    ' Handle any exceptions that might occur
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles DataGridViewFEKLStatements.RowPrePaint

        Try

            If Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.Processing Then
                Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue
            Else
                If Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.InError Then

                    If Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists _
                        And Not Me.CheckBoxFlagDuplicates.Checked Then
                        Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230) 'Light Gray
                    Else
                        Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orange
                    End If

                ElseIf Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.Processed Then
                        Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230) 'Light Gray

                    Else
                        Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewFEKLStatements.SelectionChanged

        Try
            If DataGridViewFEKLStatements.SelectedRows.Count > 0 Then
                ' At least one row is selected
                Dim selectedRow As DataGridViewRow = DataGridViewFEKLStatements.SelectedRows(0)

                ' Access the data or perform actions based on the selected row
                If selectedRow.DataBoundItem.InError Then
                    Me.LabelFEKLJSONErrorType.Text = selectedRow.DataBoundItem.ErrorType.ToString
                    Me.LabelFEKLJSONErrorString.Text = selectedRow.DataBoundItem.ErrorString
                Else
                    Me.LabelFEKLJSONErrorType.Text = "N/A"
                    Me.LabelFEKLJSONErrorString.Text = "N/A"
                End If
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub HideProcessedFEKLStatementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideProcessedFEKLStatementsToolStripMenuItem.Click

        Try
            If Me.ExportErrorerFEKLStatementsToolStripMenuItem.Tag = "Hidden" Then
                'Unhide Processed FEKL Statements

                Me.mrFEKL4JSON = Me.mrFEKL4JSONCopy.Clone
                Me.DataGridViewFEKLStatements.DataSource = Me.mrFEKL4JSON.FEKLStatement
                Me.DataGridViewFEKLStatements.Refresh()

                Me.ExportErrorerFEKLStatementsToolStripMenuItem.Tag = "UnHidden"
                Me.HideProcessedFEKLStatementsToolStripMenuItem.Text = "Hide Processed FEKL Statements"
            Else
                'Hide Processed Statements
                Me.mrFEKL4JSON.FEKLStatement = Me.mrFEKL4JSON.FEKLStatement.Where(Function(x) Not x.Processed Or x.InError).ToList

                Me.DataGridViewFEKLStatements.DataSource = Me.mrFEKL4JSON.FEKLStatement
                Me.DataGridViewFEKLStatements.Refresh()

                Me.ExportErrorerFEKLStatementsToolStripMenuItem.Tag = "Hidden"
                Me.HideProcessedFEKLStatementsToolStripMenuItem.Text = "Show All FEKL Statements"
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ExportErrorerFEKLStatementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportErrorerFEKLStatementsToolStripMenuItem.Click

        Try
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "JSON Files (*.json)|*.json"
            saveFileDialog.FileName = "ErroredFEKLStatements.json" ' Set default file name

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim selectedFilePath As String = saveFileDialog.FileName

                ' Filter and select items
                Dim larErrorerFEKLStatement = Me.mrFEKL4JSON.FEKLStatement.Where(Function(x) x.InError).ToList

                Dim lrExportFEKL4JSON As New FEKL.FEKL4JSON
                lrExportFEKL4JSON.FEKLStatement = larErrorerFEKLStatement

                ' Serialize the selected items to JSON
                Dim json As String = JsonConvert.SerializeObject(lrExportFEKL4JSON, Formatting.Indented)

                ' Write the JSON to the selected file
                Try
                    File.WriteAllText(selectedFilePath, json)
                    MessageBox.Show("JSON data exported successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("An error occurred while exporting JSON data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="abIgnoreProcessed">True if you want to ignore previously processed FEKL Statements.</param>
    Private Sub LoadFEKLStatementsIntoModelJSON(Optional ByVal abIgnoreProcessed As Boolean = True)

        Try
            Dim lrDuplexServiceClientError As DuplexServiceClient.DuplexServiceClientError = Nothing

            'Stop|Continue Processing
            Me.ButtonStopContinueProcessing.Visible = True

            'Set the Brain's Model
            prApplication.Brain.Model = Me.mrModel
            prApplication.Brain.Page = Nothing

            Dim larFEKLStatementsToProcess As List(Of FEKL.FEKL4JSONObject) = Nothing

            If abIgnoreProcessed Then
                larFEKLStatementsToProcess = Me.mrFEKL4JSON.FEKLStatement.FindAll(Function(x) x.Processed = False)
            Else
                larFEKLStatementsToProcess = Me.mrFEKL4JSON.FEKLStatement
            End If

            Dim liInd = 0
            For Each lrFEKLObject As FEKL.FEKL4JSONObject In larFEKLStatementsToProcess

                liInd = Me.mrFEKL4JSON.FEKLStatement.IndexOf(lrFEKLObject)

                If Me.TextBoxDefaultDocumentName.Text.Trim <> "" Then lrFEKLObject.DocumentName = Me.TextBoxDefaultDocumentName.Text.Trim
                If Me.TextBoxDefaultDocumentLocation.Text.Trim <> "" Then lrFEKLObject.DocumentName = Me.TextBoxDefaultDocumentLocation.Text.Trim
                If Me.TextBoxDefaultSectionId.Text.Trim <> "" Then lrFEKLObject.DocumentName = Me.TextBoxDefaultSectionId.Text.Trim
                If Me.TextBoxDefaultSectionName.Text.Trim <> "" Then lrFEKLObject.DocumentName = Me.TextBoxDefaultSectionName.Text.Trim
                If Me.TextBoxDefaultPageNumber.Text.Trim <> "" Then lrFEKLObject.DocumentName = Me.TextBoxDefaultPageNumber.Text.Trim

                '===============================
                'Make Row Visisble
                Dim displayedRowCount As Integer = DataGridViewFEKLStatements.DisplayedRowCount(False)
                Try
                    ' Calculate the new first displayed row index to show the target row at the bottom
                    Dim newFirstRowIndex As Integer = Math.Max(0, liInd - displayedRowCount + 1)
                    DataGridViewFEKLStatements.FirstDisplayedScrollingRowIndex = newFirstRowIndex
                Catch ex As Exception
                    'Well, we tried.
                End Try

                '===============================


                lrDuplexServiceClientError = prApplication.Brain.ProcessFBMInterfaceFEKLStatement(lrFEKLObject.FEKLStatement, lrFEKLObject)

                lrFEKLObject.Processing = True
                DataGridViewFEKLStatements.InvalidateRow(liInd)
                Application.DoEvents()

                If lrDuplexServiceClientError.ErrorType = [Interface].publicConstants.pcenumErrorType.None Then
                    'No Error
                    lrFEKLObject.ErrorType = [Interface].publicConstants.pcenumErrorType.None
                ElseIf lrDuplexServiceClientError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists And
                        Not Me.CheckBoxFlagDuplicates.Checked Then
                    lrFEKLObject.ErrorType = [Interface].publicConstants.pcenumErrorType.None
                Else
                    'Error in FEKL or Execution of FEKL                    
                    'Report the Error
                    lrFEKLObject.ErrorType = lrDuplexServiceClientError.ErrorType
                    lrFEKLObject.ErrorString = lrDuplexServiceClientError.ErrorString

                End If

                Dim liId = lrFEKLObject.Id

                lrFEKLObject.Processing = False
                lrFEKLObject.Processed = True

                Dim lrFEKLStatementCopy = Me.mrFEKL4JSONCopy.FEKLStatement.Find(Function(x) x.Id = liId)
                lrFEKLStatementCopy.ErrorType = lrFEKLObject.ErrorType
                lrFEKLStatementCopy.ErrorString = lrFEKLObject.ErrorString
                lrFEKLStatementCopy.Processing = lrFEKLObject.Processing
                lrFEKLStatementCopy.Processed = lrFEKLObject.Processed

                DataGridViewFEKLStatements.InvalidateRow(liInd)
                Application.DoEvents()

#Region "Pause Processing?"
                If Me.mbProcessingPaused Then
                    'CodeSafe
                    'Pause Processing
                    Me.ButtonStopContinueProcessing.BackColor = Color.DarkSeaGreen
                    Me.ButtonStopContinueProcessing.Text = "Continue Processing"
                    Me.ButtonStopContinueProcessing.Tag = "Paused"
                    Exit Sub
                End If
#End Region

                'liInd += 1
            Next

            Me.ButtonStopContinueProcessing.BackColor = Color.DarkSeaGreen
            Me.ButtonStopContinueProcessing.Text = "Start Processing"
            Me.ButtonStopContinueProcessing.Tag = "Paused"


            Dim lsFlashText = "Success. FEKL loaded."
            Dim liErrorCount = Me.mrFEKL4JSON.FEKLStatement.Where(Function(x) x.InError).Count
            If liErrorCount > 0 Then
                lsFlashText.AppendDoubleLineBreak("There were " & liErrorCount & " errors.")
            End If
            Call Boston.ShowFlashCard(lsFlashText, Color.DarkSeaGreen, 3500, 10)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonStopContinueProcessing_Click(sender As Object, e As EventArgs) Handles ButtonStopContinueProcessing.Click

        Dim lsMessage As String

        Try
            'Make sure the WorkingPage is nothing, so that a bunch of model elements don't get dumped onto the Page.
            If prApplication.Brain IsNot Nothing Then
                prApplication.Brain.Page = Nothing
            End If
            prApplication.WorkingPage = Nothing

            Dim lbHasProcessedFEKLStatements = Me.mrFEKL4JSON.FEKLStatement.FindAll(Function(x) x.Processed).Count > 0

            Dim larContinuePromptStatus = {"Paused", "NotYetStarted"}

            If larContinuePromptStatus.Contains(Me.ButtonStopContinueProcessing.Tag) Then
                'Start Processing, but show Red Pause Button
                Me.ButtonStopContinueProcessing.BackColor = Color.IndianRed
                Me.ButtonStopContinueProcessing.Text = "Pause Processing"
                Me.ButtonStopContinueProcessing.Tag = "Processing"

                Me.mbProcessingPaused = False

#Region "Check for Defaults"
                If Me.TextBoxDefaultDocumentName.Text.Trim <> "" Or
                        Me.TextBoxDefaultDocumentLocation.Text.Trim <> "" Or
                        Me.TextBoxDefaultSectionId.Text.Trim <> "" Or
                        Me.TextBoxDefaultSectionName.Text.Trim <> "" _
                        Or Me.TextBoxDefaultPageNumber.Text.Trim <> "" Then

                    lsMessage = "There are default Metadata Lineage items set in the [Default] tab."
                    lsMessage.AppendDoubleLineBreak("Do you want to continue?")
                    lsMessage.AppendDoubleLineBreak("Recommendation: Continue if you are happy for the defaults to have priority over Medata Lineage items in your JSON file.")
                    If Not MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                        Me.ButtonStopContinueProcessing.BackColor = Color.DarkSeaGreen
                        Me.ButtonStopContinueProcessing.Text = "Continue Processing"
                        Me.ButtonStopContinueProcessing.Tag = "Paused"
                        Exit Sub
                    End If
                End If
#End Region

                If lbHasProcessedFEKLStatements Then
                    If MsgBox("Do you want to ignore previously processed FEKL Statements?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        Call Me.LoadFEKLStatementsIntoModelJSON()
                    Else
                        Call Me.LoadFEKLStatementsIntoModelJSON(False)
                    End If

                Else
                    Call Me.LoadFEKLStatementsIntoModelJSON()
                End If

            ElseIf Me.ButtonStopContinueProcessing.Tag = "Processing" Then
                'Pause Processing, but show Green Continue Processing button
                Me.ButtonStopContinueProcessing.BackColor = Color.DarkSeaGreen
                Me.ButtonStopContinueProcessing.Text = "Continue Processing"
                Me.ButtonStopContinueProcessing.Tag = "Paused"

                Me.mbProcessingPaused = True
            End If

            Me.ButtonStopContinueProcessing.Refresh()
            Me.ButtonStopContinueProcessing.Invalidate()
            Me.DataGridViewFEKLStatements.Refresh()
            Me.DataGridViewFEKLStatements.Invalidate()

            Me.Invalidate()

        Catch ex As Exception


            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub frmFEKLUploader_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        Try
            Me.ButtonStopContinueProcessing.Left = (Me.Panel1.Width - Me.ButtonStopContinueProcessing.Width) / 2

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonFEKLStartStop_Click(sender As Object, e As EventArgs) Handles ButtonFEKLStartStop.Click

        Dim lsMessage As String

        Try
            'Make sure the WorkingPage is nothing, so that a bunch of model elements don't get dumped onto the Page.
            If prApplication.Brain IsNot Nothing Then
                prApplication.Brain.Page = Nothing
            End If
            prApplication.WorkingPage = Nothing

            Dim lbHasProcessedFEKLStatements = Me.miFEKLStraightProcessedUpToLine > 0

            Dim larContinuePromptStatus = {"Paused", "NotYetStarted"}

            If larContinuePromptStatus.Contains(Me.ButtonFEKLStartStop.Tag) Then
                'Start Processing, but show Red Pause Button
                Me.ButtonFEKLStartStop.BackColor = Color.IndianRed
                Me.ButtonFEKLStartStop.Text = "Pause Processing"
                Me.ButtonFEKLStartStop.Tag = "Processing"

                Me.mbProcessingPaused = False

#Region "Check for Defaults"
                If Me.TextBoxDefaultDocumentName.Text.Trim <> "" Or
                        Me.TextBoxDefaultDocumentLocation.Text.Trim <> "" Or
                        Me.TextBoxDefaultSectionId.Text.Trim <> "" Or
                        Me.TextBoxDefaultSectionName.Text.Trim <> "" _
                        Or Me.TextBoxDefaultPageNumber.Text.Trim <> "" Then

                    lsMessage = "There are default Metadata Lineage items set in the [Default] tab."
                    lsMessage.AppendDoubleLineBreak("Do you want to continue?")
                    lsMessage.AppendDoubleLineBreak("Recommendation: Continue if you are happy with the default Medata Lineage items in the [Defaults] tab.")
                    If Not MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                        Me.ButtonFEKLStartStop.BackColor = Color.DarkSeaGreen
                        Me.ButtonFEKLStartStop.Text = "Start Processing"
                        Me.ButtonFEKLStartStop.Tag = "Paused"
                        Exit Sub
                    End If
                End If
#End Region

                If lbHasProcessedFEKLStatements Then

                    If MsgBox("Do you want to ignore previously processed FEKL Statements?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        'Nothing to do
                    Else
                        Me.miFEKLStraightProcessedUpToLine = 0
                    End If
                    Call Me.LoadStraightFEKLStatements(Me.miFEKLStraightProcessedUpToLine)
                Else
                    Call Me.LoadStraightFEKLStatements(0)
                End If

            ElseIf Me.ButtonFEKLStartStop.Tag = "Processing" Then
                'Pause Processing, but show Green Continue Processing button
                Me.ButtonFEKLStartStop.BackColor = Color.DarkSeaGreen
                Me.ButtonFEKLStartStop.Text = "Continue Processing"
                Me.ButtonFEKLStartStop.Tag = "Paused"

                Me.mbProcessingPaused = True
            End If

            Me.ButtonFEKLStartStop.Refresh()
            Me.ButtonFEKLStartStop.Invalidate()

            Me.Invalidate()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub CheckBoxFlagDuplicates_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxFlagDuplicates.CheckedChanged

        Try
            Me.DataGridViewFEKLStatements.Refresh()
            Me.DataGridViewFEKLStatements.Invalidate()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Try
            Me.RichTextBoxFEKLDocument.ResetHighlighting

            Dim lsFEKLText = Me.RichTextBoxFEKLDocument.Text
            Dim liInd = 0

            Me.zrTextHighlighter = New FEKL.TextHighlighter(
                                   Me.RichTextBoxFEKLDocument,
                                   Me.zrScanner,
                                   Me.zrParser)

            Me.zrTextHighlighter.Tree = Me.zrParser.Parse(lsFEKLText)

            Me.RichTextBoxFEKLDocument.SelectionStart = Me.RichTextBoxFEKLDocument.Text.Length

            Call Me.zrTextHighlighter.HighlightText()
            Call Me.zrTextHighlighter.HighlightTextInternal()

            If liInd = 1 Then Exit Sub

            Me.TextMarker.Clear()
            Me.LabelErrorType.Text = [Interface].pcenumErrorType.None.ToString
            Me.LabelErrorMessage.Text = ""
            Me.LabelErrorType.ForeColor = Color.Black
            Me.LabelErrorMessage.ForeColor = Color.Black
            If Me.zrTextHighlighter.Tree.Errors.Count > 0 Then
                Me.TextMarker.AddWord(Me.zrTextHighlighter.Tree.Errors(0).Position, Me.zrTextHighlighter.Tree.Errors(0).Length, Color.Red)
                Me.RichTextBoxFEKLDocument.SelectionStart = Me.zrTextHighlighter.Tree.Errors(0).Position
                Me.RichTextBoxFEKLDocument.Invalidate()
                'Me.RichTextBoxFEKLDocument.Refresh()
                'Me.RichTextBoxFEKLDocument.Invalidate()
                'Me.RichTextBoxFEKLDocument.Update()
                'Me.RichTextBoxFEKLDocument.ScrollToCaret()

#Region "Highlight the errored line."
                ' Get the line index of the caret
                Dim currentLine As Integer = Me.RichTextBoxFEKLDocument.GetLineFromCharIndex(Me.RichTextBoxFEKLDocument.SelectionStart)

                ' Get the start and end positions of the line
                Dim start As Integer = Me.RichTextBoxFEKLDocument.GetFirstCharIndexFromLine(currentLine)
                Dim [end] As Integer = Me.RichTextBoxFEKLDocument.GetFirstCharIndexFromLine(currentLine + 1)
                If [end] = -1 Then
                    [end] = Me.RichTextBoxFEKLDocument.TextLength
                End If

                ' Select the line
                Me.RichTextBoxFEKLDocument.Select(start, [end] - start)

                ' Change the background color of the selected line
                Me.RichTextBoxFEKLDocument.SelectionBackColor = Color.Yellow
#End Region
                ' Deselect the text
                Me.RichTextBoxFEKLDocument.SelectionLength = 0

                Me.LabelErrorType.Text = [Interface].pcenumErrorType.SyntaxError.ToString
                Me.LabelErrorMessage.Text = Me.zrTextHighlighter.Tree.Errors(0).Message
                Me.LabelErrorType.ForeColor = Color.Red
                Me.LabelErrorMessage.ForeColor = Color.Red
                Me.TextMarker.MarkWords()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RichTextBoxFEKLDocument_KeyDown(sender As Object, e As KeyEventArgs) Handles RichTextBoxFEKLDocument.KeyDown

        Try
            'CodeSafe
            If Me.zrTextHighlighter Is Nothing Then Exit Sub
            If Me.zrTextHighlighter.threadAutoHighlight Is Nothing Then Exit Sub

            Me.zrTextHighlighter.threadAutoHighlight.Join(10)
            If Me.zrTextHighlighter.threadAutoHighlight.IsAlive Then
                Me.zrTextHighlighter.threadAutoHighlight.Abort()
                Dim liSelectionStart = Me.RichTextBoxFEKLDocument.SelectionStart
                Me.RichTextBoxFEKLDocument.ResetHighlighting
                Me.RichTextBoxFEKLDocument.SelectionStart = liSelectionStart

            End If
            Me.zrTextHighlighter.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RDFOWLttlTurtleFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RDFOWLttlTurtleFileToolStripMenuItem.Click

        Call Me.ImportGenesysRDFOWLTurtleTTLFile()

    End Sub

    Private Sub ImportGenesysRDFOWLTurtleTTLFile()

        Dim lsMessage As String
        Try
            Dim openFileDialog As New OpenFileDialog()


            ' Set the file filter to only show .ttl files
            openFileDialog.Filter = "RDF Turtle Files (*.ttl)|*.ttl"

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Dim filePath As String = openFileDialog.FileName

                ' Create a new RDF graph
                Dim graph As New Graph()

                ' Use the Turtle parser to load the file into the graph
                Try
                    Dim parser As New TurtleParser()
                    parser.Load(graph, filePath)

                    Dim outputLines As New List(Of String)
                    Dim lsRelation As String = ""
                    Dim larObjectTypeName As New List(Of String)
                    Dim larFactTypeName As New List(Of String)

                    ' Display or process the natural language triples with labels
                    For Each triple In graph.Triples

                        ' Get the subject, predicate, and object nodes
                        Dim lrSubject As INode = triple.Subject
                        Dim lrPredicate As INode = triple.Predicate
                        Dim lrObject As INode = triple.Object

                        ' Extract labels using the rdfs:label property
                        '================Subject=================
                        Dim lsSubjectLabel As String = GetRDFLabel(graph, lrSubject)
                        If Uri.IsWellFormedUriString(lsSubjectLabel, UriKind.Absolute) Then
                            lsSubjectLabel = New Uri(lsSubjectLabel).Segments.Last()
                        End If
                        '================Predicate===============
                        Dim lsPredicate As String = GetRDFLabel(graph, lrPredicate)
                        ' Get the local name of the predicate without the full URI
                        lsPredicate = New Uri(lsPredicate).Segments.Last()
                        '================Object==================
                        Dim lsObjectLabel As String = GetRDFLabel(graph, lrObject)
                        If Uri.IsWellFormedUriString(lsObjectLabel, UriKind.Absolute) Then
                            lsObjectLabel = New Uri(lsObjectLabel).Segments.Last()
                        End If

                        Dim lsSubjectLabelPascalCase = lsSubjectLabel.ToPascalCaseWithSpaces
                        Dim lsObjectLabelPascalCase = lsObjectLabel.ToPascalCaseWithSpaces

                        '===========Clean=============================
                        lsSubjectLabelPascalCase = CleanModelelementName(lsSubjectLabelPascalCase)
                        lsObjectLabelPascalCase = CleanModelelementName(lsObjectLabelPascalCase)
                        lsPredicate = CleanModelelementName(lsPredicate)

                        If lsSubjectLabel.StartsWith("SAMPLE") Then GoTo SkipTriple

                        Select Case lsPredicate
                            Case Is = "rdf schema", "isProxy"
                                GoTo SkipTriple
                            Case Is = "identifier", "22-rdf-syntax-ns"
                                GoTo SkipTriple 'For now
                            Case Is = "hasSource", "hasTarget"
                                GoTo SkipTriple 'For now
                            Case Is = "hasGenesysValue", "hasGenesysValueType"
                                GoTo SkipTriple 'For now
                            Case Is = "description"
                                'Process 'GoTo SkipTriple 'For now
                        End Select

                        Dim pattern As String = "(.*?)(\s>\s)(.*?)\s>\s(.*)"
                        Dim match As Match = Regex.Match(lsSubjectLabel, pattern)

                        Select Case lsPredicate
                            Case Is = "description"
                                Dim lsDescription = triple.Object.ToString.Replace("""", """""").Replace(vbCrLf, "").Replace(vbLf, "")
                                Dim lsDescriptionFEKL As String = lsSubjectLabel.ToPascalCaseWithSpaces & " HAS LONG DESCRIPTION """ & lsDescription & """"
                                outputLines.AddUnique(lsDescriptionFEKL)
                            Case Else
                                'hasPart, <other> section. I.e. Triples that have hasPart as the predicate, or some <other> predicate.
                                If match.Success Then
                                    'FactTypeReading
                                    lsSubjectLabelPascalCase = match.Groups(1).Value.Trim().ToPascalCaseWithSpaces
                                    lsPredicate = match.Groups(3).Value.Trim()
                                    lsObjectLabelPascalCase = match.Groups(4).Value.Trim().ToPascalCaseWithSpaces

                                    If Not larObjectTypeName.Contains(lsSubjectLabelPascalCase) Then
                                        lsRelation = $"{lsSubjectLabelPascalCase} IS AN ENTITY TYPE"
                                        outputLines.AddUnique(lsRelation)
                                        larObjectTypeName.AddUnique(lsSubjectLabelPascalCase)
                                    End If

                                    Dim lsFactTypeName = $"{lsSubjectLabelPascalCase}{lsPredicate.ToPascalCase}{lsObjectLabelPascalCase}".RemoveWhitespace
                                    Dim lsFactTypeReading = $"{lsSubjectLabelPascalCase} {lsPredicate} {lsObjectLabelPascalCase}"
                                    lsFactTypeName.RemoveDoubleWhiteSpace.RemoveWhitespace

                                    If Not larFactTypeName.Contains(lsFactTypeName) Then
                                        outputLines.AddUnique(lsFactTypeReading)
                                    End If
                                Else
                                    If Not larObjectTypeName.Contains(lsSubjectLabelPascalCase) Then
                                        lsRelation = $"{lsSubjectLabelPascalCase} IS AN ENTITY TYPE"
                                        outputLines.AddUnique(lsRelation)
                                        larObjectTypeName.AddUnique(lsSubjectLabelPascalCase)
                                    Else
                                        lsRelation = $"{lsSubjectLabelPascalCase} IS AN ENTITY TYPE"
                                        If Not outputLines.Contains(lsRelation) Then
                                            outputLines.AddUnique(lsRelation)
                                        End If
                                    End If

                                    If Not larObjectTypeName.Contains(lsObjectLabelPascalCase) Then
                                        lsRelation = $"{lsObjectLabelPascalCase} IS A CONCEPT"
                                        outputLines.AddUnique(lsRelation)
                                        larObjectTypeName.AddUnique(lsObjectLabelPascalCase)
                                    End If

                                    Dim lsFactTypeName As String = $"{lsSubjectLabelPascalCase}Has{lsObjectLabelPascalCase}".RemoveWhitespace

                                    If Not larFactTypeName.Contains(lsFactTypeName) Then
                                        'Create the Fact Type Reading
                                        Dim lsFactTypeReading = $"{lsSubjectLabelPascalCase} has AT MOST ONE {lsObjectLabelPascalCase}"
                                        outputLines.AddUnique(lsFactTypeReading)
                                    End If
                                End If
                        End Select
SkipTriple:
                    Next

                    ' Save the output lines to a text file
                    Dim saveFileDialog As New SaveFileDialog()
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt"
                    If saveFileDialog.ShowDialog() = DialogResult.OK Then
                        Dim outputFilePath As String = saveFileDialog.FileName
                        System.IO.File.WriteAllLines(outputFilePath, outputLines)
                    End If

                    Dim textWithNewlines As String = String.Join(Environment.NewLine, outputLines)
                    Clipboard.SetText(textWithNewlines)

                    lsMessage = "RDF file imported successfully."
                    lsMessage.AppendDoubleLineBreak("Copied to Clipboard.")

                    Boston.ShowFlashCard(lsMessage, Color.DarkSeaGreen, 2500, 10)

                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Function GetRDFLabel(graph As IGraph, node As INode) As String
        'Dim labelPredicateUri As Uri = New Uri("http://www.w3.org/2000/01/rdf-schema#label")
        'Dim labelPredicate As INode = graph.CreateUriNode(labelPredicateUri)
        'Dim labelTriple As Triple = graph.GetTriplesWithSubjectPredicate(node, labelPredicate).FirstOrDefault()

        'If labelTriple IsNot Nothing Then
        '    Return labelTriple.Object.ToString()
        'Else
        '    Return node.ToString()
        'End If
        Dim rdfs As String = "http://www.w3.org/2000/01/rdf-schema#"
        Dim labelPredicate As INode = graph.CreateUriNode(New Uri(rdfs & "label"))
        Dim labelTriple As Triple = graph.GetTriplesWithSubjectPredicate(node, labelPredicate).FirstOrDefault()

        If labelTriple IsNot Nothing Then
            Return labelTriple.Object.ToString()
        Else
            Return node.ToString()
        End If
    End Function

    Private Function CleanModelelementName(ByVal asModelElementName As String) As String

        Try
            Dim lsReturnString As String = asModelElementName

            lsReturnString = lsReturnString.Replace(".", "")
            lsReturnString = lsReturnString.Replace(" - ", " ")
            lsReturnString = lsReturnString.Replace("-", " ")
            lsReturnString = lsReturnString.Replace("&", "And")
            lsReturnString = lsReturnString.Replace("/", " ")
            lsReturnString = lsReturnString.Replace(",", "")
            lsReturnString = lsReturnString.Replace("_", " ")
            lsReturnString = lsReturnString.Replace("(", "")
            lsReturnString = lsReturnString.Replace(")", "")
            lsReturnString = lsReturnString.Replace(":", "")

            Return lsReturnString.Trim.RemoveDoubleWhiteSpace

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub GenerateModelsFEKLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GenerateModelsFEKLToolStripMenuItem.Click

        Try

#Region "Dynamic Form"
            ' Create form definition JSON
            Dim formDefinition As String = "
                    {
                        'UseEntityGrouping': {
                            'type': 'checkbox',
                            'label': 'Use Entity Grouping',
                            'default': false
                        },
                        'GenerateFacts': {
                            'type': 'checkbox',
                            'label': 'Generate Facts',
                            'default': false
                        }
                    }"

            ' Create and show the form
            Dim form As New frmDynamicForm()
            form.FormFields = formDefinition

            Dim lbUseEntityGrouping As Boolean = False
            Dim lbGenerateFacts As Boolean = False

            If form.ShowDialog() = DialogResult.OK Then
                ' Get the results as JSON
                Dim loResults As JObject = JObject.Parse(form.FormData)


                lbUseEntityGrouping = CBool(loResults("UseEntityGrouping"))
                lbGenerateFacts = CBool(loResults("GenerateFacts"))

            End If
#End Region

#Region "Setup Generic Selection Form"
            Dim lfrmGenericSelect As New frmGenericSelect()
            lfrmGenericSelect.zoGenericSelection.Type = pcenumGenericSelectionType.SelectFromList
            lfrmGenericSelect.zoGenericSelection.FormTitle = "Select Model Element to generate CLIF for"
            lfrmGenericSelect.zoGenericSelection.ComboBoxStyle = pcenumComboBoxStyle.DropdownList

            Dim larModelElement = (From ModelElement In Me.mrModel.getModelObjects(False).FindAll(Function(x) Not x.IsMDAModelElement)
                                   Where {GetType(FBM.EntityType), GetType(FBM.FactType)}.Contains(ModelElement.GetType)
                                   Select ModelElement).ToList.OrderByDescending(Function(x) x.Name)


            For Each lrModelElement In larModelElement
                lfrmGenericSelect.zoGenericSelection.TupleList.Add(New tComboboxItem(Nothing, lrModelElement.Name, lrModelElement))
            Next

            lfrmGenericSelect.zoGenericSelection.TupleList.Add(New tComboboxItem(Nothing, "All", Nothing))
#End Region

            If lfrmGenericSelect.ShowDialog = DialogResult.OK Then
                Me.RichTextBoxFEKLDocument.Text = Me.mrModel.GenerateFEKL(lbUseEntityGrouping, lbGenerateFacts, lfrmGenericSelect.zoGenericSelection.SelectedTag)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class