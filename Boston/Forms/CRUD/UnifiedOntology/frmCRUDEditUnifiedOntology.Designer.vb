<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDEditUnifiedOntology
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.ButtonFileSelect = New System.Windows.Forms.Button()
        Me.TextBoxImageFileLocation = New System.Windows.Forms.TextBox()
        Me.LabelPromptImageFileLocation = New System.Windows.Forms.Label()
        Me.TextBoxUnifiedOntologyName = New System.Windows.Forms.TextBox()
        Me.LabelPromptUnifiedOntologyName = New System.Windows.Forms.Label()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.ButtonFileSelect)
        Me.GroupBox.Controls.Add(Me.TextBoxImageFileLocation)
        Me.GroupBox.Controls.Add(Me.LabelPromptImageFileLocation)
        Me.GroupBox.Controls.Add(Me.TextBoxUnifiedOntologyName)
        Me.GroupBox.Controls.Add(Me.LabelPromptUnifiedOntologyName)
        Me.GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(529, 98)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'ButtonFileSelect
        '
        Me.ButtonFileSelect.BackColor = System.Drawing.Color.White
        Me.ButtonFileSelect.BackgroundImage = Global.Boston.My.Resources.Resources.folder16x16
        Me.ButtonFileSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonFileSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonFileSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFileSelect.ForeColor = System.Drawing.Color.White
        Me.ButtonFileSelect.Location = New System.Drawing.Point(490, 49)
        Me.ButtonFileSelect.Name = "ButtonFileSelect"
        Me.ButtonFileSelect.Size = New System.Drawing.Size(16, 16)
        Me.ButtonFileSelect.TabIndex = 9
        Me.ButtonFileSelect.UseVisualStyleBackColor = False
        '
        'TextBoxImageFileLocation
        '
        Me.TextBoxImageFileLocation.Location = New System.Drawing.Point(132, 48)
        Me.TextBoxImageFileLocation.Name = "TextBoxImageFileLocation"
        Me.TextBoxImageFileLocation.Size = New System.Drawing.Size(338, 20)
        Me.TextBoxImageFileLocation.TabIndex = 3
        '
        'LabelPromptImageFileLocation
        '
        Me.LabelPromptImageFileLocation.AutoSize = True
        Me.LabelPromptImageFileLocation.Location = New System.Drawing.Point(24, 48)
        Me.LabelPromptImageFileLocation.Name = "LabelPromptImageFileLocation"
        Me.LabelPromptImageFileLocation.Size = New System.Drawing.Size(102, 13)
        Me.LabelPromptImageFileLocation.TabIndex = 2
        Me.LabelPromptImageFileLocation.Text = "Image File Location:"
        '
        'TextBoxUnifiedOntologyName
        '
        Me.TextBoxUnifiedOntologyName.Location = New System.Drawing.Point(132, 17)
        Me.TextBoxUnifiedOntologyName.Name = "TextBoxUnifiedOntologyName"
        Me.TextBoxUnifiedOntologyName.Size = New System.Drawing.Size(338, 20)
        Me.TextBoxUnifiedOntologyName.TabIndex = 1
        '
        'LabelPromptUnifiedOntologyName
        '
        Me.LabelPromptUnifiedOntologyName.AutoSize = True
        Me.LabelPromptUnifiedOntologyName.Location = New System.Drawing.Point(7, 20)
        Me.LabelPromptUnifiedOntologyName.Name = "LabelPromptUnifiedOntologyName"
        Me.LabelPromptUnifiedOntologyName.Size = New System.Drawing.Size(119, 13)
        Me.LabelPromptUnifiedOntologyName.TabIndex = 0
        Me.LabelPromptUnifiedOntologyName.Text = "Unified Ontology Name:"
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(560, 19)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "&Okay"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(560, 48)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'frmCRUDAddUnifiedOntology
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(652, 125)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmCRUDAddUnifiedOntology"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Unified Ontology"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents TextBoxUnifiedOntologyName As TextBox
    Friend WithEvents LabelPromptUnifiedOntologyName As Label
    Friend WithEvents ButtonOkay As Button
    Friend WithEvents ButtonCancel As Button
    Friend WithEvents TextBoxImageFileLocation As TextBox
    Friend WithEvents LabelPromptImageFileLocation As Label
    Friend WithEvents ButtonFileSelect As Button
    Friend WithEvents ErrorProvider As ErrorProvider
End Class
