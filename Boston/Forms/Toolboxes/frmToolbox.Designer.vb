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
        Me.ToolBox = New Silver.UI.ToolBox()
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
        'ToolBox
        '
        Me.ToolBox.AllowDrop = True
        Me.ToolBox.AllowSwappingByDragDrop = True
        Me.ToolBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolBox.InitialScrollDelay = 500
        Me.ToolBox.ItemBackgroundColor = System.Drawing.Color.Empty
        Me.ToolBox.ItemBorderColor = System.Drawing.Color.Empty
        Me.ToolBox.ItemHeight = 20
        Me.ToolBox.ItemHoverColor = System.Drawing.SystemColors.Control
        Me.ToolBox.ItemHoverTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.ItemNormalColor = System.Drawing.SystemColors.Control
        Me.ToolBox.ItemNormalTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.ItemSelectedColor = System.Drawing.Color.White
        Me.ToolBox.ItemSelectedTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.ItemSpacing = 2
        Me.ToolBox.LargeItemSize = New System.Drawing.Size(64, 64)
        Me.ToolBox.LayoutDelay = 10
        Me.ToolBox.Location = New System.Drawing.Point(0, 0)
        Me.ToolBox.Name = "ToolBox"
        Me.ToolBox.ScrollDelay = 60
        Me.ToolBox.SelectAllTextWhileRenaming = True
        Me.ToolBox.SelectedTabIndex = -1
        Me.ToolBox.ShowOnlyOneItemPerRow = False
        Me.ToolBox.Size = New System.Drawing.Size(334, 771)
        Me.ToolBox.SmallItemSize = New System.Drawing.Size(32, 32)
        Me.ToolBox.TabHeight = 18
        Me.ToolBox.TabHoverTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.TabIndex = 1
        Me.ToolBox.TabNormalTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.TabSelectedTextColor = System.Drawing.SystemColors.ControlText
        Me.ToolBox.TabSpacing = 1
        Me.ToolBox.UseItemColorInRename = False
        '
        'frmToolbox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 771)
        Me.Controls.Add(Me.ToolBox)
        Me.Controls.Add(Me.ShapeListBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolbox"
        Me.TabText = "Toolbox"
        Me.Text = "Toolbox"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ShapeListBox As MindFusion.Diagramming.WinForms.ShapeListBox
    Friend WithEvents DelayTimer As System.Windows.Forms.Timer
    Friend WithEvents ToolBox As Silver.UI.ToolBox
End Class
