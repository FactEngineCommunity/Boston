<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmToolboxTaxonomyTree
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxTaxonomyTree))
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.GroupBoxTaxonomy = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageTreeLayout = New System.Windows.Forms.TabPage()
        Me.ImageListTreeView = New System.Windows.Forms.ImageList(Me.components)
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModelName = New System.Windows.Forms.Label()
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ErrorListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMVerbalisationViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RichmondBrainBoxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_TreeViewNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemValueTypeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInModelDictionaryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeView = New BostonTreeView()
        Me.GroupBox.SuspendLayout()
        Me.GroupBoxTaxonomy.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageTreeLayout.SuspendLayout()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_TreeViewNode.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.GroupBoxTaxonomy)
        Me.GroupBox.Controls.Add(Me.LabelModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelName)
        Me.GroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(804, 597)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'GroupBoxTaxonomy
        '
        Me.GroupBoxTaxonomy.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxTaxonomy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBoxTaxonomy.Controls.Add(Me.TabControl1)
        Me.GroupBoxTaxonomy.Location = New System.Drawing.Point(15, 51)
        Me.GroupBoxTaxonomy.Name = "GroupBoxTaxonomy"
        Me.GroupBoxTaxonomy.Size = New System.Drawing.Size(783, 540)
        Me.GroupBoxTaxonomy.TabIndex = 4
        Me.GroupBoxTaxonomy.TabStop = False
        Me.GroupBoxTaxonomy.Text = "Taxonomy:"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageTreeLayout)
        Me.TabControl1.Location = New System.Drawing.Point(6, 19)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(771, 515)
        Me.TabControl1.TabIndex = 0
        '
        'TabPageTreeLayout
        '
        Me.TabPageTreeLayout.Controls.Add(Me.TreeView)
        Me.TabPageTreeLayout.Location = New System.Drawing.Point(4, 22)
        Me.TabPageTreeLayout.Name = "TabPageTreeLayout"
        Me.TabPageTreeLayout.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageTreeLayout.Size = New System.Drawing.Size(763, 489)
        Me.TabPageTreeLayout.TabIndex = 0
        Me.TabPageTreeLayout.Text = "Tree Layout"
        Me.TabPageTreeLayout.UseVisualStyleBackColor = True
        '
        'ImageListTreeView
        '
        Me.ImageListTreeView.ImageStream = CType(resources.GetObject("ImageListTreeView.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListTreeView.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListTreeView.Images.SetKeyName(0, "TaxonomyModel16x16.png")
        Me.ImageListTreeView.Images.SetKeyName(1, "Entity-B&W16x16.png")
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(88, 19)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 3
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelPromptModelName
        '
        Me.LabelPromptModelName.AutoSize = True
        Me.LabelPromptModelName.Location = New System.Drawing.Point(12, 19)
        Me.LabelPromptModelName.Name = "LabelPromptModelName"
        Me.LabelPromptModelName.Size = New System.Drawing.Size(70, 13)
        Me.LabelPromptModelName.TabIndex = 2
        Me.LabelPromptModelName.Text = "Model Name:"
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem1})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(100, 26)
        '
        'ViewToolStripMenuItem1
        '
        Me.ViewToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator18, Me.ErrorListToolStripMenuItem, Me.ToolStripMenuItem8, Me.ORMVerbalisationViewToolStripMenuItem, Me.RichmondBrainBoxToolStripMenuItem})
        Me.ViewToolStripMenuItem1.Name = "ViewToolStripMenuItem1"
        Me.ViewToolStripMenuItem1.Size = New System.Drawing.Size(99, 22)
        Me.ViewToolStripMenuItem1.Text = "&View"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(200, 6)
        '
        'ErrorListToolStripMenuItem
        '
        Me.ErrorListToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.ErrorList
        Me.ErrorListToolStripMenuItem.Name = "ErrorListToolStripMenuItem"
        Me.ErrorListToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ErrorListToolStripMenuItem.Text = "&Error List"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Image = Global.Boston.My.Resources.MenuImages.FactTypeReading16x16
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(203, 22)
        Me.ToolStripMenuItem8.Text = "Fact Type &Reading Editor"
        '
        'ORMVerbalisationViewToolStripMenuItem
        '
        Me.ORMVerbalisationViewToolStripMenuItem.Image = CType(resources.GetObject("ORMVerbalisationViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMVerbalisationViewToolStripMenuItem.Name = "ORMVerbalisationViewToolStripMenuItem"
        Me.ORMVerbalisationViewToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ORMVerbalisationViewToolStripMenuItem.Text = "ORM &Verbalisation View"
        '
        'RichmondBrainBoxToolStripMenuItem
        '
        Me.RichmondBrainBoxToolStripMenuItem.Image = CType(resources.GetObject("RichmondBrainBoxToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RichmondBrainBoxToolStripMenuItem.Name = "RichmondBrainBoxToolStripMenuItem"
        Me.RichmondBrainBoxToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.RichmondBrainBoxToolStripMenuItem.Text = "&Virtual Analyst"
        '
        'ContextMenuStrip_TreeViewNode
        '
        Me.ContextMenuStrip_TreeViewNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInModelDictionaryToolStripMenuItem1, Me.ToolStripMenuItemValueTypeModelErrors, Me.ToolStripSeparator19, Me.ToolStripMenuItem10})
        Me.ContextMenuStrip_TreeViewNode.Name = "ContextMenuStrip_ValueType"
        Me.ContextMenuStrip_TreeViewNode.Size = New System.Drawing.Size(211, 98)
        '
        'ToolStripMenuItemValueTypeModelErrors
        '
        Me.ToolStripMenuItemValueTypeModelErrors.Name = "ToolStripMenuItemValueTypeModelErrors"
        Me.ToolStripMenuItemValueTypeModelErrors.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemValueTypeModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(207, 6)
        '
        'ShowInModelDictionaryToolStripMenuItem1
        '
        Me.ShowInModelDictionaryToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem1.Name = "ShowInModelDictionaryToolStripMenuItem1"
        Me.ShowInModelDictionaryToolStripMenuItem1.Size = New System.Drawing.Size(210, 22)
        Me.ShowInModelDictionaryToolStripMenuItem1.Text = "Show in Model &Dictionary"
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItem10.Text = "&Properties"
        '
        'TreeView
        '
        Me.TreeView.AllowDrop = True
        Me.TreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView.BackColor = System.Drawing.SystemColors.Window
        Me.TreeView.ForeColor = System.Drawing.Color.Black
        Me.TreeView.HideSelection = False
        Me.TreeView.ImageIndex = 0
        Me.TreeView.ImageList = Me.ImageListTreeView
        Me.TreeView.LabelEdit = True
        Me.TreeView.Location = New System.Drawing.Point(3, 3)
        Me.TreeView.MinimumSize = New System.Drawing.Size(190, 4)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.SelectedImageKey = "blank.ico"
        Me.TreeView.SelectedNode = Nothing
        Me.TreeView.Size = New System.Drawing.Size(757, 483)
        Me.TreeView.TabIndex = 1
        '
        'frmToolboxTaxonomyTree
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 597)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmToolboxTaxonomyTree"
        Me.Text = "Taxonomy Tree"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.GroupBoxTaxonomy.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageTreeLayout.ResumeLayout(False)
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_TreeViewNode.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents LabelPromptModelName As Label
    Friend WithEvents LabelModelName As Label
    Friend WithEvents GroupBoxTaxonomy As GroupBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageTreeLayout As TabPage
    Friend WithEvents TreeView As BostonTreeView
    Friend WithEvents ImageListTreeView As ImageList
    Friend WithEvents ContextMenuStrip_Diagram As ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents ErrorListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As ToolStripMenuItem
    Friend WithEvents ORMVerbalisationViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RichmondBrainBoxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_TreeViewNode As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemValueTypeModelErrors As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem10 As ToolStripMenuItem
End Class
