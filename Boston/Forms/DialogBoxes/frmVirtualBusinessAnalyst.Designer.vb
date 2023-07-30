<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVirtualBusinessAnalyst
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
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelModel = New System.Windows.Forms.Label()
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox()
        Me.TextBoxResponse = New System.Windows.Forms.TextBox()
        Me.TextBoxPrompt = New System.Windows.Forms.TextBox()
        Me.LabelPromptPrompt = New System.Windows.Forms.Label()
        Me.ButtonGO = New System.Windows.Forms.Button()
        Me.ButtonAskQuestions = New System.Windows.Forms.Button()
        Me.LabelPromptQuestionCountLimit = New System.Windows.Forms.Label()
        Me.NumericUpDownQuestionCountLimit = New System.Windows.Forms.NumericUpDown()
        Me.LabelPromptAskQuestions = New System.Windows.Forms.Label()
        Me.ButtonEraser = New System.Windows.Forms.Button()
        Me.ContextMenuStripBusinessAnalyst = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SpeakSelectedTextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBoxMain.SuspendLayout()
        CType(Me.NumericUpDownQuestionCountLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripBusinessAnalyst.SuspendLayout()
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
        Me.LabelModel.Size = New System.Drawing.Size(62, 13)
        Me.LabelModel.TabIndex = 1
        Me.LabelModel.Text = "LabelModel"
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxMain.Controls.Add(Me.TextBoxResponse)
        Me.GroupBoxMain.Location = New System.Drawing.Point(15, 36)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(773, 340)
        Me.GroupBoxMain.TabIndex = 2
        Me.GroupBoxMain.TabStop = False
        '
        'TextBoxResponse
        '
        Me.TextBoxResponse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxResponse.ContextMenuStrip = Me.ContextMenuStripBusinessAnalyst
        Me.TextBoxResponse.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxResponse.Location = New System.Drawing.Point(6, 15)
        Me.TextBoxResponse.Multiline = True
        Me.TextBoxResponse.Name = "TextBoxResponse"
        Me.TextBoxResponse.Size = New System.Drawing.Size(761, 315)
        Me.TextBoxResponse.TabIndex = 0
        '
        'TextBoxPrompt
        '
        Me.TextBoxPrompt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TextBoxPrompt.Location = New System.Drawing.Point(60, 401)
        Me.TextBoxPrompt.Multiline = True
        Me.TextBoxPrompt.Name = "TextBoxPrompt"
        Me.TextBoxPrompt.Size = New System.Drawing.Size(460, 36)
        Me.TextBoxPrompt.TabIndex = 3
        '
        'LabelPromptPrompt
        '
        Me.LabelPromptPrompt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptPrompt.AutoSize = True
        Me.LabelPromptPrompt.Location = New System.Drawing.Point(12, 401)
        Me.LabelPromptPrompt.Name = "LabelPromptPrompt"
        Me.LabelPromptPrompt.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptPrompt.TabIndex = 4
        Me.LabelPromptPrompt.Text = "Prompt:"
        '
        'ButtonGO
        '
        Me.ButtonGO.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonGO.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.ButtonGO.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonGO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonGO.ForeColor = System.Drawing.Color.White
        Me.ButtonGO.Location = New System.Drawing.Point(526, 401)
        Me.ButtonGO.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonGO.Name = "ButtonGO"
        Me.ButtonGO.Size = New System.Drawing.Size(39, 23)
        Me.ButtonGO.TabIndex = 5
        Me.ButtonGO.Text = "&GO"
        Me.ButtonGO.UseVisualStyleBackColor = False
        '
        'ButtonAskQuestions
        '
        Me.ButtonAskQuestions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonAskQuestions.Location = New System.Drawing.Point(571, 401)
        Me.ButtonAskQuestions.Name = "ButtonAskQuestions"
        Me.ButtonAskQuestions.Size = New System.Drawing.Size(171, 23)
        Me.ButtonAskQuestions.TabIndex = 6
        Me.ButtonAskQuestions.Text = "Ask &Questions about the Model"
        Me.ButtonAskQuestions.UseVisualStyleBackColor = True
        '
        'LabelPromptQuestionCountLimit
        '
        Me.LabelPromptQuestionCountLimit.AutoSize = True
        Me.LabelPromptQuestionCountLimit.Location = New System.Drawing.Point(480, 9)
        Me.LabelPromptQuestionCountLimit.Name = "LabelPromptQuestionCountLimit"
        Me.LabelPromptQuestionCountLimit.Size = New System.Drawing.Size(107, 13)
        Me.LabelPromptQuestionCountLimit.TabIndex = 7
        Me.LabelPromptQuestionCountLimit.Text = "Question Count Limit:"
        '
        'NumericUpDownQuestionCountLimit
        '
        Me.NumericUpDownQuestionCountLimit.Location = New System.Drawing.Point(594, 9)
        Me.NumericUpDownQuestionCountLimit.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.NumericUpDownQuestionCountLimit.Name = "NumericUpDownQuestionCountLimit"
        Me.NumericUpDownQuestionCountLimit.Size = New System.Drawing.Size(34, 20)
        Me.NumericUpDownQuestionCountLimit.TabIndex = 8
        Me.NumericUpDownQuestionCountLimit.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'LabelPromptAskQuestions
        '
        Me.LabelPromptAskQuestions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptAskQuestions.AutoSize = True
        Me.LabelPromptAskQuestions.Location = New System.Drawing.Point(62, 381)
        Me.LabelPromptAskQuestions.Name = "LabelPromptAskQuestions"
        Me.LabelPromptAskQuestions.Size = New System.Drawing.Size(155, 13)
        Me.LabelPromptAskQuestions.TabIndex = 9
        Me.LabelPromptAskQuestions.Text = "Ask questions about the model."
        '
        'ButtonEraser
        '
        Me.ButtonEraser.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonEraser.Image = Global.Boston.My.Resources.MenuImages.Eraser_16x16
        Me.ButtonEraser.Location = New System.Drawing.Point(763, 12)
        Me.ButtonEraser.Name = "ButtonEraser"
        Me.ButtonEraser.Size = New System.Drawing.Size(25, 23)
        Me.ButtonEraser.TabIndex = 10
        Me.ButtonEraser.UseVisualStyleBackColor = True
        '
        'ContextMenuStripBusinessAnalyst
        '
        Me.ContextMenuStripBusinessAnalyst.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SpeakSelectedTextToolStripMenuItem})
        Me.ContextMenuStripBusinessAnalyst.Name = "ContextMenuStripBusinessAnalyst"
        Me.ContextMenuStripBusinessAnalyst.Size = New System.Drawing.Size(175, 26)
        '
        'SpeakSelectedTextToolStripMenuItem
        '
        Me.SpeakSelectedTextToolStripMenuItem.Name = "SpeakSelectedTextToolStripMenuItem"
        Me.SpeakSelectedTextToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.SpeakSelectedTextToolStripMenuItem.Text = "Speak selected text"
        '
        'frmVirtualBusinessAnalyst
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.ButtonEraser)
        Me.Controls.Add(Me.LabelPromptAskQuestions)
        Me.Controls.Add(Me.NumericUpDownQuestionCountLimit)
        Me.Controls.Add(Me.LabelPromptQuestionCountLimit)
        Me.Controls.Add(Me.ButtonAskQuestions)
        Me.Controls.Add(Me.ButtonGO)
        Me.Controls.Add(Me.LabelPromptPrompt)
        Me.Controls.Add(Me.TextBoxPrompt)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Controls.Add(Me.LabelModel)
        Me.Controls.Add(Me.LabelPromptModel)
        Me.Name = "frmVirtualBusinessAnalyst"
        Me.Text = "Virtual BA"
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        CType(Me.NumericUpDownQuestionCountLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripBusinessAnalyst.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents LabelModel As Label
    Friend WithEvents GroupBoxMain As GroupBox
    Friend WithEvents TextBoxResponse As TextBox
    Friend WithEvents TextBoxPrompt As TextBox
    Friend WithEvents LabelPromptPrompt As Label
    Friend WithEvents ButtonGO As Button
    Friend WithEvents ButtonAskQuestions As Button
    Friend WithEvents LabelPromptQuestionCountLimit As Label
    Friend WithEvents NumericUpDownQuestionCountLimit As NumericUpDown
    Friend WithEvents LabelPromptAskQuestions As Label
    Friend WithEvents ButtonEraser As Button
    Friend WithEvents ContextMenuStripBusinessAnalyst As ContextMenuStrip
    Friend WithEvents SpeakSelectedTextToolStripMenuItem As ToolStripMenuItem
End Class
