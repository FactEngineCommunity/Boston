Imports System.ComponentModel
Imports System.Linq.Expressions
Imports System.Reflection

Namespace ERD


    <Serializable()>
    Public Class Relation
        Implements IEquatable(Of ERD.Relation)

        Public ConceptType As pcenumConceptType = pcenumConceptType.Relation

        Public Model As FBM.Model
        Public Page As FBM.Page

        Public Id As String = ""

        ''' <summary>
        ''' Can be either an RDS.Relation (Foreign Key Reference) or a RDS.Table (E.g. A Many-to-Many table as a Property Graph Edge Type equivalent).
        ''' </summary>
        Public ModelElement As Object

        Public Shadows _GraphLabel As New FEStrings.StringCollection

        <CategoryAttribute("Relation"),
        Browsable(True),
        [ReadOnly](False),
        DescriptionAttribute("The equivalent Graph label in the Graph View."),
        Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Shadows Property GraphLabel() As FEStrings.StringCollection  'NB This is what is edited in the PropertyGrid
            Get
                Dim lasGraphLabel() As String = {}

                If Me.RDSTable IsNot Nothing Then
                    lasGraphLabel = (From MEGraphLabel In Me.RDSTable.FBMModelElement.GraphLabel
                                     Select MEGraphLabel.Label).ToArray
                ElseIf Me.RDSRelation IsNot Nothing Then
                    lasGraphLabel = (From MEGraphLabel In Me.RDSRelation.ResponsibleFactType.GraphLabel
                                     Select MEGraphLabel.Label).ToArray
                End If

                Me._GraphLabel.Clear()
                Me._GraphLabel.AddRange(lasGraphLabel)

                Return Me._GraphLabel

            End Get
            Set(ByVal Value As FEStrings.StringCollection)

                Me._GraphLabel = Value

                Dim lrModelElement As FBM.ModelObject = Nothing

                If Me.RDSTable IsNot Nothing Then
                    lrModelElement = Me.RDSTable.FBMModelElement
                ElseIf Me.RDSRelation IsNot Nothing Then
                    lrModelElement = Me.RDSRelation.ResponsibleFactType
                End If

                ' Find synonyms that are in the model but not in the new value
                Dim graphLabelsToRemove = (From MEGraphLabel In lrModelElement.GraphLabel
                                           Where MEGraphLabel.ModelElementId = Me.Id AndAlso Not Value.Contains(MEGraphLabel.Label)).ToList()

                ' Remove synonyms that are no longer present in the new value
                For Each graphLabelToRemove In graphLabelsToRemove
                    lrModelElement.GraphLabel.Remove(graphLabelToRemove)
                Next

                ' Add new synonyms that are not in the model
                For Each graphLabelToAdd In Value
                    If Not lrModelElement.GraphLabel.Any(Function(s) s.ModelElementId = lrModelElement.Id AndAlso s.Label = graphLabelToAdd) Then
                        lrModelElement.GraphLabel.Add(New RDS.GraphLabel(lrModelElement, graphLabelToAdd))
                    End If
                Next

            End Set

        End Property

        Private _RelationshipType As String = "HAS" 'Relationship Type as in Property Graph Schema RT...e.g. (Person)-[:LIKES]->(Film). HAS, IS_FOR, IS_IN etc

        <Browsable(True),
     Description("The type of relationship in the graph."),
     Category("Relationship"),
     DefaultValue("HAS"),
     DisplayName("Relationship Type")>
        Public Property RelationshipType As String
            Get
                'Code Safe
                If Me._GraphLabel.Count = 0 Then
                    Me._GraphLabel.Add("HAS")
                End If

                Return Me._GraphLabel(0)
            End Get
            Set(value As String)
                'Code Safe
                If Me._GraphLabel.Count = 0 Then
                    Me._GraphLabel.Add(value)
                Else
                    Me._GraphLabel(0) = value 'Relationship Types are singular in Property Graph Schemas, as opposed Node Types that can have multiple Labels.
                End If
            End Set
        End Property

        ''' <summary>
        ''' The TreeNode within the Schema TreeView.
        ''' </summary>
        Public TreeNode As TreeNode

        Public OriginEntity As FBM.FactDataInstance
        Public OriginAttribute As New List(Of ERD.Attribute)
        Public OriginMandatory As Boolean = False
        Public OriginMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public OriginContributesToPrimaryKey As Boolean = False
        Public OriginPredicate As String = ""

        Public DestinationEntity As FBM.FactDataInstance
        Public DestinationAttribute As List(Of ERD.Attribute)
        Public DestinationMandatory As Boolean = False
        Public DestinationMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public DestinationPredicate As String = ""

        Public WithEvents RelationFactType As FBM.FactType

        Public IsPGSRelationNode As Boolean = False
        Public ActualPGSNode As PGS.Node

        Private _EnforcesOnCascadeUpdate As Boolean = False
        <Browsable(True),
        [ReadOnly](False),
        CategoryAttribute("Relationship"),
        DescriptionAttribute("The Relationship enforces On Cascade Update.")>
        Public Property EnforcesOnCascadeUpdate As Boolean
            Get
                Return Me._EnforcesOnCascadeUpdate
            End Get
            Set(value As Boolean)
                Me._EnforcesOnCascadeUpdate = value
            End Set
        End Property

        Private _EnforcesOnCascadeDelete As Boolean = False
        <Browsable(True),
        [ReadOnly](False),
        CategoryAttribute("Relationship"),
        DescriptionAttribute("The Relationship enforces On Cascade Delete.")>
        Public Property EnforcesOnCascadeDelete As Boolean
            Get
                Return Me._EnforcesOnCascadeDelete
            End Get
            Set(value As Boolean)
                Me._EnforcesOnCascadeDelete = value
            End Set
        End Property

        Private _EnforcesReferentialIntegrity As Boolean = False
        <Browsable(True),
        [ReadOnly](False),
        CategoryAttribute("Relationship"),
        DescriptionAttribute("The Relationship enforces On Cascade Update.")>
        Public Property EnforcesReferentialIntegrity As Boolean
            Get
                Return Me._EnforcesReferentialIntegrity
            End Get
            Set(value As Boolean)
                Me._EnforcesReferentialIntegrity = value
            End Set
        End Property

        Private _Link As Object
        Public Property Link As Object 'ERD.Link
            Get
                Return Me._Link
            End Get
            Set(value As Object)
                Me._Link = value
            End Set
        End Property

        Public WithEvents RDSRelation As RDS.Relation

        Public WithEvents RDSTable As RDS.Table 'If the Relation is a PGSRelation then has the ability to switch to a Node, and visa-versa, 
        ' if And when there are changes to Table as a result of changes to the Table's responsible FactType. 
        ' NB The Relation/Table will only have a responsible FactType is the Relation (as a PGSRelation) is a result of an ObjectifiedFactType.

        Public Sub New()
            '---------------------------
            'Parameterless Constructor
            '---------------------------
        End Sub

        ''' <summary>
        ''' Object constructor.
        ''' </summary>
        ''' <param name="arModel"></param>
        ''' <param name="arPage"></param>
        ''' <param name="asRelationId"></param>
        ''' <param name="arOriginEntity"></param>
        ''' <param name="aiOriginMultiplicity"></param>
        ''' <param name="abOriginMandatory"></param>
        ''' <param name="abOriginContributesToPrimaryKey"></param>
        ''' <param name="arDestinationEntity"></param>
        ''' <param name="aiDestinationMultiplicity"></param>
        ''' <param name="abDestinationMandatory"></param>
        ''' <param name="abCorrespondingTable">If the Relation is a PGSRelation, then has a corresponding Table.</param>
        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asRelationId As String,
                       ByRef arOriginEntity As FBM.FactDataInstance,
                       ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abOriginMandatory As Boolean,
                       ByVal abOriginContributesToPrimaryKey As Boolean,
                       ByRef arDestinationEntity As FBM.FactDataInstance,
                       ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abDestinationMandatory As Boolean,
                       Optional ByRef arCorrespondingRDSTable As RDS.Table = Nothing,
                       Optional ByRef arCorrespondingRDSRelation As RDS.Relation = Nothing)

            Me.Model = arModel
            Me.Page = arPage
            Me.Id = asRelationId

            Me.OriginEntity = arOriginEntity
            Me.OriginMultiplicity = aiOriginMultiplicity
            Me.OriginMandatory = abOriginMandatory
            Me.OriginContributesToPrimaryKey = abOriginContributesToPrimaryKey

            Me.DestinationEntity = arDestinationEntity
            Me.DestinationMultiplicity = aiDestinationMultiplicity
            Me.DestinationMandatory = abDestinationMandatory

            If arCorrespondingRDSTable IsNot Nothing Then
                Me.ModelElement = arCorrespondingRDSTable
                Me.RDSTable = arCorrespondingRDSTable
            Else
                Me.ModelElement = arCorrespondingRDSRelation
                Me.RDSRelation = arCorrespondingRDSRelation
            End If

        End Sub

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            Select Case Me.RelationFactType.IsLinkFactType
                Case = True
                    lrPageObject.Name = Me.RelationFactType.RoleGroup(0).JoinedORMObject.Id
                Case Else
                    lrPageObject.Name = Me.RelationFactType.Id
            End Select

            lrPageObject.Shape = New MindFusion.Diagramming.ShapeNode
            Select Case Me.OriginEntity.GetType
                Case = GetType(PGS.Node)
                    lrPageObject.X = CType(Me.OriginEntity, PGS.Node).X
                    lrPageObject.Y = CType(Me.OriginEntity, PGS.Node).Y
                Case Else
                    lrPageObject.X = Me.OriginEntity.X
                    lrPageObject.Y = Me.OriginEntity.Y
            End Select

            Return lrPageObject

        End Function

        Public Shadows Function Equals(other As Relation) As Boolean Implements IEquatable(Of Relation).Equals
            Return Me.Id = other.Id
        End Function

        Private Sub RelationFactType_FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingAdded

        End Sub


        Private Sub RelationFactType_FactTypeReadingModified(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingModified

            Try

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub RDSRelation_DestinationMandatoryChanged(abDestinationIsMandatory As Boolean) Handles RDSRelation.DestinationMandatoryChanged

            Me.DestinationMandatory = abDestinationIsMandatory

            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_DestinationMultiplicityChanged(aiDestinationMultiplicity As pcenumCMMLMultiplicity) Handles RDSRelation.DestinationMultiplicityChanged

            Me.DestinationMultiplicity = aiDestinationMultiplicity
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_DestinationPredicateChanged(asPredicate As String) Handles RDSRelation.DestinationPredicateChanged

            Try
                Me.DestinationPredicate = asPredicate

                If Me.Page IsNot Nothing Then
                    If Me.Page.Diagram IsNot Nothing Then
                        Call Me.Page.Diagram.Invalidate()
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelation_OriginMandatoryChanged(abOriginIsMandatory As Boolean) Handles RDSRelation.OriginMandatoryChanged

            Try
                Me.OriginMandatory = abOriginIsMandatory

                If Me.Page IsNot Nothing Then
                    If Me.Page.Diagram IsNot Nothing Then
                        Call Me.Page.Diagram.Invalidate()
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

        Private Sub RDSRelation_OriginMultiplicityChanged(aiOriginMultiplicity As pcenumCMMLMultiplicity) Handles RDSRelation.OriginMultiplicityChanged

            Me.OriginMultiplicity = aiOriginMultiplicity
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_OriginPredicateChanged(asPredicate As String) Handles RDSRelation.OriginPredicateChanged

            Me.OriginPredicate = asPredicate

        End Sub

        Private Sub RDSRelation_RemovedFromModel() Handles RDSRelation.RemovedFromModel

            Try

                If Me.Link Is Nothing Then Exit Sub
                Me.Page.Diagram.Links.Remove(Me.Link.Link)
                '20220523-VM-Was .Remove(Me). But there were times when the ERDRelation was in the list more than once.
                Me.Page.ERDiagram.Relation.RemoveAll(AddressOf Me.Equals)

                Dim lrDiagramingLink As MindFusion.Diagramming.DiagramLink = Me.Link.Link

                If lrDiagramingLink IsNot Nothing Then
                    lrDiagramingLink.Dispose()
                End If

                Me.Page.Diagram.Invalidate()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Private Sub RDSRelation_ResponsibleFactTypeChanged(ByRef arNewResponsibleFactType As FBM.FactType) Handles RDSRelation.ResponsibleFactTypeChanged

            Me.RelationFactType = arNewResponsibleFactType

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                Dim lrResponsibleFactType As FBM.FactType = Nothing 'The FactType responsible for the EdgeType/Relationship.

                '------------------------------------------------
                'Set the values in the underlying RDS.Relation
                '------------------------------------------------
                If aoChangedPropertyItem IsNot Nothing Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name

                        Case Is = "EnforcesOnCascadeUpdate"
                            Call Me.RDSRelation.SetEnforcesOnCascadeUpdate(Me.EnforcesOnCascadeUpdate)

                        Case Is = "EnforcesOnCascadeDelete"
                            Call Me.RDSRelation.SetEnforcesOnCascadeDelete(Me.EnforcesOnCascadeDelete)

                        Case Is = "EnforcesReferentialIntegrity"
                            Call Me.RDSRelation.SetEnforcesReferentialIntegrity(Me.EnforcesReferentialIntegrity)

                        Case Is = "Value"
                            With New WaitCursor
                                Select Case asSelectedGridItemLabel
                                    Case Is = "GraphLabel"
#Region "GraphLabel"
                                        'GraphLabel processing.
                                        Select Case Me.ModelElement.GetType
                                            Case Is = GetType(RDS.Relation)
                                                Call Me.RDSRelation.ResponsibleFactType.ModifyOrAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                            Case Is = GetType(RDS.Table)
                                                Call Me.RDSTable.FBMModelElement.ModifyOrAddGraphLabel(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                        End Select

                                        '-------------------------------------------------------------------------------------------------------------------------------
                                        'Removing an item using the UITypeEditor does not trigger a return of aoChangedPropertyItem (As PropertyValueChangedEventArgs).
                                        '  So we must check each time (back here) whether there is an item to remove from the GraphLabels list for the [ModelElement]Instance.
                                        Dim lrDataStore As New DataStore.Store

                                        Select Case Me.ModelElement.GetType
                                            Case Is = GetType(RDS.Relation)
                                                lrResponsibleFactType = Me.RDSRelation.ResponsibleFactType
                                            Case Is = GetType(RDS.Table)
                                                lrResponsibleFactType = Me.RDSTable.FBMModelElement
                                        End Select

                                        For Each lsGraphLabel In lrResponsibleFactType.GraphLabel.FindAll(Function(x) x.Label <> aoChangedPropertyItem.ChangedItem.Value).Select(Function(x) x.Label).ToArray
                                            If lsGraphLabel IsNot Nothing Then
                                                If Not Me._GraphLabel.Contains(lsGraphLabel) Then
                                                    Select Case Me.ModelElement.GetType
                                                        Case Is = GetType(RDS.Relation)
                                                            Call Me.RDSRelation.ResponsibleFactType.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSRelation.ResponsibleFactType.Id And x.Label = lsGraphLabel)
                                                            Dim lsModelId = Me.RDSRelation.Model.Model.ModelId
                                                            Dim lsLocalGraphLabel = lsGraphLabel
                                                            Dim whereClause As Expression(Of Func(Of RDS.GraphLabel, Boolean)) = Function(t) t.ModelId = lsModelId And t.ModelElementId = Me.RDSRelation.ResponsibleFactType.Id And t.Label = lsLocalGraphLabel
                                                            lrDataStore.Delete(Of RDS.GraphLabel)(whereClause)
                                                        Case Is = GetType(RDS.Table)
                                                            Call Me.RDSTable.FBMModelElement.GraphLabel.RemoveAll(Function(x) x.ModelElement.Id = Me.RDSTable.FBMModelElement.Id And x.Label = lsGraphLabel)
                                                            Dim lsModelId = Me.RDSRelation.Model.Model.ModelId
                                                            Dim lsLocalGraphLabel = lsGraphLabel
                                                            Dim whereClause As Expression(Of Func(Of RDS.GraphLabel, Boolean)) = Function(t) t.ModelId = lsModelId And t.ModelElementId = Me.RDSTable.FBMModelElement.Id And t.Label = lsLocalGraphLabel
                                                            lrDataStore.Delete(Of RDS.GraphLabel)(whereClause)
                                                    End Select
                                                End If
                                            End If
                                        Next
#End Region
                                    Case Else
                                        'No other collections at this stage.
                                End Select
                            End With
                    End Select
                End If

                '=======================================================================================
                'Graphics
                If Me.TreeNode IsNot Nothing Then

                    Dim lrOriginTable As RDS.Table = Nothing
                    Dim lrDestinationTable As RDS.Table = Nothing
                    lrResponsibleFactType = Me.RDSRelation.ResponsibleFactType

                    Select Case Me.ModelElement.GetType
                        Case Is = GetType(RDS.Relation)
                            If Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then

                                Dim lrObjectifyingFactType = (From FactType In Me.Model.FactType
                                                              Where FactType.getLinkFactTypes.Contains(Me.RDSRelation.ResponsibleFactType)
                                                              Select FactType).First

                                lrResponsibleFactType = lrObjectifyingFactType

                                Dim larRelationshipRole = (From Role In lrObjectifyingFactType.RoleGroup.FindAll(Function(x) x.JoinsValueType Is Nothing)
                                                           Select Role).ToList

                                lrOriginTable = larRelationshipRole(0).JoinedORMObject.getCorrespondingRDSTable
                                lrDestinationTable = larRelationshipRole(1).JoinedORMObject.getCorrespondingRDSTable
                            Else
                                lrOriginTable = Me.RDSRelation.OriginTable
                                lrDestinationTable = Me.RDSRelation.DestinationTable

                            End If
                        Case Is = GetType(RDS.Table)
                            Dim larRDSTable = From Column In Me.RDSTable.Column
                                              Where Column.Relation.Count <> 0
                                              Select Column.Relation(0).DestinationTable

                            'If the RDS Table joins more than two RDS Tables, then something will have gone wrong. But is tightly controlled within this app.
                            lrOriginTable = larRDSTable(0)
                            lrDestinationTable = larRDSTable(1)
                    End Select

                    Me.TreeNode.Text = $"({lrOriginTable.Name})-[:{lrResponsibleFactType.GraphLabel(0).Label}]->({lrDestinationTable.Name})"
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub



        Public Sub removeFromPage()

            Try
                Me.Page.Diagram.Links.Remove(Me.Link.Link)

                Dim lrDiagramingLink As MindFusion.Diagramming.DiagramLink = Me.Link.Link

                If lrDiagramingLink IsNot Nothing Then
                    lrDiagramingLink.Dispose()
                End If

                '20220523-VM-Was .Remove(Me). Changed to capture all instances if more than one.
                Me.Page.ERDiagram.Relation.RemoveAll(AddressOf Me.Equals)

                Me.Page.Diagram.Invalidate()


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub RDSRelation_OriginTableChanged(ByRef arTable As RDS.Table) Handles RDSRelation.OriginTableChanged

            Try
                If Me.Page.Diagram IsNot Nothing Then

                    If Me.Link IsNot Nothing Then
                        Dim lrTable As RDS.Table = arTable
                        Dim lrERDLink As ERD.Link = Me.Link
                        Dim lrEntity As ERD.Entity = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

                        If lrEntity IsNot Nothing Then
                            lrERDLink.OriginModelElement = lrEntity
                            lrERDLink.Link.Origin = lrERDLink.OriginModelElement.TableShape
                        Else
                            Me.Page.Diagram.Links.Remove(lrERDLink.Link)
                        End If

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

        Private Sub RDSRelation_DestinationTableChanged(ByRef arTable As RDS.Table) Handles RDSRelation.DestinationTableChanged

            Try
                If Me.Page.Diagram IsNot Nothing Then

                    If Me.Link IsNot Nothing Then
                        Dim lrTable As RDS.Table = arTable
                        Dim lrERDLink As ERD.Link = Me.Link
                        Dim lrEntity As ERD.Entity = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

                        If lrEntity IsNot Nothing Then
                            lrERDLink.DestinationModelElement = lrEntity
                            lrERDLink.Link.Destination = lrERDLink.DestinationModelElement.TableShape
                        Else
                            Me.Page.Diagram.Links.Remove(lrERDLink.Link)
                        End If

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

        Private Sub RDSRelation_EnforcesOnCascadeUpdateChanged(abNewEnforcesOnCascadeUpdate As Boolean) Handles RDSRelation.EnforcesOnCascadeUpdateChanged

            Try
                Me.EnforcesOnCascadeUpdate = abNewEnforcesOnCascadeUpdate

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub RDSRelation_EnforcesOnCascadeDeleteChanged(abNewEnforcesOnCascadeDelete As Boolean) Handles RDSRelation.EnforcesOnCascadeDeleteChanged

            Try
                Me.EnforcesOnCascadeDelete = abNewEnforcesOnCascadeDelete

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub RDSRelation_EnforcesReferentialIntegrityChanged(abNewEnforcesReferentialIntegrity As Boolean) Handles RDSRelation.EnforcesReferentialIntegrityChanged

            Try
                Me.EnforcesReferentialIntegrity = abNewEnforcesReferentialIntegrity

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub RDSRelation_GraphLabelAdded(asNewGraphLabel As String) Handles RDSRelation.GraphLabelAdded
            Me._GraphLabel.Add(asNewGraphLabel)
        End Sub

        Private Sub RDSTable_GraphLabelAdded(asNewGraphLabel As String) Handles RDSTable.GraphLabelAdded
            Me._GraphLabel.Add(asNewGraphLabel)
        End Sub

    End Class

End Namespace