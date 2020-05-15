Public Class NotificationBarGeneralNotification

    Public Event GotitClicked(ByRef arNotification As ClientServer.NotificationGeneral)

    Private Sub ButtonGotit_Click(sender As Object, e As EventArgs) Handles ButtonGotit.Click

        Me.Dispose()
        RaiseEvent GotitClicked(Me.Tag)
    End Sub

End Class
