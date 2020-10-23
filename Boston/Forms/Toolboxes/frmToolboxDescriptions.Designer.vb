<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolboxDescriptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxDescriptions))
        Me.LabelPromptModelElement = New System.Windows.Forms.Label()
        Me.LabelModelElementName = New System.Windows.Forms.Label()
        Me.LabelPromptShortDescription = New System.Windows.Forms.Label()
        Me.LabelPromptLongDescription = New System.Windows.Forms.Label()
        Me.TextBoxShortDescription = New System.Windows.Forms.TextBox()
        Me.TextBoxLongDescription = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LabelPromptModelElement
        '
        Me.LabelPromptModelElement.AutoSize = True
        Me.LabelPromptModelElement.Location = New System.Drawing.Point(10, 9)
        Me.LabelPromptModelElement.Name = "LabelPromptModelElement"
        Me.LabelPromptModelElement.Size = New System.Drawing.Size(125, 13)
        Me.LabelPromptModelElement.TabIndex = 12
        Me.LabelPromptModelElement.Text = "Selected Model Element:"
        '
        'LabelModelElementName
        '
        Me.LabelModelElementName.AutoSize = True
        Me.LabelModelElementName.Location = New System.Drawing.Point(138, 9)
        Me.LabelModelElementName.Margin = New System.Windows.Forms.Padding(0)
        Me.LabelModelElementName.Name = "LabelModelElementName"
        Me.LabelModelElementName.Size = New System.Drawing.Size(154, 13)
        Me.LabelModelElementName.TabIndex = 13
        Me.LabelModelElementName.Text = " <No Model Element Selected>"
        '
        'LabelPromptShortDescription
        '
        Me.LabelPromptShortDescription.AutoSize = True
        Me.LabelPromptShortDescription.Location = New System.Drawing.Point(10, 31)
        Me.LabelPromptShortDescription.Name = "LabelPromptShortDescription"
        Me.LabelPromptShortDescription.Size = New System.Drawing.Size(91, 13)
        Me.LabelPromptShortDescription.TabIndex = 14
        Me.LabelPromptShortDescription.Text = "Short Description:"
        '
        'LabelPromptLongDescription
        '
        Me.LabelPromptLongDescription.AutoSize = True
        Me.LabelPromptLongDescription.Location = New System.Drawing.Point(10, 76)
        Me.LabelPromptLongDescription.Name = "LabelPromptLongDescription"
        Me.LabelPromptLongDescription.Size = New System.Drawing.Size(90, 13)
        Me.LabelPromptLongDescription.TabIndex = 15
        Me.LabelPromptLongDescription.Text = "Long Description:"
        '
        'TextBoxShortDescription
        '
        Me.TextBoxShortDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxShortDescription.Location = New System.Drawing.Point(13, 47)
        Me.TextBoxShortDescription.Name = "TextBoxShortDescription"
        Me.TextBoxShortDescription.Size = New System.Drawing.Size(775, 20)
        Me.TextBoxShortDescription.TabIndex = 16
        '
        'TextBoxLongDescription
        '
        Me.TextBoxLongDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxLongDescription.Location = New System.Drawing.Point(13, 92)
        Me.TextBoxLongDescription.Multiline = True
        Me.TextBoxLongDescription.Name = "TextBoxLongDescription"
        Me.TextBoxLongDescription.Size = New System.Drawing.Size(775, 66)
        Me.TextBoxLongDescription.TabIndex = 17
        '
        'frmToolboxDescriptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 180)
        Me.Controls.Add(Me.TextBoxLongDescription)
        Me.Controls.Add(Me.TextBoxShortDescription)
        Me.Controls.Add(Me.LabelPromptLongDescription)
        Me.Controls.Add(Me.LabelPromptShortDescription)
        Me.Controls.Add(Me.LabelPromptModelElement)
        Me.Controls.Add(Me.LabelModelElementName)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxDescriptions"
        Me.Text = "Description"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptModelElement As Label
    Friend WithEvents LabelModelElementName As Label
    Friend WithEvents LabelPromptShortDescription As Label
    Friend WithEvents LabelPromptLongDescription As Label
    Friend WithEvents TextBoxShortDescription As TextBox
    Friend WithEvents TextBoxLongDescription As TextBox
End Class
