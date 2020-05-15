Public Class add_role_frm

    Private Sub add_role_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)


    End Sub


    Function check_fields()

        check_fields = True

        If textbox_role_name.text = "" Then
            MsgBox("You must enter a new 'Role Name'")
            check_fields = False
        End If

        If does_role_name_already_exist(Trim(textbox_role_name.text)) Then
            MsgBox("The 'Role Name' that you entered already exists. Please create a new 'Role Name' before proceediing.")
            check_fields = False
        End If

    End Function


    Sub get_fields()

        pr_role.role_id = get_next_role_id()
        pr_role.role_name = Trim(textbox_role_name.text)

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click


        If check_fields() Then
            Call get_fields()
            Call add_role(pr_role)
            Call create_role_facility_records_for_role(pr_role.role_id)
            Me.Close()
        End If

    End Sub

    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Me.Close()

    End Sub

End Class