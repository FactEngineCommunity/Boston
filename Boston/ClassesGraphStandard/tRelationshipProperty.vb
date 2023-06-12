Namespace GraphStandard
    Public Class RelationshipProperty
        Public Property Token As String
        Public Property Type As RelationshipPropertyType
        Public Property Nullable As Boolean

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipProperty"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipProperty"/> class with the specified token, type, and nullable flag.
        ''' </summary>
        ''' <param name="token">The token of the relationship property.</param>
        ''' <param name="type">The type of the relationship property.</param>
        ''' <param name="nullable">Indicates whether the relationship property is nullable.</param>
        Public Sub New(ByVal token As String, ByVal type As RelationshipPropertyType, ByVal nullable As Boolean)
            Me.Token = token
            Me.Type = type
            Me.Nullable = nullable
        End Sub
    End Class
End Namespace
