<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAutoComplete
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAutoComplete))
        Me.ListBox = New System.Windows.Forms.ListBox()
        Me.DateTimePicker = New Boston.DateTimePicker()
        Me.SuspendLayout()
        '
        'ListBox
        '
        Me.ListBox.BackColor = System.Drawing.Color.GhostWhite
        Me.ListBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.ItemHeight = 20
        Me.ListBox.Location = New System.Drawing.Point(12, 10)
        Me.ListBox.Margin = New System.Windows.Forms.Padding(2)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.Size = New System.Drawing.Size(217, 100)
        Me.ListBox.TabIndex = 1
        '
        'DateTimePicker
        '
        Me.DateTimePicker.Location = New System.Drawing.Point(18, 12)
        Me.DateTimePicker.Name = "DateTimePicker"
        Me.DateTimePicker.Size = New System.Drawing.Size(207, 56)
        Me.DateTimePicker.TabIndex = 2
        '
        'frmAutoComplete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(241, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.DateTimePicker)
        Me.Controls.Add(Me.ListBox)
        Me.ForeColor = System.Drawing.Color.Transparent
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAutoComplete"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListBox As ListBox
    Friend WithEvents DateTimePicker As DateTimePicker
End Class
