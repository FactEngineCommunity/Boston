Namespace ERD

    <Serializable()> _
    Public Class Diagram

        Public Entity As New List(Of FBM.FactDataInstance)
        Public Attribute As New List(Of ERD.Attribute)

        Private _Relation As New List(Of ERD.Relation)
        Public Property Relation As List(Of ERD.Relation)
            Get
                Return Me._Relation
            End Get
            Set(value As List(Of ERD.Relation))
                Me._Relation = value
            End Set
        End Property

    End Class
End Namespace