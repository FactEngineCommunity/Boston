
Partial Class frmCSVLoader
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

#Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.btnGetFile = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.numMax = New System.Windows.Forms.NumericUpDown()
        Me.numDataRow = New System.Windows.Forms.NumericUpDown()
        Me.numTitleRow = New System.Windows.Forms.NumericUpDown()
        Me.txtNameOnly = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.label17 = New System.Windows.Forms.Label()
        Me.txtDelimiter = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnGetData = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtFileNameOut = New System.Windows.Forms.TextBox()
        Me.txtDelimiterOUT = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkIncludeTitle = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.txtDestination = New System.Windows.Forms.TextBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPageExportCSVData = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabPageImportCSV = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.txtLength = New System.Windows.Forms.TextBox()
        Me.txtLastWriteTime = New System.Windows.Forms.TextBox()
        Me.txtLastAccessTime = New System.Windows.Forms.TextBox()
        Me.btnGetDetails = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.txtUNCPath = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFullName = New System.Windows.Forms.TextBox()
        Me.txtExists = New System.Windows.Forms.TextBox()
        Me.txtDirectoryPath = New System.Windows.Forms.TextBox()
        Me.txtCreationTime = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtExt = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptTable = New System.Windows.Forms.Label()
        Me.LabelTableName = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        CType(Me.numMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numDataRow, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTitleRow, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TabPageExportCSVData.SuspendLayout()
        Me.TabPageImportCSV.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.txtFileName)
        Me.Panel2.Controls.Add(Me.btnGetFile)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 38)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(763, 34)
        Me.Panel2.TabIndex = 7
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(151, 7)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.ReadOnly = True
        Me.txtFileName.Size = New System.Drawing.Size(566, 20)
        Me.txtFileName.TabIndex = 1
        '
        'btnGetFile
        '
        Me.btnGetFile.Location = New System.Drawing.Point(7, 5)
        Me.btnGetFile.Name = "btnGetFile"
        Me.btnGetFile.Size = New System.Drawing.Size(75, 23)
        Me.btnGetFile.TabIndex = 0
        Me.btnGetFile.Text = "&Select File"
        Me.btnGetFile.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(86, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File Name:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(5, 154)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(110, 20)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Exists:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'numMax
        '
        Me.numMax.Location = New System.Drawing.Point(507, 34)
        Me.numMax.Name = "numMax"
        Me.numMax.Size = New System.Drawing.Size(39, 20)
        Me.numMax.TabIndex = 18
        '
        'numDataRow
        '
        Me.numDataRow.Location = New System.Drawing.Point(100, 33)
        Me.numDataRow.Name = "numDataRow"
        Me.numDataRow.Size = New System.Drawing.Size(39, 20)
        Me.numDataRow.TabIndex = 17
        '
        'numTitleRow
        '
        Me.numTitleRow.Location = New System.Drawing.Point(100, 9)
        Me.numTitleRow.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.numTitleRow.Name = "numTitleRow"
        Me.numTitleRow.Size = New System.Drawing.Size(39, 20)
        Me.numTitleRow.TabIndex = 16
        Me.numTitleRow.Value = New Decimal(New Integer() {1, 0, 0, -2147483648})
        '
        'txtNameOnly
        '
        Me.txtNameOnly.Location = New System.Drawing.Point(120, 309)
        Me.txtNameOnly.Name = "txtNameOnly"
        Me.txtNameOnly.ReadOnly = True
        Me.txtNameOnly.Size = New System.Drawing.Size(593, 20)
        Me.txtNameOnly.TabIndex = 29
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(5, 232)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 20)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "Last Write Time:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.label17)
        Me.Panel3.Controls.Add(Me.numMax)
        Me.Panel3.Controls.Add(Me.numDataRow)
        Me.Panel3.Controls.Add(Me.numTitleRow)
        Me.Panel3.Controls.Add(Me.txtDelimiter)
        Me.Panel3.Controls.Add(Me.Label15)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.Label12)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.btnGetData)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(749, 87)
        Me.Panel3.TabIndex = 3
        '
        'label17
        '
        Me.label17.Location = New System.Drawing.Point(145, 9)
        Me.label17.Name = "label17"
        Me.label17.Size = New System.Drawing.Size(100, 20)
        Me.label17.TabIndex = 19
        Me.label17.Text = "-1 = No Title"
        Me.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDelimiter
        '
        Me.txtDelimiter.Location = New System.Drawing.Point(507, 8)
        Me.txtDelimiter.Name = "txtDelimiter"
        Me.txtDelimiter.Size = New System.Drawing.Size(25, 20)
        Me.txtDelimiter.TabIndex = 15
        Me.txtDelimiter.Text = ","
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(12, 33)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(82, 20)
        Me.Label15.TabIndex = 11
        Me.Label15.Text = "First Data Row:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(12, 9)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(82, 20)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "Title Row No:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(313, 34)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(181, 20)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "Maximum Rows to read (optional):"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(313, 8)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(181, 20)
        Me.Label11.TabIndex = 8
        Me.Label11.Text = "Field Delimiter:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnGetData
        '
        Me.btnGetData.Location = New System.Drawing.Point(5, 58)
        Me.btnGetData.Name = "btnGetData"
        Me.btnGetData.Size = New System.Drawing.Size(75, 23)
        Me.btnGetData.TabIndex = 6
        Me.btnGetData.Text = "Get Data"
        Me.btnGetData.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(5, 310)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(110, 20)
        Me.Label16.TabIndex = 30
        Me.Label16.Text = "Name Only:"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(5, 206)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(110, 20)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Last Access Time:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(5, 258)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(110, 20)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Length:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFileNameOut
        '
        Me.txtFileNameOut.Location = New System.Drawing.Point(108, 9)
        Me.txtFileNameOut.Name = "txtFileNameOut"
        Me.txtFileNameOut.Size = New System.Drawing.Size(234, 20)
        Me.txtFileNameOut.TabIndex = 16
        Me.txtFileNameOut.Text = "Data.csv"
        '
        'txtDelimiterOUT
        '
        Me.txtDelimiterOUT.Location = New System.Drawing.Point(106, 62)
        Me.txtDelimiterOUT.Name = "txtDelimiterOUT"
        Me.txtDelimiterOUT.Size = New System.Drawing.Size(25, 20)
        Me.txtDelimiterOUT.TabIndex = 15
        Me.txtDelimiterOUT.Text = ","
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(5, 180)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(110, 20)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Full Name:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkIncludeTitle
        '
        Me.chkIncludeTitle.Checked = True
        Me.chkIncludeTitle.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncludeTitle.Location = New System.Drawing.Point(107, 38)
        Me.chkIncludeTitle.Name = "chkIncludeTitle"
        Me.chkIncludeTitle.Size = New System.Drawing.Size(190, 24)
        Me.chkIncludeTitle.TabIndex = 7
        Me.chkIncludeTitle.Text = "Include Titles"
        Me.chkIncludeTitle.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(5, 128)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(110, 20)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Directory Path:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(135, 62)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(181, 20)
        Me.Label19.TabIndex = 8
        Me.Label19.Text = "Field Delimiter:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.txtDestination)
        Me.Panel4.Controls.Add(Me.txtFileNameOut)
        Me.Panel4.Controls.Add(Me.txtDelimiterOUT)
        Me.Panel4.Controls.Add(Me.Label19)
        Me.Panel4.Controls.Add(Me.chkIncludeTitle)
        Me.Panel4.Controls.Add(Me.btnExport)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(755, 150)
        Me.Panel4.TabIndex = 4
        '
        'txtDestination
        '
        Me.txtDestination.Location = New System.Drawing.Point(186, 90)
        Me.txtDestination.Name = "txtDestination"
        Me.txtDestination.ReadOnly = True
        Me.txtDestination.Size = New System.Drawing.Size(520, 20)
        Me.txtDestination.TabIndex = 18
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(105, 89)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "Export Data"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(5, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 20)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Creation Time:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TabPageExportCSVData
        '
        Me.TabPageExportCSVData.Controls.Add(Me.Panel4)
        Me.TabPageExportCSVData.Location = New System.Drawing.Point(4, 22)
        Me.TabPageExportCSVData.Name = "TabPageExportCSVData"
        Me.TabPageExportCSVData.Size = New System.Drawing.Size(755, 351)
        Me.TabPageExportCSVData.TabIndex = 2
        Me.TabPageExportCSVData.Text = "Export CSV Data"
        Me.TabPageExportCSVData.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(5, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 20)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Name:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TabPageImportCSV
        '
        Me.TabPageImportCSV.Controls.Add(Me.DataGridView1)
        Me.TabPageImportCSV.Controls.Add(Me.Panel3)
        Me.TabPageImportCSV.Location = New System.Drawing.Point(4, 22)
        Me.TabPageImportCSV.Name = "TabPageImportCSV"
        Me.TabPageImportCSV.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageImportCSV.Size = New System.Drawing.Size(755, 351)
        Me.TabPageImportCSV.TabIndex = 1
        Me.TabPageImportCSV.Text = "Get CSV Data"
        Me.TabPageImportCSV.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 90)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(749, 258)
        Me.DataGridView1.TabIndex = 2
        '
        'txtLength
        '
        Me.txtLength.Location = New System.Drawing.Point(120, 257)
        Me.txtLength.Name = "txtLength"
        Me.txtLength.ReadOnly = True
        Me.txtLength.Size = New System.Drawing.Size(593, 20)
        Me.txtLength.TabIndex = 16
        '
        'txtLastWriteTime
        '
        Me.txtLastWriteTime.Location = New System.Drawing.Point(120, 231)
        Me.txtLastWriteTime.Name = "txtLastWriteTime"
        Me.txtLastWriteTime.ReadOnly = True
        Me.txtLastWriteTime.Size = New System.Drawing.Size(593, 20)
        Me.txtLastWriteTime.TabIndex = 15
        '
        'txtLastAccessTime
        '
        Me.txtLastAccessTime.Location = New System.Drawing.Point(120, 205)
        Me.txtLastAccessTime.Name = "txtLastAccessTime"
        Me.txtLastAccessTime.ReadOnly = True
        Me.txtLastAccessTime.Size = New System.Drawing.Size(593, 20)
        Me.txtLastAccessTime.TabIndex = 14
        '
        'btnGetDetails
        '
        Me.btnGetDetails.Location = New System.Drawing.Point(6, 7)
        Me.btnGetDetails.Name = "btnGetDetails"
        Me.btnGetDetails.Size = New System.Drawing.Size(75, 23)
        Me.btnGetDetails.TabIndex = 6
        Me.btnGetDetails.Text = "Get Details"
        Me.btnGetDetails.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(5, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(110, 20)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Extension:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(681, 11)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPageImportCSV)
        Me.TabControl.Controls.Add(Me.TabPageExportCSVData)
        Me.TabControl.Controls.Add(Me.TabPage1)
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Location = New System.Drawing.Point(3, 78)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(763, 377)
        Me.TabControl.TabIndex = 8
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.txtUNCPath)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.txtNameOnly)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.txtLength)
        Me.TabPage1.Controls.Add(Me.txtLastWriteTime)
        Me.TabPage1.Controls.Add(Me.txtLastAccessTime)
        Me.TabPage1.Controls.Add(Me.txtFullName)
        Me.TabPage1.Controls.Add(Me.txtExists)
        Me.TabPage1.Controls.Add(Me.txtDirectoryPath)
        Me.TabPage1.Controls.Add(Me.txtCreationTime)
        Me.TabPage1.Controls.Add(Me.txtName)
        Me.TabPage1.Controls.Add(Me.txtExt)
        Me.TabPage1.Controls.Add(Me.btnGetDetails)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(755, 351)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "File Properties"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'txtUNCPath
        '
        Me.txtUNCPath.Location = New System.Drawing.Point(120, 283)
        Me.txtUNCPath.Name = "txtUNCPath"
        Me.txtUNCPath.ReadOnly = True
        Me.txtUNCPath.Size = New System.Drawing.Size(593, 20)
        Me.txtUNCPath.TabIndex = 32
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(4, 284)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 20)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "UNC Path:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFullName
        '
        Me.txtFullName.Location = New System.Drawing.Point(120, 179)
        Me.txtFullName.Name = "txtFullName"
        Me.txtFullName.ReadOnly = True
        Me.txtFullName.Size = New System.Drawing.Size(593, 20)
        Me.txtFullName.TabIndex = 13
        '
        'txtExists
        '
        Me.txtExists.Location = New System.Drawing.Point(120, 153)
        Me.txtExists.Name = "txtExists"
        Me.txtExists.ReadOnly = True
        Me.txtExists.Size = New System.Drawing.Size(593, 20)
        Me.txtExists.TabIndex = 12
        '
        'txtDirectoryPath
        '
        Me.txtDirectoryPath.Location = New System.Drawing.Point(120, 127)
        Me.txtDirectoryPath.Name = "txtDirectoryPath"
        Me.txtDirectoryPath.ReadOnly = True
        Me.txtDirectoryPath.Size = New System.Drawing.Size(593, 20)
        Me.txtDirectoryPath.TabIndex = 11
        '
        'txtCreationTime
        '
        Me.txtCreationTime.Location = New System.Drawing.Point(120, 101)
        Me.txtCreationTime.Name = "txtCreationTime"
        Me.txtCreationTime.ReadOnly = True
        Me.txtCreationTime.Size = New System.Drawing.Size(593, 20)
        Me.txtCreationTime.TabIndex = 10
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(120, 75)
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Size = New System.Drawing.Size(593, 20)
        Me.txtName.TabIndex = 9
        '
        'txtExt
        '
        Me.txtExt.Location = New System.Drawing.Point(120, 49)
        Me.txtExt.Name = "txtExt"
        Me.txtExt.ReadOnly = True
        Me.txtExt.Size = New System.Drawing.Size(593, 20)
        Me.txtExt.TabIndex = 7
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 461)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(763, 42)
        Me.Panel1.TabIndex = 6
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.LabelTableName)
        Me.Panel5.Controls.Add(Me.LabelPromptTable)
        Me.Panel5.Controls.Add(Me.LabelModelName)
        Me.Panel5.Controls.Add(Me.LabelPromptModel)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(3, 3)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(763, 29)
        Me.Panel5.TabIndex = 2
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel5, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(769, 506)
        Me.TableLayoutPanel1.TabIndex = 33
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(4, 6)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 0
        Me.LabelPromptModel.Text = "Model:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(42, 6)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(76, 13)
        Me.LabelModelName.TabIndex = 1
        Me.LabelModelName.Text = "<ModelName>"
        '
        'LabelPromptTable
        '
        Me.LabelPromptTable.AutoSize = True
        Me.LabelPromptTable.Location = New System.Drawing.Point(216, 6)
        Me.LabelPromptTable.Name = "LabelPromptTable"
        Me.LabelPromptTable.Size = New System.Drawing.Size(37, 13)
        Me.LabelPromptTable.TabIndex = 2
        Me.LabelPromptTable.Text = "Table:"
        '
        'LabelTableName
        '
        Me.LabelTableName.AutoSize = True
        Me.LabelTableName.Location = New System.Drawing.Point(259, 6)
        Me.LabelTableName.Name = "LabelTableName"
        Me.LabelTableName.Size = New System.Drawing.Size(74, 13)
        Me.LabelTableName.TabIndex = 3
        Me.LabelTableName.Text = "<TableName>"
        '
        'frmCSVLoader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(769, 506)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "frmCSVLoader"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CSV Handler"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.numMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numDataRow, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTitleRow, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.TabPageExportCSVData.ResumeLayout(False)
        Me.TabPageImportCSV.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents btnGetFile As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents numMax As System.Windows.Forms.NumericUpDown
    Friend WithEvents numDataRow As System.Windows.Forms.NumericUpDown
    Friend WithEvents numTitleRow As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtNameOnly As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents txtDelimiter As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnGetData As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtFileNameOut As System.Windows.Forms.TextBox
    Friend WithEvents txtDelimiterOUT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkIncludeTitle As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TabPageExportCSVData As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TabPageImportCSV As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtLength As System.Windows.Forms.TextBox
    Friend WithEvents txtLastWriteTime As System.Windows.Forms.TextBox
    Friend WithEvents txtLastAccessTime As System.Windows.Forms.TextBox
    Friend WithEvents btnGetDetails As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents txtFullName As System.Windows.Forms.TextBox
    Friend WithEvents txtExists As System.Windows.Forms.TextBox
    Friend WithEvents txtDirectoryPath As System.Windows.Forms.TextBox
    Friend WithEvents txtCreationTime As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtExt As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Private WithEvents label17 As System.Windows.Forms.Label
    Private WithEvents txtDestination As System.Windows.Forms.TextBox
    Friend WithEvents txtUNCPath As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents LabelTableName As Label
    Friend WithEvents LabelPromptTable As Label
    Friend WithEvents LabelModelName As Label
    Friend WithEvents LabelPromptModel As Label
End Class


