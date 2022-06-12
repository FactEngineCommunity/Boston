<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxClientServerBroadcastTester
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
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.ComboBoxBroadcastType = New System.Windows.Forms.ComboBox()
        Me.LabelPromptBroadcastType = New System.Windows.Forms.Label()
        Me.TextBoxClientServerResponce = New System.Windows.Forms.TextBox()
        Me.LabelPromptResponce = New System.Windows.Forms.Label()
        Me.TextBoxFEKLStatement = New System.Windows.Forms.TextBox()
        Me.LabelPromptFEKLStatement = New System.Windows.Forms.Label()
        Me.TextBoxModelName = New System.Windows.Forms.TextBox()
        Me.LabelPromptModelName = New System.Windows.Forms.Label()
        Me.TextBoxModelId = New System.Windows.Forms.TextBox()
        Me.LabelPromptModelId = New System.Windows.Forms.Label()
        Me.ButtonSendBroadcast = New System.Windows.Forms.Button()
        Me.GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.ButtonSendBroadcast)
        Me.GroupBox.Controls.Add(Me.ComboBoxBroadcastType)
        Me.GroupBox.Controls.Add(Me.LabelPromptBroadcastType)
        Me.GroupBox.Controls.Add(Me.TextBoxClientServerResponce)
        Me.GroupBox.Controls.Add(Me.LabelPromptResponce)
        Me.GroupBox.Controls.Add(Me.TextBoxFEKLStatement)
        Me.GroupBox.Controls.Add(Me.LabelPromptFEKLStatement)
        Me.GroupBox.Controls.Add(Me.TextBoxModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelName)
        Me.GroupBox.Controls.Add(Me.TextBoxModelId)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelId)
        Me.GroupBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(804, 597)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'ComboBoxBroadcastType
        '
        Me.ComboBoxBroadcastType.FormattingEnabled = True
        Me.ComboBoxBroadcastType.Location = New System.Drawing.Point(103, 81)
        Me.ComboBoxBroadcastType.Name = "ComboBoxBroadcastType"
        Me.ComboBoxBroadcastType.Size = New System.Drawing.Size(345, 21)
        Me.ComboBoxBroadcastType.TabIndex = 9
        '
        'LabelPromptBroadcastType
        '
        Me.LabelPromptBroadcastType.AutoSize = True
        Me.LabelPromptBroadcastType.Location = New System.Drawing.Point(12, 84)
        Me.LabelPromptBroadcastType.Name = "LabelPromptBroadcastType"
        Me.LabelPromptBroadcastType.Size = New System.Drawing.Size(85, 13)
        Me.LabelPromptBroadcastType.TabIndex = 8
        Me.LabelPromptBroadcastType.Text = "Broadcast Type:"
        '
        'TextBoxClientServerResponce
        '
        Me.TextBoxClientServerResponce.Location = New System.Drawing.Point(15, 373)
        Me.TextBoxClientServerResponce.Multiline = True
        Me.TextBoxClientServerResponce.Name = "TextBoxClientServerResponce"
        Me.TextBoxClientServerResponce.Size = New System.Drawing.Size(773, 208)
        Me.TextBoxClientServerResponce.TabIndex = 7
        '
        'LabelPromptResponce
        '
        Me.LabelPromptResponce.AutoSize = True
        Me.LabelPromptResponce.Location = New System.Drawing.Point(12, 357)
        Me.LabelPromptResponce.Name = "LabelPromptResponce"
        Me.LabelPromptResponce.Size = New System.Drawing.Size(119, 13)
        Me.LabelPromptResponce.TabIndex = 6
        Me.LabelPromptResponce.Text = "Client Server Responce"
        '
        'TextBoxFEKLStatement
        '
        Me.TextBoxFEKLStatement.Location = New System.Drawing.Point(15, 126)
        Me.TextBoxFEKLStatement.Multiline = True
        Me.TextBoxFEKLStatement.Name = "TextBoxFEKLStatement"
        Me.TextBoxFEKLStatement.Size = New System.Drawing.Size(773, 208)
        Me.TextBoxFEKLStatement.TabIndex = 5
        '
        'LabelPromptFEKLStatement
        '
        Me.LabelPromptFEKLStatement.AutoSize = True
        Me.LabelPromptFEKLStatement.Location = New System.Drawing.Point(12, 109)
        Me.LabelPromptFEKLStatement.Name = "LabelPromptFEKLStatement"
        Me.LabelPromptFEKLStatement.Size = New System.Drawing.Size(219, 13)
        Me.LabelPromptFEKLStatement.TabIndex = 4
        Me.LabelPromptFEKLStatement.Text = "FactEngine Knowledge Language Statement"
        '
        'TextBoxModelName
        '
        Me.TextBoxModelName.Location = New System.Drawing.Point(88, 49)
        Me.TextBoxModelName.Name = "TextBoxModelName"
        Me.TextBoxModelName.Size = New System.Drawing.Size(360, 20)
        Me.TextBoxModelName.TabIndex = 3
        '
        'LabelPromptModelName
        '
        Me.LabelPromptModelName.AutoSize = True
        Me.LabelPromptModelName.Location = New System.Drawing.Point(12, 52)
        Me.LabelPromptModelName.Name = "LabelPromptModelName"
        Me.LabelPromptModelName.Size = New System.Drawing.Size(70, 13)
        Me.LabelPromptModelName.TabIndex = 2
        Me.LabelPromptModelName.Text = "Model Name:"
        '
        'TextBoxModelId
        '
        Me.TextBoxModelId.Location = New System.Drawing.Point(88, 19)
        Me.TextBoxModelId.Name = "TextBoxModelId"
        Me.TextBoxModelId.Size = New System.Drawing.Size(360, 20)
        Me.TextBoxModelId.TabIndex = 1
        '
        'LabelPromptModelId
        '
        Me.LabelPromptModelId.AutoSize = True
        Me.LabelPromptModelId.Location = New System.Drawing.Point(31, 22)
        Me.LabelPromptModelId.Name = "LabelPromptModelId"
        Me.LabelPromptModelId.Size = New System.Drawing.Size(51, 13)
        Me.LabelPromptModelId.TabIndex = 0
        Me.LabelPromptModelId.Text = "Model Id:"
        '
        'ButtonSendBroadcast
        '
        Me.ButtonSendBroadcast.Location = New System.Drawing.Point(690, 19)
        Me.ButtonSendBroadcast.Name = "ButtonSendBroadcast"
        Me.ButtonSendBroadcast.Size = New System.Drawing.Size(98, 20)
        Me.ButtonSendBroadcast.TabIndex = 10
        Me.ButtonSendBroadcast.Text = "&Send Broadcast"
        Me.ButtonSendBroadcast.UseVisualStyleBackColor = True
        '
        'frmToolboxClientServerBroadcastTester
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(804, 597)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmToolboxClientServerBroadcastTester"
        Me.Text = "Client Server Broadcast Tester"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents TextBoxModelName As TextBox
    Friend WithEvents LabelPromptModelName As Label
    Friend WithEvents TextBoxModelId As TextBox
    Friend WithEvents LabelPromptModelId As Label
    Friend WithEvents LabelPromptResponce As Label
    Friend WithEvents TextBoxFEKLStatement As TextBox
    Friend WithEvents LabelPromptFEKLStatement As Label
    Friend WithEvents LabelPromptBroadcastType As Label
    Friend WithEvents TextBoxClientServerResponce As TextBox
    Friend WithEvents ComboBoxBroadcastType As ComboBox
    Friend WithEvents ButtonSendBroadcast As Button
End Class
