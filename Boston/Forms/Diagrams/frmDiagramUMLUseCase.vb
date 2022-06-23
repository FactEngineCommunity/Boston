Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Threading
Imports System.IO

Public Class frmDiagrmUMLUseCase

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing

    Private zo_containernode As ContainerNode

    Private MorphVector As New List(Of tMorphVector)

    Public Shadows Sub BringToFront(Optional asSelectModelElementId As String = Nothing)

        Call MyBase.BringToFront()

        'If asSelectModelElementId IsNot Nothing Then
        '    Me.Diagram.Selection.Items.Clear()

        '    Dim lrModelElement As Object = Me.zrPage.GetAllPageObjects.Find(Function(x) x.Id = asSelectModelElementId)
        '    If lrModelElement IsNot Nothing Then
        '        lrModelElement.Shape.Selected = True
        '    End If
        'End If

    End Sub

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
            Call Directory.SetCurrentDirectory(Boston.MyPath)
            Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.UsecaseShapeLibrary)
            Dim lo_shape As Shape
            Dim child As New frmToolbox

            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then

                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                child.ShapeListBox.Shapes = lsl_shape_library.Shapes

                For Each lo_shape In child.ShapeListBox.Shapes
                    Select Case lo_shape.DisplayName
                        Case Is = "Actor"
                            lo_shape.Image = My.Resources.CMMLShapes.ActorShapeToolbox
                    End Select
                Next
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

            lsMessage.AppendDoubleLineBreak(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
            lsMessage.AppendDoubleLineBreak(Boston.GetConfigFileLocation)

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

        Dim loPoint As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
        Dim loPointF As PointF = Me.DiagramView.ClientToDoc(New Point(loPoint.X, loPoint.Y))

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim loDraggedNode As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            Dim lrDroppedObject As Object
            Dim lrCMMLModelElement As Object
            lrDroppedObject = loDraggedNode.Tag


            Dim lrToolboxForm As frmToolbox
                lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

                'ModelObjects first.
                If Not (TypeOf (lrDroppedObject) Is MindFusion.Diagramming.Shape) Then
#Region "Model Objects. I.e. Not shapes"
                '---------------------------------------------------------------------
                'DraggedObject is a ModelObject (from the ModelDictionary form etc),
                ' rather than a Shape Object dragged from the Toolbox.
                '---------------------------------------------------------------------
                lrCMMLModelElement = loDraggedNode.Tag

                Select Case lrCMMLModelElement.ConceptType
                    Case Is = pcenumConceptType.Actor 'Actor

                        Dim lrCMMLActor As CMML.Actor = lrCMMLModelElement

                        If Me.zrPage.UMLDiagram.Actor.Find(Function(x) x.Name = lrCMMLActor.Name) IsNot Nothing Then
                            MsgBox("The page already contains the Actor: '" & lrCMMLActor.Name & "'")
                            Exit Sub
                        End If

                        'Page Level
                        Dim lrUCDActor As UCD.Actor = Me.zrPage.DropCMMLActorAtPoint(lrCMMLActor, loPointF, True, pcenumLanguage.UMLUseCaseDiagram)
                        lrUCDActor.CMMLActor = lrCMMLActor 'Leave here because it will not be set otherwise. The above does not set CMMLActor.
                        Call Me.zrPage.loadActorProcessRelationsForCMMLActor(lrCMMLActor)

                        '======================================================================
                        'Save the Page
                        Me.zrPage.Save()

                        Exit Sub

                    Case Is = pcenumConceptType.Process 'Process

                        Dim lrCMMLProcess As CMML.Process = lrCMMLModelElement

                        If Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLProcess.Id) IsNot Nothing Then
                            MsgBox("The page already contains the Process: '" & lrCMMLProcess.Text & "'")
                            Exit Sub
                        End If

                        'Page Level
                        Dim lrUCDProcess As UCD.Process = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Me.zo_containernode,, pcenumLanguage.UMLUseCaseDiagram)
                        lrUCDProcess.CMMLProcess = lrCMMLProcess
                        Call Me.zrPage.loadProcessProcessRelationsForCMMLProcess(lrCMMLProcess)

                        '======================================================================
                        'Save the Page
                        Me.zrPage.Save()

                        Exit Sub
                End Select

                Me.zrPage.DiagramView.Focus()
#End Region
                End If

            If loDraggedNode.Index >= 0 Then
#Region "Shape Toolbox"
                If loDraggedNode.Index < lrToolboxForm.ShapeListBox.ShapeCount Then

                    Select Case lrToolboxForm.ShapeListBox.Shapes(loDraggedNode.Index).Id
                        Case Is = "Actor"
#Region "Actor - Shape Toolbox"
                            '----------------------------------------------------------
                            'Establish a new Actor(EntityType) for the dropped object.
                            '  i.e. Establish the EntityType within the Model as well as creating a new object for the Actor.
                            '----------------------------------------------------------
                            Dim lrEntityType As FBM.EntityType = Me.zrPage.Model.CreateEntityType("New Actor", True, True, False, False)

                            Dim lrCMMLActor As New CMML.Actor(Me.zrPage.Model.UML, lrEntityType.Id, lrEntityType)
                            Me.zrPage.Model.UML.addActor(lrCMMLActor)

                            Dim lrUCDActor As UCD.Actor
                            lrUCDActor = Me.zrPage.DropCMMLActorAtPoint(lrCMMLActor, loPointF, True, pcenumLanguage.UMLUseCaseDiagram)
                            lrUCDActor.CMMLActor = lrCMMLActor 'CodeSafe

                            '20220618-VM-Was. Replaced with the above.
                            'Call Me.zrPage.DropUMLActorAtPoint(lrUCDActor, loPointF)
#End Region
                        Case Is = "Process"

                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New ProcessTest", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Me.zo_containernode,, pcenumLanguage.UMLUseCaseDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess

                    End Select
                End If
#End Region

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

                '----------------------------------------------
                'CMML
                lrActor.CMMLActor = Me.zrPage.Model.UML.Actor.Find(Function(x) x.Name = lrActor.Name)

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
                lrProcess.CMMLProcess = Me.zrPage.Model.UML.Process.Find(Function(x) x.Id = lrProcess.Id)

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

            Me.zo_containernode.Move(100, 20)

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
            Dim larActorName = (From Actor In Me.zrPage.UMLDiagram.Actor
                                Select Actor.Name).ToList

            Dim larCMMLActorProcessRelation = From ActorProcessRelation In Me.zrPage.Model.UML.ActorProcessRelation
                                              Where ActorProcessRelation.Actor IsNot Nothing
                                              Where larActorName.Contains(ActorProcessRelation.Actor.Name)
                                              Select ActorProcessRelation


            For Each lrCMMLActorProcessRelation In larCMMLActorProcessRelation

                lrActor = Me.zrPage.UMLDiagram.Actor.Find(Function(x) x.Name = lrCMMLActorProcessRelation.Actor.Name)
                lrProcess = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLActorProcessRelation.Process.Id)

                If lrActor IsNot Nothing And lrProcess IsNot Nothing Then

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE Actor = '" & lrActor.Name & "'"
                    lsSQLQuery &= " AND Process = '" & lrProcess.Id & "'"

                    lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrUCDActorProcessRelation As UCD.ActorProcessRelation
                    If lrRecordset.EOF Then
                        lrFactInstance = Me.zrPage.UMLDiagram.ActorToProcessParticipationRelationFTI.AddFact(lrCMMLActorProcessRelation.Fact)
                    Else
                        lrFactInstance = lrRecordset.CurrentFact
                    End If

                    lrUCDActorProcessRelation = lrFactInstance.CloneUCDActorProcessRelation(Me.zrPage, lrActor, lrProcess)

                    '------------------------------------------
                    'Link the Processes
                    '------------------------------------------
                    lrUCDActorProcessRelation.Fact = lrFactInstance
                    lrUCDActorProcessRelation.CMMLActorProcessRelation = lrCMMLActorProcessRelation 'Me.zrPage.Model.UML.ProcessProcessRelation.Find(Function(x) x.Process1.Id = lrProcess1.Id And x.Process2.Id = lrProcess2.Id)
                    lrUCDActorProcessRelation.CMMLActorProcessRelation = lrCMMLActorProcessRelation 'Me.zrPage.Model.UML.ActorProcessRelation.Find(Function(x) x.Process1.Id = lrProcess1.Id And x.Process2.Id = lrProcess2.Id)

                    Me.zrPage.UMLDiagram.ActorProcessRelation.Add(lrUCDActorProcessRelation)

                    Dim lo_link As DiagramLink
                    lo_link = Me.Diagram.Factory.CreateDiagramLink(lrActor.Shape, lrProcess.Shape)
                    lrUCDActorProcessRelation.Link = lo_link
                    lo_link.Tag = lrUCDActorProcessRelation

                End If
            Next
#End Region

#Region "Process to Process Participation Relations"

            Dim larProcessId = (From Process In Me.zrPage.UMLDiagram.Process
                                Select Process.Id).ToList

            Dim larCMMLProcessProcessRelation = From ProcessProcessRelation In Me.zrPage.Model.UML.ProcessProcessRelation
                                                Where larProcessId.Contains(ProcessProcessRelation.Process1.Id)
                                                Select ProcessProcessRelation

            Dim lrProcess1, lrProcess2 As UCD.Process

            For Each lrCMMLProcessProcessRelation In larCMMLProcessProcessRelation

                lrProcess1 = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLProcessProcessRelation.Process1.Id)
                lrProcess2 = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLProcessProcessRelation.Process2.Id)

                If lrProcess1 IsNot Nothing And lrProcess2 IsNot Nothing Then

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE Process1 = '" & lrProcess1.Id & "'"
                    lsSQLQuery &= " AND Process2 = '" & lrProcess2.Id & "'"

                    lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrUCDProcessProcessRelation As UCD.ProcessProcessRelation
                    If lrRecordset.EOF Then
                        lrFactInstance = Me.zrPage.UMLDiagram.PocessToProcessRelationFTI.AddFact(lrCMMLProcessProcessRelation.Fact)
                    Else
                        lrFactInstance = lrRecordset.CurrentFact
                    End If

                    lrUCDProcessProcessRelation = lrFactInstance.CloneUCDProcessProcessRelation(Me.zrPage, lrProcess1, lrProcess2)

                    '------------------------------------------
                    'Link the Processes
                    '------------------------------------------                    
                    lrUCDProcessProcessRelation.Fact = lrFactInstance
                    lrUCDProcessProcessRelation.IsExtends = lrCMMLProcessProcessRelation.IsExtends
                    lrUCDProcessProcessRelation.IsIncludes = lrCMMLProcessProcessRelation.IsIncludes
                    lrUCDProcessProcessRelation.CMMLProcessProcessRelation = lrCMMLProcessProcessRelation 'Me.zrPage.Model.UML.ProcessProcessRelation.Find(Function(x) x.Process1.Id = lrProcess1.Id And x.Process2.Id = lrProcess2.Id)
                    lrUCDProcessProcessRelation.IsExtends = lrCMMLProcessProcessRelation.IsExtends

                    Me.zrPage.UMLDiagram.ProcessProcessRelation.Add(lrUCDProcessProcessRelation)

                    Dim lo_link As DiagramLink
                    lo_link = Me.Diagram.Factory.CreateDiagramLink(lrProcess1.Shape, lrProcess2.Shape)
                    lrUCDProcessProcessRelation.Link = lo_link
                    lo_link.Tag = lrUCDProcessProcessRelation
                    If lrUCDProcessProcessRelation.IsExtends Then
                        lo_link.Text = "<<Extends>>"
                    ElseIf lrUCDProcessProcessRelation.IsIncludes Then
                        lo_link.Text = "<<Includes>>"

                    End If

                End If
            Next
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

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Try

            Dim loObject As Object = e.Link.Destination
            Dim lo_dummy_object As New MindFusion.Diagramming.DummyNode(Me.Diagram)
            Dim lrFact As New FBM.Fact
            Dim lrFactInstance As New FBM.FactInstance
            Dim loFirstModelElement As New Object
            Dim loSecondModelElement As New Object

            'CodeSafe - No links from nowhere to nowhere or a link
            If e.Link.Origin.GetType = GetType(MindFusion.Diagramming.DummyNode) Then Exit Sub
            If e.Link.Destination.GetType = GetType(MindFusion.Diagramming.DummyNode) Or e.Link.Destination.GetType = GetType(MindFusion.Diagramming.ContainerNode) Then
                Call Me.Diagram.Links.Remove(e.Link)
                Exit Sub
            End If



            Select Case loObject.GetType.ToString
                Case Is = zo_containernode.GetType.ToString, lo_dummy_object.GetType.ToString
                    Dim lo_pointf = New PointF(e.Link.Origin.Bounds.X + e.Link.Bounds.Width, e.Link.Origin.Bounds.Y + e.Link.Bounds.Height)
                    'dim lr_process_instance = New tprocess_instance
                    'Call drop_process_at_point(pr_process, lo_pointf)
                    'e.Link.Destination = lr_process_instance.shape
            End Select

            Me.Cursor = Cursors.WaitCursor

            loFirstModelElement = e.Link.Origin.Tag
            loSecondModelElement = e.Link.Destination.Tag

            If (loFirstModelElement.ConceptType = pcenumConceptType.Actor) And (loSecondModelElement.ConceptType = pcenumConceptType.Process) Then

                Dim lrCMMLActor As CMML.Actor = loFirstModelElement.CMMLActor
                Dim lrCMMLProcess As CMML.Process = loSecondModelElement.CMMLProcess

                Dim lrCMMLActorProcessRelation = New CMML.ActorProcessRelation(Me.zrPage.Model.UML, lrCMMLActor, lrCMMLProcess)

                Me.zrPage.Model.UML.addActorProcessRelation(lrCMMLActorProcessRelation)

                Me.Diagram.Links.Remove(e.Link)

            ElseIf (loFirstModelElement.ConceptType = pcenumConceptType.Process) And (loSecondModelElement.ConceptType = pcenumConceptType.Process) Then


                Dim lrCMMLProcess1 As CMML.Process = loFirstModelElement.CMMLProcess
                Dim lrCMMLProcess2 As CMML.Process = loSecondModelElement.CMMLProcess

                Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(Me.zrPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                Me.zrPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)

                Me.Diagram.Links.Remove(e.Link)

            End If

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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


        '20220615-Now handled by RemoveFromPage
        'lo_first_entity = e.Link.Origin.Tag
        'lo_second_entity = e.Link.Destination.Tag


        'If (lo_first_entity.ConceptType = pcenumConceptType.Actor) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
        '    Dim lsSQLString As String = ""
        '    lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
        '    lsSQLString &= " WHERE Actor = '" & lo_first_entity.Name & "'"
        '    lsSQLString &= "   AND Process = '" & lo_second_entity.Name & "'"

        '    Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        'ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
        '    Dim lsSQLString As String = ""
        '    lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
        '    lsSQLString &= " WHERE Process1 = '" & lo_first_entity.Name & "'"
        '    lsSQLString &= "   AND Process2 = '" & lo_second_entity.Name & "'"

        '    Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        'End If

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

        Select Case e.Node.Tag.GetType
            Case Is = GetType(UCD.Actor)

                Dim lrActor As UCD.Actor = e.Node.Tag
                Call Me.DiagramView.BeginEdit(lrActor.NameShape.Shape)
            Case Is = GetType(UCD.Process)

                Dim lrProcess As UCD.Process = e.Node.Tag
                Call Me.DiagramView.BeginEdit(lrProcess.Shape)
        End Select


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

                If lrSystemBoundary.SubordinateGroup IsNot Nothing Then
                    For Each lrProcessShape In lrSystemBoundary.SubordinateGroup.AttachedNodes
                        Try
                            Dim lrProcess As UCD.Process = lrProcessShape.Tag
                            Call lrProcess.Move(lrProcessShape.Bounds.X, lrProcessShape.Bounds.Y, True)
                        Catch ex As Exception
                            'Unknown why it is throwing an error
                        End Try

                    Next
                End If
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
        Try

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
            If Me.zrPage.SelectedObject.Count > 0 Then
                Select Case Me.zrPage.SelectedObject(0).ConceptType
                    Case Is = pcenumConceptType.Actor
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Actor
                    Case Is = pcenumConceptType.Process
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Process
                    Case Else
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
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

    Private Sub UseCaseDiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        Dim lsMessage As String

        Try
            Dim lrProcess As UCD.Process

            'CodeSafe
            If Trim(e.NewText) = "" Then Exit Sub

            Select Case e.Node.Tag.GetType
                Case Is = GetType(UML.ActorName)

                    Dim lrUCDActor As UCD.Actor = e.Node.Tag.Actor

                    Dim lrModelElement = Me.zrPage.Model.GetModelObjectByName(e.NewText, True)
                    Dim lbModelElementExists As Boolean = False
                    If lrModelElement IsNot Nothing Then
                        lbModelElementExists = lrUCDActor.CMMLActor.FBMModelElement IsNot lrModelElement
                    End If
                    If lbModelElementExists Or (Me.zrPage.Model.UML.Actor.Find(Function(x) x IsNot lrUCDActor.CMMLActor And LCase(Trim(x.Name)) = LCase(Trim(e.NewText))) IsNot Nothing) Then
                        lsMessage = "You cannot set the name of a Actor as the same as the name of another Actor in the model."
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "A Actor with the name, '" & e.NewText & "', already exists in the model."

                        lrUCDActor.NameShape.Shape.Text = e.OldText
                        MsgBox(lsMessage)
                        Exit Sub
                    End If

                    Call lrUCDActor.CMMLActor.setName(e.NewText)

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
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub IncludesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemIncludes.Click

        Try
            'CodeSafe 
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
            If Me.zrPage.SelectedObject(0).GetType <> GetType(UCD.ProcessProcessRelation) Then Exit Sub

            Dim lrProcessProcessRelation As UCD.ProcessProcessRelation = Me.zrPage.SelectedObject(0)

            Me.ToolStripMenuItemIncludes.Checked = Not Me.ToolStripMenuItemIncludes.Checked
            Me.ToolStripMenuItemExtends.Checked = Not Me.ToolStripMenuItemIncludes.Checked

            Call lrProcessProcessRelation.CMMLProcessProcessRelation.setIsExtends(Not Me.ToolStripMenuItemIncludes.Checked)
            Call lrProcessProcessRelation.CMMLProcessProcessRelation.setIsIncludes(Me.ToolStripMenuItemIncludes.Checked)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ExtendsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemExtends.Click

        Try
            'CodeSafe 
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
            If Me.zrPage.SelectedObject(0).GetType <> GetType(UCD.ProcessProcessRelation) Then Exit Sub

            Dim lrProcessProcessRelation As UCD.ProcessProcessRelation = Me.zrPage.SelectedObject(0)

            Me.ToolStripMenuItemExtends.Checked = Not Me.ToolStripMenuItemExtends.Checked
            Me.ToolStripMenuItemIncludes.Checked = Not Me.ToolStripMenuItemExtends.Checked

            Call lrProcessProcessRelation.CMMLProcessProcessRelation.setIsIncludes(Not Me.ToolStripMenuItemExtends.Checked)
            Call lrProcessProcessRelation.CMMLProcessProcessRelation.setIsExtends(Me.ToolStripMenuItemExtends.Checked)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Call Me.CopyImageToClipboard()

    End Sub

    Sub CopyImageToClipboard()

        Try
            Dim li_rectf As New RectangleF
            li_rectf = Me.Diagram.GetContentBounds(False, True)

            'Dim lo_image_processor As New t_image_processor(Diagram.CreateImage(li_rectf, 100))

            Dim lr_image As Image = Diagram.CreateImage(li_rectf, 100)

            lr_image = Boston.CropImage(lr_image, Color.White, 0)
            lr_image = Boston.CreateFramedImage(lr_image, Color.White, 15)

            Me.Diagram.ShowGrid = False

            Me.Cursor = Cursors.WaitCursor

            Windows.Forms.Clipboard.SetImage(lr_image)

            '---------------------------------
            'Set the grid back to what it was
            '---------------------------------
            Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)
            Dim lrPropertyGridForm As frmToolboxProperties = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            'CodeSafe
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
            If lrPropertyGridForm Is Nothing Then Exit Sub

            If Me.Diagram.Selection.Items.Count > 0 Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                Dim loMiscFilterAttribute1 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
                Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute1, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
                lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

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

        Call prApplication.setWorkingModel(Me.zrPage.Model)

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
            Dim loMenuOption As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            loMenuOption = Me.MenuItemUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.UMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.morph_to_UseCase_diagram
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
            Dim loMenuOption As ToolStripItem

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
                loMenuOption = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lr_page.Name)
                loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                AddHandler loMenuOption.Click, AddressOf Me.morph_to_ORM_diagram
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
            Dim loMenuOption As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            loMenuOption = Me.MenuOptionDataFlowDiagramActor.DropDownItems.Add(lr_page.Name)

            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.DataFlowDiagram,
                                                               Nothing,
                                                               lr_page.PageId)

            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.morph_to_DataFlowDiagram
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
        Dim lrShapeNode As ShapeNode
        lrShapeNode = Me.zrPage.SelectedObject(0).Shape
        lrShapeNode = Me.DiagramHidden.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
        lrShapeNode.Shape = Shapes.Ellipse
        lrShapeNode.Text = Me.zrPage.SelectedObject(0).Name
        lrShapeNode.Transparent = True

        lrShapeNode.Visible = True

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
            lrMorphVector.Shape = lrShapeNode
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

                Dim liHypotenuse As Integer = 40
                Try
                    liHypotenuse = Math.Sqrt(Math.Abs(lrEntityTypeInstance.X - lrActor.Shape.Bounds.X) ^ 2 + Math.Abs(lrEntityTypeInstance.Y - lrActor.Shape.Bounds.Y) ^ 2)
                Catch
                End Try
                Me.MorphVector(0).VectorSteps = Viev.Greater(Viev.Lesser(35, liHypotenuse), 1)


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
        Dim lrShapeNode As ShapeNode
        lrShapeNode = Me.zrPage.SelectedObject(0).Shape

        Dim lrProcess As New UCD.Process
        lrProcess = Me.zrPage.SelectedObject(0)

        lrShapeNode = Me.DiagramHidden.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, 70, 5) ' lrShapeNode.Shape = Shapes.RoundRect
        lrShapeNode.Shape = Shapes.Rectangle
        lrShapeNode.Text = lrProcess.Name
        lrShapeNode.Pen.Color = Color.Black
        lrShapeNode.Visible = True
        lrShapeNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

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
            lrMorphVector.Shape = lrShapeNode
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

            'CodeSafe
            If Me.zrPage.SelectedObject(0).GetType <> GetType(UCD.Actor) Then Exit Sub

            '--------------------------------------------------------------------------------------
            'Paste the selected Actor/EntityType to the HiddenDiagramView (for animated morphing)
            '--------------------------------------------------------------
            Dim lrShapeNode As ShapeNode
            lrShapeNode = Me.zrPage.SelectedObject(0).Shape
            lrShapeNode = Me.DiagramHidden.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
            lrShapeNode.Shape = Shapes.Rectangle
            lrShapeNode.Text = ""
            lrShapeNode.Pen.Color = Color.White
            lrShapeNode.Transparent = True
            lrShapeNode.Image = My.Resources.CMML.actor
            lrShapeNode.Visible = True

            Me.DiagramHidden.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = item.Tag
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
                prApplication.WorkingPage = lr_enterprise_view.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lr_page As FBM.Page = lr_enterprise_view.Tag

                Dim larFactDataInstance = From Page In Me.zrPage.Model.Page
                                          From FactTypeInstance In Page.FactTypeInstance
                                          From FactInstance In FactTypeInstance.Fact
                                          From FactDataInstance In FactInstance.Data
                                          Where Page.Name = lr_page.Name
                                          Where FactTypeInstance.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString
                                          Where FactDataInstance.Role.Name = "Element"
                                          Where FactDataInstance.Concept.Symbol = Me.zrPage.SelectedObject(0).Name
                                          Select FactDataInstance

                Dim lrActor = larFactDataInstance.First.CloneActor(Me.zrPage)

                Me.MorphVector(0).Shape = lrShapeNode
                Me.MorphVector(0).EndPoint = New Point(lrActor.X, lrActor.Y)
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.zrPage.SelectedObject(0).Shape.Bounds.Width, Me.zrPage.SelectedObject(0).Shape.Bounds.Height)
                Me.MorphVector(0).EndSize = New Rectangle(0, 0, Me.zrPage.SelectedObject(0).Shape.Bounds.Width, Me.zrPage.SelectedObject(0).Shape.Bounds.Height)
                Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view
                Me.MorphVector(0).TargetImage = Me.zrPage.SelectedObject(0).Shape.Image
                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True

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

            Call Me.HiddenDiagramView.SendToBack()

        End Try

    End Sub

    Public Sub morphToUseCaseDiagram_Process(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)


        Try
            Me.DiagramHidden.Nodes.Clear()
            Call Me.DiagramView.SendToBack()
            Call Me.HiddenDiagramView.BringToFront()

            Dim lrStartProcess As UML.Process = Me.zrPage.SelectedObject(0)

            '--------------------------------------------------------------------------------------
            'Paste the selected Actor/EntityType to the HiddenDiagramView (for animated morphing)
            '--------------------------------------------------------------
            Dim lrShapeNode As ShapeNode
            lrShapeNode = Me.zrPage.SelectedObject(0).Shape
            lrShapeNode = Me.DiagramHidden.Factory.CreateShapeNode(lrShapeNode.Bounds.X, lrShapeNode.Bounds.Y, lrShapeNode.Bounds.Width, lrShapeNode.Bounds.Height)
            lrShapeNode.Shape = Shapes.Ellipse
            lrShapeNode.Text = lrStartProcess.Text
            lrShapeNode.Pen.Color = Color.Black
            lrShapeNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
            lrShapeNode.Visible = True
            Me.DiagramHidden.Nodes.Add(lrShapeNode)

            Me.DiagramHidden.Invalidate()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Dim lr_enterprise_view As tEnterpriseEnterpriseView
                lr_enterprise_view = item.Tag
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
                prApplication.WorkingPage = lr_enterprise_view.Tag

                '------------------------------------------------------------------
                'Get the X,Y co-ordinates of the Actor/EntityType being morphed
                '------------------------------------------------------------------
                Dim lr_page As FBM.Page = lr_enterprise_view.Tag

                Dim lrFactDataInstance = From Page In Me.zrPage.Model.Page
                                         From FactTypeInstance In Page.FactTypeInstance
                                         From FactInstance In FactTypeInstance.Fact
                                         From FactDataInstance In FactInstance.Data
                                         Where Page.Name = lr_page.Name
                                         Where FactTypeInstance.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString
                                         Where FactDataInstance.Role.Name = "Element"
                                         Where FactDataInstance.Concept.Symbol = Me.zrPage.SelectedObject(0).Id
                                         Select FactDataInstance

                Dim lrProcess = lrFactDataInstance.First.CloneProcess(Me.zrPage)

                Me.MorphVector(0).Shape = lrShapeNode
                Me.MorphVector(0).StartPoint = New Point(lrStartProcess.Shape.Bounds.X, lrStartProcess.Shape.Bounds.Y)
                Me.MorphVector(0).EndPoint = New Point(lrProcess.X, lrProcess.Y)
                Me.MorphVector(0).StartSize = New Rectangle(0, 0, Me.zrPage.SelectedObject(0).Shape.Bounds.Width, Me.zrPage.SelectedObject(0).Shape.Bounds.Height)
                Me.MorphVector(0).EndSize = New Rectangle(0, 0, Me.zrPage.SelectedObject(0).Shape.Bounds.Width, Me.zrPage.SelectedObject(0).Shape.Bounds.Height)
                Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view
                Me.MorphVector(0).TargetShape = pcenumTargetMorphShape.Circle
                Me.MorphVector(0).TargetText = lrStartProcess.Text

                Dim liHypotenuse As Integer = 40
                Try
                    liHypotenuse = Math.Sqrt(Math.Abs(lrProcess.X - lrStartProcess.Shape.Bounds.X) ^ 2 + Math.Abs(lrProcess.Y - lrStartProcess.Shape.Bounds.Y) ^ 2)
                Catch
                End Try
                Me.MorphVector(0).VectorSteps = Viev.Greater(Viev.Lesser(35, liHypotenuse), 1)

                Me.MorphTimer.Enabled = True
                Me.MorphStepTimer.Enabled = True

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

            Call Me.HiddenDiagramView.SendToBack()

        End Try

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Me.MorphTimer.Enabled = False
        Me.MorphTimer.Stop()
        Me.DiagramHidden.Nodes.Clear()

        'CodeSafe
        Me.MorphStepTimer.Stop()
        Me.MorphStepTimer.Enabled = False
        Call Me.HiddenDiagramView.SendToBack()

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

                            If lrMorphVector.TargetImage IsNot Nothing Then
                                lrMorphVector.Shape.Image = lrMorphVector.TargetImage
                            Else
                                lrMorphVector.Shape.Image = Nothing
                            End If

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

    Private Sub createUseCaseDiagramPageForProcess(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrPage As FBM.Page

        With New WaitCursor

            Try
                Dim loToolStripItem As ToolStripItem = CType(sender, ToolStripItem)
                Dim lsPageName As String = "UCD-New Use Case Diagram Page"
                If loToolStripItem.Tag IsNot Nothing Then
                    lsPageName = "UML-UCD-" & CType(loToolStripItem.Tag, UCD.Process).Text
                    lsPageName = Viev.Strings.MakeCapCamelCase(lsPageName.Substring(0, lsPageName.Length))
                End If
                lsPageName = Me.zrPage.Model.CreateUniquePageName(lsPageName, 0)

#Region "From Core"
                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString,
                                               pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreUMLUseCaseDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the StateTransitionDiagram.
                '----------------------------------------------------
                Boston.WriteToStatusBar("Creating the Page.")
                lrPage = lrCorePage.Clone(prApplication.WorkingModel, True, True, , True) 'Assigns new PageId
                lrPage.Name = lsPageName
                lrPage.Language = pcenumLanguage.UMLUseCaseDiagram
#End Region

                lrPage.Loaded = True
                Me.zrPage.Model.Page.Add(lrPage)
                lrPage.Save(True, True)

                Me.zrPage.Model.AllowCheckForErrors = True
                frmMain.Cursor = Cursors.Default

                Dim lrEnterpriseView As tEnterpriseEnterpriseView = Nothing

                lrEnterpriseView = frmMain.zfrmModelExplorer.AddExistingPageToModel(lrPage, lrPage.Model, lrPage.Model.TreeNode, True)

                MsgBox("Added the new ORM Diagram Page, '" & lrPage.Name & "' to the Model.")

                If loToolStripItem.Tag IsNot Nothing Then
                    Dim lrUCDProcess As UCD.Process = loToolStripItem.Tag
                    Call lrPage.DropCMMLProcessAtPoint(lrUCDProcess.CMMLProcess, New PointF(80, 40), Nothing, True, pcenumLanguage.UMLUseCaseDiagram)

                    Dim liInd = 20
                    For Each lrCMMLActor In lrUCDProcess.CMMLProcess.getParticipatingActors()
                        Call lrPage.DropCMMLActorAtPoint(lrCMMLActor, New PointF(20, liInd), True, pcenumLanguage.UMLUseCaseDiagram)
                        liInd += 10
                    Next
                    Dim lrToolstripItem As New tDummyToolStripItem(lrEnterpriseView)
                    Call Me.morphToUseCaseDiagram_Process(lrToolstripItem, lrEnterpriseView)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End With

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
        Dim lrUCDProcess As New UCD.Process

        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        Call prApplication.setWorkingModel(Me.zrPage.Model)
        '-------------------------------------
        'Check that selected object is Actor
        '-------------------------------------
        If lrUCDProcess.GetType Is Me.zrPage.SelectedObject(0).GetType Then
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

        lrUCDProcess = Me.zrPage.SelectedObject(0)

        lr_model = lrUCDProcess.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for Morphing
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Dim lrMorphVector = New tMorphVector(lrUCDProcess.Shape.Bounds.X, lrUCDProcess.Shape.Bounds.Y, 0, 0, 40)
        Me.MorphVector.Clear()
        Me.MorphVector.Add(lrMorphVector)

        '----------------------------------------------------------------------------
        'Clear the list of Diagram Type menu options that may relate to the Process
        '--------------------------------------------------------------------------
        Me.DFDToolStripMenuItem.DropDownItems.Clear()
        Me.ToolStripMenuItemStateTransitionDiagram.DropDownItems.Clear()
        Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Clear()

        Dim loMenuOption As ToolStripItem

        '------------------------------------------------------------------------------
        'Load the Use Case Diagrams that relate to the EntityType as selectable menuOptions
        '--------------------------------------------------------
#Region "UCDs"
        larPage_list = prApplication.CMML.getUseCaseDiagramPagesForProcess(lrUCDProcess, Me.zrPage)

        For Each lr_page In larPage_list
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            loMenuOption = Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.UMLUseCaseDiagram,
                                                               Nothing,
                                                               lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.morphToUseCaseDiagram_Process
        Next

        '----------------------------------------------------------------------------------
        'Provide and option for the User to create a UCD Page for the current Process.
        Dim lsMessage As String = "Add an &Use Case Diagram for the selected Entity Type."
        loMenuOption = Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Add(lsMessage, My.Resources.MenuImages.UML_UseCase16x16)
        loMenuOption.Tag = lrUCDProcess
        AddHandler loMenuOption.Click, AddressOf Me.createUseCaseDiagramPageForProcess
#End Region

        '------------------------------------------------------------------------------
        'Load the DFDDiagrams that relate to the EntityType as selectable menuOptions
        '--------------------------------------------------------
#Region "DFDs"
        larPage_list = prApplication.CMML.getDataFlowDiagramPagesForProcess(lrUCDProcess)

        For Each lr_page In larPage_list
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            loMenuOption = Me.DFDToolStripMenuItem.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.DataFlowDiagram,
                                                               Nothing,
                                                               lr_page.PageId)
            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.morph_to_DataFlowDiagram
        Next
#End Region

        '------------------------------------------------------------------------------
        'Load the STDDiagrams that relate to the EntityType as selectable menuOptions
        '--------------------------------------------------------
#Region "STDs"
        larPage_list = prApplication.CMML.getStateTransitionDiagramPagesForProcess(lrUCDProcess)

        For Each lr_page In larPage_list
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            loMenuOption = Me.ToolStripMenuItemStateTransitionDiagram.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageSTD,
                                                               Nothing,
                                                               lr_page.Model.ModelId,
                                                               pcenumLanguage.StateTransitionDiagram,
                                                               Nothing,
                                                               lr_page.PageId)

            loMenuOption.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler loMenuOption.Click, AddressOf Me.morph_to_StateTransitionDiagram
        Next
#End Region

    End Sub


    ''' <summary>
    ''' NB PropertiesGrid set here.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub frm_UseCaseModel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF

        Try
            prApplication.WorkingPage = Me.zrPage

            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            Dim loNode = Diagram.GetNodeAt(lo_point)
            Dim loLink = Diagram.GetLinkAt(lo_point, 1)

            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            Me.zrPage.SelectedObject.Clear()

            If loLink IsNot Nothing Then
                Me.zrPage.SelectedObject.AddUnique(loLink.Tag)
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_ProcessLink


            ElseIf IsSomething(loNode) Then
                loNode = Diagram.GetNodeAt(lo_point)
                Me.zrPage.SelectedObject.AddUnique(loNode.Tag)
                Select Case loNode.GetType
                    Case Is = GetType(UCD.Actor)
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Actor
                    Case Is = GetType(UCD.Process)
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Process
                End Select
                loNode.Selected = True
            Else
                '---------------------------------------------------
                'MouseDown is on canvas (not on object).
                'If any objects are already highlighted as blue, 
                '  then change the outline to black/originalcolour
                '---------------------------------------------------        
                Me.zrPage.SelectedObject.Clear()

                If IsSomething(lrPropertyGridForm) Then

                    Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

                End If

                Call Me.reset_node_and_link_colors()

                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

            End If

#Region "Propery Grid"
            If IsSomething(lrPropertyGridForm) Then
                If IsSomething(loLink) Then
                    Select Case loLink.Tag.GetType
                        Case Is = GetType(UCD.ProcessProcessRelation)
                            Dim lrProcessProcessRelation As UCD.ProcessProcessRelation
                            lrProcessProcessRelation = loLink.Tag
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            Dim loMiscFilterAttribute1 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
                            Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                            Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                            Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute1, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
                            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrProcessProcessRelation
                    End Select

                ElseIf IsSomething(loNode) Then

                    Dim lrModelObject As FBM.ModelObject
                    lrModelObject = loNode.Tag
                    lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                    lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                    Select Case loNode.Tag.GetType
                        Case Is = GetType(UCD.Process)
                            Dim lrProcess As UCD.Process
                            lrProcess = loNode.Tag
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            Dim loMiscFilterAttribute1 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
                            Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                            Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                            Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute1, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
                            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrProcess
                        Case Is = GetType(UCD.Actor)
                            Dim lrActor As UCD.Actor
                            lrActor = loNode.Tag
                            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                            Dim loMiscFilterAttribute1 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
                            Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                            Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                            Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute1, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
                            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                            lrPropertyGridForm.PropertyGrid.SelectedObject = lrActor
                    End Select
                End If
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
                Dim lrProcess As UCD.Process = Me.Diagram.Selection.Items(0).Tag

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

    Private Sub RemoveFromPageToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveFromPageToolStripMenuItem1.Click

        Try
            Dim loProcessRelation = Me.zrPage.SelectedObject(0)

            Select Case loProcessRelation.GetType
                Case Is = GetType(UML.ProcessProcessRelation)
                    Dim lrActorProcessRelation As UML.ActorProcessRelation = loProcessRelation

                    Call lrActorProcessRelation.RemoveFromPage()

                Case Is = GetType(UML.ProcessProcessRelation)
                    Dim lrProcessProcessRelation As UML.ProcessProcessRelation = loProcessRelation

                    Call lrProcessProcessRelation.RemoveFromPage()
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageAndModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageAndModelToolStripMenuItem.Click

        Try
            Dim loProcessRelation = Me.zrPage.SelectedObject(0)

            If loProcessRelation Is Nothing Then Exit Sub

            If MsgBox("Are you sure you want to remove the Relation from the Model?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                Select Case loProcessRelation.GetType
                    Case Is = GetType(UCD.ActorProcessRelation)
                        Dim lrActorProcessRelation As UCD.ActorProcessRelation = loProcessRelation

                        Call lrActorProcessRelation.CMMLActorProcessRelation.RemoveFromModel()

                    Case Is = GetType(UCD.ProcessProcessRelation)
                        Dim lrProcessProcessRelation As UCD.ProcessProcessRelation = loProcessRelation

                        Call lrProcessProcessRelation.CMMLProcessProcessRelation.RemoveFromModel()
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

    Private Sub PropertiesToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem3.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)
            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            Dim lrProcessProcessRelation As UCD.ProcessProcessRelation
            lrProcessProcessRelation = Me.zrPage.SelectedObject(0)
            Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
            Dim loMiscFilterAttribute1 As Attribute = New System.ComponentModel.CategoryAttribute("Name")
            Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
            Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
            Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute1, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
            lrPropertyGridForm.PropertyGrid.SelectedObject = lrProcessProcessRelation

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub RemoveFromPageModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageModelToolStripMenuItem.Click

        Try

            With New WaitCursor
                '---------------------------------------------------------
                'Get the Process represented by the (selected) Process
                '---------------------------------------------------------
                Dim lrProcess As UCD.Process = Me.zrPage.SelectedObject(0)

#Region "Connected Relations - ActorProcess/ProcessProcess"
                Dim larConnectedProcessesProcessRelation = From ProcessProcessRelation In lrProcess.CMMLProcess.CMMLModel.ProcessProcessRelation.ToList
                                                           Where (ProcessProcessRelation.Process1.Id = lrProcess.Id) Or (ProcessProcessRelation.Process2.Id = lrProcess.Id)
                                                           Select ProcessProcessRelation

                For Each lrCMMLConnectedProcessProcessRelation In larConnectedProcessesProcessRelation
                    Call lrCMMLConnectedProcessProcessRelation.RemoveFromModel()
                Next

                Dim larConnectedActorProcessRelation = From ActorProcessRelation In lrProcess.CMMLProcess.CMMLModel.ActorProcessRelation.ToList
                                                       Where ActorProcessRelation.Process.Id = lrProcess.Id
                                                       Select ActorProcessRelation

                For Each lrCMMLConnectedActorProcessRelation In larConnectedActorProcessRelation
                    Call lrCMMLConnectedActorProcessRelation.RemoveFromModel()
                Next
#End Region

                Call lrProcess.CMMLProcess.RemoveFromModel()

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ContextMenuStrip_ProcessLink_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ProcessLink.Opening

        Try
            'CodeSafe 
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
            If Me.zrPage.SelectedObject(0) Is Nothing Then Exit Sub
            If Me.zrPage.SelectedObject(0).GetType <> GetType(UCD.ProcessProcessRelation) Then Exit Sub

            Dim loUCDLink = Me.zrPage.SelectedObject(0)

            Me.ToolStripMenuItemIncludes.Checked = False
            Me.ToolStripMenuItemExtends.Checked = False

            Select Case loUCDLink.GetType
                Case Is = GetType(UCD.ProcessProcessRelation)
                    Dim lrProcessProcessRelation As UCD.ProcessProcessRelation = loUCDLink

                    If lrProcessProcessRelation.IsIncludes And lrProcessProcessRelation.IsExtends Then
                        Me.ToolStripMenuItemIncludes.Checked = True
                        Me.ToolStripMenuItemExtends.Checked = True
                    ElseIf lrProcessProcessRelation.IsIncludes Then
                        Me.ToolStripMenuItemIncludes.Checked = True
                    ElseIf lrProcessProcessRelation.IsExtends Then
                        Me.ToolStripMenuItemExtends.Checked = True
                    End If
                Case Else
                    Me.ToolStripMenuItemIncludes.Enabled = False
                    Me.ToolStripMenuItemExtends.Enabled = False
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub RemoveFromPageToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RemoveFromPageToolStripMenuItem2.Click

        Dim lsSQLQuery As String = ""

        Try
            With New WaitCursor

                ''---------------------------------------------------------
                ''Get the Actor represented by the (selected) Actor
                ''---------------------------------------------------------
                Dim lrActor As UCD.Actor = Me.Diagram.Selection.Items(0).Tag

                '-------------------------------------------------------------------------
                'Remove the Actor from the Page
                '---------------------------------
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & lrActor.Name & "'"

                Call Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim larLinkToRemove As New List(Of DiagramLink)
                For Each lrLink In lrActor.Shape.OutgoingLinks
                    larLinkToRemove.Add(lrLink)
                Next
                For Each lrLink In larLinkToRemove
                    Me.Diagram.Links.Remove(lrLink)
                Next

                '----------------------------------------------------------------------------------------------------------
                'Remove the Actor that represents the Actor from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Diagram.Nodes.Remove(lrActor.NameShape.Shape)
                Me.Diagram.Nodes.Remove(lrActor.Shape)
                Me.zrPage.UMLDiagram.Actor.Remove(lrActor)

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

    Private Sub RemoveFromPageAndModelToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveFromPageAndModelToolStripMenuItem1.Click

        Dim lsMessage As String

        Try
            Dim lrActor As UCD.Actor = Me.zrPage.SelectedObject(0)

            lsMessage = "Are you sure that you want to remove this Actor from the Model?"

            If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) <> MsgBoxResult.Yes Then
                Exit Sub
            End If

            'CodeSafe
            If lrActor.CMMLActor.FBMModelElement.GetAdjoinedRoles(True).Count > 0 Then

                lsMessage = "You cannot remove this actor from the model until its ORM level model element has no associated Fact Types."
                lsMessage.AppendDoubleLineBreak("First go to an ORM view of the model and remove associated Fact Types.")
                MsgBox(lsMessage)

                Exit Sub
            End If

            With New WaitCursor
                '---------------------------------------------------------
                'Get the Actor represented by the (selected) Actor
                '---------------------------------------------------------

#Region "Connected Relations - ActorProcess"

                Dim larConnectedActorProcessRelation = From ActorProcessRelation In lrActor.CMMLActor.Model.ActorProcessRelation.ToList
                                                       Where ActorProcessRelation.Actor.Name = lrActor.Name
                                                       Select ActorProcessRelation

                For Each lrCMMLConnectedActorProcessRelation In larConnectedActorProcessRelation
                    Call lrCMMLConnectedActorProcessRelation.RemoveFromModel()
                Next
#End Region

                Call lrActor.CMMLActor.RemoveFromModel()

            End With

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub PropertiesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem1.Click

        Try
            Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)
            Dim lrPropertyGridForm As frmToolboxProperties = prApplication.GetToolboxForm(frmToolboxProperties.Name)

            'CodeSafe
            If Me.zrPage.SelectedObject.Count = 0 Then Exit Sub
            If lrPropertyGridForm Is Nothing Then Exit Sub

            If Me.Diagram.Selection.Items.Count > 0 Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("Description (Informal)")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4})
                lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.SelectedObject(0)
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class