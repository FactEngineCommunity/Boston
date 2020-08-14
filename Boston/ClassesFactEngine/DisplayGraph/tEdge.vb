Namespace FactEngine.DisplayGraph
    Public Class Edge

        Public QueryEdge As FactEngine.QueryEdge

        Public BaseNode As FactEngine.DisplayGraph.Node = Nothing
        Public TargetNode As FactEngine.DisplayGraph.Node = Nothing

        Public Link As MindFusion.Diagramming.DiagramLink = Nothing

        Public Sub New(ByRef arBaseNode As FactEngine.DisplayGraph.Node,
                        ByRef arTargetNode As FactEngine.DisplayGraph.Node)
            Me.BaseNode = arBaseNode
            Me.TargetNode = arTargetNode
        End Sub

    End Class

End Namespace
