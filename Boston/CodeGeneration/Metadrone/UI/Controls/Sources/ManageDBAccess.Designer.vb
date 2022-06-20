Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class ManageDBAccess
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageDBAccess))
            Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.lblTitle = New System.Windows.Forms.Label
            Me.picAccess2K = New System.Windows.Forms.PictureBox
            Me.rbAccess2K = New System.Windows.Forms.RadioButton
            Me.picAccess = New System.Windows.Forms.PictureBox
            Me.rbAccess = New System.Windows.Forms.RadioButton
            Me.btnFile = New System.Windows.Forms.Button
            Me.btnTest = New System.Windows.Forms.Button
            Me.lblFile = New System.Windows.Forms.Label
            Me.txtConnectionString = New System.Windows.Forms.TextBox
            Me.txtFile = New System.Windows.Forms.TextBox
            Me.lblConnectionString = New System.Windows.Forms.Label
            Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.splitMain = New System.Windows.Forms.SplitContainer
            Me.pnlQuery = New System.Windows.Forms.Panel
            Me.txtTableSchemaQuery = New UI.SQLEditor
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Label1 = New System.Windows.Forms.Label
            Me.lnkPreviewSchema = New System.Windows.Forms.LinkLabel
            Me.QueryResults = New UI.QueryResults
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.rbQuery = New System.Windows.Forms.RadioButton
            Me.rbGeneric = New System.Windows.Forms.RadioButton
            Me.tcMain = New System.Windows.Forms.TabControl
            Me.TabPage3 = New System.Windows.Forms.TabPage
            Me.txtTransformations = New UI.TransformationsEditor
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.TabPage1.SuspendLayout()
            CType(Me.picAccess2K, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TabPage2.SuspendLayout()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            Me.pnlQuery.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.tcMain.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            Me.SuspendLayout()
            '
            'TabPage1
            '
            Me.TabPage1.BackColor = System.Drawing.Color.Transparent
            Me.TabPage1.Controls.Add(Me.Panel2)
            Me.TabPage1.Controls.Add(Me.lblTitle)
            Me.TabPage1.Controls.Add(Me.picAccess2K)
            Me.TabPage1.Controls.Add(Me.rbAccess2K)
            Me.TabPage1.Controls.Add(Me.picAccess)
            Me.TabPage1.Controls.Add(Me.rbAccess)
            Me.TabPage1.Controls.Add(Me.btnFile)
            Me.TabPage1.Controls.Add(Me.btnTest)
            Me.TabPage1.Controls.Add(Me.lblFile)
            Me.TabPage1.Controls.Add(Me.txtConnectionString)
            Me.TabPage1.Controls.Add(Me.txtFile)
            Me.TabPage1.Controls.Add(Me.lblConnectionString)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 4)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(973, 574)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Connection"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.Silver
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(3, 33)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(967, 1)
            Me.Panel2.TabIndex = 0
            '
            'lblTitle
            '
            Me.lblTitle.BackColor = System.Drawing.Color.White
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.DimGray
            Me.lblTitle.Location = New System.Drawing.Point(3, 3)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Padding = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.lblTitle.Size = New System.Drawing.Size(967, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Microsoft Access"
            '
            'picAccess2K
            '
            Me.picAccess2K.Image = CType(resources.GetObject("picAccess2K.Image"), System.Drawing.Image)
            Me.picAccess2K.Location = New System.Drawing.Point(79, 115)
            Me.picAccess2K.Name = "picAccess2K"
            Me.picAccess2K.Size = New System.Drawing.Size(16, 16)
            Me.picAccess2K.TabIndex = 30
            Me.picAccess2K.TabStop = False
            '
            'rbAccess2K
            '
            Me.rbAccess2K.Location = New System.Drawing.Point(101, 114)
            Me.rbAccess2K.Name = "rbAccess2K"
            Me.rbAccess2K.Size = New System.Drawing.Size(108, 17)
            Me.rbAccess2K.TabIndex = 4
            Me.rbAccess2K.Text = "Access 97 - 2003"
            Me.rbAccess2K.UseVisualStyleBackColor = True
            '
            'picAccess
            '
            Me.picAccess.Image = CType(resources.GetObject("picAccess.Image"), System.Drawing.Image)
            Me.picAccess.Location = New System.Drawing.Point(79, 92)
            Me.picAccess.Name = "picAccess"
            Me.picAccess.Size = New System.Drawing.Size(16, 16)
            Me.picAccess.TabIndex = 28
            Me.picAccess.TabStop = False
            '
            'rbAccess
            '
            Me.rbAccess.Checked = True
            Me.rbAccess.Location = New System.Drawing.Point(101, 91)
            Me.rbAccess.Name = "rbAccess"
            Me.rbAccess.Size = New System.Drawing.Size(108, 17)
            Me.rbAccess.TabIndex = 3
            Me.rbAccess.TabStop = True
            Me.rbAccess.Text = "Access"
            Me.rbAccess.UseVisualStyleBackColor = True
            '
            'btnFile
            '
            Me.btnFile.Location = New System.Drawing.Point(396, 57)
            Me.btnFile.Name = "btnFile"
            Me.btnFile.Size = New System.Drawing.Size(25, 22)
            Me.btnFile.TabIndex = 2
            Me.btnFile.Text = "..."
            Me.btnFile.UseVisualStyleBackColor = True
            '
            'btnTest
            '
            Me.btnTest.Image = CType(resources.GetObject("btnTest.Image"), System.Drawing.Image)
            Me.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnTest.Location = New System.Drawing.Point(20, 216)
            Me.btnTest.Name = "btnTest"
            Me.btnTest.Size = New System.Drawing.Size(112, 30)
            Me.btnTest.TabIndex = 7
            Me.btnTest.Text = "Test Connection"
            Me.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnTest.UseVisualStyleBackColor = True
            '
            'lblFile
            '
            Me.lblFile.AutoSize = True
            Me.lblFile.Location = New System.Drawing.Point(17, 61)
            Me.lblFile.Name = "lblFile"
            Me.lblFile.Size = New System.Drawing.Size(26, 13)
            Me.lblFile.TabIndex = 0
            Me.lblFile.Text = "File:"
            '
            'txtConnectionString
            '
            Me.txtConnectionString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConnectionString.Location = New System.Drawing.Point(20, 181)
            Me.txtConnectionString.Name = "txtConnectionString"
            Me.txtConnectionString.Size = New System.Drawing.Size(934, 20)
            Me.txtConnectionString.TabIndex = 6
            '
            'txtFile
            '
            Me.txtFile.Location = New System.Drawing.Point(79, 58)
            Me.txtFile.Name = "txtFile"
            Me.txtFile.Size = New System.Drawing.Size(317, 20)
            Me.txtFile.TabIndex = 1
            '
            'lblConnectionString
            '
            Me.lblConnectionString.AutoSize = True
            Me.lblConnectionString.Location = New System.Drawing.Point(17, 165)
            Me.lblConnectionString.Name = "lblConnectionString"
            Me.lblConnectionString.Size = New System.Drawing.Size(94, 13)
            Me.lblConnectionString.TabIndex = 5
            Me.lblConnectionString.Text = "Connection String:"
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.splitMain)
            Me.TabPage2.Controls.Add(Me.Panel1)
            Me.TabPage2.ImageIndex = 1
            Me.TabPage2.Location = New System.Drawing.Point(4, 4)
            Me.TabPage2.Margin = New System.Windows.Forms.Padding(0)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(973, 574)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Meta Data"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'splitMain
            '
            Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitMain.Location = New System.Drawing.Point(0, 66)
            Me.splitMain.Name = "splitMain"
            Me.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitMain.Panel1
            '
            Me.splitMain.Panel1.Controls.Add(Me.pnlQuery)
            '
            'splitMain.Panel2
            '
            Me.splitMain.Panel2.Controls.Add(Me.QueryResults)
            Me.splitMain.Panel2Collapsed = True
            Me.splitMain.Size = New System.Drawing.Size(973, 508)
            Me.splitMain.SplitterDistance = 254
            Me.splitMain.TabIndex = 2
            '
            'pnlQuery
            '
            Me.pnlQuery.Controls.Add(Me.txtTableSchemaQuery)
            Me.pnlQuery.Controls.Add(Me.Panel3)
            Me.pnlQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlQuery.Location = New System.Drawing.Point(0, 0)
            Me.pnlQuery.Name = "pnlQuery"
            Me.pnlQuery.Size = New System.Drawing.Size(973, 508)
            Me.pnlQuery.TabIndex = 1
            Me.pnlQuery.Visible = False
            '
            'txtTableSchemaQuery
            '
            Me.txtTableSchemaQuery.BackColor = System.Drawing.SystemColors.Window
            Me.txtTableSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtTableSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtTableSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtTableSchemaQuery.Location = New System.Drawing.Point(0, 24)
            Me.txtTableSchemaQuery.Name = "txtTableSchemaQuery"
            Me.txtTableSchemaQuery.ReadOnly = False
            Me.txtTableSchemaQuery.SelectedText = ""
            Me.txtTableSchemaQuery.SelectionLength = 0
            Me.txtTableSchemaQuery.SelectionStart = 0
            Me.txtTableSchemaQuery.Size = New System.Drawing.Size(973, 484)
            Me.txtTableSchemaQuery.TabIndex = 5
            '
            'Panel3
            '
            Me.Panel3.BackColor = System.Drawing.Color.White
            Me.Panel3.Controls.Add(Me.Label1)
            Me.Panel3.Controls.Add(Me.lnkPreviewSchema)
            Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel3.Location = New System.Drawing.Point(0, 0)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(973, 24)
            Me.Panel3.TabIndex = 1
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(0, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(909, 24)
            Me.Label1.TabIndex = 1
            Me.Label1.Text = "Table Schema Query"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lnkPreviewSchema
            '
            Me.lnkPreviewSchema.BackColor = System.Drawing.Color.Transparent
            Me.lnkPreviewSchema.Dock = System.Windows.Forms.DockStyle.Right
            Me.lnkPreviewSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lnkPreviewSchema.Location = New System.Drawing.Point(909, 0)
            Me.lnkPreviewSchema.Name = "lnkPreviewSchema"
            Me.lnkPreviewSchema.Size = New System.Drawing.Size(64, 24)
            Me.lnkPreviewSchema.TabIndex = 5
            Me.lnkPreviewSchema.TabStop = True
            Me.lnkPreviewSchema.Text = "Preview"
            Me.lnkPreviewSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.lnkPreviewSchema, "Preview query results (F5)")
            '
            'QueryResults
            '
            Me.QueryResults.Dock = System.Windows.Forms.DockStyle.Fill
            Me.QueryResults.Location = New System.Drawing.Point(0, 0)
            Me.QueryResults.Messages = ""
            Me.QueryResults.Name = "QueryResults"
            Me.QueryResults.Size = New System.Drawing.Size(150, 46)
            Me.QueryResults.TabIndex = 0
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.rbQuery)
            Me.Panel1.Controls.Add(Me.rbGeneric)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(973, 66)
            Me.Panel1.TabIndex = 0
            '
            'rbQuery
            '
            Me.rbQuery.AutoSize = True
            Me.rbQuery.Location = New System.Drawing.Point(13, 35)
            Me.rbQuery.Name = "rbQuery"
            Me.rbQuery.Size = New System.Drawing.Size(154, 17)
            Me.rbQuery.TabIndex = 1
            Me.rbQuery.Text = "Use query to retrieve tables"
            Me.rbQuery.UseVisualStyleBackColor = True
            '
            'rbGeneric
            '
            Me.rbGeneric.AutoSize = True
            Me.rbGeneric.Checked = True
            Me.rbGeneric.Location = New System.Drawing.Point(13, 12)
            Me.rbGeneric.Name = "rbGeneric"
            Me.rbGeneric.Size = New System.Drawing.Size(188, 17)
            Me.rbGeneric.TabIndex = 0
            Me.rbGeneric.TabStop = True
            Me.rbGeneric.Text = "Use generic table schema retrieval"
            Me.rbGeneric.UseVisualStyleBackColor = True
            '
            'tcMain
            '
            Me.tcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom
            Me.tcMain.Controls.Add(Me.TabPage1)
            Me.tcMain.Controls.Add(Me.TabPage2)
            Me.tcMain.Controls.Add(Me.TabPage3)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.ImageList = Me.ImageList1
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(981, 601)
            Me.tcMain.TabIndex = 0
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.txtTransformations)
            Me.TabPage3.ImageIndex = 2
            Me.TabPage3.Location = New System.Drawing.Point(4, 4)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Size = New System.Drawing.Size(973, 574)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Transformations"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'txtTransformations
            '
            Me.txtTransformations.BackColor = System.Drawing.SystemColors.Window
            Me.txtTransformations.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtTransformations.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtTransformations.Location = New System.Drawing.Point(0, 0)
            Me.txtTransformations.Name = "txtTransformations"
            Me.txtTransformations.ReadOnly = False
            Me.txtTransformations.SelectedText = ""
            Me.txtTransformations.SelectionLength = 0
            Me.txtTransformations.SelectionStart = 0
            Me.txtTransformations.Size = New System.Drawing.Size(973, 574)
            Me.txtTransformations.TabIndex = 1
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "connection16x16.png")
            Me.ImageList1.Images.SetKeyName(1, "tblmeta16x16.png")
            Me.ImageList1.Images.SetKeyName(2, "transform16x16.png")
            '
            'ToolTip1
            '
            Me.ToolTip1.AutoPopDelay = 5000
            Me.ToolTip1.InitialDelay = 200
            Me.ToolTip1.ReshowDelay = 100
            '
            'ManageDBAccess
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tcMain)
            Me.Name = "ManageDBAccess"
            Me.Size = New System.Drawing.Size(981, 601)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            CType(Me.picAccess2K, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TabPage2.ResumeLayout(False)
            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            Me.splitMain.ResumeLayout(False)
            Me.pnlQuery.ResumeLayout(False)
            Me.Panel3.ResumeLayout(False)
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.tcMain.ResumeLayout(False)
            Me.TabPage3.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents btnFile As System.Windows.Forms.Button
        Friend WithEvents btnTest As System.Windows.Forms.Button
        Friend WithEvents lblFile As System.Windows.Forms.Label
        Friend WithEvents txtConnectionString As System.Windows.Forms.TextBox
        Friend WithEvents txtFile As System.Windows.Forms.TextBox
        Friend WithEvents lblConnectionString As System.Windows.Forms.Label
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents tcMain As System.Windows.Forms.TabControl
        Friend WithEvents picAccess2K As System.Windows.Forms.PictureBox
        Friend WithEvents rbAccess2K As System.Windows.Forms.RadioButton
        Friend WithEvents picAccess As System.Windows.Forms.PictureBox
        Friend WithEvents rbAccess As System.Windows.Forms.RadioButton
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents rbQuery As System.Windows.Forms.RadioButton
        Friend WithEvents rbGeneric As System.Windows.Forms.RadioButton
        Friend WithEvents pnlQuery As System.Windows.Forms.Panel
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents lnkPreviewSchema As System.Windows.Forms.LinkLabel
        Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
        Friend WithEvents QueryResults As UI.QueryResults
        Friend WithEvents txtTableSchemaQuery As UI.SQLEditor
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents txtTransformations As UI.TransformationsEditor
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    End Class

End Namespace