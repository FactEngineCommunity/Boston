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
        Me.ButtonDescribeUnifiedOntology = New System.Windows.Forms.Button()
        Me.LabelOntologyName = New System.Windows.Forms.Label()
        Me.LabelPromptOntology = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.ContextMenuStripModelElementPages = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemViewOnPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonSearch = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.ContextMenuStripModelElementPages.SuspendLayout()
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonSearch)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonDescribeUnifiedOntology)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelOntologyName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPromptOntology)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(884, 574)
        Me.SplitContainer1.SplitterDistance = 289
        Me.SplitContainer1.TabIndex = 0
        '
        'ButtonDescribeUnifiedOntology
        '
        Me.ButtonDescribeUnifiedOntology.Location = New System.Drawing.Point(238, 12)
        Me.ButtonDescribeUnifiedOntology.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDescribeUnifiedOntology.Name = "ButtonDescribeUnifiedOntology"
        Me.ButtonDescribeUnifiedOntology.Size = New System.Drawing.Size(36, 21)
        Me.ButtonDescribeUnifiedOntology.TabIndex = 4
        Me.ButtonDescribeUnifiedOntology.Text = "..."
        Me.ButtonDescribeUnifiedOntology.UseVisualStyleBackColor = True
        '
        'LabelOntologyName
        '
        Me.LabelOntologyName.AutoSize = True
        Me.LabelOntologyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOntologyName.Location = New System.Drawing.Point(98, 16)
        Me.LabelOntologyName.Name = "LabelOntologyName"
        Me.LabelOntologyName.Size = New System.Drawing.Size(120, 13)
        Me.LabelOntologyName.TabIndex = 3
        Me.LabelOntologyName.Text = "LabelOntologyName"
        '
        'LabelPromptOntology
        '
        Me.LabelPromptOntology.AutoSize = True
        Me.LabelPromptOntology.Location = New System.Drawing.Point(12, 16)
        Me.LabelPromptOntology.Name = "LabelPromptOntology"
        Me.LabelPromptOntology.Size = New System.Drawing.Size(88, 13)
        Me.LabelPromptOntology.TabIndex = 2
        Me.LabelPromptOntology.Text = "Unified Ontology:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(12, 43)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(195, 20)
        Me.TextBox1.TabIndex = 1
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(12, 69)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(262, 498)
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.WebBrowser)
        Me.SplitContainer2.Size = New System.Drawing.Size(591, 574)
        Me.SplitContainer2.SplitterDistance = 358
        Me.SplitContainer2.TabIndex = 0
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
        'ContextMenuStripModelElementPages
        '
        Me.ContextMenuStripModelElementPages.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemViewOnPage})
        Me.ContextMenuStripModelElementPages.Name = "ContextMenuStripModelElementPages"
        Me.ContextMenuStripModelElementPages.Size = New System.Drawing.Size(155, 26)
        '
        'ToolStripMenuItemViewOnPage
        '
        Me.ToolStripMenuItemViewOnPage.Name = "ToolStripMenuItemViewOnPage"
        Me.ToolStripMenuItemViewOnPage.Size = New System.Drawing.Size(154, 22)
        Me.ToolStripMenuItemViewOnPage.Text = "View on &Page..."
        '
        'ButtonSearch
        '
        Me.ButtonSearch.Location = New System.Drawing.Point(214, 43)
        Me.ButtonSearch.Name = "ButtonSearch"
        Me.ButtonSearch.Size = New System.Drawing.Size(60, 20)
        Me.ButtonSearch.TabIndex = 5
        Me.ButtonSearch.Text = "&Search"
        Me.ButtonSearch.UseVisualStyleBackColor = True
        '
        'frmUnifiedOntologyBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 574)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmUnifiedOntologyBrowser"
        Me.TabText = "Unified Ontology Browser"
        Me.Text = "Unified Ontology Browser"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ContextMenuStripModelElementPages.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents LabelOntologyName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptOntology As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStripModelElementPages As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemViewOnPage As ToolStripMenuItem
    Friend WithEvents ButtonDescribeUnifiedOntology As Button
    Friend WithEvents ButtonSearch As Button
End Class
