Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections.Generic
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports MindFusion.Diagramming.Lanes
Imports System.Reflection

Public Class frmDiagramBPMNCollaboration

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing

    Private mrPopupToolSelector As BPMNPopupToolSelector = Nothing
    Public mrPopupToolElementChanger As BPMNPopupToolElementChanger = Nothing

    ' Initialize grid
    Dim header As MindFusion.Diagramming.Lanes.Header = Nothing
    Dim grid As MindFusion.Diagramming.Lanes.Grid
    Dim ColumnHeaders As MindFusion.Diagramming.Lanes.HeaderCollection
    Dim RowHeaders As MindFusion.Diagramming.Lanes.HeaderCollection

    Private MorphVector As New List(Of tMorphVector)

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
        Me.ColumnHeaders = Me.grid.ColumnHeaders
        Me.RowHeaders = Me.grid.RowHeaders

    End Sub


    ''' <summary>
    ''' Loads the BPMN Collaboration Diagram given ORM-based BPMN meta-model Page
    ''' </summary>
    ''' <param name="arPage">The Page of the ORM Model within which the Event Trace Diagram is injected. _
    '''  The Boston data model is an ORM meta-model. BPMN Diagrams are stored as propositional functions within the ORM meta-model.</param>
    ''' <remarks></remarks>
    Sub LoadBPMNCollaborationDiagramPage(ByRef arPage As FBM.Page)

        Dim lrFactInstance As FBM.FactInstance
        Dim lrFactDataInstance As FBM.FactDataInstance
        Dim lrProcess As BPMN.Process

        'Clear the Actors, Processes etc so that they can be re/loaded.
        Me.zrPage.UMLDiagram.Reset()

        grid.MinHeaderSize = 8
        grid.HookHeaders = False
        grid.HeadersOnTop = False
        grid.ColumnHeadersHeights = New Single() {1, 0}
        grid.RowHeadersWidths = New Single() {10, 0}
        grid.AlignCells = False
        grid.AllowResizeHeaders = True
        grid.TopMargin = 10
        grid.LeftMargin = 10

        Me.zrPage.UMLDiagram.Actor.Clear()
        Me.grid.ColumnHeaders.Clear()
        Me.grid.RowHeaders.Clear()
        Me.RowHeaders.Clear()
        Me.ColumnHeaders.Clear()

        'header = New Header("Week " + i.ToString() + ", 2007")
        'header.SubHeaders.Add(New Header("S"))
        'header.SubHeaders.Add(New Header("M"))
        'header.SubHeaders.Add(New Header("T"))
        'header.SubHeaders.Add(New Header("W"))
        'header.SubHeaders.Add(New Header("T"))
        'header.SubHeaders.Add(New Header("F"))
        'header.SubHeaders.Add(New Header("S"))
        'columns.Add(header)

        Dim lsOverallProcessName As String = "New Process"
        Dim lrRecordset As New ORMQL.Recordset
        Dim lsSqlQuery As String = ""

        Try

#Region "Overall Process Name"

            lsSqlQuery = "SELECT * "
            lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreBPMNCollaborationHasCoreBPMNCollaborationName.ToString
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

            If Not lrRecordset.EOF Then
                lsOverallProcessName = lrRecordset("CollaborationName").Data
            End If

#End Region

#Region "Actors"
            lsSqlQuery = "SELECT DISTINCT Element "
            lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE ElementType = 'Actor'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

            While Not lrRecordset.EOF

                Dim lrCMMLActor = Me.zrPage.Model.UML.Actor.Find(Function(x) x.Name = lrRecordset("Element").Data)

                Dim lrActor As New BPMN.Actor(Me.zrPage, lrCMMLActor)
                lrFactDataInstance = lrRecordset("Element")
                lrActor = lrFactDataInstance.CloneBPMNActor(Me.zrPage)

                '----------------------------------------------
                'CMML
                lrActor.CMMLActor = Me.zrPage.Model.UML.Actor.Find(Function(x) x.Name = lrActor.Name)

                'Dim lrRecordsetSequenceNr As New ORMQL.Recordset
                'lsSqlQuery = "SELECT *"
                'lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreElementSequenceNr.ToString
                'lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                'lsSqlQuery &= " WHERE Element = '" & lrActor.Name & "'"

                'lrRecordsetSequenceNr = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

                'lrActor.SequenceNr = Convert.ToSingle(lrRecordsetSequenceNr("SequenceNr").Data)

                Me.zrPage.UMLDiagram.Actor.Add(lrActor)

                lrRecordset.MoveNext()
            End While

            Dim loDiagramHeader = New Header(lsOverallProcessName)
            loDiagramHeader.RotateTitle = True
            Me.grid.RowHeaders.Add(loDiagramHeader)

            Dim lrActorInSequence As UML.Actor
            'Me.zrPage.UMLDiagram.Actor.Sort(AddressOf CMML.Actor.CompareSequenceNrs) '20220624-VM-Not currently used.
            For Each lrActorInSequence In Me.zrPage.UMLDiagram.Actor
                header = New Header(lrActorInSequence.Name)
                loDiagramHeader.SubHeaders.Add(header)
                'Me.RowHeaders.Add(header)
                header.Width = 10 'Me.Diagram.Bounds.Width / Me.zrPage.UMLDiagram.Actor.Count
                header.Height = 40
                header.TitleFormat.Alignment = StringAlignment.Center
                header.TitleFormat.LineAlignment = StringAlignment.Center
                header.RotateTitle = True
            Next
#End Region

            Dim lrColumnHeader As New MindFusion.Diagramming.Lanes.Header()
            lrColumnHeader.Width = 200
            Me.grid.ColumnHeaders.Add(lrColumnHeader)

#Region "Processes"
            lsSqlQuery = "SELECT DISTINCT Element "
            lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            lsSqlQuery &= " WHERE ElementType = 'Process'"

            lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)
            While Not lrRecordset.EOF

                lrFactDataInstance = lrRecordset("Element")
                lrProcess = lrFactDataInstance.CloneBPMNProcess(arPage)
                lrProcess.X = lrFactDataInstance.X
                lrProcess.Y = lrFactDataInstance.Y
                lrProcess.CMMLProcess = Me.zrPage.Model.UML.Process.Find(Function(x) x.Id = lrProcess.Id)

#Region "Process Text - Get from CMML"

                lsSqlQuery = "SELECT *"
                lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                lsSqlQuery &= " WHERE Process = '" & lrProcess.Id & "'"

                Dim lrRecordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

                If Not lrRecordset2.EOF Then
                    lrProcess.Text = lrRecordset2("ProcessText").Data
                End If
#End Region

                Call lrProcess.DisplayAndAssociate()

                Me.zrPage.UMLDiagram.Process.Add(lrProcess)

                lrRecordset.MoveNext()
            End While
#End Region

            Me.zrPage.UMLDiagram.ActorToProcessParticipationRelationFTI = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString)
            Me.zrPage.UMLDiagram.PocessToProcessRelationFTI = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString)

#Region "Process to Process Participation Relations"

            Dim larProcessId = (From Process In Me.zrPage.UMLDiagram.Process
                                Select Process.Id).ToList

            Dim larCMMLProcessProcessRelation = From ProcessProcessRelation In Me.zrPage.Model.UML.ProcessProcessRelation
                                                Where larProcessId.Contains(ProcessProcessRelation.Process1.Id)
                                                Select ProcessProcessRelation

            Dim lrProcess1, lrProcess2 As BPMN.Process

            For Each lrCMMLProcessProcessRelation In larCMMLProcessProcessRelation

                lrProcess1 = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLProcessProcessRelation.Process1.Id)
                lrProcess2 = Me.zrPage.UMLDiagram.Process.Find(Function(x) x.Id = lrCMMLProcessProcessRelation.Process2.Id)

                If lrProcess1 IsNot Nothing And lrProcess2 IsNot Nothing Then

                    lsSqlQuery = "SELECT *"
                    lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                    lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                    lsSqlQuery &= " WHERE Process1 = '" & lrProcess1.Id & "'"
                    lsSqlQuery &= " AND Process2 = '" & lrProcess2.Id & "'"

                    lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

                    Dim lrBPMNProcessProcessRelation As BPMN.ProcessProcessRelation
                    If lrRecordset.EOF Then
                        lrFactInstance = Me.zrPage.UMLDiagram.PocessToProcessRelationFTI.AddFact(lrCMMLProcessProcessRelation.Fact)
                    Else
                        lrFactInstance = lrRecordset.CurrentFact
                    End If

                    lrBPMNProcessProcessRelation = lrFactInstance.CloneBPMNProcessProcessRelation(Me.zrPage, lrProcess1, lrProcess2)

                    '------------------------------------------
                    'Link the Processes
                    '------------------------------------------                    
                    lrBPMNProcessProcessRelation.Fact = lrFactInstance
                    lrBPMNProcessProcessRelation.IsExtends = lrCMMLProcessProcessRelation.IsExtends
                    lrBPMNProcessProcessRelation.IsIncludes = lrCMMLProcessProcessRelation.IsIncludes
                    lrBPMNProcessProcessRelation.CMMLProcessProcessRelation = lrCMMLProcessProcessRelation 'Me.zrPage.Model.UML.ProcessProcessRelation.Find(Function(x) x.Process1.Id = lrProcess1.Id And x.Process2.Id = lrProcess2.Id)
                    lrBPMNProcessProcessRelation.IsExtends = lrCMMLProcessProcessRelation.IsExtends

                    Me.zrPage.UMLDiagram.ProcessProcessRelation.Add(lrBPMNProcessProcessRelation)

                    Dim lo_link As DiagramLink
                    lo_link = Me.Diagram.Factory.CreateDiagramLink(lrProcess1.Shape, lrProcess2.Shape)
                    lo_link.AutoRoute = True
                    lrBPMNProcessProcessRelation.Link = lo_link
                    lo_link.Tag = lrBPMNProcessProcessRelation

                End If
            Next
#End Region

            '        '==================================
            '        'Draw the Processes
            '        '==================================
            '        lsSqlQuery = "SELECT *"
            '        lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
            '        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"


            '        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)
            '        While Not lrRecordset.EOF

            '            lrProcess = New UML.Process(Me.zrPage, lrRecordset("Process").Data, "DummyProcessName")
            '            lrProcess = Me.zrPage.UMLDiagram.Process.Find(AddressOf lrProcess.EqualsByName)

            '            Dim lrRecordsetSequenceNr As New ORMQL.Recordset
            '            lsSqlQuery = "SELECT *"
            '            lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreElementSequenceNr.ToString
            '            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSqlQuery &= " WHERE Element = '" & lrRecordset("Process").Data & "'"

            '            lrRecordsetSequenceNr = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)
            '            lrProcess.SequenceNr = Convert.ToSingle(lrRecordsetSequenceNr("SequenceNr").Data)

            '            '-------------------------------------------
            '            'Check to see if the Process is a decision
            '            '-------------------------------------------
            '            Dim lrRecordset1 As ORMQL.Recordset

            '            lsSqlQuery = "SELECT *"
            '            lsSqlQuery &= " FROM ProcessIsDecision"
            '            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

            '            If lrRecordset1.Facts.Count > 0 Then
            '                lrProcess.IsDecision = True
            '            End If

            '            '------------------------------------------
            '            'Check if the Process is a Start Process
            '            '------------------------------------------
            '            lsSqlQuery = "SELECT *"
            '            lsSqlQuery &= " FROM ProcessIsStart"
            '            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

            '            If lrRecordset1.Facts.Count > 0 Then
            '                lrProcess.IsStart = True
            '            End If

            '            '------------------------------------------
            '            'Check if the Process is a Stop Process
            '            '------------------------------------------
            '            lsSqlQuery = "SELECT *"
            '            lsSqlQuery &= " FROM ProcessIsStop"
            '            lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
            '            lsSqlQuery &= " WHERE Process = '" & lrProcess.Name & "'"

            '            lrRecordset1 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

            '            If lrRecordset1.Facts.Count > 0 Then
            '                lrProcess.IsStop = True
            '            End If

            '            '---------------------------
            '            'Set the responsible Actor
            '            '---------------------------
            '            Dim lrActor As New UML.Actor(zrPage)
            '            lrActor.Name = lrRecordset("Actor").Data
            '            lrActor = Me.zrPage.UMLDiagram.Actor.Find(AddressOf lrActor.EqualsByName)
            '            lrProcess.ResponsibleActor = lrActor

            '            lrProcess.DisplayAndAssociate()
            '            lrProcess.Shape.Resize(8, 10)

            '            If lrProcess.IsDecision Then
            '                lrProcess.Shape.Shape = Shapes.Decision
            '            ElseIf lrProcess.IsStart Then
            '                lrProcess.Shape.Shape = Shapes.Ellipse
            '            ElseIf lrProcess.IsStop Then
            '                lrProcess.Shape.Shape = Shapes.Ellipse
            '            End If

            '            lrRecordset.MoveNext()
            '        End While

            '        '===========================================
            '        'Move the Processes to the respective Lane
            '        '===========================================   
            '        Dim liRowLane As Integer = 0
            '        Dim lrRowHeader As New Header

            '        Me.zrPage.UMLDiagram.Process.Sort(AddressOf UML.Process.CompareSequenceNrs)

            '        For Each lrProcess In Me.zrPage.UMLDiagram.Process
            '            If liRowLane <> lrProcess.SequenceNr Then
            '                liRowLane = lrProcess.SequenceNr
            '                lrRowHeader = New Header("Flow " + liRowLane.ToString)
            '                lrRowHeader.Height = 12
            '                lrRowHeader.TitleFormat.Alignment = StringAlignment.Center
            '                lrRowHeader.TitleFormat.LineAlignment = StringAlignment.Center
            '                grid.RowHeaders.Add(lrRowHeader)
            '            End If
            '            Dim column As Header = grid.GetColumn(1)
            '            'lrProcess.shape.Move(grid.GetHeaderBounds(Me.columns(1), True).X, grid.GetHeaderBounds(lrRowHeader, False).Y)
            '            Dim liX As Integer
            '            liX = grid.GetHeaderBounds(grid.FindColumn(lrProcess.ResponsibleActor.Name), True).X
            '            liX += (grid.FindColumn(lrProcess.ResponsibleActor.Name).Width / 2) - (lrProcess.Shape.Bounds.Width / 2)
            '            lrProcess.Shape.Move(liX, grid.GetHeaderBounds(lrRowHeader, False).Y)
            '            lrProcess.X = liX
            '            lrProcess.Y = lrProcess.Shape.Bounds.Y
            '        Next
            '#End Region

            '        '==================================
            '        'Draw the links between Processes
            '        '==================================
            '#Region "ProcessProcessRelations"

            '        lsSqlQuery = "SELECT *"
            '        lsSqlQuery &= " FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
            '        lsSqlQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

            '        lrRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)


            '        While Not lrRecordset.EOF

            '            Dim lrProcess1 As New UML.Process(Me.zrPage, lrRecordset("Process1").Data, "DummyProcessName")
            '            lrProcess1.FactData.Model = Me.zrPage.Model
            '            Dim lrProcess2 As New UML.Process(Me.zrPage, lrRecordset("Process2").Data, "DummyProcessName")
            '            lrProcess2.FactData.Model = Me.zrPage.Model

            '            lrProcess1 = Me.zrPage.UMLDiagram.Process.Find(AddressOf lrProcess1.EqualsByName)
            '            lrProcess2 = Me.zrPage.UMLDiagram.Process.Find(AddressOf lrProcess2.EqualsByName)

            '            Dim loEntityLink As DiagramLink
            '            loEntityLink = Me.Diagram.Factory.CreateDiagramLink(lrProcess1.Shape, lrProcess2.Shape)
            '            loEntityLink.SnapToNodeBorder = True
            '            loEntityLink.ShadowColor = Color.White
            '            loEntityLink.Style = LinkStyle.Cascading
            '            loEntityLink.SegmentCount = 3
            '            loEntityLink.HeadShape = ArrowHead.Arrow
            '            loEntityLink.HeadShapeSize = 3
            '            If lrProcess1.Shape.Bounds.X < lrProcess2.Shape.Bounds.X Then
            '                loEntityLink.Pen = New MindFusion.Drawing.Pen(Color.Blue, 0.4)
            '            Else
            '                loEntityLink.Pen = New MindFusion.Drawing.Pen(Color.BlueViolet, 0.4)
            '            End If
            '            loEntityLink.Brush = New MindFusion.Drawing.SolidBrush(Color.Black)

            '            lrRecordset.MoveNext()
            '        End While
            '#End Region

            Me.Diagram.RouteAllLinks()

            Dim count As Integer
            Dim i As Integer
            count = Me.ColumnHeaders.Count
            For i = 0 To Me.grid.ColumnCount - 1

                Dim column As Header = grid.GetColumn(i)

                Try
                    grid(column, Nothing).Style.LeftBorderPen =
                        New MindFusion.Drawing.Pen(
                            New MindFusion.Drawing.HatchBrush(HatchStyle.Percent50, Color.Gray, Color.Transparent), 0)

                    If i <> count - 1 Then
                        grid(column, Nothing).Style.RightBorderPen = Nothing
                    End If
                Catch ex As Exception
                    '20220624-VM-Do nothing at this stage.
                End Try

            Next i


            count = Me.grid.RowCount
            For i = 0 To count - 1

                Dim row As Header = grid.GetRow(i)

                Try
                    grid(Nothing, row).Style.TopBorderPen =
                 New MindFusion.Drawing.Pen(
                  New MindFusion.Drawing.SolidBrush(Color.Black)) 'HatchStyle(HatchStyle.Percent50,Color.Gray, Color.Transparent), 0)

                    If i <> count - 1 Then
                        grid(Nothing, row).Style.BottomBorderPen = Nothing
                    End If
                Catch ex As Exception
                    '20220624-VM-Do nothing at this stage.
                End Try
            Next i

            Me.Diagram.EnableLanes = True

            ' Ensure the document is big enough to contain the grid
            Dim width As Single = Math.Max(50, grid.ColumnCount * 6 + 20)
            Dim height As Single = Math.Max(50, grid.RowCount * 6 + 20)

            Me.Diagram.Bounds = New RectangleF(0, 0, width, height)
            Me.Diagram.AlignToGrid = False

            Me.Diagram.Invalidate()
            Me.zrPage.FormLoaded = True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub SetToolbox()

        Try


            Dim loShapeLibrary As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.BPMNShapeLibraryCore)
            Dim loShape As Shape
            Dim lrToolboxForm As frmToolbox
            lrToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)

            If IsSomething(lrToolboxForm) Then

                lrToolboxForm.ShapeListBox.IconSize = New Size(40, 40)
                lrToolboxForm.ShapeListBox.BackColor = Color.White
                lrToolboxForm.ShapeListBox.BorderStyle = BorderStyle.Fixed3D
                lrToolboxForm.ShapeListBox.ForeColor = Color.Black
                lrToolboxForm.ShapeListBox.Shapes = loShapeLibrary.Shapes

                For Each loShape In lrToolboxForm.ShapeListBox.Shapes

                    Select Case loShape.DisplayName
                        Case Is = "Start Event"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Event_Start_None_Standard
                        Case Is = "Intermediate/Boundary Event"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Event_Intermediate_Boundary
                        Case Is = "End Event"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Event_End_None_Standard
                        Case Is = "Gateway"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Gateway_Exclusive
                        Case Is = "Task"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Task
                        Case Is = "Sub Process"
                            loShape.Image = My.Resources.BPMNToolboxShapes.SubProcess
                        Case Is = "Data Object Reference"
                            loShape.Image = My.Resources.BPMNToolboxShapes.DataObject
                        Case Is = "Data Store Reference"
                            loShape.Image = My.Resources.BPMNToolboxShapes.DataStore
                        Case Is = "Pool/Participant"
                            loShape.Image = My.Resources.BPMNToolboxShapes.PoolParticipant
                        Case Is = "Group"
                            loShape.Image = My.Resources.BPMNToolboxShapes.Group
                    End Select
                Next

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub ArrangeLinks()

        Me.Diagram.RoutingOptions.GridSize = 2
        Me.Diagram.RouteLinks = True

        Me.Diagram.RouteAllLinks()

        Exit Sub

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
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X + l.Origin.Bounds.Width,
                    l.Origin.Bounds.Y + l.Origin.Bounds.Height / 2)
                    l.ControlPoints(l.ControlPoints.Count - 1) = New PointF(l.Destination.Bounds.X,
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
                    l.ControlPoints(0) = New PointF(l.Origin.Bounds.X,
                    l.Origin.Bounds.Y + l.Origin.Bounds.Height / 2)
                    l.ControlPoints(l.ControlPoints.Count - 1) = New PointF(l.Destination.Bounds.X + l.Destination.Bounds.Width,
                     l.Destination.Bounds.Y + l.Destination.Bounds.Height / 2)
                    Diagram.RoutingOptions.StartOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    Diagram.RoutingOptions.EndOrientation = MindFusion.Diagramming.Orientation.Horizontal
                    l.Route()
                End If
            End If
            l.UpdateFromPoints()
        Next
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
                Case Is = lo_dummy_object.GetType.ToString
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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

        ArrangeLinks()
    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Private Sub Diagram_NodeCreated(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeCreated

        '20220624-VM-Not in UseCaseDiagram...Probably shouldn't be in BPMN. Check.

        'Dim node As ShapeNode = CType(e.Node, ShapeNode)

        'node.HandlesStyle = HandlesStyle.DashFrame
        'If node.Shape.Id = "SequenceObject" Then
        '    node.AllowOutgoingLinks = False
        '    node.AllowIncomingLinks = False
        '    node.EnabledHandles = AdjustmentHandles.None
        '    node.Brush = New MindFusion.Drawing.LinearGradientBrush(Color.White, Color.LimeGreen, 90)
        '    zNodeColection.Add(node)
        'End If

        'Dim i As Integer
        'For i = 0 To zNodeColection.Count - 1
        '    zNodeColection(i).Bounds = New RectangleF(44 * i + 10, 10, 24, 24)
        'Next
        'If node.Shape.Id = "SequenceMessage" Then
        '    node.Brush = New MindFusion.Drawing.LinearGradientBrush(Color.White, Color.Red, 90)
        '    zTextColection.Add(node)
        '    Dim j As Integer
        '    For j = 0 To zNodeColection.Count - 1
        '        If node.Bounds.X > zNodeColection(j).Bounds.X - 20 And
        '        node.Bounds.X < zNodeColection(j).Bounds.X + 30 Then
        '            node.EnabledHandles = AdjustmentHandles.ResizeBottomCenter Or AdjustmentHandles.Move Or AdjustmentHandles.ResizeTopCenter
        '            node.Constraints.MoveDirection = DirectionConstraint.Vertical
        '            node.Bounds = New RectangleF(zNodeColection(j).Bounds.X + 6,
        '                e.Node.Bounds.Y, 12, 24)
        '            Exit For
        '        End If
        '    Next
        'End If

        'Me.ETDDiagramView.EndEdit(True)
        'Me.ETDDiagramView.BeginEdit(node)
        'Diagram.Invalidate()
        'Me.ETDDiagramView.Invalidate()

    End Sub

    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        '------------------------------------------------------------------------------------------
        'NB IMPORTANT: DiagramView.MouseDown gets processed before Diagram.NodeSelected, so management of which object
        '  is displayed in the PropertyGrid is performed in ORMDiagram.MouseDown
        '------------------------------------------------------------------------------------------
        Try
            'CodeSafe 
            If e.Node.Tag Is Nothing Then Exit Sub

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
                        'Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Actor
                    Case Is = pcenumConceptType.Process
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Process
                    Case Else
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
                End Select

            End If


#Region "Popup Tool Selector"

            Try
                'GoTo SkipPopup
                Dim lrPopupToolSelector = New BPMNPopupToolSelector()
                lrPopupToolSelector.mrCMMLModel = Me.zrPage.Model.UML
                Dim liX, liY As Integer
                Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
                    Case Is = pcenumConceptType.Process
                        Dim lrBPMNProcess As BPMN.Process = Me.zrPage.SelectedObject(0)
                        lrPopupToolSelector.msLinkedProcessId = lrBPMNProcess.Id
                        lrPopupToolSelector.Type = lrBPMNProcess.CMMLProcess.ProcessType
                        liX = lrBPMNProcess.X + lrBPMNProcess.Shape.Bounds.Width + 5
                        liY = lrBPMNProcess.Y
                End Select
                lrPopupToolSelector.moDiagram = Me.Diagram
                lrPopupToolSelector.mrPage = Me.zrPage
                lrPopupToolSelector.Tag = lrPopupToolSelector
                If Me.mrPopupToolSelector IsNot Nothing Then
                    Me.Diagram.Nodes.Remove(Me.mrPopupToolSelector.Node)
                End If
                Me.mrPopupToolSelector = lrPopupToolSelector
                Dim lrControlNode As New MindFusion.Diagramming.WinForms.ControlNode(Me.DiagramView, lrPopupToolSelector)
                lrPopupToolSelector.Node = lrControlNode
                Me.zrPage.Diagram.Nodes.Add(lrControlNode)
                lrControlNode.AttachTo(e.Node, AttachToNode.TopCenter)
                lrPopupToolSelector.AttachedToNode = e.Node
                Call lrControlNode.Move(liX, liY)
SkipPopup:
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
#End Region

            Call Me.reset_node_and_link_colors()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ETDDiagramView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call Me.SetToolbox()

    End Sub

    Private Sub ContextMenuStrip_Actor_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs)

        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_actor As New UML.Actor


        If prApplication.WorkingPage.SelectedObject.Count = 0 Then
            Exit Sub
        End If

        '-------------------------------------
        'Check that selected object is Actor
        '-------------------------------------
        If lr_actor.GetType Is prApplication.WorkingPage.SelectedObject(0).GetType Then
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
        lr_actor = prApplication.WorkingPage.SelectedObject(0)

        lr_model = lr_actor.Model

        '---------------------------------------------------------------------------------------------
        'Set the initial MorphVector for the selected EntityType. Morphing the EntityType to another 
        '  shape, and to/into another diagram starts at the MorphVector.
        '---------------------------------------------------------------------------------------------
        Me.MorphVector.Add(New tMorphVector(lr_actor.X, lr_actor.Y, 0, 0, 40))

        '---------------------------------------------------------------------
        'Clear the list of UseCaseDiagrams that may relate to the EntityType
        '---------------------------------------------------------------------
        'Me.MenuOptionUseCaseDiagramActor.DropDownItems.Clear()

        '--------------------------------------------------------
        'Load the UseCaseDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '--------------------------------------------------------

        '--------------------------------------------------------------------------------------------------------
        'The EntityType represents an Actor. i.e. Has a ParentEntityType of 'Actor' within the Core meta-schema
        '--------------------------------------------------------------------------------------------------------
        '=======================================================================================================================
        '20220528-VM-Commenented out for the initial implemenation of BPMN
        'larPage_list = prApplication.CMML.get_use_case_diagram_pages_for_actor(lr_actor)


        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '---------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '---------------------------------------------------
        '    lo_menu_option = Me.MenuOptionUseCaseDiagramActor.DropDownItems.Add(lr_page.Name)
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

        ''--------------------------------------------------------------
        ''Clear the list of ORMDiagrams that may relate to the EntityType
        ''--------------------------------------------------------------
        'Me.MenuOptionORMDiagramActor.DropDownItems.Clear()

        ''--------------------------------------------------------
        ''Load the ORMDiagrams that relate to the Actor
        ''  as selectable menuOptions
        ''--------------------------------------------------------                        
        'larPage_list = prApplication.CMML.get_orm_diagram_pages_for_actor(lr_actor)

        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem

        '    '----------------------------------------------------------
        '    'Try and find the Page within the EnterpriseView.TreeView
        '    '  NB If 'Core' Pages are not shown for the model, 
        '    '  they will not be in the TreeView and so a menuOption
        '    '  is now added for those hidden Pages.
        '    '----------------------------------------------------------
        '    Dim lr_enterprise_view As tEnterpriseEnterpriseView
        '    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
        '                                               lr_page,
        '                                               lr_model.EnterpriseId,
        '                                               lr_model.SubjectAreaId,
        '                                               lr_model.ProjectId,
        '                                               lr_model.SolutionId,
        '                                               lr_model.ModelId,
        '                                               pcenumLanguage.ORMModel,
        '                                               Nothing,
        '                                               lr_page.PageId)

        '    lr_enterprise_view = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '    If IsSomething(lr_enterprise_view) Then
        '        '---------------------------------------------------
        '        'Add the Page(Name) to the MenuOption.DropDownItems
        '        '---------------------------------------------------
        '        lo_menu_option = Me.MenuOptionORMDiagramActor.DropDownItems.Add(lr_page.Name)
        '        lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '        AddHandler lo_menu_option.Click, AddressOf Me.morph_to_ORM_diagram
        '    End If
        'Next

        ''---------------------------------------------------------------------
        ''Clear the list of DataFlowDiagrams that may relate to the EntityType
        ''---------------------------------------------------------------------
        'Me.MenuOptionDataFlowDiagramActor.DropDownItems.Clear()

        ''--------------------------------------------------------
        ''Load the DataFlowDiagrams that relate to the EntityType
        ''  as selectable menuOptions
        ''--------------------------------------------------------
        'larPage_list = prApplication.CMML.get_DataFlowDiagram_pages_for_actor(lr_actor)

        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '---------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '---------------------------------------------------
        '    lo_menu_option = Me.MenuOptionDataFlowDiagramActor.DropDownItems.Add(lr_page.Name)

        '    Dim lr_enterprise_view As tEnterpriseEnterpriseView
        '    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageUseCaseDiagram,
        '                                               lr_page,
        '                                               lr_page.Model.EnterpriseId,
        '                                               lr_page.Model.SubjectAreaId,
        '                                               lr_page.Model.ProjectId,
        '                                               lr_page.Model.SolutionId,
        '                                               lr_page.Model.ModelId,
        '                                               pcenumLanguage.DataFlowDiagram,
        '                                               Nothing, lr_page.PageId)

        '    lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '    AddHandler lo_menu_option.Click, AddressOf Me.morph_to_DataFlowDiagram
        'Next
        '============================================================================================================================
        'Commented out to here. 20220528-VM

    End Sub

    Public Sub morph_to_ORM_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If prApplication.WorkingPage.SelectedObject.Count = 0 Then Exit Sub
        If prApplication.WorkingPage.SelectedObject.Count > 1 Then Exit Sub

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

        Dim lr_actor As New UML.Actor
        lr_actor = prApplication.WorkingPage.SelectedObject(0)

        Dim lr_shape_node As ShapeNode


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

            If lr_page.Loaded Then
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

    Public Sub MorphToFlowChartDiagram(ByVal sender As Object, ByVal e As EventArgs)

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

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lrEnterpriseView As tEnterpriseEnterpriseView
            lrEnterpriseView = lrMenuItem.Tag
            Me.MorphVector(0).EnterpriseTreeView = lrEnterpriseView
            prApplication.WorkingPage = lrEnterpriseView.Tag

            '---------------------------------------------
            'Set the Page that is going to be morphed to
            '---------------------------------------------
            Dim lrPage As New FBM.Page(lrEnterpriseView.Tag.Model)
            lrPage = lrEnterpriseView.Tag

            '----------------------------------------------------------------------
            'Populate the MorphVector with each Process Shape on the current Page
            '  that is also on the destination Page.
            '----------------------------------------------------------------------
            Dim lrAdditionalProcess As UML.Process
            For Each lrAdditionalProcess In Me.zrPage.UMLDiagram.Process
                If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
                    '---------------------------------------------------------------------------------------------
                    'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                    '---------------------------------------------------------------------------------------------
                Else
                    Dim lrEntityList = From FactTypeInstance In lrPage.FactTypeInstance
                                       From Fact In FactTypeInstance.Fact
                                       From FactData In Fact.Data
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
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
                                      Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
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

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

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
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.HiddenDiagram.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = ""
        lr_shape_node.Pen.Color = Color.White
        lr_shape_node.Transparent = True
        lr_shape_node.Image = My.Resources.CMML.actor
        lr_shape_node.Visible = True

        Me.MorphVector(0).Shape = lr_shape_node

        Me.HiddenDiagram.Invalidate()

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
        Call Me.DiagramView.SendToBack()
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

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            Me.MorphVector(0).EnterpriseTreeView = lr_enterprise_view
            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Process/Entity being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)

            lr_page = lr_enterprise_view.Tag

            '---------------------------------------------------------------
            'Populate the MorphVector with each Process Shape on the Page
            '---------------------------------------------------------------
            Dim lrAdditionalProcess As UML.Process
            For Each lrAdditionalProcess In Me.zrPage.UMLDiagram.Process
                If lrAdditionalProcess.Name = Me.zrPage.SelectedObject(0).Name Then
                    '---------------------------------------------------------------------------------------------
                    'Skip. Is already added to the MorphVector collection when the ContextMenu.Diagram as loaded
                    '---------------------------------------------------------------------------------------------
                Else
                    Dim lrEntityList = From FactTypeInstance In lr_page.FactTypeInstance
                                       From Fact In FactTypeInstance.Fact
                                       From FactData In Fact.Data
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrAdditionalProcess.Name
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, FactData.Role, FactData.Concept, FactData.X, FactData.Y)
                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                    Me.MorphVector.Add(New tMorphVector(lrAdditionalProcess.X, lrAdditionalProcess.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40))

                    lr_shape_node = lrAdditionalProcess.Shape
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
                    Dim lrEntityList = From FactType In lr_page.FactTypeInstance
                                       From Fact In FactType.Fact
                                       From RoleData In Fact.Data
                                       Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                                  And RoleData.Data = lrProcess.Name
                                       Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

                    For Each lrFactDataInstance In lrEntityList
                        Exit For
                    Next
                Case Is = pcenumConceptType.Process
                    Dim lrEntityList = From FactTypeInstance In lr_page.FactTypeInstance
                                       From Fact In FactTypeInstance.Fact
                                       From FactData In Fact.Data
                                       Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                                       And FactData.Role.Name = "Element" _
                                       And FactData.Concept.Symbol = lrProcess.Name
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

        frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphVector(0).EnterpriseTreeView.TreeNode
        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

        Me.DiagramView.BringToFront()
        Me.Diagram.Invalidate()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lr_point As New Point
        Dim lr_rect As New Rectangle
        Dim lrMorphVector As tMorphVector

        For Each lrMorphVector In Me.MorphVector
            lr_point = lrMorphVector.getNextMorphVectorStepPoint
            lrMorphVector.Shape.Move(lr_point.X, lr_point.Y)
        Next

        Me.HiddenDiagram.Invalidate()

        If Me.MorphVector(0).VectorStep > Me.MorphVector(0).VectorSteps Then
            Me.MorphStepTimer.Enabled = False
        End If

    End Sub

    Private Sub BPMNDiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF

        Try

            lo_point = Me.DiagramView.ClientToDoc(e.Location)

            Dim loNode = Diagram.GetNodeAt(lo_point)
            Dim loLink = Diagram.GetLinkAt(lo_point, 1)

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

#Region "Popup Tools for creating/changing ModelElements on the Page."
            If Me.mrPopupToolSelector IsNot Nothing And loNode Is Nothing And loLink Is Nothing Then
                Me.Diagram.Nodes.Remove(Me.mrPopupToolSelector.Node)
                Me.mrPopupToolSelector = Nothing
            End If
            If Me.mrPopupToolElementChanger IsNot Nothing And loNode Is Nothing And loLink Is Nothing Then
                Me.Diagram.Nodes.Remove(Me.mrPopupToolElementChanger.Node)
                Me.mrPopupToolElementChanger = Nothing
            End If
#End Region

            '--------------------------------------------------
            'Just to be sure...set the Boston.WorkingProject
            '--------------------------------------------------
            prApplication.WorkingPage = Me.zrPage

            '=====================================================================================================================
            ''CreateLink if CreatingLink: See PopopToolSelector
            '======================================================
#Region "PopupToolSelector-LinkCreating"
            If Me.zrPage.SelectedObject.Count = 1 And loNode IsNot Nothing Then
                Select Case Me.zrPage.SelectedObject(0).GetType
                    Case Is = GetType(BPMN.Process)

                        Dim lrOriginalBPMNProcess As BPMN.Process = Me.zrPage.SelectedObject(0)

                        If loNode.Tag.GetType = GetType(BPMN.Process) And lrOriginalBPMNProcess.IsCreatingLink Then
                            Dim lrCMMLProcess1 As CMML.Process = lrOriginalBPMNProcess.CMMLProcess
                            Dim lrCMMLProcess2 As CMML.Process = loNode.Tag.CMMLProcess

                            Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(Me.zrPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                            Me.zrPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)
                        End If
                End Select

            End If
#End Region
            '=====================================================================================================================

            Me.zrPage.SelectedObject.Clear()

            If loLink IsNot Nothing Then
                Me.zrPage.SelectedObject.AddUnique(loLink.Tag)
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_ProcessLink

            ElseIf IsSomething(loNode) Then
#Region "Node Processing"
                '----------------------------
                'Mouse is over an ShapeNode
                '----------------------------
                Me.Diagram.AllowUnconnectedLinks = True
                Me.zrPage.SelectedObject.Clear()
                Me.zrPage.SelectedObject.AddUnique(loNode.Tag)
                Me.Diagram.Selection.Clear()

                Select Case loNode.GetType
                    Case Is = GetType(BPMN.Process)
                        Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Process
                End Select
                loNode.Selected = True

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
                Me.DiagramView.DrawLinkCursor = Cursors.Hand
                Cursor.Show()

                '---------------------------------------------------------------------------------------------------------------------------
                'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel object selected
                '---------------------------------------------------------------------------------------------------------------------------
                If Not IsNothing(frmMain.zfrm_properties) Then
                    frmMain.zfrm_properties.PropertyGrid.SelectedObject = loNode.Tag
                End If

#End Region

#Region "Commented out. old stuff"
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
#End Region

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

                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

                Me.Diagram.AllowUnconnectedLinks = False

                '------------------------------------------------------------------------------
                'Clear the 'InPlaceEdit' on principal.
                '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas,
                '  so disable the 'InPlaceEdit'.
                '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on,
                '  then 'InPlaceEdit' is temporarily allowed.
                '------------------------------------------------------------------------------
                Me.DiagramView.AllowInplaceEdit = False

                '-----------------------------------------------------------------------------------------------------------
                'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel
                '-----------------------------------------------------------------------------------------------------------
                If Not IsNothing(frmMain.zfrm_properties) Then
                    frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
                End If

                Call Me.reset_node_and_link_colors()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub reset_node_and_link_colors()

        Dim liInd As Integer = 0

        Try

            '------------------------------------------------------------------------------------
            'Reset the border colors of the ShapeNodes (to what they were before being selected
            '------------------------------------------------------------------------------------
            For liInd = 1 To Diagram.Nodes.Count

                'CodeSafe
                If Diagram.Nodes(liInd - 1).Tag Is Nothing Then GoTo SkipTag

                Select Case Diagram.Nodes(liInd - 1).Tag.GetType
                    Case Is = GetType(BPMN.Process)

                        Dim lrBPMNProcess As BPMN.Process = Diagram.Nodes(liInd - 1).Tag

                        Try

                            Select Case lrBPMNProcess.CMMLProcess.ProcessType
                                Case Is = pcenumBPMNProcessType.Gateway,
                                          pcenumBPMNProcessType.Event
                                    If Diagram.Nodes(liInd - 1).Selected Then
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                                    Else
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                                    End If
                                Case Else
                                    If Diagram.Nodes(liInd - 1).Selected Then
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                                    Else
                                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                                    End If
                            End Select
                        Catch
                            'not ideal, but we're not crashing.
                        End Try

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
                End Select
SkipTag:
            Next

            For liInd = 1 To Diagram.Links.Count
                Diagram.Links(liInd - 1).Pen.Color = Color.Black
            Next

            Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub ProcessPopupToolSelector(ByRef arPage As FBM.Page, ByRef asInstructionType As String)

        Dim lsMessage As String

        'CodeSafe 
        If arPage.SelectedObject.Count = 0 Then Exit Sub

        Try
            Select Case asInstructionType
#Region "Activities Change Element"
                Case Is = "ChangeElementActivityTaskTypeSendTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_Send
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True

                Case Is = "ChangeElementActivityTaskTypeReceiveTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_Receive
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True


                Case Is = "ChangeElementActivityTaskTypeManualTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_Manual
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True

                Case Is = "ChangeElementActivityTaskTypeBusinessRuleTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.BusinessRule_25x25
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True

                Case Is = "ChangeElementActivityTaskTypeServiceTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_Service
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True

                Case Is = "ChangeElementActivityTaskTypeScriptTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_Script
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True

                Case Is = "ChangeElementActivityTaskTypeUserTask"

                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.TaskTypeShape.Image = My.Resources.BPMNElementDecorations.TaskType_User
                    lrBPMNProcessSelected.TaskTypeShape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.TaskTypeShape.Visible = True
#End Region

#Region "Gateways Change Element"
                Case Is = "ChangeElementGatewayTypeParallel"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Gateway_Parallel
                    lrBPMNProcessSelected.Shape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.Shape.Visible = True

                Case Is = "ChangeElementGatewayTypeInclusive"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Gateway_Inclusive
                    lrBPMNProcessSelected.Shape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.Shape.Visible = True

                Case Is = "ChangeElementGatewayTypeComplex"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Gateway_Complex
                    lrBPMNProcessSelected.Shape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.Shape.Visible = True

                Case Is = "ChangeElementGatewayTypeEventBased"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Gateway_EventBased
                    lrBPMNProcessSelected.Shape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.Shape.Visible = True

                Case Is = "ChangeElementGatewayTypeExlusive"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Gateway_Exclusive
                    lrBPMNProcessSelected.Shape.ImageAlign = ImageAlign.Fit
                    lrBPMNProcessSelected.Shape.Visible = True
#End Region

#Region "Events Change Element"

                Case Is = "ChangeElementEventEndCancelStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Cancel_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Cancel)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)


                Case Is = "ChangeElementEventEndCompensationStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Compensation_Standard
                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Compensation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)


                Case Is = "ChangeElementEventEndErrorStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Error_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Error)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndEscalationStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Escalation_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndLinkStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Link_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Link)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndMessageStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Message_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndMultipleStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Multiple_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndNoneStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_None_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.None)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndSignalStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Signal_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventEndTerminateStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_End_Terminate_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.End)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Terminate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventIntermediateCancelBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Cancel_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Cancel)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateCompensationBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Compensation_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Compensation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateCompensationSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Compensation_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Compensation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventIntermediateCompensationThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Compensation_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Compensation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateConditionalBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Conditional_BoundaryNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Conditional)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateConditionalBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Conditional_BoundaryInterupting
                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Conditional)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateConditionalCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Conditional_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Conditional)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateErrorBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Error_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Error)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateErrorSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Error_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Error)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventIntermediateEscalationBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateEscalationBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryNonInterupting
                    '
                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateEscalationSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventIntermediateEscalationSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateEscalationThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Escalation_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Escalation)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateLinkCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Link_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Link)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateLinkThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Link_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Link)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateMessageBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Message_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateMessageBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Message_BoundaryNonInterupting___Copy

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateMessageCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Message_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateMessageSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Message_SubprocessNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateMessageThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Message_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateMultipleBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateMultipleBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateMultipleCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Multiple_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateMultipleSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Multiple_SubprocessNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateMultipleThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Multiple_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateNoneThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_None_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.None)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateParallelMultipleSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_SubprocessNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateParallelMultipleBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateParallelMultipleBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateParallelMultipleCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateSignalBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateSignalBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_BoundaryNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateSignalCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventIntermediateSignalSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_Subprocess_NonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateSignalSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventIntermediateSignalThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Signal_Throwing

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Throwing)

                Case Is = "ChangeElementEventIntermediateTimerBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateTimerBoundaryNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_BoundaryNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryNonInterupting)

                Case Is = "ChangeElementEventIntermediateTimerSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventIntermediateTimerSubprocessNonInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_SubprocessNonInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessNonInterupting)

                Case Is = "ChangeElementEventIntermediateTimerBoundaryInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_BoundaryInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.BoundaryInterupting)

                Case Is = "ChangeElementEventIntermediateTimerCatching"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Intermediate_Timer_Catching

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Intermediate)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Catching)

                Case Is = "ChangeElementEventStartConditionalStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Conditional_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Conditional)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartConditionalSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Conditional_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Conditional)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventStartMessageStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Message_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartMessageSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Message_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Message)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventStartMultipleStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Multiple_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartMultipleSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Multiple_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Multiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventStartNoneStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_None_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.None)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartParallelMultipleStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_ParallelMultiple_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartParallelMultipleSubprocessInterupting"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_ParallelMultiple_SubprocessInterupting

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.ParallelMultiple)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.SubprocessInterupting)

                Case Is = "ChangeElementEventStartSignalStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Signal_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Signal)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)

                Case Is = "ChangeElementEventStartTimerStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    lrBPMNProcessSelected.Shape.Image = My.Resources.BPMN.Event_Start_Timer_Standard

                    lrBPMNProcessSelected.Shape.Visible = True

                    lrBPMNProcessSelected.CMMLProcess.SetEventPosition(pcenumBPMNEventPosition.Start)
                    lrBPMNProcessSelected.CMMLProcess.SetEventType(pcenumBPMNEventType.Timer)
                    lrBPMNProcessSelected.CMMLProcess.SetEventSubType(pcenumBPMNEventSubType.Standard)
#End Region
                Case Is = "Bin"
#Region "Bin"
                    Dim lrBPMNProcessSelected As BPMN.Process = Nothing

                    lrBPMNProcessSelected = arPage.SelectedObject(0)

                    lsMessage = "Are you sure you want to delete this model element from the Page and the Model?"
                    lsMessage.AppendDoubleLineBreak("If you only want to remove he model element from the Page (not the Model) then right-click on the element and select [Remove from Page].")
                    lsMessage.AppendDoubleLineBreak("NB If you remove the model element from the Model all of its associated relationships/flows will be removed from the Model as well.")

                    If MsgBox(lsMessage, MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

#Region "Connected Relations - ActorProcess/ProcessProcess"
                        Dim larConnectedProcessesProcessRelation = From ProcessProcessRelation In lrBPMNProcessSelected.CMMLProcess.CMMLModel.ProcessProcessRelation.ToList
                                                                   Where (ProcessProcessRelation.Process1.Id = lrBPMNProcessSelected.Id) Or (ProcessProcessRelation.Process2.Id = lrBPMNProcessSelected.Id)
                                                                   Select ProcessProcessRelation

                        For Each lrCMMLConnectedProcessProcessRelation In larConnectedProcessesProcessRelation
                            Call lrCMMLConnectedProcessProcessRelation.RemoveFromModel()
                        Next

                        Dim larConnectedActorProcessRelation = From ActorProcessRelation In lrBPMNProcessSelected.CMMLProcess.CMMLModel.ActorProcessRelation.ToList
                                                               Where ActorProcessRelation.Process.Id = lrBPMNProcessSelected.Id
                                                               Select ActorProcessRelation

                        For Each lrCMMLConnectedActorProcessRelation In larConnectedActorProcessRelation
                            Call lrCMMLConnectedActorProcessRelation.RemoveFromModel()
                        Next
#End Region

                        Call lrBPMNProcessSelected.CMMLProcess.RemoveFromModel()
                    End If
#End Region
                Case Is = "EventEndNoneStandard"
#Region "EventEndNoneStandard"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    Dim loPointF As New PointF(lrBPMNProcessSelected.Shape.Bounds.X + 30, lrBPMNProcessSelected.Shape.Bounds.Y)

                    Dim lrCMMLProcess As CMML.Process = New CMML.Process(arPage.Model.UML, System.Guid.NewGuid.ToString, "New BPMN Activity")
                    lrCMMLProcess.Text = arPage.Model.UML.CreateUniqueProcessText(lrCMMLProcess)
                    lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Event
                    lrCMMLProcess.EventType = pcenumBPMNEventType.None
                    lrCMMLProcess.EventSubType = pcenumBPMNEventSubType.Standard
                    lrCMMLProcess.EventPosition = pcenumBPMNEventPosition.End

                    arPage.Model.UML.addProcess(lrCMMLProcess)
                    Dim lrBPMNProcess As BPMN.Process = arPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                    lrBPMNProcess.CMMLProcess = lrCMMLProcess

#Region "Flow association - Process-to-Process"
                    Dim lrCMMLProcess1 As CMML.Process = lrBPMNProcessSelected.CMMLProcess
                    Dim lrCMMLProcess2 As CMML.Process = lrCMMLProcess

                    Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(arPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                    arPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)
#End Region

#End Region

                Case Is = "GatewayExclusive"
#Region "EventIntermediateNoneThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    Dim loPointF As New PointF(lrBPMNProcessSelected.Shape.Bounds.X + 30, lrBPMNProcessSelected.Shape.Bounds.Y)

                    Dim lrCMMLProcess As CMML.Process = New CMML.Process(arPage.Model.UML, System.Guid.NewGuid.ToString, "")
                    lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Gateway
                    lrCMMLProcess.GatewayType = pcenumBPMNGatewayType.ExclusiveGateway

                    arPage.Model.UML.addProcess(lrCMMLProcess)
                    Dim lrBPMNProcess As BPMN.Process = arPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                    lrBPMNProcess.CMMLProcess = lrCMMLProcess

#Region "Flow association - Process-to-Process"
                    Dim lrCMMLProcess1 As CMML.Process = lrBPMNProcessSelected.CMMLProcess
                    Dim lrCMMLProcess2 As CMML.Process = lrCMMLProcess

                    Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(arPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                    arPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)
#End Region

#End Region
                Case Is = "Task"
#Region "Activity"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    Dim loPointF As New PointF(lrBPMNProcessSelected.Shape.Bounds.X + 30, lrBPMNProcessSelected.Shape.Bounds.Y)

                    Dim lrCMMLProcess As CMML.Process = New CMML.Process(arPage.Model.UML, System.Guid.NewGuid.ToString, "New BPMN Activity")
                    lrCMMLProcess.Text = arPage.Model.UML.CreateUniqueProcessText(lrCMMLProcess)

                    arPage.Model.UML.addProcess(lrCMMLProcess)
                    Dim lrBPMNProcess As BPMN.Process = arPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                    lrBPMNProcess.CMMLProcess = lrCMMLProcess

#Region "Flow association - Process-to-Process"
                    Dim lrCMMLProcess1 As CMML.Process = lrBPMNProcessSelected.CMMLProcess
                    Dim lrCMMLProcess2 As CMML.Process = lrCMMLProcess

                    Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(arPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                    arPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)
#End Region
#End Region
                Case Is = "EventIntermediateNoneThrowing"
#Region "EventIntermediateNoneThrowing"
                    Dim lrBPMNProcessSelected As BPMN.Process = arPage.SelectedObject(0)
                    Dim loPointF As New PointF(lrBPMNProcessSelected.Shape.Bounds.X + 30, lrBPMNProcessSelected.Shape.Bounds.Y)

                    Dim lrCMMLProcess As CMML.Process = New CMML.Process(arPage.Model.UML, System.Guid.NewGuid.ToString, "New BPMN Activity")
                    lrCMMLProcess.Text = arPage.Model.UML.CreateUniqueProcessText(lrCMMLProcess)
                    lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Event
                    lrCMMLProcess.EventType = pcenumBPMNEventType.None
                    lrCMMLProcess.EventSubType = pcenumBPMNEventSubType.Throwing
                    lrCMMLProcess.EventPosition = pcenumBPMNEventPosition.Intermediate

                    arPage.Model.UML.addProcess(lrCMMLProcess)
                    Dim lrBPMNProcess As BPMN.Process = arPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                    lrBPMNProcess.CMMLProcess = lrCMMLProcess

#Region "Flow association - Process-to-Process"
                    Dim lrCMMLProcess1 As CMML.Process = lrBPMNProcessSelected.CMMLProcess
                    Dim lrCMMLProcess2 As CMML.Process = lrCMMLProcess

                    Dim lrCMMLProcessProcessRelation = New CMML.ProcessProcessRelation(arPage.Model.UML, lrCMMLProcess1, lrCMMLProcess2)

                    arPage.Model.UML.addProcessProcessRelation(lrCMMLProcessProcessRelation)
#End Region

#End Region

                Case Is = "Annotation"
                Case Is = "More"
                Case Is = "ChangeType"
                Case Is = "Bin"
                Case Is = "Connector"
                    Dim lrBPMNProcessSelected As BPMN.Process = Nothing
                    If arPage.SelectedObject.Count = 1 Then
                        lrBPMNProcessSelected = arPage.SelectedObject(0)
                        lrBPMNProcessSelected.IsCreatingLink = True
                    End If
            End Select

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ETDDiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loObject As Object
        Dim loNode As DiagramNode

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

        Dim larPage_list As New List(Of FBM.Page)
        Dim lr_model As FBM.Model
        Dim lr_process As New UML.Process

        If prApplication.WorkingPage.SelectedObject.Count = 0 Then
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
        Me.ToolStripMenuItemUseCaseDiagramProcess.DropDownItems.Clear()

        '=========================================================
        'Load the UseCaseDiagrams that relate to the EntityType
        '  as selectable menuOptions
        '=========================================================
        '============================================================================================================
        '20220528-VM-CommentOut-For first iteration of BPMN in Boston.
        ''--------------------------------------------------------------------------------------------------------
        ''The EntityType represents an Actor. i.e. Has a ParentEntityType of 'Actor' within the Core meta-schema
        ''--------------------------------------------------------------------------------------------------------
        'larPage_list = prApplication.CMML.get_use_case_diagram_pages_for_process(lr_process)


        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '---------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '---------------------------------------------------
        '    lo_menu_option = Me.MenuOptionUseCaseDiagramProcess.DropDownItems.Add(lr_page.Name)
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

        ''=================================================================
        ''Load the DFDDiagrams that relate to the EntityType as selectable menuOptions
        ''Clear the list of DFDDiagrams that may relate to the EntityType
        ''=================================================================
        'Me.MenuOptionDataFlowDiagramProcess.DropDownItems.Clear()

        ''--------------------------------------
        ''The EntityType represents a Process.
        ''--------------------------------------
        'larPage_list = prApplication.CMML.get_DataFlowDiagram_pages_for_process(lr_process)

        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '----------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '----------------------------------------------------
        '    lo_menu_option = Me.MenuOptionDataFlowDiagramProcess.DropDownItems.Add(lr_page.Name)
        '    Dim lr_enterprise_view As tEnterpriseEnterpriseView
        '    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageDataFlowDiagram,
        '                                               lr_page,
        '                                               lr_page.Model.EnterpriseId,
        '                                               lr_page.Model.SubjectAreaId,
        '                                               lr_page.Model.ProjectId,
        '                                               lr_page.Model.SolutionId,
        '                                               lr_page.Model.ModelId,
        '                                               pcenumLanguage.DataFlowDiagram,
        '                                               Nothing, lr_page.PageId)
        '    lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '    AddHandler lo_menu_option.Click, AddressOf Me.morph_to_DataFlowDiagram
        'Next

        ''==============================================================================
        ''Load the Flow Charts that relate to the EntityType as selectable menuOptions
        ''==============================================================================
        'Me.ToolStripMenuItemFlowChart.DropDownItems.Clear()

        'larPage_list = prApplication.CMML.GetFlowChartDiagramPagesForProcess(lr_process)

        'For Each lr_page In larPage_list
        '    Dim lo_menu_option As ToolStripItem
        '    '---------------------------------------------------
        '    'Add the Page(Name) to the MenuOption.DropDownItems
        '    '---------------------------------------------------
        '    lo_menu_option = Me.ToolStripMenuItemFlowChart.DropDownItems.Add(lr_page.Name)
        '    Dim lr_enterprise_view As tEnterpriseEnterpriseView
        '    lr_enterprise_view = New tEnterpriseEnterpriseView(pcenumMenuType.pageFlowChart,
        '                                               lr_page,
        '                                               lr_page.Model.EnterpriseId,
        '                                               lr_page.Model.SubjectAreaId,
        '                                               lr_page.Model.ProjectId,
        '                                               lr_page.Model.SolutionId,
        '                                               lr_page.Model.ModelId,
        '                                               pcenumLanguage.FlowChart,
        '                                               Nothing, lr_page.PageId)
        '    lo_menu_option.Tag = prPageNodes.Find(AddressOf lr_enterprise_view.Equals)
        '    AddHandler lo_menu_option.Click, AddressOf Me.MorphToFlowChartDiagram
        'Next
        'UnCommentOut-ToHere
        '============================================================================================================

    End Sub

    Private Sub PageAsORMMetaModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetaModelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Call Me.CopyImageToClipboard()

    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call Me.SetToolbox()

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

    Private Sub Diagram_LinkSelected(sender As Object, e As LinkEventArgs) Handles Diagram.LinkSelected

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub Diagram_NodeDeselected(sender As Object, e As NodeEventArgs) Handles Diagram.NodeDeselected


        Try
            If IsSomething(e.Node) Then

                'CodeSafe
                If e.Node.Tag Is Nothing Then Exit Sub

                Select Case e.Node.Tag.ConceptType
                    Case Is = pcenumConceptType.Actor
                        Dim lrActor As BPMN.Actor = e.Node.Tag
                        Call lrActor.NodeDeselected()
                    Case Is = pcenumConceptType.Process
                        Dim lrBPMNProcess As BPMN.Process = e.Node.Tag
                        lrBPMNProcess.IsCreatingLink = False
                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Diagram_NodeDoubleClicked(sender As Object, e As NodeEventArgs) Handles Diagram.NodeDoubleClicked

        Try
            Select Case e.Node.Tag.GetType
                Case Is = GetType(BPMN.Actor)

                    Dim lrActor As BPMN.Actor = e.Node.Tag
                    Call Me.DiagramView.BeginEdit(lrActor.NameShape.Shape)
                Case Is = GetType(BPMN.Process)

                    Dim lrProcess As BPMN.Process = e.Node.Tag
                    Call Me.DiagramView.BeginEdit(lrProcess.Shape)
            End Select


            Me.DiagramView.Behavior = Behavior.DrawLinks
            Me.Diagram.Invalidate()
            Me.Diagram.Selection.Clear()
            Me.Cursor = Cursors.Hand
            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Diagram_NodeModified(sender As Object, e As NodeEventArgs) Handles Diagram.NodeModified

        Dim lrShape As ShapeNode
        Me.zrPage.MakeDirty()
        frmMain.ToolStripButton_Save.Enabled = True

        Try
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
                    Case Is = pcenumConceptType.Process
                        Dim lrBPMNProcess As BPMN.Process = e.Node.Tag

                        Call lrBPMNProcess.Move(e.Node.Bounds.X, e.Node.Bounds.Y, True)

#Region "Snap to Grid"
                        Me.zrPage.Diagram.AlignToGrid = True
                        Dim ptCenter As PointF = lrBPMNProcess.Shape.GetCenter()
                        Dim ptAlignedCenter As PointF = Me.zrPage.Diagram.AlignPointToGrid(ptCenter)
                        Dim fDeltaX As Double = ptAlignedCenter.X - ptCenter.X
                        Dim fDeltaY As Double = ptAlignedCenter.Y - ptCenter.Y
                        Me.zrPage.Diagram.AlignToGrid = False

                        Dim rectBounds As RectangleF = lrBPMNProcess.Shape.Bounds
                        rectBounds.Offset(fDeltaX, fDeltaY)
                        lrBPMNProcess.Shape.Bounds = rectBounds
#End Region

                End Select

                ArrangeLinks()

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub Diagram_NodeTextEdited(sender As Object, e As EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        Dim lsMessage As String

        Try
            Dim lrProcess As BPMN.Process

            'CodeSafe
            If Trim(e.NewText) = "" Then Exit Sub

            Select Case e.Node.Tag.GetType
                Case Is = GetType(UML.ActorName)

                    Dim lrBPMNActor As BPMN.Actor = e.Node.Tag.Actor

                    Dim lrModelElement = Me.zrPage.Model.GetModelObjectByName(e.NewText, True)
                    Dim lbModelElementExists As Boolean = False
                    If lrModelElement IsNot Nothing Then
                        lbModelElementExists = lrBPMNActor.CMMLActor.FBMModelElement IsNot lrModelElement
                    End If
                    If lbModelElementExists Or (Me.zrPage.Model.UML.Actor.Find(Function(x) x IsNot lrBPMNActor.CMMLActor And LCase(Trim(x.Name)) = LCase(Trim(e.NewText))) IsNot Nothing) Then
                        lsMessage = "You cannot set the name of a Actor as the same as the name of another Actor in the model."
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "A Actor with the name, '" & e.NewText & "', already exists in the model."

                        lrBPMNActor.NameShape.Shape.Text = e.OldText
                        MsgBox(lsMessage)
                        Exit Sub
                    End If

                    Call lrBPMNActor.CMMLActor.setName(e.NewText)

                Case Is = GetType(BPMN.Process)

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RemoveFromPageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageToolStripMenuItem.Click

        Dim lsSQLQuery As String = ""

        Try
            With New WaitCursor

                ''---------------------------------------------------------
                ''Get the Process represented by the (selected) Process
                ''---------------------------------------------------------
                Dim lrProcess As BPMN.Process = Me.Diagram.Selection.Items(0).Tag

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RemoveFromPageModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageModelToolStripMenuItem.Click

        Try
            If Me.mrPopupToolSelector IsNot Nothing Then
                Me.Diagram.Nodes.Remove(Me.mrPopupToolSelector.Node)
                Me.mrPopupToolSelector = Nothing
            End If
            If Me.mrPopupToolElementChanger IsNot Nothing Then
                Me.Diagram.Nodes.Remove(Me.mrPopupToolElementChanger.Node)
                Me.mrPopupToolElementChanger = Nothing
            End If

            With New WaitCursor
                '---------------------------------------------------------
                'Get the Process represented by the (selected) Process
                '---------------------------------------------------------
                Dim lrProcess As BPMN.Process = Me.zrPage.SelectedObject(0)

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Private Sub ModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call prApplication.setWorkingModel(Me.zrPage.Model)

        Call frmMain.LoadToolboxModelDictionary(True)

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PropertiesToolStripMenuItem.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)
        Dim lrPropertyGridForm As frmToolboxProperties = prApplication.GetToolboxForm(frmToolboxProperties.Name)

        If lrPropertyGridForm IsNot Nothing Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.UMLDiagram
            End If
        End If


    End Sub

    Private Sub mnuOption_ViewGrid_Click(sender As Object, e As EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

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
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RemoveFromPageAndModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveFromPageAndModelToolStripMenuItem.Click

        Try
            Dim loProcessRelation = Me.zrPage.SelectedObject(0)

            If loProcessRelation Is Nothing Then Exit Sub

            If MsgBox("Are you sure you want to remove the Relation from the Model?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                Select Case loProcessRelation.GetType
                    Case Is = GetType(BPMN.ActorProcessRelation)

                        Dim lrActorProcessRelation As BPMN.ActorProcessRelation = loProcessRelation
                        Call lrActorProcessRelation.CMMLActorProcessRelation.RemoveFromModel()

                    Case Is = GetType(BPMN.ProcessProcessRelation)

                        Dim lrProcessProcessRelation As BPMN.ProcessProcessRelation = loProcessRelation
                        Call lrProcessProcessRelation.CMMLProcessProcessRelation.RemoveFromModel()

                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DiagramView_DragDrop(sender As Object, e As DragEventArgs) Handles DiagramView.DragDrop

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
                        Dim lrUCDProcess As UCD.Process = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
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
                        Case Is = "Process", "Task"
#Region "Task/Process"
                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New Process", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess
#End Region
                        Case Is = "Start Event"
#Region "Start Event"
                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New Process", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Event
                            lrCMMLProcess.IsStart = True
                            lrCMMLProcess.EventPosition = pcenumBPMNEventPosition.Start
                            lrCMMLProcess.EventType = pcenumBPMNEventType.None
                            lrCMMLProcess.EventSubType = pcenumBPMNEventSubType.Standard
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess
#End Region
                        Case Is = "End Event"
#Region "End Event"
                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New Process", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Event
                            lrCMMLProcess.IsStop = True
                            lrCMMLProcess.EventPosition = pcenumBPMNEventPosition.End
                            lrCMMLProcess.EventType = pcenumBPMNEventType.None
                            lrCMMLProcess.EventSubType = pcenumBPMNEventSubType.Standard
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess
#End Region
                        Case Is = "Intermediate/Boundary Event"
#Region "Intermediate/Boundary Event"
                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New Process", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Event
                            lrCMMLProcess.EventPosition = pcenumBPMNEventPosition.Intermediate
                            lrCMMLProcess.EventType = pcenumBPMNEventType.None
                            lrCMMLProcess.EventSubType = pcenumBPMNEventSubType.Throwing
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess
#End Region
                        Case Is = "Gateway"
#Region "Gateway"
                            'CMML Model level
                            Dim lsUniqueProcessText = Me.zrPage.Model.UML.CreateUniqueProcessText("New Process", 0)
                            Dim lrCMMLProcess As New CMML.Process(Me.zrPage.Model.UML, System.Guid.NewGuid.ToString, lsUniqueProcessText)
                            lrCMMLProcess.ProcessType = pcenumBPMNProcessType.Gateway
                            lrCMMLProcess.GatewayType = pcenumBPMNGatewayType.ExclusiveGateway
                            Call Me.zrPage.Model.UML.addProcess(lrCMMLProcess)

                            'Page Level
                            Dim lrUCDProcess As UCD.Process
                            lrUCDProcess = Me.zrPage.DropCMMLProcessAtPoint(lrCMMLProcess, loPointF, Nothing,, pcenumLanguage.BPMNCollaborationDiagram)
                            lrUCDProcess.CMMLProcess = lrCMMLProcess
#End Region
                    End Select
                End If
#End Region

            End If
        End If

    End Sub

    Private Sub DiagramView_DragOver(sender As Object, e As DragEventArgs) Handles DiagramView.DragOver

        Try

            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
                e.Effect = DragDropEffects.Copy
            Else
                e.Effect = DragDropEffects.None
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Diagram_ExpandButtonClicked(sender As Object, e As NodeEventArgs) Handles Diagram.ExpandButtonClicked

    End Sub

    Private Sub DiagramView_MouseMove(sender As Object, e As MouseEventArgs) Handles DiagramView.MouseMove

        Try
            Me.Diagram.Invalidate()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class