Imports System.Reflection

Public Class frmCRUDEditProject

    Public zrProject As New ClientServer.Project
    Public zbUpdatingPermissions As Boolean = False
    Private zrUserInviteList As New List(Of ClientServer.User)
    Private zrGroupInviteList As New List(Of ClientServer.Group)
    Private zbRecheckingPUDCheckbox As Boolean = False
    Private zarUserFunctionsThisProject As New List(Of pcenumFunction)

    Private Sub frmCRUDEditProject_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Dim larRole As New List(Of ClientServer.Role)
            larRole = tableClientServerProjectUserRole.GetRolesForUserOnProject(prApplication.User, _
                                                                                Me.zrProject,
                                                                                True)


            Dim larFunction = From Role In larRole _
                              From [Function] In Role.Function
                              Select [Function]

            Me.zarUserFunctionsThisProject = larFunction.ToList

            Call Me.SetupForm()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.checkFields() Then
            Call Me.getFields(Me.zrProject)
            Call tableClientServerProject.UpdateProject(Me.zrProject)

            Me.Hide()
            Me.Close()
            Me.Dispose()
        End If

    End Sub

    Private Function checkFields() As Boolean

        checkFields = True

        If Trim(Me.TextBoxProjectName.Text) = "" Then
            checkFields = False
        ElseIf prApplication.User Is Nothing Then
            '----------------------------------------------------------------
            'Need the User to populate arProject.CreatedByUser
            checkFields = False
        End If

    End Function

    Private Sub getFields(ByRef arProject As ClientServer.Project)

        arProject.Name = Trim(Me.TextBoxProjectName.Text)
        arProject.CreatedByUserId = prApplication.User.Id

    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub SetupForm()

        Try
            'Watermarks
            Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
            Me.TextBoxUserName.ForeColor = Color.LightGray
            Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag
            Me.TextBoxGroupInvite.ForeColor = Color.LightGray

            Me.TextBoxProjectName.Text = Me.zrProject.Name

            Call Me.LoadIncludedUsersForProject(Me.zrProject)
            Call Me.LoadAvailableUsersForProject(Me.zrProject)

            Call Me.LoadIncludedGroupsForProject(Me.zrProject)
            Call Me.LoadAvailableGroupsForProject(Me.zrProject)

            Call Me.LoadPermissionsUsers(Me.zrProject)
            Call Me.LoadPermissionsGroups(Me.zrProject)

            Call Me.loadIncludedNamespaces(Me.zrProject)

            Me.ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink

            '===================================================================
            'NB Invite User = Panel1, which is hidden for Superusers, but shown for all other Users (User Role).

            '-----------------------------------------------------------------------------
            'Setup the HelpTip labels.
            Dim lsMessage As String = ""
            lsMessage = "Please note that a person's overall Role/Permission combination affects what a person can and cannot do within a Project."
            lsMessage.AppendString(vbCrLf)
            lsMessage.AppendString("Permissions are 'Least Restrictive' between the User and Group combination enjoyed by an individual person.")
            lsMessage.AppendString(vbCrLf)
            lsMessage.AppendString("Go to the 'Edit Role' form to see which Functions a particular Role affords.")

            Me.LabelHelpTipsUser.Text = lsMessage
            Me.LabelHelpTipsGroup.Text = lsMessage

            '---------------------------------------------------------------------------------------------
            'Hide/Disable Functionality Depending on the User's Role/Functions/Permissions
            Call Me.hideDisableFunctionalityDependingOnTheUsersRoleFunctionsPermission()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub hideDisableFunctionalityDependingOnTheUsersRoleFunctionsPermission()

        If Me.zrProject.CreatedByUser.IsSuperuser Then

        Else

        End If

        If prApplication.User.IsSuperuser Then
            'CreatedBy User
            Me.LabelCreatedByUser.Text = Me.zrProject.CreatedByUser.Username

            'User
            Me.ListBoxAvailableUsers.BringToFront()
            Me.ListBoxAvailableUsers.Visible = True
            Me.ButtonIncludeUser.Visible = True
            'Me.ButtonExcludeUser.Visible = True

            'Group
            Me.ListBoxAvailableGroups.BringToFront()
            Me.ListBoxAvailableGroups.Visible = True
            Me.ButtonIncludeGroup.Visible = True
            Me.ButtonExcludeGroup.Visible = True


            Me.PanelInviteUser.SendToBack()
            Me.PanelInviteGroup.SendToBack()
        ElseIf Me.zarUserFunctionsThisProject.Contains(pcenumFunction.AlterProject) Then
            'CreatedBy User
            Me.LabelCreatedByUser.Text = Me.zrProject.CreatedByUser.FirstName & " " & Me.zrProject.CreatedByUser.LastName
            'Leave everything else as is, because the User has the appropriate Funciton to Edit this Project.

            'Users
            Me.ListBoxAvailableUsers.SendToBack()
            Me.ListBoxAvailableUsers.Visible = False
            Me.PanelInviteUser.BringToFront()
            Me.FlexibleListBox.BringToFront()

            Me.LabelPromptAvailableUsers.Text = "Select someone to join this Project."

            'Groups
            Me.ButtonIncludeGroup.Visible = False
            Me.ButtonIncludeGroup.Visible = False

            Me.ListBoxAvailableGroups.SendToBack()
            Me.ListBoxAvailableGroups.Visible = False
            Me.PanelInviteGroup.BringToFront()
            Me.FlexibleListBoxGroupInvite.BringToFront()

            Me.LabelPromptAvailableGroups.Text = "Select a Group to join this Project."
        Else
            '============================================================================================================
            'General User

            'CreatedBy User
            Me.LabelCreatedByUser.Text = Me.zrProject.CreatedByUser.FirstName & " " & Me.zrProject.CreatedByUser.LastName

            'User
            Me.ButtonIncludeUser.Visible = False

            Me.ListBoxAvailableUsers.Visible = False
            Me.PanelInviteUser.Visible = False
            Me.LabelPromptAvailableUsers.Visible = False

            Me.CheckBoxUserPermissionCreate.Enabled = False
            Me.CheckBoxUserPermissionRead.Enabled = False
            Me.CheckBoxUserPermissionAlter.Enabled = False
            Me.CheckBoxUserPermissionFullRights.Enabled = False
            Me.CheckBoxUserPermissionNoPermission.Enabled = False

            Me.ComboBoxUserPermissions.ItemSelect(1, prApplication.User.FullName, True)
            Call Me.showPermissionsForUser(prApplication.User)
            Me.ComboBoxUserPermissions.Enabled = False

            'Group
            Me.ButtonIncludeGroup.Visible = False
            Me.ButtonExcludeGroup.Visible = False

            Me.ListBoxAvailableGroups.SendToBack()
            Me.ListBoxAvailableGroups.Visible = False
            Me.PanelInviteGroup.Visible = False
            'Me.FlexibleListBoxGroupInvite.BringToFront()
            Me.LabelPromptAvailableGroups.Visible = False

            Me.CheckBoxGroupPermissionCreate.Enabled = False
            Me.CheckBoxGroupPermissionRead.Enabled = False
            Me.CheckBoxGroupPermissionAlter.Enabled = False
            Me.CheckBoxGroupPermissionFullRights.Enabled = False
            Me.CheckBoxGroupPermissionNoPermission.Enabled = False
        End If

    End Sub


    Private Sub LoadIncludedUsersForProject(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrUser As ClientServer.User

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id IN (SELECT UserId"
            lsSQLQuery &= "                    FROM ClientServerProjectUser"
            lsSQLQuery &= "                   WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY FirstName"

            lREcordset.Open(lsSQLQuery)

            Dim liInd As Integer = 0
            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrUser = New ClientServer.User
                    lrUser.Id = lREcordset("Id").Value
                    lrUser.FirstName = lREcordset("FirstName").Value
                    lrUser.LastName = lREcordset("LastName").Value

                    Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
                    Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
                    'Me.ListBoxIncludedUsers.Items.Add(lrComboboxItem)

                    Dim larRole As New List(Of ClientServer.Role)
                    larRole = tableClientServerProjectUserRole.GetRolesForUserOnProject(lrUser, Me.zrProject)

                    '======================================================================================
                    'FlexibleListBox
                    Dim loProjectUserDetails As New ProjectUserDetails
                    loProjectUserDetails.LabelIncludedUserFullName.Text = lsName

                    Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser)

                    For Each lrRole In lrUser.Role
                        loProjectUserDetails.CheckedComboBoxRole.Items.Add(lrRole.Name)
                        loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems(lrRole.Name).Checked = larRole.Contains(lrRole)
                        If Me.zarUserFunctionsThisProject.Contains(pcenumFunction.AlterProject) _
                            Or prApplication.User.IsSuperuser Then
                            'Can modify a Users Role on the Project.
                        Else
                            loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems(lrRole.Name).Enabled = False
                        End If
                        loProjectUserDetails.CheckedComboBoxRole.Tag = lrUser.Id
                    Next

                    loProjectUserDetails.Tag = lrUser

                    AddHandler loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems.CheckBoxCheckedChanged, AddressOf Me.HandlerTest

                    If Me.zarUserFunctionsThisProject.Contains(pcenumFunction.AlterProject) _
                        Or prApplication.User.IsSuperuser Then
                        AddHandler loProjectUserDetails.RemoveUserFromProject, AddressOf Me.HandleRemoveUserFromProject
                        AddHandler loProjectUserDetails.UserSelected, AddressOf Me.HandleUserSelected

                        loProjectUserDetails.ButtonRemoveUserFromProject.Tag = True
                    Else
                        loProjectUserDetails.ButtonRemoveUserFromProject.Tag = False
                    End If

                    AddHandler loProjectUserDetails.CheckedComboBoxRole.LostFocus, AddressOf Me.HandlePUDLostFocus

                    Me.FlexibleListBoxIncludedUser.Add(loProjectUserDetails)
                    Call loProjectUserDetails.CheckedComboBoxRole.refreshText()
                    '======================================================================================

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub HandleRemoveUserFromProject(ByVal asUser As Object)

        Dim lrUser As ClientServer.User
        lrUser = CType(asUser, ClientServer.User)

        If lrUser.Id = Me.zrProject.CreatedByUser.Id Then
            MsgBox("You cannot remove the User that created the Project from the Project.")
            Exit Sub
        End If

        Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
        Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
        Me.ListBoxAvailableUsers.Items.Add(lrComboboxItem)

        Call tableClientServerProjectUser.removeUserFromProject(lrUser, Me.zrProject)
        Call tableClientServerProjectUserPermission.removeAllPermissionsForUserForProject(lrUser, Me.zrProject)
        Call tableClientServerProjectUserRole.removeAllUserRolesFromProject(lrUser, Me.zrProject)

        Call Me.LoadPermissionsUsers(Me.zrProject)


    End Sub

    Private Sub HandlerTest(sender As Object, e As System.EventArgs)

        Dim lsRoleName As String = ""
        Dim lbChecked As Boolean = False
        Dim lrUser As New ClientServer.User
        Dim lrRole As New ClientServer.Role

        Dim lrComboboxItem As PresentationControls.CheckBoxComboBoxItem

        lrComboboxItem = CType(sender, PresentationControls.CheckBoxComboBoxItem)

        lsRoleName = lrComboboxItem.Text
        lbChecked = lrComboboxItem.Checked
        lrUser.Id = CType(e, PresentationControls.MyEventArgs).UserId

        Call tableClientServerRole.getRoleDetailsByName(lsRoleName, lrRole)

        If lbChecked And Not Me.zbRecheckingPUDCheckbox Then
            Call tableClientServerProjectUserRole.AddUserRoleToProject(lrUser, lrRole, Me.zrProject)
        ElseIf Me.zbRecheckingPUDCheckbox = False Then
            If lrRole.Id = "Modeller" Then
                MsgBox("Every person on a Project must at least play the Role of 'Modeller'.")
                Me.zbRecheckingPUDCheckbox = True
                lrComboboxItem.Checked = True
                Me.zbRecheckingPUDCheckbox = False
            Else
                Call tableClientServerProjectUserRole.removeUserRoleFromProject(lrUser, lrRole, Me.zrProject)
            End If
        End If

    End Sub

    Private Sub LoadAvailableUsersForProject(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrUser As ClientServer.User

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT UserId"
            lsSQLQuery &= "                    FROM ClientServerProjectUser"
            lsSQLQuery &= "                   WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY FirstName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrUser = New ClientServer.User
                    lrUser.Id = lREcordset("Id").Value
                    lrUser.FirstName = lREcordset("FirstName").Value
                    lrUser.LastName = lREcordset("LastName").Value
                    lrUser.Username = lREcordset("Username").Value
                    lrUser.IsSuperuser = lREcordset("IsSuperuser").Value

                    If lrUser.IsSuperuser And lrUser.Username = "admin" Then
                        'Do nothing
                    Else
                        Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
                        Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
                        Me.ListBoxAvailableUsers.Items.Add(lrComboboxItem)
                    End If

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonIncludeUser_Click(sender As Object, e As EventArgs) Handles ButtonIncludeUser.Click

        If Me.ListBoxAvailableUsers.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrUser As ClientServer.User = Me.ListBoxAvailableUsers.SelectedItem.Tag
            Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
            Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)

            Me.ListBoxAvailableUsers.Items.RemoveAt(Me.ListBoxAvailableUsers.SelectedIndex)

            '======================================================================================
            'FlexibleListBox
            Dim loProjectUserDetails As New ProjectUserDetails
            loProjectUserDetails.LabelIncludedUserFullName.Text = lsName
            loProjectUserDetails.CheckedComboBoxRole.Tag = lrUser.Id
            loProjectUserDetails.Tag = lrUser
            loProjectUserDetails.CheckedComboBoxRole.Tag = lrUser.Id

            Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser)

            For Each lrRole In lrUser.Role
                loProjectUserDetails.CheckedComboBoxRole.Items.Add(lrRole.Name)
                Call tableClientServerProjectUserRole.AddUserRoleToProject(lrUser, lrRole, Me.zrProject)
                If lrRole.Id = "Modeller" Then
                    loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems(lrRole.Name).Checked = True                    
                Else
                    loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems(lrRole.Name).Checked = False
                End If
            Next

            AddHandler loProjectUserDetails.CheckedComboBoxRole.CheckBoxItems.CheckBoxCheckedChanged, AddressOf Me.HandlerTest
            AddHandler loProjectUserDetails.RemoveUserFromProject, AddressOf Me.HandleRemoveUserFromProject
            AddHandler loProjectUserDetails.UserSelected, AddressOf Me.HandleUserSelected
            AddHandler loProjectUserDetails.CheckedComboBoxRole.LostFocus, AddressOf Me.HandlePUDLostFocus
            loProjectUserDetails.ButtonRemoveUserFromProject.Tag = True 'So that the User can remove the User if they want to.

            Me.FlexibleListBoxIncludedUser.Add(loProjectUserDetails)

            '==============================================================================
            'Reload the UserPremissions Combobox
            Dim dt As DataTable
            Dim dr As DataRow
            dr = ComboBoxUserPermissions.SourceDataTable.NewRow

            dr("Name") = lsName
            dr("Username") = lrUser.Username

            dt = Me.ComboBoxUserPermissions.SourceDataTable
            dt.Rows.Add(dr)

            ComboBoxUserPermissions.SelectedIndex = -1
            ComboBoxUserPermissions.Items.Clear()
            ComboBoxUserPermissions.LoadingType = MTGCComboBox.CaricamentoCombo.DataTable
            ComboBoxUserPermissions.SourceDataTable = dt
            '------------------------------

            Call tableClientServerProjectUser.AddUserToProject(lrUser, Me.zrProject)

            Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission
            lrProjectUserPermission.Project = Me.zrProject
            lrProjectUserPermission.User = lrUser
            lrProjectUserPermission.Permission = pcenumPermission.FullRights
            Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            lrProjectUserPermission.Permission = pcenumPermission.Create
            Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            lrProjectUserPermission.Permission = pcenumPermission.Read
            Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            lrProjectUserPermission.Permission = pcenumPermission.Alter
            Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)

        End If

    End Sub

    Private Sub HandlePUDLostFocus(sender As Object, e As System.EventArgs)

        Dim loControl As PresentationControls.CheckBoxComboBox = CType(sender, PresentationControls.CheckBoxComboBox)
        Call loControl.refreshText()

    End Sub

    Private Sub LoadPermissionsUsers(ByRef arProject As ClientServer.Project)

        Dim lsSQLQuery As String
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT Username & ' ' & LastName AS Name, *"
        lsSQLQuery &= " FROM ClientServerUser"
        lsSQLQuery &= " WHERE Id IN (SELECT UserId FROM ClientServerProjectUser WHERE ProjectId = '" & arProject.Id & "')"

        lREcordset.Open(lsSQLQuery)

        Dim lasFields() As String = Viev.Strings.RemoveWhiteSpace("Name, Username").Split(",")

        '=================================================================================================================
        Dim dtLoading As New DataTable("UsStates")

        For Each lsField In lasFields
            dtLoading.Columns.Add(lsField, System.Type.GetType("System.String"))
        Next

        While Not lREcordset.EOF
            Dim dr As DataRow
            dr = dtLoading.NewRow

            For Each lsField In lasFields
                dr(lsField) = lREcordset(lsField).Value
            Next

            dtLoading.Rows.Add(dr)
            lREcordset.MoveNext()
        End While

        lREcordset.Close()

        ComboBoxUserPermissions.ColumnNum = lasFields.Count
        ComboBoxUserPermissions.ColumnWidth = "150;100"
        ComboBoxUserPermissions.SelectedIndex = -1
        ComboBoxUserPermissions.Items.Clear()
        ComboBoxUserPermissions.LoadingType = MTGCComboBox.CaricamentoCombo.DataTable
        ComboBoxUserPermissions.SourceDataString = New String(1) {"Name", "Username"}
        ComboBoxUserPermissions.SourceDataTable = dtLoading
        '=================================================================================================================

        If ComboBoxUserPermissions.Items.Count > 0 Then ComboBoxUserPermissions.SelectedIndex = 0

    End Sub

    Private Sub LoadPermissionsGroups(ByRef arProject As ClientServer.Project)

        Dim lsSQLQuery As String
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM ClientServerGroup"
        lsSQLQuery &= " WHERE Id IN (SELECT GroupId FROM ClientServerProjectGroup WHERE ProjectId = '" & arProject.Id & "')"

        lREcordset.Open(lsSQLQuery)

        Dim lrComboboxItem As tComboboxItem
        Dim lrGroup As ClientServer.Group

        While Not lREcordset.EOF

            lrGroup = New ClientServer.Group

            lrGroup.Id = lREcordset("Id").Value
            lrGroup.Name = lREcordset("GroupName").Value

            lrComboboxItem = New tComboboxItem(lREcordset("Id").Value, lREcordset("GroupName").Value, lrGroup)
            Me.ComboBoxGroupPermissions.Items.Add(lrComboboxItem)

            lREcordset.MoveNext()
        End While

        lREcordset.Close()

        '=================================================================================================================

        If ComboBoxGroupPermissions.Items.Count > 0 Then ComboBoxGroupPermissions.SelectedIndex = 0

    End Sub

    Private Sub ComboBoxUserPermissions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxUserPermissions.SelectedIndexChanged

        Dim lrUser As New ClientServer.User

        If Me.ComboBoxUserPermissions.SelectedIndex = -1 Then Exit Sub

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)

        Me.CheckBoxUserPermissionCreate.Enabled = Not lrUser.IsSuperuser
        Me.CheckBoxUserPermissionRead.Enabled = Not lrUser.IsSuperuser
        Me.CheckBoxUserPermissionAlter.Enabled = Not lrUser.IsSuperuser
        Me.CheckBoxUserPermissionFullRights.Enabled = Not lrUser.IsSuperuser
        Me.CheckBoxUserPermissionNoPermission.Enabled = Not lrUser.IsSuperuser

        Call Me.showPermissionsForUser(lrUser)

    End Sub

    Private Sub showPermissionsForUser(ByRef arUser As ClientServer.User)

        Me.zbUpdatingPermissions = True
        Me.CheckBoxUserPermissionFullRights.Checked = False
        Me.CheckBoxUserPermissionNoPermission.Checked = False
        Me.CheckBoxUserPermissionCreate.Checked = False
        Me.CheckBoxUserPermissionRead.Checked = False
        Me.CheckBoxUserPermissionAlter.Checked = False
        Me.zbUpdatingPermissions = False

        Me.zbUpdatingPermissions = True
        Call Me.LoadPermissionsForProjectUser(Me.zrProject, arUser)
        Me.zbUpdatingPermissions = False

    End Sub

    Private Sub LoadPermissionsForProjectGroup(ByRef arProject As ClientServer.Project, ByRef arGroup As ClientServer.Group)

        Dim larProjectGroupPermissions As New List(Of ClientServer.ProjectGroupPermission)

        Call tableClientServerProjectGroupPermission.GetPermissionsForProjectGroup(arProject, arGroup, larProjectGroupPermissions)

        For Each lrProjectGroupPermission In larProjectGroupPermissions

            Select Case lrProjectGroupPermission.Permission
                Case Is = pcenumPermission.FullRights
                    Me.CheckBoxGroupPermissionFullRights.Checked = True
                Case Is = pcenumPermission.NoRights
                    Me.CheckBoxGroupPermissionNoPermission.Checked = True
                Case Is = pcenumPermission.Create
                    Me.CheckBoxGroupPermissionCreate.Checked = True
                Case Is = pcenumPermission.Read
                    Me.CheckBoxGroupPermissionRead.Checked = True
                Case Is = pcenumPermission.Alter
                    Me.CheckBoxGroupPermissionAlter.Checked = True
            End Select
        Next

    End Sub

    Private Sub LoadPermissionsForProjectUser(ByRef arProject As ClientServer.Project, ByRef arUser As ClientServer.User)

        Dim larProjectUserPermissions As New List(Of ClientServer.ProjectUserPermission)

        Call tableClientServerProjectUserPermission.GetPermissionsForProjectUser(arProject, arUser, larProjectUserPermissions)

        For Each lrProjectUserPermission In larProjectUserPermissions

            Select Case lrProjectUserPermission.Permission
                Case Is = pcenumPermission.FullRights
                    Me.CheckBoxUserPermissionFullRights.Checked = True
                Case Is = pcenumPermission.NoRights
                    Me.CheckBoxUserPermissionNoPermission.Checked = True
                Case Is = pcenumPermission.Create
                    Me.CheckBoxUserPermissionCreate.Checked = True
                Case Is = pcenumPermission.Read
                    Me.CheckBoxUserPermissionRead.Checked = True
                Case Is = pcenumPermission.Alter
                    Me.CheckBoxUserPermissionAlter.Checked = True
            End Select
        Next

    End Sub

    Private Sub CheckBoxUserPermissionFullRights_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUserPermissionFullRights.CheckedChanged

        Dim lrUser As New ClientServer.User
        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

        If Me.ComboBoxUserPermissions.SelectedIndex = -1 Then Exit Sub

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)
        lrProjectUserPermission.Project = Me.zrProject
        lrProjectUserPermission.User = lrUser
        lrProjectUserPermission.Permission = pcenumPermission.FullRights

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxUserPermissionFullRights.Checked Then
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)

                Call Me.setUserPermission(lrUser, pcenumPermission.Create)
                Call Me.setUserPermission(lrUser, pcenumPermission.Read)
                Call Me.setUserPermission(lrUser, pcenumPermission.Alter)
                Call Me.removeUserPermission(lrUser, pcenumPermission.NoRights)
            Else
                Call tableClientServerProjectUserPermission.DeleteProjectUserPermission(lrProjectUserPermission)
            End If
        End If

    End Sub

    Private Sub CheckBoxUserPermissionNoPermission_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUserPermissionNoPermission.CheckedChanged

        Dim lrUser As New ClientServer.User
        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)
        lrProjectUserPermission.Project = Me.zrProject
        lrProjectUserPermission.User = lrUser
        lrProjectUserPermission.Permission = pcenumPermission.NoRights

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxUserPermissionNoPermission.Checked Then
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)

                Call Me.removeUserPermission(lrUser, pcenumPermission.FullRights)
                Call Me.removeUserPermission(lrUser, pcenumPermission.Create)
                Call Me.removeUserPermission(lrUser, pcenumPermission.Read)
                Call Me.removeUserPermission(lrUser, pcenumPermission.Alter)
            Else
                Call tableClientServerProjectUserPermission.DeleteProjectUserPermission(lrProjectUserPermission)
            End If
        End If

    End Sub

    Private Sub CheckBoxUserPermissionCreate_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUserPermissionCreate.CheckedChanged

        Dim lrUser As New ClientServer.User
        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)
        lrProjectUserPermission.Project = Me.zrProject
        lrProjectUserPermission.User = lrUser
        lrProjectUserPermission.Permission = pcenumPermission.Create

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxUserPermissionCreate.Checked Then
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            Else
                Call tableClientServerProjectUserPermission.DeleteProjectUserPermission(lrProjectUserPermission)

                Call Me.removeUserPermission(lrUser, pcenumPermission.FullRights)
            End If
        End If
    End Sub

    Private Sub CheckBoxUserPermissionRead_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUserPermissionRead.CheckedChanged

        Dim lrUser As New ClientServer.User
        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)
        lrProjectUserPermission.Project = Me.zrProject
        lrProjectUserPermission.User = lrUser
        lrProjectUserPermission.Permission = pcenumPermission.Read

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxUserPermissionRead.Checked Then
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            Else
                Call tableClientServerProjectUserPermission.DeleteProjectUserPermission(lrProjectUserPermission)

                Call Me.removeUserPermission(lrUser, pcenumPermission.FullRights)
            End If
        End If
    End Sub

    Private Sub CheckBoxUserPermissionAlter_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxUserPermissionAlter.CheckedChanged

        Dim lrUser As New ClientServer.User
        Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

        Call tableClientServerUser.getUserDetailsByUsername(Me.ComboBoxUserPermissions.SelectedItem.Col2, lrUser)
        lrProjectUserPermission.Project = Me.zrProject
        lrProjectUserPermission.User = lrUser
        lrProjectUserPermission.Permission = pcenumPermission.Alter

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxUserPermissionAlter.Checked Then
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
            Else
                Call tableClientServerProjectUserPermission.DeleteProjectUserPermission(lrProjectUserPermission)

                Call Me.removeUserPermission(lrUser, pcenumPermission.FullRights)
            End If
        End If
    End Sub

    Private Sub LoadAvailableGroupsForProject(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrGroup As ClientServer.Group

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT GroupId"
            lsSQLQuery &= "                    FROM ClientServerProjectGroup"
            lsSQLQuery &= "                   WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY GroupName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrGroup = New ClientServer.Group
                    lrGroup.Id = lREcordset("Id").Value
                    lrGroup.Name = lREcordset("GroupName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrGroup.Id, lrGroup.Name, lrGroup)
                    Me.ListBoxAvailableGroups.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub LoadIncludedGroupsForProject(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrGroup As ClientServer.Group

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id IN (SELECT GroupId"
            lsSQLQuery &= "                FROM ClientServerProjectGroup"
            lsSQLQuery &= "               WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "             )"
            lsSQLQuery &= " ORDER BY GroupName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrGroup = New ClientServer.Group
                    lrGroup.Id = lREcordset("Id").Value
                    lrGroup.Name = lREcordset("GroupName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrGroup.Id, lrGroup.Name, lrGroup)
                    Me.ListBoxIncludedGroups.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonIncludeGroup_Click(sender As Object, e As EventArgs) Handles ButtonIncludeGroup.Click

        Try
            If Me.ListBoxAvailableGroups.SelectedIndex = -1 Then
                Exit Sub
            Else
                Dim lrGroup As ClientServer.Group = Me.ListBoxAvailableGroups.SelectedItem.Tag

                Dim lrComboboxItem As New tComboboxItem(lrGroup.Id, lrGroup.Name, lrGroup)
                Me.ListBoxIncludedGroups.Items.Add(lrComboboxItem)
                Me.ListBoxAvailableGroups.Items.RemoveAt(Me.ListBoxAvailableGroups.SelectedIndex)

                Me.ComboBoxGroupPermissions.Items.Add(lrComboboxItem)

                pdbConnection.BeginTrans()
                Call tableClientServerProjectGroup.AddGroupToProject(lrGroup, Me.zrProject)
                Call lrGroup.IncludeAllUsersInProject(Me.zrProject)
                pdbConnection.CommitTrans()
            End If
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonExcludeGroup_Click(sender As Object, e As EventArgs) Handles ButtonExcludeGroup.Click

        If Me.ListBoxIncludedGroups.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrGroup As ClientServer.Group = Me.ListBoxIncludedGroups.SelectedItem.Tag

            Dim lrComboboxItem As New tComboboxItem(lrGroup.Id, lrGroup.Name, lrGroup)
            Me.ListBoxAvailableGroups.Items.Add(lrComboboxItem)
            Me.ListBoxIncludedGroups.Items.RemoveAt(Me.ListBoxIncludedGroups.SelectedIndex)

            Call tableClientServerProjectGroup.removeGroupFromProject(lrGroup, Me.zrProject)
        End If

    End Sub

    Private Sub TextBoxProjectName_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBoxProjectName.KeyUp

        If (Me.TextBoxProjectName.Text) = "" Then
            Me.ErrorProvider.SetError(Me.TextBoxProjectName, "Name is mandatory.")
        Else
            Me.ErrorProvider.Clear()
        End If

    End Sub

    Private Sub setUserPermission(ByRef arUser As ClientServer.User, aiPermission As pcenumPermission)

        Try
            Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

            lrProjectUserPermission.Project = Me.zrProject
            lrProjectUserPermission.User = arUser
            lrProjectUserPermission.Permission = aiPermission

            Select Case aiPermission
                Case Is = pcenumPermission.Create
                    If Not Me.CheckBoxUserPermissionCreate.Checked Then
                        Me.CheckBoxUserPermissionCreate.Checked = True 'Triggers Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.Read
                    If Not Me.CheckBoxUserPermissionRead.Checked Then
                        Me.CheckBoxUserPermissionRead.Checked = True 'Triggers Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.Alter
                    If Not Me.CheckBoxUserPermissionAlter.Checked Then
                        Me.CheckBoxUserPermissionAlter.Checked = True 'Triggers Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Else
                    Throw New Exception("Permission: " & aiPermission.ToString & " not allowed for this method.")
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub removeUserPermission(ByRef arUser As ClientServer.User, ByVal aiPermission As pcenumPermission)

        Try
            Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission

            lrProjectUserPermission.Project = Me.zrProject
            lrProjectUserPermission.User = arUser
            lrProjectUserPermission.Permission = aiPermission

            Select Case aiPermission
                Case Is = pcenumPermission.FullRights
                    If Me.CheckBoxUserPermissionFullRights.Checked Then
                        Me.CheckBoxUserPermissionFullRights.Checked = False 'Triggers Call tableClientServerProjectUserPermission.RemoveProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.Create
                    If Me.CheckBoxUserPermissionCreate.Checked Then
                        Me.CheckBoxUserPermissionCreate.Checked = False 'Triggers Call tableClientServerProjectUserPermission.RemoveProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.Read
                    If Me.CheckBoxUserPermissionRead.Checked Then
                        Me.CheckBoxUserPermissionRead.Checked = False 'Triggers Call tableClientServerProjectUserPermission.RemoveProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.Alter
                    If Me.CheckBoxUserPermissionAlter.Checked Then
                        Me.CheckBoxUserPermissionAlter.Checked = False 'Triggers Call tableClientServerProjectUserPermission.RemoveProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Is = pcenumPermission.NoRights
                    If Me.CheckBoxUserPermissionNoPermission.Checked Then
                        Me.CheckBoxUserPermissionNoPermission.Checked = False 'Triggers Call tableClientServerProjectUserPermission.RemoveProjectUserPermission(lrProjectUserPermission)
                    End If
                Case Else
                    Throw New Exception("Permission: " & aiPermission.ToString & " not allowed for this method.")
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub setGroupPermission(ByRef arGroup As ClientServer.Group, aiPermission As pcenumPermission)

        Try
            Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

            lrProjectGroupPermission.Project = Me.zrProject
            lrProjectGroupPermission.Group = arGroup
            lrProjectGroupPermission.Permission = aiPermission

            Select Case aiPermission
                Case Is = pcenumPermission.Create
                    If Not Me.CheckBoxGroupPermissionCreate.Checked Then
                        Me.CheckBoxGroupPermissionCreate.Checked = True 'Triggers Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.Read
                    If Not Me.CheckBoxGroupPermissionRead.Checked Then
                        Me.CheckBoxGroupPermissionRead.Checked = True 'Triggers Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.Alter
                    If Not Me.CheckBoxGroupPermissionAlter.Checked Then
                        Me.CheckBoxGroupPermissionAlter.Checked = True 'Triggers Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Else
                    Throw New Exception("Permission: " & aiPermission.ToString & " not allowed for this method.")
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CheckBoxGroupPermissionFullRights_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGroupPermissionFullRights.CheckedChanged

        Dim lrGroup As New ClientServer.Group
        Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag
        lrProjectGroupPermission.Project = Me.zrProject
        lrProjectGroupPermission.Group = lrGroup
        lrProjectGroupPermission.Permission = pcenumPermission.FullRights

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxGroupPermissionFullRights.Checked Then
                Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)

                Call Me.setGroupPermission(lrGroup, pcenumPermission.Create)
                Call Me.setGroupPermission(lrGroup, pcenumPermission.Read)
                Call Me.setGroupPermission(lrGroup, pcenumPermission.Alter)
                Call Me.removeGroupPermission(lrGroup, pcenumPermission.NoRights)
            Else
                Call tableClientServerProjectGroupPermission.DeleteProjectGroupPermission(lrProjectGroupPermission)
            End If
        End If

    End Sub

    Private Sub removeGroupPermission(ByRef arGroup As ClientServer.Group, ByVal aiPermission As pcenumPermission)

        Try
            Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

            lrProjectGroupPermission.Project = Me.zrProject
            lrProjectGroupPermission.Group = arGroup
            lrProjectGroupPermission.Permission = aiPermission

            Select Case aiPermission
                Case Is = pcenumPermission.FullRights
                    If Me.CheckBoxGroupPermissionFullRights.Checked Then
                        Me.CheckBoxGroupPermissionFullRights.Checked = False 'Triggers Call tableClientServerProjectGroupPermission.RemoveProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.Create
                    If Me.CheckBoxGroupPermissionCreate.Checked Then
                        Me.CheckBoxGroupPermissionCreate.Checked = False 'Triggers Call tableClientServerProjectGroupPermission.RemoveProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.Read
                    If Me.CheckBoxGroupPermissionRead.Checked Then
                        Me.CheckBoxGroupPermissionRead.Checked = False 'Triggers Call tableClientServerProjectGroupPermission.RemoveProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.Alter
                    If Me.CheckBoxGroupPermissionAlter.Checked Then
                        Me.CheckBoxGroupPermissionAlter.Checked = False 'Triggers Call tableClientServerProjectGroupPermission.RemoveProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Is = pcenumPermission.NoRights
                    If Me.CheckBoxGroupPermissionNoPermission.Checked Then
                        Me.CheckBoxGroupPermissionNoPermission.Checked = False 'Triggers Call tableClientServerProjectGroupPermission.RemoveProjectGroupPermission(lrProjectGroupPermission)
                    End If
                Case Else
                    Throw New Exception("Permission: " & aiPermission.ToString & " not allowed for this method.")
            End Select

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CheckBoxGroupPermissionNoPermission_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGroupPermissionNoPermission.CheckedChanged

        Dim lrGroup As New ClientServer.Group
        Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag
        lrProjectGroupPermission.Project = Me.zrProject
        lrProjectGroupPermission.Group = lrGroup
        lrProjectGroupPermission.Permission = pcenumPermission.NoRights

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxGroupPermissionNoPermission.Checked Then
                Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)

                Call Me.removeGroupPermission(lrGroup, pcenumPermission.FullRights)
                Call Me.removeGroupPermission(lrGroup, pcenumPermission.Create)
                Call Me.removeGroupPermission(lrGroup, pcenumPermission.Read)
                Call Me.removeGroupPermission(lrGroup, pcenumPermission.Alter)
            Else
                Call tableClientServerProjectGroupPermission.DeleteProjectGroupPermission(lrProjectGroupPermission)
            End If
        End If

    End Sub

    Private Sub CheckBoxGroupPermissionCreate_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGroupPermissionCreate.CheckedChanged

        Dim lrGroup As New ClientServer.Group
        Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag
        lrProjectGroupPermission.Project = Me.zrProject
        lrProjectGroupPermission.Group = lrGroup
        lrProjectGroupPermission.Permission = pcenumPermission.Create

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxGroupPermissionCreate.Checked Then
                Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
            Else
                Call tableClientServerProjectGroupPermission.DeleteProjectGroupPermission(lrProjectGroupPermission)

                Call Me.removeGroupPermission(lrGroup, pcenumPermission.FullRights)
            End If
        End If

    End Sub

    Private Sub CheckBoxGroupPermissionRead_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGroupPermissionRead.CheckedChanged

        Dim lrGroup As New ClientServer.Group
        Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag
        lrProjectGroupPermission.Project = Me.zrProject
        lrProjectGroupPermission.Group = lrGroup
        lrProjectGroupPermission.Permission = pcenumPermission.Read

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxGroupPermissionRead.Checked Then
                Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
            Else
                Call tableClientServerProjectGroupPermission.DeleteProjectGroupPermission(lrProjectGroupPermission)

                Call Me.removeGroupPermission(lrGroup, pcenumPermission.FullRights)
            End If
        End If

    End Sub

    Private Sub CheckBoxGroupPermissionAlter_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxGroupPermissionAlter.CheckedChanged

        Dim lrGroup As New ClientServer.Group
        Dim lrProjectGroupPermission As New ClientServer.ProjectGroupPermission

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag
        lrProjectGroupPermission.Project = Me.zrProject
        lrProjectGroupPermission.Group = lrGroup
        lrProjectGroupPermission.Permission = pcenumPermission.Alter

        If Not Me.zbUpdatingPermissions Then
            If Me.CheckBoxGroupPermissionAlter.Checked Then
                Call tableClientServerProjectGroupPermission.AddProjectGroupPermission(lrProjectGroupPermission)
            Else
                Call tableClientServerProjectGroupPermission.DeleteProjectGroupPermission(lrProjectGroupPermission)

                Call Me.removeGroupPermission(lrGroup, pcenumPermission.FullRights)
            End If
        End If

    End Sub

    Private Sub loadIncludedNamespaces(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrNamespace As ClientServer.Namespace

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerNamespace"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= " ORDER BY Namespace"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrNamespace = New ClientServer.Namespace
                    lrNamespace.Id = lREcordset("Id").Value
                    lrNamespace.Name = lREcordset("Namespace").Value

                    Dim lrComboboxItem As New tComboboxItem(lrNamespace.Id, lrNamespace.Name, lrNamespace)
                    Me.ListBoxIncludedNamespaces.Items.Add(lrComboboxItem)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ComboBoxGroupPermissions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxGroupPermissions.SelectedIndexChanged

        Dim lrGroup As New ClientServer.Group

        lrGroup = Me.ComboBoxGroupPermissions.SelectedItem.Tag

        Me.zbUpdatingPermissions = True
        Me.CheckBoxGroupPermissionFullRights.Checked = False
        Me.CheckBoxGroupPermissionNoPermission.Checked = False
        Me.CheckBoxGroupPermissionCreate.Checked = False
        Me.CheckBoxGroupPermissionRead.Checked = False
        Me.CheckBoxGroupPermissionAlter.Checked = False
        Me.zbUpdatingPermissions = False

        Me.zbUpdatingPermissions = True
        Call Me.LoadPermissionsForProjectGroup(Me.zrProject, lrGroup)
        Me.zbUpdatingPermissions = False

    End Sub

    Public Sub HandleUserSelected(ByVal asUserId As String)

        Dim lrUser As New ClientServer.User
        lrUser.Id = asUserId

        Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser)

        Dim larPermission As New List(Of ClientServer.Permission)

        larPermission = lrUser.getProjectPermissions(Me.zrProject)


        Me.CheckBoxLeastRestrictiveFullPermissions.Checked = larPermission.FindAll(Function(x) x.Permission = pcenumPermission.FullRights).Count > 0
        Me.CheckBoxLeastRestrictiveCreate.Checked = larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Create).Count > 0
        Me.CheckBoxLeastRestrictiveRead.Checked = larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Read).Count > 0
        Me.CheckBoxLeastRestrictiveAlter.Checked = larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0
        Me.CheckBoxLeastRestrictivenoPermission.Checked = larPermission.FindAll(Function(x) x.Permission = pcenumPermission.NoRights).Count > 0

    End Sub

    Public Sub HandleUserToJoinProjectInvite(ByRef loObject As Object)

        Dim lrUser As ClientServer.User
        Dim lrInvitation As New ClientServer.Invitation
        Dim lsMessage As String = ""

        lrUser = CType(loObject, ClientServer.User)

        lsMessage = "Please confirm that you would like to invite, " & lrUser.FullName & " , to join the Project, '" & Me.zrProject.Name & "'"

        If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            lrInvitation.DateTime = Now
            lrInvitation.InvitingUser = prApplication.User
            lrInvitation.InvitedUser = lrUser
            lrInvitation.InvitationType = pcenumInvitationType.UserToJoinProject
            lrInvitation.Tag = Me.zrProject.Id
            lrInvitation.Accepted = False
            lrInvitation.Closed = False

            Call tableClientServerInvitation.addInvitation(lrInvitation)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                Dim lrInterfaceInvitation As New Viev.FBM.Interface.Invitation

                lrInterfaceInvitation.InvitedUserId = lrUser.Id
                lrInterfaceInvitation.InvitedToJoinProjectId = Me.zrProject.Id

                lrBroadcast.Invitation = lrInterfaceInvitation

                Call prDuplexServiceClient.BroadcastInvitationToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.UserManagementInviteUserToJoinProject,
                                                                              lrBroadcast)
            End If

        End If

    End Sub

    Public Sub HandleGroupToJoinProjectInvite(ByRef loObject As Object)

        Dim lrGroup As ClientServer.Group
        Dim lrInvitation As New ClientServer.Invitation
        Dim lsMessage As String = ""

        lrGroup = CType(loObject, ClientServer.Group)

        lsMessage = "Please confirm that you would like to invite, " & lrGroup.Name & " , to join the Project, '" & Me.zrProject.Name & "'"

        If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            lrInvitation.DateTime = Now
            lrInvitation.InvitingUser = prApplication.User
            lrInvitation.InvitedGroup = lrGroup
            lrInvitation.InvitationType = pcenumInvitationType.GroupToJoinProject
            lrInvitation.Tag = Me.zrProject.Id
            lrInvitation.Accepted = False
            lrInvitation.Closed = False

            Call tableClientServerInvitation.addInvitation(lrInvitation)

        End If

    End Sub

    Private Sub TextBoxUserName_Enter(sender As Object, e As EventArgs) Handles TextBoxUserName.Enter

        'Watermark
        If Me.TextBoxUserName.Text = "" Then 'Display the Watermark
            Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
            Me.TextBoxUserName.ForeColor = Color.LightGray
        ElseIf Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag Then
            Me.TextBoxUserName.Text = ""
            Me.TextBoxUserName.ForeColor = Color.Black
        End If

    End Sub

    Private Sub TextBoxUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxUserName.KeyDown

        'Watermark
        'If Me.TextBoxUserName.Text = "" Then 'Display the Watermark
        '    Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
        '    Me.TextBoxUserName.ForeColor = Color.LightGray
        'Else

        If Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag Then
            Me.TextBoxUserName.Text = ""
            Me.TextBoxUserName.ForeColor = Color.Black
        End If

    End Sub

    Private Sub TextBoxUserName_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBoxUserName.KeyUp

        Dim lsMessage As String = ""
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        'Watermark        
        If Me.TextBoxUserName.Text = "" Then 'Display the Watermark
            Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
            Me.TextBoxUserName.ForeColor = Color.LightGray
        End If

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Dim lrUser As ClientServer.User

        If Trim(Me.TextBoxUserName.Text) = "" Then
            Me.FlexibleListBox.Clear()
            Me.zrUserInviteList.Clear()
            Exit Sub
        End If

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT UserId"
            lsSQLQuery &= "                    FROM ClientServerProjectUser"
            lsSQLQuery &= "                   WHERE ProjectId = '" & Me.zrProject.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= "   AND (FirstName + ' ' + LastName) LIKE '" & Trim(Me.TextBoxUserName.Text) & "%'"
            If Not prApplication.User.IsSuperuser Then
                lsSQLQuery &= "   AND NOT Username = 'admin'"
            End If
            lsSQLQuery &= " ORDER BY FirstName"

            lREcordset.Open(lsSQLQuery)

            Dim larNewUserInviteList As New List(Of ClientServer.User)
            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrUser = New ClientServer.User
                    lrUser.Id = lREcordset("Id").Value
                    Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser)

                    If Not Me.zrUserInviteList.Contains(lrUser) Then
                        Me.zrUserInviteList.Add(lrUser)

                        Dim loUserInviter As New UserInviter

                        loUserInviter.LabelUserName.Text = lrUser.FullName
                        loUserInviter.Width = Me.FlexibleListBox.Width - 8
                        loUserInviter.Tag = lrUser

                        AddHandler loUserInviter.InvitePressed, AddressOf HandleUserToJoinProjectInvite

                        Me.FlexibleListBox.Add(loUserInviter)
                    End If

                    larNewUserInviteList.Add(lrUser)

                    lREcordset.MoveNext()
                End While
            End If

            Dim larControlsToRemove As New List(Of UserInviter)
            Dim loCtl As UserInviter
            For Each lrUser In Me.zrUserInviteList.ToArray
                If Not larNewUserInviteList.Contains(lrUser) Then
                    For Each loCtl In Me.FlexibleListBox.flpListBox.Controls
                        If loCtl.Tag.Id = lrUser.Id Then
                            larControlsToRemove.Add(loCtl)
                        End If
                    Next
                    Me.zrUserInviteList.Remove(lrUser)
                End If
            Next

            For Each loCtl In larControlsToRemove
                loCtl.Hide()
                loCtl.Dispose()
            Next

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TabControl1_GotFocus(sender As Object, e As EventArgs) Handles TabControl1.GotFocus

        Me.TextBoxUserName.Focus()

    End Sub

    Private Sub TextBoxGroupInvite_Enter(sender As Object, e As EventArgs) Handles TextBoxGroupInvite.Enter

        'Watermark
        If Me.TextBoxGroupInvite.Text = "" Then 'Display the Watermark
            Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag
            Me.TextBoxGroupInvite.ForeColor = Color.LightGray
        ElseIf Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag Then
            Me.TextBoxGroupInvite.Text = ""
            Me.TextBoxGroupInvite.ForeColor = Color.Black
        End If

    End Sub

    Private Sub TextBoxGroupInvite_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxGroupInvite.KeyDown

        'Watermark
        'If Me.TextBoxGroupInvite.Text = "" Then 'Display the Watermark
        '    Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag
        '    Me.TextBoxGroupInvite.ForeColor = Color.LightGray
        'Else
        If Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag Then
            Me.TextBoxGroupInvite.Text = ""
            Me.TextBoxGroupInvite.ForeColor = Color.Black
        End If

    End Sub

    Private Sub TextBoxGroupInvite_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBoxGroupInvite.KeyUp

        Dim lsMessage As String = ""
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        'Watermark
        If Me.TextBoxGroupInvite.Text = "" Then 'Display the Watermark
            Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag
            Me.TextBoxGroupInvite.ForeColor = Color.LightGray
        End If

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Dim lrGroup As ClientServer.Group

        If Trim(Me.TextBoxGroupInvite.Text) = "" Then
            Me.FlexibleListBoxGroupInvite.Clear()
            Me.zrGroupInviteList.Clear()
            Exit Sub
        End If

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT GroupId"
            lsSQLQuery &= "                    FROM ClientServerProjectGroup"
            lsSQLQuery &= "                   WHERE ProjectId = '" & Me.zrProject.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= "   AND GroupName LIKE '" & Trim(Me.TextBoxGroupInvite.Text) & "%'"
            lsSQLQuery &= " ORDER BY GroupName"

            lREcordset.Open(lsSQLQuery)

            Dim larNewGroupInviteList As New List(Of ClientServer.Group)
            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrGroup = New ClientServer.Group
                    lrGroup.Id = lREcordset("Id").Value
                    Call tableClientServerGroup.getGroupDetailsById(lrGroup.Id, lrGroup)

                    If Not Me.zrGroupInviteList.Contains(lrGroup) Then
                        Me.zrGroupInviteList.Add(lrGroup)

                        Dim loGroupInviter As New GroupInviter

                        loGroupInviter.LabelGroupName.Text = lrGroup.Name
                        loGroupInviter.Width = Me.FlexibleListBox.Width - 8
                        loGroupInviter.Tag = lrGroup

                        AddHandler loGroupInviter.InvitePressed, AddressOf HandleGroupToJoinProjectInvite

                        Me.FlexibleListBoxGroupInvite.Add(loGroupInviter)
                    End If

                    larNewGroupInviteList.Add(lrGroup)

                    lREcordset.MoveNext()
                End While
            End If

            Dim larControlsToRemove As New List(Of GroupInviter)
            Dim loCtl As GroupInviter
            For Each lrGroup In Me.zrGroupInviteList.ToArray
                If Not larNewGroupInviteList.Contains(lrGroup) Then
                    For Each loCtl In Me.FlexibleListBoxGroupInvite.flpListBox.Controls
                        If loCtl.Tag.Id = lrGroup.Id Then
                            larControlsToRemove.Add(loCtl)
                        End If
                    Next
                    Me.zrGroupInviteList.Remove(lrGroup)
                End If
            Next

            For Each loCtl In larControlsToRemove
                loCtl.Hide()
                loCtl.Dispose()
            Next

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub TextBoxUserName_Leave(sender As Object, e As EventArgs) Handles TextBoxUserName.Leave

        'Watermark
        If Me.TextBoxUserName.Text = "" Then 'Display the Watermark
            Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
            Me.TextBoxUserName.ForeColor = Color.LightGray
        ElseIf Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag Then
            Me.TextBoxUserName.ForeColor = Color.LightGray
        End If

    End Sub

    Private Sub TextBoxGroupInvite_Leave(sender As Object, e As EventArgs) Handles TextBoxGroupInvite.Leave

        'Watermark
        If Me.TextBoxGroupInvite.Text = "" Then 'Display the Watermark
            Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag
            Me.TextBoxGroupInvite.ForeColor = Color.LightGray
        ElseIf Me.TextBoxGroupInvite.Text = Me.TextBoxGroupInvite.Tag Then
            Me.TextBoxGroupInvite.ForeColor = Color.LightGray
        End If

    End Sub

    Private Sub ListBoxIncludedGroups_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBoxIncludedGroups.SelectedIndexChanged

        If Me.ListBoxIncludedGroups.SelectedIndex = -1 Then Exit Sub

        Dim lrGroup As ClientServer.Group
        lrGroup = Me.ListBoxIncludedGroups.SelectedItem.Tag

        Dim larUser As New List(Of ClientServer.User)
        larUser = tableClientServerGroupUser.GetUsersForGroupOnProject(lrGroup, Me.zrProject)

        Me.LabelPromptGroupUsersInProject.Text = "Group Users in this Project ('" & lrGroup.Name & "' Group):"

        'Clear the GroupUsers listbox
        Me.ListBoxGroupUsers.Items.Clear()

        Dim lrComboboxItem As tComboboxItem
        For Each lrUser In larUser
            lrComboboxItem = New tComboboxItem(lrUser.Id, lrUser.FullName, lrUser)
            Me.ListBoxGroupUsers.Items.Add(lrComboboxItem)
        Next

    End Sub

End Class