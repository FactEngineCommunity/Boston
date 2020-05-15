Namespace ClientServer

    Public Class Permission
        Implements IEquatable(Of ClientServer.Permission)

        Public Project As ClientServer.Project

        Public PermissionClass As pcenumPermissionClass 'User or Group

        Public Permission As pcenumPermission


        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arProject As ClientServer.Project,
                       ByRef aiPermissionClass As pcenumPermissionClass,
                       ByRef aiPermission As pcenumPermission)

            Me.Project = arProject
            Me.PermissionClass = aiPermissionClass
            Me.Permission = aiPermission

        End Sub

        Public Shadows Function Equals(other As Permission) As Boolean Implements IEquatable(Of Permission).Equals

            Return other.Project.Id = Me.Project.Id _
                And other.PermissionClass = Me.PermissionClass _
                And other.Permission = Me.Permission

        End Function

    End Class

End Namespace
