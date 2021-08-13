Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace DFD
    <Serializable()> _
    Public Class DataStore
        Inherits FBM.FactDataInstance

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public Sub New()

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asDataStoreName As String)

            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.SetName(asDataStoreName)            


        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Me.Page = arPage
            Me.Role = arRoleInstance
            Me.Concept = ar_concept

            '------------------------------------
            'link the RoleData back to the Model
            '------------------------------------
            Dim lrRole As FBM.Role = arRoleInstance.Role
            Dim lrRole_data As New FBM.FactData(arRoleInstance.Role, ar_concept)
            lrRole_data = lrRole.Data.Find(AddressOf lrRole_data.Equals)
            Me.FactData = lrRole_data

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept, ByVal aiX As Integer, ByVal aiY As Integer)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Call Me.New(arPage, arRoleInstance, ar_concept)
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Shadows Function EqualsByName(ByVal other As DFD.DataStore) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub DisplayAndAssociate(Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Dim loDroppedNode As New ShapeNode
            '--------------------------------------------------------------------
            'Create a Shape for the EntityTypeInstance on the DiagramView object
            '--------------------------------------------------------------------            
            loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
            loDroppedNode.Shape = Shapes.RoundRect
            loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
            loDroppedNode.AllowOutgoingLinks = True
            loDroppedNode.AllowIncomingLinks = True
            loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
            loDroppedNode.Text = Me.Name
            loDroppedNode.Transparent = False
            loDroppedNode.ShadowColor = Color.White
            loDroppedNode.Resize(20, 15)
            loDroppedNode.Tag = New DFD.DataStore
            loDroppedNode.Tag = Me
            loDroppedNode.Obstacle = True

            ' VB.NET ShapeTemplate definition
            loDroppedNode.Shape = New Shape( _
                                     New ElementTemplate() _
                                     { _
                                        New LineTemplate(0, 0, 100, 0, Color.White, Nothing, 1), _
                                        New LineTemplate(100, 0, 100, 100, Color.White, Nothing, 1), _
                                        New LineTemplate(100, 100, 0, 100, Color.White, Nothing, 1), _
                                        New LineTemplate(0, 100, 0, 0, Color.White, Nothing, 1) _
                                     }, _
                                     New ElementTemplate() _
                                     { _
                                          New LineTemplate(2, 4, 98, 4), _
                                          New LineTemplate(1, 97, 98, 97) _
                                     }, _
                                     New ElementTemplate() _
                                     { _
                                          New LineTemplate(5, 5, 95, 5), _
                                          New LineTemplate(95, 5, 95, 90), _
                                          New LineTemplate(95, 90, 5, 90), _
                                          New LineTemplate(5, 90, 5, 5) _
                                     }, _
                                     FillMode.Winding, "test")

            Dim StringSize As New SizeF
            Dim loActorNameShape As New ShapeNode

            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name) & "__", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            loDroppedNode.Resize(StringSize.Width, 5)

            Me.shape = loDroppedNode

            If IsSomething(aoContainerNode) Then
                aoContainerNode.Add(loDroppedNode)
            End If

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Dim StringSize As New SizeF

            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name) & "__", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            Me.Shape.Resize(StringSize.Width, 5)


            Me.Shape.Text = Me.Name

            Call Me.setName(Me.Name)

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated
            Try
                Call Me.UpdateGUIFromModel()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Friend Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.shape) Then
                        If Me.shape.Text <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.shape.Text = Trim(Me.FactData.Data)
                            Call Me.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("t_Entity.update_from_model: " & lo_err.Message) '& "FactTypeId: " & Me.role.FactType.FactTypeId & ", ValueSymbol:" & Me.concept.Symbol & ", PageId:") ' & Me.Page.PageId)
            End Try

        End Sub


    End Class

End Namespace