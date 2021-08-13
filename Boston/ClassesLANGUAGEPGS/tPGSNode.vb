Imports System.ComponentModel
Imports System.Xml.Serialization
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Reflection
Imports Boston.RDS
Imports Boston.FBM

Namespace PGS

    Public Class Node
        Inherits FBM.FactDataInstance
        Implements FBM.iPageObject
        Implements IEquatable(Of PGS.Node)

        <XmlAttribute()>
        Public NodeType As pcenumPGSEntityType = pcenumPGSEntityType.Node

        Public Shadows WithEvents FactData As New FBM.FactData

        <CategoryAttribute("Entity"),
         Browsable(True),
         [ReadOnly](False),
         BindableAttribute(True),
         DefaultValueAttribute(""),
         DesignOnly(False),
         DescriptionAttribute("The name of the Entity")>
        Public Overrides Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal Value As String)
                Me._Name = Value
                Me._Symbol = Value
                Me.Id = Value
            End Set
        End Property

        Public WithEvents RDSTable As RDS.Table  'The Table may be a RDSRelation, within a Property Graph Schema. If the Table is a PGSRelation then has the ability to switch to a Node, and visa-versa, 
        ' if And when there are changes to Table as a result of changes to the Table's responsible FactType. 
        ' NB The Relation/Table will only have a responsible FactType is the Relation (as a PGSRelation) is a result of an ObjectifiedFactType.

        Public Attribute As New List(Of ERD.Attribute)
        Public Relation As New List(Of ERD.Relation)

        ''' <summary>
        ''' A PGSRelation always has a corresponding Node, because but for the relations attached to the underlying FactType the PGSRelation is either a Link or a Node.
        ''' For example, an ObjectifiedFactType that has no associated FactTypes is a PGSRelation, but the moment a binary FactType linking the ObjectifiedFactType to an EntityType is created
        ''' then the PGSRelation must be presented as a Node on the PGS Page.
        ''' </summary>
        Public PGSRelation As ERD.Relation = Nothing 'If the Node would otherwise be a PGSRelation, is the PGSRelation's Link, else is Nothing.


        Public PrimaryKey As New List(Of ERD.Attribute)

        Public Shadows TableShape As ERD.TableNode

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Public Sub New()

            Me.ConceptType = pcenumConceptType.PGSNode

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asEntityName As String)

            Me.ConceptType = pcenumConceptType.PGSNode
            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.FactData.Name = asEntityName
            Me.FactData.Concept = New FBM.Concept(asEntityName)
            Me.FactData.Data = asEntityName
            Me.Name = asEntityName

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asEntityName, pcenumConceptType.Value)

            Me.Concept = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry).Concept


        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Me.ConceptType = pcenumConceptType.PGSNode
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
            Me.ConceptType = pcenumConceptType.PGSNode
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = New ShapeNode()
            lrPageObject.X = Me.X
            lrPageObject.Y = Me.Y

            Return lrPageObject

        End Function


        Public Shadows Function Equals(ByVal other As PGS.Node) As Boolean Implements System.IEquatable(Of PGS.Node).Equals

            Return Me.Id = other.Id

        End Function


        Public Sub DisplayAndAssociate()

            Dim loDroppedNode As ShapeNode = Nothing
            Dim StringSize As New SizeF
            Dim G As Graphics

            G = Me.Page.Form.CreateGraphics
            StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

            '=====================================================================            
            'Create a ShapeNode on the Page for the PGS Node
            '----------------------------------------------            
            loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 20, 20, Shapes.Ellipse)
            loDroppedNode.Resize(20, 20)
            loDroppedNode.HandlesStyle = HandlesStyle.Invisible
            loDroppedNode.Pen.Width = 0.5
            loDroppedNode.Pen.Color = Color.DeepSkyBlue
            loDroppedNode.TextColor = Color.Black

            loDroppedNode.Brush = New SolidBrush(Color.White)
            loDroppedNode.ShadowColor = Color.LightGray
            loDroppedNode.EnableStyledText = True
            loDroppedNode.Expandable = False
            loDroppedNode.Obstacle = True
            loDroppedNode.AllowIncomingLinks = True
            loDroppedNode.AllowOutgoingLinks = True
            loDroppedNode.Text = Me.FactDataInstance.Data

            loDroppedNode.Tag = Me

            Me.Shape = loDroppedNode
            Me.FactDataInstance.Shape = loDroppedNode

            loDroppedNode.Image = My.Resources.ORMShapes.Blank

        End Sub

        Public Overrides Function getCorrespondingRDSTable() As RDS.Table
            Return Me.RDSTable
        End Function

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements FBM.iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements FBM.iPageObject.MouseUp

        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Me.FactDataInstance.X = aiNewX
            Me.FactDataInstance.Y = aiNewY

            Me.FactDataInstance.Fact.FactType.isDirty = True
            Me.FactDataInstance.Fact.isDirty = True
            Me.FactDataInstance.isDirty = True

            Me.Page.IsDirty = True

        End Sub

        Public Sub Moved() Implements FBM.iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements FBM.iPageObject.NodeDeleting

        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Me.Shape.Image = My.Resources.ORMShapes.Blank
            Me.Shape.Pen.Color = Color.DeepSkyBlue

        End Sub

        Public Sub NodeModified() Implements FBM.iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements FBM.iPageObject.NodeSelected

            If Me.Shape IsNot Nothing Then
                Me.Shape.Pen.Color = Color.Blue
            End If

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")
            Try

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            '-----------------------------------------------------------------------------
                            'Update the Model.
                            '  GUI is updated via the event triggered, (Me)FactData.ConceptSymbolUpdated
                            '-----------------------------------------------------------------------------
                            'Me.FactData.Data = Me.Name
                            Call Me.RDSTable.FBMModelElement.setName(Me.Name)
                            Call Me.Model.Save()
                    End Select
                End If

            Catch ex As Exception

            End Try
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements FBM.iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

        End Sub

        Private Sub FactData_ConceptSwitched(ByRef arConcept As FBM.Concept) Handles FactData.ConceptSwitched

            Try
                Me.Concept = arConcept
                Me.Shape.Text = arConcept.Symbol

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated

            Try
                Me.Shape.Text = Me.Concept.Symbol

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSTable_IsPGSRelationChanged(abNewValue As Boolean) Handles RDSTable.IsPGSRelationChanged

            Try
                If abNewValue = False Then
                    Call Me.DisplayAndAssociate() 'Because what was a PGSRelation is now a Node. Results from a change to the relations to the underlying ObjectifiedFactType.

                    Me.Page.Diagram.Links.Remove(Me.PGSRelation.Link.Link) 'Remove the existing PGSRelation Link before loading new links because the process of loading changes the nature of the Links on the Page.

                    Call Me.Page.loadRelationsForPGSNode(Me)
                    Call Me.Page.loadPropertyRelationsForPGSNode(Me)

                Else
                    If Me.PGSRelation Is Nothing Then
                        'Need to create or find the Relation
                        Me.PGSRelation = Me.Page.ERDiagram.Relation.Find(Function(x) x.ActualPGSNode Is Me)

                        If Me.PGSRelation Is Nothing Then
                            'Still haven't found the Relation, so must create it.
                            'VM-Use code from frmDiagramPGS.LoadPGSDiagramPage
                            Call Me.loadPGSRelationAndLink() 'Does the displaying and associating
                        Else
                            Me.PGSRelation.Link.DisplayAndAssociate
                        End If
                    Else
                        Me.PGSRelation.Link.DisplayAndAssociate
                    End If

                    Me.Page.Diagram.Nodes.Remove(Me.Shape)

                    'CMML
                    Dim lsSQLQuery As String = ""
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                    lsSQLQuery &= " WHERE IsPGSRelation = '" & Me.RDSTable.Name & "'"

                    Dim lrRecordsetIsPGSRelation As ORMQL.Recordset
                    lrRecordsetIsPGSRelation = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrRecordsetIsPGSRelation.EOF Then
                        lsSQLQuery = "ADD FACT '" & lrRecordsetIsPGSRelation.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If
                End If

                Call Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub loadPGSRelationAndLink()

            Dim lsSQLQuery As String = ""
            Dim lrRecordset1 As ORMQL.Recordset
            Dim lrRecordset2 As ORMQL.Recordset

            Try
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                'lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                lsSQLQuery &= " WHERE Entity = '" & Me.RDSTable.Name & "'"

                lrRecordset1 = Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset1.Facts.Count > 2 Then
                    'Ignore for now. Won't happen that much. Most relations of this type are binary.
                    'For ORM/PGS binary-manyToMany relations, the ORM relationship is actually a PGS relation, rather than a PGS Node.
                    'For ORM FactTypes that have 3 or more Roles, within the PGS diagram, the FT is a Node.
                    'NB See DiagramView.MouseDown for how the Predicates are shown for the Link of a Binary PGSRelationNode-come-Relation.
                    '  Must get the Predicates from the ResponsibleFactType rather than the actual Relations which are likely on/from the LinkFactTypes.
                Else
                    Dim liInd As Integer = 1
                    Dim lrNode1 As PGS.Node = Nothing
                    Dim lrNode2 As PGS.Node = Nothing

                    Dim lsRelationId As String = ""

                    While Not lrRecordset1.EOF
                        lsRelationId = lrRecordset1("Relation").Data

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If liInd = 1 Then
                            lrNode2 = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
                        Else
                            lrNode1 = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
                        End If

                        liInd += 1
                        lrRecordset1.MoveNext()
                    End While

                    Dim lrRelation As New ERD.Relation(Me.Page.Model,
                                                       Me.Page,
                                                       lsRelationId,
                                                       lrNode1,
                                                       pcenumCMMLMultiplicity.One,
                                                       False,
                                                       False,
                                                       lrNode2,
                                                       pcenumCMMLMultiplicity.One,
                                                       False,
                                                       Me.RDSTable)

                    lrRelation.IsPGSRelationNode = True
                    lrRelation.ActualPGSNode = Me.Page.ERDiagram.Entity.Find(Function(x) x.Id = Me.Id)
                    lrRelation.ActualPGSNode.PGSRelation = lrRelation

                    'NB Even though the RDSRelation is stored against the Link (below), the Predicates for the Link come from the ResponsibleFactType.
                    '  because the relation is actually a PGSRelationNode.
                    Dim lrRDSRelation As RDS.Relation = Me.Page.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)
                    lrRelation.RelationFactType = lrRDSRelation.ResponsibleFactType

                    Dim lrLink As PGS.Link
                    lrLink = New PGS.Link(Me.Page, New FBM.FactInstance, lrNode1, lrNode2, Nothing, Nothing, lrRelation)
                    lrLink.RDSRelation = lrRDSRelation
                    lrLink.DisplayAndAssociate()
                    lrLink.Link.Text = lrRelation.ActualPGSNode.Id

                    lrLink.Relation.Link = lrLink

                    Me.Page.ERDiagram.Relation.Add(lrRelation)

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSTable_ColumnAdded(ByRef arColumn As Column) Handles RDSTable.ColumnAdded
            Call Me.Page.AddAttributeToEntity(arColumn)
        End Sub

        Private Sub RDSTable_NameChanged(asNewName As String) Handles RDSTable.NameChanged

            Me.Id = asNewName
            Me.Name = asNewName
            Me.Shape.Text = asNewName
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Public Overloads Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace
