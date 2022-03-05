<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxModelDictionary
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxModelDictionary))
        Me.GroupBox_Main = New System.Windows.Forms.GroupBox()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.CheckBoxShowModelDictionary = New System.Windows.Forms.CheckBox()
        Me.CheckBoxShowCoreModelElements = New System.Windows.Forms.CheckBox()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPrompt = New System.Windows.Forms.Label()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemViewOnPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemViewInDiagramSpy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRemoveFromModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemMakeNewPageForThisModelElement = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelPromptRealisationsCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelRealisationsCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelPromptModelElementTypeCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelModelElementTypeCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ViewInGlossaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox_Main.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_Main
        '
        Me.GroupBox_Main.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox_Main.Controls.Add(Me.ButtonRefresh)
        Me.GroupBox_Main.Controls.Add(Me.CheckBoxShowModelDictionary)
        Me.GroupBox_Main.Controls.Add(Me.CheckBoxShowCoreModelElements)
        Me.GroupBox_Main.Controls.Add(Me.LabelModelName)
        Me.GroupBox_Main.Controls.Add(Me.LabelPrompt)
        Me.GroupBox_Main.Controls.Add(Me.TreeView1)
        Me.GroupBox_Main.ForeColor = System.Drawing.Color.Black
        Me.GroupBox_Main.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Main.Name = "GroupBox_Main"
        Me.GroupBox_Main.Size = New System.Drawing.Size(346, 456)
        Me.GroupBox_Main.TabIndex = 0
        Me.GroupBox_Main.TabStop = False
        Me.GroupBox_Main.Text = "Model Dictionary:"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(316, 25)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(24, 23)
        Me.ButtonRefresh.TabIndex = 6
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'CheckBoxShowModelDictionary
        '
        Me.CheckBoxShowModelDictionary.AutoSize = True
        Me.CheckBoxShowModelDictionary.Location = New System.Drawing.Point(183, 26)
        Me.CheckBoxShowModelDictionary.Name = "CheckBoxShowModelDictionary"
        Me.CheckBoxShowModelDictionary.Size = New System.Drawing.Size(135, 17)
        Me.CheckBoxShowModelDictionary.TabIndex = 5
        Me.CheckBoxShowModelDictionary.Text = "Show Model Dictionary"
        Me.CheckBoxShowModelDictionary.UseVisualStyleBackColor = True
        Me.CheckBoxShowModelDictionary.Visible = False
        '
        'CheckBoxShowCoreModelElements
        '
        Me.CheckBoxShowCoreModelElements.AutoSize = True
        Me.CheckBoxShowCoreModelElements.Location = New System.Drawing.Point(183, 9)
        Me.CheckBoxShowCoreModelElements.Name = "CheckBoxShowCoreModelElements"
        Me.CheckBoxShowCoreModelElements.Size = New System.Drawing.Size(156, 17)
        Me.CheckBoxShowCoreModelElements.TabIndex = 4
        Me.CheckBoxShowCoreModelElements.Text = "Show Core Model Elements"
        Me.CheckBoxShowCoreModelElements.UseVisualStyleBackColor = True
        Me.CheckBoxShowCoreModelElements.Visible = False
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(53, 25)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(102, 13)
        Me.LabelModelName.TabIndex = 3
        Me.LabelModelName.Text = "<LabelModelName>"
        '
        'LabelPrompt
        '
        Me.LabelPrompt.AutoSize = True
        Me.LabelPrompt.Location = New System.Drawing.Point(17, 25)
        Me.LabelPrompt.Name = "LabelPrompt"
        Me.LabelPrompt.Size = New System.Drawing.Size(39, 13)
        Me.LabelPrompt.TabIndex = 2
        Me.LabelPrompt.Text = "Model:"
        '
        'TreeView1
        '
        Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.ImageIndex = 0
        Me.TreeView1.ImageList = Me.ImageList
        Me.TreeView1.Location = New System.Drawing.Point(3, 49)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.SelectedImageIndex = 0
        Me.TreeView1.Size = New System.Drawing.Size(337, 401)
        Me.TreeView1.TabIndex = 1
        '
        'ImageList
        '
        Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList.Images.SetKeyName(0, "model_dictionary_entity_type")
        Me.ImageList.Images.SetKeyName(1, "model_dictionary_value_type")
        Me.ImageList.Images.SetKeyName(2, "model_dictionary_FactType")
        Me.ImageList.Images.SetKeyName(3, "ObjectifiedFactType")
        Me.ImageList.Images.SetKeyName(4, "ExclusionConstraint")
        Me.ImageList.Images.SetKeyName(5, "EqualityConstraint")
        Me.ImageList.Images.SetKeyName(6, "SubsetConstraint")
        Me.ImageList.Images.SetKeyName(7, "InclusiveOrConstraint")
        Me.ImageList.Images.SetKeyName(8, "ExclusiveOrConstraint")
        Me.ImageList.Images.SetKeyName(9, "FrequencyConstraint")
        Me.ImageList.Images.SetKeyName(10, "RingConstraint")
        Me.ImageList.Images.SetKeyName(11, "ExternalUniquenessConstraint")
        Me.ImageList.Images.SetKeyName(12, "ERD-16-16.png")
        Me.ImageList.Images.SetKeyName(13, "ERDEntity16x16.png")
        Me.ImageList.Images.SetKeyName(14, "Attribute.png")
        Me.ImageList.Images.SetKeyName(15, "EntityTypeDerived")
        Me.ImageList.Images.SetKeyName(16, "FactTypeUnary")
        Me.ImageList.Images.SetKeyName(17, "SubtypeRelationship")
        Me.ImageList.Images.SetKeyName(18, "FactTypeDerived")
        Me.ImageList.Images.SetKeyName(19, "FactTypeUnaryDerived")
        Me.ImageList.Images.SetKeyName(20, "PreferredExternalUniqueness")
        Me.ImageList.Images.SetKeyName(21, "PGSNode.png")
        Me.ImageList.Images.SetKeyName(22, "PGSRelation.png")
        Me.ImageList.Images.SetKeyName(23, "value_comparison.png")
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemViewOnPage, Me.ToolStripMenuItemViewInDiagramSpy, Me.ViewInGlossaryToolStripMenuItem, Me.ToolStripMenuItemRemoveFromModel, Me.ToolStripMenuItemMakeNewPageForThisModelElement, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(281, 158)
        '
        'ToolStripMenuItemViewOnPage
        '
        Me.ToolStripMenuItemViewOnPage.Name = "ToolStripMenuItemViewOnPage"
        Me.ToolStripMenuItemViewOnPage.Size = New System.Drawing.Size(280, 22)
        Me.ToolStripMenuItemViewOnPage.Text = "&View on Page..."
        '
        'ToolStripMenuItemViewInDiagramSpy
        '
        Me.ToolStripMenuItemViewInDiagramSpy.Image = Global.Boston.My.Resources.Resources.Spyglass16x16
        Me.ToolStripMenuItemViewInDiagramSpy.Name = "ToolStripMenuItemViewInDiagramSpy"
        Me.ToolStripMenuItemViewInDiagramSpy.Size = New System.Drawing.Size(280, 22)
        Me.ToolStripMenuItemViewInDiagramSpy.Text = "View in Diagram Spy"
        '
        'ToolStripMenuItemRemoveFromModel
        '
        Me.ToolStripMenuItemRemoveFromModel.Image = Global.Boston.My.Resources.MenuImages.Remove16x16
        Me.ToolStripMenuItemRemoveFromModel.Name = "ToolStripMenuItemRemoveFromModel"
        Me.ToolStripMenuItemRemoveFromModel.Size = New System.Drawing.Size(280, 22)
        Me.ToolStripMenuItemRemoveFromModel.Text = "&Remove From Model"
        '
        'ToolStripMenuItemMakeNewPageForThisModelElement
        '
        Me.ToolStripMenuItemMakeNewPageForThisModelElement.Name = "ToolStripMenuItemMakeNewPageForThisModelElement"
        Me.ToolStripMenuItemMakeNewPageForThisModelElement.Size = New System.Drawing.Size(280, 22)
        Me.ToolStripMenuItemMakeNewPageForThisModelElement.Text = "&Make new Page for this model element"
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(280, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelPromptRealisationsCount, Me.ToolStripStatusLabelRealisationsCount, Me.ToolStripStatusLabelPromptModelElementTypeCount, Me.ToolStripStatusLabelModelElementTypeCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 471)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(370, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabelPromptRealisationsCount
        '
        Me.ToolStripStatusLabelPromptRealisationsCount.Name = "ToolStripStatusLabelPromptRealisationsCount"
        Me.ToolStripStatusLabelPromptRealisationsCount.Size = New System.Drawing.Size(111, 17)
        Me.ToolStripStatusLabelPromptRealisationsCount.Text = "Realisations Count :"
        '
        'ToolStripStatusLabelRealisationsCount
        '
        Me.ToolStripStatusLabelRealisationsCount.Name = "ToolStripStatusLabelRealisationsCount"
        Me.ToolStripStatusLabelRealisationsCount.Size = New System.Drawing.Size(13, 17)
        Me.ToolStripStatusLabelRealisationsCount.Text = "0"
        '
        'ToolStripStatusLabelPromptModelElementTypeCount
        '
        Me.ToolStripStatusLabelPromptModelElementTypeCount.Name = "ToolStripStatusLabelPromptModelElementTypeCount"
        Me.ToolStripStatusLabelPromptModelElementTypeCount.Size = New System.Drawing.Size(156, 17)
        Me.ToolStripStatusLabelPromptModelElementTypeCount.Text = "Model Element Type Count :"
        '
        'ToolStripStatusLabelModelElementTypeCount
        '
        Me.ToolStripStatusLabelModelElementTypeCount.Name = "ToolStripStatusLabelModelElementTypeCount"
        Me.ToolStripStatusLabelModelElementTypeCount.Size = New System.Drawing.Size(13, 17)
        Me.ToolStripStatusLabelModelElementTypeCount.Text = "0"
        '
        'ViewInGlossaryToolStripMenuItem
        '
        Me.ViewInGlossaryToolStripMenuItem.Name = "ViewInGlossaryToolStripMenuItem"
        Me.ViewInGlossaryToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.ViewInGlossaryToolStripMenuItem.Text = "View in &Glossary"
        '
        'frmToolboxModelDictionary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(370, 493)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox_Main)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxModelDictionary"
        Me.TabText = "Model Dictionary"
        Me.Text = "Model Dictionary"
        Me.GroupBox_Main.ResumeLayout(False)
        Me.GroupBox_Main.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox_Main As System.Windows.Forms.GroupBox
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ImageList As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemViewOnPage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRemoveFromModel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelModelName As System.Windows.Forms.Label
    Friend WithEvents LabelPrompt As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabelPromptRealisationsCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelRealisationsCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelPromptModelElementTypeCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelModelElementTypeCount As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents CheckBoxShowCoreModelElements As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxShowModelDictionary As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonRefresh As Button
    Friend WithEvents ToolStripMenuItemViewInDiagramSpy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemMakeNewPageForThisModelElement As ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ViewInGlossaryToolStripMenuItem As ToolStripMenuItem
End Class
