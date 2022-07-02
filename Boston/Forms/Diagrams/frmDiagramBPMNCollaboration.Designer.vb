<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramBPMNCollaboration
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PageAsORMMetaModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TimerLinkSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Process = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemUseCaseDiagramProcess = New System.Windows.Forms.ToolStripMenuItem()
        Me.DFDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemStateTransitionDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_ProcessLink = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFromPageToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageAndModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_Process.SuspendLayout()
        Me.ContextMenuStrip_ProcessLink.SuspendLayout()
        Me.SuspendLayout()
        '
        'DiagramView
        '
        Me.DiagramView.AllowDrop = True
        Me.DiagramView.AllowInplaceEdit = True
        Me.DiagramView.BackColor = System.Drawing.SystemColors.Control
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.DrawLinks
        Me.DiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.DiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.DiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.DiagramView.Diagram = Me.Diagram
        Me.DiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiagramView.InplaceEditAcceptOnEnter = True
        Me.DiagramView.Location = New System.Drawing.Point(0, 0)
        Me.DiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.DiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.DiagramView.Name = "DiagramView"
        Me.DiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.DiagramView.ShowScrollbars = False
        Me.DiagramView.Size = New System.Drawing.Size(728, 498)
        Me.DiagramView.TabIndex = 0
        Me.DiagramView.Text = "DiagramView"
        '
        'Diagram
        '
        Me.Diagram.AllowSelfLoops = False
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.DynamicLinks = True
        Me.Diagram.EnableLanes = True
        Me.Diagram.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Diagram.LinkBrush = New MindFusion.Drawing.SolidBrush("#FF000000")
        Me.Diagram.LinkCascadeOrientation = MindFusion.Diagramming.Orientation.Vertical
        Me.Diagram.LinkCrossings = MindFusion.Diagramming.LinkCrossings.Arcs
        Me.Diagram.LinkHeadShape = MindFusion.Diagramming.ArrowHead.Triangle
        Me.Diagram.LinkHeadShapeSize = 3.0!
        Me.Diagram.LinkSegments = CType(2, Short)
        Me.Diagram.LinksRetainForm = True
        Me.Diagram.LinkStyle = MindFusion.Diagramming.LinkStyle.Cascading
        Me.Diagram.RouteLinks = True
        Me.Diagram.ShadowColor = System.Drawing.Color.White
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.ShapePen = New MindFusion.Drawing.Pen("0/#FF000000/1/0/0//0/0/10/")
        Me.Diagram.ShowHandlesOnDrag = False
        '
        'HiddenDiagramView
        '
        Me.HiddenDiagramView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.HiddenDiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.HiddenDiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.HiddenDiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.HiddenDiagramView.Diagram = Me.HiddenDiagram
        Me.HiddenDiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HiddenDiagramView.Location = New System.Drawing.Point(0, 0)
        Me.HiddenDiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.HiddenDiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.HiddenDiagramView.Name = "HiddenDiagramView"
        Me.HiddenDiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.HiddenDiagramView.Size = New System.Drawing.Size(728, 498)
        Me.HiddenDiagramView.TabIndex = 3
        Me.HiddenDiagramView.Text = "HiddenDiagramView"
        '
        'HiddenDiagram
        '
        Me.HiddenDiagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.HiddenDiagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FF99B4D1")
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.CopyToolStripMenuItem, Me.ToolStripSeparator1})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 76)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolboxToolStripMenuItem, Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator2, Me.PageAsORMMetaModelToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ToolboxToolStripMenuItem
        '
        Me.ToolboxToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Toolbox16x16B_W
        Me.ToolboxToolStripMenuItem.Name = "ToolboxToolStripMenuItem"
        Me.ToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ToolboxToolStripMenuItem.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(205, 6)
        '
        'PageAsORMMetaModelToolStripMenuItem
        '
        Me.PageAsORMMetaModelToolStripMenuItem.Name = "PageAsORMMetaModelToolStripMenuItem"
        Me.PageAsORMMetaModelToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PageAsORMMetaModelToolStripMenuItem.Text = "Page as &ORM MetaModel"
        '
        'ShowHideToolStripMenuItem
        '
        Me.ShowHideToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_ViewGrid})
        Me.ShowHideToolStripMenuItem.Name = "ShowHideToolStripMenuItem"
        Me.ShowHideToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ShowHideToolStripMenuItem.Text = "&Show/Hide"
        '
        'mnuOption_ViewGrid
        '
        Me.mnuOption_ViewGrid.Image = Global.Boston.My.Resources.MenuImages.Grid
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(96, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Camera16x16
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(204, 6)
        '
        'MorphTimer
        '
        Me.MorphTimer.Interval = 1500
        '
        'MorphStepTimer
        '
        Me.MorphStepTimer.Interval = 25
        '
        'TimerLinkSwitch
        '
        Me.TimerLinkSwitch.Interval = 1000
        '
        'ContextMenuStrip_Process
        '
        Me.ContextMenuStrip_Process.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripSeparator3, Me.RemoveFromPageToolStripMenuItem, Me.RemoveFromPageModelToolStripMenuItem, Me.ToolStripSeparator6, Me.PropertiesToolStripMenuItem2})
        Me.ContextMenuStrip_Process.Name = "ContextMenuStrip_Process"
        Me.ContextMenuStrip_Process.Size = New System.Drawing.Size(236, 104)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemUseCaseDiagramProcess, Me.DFDToolStripMenuItem, Me.ToolStripMenuItemStateTransitionDiagram})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(235, 22)
        Me.ToolStripMenuItem1.Text = "&Morph to..."
        '
        'ToolStripMenuItemUseCaseDiagramProcess
        '
        Me.ToolStripMenuItemUseCaseDiagramProcess.Image = Global.Boston.My.Resources.Resources.UML_UseCase16x16
        Me.ToolStripMenuItemUseCaseDiagramProcess.Name = "ToolStripMenuItemUseCaseDiagramProcess"
        Me.ToolStripMenuItemUseCaseDiagramProcess.Size = New System.Drawing.Size(202, 22)
        Me.ToolStripMenuItemUseCaseDiagramProcess.Text = "&Use Case Diagram"
        '
        'DFDToolStripMenuItem
        '
        Me.DFDToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.DataFlowDiagram16x16
        Me.DFDToolStripMenuItem.Name = "DFDToolStripMenuItem"
        Me.DFDToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.DFDToolStripMenuItem.Text = "&Data Flow Diagram"
        '
        'ToolStripMenuItemStateTransitionDiagram
        '
        Me.ToolStripMenuItemStateTransitionDiagram.Image = Global.Boston.My.Resources.Resources.StateTransitionDiagram_16x16
        Me.ToolStripMenuItemStateTransitionDiagram.Name = "ToolStripMenuItemStateTransitionDiagram"
        Me.ToolStripMenuItemStateTransitionDiagram.Size = New System.Drawing.Size(202, 22)
        Me.ToolStripMenuItemStateTransitionDiagram.Text = "&State Transition Diagram"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(232, 6)
        '
        'RemoveFromPageToolStripMenuItem
        '
        Me.RemoveFromPageToolStripMenuItem.Name = "RemoveFromPageToolStripMenuItem"
        Me.RemoveFromPageToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageToolStripMenuItem.Text = "Remove from &Page"
        '
        'RemoveFromPageModelToolStripMenuItem
        '
        Me.RemoveFromPageModelToolStripMenuItem.Name = "RemoveFromPageModelToolStripMenuItem"
        Me.RemoveFromPageModelToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageModelToolStripMenuItem.Text = "Remove from Page and &Model"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(232, 6)
        '
        'PropertiesToolStripMenuItem2
        '
        Me.PropertiesToolStripMenuItem2.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem2.Name = "PropertiesToolStripMenuItem2"
        Me.PropertiesToolStripMenuItem2.Size = New System.Drawing.Size(235, 22)
        Me.PropertiesToolStripMenuItem2.Text = "&Properties"
        '
        'ContextMenuStrip_ProcessLink
        '
        Me.ContextMenuStrip_ProcessLink.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFromPageToolStripMenuItem1, Me.RemoveFromPageAndModelToolStripMenuItem, Me.ToolStripSeparator7, Me.PropertiesToolStripMenuItem3})
        Me.ContextMenuStrip_ProcessLink.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_ProcessLink.Size = New System.Drawing.Size(236, 76)
        '
        'RemoveFromPageToolStripMenuItem1
        '
        Me.RemoveFromPageToolStripMenuItem1.Name = "RemoveFromPageToolStripMenuItem1"
        Me.RemoveFromPageToolStripMenuItem1.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageToolStripMenuItem1.Text = "Remove from &Page"
        '
        'RemoveFromPageAndModelToolStripMenuItem
        '
        Me.RemoveFromPageAndModelToolStripMenuItem.Name = "RemoveFromPageAndModelToolStripMenuItem"
        Me.RemoveFromPageAndModelToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageAndModelToolStripMenuItem.Text = "Remove from Page and &Model"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(232, 6)
        '
        'PropertiesToolStripMenuItem3
        '
        Me.PropertiesToolStripMenuItem3.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem3.Name = "PropertiesToolStripMenuItem3"
        Me.PropertiesToolStripMenuItem3.Size = New System.Drawing.Size(235, 22)
        Me.PropertiesToolStripMenuItem3.Text = "&Properties"
        '
        'frmDiagramBPMNCollaboration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 498)
        Me.Controls.Add(Me.DiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmDiagramBPMNCollaboration"
        Me.TabText = "Event Trace Diagram"
        Me.Text = "Event Trace Diagram"
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_Process.ResumeLayout(False)
        Me.ContextMenuStrip_ProcessLink.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents TimerLinkSwitch As System.Windows.Forms.Timer
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageAsORMMetaModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Process As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemUseCaseDiagramProcess As ToolStripMenuItem
    Friend WithEvents DFDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemStateTransitionDiagram As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents RemoveFromPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveFromPageModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ProcessLink As ContextMenuStrip
    Friend WithEvents RemoveFromPageToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents RemoveFromPageAndModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem3 As ToolStripMenuItem
End Class
