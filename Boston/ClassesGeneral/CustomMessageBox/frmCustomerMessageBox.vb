Imports System.Reflection

Public Class frmCustomMessageBox

    Public ButtonText As New List(Of String)
    Public Message As String
    Private ReturnValue As String
    Public LeftJustify As Boolean = False

    Private Sub frmCustomMessageBox_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call Me.SetupForm()
    End Sub

    Public Sub SetupForm()

        Try
            Me.LabelMessage.Text = Me.Message
            If Me.LeftJustify Then
                Me.LabelMessage.TextAlign = ContentAlignment.MiddleLeft
                Me.LabelMessage.AutoSize = False
            End If

            Dim loGraphics As Graphics = Me.LabelMessage.CreateGraphics

            Dim loStringSize = loGraphics.MeasureString(Me.Message, Me.LabelMessage.Font, Me.LabelMessage.Width)
            Me.Height = loStringSize.Height + 80

            For Each lsButton In ButtonText
                Dim loButton As New Button()
                loButton.Text = lsButton
                loButton.Tag = lsButton
                loButton.AutoSize = True
                loButton.AutoSizeMode = AutoSizeMode.GrowAndShrink
                loButton.AutoEllipsis = False
                FlowLayoutPanel.Controls.Add(loButton)
                loButton.Anchor = AnchorStyles.Top
                AddHandler loButton.Click, AddressOf Me.ReturnResult
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Overloads Function ShowDialog() As String
        MyBase.ShowDialog()
        Return Me.ReturnValue
    End Function


    Private Sub ReturnResult(sender As Object, e As EventArgs)
        Me.ReturnValue = sender.tag
        Me.Close()
    End Sub

End Class