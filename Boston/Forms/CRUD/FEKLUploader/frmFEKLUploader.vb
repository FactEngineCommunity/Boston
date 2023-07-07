Imports System.Reflection
Imports System.Text

Public Class frmFEKLUploader

    Public WithEvents mrModel As FBM.Model

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

End Class