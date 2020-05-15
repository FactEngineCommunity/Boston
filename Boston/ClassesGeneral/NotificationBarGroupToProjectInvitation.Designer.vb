<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NotificationBarGroupToProjectInvitation
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonAcceptInvitation = New System.Windows.Forms.Button()
        Me.LabelPromptDetails = New System.Windows.Forms.Label()
        Me.ButtonDeclineInvitation = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Boston.My.Resources.Resources.Group16x16
        Me.PictureBox1.Location = New System.Drawing.Point(9, 15)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ButtonAcceptInvitation
        '
        Me.ButtonAcceptInvitation.Location = New System.Drawing.Point(34, 9)
        Me.ButtonAcceptInvitation.Name = "ButtonAcceptInvitation"
        Me.ButtonAcceptInvitation.Size = New System.Drawing.Size(55, 28)
        Me.ButtonAcceptInvitation.TabIndex = 1
        Me.ButtonAcceptInvitation.Text = "Accept"
        Me.ButtonAcceptInvitation.UseVisualStyleBackColor = True
        '
        'LabelPromptDetails
        '
        Me.LabelPromptDetails.AutoSize = True
        Me.LabelPromptDetails.Location = New System.Drawing.Point(153, 5)
        Me.LabelPromptDetails.MaximumSize = New System.Drawing.Size(330, 0)
        Me.LabelPromptDetails.Name = "LabelPromptDetails"
        Me.LabelPromptDetails.Size = New System.Drawing.Size(98, 13)
        Me.LabelPromptDetails.TabIndex = 2
        Me.LabelPromptDetails.Text = "LabelPromptDetails"
        '
        'ButtonDeclineInvitation
        '
        Me.ButtonDeclineInvitation.Location = New System.Drawing.Point(92, 9)
        Me.ButtonDeclineInvitation.Name = "ButtonDeclineInvitation"
        Me.ButtonDeclineInvitation.Size = New System.Drawing.Size(55, 28)
        Me.ButtonDeclineInvitation.TabIndex = 3
        Me.ButtonDeclineInvitation.Text = "Decline"
        Me.ButtonDeclineInvitation.UseVisualStyleBackColor = True
        '
        'NotificationBarGroupToProjectInvitation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.ButtonDeclineInvitation)
        Me.Controls.Add(Me.LabelPromptDetails)
        Me.Controls.Add(Me.ButtonAcceptInvitation)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "NotificationBarGroupToProjectInvitation"
        Me.Size = New System.Drawing.Size(509, 50)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAcceptInvitation As System.Windows.Forms.Button
    Friend WithEvents LabelPromptDetails As System.Windows.Forms.Label
    Friend WithEvents ButtonDeclineInvitation As System.Windows.Forms.Button

End Class
