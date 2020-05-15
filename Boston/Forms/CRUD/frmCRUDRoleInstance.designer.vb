<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDRoleInstance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCRUDRoleInstance))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.LabelCreatingNewOrExtending = New System.Windows.Forms.Label
        Me.TextBox_FactTypeName = New System.Windows.Forms.TextBox
        Me.LabelPromptFactTypeName = New System.Windows.Forms.Label
        Me.GroupBox_joins = New System.Windows.Forms.GroupBox
        Me.PictureBoxBracket = New System.Windows.Forms.PictureBox
        Me.ComboBox_join_object = New System.Windows.Forms.ComboBox
        Me.RadioButton_nested_FactType = New System.Windows.Forms.RadioButton
        Me.RadioButton_value_type = New System.Windows.Forms.RadioButton
        Me.RadioButton_entity_type = New System.Windows.Forms.RadioButton
        Me.checkbox_mandatory = New System.Windows.Forms.CheckBox
        Me.TextBox_RoleName = New System.Windows.Forms.TextBox
        Me.labelpromptRoleName = New System.Windows.Forms.Label
        Me.Button_cancel = New System.Windows.Forms.Button
        Me.button_okay = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox_joins.SuspendLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelCreatingNewOrExtending)
        Me.GroupBox1.Controls.Add(Me.TextBox_FactTypeName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptFactTypeName)
        Me.GroupBox1.Controls.Add(Me.GroupBox_joins)
        Me.GroupBox1.Controls.Add(Me.checkbox_mandatory)
        Me.GroupBox1.Controls.Add(Me.TextBox_RoleName)
        Me.GroupBox1.Controls.Add(Me.labelpromptRoleName)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(454, 235)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'LabelCreatingNewOrExtending
        '
        Me.LabelCreatingNewOrExtending.AutoSize = True
        Me.LabelCreatingNewOrExtending.Location = New System.Drawing.Point(12, 18)
        Me.LabelCreatingNewOrExtending.Name = "LabelCreatingNewOrExtending"
        Me.LabelCreatingNewOrExtending.Size = New System.Drawing.Size(152, 13)
        Me.LabelCreatingNewOrExtending.TabIndex = 6
        Me.LabelCreatingNewOrExtending.Text = "LabelCreatingNewOrExtending"
        '
        'TextBox_FactTypeName
        '
        Me.TextBox_FactTypeName.Location = New System.Drawing.Point(110, 42)
        Me.TextBox_FactTypeName.Name = "TextBox_FactTypeName"
        Me.TextBox_FactTypeName.Size = New System.Drawing.Size(329, 20)
        Me.TextBox_FactTypeName.TabIndex = 5
        '
        'LabelPromptFactTypeName
        '
        Me.LabelPromptFactTypeName.AutoSize = True
        Me.LabelPromptFactTypeName.Location = New System.Drawing.Point(12, 45)
        Me.LabelPromptFactTypeName.Name = "LabelPromptFactTypeName"
        Me.LabelPromptFactTypeName.Size = New System.Drawing.Size(92, 13)
        Me.LabelPromptFactTypeName.TabIndex = 4
        Me.LabelPromptFactTypeName.Text = "Fact Type Name :"
        '
        'GroupBox_joins
        '
        Me.GroupBox_joins.Controls.Add(Me.PictureBoxBracket)
        Me.GroupBox_joins.Controls.Add(Me.ComboBox_join_object)
        Me.GroupBox_joins.Controls.Add(Me.RadioButton_nested_FactType)
        Me.GroupBox_joins.Controls.Add(Me.RadioButton_value_type)
        Me.GroupBox_joins.Controls.Add(Me.RadioButton_entity_type)
        Me.GroupBox_joins.Location = New System.Drawing.Point(15, 117)
        Me.GroupBox_joins.Name = "GroupBox_joins"
        Me.GroupBox_joins.Size = New System.Drawing.Size(424, 104)
        Me.GroupBox_joins.TabIndex = 3
        Me.GroupBox_joins.TabStop = False
        Me.GroupBox_joins.Text = "Joins :"
        '
        'PictureBoxBracket
        '
        Me.PictureBoxBracket.Image = CType(resources.GetObject("PictureBoxBracket.Image"), System.Drawing.Image)
        Me.PictureBoxBracket.Location = New System.Drawing.Point(139, 20)
        Me.PictureBoxBracket.Name = "PictureBoxBracket"
        Me.PictureBoxBracket.Size = New System.Drawing.Size(21, 78)
        Me.PictureBoxBracket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxBracket.TabIndex = 7
        Me.PictureBoxBracket.TabStop = False
        '
        'ComboBox_join_object
        '
        Me.ComboBox_join_object.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_join_object.FormattingEnabled = True
        Me.ComboBox_join_object.Location = New System.Drawing.Point(166, 45)
        Me.ComboBox_join_object.Name = "ComboBox_join_object"
        Me.ComboBox_join_object.Size = New System.Drawing.Size(240, 21)
        Me.ComboBox_join_object.Sorted = True
        Me.ComboBox_join_object.TabIndex = 6
        '
        'RadioButton_nested_FactType
        '
        Me.RadioButton_nested_FactType.AutoSize = True
        Me.RadioButton_nested_FactType.Location = New System.Drawing.Point(27, 74)
        Me.RadioButton_nested_FactType.Name = "RadioButton_nested_FactType"
        Me.RadioButton_nested_FactType.Size = New System.Drawing.Size(116, 17)
        Me.RadioButton_nested_FactType.TabIndex = 5
        Me.RadioButton_nested_FactType.Text = "&Nested Fact Type :"
        Me.RadioButton_nested_FactType.UseVisualStyleBackColor = True
        '
        'RadioButton_value_type
        '
        Me.RadioButton_value_type.AutoSize = True
        Me.RadioButton_value_type.Location = New System.Drawing.Point(27, 51)
        Me.RadioButton_value_type.Name = "RadioButton_value_type"
        Me.RadioButton_value_type.Size = New System.Drawing.Size(85, 17)
        Me.RadioButton_value_type.TabIndex = 4
        Me.RadioButton_value_type.Text = "&Value Type :"
        Me.RadioButton_value_type.UseVisualStyleBackColor = True
        '
        'RadioButton_entity_type
        '
        Me.RadioButton_entity_type.AutoSize = True
        Me.RadioButton_entity_type.Checked = True
        Me.RadioButton_entity_type.Location = New System.Drawing.Point(27, 28)
        Me.RadioButton_entity_type.Name = "RadioButton_entity_type"
        Me.RadioButton_entity_type.Size = New System.Drawing.Size(84, 17)
        Me.RadioButton_entity_type.TabIndex = 3
        Me.RadioButton_entity_type.TabStop = True
        Me.RadioButton_entity_type.Text = "&Entity Type :"
        Me.RadioButton_entity_type.UseVisualStyleBackColor = True
        '
        'checkbox_mandatory
        '
        Me.checkbox_mandatory.AutoSize = True
        Me.checkbox_mandatory.Location = New System.Drawing.Point(110, 94)
        Me.checkbox_mandatory.Name = "checkbox_mandatory"
        Me.checkbox_mandatory.Size = New System.Drawing.Size(101, 17)
        Me.checkbox_mandatory.TabIndex = 2
        Me.checkbox_mandatory.Text = "&Mandatory Role"
        Me.checkbox_mandatory.UseVisualStyleBackColor = True
        '
        'TextBox_RoleName
        '
        Me.TextBox_RoleName.Location = New System.Drawing.Point(110, 68)
        Me.TextBox_RoleName.Name = "TextBox_RoleName"
        Me.TextBox_RoleName.Size = New System.Drawing.Size(329, 20)
        Me.TextBox_RoleName.TabIndex = 1
        '
        'labelpromptRoleName
        '
        Me.labelpromptRoleName.AutoSize = True
        Me.labelpromptRoleName.CausesValidation = False
        Me.labelpromptRoleName.Location = New System.Drawing.Point(38, 71)
        Me.labelpromptRoleName.Name = "labelpromptRoleName"
        Me.labelpromptRoleName.Size = New System.Drawing.Size(66, 13)
        Me.labelpromptRoleName.TabIndex = 0
        Me.labelpromptRoleName.Text = "&Role Name :"
        '
        'Button_cancel
        '
        Me.Button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_cancel.Location = New System.Drawing.Point(473, 39)
        Me.Button_cancel.Name = "Button_cancel"
        Me.Button_cancel.Size = New System.Drawing.Size(76, 26)
        Me.Button_cancel.TabIndex = 8
        Me.Button_cancel.Text = "&Cancel"
        Me.Button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(474, 7)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(75, 26)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&Ok"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'frmCRUDRoleInstance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(561, 250)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.Button_cancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmCRUDRoleInstance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Role"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox_joins.ResumeLayout(False)
        Me.GroupBox_joins.PerformLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents labelpromptRoleName As System.Windows.Forms.Label
    Friend WithEvents GroupBox_joins As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_nested_FactType As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_value_type As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_entity_type As System.Windows.Forms.RadioButton
    Friend WithEvents checkbox_mandatory As System.Windows.Forms.CheckBox
    Friend WithEvents TextBox_RoleName As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox_join_object As System.Windows.Forms.ComboBox
    Friend WithEvents Button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents TextBox_FactTypeName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptFactTypeName As System.Windows.Forms.Label
    Friend WithEvents PictureBoxBracket As System.Windows.Forms.PictureBox
    Friend WithEvents LabelCreatingNewOrExtending As System.Windows.Forms.Label
End Class
