<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxProperties
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxProperties))
        Me.PropertyGrid = New Azuria.Common.Controls.FilteredPropertyGrid
        Me.SuspendLayout()
        '
        'PropertyGrid
        '
        Me.PropertyGrid.BrowsableProperties = Nothing
        Me.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PropertyGrid.HiddenAttributes = Nothing
        Me.PropertyGrid.HiddenProperties = Nothing
        Me.PropertyGrid.Location = New System.Drawing.Point(0, 0)
        Me.PropertyGrid.Name = "PropertyGrid"
        Me.PropertyGrid.Size = New System.Drawing.Size(302, 616)
        Me.PropertyGrid.TabIndex = 1
        '
        'frmToolboxProperties
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(302, 616)
        Me.Controls.Add(Me.PropertyGrid)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxProperties"
        Me.TabText = "Properties"
        Me.Text = "Properties"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PropertyGrid As Azuria.Common.Controls.FilteredPropertyGrid
End Class
