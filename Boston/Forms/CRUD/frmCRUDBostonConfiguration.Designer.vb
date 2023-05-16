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
        Me.GroupBoxAutoComplete = New System.Windows.Forms.GroupBox()
        Me.CheckBoxAutoCompleteSingleClickSelects = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonCopyModelIdToClipboard = New System.Windows.Forms.Button()
        Me.LabelConfigurationFileLocation = New System.Windows.Forms.Label()
        Me.LabelUserConfigurationFileLocation = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBoxStartVirtualAnalystInQuietMode = New System.Windows.Forms.CheckBox()
        Me.CheckBoxVirtualAnalystDisplayBriana = New System.Windows.Forms.CheckBox()
        Me.ButtonImportLanguageRules = New System.Windows.Forms.Button()
        Me.GroupBoxDatabase = New System.Windows.Forms.GroupBox()
        Me.CheckBoxUseThreadingDatabaseLoad = New System.Windows.Forms.CheckBox()
        Me.ButtonFileSelect = New System.Windows.Forms.Button()
        Me.TextBoxDatabaseConnectionString = New System.Windows.Forms.TextBox()
        Me.LabelConnectionString = New System.Windows.Forms.Label()
        Me.LabelDatabaseType = New System.Windows.Forms.Label()
        Me.ComboBoxDatabaseType = New System.Windows.Forms.ComboBox()
        Me.GroupBoxDebugging = New System.Windows.Forms.GroupBox()
        Me.CheckBoxUseFlashCardErrorMessages = New System.Windows.Forms.CheckBox()
        Me.CheckBoxShowStackTrace = New System.Windows.Forms.CheckBox()
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
        Me.TextBoxBostonServerPortNumber = New System.Windows.Forms.TextBox()
        Me.LabelPromptBostonServerPortNumber = New System.Windows.Forms.Label()
        Me.CheckBoxClientServerInitialiseClient = New System.Windows.Forms.CheckBox()
        Me.CheckBoxClientServerRequireLoginAtStartup = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUseRemoteUI = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLoggingOutEndsSession = New System.Windows.Forms.CheckBox()
        Me.CheckBoxEnableClientServer = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBoxFactEngine = New System.Windows.Forms.GroupBox()
        Me.TextBoxFactEngineOpenAIAPIKey = New System.Windows.Forms.TextBox()
        Me.LabelPromptFactEngineOpenAIAPIKey = New System.Windows.Forms.Label()
        Me.CheckBoxFactEngineUseGPT3 = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFactEngineUseTransformations = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFactEngineShowStackTrace = New System.Windows.Forms.CheckBox()
        Me.ComboBoxFactEngineUserDateTimeFormat = New System.Windows.Forms.ComboBox()
        Me.ComboBoxFactEngineUserDateFormat = New System.Windows.Forms.ComboBox()
        Me.LabelPromptFactEngineUserDateTimeFormat = New System.Windows.Forms.Label()
        Me.LabelPromptFactEngineUserDateFormat = New System.Windows.Forms.Label()
        Me.CheckBoxFactEngineUseReferenceModeOnlyForSimpleReferenceSchemes = New System.Windows.Forms.CheckBox()
        Me.CheckBoxFactEngineShowDatabaseLogoModelExplorer = New System.Windows.Forms.CheckBox()
        Me.LabelFactEngineDefaultQueryResultLimit = New System.Windows.Forms.Label()
        Me.DomainUpDownFactEngineDefaultQueryResultLimit = New System.Windows.Forms.DomainUpDown()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.GroupBoxERDiagrams = New System.Windows.Forms.GroupBox()
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName = New System.Windows.Forms.CheckBox()
        Me.CheckBoxHideUnknownPredicates = New System.Windows.Forms.CheckBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.LabelPromptReverseEngineeringDefaultReferenceMode = New System.Windows.Forms.Label()
        Me.TextBoxReverseEngineeringDefaultReferenceMode = New System.Windows.Forms.TextBox()
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames = New System.Windows.Forms.CheckBox()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBoxConfiguration = New System.Windows.Forms.GroupBox()
        Me.ButtonImportConfigurationTableItems = New System.Windows.Forms.Button()
        Me.GroupBoxImportExport = New System.Windows.Forms.GroupBox()
        Me.CheckBoxExportSuppressMDAModelElements = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAutomaticallyReportErrorEvents = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAutomaticallyCheckForUpdates = New System.Windows.Forms.CheckBox()
        Me.ButtonReplaceCoreMetamodel = New System.Windows.Forms.Button()
        Me.CheckBoxDiagramSpyShowLinkFactTypes = New System.Windows.Forms.CheckBox()
        Me.CheckBoxSuperuserMode = New System.Windows.Forms.CheckBox()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.GroupBoxModelling = New System.Windows.Forms.GroupBox()
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage = New System.Windows.Forms.CheckBox()
        Me.ComboBoxDefaultGeneralConceptConversion = New System.Windows.Forms.ComboBox()
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo = New System.Windows.Forms.Label()
        Me.TextBoxDefaultReferenceMode = New System.Windows.Forms.TextBox()
        Me.LabelPromptDefaultReferenceMode = New System.Windows.Forms.Label()
        Me.CheckBoxUseDefaultReferenceMode = New System.Windows.Forms.CheckBox()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.GroupBoxCodeGeneration = New System.Windows.Forms.GroupBox()
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames = New System.Windows.Forms.CheckBox()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CheckBoxStoreAndUseBinarySerialisations = New System.Windows.Forms.CheckBox()
        Me.GroupBox_main.SuspendLayout()
        Me.GroupBoxAutoComplete.SuspendLayout()
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
        Me.TabPage6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBoxConfiguration.SuspendLayout()
        Me.GroupBoxImportExport.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.GroupBoxModelling.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.GroupBoxCodeGeneration.SuspendLayout()
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
        Me.GroupBox_main.Controls.Add(Me.GroupBoxAutoComplete)
        Me.GroupBox_main.Controls.Add(Me.GroupBox2)
        Me.GroupBox_main.Controls.Add(Me.GroupBox1)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDatabase)
        Me.GroupBox_main.Controls.Add(Me.GroupBoxDebugging)
        Me.GroupBox_main.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Size = New System.Drawing.Size(595, 623)
        Me.GroupBox_main.TabIndex = 6
        Me.GroupBox_main.TabStop = False
        '
        'GroupBoxAutoComplete
        '
        Me.GroupBoxAutoComplete.Controls.Add(Me.CheckBoxAutoCompleteSingleClickSelects)
        Me.GroupBoxAutoComplete.Location = New System.Drawing.Point(17, 548)
        Me.GroupBoxAutoComplete.Name = "GroupBoxAutoComplete"
        Me.GroupBoxAutoComplete.Size = New System.Drawing.Size(556, 69)
        Me.GroupBoxAutoComplete.TabIndex = 15
        Me.GroupBoxAutoComplete.TabStop = False
        Me.GroupBoxAutoComplete.Text = "AutoComplete"
        '
        'CheckBoxAutoCompleteSingleClickSelects
        '
        Me.CheckBoxAutoCompleteSingleClickSelects.AutoSize = True
        Me.CheckBoxAutoCompleteSingleClickSelects.Location = New System.Drawing.Point(25, 29)
        Me.CheckBoxAutoCompleteSingleClickSelects.Name = "CheckBoxAutoCompleteSingleClickSelects"
        Me.CheckBoxAutoCompleteSingleClickSelects.Size = New System.Drawing.Size(218, 17)
        Me.CheckBoxAutoCompleteSingleClickSelects.TabIndex = 0
        Me.CheckBoxAutoCompleteSingleClickSelects.Text = "Single click selects item in AutoComplete"
        Me.CheckBoxAutoCompleteSingleClickSelects.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 414)
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
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonCopyModelIdToClipboard, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelConfigurationFileLocation, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelUserConfigurationFileLocation, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(25, 45)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(525, 67)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'ButtonCopyModelIdToClipboard
        '
        Me.ButtonCopyModelIdToClipboard.FlatAppearance.BorderSize = 0
        Me.ButtonCopyModelIdToClipboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCopyModelIdToClipboard.Image = Global.Boston.My.Resources.Resources.CopyToClipboard16x16
        Me.ButtonCopyModelIdToClipboard.Location = New System.Drawing.Point(3, 49)
        Me.ButtonCopyModelIdToClipboard.Name = "ButtonCopyModelIdToClipboard"
        Me.ButtonCopyModelIdToClipboard.Size = New System.Drawing.Size(20, 15)
        Me.ButtonCopyModelIdToClipboard.TabIndex = 14
        Me.ButtonCopyModelIdToClipboard.UseVisualStyleBackColor = True
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
        Me.LabelUserConfigurationFileLocation.Location = New System.Drawing.Point(3, 23)
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
        Me.GroupBox1.Location = New System.Drawing.Point(17, 294)
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
        Me.GroupBoxDatabase.Controls.Add(Me.CheckBoxStoreAndUseBinarySerialisations)
        Me.GroupBoxDatabase.Controls.Add(Me.CheckBoxUseThreadingDatabaseLoad)
        Me.GroupBoxDatabase.Controls.Add(Me.ButtonFileSelect)
        Me.GroupBoxDatabase.Controls.Add(Me.TextBoxDatabaseConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelConnectionString)
        Me.GroupBoxDatabase.Controls.Add(Me.LabelDatabaseType)
        Me.GroupBoxDatabase.Controls.Add(Me.ComboBoxDatabaseType)
        Me.GroupBoxDatabase.Location = New System.Drawing.Point(17, 18)
        Me.GroupBoxDatabase.Name = "GroupBoxDatabase"
        Me.GroupBoxDatabase.Size = New System.Drawing.Size(556, 117)
        Me.GroupBoxDatabase.TabIndex = 12
        Me.GroupBoxDatabase.TabStop = False
        Me.GroupBoxDatabase.Text = "Database:"
        '
        'CheckBoxUseThreadingDatabaseLoad
        '
        Me.CheckBoxUseThreadingDatabaseLoad.AutoSize = True
        Me.CheckBoxUseThreadingDatabaseLoad.Location = New System.Drawing.Point(28, 84)
        Me.CheckBoxUseThreadingDatabaseLoad.Name = "CheckBoxUseThreadingDatabaseLoad"
        Me.CheckBoxUseThreadingDatabaseLoad.Size = New System.Drawing.Size(223, 17)
        Me.CheckBoxUseThreadingDatabaseLoad.TabIndex = 11
        Me.CheckBoxUseThreadingDatabaseLoad.Text = "Use &Threading on Boston Database Load"
        Me.CheckBoxUseThreadingDatabaseLoad.UseVisualStyleBackColor = True
        '
        'ButtonFileSelect
        '
        Me.ButtonFileSelect.BackColor = System.Drawing.Color.White
        Me.ButtonFileSelect.BackgroundImage = Global.Boston.My.Resources.Resources.folder16x16
        Me.ButtonFileSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonFileSelect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonFileSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFileSelect.ForeColor = System.Drawing.Color.White
        Me.ButtonFileSelect.Location = New System.Drawing.Point(244, 25)
        Me.ButtonFileSelect.Name = "ButtonFileSelect"
        Me.ButtonFileSelect.Size = New System.Drawing.Size(16, 16)
        Me.ButtonFileSelect.TabIndex = 10
        Me.ButtonFileSelect.UseVisualStyleBackColor = False
        Me.ButtonFileSelect.Visible = False
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
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxUseFlashCardErrorMessages)
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxShowStackTrace)
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxThrowCriticalDebugMessagesToScreen)
        Me.GroupBoxDebugging.Controls.Add(Me.CheckBoxThrowInformationDebugMessagesToScreen)
        Me.GroupBoxDebugging.Controls.Add(Me.ComboBoxDebugMode)
        Me.GroupBoxDebugging.Controls.Add(Me.LabelPrompt_StrategyTerm)
        Me.GroupBoxDebugging.Location = New System.Drawing.Point(17, 141)
        Me.GroupBoxDebugging.Name = "GroupBoxDebugging"
        Me.GroupBoxDebugging.Size = New System.Drawing.Size(443, 147)
        Me.GroupBoxDebugging.TabIndex = 11
        Me.GroupBoxDebugging.TabStop = False
        Me.GroupBoxDebugging.Text = "Error Management:"
        '
        'CheckBoxUseFlashCardErrorMessages
        '
        Me.CheckBoxUseFlashCardErrorMessages.AutoSize = True
        Me.CheckBoxUseFlashCardErrorMessages.Location = New System.Drawing.Point(25, 122)
        Me.CheckBoxUseFlashCardErrorMessages.Name = "CheckBoxUseFlashCardErrorMessages"
        Me.CheckBoxUseFlashCardErrorMessages.Size = New System.Drawing.Size(189, 17)
        Me.CheckBoxUseFlashCardErrorMessages.TabIndex = 16
        Me.CheckBoxUseFlashCardErrorMessages.Text = "Use Flash Card for Error Messages"
        Me.CheckBoxUseFlashCardErrorMessages.UseVisualStyleBackColor = True
        '
        'CheckBoxShowStackTrace
        '
        Me.CheckBoxShowStackTrace.AutoSize = True
        Me.CheckBoxShowStackTrace.Location = New System.Drawing.Point(25, 101)
        Me.CheckBoxShowStackTrace.Name = "CheckBoxShowStackTrace"
        Me.CheckBoxShowStackTrace.Size = New System.Drawing.Size(115, 17)
        Me.CheckBoxShowStackTrace.TabIndex = 15
        Me.CheckBoxShowStackTrace.Text = "Show &Stack Trace"
        Me.CheckBoxShowStackTrace.UseVisualStyleBackColor = True
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
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Location = New System.Drawing.Point(12, 11)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(614, 661)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox_main)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(606, 635)
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
        Me.TabPage2.Size = New System.Drawing.Size(606, 635)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Client/Server"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.TextBoxBostonServerPortNumber)
        Me.GroupBox3.Controls.Add(Me.LabelPromptBostonServerPortNumber)
        Me.GroupBox3.Controls.Add(Me.CheckBoxClientServerInitialiseClient)
        Me.GroupBox3.Controls.Add(Me.CheckBoxClientServerRequireLoginAtStartup)
        Me.GroupBox3.Controls.Add(Me.CheckBoxUseRemoteUI)
        Me.GroupBox3.Controls.Add(Me.CheckBoxLoggingOutEndsSession)
        Me.GroupBox3.Controls.Add(Me.CheckBoxEnableClientServer)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(594, 193)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'TextBoxBostonServerPortNumber
        '
        Me.TextBoxBostonServerPortNumber.Location = New System.Drawing.Point(152, 142)
        Me.TextBoxBostonServerPortNumber.Name = "TextBoxBostonServerPortNumber"
        Me.TextBoxBostonServerPortNumber.Size = New System.Drawing.Size(60, 20)
        Me.TextBoxBostonServerPortNumber.TabIndex = 6
        '
        'LabelPromptBostonServerPortNumber
        '
        Me.LabelPromptBostonServerPortNumber.AutoSize = True
        Me.LabelPromptBostonServerPortNumber.Location = New System.Drawing.Point(7, 145)
        Me.LabelPromptBostonServerPortNumber.Name = "LabelPromptBostonServerPortNumber"
        Me.LabelPromptBostonServerPortNumber.Size = New System.Drawing.Size(139, 13)
        Me.LabelPromptBostonServerPortNumber.TabIndex = 5
        Me.LabelPromptBostonServerPortNumber.Text = "Boston Server Port Number:"
        '
        'CheckBoxClientServerInitialiseClient
        '
        Me.CheckBoxClientServerInitialiseClient.AutoSize = True
        Me.CheckBoxClientServerInitialiseClient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxClientServerInitialiseClient.Enabled = False
        Me.CheckBoxClientServerInitialiseClient.Location = New System.Drawing.Point(65, 111)
        Me.CheckBoxClientServerInitialiseClient.Name = "CheckBoxClientServerInitialiseClient"
        Me.CheckBoxClientServerInitialiseClient.Size = New System.Drawing.Size(95, 17)
        Me.CheckBoxClientServerInitialiseClient.TabIndex = 4
        Me.CheckBoxClientServerInitialiseClient.Text = "Initialise Client:"
        Me.CheckBoxClientServerInitialiseClient.UseVisualStyleBackColor = True
        '
        'CheckBoxClientServerRequireLoginAtStartup
        '
        Me.CheckBoxClientServerRequireLoginAtStartup.AutoSize = True
        Me.CheckBoxClientServerRequireLoginAtStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxClientServerRequireLoginAtStartup.Enabled = False
        Me.CheckBoxClientServerRequireLoginAtStartup.Location = New System.Drawing.Point(22, 88)
        Me.CheckBoxClientServerRequireLoginAtStartup.Name = "CheckBoxClientServerRequireLoginAtStartup"
        Me.CheckBoxClientServerRequireLoginAtStartup.Size = New System.Drawing.Size(138, 17)
        Me.CheckBoxClientServerRequireLoginAtStartup.TabIndex = 3
        Me.CheckBoxClientServerRequireLoginAtStartup.Text = "Require login at startup:"
        Me.CheckBoxClientServerRequireLoginAtStartup.UseVisualStyleBackColor = True
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
        Me.TabPage3.Size = New System.Drawing.Size(606, 635)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "FactEngine"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBoxFactEngine
        '
        Me.GroupBoxFactEngine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxFactEngine.Controls.Add(Me.TextBoxFactEngineOpenAIAPIKey)
        Me.GroupBoxFactEngine.Controls.Add(Me.LabelPromptFactEngineOpenAIAPIKey)
        Me.GroupBoxFactEngine.Controls.Add(Me.CheckBoxFactEngineUseGPT3)
        Me.GroupBoxFactEngine.Controls.Add(Me.CheckBoxFactEngineUseTransformations)
        Me.GroupBoxFactEngine.Controls.Add(Me.CheckBoxFactEngineShowStackTrace)
        Me.GroupBoxFactEngine.Controls.Add(Me.ComboBoxFactEngineUserDateTimeFormat)
        Me.GroupBoxFactEngine.Controls.Add(Me.ComboBoxFactEngineUserDateFormat)
        Me.GroupBoxFactEngine.Controls.Add(Me.LabelPromptFactEngineUserDateTimeFormat)
        Me.GroupBoxFactEngine.Controls.Add(Me.LabelPromptFactEngineUserDateFormat)
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
        'TextBoxFactEngineOpenAIAPIKey
        '
        Me.TextBoxFactEngineOpenAIAPIKey.Location = New System.Drawing.Point(104, 248)
        Me.TextBoxFactEngineOpenAIAPIKey.Name = "TextBoxFactEngineOpenAIAPIKey"
        Me.TextBoxFactEngineOpenAIAPIKey.Size = New System.Drawing.Size(295, 20)
        Me.TextBoxFactEngineOpenAIAPIKey.TabIndex = 12
        '
        'LabelPromptFactEngineOpenAIAPIKey
        '
        Me.LabelPromptFactEngineOpenAIAPIKey.AutoSize = True
        Me.LabelPromptFactEngineOpenAIAPIKey.Location = New System.Drawing.Point(19, 251)
        Me.LabelPromptFactEngineOpenAIAPIKey.Name = "LabelPromptFactEngineOpenAIAPIKey"
        Me.LabelPromptFactEngineOpenAIAPIKey.Size = New System.Drawing.Size(87, 13)
        Me.LabelPromptFactEngineOpenAIAPIKey.TabIndex = 11
        Me.LabelPromptFactEngineOpenAIAPIKey.Text = "OpenAI API Key:"
        '
        'CheckBoxFactEngineUseGPT3
        '
        Me.CheckBoxFactEngineUseGPT3.AutoSize = True
        Me.CheckBoxFactEngineUseGPT3.Location = New System.Drawing.Point(19, 227)
        Me.CheckBoxFactEngineUseGPT3.Name = "CheckBoxFactEngineUseGPT3"
        Me.CheckBoxFactEngineUseGPT3.Size = New System.Drawing.Size(225, 17)
        Me.CheckBoxFactEngineUseGPT3.TabIndex = 10
        Me.CheckBoxFactEngineUseGPT3.Text = "Use GPT3 for NL-2-FEQL Transformations"
        Me.CheckBoxFactEngineUseGPT3.UseVisualStyleBackColor = True
        '
        'CheckBoxFactEngineUseTransformations
        '
        Me.CheckBoxFactEngineUseTransformations.AutoSize = True
        Me.CheckBoxFactEngineUseTransformations.Location = New System.Drawing.Point(19, 204)
        Me.CheckBoxFactEngineUseTransformations.Name = "CheckBoxFactEngineUseTransformations"
        Me.CheckBoxFactEngineUseTransformations.Size = New System.Drawing.Size(185, 17)
        Me.CheckBoxFactEngineUseTransformations.TabIndex = 9
        Me.CheckBoxFactEngineUseTransformations.Text = "Use Transformations (NL-2-FEQL)"
        Me.CheckBoxFactEngineUseTransformations.UseVisualStyleBackColor = True
        '
        'CheckBoxFactEngineShowStackTrace
        '
        Me.CheckBoxFactEngineShowStackTrace.AutoSize = True
        Me.CheckBoxFactEngineShowStackTrace.Location = New System.Drawing.Point(19, 181)
        Me.CheckBoxFactEngineShowStackTrace.Name = "CheckBoxFactEngineShowStackTrace"
        Me.CheckBoxFactEngineShowStackTrace.Size = New System.Drawing.Size(160, 17)
        Me.CheckBoxFactEngineShowStackTrace.TabIndex = 8
        Me.CheckBoxFactEngineShowStackTrace.Text = "Show Stack Trace on Errors"
        Me.CheckBoxFactEngineShowStackTrace.UseVisualStyleBackColor = True
        '
        'ComboBoxFactEngineUserDateTimeFormat
        '
        Me.ComboBoxFactEngineUserDateTimeFormat.FormattingEnabled = True
        Me.ComboBoxFactEngineUserDateTimeFormat.Items.AddRange(New Object() {"dd/MM/yyyy HH:mm:ss", "MM/dd/yyyy HH:mm:ss"})
        Me.ComboBoxFactEngineUserDateTimeFormat.Location = New System.Drawing.Point(154, 147)
        Me.ComboBoxFactEngineUserDateTimeFormat.Name = "ComboBoxFactEngineUserDateTimeFormat"
        Me.ComboBoxFactEngineUserDateTimeFormat.Size = New System.Drawing.Size(245, 21)
        Me.ComboBoxFactEngineUserDateTimeFormat.TabIndex = 7
        '
        'ComboBoxFactEngineUserDateFormat
        '
        Me.ComboBoxFactEngineUserDateFormat.FormattingEnabled = True
        Me.ComboBoxFactEngineUserDateFormat.Items.AddRange(New Object() {"dd/MM/yyyy", "MM/dd/yyyy"})
        Me.ComboBoxFactEngineUserDateFormat.Location = New System.Drawing.Point(153, 117)
        Me.ComboBoxFactEngineUserDateFormat.Name = "ComboBoxFactEngineUserDateFormat"
        Me.ComboBoxFactEngineUserDateFormat.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxFactEngineUserDateFormat.TabIndex = 6
        '
        'LabelPromptFactEngineUserDateTimeFormat
        '
        Me.LabelPromptFactEngineUserDateTimeFormat.AutoSize = True
        Me.LabelPromptFactEngineUserDateTimeFormat.Location = New System.Drawing.Point(16, 147)
        Me.LabelPromptFactEngineUserDateTimeFormat.Name = "LabelPromptFactEngineUserDateTimeFormat"
        Me.LabelPromptFactEngineUserDateTimeFormat.Size = New System.Drawing.Size(113, 13)
        Me.LabelPromptFactEngineUserDateTimeFormat.TabIndex = 5
        Me.LabelPromptFactEngineUserDateTimeFormat.Text = "User DateTime format:"
        '
        'LabelPromptFactEngineUserDateFormat
        '
        Me.LabelPromptFactEngineUserDateFormat.AutoSize = True
        Me.LabelPromptFactEngineUserDateFormat.Location = New System.Drawing.Point(16, 120)
        Me.LabelPromptFactEngineUserDateFormat.Name = "LabelPromptFactEngineUserDateFormat"
        Me.LabelPromptFactEngineUserDateFormat.Size = New System.Drawing.Size(93, 13)
        Me.LabelPromptFactEngineUserDateFormat.TabIndex = 4
        Me.LabelPromptFactEngineUserDateFormat.Text = "User Date Format:"
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
        Me.TabPage4.Size = New System.Drawing.Size(606, 635)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "ER Diagrams"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'GroupBoxERDiagrams
        '
        Me.GroupBoxERDiagrams.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxERDiagrams.Controls.Add(Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName)
        Me.GroupBoxERDiagrams.Controls.Add(Me.CheckBoxHideUnknownPredicates)
        Me.GroupBoxERDiagrams.Location = New System.Drawing.Point(3, 6)
        Me.GroupBoxERDiagrams.Name = "GroupBoxERDiagrams"
        Me.GroupBoxERDiagrams.Size = New System.Drawing.Size(600, 493)
        Me.GroupBoxERDiagrams.TabIndex = 0
        Me.GroupBoxERDiagrams.TabStop = False
        '
        'CheckBoxImportColumnNameEqualsFKReferencedEntityName
        '
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.AutoSize = True
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.Location = New System.Drawing.Point(17, 42)
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.Name = "CheckBoxImportColumnNameEqualsFKReferencedEntityName"
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.Size = New System.Drawing.Size(334, 17)
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.TabIndex = 1
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.Text = "Import/Export - Column Name equals FK Referenced Entity Name"
        Me.CheckBoxImportColumnNameEqualsFKReferencedEntityName.UseVisualStyleBackColor = True
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
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.GroupBox5)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(606, 635)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Reverse Engineering"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox5.Controls.Add(Me.LabelPromptReverseEngineeringDefaultReferenceMode)
        Me.GroupBox5.Controls.Add(Me.TextBoxReverseEngineeringDefaultReferenceMode)
        Me.GroupBox5.Controls.Add(Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames)
        Me.GroupBox5.Location = New System.Drawing.Point(3, 6)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(600, 574)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        '
        'LabelPromptReverseEngineeringDefaultReferenceMode
        '
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.AutoSize = True
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.Location = New System.Drawing.Point(9, 52)
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.Name = "LabelPromptReverseEngineeringDefaultReferenceMode"
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.Size = New System.Drawing.Size(127, 13)
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.TabIndex = 2
        Me.LabelPromptReverseEngineeringDefaultReferenceMode.Text = "Default Reference Mode:"
        '
        'TextBoxReverseEngineeringDefaultReferenceMode
        '
        Me.TextBoxReverseEngineeringDefaultReferenceMode.Location = New System.Drawing.Point(142, 49)
        Me.TextBoxReverseEngineeringDefaultReferenceMode.MaxLength = 10
        Me.TextBoxReverseEngineeringDefaultReferenceMode.Name = "TextBoxReverseEngineeringDefaultReferenceMode"
        Me.TextBoxReverseEngineeringDefaultReferenceMode.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxReverseEngineeringDefaultReferenceMode.TabIndex = 1
        '
        'CheckBoxReverseEngineeringKeepDatabaseColumnNames
        '
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.AutoSize = True
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Location = New System.Drawing.Point(9, 26)
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Name = "CheckBoxReverseEngineeringKeepDatabaseColumnNames"
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Size = New System.Drawing.Size(222, 17)
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.TabIndex = 0
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.Text = "Keep database column names (e.g. case)"
        Me.CheckBoxReverseEngineeringKeepDatabaseColumnNames.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.GroupBox4)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(606, 635)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Boston"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.AutoSize = True
        Me.GroupBox4.Controls.Add(Me.GroupBoxConfiguration)
        Me.GroupBox4.Controls.Add(Me.GroupBoxImportExport)
        Me.GroupBox4.Controls.Add(Me.CheckBoxAutomaticallyReportErrorEvents)
        Me.GroupBox4.Controls.Add(Me.CheckBoxAutomaticallyCheckForUpdates)
        Me.GroupBox4.Controls.Add(Me.ButtonReplaceCoreMetamodel)
        Me.GroupBox4.Controls.Add(Me.CheckBoxDiagramSpyShowLinkFactTypes)
        Me.GroupBox4.Controls.Add(Me.CheckBoxSuperuserMode)
        Me.GroupBox4.Location = New System.Drawing.Point(10, 9)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(2)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(2)
        Me.GroupBox4.Size = New System.Drawing.Size(583, 484)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        '
        'GroupBoxConfiguration
        '
        Me.GroupBoxConfiguration.Controls.Add(Me.ButtonImportConfigurationTableItems)
        Me.GroupBoxConfiguration.Location = New System.Drawing.Point(12, 230)
        Me.GroupBoxConfiguration.Name = "GroupBoxConfiguration"
        Me.GroupBoxConfiguration.Size = New System.Drawing.Size(559, 60)
        Me.GroupBoxConfiguration.TabIndex = 6
        Me.GroupBoxConfiguration.TabStop = False
        Me.GroupBoxConfiguration.Text = "Configuration Items / Reference Tables"
        '
        'ButtonImportConfigurationTableItems
        '
        Me.ButtonImportConfigurationTableItems.Location = New System.Drawing.Point(6, 24)
        Me.ButtonImportConfigurationTableItems.Name = "ButtonImportConfigurationTableItems"
        Me.ButtonImportConfigurationTableItems.Size = New System.Drawing.Size(224, 23)
        Me.ButtonImportConfigurationTableItems.TabIndex = 0
        Me.ButtonImportConfigurationTableItems.Text = "&Import Configuration/Reference Table Items"
        Me.ButtonImportConfigurationTableItems.UseVisualStyleBackColor = True
        '
        'GroupBoxImportExport
        '
        Me.GroupBoxImportExport.Controls.Add(Me.CheckBoxExportSuppressMDAModelElements)
        Me.GroupBoxImportExport.Location = New System.Drawing.Point(12, 155)
        Me.GroupBoxImportExport.Name = "GroupBoxImportExport"
        Me.GroupBoxImportExport.Size = New System.Drawing.Size(559, 70)
        Me.GroupBoxImportExport.TabIndex = 5
        Me.GroupBoxImportExport.TabStop = False
        Me.GroupBoxImportExport.Text = "Import/Export"
        '
        'CheckBoxExportSuppressMDAModelElements
        '
        Me.CheckBoxExportSuppressMDAModelElements.AutoSize = True
        Me.CheckBoxExportSuppressMDAModelElements.Location = New System.Drawing.Point(6, 28)
        Me.CheckBoxExportSuppressMDAModelElements.Name = "CheckBoxExportSuppressMDAModelElements"
        Me.CheckBoxExportSuppressMDAModelElements.Size = New System.Drawing.Size(331, 17)
        Me.CheckBoxExportSuppressMDAModelElements.TabIndex = 0
        Me.CheckBoxExportSuppressMDAModelElements.Text = "Fact Based model (.fbm) Export - Suppress MDA Model Elements"
        Me.CheckBoxExportSuppressMDAModelElements.UseVisualStyleBackColor = True
        '
        'CheckBoxAutomaticallyReportErrorEvents
        '
        Me.CheckBoxAutomaticallyReportErrorEvents.AutoSize = True
        Me.CheckBoxAutomaticallyReportErrorEvents.Location = New System.Drawing.Point(12, 41)
        Me.CheckBoxAutomaticallyReportErrorEvents.Name = "CheckBoxAutomaticallyReportErrorEvents"
        Me.CheckBoxAutomaticallyReportErrorEvents.Size = New System.Drawing.Size(246, 17)
        Me.CheckBoxAutomaticallyReportErrorEvents.TabIndex = 4
        Me.CheckBoxAutomaticallyReportErrorEvents.Text = "Automatically report error events to FactEngine"
        Me.CheckBoxAutomaticallyReportErrorEvents.UseVisualStyleBackColor = True
        '
        'CheckBoxAutomaticallyCheckForUpdates
        '
        Me.CheckBoxAutomaticallyCheckForUpdates.AutoSize = True
        Me.CheckBoxAutomaticallyCheckForUpdates.Location = New System.Drawing.Point(12, 18)
        Me.CheckBoxAutomaticallyCheckForUpdates.Name = "CheckBoxAutomaticallyCheckForUpdates"
        Me.CheckBoxAutomaticallyCheckForUpdates.Size = New System.Drawing.Size(177, 17)
        Me.CheckBoxAutomaticallyCheckForUpdates.TabIndex = 3
        Me.CheckBoxAutomaticallyCheckForUpdates.Text = "Automatically check for updates"
        Me.CheckBoxAutomaticallyCheckForUpdates.UseVisualStyleBackColor = True
        '
        'ButtonReplaceCoreMetamodel
        '
        Me.ButtonReplaceCoreMetamodel.Location = New System.Drawing.Point(377, 12)
        Me.ButtonReplaceCoreMetamodel.Name = "ButtonReplaceCoreMetamodel"
        Me.ButtonReplaceCoreMetamodel.Size = New System.Drawing.Size(149, 23)
        Me.ButtonReplaceCoreMetamodel.TabIndex = 2
        Me.ButtonReplaceCoreMetamodel.Text = "Replace Core Metamodel"
        Me.ButtonReplaceCoreMetamodel.UseVisualStyleBackColor = True
        Me.ButtonReplaceCoreMetamodel.Visible = False
        '
        'CheckBoxDiagramSpyShowLinkFactTypes
        '
        Me.CheckBoxDiagramSpyShowLinkFactTypes.AutoSize = True
        Me.CheckBoxDiagramSpyShowLinkFactTypes.Location = New System.Drawing.Point(12, 80)
        Me.CheckBoxDiagramSpyShowLinkFactTypes.Name = "CheckBoxDiagramSpyShowLinkFactTypes"
        Me.CheckBoxDiagramSpyShowLinkFactTypes.Size = New System.Drawing.Size(201, 17)
        Me.CheckBoxDiagramSpyShowLinkFactTypes.TabIndex = 1
        Me.CheckBoxDiagramSpyShowLinkFactTypes.Text = "Diagram Spy - Show Link Fact Types"
        Me.CheckBoxDiagramSpyShowLinkFactTypes.UseVisualStyleBackColor = True
        '
        'CheckBoxSuperuserMode
        '
        Me.CheckBoxSuperuserMode.AutoSize = True
        Me.CheckBoxSuperuserMode.Enabled = False
        Me.CheckBoxSuperuserMode.Location = New System.Drawing.Point(12, 123)
        Me.CheckBoxSuperuserMode.Margin = New System.Windows.Forms.Padding(2)
        Me.CheckBoxSuperuserMode.Name = "CheckBoxSuperuserMode"
        Me.CheckBoxSuperuserMode.Size = New System.Drawing.Size(304, 17)
        Me.CheckBoxSuperuserMode.TabIndex = 0
        Me.CheckBoxSuperuserMode.Text = "Superuser Mode (used rarely and on advice of FactEngine)"
        Me.CheckBoxSuperuserMode.UseVisualStyleBackColor = True
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.GroupBoxModelling)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(606, 635)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Modelling"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'GroupBoxModelling
        '
        Me.GroupBoxModelling.Controls.Add(Me.CheckBoxModelllingUseThreadingLoadingXMLPage)
        Me.GroupBoxModelling.Controls.Add(Me.ComboBoxDefaultGeneralConceptConversion)
        Me.GroupBoxModelling.Controls.Add(Me.LabelPromptNLAPIConvertsGeneralConceptsTo)
        Me.GroupBoxModelling.Controls.Add(Me.TextBoxDefaultReferenceMode)
        Me.GroupBoxModelling.Controls.Add(Me.LabelPromptDefaultReferenceMode)
        Me.GroupBoxModelling.Controls.Add(Me.CheckBoxUseDefaultReferenceMode)
        Me.GroupBoxModelling.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxModelling.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxModelling.Name = "GroupBoxModelling"
        Me.GroupBoxModelling.Size = New System.Drawing.Size(606, 635)
        Me.GroupBoxModelling.TabIndex = 0
        Me.GroupBoxModelling.TabStop = False
        '
        'CheckBoxModelllingUseThreadingLoadingXMLPage
        '
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.AutoSize = True
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Location = New System.Drawing.Point(18, 123)
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Name = "CheckBoxModelllingUseThreadingLoadingXMLPage"
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Size = New System.Drawing.Size(190, 17)
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.TabIndex = 5
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.Text = "Use Threading Loading XML Page"
        Me.CheckBoxModelllingUseThreadingLoadingXMLPage.UseVisualStyleBackColor = True
        '
        'ComboBoxDefaultGeneralConceptConversion
        '
        Me.ComboBoxDefaultGeneralConceptConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDefaultGeneralConceptConversion.FormattingEnabled = True
        Me.ComboBoxDefaultGeneralConceptConversion.Items.AddRange(New Object() {"Value Type", "Entity Type"})
        Me.ComboBoxDefaultGeneralConceptConversion.Location = New System.Drawing.Point(288, 93)
        Me.ComboBoxDefaultGeneralConceptConversion.Name = "ComboBoxDefaultGeneralConceptConversion"
        Me.ComboBoxDefaultGeneralConceptConversion.Size = New System.Drawing.Size(82, 21)
        Me.ComboBoxDefaultGeneralConceptConversion.TabIndex = 4
        '
        'LabelPromptNLAPIConvertsGeneralConceptsTo
        '
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.AutoSize = True
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.Location = New System.Drawing.Point(18, 96)
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.Name = "LabelPromptNLAPIConvertsGeneralConceptsTo"
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.Size = New System.Drawing.Size(264, 13)
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.TabIndex = 3
        Me.LabelPromptNLAPIConvertsGeneralConceptsTo.Text = "Natural Language API Converts General Concepts To:"
        '
        'TextBoxDefaultReferenceMode
        '
        Me.TextBoxDefaultReferenceMode.Location = New System.Drawing.Point(247, 62)
        Me.TextBoxDefaultReferenceMode.Name = "TextBoxDefaultReferenceMode"
        Me.TextBoxDefaultReferenceMode.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDefaultReferenceMode.TabIndex = 2
        '
        'LabelPromptDefaultReferenceMode
        '
        Me.LabelPromptDefaultReferenceMode.AutoSize = True
        Me.LabelPromptDefaultReferenceMode.Location = New System.Drawing.Point(15, 65)
        Me.LabelPromptDefaultReferenceMode.Name = "LabelPromptDefaultReferenceMode"
        Me.LabelPromptDefaultReferenceMode.Size = New System.Drawing.Size(226, 13)
        Me.LabelPromptDefaultReferenceMode.TabIndex = 1
        Me.LabelPromptDefaultReferenceMode.Text = "Default Reference Mode for new Entity Types:"
        '
        'CheckBoxUseDefaultReferenceMode
        '
        Me.CheckBoxUseDefaultReferenceMode.AutoSize = True
        Me.CheckBoxUseDefaultReferenceMode.Location = New System.Drawing.Point(18, 36)
        Me.CheckBoxUseDefaultReferenceMode.Name = "CheckBoxUseDefaultReferenceMode"
        Me.CheckBoxUseDefaultReferenceMode.Size = New System.Drawing.Size(262, 17)
        Me.CheckBoxUseDefaultReferenceMode.TabIndex = 0
        Me.CheckBoxUseDefaultReferenceMode.Text = "Use default &Reference Mode for new Entity Types"
        Me.CheckBoxUseDefaultReferenceMode.UseVisualStyleBackColor = True
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.GroupBoxCodeGeneration)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(606, 635)
        Me.TabPage8.TabIndex = 7
        Me.TabPage8.Text = "Code Generation"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'GroupBoxCodeGeneration
        '
        Me.GroupBoxCodeGeneration.Controls.Add(Me.CheckBoxCodeGenerationUseSquareBracketsTableNames)
        Me.GroupBoxCodeGeneration.Location = New System.Drawing.Point(3, 6)
        Me.GroupBoxCodeGeneration.Name = "GroupBoxCodeGeneration"
        Me.GroupBoxCodeGeneration.Size = New System.Drawing.Size(600, 595)
        Me.GroupBoxCodeGeneration.TabIndex = 0
        Me.GroupBoxCodeGeneration.TabStop = False
        '
        'CheckBoxCodeGenerationUseSquareBracketsTableNames
        '
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.AutoSize = True
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Location = New System.Drawing.Point(16, 19)
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Name = "CheckBoxCodeGenerationUseSquareBracketsTableNames"
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Size = New System.Drawing.Size(220, 17)
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.TabIndex = 0
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.Text = "Use square brackets around table names"
        Me.CheckBoxCodeGenerationUseSquareBracketsTableNames.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'CheckBoxStoreAndUseBinarySerialisations
        '
        Me.CheckBoxStoreAndUseBinarySerialisations.AutoSize = True
        Me.CheckBoxStoreAndUseBinarySerialisations.Enabled = False
        Me.CheckBoxStoreAndUseBinarySerialisations.Location = New System.Drawing.Point(279, 84)
        Me.CheckBoxStoreAndUseBinarySerialisations.Name = "CheckBoxStoreAndUseBinarySerialisations"
        Me.CheckBoxStoreAndUseBinarySerialisations.Size = New System.Drawing.Size(190, 17)
        Me.CheckBoxStoreAndUseBinarySerialisations.TabIndex = 12
        Me.CheckBoxStoreAndUseBinarySerialisations.Text = "Store and Use Binary Serialisations"
        Me.CheckBoxStoreAndUseBinarySerialisations.UseVisualStyleBackColor = True
        '
        'frmCRUDBostonConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 684)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Name = "frmCRUDBostonConfiguration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TabText = "Boston Configuration"
        Me.Text = "Boston Configuration"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBoxAutoComplete.ResumeLayout(False)
        Me.GroupBoxAutoComplete.PerformLayout()
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
        Me.TabPage6.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBoxConfiguration.ResumeLayout(False)
        Me.GroupBoxImportExport.ResumeLayout(False)
        Me.GroupBoxImportExport.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.GroupBoxModelling.ResumeLayout(False)
        Me.GroupBoxModelling.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.GroupBoxCodeGeneration.ResumeLayout(False)
        Me.GroupBoxCodeGeneration.PerformLayout()
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
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents CheckBoxSuperuserMode As CheckBox
    Friend WithEvents ComboBoxFactEngineUserDateTimeFormat As ComboBox
    Friend WithEvents ComboBoxFactEngineUserDateFormat As ComboBox
    Friend WithEvents LabelPromptFactEngineUserDateTimeFormat As Label
    Friend WithEvents LabelPromptFactEngineUserDateFormat As Label
    Friend WithEvents GroupBoxAutoComplete As GroupBox
    Friend WithEvents CheckBoxAutoCompleteSingleClickSelects As CheckBox
    Friend WithEvents TabPage6 As TabPage
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents CheckBoxReverseEngineeringKeepDatabaseColumnNames As CheckBox
    Friend WithEvents LabelPromptReverseEngineeringDefaultReferenceMode As Label
    Friend WithEvents TextBoxReverseEngineeringDefaultReferenceMode As TextBox
    Friend WithEvents CheckBoxDiagramSpyShowLinkFactTypes As CheckBox
    Friend WithEvents ButtonReplaceCoreMetamodel As Button
    Friend WithEvents CheckBoxAutomaticallyCheckForUpdates As CheckBox
    Friend WithEvents CheckBoxAutomaticallyReportErrorEvents As CheckBox
    Friend WithEvents TabPage7 As TabPage
    Friend WithEvents GroupBoxModelling As GroupBox
    Friend WithEvents TextBoxDefaultReferenceMode As TextBox
    Friend WithEvents LabelPromptDefaultReferenceMode As Label
    Friend WithEvents CheckBoxUseDefaultReferenceMode As CheckBox
    Friend WithEvents CheckBoxShowStackTrace As CheckBox
    Friend WithEvents CheckBoxFactEngineShowStackTrace As CheckBox
    Friend WithEvents TabPage8 As TabPage
    Friend WithEvents GroupBoxCodeGeneration As GroupBox
    Friend WithEvents CheckBoxCodeGenerationUseSquareBracketsTableNames As CheckBox
    Friend WithEvents GroupBoxImportExport As GroupBox
    Friend WithEvents CheckBoxExportSuppressMDAModelElements As CheckBox
    Friend WithEvents CheckBoxUseFlashCardErrorMessages As CheckBox
    Friend WithEvents CheckBoxFactEngineUseTransformations As CheckBox
    Friend WithEvents ButtonCopyModelIdToClipboard As Button
    Friend WithEvents CheckBoxClientServerInitialiseClient As CheckBox
    Friend WithEvents CheckBoxClientServerRequireLoginAtStartup As CheckBox
    Friend WithEvents ComboBoxDefaultGeneralConceptConversion As ComboBox
    Friend WithEvents LabelPromptNLAPIConvertsGeneralConceptsTo As Label
    Friend WithEvents TextBoxBostonServerPortNumber As TextBox
    Friend WithEvents LabelPromptBostonServerPortNumber As Label
    Friend WithEvents GroupBoxConfiguration As GroupBox
    Friend WithEvents ButtonImportConfigurationTableItems As Button
    Friend WithEvents LabelPromptFactEngineOpenAIAPIKey As Label
    Friend WithEvents CheckBoxFactEngineUseGPT3 As CheckBox
    Friend WithEvents TextBoxFactEngineOpenAIAPIKey As TextBox
    Friend WithEvents CheckBoxImportColumnNameEqualsFKReferencedEntityName As CheckBox
    Friend WithEvents CheckBoxModelllingUseThreadingLoadingXMLPage As CheckBox
    Friend WithEvents ButtonFileSelect As Button
    Friend WithEvents CheckBoxUseThreadingDatabaseLoad As CheckBox
    Friend WithEvents CheckBoxStoreAndUseBinarySerialisations As CheckBox
End Class
