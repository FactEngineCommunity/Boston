<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotifications
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
        Me.FlexibleListBox = New Boston.FlexibleListBox()
        Me.SuspendLayout()
        '
        'FlexibleListBox
        '
        Me.FlexibleListBox.AutoScroll = True
        Me.FlexibleListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlexibleListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlexibleListBox.Location = New System.Drawing.Point(0, 0)
        Me.FlexibleListBox.Name = "FlexibleListBox"
        Me.FlexibleListBox.Size = New System.Drawing.Size(284, 261)
        Me.FlexibleListBox.TabIndex = 0
        '
        'frmNotifications
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.FlexibleListBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmNotifications"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "frmNotifications"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FlexibleListBox As Boston.FlexibleListBox
End Class
