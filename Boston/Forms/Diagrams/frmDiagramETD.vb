Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports MindFusion.Diagramming.Lanes

Public Class frmDiagramETD

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing
    Public ETD_model As New ETD.ETDModel

    ' Initialize grid
    Dim header As Header = Nothing
    Dim grid As Grid
    Dim columns As HeaderCollection

    Private MorphVector As New List(Of tMorphVector)
    Private MorphShape As New ShapeNode

    Dim zNodeColection As New List(Of DiagramNode)
    Dim zTextColection As New List(Of DiagramNode)

    Private Sub frmDiagramETD_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        Call Me.SetToolbox()

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


    Private Sub frm_EventTraceDiagram_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        Call SetToolbox()

    End Sub

    Private Sub frm_EventTraceDiagram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Sub SetupForm()

        Diagram.AllowSelfLoops = False
        Diagram.AllowLinksRepeat = False

        Call Me.SetToolbox()

        Me.grid = Me.Diagram.LaneGrid
        Me.columns = grid.ColumnHeaders

    End Sub


    ''' <summary>
    ''' Loads the Event Trace Diagram for the given ORM (meta-model) Page
    ''' </summary>
    ''' <param name="arPage">The Page of the ORM Model within which the Event Trace Diagram is injected. _
    '''  The Richmond data model is an ORM meta-model. Event Trace Diagrams are stored as propositional functions within the ORM meta-model.</param>
    ''' <remarks></remarks>
    Sub LoadEventTraceDiagramPage(ByRef arPage As FBM.Page)

        grid.MinHeaderSize = 8
        grid.HookHeaders = False
        grid.HeadersOnTop = False
        grid.ColumnHeadersHeights = New Single() {8, 8}
        grid.AlignCells = False
        grid.AllowResizeHeaders = True
        grid.TopMargin = 10
        grid.LeftMargin = 10

        Dim lrProcess As CMML.Process


        'header = New Header("Week " + i.ToString() + ", 2007")
        'header.SubHeaders.Add(New Header("S"))
        'header.SubHeaders.Add(New Header("M"))
        'header.SubHeaders.Add(New Header("T"))
        'header.SubHeaders.Add(New Header("W"))
        'header.SubHeaders.Add(New Header("T"))
        'header.SubHeaders.Add(New Header("F"))
        'header.SubHeaders.Add(New Header("S"))
        'columns.Add(header)

        Dim lrRecordset As New ORMQL.Recordset
        Dim lsSqlQuery As String = ""

        lsSqlQuery = "SELECT DISTINCT Element "
        lsSqlQuery &= " FROM " & pcenumCMMLRelations.ElementHasElementType.ToString
        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSqlQuery &= " WHERE ElementType = 'Actor'"

        lrRecordset = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)

        While Not lrRecordset.EOF

            Dim lrActor As New CMML.tActor(Me.zrPage)
            Dim lrFactDataInstance As FBM.FactDataInstance
            lrFactDataInstance = lrRecordset("Element")
            lrActor = lrFactDataInstance.CloneActor(Me.zrPage)

            Dim lrRecordsetSequenceNr As New ORMQL.Recordset
            lsSqlQuery = "SELECT *"
            lsSqlQuery &= " FROM " & pcenumCMMLRelations.ElementSequenceNr.ToString
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE Element = '" & lrActor.Name & "'"

            lrRecordsetSequenceNr = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)

            lrActor.SequenceNr = Convert.ToSingle(lrRecordsetSequenceNr("SequenceNr").Data)

            Me.ETD_model.Actor.Add(lrActor)

            lrRecordset.MoveNext()
        End While

        Dim lrActorInSequence As CMML.tActor
        Me.ETD_model.Actor.Sort(AddressOf CMML.tActor.CompareSequenceNrs)
        For Each lrActorInSequence In Me.ETD_model.Actor
            header = New Header(lrActorInSequence.Name)
            Me.columns.Add(header)
            header.Width = Me.Diagram.Bounds.Width / Me.ETD_model.Actor.Count
            header.TitleFormat.Alignment = StringAlignment.Center
            header.TitleFormat.LineAlignment = StringAlignment.Center
        Next

        lsSqlQuery = "SELECT DISTINCT Element "
        lsSqlQuery &= " FROM " & pcenumCMMLRelations.ElementHasElementType.ToString
        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
        lsSqlQuery &= " WHERE ElementType = 'Process'"

        lrRecordset = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)
        While Not lrRecordset.EOF

            lrProcess = New CMML.Process(Me.zrPage, System.Guid.NewGuid.ToString)

            Dim lrFactDataInstance As FBM.FactDataInstance

            lrFactDataInstance = lrRecordset("Element")
            lrProcess = lrFactDataInstance.CloneProcess(Me.zrPage)

            Me.ETD_model.Process.Add(lrProcess)

            lrRecordset.MoveNext()
        End While

        '==================================
        'Draw the Processes
        '==================================
        lsSqlQuery = "SELECT *"
        lsSqlQuery &= " FROM " & pcenumCMMLRelations.ActorToProcessParticipationRelation.ToString
        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"


        lrRecordset = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)
        While Not lrRecordset.EOF

            lrProcess = New CMML.Process(Me.zrPage, lrRecordset("Process").Data)
            lrProcess = Me.ETD_model.Process.Find(AddressOf lrProcess.EqualsByName)

            Dim lrRecordsetSequenceNr As New ORMQL.Recordset
            lsSqlQuery = "SELECT *"
            lsSqlQuery &= " FROM " & pcenumCMMLRelations.ElementSequenceNr.ToString
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE Element = '" & lrRecordset("Process").Data & "'"

            lrRecordsetSequenceNr = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)
            lrProcess.SequenceNr = Convert.ToSingle(lrRecordsetSequenceNr("SequenceNr").Data)

            '-------------------------------------------
            'Check to see if the Process is a decision
            '-------------------------------------------
            Dim lrRecordset1 As ORMQL.Recordset

            lsSqlQuery = "SELECT *"
            lsSqlQuery &= " FROM ProcessIsDecision"
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            lrRecordset1 = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)

            If lrRecordset1.Facts.Count > 0 Then
                lrProcess.IsDecision = True
            End If

            '------------------------------------------
            'Check if the Process is a Start Process
            '------------------------------------------
            lsSqlQuery = "SELECT *"
            lsSqlQuery &= " FROM ProcessIsStart"
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            lrRecordset1 = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)

            If lrRecordset1.Facts.Count > 0 Then
                lrProcess.IsStart = True
            End If

            '------------------------------------------
            'Check if the Process is a Stop Process
            '------------------------------------------
            lsSqlQuery = "SELECT *"
            lsSqlQuery &= " FROM ProcessIsStop"
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            lrRecordset1 = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)

            If lrRecordset1.Facts.Count > 0 Then
                lrProcess.IsStop = True
            End If

            '---------------------------
            'Set the responsible Actor
            '---------------------------
            Dim lrActor As New CMML.tActor(zrPage)
            lrActor.Name = lrRecordset("Actor").Data
            lrActor = Me.ETD_model.Actor.Find(AddressOf lrActor.EqualsByName)
            lrProcess.ResponsibleActor = lrActor

            lrProcess.DisplayAndAssociate()
            lrProcess.shape.Resize(8, 10)

            If lrProcess.IsDecision Then
                lrProcess.shape.Shape = Shapes.Decision
            ElseIf lrProcess.IsStart Then
                lrProcess.shape.Shape = Shapes.Ellipse
            ElseIf lrProcess.IsStop Then
                lrProcess.shape.Shape = Shapes.Ellipse
            End If

            lrRecordset.MoveNext()
        End While

        '===========================================
        'Move the Processes to the respective Lane
        '===========================================   
        Dim liRowLane As Integer = 0
        Dim lrRowHeader As New Header

        Me.ETD_model.Process.Sort(AddressOf CMML.Process.CompareSequenceNrs)

        For Each lrProcess In Me.ETD_model.Process
            If liRowLane <> lrProcess.SequenceNr Then
                liRowLane = lrProcess.SequenceNr
                lrRowHeader = New Header("Flow " + liRowLane.ToString)
                lrRowHeader.Height = 12
                lrRowHeader.TitleFormat.Alignment = StringAlignment.Center
                lrRowHeader.TitleFormat.LineAlignment = StringAlignment.Center
                grid.RowHeaders.Add(lrRowHeader)
            End If
            Dim column As Header = grid.GetColumn(1)
            'lrProcess.shape.Move(grid.GetHeaderBounds(Me.columns(1), True).X, grid.GetHeaderBounds(lrRowHeader, False).Y)
            Dim liX As Integer
            liX = grid.GetHeaderBounds(grid.FindColumn(lrProcess.ResponsibleActor.Name), True).X
            liX += (grid.FindColumn(lrProcess.ResponsibleActor.Name).Width / 2) - (lrProcess.shape.Bounds.Width / 2)
            lrProcess.shape.Move(liX, grid.GetHeaderBounds(lrRowHeader, False).Y)
            lrProcess.X = liX
            lrProcess.Y = lrProcess.shape.Bounds.Y
        Next

        '==================================
        'Draw the links between Processes
        '==================================
        lsSqlQuery = "SELECT *"
        lsSqlQuery &= " FROM " & pcenumCMMLRelations.ProcessToProcessRelation.ToString
        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrRecordset = Me.zrPage.Model.processORMQLStatement(lsSqlQuery)


        While Not lrRecordset.EOF

            Dim lrProcess1 As New CMML.Process(Me.zrPage, lrRecordset("Process1").Data)
            lrProcess1.FactData.Model = Me.zrPage.Model
            Dim lrProcess2 As New CMML.Process(Me.zrPage, lrRecordset("Process2").Data)
            lrProcess2.FactData.Model = Me.zrPage.Model

            lrProcess1 = Me.ETD_model.Process.Find(AddressOf lrProcess1.EqualsByName)
            lrProcess2 = Me.ETD_model.Process.Find(AddressOf lrProcess2.EqualsByName)

            Dim loEntityLink As DiagramLink
            loEntityLink = Me.Diagram.Factory.CreateDiagramLink(lrProcess1.shape, lrProcess2.shape)
            loEntityLink.SnapToNodeBorder = True
            loEntityLink.ShadowColor = Color.White
            loEntityLink.Style = LinkStyle.Cascading
            loEntityLink.SegmentCount = 3
            loEntityLink.HeadShape = ArrowHead.Arrow
            loEntityLink.HeadShapeSize = 3
            If lrProcess1.shape.Bounds.X < lrProcess2.shape.Bounds.X Then
                loEntityLink.Pen = New MindFusion.Drawing.Pen(Color.Blue, 0.4)
            Else
                loEntityLink.Pen = New MindFusion.Drawing.Pen(Color.BlueViolet, 0.4)
            End If
            loEntityLink.Brush = New MindFusion.Drawing.SolidBrush(Color.Black)

            lrRecordset.MoveNext()
        End While

        Me.Diagram.RouteAllLinks()
        Dim count As Integer
        Dim i As Integer
        count = Me.columns.Count
        For i = 0 To count - 1

            Dim column As Header = grid.GetColumn(i)

            grid(column, Nothing).Style.LeftBorderPen = _
             New MindFusion.Drawing.Pen( _
              New MindFusion.Drawing.HatchBrush(HatchStyle.Percent50, Color.Gray, Color.Transparent), 0)

            If i <> count - 1 Then
                grid(column, Nothing).Style.RightBorderPen = Nothing
            End If

        Next i


        count = Me.grid.RowCount
        For i = 0 To count - 1

            Dim row As Header = grid.GetRow(i)


            grid(Nothing, row).Style.TopBorderPen = _
             New MindFusion.Drawing.Pen( _
              New MindFusion.Drawing.HatchBrush(HatchStyle.Percent50, Color.Gray, Color.Transparent), 0)


            If i <> count - 1 Then
                grid(Nothing, row).Style.BottomBorderPen = Nothing
            End If

        Next i

        Me.Diagram.EnableLanes = True

        ' Ensure the document is big enough to contain the grid
        Dim width As Single = Math.Max(50, grid.ColumnCount * 6 + 20)
        Dim height As Single = Math.Max(50, grid.RowCount * 6 + 20)

        Me.Diagram.Bounds = New RectangleF(0, 0, width, height)
        Me.Diagram.AlignToGrid = False

        Me.Diagram.Invalidate()
        Me.zrPage.FormLoaded = True

    End Sub

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.ETDShapeLibrary)
        Dim lo_shape As Shape
        Dim lrToolboxForm As frmToolbox
        lrToolboxForm = prRichmondApplication.GetToolboxForm(frmToolbox.Name)

        If IsSomething(lrToolboxForm) Then
            lrToolboxForm.ShapeListBox.Shapes = lsl_shape_library.Shapes

            For Each lo_shape In lrToolboxForm.ShapeListBox.Shapes
                Select Case lo_shape.DisplayName
                    Case Is = "Actor"
                        lo_shape.Image = My.Resources.ActorSmall
                End Select
            Next

        End If

    End Sub


    Private Sub ArrangeLinks()

        For Each l As DiagramLink In Diagram.Links
            If l.Origin.Bounds.X < l.Destination.Bounds.X Then
                Dim a As Single
                If l.Origin.Bounds.Y > l.Destination.Bounds.Y Then
                    a = l.Origin.Bounds.Y
                Else
                    a = l.Destination.Bounds.Y
                End If

                Dim b As Single
                If l.Origin.Bounds.Y + l.Origin.Bounds.Height < l.Destination.Bounds.Y + l.Destination.Bounds.Height Then
                    b = l.Origin.Bounds.Y + l.Origin.Bounds.Height
                Else
                    b = l.Destination.Bounds.Y + l.Destination.Bounds.Height
                End If

                If b >= a Then
                    l.SegmentCount = 1
                    l.AutoRoute = False
                    l.Style = LinkStyle.Polyline
                    Dim y As Single = a + ((b - a) / 2)
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X + l.Origin.Bounds.Width, y)
                    l.ControlPoints(1) = New PointF(l.Destination.Bounds.X, y)
                Else
                    l.Style = LinkStyle.Cascading
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X + l.Origin.Bounds.Width, _
                    l.Origin.Bounds.Y + l.Origin.Bounds.Height / 2)
                    l.ControlPoints(l.ControlPoints.Count - 1) = New PointF(l.Destination.Bounds.X, _
                     l.Destination.Bounds.Y + l.Destination.Bounds.Height / 2)
                    Diagram.RoutingOptions.StartOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    Diagram.RoutingOptions.EndOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    l.Route()
                End If
            Else
                Dim a As Single
                If l.Origin.Bounds.Y > l.Destination.Bounds.Y Then
                    a = l.Origin.Bounds.Y
                Else
                    a = l.Destination.Bounds.Y
                End If

                Dim b As Single
                If l.Origin.Bounds.Y + l.Origin.Bounds.Height < l.Destination.Bounds.Y + l.Destination.Bounds.Height Then
                    b = l.Origin.Bounds.Y + l.Origin.Bounds.Height
                Else
                    b = l.Destination.Bounds.Y + l.Destination.Bounds.Height
                End If

                If b >= a Then
                    l.SegmentCount = 1
                    l.AutoRoute = False
                    l.Style = LinkStyle.Polyline
                    Dim y As Single = a + ((b - a) / 2)
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X, y)
                    l.ControlPoints(1) = New PointF(l.Destination.Bounds.X + l.Destination.Bounds.Width, y)
                Else
                    l.Style = LinkStyle.Cascading
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X, _
                    l.Origin.Bounds.Y + l.Origin.Bounds.Height / 2)
                    l.ControlPoints(l.ControlPoints.Count - 1) = New PointF(l.Destination.Bounds.X + l.Destination.Bounds.Width, _
                     l.Destination.Bounds.Y + l.Destination.Bounds.Height / 2)
                    Diagram.RoutingOptions.StartOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    Diagram.RoutingOptions.EndOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    l.Route()
                End If
            End If
            l.UpdateFromPoints()
        Next
    End Sub

    Private Sub Diagram_CellClicked(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs) Handles Diagram.CellClicked

        MsgBox("Hello")
    End Sub

    Private Sub Diagram_DrawBackground(ByVal sender As Object, ByVal e As MindFusion.Diagramming.DiagramEventArgs) Handles Diagram.DrawBackground

        'For Each d As DiagramNode In zNodeColection
        '    Dim p As System.Drawing.Pen = New System.Drawing.Pen(Color.Black)
        '    p.DashStyle = DashStyle.Dash
        '    p.Width = 0.5F
        '    e.Graphics.DrawLine(p, d.Bounds.X + d.Bounds.Width / 2, _
        '     d.Bounds.Y + d.Bounds.Height, d.Bounds.X + d.Bounds.Width / 2, 2100)
        'Next

    End Sub


    Private Sub Diagram_LinkCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkCreated
        ArrangeLinks()
    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs)

        ArrangeLinks()

    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Private Sub Diagram_NodeCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeCreated

        Dim node As ShapeNode = CType(e.Node, ShapeNode)

        node.HandlesStyle = HandlesStyle.DashFrame
        If node.Shape.Id = "SequenceObject" Then
            node.AllowOutgoingLinks = False
            node.AllowIncomingLinks = False
            node.EnabledHandles = AdjustmentHandles.None
            node.Brush = New MindFusion.Drawing.LinearGradientBrush(Color.White, Color.LimeGreen, 90)
            zNodeColection.Add(node)
        End If

        Dim i As Integer
        For i = 0 To zNodeColection.Count - 1
            zNodeColection(i).Bounds = New RectangleF(44 * i + 10, 10, 24, 24)
        Next
        If node.Shape.Id = "SequenceMessage" Then
            node.Brush = New MindFusion.Drawing.LinearGradientBrush(Color.White, Color.Red, 90)
            zTextColection.Add(node)
            Dim j As Integer
            For j = 0 To zNodeColection.Count - 1
                If node.Bounds.X > zNodeColection(j).Bounds.X - 20 And _
                node.Bounds.X < zNodeColection(j).Bounds.X + 30 Then
                    node.EnabledHandles = AdjustmentHandles.ResizeBottomCenter Or AdjustmentHandles.Move Or AdjustmentHandles.ResizeTopCenter
                    node.Constraints.MoveDirection = DirectionConstraint.Vertical
                    node.Bounds = New RectangleF(zNodeColection(j).Bounds.X + 6, _
                        e.Node.Bounds.Y, 12, 24)
                    Exit For
                End If
            Next
        End If

        Me.ETDDiagramView.EndEdit(True)
        Me.ETDDiagramView.BeginEdit(node)
        Diagram.Invalidate()
        Me.ETDDiagramView.Invalidate()

    End Sub

    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        '------------------------------------------------------------------------------------------
        'NB IMPORTANT: ETDDiagramView.MouseDown gets processed before Diagram.NodeSelected, so management of which object
        '  is displayed in the PropertyGrid is performed in ORMDiagram.MouseDown
        '------------------------------------------------------------------------------------------

        'Select Case e.Node.Tag.ConceptType
        '    Case Is = pcenumConceptType.Process
        '        e.Node.Pen.Color = Color.Blue
        '    Case Is = pcenumConceptType.Actor
        '        e.Node.Pen.Color = Color.Blue
        '    Case Else
        '        'Do nothing
        'End Select

        '----------------------------------------------------
        'Set the ContextMenuStrip menu for the selected item
        '----------------------------------------------------
        Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
            Case Is = pcenumConceptType.Actor
                Me.ETDDiagramView.ContextMenuStrip = ContextMenuStrip_Actor
            Case Is = pcenumConceptType.Process
                Me.ETDDiagramView.ContextMenuStrip = ContextMenuStrip_Process
            Case Else
                Me.ETDDiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
        End Select

        Me.zrPage.SelectedObject.Add(e.Node.Tag)

    End Sub

    Private Sub ETDDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ETDDiagramView.Click

        Call Me.SetToolbox()

    End Sub

    Private Sub ContextMenuStrip_Actor_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Actor.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_actor As New CMML.tActor


        If prRichmondApplication.WorkingPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '-------------------------------------
        'Check that selected object is Actor
        '-------------------------------------
        If lr_actor.GetType Is prRichmondApplication.WorkingPage.SelectedObject(0).GetType Then
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
        lr_actor = prRichmondApplication.WorkingPage.SelectedObject(0)

        lr_model = lr_actor.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.MorphVector.Add(New tMorphVector(lr_actor.X, lr_actor.Y, 0, 0, 40))

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.MenuOptionUseCaseDiagramActor.DropDownItems.Clear()

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
            lo_menu_option = Me.MenuOptionUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
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
        Me.MenuOptionORMDiagramActor.DropDownItems.Clear()

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
                lo_menu_option = Me.MenuOptionORMDiagramActor.DropDownItems.Add(lr_page.Name)
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

    Public Sub morph_to_ORM_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If prRichmondApplication.WorkingPage.SelectedObject.Count = 0 Then Exit Sub
        If prRichmondApplication.WorkingPage.SelectedObject.Count > 1 Then Exit Sub

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ETDDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.ETDDiagramView.SendToBack()
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
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
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
                                                                    EntityTypeInstance.X, _
                                                                    EntityTypeInstance.Y)

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
            For Each lrEntityTypeInstance In lrEntityTypeInstanceList
                Exit For
            Next

            '----------------------------------------------------------------
            'Retreive the actual EntityTypeInstance on the destination page
            '----------------------------------------------------------------
            lrEntityTypeInstance = lr_page.EntityTypeInstance.Find(AddressOf lrEntityTypeInstance.Equals)

            If lr_page.Loaded Then
                lr_shape_node = lrEntityTypeInstance.shape.Clone(True)
                Me.MorphVector(0).Shape = lr_shape_node
            Else
                Me.MorphVector(0).Shape = lr_actor.shape.Clone(True)
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

    Public Sub MorphToFlowChartDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrFactDataInstance As New Object
        Dim lrMenuItem As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ORMDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.ETDDiagramView.SendToBack()
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
            For Each lrAdditionalProcess In Me.ETD_model.Process
                If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
                    '---------------------------------------------------------------------------------------------
                    'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                    '---------------------------------------------------------------------------------------------
                Else
                    Dim lrEntityList = From FactTypeInstance In lrPage.FactTypeInstance _
                                       From Fact In FactTypeInstance.Fact _
                                       From FactData In Fact.Data _
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrAdditionalProcess.Name _
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                    Me.MorphVector.Add(New tMorphVector(lrAdditionalProcess.X, lrAdditionalProcess.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40))

                    lrShapeNode = lrAdditionalProcess.shape
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
            Dim larFactDataInstance = From FactTypeInstance In lrPage.FactTypeInstance _
                                      From Fact In FactTypeInstance.Fact _
                                      From FactData In Fact.Data _
                                      Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                      And FactData.Role.Name = "Element" _
                                      And FactData.Concept.Symbol = Me.zrPage.SelectedObject(0).Name _
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

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ORMDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.ETDDiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = ""
        lr_shape_node.Pen.Color = Color.White
        lr_shape_node.Transparent = True
        lr_shape_node.Image = My.Resources.resource_file.actor
        lr_shape_node.Visible = True

        Me.MorphVector(0).Shape = lr_shape_node

        Me.HiddenDiagram.Invalidate()

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

            Me.MorphVector(0) = New tMorphVector(Me.MorphVector(0).StartPoint.X, Me.MorphVector(0).StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Private Sub morph_to_DataFlowDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrFactDataInstance As New Object
        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.UseCaseDiagramView.CopyToClipboard(False)

        Me.HiddenDiagram.Nodes.Clear()
        Call Me.ETDDiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Ellipse
        lr_shape_node.Text = Me.zrPage.SelectedObject(0).Name
        lr_shape_node.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

        lr_shape_node.Visible = True

        Me.MorphVector(0).Shape = lr_shape_node

        Me.HiddenDiagram.Invalidate()

        Dim lrProcess As CMML.Process = Me.zrPage.SelectedObject(0)
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
            Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view
            prRichmondApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)

            lr_page = lr_enterprise_view.Tag

            '---------------------------------------------------------------
            'Populate the MorphVector with each Process Shape on the Page
            '---------------------------------------------------------------
            Dim lrAdditionalProcess As CMML.Process
            For Each lrAdditionalProcess In Me.ETD_model.Process
                If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
                    '---------------------------------------------------------------------------------------------
                    'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                    '---------------------------------------------------------------------------------------------
                Else
                    Dim lrEntityList = From FactTypeInstance In lr_page.FactTypeInstance _
                                       From Fact In FactTypeInstance.Fact _
                                       From FactData In Fact.Data _
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrAdditionalProcess.Name _
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                    Me.MorphVector.Add(New tMorphVector(lrAdditionalProcess.X, lrAdditionalProcess.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40))

                    lr_shape_node = lrAdditionalProcess.shape
                    lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
                    lr_shape_node.Shape = Shapes.Ellipse
                    lr_shape_node.Text = lrAdditionalProcess.Name
                    lr_shape_node.Visible = True
                    Me.HiddenDiagram.Nodes.Add(lr_shape_node)

                    Me.MorphVector(Me.MorphVector.Count - 1).Shape = lr_shape_node

                End If
            Next

            Select Case liConceptType
                Case Is = pcenumConceptType.Actor
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance _
                                  From Fact In FactType.Fact _
                                  From RoleData In Fact.Data _
                                  Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                                  And RoleData.Data = lrProcess.Name _
                                  Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                Case Is = pcenumConceptType.Process
                    Dim lrEntityList = From FactTypeInstance In lr_page.FactTypeInstance _
                                       From Fact In FactTypeInstance.Fact _
                                       From FactData In Fact.Data _
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.ElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrProcess.Name _
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
            End Select

            Me.MorphVector(0).EndPoint = New Point(lrFactDataInstance.x, lrFactDataInstance.y)
            Me.MorphVector(0).VectorSteps = 40
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Call Me.HiddenDiagramView.SendToBack()
        Me.MorphTimer.Enabled = False

        frmMain.zfrm_enterprise_tree_viewer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
        Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)

        Me.ETDDiagramView.BringToFront()
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

    Private Sub ETDDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ETDDiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF
        Dim loNode As DiagramNode

        lo_point = Me.ETDDiagramView.ClientToDoc(e.Location)

        Me.ETDDiagramView.SmoothingMode = SmoothingMode.AntiAlias

        '--------------------------------------------------
        'Just to be sure...set the Richmond.WorkingProject
        '--------------------------------------------------
        prRichmondApplication.WorkingPage = Me.zrPage

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
            Me.ETDDiagramView.DrawLinkCursor = Cursors.Hand
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
            '        prRichmondApplication.workingpage.MultiSelectionPerformed = True

            '        Select Case loNode.Tag.ConceptType
            '            Case Is = pcenumConceptType.role
            '                '----------------------------------------------------------------------
            '                'This stops a Role AND its FactType being selected at the same time
            '                '----------------------------------------------------------------------
            '                prRichmondApplication.workingpage.SelectedObject.Remove(loNode.Tag.FactType)
            '        End Select

            '        Exit Sub
            '    Else
            '        If prRichmondApplication.workingpage.MultiSelectionPerformed Then
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
            '                        prRichmondApplication.workingpage.SelectedObject.Clear()
            '                        Diagram.Selection.Clear()
            '                        '-----------------------------------------------
            '                        'Select the ShapeNode/ORMObject just clicked on
            '                        '-----------------------------------------------                    
            '                        loNode.Selected = True
            '                        loNode.Pen.Color = Color.Blue

            '                        '-------------------------------------------------------------------
            '                        'Reset the MultiSelectionPerformed flag on the ORMModel
            '                        '-------------------------------------------------------------------
            '                        prRichmondApplication.workingpage.MultiSelectionPerformed = False
            '                End Select
            '            Else
            '                '---------------------------------------------------------------------------
            '                'Clear the SelectedObjects because the user neither did a MultiSelection
            '                '  nor held down the [Ctrl] key before clicking on the ShapeNode.
            '                '  Clearing the SelectedObject groups, allows for new objects to be selected
            '                '  starting with the ShapeNode/ORMObject just clicked on.
            '                '---------------------------------------------------------------------------
            '                prRichmondApplication.workingpage.SelectedObject.Clear()
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
            prRichmondApplication.WorkingPage.SelectedObject.Clear()
            Me.Diagram.Selection.Clear()

            Me.ETDDiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

            Me.Diagram.AllowUnconnectedLinks = False

            '------------------------------------------------------------------------------
            'Clear the 'InPlaceEdit' on principal.
            '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas,
            '  so disable the 'InPlaceEdit'.
            '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on,
            '  then 'InPlaceEdit' is temporarily allowed.
            '------------------------------------------------------------------------------
            Me.ETDDiagramView.AllowInplaceEdit = False

            '-----------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel
            '-----------------------------------------------------------------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.ETD_model
            End If

            Call Me.reset_node_and_link_colors()
        End If

    End Sub

    Sub reset_node_and_link_colors()

        Dim liInd As Integer = 0

        '------------------------------------------------------------------------------------
        'Reset the border colors of the ShapeNodes (to what they were before being selected
        '------------------------------------------------------------------------------------
        'For liInd = 1 To Diagram.Nodes.Count
        '    Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
        '        Case Is = pcenumConceptType.EntityType
        '            Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
        '        Case Is = pcenumConceptType.FactType
        '            If Diagram.Nodes(liInd - 1).Tag.IsObjectified Then
        '                Diagram.Nodes(liInd - 1).Visible = True
        '                Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
        '            Else
        '                Diagram.Nodes(liInd - 1).Visible = False
        '                Diagram.Nodes(liInd - 1).Pen.Color = Color.Pink
        '            End If
        '        Case Is = pcenumConceptType.RoleName
        '            Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
        '        Case Is = pcenumConceptType.RoleConstraint
        '            Select Case Diagram.Nodes(liInd - 1).Tag.RoleConstraintType
        '                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
        '                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Maroon
        '                Case Is = pcenumRoleConstraintType.ExternalExclusionConstraint, _
        '                          pcenumRoleConstraintType.ExternalUniquenessConstraint
        '                    Diagram.Nodes(liInd - 1).Pen.Color = Color.White
        '            End Select
        '        Case Is = pcenumConceptType.FactTable
        '            Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
        '        Case Else
        '            Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
        '    End Select
        'Next

        'For liInd = 1 To Diagram.Links.Count
        '    Diagram.Links(liInd - 1).Pen.Color = Color.Black
        'Next

        Me.Diagram.Invalidate()

    End Sub

    Private Sub ETDDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ETDDiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loObject As Object
        Dim loNode As DiagramNode

        Me.ETDDiagramView.SmoothingMode = SmoothingMode.Default

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
            'For liInd = 1 To Diagram.Nodes.Count
            '    Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
            '        Case Else
            '            Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
            '    End Select
            'Next

            'For liInd = 1 To Diagram.Links.Count
            '    Diagram.Links(liInd - 1).Pen.Color = Color.Black
            'Next

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

            'Select Case loNode.Tag.ConceptType
            '    Case Is = pcenumConceptType.Actor
            '        loNode.Pen.Color = Color.Blue
            '    Case Else
            '        loNode.Pen.Color = Color.Blue
            'End Select

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

    Private Sub ContextMenuStrip_Process_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Process.Opening

        Dim lr_page As FBM.Page
        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_process As New CMML.Process


        If prRichmondApplication.WorkingPage.SelectedObject.Count = 0 Then
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
        Me.MorphVector.Clear()
        Me.MorphVector.Add(New tMorphVector(lr_process.X, lr_process.Y, 0, 0, 40))

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        Me.MenuOptionUseCaseDiagramProcess.DropDownItems.Clear()

        '=========================================================
        'Load the UseCaseDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '=========================================================

        '--------------------------------------------------------------------------------------------------------
        'The EntityType represents an Actor. i.e. Has a ParentEntityType of 'Actor' within the Core meta-schema
        '--------------------------------------------------------------------------------------------------------
        larPage_list = prRichmondApplication.CMML.get_use_case_diagram_pages_for_process(lr_process)


        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.MenuOptionUseCaseDiagramProcess.DropDownItems.Add(lr_page.Name)
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

        '=================================================================
        'Load the DFDDiagrams that relate to the EntityType as selectable menuOptions
        'Clear the list of DFDDiagrams that may relate to the EntityType
        '=================================================================
        Me.MenuOptionDataFlowDiagramProcess.DropDownItems.Clear()

        '--------------------------------------
        'The EntityType represents a Process.
        '--------------------------------------
        larPage_list = prRichmondApplication.CMML.get_DataFlowDiagram_pages_for_process(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '----------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '----------------------------------------------------
            lo_menu_option = Me.MenuOptionDataFlowDiagramProcess.DropDownItems.Add(lr_page.Name)
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

        '==============================================================================
        'Load the Flow Charts that relate to the EntityType as selectable menuOptions
        '==============================================================================
        Me.ToolStripMenuItemFlowChart.DropDownItems.Clear()

        larPage_list = prRichmondApplication.CMML.GetFlowChartDiagramPagesForProcess(lr_process)

        For Each lr_page In larPage_list
            Dim lo_menu_option As ToolStripItem
            '---------------------------------------------------
            'Add the Page(Name) to the MenuOption.DropDownItems
            '---------------------------------------------------
            lo_menu_option = Me.ToolStripMenuItemFlowChart.DropDownItems.Add(lr_page.Name)
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageFlowChart, _
                                                       lr_page, _
                                                       lr_page.Model.EnterpriseId, _
                                                       lr_page.Model.SubjectAreaId, _
                                                       lr_page.Model.ProjectId, _
                                                       lr_page.Model.SolutionId, _
                                                       lr_page.Model.ModelId, _
                                                       pcenumLanguage.FlowChart, _
                                                       Nothing, lr_page.PageId)
            lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
            AddHandler lo_menu_option.Click, AddressOf Me.MorphToFlowChartDiagram
        Next

    End Sub

    Private Sub PageAsORMMetaModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetaModelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrm_enterprise_tree_viewer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub

    Sub CopyImageToClipboard()

        Dim li_rectf As New RectangleF(0, 0, Diagram.Bounds.Width, Diagram.Bounds.Height)
        'li_rectf = Me.Diagram.GetContentBounds(False, True)

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

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Call Me.CopyImageToClipboard()

    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call Me.SetToolbox()

    End Sub

    Private Sub ETDDiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ETDDiagramView.MouseWheel

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
End Class