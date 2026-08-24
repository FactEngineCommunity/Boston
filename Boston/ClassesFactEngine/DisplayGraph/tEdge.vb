Namespace FactEngine.DisplayGraph
    Public Class Edge
        Implements IEquatable(Of FactEngine.DisplayGraph.Edge)

        Public QueryEdge As FactEngine.QueryEdge

        Public BaseNode As FactEngine.DisplayGraph.Node = Nothing
        Public TargetNode As FactEngine.DisplayGraph.Node = Nothing

        Public Link As MindFusion.Diagramming.DiagramLink = Nothing

        Public Predicate As String = ""

        Public Sub New(ByRef arBaseNode As FactEngine.DisplayGraph.Node,
                        ByRef arTargetNode As FactEngine.DisplayGraph.Node)
            Me.BaseNode = arBaseNode
            Me.TargetNode = arTargetNode
        End Sub

        Public Shadows Function Equals(other As Edge) As Boolean Implements IEquatable(Of Edge).Equals

            Return Me.BaseNode.Name = other.BaseNode.Name And Me.TargetNode.Name = other.TargetNode.Name

        End Function
    End Class

End Namespace
