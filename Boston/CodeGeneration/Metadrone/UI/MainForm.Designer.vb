Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class MainForm
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.tsMain = New System.Windows.Forms.ToolStrip()
            Me.btnNew = New System.Windows.Forms.ToolStripButton()
            Me.btnOpen = New System.Windows.Forms.ToolStripButton()
            Me.btnSave = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.btnBuild = New System.Windows.Forms.ToolStripButton()
            Me.btnCancel = New System.Windows.Forms.ToolStripButton()
            Me.splitMain = New System.Windows.Forms.SplitContainer()
            Me.splitWorkspace = New System.Windows.Forms.SplitContainer()
            Me.tcExplore = New Boston.UI.CustTabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.tvwExplorer = New Boston.UI.Explorer()
            Me.imlExploreTabPageImages = New System.Windows.Forms.ImageList(Me.components)
            Me.tcMain = New Boston.UI.CustTabControl()
            Me.tpStart = New System.Windows.Forms.TabPage()
            Me.StartPage1 = New Boston.UI.StartPage()
            Me.imlTabPageImages = New System.Windows.Forms.ImageList(Me.components)
            Me.tcResults = New Boston.UI.CustTabControl()
            Me.tpResult = New System.Windows.Forms.TabPage()
            Me.txtResult = New System.Windows.Forms.TextBox()
            Me.tpOutput = New System.Windows.Forms.TabPage()
            Me.grdOutput = New System.Windows.Forms.DataGridView()
            Me.Path = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PathSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PathDateModified = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.tpConsole = New System.Windows.Forms.TabPage()
            Me.txtConsole = New System.Windows.Forms.TextBox()
            Me.imlOutputTabPageImages = New System.Windows.Forms.ImageList(Me.components)
            Me.StatusStrip = New System.Windows.Forms.StatusStrip()
            Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
            Me.spliSearchContainer = New System.Windows.Forms.SplitContainer()
            Me.lblSearchMessage = New System.Windows.Forms.Label()
            Me.SearchBox = New Boston.UI.SearchBox()
            Me.mnuTabControl = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mniCloseTab = New System.Windows.Forms.ToolStripMenuItem()
            Me.mniCloseOtherTabs = New System.Windows.Forms.ToolStripMenuItem()
            Me.mniSep1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mniCloseAllTabs = New System.Windows.Forms.ToolStripMenuItem()
            Me.tsMain.SuspendLayout()
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            CType(Me.splitWorkspace, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitWorkspace.Panel1.SuspendLayout()
            Me.splitWorkspace.Panel2.SuspendLayout()
            Me.splitWorkspace.SuspendLayout()
            Me.tcExplore.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.tcMain.SuspendLayout()
            Me.tpStart.SuspendLayout()
            Me.tcResults.SuspendLayout()
            Me.tpResult.SuspendLayout()
            Me.tpOutput.SuspendLayout()
            CType(Me.grdOutput, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tpConsole.SuspendLayout()
            Me.StatusStrip.SuspendLayout()
            CType(Me.spliSearchContainer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.spliSearchContainer.Panel1.SuspendLayout()
            Me.spliSearchContainer.Panel2.SuspendLayout()
            Me.spliSearchContainer.SuspendLayout()
            Me.mnuTabControl.SuspendLayout()
            Me.SuspendLayout()
            '
            'tsMain
            '
            Me.tsMain.AutoSize = False
            Me.tsMain.BackgroundImage = CType(resources.GetObject("tsMain.BackgroundImage"), System.Drawing.Image)
            Me.tsMain.GripMargin = New System.Windows.Forms.Padding(0)
            Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNew, Me.btnOpen, Me.btnSave, Me.ToolStripSeparator1, Me.btnBuild, Me.btnCancel})
            Me.tsMain.Location = New System.Drawing.Point(0, 0)
            Me.tsMain.Name = "tsMain"
            Me.tsMain.Padding = New System.Windows.Forms.Padding(0)
            Me.tsMain.Size = New System.Drawing.Size(1196, 25)
            Me.tsMain.Stretch = True
            Me.tsMain.TabIndex = 1
            '
            'btnNew
            '
            Me.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnNew.Image = CType(resources.GetObject("btnNew.Image"), System.Drawing.Image)
            Me.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnNew.Name = "btnNew"
            Me.btnNew.Size = New System.Drawing.Size(23, 22)
            Me.btnNew.Text = "Add New Item"
            '
            'btnOpen
            '
            Me.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnOpen.Image = CType(resources.GetObject("btnOpen.Image"), System.Drawing.Image)
            Me.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnOpen.Name = "btnOpen"
            Me.btnOpen.Size = New System.Drawing.Size(23, 22)
            Me.btnOpen.Text = "Open Project"
            '
            'btnSave
            '
            Me.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.btnSave.Enabled = False
            Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
            Me.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(23, 22)
            Me.btnSave.Text = "Save Whole Project"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
            '
            'btnBuild
            '
            Me.btnBuild.Enabled = False
            Me.btnBuild.Image = CType(resources.GetObject("btnBuild.Image"), System.Drawing.Image)
            Me.btnBuild.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnBuild.Name = "btnBuild"
            Me.btnBuild.Size = New System.Drawing.Size(54, 22)
            Me.btnBuild.Text = "Build"
            Me.btnBuild.ToolTipText = "Run Code Generation Build (F5)"
            '
            'btnCancel
            '
            Me.btnCancel.Enabled = False
            Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
            Me.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(63, 22)
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.ToolTipText = "Cancel Build"
            '
            'splitMain
            '
            Me.splitMain.BackColor = System.Drawing.Color.Transparent
            Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
            Me.splitMain.Location = New System.Drawing.Point(0, 25)
            Me.splitMain.Name = "splitMain"
            Me.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitMain.Panel1
            '
            Me.splitMain.Panel1.Controls.Add(Me.splitWorkspace)
            '
            'splitMain.Panel2
            '
            Me.splitMain.Panel2.Controls.Add(Me.tcResults)
            Me.splitMain.Size = New System.Drawing.Size(1196, 717)
            Me.splitMain.SplitterDistance = 540
            Me.splitMain.TabIndex = 2
            '
            'splitWorkspace
            '
            Me.splitWorkspace.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitWorkspace.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
            Me.splitWorkspace.Location = New System.Drawing.Point(0, 0)
            Me.splitWorkspace.Margin = New System.Windows.Forms.Padding(0)
            Me.splitWorkspace.Name = "splitWorkspace"
            '
            'splitWorkspace.Panel1
            '
            Me.splitWorkspace.Panel1.Controls.Add(Me.tcExplore)
            '
            'splitWorkspace.Panel2
            '
            Me.splitWorkspace.Panel2.Controls.Add(Me.tcMain)
            Me.splitWorkspace.Size = New System.Drawing.Size(1196, 540)
            Me.splitWorkspace.SplitterDistance = 196
            Me.splitWorkspace.TabIndex = 0
            '
            'tcExplore
            '
            Me.tcExplore.Controls.Add(Me.TabPage1)
            Me.tcExplore.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcExplore.ImageList = Me.imlExploreTabPageImages
            Me.tcExplore.ItemSize = New System.Drawing.Size(0, 15)
            Me.tcExplore.Location = New System.Drawing.Point(0, 0)
            Me.tcExplore.Name = "tcExplore"
            Me.tcExplore.NoClosing = True
            Me.tcExplore.Padding = New System.Drawing.Point(9, 0)
            Me.tcExplore.SelectedIndex = 0
            Me.tcExplore.Size = New System.Drawing.Size(196, 540)
            Me.tcExplore.TabIndex = 0
            Me.tcExplore.TabsStationary = True
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.tvwExplorer)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 19)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(4)
            Me.TabPage1.Size = New System.Drawing.Size(188, 517)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Project"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'tvwExplorer
            '
            Me.tvwExplorer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tvwExplorer.LabelEdit = False
            Me.tvwExplorer.Location = New System.Drawing.Point(4, 4)
            Me.tvwExplorer.Name = "tvwExplorer"
            Me.tvwExplorer.SelectedNode = Nothing
            Me.tvwExplorer.Size = New System.Drawing.Size(180, 509)
            Me.tvwExplorer.TabIndex = 3
            Me.tvwExplorer.TopNode = Nothing
            '
            'imlExploreTabPageImages
            '
            Me.imlExploreTabPageImages.ImageStream = CType(resources.GetObject("imlExploreTabPageImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlExploreTabPageImages.TransparentColor = System.Drawing.Color.Transparent
            Me.imlExploreTabPageImages.Images.SetKeyName(0, "sitemap16x16.png")
            '
            'tcMain
            '
            Me.tcMain.Controls.Add(Me.tpStart)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.ImageList = Me.imlTabPageImages
            Me.tcMain.ItemSize = New System.Drawing.Size(0, 15)
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.NoClosing = False
            Me.tcMain.Padding = New System.Drawing.Point(9, 0)
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(996, 540)
            Me.tcMain.TabIndex = 0
            Me.tcMain.TabsStationary = False
            '
            'tpStart
            '
            Me.tpStart.Controls.Add(Me.StartPage1)
            Me.tpStart.ImageIndex = 0
            Me.tpStart.Location = New System.Drawing.Point(4, 19)
            Me.tpStart.Name = "tpStart"
            Me.tpStart.Size = New System.Drawing.Size(988, 517)
            Me.tpStart.TabIndex = 0
            Me.tpStart.Text = "Home"
            Me.tpStart.UseVisualStyleBackColor = True
            '
            'StartPage1
            '
            Me.StartPage1.BackColor = System.Drawing.Color.White
            Me.StartPage1.BackgroundImage = CType(resources.GetObject("StartPage1.BackgroundImage"), System.Drawing.Image)
            Me.StartPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.StartPage1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.StartPage1.Location = New System.Drawing.Point(0, 0)
            Me.StartPage1.Name = "StartPage1"
            Me.StartPage1.Size = New System.Drawing.Size(988, 517)
            Me.StartPage1.TabIndex = 0
            '
            'imlTabPageImages
            '
            Me.imlTabPageImages.ImageStream = CType(resources.GetObject("imlTabPageImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlTabPageImages.TransparentColor = System.Drawing.Color.Transparent
            Me.imlTabPageImages.Images.SetKeyName(0, "house16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(1, "music16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(2, "Template16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(3, "VB16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(4, "CS16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(5, "Source16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(6, "Properties16x16.png")
            Me.imlTabPageImages.Images.SetKeyName(7, "TemplateLibrary16x16.png")
            '
            'tcResults
            '
            Me.tcResults.Controls.Add(Me.tpResult)
            Me.tcResults.Controls.Add(Me.tpOutput)
            Me.tcResults.Controls.Add(Me.tpConsole)
            Me.tcResults.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcResults.ImageList = Me.imlOutputTabPageImages
            Me.tcResults.ItemSize = New System.Drawing.Size(0, 15)
            Me.tcResults.Location = New System.Drawing.Point(0, 0)
            Me.tcResults.Name = "tcResults"
            Me.tcResults.NoClosing = True
            Me.tcResults.Padding = New System.Drawing.Point(9, 0)
            Me.tcResults.SelectedIndex = 0
            Me.tcResults.Size = New System.Drawing.Size(1196, 173)
            Me.tcResults.TabIndex = 1
            Me.tcResults.TabsStationary = True
            '
            'tpResult
            '
            Me.tpResult.Controls.Add(Me.txtResult)
            Me.tpResult.ImageIndex = 0
            Me.tpResult.Location = New System.Drawing.Point(4, 19)
            Me.tpResult.Name = "tpResult"
            Me.tpResult.Size = New System.Drawing.Size(1188, 150)
            Me.tpResult.TabIndex = 0
            Me.tpResult.Text = "Result"
            Me.tpResult.UseVisualStyleBackColor = True
            '
            'txtResult
            '
            Me.txtResult.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtResult.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtResult.Location = New System.Drawing.Point(0, 0)
            Me.txtResult.Multiline = True
            Me.txtResult.Name = "txtResult"
            Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.txtResult.Size = New System.Drawing.Size(1188, 150)
            Me.txtResult.TabIndex = 0
            '
            'tpOutput
            '
            Me.tpOutput.Controls.Add(Me.grdOutput)
            Me.tpOutput.ImageIndex = 2
            Me.tpOutput.Location = New System.Drawing.Point(4, 19)
            Me.tpOutput.Name = "tpOutput"
            Me.tpOutput.Size = New System.Drawing.Size(1188, 150)
            Me.tpOutput.TabIndex = 2
            Me.tpOutput.Text = "Output"
            Me.tpOutput.UseVisualStyleBackColor = True
            '
            'grdOutput
            '
            Me.grdOutput.AllowUserToAddRows = False
            Me.grdOutput.AllowUserToDeleteRows = False
            Me.grdOutput.AllowUserToResizeColumns = False
            Me.grdOutput.AllowUserToResizeRows = False
            Me.grdOutput.BackgroundColor = System.Drawing.SystemColors.Window
            Me.grdOutput.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.grdOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdOutput.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Path, Me.PathSize, Me.PathDateModified})
            Me.grdOutput.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdOutput.GridColor = System.Drawing.SystemColors.Window
            Me.grdOutput.Location = New System.Drawing.Point(0, 0)
            Me.grdOutput.MultiSelect = False
            Me.grdOutput.Name = "grdOutput"
            Me.grdOutput.ReadOnly = True
            Me.grdOutput.RowHeadersVisible = False
            Me.grdOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.grdOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.grdOutput.Size = New System.Drawing.Size(1188, 150)
            Me.grdOutput.TabIndex = 1
            '
            'Path
            '
            Me.Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.Path.HeaderText = "Path"
            Me.Path.Name = "Path"
            Me.Path.ReadOnly = True
            '
            'PathSize
            '
            Me.PathSize.HeaderText = "Size"
            Me.PathSize.Name = "PathSize"
            Me.PathSize.ReadOnly = True
            Me.PathSize.Width = 70
            '
            'PathDateModified
            '
            Me.PathDateModified.HeaderText = "Date Modified"
            Me.PathDateModified.Name = "PathDateModified"
            Me.PathDateModified.ReadOnly = True
            Me.PathDateModified.Width = 150
            '
            'tpConsole
            '
            Me.tpConsole.Controls.Add(Me.txtConsole)
            Me.tpConsole.ImageIndex = 1
            Me.tpConsole.Location = New System.Drawing.Point(4, 19)
            Me.tpConsole.Name = "tpConsole"
            Me.tpConsole.Size = New System.Drawing.Size(1188, 150)
            Me.tpConsole.TabIndex = 1
            Me.tpConsole.Text = "Console"
            Me.tpConsole.UseVisualStyleBackColor = True
            '
            'txtConsole
            '
            Me.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtConsole.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtConsole.Location = New System.Drawing.Point(0, 0)
            Me.txtConsole.Multiline = True
            Me.txtConsole.Name = "txtConsole"
            Me.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.txtConsole.Size = New System.Drawing.Size(1188, 150)
            Me.txtConsole.TabIndex = 1
            '
            'imlOutputTabPageImages
            '
            Me.imlOutputTabPageImages.ImageStream = CType(resources.GetObject("imlOutputTabPageImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.imlOutputTabPageImages.TransparentColor = System.Drawing.Color.Transparent
            Me.imlOutputTabPageImages.Images.SetKeyName(0, "application_view_list16x16.png")
            Me.imlOutputTabPageImages.Images.SetKeyName(1, "terminal16x16.png")
            Me.imlOutputTabPageImages.Images.SetKeyName(2, "SavePreview16x16.png")
            '
            'StatusStrip
            '
            Me.StatusStrip.BackColor = System.Drawing.Color.Transparent
            Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
            Me.StatusStrip.Location = New System.Drawing.Point(0, 742)
            Me.StatusStrip.Name = "StatusStrip"
            Me.StatusStrip.Size = New System.Drawing.Size(1196, 22)
            Me.StatusStrip.TabIndex = 3
            Me.StatusStrip.Text = "StatusStrip1"
            '
            'lblStatus
            '
            Me.lblStatus.BackColor = System.Drawing.Color.Transparent
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(1181, 17)
            Me.lblStatus.Spring = True
            Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopLeft
            '
            'spliSearchContainer
            '
            Me.spliSearchContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.spliSearchContainer.BackColor = System.Drawing.Color.Transparent
            Me.spliSearchContainer.Location = New System.Drawing.Point(201, 0)
            Me.spliSearchContainer.Name = "spliSearchContainer"
            '
            'spliSearchContainer.Panel1
            '
            Me.spliSearchContainer.Panel1.BackColor = System.Drawing.Color.Transparent
            Me.spliSearchContainer.Panel1.Controls.Add(Me.lblSearchMessage)
            '
            'spliSearchContainer.Panel2
            '
            Me.spliSearchContainer.Panel2.BackColor = System.Drawing.Color.Transparent
            Me.spliSearchContainer.Panel2.Controls.Add(Me.SearchBox)
            Me.spliSearchContainer.Panel2MinSize = 100
            Me.spliSearchContainer.Size = New System.Drawing.Size(995, 24)
            Me.spliSearchContainer.SplitterDistance = 790
            Me.spliSearchContainer.TabIndex = 5
            '
            'lblSearchMessage
            '
            Me.lblSearchMessage.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSearchMessage.Location = New System.Drawing.Point(0, 0)
            Me.lblSearchMessage.Name = "lblSearchMessage"
            Me.lblSearchMessage.Size = New System.Drawing.Size(790, 24)
            Me.lblSearchMessage.TabIndex = 0
            Me.lblSearchMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'SearchBox
            '
            Me.SearchBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SearchBox.Location = New System.Drawing.Point(0, 0)
            Me.SearchBox.Name = "SearchBox"
            Me.SearchBox.SearchScope = Boston.UI.SearchBox.SearchScopes.Project
            Me.SearchBox.Size = New System.Drawing.Size(201, 24)
            Me.SearchBox.TabIndex = 4
            '
            'mnuTabControl
            '
            Me.mnuTabControl.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniCloseTab, Me.mniCloseOtherTabs, Me.mniSep1, Me.mniCloseAllTabs})
            Me.mnuTabControl.Name = "mnuTabControl"
            Me.mnuTabControl.Size = New System.Drawing.Size(163, 76)
            '
            'mniCloseTab
            '
            Me.mniCloseTab.Image = CType(resources.GetObject("mniCloseTab.Image"), System.Drawing.Image)
            Me.mniCloseTab.Name = "mniCloseTab"
            Me.mniCloseTab.Size = New System.Drawing.Size(162, 22)
            Me.mniCloseTab.Text = "Close Tab"
            '
            'mniCloseOtherTabs
            '
            Me.mniCloseOtherTabs.Image = CType(resources.GetObject("mniCloseOtherTabs.Image"), System.Drawing.Image)
            Me.mniCloseOtherTabs.Name = "mniCloseOtherTabs"
            Me.mniCloseOtherTabs.Size = New System.Drawing.Size(162, 22)
            Me.mniCloseOtherTabs.Text = "Close Other Tabs"
            '
            'mniSep1
            '
            Me.mniSep1.Name = "mniSep1"
            Me.mniSep1.Size = New System.Drawing.Size(159, 6)
            '
            'mniCloseAllTabs
            '
            Me.mniCloseAllTabs.Image = CType(resources.GetObject("mniCloseAllTabs.Image"), System.Drawing.Image)
            Me.mniCloseAllTabs.Name = "mniCloseAllTabs"
            Me.mniCloseAllTabs.Size = New System.Drawing.Size(162, 22)
            Me.mniCloseAllTabs.Text = "Close All Tabs"
            '
            'MainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.GhostWhite
            Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
            Me.ClientSize = New System.Drawing.Size(1196, 764)
            Me.Controls.Add(Me.spliSearchContainer)
            Me.Controls.Add(Me.splitMain)
            Me.Controls.Add(Me.tsMain)
            Me.Controls.Add(Me.StatusStrip)
            Me.DoubleBuffered = True
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(450, 400)
            Me.Name = "MainForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Metadrone"
            Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
            Me.tsMain.ResumeLayout(False)
            Me.tsMain.PerformLayout()
            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            Me.splitWorkspace.Panel1.ResumeLayout(False)
            Me.splitWorkspace.Panel2.ResumeLayout(False)
            CType(Me.splitWorkspace, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitWorkspace.ResumeLayout(False)
            Me.tcExplore.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.tcMain.ResumeLayout(False)
            Me.tpStart.ResumeLayout(False)
            Me.tcResults.ResumeLayout(False)
            Me.tpResult.ResumeLayout(False)
            Me.tpResult.PerformLayout()
            Me.tpOutput.ResumeLayout(False)
            CType(Me.grdOutput, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tpConsole.ResumeLayout(False)
            Me.tpConsole.PerformLayout()
            Me.StatusStrip.ResumeLayout(False)
            Me.StatusStrip.PerformLayout()
            Me.spliSearchContainer.Panel1.ResumeLayout(False)
            Me.spliSearchContainer.Panel2.ResumeLayout(False)
            CType(Me.spliSearchContainer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.spliSearchContainer.ResumeLayout(False)
            Me.mnuTabControl.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
        Friend WithEvents btnNew As System.Windows.Forms.ToolStripButton
        Friend WithEvents btnOpen As System.Windows.Forms.ToolStripButton
        Friend WithEvents btnSave As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents btnBuild As System.Windows.Forms.ToolStripButton
        Friend WithEvents btnCancel As System.Windows.Forms.ToolStripButton
        Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
        Friend WithEvents splitWorkspace As System.Windows.Forms.SplitContainer
        Friend WithEvents imlTabPageImages As System.Windows.Forms.ImageList
        Friend WithEvents tvwExplorer As Boston.UI.Explorer
        Friend WithEvents tcMain As Boston.UI.CustTabControl
        Friend WithEvents tpStart As System.Windows.Forms.TabPage
        Friend WithEvents StartPage1 As Boston.UI.StartPage
        Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
        Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents SearchBox As Boston.UI.SearchBox
        Friend WithEvents spliSearchContainer As System.Windows.Forms.SplitContainer
        Friend WithEvents lblSearchMessage As System.Windows.Forms.Label
        Friend WithEvents mnuTabControl As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mniCloseTab As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniCloseOtherTabs As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mniSep1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mniCloseAllTabs As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents txtResult As System.Windows.Forms.TextBox
        Friend WithEvents tcResults As Boston.UI.CustTabControl
        Friend WithEvents tpResult As System.Windows.Forms.TabPage
        Friend WithEvents imlOutputTabPageImages As System.Windows.Forms.ImageList
        Friend WithEvents tpConsole As System.Windows.Forms.TabPage
        Friend WithEvents txtConsole As System.Windows.Forms.TextBox
        Friend WithEvents tcExplore As Boston.UI.CustTabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents imlExploreTabPageImages As System.Windows.Forms.ImageList
        Friend WithEvents tpOutput As System.Windows.Forms.TabPage
        Friend WithEvents grdOutput As System.Windows.Forms.DataGridView
        Friend WithEvents Path As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PathSize As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PathDateModified As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace