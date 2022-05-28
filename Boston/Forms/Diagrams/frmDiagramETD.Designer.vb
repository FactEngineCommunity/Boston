<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramETD
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
        Me.components = New System.ComponentModel.Container
        Me.ETDDiagramView = New MindFusion.Diagramming.WinForms.DiagramView
        Me.Diagram = New MindFusion.Diagramming.Diagram
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.PageAsORMMetaModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ContextMenuStrip_Actor = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuOptionUseCaseDiagramActor = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuOptionORMDiagramActor = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuOptionDataFlowDiagramActor = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip_Process = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOption_EntityTypeMorphTo = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuOptionUseCaseDiagramProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuOptionDataFlowDiagramProcess = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItemFlowChart = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuOption_EntityTypeProperties = New System.Windows.Forms.ToolStripMenuItem
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TimerLinkSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_Actor.SuspendLayout()
        Me.ContextMenuStrip_Process.SuspendLayout()
        Me.SuspendLayout()
        '
        'ETDDiagramView
        '
        Me.ETDDiagramView.AllowDrop = True
        Me.ETDDiagramView.AllowInplaceEdit = True
        Me.ETDDiagramView.BackColor = System.Drawing.SystemColors.Control
        Me.ETDDiagramView.Behavior = MindFusion.Diagramming.Behavior.DrawLinks
        Me.ETDDiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.ETDDiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.ETDDiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.ETDDiagramView.Diagram = Me.Diagram
        Me.ETDDiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ETDDiagramView.Location = New System.Drawing.Point(0, 0)
        Me.ETDDiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.ETDDiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.ETDDiagramView.Name = "ETDDiagramView"
        Me.ETDDiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.ETDDiagramView.ShowScrollbars = False
        Me.ETDDiagramView.Size = New System.Drawing.Size(728, 498)
        Me.ETDDiagramView.TabIndex = 0
        Me.ETDDiagramView.Text = "DiagramView1"
        '
        'Diagram
        '
        Me.Diagram.AllowSelfLoops = False
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.EnableLanes = True
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
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
        Me.ToolboxToolStripMenuItem.Image = Global.Richmond.My.Resources.Resources.Toolbox16x16BW
        Me.ToolboxToolStripMenuItem.Name = "ToolboxToolStripMenuItem"
        Me.ToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ToolboxToolStripMenuItem.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Richmond.My.Resources.Resources.dictionary16x161
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Richmond.My.Resources.Resources.Properties216x16
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
        Me.mnuOption_ViewGrid.Image = Global.Richmond.My.Resources.Resources.Grid
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(96, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.Richmond.My.Resources.Resources.Camera16x16
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(204, 6)
        '
        'ContextMenuStrip_Actor
        '
        Me.ContextMenuStrip_Actor.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator4, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_Actor.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_Actor.Size = New System.Drawing.Size(128, 54)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOptionUseCaseDiagramActor, Me.MenuOptionORMDiagramActor, Me.MenuOptionDataFlowDiagramActor})
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To"
        '
        'MenuOptionUseCaseDiagramActor
        '
        Me.MenuOptionUseCaseDiagramActor.Name = "MenuOptionUseCaseDiagramActor"
        Me.MenuOptionUseCaseDiagramActor.Size = New System.Drawing.Size(174, 22)
        Me.MenuOptionUseCaseDiagramActor.Text = "&Use Case Diagram"
        '
        'MenuOptionORMDiagramActor
        '
        Me.MenuOptionORMDiagramActor.Name = "MenuOptionORMDiagramActor"
        Me.MenuOptionORMDiagramActor.Size = New System.Drawing.Size(174, 22)
        Me.MenuOptionORMDiagramActor.Text = "&ORM Diagram"
        '
        'MenuOptionDataFlowDiagramActor
        '
        Me.MenuOptionDataFlowDiagramActor.Name = "MenuOptionDataFlowDiagramActor"
        Me.MenuOptionDataFlowDiagramActor.Size = New System.Drawing.Size(174, 22)
        Me.MenuOptionDataFlowDiagramActor.Text = "&Data Flow Diagram"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(124, 6)
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(127, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'ContextMenuStrip_Process
        '
        Me.ContextMenuStrip_Process.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_EntityTypeMorphTo, Me.ToolStripSeparator5, Me.mnuOption_EntityTypeProperties})
        Me.ContextMenuStrip_Process.Name = "ContextMenuStrip_EntityType"
        Me.ContextMenuStrip_Process.Size = New System.Drawing.Size(134, 54)
        '
        'mnuOption_EntityTypeMorphTo
        '
        Me.mnuOption_EntityTypeMorphTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuOptionUseCaseDiagramProcess, Me.MenuOptionDataFlowDiagramProcess, Me.ToolStripMenuItemFlowChart})
        Me.mnuOption_EntityTypeMorphTo.Name = "mnuOption_EntityTypeMorphTo"
        Me.mnuOption_EntityTypeMorphTo.Size = New System.Drawing.Size(133, 22)
        Me.mnuOption_EntityTypeMorphTo.Text = "&Morph to..."
        '
        'MenuOptionUseCaseDiagramProcess
        '
        Me.MenuOptionUseCaseDiagramProcess.Name = "MenuOptionUseCaseDiagramProcess"
        Me.MenuOptionUseCaseDiagramProcess.Size = New System.Drawing.Size(174, 22)
        Me.MenuOptionUseCaseDiagramProcess.Text = "&Use Case Diagram"
        '
        'MenuOptionDataFlowDiagramProcess
        '
        Me.MenuOptionDataFlowDiagramProcess.Name = "MenuOptionDataFlowDiagramProcess"
        Me.MenuOptionDataFlowDiagramProcess.Size = New System.Drawing.Size(174, 22)
        Me.MenuOptionDataFlowDiagramProcess.Text = "&Data Flow Diagram"
        '
        'ToolStripMenuItemFlowChart
        '
        Me.ToolStripMenuItemFlowChart.Name = "ToolStripMenuItemFlowChart"
        Me.ToolStripMenuItemFlowChart.Size = New System.Drawing.Size(174, 22)
        Me.ToolStripMenuItemFlowChart.Text = "&Flow Chart"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(130, 6)
        '
        'mnuOption_EntityTypeProperties
        '
        Me.mnuOption_EntityTypeProperties.Name = "mnuOption_EntityTypeProperties"
        Me.mnuOption_EntityTypeProperties.Size = New System.Drawing.Size(133, 22)
        Me.mnuOption_EntityTypeProperties.Text = "&Properties"
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
        'frmDiagramETD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 498)
        Me.Controls.Add(Me.ETDDiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmDiagramETD"
        Me.TabText = "Event Trace Diagram"
        Me.Text = "Event Trace Diagram"
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_Actor.ResumeLayout(False)
        Me.ContextMenuStrip_Process.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ETDDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStrip_Actor As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOptionUseCaseDiagramActor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOptionORMDiagramActor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Process As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOption_EntityTypeMorphTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOptionUseCaseDiagramProcess As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_EntityTypeProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents MenuOptionDataFlowDiagramActor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerLinkSwitch As System.Windows.Forms.Timer
    Friend WithEvents MenuOptionDataFlowDiagramProcess As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageAsORMMetaModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFlowChart As System.Windows.Forms.ToolStripMenuItem
End Class
