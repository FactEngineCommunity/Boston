Imports System.Reflection

Public Module tableClientServerUser

    Public Sub addUser(ByRef arUser As ClientServer.User)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerUser"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arUser.Id & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arUser.Username, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arUser.PasswordHash, "'", "`")) & "'"
            lsSQLQuery &= " ," & arUser.ResetPassword
            lsSQLQuery &= " ," & arUser.IsActive
            lsSQLQuery &= " ," & arUser.IsSuperuser
            lsSQLQuery &= " ,'" & Trim(Replace(arUser.FirstName, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arUser.LastName, "'", "`")) & "'"
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Function GetAllPermissionsForUserForProject(ByRef arUser As ClientServer.User,
                                                       ByRef arProject As ClientServer.Project) As List(Of ClientServer.Permission)

        Dim larPermission As New List(Of ClientServer.Permission)

        Try
            Dim lrPermission As ClientServer.Permission

            '==============================================================================================
            'Get the User Permissions for the User for the Project
            Dim larProjectUserPermissions As New List(Of ClientServer.ProjectUserPermission)
            Call tableClientServerProjectUserPermission.GetPermissionsForProjectUser(arProject, arUser, larProjectUserPermissions)

            For Each lrProjectUserPermission In larProjectUserPermissions
                lrPermission = New ClientServer.Permission
                lrPermission.Project = arProject
                lrPermission.PermissionClass = pcenumPermissionClass.User
                lrPermission.Permission = lrProjectUserPermission.Permission

                larPermission.Add(lrPermission)
            Next

            '===============================================================================================
            'Get the Group Permissions for the User for the Project
            Dim larProjectGroupPermissions As New List(Of ClientServer.ProjectGroupPermission)
            Dim larGroup As New List(Of ClientServer.Group)

            larGroup = tableClientServerProjectGroup.getGroupsForUserForProject(arUser, arProject)
            For Each lrGroup In larGroup
                Call tableClientServerProjectGroupPermission.GetPermissionsForProjectGroup(arProject, lrGroup, larProjectGroupPermissions)

                For Each lrProjectGroupPermission In larProjectGroupPermissions
                    lrPermission = New ClientServer.Permission
                    lrPermission.Project = arProject
                    lrPermission.PermissionClass = pcenumPermissionClass.Group
                    lrPermission.Permission = lrProjectGroupPermission.Permission

                    larPermission.Add(lrPermission)
                Next
            Next

            Return larPermission

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larPermission
        End Try

    End Function

    Public Function IsValidUser(ByVal asUsername As String, ByVal asPassword As String) As Boolean

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        '------------------------
        'Initialise return value
        '------------------------
        IsValidUser = False

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT COUNT(*)"
        lsSQLQuery &= "  FROM ClientServerUser"
        lsSQLQuery &= " WHERE Username = '" & Trim(Replace(asUsername, "'", "`")) & "'"
        lsSQLQuery &= " AND PasswordHash = '" & Trim(Replace(ClientServer.getHash(asPassword), "'", "`")) & "'"

        lREcordset.Open(lsSQLQuery)

        Return lREcordset(0).Value > 0

        lREcordset.Close()

    End Function

    Public Sub getAvailableFunctions(ByRef arUser As ClientServer.User)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim liFunction As pcenumFunction

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerRoleFunction"
            lsSQLQuery &= " WHERE RoleId IN (SELECT RoleId FROM ClientServerUserRole WHERE UserId  = '" & arUser.Id & "')"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF

                liFunction = CType([Enum].Parse(GetType(pcenumFunction), lREcordset("FunctionName").Value), pcenumFunction)
                arUser.Function.AddUnique(liFunction)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function getAllUsers(Optional ByVal abIgnoreErrorMessage As Boolean = False) As List(Of ClientServer.User)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy
        Dim lrUser As ClientServer.User = Nothing

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerUser"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then

                getAllUsers = New List(Of ClientServer.User)

                While Not lREcordset.EOF
                    lrUser = New ClientServer.User

                    lrUser.Id = lREcordset("Id").Value
                    lrUser.Username = Trim(lREcordset("Username").Value)
                    lrUser.FirstName = Trim(lREcordset("FirstName").Value)
                    lrUser.LastName = Trim(lREcordset("LastName").Value)
                    lrUser.PasswordHash = lREcordset("PasswordHash").Value
                    lrUser.IsActive = CBool(lREcordset("IsActive").Value)
                    lrUser.ResetPassword = CBool(lREcordset("ResetPassword").Value)
                    lrUser.IsSuperuser = CBool(lREcordset("IsSuperuser").Value)

                    lrUser.Role = tableClientServerUserRole.getRolesForUser(lrUser, True)

                    'arUser.Function is populated in publicClientServerModule.loginUser. No need to do anything here.
                    getAllUsers.Add(lrUser)

                    lREcordset.MoveNext()
                End While
            Else
                If Not abIgnoreErrorMessage Then
                    Dim lsMessage As String = "Error: Thre are no users in the database."
                    Throw New Exception(lsMessage)
                End If
                Return New List(Of ClientServer.User)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return New List(Of ClientServer.User)
        End Try

    End Function

    Public Function getUserDetailsByUsername(ByVal asUsername As String, ByRef arUser As ClientServer.User, Optional ByVal abIgnoreErrorMessage As Boolean = False) As ClientServer.User

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerUser"
            lsSQLQuery &= " WHERE Username = '" & Trim(asUsername) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                If arUser Is Nothing Then
                    arUser = New ClientServer.User
                End If

                arUser.Id = lREcordset("Id").Value
                arUser.Username = Trim(lREcordset("Username").Value)
                arUser.FirstName = Trim(lREcordset("FirstName").Value)
                arUser.LastName = Trim(lREcordset("LastName").Value)
                arUser.PasswordHash = lREcordset("PasswordHash").Value
                arUser.IsActive = CBool(lREcordset("IsActive").Value)
                arUser.ResetPassword = CBool(lREcordset("ResetPassword").Value)
                arUser.IsSuperuser = CBool(lREcordset("IsSuperuser").Value)

                arUser.Role = tableClientServerUserRole.getRolesForUser(arUser, True)
                'arUser.Function is populated in publicClientServerModule.loginUser. No need to do anything here.

                Return arUser
            Else
                If Not abIgnoreErrorMessage Then
                    Dim lsMessage As String = "Error: getUserDetailsByUsername: No User returned for Username: " & asUsername
                    Throw New Exception(lsMessage)
                End If
                Return Nothing
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try


    End Function

    Public Sub getUserDetailsById(ByVal asUserId As String,
                                  ByRef arUser As ClientServer.User,
                                  Optional ByVal abGetRoleDetails As Boolean = True)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id = '" & Trim(asUserId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arUser.Id = lREcordset("Id").Value
                arUser.Username = Trim(lREcordset("Username").Value)
                arUser.FirstName = Trim(lREcordset("FirstName").Value)
                arUser.LastName = Trim(lREcordset("LastName").Value)
                arUser.PasswordHash = lREcordset("PasswordHash").Value
                arUser.IsActive = CBool(lREcordset("IsActive").Value)
                arUser.ResetPassword = CBool(lREcordset("ResetPassword").Value)
                arUser.IsSuperuser = CBool(lREcordset("IsSuperuser").Value)

                If abGetRoleDetails Then
                    arUser.Role = tableClientServerUserRole.getRolesForUser(arUser, True)
                End If
                'arUser.Function is populated in publicClientServerModule.loginUser. No need to do anything here.
            Else
                Dim lsMessage As String = "Error: getUserDetailsById: No User returned for Id: " & asUserId
                Throw New Exception(lsMessage)
            End If

            lREcordset.Close()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub updateUser(ByRef arUser As ClientServer.User)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE ClientServerUser"
            lsSQLQuery &= "   SET Username = '" & Trim(Replace(arUser.Username, "'", "`")) & "'"
            lsSQLQuery &= "       , PasswordHash = '" & Trim(arUser.PasswordHash) & "'"
            lsSQLQuery &= "       , IsActive = " & arUser.IsActive
            lsSQLQuery &= "       , ResetPassword = " & arUser.ResetPassword
            lsSQLQuery &= "       , IsSuperuser = " & arUser.IsSuperuser
            lsSQLQuery &= "       , FirstName = '" & Trim(arUser.FirstName) & "'"
            lsSQLQuery &= "       , LastName = '" & Trim(arUser.LastName) & "'"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arUser.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception

            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try


    End Sub

End Module
