<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDIndexManager
    Inherits System.Windows.Forms.Form

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
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.ButtonRevert = New System.Windows.Forms.Button()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.GroupBoxIndexes = New System.Windows.Forms.GroupBox()
        Me.ButtonAddConstraint = New System.Windows.Forms.Button()
        Me.GroupBoxIndexColumns = New System.Windows.Forms.GroupBox()
        Me.DataGridViewColumns = New System.Windows.Forms.DataGridView()
        Me.DataGridViewIndexes = New System.Windows.Forms.DataGridView()
        Me.LabelPromotModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelPromptTableNodeTypeName = New System.Windows.Forms.Label()
        Me.LabelPromotTableNodeType = New System.Windows.Forms.Label()
        Me.GroupBox.SuspendLayout()
        Me.GroupBoxIndexes.SuspendLayout()
        Me.GroupBoxIndexColumns.SuspendLayout()
        CType(Me.DataGridViewColumns, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewIndexes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.ButtonRevert)
        Me.GroupBox.Controls.Add(Me.ButtonApply)
        Me.GroupBox.Controls.Add(Me.GroupBoxIndexes)
        Me.GroupBox.Controls.Add(Me.LabelPromotModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModel)
        Me.GroupBox.Controls.Add(Me.LabelPromptTableNodeTypeName)
        Me.GroupBox.Controls.Add(Me.LabelPromotTableNodeType)
        Me.GroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(800, 450)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'ButtonRevert
        '
        Me.ButtonRevert.Enabled = False
        Me.ButtonRevert.Location = New System.Drawing.Point(701, 19)
        Me.ButtonRevert.Name = "ButtonRevert"
        Me.ButtonRevert.Size = New System.Drawing.Size(75, 23)
        Me.ButtonRevert.TabIndex = 6
        Me.ButtonRevert.Text = "&Revert"
        Me.ButtonRevert.UseVisualStyleBackColor = True
        '
        'ButtonApply
        '
        Me.ButtonApply.Enabled = False
        Me.ButtonApply.Location = New System.Drawing.Point(627, 19)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.Size = New System.Drawing.Size(68, 23)
        Me.ButtonApply.TabIndex = 5
        Me.ButtonApply.Text = "&Apply"
        Me.ButtonApply.UseVisualStyleBackColor = True
        '
        'GroupBoxIndexes
        '
        Me.GroupBoxIndexes.Controls.Add(Me.ButtonAddConstraint)
        Me.GroupBoxIndexes.Controls.Add(Me.GroupBoxIndexColumns)
        Me.GroupBoxIndexes.Controls.Add(Me.DataGridViewIndexes)
        Me.GroupBoxIndexes.Location = New System.Drawing.Point(12, 63)
        Me.GroupBoxIndexes.Name = "GroupBoxIndexes"
        Me.GroupBoxIndexes.Size = New System.Drawing.Size(776, 375)
        Me.GroupBoxIndexes.TabIndex = 4
        Me.GroupBoxIndexes.TabStop = False
        Me.GroupBoxIndexes.Text = "Indexes:"
        '
        'ButtonAddConstraint
        '
        Me.ButtonAddConstraint.Location = New System.Drawing.Point(7, 19)
        Me.ButtonAddConstraint.Name = "ButtonAddConstraint"
        Me.ButtonAddConstraint.Size = New System.Drawing.Size(123, 22)
        Me.ButtonAddConstraint.TabIndex = 2
        Me.ButtonAddConstraint.Text = "&Add Constraint/Index"
        Me.ButtonAddConstraint.UseVisualStyleBackColor = True
        '
        'GroupBoxIndexColumns
        '
        Me.GroupBoxIndexColumns.Controls.Add(Me.DataGridViewColumns)
        Me.GroupBoxIndexColumns.Location = New System.Drawing.Point(285, 45)
        Me.GroupBoxIndexColumns.Name = "GroupBoxIndexColumns"
        Me.GroupBoxIndexColumns.Size = New System.Drawing.Size(485, 324)
        Me.GroupBoxIndexColumns.TabIndex = 1
        Me.GroupBoxIndexColumns.TabStop = False
        Me.GroupBoxIndexColumns.Text = "Constranit/Index Columns:"
        '
        'DataGridViewColumns
        '
        Me.DataGridViewColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewColumns.Location = New System.Drawing.Point(6, 19)
        Me.DataGridViewColumns.Name = "DataGridViewColumns"
        Me.DataGridViewColumns.Size = New System.Drawing.Size(473, 299)
        Me.DataGridViewColumns.TabIndex = 0
        '
        'DataGridViewIndexes
        '
        Me.DataGridViewIndexes.AllowUserToAddRows = False
        Me.DataGridViewIndexes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewIndexes.Location = New System.Drawing.Point(6, 47)
        Me.DataGridViewIndexes.Name = "DataGridViewIndexes"
        Me.DataGridViewIndexes.Size = New System.Drawing.Size(273, 322)
        Me.DataGridViewIndexes.TabIndex = 0
        '
        'LabelPromotModelName
        '
        Me.LabelPromotModelName.AutoSize = True
        Me.LabelPromotModelName.Location = New System.Drawing.Point(120, 38)
        Me.LabelPromotModelName.Name = "LabelPromotModelName"
        Me.LabelPromotModelName.Size = New System.Drawing.Size(123, 13)
        Me.LabelPromotModelName.TabIndex = 3
        Me.LabelPromotModelName.Text = "LabelPromotModelName"
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(74, 38)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 2
        Me.LabelPromptModel.Text = "Model:"
        '
        'LabelPromptTableNodeTypeName
        '
        Me.LabelPromptTableNodeTypeName.AutoSize = True
        Me.LabelPromptTableNodeTypeName.Location = New System.Drawing.Point(120, 16)
        Me.LabelPromptTableNodeTypeName.Name = "LabelPromptTableNodeTypeName"
        Me.LabelPromptTableNodeTypeName.Size = New System.Drawing.Size(171, 13)
        Me.LabelPromptTableNodeTypeName.TabIndex = 1
        Me.LabelPromptTableNodeTypeName.Text = "LabelPromptTableNodeTypeName"
        '
        'LabelPromotTableNodeType
        '
        Me.LabelPromotTableNodeType.AutoSize = True
        Me.LabelPromotTableNodeType.Location = New System.Drawing.Point(12, 16)
        Me.LabelPromotTableNodeType.Name = "LabelPromotTableNodeType"
        Me.LabelPromotTableNodeType.Size = New System.Drawing.Size(101, 13)
        Me.LabelPromotTableNodeType.TabIndex = 0
        Me.LabelPromotTableNodeType.Text = "Table / Node Type:"
        '
        'frmCRUDIndexManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmCRUDIndexManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Constraint/Index Manager"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.GroupBoxIndexes.ResumeLayout(False)
        Me.GroupBoxIndexColumns.ResumeLayout(False)
        CType(Me.DataGridViewColumns, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewIndexes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents GroupBoxIndexes As GroupBox
    Friend WithEvents LabelPromotModelName As Label
    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents LabelPromptTableNodeTypeName As Label
    Friend WithEvents LabelPromotTableNodeType As Label
    Friend WithEvents DataGridViewIndexes As DataGridView
    Friend WithEvents GroupBoxIndexColumns As GroupBox
    Friend WithEvents ButtonApply As Button
    Friend WithEvents ButtonRevert As Button
    Friend WithEvents ButtonAddConstraint As Button
    Friend WithEvents DataGridViewColumns As DataGridView
End Class
