<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSignatueResolution
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSignatueResolution))
        Me.LabelPrompt = New System.Windows.Forms.Label()
        Me.DataGridView = New System.Windows.Forms.DataGridView()
        Me.ButtonClose = New System.Windows.Forms.Button()
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelPrompt
        '
        Me.LabelPrompt.Location = New System.Drawing.Point(12, 13)
        Me.LabelPrompt.Name = "LabelPrompt"
        Me.LabelPrompt.Size = New System.Drawing.Size(582, 82)
        Me.LabelPrompt.TabIndex = 0
        Me.LabelPrompt.Text = resources.GetString("LabelPrompt.Text")
        '
        'DataGridView
        '
        Me.DataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView.Location = New System.Drawing.Point(12, 106)
        Me.DataGridView.Name = "DataGridView"
        Me.DataGridView.Size = New System.Drawing.Size(655, 194)
        Me.DataGridView.TabIndex = 1
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Location = New System.Drawing.Point(600, 12)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(67, 26)
        Me.ButtonClose.TabIndex = 2
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmSignatueResolution
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(680, 314)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.DataGridView)
        Me.Controls.Add(Me.LabelPrompt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmSignatueResolution"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmSignatueResolution"
        CType(Me.DataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelPrompt As System.Windows.Forms.Label
    Friend WithEvents DataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
End Class
