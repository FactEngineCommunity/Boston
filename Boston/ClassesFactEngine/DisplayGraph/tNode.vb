Imports MindFusion.Diagramming

Namespace FactEngine.DisplayGraph
    Public Class Node

        Private Diagram As MindFusion.Diagramming.Diagram = Nothing

        Public Table As RDS.Table
        Public Column As New List(Of RDS.Column)

        Public Name As String = ""

        Public Shape As MindFusion.Diagramming.ShapeNode = Nothing

        Public Edge As List(Of FactEngine.DisplayGraph.Link)

        Public Link As New List(Of FactEngine.DisplayGraph.Link)

        Public X As Integer
        Public Y As Integer

        Public Sub New(ByRef arDiagram As MindFusion.Diagramming.Diagram,
                       ByVal arTable As RDS.Table,
                       ByVal aarColumn As List(Of RDS.Column),
                       ByVal asName As String,
                       ByVal aarLink As List(Of FactEngine.DisplayGraph.Link)
                       )

            Me.Diagram = arDiagram
            Me.Table = arTable
            Me.Column = aarColumn
            Me.Name = asName
            Me.Link = aarLink

        End Sub

        Public Sub DisplayAndAssociate()

            Dim loDroppedNode As New ShapeNode

            loDroppedNode = Me.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
            'loDroppedNode.Move(Me.X, Me.Y)
            loDroppedNode.Shape = Shapes.Ellipse
            loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
            loDroppedNode.AllowOutgoingLinks = True
            loDroppedNode.AllowIncomingLinks = True
            loDroppedNode.Resize(10, 10)
            'loDroppedNode.Tag = New FBM.EntityTypeInstance
            loDroppedNode.Tag = Me
            loDroppedNode.ShadowColor = Color.White
            'loDroppedNode.ShadowOffsetX = 1
            'loDroppedNode.ShadowOffsetY = 1
            'loDroppedNode.ShadowColor = Color.LightGray
            loDroppedNode.Pen.Width = 0.5 '0.4
            loDroppedNode.Pen.Color = Color.Navy
            loDroppedNode.ToolTip = "Node"
            loDroppedNode.Text = Me.Name

            Me.Diagram.Nodes.Add(loDroppedNode)

            Me.Shape = loDroppedNode

        End Sub

    End Class

End Namespace