Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout

Public Class frm_UseCaseModel

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing
    Public UseCaseModel As New tUseCaseModel

    Private zo_containernode As ContainerNode

    Private morph_vector As tmorphvector
    Private morph_shape As New ShapeNode


    Private Sub frm_UseCaseModel_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '-------------------------------------------
        'Process the page associated with the form.
        '-------------------------------------------
        If IsSomething(Me.zrPage) Then
            If Me.zrPage.IsDirty Then
                Select Case MsgBox("Changes have been made to the Page. Would you like to save those changes?", MsgBoxStyle.YesNoCancel)
                    Case Is = MsgBoxResult.Yes
                        Me.zrPage.save()
                    Case Is = MsgBoxResult.Cancel
                        e.Cancel = True
                        Exit Sub
                End Select
            End If
            Me.zrPage.form = Nothing
            Me.zrPage.ReferencedForm = Nothing
        End If

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------        
        Me.zrPage.FormLoaded = False

        prRichmondApplication.workingmodel = Nothing
        prRichmondApplication.workingpage = Nothing

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


    Private Sub frm_UseCaseModel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Sub SetupForm()

        zo_containernode = Diagram.Factory.CreateContainerNode(10, 10, 100, 100, False)

        zo_containernode.Tag = New tUseCaseSystemBoundary(pcenumConceptType.SystemBoundary)
        'lo_containernode.AllowIncomingLinks = False
        zo_containernode.AllowOutgoingLinks = False
        zo_containernode.Caption = "System Boundary"
        zo_containernode.ToolTip = "System Boundary"
        zo_containernode.Foldable = True
        zo_containernode.AutoShrink = True

    End Sub


    Private Sub AddActorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'add_actor_frm.ShowDialog()

    End Sub

    Private Sub frm_UseCaseModel_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call SetToolbox()


    End Sub

    Private Sub frm_UseCaseModel_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
                frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = Me.zoTreeNode
            End If

            If IsSomething(frmMain.zfrm_model_dictionary) Then
                Call frmMain.zfrm_model_dictionary.LoadToolboxModelDictionary(Me.zrPage.Language)
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        Call SetToolbox()

    End Sub


    Private Sub UseCaseDiagramView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call SetToolbox()

        'Me.UseCaseDiagramView.Behavior = Behavior.Modify
        Me.Diagram.Invalidate()

    End Sub

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.UsecaseShapeLibrary)
        Dim lo_shape As Shape
        Dim child As New frmToolbox

        If prRichmondApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then

            child = prRichmondApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
            child.ShapeListBox.Shapes = lsl_shape_library.Shapes

            For Each lo_shape In child.ShapeListBox.Shapes
                Select Case lo_shape.DisplayName
                    Case Is = "Actor"
                        lo_shape.Image = My.Resources.actor
                End Select
            Next
        End If

    End Sub

    Private Sub UseCaseDiagramView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.DoubleClick

        '---------------------------------------
        'Only allow 'InPlaceEdit' on Processes
        '---------------------------------------
        If Me.Diagram.Selection.Items.Count = 1 Then
            If Me.Diagram.Selection.Items(0).Tag.ConceptType = pcenumConceptType.process Then
                Me.DiagramView.AllowInplaceEdit = True
            Else
                Me.DiagramView.AllowInplaceEdit = False
            End If
        Else
            Me.DiagramView.AllowInplaceEdit = False
        End If

    End Sub

    Private Sub UseCaseDiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim liInd As Integer = 0
        Dim loNode As New ShapeNode

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            If loDraggedNode.Index >= 0 Then
                If loDraggedNode.Index < frmMain.zfrm_toolbox.ShapeListBox.ShapeCount Then
                    Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
                    Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

                    Select Case frmMain.zfrm_toolbox.ShapeListBox.Shapes(loDraggedNode.Index).Id
                        Case Is = "Actor"
                            '----------------------------------------------------------
                            'Establish a new Actor(EntityType) for the dropped object.
                            '  i.e. Establish the EntityType within the Model as well
                            '  as creating a new object for the Actor.
                            '----------------------------------------------------------
                            Dim lrEntityType As FBM.EntityType = Me.zrPage.Model.CreateEntityType

                            lrEntityType.SetName("New Actor")
                            Dim liCount As Integer
                            liCount = Me.zrPage.Model.EntityType.FindAll(AddressOf lrEntityType.EqualsByNameLike).Count
                            lrEntityType.Name &= " " & (CStr(liCount) + 1)

                            '--------------------------------------------
                            'Set the ParentEntityType for the new Actor
                            '--------------------------------------------
                            Dim lrParentEntityType As FBM.EntityType = New FBM.EntityType(Me.zrPage.Model, pcenumLanguage.ORMModel, "Actor", "Actor")
                            lrParentEntityType = Me.zrPage.Model.EntityType.Find(AddressOf lrParentEntityType.Equals)

                            lrEntityType.parentEntityType.Add(lrParentEntityType)

                            '---------------------------------------------------
                            'Find the Core Page that lists Actor (EntityTypes)
                            '---------------------------------------------------
                            Dim lrPage As New FBM.Page(Me.zrPage.Model, "Core-ActorEntityTypes", "Core-ActorEntityTypes", pcenumLanguage.ORMModel)
                            lrPage = Me.zrPage.Model.Page.Find(AddressOf lrPage.Equals)
                            lrPage.EntityTypeInstance.Add(lrEntityType.CloneInstance(lrPage))

                            Call lrPage.ClearAndRefresh()

                            Dim lrActor As New CMML.tActor(Me.zrPage)

                            liCount = Me.zrPage.Model.EntityType.FindAll(AddressOf lrActor.EqualsByNameLike).Count
                            lrActor.Name &= " " & (CStr(liCount) + 1)

                            Call Me.DropActorAtPoint(lrActor, loPointF)
                        Case Is = "Process"
                            Dim lrProcess As New CMML.Process(Me.zrPage, System.Guid.NewGuid.ToString)
                            Call Me.DropProcessAtPoint(lrProcess, loPointF)
                    End Select
                End If
            End If
        End If


    End Sub

    Sub DropProcessAtPoint(ByRef arProcessInstance As CMML.Process, ByVal aoPointF As PointF)

        Try

            arProcessInstance.X = aoPointF.X
            arProcessInstance.Y = aoPointF.Y

            Call arProcessInstance.DisplayAndAssociate(Me.zo_containernode)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prRichmondApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    'Sub drop_process_at_point(ByRef ar_process_instance As tprocess_instance, ByVal ao_pt As PointF)

    '    Dim loDroppedNode As ShapeNode
    '    Dim lo_container_node As ContainerNode = Nothing

    '    Try
    '        lo_container_node = Me.Diagram.GetNodeAt(ao_pt, True, True)
    '    Catch exception As Exception
    '    End Try

    '    dim lr_process_instance = New tprocess_instance

    '    loDroppedNode = Diagram.Factory.CreateShapeNode(ao_pt.X, ao_pt.Y, 2, 2)
    '    loDroppedNode.Shape = Shapes.Ellipse
    '    loDroppedNode.HandlesStyle = HandlesStyle.HatchHandles3 'HatchHandles3 is a very professional look, or SquareHandles2
    '    loDroppedNode.ToolTip = "process"
    '    loDroppedNode.Visible = False

    '    If IsSomething(lo_container_node) Then
    '        lo_container_node.Add(loDroppedNode)
    '    End If

    '    loDroppedNode.Tag = New tprocess_instance
    '    loDroppedNode.Resize(40, 12)

    '    loDroppedNode.Text = ar_process_instance.name

    '    lr_process_instance.Model = prRichmondApplication.workingmodel
    '    lr_process_instance.PageId = prRichmondApplication.workingpage.PageId
    '    lr_process_instance.process.LongDescription = ar_process_instance.LongDescription
    '    lr_process_instance.process.ShortDescription = ar_process_instance.ShortDescription
    '    lr_process_instance.shape = loDroppedNode
    '    lr_process_instance.Symbol = ar_process_instance.Symbol
    '    lr_process_instance.X = loDroppedNode.Bounds.X
    '    lr_process_instance.Y = loDroppedNode.Bounds.Y

    '    If Not pr_UseCaseModel.process.Exists(AddressOf ar_process_instance.Equals) Then
    '        '--------------------------------------------------
    '        'The EntityType is not already within the ORMModel
    '        '  so add it.
    '        '--------------------------------------------------
    '        pr_UseCaseModel.process.Add(pr_process)
    '    End If

    '    pr_UseCaseModel.process_instance.Add(lr_process_instance)
    '    loDroppedNode.Text = lr_process_instance.name
    '    loDroppedNode.Tag = lr_process_instance
    '    loDroppedNode.Visible = True
    '    ar_process_instance.shape = loDroppedNode

    'End Sub

    Public Sub load_use_case_page(ByRef arPage As FBM.Page, ByRef aoTreeNode As TreeNode)

        '------------------------------------------------------------------------
        'Loads the Use Case Diagram for the given ORM (meta-model) Page
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

        '-------------------------------------------------------
        'Set the Caption/Title of the Page to the PageName
        '-------------------------------------------------------
        Me.zrPage = arPage
        Me.TabText = arPage.Name

        Me.zoTreeNode = aoTreeNode

        Dim lr_actor_shape, lr_process_shape As New ShapeNode


        '------------------------------------------------------------------------
        'Display the UseCaseDiagram by logically associating Shape objects
        '   with the corresponding 'object' within the ORMModelPage object
        '------------------------------------------------------------------------
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString)
        If IsSomething(lrFactTypeInstance) Then
            '------------------------------------------------------------------
            'At least one Actor/Process relation has already been established
            '------------------------------------------------------------------
            Me.UseCaseModel.ActorToProcessParticipationRelation = lrFactTypeInstance
        Else
        End If

        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ProcessToProcessRelation.ToString)
        Me.UseCaseModel.PocessToProcessRelation = lrFactTypeInstance


        Dim lrFactInstance As New FBM.FactInstance
        Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

        If IsSomething(Me.UseCaseModel.ActorToProcessParticipationRelation) Then
            For Each lrFactInstance In Me.UseCaseModel.ActorToProcessParticipationRelation.Fact
                For Each lrFactDataInstance In lrFactInstance.Data

                    If lrFactDataInstance.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString Then
                        '------------
                        'Is Actor
                        '------------
                        Dim lr_actor As New CMML.tActor
                        lr_actor = lrFactDataInstance.CloneActor(arPage)

                        '-------------------------------------------------------------------------------
                        'Check to see if the Shape for the Actor has already been loaded onto the Page
                        '-------------------------------------------------------------------------------
                        Dim lr_node As MindFusion.Diagramming.DiagramNode
                        Dim lbActorAlreadyLoaded As Boolean = False
                        For Each lr_node In Diagram.Nodes
                            Select Case lr_node.GetType.ToString
                                Case Is = GetType(MindFusion.Diagramming.ShapeNode).ToString
                                    If lr_node.Tag.ConceptType = pcenumConceptType.Actor Then
                                        If lr_node.Tag.Name = lr_actor.Name Then
                                            lbActorAlreadyLoaded = True
                                            loDroppedNode = lr_node
                                            lr_actor_shape = loDroppedNode
                                        End If
                                    End If
                            End Select
                        Next

                        If lbActorAlreadyLoaded Then
                            '----------------------------------------------------------------
                            'Nothing further to do, because the Actor ShapeNode has already
                            '  been loaded onto the Page.
                            '----------------------------------------------------------------
                        Else
                            '----------------------------------------------
                            'Create a ShapeNode on the Page for the Actor
                            '----------------------------------------------
                            loDroppedNode = Diagram.Factory.CreateShapeNode(lr_actor.X, lr_actor.Y, 2, 2)
                            loDroppedNode.Shape = Shapes.Ellipse
                            loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                            loDroppedNode.Resize(20, 15)
                            loDroppedNode.AllowIncomingLinks = True
                            loDroppedNode.AllowOutgoingLinks = True
                            loDroppedNode.Image = My.Resources.actor
                            loDroppedNode.Transparent = True

                            loDroppedNode.Tag = New ERD.Entity
                            loDroppedNode.Tag = lr_actor
                            lr_actor.shape = loDroppedNode

                            lr_actor_shape = loDroppedNode 'lr_actor_shape is used below to draw the link

                            '-----------------------------------------
                            'Establish the Name caption for the Actor
                            '-----------------------------------------
                            Dim StringSize As New SizeF
                            Dim lo_actor_name As New ShapeNode

                            StringSize = Me.Diagram.MeasureString("[" & Trim(lr_actor.Name) & "]", Me.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            Dim lr_rectanglef As New RectangleF(loDroppedNode.Bounds.X, loDroppedNode.Bounds.Bottom, StringSize.Width, StringSize.Height)
                            lo_actor_name = Me.Diagram.Factory.CreateShapeNode(lr_rectanglef, MindFusion.Diagramming.Shapes.Rectangle)
                            lo_actor_name.HandlesStyle = HandlesStyle.InvisibleMove
                            lo_actor_name.TextColor = Color.Black
                            lo_actor_name.Transparent = True
                            lo_actor_name.Visible = True
                            lo_actor_name.Text = lr_actor.Name
                            lo_actor_name.ZTop()
                            Dim lrActorName As New CMML.ActorName
                            lo_actor_name.Tag = lrActorName
                            lrActorName.Shape = lo_actor_name

                            lr_actor.NameShape = lo_actor_name

                            '-----------------------------------------------------------
                            'Attach the Actor.Name ShapeNode to the Actor Shape
                            '-----------------------------------------------------------
                            lo_actor_name.AttachTo(loDroppedNode, AttachToNode.BottomCenter)
                        End If
                    Else
                        '------------
                        'Is Process
                        '------------
                        Dim lr_process As New CMML.Process
                        lr_process = lrFactDataInstance.CloneProcess(arPage)

                        loDroppedNode = Diagram.Factory.CreateShapeNode(lr_process.X, lr_process.Y, 2, 2)
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                        loDroppedNode.Resize(20, 15)
                        loDroppedNode.AllowIncomingLinks = True
                        loDroppedNode.AllowOutgoingLinks = True
                        loDroppedNode.Text = lr_process.Name
                        Me.zo_containernode.Add(loDroppedNode)

                        loDroppedNode.Tag = New ERD.Entity
                        loDroppedNode.Tag = lr_process
                        lr_process.shape = loDroppedNode

                        lr_process_shape = loDroppedNode
                    End If

                Next 'For Each lrFactDataInstance In lrFactInstance.data

                '------------------------------------------
                'Link the Actor to the associated Process
                '------------------------------------------
                Dim lo_link As DiagramLink
                lo_link = Me.Diagram.Factory.CreateDiagramLink(lr_actor_shape, lr_process_shape)
                lo_link.Tag = New FBM.ModelObject
                lo_link.Tag = lrFactInstance
            Next
        End If

        If IsSomething(Me.UseCaseModel.PocessToProcessRelation) Then
            For Each lrFactInstance In Me.UseCaseModel.ActorToProcessParticipationRelation.Fact
                For Each lrFactDataInstance In lrFactInstance.Data

                Next
            Next
        End If

        Me.Diagram.Invalidate()
        Me.zrPage.FormLoaded = True

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
                        Case Else
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                    End Select
                Case Is = pcenumConceptType.FactTable
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                Case Else
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
            End Select
        Next

        For liInd = 1 To Diagram.Links.Count
            Diagram.Links(liInd - 1).Pen.Color = Color.Black
        Next

        Me.Diagram.Invalidate()

    End Sub

    Private Sub UseCaseDiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragOver

        Dim p As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
        Dim lo_point As PointF = Me.DiagramView.ClientToDoc(New Point(p.X, p.Y))
        Dim loNode As MindFusion.Diagramming.DiagramNode


        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            loNode = Diagram.GetNodeAt(lo_point)
            If TypeOf loNode Is MindFusion.Diagramming.ShapeNode Then
                loNode.Pen = New MindFusion.Drawing.Pen(Color.Brown)
            End If
        Else
            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
                Dim lnode_dragged_node As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)
                If lnode_dragged_node.Index = 1 Then
                    'Is Process and should be dropped only within the SystemBoundary (ContainerNode)
                    e.Effect = DragDropEffects.None
                    Exit Sub
                End If
            End If
        End If


        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Sub add_actor()


    End Sub

    Sub add_process()


    End Sub

    Sub DropActorAtPoint(ByRef arActor As CMML.tActor, ByVal aoPointF As PointF)

        arActor.X = aoPointF.X
        arActor.Y = aoPointF.Y

        If Not Me.UseCaseModel.Actor.Exists(AddressOf arActor.Equals) Then
            '--------------------------------------------------
            'The EntityType is not already within the ORMModel
            '  so add it.
            '--------------------------------------------------
            Me.UseCaseModel.Actor.Add(arActor)
        End If

        Call arActor.DisplayAndAssociate()

    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Dim loObject As Object = e.Link.Destination
        Dim lo_dummy_object As New MindFusion.Diagramming.DummyNode(Me.Diagram)
        Dim lrFact As New FBM.Fact
        Dim lrFactInstance As New FBM.FactInstance
        Dim lo_first_entity As New Object
        Dim lo_second_entity As New Object


        Select Case loObject.GetType.ToString
            Case Is = zo_containernode.GetType.ToString, lo_dummy_object.GetType.ToString
                Dim lo_pointf = New PointF(e.Link.Origin.Bounds.X + e.Link.Bounds.Width, e.Link.Origin.Bounds.Y + e.Link.Bounds.Height)
                'dim lr_process_instance = New tprocess_instance
                'Call drop_process_at_point(pr_process, lo_pointf)
                'e.Link.Destination = lr_process_instance.shape
        End Select

        Me.Cursor = Cursors.WaitCursor

        lo_first_entity = e.Link.Origin.Tag
        lo_second_entity = e.Link.Destination.Tag

        Dim lrTypeOfRelation As pcenumCMMLRelations

        If (lo_first_entity.ConceptType = pcenumConceptType.Actor) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
            lrTypeOfRelation = pcenumCMMLRelations.ActorToProcessParticipationRelation
            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            Dim lsSQLString As String = ""
            lsSQLString = "INSERT INTO " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
            lsSQLString &= " (Actor, Process)"
            lsSQLString &= " VALUES ("
            lsSQLString &= "'" & lo_first_entity.Name & "'"
            lsSQLString &= ",'" & lo_second_entity.Name & "'"
            lsSQLString &= ")"

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            lrFact = Me.zrPage.Model.processORMQLStatement(lsSQLString)

            '----------------------------------
            'Clone an instance of the Fact
            '----------------------------------
            lrFactInstance = lrFact.CloneInstance(Me.zrPage)
            Me.UseCaseModel.ActorToProcessParticipationRelation.AddFact(lrFactInstance)

            Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
            Dim lrRole As New FBM.Role
            lrRole.Id = pcenumCMML.Process.ToString
            lrRole.Name = lrRole.Id
            lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
            lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
            Dim lr_process As New CMML.Process

            lr_process = lrFactDataInstance.CloneProcess(Me.zrPage)
            lr_process.shape = New ShapeNode
            lr_process.shape = Me.Diagram.FindNode(lo_second_entity)
            lr_process.shape.Tag = lr_process
        ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            Dim lsSQLString As String = ""
            lsSQLString = "INSERT INTO " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
            lsSQLString &= " (Process1, Process2)"
            lsSQLString &= " VALUES ("
            lsSQLString &= "'" & lo_first_entity.Name & "'"
            lsSQLString &= ",'" & lo_second_entity.Name & "'"
            lsSQLString &= ")"

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            lrFact = Me.zrPage.Model.processORMQLStatement(lsSQLString)

            '----------------------------------
            'Clone and instance of the Fact
            '----------------------------------
            lrFactInstance = lrFact.CloneInstance(Me.zrPage)
            Me.UseCaseModel.PocessToProcessRelation.AddFact(lrFactInstance)

            Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
            Dim lrRole As New FBM.Role
            lrRole.Id = pcenumCMML.Process.ToString & "2"
            lrRole.Name = lrRole.Id
            lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
            lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
            Dim lr_process As New CMML.Process

            lr_process = lrFactDataInstance.CloneProcess(Me.zrPage)
            lr_process.shape = New ShapeNode
            lr_process.shape = Me.Diagram.FindNode(lo_second_entity)
            lr_process.shape.Tag = lr_process

        End If


        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Diagram_LinkCreating(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkValidationEventArgs) Handles Diagram.LinkCreating

        'e.Link.Tag = New t_actor_process_link
        e.Link.TextStyle = LinkTextStyle.Center

    End Sub

    Private Sub Diagram_LinkDeleted(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkDeleted

        Dim loObject As Object = e.Link.Destination
        Dim lo_dummy_object As New MindFusion.Diagramming.DummyNode(Me.Diagram)
        Dim lo_first_entity As New Object
        Dim lo_second_entity As New Object


        Me.Cursor = Cursors.WaitCursor


        lo_first_entity = e.Link.Origin.Tag
        lo_second_entity = e.Link.Destination.Tag


        If (lo_first_entity.ConceptType = pcenumConceptType.Actor) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
            Dim lsSQLString As String = ""
            lsSQLString = "DELETE FROM " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
            lsSQLString &= " WHERE Actor = '" & lo_first_entity.Name & "'"
            lsSQLString &= "   AND Process = '" & lo_second_entity.Name & "'"

            Me.zrPage.Model.processORMQLStatement(lsSQLString)
        ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
            Dim lsSQLString As String = ""
            lsSQLString = "DELETE FROM " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
            lsSQLString &= " WHERE Process1 = '" & lo_first_entity.Name & "'"
            lsSQLString &= "   AND Process2 = '" & lo_second_entity.Name & "'"

            Me.zrPage.Model.processORMQLStatement(lsSQLString)
        End If

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        '-------------------------------------------------------
        'ORM Verbalisation
        '-------------------------------------------------------
        Dim lrToolboxForm As frmToolboxORMVerbalisation
        lrToolboxForm = prRichmondApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
        If IsSomething(lrToolboxForm) Then
            lrToolboxForm.zrModel = Me.zrPage.Model
            Select Case e.Link.Tag.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Call lrToolboxForm.VerbaliseEntityType(e.Link.Tag.EntityType)
                Case Is = pcenumConceptType.ValueType
                    Call lrToolboxForm.VerbaliseValueType(e.Link.Tag.ValueType)
                Case Is = pcenumConceptType.FactType
                    Call lrToolboxForm.VerbaliseFactType(e.Link.Tag.FactType)
                Case Is = pcenumConceptType.Fact
                    Dim lrFact As New FBM.Fact
                    lrFact = e.Link.Tag.Fact
                    Call lrToolboxForm.VerbaliseFact(lrFact)
                Case Is = pcenumConceptType.RoleConstraint
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    lrRoleConstraintInstance = e.Link.Tag
                    Select Case lrRoleConstraintInstance.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.SubsetConstraint
                            Call lrToolboxForm.VerbaliseRoleConstraintSubset(lrRoleConstraintInstance.RoleConstraint)
                    End Select
            End Select
        End If

        'Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
        '    Case Is = pcenumConceptType.actor_process_link
        '        Me.UseCaseDiagramView.ContextMenuStrip = ContextMenuStrip_ProcessLink
        'End Select

    End Sub

    Private Sub Diagram_NodeDoubleClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDoubleClicked

        Me.DiagramView.Behavior = Behavior.DrawLinks
        Me.Diagram.Invalidate()
        Me.Diagram.Selection.Clear()
        Me.Cursor = Cursors.Hand
        Me.Diagram.Invalidate()

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        Dim lrShape As ShapeNode
        Me.zrPage.MakeDirty()
        frmMain.ToolStripButton_Save.Enabled = True


        If TypeOf (e.Node) Is MindFusion.Diagramming.ContainerNode Then
            Exit Sub
        End If

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        'Dim lrLink As DiagramLink
        Dim lrORMObject As New Object
        Dim lrShapeNode As New ShapeNode
        Dim lrFactInstance As New FBM.FactInstance

        lrShape = e.Node

        If IsSomething(e.Node.Tag) Then

            Select Case e.Node.Tag.ConceptType
                Case Is = pcenumConceptType.Class, _
                          pcenumConceptType.Process
                    lrShapeNode = e.Node
                    Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                    lrFactDataInstance = lrShapeNode.Tag.FactDataInstance
                    lrFactDataInstance.X = e.Node.Bounds.X
                    lrFactDataInstance.Y = e.Node.Bounds.Y
            End Select


            'For Each lrLink In lrShape.GetAllLinks
            '    If lrLink.Origin Is lrShape Then
            '        lrORMObject = lrLink.Origin.Tag
            '    End If
            '    If lrLink.Destination Is lrShape Then
            '        lrORMObject = lrLink.Destination.Tag
            '    End If
            '    lrORMObject.X = e.Node.Bounds.X
            '    lrORMObject.Y = e.Node.Bounds.Y
            '    '---------------------------------------
            '    'Get the Fact associated with the Link
            '    '---------------------------------------
            '    lrFactInstance = lrLink.Tag
            '    Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
            '    Dim lrRole As New FBM.Role
            '    Select Case lrORMObject.ConceptType
            '        Case Is = pcenumConceptType.Actor
            '            lrRole.Id = pcenumCMML.Actor.ToString
            '        Case Is = pcenumConceptType.Process
            '            lrRole.Id = pcenumCMML.Process.ToString
            '    End Select
            '    lrRole.Name = lrRole.Id
            '    lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
            '    lrFactDataInstance.Data = lrORMObject.Data
            '    lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.Equals)
            '    lrFactDataInstance.X = e.Node.Bounds.X
            '    lrFactDataInstance.Y = e.Node.Bounds.Y
            'Next
        End If

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

        Me.zrPage.SelectedObject.Add(e.Node.Tag)

    End Sub

    Private Sub UseCaseDiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        If e.Node.Tag.ConceptType = pcenumConceptType.Process Then
            e.Node.Tag.name = e.NewText
        End If

    End Sub

    Private Sub IncludesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncludesToolStripMenuItem.Click

        Dim lo_link As DiagramLink = Me.Diagram.Selection.Items(0)
        lo_link.Text = "<Includes>"

    End Sub

    Private Sub ExtendsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtendsToolStripMenuItem.Click

        Dim lo_link As DiagramLink = Me.Diagram.Selection.Items(0)
        lo_link.Text = "<Extends>"

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Dim image As Image = Diagram.CreateImage()
        Windows.Forms.Clipboard.SetImage(image)

    End Sub

    Private Sub UseCaseDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseUp

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

            '------------------------------------------------------------------------------------
            'Reset ShapeNode.Colors to Black. e.g. User 'may' have just moved a whole selection.
            '------------------------------------------------------------------------------------
            For liInd = 1 To Diagram.Nodes.Count
                Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                    Case Else
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                End Select
            Next

            For liInd = 1 To Diagram.Links.Count
                Diagram.Links(liInd - 1).Pen.Color = Color.Black
            Next

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

    Private Sub TimerLinkSwitch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerLinkSwitch.Tick

        'Me.UseCaseDiagramView.Behavior = Behavior.DrawLinks

        TimerLinkSwitch.Enabled = False

    End Sub



    Private Sub mnuOption_ProcessProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem2.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        If Not IsNothing(frmMain.zfrm_properties) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.UseCaseModel
            End If
        End If

    End Sub

    Private Sub UseCaseDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseWheel

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

    Private Sub ModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary(Me.zrPage.Model)

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
        'Check that selected object is Actor
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
        Me.morph_vector = New tMorphVector(lr_actor.X, lr_actor.Y, 0, 0, 40)

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.MenuItemUseCaseDiagramActor.DropDownItems.Clear()

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
            lo_menu_option = Me.MenuItemUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
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
            AddHandler lo_menu_option.Click, AddressOf Me.morph_to_UseCase_diagram
        Next

        '--------------------------------------------------------------
        'Clear the list of ORMDiagrams that may relate to the EntityType
        '--------------------------------------------------------------
        Me.ORMDiagramToolStripMenuItem.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the ORMDiagrams that relate to the Actor
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
                lo_menu_option = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lr_page.Name)
                lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                AddHandler lo_menu_option.Click, AddressOf Me.morph_to_ORM_diagram
            End If
        Next

        '---------------------------------------------------------------------
        'Clear the list of DataFlowDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.MenuOptionDataFlowDiagramActor.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the DataFlowDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------
        larPage_list = prRichmondApplication.CMML.get_DataFlowDiagram_pages_for_actor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.MenuOptionDataFlowDiagramActor.DropDownItems.Add(lr_page.Name)

            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUseCaseDiagram, _
                                                       lr_page, _
                                                       lr_page.Model.EnterpriseId, _
                                                       lr_page.Model.SubjectAreaId, _
                                                       lr_page.Model.ProjectId, _
                                                       lr_page.Model.SolutionId, _
                                                       lr_page.Model.ModelId, _
                                                       pcenumLanguage.DataFlowDiagram, _
                                                       Nothing, lr_page.PageId)

            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.morph_to_DataFlowDiagram
        Next


    End Sub

    Private Sub morph_to_DataFlowDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.UseCaseDiagramView.CopyToClipboard(False)

        Me.DiagramHidden.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.DiagramHidden.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Ellipse
        lr_shape_node.Text = Me.zrPage.SelectedObject(0).Name
        lr_shape_node.Transparent = True

        lr_shape_node.Visible = True

        Me.morph_shape = lr_shape_node

        Me.DiagramHidden.Invalidate()

        Dim lrEntity As ERD.Entity = Me.zrPage.SelectedObject(0)
        Dim liConceptType As pcenumConceptType

        Select Case Me.zrPage.SelectedObject(0).ConceptType
            Case Is = pcenumConceptType.Actor
                liConceptType = pcenumConceptType.Actor
            Case Is = pcenumConceptType.Process
                liConceptType = pcenumConceptType.Process
        End Select


        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            Dim lrFactDataInstance As New Object

            lr_page = lr_enterprise_view.Tag

            Select Case liConceptType
                Case Is = pcenumConceptType.Actor
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance _
                                  From Fact In FactType.Fact _
                                  From RoleData In Fact.Data _
                                  Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                                  And RoleData.Data = lrEntity.Name _
                                  Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                Case Is = pcenumConceptType.Process
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance _
                                       From Fact In FactType.Fact _
                                       From RoleData In Fact.Data _
                                       Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Process.ToString _
                                       And RoleData.Data = lrEntity.Name _
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
            End Select

            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Public Sub morph_to_ORM_diagram(ByVal sender As Object, ByVal e As EventArgs)

        '-----------------------------------------------
        'Get the Menu that just called this procedure.
        '-----------------------------------------------
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
        If Me.zrPage.SelectedObject.Count > 1 Then Exit Sub

        '----------------------------------------------------------
        'Diagram1 is on the HiddenDiagramView
        '  Is a MindFusion Diagram on which the morphing is done.
        '----------------------------------------------------------
        Me.DiagramHidden.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        Dim lr_actor As New CMML.tActor
        lr_actor = Me.zrPage.SelectedObject(0)

        Dim lr_shape_node As ShapeNode


        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrEntityTypeInstanceList = From EntityTypeInstance In lr_page.EntityTypeInstance _
                                           Where EntityTypeInstance.Id = lr_actor.Data _
                                           Select New FBM.EntityTypeInstance(lr_page.Model, _
                                                                    pcenumLanguage.ORMModel, _
                                                                    EntityTypeInstance.Name, _
                                                                    True, _
                                                                    EntityTypeInstance.x, _
                                                                    EntityTypeInstance.y)

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
            For Each lrEntityTypeInstance In lrEntityTypeInstanceList
                Exit For
            Next

            '----------------------------------------------------------------
            'Retreive the actual EntityTypeInstance on the destination page
            '----------------------------------------------------------------
            lrEntityTypeInstance = lr_page.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

            If lr_page.FormLoaded Then
                lr_shape_node = lrEntityTypeInstance.shape.Clone(True)
                Me.morph_shape = lr_shape_node
            Else
                Me.morph_shape = lr_actor.shape.Clone(True)
                Me.morph_shape.Shape = Shapes.RoundRect
                Me.morph_shape.HandlesStyle = HandlesStyle.InvisibleMove
                Me.morph_shape.Text = lr_actor.Data
                Me.morph_shape.Resize(20, 15)
            End If

            Me.DiagramHidden.Nodes.Add(Me.morph_shape)
            Me.DiagramHidden.Invalidate()

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrEntityTypeInstance.x, lrEntityTypeInstance.y, 40)
            Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()

        End If

    End Sub

    Private Sub morph_to_StateTransitionDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        Me.DiagramHidden.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape

        Dim lrProcess As New CMML.Process
        lrProcess = Me.zrPage.SelectedObject(0)

        lr_shape_node = Me.DiagramHidden.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, 70, 5) ' lr_shape_node.Shape = Shapes.RoundRect
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = lrProcess.Name
        lr_shape_node.Pen.Color = Color.Black
        lr_shape_node.Visible = True
        lr_shape_node.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

        Me.morph_shape = lr_shape_node

        Me.DiagramHidden.Invalidate()


        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag

            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the ValueType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, 5, 5, 40)
            Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()

        End If

    End Sub

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ORMDiagramView.CopyToClipboard(False)

        Me.DiagramHidden.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.DiagramHidden.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = ""
        lr_shape_node.Pen.Color = Color.White
        lr_shape_node.Transparent = True
        lr_shape_node.Image = My.Resources.resource_file.actor
        lr_shape_node.Visible = True

        Me.morph_shape = lr_shape_node

        Me.DiagramHidden.Invalidate()

        If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrActor = From FactType In lr_page.FactTypeInstance _
                          From Fact In FactType.Fact _
                          From RoleData In Fact.Data _
                          Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                          Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

            Dim lrFactDataInstance As New Object
            For Each lrFactDataInstance In lrActor
                Exit For
            Next

            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lr_point As New Point
        Dim lr_rect As New Rectangle

        lr_point = Me.morph_vector.get_nextmorphvector_step_point

        Me.morph_shape.Move(lr_point.X, lr_point.Y)
        Me.DiagramHidden.Invalidate()

        If Me.morph_vector.VectorStep > Me.morph_vector.VectorSteps Then
            Me.MorphStepTimer.Stop()
            Me.MorphStepTimer.Enabled = False
            frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = Me.MorphStepTimer.Tag
            Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)
            Me.DiagramView.BringToFront()
            Me.Diagram.Invalidate()
            Me.MorphTimer.Enabled = False
        End If

    End Sub

    Private Sub frm_UseCaseModel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrm_enterprise_tree_viewer) Then
                frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        If IsSomething(Me.zrPage) Then
            Me.zrPage.SelectedObject.Clear()
        End If

        Call Me.SetToolbox()

    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call SetToolbox()

    End Sub

    Private Sub ContextMenuStrip_Process_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Process.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_process As New CMML.Process


        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '-------------------------------------
        'Check that selected object is Actor
        '-------------------------------------
        If lr_process.GetType Is Me.zrPage.SelectedObject(0).GetType Then
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

        lr_process = Me.zrPage.SelectedObject(0)

        lr_model = lr_process.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.morph_vector = New tmorphvector(lr_process.X, lr_process.Y, 0, 0, 40)

        '---------------------------------------------------------------------
        'Clear the list of DFDDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.DFDToolStripMenuItem.DropDownItems.Clear()

        '---------------------------------------------------------------------
        'Clear the list of STDDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.ToolStripMenuItemStateTransitionDiagram.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the DFDDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------

        '--------------------------------------
        'The EntityType represents a Process.
        '--------------------------------------
        larPage_list = prRichmondApplication.CMML.get_DataFlowDiagram_pages_for_process(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            lo_menu_option = Me.DFDToolStripMenuItem.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram, _
                                                       lr_page, _
                                                       lr_page.Model.EnterpriseId, _
                                                       lr_page.Model.SubjectAreaId, _
                                                       lr_page.Model.ProjectId, _
                                                       lr_page.Model.SolutionId, _
                                                       lr_page.Model.ModelId, _
                                                       pcenumLanguage.DataFlowDiagram, _
                                                       Nothing, lr_page.PageId)
            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.morph_to_DataFlowDiagram
        Next

        '--------------------------------------------------------
        'Load the STDDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------

        '--------------------------------------
        'The Entity represents a Process.
        '--------------------------------------
        larPage_list = prRichmondApplication.CMML.get_StateTransitionDiagram_pages_for_process(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            lo_menu_option = Me.ToolStripMenuItemStateTransitionDiagram.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageStateTransitionDiagram, _
                                                       lr_page, _
                                                       lr_page.Model.EnterpriseId, _
                                                       lr_page.Model.SubjectAreaId, _
                                                       lr_page.Model.ProjectId, _
                                                       lr_page.Model.SolutionId, _
                                                       lr_page.Model.ModelId, _
                                                       pcenumLanguage.StateTransitionDiagram, _
                                                       Nothing, lr_page.PageId)
            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.morph_to_StateTransitionDiagram
        Next

    End Sub

    Private Sub frm_UseCaseModel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF

        prRichmondApplication.WorkingPage = Me.zrPage

        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
        Else
            '---------------------------------------------------
            'MouseDown is on canvas (not on object).
            'If any objects are already highlighted as blue, 
            '  then change the outline to black/originalcolour
            '---------------------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prRichmondApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then

                Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                ' And you pass it to the PropertyGrid,
                ' via its BrowsableAttributes property :
                lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

            End If
        End If

        '===============================================================================
        'From second method handling same event
        'Dim lo_point As System.Drawing.PointF
        'Dim loNode As DiagramNode

        'lo_point = Me.UseCaseDiagramView.ClientToDoc(e.Location)

        'Me.UseCaseDiagramView.SmoothingMode = SmoothingMode.AntiAlias

        ''--------------------------------------------------
        ''Just to be sure...set the Richmond.WorkingProject
        ''--------------------------------------------------
        'prRichmondApplication.WorkingPage = Me.zrPage

        'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
        '    '----------------------------
        '    'Mouse is over an ShapeNode
        '    '----------------------------
        '    Me.Diagram.AllowUnconnectedLinks = True

        '    '--------------------------------------------
        '    'Turn on the TimerLinkSwitch.
        '    '  After the user has held down the mouse button for 1second over a object,
        '    '  then link creation will be allowed
        '    '--------------------------------------------
        '    TimerLinkSwitch.Enabled = True

        '    '----------------------------------------------------
        '    'Get the Node/Shape under the mouse cursor.
        '    '----------------------------------------------------
        '    loNode = Diagram.GetNodeAt(lo_point)
        '    Me.UseCaseDiagramView.DrawLinkCursor = Cursors.Hand
        '    Cursor.Show()

        '    '---------------------------------------------------------------------------------------------------------------------------
        '    'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel object selected
        '    '---------------------------------------------------------------------------------------------------------------------------
        '    If Not IsNothing(frmMain.zfrm_properties) Then
        '        frmMain.zfrm_properties.PropertyGrid.SelectedObject = loNode.Tag
        '    End If

        '    'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
        '    '    '----------------------------
        '    '    'Mouse is over an ShapeNode
        '    '    '----------------------------

        '    '    '----------------------------------------------------
        '    '    'Get the Node/Shape under the mouse cursor.
        '    '    '----------------------------------------------------
        '    '    If TypeOf Diagram.GetNodeAt(lo_point) Is MindFusion.Diagramming.TableNode Then
        '    '        Exit Sub
        '    '    End If
        '    '    loNode = Diagram.GetNodeAt(lo_point)

        '    '    If Control.ModifierKeys And Keys.Control Then
        '    '        '------------------------------------
        '    '        'Use is holding down the CtrlKey so
        '    '        '  enforce the selection of the object
        '    '        '------------------------------------                
        '    '        loNode.Selected = True
        '    '        loNode.Pen.Color = Color.Blue
        '    '        prRichmondApplication.workingpage.MultiSelectionPerformed = True

        '    '        Select Case loNode.Tag.ConceptType
        '    '            Case Is = pcenumConceptType.role
        '    '                '----------------------------------------------------------------------
        '    '                'This stops a Role AND its FactType being selected at the same time
        '    '                '----------------------------------------------------------------------
        '    '                prRichmondApplication.workingpage.SelectedObject.Remove(loNode.Tag.FactType)
        '    '        End Select

        '    '        Exit Sub
        '    '    Else
        '    '        If prRichmondApplication.workingpage.MultiSelectionPerformed Then
        '    '            If Diagram.Selection.Nodes.Contains(loNode) Then
        '    '                '--------------------------------------------------------------------
        '    '                'Don't clear the SelectedObjects if the ShapeNode selected/clicked on 
        '    '                '  is within the Diagram.Selection because the user has just performed
        '    '                '  a MultiSelection, ostensibly (one would assume) to then 'move'
        '    '                '  or 'delete' the selection of objects.
        '    '                '--------------------------------------------------------------------                    
        '    '                '------------------------------------------------------------------------------
        '    '                'Unless the Shape.Tag is a FactType, then just select it
        '    '                '  The reason for this, is because of MindFusion.Groups.
        '    '                '  When a User selects a Role...the whole RoleGroup (all Roles in the FactType)
        '    '                '  are selected, so it is a MultiSelection by default.
        '    '                '------------------------------------------------------------------------------
        '    '                Select Case loNode.Tag.ConceptType
        '    '                    Case Is = pcenumConceptType.FactType
        '    '                        me.zrPage.SelectedObject.Clear()
        '    '                        Diagram.Selection.Clear()
        '    '                        '-----------------------------------------------
        '    '                        'Select the ShapeNode/ORMObject just clicked on
        '    '                        '-----------------------------------------------                    
        '    '                        loNode.Selected = True
        '    '                        loNode.Pen.Color = Color.Blue

        '    '                        '-------------------------------------------------------------------
        '    '                        'Reset the MultiSelectionPerformed flag on the ORMModel
        '    '                        '-------------------------------------------------------------------
        '    '                        me.zrPage.MultiSelectionPerformed = False
        '    '                End Select
        '    '            Else
        '    '                '---------------------------------------------------------------------------
        '    '                'Clear the SelectedObjects because the user neither did a MultiSelection
        '    '                '  nor held down the [Ctrl] key before clicking on the ShapeNode.
        '    '                '  Clearing the SelectedObject groups, allows for new objects to be selected
        '    '                '  starting with the ShapeNode/ORMObject just clicked on.
        '    '                '---------------------------------------------------------------------------
        '    '                me.zrPage.SelectedObject.Clear()
        '    '                Diagram.Selection.Clear()
        'ElseIf IsSomething(Diagram.GetLinkAt(lo_point, 2)) Then
        '    '-------------------------
        '    'User clicked on a link
        '    '-------------------------
        'Else
        '    '------------------------------------------------
        '    'Use clicked on the Canvas
        '    '------------------------------------------------

        '    '---------------------------
        '    'Clear the SelectedObjects
        '    '---------------------------
        '    Me.zrPage.SelectedObject.Clear()
        '    Me.Diagram.Selection.Clear()

        '    Me.UseCaseDiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

        '    Me.Diagram.AllowUnconnectedLinks = False

        '    '------------------------------------------------------------------------------
        '    'Clear the 'InPlaceEdit' on principal.
        '    '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas,
        '    '  so disable the 'InPlaceEdit'.
        '    '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on,
        '    '  then 'InPlaceEdit' is temporarily allowed.
        '    '------------------------------------------------------------------------------
        '    Me.UseCaseDiagramView.AllowInplaceEdit = False

        '    '-----------------------------------------------------------------------------------------------------------
        '    'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel
        '    '-----------------------------------------------------------------------------------------------------------
        '    If Not IsNothing(frmMain.zfrm_properties) Then
        '        frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.UseCaseModel
        '    End If

        '    Call Me.reset_node_and_link_colors()
        'End If



    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        If Not IsNothing(frmMain.zfrm_properties) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.UseCaseModel
            End If
        End If
    End Sub

    Private Sub mnuOption_ViewGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

    End Sub

    Private Sub PageAsORMMetaModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetaModelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub
End Class