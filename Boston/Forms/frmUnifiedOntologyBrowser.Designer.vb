<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUnifiedOntologyBrowser
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.RadioButtonShadeDuplicates = New System.Windows.Forms.RadioButton()
        Me.SearchTextbox = New CustomSearchTextbox()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.MenuStripMain = New System.Windows.Forms.MenuStrip()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComboBoxModel = New System.Windows.Forms.ComboBox()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonDescribeUnifiedOntology = New System.Windows.Forms.Button()
        Me.LabelOntologyName = New System.Windows.Forms.Label()
        Me.LabelPromptOntology = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.MenuStripModelElement = New System.Windows.Forms.MenuStrip()
        Me.ModelElementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCopyModelElementToModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CopyToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripDropDownButton2 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ContextMenuStripModelElementPages = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemViewOnPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCopyToModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataLineageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelModel = New System.Windows.Forms.ToolStripStatusLabel()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.MenuStripMain.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.MenuStripModelElement.SuspendLayout()
        Me.StatusStrip2.SuspendLayout()
        Me.ContextMenuStripModelElementPages.SuspendLayout()
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.RadioButtonShadeDuplicates)
        Me.SplitContainer1.Panel1.Controls.Add(Me.SearchTextbox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPromptModel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.MenuStripMain)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ComboBoxModel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonRefresh)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonDescribeUnifiedOntology)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelOntologyName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPromptOntology)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(884, 574)
        Me.SplitContainer1.SplitterDistance = 289
        Me.SplitContainer1.TabIndex = 0
        '
        'RadioButtonShadeDuplicates
        '
        Me.RadioButtonShadeDuplicates.AutoCheck = False
        Me.RadioButtonShadeDuplicates.AutoSize = True
        Me.RadioButtonShadeDuplicates.Location = New System.Drawing.Point(12, 87)
        Me.RadioButtonShadeDuplicates.Name = "RadioButtonShadeDuplicates"
        Me.RadioButtonShadeDuplicates.Size = New System.Drawing.Size(109, 17)
        Me.RadioButtonShadeDuplicates.TabIndex = 12
        Me.RadioButtonShadeDuplicates.TabStop = True
        Me.RadioButtonShadeDuplicates.Text = "Shade &Duplicates"
        Me.RadioButtonShadeDuplicates.UseVisualStyleBackColor = True
        '
        'SearchTextbox
        '
        Me.SearchTextbox.Location = New System.Drawing.Point(5, 108)
        Me.SearchTextbox.Name = "SearchTextbox"
        Me.SearchTextbox.Size = New System.Drawing.Size(241, 26)
        Me.SearchTextbox.TabIndex = 11
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(12, 60)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 10
        Me.LabelPromptModel.Text = "Model:"
        '
        'MenuStripMain
        '
        Me.MenuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem})
        Me.MenuStripMain.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripMain.Name = "MenuStripMain"
        Me.MenuStripMain.Size = New System.Drawing.Size(289, 24)
        Me.MenuStripMain.TabIndex = 13
        Me.MenuStripMain.Text = "MenuStrip2"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.CloseToolStripMenuItem.Text = "&Close"
        '
        'ComboBoxModel
        '
        Me.ComboBoxModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxModel.FormattingEnabled = True
        Me.ComboBoxModel.Location = New System.Drawing.Point(54, 55)
        Me.ComboBoxModel.Name = "ComboBoxModel"
        Me.ComboBoxModel.Size = New System.Drawing.Size(220, 24)
        Me.ComboBoxModel.TabIndex = 8
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.MenuImages.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(250, 111)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(24, 23)
        Me.ButtonRefresh.TabIndex = 7
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'ButtonDescribeUnifiedOntology
        '
        Me.ButtonDescribeUnifiedOntology.Location = New System.Drawing.Point(250, 25)
        Me.ButtonDescribeUnifiedOntology.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDescribeUnifiedOntology.Name = "ButtonDescribeUnifiedOntology"
        Me.ButtonDescribeUnifiedOntology.Size = New System.Drawing.Size(24, 24)
        Me.ButtonDescribeUnifiedOntology.TabIndex = 1
        Me.ButtonDescribeUnifiedOntology.Text = "..."
        Me.ButtonDescribeUnifiedOntology.UseVisualStyleBackColor = True
        '
        'LabelOntologyName
        '
        Me.LabelOntologyName.AutoSize = True
        Me.LabelOntologyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOntologyName.Location = New System.Drawing.Point(98, 32)
        Me.LabelOntologyName.Name = "LabelOntologyName"
        Me.LabelOntologyName.Size = New System.Drawing.Size(120, 13)
        Me.LabelOntologyName.TabIndex = 3
        Me.LabelOntologyName.Text = "LabelOntologyName"
        '
        'LabelPromptOntology
        '
        Me.LabelPromptOntology.AutoSize = True
        Me.LabelPromptOntology.Location = New System.Drawing.Point(12, 32)
        Me.LabelPromptOntology.Name = "LabelPromptOntology"
        Me.LabelPromptOntology.Size = New System.Drawing.Size(88, 13)
        Me.LabelPromptOntology.TabIndex = 2
        Me.LabelPromptOntology.Text = "Unified Ontology:"
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(12, 134)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(262, 433)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 0
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.StatusStrip2)
        Me.SplitContainer2.Size = New System.Drawing.Size(591, 574)
        Me.SplitContainer2.SplitterDistance = 358
        Me.SplitContainer2.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.WebBrowser, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MenuStripModelElement, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(591, 336)
        Me.TableLayoutPanel1.TabIndex = 11
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(3, 27)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(585, 358)
        Me.WebBrowser.TabIndex = 2
        '
        'MenuStripModelElement
        '
        Me.MenuStripModelElement.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModelElementToolStripMenuItem})
        Me.MenuStripModelElement.Location = New System.Drawing.Point(0, 0)
        Me.MenuStripModelElement.Name = "MenuStripModelElement"
        Me.MenuStripModelElement.Size = New System.Drawing.Size(591, 24)
        Me.MenuStripModelElement.TabIndex = 10
        Me.MenuStripModelElement.Text = "MenuStrip1"
        '
        'ModelElementToolStripMenuItem
        '
        Me.ModelElementToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemCopyModelElementToModel})
        Me.ModelElementToolStripMenuItem.Name = "ModelElementToolStripMenuItem"
        Me.ModelElementToolStripMenuItem.Size = New System.Drawing.Size(99, 20)
        Me.ModelElementToolStripMenuItem.Text = "Model &Element"
        '
        'ToolStripMenuItemCopyModelElementToModel
        '
        Me.ToolStripMenuItemCopyModelElementToModel.Image = Global.Boston.My.Resources.Resources.Copy16x16
        Me.ToolStripMenuItemCopyModelElementToModel.Name = "ToolStripMenuItemCopyModelElementToModel"
        Me.ToolStripMenuItemCopyModelElementToModel.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItemCopyModelElementToModel.Text = "&Copy to Model..."
        '
        'StatusStrip2
        '
        Me.StatusStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButton1, Me.ToolStripDropDownButton2})
        Me.StatusStrip2.Location = New System.Drawing.Point(0, 336)
        Me.StatusStrip2.Name = "StatusStrip2"
        Me.StatusStrip2.Size = New System.Drawing.Size(591, 22)
        Me.StatusStrip2.TabIndex = 9
        Me.StatusStrip2.Text = "StatusStrip2"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToClipboardToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = Global.Boston.My.Resources.MenuImages.Copy16x16
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
        'ToolStripDropDownButton2
        '
        Me.ToolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripDropDownButton2.Image = Global.Boston.My.Resources.MenuImages.Expand16x16
        Me.ToolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton2.Name = "ToolStripDropDownButton2"
        Me.ToolStripDropDownButton2.Size = New System.Drawing.Size(29, 20)
        Me.ToolStripDropDownButton2.Text = "ToolStripDropDownButton2"
        '
        'ContextMenuStripModelElementPages
        '
        Me.ContextMenuStripModelElementPages.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemViewOnPage, Me.ToolStripMenuItemCopyToModel, Me.DataLineageToolStripMenuItem})
        Me.ContextMenuStripModelElementPages.Name = "ContextMenuStripModelElementPages"
        Me.ContextMenuStripModelElementPages.Size = New System.Drawing.Size(163, 70)
        '
        'ToolStripMenuItemViewOnPage
        '
        Me.ToolStripMenuItemViewOnPage.Name = "ToolStripMenuItemViewOnPage"
        Me.ToolStripMenuItemViewOnPage.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItemViewOnPage.Text = "View on &Page..."
        '
        'ToolStripMenuItemCopyToModel
        '
        Me.ToolStripMenuItemCopyToModel.Image = Global.Boston.My.Resources.Resources.Copy16x16
        Me.ToolStripMenuItemCopyToModel.Name = "ToolStripMenuItemCopyToModel"
        Me.ToolStripMenuItemCopyToModel.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItemCopyToModel.Text = "&Copy to Model..."
        '
        'DataLineageToolStripMenuItem
        '
        Me.DataLineageToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Lineage16x16
        Me.DataLineageToolStripMenuItem.Name = "DataLineageToolStripMenuItem"
        Me.DataLineageToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.DataLineageToolStripMenuItem.Text = "&Lineage"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelModel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 552)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(884, 22)
        Me.StatusStrip1.TabIndex = 1
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabelModel
        '
        Me.ToolStripStatusLabelModel.Name = "ToolStripStatusLabelModel"
        Me.ToolStripStatusLabelModel.Size = New System.Drawing.Size(147, 17)
        Me.ToolStripStatusLabelModel.Text = "ToolStripStatusLabelModel"
        Me.ToolStripStatusLabelModel.Visible = False
        '
        'frmUnifiedOntologyBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 574)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.MainMenuStrip = Me.MenuStripModelElement
        Me.Name = "frmUnifiedOntologyBrowser"
        Me.TabText = "Unified Ontology Browser"
        Me.Text = "Unified Ontology Browser"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.MenuStripMain.ResumeLayout(False)
        Me.MenuStripMain.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.MenuStripModelElement.ResumeLayout(False)
        Me.MenuStripModelElement.PerformLayout()
        Me.StatusStrip2.ResumeLayout(False)
        Me.StatusStrip2.PerformLayout()
        Me.ContextMenuStripModelElementPages.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents LabelOntologyName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptOntology As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStripModelElementPages As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemViewOnPage As ToolStripMenuItem
    Friend WithEvents ButtonDescribeUnifiedOntology As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabelModel As ToolStripStatusLabel
    Friend WithEvents DataLineageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ButtonRefresh As Button
    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents ComboBoxModel As ComboBox
    Friend WithEvents ToolStripMenuItemCopyToModel As ToolStripMenuItem
    Friend WithEvents StatusStrip2 As StatusStrip
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents CopyToClipboardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripDropDownButton2 As ToolStripDropDownButton
    Friend WithEvents MenuStripModelElement As MenuStrip
    Friend WithEvents ModelElementToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemCopyModelElementToModel As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents SearchTextbox As CustomSearchTextbox
    Friend WithEvents RadioButtonShadeDuplicates As RadioButton
    Friend WithEvents MenuStripMain As MenuStrip
    Friend WithEvents CloseToolStripMenuItem As ToolStripMenuItem
End Class
