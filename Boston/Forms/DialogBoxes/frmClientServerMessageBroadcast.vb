Imports System.Reflection

Public Class frmClientServerMessageBroadcast
    Private Sub ButtonBroadcastMessage_Click(sender As Object, e As EventArgs) Handles ButtonBroadcastMessage.Click

        Try
            If MsgBox("Are you sure you want to broadcast the message to all users of Boston Online?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                Dim liInd As Integer = 0
                For Each lrUser As ClientServer.User In tableClientServerUser.getAllUsers(True)

                    Dim lrNotification As New ClientServer.NotificationGeneral()
                    lrNotification.TargetUser = lrUser
                    lrNotification.DateTime = DateTime.Now
                    lrNotification.NotificationType = pcenumNotificationType.GeneralNotification
                    lrNotification.Text = Database.MakeStringSafe(Trim(Me.TextBoxNotification.Text))

                    Call tableClientServerNotifications.addNotification(lrNotification)
                    liInd += 1
                Next

                If liInd > 0 Then
                    Dim lfrmFlashCard As New frmFlashCard
                    lfrmFlashCard.ziIntervalMilliseconds = 2500
                    lfrmFlashCard.zsText = "Message sent to " & liInd & " users."
                    lfrmFlashCard.Show(frmMain, "LightGray")
                End If

                Me.Hide()
                Me.Close()
                Me.Dispose()

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
End Class