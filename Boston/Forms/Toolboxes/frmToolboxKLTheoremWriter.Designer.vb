<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmToolboxKLTheoremWriter
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ButtonAnalyseCurrentPage = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 50)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(433, 433)
        Me.TextBox1.TabIndex = 0
        '
        'ButtonAnalyseCurrentPage
        '
        Me.ButtonAnalyseCurrentPage.Location = New System.Drawing.Point(12, 12)
        Me.ButtonAnalyseCurrentPage.Name = "ButtonAnalyseCurrentPage"
        Me.ButtonAnalyseCurrentPage.Size = New System.Drawing.Size(120, 23)
        Me.ButtonAnalyseCurrentPage.TabIndex = 1
        Me.ButtonAnalyseCurrentPage.Text = "&Analyse current Page"
        Me.ButtonAnalyseCurrentPage.UseVisualStyleBackColor = True
        '
        'frmToolboxKLTheoremWriter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 498)
        Me.Controls.Add(Me.ButtonAnalyseCurrentPage)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "frmToolboxKLTheoremWriter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "KL Theorem Writer"
        Me.Text = "KL Theorem Writer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAnalyseCurrentPage As System.Windows.Forms.Button
End Class
