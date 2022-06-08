Imports System.Drawing.Drawing2D

Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Reflection

Public Class frmDiagramPGS

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing
    Public ERDiagram As New ERD.Diagram

    Private MorphVector As New List(Of tMorphVector)

    Private PropertyTableNode As TableNode

    Public Function areAllEntitiesAtPoint00() As Boolean

        For Each lrEntity As PGS.Node In Me.zrPage.ERDiagram.Entity
            If (lrEntity.X <> 0) Or (lrEntity.Y <> 0) Then
                Return False
            End If
        Next

        Return True

    End Function

    Public Sub autoLayout()

        Try
            '---------------------------------------------------------------------------------------
            ' Create the layouter object
            Dim layout As New MindFusion.Diagramming.Layout.AnnealLayout

            ' Adjust the attributes of the layouter
            layout.MultipleGraphsPlacement = MultipleGraphsPlacement.Horizontal
            layout.KeepGroupLayout = True

            'layout.NodeDistance = 40
            'layout.RandomSeed = 50
            'layout.MinimizeCrossings = True
            'layout.RepulsionFactor = 50
            'layout.EnableClusters = True
            'layout.IterationCount = 100

            'layout.GridSize = 20 'Not part of SpringLayout.                

            layout.Arrange(Diagram)

            Call Me.layoutSelfJoiningLinks()

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub layoutSelfJoiningLinks()

        Try

            Dim larShapeNode As New List(Of ShapeNode)

            For Each lrShapeNode In Me.Diagram.Nodes
                If lrShapeNode.GetType = GetType(ShapeNode) Then
                    larShapeNode.Add(lrShapeNode)
                End If
            Next

            For Each lrNode As ShapeNode In larShapeNode

                Dim commonLinks As DiagramLinkCollection = getOutgoingLinksWithSameOriginAndDestination(lrNode, lrNode)

                Dim liDegrees As Double = 10
                For Each lrLink As DiagramLink In commonLinks
                    Dim lrNewPointF As PointF

                    Dim liRadius As Integer = lrNode.Bounds.Width / 2
                    Dim liCircleCentreX, liCircleCentreY As Integer
                    liCircleCentreX = lrNode.Bounds.X + (lrNode.Bounds.Width / 2)
                    liCircleCentreY = lrNode.Bounds.Y + (lrNode.Bounds.Height / 2)

                    lrNewPointF = Me.getPointOnCircle(liCircleCentreX, liCircleCentreY, liRadius, liDegrees)

                    lrLink.ControlPoints(0) = lrNewPointF
                    lrLink.ControlPoints(1) = Me.getPointOnCircle(liCircleCentreX, liCircleCentreY, liRadius + 15, liDegrees - 0.3)
                    lrLink.ControlPoints(2) = Me.getPointOnCircle(liCircleCentreX, liCircleCentreY, liRadius + 15, liDegrees + 0.3)
                    lrLink.ControlPoints(lrLink.ControlPoints.Count - 1) = lrNewPointF

                    Call lrLink.UpdateFromPoints()

                    liDegrees += 20
                Next
            Next
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Function getPointOnCircle(ByVal aiCircleCentreX As Integer, ByVal aiCircleCentreY As Integer, ByVal aiRadius As Integer, ByVal aiDegrees As Double) As PointF

        Dim x As Single = aiRadius * Math.Cos(aiDegrees) + aiCircleCentreX
        Dim y As Single = aiRadius * Math.Sin(aiDegrees) + aiCircleCentreY

        Return New PointF(x, y)

    End Function

    Public Shadows Sub BringToFront(Optional asSelectModelElementId As String = Nothing)

        Call MyBase.BringToFront()

        If asSelectModelElementId IsNot Nothing Then
            Me.Diagram.Selection.Items.Clear()

            Dim lrNode As PGS.Node = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Id = asSelectModelElementId)
            If lrNode IsNot Nothing Then
                lrNode.shape.Selected = True
            End If

        End If

    End Sub

    Private Sub CarteseanToPolar(ByVal coordCenter As PointF, ByVal dekart As PointF, ByRef a As Single, ByRef r As Single)
        If coordCenter = dekart Then
            a = 0
            r = 0
            Return
        End If

        Dim dx As Single = dekart.X - coordCenter.X
        Dim dy As Single = dekart.Y - coordCenter.Y
        r = CSng(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)))

        a = CSng(Math.Atan(-dy / dx) * 180 / Math.PI)
        If dx < 0 Then
            a += 180
        End If
    End Sub

    Private Sub frm_EntityRelationshipDiagram_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call SetToolbox()

    End Sub

    Private Sub frmDiagramPGS_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        prApplication.ActivePages.Remove(Me)

    End Sub

    Private Sub frm_EntityRelationshipDiagram_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        Call SetToolbox()

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        If IsSomething(Me.zrPage) Then
            If prApplication.WorkingModel Is Nothing Then
                prApplication.WorkingModel = Me.zrPage.Model
            End If

            Dim lrModelDictionaryForm As frmToolboxModelDictionary
            lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

            If IsSomething(lrModelDictionaryForm) Then
                Call lrModelDictionaryForm.LoadToolboxModelDictionary(Me.zrPage.Language)
            End If
        End If

    End Sub

    Private Sub frmDiagramERD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------        
        Me.zrPage.FormLoaded = False

    End Sub

    Private Sub frm_EntityRelationshipDiagram_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        Me.zrPage.SelectedObject.Clear()

        Call Me.SetToolbox()


    End Sub

    Private Sub frm_EntityRelationshipDiagram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

        prApplication.ActivePages.AddUnique(Me)

    End Sub

    Sub SetupForm()

        Me.PageAsORMMetamodelToolStripMenuItem.Visible = My.Settings.SuperuserMode

    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Private Function GetCommonLinks(ByVal node1 As DiagramNode, ByVal node2 As DiagramNode) As DiagramLinkCollection
        Dim commonLinks As New DiagramLinkCollection()

        Try

            For Each link As DiagramLink In node1.OutgoingLinks

                'CodeSafe
                If link.Destination.Tag Is Nothing Then
                    Return commonLinks
                End If

                If link.Destination.Tag.Name = node2.Tag.Name Then
                    commonLinks.Add(link)
                End If
            Next

            For Each link As DiagramLink In node1.IncomingLinks
                If link.Origin.Tag.Name = node2.Tag.Name Then
                    commonLinks.Add(link)
                End If
            Next

            Return commonLinks

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return commonLinks
        End Try

    End Function

    Private Function getOutgoingLinksWithSameOriginAndDestination(ByVal node1 As DiagramNode, ByVal node2 As DiagramNode) As DiagramLinkCollection

        Dim commonLinks As New DiagramLinkCollection()

        For Each link As DiagramLink In node1.OutgoingLinks
            If link.Destination.Tag.Name = node2.Tag.Name Then
                commonLinks.Add(link)
            End If
        Next

        Return commonLinks

    End Function

    Sub loadPGSDiagramPage(ByRef arPage As FBM.Page, ByRef aoTreeNode As TreeNode, Optional asSelectModelElementId As String = Nothing)

        Dim loDroppedNode As TableNode = Nothing
        Dim lrFactTypeInstance_pt As PointF = Nothing
        Dim lrFactInstance As New FBM.FactInstance
        Dim lrPGSNode As New PGS.Node

        Try
            '-------------------------------------------------------
            'Set the Caption/Title of the Page to the PageName
            '-------------------------------------------------------
            Me.zrPage = arPage
            Me.TabText = arPage.Name

            Me.zoTreeNode = aoTreeNode

            Me.zrPage.ERDiagram.Entity.Clear()

            '------------------------------------------------------------------------------
            'Display the EntityRelationshipDiagram by logically associating Shape objects
            '   with the corresponding 'object' within the ORMModelPage object
            '------------------------------------------------------------------------------
            Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

            '--------------------
            'Load the Entities.
            '--------------------
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE ElementType = 'Entity'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF

                lrPGSNode = New PGS.Node

                lrFactDataInstance = lrRecordset("Element")
                lrPGSNode = lrFactDataInstance.ClonePGSNodeType(arPage)

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " WHERE IsPGSRelation = '" & lrPGSNode.Name & "'"

                Dim lrRecordsetIsPGSRelation As ORMQL.Recordset
                lrRecordsetIsPGSRelation = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordsetIsPGSRelation("Count").Data) > 0 Then
                    lrPGSNode.NodeType = pcenumPGSEntityType.Relationship
                End If

                '----------------------------------------------
                'RDS
                lrPGSNode.RDSTable = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrPGSNode.Name)

                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                If lrPGSNode.NodeType <> pcenumPGSEntityType.Relationship Then
                    lrPGSNode.DisplayAndAssociate()
                ElseIf lrPGSNode.RDSTable.PGSArity >= 3 Then
                    lrPGSNode.DisplayAndAssociate()
                End If

                If lrPGSNode.RDSTable.FBMModelElement.ConceptType = pcenumConceptType.FactType Then
                    If lrPGSNode.RDSTable.CountNonValueTypeColumns > 2 Then
                        If Not lrPGSNode.IsDisplayedAssociated Then
                            lrPGSNode.DisplayAndAssociate()
                        End If
                    End If
                End If

                Me.zrPage.ERDiagram.Entity.Add(lrPGSNode)

                lrRecordset.MoveNext()
            End While

            Dim lrNodeType As PGS.Node
            For Each lrNodeType In Me.zrPage.ERDiagram.Entity
                Call lrNodeType.GetAttributesFromRDSColumns()
            Next

            '=======================================================
            ' Map the Relations - Link the Nodes. From Model level
            '=======================================================
            Dim larLoadedRelationNodes As New List(Of PGS.Node)
            Dim lrOriginNode As PGS.Node = Nothing
            Dim lrDestinationNode As PGS.Node = Nothing

            Dim lasLoadedRelationIds As New List(Of String)

            For Each lrNode As PGS.Node In Me.zrPage.ERDiagram.Entity.ToArray

                If lrNode.RDSTable.isPGSRelation And
                    (lrNode.RDSTable.Arity < 3 Or lrNode.RDSTable.PGSArity < 3) Then
                    'Need to load the relation for the joined Nodes, not the PGSRelation.
                    'E.g. If 'Person likes Person WITH Rating'...then need to load that relation

                    Call Me.zrPage.loadRelationsForPGSNode(lrNode, False)

                    Dim lrFactType As FBM.FactType = lrNode.RDSTable.FBMModelElement

                    Dim larDestinationModelObjects = lrFactType.getDestinationModelObjects

                    Dim lrRDSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.OriginTable.Name = lrNode.Name And
                                                                                      larDestinationModelObjects.Select(Function(y) y.Id).ToList.Contains(x.DestinationTable.Name))

                    Dim lbAllFound As Boolean = True
                    For Each lrModelObject In larDestinationModelObjects
                        If lrModelObject.GetType <> GetType(FBM.ValueType) Then
                            If Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrModelObject.Id) Is Nothing Then
                                lbAllFound = False
                            End If
                        End If
                    Next

                    If lbAllFound Then
                        If lrNode.RDSTable.isPGSRelation Then
                            Call Me.zrPage.DisplayPGSRelationNodeLink(lrNode, lrRDSRelation)
                            If lrNode.Shape IsNot Nothing Then
                                lrNode.Shape.Visible = False
                                For Each lrEdge As MindFusion.Diagramming.DiagramLink In lrNode.Shape.OutgoingLinks
                                    lrEdge.Visible = False
                                Next
                            End If
                        End If
                    End If
                Else
                    Call Me.zrPage.loadRelationsForPGSNode(lrNode, False, lasLoadedRelationIds)
                    'Call Me.zrPage.loadPropertyRelationsForPGSNode(lrNode, False)
                End If

                '20210929-VM-Was not getting PGSRelations
                'For Each lrRDSRelation In lrNode.RDSTable.getOutgoingRelations

                '    lrDestinationNode = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrRDSRelation.DestinationTable.Name)

                '    If lrDestinationNode IsNot Nothing Then
                '        'The Destination Table is on the Page.
                '        lrOriginNode = lrNode

                '        If (lrOriginNode.NodeType = pcenumPGSEntityType.Relationship) And (lrOriginNode.RDSTable.CountNonValueTypeColumns < 3) Then


                '            'For BinaryManyToMany relations, the ORM relationship is actually a PGS relation, rather than a PGS Node.
                '            If Not larLoadedRelationNodes.Contains(lrOriginNode) Then

                '                Dim lrPGSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.Id = lrRDSRelation.Id)
                '                Call Me.zrPage.displayPGSRelationNodeLink(lrOriginNode, lrRDSRelation)

                '                larLoadedRelationNodes.Add(lrOriginNode)
                '            End If
                '        Else 'Is Not a PGSRelation

                '            Dim lrPGSRelation As New ERD.Relation(Me.zrPage.Model,
                '                                          Me.zrPage,
                '                                          lrRDSRelation.Id,
                '                                          lrOriginNode,
                '                                          lrRDSRelation.OriginMultiplicity,
                '                                          lrRDSRelation.RelationOriginIsMandatory,
                '                                          lrRDSRelation.ContributesToPrimaryKey,
                '                                          lrDestinationNode,
                '                                          lrRDSRelation.DestinationMultiplicity,
                '                                          lrRDSRelation.RelationDestinationIsMandatory)

                '            lrPGSRelation.OriginPredicate = lrRDSRelation.OriginPredicate
                '            lrPGSRelation.DestinationPredicate = lrRDSRelation.DestinationPredicate
                '            lrPGSRelation.RelationFactType = lrRDSRelation.ResponsibleFactType

                '            '====================================================================
                '            'RDS
                '            lrPGSRelation.RDSRelation = lrRDSRelation

                '            Me.zrPage.ERDiagram.Relation.Add(lrPGSRelation)

                '            Dim lrLink As PGS.Link
                '            lrLink = New PGS.Link(Me.zrPage, lrFactInstance, lrOriginNode, lrDestinationNode, Nothing, Nothing, lrPGSRelation)
                '            lrLink.DisplayAndAssociate()
                '            lrPGSRelation.Link = lrLink
                '            lrOriginNode.Relation.Add(lrPGSRelation)

                '        End If

                '    End If

                'Next

            Next

#Region "Vitage Map Relations, based on CMML Page Instances"
            '20220523-VM-Get rid of after a while. Has been fine for a long time.
            ''=======================================
            '' Map the Relations - Link the Nodes
            ''=======================================
            'Dim larLoadedRelationNodes As New List(Of PGS.Node)

            'lsSQLQuery = "SELECT *"
            'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
            'lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            'lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            'While Not lrRecordset.EOF

            '    '------------------------
            '    'Find the Origin Entity
            '    '------------------------
            '    Dim lrOriginatingNode As New PGS.Node
            '    Dim lsRelationId As String = ""

            '    lrOriginatingNode.Symbol = lrRecordset("Entity").Data

            '    lsRelationId = lrRecordset("Relation").Data

            '    lrOriginatingNode = Me.zrPage.ERDiagram.Entity.Find(AddressOf lrOriginatingNode.EqualsBySymbol)

            '    If (lrOriginatingNode.NodeType = pcenumPGSEntityType.Relationship) And
            '        (lrOriginatingNode.RDSTable.Arity < 3) Then


            '        'For binary-manyToMany relations, the ORM relationship is actually a PGS relation, rather than a PGS Node.
            '        If Not larLoadedRelationNodes.Contains(lrOriginatingNode) Then

            '            Dim lrRDSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)
            '            Call Me.zrPage.displayPGSRelationNodeLink(lrOriginatingNode, lrRDSRelation)

            '            larLoadedRelationNodes.Add(lrOriginatingNode)

            '            '    lsSQLQuery = "SELECT *"
            '            '    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
            '            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            '    lsSQLQuery &= " WHERE Entity = '" & lrOriginatingNode.Name & "'"

            '            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            '    If lrRecordset1.Facts.Count > 2 Then
            '            '        'Ignore for now. Won't happen that much. Most relations of this type are binary.
            '            '        'For ORM/PGS binary-manyToMany relations, the ORM relationship is actually a PGS relation, rather than a PGS Node.
            '            '        'For ORM FactTypes that have 3 or more Roles, within the PGS diagram, the FT is a Node.
            '            '        'NB See DiagramView.MouseDown for how the Predicates are shown for the Link of a Binary PGSRelationNode-come-Relation.
            '            '        '  Must get the Predicates from the ResponsibleFactType rather than the actual Relations which are likely on/from the LinkFactTypes.
            '            '    Else
            '            '        Dim liInd As Integer = 1
            '            '        Dim lrNode1 As PGS.Node = Nothing
            '            '        Dim lrNode2 As PGS.Node = Nothing

            '            '        While Not lrRecordset1.EOF
            '            '            lsSQLQuery = "SELECT *"
            '            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
            '            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            '            lsSQLQuery &= " WHERE Relation = '" & lrRecordset1("Relation").Data & "'"

            '            '            lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            '            If liInd = 1 Then
            '            '                lrNode2 = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
            '            '            Else
            '            '                lrNode1 = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
            '            '            End If

            '            '            liInd += 1
            '            '            lrRecordset1.MoveNext()
            '            '        End While

            '            '        Dim lrRelation As New ERD.Relation(Me.zrPage.Model,
            '            '                                           Me.zrPage,
            '            '                                           lrRecordset("Relation").Data,
            '            '                                           lrNode1,
            '            '                                           pcenumCMMLMultiplicity.One,
            '            '                                           False,
            '            '                                           False,
            '            '                                           lrNode2,
            '            '                                           pcenumCMMLMultiplicity.One,
            '            '                                           False,
            '            '                                           lrOriginatingNode.RDSTable)

            '            '        lrRelation.IsPGSRelationNode = True
            '            '        lrRelation.ActualPGSNode = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Id = lrOriginatingNode.Id)
            '            '        lrRelation.ActualPGSNode.PGSRelation = lrRelation

            '            '        'NB Even though the RDSRelation is stored against the Link (below), the Predicates for the Link come from the ResponsibleFactType.
            '            '        '  because the relation is actually a PGSRelationNode.
            '            '        Dim lrRDSRelation As RDS.Relation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.Id = lrRecordset("Relation").Data)
            '            '        lrRelation.RelationFactType = lrRDSRelation.ResponsibleFactType

            '            '        If lrRDSRelation.ResponsibleFactType.FactTypeReading.Count = 1 Then
            '            '            Dim lrFactTypeReading = lrRDSRelation.ResponsibleFactType.FactTypeReading(0)
            '            '            If Not lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = lrNode1.Name Then
            '            '                'Swap the Origin and Desination nodes, for directed Graphs. i.e. The single FactTypeReading determines the direction.
            '            '                Dim lrTempNode = lrNode1
            '            '                lrNode1 = lrRelation.DestinationEntity
            '            '                lrNode2 = lrTempNode
            '            '            End If
            '            '        End If

            '            '        Dim lrLink As PGS.Link

            '            '        lrLink = New PGS.Link(Me.zrPage, lrFactInstance, lrNode1, lrNode2, Nothing, Nothing, lrRelation)
            '            '        lrLink.RDSRelation = lrRDSRelation

            '            '        lrLink.DisplayAndAssociate()

            '            '        lrLink.Link.Text = lrRelation.ActualPGSNode.Id
            '            '        lrLink.Relation.Link = lrLink
            '            '        Me.zrPage.ERDiagram.Relation.Add(lrRelation)
            '            '        larLoadedRelationNodes.Add(lrRelation.ActualPGSNode)

            '            '        End If
            '        End If
            '    Else 'Is Not a PGSRelation

            '        '-----------------------------
            '        'Find the Destination Entity
            '        '-----------------------------
            '        Dim lrDestinationNode As New PGS.Node

            '        lsSQLQuery = "SELECT *"
            '        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
            '        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

            '        lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '        lrDestinationNode.Symbol = lrRecordset1("Entity").Data

            '        lrDestinationNode = Me.zrPage.ERDiagram.Entity.Find(AddressOf lrDestinationNode.EqualsBySymbol)

            '        If Not lrDestinationNode Is Nothing Then

            '            '================================
            '            lsSQLQuery = "SELECT *"
            '            lsSQLQuery &= " FROM CoreOriginMultiplicity"
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lsOriginMultiplicity As String = lrRecordset1("Multiplicity").Data

            '            Dim liOriginMultiplicity As pcenumCMMLMultiplicity
            '            Select Case lsOriginMultiplicity
            '                Case Is = pcenumCMMLMultiplicity.One.ToString
            '                    liOriginMultiplicity = pcenumCMMLMultiplicity.One
            '                Case Is = pcenumCMMLMultiplicity.Many.ToString
            '                    liOriginMultiplicity = pcenumCMMLMultiplicity.Many
            '            End Select

            '            lsSQLQuery = "SELECT COUNT(*)"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE OriginIsMandatory = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lbRelationOriginIsMandatory As Boolean = False
            '            If CInt(lrRecordset1("Count").Data) > 0 Then
            '                lbRelationOriginIsMandatory = True
            '            End If

            '            '================================
            '            lsSQLQuery = "SELECT *"
            '            lsSQLQuery &= " FROM CoreDestinationMultiplicity"
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lsDestinationMultiplicity As String = lrRecordset1("Multiplicity").Data

            '            Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
            '            Select Case lsDestinationMultiplicity
            '                Case Is = pcenumCMMLMultiplicity.One.ToString
            '                    liDestinationMultiplicity = pcenumCMMLMultiplicity.One
            '                Case Is = pcenumCMMLMultiplicity.Many.ToString
            '                    liDestinationMultiplicity = pcenumCMMLMultiplicity.Many
            '            End Select

            '            lsSQLQuery = "SELECT COUNT(*)"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lbRelationDestinationIsMandatory As Boolean = False
            '            If CInt(lrRecordset1("Count").Data) > 0 Then
            '                lbRelationDestinationIsMandatory = True
            '            End If

            '            '-------------------------------------------------------------------------------
            '            'Check to see whether the Relation contributes to the PrimaryKey of the Entity
            '            '-------------------------------------------------------------------------------
            '            lsSQLQuery = "SELECT COUNT(*)"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & " = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lbContributesToPrimaryKey As Boolean = False

            '            If CInt(lrRecordset1("Count").Data) > 0 Then
            '                lbContributesToPrimaryKey = True
            '            End If

            '            Dim lrRelation As New ERD.Relation(Me.zrPage.Model,
            '                                               Me.zrPage,
            '                                               lrRecordset("Relation").Data,
            '                                               lrOriginatingNode, _
            '                                               liOriginMultiplicity, _
            '                                               lbRelationOriginIsMandatory, _
            '                                               lbContributesToPrimaryKey, _
            '                                               lrDestinationNode, _
            '                                               liDestinationMultiplicity, _
            '                                               lbRelationDestinationIsMandatory)

            '            '-------------------------------------
            '            'Get the Predicates for the Relation
            '            '-------------------------------------
            '            lsSQLQuery = "SELECT *"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            '            If Not lrRecordset1.EOF Then
            '                lrRelation.OriginPredicate = lrRecordset1("Predicate").Data
            '            End If


            '            lsSQLQuery = "SELECT *"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            '            If Not lrRecordset1.EOF Then
            '                lrRelation.DestinationPredicate = lrRecordset1("Predicate").Data
            '            End If

            '            '-----------------------------------
            '            'Get the FactType for the Relation
            '            '-----------------------------------
            '            lsSQLQuery = "SELECT *"
            '            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
            '            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '            Dim lrFactType As New FBM.FactType(Me.zrPage.Model, lrRecordset1("FactType").Data, True)
            '            lrFactType = Me.zrPage.Model.FactType.Find(AddressOf lrFactType.Equals)
            '            lrRelation.RelationFactType = lrFactType

            '            Me.zrPage.ERDiagram.Relation.Add(lrRelation)

            '            lrRelation.RDSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.Id = lrRelation.Id)

            '            Dim lrLink As PGS.Link
            '            lrLink = New PGS.Link(Me.zrPage, lrFactInstance, lrOriginatingNode, lrDestinationNode, Nothing, Nothing, lrRelation)
            '            'lrLink.RDSRelation = lrRelation.RDSRelation '20180810-Shouldn't need this. Is in the New constructor.
            '            lrLink.DisplayAndAssociate()

            '            lrRelation.Link = lrLink
            '            lrOriginatingNode.Relation.Add(lrRelation)
            '        End If

            '    End If

            '    lrRecordset.MoveNext()
            'End While
#End Region

            '==================================================================
            'Subtype Relationships
            Call Me.mapSubtypeRelationships()

            If Me.areAllEntitiesAtPoint00() Then
                Call Me.autoLayout()
            End If

            If asSelectModelElementId IsNot Nothing Then
                Dim lrNode As PGS.Node = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Id = asSelectModelElementId)
                If lrNode IsNot Nothing Then
                    lrNode.Shape.Selected = True
                End If
            End If

            Call Me.layoutSelfJoiningLinks()

            Call Me.resetNodeAndLinkColors()

            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Sub SetToolbox()

        Try
            Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.PGSShapeLibrary)

            Dim lrToolboxForm As frmToolbox
            lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.ShapeListBox.Shapes = lsl_shape_library.Shapes
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ETDDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call SetToolbox()

    End Sub

    Private Sub ContextMenuStrip_Node_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Node.Opening

        Dim lrPage As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lrModel As FBM.Model
        Dim lrNode As New PGS.Node

        Try
            Me.ContextMenuStrip_Node.ImageScalingSize = New Drawing.Size(16, 16)

            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            '-------------------------------------
            'Check that selected object is Actor
            '-------------------------------------
            If Me.zrPage.SelectedObject(0).GetType Is lrNode.GetType Then
                '----------
                'All good
                '----------
            Else
                '--------------------------------------------------------
                'Sometimes the MouseDown/NodeSelected gets it wrong
                '  and this sub receives invocation before an Actor is 
                '  properly selected. The user may try and click again.
                '  If it's a bug, then this can be removed obviously.
                '--------------------------------------------------------
                Exit Sub
            End If

            lrNode = Me.zrPage.SelectedObject(0)

            lrModel = lrNode.Model

            '---------------------------------------------------------------------------------------------
            'Model Errors
            '-----------------------------------
            Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Clear()
            Dim lrModelError As FBM.ModelError
            Dim lrModelElement As FBM.ModelObject = lrNode.RDSTable.FBMModelElement
            Dim lo_menu_option As ToolStripItem
            If lrModelElement.ModelError.Count > 0 Then
                For Each lrModelError In lrModelElement.ModelError
                    lo_menu_option = Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Add(lrModelError.Description)
                    lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                Next
                ToolStripMenuItemEntityTypeModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
            Else
                Me.ToolStripMenuItemEntityTypeModelErrors.Image = My.Resources.MenuImages.Cloud216x16
                lo_menu_option = Me.ToolStripMenuItemEntityTypeModelErrors.DropDownItems.Add("There are no Model Errors for this Node Type.")
                lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
            End If

            '---------------------------------------------------------------------------------------------
            'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
            '  shape, and to/into another diagram starts at the MorphVector.
            '---------------------------------------------------------------------------------------------
            Me.MorphVector.Clear()
            'Me.MorphVector.Add(New tMorphVector(lrNode.X, lrNode.Y, 0, 0, 40))
            Me.MorphVector.Add(New tMorphVector(lrNode.Shape.Bounds.X, lrNode.Shape.Bounds.Y, 0, 0, 40))

            '====================================================
            'Load the ORMDiagrams that relate to the EntityType
            '  as selectable menuOptions
            '====================================================
            '--------------------------------------------------------------
            'Clear the list of ORMDiagrams that may relate to the EntityType
            '--------------------------------------------------------------
            Me.ORMDiagramToolStripMenuItem.DropDownItems.Clear()
            Me.ERDiagramToolStripMenuItem.DropDownItems.Clear()
            Me.PGSDiagramToolStripMenuItem.DropDownItems.Clear()

            Dim loMenuOption As ToolStripItem
            '--------------------------------------------------------------------------------------------------------
            'Get the ORM Diagrams for the selected Node.
            larPage_list = prApplication.CMML.getORMDiagramPagesForPGSNode(lrNode)

            For Each lrPage In larPage_list
                '---------------------------------------------------------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is not added for those hidden Pages.
                '----------------------------------------------------------
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                If IsSomething(lr_enterprise_view) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ORM16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.morphToORMDiagram
                End If
            Next

            '--------------------------------------------------------------------------------------------------------
            'Get the ER Diagrams for the selected Node.
            larPage_list = prApplication.CMML.getERDiagramPagesForNode(lrNode)

            For Each lrPage In larPage_list
                '---------------------------------------------------------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is not added for those hidden Pages.
                '----------------------------------------------------------
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                If IsSomething(lr_enterprise_view) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.ERDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ERD16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.morphToERDiagram
                End If
            Next

            '--------------------------------------------------------------------------------------------------------
            'Get the PGS Diagrams for the selected Node.
            larPage_list = prApplication.CMML.getPGSDiagramPagesForModelElementName(lrNode.Model, lrNode.Id)

            For Each lrPage In larPage_list
                '---------------------------------------------------------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is not added for those hidden Pages.
                '----------------------------------------------------------
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                If IsSomething(lr_enterprise_view) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.PGSDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.PGS16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.morphToPGSDiagram
                End If
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub Diagram_CellClicked(sender As Object, e As CellEventArgs) Handles Diagram.CellClicked

        Try
            Dim lrColumn As RDS.Column
            lrColumn = e.Cell.Tag

            Dim lrNodeType As PGS.Node = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrColumn.Table.Name)
            Dim lrAttribute As ERD.Attribute = lrNodeType.Attribute.Find(Function(x) x.Column Is lrColumn)

            Me.zrPage.SelectedObject.Clear()
            Me.zrPage.SelectedObject.Add(lrAttribute)

            If e.MouseButton = MouseButton.Left Then

                e.Cell.TextColor = Color.White
                e.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)
                Me.Diagram.Invalidate()

                Call Me.resetAttributeCellColours()

                '-----------------------------------------------
                'Paint the cell again. First is for GUI speed.
                '-----------------------------------------------
                e.Cell.TextColor = Color.White
                e.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)

                '----------------------------------------------------------------------------------------
                For Each lrDiagramNode In Me.Diagram.Nodes
                    If lrDiagramNode.GetType = GetType(MindFusion.Diagramming.ShapeNode) Then
                        Call lrDiagramNode.Tag.ResetAttributeCellColours()
                    End If
                Next

                e.Cell.TextColor = Color.White
                e.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)
                Me.Diagram.Invalidate()

                '--------------------------------------------------------------------
                'Highlight the Relationships (if any) associated with the Attribute
                '--------------------------------------------------------------------
                If IsSomething(lrAttribute.Relation) Then

                    lrAttribute.Relation.Link.Color = Color.Blue
                    Me.Diagram.Invalidate()
                    Me.ERDiagram.Relation(0).Link.Color = Color.Blue
                End If
                '=========================================


                '--------------------------------------
                'Set the PropertiesGrid.SeletedObject
                '--------------------------------------
                Dim lrPropertyGridForm As frmToolboxProperties

                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                If IsSomething(lrPropertyGridForm) Then
                    Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute})
                    lrPropertyGridForm.PropertyGrid.SelectedObject = e.Cell.Tag
                End If

                '-------------------------------------------------------
                'ORM Verbalisation
                '-------------------------------------------------------
                Dim lrToolboxForm As frmToolboxORMVerbalisation
                lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
                If IsSomething(lrToolboxForm) Then
                    lrToolboxForm.zrModel = Me.zrPage.Model
                    Select Case TypeOf (e.Cell.Tag) Is RDS.Column
                        Case Is = pcenumConceptType.Actor
                            Call lrToolboxForm.VerbaliseColumn(e.Cell.Tag)
                    End Select
                End If
            ElseIf e.MouseButton = MouseButton.Right Then
                ContextMenuStripAttribute.Show(Me.DiagramView, Me.DiagramView.DocToClient(e.MousePosition))
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Dim lsMessage As String

        Try
            Dim larModelElement As New List(Of FBM.ModelObject)
            Dim lrModelElement As FBM.ModelObject = Nothing
            Dim lsFactTypeName As String = ""
            Dim lrFactType As FBM.FactType

            'CodeSafe
            If e.Link.Origin.GetType = GetType(MindFusion.Diagramming.DummyNode) Then Exit Sub
            If e.Link.Destination.GetType = GetType(MindFusion.Diagramming.DummyNode) Then
                Call Me.Diagram.Links.Remove(e.Link)
                Exit Sub
            End If

            lrModelElement = e.Link.Origin.Tag.RDSTable.FBMModelElement
            larModelElement.Add(lrModelElement)
            lsFactTypeName.AppendString(lrModelElement.Id & "RelatesTo")

            If lrModelElement.ModelError.Find(Function(x) x.ErrorId = "105") IsNot Nothing Then
                lsMessage = "You should create a Primary Reference Scheme for the Node Type, " & lrModelElement.Id & ", as soon as possible."
                lsMessage.AppendDoubleLineBreak("A Node Type without a Primary Reference Scheme is like a Table without a Primary Key, or a graph database Node Type without a unique identifier for Nodes at the data level.")
                MsgBox(lsMessage)
            End If

            lrModelElement = e.Link.Destination.Tag.RDSTable.FBMModelElement
            larModelElement.Add(lrModelElement)
            lsFactTypeName.AppendString(lrModelElement.Id)

            If Not lrModelElement.HasPrimaryReferenceScheme Then  'ModelError.Find(Function(x) x.ErrorId = "105") IsNot Nothing Then
                lsMessage = "You need to create a Primary Reference Scheme for the Node Type, " & lrModelElement.Id & "."
                lsMessage.AppendDoubleLineBreak("Linking to a Node Type without a Primary Reference Scheme is like referencing a Table without a Primary Key, or a graph database Node Type without a unique identifier for Nodes at the data level.")
                MsgBox(lsMessage)
                Me.zrPage.Diagram.Links.Remove(e.Link)
                Exit Sub
            End If

            Dim lrModel = Me.zrPage.Model

            lrFactType = lrModel.CreateFactType(lsFactTypeName, larModelElement, False, True, False, Nothing, True, Nothing)
            Dim larRole As New List(Of FBM.Role) From {lrFactType.RoleGroup(0)}

            lrFactType.CreateInternalUniquenessConstraint(larRole, False, True, True, False, Nothing, True)

            larRole = New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}
            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, New List(Of String) From {"relates to", ""})
            lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)

            Me.zrPage.Diagram.Links.Remove(e.Link)

            Dim lrPGSNodeType As PGS.Node = e.Link.Origin.Tag
            Call Me.zrPage.loadRelationsForPGSNode(lrPGSNodeType, True)

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_NodeCreating(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeCreating

        e.Cancel = True

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        frmMain.ToolStripButton_Save.Enabled = True

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        Dim lrShapeNode As ShapeNode

        Dim lrFactInstance As New FBM.FactInstance

        If TypeOf (e.Node) Is ShapeNode Then
            Select Case e.Node.Tag.ConceptType
                Case Is = pcenumConceptType.PGSNode
                    lrShapeNode = e.Node
                    Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                    lrFactDataInstance = lrShapeNode.Tag.FactDataInstance
                    lrFactDataInstance.X = e.Node.Bounds.X
                    lrFactDataInstance.Y = e.Node.Bounds.Y
                    lrShapeNode.Tag.X = lrFactDataInstance.X
                    lrShapeNode.Tag.Y = lrFactDataInstance.Y
                    Call lrShapeNode.Tag.Move(lrFactDataInstance.X, lrFactDataInstance.Y, True)

                    Dim lrLink As MindFusion.Diagramming.DiagramLink
                    For Each lrLink In lrShapeNode.OutgoingLinks
                        lrLink.SegmentCount = 2
                        lrLink.SegmentCount = 1
                        If lrLink.Tag IsNot Nothing Then lrLink.Tag.setPredicate()
                    Next
                    For Each lrLink In lrShapeNode.IncomingLinks
                        lrLink.SegmentCount = 2
                        lrLink.SegmentCount = 1

                        If lrLink.Tag IsNot Nothing Then lrLink.Tag.setPredicate()
                    Next
                Case Else
            End Select

        End If

        Call Me.layoutSelfJoiningLinks()
        Call Me.resetNodeAndLinkColors()

    End Sub

    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        Dim lrNode As Object
        Dim lrPGSNodeType As PGS.Node

        Try

            lrNode = e.Node.Tag

            Select Case lrNode.GetType
                Case Is = GetType(PGS.Node)
                    lrPGSNodeType = lrNode
                    If e.Node.GetType = GetType(MindFusion.Diagramming.ShapeNode) Then
                        e.Node.Pen.Color = Color.Blue
                        lrNode.shape.Image = My.Resources.ORMShapes.Blank
                        Call lrPGSNodeType.NodeSelected()
                    End If
                Case Else
                    'Do nothing
            End Select

            '===================================================================================
            'Draw the Properties of the Node
            If Me.PropertyTableNode IsNot Nothing Then
                If Me.PropertyTableNode.Tag.Id <> lrNode.Id Then
                    Me.Diagram.Nodes.Remove(Me.PropertyTableNode)
                End If
            End If

            If Not TypeOf (e.Node) Is TableNode Then
                If Me.PropertyTableNode Is Nothing And
                    ((Control.ModifierKeys = Keys.Control) Or (Control.ModifierKeys = Keys.ControlKey)) Then

                    Me.PropertyTableNode = Me.zrPage.Diagram.Factory.CreateTableNode(e.Node.Bounds.X, e.Node.Bounds.Y + 25, 30, 20, 1, 0)
                    Me.PropertyTableNode.EnableStyledText = True
                    Me.PropertyTableNode.Caption = "<B>" & " " & lrNode.Name & " "
                    Me.PropertyTableNode.Tag = lrNode

                    Dim lrRDSTable As RDS.Table = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrNode.Id)

                    Dim larColumn = lrRDSTable.Column.ToList.OrderBy(Function(x) x.OrdinalPosition).ToList

                    '--------------------------------------------------------------------
                    'Refined sort of Columns based on Supertype Column ordering and Subtype ordering
                    Dim liInd As Integer
                    Dim larSupertypeTable = lrRDSTable.getSupertypeTables
                    If larSupertypeTable.Count > 0 Then
                        larSupertypeTable.Reverse()
                        larSupertypeTable.Add(lrRDSTable)
                        liInd = 0
                        For Each lrSupertypeTable In larSupertypeTable
                            For Each lrColumn In larColumn.FindAll(Function(x) x.Role.JoinedORMObject.Id = lrSupertypeTable.Name).OrderBy(Function(x) x.OrdinalPosition)
                                larColumn.Remove(lrColumn)
                                larColumn.Insert(liInd, lrColumn)
                                liInd += 1
                            Next
                        Next
                    End If

                    For Each lrColumn In larColumn

                        '============================================================
                        'If lrColumn.ContributesToPrimaryKey And lrRDSTable.Column.Count > 1 Then
                        '    'Don't show the Column
                        'Else

                        '20200326-VM-Removed for demo purposes.
                        'If lrColumn.Relation.FindAll(Function(x) x.OriginTable.Name = lrColumn.Table.Name).Count > 0 Then
                        '    'ForeignKey. Don't show the Column
                        'Else
                        Me.PropertyTableNode.RowCount += 1

                        Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Tag = lrColumn
                        Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Text = lrColumn.Name
                        'End If
                        '============================================================
                    Next
                    Me.PropertyTableNode.ResizeToFitText(True)
                End If
            End If


            '----------------------------------------------------
            'Set the ContextMenuStrip menu for the selected item
            '----------------------------------------------------
            Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
                Case Is = pcenumConceptType.Entity
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Node
            End Select

            Me.zrPage.SelectedObject.Add(e.Node.Tag)

            '--------------------------------------
            'Set the PropertiesGrid.SeletedObject
            '--------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                lrPropertyGridForm.PropertyGrid.SelectedObject = lrPGSNodeType
            End If

            '===========================================
            'Populate the Cypher Toolbox if it is open
            '===========================================
            Dim lrToolboxCypher As frmToolboxCypher
            lrToolboxCypher = prApplication.GetToolboxForm(frmToolboxCypher.Name)
            If IsSomething(lrToolboxCypher) Then
                lrToolboxCypher.zrModel = Me.zrPage.Model
                lrToolboxCypher.zrPage = Me.zrPage
                Call lrToolboxCypher.DevelopCypherText()
            End If

            '-------------------------------------------------------
            'ORM Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Call lrToolboxForm.VerbalisePGSNode(lrNode)
            End If

            '---------------------------------------------------------------------------
            'ORM(FactType) Reading Editor
            'For if the Node represents a FactType.        
            '  matches the selected FactType (if a FactType is selected by the user)
            '---------------------------------------------------------------------------
            Dim lrORMReadingEditor As frmToolboxORMReadingEditor
            lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

            If IsSomething(lrORMReadingEditor) And lrNode.GetType = GetType(PGS.Node) Then

                Dim lrFactTypeInstance As FBM.FactTypeInstance = Nothing
                Dim lrPGSNode As PGS.Node = CType(lrNode, PGS.Node)

                If lrPGSNode.RDSTable.FBMModelElement.GetType = GetType(FBM.FactType) Then
                    Dim lrFactType As FBM.FactType = lrPGSNode.RDSTable.FBMModelElement
                    lrFactTypeInstance = lrFactType.CloneInstance(New FBM.Page(Me.zrPage.Model), False)

                    lrORMReadingEditor.zrPage = Me.zrPage
                    lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance

                    Call lrORMReadingEditor.SetupForm()
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

    Public Sub mapSubtypeRelationships()

        Try
            Dim lrEntity, lrSubtypeEntity As PGS.Node

            For Each lrEntity In Me.zrPage.ERDiagram.Entity

                Dim lrRDSTable = lrEntity.getCorrespondingRDSTable

                For Each lrSubtypeTable In lrRDSTable.getSubtypeTables

                    lrSubtypeEntity = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrSubtypeTable.Name)

                    If lrSubtypeEntity IsNot Nothing Then
                        Dim lo_link As New DiagramLink(Me.zrPage.Diagram, lrSubtypeEntity.Shape, lrEntity.Shape)
                        lo_link.HeadShape = ArrowHead.Arrow
                        lo_link.Pen.Color = Color.Gray
                        lo_link.HeadPen.Color = Color.Gray
                        lo_link.ShadowColor = Color.White
                        lo_link.HeadShapeSize = 3
                        lo_link.Pen.DashStyle = DashStyle.Dash
                        lo_link.Pen.Width = 0.001
                        lo_link.Locked = True
                        Me.zrPage.Diagram.Links.Add(lo_link)
                        'lrSubtypeEntity.Shape.OutgoingLinks.Add(lo_link)
                    End If
                Next

            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub morphToERDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            Me.HiddenDiagramView.ZoomFactor = Me.DiagramView.ZoomFactor

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            'If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Dim lrPageObject As New FBM.PageObject
            lrPageObject = Me.zrPage.SelectedObject(0).ClonePageObject

            Dim lrShapeNode As ShapeNode
            lrShapeNode = lrPageObject.Shape.Clone(True)
            lrShapeNode = New ShapeNode(lrPageObject.Shape)
            lrShapeNode.Shape = Shapes.Ellipse
            lrShapeNode.Pen.Width = 0.5
            lrShapeNode.Text = lrPageObject.Name

            If Me.zrPage.SelectedObject(0).GetType Is GetType(Boston.ERD.Relation) Then
                lrShapeNode.SetRect(New RectangleF(New PointF(lrPageObject.X, lrPageObject.Y), New SizeF(20, 20)), False)
            End If

            Me.MorphVector(0).ModelElementId = lrPageObject.Name 'Me.zrPage.SelectedObject(0).id
            Me.MorphVector(0).Shape = lrShapeNode
            Me.HiddenDiagram.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = item.Tag 'Set when the ContextMenu.Opening event is triggered.            
                Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
                prApplication.WorkingPage = lrEnterpriseView.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
                lrPage = lrEnterpriseView.Tag

                Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                End If

                Dim lrNode As PGS.Node

                '-------------------------------------------------------------------------------------
                'Populate the MorphVector with each relevant ModelObjectInstance on the current Page
                '  that is also on the destination Page.
                '----------------------------------------------------------------------
#Region "Additional MorphVectors"
                Dim lrAdditionalShapeNode As ShapeNode = Nothing
                Dim larConceptTypes = {pcenumConceptType.EntityType, pcenumConceptType.FactType}

                For Each lrAdditionalObject As PGS.Node In Me.zrPage.ERDiagram.Entity
                    If lrAdditionalObject.Id = Me.zrPage.SelectedObject(0).Id Then
                        '---------------------------------------------------------------------------------------------
                        'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                        '---------------------------------------------------------------------------------------------
                    Else
                        If lrAdditionalObject.NodeType <> pcenumPGSEntityType.Relationship And lrAdditionalObject.PGSRelation Is Nothing Then

                            Dim larEntityList = (From Entity In lrPage.ERDiagram.Entity
                                                 Where Entity.Name = lrAdditionalObject.Name
                                                 Select Entity.ClonePGSNodeType(Me.zrPage))

                            For Each lrNode In larEntityList
                                'Will only be one, but saves coding if...then...
                                Me.MorphVector.Add(New tMorphVector(lrAdditionalObject.Shape.Bounds.X, lrAdditionalObject.Shape.Bounds.Y, lrNode.X, lrNode.Y, 40))

                                Dim lrAdditionalPageObject As FBM.PageObject = lrAdditionalObject.ClonePageObject
                                lrAdditionalShapeNode = lrAdditionalPageObject.Shape.Clone(True)
                                lrAdditionalShapeNode = New ShapeNode(lrAdditionalPageObject.Shape)
                                lrAdditionalShapeNode.Text = lrAdditionalObject.Name
                                lrAdditionalShapeNode.Visible = True
                                lrAdditionalShapeNode.Move(lrAdditionalPageObject.X, lrAdditionalPageObject.Y)
                                lrAdditionalShapeNode.Shape = Shapes.Ellipse
                                lrAdditionalShapeNode.Pen.Width = 0.5

                                Me.HiddenDiagram.Nodes.Add(lrAdditionalShapeNode)
                                Me.MorphVector(Me.MorphVector.Count - 1).ModelElementId = lrNode.Name
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape = lrAdditionalShapeNode
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.Font = Me.zrPage.Diagram.Font
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat.Alignment = StringAlignment.Center
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat.LineAlignment = StringAlignment.Center
                            Next

                        End If
                    End If
                Next
#End Region

                Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance _
                                   From FactInstance In FactTypeInstance.Fact _
                                   From FactDataInstance In FactInstance.Data _
                                   Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                   And FactDataInstance.Role.Name = pcenumCMML.Element.ToString _
                                   And FactDataInstance.Concept.Symbol = lrPageObject.Name _
                                   Select New FBM.FactDataInstance(lrPage, FactInstance, FactDataInstance.Role, FactDataInstance.Concept, FactDataInstance.X, FactDataInstance.Y)

                Dim lrFactDataInstance As New FBM.FactDataInstance
                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                If lrFactDataInstance.TableShape IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrFactDataInstance.X,
                                                              lrFactDataInstance.Y,
                                                              lrFactDataInstance.TableShape.Bounds.Width,
                                                              lrFactDataInstance.TableShape.Bounds.Height)
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                End If

                'Start size
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)

                '===========================================
                Dim lrEntity As ERD.Entity = lrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrPageObject.Name)
                If lrEntity IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrEntity.X,
                                                              lrEntity.Y,
                                                              lrEntity.TableShape.Bounds.Width,
                                                              lrEntity.TableShape.Bounds.Height)
                    Me.MorphVector(0).EndPoint = New Point(lrEntity.TableShape.Bounds.X - lrPage.DiagramView.ScrollX, lrEntity.TableShape.Bounds.Y - lrPage.DiagramView.ScrollY)
                    Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrEntity.TableShape.Bounds.X - lrShapeNode.Bounds.X) + Math.Abs(lrEntity.TableShape.Bounds.Y - lrShapeNode.Bounds.Y) + 1))
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.X, lrFactDataInstance.Y)
                    Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrFactDataInstance.X - lrShapeNode.Bounds.X) + Math.Abs(lrFactDataInstance.Y - lrShapeNode.Bounds.Y) + 1))
                End If
                '===========================================
                Me.MorphVector(0).Shape.Font = Me.zrPage.Diagram.Font
                Me.MorphVector(0).Shape.Text = Me.MorphVector(0).ModelElementId
                Me.MorphVector(0).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                Me.MorphVector(0).Shape.TextFormat.Alignment = StringAlignment.Center
                Me.MorphVector(0).Shape.TextFormat.LineAlignment = StringAlignment.Center

                Me.MorphVector(0).Shape.SetRect(New RectangleF(lrPageObject.X, lrPageObject.Y, 20, 20), False)
                Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)

                Me.MorphVector(0).TargetShape = pcenumTargetMorphShape.RoundRectangle

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            Me.DiagramView.BringToFront()
        End Try

    End Sub

    Public Sub morphToORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        Try

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            Dim loDiagrammingShape As MindFusion.Diagramming.Shape

            Dim lrNode As New PGS.Node
            If TypeOf (Me.zrPage.SelectedObject(0)) Is PGS.Node Then
                lrNode = Me.zrPage.SelectedObject(0)
                loDiagrammingShape = Shapes.Ellipse
            Else
                Dim lrRelation As ERD.Relation
                lrRelation = Me.zrPage.SelectedObject(0)

                If lrRelation.IsPGSRelationNode Then
                    lrNode = New PGS.Node(Me.zrPage, lrRelation.ActualPGSNode.Id)
                Else
                    lrNode = New PGS.Node(Me.zrPage, lrRelation.RelationFactType.Id)
                End If

                lrNode.X = lrRelation.Link.Link.Bounds.X
                lrNode.Y = lrRelation.Link.Link.Bounds.Y

                lrNode.shape.SetRect(New RectangleF(lrRelation.Link.Link.Bounds.X,
                                     lrRelation.Link.Link.Bounds.Y,
                                     lrRelation.Link.Link.Bounds.Width,
                                     lrRelation.Link.Link.Bounds.Height),
                                     False)

                loDiagrammingShape = Shapes.RoundRect
            End If

            Dim lrShapeNode As MindFusion.Diagramming.ShapeNode
            'lrShapeNode = lrPageObject.Shape.Clone(False)
            lrShapeNode = New MindFusion.Diagramming.ShapeNode(lrNode.shape)
            lrShapeNode.Shape = loDiagrammingShape
            lrShapeNode.HandlesStyle = HandlesStyle.InvisibleMove
            lrShapeNode.SetRect(lrNode.shape.Bounds, False)
            lrShapeNode.Font = New System.Drawing.Font("Arial", 10) 'lrShapeNode.Font = Me.zrPage.Diagram.Font
            lrShapeNode.TextFormat.Alignment = StringAlignment.Center
            lrShapeNode.Text = lrNode.Data
            lrShapeNode.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
            lrShapeNode.TextFormat.Alignment = StringAlignment.Center
            lrShapeNode.TextFormat.LineAlignment = StringAlignment.Center
            lrShapeNode.Pen.Width = 0.5

            Me.MorphVector(0).Shape = lrShapeNode


            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = item.Tag
                prApplication.WorkingPage = lrEnterpriseView.Tag

                '------------------------------------------------------
                'Get the X,Y co-ordinates of the Entity being morphed
                '------------------------------------------------------
                Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
                lrPage = lrEnterpriseView.Tag

                Dim lrEntityTypeInstance As New FBM.EntityTypeInstance(lrPage.Model, pcenumLanguage.ORMModel, lrNode.Data, True, 0, 0)
                Dim lrFactTypeInstance As New FBM.FactTypeInstance(lrPage.Model, lrPage, pcenumLanguage.ORMModel, lrNode.Data, True, 0, 0)

                lrFactTypeInstance = lrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                lrEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

                If lrPage.Loaded Then
                    If IsSomething(lrFactTypeInstance) Then
                        Me.MorphVector(0).EndPoint = New Point(lrFactTypeInstance.X, lrFactTypeInstance.Y)
                        If lrFactTypeInstance.Shape IsNot Nothing Then
                            Me.MorphVector(0).EndSize = New Rectangle(lrFactTypeInstance.X,
                                                                      lrFactTypeInstance.Y,
                                                                      lrFactTypeInstance.Shape.Bounds.Width,
                                                                      lrFactTypeInstance.Shape.Bounds.Height)
                        Else
                            Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                        End If
                    ElseIf IsSomething(lrEntityTypeInstance) Then
                        Me.MorphVector(0).EndPoint = New Point(lrEntityTypeInstance.X, lrEntityTypeInstance.Y)
                        If lrEntityTypeInstance.Shape IsNot Nothing Then
                            Me.MorphVector(0).EndSize = New Rectangle(lrEntityTypeInstance.X,
                                                                      lrEntityTypeInstance.Y,
                                                                      lrEntityTypeInstance.Shape.Bounds.Width,
                                                                      lrEntityTypeInstance.Shape.Bounds.Height)
                        Else
                            Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                        End If
                    End If
                End If

                'Start size
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)

                '-------------------------------------------------------------------------------------
                'Populate the MorphVector with each relevant ModelObjectInstance on the current Page
                '  that is also on the destination Page.
                '----------------------------------------------------------------------
#Region "Additional MorphVectors"
                Dim lrAdditionalShapeNode As ShapeNode = Nothing
                Dim larConceptTypes = {pcenumConceptType.EntityType, pcenumConceptType.FactType}

                For Each lrAdditionalObject As PGS.Node In Me.zrPage.ERDiagram.Entity
                    If lrAdditionalObject.Id = Me.zrPage.SelectedObject(0).Id Then
                        '---------------------------------------------------------------------------------------------
                        'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                        '---------------------------------------------------------------------------------------------
                    Else
                        If lrAdditionalObject.NodeType <> pcenumPGSEntityType.Relationship And lrAdditionalObject.PGSRelation Is Nothing Then
                            Dim larORMObjectList = From ModelObject In lrPage.GetAllPageObjects
                                                   Where ModelObject.Id = lrAdditionalObject.Name
                                                   Select ModelObject

                            For Each lrModelObject As Object In larORMObjectList
                                'Will only be one, but saves coding if...then...
                                Me.MorphVector.Add(New tMorphVector(lrAdditionalObject.Shape.Bounds.X, lrAdditionalObject.Shape.Bounds.Y, lrModelObject.X, lrModelObject.Y, 40))

                                Dim lrAdditionalPageObject As FBM.PageObject = lrAdditionalObject.ClonePageObject
                                lrAdditionalShapeNode = lrAdditionalPageObject.Shape.Clone(True)
                                lrAdditionalShapeNode = New ShapeNode(lrAdditionalPageObject.Shape)
                                lrAdditionalShapeNode.Text = lrAdditionalObject.Name
                                lrAdditionalShapeNode.Visible = True
                                lrAdditionalShapeNode.Move(lrAdditionalPageObject.X, lrAdditionalPageObject.Y)
                                lrAdditionalShapeNode.Shape = Shapes.Ellipse
                                lrAdditionalShapeNode.Pen.Width = 0.5

                                Me.HiddenDiagram.Nodes.Add(lrAdditionalShapeNode)
                                Me.MorphVector(Me.MorphVector.Count - 1).ModelElementId = lrNode.Name
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape = lrAdditionalShapeNode
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.Font = Me.zrPage.Diagram.Font
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat.Alignment = StringAlignment.Center
                                Me.MorphVector(Me.MorphVector.Count - 1).Shape.TextFormat.LineAlignment = StringAlignment.Center
                            Next

                        End If
                    End If
                Next
#End Region


                If lrPage.Loaded Then
                    If IsSomething(lrFactTypeInstance) Then
                        Me.MorphVector(0).EndPoint = New Point(lrFactTypeInstance.X, lrFactTypeInstance.Y)
                        If lrFactTypeInstance.Shape IsNot Nothing Then
                            Me.MorphVector(0).EndSize = New Rectangle(lrFactTypeInstance.X,
                                                                      lrFactTypeInstance.Y,
                                                                      lrFactTypeInstance.Shape.Bounds.Width,
                                                                      lrFactTypeInstance.Shape.Bounds.Height)
                        Else
                            Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                        End If
                        Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrFactTypeInstance.X - lrNode.X) + Math.Abs(lrFactTypeInstance.Y - lrNode.Y) + 1))

                    ElseIf IsSomething(lrEntityTypeInstance) Then
                        Me.MorphVector(0).EndPoint = New Point(lrEntityTypeInstance.X, lrEntityTypeInstance.Y)
                        If lrEntityTypeInstance.Shape IsNot Nothing Then
                            Me.MorphVector(0).EndSize = New Rectangle(lrEntityTypeInstance.X,
                                                                      lrEntityTypeInstance.Y,
                                                                      lrEntityTypeInstance.Shape.Bounds.Width,
                                                                      lrEntityTypeInstance.Shape.Bounds.Height)
                        Else
                            Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                        End If

                        Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrEntityTypeInstance.X - lrNode.X) + Math.Abs(lrEntityTypeInstance.Y - lrNode.Y) + 1))
                    End If

                End If

                Me.MorphVector(0).ModelElementId = lrNode.Id
                Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                Else
                    Me.MorphVector(0).TargetZoomFactor = My.Settings.DefaultPageZoomFactor
                End If

                Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
                Me.HiddenDiagram.Invalidate()

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True

                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Me.CircularProgressBar.SendToBack()
            Me.HiddenDiagramView.SendToBack()
        End Try

    End Sub

    Public Sub ResetPropertiesGridToolbox(ByRef arObject As Object)

        Try

            If IsSomething(arObject) Then
                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

                If IsSomething(lrPropertyGridForm) Then
                    Dim lrModelObject As FBM.ModelObject
                    lrModelObject = arObject
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    Select Case lrModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                            lrEntityTypeInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                        Case Is = pcenumConceptType.EntityTypeName
                            Dim lrEntityTypeName As FBM.EntityTypeName
                            lrEntityTypeName = lrModelObject
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeName.EntityTypeInstance
                        Case Is = pcenumConceptType.RoleConstraintRole
                            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                            lrRoleConstraintRoleInstance = lrModelObject
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintRoleInstance.RoleConstraint
                        Case Is = pcenumConceptType.RoleConstraint

                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            lrRoleConstraintInstance = lrModelObject
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
                        Case Else
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                    End Select
                End If
            Else
                Throw New Exception("No object passed as argument, 'arObject'")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub



    Public Sub morphToPGSDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            Me.HiddenDiagramView.ZoomFactor = Me.DiagramView.ZoomFactor

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            'If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Dim lrPageObject As New FBM.PageObject
            lrPageObject = Me.zrPage.SelectedObject(0).ClonePageObject

            Dim lrShapeNode As ShapeNode

            lrShapeNode = lrPageObject.Shape.Clone(True)
            lrShapeNode = New ShapeNode(lrPageObject.Shape)

            '=================================================            
            lrShapeNode.Shape = Shapes.Ellipse
            lrShapeNode.HandlesStyle = HandlesStyle.InvisibleMove
            'lrShapeNode.SetRect(lrPageObject.Shape.Bounds, False)
            lrShapeNode.Font = New System.Drawing.Font("Arial", 10) 'lrShapeNode.Font = Me.zrPage.Diagram.Font
            lrShapeNode.TextFormat.Alignment = StringAlignment.Center
            lrShapeNode.Text = lrPageObject.Name
            lrShapeNode.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
            lrShapeNode.TextFormat.Alignment = StringAlignment.Center
            lrShapeNode.TextFormat.LineAlignment = StringAlignment.Center
            '=================================================            

            Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
            Me.MorphVector(0).Shape = lrShapeNode
            Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
            Me.HiddenDiagram.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = item.Tag 'Set when the ContextMenu.Opening event is triggered.            
                Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
                prApplication.WorkingPage = lrEnterpriseView.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
                lrPage = lrEnterpriseView.Tag

                Me.MorphVector(0).InitialZoomFactor = Me.DiagramView.ZoomFactor
                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                End If

                Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance _
                                   From FactInstance In FactTypeInstance.Fact _
                                   From FactDataInstance In FactInstance.Data _
                                   Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                   And FactDataInstance.Role.Name = pcenumCMML.Element.ToString _
                                   And FactDataInstance.Concept.Symbol = lrPageObject.Name _
                                   Select New FBM.FactDataInstance(lrPage, FactInstance, FactDataInstance.Role, FactDataInstance.Concept, FactDataInstance.X, FactDataInstance.Y)

                Dim lrFactDataInstance As New FBM.FactDataInstance
                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                If lrFactDataInstance.shape IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrFactDataInstance.X,
                                                              lrFactDataInstance.Y,
                                                              lrFactDataInstance.shape.Bounds.Width,
                                                              lrFactDataInstance.shape.Bounds.Height)
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 10)
                End If

                'Start size
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)

                '===========================================
                Dim lrNode As PGS.Node

                lrNode = lrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrPageObject.Name)

                If lrNode IsNot Nothing Then
                    Me.MorphVector(0).EndSize = New Rectangle(lrNode.X,
                                                              lrNode.Y,
                                                              lrNode.shape.Bounds.Width,
                                                              lrNode.shape.Bounds.Height)
                    Me.MorphVector(0).EndPoint = New Point(lrNode.Shape.Bounds.X, lrNode.Shape.Bounds.Y) ' (lrFactDataInstance.x, lrFactDataInstance.y)
                    Me.MorphVector(0).VectorSteps = Viev.Greater(25, (Math.Abs(lrNode.Shape.Bounds.X - lrNode.X) + Math.Abs(lrNode.Shape.Bounds.Y - lrNode.Y) + 1))
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 20)
                    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.X, lrFactDataInstance.Y)
                End If
                '===========================================

                Me.MorphVector(0).TargetShape = pcenumTargetMorphShape.Circle

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub MorphToProcessBasedDiagram(ByVal sender As Object, ByVal e As EventArgs)

        'Dim lrFactDataInstance As New Object
        'Dim lrMenuItem As ToolStripItem = CType(sender, ToolStripItem)

        ''---------------------------------------------
        ''Take a copy of the selected Actor/EntityType
        ''---------------------------------------------
        ''Me.ORMDiagramView.CopyToClipboard(False)

        'Me.HiddenDiagram.Nodes.Clear()
        'Call Me.ERDDiagramView.SendToBack()
        'Call Me.HiddenDiagramView.BringToFront()

        ''--------------------------------------------------------------
        ''Paste the selected Actor/EntityType to the HiddenDiagramView
        ''  (for animated morphing)
        ''--------------------------------------------------------------
        'Dim lrShapeNode As ShapeNode
        'lrShapeNode = Me.zrPage.SelectedObject(0).Shape
        'lrShapeNode = Me.HiddenDiagram.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
        'lrShapeNode.Shape = Shapes.RoundRect
        'lrShapeNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
        'lrShapeNode.Text = Me.zrPage.SelectedObject(0).Name
        'lrShapeNode.Pen.Color = Color.Black
        'lrShapeNode.Visible = True

        'Me.MorphVector(0).Shape = lrShapeNode

        'Me.HiddenDiagram.Invalidate()

        'If IsSomething(frmMain.zfrmModelExplorer) Then
        '    Dim lrEnterpriseView As tEnterpriseView
        '    lrEnterpriseView = lrMenuItem.Tag
        '    Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
        '    prApplication.WorkingPage = lrEnterpriseView.Tag

        '    '---------------------------------------------
        '    'Set the Page that is going to be morphed to
        '    '---------------------------------------------
        '    Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
        '    lrPage = lrEnterpriseView.Tag

        '    '----------------------------------------------------------------------
        '    'Populate the MorphVector with each Process Shape on the current Page
        '    '  that is also on the destination Page.
        '    '----------------------------------------------------------------------
        '    Dim lrAdditionalProcess As CMML.Process
        '    For Each lrAdditionalProcess In Me.DataFlowModel.Process
        '        If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
        '            '---------------------------------------------------------------------------------------------
        '            'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
        '            '---------------------------------------------------------------------------------------------
        '        Else
        '            Dim lrEntityList = From FactTypeInstance In lrPage.FactTypeInstance _
        '                               From Fact In FactTypeInstance.Fact _
        '                               From FactData In Fact.Data _
        '                               Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
        '                               And FactData.Role.Name = "Element" _
        '                               And FactData.Concept.Symbol = lrAdditionalProcess.Name _
        '                               Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
        '            For Each lrFactDataInstance In lrEntityList
        '                Exit For
        '            Next
        '            Me.MorphVector.Add(New tMorphVector(lrAdditionalProcess.X, lrAdditionalProcess.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40))

        '            lrShapeNode = lrAdditionalProcess.shape
        '            lrShapeNode = Me.HiddenDiagram.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
        '            lrShapeNode.Shape = Shapes.Ellipse
        '            lrShapeNode.Text = lrAdditionalProcess.Name
        '            lrShapeNode.Visible = True
        '            Me.HiddenDiagram.Nodes.Add(lrShapeNode)

        '            Me.MorphVector(Me.MorphVector.Count - 1).Shape = lrShapeNode

        '        End If
        '    Next

        '    '----------------------------------------------------------------
        '    'Get the X,Y co-ordinates of the Actor/EntityType being morphed
        '    '----------------------------------------------------------------
        '    Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance _
        '                              From Fact In FactTypeInstance.Fact _
        '                              From FactData In Fact.Data _
        '                              Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
        '                              And FactData.Role.Name = "Element" _
        '                              And FactData.Concept.Symbol = Me.zrPage.SelectedObject(0).Name _
        '                              Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)


        '    For Each lrFactDataInstance In larFactDataInstance
        '        Exit For
        '    Next

        '    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.x, lrFactDataInstance.y)
        '    Me.MorphVector(0).VectorSteps = 40
        '    Me.MorphTimer.Enabled = True
        '    Me.MorphStepTimer.Enabled = True

        'End If
    End Sub

    Private Sub PolarToCartesean(ByVal coordCenter As PointF, ByVal a As Single, ByVal r As Single, ByRef dekart As PointF)
        If r = 0 Then
            dekart = coordCenter
            Return
        End If

        dekart.X = CSng(coordCenter.X + Math.Cos(a * Math.PI / 180) * r)
        dekart.Y = CSng(coordCenter.Y - Math.Sin(a * Math.PI / 180) * r)
    End Sub


    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        'Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lrPoint As New Point
        Dim lrRectangle As New Rectangle

        For Each lrMorphVector In Me.MorphVector
            lrPoint = lrMorphVector.getNextMorphVectorStepPoint
            lrMorphVector.Shape.Move(lrPoint.X, lrPoint.Y)

            lrRectangle = lrMorphVector.getNextMorphVectorRectangle
            lrMorphVector.Shape.Resize(lrRectangle.Width, lrRectangle.Height)

            If lrMorphVector.VectorStep > lrMorphVector.VectorSteps / 2 Then
                Select Case lrMorphVector.TargetShape
                    Case Is = pcenumTargetMorphShape.Circle
                        lrMorphVector.Shape.Shape = Shapes.Ellipse
                        lrMorphVector.Shape.Image = My.Resources.ORMShapes.Blank
                    Case Else
                        lrMorphVector.Shape.Shape = Shapes.RoundRect
                End Select
            Else
                lrMorphVector.Shape.Image = My.Resources.ORMShapes.Blank
            End If
        Next

        Me.HiddenDiagramView.ZoomFactor = Me.MorphVector(0).InitialZoomFactor + ((Me.MorphVector(0).VectorStep / Me.MorphVector(0).VectorSteps) * (Me.MorphVector(0).TargetZoomFactor - Me.MorphVector(0).InitialZoomFactor))


        Me.HiddenDiagram.Invalidate()

        If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
            Me.MorphStepTimer.Stop()
            Me.MorphStepTimer.Enabled = False
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphStepTimer.Tag
            Call frmMain.zfrmModelExplorer.LoadSelectedPage(Me.MorphVector(0).ModelElementId)
            Me.DiagramView.BringToFront()
            Me.Diagram.Invalidate()
            Me.MorphTimer.Enabled = False
        End If

    End Sub

    Private Sub DiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim lsMessage As String

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            '------------------------------------------------------------------------------------------------------------------------------------
            'Make sure the current page points to the Diagram on this form. The reason is that the user may be viewing the Page as an ORM Model
            '------------------------------------------------------------------------------------------------------------------------------------
            Me.zrPage.Diagram = Me.Diagram

            Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
            Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

            Dim lrNode As PGS.Node

            If loDraggedNode.Index >= 0 Then

                Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

                If loDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount Then

                    Select Case lrToolboxForm.ShapeListBox.Shapes(loDraggedNode.Index).Id
                        Case Is = "Node Type"
                            '---------------------------------------------------------------
                            'Establish a new EntityType and Entity for the dropped object.
                            '---------------------------------------------------------------    

                            With New WaitCursor
                                Dim lrEntityType As FBM.EntityType
                                Dim lsNodeTypeName As String = Me.zrPage.Model.CreateUniqueEntityName("New Node Type")
                                lrEntityType = Me.zrPage.Model.CreateEntityType(lsNodeTypeName, True, True, False)

                                If My.Settings.UseDefaultReferenceModeNewEntityTypes Then
                                    Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode)
                                    Call lrEntityType.SetDataType(pcenumORMDataType.TextFixedLength, 50, 0, True)
                                Else
                                    Dim lsErrorMessage As String = "Entity Type Requires Reference Scheme Error - Entity Type '" & lrEntityType.Id & "'."
                                    Dim lrModelError As New FBM.ModelError(pcenumModelErrors.EntityTypeRequiresReferenceSchemeError,
                                                                    lsErrorMessage,
                                                                    Nothing,
                                                                    lrEntityType)
                                    Call lrEntityType.AddModelError(New FBM.ModelError(105, lrEntityType))
                                End If

                                '20220522-Was below...but the creation of the EntityType should have created the RDS.Table.
                                'Dim lrTable As New RDS.Table(Me.zrPage.Model.RDS, lrEntityType.Id, lrEntityType)
                                'Call Me.zrPage.Model.RDS.addTable(lrTable)
                                Dim lrTable As RDS.Table = lrEntityType.getCorrespondingRDSTable

                                '=========================================================================================
                                Dim lrNodeType As PGS.Node = Me.zrPage.LoadPGSNodeTypeFromRDSTable(lrTable, loPointF)
                                'Call Me.zrPage.DropPGSNodeTypeAtPoint(lrNodeType, loPointF)

                                'Leave at this point in the code.
                                lrNodeType.RDSTable = lrTable


                                Me.zrPage.ERDiagram.Entity.AddUnique(lrNodeType)
                            End With
                            '=========================================================================================
                    End Select
                End If

            ElseIf loDraggedNode.Tag.GetType Is GetType(RDS.Table) Then

                Dim lrTable As RDS.Table
                lrTable = loDraggedNode.Tag

                Dim lrClashNode As PGS.Node = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)
                If lrClashNode IsNot Nothing Then
                    lrNode = lrClashNode
                    'The Node is already on the Page.
                    lsMessage = "This Page already contains a Node with the name, '" & lrTable.Name & "'."
                    If lrClashNode.getCorrespondingRDSTable.isPGSRelation Then
                        lsMessage &= vbCrLf & vbCrLf & "If the existing Node is not visible it is because it is represented by an Edge/Relation."

                        Dim lrCustomMessageBox As New frmCustomMessageBox

                        lrCustomMessageBox.Message = lsMessage
                        lrCustomMessageBox.ButtonText.Add("Ok")
                        lrCustomMessageBox.ButtonText.Add("Display as PGS Node")
                        lrCustomMessageBox.ButtonText.Add("Display Anyway")

                        Select Case lrCustomMessageBox.ShowDialog
                            Case Is = "Display as PGS Node"
                                Call lrClashNode.RDSTable.setIsPGSRelation(False)

                                '20220522-Was...but was displaying the node twice.
                                'Call Me.zrPage.DropExistingPGSNodeAtPoint(lrPGSRelation.ActualPGSNode, New PointF(20, 20))

                                'Call Me.zrPage.loadRelationsForPGSNode(lrPGSRelation.ActualPGSNode)
                                Call Me.zrPage.loadPropertyRelationsForPGSNode(lrClashNode)
                            Case Is = "Display Anyway"
                                Call Me.zrPage.LoadPGSNodeTypeFromRDSTable(lrTable, loPointF, True)
                        End Select
                    End If

                    Exit Sub
                End If

                '------------------
                'Load the PGS Node from the RDS.Table.
                '==================================================================================================================
                Call Me.zrPage.LoadPGSNodeTypeFromRDSTable(lrTable, loPointF)
            End If

        End If

    End Sub

    Private Sub DiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragOver

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim lrDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            '-----------------------------------------------------------------------
            'Get the Object being dragged (if there is one).
            '  If the user is dragging from the ModelDictionary form, 
            '  then the DragItem will have a Tag of the ModelObject being dragged.
            '-----------------------------------------------------------------------
            Dim lrModelOject As Object
            lrModelOject = lrDraggedNode.Tag

            If Not (TypeOf (lrModelOject) Is MindFusion.Diagramming.Shape) Then
                e.Effect = DragDropEffects.Copy
            ElseIf lrDraggedNode.Index >= 0 Then
                Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
                If (lrDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount) Then
                    Select Case lrDraggedNode.Tag.Id
                        Case Is = "Node Type"
                            e.Effect = DragDropEffects.Copy
                        Case Else
                            e.Effect = DragDropEffects.None
                    End Select
                End If
            End If
        ElseIf e.Data.GetDataPresent("System.Windows.Forms.TreeNode", False) Then
            '-------------------------------------------------------------------
            'If the item is a TreeNode item from the EnterpriseTreeView form
            '-------------------------------------------------------------------
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent(GetType(ERD.Entity).ToString, False) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub ERDDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown


        Dim lo_point As System.Drawing.PointF
        Dim loNode As DiagramNode = Nothing

        Try
            'Me.DiagramView.Behavior = Behavior.DrawLinks

            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

            '--------------------------------------------------
            'Just to be sure...set the Richmond.WorkingProject
            '--------------------------------------------------
            loNode = Diagram.GetNodeAt(lo_point)

            Call prApplication.setWorkingPage(Me.zrPage)

            If (ModifierKeys = (Keys.Control Or Keys.ShiftKey)) And My.Settings.SuperuserMode Then
                For Each lrPGSRelationNode In Me.zrPage.ERDiagram.Entity.FindAll(Function(x) x.getCorrespondingRDSTable.isPGSRelation)
                    lrPGSRelationNode.Shape.Visible = True
                    If Not Me.zrPage.Diagram.Nodes.Contains(lrPGSRelationNode.Shape) Then
                        Me.zrPage.Diagram.Nodes.Add(lrPGSRelationNode.Shape)
                        'Make sure the Shape has a Tag
                        lrPGSRelationNode.Shape.Tag = lrPGSRelationNode
                    End If
                    Me.zrPage.Diagram.Invalidate()
                Next
            End If

            If IsSomething(loNode) And (e.Button = Windows.Forms.MouseButtons.Left) Then

                If 1 = 0 Then
                    '----------------------------------------
                    'Put MultiSelection Code here if needed
                    '----------------------------------------
                Else
                    '------------------------------------------------------------------------------
                    ' Otherwise clear the selected objects.
                    '  Will be populated by the event on the ShapeObject itself
                    '-------------------------------------------------------------------------------

                    Me.zrPage.SelectedObject.Clear()
                    Try
                        Me.Diagram.Selection.Clear()
                    Catch ex As Exception
                        'Not a biggie. Can't work out why if fails however.
                    End Try

                    '----------------------------------------------------
                    'Select the ShapeNode/ORMObject just clicked on
                    '  Updates the Me.zrPage.SelectedObject collection.
                    '----------------------------------------------------                 
                    If loNode.GetType = GetType(MindFusion.Diagramming.ShapeNode) Then
                        loNode.Selected = True
                    End If

                End If

                Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Node


                '----------------------------
                'Mouse is over an ShapeNode
                '----------------------------
                Me.Diagram.AllowUnconnectedLinks = True

                '--------------------------------------------
                'Turn on the TimerLinkSwitch.
                '  After the user has held down the mouse button for 1second over a object,
                '  then link creation will be allowed
                '--------------------------------------------
                'TimerLinkSwitch.Enabled = True

                '----------------------------------------------------
                'Get the Node/Shape under the mouse cursor.
                '----------------------------------------------------
                loNode = Diagram.GetNodeAt(lo_point)
                Me.DiagramView.DrawLinkCursor = Cursors.Hand
                Cursor.Show()


            ElseIf IsSomething(Diagram.GetLinkAt(lo_point, 2)) Then
                '-------------------------
                'User clicked on a link
                '-------------------------
                Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Relation

                Dim lrPGSRelation As PGS.Link = Diagram.GetLinkAt(lo_point, 2).Tag

                If lrPGSRelation Is Nothing Then Exit Sub

                lrPGSRelation.Link.TextStyle = LinkTextStyle.Rotate
                If lrPGSRelation.Link.Origin Is lrPGSRelation.Link.Destination Then
                    lrPGSRelation.Link.Font = New Font("Arial", 8)
                Else
                    lrPGSRelation.Link.Font = New Font("Arial", 10)
                End If

                If lrPGSRelation.Relation.IsPGSRelationNode Then
                    '=================================================================
                    'Origin/Destination Predicates
                    Dim lsOriginPredicate As String = ""
                    Dim lsDestinationPredicate As String = ""

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading

                    larRole.Add(lrPGSRelation.RDSRelation.ResponsibleFactType.RoleGroup(0)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(lrPGSRelation.RDSRelation.ResponsibleFactType.RoleGroup(1))

                    lrFactTypeReading = lrPGSRelation.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsDestinationPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsDestinationPredicate = "unknown predicate"
                    End If

                    larRole.Clear()
                    larRole.Add(lrPGSRelation.RDSRelation.ResponsibleFactType.RoleGroup(1)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(lrPGSRelation.RDSRelation.ResponsibleFactType.RoleGroup(0))

                    lrFactTypeReading = lrPGSRelation.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsOriginPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsOriginPredicate = "unknown predicate"
                    End If

                    If lrPGSRelation.Link.Origin.Bounds.X < lrPGSRelation.Link.Destination.Bounds.X Then
                        lrPGSRelation.Link.Text = lsOriginPredicate & " / " & lsDestinationPredicate
                    Else
                        lrPGSRelation.Link.Text = lsDestinationPredicate & " / " & lsOriginPredicate
                    End If
                Else
                    Dim lsOriginPredicate, lsDestinationPredicate As String
                    If lrPGSRelation.RDSRelation IsNot Nothing Then
                        lsOriginPredicate = lrPGSRelation.RDSRelation.OriginPredicate
                        lsDestinationPredicate = lrPGSRelation.RDSRelation.DestinationPredicate
                    Else
                        lsOriginPredicate = lrPGSRelation.Relation.OriginPredicate
                        lsDestinationPredicate = lrPGSRelation.Relation.DestinationPredicate
                    End If
                    If lrPGSRelation.Link.Origin.Bounds.X < lrPGSRelation.Link.Destination.Bounds.X Then
                        lrPGSRelation.Link.Text = lsOriginPredicate & " / " & lsDestinationPredicate
                    Else
                        lrPGSRelation.Link.Text = lsDestinationPredicate & " / " & lsOriginPredicate
                    End If
                End If

            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                '------------------------------------------------
                'Keep, so that ContextMenu is not changed from
                '  how set in Diagram.NodeSelected
                '-----------------------------------------------
                If loNode IsNot Nothing Then
                    If loNode.GetType = GetType(MindFusion.Diagramming.ShapeNode) Then
                        Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Node
                    Else
                        Me.DiagramView.ContextMenuStrip = Nothing
                    End If
                End If
            Else
                Call prApplication.setWorkingPage(Me.zrPage)

                If prApplication.ToolboxForms.FindAll(Function(x) x.Name = frmToolboxBrainBox.Name).Count > 0 Then
                    prApplication.Brain.Page = Me.zrPage
                End If

                '------------------------------------------------
                'User Left-Clicked on the Canvas
                '------------------------------------------------
                If Me.PropertyTableNode IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(Me.PropertyTableNode)
                    Me.PropertyTableNode = Nothing
                End If
                Me.PropertyTableNode = Nothing

                '---------------------------
                'Clear the SelectedObjects
                '---------------------------
                Me.zrPage.SelectedObject.Clear()

                'Me.Diagram.Selection.Clear()

                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

                'Me.Diagram.AllowUnconnectedLinks = False

                Call Me.resetNodeAndLinkColors()
            End If

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error:  " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub resetAttributeCellColours()

        Dim lrCell As MindFusion.Diagramming.TableNode.Cell
        Dim liInd As Integer = 0

        If Me.PropertyTableNode IsNot Nothing Then
            For liInd = 0 To Me.PropertyTableNode.RowCount - 1
                lrCell = Me.PropertyTableNode.Item(0, liInd)
                lrCell.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                lrCell.TextColor = Color.Black
            Next
        End If

    End Sub

    Sub resetNodeAndLinkColors()

        Try
            Dim liInd As Integer = 0

            DiagramView.SmoothingMode = SmoothingMode.HighQuality

            '------------------------------------------------------------------------------------
            'Reset the border colors of the ShapeNodes (to what they were before being selected
            '------------------------------------------------------------------------------------
            For liInd = 1 To Diagram.Nodes.Count
                Select Case Diagram.Nodes(liInd - 1).GetType
                    Case Is = GetType(MindFusion.Diagramming.ShapeNode)
                        Dim lrPGSNode As PGS.Node = Diagram.Nodes(liInd - 1).Tag
                        If CType(lrPGSNode.RDSTable.FBMModelElement, Object).ModelError.Count > 0 Then
                            lrPGSNode.Shape.Pen.Color = Color.Red
                        ElseIf lrPGSNode.Shape.Selected Then
                            lrPGSNode.Shape.Pen.Color = Color.Blue
                        Else
                            lrPGSNode.Shape.Pen.Color = Color.DeepSkyBlue
                        End If
                    Case Else
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                End Select
            Next

            Dim lrLink As DiagramLink
            For Each lrLink In Diagram.Links
                If lrLink.Tag IsNot Nothing Then
                    lrLink.Pen.Color = Color.DeepSkyBlue
                End If
            Next
            'For liInd = 1 To Diagram.Links.Count
            '    Diagram.Links(liInd - 1).Pen.Color = Color.DeepSkyBlue
            'Next

            '===========================================
#Region "Create DashPattern for Link Fact Types"
            'Create the dash pattern
            Dim loPen As New MindFusion.Drawing.Pen(Color.SkyBlue, 0.2)
            ReDim loPen.DashPattern(3)
            loPen.DashPattern(0) = 10
            loPen.DashPattern(1) = 5
            loPen.DashPattern(2) = 10
            loPen.DashPattern(3) = 5
#End Region


            For liInd = 1 To Diagram.Links.Count

                lrLink = Diagram.Links(liInd - 1)

                'Reset the text to ""
                'lrLink.Text = ""            
                lrLink.TextStyle = LinkTextStyle.Rotate
                Dim lrPGSLink As PGS.Link = lrLink.Tag

#Region "Experimental - Link Fact Types as dashed lines"

                Dim lrFactType As FBM.FactType

                If lrPGSLink.Relation.IsPGSRelationNode Or lrPGSLink.RDSRelation.ResponsibleFactType.isRDSTable Then

                    If lrPGSLink.RDSRelation.ResponsibleFactType.IsObjectified Or lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        If lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                            lrFactType = lrPGSLink.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                            If lrFactType.getCorrespondingRDSTable(Nothing, True).isPGSRelation Then
                                'SimplePredicate                                
                                lrPGSLink.Link.Pen.DashStyle = DashStyle.Solid
                            Else
                                lrPGSLink.Link.Pen = loPen
                            End If
                        Else
                            lrPGSLink.Link.Pen.DashStyle = DashStyle.Solid
                        End If
                    Else
                        lrPGSLink.Link.Pen.DashStyle = DashStyle.Solid
                    End If
                ElseIf lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                    lrPGSLink.Link.Pen = loPen
                End If
#End Region

                If lrPGSLink IsNot Nothing Then
                    Call lrPGSLink.setPredicate()

                    Call lrPGSLink.setHeadShapes()
                    '--------------------------------
                    'Disambiguate overlapping links
                    '--------------------------------
                    Dim commonLinks As DiagramLinkCollection = GetCommonLinks(lrLink.Origin, lrLink.Destination)

                    Dim pt1 As PointF = lrLink.ControlPoints(0)
                    Dim pt2 As PointF = lrLink.ControlPoints(lrLink.ControlPoints.Count - 1)

                    If commonLinks.Count > 1 Then
                        For c As Integer = 0 To commonLinks.Count - 1

                            Dim link As DiagramLink = commonLinks(c)

                            If link.Origin Is link.Destination Then

                            Else
                                '===================================                        
                                'If Not link.Tag.HasBeenMoved Then
                                link.Style = LinkStyle.Bezier
                                link.SegmentCount = 1 'Keep as 1, because bows () links. 2 makes funny bezier links.

                                Dim cp1 As New PointF(pt1.X + 1 * (pt2.X - pt1.X) / 3, pt1.Y + 1 * (pt2.Y - pt1.Y) / 3)
                                Dim cp2 As New PointF(pt1.X + 2 * (pt2.X - pt1.X) / 3, pt1.Y + 2 * (pt2.Y - pt1.Y) / 3)

                                Dim angle As Single = 0, radius As Single = 0
                                CarteseanToPolar(pt1, pt2, angle, radius)

                                Dim pairOffset As Integer = (c / 2 + 1) * 5
                                'If commonLinks.Count Mod 2 = 0 Then
                                PolarToCartesean(cp1, If(c Mod 2 = 0, angle - 90, angle + 90), pairOffset, cp1)
                                PolarToCartesean(cp2, If(c Mod 2 = 0, angle - 90, angle + 90), pairOffset, cp2)

                                If link.ControlPoints(0) = pt1 Then
                                    link.ControlPoints(1) = cp1
                                    link.ControlPoints(2) = cp2
                                Else
                                    link.ControlPoints(1) = cp2
                                    link.ControlPoints(2) = cp1
                                End If

                                'link.Tag.HasBeenMoved = True

                                link.UpdateFromPoints()

                                '===================================
                            End If
                        Next
                    Else
                        'lrLink.AutoRoute = True
                        lrLink.SegmentCount = 2
                        lrLink.SegmentCount = 1
                    End If

                End If
            Next
            '===========================================

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ERDDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Select Case e.Delta
            Case Is = 0
                'Do Nothing
            Case Is < 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex > 0 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex -= 1
                End If
            Case Is > 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count Then
                    If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count - 1 Then
                        frmMain.ToolStripComboBox_zoom.SelectedIndex += 1
                    End If
                End If
        End Select

    End Sub

    Private Sub EditAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditAttributeToolStripMenuItem.Click

        Dim lrEditAttributeForm As New frmCRUDEditAttribute
        Dim lrEntityType As FBM.EntityType
        Dim lrValueType As FBM.ValueType
        Dim lrTableNode As MindFusion.Diagramming.TableNode = Me.Diagram.Selection.Items(0)
        Dim lr_entity As New ERD.Entity

        '---------------------------------------------------------
        'Get the EntityType represented by the (selected) Entity
        '---------------------------------------------------------
        lr_entity = lrTableNode.Tag

        lrEntityType = New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel, lr_entity.Data, Nothing, True)
        lrEntityType = Me.zrPage.Model.EntityType.Find(AddressOf lrEntityType.Equals)

        lrEditAttributeForm.zrModel = Me.zrPage.Model
        lrEditAttributeForm.zrModelObject = lrEntityType
        lrEditAttributeForm.StartPosition = FormStartPosition.CenterParent

        If lrEditAttributeForm.ShowDialog = Windows.Forms.DialogResult.OK Then

            '-------------------------------------------------------------
            'Get the ValueType being added as an Attribute of the Entity
            '-------------------------------------------------------------
            lrValueType = lrEditAttributeForm.Tag

            lrTableNode.AddRow()
            lrTableNode.Item(0, lrTableNode.RowCount - 1).Text = lrValueType.Name
            lrTableNode.ResizeToFitText(False)

        End If

        lrEditAttributeForm.Dispose()

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Call Me.CopyImageToClipboard()

    End Sub

    Sub CopyImageToClipboard()

        Dim li_rectf As New RectangleF
        li_rectf = Me.Diagram.GetContentBounds(False, True)

        'Dim lo_image_processor As New t_image_processor(Diagram.CreateImage(li_rectf, 100))

        Dim lr_image As Image = Diagram.CreateImage(li_rectf, 100)

        Me.Diagram.ShowGrid = False

        Me.Cursor = Cursors.WaitCursor

        Windows.Forms.Clipboard.SetImage(lr_image)

        '---------------------------------
        'Set the grid back to what it was
        '---------------------------------
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call Me.SetToolbox()

    End Sub


    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.Model
            End If
        End If

    End Sub

    Private Sub mnuOption_ViewGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked

        If mnuOption_ViewGrid.Checked Then
            Me.Diagram.ShowGrid = True
        Else
            Me.Diagram.ShowGrid = False
        End If


    End Sub

    Private Sub ModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary()

    End Sub

    Private Sub PageAsORMMetamodelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetamodelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem1.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.Model
            End If
        End If

    End Sub

    Private Sub AutoLayoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoLayoutToolStripMenuItem.Click

        Call Me.autoLayout()

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Dim lrPGSLink As PGS.Link

        Try

            lrPGSLink = e.Link.Tag

            'CodeSafe
            If lrPGSLink Is Nothing Then
                Exit Sub
            End If

            If Me.GetCommonLinks(e.Link.Origin, e.Link.Destination).Count = 1 Then
                'Straighten the Link
                e.Link.SegmentCount = 2
                e.Link.SegmentCount = 1
            End If

            Call lrPGSLink.LinkSelected()

            Me.zrPage.SelectedObject.Clear()
            lrPGSLink.Relation.Link = lrPGSLink 'Leave here. Can't find bug where it's not set properly.
            Me.zrPage.SelectedObject.Add(lrPGSLink.Relation)

            '===================================================================================
            'Draw the Properties of the Node
            If lrPGSLink.Relation.IsPGSRelationNode Then

                If Me.PropertyTableNode IsNot Nothing Then
                    If Me.PropertyTableNode.Tag.Id <> lrPGSLink.Relation.ActualPGSNode.Id Then
                        Me.Diagram.Nodes.Remove(Me.PropertyTableNode)
                    End If
                End If

                If Me.PropertyTableNode Is Nothing And (Control.ModifierKeys = Keys.Control Or Control.ModifierKeys = Keys.ControlKey) Then

                    Dim liX As Integer = Math.Abs(lrPGSLink.Link.Bounds.X + (lrPGSLink.Link.Bounds.Width + 1) / 2)
                    Dim liY As Integer = Math.Abs(lrPGSLink.Link.Bounds.Y + (lrPGSLink.Link.Bounds.Height + 1) / 2)

                    Me.PropertyTableNode = Me.zrPage.Diagram.Factory.CreateTableNode(liX, liY, 30, 20, 1, 0)
                    Me.PropertyTableNode.EnableStyledText = True
                    Me.PropertyTableNode.Caption = "<B>" & " " & lrPGSLink.Relation.ActualPGSNode.Name & " "
                    Me.PropertyTableNode.Tag = lrPGSLink.Relation.ActualPGSNode
                    Dim lrRDSTable As RDS.Table = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrPGSLink.Relation.ActualPGSNode.Id)

                    For Each lrColumn In lrRDSTable.Column
                        If lrColumn.isForeignKey And My.Settings.PGSShowForeignKeyProperties Then
                            Me.PropertyTableNode.RowCount += 1
                            Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Tag = lrColumn
                            Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Text = lrColumn.Name
                        ElseIf Not lrColumn.isForeignKey Then
                            Me.PropertyTableNode.RowCount += 1
                            Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Tag = lrColumn
                            Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Text = lrColumn.Name
                        End If

                    Next
                    Me.PropertyTableNode.ResizeToFitText(True)
                    Me.PropertyTableNode.ZTop()
                End If
            End If

            '---------------------------------------------------------------------------
            'If the ORM(FactType)ReadingEditor is loaded, then
            '  do the appropriate processing so that the data in the ReadingEditor grid
            '  matches the selected FactType (if a FactType is selected by the user)
            '---------------------------------------------------------------------------
            Dim lrFactType As FBM.FactType

            Dim lrORMReadingEditor As frmToolboxORMReadingEditor
            lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

            If IsSomething(lrORMReadingEditor) Then

                Dim lrFactTypeInstance As FBM.FactTypeInstance = Nothing

                If IsSomething(lrPGSLink.Relation.RelationFactType) Then
                    If lrPGSLink.Relation.IsPGSRelationNode Then
                        If lrPGSLink.Relation.RelationFactType.IsLinkFactType Then
                            lrFactType = lrPGSLink.Relation.RelationFactType.RoleGroup(0).JoinedORMObject
                            lrFactTypeInstance = lrFactType.CloneInstance(New FBM.Page(Me.zrPage.Model), False)
                        Else
                            lrFactTypeInstance = lrPGSLink.Relation.RelationFactType.CloneInstance(New FBM.Page(Me.zrPage.Model), False)
                        End If
                    Else
                        lrFactTypeInstance = lrPGSLink.Relation.RelationFactType.CloneInstance(New FBM.Page(Me.zrPage.Model), False)
                    End If

                    lrORMReadingEditor.zrPage = Me.zrPage
                    lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance

                    Call lrORMReadingEditor.SetupForm()
                End If

            End If

#Region "Verbalisation"

            Dim lfrmORMVerbalisationView As frmToolboxORMVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)

            If lfrmORMVerbalisationView IsNot Nothing Then
                Try
                    If lrPGSLink.Relation.RDSRelation.ResponsibleFactType.IsObjectified Or lrPGSLink.Relation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        If lrPGSLink.Relation.ActualPGSNode Is Nothing And lrPGSLink.Relation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                            lrFactType = lrPGSLink.Relation.RDSRelation.ResponsibleFactType
                        ElseIf lrPGSLink.Relation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                            lrFactType = lrPGSLink.Relation.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                        Else
                            lrFactType = lrPGSLink.Relation.RDSRelation.ResponsibleFactType
                        End If
                    Else
                        lrFactType = lrPGSLink.Relation.RDSRelation.ResponsibleFactType
                    End If

                Catch ex As Exception
                    'Not a biggie
                End Try

                Call lfrmORMVerbalisationView.VerbaliseFactType(lrFactType)
            End If
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemIsMandatory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemIsMandatory.Click

        Dim lrAttribute As ERD.Attribute
        Dim lbInitialMadatoryState As Boolean
        Dim lsSQLQuery As String = ""

        lrAttribute = Me.zrPage.SelectedObject(0)

        lbInitialMadatoryState = lrAttribute.Mandatory

        Me.ToolStripMenuItemIsMandatory.Checked = Not Me.ToolStripMenuItemIsMandatory.Checked

        lrAttribute.Mandatory = Me.ToolStripMenuItemIsMandatory.Checked

        If (lbInitialMadatoryState = False) And lrAttribute.Mandatory Then
            lsSQLQuery = "INSERT INTO IsMandatory"
            lsSQLQuery &= " (IsMandatory)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lrAttribute.FactDataInstance.Fact.Id & "'"
            lsSQLQuery &= " )"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        Else
            lsSQLQuery = "DELETE FROM IsMandatory"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE IsMandatory = '" & lrAttribute.FactDataInstance.Fact.Id & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        End If

        Call lrAttribute.RefreshShape()

    End Sub

    Private Sub ToolStripMenuItemIsPartOfPrimaryKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemIsPartOfPrimaryKey.Click

        Dim lrAttribute As ERD.Attribute
        Dim lsSQLQuery As String = ""
        Dim lrRecordset As ORMQL.Recordset

        lrAttribute = Me.zrPage.SelectedObject(0)

        Me.ToolStripMenuItemIsPartOfPrimaryKey.Checked = Not Me.ToolStripMenuItemIsPartOfPrimaryKey.Checked

        lrAttribute.PartOfPrimaryKey = Me.ToolStripMenuItemIsPartOfPrimaryKey.Checked

        Call lrAttribute.RefreshShape()

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
        lsSQLQuery &= " WHERE Entity = '" & lrAttribute.Entity.Id & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        Dim lrFactInstance As FBM.FactInstance

        Dim lsIndexName As String = ""
        lsIndexName = Viev.Strings.RemoveWhiteSpace(lrAttribute.Entity.Id & "PK")

        If lrAttribute.PartOfPrimaryKey Then

            If lrRecordset.Facts.Count = 0 Then
                '-------------------------------------------------
                'Must create a Primary Identifier for the Entity
                '-------------------------------------------------
                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                lsSQLQuery &= " (Entity, Index)"
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & lrAttribute.Entity.Id & "'"
                lsSQLQuery &= ",'" & lsIndexName & "'"
                lsSQLQuery &= ")"

                lrFactInstance = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

            '-------------------------------------
            'Add the Attribute to the PrimaryKey
            '-------------------------------------
            lsSQLQuery = "INSERT INTO "
            lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
            lsSQLQuery &= " (Index, Property)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lsIndexName & "'"
            lsSQLQuery &= ",'" & lrAttribute.Name & "'"
            lsSQLQuery &= ")"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        Else
            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
            lsSQLQuery &= " WHERE Index = '" & lsIndexName & "'"
            lsSQLQuery &= " AND Property = '" & lrAttribute.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
            'lsSQLQuery &= " WHERE Class = '" & lrAttribute.Entity.Id & "'"

            'Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        End If

    End Sub

    Private Sub MoveUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpToolStripMenuItem.Click

        Dim lrAttribute As ERD.Attribute
        Dim lsSQLQuery As String = ""

        lrAttribute = Me.zrPage.SelectedObject(0)

        If lrAttribute.OrdinalPosition = 1 Then
            '-----------------------------------------------------------------
            'Can't move up any further in the OrdinalPositions of Attributes
            '-----------------------------------------------------------------
            Exit Sub
        Else
            'lrAttribute.OrdinalPosition -= 1

            '--------------------------------------------------------------------
            'Change the Ordinal Positions of the Attributes above the Attribute
            '--------------------------------------------------------------------
            Dim lrChangingAttribute As ERD.Attribute

            Dim lrTableNode As MindFusion.Diagramming.TableNode = Me.Diagram.Selection.Items(0)
            Dim lrEntity As New ERD.Entity
            '---------------------------------------------------------
            'Get the EntityType represented by the (selected) Entity
            '---------------------------------------------------------
            lrEntity = lrTableNode.Tag

            lrChangingAttribute = lrEntity.Attribute(lrAttribute.OrdinalPosition - 1)
            'lrChangingAttribute.OrdinalPosition += 1

            lrAttribute.Cell = lrEntity.TableShape.Item(0, lrAttribute.OrdinalPosition - 1)
            lrAttribute.Cell.Tag = lrAttribute
            lrChangingAttribute.Cell = lrEntity.TableShape.Item(0, lrChangingAttribute.OrdinalPosition - 1)
            lrChangingAttribute.Cell.Tag = lrChangingAttribute

            Call lrAttribute.RefreshShape()
            Call lrChangingAttribute.RefreshShape()

            lrEntity.Attribute.Insert(lrAttribute.OrdinalPosition - 1, lrAttribute)
            lrEntity.Attribute.RemoveAt(lrAttribute.OrdinalPosition + 1)

            Dim lrFactInstance As FBM.FactInstance

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrAttribute.FactDataInstance.Fact.Id & "'"

            Dim lrRecordset As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrAttribute.OrdinalPosition.ToString

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.FactDataInstance.Fact.Id & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

            Call lrFactInstance.FactType.FactTable.ResortFactTable()

        End If



    End Sub

    Private Sub MoveDownToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownToolStripMenuItem.Click

        Dim lrAttribute As ERD.Attribute
        Dim lsSQLQuery As String = ""

        lrAttribute = Me.zrPage.SelectedObject(0)

        Dim lrTableNode As MindFusion.Diagramming.TableNode = Me.Diagram.Selection.Items(0)
        Dim lrEntity As New ERD.Entity
        '---------------------------------------------------------
        'Get the EntityType represented by the (selected) Entity
        '---------------------------------------------------------
        lrEntity = lrTableNode.Tag

        If lrAttribute.OrdinalPosition = lrEntity.Attribute.Count Then
            '-----------------------------------------------------------------
            'Can't move up any further in the OrdinalPositions of Attributes
            '-----------------------------------------------------------------
            Exit Sub
        Else
            'lrAttribute.OrdinalPosition += 1

            '--------------------------------------------------------------------
            'Change the Ordinal Positions of the Attributes above the Attribute
            '--------------------------------------------------------------------
            Dim lrChangingAttribute As ERD.Attribute

            lrChangingAttribute = lrEntity.Attribute(lrAttribute.OrdinalPosition - 1)
            'lrChangingAttribute.OrdinalPosition -= 1

            lrAttribute.Cell = lrEntity.TableShape.Item(0, lrAttribute.OrdinalPosition - 1)
            lrAttribute.Cell.Tag = lrAttribute
            lrChangingAttribute.Cell = lrEntity.TableShape.Item(0, lrChangingAttribute.OrdinalPosition - 1)
            lrChangingAttribute.Cell.Tag = lrChangingAttribute

            Call lrAttribute.RefreshShape()
            Call lrChangingAttribute.RefreshShape()

            lrEntity.Attribute.Insert(lrAttribute.OrdinalPosition, lrAttribute)
            lrEntity.Attribute.RemoveAt(lrAttribute.OrdinalPosition - 2)

            Dim lrFactInstance As FBM.FactInstance

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrAttribute.FactDataInstance.Fact.Id & "'"

            Dim lrRecordset As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrAttribute.OrdinalPosition.ToString

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.FactDataInstance.Fact.Id & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

            Call lrFactInstance.FactType.FactTable.ResortFactTable()

        End If

    End Sub

    Private Sub DeleteAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteAttributeToolStripMenuItem.Click

        Dim lrAttribute As ERD.Attribute
        Dim lrChangingAttribute As ERD.Attribute
        Dim lrFactInstance As FBM.FactInstance
        Dim liInd As Integer = 0
        Dim lrRecordset As ORMQL.Recordset
        Dim lsSQLQuery As String = ""

        lrAttribute = Me.zrPage.SelectedObject(0)

        lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
        lsSQLQuery &= " WHERE ModelObject = '" & lrAttribute.Entity.Id & "'"
        lsSQLQuery &= " AND Attribute = '" & lrAttribute.Name & "'"

        Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        lrAttribute.Entity.Attribute.Remove(lrAttribute)

        lrAttribute.Entity.TableShape.DeleteRow(lrAttribute.OrdinalPosition - 1)

        '----------------------------------------------------------------------------------
        'Reset the OrdinalPosition of the Attributes above the Attribute that was deleted
        '----------------------------------------------------------------------------------
        For liInd = lrAttribute.OrdinalPosition To lrAttribute.Entity.Attribute.Count
            lrChangingAttribute = lrAttribute.Entity.Attribute(liInd - 1)
            'lrChangingAttribute.OrdinalPosition -= 1

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.FactDataInstance.Fact.Id & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

            Call lrFactInstance.FactType.FactTable.ResortFactTable()
        Next

    End Sub

    Private Sub Diagram_NodeDeselected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDeselected

        Dim lrNode As PGS.Node

        lrNode = e.Node.Tag

        Call lrNode.NodeDeselected()

        If Me.PropertyTableNode IsNot Nothing Then
            If Me.PropertyTableNode.Tag.Id <> lrNode.Id Then
                Me.Diagram.Nodes.Remove(Me.PropertyTableNode)
                Me.PropertyTableNode = Nothing
            End If
        End If

    End Sub

    Private Sub CypherToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CypherToolboxToolStripMenuItem.Click

        Call frmMain.loadToolboxCypher(Me.DockPanel.ActivePane)

    End Sub

    Private Sub Diagram_LinkDeselected(sender As Object, e As LinkEventArgs) Handles Diagram.LinkDeselected

        Try
            If e.Link.Origin Is e.Link.Destination Then

            Else
                'e.Link.SegmentCount = 1
            End If

            e.Link.Pen.Color = Color.DeepSkyBlue

            Dim lrPGSLink As PGS.Link = e.Link.Tag

            If lrPGSLink IsNot Nothing Then
                Call lrPGSLink.setPredicate()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStrip_Relation_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Relation.Opening

        Dim lrPage As FBM.Page
        Dim lrPageList As New List(Of FBM.Page)
        Dim lrRelation As New ERD.Relation

        Try
            Me.ContextMenuStrip_Relation.ImageScalingSize = New Drawing.Size(16, 16)

            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            lrRelation = Me.zrPage.SelectedObject(0)

            Me.ToolStripSeparator8.Visible = My.Settings.SuperuserMode
            Me.ToolStripMenuItemRelationRemoveFromPage.Visible = lrRelation.IsPGSRelationNode
            Me.ToolStripMenuItemViewEdgeTableData.Enabled = lrRelation.IsPGSRelationNode

            '=============================================================================================
            'Morphing
            '---------------------------------------------------------------------------------------------
            'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
            '  shape, and to/into another diagram starts at the MorphVector.
            '---------------------------------------------------------------------------------------------
            Me.MorphVector.Clear()
            Dim liX = 10
            Dim liY = 10
            Try
                liX = lrRelation.Link.Link.Bounds.X
                liY = lrRelation.Link.Link.Bounds.Y
            Catch
                liX = 0
                liY = 0
            End Try

            Me.MorphVector.Add(New tMorphVector(liX, liY, 0, 0, 40))

                '====================================================
                '--------------------------------------------------------------
                'Clear the list of ORMDiagrams that may relate to the EntityType
                '--------------------------------------------------------------
                Me.ORMDiagramToolStripMenuItem1.DropDownItems.Clear()
                Me.ToolStripMenuItemERDDiagram1.DropDownItems.Clear()
                Me.PGSDiagramToolStripMenuItem1.DropDownItems.Clear()

                Dim loMenuOption As ToolStripItem
                Dim lrEnterpriseView As tEnterpriseEnterpriseView

                '--------------------------------------------------------------------------------------------------------
                'Get the ORM Diagrams for the selected Node.
                If lrRelation.IsPGSRelationNode Then
                    lrPageList = prApplication.CMML.getORMDiagramPagesForModelElementName(lrRelation.Model, lrRelation.ActualPGSNode.Id)
                Else
                    lrPageList = prApplication.CMML.getORMDiagramPagesForModelElementName(lrRelation.Model, lrRelation.RelationFactType.Id)
                End If

                For Each lrPage In lrPageList
                    '---------------------------------------------------------------------------------------------------------
                    'Try and find the Page within the EnterpriseView.TreeView
                    '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                    '  is not added for those hidden Pages.
                    '----------------------------------------------------------
                    lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                    If IsSomething(lrEnterpriseView) Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        loMenuOption = Me.ORMDiagramToolStripMenuItem1.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ORM16x16)
                        loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                        AddHandler loMenuOption.Click, AddressOf Me.morphToORMDiagram
                    End If
                Next

                '--------------------------------------------------------------------------------------------------------
                'Get the ER Diagrams for the selected Node.
                If lrRelation.IsPGSRelationNode Then
                    lrPageList = prApplication.CMML.getERDiagramPagesForModelElementName(lrRelation.Model, lrRelation.ActualPGSNode.Id)
                Else
                    lrPageList = prApplication.CMML.getERDiagramPagesForModelElementName(lrRelation.Model, lrRelation.RelationFactType.Id)
                End If

                For Each lrPage In lrPageList
                    '---------------------------------------------------------------------------------------------------------
                    'Try and find the Page within the EnterpriseView.TreeView
                    '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                    '  is not added for those hidden Pages.
                    '----------------------------------------------------------
                    lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                    If IsSomething(lrEnterpriseView) Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        loMenuOption = Me.ToolStripMenuItemERDDiagram1.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ERD16x16)
                        loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                        AddHandler loMenuOption.Click, AddressOf Me.morphToERDiagram
                    End If
                Next

                '--------------------------------------------------------------------------------------------------------
                'Get the PGS Diagrams for the selected Node.
                If lrRelation.IsPGSRelationNode Then
                    lrPageList = prApplication.CMML.getPGSDiagramPagesForModelElementName(lrRelation.Model, lrRelation.ActualPGSNode.Id)
                Else
                    lrPageList = prApplication.CMML.getPGSDiagramPagesForModelElementName(lrRelation.Model, lrRelation.RelationFactType.Id)
                End If

                For Each lrPage In lrPageList
                    '---------------------------------------------------------------------------------------------------------
                    'Try and find the Page within the EnterpriseView.TreeView
                    '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                    '  is not added for those hidden Pages.
                    '----------------------------------------------------------
                    lrEnterpriseView = prPageNodes.Find(Function(x) x.PageId = lrPage.PageId)

                    If IsSomething(lrEnterpriseView) Then
                        '---------------------------------------------------
                        'Add the Page(Name) to the MenuOption.DropDownItems
                        '---------------------------------------------------
                        loMenuOption = Me.PGSDiagramToolStripMenuItem1.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.PGS16x16)
                        loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                        AddHandler loMenuOption.Click, AddressOf Me.morphToPGSDiagram
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            Me.Cursor = Cursors.Default

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_LinkModified(sender As Object, e As LinkEventArgs) Handles Diagram.LinkModified

        'e.Link.SegmentCount = 1

    End Sub

    Private Sub DiagramView_MouseUp(sender As Object, e As MouseEventArgs) Handles DiagramView.MouseUp

        Try

            Dim lo_point As System.Drawing.PointF
            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            If IsSomething(Diagram.GetLinkAt(lo_point, 2)) Then
                Dim lrPGSRelation As PGS.Link = Diagram.GetLinkAt(lo_point, 2).Tag

                'CodeSafe
                If lrPGSRelation Is Nothing Then Exit Sub

                lrPGSRelation.Link.Text = ""
                Call lrPGSRelation.setPredicate()
                'lrPGSRelation.Link.Text = ""
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

        Me.Close()

    End Sub

    Private Sub CloseAllButThisPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseAllButThisPageToolStripMenuItem.Click

        For Each lrForm In prApplication.ActivePages.ToArray

            Select Case lrForm.GetType
                Case Is = GetType(frmDiagramORM), GetType(frmDiagramERD), GetType(frmDiagramPGS)
                    If lrForm IsNot Me Then
                        Call lrForm.Close()
                    End If
            End Select
        Next

    End Sub

    Public Sub writeImageToFile(ByVal asFileLocation As String)

        Dim li_rectf As New RectangleF
        li_rectf = Me.Diagram.GetContentBounds(False, True)

        'Dim lo_image_processor As New t_image_processor(Diagram.CreateImage(li_rectf, 100))

        Dim lr_image As Image = Diagram.CreateImage(li_rectf, 100)

        lr_image = Richmond.CropImage(lr_image, Color.White, 0)
        lr_image = Richmond.CreateFramedImage(lr_image, Color.White, 15)

        lr_image.Save(asFileLocation, System.Drawing.Imaging.ImageFormat.Jpeg)

    End Sub

    Private Sub ToolStripMenuItem_RemoveFromPage_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RemoveFromPage.Click

        Dim lrShapeNode As MindFusion.Diagramming.ShapeNode
        Dim lsSQLQuery As String
        Dim lrNode As PGS.Node

        Try
            Try
                lrShapeNode = Me.Diagram.Selection.Items(0)
            Catch ex As Exception
                Exit Sub
            End Try

            With New WaitCursor

                ''---------------------------------------------------------
                ''Get the EntityType represented by the (selected) Entity
                ''---------------------------------------------------------
                lrNode = lrShapeNode.Tag '(above) = Me.Diagram.Selection.Items(0)

                '------------
                'Relations
                Call Me.zrPage.removeRelationsForEntity(lrNode)

                '------------
                'Attributes
                For Each lrERDAttribute In lrNode.Attribute
                    Call Me.zrPage.removeCMMLAttribute(lrERDAttribute)
                Next

                '-------------------------------------------------------------------------
                'Remove the Entity from the Page
                '---------------------------------
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & lrNode.Name & "'"

                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim larLinkToRemove As New List(Of DiagramLink)
                For Each lrLink In lrShapeNode.IncomingLinks
                    larLinkToRemove.Add(lrLink)
                Next

                For Each lrLink In lrShapeNode.OutgoingLinks
                    larLinkToRemove.Add(lrLink)
                Next

                Dim lrPGSLink As PGS.Link
                For Each lrLink In larLinkToRemove
                    Me.Diagram.Links.Remove(lrLink)
                    lrPGSLink = lrLink.Tag
                    Call Me.zrPage.ERDiagram.Relation.RemoveAll(AddressOf lrPGSLink.Relation.Equals)
                    If lrPGSLink.Relation.ActualPGSNode IsNot Nothing Then
                        Call lrPGSLink.Relation.ActualPGSNode.RemoveFromPage()
                    End If

                Next

                '----------------------------------------------------------------------------------------------------------
                'Remove the TableNode that represents the Entity from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Diagram.Nodes.Remove(lrShapeNode)
                Me.zrPage.ERDiagram.Entity.Remove(lrNode)

                Me.zrPage.SelectedObject.Remove(lrNode)
                Me.Diagram.Selection.RemoveItem(lrShapeNode)
            End With

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()



            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemEdgeReadingEditor_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEdgeReadingEditor.Click
        prApplication.WorkingPage = Me.zrPage
        Call frmMain.loadToolboxORMReadingEditor(Me.zrPage, Me.DockPanel.ActivePane)
    End Sub

    Private Sub DiagramView_MouseWheel(sender As Object, e As MouseEventArgs) Handles DiagramView.MouseWheel

        Select Case e.Delta
            Case Is = 0
                'Do Nothing
            Case Is < 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex > 0 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex -= 1
                End If
            Case Is > 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count Then
                    If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count - 1 Then
                        frmMain.ToolStripComboBox_zoom.SelectedIndex += 1
                    End If
                End If
        End Select

    End Sub

    Private Sub ShowPropertiesForNode(ByRef arNode As PGS.Node, Optional ByRef arERDRelation As ERD.Relation = Nothing)

        Try
            Dim lrNode As PGS.Node = arNode

            If arERDRelation IsNot Nothing Then
                Dim lrPGSLink As PGS.Link = arERDRelation.Link
                Me.PropertyTableNode = Me.zrPage.Diagram.Factory.CreateTableNode(lrPGSLink.Link.Bounds.X, lrPGSLink.Link.Bounds.Y + 25, 30, 20, 1, 0)
            Else
                Me.PropertyTableNode = Me.zrPage.Diagram.Factory.CreateTableNode(lrNode.Shape.Bounds.X, lrNode.Shape.Bounds.Y + 25, 30, 20, 1, 0)
            End If

            Me.PropertyTableNode.EnableStyledText = True
            Me.PropertyTableNode.Caption = "<B>" & " " & arNode.Name & " "
            Me.PropertyTableNode.Tag = arNode

            Dim lrRDSTable As RDS.Table = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrNode.Id)

            Dim larColumn = lrRDSTable.Column.ToList.OrderBy(Function(x) x.OrdinalPosition).ToList

            '--------------------------------------------------------------------
            'Refined sort of Columns based on Supertype Column ordering and Subtype ordering
            Dim liInd As Integer
            Dim larSupertypeTable = lrRDSTable.getSupertypeTables
            If larSupertypeTable.Count > 0 Then
                larSupertypeTable.Reverse()
                larSupertypeTable.Add(lrRDSTable)
                liInd = 0
                For Each lrSupertypeTable In larSupertypeTable
                    For Each lrColumn In larColumn.FindAll(Function(x) x.Role.JoinedORMObject.Id = lrSupertypeTable.Name).OrderBy(Function(x) x.OrdinalPosition)
                        larColumn.Remove(lrColumn)
                        larColumn.Insert(liInd, lrColumn)
                        liInd += 1
                    Next
                Next
            End If

            For Each lrColumn In larColumn
                Me.PropertyTableNode.RowCount += 1

                Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Tag = lrColumn
                Me.PropertyTableNode.Item(0, Me.PropertyTableNode.RowCount - 1).Text = lrColumn.Name
            Next

            Me.PropertyTableNode.ResizeToFitText(True)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ViewPropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewPropertiesToolStripMenuItem.Click


        'CodeSafe
        If Me.Diagram Is Nothing Then Exit Sub
        If Me.Diagram.Selection.Items(0).GetType <> GetType(MindFusion.Diagramming.ShapeNode) Then Exit Sub

        Try
            Dim lrShapeNode As MindFusion.Diagramming.ShapeNode = Me.Diagram.Selection.Items(0)
            Dim lrNode As New PGS.Node

            Try
                ''---------------------------------------------------------
                ''Get the EntityType represented by the (selected) Entity
                ''---------------------------------------------------------
                lrNode = lrShapeNode.Tag
                Call Me.ShowPropertiesForNode(lrNode)
            Catch
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemRelationRemoveFromPage.Click

        Dim lsMessage As String

        Try
            Dim lrPGSRelation As ERD.Relation
            Dim lsSQLQuery As String

            Try
                lrPGSRelation = Me.zrPage.SelectedObject(0)
            Catch ex As Exception
                lsMessage = "Oops. Something went wrong. Click on the canvas then the Edge Type again."
                MsgBox(lsMessage)
                Exit Sub
            End Try

            If lrPGSRelation.IsPGSRelationNode Then

                Me.Diagram.Nodes.Remove(lrPGSRelation.ActualPGSNode.Shape)
                Me.Diagram.Links.Remove(lrPGSRelation.Link.Link)
                Me.zrPage.ERDiagram.Entity.Remove(lrPGSRelation.ActualPGSNode)
                '-------------------------------------------------------------------------
                'Remove the Entity from the Page
                '---------------------------------
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & lrPGSRelation.ActualPGSNode.Name & "'"

                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            End If

        Catch ex As Exception

            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_LinkCreating(sender As Object, e As LinkValidationEventArgs) Handles Diagram.LinkCreating

        '================================================================================
        'Couldn't create links.
        '
        '   DiagramView
        '     Behavior = DrawLinks
        '
        '   Shape
        '     AllowOutgoinglinks = True
        '     HandlesStyle = HandlesStyle.Invisible
        '     EnabledHandles = AdjustmentHandles.Move
        '
        'Solved -Below
        '    Diagram
        '       AllowUnanchoredLinks = True
        '       I also set 
        '       AllowUnconnectedLinks = True
        '================================================================================

        Try
            Select Case e.Link.Origin.GetType
                Case Is = GetType(MindFusion.Diagramming.DummyNode), GetType(MindFusion.Diagramming.TableNode)
                    e.Cancel = True
                    Exit Sub
            End Select


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemLinkViewReadingEditor_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemLinkViewReadingEditor.Click

        Try
            prApplication.WorkingPage = Me.zrPage
            Dim lrORMReadingEditor = frmMain.loadToolboxORMReadingEditor(Me.zrPage, Me.DockPanel.ActivePane)

            Dim lrFactType As FBM.FactType = Nothing

            If Me.zrPage.SelectedObject.Count = 1 Then

                Dim lrERDRelation As ERD.Relation = Me.zrPage.SelectedObject(0)

                lrFactType = lrERDRelation.RDSRelation.ResponsibleFactType

                Dim lrFactTypeInstance As FBM.FactTypeInstance = lrFactType.CloneInstance(prApplication.WorkingPage, False)
                lrORMReadingEditor.zrPage = prApplication.WorkingPage
                lrORMReadingEditor.zrFactTypeInstance = lrFactTypeInstance
                prApplication.WorkingPage.SelectedObject.Clear()
                prApplication.WorkingPage.SelectedObject.Add(lrFactTypeInstance)
                Call lrORMReadingEditor.SetupForm()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub IndexManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IndexManagerToolStripMenuItem.Click

        Try
            Dim lrfrmCRUDIndexManager As New frmCRUDIndexManager

            Dim lrTable As RDS.Table
            Try
                lrTable = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = Me.zrPage.SelectedObject(0).name)
            Catch ex As Exception
                Exit Sub
            End Try

            lrfrmCRUDIndexManager.mrTable = lrTable
            Call lrfrmCRUDIndexManager.ShowDialog()

            Call Me.zrPage.Model.checkIfCanCheckForErrors()
            Call Me.resetNodeAndLinkColors()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ConvertToFactTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertToFactTypeToolStripMenuItem.Click

        Try
            Dim lrPGSNodeType As PGS.Node
            Dim lrModelElement As FBM.ModelObject = Nothing
            Try
                    lrPGSNodeType = Me.zrPage.SelectedObject(0)
                Catch ex As Exception
                    Exit Sub
                End Try

                lrModelElement = lrPGSNodeType.FBMModelElement

            If lrModelElement.GetType = GetType(FBM.EntityType) Then

                If CType(lrModelElement, FBM.EntityType).HasSimpleReferenceScheme Then
                    MsgBox("You cannot convert Node Types that represent ORM Entity Types with a Simple Reference Scheme (Reference Mode) to a Fact Type.")
                    Exit Sub
                End If

                If MsgBox("Are you sure you want to convert the model element for this Node Type to a Fact Type?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
                    Call CType(lrModelElement, FBM.EntityType).ConvertToFactType()
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

    ''' <summary>
    ''' Called when [Add Property] menu option selected on Node Type or Edge Type.
    ''' </summary>
    Private Sub AddPropertyToEntity()

        Dim lsMessage As String
        Try
            Dim lrNodeType As PGS.Node
            Dim lrRelation As ERD.Relation

            Dim lrModelElement As FBM.ModelObject

            Dim lfrmAddAttributeForm = New frmCRUDAddAttributeNew

            'CodeSafe
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Select Case Me.zrPage.SelectedObject(0).GetType
                Case Is = GetType(PGS.Node)
                    lrNodeType = Me.zrPage.SelectedObject(0)
                    lfrmAddAttributeForm.zrEntity = lrNodeType
                    lrModelElement = lrNodeType.RDSTable.FBMModelElement
                Case Is = GetType(ERD.Relation)
                    lrRelation = Me.zrPage.SelectedObject(0)
                    Dim lrRelationFactType As FBM.FactType = Nothing
                    If lrRelation.RelationFactType.IsLinkFactType Then
                        lrModelElement = lrRelation.RelationFactType.LinkFactTypeRole.FactType
                    Else
                        lrModelElement = lrRelation.RelationFactType
                    End If
                    lrRelationFactType = lrModelElement
                    If Not lrRelationFactType.IsObjectified Then
                        Call lrRelation.RelationFactType.Objectify(,,, True)
                    End If
                    lfrmAddAttributeForm.zrEntity = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrRelationFactType.Id)

                Case Else
                    Exit Sub
            End Select


            Dim lrModel As FBM.Model = Me.zrPage.Model

            lfrmAddAttributeForm.zrModel = lrModel
            lfrmAddAttributeForm.zrModelObject = lrModelElement

            Dim lsValueTypeName As String = lrModel.CreateUniqueValueTypeName("NewValueType", 0)
            Dim lrValueType As FBM.ValueType = lrModel.CreateValueType(lsValueTypeName, True,,,, False)
            lfrmAddAttributeForm.zrValueType = lrValueType

            '----------------------------------------------------------------------------------------------------------------
            'Establish a dummy FactType for the new Attribute.
            '  NB If the User clicks [Cancel] then the FactType and ValueType must be removed from the Model.
            '-------------------------------------------------------------------
            Dim lrFactType As FBM.FactType = Nothing
            If lrModelElement.ConceptType = pcenumConceptType.EntityType Then
                Dim lrEntityType As FBM.EntityType = lrModelElement
                lrFactType = lrEntityType.AddBinaryRelationToValueType(lrValueType, pcenumBinaryRelationMultiplicityType.ManyToOne, True)

            ElseIf lrModelElement.ConceptType = pcenumConceptType.FactType Then
                Dim lrSelectedFactType As FBM.FactType = lrModelElement

                If Not lrSelectedFactType.IsObjectified Then
                    lrSelectedFactType.Objectify()
                End If
                lrFactType = lrSelectedFactType.AddBinaryRelationToValueType(lrValueType, pcenumBinaryRelationMultiplicityType.ManyToOne, True)
            Else
                Throw New NotImplementedException("Not implemented.")
            End If

            lfrmAddAttributeForm.zrFactType = lrFactType

            Dim lsUniqueAttributeName = lfrmAddAttributeForm.zrEntity.CreateUniqueAttributeName("NewAttribute", 0)

            If lfrmAddAttributeForm.ShowDialog(lsUniqueAttributeName) = DialogResult.OK Then
                With New WaitCursor

                    Dim lrTempValueType As FBM.ValueType = Me.zrPage.Model.GetModelObjectByName(lfrmAddAttributeForm.zsValueTypeName, True)
                    If lrTempValueType IsNot Nothing Then
                        'Setting the Property/Attribute to an existing Value Type.
                        Call lrFactType.RoleGroup(1).ReassignJoinedModelObject(lrTempValueType, True, Nothing, False)
                        lrValueType.RemoveFromModel(True, False, True,, True, False)

                        Dim lsSubMessage = ""
                        If lrTempValueType.DataTypeLength <> lrValueType.DataTypeLength Then
                            lsSubMessage = "Length"
                        End If

                        If lrTempValueType.DataTypePrecision <> lrValueType.DataTypePrecision Then
                            If lsSubMessage <> "" Then lsSubMessage.AppendString(" and ")
                            lsSubMessage.AppendString("Precision")
                        End If

                        If lsSubMessage <> "" Then
                            lsMessage = "You are reusing the ORM level Value Type, " & lrValueType.Id & ", for this Property/Attribute."
                            lsMessage.AppendDoubleLineBreak("Do you want to change it's " & lsSubMessage & " to the new value you have set?")
                            Select Case MsgBox(lsMessage, MsgBoxStyle.YesNoCancel)
                                Case Is = MsgBoxResult.Yes
                                    lrTempValueType.SetDataType(lfrmAddAttributeForm.zbDataType, 0, 0, True)
                                    lrTempValueType.SetDataTypeLength(lfrmAddAttributeForm.ziDataTypeLength, True)
                                    lrTempValueType.SetDataTypePrecision(lfrmAddAttributeForm.ziDataTypePrecision, True)
                                Case Is = MsgBoxResult.Cancel
                                    Dim lfrmFlashCard As New frmFlashCard
                                    lfrmFlashCard.ziIntervalMilliseconds = 1500
                                    lfrmFlashCard.zsText = "Aborting addition of new Property."
                                    lfrmFlashCard.Show(frmMain)

                                    GoTo Aborted
                            End Select
                        End If

                        lrValueType = lrTempValueType

                    Else
                        lrValueType.SetName(lfrmAddAttributeForm.zsValueTypeName, True)
                        lrValueType.SetDataType(lfrmAddAttributeForm.zbDataType, 0, 0, True)
                        lrValueType.SetDataTypeLength(lfrmAddAttributeForm.ziDataTypeLength, True)
                        lrValueType.SetDataTypePrecision(lfrmAddAttributeForm.ziDataTypePrecision, True)
                    End If

                    lrFactType.RoleGroup(0).SetMandatory(lfrmAddAttributeForm.zbAttributeIsMandatory, True)
                    Dim larRole As New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}
                    Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, New List(Of String) From {"has", ""})
                    lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                    Dim lsNewName As String = lrFactType.MakeNameFromFactTypeReadings()
                    Call lrFactType.setName(lsNewName, True)

                    Call Me.resetNodeAndLinkColors()
                End With
            Else
Aborted:
                Call lrFactType.RemoveFromModel(True, False)
                Call lrValueType.RemoveFromModel(True, False)
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub AddAttributeToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddAttributeToolStripMenuItem1.Click

        Try
            Call Me.AddPropertyToEntity()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub ToolStripMenuItem1_Click_1(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        Try
            Call Me.AddPropertyToEntity()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub MakeManytoManyRelationshipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MakeManytoManyRelationshipToolStripMenuItem.Click

        Try
            Dim lrPGSRelation As ERD.Relation

            With New WaitCursor
                lrPGSRelation = Me.zrPage.SelectedObject(0)
                Dim lrFactType As FBM.FactType = Nothing
                Dim lbGoForward As Boolean = True

#Region "Prechecks"
                If lrPGSRelation.RelationFactType.IsLinkFactType Then

                    Try
                        lrFactType = lrPGSRelation.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType

                        lbGoForward = lrFactType.getCorrespondingRDSTable.isPGSRelation
                        GoTo FinishedPretesting
                    Catch ex As Exception
                        lbGoForward = False
                        GoTo FinishedPretesting
                    End Try
                Else
                    lrFactType = lrPGSRelation.RelationFactType
                End If

                'CodeSafe
                If Not (lrFactType.IsManyTo1BinaryFactType Or lrFactType.Is1To1BinaryFactType) Then Exit Sub
#End Region

FinishedPretesting:
                If lbGoForward Then

                    Call lrFactType.MakeManyToManyRelationship

                End If

                Call Me.resetNodeAndLinkColors()
                Call Me.resetNodeAndLinkColors()

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DisplayAsNodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisplayAsNodeToolStripMenuItem.Click

        Dim lrPGSRelation As ERD.Relation
        Dim lsMessage As String

        Try
            lrPGSRelation = Me.zrPage.SelectedObject(0)

            If lrPGSRelation.ActualPGSNode Is Nothing Then
                lsMessage = "This particular Edge Type cannot be displayed as a Node Type."
                lsMessage.AppendDoubleLineBreak("Only Edge Types that represent Objectified Fact Types at the Object-Role Model level can be displayed as Node Types.")
                MsgBox(lsMessage)
                Exit Sub
            End If

            'Remove Link
            If lrPGSRelation.Link.Link IsNot Nothing Then
                Call Me.Diagram.Links.Remove(lrPGSRelation.Link.Link)
            End If

            lrPGSRelation.ActualPGSNode.NodeType = pcenumPGSEntityType.Node

            Call lrPGSRelation.ActualPGSNode.RDSTable.setIsPGSRelation(False)

            '20220522-Was...but was displaying the node twice.
            'Call Me.zrPage.DropExistingPGSNodeAtPoint(lrPGSRelation.ActualPGSNode, New PointF(20, 20))

            'Call Me.zrPage.loadRelationsForPGSNode(lrPGSRelation.ActualPGSNode)
            Call Me.zrPage.loadPropertyRelationsForPGSNode(lrPGSRelation.ActualPGSNode)


        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PropertyGraphSchemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertyGraphSchemaToolStripMenuItem.Click

        Dim lsMessage As String

        Try

            ''====================================================================================
            ''Check to see that there are no Entity Types on this Page with no Reference Scheme
            'Dim larEntityTypeInstance As New List(Of FBM.EntityTypeInstance)
            'If Me.zrPage.hasEntityTypeInstancessWithoutReferenceScheme(larEntityTypeInstance) Then
            '    lsMessage = "There are Entity Types on this Page that have no Reference Scheme."
            '    lsMessage &= vbCrLf & vbCrLf & "Create a Reference Scheme for the following Entity Types before convertinng this page to a Property Graph Schema:" & vbCrLf
            '    For Each lrEntityTypeInstance In larEntityTypeInstance
            '        lsMessage &= vbCrLf & " - " & lrEntityTypeInstance.Id
            '    Next
            '    MsgBox(lsMessage)
            '    Exit Sub
            'End If

            With New WaitCursor
                Call Me.createObjectRoleModelFromCurrentPage(sender, e)
            End With

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub createEntityRelationshipDiagramFromCurrentPage(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '============================
        Dim lrPage As FBM.Page

        Try

            frmMain.Cursor = Cursors.WaitCursor
            Me.zrPage.Model.AllowCheckForErrors = False

            Me.CircularProgressBar.Left = (Me.Width / 2) - (Me.CircularProgressBar.Size.Width / 2)
            Me.CircularProgressBar.BringToFront()
            Me.CircularProgressBar.Value = 1
            Me.CircularProgressBar.Invalidate()
            Me.Invalidate()
            Me.BackgroundWorker.ReportProgress(10)

            lrPage = Me.zrPage.CreateEntityRelationshipDiagramCMML(Me.BackgroundWorker)

            If lrPage Is Nothing Then
                Throw New Exception("Failed to create the Entity-Relationship Diagram.")
            End If

            lrPage.Loaded = True
            lrPage.Save(False, True)

            Me.CircularProgressBar.Value = 0
            Me.CircularProgressBar.Text = "0%"
            Me.CircularProgressBar.Invalidate()
            Me.CircularProgressBar.SendToBack()


            Me.zrPage.Model.AllowCheckForErrors = True
            frmMain.Cursor = Cursors.Default

            Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing
            If IsSomething(lrPage) Then
                lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

                MsgBox("Added the new Entity-Relationship Diagram Page, '" & lrPage.Name & "', to the Model.")

                Dim loToolStripItem As ToolStripItem = CType(sender, ToolStripItem)

                If loToolStripItem.Tag = True Then
                    Dim lrToolstripItem As New tDummyToolStripItem(lrEnterpriseView)
                    Call Me.morphToERDiagram(lrToolstripItem, lrEnterpriseView)
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


    Private Sub createObjectRoleModelFromCurrentPage(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '============================
        Dim lrPage As FBM.Page

        Try

            frmMain.Cursor = Cursors.WaitCursor
            Me.zrPage.Model.AllowCheckForErrors = False

            Me.CircularProgressBar.Left = (Me.Width / 2) - (Me.CircularProgressBar.Size.Width / 2)
            Me.CircularProgressBar.BringToFront()
            Me.CircularProgressBar.Value = 1
            Me.CircularProgressBar.Invalidate()
            Me.Invalidate()
            Me.BackgroundWorker.ReportProgress(0)

            lrPage = Me.zrPage.CreateObjectRoleModel(Me.BackgroundWorker)

            If lrPage Is Nothing Then
                Throw New Exception("Failed to create the Object-Role Model.")
            End If

            lrPage.Loaded = True
            lrPage.Save(False, True)

            Me.CircularProgressBar.Value = 0
            Me.CircularProgressBar.Text = "0%"
            Me.CircularProgressBar.Invalidate()
            Me.CircularProgressBar.SendToBack()


            Me.zrPage.Model.AllowCheckForErrors = True
            frmMain.Cursor = Cursors.Default

            Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing
            If IsSomething(lrPage) Then
                lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

                MsgBox("Added the new Object-Role Model Page, '" & lrPage.Name & "', to the Model.")

                Dim loToolStripItem As ToolStripItem = CType(sender, ToolStripItem)

                If loToolStripItem.Tag = True Then
                    Dim lrToolstripItem As New tDummyToolStripItem(lrEnterpriseView)
                    Call Me.morphToORMDiagram(lrToolstripItem, lrEnterpriseView)
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

    Private Sub Diagram_NodeTextEdited(sender As Object, e As EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        Try
            Dim lrNodeType As PGS.Node
            Dim lrModelObject As New FBM.ModelObject


            lrNodeType = e.Node.Tag
            lrModelObject = lrNodeType.RDSTable.FBMModelElement

            'Check Nodes/Tables first before looking at the ORM level.
            If Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = e.NewText) IsNot Nothing Then
                If e.NewText = e.OldText Then Exit Sub
                MsgBox("The name, '" & e.NewText & "', conflicts with a Node Type of the same name in the Model, '" & e.NewText & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                lrNodeType.Shape.Text = e.OldText
                Exit Sub
            End If

            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.EntityType

                    Dim lrEntityType As FBM.EntityType = lrModelObject

                    '------------------------------------------------
                    'Set the values in the underlying Model.EntityType
                    '------------------------------------------------
                    If e.NewText = lrEntityType.Name Then
                        '------------------------------------------------------------
                        'Nothing to do. Name of the EntityType has not been changed.
                        '------------------------------------------------------------
                    Else
                        Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityType.Model, e.NewText, pcenumConceptType.EntityType)
                        Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityType.Model, e.NewText, pcenumConceptType.ValueType)
                        Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(lrEntityType.Model, e.NewText, pcenumConceptType.FactType)
                        Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(lrEntityType.Model, e.NewText, pcenumConceptType.RoleConstraint)

                        If lrEntityType.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("An Entity Type with the name, '" & lrEntityTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & lrEntityType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrEntityType.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Value Type of the same name in the Model, '" & lrEntityType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrEntityType.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & lrEntityType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrEntityType.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & lrEntityType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        Else
                            If Not lrEntityType.SetName(e.NewText) Then
                                lrNodeType.Shape.Text = e.OldText
                            End If
                        End If
                    End If

                Case Is = pcenumConceptType.FactType
                    Dim lrFactType As FBM.FactType

                    lrFactType = lrModelObject

                    If e.NewText = lrFactType.Id Then
                        '------------------------------------------------------------
                        'Nothing to do. Name of the FactType has not been changed.
                        '------------------------------------------------------------
                    Else
                        Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(lrFactType.Model, e.NewText, pcenumConceptType.EntityType)
                        Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(lrFactType.Model, e.NewText, pcenumConceptType.FactType)
                        Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(lrFactType.Model, e.NewText, pcenumConceptType.ValueType)
                        Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(lrFactType.Model, e.NewText, pcenumConceptType.RoleConstraint)

                        If lrFactType.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("A Fact Type with the name, '" & lrFactTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & lrFactType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrFactType.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Entity Type of the same name in the Model, '" & lrFactType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrFactType.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & lrFactType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        ElseIf lrFactType.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsByConceptTypeOnly) Then
                            MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & lrFactType.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                            lrNodeType.Shape.Text = e.OldText
                        Else
                            If Not lrFactType.setName(e.NewText) Then
                                lrNodeType.Shape.Text = e.OldText
                            End If
                        End If
                    End If
            End Select

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem.Click

        Dim lrShapeNode As MindFusion.Diagramming.ShapeNode

        Try
            Try
                lrShapeNode = Me.Diagram.Selection.Items(0)
            Catch ex As Exception
                Exit Sub
            End Try

            If Me.Diagram IsNot Nothing Then
                Call Me.DiagramView.BeginEdit(lrShapeNode)
            End If

        Catch ex As Exception

        End Try


    End Sub

    Private Sub RemoveFromPageAndModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageAndModelToolStripMenuItem.Click

        Dim lrPGSRelation As ERD.Relation
        Dim lsMessage As String

        Try
            With New WaitCursor
                Try
                    lrPGSRelation = Me.zrPage.SelectedObject(0)
                Catch ex As Exception
                    Exit Sub
                End Try

                If lrPGSRelation.RDSRelation.UltimateFactType.IsObjectified Then
                    'Not implemented yet.
                    Dim larFactType As New List(Of FBM.FactType)
                    If lrPGSRelation.RDSRelation.UltimateFactType.hasAssociatedFactTypes(larFactType) Then
                        lsMessage = "First remove any Fact Types associated with the Objectified Fact Type for this Edge Type at the Object-Role Modeling level before trying to remove this Edge Type from the Model."
                        lsMessage.AppendDoubleLineBreak("Associated Fact Types include:")
                        lsMessage.AppendLine("")
                        For Each lrFactType In larFactType
                            lsMessage.AppendLine(vbTab & "-" & lrFactType.Id)
                        Next
                        lsMessage.AppendLine("")
                        lsMessage.AppendLine("Hint: You can convert this Page to an ORM Diagram and work from that diagram.")
                        MsgBox(lsMessage)
                    Else
                        lrPGSRelation.RDSRelation.UltimateFactType.RemoveFromModel(True, True,,,)
                    End If
                Else
                    lrPGSRelation.RDSRelation.UltimateFactType.RemoveFromModel(True, True,,,)
                End If


            End With

        Catch ex As Exception

            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DisplayAsEdgeTypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisplayAsEdgeTypeToolStripMenuItem.Click

        Dim lrPGSNodeType As PGS.Node
        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lrFactType As FBM.FactType

        Try
            lrPGSNodeType = Me.zrPage.SelectedObject(0)

            Dim lbCanConvert As Boolean = False
            Dim lrModelElement = lrPGSNodeType.RDSTable.FBMModelElement
            If lrModelElement.GetType = GetType(FBM.FactType) Then
                lrFactType = CType(lrModelElement, FBM.FactType)

                If lrFactType.IsObjectified Then

                    If lrFactType.RoleGroup.Count = 2 Then
                        lbCanConvert = True
                        GoTo MoveForward
                    End If

                    If lrFactType.RoleGroup.Contains(lrFactType.ReferenceSchemeRoleConstraint.Role(0)) Then
                        If lrFactType.ReferenceSchemeRoleConstraint.Role.Count = 2 Then
                            lbCanConvert = True
                            GoTo MoveForward
                        End If
                    End If

                    GoTo ErrorOut
                End If

            End If

MoveForward:

            If Not lbCanConvert Then
ErrorOut:
                lsMessage = "This particular Edge Type cannot be displayed as an Edge Type."
                lsMessage.AppendDoubleLineBreak("Only Edge Types that represent Objectified Fact Types and have a Preferred Identifier accross two Roles at the Object-Role Model level can be displayed as Edge Types.")
                MsgBox(lsMessage)
                Exit Sub
            End If

            lrPGSNodeType.NodeType = pcenumPGSEntityType.Relationship

            Call lrPGSNodeType.RDSTable.setIsPGSRelation(True)

#Region "Do transition"
            '------------
            'Relations
            'Call Me.zrPage.removeRelationsForEntity(lrPGSNodeType)

            Dim larLinkToRemove As New List(Of DiagramLink)
            For Each lrLink In lrPGSNodeType.Shape.IncomingLinks
                larLinkToRemove.Add(lrLink)
            Next

            For Each lrLink In lrPGSNodeType.Shape.OutgoingLinks
                larLinkToRemove.Add(lrLink)
            Next

            For Each lrLink In larLinkToRemove
                Me.Diagram.Links.Remove(lrLink)
            Next

            '----------------------------------------------------------------------------------------------------------
            'Remove the TableNode that represents the Entity from the Diagram on the Page.
            '-------------------------------------------------------------------------------
            Me.Diagram.Nodes.Remove(lrPGSNodeType.Shape)

            Me.zrPage.SelectedObject.Remove(lrPGSNodeType)
            Me.Diagram.Selection.RemoveItem(lrPGSNodeType.Shape)

            '==============================================================================================================================================
            'Call Me.zrPage.loadRelationsForPGSNode(lrPGSNodeType, False)

            lrFactType = lrPGSNodeType.RDSTable.FBMModelElement

            Dim larDestinationModelObjects = lrFactType.getDestinationModelObjects

            Dim lrRDSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.OriginTable.Name = lrPGSNodeType.Name And
                                                                                      larDestinationModelObjects.Select(Function(y) y.Id).ToList.Contains(x.DestinationTable.Name))

            Dim lbAllFound As Boolean = True
            For Each lrModelObject In larDestinationModelObjects
                If lrModelObject.GetType <> GetType(FBM.ValueType) Then
                    If Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrModelObject.Id) Is Nothing Then
                        lbAllFound = False
                    End If
                End If
            Next

            If lbAllFound Then
                If lrPGSNodeType.RDSTable.isPGSRelation Then
                    Call Me.zrPage.DisplayPGSRelationNodeLink(lrPGSNodeType, lrRDSRelation)
                    If lrPGSNodeType.Shape IsNot Nothing Then
                        lrPGSNodeType.Shape.Visible = False
                        For Each lrEdge As MindFusion.Diagramming.DiagramLink In lrPGSNodeType.Shape.OutgoingLinks
                            lrEdge.Visible = False
                        Next
                    End If
                End If
            End If

            '==============================================================================================================================================

#End Region

            'Call Me.zrPage.DropExistingPGSNodeAtPoint(lrPGSRelation.ActualPGSNode, New PointF(20, 20))

            'Call Me.zrPage.loadRelationsForPGSNode(lrPGSRelation.ActualPGSNode)
            'Call Me.zrPage.loadPropertyRelationsForPGSNode(lrPGSRelation.ActualPGSNode)

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

        Try
            Dim lrERDRelation As ERD.Relation
            Dim lrFactType As FBM.FactType = Nothing

            Try
                lrERDRelation = Me.zrPage.SelectedObject(0)
            Catch ex As Exception
                MsgBox("Oops. Something went wrong. Click on the canvas and select the Edge Type again.")
                Exit Sub
            End Try

            If lrERDRelation.RDSRelation.UltimateFactType.IsObjectified And lrERDRelation.IsPGSRelationNode Then
                Dim lrNodeType As PGS.Node = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrERDRelation.RDSRelation.UltimateFactType.Id)
                Call Me.ShowPropertiesForNode(lrNodeType, lrERDRelation)
            End If



        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub Diagram_LinkTextEdited(sender As Object, e As EditLinkTextEventArgs) Handles Diagram.LinkTextEdited

        Try
            Dim lrPGSLink As PGS.Link = e.Link.Tag

            'CodeSafe
            If e.NewText = "" Then e.Link.Text = e.OldText
            'CodeSafe
            If lrPGSLink.RDSRelation IsNot Nothing Then
                If lrPGSLink.RDSRelation.ResponsibleFactType Is Nothing Then
                    lrPGSLink.Link.Text = e.OldText
                    Exit Sub
                End If
            Else
                Exit Sub
            End If
            'CodeSafe
            If LCase(Trim(e.NewText)) = LCase(Trim(e.OldText)) Then Exit Sub

            Dim lrFactType As FBM.FactType = Nothing
            Dim lrFactTypeReading As FBM.FactTypeReading = Nothing
            Dim larRole As New List(Of FBM.Role)

            '--------------------------------------------------------------------------------------------------
            'Find the FactType for the Link
            Try
                If lrPGSLink.RDSRelation.ResponsibleFactType.IsObjectified Or lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                    If lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        lrFactType = lrPGSLink.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                        If Not lrFactType.getCorrespondingRDSTable().isPGSRelation Then
                            GoTo SimplePredicate
                        End If
                    Else
                        lrFactType = lrPGSLink.RDSRelation.ResponsibleFactType
                    End If
                Else
                    lrFactType = lrPGSLink.RDSRelation.ResponsibleFactType
                End If
            Catch ex As Exception
                e.Link.Text = e.OldText
                Exit Sub
            End Try

            '==================================================================================================
            If lrPGSLink.Relation.IsPGSRelationNode Or lrPGSLink.RDSRelation.ResponsibleFactType.isRDSTable Then

                Dim lrRDSTable As RDS.Table
                If Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrFactType.Id) Is Nothing Then
                    Try
                        lrRDSTable = lrFactTypeReading.FactType.getCorrespondingRDSTable(Nothing, True)
                    Catch ex As Exception
                        e.Link.Text = e.OldText
                        Exit Sub
                    End Try
                    If lrRDSTable Is Nothing Then
                        e.Link.Text = e.OldText
                        Exit Sub
                    End If
                Else
                    lrRDSTable = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrFactType.Id).getCorrespondingRDSTable
                End If

                'Check to see if the Link has Properties. Can't set the predicate from the Link if has Properties, must use the FactTypeReading editor.
                If lrRDSTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey).Count = 0 Then
                    '============================================================
                    'Can set the PrediatePart.PredicateText                    
                    'All Okay
                Else
                    e.Link.Text = e.OldText
                    Exit Sub
                End If
            Else
SimplePredicate:
            End If

            'CodeSafe
            If lrFactType Is Nothing Then
                e.Link.Text = e.OldText
                Exit Sub
            End If

            Dim lsPredicate As String = LCase(Trim(e.NewText))

            If lrFactType.FactTypeReading.Count = 0 Then
                'Create a new FactTypeReading
                larRole = New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}
                lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, New List(Of String) From {lsPredicate, ""})
                lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
            ElseIf lrFactType.FactTypeReading.Count = 1 Then
                'Great...set the Predicate for the FactTypeReading's first PredicatePart
                lrFactTypeReading = lrFactType.FactTypeReading(0)
                If lrFactTypeReading.RoleList(0).JoinedORMObject.Id = e.Link.Origin.Tag.RDSTable.Name Then
                    lrFactTypeReading = lrFactType.FactTypeReading(0)
                    lrFactTypeReading.PredicatePart(0).SetPredicateText(lsPredicate, True)
                Else
                    'Create a new FactTypeReading
                    larRole = New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}
                    lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, New List(Of String) From {lsPredicate, ""})
                    lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                End If
            Else
                lrFactTypeReading = lrFactType.FactTypeReading(0)
                If lrFactTypeReading.RoleList(0).JoinedORMObject.Id = e.Link.Origin.Tag.RDSTable.Name Then
                    lrFactTypeReading = lrFactType.FactTypeReading(0)
                    lrFactTypeReading.PredicatePart(0).SetPredicateText(lsPredicate, True)
                Else
                    'Create a new FactTypeReading
                    Dim larFactTypeReading = From FactTypeReading In lrFactType.FactTypeReading
                                             From PredicatePart In FactTypeReading.PredicatePart
                                             Where PredicatePart.Role.JoinedORMObject.Id = e.Link.Origin.Tag.RDSTable.Name
                                             Select FactTypeReading

                    If larFactTypeReading.Count = 0 Then
                        'Create a new FactTypeReading
                        larRole = New List(Of FBM.Role) From {lrFactType.RoleGroup(0), lrFactType.RoleGroup(1)}
                        lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, New List(Of String) From {lsPredicate, ""})
                        lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)
                    Else
                        lrFactTypeReading = larFactTypeReading.First
                        lrFactTypeReading.PredicatePart(0).SetPredicateText(lsPredicate, True)
                    End If
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

    Private Sub Diagram_LinkTextEditing(sender As Object, e As LinkValidationEventArgs) Handles Diagram.LinkTextEditing

        Try
            Dim lrPGSLink As PGS.Link = e.Link.Tag

            Call lrPGSLink.setPredicate()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub EntityRelationshipDiagramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EntityRelationshipDiagramToolStripMenuItem.Click

        Try
            With New WaitCursor
                Call Me.createEntityRelationshipDiagramFromCurrentPage(sender, e)
            End With


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_LinkDeleting(sender As Object, e As LinkValidationEventArgs) Handles Diagram.LinkDeleting

        Dim lrPGSLink As PGS.Link
        Dim lsMessage As String

        Try

            lrPGSLink = e.Link.Tag

            'CodeSafe
            If lrPGSLink Is Nothing Then
                Exit Sub
            End If

            lsMessage = "Are you sure you want to remove this Edge Type from the Model?"
            lsMessage.AppendDoubleLineBreak("All Edge Types are either a Fact Type or a Link Fact Type at the Object-Role Model level.")
            lsMessage.AppendDoubleLineBreak("You might not be able to remove the Fact Type from this view.")

            Dim lrFactType As FBM.FactType

            If MsgBox(lsMessage, MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then

                If lrPGSLink.Relation.ActualPGSNode IsNot Nothing Then
#Region "ActualPGSNode"
                    Try
                        If lrPGSLink.Relation.ActualPGSNode.RDSTable.FBMModelElement.CanSafelyRemoveFromModel Then
                            lrFactType = lrPGSLink.Relation.ActualPGSNode.RDSTable.FBMModelElement
                            Call lrFactType.RemoveFromModel(False, True, True, True, True, False)
                            GoTo EndProcessing
                        Else
                            lsMessage = "Sorry, you can't remove this Edge Type from the Property Graph Schema view. Morph to an Object-Role Model view and remove the Fact Type from there."
                            lsMessage.AppendDoubleLineBreak("The responsible Fact Type is called: " & lrPGSLink.Relation.ActualPGSNode.RDSTable.FBMModelElement.Id)
                            lsMessage.AppendDoubleLineBreak("Reasons for this message includes that if the Edge Type has Properties they must be removed from the Object-Role Model level.")
                            MsgBox(lsMessage)
                            Exit Sub
                        End If

                    Catch ex As Exception
                        Throw New Exception("Couldn't remove Actual PGS Node for Edge Type" & vbCrLf & ex.Message)
                    End Try
#End Region
                ElseIf Not lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType And lrPGSLink.RDSRelation.ResponsibleFactType.CanSafelyRemoveFromModel Then
#Region "Not LinkFactType and can safely remove from the Model"
                    lrFactType = lrPGSLink.RDSRelation.ResponsibleFactType
                    Call lrFactType.RemoveFromModel(False, True, True, True, True, False)
                    GoTo EndProcessing
#End Region
                ElseIf lrPGSLink.RDSRelation.ResponsibleFactType.IsLinkFactType And lrPGSLink.RDSRelation.ResponsibleFactType.CanSafelyRemoveFromModel Then
#Region "Not is LinkFactType"
                    lsMessage = "Sorry, you can't remove Edge Types that are Link Fact Types from the Property Graph Schema view. Morph to an Object-Role Model view and remove the Fact Type from there."
                    MsgBox(lsMessage)
                    Exit Sub
#End Region
                End If
            Else
                e.Cancel = True
                Exit Sub
            End If
EndProcessing:

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ViewVerbalisationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewVerbalisationToolStripMenuItem.Click

        Try
            prApplication.WorkingPage = Me.zrPage
            Dim lrfrmVerbalisationView = frmMain.loadToolboxORMVerbalisationForm(Me.zrPage.Model, Me.DockPanel.ActivePane)

            Dim lrFactType As FBM.FactType = Nothing

            If Me.zrPage.SelectedObject.Count = 1 Then

                Dim lrERDRelation As ERD.Relation = Me.zrPage.SelectedObject(0)

                If lrERDRelation.RDSRelation.ResponsibleFactType.IsObjectified Or lrERDRelation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                    If lrERDRelation.ActualPGSNode Is Nothing And lrERDRelation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        lrFactType = lrERDRelation.RDSRelation.ResponsibleFactType
                    ElseIf lrERDRelation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        lrFactType = lrERDRelation.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                    Else
                        lrFactType = lrERDRelation.RDSRelation.ResponsibleFactType
                    End If
                Else
                        lrFactType = lrERDRelation.RDSRelation.ResponsibleFactType
                End If

                Call lrfrmVerbalisationView.VerbaliseFactType(lrFactType)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ORMVerbalisationViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ORMVerbalisationViewToolStripMenuItem.Click

        Try
            prApplication.WorkingModel = Me.zrPage.Model
            prApplication.WorkingPage = Me.zrPage

            Call frmMain.loadToolboxORMVerbalisationForm(Me.zrPage.Model, Me.DockPanel.ActivePane)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ErrorListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ErrorListToolStripMenuItem.Click

        Try
            Call frmMain.loadToolboxErrorListForm(Me.DockPanel.ActivePane)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RichmondBrainBoxToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RichmondBrainBoxToolStripMenuItem.Click

        Try
            prApplication.WorkingModel = Me.zrPage.Model
            prApplication.WorkingPage = Me.zrPage

            frmMain.Cursor = Cursors.WaitCursor
            Call frmMain.loadToolboxRichmondBrainBox(Me.zrPage, Me.DockPanel.ActivePane)
            frmMain.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemViewNodeTableData_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewNodeTableData.Click

        Dim lrPGSNode As New PGS.Node

        Try
            '-------------------------
            'Get the selected PGS Node            
            lrPGSNode = Me.Diagram.Selection.Items(0).Tag

            prApplication.WorkingModel = Me.zrPage.Model
            prApplication.WorkingPage = Me.zrPage

            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
                Call prApplication.WorkingModel.connectToDatabase()
            End If

            Dim lfrmToolboxTableData = frmMain.loadToolboxTableDataForm(Me.zrPage.Model, Me.DockPanel.ActivePane)

            lfrmToolboxTableData.mrTable = lrPGSNode.RDSTable
            lfrmToolboxTableData.mrModel = prApplication.WorkingModel
            Call lfrmToolboxTableData.SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ToolStripMenuItemViewEdgeTableData_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewEdgeTableData.Click

        Dim lrPGSNode As New PGS.Node
        Dim lrPGSRelation As ERD.Relation = Nothing
        Dim lsMessage As String = ""
        Try
            Try
                lrPGSRelation = Me.zrPage.SelectedObject(0)
            Catch ex As Exception
                lsMessage = "Oops. Something went wrong. Click on the canvas then the Edge Type again."
                MsgBox(lsMessage)
                Exit Sub
            End Try

            '-------------------------
            'Get the selected PGS Node            
            lrPGSNode = lrPGSRelation.ActualPGSNode

            prApplication.WorkingModel = Me.zrPage.Model
            prApplication.WorkingPage = Me.zrPage

            If prApplication.WorkingModel.DatabaseConnection Is Nothing Then
                Call prApplication.WorkingModel.connectToDatabase()
            End If

            Dim lfrmToolboxTableData = frmMain.loadToolboxTableDataForm(Me.zrPage.Model, Me.DockPanel.ActivePane)

            lfrmToolboxTableData.mrTable = lrPGSNode.RDSTable
            lfrmToolboxTableData.mrModel = prApplication.WorkingModel
            Call lfrmToolboxTableData.SetupForm()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    'Private Sub AddAttributeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAttributeToolStripMenuItem.Click

    '        Dim lrAddAttributeForm As New frmCRUDAddAttribute
    '        Dim lrEntityType As FBM.EntityType
    '        Dim lrTableNode As ERD.TableNode = Me.Diagram.Selection.Items(0)
    '        Dim lrEntity As New ERD.Entity
    '        Dim lrFactInstanceAttribute As FBM.FactInstance
    '        Dim lrFactInstanceRelation As FBM.FactInstance = Nothing
    '        Dim lsSQLQuery As String = ""

    '        '---------------------------------------------------------
    '        'Get the EntityType represented by the (selected) Entity
    '        '---------------------------------------------------------
    '        lrEntity = lrTableNode.Tag '(above) = Me.Diagram.Selection.Items(0)

    '        lrEntityType = New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel, lrEntity.Name, Nothing, True)
    '        lrEntityType = Me.zrPage.Model.EntityType.Find(AddressOf lrEntityType.Equals)

    '        lrAddAttributeForm.zrModel = Me.zrPage.Model
    '        lrAddAttributeForm.zrModelObject = lrEntityType
    '        lrAddAttributeForm.zrEntity = lrEntity
    '        lrAddAttributeForm.zarEntity = Me.ERDiagram.Entity

    '        lrAddAttributeForm.StartPosition = FormStartPosition.CenterParent

    '        If lrAddAttributeForm.ShowDialog = Windows.Forms.DialogResult.OK Then

    '            Dim lrERAttribute As ERD.Attribute

    '            '----------------------------------------------------------------------------
    '            'Relationship processing.
    '            '  NB Do this first, because we need lrFactInstanceRelation for when 
    '            '  mapping the relationship between an Attribute and its associated Relation.
    '            '----------------------------------------------------------------------------
    '            If IsSomething(lrAddAttributeForm.zrReferencesModelObject) Then

    '                Dim lrLink As ERD.Link

    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
    '                lsSQLQuery &= " (Relation, FactType)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= ",'" & lrAddAttributeForm.zrFactType.Name & "'"
    '                lsSQLQuery &= " )"

    '                'Must Fix
    '                'lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreERDRelation.ToString
    '                lsSQLQuery &= " (ModelObject, Relation)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrEntity.Name & "'"
    '                lsSQLQuery &= ",'" & lrAddAttributeForm.zrReferencesModelObject.Name & "'"
    '                lsSQLQuery &= " )"

    '                lrFactInstanceRelation = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                If lrAddAttributeForm.zrRelation.OriginMandatory Then
    '                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
    '                    lsSQLQuery &= " (" & pcenumCMMLRelations.CoreOriginIsMandatory.ToString & ")"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                    lsSQLQuery &= " )"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If

    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
    '                lsSQLQuery &= " (ERDRelation, Multiplicity)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= ",'" & lrAddAttributeForm.zrRelation.OriginMultiplicity.ToString & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                If lrAddAttributeForm.zrRelation.DestinationMandatory Then
    '                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
    '                    lsSQLQuery &= " (" & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString & ")"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                    lsSQLQuery &= " )"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If

    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
    '                lsSQLQuery &= " (ERDRelation, Multiplicity)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                lsSQLQuery &= ",'" & lrAddAttributeForm.zrRelation.DestinationMultiplicity.ToString & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                If lrAddAttributeForm.zrRelation.OriginContributesToPrimaryKey Then
    '                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
    '                    lsSQLQuery &= " (" & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & ")"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrFactInstanceRelation.Id & "'"
    '                    lsSQLQuery &= " )"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                lrLink = New ERD.Link(Me.zrPage,
    '                                      lrFactInstanceRelation,
    '                                      lrEntity,
    '                                      lrAddAttributeForm.zrReferencesModelObject,
    '                                      Nothing,
    '                                      Nothing,
    '                                      lrAddAttributeForm.zrRelation)
    '                lrLink.Relation = lrAddAttributeForm.zrRelation

    '                lrLink.DisplayAndAssociate()

    '            End If

    '            '===================================================
    '            'Process the Attribute/s
    '            '===================================================
    '            For Each lrERAttribute In lrAddAttributeForm.zarAttribute

    '                '---------------------------------------------------
    '                'The String put in the TableNode for the Attribute
    '                '---------------------------------------------------
    '                Dim lsAttribute As String = ""

    '                lsSQLQuery = "INSERT INTO ERDAttribute"
    '                lsSQLQuery &= " (ModelObject, Attribute)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrEntity.Name & "'"
    '                lsSQLQuery &= ",'" & lrERAttribute.Name & "'"
    '                lsSQLQuery &= " )"

    '                lrFactInstanceAttribute = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                '--------------------------------------------------
    '                'Set the Ordinal Position of the Attribute
    '                '--------------------------------------------------
    '                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
    '                lsSQLQuery &= " (Property, Position)"
    '                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                lsSQLQuery &= " VALUES ("
    '                lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '                lsSQLQuery &= ",'" & (lrEntity.Attribute.Count + 1).ToString & "'"
    '                lsSQLQuery &= " )"

    '                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                Dim lrFactDataInstance As FBM.FactDataInstance
    '                lrFactDataInstance = lrFactInstanceAttribute.GetFactDataInstanceByRoleName("Attribute")

    '                lrERAttribute = lrFactDataInstance.CloneAttribute(Me.zrPage)

    '                'lrERAttribute.OrdinalPosition = lrEntity.Attribute.Count + 1
    '                lrERAttribute.Entity = New ERD.Entity
    '                lrERAttribute.Entity = lrEntity
    '                lrEntity.Attribute.Add(lrERAttribute)

    '                If lrAddAttributeForm.zbAttributeIsMandatory And lrAddAttributeForm.zbAttributeIsPartOfPrimaryKey Then
    '                    lsAttribute = "# * " & lrERAttribute.Name
    '                    lrERAttribute.Mandatory = True
    '                    lrERAttribute.PartOfPrimaryKey = True
    '                    lrEntity.PrimaryKey.Add(lrERAttribute)
    '                ElseIf lrAddAttributeForm.zbAttributeIsMandatory Then
    '                    lsAttribute = "* " & lrERAttribute.Name
    '                    lrERAttribute.Mandatory = True
    '                Else
    '                    lsAttribute = "o " & lrERAttribute.Name
    '                End If

    '                lrTableNode.AddRow()
    '                lrTableNode.Item(0, lrTableNode.RowCount - 1).Text = " " & lsAttribute

    '                lrTableNode.Item(0, lrTableNode.RowCount - 1).Tag = lrERAttribute
    '                lrERAttribute.Cell = lrTableNode.Item(0, lrTableNode.RowCount - 1)
    '                lrTableNode.ResizeToFitText(False)

    '                If lrERAttribute.Mandatory Then

    '                    lsSQLQuery = "INSERT INTO IsMandatory"
    '                    lsSQLQuery &= " (IsMandatory)"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '                    lsSQLQuery &= " )"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If

    '                '==========================================================
    '                'Part of PrimaryKey

    '                lsSQLQuery = "SELECT *"
    '                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
    '                lsSQLQuery &= " WHERE Entity = '" & lrEntity.Id & "'"

    '                Dim lrRecordset As ORMQL.Recordset
    '                lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    '                Dim lrFactInstance As FBM.FactInstance

    '                Dim lsIndexName As String = ""
    '                lsIndexName = Viev.Strings.RemoveWhiteSpace(lrEntity.Id & "PK")

    '                If lrERAttribute.PartOfPrimaryKey Then

    '                    If lrRecordset.Facts.Count = 0 Then
    '                        '-------------------------------------------------
    '                        'Must create a Primary Identifier for the Entity
    '                        '-------------------------------------------------
    '                        lsSQLQuery = "INSERT INTO "
    '                        lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
    '                        lsSQLQuery &= " (Entity, Index)"
    '                        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                        lsSQLQuery &= " VALUES ("
    '                        lsSQLQuery &= "'" & lrEntity.Id & "'"
    '                        lsSQLQuery &= ",'" & lsIndexName & "'"
    '                        lsSQLQuery &= ")"

    '                        lrFactInstance = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                    End If

    '                    '-------------------------------------
    '                    'Add the Attribute to the PrimaryKey
    '                    '-------------------------------------
    '                    lsSQLQuery = "INSERT INTO "
    '                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
    '                    lsSQLQuery &= " (Index, Property)"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lsIndexName & "'"
    '                    lsSQLQuery &= ",'" & lrERAttribute.Name & "'"
    '                    lsSQLQuery &= ")"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If
    '                '==========================================================

    '                If IsSomething(lrAddAttributeForm.zrReferencesModelObject) Then
    '                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelation.ToString
    '                    lsSQLQuery &= " (Attribute, Relation)"
    '                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
    '                    lsSQLQuery &= " VALUES ("
    '                    lsSQLQuery &= "'" & lrFactInstanceAttribute.Id & "'"
    '                    lsSQLQuery &= ",'" & lrFactInstanceRelation.Id & "'"
    '                    lsSQLQuery &= " )"

    '                    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
    '                End If
    '            Next

    '        End If

    '        lrAddAttributeForm.Dispose()

    '    End Sub

End Class