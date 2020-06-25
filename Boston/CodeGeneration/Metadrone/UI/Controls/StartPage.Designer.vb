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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartPage))
            Me.pnlAbout = New System.Windows.Forms.Panel
            Me.lnkAbout = New System.Windows.Forms.LinkLabel
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            Me.pnlFooter = New System.Windows.Forms.Panel
            Me.pnlSplash = New System.Windows.Forms.Panel
            Me.pnlRecents = New System.Windows.Forms.Panel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.Label1 = New System.Windows.Forms.Label
            Me.PictureBox5 = New System.Windows.Forms.PictureBox
            Me.lnkOpenProject = New System.Windows.Forms.LinkLabel
            Me.PictureBox4 = New System.Windows.Forms.PictureBox
            Me.lnkNewProject = New System.Windows.Forms.LinkLabel
            Me.lblBuild = New System.Windows.Forms.Label
            Me.ttFast = New System.Windows.Forms.ToolTip(Me.components)
            Me.ttNormal = New System.Windows.Forms.ToolTip(Me.components)
            Me.pnlAbout.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlFooter.SuspendLayout()
            Me.pnlSplash.SuspendLayout()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlAbout
            '
            Me.pnlAbout.BackColor = System.Drawing.Color.Transparent
            Me.pnlAbout.Controls.Add(Me.lnkAbout)
            Me.pnlAbout.Controls.Add(Me.PictureBox1)
            Me.pnlAbout.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlAbout.Location = New System.Drawing.Point(0, 0)
            Me.pnlAbout.Name = "pnlAbout"
            Me.pnlAbout.Size = New System.Drawing.Size(99, 41)
            Me.pnlAbout.TabIndex = 0
            '
            'lnkAbout
            '
            Me.lnkAbout.AutoSize = True
            Me.lnkAbout.LinkColor = System.Drawing.Color.FromArgb(CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer))
            Me.lnkAbout.Location = New System.Drawing.Point(47, 16)
            Me.lnkAbout.Name = "lnkAbout"
            Me.lnkAbout.Size = New System.Drawing.Size(35, 13)
            Me.lnkAbout.TabIndex = 1
            Me.lnkAbout.TabStop = True
            Me.lnkAbout.Text = "About"
            Me.ttFast.SetToolTip(Me.lnkAbout, "About Metadrone")
            Me.lnkAbout.VisitedLinkColor = System.Drawing.Color.FromArgb(CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer))
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
            Me.PictureBox1.Location = New System.Drawing.Point(13, 3)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
            Me.PictureBox1.TabIndex = 0
            Me.PictureBox1.TabStop = False
            '
            'pnlFooter
            '
            Me.pnlFooter.BackColor = System.Drawing.Color.Transparent
            Me.pnlFooter.Controls.Add(Me.pnlAbout)
            Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlFooter.Location = New System.Drawing.Point(0, 587)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New System.Drawing.Size(1031, 41)
            Me.pnlFooter.TabIndex = 0
            '
            'pnlSplash
            '
            Me.pnlSplash.BackColor = System.Drawing.Color.White
            Me.pnlSplash.BackgroundImage = CType(resources.GetObject("pnlSplash.BackgroundImage"), System.Drawing.Image)
            Me.pnlSplash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlSplash.Controls.Add(Me.pnlRecents)
            Me.pnlSplash.Controls.Add(Me.Panel1)
            Me.pnlSplash.Controls.Add(Me.Label1)
            Me.pnlSplash.Controls.Add(Me.PictureBox5)
            Me.pnlSplash.Controls.Add(Me.lnkOpenProject)
            Me.pnlSplash.Controls.Add(Me.PictureBox4)
            Me.pnlSplash.Controls.Add(Me.lnkNewProject)
            Me.pnlSplash.Controls.Add(Me.lblBuild)
            Me.pnlSplash.Controls.Add(Me.pnlFooter)
            Me.pnlSplash.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlSplash.Location = New System.Drawing.Point(0, 0)
            Me.pnlSplash.Name = "pnlSplash"
            Me.pnlSplash.Size = New System.Drawing.Size(1031, 628)
            Me.pnlSplash.TabIndex = 0
            '
            'pnlRecents
            '
            Me.pnlRecents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlRecents.AutoScroll = True
            Me.pnlRecents.BackColor = System.Drawing.Color.Transparent
            Me.pnlRecents.Location = New System.Drawing.Point(311, 158)
            Me.pnlRecents.Name = "pnlRecents"
            Me.pnlRecents.Size = New System.Drawing.Size(714, 423)
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
            Me.Panel1.Location = New System.Drawing.Point(309, 151)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(716, 1)
            Me.Panel1.TabIndex = 7
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.ForeColor = System.Drawing.Color.SlateGray
            Me.Label1.Location = New System.Drawing.Point(307, 128)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(70, 24)
            Me.Label1.TabIndex = 6
            Me.Label1.Text = "Recent"
            '
            'PictureBox5
            '
            Me.PictureBox5.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
            Me.PictureBox5.Location = New System.Drawing.Point(473, 85)
            Me.PictureBox5.Name = "PictureBox5"
            Me.PictureBox5.Size = New System.Drawing.Size(20, 20)
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
            Me.lnkOpenProject.Location = New System.Drawing.Point(496, 81)
            Me.lnkOpenProject.Name = "lnkOpenProject"
            Me.lnkOpenProject.Size = New System.Drawing.Size(121, 24)
            Me.lnkOpenProject.TabIndex = 4
            Me.lnkOpenProject.TabStop = True
            Me.lnkOpenProject.Text = "Open Project"
            Me.ttNormal.SetToolTip(Me.lnkOpenProject, "Open an existing Metadrone project")
            '
            'PictureBox4
            '
            Me.PictureBox4.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
            Me.PictureBox4.Location = New System.Drawing.Point(311, 85)
            Me.PictureBox4.Name = "PictureBox4"
            Me.PictureBox4.Size = New System.Drawing.Size(20, 20)
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
            Me.lnkNewProject.Location = New System.Drawing.Point(334, 81)
            Me.lnkNewProject.Name = "lnkNewProject"
            Me.lnkNewProject.Size = New System.Drawing.Size(112, 24)
            Me.lnkNewProject.TabIndex = 2
            Me.lnkNewProject.TabStop = True
            Me.lnkNewProject.Text = "New Project"
            Me.ttNormal.SetToolTip(Me.lnkNewProject, "Create new project, closing currently opened project")
            '
            'lblBuild
            '
            Me.lblBuild.BackColor = System.Drawing.Color.Transparent
            Me.lblBuild.ForeColor = System.Drawing.Color.DimGray
            Me.lblBuild.Location = New System.Drawing.Point(172, 54)
            Me.lblBuild.Name = "lblBuild"
            Me.lblBuild.Size = New System.Drawing.Size(132, 17)
            Me.lblBuild.TabIndex = 1
            Me.lblBuild.Text = "Angry Army 1.0"
            Me.lblBuild.TextAlign = System.Drawing.ContentAlignment.TopRight
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
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.Controls.Add(Me.pnlSplash)
            Me.DoubleBuffered = True
            Me.Name = "StartPage"
            Me.Size = New System.Drawing.Size(1031, 628)
            Me.pnlAbout.ResumeLayout(False)
            Me.pnlAbout.PerformLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlFooter.ResumeLayout(False)
            Me.pnlSplash.ResumeLayout(False)
            Me.pnlSplash.PerformLayout()
            CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlAbout As System.Windows.Forms.Panel
        Friend WithEvents lnkAbout As System.Windows.Forms.LinkLabel
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents pnlFooter As System.Windows.Forms.Panel
        Friend WithEvents pnlSplash As System.Windows.Forms.Panel
        Friend WithEvents lblBuild As System.Windows.Forms.Label
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