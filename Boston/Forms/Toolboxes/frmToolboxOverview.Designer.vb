<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxOverview
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
        Me.Overview = New MindFusion.Diagramming.WinForms.Overview
        Me.SuspendLayout()
        '
        'Overview
        '
        Me.Overview.DiagramView = Nothing
        Me.Overview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Overview.Location = New System.Drawing.Point(0, 0)
        Me.Overview.Name = "Overview"
        Me.Overview.Size = New System.Drawing.Size(127, 127)
        Me.Overview.TabIndex = 0
        '
        'overview_frm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.ClientSize = New System.Drawing.Size(127, 127)
        Me.Controls.Add(Me.Overview)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "overview_frm"
        Me.Text = "Overview"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Overview As MindFusion.Diagramming.WinForms.Overview
End Class
