<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolbox
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolbox))
        Me.ShapeListBox = New MindFusion.Diagramming.WinForms.ShapeListBox()
        Me.DelayTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ShapeListBox
        '
        Me.ShapeListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ShapeListBox.FormattingEnabled = True
        Me.ShapeListBox.Location = New System.Drawing.Point(0, 0)
        Me.ShapeListBox.Name = "ShapeListBox"
        Me.ShapeListBox.ShapeFillColor = System.Drawing.Color.White
        Me.ShapeListBox.Size = New System.Drawing.Size(334, 771)
        Me.ShapeListBox.TabIndex = 0
        '
        'DelayTimer
        '
        Me.DelayTimer.Interval = 1000
        '
        'frmToolbox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 771)
        Me.Controls.Add(Me.ShapeListBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolbox"
        Me.TabText = "Toolbox"
        Me.Text = "Toolbox"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ShapeListBox As MindFusion.Diagramming.WinForms.ShapeListBox
    Friend WithEvents DelayTimer As System.Windows.Forms.Timer
End Class
