Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class Role
        Implements IEquatable(Of ClientServer.Role)

        Public Id As String = System.Guid.NewGuid.ToString

        Public Name As String = ""

        Public [FunctionT] As New List(Of ClientServer.Function)

        Public [Function] As New List(Of pcenumFunction)

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asRoleId As String, ByVal asRoleName As String)

            Me.Id = asRoleId
            Me.Name = asRoleName

        End Sub

        Public Shadows Function Equals(other As Role) As Boolean Implements IEquatable(Of Role).Equals

            Return Me.Id = other.Id
        End Function
    End Class

End Namespace
