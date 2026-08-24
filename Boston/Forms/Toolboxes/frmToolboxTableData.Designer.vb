<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmToolboxTableData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxTableData))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.RichTextBoxQuery = New System.Windows.Forms.RichTextBox()
        Me.AdvancedDataGridView = New ADGV.AdvancedDataGridView()
        Me.ContextMenuStripGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemEditInForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyWithHeaderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonGO = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonCommit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonUndo = New System.Windows.Forms.ToolStripButton()
        Me.ButtonAddRow = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonCSVImport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonCSVExport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonEditViaDynamicForm = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonSQL = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabelPromptDatabase = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBoxDatabase = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripLabelPromptTable = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBoxTable = New System.Windows.Forms.ToolStripComboBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1.SuspendLayout()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.AdvancedDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripGrid.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanelMain)
        Me.GroupBox1.Controls.Add(Me.StatusStrip1)
        Me.GroupBox1.Controls.Add(Me.ToolStrip1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 8)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox1.Size = New System.Drawing.Size(819, 343)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "TableName"
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 1
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(2, 40)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 1
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.27599!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(815, 279)
        Me.TableLayoutPanelMain.TabIndex = 4
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.SplitContainer1, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(809, 273)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanel2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.AdvancedDataGridView)
        Me.SplitContainer1.Size = New System.Drawing.Size(803, 267)
        Me.SplitContainer1.SplitterDistance = 135
        Me.SplitContainer1.TabIndex = 1
        '
        'RichTextBoxQuery
        '
        Me.RichTextBoxQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RichTextBoxQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxQuery.Location = New System.Drawing.Point(3, 29)
        Me.RichTextBoxQuery.Name = "RichTextBoxQuery"
        Me.RichTextBoxQuery.Size = New System.Drawing.Size(797, 103)
        Me.RichTextBoxQuery.TabIndex = 4
        Me.RichTextBoxQuery.Text = ""
        '
        'AdvancedDataGridView
        '
        Me.AdvancedDataGridView.AutoGenerateContextFilters = True
        Me.AdvancedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AdvancedDataGridView.ContextMenuStrip = Me.ContextMenuStripGrid
        Me.AdvancedDataGridView.DateWithTime = False
        Me.AdvancedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AdvancedDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.AdvancedDataGridView.Name = "AdvancedDataGridView"
        Me.AdvancedDataGridView.Size = New System.Drawing.Size(803, 128)
        Me.AdvancedDataGridView.TabIndex = 3
        Me.AdvancedDataGridView.TimeFilter = False
        '
        'ContextMenuStripGrid
        '
        Me.ContextMenuStripGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemEditInForm, Me.CopyToolStripMenuItem, Me.CopyWithHeaderToolStripMenuItem})
        Me.ContextMenuStripGrid.Name = "ContextMenuStripCRUD"
        Me.ContextMenuStripGrid.Size = New System.Drawing.Size(170, 70)
        '
        'ToolStripMenuItemEditInForm
        '
        Me.ToolStripMenuItemEditInForm.Name = "ToolStripMenuItemEditInForm"
        Me.ToolStripMenuItemEditInForm.Size = New System.Drawing.Size(169, 22)
        Me.ToolStripMenuItemEditInForm.Text = "&Edit in Form"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'CopyWithHeaderToolStripMenuItem
        '
        Me.CopyWithHeaderToolStripMenuItem.Name = "CopyWithHeaderToolStripMenuItem"
        Me.CopyWithHeaderToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.CopyWithHeaderToolStripMenuItem.Text = "Copy with &Header"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonGO})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(803, 25)
        Me.ToolStrip2.TabIndex = 5
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripButtonGO
        '
        Me.ToolStripButtonGO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonGO.Image = Global.Boston.My.Resources.Resources.GO16x16
        Me.ToolStripButtonGO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonGO.Name = "ToolStripButtonGO"
        Me.ToolStripButtonGO.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonGO.Text = "ToolStripButton2"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(2, 319)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(815, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(113, 17)
        Me.ToolStripStatusLabel.Text = "ToolStripStatusLabel"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonCommit, Me.ToolStripButtonUndo, Me.ButtonAddRow, Me.ToolStripSeparator1, Me.ToolStripButtonCSVImport, Me.ToolStripButtonCSVExport, Me.ToolStripButtonEditViaDynamicForm, Me.ToolStripButtonSQL, Me.ToolStripSeparator2, Me.ToolStripLabelPromptDatabase, Me.ToolStripComboBoxDatabase, Me.ToolStripLabelPromptTable, Me.ToolStripComboBoxTable})
        Me.ToolStrip1.Location = New System.Drawing.Point(2, 15)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(815, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonCommit
        '
        Me.ToolStripButtonCommit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonCommit.Enabled = False
        Me.ToolStripButtonCommit.Image = CType(resources.GetObject("ToolStripButtonCommit.Image"), System.Drawing.Image)
        Me.ToolStripButtonCommit.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonCommit.Name = "ToolStripButtonCommit"
        Me.ToolStripButtonCommit.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonCommit.Text = "ToolStripButton1"
        '
        'ToolStripButtonUndo
        '
        Me.ToolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonUndo.Enabled = False
        Me.ToolStripButtonUndo.Image = Global.Boston.My.Resources.Resources.Undo16x16
        Me.ToolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonUndo.Name = "ToolStripButtonUndo"
        Me.ToolStripButtonUndo.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonUndo.Text = "ToolStripButton1"
        '
        'ButtonAddRow
        '
        Me.ButtonAddRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ButtonAddRow.Enabled = False
        Me.ButtonAddRow.Image = Global.Boston.My.Resources.Resources.AddAttribute16x16
        Me.ButtonAddRow.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ButtonAddRow.Name = "ButtonAddRow"
        Me.ButtonAddRow.Size = New System.Drawing.Size(23, 22)
        Me.ButtonAddRow.Text = "ToolStripButton1"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButtonCSVImport
        '
        Me.ToolStripButtonCSVImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonCSVImport.Image = CType(resources.GetObject("ToolStripButtonCSVImport.Image"), System.Drawing.Image)
        Me.ToolStripButtonCSVImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonCSVImport.Name = "ToolStripButtonCSVImport"
        Me.ToolStripButtonCSVImport.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonCSVImport.Text = "ToolStripButton1"
        Me.ToolStripButtonCSVImport.ToolTipText = "Import CSV Data"
        '
        'ToolStripButtonCSVExport
        '
        Me.ToolStripButtonCSVExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonCSVExport.Image = CType(resources.GetObject("ToolStripButtonCSVExport.Image"), System.Drawing.Image)
        Me.ToolStripButtonCSVExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonCSVExport.Name = "ToolStripButtonCSVExport"
        Me.ToolStripButtonCSVExport.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonCSVExport.Text = "ToolStripButton2"
        Me.ToolStripButtonCSVExport.ToolTipText = "Export CSV Data"
        '
        'ToolStripButtonEditViaDynamicForm
        '
        Me.ToolStripButtonEditViaDynamicForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonEditViaDynamicForm.Image = Global.Boston.My.Resources.Resources.Form16x16
        Me.ToolStripButtonEditViaDynamicForm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonEditViaDynamicForm.Name = "ToolStripButtonEditViaDynamicForm"
        Me.ToolStripButtonEditViaDynamicForm.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonEditViaDynamicForm.Text = "ToolStripButton1"
        '
        'ToolStripButtonSQL
        '
        Me.ToolStripButtonSQL.CheckOnClick = True
        Me.ToolStripButtonSQL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonSQL.Image = Global.Boston.My.Resources.Resources.sql
        Me.ToolStripButtonSQL.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonSQL.Name = "ToolStripButtonSQL"
        Me.ToolStripButtonSQL.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButtonSQL.Text = "ToolStripButton1"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripLabelPromptDatabase
        '
        Me.ToolStripLabelPromptDatabase.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToolStripLabelPromptDatabase.Name = "ToolStripLabelPromptDatabase"
        Me.ToolStripLabelPromptDatabase.Size = New System.Drawing.Size(58, 22)
        Me.ToolStripLabelPromptDatabase.Text = "Database:"
        '
        'ToolStripComboBoxDatabase
        '
        Me.ToolStripComboBoxDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBoxDatabase.Name = "ToolStripComboBoxDatabase"
        Me.ToolStripComboBoxDatabase.Size = New System.Drawing.Size(250, 25)
        '
        'ToolStripLabelPromptTable
        '
        Me.ToolStripLabelPromptTable.ForeColor = System.Drawing.Color.SteelBlue
        Me.ToolStripLabelPromptTable.Name = "ToolStripLabelPromptTable"
        Me.ToolStripLabelPromptTable.Size = New System.Drawing.Size(37, 22)
        Me.ToolStripLabelPromptTable.Text = "Table:"
        '
        'ToolStripComboBoxTable
        '
        Me.ToolStripComboBoxTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBoxTable.Name = "ToolStripComboBoxTable"
        Me.ToolStripComboBoxTable.Size = New System.Drawing.Size(200, 25)
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.ToolStrip2, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.RichTextBoxQuery, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(803, 135)
        Me.TableLayoutPanel2.TabIndex = 6
        '
        'frmToolboxTableData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(835, 358)
        Me.Controls.Add(Me.GroupBox1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmToolboxTableData"
        Me.Text = "Database Table Data"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.AdvancedDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripGrid.ResumeLayout(False)
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButtonCommit As ToolStripButton
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripButtonUndo As ToolStripButton
    Friend WithEvents AdvancedDataGridView As ADGV.AdvancedDataGridView
    Friend WithEvents ButtonAddRow As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripButtonCSVImport As ToolStripButton
    Friend WithEvents ToolStripButtonCSVExport As ToolStripButton
    Friend WithEvents ContextMenuStripGrid As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemEditInForm As ToolStripMenuItem
    Friend WithEvents ToolStripButtonEditViaDynamicForm As ToolStripButton
    Friend WithEvents ToolStripButtonSQL As ToolStripButton
    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents RichTextBoxQuery As RichTextBox
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents ToolStripButtonGO As ToolStripButton
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripLabelPromptDatabase As ToolStripLabel
    Friend WithEvents ToolStripComboBoxDatabase As ToolStripComboBox
    Friend WithEvents ToolStripLabelPromptTable As ToolStripLabel
    Friend WithEvents ToolStripComboBoxTable As ToolStripComboBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyWithHeaderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
End Class
