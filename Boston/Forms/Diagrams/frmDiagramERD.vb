Imports System.Drawing.Drawing2D

Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Reflection

Public Class frmDiagramERD

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing

    Private morph_vector As tmorphvector
    Private morph_shape As New ShapeNode
    Private MorphVector As New List(Of tMorphVector)

    Public Sub autoLayout()

        '---------------------------------------------------------------------------------------
        ' Create the layouter object
        Dim layout As New MindFusion.Diagramming.Layout.SpringLayout


        ' Adjust the attributes of the layouter
        layout.MultipleGraphsPlacement = MultipleGraphsPlacement.Horizontal
        layout.KeepGroupLayout = True
        layout.NodeDistance = 40
        layout.RandomSeed = 50
        layout.MinimizeCrossings = True
        layout.RepulsionFactor = 50
        layout.EnableClusters = True
        layout.IterationCount = 100
        'layout.GridSize = 20 'Not part of SpringLayout.

        layout.Arrange(Diagram)

        Dim SecondLayout = New MindFusion.Diagramming.Layout.OrthogonalLayout
        SecondLayout.Arrange(Me.Diagram)
        Me.Diagram.RouteAllLinks()

        For Each lrTable In Me.zrPage.Diagram.Nodes
            Dim lrEntity As ERD.Entity
            lrEntity = lrTable.Tag
            lrEntity.Move(lrTable.Bounds.X, lrTable.Bounds.Y, False)
        Next

    End Sub

    Public Shadows Sub BringToFront(Optional asSelectModelElementId As String = Nothing)

        Call MyBase.BringToFront()

        If asSelectModelElementId IsNot Nothing Then
            Me.Diagram.Selection.Items.Clear()

            Dim lrNode As ERD.Entity = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Id = asSelectModelElementId)
            If lrNode IsNot Nothing Then
                lrNode.TableShape.Selected = True
            End If
        End If

    End Sub

    Private Sub frm_EntityRelationshipDiagram_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call SetToolbox()

    End Sub

    Private Sub frmDiagramERD_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

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

        Try
            '-------------------------------------
            'Raised from ModelObjects themselves
            '-------------------------------------
            frmMain.ToolStripButton_Save.Enabled = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Sub LoadERDDiagramPage(ByRef arPage As FBM.Page, ByRef aoTreeNode As TreeNode)

        Dim loDroppedNode As TableNode = Nothing
        Dim lrFactTypeInstance_pt As PointF = Nothing
        Dim lrFactInstance As New FBM.FactInstance
        Dim lrEREntity As ERD.Entity
        Dim lrRecordset1 As ORMQL.Recordset

        Try
            '---------------------------------------------------
            'Set the Caption/Title of the Page to the PageName
            '---------------------------------------------------
            Me.zrPage = arPage
            Me.Tag = arPage
            Me.TabText = arPage.Name

            '-------------------------------------------------------------------------
            'Clear the Entities and Attributes in the Page because it is reloading.
            Me.zrPage.ERDiagram.Entity.Clear()
            Me.zrPage.ERDiagram.Attribute.Clear()
            Me.zrPage.ERDiagram.Relation.Clear()

            Me.zoTreeNode = aoTreeNode

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

            Dim lrEntity As ERD.Entity

            While Not lrRecordset.EOF

                lrFactDataInstance = lrRecordset("Element")
                lrEntity = lrFactDataInstance.CloneEntity(arPage)
                lrEntity.X = lrFactDataInstance.X
                lrEntity.Y = lrFactDataInstance.Y

                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                lrEntity.DisplayAndAssociate()

                '----------------------------------------------
                'RDS
                lrEntity.RDSTable = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrEntity.Name)

                Me.zrPage.ERDiagram.Entity.AddUnique(lrEntity)

                lrRecordset.MoveNext()
            End While

            Dim lrERAttribute As ERD.Attribute
            For Each lrEntity In Me.zrPage.ERDiagram.Entity

                For Each lrColumn In lrEntity.RDSTable.Column

                    lrERAttribute = New ERD.Attribute
                    lrERAttribute.Model = Me.zrPage.Model
                    lrERAttribute.Id = lrColumn.Id
                    lrERAttribute.Entity = lrEntity
                    lrERAttribute.AttributeName = lrColumn.Name
                    lrERAttribute.ResponsibleRole = lrColumn.Role
                    lrERAttribute.ActiveRole = lrColumn.ActiveRole
                    lrERAttribute.ResponsibleFactType = lrERAttribute.ResponsibleRole.FactType
                    lrERAttribute.Mandatory = lrColumn.IsMandatory
                    lrERAttribute.OrdinalPosition = lrColumn.OrdinalPosition
                    lrERAttribute.PartOfPrimaryKey = lrColumn.ContributesToPrimaryKey
                    lrERAttribute.Page = Me.zrPage

                    lrERAttribute.Column = lrColumn

                    lrEntity.Attribute.Add(lrERAttribute)
                    Me.zrPage.ERDiagram.Attribute.Add(lrERAttribute)

                Next

            Next

            ''=====================
            ''Load the Attributes
            ''=====================
            'lsSQLQuery = "SELECT *"
            'lsSQLQuery &= " FROM CoreERDAttribute"
            'lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            'lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            'While Not lrRecordset.EOF

            '    Dim lsMandatory As String = ""

            '    lrERAttribute = New ERD.Attribute
            '    lrERAttribute.Id = lrRecordset("Attribute").Data

            '    lrFactDataInstance = lrRecordset("Attribute")
            '    lrERAttribute = lrFactDataInstance.CloneAttribute(arPage)

            '    '---------------------------------------------------
            '    'Find the ER Entity to add the Attribute to.
            '    '---------------------------------------------------
            '    lrEREntity = New ERD.Entity
            '    lrEREntity.Symbol = lrRecordset("ModelObject").Data

            '    lrEREntity = Me.zrPage.ERDiagram.Entity.Find(AddressOf lrEREntity.EqualsBySymbol)
            '    lrERAttribute.Entity = New ERD.Entity
            '    lrERAttribute.Entity = lrEREntity

            '    '-------------------------------
            '    'Get the Name of the Attribute
            '    '-------------------------------
            '    lsSQLQuery = "SELECT *"
            '    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '    lrERAttribute.AttributeName = lrRecordset1("PropertyName").Data

            '    '----------------------------------------------------
            '    'Get the Role and FactType of the Attribute
            '    lsSQLQuery = "SELECT *"
            '    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '    lrERAttribute.ResponsibleRole = Me.zrPage.Model.Role.Find(Function(x) x.Id = lrRecordset1("Role").Data)
            '    lrERAttribute.ResponsibleFactType = lrERAttribute.ResponsibleRole.FactType

            '    '----------------------------------------------------
            '    'Get the Active Role of the Attribute
            '    lsSQLQuery = "SELECT *"
            '    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            '    lrERAttribute.ActiveRole = Me.zrPage.Model.Role.Find(Function(x) x.Id = lrRecordset1("Role").Data)

            '    '------------------------------------------------------------------------------------------------------
            '    'RDS
            '    lrERAttribute.Entity.RDSTable = Me.zrPage.Model.RDS.Table.Find(Function(x) x.Name = lrERAttribute.Entity.Name)

            '    '-------------------------------------------------
            '    'Check to see whether the Attribute is Mandatory
            '    '-------------------------------------------------
            '    lsSQLQuery = "SELECT COUNT(*)"
            '    lsSQLQuery &= " FROM CoreIsMandatory"
            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '    lsSQLQuery &= " WHERE IsMandatory = '" & lrRecordset("Attribute").Data & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '    If CInt(lrRecordset1("Count").Data) = 1 Then
            '        lrERAttribute.Mandatory = True
            '        lsMandatory = "*"
            '    End If

            '    'Ordinal Position
            '    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            '    lrFactInstance = lrRecordset1.CurrentFact
            '    lrERAttribute.OrdinalPosition = CInt(lrFactInstance.GetFactDataInstanceByRoleName("Position").Data)

            '    '--------------------------------------------------------
            '    'Check to see whether the Entity has a PrimaryKey
            '    '--------------------------------------------------------
            '    lsSQLQuery = "SELECT *"
            '    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
            '    lsSQLQuery &= " WHERE Entity = '" & lrEREntity.Id & "'"

            '    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '    If lrRecordset.Facts.Count > 0 Then
            '        '-------------------------
            '        'Entity has a PrimaryKey
            '        '-------------------------
            '        '=============================================================================================
            '        Dim lsIndexName As String = ""
            '        lsIndexName = Viev.Strings.RemoveWhiteSpace(lrEREntity.Id & "PK")

            '        '-------------------------------------
            '        'Add the Attribute to the PrimaryKey
            '        '-------------------------------------
            '        lsSQLQuery = "SELECT COUNT(*) FROM "
            '        lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
            '        lsSQLQuery &= " WHERE Index = '" & lsIndexName & "'"
            '        lsSQLQuery &= " AND Property = '" & lrERAttribute.Name & "'"

            '        lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '        If CInt(lrRecordset1("Count").Data) > 0 Then
            '            lrERAttribute.PartOfPrimaryKey = True
            '            lrEREntity.PrimaryKey.Add(lrERAttribute)
            '        End If
            '        '=============================================================================================
            '    End If

            '    lrEREntity.Attribute.Add(lrERAttribute)
            '    Me.zrPage.ERDiagram.Attribute.Add(lrERAttribute)

            '    Dim lrColumn As RDS.Column = lrEREntity.RDSTable.Column.Find(Function(x) x.Id = lrERAttribute.Id)
            '    lrERAttribute.Column = lrColumn

            '    lrERAttribute.PartOfPrimaryKey = lrColumn.ContributesToPrimaryKey

            '    lrRecordset.MoveNext()
            'End While

            '-------------------------------------------------------------------
            'Paint the sorted Attributes (By Ordinal Position) for each Entity
            '-------------------------------------------------------------------
            For Each lrEREntity In Me.zrPage.ERDiagram.Entity

                lrEREntity.Attribute.Sort(AddressOf ERD.Attribute.ComparerOrdinalPosition)

                For Each lrERAttribute In lrEREntity.Attribute
                    lrEREntity.TableShape.RowCount += 1
                    lrERAttribute.Cell = lrEREntity.TableShape.Item(0, lrEREntity.TableShape.RowCount - 1)
                    lrEREntity.TableShape.Item(0, lrEREntity.TableShape.RowCount - 1).Tag = lrERAttribute
                    Call lrERAttribute.RefreshShape()

                    lrEREntity.TableShape.ResizeToFitText(False)
                Next

            Next

            '=======================================
            'Map the Relations - Link the Entities 
            '=======================================
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF

                '------------------------
                'Find the Origin Entity
                '------------------------
                Dim lrOrigingEREntity As ERD.Entity
                Dim lsRelationId As String = ""
                lrOrigingEREntity = New ERD.Entity

                lrOrigingEREntity.Symbol = lrRecordset("Entity").Data
                lsRelationId = lrRecordset("Relation").Data

                lrOrigingEREntity = Me.zrPage.ERDiagram.Entity.Find(AddressOf lrOrigingEREntity.EqualsBySymbol)

                '-----------------------------
                'Find the Destination Entity
                '-----------------------------
                Dim lrDestinationgEREntity As ERD.Entity
                lrDestinationgEREntity = New ERD.Entity

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrDestinationgEREntity.Symbol = lrRecordset1("Entity").Data

                lrDestinationgEREntity = Me.zrPage.ERDiagram.Entity.Find(AddressOf lrDestinationgEREntity.EqualsBySymbol)

                If Not lrDestinationgEREntity Is Nothing Then

                    Dim loOriginTableNode As ERD.TableNode = lrOrigingEREntity.TableShape
                    Dim loDestinationTableNode As ERD.TableNode = lrDestinationgEREntity.TableShape

                    '================================
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreOriginMultiplicity"
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsOriginMultiplicity As String = lrRecordset1("Multiplicity").Data

                    Dim liOriginMultiplicity As pcenumCMMLMultiplicity
                    Select Case lsOriginMultiplicity
                        Case Is = pcenumCMMLMultiplicity.One.ToString
                            liOriginMultiplicity = pcenumCMMLMultiplicity.One
                        Case Is = pcenumCMMLMultiplicity.Many.ToString
                            liOriginMultiplicity = pcenumCMMLMultiplicity.Many
                    End Select

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE OriginIsMandatory = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbRelationOriginIsMandatory As Boolean = False
                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbRelationOriginIsMandatory = True
                    End If

                    '================================
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreDestinationMultiplicity"
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsDestinationMultiplicity As String = lrRecordset1("Multiplicity").Data

                    Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
                    Select Case lsDestinationMultiplicity
                        Case Is = pcenumCMMLMultiplicity.One.ToString
                            liDestinationMultiplicity = pcenumCMMLMultiplicity.One
                        Case Is = pcenumCMMLMultiplicity.Many.ToString
                            liDestinationMultiplicity = pcenumCMMLMultiplicity.Many
                    End Select

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbRelationDestinationIsMandatory As Boolean = False
                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbRelationDestinationIsMandatory = True
                    End If

                    '-------------------------------------------------------------------------------
                    'Check to see whether the Relation contributes to the PrimaryKey of the Entity
                    '-------------------------------------------------------------------------------
                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & " = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbContributesToPrimaryKey As Boolean = False

                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbContributesToPrimaryKey = True
                    End If

                    Dim lrRelation As New ERD.Relation(Me.zrPage.Model,
                                                       Me.zrPage,
                                                       lrRecordset("Relation").Data,
                                                       lrOrigingEREntity, _
                                                       liOriginMultiplicity, _
                                                       lbRelationOriginIsMandatory, _
                                                       lbContributesToPrimaryKey, _
                                                       lrDestinationgEREntity, _
                                                       liDestinationMultiplicity, _
                                                       lbRelationDestinationIsMandatory)

                    lrRelation.Id = lsRelationId

                    '-------------------------------------
                    'Get the Predicates for the Relation
                    '-------------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    If Not lrRecordset1.EOF Then
                        lrRelation.OriginPredicate = lrRecordset1("Predicate").Data
                    End If


                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    If Not lrRecordset1.EOF Then
                        lrRelation.DestinationPredicate = lrRecordset1("Predicate").Data
                    End If

                    '-----------------------------------
                    'Get the FactType for the Relation
                    '-----------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                    lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrFactType As New FBM.FactType(Me.zrPage.Model, lrRecordset1("FactType").Data, True)
                    lrFactType = Me.zrPage.Model.FactType.Find(AddressOf lrFactType.Equals)
                    lrRelation.RelationFactType = lrFactType

                    '====================================================================
                    'RDS
                    lrRelation.RDSRelation = Me.zrPage.Model.RDS.Relation.Find(Function(x) x.Id = lrRelation.Id)

                    Me.zrPage.ERDiagram.Relation.Add(lrRelation)

                    Dim lrLink As ERD.Link
                    lrLink = New ERD.Link(Me.zrPage, New FBM.FactInstance, lrOrigingEREntity, lrDestinationgEREntity, Nothing, Nothing, lrRelation)
                    lrLink.DisplayAndAssociate()
                    lrRelation.Link = lrLink

                End If

                lrRecordset.MoveNext()
            End While

            '==================================================================
            'Subtype Relationships
            Call Me.mapSubtypeRelationships

            If Me.areAllEntitiesAtPoint00() Then
                Call Me.autoLayout()
            End If

            Me.Diagram.RoutingOptions.GridSize = 2
            Me.Diagram.RouteLinks = False

            Me.Diagram.RouteAllLinks()

            Dim layout As New MindFusion.Diagramming.Layout.OrthogonalLayout
            layout.Arrange(Me.Diagram)

            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Public Function areAllEntitiesAtPoint00() As Boolean

        For Each lrEntity In Me.zrPage.ERDiagram.Entity
            If (lrEntity.X <> 0) Or (lrEntity.Y <> 0) Then
                Return False
            End If
        Next

        Return True

    End Function

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.ERDShapeLibrary)

        Dim lrToolboxForm As frmToolbox
        lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

        If IsSomething(lrToolboxForm) Then
            lrToolboxForm.ShapeListBox.Shapes = lsl_shape_library.Shapes
        End If

    End Sub

    Private Sub ETDDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call SetToolbox()

    End Sub

    Private Sub ContextMenuStrip_Entity_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Entity.Opening

        Dim lrPage As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lrEntity As New ERD.Entity

        Try
            If Me.zrPage.SelectedObject.Count = 0 Then
                Exit Sub
            End If

            '-------------------------------------
            'Check that selected object is Actor
            '-------------------------------------
            If Me.zrPage.SelectedObject(0).GetType Is lrEntity.GetType Then
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

            lrEntity = Me.zrPage.SelectedObject(0)

            lr_model = lrEntity.Model

            '--------------------------------------------------------------------
            'ModelErrors - Add menu items for the ModelErrors for the EntityType
            '--------------------------------------------------------------------
            Me.ToolStripMenuItemEntityModelErrors.DropDownItems.Clear()
            Dim lo_menu_option As ToolStripItem
            Dim lrModelError As FBM.ModelError
            Me.ToolStripMenuItemEntityModelErrors.Image = My.Resources.MenuImages.Cloud216x16
            Dim liErrorCount As Integer = 0
            For Each lrAttribute In lrEntity.Attribute
                If lrAttribute.Column.getMetamodelDataType = pcenumORMDataType.DataTypeNotSet Then
                    If lrAttribute.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                        Dim lrValueType As FBM.ValueType = lrAttribute.Column.ActiveRole.JoinedORMObject
                        For Each lrModelError In lrValueType.ModelError
                            Me.ToolStripMenuItemEntityModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                            lo_menu_option = Me.ToolStripMenuItemEntityModelErrors.DropDownItems.Add(lrModelError.Description)
                            lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                            liErrorCount += 1
                        Next
                    ElseIf lrAttribute.ActiveRole.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                        Dim lrEntityType As FBM.EntityType = lrAttribute.ActiveRole.JoinedORMObject
                        For Each lrModelError In lrEntityType.ModelError
                            Me.ToolStripMenuItemEntityModelErrors.Image = My.Resources.MenuImages.RainCloudRed16x16
                            lo_menu_option = Me.ToolStripMenuItemEntityModelErrors.DropDownItems.Add(lrModelError.Description)
                            lo_menu_option.Image = My.Resources.MenuImages.RainCloudRed16x16
                            liErrorCount += 1
                        Next
                    End If
                End If
            Next
            If liErrorCount = 0 Then
                lo_menu_option = Me.ToolStripMenuItemEntityModelErrors.DropDownItems.Add("There are no Model Errors for this Entity.")
                lo_menu_option.Image = My.Resources.MenuImages.Cloud216x16
            End If

            '========================
            'Morphing
            '---------------------------------------------------------------------------------------------
            'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
            '  shape, and to/into another diagram starts at the MorphVector.
            '---------------------------------------------------------------------------------------------
            Me.morph_vector = New tMorphVector(lrEntity.X, lrEntity.Y, 0, 0, 40)
            Me.MorphVector.Clear()
            Me.MorphVector.Add(New tMorphVector(lrEntity.TableShape.Bounds.X, lrEntity.TableShape.Bounds.Y, 0, 0, 40))

            '-------------------------------------------------------------------------------
            Me.DisplayDataIndexRelationInformationToolStripMenuItem.Checked = lrEntity.DisplayRDSData

            '--------------------------------------------------------------
            'Clear the list of ORMDiagrams that may relate to the EntityType
            '--------------------------------------------------------------
            Me.ToolStripMenuItemORMDiagram.DropDownItems.Clear()
            Me.ToolStripMenuItemEntityRelationshipDiagram.DropDownItems.Clear()
            Me.PGSDiagramToolStripMenuItem.DropDownItems.Clear()

            Dim loMenuOption As ToolStripItem
            '-------------------------------------------------------------------
            'Load the ERDs that relate to the Entity as selectable menuOptions
            '--------------------------------------------------------                        
            larPage_list = prApplication.CMML.GetERDiagramPagesForEntity(lrEntity)

            For Each lrPage In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageERD, _
                                                           lrPage, _
                                                           lr_model.ModelId, _
                                                           pcenumLanguage.EntityRelationshipDiagram, _
                                                           Nothing, _
                                                           lrPage.PageId)

                lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.ToolStripMenuItemEntityRelationshipDiagram.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ERD16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.MorphToERDiagram
                End If

            Next

            '------------------------------------------------------------------------------
            'Load the ORMDiagrams that relate to the EntityType as selectable menuOptions
            '--------------------------------------------------------                        
            larPage_list = prApplication.CMML.getORMDiagramPagesForEntity(lrEntity)

            For Each lrPage In larPage_list
                '----------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is now added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel, _
                                                           lrPage, _
                                                           lr_model.ModelId, _
                                                           pcenumLanguage.ORMModel, _
                                                           Nothing, _
                                                           lrPage.PageId)

                lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.ToolStripMenuItemORMDiagram.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.ORM16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.morphToORMDiagram
                End If

            Next

            '--------------------------------------------------------------------------------------------------------
            'Get the PGS Diagrams for the selected Node.
            larPage_list = prApplication.CMML.getPGSDiagramPagesForModelElementName(lrEntity.Model, lrEntity.Id)

            For Each lrPage In larPage_list
                '---------------------------------------------------------------------------------------------------------
                'Try and find the Page within the EnterpriseView.TreeView
                '  NB If 'Core' Pages are not shown for the model, they will not be in the TreeView and so a menuOption
                '  is not added for those hidden Pages.
                '----------------------------------------------------------
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = prPageNodes.Find(Function(x) x.ModelId = lrEntity.Model.ModelId And x.PageId = lrPage.PageId)

                If IsSomething(lrEnterpriseView) Then
                    '---------------------------------------------------
                    'Add the Page(Name) to the MenuOption.DropDownItems
                    '---------------------------------------------------
                    loMenuOption = Me.PGSDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name, My.Resources.MenuImages.PGS16x16)
                    loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                    AddHandler loMenuOption.Click, AddressOf Me.morphToPGSDiagram
                End If
            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_CellClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs) Handles Diagram.CellClicked

        Dim lrAttribute As ERD.Attribute
        lrAttribute = e.Cell.Tag

        If e.MouseButton = MouseButton.Right Then
            'ContextMenuStripAttribute.Show(Me.ERDDiagramView, New Point((e.Table.Bounds.X + e.MousePosition.X), (e.Table.Bounds.Y + e.MousePosition.Y)))




            Me.ToolStripMenuItemIsMandatory.Checked = lrAttribute.Mandatory

            ContextMenuStripAttribute.Show(Me.DiagramView, Me.DiagramView.DocToClient(e.MousePosition))
            'Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Role

            Me.zrPage.SelectedObject.Clear()
            Me.zrPage.SelectedObject.Add(lrAttribute)
        Else
            Dim lrTableNode As MindFusion.Diagramming.TableNode

            Call Me.reset_node_and_link_colors()

            e.Cell.TextColor = Color.White
            e.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)
            Me.Diagram.Invalidate()

            For Each lrTableNode In Me.Diagram.Nodes
                Call lrTableNode.Tag.ResetAttributeCellColours()
            Next

            '-----------------------------------------------
            'Paint the cell again. First is for GUI speed.
            '-----------------------------------------------
            e.Cell.TextColor = Color.White
            e.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)

            '--------------------------------------------------------------------
            'Highlight the Relationships (if any) associated with the Attribute
            '--------------------------------------------------------------------
            For Each lrRelation In lrAttribute.Column.Relation
                Dim lrERDRelation As ERD.Relation = Me.zrPage.ERDiagram.Relation.Find(Function(x) x.Id = lrRelation.Id)
                If lrERDRelation IsNot Nothing Then
                    lrERDRelation.Link.Color = Color.Blue
                    Me.Diagram.Invalidate()
                End If
            Next

            '--------------------------------------
            'Set the PropertiesGrid.SeletedObject
            '--------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute})
                lrPropertyGridForm.PropertyGrid.SelectedObject = lrAttribute
            End If

            '-------------------------------------------------------
            'Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Call lrToolboxForm.VerbaliseAttribute(lrAttribute)
            End If

            lrAttribute.Entity.TableShape.ResizeToFitText(False)

            Me.Diagram.RouteAllLinks()
            Dim layout As New MindFusion.Diagramming.Layout.OrthogonalLayout
            layout.Arrange(Me.Diagram)

        End If
    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        '-------------------------
        'Create the ERD.Relation
        '-------------------------
        Dim lrRelation As New ERD.Relation

        lrRelation.OriginEntity = e.Link.Origin.Tag
        lrRelation.DestinationEntity = e.Link.Destination.Tag
        lrRelation.OriginMultiplicity = pcenumCMMLMultiplicity.One
        lrRelation.DestinationMultiplicity = pcenumCMMLMultiplicity.One
        lrRelation.OriginAttribute = lrRelation.OriginEntity.TableShape.Item(0, e.Link.OriginIndex).Tag
        lrRelation.DestinationAttribute = lrRelation.DestinationEntity.TableShape.Item(0, e.Link.DestinationIndex).Tag

        Dim lsSQLQuery As String = ""
        Dim lrFact As FBM.Fact

        lsSQLQuery = "INSERT INTO ERDRelation (ModelObject, Relation)"
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " VALUES ("
        lsSQLQuery &= "'" & lrRelation.OriginEntity.Name & "'"
        lsSQLQuery &= ",'" & lrRelation.DestinationEntity.Name & "')"

        lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        '-------------------------------
        'Create a Concept for the Fact
        '-------------------------------
        Dim lrConcept As New FBM.Concept(lrFact.Id)
        lrConcept.Save()

        '-------------------------------------------------
        'Create a new ModelDictionary entry for the Fact
        '-------------------------------------------------
        Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.zrPage.Model, lrFact.Id, pcenumConceptType.Fact, "ERDRelation")
        lrDictionaryEntry = Me.zrPage.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
        lrDictionaryEntry.isFact = True
        lrDictionaryEntry.Save()

        lsSQLQuery = "INSERT INTO OriginMultiplicity (ERDRelation, Multiplicity)"
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " VALUES ("
        lsSQLQuery &= "'" & lrFact.Id & "'"
        lsSQLQuery &= ",'One')"

        Dim lrOriginMultiplicityFact As FBM.Fact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        lsSQLQuery = "INSERT INTO DestinationMultiplicity (ERDRelation, Multiplicity)"
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " VALUES ("
        lsSQLQuery &= "'" & lrFact.Id & "'"
        lsSQLQuery &= ",'One')"

        Dim lrDestinationMultiplicityFact As FBM.Fact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

    End Sub

    Private Sub Diagram_LinkModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkModified

        Call e.Link.Route() 'Was Call Me.Diagram.RouteAllLinks(), but this made it almost impossible for a User to move a link.

    End Sub

    Private Sub Diagram_NodeCreating(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeCreating

        e.Cancel = True

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        frmMain.ToolStripButton_Save.Enabled = True

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        Dim lrTableNode As TableNode

        Dim lrFactInstance As New FBM.FactInstance

        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Entity

                lrTableNode = e.Node

                Dim lrEntity As ERD.Entity
                lrEntity = e.Node.Tag
                lrEntity.Move(e.Node.Bounds.X, e.Node.Bounds.Y, False)

                'lrFactDataInstance = lrTableNode.Tag.FactDataInstance
                'lrFactDataInstance.X = e.Node.Bounds.X
                'lrFactDataInstance.Y = e.Node.Bounds.Y
                Me.zrPage.MakeDirty()

                'lrTableNode.Tag.X = lrFactDataInstance.X
                'lrTableNode.Tag.Y = lrFactDataInstance.Y
            Case Else

        End Select

        Me.Diagram.RouteAllLinks()

        Dim layout As New MindFusion.Diagramming.Layout.OrthogonalLayout
        layout.Arrange(Me.Diagram)

    End Sub

    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Entity
                e.Node.Pen.Color = Color.Blue
            Case Else
                'Do nothing
        End Select

        '----------------------------------------------------
        'Set the ContextMenuStrip menu for the selected item
        '----------------------------------------------------
        Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
            Case Is = pcenumConceptType.Entity
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Entity
                Dim lrEntity As ERD.Entity = Me.Diagram.Selection.Items(0).Tag
                Call lrEntity.NodeSelected()
        End Select

        Me.zrPage.SelectedObject.Add(e.Node.Tag)

        '--------------------------------------
        'Set the PropertiesGrid.SeletedObject
        '--------------------------------------
        Dim lrPropertyGridForm As frmToolboxProperties

        Dim lrERDTableNode As ERD.TableNode
        lrERDTableNode = e.Node.Tag.TableShape

        Dim loMousePoint As PointF = Me.DiagramView.ClientToDoc(New Point(Me.DiagramView.PointToClient(Control.MousePosition).X, Me.DiagramView.PointToClient(Control.MousePosition).Y))

        If loMousePoint.X > lrERDTableNode.Bounds.Left And loMousePoint.X < lrERDTableNode.Bounds.Right _
           And loMousePoint.Y > lrERDTableNode.Bounds.Top And loMousePoint.Y < (lrERDTableNode.Bounds.Y + lrERDTableNode.CaptionHeight) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
            End If

            '-------------------------------------------------------
            'ORM Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.Entity
                        Call lrToolboxForm.VerbaliseEntity(e.Node.Tag)
                End Select
            End If
        End If


    End Sub

    Public Sub mapSubtypeRelationships()

        Try
            Dim lrEntity, lrSubtypeEntity As ERD.Entity

            For Each lrEntity In Me.zrPage.ERDiagram.Entity

                Dim lrRDSTable = lrEntity.getCorrespondingRDSTable

                For Each lrSubtypeTable In lrRDSTable.getSubtypeTables

                    lrSubtypeEntity = Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrSubtypeTable.Name)

                    If lrSubtypeEntity IsNot Nothing Then
                        Dim lo_link As New DiagramLink(Me.zrPage.Diagram, lrSubtypeEntity.TableShape, lrEntity.TableShape)
                        lo_link.HeadShape = ArrowHead.Arrow
                        lo_link.Pen.Color = Color.Gray
                        lo_link.Locked = True
                        Me.zrPage.Diagram.Links.Add(lo_link)
                        lrSubtypeEntity.TableShape.OutgoingLinks.Add(lo_link)
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

    Public Sub MorphToERDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim item As ToolStripItem = CType(sender, ToolStripItem)

            Me.HiddenDiagram.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            'If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub

            Dim lr_page_object As New FBM.PageObject
            lr_page_object = Me.zrPage.SelectedObject(0).ClonePageObject

            Dim lrShapeNode As MindFusion.Diagramming.ShapeNode
            lrShapeNode = lr_page_object.Shape.Clone(True)
            lrShapeNode = New MindFusion.Diagramming.ShapeNode(lr_page_object.Shape)
            lrShapeNode.Shape = Shapes.RoundRect
            lrShapeNode.SetRect(Me.zrPage.SelectedObject(0).TableShape.Bounds, False)
            lrShapeNode.Font = New System.Drawing.Font("Arial", 10)
            lrShapeNode.TextFormat.Alignment = StringAlignment.Center

            lrShapeNode.Text = lr_page_object.Name

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
                Dim lr_page As New FBM.Page(lrEnterpriseView.Tag.Model)
                lr_page = lrEnterpriseView.Tag

                Dim larFactDataInstance = From FactTypeInstance In lr_page.FactTypeInstance _
                                   From FactInstance In FactTypeInstance.Fact _
                                   From FactDataInstance In FactInstance.Data _
                                   Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                   And FactDataInstance.Role.Name = pcenumCMML.Element.ToString _
                                   And FactDataInstance.Concept.Symbol = lr_page_object.Name _
                                   Select New FBM.FactDataInstance(lr_page, FactInstance, FactDataInstance.Role, FactDataInstance.Concept, FactDataInstance.X, FactDataInstance.Y)

                Dim lrFactDataInstance As New Object

                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                'Start size
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)
                'End Size
                Me.MorphVector(0).EndSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.x, lrFactDataInstance.y)
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

    Public Sub morphToORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '========================================================================================================
        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        'Set the Zoom factor
        Me.HiddenDiagramView.ZoomFactor = My.Settings.DefaultPageZoomFactor
        Call Me.HiddenDiagramView.BringToFront()

        Dim lrPageObject As New FBM.PageObject
        lrPageObject = Me.zrPage.SelectedObject(0).ClonePageObject

        Dim lrShapeNode As MindFusion.Diagramming.ShapeNode
        lrShapeNode = lrPageObject.Shape.Clone(False)
        lrShapeNode = New MindFusion.Diagramming.ShapeNode(lrPageObject.Shape)
        lrShapeNode.Shape = Shapes.RoundRect
        lrShapeNode.SetRect(Me.zrPage.SelectedObject(0).TableShape.Bounds, False)
        lrShapeNode.Font = New System.Drawing.Font("Arial", 10)
        lrShapeNode.TextFormat.Alignment = StringAlignment.Center

        lrShapeNode.Text = lrPageObject.Name

        Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
        Me.MorphVector(0).Shape = lrShapeNode
        Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
        Me.HiddenDiagram.Invalidate()
        '========================================================================================================

        Dim lrEntity As New ERD.Entity
        lrEntity = Me.zrPage.SelectedObject(0)

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = item.Tag
            prApplication.WorkingPage = lrEnterpriseView.Tag

            '---------------------------------------------------------------
            'Get the X,Y co-ordinates of the ModelElement being morphed to
            '------------------------------------------------------
            Dim lrPage As FBM.Page
            lrPage = lrEnterpriseView.Tag

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance(lrPage.Model, pcenumLanguage.ORMModel, lrEntity.Data, True, 0, 0)
            Dim lrFactTypeInstance As New FBM.FactTypeInstance(lrPage.Model, lrPage, pcenumLanguage.ORMModel, lrEntity.Data, True, 0, 0)

            lrFactTypeInstance = lrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
            lrEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

            'Start size
            Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.MorphVector(0).Shape.Bounds.Width, Me.MorphVector(0).Shape.Bounds.Height)

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
                        Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrFactTypeInstance.X - lrShapeNode.Bounds.X) + Math.Abs(lrFactTypeInstance.Y - lrShapeNode.Bounds.Y) + 1))
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
                    Me.MorphVector(0).VectorSteps = Viev.Lesser(25, (Math.Abs(lrEntityTypeInstance.X - lrShapeNode.Bounds.X) + Math.Abs(lrEntityTypeInstance.Y - lrShapeNode.Bounds.Y) + 1))
                End If

            End If

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            'Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X, _
            '                                     Me.MorphVector(0).StartPoint.Y, _
            '                                     lrEntityTypeInstance.X, _
            '                                     lrEntityTypeInstance.Y, _
            '                                     40, _
            '                                     Me.MorphVector(0).Shape)

            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()


        End If

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
            lrShapeNode.Shape = Shapes.RoundRect
            lrShapeNode.SetRect(Me.zrPage.SelectedObject(0).TableShape.Bounds, False)

            lrShapeNode.Text = lrPageObject.Name

            Me.MorphVector(0).ModelElementId = Me.zrPage.SelectedObject(0).Id
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
                    Me.MorphVector(0).EndPoint = New Point(lrNode.shape.Bounds.X, lrNode.shape.Bounds.Y) ' (lrFactDataInstance.x, lrFactDataInstance.y)

                    If lrNode.NodeType = pcenumPGSEntityType.Relationship Then
                        Dim lrRelation As ERD.Relation
                        lrRelation = lrPage.ERDiagram.Relation.FindAll(Function(x) x.ActualPGSNode IsNot Nothing).Find(Function(x) x.ActualPGSNode.Id = lrPageObject.Name)

                        Dim lrPGSLink As PGS.Link = lrRelation.Link

                        Me.MorphVector(0).EndSize = New Rectangle(lrPGSLink.Link.Bounds.X, lrPGSLink.Link.Bounds.Y, lrPGSLink.Link.Bounds.Width, Viev.Greater(1, lrPGSLink.Link.Bounds.Height))
                        Me.MorphVector(0).EndPoint = New Point(lrRelation.Link.Link.Bounds.X - lrPage.DiagramView.ScrollX, lrRelation.Link.Link.bounds.Y - lrPage.DiagramView.ScrollY)
                        Me.MorphVector(0).VectorSteps = Viev.Greater(15, (Math.Abs(lrRelation.Link.Link.Bounds.X - lrShapeNode.Bounds.X) + Math.Abs(lrRelation.Link.Link.Bounds.Y - lrShapeNode.Bounds.Y) + 1)) / 3
                    Else
                        Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 20)
                        Me.MorphVector(0).EndPoint = New Point(lrNode.shape.Bounds.X - lrPage.DiagramView.ScrollX, lrNode.shape.Bounds.Y - lrPage.DiagramView.ScrollY) 'lrFactDataInstance.X, lrFactDataInstance.Y)
                    End If
                Else
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 20)
                    Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.X, lrFactDataInstance.Y)
                    Me.MorphVector(0).VectorSteps = Viev.Greater(15, (Math.Abs(lrFactDataInstance.X - lrShapeNode.Bounds.X) + Math.Abs(lrFactDataInstance.Y - lrShapeNode.Bounds.Y) + 1)) / 3
                End If

                '===========================================
                Me.MorphVector(0).Shape.Font = Me.zrPage.Diagram.Font
                Me.MorphVector(0).Shape.Text = Me.MorphVector(0).ModelElementId
                Me.MorphVector(0).Shape.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                Me.MorphVector(0).Shape.TextFormat.Alignment = StringAlignment.Center
                Me.MorphVector(0).Shape.TextFormat.LineAlignment = StringAlignment.Center

                Me.MorphVector(0).Shape.SetRect(New RectangleF(lrPageObject.X, lrPageObject.Y, 20, 20), False)
                Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
                Me.MorphVector(0).TargetShape = pcenumTargetMorphShape.Circle

                'Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True
                Me.MorphStepTimer.Tag = lrEnterpriseView.TreeNode
                Me.MorphStepTimer.Start()
                'Me.MorphTimer.Start()

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
        '                               Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
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
        '                              Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
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

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        'Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick


        Try
            Dim lrPoint As New Point
            Dim lrRectangle As Rectangle

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
                End If
            Next

            Me.HiddenDiagram.Invalidate()

            If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
                Me.MorphStepTimer.Stop()
                Me.MorphStepTimer.Enabled = False
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
                Call frmMain.zfrmModelExplorer.LoadSelectedPage(Me.MorphVector(0).ModelElementId)
                System.Threading.Thread.Sleep(2000)
                Me.DiagramView.BringToFront()
                Me.Diagram.Invalidate()
                Me.MorphTimer.Enabled = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim lsMessage As String

        Try
            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

                '------------------------------------------------------------------------------------------------------------------------------------
                'Make sure the current page points to the Diagram on this form. The reason is that the user may be viewing the Page as an ORM Model
                '------------------------------------------------------------------------------------------------------------------------------------
                Me.zrPage.Diagram = Me.Diagram

                Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

                Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
                Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

                If loDraggedNode.Index >= 0 Then

                    Dim lrToolboxForm As frmToolbox
                    lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

                    If loDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount Then

                        Select Case lrToolboxForm.ShapeListBox.Shapes(loDraggedNode.Index).Id
                            Case Is = "Entity"
                                '---------------------------------------------------------------
                                'Establish a new EntityType and Entity for the dropped object.
                                '---------------------------------------------------------------    

                                Dim lrEntityType As New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel)
                                lrEntityType = Me.zrPage.Model.CreateEntityType

                                Dim lsEntityName As String = Me.zrPage.Model.CreateUniqueEntityName(lrEntityType.Name)
                                Dim lrEntity As New ERD.Entity(Me.zrPage, lsEntityName)
                                '=========================================================================================

                                Call Me.zrPage.DropEntityAtPoint(lrEntity, loPointF)

                                Me.zrPage.ERDiagram.Entity.Add(lrEntity)
                                '=========================================================================================
                        End Select
                    End If
                ElseIf loDraggedNode.Tag.GetType Is GetType(RDS.Table) Then

                    Dim lrTable As RDS.Table
                    lrTable = loDraggedNode.Tag

                    '------------------
                    'Load the Entity.
                    '==================================================================================================================
                    Dim lsSQLQuery As String = ""
                    Dim lrRecordset As ORMQL.Recordset
                    Dim lrFactInstance As FBM.FactInstance

                    Dim lrEntity As ERD.Entity

                    If Me.zrPage.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name) IsNot Nothing Then
                        lsMessage = "This page already has the Entity, '" & lrTable.Name & "', loaded on it. Please choose another Entity to drop on the page."
                        MsgBox(lsMessage)
                        Exit Sub
                    End If

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " WHERE Element = '" & lrTable.Name & "'"

                    lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset.EOF Then
                        lsMessage = "The Entity, '" & lrTable.Name & "', does not seem to have any Attributes at this stage. Make sure that the corresponding Model Element in your Object-Role Model at least has a Primary Reference Scheme."
                        lsMessage &= vbCrLf & vbCrLf & "Entities in Entity-Relationship Diagrams in Boston have their Attributes created by the relative relations of the corresponding Model Element in your Object-Role Model."

                        If Me.zrPage.Model.GetModelObjectByName(lrTable.Name).ConceptType = pcenumConceptType.EntityType Then
                            lsMessage &= vbCrLf & vbCrLf & "i.e. Make sure the Entity Type, '" & lrTable.Name & "', at least has a Primary Reference Scheme in your Object-Role Model."
                        End If
                        MsgBox(lsMessage)
                        Exit Sub
                    Else
                        lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

                        lrFactInstance = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrEntity = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneEntity(Me.zrPage)
                        '===================================================================================================================
                        With New WaitCursor
                            lrEntity.RDSTable = lrTable 'IMPORTANT: Leave this at this point in the code. Otherwise (somehow) lrEntity ends up with no TableShape.

                            Call Me.zrPage.DropExistingEntityAtPoint(lrEntity, loPointF, False)
                            Call Me.zrPage.loadRelationsForEntity(lrEntity, False)
                            Call lrEntity.Move(loPointF.X, loPointF.Y, True)
                            Call Me.zrPage.Save(False, False)
                        End With

                    End If
                End If
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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
                        Case Is = "Entity"
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

        Try
            Dim lo_point As System.Drawing.PointF
            Dim loNode As DiagramNode

            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

            '--------------------------------------------------
            'Just to be sure...set the Richmond.WorkingProject
            '--------------------------------------------------
            prApplication.WorkingPage = Me.zrPage

            '---------------------------------------------------------------------------------------------------------------------
            'RichtClick handling is mostly taken care of in Diagram.NodeSelected
            '---------------------------------------------------------------------
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Dim loSelectedNode As Object = Diagram.GetNodeAt(lo_point)
                Dim loSelectedLink As DiagramLink = Diagram.GetLinkAt(lo_point, 2)
                If (loSelectedNode Is Nothing) And (loSelectedLink Is Nothing) Then
                    Me.zrPage.SelectedObject.Clear()
                    Me.Diagram.Selection.Clear()
                    Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
                End If

                Exit Sub
            End If

            loNode = Diagram.GetNodeAt(lo_point)
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
                    Me.Diagram.Selection.Clear()
                    '----------------------------------------------------
                    'Select the ShapeNode/ORMObject just clicked on
                    '  Updates the Me.zrPage.SelectedObject collection.
                    '----------------------------------------------------                 
                    loNode.Selected = True

                End If

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
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                '------------------------------------------------
                'Keep, so that ContextMenu is not changed from
                '  how set in Diagram.NodeSelected
                '-----------------------------------------------
            Else
                '------------------------------------------------
                'User Left-Clicked on the Canvas
                '------------------------------------------------

                '---------------------------
                'Clear the SelectedObjects
                '---------------------------
                Me.zrPage.SelectedObject.Clear()
                Me.Diagram.Selection.Clear()

                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

                Me.Diagram.AllowUnconnectedLinks = False

                '-------------------------------------------------------------------------------------------------------------------
                'Clear the 'InPlaceEdit' on principal.
                '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas, so disable the 'InPlaceEdit'.
                '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on, then 'InPlaceEdit' is temporarily allowed.
                '------------------------------------------------------------------------------
                Me.DiagramView.AllowInplaceEdit = False

                '----------------------------------------------------------
                'Set the PropertiesGrid.SeletedObject to the Page itself
                Dim lrPropertyGridForm As frmToolboxProperties = prApplication.GetToolboxForm(frmToolboxProperties.Name)

                If IsSomething(lrPropertyGridForm) Then

                    Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                    ' And you pass it to the PropertyGrid,
                    ' via its BrowsableAttributes property :
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

                End If
                '---------------------------------------------------------------------------------------------------------------------------

                Call Me.reset_node_and_link_colors()
            End If

            Me.Diagram.RouteAllLinks()

            Dim layout As New MindFusion.Diagramming.Layout.OrthogonalLayout
            layout.Arrange(Me.Diagram)

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Sub reset_node_and_link_colors()

        Dim liInd As Integer = 0

        '------------------------------------------------------------------------------------
        'Reset the border colors of the ShapeNodes (to what they were before being selected
        '------------------------------------------------------------------------------------
        For liInd = 1 To Diagram.Nodes.Count
            Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                Case Is = pcenumConceptType.FactType
                    If Diagram.Nodes(liInd - 1).Tag.IsObjectified Then
                        Diagram.Nodes(liInd - 1).Visible = True
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                    Else
                        Diagram.Nodes(liInd - 1).Visible = False
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Pink
                    End If
                Case Is = pcenumConceptType.RoleName
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                Case Is = pcenumConceptType.RoleConstraint
                    Select Case Diagram.Nodes(liInd - 1).Tag.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.Maroon
                        Case Is = pcenumRoleConstraintType.ExclusionConstraint, _
                                  pcenumRoleConstraintType.ExternalUniquenessConstraint
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                    End Select
                Case Is = pcenumConceptType.FactTable
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                Case Else
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
            End Select
        Next

        For Each lrRelation In Me.zrPage.ERDiagram.Relation
            lrRelation.Link.Color = Color.Black
        Next

        Dim lrTableNode As MindFusion.Diagramming.TableNode

        For Each lrTableNode In Me.Diagram.Nodes
            Call lrTableNode.Tag.ResetAttributeCellColours()
        Next

        Me.Diagram.Invalidate()

    End Sub

    Private Sub ERDDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseWheel

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


    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
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
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.Model
            End If
        End If

    End Sub

    Private Sub AutoLayoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoLayoutToolStripMenuItem.Click

    End Sub

    ''' <summary>
    ''' Diagram.LinkCustomDraw is set to 'Full'. This method is called for all Links on the diagram.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomDrawLink(ByVal sender As Object, ByVal e As MindFusion.Diagramming.DrawLinkEventArgs) Handles Diagram.DrawLink

        If e.Shadow Then
            ' We are not drawing arrows shadows
            Return
        End If

        If e.Link.GetType Is GetType(ERD.tERDLink) Then
            Return
        End If

        Dim liInd As Integer
        Dim lrPen As New System.Drawing.Pen(Color.Black, 0.03)

        Dim pt1 As PointF
        Dim pt2 As PointF

        If e.Link.GetType Is GetType(MindFusion.Diagramming.DiagramLink) Then

            For liInd = 0 To e.Link.ControlPoints.Count - 2

                pt1 = New PointF(e.Link.ControlPoints(liInd).X, e.Link.ControlPoints(liInd).Y)
                pt2 = New PointF(e.Link.ControlPoints(liInd + 1).X, e.Link.ControlPoints(liInd + 1).Y)

                lrPen = New System.Drawing.Pen(ColorTranslator.FromHtml("#BDBCBC"))

                lrPen.DashStyle = DashStyle.Dash

                lrPen.Width = 0.001
                e.Graphics.DrawLine(lrPen, pt1, pt2)
            Next

            '---------------------------------------
            'Arrow
            Dim a As PointF = pt2
            Dim an As Double = 0, r = 0

            Dim lsglOffset As Single = 0
            If pt1.X <= pt2.X Then
                lsglOffset = 2.7
                an = 270
            Else
                lsglOffset = -2.7
                an = 270
            End If

            If pt1.Y > pt2.Y Then
                lsglOffset = 2.7
                an = 0
            ElseIf pt1.Y < pt2.Y Then
                lsglOffset = -2.7
                an = 0
            End If

            pt1 = New PointF
            pt2 = New PointF
            ' Find two points around the origin control point
            Conversion.PolarToDekart(a, an - 75, lsglOffset, pt1)
            Conversion.PolarToDekart(a, an, 0, pt2)
            e.Graphics.DrawLine(lrPen, pt1, pt2)

            Conversion.PolarToDekart(a, an - 105, lsglOffset, pt1)
            Conversion.PolarToDekart(a, an, 0, pt2)
            e.Graphics.DrawLine(lrPen, pt1, pt2)
            Return
        End If

        lrPen = New System.Drawing.Pen(Color.Black, 0.03)


        For liInd = 0 To e.Link.ControlPoints.Count - 2

            pt1 = New PointF(e.Link.ControlPoints(liInd).X, e.Link.ControlPoints(liInd).Y)
            pt2 = New PointF(e.Link.ControlPoints(liInd + 1).X, e.Link.ControlPoints(liInd + 1).Y)

            lrPen = New System.Drawing.Pen(Color.Black)

            Select Case liInd
                Case Is = 0
                    lrPen.DashStyle = DashStyle.Solid
                Case Else
                    lrPen.DashStyle = DashStyle.Dash
            End Select

            lrPen.Width = 0.001
            e.Graphics.DrawLine(lrPen, pt1, pt2)
        Next

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Try
            Dim lrERDLink As ERD.Link

            If e.Link.GetType Is GetType(MindFusion.Diagramming.DiagramLink) Then Exit Sub

            lrERDLink = e.Link.Tag

            Call lrERDLink.LinkSelected()

            lrERDLink.Link.Origin.Tag.ResetAttributeCellColours()
            lrERDLink.Link.Destination.Tag.ResetAttributeCellColours()

            'CodeSafe: Exit the sub if the ERDLink has no RDSRelation
            '20200902-This obviously should not be the case, but it is a worse customer experience if null/nothing exceptions are thrown.
            If lrERDLink.Relation.RDSRelation Is Nothing Then Exit Sub

            '-----------------------------------------------------------------------------------
            'Highlight the Attributes of the Relation
            Dim lrAttribute As ERD.Attribute
            For Each lrOriginColumn In lrERDLink.Relation.RDSRelation.OriginColumns
                'CodeSafe: Remove Attributes with no columns
                Me.zrPage.ERDiagram.Attribute.RemoveAll(Function(x) x.Column Is Nothing)

                lrAttribute = Me.zrPage.ERDiagram.Attribute.Find(Function(x) x.Column.Id = lrOriginColumn.Id)
                lrAttribute.Cell.TextColor = Color.White
                lrAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)

                If lrERDLink.Relation.RelationFactType.Is1To1BinaryFactType Then
                    'Highlight the reverse Attributes
                    For Each lrOriginAttribute In lrAttribute.Entity.Attribute.FindAll(Function(x) x.PartOfPrimaryKey = True)
                        lrOriginAttribute.Cell.TextColor = Color.White
                        lrOriginAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightSteelBlue)
                    Next
                End If
            Next

            For Each lrDestinationColumn In lrERDLink.Relation.RDSRelation.DestinationColumns
                lrAttribute = Me.zrPage.ERDiagram.Attribute.Find(Function(x) x.Column.Id = lrDestinationColumn.Id)
                lrAttribute.Cell.TextColor = Color.White
                lrAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightGray)

                If lrERDLink.Relation.RelationFactType.Is1To1BinaryFactType Then
                    'Highlight the reverse Attributes
                    For Each lrDestinationAttribute In lrAttribute.Entity.Attribute.FindAll(Function(x) x.Column.Relation.Contains(lrERDLink.Relation.RDSRelation))
                        lrDestinationAttribute.Cell.TextColor = Color.White
                        lrDestinationAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.LightSteelBlue)
                    Next
                End If
            Next

            '---------------------------------------------------------------------------
            'If the ORM(FactType)ReadingEditor is loaded, then
            '  do the appropriate processing so that the data in the ReadingEditor grid
            '  matches the selected FactType (if a FactType is selected by the user)
            '---------------------------------------------------------------------------
            Dim lrORMReadingEditor As frmToolboxORMReadingEditor
            lrORMReadingEditor = prApplication.GetToolboxForm(frmToolboxORMReadingEditor.Name)

            If IsSomething(lrORMReadingEditor) Then

                Me.zrPage.SelectedObject.Clear()

                lrORMReadingEditor.zrPage = Me.zrPage
                lrORMReadingEditor.zrFactTypeInstance = lrERDLink.Relation.RelationFactType.CloneInstance(New FBM.Page(Me.zrPage.Model), False)
                Me.zrPage.SelectedObject.Add(lrORMReadingEditor.zrFactTypeInstance)
                Call lrORMReadingEditor.SetupForm()

            End If

            '-------------------------------------------------------
            'Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
            If IsSomething(lrToolboxForm) Then
                lrToolboxForm.zrModel = Me.zrPage.Model
                Call lrToolboxForm.VerbaliseRelation(lrERDLink.Relation.RDSRelation.ResponsibleFactType)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ToolStripMenuItemIsMandatory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemIsMandatory.Click


        Exit Sub

        'Dim lrAttribute As ERD.Attribute
        'Dim lbInitialMadatoryState As Boolean
        'Dim lsSQLQuery As String = ""

        'lrAttribute = Me.zrPage.SelectedObject(0)

        'lbInitialMadatoryState = lrAttribute.Mandatory

        'Me.ToolStripMenuItemIsMandatory.Checked = Not Me.ToolStripMenuItemIsMandatory.Checked

        'lrAttribute.Mandatory = Me.ToolStripMenuItemIsMandatory.Checked

        'If (lbInitialMadatoryState = False) And lrAttribute.Mandatory Then
        '    lsSQLQuery = "INSERT INTO IsMandatory"
        '    lsSQLQuery &= " (IsMandatory)"
        '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        '    lsSQLQuery &= " VALUES ("
        '    lsSQLQuery &= "'" & lrAttribute.FactDataInstance.Fact.Id & "'"
        '    lsSQLQuery &= " )"

        '    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        'Else
        '    lsSQLQuery = "DELETE FROM IsMandatory"
        '    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        '    lsSQLQuery &= " WHERE IsMandatory = '" & lrAttribute.FactDataInstance.Fact.Id & "'"

        '    Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        'End If

        'Call lrAttribute.RefreshShape()

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
            lsSQLQuery &= " WHERE Indexx = '" & lsIndexName & "'"
            lsSQLQuery &= " AND Property = '" & lrAttribute.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.ClassHasPreferredIndex.ToString
            'lsSQLQuery &= " WHERE Class = '" & lrAttribute.Entity.Id & "'"

            'Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
        End If

    End Sub

    Private Sub MoveUpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpToolStripMenuItem.Click


        Try
            Dim lrAttribute As ERD.Attribute
            Dim lsSQLQuery As String = ""

            lrAttribute = Me.zrPage.SelectedObject(0)

            If lrAttribute.OrdinalPosition = 1 Then
                '-----------------------------------------------------------------
                'Can't move up any further in the OrdinalPositions of Attributes
                '-----------------------------------------------------------------
                Exit Sub
            Else
                lrAttribute.OrdinalPosition -= 1

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
                lrChangingAttribute.OrdinalPosition += 1

                lrAttribute.Cell = lrEntity.TableShape.Item(0, lrAttribute.OrdinalPosition - 1)
                lrAttribute.Cell.Tag = lrAttribute
                lrChangingAttribute.Cell = lrEntity.TableShape.Item(0, lrChangingAttribute.OrdinalPosition - 1)
                lrChangingAttribute.Cell.Tag = lrChangingAttribute

                Call lrAttribute.RefreshShape()
                Call lrChangingAttribute.RefreshShape()

                lrEntity.Attribute.Insert(lrAttribute.OrdinalPosition - 1, lrAttribute)
                lrEntity.Attribute.RemoveAt(lrAttribute.OrdinalPosition + 1)

                Dim lrFact As FBM.Fact

                lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " WHERE Property = '" & lrAttribute.Id & "'"

                Dim lrRecordset As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrFact = lrRecordset.CurrentFact
                lrFact.GetFactDataByRoleName("Position").Data = lrAttribute.OrdinalPosition.ToString

                lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.Id & "'"

                lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrFact = lrRecordset.CurrentFact
                lrFact.GetFactDataByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

                '20200725-VM-Might still need this.
                'Call lrFactInstance.FactType.FactTable.ResortFactTable()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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
            lrAttribute.OrdinalPosition += 1

            '--------------------------------------------------------------------
            'Change the Ordinal Positions of the Attributes above the Attribute
            '--------------------------------------------------------------------
            Dim lrChangingAttribute As ERD.Attribute

            lrChangingAttribute = lrEntity.Attribute(lrAttribute.OrdinalPosition - 1)
            lrChangingAttribute.OrdinalPosition -= 1

            lrAttribute.Cell = lrEntity.TableShape.Item(0, lrAttribute.OrdinalPosition - 1)
            lrAttribute.Cell.Tag = lrAttribute
            lrChangingAttribute.Cell = lrEntity.TableShape.Item(0, lrChangingAttribute.OrdinalPosition - 1)
            lrChangingAttribute.Cell.Tag = lrChangingAttribute

            Call lrAttribute.RefreshShape()
            Call lrChangingAttribute.RefreshShape()

            lrEntity.Attribute.Insert(lrAttribute.OrdinalPosition, lrAttribute)
            lrEntity.Attribute.RemoveAt(lrAttribute.OrdinalPosition - 2)

            Dim lrFact As FBM.Fact

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " WHERE Property = '" & lrAttribute.Id & "'"

            Dim lrRecordset As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFact = lrRecordset.CurrentFact
            lrFact.GetFactDataByRoleName("Position").Data = lrAttribute.OrdinalPosition.ToString

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.Id & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFact = lrRecordset.CurrentFact
            lrFact.GetFactDataByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

            '20200725-VM-Might still need this.
            'Call lrFactInstance.FactType.FactTable.ResortFactTable()

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

        lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
        lsSQLQuery &= " WHERE Property = '" & lrAttribute.Name & "'"

        Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        lrAttribute.Entity.Attribute.Remove(lrAttribute)

        lrAttribute.Entity.TableShape.DeleteRow(lrAttribute.OrdinalPosition - 1)

        '----------------------------------------------------------------------------------
        'Reset the OrdinalPosition of the Attributes above the Attribute that was deleted
        '----------------------------------------------------------------------------------
        For liInd = lrAttribute.OrdinalPosition To lrAttribute.Entity.Attribute.Count
            lrChangingAttribute = lrAttribute.Entity.Attribute(liInd - 1)
            lrChangingAttribute.OrdinalPosition -= 1

            lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & lrChangingAttribute.FactDataInstance.Fact.Id & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            lrFactInstance = lrRecordset.CurrentFact
            lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = lrChangingAttribute.OrdinalPosition.ToString

            Call lrFactInstance.FactType.FactTable.ResortFactTable()
        Next

        '-------------------------------------------------
        'Remove the underlying FactType from the Model
        '-------------------------------------------------
        Dim lrModelObject As New FBM.ModelObject
        Dim lsRoleId As String = ""

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= "FROM CorePropertyIsForRole"
        lsSQLQuery &= " WHERE Property = '" & lrAttribute.Id & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        lsRoleId = lrRecordset("Role").Data

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= "FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
        lsSQLQuery &= " WHERE Property = '" & lrAttribute.Id & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        Dim lrFactType As New FBM.FactType(lrRecordset("FactType").Data, True)

        lrFactType = Me.zrPage.Model.FactType.Find(AddressOf lrFactType.Equals)

        If lrFactType.CanSafelyRemoveFromModel Then
            If lrFactType.IsBinaryFactType Then
                lrModelObject = lrFactType.GetOtherRoleOfBinaryFactType(lsRoleId).JoinedORMObject

                Call lrFactType.RemoveFromModel()

                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        lrModelObject.RemoveFromModel()
                    Case Is = pcenumConceptType.EntityType
                        lrModelObject.RemoveFromModel()
                    Case Is = pcenumConceptType.FactType
                End Select

            End If

        End If

    End Sub

    Private Sub Diagram_NodeDeselected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDeselected

        Dim lrEntity As ERD.Entity

        lrEntity = e.Node.Tag

        Call lrEntity.NodeDeselected()


    End Sub

    Private Sub ToolStripMenuItemEditRelation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemEditRelation.Click

        Dim lfrmEditRelationship As New frmCRUDEditRelationship

        If lfrmEditRelationship.ShowDialog = Windows.Forms.DialogResult.OK Then

        End If

    End Sub

    Private Sub RemoveFromPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromPageToolStripMenuItem.Click

        Dim lrTableNode As ERD.TableNode = Me.Diagram.Selection.Items(0)
        Dim lrEntity As New ERD.Entity
        Dim lsAttributeId As String = ""
        Dim lsIdentifierId As String = ""
        Dim lsSQLQuery As String = ""

        Me.Cursor = Cursors.WaitCursor

        Try
            ''---------------------------------------------------------
            ''Get the EntityType represented by the (selected) Entity
            ''---------------------------------------------------------
            lrEntity = lrTableNode.Tag '(above) = Me.Diagram.Selection.Items(0)

            'Relations
            Call Me.zrPage.removeRelationsForEntity(lrEntity)

            For Each lrERDAttribute In lrEntity.Attribute
                Call Me.zrPage.removeCMMLAttribute(lrERDAttribute)
            Next

            '-------------------------------------------------------------------------
            'Remove the Entity from the Page
            '---------------------------------
            lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE Element = '" & lrEntity.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim larLinkToRemove As New List(Of DiagramLink)
            For Each lrLink In lrTableNode.IncomingLinks
                larLinkToRemove.Add(lrLink)
            Next

            For Each lrLink In lrTableNode.OutgoingLinks
                larLinkToRemove.Add(lrLink)
            Next

            For Each lrLink In larLinkToRemove
                Me.Diagram.Links.Remove(lrLink)
            Next

            '----------------------------------------------------------------------------------------------------------
            'Remove the TableNode that represents the Entity from the Diagram on the Page.
            '-------------------------------------------------------------------------------
            Me.Diagram.Nodes.Remove(lrTableNode)
            Me.zrPage.ERDiagram.Entity.Remove(lrEntity)

            Me.Cursor = Cursors.Default

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            Me.Cursor = Cursors.Default

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub Diagram_LinkDeselected(sender As Object, e As LinkEventArgs) Handles Diagram.LinkDeselected

        Try
            Dim lrERDLink As ERD.Link

            If e.Link.GetType Is GetType(MindFusion.Diagramming.DiagramLink) Then Exit Sub

            lrERDLink = e.Link.Tag

            'CodeSafe: Exit the sub if the lrERDLink has no RDSRelation.
            '20200902:VM: This is obviously not a good situation, but it is a worse customer/user experience if NULL/Nothing exceptions are thrown.
            If lrERDLink.Relation.RDSRelation Is Nothing Then Exit Sub

            '-----------------------------------------------------------------------------------
            'Highlight the Attributes of the Relation
            Dim lrAttribute As ERD.Attribute
            For Each lrOriginColumn In lrERDLink.Relation.RDSRelation.OriginColumns
                lrAttribute = Me.zrPage.ERDiagram.Attribute.Find(Function(x) x.Column.Id = lrOriginColumn.Id)
                lrAttribute.Cell.TextColor = Color.Black
                lrAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
            Next

            For Each lrDestinationColumn In lrERDLink.Relation.RDSRelation.DestinationColumns
                lrAttribute = Me.zrPage.ERDiagram.Attribute.Find(Function(x) x.Column.Id = lrDestinationColumn.Id)
                lrAttribute.Cell.TextColor = Color.Black
                lrAttribute.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
            Next

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub DisplayDataIndexRelationInformationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DisplayDataIndexRelationInformationToolStripMenuItem.Click

        Dim lrTableNode As ERD.TableNode = Me.Diagram.Selection.Items(0)
        Dim lrEntity As New ERD.Entity

        '-------------------------
        'Get the selected Entity        
        lrEntity = lrTableNode.Tag '(above lrTableNode = Me.Diagram.Selection.Items(0) )

        Me.DisplayDataIndexRelationInformationToolStripMenuItem.Checked = Not Me.DisplayDataIndexRelationInformationToolStripMenuItem.Checked
        lrEntity.DisplayRDSData = Me.DisplayDataIndexRelationInformationToolStripMenuItem.Checked

        For Each lrAttribute In lrEntity.Attribute
            Call lrAttribute.RefreshShape()
        Next


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

    Private Sub DiagramView_KeyDown(sender As Object, e As KeyEventArgs) Handles DiagramView.KeyDown

        Select Case e.KeyCode
            Case Is = Keys.P
                Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                If IsSomething(lrPropertyGridForm) Then
                    If Me.Diagram.Selection.Items.Count > 0 Then
                        lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(Me.Diagram.Selection.Items.Count - 1).Tag
                    Else
                        'lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
                    End If
                End If
            Case Is = Keys.M
                Call frmMain.LoadToolboxModelDictionary()
            Case Is = Keys.T
                Call frmMain.LoadToolbox()
                Call Me.SetToolbox()
        End Select

        Call Me.DiagramView.Focus()
    End Sub
End Class