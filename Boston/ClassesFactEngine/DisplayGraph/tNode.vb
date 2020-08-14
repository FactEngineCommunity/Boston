Imports MindFusion.Diagramming

Namespace FactEngine.DisplayGraph
    Public Class Node
        Implements IEquatable(Of FactEngine.DisplayGraph.Node)

        Private Diagram As MindFusion.Diagramming.Diagram = Nothing

        Public Table As RDS.Table
        Public Column As New List(Of RDS.Column)

        Public Type As String = "" 'E.g. 'Person', 'Lecturer', 'TimetableBooking'. I.e. As in the name of the RDS.Table represented by the Node.
        Public [Alias] As String = "" 'E.g. As in '1' for 'Person 1' in Projected Columns of the Query.
        Public Name As String = "Dummy" 'As in the extrapolated Alternate/PrimaryKey value for the Table represented by the Node. E.g. 'Peter Stevens' for alternate key, FirstName, LastName.

        Public Data As New List(Of String) 'E.g. Stores 'Peter' and 'Lecturer' for a Node with a compound reference scheme.

        Public Shape As MindFusion.Diagramming.ShapeNode = Nothing

        Public Edge As New List(Of FactEngine.DisplayGraph.Edge)

        Public X As Integer
        Public Y As Integer

        Public Sub New(ByRef arDiagram As MindFusion.Diagramming.Diagram,
                       ByVal arTable As RDS.Table,
                       ByVal aarColumn As List(Of RDS.Column),
                       ByVal asType As String,
                       ByVal asName As String,
                       ByVal asAlias As String,
                       ByVal aarLink As List(Of FactEngine.DisplayGraph.Edge)
                       )

            Me.Diagram = arDiagram
            Me.Table = arTable
            Me.Column = aarColumn
            Me.Type = asType
            Me.Name = asName
            Me.Alias = asAlias
            Me.Edge = aarLink

        End Sub

        Public Shadows Function Equals(other As Node) As Boolean Implements IEquatable(Of Node).Equals
            Return Me.Type = other.Type And
                   Me.Alias = other.Alias And
                   Me.Name = other.Name
        End Function

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
            loDroppedNode.ToolTip = Me.Type

            loDroppedNode.Text = Me.Name

            Me.Diagram.Nodes.Add(loDroppedNode)

            Me.Shape = loDroppedNode

        End Sub

    End Class

End Namespace