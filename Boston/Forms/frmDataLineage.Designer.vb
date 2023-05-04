<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataLineage
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
        Me.GroupBoxCategories = New System.Windows.Forms.GroupBox()
        Me.LabelPromtLineageItem = New System.Windows.Forms.Label()
        Me.LabelLineageItem = New System.Windows.Forms.Label()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'GroupBoxCategories
        '
        Me.GroupBoxCategories.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxCategories.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxCategories.ForeColor = System.Drawing.Color.Gray
        Me.GroupBoxCategories.Location = New System.Drawing.Point(12, 85)
        Me.GroupBoxCategories.Name = "GroupBoxCategories"
        Me.GroupBoxCategories.Size = New System.Drawing.Size(932, 520)
        Me.GroupBoxCategories.TabIndex = 0
        Me.GroupBoxCategories.TabStop = False
        Me.GroupBoxCategories.Text = "Lineage Categories:"
        '
        'LabelPromtLineageItem
        '
        Me.LabelPromtLineageItem.AutoSize = True
        Me.LabelPromtLineageItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromtLineageItem.ForeColor = System.Drawing.Color.Gray
        Me.LabelPromtLineageItem.Location = New System.Drawing.Point(12, 30)
        Me.LabelPromtLineageItem.Name = "LabelPromtLineageItem"
        Me.LabelPromtLineageItem.Size = New System.Drawing.Size(88, 16)
        Me.LabelPromtLineageItem.TabIndex = 1
        Me.LabelPromtLineageItem.Text = "Lineage Item:"
        '
        'LabelLineageItem
        '
        Me.LabelLineageItem.AutoSize = True
        Me.LabelLineageItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelLineageItem.Location = New System.Drawing.Point(106, 24)
        Me.LabelLineageItem.Name = "LabelLineageItem"
        Me.LabelLineageItem.Size = New System.Drawing.Size(100, 24)
        Me.LabelLineageItem.TabIndex = 2
        Me.LabelLineageItem.Text = "LabelItem"
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(15, 56)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 3
        Me.ButtonSave.Text = "&Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(96, 56)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 4
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmDataLineage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(956, 617)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.LabelLineageItem)
        Me.Controls.Add(Me.LabelPromtLineageItem)
        Me.Controls.Add(Me.GroupBoxCategories)
        Me.Name = "frmDataLineage"
        Me.Text = "frmDataLineage"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBoxCategories As GroupBox
    Friend WithEvents LabelPromtLineageItem As Label
    Friend WithEvents LabelLineageItem As Label
    Friend WithEvents ButtonSave As Button
    Friend WithEvents ButtonClose As Button
End Class
