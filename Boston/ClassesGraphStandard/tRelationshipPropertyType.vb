Namespace GraphStandard
    Public Class RelationshipPropertyType
        Public Property Type As String
        Public Property Items As RelationshipPropertyItemType

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipPropertyType"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipPropertyType"/> class with the specified type and items.
        ''' </summary>
        ''' <param name="type">The type of the relationship property.</param>
        ''' <param name="items">The item type of the relationship property.</param>
        Public Sub New(ByVal type As String, ByVal items As RelationshipPropertyItemType)
            Me.Type = type
            Me.Items = items
        End Sub
    End Class
End Namespace
