<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmStateTransitionDiagram
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.StateTransitionDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.PageAsORMMetaModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_State = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseCaseDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Process = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_ProcessLink = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseCaseDiagramToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerLinkSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_EntityType = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOption_EntityTypeMorphTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOption_EntityTypeProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel_ValueType = New System.Windows.Forms.ToolStripLabel()
        Me.ComboBox_ValueType = New System.Windows.Forms.ToolStripComboBox()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_State.SuspendLayout()
        Me.ContextMenuStrip_Process.SuspendLayout()
        Me.ContextMenuStrip_ProcessLink.SuspendLayout()
        Me.ContextMenuStrip_EntityType.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Diagram
        '
        Me.Diagram.AllowSelfLoops = False
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.DynamicLinks = True
        Me.Diagram.HitTestPriority = MindFusion.Diagramming.HitTestPriority.ZOrder
        Me.Diagram.LinkBrush = New MindFusion.Drawing.SolidBrush("#FF000000")
        Me.Diagram.LinkHeadShape = MindFusion.Diagramming.ArrowHead.PointerArrow
        Me.Diagram.LinkHeadShapeSize = 3.0!
        Me.Diagram.LinkIntermediateShapeSize = 3.0!
        Me.Diagram.LinksSnapToBorders = True
        Me.Diagram.SelectAfterCreate = False
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.ShowAnchors = MindFusion.Diagramming.ShowAnchors.Always
        Me.Diagram.ShowHandlesOnDrag = False
        '
        'StateTransitionDiagramView
        '
        Me.StateTransitionDiagramView.AllowDrop = True
        Me.StateTransitionDiagramView.BackColor = System.Drawing.Color.White
        Me.StateTransitionDiagramView.Behavior = MindFusion.Diagramming.Behavior.DrawLinks
        Me.StateTransitionDiagramView.ContextMenuStrip = Me.ContextMenuStrip_Diagram
        Me.StateTransitionDiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.StateTransitionDiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.IgnoreControl
        Me.StateTransitionDiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.StateTransitionDiagramView.Diagram = Me.Diagram
        Me.StateTransitionDiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StateTransitionDiagramView.Location = New System.Drawing.Point(0, 0)
        Me.StateTransitionDiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.StateTransitionDiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.StateTransitionDiagramView.Name = "StateTransitionDiagramView"
        Me.StateTransitionDiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.StateTransitionDiagramView.Size = New System.Drawing.Size(891, 530)
        Me.StateTransitionDiagramView.TabIndex = 1
        Me.StateTransitionDiagramView.Text = "DiagramView1"
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator1, Me.CopyToolStripMenuItem})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 76)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolboxToolStripMenuItem, Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator6, Me.PageAsORMMetaModelToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ToolboxToolStripMenuItem
        '
        Me.ToolboxToolStripMenuItem.Name = "ToolboxToolStripMenuItem"
        Me.ToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ToolboxToolStripMenuItem.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(205, 6)
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
        Me.mnuOption_ViewGrid.Image = Global.Boston.My.Resources.MenuImagesMain.Grid
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(96, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(204, 6)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.Camera16x16
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ContextMenuStrip_State
        '
        Me.ContextMenuStrip_State.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator4, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_State.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_State.Size = New System.Drawing.Size(128, 54)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseCaseDiagramToolStripMenuItem, Me.ORMDiagramToolStripMenuItem})
        Me.MorphToToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To"
        '
        'UseCaseDiagramToolStripMenuItem
        '
        Me.UseCaseDiagramToolStripMenuItem.Name = "UseCaseDiagramToolStripMenuItem"
        Me.UseCaseDiagramToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.UseCaseDiagramToolStripMenuItem.Text = "&Use Case Diagram"
        '
        'ORMDiagramToolStripMenuItem
        '
        Me.ORMDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.ORMDiagramToolStripMenuItem.Name = "ORMDiagramToolStripMenuItem"
        Me.ORMDiagramToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.ORMDiagramToolStripMenuItem.Text = "&ORM Diagram"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(124, 6)
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(127, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'ContextMenuStrip_Process
        '
        Me.ContextMenuStrip_Process.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripSeparator3, Me.PropertiesToolStripMenuItem2})
        Me.ContextMenuStrip_Process.Name = "ContextMenuStrip_Process"
        Me.ContextMenuStrip_Process.Size = New System.Drawing.Size(134, 54)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2})
        Me.ToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(133, 22)
        Me.ToolStripMenuItem1.Text = "&Morph to..."
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(257, 22)
        Me.ToolStripMenuItem2.Text = "&Data Flow Diagram for this Process"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(130, 6)
        '
        'PropertiesToolStripMenuItem2
        '
        Me.PropertiesToolStripMenuItem2.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem2.Name = "PropertiesToolStripMenuItem2"
        Me.PropertiesToolStripMenuItem2.Size = New System.Drawing.Size(133, 22)
        Me.PropertiesToolStripMenuItem2.Text = "&Properties"
        '
        'ContextMenuStrip_ProcessLink
        '
        Me.ContextMenuStrip_ProcessLink.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem1, Me.ToolStripSeparator2, Me.PropertiesToolStripMenuItem3})
        Me.ContextMenuStrip_ProcessLink.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_ProcessLink.Size = New System.Drawing.Size(128, 54)
        '
        'MorphToToolStripMenuItem1
        '
        Me.MorphToToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseCaseDiagramToolStripMenuItem1})
        Me.MorphToToolStripMenuItem1.Name = "MorphToToolStripMenuItem1"
        Me.MorphToToolStripMenuItem1.Size = New System.Drawing.Size(127, 22)
        Me.MorphToToolStripMenuItem1.Text = "&Morph To"
        '
        'UseCaseDiagramToolStripMenuItem1
        '
        Me.UseCaseDiagramToolStripMenuItem1.Name = "UseCaseDiagramToolStripMenuItem1"
        Me.UseCaseDiagramToolStripMenuItem1.Size = New System.Drawing.Size(169, 22)
        Me.UseCaseDiagramToolStripMenuItem1.Text = "&Use Case Diagram"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(124, 6)
        '
        'PropertiesToolStripMenuItem3
        '
        Me.PropertiesToolStripMenuItem3.Name = "PropertiesToolStripMenuItem3"
        Me.PropertiesToolStripMenuItem3.Size = New System.Drawing.Size(127, 22)
        Me.PropertiesToolStripMenuItem3.Text = "&Properties"
        '
        'TimerLinkSwitch
        '
        Me.TimerLinkSwitch.Interval = 1000
        '
        'ContextMenuStrip_EntityType
        '
        Me.ContextMenuStrip_EntityType.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_EntityTypeMorphTo, Me.ToolStripSeparator5, Me.mnuOption_EntityTypeProperties})
        Me.ContextMenuStrip_EntityType.Name = "ContextMenuStrip_EntityType"
        Me.ContextMenuStrip_EntityType.Size = New System.Drawing.Size(134, 54)
        '
        'mnuOption_EntityTypeMorphTo
        '
        Me.mnuOption_EntityTypeMorphTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UseCToolStripMenuItem})
        Me.mnuOption_EntityTypeMorphTo.Name = "mnuOption_EntityTypeMorphTo"
        Me.mnuOption_EntityTypeMorphTo.Size = New System.Drawing.Size(133, 22)
        Me.mnuOption_EntityTypeMorphTo.Text = "&Morph to..."
        '
        'UseCToolStripMenuItem
        '
        Me.UseCToolStripMenuItem.Name = "UseCToolStripMenuItem"
        Me.UseCToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.UseCToolStripMenuItem.Text = "&Use Case Diagram for this EntityType/Actor"
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
        Me.HiddenDiagramView.Size = New System.Drawing.Size(891, 530)
        Me.HiddenDiagramView.TabIndex = 10
        Me.HiddenDiagramView.Text = "DiagramView1"
        '
        'HiddenDiagram
        '
        Me.HiddenDiagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.HiddenDiagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'MorphTimer
        '
        Me.MorphTimer.Interval = 1500
        '
        'MorphStepTimer
        '
        Me.MorphStepTimer.Interval = 25
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.White
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel_ValueType, Me.ComboBox_ValueType})
        Me.ToolStrip1.Location = New System.Drawing.Point(113, 9)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(282, 25)
        Me.ToolStrip1.TabIndex = 11
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabel_ValueType
        '
        Me.ToolStripLabel_ValueType.Name = "ToolStripLabel_ValueType"
        Me.ToolStripLabel_ValueType.Size = New System.Drawing.Size(68, 22)
        Me.ToolStripLabel_ValueType.Text = "Value Type :"
        '
        'ComboBox_ValueType
        '
        Me.ComboBox_ValueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ValueType.Name = "ComboBox_ValueType"
        Me.ComboBox_ValueType.Size = New System.Drawing.Size(200, 25)
        '
        'frmStateTransitionDiagram
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(891, 530)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StateTransitionDiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmStateTransitionDiagram"
        Me.TabText = "State Transition Diagram"
        Me.Text = "State Transition Diagram"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_State.ResumeLayout(False)
        Me.ContextMenuStrip_Process.ResumeLayout(False)
        Me.ContextMenuStrip_ProcessLink.ResumeLayout(False)
        Me.ContextMenuStrip_EntityType.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents StateTransitionDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStrip_State As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuStrip_Process As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ProcessLink As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PropertiesToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerLinkSwitch As System.Windows.Forms.Timer
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStrip_EntityType As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOption_EntityTypeMorphTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_EntityTypeProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseCaseDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel_ValueType As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ComboBox_ValueType As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents MorphToToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseCaseDiagramToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageAsORMMetaModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
