Namespace GraphStandard
    Public Class NodeProperty
        Public Property Token As String
        Public Property Type As NodeType
        Public Property Nullable As Boolean

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NodeProperty"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NodeProperty"/> class with the specified token, type, and nullable flag.
        ''' </summary>
        ''' <param name="token">The token of the property.</param>
        ''' <param name="type">The type of the property.</param>
        ''' <param name="nullable">Indicates whether the property is nullable.</param>
        Public Sub New(ByVal token As String, ByVal type As NodeType, ByVal nullable As Boolean)
            Me.Token = token
            Me.Type = type
            Me.Nullable = nullable
        End Sub
    End Class
End Namespace