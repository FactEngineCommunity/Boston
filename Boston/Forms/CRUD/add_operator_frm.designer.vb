<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class add_operator_frm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.button_cancel = New System.Windows.Forms.Button
        Me.button_okay = New System.Windows.Forms.Button
        Me.groupbox_main = New System.Windows.Forms.GroupBox
        Me.combobox_employee_name = New System.Windows.Forms.ComboBox
        Me.labelprompt_employee = New System.Windows.Forms.Label
        Me.checkbox_operator_is_employee = New System.Windows.Forms.CheckBox
        Me.PictureBox_line2 = New System.Windows.Forms.PictureBox
        Me.labelprompt_confirmation_password = New System.Windows.Forms.Label
        Me.labelprompt_password = New System.Windows.Forms.Label
        Me.textbox_confirmation_password = New System.Windows.Forms.TextBox
        Me.textbox_password = New System.Windows.Forms.TextBox
        Me.PictureBox_line1 = New System.Windows.Forms.PictureBox
        Me.labelprompt_role = New System.Windows.Forms.Label
        Me.combobox_role_name = New System.Windows.Forms.ComboBox
        Me.labelprompt_operator_name = New System.Windows.Forms.Label
        Me.textbox_operator_name = New System.Windows.Forms.TextBox
        Me.groupbox_main.SuspendLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_line1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(378, 41)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button_okay.Location = New System.Drawing.Point(378, 8)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(69, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&OK"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.combobox_employee_name)
        Me.groupbox_main.Controls.Add(Me.labelprompt_employee)
        Me.groupbox_main.Controls.Add(Me.checkbox_operator_is_employee)
        Me.groupbox_main.Controls.Add(Me.PictureBox_line2)
        Me.groupbox_main.Controls.Add(Me.labelprompt_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.labelprompt_password)
        Me.groupbox_main.Controls.Add(Me.textbox_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.textbox_password)
        Me.groupbox_main.Controls.Add(Me.PictureBox_line1)
        Me.groupbox_main.Controls.Add(Me.labelprompt_role)
        Me.groupbox_main.Controls.Add(Me.combobox_role_name)
        Me.groupbox_main.Controls.Add(Me.labelprompt_operator_name)
        Me.groupbox_main.Controls.Add(Me.textbox_operator_name)
        Me.groupbox_main.Location = New System.Drawing.Point(9, 2)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(363, 252)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'combobox_employee_name
        '
        Me.combobox_employee_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.combobox_employee_name.FormattingEnabled = True
        Me.combobox_employee_name.Location = New System.Drawing.Point(125, 219)
        Me.combobox_employee_name.Name = "combobox_employee_name"
        Me.combobox_employee_name.Size = New System.Drawing.Size(195, 21)
        Me.combobox_employee_name.TabIndex = 12
        '
        'labelprompt_employee
        '
        Me.labelprompt_employee.AutoSize = True
        Me.labelprompt_employee.Location = New System.Drawing.Point(63, 219)
        Me.labelprompt_employee.Name = "labelprompt_employee"
        Me.labelprompt_employee.Size = New System.Drawing.Size(59, 13)
        Me.labelprompt_employee.TabIndex = 11
        Me.labelprompt_employee.Text = "Employee :"
        '
        'checkbox_operator_is_employee
        '
        Me.checkbox_operator_is_employee.AutoSize = True
        Me.checkbox_operator_is_employee.Location = New System.Drawing.Point(128, 190)
        Me.checkbox_operator_is_employee.Name = "checkbox_operator_is_employee"
        Me.checkbox_operator_is_employee.Size = New System.Drawing.Size(126, 17)
        Me.checkbox_operator_is_employee.TabIndex = 10
        Me.checkbox_operator_is_employee.Text = "Operator is Employee"
        Me.checkbox_operator_is_employee.UseVisualStyleBackColor = True
        '
        'PictureBox_line2
        '
        Me.PictureBox_line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox_line2.Location = New System.Drawing.Point(15, 177)
        Me.PictureBox_line2.Name = "PictureBox_line2"
        Me.PictureBox_line2.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox_line2.TabIndex = 9
        Me.PictureBox_line2.TabStop = False
        '
        'labelprompt_confirmation_password
        '
        Me.labelprompt_confirmation_password.AutoSize = True
        Me.labelprompt_confirmation_password.Location = New System.Drawing.Point(25, 144)
        Me.labelprompt_confirmation_password.Name = "labelprompt_confirmation_password"
        Me.labelprompt_confirmation_password.Size = New System.Drawing.Size(97, 13)
        Me.labelprompt_confirmation_password.TabIndex = 8
        Me.labelprompt_confirmation_password.Text = "Confirm Password :"
        Me.labelprompt_confirmation_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'labelprompt_password
        '
        Me.labelprompt_password.AutoSize = True
        Me.labelprompt_password.Location = New System.Drawing.Point(63, 112)
        Me.labelprompt_password.Name = "labelprompt_password"
        Me.labelprompt_password.Size = New System.Drawing.Size(59, 13)
        Me.labelprompt_password.TabIndex = 7
        Me.labelprompt_password.Text = "Password :"
        Me.labelprompt_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'textbox_confirmation_password
        '
        Me.textbox_confirmation_password.Location = New System.Drawing.Point(125, 141)
        Me.textbox_confirmation_password.Name = "textbox_confirmation_password"
        Me.textbox_confirmation_password.Size = New System.Drawing.Size(195, 20)
        Me.textbox_confirmation_password.TabIndex = 6
        '
        'textbox_password
        '
        Me.textbox_password.Location = New System.Drawing.Point(125, 109)
        Me.textbox_password.Name = "textbox_password"
        Me.textbox_password.Size = New System.Drawing.Size(196, 20)
        Me.textbox_password.TabIndex = 5
        '
        'PictureBox_line1
        '
        Me.PictureBox_line1.BackColor = System.Drawing.SystemColors.Control
        Me.PictureBox_line1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox_line1.Location = New System.Drawing.Point(15, 94)
        Me.PictureBox_line1.Name = "PictureBox_line1"
        Me.PictureBox_line1.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox_line1.TabIndex = 4
        Me.PictureBox_line1.TabStop = False
        '
        'labelprompt_role
        '
        Me.labelprompt_role.AutoSize = True
        Me.labelprompt_role.Location = New System.Drawing.Point(87, 61)
        Me.labelprompt_role.Name = "labelprompt_role"
        Me.labelprompt_role.Size = New System.Drawing.Size(35, 13)
        Me.labelprompt_role.TabIndex = 3
        Me.labelprompt_role.Text = "Role :"
        '
        'combobox_role_name
        '
        Me.combobox_role_name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.combobox_role_name.FormattingEnabled = True
        Me.combobox_role_name.Location = New System.Drawing.Point(125, 58)
        Me.combobox_role_name.Name = "combobox_role_name"
        Me.combobox_role_name.Size = New System.Drawing.Size(195, 21)
        Me.combobox_role_name.TabIndex = 2
        '
        'labelprompt_operator_name
        '
        Me.labelprompt_operator_name.AutoSize = True
        Me.labelprompt_operator_name.Location = New System.Drawing.Point(37, 29)
        Me.labelprompt_operator_name.Name = "labelprompt_operator_name"
        Me.labelprompt_operator_name.Size = New System.Drawing.Size(85, 13)
        Me.labelprompt_operator_name.TabIndex = 1
        Me.labelprompt_operator_name.Text = "Operator Name :"
        '
        'textbox_operator_name
        '
        Me.textbox_operator_name.Location = New System.Drawing.Point(125, 26)
        Me.textbox_operator_name.Name = "textbox_operator_name"
        Me.textbox_operator_name.Size = New System.Drawing.Size(196, 20)
        Me.textbox_operator_name.TabIndex = 0
        '
        'add_operator_frm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 266)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "add_operator_frm"
        Me.Text = "Add Operator"
        Me.groupbox_main.ResumeLayout(False)
        Me.groupbox_main.PerformLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_line1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents textbox_confirmation_password As System.Windows.Forms.TextBox
    Friend WithEvents textbox_password As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox_line1 As System.Windows.Forms.PictureBox
    Friend WithEvents labelprompt_role As System.Windows.Forms.Label
    Friend WithEvents combobox_role_name As System.Windows.Forms.ComboBox
    Friend WithEvents labelprompt_operator_name As System.Windows.Forms.Label
    Friend WithEvents textbox_operator_name As System.Windows.Forms.TextBox
    Friend WithEvents checkbox_operator_is_employee As System.Windows.Forms.CheckBox
    Friend WithEvents PictureBox_line2 As System.Windows.Forms.PictureBox
    Friend WithEvents labelprompt_confirmation_password As System.Windows.Forms.Label
    Friend WithEvents labelprompt_password As System.Windows.Forms.Label
    Friend WithEvents combobox_employee_name As System.Windows.Forms.ComboBox
    Friend WithEvents labelprompt_employee As System.Windows.Forms.Label
End Class
