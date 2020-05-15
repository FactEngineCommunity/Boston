<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class edit_role_frm
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
        Me.labelprompt_role_name = New System.Windows.Forms.Label
        Me.textbox_role_name = New System.Windows.Forms.TextBox
        Me.groupbox_main.SuspendLayout()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(396, 40)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button_okay.Location = New System.Drawing.Point(396, 7)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(69, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&OK"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.textbox_role_name)
        Me.groupbox_main.Controls.Add(Me.labelprompt_role_name)
        Me.groupbox_main.Location = New System.Drawing.Point(8, 3)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(373, 63)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'labelprompt_role_name
        '
        Me.labelprompt_role_name.AutoSize = True
        Me.labelprompt_role_name.Location = New System.Drawing.Point(22, 28)
        Me.labelprompt_role_name.Name = "labelprompt_role_name"
        Me.labelprompt_role_name.Size = New System.Drawing.Size(66, 13)
        Me.labelprompt_role_name.TabIndex = 0
        Me.labelprompt_role_name.Text = "Role Name :"
        '
        'textbox_role_name
        '
        Me.textbox_role_name.Location = New System.Drawing.Point(94, 25)
        Me.textbox_role_name.Name = "textbox_role_name"
        Me.textbox_role_name.Size = New System.Drawing.Size(249, 20)
        Me.textbox_role_name.TabIndex = 1
        '
        'edit_role_frm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(475, 75)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "edit_role_frm"
        Me.Text = "Edit Role"
        Me.groupbox_main.ResumeLayout(False)
        Me.groupbox_main.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents textbox_role_name As System.Windows.Forms.TextBox
    Friend WithEvents labelprompt_role_name As System.Windows.Forms.Label
End Class
