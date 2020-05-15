Public Class NotificationBarGroupToProjectInvitation

    Public Event InvitationGroupToProjectDeclined(ByRef arObject As Object)
    Public Event InvitationGroupToProjectAccepted(ByRef arObject As Object)

    Private Sub ButtonDeclineInvitation_Click(sender As Object, e As EventArgs) Handles ButtonDeclineInvitation.Click

        Dim lfrmNotifications As frmNotifications
        lfrmNotifications = CType(Me.Parent.Parent.Parent, frmNotifications)

        Me.Hide()
        Me.Dispose()

        Call lfrmNotifications.CloseIfEmpty()

        RaiseEvent InvitationGroupToProjectDeclined(Me.Tag) 'Tag is the Invitation

    End Sub

    Private Sub ButtonAcceptInvitation_Click(sender As Object, e As EventArgs) Handles ButtonAcceptInvitation.Click

        Dim lfrmNotifications As frmNotifications
        lfrmNotifications = CType(Me.Parent.Parent.Parent, frmNotifications)

        Me.Hide()
        Me.Dispose()

        Call lfrmNotifications.CloseIfEmpty()

        RaiseEvent InvitationGroupToProjectAccepted(Me.Tag) 'Tag is the Invitation

    End Sub

End Class
