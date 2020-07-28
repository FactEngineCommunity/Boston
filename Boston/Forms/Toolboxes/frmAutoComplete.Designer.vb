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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox = New FlickerFreeListBox 'System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'ListBox
        '
        Me.ListBox.BackColor = System.Drawing.Color.White
        Me.ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.IntegralHeight = False
        Me.ListBox.Location = New System.Drawing.Point(3, 4)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.Size = New System.Drawing.Size(236, 134)
        Me.ListBox.TabIndex = 0
        '
        'frmAutoComplete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(244, 144)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmAutoComplete"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox As System.Windows.Forms.ListBox
End Class
