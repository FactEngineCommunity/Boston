Imports System.Drawing
Imports System.Drawing.Drawing2D

Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Reflection

Public Class frmStateTransitionDiagram

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing

    Private morph_vector As tMorphVector
    Private morph_shape As New ShapeNode
    Private MorphVector As New List(Of tMorphVector)

    Public Shadows Sub BringToFront(Optional asSelectModelElementId As String = Nothing)

        Call MyBase.BringToFront()

    End Sub


    Private Sub frmStateTransitionDiagram_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------                
        Me.zrPage.FormLoaded = False

    End Sub


    Private Sub frmStateTransitionDiagram_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Sub SetupForm()

        Call Me.load_value_types(Me.zrPage.Model)

    End Sub

    Private Sub load_value_types(ByVal arModel As FBM.Model)

        Dim lrValueType As FBM.ValueType
        Dim larValueTypeList As New List(Of FBM.ValueType)

        larValueTypeList = Me.zrPage.Model.ValueType.FindAll(Function(x) x.IsMDAModelElement = False And
                                                                         pcenumORMDataType.NumericAutoCounter <> x.DataType And
                                                                         Not x.IsReferenceMode)

        For Each lrValueType In larValueTypeList
            Me.ComboBox_ValueType.Items.Add(New tComboboxItem(lrValueType, lrValueType.Name, lrValueType))
        Next

    End Sub


    Private Sub AddActorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'add_actor_frm.ShowDialog()

    End Sub

    Private Sub frmStateTransitionDiagram_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call Me.SetToolbox()

    End Sub

    Private Sub frmStateTransitionDiagram_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
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

        Call SetToolbox()

        '--------------------------------------------------
        'Send the views to the back, so that the ComboBox
        '  is brought to the front.
        '--------------------------------------------------
        Me.ToolStrip1.BringToFront()

    End Sub


    Private Sub UseCaseDiagramView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateTransitionDiagramView.Click

        '---------------------------------------------------------------
        'Update the Working Environment within the Main form so that
        '  the User can see where they are working/viewing.
        '---------------------------------------------------------------
        Dim lr_working_environment As New tWorkingEnvironment

        lr_working_environment.Model = Me.zrPage.Model
        lr_working_environment.Page = Me.zrPage

        prApplication.ChangeWorkingEnvironment(lr_working_environment)

        Call SetToolbox()

        Me.Diagram.Invalidate()
        Me.zrPage.IsDirty = True

    End Sub

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.StateTransitionShapeLibrary)
        Dim lrToolboxForm As frmToolbox
        lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

        If IsSomething(lrToolboxForm) Then
            lrToolboxForm.ShapeListBox.Shapes = lsl_shape_library.Shapes

            Dim lo_shape As Shape

            For Each lo_shape In lrToolboxForm.ShapeListBox.Shapes
                Select Case lo_shape.DisplayName
                    Case Is = "State"
                End Select
            Next
        End If

    End Sub

    Private Sub UseCaseDiagramView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles StateTransitionDiagramView.DoubleClick

        '---------------------------------------
        'Only allow 'InPlaceEdit' on Processes
        '---------------------------------------
        If Me.Diagram.Selection.Items.Count = 1 Then
            If Me.Diagram.Selection.Items(0).Tag.ConceptType = pcenumConceptType.Process Then
                Me.StateTransitionDiagramView.AllowInplaceEdit = True
            Else
                Me.StateTransitionDiagramView.AllowInplaceEdit = False
            End If
        Else
            Me.StateTransitionDiagramView.AllowInplaceEdit = False
        End If

    End Sub

    Private Sub UseCaseDiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles StateTransitionDiagramView.DragDrop

        Dim liInd As Integer = 0
        Dim ls_message As String = ""
        Dim loNode As New ShapeNode
        Dim li_language_id As pcenumLanguage

        li_language_id = pcenumLanguage.ORMModel

        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim lnode_dragged_node As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            Dim lrToolboxForm As frmToolbox
            lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

            If lnode_dragged_node.Index >= 0 Then
                If lnode_dragged_node.Index < lrToolboxForm.ShapeListBox.ShapeCount Then
                    Dim p As Point = Me.StateTransitionDiagramView.PointToClient(New Point(e.X, e.Y))
                    Dim pt As PointF = Me.StateTransitionDiagramView.ClientToDoc(New Point(p.X, p.Y))

                    Select Case lrToolboxForm.ShapeListBox.Shapes(lnode_dragged_node.Index).Id
                        Case Is = "State"

                            'Use the frmDialogSelectValueTypeState
                            Using lfrmStateSelectDialog As New frmDialogSelectValueTypeState(Me.zrPage.Model)

                                If lfrmStateSelectDialog.ShowDialog = DialogResult.OK Then

                                    Dim lrSTMState = Me.zrPage.Model.createCMMLState(Me.zrPage.STDiagram.ValueType, "New State")

                                    Dim lrSTDState As New STD.State(Me.zrPage)
                                    lrSTDState.Concept = New FBM.Concept("New State")
                                    lrSTDState.StateName = "New State"
                                    lrSTDState.STMState = lrSTMState
                                    Call Me.dropStateAtPoint(lrSTDState, pt)

                                End If
                            End Using


                        Case Is = "Start"
                            Dim lrStartIndicator As New STD.StartStateIndicator()
                            Call Me.dropStartIndicatorAtPoint(lrStartIndicator, pt)
                        Case Is = "Stop"
                            Dim lrStopIndicator As New STD.EndStateIndicator()
                            Call Me.dropStopIndicatorAtPoint(lrStopIndicator, pt)
                    End Select
                End If
            End If
        End If


    End Sub


    Public Sub load_StateTransitionDiagram(ByRef arPage As FBM.Page, ByRef aoTreeNode As TreeNode)

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
        Dim lr_state_shape As New ShapeNode
        Dim lr_process_link As MindFusion.Diagramming.DiagramLink = Nothing
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        Dim lrFactInstance As FBM.FactInstance
        Dim lrFactDataInstance As New FBM.FactDataInstance(arPage)

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As ORMQL.Recordset

        Try

            '-------------------------------------------------------
            'Set the Caption/Title of the Page to the PageName
            '-------------------------------------------------------
            Me.zrPage = arPage
            Me.TabText = arPage.Name
            Me.zoTreeNode = aoTreeNode

            Me.zrPage.STDiagram = New STD.Diagram(Me.zrPage)
            Me.zrPage.STDiagram.STM = Me.zrPage.Model.STM

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransitionIsForValueType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)



            'lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreStateTransitionIsForValueType.ToString)
            'Me.StateTransitionDiagram.StateTransitionValueType = lrFactTypeInstance

            'Dim ValueTypeIdList = From Fact In lrFactTypeInstance.Fact
            '                      From FactDataInstance In Fact.Data
            '                      Where FactDataInstance.Role.Name = pcenumCMML.ValueType.ToString
            '                      Select FactDataInstance.Data Distinct

            Dim lsValueTypeId As String = ""

            'For Each lsValueTypeId In ValueTypeIdList
            '    Exit For
            'Next

            If lrRecordset.Facts.Count > 0 Then
                lsValueTypeId = lrRecordset("ValueType").Data

                Me.ComboBox_ValueType.SelectedIndex = Me.ComboBox_ValueType.FindString(lsValueTypeId)
                Me.zrPage.STDiagram.ValueType = Me.zrPage.Model.ValueType.Find(Function(x) x.Id = lsValueTypeId)
                Me.ComboBox_ValueType.Enabled = False
            Else
                Me.ComboBox_ValueType.SelectedIndex = -1
            End If

            '------------------------------------------------------------------------
            'Display the StateTransitionDiagram by logically associating Shape objects
            '   with the corresponding 'object' within the ORMModelPage object
            '------------------------------------------------------------------------
            lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreStateTransition.ToString)
            Me.zrPage.STDiagram.StateTransitionRelation = lrFactTypeInstance

            '------------------------------------------------------------
            'PSEUDOCODE
            '  * Load all of the State shapes
            '  * Load all of the Transitions by linking the State shapes
            '------------------------------------------------------------
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrState As STD.State
            Dim lrFromState As STD.State
            Dim lrToState As STD.State

            While Not lrRecordset.EOF

                lrFactDataInstance = lrRecordset("Concept1")
                lrFromState = lrFactDataInstance.CloneState(arPage)
                lrFromState.X = lrFactDataInstance.X
                lrFromState.Y = lrFactDataInstance.Y
                lrFromState.StateName = lrRecordset("Concept1").Data
                lrFromState.ValueType = Me.zrPage.STDiagram.ValueType

                lrFromState.STMState = Me.zrPage.Model.STM.State.Find(Function(x) x.ValueType.Id = lrFromState.ValueType.Id And x.Name = lrFromState.StateName)

                Dim LoadedCount = Aggregate Node In Me.Diagram.Nodes
                                  Where Node.GetType.ToString = GetType(MindFusion.Diagramming.ShapeNode).ToString _
                                  And Node.Tag.ConceptType = pcenumConceptType.State _
                                  And Node.Tag.Name = lrFromState.Name
                                  Into Count()

                If LoadedCount = 0 Then
                    Me.zrPage.STDiagram.State.AddUnique(lrFromState)
                    lrFromState.DisplayAndAssociate()
                Else
                    lrFromState = Me.zrPage.STDiagram.State.Find(Function(x) x.Name = lrFromState.Name)
                End If

                lrFactDataInstance = lrRecordset("Concept2")
                lrToState = lrFactDataInstance.CloneState(arPage)
                lrToState.X = lrFactDataInstance.X
                lrToState.Y = lrFactDataInstance.Y
                lrToState.StateName = lrRecordset("Concept2").Data
                lrToState.ValueType = Me.zrPage.STDiagram.ValueType

                lrToState.STMState = Me.zrPage.Model.STM.State.Find(Function(x) x.ValueType.Id = lrToState.ValueType.Id And x.Name = lrToState.StateName)

                Dim LoadedCount2 = Aggregate Node In Me.Diagram.Nodes
                                   Where Node.GetType.ToString = GetType(MindFusion.Diagramming.ShapeNode).ToString _
                                     And Node.Tag.ConceptType = pcenumConceptType.State _
                                     And Node.Tag.Name = lrToState.Name
                                    Into Count()

                If LoadedCount2 = 0 Then
                    Me.zrPage.STDiagram.State.AddUnique(lrToState)
                    lrToState.DisplayAndAssociate()
                Else
                    lrToState = Me.zrPage.STDiagram.State.Find(Function(x) x.Name = lrToState.Name)
                End If

                '================================================================
                'Load the Transitions
                lrFactInstance = lrRecordset.CurrentFact
                Dim lrStateTransition As New STD.StateTransition
                lrStateTransition = lrFactInstance.CloneStateTransition(arPage, lrFromState, lrToState, lrRecordset("Event").Data)

                Me.zrPage.STDiagram.StateTransition.AddUnique(lrStateTransition)
                Call lrStateTransition.DisplayAndAssociate()

                lrRecordset.MoveNext()
            End While


            '-----------------------------------------------------------------------------------
            'Load the Start State Indicator (terminal)
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrStartStateIndicator As STD.StartStateIndicator

            While Not lrRecordset.EOF

                lrState = Me.zrPage.STDiagram.State.Find(Function(x) x.Name = lrRecordset("CoreElement").Data)

                lrFactInstance = New FBM.FactInstance
                lrFactInstance = lrRecordset.CurrentFact

                lrStartStateIndicator = lrFactInstance.CloneStartStateIndicator(arPage, lrState)
                lrStartStateIndicator.EventName = lrRecordset("Event").Data
                Me.zrPage.STDiagram.StartBubble = lrStartStateIndicator

                Call lrStartStateIndicator.DisplayAndAssociate()

                lrRecordset.MoveNext()
            End While

            '-----------------------------------------------------------------------------------
            'Load the End State Indicators (terminals)
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreValueTypeHasFinishCoreElementState.ToString
            lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrEndStateIndicator As New STD.EndStateIndicator

            While Not lrRecordset.EOF

                lrState = Me.zrPage.STDiagram.State.Find(Function(x) x.Name = lrRecordset("CoreElement").Data)

                lrFactInstance = New FBM.FactInstance
                lrFactInstance = lrRecordset.CurrentFact

                lrFactDataInstance = lrRecordset("EndState")
                lrEndStateIndicator = lrFactDataInstance.CloneEndStateIndicator(arPage, lrState)
                'lrEndStateIndicator.EndStateId = lrRecordset("EndState").Data

                If Me.zrPage.STDiagram.EndStateIndicator.Find(AddressOf lrEndStateIndicator.Equals) Is Nothing Then
                    Me.zrPage.STDiagram.EndStateIndicator.Add(lrEndStateIndicator)
                    Call lrEndStateIndicator.DisplayAndAssociate()
                Else
                    lrEndStateIndicator = Me.zrPage.STDiagram.EndStateIndicator.Find(AddressOf lrEndStateIndicator.Equals)
                End If

                Dim lrEndStateTransition = New STD.EndStateTransition(lrState, lrEndStateIndicator, lrRecordset("Event").Data)
                Call lrEndStateTransition.DisplayAndAssociate()

                lrRecordset.MoveNext()
            End While

            Me.StateTransitionDiagramView.SendToBack()
            Me.HiddenDiagramView.SendToBack()

            Call Me.reset_node_and_link_colors()
            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

            '=================================
            'EXIT SUB - Old code below
            Exit Sub

            For Each lrFactInstance In Me.zrPage.STDiagram.StateTransitionRelation.Fact
                For Each lrFactDataInstance In lrFactInstance.Data
                    If lrFactDataInstance.Role.JoinedORMObject.Name = pcenumCMML.Concept.ToString Then
                        '------------
                        'Is State
                        '------------
                        Dim lr_state As STD.State = lrFactDataInstance.CloneState(arPage)

                        '-------------------------------------------------------------------------------
                        'Check to see if the Shape for the State has already been loaded onto the Page
                        '-------------------------------------------------------------------------------
                        Dim lr_node As MindFusion.Diagramming.DiagramNode
                        Dim lbStateAlreadyLoaded As Boolean = False
                        For Each lr_node In Diagram.Nodes
                            Select Case lr_node.GetType.ToString
                                Case Is = GetType(MindFusion.Diagramming.ShapeNode).ToString
                                    If lr_node.Tag.ConceptType = pcenumConceptType.State Then
                                        If lr_node.Tag.Name = lr_state.Name Then
                                            lbStateAlreadyLoaded = True
                                            loDroppedNode = lr_node
                                            lr_state_shape = loDroppedNode
                                        End If
                                    End If
                            End Select
                        Next

                        If lbStateAlreadyLoaded Then
                            '----------------------------------------------------------------
                            'Nothing further to do, because the State ShapeNode has already
                            '  been loaded onto the Page.
                            '----------------------------------------------------------------
                            lrFactDataInstance.Shape = lr_state_shape
                        Else
                            '----------------------------------------------
                            'Create a ShapeNode on the Page for the State
                            '----------------------------------------------
                            loDroppedNode = Diagram.Factory.CreateShapeNode(lr_state.X, lr_state.Y, 2, 2)
                            loDroppedNode.Shape = Shapes.RoundRect
                            loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                            loDroppedNode.Resize(20, 15)
                            loDroppedNode.AllowIncomingLinks = True
                            loDroppedNode.AllowOutgoingLinks = True
                            loDroppedNode.Text = lr_state.Data
                            loDroppedNode.Transparent = False

                            loDroppedNode.Tag = New ERD.Entity
                            loDroppedNode.Tag = lr_state
                            lr_state.Shape = loDroppedNode

                            lr_state_shape = loDroppedNode

                            lrFactDataInstance.Shape = loDroppedNode
                        End If
                    End If
                Next 'For Each lrFactDataInstance In lrFactInstance.data
            Next 'For each Fact in the StateTransitionRelation

            '-----------------------------
            'Load Transitions (Processes)
            '-----------------------------
            Dim lr_state_shape1, lr_state_shape2 As New ShapeNode
            For Each lrFactInstance In Me.zrPage.STDiagram.StateTransitionRelation.Fact
                '--------------------------------------
                'Locate the State (Concept) ShapeNodes
                '--------------------------------------
                lrFactDataInstance = New FBM.FactDataInstance
                lrFactDataInstance.Role = New FBM.RoleInstance
                lrFactDataInstance.Role.Id = "Value1"
                lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
                lr_state_shape1 = lrFactDataInstance.Shape

                lrFactDataInstance = New FBM.FactDataInstance
                lrFactDataInstance.Role = New FBM.RoleInstance
                lrFactDataInstance.Role.Id = "Value2"
                lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
                lr_state_shape2 = lrFactDataInstance.Shape

                lrFactDataInstance = New FBM.FactDataInstance
                lrFactDataInstance.Role = New FBM.RoleInstance
                lrFactDataInstance.Role.Id = "StateTransitionRelation-Process"
                lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)


                '----------------------------
                'Link the transition States  
                '----------------------------
                Dim lo_link As DiagramLink
                lo_link = Me.Diagram.Factory.CreateDiagramLink(lr_state_shape1, lr_state_shape2)
                lo_link.Text = lrFactDataInstance.Data
                lo_link.Tag = lrFactInstance
            Next 'For each Fact in the StateTransitionRelation

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

    Private Sub UseCaseDiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles StateTransitionDiagramView.DragOver

        Dim p As Point = Me.StateTransitionDiagramView.PointToClient(New Point(e.X, e.Y))
        Dim lo_point As PointF = Me.StateTransitionDiagramView.ClientToDoc(New Point(p.X, p.Y))
        Dim loNode As MindFusion.Diagramming.DiagramNode


        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            loNode = Diagram.GetNodeAt(lo_point)
            If TypeOf loNode Is MindFusion.Diagramming.ShapeNode Then
                loNode.Pen = New MindFusion.Drawing.Pen(Color.Brown)
            End If
        Else
            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
                Dim lnode_dragged_node As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

                Select Case lnode_dragged_node.Tag.Id
                    Case = "Start", "State", "Stop"
                    Case Else
                        e.Effect = DragDropEffects.None
                        Exit Sub
                End Select
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

    Sub dropStateAtPoint(ByVal ar_state As STD.State, ByVal ao_pt As PointF)

        'Dim lrStateInstance As New STD.State
        'Dim loDroppedNode As ShapeNode


        'loDroppedNode = Diagram.Factory.CreateShapeNode(ao_pt.X, ao_pt.Y, 2, 2)
        'loDroppedNode.Shape = Shapes.RoundRect
        'loDroppedNode.HandlesStyle = HandlesStyle.MoveOnly
        'loDroppedNode.ToolTip = "Actor"
        'loDroppedNode.Visible = False
        'loDroppedNode.Image = My.Resources.CMML.actor
        'loDroppedNode.Pen.Color = Color.White
        'loDroppedNode.ShadowColor = Color.White

        'loDroppedNode.Tag = New CMML.tActor
        'loDroppedNode.Resize(10, 15)

        'lrStateInstance.Model = prApplication.WorkingModel
        'lrStateInstance.LongDescription = ar_state.LongDescription
        'lrStateInstance.ShortDescription = ar_state.ShortDescription
        'lrStateInstance.Name = ar_state.Name
        'lrStateInstance.Shape = loDroppedNode
        ''lrStateInstance.Symbol = ar_state.Concept.Symbol
        'lrStateInstance.X = loDroppedNode.Bounds.X
        'lrStateInstance.Y = loDroppedNode.Bounds.Y

        If Not Me.zrPage.STDiagram.State.Exists(AddressOf ar_state.Equals) Then
            '----------------------------------------------------------
            'The State is not already within the STDiagram so add it.
            Me.zrPage.STDiagram.State.Add(ar_state)
            ar_state.DisplayAndAssociate()
        End If

        'Me.zrPage.STDiagram.State.Add(lrStateInstance)
        'loDroppedNode.Tag = lrStateInstance
        'loDroppedNode.Visible = True

    End Sub

    Sub dropStartIndicatorAtPoint(ByVal arStartIndicator As STD.StartStateIndicator, ByVal aoPtf As PointF)

        Dim lrStartIndicator As New STD.StartStateIndicator
        Dim loDroppedNode As ShapeNode


        loDroppedNode = Diagram.Factory.CreateShapeNode(aoPtf.X, aoPtf.Y, 8, 8)
        loDroppedNode.Shape = Shapes.Ellipse
        loDroppedNode.HandlesStyle = HandlesStyle.MoveOnly
        loDroppedNode.ToolTip = "Start Indicator"
        loDroppedNode.Visible = True
        loDroppedNode.Image = Nothing
        loDroppedNode.Pen.Color = Color.Black
        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.Black)
        loDroppedNode.ShadowColor = Color.White
        loDroppedNode.AllowIncomingLinks = False
        loDroppedNode.AllowOutgoingLinks = True

        loDroppedNode.Tag = New STD.StartStateIndicator
        loDroppedNode.Resize(8, 8)

        lrStartIndicator.Model = prApplication.WorkingModel
        lrStartIndicator.LongDescription = arStartIndicator.LongDescription
        lrStartIndicator.ShortDescription = arStartIndicator.ShortDescription
        lrStartIndicator.Shape = loDroppedNode
        lrStartIndicator.X = loDroppedNode.Bounds.X
        lrStartIndicator.Y = loDroppedNode.Bounds.Y

        'If Not Me.StateTransitionDiagram.State.Exists(AddressOf ar_state.Equals) Then
        '--------------------------------------------------
        'The State is not already within the ORMModel
        '  so add it.
        '--------------------------------------------------
        '   Me.StateTransitionDiagram.State.Add(ar_state)
        'End If
        'Me.StateTransitionDiagram.Stat.Add(lrStartIndicator )

        loDroppedNode.Tag = lrStartIndicator


    End Sub

    Sub dropStopIndicatorAtPoint(ByVal arStopIndicator As STD.EndStateIndicator, ByVal aoPtf As PointF)

        Dim lrStopIndicator As New STD.EndStateIndicator
        Dim loDroppedNode As ShapeNode


        loDroppedNode = Diagram.Factory.CreateShapeNode(aoPtf.X, aoPtf.Y, 18, 18)
        loDroppedNode.Shape = New Shape(
                  New ElementTemplate() {
                     New ArcTemplate(0, 0, 74, 74, 0, 360, Color.Black, DashStyle.Solid, 0.5),
                     New ArcTemplate(14, 14, 48, 48, 0, 360)
                  },
                  FillMode.Winding, "Custom")

        loDroppedNode.HandlesStyle = HandlesStyle.MoveOnly
        loDroppedNode.ToolTip = "Start Indicator"
        loDroppedNode.Visible = True
        loDroppedNode.Image = Nothing
        loDroppedNode.Pen.Color = Color.Black
        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
        loDroppedNode.ShadowColor = Color.White
        loDroppedNode.AllowIncomingLinks = True
        loDroppedNode.AllowOutgoingLinks = False

        loDroppedNode.Tag = New STD.StartStateIndicator
        loDroppedNode.Resize(8, 8)

        lrStopIndicator.Model = prApplication.WorkingModel
        lrStopIndicator.LongDescription = arStopIndicator.LongDescription
        lrStopIndicator.ShortDescription = arStopIndicator.ShortDescription
        lrStopIndicator.Shape = loDroppedNode
        lrStopIndicator.X = loDroppedNode.Bounds.X
        lrStopIndicator.Y = loDroppedNode.Bounds.Y

        'If Not Me.StateTransitionDiagram.State.Exists(AddressOf ar_state.Equals) Then
        '--------------------------------------------------
        'The State is not already within the ORMModel
        '  so add it.
        '--------------------------------------------------
        '   Me.StateTransitionDiagram.State.Add(ar_state)
        'End If
        'Me.StateTransitionDiagram.Stat.Add(lrStopIndicator )

        loDroppedNode.Tag = lrStopIndicator

    End Sub

    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated

        Dim lsMessage As String
        Dim loFirstEntity As New Object
        Dim loSecondEntity As New Object

        loFirstEntity = e.Link.Origin.Tag
        loSecondEntity = e.Link.Destination.Tag

        '===================================================================================================================
        '20200503-VM-The code below seems to come from UseCaseDiagram processing. Modify or remove. Likely modify for STDs

        'Dim lrTypeOfRelation As pcenumCMMLRelations
        If (loFirstEntity.ConceptType = pcenumConceptType.StartStateIndicator) And (loSecondEntity.ConceptType = pcenumConceptType.State) Then

            Dim lrStartState As STD.State = loSecondEntity

            If Me.zrPage.STDiagram.State.FindAll(Function(x) x.IsStartState).Count = 0 Then

                Call lrStartState.setStartState(True)
            Else
                lsMessage = "There is already a Start State for this Value Type."
                lsMessage &= vbCrLf & vbCrLf & "Would you like to remove the existing Start State and create a new one?"

                If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                    Dim lrExistingStartState = Me.zrPage.STDiagram.State.Find(Function(x) x.IsStartState)

                    Call Me.zrPage.Model.STM.removeStartState(lrExistingStartState.STMState)

                    Me.Diagram.Links.Remove(Me.zrPage.STDiagram.StartBubble.Link)

                    Call lrStartState.setStartState(True)
                End If
            End If

        ElseIf (loFirstEntity.ConceptType = pcenumConceptType.State) And (loSecondEntity.ConceptType = pcenumConceptType.EndStateIndicator) Then

            Dim lrState As STD.State = loFirstEntity

            If Not lrState.IsEndState Then
                Call lrState.STMState.setEndState(True)
            End If

            'the State could already be an EndState, but this EndStateTransition is different (i.e. to a different EndStateBubble).
            Dim lrEndStateTransition As New FBM.STM.EndStateTransition(Me.zrPage.STDiagram.ValueType, lrState.STMState, "")

            If Me.zrPage.Model.STM.EndStateTransition.find(AddressOf lrEndStateTransition.Equals) Is Nothing Then
                Call Me.zrPage.Model.STM.addEndStateTransition(lrEndStateTransition)
            Else
                lsMessage = "There is already an End State for the State, '" & lrState.StateName & "', and with a transition event called '' (nothing)."
                lsMessage &= vbCrLf & vbCrLf & "Name the transition event for that End State Transition and then recreate this End State."
                lsMessage &= vbCrLf & "I.e. If you have more than one End State Transition for a State, then the End State Transitions for that State must be named."
                MsgBox(lsMessage)
            End If

        ElseIf (loFirstEntity.ConceptType = pcenumConceptType.State) And (loSecondEntity.ConceptType = pcenumConceptType.State) Then

            Dim lrFromState As STD.State = loFirstEntity
            Dim lrToState As STD.State = loSecondEntity

            Dim lrStateTransition As New FBM.STM.StateTransition(Me.zrPage.STDiagram.ValueType,
                                                                 lrFromState.STMState,
                                                                 lrToState.STMState,
                                                                 "")

            'Remove the link just created, because the link is created by the Event when the new StateTransition is added to the Model.
            Me.Diagram.Links.Remove(e.Link)

            Call Me.zrPage.STDiagram.STM.addStateTransition(lrStateTransition)


            '    lrTypeOfRelation = pcenumCMMLRelations.ActorToProcessParticipationRelation
            '    '----------------------------------
            '    'Create the Fact within the Model
            '    '----------------------------------
            '    Dim lsSQLString As String = ""
            '    lsSQLString = "INSERT INTO " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
            '    lsSQLString &= " (Actor, Process)"
            '    lsSQLString &= " VALUES ("
            '    lsSQLString &= "'" & loFirstEntity.Name & "'"
            '    lsSQLString &= ",'" & loSecondEntity.Name & "'"
            '    lsSQLString &= ")"

            '    '----------------------------------
            '    'Create the Fact within the Model
            '    '----------------------------------
            '    lrFact = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)

            '    '----------------------------------
            '    'Clone and instance of the Fact
            '    '----------------------------------
            '    lrFactInstance = lrFact.CloneInstance(Me.zrPage)
            '    Me.StateTransitionDiagram.StateTransitionRelation.AddFact(lrFactInstance)

            '    Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
            '    Dim lrRole As New FBM.Role
            '    lrRole.Id = pcenumCMML.Process.ToString
            '    lrRole.Name = lrRole.Id
            '    lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
            '    lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
            '    Dim lr_process As New CMML.Process

            '    lr_process = lrFactDataInstance.CloneProcess(Me.zrPage)
            '    lr_process.shape = New ShapeNode
            '    lr_process.shape = Me.Diagram.FindNode(loSecondEntity)
            '    lr_process.shape.Tag = lr_process
        End If

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

        '=============================================================================================================================================
        '20200503-VM-The following code likely comes from UseCaseDiagram processing. Modify for STD processing.
        'If (lo_first_entity.ConceptType = pcenumConceptType.Actor) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
        '    Dim lsSQLString As String = ""
        '    lsSQLString = "DELETE FROM " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
        '    lsSQLString &= " WHERE Actor = '" & lo_first_entity.Name & "'"
        '    lsSQLString &= "   AND Process = '" & lo_second_entity.Name & "'"

        '    Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        'ElseIf (lo_first_entity.ConceptType = pcenumConceptType.Process) And (lo_second_entity.ConceptType = pcenumConceptType.Process) Then
        '    Dim lsSQLString As String = ""
        '    lsSQLString = "DELETE FROM " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
        '    lsSQLString &= " WHERE Process1 = '" & lo_first_entity.Name & "'"
        '    lsSQLString &= "   AND Process2 = '" & lo_second_entity.Name & "'"
        '    Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLString)
        'End If

        Me.Cursor = Cursors.Default

    End Sub

    Private Sub Diagram_LinkModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkModified

    End Sub



    Private Sub Diagram_LinkSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkSelected

        Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
            Case Is = pcenumConceptType.Fact
                '-------------------------------------------------------------
                'The only type of Facts in a StateTransitionDiagram Page are
                '  Process/Transition links between States.
                '-------------------------------------------------------------
                Me.zrPage.SelectedObject.Clear()
                Me.zrPage.SelectedObject.Add(Me.Diagram.Selection.Items(0).Tag)

                Me.StateTransitionDiagramView.ContextMenuStrip = ContextMenuStrip_ProcessLink

        End Select

    End Sub

    Private Sub Diagram_NodeDoubleClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeDoubleClicked

        Me.StateTransitionDiagramView.Behavior = Behavior.DrawLinks
        Me.Diagram.Invalidate()
        Me.Diagram.Selection.Clear()
        Me.Cursor = Cursors.Hand
        Me.Diagram.Invalidate()

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        Me.zrPage.MakeDirty()
        frmMain.ToolStripButton_Save.Enabled = True

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        Dim lrShape As ShapeNode
        'Dim lrLink As DiagramLink
        Dim lrORMObject As New Object
        Dim lrFactInstance As New FBM.FactInstance
        Dim lrRole As New FBM.Role

        If IsSomething(e.Node.Tag) Then
            lrShape = e.Node

            Select Case lrShape.Tag.ConceptType
                Case Is = pcenumConceptType.StartStateIndicator
                    Dim lrStartStateIndicator As STD.StartStateIndicator = e.Node.Tag
                    Call lrStartStateIndicator.NodeModified()

                Case Is = pcenumConceptType.EndStateIndicator
                    Dim lrEndStateIndicator As STD.EndStateIndicator = e.Node.Tag
                    Call lrEndStateIndicator.NodeModified()

                    '=======================================================================================================
                    'Move all EndStates in all EndStateTransitions that have the same EndState.
                    '  This is because you cannot guarantee that the page load will return the rows in the same order.
                    '  Each of the EndState values (of the same EndStateId) in CoreValueTypeHasFinishCoreElementState have
                    '  the same X,Y position and represent the same EndState.
                    Dim lsSQLQuery = "SELECT EndState"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreValueTypeHasFinishCoreElementState.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSQLQuery &= " WHERE EndState = '" & lrEndStateIndicator.EndStateId & "'"

                    Dim lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset.eof
                        Dim lrTempEndStateIndicator = lrRecordset("EndState").CloneEndStateIndicator(Me.zrPage, Nothing)
                        Call lrTempEndStateIndicator.Move(lrEndStateIndicator.X, lrEndStateIndicator.Y, True)
                        lrRecordset.MoveNext
                    End While
                    '=======================================================================================================

                Case Is = pcenumConceptType.State

                    'Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                    'lrFactDataInstance = lrShape.Tag.FactDataInstance
                    'lrFactDataInstance.X = e.Node.Bounds.X
                    'lrFactDataInstance.Y = e.Node.Bounds.Y

                    Dim lrState As STD.State = lrShape.Tag
                    Call lrState.Move(lrShape.Bounds.X, lrShape.Bounds.Y, True)


                Case Else
                    'For Each lrLink In lrShape.GetAllLinks
                    '    If lrLink.Origin Is lrShape Then
                    '        lrORMObject = lrLink.Origin.Tag
                    '        lrRole.Id = pcenumCMML.Value1.ToString
                    '    End If
                    '    If lrLink.Destination Is lrShape Then
                    '        lrORMObject = lrLink.Destination.Tag
                    '        lrRole.Id = pcenumCMML.Value2.ToString
                    '    End If
                    '    lrORMObject.X = e.Node.Bounds.X
                    '    lrORMObject.Y = e.Node.Bounds.Y
                    '    '---------------------------------------
                    '    'Get the Fact associated with the Link
                    '    '---------------------------------------
                    '    lrFactInstance = lrLink.Tag
                    '    Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                    '    lrRole.Name = lrRole.Id
                    '    lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
                    '    lrFactDataInstance.Data = lrORMObject.Data
                    '    lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.Equals)
                    '    lrFactDataInstance.X = e.Node.Bounds.X
                    '    lrFactDataInstance.Y = e.Node.Bounds.Y
                    'Next
            End Select
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
            Case Is = pcenumConceptType.State
                Me.StateTransitionDiagramView.ContextMenuStrip = ContextMenuStrip_State
            Case Else
                Me.StateTransitionDiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
        End Select

        Me.zrPage.SelectedObject.Add(e.Node.Tag)

    End Sub

    Private Sub UseCaseDiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles StateTransitionDiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub UseCaseDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles StateTransitionDiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF
        Dim loNode As DiagramNode

        lo_point = Me.StateTransitionDiagramView.ClientToDoc(e.Location)

        Me.StateTransitionDiagramView.SmoothingMode = SmoothingMode.AntiAlias

        '--------------------------------------------------
        'Just to be sure...set the Richmond.WorkingProject
        '--------------------------------------------------
        prApplication.WorkingPage = Me.zrPage

        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '----------------------------
            'Mouse is over an ShapeNode
            '----------------------------
            Me.Diagram.AllowUnconnectedLinks = True

            '--------------------------------------------
            'Turn on the TimerLinkSwitch.
            '  After the user has held down the mouse button for 1second over a object,
            '  then link creation will be allowed
            '--------------------------------------------
            TimerLinkSwitch.Enabled = True

            '----------------------------------------------------
            'Get the Node/Shape under the mouse cursor.
            '----------------------------------------------------
            loNode = Diagram.GetNodeAt(lo_point)
            Me.StateTransitionDiagramView.DrawLinkCursor = Cursors.Hand
            Cursor.Show()

            '---------------------------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel object selected
            '---------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = loNode.Tag
            End If

            'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '    '----------------------------
            '    'Mouse is over an ShapeNode
            '    '----------------------------

            '    '----------------------------------------------------
            '    'Get the Node/Shape under the mouse cursor.
            '    '----------------------------------------------------
            '    If TypeOf Diagram.GetNodeAt(lo_point) Is MindFusion.Diagramming.TableNode Then
            '        Exit Sub
            '    End If
            '    loNode = Diagram.GetNodeAt(lo_point)

            '    If Control.ModifierKeys And Keys.Control Then
            '        '------------------------------------
            '        'Use is holding down the CtrlKey so
            '        '  enforce the selection of the object
            '        '------------------------------------                
            '        loNode.Selected = True
            '        loNode.Pen.Color = Color.Blue
            '        prApplication.workingpage.MultiSelectionPerformed = True

            '        Select Case loNode.Tag.ConceptType
            '            Case Is = pcenumConceptType.role
            '                '----------------------------------------------------------------------
            '                'This stops a Role AND its FactType being selected at the same time
            '                '----------------------------------------------------------------------
            '                prApplication.workingpage.SelectedObject.Remove(loNode.Tag.FactType)
            '        End Select

            '        Exit Sub
            '    Else
            '        If prApplication.workingpage.MultiSelectionPerformed Then
            '            If Diagram.Selection.Nodes.Contains(loNode) Then
            '                '--------------------------------------------------------------------
            '                'Don't clear the SelectedObjects if the ShapeNode selected/clicked on 
            '                '  is within the Diagram.Selection because the user has just performed
            '                '  a MultiSelection, ostensibly (one would assume) to then 'move'
            '                '  or 'delete' the selection of objects.
            '                '--------------------------------------------------------------------                    
            '                '------------------------------------------------------------------------------
            '                'Unless the Shape.Tag is a FactType, then just select it
            '                '  The reason for this, is because of MindFusion.Groups.
            '                '  When a User selects a Role...the whole RoleGroup (all Roles in the FactType)
            '                '  are selected, so it is a MultiSelection by default.
            '                '------------------------------------------------------------------------------
            '                Select Case loNode.Tag.ConceptType
            '                    Case Is = pcenumConceptType.FactType
            '                        prApplication.workingpage.SelectedObject.Clear()
            '                        Diagram.Selection.Clear()
            '                        '-----------------------------------------------
            '                        'Select the ShapeNode/ORMObject just clicked on
            '                        '-----------------------------------------------                    
            '                        loNode.Selected = True
            '                        loNode.Pen.Color = Color.Blue

            '                        '-------------------------------------------------------------------
            '                        'Reset the MultiSelectionPerformed flag on the ORMModel
            '                        '-------------------------------------------------------------------
            '                        prApplication.workingpage.MultiSelectionPerformed = False
            '                End Select
            '            Else
            '                '---------------------------------------------------------------------------
            '                'Clear the SelectedObjects because the user neither did a MultiSelection
            '                '  nor held down the [Ctrl] key before clicking on the ShapeNode.
            '                '  Clearing the SelectedObject groups, allows for new objects to be selected
            '                '  starting with the ShapeNode/ORMObject just clicked on.
            '                '---------------------------------------------------------------------------
            '                prApplication.workingpage.SelectedObject.Clear()
            '                Diagram.Selection.Clear()
        ElseIf IsSomething(Diagram.GetLinkAt(lo_point, 2)) Then
            '-------------------------
            'User clicked on a link
            '-------------------------
        Else
            '------------------------------------------------
            'Use clicked on the Canvas
            '------------------------------------------------

            '---------------------------
            'Clear the SelectedObjects
            '---------------------------
            prApplication.WorkingPage.SelectedObject.Clear()
            Me.Diagram.Selection.Clear()

            Me.StateTransitionDiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

            Me.Diagram.AllowUnconnectedLinks = False

            '------------------------------------------------------------------------------
            'Clear the 'InPlaceEdit' on principal.
            '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas,
            '  so disable the 'InPlaceEdit'.
            '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on,
            '  then 'InPlaceEdit' is temporarily allowed.
            '------------------------------------------------------------------------------
            Me.StateTransitionDiagramView.AllowInplaceEdit = False

            '----------------------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the StateTransitionDiagram.
            '----------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.STDiagram
            End If

            Call Me.reset_node_and_link_colors()
        End If

    End Sub

    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        If e.Node.Tag.ConceptType = pcenumConceptType.Process Then
            e.Node.Tag.name = e.NewText
        End If

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Dim image As Image = Diagram.CreateImage()
        Windows.Forms.Clipboard.SetImage(image)

    End Sub

    Private Sub UseCaseDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles StateTransitionDiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loObject As Object
        Dim loNode As DiagramNode

        StateTransitionDiagramView.SmoothingMode = SmoothingMode.Default

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
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.STDiagram
            End If
        End If

    End Sub

    Private Sub UseCaseDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles StateTransitionDiagramView.MouseWheel

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

        Call frmMain.LoadToolboxModelDictionary()

    End Sub


    Private Sub ContextMenuStrip_Actor_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_State.Opening

        Dim lrPage As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lrModel As FBM.Model
        Dim lrState As STD.State

        If prApplication.WorkingPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If



        '-------------------------------------
        'Check that selected object is Actor
        '-------------------------------------
        If Not TypeOf prApplication.WorkingPage.SelectedObject(0) Is STD.State Then
            '--------------------------------------------------------
            'Sometimes the MouseDown/NodeSelected gets it wrong and this sub receives invocation before an State is 
            '  properly selected. The user may try and click again. If it's a bug, then this can be removed obviously.
            '--------------------------------------------------------
            Exit Sub
        End If

        lrState = prApplication.WorkingPage.SelectedObject(0)

        lrModel = lrState.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.morph_vector = New tMorphVector(lrState.X, lrState.Y, 0, 0, 40)


        '--------------------------------------------------------------
        'Clear the list of ORMDiagrams that may relate to the EntityType
        '--------------------------------------------------------------
        Me.ORMDiagramToolStripMenuItem.DropDownItems.Clear()

        '----------------------------------------------------
        'Load the ORMDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '----------------------------------------------------                        
        larPage_list = prApplication.CMML.getORMDiagramPagesForState(lrState)

        Me.morph_vector = New tMorphVector(lrState.X, lrState.Y, 0, 0, 40)
        Me.MorphVector.Clear()
        Me.MorphVector.Add(New tMorphVector(lrState.Shape.Bounds.X, lrState.Shape.Bounds.Y, 0, 0, 40))

        For Each lrPage In larPage_list
            Dim lo_menu_option As ToolStripItem

            '----------------------------------------------------------
            'Try and find the Page within the EnterpriseView.TreeView
            '  NB If 'Core' Pages are not shown for the model, 
            '  they will not be in the TreeView and so a menuOption
            '  is now added for those hidden Pages.
            '----------------------------------------------------------
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                       lrPage,
                                                       lrModel.ModelId,
                                                       pcenumLanguage.ORMModel,
                                                       Nothing,
                                                       lrPage.PageId)

            lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            If IsSomething(lr_enterprise_view) Then
                '---------------------------------------------------
                'Add the Page(Name) to the MenuOption.DropDownItems
                '---------------------------------------------------
                lo_menu_option = Me.ORMDiagramToolStripMenuItem.DropDownItems.Add(lrPage.Name)
                lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
                AddHandler lo_menu_option.Click, AddressOf Me.morph_to_ORM_diagram
            End If

        Next

    End Sub

    Public Sub morph_to_ORM_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If prApplication.WorkingPage.SelectedObject.Count = 0 Then Exit Sub
        If prApplication.WorkingPage.SelectedObject.Count > 1 Then Exit Sub

        '---------------------------------------------
        'Take a copy of the selected State
        '---------------------------------------------
        'Me.StateTransitionDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.StateTransitionDiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()


        Dim lr_state As STD.State = prApplication.WorkingPage.SelectedObject(0)

        Dim lr_shape_node As ShapeNode


        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the State being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrValueTypeInstanceList = From ValueTypeInstance In lr_page.ValueTypeInstance
                                          Where ValueTypeInstance.ValueConstraint.Contains(lr_state.Data)
                                          Select New FBM.ValueTypeInstance(lr_page.Model,
                                                                    lr_page,
                                                                    pcenumLanguage.ORMModel,
                                                                    ValueTypeInstance.Name,
                                                                    True,
                                                                    ValueTypeInstance.X,
                                                                    ValueTypeInstance.Y)


            Dim lrValueTypeInstance As New FBM.ValueTypeInstance
            For Each lrValueTypeInstance In lrValueTypeInstanceList
                Exit For
            Next

            '----------------------------------------------------------------
            'Retreive the actual ValueTypeInstance on the destination page
            '----------------------------------------------------------------
            lrValueTypeInstance = lr_page.ValueTypeInstance.Find(AddressOf lrValueTypeInstance.Equals)

            If lr_page.Loaded And lrValueTypeInstance.Shape IsNot Nothing Then
                lr_shape_node = lrValueTypeInstance.Shape.Clone(True)
                Me.morph_shape = lr_shape_node
            Else
                Me.morph_shape = lr_state.Shape.Clone(True)
                Me.morph_shape.Shape = Shapes.RoundRect
                Me.morph_shape.HandlesStyle = HandlesStyle.InvisibleMove
                Me.morph_shape.Text = lr_state.Data
                Me.morph_shape.Resize(20, 15)
            End If

            Me.HiddenDiagram.Nodes.Add(Me.morph_shape)
            Me.HiddenDiagram.Invalidate()

            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrValueTypeInstance.X, lrValueTypeInstance.Y, 40)

            Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)


        Me.HiddenDiagram.Nodes.Clear()
        Call Me.StateTransitionDiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Process/Entity to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        Dim lr_link_node As New DiagramLink(Me.Diagram)
        lr_link_node = Me.Diagram.Selection.Items(0)
        lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_link_node.Bounds.X, lr_link_node.Bounds.Y, 20, 20)
        lr_shape_node.Shape = Shapes.Ellipse
        lr_shape_node.Pen.Color = Color.Black
        lr_shape_node.Text = lr_link_node.Text
        lr_shape_node.Visible = True

        Me.morph_shape = lr_shape_node

        Me.HiddenDiagram.Invalidate()


        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            'frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = lr_enterprise_view.treeNode
            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrProcess = From FactType In lr_page.FactTypeInstance
                            From Fact In FactType.Fact
                            From RoleData In Fact.Data
                            Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Process.ToString
                            Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

            Dim lrFactDataInstance As New Object
            For Each lrFactDataInstance In lrProcess
                Exit For
            Next

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()

        End If

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lr_point As New Point
        Dim lr_rect As New Rectangle

        lr_point = Me.morph_vector.getNextMorphVectorStepPoint

        Me.morph_shape.Move(lr_point.X, lr_point.Y)
        Me.HiddenDiagram.Invalidate()

        If Me.morph_vector.VectorStep > Me.morph_vector.VectorSteps Then
            Me.MorphStepTimer.Stop()
            Me.MorphStepTimer.Enabled = False

            frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
            Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)
            Me.StateTransitionDiagramView.BringToFront()
            Me.Diagram.Invalidate()
            Me.MorphTimer.Enabled = False

        End If

    End Sub

    Private Sub frmStateTransitionDiagram_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        Me.StateTransitionDiagramView.SendToBack()
        Me.HiddenDiagramView.SendToBack()

        Me.ComboBox_ValueType.Visible = True

        'prApplication.WorkingPage.SelectedObject.Clear()

        Call Me.SetToolbox()

    End Sub

    Private Sub ComboBox_ValueType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_ValueType.SelectedIndexChanged

        prApplication.WorkingValueType = Me.ComboBox_ValueType.SelectedItem.ItemData

        Dim lrModelDictionaryForm As frmToolboxModelDictionary
        lrModelDictionaryForm = prApplication.GetToolboxForm(frmToolboxModelDictionary.Name)

        If IsSomething(lrModelDictionaryForm) Then
            Call lrModelDictionaryForm.LoadToolboxModelDictionary(pcenumLanguage.StateTransitionDiagram)
        End If

    End Sub

    Private Sub ContextMenuStrip_ProcessLink_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ProcessLink.Opening

        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_process As New CMML.Process


        If Me.zrPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '-------------------------------------
        'Check that selected object is Process
        '-------------------------------------
        If Me.zrPage.SelectedObject(0).GetType Is GetType(FBM.FactInstance) Then
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

        Dim lrDiagramLink As DiagramLink = Me.Diagram.Selection.Items(0)
        Dim lrFactInstance As FBM.FactInstance = Me.zrPage.SelectedObject(0)



        Dim lrFactDataInstance As New FBM.FactDataInstance
        lrFactDataInstance.Role.Id = "StateTransitionRelation-Process"
        lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)

        lr_process = lrFactDataInstance.CloneProcess(Me.zrPage)
        lr_model = lr_process.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.morph_vector = New tMorphVector(lrDiagramLink.Bounds.X, lrDiagramLink.Bounds.Y, 0, 0, 40)

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.UseCaseDiagramToolStripMenuItem1.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the UseCaseDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------

        '--------------------------------------------------------------------------------------------------------
        'The EntityType represents an Actor. i.e. Has a ParentEntityType of 'Actor' within the Core meta-schema
        '--------------------------------------------------------------------------------------------------------
        '=================================================================================================
        '20200503-VM-Undo commentout below when have some UseCase (or other) diagram type to morph to.
        '=================================================================================================
        'larPage_list = prApplication.CMML.get_use_case_diagram_pages_for_process(lr_process)


        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '---------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '---------------------------------------------------
        '    lo_menu_option = Me.UseCaseDiagramToolStripMenuItem1.DropDownItems.Add(lr_page.Name)
        '    Dim lr_enterprise_view As tEnterpriseEnterpriseView
        '    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUseCaseDiagram,
        '                                               lr_page,
        '                                               lr_page.Model.EnterpriseId,
        '                                               lr_page.Model.SubjectAreaId,
        '                                               lr_page.Model.ProjectId,
        '                                               lr_page.Model.SolutionId,
        '                                               lr_page.Model.ModelId,
        '                                               pcenumLanguage.UseCaseDiagram,
        '                                               Nothing, lr_page.PageId)
        '    lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '    AddHandler lo_menu_option.Click, AddressOf Me.morph_to_UseCase_diagram
        'Next

    End Sub

    Private Sub mnuOption_ViewGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        If Not IsNothing(frmMain.zfrm_properties) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.STDiagram
            End If
        End If

    End Sub

    Private Sub PageAsORMMetaModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetaModelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call Me.SetToolbox()

    End Sub

End Class