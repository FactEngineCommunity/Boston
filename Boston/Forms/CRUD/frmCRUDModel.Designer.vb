<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDModel
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.GroupBox_main = New System.Windows.Forms.GroupBox()
        Me.LabelCoreVersion = New System.Windows.Forms.Label()
        Me.LabelPromptCoreVersion = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelModel = New System.Windows.Forms.Label()
        Me.GroupBoxDatabase = New System.Windows.Forms.GroupBox()
        Me.ButtonFileSelect = New System.Windows.Forms.Button()
        Me.ButtonCreateDatabase = New System.Windows.Forms.Button()
        Me.LabelOpenSuccessfull = New System.Windows.Forms.Label()
        Me.ButtonTestConnection = New System.Windows.Forms.Button()
        Me.ComboBoxDatabaseType = New System.Windows.Forms.ComboBox()
        Me.TextBoxDatabaseConnectionString = New System.Windows.Forms.TextBox()
        Me.LabelConnectionString = New System.Windows.Forms.Label()
        Me.LabelDatabaseType = New System.Windows.Forms.Label()
        Me.GroupBoxReverseEngineering = New System.Windows.Forms.GroupBox()
        Me.GroupBoxREMessages = New System.Windows.Forms.GroupBox()
        Me.RichTextBoxREErrorMessages = New System.Windows.Forms.RichTextBox()
        Me.LabelPromptErrorMessages = New System.Windows.Forms.Label()
        Me.RichTextBoxREMessages = New System.Windows.Forms.RichTextBox()
        Me.ProgressBarReverseEngineering = New System.Windows.Forms.ProgressBar()
        Me.ButtonReverseEngineerDatabase = New System.Windows.Forms.Button()
        Me.CheckBoxIsDatabaseSynchronised = New System.Windows.Forms.CheckBox()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Tab1 = New System.Windows.Forms.TabPage()
        Me.Advanced = New System.Windows.Forms.TabPage()
        Me.LabelPromptIsDatabaseSynchronised = New System.Windows.Forms.Label()
        Me.TabPageReverseEngineering = New System.Windows.Forms.TabPage()
        Me.TabPageConnection = New System.Windows.Forms.TabPage()
        Me.GroupBoxConnection = New System.Windows.Forms.GroupBox()
        Me.TextBoxRoleName = New System.Windows.Forms.TextBox()
        Me.LabelPromptRoleName = New System.Windows.Forms.Label()
        Me.TextBoxWarehouseName = New System.Windows.Forms.TextBox()
        Me.LabelPromptWarehouseName = New System.Windows.Forms.Label()
        Me.TextBoxSchemaName = New System.Windows.Forms.TextBox()
        Me.LabelPromptSchemaName = New System.Windows.Forms.Label()
        Me.TextBoxDatabaseName = New System.Windows.Forms.TextBox()
        Me.LabelPromptDatabaseName = New System.Windows.Forms.Label()
        Me.TextBoxServerName = New System.Windows.Forms.TextBox()
        Me.LabelPromptServerName = New System.Windows.Forms.Label()
        Me.ButtonApply = New System.Windows.Forms.Button()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.TextBoxPort = New System.Windows.Forms.TextBox()
        Me.LabelPromptPort = New System.Windows.Forms.Label()
        Me.GroupBox_main.SuspendLayout()
        Me.GroupBoxDatabase.SuspendLayout()
        Me.GroupBoxReverseEngineering.SuspendLayout()
        Me.GroupBoxREMessages.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.Tab1.SuspendLayout()
        Me.Advanced.SuspendLayout()
        Me.TabPageReverseEngineering.SuspendLayout()
        Me.TabPageConnection.SuspendLayout()
        Me.GroupBoxConnection.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(649, 59)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(70, 24)
        Me.Button_Cancel.TabIndex = 8
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(649, 27)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(70, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&Okay"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'GroupBox_main
        '
        Me.GroupBox_main.Controls.Add(Me.LabelCoreVersion)
        Me.GroupBox_main.Controls.Add(Me.LabelPromptCoreVersion)
        Me.GroupBox_main.Controls.Add(Me.LabelModelName)
        Me.GroupBox_main.Controls.Add(Me.LabelModel)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDatabase)
        Me.GroupBox_main.Location = New System.Drawing.Point(12, 14)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Size = New System.Drawing.Size(595, 329)
        Me.GroupBox_main.TabIndex = 6
        Me.GroupBox_main.TabStop = False
        '
        'LabelCoreVersion
        '
        Me.LabelCoreVersion.AutoSize = True
        Me.LabelCoreVersion.Location = New System.Drawing.Point(540, 24)
        Me.LabelCoreVersion.Name = "LabelCoreVersion"
        Me.LabelCoreVersion.Size = New System.Drawing.Size(90, 13)
        Me.LabelCoreVersion.TabIndex = 17
        Me.LabelCoreVersion.Text = "LabelCoreVersion"
        '
        'LabelPromptCoreVersion
        '
        Me.LabelPromptCoreVersion.AutoSize = True
        Me.LabelPromptCoreVersion.Location = New System.Drawing.Point(474, 24)
        Me.LabelPromptCoreVersion.Name = "LabelPromptCoreVersion"
        Me.LabelPromptCoreVersion.Size = New System.Drawing.Size(69, 13)
        Me.LabelPromptCoreVersion.TabIndex = 16
        Me.LabelPromptCoreVersion.Text = "Core version:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(68, 24)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 14
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelModel
        '
        Me.LabelModel.AutoSize = True
        Me.LabelModel.Location = New System.Drawing.Point(20, 24)
        Me.LabelModel.Name = "LabelModel"
        Me.LabelModel.Size = New System.Drawing.Size(42, 13)
        Me.LabelModel.TabIndex = 13
        Me.LabelModel.Text = "Model :"
        '
        'GroupBoxDatabase
        '
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonFileSelect)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonCreateDatabase)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelOpenSuccessfull)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonTestConnection)
        Me.GroupBoxDatabase.Controls.Add(Me.ComboBoxDatabaseType)
        Me.GroupBoxDatabase.Controls.Add(Me.TextBoxDatabaseConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelDatabaseType)
        Me.GroupBoxDatabase.Location = New System.Drawing.Point(19, 59)
        Me.GroupBoxDatabase.Name = "GroupBoxDatabase"
        Me.GroupBoxDatabase.Size = New System.Drawing.Size(556, 150)
        Me.GroupBoxDatabase.TabIndex = 12
        Me.GroupBoxDatabase.TabStop = False
        Me.GroupBoxDatabase.Text = "Database:"
        '
        'ButtonFileSelect
        '
        Me.ButtonFileSelect.BackColor = System.Drawing.Color.White
        Me.ButtonFileSelect.BackgroundImage = Global.Boston.My.Resources.Resources.folder16x16
        Me.ButtonFileSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonFileSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonFileSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFileSelect.ForeColor = System.Drawing.Color.White
        Me.ButtonFileSelect.Location = New System.Drawing.Point(275, 23)
        Me.ButtonFileSelect.Name = "ButtonFileSelect"
        Me.ButtonFileSelect.Size = New System.Drawing.Size(16, 16)
        Me.ButtonFileSelect.TabIndex = 8
        Me.ButtonFileSelect.UseVisualStyleBackColor = False
        Me.ButtonFileSelect.Visible = False
        '
        'ButtonCreateDatabase
        '
        Me.ButtonCreateDatabase.BackColor = System.Drawing.Color.White
        Me.ButtonCreateDatabase.BackgroundImage = Global.Boston.My.Resources.Resources.Add16x16
        Me.ButtonCreateDatabase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonCreateDatabase.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCreateDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCreateDatabase.ForeColor = System.Drawing.Color.White
        Me.ButtonCreateDatabase.Location = New System.Drawing.Point(253, 23)
        Me.ButtonCreateDatabase.Name = "ButtonCreateDatabase"
        Me.ButtonCreateDatabase.Size = New System.Drawing.Size(16, 16)
        Me.ButtonCreateDatabase.TabIndex = 7
        Me.ButtonCreateDatabase.UseVisualStyleBackColor = False
        '
        'LabelOpenSuccessfull
        '
        Me.LabelOpenSuccessfull.Location = New System.Drawing.Point(128, 110)
        Me.LabelOpenSuccessfull.Name = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Size = New System.Drawing.Size(410, 26)
        Me.LabelOpenSuccessfull.TabIndex = 6
        Me.LabelOpenSuccessfull.Text = "LabelOpenSuccessfull"
        Me.LabelOpenSuccessfull.Visible = False
        '
        'ButtonTestConnection
        '
        Me.ButtonTestConnection.Location = New System.Drawing.Point(28, 110)
        Me.ButtonTestConnection.Name = "ButtonTestConnection"
        Me.ButtonTestConnection.Size = New System.Drawing.Size(94, 25)
        Me.ButtonTestConnection.TabIndex = 5
        Me.ButtonTestConnection.Text = "Test Connection"
        Me.ButtonTestConnection.UseVisualStyleBackColor = True
        '
        'ComboBoxDatabaseType
        '
        Me.ComboBoxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDatabaseType.FormattingEnabled = True
        Me.ComboBoxDatabaseType.Location = New System.Drawing.Point(128, 20)
        Me.ComboBoxDatabaseType.Name = "ComboBoxDatabaseType"
        Me.ComboBoxDatabaseType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxDatabaseType.TabIndex = 4
        '
        'TextBoxDatabaseConnectionString
        '
        Me.TextBoxDatabaseConnectionString.Location = New System.Drawing.Point(128, 51)
        Me.TextBoxDatabaseConnectionString.Multiline = True
        Me.TextBoxDatabaseConnectionString.Name = "TextBoxDatabaseConnectionString"
        Me.TextBoxDatabaseConnectionString.Size = New System.Drawing.Size(410, 48)
        Me.TextBoxDatabaseConnectionString.TabIndex = 3
        '
        'LabelConnectionString
        '
        Me.LabelConnectionString.AutoSize = True
        Me.LabelConnectionString.Location = New System.Drawing.Point(25, 51)
        Me.LabelConnectionString.Name = "LabelConnectionString"
        Me.LabelConnectionString.Size = New System.Drawing.Size(97, 13)
        Me.LabelConnectionString.TabIndex = 2
        Me.LabelConnectionString.Text = "Connection String :"
        '
        'LabelDatabaseType
        '
        Me.LabelDatabaseType.AutoSize = True
        Me.LabelDatabaseType.Location = New System.Drawing.Point(25, 23)
        Me.LabelDatabaseType.Name = "LabelDatabaseType"
        Me.LabelDatabaseType.Size = New System.Drawing.Size(86, 13)
        Me.LabelDatabaseType.TabIndex = 1
        Me.LabelDatabaseType.Text = "Database Type :"
        '
        'GroupBoxReverseEngineering
        '
        Me.GroupBoxReverseEngineering.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxReverseEngineering.Controls.Add(Me.GroupBoxREMessages)
        Me.GroupBoxReverseEngineering.Controls.Add(Me.ProgressBarReverseEngineering)
        Me.GroupBoxReverseEngineering.Controls.Add(Me.ButtonReverseEngineerDatabase)
        Me.GroupBoxReverseEngineering.Location = New System.Drawing.Point(13, 10)
        Me.GroupBoxReverseEngineering.Name = "GroupBoxReverseEngineering"
        Me.GroupBoxReverseEngineering.Size = New System.Drawing.Size(596, 345)
        Me.GroupBoxReverseEngineering.TabIndex = 15
        Me.GroupBoxReverseEngineering.TabStop = False
        Me.GroupBoxReverseEngineering.Text = "Reverse Engineering"
        '
        'GroupBoxREMessages
        '
        Me.GroupBoxREMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxREMessages.Controls.Add(Me.RichTextBoxREErrorMessages)
        Me.GroupBoxREMessages.Controls.Add(Me.LabelPromptErrorMessages)
        Me.GroupBoxREMessages.Controls.Add(Me.RichTextBoxREMessages)
        Me.GroupBoxREMessages.Location = New System.Drawing.Point(13, 68)
        Me.GroupBoxREMessages.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBoxREMessages.Name = "GroupBoxREMessages"
        Me.GroupBoxREMessages.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBoxREMessages.Size = New System.Drawing.Size(569, 262)
        Me.GroupBoxREMessages.TabIndex = 6
        Me.GroupBoxREMessages.TabStop = False
        Me.GroupBoxREMessages.Text = "Messages:"
        '
        'RichTextBoxREErrorMessages
        '
        Me.RichTextBoxREErrorMessages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBoxREErrorMessages.Location = New System.Drawing.Point(11, 167)
        Me.RichTextBoxREErrorMessages.Name = "RichTextBoxREErrorMessages"
        Me.RichTextBoxREErrorMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.RichTextBoxREErrorMessages.Size = New System.Drawing.Size(544, 90)
        Me.RichTextBoxREErrorMessages.TabIndex = 7
        Me.RichTextBoxREErrorMessages.Text = ""
        '
        'LabelPromptErrorMessages
        '
        Me.LabelPromptErrorMessages.AutoSize = True
        Me.LabelPromptErrorMessages.Location = New System.Drawing.Point(8, 151)
        Me.LabelPromptErrorMessages.Name = "LabelPromptErrorMessages"
        Me.LabelPromptErrorMessages.Size = New System.Drawing.Size(37, 13)
        Me.LabelPromptErrorMessages.TabIndex = 6
        Me.LabelPromptErrorMessages.Text = "Errors:"
        Me.LabelPromptErrorMessages.Visible = False
        '
        'RichTextBoxREMessages
        '
        Me.RichTextBoxREMessages.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxREMessages.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBoxREMessages.Location = New System.Drawing.Point(11, 23)
        Me.RichTextBoxREMessages.Margin = New System.Windows.Forms.Padding(2)
        Me.RichTextBoxREMessages.Name = "RichTextBoxREMessages"
        Me.RichTextBoxREMessages.Size = New System.Drawing.Size(544, 115)
        Me.RichTextBoxREMessages.TabIndex = 5
        Me.RichTextBoxREMessages.Text = ""
        '
        'ProgressBarReverseEngineering
        '
        Me.ProgressBarReverseEngineering.Location = New System.Drawing.Point(184, 22)
        Me.ProgressBarReverseEngineering.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgressBarReverseEngineering.Name = "ProgressBarReverseEngineering"
        Me.ProgressBarReverseEngineering.Size = New System.Drawing.Size(153, 20)
        Me.ProgressBarReverseEngineering.TabIndex = 4
        Me.ProgressBarReverseEngineering.Visible = False
        '
        'ButtonReverseEngineerDatabase
        '
        Me.ButtonReverseEngineerDatabase.Location = New System.Drawing.Point(13, 22)
        Me.ButtonReverseEngineerDatabase.Name = "ButtonReverseEngineerDatabase"
        Me.ButtonReverseEngineerDatabase.Size = New System.Drawing.Size(153, 28)
        Me.ButtonReverseEngineerDatabase.TabIndex = 3
        Me.ButtonReverseEngineerDatabase.Text = "Reverse Engineer Database"
        Me.ButtonReverseEngineerDatabase.UseVisualStyleBackColor = True
        '
        'CheckBoxIsDatabaseSynchronised
        '
        Me.CheckBoxIsDatabaseSynchronised.AutoSize = True
        Me.CheckBoxIsDatabaseSynchronised.Location = New System.Drawing.Point(13, 15)
        Me.CheckBoxIsDatabaseSynchronised.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBoxIsDatabaseSynchronised.Name = "CheckBoxIsDatabaseSynchronised"
        Me.CheckBoxIsDatabaseSynchronised.Size = New System.Drawing.Size(150, 17)
        Me.CheckBoxIsDatabaseSynchronised.TabIndex = 7
        Me.CheckBoxIsDatabaseSynchronised.Text = "Is Database Synchronised"
        Me.CheckBoxIsDatabaseSynchronised.UseVisualStyleBackColor = True
        '
        'DialogOpenFile
        '
        Me.DialogOpenFile.FileName = "OpenFileDialog1"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Tab1)
        Me.TabControl1.Controls.Add(Me.Advanced)
        Me.TabControl1.Controls.Add(Me.TabPageReverseEngineering)
        Me.TabControl1.Controls.Add(Me.TabPageConnection)
        Me.TabControl1.Location = New System.Drawing.Point(8, 8)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(629, 386)
        Me.TabControl1.TabIndex = 9
        '
        'Tab1
        '
        Me.Tab1.Controls.Add(Me.GroupBox_main)
        Me.Tab1.Location = New System.Drawing.Point(4, 22)
        Me.Tab1.Margin = New System.Windows.Forms.Padding(2)
        Me.Tab1.Name = "Tab1"
        Me.Tab1.Padding = New System.Windows.Forms.Padding(2)
        Me.Tab1.Size = New System.Drawing.Size(621, 360)
        Me.Tab1.TabIndex = 0
        Me.Tab1.Text = "Database"
        Me.Tab1.UseVisualStyleBackColor = True
        '
        'Advanced
        '
        Me.Advanced.Controls.Add(Me.LabelPromptIsDatabaseSynchronised)
        Me.Advanced.Controls.Add(Me.CheckBoxIsDatabaseSynchronised)
        Me.Advanced.Location = New System.Drawing.Point(4, 22)
        Me.Advanced.Margin = New System.Windows.Forms.Padding(2)
        Me.Advanced.Name = "Advanced"
        Me.Advanced.Padding = New System.Windows.Forms.Padding(2)
        Me.Advanced.Size = New System.Drawing.Size(621, 360)
        Me.Advanced.TabIndex = 1
        Me.Advanced.Text = "Advanced"
        Me.Advanced.UseVisualStyleBackColor = True
        '
        'LabelPromptIsDatabaseSynchronised
        '
        Me.LabelPromptIsDatabaseSynchronised.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptIsDatabaseSynchronised.Location = New System.Drawing.Point(13, 43)
        Me.LabelPromptIsDatabaseSynchronised.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LabelPromptIsDatabaseSynchronised.Name = "LabelPromptIsDatabaseSynchronised"
        Me.LabelPromptIsDatabaseSynchronised.Size = New System.Drawing.Size(549, 34)
        Me.LabelPromptIsDatabaseSynchronised.TabIndex = 8
        Me.LabelPromptIsDatabaseSynchronised.Text = "Warning: When 'Is Database Synchronised' is checked, Boston will modify the datab" &
    "ase when you change the ORM Model within the Model."
        '
        'TabPageReverseEngineering
        '
        Me.TabPageReverseEngineering.Controls.Add(Me.GroupBoxReverseEngineering)
        Me.TabPageReverseEngineering.Location = New System.Drawing.Point(4, 22)
        Me.TabPageReverseEngineering.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPageReverseEngineering.Name = "TabPageReverseEngineering"
        Me.TabPageReverseEngineering.Size = New System.Drawing.Size(621, 360)
        Me.TabPageReverseEngineering.TabIndex = 2
        Me.TabPageReverseEngineering.Text = "Reverse Engineering"
        Me.TabPageReverseEngineering.UseVisualStyleBackColor = True
        '
        'TabPageConnection
        '
        Me.TabPageConnection.Controls.Add(Me.GroupBoxConnection)
        Me.TabPageConnection.Location = New System.Drawing.Point(4, 22)
        Me.TabPageConnection.Name = "TabPageConnection"
        Me.TabPageConnection.Size = New System.Drawing.Size(621, 360)
        Me.TabPageConnection.TabIndex = 3
        Me.TabPageConnection.Text = "Connection"
        Me.TabPageConnection.UseVisualStyleBackColor = True
        '
        'GroupBoxConnection
        '
        Me.GroupBoxConnection.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxPort)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptPort)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxRoleName)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptRoleName)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxWarehouseName)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptWarehouseName)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxSchemaName)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptSchemaName)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxDatabaseName)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptDatabaseName)
        Me.GroupBoxConnection.Controls.Add(Me.TextBoxServerName)
        Me.GroupBoxConnection.Controls.Add(Me.LabelPromptServerName)
        Me.GroupBoxConnection.Location = New System.Drawing.Point(3, 3)
        Me.GroupBoxConnection.Name = "GroupBoxConnection"
        Me.GroupBoxConnection.Size = New System.Drawing.Size(615, 354)
        Me.GroupBoxConnection.TabIndex = 0
        Me.GroupBoxConnection.TabStop = False
        Me.GroupBoxConnection.Text = "Connection Details:"
        '
        'TextBoxRoleName
        '
        Me.TextBoxRoleName.Location = New System.Drawing.Point(109, 153)
        Me.TextBoxRoleName.MaxLength = 100
        Me.TextBoxRoleName.Name = "TextBoxRoleName"
        Me.TextBoxRoleName.Size = New System.Drawing.Size(179, 20)
        Me.TextBoxRoleName.TabIndex = 9
        '
        'LabelPromptRoleName
        '
        Me.LabelPromptRoleName.AutoSize = True
        Me.LabelPromptRoleName.Location = New System.Drawing.Point(7, 156)
        Me.LabelPromptRoleName.Name = "LabelPromptRoleName"
        Me.LabelPromptRoleName.Size = New System.Drawing.Size(63, 13)
        Me.LabelPromptRoleName.TabIndex = 8
        Me.LabelPromptRoleName.Text = "Role Name:"
        '
        'TextBoxWarehouseName
        '
        Me.TextBoxWarehouseName.Location = New System.Drawing.Point(109, 121)
        Me.TextBoxWarehouseName.MaxLength = 255
        Me.TextBoxWarehouseName.Name = "TextBoxWarehouseName"
        Me.TextBoxWarehouseName.Size = New System.Drawing.Size(457, 20)
        Me.TextBoxWarehouseName.TabIndex = 7
        '
        'LabelPromptWarehouseName
        '
        Me.LabelPromptWarehouseName.AutoSize = True
        Me.LabelPromptWarehouseName.Location = New System.Drawing.Point(7, 124)
        Me.LabelPromptWarehouseName.Name = "LabelPromptWarehouseName"
        Me.LabelPromptWarehouseName.Size = New System.Drawing.Size(96, 13)
        Me.LabelPromptWarehouseName.TabIndex = 6
        Me.LabelPromptWarehouseName.Text = "Warehouse Name:"
        '
        'TextBoxSchemaName
        '
        Me.TextBoxSchemaName.Location = New System.Drawing.Point(109, 93)
        Me.TextBoxSchemaName.MaxLength = 255
        Me.TextBoxSchemaName.Name = "TextBoxSchemaName"
        Me.TextBoxSchemaName.Size = New System.Drawing.Size(457, 20)
        Me.TextBoxSchemaName.TabIndex = 5
        '
        'LabelPromptSchemaName
        '
        Me.LabelPromptSchemaName.AutoSize = True
        Me.LabelPromptSchemaName.Location = New System.Drawing.Point(7, 96)
        Me.LabelPromptSchemaName.Name = "LabelPromptSchemaName"
        Me.LabelPromptSchemaName.Size = New System.Drawing.Size(80, 13)
        Me.LabelPromptSchemaName.TabIndex = 4
        Me.LabelPromptSchemaName.Text = "Schema Name:"
        '
        'TextBoxDatabaseName
        '
        Me.TextBoxDatabaseName.Location = New System.Drawing.Point(109, 62)
        Me.TextBoxDatabaseName.MaxLength = 255
        Me.TextBoxDatabaseName.Name = "TextBoxDatabaseName"
        Me.TextBoxDatabaseName.Size = New System.Drawing.Size(457, 20)
        Me.TextBoxDatabaseName.TabIndex = 3
        '
        'LabelPromptDatabaseName
        '
        Me.LabelPromptDatabaseName.AutoSize = True
        Me.LabelPromptDatabaseName.Location = New System.Drawing.Point(7, 65)
        Me.LabelPromptDatabaseName.Name = "LabelPromptDatabaseName"
        Me.LabelPromptDatabaseName.Size = New System.Drawing.Size(87, 13)
        Me.LabelPromptDatabaseName.TabIndex = 2
        Me.LabelPromptDatabaseName.Text = "Database Name:"
        '
        'TextBoxServerName
        '
        Me.TextBoxServerName.Location = New System.Drawing.Point(109, 33)
        Me.TextBoxServerName.MaxLength = 255
        Me.TextBoxServerName.Name = "TextBoxServerName"
        Me.TextBoxServerName.Size = New System.Drawing.Size(457, 20)
        Me.TextBoxServerName.TabIndex = 1
        '
        'LabelPromptServerName
        '
        Me.LabelPromptServerName.AutoSize = True
        Me.LabelPromptServerName.Location = New System.Drawing.Point(7, 36)
        Me.LabelPromptServerName.Name = "LabelPromptServerName"
        Me.LabelPromptServerName.Size = New System.Drawing.Size(72, 13)
        Me.LabelPromptServerName.TabIndex = 0
        Me.LabelPromptServerName.Text = "Server Name:"
        '
        'ButtonApply
        '
        Me.ButtonApply.Enabled = False
        Me.ButtonApply.Location = New System.Drawing.Point(649, 89)
        Me.ButtonApply.Name = "ButtonApply"
        Me.ButtonApply.Size = New System.Drawing.Size(70, 23)
        Me.ButtonApply.TabIndex = 10
        Me.ButtonApply.Text = "&Apply"
        Me.ButtonApply.UseVisualStyleBackColor = True
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
        '
        'TextBoxPort
        '
        Me.TextBoxPort.Location = New System.Drawing.Point(109, 189)
        Me.TextBoxPort.MaxLength = 100
        Me.TextBoxPort.Name = "TextBoxPort"
        Me.TextBoxPort.Size = New System.Drawing.Size(179, 20)
        Me.TextBoxPort.TabIndex = 11
        '
        'LabelPromptPort
        '
        Me.LabelPromptPort.AutoSize = True
        Me.LabelPromptPort.Location = New System.Drawing.Point(7, 192)
        Me.LabelPromptPort.Name = "LabelPromptPort"
        Me.LabelPromptPort.Size = New System.Drawing.Size(29, 13)
        Me.LabelPromptPort.TabIndex = 10
        Me.LabelPromptPort.Text = "Port:"
        '
        'frmCRUDModel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(763, 402)
        Me.Controls.Add(Me.ButtonApply)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Name = "frmCRUDModel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "Model Configuration"
        Me.Text = "Model Configuration"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBox_main.PerformLayout()
        Me.GroupBoxDatabase.ResumeLayout(False)
        Me.GroupBoxDatabase.PerformLayout()
        Me.GroupBoxReverseEngineering.ResumeLayout(False)
        Me.GroupBoxREMessages.ResumeLayout(False)
        Me.GroupBoxREMessages.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.Tab1.ResumeLayout(False)
        Me.Advanced.ResumeLayout(False)
        Me.Advanced.PerformLayout()
        Me.TabPageReverseEngineering.ResumeLayout(False)
        Me.TabPageConnection.ResumeLayout(False)
        Me.GroupBoxConnection.ResumeLayout(False)
        Me.GroupBoxConnection.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents GroupBox_main As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxDatabase As System.Windows.Forms.GroupBox
    Friend WithEvents LabelConnectionString As System.Windows.Forms.Label
    Friend WithEvents LabelDatabaseType As System.Windows.Forms.Label
    Friend WithEvents TextBoxDatabaseConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents DialogOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DialogFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents LabelModelName As System.Windows.Forms.Label
    Friend WithEvents LabelModel As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDatabaseType As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonTestConnection As System.Windows.Forms.Button
    Friend WithEvents LabelOpenSuccessfull As System.Windows.Forms.Label
    Friend WithEvents GroupBoxReverseEngineering As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonReverseEngineerDatabase As System.Windows.Forms.Button
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents CheckBoxIsDatabaseSynchronised As CheckBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents Tab1 As TabPage
    Friend WithEvents Advanced As TabPage
    Friend WithEvents LabelPromptIsDatabaseSynchronised As Label
    Friend WithEvents LabelCoreVersion As Label
    Friend WithEvents LabelPromptCoreVersion As Label
    Friend WithEvents ButtonCreateDatabase As Button
    Friend WithEvents ButtonFileSelect As Button
    Friend WithEvents ButtonApply As Button
    Friend WithEvents TabPageReverseEngineering As TabPage
    Friend WithEvents GroupBoxREMessages As GroupBox
    Friend WithEvents RichTextBoxREMessages As RichTextBox
    Friend WithEvents ProgressBarReverseEngineering As ProgressBar
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents RichTextBoxREErrorMessages As RichTextBox
    Friend WithEvents LabelPromptErrorMessages As Label
    Friend WithEvents TabPageConnection As TabPage
    Friend WithEvents GroupBoxConnection As GroupBox
    Friend WithEvents TextBoxServerName As TextBox
    Friend WithEvents LabelPromptServerName As Label
    Friend WithEvents TextBoxDatabaseName As TextBox
    Friend WithEvents LabelPromptDatabaseName As Label
    Friend WithEvents TextBoxSchemaName As TextBox
    Friend WithEvents LabelPromptSchemaName As Label
    Friend WithEvents TextBoxRoleName As TextBox
    Friend WithEvents LabelPromptRoleName As Label
    Friend WithEvents TextBoxWarehouseName As TextBox
    Friend WithEvents LabelPromptWarehouseName As Label
    Friend WithEvents TextBoxPort As TextBox
    Friend WithEvents LabelPromptPort As Label
End Class
