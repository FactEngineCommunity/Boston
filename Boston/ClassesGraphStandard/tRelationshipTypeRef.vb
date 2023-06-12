Namespace GraphStandard
    Public Class RelationshipTypeRef
        Public Property ref As String

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipTypeRef"/> class.
        ''' </summary>
        Public Sub New()
            ' Default constructor with no arguments
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelationshipTypeRef"/> class with the specified reference.
        ''' </summary>
        ''' <param name="ref">The reference of the relationship type.</param>
        Public Sub New(ByVal ref As String)
            Me.ref = ref
        End Sub
    End Class
End Namespace
