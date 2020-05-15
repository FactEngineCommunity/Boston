<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NotificationBarGeneralNotification
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
        Me.ButtonGotit = New System.Windows.Forms.Button()
        Me.LabelNotificationText = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ButtonGotit
        '
        Me.ButtonGotit.Location = New System.Drawing.Point(12, 13)
        Me.ButtonGotit.Name = "ButtonGotit"
        Me.ButtonGotit.Size = New System.Drawing.Size(70, 22)
        Me.ButtonGotit.TabIndex = 0
        Me.ButtonGotit.Text = "Got it!"
        Me.ButtonGotit.UseVisualStyleBackColor = True
        '
        'LabelNotificationText
        '
        Me.LabelNotificationText.Location = New System.Drawing.Point(93, 4)
        Me.LabelNotificationText.Name = "LabelNotificationText"
        Me.LabelNotificationText.Size = New System.Drawing.Size(382, 42)
        Me.LabelNotificationText.TabIndex = 1
        Me.LabelNotificationText.Text = "LabelNotificationText"
        '
        'NotificationBarGeneralNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.LabelNotificationText)
        Me.Controls.Add(Me.ButtonGotit)
        Me.Name = "NotificationBarGeneralNotification"
        Me.Size = New System.Drawing.Size(509, 50)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonGotit As System.Windows.Forms.Button
    Friend WithEvents LabelNotificationText As System.Windows.Forms.Label

End Class
