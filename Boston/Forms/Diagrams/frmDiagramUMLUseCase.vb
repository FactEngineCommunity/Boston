Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Threading

Public Class frmDiagrmUMLUseCase

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing

    Private zo_containernode As ContainerNode

    Private MorphVector As New List(Of tMorphVector)
    Private morph_shape As New ShapeNode


    Private Sub frm_UseCaseModel_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Try
            '-------------------------------------------
            'Process the page associated with the form.
            '-------------------------------------------
            If IsSomething(Me.zrPage) Then
                If Me.zrPage.IsDirty Then
                    Select Case MsgBox("Changes have been made to the Page. Would you like to save those changes?", MsgBoxStyle.YesNoCancel)
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

            prApplication.WorkingModel = Nothing
            prApplication.WorkingPage = Nothing

            '------------------------------------------------
            'If the 'Properties' window is open, reset the
            '  SelectedObject
            '------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Nothing
            End If

            Me.Hide()

            frmMain.ToolStripButton_Save.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub frm_UseCaseModel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Sub SetupForm()

        Try
            zo_containernode = Diagram.Factory.CreateContainerNode(10, 100, 100, 100, False)

            zo_containernode.Tag = New UCD.UseCaseSystemBoundary(pcenumConceptType.SystemBoundary)
            'lo_containernode.AllowIncomingLinks = False
            zo_containernode.AllowOutgoingLinks = False
            zo_containernode.Caption = "System Boundary"
            zo_containernode.ToolTip = "System Boundary"
            zo_containernode.Foldable = True
            zo_containernode.AutoShrink = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub AddActorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'add_actor_frm.ShowDialog()

    End Sub

    Private Sub frm_UseCaseModel_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try
            Call SetToolbox()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub frm_UseCaseModel_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        Try
            If IsSomething(Me.zoTreeNode) Then
                If IsSomething(frmMain.zfrmModelExplorer) Then
                    frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
                End If

                If prApplication.GetToolboxForm(frmToolboxModelDictionary.Name) Is Nothing Then
                    Call frmMain.LoadToolboxModelDictionary(Me.zrPage.Language)
                End If
            End If

            If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
                frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
            End If

            Call SetToolbox()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub UseCaseDiagramView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call SetToolbox()

        'Me.UseCaseDiagramView.Behavior = Behavior.Modify
        Me.Diagram.Invalidate()

    End Sub

    Sub SetToolbox()

        Try

            Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.UsecaseShapeLibrary)
            Dim lo_shape As Shape
            Dim child As New frmToolbox

            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then

                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                child.ShapeListBox.Shapes = lsl_shape_library.Shapes

                For Each lo_shape In child.ShapeListBox.Shapes
                    Select Case lo_shape.DisplayName
                        Case Is = "Actor"
                            lo_shape.Image = My.Resources.CMML.actor
                    End Select
                Next
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

            lsMessage.AppendDoubleLineBreak(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
            lsMessage.AppendDoubleLineBreak(Richmond.GetConfigFileLocation)

            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub UseCaseDiagramView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.DoubleClick

        '---------------------------------------
        'Only allow 'InPlaceEdit' on Processes
        '---------------------------------------
        If Me.Diagram.Selection.Items.Count = 1 Then
            If Me.Diagram.Selection.Items(0).Tag.ConceptType = pcenumConceptType.Process Then
                Me.DiagramView.AllowInplaceEdit = True
            Else
                'Me.DiagramView.AllowInplaceEdit = False
            End If
        Else
            'Me.DiagramView.AllowInplaceEdit = False
        End If

    End Sub

    Private Sub UseCaseDiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim liInd As Integer = 0
        Dim loNode As New ShapeNode

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            If loDraggedNode.Index >= 0 Then

                Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

                If loDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount Then
                    Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
                    Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

                    Select Case lrToolboxForm.ShapeListBox.Shapes(loDraggedNode.Index).Id
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

                            '20220613-VM-Removed. Now using CoreElementHasElementType at the CMML level.
                            'lrEntityType.parentEntityType.Add(lrParentEntityType)

                            '---------------------------------------------------
                            'Find the Core Page that lists Actor (EntityTypes)
                            '---------------------------------------------------
                            Dim lrPage As New FBM.Page(Me.zrPage.Model, "Core-ActorEntityTypes", "Core-ActorEntityTypes", pcenumLanguage.ORMModel)
                            lrPage = Me.zrPage.Model.Page.Find(AddressOf lrPage.Equals)
                            lrPage.EntityTypeInstance.Add(lrEntityType.CloneInstance(lrPage))

                            Call lrPage.ClearAndRefresh()

                            Dim lrActor As New UCD.Actor(Me.zrPage)

                            liCount = Me.zrPage.Model.EntityType.FindAll(AddressOf lrActor.EqualsByNameLike).Count
                            lrActor.Name &= " " & (CStr(liCount) + 1)

                            Call Me.DropActorAtPoint(lrActor, loPointF)

                        Case Is = "Process"

                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New ProcessTest", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Call Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Me.zo_containernode)
                            Debugger.Break()
                    End Select
                End If
            End If
        End If


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

    '    lr_process_instance.Model = prApplication.workingmodel
    '    lr_process_instance.PageId = prApplication.workingpage.PageId
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
        '            The Boston data model is an ORM meta-model. Use Case Diagrams are stored as 
        '            propositional functions within the ORM meta-model.
        '
        'PSEUDOCODE
        '        
        '  * 
        '------------------------------------------------------------------------
        Dim loDroppedNode As ShapeNode = Nothing
        Dim lrFactTypeInstance_pt As PointF = Nothing

        Try
            '-------------------------------------------------------
            'Set the Caption/Title of the Page to the PageName
            '-------------------------------------------------------
            Me.zrPage = arPage
            Me.TabText = arPage.Name

            Me.zoTreeNode = aoTreeNode

            Dim lrActorShape, lrProcessShape As New ShapeNode

            Me.zrPage.UMLDiagram.Actor.Clear()
            Me.zrPage.UMLDiagram.Process.Clear()

            '--------------------
            'Load the Entities.
            '--------------------
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Dim lrActor As UCD.Actor = Nothing
            Dim lrProcess As UCD.Process
            Dim lrFactTypeInstance As New FBM.FactTypeInstance
            Dim lrFactInstance As FBM.FactInstance
            Dim lrFactDataInstance As FBM.FactDataInstance

#Region "Actors - Load"
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE ElementType = 'Actor'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF

                lrFactDataInstance = lrRecordset("Element")
                lrActor = lrFactDataInstance.CloneUCDActor(arPage)
                lrActor.X = lrFactDataInstance.X
                lrActor.Y = lrFactDataInstance.Y

                Call lrActor.DisplayAndAssociate()

                Me.zrPage.UMLDiagram.Actor.Add(lrActor)

                lrRecordset.MoveNext()
            End While
#End Region

#Region "Processes - Load"

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSQLQuery &= " WHERE ElementType = 'Process'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF

                lrFactDataInstance = lrRecordset("Element")
                lrProcess = lrFactDataInstance.CloneUCDProcess(arPage)
                lrProcess.X = lrFactDataInstance.X
                lrProcess.Y = lrFactDataInstance.Y

#Region "Process Text - Get from CMML"

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                lsSQLQuery &= " WHERE Process = '" & lrProcess.Id & "'"

                Dim lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrRecordset2.EOF Then
                    lrProcess.Text = lrRecordset2("ProcessText").Data
                End If
#End Region

                Call lrProcess.DisplayAndAssociate()

                Me.zrPage.UMLDiagram.Process.Add(lrProcess)

                lrRecordset.MoveNext()
            End While
#End Region

            'Set the location of the System Boundary
            If Me.zrPage.UMLDiagram.Actor.Count > 0 Then
                Dim larMinActorX = (From Actor In Me.zrPage.UMLDiagram.Actor
                                    Select Actor.X).Min
                Me.zo_containernode.Move(larMinActorX + 30, zo_containernode.Bounds.Y)
            End If

            '------------------------------------------------------------------------
            'Display the UseCaseDiagram by logically associating Shape objects
            '   with the corresponding 'object' within the ORMModelPage object
            '------------------------------------------------------------------------
            Me.zrPage.UMLDiagram.ActorToProcessParticipationRelationFTI = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString)
            Me.zrPage.UMLDiagram.PocessToProcessRelationFTI = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString)

#Region "Actor to Process Participation Relations"
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF

#Region "Actor"
                Dim lbActorAlreadyLoaded As Boolean = False

                lrFactDataInstance = lrRecordset("Actor")
                lrActor = lrFactDataInstance.CloneUCDActor(arPage)

                '-------------------------------------------------------------------------------
                'Check to see if the Shape for the Actor has already been loaded onto the Page
                '-------------------------------------------------------------------------------
                Dim lrExistingActor = Me.zrPage.UMLDiagram.Actor.Find(Function(x) x.Name = lrActor.Name)
                If lrExistingActor IsNot Nothing Then
                    lbActorAlreadyLoaded = True
                    loDroppedNode = lrActor.Shape
                    lrActorShape = loDroppedNode
                    lrActor = lrExistingActor
                Else
                    lrActor.DisplayAndAssociate()
                    Me.zrPage.UMLDiagram.Actor.Add(lrActor)
                End If
#End Region

#Region "Process"
                lrFactDataInstance = lrRecordset("Process")
                lrProcess = lrFactDataInstance.CloneUCDProcess(arPage)

                Dim lrActualProcess As UCD.Process = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrProcess.Id)

                If lrActualProcess Is Nothing Then

#Region "Process Text - Get from CMML"

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrProcess.Id & "'"

                    Dim lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrRecordset2.EOF Then
                        lrProcess.Text = lrRecordset2("ProcessText").Data
                    End If
#End Region

                    Call lrProcess.DisplayAndAssociate()
                    Me.zrPage.UMLDiagram.Process.Add(lrProcess)
                Else
                    lrProcess = lrActualProcess
                End If
#End Region

                '------------------------------------------
                'Link the Actor to the associated Process
                '------------------------------------------
                Dim lo_link As DiagramLink
                lo_link = Me.Diagram.Factory.CreateDiagramLink(lrActor.Shape, lrProcess.Shape)
                lo_link.Tag = lrRecordset.CurrentFact

                lrRecordset.MoveNext()
            End While
#End Region

#Region "Process to Process Participation Relations"
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrProcess1, lrProcess2 As UCD.Process

            While Not lrRecordset.EOF

#Region "Process 1"
                lrFactDataInstance = lrRecordset("Process1")
                lrProcess1 = lrFactDataInstance.CloneUCDProcess(arPage)

                '-------------------------------------------------------------------------------
                'Check to see if the Shape for the Actor has already been loaded onto the Page
                '-------------------------------------------------------------------------------
                Dim lrExistingProcess = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrProcess1.Id)
                If lrExistingProcess IsNot Nothing Then
                    lrProcess1 = lrExistingProcess
                Else
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrProcess1.Id & "'"

                    Dim lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    If Not lrRecordset.EOF Then
                        lrProcess1.Text = lrRecordset2("ProcessText").data
                    End If

                    lrProcess1.DisplayAndAssociate()
                    Me.zrPage.UMLDiagram.Process.AddUnique(lrProcess1)

                End If
#End Region

#Region "Process 2"
                lrFactDataInstance = lrRecordset("Process2")
                lrProcess2 = lrFactDataInstance.CloneUCDProcess(arPage)

                lrExistingProcess = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrProcess2.Id)

                If lrExistingProcess IsNot Nothing Then
                    lrProcess2 = lrExistingProcess
                Else

#Region "Process Text - Get from CMML"

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrProcess2.Id & "'"

                    Dim lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrRecordset2.EOF Then
                        lrProcess2.Text = lrRecordset2("ProcessText").Data
                    End If
#End Region

                    Call lrProcess2.DisplayAndAssociate()
                    Me.zrPage.UMLDiagram.Process.AddUnique(lrProcess2)
                End If
#End Region

                '------------------------------------------
                'Link the Actor to the associated Process
                '------------------------------------------
                Dim lo_link As DiagramLink
                lo_link = Me.Diagram.Factory.CreateDiagramLink(lrProcess1.Shape, lrProcess2.Shape)
                lo_link.Tag = lrRecordset.CurrentFact

                lrRecordset.MoveNext()
            End While
#End Region

            'System Boundary - Wrap around Processes
#Region "System Boundary - Wrap around Processes"
            If Me.zrPage.UMLDiagram.Process.Count > 0 Then
                Dim larMinProcessX = (From Process In Me.zrPage.UMLDiagram.Process
                                      Select Process.X).Min
                Dim larMinProcessY = (From Process In Me.zrPage.UMLDiagram.Process
                                      Select Process.Y).Min
                Dim larMaxProcessX = (From Process In Me.zrPage.UMLDiagram.Process
                                      Select Process.X).Max
                Dim larMaxProcessY = (From Process In Me.zrPage.UMLDiagram.Process
                                      Select Process.Y).Max
                Me.zo_containernode.Move(larMinProcessX - 10, larMinProcessY)
                Me.zo_containernode.Resize(larMaxProcessX - larMinProcessX, larMaxProcessY - larMinProcessY)
            End If

            For Each lrProcess In Me.zrPage.UMLDiagram.Process
                Me.zo_containernode.Add(lrProcess.Shape)
            Next
#End Region

            If IsSomething(Me.zrPage.UMLDiagram.PocessToProcessRelationFTI) Then
                For Each lrFactInstance In Me.zrPage.UMLDiagram.ActorToProcessParticipationRelationFTI.Fact
                    For Each lrFactDataInstance In lrFactInstance.Data

                    Next
                Next
            End If

            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Me.zrPage.FormLoaded = False
        End Try

    End Sub

    Sub reset_node_and_link_colors()

        Dim liInd As Integer = 0

        '------------------------------------------------------------------------------------
        'Reset the border colors of the ShapeNodes (to what they were before being selected
        '------------------------------------------------------------------------------------
        For liInd = 1 To Diagram.Nodes.Count
            Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                Case Is = pcenumConceptType.Actor
                    If Diagram.Nodes(liInd - 1).Selected Then
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                    Else
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                    End If

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
                        Case Is = pcenumRoleConstraintType.ExclusionConstraint,
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

    Sub DropActorAtPoint(ByRef arActor As UCD.Actor, ByVal aoPointF As PointF)

        arActor.X = aoPointF.X
        arActor.Y = aoPointF.Y

        If Not Me.zrPage.UMLDiagram.Actor.Exists(AddressOf arActor.Equals) Then
            '--------------------------------------------------
            'The EntityType is not already within the ORMModel
            '  so add it.
            '--------------------------------------------------
            Me.zrPage.UMLDiagram.Actor.Add(arActor)
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
            lrTypeOfRelation = pcenumCMMLRelations.CoreActorToProcessParticipationRelation
            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            Dim lsSQLString As String = ""
            lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
            lsSQLString &= " (Actor, Process, Data)"
            lsSQLString &= " VALUES ("
            lsSQLString &= "'" & lo_first_entity.Name & "'"
            lsSQLString &= ",'" & lo_second_entity.Name & "'"
            lsSQLString &= ",''"
            lsSQLString &= ")"

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)

            '----------------------------------
            'Clone an instance of the Fact
            '----------------------------------
            Me.zrPage.UMLDiagram.ActorToProcessParticipationRelationFTI.AddFact(lrFact)

        ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            Dim lsSQLString As String = ""
            lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
            lsSQLString &= " (Process1, Process2, Data)"
            lsSQLString &= " VALUES ("
            lsSQLString &= "'" & lo_first_entity.Name & "'"
            lsSQLString &= ",'" & lo_second_entity.Name & "'"
            lsSQLString &= ",''"
            lsSQLString &= ")"

            '----------------------------------
            'Create the Fact within the Model
            '----------------------------------
            lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)

            '----------------------------------
            'Add the Fact to the FactTypeInstance
            '----------------------------------
            Me.zrPage.UMLDiagram.PocessToProcessRelationFTI.AddFact(lrFact)

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
            lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
            lsSQLString &= " WHERE Actor = '" & lo_first_entity.Name & "'"
            lsSQLString &= "   AND Process = '" & lo_second_entity.Name & "'"

            Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
            Dim lsSQLString As String = ""
            lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
            lsSQLString &= " WHERE Process1 = '" & lo_first_entity.Name & "'"
            lsSQLString &= "   AND Process2 = '" & lo_second_entity.Name & "'"

            Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        End If

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Try
            '-------------------------------------------------------
            'ORM Verbalisation
            '-------------------------------------------------------
            Dim lrToolboxForm As frmToolboxORMVerbalisation
            lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
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

            Try
                Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
                    Case Is = pcenumConceptType.ProcessProcessLink
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_ProcessLink
                End Select
            Catch ex As Exception
            End Try

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
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

        Try

            If TypeOf (e.Node) Is MindFusion.Diagramming.ContainerNode Then

                Dim lrSystemBoundary = e.Node

                For Each lrProcessShape In lrSystemBoundary.SubordinateGroup.AttachedNodes
                    Dim lrProcess As UCD.Process = lrProcessShape.Tag
                    Call lrProcess.Move(lrProcessShape.Bounds.X, lrProcessShape.Bounds.Y, True)
                Next

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
                    Case Is = pcenumConceptType.Class,
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
                '            lrRole.Id = pcenumUCD.Actor.ToString
                '        Case Is = pcenumConceptType.Process
                '            lrRole.Id = pcenumUCD.Process.ToString
                '    End Select
                '    lrRole.Name = lrRole.Id
                '    lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
                '    lrFactDataInstance.Data = lrORMObject.Data
                '    lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.Equals)
                '    lrFactDataInstance.X = e.Node.Bounds.X
                '    lrFactDataInstance.Y = e.Node.Bounds.Y
                'Next
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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

        Me.zrPage.SelectedObject.AddUnique(e.Node.Tag)

    End Sub

    Private Sub UseCaseDiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        Try
            Dim lrActor As UCD.Actor
            Dim lrProcess As UCD.Process

            Select Case e.Node.Tag.GetType
                Case Is = GetType(UCD.Actor)

                Case Is = GetType(UCD.Process)

                    lrProcess = e.Node.Tag

                    'Check Processes for duplicate Process Text.
                    If Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id <> lrProcess.Id And x.Text = lrProcess.Text) IsNot Nothing Then
                        If e.NewText = e.OldText Then Exit Sub
                        MsgBox("A Process with the text, '" & e.NewText & "', already exists on the Page.", MsgBoxStyle.Exclamation, "Process Text Conflict")
                        lrProcess.Shape.Text = e.OldText
                        Exit Sub
                    End If

                    Call lrProcess.SetProcessText(e.NewText)

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

    Private Sub IncludesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncludesToolStripMenuItem.Click

        Me.IncludesToolStripMenuItem.Checked = Not Me.IncludesToolStripMenuItem.Checked
        Me.ExtendsToolStripMenuItem.Checked = Not Me.IncludesToolStripMenuItem.Checked

        Dim lo_link As DiagramLink = Me.Diagram.Selection.Items(0)
        lo_link.Text = "<Includes>"



    End Sub

    Private Sub ExtendsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtendsToolStripMenuItem.Click

        Me.ExtendsToolStripMenuItem.Checked = Not Me.ExtendsToolStripMenuItem.Checked
        Me.IncludesToolStripMenuItem.Checked = Not Me.ExtendsToolStripMenuItem.Checked

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

        Call Me.reset_node_and_link_colors()

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
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
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

        Call frmMain.LoadToolboxModelDictionary(True)

    End Sub


    Private Sub ContextMenuStrip_Actor_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Actor.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_actor As New UCD.Actor


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
        Me.MorphVector.Clear()
        Dim lrMorphVector As New tMorphVector(lr_actor.X, lr_actor.Y, 0, 0, 40)
        Me.MorphVector.Add(lrMorphVector)

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
        larPage_list = prApplication.CMML.getUseCaseDiagramPagesForActor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.MenuItemUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.UMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.PageId)
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
        larPage_list = prApplication.CMML.getORMDiagramPagesForActor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem

            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, 
            '  they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.ORMModel,
                                                               Nothing,
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
        larPage_list = prApplication.CMML.getDataFlowDiagramPagesForActor(lr_actor)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.MenuOptionDataFlowDiagramActor.DropDownItems.Add(lr_page.Name)

            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.DataFlowDiagram,
                                                               Nothing,
                                                               lr_page.PageId)

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


        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            Dim lrFactDataInstance As New Object

            lr_page = lr_enterprise_view.Tag

            Select Case liConceptType
                Case Is = pcenumConceptType.Actor
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance
                                       From Fact In FactType.Fact
                                       From RoleData In Fact.Data
                                       Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                                  And RoleData.Data = lrEntity.Name
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                Case Is = pcenumConceptType.Process
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance
                                       From Fact In FactType.Fact
                                       From RoleData In Fact.Data
                                       Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Process.ToString _
                                       And RoleData.Data = lrEntity.Name
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
            End Select

            Dim lrMorphVector = New tMorphVector(Me.zrPage.SelectedObject(0).Shape.bounds.X, Me.zrPage.SelectedObject(0).Shape.bounds.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphVector.Add(lrMorphVector)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    ''' <summary>
    ''' 20220614-Looking very good. I am very happy with the Morphing mechanism implemented here. There may be more elegant ways of setting up the MorphVector, but the actual morphing is very good.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub morph_to_ORM_diagram(ByVal sender As Object, ByVal e As EventArgs)

        '-----------------------------------------------
        'Get the Menu that just called this procedure.
        '-----------------------------------------------
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
        If Me.zrPage.SelectedObject.Count > 1 Then Exit Sub

        Try
            '----------------------------------------------------------
            'Diagram1 is on the HiddenDiagramView
            '  Is a MindFusion Diagram on which the morphing is done.
            '----------------------------------------------------------
            Me.DiagramHidden.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            Dim lrActor As New UCD.Actor
            lrActor = Me.zrPage.SelectedObject(0)

            Dim lrShapeNode As ShapeNode

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = item.Tag
                prApplication.WorkingPage = lr_enterprise_view.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lrPage As New FBM.Page(lr_enterprise_view.Tag.Model)
                lrPage = lr_enterprise_view.Tag
                Dim lrEntityTypeInstanceList = From EntityTypeInstance In lrPage.EntityTypeInstance
                                               Where EntityTypeInstance.Id = lrActor.Data
                                               Select New FBM.EntityTypeInstance(lrPage.Model,
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
                lrEntityTypeInstance = lrPage.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)


                If lrPage.FormLoaded Then
                    lrShapeNode = New ShapeNode(lrEntityTypeInstance.Shape)
                    Me.MorphVector(0).TargetText = lrEntityTypeInstance.Id
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, lrEntityTypeInstance.Shape.Bounds.Width, lrEntityTypeInstance.Shape.Bounds.Height)
                    lrShapeNode.Image = lrActor.Shape.Image
                Else
                    lrShapeNode = lrActor.Shape.Clone(True)
                    lrShapeNode.Shape = Shapes.RoundRect
                    lrShapeNode.HandlesStyle = HandlesStyle.Invisible
                    Me.MorphVector(0).TargetText = lrActor.Data
                    Me.MorphVector(0).EndSize = New Rectangle(0, 0, 20, 15)
                    lrShapeNode.Resize(20, 15)
                End If

                lrShapeNode.Font = New System.Drawing.Font("Arial", 10)
                Dim lrStringFormat = New StringFormat
                lrStringFormat.Alignment = StringAlignment.Center
                lrStringFormat.LineAlignment = StringAlignment.Center
                lrShapeNode.TextFormat = lrStringFormat

                If lrPage.DiagramView IsNot Nothing Then
                    Me.MorphVector(0).TargetZoomFactor = lrPage.DiagramView.ZoomFactor
                Else
                    Me.MorphVector(0).TargetZoomFactor = My.Settings.DefaultPageZoomFactor
                End If

                Me.MorphVector(0).StartPoint = New Point(lrActor.Shape.Bounds.X, lrActor.Shape.Bounds.Y)
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, lrActor.Shape.Bounds.Width, lrActor.Shape.Bounds.Height)
                Me.MorphVector(0).Shape = lrShapeNode
                Me.DiagramHidden.Nodes.Add(Me.MorphVector(0).Shape)
                Me.DiagramHidden.Invalidate()

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True

                Me.MorphVector(0).EndPoint = New Point(lrEntityTypeInstance.X, lrEntityTypeInstance.Y)
                Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view

                Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
                Me.MorphStepTimer.Start()
                Me.MorphTimer.Start()

            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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

        Dim lrProcess As New UCD.Process
        lrProcess = Me.zrPage.SelectedObject(0)

        lr_shape_node = Me.DiagramHidden.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, 70, 5) ' lr_shape_node.Shape = Shapes.RoundRect
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = lrProcess.Name
        lr_shape_node.Pen.Color = Color.Black
        lr_shape_node.Visible = True
        lr_shape_node.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

        Me.morph_shape = lr_shape_node

        Me.DiagramHidden.Invalidate()


        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag

            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the ValueType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Dim lrMorphVector = New tMorphVector(Me.zrPage.SelectedObject(0).Shape.Bounds.X, Me.zrPage.SelectedObject(0).Shape.Bounds.Y, 5, 5, 40)
            Me.MorphVector.Add(lrMorphVector)
            Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()

        End If

    End Sub

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)


        Try
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
            lr_shape_node.Image = My.Resources.CMML.actor
            lr_shape_node.Visible = True

            Me.morph_shape = lr_shape_node

            Me.DiagramHidden.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = item.Tag
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
                prApplication.WorkingPage = lr_enterprise_view.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
                lr_page = lr_enterprise_view.Tag
                Dim lrActor = From FactType In lr_page.FactTypeInstance
                              From Fact In FactType.Fact
                              From RoleData In Fact.Data
                              Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString
                              Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                Dim lrFactDataInstance As New Object
                For Each lrFactDataInstance In lrActor
                    Exit For
                Next

                Dim lrMorphVector = New tMorphVector(Me.zrPage.SelectedObject(0).Shape.Bounds.X, Me.zrPage.SelectedObject(0).Shape.Bounds.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
                Me.MorphVector.Add(lrMorphVector)
                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

        End Try

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Me.MorphTimer.Enabled = False
        Me.MorphTimer.Stop()
        Me.DiagramHidden.Nodes.Clear()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lrPoint As New Point
        Dim lrRectangle As New Rectangle
        Dim lrMorphVector As tMorphVector

        Try
            For Each lrMorphVector In Me.MorphVector
                lrPoint = lrMorphVector.getNextMorphVectorStepPoint

                Me.HiddenDiagramView.ZoomFactor = Me.MorphVector(0).InitialZoomFactor + ((Me.MorphVector(0).VectorStep / Me.MorphVector(0).VectorSteps) * (Me.MorphVector(0).TargetZoomFactor - Me.MorphVector(0).InitialZoomFactor))

                lrMorphVector.Shape.Move(lrPoint.X, lrPoint.Y)

                lrRectangle = lrMorphVector.getNextMorphVectorRectangle
                lrMorphVector.Shape.Resize(lrRectangle.Width, lrRectangle.Height)

                If lrMorphVector.VectorStep > lrMorphVector.VectorSteps / 2 Then
                    Select Case lrMorphVector.TargetShape
                        Case Is = pcenumTargetMorphShape.Circle
                            lrMorphVector.Shape.Shape = Shapes.Ellipse
                            lrMorphVector.Shape.Image = My.Resources.ORMShapes.Blank
                            lrMorphVector.Shape.Text = lrMorphVector.TargetText
                        Case Else
                            lrMorphVector.Shape.Shape = Shapes.RoundRect
                            lrMorphVector.Shape.Image = Nothing
                            lrMorphVector.Shape.Text = lrMorphVector.TargetText
                    End Select
                End If
            Next

            Me.DiagramHidden.Invalidate()

            '20220614-This is the best version of morphing yet.
            If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
                Me.MorphStepTimer.Stop()
                Me.MorphStepTimer.Enabled = False
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphStepTimer.Tag 'Me.MorphVector(0).EnterpriseTreeView.TreeNode
                Call frmMain.zfrmModelExplorer.LoadSelectedPage(Me.MorphVector(0).ModelElementId)
                Call Me.HiddenDiagramView.SendToBack()
            End If

        Catch ex As Exception

            Me.MorphStepTimer.Stop()
            Me.DiagramView.BringToFront()
            Me.Diagram.Invalidate()

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

        End Try


    End Sub

    Private Sub frm_UseCaseModel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        Try
            If IsSomething(Me.zoTreeNode) Then
                If IsSomething(frmMain.zfrmModelExplorer) Then
                    frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
                End If
            End If

            If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
                frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
            End If

            If IsSomething(Me.zrPage) Then
                Me.zrPage.SelectedObject.Clear()
            End If

            Call Me.SetToolbox()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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
        Dim lr_process As New UCD.Process


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
        Dim lrMorphVector = New tMorphVector(lr_process.Shape.Bounds.X, lr_process.Shape.Bounds.Y, 0, 0, 40)
        Me.MorphVector.Add(lrMorphVector)

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
        larPage_list = prApplication.CMML.getDataFlowDiagramPagesForProcess(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            lo_menu_option = Me.DFDToolStripMenuItem.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.DataFlowDiagram,
                                                               Nothing,
                                                               lr_page.PageId)
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
        larPage_list = prApplication.CMML.getStateTransitionDiagramPagesForProcess(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            lo_menu_option = Me.ToolStripMenuItemStateTransitionDiagram.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageSTD,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.StateTransitionDiagram,
                                                               Nothing,
                                                               lr_page.PageId)

            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.morph_to_StateTransitionDiagram
        Next

    End Sub

    Private Sub frm_UseCaseModel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF

        prApplication.WorkingPage = Me.zrPage

        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        Dim loNode = Diagram.GetNodeAt(lo_point)

        If IsSomething(loNode) Then
            Me.zrPage.SelectedObject.AddUnique(loNode.Tag)
        Else
            '---------------------------------------------------
            'MouseDown is on canvas (not on object).
            'If any objects are already highlighted as blue, 
            '  then change the outline to black/originalcolour
            '---------------------------------------------------        
            Me.zrPage.SelectedObject.Clear()

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then

                Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                ' And you pass it to the PropertyGrid,
                ' via its BrowsableAttributes property :
                lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

            End If

            Call Me.reset_node_and_link_colors()

            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

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
        'prApplication.WorkingPage = Me.zrPage

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
        '    '        prApplication.workingpage.MultiSelectionPerformed = True

        '    '        Select Case loNode.Tag.ConceptType
        '    '            Case Is = pcenumConceptType.role
        '    '                '----------------------------------------------------------------------
        '    '                'This stops a Role AND its FactType being selected at the same time
        '    '                '----------------------------------------------------------------------
        '    '                prApplication.workingpage.SelectedObject.Remove(loNode.Tag.FactType)
        '    '        End Select

        '    '        Exit Sub
        '    '    Else
        '    '        If prApplication.workingpage.MultiSelectionPerformed Then
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
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
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

        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub

    Private Sub Diagram_NodeDeselected(sender As Object, e As NodeEventArgs) Handles Diagram.NodeDeselected

        Try

            If IsSomething(e.Node) Then
                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.Actor
                        Dim lrActor As UCD.Actor = e.Node.Tag
                        Call lrActor.NodeDeselected()
                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageToolStripMenuItem.Click

        Dim lsSQLQuery As String = ""

        Try
            With New WaitCursor

                ''---------------------------------------------------------
                ''Get the Process represented by the (selected) Process
                ''---------------------------------------------------------
                Dim lrProcess As UML.Process = Me.Diagram.Selection.Items(0).Tag

                '-------------------------------------------------------------------------
                'Remove the Process from the Page
                '---------------------------------
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & lrProcess.Name & "'"

                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim larLinkToRemove As New List(Of DiagramLink)

                For Each lrLink In lrProcess.Shape.IncomingLinks
                    larLinkToRemove.Add(lrLink)
                Next

                For Each lrLink In lrProcess.Shape.OutgoingLinks
                    larLinkToRemove.Add(lrLink)
                Next

                For Each lrLink In larLinkToRemove
                    Me.Diagram.Links.Remove(lrLink)
                Next

                '----------------------------------------------------------------------------------------------------------
                'Remove the Process that represents the Process from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Diagram.Nodes.Remove(lrProcess.Shape)
                Me.zrPage.UMLDiagram.Process.Remove(lrProcess)

            End With

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            Me.Cursor = Cursors.Default

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class