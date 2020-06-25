Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SearchBox
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SearchBox))
            Me.pnlSearch = New System.Windows.Forms.Panel
            Me.pnlText = New System.Windows.Forms.Panel
            Me.txtSearch = New System.Windows.Forms.TextBox
            Me.pnlEnd = New System.Windows.Forms.Panel
            Me.pnlDown2 = New System.Windows.Forms.Panel
            Me.pnlDown1 = New System.Windows.Forms.Panel
            Me.mnuScope = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mniProject = New System.Windows.Forms.ToolStripMenuItem
            Me.mniPackage = New System.Windows.Forms.ToolStripMenuItem
            Me.mniDocument = New System.Windows.Forms.ToolStripMenuItem
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.imlBackGroundStart = New System.Windows.Forms.ImageList(Me.components)
            Me.imlBackGroundMiddle = New System.Windows.Forms.ImageList(Me.components)
            Me.imlBackGroundEnd = New System.Windows.Forms.ImageList(Me.components)
            Me.pnlText.SuspendLayout()
            Me.pnlEnd.SuspendLayout()
            Me.mnuScope.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlSearch
            '
            Me.pnlSearch.BackgroundImage = CType(resources.GetObject("pnlSearch.BackgroundImage"), System.Drawing.Image)
            Me.pnlSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlSearch.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSearch.Location = New System.Drawing.Point(0, 0)
            Me.pnlSearch.Name = "pnlSearch"
            Me.pnlSearch.Size = New System.Drawing.Size(24, 24)
            Me.pnlSearch.TabIndex = 0
            Me.ToolTip1.SetToolTip(Me.pnlSearch, "Search")
            '
            'pnlText
            '
            Me.pnlText.BackgroundImage = CType(resources.GetObject("pnlText.BackgroundImage"), System.Drawing.Image)
            Me.pnlText.Controls.Add(Me.txtSearch)
            Me.pnlText.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlText.Location = New System.Drawing.Point(24, 0)
            Me.pnlText.Margin = New System.Windows.Forms.Padding(0)
            Me.pnlText.Name = "pnlText"
            Me.pnlText.Size = New System.Drawing.Size(126, 24)
            Me.pnlText.TabIndex = 1
            '
            'txtSearch
            '
            Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtSearch.BackColor = System.Drawing.Color.White
            Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtSearch.Location = New System.Drawing.Point(1, 5)
            Me.txtSearch.Name = "txtSearch"
            Me.txtSearch.Size = New System.Drawing.Size(125, 13)
            Me.txtSearch.TabIndex = 0
            '
            'pnlEnd
            '
            Me.pnlEnd.BackgroundImage = CType(resources.GetObject("pnlEnd.BackgroundImage"), System.Drawing.Image)
            Me.pnlEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlEnd.Controls.Add(Me.pnlDown2)
            Me.pnlEnd.Controls.Add(Me.pnlDown1)
            Me.pnlEnd.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlEnd.Location = New System.Drawing.Point(150, 0)
            Me.pnlEnd.Name = "pnlEnd"
            Me.pnlEnd.Size = New System.Drawing.Size(50, 24)
            Me.pnlEnd.TabIndex = 2
            '
            'pnlDown2
            '
            Me.pnlDown2.BackColor = System.Drawing.Color.Transparent
            Me.pnlDown2.BackgroundImage = CType(resources.GetObject("pnlDown2.BackgroundImage"), System.Drawing.Image)
            Me.pnlDown2.Location = New System.Drawing.Point(28, 4)
            Me.pnlDown2.Name = "pnlDown2"
            Me.pnlDown2.Size = New System.Drawing.Size(10, 16)
            Me.pnlDown2.TabIndex = 1
            '
            'pnlDown1
            '
            Me.pnlDown1.BackColor = System.Drawing.Color.Transparent
            Me.pnlDown1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlDown1.Location = New System.Drawing.Point(0, 4)
            Me.pnlDown1.Name = "pnlDown1"
            Me.pnlDown1.Size = New System.Drawing.Size(42, 16)
            Me.pnlDown1.TabIndex = 0
            '
            'mnuScope
            '
            Me.mnuScope.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniProject, Me.mniPackage, Me.mniDocument})
            Me.mnuScope.Name = "mnuScope"
            Me.mnuScope.Size = New System.Drawing.Size(222, 70)
            '
            'mniProject
            '
            Me.mniProject.Image = CType(resources.GetObject("mniProject.Image"), System.Drawing.Image)
            Me.mniProject.Name = "mniProject"
            Me.mniProject.Size = New System.Drawing.Size(221, 22)
            Me.mniProject.Text = "Search in project"
            '
            'mniPackage
            '
            Me.mniPackage.Image = CType(resources.GetObject("mniPackage.Image"), System.Drawing.Image)
            Me.mniPackage.Name = "mniPackage"
            Me.mniPackage.Size = New System.Drawing.Size(221, 22)
            Me.mniPackage.Text = "Search in current package"
            '
            'mniDocument
            '
            Me.mniDocument.Image = CType(resources.GetObject("mniDocument.Image"), System.Drawing.Image)
            Me.mniDocument.Name = "mniDocument"
            Me.mniDocument.Size = New System.Drawing.Size(221, 22)
            Me.mniDocument.Text = "Search in current document"
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "Project28x16.png")
            Me.ImageList1.Images.SetKeyName(1, "Package28x16.png")
            Me.ImageList1.Images.SetKeyName(2, "CurrentDocument28x16.png")
            '
            'imlBackGroundStart
            '
            Me.imlBackGroundStart.ImageStream = CType(resources.GetObject("imlBackGroundStart.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlBackGroundStart.TransparentColor = System.Drawing.Color.Transparent
            Me.imlBackGroundStart.Images.SetKeyName(0, "SearchBoxStartNoFocus.png")
            Me.imlBackGroundStart.Images.SetKeyName(1, "SearchBoxStart.png")
            '
            'imlBackGroundMiddle
            '
            Me.imlBackGroundMiddle.ImageStream = CType(resources.GetObject("imlBackGroundMiddle.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlBackGroundMiddle.TransparentColor = System.Drawing.Color.Transparent
            Me.imlBackGroundMiddle.Images.SetKeyName(0, "SearchBoxMiddleNoFocus.png")
            Me.imlBackGroundMiddle.Images.SetKeyName(1, "SearchBoxMiddle.png")
            '
            'imlBackGroundEnd
            '
            Me.imlBackGroundEnd.ImageStream = CType(resources.GetObject("imlBackGroundEnd.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlBackGroundEnd.TransparentColor = System.Drawing.Color.Transparent
            Me.imlBackGroundEnd.Images.SetKeyName(0, "SearchBoxEndNoFocus.png")
            Me.imlBackGroundEnd.Images.SetKeyName(1, "SearchBoxEnd.png")
            '
            'SearchBox
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.pnlText)
            Me.Controls.Add(Me.pnlEnd)
            Me.Controls.Add(Me.pnlSearch)
            Me.Name = "SearchBox"
            Me.Size = New System.Drawing.Size(200, 24)
            Me.pnlText.ResumeLayout(False)
            Me.pnlText.PerformLayout()
            Me.pnlEnd.ResumeLayout(False)
            Me.mnuScope.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlSearch As System.Windows.Forms.Panel
        Friend WithEvents pnlText As System.Windows.Forms.Panel
        Friend WithEvents pnlEnd As System.Windows.Forms.Panel
        Friend WithEvents txtSearch As System.Windows.Forms.TextBox
        Friend WithEvents pnlDown2 As System.Windows.Forms.Panel
        Friend WithEvents pnlDown1 As System.Windows.Forms.Panel
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Friend WithEvents mnuScope As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mniProject As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniPackage As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniDocument As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents imlBackGroundStart As System.Windows.Forms.ImageList
        Friend WithEvents imlBackGroundMiddle As System.Windows.Forms.ImageList
        Friend WithEvents imlBackGroundEnd As System.Windows.Forms.ImageList

    End Class

End Namespace