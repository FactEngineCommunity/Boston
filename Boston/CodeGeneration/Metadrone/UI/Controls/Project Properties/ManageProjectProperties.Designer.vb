Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class ManageProjectProperties
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageProjectProperties))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.tsMenu = New System.Windows.Forms.ToolStrip
            Me.btnSupermain = New System.Windows.Forms.ToolStripButton
            Me.btnSettings = New System.Windows.Forms.ToolStripButton
            Me.splitProps = New System.Windows.Forms.SplitContainer
            Me.grdMain = New System.Windows.Forms.DataGridView
            Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.lblDescription = New System.Windows.Forms.Label
            Me.lblTitle = New System.Windows.Forms.Label
            Me.pnlMain = New System.Windows.Forms.Panel
            Me.SuperMain = New Boston.UI.ManageProjectSuperMain
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.tsMenu.SuspendLayout()
            Me.splitProps.Panel1.SuspendLayout()
            Me.splitProps.Panel2.SuspendLayout()
            Me.splitProps.SuspendLayout()
            CType(Me.grdMain, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlMain.SuspendLayout()
            Me.SuspendLayout()
            '
            'tsMenu
            '
            Me.tsMenu.AutoSize = False
            Me.tsMenu.BackColor = System.Drawing.Color.GhostWhite
            Me.tsMenu.Dock = System.Windows.Forms.DockStyle.Left
            Me.tsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.tsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSupermain, Me.btnSettings})
            Me.tsMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
            Me.tsMenu.Location = New System.Drawing.Point(0, 0)
            Me.tsMenu.Name = "tsMenu"
            Me.tsMenu.Padding = New System.Windows.Forms.Padding(0)
            Me.tsMenu.Size = New System.Drawing.Size(120, 554)
            Me.tsMenu.TabIndex = 0
            Me.tsMenu.Text = "ToolStrip1"
            '
            'btnSupermain
            '
            Me.btnSupermain.Image = CType(resources.GetObject("btnSupermain.Image"), System.Drawing.Image)
            Me.btnSupermain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSupermain.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.btnSupermain.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSupermain.Name = "btnSupermain"
            Me.btnSupermain.Size = New System.Drawing.Size(119, 20)
            Me.btnSupermain.Text = "   Super Main"
            '
            'btnSettings
            '
            Me.btnSettings.Image = CType(resources.GetObject("btnSettings.Image"), System.Drawing.Image)
            Me.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.btnSettings.Name = "btnSettings"
            Me.btnSettings.Size = New System.Drawing.Size(119, 20)
            Me.btnSettings.Text = "   Settings"
            '
            'splitProps
            '
            Me.splitProps.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitProps.Location = New System.Drawing.Point(0, 0)
            Me.splitProps.Name = "splitProps"
            '
            'splitProps.Panel1
            '
            Me.splitProps.Panel1.Controls.Add(Me.grdMain)
            '
            'splitProps.Panel2
            '
            Me.splitProps.Panel2.BackColor = System.Drawing.Color.White
            Me.splitProps.Panel2.Controls.Add(Me.lblDescription)
            Me.splitProps.Panel2.Controls.Add(Me.lblTitle)
            Me.splitProps.Size = New System.Drawing.Size(865, 554)
            Me.splitProps.SplitterDistance = 220
            Me.splitProps.TabIndex = 0
            Me.splitProps.Visible = False
            '
            'grdMain
            '
            Me.grdMain.AllowUserToAddRows = False
            Me.grdMain.AllowUserToDeleteRows = False
            Me.grdMain.AllowUserToOrderColumns = True
            Me.grdMain.AllowUserToResizeRows = False
            Me.grdMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.grdMain.BackgroundColor = System.Drawing.Color.GhostWhite
            Me.grdMain.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.grdMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.grdMain.ColumnHeadersVisible = False
            Me.grdMain.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.grdMain.DefaultCellStyle = DataGridViewCellStyle1
            Me.grdMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grdMain.GridColor = System.Drawing.SystemColors.Control
            Me.grdMain.Location = New System.Drawing.Point(0, 0)
            Me.grdMain.MultiSelect = False
            Me.grdMain.Name = "grdMain"
            Me.grdMain.RowHeadersVisible = False
            Me.grdMain.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
            Me.grdMain.Size = New System.Drawing.Size(220, 554)
            Me.grdMain.TabIndex = 16
            '
            'Column1
            '
            Me.Column1.FillWeight = 91.37056!
            Me.Column1.HeaderText = "Column1"
            Me.Column1.Name = "Column1"
            Me.Column1.ReadOnly = True
            '
            'Column2
            '
            Me.Column2.FillWeight = 108.6294!
            Me.Column2.HeaderText = "Column2"
            Me.Column2.Name = "Column2"
            '
            'lblDescription
            '
            Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDescription.ForeColor = System.Drawing.Color.Black
            Me.lblDescription.Location = New System.Drawing.Point(0, 36)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Padding = New System.Windows.Forms.Padding(2, 0, 0, 0)
            Me.lblDescription.Size = New System.Drawing.Size(641, 518)
            Me.lblDescription.TabIndex = 1
            '
            'lblTitle
            '
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.DimGray
            Me.lblTitle.Location = New System.Drawing.Point(0, 0)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(641, 36)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'pnlMain
            '
            Me.pnlMain.Controls.Add(Me.splitProps)
            Me.pnlMain.Controls.Add(Me.SuperMain)
            Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlMain.Location = New System.Drawing.Point(124, 0)
            Me.pnlMain.Name = "pnlMain"
            Me.pnlMain.Size = New System.Drawing.Size(865, 554)
            Me.pnlMain.TabIndex = 1
            '
            'SuperMain
            '
            Me.SuperMain.BackColor = System.Drawing.SystemColors.Window
            Me.SuperMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SuperMain.Location = New System.Drawing.Point(0, 0)
            Me.SuperMain.Name = "SuperMain"
            Me.SuperMain.ReadOnly = False
            Me.SuperMain.SelectedText = ""
            Me.SuperMain.SelectionLength = 0
            Me.SuperMain.SelectionStart = 0
            Me.SuperMain.Size = New System.Drawing.Size(865, 554)
            Me.SuperMain.TabIndex = 0
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.LightSteelBlue
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
            Me.Panel1.Location = New System.Drawing.Point(120, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(4, 554)
            Me.Panel1.TabIndex = 2
            '
            'ManageProjectProperties
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.GhostWhite
            Me.Controls.Add(Me.pnlMain)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.tsMenu)
            Me.Name = "ManageProjectProperties"
            Me.Size = New System.Drawing.Size(989, 554)
            Me.tsMenu.ResumeLayout(False)
            Me.tsMenu.PerformLayout()
            Me.splitProps.Panel1.ResumeLayout(False)
            Me.splitProps.Panel2.ResumeLayout(False)
            Me.splitProps.ResumeLayout(False)
            CType(Me.grdMain, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlMain.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents splitProps As System.Windows.Forms.SplitContainer
        Friend WithEvents grdMain As System.Windows.Forms.DataGridView
        Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents tsMenu As System.Windows.Forms.ToolStrip
        Friend WithEvents btnSupermain As System.Windows.Forms.ToolStripButton
        Friend WithEvents btnSettings As System.Windows.Forms.ToolStripButton
        Friend WithEvents SuperMain As Boston.UI.ManageProjectSuperMain
        Friend WithEvents pnlMain As System.Windows.Forms.Panel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel

    End Class

End Namespace