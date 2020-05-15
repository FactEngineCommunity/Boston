Namespace ERD

    <Serializable()> _
    Public Class Diagram

        Public Entity As New List(Of FBM.FactDataInstance)
        Public Attribute As New List(Of ERD.Attribute)
        Public Relation As New List(Of ERD.Relation)

    End Class
End Namespace