Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class ManageSource
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ManageSource))
            Me.pnlProviders = New System.Windows.Forms.Panel
            Me.picExcel = New System.Windows.Forms.PictureBox
            Me.rbExcel = New System.Windows.Forms.RadioButton
            Me.picOracle = New System.Windows.Forms.PictureBox
            Me.rbOracle = New System.Windows.Forms.RadioButton
            Me.picODBC = New System.Windows.Forms.PictureBox
            Me.rbODBC = New System.Windows.Forms.RadioButton
            Me.picOLEDB = New System.Windows.Forms.PictureBox
            Me.rbOLEDB = New System.Windows.Forms.RadioButton
            Me.picAccess = New System.Windows.Forms.PictureBox
            Me.rbAccess = New System.Windows.Forms.RadioButton
            Me.Label1 = New System.Windows.Forms.Label
            Me.picSQLServer = New System.Windows.Forms.PictureBox
            Me.rbSQLServer = New System.Windows.Forms.RadioButton
            Me.pnlManagers = New System.Windows.Forms.Panel
            Me.ctlSQLServer = New Boston.UI.ManageDBSQLServer
            Me.ctlExcel = New Boston.UI.ManageDBExcel
            Me.ctlAccess = New Boston.UI.ManageDBAccess
            Me.ctlOracle = New Boston.UI.ManageDBOracle
            Me.ctlOLEDB = New Boston.UI.ManageDBOLEDB
            Me.ctlODBC = New Boston.UI.ManageDBODBC
            Me.pnlLeft = New System.Windows.Forms.Panel
            Me.pnlMessage = New System.Windows.Forms.Panel
            Me.lnkMessage = New System.Windows.Forms.LinkLabel
            Me.picMessage = New System.Windows.Forms.PictureBox
            Me.pnlProviders.SuspendLayout()
            CType(Me.picExcel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picOracle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picODBC, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picOLEDB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picSQLServer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlManagers.SuspendLayout()
            Me.pnlLeft.SuspendLayout()
            Me.pnlMessage.SuspendLayout()
            CType(Me.picMessage, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlProviders
            '
            Me.pnlProviders.AutoScroll = True
            Me.pnlProviders.BackColor = System.Drawing.Color.Transparent
            Me.pnlProviders.Controls.Add(Me.picExcel)
            Me.pnlProviders.Controls.Add(Me.rbExcel)
            Me.pnlProviders.Controls.Add(Me.picOracle)
            Me.pnlProviders.Controls.Add(Me.rbOracle)
            Me.pnlProviders.Controls.Add(Me.picODBC)
            Me.pnlProviders.Controls.Add(Me.rbODBC)
            Me.pnlProviders.Controls.Add(Me.picOLEDB)
            Me.pnlProviders.Controls.Add(Me.rbOLEDB)
            Me.pnlProviders.Controls.Add(Me.picAccess)
            Me.pnlProviders.Controls.Add(Me.rbAccess)
            Me.pnlProviders.Controls.Add(Me.Label1)
            Me.pnlProviders.Controls.Add(Me.picSQLServer)
            Me.pnlProviders.Controls.Add(Me.rbSQLServer)
            Me.pnlProviders.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlProviders.Location = New System.Drawing.Point(0, 0)
            Me.pnlProviders.Name = "pnlProviders"
            Me.pnlProviders.Size = New System.Drawing.Size(200, 536)
            Me.pnlProviders.TabIndex = 0
            '
            'picExcel
            '
            Me.picExcel.Image = CType(resources.GetObject("picExcel.Image"), System.Drawing.Image)
            Me.picExcel.Location = New System.Drawing.Point(13, 147)
            Me.picExcel.Name = "picExcel"
            Me.picExcel.Size = New System.Drawing.Size(32, 32)
            Me.picExcel.TabIndex = 38
            Me.picExcel.TabStop = False
            '
            'rbExcel
            '
            Me.rbExcel.Location = New System.Drawing.Point(51, 154)
            Me.rbExcel.Name = "rbExcel"
            Me.rbExcel.Size = New System.Drawing.Size(108, 17)
            Me.rbExcel.TabIndex = 3
            Me.rbExcel.Text = "Microsoft Excel"
            Me.rbExcel.UseVisualStyleBackColor = True
            '
            'picOracle
            '
            Me.picOracle.Image = CType(resources.GetObject("picOracle.Image"), System.Drawing.Image)
            Me.picOracle.Location = New System.Drawing.Point(13, 71)
            Me.picOracle.Name = "picOracle"
            Me.picOracle.Size = New System.Drawing.Size(32, 32)
            Me.picOracle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
            Me.picOracle.TabIndex = 34
            Me.picOracle.TabStop = False
            '
            'rbOracle
            '
            Me.rbOracle.Location = New System.Drawing.Point(51, 78)
            Me.rbOracle.Name = "rbOracle"
            Me.rbOracle.Size = New System.Drawing.Size(108, 17)
            Me.rbOracle.TabIndex = 1
            Me.rbOracle.Text = "Oracle"
            Me.rbOracle.UseVisualStyleBackColor = True
            '
            'picODBC
            '
            Me.picODBC.Image = CType(resources.GetObject("picODBC.Image"), System.Drawing.Image)
            Me.picODBC.Location = New System.Drawing.Point(13, 223)
            Me.picODBC.Name = "picODBC"
            Me.picODBC.Size = New System.Drawing.Size(32, 32)
            Me.picODBC.TabIndex = 32
            Me.picODBC.TabStop = False
            '
            'rbODBC
            '
            Me.rbODBC.Location = New System.Drawing.Point(51, 230)
            Me.rbODBC.Name = "rbODBC"
            Me.rbODBC.Size = New System.Drawing.Size(108, 17)
            Me.rbODBC.TabIndex = 5
            Me.rbODBC.Text = "Generic ODBC"
            Me.rbODBC.UseVisualStyleBackColor = True
            '
            'picOLEDB
            '
            Me.picOLEDB.Image = CType(resources.GetObject("picOLEDB.Image"), System.Drawing.Image)
            Me.picOLEDB.Location = New System.Drawing.Point(13, 185)
            Me.picOLEDB.Name = "picOLEDB"
            Me.picOLEDB.Size = New System.Drawing.Size(32, 32)
            Me.picOLEDB.TabIndex = 28
            Me.picOLEDB.TabStop = False
            '
            'rbOLEDB
            '
            Me.rbOLEDB.Location = New System.Drawing.Point(51, 192)
            Me.rbOLEDB.Name = "rbOLEDB"
            Me.rbOLEDB.Size = New System.Drawing.Size(108, 17)
            Me.rbOLEDB.TabIndex = 4
            Me.rbOLEDB.Text = "Generic OLEDB"
            Me.rbOLEDB.UseVisualStyleBackColor = True
            '
            'picAccess
            '
            Me.picAccess.Image = CType(resources.GetObject("picAccess.Image"), System.Drawing.Image)
            Me.picAccess.Location = New System.Drawing.Point(13, 109)
            Me.picAccess.Name = "picAccess"
            Me.picAccess.Size = New System.Drawing.Size(32, 32)
            Me.picAccess.TabIndex = 26
            Me.picAccess.TabStop = False
            '
            'rbAccess
            '
            Me.rbAccess.Location = New System.Drawing.Point(51, 116)
            Me.rbAccess.Name = "rbAccess"
            Me.rbAccess.Size = New System.Drawing.Size(108, 17)
            Me.rbAccess.TabIndex = 2
            Me.rbAccess.Text = "Microsoft Access"
            Me.rbAccess.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(0, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Padding = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label1.Size = New System.Drawing.Size(200, 30)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Provider"
            '
            'picSQLServer
            '
            Me.picSQLServer.Image = CType(resources.GetObject("picSQLServer.Image"), System.Drawing.Image)
            Me.picSQLServer.Location = New System.Drawing.Point(13, 33)
            Me.picSQLServer.Name = "picSQLServer"
            Me.picSQLServer.Size = New System.Drawing.Size(32, 32)
            Me.picSQLServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
            Me.picSQLServer.TabIndex = 20
            Me.picSQLServer.TabStop = False
            '
            'rbSQLServer
            '
            Me.rbSQLServer.Checked = True
            Me.rbSQLServer.Location = New System.Drawing.Point(51, 40)
            Me.rbSQLServer.Name = "rbSQLServer"
            Me.rbSQLServer.Size = New System.Drawing.Size(108, 17)
            Me.rbSQLServer.TabIndex = 0
            Me.rbSQLServer.TabStop = True
            Me.rbSQLServer.Text = "SQL Server"
            Me.rbSQLServer.UseVisualStyleBackColor = True
            '
            'pnlManagers
            '
            Me.pnlManagers.Controls.Add(Me.ctlSQLServer)
            Me.pnlManagers.Controls.Add(Me.ctlExcel)
            Me.pnlManagers.Controls.Add(Me.ctlAccess)
            Me.pnlManagers.Controls.Add(Me.ctlOracle)
            Me.pnlManagers.Controls.Add(Me.ctlOLEDB)
            Me.pnlManagers.Controls.Add(Me.ctlODBC)
            Me.pnlManagers.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlManagers.Location = New System.Drawing.Point(200, 0)
            Me.pnlManagers.Name = "pnlManagers"
            Me.pnlManagers.Size = New System.Drawing.Size(633, 558)
            Me.pnlManagers.TabIndex = 1
            '
            'ctlSQLServer
            '
            Me.ctlSQLServer.ColumnSchemaGeneric = False
            Me.ctlSQLServer.ColumnSchemaQuery = ""
            Me.ctlSQLServer.ConnectionString = "Data Source=;Initial Catalog=;Integrated Security=True;"
            Me.ctlSQLServer.Location = New System.Drawing.Point(19, 3)
            Me.ctlSQLServer.Name = "ctlSQLServer"
            Me.ctlSQLServer.RoutineSchemaQuery = ""
            Me.ctlSQLServer.SchemaQuery = resources.GetString("ctlSQLServer.SchemaQuery")
            Me.ctlSQLServer.SingleResultApproach = True
            Me.ctlSQLServer.Size = New System.Drawing.Size(290, 100)
            Me.ctlSQLServer.TabIndex = 0
            Me.ctlSQLServer.TableName = ""
            Me.ctlSQLServer.TableSchemaGeneric = True
            Me.ctlSQLServer.TableSchemaQuery = ""
            Me.ctlSQLServer.Transformations = ""
            '
            'ctlExcel
            '
            Me.ctlExcel.ColumnSchemaGeneric = False
            Me.ctlExcel.ColumnSchemaQuery = Nothing
            Me.ctlExcel.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=;Extended Properties=""Excel 12.0 Xm" & _
                "l;HDR=Yes"";"
            Me.ctlExcel.Excel2K = False
            Me.ctlExcel.Location = New System.Drawing.Point(315, 114)
            Me.ctlExcel.Name = "ctlExcel"
            Me.ctlExcel.RoutineSchemaQuery = Nothing
            Me.ctlExcel.SchemaQuery = Nothing
            Me.ctlExcel.SingleResultApproach = False
            Me.ctlExcel.Size = New System.Drawing.Size(290, 100)
            Me.ctlExcel.TabIndex = 3
            Me.ctlExcel.TableName = Nothing
            Me.ctlExcel.TableSchemaGeneric = False
            Me.ctlExcel.TableSchemaQuery = Nothing
            Me.ctlExcel.Transformations = ""
            Me.ctlExcel.UseOleDBSchemaGet = False
            '
            'ctlAccess
            '
            Me.ctlAccess.Access2K = False
            Me.ctlAccess.ColumnSchemaGeneric = False
            Me.ctlAccess.ColumnSchemaQuery = Nothing
            Me.ctlAccess.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=;Persist Security Info=True;"
            Me.ctlAccess.Location = New System.Drawing.Point(19, 114)
            Me.ctlAccess.Name = "ctlAccess"
            Me.ctlAccess.RoutineSchemaQuery = Nothing
            Me.ctlAccess.SchemaQuery = Nothing
            Me.ctlAccess.SingleResultApproach = False
            Me.ctlAccess.Size = New System.Drawing.Size(290, 100)
            Me.ctlAccess.TabIndex = 2
            Me.ctlAccess.TableName = Nothing
            Me.ctlAccess.TableSchemaGeneric = False
            Me.ctlAccess.TableSchemaQuery = Nothing
            Me.ctlAccess.Transformations = ""
            Me.ctlAccess.UseOleDBSchemaGet = True
            '
            'ctlOracle
            '
            Me.ctlOracle.ColumnSchemaGeneric = False
            Me.ctlOracle.ColumnSchemaQuery = ""
            Me.ctlOracle.ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=)(PORT=1521))" & _
                ")(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=)));User Id=;Password=;"
            Me.ctlOracle.Location = New System.Drawing.Point(315, 3)
            Me.ctlOracle.Name = "ctlOracle"
            Me.ctlOracle.RoutineSchemaQuery = ""
            Me.ctlOracle.SchemaQuery = resources.GetString("ctlOracle.SchemaQuery")
            Me.ctlOracle.SingleResultApproach = True
            Me.ctlOracle.Size = New System.Drawing.Size(290, 100)
            Me.ctlOracle.TabIndex = 1
            Me.ctlOracle.TableName = ""
            Me.ctlOracle.TableSchemaGeneric = True
            Me.ctlOracle.TableSchemaQuery = ""
            Me.ctlOracle.Transformations = ""
            '
            'ctlOLEDB
            '
            Me.ctlOLEDB.ColumnSchemaGeneric = True
            Me.ctlOLEDB.ColumnSchemaQuery = ""
            Me.ctlOLEDB.ConnectionString = ""
            Me.ctlOLEDB.Location = New System.Drawing.Point(19, 221)
            Me.ctlOLEDB.Name = "ctlOLEDB"
            Me.ctlOLEDB.RoutineSchemaQuery = Nothing
            Me.ctlOLEDB.SchemaQuery = ""
            Me.ctlOLEDB.SingleResultApproach = True
            Me.ctlOLEDB.Size = New System.Drawing.Size(290, 100)
            Me.ctlOLEDB.TabIndex = 4
            Me.ctlOLEDB.TableName = ""
            Me.ctlOLEDB.TableSchemaGeneric = True
            Me.ctlOLEDB.TableSchemaQuery = ""
            Me.ctlOLEDB.Transformations = ""
            '
            'ctlODBC
            '
            Me.ctlODBC.ColumnSchemaGeneric = False
            Me.ctlODBC.ColumnSchemaQuery = ""
            Me.ctlODBC.ConnectionString = ""
            Me.ctlODBC.Location = New System.Drawing.Point(315, 221)
            Me.ctlODBC.Name = "ctlODBC"
            Me.ctlODBC.RoutineSchemaQuery = Nothing
            Me.ctlODBC.SchemaQuery = ""
            Me.ctlODBC.SingleResultApproach = True
            Me.ctlODBC.Size = New System.Drawing.Size(290, 100)
            Me.ctlODBC.TabIndex = 5
            Me.ctlODBC.TableName = ""
            Me.ctlODBC.TableSchemaGeneric = True
            Me.ctlODBC.TableSchemaQuery = ""
            Me.ctlODBC.Transformations = ""
            '
            'pnlLeft
            '
            Me.pnlLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            Me.pnlLeft.Controls.Add(Me.pnlProviders)
            Me.pnlLeft.Controls.Add(Me.pnlMessage)
            Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
            Me.pnlLeft.Name = "pnlLeft"
            Me.pnlLeft.Size = New System.Drawing.Size(200, 558)
            Me.pnlLeft.TabIndex = 0
            '
            'pnlMessage
            '
            Me.pnlMessage.BackColor = System.Drawing.Color.Transparent
            Me.pnlMessage.Controls.Add(Me.lnkMessage)
            Me.pnlMessage.Controls.Add(Me.picMessage)
            Me.pnlMessage.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlMessage.Location = New System.Drawing.Point(0, 536)
            Me.pnlMessage.Name = "pnlMessage"
            Me.pnlMessage.Size = New System.Drawing.Size(200, 22)
            Me.pnlMessage.TabIndex = 1
            Me.pnlMessage.Visible = False
            '
            'lnkMessage
            '
            Me.lnkMessage.BackColor = System.Drawing.Color.Transparent
            Me.lnkMessage.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lnkMessage.Location = New System.Drawing.Point(21, 0)
            Me.lnkMessage.Name = "lnkMessage"
            Me.lnkMessage.Size = New System.Drawing.Size(179, 22)
            Me.lnkMessage.TabIndex = 1
            Me.lnkMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'picMessage
            '
            Me.picMessage.BackColor = System.Drawing.Color.Transparent
            Me.picMessage.Dock = System.Windows.Forms.DockStyle.Left
            Me.picMessage.Image = CType(resources.GetObject("picMessage.Image"), System.Drawing.Image)
            Me.picMessage.Location = New System.Drawing.Point(0, 0)
            Me.picMessage.Name = "picMessage"
            Me.picMessage.Size = New System.Drawing.Size(21, 22)
            Me.picMessage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
            Me.picMessage.TabIndex = 0
            Me.picMessage.TabStop = False
            '
            'ManageSource
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.Controls.Add(Me.pnlManagers)
            Me.Controls.Add(Me.pnlLeft)
            Me.Name = "ManageSource"
            Me.Size = New System.Drawing.Size(833, 558)
            Me.pnlProviders.ResumeLayout(False)
            CType(Me.picExcel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picOracle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picODBC, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picOLEDB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picAccess, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picSQLServer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlManagers.ResumeLayout(False)
            Me.pnlLeft.ResumeLayout(False)
            Me.pnlMessage.ResumeLayout(False)
            CType(Me.picMessage, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlProviders As System.Windows.Forms.Panel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents picSQLServer As System.Windows.Forms.PictureBox
        Friend WithEvents rbSQLServer As System.Windows.Forms.RadioButton
        Friend WithEvents ctlSQLServer As Boston.UI.ManageDBSQLServer
        Friend WithEvents picAccess As System.Windows.Forms.PictureBox
        Friend WithEvents rbAccess As System.Windows.Forms.RadioButton
        Friend WithEvents ctlAccess As Boston.UI.ManageDBAccess
        Friend WithEvents picOLEDB As System.Windows.Forms.PictureBox
        Friend WithEvents rbOLEDB As System.Windows.Forms.RadioButton
        Friend WithEvents ctlOLEDB As Boston.UI.ManageDBOLEDB
        Friend WithEvents picODBC As System.Windows.Forms.PictureBox
        Friend WithEvents rbODBC As System.Windows.Forms.RadioButton
        Friend WithEvents ctlODBC As Boston.UI.ManageDBODBC
        Friend WithEvents ctlOracle As Boston.UI.ManageDBOracle
        Friend WithEvents picOracle As System.Windows.Forms.PictureBox
        Friend WithEvents rbOracle As System.Windows.Forms.RadioButton
        Friend WithEvents picExcel As System.Windows.Forms.PictureBox
        Friend WithEvents rbExcel As System.Windows.Forms.RadioButton
        Friend WithEvents ctlExcel As Boston.UI.ManageDBExcel
        Friend WithEvents pnlManagers As System.Windows.Forms.Panel
        Friend WithEvents pnlLeft As System.Windows.Forms.Panel
        Friend WithEvents lnkMessage As System.Windows.Forms.LinkLabel
        Friend WithEvents pnlMessage As System.Windows.Forms.Panel
        Friend WithEvents picMessage As System.Windows.Forms.PictureBox

    End Class

End Namespace