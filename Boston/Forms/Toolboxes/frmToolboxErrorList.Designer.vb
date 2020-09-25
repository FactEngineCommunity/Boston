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
        Me.DataGrid_ErrorList = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripHelp = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemShowInDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        CType(Me.DataGrid_ErrorList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripHelp.SuspendLayout()
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
        Me.DataGrid_ErrorList.Location = New System.Drawing.Point(0, 40)
        Me.DataGrid_ErrorList.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DataGrid_ErrorList.Name = "DataGrid_ErrorList"
        Me.DataGrid_ErrorList.RowHeadersWidth = 62
        Me.DataGrid_ErrorList.Size = New System.Drawing.Size(1132, 292)
        Me.DataGrid_ErrorList.TabIndex = 1
        '
        'ContextMenuStripHelp
        '
        Me.ContextMenuStripHelp.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripHelp.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpToolStripMenuItem, Me.ToolStripMenuItemShowInDiagram})
        Me.ContextMenuStripHelp.Name = "ContextMenuStripHelp"
        Me.ContextMenuStripHelp.Size = New System.Drawing.Size(241, 101)
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(240, 32)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'ToolStripMenuItemShowInDiagram
        '
        Me.ToolStripMenuItemShowInDiagram.Name = "ToolStripMenuItemShowInDiagram"
        Me.ToolStripMenuItemShowInDiagram.Size = New System.Drawing.Size(240, 32)
        Me.ToolStripMenuItemShowInDiagram.Text = "&Show in Diagram..."
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(3, 0)
        Me.ButtonRefresh.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(36, 35)
        Me.ButtonRefresh.TabIndex = 2
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'frmToolboxErrorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1131, 334)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.DataGrid_ErrorList)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmToolboxErrorList"
        Me.TabText = "Error List"
        Me.Text = "Error List"
        CType(Me.DataGrid_ErrorList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripHelp.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGrid_ErrorList As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStripHelp As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemShowInDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ButtonRefresh As Button
End Class
