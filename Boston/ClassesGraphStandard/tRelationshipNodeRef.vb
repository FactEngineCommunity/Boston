Namespace GraphStandard
    Public Class RelationshipNodeRef
        Public Property ref As String

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipNodeRef"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipNodeRef"/> class with the specified reference.
        ''' </summary>
        ''' <param name="ref">The reference of the relationship node.</param>
        Public Sub New(ByVal ref As String)
            Me.ref = ref
        End Sub
    End Class
End Namespace
