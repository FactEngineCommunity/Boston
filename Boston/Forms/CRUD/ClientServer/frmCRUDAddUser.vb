Imports System.Reflection

Public Class frmCRUDAddUser

    Private zrUser As New ClientServer.User

    Private Sub add_operator_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call setup_form()

    End Sub

    Private Sub setup_form()

        'Call load_roles()
        'Call load_employees()
        Me.textbox_operator_name.Focus()

        Me.ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink

    End Sub

    Function check_fields() As Boolean

        check_fields = True

        If Trim(textbox_operator_name.Text) = "" Then
            MsgBox("You must enter an 'Username' before saving.")
            check_fields = False
        End If

        Dim lrUser As ClientServer.User
        If tableClientServerUser.getUserDetailsByUsername(Trim(textbox_operator_name.Text), lrUser, True) IsNot Nothing Then
            MsgBox("You must create a unique 'Username' before saving.")
            check_fields = False
        End If

        If textbox_password.Text <> textbox_confirmation_password.Text Then
            MsgBox("The 'Confirmation Password' does not match the 'Password'. Please reenter the 'Password' and 'Confirm Password' field values.")
            textbox_password.Text = ""
            textbox_confirmation_password.Text = ""
            check_fields = False
        End If

        If Trim(Me.TextBoxFirstName.Text) = "" Then
            Me.ErrorProvider.SetError(Me.TextBoxFirstName, "First Name is mandatory.")
            check_fields = False
        End If

        If Trim(Me.TextBoxLastName.Text) = "" Then
            Me.ErrorProvider.SetError(Me.TextBoxLastName, "Last Name is mandatory.")
            check_fields = False
        End If

    End Function


    Sub get_fields()

        Me.zrUser.Username = Trim(textbox_operator_name.Text)
        Me.zrUser.PasswordHash = ClientServer.getHash(Trim(textbox_password.Text))

        Me.zrUser.FirstName = Trim(Me.TextBoxFirstName.Text)
        Me.zrUser.LastName = Trim(Me.TextBoxLastName.Text)

        'Me.zrUser.IsActive = checkboxIsActive.Checked
        'prApplication.User.role_id = combobox_role_name.selecteditem.itemdata


    End Sub


    Sub load_employees()

        'Dim ls_sql_query As String
        'Dim lrecordset As New ADODB.Recordset

        'lrecordset.ActiveConnection = pdb_connection
        'lrecordset.CursorType = pc_open_static

        'ls_sql_query = "SELECT * FROM employee ORDER BY employee_name"

        'lrecordset.Open(ls_sql_query)

        'If Not lrecordset.EOF Then
        '    While Not lrecordset.EOF
        '        combobox_employee_name.Items.Add(New pr_combobox_item(lrecordset("employee_id").Value, lrecordset("employee_name").Value))
        '        lrecordset.MoveNext()
        '    End While
        '    combobox_employee_name.SelectedIndex = 0
        'End If


    End Sub

    Sub load_roles()

        'Dim ls_sql_query As String
        'Dim lrecordset As New ADODB.Recordset

        'lrecordset.ActiveConnection = pdb_connection
        'lrecordset.CursorType = pc_open_static


        'ls_sql_query = "SELECT * FROM access_role ORDER BY role_name"
        'lrecordset.Open(ls_sql_query)  ' Create recordset.

        'If Not lrecordset.EOF Then
        '    lrecordset.MoveFirst()
        '    While Not lrecordset.EOF
        '        combobox_role_name.Items.Add(New pr_combobox_item(lrecordset("role_id").Value, Trim(lrecordset("role_name").Value)))
        '        lrecordset.MoveNext()
        '    End While
        '    combobox_role_name.SelectedIndex = 0
        'Else
        '    MsgBox("ERROR: No Roles exists.")
        '    Exit Sub
        'End If

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then
            Call get_fields()

            Call tableClientServerUser.addUser(Me.zrUser)

            '--------------------------------------------------------------------------------
            'Every user at least has the 'Modeller' Role. Every User is allocated the Role of "Modeller" on a Project
            '  and that Role cannot be removed from the User on the Project. See also "ProjectLeader" below:
            Dim lrRole As New ClientServer.Role("Modeller", "Modeller")
            Call tableClientServerUserRole.addRoleToUser(lrRole, Me.zrUser)

            'Every user at least has the 'ProjectLeader' Role, by only some Users (at least one) are allocated that Role on a Project
            lrRole = New ClientServer.Role("ProjectLeader", "Project Leader")
            Call tableClientServerUserRole.addRoleToUser(lrRole, Me.zrUser)


            Me.Close()
        End If

    End Sub

    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click


        Me.Close()

    End Sub

    Private Sub TextBoxFirstName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxFirstName.TextChanged

        Try
            If (Me.TextBoxFirstName.Text) = "" Then
                Me.ErrorProvider.SetError(Me.TextBoxFirstName, "First Name is mandatory.")
            Else
                Me.ErrorProvider.Clear()
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxLastName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxLastName.TextChanged

        Try
            If (Me.TextBoxLastName.Text) = "" Then
                Me.ErrorProvider.SetError(Me.TextBoxLastName, "Last Name is mandatory.")
            Else
                Me.ErrorProvider.Clear()
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class