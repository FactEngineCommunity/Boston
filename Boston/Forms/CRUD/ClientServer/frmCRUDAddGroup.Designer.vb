<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDAddGroup
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBoxGroupName = New System.Windows.Forms.TextBox()
        Me.LabelPromptGroupName = New System.Windows.Forms.Label()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TextBoxGroupName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptGroupName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(387, 62)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'TextBoxGroupName
        '
        Me.TextBoxGroupName.Location = New System.Drawing.Point(82, 18)
        Me.TextBoxGroupName.Name = "TextBoxGroupName"
        Me.TextBoxGroupName.Size = New System.Drawing.Size(262, 20)
        Me.TextBoxGroupName.TabIndex = 1
        '
        'LabelPromptGroupName
        '
        Me.LabelPromptGroupName.AutoSize = True
        Me.LabelPromptGroupName.Location = New System.Drawing.Point(6, 21)
        Me.LabelPromptGroupName.Name = "LabelPromptGroupName"
        Me.LabelPromptGroupName.Size = New System.Drawing.Size(70, 13)
        Me.LabelPromptGroupName.TabIndex = 0
        Me.LabelPromptGroupName.Text = "Group Name:"
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(420, 20)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(69, 24)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "&OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(420, 50)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(69, 24)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'frmCRUDAddGroup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(574, 278)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDAddGroup"
        Me.TabText = "Add Group"
        Me.Text = "Add Group"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxGroupName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptGroupName As System.Windows.Forms.Label
    Friend WithEvents ButtonOkay As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
End Class
