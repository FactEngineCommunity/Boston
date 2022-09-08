<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenu_main = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MaximiseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListBox = New System.Windows.Forms.ListBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EndSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MinimiseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MaximiseToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.NormalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutBostonServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenu_main.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'NotifyIcon
        '
        Me.NotifyIcon.ContextMenuStrip = Me.ContextMenu_main
        Me.NotifyIcon.Icon = CType(resources.GetObject("NotifyIcon.Icon"), System.Drawing.Icon)
        Me.NotifyIcon.Text = "NotifyIcon"
        Me.NotifyIcon.Visible = True
        '
        'ContextMenu_main
        '
        Me.ContextMenu_main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MaximiseToolStripMenuItem, Me.ToolStripSeparator1, Me.QuitToolStripMenuItem})
        Me.ContextMenu_main.Name = "ContextMenu_main"
        Me.ContextMenu_main.Size = New System.Drawing.Size(126, 54)
        '
        'MaximiseToolStripMenuItem
        '
        Me.MaximiseToolStripMenuItem.Name = "MaximiseToolStripMenuItem"
        Me.MaximiseToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.MaximiseToolStripMenuItem.Text = "&Maximise"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(122, 6)
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.QuitToolStripMenuItem.Text = "&Quit"
        '
        'ListBox
        '
        Me.ListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.Location = New System.Drawing.Point(3, 16)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.Size = New System.Drawing.Size(725, 268)
        Me.ListBox.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SessionToolStripMenuItem, Me.ViewToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(731, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SessionToolStripMenuItem
        '
        Me.SessionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EndSessionToolStripMenuItem})
        Me.SessionToolStripMenuItem.Name = "SessionToolStripMenuItem"
        Me.SessionToolStripMenuItem.Size = New System.Drawing.Size(58, 20)
        Me.SessionToolStripMenuItem.Text = "&Session"
        '
        'EndSessionToolStripMenuItem
        '
        Me.EndSessionToolStripMenuItem.Name = "EndSessionToolStripMenuItem"
        Me.EndSessionToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.EndSessionToolStripMenuItem.Text = "&End Session"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MinimiseToolStripMenuItem, Me.MaximiseToolStripMenuItem1, Me.NormalToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'MinimiseToolStripMenuItem
        '
        Me.MinimiseToolStripMenuItem.Name = "MinimiseToolStripMenuItem"
        Me.MinimiseToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.MinimiseToolStripMenuItem.Text = "&Minimise to Tray"
        '
        'MaximiseToolStripMenuItem1
        '
        Me.MaximiseToolStripMenuItem1.Name = "MaximiseToolStripMenuItem1"
        Me.MaximiseToolStripMenuItem1.Size = New System.Drawing.Size(161, 22)
        Me.MaximiseToolStripMenuItem1.Text = "Ma&ximise"
        '
        'NormalToolStripMenuItem
        '
        Me.NormalToolStripMenuItem.Name = "NormalToolStripMenuItem"
        Me.NormalToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.NormalToolStripMenuItem.Text = "&Normal"
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.ListBox)
        Me.GroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox.Location = New System.Drawing.Point(0, 24)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(731, 287)
        Me.GroupBox.TabIndex = 3
        Me.GroupBox.TabStop = False
        Me.GroupBox.Text = "Status/Events:"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutBostonServerToolStripMenuItem})
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AboutToolStripMenuItem.Text = "&About"
        '
        'AboutBostonServerToolStripMenuItem
        '
        Me.AboutBostonServerToolStripMenuItem.Name = "AboutBostonServerToolStripMenuItem"
        Me.AboutBostonServerToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.AboutBostonServerToolStripMenuItem.Text = "&About Boston Server"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 311)
        Me.Controls.Add(Me.GroupBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Boston Server"
        Me.WindowState = System.Windows.Forms.FormWindowState.Minimized
        Me.ContextMenu_main.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenu_main As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MaximiseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListBox As System.Windows.Forms.ListBox
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EndSessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MinimiseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MaximiseToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NormalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutBostonServerToolStripMenuItem As ToolStripMenuItem
End Class
