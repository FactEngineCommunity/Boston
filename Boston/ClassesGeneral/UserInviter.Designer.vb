<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UserInviter
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
        Me.LabelUserName = New System.Windows.Forms.Label()
        Me.ButtonInvite = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Boston.My.Resources.MenuImagesMain.profile16x16
        Me.PictureBox1.Location = New System.Drawing.Point(9, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(20, 19)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'LabelUserName
        '
        Me.LabelUserName.AutoSize = True
        Me.LabelUserName.Location = New System.Drawing.Point(93, 6)
        Me.LabelUserName.MaximumSize = New System.Drawing.Size(150, 0)
        Me.LabelUserName.Name = "LabelUserName"
        Me.LabelUserName.Size = New System.Drawing.Size(83, 13)
        Me.LabelUserName.TabIndex = 1
        Me.LabelUserName.Text = "LabelUserName"
        '
        'ButtonInvite
        '
        Me.ButtonInvite.Location = New System.Drawing.Point(37, 7)
        Me.ButtonInvite.Name = "ButtonInvite"
        Me.ButtonInvite.Size = New System.Drawing.Size(50, 22)
        Me.ButtonInvite.TabIndex = 2
        Me.ButtonInvite.Text = "Invite"
        Me.ButtonInvite.UseVisualStyleBackColor = True
        '
        'UserInviter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.ButtonInvite)
        Me.Controls.Add(Me.LabelUserName)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "UserInviter"
        Me.Size = New System.Drawing.Size(281, 37)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents LabelUserName As System.Windows.Forms.Label
    Friend WithEvents ButtonInvite As System.Windows.Forms.Button

End Class
