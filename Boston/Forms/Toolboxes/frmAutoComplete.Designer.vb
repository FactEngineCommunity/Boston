<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAutoComplete
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ListBox = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'ListBox
        '
        Me.ListBox.BackColor = System.Drawing.Color.GhostWhite
        Me.ListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.ItemHeight = 20
        Me.ListBox.Location = New System.Drawing.Point(18, 16)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.Size = New System.Drawing.Size(325, 180)
        Me.ListBox.TabIndex = 1
        '
        'frmAutoComplete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(362, 215)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListBox)
        Me.ForeColor = System.Drawing.Color.Transparent
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmAutoComplete"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox As ListBox
End Class
