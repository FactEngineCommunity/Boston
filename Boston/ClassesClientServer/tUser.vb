Imports System.Reflection
Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class User
        Implements IEquatable(Of ClientServer.User)

        Public Id As String = System.Guid.NewGuid.ToString

        Public Username As String = ""

        Public PasswordHash As String = ""

        Public ResetPassword As Boolean = False

        Public IsActive As Boolean = True

        Public IsSuperuser As Boolean = False

        Public FirstName As String = ""

        Public LastName As String = ""

        Public IsLoggedIn As Boolean = False

        Public [Function] As New List(Of pcenumFunction)

        Public Role As New List(Of ClientServer.Role)

        Public ProjectPermission As New List(Of ClientServer.Permission)

        Public Function FullName() As String

            Return Me.FirstName & " " & Me.LastName

        End Function

        Public Shadows Function Equals(other As User) As Boolean Implements IEquatable(Of User).Equals

            Return Me.Id = other.Id

        End Function

        Public Function CanAlterOnProject(ByVal arProject As ClientServer.Project) As Boolean

            Try
                If Me.ProjectPermission.Count = 0 Then Return False

                Return Me.ProjectPermission.FindAll(Function(x) (x.Project.Id = arProject.Id) And (x.Permission = pcenumPermission.Alter)).Count > 0

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getProjectPermissions(ByRef arProject As ClientServer.Project) As List(Of ClientServer.Permission)

            '----------------------------------------------------------------------------
            'Clear out any previously collected Permissions
            Me.ProjectPermission.Clear()

            Dim larPermission As New List(Of ClientServer.Permission)

            If arProject.Id = "MyPersonalModels" Then
                larPermission.Add(New ClientServer.Permission(arProject, pcenumPermissionClass.User, pcenumPermission.FullRights))
                larPermission.Add(New ClientServer.Permission(arProject, pcenumPermissionClass.User, pcenumPermission.Create))
                larPermission.Add(New ClientServer.Permission(arProject, pcenumPermissionClass.User, pcenumPermission.Read))
                larPermission.Add(New ClientServer.Permission(arProject, pcenumPermissionClass.User, pcenumPermission.Alter))
                Return larPermission
            End If

            '============================================================================
            'Get the Users Permissions for the Project
            Dim larAllPermissions As New List(Of ClientServer.Permission)

            larAllPermissions = tableClientServerUser.GetAllPermissionsForUserForProject(Me, arProject)

            '----------------------------------------------------------------------------------------------
            'Now decide which permissons are the least restrictive for the User on the Project

            Dim larFullRights As New List(Of ClientServer.Permission)
            larFullRights = larAllPermissions.FindAll(Function(x) x.Permission = pcenumPermission.FullRights)

            Dim larNoPermissions As New List(Of ClientServer.Permission)
            larNoPermissions = larAllPermissions.FindAll(Function(x) x.Permission = pcenumPermission.NoRights)

            Dim larCreatePermission As New List(Of ClientServer.Permission)
            larCreatePermission = larAllPermissions.FindAll(Function(x) x.Permission = pcenumPermission.Create)

            Dim larReadPermission As New List(Of ClientServer.Permission)
            larReadPermission = larAllPermissions.FindAll(Function(x) x.Permission = pcenumPermission.Read)

            Dim larAlterPermission As New List(Of ClientServer.Permission)
            larAlterPermission = larAllPermissions.FindAll(Function(x) x.Permission = pcenumPermission.Alter)

            If larFullRights.Count > 0 Then
                larPermission.Add(larFullRights(0))
                larPermission.Add(larCreatePermission(0))
                larPermission.Add(larReadPermission(0))
                larPermission.Add(larAlterPermission(0))
            End If

            Dim laPermission = {}

            If (larCreatePermission.Count > 0) And Not (larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Create).Count > 0) Then
                larPermission.Add(larCreatePermission(0))
            End If

            If (larReadPermission.Count > 0) And Not (larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Read).Count > 0) Then
                larPermission.Add(larReadPermission(0))
            End If

            If (larAlterPermission.Count > 0) And Not (larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0) Then
                larPermission.Add(larAlterPermission(0))
            End If

            If larPermission.FindAll(Function(x) x.Permission = pcenumPermission.FullRights).Count > 0 Then
                'Can't have NoPermission
            ElseIf larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Create).Count > 0 Then
                'Can't have NoPermission
            ElseIf larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Read).Count > 0 Then
                'Can't have NoPermission
            ElseIf larPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                'Can't have NoPermission
            Else
                If (larNoPermissions.Count > 0) Then
                    larPermission.Add(larNoPermissions(0))
                End If
            End If

            Me.ProjectPermission = larPermission
            Return larPermission

        End Function

    End Class

End Namespace

