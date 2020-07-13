Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class StartPage
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartPage))
            Me.pnlAbout = New System.Windows.Forms.Panel()
            Me.pnlFooter = New System.Windows.Forms.Panel()
            Me.pnlSplash = New System.Windows.Forms.Panel()
            Me.pnlRecents = New System.Windows.Forms.Panel()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.PictureBox5 = New System.Windows.Forms.PictureBox()
            Me.lnkOpenProject = New System.Windows.Forms.LinkLabel()
            Me.PictureBox4 = New System.Windows.Forms.PictureBox()
            Me.lnkNewProject = New System.Windows.Forms.LinkLabel()
            Me.ttFast = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttNormal = New System.Windows.Forms.ToolTip(Me.components)
            Me.pnlFooter.SuspendLayout()
            Me.pnlSplash.SuspendLayout()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlAbout
            '
            Me.pnlAbout.BackColor = System.Drawing.Color.Transparent
            Me.pnlAbout.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlAbout.Location = New System.Drawing.Point(0, 0)
            Me.pnlAbout.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.pnlAbout.Name = "pnlAbout"
            Me.pnlAbout.Size = New System.Drawing.Size(148, 63)
            Me.pnlAbout.TabIndex = 0
            '
            'pnlFooter
            '
            Me.pnlFooter.BackColor = System.Drawing.Color.Transparent
            Me.pnlFooter.Controls.Add(Me.pnlAbout)
            Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlFooter.Location = New System.Drawing.Point(0, 903)
            Me.pnlFooter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New System.Drawing.Size(1546, 63)
            Me.pnlFooter.TabIndex = 0
            '
            'pnlSplash
            '
            Me.pnlSplash.BackColor = System.Drawing.Color.White
            Me.pnlSplash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlSplash.Controls.Add(Me.pnlRecents)
            Me.pnlSplash.Controls.Add(Me.Panel1)
            Me.pnlSplash.Controls.Add(Me.Label1)
            Me.pnlSplash.Controls.Add(Me.PictureBox5)
            Me.pnlSplash.Controls.Add(Me.lnkOpenProject)
            Me.pnlSplash.Controls.Add(Me.PictureBox4)
            Me.pnlSplash.Controls.Add(Me.lnkNewProject)
            Me.pnlSplash.Controls.Add(Me.pnlFooter)
            Me.pnlSplash.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlSplash.Location = New System.Drawing.Point(0, 0)
            Me.pnlSplash.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.pnlSplash.Name = "pnlSplash"
            Me.pnlSplash.Size = New System.Drawing.Size(1546, 966)
            Me.pnlSplash.TabIndex = 0
            '
            'pnlRecents
            '
            Me.pnlRecents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlRecents.AutoScroll = True
            Me.pnlRecents.BackColor = System.Drawing.Color.Transparent
            Me.pnlRecents.Location = New System.Drawing.Point(16, 112)
            Me.pnlRecents.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.pnlRecents.Name = "pnlRecents"
            Me.pnlRecents.Size = New System.Drawing.Size(1071, 782)
            Me.pnlRecents.TabIndex = 8
            '
            'Panel1
            '
            Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Panel1.BackColor = System.Drawing.Color.Transparent
            Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
            Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.Panel1.ForeColor = System.Drawing.Color.Transparent
            Me.Panel1.Location = New System.Drawing.Point(464, 232)
            Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(1074, 2)
            Me.Panel1.TabIndex = 7
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.ForeColor = System.Drawing.Color.SlateGray
            Me.Label1.Location = New System.Drawing.Point(15, 72)
            Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(107, 33)
            Me.Label1.TabIndex = 6
            Me.Label1.Text = "Recent"
            '
            'PictureBox5
            '
            Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
            Me.PictureBox5.Location = New System.Drawing.Point(260, 26)
            Me.PictureBox5.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.PictureBox5.Name = "PictureBox5"
            Me.PictureBox5.Size = New System.Drawing.Size(30, 31)
            Me.PictureBox5.TabIndex = 5
            Me.PictureBox5.TabStop = False
            '
            'lnkOpenProject
            '
            Me.lnkOpenProject.ActiveLinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkOpenProject.AutoSize = True
            Me.lnkOpenProject.BackColor = System.Drawing.Color.Transparent
            Me.lnkOpenProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lnkOpenProject.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lnkOpenProject.LinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkOpenProject.Location = New System.Drawing.Point(294, 20)
            Me.lnkOpenProject.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lnkOpenProject.Name = "lnkOpenProject"
            Me.lnkOpenProject.Size = New System.Drawing.Size(185, 33)
            Me.lnkOpenProject.TabIndex = 4
            Me.lnkOpenProject.TabStop = True
            Me.lnkOpenProject.Text = "Open Project"
            Me.ttNormal.SetToolTip(Me.lnkOpenProject, "Open an existing Metadrone project")
            '
            'PictureBox4
            '
            Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
            Me.PictureBox4.Location = New System.Drawing.Point(16, 26)
            Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.PictureBox4.Name = "PictureBox4"
            Me.PictureBox4.Size = New System.Drawing.Size(30, 31)
            Me.PictureBox4.TabIndex = 3
            Me.PictureBox4.TabStop = False
            '
            'lnkNewProject
            '
            Me.lnkNewProject.ActiveLinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkNewProject.AutoSize = True
            Me.lnkNewProject.BackColor = System.Drawing.Color.Transparent
            Me.lnkNewProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lnkNewProject.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lnkNewProject.LinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkNewProject.Location = New System.Drawing.Point(51, 20)
            Me.lnkNewProject.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lnkNewProject.Name = "lnkNewProject"
            Me.lnkNewProject.Size = New System.Drawing.Size(172, 33)
            Me.lnkNewProject.TabIndex = 2
            Me.lnkNewProject.TabStop = True
            Me.lnkNewProject.Text = "New Project"
            Me.ttNormal.SetToolTip(Me.lnkNewProject, "Create new project, closing currently opened project")
            '
            'ttFast
            '
            Me.ttFast.AutoPopDelay = 5000
            Me.ttFast.InitialDelay = 100
            Me.ttFast.IsBalloon = True
            Me.ttFast.ReshowDelay = 100
            '
            'StartPage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.Controls.Add(Me.pnlSplash)
            Me.DoubleBuffered = True
            Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
            Me.Name = "StartPage"
            Me.Size = New System.Drawing.Size(1546, 966)
            Me.pnlFooter.ResumeLayout(False)
            Me.pnlSplash.ResumeLayout(False)
            Me.pnlSplash.PerformLayout()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlAbout As System.Windows.Forms.Panel
        Friend WithEvents pnlFooter As System.Windows.Forms.Panel
        Friend WithEvents pnlSplash As System.Windows.Forms.Panel
        Friend WithEvents ttFast As System.Windows.Forms.ToolTip
        Friend WithEvents lnkNewProject As System.Windows.Forms.LinkLabel
        Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
        Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
        Friend WithEvents lnkOpenProject As System.Windows.Forms.LinkLabel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents pnlRecents As System.Windows.Forms.Panel
        Friend WithEvents ttNormal As System.Windows.Forms.ToolTip

    End Class

End Namespace