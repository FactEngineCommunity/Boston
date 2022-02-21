Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.ComponentModel

Namespace FBM
    <Serializable()>
    Public Class SubtypeRelationshipInstance
        Inherits FBM.ModelObject
        Implements FBM.iPageObject
        Implements IEquatable(Of FBM.SubtypeRelationshipInstance)

        <XmlAttribute()>
        Public Overrides Property ConceptType As pcenumConceptType
            Get
                Return pcenumConceptType.SubtypeRelationship
            End Get
            Set(value As pcenumConceptType)
                'Nothing to do here
            End Set
        End Property

        ''' <summary>
        ''' The EntityType for which the SubTypeIstance (line) acts as View/Proxy.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows ModelElement As FBM.ModelObject
        Public Shadows parentModelElement As FBM.ModelObject

        ''' <summary>
        ''' The FactTypeInstance of the FactType that represents the Subtype
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows FactType As New FBM.FactTypeInstance

        Public Shadows WithEvents SubtypeRelationship As New FBM.tSubtypeRelationship

        <XmlIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsPrimarySubtypeRelationship As Boolean = False

        <XmlAttribute()>
        <CategoryAttribute("Subtype Relationship"),
        Browsable(True),
        [ReadOnly](False),
        DescriptionAttribute("True if the Primary Subtype Relationship for the Model Element."),
        Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property IsPrimarySubtypeRelationship As Boolean
            Get
                Return Me._IsPrimarySubtypeRelationship
            End Get
            Set(value As Boolean)
                Me._IsPrimarySubtypeRelationship = value
            End Set
        End Property

        <NonSerialized(),
        XmlIgnore()>
        Public Link As DiagramLink

        <NonSerialized(),
        XmlIgnore()>
        Public Page As FBM.Page

        <NonSerialized(),
        XmlIgnore()>
        Public _Shape As ShapeNode
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property


        Public Property X As Integer Implements iPageObject.X
            Get
                'Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                'Throw New NotImplementedException()
            End Set
        End Property

        Public Property Y As Integer Implements iPageObject.Y
            Get
                'Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                'Throw New NotImplementedException()
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arPage As FBM.Page,
                       ByRef ar_entity_type_instance As FBM.EntityTypeInstance,
                       ByRef ar_parentEntityTypeInstance As FBM.EntityTypeInstance)

            Call Me.New()

            Me.Model = arPage.Model
            Me.Page = arPage
            Me.ModelElement = ar_entity_type_instance
            Me.parentModelElement = ar_parentEntityTypeInstance

        End Sub

        Public Overloads Function Equals(other As SubtypeRelationshipInstance) As Boolean Implements IEquatable(Of SubtypeRelationshipInstance).Equals

            Return (Me.ModelElement.Id = other.ModelElement.Id) And (Me.parentModelElement.Id = other.parentModelElement.Id)

        End Function

        ''' <summary>
        ''' PRECONDITIONS: The ObjectType, ParentObjectType and SubtypeRelationshipFactType must already exist on the target Page (arPage)
        ''' </summary>
        ''' <param name="arPage">The target Page to which the SubtypeRelationshipInstance is cloned.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByVal abAddToModel As Boolean) As Object

            Dim lrSubtypeRelationshipInstance As New FBM.SubtypeRelationshipInstance

            Try
                With Me
                    lrSubtypeRelationshipInstance.Page = arPage
                    lrSubtypeRelationshipInstance.Model = arPage.Model
                    lrSubtypeRelationshipInstance.SubtypeRelationship = .SubtypeRelationship.Clone(arPage.Model, abAddToModel)
                    lrSubtypeRelationshipInstance.ModelElement = arPage.getModelElementById(.ModelElement.Id)
                    lrSubtypeRelationshipInstance.parentModelElement = arPage.getModelElementById(.parentModelElement.Id)
                    lrSubtypeRelationshipInstance.FactType = arPage.FactTypeInstance.Find(AddressOf .FactType.Equals)
                End With

                Return lrSubtypeRelationshipInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrSubtypeRelationshipInstance
            End Try

        End Function

        Public Sub DisplayAndAssociate()

            Dim lo_subtype_link As DiagramLink

            Try
                '----------------------------------------------
                'CodeSafe
                If Me.Page.Diagram Is Nothing Then Exit Sub

                '--------------------------------------------------------
                'Create a Link for the SubType on the DiagramView object
                '--------------------------------------------------------        
                Dim loNode As MindFusion.Diagramming.ShapeNode

                'CodeSafe
                If Me.ModelElement Is Nothing Then
                    Me.ModelElement = Me.Page.getModelElementById(Me.SubtypeRelationship.ModelElement.Id)
                End If

                If Me.parentModelElement IsNot Nothing Then
                    If Me.parentModelElement.IsObjectifyingEntityType Then
                        Me.parentModelElement = Me.Page.getModelElementById(CType(Me.SubtypeRelationship.parentModelElement, FBM.EntityType).ObjectifiedFactType.Id)
                    Else
                        Me.parentModelElement = Me.Page.getModelElementById(Me.SubtypeRelationship.parentModelElement.Id)
                    End If
                Else
                    Me.parentModelElement = Me.Page.getModelElementById(Me.SubtypeRelationship.parentModelElement.Id)
                End If

                If IsSomething(Me.parentModelElement) Then

                    If Me.parentModelElement.IsObjectifyingEntityType Then
                        loNode = CType(Me.parentModelElement, FBM.EntityTypeInstance).ObjectifiedFactType.Shape
                    Else
                        loNode = CType(Me.parentModelElement, Object).Shape
                    End If

                    '-------------------------------------------------------------------
                    'CodeSafe: Esp for working with the Core model
                    If (CType(Me.ModelElement, Object).Shape Is Nothing) Or (loNode Is Nothing) Then
                        Exit Sub
                    End If

                    lo_subtype_link = Me.Page.Diagram.Factory.CreateDiagramLink(CType(Me.ModelElement, Object).Shape, loNode)
                    lo_subtype_link.BaseShape = ArrowHead.None
                    lo_subtype_link.HeadShape = ArrowHead.PointerArrow
                    lo_subtype_link.HeadShapeSize = 3.0
                    lo_subtype_link.HeadPen.Color = Color.Purple
                    lo_subtype_link.Pen.Color = Color.Purple 'RGB(121, 0, 121)
                    lo_subtype_link.Pen.Width = 0.5

                    Select Case Me.IsPrimarySubtypeRelationship
                        Case Is = True
                            lo_subtype_link.Pen.DashStyle = Drawing2D.DashStyle.Solid
                        Case Else
                            lo_subtype_link.Pen.DashPattern = New Single() {2, 2, 2, 2}
                    End Select

                    lo_subtype_link.Tag = Me
                    Me.Link = lo_subtype_link
                    CType(Me.ModelElement, Object).OutgoingLink.Add(lo_subtype_link)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                'Managing changes to properties.
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "IsPrimarySubtypeRelationship"
                            Call Me.SubtypeRelationship.setIsPrimarySubtypeRelationship(Me.IsPrimarySubtypeRelationship)

                    End Select
                End If

                'Drawing the object
                If Me.Link IsNot Nothing Then
                    Select Case Me.IsPrimarySubtypeRelationship
                        Case Is = True
                            Me.Link.Pen = New MindFusion.Drawing.Pen(Color.Purple, 0.5)
                            'Me.Link.Pen.DashStyle = Drawing2D.DashStyle.Solid                            
                        Case Else
                            Me.Link.Pen.DashPattern = New Single() {2, 2, 2, 2}
                    End Select
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveFromPage()

            If Me.Page.Diagram IsNot Nothing Then
                Me.Page.Diagram.Links.Remove(Me.Link)
            End If

            If Me.FactType IsNot Nothing Then
                Me.FactType.RemoveFromPage(True)
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
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Throw New NotImplementedException()
        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move
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

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

        Private Sub SubtypeRelationship_IsPrimarySubtypeRelationshipChanged(abIsPrimarySubtypeRelationship As Boolean) Handles SubtypeRelationship.IsPrimarySubtypeRelationshipChanged

            Me.IsPrimarySubtypeRelationship = abIsPrimarySubtypeRelationship

            Call Me.RefreshShape()

        End Sub

    End Class

End Namespace