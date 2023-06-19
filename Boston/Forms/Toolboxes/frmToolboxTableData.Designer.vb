<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxTableData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxTableData))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AdvancedDataGridView = New ADGV.AdvancedDataGridView()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonCommit = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonUndo = New System.Windows.Forms.ToolStripButton()
        Me.ButtonAddRow = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonCSVImport = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonCSVExport = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1.SuspendLayout()
        CType(Me.AdvancedDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.AdvancedDataGridView)
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
        'AdvancedDataGridView
        '
        Me.AdvancedDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AdvancedDataGridView.AutoGenerateContextFilters = True
        Me.AdvancedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AdvancedDataGridView.DateWithTime = False
        Me.AdvancedDataGridView.Location = New System.Drawing.Point(2, 38)
        Me.AdvancedDataGridView.Name = "AdvancedDataGridView"
        Me.AdvancedDataGridView.Size = New System.Drawing.Size(813, 278)
        Me.AdvancedDataGridView.TabIndex = 3
        Me.AdvancedDataGridView.TimeFilter = False
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
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonCommit, Me.ToolStripButtonUndo, Me.ButtonAddRow, Me.ToolStripSeparator1, Me.ToolStripButtonCSVImport, Me.ToolStripButtonCSVExport})
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
        CType(Me.AdvancedDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
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
End Class
