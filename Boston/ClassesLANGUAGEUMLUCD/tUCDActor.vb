Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace UCD
    <Serializable()>
    Public Class Actor
        Inherits UML.Actor 'NB UML.Actor is common to many languages, but methods, members can be overridden.
        Implements FBM.iPageObject

        Public Shadows WithEvents CMMLActor As CMML.Actor

        Public Sub New()
            '-----------------------------------
            'Default Parameterless constructor
            '-----------------------------------
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arCMMLActor As CMML.Actor)

            Me.Model = arPage.Model
            Me.Page = arPage
            Me.CMMLActor = arCMMLActor
            Me.FactData.Model = arPage.Model
            Me.Data = "New Actor"
            Me.Id = Me.Data
            Me.Name = arCMMLActor.Name
            Me.Symbol = Me.Data
            Me.Concept = New FBM.Concept(Me.Data)

        End Sub

        Public Overrides Sub DisplayAndAssociate()

            Dim loDroppedNode As ShapeNode

            Try
                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
                loDroppedNode.Shape = Shapes.RoundRect
                loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                loDroppedNode.ToolTip = "Actor"
                loDroppedNode.Visible = True
                loDroppedNode.Image = My.Resources.CMML.actor
                loDroppedNode.Pen.Color = Color.White
                loDroppedNode.ShadowColor = Color.White

                loDroppedNode.Tag = New CMML.Actor
                loDroppedNode.Resize(10, 15)

                loDroppedNode.Tag = Me

                Me.Shape = loDroppedNode

                '-----------------------------------------
                'Establish the Name caption for the Actor
                '-----------------------------------------
                Dim StringSize As New SizeF
                Dim loActorNameShape As New ShapeNode

                StringSize = Me.Page.Diagram.MeasureString("[" & Trim(Me.Name) & "]", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                Dim liNameX = loDroppedNode.Bounds.X - (((StringSize.Width + 5) - loDroppedNode.Bounds.Width) / 2)

                Dim lr_rectanglef As New RectangleF(liNameX, loDroppedNode.Bounds.Bottom, StringSize.Width + 5, StringSize.Height + 3)
                loActorNameShape = Me.Page.Diagram.Factory.CreateShapeNode(lr_rectanglef, MindFusion.Diagramming.Shapes.Rectangle)
                loActorNameShape.HandlesStyle = HandlesStyle.Invisible
                loActorNameShape.TextColor = Color.Black
                loActorNameShape.Transparent = True
                loActorNameShape.Visible = True
                loActorNameShape.Text = Me.Name
                loActorNameShape.ZTop()
                loActorNameShape.Tag = Me.NameShape
                Me.NameShape.Shape = loActorNameShape

                '-----------------------------------------------------------
                'Attach the Actor.Name ShapeNode to the Actor Shape
                '-----------------------------------------------------------
                loActorNameShape.AttachTo(loDroppedNode, AttachToNode.BottomCenter)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                          Optional ByVal asSelectedGridItemLabel As String = "")

            Dim lsMessage As String = ""

            Try
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"

                            Dim lrModelElement = Me.Model.GetModelObjectByName(Me.Name, True)
                            Dim lbModelElementExists As Boolean = Me.CMMLActor.FBMModelElement IsNot lrModelElement
                            If lbModelElementExists Or (Me.Model.UML.Actor.Find(Function(x) x IsNot Me.CMMLActor And Trim(x.Name) = Trim(Me.Name)) IsNot Nothing) Then

                                lsMessage = "You cannot set the name of a Actor as the same as the name of another Actor in the model."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "A Actor with the name, '" & Me.Name & "', already exists in the model."

                                Me.Name = Me.CMMLActor.Name

                                MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                                Exit Sub
                            Else
                                Call Me.CMMLActor.setName(Me.Name)
                            End If
                    End Select
                End If

                If Me.Shape IsNot Nothing Then
                    Me.NameShape.Shape.Text = Me.Name
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated
            Try
                Me.Data = MyBase.FactData.Data
                Call Me.UpdateGUIFromModel()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Friend Shadows Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.NameShape) Then
                        If Me.NameShape.Shape.Text <> "" Then
                            Me.NameShape.Shape.Text = Trim(Me.FactData.Data)

                            '----------------------------------------
                            'Setup the ActorName shape size
                            '----------------------------------------                    
                            Dim StringSize As New SizeF
                            Dim G As Graphics

                            G = Me.Page.Form.CreateGraphics
                            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.FactData.Data), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            StringSize.Height += 2

                            Me.NameShape.Shape.SetRect(New RectangleF(Me.NameShape.Shape.Bounds.X, Me.NameShape.Shape.Bounds.Y, StringSize.Width, StringSize.Height), True)

                            Me.Page.Diagram.Invalidate()

                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("tActor.UpdateGUIFromModel: " & lo_err.Message & "FactTypeId: " & Me.Role.FactType.Id & ", ValueSymbol:" & Me.Concept.Symbol & ", PageId:" & Me.Page.PageId)
            End Try

        End Sub

        Public Overrides Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean,
                                  Optional ByVal abMakeDirty As Boolean = True) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Try

                Me.FactDataInstance.X = aiNewX
                Me.FactDataInstance.Y = aiNewY

                Me.FactDataInstance.Fact.FactType.isDirty = True
                Me.FactDataInstance.Fact.isDirty = True
                Me.FactDataInstance.isDirty = True

                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Try
                Me.FactDataInstance.Page.MakeDirty()
            Catch ex As Exception
            End Try

        End Sub
        Public Overloads Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Overloads Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

            Try
                If IsSomething(Me.Shape) Then
                    If Me.Shape.Selected Then
                        Me.Shape.Pen.Color = Color.Blue
                    Else
                        Me.Shape.Pen.Color = Color.White
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function setName(asNewName As String, Optional abBroadcastInterfaceEvent As Boolean = True, Optional abSuppressModelSave As Boolean = False) As Boolean

            Try
                Call Me.CMMLActor.FBMModelElement.setName(asNewName, abBroadcastInterfaceEvent, abSuppressModelSave)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Private Sub CMMLActor_NameChanged(asNewName As String) Handles CMMLActor.NameChanged

            Try
                Me.Name = asNewName

                If Me.NameShape.Shape IsNot Nothing Then
                    Me.NameShape.Shape.Text = asNewName
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function RemoveFromPage() As Boolean

            Try
                Dim lsSQLQuery As String

                '----------------------------------------------------------------------------------------------------------
                'Remove the Actor that represents the Actor from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Page.UMLDiagram.Actor.Remove(Me)

                Dim larLinkToRemove As New List(Of DiagramLink)

                If Me.Page.Diagram IsNot Nothing Then

                    Me.Page.Diagram.Nodes.Remove(Me.Shape)
                    Me.Page.Diagram.Nodes.Remove(Me.NameShape.Shape)

                    For Each lrLink In Me.Shape.OutgoingLinks
                        larLinkToRemove.Add(lrLink)
                    Next
                    For Each lrLink In larLinkToRemove
                        Me.Page.Diagram.Links.Remove(lrLink)
                    Next
                End If

                '-------------------------------------------------------------------------
                'Remove the Actor from the Page
                '---------------------------------
#Region "CMML"
                'Likely already deleted when deleted at the Model level.
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & Me.Name & "'"
                lsSQLQuery &= "   AND ElementType = 'Actor'"

                Call Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Private Sub CMMLActor_RemovedFromModel() Handles CMMLActor.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

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