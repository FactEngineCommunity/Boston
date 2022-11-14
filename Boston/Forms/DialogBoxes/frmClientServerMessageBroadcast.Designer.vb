<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClientServerMessageBroadcast
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClientServerMessageBroadcast))
        Me.GroupBoxMessageBroadcast = New System.Windows.Forms.GroupBox()
        Me.TextBoxNotification = New System.Windows.Forms.TextBox()
        Me.ButtonBroadcastMessage = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.GroupBoxMessageBroadcast.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxMessageBroadcast
        '
        Me.GroupBoxMessageBroadcast.Controls.Add(Me.ButtonClose)
        Me.GroupBoxMessageBroadcast.Controls.Add(Me.ButtonBroadcastMessage)
        Me.GroupBoxMessageBroadcast.Controls.Add(Me.TextBoxNotification)
        Me.GroupBoxMessageBroadcast.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxMessageBroadcast.Name = "GroupBoxMessageBroadcast"
        Me.GroupBoxMessageBroadcast.Size = New System.Drawing.Size(624, 117)
        Me.GroupBoxMessageBroadcast.TabIndex = 0
        Me.GroupBoxMessageBroadcast.TabStop = False
        Me.GroupBoxMessageBroadcast.Text = "Message:"
        '
        'TextBoxNotification
        '
        Me.TextBoxNotification.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxNotification.Location = New System.Drawing.Point(10, 19)
        Me.TextBoxNotification.MaxLength = 254
        Me.TextBoxNotification.Multiline = True
        Me.TextBoxNotification.Name = "TextBoxNotification"
        Me.TextBoxNotification.Size = New System.Drawing.Size(603, 57)
        Me.TextBoxNotification.TabIndex = 0
        '
        'ButtonBroadcastMessage
        '
        Me.ButtonBroadcastMessage.Location = New System.Drawing.Point(139, 82)
        Me.ButtonBroadcastMessage.Name = "ButtonBroadcastMessage"
        Me.ButtonBroadcastMessage.Size = New System.Drawing.Size(246, 23)
        Me.ButtonBroadcastMessage.TabIndex = 1
        Me.ButtonBroadcastMessage.Text = "Broadcast message to all Boston Online users..."
        Me.ButtonBroadcastMessage.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(391, 82)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(47, 23)
        Me.ButtonClose.TabIndex = 2
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmClientServerMessageBroadcast
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(648, 137)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBoxMessageBroadcast)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmClientServerMessageBroadcast"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Boston Online Message Broadcast"
        Me.GroupBoxMessageBroadcast.ResumeLayout(False)
        Me.GroupBoxMessageBroadcast.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBoxMessageBroadcast As GroupBox
    Friend WithEvents ButtonBroadcastMessage As Button
    Friend WithEvents TextBoxNotification As TextBox
    Friend WithEvents ButtonClose As Button
End Class
