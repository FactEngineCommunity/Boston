Imports System.Reflection
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Schema
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class frmFEKLUploader

    Public WithEvents mrModel As FBM.Model

    Private mrFEKL4JSON As New FEKL.FEKL4JSON
    Private mrFEKL4JSONCopy As FEKL.FEKL4JSON

    Private mbProcessingPaused As Boolean = False

    Private miFEKLStraightProcessedUpToLine As Integer = 0

    Private Sub frmFEKLUploader_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SetupForm()

        Try
            Me.LabelModelName.Text = Me.mrModel.Name

            Call Me.LoadDatabaseTypes()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadStraightFEKLStatements(ByVal aiStartLineNumber As Integer)

        Try
            Dim lsFEKLStatement As String
            Dim lrDuplexServiceClientError As DuplexServiceClient.DuplexServiceClientError = Nothing

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

                        lrDuplexServiceClientError = prApplication.Brain.ProcessFBMInterfaceFEKLStatement(lsFEKLStatement)

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub RichTextBoxFEKLDocument_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RichTextBoxFEKLDocument.KeyPress

        Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RichTextBoxFEKLDocument_KeyDown(sender As Object, e As KeyEventArgs) Handles RichTextBoxFEKLDocument.KeyDown

        Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RichTextBoxFEKLDocument_Click(sender As Object, e As EventArgs) Handles RichTextBoxFEKLDocument.Click

        Try
            Dim CaretPosition As Integer = Me.RichTextBoxFEKLDocument.SelectionStart
            Me.RichTextBoxFEKLDocument.SelectAll()
            Me.RichTextBoxFEKLDocument.SelectionBackColor = Me.RichTextBoxFEKLDocument.BackColor
            Me.RichTextBoxFEKLDocument.DeselectAll()
            Try
                Me.RichTextBoxFEKLDocument.SelectionStart = CaretPosition
                Me.RichTextBoxFEKLDocument.SelectionLength = 0
                Me.RichTextBoxFEKLDocument.ScrollToCaret()
            Catch ex As Exception
                'Not a biggie.
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

            Debugger.Break()


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                    Dim validationResults As IList(Of ValidationError)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DataGridView1_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles DataGridViewFEKLStatements.RowPrePaint

        Try

            If Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.Processing Then
                Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue
            Else
                If Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.InError Then
                    Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orange

                ElseIf Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DataBoundItem.Processed Then
                    Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230)

                Else
                    Me.DataGridViewFEKLStatements.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="abIgnoreProcessed">True if you want to ignore previously processed FEKL Statements.</param>
    Private Sub LoadFEKLStatementsIntoModelJSON(Optional ByVal abIgnoreProcessed As Boolean = True)

        Try
            Dim lrDuplexServiceClientError As DuplexServiceClient.DuplexServiceClientError = Nothing
            Dim lbErrorThrown As Boolean = False

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

                DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Processing = True
                DataGridViewFEKLStatements.InvalidateRow(liInd)
                Application.DoEvents()

                If lrDuplexServiceClientError.ErrorType = [Interface].publicConstants.pcenumErrorType.None Then
                    'No Error
                    DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.ErrorType = [Interface].publicConstants.pcenumErrorType.None
                Else
                    'Error in FEKL or Execution of FEKL

                    lbErrorThrown = True

                    'Report the Error
                    DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.ErrorType = lrDuplexServiceClientError.ErrorType
                    DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.ErrorString = lrDuplexServiceClientError.ErrorString

                End If

                Dim liId = DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Id

                DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Processing = False
                DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Processed = True

                Dim lrFEKLStatementCopy = Me.mrFEKL4JSONCopy.FEKLStatement.Find(Function(x) x.Id = liId)
                lrFEKLStatementCopy.ErrorType = DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.ErrorType
                lrFEKLStatementCopy.ErrorString = DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.ErrorString
                lrFEKLStatementCopy.Processing = DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Processing
                lrFEKLStatementCopy.Processed = DataGridViewFEKLStatements.Rows(liInd).DataBoundItem.Processed

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

            'Buttons
            Me.ButtonStopContinueProcessing.Visible = False

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonStopContinueProcessing_Click(sender As Object, e As EventArgs) Handles ButtonStopContinueProcessing.Click

        Dim lbHasProcessedFEKLStatements = Me.mrFEKL4JSON.FEKLStatement.FindAll(Function(x) x.Processed).Count > 0

        Dim larContinuePromptStatus = {"Paused", "NotYetStarted"}

        If larContinuePromptStatus.Contains(Me.ButtonStopContinueProcessing.Tag) Then
            'Start Processing, but show Red Pause Button
            Me.ButtonStopContinueProcessing.BackColor = Color.IndianRed
            Me.ButtonStopContinueProcessing.Text = "Pause Processing"
            Me.ButtonStopContinueProcessing.Tag = "Processing"

            Me.mbProcessingPaused = False

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

    End Sub

    Private Sub frmFEKLUploader_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        Try
            Me.ButtonStopContinueProcessing.Left = (Me.Panel1.Width - Me.ButtonStopContinueProcessing.Width) / 2

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonFEKLStartStop_Click(sender As Object, e As EventArgs) Handles ButtonFEKLStartStop.Click

        Try
            Dim lbHasProcessedFEKLStatements = Me.miFEKLStraightProcessedUpToLine > 0

            Dim larContinuePromptStatus = {"Paused", "NotYetStarted"}

            If larContinuePromptStatus.Contains(Me.ButtonFEKLStartStop.Tag) Then
                'Start Processing, but show Red Pause Button
                Me.ButtonFEKLStartStop.BackColor = Color.IndianRed
                Me.ButtonFEKLStartStop.Text = "Pause Processing"
                Me.ButtonFEKLStartStop.Tag = "Processing"

                Me.mbProcessingPaused = False

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
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub
End Class