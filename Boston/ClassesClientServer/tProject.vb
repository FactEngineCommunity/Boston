Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class Project
        Implements IEquatable(Of ClientServer.Project)

        Public Id As String = System.Guid.NewGuid.ToString

        Public Name As String = ""

        Private _CreatedByUserId As String = ""
        Public Property CreatedByUserId As String
            Get
                If Me.CreatedByUser Is Nothing Then
                    Return Me._CreatedByUserId
                Else
                    Return Me.CreatedByUser.Id
                End If
            End Get
            Set(value As String)

            End Set
        End Property

        Public CreatedByUser As ClientServer.User

        ''' <summary>
        ''' Parameterless New for Serialisation
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String, ByVal asName As String)

            Me.Id = asId
            Me.Name = asName

        End Sub

        Public Shadows Function Equals(other As Project) As Boolean Implements IEquatable(Of Project).Equals

            Return Me.Id = other.Id

        End Function
    End Class

End Namespace
