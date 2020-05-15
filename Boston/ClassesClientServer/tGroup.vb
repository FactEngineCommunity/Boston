Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class Group
        Implements IEquatable(Of ClientServer.Group)

        Public Id As String = System.Guid.NewGuid.ToString

        Public Name As String = ""

        Public CreatedByUser As ClientServer.User

        Public Shadows Function Equals(other As ClientServer.Group) As Boolean Implements IEquatable(Of ClientServer.Group).Equals

            Return Me.Id = other.Id

        End Function

        ''' <summary>
        ''' Includes all the Users in the Group within the nominated Project.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub IncludeAllUsersInProject(ByRef arProject As ClientServer.Project)

            Dim lrRole As New ClientServer.Role("Modeller", "Modeller")

            For Each lrUser In tableClientServerGroupUser.GetUsersForGroup(Me)

                If tableClientServerProjectUser.isUserOnProject(lrUser, arProject) Then
                    'User is already on the Project
                Else
                    Call tableClientServerProjectUser.AddUserToProject(lrUser, arProject)
                    '------------------------------------------------------------------------------------
                    'Make sure that each User at least has the Role of "Modeller" on the Project.
                    Call tableClientServerProjectUserRole.AddUserRoleToProject(lrUser, lrRole, arProject)
                End If

            Next

        End Sub

    End Class

End Namespace
