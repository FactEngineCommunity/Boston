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
        Me.AutoLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_State = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_StateTransition = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveFromPageAndModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerLinkSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_EntityType = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOption_EntityTypeMorphTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOption_EntityTypeProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabel_ValueType = New System.Windows.Forms.ToolStripLabel()
        Me.ComboBox_ValueType = New System.Windows.Forms.ToolStripComboBox()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ContextMenuStrip_StartStateTransition = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_State.SuspendLayout()
        Me.ContextMenuStrip_StateTransition.SuspendLayout()
        Me.ContextMenuStrip_EntityType.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.ContextMenuStrip_StartStateTransition.SuspendLayout()
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
        Me.Diagram.ShadowColor = System.Drawing.Color.White
        Me.Diagram.ShadowOffsetX = 0!
        Me.Diagram.ShadowOffsetY = 0!
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.ShowAnchors = MindFusion.Diagramming.ShowAnchors.Always
        Me.Diagram.ShowHandlesOnDrag = False
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator1, Me.AutoLayoutToolStripMenuItem, Me.ToolStripSeparator7, Me.CopyToolStripMenuItem})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 104)
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
        Me.ToolboxToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Toolbox16x16B_W
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
        'AutoLayoutToolStripMenuItem
        '
        Me.AutoLayoutToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.Layout16x16
        Me.AutoLayoutToolStripMenuItem.Name = "AutoLayoutToolStripMenuItem"
        Me.AutoLayoutToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.AutoLayoutToolStripMenuItem.Text = "&AutoLayout"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(204, 6)
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
        Me.ContextMenuStrip_State.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator4, Me.RemoveFromPageToolStripMenuItem, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_State.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_State.Size = New System.Drawing.Size(176, 76)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ORMDiagramToolStripMenuItem})
        Me.MorphToToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To"
        '
        'ORMDiagramToolStripMenuItem
        '
        Me.ORMDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.ORMDiagramToolStripMenuItem.Name = "ORMDiagramToolStripMenuItem"
        Me.ORMDiagramToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ORMDiagramToolStripMenuItem.Text = "&ORM Diagram"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(172, 6)
        '
        'RemoveFromPageToolStripMenuItem
        '
        Me.RemoveFromPageToolStripMenuItem.Name = "RemoveFromPageToolStripMenuItem"
        Me.RemoveFromPageToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.RemoveFromPageToolStripMenuItem.Text = "&Remove from Page"
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(175, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'ContextMenuStrip_StateTransition
        '
        Me.ContextMenuStrip_StateTransition.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFromPageAndModelToolStripMenuItem})
        Me.ContextMenuStrip_StateTransition.Name = "ContextMenuStrip_Process"
        Me.ContextMenuStrip_StateTransition.Size = New System.Drawing.Size(236, 26)
        '
        'RemoveFromPageAndModelToolStripMenuItem
        '
        Me.RemoveFromPageAndModelToolStripMenuItem.Name = "RemoveFromPageAndModelToolStripMenuItem"
        Me.RemoveFromPageAndModelToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageAndModelToolStripMenuItem.Text = "&Remove from Page and Model"
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
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel_ValueType, Me.ComboBox_ValueType})
        Me.ToolStrip1.Location = New System.Drawing.Point(81, 9)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(417, 33)
        Me.ToolStrip1.TabIndex = 11
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabel_ValueType
        '
        Me.ToolStripLabel_ValueType.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripLabel_ValueType.Name = "ToolStripLabel_ValueType"
        Me.ToolStripLabel_ValueType.Size = New System.Drawing.Size(112, 30)
        Me.ToolStripLabel_ValueType.Text = "Value Type :"
        '
        'ComboBox_ValueType
        '
        Me.ComboBox_ValueType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ValueType.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox_ValueType.ForeColor = System.Drawing.Color.SteelBlue
        Me.ComboBox_ValueType.Name = "ComboBox_ValueType"
        Me.ComboBox_ValueType.Size = New System.Drawing.Size(300, 33)
        '
        'DiagramView
        '
        Me.DiagramView.AllowDrop = True
        Me.DiagramView.AllowInplaceEdit = True
        Me.DiagramView.BackColor = System.Drawing.Color.White
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Diagram
        Me.DiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.DiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.IgnoreControl
        Me.DiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.DiagramView.Diagram = Me.Diagram
        Me.DiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiagramView.InplaceEditAcceptOnEnter = True
        Me.DiagramView.Location = New System.Drawing.Point(0, 0)
        Me.DiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.DiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.DiagramView.Name = "DiagramView"
        Me.DiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.DiagramView.Size = New System.Drawing.Size(891, 530)
        Me.DiagramView.TabIndex = 1
        Me.DiagramView.Text = "DiagramView1"
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
        'ContextMenuStrip_StartStateTransition
        '
        Me.ContextMenuStrip_StartStateTransition.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.ContextMenuStrip_StartStateTransition.Name = "ContextMenuStrip_Process"
        Me.ContextMenuStrip_StartStateTransition.Size = New System.Drawing.Size(236, 48)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(235, 22)
        Me.ToolStripMenuItem1.Text = "&Remove from Page and Model"
        '
        'frmStateTransitionDiagram
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(891, 530)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.DiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmStateTransitionDiagram"
        Me.TabText = "State Transition Diagram"
        Me.Text = "State Transition Diagram"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_State.ResumeLayout(False)
        Me.ContextMenuStrip_StateTransition.ResumeLayout(False)
        Me.ContextMenuStrip_EntityType.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ContextMenuStrip_StartStateTransition.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStrip_State As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuStrip_StateTransition As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerLinkSwitch As System.Windows.Forms.Timer
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_EntityType As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOption_EntityTypeMorphTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_EntityTypeProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel_ValueType As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ComboBox_ValueType As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageAsORMMetaModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoLayoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents RemoveFromPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RemoveFromPageAndModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_StartStateTransition As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
End Class
