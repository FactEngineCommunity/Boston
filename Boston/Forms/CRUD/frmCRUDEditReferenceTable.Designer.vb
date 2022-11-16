<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditReferenceTable
    Inherits System.Windows.Forms.Form

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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button_delete = New System.Windows.Forms.Button()
        Me.Button_add_new = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.button_save = New System.Windows.Forms.Button()
        Me.LabelPrompt_ConfigurationItem = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ButtonExportConfigurationItems = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ButtonExportConfigurationItems)
        Me.GroupBox1.Controls.Add(Me.Button_delete)
        Me.GroupBox1.Controls.Add(Me.Button_add_new)
        Me.GroupBox1.Controls.Add(Me.ComboBox1)
        Me.GroupBox1.Controls.Add(Me.button_save)
        Me.GroupBox1.Controls.Add(Me.LabelPrompt_ConfigurationItem)
        Me.GroupBox1.Controls.Add(Me.DataGridView1)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(569, 328)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Button_delete
        '
        Me.Button_delete.Enabled = False
        Me.Button_delete.Location = New System.Drawing.Point(176, 290)
        Me.Button_delete.Name = "Button_delete"
        Me.Button_delete.Size = New System.Drawing.Size(75, 24)
        Me.Button_delete.TabIndex = 5
        Me.Button_delete.Text = "&Delete"
        Me.Button_delete.UseVisualStyleBackColor = True
        '
        'Button_add_new
        '
        Me.Button_add_new.Location = New System.Drawing.Point(95, 290)
        Me.Button_add_new.Name = "Button_add_new"
        Me.Button_add_new.Size = New System.Drawing.Size(75, 24)
        Me.Button_add_new.TabIndex = 4
        Me.Button_add_new.Text = "&New"
        Me.Button_add_new.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(118, 24)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(429, 21)
        Me.ComboBox1.TabIndex = 3
        '
        'button_save
        '
        Me.button_save.Location = New System.Drawing.Point(19, 290)
        Me.button_save.Name = "button_save"
        Me.button_save.Size = New System.Drawing.Size(70, 24)
        Me.button_save.TabIndex = 2
        Me.button_save.Text = "&Save"
        Me.button_save.UseVisualStyleBackColor = True
        '
        'LabelPrompt_ConfigurationItem
        '
        Me.LabelPrompt_ConfigurationItem.AutoSize = True
        Me.LabelPrompt_ConfigurationItem.Location = New System.Drawing.Point(14, 27)
        Me.LabelPrompt_ConfigurationItem.Name = "LabelPrompt_ConfigurationItem"
        Me.LabelPrompt_ConfigurationItem.Size = New System.Drawing.Size(98, 13)
        Me.LabelPrompt_ConfigurationItem.TabIndex = 1
        Me.LabelPrompt_ConfigurationItem.Text = "Configuration Item :"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(17, 64)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(530, 211)
        Me.DataGridView1.TabIndex = 0
        '
        'ButtonExportConfigurationItems
        '
        Me.ButtonExportConfigurationItems.Location = New System.Drawing.Point(257, 291)
        Me.ButtonExportConfigurationItems.Name = "ButtonExportConfigurationItems"
        Me.ButtonExportConfigurationItems.Size = New System.Drawing.Size(145, 23)
        Me.ButtonExportConfigurationItems.TabIndex = 6
        Me.ButtonExportConfigurationItems.Text = "&Export configuration items"
        Me.ButtonExportConfigurationItems.UseVisualStyleBackColor = True
        '
        'frmCRUDEditReferenceTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 343)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDEditReferenceTable"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Maintain Configuration Reference Items"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LabelPrompt_ConfigurationItem As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents button_save As System.Windows.Forms.Button
    Friend WithEvents Button_add_new As System.Windows.Forms.Button
    Friend WithEvents Button_delete As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonExportConfigurationItems As Button
End Class
