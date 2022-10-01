<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
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
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.groupbox_main = New System.Windows.Forms.GroupBox()
        Me.TextBox_password = New System.Windows.Forms.TextBox()
        Me.TextBox_username = New System.Windows.Forms.TextBox()
        Me.LabelPrompt_Password = New System.Windows.Forms.Label()
        Me.LabelPrompt_Username = New System.Windows.Forms.Label()
        Me.groupbox_main.SuspendLayout
        Me.SuspendLayout
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(333, 45)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = true
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(333, 12)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(69, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&OK"
        Me.button_okay.UseVisualStyleBackColor = true
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.TextBox_password)
        Me.groupbox_main.Controls.Add(Me.TextBox_username)
        Me.groupbox_main.Controls.Add(Me.LabelPrompt_Password)
        Me.groupbox_main.Controls.Add(Me.LabelPrompt_Username)
        Me.groupbox_main.Location = New System.Drawing.Point(9, 2)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(318, 89)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = false
        '
        'TextBox_password
        '
        Me.TextBox_password.Location = New System.Drawing.Point(80, 53)
        Me.TextBox_password.Name = "TextBox_password"
        Me.TextBox_password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(35)
        Me.TextBox_password.Size = New System.Drawing.Size(223, 20)
        Me.TextBox_password.TabIndex = 3
        '
        'TextBox_username
        '
        Me.TextBox_username.Location = New System.Drawing.Point(80, 21)
        Me.TextBox_username.Name = "TextBox_username"
        Me.TextBox_username.Size = New System.Drawing.Size(224, 20)
        Me.TextBox_username.TabIndex = 2
        '
        'LabelPrompt_Password
        '
        Me.LabelPrompt_Password.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.LabelPrompt_Password.AutoSize = true
        Me.LabelPrompt_Password.Location = New System.Drawing.Point(15, 55)
        Me.LabelPrompt_Password.Name = "LabelPrompt_Password"
        Me.LabelPrompt_Password.Size = New System.Drawing.Size(59, 13)
        Me.LabelPrompt_Password.TabIndex = 1
        Me.LabelPrompt_Password.Text = "Password :"
        Me.LabelPrompt_Password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LabelPrompt_Username
        '
        Me.LabelPrompt_Username.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.LabelPrompt_Username.AutoSize = true
        Me.LabelPrompt_Username.Location = New System.Drawing.Point(13, 24)
        Me.LabelPrompt_Username.Name = "LabelPrompt_Username"
        Me.LabelPrompt_Username.Size = New System.Drawing.Size(61, 13)
        Me.LabelPrompt_Username.TabIndex = 0
        Me.LabelPrompt_Username.Text = "Username :"
        Me.LabelPrompt_Username.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'frmLogin
        '
        Me.AcceptButton = Me.button_okay
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(406, 103)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = false
        Me.MinimizeBox = false
        Me.Name = "frmLogin"
        Me.Text = "Log-In to Boston"
        Me.groupbox_main.ResumeLayout(false)
        Me.groupbox_main.PerformLayout
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents LabelPrompt_Username As System.Windows.Forms.Label
    Friend WithEvents TextBox_password As System.Windows.Forms.TextBox
    Friend WithEvents TextBox_username As System.Windows.Forms.TextBox
    Friend WithEvents LabelPrompt_Password As System.Windows.Forms.Label
End Class
