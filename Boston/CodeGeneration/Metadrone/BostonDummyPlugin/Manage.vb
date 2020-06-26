Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Partial Public Class Manage
        Inherits UserControl
        Implements PluginInterface.Sources.IManageSource

        Public Event ValueChanged As PluginInterface.Sources.IManageSource.ValueChangedEventHandler
        Public Delegate Sub ValueChangedEventHandler(ByVal value As Object)
        Public Event Save As PluginInterface.Sources.IManageSource.SaveEventHandler
        Private Event IManageSource_ValueChanged(value As Object) Implements IManageSource.ValueChanged
        Private Event IManageSource_Save() Implements IManageSource.Save

        Public Delegate Sub SaveEventHandler()

        Public Sub New()
            InitializeComponent()

            AddHandler Me.Load, New System.EventHandler(AddressOf Me.Manage_Load)
            'AddHandler Me.txtServer.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.chkPort.CheckedChanged, New System.EventHandler(AddressOf Me.chkPort_CheckedChanged)
            'AddHandler Me.txtPort.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtDatabase.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtUsername.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtPassword.TextChanged, New System.EventHandler(AddressOf Me.txtConnParts_TextChanged)
            'AddHandler Me.txtConnectionString.TextChanged, New System.EventHandler(AddressOf Me.txtConnectionString_TextChanged)
            'AddHandler Me.btnTest.Click, New System.EventHandler(AddressOf Me.btnTest_Click)
            'Me.txtSchemaQuery.KeyDown += New Metadrone.UI.SQLEditor.KeyDownEventHandler(AddressOf Me.txtSchemaQuery_KeyDown)
            'Me.lnkPreviewSchema.LinkClicked += New System.Windows.Forms.LinkLabelLinkClickedEventHandler(AddressOf Me.lnkPreviewSchema_LinkClicked)
            'Me.txtRoutineSchemaQuery.KeyDown += New Metadrone.UI.SQLEditor.KeyDownEventHandler(AddressOf Me.txtRoutineSchemaQuery_KeyDown)
            'Me.lnkPreviewRoutineSchema.LinkClicked += New System.Windows.Forms.LinkLabelLinkClickedEventHandler(AddressOf Me.lnkPreviewRoutineSchema_LinkClicked)
            'Me.txtSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtSchemaQuery_TextChanged)
            'Me.txtSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtTableSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtTableSchemaQuery_TextChanged)
            'Me.txtTableSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtColumnSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtColumnSchemaQuery_TextChanged)
            'Me.txtColumnSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtRoutineSchemaQuery.TextChanged += New Metadrone.UI.SQLEditor.TextChangedEventHandler(AddressOf Me.txtRoutineSchemaQuery_TextChanged)
            'Me.txtRoutineSchemaQuery.SavePress += New Metadrone.UI.SQLEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'AddHandler Me.txtTableName.TextChanged, New System.EventHandler(AddressOf Me.txtTableName_TextChanged)
            'AddHandler Me.rbApproachSingle.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbApproachTableColumn.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbTableDefault.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'AddHandler Me.rbTableQuery.CheckedChanged, New System.EventHandler(AddressOf Me.rbMeta_CheckedChanged)
            'Me.txtTransformations.SavePress += New Metadrone.UI.TransformationsEditor.SavePressEventHandler(AddressOf Me.SavePress)
            'Me.txtTransformations.TextChanged += New Metadrone.UI.TransformationsEditor.TextChangedEventHandler(AddressOf Me.txtTransformations_TextChanged)
        End Sub

        Public Sub Setup()
            If Me.SchemaQuery.Length = 0 Then
                'Me.SchemaQuery = New Connection("", "").GetQuery(Connection.QueryEnum.SchemaQuery)
            End If

            If Me.RoutineSchemaQuery.Length = 0 Then
                'Me.RoutineSchemaQuery = New Connection("", "").GetQuery(Connection.QueryEnum.RoutineSchemaQuery)
            End If

            Me.splitMain.Panel2Collapsed = True

        End Sub

        Private Sub TestConn()
            'Dim conn As Connection = New Connection("", Me.txtConnectionString.Text)
            'conn.TestConnection()
        End Sub

        Public Property ConnectionString As String Implements IManageSource.ConnectionString
            Get
                Return "None Required"
            End Get
            Set(ByVal value As String)
                'Nothing to do here
            End Set
        End Property

        Public Property SingleResultApproach As Boolean Implements IManageSource.SingleResultApproach
            Get
                Return True 'Me.rbApproachSingle.Checked
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
                'Me.rbApproachSingle.Checked = value
                'Me.rbApproachTableColumn.Checked = Not value
            End Set
        End Property

        Public Property TableSchemaGeneric As Boolean Implements IManageSource.TableSchemaGeneric
            Get
                Return True 'Me.rbTableDefault.Checked
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
                'Me.rbTableDefault.Checked = value
                'Me.rbTableQuery.Checked = Not value
            End Set
        End Property

        Public Property ColumnSchemaGeneric As Boolean Implements IManageSource.ColumnSchemaGeneric
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)
                'Nothing to do here
            End Set
        End Property

        Public Property SchemaQuery As String Implements IManageSource.SchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all" 'Me.txtSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableSchemaQuery As String Implements IManageSource.TableSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all" 'Me.txtTableSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtTableSchemaQuery.Text = value
            End Set
        End Property

        Public Property ColumnSchemaQuery As String Implements IManageSource.ColumnSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here at all." 'Return Me.txtColumnSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtColumnSchemaQuery.Text = value
            End Set
        End Property

        Public Property TableName As String Implements IManageSource.TableName
            Get
                Return "Boston Model direct query. No need for a Schema Query here." 'Return Me.txtTableName.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtTableName.Text = value
            End Set
        End Property

        Public Property RoutineSchemaQuery As String Implements IManageSource.RoutineSchemaQuery
            Get
                Return "Boston Model direct query. No need for a Schema Query here." 'Return Me.txtRoutineSchemaQuery.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtRoutineSchemaQuery.Text = value
            End Set
        End Property

        Public Property Transformations As String Implements IManageSource.Transformations
            Get
                Return "Boston Model direct query. No need for a Schema Query here." 'Return Me.txtTransformations.Text
            End Get
            Set(ByVal value As String)
                'Nothing to do here
                'Me.txtTransformations.Text = value
            End Set
        End Property

        Private Property IManageSource_Transformations As String
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Sub Manage_Load(ByVal sender As Object, ByVal e As EventArgs)
            'Me.rbMeta_CheckedChanged(sender, e)
        End Sub

        Private Sub SavePress()
            RaiseEvent Save()
        End Sub

        'Private Sub txtSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtSchemaQuery.Text)
        'End Sub

        'Private Sub txtTableSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTableSchemaQuery.Text)
        'End Sub

        'Private Sub txtColumnSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtColumnSchemaQuery.Text)
        'End Sub

        'Private Sub txtTableName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTableName.Text)
        'End Sub

        'Private Sub txtRoutineSchemaQuery_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtRoutineSchemaQuery.Text)
        'End Sub

        'Private Sub txtTransformations_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        '    RaiseEvent ValueChanged(Me.txtTransformations.Text)
        'End Sub

        'Private Sub rbMeta_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        '    Me.splitQuery.Panel1Collapsed = Not Me.rbApproachSingle.Checked
        '    Me.splitQuery.Panel2Collapsed = Me.rbApproachSingle.Checked
        '    Me.grpTableSchema.Visible = Not Me.rbApproachSingle.Checked
        '    Me.grpColumnSchema.Visible = Not Me.rbApproachSingle.Checked
        '    Me.splitTableColumn.Panel1Collapsed = Not Me.rbTableQuery.Checked
        '    Dim conn As Connection = New Connection("", "")

        '    If Me.rbApproachSingle.Checked Then
        '        If Me.SchemaQuery.Length = 0 Then Me.SchemaQuery = conn.GetQuery(Connection.QueryEnum.SchemaQuery)
        '        Me.txtTableSchemaQuery.Text = ""
        '        Me.txtColumnSchemaQuery.Text = ""
        '        Me.txtTableName.Text = ""
        '    Else
        '        Me.splitMain.Panel2Collapsed = True

        '        If Me.rbTableQuery.Checked Then
        '            If Me.txtTableSchemaQuery.Text.Length = 0 Then Me.txtTableSchemaQuery.Text = conn.GetQuery(Connection.QueryEnum.TableQuery)
        '        Else
        '            Me.txtTableSchemaQuery.Text = ""
        '        End If

        '        If Me.txtColumnSchemaQuery.Text.Length = 0 Then Me.txtColumnSchemaQuery.Text = conn.GetQuery(Connection.QueryEnum.ColumnQuery)

        '        If Me.txtTableName.Text.Length = 0 Then
        '            Me.txtTableName.Text = Metadrone.Persistence.Source.Default_TableNamePlaceHolder
        '        End If

        '        Me.SchemaQuery = ""
        '    End If
        'End Sub

        Private Sub BuildConnectionString()
            If Me.chkPort.Checked Then
                Me.txtConnectionString.Text = "Server=" & Me.txtServer.Text & ";Port=" + Me.txtPort.Text & ";Database=" + Me.txtDatabase.Text & ";Uid=" + Me.txtUsername.Text & ";Pwd=" + Me.txtPassword.Text
            Else
                Me.txtConnectionString.Text = "Server=" & Me.txtServer.Text & ";Database=" + Me.txtDatabase.Text & ";Uid=" + Me.txtUsername.Text & ";Pwd=" + Me.txtPassword.Text
            End If
        End Sub

        'Private Sub txtConnParts_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.BuildConnectionString()
        'End Sub

        'Private Sub chkPort_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    Me.txtPort.Enabled = Me.chkPort.Checked
        '    Me.BuildConnectionString()
        'End Sub

        'Private Sub txtConnectionString_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        '    RaiseEvent ValueChanged((CType(sender, TextBox)).Text)
        'End Sub

        'Private Sub btnTest_Click(ByVal sender As Object, ByVal e As EventArgs)
        '    Try
        '        Me.Cursor = Cursors.WaitCursor
        '        Me.TestConn()
        '        Me.Cursor = Cursors.[Default]
        '        MessageBox.Show("Test connection successful.", "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '    Catch ex As Exception
        '        Me.Cursor = Cursors.[Default]
        '        MessageBox.Show(ex.Message, "Metadrone", MessageBoxButtons.OK, MessageBoxIcon.[Stop], MessageBoxDefaultButton.Button1)
        '    End Try
        'End Sub

        'Private Sub lnkPreviewSchema_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        '    Me.TestQuery()
        'End Sub

        'Private Sub txtSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If e.KeyCode = Keys.F5 Then
        '        Me.TestQuery()
        '        Me.txtSchemaQuery.Focus()
        '    End If
        'End Sub

        'Private Sub TestQuery()
        '    Me.splitMain.Panel2Collapsed = False
        '    Me.Cursor = Cursors.WaitCursor

        '    Try
        '        Me.QueryResults.PrepareSourceLoad()
        '        Dim dt As Connection = New Connection("", Me.txtConnectionString.Text)
        '        Me.QueryResults.SetSource(dt.TestQuery(Me.SchemaQuery))
        '    Catch ex As Exception
        '        Me.QueryResults.Messages = ex.Message
        '    End Try

        '    Me.Cursor = Cursors.[Default]
        'End Sub

        'Private Sub lnkPreviewRoutineSchema_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        '    Me.TestRoutineQuery()
        'End Sub

        'Private Sub txtRoutineSchemaQuery_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        '    If e.KeyCode = Keys.F5 Then
        '        Me.TestRoutineQuery()
        '        Me.txtRoutineSchemaQuery.Focus()
        '    End If
        'End Sub

        'Private Sub TestRoutineQuery()
        '    Me.splitRoutine.Panel2Collapsed = False
        '    Me.Cursor = Cursors.WaitCursor

        '    Try
        '        Me.RoutineQueryResults.PrepareSourceLoad()
        '        Dim dt As Connection = New Connection("", Me.txtConnectionString.Text)
        '        Me.RoutineQueryResults.SetSource(dt.TestQuery(Me.RoutineSchemaQuery))
        '    Catch ex As Exception
        '        Me.RoutineQueryResults.Messages = ex.Message
        '    End Try

        '    Me.Cursor = Cursors.[Default]
        'End Sub

        Private Sub IManageSource_Setup() Implements IManageSource.Setup
            'Throw New NotImplementedException()
        End Sub

#Region "Form Initialisation"
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Manage))
            Me.tcMain = New System.Windows.Forms.TabControl()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.txtPort = New System.Windows.Forms.TextBox()
            Me.chkPort = New System.Windows.Forms.CheckBox()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.btnTest = New System.Windows.Forms.Button()
            Me.lblServer = New System.Windows.Forms.Label()
            Me.txtConnectionString = New System.Windows.Forms.TextBox()
            Me.txtServer = New System.Windows.Forms.TextBox()
            Me.lblConnectionString = New System.Windows.Forms.Label()
            Me.lblDatabase = New System.Windows.Forms.Label()
            Me.txtPassword = New System.Windows.Forms.TextBox()
            Me.txtDatabase = New System.Windows.Forms.TextBox()
            Me.lblPassword = New System.Windows.Forms.Label()
            Me.lblUsername = New System.Windows.Forms.Label()
            Me.txtUsername = New System.Windows.Forms.TextBox()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.splitMain = New System.Windows.Forms.SplitContainer()
            Me.splitQuery = New System.Windows.Forms.SplitContainer()
            Me.Panel3 = New System.Windows.Forms.Panel()
            Me.lblSchemaQuery = New System.Windows.Forms.Label()
            Me.lnkPreviewSchema = New System.Windows.Forms.LinkLabel()
            Me.splitTableColumn = New System.Windows.Forms.SplitContainer()
            Me.lblTableSchemaQuery = New System.Windows.Forms.Label()
            Me.lblColumnSchemaQuery = New System.Windows.Forms.Label()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.grpApproach = New System.Windows.Forms.GroupBox()
            Me.rbApproachSingle = New System.Windows.Forms.RadioButton()
            Me.rbApproachTableColumn = New System.Windows.Forms.RadioButton()
            Me.grpColumnSchema = New System.Windows.Forms.GroupBox()
            Me.txtTableName = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.grpTableSchema = New System.Windows.Forms.GroupBox()
            Me.rbTableDefault = New System.Windows.Forms.RadioButton()
            Me.rbTableQuery = New System.Windows.Forms.RadioButton()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.splitRoutine = New System.Windows.Forms.SplitContainer()
            Me.Panel5 = New System.Windows.Forms.Panel()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lnkPreviewRoutineSchema = New System.Windows.Forms.LinkLabel()
            Me.TabPage4 = New System.Windows.Forms.TabPage()
            Me.tcMain.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.SuspendLayout()
            CType(Me.splitQuery, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitQuery.Panel1.SuspendLayout()
            Me.splitQuery.Panel2.SuspendLayout()
            Me.splitQuery.SuspendLayout()
            Me.Panel3.SuspendLayout()
            CType(Me.splitTableColumn, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitTableColumn.Panel1.SuspendLayout()
            Me.splitTableColumn.Panel2.SuspendLayout()
            Me.splitTableColumn.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.grpApproach.SuspendLayout()
            Me.grpColumnSchema.SuspendLayout()
            Me.grpTableSchema.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            CType(Me.splitRoutine, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitRoutine.Panel1.SuspendLayout()
            Me.splitRoutine.SuspendLayout()
            Me.Panel5.SuspendLayout()
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
            Me.tcMain.Location = New System.Drawing.Point(0, 0)
            Me.tcMain.Name = "tcMain"
            Me.tcMain.SelectedIndex = 0
            Me.tcMain.Size = New System.Drawing.Size(777, 579)
            Me.tcMain.TabIndex = 2
            '
            'TabPage1
            '
            Me.TabPage1.BackColor = System.Drawing.Color.Transparent
            Me.TabPage1.Controls.Add(Me.txtPort)
            Me.TabPage1.Controls.Add(Me.chkPort)
            Me.TabPage1.Controls.Add(Me.Panel2)
            Me.TabPage1.Controls.Add(Me.lblTitle)
            Me.TabPage1.Controls.Add(Me.btnTest)
            Me.TabPage1.Controls.Add(Me.lblServer)
            Me.TabPage1.Controls.Add(Me.txtConnectionString)
            Me.TabPage1.Controls.Add(Me.txtServer)
            Me.TabPage1.Controls.Add(Me.lblConnectionString)
            Me.TabPage1.Controls.Add(Me.lblDatabase)
            Me.TabPage1.Controls.Add(Me.txtPassword)
            Me.TabPage1.Controls.Add(Me.txtDatabase)
            Me.TabPage1.Controls.Add(Me.lblPassword)
            Me.TabPage1.Controls.Add(Me.lblUsername)
            Me.TabPage1.Controls.Add(Me.txtUsername)
            Me.TabPage1.ImageIndex = 0
            Me.TabPage1.Location = New System.Drawing.Point(4, 4)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage1.Size = New System.Drawing.Size(769, 553)
            Me.TabPage1.TabIndex = 0
            Me.TabPage1.Text = "Connection"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'txtPort
            '
            Me.txtPort.Enabled = False
            Me.txtPort.Location = New System.Drawing.Point(171, 79)
            Me.txtPort.Name = "txtPort"
            Me.txtPort.Size = New System.Drawing.Size(75, 20)
            Me.txtPort.TabIndex = 3
            Me.txtPort.Text = "3306"
            '
            'chkPort
            '
            Me.chkPort.AutoSize = True
            Me.chkPort.Location = New System.Drawing.Point(79, 81)
            Me.chkPort.Name = "chkPort"
            Me.chkPort.Size = New System.Drawing.Size(86, 17)
            Me.chkPort.TabIndex = 2
            Me.chkPort.Text = "Specify Port:"
            Me.chkPort.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.Silver
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel2.Location = New System.Drawing.Point(3, 33)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(763, 1)
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
            Me.lblTitle.Size = New System.Drawing.Size(763, 30)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Boston Model - Direct Access"
            '
            'btnTest
            '
            Me.btnTest.Location = New System.Drawing.Point(20, 258)
            Me.btnTest.Name = "btnTest"
            Me.btnTest.Size = New System.Drawing.Size(103, 23)
            Me.btnTest.TabIndex = 12
            Me.btnTest.Text = "Test Connection"
            Me.btnTest.UseVisualStyleBackColor = True
            '
            'lblServer
            '
            Me.lblServer.AutoSize = True
            Me.lblServer.Location = New System.Drawing.Point(17, 58)
            Me.lblServer.Name = "lblServer"
            Me.lblServer.Size = New System.Drawing.Size(41, 13)
            Me.lblServer.TabIndex = 0
            Me.lblServer.Text = "Server:"
            '
            'txtConnectionString
            '
            Me.txtConnectionString.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtConnectionString.Location = New System.Drawing.Point(20, 223)
            Me.txtConnectionString.Name = "txtConnectionString"
            Me.txtConnectionString.Size = New System.Drawing.Size(735, 20)
            Me.txtConnectionString.TabIndex = 11
            '
            'txtServer
            '
            Me.txtServer.Location = New System.Drawing.Point(79, 55)
            Me.txtServer.Name = "txtServer"
            Me.txtServer.Size = New System.Drawing.Size(317, 20)
            Me.txtServer.TabIndex = 1
            '
            'lblConnectionString
            '
            Me.lblConnectionString.AutoSize = True
            Me.lblConnectionString.Location = New System.Drawing.Point(17, 207)
            Me.lblConnectionString.Name = "lblConnectionString"
            Me.lblConnectionString.Size = New System.Drawing.Size(94, 13)
            Me.lblConnectionString.TabIndex = 10
            Me.lblConnectionString.Text = "Connection String:"
            '
            'lblDatabase
            '
            Me.lblDatabase.AutoSize = True
            Me.lblDatabase.Location = New System.Drawing.Point(17, 108)
            Me.lblDatabase.Name = "lblDatabase"
            Me.lblDatabase.Size = New System.Drawing.Size(56, 13)
            Me.lblDatabase.TabIndex = 4
            Me.lblDatabase.Text = "Database:"
            '
            'txtPassword
            '
            Me.txtPassword.Location = New System.Drawing.Point(79, 157)
            Me.txtPassword.Name = "txtPassword"
            Me.txtPassword.Size = New System.Drawing.Size(317, 20)
            Me.txtPassword.TabIndex = 9
            '
            'txtDatabase
            '
            Me.txtDatabase.Location = New System.Drawing.Point(79, 105)
            Me.txtDatabase.Name = "txtDatabase"
            Me.txtDatabase.Size = New System.Drawing.Size(317, 20)
            Me.txtDatabase.TabIndex = 5
            '
            'lblPassword
            '
            Me.lblPassword.AutoSize = True
            Me.lblPassword.Location = New System.Drawing.Point(17, 160)
            Me.lblPassword.Name = "lblPassword"
            Me.lblPassword.Size = New System.Drawing.Size(56, 13)
            Me.lblPassword.TabIndex = 8
            Me.lblPassword.Text = "Password:"
            '
            'lblUsername
            '
            Me.lblUsername.AutoSize = True
            Me.lblUsername.Location = New System.Drawing.Point(17, 134)
            Me.lblUsername.Name = "lblUsername"
            Me.lblUsername.Size = New System.Drawing.Size(58, 13)
            Me.lblUsername.TabIndex = 6
            Me.lblUsername.Text = "Username:"
            '
            'txtUsername
            '
            Me.txtUsername.Location = New System.Drawing.Point(79, 131)
            Me.txtUsername.Name = "txtUsername"
            Me.txtUsername.Size = New System.Drawing.Size(317, 20)
            Me.txtUsername.TabIndex = 7
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.splitMain)
            Me.TabPage2.Controls.Add(Me.Panel1)
            Me.TabPage2.ImageIndex = 1
            Me.TabPage2.Location = New System.Drawing.Point(4, 4)
            Me.TabPage2.Margin = New System.Windows.Forms.Padding(0)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Size = New System.Drawing.Size(769, 553)
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
            Me.splitMain.Panel2Collapsed = True
            Me.splitMain.Size = New System.Drawing.Size(570, 553)
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
            Me.splitQuery.Panel1.Controls.Add(Me.Panel3)
            '
            'splitQuery.Panel2
            '
            Me.splitQuery.Panel2.Controls.Add(Me.splitTableColumn)
            Me.splitQuery.Size = New System.Drawing.Size(570, 553)
            Me.splitQuery.SplitterDistance = 272
            Me.splitQuery.TabIndex = 1
            '
            'Panel3
            '
            Me.Panel3.BackColor = System.Drawing.Color.White
            Me.Panel3.Controls.Add(Me.lblSchemaQuery)
            Me.Panel3.Controls.Add(Me.lnkPreviewSchema)
            Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel3.Location = New System.Drawing.Point(0, 0)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(570, 24)
            Me.Panel3.TabIndex = 2
            '
            'lblSchemaQuery
            '
            Me.lblSchemaQuery.BackColor = System.Drawing.Color.Transparent
            Me.lblSchemaQuery.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblSchemaQuery.Name = "lblSchemaQuery"
            Me.lblSchemaQuery.Size = New System.Drawing.Size(506, 24)
            Me.lblSchemaQuery.TabIndex = 2
            Me.lblSchemaQuery.Text = "Schema Query"
            Me.lblSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lnkPreviewSchema
            '
            Me.lnkPreviewSchema.BackColor = System.Drawing.Color.Transparent
            Me.lnkPreviewSchema.Dock = System.Windows.Forms.DockStyle.Right
            Me.lnkPreviewSchema.Image = CType(resources.GetObject("lnkPreviewSchema.Image"), System.Drawing.Image)
            Me.lnkPreviewSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lnkPreviewSchema.Location = New System.Drawing.Point(506, 0)
            Me.lnkPreviewSchema.Name = "lnkPreviewSchema"
            Me.lnkPreviewSchema.Size = New System.Drawing.Size(64, 24)
            Me.lnkPreviewSchema.TabIndex = 4
            Me.lnkPreviewSchema.TabStop = True
            Me.lnkPreviewSchema.Text = "Preview"
            Me.lnkPreviewSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
            Me.splitTableColumn.Panel1.Controls.Add(Me.lblTableSchemaQuery)
            '
            'splitTableColumn.Panel2
            '
            Me.splitTableColumn.Panel2.Controls.Add(Me.lblColumnSchemaQuery)
            Me.splitTableColumn.Size = New System.Drawing.Size(570, 277)
            Me.splitTableColumn.SplitterDistance = 135
            Me.splitTableColumn.TabIndex = 1
            '
            'lblTableSchemaQuery
            '
            Me.lblTableSchemaQuery.BackColor = System.Drawing.Color.White
            Me.lblTableSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblTableSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTableSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblTableSchemaQuery.Name = "lblTableSchemaQuery"
            Me.lblTableSchemaQuery.Size = New System.Drawing.Size(570, 24)
            Me.lblTableSchemaQuery.TabIndex = 0
            Me.lblTableSchemaQuery.Text = "Table Schema Query"
            Me.lblTableSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblColumnSchemaQuery
            '
            Me.lblColumnSchemaQuery.BackColor = System.Drawing.Color.White
            Me.lblColumnSchemaQuery.Dock = System.Windows.Forms.DockStyle.Top
            Me.lblColumnSchemaQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblColumnSchemaQuery.Location = New System.Drawing.Point(0, 0)
            Me.lblColumnSchemaQuery.Name = "lblColumnSchemaQuery"
            Me.lblColumnSchemaQuery.Size = New System.Drawing.Size(570, 24)
            Me.lblColumnSchemaQuery.TabIndex = 0
            Me.lblColumnSchemaQuery.Text = "Column Schema Query"
            Me.lblColumnSchemaQuery.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.grpApproach)
            Me.Panel1.Controls.Add(Me.grpColumnSchema)
            Me.Panel1.Controls.Add(Me.grpTableSchema)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
            Me.Panel1.Location = New System.Drawing.Point(0, 0)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(199, 553)
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
            Me.TabPage3.Size = New System.Drawing.Size(769, 553)
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
            Me.splitRoutine.Panel1.Controls.Add(Me.Panel5)
            Me.splitRoutine.Panel2Collapsed = True
            Me.splitRoutine.Size = New System.Drawing.Size(769, 553)
            Me.splitRoutine.SplitterDistance = 315
            Me.splitRoutine.TabIndex = 2
            '
            'Panel5
            '
            Me.Panel5.BackColor = System.Drawing.Color.White
            Me.Panel5.Controls.Add(Me.Label2)
            Me.Panel5.Controls.Add(Me.lnkPreviewRoutineSchema)
            Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel5.Location = New System.Drawing.Point(0, 0)
            Me.Panel5.Name = "Panel5"
            Me.Panel5.Size = New System.Drawing.Size(769, 24)
            Me.Panel5.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.Transparent
            Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label2.Location = New System.Drawing.Point(0, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(705, 24)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Routine/Parameter Schema Query"
            Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lnkPreviewRoutineSchema
            '
            Me.lnkPreviewRoutineSchema.BackColor = System.Drawing.Color.Transparent
            Me.lnkPreviewRoutineSchema.Dock = System.Windows.Forms.DockStyle.Right
            Me.lnkPreviewRoutineSchema.Image = CType(resources.GetObject("lnkPreviewRoutineSchema.Image"), System.Drawing.Image)
            Me.lnkPreviewRoutineSchema.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lnkPreviewRoutineSchema.Location = New System.Drawing.Point(705, 0)
            Me.lnkPreviewRoutineSchema.Name = "lnkPreviewRoutineSchema"
            Me.lnkPreviewRoutineSchema.Size = New System.Drawing.Size(64, 24)
            Me.lnkPreviewRoutineSchema.TabIndex = 1
            Me.lnkPreviewRoutineSchema.TabStop = True
            Me.lnkPreviewRoutineSchema.Text = "Preview"
            Me.lnkPreviewRoutineSchema.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'TabPage4
            '
            Me.TabPage4.ImageIndex = 3
            Me.TabPage4.Location = New System.Drawing.Point(4, 4)
            Me.TabPage4.Name = "TabPage4"
            Me.TabPage4.Size = New System.Drawing.Size(769, 553)
            Me.TabPage4.TabIndex = 3
            Me.TabPage4.Text = "Transformations"
            Me.TabPage4.UseVisualStyleBackColor = True
            '
            'Manage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.tcMain)
            Me.Name = "Manage"
            Me.Size = New System.Drawing.Size(777, 579)
            Me.tcMain.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TabPage1.PerformLayout()
            Me.TabPage2.ResumeLayout(False)
            Me.splitMain.Panel1.ResumeLayout(False)
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            Me.splitQuery.Panel1.ResumeLayout(False)
            Me.splitQuery.Panel2.ResumeLayout(False)
            CType(Me.splitQuery, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitQuery.ResumeLayout(False)
            Me.Panel3.ResumeLayout(False)
            Me.splitTableColumn.Panel1.ResumeLayout(False)
            Me.splitTableColumn.Panel2.ResumeLayout(False)
            CType(Me.splitTableColumn, System.ComponentModel.ISupportInitialize).EndInit()
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
            CType(Me.splitRoutine, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitRoutine.ResumeLayout(False)
            Me.Panel5.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents tcMain As TabControl
        Friend WithEvents TabPage1 As TabPage
        Friend WithEvents txtPort As TextBox
        Friend WithEvents chkPort As CheckBox
        Friend WithEvents Panel2 As Panel
        Friend WithEvents lblTitle As Label
        Friend WithEvents btnTest As Button
        Friend WithEvents lblServer As Label
        Friend WithEvents txtConnectionString As TextBox
        Friend WithEvents txtServer As TextBox
        Friend WithEvents lblConnectionString As Label
        Friend WithEvents lblDatabase As Label
        Friend WithEvents txtPassword As TextBox
        Friend WithEvents txtDatabase As TextBox
        Friend WithEvents lblPassword As Label
        Friend WithEvents lblUsername As Label
        Friend WithEvents txtUsername As TextBox
        Friend WithEvents TabPage2 As TabPage
        Friend WithEvents splitMain As SplitContainer
        Friend WithEvents splitQuery As SplitContainer
        Friend WithEvents Panel3 As Panel
        Friend WithEvents lblSchemaQuery As Label
        Friend WithEvents lnkPreviewSchema As LinkLabel
        Friend WithEvents splitTableColumn As SplitContainer
        Friend WithEvents lblTableSchemaQuery As Label
        Friend WithEvents lblColumnSchemaQuery As Label
        Friend WithEvents Panel1 As Panel
        Friend WithEvents grpApproach As GroupBox
        Friend WithEvents rbApproachSingle As RadioButton
        Friend WithEvents rbApproachTableColumn As RadioButton
        Friend WithEvents grpColumnSchema As GroupBox
        Friend WithEvents txtTableName As TextBox
        Friend WithEvents Label3 As Label
        Friend WithEvents grpTableSchema As GroupBox
        Friend WithEvents rbTableDefault As RadioButton
        Friend WithEvents rbTableQuery As RadioButton
        Friend WithEvents TabPage3 As TabPage
        Friend WithEvents splitRoutine As SplitContainer
        Friend WithEvents Panel5 As Panel
        Friend WithEvents Label2 As Label
        Friend WithEvents lnkPreviewRoutineSchema As LinkLabel
        Friend WithEvents TabPage4 As TabPage

#End Region

    End Class
End Namespace

