<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDocumentGeneratorSettings
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ButtonGenerateDocumentation = New System.Windows.Forms.Button()
        Me.CheckBoxDisplayPageImages = New System.Windows.Forms.CheckBox()
        Me.CheckBoxDisplayTermDiagram = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBoxDisplayTermDiagram)
        Me.GroupBox1.Controls.Add(Me.ButtonGenerateDocumentation)
        Me.GroupBox1.Controls.Add(Me.CheckBoxDisplayPageImages)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(368, 123)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'ButtonGenerateDocumentation
        '
        Me.ButtonGenerateDocumentation.Location = New System.Drawing.Point(104, 77)
        Me.ButtonGenerateDocumentation.Name = "ButtonGenerateDocumentation"
        Me.ButtonGenerateDocumentation.Size = New System.Drawing.Size(152, 23)
        Me.ButtonGenerateDocumentation.TabIndex = 1
        Me.ButtonGenerateDocumentation.Text = "&Generate Documentation"
        Me.ButtonGenerateDocumentation.UseVisualStyleBackColor = True
        '
        'CheckBoxDisplayPageImages
        '
        Me.CheckBoxDisplayPageImages.AutoSize = True
        Me.CheckBoxDisplayPageImages.Location = New System.Drawing.Point(6, 42)
        Me.CheckBoxDisplayPageImages.Name = "CheckBoxDisplayPageImages"
        Me.CheckBoxDisplayPageImages.Size = New System.Drawing.Size(283, 17)
        Me.CheckBoxDisplayPageImages.TabIndex = 0
        Me.CheckBoxDisplayPageImages.Text = "Display all &Page diagrams at the end of the document?"
        Me.CheckBoxDisplayPageImages.UseVisualStyleBackColor = True
        '
        'CheckBoxDisplayTermDiagram
        '
        Me.CheckBoxDisplayTermDiagram.AutoSize = True
        Me.CheckBoxDisplayTermDiagram.Checked = True
        Me.CheckBoxDisplayTermDiagram.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBoxDisplayTermDiagram.Location = New System.Drawing.Point(7, 20)
        Me.CheckBoxDisplayTermDiagram.Name = "CheckBoxDisplayTermDiagram"
        Me.CheckBoxDisplayTermDiagram.Size = New System.Drawing.Size(352, 17)
        Me.CheckBoxDisplayTermDiagram.TabIndex = 2
        Me.CheckBoxDisplayTermDiagram.Text = "For each &Term, display Page diagram of same name? (recommended)"
        Me.CheckBoxDisplayTermDiagram.UseVisualStyleBackColor = True
        '
        'frmDocumentGeneratorSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(392, 151)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmDocumentGeneratorSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Document Generator Settings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxDisplayPageImages As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonGenerateDocumentation As System.Windows.Forms.Button
    Friend WithEvents CheckBoxDisplayTermDiagram As System.Windows.Forms.CheckBox
End Class
