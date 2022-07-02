<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class BPMNPopupToolElementChanger
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.LabelPromptChangeElement = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ListBox = New ExtListAndCombo.ExtListBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelPromptChangeElement
        '
        Me.LabelPromptChangeElement.AutoSize = True
        Me.LabelPromptChangeElement.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptChangeElement.ForeColor = System.Drawing.Color.DimGray
        Me.LabelPromptChangeElement.Location = New System.Drawing.Point(3, 9)
        Me.LabelPromptChangeElement.Name = "LabelPromptChangeElement"
        Me.LabelPromptChangeElement.Size = New System.Drawing.Size(133, 19)
        Me.LabelPromptChangeElement.TabIndex = 0
        Me.LabelPromptChangeElement.Text = "Change element"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(33, 42)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(261, 20)
        Me.TextBox1.TabIndex = 1
        '
        'ListBox
        '
        Me.ListBox.AlignText = True
        Me.ListBox.CaretIndex = -1
        Me.ListBox.DefaultImage = Nothing
        Me.ListBox.DisplayMode = CType((ExtListAndCombo.DisplayMode.ShowText Or ExtListAndCombo.DisplayMode.ShowImage), ExtListAndCombo.DisplayMode)
        Me.ListBox.DrawMode = ExtListAndCombo.DrawMode.UseFixedHeight
        Me.ListBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox.FormattingEnabled = True
        Me.ListBox.HighlightBackColor = System.Drawing.SystemColors.Highlight
        Me.ListBox.HighlightTextColor = System.Drawing.SystemColors.HighlightText
        Me.ListBox.ImagePadding = New System.Windows.Forms.Padding(1)
        Me.ListBox.ImageSize = New System.Drawing.Size(23, 23)
        Me.ListBox.ItemHeight = 25
        Me.ListBox.Location = New System.Drawing.Point(7, 79)
        Me.ListBox.Name = "ListBox"
        Me.ListBox.SelectedItemInfo = Nothing
        Me.ListBox.Size = New System.Drawing.Size(287, 229)
        Me.ListBox.TabIndex = 3
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Boston.My.Resources.Resources.Spyglass16x16
        Me.PictureBox1.Location = New System.Drawing.Point(7, 42)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(20, 20)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'BPMNPopupToolElementChanger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.ListBox)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.LabelPromptChangeElement)
        Me.ForeColor = System.Drawing.Color.White
        Me.Name = "BPMNPopupToolElementChanger"
        Me.Size = New System.Drawing.Size(302, 326)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LabelPromptChangeElement As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ListBox As ExtListAndCombo.ExtListBox
End Class
