Imports Newtonsoft.Json

Namespace TestManagement

    ' Class for steps within a test
    Public Class Resource
        Inherits Identifiable
        Implements IEquatable(Of TestManagement.Resource)

        Public Property Location As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As Resource) As Boolean Implements IEquatable(Of Resource).Equals
            Return Me.Identifier = other.Identifier
        End Function
    End Class

End Namespace