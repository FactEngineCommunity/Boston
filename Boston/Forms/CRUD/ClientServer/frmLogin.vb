Public Class frmLogin

    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Me.Close()

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        Dim lsUsername As String
        Dim lsPassword As String
        'Dim lfrmEditOperator As New frmEditOperator

        lsUsername = LTrim(RTrim(TextBox_username.Text))
        lsPassword = LTrim(RTrim(TextBox_password.Text))

        If tableClientServerUser.isValidUser(lsUsername, lsPassword) Then

            'g_operator_login_used = True
            'g_user_name = lsUsername
            prApplication.User = New ClientServer.User
            Call tableClientServerUser.getUserDetailsByUsername(lsUsername, prApplication.User)

            Me.Hide()

            If prApplication.User.ResetPassword Then
                MsgBox("Please change your password to something new.")
                'lfrm_edit_operator_frm.ShowDialog()
            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else
            MsgBox("Invalid Username or Password. Try another combination.")
            TextBox_username.Focus()
        End If

    End Sub

    Private Sub login_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '----------------------------
        'Centre the form
        '----------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height / 3) - (Me.Height / 3)

    End Sub

    Private Sub login_frm_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles Me.Validating

        e.Cancel = True

    End Sub

    Private Function check_fields() As Boolean

    End Function


End Class