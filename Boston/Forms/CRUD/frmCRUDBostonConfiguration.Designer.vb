<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDBostonConfiguration
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

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
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.GroupBox_main = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelConfigurationFileLocation = New System.Windows.Forms.Label()
        Me.LabelUserConfigurationFileLocation = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxStartVirtualAnalystInQuietMode = New System.Windows.Forms.CheckBox()
        Me.CheckBoxVirtualAnalystDisplayBriana = New System.Windows.Forms.CheckBox()
        Me.ButtonImportLanguageRules = New System.Windows.Forms.Button()
        Me.GroupBoxDatabase = New System.Windows.Forms.GroupBox()
        Me.TextBoxDatabaseConnectionString = New System.Windows.Forms.TextBox()
        Me.LabelConnectionString = New System.Windows.Forms.Label()
        Me.LabelDatabaseType = New System.Windows.Forms.Label()
        Me.ComboBoxDatabaseType = New System.Windows.Forms.ComboBox()
        Me.GroupBoxDebugging = New System.Windows.Forms.GroupBox()
        Me.CheckBoxThrowCriticalDebugMessagesToScreen = New System.Windows.Forms.CheckBox()
        Me.CheckBoxThrowInformationDebugMessagesToScreen = New System.Windows.Forms.CheckBox()
        Me.ComboBoxDebugMode = New System.Windows.Forms.ComboBox()
        Me.LabelPrompt_StrategyTerm = New System.Windows.Forms.Label()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxUseRemoteUI = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLoggingOutEndsSession = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnableClientServer = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBoxFactEngine = New System.Windows.Forms.GroupBox()
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer = New System.Windows.Forms.CheckBox()
        Me.LabelFactEngineDefaultQueryResultLimit = New System.Windows.Forms.Label()
        Me.DomainUpDownFactEngineDefaultQueryResultLimit = New System.Windows.Forms.DomainUpDown()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBoxERDiagrams = New System.Windows.Forms.GroupBox()
        Me.CheckBoxHideUnknownPredicates = New System.Windows.Forms.CheckBox()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes = New System.Windows.Forms.CheckBox()
        Me.GroupBox_main.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxDatabase.SuspendLayout()
        Me.GroupBoxDebugging.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBoxFactEngine.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.GroupBoxERDiagrams.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(648, 69)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(70, 24)
        Me.Button_Cancel.TabIndex = 8
        Me.Button_Cancel.Text = "&Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.Location = New System.Drawing.Point(648, 39)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(70, 24)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&Okay"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'GroupBox_main
        '
        Me.GroupBox_main.Controls.Add(Me.GroupBox2)
        Me.GroupBox_main.Controls.Add(Me.GroupBox1)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDatabase)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDebugging)
        Me.GroupBox_main.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Size = New System.Drawing.Size(595, 484)
        Me.GroupBox_main.TabIndex = 6
        Me.GroupBox_main.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 350)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(556, 128)
        Me.GroupBox2.TabIndex = 14
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Configuration:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelConfigurationFileLocation, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelUserConfigurationFileLocation, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(25, 45)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(525, 67)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'LabelConfigurationFileLocation
        '
        Me.LabelConfigurationFileLocation.AutoSize = True
        Me.LabelConfigurationFileLocation.Location = New System.Drawing.Point(3, 0)
        Me.LabelConfigurationFileLocation.MaximumSize = New System.Drawing.Size(300, 50)
        Me.LabelConfigurationFileLocation.Name = "LabelConfigurationFileLocation"
        Me.LabelConfigurationFileLocation.Size = New System.Drawing.Size(152, 13)
        Me.LabelConfigurationFileLocation.TabIndex = 1
        Me.LabelConfigurationFileLocation.Text = "LabelConfigurationFileLocation"
        '
        'LabelUserConfigurationFileLocation
        '
        Me.LabelUserConfigurationFileLocation.AutoSize = True
        Me.LabelUserConfigurationFileLocation.Location = New System.Drawing.Point(3, 33)
        Me.LabelUserConfigurationFileLocation.Name = "LabelUserConfigurationFileLocation"
        Me.LabelUserConfigurationFileLocation.Size = New System.Drawing.Size(174, 13)
        Me.LabelUserConfigurationFileLocation.TabIndex = 2
        Me.LabelUserConfigurationFileLocation.Text = "LabelUserConfigurationFileLocation"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(135, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Configuration File Location:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBoxStartVirtualAnalystInQuietMode)
        Me.GroupBox1.Controls.Add(Me.CheckBoxVirtualAnalystDisplayBriana)
        Me.GroupBox1.Controls.Add(Me.ButtonImportLanguageRules)
        Me.GroupBox1.Location = New System.Drawing.Point(17, 230)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(443, 114)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Virtual Analyst:"
        '
        'CheckBoxStartVirtualAnalystInQuietMode
        '
        Me.CheckBoxStartVirtualAnalystInQuietMode.AutoSize = True
        Me.CheckBoxStartVirtualAnalystInQuietMode.Location = New System.Drawing.Point(25, 45)
        Me.CheckBoxStartVirtualAnalystInQuietMode.Name = "CheckBoxStartVirtualAnalystInQuietMode"
        Me.CheckBoxStartVirtualAnalystInQuietMode.Size = New System.Drawing.Size(204, 17)
        Me.CheckBoxStartVirtualAnalystInQuietMode.TabIndex = 2
        Me.CheckBoxStartVirtualAnalystInQuietMode.Text = "Start the Virtual Analyst in &Quiet Mode"
        Me.CheckBoxStartVirtualAnalystInQuietMode.UseVisualStyleBackColor = True
        '
        'CheckBoxVirtualAnalystDisplayBriana
        '
        Me.CheckBoxVirtualAnalystDisplayBriana.AutoSize = True
        Me.CheckBoxVirtualAnalystDisplayBriana.Location = New System.Drawing.Point(25, 22)
        Me.CheckBoxVirtualAnalystDisplayBriana.Name = "CheckBoxVirtualAnalystDisplayBriana"
        Me.CheckBoxVirtualAnalystDisplayBriana.Size = New System.Drawing.Size(180, 17)
        Me.CheckBoxVirtualAnalystDisplayBriana.TabIndex = 1
        Me.CheckBoxVirtualAnalystDisplayBriana.Text = "Display &Briana the Virtual Analyst"
        Me.CheckBoxVirtualAnalystDisplayBriana.UseVisualStyleBackColor = True
        '
        'ButtonImportLanguageRules
        '
        Me.ButtonImportLanguageRules.Location = New System.Drawing.Point(25, 73)
        Me.ButtonImportLanguageRules.Name = "ButtonImportLanguageRules"
        Me.ButtonImportLanguageRules.Size = New System.Drawing.Size(131, 23)
        Me.ButtonImportLanguageRules.TabIndex = 0
        Me.ButtonImportLanguageRules.Text = "&Import Language Rules"
        Me.ButtonImportLanguageRules.UseVisualStyleBackColor = True
        '
        'GroupBoxDatabase
        '
        Me.GroupBoxDatabase.Controls.Add(Me.TextBoxDatabaseConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelDatabaseType)
        Me.GroupBoxDatabase.Controls.Add(Me.ComboBoxDatabaseType)
        Me.GroupBoxDatabase.Location = New System.Drawing.Point(17, 18)
        Me.GroupBoxDatabase.Name = "GroupBoxDatabase"
        Me.GroupBoxDatabase.Size = New System.Drawing.Size(556, 88)
        Me.GroupBoxDatabase.TabIndex = 12
        Me.GroupBoxDatabase.TabStop = False
        Me.GroupBoxDatabase.Text = "Database:"
        '
        'TextBoxDatabaseConnectionString
        '
        Me.TextBoxDatabaseConnectionString.Location = New System.Drawing.Point(128, 51)
        Me.TextBoxDatabaseConnectionString.Name = "TextBoxDatabaseConnectionString"
        Me.TextBoxDatabaseConnectionString.Size = New System.Drawing.Size(410, 20)
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
        'ComboBoxDatabaseType
        '
        Me.ComboBoxDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDatabaseType.FormattingEnabled = True
        Me.ComboBoxDatabaseType.Location = New System.Drawing.Point(117, 20)
        Me.ComboBoxDatabaseType.Name = "ComboBoxDatabaseType"
        Me.ComboBoxDatabaseType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxDatabaseType.TabIndex = 0
        '
        'GroupBoxDebugging
        '
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxThrowCriticalDebugMessagesToScreen)
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxThrowInformationDebugMessagesToScreen)
        Me.GroupBoxDebugging.Controls.Add(Me.ComboBoxDebugMode)
        Me.GroupBoxDebugging.Controls.Add(Me.LabelPrompt_StrategyTerm)
        Me.GroupBoxDebugging.Location = New System.Drawing.Point(17, 112)
        Me.GroupBoxDebugging.Name = "GroupBoxDebugging"
        Me.GroupBoxDebugging.Size = New System.Drawing.Size(443, 112)
        Me.GroupBoxDebugging.TabIndex = 11
        Me.GroupBoxDebugging.TabStop = False
        Me.GroupBoxDebugging.Text = "Error Management:"
        '
        'CheckBoxThrowCriticalDebugMessagesToScreen
        '
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.AutoSize = True
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.Location = New System.Drawing.Point(25, 77)
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.Name = "CheckBoxThrowCriticalDebugMessagesToScreen"
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.Size = New System.Drawing.Size(304, 17)
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.TabIndex = 14
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.Text = "Throw 'Critical' Error Messages To Screen (Recommended)"
        Me.CheckBoxThrowCriticalDebugMessagesToScreen.UseVisualStyleBackColor = True
        '
        'CheckBoxThrowInformationDebugMessagesToScreen
        '
        Me.CheckBoxThrowInformationDebugMessagesToScreen.AutoSize = True
        Me.CheckBoxThrowInformationDebugMessagesToScreen.Location = New System.Drawing.Point(25, 54)
        Me.CheckBoxThrowInformationDebugMessagesToScreen.Name = "CheckBoxThrowInformationDebugMessagesToScreen"
        Me.CheckBoxThrowInformationDebugMessagesToScreen.Size = New System.Drawing.Size(219, 17)
        Me.CheckBoxThrowInformationDebugMessagesToScreen.TabIndex = 13
        Me.CheckBoxThrowInformationDebugMessagesToScreen.Text = "Throw 'Information' Messages To Screen"
        Me.CheckBoxThrowInformationDebugMessagesToScreen.UseVisualStyleBackColor = True
        '
        'ComboBoxDebugMode
        '
        Me.ComboBoxDebugMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDebugMode.FormattingEnabled = True
        Me.ComboBoxDebugMode.Location = New System.Drawing.Point(71, 23)
        Me.ComboBoxDebugMode.Name = "ComboBoxDebugMode"
        Me.ComboBoxDebugMode.Size = New System.Drawing.Size(302, 21)
        Me.ComboBoxDebugMode.TabIndex = 11
        '
        'LabelPrompt_StrategyTerm
        '
        Me.LabelPrompt_StrategyTerm.AutoSize = True
        Me.LabelPrompt_StrategyTerm.Location = New System.Drawing.Point(22, 26)
        Me.LabelPrompt_StrategyTerm.Name = "LabelPrompt_StrategyTerm"
        Me.LabelPrompt_StrategyTerm.Size = New System.Drawing.Size(43, 13)
        Me.LabelPrompt_StrategyTerm.TabIndex = 12
        Me.LabelPrompt_StrategyTerm.Text = " Mode :"
        '
        'DialogOpenFile
        '
        Me.DialogOpenFile.FileName = "OpenFileDialog1"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Location = New System.Drawing.Point(12, 11)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(614, 528)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox_main)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(606, 502)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(606, 502)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Client/Server"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.CheckBoxUseRemoteUI)
        Me.GroupBox3.Controls.Add(Me.CheckBoxLoggingOutEndsSession)
        Me.GroupBox3.Controls.Add(Me.CheckBoxEnableClientServer)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(594, 129)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'CheckBoxUseRemoteUI
        '
        Me.CheckBoxUseRemoteUI.AutoSize = True
        Me.CheckBoxUseRemoteUI.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxUseRemoteUI.Location = New System.Drawing.Point(58, 65)
        Me.CheckBoxUseRemoteUI.Name = "CheckBoxUseRemoteUI"
        Me.CheckBoxUseRemoteUI.Size = New System.Drawing.Size(102, 17)
        Me.CheckBoxUseRemoteUI.TabIndex = 2
        Me.CheckBoxUseRemoteUI.Text = "Use Remote UI:"
        Me.CheckBoxUseRemoteUI.UseVisualStyleBackColor = True
        '
        'CheckBoxLoggingOutEndsSession
        '
        Me.CheckBoxLoggingOutEndsSession.AutoSize = True
        Me.CheckBoxLoggingOutEndsSession.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxLoggingOutEndsSession.Location = New System.Drawing.Point(6, 42)
        Me.CheckBoxLoggingOutEndsSession.Name = "CheckBoxLoggingOutEndsSession"
        Me.CheckBoxLoggingOutEndsSession.Size = New System.Drawing.Size(154, 17)
        Me.CheckBoxLoggingOutEndsSession.TabIndex = 1
        Me.CheckBoxLoggingOutEndsSession.Text = "Logging Out Ends Session:"
        Me.CheckBoxLoggingOutEndsSession.UseVisualStyleBackColor = True
        '
        'CheckBoxEnableClientServer
        '
        Me.CheckBoxEnableClientServer.AutoSize = True
        Me.CheckBoxEnableClientServer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxEnableClientServer.Location = New System.Drawing.Point(10, 19)
        Me.CheckBoxEnableClientServer.Name = "CheckBoxEnableClientServer"
        Me.CheckBoxEnableClientServer.Size = New System.Drawing.Size(150, 17)
        Me.CheckBoxEnableClientServer.TabIndex = 0
        Me.CheckBoxEnableClientServer.Text = "Enable Boston as a Client:"
        Me.CheckBoxEnableClientServer.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBoxFactEngine)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(606, 502)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "FactEngine"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBoxFactEngine
        '
        Me.GroupBoxFactEngine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxFactEngine.Controls.Add(Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes)
        Me.GroupBoxFactEngine.Controls.Add(Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer)
        Me.GroupBoxFactEngine.Controls.Add(Me.LabelFactEngineDefaultQueryResultLimit)
        Me.GroupBoxFactEngine.Controls.Add(Me.DomainUpDownFactEngineDefaultQueryResultLimit)
        Me.GroupBoxFactEngine.Location = New System.Drawing.Point(6, 6)
        Me.GroupBoxFactEngine.Name = "GroupBoxFactEngine"
        Me.GroupBoxFactEngine.Size = New System.Drawing.Size(597, 493)
        Me.GroupBoxFactEngine.TabIndex = 0
        Me.GroupBoxFactEngine.TabStop = False
        '
        'CheckBoxFactEngineShowDatabaseLogoModelExplorer
        '
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.AutoSize = True
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Location = New System.Drawing.Point(19, 67)
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Name = "CheckBoxFactEngineShowDatabaseLogoModelExplorer"
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Size = New System.Drawing.Size(225, 17)
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.TabIndex = 2
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.Text = "&Show database logo in the Model Explorer"
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer.UseVisualStyleBackColor = True
        '
        'LabelFactEngineDefaultQueryResultLimit
        '
        Me.LabelFactEngineDefaultQueryResultLimit.AutoSize = True
        Me.LabelFactEngineDefaultQueryResultLimit.Location = New System.Drawing.Point(16, 34)
        Me.LabelFactEngineDefaultQueryResultLimit.Name = "LabelFactEngineDefaultQueryResultLimit"
        Me.LabelFactEngineDefaultQueryResultLimit.Size = New System.Drawing.Size(132, 13)
        Me.LabelFactEngineDefaultQueryResultLimit.TabIndex = 1
        Me.LabelFactEngineDefaultQueryResultLimit.Text = "Default Query Result Limit:"
        '
        'DomainUpDownFactEngineDefaultQueryResultLimit
        '
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Items.Add("10")
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Items.Add("100")
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Items.Add("1000")
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Items.Add("10000")
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Items.Add("Infinite")
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Location = New System.Drawing.Point(154, 32)
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Name = "DomainUpDownFactEngineDefaultQueryResultLimit"
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Size = New System.Drawing.Size(120, 20)
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.TabIndex = 0
        Me.DomainUpDownFactEngineDefaultQueryResultLimit.Text = "DomainUpDown1"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.GroupBoxERDiagrams)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(606, 502)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "ER Diagrams"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'GroupBoxERDiagrams
        '
        Me.GroupBoxERDiagrams.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxERDiagrams.Controls.Add(Me.CheckBoxHideUnknownPredicates)
        Me.GroupBoxERDiagrams.Location = New System.Drawing.Point(3, 6)
        Me.GroupBoxERDiagrams.Name = "GroupBoxERDiagrams"
        Me.GroupBoxERDiagrams.Size = New System.Drawing.Size(600, 493)
        Me.GroupBoxERDiagrams.TabIndex = 0
        Me.GroupBoxERDiagrams.TabStop = False
        '
        'CheckBoxHideUnknownPredicates
        '
        Me.CheckBoxHideUnknownPredicates.AutoSize = True
        Me.CheckBoxHideUnknownPredicates.Location = New System.Drawing.Point(17, 19)
        Me.CheckBoxHideUnknownPredicates.Name = "CheckBoxHideUnknownPredicates"
        Me.CheckBoxHideUnknownPredicates.Size = New System.Drawing.Size(150, 17)
        Me.CheckBoxHideUnknownPredicates.TabIndex = 0
        Me.CheckBoxHideUnknownPredicates.Text = "Hide Unknown Predicates"
        Me.CheckBoxHideUnknownPredicates.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes
        '
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.AutoSize = True
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Location = New System.Drawing.Point(19, 90)
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Name = "CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes"
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Size = New System.Drawing.Size(290, 17)
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.TabIndex = 3
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.Text = "Use Reference Mode only for simple reference schemes"
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes.UseVisualStyleBackColor = True
        '
        'frmCRUDBostonConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 561)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Name = "frmCRUDBostonConfiguration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "Boston Configuration"
        Me.Text = "Boston Configuration"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBoxDatabase.ResumeLayout(False)
        Me.GroupBoxDatabase.PerformLayout()
        Me.GroupBoxDebugging.ResumeLayout(False)
        Me.GroupBoxDebugging.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBoxFactEngine.ResumeLayout(False)
        Me.GroupBoxFactEngine.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.GroupBoxERDiagrams.ResumeLayout(False)
        Me.GroupBoxERDiagrams.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents GroupBox_main As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxDatabase As System.Windows.Forms.GroupBox
    Friend WithEvents LabelConnectionString As System.Windows.Forms.Label
    Friend WithEvents LabelDatabaseType As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDatabaseType As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxDebugging As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxThrowCriticalDebugMessagesToScreen As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxThrowInformationDebugMessagesToScreen As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxDebugMode As System.Windows.Forms.ComboBox
    Friend WithEvents LabelPrompt_StrategyTerm As System.Windows.Forms.Label
    Friend WithEvents TextBoxDatabaseConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonImportLanguageRules As System.Windows.Forms.Button
    Friend WithEvents DialogOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents DialogFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents CheckBoxVirtualAnalystDisplayBriana As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxStartVirtualAnalystInQuietMode As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents LabelConfigurationFileLocation As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelUserConfigurationFileLocation As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxEnableClientServer As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLoggingOutEndsSession As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUseRemoteUI As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents GroupBoxFactEngine As GroupBox
    Friend WithEvents DomainUpDownFactEngineDefaultQueryResultLimit As DomainUpDown
    Friend WithEvents LabelFactEngineDefaultQueryResultLimit As Label
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents CheckBoxFactEngineShowDatabaseLogoModelExplorer As CheckBox
    Friend WithEvents TabPage4 As TabPage
    Friend WithEvents GroupBoxERDiagrams As GroupBox
    Friend WithEvents CheckBoxHideUnknownPredicates As CheckBox
    Friend WithEvents CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes As CheckBox
End Class
