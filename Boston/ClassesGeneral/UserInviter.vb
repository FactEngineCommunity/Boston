Public Class UserInviter

    Public Event InvitePressed(ByRef arObject As Object)

    Private Sub ButtonInvite_Click(sender As Object, e As EventArgs) Handles ButtonInvite.Click

        RaiseEvent InvitePressed(Me.Tag)
        Me.Hide()
        Me.Dispose()

    End Sub

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

End Class
