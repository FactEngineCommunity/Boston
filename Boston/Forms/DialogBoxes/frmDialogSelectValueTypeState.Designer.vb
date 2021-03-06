<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDialogSelectValueTypeState
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
        Me.LabelPromptValueTypeState = New System.Windows.Forms.Label()
        Me.LabelDescription = New System.Windows.Forms.Label()
        Me.ComboBoxState = New System.Windows.Forms.ComboBox()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonIllCreateMyOwn = New System.Windows.Forms.Button()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelPromptValueTypeState
        '
        Me.LabelPromptValueTypeState.AutoSize = True
        Me.LabelPromptValueTypeState.Location = New System.Drawing.Point(13, 19)
        Me.LabelPromptValueTypeState.Name = "LabelPromptValueTypeState"
        Me.LabelPromptValueTypeState.Size = New System.Drawing.Size(92, 13)
        Me.LabelPromptValueTypeState.TabIndex = 0
        Me.LabelPromptValueTypeState.Text = "Value Type State:"
        '
        'LabelDescription
        '
        Me.LabelDescription.AutoSize = True
        Me.LabelDescription.Location = New System.Drawing.Point(13, 50)
        Me.LabelDescription.Name = "LabelDescription"
        Me.LabelDescription.Size = New System.Drawing.Size(232, 13)
        Me.LabelDescription.TabIndex = 1
        Me.LabelDescription.Text = "NB States are Value Constraints of Value Types"
        '
        'ComboBoxState
        '
        Me.ComboBoxState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxState.FormattingEnabled = True
        Me.ComboBoxState.Location = New System.Drawing.Point(111, 16)
        Me.ComboBoxState.Name = "ComboBoxState"
        Me.ComboBoxState.Size = New System.Drawing.Size(340, 21)
        Me.ComboBoxState.TabIndex = 2
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(16, 82)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 3
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonIllCreateMyOwn
        '
        Me.ButtonIllCreateMyOwn.Location = New System.Drawing.Point(97, 82)
        Me.ButtonIllCreateMyOwn.Name = "ButtonIllCreateMyOwn"
        Me.ButtonIllCreateMyOwn.Size = New System.Drawing.Size(131, 23)
        Me.ButtonIllCreateMyOwn.TabIndex = 4
        Me.ButtonIllCreateMyOwn.Text = "I'll create my own State Name"
        Me.ButtonIllCreateMyOwn.UseVisualStyleBackColor = True
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(235, 82)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 5
        Me.ButtonOK.Text = "&OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'frmDialogSelectValueTypeState
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(470, 113)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.ButtonIllCreateMyOwn)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ComboBoxState)
        Me.Controls.Add(Me.LabelDescription)
        Me.Controls.Add(Me.LabelPromptValueTypeState)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmDialogSelectValueTypeState"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptValueTypeState As Label
    Friend WithEvents LabelDescription As Label
    Friend WithEvents ComboBoxState As ComboBox
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents ButtonIllCreateMyOwn As Button
    Friend WithEvents ButtonOK As Button
End Class
