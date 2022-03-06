<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxErrorList
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxErrorList))
        Me.DataGrid_ErrorList = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripHelp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemShowInDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ContextMenuStripShowCoreModelErrors = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemShowCoreModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.DataGrid_ErrorList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripHelp.SuspendLayout()
        Me.ContextMenuStripShowCoreModelErrors.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGrid_ErrorList
        '
        Me.DataGrid_ErrorList.AllowUserToAddRows = False
        Me.DataGrid_ErrorList.AllowUserToDeleteRows = False
        Me.DataGrid_ErrorList.AllowUserToResizeRows = False
        Me.DataGrid_ErrorList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid_ErrorList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid_ErrorList.ContextMenuStrip = Me.ContextMenuStripHelp
        Me.DataGrid_ErrorList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGrid_ErrorList.Location = New System.Drawing.Point(0, 26)
        Me.DataGrid_ErrorList.Name = "DataGrid_ErrorList"
        Me.DataGrid_ErrorList.RowHeadersWidth = 62
        Me.DataGrid_ErrorList.Size = New System.Drawing.Size(755, 190)
        Me.DataGrid_ErrorList.TabIndex = 1
        '
        'ContextMenuStripHelp
        '
        Me.ContextMenuStripHelp.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripHelp.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem, Me.ToolStripMenuItemShowInDiagram})
        Me.ContextMenuStripHelp.Name = "ContextMenuStripHelp"
        Me.ContextMenuStripHelp.Size = New System.Drawing.Size(181, 70)
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'ToolStripMenuItemShowInDiagram
        '
        Me.ToolStripMenuItemShowInDiagram.Name = "ToolStripMenuItemShowInDiagram"
        Me.ToolStripMenuItemShowInDiagram.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItemShowInDiagram.Text = "&Show in Diagram..."
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(2, 0)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(24, 23)
        Me.ButtonRefresh.TabIndex = 2
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'ContextMenuStripShowCoreModelErrors
        '
        Me.ContextMenuStripShowCoreModelErrors.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemShowCoreModelErrors})
        Me.ContextMenuStripShowCoreModelErrors.Name = "ContextMenuStripShowCoreModelErrors"
        Me.ContextMenuStripShowCoreModelErrors.Size = New System.Drawing.Size(202, 26)
        '
        'ToolStripMenuItemShowCoreModelErrors
        '
        Me.ToolStripMenuItemShowCoreModelErrors.Name = "ToolStripMenuItemShowCoreModelErrors"
        Me.ToolStripMenuItemShowCoreModelErrors.Size = New System.Drawing.Size(201, 22)
        Me.ToolStripMenuItemShowCoreModelErrors.Text = "Show &Core Model errors"
        '
        'frmToolboxErrorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(754, 217)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.DataGrid_ErrorList)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxErrorList"
        Me.TabText = "Error List"
        Me.Text = "Error List"
        CType(Me.DataGrid_ErrorList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripHelp.ResumeLayout(False)
        Me.ContextMenuStripShowCoreModelErrors.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGrid_ErrorList As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStripHelp As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemShowInDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonRefresh As Button
    Friend WithEvents ContextMenuStripShowCoreModelErrors As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemShowCoreModelErrors As ToolStripMenuItem
End Class
