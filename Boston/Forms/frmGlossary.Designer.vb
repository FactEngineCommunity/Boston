<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGlossary
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonGenerateHTMLGlossary = New System.Windows.Forms.Button()
        Me.CheckBoxShowGeneralConcepts = New System.Windows.Forms.CheckBox()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.ListBoxGlossary = New System.Windows.Forms.ListBox()
        Me.ContextMenuStripMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemViewOnPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemViewInDiagramSpy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowInModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRemoveFromModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CopyToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView = New System.Windows.Forms.CheckBox()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.TextboxSearch = New SearchTextbox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStripMain.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextboxSearch)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonRefresh)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonGenerateHTMLGlossary)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBoxShowGeneralConcepts)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelModelName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPromptModel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBoxGlossary)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(884, 574)
        Me.SplitContainer1.SplitterDistance = 289
        Me.SplitContainer1.TabIndex = 0
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(213, 43)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(24, 23)
        Me.ButtonRefresh.TabIndex = 6
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'ButtonGenerateHTMLGlossary
        '
        Me.ButtonGenerateHTMLGlossary.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonGenerateHTMLGlossary.Location = New System.Drawing.Point(12, 539)
        Me.ButtonGenerateHTMLGlossary.Name = "ButtonGenerateHTMLGlossary"
        Me.ButtonGenerateHTMLGlossary.Size = New System.Drawing.Size(141, 23)
        Me.ButtonGenerateHTMLGlossary.TabIndex = 5
        Me.ButtonGenerateHTMLGlossary.Text = "Generate &HTML Glossary"
        Me.ButtonGenerateHTMLGlossary.UseVisualStyleBackColor = True
        '
        'CheckBoxShowGeneralConcepts
        '
        Me.CheckBoxShowGeneralConcepts.AutoSize = True
        Me.CheckBoxShowGeneralConcepts.Location = New System.Drawing.Point(12, 69)
        Me.CheckBoxShowGeneralConcepts.Name = "CheckBoxShowGeneralConcepts"
        Me.CheckBoxShowGeneralConcepts.Size = New System.Drawing.Size(141, 17)
        Me.CheckBoxShowGeneralConcepts.TabIndex = 4
        Me.CheckBoxShowGeneralConcepts.Text = "Show &General Concepts"
        Me.CheckBoxShowGeneralConcepts.UseVisualStyleBackColor = True
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelModelName.Location = New System.Drawing.Point(57, 16)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(104, 13)
        Me.LabelModelName.TabIndex = 3
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(12, 16)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 2
        Me.LabelPromptModel.Text = "Model:"
        '
        'ListBoxGlossary
        '
        Me.ListBoxGlossary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxGlossary.ContextMenuStrip = Me.ContextMenuStripMain
        Me.ListBoxGlossary.FormattingEnabled = True
        Me.ListBoxGlossary.Location = New System.Drawing.Point(12, 95)
        Me.ListBoxGlossary.Name = "ListBoxGlossary"
        Me.ListBoxGlossary.Size = New System.Drawing.Size(262, 433)
        Me.ListBoxGlossary.Sorted = True
        Me.ListBoxGlossary.TabIndex = 0
        '
        'ContextMenuStripMain
        '
        Me.ContextMenuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemViewOnPage, Me.ToolStripMenuItemViewInDiagramSpy, Me.ShowInModelDictionaryToolStripMenuItem, Me.ToolStripSeparator1, Me.ToolStripMenuItemRemoveFromModel, Me.ToolStripSeparator2, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStripMain.Name = "ContextMenuStrip1"
        Me.ContextMenuStripMain.Size = New System.Drawing.Size(211, 126)
        '
        'ToolStripMenuItemViewOnPage
        '
        Me.ToolStripMenuItemViewOnPage.Name = "ToolStripMenuItemViewOnPage"
        Me.ToolStripMenuItemViewOnPage.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemViewOnPage.Text = "&View on Page..."
        '
        'ToolStripMenuItemViewInDiagramSpy
        '
        Me.ToolStripMenuItemViewInDiagramSpy.Image = Global.Boston.My.Resources.MenuImages.Spyglass16x16
        Me.ToolStripMenuItemViewInDiagramSpy.Name = "ToolStripMenuItemViewInDiagramSpy"
        Me.ToolStripMenuItemViewInDiagramSpy.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemViewInDiagramSpy.Text = "View in Diagram Spy"
        '
        'ShowInModelDictionaryToolStripMenuItem
        '
        Me.ShowInModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem.Name = "ShowInModelDictionaryToolStripMenuItem"
        Me.ShowInModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.ShowInModelDictionaryToolStripMenuItem.Text = "Show in Model &Dictionary"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItemRemoveFromModel
        '
        Me.ToolStripMenuItemRemoveFromModel.Image = Global.Boston.My.Resources.MenuImages.Remove16x16
        Me.ToolStripMenuItemRemoveFromModel.Name = "ToolStripMenuItemRemoveFromModel"
        Me.ToolStripMenuItemRemoveFromModel.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemRemoveFromModel.Text = "&Remove From Model"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(207, 6)
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(210, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.StatusStrip1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.CheckBoxHideFadedFactTypeNamesVerbalisationView)
        Me.SplitContainer2.Panel1.Controls.Add(Me.WebBrowser)
        Me.SplitContainer2.Size = New System.Drawing.Size(591, 574)
        Me.SplitContainer2.SplitterDistance = 358
        Me.SplitContainer2.TabIndex = 0
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 336)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(591, 22)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToClipboardToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = Global.Boston.My.Resources.Resources.Copy16x16
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(29, 20)
        Me.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1"
        '
        'CopyToClipboardToolStripMenuItem
        '
        Me.CopyToClipboardToolStripMenuItem.Name = "CopyToClipboardToolStripMenuItem"
        Me.CopyToClipboardToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.CopyToClipboardToolStripMenuItem.Text = "&Copy to Clipboard"
        '
        'CheckBoxHideFadedFactTypeNamesVerbalisationView
        '
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.AutoSize = True
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.BackColor = System.Drawing.Color.Transparent
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.ForeColor = System.Drawing.Color.DarkGray
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Location = New System.Drawing.Point(420, 316)
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Name = "CheckBoxHideFadedFactTypeNamesVerbalisationView"
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Size = New System.Drawing.Size(168, 17)
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.TabIndex = 7
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.Text = "Hide Faded Fact Type Names"
        Me.CheckBoxHideFadedFactTypeNamesVerbalisationView.UseVisualStyleBackColor = False
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(591, 358)
        Me.WebBrowser.TabIndex = 2
        '
        'TextboxSearch
        '
        Me.TextboxSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextboxSearch.Location = New System.Drawing.Point(12, 43)
        Me.TextboxSearch.Name = "TextboxSearch"
        Me.TextboxSearch.Size = New System.Drawing.Size(195, 26)
        Me.TextboxSearch.TabIndex = 10
        '
        'frmGlossary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 574)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmGlossary"
        Me.TabText = "Glossary"
        Me.Text = "Glossary"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStripMain.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ListBoxGlossary As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents LabelModelName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptModel As System.Windows.Forms.Label
    Friend WithEvents CheckBoxShowGeneralConcepts As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonGenerateHTMLGlossary As Button
    Friend WithEvents ContextMenuStripMain As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemViewOnPage As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemViewInDiagramSpy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRemoveFromModel As ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ButtonRefresh As Button
    Friend WithEvents CheckBoxHideFadedFactTypeNamesVerbalisationView As CheckBox
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents CopyToClipboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TextboxSearch As SearchTextbox
End Class
