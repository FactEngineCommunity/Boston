<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxORMReadingEditor
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
        Me.TextboxReading = New System.Windows.Forms.RichTextBox()
        Me.ContextMenuStrip_MoveTerms = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MoveLeftToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveRightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.DataGrid_Readings = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripIsPreferred = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemIsPreferred = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelPromptFactType = New System.Windows.Forms.Label()
        Me.LabelFactTypeName = New System.Windows.Forms.Label()
        Me.LabelHelpTips = New System.Windows.Forms.Label()
        Me.LabelFactTypeReadingEditor = New System.Windows.Forms.Label()
        Me.ContextMenuStripIsPreferredForPredicate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemIsPreferredForPredicate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_MoveTerms.SuspendLayout()
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripIsPreferred.SuspendLayout()
        Me.ContextMenuStripIsPreferredForPredicate.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextboxReading
        '
        Me.TextboxReading.AutoWordSelection = True
        Me.TextboxReading.ContextMenuStrip = Me.ContextMenuStrip_MoveTerms
        Me.TextboxReading.ForeColor = System.Drawing.Color.Silver
        Me.TextboxReading.Location = New System.Drawing.Point(12, 55)
        Me.TextboxReading.Multiline = False
        Me.TextboxReading.Name = "TextboxReading"
        Me.TextboxReading.Size = New System.Drawing.Size(643, 25)
        Me.TextboxReading.TabIndex = 0
        Me.TextboxReading.Text = "Enter a Fact Type Reading for the selected Fact Type here."
        '
        'ContextMenuStrip_MoveTerms
        '
        Me.ContextMenuStrip_MoveTerms.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MoveLeftToolStripMenuItem, Me.MoveRightToolStripMenuItem})
        Me.ContextMenuStrip_MoveTerms.Name = "ContextMenuStrip_MoveTerms"
        Me.ContextMenuStrip_MoveTerms.Size = New System.Drawing.Size(136, 48)
        '
        'MoveLeftToolStripMenuItem
        '
        Me.MoveLeftToolStripMenuItem.Name = "MoveLeftToolStripMenuItem"
        Me.MoveLeftToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.MoveLeftToolStripMenuItem.Text = "&Move Left"
        '
        'MoveRightToolStripMenuItem
        '
        Me.MoveRightToolStripMenuItem.Name = "MoveRightToolStripMenuItem"
        Me.MoveRightToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.MoveRightToolStripMenuItem.Text = "&Move Right"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(661, 55)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(88, 25)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "&Clear Reading"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'DataGrid_Readings
        '
        Me.DataGrid_Readings.AllowUserToAddRows = False
        Me.DataGrid_Readings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid_Readings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid_Readings.ContextMenuStrip = Me.ContextMenuStripIsPreferred
        Me.DataGrid_Readings.Location = New System.Drawing.Point(11, 84)
        Me.DataGrid_Readings.Name = "DataGrid_Readings"
        Me.DataGrid_Readings.Size = New System.Drawing.Size(800, 123)
        Me.DataGrid_Readings.TabIndex = 7
        '
        'ContextMenuStripIsPreferred
        '
        Me.ContextMenuStripIsPreferred.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemIsPreferred})
        Me.ContextMenuStripIsPreferred.Name = "ContextMenuStripIsPreferred"
        Me.ContextMenuStripIsPreferred.Size = New System.Drawing.Size(134, 26)
        '
        'ToolStripMenuItemIsPreferred
        '
        Me.ToolStripMenuItemIsPreferred.Name = "ToolStripMenuItemIsPreferred"
        Me.ToolStripMenuItemIsPreferred.Size = New System.Drawing.Size(133, 22)
        Me.ToolStripMenuItemIsPreferred.Text = "Is Preferred"
        '
        'LabelPromptFactType
        '
        Me.LabelPromptFactType.AutoSize = True
        Me.LabelPromptFactType.Location = New System.Drawing.Point(9, 14)
        Me.LabelPromptFactType.Name = "LabelPromptFactType"
        Me.LabelPromptFactType.Size = New System.Drawing.Size(103, 13)
        Me.LabelPromptFactType.TabIndex = 9
        Me.LabelPromptFactType.Text = "Selected Fact Type:"
        '
        'LabelFactTypeName
        '
        Me.LabelFactTypeName.AutoSize = True
        Me.LabelFactTypeName.Location = New System.Drawing.Point(115, 14)
        Me.LabelFactTypeName.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelFactTypeName.Name = "LabelFactTypeName"
        Me.LabelFactTypeName.Size = New System.Drawing.Size(132, 13)
        Me.LabelFactTypeName.TabIndex = 10
        Me.LabelFactTypeName.Text = " <No Fact Type Selected>"
        '
        'LabelHelpTips
        '
        Me.LabelHelpTips.BackColor = System.Drawing.Color.LemonChiffon
        Me.LabelHelpTips.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelpTips.Location = New System.Drawing.Point(0, 212)
        Me.LabelHelpTips.Name = "LabelHelpTips"
        Me.LabelHelpTips.Size = New System.Drawing.Size(824, 40)
        Me.LabelHelpTips.TabIndex = 11
        Me.LabelHelpTips.Text = "LabelHelpTips"
        '
        'LabelFactTypeReadingEditor
        '
        Me.LabelFactTypeReadingEditor.AutoSize = True
        Me.LabelFactTypeReadingEditor.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.LabelFactTypeReadingEditor.Location = New System.Drawing.Point(9, 39)
        Me.LabelFactTypeReadingEditor.Name = "LabelFactTypeReadingEditor"
        Me.LabelFactTypeReadingEditor.Size = New System.Drawing.Size(131, 13)
        Me.LabelFactTypeReadingEditor.TabIndex = 12
        Me.LabelFactTypeReadingEditor.Text = "Fact Type Reading Editor:"
        '
        'ContextMenuStripIsPreferredForPredicate
        '
        Me.ContextMenuStripIsPreferredForPredicate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemIsPreferredForPredicate})
        Me.ContextMenuStripIsPreferredForPredicate.Name = "ContextMenuStripIsPreferredForPredicate"
        Me.ContextMenuStripIsPreferredForPredicate.Size = New System.Drawing.Size(204, 48)
        '
        'ToolStripMenuItemIsPreferredForPredicate
        '
        Me.ToolStripMenuItemIsPreferredForPredicate.Name = "ToolStripMenuItemIsPreferredForPredicate"
        Me.ToolStripMenuItemIsPreferredForPredicate.Size = New System.Drawing.Size(203, 22)
        Me.ToolStripMenuItemIsPreferredForPredicate.Text = "Is Preferred for Predicate"
        '
        'frmToolboxORMReadingEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(824, 252)
        Me.Controls.Add(Me.LabelFactTypeReadingEditor)
        Me.Controls.Add(Me.LabelHelpTips)
        Me.Controls.Add(Me.LabelPromptFactType)
        Me.Controls.Add(Me.LabelFactTypeName)
        Me.Controls.Add(Me.TextboxReading)
        Me.Controls.Add(Me.DataGrid_Readings)
        Me.Controls.Add(Me.Button3)
        Me.Name = "frmToolboxORMReadingEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "ORM Reading Editor"
        Me.Text = "ORM Reading Editor"
        Me.ContextMenuStrip_MoveTerms.ResumeLayout(False)
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripIsPreferred.ResumeLayout(False)
        Me.ContextMenuStripIsPreferredForPredicate.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextboxReading As System.Windows.Forms.RichTextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip_MoveTerms As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MoveLeftToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveRightToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGrid_Readings As System.Windows.Forms.DataGridView
    Friend WithEvents LabelPromptFactType As System.Windows.Forms.Label
    Friend WithEvents LabelFactTypeName As System.Windows.Forms.Label
    Friend WithEvents LabelHelpTips As System.Windows.Forms.Label
    Friend WithEvents LabelFactTypeReadingEditor As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStripIsPreferred As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemIsPreferred As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStripIsPreferredForPredicate As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemIsPreferredForPredicate As System.Windows.Forms.ToolStripMenuItem
End Class
