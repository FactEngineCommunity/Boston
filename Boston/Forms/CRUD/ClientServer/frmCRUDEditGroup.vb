Imports System.Reflection

Public Class frmCRUDEditGroup

    Public zrGroup As ClientServer.Group
    Private zbValidationError As Boolean = False
    Private zrUserInviteList As New List(Of ClientServer.User)

    Private Sub frmCRUDEditGroup_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Me.setupForm()

    End Sub

    Private Sub setupForm()

        'Watermarks
        Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag
        Me.TextBoxUserName.ForeColor = Color.LightGray

        If prApplication.User.IsSuperuser Then
            'Leave the form the way it is.
            Me.PanelInviteUser.SendToBack()
            Me.ListBoxAvailableUsers.BringToFront()
        ElseIf prApplication.User.Id = Me.zrGroup.CreatedByUser.Id Then
            'Is the person who created the Group, so can edit the Group
            Me.ButtonIncludeUser.Visible = False 'Because using the Invitation method
            Me.ButtonExcludeUser.Visible = True

            Me.ListBoxAvailableUsers.SendToBack() 'Because using the Invitation method
            Me.ListBoxAvailableUsers.Visible = False 'Because using the Invitation method
            Me.PanelInviteUser.BringToFront()
            Me.FlexibleListBox.BringToFront()

            Me.LabelPromptAvailableUsers.Text = "Select people to join this Group:"
        Else
            Me.ButtonIncludeUser.Visible = False
            Me.ButtonExcludeUser.Visible = False

            Me.ListBoxAvailableUsers.SendToBack()
            Me.ListBoxAvailableUsers.Visible = False
            Me.PanelInviteUser.BringToFront()
            Me.FlexibleListBox.BringToFront()
            Me.PanelInviteUser.Visible = False

            Me.LabelPromptAvailableUsers.Visible = False

            Me.TextBoxGroupName.Enabled = False
            Me.ListBoxIncludedUsers.Focus()
        End If

        Me.LabelCreatedByUser.Text = Me.zrGroup.CreatedByUser.FullName

        Me.ErrorProvider.BlinkStyle = ErrorBlinkStyle.AlwaysBlink

        Me.TextBoxGroupName.Text = Trim(Me.zrGroup.Name)

        Call Me.loadAvailableUsers(Me.zrGroup)
        Call Me.loadIncludedUsers(Me.zrGroup)

        Call Me.loadAvailableOntologies(Me.zrGroup)
        Call Me.loadIncludedOntologies(Me.zrGroup)

    End Sub

    Private Sub TextBoxGroupName_TextChanged(sender As Object, e As EventArgs) Handles TextBoxGroupName.TextChanged

        If Trim(TextBoxGroupName.Text) = "" Then
            Me.ErrorProvider.SetError(Me.TextBoxGroupName, "Name is mandatory.")
        Else
            Me.ErrorProvider.Clear()
        End If
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.checkFields() Then
            If Me.getFields(Me.zrGroup) Then

                Call tableClientServerGroup.updateGroup(Me.zrGroup)
                Me.Hide()
                Me.Close()
                Me.Dispose()

            End If
        End If

    End Sub

    Private Function checkFields() As Boolean

        Dim lsMessage As String = ""
        Try
            checkFields = True

            Me.zbValidationError = False

            If Me.TextBoxGroupName.Text = "" Then
                Me.zbValidationError = True
            End If

            Call Me.ValidateChildren()

            If Me.zbValidationError Then
                lsMessage = "There is an error in the data you've set."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Check all field values and try saving again."
                MsgBox(lsMessage)
                checkFields = False
            ElseIf Trim(Me.TextBoxGroupName.Text) = "" Then
                checkFields = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Function

    Private Function getFields(ByRef arGroup As ClientServer.Group) As Boolean

        Try
            getFields = True

            arGroup.Name = Trim(Me.TextBoxGroupName.Text)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Sub TextBoxGroupName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TextBoxGroupName.Validating

        If Trim(Me.TextBoxGroupName.Text) = "" Then
            Me.zbValidationError = True
        End If

    End Sub

    Private Sub loadAvailableOntologies(ByRef arGroup As ClientServer.Group)

        Dim lsMessage As String
        Dim lrOntology As Ontology.UnifiedOntology

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM UnifiedOntology"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT OntologyId"
            lsSQLQuery &= "                    FROM ClientServerGroupOntology"
            lsSQLQuery &= "                   WHERE GroupId = '" & arGroup.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY UnifiedOntologyName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrOntology = New Ontology.UnifiedOntology
                    lrOntology.Id = lREcordset("Id").Value
                    lrOntology.Name = lREcordset("UnifiedOntologyName").Value
                    lrOntology.ImageFileLocationName = lREcordset("ImageFileLocationName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrOntology.Id, lrOntology.Name, lrOntology)
                    Me.ListBoxAvailableOntologies.Items.Add(lrComboboxItem)

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

    Private Sub loadIncludedOntologies(ByRef arGroup As ClientServer.Group)

        Dim lsMessage As String
        Dim lrOntology As Ontology.UnifiedOntology

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM UnifiedOntology"
            lsSQLQuery &= " WHERE Id IN (SELECT OntologyId"
            lsSQLQuery &= "                FROM ClientServerGroupOntology"
            lsSQLQuery &= "               WHERE GroupId = '" & arGroup.Id & "'"
            lsSQLQuery &= "             )"
            lsSQLQuery &= " ORDER BY UnifiedOntologyName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrOntology = New Ontology.UnifiedOntology
                    lrOntology.Id = lREcordset("Id").Value
                    lrOntology.Name = lREcordset("UnifiedName").Value
                    lrOntology.ImageFileLocationName = lREcordset("ImageFileLocationName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrOntology.Id, lrOntology.Name, lrOntology)
                    Me.ListBoxIncludedOntologies.Items.Add(lrComboboxItem)

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


    Private Sub loadAvailableUsers(ByRef arGroup As ClientServer.Group)

        Dim lsMessage As String
        Dim lrUser As ClientServer.User

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id NOT IN (SELECT UserId"
            lsSQLQuery &= "                    FROM ClientServerGroupUser"
            lsSQLQuery &= "                   WHERE GroupId = '" & arGroup.Id & "'"
            lsSQLQuery &= "                 )"
            lsSQLQuery &= " ORDER BY FirstName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrUser = New ClientServer.User
                    lrUser.Id = lREcordset("Id").Value
                    lrUser.FirstName = lREcordset("FirstName").Value
                    lrUser.LastName = lREcordset("LastName").Value

                    Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
                    Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
                    Me.ListBoxAvailableUsers.Items.Add(lrComboboxItem)

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

    Private Sub loadIncludedUsers(ByRef arGroup As ClientServer.Group)

        Dim lsMessage As String
        Dim lrUser As ClientServer.User

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id IN (SELECT UserId"
            lsSQLQuery &= "                FROM ClientServerGroupUser"
            lsSQLQuery &= "               WHERE GroupId = '" & arGroup.Id & "'"
            lsSQLQuery &= "             )"
            lsSQLQuery &= " ORDER BY FirstName"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrUser = New ClientServer.User
                    lrUser.Id = lREcordset("Id").Value
                    lrUser.FirstName = lREcordset("FirstName").Value
                    lrUser.LastName = lREcordset("LastName").Value

                    Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
                    Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
                    Me.ListBoxIncludedUsers.Items.Add(lrComboboxItem)

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

    Private Sub ButtonIncludeUser_Click(sender As Object, e As EventArgs) Handles ButtonIncludeUser.Click

        If Me.ListBoxAvailableUsers.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrUser As ClientServer.User = Me.ListBoxAvailableUsers.SelectedItem.Tag
            Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
            Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
            Me.ListBoxIncludedUsers.Items.Add(lrComboboxItem)
            Me.ListBoxAvailableUsers.Items.RemoveAt(Me.ListBoxAvailableUsers.SelectedIndex)

            Call tableClientServerGroupUser.AddUserToGroup(lrUser, Me.zrGroup)
        End If

    End Sub

    Private Sub ButtonExcludeUser_Click(sender As Object, e As EventArgs) Handles ButtonExcludeUser.Click

        If Me.ListBoxIncludedUsers.SelectedIndex = -1 Then
            Exit Sub
        Else
            Dim lrUser As ClientServer.User = Me.ListBoxIncludedUsers.SelectedItem.Tag

            If lrUser.Id = Me.zrGroup.CreatedByUser.Id Then
                'Can't remove the Creator of the Group from the Group.
                MsgBox("You cannot remove the creator of the Group from the Group.")
                Exit Sub
            End If

            Dim lsName As String = lrUser.FirstName & " " & lrUser.LastName
            Dim lrComboboxItem As New tComboboxItem(lrUser.Id, lsName, lrUser)
            Me.ListBoxAvailableUsers.Items.Add(lrComboboxItem)
            Me.ListBoxIncludedUsers.Items.RemoveAt(Me.ListBoxIncludedUsers.SelectedIndex)

            Call tableClientServerGroupUser.removeUserFromGroup(lrUser, Me.zrGroup)
        End If

    End Sub

    Private Sub TextBoxUserName_Enter(sender As Object, e As EventArgs) Handles TextBoxUserName.Enter

        'Watermark
        If Me.TextBoxUserName.Text = Me.TextBoxUserName.Tag Then
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
        Dim lREcordset As New RecordsetProxy

        'Watermark on KeyDown and Leave
        If Me.TextBoxUserName.Text = "" Then
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
            lsSQLQuery &= "                    FROM ClientServerGroupUser"
            lsSQLQuery &= "                   WHERE GroupId = '" & Me.zrGroup.Id & "'"
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

                        AddHandler loUserInviter.InvitePressed, AddressOf HandleUserInvite

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub HandleUserInvite(ByRef loObject As Object)

        Dim lrUser As ClientServer.User
        Dim lrInvitation As New ClientServer.Invitation
        Dim lsMessage As String = ""

        lrUser = CType(loObject, ClientServer.User)

        lsMessage = "Please confirm that you would like to invite, " & lrUser.FullName & " , to join the Group, '" & Me.zrGroup.Name & "'"

        If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            lrInvitation.DateTime = Now
            lrInvitation.InvitingUser = prApplication.User
            lrInvitation.InvitedUser = lrUser
            lrInvitation.InvitationType = pcenumInvitationType.UserToJoinGroup
            lrInvitation.Tag = Me.zrGroup.Id
            lrInvitation.Accepted = False
            lrInvitation.Closed = False

            Call tableClientServerInvitation.addInvitation(lrInvitation)

        End If

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

    Private Sub ButtonAddOntology_Click(sender As Object, e As EventArgs) Handles ButtonAddOntology.Click

        Try
            If Me.ListBoxAvailableOntologies.SelectedIndex = -1 Then
                Exit Sub
            Else
                Dim lrOntology As Ontology.UnifiedOntology = Me.ListBoxAvailableOntologies.SelectedItem.Tag

                Dim lrComboboxItem As New tComboboxItem(lrOntology.Id, lrOntology.Name, lrOntology)
                Me.ListBoxIncludedOntologies.Items.Add(lrComboboxItem)
                Me.ListBoxAvailableOntologies.Items.RemoveAt(Me.ListBoxAvailableOntologies.SelectedIndex)

                Call tableClientServerGroupOntology.AddOntologyToGroup(lrOntology, Me.zrGroup)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonRemoveOntology_Click(sender As Object, e As EventArgs) Handles ButtonRemoveOntology.Click

        Try
            If Me.ListBoxIncludedOntologies.SelectedIndex = -1 Then
                Exit Sub
            Else
                Dim lrOntology As Ontology.UnifiedOntology = Me.ListBoxIncludedOntologies.SelectedItem.Tag

                Dim lrComboboxItem As New tComboboxItem(lrOntology.Id, lrOntology.Name, lrOntology)
                Me.ListBoxAvailableOntologies.Items.Add(lrComboboxItem)
                Me.ListBoxIncludedOntologies.Items.RemoveAt(Me.ListBoxIncludedOntologies.SelectedIndex)

                Call tableClientServerGroupOntology.removeOntologyFromGroup(lrOntology, Me.zrGroup)
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