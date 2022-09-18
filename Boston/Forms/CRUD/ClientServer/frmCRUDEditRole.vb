Imports System.Reflection

Public Class frmCRUDEditRole

    Public zrRole As ClientServer.Role
    Private zbValidationError As Boolean = False

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click


        If Me.checkFields() Then
            If Me.getFields(Me.zrRole) Then
                Call tableClientServerRole.updateRole(Me.zrRole)

                Me.Hide()
                Me.Close()
                Me.Dispose()
            End If
        End If

    End Sub

    Private Function checkFields() As Boolean

        checkFields = True

        Me.zbValidationError = False
        Call Me.ValidateChildren()

        If Trim(Me.TextBoxRoleName.Text) = "" Then
            checkFields = False
        End If

    End Function

    ''' <summary>
    ''' PRECONDITION: A User is logged into Boston.
    ''' </summary>
    ''' <param name="arRole"></param>
    ''' <remarks></remarks>
    Private Function getFields(ByRef arRole As ClientServer.Role) As Boolean

        Try
            If prApplication.User Is Nothing Then
                Throw New Exception("You must log in before creating a Role.")
            End If

            arRole.Name = Trim(Me.TextBoxRoleName.Text)

            Return True

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub frmCRUDAddRole_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Me.setupForm()

    End Sub

    Private Sub setupForm()

        Me.ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink

        Me.TextBoxRoleName.Text = Trim(Me.zrRole.Name)
        Call Me.loadIncludedFunctionsForRole(Me.zrRole)
        Call Me.LoadAvailableFunctionsForRole(Me.zrRole)

        '---------------------------------------------------------------------------------------------
        'Hide/Disable Functionality Depending on the User's Role/Functions/Permissions
        If prApplication.User.Function.Contains(pcenumFunction.FullPermission) Then
            'Is most likely a SuperUser
        Else
            'All other users can't add Functions to a Role or modify the name of a Role.
            Me.ButtonIncludeFunction.Visible = False
            Me.ButtonExcludeFunction.Visible = False
            Me.ListBoxAvailableFunctions.Visible = False
            Me.LabelPromptAvailableFunctions.Visible = False
            Me.TextBoxRoleName.Enabled = False
        End If

        Dim lsMessage As String = ""
        lsMessage = "Please note that a person's 'Permission/s' within a Project is/are set at the Project level."
        lsMessage.AppendString(vbCrLf & vbCrLf)
        lsMessage.AppendString("To manage the Role played by a person on a Project, and their Permission/s, go to the 'Edit Project' form.")
        Me.LabelHelpTips.Text = lsMessage

    End Sub

    Private Sub loadIncludedFunctionsForRole(ByRef arRole As ClientServer.Role)

        Dim lsMessage As String

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerFunction"
            lsSQLQuery &= " WHERE FunctionName IN (SELECT FunctionName FROM ClientServerRoleFunction WHERE RoleId = '" & Me.zrRole.Id & "')"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    Dim liFunction As pcenumFunction

                    liFunction = CType([Enum].Parse(GetType(pcenumFunction), lREcordset("FunctionName").Value), pcenumFunction)

                    Dim lrComboboxItem As New tComboboxItem(liFunction.ToString, lREcordset("FunctionFullText").Value, liFunction)
                    Me.ListBoxIncludedFunctions.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception            
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadAvailableFunctionsForRole(ByRef arRole As ClientServer.Role)

        Dim lsMessage As String

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerFunction"
            lsSQLQuery &= " WHERE FunctionName NOT IN (SELECT FunctionName FROM ClientServerRoleFunction WHERE RoleId = '" & Me.zrRole.Id & "')"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    Dim liFunction As pcenumFunction

                    liFunction = CType([Enum].Parse(GetType(pcenumFunction), lREcordset("FunctionName").Value), pcenumFunction)

                    Dim lrComboboxItem As New tComboboxItem(liFunction.ToString, lREcordset("FunctionFullText").Value, liFunction)
                    Me.ListBoxAvailableFunctions.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonIncludeFunction_Click(sender As Object, e As EventArgs) Handles ButtonIncludeFunction.Click

        If Me.ListBoxAvailableFunctions.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim liFunction As pcenumFunction = Me.ListBoxAvailableFunctions.SelectedItem.Tag
            Dim lrComboboxItem As New tComboboxItem(liFunction.ToString, Me.ListBoxAvailableFunctions.SelectedItem.Text, liFunction)
            Me.ListBoxIncludedFunctions.Items.Add(lrComboboxItem)
            Me.ListBoxAvailableFunctions.Items.RemoveAt(Me.ListBoxAvailableFunctions.SelectedIndex)

            Call tableClientServerRoleFunction.AddFunctionToRole(liFunction, Me.zrRole)
        End If

    End Sub

    Private Sub ButtonExcludeFunction_Click(sender As Object, e As EventArgs) Handles ButtonExcludeFunction.Click

        If Me.ListBoxIncludedFunctions.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim liFunction As pcenumFunction = Me.ListBoxIncludedFunctions.SelectedItem.Tag
            Dim lrComboboxItem As New tComboboxItem(liFunction.ToString, Me.ListBoxIncludedFunctions.SelectedItem.Text, liFunction)
            Me.ListBoxAvailableFunctions.Items.Add(lrComboboxItem)
            Me.ListBoxIncludedFunctions.Items.RemoveAt(Me.ListBoxIncludedFunctions.SelectedIndex)

            Call tableClientServerRoleFunction.removeFunctionFromRole(liFunction, Me.zrRole)
        End If

    End Sub

    Private Sub TextBoxRoleName_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBoxRoleName.KeyUp

        If Trim(Me.TextBoxRoleName.Text) = "" Then
            Me.zbValidationError = True
            Me.ErrorProvider.SetError(Me.TextBoxRoleName, "You must provide a Role Name.")
        Else
            Me.ErrorProvider.Clear()
        End If

    End Sub

    Private Sub TextBoxRoleName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBoxRoleName.Validating

        If Trim(Me.TextBoxRoleName.Text) = "" Then
            Me.zbValidationError = True
            Me.ErrorProvider.SetError(Me.TextBoxRoleName, "You must provide a Role Name.")
        End If

    End Sub

End Class