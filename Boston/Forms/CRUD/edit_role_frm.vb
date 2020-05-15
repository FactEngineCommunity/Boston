Public Class edit_role_frm

    Private Sub edit_role_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)

        Call setup_form()

    End Sub

    Private Sub setup_form()

        textbox_role_name.Text = pr_role.role_name

    End Sub


    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Me.Close()

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then
            Call get_fields()
            Call update_role(pr_role.role_id, pr_role.role_name)
            Me.Close()
        End If
    End Sub

    Private Function check_fields() As Boolean

        check_fields = False

        If textbox_role_name.Text <> "" Then
            check_fields = True
        Else
            MsgBox("Please enter a 'Role Name' before saving.")
        End If

    End Function

    Private Sub get_fields()


        pr_role.role_name = Trim(textbox_role_name.Text)



    End Sub

End Class