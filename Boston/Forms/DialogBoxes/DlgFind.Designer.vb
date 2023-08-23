<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DlgFind
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextboxFindWhat = New System.Windows.Forms.TextBox()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkWholeWord = New System.Windows.Forms.CheckBox()
        Me.chkMatchCase = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optDown = New System.Windows.Forms.RadioButton()
        Me.optUp = New System.Windows.Forms.RadioButton()
        Me.ButtonReplace = New System.Windows.Forms.Button()
        Me.TextBoxReplaceWith = New System.Windows.Forms.TextBox()
        Me.LabelPromptReplaceWith = New System.Windows.Forms.Label()
        Me.ButtonReplaceAll = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Find what:"
        '
        'TextboxFindWhat
        '
        Me.TextboxFindWhat.Location = New System.Drawing.Point(90, 10)
        Me.TextboxFindWhat.Name = "TextboxFindWhat"
        Me.TextboxFindWhat.Size = New System.Drawing.Size(265, 21)
        Me.TextboxFindWhat.TabIndex = 0
        '
        'btnFind
        '
        Me.btnFind.Enabled = False
        Me.btnFind.Location = New System.Drawing.Point(361, 6)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(71, 23)
        Me.btnFind.TabIndex = 6
        Me.btnFind.Text = "Find &Next"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(361, 89)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(71, 23)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Close"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'chkWholeWord
        '
        Me.chkWholeWord.AutoSize = True
        Me.chkWholeWord.Location = New System.Drawing.Point(15, 66)
        Me.chkWholeWord.Name = "chkWholeWord"
        Me.chkWholeWord.Size = New System.Drawing.Size(136, 17)
        Me.chkWholeWord.TabIndex = 2
        Me.chkWholeWord.Text = "Match &whole word only"
        Me.chkWholeWord.UseVisualStyleBackColor = True
        '
        'chkMatchCase
        '
        Me.chkMatchCase.AutoSize = True
        Me.chkMatchCase.Location = New System.Drawing.Point(15, 89)
        Me.chkMatchCase.Name = "chkMatchCase"
        Me.chkMatchCase.Size = New System.Drawing.Size(80, 17)
        Me.chkMatchCase.TabIndex = 3
        Me.chkMatchCase.Text = "Match &case"
        Me.chkMatchCase.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optDown)
        Me.GroupBox1.Controls.Add(Me.optUp)
        Me.GroupBox1.Location = New System.Drawing.Point(158, 64)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(103, 40)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Direction"
        '
        'optDown
        '
        Me.optDown.AutoSize = True
        Me.optDown.Checked = True
        Me.optDown.Location = New System.Drawing.Point(46, 17)
        Me.optDown.Name = "optDown"
        Me.optDown.Size = New System.Drawing.Size(52, 17)
        Me.optDown.TabIndex = 5
        Me.optDown.TabStop = True
        Me.optDown.Text = "&Down"
        Me.optDown.UseVisualStyleBackColor = True
        '
        'optUp
        '
        Me.optUp.AutoSize = True
        Me.optUp.Location = New System.Drawing.Point(7, 17)
        Me.optUp.Name = "optUp"
        Me.optUp.Size = New System.Drawing.Size(38, 17)
        Me.optUp.TabIndex = 4
        Me.optUp.Text = "&Up"
        Me.optUp.UseVisualStyleBackColor = True
        '
        'ButtonReplace
        '
        Me.ButtonReplace.Location = New System.Drawing.Point(361, 35)
        Me.ButtonReplace.Name = "ButtonReplace"
        Me.ButtonReplace.Size = New System.Drawing.Size(71, 23)
        Me.ButtonReplace.TabIndex = 7
        Me.ButtonReplace.Text = "&Replace"
        Me.ButtonReplace.UseVisualStyleBackColor = True
        '
        'TextBoxReplaceWith
        '
        Me.TextBoxReplaceWith.Location = New System.Drawing.Point(90, 37)
        Me.TextBoxReplaceWith.Name = "TextBoxReplaceWith"
        Me.TextBoxReplaceWith.Size = New System.Drawing.Size(265, 21)
        Me.TextBoxReplaceWith.TabIndex = 1
        '
        'LabelPromptReplaceWith
        '
        Me.LabelPromptReplaceWith.AutoSize = True
        Me.LabelPromptReplaceWith.Location = New System.Drawing.Point(12, 40)
        Me.LabelPromptReplaceWith.Name = "LabelPromptReplaceWith"
        Me.LabelPromptReplaceWith.Size = New System.Drawing.Size(72, 13)
        Me.LabelPromptReplaceWith.TabIndex = 8
        Me.LabelPromptReplaceWith.Text = "Replace with:"
        '
        'ButtonReplaceAll
        '
        Me.ButtonReplaceAll.Location = New System.Drawing.Point(361, 60)
        Me.ButtonReplaceAll.Name = "ButtonReplaceAll"
        Me.ButtonReplaceAll.Size = New System.Drawing.Size(71, 23)
        Me.ButtonReplaceAll.TabIndex = 8
        Me.ButtonReplaceAll.Text = "Replace &All"
        Me.ButtonReplaceAll.UseVisualStyleBackColor = True
        '
        'DlgFind
        '
        Me.AcceptButton = Me.btnFind
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(444, 117)
        Me.Controls.Add(Me.ButtonReplaceAll)
        Me.Controls.Add(Me.ButtonReplace)
        Me.Controls.Add(Me.TextBoxReplaceWith)
        Me.Controls.Add(Me.LabelPromptReplaceWith)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkMatchCase)
        Me.Controls.Add(Me.chkWholeWord)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.TextboxFindWhat)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DlgFind"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Find"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextboxFindWhat As System.Windows.Forms.TextBox
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkWholeWord As System.Windows.Forms.CheckBox
    Friend WithEvents chkMatchCase As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optDown As System.Windows.Forms.RadioButton
    Friend WithEvents optUp As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonReplace As Button
    Friend WithEvents TextBoxReplaceWith As TextBox
    Friend WithEvents LabelPromptReplaceWith As Label
    Friend WithEvents ButtonReplaceAll As Button
End Class
