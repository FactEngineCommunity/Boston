Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class StartPageRecent
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartPageRecent))
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            Me.lnkRecent = New System.Windows.Forms.LinkLabel
            Me.lblPath = New System.Windows.Forms.Label
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
            Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(20, 20)
            Me.PictureBox1.TabIndex = 4
            Me.PictureBox1.TabStop = False
            '
            'lnkRecent
            '
            Me.lnkRecent.AutoSize = True
            Me.lnkRecent.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lnkRecent.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lnkRecent.LinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkRecent.Location = New System.Drawing.Point(26, 1)
            Me.lnkRecent.Name = "lnkRecent"
            Me.lnkRecent.Size = New System.Drawing.Size(86, 20)
            Me.lnkRecent.TabIndex = 5
            Me.lnkRecent.TabStop = True
            Me.lnkRecent.Text = "LinkLabel1"
            '
            'lblPath
            '
            Me.lblPath.AutoSize = True
            Me.lblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPath.ForeColor = System.Drawing.Color.SlateGray
            Me.lblPath.Location = New System.Drawing.Point(28, 19)
            Me.lblPath.Name = "lblPath"
            Me.lblPath.Size = New System.Drawing.Size(49, 16)
            Me.lblPath.TabIndex = 6
            Me.lblPath.Text = "Label1"
            '
            'StartPageRecent
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.Transparent
            Me.Controls.Add(Me.lnkRecent)
            Me.Controls.Add(Me.lblPath)
            Me.Controls.Add(Me.PictureBox1)
            Me.Name = "StartPageRecent"
            Me.Size = New System.Drawing.Size(215, 48)
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents lnkRecent As System.Windows.Forms.LinkLabel
        Friend WithEvents lblPath As System.Windows.Forms.Label

    End Class

End Namespace
