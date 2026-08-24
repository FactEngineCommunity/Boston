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
        Public DataItemsHaveBeenAdded As Boolean = False

        Public Shape As MindFusion.Diagramming.ShapeNode = Nothing

        Public Edge As New List(Of FactEngine.DisplayGraph.Edge)

        Public OrdinalPosition As Integer 'To set the color by using Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.FromArgb(Values(lrArgument.SequenceNr Mod 6)))
        Public Color As Color

        Public X As Integer
        Public Y As Integer

        Private Graphics As Graphics

        Public Sub New(ByRef arDiagram As MindFusion.Diagramming.Diagram,
                       ByVal arTable As RDS.Table,
                       ByVal aarColumn As List(Of RDS.Column),
                       ByVal asType As String,
                       ByVal asName As String,
                       ByVal asAlias As String,
                       ByVal aarLink As List(Of FactEngine.DisplayGraph.Edge),
                       Optional ByRef arGraphics As Graphics = Nothing
                       )

            Me.Diagram = arDiagram
            Me.Table = arTable
            Me.Column = aarColumn
            Me.Type = asType
            Me.Name = asName
            Me.Alias = asAlias
            Me.Edge = aarLink
            Me.Graphics = arGraphics


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
            loDroppedNode.Resize(15, 15)
            'loDroppedNode.Tag = New FBM.EntityTypeInstance
            loDroppedNode.Tag = Me
            loDroppedNode.ShadowColor = Color.White
            'loDroppedNode.ShadowOffsetX = 1
            'loDroppedNode.ShadowOffsetY = 1
            'loDroppedNode.ShadowColor = Color.LightGray
            loDroppedNode.Pen.Width = 0.3 '0.4
            loDroppedNode.Pen.Color = Color.Navy
            loDroppedNode.ToolTip = Me.Type
            loDroppedNode.AllowOutgoingLinks = False
            loDroppedNode.Shape.Image = Nothing

#Region "Random Pastel Colour"
            Dim random As New Random()

            ' Generate random RGB values within the pastel range (180 to 255)
            Dim red As Integer = random.Next(180, 256)
            Dim green As Integer = random.Next(180, 256)
            Dim blue As Integer = random.Next(180, 256)
#End Region
            loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(PastelColourGenerator.GenerateRandomPastelColor)

            loDroppedNode.Text = Me.Name

            Me.Diagram.Nodes.Add(loDroppedNode)

#Region "Data"
            If Me.Graphics IsNot Nothing And Me.Data.Count > 0 Then
                Dim lsDataString As String = ""

                Dim liInd = 0
                For Each lsData In Me.Data
                    If liInd > 0 Then lsDataString &= ", "
                    lsDataString &= lsData
                    liInd += 1
                Next

                Dim lrFont = Me.Diagram.Font
                Dim StringSize = Me.Diagram.MeasureString(lsDataString, lrFont, 1000, System.Drawing.StringFormat.GenericDefault)

                Dim liX, liY As Integer
                liX = loDroppedNode.Bounds.X
                liY = loDroppedNode.Bounds.Y + loDroppedNode.Bounds.Height + 2 'Shape.Bounds.Y

                'For Y = liY + (StringSize.Height + 8)
                Dim loDataDroppedNode = Me.Diagram.Factory.CreateShapeNode(liX, liY, StringSize.Width + 1, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)
                loDataDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDataDroppedNode.Text = Trim(lsDataString)
                loDataDroppedNode.TextColor = Color.Black ' 20230603-VM-Was Color.Blue
                loDataDroppedNode.Transparent = True
                loDataDroppedNode.Font = Me.Diagram.Font
                loDataDroppedNode.ResizeToFitText(FitSize.KeepHeight)
                loDataDroppedNode.ZTop()
                loDataDroppedNode.Tag = Me

                '---------------------------------------------------------------------------
                'Attach the FactTypeName ShapeNode to the FactTypeInstance ShapeNode
                '---------------------------------------------------------------------------
                loDataDroppedNode.AttachTo(loDroppedNode, AttachToNode.MiddleRight)
            End If
#End Region

            Me.Shape = loDroppedNode

        End Sub

    End Class

End Namespace