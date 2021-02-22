Imports Boston.FBM
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Reflection

Namespace STD

    Public Class EndStateIndicator
        Inherits FBM.FactDataInstance
        Implements FBM.iPageObject
        Implements IEquatable(Of STD.EndStateIndicator)

        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.EndStateIndicator

        Public EndStateId As String = "" 'The unique identifier for an EndState.        

        Public Link As DiagramLink

        Public WithEvents STMEndStateIndicator As STM.EndStateIndicator

        Public Shadows Property X As Integer Implements iPageObject.X
            Get
                Return Me._X
            End Get
            Set(value As Integer)
                Me._X = value
            End Set
        End Property

        Public Shadows Property Y As Integer Implements iPageObject.Y
            Get
                Return Me._Y
            End Get
            Set(value As Integer)
                Me._Y = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arPage As FBM.Page)
            Me.Model = arPage.Model
            Me.Page = arPage
            Me.EndStateId = System.Guid.NewGuid.ToString
        End Sub

        Public Shadows Function Equals(other As EndStateIndicator) As Boolean Implements IEquatable(Of EndStateIndicator).Equals
            Return Me.EndStateId = other.EndStateId
        End Function

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            If Me.FactDataInstance IsNot Nothing Then
                Me.FactDataInstance.X = aiNewX
                Me.FactDataInstance.Y = aiNewY
                Me.FactDataInstance.makeDirty()
            End If

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

            Call Me.Move(Me.Shape.Bounds.X, Me.Shape.Bounds.Y, True)

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Throw New NotImplementedException()
        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour
            Throw New NotImplementedException()
        End Sub

        Public Sub DisplayAndAssociate()

            '=====================================================================            
            'Create a ShapeNode on the Page for the PGS Node
            '----------------------------------------------            
            Dim loDroppedNode As ShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 10, 10, Shapes.Ellipse)

            loDroppedNode.Resize(10, 10)
            loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
            loDroppedNode.Pen.Width = 0.5
            loDroppedNode.Pen.Color = Color.Black

            loDroppedNode.Brush = New SolidBrush(Color.White)
            loDroppedNode.ShadowColor = Color.White
            loDroppedNode.Expandable = False
            loDroppedNode.Obstacle = True
            loDroppedNode.AllowIncomingLinks = True
            loDroppedNode.AllowOutgoingLinks = False
            loDroppedNode.Text = "End"

            loDroppedNode.Tag = Me

            Me.Shape = loDroppedNode
            Me.FactDataInstance.Shape = loDroppedNode

            loDroppedNode.Image = My.Resources.ORMShapes.Blank

        End Sub

        Private Sub STMEndStateIndicator_RemovedFromModel() Handles STMEndStateIndicator.RemovedFromModel

            Try
                Me.Page.STDiagram.EndStateIndicator.Remove(Me)

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Nodes.Remove(Me.Shape)
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
