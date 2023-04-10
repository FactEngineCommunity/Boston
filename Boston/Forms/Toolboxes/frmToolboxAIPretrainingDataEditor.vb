Imports System.Reflection

Public Class frmToolboxAIPretrainingDataEditor

    Public mrModel As FBM.Model
    Public msAITrainingDataFilePath As String = ""

    Private Sub frmToolboxAIPretrainingDataEditor_Load(sender As Object, e As EventArgs) Handles Me.Load

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

        Dim lsMessage As String

        Try
            If My.Settings.FactEngineUseGPT3 Then
#Region "GPT3 Transforms"
                Dim loTransformation As Object = New System.Dynamic.ExpandoObject
                Dim larTransformationTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(36, loTransformation)

                Dim lsGPT3TrainingExamplesFilePath = larTransformationTuples.Where(Function(x) x.ModelId = Me.mrModel.ModelId).Select(Function(x) x.GPT3TrainingFileLocation)(0)

                If System.IO.File.Exists(lsGPT3TrainingExamplesFilePath) And lsGPT3TrainingExamplesFilePath IsNot Nothing Then

                    Me.msAITrainingDataFilePath = lsGPT3TrainingExamplesFilePath

                    Me.RichTextBox.LoadFile(lsGPT3TrainingExamplesFilePath, RichTextBoxStreamType.PlainText)

                Else
                    lsMessage = "Please check the file path set up for your AI pretraining data."
                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,,,)
                    Me.Close()
                End If
#End Region
            Else
                lsMessage = "Please check that your instance of Boston/FactEngine is set up for natural language queries using AI. Closing."
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,,,)
                Me.Close()
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click

        Try
            If Not Me.RichTextBox.Text.EndsWith(vbCrLf & vbCrLf) Then Me.RichTextBox.Text.AppendString(vbCrLf & vbCrLf)
            Me.RichTextBox.SaveFile(Me.msAITrainingDataFilePath, RichTextBoxStreamType.PlainText)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RichTextBox_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox.TextChanged

        Try
            Me.ToolStripButtonSave.Enabled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButtonSave_Click(sender As Object, e As EventArgs) Handles ToolStripButtonSave.Click

        Try
            If Not Me.RichTextBox.Text.EndsWith(vbCrLf & vbCrLf) Then Me.RichTextBox.Text.AppendString(vbCrLf & vbCrLf)
            Me.RichTextBox.SaveFile(Me.msAITrainingDataFilePath, RichTextBoxStreamType.PlainText)
            Me.ToolStripButtonSave.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class