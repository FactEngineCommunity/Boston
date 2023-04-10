<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFEKLUploader
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
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModelName = New System.Windows.Forms.Label()
        Me.ButtonLoadIntoModel = New System.Windows.Forms.Button()
        Me.ButtonOpenFEKLFile = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.RichTextBoxFEKLDocument = New System.Windows.Forms.RichTextBox()
        Me.LabelPromptErrorType = New System.Windows.Forms.Label()
        Me.LabelErrorType = New System.Windows.Forms.Label()
        Me.LabelPromptErrorMessage = New System.Windows.Forms.Label()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.GroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox.Controls.Add(Me.LabelErrorMessage)
        Me.GroupBox.Controls.Add(Me.LabelPromptErrorMessage)
        Me.GroupBox.Controls.Add(Me.LabelErrorType)
        Me.GroupBox.Controls.Add(Me.LabelPromptErrorType)
        Me.GroupBox.Controls.Add(Me.RichTextBoxFEKLDocument)
        Me.GroupBox.Controls.Add(Me.LabelModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelName)
        Me.GroupBox.Controls.Add(Me.ButtonLoadIntoModel)
        Me.GroupBox.Controls.Add(Me.ButtonOpenFEKLFile)
        Me.GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(494, 496)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(55, 20)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(67, 13)
        Me.LabelModelName.TabIndex = 4
        Me.LabelModelName.Text = "Model Name"
        '
        'LabelPromptModelName
        '
        Me.LabelPromptModelName.AutoSize = True
        Me.LabelPromptModelName.Location = New System.Drawing.Point(9, 20)
        Me.LabelPromptModelName.Name = "LabelPromptModelName"
        Me.LabelPromptModelName.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModelName.TabIndex = 3
        Me.LabelPromptModelName.Text = "Model:"
        '
        'ButtonLoadIntoModel
        '
        Me.ButtonLoadIntoModel.Location = New System.Drawing.Point(107, 51)
        Me.ButtonLoadIntoModel.Name = "ButtonLoadIntoModel"
        Me.ButtonLoadIntoModel.Size = New System.Drawing.Size(99, 23)
        Me.ButtonLoadIntoModel.TabIndex = 1
        Me.ButtonLoadIntoModel.Text = "&Load into Model"
        Me.ButtonLoadIntoModel.UseVisualStyleBackColor = True
        '
        'ButtonOpenFEKLFile
        '
        Me.ButtonOpenFEKLFile.Location = New System.Drawing.Point(6, 51)
        Me.ButtonOpenFEKLFile.Name = "ButtonOpenFEKLFile"
        Me.ButtonOpenFEKLFile.Size = New System.Drawing.Size(95, 23)
        Me.ButtonOpenFEKLFile.TabIndex = 0
        Me.ButtonOpenFEKLFile.Text = "&Open FEKL File"
        Me.ButtonOpenFEKLFile.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.Location = New System.Drawing.Point(516, 22)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(73, 23)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'RichTextBoxFEKLDocument
        '
        Me.RichTextBoxFEKLDocument.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxFEKLDocument.Location = New System.Drawing.Point(6, 80)
        Me.RichTextBoxFEKLDocument.Name = "RichTextBoxFEKLDocument"
        Me.RichTextBoxFEKLDocument.Size = New System.Drawing.Size(480, 347)
        Me.RichTextBoxFEKLDocument.TabIndex = 5
        Me.RichTextBoxFEKLDocument.Text = ""
        '
        'LabelPromptErrorType
        '
        Me.LabelPromptErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorType.AutoSize = True
        Me.LabelPromptErrorType.Location = New System.Drawing.Point(7, 434)
        Me.LabelPromptErrorType.Name = "LabelPromptErrorType"
        Me.LabelPromptErrorType.Size = New System.Drawing.Size(59, 13)
        Me.LabelPromptErrorType.TabIndex = 6
        Me.LabelPromptErrorType.Text = "Error Type:"
        '
        'LabelErrorType
        '
        Me.LabelErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorType.AutoSize = True
        Me.LabelErrorType.Location = New System.Drawing.Point(72, 434)
        Me.LabelErrorType.Name = "LabelErrorType"
        Me.LabelErrorType.Size = New System.Drawing.Size(27, 13)
        Me.LabelErrorType.TabIndex = 7
        Me.LabelErrorType.Text = "N/A"
        '
        'LabelPromptErrorMessage
        '
        Me.LabelPromptErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorMessage.AutoSize = True
        Me.LabelPromptErrorMessage.Location = New System.Drawing.Point(7, 457)
        Me.LabelPromptErrorMessage.Name = "LabelPromptErrorMessage"
        Me.LabelPromptErrorMessage.Size = New System.Drawing.Size(78, 13)
        Me.LabelPromptErrorMessage.TabIndex = 8
        Me.LabelPromptErrorMessage.Text = "Error Message:"
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorMessage.Location = New System.Drawing.Point(92, 457)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(393, 36)
        Me.LabelErrorMessage.TabIndex = 9
        Me.LabelErrorMessage.Text = "N/A"
        '
        'frmFEKLUploader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(601, 510)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmFEKLUploader"
        Me.Text = "FEKL Uploader"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents LabelModelName As Label
    Friend WithEvents LabelPromptModelName As Label
    Friend WithEvents ButtonLoadIntoModel As Button
    Friend WithEvents ButtonOpenFEKLFile As Button
    Friend WithEvents ButtonClose As Button
    Friend WithEvents RichTextBoxFEKLDocument As RichTextBox
    Friend WithEvents LabelErrorMessage As Label
    Friend WithEvents LabelPromptErrorMessage As Label
    Friend WithEvents LabelErrorType As Label
    Friend WithEvents LabelPromptErrorType As Label
End Class
