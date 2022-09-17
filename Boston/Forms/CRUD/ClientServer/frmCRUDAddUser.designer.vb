<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDAddUser
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.components = New System.ComponentModel.Container()
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.groupbox_main = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TextBoxLastName = New System.Windows.Forms.TextBox()
        Me.TextBoxFirstName = New System.Windows.Forms.TextBox()
        Me.LabelPromptLastName = New System.Windows.Forms.Label()
        Me.LabelPromptFirstName = New System.Windows.Forms.Label()
        Me.PictureBox_line2 = New System.Windows.Forms.PictureBox()
        Me.labelprompt_confirmation_password = New System.Windows.Forms.Label()
        Me.labelprompt_password = New System.Windows.Forms.Label()
        Me.textbox_confirmation_password = New System.Windows.Forms.TextBox()
        Me.textbox_password = New System.Windows.Forms.TextBox()
        Me.labelprompt_operator_name = New System.Windows.Forms.Label()
        Me.textbox_operator_name = New System.Windows.Forms.TextBox()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.groupbox_main.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(503, 45)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 6
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button_okay.Location = New System.Drawing.Point(503, 12)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(69, 24)
        Me.button_okay.TabIndex = 5
        Me.button_okay.Text = "&OK"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.PictureBox1)
        Me.groupbox_main.Controls.Add(Me.TextBoxLastName)
        Me.groupbox_main.Controls.Add(Me.TextBoxFirstName)
        Me.groupbox_main.Controls.Add(Me.LabelPromptLastName)
        Me.groupbox_main.Controls.Add(Me.LabelPromptFirstName)
        Me.groupbox_main.Controls.Add(Me.PictureBox_line2)
        Me.groupbox_main.Controls.Add(Me.labelprompt_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.labelprompt_password)
        Me.groupbox_main.Controls.Add(Me.textbox_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.textbox_password)
        Me.groupbox_main.Controls.Add(Me.labelprompt_operator_name)
        Me.groupbox_main.Controls.Add(Me.textbox_operator_name)
        Me.groupbox_main.Location = New System.Drawing.Point(9, 2)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(478, 375)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(27, 122)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox1.TabIndex = 23
        Me.PictureBox1.TabStop = False
        '
        'TextBoxLastName
        '
        Me.TextBoxLastName.Location = New System.Drawing.Point(124, 88)
        Me.TextBoxLastName.Name = "TextBoxLastName"
        Me.TextBoxLastName.Size = New System.Drawing.Size(194, 20)
        Me.TextBoxLastName.TabIndex = 2
        '
        'TextBoxFirstName
        '
        Me.TextBoxFirstName.Location = New System.Drawing.Point(124, 61)
        Me.TextBoxFirstName.Name = "TextBoxFirstName"
        Me.TextBoxFirstName.Size = New System.Drawing.Size(194, 20)
        Me.TextBoxFirstName.TabIndex = 1
        '
        'LabelPromptLastName
        '
        Me.LabelPromptLastName.AutoSize = True
        Me.LabelPromptLastName.Location = New System.Drawing.Point(54, 91)
        Me.LabelPromptLastName.Name = "LabelPromptLastName"
        Me.LabelPromptLastName.Size = New System.Drawing.Size(61, 13)
        Me.LabelPromptLastName.TabIndex = 20
        Me.LabelPromptLastName.Text = "Last Name:"
        '
        'LabelPromptFirstName
        '
        Me.LabelPromptFirstName.AutoSize = True
        Me.LabelPromptFirstName.Location = New System.Drawing.Point(55, 64)
        Me.LabelPromptFirstName.Name = "LabelPromptFirstName"
        Me.LabelPromptFirstName.Size = New System.Drawing.Size(60, 13)
        Me.LabelPromptFirstName.TabIndex = 19
        Me.LabelPromptFirstName.Text = "First Name:"
        '
        'PictureBox_line2
        '
        Me.PictureBox_line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox_line2.Location = New System.Drawing.Point(27, 208)
        Me.PictureBox_line2.Name = "PictureBox_line2"
        Me.PictureBox_line2.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox_line2.TabIndex = 9
        Me.PictureBox_line2.TabStop = False
        '
        'labelprompt_confirmation_password
        '
        Me.labelprompt_confirmation_password.AutoSize = True
        Me.labelprompt_confirmation_password.Location = New System.Drawing.Point(24, 174)
        Me.labelprompt_confirmation_password.Name = "labelprompt_confirmation_password"
        Me.labelprompt_confirmation_password.Size = New System.Drawing.Size(97, 13)
        Me.labelprompt_confirmation_password.TabIndex = 8
        Me.labelprompt_confirmation_password.Text = "Confirm Password :"
        Me.labelprompt_confirmation_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'labelprompt_password
        '
        Me.labelprompt_password.AutoSize = True
        Me.labelprompt_password.Location = New System.Drawing.Point(62, 142)
        Me.labelprompt_password.Name = "labelprompt_password"
        Me.labelprompt_password.Size = New System.Drawing.Size(59, 13)
        Me.labelprompt_password.TabIndex = 7
        Me.labelprompt_password.Text = "Password :"
        Me.labelprompt_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'textbox_confirmation_password
        '
        Me.textbox_confirmation_password.Location = New System.Drawing.Point(124, 171)
        Me.textbox_confirmation_password.Name = "textbox_confirmation_password"
        Me.textbox_confirmation_password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.textbox_confirmation_password.Size = New System.Drawing.Size(195, 20)
        Me.textbox_confirmation_password.TabIndex = 4
        '
        'textbox_password
        '
        Me.textbox_password.Location = New System.Drawing.Point(124, 139)
        Me.textbox_password.Name = "textbox_password"
        Me.textbox_password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.textbox_password.Size = New System.Drawing.Size(196, 20)
        Me.textbox_password.TabIndex = 3
        '
        'labelprompt_operator_name
        '
        Me.labelprompt_operator_name.AutoSize = True
        Me.labelprompt_operator_name.Location = New System.Drawing.Point(30, 29)
        Me.labelprompt_operator_name.Name = "labelprompt_operator_name"
        Me.labelprompt_operator_name.Size = New System.Drawing.Size(85, 13)
        Me.labelprompt_operator_name.TabIndex = 1
        Me.labelprompt_operator_name.Text = "Operator Name :"
        '
        'textbox_operator_name
        '
        Me.textbox_operator_name.Location = New System.Drawing.Point(124, 26)
        Me.textbox_operator_name.Name = "textbox_operator_name"
        Me.textbox_operator_name.Size = New System.Drawing.Size(196, 20)
        Me.textbox_operator_name.TabIndex = 0
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'frmCRUDAddUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 488)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCRUDAddUser"
        Me.TabText = "Add User"
        Me.Text = "Add User"
        Me.groupbox_main.ResumeLayout(False)
        Me.groupbox_main.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents textbox_confirmation_password As System.Windows.Forms.TextBox
    Friend WithEvents textbox_password As System.Windows.Forms.TextBox
    Friend WithEvents labelprompt_operator_name As System.Windows.Forms.Label
    Friend WithEvents textbox_operator_name As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox_line2 As System.Windows.Forms.PictureBox
    Friend WithEvents labelprompt_confirmation_password As System.Windows.Forms.Label
    Friend WithEvents labelprompt_password As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TextBoxLastName As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFirstName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptLastName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptFirstName As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider As ErrorProvider
End Class
