Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class ManageDBOracle
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageDBOracle))
            Me.tcMain = New System.Windows.Forms.TabControl
            Me.TabPage1 = New System.Windows.Forms.TabPage
            Me.lblPort = New System.Windows.Forms.Label
            Me.txtPort = New System.Windows.Forms.TextBox
            Me.lblServer = New System.Windows.Forms.Label
            Me.txtServer = New System.Windows.Forms.TextBox
            Me.lblSID = New System.Windows.Forms.Label
            Me.txtPassword = New System.Windows.Forms.TextBox
            Me.txtSID = New System.Windows.Forms.TextBox
            Me.lblPassword = New System.Windows.Forms.Label
            Me.lblUsername = New System.Windows.Forms.Label
            Me.txtUsername = New System.Windows.Forms.TextBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.lblTitle = New System.Windows.Forms.Label
            Me.btnTest = New System.Windows.Forms.Button
            Me.txtConnectionString = New System.Windows.Forms.TextBox
            Me.lblConnectionString = New System.Windows.Forms.Label
            Me.TabPage2 = New System.Windows.Forms.TabPage
            Me.splitMain = New System.Windows.Forms.SplitContainer
            Me.splitQuery = New System.Windows.Forms.SplitContainer
            Me.txtSchemaQuery = New Boston.UI.SQLEditor
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.lblSchemaQuery = New System.Windows.Forms.Label
            Me.lnkPreviewSchema = New System.Windows.Forms.LinkLabel
            Me.splitTableColumn = New System.Windows.Forms.SplitContainer
            Me.txtTableSchemaQuery = New Boston.UI.SQLEditor
            Me.lblTableSchemaQuery = New System.Windows.Forms.Label
            Me.txtColumnSchemaQuery = New Boston.UI.SQLEditor
            Me.lblColumnSchemaQuery = New System.Windows.Forms.Label
            Me.QueryResults = New Boston.UI.QueryResults
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.grpApproach = New System.Windows.Forms.GroupBox
            Me.rbApproachSingle = New System.Windows.Forms.RadioButton
            Me.rbApproachTableColumn = New System.Windows.Forms.RadioButton
            Me.grpColumnSchema = New System.Windows.Forms.GroupBox
            Me.txtTableName = New System.Windows.Forms.TextBox
            Me.Label3 = New System.Windows.Forms.Label
            Me.grpTableSchema = New System.Windows.Forms.GroupBox
            Me.rbTableDefault = New System.Windows.Forms.RadioButton
            Me.rbTableQuery = New System.Windows.Forms.RadioButton
            Me.TabPage3 = New System.Windows.Forms.TabPage
            Me.splitRoutine = New System.Windows.Forms.SplitContainer
            Me.txtRoutineSchemaQuery = New Boston.UI.SQLEditor
            Me.Panel5 = New System.Windows.Forms.Panel
            Me.Label2 = New System.Windows.Forms.Label
            Me.lnkPreviewRoutineSchema = New System.Windows.Forms.LinkLabel
            Me.RoutineQueryResults = New Boston.UI.QueryResults
            Me.TabPage4 = New System.Windows.Forms.TabPage
            Me.txtTransformations = New Boston.UI.TransformationsEditor
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.tcMain.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            Me.splitQuery.Panel1.SuspendLayout()
            Me.splitQuery.Panel2.SuspendLayout()
            Me.splitQuery.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.splitTableColumn.Panel1.SuspendLayout()
            Me.splitTableColumn.Panel2.SuspendLayout()
            Me.splitTableColumn.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.grpApproach.SuspendLayout()
            Me.grpColumnSchema.SuspendLayout()
            Me.grpTableSchema.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            Me.splitRoutine.Panel1.SuspendLayout()
            Me.splitRoutine.Panel2.SuspendLayout()
            Me.splitRoutine.SuspendLayout()
            Me.Panel5.SuspendLayout()
            Me.TabPage4.SuspendLayout()
            Me.SuspendLayout()
            '
            'tcMain
            '
            Me.tcMain.Alignment = System.Windows.Forms.TabAlignment.Bottom
            Me.tcMain.Controls.Add(Me.TabPage1)
            Me.tcMain.Controls.Add(Me.TabPage2)
            Me.tcMain.Controls.Add(Me.TabPage3)
            Me.tcMain.Controls.Add(Me.TabPage4)
            Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tcMain.ImageList = Me.ImageList1
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(688, 599)
            Me.tcMain.TabIndex = 0
            '
            'TabPage1
            '
            Me.TabPage1.BackColor = System.Drawing.Color.Transparent
            Me.TabPage1.Controls.Add(Me.lblPort)
            Me.TabPage1.Controls.Add(Me.txtPort)
            Me.TabPage1.Controls.Add(Me.lblServer)
            Me.TabPage1.Controls.Add(Me.txtServer)
            Me.TabPage1.Controls.Add(Me.lblSID)
            Me.TabPage1.Controls.Add(Me.txtPassword)
            Me.TabPage1.Controls.Add(Me.txtSID)
            Me.TabPage1.Controls.Add(Me.lblPassword)
            Me.TabPage1.Controls.Add(Me.lblUsername)
            Me.TabPage1.Controls.Add(Me.txtUsername)
            Me.TabPage1.Controls.Add(Me.Panel2)
            Me.TabPage1.Controls.Add(Me.lblTitle)
            Me.TabPage1.Controls.Add(Me.btnTest)
            Me.TabPage1.Controls.Add(Me.txtConnectionString)
            Me.TabPage1.Controls.Add(Me.lblConnectionString)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 4)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(680, 572)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Connection"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'lblPort
            '
            Me.lblPort.AutoSize = True
            Me.lblPort.Location = New System.Drawing.Point(19, 83)
            Me.lblPort.Name = "lblPort"
            Me.lblPort.Size = New System.Drawing.Size(29, 13)
            Me.lblPort.TabIndex = 2
            Me.lblPort.Text = "Port:"
            '
            'txtPort
            '
            Me.txtPort.Location = New System.Drawing.Point(81, 80)
            Me.txtPort.Name = "txtPort"
            Me.txtPort.Size = New System.Drawing.Size(75, 20)
            Me.txtPort.TabIndex = 3
            Me.txtPort.Text = "1521"
            '
            'lblServer
            '
            Me.lblServer.AutoSize = True
            Me.lblServer.Location = New System.Drawing.Point(19, 59)
            Me.lblServer.Name = "lblServer"
            Me.lblServer.Size = New System.Drawing.Size(32, 13)
            Me.lblServer.TabIndex = 0
            Me.lblServer.Text = "Host:"
            '
            'txtServer
            '
            Me.txtServer.Location = New System.Drawing.Point(81, 56)
            Me.txtServer.Name = "txtServer"
            Me.txtServer.Size = New System.Drawing.Size(317, 20)
            Me.txtServer.TabIndex = 1
            '
            'lblSID
            '
            Me.lblSID.AutoSize = True
            Me.lblSID.Location = New System.Drawing.Point(19, 109)
            Me.lblSID.Name = "lblSID"
            Me.lblSID.Size = New System.Drawing.Size(28, 13)
            Me.lblSID.TabIndex = 4
            Me.lblSID.Text = "SID:"
            '
            'txtPassword
            '
            Me.txtPassword.Location = New System.Drawing.Point(81, 158)
            Me.txtPassword.Name = "txtPassword"
            Me.txtPassword.Size = New System.Drawing.Size(317, 20)
            Me.txtPassword.TabIndex = 9
            '
            'txtSID
            '
            Me.txtSID.Location = New System.Drawing.Point(81, 106)
            Me.txtSID.Name = "txtSID"
            Me.txtSID.Size = New System.Drawing.Size(317, 20)
            Me.txtSID.TabIndex = 5
            '
            'lblPassword
            '
            Me.lblPassword.AutoSize = True
            Me.lblPassword.Location = New System.Drawing.Point(19, 161)
            Me.lblPassword.Name = "lblPassword"
            Me.lblPassword.Size = New System.Drawing.Size(56, 13)
            Me.lblPassword.TabIndex = 8
            Me.lblPassword.Text = "Password:"
            '
            'lblUsername
            '
            Me.lblUsername.AutoSize = True
            Me.lblUsername.Location = New System.Drawing.Point(19, 135)
            Me.lblUsername.Name = "lblUsername"
            Me.lblUsername.Size = New System.Drawing.Size(58, 13)
            Me.lblUsername.TabIndex = 6
            Me.lblUsername.Text = "Username:"
            '
            'txtUsername
            '
            Me.txtUsername.Location = New System.Drawing.Point(81, 132)
            Me.txtUsername.Name = "txtUsername"
            Me.txtUsername.Size = New System.Drawing.Size(317, 20)
            Me.txtUsername.TabIndex = 7
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.Silver
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(3, 33)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(674, 1)
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
            Me.lblTitle.Size = New System.Drawing.Size(674, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Oracle"
            '
            'btnTest
            '
            Me.btnTest.Image = CType(resources.GetObject("btnTest.Image"), System.Drawing.Image)
            Me.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnTest.Location = New System.Drawing.Point(22, 302)
            Me.btnTest.Name = "btnTest"
            Me.btnTest.Size = New System.Drawing.Size(112, 30)
            Me.btnTest.TabIndex = 12
            Me.btnTest.Text = "Test Connection"
            Me.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnTest.UseVisualStyleBackColor = True
            '
            'txtConnectionString
            '
            Me.txtConnectionString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConnectionString.Location = New System.Drawing.Point(22, 267)
            Me.txtConnectionString.Name = "txtConnectionString"
            Me.txtConnectionString.Size = New System.Drawing.Size(640, 20)
            Me.txtConnectionString.TabIndex = 11
            '
            'lblConnectionString
            '
            Me.lblConnectionString.AutoSize = True
            Me.lblConnectionString.Location = New System.Drawing.Point(19, 251)
            Me.lblConnectionString.Name = "lblConnectionString"
            Me.lblConnectionString.Size = New System.Drawing.Size(94, 13)
            Me.lblConnectionString.TabIndex = 10
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
            Me.TabPage2.Size = New System.Drawing.Size(680, 572)
            Me.TabPage2.TabIndex = 1
            Me.TabPage2.Text = "Tables/Views Meta Data"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'splitMain
            '
            Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitMain.Location = New System.Drawing.Point(199, 0)
            Me.splitMain.Name = "splitMain"
            Me.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitMain.Panel1
            '
            Me.splitMain.Panel1.Controls.Add(Me.splitQuery)
            '
            'splitMain.Panel2
            '
            Me.splitMain.Panel2.Controls.Add(Me.QueryResults)
            Me.splitMain.Panel2Collapsed = True
            Me.splitMain.Size = New System.Drawing.Size(481, 572)
            Me.splitMain.TabIndex = 1
            '
            'splitQuery
            '
            Me.splitQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitQuery.Location = New System.Drawing.Point(0, 0)
            Me.splitQuery.Name = "splitQuery"
            Me.splitQuery.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitQuery.Panel1
            '
            Me.splitQuery.Panel1.Controls.Add(Me.txtSchemaQuery)
            Me.splitQuery.Panel1.Controls.Add(Me.Panel3)
            '
            'splitQuery.Panel2
            '
            Me.splitQuery.Panel2.Controls.Add(Me.splitTableColumn)
            Me.splitQuery.Size = New System.Drawing.Size(481, 572)
            Me.splitQuery.SplitterDistance = 283
            Me.splitQuery.TabIndex = 1
            '
            'txtSchemaQuery
            '
            Me.txtSchemaQuery.BackColor = System.Drawing.SystemColors.Window
            Me.txtSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtSchemaQuery.Location = New System.Drawing.Point(0, 24)
            Me.txtSchemaQuery.Name = "txtSchemaQuery"
            Me.txtSchemaQuery.ReadOnly = False
            Me.txtSchemaQuery.SelectedText = ""
            Me.txtSchemaQuery.SelectionLength = 0
            Me.txtSchemaQuery.SelectionStart = 0
            Me.txtSchemaQuery.Size = New System.Drawing.Size(481, 259)
            Me.txtSchemaQuery.TabIndex = 5
            '
            'Panel3
            '
            Me.Panel3.BackColor = System.Drawing.Color.White
            Me.Panel3.Controls.Add(Me.lblSchemaQuery)
            Me.Panel3.Controls.Add(Me.lnkPreviewSchema)
            Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel3.Location = New System.Drawing.Point(0, 0)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(481, 24)
            Me.Panel3.TabIndex = 2
            '
            'lblSchemaQuery
            '
            Me.lblSchemaQuery.BackColor = System.Drawing.Color.Transparent
            Me.lblSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblSchemaQuery.Name = "lblSchemaQuery"
            Me.lblSchemaQuery.Size = New System.Drawing.Size(417, 24)
            Me.lblSchemaQuery.TabIndex = 2
            Me.lblSchemaQuery.Text = "Schema Query"
            Me.lblSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lnkPreviewSchema
            '
            Me.lnkPreviewSchema.BackColor = System.Drawing.Color.Transparent
            Me.lnkPreviewSchema.Dock = System.Windows.Forms.DockStyle.Right
            Me.lnkPreviewSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lnkPreviewSchema.Location = New System.Drawing.Point(417, 0)
            Me.lnkPreviewSchema.Name = "lnkPreviewSchema"
            Me.lnkPreviewSchema.Size = New System.Drawing.Size(64, 24)
            Me.lnkPreviewSchema.TabIndex = 4
            Me.lnkPreviewSchema.TabStop = True
            Me.lnkPreviewSchema.Text = "Preview"
            Me.lnkPreviewSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.lnkPreviewSchema, "Preview query results (F5)")
            '
            'splitTableColumn
            '
            Me.splitTableColumn.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitTableColumn.Location = New System.Drawing.Point(0, 0)
            Me.splitTableColumn.Name = "splitTableColumn"
            Me.splitTableColumn.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitTableColumn.Panel1
            '
            Me.splitTableColumn.Panel1.Controls.Add(Me.txtTableSchemaQuery)
            Me.splitTableColumn.Panel1.Controls.Add(Me.lblTableSchemaQuery)
            '
            'splitTableColumn.Panel2
            '
            Me.splitTableColumn.Panel2.Controls.Add(Me.txtColumnSchemaQuery)
            Me.splitTableColumn.Panel2.Controls.Add(Me.lblColumnSchemaQuery)
            Me.splitTableColumn.Size = New System.Drawing.Size(481, 285)
            Me.splitTableColumn.SplitterDistance = 140
            Me.splitTableColumn.TabIndex = 1
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
            Me.txtTableSchemaQuery.Size = New System.Drawing.Size(481, 116)
            Me.txtTableSchemaQuery.TabIndex = 5
            '
            'lblTableSchemaQuery
            '
            Me.lblTableSchemaQuery.BackColor = System.Drawing.Color.White
            Me.lblTableSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTableSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTableSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblTableSchemaQuery.Name = "lblTableSchemaQuery"
            Me.lblTableSchemaQuery.Size = New System.Drawing.Size(481, 24)
            Me.lblTableSchemaQuery.TabIndex = 0
            Me.lblTableSchemaQuery.Text = "Table Schema Query"
            Me.lblTableSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'txtColumnSchemaQuery
            '
            Me.txtColumnSchemaQuery.BackColor = System.Drawing.SystemColors.Window
            Me.txtColumnSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtColumnSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtColumnSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtColumnSchemaQuery.Location = New System.Drawing.Point(0, 24)
            Me.txtColumnSchemaQuery.Name = "txtColumnSchemaQuery"
            Me.txtColumnSchemaQuery.ReadOnly = False
            Me.txtColumnSchemaQuery.SelectedText = ""
            Me.txtColumnSchemaQuery.SelectionLength = 0
            Me.txtColumnSchemaQuery.SelectionStart = 0
            Me.txtColumnSchemaQuery.Size = New System.Drawing.Size(481, 117)
            Me.txtColumnSchemaQuery.TabIndex = 5
            '
            'lblColumnSchemaQuery
            '
            Me.lblColumnSchemaQuery.BackColor = System.Drawing.Color.White
            Me.lblColumnSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblColumnSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblColumnSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblColumnSchemaQuery.Name = "lblColumnSchemaQuery"
            Me.lblColumnSchemaQuery.Size = New System.Drawing.Size(481, 24)
            Me.lblColumnSchemaQuery.TabIndex = 0
            Me.lblColumnSchemaQuery.Text = "Column Schema Query"
            Me.lblColumnSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
            Me.Panel1.Controls.Add(Me.grpApproach)
            Me.Panel1.Controls.Add(Me.grpColumnSchema)
            Me.Panel1.Controls.Add(Me.grpTableSchema)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(199, 572)
            Me.Panel1.TabIndex = 0
            '
            'grpApproach
            '
            Me.grpApproach.Controls.Add(Me.rbApproachSingle)
            Me.grpApproach.Controls.Add(Me.rbApproachTableColumn)
            Me.grpApproach.Location = New System.Drawing.Point(4, 3)
            Me.grpApproach.Name = "grpApproach"
            Me.grpApproach.Size = New System.Drawing.Size(188, 87)
            Me.grpApproach.TabIndex = 0
            Me.grpApproach.TabStop = False
            Me.grpApproach.Text = "Approach"
            '
            'rbApproachSingle
            '
            Me.rbApproachSingle.AutoSize = True
            Me.rbApproachSingle.Checked = True
            Me.rbApproachSingle.Location = New System.Drawing.Point(21, 21)
            Me.rbApproachSingle.Name = "rbApproachSingle"
            Me.rbApproachSingle.Size = New System.Drawing.Size(99, 17)
            Me.rbApproachSingle.TabIndex = 0
            Me.rbApproachSingle.TabStop = True
            Me.rbApproachSingle.Text = "Single result set"
            Me.rbApproachSingle.UseVisualStyleBackColor = True
            '
            'rbApproachTableColumn
            '
            Me.rbApproachTableColumn.AutoSize = True
            Me.rbApproachTableColumn.Location = New System.Drawing.Point(21, 44)
            Me.rbApproachTableColumn.Name = "rbApproachTableColumn"
            Me.rbApproachTableColumn.Size = New System.Drawing.Size(132, 17)
            Me.rbApproachTableColumn.TabIndex = 1
            Me.rbApproachTableColumn.Text = "Table/Column retrieval"
            Me.rbApproachTableColumn.UseVisualStyleBackColor = True
            '
            'grpColumnSchema
            '
            Me.grpColumnSchema.Controls.Add(Me.txtTableName)
            Me.grpColumnSchema.Controls.Add(Me.Label3)
            Me.grpColumnSchema.Location = New System.Drawing.Point(4, 189)
            Me.grpColumnSchema.Name = "grpColumnSchema"
            Me.grpColumnSchema.Size = New System.Drawing.Size(188, 87)
            Me.grpColumnSchema.TabIndex = 2
            Me.grpColumnSchema.TabStop = False
            Me.grpColumnSchema.Text = "Column Schema Retrieval Method"
            '
            'txtTableName
            '
            Me.txtTableName.Location = New System.Drawing.Point(21, 42)
            Me.txtTableName.Name = "txtTableName"
            Me.txtTableName.Size = New System.Drawing.Size(161, 20)
            Me.txtTableName.TabIndex = 3
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(18, 26)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(127, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Table name place-holder:"
            '
            'grpTableSchema
            '
            Me.grpTableSchema.Controls.Add(Me.rbTableDefault)
            Me.grpTableSchema.Controls.Add(Me.rbTableQuery)
            Me.grpTableSchema.Location = New System.Drawing.Point(4, 96)
            Me.grpTableSchema.Name = "grpTableSchema"
            Me.grpTableSchema.Size = New System.Drawing.Size(188, 87)
            Me.grpTableSchema.TabIndex = 1
            Me.grpTableSchema.TabStop = False
            Me.grpTableSchema.Text = "Table Schema Retrieval Method"
            '
            'rbTableDefault
            '
            Me.rbTableDefault.AutoSize = True
            Me.rbTableDefault.Checked = True
            Me.rbTableDefault.Location = New System.Drawing.Point(21, 21)
            Me.rbTableDefault.Name = "rbTableDefault"
            Me.rbTableDefault.Size = New System.Drawing.Size(62, 17)
            Me.rbTableDefault.TabIndex = 0
            Me.rbTableDefault.TabStop = True
            Me.rbTableDefault.Text = "Generic"
            Me.rbTableDefault.UseVisualStyleBackColor = True
            '
            'rbTableQuery
            '
            Me.rbTableQuery.AutoSize = True
            Me.rbTableQuery.Location = New System.Drawing.Point(21, 44)
            Me.rbTableQuery.Name = "rbTableQuery"
            Me.rbTableQuery.Size = New System.Drawing.Size(53, 17)
            Me.rbTableQuery.TabIndex = 1
            Me.rbTableQuery.Text = "Query"
            Me.rbTableQuery.UseVisualStyleBackColor = True
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.splitRoutine)
            Me.TabPage3.ImageIndex = 2
            Me.TabPage3.Location = New System.Drawing.Point(4, 4)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Size = New System.Drawing.Size(680, 572)
            Me.TabPage3.TabIndex = 2
            Me.TabPage3.Text = "Routines Meta Data"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'splitRoutine
            '
            Me.splitRoutine.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitRoutine.Location = New System.Drawing.Point(0, 0)
            Me.splitRoutine.Name = "splitRoutine"
            Me.splitRoutine.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitRoutine.Panel1
            '
            Me.splitRoutine.Panel1.Controls.Add(Me.txtRoutineSchemaQuery)
            Me.splitRoutine.Panel1.Controls.Add(Me.Panel5)
            '
            'splitRoutine.Panel2
            '
            Me.splitRoutine.Panel2.Controls.Add(Me.RoutineQueryResults)
            Me.splitRoutine.Panel2Collapsed = True
            Me.splitRoutine.Size = New System.Drawing.Size(680, 572)
            Me.splitRoutine.SplitterDistance = 315
            Me.splitRoutine.TabIndex = 2
            '
            'txtRoutineSchemaQuery
            '
            Me.txtRoutineSchemaQuery.BackColor = System.Drawing.SystemColors.Window
            Me.txtRoutineSchemaQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.txtRoutineSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.txtRoutineSchemaQuery.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtRoutineSchemaQuery.Location = New System.Drawing.Point(0, 24)
            Me.txtRoutineSchemaQuery.Name = "txtRoutineSchemaQuery"
            Me.txtRoutineSchemaQuery.ReadOnly = False
            Me.txtRoutineSchemaQuery.SelectedText = ""
            Me.txtRoutineSchemaQuery.SelectionLength = 0
            Me.txtRoutineSchemaQuery.SelectionStart = 0
            Me.txtRoutineSchemaQuery.Size = New System.Drawing.Size(680, 548)
            Me.txtRoutineSchemaQuery.TabIndex = 0
            '
            'Panel5
            '
            Me.Panel5.BackColor = System.Drawing.Color.White
            Me.Panel5.Controls.Add(Me.Label2)
            Me.Panel5.Controls.Add(Me.lnkPreviewRoutineSchema)
            Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel5.Location = New System.Drawing.Point(0, 0)
            Me.Panel5.Name = "Panel5"
            Me.Panel5.Size = New System.Drawing.Size(680, 24)
            Me.Panel5.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.Transparent
            Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label2.Location = New System.Drawing.Point(0, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(616, 24)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Routine/Parameter Schema Query"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lnkPreviewRoutineSchema
            '
            Me.lnkPreviewRoutineSchema.BackColor = System.Drawing.Color.Transparent
            Me.lnkPreviewRoutineSchema.Dock = System.Windows.Forms.DockStyle.Right
            Me.lnkPreviewRoutineSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lnkPreviewRoutineSchema.Location = New System.Drawing.Point(616, 0)
            Me.lnkPreviewRoutineSchema.Name = "lnkPreviewRoutineSchema"
            Me.lnkPreviewRoutineSchema.Size = New System.Drawing.Size(64, 24)
            Me.lnkPreviewRoutineSchema.TabIndex = 1
            Me.lnkPreviewRoutineSchema.TabStop = True
            Me.lnkPreviewRoutineSchema.Text = "Preview"
            Me.lnkPreviewRoutineSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.ToolTip1.SetToolTip(Me.lnkPreviewRoutineSchema, "Preview query results (F5)")
            '
            'RoutineQueryResults
            '
            Me.RoutineQueryResults.Dock = System.Windows.Forms.DockStyle.Fill
            Me.RoutineQueryResults.Location = New System.Drawing.Point(0, 0)
            Me.RoutineQueryResults.Messages = ""
            Me.RoutineQueryResults.Name = "RoutineQueryResults"
            Me.RoutineQueryResults.Size = New System.Drawing.Size(150, 46)
            Me.RoutineQueryResults.TabIndex = 1
            '
            'TabPage4
            '
            Me.TabPage4.Controls.Add(Me.txtTransformations)
            Me.TabPage4.ImageIndex = 3
            Me.TabPage4.Location = New System.Drawing.Point(4, 4)
            Me.TabPage4.Name = "TabPage4"
            Me.TabPage4.Size = New System.Drawing.Size(680, 572)
            Me.TabPage4.TabIndex = 3
            Me.TabPage4.Text = "Transformations"
            Me.TabPage4.UseVisualStyleBackColor = True
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
            Me.txtTransformations.Size = New System.Drawing.Size(680, 572)
            Me.txtTransformations.TabIndex = 1
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "connection16x16.png")
            Me.ImageList1.Images.SetKeyName(1, "tblmeta16x16.png")
            Me.ImageList1.Images.SetKeyName(2, "fncmeta16x16.png")
            Me.ImageList1.Images.SetKeyName(3, "transform16x16.png")
            '
            'ToolTip1
            '
            Me.ToolTip1.AutoPopDelay = 5000
            Me.ToolTip1.InitialDelay = 200
            Me.ToolTip1.ReshowDelay = 100
            '
            'ManageDBOracle
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tcMain)
            Me.Name = "ManageDBOracle"
            Me.Size = New System.Drawing.Size(688, 599)
            Me.tcMain.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            Me.splitMain.ResumeLayout(False)
            Me.splitQuery.Panel1.ResumeLayout(False)
            Me.splitQuery.Panel2.ResumeLayout(False)
            Me.splitQuery.ResumeLayout(False)
            Me.Panel3.ResumeLayout(False)
            Me.splitTableColumn.Panel1.ResumeLayout(False)
            Me.splitTableColumn.Panel2.ResumeLayout(False)
            Me.splitTableColumn.ResumeLayout(False)
            Me.Panel1.ResumeLayout(False)
            Me.grpApproach.ResumeLayout(False)
            Me.grpApproach.PerformLayout()
            Me.grpColumnSchema.ResumeLayout(False)
            Me.grpColumnSchema.PerformLayout()
            Me.grpTableSchema.ResumeLayout(False)
            Me.grpTableSchema.PerformLayout()
            Me.TabPage3.ResumeLayout(False)
            Me.splitRoutine.Panel1.ResumeLayout(False)
            Me.splitRoutine.Panel2.ResumeLayout(False)
            Me.splitRoutine.ResumeLayout(False)
            Me.Panel5.ResumeLayout(False)
            Me.TabPage4.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents tcMain As System.Windows.Forms.TabControl
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents btnTest As System.Windows.Forms.Button
        Friend WithEvents txtConnectionString As System.Windows.Forms.TextBox
        Friend WithEvents lblConnectionString As System.Windows.Forms.Label
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents grpApproach As System.Windows.Forms.GroupBox
        Friend WithEvents rbApproachSingle As System.Windows.Forms.RadioButton
        Friend WithEvents rbApproachTableColumn As System.Windows.Forms.RadioButton
        Friend WithEvents grpColumnSchema As System.Windows.Forms.GroupBox
        Friend WithEvents txtTableName As System.Windows.Forms.TextBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents grpTableSchema As System.Windows.Forms.GroupBox
        Friend WithEvents rbTableDefault As System.Windows.Forms.RadioButton
        Friend WithEvents rbTableQuery As System.Windows.Forms.RadioButton
        Friend WithEvents txtPort As System.Windows.Forms.TextBox
        Friend WithEvents lblServer As System.Windows.Forms.Label
        Friend WithEvents txtServer As System.Windows.Forms.TextBox
        Friend WithEvents lblSID As System.Windows.Forms.Label
        Friend WithEvents txtPassword As System.Windows.Forms.TextBox
        Friend WithEvents txtSID As System.Windows.Forms.TextBox
        Friend WithEvents lblPassword As System.Windows.Forms.Label
        Friend WithEvents lblUsername As System.Windows.Forms.Label
        Friend WithEvents txtUsername As System.Windows.Forms.TextBox
        Friend WithEvents lblPort As System.Windows.Forms.Label
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
        Friend WithEvents splitQuery As System.Windows.Forms.SplitContainer
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents lblSchemaQuery As System.Windows.Forms.Label
        Friend WithEvents lnkPreviewSchema As System.Windows.Forms.LinkLabel
        Friend WithEvents splitTableColumn As System.Windows.Forms.SplitContainer
        Friend WithEvents lblTableSchemaQuery As System.Windows.Forms.Label
        Friend WithEvents lblColumnSchemaQuery As System.Windows.Forms.Label
        Friend WithEvents QueryResults As Boston.UI.QueryResults
        Friend WithEvents txtSchemaQuery As Boston.UI.SQLEditor
        Friend WithEvents txtTableSchemaQuery As Boston.UI.SQLEditor
        Friend WithEvents txtColumnSchemaQuery As Boston.UI.SQLEditor
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents splitRoutine As System.Windows.Forms.SplitContainer
        Friend WithEvents txtRoutineSchemaQuery As Boston.UI.SQLEditor
        Friend WithEvents Panel5 As System.Windows.Forms.Panel
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lnkPreviewRoutineSchema As System.Windows.Forms.LinkLabel
        Friend WithEvents RoutineQueryResults As Boston.UI.QueryResults
        Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
        Friend WithEvents txtTransformations As Boston.UI.TransformationsEditor
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    End Class

End Namespace