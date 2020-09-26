Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()>
    Public Class SubtypeRelationshipInstance
        Inherits FBM.ModelObject
        Implements FBM.iPageObject
        Implements IEquatable(Of FBM.SubtypeRelationshipInstance)

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.SubtypeConstraint

        ''' <summary>
        ''' The EntityType for which the SubTypeIstance (line) acts as View/Proxy.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows EntityType As FBM.EntityTypeInstance
        Public Shadows parentEntityType As FBM.EntityTypeInstance

        ''' <summary>
        ''' The FactTypeInstance of the FactType that represents the Subtype
        ''' </summary>
        ''' <remarks></remarks>
        Public Shadows FactType As New FBM.FactTypeInstance

        Public Shadows SubtypeRelationship As New FBM.tSubtypeRelationship

        <NonSerialized(),
        XmlIgnore()>
        Public Link As DiagramLink

        <NonSerialized(),
        XmlIgnore()>
        Public Page As FBM.Page

        Public Property X As Integer Implements iPageObject.X
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Y As Integer Implements iPageObject.Y
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub New()

            Me.ConceptType = pcenumConceptType.SubtypeLink

        End Sub

        Public Sub New(ByRef arPage As FBM.Page,
                       ByRef ar_entity_type_instance As FBM.EntityTypeInstance,
                       ByRef ar_parentEntityTypeInstance As FBM.EntityTypeInstance)

            Call Me.New()

            Me.Model = arPage.Model
            Me.Page = arPage
            Me.EntityType = ar_entity_type_instance
            Me.parentEntityType = ar_parentEntityTypeInstance

        End Sub

        Public Overloads Function Equals(other As SubtypeRelationshipInstance) As Boolean Implements IEquatable(Of SubtypeRelationshipInstance).Equals

            Return (Me.EntityType.Id = other.EntityType.Id) And (Me.parentEntityType.Id = other.parentEntityType.Id)

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
                    lrSubtypeRelationshipInstance.EntityType = arPage.EntityTypeInstance.Find(AddressOf .EntityType.Equals)
                    lrSubtypeRelationshipInstance.parentEntityType = arPage.EntityTypeInstance.Find(AddressOf .parentEntityType.Equals)
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

                If Me.parentEntityType Is Nothing Then
                    Me.parentEntityType = Me.Page.getModelElementById(Me.SubtypeRelationship.parentEntityType.Id)
                End If

                If IsSomething(Me.parentEntityType) Then

                    If Me.parentEntityType.IsObjectifyingEntityType Then
                        loNode = Me.parentEntityType.ObjectifiedFactType.Shape
                    Else
                        loNode = Me.parentEntityType.Shape
                    End If

                    '-------------------------------------------------------------------
                    'CodeSafe: Esp for working with the Core model
                    If (Me.EntityType.Shape Is Nothing) Or (loNode Is Nothing) Then
                        Exit Sub
                    End If

                    lo_subtype_link = Me.Page.Diagram.Factory.CreateDiagramLink(Me.EntityType.Shape, loNode)
                    lo_subtype_link.BaseShape = ArrowHead.None
                    lo_subtype_link.HeadShape = ArrowHead.PointerArrow
                    lo_subtype_link.HeadShapeSize = 3.0
                    lo_subtype_link.HeadPen.Color = Color.Purple
                    lo_subtype_link.Pen.Color = Color.Purple 'RGB(121, 0, 121)
                    lo_subtype_link.Pen.Width = 0.5
                    lo_subtype_link.Tag = Me

                    Me.Link = lo_subtype_link
                    Me.EntityType.OutgoingLink.Add(lo_subtype_link)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Shadows Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)


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
    End Class

End Namespace