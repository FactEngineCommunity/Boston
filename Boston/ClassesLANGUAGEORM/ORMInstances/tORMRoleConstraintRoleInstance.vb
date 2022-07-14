Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization

Namespace FBM
    '---------------------------------------------------------------------
    'NB This class is used predominantly (if 'only') for the bars used
    '  to represent an InternalUniquenessConstraint. The bars themselves
    '  are not the RoleConstraint...they are merely the link between the 
    '  RoleConstraint and the Roles to which the RoleConstraint applies.
    '---------------------------------------------------------------------
    <Serializable()> _
    Public Class RoleConstraintRoleInstance
        Inherits FBM.RoleConstraintRole
        Implements ICloneable
        Implements FBM.iPageObject

        <XmlIgnore()> _
        Public Page As FBM.Page
        Public RoleConstraintRole As FBM.RoleConstraintRole

        Public Shadows Role As FBM.RoleInstance
        Public Shadows RoleConstraint As FBM.RoleConstraintInstance

        ''' <summary>
        ''' If the RoleConstraintRole represents a link to a SubtypingConstraint then stores a reference to the SubtypeConstraintInstance.
        '''   This is so that the 'link' of the RoleConstraintRole can join to the line/link of the Subtyping constraint on the diagram.
        '''   NB The RoleConstraintRole (obviously) joins to a 'Role', but on the diagram the link (for this particular type of RoleConstraintRole)
        '''   joins to the line/arrow that represents the Subtype relationship between a Subtype and its Supertype.
        '''   i.e. The member SubtypeConstraintInstances is used for RoleConstraintRoles on RoleConstraints that represent constraints between 
        '''   SubtypingRelationships. e.g. An ExclusionConstraint representing 'Person IS A Student OR Person IS A Teacher BUT NOT BOTH'.
        ''' </summary>
        ''' <remarks></remarks>
        Public SubtypeConstraintInstance As FBM.SubtypeRelationshipInstance = Nothing

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


        <NonSerialized()> _
        Public Link As DiagramLink

        <NonSerialized(), _
        XmlIgnore()> _
        Public BackingShape As New ShapeNode

        Public Height As Integer
        Public Width As Integer = 6
        Public Property X As Integer = 0 Implements FBM.iPageObject.X
        Public Property Y As Integer = 0 Implements FBM.iPageObject.Y

        Public Property InstanceNumber As Integer Implements iPageObject.InstanceNumber
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub New()
            '--------
            'Default
            '--------
            Me.ConceptType = pcenumConceptType.RoleConstraintRole
        End Sub

        Public Sub New(ByRef arRoleConstraintRole As FBM.RoleConstraintRole, ByRef arRoleConstraintInstance As FBM.RoleConstraintInstance, ByRef arRoleInstance As FBM.RoleInstance)

            Call Me.New()

            Me.Page = arRoleConstraintInstance.Page
            Me.Model = Me.Page.Model

            Me.RoleConstraintRole = arRoleConstraintRole
            Me.RoleConstraint = arRoleConstraintInstance
            Me.Role = arRoleInstance

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Dim lrRoleConstraintRoleInstance As New FBM.RoleConstraintRoleInstance

            Try

                With Me
                    lrRoleConstraintRoleInstance.Model = arPage.Model
                    lrRoleConstraintRoleInstance.Page = arPage
                    lrRoleConstraintRoleInstance.Symbol = .Symbol
                    lrRoleConstraintRoleInstance.Id = .Id
                    lrRoleConstraintRoleInstance.RoleConstraint = arPage.RoleConstraintInstance.Find(AddressOf .RoleConstraint.Equals)
                    lrRoleConstraintRoleInstance.Role = New FBM.RoleInstance
                    lrRoleConstraintRoleInstance.Role = arPage.RoleInstance.Find(AddressOf .Role.Equals)

                    lrRoleConstraintRoleInstance.X = .X
                    lrRoleConstraintRoleInstance.Y = .Y
                End With

                Return lrRoleConstraintRoleInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleConstraintRoleInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleConstraintRoleInstance
            End Try

        End Function

        Public Overloads Function EqualsByRole(ByVal other As FBM.RoleConstraintRoleInstance) As Boolean

            If Me.Role.Id = other.Role.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub displayAndAssociateForInternalUniquenessConstraint()

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance

            '-----------------------------------------------------------------------
            'Get the RoleInstance to draw the InternalUniquenessConstraint against.
            '-----------------------------------------------------------------------
            lrFactTypeInstance = Me.RoleConstraint.RoleConstraintRole(0).Role.FactType

            lrRoleInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.Id = Me.Role.Id)

            '---------------------------------------------------------------------------------------------
            'Create the ShapeNode/Shape for the actual line/s drawn for the InternalUniquenessConstraint
            '---------------------------------------------------------------------------------------------
            Dim loDroppedShapeNode As New ShapeNode
            Dim lrShape As MindFusion.Diagramming.Shape
            If Me.RoleConstraint.IsDeontic Then
                lrShape = New Shape( _
                               New ElementTemplate() _
                               { _
                                    New LineTemplate(23, 0, 100, 0) _
                               }, _
                               New ElementTemplate() _
                               { _
                                    New ArcTemplate(0, -23, 23, 48, 0, 360) _
                               }, _
                                Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

            ElseIf Me.RoleConstraint.IsPreferredIdentifier Then
                '-------------------------------------
                'For preferred uniqueness constraint
                '-------------------------------------
                lrShape = New Shape( _
                                New ElementTemplate() _
                                { _
                                    New LineTemplate(0, 0, 100, 0), _
                                    New LineTemplate(100, 0, 100, 100), _
                                    New LineTemplate(100, 100, 0, 100), _
                                    New LineTemplate(0, 100, 0, 0) _
                                }, _
                                    New ElementTemplate() _
                                { _
                                    New LineTemplate(0, 40, 100, 40), _
                                    New LineTemplate(0, 70, 100, 70) _
                                }, _
                                    Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

                lrShape.Decorations(0).Color = Color.Purple
                lrShape.Decorations(1).Color = Color.Purple
            Else
                lrShape = New Shape( _
                            New ElementTemplate() _
                            { _
                                New LineTemplate(0, 0, 100, 0), _
                                New LineTemplate(100, 0, 100, 100), _
                                New LineTemplate(100, 100, 0, 100), _
                                New LineTemplate(0, 100, 0, 0) _
                            }, _
                            New ElementTemplate() _
                            { _
                                New LineTemplate(0, 50, 100, 50) _
                            }, _
                            Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

                lrShape.Decorations(0).Color = Color.Purple
            End If


            loDroppedShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(lrRoleInstance.Shape.Bounds.X,
                                                                         (lrRoleInstance.Shape.Bounds.Y - 3.5) - ((Me.RoleConstraint.LevelNr - 1) * 2), 6, 2, lrShape)
            loDroppedShapeNode.Pen = New MindFusion.Drawing.Pen(Color.White, 0.0002)
            loDroppedShapeNode.ShadowColor = Color.White
            'loDroppedShapeNode.Shape.Decorations(0).Color = Color.Green
            loDroppedShapeNode.HandlesStyle = HandlesStyle.InvisibleMove
            'loDroppedShapeNode.Transparent = True
            loDroppedShapeNode.AllowOutgoingLinks = False
            loDroppedShapeNode.AllowIncomingLinks = False
            loDroppedShapeNode.ZBottom()

            '-------------------------------------------------------------------------------
            'Assign the RoleConstraint object to the Tag of the UniquenessConstraint Shape
            '-------------------------------------------------------------------------------
            loDroppedShapeNode.Tag = New Object
            loDroppedShapeNode.Tag = Me

            '------------------------------------------------------------------------------------------------------------------
            'Attach the UniquenessConstraint Shape to the RoleInstance.Shape
            '  i.e. MindFusion attach so that when the RoleInstance is moved the UniquenessConstraint(Instance) will move too.
            '------------------------------------------------------------------------------------------------------------------
            loDroppedShapeNode.AttachTo(lrRoleInstance.Shape, AttachToNode.TopCenter)

            '------------------------------------------------------------------------------------------------------------------
            'Assign the UniquenessConstraint(Instance) Shape to the corresponding RoleConstraintRole object
            '  NB For an InternalUniquenessConstraint there is no shape for the RoleConstraint...just the RoleConstraintRoles
            '------------------------------------------------------------------------------------------------------------------
            Me.Shape = loDroppedShapeNode

            If Me.Role.FactType.isPreferredReferenceMode Then
                Me.Shape.Visible = False
            Else
                Me.Shape.Visible = True
            End If

            '-------------------------------------------------
            'Force the FactTypeInstance.Shape to the ZBottom
            '-------------------------------------------------
            If lrRoleInstance.FactType.Shape IsNot Nothing Then
                lrRoleInstance.FactType.Shape.ZBottom()
            End If

            Me.Page.Diagram.Invalidate()


        End Sub

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

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

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

        Public Function ShapeMidPoint() As Point Implements iPageObject.ShapeMidPoint
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace