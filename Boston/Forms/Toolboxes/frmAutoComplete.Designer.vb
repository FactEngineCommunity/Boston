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
        Me.ListBox = New Boston.FlickerFreeListBox()
        Me.SuspendLayout()
        '
        'ListBox
        '
        Me.ListBox.BackColor = System.Drawing.Color.GhostWhite
        Me.ListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ListBox.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.IntegralHeight = False
        Me.ListBox.Location = New System.Drawing.Point(5, 2)
        Me.ListBox.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.Size = New System.Drawing.Size(352, 204)
        Me.ListBox.TabIndex = 0
        '
        'frmAutoComplete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(362, 215)
        Me.ControlBox = False
        Me.Controls.Add(Me.ListBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmAutoComplete"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListBox As FlickerFreeListBox
End Class
