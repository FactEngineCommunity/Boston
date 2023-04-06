Imports DynamicClassLibrary.Factory

Public Class tTuple
    Inherits tClass

    Public TupleId As String 'The UniqueIdentifier for the Tuple
    Public AttributeList As New List(Of String)

    Sub New()
        Me.TupleId = System.Guid.NewGuid.ToString
    End Sub

    Sub New(ByVal as_tuple_id As String)

        Me.TupleId = as_tuple_id
    End Sub


End Class
