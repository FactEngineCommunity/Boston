Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class LinkButton
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'LinkLabel1
            '
            Me.LinkLabel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LinkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.LinkLabel1.Location = New System.Drawing.Point(20, 0)
            Me.LinkLabel1.Name = "LinkLabel1"
            Me.LinkLabel1.Size = New System.Drawing.Size(130, 16)
            Me.LinkLabel1.TabIndex = 1
            Me.LinkLabel1.TabStop = True
            Me.LinkLabel1.Text = "LinkButton"
            Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'PictureBox1
            '
            Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
            Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Left
            Me.PictureBox1.Location = New System.Drawing.Point(4, 0)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
            Me.PictureBox1.TabIndex = 2
            Me.PictureBox1.TabStop = False
            '
            'LinkButton
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.LinkLabel1)
            Me.Controls.Add(Me.PictureBox1)
            Me.Name = "LinkButton"
            Me.Padding = New System.Windows.Forms.Padding(4, 0, 0, 0)
            Me.Size = New System.Drawing.Size(150, 16)
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

    End Class

End Namespace