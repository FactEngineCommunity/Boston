<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxBrainBox
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
        Me.TextBox_Output = New System.Windows.Forms.RichTextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TextBoxInput = New System.Windows.Forms.RichTextBox()
        Me.ListBoxEnterpriseAware = New System.Windows.Forms.ListBox()
        Me.ContextMenuVirtualAnalyst = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemDictationMode = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemQuietMode = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerInput = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabelMain = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ContextMenuStripBrainBox = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuVirtualAnalyst.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.ContextMenuStripBrainBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox_Output
        '
        Me.TextBox_Output.BackColor = System.Drawing.Color.Ivory
        Me.TextBox_Output.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox_Output.HideSelection = False
        Me.TextBox_Output.Location = New System.Drawing.Point(0, 0)
        Me.TextBox_Output.Name = "TextBox_Output"
        Me.TextBox_Output.Size = New System.Drawing.Size(542, 149)
        Me.TextBox_Output.TabIndex = 1
        Me.TextBox_Output.Text = ""
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(12, 12)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextBoxInput)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListBoxEnterpriseAware)
        Me.SplitContainer1.Panel2.Controls.Add(Me.TextBox_Output)
        Me.SplitContainer1.Size = New System.Drawing.Size(542, 188)
        Me.SplitContainer1.SplitterDistance = 35
        Me.SplitContainer1.TabIndex = 2
        '
        'TextBoxInput
        '
        Me.TextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxInput.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxInput.ForeColor = System.Drawing.Color.SteelBlue
        Me.TextBoxInput.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxInput.MinimumSize = New System.Drawing.Size(4, 34)
        Me.TextBoxInput.Multiline = False
        Me.TextBoxInput.Name = "TextBoxInput"
        Me.TextBoxInput.Size = New System.Drawing.Size(542, 35)
        Me.TextBoxInput.TabIndex = 1
        Me.TextBoxInput.Text = ""
        '
        'ListBoxEnterpriseAware
        '
        Me.ListBoxEnterpriseAware.FormattingEnabled = True
        Me.ListBoxEnterpriseAware.Location = New System.Drawing.Point(84, 3)
        Me.ListBoxEnterpriseAware.Name = "ListBoxEnterpriseAware"
        Me.ListBoxEnterpriseAware.Size = New System.Drawing.Size(119, 69)
        Me.ListBoxEnterpriseAware.TabIndex = 3
        Me.ListBoxEnterpriseAware.Visible = False
        '
        'ContextMenuVirtualAnalyst
        '
        Me.ContextMenuVirtualAnalyst.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuVirtualAnalyst.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemDictationMode, Me.ToolStripMenuItemQuietMode})
        Me.ContextMenuVirtualAnalyst.Name = "ContextMenuVirtualAnalyst"
        Me.ContextMenuVirtualAnalyst.Size = New System.Drawing.Size(157, 48)
        '
        'ToolStripMenuItemDictationMode
        '
        Me.ToolStripMenuItemDictationMode.Name = "ToolStripMenuItemDictationMode"
        Me.ToolStripMenuItemDictationMode.Size = New System.Drawing.Size(156, 22)
        Me.ToolStripMenuItemDictationMode.Text = "&Dictation Mode"
        '
        'ToolStripMenuItemQuietMode
        '
        Me.ToolStripMenuItemQuietMode.Name = "ToolStripMenuItemQuietMode"
        Me.ToolStripMenuItemQuietMode.Size = New System.Drawing.Size(156, 22)
        Me.ToolStripMenuItemQuietMode.Text = "&Quiet Mode"
        '
        'TimerInput
        '
        Me.TimerInput.Interval = 1000
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabelMain})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 202)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(562, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabelMain
        '
        Me.StatusLabelMain.ForeColor = System.Drawing.Color.DarkGray
        Me.StatusLabelMain.Name = "StatusLabelMain"
        Me.StatusLabelMain.Size = New System.Drawing.Size(94, 17)
        Me.StatusLabelMain.Text = "StatusLabelMain"
        '
        'ContextMenuStripBrainBox
        '
        Me.ContextMenuStripBrainBox.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripBrainBox.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem})
        Me.ContextMenuStripBrainBox.Name = "ContextMenuStripBrainBox"
        Me.ContextMenuStripBrainBox.Size = New System.Drawing.Size(103, 26)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(102, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'frmToolboxBrainBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(562, 224)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmToolboxBrainBox"
        Me.TabText = "Virtual Analyst"
        Me.Text = "Virtual Analyst"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuVirtualAnalyst.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ContextMenuStripBrainBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox_Output As System.Windows.Forms.RichTextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ListBoxEnterpriseAware As System.Windows.Forms.ListBox
    Friend WithEvents ContextMenuVirtualAnalyst As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemDictationMode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerInput As System.Windows.Forms.Timer
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabelMain As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItemQuietMode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TextBoxInput As System.Windows.Forms.RichTextBox
    Friend WithEvents ContextMenuStripBrainBox As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
