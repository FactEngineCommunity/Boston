Imports System.ComponentModel
Imports System.Collections.Specialized
Imports MindFusion.Diagramming

Namespace FBM
    <Serializable()> _
    Public Class ValueConstraintInstance
        Inherits FBM.PageObject
        Implements FBM.iPageObject

        Public ValueType As FBM.ValueTypeInstance
        Public EntityType As FBM.EntityTypeInstance

        Public Text As String = ""

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Sub New()

            Me.ConceptType = pcenumConceptType.ValueConstraint

        End Sub

        Sub New(ByVal arValueTypeInstance As FBM.ValueTypeInstance, Optional ByRef arEntityTypeInstance As FBM.EntityTypeInstance = Nothing)

            Call Me.new()

            Me.Page = arValueTypeInstance.Page
            Me.ValueType = arValueTypeInstance

            If IsSomething(arEntityTypeInstance) Then
                Me.EntityType = arEntityTypeInstance
            End If

        End Sub

        Public Sub DisplayAndAssociate(ByRef arModelObject As FBM.ModelObject)

            Dim loValueConstraintShapeNode As ShapeNode

            '----------------------------------------
            'Setup the ValueConstraint
            '----------------------------------------                        
            Dim StringSize As New SizeF
            Dim G As Graphics
            Dim lsEnumeratedValueConstraint As String = Me.ValueType.EnumerateValueConstraint 'The ValueTypeConstraint enumerated e.g. "1,2,3,4" or "value1, value2, value3" etc

            G = Me.Page.Form.CreateGraphics

            If lsEnumeratedValueConstraint = "" Then
                StringSize = Me.Page.Diagram.MeasureString("", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            Else
                StringSize = Me.Page.Diagram.MeasureString("{" & Trim(lsEnumeratedValueConstraint) & "}", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            End If

            If IsSomething(Me.EntityType) Then
                loValueConstraintShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.EntityType.Shape.Bounds.X, Me.EntityType.Shape.Bounds.Y - (StringSize.Height * 2), StringSize.Width, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)
            Else
                loValueConstraintShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.ValueType.Shape.Bounds.X, Me.ValueType.Shape.Bounds.Y - (StringSize.Height * 2), StringSize.Width, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)
            End If

            If lsEnumeratedValueConstraint = "" Then
                loValueConstraintShapeNode.Text = ""
            Else
                loValueConstraintShapeNode.Text = "{" & Trim(lsEnumeratedValueConstraint) & "}"
                loValueConstraintShapeNode.HandlesStyle = HandlesStyle.InvisibleMove
                loValueConstraintShapeNode.Resize(StringSize.Width, StringSize.Height)

                loValueConstraintShapeNode.TextColor = Color.Maroon
                loValueConstraintShapeNode.Transparent = True
                loValueConstraintShapeNode.Visible = True
                loValueConstraintShapeNode.ZTop()
            End If

            loValueConstraintShapeNode.Tag = Me

            '--------------------------------------------------------------------------------------------------
            'Attach the ValueConstraintInstance ShapeNode to the EntityType or ValueType to which it belongs.
            '--------------------------------------------------------------------------------------------------
            If IsSomething(Me.EntityType) Then
                If IsSomething(Me.EntityType.Shape) Then
                    loValueConstraintShapeNode.AttachTo(Me.EntityType.Shape, AttachToNode.TopLeft)
                End If
            Else
                loValueConstraintShapeNode.AttachTo(Me.ValueType.Shape, AttachToNode.TopLeft)
                If Me.ValueType.IsReferenceMode Then
                    Me.Shape.Visible = False
                Else
                    Me.Shape.Visible = True
                End If
            End If

            Me.Shape = loValueConstraintShapeNode

        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            Dim lo_Diagram As New MindFusion.Diagramming.Diagram
            Dim StringSize As New SizeF

            lo_Diagram = Me.Shape.Parent

            If IsSomething(lo_Diagram) Then
                '-------------
                'Go forward
                '-------------
            Else
                '------------------------------------------------------------------------
                'Exit, because there is no ValueContraint yet set for the ValueType
                '  so nothing to display. The ShapeNode is not associated with the
                '  ValueConstraint until there is something to display, or after
                '  something has been displayed. No harm in exiting here
                '------------------------------------------------------------------------
                Exit Sub
            End If

            '-----------------------------------------------------------------------------------
            'The ValueTypeConstraint enumerated e.g. "1,2,3,4", "1..2,20..22" or "value1, value2, value3" etc
            '-----------------------------------------------------------------------------------
            Dim ls_enumerated_value_constraint As String = Me.ValueType.EnumerateValueConstraint

            If (ls_enumerated_value_constraint = "") Then
                StringSize = lo_Diagram.MeasureString("", lo_Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            Else
                StringSize = lo_Diagram.MeasureString("{" & Trim(ls_enumerated_value_constraint) & "}", lo_Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            End If

            '--------------------------------------------------------------
            'ValueConstraint is already displayed for the ValueTypeInstance
            '--------------------------------------------------------------
            If ls_enumerated_value_constraint = "" Then
                Me.Shape.Text = ""
            Else
                Me.Shape.Text = "{" & Trim(ls_enumerated_value_constraint) & "}"
                Me.Shape.Resize(StringSize.Width, StringSize.Height)

                Me.Shape.TextColor = Color.Maroon
                Me.Shape.Transparent = True
                Me.Shape.Visible = True
                Me.Shape.ZTop()

            End If

            If (Me.Shape.Bounds.Y >= Me.ValueType.Shape.Bounds.Y) And (Me.Shape.Bounds.Y <= Me.ValueType.Shape.Bounds.Y) Then
                Me.Shape.Move(Me.Shape.Bounds.X, Me.ValueType.Shape.Bounds.Y - 8)
            End If

            lo_Diagram.Invalidate()

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

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

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace