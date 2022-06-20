Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class SyntaxPopup
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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SyntaxPopup))
            Me.pnlTitle = New System.Windows.Forms.Panel
            Me.lnkCancel = New System.Windows.Forms.LinkLabel
            Me.lblTitle = New System.Windows.Forms.Label
            Me.imglList = New System.Windows.Forms.ImageList(Me.components)
            Me.lblDesc = New System.Windows.Forms.Label
            Me.ttFast = New System.Windows.Forms.ToolTip(Me.components)
            Me.lst = New UI.ImageListBox
            Me.pnlTitle.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlTitle
            '
            Me.pnlTitle.BackgroundImage = CType(resources.GetObject("pnlTitle.BackgroundImage"), System.Drawing.Image)
            Me.pnlTitle.Controls.Add(Me.lnkCancel)
            Me.pnlTitle.Controls.Add(Me.lblTitle)
            Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
            Me.pnlTitle.Name = "pnlTitle"
            Me.pnlTitle.Size = New System.Drawing.Size(594, 32)
            Me.pnlTitle.TabIndex = 0
            '
            'lnkCancel
            '
            Me.lnkCancel.AutoSize = True
            Me.lnkCancel.BackColor = System.Drawing.Color.Transparent
            Me.lnkCancel.Location = New System.Drawing.Point(524, 11)
            Me.lnkCancel.Name = "lnkCancel"
            Me.lnkCancel.Size = New System.Drawing.Size(67, 13)
            Me.lnkCancel.TabIndex = 1
            Me.lnkCancel.TabStop = True
            Me.lnkCancel.Text = "Cancel Load"
            Me.lnkCancel.Visible = False
            '
            'lblTitle
            '
            Me.lblTitle.BackColor = System.Drawing.Color.Transparent
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(594, 32)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Syntax Helper"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'imglList
            '
            Me.imglList.ImageStream = CType(resources.GetObject("imglList.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imglList.TransparentColor = System.Drawing.Color.Transparent
            Me.imglList.Images.SetKeyName(0, "SYSTEM")
            Me.imglList.Images.SetKeyName(1, "METHOD")
            Me.imglList.Images.SetKeyName(2, "PROPERTY")
            Me.imglList.Images.SetKeyName(3, "OBJECT_SOURCE")
            Me.imglList.Images.SetKeyName(4, "OBJECT_TABLE")
            Me.imglList.Images.SetKeyName(5, "OBJECT_COLUMN")
            Me.imglList.Images.SetKeyName(6, "OBJECT_VIEW")
            Me.imglList.Images.SetKeyName(7, "OBJECT_FILE")
            Me.imglList.Images.SetKeyName(8, "OBJECT_PROC")
            Me.imglList.Images.SetKeyName(9, "OBJECT_FUNC")
            Me.imglList.Images.SetKeyName(10, "OBJECT_PARAM")
            Me.imglList.Images.SetKeyName(11, "OBJECT_INPARAM")
            Me.imglList.Images.SetKeyName(12, "OBJECT_OUTPARAM")
            Me.imglList.Images.SetKeyName(13, "OBJECT_INOUTPARAM")
            Me.imglList.Images.SetKeyName(14, "musicvar16x16.png")
            Me.imglList.Images.SetKeyName(15, "TemplateVar16x16.png")
            Me.imglList.Images.SetKeyName(16, "DataSource16x16.png")
            Me.imglList.Images.SetKeyName(17, "Template16x16.png")
            Me.imglList.Images.SetKeyName(18, "TemplateParam16x16.png")
            Me.imglList.Images.SetKeyName(19, "transform16x16.png")
            '
            'lblDesc
            '
            Me.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblDesc.Font = New System.Drawing.Font("Lucida Console", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDesc.Location = New System.Drawing.Point(170, 32)
            Me.lblDesc.Name = "lblDesc"
            Me.lblDesc.Padding = New System.Windows.Forms.Padding(4)
            Me.lblDesc.Size = New System.Drawing.Size(424, 272)
            Me.lblDesc.TabIndex = 1
            '
            'ttFast
            '
            Me.ttFast.AutoPopDelay = 5000
            Me.ttFast.InitialDelay = 100
            Me.ttFast.IsBalloon = True
            Me.ttFast.ReshowDelay = 100
            '
            'lst
            '
            Me.lst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lst.Dock = System.Windows.Forms.DockStyle.Left
            Me.lst.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.lst.FormattingEnabled = True
            Me.lst.ImageList = Me.imglList
            Me.lst.Location = New System.Drawing.Point(0, 32)
            Me.lst.Name = "lst"
            Me.lst.Size = New System.Drawing.Size(170, 262)
            Me.lst.TabIndex = 0
            '
            'SyntaxPopup
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(594, 304)
            Me.ControlBox = False
            Me.Controls.Add(Me.lblDesc)
            Me.Controls.Add(Me.lst)
            Me.Controls.Add(Me.pnlTitle)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "SyntaxPopup"
            Me.Opacity = 0.94
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
            Me.pnlTitle.ResumeLayout(False)
            Me.pnlTitle.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlTitle As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents imglList As System.Windows.Forms.ImageList
        Friend WithEvents lst As UI.ImageListBox
        Friend WithEvents lnkCancel As System.Windows.Forms.LinkLabel
        Friend WithEvents lblDesc As System.Windows.Forms.Label
        Friend WithEvents ttFast As System.Windows.Forms.ToolTip
    End Class

End Namespace