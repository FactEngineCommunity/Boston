Namespace TestManagement

    Public Class ConfigurationItem
        Inherits Identifiable
        Implements IEquatable(Of TestManagement.ConfigurationItem)


        Public Property Location As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Shadows Function Equals(other As ConfigurationItem) As Boolean Implements IEquatable(Of ConfigurationItem).Equals
            Return Me.Identifier = other.Identifier
        End Function
    End Class

End Namespace
