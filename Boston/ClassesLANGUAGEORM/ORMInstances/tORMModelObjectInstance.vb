Namespace FBM

    Public Class ModelObjectInstance
        Inherits FBM.ModelObject

        Private _SubtypeRelationship As New List(Of FBM.SubtypeRelationshipInstance)

        Public Overloads Property SubtypeRelationship As List(Of FBM.SubtypeRelationshipInstance)
            Get
                Return Me._SubtypeRelationship
            End Get
            Set(value As List(Of FBM.SubtypeRelationshipInstance))
                Me._SubtypeRelationship = value
            End Set
        End Property

    End Class

End Namespace
