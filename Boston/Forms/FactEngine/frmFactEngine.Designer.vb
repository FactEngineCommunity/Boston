<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFactEngine
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFactEngine))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TextBoxInput = New System.Windows.Forms.RichTextBox()
        Me.ContextMenuStripFactEngine = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemHelpTips = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelHelp = New System.Windows.Forms.Label()
        Me.ContextMenuStripHelpLabel = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonGO = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelWorkingModelName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelLookingFor = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelCurrentProduction = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelRequiresConnectionString = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelGOPrompt = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageResults = New System.Windows.Forms.TabPage()
        Me.LabelError = New System.Windows.Forms.TextBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.TabPageQuery = New System.Windows.Forms.TabPage()
        Me.TextBoxQuery = New System.Windows.Forms.TextBox()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonQueryGO = New System.Windows.Forms.ToolStripButton()
        Me.TabPageGraph = New System.Windows.Forms.TabPage()
        Me.GraphView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ContextMenuStripGraph = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VirtualAnalystToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStripFactEngine.SuspendLayout()
        Me.ContextMenuStripHelpLabel.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip2.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageResults.SuspendLayout()
        Me.TabPageQuery.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.TabPageGraph.SuspendLayout()
        Me.ContextMenuStripGraph.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextBoxInput)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelHelp)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.StatusStrip2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.StatusStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1239, 737)
        Me.SplitContainer1.SplitterDistance = 398
        Me.SplitContainer1.SplitterWidth = 5
        Me.SplitContainer1.TabIndex = 0
        '
        'TextBoxInput
        '
        Me.TextBoxInput.AcceptsTab = True
        Me.TextBoxInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TextBoxInput.ContextMenuStrip = Me.ContextMenuStripFactEngine
        Me.TextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxInput.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxInput.ForeColor = System.Drawing.Color.Wheat
        Me.TextBoxInput.Location = New System.Drawing.Point(0, 33)
        Me.TextBoxInput.Name = "TextBoxInput"
        Me.TextBoxInput.Size = New System.Drawing.Size(1239, 202)
        Me.TextBoxInput.TabIndex = 2
        Me.TextBoxInput.TabStop = False
        Me.TextBoxInput.Text = ""
        '
        'ContextMenuStripFactEngine
        '
        Me.ContextMenuStripFactEngine.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripFactEngine.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemHelpTips, Me.ViewToolStripMenuItem})
        Me.ContextMenuStripFactEngine.Name = "ContextMenuStripFactEngine"
        Me.ContextMenuStripFactEngine.Size = New System.Drawing.Size(241, 101)
        '
        'ToolStripMenuItemHelpTips
        '
        Me.ToolStripMenuItemHelpTips.Checked = True
        Me.ToolStripMenuItemHelpTips.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemHelpTips.Name = "ToolStripMenuItemHelpTips"
        Me.ToolStripMenuItemHelpTips.Size = New System.Drawing.Size(240, 32)
        Me.ToolStripMenuItemHelpTips.Text = "&Help Tips"
        '
        'LabelHelp
        '
        Me.LabelHelp.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelp.ContextMenuStrip = Me.ContextMenuStripHelpLabel
        Me.LabelHelp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelp.ForeColor = System.Drawing.Color.DarkGray
        Me.LabelHelp.Location = New System.Drawing.Point(0, 235)
        Me.LabelHelp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(1239, 109)
        Me.LabelHelp.TabIndex = 12
        '
        'ContextMenuStripHelpLabel
        '
        Me.ContextMenuStripHelpLabel.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripHelpLabel.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideToolStripMenuItem})
        Me.ContextMenuStripHelpLabel.Name = "ContextMenuStripHelpLabel"
        Me.ContextMenuStripHelpLabel.Size = New System.Drawing.Size(122, 36)
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(121, 32)
        Me.HideToolStripMenuItem.Text = "&Hide"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonGO})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip1.Size = New System.Drawing.Size(1239, 33)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonGO
        '
        Me.ToolStripButtonGO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonGO.Image = Global.Boston.My.Resources.MenuImagesMain.GO16x16
        Me.ToolStripButtonGO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonGO.Name = "ToolStripButtonGO"
        Me.ToolStripButtonGO.Size = New System.Drawing.Size(34, 28)
        Me.ToolStripButtonGO.Text = "ToolStripButton1"
        '
        'StatusStrip2
        '
        Me.StatusStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelWorkingModelName, Me.ToolStripStatusLabelLookingFor, Me.ToolStripStatusLabelCurrentProduction, Me.ToolStripStatusLabelRequiresConnectionString, Me.ToolStripStatusLabelGOPrompt})
        Me.StatusStrip2.Location = New System.Drawing.Point(0, 344)
        Me.StatusStrip2.Name = "StatusStrip2"
        Me.StatusStrip2.Padding = New System.Windows.Forms.Padding(2, 0, 14, 0)
        Me.StatusStrip2.Size = New System.Drawing.Size(1239, 32)
        Me.StatusStrip2.TabIndex = 1
        Me.StatusStrip2.Text = "StatusStrip2"
        '
        'ToolStripStatusLabelWorkingModelName
        '
        Me.ToolStripStatusLabelWorkingModelName.Name = "ToolStripStatusLabelWorkingModelName"
        Me.ToolStripStatusLabelWorkingModelName.Size = New System.Drawing.Size(177, 25)
        Me.ToolStripStatusLabelWorkingModelName.Text = "WorkingModelName"
        '
        'ToolStripStatusLabelLookingFor
        '
        Me.ToolStripStatusLabelLookingFor.Name = "ToolStripStatusLabelLookingFor"
        Me.ToolStripStatusLabelLookingFor.Size = New System.Drawing.Size(92, 25)
        Me.ToolStripStatusLabelLookingFor.Text = "Expecting:"
        '
        'ToolStripStatusLabelCurrentProduction
        '
        Me.ToolStripStatusLabelCurrentProduction.Name = "ToolStripStatusLabelCurrentProduction"
        Me.ToolStripStatusLabelCurrentProduction.Size = New System.Drawing.Size(157, 25)
        Me.ToolStripStatusLabelCurrentProduction.Text = "CurrentProduction"
        '
        'ToolStripStatusLabelRequiresConnectionString
        '
        Me.ToolStripStatusLabelRequiresConnectionString.Name = "ToolStripStatusLabelRequiresConnectionString"
        Me.ToolStripStatusLabelRequiresConnectionString.Size = New System.Drawing.Size(215, 25)
        Me.ToolStripStatusLabelRequiresConnectionString.Text = "RequiresConnectionString"
        '
        'ToolStripStatusLabelGOPrompt
        '
        Me.ToolStripStatusLabelGOPrompt.Name = "ToolStripStatusLabelGOPrompt"
        Me.ToolStripStatusLabelGOPrompt.Size = New System.Drawing.Size(98, 25)
        Me.ToolStripStatusLabelGOPrompt.Text = "GOPrompt"
        Me.ToolStripStatusLabelGOPrompt.Visible = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 376)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(2, 0, 14, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1239, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageResults)
        Me.TabControl1.Controls.Add(Me.TabPageQuery)
        Me.TabControl1.Controls.Add(Me.TabPageGraph)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1239, 334)
        Me.TabControl1.TabIndex = 2
        '
        'TabPageResults
        '
        Me.TabPageResults.Controls.Add(Me.LabelError)
        Me.TabPageResults.Controls.Add(Me.ToolStrip2)
        Me.TabPageResults.Location = New System.Drawing.Point(4, 29)
        Me.TabPageResults.Name = "TabPageResults"
        Me.TabPageResults.Padding = New System.Windows.Forms.Padding(3, 3, 3, 3)
        Me.TabPageResults.Size = New System.Drawing.Size(1231, 301)
        Me.TabPageResults.TabIndex = 0
        Me.TabPageResults.Text = "Results"
        Me.TabPageResults.UseVisualStyleBackColor = True
        '
        'LabelError
        '
        Me.LabelError.BackColor = System.Drawing.SystemColors.Control
        Me.LabelError.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelError.Location = New System.Drawing.Point(3, 28)
        Me.LabelError.Multiline = True
        Me.LabelError.Name = "LabelError"
        Me.LabelError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LabelError.Size = New System.Drawing.Size(1225, 270)
        Me.LabelError.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip2.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip2.Size = New System.Drawing.Size(1225, 25)
        Me.ToolStrip2.TabIndex = 2
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'TabPageQuery
        '
        Me.TabPageQuery.Controls.Add(Me.TextBoxQuery)
        Me.TabPageQuery.Controls.Add(Me.ToolStrip3)
        Me.TabPageQuery.Location = New System.Drawing.Point(4, 29)
        Me.TabPageQuery.Name = "TabPageQuery"
        Me.TabPageQuery.Padding = New System.Windows.Forms.Padding(3, 3, 3, 3)
        Me.TabPageQuery.Size = New System.Drawing.Size(1231, 301)
        Me.TabPageQuery.TabIndex = 1
        Me.TabPageQuery.Text = "Query"
        Me.TabPageQuery.UseVisualStyleBackColor = True
        '
        'TextBoxQuery
        '
        Me.TextBoxQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxQuery.Location = New System.Drawing.Point(3, 36)
        Me.TextBoxQuery.Multiline = True
        Me.TextBoxQuery.Name = "TextBoxQuery"
        Me.TextBoxQuery.Size = New System.Drawing.Size(1225, 262)
        Me.TextBoxQuery.TabIndex = 2
        '
        'ToolStrip3
        '
        Me.ToolStrip3.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonQueryGO})
        Me.ToolStrip3.Location = New System.Drawing.Point(3, 3)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip3.Size = New System.Drawing.Size(1225, 33)
        Me.ToolStrip3.TabIndex = 3
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'ToolStripButtonQueryGO
        '
        Me.ToolStripButtonQueryGO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonQueryGO.Image = Global.Boston.My.Resources.MenuImagesMain.GO16x16
        Me.ToolStripButtonQueryGO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonQueryGO.Name = "ToolStripButtonQueryGO"
        Me.ToolStripButtonQueryGO.Size = New System.Drawing.Size(34, 28)
        Me.ToolStripButtonQueryGO.Text = "ToolStripButton1"
        '
        'TabPageGraph
        '
        Me.TabPageGraph.Controls.Add(Me.GraphView)
        Me.TabPageGraph.Location = New System.Drawing.Point(4, 29)
        Me.TabPageGraph.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageGraph.Name = "TabPageGraph"
        Me.TabPageGraph.Size = New System.Drawing.Size(1231, 301)
        Me.TabPageGraph.TabIndex = 2
        Me.TabPageGraph.Text = "Graph"
        Me.TabPageGraph.UseVisualStyleBackColor = True
        '
        'GraphView
        '
        Me.GraphView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.GraphView.ContextMenuStrip = Me.ContextMenuStripGraph
        Me.GraphView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.GraphView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.GraphView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.GraphView.Diagram = Me.Diagram
        Me.GraphView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GraphView.Location = New System.Drawing.Point(0, 0)
        Me.GraphView.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GraphView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.GraphView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.GraphView.Name = "GraphView"
        Me.GraphView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.GraphView.Size = New System.Drawing.Size(1231, 301)
        Me.GraphView.TabIndex = 0
        Me.GraphView.Text = "DiagramView1"
        '
        'ContextMenuStripGraph
        '
        Me.ContextMenuStripGraph.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripGraph.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LayoutToolStripMenuItem})
        Me.ContextMenuStripGraph.Name = "ContextMenuStripGraph"
        Me.ContextMenuStripGraph.Size = New System.Drawing.Size(138, 36)
        '
        'LayoutToolStripMenuItem
        '
        Me.LayoutToolStripMenuItem.Name = "LayoutToolStripMenuItem"
        Me.LayoutToolStripMenuItem.Size = New System.Drawing.Size(137, 32)
        Me.LayoutToolStripMenuItem.Text = "&Layout"
        '
        'Diagram
        '
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Diagram.LinkStyle = MindFusion.Diagramming.LinkStyle.Bezier
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VirtualAnalystToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(240, 32)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'VirtualAnalystToolStripMenuItem
        '
        Me.VirtualAnalystToolStripMenuItem.Name = "VirtualAnalystToolStripMenuItem"
        Me.VirtualAnalystToolStripMenuItem.Size = New System.Drawing.Size(270, 34)
        Me.VirtualAnalystToolStripMenuItem.Text = "Virtual Analyst"
        '
        'frmFactEngine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1239, 737)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFactEngine"
        Me.Text = "Fact Engine"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStripFactEngine.ResumeLayout(False)
        Me.ContextMenuStripHelpLabel.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip2.ResumeLayout(False)
        Me.StatusStrip2.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageResults.ResumeLayout(False)
        Me.TabPageResults.PerformLayout()
        Me.TabPageQuery.ResumeLayout(False)
        Me.TabPageQuery.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.TabPageGraph.ResumeLayout(False)
        Me.ContextMenuStripGraph.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TextBoxInput As RichTextBox
    Friend WithEvents StatusStrip2 As StatusStrip
    Friend WithEvents ToolStripStatusLabelWorkingModelName As ToolStripStatusLabel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButtonGO As ToolStripButton
    Friend WithEvents ToolStripStatusLabelCurrentProduction As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelLookingFor As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelRequiresConnectionString As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelGOPrompt As ToolStripStatusLabel
    Friend WithEvents LabelError As TextBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageQuery As TabPage
    Friend WithEvents TextBoxQuery As TextBox
    Friend WithEvents TabPageResults As TabPage
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents ToolStrip3 As ToolStrip
    Friend WithEvents ToolStripButtonQueryGO As ToolStripButton
    Friend WithEvents LabelHelp As Label
    Friend WithEvents ContextMenuStripHelpLabel As ContextMenuStrip
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripFactEngine As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemHelpTips As ToolStripMenuItem
    Friend WithEvents TabPageGraph As TabPage
    Friend WithEvents GraphView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStripGraph As ContextMenuStrip
    Friend WithEvents LayoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VirtualAnalystToolStripMenuItem As ToolStripMenuItem
End Class
