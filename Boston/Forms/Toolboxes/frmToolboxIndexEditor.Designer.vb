<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxIndexEditor
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

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
        Me.LabelPromptTableName = New System.Windows.Forms.Label()
        Me.LabelTableName = New System.Windows.Forms.Label()
        Me.DataGridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripIndex = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteIndexToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripIndex.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelPromptTableName
        '
        Me.LabelPromptTableName.AutoSize = True
        Me.LabelPromptTableName.Location = New System.Drawing.Point(12, 9)
        Me.LabelPromptTableName.Name = "LabelPromptTableName"
        Me.LabelPromptTableName.Size = New System.Drawing.Size(68, 13)
        Me.LabelPromptTableName.TabIndex = 0
        Me.LabelPromptTableName.Text = "Table Name:"
        '
        'LabelTableName
        '
        Me.LabelTableName.AutoSize = True
        Me.LabelTableName.Location = New System.Drawing.Point(86, 9)
        Me.LabelTableName.Name = "LabelTableName"
        Me.LabelTableName.Size = New System.Drawing.Size(88, 13)
        Me.LabelTableName.TabIndex = 1
        Me.LabelTableName.Text = "LabelTableName"
        '
        'DataGridView
        '
        Me.DataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView.Location = New System.Drawing.Point(15, 35)
        Me.DataGridView.Name = "DataGridView"
        Me.DataGridView.Size = New System.Drawing.Size(743, 176)
        Me.DataGridView.TabIndex = 2
        '
        'ContextMenuStripIndex
        '
        Me.ContextMenuStripIndex.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteIndexToolStripMenuItem})
        Me.ContextMenuStripIndex.Name = "ContextMenuStripIndex"
        Me.ContextMenuStripIndex.Size = New System.Drawing.Size(181, 48)
        '
        'DeleteIndexToolStripMenuItem
        '
        Me.DeleteIndexToolStripMenuItem.Name = "DeleteIndexToolStripMenuItem"
        Me.DeleteIndexToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.DeleteIndexToolStripMenuItem.Text = "&Delete Index"
        '
        'frmToolboxIndexEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(770, 223)
        Me.Controls.Add(Me.DataGridView)
        Me.Controls.Add(Me.LabelTableName)
        Me.Controls.Add(Me.LabelPromptTableName)
        Me.Name = "frmToolboxIndexEditor"
        Me.Text = "Index Editor"
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripIndex.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptTableName As Label
    Friend WithEvents LabelTableName As Label
    Friend WithEvents DataGridView As DataGridView
    Friend WithEvents ContextMenuStripIndex As ContextMenuStrip
    Friend WithEvents DeleteIndexToolStripMenuItem As ToolStripMenuItem
End Class
