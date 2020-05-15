Public Class add_operator_frm

    Private Sub add_operator_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)

        Call setup_form()

    End Sub

    Private Sub setup_form()

        Call load_roles()
        Call load_employees()

    End Sub


    Function check_fields() As Boolean


        check_fields = True


        If Trim(textbox_operator_name.Text) = "" Then
            MsgBox("You must enter an 'Operator Name' before saving.")
            check_fields = False
        End If

        If textbox_password.Text <> textbox_confirmation_password.Text Then
            MsgBox("The 'Confirmation Password' does not match the 'Password'. Please reenter the 'Password' and 'Confirm Password' field values.")
            textbox_password.Text = ""
            textbox_confirmation_password.Text = ""
            check_fields = False
        End If

    End Function


    Sub get_fields()


        pr_operator.operator_name = Trim(textbox_operator_name.Text)
        pr_operator.role_id = combobox_role_name.selecteditem.itemdata
        pr_operator.password = textbox_password.Text
        pr_operator.is_employee = checkbox_operator_is_employee.Checked
        If pr_operator.is_employee Then
            pr_operator.employee_id = combobox_employee_name.SelectedItem.itemdata
        Else
            pr_operator.employee_id = 0
        End If


    End Sub


    Sub load_employees()

        Dim ls_sql_query As String
        Dim lrecordset As New ADODB.Recordset

        lrecordset.ActiveConnection = pdb_connection
        lrecordset.CursorType = pc_open_static

        ls_sql_query = "SELECT * FROM employee ORDER BY employee_name"

        lrecordset.Open(ls_sql_query)

        If Not lrecordset.EOF Then
            While Not lrecordset.EOF
                combobox_employee_name.Items.Add(New pr_combobox_item(lrecordset("employee_id").Value, lrecordset("employee_name").Value))
                lrecordset.MoveNext()
            End While
            combobox_employee_name.SelectedIndex = 0
        End If


    End Sub

    Sub load_roles()

        Dim ls_sql_query As String
        Dim lrecordset As New ADODB.Recordset

        lrecordset.ActiveConnection = pdb_connection
        lrecordset.CursorType = pc_open_static


        ls_sql_query = "SELECT * FROM access_role ORDER BY role_name"
        lrecordset.Open(ls_sql_query)  ' Create recordset.

        If Not lrecordset.EOF Then
            lrecordset.MoveFirst()
            While Not lrecordset.EOF
                combobox_role_name.Items.Add(New pr_combobox_item(lrecordset("role_id").Value, Trim(lrecordset("role_name").Value)))
                lrecordset.MoveNext()
            End While
            combobox_role_name.SelectedIndex = 0
        Else
            MsgBox("ERROR: No Roles exists.")
            Exit Sub
        End If

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then
            Call get_fields()
            Call add_operator(pr_operator)
            Me.Close()
        End If

    End Sub

    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click


        Me.Close()

    End Sub

    Private Sub checkbox_operator_is_employee_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkbox_operator_is_employee.CheckedChanged

        If checkbox_operator_is_employee.Checked = True Then
            combobox_employee_name.Enabled = True
        Else
            combobox_employee_name.Enabled = False
        End If

    End Sub

End Class