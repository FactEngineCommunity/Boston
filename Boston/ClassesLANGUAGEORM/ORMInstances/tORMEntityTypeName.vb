Imports System.ComponentModel
Imports MindFusion.Diagramming

Namespace FBM
    <Serializable()> Public Class EntityTypeName
        Inherits FBM.PageObject
        Implements FBM.iPageObject

        Public EntityTypeInstance As FBM.EntityTypeInstance

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Sub New()

            Me.ConceptType = pcenumConceptType.EntityTypeName

        End Sub

        Public Sub New(ByRef arEntityTypeInstance As FBM.EntityTypeInstance)

            Me.New()
            Me.EntityTypeInstance = arEntityTypeInstance
        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Return New FBM.EntityTypeName

        End Function

        Public Overloads Sub RefreshShape()

            Me.Shape.Text = Me.EntityTypeInstance.Name
            Me.EntityTypeInstance.Page.Diagram.Invalidate()

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour
            If IsSomething(Me.Shape) Then
                Me.Shape.Pen.Color = Color.White
            End If
        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace