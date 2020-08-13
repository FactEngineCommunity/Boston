Namespace FactEngine.DisplayGraph
    Public Class Node

        Public Table As RDS.Table
        Public Column As New List(Of RDS.Column)

        Public Shape As MindFusion.Diagramming.Shape = Nothing

        Public Edge As List(Of FactEngine.DisplayGraph.Link)

        Public Sub New(ByVal arTable As RDS.Table,
                       ByVal aarColumn As List(Of RDS.Column),
                       ByVal aarLink As List(Of FactEngine.DisplayGraph.Link))

            Me.Table = arTable
            Me.Column = aarColumn

        End Sub

        Public Sub DisplayAndAssociate()

        End Sub

    End Class

End Namespace