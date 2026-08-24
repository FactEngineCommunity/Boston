Imports System.Linq.Expressions
Imports System.Reflection

Public Class frmCRUDEditUser

    Public zrUser As ClientServer.User

    ''' <summary>
    ''' Used to set a User to Demo User.
    ''' </summary>
    Private miClickCounter As Integer = 0

    Private Sub edit_operator_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            '============================================================================
            'Most Restrictive
            If Not My.Settings.UseClientServer And Not My.Settings.ShowProjectUserMenuItems Then
                Me.TabControl1.Visible = False
            End If

            If Not Me.zrUser.IsSuperuser Then
                Me.TabControl1.TabPages.Remove(Me.TabPageAdvanced)
            End If

            '============================================================================
            'Least Restrictive

            If Me.zrUser.Function.Contains(pcenumFunction.AddRoleToUser) _
                Or Me.zrUser.Function.Contains(pcenumFunction.FullPermission) _
                Or prApplication.User.IsSuperuser Then
                'Leave as is
            Else
                Me.ButtonIncludeRole.Visible = False
                Me.ListBoxAvailableRoles.Visible = False
                Me.LabelPromptAvailableRoles.Visible = False
            End If

            If Me.zrUser.Function.Contains(pcenumFunction.RemoveRoleFromUser) _
                Or Me.zrUser.Function.Contains(pcenumFunction.FullPermission) _
                Or prApplication.User.IsSuperuser Then
                Me.ButtonExcludeRole.Visible = True
                Me.ButtonExcludeRole.Enabled = True
            Else
                Me.ButtonExcludeRole.Visible = False
            End If

            If zrUser.Function.Contains(pcenumFunction.ReadOwnRoles) Then
                Me.TabControl1.Visible = True
            End If

            Me.textbox_operator_name.Text = Trim(Me.zrUser.Username)
            Me.TextBoxFirstName.Text = Trim(Me.zrUser.FirstName)
            Me.TextBoxLastName.Text = Trim(Me.zrUser.LastName)
            Me.checkboxIsActive.Checked = Me.zrUser.IsActive

            '=======================================
            'Check if is Demo User
            If Me.zrUser.Username = "demo" And Me.zrUser.PasswordHash = ClientServer.getHash("demo") Then
                'Is Demo User. Disable chaning Username and Password
                Me.textbox_operator_name.Enabled = False
                Me.TextBoxFirstName.Enabled = False
                Me.TextBoxLastName.Enabled = False
                Me.textbox_password.Enabled = False
                Me.textbox_confirmation_password.Enabled = False
            End If

            'Profie - Personalisation
            If Me.zrUser.Profile IsNot Nothing Then
                Me.TextBoxLogoFileLocation.Text = Me.zrUser.Profile.LogoFileLocation
                Me.CheckBoxShowEnterpriseExplorer.Checked = Me.zrUser.Profile.ShowEnterpriseExplorer
                Me.CheckBoxShowTheBox.Checked = Me.zrUser.Profile.ShowTheBox
                Me.CheckBoxShowVirtualAnalyst.Checked = Me.zrUser.Profile.ShowVirtualAnalyst
            End If

            If Me.zrUser.IsSuperuser Then
                Me.checkboxIsActive.Enabled = False
                Me.LabelPromptIsSuperuser.Visible = True
            ElseIf Not prApplication.User.IsSuperuser Then
                Me.checkboxIsActive.Enabled = False
            End If

            Call loadAvailableRoles(Me.zrUser)
            Call loadIncludedRoles(Me.zrUser)
            Call Me.loadIncludedGroups(Me.zrUser)
            Call Me.loadIncludedProjects(Me.zrUser)

            Call Me.loadPersonalisation(Me.zrUser)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Function check_fields() As Boolean

        check_fields = True

        If Trim(textbox_operator_name.Text) = "" Then
            MsgBox("You must enter an 'Username' before saving.")
            check_fields = False
        End If

        Dim lrUser As ClientServer.User = Nothing
        tableClientServerUser.getUserDetailsByUsername(Trim(textbox_operator_name.Text), lrUser, True)
        If lrUser IsNot Nothing Then
            If lrUser.Id <> Me.zrUser.Id Then
                MsgBox("You must create a unique 'Username' before saving.")
                check_fields = False
            End If
        End If

        If textbox_password.Text <> textbox_confirmation_password.Text Then
            MsgBox("The 'Confirmation Password' does not match the 'Password'. Please reenter the 'Password' and 'Confirm Password' field values.")
            textbox_password.Text = ""
            textbox_confirmation_password.Text = ""
            check_fields = False
        End If

    End Function


    Sub get_fields(ByRef arUser As ClientServer.User)

        arUser.Username = Trim(textbox_operator_name.Text)

        arUser.FirstName = Trim(TextBoxFirstName.Text)
        arUser.LastName = Trim(TextBoxLastName.Text)

        If Trim(textbox_password.Text) <> "" Then
            arUser.PasswordHash = ClientServer.getHash(textbox_password.Text)
        End If

        'prApplication.User.role_id = combobox_role_name.selecteditem.itemdata
        'prApplication.User.is_employee = checkbox_operator_is_employee.Checked

    End Sub

    Private Sub button_okay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_okay.Click

        If check_fields() Then
            Call get_fields(Me.zrUser)
            Call tableClientServerUser.updateUser(Me.zrUser)

#Region "Profile - Personalisation"
            Dim lrDataStore As New DataStore.Store
            Dim whereClause As Expression(Of Func(Of Personalisation.Profile, Boolean)) = Function(t) t.UserId = Me.zrUser.Id

            Dim lrProfile As New Personalisation.Profile

            lrProfile.UserId = Me.zrUser.Id
            lrProfile.LogoFileLocation = Me.TextBoxLogoFileLocation.Text.Trim
            lrProfile.ShowEnterpriseExplorer = Me.CheckBoxShowEnterpriseExplorer.Checked
            lrProfile.ShowVirtualAnalyst = Me.CheckBoxShowVirtualAnalyst.Checked
            lrProfile.ShowTheBox = Me.CheckBoxShowTheBox.Checked

            Try
                lrProfile.DefaultOSMTaskId = Me.ComboBoxDefaultTask.SelectedItem.ItemData
            Catch ex As Exception
                lrProfile.DefaultOSMTaskId = ""
            End Try

            Call lrDataStore.Upsert(lrProfile, whereClause)
#End Region

            Me.Close()
        End If

    End Sub

    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub loadAvailableRoles(ByRef arUser As ClientServer.User)

        Dim lsMessage As String
        Dim lrRole As ClientServer.Role

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerRole"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT RoleId"
            lsSQLQuery &= "                    FROM ClientServerUserRole"
            lsSQLQuery &= "                   WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY RoleName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrRole = New ClientServer.Role
                    lrRole.Id = lREcordset("Id").Value
                    lrRole.Name = lREcordset("RoleName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrRole.Id, lrRole.Name, lrRole)
                    Me.ListBoxAvailableRoles.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub loadIncludedRoles(ByRef arUser As ClientServer.User)

        Dim lsMessage As String
        Dim lrRole As ClientServer.Role

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            For Each lrRole In Me.zrUser.Role
                Dim lrComboboxItem As New tComboboxItem(lrRole.Id, lrRole.Name, lrRole)
                Me.ListBoxIncludedRoles.Items.Add(lrComboboxItem)
            Next

            ''---------------------------------------------
            ''First get EntityTypes with no ParentEntityId
            ''---------------------------------------------
            'lsSQLQuery = " SELECT *"
            'lsSQLQuery &= "  FROM ClientServerRole"
            'lsSQLQuery &= " WHERE Id IN (SELECT RoleId"
            'lsSQLQuery &= "                FROM ClientServerUserRole"
            'lsSQLQuery &= "               WHERE UserId = '" & arUser.Id & "'"
            'lsSQLQuery &= "             )"
            'lsSQLQuery &= " ORDER BY RoleName"

            'lREcordset.Open(lsSQLQuery)

            'If Not lREcordset.EOF Then
            '    While Not lREcordset.EOF
            '        lrRole = New ClientServer.Role
            '        lrRole.Id = lREcordset("Id").Value
            '        lrRole.Name = lREcordset("RoleName").Value

            '        Dim lrComboboxItem As New tComboboxItem(lrRole.Id, lrRole.Name, lrRole)
            '        Me.ListBoxIncludedRoles.Items.Add(lrComboboxItem)

            '        lREcordset.MoveNext()
            '    End While
            'End If

            'lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub loadPersonalisation(ByRef arUser As ClientServer.User)

        Try
            Dim lrUser = arUser

            Dim lrDataStore As New DataStore.Store
            Dim whereClause As Expression(Of Func(Of Personalisation.Profile, Boolean)) = Function(t) t.UserId = lrUser.Id

            Dim larProfile = lrDataStore.Get(Of Personalisation.Profile)(whereClause)

            If larProfile.Any() Then

                Dim lrProfile = larProfile.First

                Me.TextBoxLogoFileLocation.Text = lrProfile.LogoFileLocation

                Dim loTaskWhereClause As Expression(Of Func(Of OSM.Task, Boolean)) = Nothing
                Dim larTask = lrDataStore.Get(Of OSM.Task)

                For Each lrTask In larTask
                    Dim lrComboBoxItem = New tComboboxItem(lrTask.TaskId, lrTask.Name, lrTask)
                    Me.ComboBoxDefaultTask.Items.Add(lrComboBoxItem)

                    If lrTask.TaskId = lrProfile.DefaultOSMTaskId Then
                        Me.ComboBoxDefaultTask.SelectedIndex = Me.ComboBoxDefaultTask.Items.IndexOf(lrComboBoxItem)
                    End If
                Next

                Dim larBostonApplication = lrDataStore.Get(Of BostonApplication.Application)()

                For Each lrBostonApplication In larBostonApplication

                    Dim lrComboboxItem = New tComboboxItem(lrBostonApplication, lrBostonApplication.Name, lrBostonApplication)
                    Dim liNewIndex = Me.ComboBoxDefaultBostonApplication.Items.Add(lrComboboxItem)

                    If lrBostonApplication.BostonApplicationId = lrBostonApplication.BostonApplicationId Then
                        Me.ComboBoxDefaultBostonApplication.SelectedIndex = liNewIndex
                    End If

                Next

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonIncludeRole_Click(sender As Object, e As EventArgs) Handles ButtonIncludeRole.Click

        If Me.ListBoxAvailableRoles.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrRole As ClientServer.Role = Me.ListBoxAvailableRoles.SelectedItem.Tag            
            Dim lrComboboxItem As New tComboboxItem(lrRole.Id, lrRole.Name, lrRole)
            Me.ListBoxIncludedRoles.Items.Add(lrComboboxItem)
            Me.ListBoxAvailableRoles.Items.RemoveAt(Me.ListBoxAvailableRoles.SelectedIndex)

            Call tableClientServerUserRole.addRoleToUser(lrRole, Me.zrUser)
        End If

    End Sub

    Private Sub ButtonExcludeRole_Click(sender As Object, e As EventArgs) Handles ButtonExcludeRole.Click

        If Me.ListBoxIncludedRoles.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrRole As ClientServer.Role = Me.ListBoxIncludedRoles.SelectedItem.Tag
            Dim lrComboboxItem As New tComboboxItem(lrRole.Id, lrRole.Name, lrRole)
            Me.ListBoxAvailableRoles.Items.Add(lrComboboxItem)
            Me.ListBoxIncludedRoles.Items.RemoveAt(Me.ListBoxIncludedRoles.SelectedIndex)

            Call tableClientServerUserRole.removeRoleFromUser(lrRole, Me.zrUser)
        End If

    End Sub

    Private Sub loadIncludedGroups(ByRef arUser As ClientServer.User)

        Dim lsMessage As String
        Dim lrGroup As ClientServer.Group

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id IN (SELECT GroupId"
            lsSQLQuery &= "                FROM ClientServerGroupUser"
            lsSQLQuery &= "               WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "             )"
            lsSQLQuery &= " ORDER BY GroupName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrGroup = New ClientServer.Group
                    lrGroup.Id = lREcordset("Id").Value
                    lrGroup.Name = lREcordset("GroupName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrGroup.Id, lrGroup.Name, lrGroup)
                    Me.ListBoxIncludedInGroups.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub loadIncludedProjects(ByRef arUser As ClientServer.User)

        Dim lsMessage As String
        Dim lrProject As ClientServer.Project

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            Dim lrComboboxItem As tComboboxItem

            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProject"
            lsSQLQuery &= " WHERE Id IN (SELECT ProjectId"
            lsSQLQuery &= "                FROM ClientServerProjectUser"
            lsSQLQuery &= "               WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "             )"
            lsSQLQuery &= " ORDER BY ProjectName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrProject = New ClientServer.Project
                    lrProject.Id = lREcordset("Id").Value
                    lrProject.Name = lREcordset("ProjectName").Value

                    lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
                    Me.ListBoxIncludedInProjects.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lrProject = New ClientServer.Project
            lrProject.Id = "MyPersonalModels"
            lrProject.Name = "My Personal Models"

            lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
            Me.ListBoxIncludedInProjects.Items.Add(lrComboboxItem)

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ListBoxIncludedRoles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxIncludedRoles.SelectedIndexChanged

        If Me.ListBoxIncludedRoles.SelectedIndex < 0 Then Exit Sub

        Dim lrRole As ClientServer.Role

        lrRole = Me.ListBoxIncludedRoles.SelectedItem.Tag

        Me.ListBoxFunctions.Items.Clear()
        For Each lrFunction In lrRole.FunctionT
            Me.ListBoxFunctions.Items.Add(lrFunction.FullText)
        Next

    End Sub

    Private Sub ListBoxIncludedInGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxIncludedInGroups.SelectedIndexChanged

        If Me.ListBoxIncludedInGroups.SelectedIndex = -1 Then Exit Sub

        Me.ListBoxProjectsInGroup.Items.Clear()
        Me.ListBoxWhosInTheGroup.Items.Clear()

        Dim lrGroup As ClientServer.Group = Me.ListBoxIncludedInGroups.SelectedItem.Tag

        Dim larProject As New List(Of ClientServer.Project)
        larProject = tableClientServerProjectGroup.GetProjectsForGroup(lrGroup)

        Dim lrComboboxItem As tComboboxItem
        For Each lrProject In larProject
            lrComboboxItem = New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
            Me.ListBoxProjectsInGroup.Items.Add(lrComboboxItem)
        Next

        Dim larUser = tableClientServerGroupUser.GetUsersForGroup(lrGroup)
        For Each lrUser As ClientServer.User In larUser
            lrComboboxItem = New tComboboxItem(lrUser.Id, lrUser.FullName, lrUser)
            Me.ListBoxWhosInTheGroup.Items.Add(lrComboboxItem)
        Next

    End Sub

    Private Sub ListBoxIncludedInProjects_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxIncludedInProjects.SelectedIndexChanged

        If Me.ListBoxIncludedInProjects.SelectedIndex = -1 Then Exit Sub

        Try
            Dim lrProject As ClientServer.Project
            lrProject = Me.ListBoxIncludedInProjects.SelectedItem.Tag

            '============================================================================
            'Uncheck the Permission CheckBoxes
            Me.CheckBoxUserPermissionFullRights.Checked = False
            Me.CheckBoxUserPermissionCreate.Checked = False
            Me.CheckBoxUserPermissionRead.Checked = False
            Me.CheckBoxUserPermissionAlter.Checked = False
            Me.CheckBoxUserPermissionNoPermission.Checked = False

            '============================================================================
            'Get the Users Permissions for the Project
            Dim larPermission As New List(Of ClientServer.Permission)
            larPermission = prApplication.User.getProjectPermissions(lrProject) 'NB This function gets the 'least restrictive' over Group and User for the Project

            '==========================================================
            'Set the CheckBoxes
            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.FullRights).Count > 0 Then
                Me.CheckBoxUserPermissionFullRights.Checked = True
            End If

            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Create).Count > 0 Then
                Me.CheckBoxUserPermissionCreate.Checked = True
            End If

            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Read).Count > 0 Then
                Me.CheckBoxUserPermissionRead.Checked = True
            End If

            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                Me.CheckBoxUserPermissionAlter.Checked = True
            End If

            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.NoRights).Count > 0 Then
                Me.CheckBoxUserPermissionNoPermission.Checked = True
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmCRUDEditUser_Click(sender As Object, e As EventArgs) Handles Me.Click

        Try
            Me.miClickCounter += 1

            If Me.miClickCounter = 8 Then
                Me.ButtonMakeDemoUser.Visible = True
                Me.ButtonMakeDemoUser.Enabled = True
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonMakeDemoUser_Click(sender As Object, e As EventArgs) Handles ButtonMakeDemoUser.Click

        Try
            Me.TextBoxFirstName.Text = "demo"
            Me.TextBoxLastName.Text = "demo"
            Me.textbox_password.Text = "demo"
            Me.textbox_confirmation_password.Text = "demo"

            Dim liUserFlag = pcenumUserFlag.IsDemoUser

            Call Me.zrUser.Save

        Catch ex As Exception

        End Try

    End Sub
End Class