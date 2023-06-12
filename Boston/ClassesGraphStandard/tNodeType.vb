Namespace GraphStandard
    Public Class NodeType
        Public Property Type As String

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NodeType"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NodeType"/> class with the specified type.
        ''' </summary>
        ''' <param name="type">The type of the node.</param>
        Public Sub New(ByVal type As String)
            Me.Type = type
        End Sub
    End Class
End Namespace
