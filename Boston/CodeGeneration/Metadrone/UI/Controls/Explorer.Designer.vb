Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
   Partial Friend Class Explorer
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Explorer))
            Me.tvwMain = New System.Windows.Forms.TreeView
            Me.imlMain = New System.Windows.Forms.ImageList(Me.components)
            Me.mnuMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mniOpen = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAdd = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewPackage = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewSource = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewTemplate = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewVB = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewCS = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewFolder = New System.Windows.Forms.ToolStripMenuItem
            Me.mniAddNewProjectFolder = New System.Windows.Forms.ToolStripMenuItem
            Me.mniDelete = New System.Windows.Forms.ToolStripMenuItem
            Me.mniRename = New System.Windows.Forms.ToolStripMenuItem
            Me.mniSep1 = New System.Windows.Forms.ToolStripSeparator
            Me.mniCut = New System.Windows.Forms.ToolStripMenuItem
            Me.mniCopy = New System.Windows.Forms.ToolStripMenuItem
            Me.mniPaste = New System.Windows.Forms.ToolStripMenuItem
            Me.mnuMain.SuspendLayout()
            Me.SuspendLayout()
            '
            'tvwMain
            '
            Me.tvwMain.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.tvwMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tvwMain.FullRowSelect = True
            Me.tvwMain.HideSelection = False
            Me.tvwMain.HotTracking = True
            Me.tvwMain.ImageIndex = 0
            Me.tvwMain.ImageList = Me.imlMain
            Me.tvwMain.Location = New System.Drawing.Point(0, 0)
            Me.tvwMain.Name = "tvwMain"
            Me.tvwMain.SelectedImageIndex = 0
            Me.tvwMain.ShowNodeToolTips = True
            Me.tvwMain.ShowRootLines = False
            Me.tvwMain.Size = New System.Drawing.Size(199, 413)
            Me.tvwMain.TabIndex = 4
            '
            'imlMain
            '
            Me.imlMain.ImageStream = CType(resources.GetObject("imlMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlMain.TransparentColor = System.Drawing.Color.Transparent
            Me.imlMain.Images.SetKeyName(0, "Project16x16.png")
            Me.imlMain.Images.SetKeyName(1, "ProjFolderClosed16x16.png")
            Me.imlMain.Images.SetKeyName(2, "ProjFolderOpen16x16.png")
            Me.imlMain.Images.SetKeyName(3, "Package16x16.png")
            Me.imlMain.Images.SetKeyName(4, "music16x16.png")
            Me.imlMain.Images.SetKeyName(5, "Template16x16.png")
            Me.imlMain.Images.SetKeyName(6, "VB16x16.png")
            Me.imlMain.Images.SetKeyName(7, "CS16x16.png")
            Me.imlMain.Images.SetKeyName(8, "FolderClosed16x16.png")
            Me.imlMain.Images.SetKeyName(9, "FolderOpen16x16.png")
            Me.imlMain.Images.SetKeyName(10, "Source16x16.png")
            Me.imlMain.Images.SetKeyName(11, "Properties16x16.png")
            Me.imlMain.Images.SetKeyName(12, "ProjFolderClosedDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(13, "ProjFolderOpenDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(14, "PackageDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(15, "musicDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(16, "TemplateDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(17, "FolderClosedDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(18, "FolderOpenDimmed16x16.png")
            Me.imlMain.Images.SetKeyName(19, "SourceDimmed16x16.png")
            '
            'mnuMain
            '
            Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniOpen, Me.mniAdd, Me.mniDelete, Me.mniRename, Me.mniSep1, Me.mniCut, Me.mniCopy, Me.mniPaste})
            Me.mnuMain.Name = "mnuMain"
            Me.mnuMain.Size = New System.Drawing.Size(118, 164)
            '
            'mniOpen
            '
            Me.mniOpen.Image = CType(resources.GetObject("mniOpen.Image"), System.Drawing.Image)
            Me.mniOpen.Name = "mniOpen"
            Me.mniOpen.Size = New System.Drawing.Size(117, 22)
            Me.mniOpen.Text = "Open"
            '
            'mniAdd
            '
            Me.mniAdd.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniAddNewPackage, Me.mniAddNewSource, Me.mniAddNewTemplate, Me.mniAddNewVB, Me.mniAddNewCS, Me.mniAddNewFolder, Me.mniAddNewProjectFolder})
            Me.mniAdd.Image = CType(resources.GetObject("mniAdd.Image"), System.Drawing.Image)
            Me.mniAdd.Name = "mniAdd"
            Me.mniAdd.Size = New System.Drawing.Size(117, 22)
            Me.mniAdd.Text = "Add"
            '
            'mniAddNewPackage
            '
            Me.mniAddNewPackage.Image = CType(resources.GetObject("mniAddNewPackage.Image"), System.Drawing.Image)
            Me.mniAddNewPackage.Name = "mniAddNewPackage"
            Me.mniAddNewPackage.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewPackage.Text = "Package"
            '
            'mniAddNewSource
            '
            Me.mniAddNewSource.Image = CType(resources.GetObject("mniAddNewSource.Image"), System.Drawing.Image)
            Me.mniAddNewSource.Name = "mniAddNewSource"
            Me.mniAddNewSource.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewSource.Text = "Source"
            '
            'mniAddNewTemplate
            '
            Me.mniAddNewTemplate.Image = CType(resources.GetObject("mniAddNewTemplate.Image"), System.Drawing.Image)
            Me.mniAddNewTemplate.Name = "mniAddNewTemplate"
            Me.mniAddNewTemplate.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewTemplate.Text = "Template"
            '
            'mniAddNewVB
            '
            Me.mniAddNewVB.Image = CType(resources.GetObject("mniAddNewVB.Image"), System.Drawing.Image)
            Me.mniAddNewVB.Name = "mniAddNewVB"
            Me.mniAddNewVB.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewVB.Text = "VB Code"
            '
            'mniAddNewCS
            '
            Me.mniAddNewCS.Image = CType(resources.GetObject("mniAddNewCS.Image"), System.Drawing.Image)
            Me.mniAddNewCS.Name = "mniAddNewCS"
            Me.mniAddNewCS.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewCS.Text = "C Sharp Code"
            '
            'mniAddNewFolder
            '
            Me.mniAddNewFolder.Image = CType(resources.GetObject("mniAddNewFolder.Image"), System.Drawing.Image)
            Me.mniAddNewFolder.Name = "mniAddNewFolder"
            Me.mniAddNewFolder.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewFolder.Text = "Folder"
            '
            'mniAddNewProjectFolder
            '
            Me.mniAddNewProjectFolder.Image = CType(resources.GetObject("mniAddNewProjectFolder.Image"), System.Drawing.Image)
            Me.mniAddNewProjectFolder.Name = "mniAddNewProjectFolder"
            Me.mniAddNewProjectFolder.Size = New System.Drawing.Size(147, 22)
            Me.mniAddNewProjectFolder.Text = "Project Folder"
            '
            'mniDelete
            '
            Me.mniDelete.Image = CType(resources.GetObject("mniDelete.Image"), System.Drawing.Image)
            Me.mniDelete.Name = "mniDelete"
            Me.mniDelete.Size = New System.Drawing.Size(117, 22)
            Me.mniDelete.Text = "Delete"
            '
            'mniRename
            '
            Me.mniRename.Image = CType(resources.GetObject("mniRename.Image"), System.Drawing.Image)
            Me.mniRename.Name = "mniRename"
            Me.mniRename.Size = New System.Drawing.Size(117, 22)
            Me.mniRename.Text = "Rename"
            '
            'mniSep1
            '
            Me.mniSep1.Name = "mniSep1"
            Me.mniSep1.Size = New System.Drawing.Size(114, 6)
            Me.mniSep1.Visible = False
            '
            'mniCut
            '
            Me.mniCut.Image = CType(resources.GetObject("mniCut.Image"), System.Drawing.Image)
            Me.mniCut.Name = "mniCut"
            Me.mniCut.Size = New System.Drawing.Size(117, 22)
            Me.mniCut.Text = "Cut"
            '
            'mniCopy
            '
            Me.mniCopy.Image = CType(resources.GetObject("mniCopy.Image"), System.Drawing.Image)
            Me.mniCopy.Name = "mniCopy"
            Me.mniCopy.Size = New System.Drawing.Size(117, 22)
            Me.mniCopy.Text = "Copy"
            '
            'mniPaste
            '
            Me.mniPaste.Image = CType(resources.GetObject("mniPaste.Image"), System.Drawing.Image)
            Me.mniPaste.Name = "mniPaste"
            Me.mniPaste.Size = New System.Drawing.Size(117, 22)
            Me.mniPaste.Text = "Paste"
            '
            'Explorer
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tvwMain)
            Me.Name = "Explorer"
            Me.Size = New System.Drawing.Size(199, 413)
            Me.mnuMain.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents tvwMain As System.Windows.Forms.TreeView
        Friend WithEvents imlMain As System.Windows.Forms.ImageList
        Friend WithEvents mnuMain As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mniOpen As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAdd As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewPackage As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewTemplate As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewFolder As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniDelete As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mniAddNewSource As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniRename As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewProjectFolder As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniCut As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniCopy As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniPaste As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewVB As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniAddNewCS As System.Windows.Forms.ToolStripMenuItem

    End Class

End Namespace