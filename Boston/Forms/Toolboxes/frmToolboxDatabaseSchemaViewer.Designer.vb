<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxDatabaseSchemaViewer
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
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelModel = New System.Windows.Forms.Label()
        Me.GroupboxSchema = New System.Windows.Forms.GroupBox()
        Me.TreeViewSchema = New System.Windows.Forms.TreeView()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.GroupboxSchema.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(12, 9)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 0
        Me.LabelPromptModel.Text = "Model:"
        '
        'LabelModel
        '
        Me.LabelModel.AutoSize = True
        Me.LabelModel.Location = New System.Drawing.Point(57, 9)
        Me.LabelModel.Name = "LabelModel"
        Me.LabelModel.Size = New System.Drawing.Size(79, 13)
        Me.LabelModel.TabIndex = 1
        Me.LabelModel.Text = "<Model Name>"
        '
        'GroupboxSchema
        '
        Me.GroupboxSchema.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupboxSchema.Controls.Add(Me.TreeViewSchema)
        Me.GroupboxSchema.Location = New System.Drawing.Point(12, 37)
        Me.GroupboxSchema.Name = "GroupboxSchema"
        Me.GroupboxSchema.Size = New System.Drawing.Size(196, 429)
        Me.GroupboxSchema.TabIndex = 2
        Me.GroupboxSchema.TabStop = False
        Me.GroupboxSchema.Text = "Schema:"
        '
        'TreeViewSchema
        '
        Me.TreeViewSchema.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeViewSchema.Location = New System.Drawing.Point(6, 19)
        Me.TreeViewSchema.Name = "TreeViewSchema"
        Me.TreeViewSchema.Size = New System.Drawing.Size(184, 404)
        Me.TreeViewSchema.TabIndex = 0
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefresh.Location = New System.Drawing.Point(185, 12)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(23, 23)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.UseVisualStyleBackColor = True
        '
        'frmToolboxDatabaseSchemaViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(220, 478)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.GroupboxSchema)
        Me.Controls.Add(Me.LabelModel)
        Me.Controls.Add(Me.LabelPromptModel)
        Me.Name = "frmToolboxDatabaseSchemaViewer"
        Me.Text = "Database Schema Viewer"
        Me.GroupboxSchema.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents LabelModel As Label
    Friend WithEvents GroupboxSchema As GroupBox
    Friend WithEvents TreeViewSchema As TreeView
    Friend WithEvents ButtonRefresh As Button
End Class
