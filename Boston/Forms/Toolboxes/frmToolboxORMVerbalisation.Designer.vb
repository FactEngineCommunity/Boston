<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxORMVerbalisation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxORMVerbalisation))
        Me.ContextMenuStripAddBusinessRequirement = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddAsBusinessRequirementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.ContextMenuStripAddBusinessRequirement.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStripAddBusinessRequirement
        '
        Me.ContextMenuStripAddBusinessRequirement.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddAsBusinessRequirementToolStripMenuItem})
        Me.ContextMenuStripAddBusinessRequirement.Name = "ContextMenuStripAddBusinessRequirement"
        Me.ContextMenuStripAddBusinessRequirement.Size = New System.Drawing.Size(230, 26)
        '
        'AddAsBusinessRequirementToolStripMenuItem
        '
        Me.AddAsBusinessRequirementToolStripMenuItem.Name = "AddAsBusinessRequirementToolStripMenuItem"
        Me.AddAsBusinessRequirementToolStripMenuItem.Size = New System.Drawing.Size(229, 22)
        Me.AddAsBusinessRequirementToolStripMenuItem.Text = "&Add as Business Requirement"
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(962, 424)
        Me.WebBrowser.TabIndex = 1
        '
        'frmToolboxORMVerbalisation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 424)
        Me.Controls.Add(Me.WebBrowser)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxORMVerbalisation"
        Me.TabText = "ORM Verbalisation"
        Me.Text = "ORM Verbalisation"
        Me.ContextMenuStripAddBusinessRequirement.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStripAddBusinessRequirement As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddAsBusinessRequirementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
End Class
