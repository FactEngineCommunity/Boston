<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModelElementSelector
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
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.ComboBoxModel = New System.Windows.Forms.ComboBox()
        Me.LabelPromptModelElement = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox.Controls.Add(Me.ComboBox1)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelElement)
        Me.GroupBox.Controls.Add(Me.ComboBoxModel)
        Me.GroupBox.Controls.Add(Me.LabelPromptModel)
        Me.GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(725, 95)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(58, 22)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 0
        Me.LabelPromptModel.Text = "Model:"
        '
        'ComboBoxModel
        '
        Me.ComboBoxModel.FormattingEnabled = True
        Me.ComboBoxModel.Location = New System.Drawing.Point(103, 19)
        Me.ComboBoxModel.Name = "ComboBoxModel"
        Me.ComboBoxModel.Size = New System.Drawing.Size(413, 21)
        Me.ComboBoxModel.TabIndex = 1
        '
        'LabelPromptModelElement
        '
        Me.LabelPromptModelElement.AutoSize = True
        Me.LabelPromptModelElement.Location = New System.Drawing.Point(20, 59)
        Me.LabelPromptModelElement.Name = "LabelPromptModelElement"
        Me.LabelPromptModelElement.Size = New System.Drawing.Size(77, 13)
        Me.LabelPromptModelElement.TabIndex = 2
        Me.LabelPromptModelElement.Text = "ModelElement:"
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(103, 56)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(613, 21)
        Me.ComboBox1.TabIndex = 3
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOkay.Location = New System.Drawing.Point(743, 24)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "&Okay"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(743, 53)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'frmModelElementSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(826, 119)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmModelElementSelector"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Model Element Selector"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents LabelPromptModelElement As Label
    Friend WithEvents ComboBoxModel As ComboBox
    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents ButtonOkay As Button
    Friend WithEvents ButtonCancel As Button
End Class
