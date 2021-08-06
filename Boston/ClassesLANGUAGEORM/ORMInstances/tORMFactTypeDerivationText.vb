Imports System.Xml.Serialization
Imports System.Reflection
Imports MindFusion.Diagramming

Namespace FBM
    <Serializable()> _
    Public Class FactTypeDerivationText
        Inherits FBM.PageObject
        Implements FBM.iPageObject

        <XmlIgnore()> _
        Public FactType As New FBM.FactType

        <XmlIgnore()> _
        Public FactTypeInstance As FBM.FactTypeInstance

        Public Shadows Property X As Integer Implements iPageObject.X
        Public Shadows Property Y As Integer Implements iPageObject.Y

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.ConceptType = pcenumConceptType.FactTypeDerivationText
        End Sub


        Public Sub New(ByRef arModel As FBM.Model, ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage
            Me.FactTypeInstance = arFactTypeInstance
            Me.FactType = arFactTypeInstance.FactType

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance) As FBM.FactTypeDerivationText

            Dim lrFactTypeDerivationText As New FBM.FactTypeDerivationText

            With Me

                lrFactTypeDerivationText.Model = arPage.Model
                lrFactTypeDerivationText.Page = arPage
                lrFactTypeDerivationText.Name = .Name
                lrFactTypeDerivationText.FactTypeInstance = arFactTypeInstance
                lrFactTypeDerivationText.FactType = arFactTypeInstance.FactType
                lrFactTypeDerivationText.X = .X
                lrFactTypeDerivationText.Y = .Y

            End With

            Return lrFactTypeDerivationText

        End Function

        Public Sub displayAndAssociate()

            Dim StringSize As SizeF
            Dim lsDerivationText As String
            Dim loFactTypeDerivationTextShape As ShapeNode

            If Me.FactType.IsManyTo1BinaryFactType Then
                '-----------------------------------------------------------------------------------------------
                'That's good, because needs to be at least that for Derived Fact Type.
                Dim lrRole As FBM.Role
                lrRole = Me.FactType.GetFirstRoleWithInternalUniquenessConstraint
                lsDerivationText = "* <b>For each</b> " & lrRole.JoinedORMObject.Name
                lsDerivationText &= vbCrLf & Me.FactType.DerivationText
            Else
                lsDerivationText = "* " & Me.FactType.DerivationText
            End If

            StringSize = Me.Page.Diagram.MeasureString(Trim(lsDerivationText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            StringSize.Height += 2

            If StringSize.Width > 70 Then
                StringSize = New SizeF(70, (StringSize.Height + 2) * lsDerivationText.Length / 100)
            End If

            loFactTypeDerivationTextShape = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y + Me.Shape.Bounds.Height + 5, StringSize.Width, StringSize.Height)
            loFactTypeDerivationTextShape.Shape = MindFusion.Diagramming.Shapes.Rectangle
            loFactTypeDerivationTextShape.HandlesStyle = HandlesStyle.MoveOnly
            loFactTypeDerivationTextShape.EnableStyledText = True
            loFactTypeDerivationTextShape.Locked = False
            loFactTypeDerivationTextShape.TextFormat.Alignment = StringAlignment.Near
            loFactTypeDerivationTextShape.Text = lsDerivationText
            Call loFactTypeDerivationTextShape.ResizeToFitText(FitSize.KeepWidth)
            loFactTypeDerivationTextShape.TextColor = Color.Black
            loFactTypeDerivationTextShape.Transparent = True
            loFactTypeDerivationTextShape.AllowIncomingLinks = False
            loFactTypeDerivationTextShape.AllowOutgoingLinks = False
            loFactTypeDerivationTextShape.ZTop()

            'Me.FactTypeDerivationText = New FBM.FactTypeDerivationText(Me.Model, Me.Page, Me)
            Me.Shape = loFactTypeDerivationTextShape
            loFactTypeDerivationTextShape.Tag = Me

            If Me.X = 0 Then Me.X = Me.X
            If Me.Y = 0 Then Me.Y = Me.Y + Me.Shape.Bounds.Height + 5

            Me.Shape.Move(Me.X, Me.Y)

            Me.Page.Diagram.Nodes.Add(Me.Shape)

            Me.Shape.Visible = Me.FactType.IsDerived
            Call Me.Shape.ZBottom()


        End Sub

        Public Sub RemoveFromPage()

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactTypeInstance.Name, pcenumConceptType.FactTypeDerivationText)

            Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '--------------------------------------------------
            'Saves the FactTypeDerivationText to the database
            '--------------------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            Try
                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.FactTypeInstance.Name
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.FactTypeDerivationText
                lrConceptInstance.Visible = Me.FactTypeInstance.IsDerived


                '--------------------------------------------------
                'Make sure the new Symbol is in the Concept table
                '--------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.FactTypeInstance.Name)
                lrConcept.Save()

                If abRapidSave Then
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                Else
                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

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

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace
