<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRegistration
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
        Me.LabelPromptApplicationKey = New System.Windows.Forms.Label()
        Me.LabelApplicationKey = New System.Windows.Forms.Label()
        Me.LabelPromptRegistrationKey = New System.Windows.Forms.Label()
        Me.TextBoxRegistrationKey = New System.Windows.Forms.TextBox()
        Me.LabelPromptRegistrationStatus = New System.Windows.Forms.Label()
        Me.LabelRegistrationStatus = New System.Windows.Forms.Label()
        Me.ButtonSaveRegistrationKey = New System.Windows.Forms.Button()
        Me.ButtonCopyModelIdToClipboard = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelHintApplicationCopyIcon = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelPromptApplicationKey
        '
        Me.LabelPromptApplicationKey.AutoSize = True
        Me.LabelPromptApplicationKey.Location = New System.Drawing.Point(24, 20)
        Me.LabelPromptApplicationKey.Name = "LabelPromptApplicationKey"
        Me.LabelPromptApplicationKey.Size = New System.Drawing.Size(83, 13)
        Me.LabelPromptApplicationKey.TabIndex = 0
        Me.LabelPromptApplicationKey.Text = "Application Key:"
        '
        'LabelApplicationKey
        '
        Me.LabelApplicationKey.AutoSize = True
        Me.LabelApplicationKey.Location = New System.Drawing.Point(139, 20)
        Me.LabelApplicationKey.Name = "LabelApplicationKey"
        Me.LabelApplicationKey.Size = New System.Drawing.Size(103, 13)
        Me.LabelApplicationKey.TabIndex = 1
        Me.LabelApplicationKey.Text = "LabelApplicationKey"
        '
        'LabelPromptRegistrationKey
        '
        Me.LabelPromptRegistrationKey.AutoSize = True
        Me.LabelPromptRegistrationKey.Location = New System.Drawing.Point(20, 46)
        Me.LabelPromptRegistrationKey.Name = "LabelPromptRegistrationKey"
        Me.LabelPromptRegistrationKey.Size = New System.Drawing.Size(87, 13)
        Me.LabelPromptRegistrationKey.TabIndex = 2
        Me.LabelPromptRegistrationKey.Text = "Registration Key:"
        '
        'TextBoxRegistrationKey
        '
        Me.TextBoxRegistrationKey.Location = New System.Drawing.Point(113, 43)
        Me.TextBoxRegistrationKey.Name = "TextBoxRegistrationKey"
        Me.TextBoxRegistrationKey.Size = New System.Drawing.Size(291, 20)
        Me.TextBoxRegistrationKey.TabIndex = 3
        '
        'LabelPromptRegistrationStatus
        '
        Me.LabelPromptRegistrationStatus.AutoSize = True
        Me.LabelPromptRegistrationStatus.Location = New System.Drawing.Point(8, 77)
        Me.LabelPromptRegistrationStatus.Name = "LabelPromptRegistrationStatus"
        Me.LabelPromptRegistrationStatus.Size = New System.Drawing.Size(99, 13)
        Me.LabelPromptRegistrationStatus.TabIndex = 4
        Me.LabelPromptRegistrationStatus.Text = "Registration Status:"
        '
        'LabelRegistrationStatus
        '
        Me.LabelRegistrationStatus.AutoSize = True
        Me.LabelRegistrationStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelRegistrationStatus.Location = New System.Drawing.Point(113, 77)
        Me.LabelRegistrationStatus.Name = "LabelRegistrationStatus"
        Me.LabelRegistrationStatus.Size = New System.Drawing.Size(119, 13)
        Me.LabelRegistrationStatus.TabIndex = 5
        Me.LabelRegistrationStatus.Text = "LabelRegistrationStatus"
        '
        'ButtonSaveRegistrationKey
        '
        Me.ButtonSaveRegistrationKey.Location = New System.Drawing.Point(410, 41)
        Me.ButtonSaveRegistrationKey.Name = "ButtonSaveRegistrationKey"
        Me.ButtonSaveRegistrationKey.Size = New System.Drawing.Size(142, 23)
        Me.ButtonSaveRegistrationKey.TabIndex = 6
        Me.ButtonSaveRegistrationKey.Text = "&Save my Registration Key"
        Me.ButtonSaveRegistrationKey.UseVisualStyleBackColor = True
        '
        'ButtonCopyModelIdToClipboard
        '
        Me.ButtonCopyModelIdToClipboard.FlatAppearance.BorderSize = 0
        Me.ButtonCopyModelIdToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCopyModelIdToClipboard.Image = Global.Boston.My.Resources.Resources.CopyToClipboard16x16
        Me.ButtonCopyModelIdToClipboard.Location = New System.Drawing.Point(113, 14)
        Me.ButtonCopyModelIdToClipboard.Name = "ButtonCopyModelIdToClipboard"
        Me.ButtonCopyModelIdToClipboard.Size = New System.Drawing.Size(20, 23)
        Me.ButtonCopyModelIdToClipboard.TabIndex = 13
        Me.ButtonCopyModelIdToClipboard.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelHintApplicationCopyIcon})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 176)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(626, 22)
        Me.StatusStrip1.TabIndex = 14
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabelHintApplicationCopyIcon
        '
        Me.ToolStripStatusLabelHintApplicationCopyIcon.Name = "ToolStripStatusLabelHintApplicationCopyIcon"
        Me.ToolStripStatusLabelHintApplicationCopyIcon.Size = New System.Drawing.Size(388, 17)
        Me.ToolStripStatusLabelHintApplicationCopyIcon.Text = "Click on the clipboard icon to copy the Application Key to the clipboard."
        '
        'frmRegistration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(626, 198)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ButtonCopyModelIdToClipboard)
        Me.Controls.Add(Me.ButtonSaveRegistrationKey)
        Me.Controls.Add(Me.LabelRegistrationStatus)
        Me.Controls.Add(Me.LabelPromptRegistrationStatus)
        Me.Controls.Add(Me.TextBoxRegistrationKey)
        Me.Controls.Add(Me.LabelPromptRegistrationKey)
        Me.Controls.Add(Me.LabelApplicationKey)
        Me.Controls.Add(Me.LabelPromptApplicationKey)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRegistration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Boston Registration"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptApplicationKey As Label
    Friend WithEvents LabelApplicationKey As Label
    Friend WithEvents LabelPromptRegistrationKey As Label
    Friend WithEvents TextBoxRegistrationKey As TextBox
    Friend WithEvents LabelPromptRegistrationStatus As Label
    Friend WithEvents LabelRegistrationStatus As Label
    Friend WithEvents ButtonSaveRegistrationKey As Button
    Friend WithEvents ButtonCopyModelIdToClipboard As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabelHintApplicationCopyIcon As ToolStripStatusLabel
End Class
