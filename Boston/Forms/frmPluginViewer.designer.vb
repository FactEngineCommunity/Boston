<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPluginViewer
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
        Me.components = New System.ComponentModel.Container()
        Me.groupbox_main = New System.Windows.Forms.GroupBox()
        Me.listbox_plugin_name = New System.Windows.Forms.ListBox()
        Me.button_close = New System.Windows.Forms.Button()
        Me.ContextMenuStripShowForm = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowPluginFormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.groupbox_main.SuspendLayout()
        Me.ContextMenuStripShowForm.SuspendLayout()
        Me.SuspendLayout()
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.listbox_plugin_name)
        Me.groupbox_main.Location = New System.Drawing.Point(9, 4)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(389, 233)
        Me.groupbox_main.TabIndex = 0
        Me.groupbox_main.TabStop = False
        Me.groupbox_main.Text = "Installed Plugins :"
        '
        'listbox_plugin_name
        '
        Me.listbox_plugin_name.ContextMenuStrip = Me.ContextMenuStripShowForm
        Me.listbox_plugin_name.FormattingEnabled = True
        Me.listbox_plugin_name.Location = New System.Drawing.Point(17, 29)
        Me.listbox_plugin_name.Name = "listbox_plugin_name"
        Me.listbox_plugin_name.Size = New System.Drawing.Size(352, 186)
        Me.listbox_plugin_name.TabIndex = 0
        '
        'button_close
        '
        Me.button_close.Location = New System.Drawing.Point(404, 12)
        Me.button_close.Name = "button_close"
        Me.button_close.Size = New System.Drawing.Size(76, 26)
        Me.button_close.TabIndex = 1
        Me.button_close.Text = "&Close"
        Me.button_close.UseVisualStyleBackColor = True
        '
        'ContextMenuStripShowForm
        '
        Me.ContextMenuStripShowForm.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowPluginFormToolStripMenuItem})
        Me.ContextMenuStripShowForm.Name = "ContextMenuStripShowForm"
        Me.ContextMenuStripShowForm.Size = New System.Drawing.Size(172, 26)
        '
        'ShowPluginFormToolStripMenuItem
        '
        Me.ShowPluginFormToolStripMenuItem.Name = "ShowPluginFormToolStripMenuItem"
        Me.ShowPluginFormToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.ShowPluginFormToolStripMenuItem.Text = "&Show Plugin Form"
        '
        'frmPluginViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(485, 246)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_close)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmPluginViewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Boston Server - PlugIn Viewer"
        Me.groupbox_main.ResumeLayout(False)
        Me.ContextMenuStripShowForm.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents listbox_plugin_name As System.Windows.Forms.ListBox
    Friend WithEvents button_close As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStripShowForm As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowPluginFormToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
