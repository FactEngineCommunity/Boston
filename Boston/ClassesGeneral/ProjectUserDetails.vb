Public Class ProjectUserDetails

    Public Event RemoveUserFromProject(ByVal arUserId As Object)
    Public Event UserSelected(ByVal arUserId As String)

    Private Sub ButtonRemoveUserFromProject_Click(sender As Object, e As EventArgs) Handles ButtonRemoveUserFromProject.Click

        If Me.ButtonRemoveUserFromProject.Tag = True Then
            If MsgBox("Please confirm that you would like to remove User: " & Me.Tag.FullName & ", from the project.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            Else
                Me.Dispose()

                RaiseEvent RemoveUserFromProject(Me.Tag)
            End If
        Else
            MsgBox("You don't have permission to remove a User from this Project")
        End If

    End Sub

    Private Sub LabelIncludedUserFullName_Click(sender As Object, e As EventArgs) Handles LabelIncludedUserFullName.Click

        Me.LabelIncludedUserFullName.Focus()
        Me.Focus()

    End Sub

    Private Sub LabelIncludedUserFullName_GotFocus(sender As Object, e As EventArgs) Handles LabelIncludedUserFullName.GotFocus

        Me.BackColor = Color.AliceBlue

        RaiseEvent UserSelected(Me.Tag.Id)

    End Sub

    Private Sub LabelIncludedUserFullName_LostFocus(sender As Object, e As EventArgs) Handles LabelIncludedUserFullName.LostFocus

        Me.BackColor = Color.Transparent

    End Sub

    Private Sub CheckedComboBoxRole_GotFocus(sender As Object, e As EventArgs) Handles CheckedComboBoxRole.GotFocus

        Me.BackColor = Color.AliceBlue
        RaiseEvent UserSelected(Me.Tag.Id)

    End Sub

    Private Sub CheckedComboBoxRole_LostFocus(sender As Object, e As EventArgs) Handles CheckedComboBoxRole.LostFocus
        Me.BackColor = Color.Transparent
    End Sub
End Class
