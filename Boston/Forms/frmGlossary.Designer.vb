<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGlossary
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.CheckBoxShowGeneralConcepts = New System.Windows.Forms.CheckBox()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.WebBrowser = New System.Windows.Forms.WebBrowser()
        Me.ButtonGenerateHTMLGlossary = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonGenerateHTMLGlossary)
        Me.SplitContainer1.Panel1.Controls.Add(Me.CheckBoxShowGeneralConcepts)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelModelName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPromptModel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextBox1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(884, 574)
        Me.SplitContainer1.SplitterDistance = 289
        Me.SplitContainer1.TabIndex = 0
        '
        'CheckBoxShowGeneralConcepts
        '
        Me.CheckBoxShowGeneralConcepts.AutoSize = True
        Me.CheckBoxShowGeneralConcepts.Location = New System.Drawing.Point(12, 69)
        Me.CheckBoxShowGeneralConcepts.Name = "CheckBoxShowGeneralConcepts"
        Me.CheckBoxShowGeneralConcepts.Size = New System.Drawing.Size(141, 17)
        Me.CheckBoxShowGeneralConcepts.TabIndex = 4
        Me.CheckBoxShowGeneralConcepts.Text = "Show &General Concepts"
        Me.CheckBoxShowGeneralConcepts.UseVisualStyleBackColor = True
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelModelName.Location = New System.Drawing.Point(57, 16)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(104, 13)
        Me.LabelModelName.TabIndex = 3
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(12, 16)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 2
        Me.LabelPromptModel.Text = "Model:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(12, 43)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(195, 20)
        Me.TextBox1.TabIndex = 1
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(12, 95)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(262, 433)
        Me.ListBox1.Sorted = True
        Me.ListBox1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.WebBrowser)
        Me.SplitContainer2.Size = New System.Drawing.Size(591, 574)
        Me.SplitContainer2.SplitterDistance = 358
        Me.SplitContainer2.TabIndex = 0
        '
        'WebBrowser
        '
        Me.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser.Name = "WebBrowser"
        Me.WebBrowser.Size = New System.Drawing.Size(591, 358)
        Me.WebBrowser.TabIndex = 2
        '
        'ButtonGenerateHTMLGlossary
        '
        Me.ButtonGenerateHTMLGlossary.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonGenerateHTMLGlossary.Location = New System.Drawing.Point(12, 534)
        Me.ButtonGenerateHTMLGlossary.Name = "ButtonGenerateHTMLGlossary"
        Me.ButtonGenerateHTMLGlossary.Size = New System.Drawing.Size(141, 23)
        Me.ButtonGenerateHTMLGlossary.TabIndex = 5
        Me.ButtonGenerateHTMLGlossary.Text = "Generate &HTML Glossary"
        Me.ButtonGenerateHTMLGlossary.UseVisualStyleBackColor = True
        '
        'frmGlossary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 574)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "frmGlossary"
        Me.TabText = "Glossary"
        Me.Text = "Glossary"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents WebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents LabelModelName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptModel As System.Windows.Forms.Label
    Friend WithEvents CheckBoxShowGeneralConcepts As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonGenerateHTMLGlossary As Button
End Class
