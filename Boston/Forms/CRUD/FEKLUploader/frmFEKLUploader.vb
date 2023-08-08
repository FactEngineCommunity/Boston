Imports System.Reflection
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Schema
Imports System.IO
Imports Newtonsoft.Json.Linq

Public Class frmFEKLUploader

    Public WithEvents mrModel As FBM.Model

    Private mrFEKL4JSON As New FEKL.FEKL4JSON

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

    Private Sub ButtonLoadIntoModel_Click(sender As Object, e As EventArgs) Handles ButtonLoadIntoModel.Click

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

            For liInd = 0 To Me.RichTextBoxFEKLDocument.Lines.Length - 1
                lsFEKLStatement = Me.RichTextBoxFEKLDocument.Lines(liInd)

                Try
                    With New WaitCursor

                        Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.LightBlue)
                        Me.RichTextBoxFEKLDocument.Refresh()

                        lrDuplexServiceClientError = prApplication.Brain.ProcessFBMInterfaceFEKLStatement(lsFEKLStatement)

                        If lrDuplexServiceClientError.ErrorType <> [Interface].publicConstants.pcenumErrorType.None Then

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

                        Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.White)

                    End With

                Catch ex As Exception

                    lbErrorThrown = True

                    Me.LabelErrorType.Text = "Unknown Error"
                    Me.LabelPromptErrorMessage.Text = ex.Message

                    Me.RichTextBoxFEKLDocument.HighlightLine(liInd, Color.Orange)

                    Try
                        Me.RichTextBoxFEKLDocument.gotoline(liInd)
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
                        ' Deserialize JSON into RootObject
                        Me.mrFEKL4JSON = JsonConvert.DeserializeObject(Of FEKL.FEKL4JSON)(jsonContent)

                        ' Assign RootObject as DataSource to DataGridView
                        DataGridView1.DataSource = Me.mrFEKL4JSON.FEKLStatement

                        ' Find the index of the FEKLStatement column
                        Dim feklStatementColumnIndex As Integer = DataGridView1.Columns("FEKLStatement").Index

                        ' Move the FEKLStatement column to the first position
                        DataGridView1.Columns(feklStatementColumnIndex).DisplayIndex = 0

                        ' Reorder the display indices of the remaining columns
                        Dim columnIndex As Integer = 1
                        For Each column As DataGridViewColumn In DataGridView1.Columns
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

    Private Sub ButtonLoadJSONFEKL_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try
            Dim lrDuplexServiceClientError As DuplexServiceClient.DuplexServiceClientError = Nothing
            Dim lbErrorThrown As Boolean = False

            'Set the Brain's Model
            prApplication.Brain.Model = Me.mrModel
            prApplication.Brain.Page = Nothing

            Dim liInd = 0
            For Each lrFEKLObject As FEKL.FEKL4JSONObject In Me.mrFEKL4JSON.FEKLStatement

                '===============================
                'Make Row Visisble
                Dim displayedRowCount As Integer = DataGridView1.DisplayedRowCount(False)
                Try
                    ' Calculate the new first displayed row index to show the target row at the bottom
                    Dim newFirstRowIndex As Integer = Math.Max(0, liInd - displayedRowCount + 1)
                    DataGridView1.FirstDisplayedScrollingRowIndex = newFirstRowIndex
                Catch ex As Exception
                    'Well, we tried.
                End Try

                '===============================


                lrDuplexServiceClientError = prApplication.Brain.ProcessFBMInterfaceFEKLStatement(lrFEKLObject.FEKLStatement)

                DataGridView1.Rows(liInd).DataBoundItem.Processing = True
                DataGridView1.InvalidateRow(liInd)
                Application.DoEvents()

                If lrDuplexServiceClientError.ErrorType = [Interface].publicConstants.pcenumErrorType.None Then
                    'No Error
                    DataGridView1.Rows(liInd).DataBoundItem.ErrorType = [Interface].publicConstants.pcenumErrorType.None
                Else
                    'Error in FEKL or Execution of FEKL

                    lbErrorThrown = True

                    'Report the Error
                    DataGridView1.Rows(liInd).DataBoundItem.ErrorType = lrDuplexServiceClientError.ErrorType
                    DataGridView1.Rows(liInd).DataBoundItem.ErrorString = lrDuplexServiceClientError.ErrorString

                End If

                DataGridView1.Rows(liInd).DataBoundItem.Processing = False
                DataGridView1.InvalidateRow(liInd)
                Application.DoEvents()

                liInd += 1
            Next

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

    Private Sub DataGridView1_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles DataGridView1.RowPrePaint

        Try

            If Me.DataGridView1.Rows(e.RowIndex).DataBoundItem.Processing Then
                Me.DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightBlue
            Else
                If Me.DataGridView1.Rows(e.RowIndex).DataBoundItem.InError Then
                    Me.DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Orange
                Else
                    Me.DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
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

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged

        Try
            If DataGridView1.SelectedRows.Count > 0 Then
                ' At least one row is selected
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

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

End Class