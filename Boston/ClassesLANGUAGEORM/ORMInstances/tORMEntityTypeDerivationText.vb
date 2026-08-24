Imports System.Xml.Serialization
Imports System.Reflection
Imports MindFusion.Diagramming

Namespace FBM
    <Serializable()>
    Public Class EntityTypeDerivationText
        Inherits FBM.PageObject
        Implements FBM.iPageObject

        <XmlIgnore()>
        Public EntityType As New FBM.EntityType

        <XmlIgnore()>
        Public EntityTypeInstance As FBM.EntityTypeInstance

        Public Shadows Property X As Integer Implements iPageObject.X
        Public Shadows Property Y As Integer Implements iPageObject.Y

        Public [RichTextBox] As RichTextBox = Nothing

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.ConceptType = pcenumConceptType.EntityTypeDerivationText
        End Sub


        Public Sub New(ByRef arModel As FBM.Model, ByRef arPage As FBM.Page, ByRef arEntityTypeInstance As FBM.EntityTypeInstance)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage
            Me.EntityTypeInstance = arEntityTypeInstance
            Me.EntityType = arEntityTypeInstance.EntityType

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arEntityTypeInstance As FBM.EntityTypeInstance) As FBM.EntityTypeDerivationText

            Dim lrEntityTypeDerivationText As New FBM.EntityTypeDerivationText

            With Me

                lrEntityTypeDerivationText.Model = arPage.Model
                lrEntityTypeDerivationText.Page = arPage
                lrEntityTypeDerivationText.Name = .Name
                lrEntityTypeDerivationText.EntityTypeInstance = arEntityTypeInstance
                lrEntityTypeDerivationText.EntityType = arEntityTypeInstance.EntityType
                lrEntityTypeDerivationText.X = .X
                lrEntityTypeDerivationText.Y = .Y

            End With

            Return lrEntityTypeDerivationText

        End Function

        Public Sub displayAndAssociate()

            Try

                Dim StringSize As SizeF
                Dim lsDerivationText As String
                Dim loEntityTypeDerivationTextShape As ShapeNode

                lsDerivationText = "* " & Me.EntityType.DerivationText


                StringSize = Me.Page.Diagram.MeasureString(Trim(lsDerivationText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                StringSize.Height += 2

                If StringSize.Width > 70 Then
                    StringSize = New SizeF(70, (StringSize.Height + 2) * lsDerivationText.Length / 100)
                End If

                loEntityTypeDerivationTextShape = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y + Me.EntityTypeInstance.Shape.Bounds.Height + 5, StringSize.Width, StringSize.Height)
                loEntityTypeDerivationTextShape.Shape = MindFusion.Diagramming.Shapes.Rectangle
                loEntityTypeDerivationTextShape.HandlesStyle = HandlesStyle.MoveOnly
                loEntityTypeDerivationTextShape.EnableStyledText = True
                loEntityTypeDerivationTextShape.Locked = False
                loEntityTypeDerivationTextShape.TextFormat.Alignment = StringAlignment.Near
                loEntityTypeDerivationTextShape.Text = lsDerivationText
                Call loEntityTypeDerivationTextShape.ResizeToFitText(FitSize.KeepWidth)
                loEntityTypeDerivationTextShape.TextColor = Color.Black
                loEntityTypeDerivationTextShape.Transparent = True
                loEntityTypeDerivationTextShape.AllowIncomingLinks = False
                loEntityTypeDerivationTextShape.AllowOutgoingLinks = False
                loEntityTypeDerivationTextShape.ZTop()

                'Me.EntityTypeDerivationText = New FBM.EntityTypeDerivationText(Me.Model, Me.Page, Me)
                Me.Shape = loEntityTypeDerivationTextShape
                loEntityTypeDerivationTextShape.Tag = Me

                If Me.X = 0 Then Me.X = Me.X
                If Me.Y = 0 Then Me.Y = Me.Y + Me.Shape.Bounds.Height + 5

                Me.Shape.Move(Me.X, Me.Y)

                Me.Page.Diagram.Nodes.Add(Me.Shape)

                Me.Shape.Visible = Me.EntityType.IsDerived
                Call Me.Shape.ZBottom()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub RemoveFromPage()

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.EntityTypeInstance.Name, pcenumConceptType.EntityTypeDerivationText)

            Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '--------------------------------------------------
            'Saves the EntityTypeDerivationText to the database
            '--------------------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            Try
                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.EntityTypeInstance.Name
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.EntityTypeDerivationText
                lrConceptInstance.Visible = Me.EntityTypeInstance.IsDerived


                '--------------------------------------------------
                'Make sure the new Symbol is in the Concept table
                '--------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.EntityTypeInstance.Name)
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Overloads Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Overloads Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Overloads Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Overloads Sub Move(aiNewX As Integer, aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean,
                                   Optional ByVal abMakeDirty As Boolean = True) Implements iPageObject.Move

        End Sub

        Public Overloads Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Overloads Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Overloads Sub NodeDeselected() Implements iPageObject.NodeDeselected

        End Sub

        Public Overloads Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Overloads Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Overloads Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Overloads Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub SetSize()

            Try
                'CodeSafe
                If Me.Page.Diagram Is Nothing Then Exit Sub 'What are you going to do?

                Dim StringSize = Me.Page.Diagram.MeasureString(Trim(Me.EntityType.DerivationText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                StringSize.Height += 2

                If StringSize.Width > 70 Then
                    StringSize = New SizeF(70, (StringSize.Height + 2) * Me.EntityType.DerivationText.Length / 100)
                End If

                Me.Shape.Resize(StringSize.Width, StringSize.Height)
                Me.Shape.ResizeToFitText(FitSize.KeepWidth)
                Me.Shape.Move(Me.X, Me.Y)

                If Me.RichTextBox IsNot Nothing Then
                    'See Also: DiagramView.EnterInplaceEditMode
                    'See Also: DiagramView.CreateEditControl
                    StringSize = Me.Page.Diagram.MeasureString(Trim(Me.EntityType.DerivationText), New Font("Tahoma", 10), 1000, System.Drawing.StringFormat.GenericDefault)
                    StringSize.Height += 2

                    If StringSize.Width > 70 Then
                        StringSize = New SizeF(70, (StringSize.Height + 2) * Me.EntityType.DerivationText.Length / 100)
                    End If

                    Dim StringSizeF = Me.Page.DiagramView.DocToClient(New Rectangle(Me.Shape.Bounds.X, Me.Shape.Bounds.Y, StringSize.Width + 2, StringSize.Height))

                    Me.RichTextBox.Width = StringSizeF.Width
                    Me.RichTextBox.Height = StringSizeF.Height
                    Me.RichTextBox.Top = StringSizeF.Y

                    'OR=======================================================
                    ' Set WordWrap to True so that we measure the height considering the current width of the RichTextBox
                    RichTextBox.WordWrap = True

                    ' Perform the layout of the text
                    RichTextBox.PerformLayout()

                    ' Get the number of lines and the height of one line
                    Dim numberOfLines As Integer = RichTextBox.GetLineFromCharIndex(RichTextBox.TextLength) + 1
                    Dim oneLineHeight As Integer = RichTextBox.Font.Height + 5

                    ' Calculate the total height required for the text
                    Dim requiredHeight As Integer = oneLineHeight * numberOfLines

                    ' Check if the required height is different from the current height before resizing
                    If requiredHeight <> RichTextBox.Height Then
                        RichTextBox.Height = requiredHeight
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Public Overloads Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

    End Class

End Namespace
