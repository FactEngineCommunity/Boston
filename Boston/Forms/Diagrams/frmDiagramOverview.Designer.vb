<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramOverview
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiagramOverview))
        Me.Overview = New MindFusion.Diagramming.WinForms.Overview()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Overview
        '
        Me.Overview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Overview.DiagramView = Nothing
        Me.Overview.Location = New System.Drawing.Point(-1, 28)
        Me.Overview.Name = "Overview"
        Me.Overview.Size = New System.Drawing.Size(356, 432)
        Me.Overview.TabIndex = 0
        Me.Overview.Text = "Overview1"
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(-1, -1)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(24, 23)
        Me.ButtonRefresh.TabIndex = 7
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'frmDiagramOverview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(355, 459)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.Overview)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDiagramOverview"
        Me.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft
        Me.TabText = "Diagram Overview"
        Me.Text = "Diagram Overview"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Overview As MindFusion.Diagramming.WinForms.Overview
    Friend WithEvents ButtonRefresh As Button
End Class
