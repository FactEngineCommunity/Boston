Namespace GraphStandard
    Public Class RelationshipObjectType
        Public Property id As String
        Public Property Type As RelationshipTypeRef
        Public Property From As RelationshipNodeRef
        Public Property [To] As RelationshipNodeRef
        Public Property Properties As List(Of RelationshipProperty)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipObjectType"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
            Properties = New List(Of RelationshipProperty)()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipObjectType"/> class with the specified id, type, from node, to node, and properties.
        ''' </summary>
        ''' <param name="id">The identifier of the relationship object type.</param>
        ''' <param name="type">The reference to the relationship type.</param>
        ''' <param name="from">The reference to the starting node of the relationship.</param>
        ''' <param name="to">The reference to the ending node of the relationship.</param>
        ''' <param name="properties">The list of properties associated with the relationship.</param>
        Public Sub New(ByVal id As String, ByVal type As RelationshipTypeRef, ByVal from As RelationshipNodeRef, ByVal [to] As RelationshipNodeRef, ByVal properties As List(Of RelationshipProperty))
            Me.id = id
            Me.Type = type
            Me.From = from
            Me.[To] = [to]
            Me.Properties = properties
        End Sub
    End Class
End Namespace
