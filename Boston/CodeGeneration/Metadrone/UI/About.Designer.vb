Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class About
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(About))
            Me.lblVersion = New System.Windows.Forms.Label
            Me.lblCopyright = New System.Windows.Forms.Label
            Me.lnkDownload = New System.Windows.Forms.LinkLabel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.lblComponents = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.grdPlugins = New System.Windows.Forms.DataGridView
            Me.clmPluginName = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.clmPluginVersion = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.clmPluginFileName = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.grdComponents = New System.Windows.Forms.DataGridView
            Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.tcInfo = New System.Windows.Forms.TabControl
            Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.txtDetails = New System.Windows.Forms.RichTextBox
            Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.txtLicence = New System.Windows.Forms.RichTextBox
            Me.Panel1.SuspendLayout()
            CType(Me.grdPlugins, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.grdComponents, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tcInfo.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblVersion
            '
            Me.lblVersion.AutoSize = True
            Me.lblVersion.BackColor = System.Drawing.Color.Transparent
            Me.lblVersion.Location = New System.Drawing.Point(27, 74)
            Me.lblVersion.Name = "lblVersion"
            Me.lblVersion.Size = New System.Drawing.Size(42, 13)
            Me.lblVersion.TabIndex = 0
            Me.lblVersion.Text = "Version"
            '
            'lblCopyright
            '
            Me.lblCopyright.AutoSize = True
            Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
            Me.lblCopyright.Location = New System.Drawing.Point(27, 92)
            Me.lblCopyright.Name = "lblCopyright"
            Me.lblCopyright.Size = New System.Drawing.Size(90, 13)
            Me.lblCopyright.TabIndex = 1
            Me.lblCopyright.Text = "Copyright © 2011"
            '
            'lnkDownload
            '
            Me.lnkDownload.AutoSize = True
            Me.lnkDownload.BackColor = System.Drawing.Color.Transparent
            Me.lnkDownload.Cursor = System.Windows.Forms.Cursors.Hand
            Me.lnkDownload.DisabledLinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkDownload.LinkColor = System.Drawing.Color.RoyalBlue
            Me.lnkDownload.Location = New System.Drawing.Point(365, 92)
            Me.lnkDownload.Name = "lnkDownload"
            Me.lnkDownload.Size = New System.Drawing.Size(107, 13)
            Me.lnkDownload.TabIndex = 2
            Me.lnkDownload.TabStop = True
            Me.lnkDownload.Text = "www.metadrone.com"
            Me.lnkDownload.VisitedLinkColor = System.Drawing.Color.RoyalBlue
            '
            'Panel1
            '
            Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
            Me.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.Panel1.Controls.Add(Me.lblVersion)
            Me.Panel1.Controls.Add(Me.lnkDownload)
            Me.Panel1.Controls.Add(Me.lblCopyright)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(500, 122)
            Me.Panel1.TabIndex = 0
            '
            'lblComponents
            '
            Me.lblComponents.AutoSize = True
            Me.lblComponents.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblComponents.ForeColor = System.Drawing.Color.LightSlateGray
            Me.lblComponents.Location = New System.Drawing.Point(27, 132)
            Me.lblComponents.Name = "lblComponents"
            Me.lblComponents.Size = New System.Drawing.Size(97, 17)
            Me.lblComponents.TabIndex = 1
            Me.lblComponents.Text = "Components"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.ForeColor = System.Drawing.Color.LightSlateGray
            Me.Label1.Location = New System.Drawing.Point(27, 223)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(61, 17)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "Plugins"
            '
            'grdPlugins
            '
            Me.grdPlugins.AllowUserToAddRows = False
            Me.grdPlugins.AllowUserToDeleteRows = False
            Me.grdPlugins.AllowUserToResizeRows = False
            Me.grdPlugins.BackgroundColor = System.Drawing.Color.White
            Me.grdPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdPlugins.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmPluginName, Me.clmPluginVersion, Me.clmPluginFileName})
            Me.grdPlugins.GridColor = System.Drawing.Color.Gainsboro
            Me.grdPlugins.Location = New System.Drawing.Point(30, 243)
            Me.grdPlugins.MultiSelect = False
            Me.grdPlugins.Name = "grdPlugins"
            Me.grdPlugins.ReadOnly = True
            Me.grdPlugins.RowHeadersVisible = False
            Me.grdPlugins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.grdPlugins.Size = New System.Drawing.Size(442, 102)
            Me.grdPlugins.TabIndex = 7
            '
            'clmPluginName
            '
            Me.clmPluginName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.clmPluginName.HeaderText = "Name"
            Me.clmPluginName.Name = "clmPluginName"
            Me.clmPluginName.ReadOnly = True
            Me.clmPluginName.Width = 60
            '
            'clmPluginVersion
            '
            Me.clmPluginVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.clmPluginVersion.HeaderText = "Version"
            Me.clmPluginVersion.Name = "clmPluginVersion"
            Me.clmPluginVersion.ReadOnly = True
            '
            'clmPluginFileName
            '
            Me.clmPluginFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.clmPluginFileName.HeaderText = "File"
            Me.clmPluginFileName.Name = "clmPluginFileName"
            Me.clmPluginFileName.ReadOnly = True
            Me.clmPluginFileName.Width = 48
            '
            'grdComponents
            '
            Me.grdComponents.AllowUserToAddRows = False
            Me.grdComponents.AllowUserToDeleteRows = False
            Me.grdComponents.AllowUserToResizeRows = False
            Me.grdComponents.BackgroundColor = System.Drawing.Color.White
            Me.grdComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdComponents.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
            Me.grdComponents.GridColor = System.Drawing.Color.White
            Me.grdComponents.Location = New System.Drawing.Point(30, 152)
            Me.grdComponents.MultiSelect = False
            Me.grdComponents.Name = "grdComponents"
            Me.grdComponents.ReadOnly = True
            Me.grdComponents.RowHeadersVisible = False
            Me.grdComponents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.grdComponents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.grdComponents.Size = New System.Drawing.Size(442, 68)
            Me.grdComponents.TabIndex = 8
            '
            'DataGridViewTextBoxColumn1
            '
            Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
            Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
            Me.DataGridViewTextBoxColumn1.ReadOnly = True
            Me.DataGridViewTextBoxColumn1.Width = 60
            '
            'DataGridViewTextBoxColumn2
            '
            Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.DataGridViewTextBoxColumn2.HeaderText = "Version"
            Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
            Me.DataGridViewTextBoxColumn2.ReadOnly = True
            '
            'tcInfo
            '
            Me.tcInfo.Controls.Add(Me.TabPage1)
            Me.tcInfo.Controls.Add(Me.TabPage2)
            Me.tcInfo.Location = New System.Drawing.Point(30, 357)
            Me.tcInfo.Name = "tcInfo"
            Me.tcInfo.SelectedIndex = 0
            Me.tcInfo.Size = New System.Drawing.Size(442, 193)
            Me.tcInfo.TabIndex = 8
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.txtDetails)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(2)
            Me.TabPage1.Size = New System.Drawing.Size(434, 167)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Details"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'txtDetails
            '
            Me.txtDetails.BackColor = System.Drawing.SystemColors.Window
            Me.txtDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtDetails.Location = New System.Drawing.Point(2, 2)
            Me.txtDetails.Name = "txtDetails"
            Me.txtDetails.ReadOnly = True
            Me.txtDetails.Size = New System.Drawing.Size(430, 163)
            Me.txtDetails.TabIndex = 0
            Me.txtDetails.Text = ""
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.txtLicence)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(2)
            Me.TabPage2.Size = New System.Drawing.Size(434, 167)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Licence"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'txtLicence
            '
            Me.txtLicence.BackColor = System.Drawing.SystemColors.Window
            Me.txtLicence.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.txtLicence.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtLicence.Location = New System.Drawing.Point(2, 2)
            Me.txtLicence.Name = "txtLicence"
            Me.txtLicence.ReadOnly = True
            Me.txtLicence.Size = New System.Drawing.Size(430, 163)
            Me.txtLicence.TabIndex = 1
            Me.txtLicence.Text = ""
            '
            'About
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(500, 574)
            Me.Controls.Add(Me.tcInfo)
            Me.Controls.Add(Me.grdComponents)
            Me.Controls.Add(Me.grdPlugins)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblComponents)
            Me.Controls.Add(Me.Panel1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "About"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "About Metadrone"
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            CType(Me.grdPlugins, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.grdComponents, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tcInfo.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage2.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lblVersion As System.Windows.Forms.Label
        Friend WithEvents lblCopyright As System.Windows.Forms.Label
        Friend WithEvents lnkDownload As System.Windows.Forms.LinkLabel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents lblComponents As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents grdPlugins As System.Windows.Forms.DataGridView
        Friend WithEvents grdComponents As System.Windows.Forms.DataGridView
        Friend WithEvents clmPluginName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents clmPluginVersion As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents clmPluginFileName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents tcInfo As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents txtDetails As System.Windows.Forms.RichTextBox
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents txtLicence As System.Windows.Forms.RichTextBox
    End Class

End Namespace