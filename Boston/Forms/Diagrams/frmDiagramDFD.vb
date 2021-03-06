Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout

Public Class frmDiagramDFD

    Public zrPage As FBM.Page '= Nothing
    Public zrCMMLPage As CMML.Page
    Public zoTreeNode As TreeNode = Nothing
    Public DataFlowModel As New tDataFlowModel

    Private zoContainerNode As ContainerNode

    Private MorphVector As New List(Of tMorphVector)
    Private morph_shape As New ShapeNode

    Private Sub frm_DataFlowDiagram_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call SetToolbox()

    End Sub

    Private Sub frm_DataFlowDiagram_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If IsSomething(Me.zrPage) Then
            If IsSomething(frmMain.zfrm_model_dictionary) Then
                Call frmMain.zfrm_model_dictionary.LoadToolboxModelDictionary(Me.zrPage.Language)
            End If
        End If

        Call SetToolbox()

        frmMain.ToolStripComboBox_zoom.Enabled = True

    End Sub

    Private Sub frmDiagramETD_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '-------------------------------------------
        'Process the page associated with the form.
        '-------------------------------------------
        If IsSomething(Me.zrPage) Then
            If Me.zrPage.IsDirty Then
                Select Case MsgBox("Changes have been made to the Page, '" & Me.zrPage.Name & "'. Would you like to save those changes?", MsgBoxStyle.YesNoCancel)
                    Case Is = MsgBoxResult.Yes
                        Me.zrPage.Save()
                    Case Is = MsgBoxResult.Cancel
                        e.Cancel = True
                        Exit Sub
                End Select
            End If
            Me.zrPage.Form = Nothing
            Me.zrPage.ReferencedForm = Nothing
        End If

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------        
        Me.zrPage.FormLoaded = False

        prRichmondApplication.WorkingModel = Nothing
        prRichmondApplication.WorkingPage = Nothing

        '------------------------------------------------
        'If the 'Properties' window is open, reset the
        '  SelectedObject
        '------------------------------------------------
        If Not IsNothing(frmMain.zfrm_properties) Then
            frmMain.zfrm_properties.PropertyGrid.SelectedObject = Nothing
        End If

        Me.Hide()

        frmMain.ToolStripButton_Save.Enabled = False

    End Sub

    Private Sub frm_DataFlowDiagram_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        Call SetToolbox()

    End Sub

    Private Sub frm_DataFlowDiagram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Sub SetupForm()

        Me.zoContainerNode = Diagram.Factory.CreateContainerNode(10, 10, 100, 100, False)

        Me.zoContainerNode.Tag = New tUseCaseSystemBoundary(pcenumConceptType.SystemBoundary)
        'lo_containernode.AllowIncomingLinks = False
        Me.zoContainerNode.AllowOutgoingLinks = False
        Me.zoContainerNode.Caption = "System Boundary"
        Me.zoContainerNode.ToolTip = "System Boundary"
        Me.zoContainerNode.Foldable = True
        Me.zoContainerNode.AutoShrink = True

    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Sub LoadDataFlowDiagramPage(ByRef arPage As FBM.Page)

        '------------------------------------------------------------------------
        'Loads the Data Flow Diagram for the given ORM (meta-model) Page
        '
        'PARAMETERS
        '  arPage:  The Page of the ORM (meta-) Model within which the Use Case Diagram is injected.
        '            The Richmond data model is an ORM meta-model. Use Case Diagrams are stored as 
        '            propositional functions within the ORM meta-model.
        '
        'PSEUDOCODE
        '        
        '  * 
        '------------------------------------------------------------------------
        Dim loDroppedNode As ShapeNode = Nothing
        Dim lrFactTypeInstance_pt As PointF = Nothing
        Dim lrFactInstance As New FBM.FactInstance
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactDataInstance As New FBM.FactDataInstance(arPage)
        Dim lr_actor_shape, lr_process_shape As New ShapeNode

        '-------------------------------------------------------
        'Set the Caption/Title of the Page to the PageName
        '-------------------------------------------------------
        Me.zrPage = arPage
        Me.TabText = arPage.Name

        '=========================================================================================
        'LinFu code
        Dim loDynamicPageObject As New LinFu.Reflection.DynamicObject

        loDynamicPageObject.MixWith(Me.zrPage)
        loDynamicPageObject.MixWith(New CMML.Page)

        Me.zrCMMLPage = loDynamicPageObject.CreateDuck(Of CMML.Page)()
        '=========================================================================================

        '--------------------------------------------------------------------
        'Display the DataFlowDiagram by logically associating Shape objects
        '   with the corresponding 'object' within the ORMModelPage object
        '--------------------------------------------------------------------

        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString)
        Me.DataFlowModel.ActorToProcessParticipationRelation = lrFactTypeInstance

        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ProcessToProcessRelation.ToString)
        Me.DataFlowModel.PocessToProcessRelation = lrFactTypeInstance

        Dim lrRecordset As New ORMQL.Recordset
        Dim lsSQLQuery As String = ""
        '================================
        'Draw the Processes on the Page
        '================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ElementHasElementType.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " WHERE ElementType = 'Process'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        If lrRecordset.Facts.Count > 0 Then

            For Each lrFactInstance In lrRecordset.Facts

                Dim lrProcess As New CMML.Process(Me.zrPage, System.Guid.NewGuid.ToString)

                lrProcess = lrFactInstance.GetFactDataInstanceByRoleName("Element").CloneProcess(Me.zrPage)
                lrProcess.Shape.Resize(20, 20)
                lrProcess.DisplayAndAssociate(Me.zoContainerNode)

                Me.DataFlowModel.Process.Add(lrProcess)
            Next

        End If

        '==================================
        'Draw the links between Processes
        '==================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        While Not lrRecordset.EOF

            Dim lrProcess1 As New CMML.Process(Me.zrPage, lrRecordset("Process1").Data)
            lrProcess1.FactData.Model = Me.zrPage.Model
            Dim lrProcess2 As New CMML.Process(Me.zrPage, lrRecordset("Process2").Data)
            lrProcess2.FactData.Model = Me.zrPage.Model

            lrProcess1 = Me.DataFlowModel.Process.Find(AddressOf lrProcess1.EqualsByName)
            lrProcess2 = Me.DataFlowModel.Process.Find(AddressOf lrProcess2.EqualsByName)

            Dim lrLink As New CMML.Link(Me.zrPage, lrRecordset.CurrentFact, lrProcess1, lrProcess2, lrRecordset("Data").Data)
            lrLink.DisplayAndAssociate()

            lrRecordset.MoveNext()
        End While

        '=================================
        'Draw the DataStores on the Page
        '=================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ElementHasElementType.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " WHERE ElementType = 'DataStore'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        While Not lrRecordset.EOF

            Dim lrDataStore As New DFD.DataStore
            lrFactDataInstance = New FBM.FactDataInstance
            lrFactDataInstance = lrRecordset("Element")
            lrDataStore = lrFactDataInstance.CloneDataStore(Me.zrPage)

            '--------------------------------------------------
            'Create a ShapeNode on the Page for the DataStore
            '--------------------------------------------------
            lrDataStore.DisplayAndAssociate()

            Me.DataFlowModel.DataStore.Add(lrDataStore)
            Me.zoContainerNode.Add(lrDataStore.Shape)

            lrRecordset.MoveNext()
        End While


        '=============================
        'Load the Actors on the Page
        '=============================

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ElementHasElementType.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSQLQuery &= " WHERE ElementType = 'Actor'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        While Not lrRecordset.EOF

            Dim lrActor As New CMML.tActor(Me.zrPage)

            lrFactDataInstance = New FBM.FactDataInstance
            lrFactDataInstance = lrRecordset("Element")
            lrActor = lrFactDataInstance.CloneActor(Me.zrPage)

            lrActor.DisplayAndAssociate()

            Me.DataFlowModel.Actor.Add(lrActor)


            lrRecordset.MoveNext()
        End While

        '===============================================
        'Link the Actors to their respective Processes
        '===============================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        While Not lrRecordset.EOF

            ''------------------------------------------
            ''Link the Actor to the associated Process
            ''------------------------------------------
            Dim lrActor As New CMML.tActor(Me.zrPage)
            Dim lrProcess As New CMML.Process(Me.zrPage, lrRecordset("Process").Data)

            lrActor.Name = lrRecordset("Actor").Data

            lrActor = Me.DataFlowModel.Actor.Find(AddressOf lrActor.EqualsByName)
            lrProcess = Me.DataFlowModel.Process.Find(AddressOf lrProcess.EqualsByName)

            Dim lrRecordset1 As ORMQL.Recordset

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.ActorIsReceivingParty.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE ActorIsReceivingParty = '" & lrRecordset.CurrentFact.Id & "'"

            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrLink As New CMML.Link
            'Dim lo_link As DiagramLink
            If lrRecordset1.Facts.Count > 0 Then
                '--------------------------
                'Actor is receiving party
                '--------------------------
                'lo_link = Me.Diagram.Factory.CreateDiagramLink(lrProcess.shape, lrActor.shape)

                lrLink = New CMML.Link(Me.zrPage, lrRecordset.CurrentFact, lrProcess, lrActor)
                lrLink.DisplayAndAssociate()
            Else
                'lo_link = Me.Diagram.Factory.CreateDiagramLink(lrActor.shape, lrProcess.shape)
                lrLink = New CMML.Link(Me.zrPage, lrRecordset.CurrentFact, lrActor, lrProcess, lrRecordset("Data").Data)
                lrLink.DisplayAndAssociate()
            End If

            lrRecordset.MoveNext()
        End While

        '====================================================
        'Link the Processes to the DataStores they write to
        '====================================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessWritesToDataStore.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        While Not lrRecordset.EOF

            '----------------------------------------------
            'Link the Process to the associated DataStore
            '----------------------------------------------
            Dim lrProcess As New CMML.Process(Me.zrPage, lrRecordset("Process").Data)
            Dim lrDataStore As New DFD.DataStore(Me.zrPage, lrRecordset("DataStore").Data)

            lrProcess = Me.DataFlowModel.Process.Find(AddressOf lrProcess.EqualsByName)
            lrDataStore = Me.DataFlowModel.DataStore.Find(AddressOf lrDataStore.EqualsByName)

            Dim lrLink As New CMML.Link(Me.zrPage, lrRecordset.CurrentFact, lrProcess, lrDataStore)
            lrLink.DisplayAndAssociate()

            'Dim lo_link As DiagramLink
            'lo_link = Me.Diagram.Factory.CreateDiagramLink(lrProcess.shape, lrDataStore.shape)
            'lo_link.Style = LinkStyle.Bezier
            'lo_link.Tag = lrFactInstance


            lrRecordset.MoveNext()
        End While

        '=====================================================
        'Link the Processes to the DataStores they read from
        '=====================================================
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessReadsFromDataStore.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        While Not lrRecordset.EOF

            '----------------------------------------------
            'Link the Process to the associated DataStore
            '----------------------------------------------
            Dim lrProcess As New CMML.Process(Me.zrPage, lrRecordset("Process").Data)
            Dim lrDataStore As New DFD.DataStore(Me.zrPage, lrRecordset("DataStore").Data)

            lrProcess = Me.DataFlowModel.Process.Find(AddressOf lrProcess.EqualsByName)
            lrDataStore = Me.DataFlowModel.DataStore.Find(AddressOf lrDataStore.EqualsByName)


            Dim lo_link As DiagramLink
            lo_link = Me.Diagram.Factory.CreateDiagramLink(lrDataStore.Shape, lrProcess.Shape)
            lo_link.Style = LinkStyle.Bezier
            lo_link.Tag = lrFactInstance


            lrRecordset.MoveNext()
        End While

        Me.Diagram.RoutingOptions.LengthCost = 90
        Me.Diagram.RoutingOptions.NodeVicinityCost = 90
        Me.Diagram.RoutingOptions.TurnCost = 99
        'Me.Diagram.RouteAllLinks()

        Me.Diagram.Invalidate()
        Me.zrPage.FormLoaded = True

        Dim liInd As Integer
        For liInd = 1 To Diagram.Nodes.Count
            'MsgBox(Diagram.Nodes(liInd - 1).Tag.ConceptType.ToString)
        Next
        Me.zoContainerNode.ZBottom()

        Me.Diagram.HitTestPriority = HitTestPriority.ZOrder

        Call Me.ResetNodeAndLinkColours()

    End Sub

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.DFDShapeLibrary)
        Dim lo_shape As Shape
        Dim lrToolboxForm As frmToolbox
        lrToolboxForm = prRichmondApplication.GetToolboxForm(frmToolbox.Name)

        If IsSomething(lrToolboxForm) Then
            lrToolboxForm.ShapeListBox.Shapes = lsl_shape_library.Shapes

            For Each lo_shape In lrToolboxForm.ShapeListBox.Shapes
                Select Case lo_shape.DisplayName
                    Case Is = "Actor"
                        lo_shape.Image = My.Resources.ActorSmall
                    Case Is = "Data Store"
                        lo_shape.Image = My.Resources.DataStore
                End Select
            Next


        End If

    End Sub

    Private Sub ETDDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        '---------------------------------------------------------------
        'Update the Working Environment within the Main form so that
        '  the User can see where they are working/viewing.
        '---------------------------------------------------------------
        Dim lr_working_environment As New tWorkingEnvironment

        'lr_working_environment.EnterpriseId = prApplication.WorkingEnterpriseId
        lr_working_environment.ProjectId = prApplication.WorkingProject.Id
        'lr_working_environment.SolutionId = prApplication.WorkingSolutionId
        'lr_working_environment.SubjectAreaId = prApplication.WorkingSubjectAreaId
        lr_working_environment.Model = Me.zrPage.Model
        lr_working_environment.Page = Me.zrPage

        prApplication.ChangeWorkingEnvironment(lr_working_environment)
        'frmMain.UpdateWorkingEnterpriseName()

        Call SetToolbox()

        'Me.UseCaseDiagramView.Behavior = Behavior.Modify
        Me.Diagram.Invalidate()
    End Sub

    Private Sub DFDDiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim liCount As Integer
        Dim liInd As Integer = 0

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            '------------------------------------------------------------------------------------------------------------------------------------
            'Make sure the current page points to the Diagram on this form. The reason is that the user may be viewing the Page as an ORM Model
            '------------------------------------------------------------------------------------------------------------------------------------
            Me.zrPage.Diagram = Me.Diagram

            Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            If loDraggedNode.Index >= 0 Then

                Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prRichmondApplication.GetToolboxForm(frmToolbox.Name)

                If loDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount Then
                    Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
                    Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

                    Select Case lrToolboxForm.ShapeListBox.Shapes(loDraggedNode.Index).Id
                        Case Is = "Actor"
                            '---------------------------------------------------------------
                            'Establish a new FBM Actor(EntityType) for the dropped object.
                            '  i.e. Establish the EntityType within the Model as well
                            '  as creating a new object for the Actor.
                            '---------------------------------------------------------------
                            '=========================================================================================
                            'LinFu code
                            Dim loDynamicObject As New LinFu.Reflection.DynamicObject

                            loDynamicObject.MixWith(Me.zrPage.Model)
                            loDynamicObject.MixWith(New CMML.Model)

                            Dim lrCMMLModel As CMML.Model = loDynamicObject.CreateDuck(Of CMML.Model)()

                            Call lrCMMLModel.CreateActor()
                            '=========================================================================================

                            Dim lrActor As New CMML.tActor(Me.zrPage)

                            'lrActor = prRichmondApplication.CMML.CreateActor(Me.zrPage.Model, "New Actor")

                            liCount = Me.zrPage.Model.EntityType.FindAll(AddressOf lrActor.EqualsByNameLike).Count
                            lrActor.Name &= " " & (CStr(liCount) + 1)

                            '=========================================================================================
                            'LinFu code
                            Dim loDynamicPageObject As New LinFu.Reflection.DynamicObject

                            loDynamicPageObject.MixWith(Me.zrPage)
                            loDynamicPageObject.MixWith(New CMML.Page)

                            Dim lrCMMLPage As CMML.Page = loDynamicPageObject.CreateDuck(Of CMML.Page)()

                            Call lrCMMLPage.DropActorAtPoint(lrActor, loPointF)
                            '=========================================================================================

                            If Not Me.DataFlowModel.Actor.Exists(AddressOf lrActor.Equals) Then
                                '--------------------------------------------------
                                'The EntityType is not already within the ORMModel
                                '  so add it.
                                '--------------------------------------------------
                                Me.DataFlowModel.Actor.Add(lrActor)
                            End If

                        Case Is = "Process"

                            Dim lrProcess As New CMML.Process(Me.zrPage, "NewProcess")

                            '=========================================================================================
                            'LinFu code, Model level.
                            Dim loDynamicObject As New LinFu.Reflection.DynamicObject

                            loDynamicObject.MixWith(Me.zrPage.Model)
                            loDynamicObject.MixWith(New CMML.Model)

                            Dim lrCMMLModel As CMML.Model = loDynamicObject.CreateDuck(Of CMML.Model)()

                            lrProcess.Name = lrCMMLModel.CreateUniqueProcessName(lrProcess)
                            '=========================================================================================

                            '=========================================================================================
                            'LinFu code, Page level
                            loDynamicObject = New LinFu.Reflection.DynamicObject

                            loDynamicObject.MixWith(Me.zrPage)
                            loDynamicObject.MixWith(New CMML.Page)

                            Dim lrCMMLPage As CMML.Page = loDynamicObject.CreateDuck(Of CMML.Page)()

                            Call lrCMMLPage.DropProcessAtPoint(lrProcess, loPointF, Me.zoContainerNode)
                            '=========================================================================================

                        Case Is = "Data Store"

                            Dim lrDataStore As New DFD.DataStore(Me.zrPage, "NewDataStore")

                            '=========================================================================================
                            'LinFu code, Model level.
                            Dim loDynamicObject As New LinFu.Reflection.DynamicObject

                            loDynamicObject.MixWith(Me.zrPage.Model)
                            loDynamicObject.MixWith(New CMML.Model)

                            Dim lrCMMLModel As CMML.Model = loDynamicObject.CreateDuck(Of CMML.Model)()

                            lrDataStore.Name = lrCMMLModel.CreateUniqueDataStoreName(lrDataStore)
                            '=========================================================================================

                            '=========================================================================================
                            'LinFu code, Page level.
                            loDynamicObject = New LinFu.Reflection.DynamicObject

                            loDynamicObject.MixWith(Me.zrPage)
                            loDynamicObject.MixWith(New CMML.Page)

                            Dim lrCMMLPage As CMML.Page = loDynamicObject.CreateDuck(Of CMML.Page)()

                            Call lrCMMLPage.DropDataStoreAtPoint(lrDataStore, loPointF, Me.zoContainerNode)
                            '=========================================================================================

                    End Select
                End If
            End If
        End If

    End Sub

    Private Sub ETDDiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragOver

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
                lrToolboxForm = prRichmondApplication.GetToolboxForm(frmToolbox.Name)
                If (lrDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount) Then
                    Select Case lrDraggedNode.Tag.Id
                        Case Is = "Actor"
                            e.Effect = DragDropEffects.Copy
                        Case Is = "Process"
                            e.Effect = DragDropEffects.Copy
                        Case Is = "Data Store"
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
        ElseIf e.Data.GetDataPresent(GetType(CMML.tActor).ToString, False) Then
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent(GetType(CMML.Process).ToString, False) Then
            e.Effect = DragDropEffects.Copy
        ElseIf e.Data.GetDataPresent(GetType(DFD.DataStore).ToString, False) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub ETDDiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub ETDDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF

        'prRichmondApplication.WorkingPage = Me.zrPage

        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        If IsSomething(Diagram.GetLinkAt(lo_point, 1)) Then
            '    Diagram.GetLinkAt(lo_point, 1).Selected = True
            '    Exit Sub
        ElseIf IsSomething(Diagram.GetNodeAt(lo_point)) Then

            '    Dim lrModelObject As FBM.ModelObject
            '    lrModelObject = Diagram.GetNodeAt(lo_point).Tag
            '    '---------------------------
            '    'Clear the SelectedObjects
            '    '---------------------------
            '    Me.zrPage.SelectedObject.Clear()
            '    Me.zrPage.SelectedObject.Add(lrModelObject)
        Else
            '    '---------------------------------------------------
            '    'MouseDown is on canvas (not on object).
            '    'If any objects are already highlighted as blue, 
            '    '  then change the outline to black/originalcolour
            '    '---------------------------------------------------

            '    '---------------------------
            '    'Clear the SelectedObjects
            '    '---------------------------
            '    Me.zrPage.SelectedObject.Clear()
            '    'Me.Diagram.Selection.Clear()
            Call Me.ResetNodeAndLinkColours()

            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram


            '    '-----------------------
            '    'Set the Property Grid
            '    '-----------------------
            '    Dim lrPropertyGridForm As frmToolboxProperties

            '    lrPropertyGridForm = prRichmondApplication.GetToolboxForm(frmToolboxProperties.Name)
            '    If IsSomething(lrPropertyGridForm) Then

            '        Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
            '        ' And you pass it to the PropertyGrid,
            '        ' via its BrowsableAttributes property :
            '        lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})

            '        lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

            '    End If
        End If

    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call SetToolbox()

    End Sub

    Public Sub MorphToORMDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If prRichmondApplication.WorkingPage.SelectedObject.Count = 0 Then Exit Sub
        If prRichmondApplication.WorkingPage.SelectedObject.Count > 1 Then Exit Sub

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ETDDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        'Me.HiddenDiagramView.PasteFromClipboard(0, 0)

        Dim lr_actor As New CMML.tActor
        lr_actor = prRichmondApplication.WorkingPage.SelectedObject(0)

        Dim lr_shape_node As ShapeNode


        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = item.Tag
            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            prRichmondApplication.WorkingPage = lrEnterpriseView.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lrEnterpriseView.Tag.Model)
            lr_page = lrEnterpriseView.Tag
            Dim lrEntityTypeInstanceList = From EntityTypeInstance In lr_page.EntityTypeInstance
                                           Where EntityTypeInstance.Id = lr_actor.Data
                                           Select New FBM.EntityTypeInstance(lr_page.Model,
                                                                    pcenumLanguage.ORMModel,
                                                                    EntityTypeInstance.Name,
                                                                    True,
                                                                    EntityTypeInstance.X,
                                                                    EntityTypeInstance.Y)

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
            For Each lrEntityTypeInstance In lrEntityTypeInstanceList
                Exit For
            Next

            '----------------------------------------------------------------
            'Retreive the actual EntityTypeInstance on the destination page
            '----------------------------------------------------------------
            lrEntityTypeInstance = lr_page.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

            If lr_page.FormLoaded Then
                lr_shape_node = lrEntityTypeInstance.Shape.Clone(True)
                Me.MorphVector(0).Shape = lr_shape_node
            Else
                Me.MorphVector(0).Shape = lr_actor.Shape.Clone(True)
                Me.MorphVector(0).Shape.Shape = Shapes.RoundRect
                Me.MorphVector(0).Shape.HandlesStyle = HandlesStyle.InvisibleMove
                Me.MorphVector(0).Shape.Text = lr_actor.Data
                Me.MorphVector(0).Shape.Resize(20, 15)
            End If

            Me.HiddenDiagram.Nodes.Add(Me.MorphVector(0).Shape)
            Me.HiddenDiagram.Invalidate()

            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X, Me.MorphVector(0).StartPoint.Y, lrEntityTypeInstance.X, lrEntityTypeInstance.Y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Public Sub MorphToUseCaseDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ETDDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Process/Entity to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape

        '--------------------------------------------------
        'Retrieve the Actor or Process from the Shape.Tag
        '--------------------------------------------------
        Dim lrEntity As New ERD.Entity
        Dim lrActor As New CMML.tActor
        Dim lrProcess As New CMML.Process
        Dim liConceptType As pcenumConceptType
        lrEntity = lr_shape_node.Tag

        Select Case lr_shape_node.Tag.ConceptType
            Case Is = pcenumConceptType.Actor
                lrActor = lrEntity.CloneActor(Me.zrPage)
                liConceptType = lrActor.ConceptType
            Case Is = pcenumConceptType.Process
                lrProcess = lrEntity.CloneProcess(Me.zrPage)
                liConceptType = lrProcess.ConceptType
        End Select

        lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        '--------------------------------------------
        'Base the morphing shape on the ConceptType
        '  of the model element being morphed.
        '--------------------------------------------
        Select Case liConceptType
            Case pcenumConceptType.Actor
                lr_shape_node.Shape = Shapes.Rectangle
                lr_shape_node.Text = ""
                lr_shape_node.Pen.Color = Color.White
                lr_shape_node.Transparent = True
                lr_shape_node.Image = My.Resources.resource_file.actor
            Case pcenumConceptType.Process
                lr_shape_node.Shape = Shapes.Ellipse
        End Select

        lr_shape_node.Pen.Color = Color.Black
        lr_shape_node.Text = Me.zrPage.SelectedObject(0).Name
        lr_shape_node.Visible = True

        Me.morph_shape = lr_shape_node

        Me.HiddenDiagram.Invalidate()

        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrFactDataInstance As New Object

            Select Case liConceptType
                Case Is = pcenumConceptType.Actor
                    Dim lrActorList = From FactType In lr_page.FactTypeInstance
                                      From Fact In FactType.Fact
                                      From RoleData In Fact.Data
                                      Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString
                                      Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrActorList
                        Exit For
                    Next
                Case Is = pcenumConceptType.Process
                    Dim lrProcessList = From FactType In lr_page.FactTypeInstance
                                        From Fact In FactType.Fact
                                        From RoleData In Fact.Data
                                        Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Process.ToString
                                        Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrProcessList
                        Exit For
                    Next
            End Select

            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X, Me.MorphVector(0).StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Private Sub ContextMenuStrip_Process_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Process.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lrModel As FBM.Model
        Dim lrProcess As New CMML.Process


        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '=======================================
        'Check that selected object is Process
        '=======================================
        If lrProcess.GetType Is Me.zrPage.SelectedObject(0).GetType Then
            '----------
            'All good
            '----------
        Else
            '---------------------------------------------------------------------------------------------------------
            'Sometimes the MouseDown/NodeSelected gets it wrong and this sub receives invocation before a Process is 
            '  properly selected. The user may try and click again.
            '  If it's a bug, then this can be removed obviously.
            '---------------------------------------------------------------------------------------------------------
            Exit Sub
        End If

        lrProcess = Me.zrPage.SelectedObject(0)
        lrModel = lrProcess.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.MorphVector.Clear()
        Me.MorphVector.Add(New tMorphVector(lrProcess.X, lrProcess.Y, 0, 0, 40))

        '===============================================================================
        'Load the UseCaseDiagrams that relate to the Process as selectable menuOptions
        '===============================================================================
        Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Clear()

        larPage_list = prRichmondApplication.CMML.get_use_case_diagram_pages_for_process(lrProcess)

        For Each lr_page In larPage_list
            Dim loMenuOption As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            loMenuOption = Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUseCaseDiagram,
                                                       lr_page,
                                                       lr_page.Model.EnterpriseId,
                                                       lr_page.Model.SubjectAreaId,
                                                       lr_page.Model.ProjectId,
                                                       lr_page.Model.SolutionId,
                                                       lr_page.Model.ModelId,
                                                       pcenumLanguage.UseCaseDiagram,
                                                       Nothing, lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.MorphToUseCaseDiagram
        Next

        '================================================================================
        'Load the Flow Chart Pages that relate to the Process as selectable menuOptions
        '================================================================================
        Me.ToolStripMenuItemFlowChart.DropDownItems.Clear()

        larPage_list = prRichmondApplication.CMML.GetFlowChartDiagramPagesForProcess(lrProcess)

        For Each lr_page In larPage_list
            Dim loMenuOption As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            loMenuOption = Me.ToolStripMenuItemFlowChart.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageFlowChart,
                                                       lr_page,
                                                       lr_page.Model.EnterpriseId,
                                                       lr_page.Model.SubjectAreaId,
                                                       lr_page.Model.ProjectId,
                                                       lr_page.Model.SolutionId,
                                                       lr_page.Model.ModelId,
                                                       pcenumLanguage.FlowChart,
                                                       Nothing, lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.MorphToProcessBasedDiagram
        Next

        '=========================================================================================
        'Load the Event Tract Diagram Pages that relate to the Process as selectable menuOptions
        '=========================================================================================
        Me.ToolStripMenuItemEventTraceDiagram.DropDownItems.Clear()

        larPage_list = prRichmondApplication.CMML.GetEventTractDiagramPagesForProcess(lrProcess)

        For Each lr_page In larPage_list
            Dim loMenuOption As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            loMenuOption = Me.ToolStripMenuItemEventTraceDiagram.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageEventTraceDiagram,
                                                       lr_page,
                                                       lr_page.Model.EnterpriseId,
                                                       lr_page.Model.SubjectAreaId,
                                                       lr_page.Model.ProjectId,
                                                       lr_page.Model.SolutionId,
                                                       lr_page.Model.ModelId,
                                                       pcenumLanguage.EventTraceDiagram,
                                                       Nothing, lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.MorphToProcessBasedDiagram
        Next

    End Sub

    Public Sub MorphToEventTraceDiagram(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Public Sub MorphToProcessBasedDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrFactDataInstance As New Object
        Dim lrMenuItem As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ORMDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lrShapeNode As ShapeNode
        lrShapeNode = Me.zrPage.SelectedObject(0).Shape
        lrShapeNode = Me.HiddenDiagram.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
        lrShapeNode.Shape = Shapes.RoundRect
        lrShapeNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
        lrShapeNode.Text = Me.zrPage.SelectedObject(0).Name
        lrShapeNode.Pen.Color = Color.Black
        lrShapeNode.Visible = True

        Me.MorphVector(0).Shape = lrShapeNode

        Me.HiddenDiagram.Invalidate()

        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = lrMenuItem.Tag
            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            prRichmondApplication.WorkingPage = lrEnterpriseView.Tag

            '---------------------------------------------
            'Set the Page that is going to be morphed to
            '---------------------------------------------
            Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
            lrPage = lrEnterpriseView.Tag

            '----------------------------------------------------------------------
            'Populate the MorphVector with each Process Shape on the current Page
            '  that is also on the destination Page.
            '----------------------------------------------------------------------
            Dim lrAdditionalProcess As CMML.Process
            For Each lrAdditionalProcess In Me.DataFlowModel.Process
                If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
                    '---------------------------------------------------------------------------------------------
                    'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                    '---------------------------------------------------------------------------------------------
                Else
                    Dim lrEntityList = From FactTypeInstance In lrPage.FactTypeInstance
                                       From Fact In FactTypeInstance.Fact
                                       From FactData In Fact.Data
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrAdditionalProcess.Name
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                    Me.MorphVector.Add(New tMorphVector(lrAdditionalProcess.X, lrAdditionalProcess.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40))

                    lrShapeNode = lrAdditionalProcess.Shape
                    lrShapeNode = Me.HiddenDiagram.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
                    lrShapeNode.Shape = Shapes.Ellipse
                    lrShapeNode.Text = lrAdditionalProcess.Name
                    lrShapeNode.Visible = True
                    Me.HiddenDiagram.Nodes.Add(lrShapeNode)

                    Me.MorphVector(Me.MorphVector.Count - 1).Shape = lrShapeNode

                End If
            Next

            '----------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '----------------------------------------------------------------
            Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance
                                      From Fact In FactTypeInstance.Fact
                                      From FactData In Fact.Data
                                      Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                      And FactData.Role.Name = "Element" _
                                      And FactData.Concept.Symbol = Me.zrPage.SelectedObject(0).Name
                                      Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)


            For Each lrFactDataInstance In larFactDataInstance
                Exit For
            Next

            Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.x, lrFactDataInstance.y)
            Me.MorphVector(0).VectorSteps = 40
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If


    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Dim lsSQLQuery As String = ""

        e.Link.SnapToNodeBorder = True

        If Me.LinkRepresentsActorProcessRelation(e.Link) Then
            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
            lsSQLQuery &= " (Actor, Process, Data)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            If e.Link.Origin.Tag.ConceptType = pcenumConceptType.Actor Then
                lsSQLQuery &= "'" & e.Link.Origin.Tag.Name & "'"
                lsSQLQuery &= ",'" & e.Link.Destination.Tag.Name & "'"
            Else
                lsSQLQuery &= "'" & e.Link.Destination.Tag.Name & "'"
                lsSQLQuery &= ",'" & e.Link.Origin.Tag.Name & "'"
            End If
            lsSQLQuery &= ",'None'"
            lsSQLQuery &= ")"

            Dim lrFact As New FBM.Fact
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrLink As New CMML.Link(Me.zrPage, lrFact, e.Link.Origin.Tag, e.Link.Destination.Tag, "None", e.Link)

            If e.Link.Origin.Tag.ConceptType = pcenumConceptType.Actor Then
                '------------------------
                'Actor is sending party
                '------------------------
                Dim lrCMMLProcess As CMML.Process
                lrCMMLProcess = e.Link.Destination.Tag
                Call lrCMMLProcess.SetSizeProportionalToInputs(Me.zoContainerNode)
            Else
                lsSQLQuery = "INSERT INTO ActorIsReceivingParty" '& pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
                lsSQLQuery &= " (ActorIsReceivingParty)"
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & lrFact.Id & "'"
                lsSQLQuery &= ")"

                lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If
        ElseIf Me.LinkRepresentsProcessProcessRelation(e.Link) Then

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
            lsSQLQuery &= " (Process1, Process2, Data)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & e.Link.Origin.Tag.Name & "'"
            lsSQLQuery &= ",'" & e.Link.Destination.Tag.Name & "'"
            lsSQLQuery &= ",'None'"
            lsSQLQuery &= ")"

            Dim lrFact As New FBM.Fact
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrLink As New CMML.Link(Me.zrPage, lrFact, e.Link.Origin.Tag, e.Link.Destination.Tag, "None", e.Link)

            Dim lrCMMLProcess As CMML.Process
            lrCMMLProcess = e.Link.Destination.Tag
            Call lrCMMLProcess.SetSizeProportionalToInputs()

        ElseIf Me.LinkRepresentsProcessToDataStoreRelation(e.Link) Then

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.ProcessWritesToDataStore.ToString
            lsSQLQuery &= " (Process, DataStore)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & e.Link.Origin.Tag.Name & "'"
            lsSQLQuery &= ",'" & e.Link.Destination.Tag.Name & "'"
            lsSQLQuery &= ")"

            Dim lrFact As New FBM.Fact
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrLink As New CMML.Link(Me.zrPage, lrFact, e.Link.Origin.Tag, e.Link.Destination.Tag, e.Link.Destination.Tag.Name, e.Link)


        ElseIf Me.LinkRepresentsProcessReadsFromDataStoreRelation(e.Link) Then

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.ProcessReadsFromDataStore.ToString
            lsSQLQuery &= " (DataStore, Process)"
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & e.Link.Origin.Tag.Name & "'"
            lsSQLQuery &= ",'" & e.Link.Destination.Tag.Name & "'"

            lsSQLQuery &= ")"

            Dim lrFact As New FBM.Fact
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrLink As New CMML.Link(Me.zrPage, lrFact, e.Link.Origin.Tag, e.Link.Destination.Tag, e.Link.Origin.Tag.Name, e.Link)

            Dim lrCMMLProcess As CMML.Process
            lrCMMLProcess = e.Link.Destination.Tag
            Call lrCMMLProcess.SetSizeProportionalToInputs()

        Else
            Me.Diagram.Links.Remove(e.Link)
        End If
        'Me.Diagram.RouteAllLinks()

        Call Me.ResetNodeAndLinkColours()

    End Sub

    Private Function LinkRepresentsActorProcessRelation(ByVal arLink As DiagramLink) As Boolean

        If arLink.Origin.Tag.ConceptType = pcenumConceptType.Actor And
           arLink.Destination.Tag.ConceptType = pcenumConceptType.Process Then
            Return True
        ElseIf arLink.Origin.Tag.ConceptType = pcenumConceptType.Process And
               arLink.Destination.Tag.ConceptType = pcenumConceptType.Actor Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function LinkRepresentsProcessProcessRelation(ByVal arLink As DiagramLink) As Boolean

        If arLink.Origin.Tag.ConceptType = pcenumConceptType.Process And
           arLink.Destination.Tag.ConceptType = pcenumConceptType.Process Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function LinkRepresentsProcessToDataStoreRelation(ByVal arLink As DiagramLink) As Boolean

        If arLink.Origin.Tag.ConceptType = pcenumConceptType.Process And
           arLink.Destination.Tag.ConceptType = pcenumConceptType.DataStore Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function LinkRepresentsProcessReadsFromDataStoreRelation(ByVal arLink As DiagramLink) As Boolean

        If arLink.Origin.Tag.ConceptType = pcenumConceptType.DataStore And
           arLink.Destination.Tag.ConceptType = pcenumConceptType.Process Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub Diagram_LinkDeleted(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkDeleted

        Dim lsSQLQuery As String = ""
        Dim lrLink As CMML.Link


        lrLink = e.Link.Tag

        '--------------------------------------------------------------------------------
        'Delete the link from the respective FactType in the DataFlowDiagram metamodel.
        '--------------------------------------------------------------------------------
        If Me.LinkRepresentsActorProcessRelation(e.Link) Then

            lsSQLQuery = "DELETE FACT '" & lrLink.Fact.Id & "'"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If e.Link.Origin.Tag.ConceptType = pcenumConceptType.Actor Then
                '------------------------
                'Actor is sending party
                '------------------------
            Else
                lsSQLQuery = "DELETE FROM ActorIsReceivingParty" '& pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE ActorIsReceivingParty = '" & lrLink.Fact.Id & "'"

                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If
        ElseIf Me.LinkRepresentsProcessProcessRelation(e.Link) Then

            lsSQLQuery = "DELETE FACT '" & lrLink.Fact.Id & "'"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        ElseIf Me.LinkRepresentsProcessReadsFromDataStoreRelation(e.Link) Then

            lsSQLQuery = "DELETE FACT '" & lrLink.Fact.Id & "'"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessReadsFromDataStore.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        ElseIf Me.LinkRepresentsProcessToDataStoreRelation(e.Link) Then

            lsSQLQuery = "DELETE FACT '" & lrLink.Fact.Id & "'"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.ProcessWritesToDataStore.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End If

    End Sub

    Private Sub Diagram_LinkDeselected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkDeselected

        Dim lrLink As CMML.Link

        lrLink = e.Link.Tag

        Call lrLink.LinkDeselected()

    End Sub

    Private Sub Diagram_LinkModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkModified

        e.Link.AutoRoute = False
        e.Link.Style = LinkStyle.Bezier
        e.Link.SegmentCount = 1

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Dim lrLink As CMML.Link

        lrLink = e.Link.Tag

        Call lrLink.LinkSelected()

        Call Me.zrPage.SelectedObject.Clear()
        Me.zrPage.SelectedObject.Add(lrLink)

        '--------------------------------------
        'Set the PropertiesGrid.SeletedObject
        '--------------------------------------
        Dim lrPropertyGridForm As frmToolboxProperties

        lrPropertyGridForm = prRichmondApplication.GetToolboxForm(frmToolboxProperties.Name)
        If IsSomething(lrPropertyGridForm) Then

            '----------------------------------------------------
            'Get the DataStore values for the SentData property
            '----------------------------------------------------
            ReDim tGlobalForTypeConverter.DataStores(Me.zrCMMLPage.GetModelDataStores.Count)
            tGlobalForTypeConverter.DataStores = Me.zrCMMLPage.GetModelDataStores.ToArray

            '--------------------------------------------------------------------
            'Filter out the unwanted Properties/Attributes for the PropertyGrid
            '--------------------------------------------------------------------
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            Dim loNameFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Name")            
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loNameFilterAttribute})
            lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
        End If


    End Sub

    Private Sub Diagram_LinkSelecting(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkSelecting

    End Sub

    Private Sub Diagram_NodeCreating(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeValidationEventArgs) Handles Diagram.NodeCreating

        e.Cancel = True

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        Dim lrShapeNode As ShapeNode

        Me.zrPage.MakeDirty()
        frmMain.ToolStripButton_Save.Enabled = True

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Process, _
                      pcenumConceptType.Actor, _
                      pcenumConceptType.DataStore
                lrShapeNode = e.Node
                Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                lrFactDataInstance = lrShapeNode.Tag.FactDataInstance
                lrFactDataInstance.X = e.Node.Bounds.X
                lrFactDataInstance.Y = e.Node.Bounds.Y
        End Select


        'Dim lrShape As ShapeNode
        'Dim lrLink As DiagramLink
        'Dim lrORMObject As New Object
        'Dim lrFactInstance As New FBM.FactInstance


        ''-------------------------------------------------------------------------------------------
        ''The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        ''-------------------------------------------------------------------------------------------            

        'Me.zrPage.MakeDirty()
        'frmMain.ToolStripButton_Save.Enabled = True

        'lrShape = e.Node

        'If IsSomething(e.Node.Tag) Then
        '    For Each lrLink In lrShape.GetAllLinks
        '        '--------------------------------------------------------------------------------
        '        'Update the X,Y coordinates of the Object/s associated with the ShapeNode.
        '        '  Remember, in this type of diagram, the Objects are FactDataInstances
        '        '  on the Fact data associated with the Object (e.g. {Storeman, ReceiveGoods}
        '        '  where that Fact is within the FactType 'ActorToProcessParticipationRelation'
        '        '--------------------------------------------------------------------------------
        '        If lrLink.Origin Is lrShape Then
        '            lrORMObject = lrLink.Origin.Tag
        '        End If
        '        If lrLink.Destination Is lrShape Then
        '            lrORMObject = lrLink.Destination.Tag
        '        End If
        '        lrORMObject.X = e.Node.Bounds.X
        '        lrORMObject.Y = e.Node.Bounds.Y

        '        '---------------------------------------
        '        'Get the Fact associated with the Link
        '        '---------------------------------------
        '        lrFactInstance = lrLink.Tag
        '        Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
        '        Dim lrRole As New FBM.Role(lrFactInstance.FactType.FactType, "Temp")
        '        Select Case lrORMObject.ConceptType
        '            Case Is = pcenumConceptType.Actor
        '                lrRole.Id = pcenumCMML.Actor.ToString
        '            Case Is = pcenumConceptType.Process
        '                lrRole.Id = pcenumCMML.Process.ToString
        '        End Select
        '        lrRole.Name = lrRole.Id
        '        lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
        '        lrFactDataInstance.Data = lrORMObject.Data
        '        lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.Equals)
        '        lrFactDataInstance.X = e.Node.Bounds.X
        '        lrFactDataInstance.Y = e.Node.Bounds.Y
        '    Next
        'End If

        'Me.Diagram.RouteAllLinks()

    End Sub

    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        '------------------------------------------------------------------------------------------
        'NB IMPORTANT: UseCaseDiagramView.MouseDown gets processed before Diagram.NodeSelected, so management of which object
        '  is displayed in the PropertyGrid is performed in ORMDiagram.MouseDown
        '------------------------------------------------------------------------------------------

        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Process
                e.Node.Pen.Color = Color.Blue
            Case Is = pcenumConceptType.Actor
                e.Node.Pen.Color = Color.Blue
            Case Else
                'Do nothing
        End Select

        '----------------------------------------------------
        'Set the ContextMenuStrip menu for the selected item
        '----------------------------------------------------
        Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
            Case Is = pcenumConceptType.Actor
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Actor
            Case Is = pcenumConceptType.Process
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Process
            Case Else
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
        End Select

        Me.zrPage.SelectedObject.Clear()
        Me.zrPage.SelectedObject.Add(e.Node.Tag)

        '--------------------------------------
        'Set the PropertiesGrid.SeletedObject
        '--------------------------------------
        Dim lrPropertyGridForm As frmToolboxProperties

        lrPropertyGridForm = prRichmondApplication.GetToolboxForm(frmToolboxProperties.Name)
        If IsSomething(lrPropertyGridForm) Then
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")            
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
            lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
        End If


    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Call Me.HiddenDiagramView.SendToBack()
        Me.MorphTimer.Enabled = False

        frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
        Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)

        Me.DiagramView.BringToFront()
        Me.Diagram.Invalidate()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lr_point As New Point
        Dim lr_rect As New Rectangle
        Dim lrMorphVector As tMorphVector

        For Each lrMorphVector In Me.MorphVector
            lr_point = lrMorphVector.get_nextmorphvector_step_point
            lrMorphVector.Shape.Move(lr_point.X, lr_point.Y)
        Next
        Me.HiddenDiagram.Invalidate()

        If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
            Me.MorphStepTimer.Enabled = False
        End If

    End Sub

    Private Sub ContextMenuStrip_Actor_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Actor.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_actor As New CMML.tActor


        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '-------------------------------------
        'Check that selected object is Process
        '-------------------------------------
        If lr_actor.GetType Is Me.zrPage.SelectedObject(0).GetType Then
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

        lr_actor = Me.zrPage.SelectedObject(0)

        lr_model = lr_actor.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.MorphVector.Clear()
        Me.MorphVector.Add(New tMorphVector(lr_actor.X, lr_actor.Y, 0, 0, 40))

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.ToolStripMenuItemUseCaseDiagramActor.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the UseCaseDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------

        '--------------------------------------------------------------------------------------------------------
        'The EntityType represents an Actor. i.e. Has a ParentEntityType of 'Actor' within the Core meta-schema
        '--------------------------------------------------------------------------------------------------------
        larPage_list = prRichmondApplication.CMML.get_use_case_diagram_pages_for_actor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.ToolStripMenuItemUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUseCaseDiagram, _
                                                       lr_page, _
                                                       lr_page.Model.EnterpriseId, _
                                                       lr_page.Model.SubjectAreaId, _
                                                       lr_page.Model.ProjectId, _
                                                       lr_page.Model.SolutionId, _
                                                       lr_page.Model.ModelId, _
                                                       pcenumLanguage.UseCaseDiagram, _
                                                       Nothing, lr_page.PageId)
            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.MorphToUseCaseDiagram
        Next

        '--------------------------------------------------------------
        'Clear the list of ORMDiagrams that may relate to the EntityType
        '--------------------------------------------------------------
        Me.MenuOptionORMDiagramActor.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the ORMDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------                        
        larPage_list = prRichmondApplication.CMML.get_orm_diagram_pages_for_actor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem

            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, 
            '  they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel, _
                                                       lr_page, _
                                                       lr_model.EnterpriseId, _
                                                       lr_model.SubjectAreaId, _
                                                       lr_model.ProjectId, _
                                                       lr_model.SolutionId, _
                                                       lr_model.ModelId, _
                                                       pcenumLanguage.ORMModel, _
                                                       Nothing, _
                                                       lr_page.PageId)

            lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            If IsSomething(lr_enterprise_view) Then
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                lo_menu_option = Me.MenuOptionORMDiagramActor.DropDownItems.Add(lr_page.Name)
                lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                AddHandler lo_menu_option.Click, AddressOf Me.MorphToORMDiagram
            End If
        Next

    End Sub

    Private Sub ETDDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loObject As Object
        Dim loNode As DiagramNode

        'UseCaseDiagramView.SmoothingMode = SmoothingMode.Default

        '----------------------------------------------------
        'Check to see if the user has used the Control key to
        '  do a multi-select
        '----------------------------------------------------
        If Control.ModifierKeys And Keys.Control Then
            Exit Sub
        End If

        '-------------------------------------------------------
        'Check to see if the user was clicking over a ShapeNode
        '-------------------------------------------------------
        lo_point = Diagram.PixelToUnit(e.Location)

        If IsSomething(Diagram.GetItemAt(lo_point, False)) Then
            '----------------------------------------------
            'Mouse is over a DiagramItem
            '----------------------------------------------
            loObject = Diagram.GetItemAt(lo_point, False)

            If Not (TypeOf (loObject) Is MindFusion.Diagramming.DiagramNode) Then
                Exit Sub
            Else
                loNode = Diagram.GetItemAt(lo_point, False)
            End If


            '----------------------------------------------
            'Reset the cursor to a hand
            '----------------------------------------------
            'UseCaseDiagramView.DrawLinkCursor = Cursors.Hand
            Cursor.Show()

            '-------------------------------------------------------------------------------------------
            'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
            '-------------------------------------------------------------------------------------------            
            If Not IsNothing(loNode.Tag) Then
                loNode.Tag.x = loNode.Bounds.X
                loNode.Tag.y = loNode.Bounds.Y
            End If

            '-----------------------------------------------------------------------------------
            'Set/Reset the color of the ShapeNode under the mouse cursor
            '-----------------------------------------------------------------------------------
            loNode = Diagram.GetItemAt(lo_point, False)

            Select Case loNode.Tag.ConceptType
                Case Is = pcenumConceptType.Actor
                    loNode.Pen.Color = Color.Blue
                Case Else
                    loNode.Pen.Color = Color.Blue
            End Select

        Else
            '------------------------------------------------------------------
            'Mouse is over the canvas and a MultiSelection may have taken place
            '------------------------------------------------------------------
        End If

        '---------------------------------------------
        'Refresh the Diagram drawing (ShapeNode/Links)
        '---------------------------------------------
        Diagram.Invalidate()

    End Sub

    Private Sub ETDDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseWheel

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

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        If Not IsNothing(frmMain.zfrm_properties) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.DataFlowModel
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

    Private Sub PageAsORMMetamodelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetamodelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)

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

    Private Sub ModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary(Me.zrPage.Model)

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        If e.Node.Tag.ConceptType = pcenumConceptType.Process Then
            e.Node.Tag.name = e.NewText
        End If

    End Sub

    Private Sub ResetNodeAndLinkColours()

        Dim liInd As Integer

        '----------------------------------
        'Reset ShapeNode.Colors
        '----------------------------------
        For liInd = 1 To Diagram.Nodes.Count
            Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                Case Is = pcenumConceptType.Actor
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                Case Else
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
            End Select
        Next

        '----------------------------
        'Reset Link.Colors to Black. 
        '----------------------------
        Dim lrLink As DiagramLink
        For liInd = 1 To Diagram.Links.Count
            lrLink = Diagram.Links(liInd - 1)
            lrLink.Pen.Color = Color.Black
            'lrLink.Tag.HasBeenMoved = False
        Next

        For liInd = 1 To Diagram.Links.Count

            lrLink = Diagram.Links(liInd - 1)

            '--------------------------------
            'Disambiguate overlapping links
            '--------------------------------
            If lrLink.Origin.Tag.Name = "New Actor 3" Then
                'Debugger.Break()
            End If
            Dim commonLinks As DiagramLinkCollection = GetCommonLinks(lrLink.Origin, lrLink.Destination)

            Dim pt1 As PointF = lrLink.ControlPoints(0)
            Dim pt2 As PointF = lrLink.ControlPoints(lrLink.ControlPoints.Count - 1)

            If commonLinks.Count > 1 Then
                For c As Integer = 0 To commonLinks.Count - 1
                    Dim link As DiagramLink = commonLinks(c)

                    'If Not link.Tag.HasBeenMoved Then
                    link.Style = LinkStyle.Bezier
                    link.SegmentCount = 1

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
                    'End If

                    '  End If
                Next
            Else
                'lrLink.AutoRoute = True
            End If


        Next

    End Sub


    Private Function GetCommonLinks(ByVal node1 As DiagramNode, ByVal node2 As DiagramNode) As DiagramLinkCollection
        Dim commonLinks As New DiagramLinkCollection()

        For Each link As DiagramLink In node1.OutgoingLinks
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
    End Function

    Private Sub PolarToCartesean(ByVal coordCenter As PointF, ByVal a As Single, ByVal r As Single, ByRef dekart As PointF)
        If r = 0 Then
            dekart = coordCenter
            Return
        End If

        dekart.X = CSng(coordCenter.X + Math.Cos(a * Math.PI / 180) * r)
        dekart.Y = CSng(coordCenter.Y - Math.Sin(a * Math.PI / 180) * r)
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

End Class