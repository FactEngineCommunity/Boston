Imports System
Imports System.IO
Imports System.Data.SqlClient
Imports DynamicClassLibrary.Factory
Imports System.Threading
Imports System.Reflection
Imports System.Security.AccessControl
Imports Boston.DuplexServiceClient  'Client/Server
Imports System.ServiceModel
Imports AutoUpdaterDotNET
Imports System.Configuration
Imports System.ComponentModel

Public Class frmMain

    Private ltThread As Thread
    Private ltSplashThread As Thread

    Public WithEvents zfrmModelExplorer As frmToolboxEnterpriseExplorer = Nothing
    Public zfrm_toolbox As frmToolbox = Nothing
    Public zfrm_diagram_overview As frmDiagramOverview = Nothing
    Public zfrm_orm_reading_editor As frmToolboxORMReadingEditor = Nothing
    Public zfrm_properties As frmToolboxProperties
    Public zfrm_KL_theorem_writer As frmToolboxKLTheoremWriter = Nothing
    Public zrORMModel_view As frmDiagramORM = Nothing
    Public zfrm_Brain_box As frmToolboxBrainBox = Nothing
    Public zfrmFactEngine As frmFactEngine = Nothing
    Public zfrmStartup As frmStartup = Nothing
    Public zfrmCRUDAddGroup As frmCRUDAddGroup = Nothing
    Public zfrmCRUDAddNamespace As frmCRUDAddNamespace = Nothing
    Public zfrmCRUDAddProject As frmCRUDAddProject = Nothing
    Public zfrmCRUDAddRole As frmCRUDAddRole = Nothing
    Public zfrmCRUDAddUser As frmCRUDAddUser = Nothing
    Public zfrmCRUDBostonConfiguration As frmCRUDBostonConfiguration = Nothing
    Public zfrmCRUDEditGroup As frmCRUDEditGroup = Nothing
    Public zfrmCRUDEditNamespace As frmCRUDEditNamespace = Nothing
    Public zfrmCRUDEditProject As frmCRUDEditProject = Nothing
    Public zfrmCRUDEditRole As frmCRUDEditRole = Nothing
    Public zfrmCRUDEditUser As frmCRUDEditUser = Nothing
    Public zfrm_ER_diagram_view As frmDiagramERD = Nothing
    Public zfrmOntologyORMModelView As frmDiagramORMForOntologyBrowser = Nothing
    Public zfrm_PGS_diagram_view As frmDiagramPGS = Nothing
    Public zfrmStateTransitionDiagramView As frmStateTransitionDiagram = Nothing
    Public zfrmNotifications As frmNotifications = Nothing
    Friend zfrmCodeGenerator As UI.MainForm = Nothing
    Public zfrmUMLUseCaseDiagramView As frmDiagrmUMLUseCase

    'ClientServer
    'NB See method InitialiseClient
    'NB See method Private prDubplexServiceClient 
    'NB See Main.Designer  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    'http://localhost
    Private ServiceEndpointUri As String = My.Settings.BostonServerIPAddress & ":" & My.Settings.BostonServerPortNumber & "/WCFServices/DuplexService"

    Private Sub frm_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            'NB The current SVN Repository for Richmond is at
            '  github
            '-----------------------------------------------------
            'Configuration file ends up in something like: C:\Users\Viev\AppData\Local\Viev_Pty_Ltd\Boston.exe_Url_wd25rcgtvmds0ynngskc2ps2lwmmryie\2.5.0.0
            '------------------------------------------------------------------------------------------------------------------------

            If My.Settings.DebugMode = pcenumDebugMode.Debug.ToString Then
                prApplication.ThrowErrorMessage("Starting Boston", pcenumErrorType.Information)
            End If

            Dim lsMessage As String = ""

            If My.Settings.FirstRun Then
                Try
                    Dim lrCommonApplicationData As New CommonApplicationData("FactEngine", "Boston")
                    Call lrCommonApplicationData.CreateFolders(True)
                Catch ex As Exception
                    'Not a biggie. This is only rarely a problem.
                End Try

            End If

            Me.MenuStrip_main.ImageScalingSize = New Drawing.Size(16, 16)

            Cursor.Current = Cursors.WaitCursor

            '---------------------------------------------------------------------------
            'Check if the user wants to log the startup process
            '----------------------------------------------------
            If My.Computer.Keyboard.ShiftKeyDown Then
                pbLogStartup = True
            End If

            '-------------------------------------------------------------------------------------------------------------
            'Change the SoftwareCategory before building the project for release.
            Me.StatusLabelGeneralStatus.Text = "Setting Software Category"
            prSoftwareCategory = pcenumSoftwareCategory.Professional
            Me.StatusLabelGeneralStatus.Text = "Software Category Set"

            '====================================================================================
            'Notes
            '  Core v2.1 introduces changes to the StateTransitionDiagram model, with changes to the underlying ModelElements. Introduced in Boston v5.4
            psApplicationApplicationVersionNr = "6.6"
            psApplicationDatabaseVersionNr = "1.37"
            'NB To access the Core version number go to prApplication.CMML.Core.CoreVersionNumber once the Core has loaded.

            Dim loAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
            Dim loFVI As System.Diagnostics.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(loAssembly.Location)
            psAssemblyFileVersionNumber = loFVI.FileVersion

            'Make sure the Config is up to date
            Dim loConfiguration As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
            If Not loConfiguration.HasFile Then

                Try
                    Dim lsConfigurationFileLocation As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Boston", "ConfigurationFileLocation", Nothing)
                    If lsConfigurationFileLocation IsNot Nothing Then
                        loConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(lsConfigurationFileLocation)
                        My.Settings.Save()
                        loConfiguration.Save(ConfigurationSaveMode.Full, True)
                        GoTo ConfigurationOK
                    End If

                    Dim lsPath = Path.GetDirectoryName(Boston.GetConfigFileLocation)
                    Dim lasAssemblyFileNumber As New List(Of String) From {"6.1.1.0", "6.1.0.0", "6.0.1.0", "4.0.0.0"}
                    For Each lsAssemblyFileVersionNr In lasAssemblyFileNumber
                        lsPath = lsPath.Replace("FactEngine", "Viev_Pty_Ltd").Replace(psAssemblyFileVersionNumber, lsAssemblyFileVersionNr) & "\user.config"
                        If File.Exists(lsPath) Then
                            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Boston.GetConfigFileLocation))
                            File.Copy(lsPath, Boston.GetConfigFileLocation)

                            MsgBox("As part Of this upgrade Boston needs To restart. Press [Ok] To close Boston And Then restart Boston.")
                            Me.Close()
                            Me.Dispose()

                        End If
                    Next
                Catch ex As Exception
                    'Not a biggie
                End Try

                My.Settings.Upgrade()
                My.Settings.Save()

            End If

ConfigurationOK:

            If Not My.Settings.UseVirtualUI Then
                ltSplashThread = New Thread(AddressOf Me.LoadSplashScreen)
                ltSplashThread.IsBackground = True
                ltSplashThread.Start(psAssemblyFileVersionNumber)
            Else
                Call Me.LoadSplashScreen(psAssemblyFileVersionNumber)
            End If

            '==============================================================================================================================
            Me.StatusLabelGeneralStatus.Text = "Initialising Application"
            prApplication = New tApplication
            Me.StatusLabelGeneralStatus.Text = "Application Initialised"

            prApplication.SoftwareCategory = prSoftwareCategory

            prApplication.ToolboxForms = New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
            prApplication.LeftToolboxForms = New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
            prApplication.RightToolboxForms = New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)
            prApplication.ActivePages = New List(Of WeifenLuo.WinFormsUI.Docking.DockContent)

            Me.StatusLabelGeneralStatus.Text = "Loaded docking forms"


            prApplication.MainForm = Me

            '==============================================================================================================================

            Me.StatusLabelGeneralStatus.Text = "Checking For Plugins"
            '===============================================================================
            'Get the Plugins for the Application.
            '  NB Plugins are not critical, so if the \plugins\ directory doesn't exist, then skip this step.
            Dim lsPluginDirectoryPath As String
            lsPluginDirectoryPath = Boston.MyPath & "\plugins\"
            If Directory.Exists(lsPluginDirectoryPath) Then
                prApplication.Plugins = PluginServices.FindPlugins(lsPluginDirectoryPath, "IPlugin")

                'Dim lrPlugin As PluginServices.AvailablePlugin
                Dim liInd As Integer = 0
                If prApplication.Plugins IsNot Nothing Then
                    For liInd = 0 To prApplication.Plugins.Count - 1
                        'Create and initialize plugin
                        prApplication.Plugins(liInd).PluginObject = DirectCast(PluginServices.CreateInstance(prApplication.Plugins(liInd)), Viev.PluginFramework.IPlugin)
                        prApplication.PluginInterface.DockPanel = Me.DockPanel
                        prApplication.Plugins(liInd).PluginObject.Initialize(prApplication.PluginInterface)
                    Next
                End If
            End If
            '===============================================================================
            '-------------------------------------------------------------------------------------------------------------
            '20170101-VM-Might pay to (first-use) copy the database to the following directory and set the user permissions to 
            '  stop MS Access "An updatable query Is required" errors, when the JetEngine can't access the database file.
            '  There's a problem storing the database in the C:\ProgramFiles\<ApplicationDirectory> because of hightened security 
            '  in later versions of MS Windows.
            'NB Initially, testing to see if changing the database directory security permissions can be done here (see below).          
            Try
                publicAccessControl.AddDirectorySecurity(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData,
                                                     "Users",
                                                     FileSystemRights.FullControl,
                                                     AccessControlType.Allow)
            Catch
                'Not a biggie. But Boston data does need to be written to C:\ProgramData. If that fails, the customer will have to get in touch.
            End Try

            Dim lsDatabaseLocation As String = ""
            Dim lsDatabaseName As String = ""
            Dim lsDatabaseLocationDirectory As String = ""
            Dim lsDatabaseType As String = "" 'Jet, SQLServer

            Dim lsConnectionString As String
            lsConnectionString = Trim(My.Settings.DatabaseConnectionString)
            Dim lsCommonDatabaseFileLocation As String = ""

            Dim lbFirstRun As Boolean = My.Settings.FirstRun

            Me.StatusLabelGeneralStatus.Text = "Checking Database availability"
            If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then
                Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")
                lsDatabaseName = Path.GetFileName(lsDatabaseLocation)
                lsDatabaseLocationDirectory = Path.GetDirectoryName(lsDatabaseLocation)
                lsDatabaseType = lrSQLConnectionStringBuilder("Provider")

                If My.Settings.FirstRun = True Then
                    '----------------------------------------------------------------------------------------
                    'Move the database to My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData
                    '  and update the ConnectionString for the database.
                    '----------------------------------------------------------------------------------------
                    lsCommonDatabaseFileLocation = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\database"
                    IO.Directory.CreateDirectory(lsCommonDatabaseFileLocation)

                    Dim lsFirstRunDatabaseLocation As String = ""
                    lsFirstRunDatabaseLocation = Boston.MyPath & "\database\" & lsDatabaseName
                    Try
                        IO.File.Copy(lsFirstRunDatabaseLocation, lsCommonDatabaseFileLocation & "\" & lsDatabaseName, False)
                    Catch
                        'If the file already exists then not a problem.
                    End Try

                    lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True)
                    lrSQLConnectionStringBuilder.Add("Provider", lsDatabaseType)
                    lrSQLConnectionStringBuilder.Add("Data Source", lsCommonDatabaseFileLocation & "\" & lsDatabaseName)

                    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString

                    My.Settings.FirstRun = False
                    My.Settings.Save()

                    If pbLogStartup Then
                        lsMessage = "FirstRun-Moved database To " & lsCommonDatabaseFileLocation
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)
                    End If
                End If

            End If

            'DockPanel
            'Dim configFile As String = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config")            
            Dim configFile As String = System.IO.Path.Combine(Boston.MyPath, "DockPanel.config")

            Me.IsMdiContainer = True

            prApplication.ApplicationVersionNr = psApplicationApplicationVersionNr
            'The database version required by the Boston application.
            '  NB May be different from My.Settings.DatabaseVersionNumber, which is the actual version of the database installed.
            prApplication.DatabaseVersionNr = psApplicationDatabaseVersionNr

#Region "Open the database & Upgrade If necessary"
            Me.StatusLabelGeneralStatus.Text = "Opening Database"
            If Boston.OpenDatabase() Then

                If pbLogStartup Then
                    prApplication.ThrowErrorMessage("Successfully opened the database", pcenumErrorType.Information)
                End If

                '----------------------------------------------------------------------------------------------------------------------
                'Check to see if the database is the correct version
                '  NB Change the culture of the Application to EN-US because the CDbl function (below) will error if the 
                '  users computer regional settings has a "," (comma) for a decimal separator.
                '  Setting the culture to EN-US will ensure the the string of the database version (which uses "." decimal separator)
                '  will correctly convert to a double precision number.
                '----------------------------------------------------------------------------------------------------------------------

                Application.CurrentCulture = New System.Globalization.CultureInfo("EN-US")

                Dim lsDatabaseVersionNumber As String = ""
                lsDatabaseVersionNumber = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
                If CDbl(prApplication.DatabaseVersionNr) <> CDbl(lsDatabaseVersionNumber) Then
                    '--------------------------------------------------------------------------------------
                    'The Richmond application requires a different DatabaseVersion than the one installed
                    '--------------------------------------------------------------------------------------
                    If CDbl(prApplication.DatabaseVersionNr) > CDbl(lsDatabaseVersionNumber) Then
                        If Me.PerformDatabaseUpgrade() Then
                            '----------------------------------------------------
                            'Great. The database upgrade finished successfully.
                            '----------------------------------------------------
                        Else
                            '---------------------------------------------------------------
                            'Messages to the user handled within Me.PerformDatabaseUpgrade
                            '---------------------------------------------------------------
                            Me.Close()
                            Me.Dispose()
                            Exit Sub
                        End If
                    Else
                        '--------------------------------------------------------------------------------------------------------------
                        'Real problems exist. The application requires a DatabaseVersionNumber 'less' than the one installed
                        '--------------------------------------------------------------------------------------------------------------
                        lsMessage = "Contact FactEngine support. This installation Of Boston requires a Database Version Number less than the one installed"
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "Database version required by software  " & prApplication.DatabaseVersionNr & vbCrLf
                        lsMessage &= "Required database version (Configuration) " & My.Settings.DatabaseVersionNumber & vbCrLf
                        lsMessage &= "Database version (actual database) " & lsDatabaseVersionNumber & vbCrLf
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "Installed database location " & vbCrLf
                        lsMessage &= lsCommonDatabaseFileLocation
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical)
                        Me.Close()
                        Me.Dispose()
                        Exit Sub
                    End If
                End If
#End Region

                Me.StatusLabelGeneralStatus.Text = "Database Opened Successfully"

                '=========================================================================
                'CodeSafe - Settings
                My.Settings.UsecaseShapeLibrary = ".\shapelibrary\usecase.shl"

                '======================================================
                'Load the Core Model
                '---------------------
                ltThread = New Thread(AddressOf Me.LoadCoreModel)
                ltThread.IsBackground = True
                ltThread.Start()
                '======================================================

                '=======================================
                Call TableModel.GetModelDetails(prApplication.Language.Model)
                Call prApplication.Language.Model.Load()

                prApplication.Language.LanguagePhrase = Language.TableLanguagePhrase.GetLanguagePhrasesByLanguage

                If pbLogStartup Then
                    prApplication.ThrowErrorMessage("Successfully loaded the Language Model", pcenumErrorType.Information)
                End If
                '=======================================

                '==========================================
                'Client/Server                
#Region "Client/Server"
                If My.Settings.UseClientServer _
                And My.Settings.RequireLoginAtStartup _
                And Not My.Settings.UseWindowsAuthenticationVirtualUI Then
                    If frmLogin.ShowDialog() = Windows.Forms.DialogResult.OK Then

                        '------------------------------------------------------------------
                        'LogIn from populates prApplication.User
                        Call Me.logInUser(prApplication.User)
                    Else
                        Me.Close()
                        Me.Dispose()
                    End If
                ElseIf My.Settings.RequireLoginAtStartup _
                And Not My.Settings.UseWindowsAuthenticationVirtualUI Then
                    If frmLogin.ShowDialog() = Windows.Forms.DialogResult.OK Then

                        '------------------------------------------------------------------
                        'LogIn from populates prApplication.User
                        Call Me.logInUser(prApplication.User)
                    Else
                        Me.Close()
                        Me.Dispose()
                    End If
                End If

                If My.Settings.UseClientServer = True Then
                    If My.Settings.InitialiseClient Then
                        Call Me.InitializeClient() 'Connects to the Boston Server Host
                    End If

                    If prUser IsNot Nothing Then
                        Call Me.logInUser(prUser)
                    End If
                End If
#End Region

                Call Me.SetupForm()

                '=======================================================================================================================
                '-----------------------
                'Load the Startup Page
                '-----------------------
                Dim lrChildForm As New frmStartup
                lrChildForm.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
                Me.zfrmStartup = lrChildForm

                If pbLogStartup Then
                    prApplication.ThrowErrorMessage("Successfully loaded the Startup page", pcenumErrorType.Information)
                End If
                '=======================================================================================================================

                If pbLogStartup Then
                    prApplication.ThrowErrorMessage("Finished loading the Main form.", pcenumErrorType.Information)
                End If

                Cursor.Current = Cursors.Default

                '----------------------------------------------------------------------------
                'Housekeeping
                '-------------------------
                Dim lrReferenceTable As New ReferenceTable(34, "Registration")
                TableReferenceTable.CreateReferenceTableIfNotExists(lrReferenceTable)
                Dim lrReferenceField As New tReferenceField(34, 1, "ApplicationKey", 3, 50, False, False)
                tableReferenceField.CreateReferenceFieldIfNotExists(lrReferenceField)
                lrReferenceField = New tReferenceField(34, 2, "RegistrationKey", 3, 50, False, False)
                tableReferenceField.CreateReferenceFieldIfNotExists(lrReferenceField)
                Dim lrDefaultReferenceField As New tReferenceField(34, 3, "DefaultRegistrationKey", 3, 50, False, False)
                tableReferenceField.CreateReferenceFieldIfNotExists(lrReferenceField)

                'Set the ApplicationKey
                Dim lrReferenceFieldValue As New tReferenceFieldValue(34, 1, 1, publicRegistration.GenerateApplicationKey)
                Call TableReferenceFieldValue.CreateReferenceFieldValueIfNotExists(lrReferenceFieldValue)
                If lbFirstRun Then
                    TableReferenceFieldValue.UpdateReferenceFieldValue(lrReferenceFieldValue)
                End If

                '----------------------------------------------------------------------------
                'Registration Checking
                '-------------------------
#Region "Registration Checking"
                Dim lbCanCheckForUpdates As Boolean = False
                Try
                    Dim lsApplicationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 1, True)
                    Dim lsRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 2, True)
                    Dim lsDefaultRegistrationKey As String = TableReferenceFieldValue.GetReferenceFieldValue(34, 3, True)

                    If lsApplicationKey = "a" Then 'Initial value, but should have been covered by FirstRun (lbFirstRun)
                        'No Applicatipon Key exists in the database. Create one.
                        lsApplicationKey = publicRegistration.GenerateApplicationKey
                        lrReferenceFieldValue = New tReferenceFieldValue(34, 1, 1, lsApplicationKey)
                        Call lrReferenceFieldValue.Save()
                    End If

                    If lsRegistrationKey = "" Then
                        'No Registration Key exists in the database. Create one. Product won't be registered.
                        lsRegistrationKey = "1A66-067A-B9A2-5D7A-CE1F-0A38-EAB6"
                        lrReferenceFieldValue = New tReferenceFieldValue(34, 2, 1, lsRegistrationKey)
                        Call lrReferenceFieldValue.Save()
                    End If

                    If lsDefaultRegistrationKey = "" Then
                        'No Default Registration Key exists in the database. Create one. Product won't be registered.                        
                        lrReferenceFieldValue = New tReferenceFieldValue(34, 3, 1, lsRegistrationKey)
                        Call lrReferenceFieldValue.Save()
                    End If

                    Dim lrRegistrationResult As New tRegistrationResult

                    If prSoftwareCategory = pcenumSoftwareCategory.Student Then
                        GoTo SkipRegistrationChecking
                    End If

                    If publicRegistration.CheckRegistration(lsApplicationKey, lsRegistrationKey, lrRegistrationResult, lsDefaultRegistrationKey) Then
                        If lrRegistrationResult.SubscriptionType = "Subscription" Then lbCanCheckForUpdates = True
                    Else
                        'The Trial must be up, or new install of Boston.
                        lsMessage = "Please contact FactEngine to obtain a registration key for Boston."
                        lsMessage.AppendDoubleLineBreak("Either")
                        lsMessage.AppendLine("1. You have installed a New copy of Boston Professional;")
                        lsMessage.AppendLine("2. Your trial of Boston has expired; Or")
                        lsMessage.AppendLine("3. Your registration key Is invalid.")
                        MsgBox(lsMessage)

                        Dim lrRegistrationForm As New frmRegistration
                        If Not lrRegistrationForm.ShowDialog() Then
                            Call Me.Close()
                            Exit Sub
                        End If
                    End If

                    'Me.LabelRegistrationStatus.Text = lrRegistrationResult.SoftwareType & " " & lrRegistrationResult.SubscriptionType & lrRegistrationResult.RegisteredToDate
                Catch ex As Exception
                    Dim lrRegistrationForm As New frmRegistration
                    Call lrRegistrationForm.ShowDialog()
                    Call Me.Close()
                    Exit Sub
                End Try
#End Region

SkipRegistrationChecking:
                '-----------------------------------------------------------
                'Automatic Update Checker
                '-------------------------
                If My.Settings.UseAutoUpdateChecker And lbCanCheckForUpdates Then
                    AutoUpdater.InstalledVersion = New Version(psAssemblyFileVersionNumber)
                    AutoUpdater.Start("https://www.factengine.ai/products/Boston/update-info.xml")
                End If

            Else
                '-------------------------------------------------------------------
                'The database wasn't opened successfully, so close the application.
                '-------------------------------------------------------------------

                'Call Me.LoadCRUDRichmondConfiguration()

                Me.Close()
                Me.Dispose()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadCoreModel()

        Try
            prApplication.CMML.Core = New FBM.Model(pcenumLanguage.ORMModel, pcenumCMMLCoreModel.Core.ToString, True)

            Boston.WriteToStatusBar("Loading the Core MetaMetaModel")
            Call TableModel.GetModelDetails(prApplication.CMML.Core)
            prApplication.CMML.Core.Load(True, False)

            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Successfully loaded the Core Model", pcenumErrorType.Information)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadSplashScreen(ByVal asAssemblyFileVersionNumber As String)

        frmSplash.msAssemblyFileVersionNumber = asAssemblyFileVersionNumber
        frmSplash.ShowDialog()

    End Sub

    Sub SetupForm()

        Try

            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Starting to setup the Main form.", pcenumErrorType.Information)
            End If

            '--------------------------------------------
            'DockPanel
            'Set DockPanel properties
            With DockPanel
                .ActiveAutoHideContent = Nothing
                .Parent = Me
                'VS2005Style.Extender.SetSchema(DockPanel, VS2005Style.Extender.Schema.FromBase)            
                .SuspendLayout(True)
                '    If System.IO.File.Exists(configFile) Then
                '        DockPanel.LoadFromXml(configFile, AddressOf ReloadContent)
                '    Else
                '        ' Load a basic layout
                '        CreateBasicLayout()
                '    End If
                .ResumeLayout(True, True)
                .BackColor = Color.White
                .BringToFront()
            End With

            Call load_zoom_scales()

            Me.StatusBarToolStripMenuItem.Checked = My.Settings.DisplayStatusBar
            Me.StatusBar_main.Visible = My.Settings.DisplayStatusBar

            '-----------------------------------------------------------------------------
            'If the user setting for ShowEnterpriseTreeView is set to True, then 
            '  show the EnterpriseTreeView form.
            '-----------------------------------------------------------------------------
            If My.Settings.ShowModelExplorer Then
                Call Me.LoadEnterpriseTreeViewer()
                Me.MenuItem_ShowEnterpriseTreeView.Checked = True
            End If

            '------------------------------------------------------------------------------
            'Use the following code that loads a form like the Visual Studio 'Start Page'
            '  Requires WebKit.Net to be installed. Contact Victor for more details.
            '  Test of functionality currently in Viev/Product/Products/TestWebKit
            '------------------------------------------------------------------------------
            'Dim lrChildForm As New frmStartPage
            'lrChildForm.MdiParent = Me
            'lrChildForm.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

            Call Me.ShowHideMenuOptions()

            Me.ToolStrip_main.Visible = My.Settings.ShowStandardToolbar

            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Successfully setup the Main form.", pcenumErrorType.Information)
            End If

            Me.TimerNotifications.Start()

            Cursor.Current = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

    Public Sub ShowHideMenuOptions()

        Try

            If prApplication.ActivePages.Count = 0 Then
                Me.ToolStripMenuItemPage.Enabled = False
                Me.ToolStripMenuItemPage.Visible = False
                Me.ToolStripMenuItemToolbox.Enabled = False
                Me.ToolStripLabelPrompt_zoom.Enabled = False
                Me.ToolStripComboBox_zoom.Enabled = False
                Me.ToolStripMenuItemDiagramOverview.Enabled = False
                Me.ToolStripMenuItemKLTheoremWriter.Enabled = False
                Me.ToolStripMenuItemEdit.Visible = False
            Else
                Me.ToolStripMenuItemPage.Enabled = True
                Me.ToolStripMenuItemPage.Visible = True
                Me.ToolStripMenuItemToolbox.Enabled = True
                Me.ToolStripLabelPrompt_zoom.Enabled = True
                Try
                    Me.ToolStripComboBox_zoom.Enabled = True
                Catch ex As Exception
                    'Because may be trouble when ComboBoxZoom enabled. Not a biggie.
                End Try

                Me.ToolStripMenuItemDiagramOverview.Enabled = True
                Me.ToolStripMenuItemKLTheoremWriter.Enabled = True
                Me.ToolStripMenuItemEdit.Visible = True
            End If

            StandardToolStripMenuItem.Checked = My.Settings.ShowStandardToolbar

            '--------------------------------------------------------------
            'Model Explorer
            If Me.zfrmModelExplorer Is Nothing Then
                Me.SaveToolStripMenuItem.Enabled = False
                Me.SaveAllToolStripMenuItem.Enabled = False
                Me.ToolStripMenuItemNewModel.Enabled = False
            Else
                Me.SaveToolStripMenuItem.Enabled = True
                Me.SaveAllToolStripMenuItem.Enabled = True
                Me.ToolStripMenuItemNewModel.Enabled = True
            End If

            '=======================================================================
            'Unified Ontology
            Me.ToolStripMenuItemUnifiedOntology.Visible = My.Settings.ShowUnifiedOntologyBrowser

            '=======================================================================
            'Client/Server
            If My.Settings.ShowProjectUserMenuItems Then
                Me.ToolStripMenuItemUser.Visible = True
                Me.ToolStripMenuItemProject.Visible = True
            End If
            If My.Settings.UseClientServer Then
                Me.ToolStripMenuItemCodeGenerator.Enabled = My.Settings.ClientServerViewCodeGenerator
                Me.ToolStripMenuItemLogInAs.Visible = True
            End If

            '-------------------------------------------------------------------------------------------------------
            'Main Client/Server menu items
            If prApplication.User IsNot Nothing Then 'Is nothing if not using Client/Server.
                If prApplication.User.IsSuperuser Or prApplication.User.Function.Contains(pcenumFunction.FullPermission) Then
                    Me.ToolStripMenuItemUser.Visible = True
                    Me.ToolStripMenuItemProject.Visible = True
                Else
                    Me.ToolStripMenuItemOpenLogFile.Visible = False
                    Me.ToolStripMenuItemTestClientServer.Visible = False
                End If
            End If

            '=======================================================================================================
            'Do User specific processing as to what menu item are available

            'Most Restrictive
            If My.Settings.UseClientServer Then
                Me.ToolStripMenuItemRecentNodes.Visible = False
                Me.ToolStripSeparator10.Visible = False
                Me.ToolStripSeparator5.Visible = False
                Me.ToolStripMenuItemUnifiedOntologyBrowser.Visible = True
                Me.RegistrationToolStripMenuItem.Visible = False
            ElseIf Not My.Settings.UseClientServer Then
                Me.ToolStripMenuItemUser.Visible = False
                Me.ToolStripMenuItemProject.Visible = False
                Me.ToolStripMenuItemTestClientServer.Visible = False
            End If

            If Not My.Settings.ShowProjectUserMenuItems Then
                Me.ToolStripMenuItemUser.Visible = False
                Me.ToolStripMenuItemProject.Visible = False
            End If

            If My.Settings.UseClientServer And prApplication.User Is Nothing Then
                Me.ToolStripMenuItemUser.Visible = False
                Me.ToolStripMenuItemProject.Visible = False
                Me.ToolStripMenuItemTestClientServer.Visible = False
            ElseIf My.Settings.UseClientServer Then
                If Not prApplication.User.IsLoggedIn Then
                    Me.ToolStripMenuItemUser.Visible = False
                    Me.ToolStripMenuItemProject.Visible = False
                    Me.ToolStripMenuItemTestClientServer.Visible = False
                End If
            End If

            If prApplication.User IsNot Nothing Then

                'User
                If Not prApplication.User.Function.Contains(pcenumFunction.CreateUser) Then
                    Me.ToolStripMenuItemAddUser.Visible = False
                End If

                Me.ToolStripMenuItemLogInAs.Enabled = prApplication.User.IsSuperuser

                'Group 
                If Not prApplication.User.Function.Contains(pcenumFunction.CreateGroup) Then
                    Me.ToolStripMenuItemAddGroup.Visible = False
                End If


                'Role
                If Not prApplication.User.IsSuperuser Then
                    '-----------------------------------------------------------------------------------
                    'User isn't a Superuser, so can't Add or Edit Roles at all...only can view Roles within Projects etc.
                    Me.ToolStripMenuItemRole.Visible = False
                    Me.ToolStripSeparator15.Visible = False
                End If

                '================================================
                'Least Restrictive
                If prApplication.User.IsSuperuser Then
                    Me.ToolStripMenuItemAddGroup.Enabled = True
                    Me.ToolStripMenuItemBoston.Visible = True
                Else
                    Me.ToolStripMenuItemBoston.Visible = False
                End If

                'Project
                If prApplication.User.Function.Contains(pcenumFunction.ReadOwnProjects) Then
                    Me.ToolStripMenuItemProject.Visible = True
                End If

                'Group
                If prApplication.User.Function.Contains(pcenumFunction.CreateGroup) Then
                    Me.ToolStripMenuItemAddGroup.Visible = True
                End If

                If prApplication.User.Function.Contains(pcenumFunction.ReadGroup) Then
                    Me.ToolStripMenuItemUser.Visible = True
                    Me.ToolStripMenuItemEditGroup.Visible = True
                    Me.ToolStripMenuItemEditGroup.Enabled = True
                End If
            End If

            '-----------------------------------------------------------
            'Unified Ontology Browser
            Me.ToolStripMenuItemUnifiedOntologyBrowser.Visible = My.Settings.ShowUnifiedOntologyBrowser

            '------------------------------------------------------------------------------------------------------
            'Toggle LogIn/LogOut menu items
            Me.ToolStripMenuItemLogOut.Visible = My.Settings.UseClientServer Or prApplication.User IsNot Nothing
            Me.ToolStripMenuItemLogIn.Visible = False
            If Me.ToolStripMenuItemLogOut.Visible = False And Me.ToolStripMenuItemLogIn.Visible = False Then
                Me.ToolStripSeparator6.Visible = False
            End If

            'SuperUser
            If prApplication.User IsNot Nothing Then
                Me.RegistrationToolStripMenuItem.Visible = My.Settings.UseClientServer And prApplication.User.IsSuperuser
            End If
            If My.Settings.SuperuserMode Then
                Me.ToolStripMenuItemSuperuser.Visible = True
                Me.ToolStripMenuItemEditConfigurationData.Visible = True
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub ShowHideToolboxes(Optional abForceShow As Boolean = False)

        Dim ChildForm As WeifenLuo.WinFormsUI.Docking.DockContent

        Try
            If prApplication.ActivePages.Count = 0 And Not abForceShow Then

                For Each ChildForm In prApplication.RightToolboxForms.ToArray
                    ChildForm.Close()
                Next

                For Each ChildForm In prApplication.ToolboxForms.ToArray
                    ChildForm.Close()
                Next

            ElseIf prApplication.ActivePages.Count = 1 Then

                If prApplication.WorkingModel Is Nothing Then
                    Try
                        If prApplication.ActivePages.Count > 0 Then
                            Dim lrObject As Object = prApplication.ActivePages(0)
                            prApplication.WorkingModel = lrObject.zrPage.Model
                        End If
                    Catch ex As Exception
                        'Not a biggie at this stage.
                    End Try
                End If

                Call Me.loadToolboxErrorListForm(prApplication.ActivePages(0).Pane)

                Select Case prApplication.ActivePages(0).GetType
                    Case Is = GetType(frmDiagramORM)
                        Dim lrORMDiagram As frmDiagramORM
                        lrORMDiagram = prApplication.ActivePages(0)
                        Call Me.loadToolboxDescriptions(prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxRichmondBrainBox(lrORMDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxORMReadingEditor(lrORMDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxORMVerbalisationForm(lrORMDiagram.zrPage.Model, prApplication.ActivePages(0).Pane)
                        Call Me.LoadToolboxModelDictionary()
                        Call Me.LoadToolboxPropertyWindow(prApplication.ActivePages(0).Pane)
                        If IsNothing(zfrm_toolbox) And My.Settings.LoadToolboxWithPage Then
                            Call Me.LoadToolbox()
                            Dim lfrmToolboxForm As frmToolbox
                            lfrmToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
                            lfrmToolboxForm.Focus()
                            lfrmToolboxForm.ShapeListBox.Update()
                        End If
                    Case Is = GetType(frmDiagramPGS)

                        Dim lfrmPGSDiagram As frmDiagramPGS
                        lfrmPGSDiagram = prApplication.ActivePages(0)
                        Call Me.loadToolboxORMReadingEditor(lfrmPGSDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxORMVerbalisationForm(lfrmPGSDiagram.zrPage.Model, prApplication.ActivePages(0).Pane)
                        Call Me.LoadToolboxPropertyWindow(prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxRichmondBrainBox(lfrmPGSDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.LoadToolboxModelDictionary()
                        If IsNothing(zfrm_toolbox) And My.Settings.LoadToolboxWithPage Then
                            Call Me.LoadToolbox()
                            Dim lfrmToolboxForm As frmToolbox
                            lfrmToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
                            lfrmToolboxForm.Focus()
                            lfrmToolboxForm.ShapeListBox.Update()
                        End If
                    Case Is = GetType(frmDiagramERD)

                        Dim lfrmERDiagram As frmDiagramERD
                        lfrmERDiagram = prApplication.ActivePages(0)
                        Call Me.loadToolboxORMReadingEditor(lfrmERDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxORMVerbalisationForm(lfrmERDiagram.zrPage.Model, prApplication.ActivePages(0).Pane)
                        Call Me.LoadToolboxPropertyWindow(prApplication.ActivePages(0).Pane)
                        Call Me.loadToolboxRichmondBrainBox(lfrmERDiagram.zrPage, prApplication.ActivePages(0).Pane)
                        Call Me.LoadToolboxModelDictionary()

                    Case Is = GetType(frmStateTransitionDiagram)

                        Call Me.LoadToolboxPropertyWindow(prApplication.ActivePages(0).Pane)

                        If IsNothing(zfrm_toolbox) And My.Settings.LoadToolboxWithPage Then
                            Call Me.LoadToolbox()
                            Dim lfrmToolboxForm As frmToolbox
                            lfrmToolboxForm = prApplication.GetToolboxForm(frmToolbox.Name)
                            lfrmToolboxForm.Focus()

                            lfrmToolboxForm.ShapeListBox.Shapes = MindFusion.Diagramming.ShapeLibrary.LoadFrom(My.Settings.StateTransitionShapeLibrary).Shapes

                            lfrmToolboxForm.ShapeListBox.Update()
                        End If

                End Select
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub frm_main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Dim lfrm_form As System.Windows.Forms.Form

        Dim lrPropertyGridForm As frmToolboxProperties
        lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
        If lrPropertyGridForm IsNot Nothing Then
            lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'So that it doesn't trigger an event on a 'Instance' that doesn't exist.
        End If

        For Each lfrm_form In Me.MdiChildren
            lfrm_form.Close()
            If pbCancelClosing Then
                e.Cancel = True
                pbCancelClosing = False
                Exit Sub
            End If
        Next

        '===========================================================================================================
        'Client/Server
        If My.Settings.UseClientServer And (prDuplexServiceClient IsNot Nothing) Then
            Try
                prDuplexServiceClient.Disconnect()
                prDuplexServiceClient.Abort()
                '_proxy.Close() 'Close sometimes just hang there. Abort seems to work better/just as well.
            Catch
                prDuplexServiceClient.Abort()
            End Try
        End If

        Try
            If IsSomething(Me.DockPanel) Then
                Me.DockPanel.Dispose()
            End If
        Catch
        End Try

        If My.Settings.UseClientServer Then
            Dim lrLogEntry As New ClientServer.Log
            lrLogEntry.DateTime = Now
            lrLogEntry.User = prApplication.User
            lrLogEntry.LogType = pcenumLogType.LogOut
            If My.Settings.UseVirtualUI Then
                lrLogEntry.IPAddress = prThinfinity.BrowserInfo.IPAddress
            Else
                lrLogEntry.IPAddress = "NOTHING"
            End If

            If prApplication.User IsNot Nothing Then
                Call tableClientServerLog.AddLogEntry(lrLogEntry)
            Else
                '---------------------------------------------------------------
                'User likely [Cancelled] logging into Boston, in which case there is no User to log details for.
            End If
        End If

        Try
            Call Environment.Exit(0)
        Catch ex As Exception
        End Try

    End Sub

    Sub load_zoom_scales()

        Try
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(25, "25%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(50, "50%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(75, "75%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(80, "80%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(90, "90%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(100, "100%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(150, "150%"))
            ToolStripComboBox_zoom.Items.Add(New tComboboxItem(200, "200%"))

            ToolStripComboBox_zoom.SelectedIndex = 5

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub mnuOption_EndSession_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_EndSession.Click

        Try

            If IsSomething(zfrmModelExplorer) Then
                Me.zfrmModelExplorer.zoRecentNodes.Serialize(Me.zfrmModelExplorer.zsRecentNodesFileName)
            End If

            Dim larPage = From Model In prApplication.Models
                          From Page In Model.Page
                          Where Page.IsDirty = True
                          Select Page

            For Each lrPage In larPage

                Dim lsMessage As String = ""

                lsMessage = "Changes have been made to the Page, '" & lrPage.Name & "', in model, '" & lrPage.Model.Name & "'."
                lsMessage.AppendString(vbCrLf & vbCrLf)
                lsMessage.AppendString(" Would you like to save those changes?")

                Select Case MsgBox(lsMessage, MsgBoxStyle.YesNoCancel)
                    Case Is = MsgBoxResult.Yes
                        Using lrWaitCursor As New WaitCursor
                            lrPage.Save()
                        End Using
                    Case Is = MsgBoxResult.No
                        lrPage.UserRejectedSave = True
                    Case Is = MsgBoxResult.Cancel
                        pbCancelClosing = True
                        Exit Sub
                End Select
            Next

            For Each lrForm In prApplication.RightToolboxForms.ToArray
                Call lrForm.Close()
            Next

            Me.Close()

            Try
                pdbConnection.Close()
            Catch ex As Exception
                'Not a biggie.
            End Try

            Application.Exit()
            Environment.Exit(0)

        Catch err As Exception
            MsgBox(err.Message)
        End Try


    End Sub

    Sub load_diagram_overview_form()

        Try

            Dim child As New frmDiagramOverview

            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                Call child.Show()
            Else
                child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                prApplication.RightToolboxForms.Add(child)
                zfrm_diagram_overview = child
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub loadToolboxDescriptions(ByRef aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane)

        Dim child As New frmToolboxDescriptions

        Try
            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                child.BringToFront()
            Else
                '----------------------------------------------------------
                'Create a new instance of the FactTypeReadingEditor form.
                '----------------------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------------------
                    'Add the FactTypeReadingEditor form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------------------    
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane

                    prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                    prApplication.ToolboxForms.Add(child)
                Else
                    '--------------------------------------------------
                    'Add the ORMReadingEditor form to the bottom of the Page
                    '--------------------------------------------------
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)

                End If
                prApplication.ToolboxForms.Add(child)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Sub loadToolboxErrorListForm(ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane)

        Dim child As New frmToolboxErrorList

        Try
            child.zrModel = prApplication.WorkingModel
            child.mrApplication = prApplication

            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                child.zrModel = prApplication.WorkingModel
                child.BringToFront()
                Call child.SetupForm()
            Else
                '----------------------------------------------
                'Create a new instance of the ErrorList form.
                '----------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------   
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane

                    prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane
                    child.MdiParent = Me
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    prApplication.ToolboxForms.Add(child)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Sub loadToolboxIndexEditor(ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane)

        Dim child As New frmToolboxIndexEditor

        Try

            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                child.BringToFront()
                Call child.SetupForm()
            Else
                '----------------------------------------------
                'Create a new instance of the ErrorList form.
                '----------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------   
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane

                    prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane
                    child.MdiParent = Me
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                    prApplication.ToolboxForms.AddUnique(child)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    prApplication.ToolboxForms.Add(child)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Sub loadToolboxCypher(ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane)

        Dim child As New frmToolboxCypher
        child.zrModel = prApplication.WorkingModel

        If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
            '-------------------------------------------------------------
            'Form is already loaded. Bring it to the front of the ZOrder
            '-------------------------------------------------------------            
            child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
            child.zrModel = prApplication.WorkingModel
            child.BringToFront()
            Call child.SetupForm()
        Else
            '----------------------------------------------
            'Create a new instance of the ErrorList form.
            '----------------------------------------------
            If prApplication.ToolboxForms.Count > 0 Then
                '----------------------------------------------------------------------------------------
                'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                '----------------------------------------------------------------------------------------   
                Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane

                prApplication.ToolboxForms(0).Focus()
                lrPane = prApplication.ToolboxForms(0).Pane
                child.MdiParent = Me
                child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.4)
            Else
                '--------------------------------------------------
                'Add the ErrorList form to the bottom of the Page
                '--------------------------------------------------
                child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.4)
                prApplication.ToolboxForms.Add(child)
            End If
        End If


    End Sub

    Private Sub LoadCRUDRichmondConfiguration()

        Dim child As New frmCRUDBostonConfiguration

        child.MdiParent = Me

        zfrmCRUDBostonConfiguration = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDAddGroup()

        Dim child As New frmCRUDAddGroup

        child.MdiParent = Me

        zfrmCRUDAddGroup = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDAddProject()

        Dim child As New frmCRUDAddProject

        child.MdiParent = Me

        zfrmCRUDAddProject = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDAddNamespace()

        Dim child As New frmCRUDAddNamespace

        child.MdiParent = Me

        zfrmCRUDAddNamespace = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDAddRole()

        Dim child As New frmCRUDAddRole

        child.MdiParent = Me

        zfrmCRUDAddRole = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub


    Private Sub LoadCRUDAddUser()

        Dim child As New frmCRUDAddUser

        child.MdiParent = Me

        zfrmCRUDAddUser = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Public Sub LoadCRUDEditGroup(ByRef arGroup As ClientServer.Group)

        Dim child As New frmCRUDEditGroup

        child.MdiParent = Me

        child.zrGroup = arGroup
        zfrmCRUDEditGroup = child


        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDEditNamespace(ByRef arNamespace As ClientServer.Namespace)

        Dim child As New frmCRUDEditNamespace

        child.zrNamespace = arNamespace
        child.MdiParent = Me

        zfrmCRUDEditNamespace = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDEditProject(ByRef arProject As ClientServer.Project)

        Dim child As New frmCRUDEditProject

        child.zrProject = arProject
        child.MdiParent = Me

        zfrmCRUDEditProject = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDEditRole(ByRef arRole As ClientServer.Role)

        Dim child As New frmCRUDEditRole

        child.zrRole = arRole
        child.MdiParent = Me

        zfrmCRUDEditRole = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub LoadCRUDEditUser(ByRef arUser As ClientServer.User)

        Dim child As New frmCRUDEditUser

        child.zrUser = arUser
        child.MdiParent = Me

        zfrmCRUDEditUser = child

        Me.Cursor = Cursors.WaitCursor
        child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.Cursor = Cursors.Default

    End Sub

    Public Function LoadToolboxModelDictionary(Optional abRefreshModelDictionary As Boolean = False) As frmToolboxModelDictionary

        Dim child As New frmToolboxModelDictionary

        Try
            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                Call child.Show()

                If abRefreshModelDictionary Then
                    Call child.SetupForm()
                End If
            Else
                '----------------------------------------------
                'Create a new instance of the ModelDictionary form.
                '----------------------------------------------
                If prApplication.RightToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)

                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                End If
                prApplication.RightToolboxForms.Add(child)
            End If

            Return child

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Sub focusFactTypeReadingToolbox()

        Try
            Dim child As frmToolboxORMReadingEditor = prApplication.ToolboxForms.Find(Function(x) x.Name = frmToolboxORMReadingEditor.Name)
            '-------------------------------------------------------------
            'Form is already loaded. Bring it to the front of the ZOrder
            '-------------------------------------------------------------
            If child IsNot Nothing Then
                Call child.Show()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Sub focusVerbalisationToolbox()

        Try
            Dim child As frmToolboxORMVerbalisation = prApplication.ToolboxForms.Find(Function(x) x.Name = frmToolboxORMVerbalisation.Name)
            If child IsNot Nothing Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------                            
                Call child.Show()
            End If


        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub


    Public Sub load_page(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        If IsSomething(zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            Me.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = lr_enterprise_view.Tag
            Call Me.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)
        End If

    End Sub


    Public Sub LoadEnterpriseTreeViewer()

        Try
            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Starting to load the Model Explorer", pcenumErrorType.Information)
            End If

            Dim child As New frmToolboxEnterpriseExplorer

            child.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft)

            Me.zfrmModelExplorer = child

            If pbLogStartup Then
                prApplication.ThrowErrorMessage("Successfully loaded the Model Explorer", pcenumErrorType.Information)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub LoadFactEngine()

        Try
            Dim child As New frmFactEngine

            If Me.zfrmFactEngine Is Nothing Then
                child.Show(DockPanel)
                Me.zfrmFactEngine = child
            Else
                If Not Me.zfrmFactEngine.IsDisposed Then
                    Me.zfrmFactEngine.BringToFront()
                Else
                    Me.zfrmFactEngine = New frmFactEngine
                    Me.zfrmFactEngine.Show(Me.DockPanel)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub LoadKeywordExtractionTool(ByRef arModel As FBM.Model)

        Dim child As New frmKeywordExtraction

        child.MdiParent = Me

        child.mrModel = arModel

        child.Show(Me.DockPanel)

    End Sub

    Public Sub LoadFEKLUploaderTool(ByRef arModel As FBM.Model)

        Dim child As New frmFEKLUploader

        Try

            child.MdiParent = Me

            child.mrModel = arModel

            child.Show(Me.DockPanel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        End Try

    End Sub

    Public Sub LoadToolboxAIPretrainingDataEditor(ByRef arModel As FBM.Model)

        Dim lsMessage As String

        Try
            Dim lrModel = arModel

            If My.Settings.FactEngineUseGPT3 Then
#Region "GPT3 Transforms"
                Dim loTransformation As Object = New System.Dynamic.ExpandoObject
                Dim larTransformationTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(36, loTransformation)

                Dim lsGPT3TrainingExamplesFilePath = larTransformationTuples.Where(Function(x) x.ModelId = lrModel.ModelId).Select(Function(x) x.GPT3TrainingFileLocation)(0)

                If Not System.IO.File.Exists(lsGPT3TrainingExamplesFilePath) Or lsGPT3TrainingExamplesFilePath Is Nothing Then

                    lsMessage = "Please check the file path set up for your AI pretraining data."
                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,,,)
                    Exit Sub
                End If
#End Region
            Else
                lsMessage = "Please check that your instance of Boston/FactEngine is set up for natural language queries using AI. Closing."
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,,,)
                Exit Sub
            End If


            '==================================================================================
            Dim child As New frmToolboxAIPretrainingDataEditor

            child.MdiParent = Me

            child.mrModel = arModel

            child.Show(Me.DockPanel)

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub LoadToolboxTaxonomyTree(ByRef arModel As FBM.Model)

        Try
            Dim child As New frmToolboxTaxonomyTree

            child.MdiParent = Me

            child.mrModel = arModel

            child.Show(Me.DockPanel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub LoadCRUDModel(ByRef arModel As FBM.Model)

        Dim child As New frmCRUDModel

        Try

            child.MdiParent = Me

            child.zrModel = arModel

            child.Show(Me.DockPanel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub LoadFixModelErrorsForm(ByRef arModel As FBM.Model)

        Dim child As New frmFixModelErrors

        child.MdiParent = Me

        child.mrModel = arModel

        child.Show(Me.DockPanel)

    End Sub

    Public Function loadERDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode) As Object

        Dim child As New frmDiagramERD

        child.MdiParent = Me

        zfrm_ER_diagram_view = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a 
        '  Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the 
        '  ZOrder of that form to OnTop.
        '---------------------------------------------------------------        
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView
        child.zrPage = arPage

        Call Me.ShowHideToolboxes(True)

        '---------------------------------------------------------------
        'Setup the 'Page' (i.e. Load the Event Trace Diagram)
        '---------------------------------------------------------------        
        Call child.LoadERDDiagramPage(arPage, ao_tree_node)



        Return child

    End Function

    Public Function loadPGSDiagramView(ByRef arPage As FBM.Page,
                                          ByVal ao_tree_node As TreeNode,
                                          Optional ByVal asSelectModelElementId As String = Nothing,
                                          Optional ByVal abLoadToolboxes As Boolean = True) As Object

        Dim child As New frmDiagramPGS

        child.MdiParent = Me

        zfrm_PGS_diagram_view = child

        'Model Check Errors...so that Node Types with errors show red
        Call arPage.Model.checkForErrors()

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a 
        '  Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the 
        '  ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Setup the 'Page' (i.e. Load the Event Trace Diagram)
        '---------------------------------------------------------------        
        Call child.loadPGSDiagramPage(arPage, ao_tree_node, asSelectModelElementId)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
            Call Me.ShowHideMenuOptions()
        End If

        Return child

    End Function

    Sub load_StateTransitionDiagram_view(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmStateTransitionDiagram

        child.MdiParent = Me

        zfrmStateTransitionDiagramView = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a 
        '  Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the 
        '  ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView

        '---------------------------------------------------------------
        'Setup the 'Page' title details
        '---------------------------------------------------------------        
        Call child.load_StateTransitionDiagram(arPage, ao_tree_node)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
        End If

    End Sub

    Sub loadBPMNProcessDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmDiagrmUMLUseCase

        child.MdiParent = Me

        zfrmUMLUseCaseDiagramView = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView

        '---------------------------------------------------------------
        'Setup the 'Page' title details
        '---------------------------------------------------------------        
        Call child.load_use_case_page(arPage, ao_tree_node)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
        End If

    End Sub


    Sub loadBPMNConversationDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmDiagrmUMLUseCase

        child.MdiParent = Me

        zfrmUMLUseCaseDiagramView = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView

        '---------------------------------------------------------------
        'Setup the 'Page' title details
        '---------------------------------------------------------------        
        Call child.load_use_case_page(arPage, ao_tree_node)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
        End If

    End Sub


    Sub loadBPMNCollaborationDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmDiagramBPMNCollaboration

        Try

            child.MdiParent = Me

            '----------------------------------------------------
            'Set the TreeNode from which the form was launched,
            '  so that when the User clicks on the Form, the
            '  respective TreeNode in the navigation tree can
            '  be selected/expanded etc
            '----------------------------------------------------
            child.zoTreeNode = ao_tree_node
            child.zrPage = arPage

            child.Show(DockPanel)

            '---------------------------------------------------------------
            'Reference the Form back from the Page.
            '  The reason for this is because if the User elects to Edit a Page that is already opened for editing, then it is very easy
            '  to find the form that the page is displayed as to set the ZOrder of that form to OnTop.
            '---------------------------------------------------------------
            arPage.Form = New Windows.Forms.Form
            arPage.Form = child
            arPage.ReferencedForm = child
            arPage.Diagram = child.Diagram
            arPage.DiagramView = child.DiagramView

            '---------------------------------------------------------------
            'Setup the 'Page' title details
            '---------------------------------------------------------------        
            Call child.LoadBPMNCollaborationDiagramPage(arPage)

            If abLoadToolboxes Then
                Call Me.ShowHideToolboxes(True)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Sub loadBPMNChoreographyDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmDiagrmUMLUseCase

        child.MdiParent = Me

        zfrmUMLUseCaseDiagramView = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView

        '---------------------------------------------------------------
        'Setup the 'Page' title details
        '---------------------------------------------------------------        
        Call child.load_use_case_page(arPage, ao_tree_node)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
        End If

    End Sub


    Sub loadUMLUseCaseDiagramView(ByRef arPage As FBM.Page, ByVal ao_tree_node As TreeNode, Optional ByVal abLoadToolboxes As Boolean = False)

        Dim child As New frmDiagrmUMLUseCase

        child.MdiParent = Me

        zfrmUMLUseCaseDiagramView = child

        '----------------------------------------------------
        'Set the TreeNode from which the form was launched,
        '  so that when the User clicks on the Form, the
        '  respective TreeNode in the navigation tree can
        '  be selected/expanded etc
        '----------------------------------------------------
        child.zoTreeNode = ao_tree_node
        child.zrPage = arPage

        child.Show(DockPanel)

        '---------------------------------------------------------------
        'Reference the Form back from the Page.
        '  The reason for this is because if the User elects to Edit a Page that is already opened for editing, then it is very easy
        '  to find the form that the page is displayed as to set the ZOrder of that form to OnTop.
        '---------------------------------------------------------------
        arPage.Form = New Windows.Forms.Form
        arPage.Form = child
        arPage.ReferencedForm = child
        arPage.Diagram = child.Diagram
        arPage.DiagramView = child.DiagramView

        '---------------------------------------------------------------
        'Setup the 'Page' title details
        '---------------------------------------------------------------        
        Call child.load_use_case_page(arPage, ao_tree_node)

        If abLoadToolboxes Then
            Call Me.ShowHideToolboxes(True)
        End If

    End Sub


    ''' <summary>
    ''' Loads a frmDiagramORM.frm as the DiagramSpy.
    ''' </summary>
    ''' <param name="arPage"></param>
    ''' <remarks></remarks>
    Public Function LoadDiagramSpy(ByRef arPage As FBM.Page,
                              ByRef arFocalModelObject As FBM.ModelObject,
                              Optional ByVal abControlKeyPressed As Boolean = False) As FBM.ModelObject

        Dim child As New frmDiagramORM

        Try

            prApplication.WorkingModel = arPage.Model

            child.MdiParent = Me
            zrORMModel_view = child

            If prApplication.ActivePages.Count > 0 Then
                child.Show(Me.DockPanel)
                child.DockTo(prApplication.ActivePages(0).Pane, DockStyle.Fill, 0)
            Else
                child.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
            End If


            '---------------------------------------------------------------
            'Reference the Form back from the Page.
            '  The reason for this is because if the User elects to Edit a 
            '  Page that is already opened for editing, then it is very easy
            '  to find the form that the page is displayed as to set the 
            '  ZOrder of that form to OnTop.
            '---------------------------------------------------------------
            arPage.Form = New Windows.Forms.Form
            arPage.Form = child
            arPage.ReferencedForm = child
            arPage.Diagram = child.Diagram
            arPage.DiagramView = child.DiagramView

            '-----------------
            'Display the Page
            '-----------------
            Call child.LoadORMModelPage(arPage)

            If arFocalModelObject Is Nothing Then
                '--------------------------------------
                'Nothing to load but the form itself.
                '--------------------------------------
            Else
                Dim loPt As New PointF(200, 100)
                Dim lrFocalModelElementInstance As FBM.ModelObject = Nothing

                Select Case arFocalModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Dim lrValueType As FBM.ValueType
                        lrValueType = arFocalModelObject
                        lrFocalModelElementInstance = child.zrPage.DropValueTypeAtPoint(lrValueType, loPt)
                        If Not abControlKeyPressed Then Call child.LoadAssociatedFactTypes(lrValueType)
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityType As FBM.EntityType
                        lrEntityType = arFocalModelObject
                        lrFocalModelElementInstance = child.zrPage.DropEntityTypeAtPoint(lrEntityType, loPt)
                        If Not abControlKeyPressed Then Call child.LoadAssociatedFactTypes(lrEntityType)
                        If lrEntityType.HasCompoundReferenceMode Then
                            Call child.zrPage.DropRoleConstraintAtPoint(lrEntityType.ReferenceModeRoleConstraint, New PointF(10, 10), False)
                        End If
                    Case Is = pcenumConceptType.FactType
                        Dim lrFactType As FBM.FactType
                        lrFactType = arFocalModelObject
                        lrFocalModelElementInstance = child.zrPage.DropFactTypeAtPoint(lrFactType, loPt, False, False, False, True)
                        If Not abControlKeyPressed Then Call child.LoadAssociatedFactTypes(lrFactType)
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraint As FBM.RoleConstraint

                        lrRoleConstraint = arFocalModelObject

                        Select Case lrRoleConstraint.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                lrFocalModelElementInstance = arPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Case Is = pcenumRoleConstraintType.RingConstraint,
                                      pcenumRoleConstraintType.EqualityConstraint,
                                      pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                      pcenumRoleConstraintType.ExclusiveORConstraint,
                                      pcenumRoleConstraintType.ExclusionConstraint,
                                      pcenumRoleConstraintType.SubsetConstraint
                                lrFocalModelElementInstance = arPage.DropRoleConstraintAtPoint(lrRoleConstraint, loPt)
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                lrFocalModelElementInstance = arPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, loPt)
                        End Select
                End Select

                Select Case lrFocalModelElementInstance.GetType
                    Case Is = GetType(FBM.FactTypeInstance)
                        Call child.AutoLayout(lrFocalModelElementInstance)
                    Case Else
                        Call child.AutoLayout()
                End Select

                Return lrFocalModelElementInstance
            End If

            Call Me.ShowHideToolboxes()

            Return Nothing

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    ''' <summary>
    '''   * See also: Me.LoadDiagramSpy
    ''' </summary>
    ''' <param name="arPage"></param>
    ''' <param name="aoTreeNode">The TreeView.Node in the Model Explorer corresponding to the Page being loaded.</param>
    ''' <remarks></remarks>
    Public Function loadORMModelPage(ByRef arPage As FBM.Page,
                                     ByRef aoTreeNode As TreeNode,
                                     Optional ByVal abLoadToolboxes As Boolean = True) As Object

        Dim child As New frmDiagramORM

        Try
            prApplication.WorkingModel = arPage.Model

            prApplication.ThrowErrorMessage("Opening the Toolbox", pcenumErrorType.Information)

            child.MdiParent = Me

            Call arPage.Model.checkIfCanCheckForErrors()

            zrORMModel_view = child

            '----------------------------------------------------
            'Set the TreeNode from which the form was launched,
            '  so that when the User clicks on the Form, the
            '  respective TreeNode in the navigation tree can
            '  be selected/expanded etc
            '----------------------------------------------------
            child.zoTreeNode = aoTreeNode

            prApplication.ThrowErrorMessage("Showing the ORM Diagram Form", pcenumErrorType.Information)

            child.Show(DockPanel)

            '---------------------------------------------------------------
            'Reference the Form back from the Page.
            '  The reason for this is because if the User elects to Edit a 
            '  Page that is already opened for editing, then it is very easy
            '  to find the form that the page is displayed as to set the 
            '  ZOrder of that form to OnTop.
            '---------------------------------------------------------------
            arPage.Form = New Windows.Forms.Form
            arPage.Form = child
            arPage.ReferencedForm = child
            arPage.Diagram = child.Diagram
            arPage.DiagramView = child.DiagramView

            '--------------------------------------------------------------------
            'If the Page IsDirty (i.e. Is a 'New' page), enable the Save [button]
            '--------------------------------------------------------------------
            If arPage.IsDirty Then
                Me.ToolStripButton_Save.Enabled = True
            End If

            '-----------------
            'Display the Page
            '-----------------
            prApplication.ThrowErrorMessage("About to load the ORM Model Page", pcenumErrorType.Information)


            Call child.LoadORMModelPage(arPage)

            If abLoadToolboxes Then
                Call Me.ShowHideToolboxes(True)
            End If


            Try
                If My.Settings.UseClientServer Then
                    If Not prApplication.User.ProjectPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                        Dim lfrmFlashCard As New frmFlashCard
                        lfrmFlashCard.ziIntervalMilliseconds = 3500
                        lfrmFlashCard.zsText = "Please note that you do not have Alter Permission on this Project."
                        Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)
                    End If
                End If
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Failed Here: Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

            child.Focus()

            Return child

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    ''' <summary>
    '''   * See also: Me.LoadDiagramSpy
    ''' </summary>
    ''' <param name="arPage"></param>
    ''' <param name="aoTreeNode">The TreeView.Node in the Model Explorer corresponding to the Page being loaded.</param>
    ''' <remarks></remarks>
    Public Function loadOntologyORMModelPage(ByRef arPage As FBM.Page,
                                             ByRef aoTreeNode As TreeNode,
                                             Optional ByVal abLoadToolboxes As Boolean = True) As Object

        Dim child As New frmDiagramORMForOntologyBrowser

        Try
            prApplication.WorkingModel = arPage.Model

            prApplication.ThrowErrorMessage("Opening the Toolbox", pcenumErrorType.Information)

            child.MdiParent = Me

            zfrmOntologyORMModelView = child

            '----------------------------------------------------
            'Set the TreeNode from which the form was launched,
            '  so that when the User clicks on the Form, the
            '  respective TreeNode in the navigation tree can
            '  be selected/expanded etc
            '----------------------------------------------------
            child.zoTreeNode = aoTreeNode

            prApplication.ThrowErrorMessage("Showing the ORM Diagram Form", pcenumErrorType.Information)

            child.Show(DockPanel)

            '---------------------------------------------------------------
            'Reference the Form back from the Page.
            '  The reason for this is because if the User elects to Edit a 
            '  Page that is already opened for editing, then it is very easy
            '  to find the form that the page is displayed as to set the 
            '  ZOrder of that form to OnTop.
            '---------------------------------------------------------------
            arPage.Form = New Windows.Forms.Form
            arPage.Form = child
            arPage.ReferencedForm = child
            arPage.Diagram = child.Diagram
            arPage.DiagramView = child.DiagramView

            '--------------------------------------------------------------------
            'If the Page IsDirty (i.e. Is a 'New' page), enable the Save [button]
            '--------------------------------------------------------------------
            If arPage.IsDirty Then
                Me.ToolStripButton_Save.Enabled = True
            End If

            '-----------------
            'Display the Page
            '-----------------
            prApplication.ThrowErrorMessage("About to load the ORM Model Page", pcenumErrorType.Information)


            Call child.DisplayORMModelPage(arPage)

            If abLoadToolboxes Then
                Call Me.ShowHideToolboxes(True)
            End If


            Try
                If My.Settings.UseClientServer Then
                    If Not prApplication.User.ProjectPermission.FindAll(Function(x) x.Permission = pcenumPermission.Alter).Count > 0 Then
                        Dim lfrmFlashCard As New frmFlashCard
                        lfrmFlashCard.ziIntervalMilliseconds = 3500
                        lfrmFlashCard.zsText = "Please note that you do not have Alter Permission on this Project."
                        Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)
                    End If
                End If
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Failed Here: Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

            child.Focus()

            Return child

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub loadCodeGenerator()

        If zfrmCodeGenerator Is Nothing Then
            zfrmCodeGenerator = UI.MainForm
            Call zfrmCodeGenerator.Show(DockPanel)
        End If

    End Sub

    ''' <summary>
    ''' Loads the Glossary form within the main DockPanel
    ''' </summary>
    ''' <remarks></remarks>
    Sub LoadUnifiedOntologyBrowser(ByRef arUnifedOntology As Ontology.UnifiedOntology,
                                   Optional arModelElement As FBM.ModelObject = Nothing)

        Dim child As New frmUnifiedOntologyBrowser

        Try
            With New WaitCursor

                'Close the StartupPage
                If Me.zfrmStartup IsNot Nothing Then
                    Me.zfrmStartup.Close()
                    Me.zfrmStartup = Nothing
                End If

                If Me.zfrmModelExplorer IsNot Nothing Then
                    Me.zfrmModelExplorer.VisibleState = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide
                End If

                child.zrUnifiedOntology = arUnifedOntology
                child.Show(DockPanel)

                If arModelElement IsNot Nothing Then
                    child.FocusModelElement(arModelElement)
                    child.DescribeModelElement(arModelElement)
                End If

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Sub LoadDataLineageForm(Optional arModelElement As FBM.ModelObject = Nothing)

        Dim child As New frmDataLineage

        Try
            Me.Cursor = Cursors.WaitCursor

            child.mrModelElement = arModelElement

            child.Show(DockPanel)

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    ''' <summary>
    ''' Loads the Glossary form within the main DockPanel
    ''' </summary>
    ''' <remarks></remarks>
    Sub LoadGlossaryForm(Optional arModelElement As FBM.ModelObject = Nothing)

        Dim child As New frmGlossary

        Try
            Me.Cursor = Cursors.WaitCursor

            child.Show(DockPanel)

            If arModelElement IsNot Nothing Then
                child.DescribeModelElement(arModelElement)
                child.FocusModelElement(arModelElement)
            End If

            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function LoadToolboxPropertyWindow(ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane,
                                              Optional ByRef arModelElement As FBM.ModelObject = Nothing) As frmToolboxProperties

        Try
            Dim child As New frmToolboxProperties

            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            

                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)

                child.Show()
                'child.BringToFront()
                'child.Pane.BringToFront()
                'child.Close()
                'child = New frmToolboxProperties
                'child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                'prApplication.RightToolboxForms.Add(child)
            Else
                '----------------------------------------------
                'Create a new instance of the Properties form.
                '----------------------------------------------
                If prApplication.RightToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the PropertyGrid form to DockPanel
                    '----------------------------------------------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                    'Else
                    '--------------------------------------------------
                    'Add the PropertyGrid form to the DockPanel
                    '--------------------------------------------------                
                    'child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                Else
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                End If
                prApplication.RightToolboxForms.Add(child)
            End If

#Region "ModelElement"
            If arModelElement IsNot Nothing Then

                Dim lrPropertyGridForm As frmToolboxProperties
                lrPropertyGridForm = child
                Dim lrModelObject As FBM.ModelObject = arModelElement

                lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Dim lrValueTypeInstance As FBM.ValueTypeInstance
                        lrValueTypeInstance = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                        Select Case lrValueTypeInstance.DataType
                            Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                              pcenumORMDataType.NumericDecimal,
                                              pcenumORMDataType.NumericMoney
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            Case Is = pcenumORMDataType.RawDataFixedLength,
                                              pcenumORMDataType.RawDataLargeLength,
                                              pcenumORMDataType.RawDataVariableLength,
                                              pcenumORMDataType.TextFixedLength,
                                              pcenumORMDataType.TextLargeLength,
                                              pcenumORMDataType.TextVariableLength
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Case Else
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrValueTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                        End Select
                        If lrPropertyGridForm.PropertyGrid.SelectedObject IsNot Nothing Then
                            lrPropertyGridForm.PropertyGrid.SelectedObject = New Object
                        End If
                        lrPropertyGridForm.zrSelectedObject = lrValueTypeInstance
                        lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrValueTypeInstance
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                        lrEntityTypeInstance = lrModelObject
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                        Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DerivationText", True)
                        lrPropertyGridForm.zrSelectedObject = lrEntityTypeInstance
                        If lrEntityTypeInstance.EntityType.HasSimpleReferenceScheme Then
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                            Select Case lrEntityTypeInstance.DataType
                                Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                                  pcenumORMDataType.NumericDecimal,
                                                  pcenumORMDataType.NumericMoney
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Case Is = pcenumORMDataType.RawDataFixedLength,
                                                  pcenumORMDataType.RawDataLargeLength,
                                                  pcenumORMDataType.RawDataVariableLength,
                                                  pcenumORMDataType.TextFixedLength,
                                                  pcenumORMDataType.TextLargeLength,
                                                  pcenumORMDataType.TextVariableLength
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Case Else
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End Select
                        Else
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                            Call lrEntityTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                        End If
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrEntityTypeInstance
                        lrPropertyGridForm.PropertyGrid.Refresh()

                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                        lrRoleConstraintInstance = lrModelObject

                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})

                        lrPropertyGridForm.zrSelectedObject = lrModelObject
                        lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.

                        Select Case lrRoleConstraintInstance.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                Dim lrFrequencyConstraintInstance As FBM.FrequencyConstraint
                                lrFrequencyConstraintInstance = lrModelObject
                                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("Comparitor")
                                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                                Dim loMiscFilterAttribute5 As Attribute = New System.ComponentModel.CategoryAttribute("Value Constraint")
                                Dim loMiscFilterAttribute6 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4, loMiscFilterAttribute5, loMiscFilterAttribute6})

                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrFrequencyConstraintInstance
                            Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                Dim lrRoleValueConstraintInstance As FBM.RoleValueConstraint
                                lrRoleValueConstraintInstance = lrModelObject
                                lrPropertyGridForm.zrSelectedObject = lrRoleValueConstraintInstance
                                lrPropertyGridForm.PropertyGrid.SelectedObjects = {} 'Part of the fix to the problem where ValueConstraint were being added to the wrong ValueType.
                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleValueConstraintInstance
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                Dim lrRingConstraintInstance As FBM.RingConstraint
                                lrRingConstraintInstance = lrModelObject
                                Dim loMiscFilterAttribute3 As Attribute = New System.ComponentModel.CategoryAttribute("Comparitor")
                                Dim loMiscFilterAttribute4 As Attribute = New System.ComponentModel.CategoryAttribute("DBName")
                                Dim loMiscFilterAttribute5 As Attribute = New System.ComponentModel.CategoryAttribute("Value Constraint")
                                Dim loMiscFilterAttribute6 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2, loMiscFilterAttribute3, loMiscFilterAttribute4, loMiscFilterAttribute5, loMiscFilterAttribute6})

                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRingConstraintInstance
                            Case Else
                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrRoleConstraintInstance
                        End Select


                    Case Is = pcenumConceptType.FactType
                        Dim lrFactTypeInstance = CType(lrModelObject, FBM.FactTypeInstance)

                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "ReferenceMode", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", lrFactTypeInstance.IsObjectified)
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", lrFactTypeInstance.IsObjectified)
                        If lrFactTypeInstance.IsObjectified Then

                            'CodeSafe 
                            If lrFactTypeInstance.ObjectifyingEntityType Is Nothing Then
                                'Try and find the ObjectifyingEntityType
                                lrFactTypeInstance.ObjectifyingEntityType = lrFactTypeInstance.ObjectifyingEntityType.CloneInstance(New FBM.Page(lrModelObject.Model, Nothing, "DummyPage", pcenumLanguage.ORMModel), False)
                            End If


                            If lrFactTypeInstance.ObjectifyingEntityType.EntityType.HasSimpleReferenceScheme Then
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", True)
                                Select Case lrFactTypeInstance.ObjectifyingEntityType.DataType
                                    Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                                  pcenumORMDataType.NumericDecimal,
                                                  pcenumORMDataType.NumericMoney
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                    Case Is = pcenumORMDataType.RawDataFixedLength,
                                                  pcenumORMDataType.RawDataLargeLength,
                                                  pcenumORMDataType.RawDataVariableLength,
                                                  pcenumORMDataType.TextFixedLength,
                                                  pcenumORMDataType.TextLargeLength,
                                                  pcenumORMDataType.TextVariableLength
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", True)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Case Else
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                                End Select
                            Else
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataType", False)
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Call lrFactTypeInstance.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End If
                        End If

                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        Call lrFactTypeInstance.SetPropertyAttributes(Me, "DerivationText", True)
                        If lrPropertyGridForm.PropertyGrid.SelectedObject IsNot Nothing Then
                            lrPropertyGridForm.PropertyGrid.SelectedObject = New Object
                        End If
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject

                    Case Else
                        Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                        Dim loMiscFilterAttribute2 As Attribute = New System.ComponentModel.CategoryAttribute("Instances")
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute, loMiscFilterAttribute2})
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                End Select

            End If
#End Region

            Return child

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function



    Public Function loadToolboxORMReadingEditor(ByRef arPage As FBM.Page,
                                        ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane,
                                        Optional ByRef arFactTypeInstance As FBM.FactTypeInstance = Nothing) As frmToolboxORMReadingEditor

        Dim child As New frmToolboxORMReadingEditor

        Try
            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                child.zrPage = arPage
                child.BringToFront()

                Return child
            Else
                '----------------------------------------------------------
                'Create a new instance of the FactTypeReadingEditor form.
                '----------------------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------------------
                    'Add the FactTypeReadingEditor form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------------------    
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane

                    prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane

                    child = New frmToolboxORMReadingEditor
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)

                    child.DockTo(lrPane, DockStyle.Fill, 0)

                    child.zrPage = arPage

                    If arFactTypeInstance IsNot Nothing Then
                        child.zrFactTypeInstance = arFactTypeInstance
                        child.zrFactType = arFactTypeInstance.FactType
                    End If
                    Call child.SetupForm()
                Else
                    '--------------------------------------------------
                    'Add the ORMReadingEditor form to the bottom of the Page
                    '--------------------------------------------------
                    child = New frmToolboxORMReadingEditor
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    child.zrPage = arPage
                    If arFactTypeInstance IsNot Nothing Then
                        child.zrFactTypeInstance = arFactTypeInstance
                        child.zrFactType = arFactTypeInstance.FactType
                    End If
                    Call child.SetupForm()
                End If
                prApplication.ToolboxForms.Add(child)

                Return child
            End If

            Return Nothing

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    Public Function loadToolboxORMVerbalisationForm(ByVal arModel As FBM.Model, ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane) As frmToolboxORMVerbalisation

        Dim child As New frmToolboxORMVerbalisation

        Try
            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                'child.Show()
                child.BringToFront()

                Return child
            Else
                '----------------------------------------------
                'Create a new instance of the ErrorList form.
                '----------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------                
                    child.zrModel = arModel
                    'child.MdiParent = prApplication.ToolboxForms(0)
                    'child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.4)

                    'child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom)
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane
                    Dim lrDockpanel As WeifenLuo.WinFormsUI.Docking.DockPanel
                    'prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane  'DockPanel.ActivePane
                    lrDockpanel = prApplication.ToolboxForms(0).DockPanel
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    'child.DockTo(prApplication.ToolboxForms(0).PanelPane, DockStyle.Right, 1)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                    prApplication.ToolboxForms.Add(child)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------
                    child.zrModel = arModel
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    prApplication.ToolboxForms.Add(child)
                End If

                Return child
            End If

            Return Nothing
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function loadToolboxTableDataForm(ByVal arModel As FBM.Model, ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane) As frmToolboxTableData

        Dim child As New frmToolboxTableData

        Try
            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                'child.Show()
                child.BringToFront()
                Return child
            Else
                '----------------------------------------------
                'Create a new instance of the ErrorList form.
                '----------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------                
                    child.mrModel = arModel
                    'child.MdiParent = prApplication.ToolboxForms(0)
                    'child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.4)

                    'child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom)
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane
                    Dim lrDockpanel As WeifenLuo.WinFormsUI.Docking.DockPanel
                    'prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane  'DockPanel.ActivePane
                    lrDockpanel = prApplication.ToolboxForms(0).DockPanel
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    'child.DockTo(prApplication.ToolboxForms(0).PanelPane, DockStyle.Right, 1)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                    prApplication.ToolboxForms.Add(child)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------
                    child.mrModel = arModel
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    prApplication.ToolboxForms.Add(child)
                End If
            End If

            Return child
        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    Public Function loadToolboxRichmondBrainBox(ByRef arPage As FBM.Page,
                                    ByVal aoActivePane As WeifenLuo.WinFormsUI.Docking.DockPane) As frmToolboxBrainBox

        Dim child As New frmToolboxBrainBox

        Try
            'CodeSafe
            If aoActivePane Is Nothing Then Return Nothing

            If prApplication.ToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.ToolboxForms.Find(AddressOf child.EqualsByName)
                Call child.setup(arPage)
                child.BringToFront()
            Else
                '----------------------------------------------
                'Create a new instance of the ErrorList form.
                '----------------------------------------------
                If prApplication.ToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the BrainBox form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------  
                    Dim lrPane As WeifenLuo.WinFormsUI.Docking.DockPane
                    Dim lrDockpanel As WeifenLuo.WinFormsUI.Docking.DockPanel

                    prApplication.ToolboxForms(0).Focus()
                    lrPane = prApplication.ToolboxForms(0).Pane
                    lrDockpanel = prApplication.ToolboxForms(0).DockPanel
                    child.Show(lrPane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Right, 0.3)
                    child.DockTo(lrPane, DockStyle.Fill, 0)
                    prApplication.ToolboxForms.Add(child)
                    Call child.setup(arPage)
                Else
                    '--------------------------------------------------
                    'Add the BrainBox form to the bottom of the Page
                    '--------------------------------------------------
                    child.Show(aoActivePane, WeifenLuo.WinFormsUI.Docking.DockAlignment.Bottom, 0.3)
                    Call child.setup(arPage)
                    prApplication.ToolboxForms.Add(child)
                End If
            End If

            Return child

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub LoadToolbox()

        Dim child As New frmToolbox

        Try
            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                child.Close()
                child = New frmToolbox
                child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                prApplication.RightToolboxForms.Add(child)
            Else
                '----------------------------------------------
                'Create a new instance of the Toolbox form.
                '----------------------------------------------
                If prApplication.RightToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                End If
                prApplication.RightToolboxForms.Add(child)
            End If

            '--------------------------------------------------------------------------
            'For Boston there is only one Toolbox Shape set, the ORM toolset
            '--------------------------------------------------------------------------
            child.SetToolbox(pcenumLanguage.ORMModel)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButton_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton_Save.Click

        Try
            Dim lrModel As FBM.Model = Nothing

            'CodeSafe
            Try
                Dim lrObject = zfrmModelExplorer.TreeView.SelectedNode.Tag.MenuType
            Catch ex As Exception
                If prApplication.WorkingModel IsNot Nothing Then
                    lrModel = prApplication.WorkingModel
                    GoTo SaveModel
                Else
                    Exit Sub
                End If
            End Try

            If IsSomething(zfrmModelExplorer) Then
                Select Case zfrmModelExplorer.TreeView.SelectedNode.Tag.MenuType
                    Case Is = pcenumMenuType.modelORMModel
                        lrModel = zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag
SaveModel:
                        If lrModel.IsDirty Or lrModel.hasADirtyPage Then
                            Me.Cursor = Cursors.WaitCursor
                            Boston.WriteToStatusBar("Saving Model: '" & lrModel.Name & "'", True)
                            Call lrModel.Save()
                            Boston.WriteToStatusBar("Saved Model: '" & lrModel.Name & "'")
                            Me.Cursor = Cursors.Default

                            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                                lrInterfaceModel.ModelId = lrModel.ModelId
                                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                                lrBroadcast.Model = lrInterfaceModel
                                Call prDuplexServiceClient.SendBroadcast(Viev.FBM.Interface.pcenumBroadcastType.ModelSaved, lrBroadcast)
                            End If
                        End If

                    Case Is = pcenumMenuType.pageORMModel
                        Dim lrPage As FBM.Page
                        lrPage = zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag
                        If lrPage.IsDirty Or lrPage.Model.IsDirty Then
                            Me.Cursor = Cursors.WaitCursor
                            Boston.WriteToStatusBar("Saving Model: '" & lrPage.Model.Name & "'", True)
                            Call lrPage.Save()
                            Boston.WriteToStatusBar("Saved Model: '" & lrPage.Model.Name & "'", True)
                            Me.Cursor = Cursors.Default

                            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                                lrInterfaceModel.ModelId = lrPage.Model.ModelId
                                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                                lrBroadcast.Model = lrInterfaceModel
                                Call prDuplexServiceClient.SendBroadcast(Viev.FBM.Interface.pcenumBroadcastType.ModelSaved, lrBroadcast)
                            End If
                        End If
                    Case Else
                        '-----------------------------------------------------
                        'Save the current 'WorkingPage' back to the database
                        '-----------------------------------------------------
                        If IsSomething(prApplication.WorkingPage) Then
                            Me.Cursor = Cursors.WaitCursor
                            Boston.WriteToStatusBar("Saving Model: '" & prApplication.WorkingPage.Model.Name & "'", True)
                            Call prApplication.WorkingPage.Save()
                            Boston.WriteToStatusBar("Saved Model: '" & prApplication.WorkingPage.Model.Name & "'", True)
                            Me.Cursor = Cursors.Default

                            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then
                                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                                lrInterfaceModel.ModelId = prApplication.WorkingPage.Model.ModelId
                                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                                lrBroadcast.Model = lrInterfaceModel
                                Call prDuplexServiceClient.SendBroadcast(Viev.FBM.Interface.pcenumBroadcastType.ModelSaved, lrBroadcast)
                            End If
                        End If
                End Select
            End If

            Me.ToolStripButton_Save.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub AboutRichmondToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutRichmondToolStripMenuItem.Click

        frmAbout.ShowDialog()

    End Sub

    Private Sub ToolStripMenuItem12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem_ShowEnterpriseTreeView.Click

        Me.MenuItem_ShowEnterpriseTreeView.Checked = Not Me.MenuItem_ShowEnterpriseTreeView.Checked

        If Me.MenuItem_ShowEnterpriseTreeView.Checked Then
            If IsSomething(zfrmModelExplorer) Then
                '---------------------------------------
                'Enterprise TreeView is already loaded
                '---------------------------------------
            Else
                Call LoadEnterpriseTreeViewer()
            End If
        Else
            '------------------------------------------------
            'If the EnterpriseTreeViewer is open, close it
            '------------------------------------------------
            If IsSomething(zfrmModelExplorer) Then
                '---------------------------------------
                'Enterprise TreeView is already loaded
                '---------------------------------------                
                Me.zfrmModelExplorer = Nothing
            End If
        End If

    End Sub

    Private Sub ToolStripComboBox_zoom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripComboBox_zoom.SelectedIndexChanged

        '---------------------------------------------------------
        'Zooms objects within the selected (Diagram)View form
        '---------------------------------------------------------
        Dim loShapeNode As Object
        Dim lrForm As Object

        Try
            Dim lrPage As FBM.Page = prApplication.WorkingPage

            If lrPage Is Nothing Then
                lrForm = Me.DockPanel.ActiveDocument
            Else
                lrForm = lrPage.Form
            End If

            'CodeSafe
            If ToolStripComboBox_zoom.SelectedItem Is Nothing Then Exit Sub

            If IsSomething(lrForm) Then
                If TypeOf (lrForm) Is frmDiagramORM Then
                    Dim lrfrmORMModel As frmDiagramORM
                    lrfrmORMModel = lrForm

                    'CodeSafe
                    If lrfrmORMModel.DiagramView Is Nothing Then Exit Sub

                    lrfrmORMModel.DiagramView.ZoomFactor = ToolStripComboBox_zoom.SelectedItem.itemdata
                    lrfrmORMModel.Diagram.Invalidate()
                    For Each loShapeNode In lrfrmORMModel.Diagram.Nodes
                        loShapeNode.AllowOutgoingLinks = False
                    Next
                ElseIf TypeOf (lrForm) Is frmDiagramERD Then
                    Dim lrfrmERDiagram As New frmDiagramERD
                    lrfrmERDiagram = lrForm
                    lrfrmERDiagram.DiagramView.ZoomFactor = ToolStripComboBox_zoom.SelectedItem.itemdata
                    lrfrmERDiagram.Diagram.Invalidate()

                ElseIf TypeOf (lrForm) Is frmDiagramPGS Then
                    Dim lrfrmPGSiagram As frmDiagramPGS = lrForm
                    lrfrmPGSiagram.DiagramView.ZoomFactor = ToolStripComboBox_zoom.SelectedItem.itemdata
                    lrfrmPGSiagram.Diagram.Invalidate()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, False,, False, ex)
        End Try

    End Sub

    Private Sub mnuItem_Undo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemUndo.Click

        'Dim loObject As Object = Me.DockPanel.ActiveDocument.DockHandler.Form

        'Select Case loObject.name
        '    Case Is = frmDiagramORM.Name
        '        zrORMModel_view.Diagram.UndoManager.Undo()
        '    Case Is = frm_UseCaseModel.Name
        '        zfrm_UseCaseModel_view.Diagram.UndoManager.Undo()
        'End Select

        Call prApplication.UndoLastAction()


    End Sub


    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click

        ' Determine the active child form.
        Dim lfrm_activeChild As Form = Me.ActiveMdiChild

        '--------------------------------------------------
        'Exit the sub if there are no ActiveMdiChild forms
        '--------------------------------------------------
        If lfrm_activeChild Is Nothing Then Exit Sub

        Select Case lfrm_activeChild.Name
            Case Is = Me.zrORMModel_view.Name
                Dim lfrmORMDiagram As frmDiagramORM
                lfrmORMDiagram = lfrm_activeChild
                Call lfrmORMDiagram.select_all()
        End Select

    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusBarToolStripMenuItem.Click

        StatusBarToolStripMenuItem.Checked = Not StatusBarToolStripMenuItem.Checked
        My.Settings.DisplayStatusBar = StatusBarToolStripMenuItem.Checked
        Me.StatusBar_main.Visible = StatusBarToolStripMenuItem.Checked

    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click

        If IsSomething(prApplication.WorkingModel) Then
            Call prApplication.WorkingModel.Save()
        End If

        '-----------------------------------------------------
        'Save the current 'WorkingPage' back to the database
        '  20150509
        '  NB If the Page is dirty will get saved above.
        '  Look at removing this code in the future.
        '-----------------------------------------------------
        If IsSomething(prApplication.WorkingPage) Then
            Call prApplication.WorkingPage.Save()
        End If

        Me.ToolStripButton_Save.Enabled = False

    End Sub

    Private Sub SaveAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAllToolStripMenuItem.Click

        Dim lr_tree_node As TreeNode
        Dim lr_model As FBM.Model

        'CodeSafe
        If Me.zfrmModelExplorer Is Nothing Then Exit Sub

        Try

            Using lrWaitCursor As New WaitCursor
                For Each lr_tree_node In Me.zfrmModelExplorer.TreeView.Nodes(0).Nodes
                    If IsSomething(lr_tree_node.Tag.Tag) Then
                        lr_model = lr_tree_node.Tag.Tag
                        If lr_model.Loaded And lr_model.IsDirty Then
                            Call lr_model.Save()
                        End If
                    End If
                Next
            End Using

            Me.ToolStripButton_Save.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub SaveAllModels()

        Dim lr_tree_node As TreeNode
        Dim lr_model As FBM.Model

        'CodeSafe
        If Me.zfrmModelExplorer Is Nothing Then Exit Sub

        Try

            Using lrWaitCursor As New WaitCursor
                For Each lr_tree_node In Me.zfrmModelExplorer.TreeView.Nodes(0).Nodes
                    If IsSomething(lr_tree_node.Tag.Tag) Then
                        lr_model = lr_tree_node.Tag.Tag
                        If lr_model.Loaded And lr_model.IsDirty Then
                            Call lr_model.Save()
                        End If
                    End If
                Next
            End Using

            Me.ToolStripButton_Save.Enabled = False

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CopyAsImageToClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyAsImageToClipboardToolStripMenuItem.Click

        ' Determine the active child form.
        Dim lfrm_activeChild As Object = Me.ActiveMdiChild

        '--------------------------------------------------
        'Exit the sub if there are no ActiveMdiChild forms
        '--------------------------------------------------
        If lfrm_activeChild Is Nothing Then Exit Sub

        Call lfrm_activeChild.CopyImageToClipboard()

    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click

        If IsSomething(prApplication.WorkingPage) Then
            prApplication.WorkingPage.DiagramView.PrintOptions.DocumentName = prApplication.WorkingPage.Name
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableImages = False
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableInterior = True
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableShadows = True
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableImages = True
            prApplication.WorkingPage.DiagramView.PrintOptions.Scale = ToolStripComboBox_zoom.SelectedItem.itemdata
            Dim pdoc As New System.Drawing.Printing.PrintDocument
            pdoc.DefaultPageSettings.Landscape = True
            prApplication.WorkingPage.DiagramView.PrintPreview(pdoc)
        End If

    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click

        prApplication.WorkingPage.DiagramView.PrintOptions.DocumentName = "Flowchart"
        prApplication.WorkingPage.DiagramView.PrintOptions.EnableImages = False
        prApplication.WorkingPage.DiagramView.PrintOptions.EnableInterior = True
        prApplication.WorkingPage.DiagramView.PrintOptions.EnableShadows = True
        prApplication.WorkingPage.DiagramView.PrintOptions.Scale = 100
        prApplication.WorkingPage.DiagramView.Print()

    End Sub

    ''' <summary>
    ''' 20220808-VM-Was crashing from within the frmDiagramORMGlossaryView.
    '''         Added Try...Catch
    ''' </summary>
    ''' <param name="arPage"></param>
    Public Sub hide_unnecessary_forms(ByRef arPage As FBM.Page)

        Try
            If IsSomething(arPage) Then
                Select Case arPage.Language
                    Case Is = pcenumLanguage.ORMModel
                        If IsSomething(Me.zfrm_orm_reading_editor) Then
                            If Me.zfrm_orm_reading_editor.Visible = False Then
                                Try
                                    Call Me.zfrm_orm_reading_editor.Show()
                                Catch ex As Exception
                                    'Not a biggie. 20220808-VM-Was crashing from within the frmDiagramORMGlossaryView
                                End Try

                            End If
                        End If
                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function IsDiagramSpyFormLoaded() As Boolean

        Try
            Dim larDiagramSpyPage = From ActivePage In prApplication.ActivePages.ToArray
                                    Where ActivePage.Tag IsNot Nothing
                                    Where ActivePage.Tag.GetType Is GetType(FBM.DiagramSpyPage)
                                    Select ActivePage

            If larDiagramSpyPage.Count = 1 Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function

    Public Sub LoadToolboxKLTheoremWriter()

        Dim child As New frmToolboxKLTheoremWriter

        Try
            If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then
                '-------------------------------------------------------------
                'Form is already loaded. Bring it to the front of the ZOrder
                '-------------------------------------------------------------            
                child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
                Call child.Show()
            Else
                '----------------------------------------------
                'Create a new instance of the ModelDictionary form.
                '----------------------------------------------
                If prApplication.RightToolboxForms.Count > 0 Then
                    '----------------------------------------------------------------------------------------
                    'Add the ErrorList form to the Panel of a form already loaded at the bottom of the Page
                    '----------------------------------------------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                Else
                    '--------------------------------------------------
                    'Add the ErrorList form to the bottom of the Page
                    '--------------------------------------------------                
                    child.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight)
                End If
                prApplication.RightToolboxForms.Add(child)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Call Me.CopySelectedObjectsToClipboard()

    End Sub

    Public Sub CopySelectedObjectsToClipboard()

        Dim liInd As Integer = 0

        If IsSomething(prApplication.WorkingPage) Then
            If IsSomething(prApplication.WorkingPage.SelectedObject) Then
                If prApplication.WorkingPage.SelectedObject.Count > 0 Then
                    'Dim lrModel As Clipbrd.ClipboardModel = prApplication.WorkingPage.Model.CloneClipboard
                    'Dim lrPage As New Clipbrd.ClipboardPage(lrModel, System.Guid.NewGuid.ToString, prApplication.WorkingPage.Name, pcenumLanguage.ORMModel)

                    Dim lrModel As New FBM.Model(pcenumLanguage.ORMModel, System.Guid.NewGuid.ToString, True,, True)
                    lrModel.OriginModelId = prApplication.WorkingModel.ModelId 'Used for differentiation when Pasting.

                    Dim lrPage As New FBM.Page(lrModel, "ClipboardPage", "ClipboardPage", prApplication.WorkingPage.Language)
                    lrPage.CopiedModelId = prApplication.WorkingModel.ModelId
                    lrPage.CopiedPageId = prApplication.WorkingPage.PageId

                    'Get rid of RoleInstances and RoleConstraintRoleInsances from within the set of SelectedObjects
                    For Each lrModelObject In prApplication.WorkingPage.SelectedObject.ToArray
                        If lrModelObject.ConceptType = pcenumConceptType.Role Then
                            Dim lrRoleInstance As FBM.RoleInstance = lrModelObject
                            prApplication.WorkingPage.SelectedObject.Remove(lrRoleInstance)
                            prApplication.WorkingPage.SelectedObject.AddUnique(lrRoleInstance.FactType)
                        End If

                        If lrModelObject.ConceptType = pcenumConceptType.RoleConstraintRole Then
                            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance = lrModelObject
                            prApplication.WorkingPage.SelectedObject.Remove(lrRoleConstraintRoleInstance)
                        End If
                    Next

                    Dim larSelectedObject As New List(Of FBM.ModelObject)
                    'FactTypes first
                    For Each lrModelElement In prApplication.WorkingPage.SelectedObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.FactType).ToArray
                        larSelectedObject.Add(lrModelElement)
                        prApplication.WorkingPage.SelectedObject.Remove(lrModelElement)
                    Next
                    'ValueTypes
                    For Each lrModelElement In prApplication.WorkingPage.SelectedObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.ValueType).ToArray
                        larSelectedObject.Add(lrModelElement)
                        prApplication.WorkingPage.SelectedObject.Remove(lrModelElement)
                    Next
                    'EntityType
                    For Each lrModelElement In prApplication.WorkingPage.SelectedObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType).ToArray
                        larSelectedObject.Add(lrModelElement)
                        prApplication.WorkingPage.SelectedObject.Remove(lrModelElement)
                    Next
                    'RoleConstraint
                    For Each lrModelElement In prApplication.WorkingPage.SelectedObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.RoleConstraint).ToArray
                        larSelectedObject.Add(lrModelElement)
                        prApplication.WorkingPage.SelectedObject.Remove(lrModelElement)
                    Next
                    'The rest
                    For Each lrModelElement In prApplication.WorkingPage.SelectedObject.ToArray
                        larSelectedObject.Add(lrModelElement)
                        prApplication.WorkingPage.SelectedObject.Remove(lrModelElement)
                    Next

                    'CodeSafe
                    'EntityTypes with a CompoundReferenceScheme...but the FactTypes for the ReferenceSchemeRoleConstraint are not present...just don't serialise the ReferenceSchemeRoleConstraint
                    For Each lrModelElement In larSelectedObject.FindAll(Function(x) x.ConceptType = pcenumConceptType.EntityType)
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance = lrModelElement
                        If lrEntityTypeInstance.EntityType.HasCompoundReferenceMode Then
                            For Each lrRole In lrEntityTypeInstance.EntityType.ReferenceModeRoleConstraint.Role
                                If larSelectedObject.Find(Function(x) x.Id = lrRole.FactType.Id) Is Nothing Then
                                    lrEntityTypeInstance.ReferenceModeRoleConstraint = Nothing
                                    lrEntityTypeInstance.EntityType.ReferenceModeRoleConstraint = Nothing
                                End If
                            Next
                        End If
                    Next

                    For liInd = 1 To larSelectedObject.Count 'prApplication.WorkingPage.SelectedObject.Count

                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

                        Dim lrModelObject = larSelectedObject(liInd - 1) 'prApplication.WorkingPage.SelectedObject(liInd - 1)

                        Select Case lrModelObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                Dim lrValueTypeInstance As FBM.ValueTypeInstance
                                lrValueTypeInstance = lrModelObject 'prApplication.WorkingPage.SelectedObject(liInd - 1)

                                lrValueTypeInstance = lrValueTypeInstance.Clone(lrPage, lrValueTypeInstance.ValueType.IsMDAModelElement)
                                lrPage.ValueTypeInstance.AddUnique(lrValueTypeInstance)

                            Case Is = pcenumConceptType.EntityType
                                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                lrEntityTypeInstance = lrModelObject 'prApplication.WorkingPage.SelectedObject(liInd - 1)
                                Boston.IsSerializable(lrPage)
                                lrEntityTypeInstance = lrEntityTypeInstance.Clone(lrPage, True, lrEntityTypeInstance.EntityType.IsMDAModelElement)
                                Boston.IsSerializable(lrPage)
                                lrPage.EntityTypeInstance.AddUnique(lrEntityTypeInstance)
                            Case Is = pcenumConceptType.FactType
                                Dim lrFactTypeInstance As FBM.FactTypeInstance
                                lrFactTypeInstance = lrModelObject 'prApplication.WorkingPage.SelectedObject(liInd - 1)

                                Call Me.CloneFactTypeInstanceToPage(lrFactTypeInstance, lrPage)

                            Case Is = pcenumConceptType.RoleConstraintRole

                                lrRoleConstraintRoleInstance = lrModelObject 'prApplication.WorkingPage.SelectedObject(liInd - 1)

                                If lrModel.RoleConstraint.Exists(AddressOf lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraint.Equals) Then
                                    '--------------------------------------------------------------------------------------------
                                    'The RoleConstraint is already in the Model.
                                    '  NB More than one RoleConstraintRole for the same RoleConstraint may be in the selection.
                                    '--------------------------------------------------------------------------------------------
                                Else
                                    lrModel.AddRoleConstraint(lrRoleConstraintRoleInstance.RoleConstraint.RoleConstraint)
                                End If

                                '---------------------------------------------------------------
                                'Make sure the FactTypes are there for Roles in RoleConstraint
                                '---------------------------------------------------------------                                
                                If lrModel.FactType.Exists(AddressOf lrRoleConstraintRoleInstance.RoleConstraintRole.Role.FactType.Equals) Then
                                Else
                                    lrModel.FactType.Add(lrRoleConstraintRoleInstance.RoleConstraintRole.Role.FactType)
                                End If

                                If lrPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintRoleInstance.RoleConstraint.Equals) Then
                                    '---------------------------------------------------
                                    'The RoleConstraintInstance is already on the Page
                                    '---------------------------------------------------
                                Else
                                    lrPage.RoleConstraintInstance.AddUnique(lrRoleConstraintRoleInstance.RoleConstraint)
                                End If

                                If lrPage.FactTypeInstance.Exists(AddressOf lrRoleConstraintRoleInstance.Role.FactType.Equals) Then
                                Else
                                    lrPage.FactTypeInstance.AddUnique(lrRoleConstraintRoleInstance.Role.FactType)
                                End If

                            Case Is = pcenumConceptType.RoleConstraint

                                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                                lrRoleConstraintInstance = lrModelObject 'prApplication.WorkingPage.SelectedObject(liInd - 1)

                                '---------------------------------------------------------------
                                'Make sure the FactTypes are there for Roles in RoleConstraint
                                '---------------------------------------------------------------
                                For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                                    If Not lrPage.FactTypeInstance.Exists(AddressOf lrRoleConstraintRoleInstance.Role.FactType.Equals) Then
                                        Call Me.CloneFactTypeInstanceToPage(lrRoleConstraintRoleInstance.Role.FactType, lrPage)
                                    End If
                                Next

                                lrRoleConstraintInstance = lrRoleConstraintInstance.Clone(lrPage, lrRoleConstraintInstance.RoleConstraint.IsMDAModelElement)
                                lrPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)

                            Case Is = pcenumConceptType.SubtypeRelationship

                                Dim lrSubtypeRelationshipInstance As FBM.SubtypeRelationshipInstance = lrModelObject

                                lrSubtypeRelationshipInstance.FactType = Me.CloneFactTypeInstanceToPage(lrSubtypeRelationshipInstance.FactType, lrPage)

                                lrSubtypeRelationshipInstance = lrSubtypeRelationshipInstance.Clone(lrPage, True)

                                Dim lrModelElementInstance = lrPage.EntityTypeInstance.Find(Function(x) x.Id = lrSubtypeRelationshipInstance.ModelElement.Id)
                                lrModelElementInstance.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)

                        End Select
                    Next

                    '------------------------------------------------------------
                    'IMPORTANT: Keep the below for testing/debugging.
                    'Objects must be serialisable to use the Clipboard.
                    '  Use the following to test if the Object is serialisable.
                    '------------------------------------------------------------
                    If Boston.IsSerializable(lrPage) Then

                        '----------------------------
                        ' Create a new data format.
                        '----------------------------
                        Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

                        '------------------------------------------------------------------
                        ' Store the Page in a DataObject using the RichmondPage format 
                        ' as the type of format.                     
                        '------------------------------------------------------------------
                        Dim dataObj As IDataObject = New DataObject()

                        Clipboard.Clear()
                        dataObj.SetData(RichmondPage.Name, False, lrPage)
                        Clipboard.SetDataObject(dataObj, True)

                    End If

                End If
            End If
        End If


    End Sub

    Private Function CloneFactTypeInstanceToPage(ByRef arFactTypeInstance As FBM.FactTypeInstance, ByRef arPage As FBM.Page) As FBM.FactTypeInstance

        Dim larInternalUniquenessConstraintInstance As New List(Of FBM.RoleConstraintInstance)
        For Each lrInternalUniquenessConstraintInstance In arFactTypeInstance.InternalUniquenessConstraint
            larInternalUniquenessConstraintInstance.Add(lrInternalUniquenessConstraintInstance)
        Next

        Dim lrOriginalFactType As FBM.FactType = arFactTypeInstance.FactType

        arFactTypeInstance = arFactTypeInstance.Clone(arPage, True, arFactTypeInstance.FactType.IsMDAModelElement)

        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        For Each lrInternalUniquenessConstraintInstance In larInternalUniquenessConstraintInstance
            lrRoleConstraintInstance = lrInternalUniquenessConstraintInstance.Clone(arPage, lrInternalUniquenessConstraintInstance.RoleConstraint.IsMDAModelElement)
            arFactTypeInstance.InternalUniquenessConstraint.Add(lrRoleConstraintInstance)
        Next

        arPage.FactTypeInstance.AddUnique(arFactTypeInstance)

        Dim lrFactType As FBM.FactType
        If arFactTypeInstance.FactType.IsObjectified Then
            For Each lrImpliedFactType In lrOriginalFactType.getImpliedFactTypes
                lrFactType = lrImpliedFactType.Clone(arPage.Model, True, lrImpliedFactType.IsMDAModelElement)
            Next
        End If

        For Each lrInternalUniquenessConstraintInstance In arFactTypeInstance.InternalUniquenessConstraint
            arPage.Model.AddRoleConstraint(lrInternalUniquenessConstraintInstance.RoleConstraint)
            arPage.RoleConstraintInstance.AddUnique(lrInternalUniquenessConstraintInstance)
        Next

        Return arFactTypeInstance

    End Function

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click

        Call Me.PasteToCurrentPageFromClipboard()

    End Sub

    Public Function PageDataExistsInClipboard(ByRef arPage As FBM.Page) As Boolean

        Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

        Try
            '----------------------------------------
            ' Retrieve the data from the clipboard.
            '----------------------------------------
            Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()

            '----------------------------------------------------
            ' Convert the IDataObject type to MyNewObject type. 
            '----------------------------------------------------
            Dim lrPage As FBM.Page

            lrPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page)

            If lrPage Is Nothing Then
                Return False
            Else
                arPage = lrPage
                Return True
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Sub PasteToCurrentPageFromClipboard()

        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
        Dim lrValueTypeInstance As FBM.ValueTypeInstance
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
        Dim lrModelNoteInstance As FBM.ModelNoteInstance

        Try
            If IsSomething(prApplication.WorkingPage) Then

                '-----------------------------
                'Set the RichmondPage format
                '-----------------------------
                Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

                '----------------------------------------
                ' Retrieve the data from the clipboard.
                '----------------------------------------
                Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()

                '----------------------------------------------------
                ' Convert the IDataObject type to MyNewObject type. 
                '----------------------------------------------------
                Dim lrPage As FBM.Page

                lrPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page) 'Clipbrd.ClipboardPage)

                If lrPage Is Nothing Then
                    Exit Sub
                End If

                'CodeSafe: Make sure the Model of the Page has a ModelDictionary
                '=================================================================================
                If lrPage.Model.ModelDictionary Is Nothing Then
                    lrPage.Model.ModelDictionary = New List(Of FBM.DictionaryEntry)
                End If
#Region "Clean ModelError sets"
                For Each lrValueTypeInstance In lrPage.ValueTypeInstance
                    lrValueTypeInstance.ValueType.ModelError = New List(Of FBM.ModelError)
                    lrValueTypeInstance.ModelError = New List(Of FBM.ModelError)
                Next
                For Each lrEntityTypeInstance In lrPage.EntityTypeInstance
                    lrEntityTypeInstance.EntityType.ModelError = New List(Of FBM.ModelError)
                    lrEntityTypeInstance.ModelError = New List(Of FBM.ModelError)
                Next
                For Each lrFactTypeInstance In lrPage.FactTypeInstance
                    lrFactTypeInstance.FactType.ModelError = New List(Of FBM.ModelError)
                    lrFactTypeInstance.ModelError = New List(Of FBM.ModelError)
                Next
                For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance
                    lrRoleConstraintInstance.RoleConstraint.ModelError = New List(Of FBM.ModelError)
                    lrRoleConstraintInstance.ModelError = New List(Of FBM.ModelError)
                Next

                Dim larFact = From FactType In lrPage.Model.FactType
                              From Fact In FactType.Fact
                              Select Fact

                For Each lrFact In larFact
                    lrFact.ModelError = New List(Of FBM.ModelError)
                Next

#End Region
                '=================================================================================


                If lrPage.CopiedModelId = prApplication.WorkingModel.ModelId And
                    lrPage.CopiedPageId = prApplication.WorkingPage.PageId Then
                    MsgBox("You cannot copy and paste to the same Model and Page")
                    Exit Sub
                End If


                If lrPage.CopiedModelId <> prApplication.WorkingModel.ModelId Then
                    '--------------------------------------------------------
                    'Must merge the Page/ModelObjects into the target Model
                    '--------------------------------------------------------

                    '-----------------------------------------------------
                    'Setup the Class to display the SignatureResolutions
                    '-----------------------------------------------------
                    Dim lrSignatureResolutionClass As New tClass
                    Dim loTuple As New Object
                    lrSignatureResolutionClass.add_attribute(New tAttribute("CopiedModelObjectName", GetType(String)))
                    lrSignatureResolutionClass.add_attribute(New tAttribute("NewModelObjectName", GetType(String)))
                    lrSignatureResolutionClass.add_attribute(New tAttribute("Signature", GetType(String)))
                    lrSignatureResolutionClass.add_attribute(New tAttribute("UseEncumbent", GetType(Boolean)))
                    '-----------------------------------------------------------------------------------------------
                    'Create a new instance of the frmSignatureResolution to display any (if any) Signature Clashes
                    '-----------------------------------------------------------------------------------------------
                    Dim lrfrmSignatueResolution As New frmSignatueResolution
                    Dim larSignatureResolutionList As New List(Of Object)
                    Dim lrModelObject As FBM.ModelObject
                    Dim lrEncumbentModelObject As FBM.ModelObject
                    Dim lbDifferentSignatureFound As Boolean = False
                    For Each lrModelObject In lrPage.GetAllPageObjects

                        lrEncumbentModelObject = prApplication.WorkingModel.GetModelObjectByName(lrModelObject.Id)
                        If lrEncumbentModelObject IsNot Nothing Then
                            loTuple = lrSignatureResolutionClass.clone
                            loTuple.CopiedModelObjectName = lrModelObject.Name

                            Select Case lrModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrBaseModelObject As FBM.EntityType = lrModelObject.getBaseModelObject
                            End Select

                            If Not lrModelObject.getBaseModelObject.EqualsBySignature(lrEncumbentModelObject) Then

                                loTuple.NewModelObjectName = "Signature differs from the encumbent Model Object."
                                loTuple.Signature = lrModelObject.getBaseModelObject.GetSignature
                                lbDifferentSignatureFound = True

                                '-----------------------------------------------------------------------------
                                'Add the SignatureResolution to the list on the lrfrmSignatueResolution form                            
                                lrfrmSignatueResolution.zarSignatureResolutionList.Add(loTuple)
                            End If

                        End If
                    Next

                    '-----------------------------------------------------
                    'Display or dispose-of the Signature Resolution form
                    '-----------------------------------------------------
                    If lbDifferentSignatureFound Then
                        If lrfrmSignatueResolution.zarSignatureResolutionList.Count > 0 Then
                            '-----------------------------------------------------------------------
                            'There were Signature Clashes, so display the SignatureResolution form                        
                            lrfrmSignatueResolution.ShowDialog()

                            larSignatureResolutionList = lrfrmSignatueResolution.zarSignatureResolutionList
                        Else
                            lrfrmSignatueResolution.Dispose()
                        End If

                        If larSignatureResolutionList.FindAll(Function(x) x.UseEncumbent = True).Count = 0 Then

                            Dim lsMessage As String = "Resolve the different signatures before pasting to the new Model."
                            lsMessage.AppendString(vbCrLf & vbCrLf & "i.e. Make sure that the Model Objects that you are pasting are the same as the ones you are trying to override.")
                            MsgBox(lsMessage)
                            Exit Sub
                        Else
                            For Each lrTuple In larSignatureResolutionList.FindAll(Function(x) x.UseEncumbent = True)

                                Dim lrEncumbentModelElement = prApplication.WorkingPage.getModelElementById(lrTuple.CopiedModelObjectName)
                                Dim lrReplaceMentModelElement = lrPage.getModelElementById(lrTuple.CopiedModelObjectName)

                                lrReplaceMentModelElement = lrEncumbentModelElement

                                Select Case lrEncumbentModelElement.ConceptType
                                    Case Is = pcenumConceptType.EntityType,
                                              pcenumConceptType.ValueType,
                                              pcenumConceptType.FactType

                                        Dim larFacType As New List(Of FBM.FactType)

                                        For Each lrFactType In lrPage.FactTypeInstance
                                            Dim larRole = From Role In lrFactType.RoleGroup
                                                          Where Role.JoinedORMObject.Id = lrEncumbentModelElement.Id
                                                          Select Role

                                            For Each lrRole In larRole
                                                lrRole.JoinedORMObject = lrEncumbentModelElement
                                            Next
                                        Next

                                        For Each lrFactType In lrPage.Model.FactType
                                            Dim larRole = From Role In lrFactType.RoleGroup
                                                          Where Role.JoinedORMObject.Id = lrEncumbentModelElement.Id
                                                          Select Role

                                            For Each lrRole In larRole
                                                Select Case lrRole.TypeOfJoin
                                                    Case Is = pcenumRoleJoinType.EntityType
                                                        lrRole.JoinedORMObject = CType(lrEncumbentModelElement, FBM.EntityTypeInstance).EntityType
                                                    Case Is = pcenumRoleJoinType.ValueType
                                                        lrRole.JoinedORMObject = CType(lrEncumbentModelElement, FBM.ValueTypeInstance).ValueType
                                                    Case Is = pcenumRoleJoinType.FactType
                                                        lrRole.JoinedORMObject = CType(lrEncumbentModelElement, FBM.FactTypeInstance).FactType
                                                End Select
                                            Next
                                        Next

                                End Select

                            Next
                        End If

                    End If

                    lrfrmSignatueResolution.Dispose()

                End If 'Pasting to a different Model.



                '-------------------------------------------------------------------------------------------------------------
                'Process ValueTypes first, because they are the least bothersome in terms of cloning to the new/target Model
                '  NB If an EntityType has a ReferenceModeValueType then processing ValueTypes first makes sense.
                '  The ReferenceModeValueType will already be loaded on the Page/Model.
                '-------------------------------------------------------------------------------------------------------------
                For Each lrValueTypeInstance In lrPage.ValueTypeInstance
                    Dim loPt As New PointF(lrValueTypeInstance.X, lrValueTypeInstance.Y)
                    lrValueTypeInstance.ValueType.Model = prApplication.WorkingModel

                    If prApplication.WorkingPage.ValueTypeInstance.FindAll(Function(x) x.Id = lrValueTypeInstance.Id).Count = 0 Then
                        prApplication.WorkingPage.DropValueTypeAtPoint(lrValueTypeInstance.ValueType, loPt, True)
                    End If
                Next

                '--------------------------------------------------------------------------------------------------
                'CodeSafe: Some RoleConstraints are within copied FactTypes (as InternalUniquenessConstraints)
                '  and not at the Page level.
                For Each lrRoleConstraint In lrPage.Model.RoleConstraint
                    lrRoleConstraint.ChangeModel(prApplication.WorkingModel, False)
                Next

                'RoleConstraints...change Model and make sure Ids are unique
                If lrPage.CopiedModelId <> prApplication.WorkingModel.ModelId Then
                    For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance

                        lrRoleConstraintInstance.RoleConstraint = lrRoleConstraintInstance.RoleConstraint.ChangeModel(prApplication.WorkingModel, False, True)

                        Dim lsUniqueId As String = prApplication.WorkingModel.CreateUniqueRoleConstraintName(lrRoleConstraintInstance.Id, 0)

                        'Has to be unique in the copied model as well.
                        If lsUniqueId <> lrRoleConstraintInstance.Id Then
                            lsUniqueId = lrPage.Model.CreateUniqueRoleConstraintName(lsUniqueId, 0)
                        End If

                        lrRoleConstraintInstance.Id = lsUniqueId
                        lrRoleConstraintInstance.Name = lsUniqueId
                        lrRoleConstraintInstance.Symbol = lsUniqueId
                        lrRoleConstraintInstance.RoleConstraint.Id = lsUniqueId
                        lrRoleConstraintInstance.RoleConstraint.Name = lsUniqueId
                        lrRoleConstraintInstance.RoleConstraint.Symbol = lsUniqueId
                    Next

                    'Make doubly sure have captured all the RoleConstraints, to change Model and have/set unique Id.
                    For Each lrRoleConstraint In lrPage.Model.RoleConstraint

                        Call lrRoleConstraint.ChangeModel(prApplication.WorkingModel, False)

                        Dim lsUniqueId As String = prApplication.WorkingModel.CreateUniqueRoleConstraintName(lrRoleConstraint.Id, 0)

                        lrRoleConstraint.Id = lsUniqueId
                        lrRoleConstraint.Name = lsUniqueId
                        lrRoleConstraint.Symbol = lsUniqueId
                    Next

                    'Make tripply sure we have got all the RoleConstraints
                    For Each lrFactTypeInstance In lrPage.FactTypeInstance

                        For Each lrRoleConstraintInstance In lrFactTypeInstance.InternalUniquenessConstraint

                            lrRoleConstraintInstance.RoleConstraint = lrRoleConstraintInstance.RoleConstraint.ChangeModel(prApplication.WorkingModel, False, True)

                            Dim lsUniqueId As String = prApplication.WorkingModel.CreateUniqueRoleConstraintName(lrRoleConstraintInstance.Id, 0)

                            lrRoleConstraintInstance.Id = lsUniqueId
                            lrRoleConstraintInstance.Name = lsUniqueId
                            lrRoleConstraintInstance.Symbol = lsUniqueId
                            lrRoleConstraintInstance.RoleConstraint.Id = lsUniqueId
                            lrRoleConstraintInstance.RoleConstraint.Name = lsUniqueId
                            lrRoleConstraintInstance.RoleConstraint.Symbol = lsUniqueId
                        Next

                    Next
                End If

                lrPage.EntityTypeInstance.Sort(AddressOf FBM.EntityType.CompareSubtypeConstraintExistance)
                For Each lrEntityTypeInstance In lrPage.EntityTypeInstance.ToArray
                    Dim loPt As New PointF(lrEntityTypeInstance.X, lrEntityTypeInstance.Y)
                    lrEntityTypeInstance.EntityType = lrEntityTypeInstance.EntityType.ChangeModel(prApplication.WorkingModel, False, True)
                    If prApplication.WorkingPage.EntityTypeInstance.FindAll(Function(x) x.Id = lrEntityTypeInstance.Id).Count = 0 Then
                        'Make sure there is a DictionaryEntry for the EntityType.
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(prApplication.WorkingModel,
                                                                         lrEntityTypeInstance.Id,
                                                                         pcenumConceptType.EntityType,
                                                                         lrEntityTypeInstance.EntityType.ShortDescription,
                                                                         lrEntityTypeInstance.EntityType.LongDescription,
                                                                         True,
                                                                         True,
                                                                         "")
                        Call prApplication.WorkingModel.AddModelDictionaryEntry(lrDictionaryEntry,,,,,, True,,)
                        'The EntityType doesn't already exist on the Page.
                        prApplication.WorkingPage.DropEntityTypeAtPoint(lrEntityTypeInstance.EntityType, loPt, True)
                    End If
                Next

                '--------------------------------
                'Display any Subtype Relationships
                '--------------------------------
                Dim lrSubtypeRelationship As FBM.SubtypeRelationshipInstance
                For Each lrEntityTypeInstance In prApplication.WorkingPage.EntityTypeInstance
                    For Each lrSubtypeRelationship In lrEntityTypeInstance.SubtypeRelationship
                        Call lrSubtypeRelationship.DisplayAndAssociate()
                    Next
                Next

                '-----------------------------------------------------------------
                'ImpliedFactTypes
                For Each lrFactType In lrPage.Model.FactType
                    If Not prApplication.WorkingModel.FactType.Exists(AddressOf lrFactType.Equals) Then
                        Call lrFactType.ChangeModel(prApplication.WorkingModel, True)
                    End If
                Next


                lrPage.FactTypeInstance.Sort(AddressOf FBM.FactType.CompareRolesJoiningFactTypesCount)
                For Each lrFactTypeInstance In lrPage.FactTypeInstance
                    Dim loPt As New PointF(lrFactTypeInstance.X, lrFactTypeInstance.Y)
                    lrFactTypeInstance.FactType.Model = prApplication.WorkingModel
                    lrFactTypeInstance.Model = prApplication.WorkingModel
                    If prApplication.WorkingPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                        If prApplication.WorkingPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals).IsDisplayedAssociated Then
                            '---------------------------------------------
                            'Already on the Page and DisplayedAssociated
                            '---------------------------------------------
                        Else
                            prApplication.WorkingPage.DropFactTypeAtPoint(lrFactTypeInstance.FactType, loPt, True)
                        End If
                    Else
                        prApplication.WorkingPage.DropFactTypeAtPoint(lrFactTypeInstance.FactType, loPt, True)
                    End If
                Next

                For Each lrRoleConstraintInstance In lrPage.RoleConstraintInstance

                    Dim loPt As New PointF(lrRoleConstraintInstance.X, lrRoleConstraintInstance.Y)

                    If Not prApplication.WorkingPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                        prApplication.WorkingPage.DropRoleConstraintAtPoint(lrRoleConstraintInstance.RoleConstraint, loPt, True)
                    End If
                Next

                For Each lrModelNoteInstance In lrPage.ModelNoteInstance
                    Dim loPt As New PointF(lrModelNoteInstance.X, lrModelNoteInstance.Y)
                    'prApplication.WorkingPage.DropModelNoteAtPoint(lrModelNoteInstance.ModelNote, loPt)
                Next

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub StandardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StandardToolStripMenuItem.Click

        If StandardToolStripMenuItem.Checked Then
            Me.ToolStrip_main.Visible = False
            StandardToolStripMenuItem.Checked = False
            My.Settings.ShowStandardToolbar = False
        Else
            Me.ToolStrip_main.Visible = True
            StandardToolStripMenuItem.Checked = True
            My.Settings.ShowStandardToolbar = True
        End If

        My.Settings.Save()

    End Sub

    Private Sub ToolStripButtonPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonPrint.Click

        If IsSomething(prApplication.WorkingPage) Then
            prApplication.WorkingPage.DiagramView.PrintOptions.DocumentName = prApplication.WorkingModel.Name & ", " & prApplication.WorkingPage.Name
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableImages = False
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableInterior = True
            prApplication.WorkingPage.DiagramView.PrintOptions.EnableShadows = True
            prApplication.WorkingPage.DiagramView.PrintOptions.Scale = 100
            prApplication.WorkingPage.DiagramView.Print()
        End If

    End Sub

    Private Sub ToolStripButtonHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonHelp.Click

        Me.HelpProvider.HelpNamespace = My.Settings.HelpfileLocation
        Help.ShowHelp(Me, Me.HelpProvider.HelpNamespace)

    End Sub

    Private Sub ToolStripMenuItemHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemHelp.Click

        Me.HelpProvider.HelpNamespace = My.Settings.HelpfileLocation
        Help.ShowHelp(Me, Me.HelpProvider.HelpNamespace)

    End Sub

    Private Sub OpenLogFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemOpenLogFile.Click

        Dim lrErrorLogFilePath As String = ""
        Dim lrErrorLogFilePathName As String = ""

        lrErrorLogFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\Errors\"
        'MsgBox(lrErrorLogFilePath)

        If Not System.IO.Directory.Exists(lrErrorLogFilePath) Then
            System.IO.Directory.CreateDirectory(lrErrorLogFilePath)
        End If

        lrErrorLogFilePathName = lrErrorLogFilePath & "errlog.txt"

        If (System.IO.File.Exists(lrErrorLogFilePathName)) Then
            System.Diagnostics.Process.Start(lrErrorLogFilePathName)
        End If

    End Sub

    Private Sub BackupDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackupDatabaseToolStripMenuItem.Click

        Try
            Dim ls_db_location As String = ""
            Dim liInd As Integer = 0
            Dim larConnectionString() As String
            Dim lsNewDatabaseAndPath As String

            If My.Settings.DatabaseType <> pcenumDatabaseType.MSJet.ToString Then
                MsgBox("Can only backup the MSJet version of the Boston database. Exiting.")
                Exit Sub
            End If

            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)


            lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

            larConnectionString = Split(My.Settings.DatabaseConnectionString, ";")

            '20221006-VM-Removed
            'For liInd = 0 To larConnectionString.Length - 1
            '    ls_db_location = larConnectionString(liInd)
            '    ls_db_location = Trim(ls_db_location)
            '    If ls_db_location.StartsWith("Data Source=") Then
            '        ls_db_location = ls_db_location.Replace("Data Source=", "")
            '        Exit For
            '    End If
            'Next

            ls_db_location = lrSQLConnectionStringBuilder("Data Source")

            '------------------------------------------------
            'Define the location of database
            '------------------------------------------------
            lsNewDatabaseAndPath = Path.GetDirectoryName(ls_db_location)
            lsNewDatabaseAndPath = String.Format(lsNewDatabaseAndPath & "\boston{0:yyyyMMddHHmmss}.vdb", Now)

            Dim lrSaveFileDialog As New SaveFileDialog()

            lrSaveFileDialog.InitialDirectory = Path.GetDirectoryName(ls_db_location)
            lrSaveFileDialog.Filter = "Boston Databaes File (*.vdb)|*.vdb"
            lrSaveFileDialog.FilterIndex = 0
            lrSaveFileDialog.RestoreDirectory = True
            lrSaveFileDialog.FileName = lsNewDatabaseAndPath

            If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then
                lsNewDatabaseAndPath = lrSaveFileDialog.FileName
                File.Copy(ls_db_location, lsNewDatabaseAndPath, True)
                MsgBox("Created database backup at :" & lsNewDatabaseAndPath)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub DeleteLogFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteLogFileToolStripMenuItem.Click

        Dim lrErrorLogFilePath As String = ""

        lrErrorLogFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\Errors\"
        'MsgBox(lrErrorLogFilePath)

        If Not System.IO.Directory.Exists(lrErrorLogFilePath) Then
            System.IO.Directory.CreateDirectory(lrErrorLogFilePath)
        End If

        System.IO.File.Delete(lrErrorLogFilePath & "errlog.txt")

        lrErrorLogFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\Errors\"

        'Create an Empty File
        Dim fs As FileStream = New FileStream(lrErrorLogFilePath & "errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim s As StreamWriter = New StreamWriter(fs)
        s.Close()
        fs.Close()

        Boston.WriteToStatusBar("Successfully deleted the log file: " & lrErrorLogFilePath & "errlog.txt")

        Me.NotifyIcon.Icon = SystemIcons.Application
        Me.NotifyIcon.BalloonTipTitle = "Boston"
        Me.NotifyIcon.BalloonTipText = "Log file deleted."
        Me.NotifyIcon.BalloonTipIcon = ToolTipIcon.Info
        Me.NotifyIcon.Visible = True
        Me.NotifyIcon.ShowBalloonTip(1000)

    End Sub

    Private Sub ToolStripButtonNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonNew.Click

        If IsSomething(Me.zfrmModelExplorer) Then
            If IsSomething(Me.zfrmModelExplorer.TreeView.SelectedNode) Then
                Dim lrEnterpriseView As tEnterpriseEnterpriseView
                lrEnterpriseView = Me.zfrmModelExplorer.TreeView.SelectedNode.Tag

                If lrEnterpriseView.MenuType = pcenumMenuType.modelORMModel Then
                    Call Me.zfrmModelExplorer.AddPageToModel(Me.zfrmModelExplorer.TreeView.SelectedNode)
                End If
            End If
        End If

    End Sub


    Private Sub TestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestToolStripMenuItem.Click

        Call LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

    End Sub


    Private Sub ToolStripMenuItemToolbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemToolbox.Click

        Call LoadToolbox()

    End Sub

    Private Sub ToolStripMenuItemDiagramOverview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemDiagramOverview.Click

        Try

            Call load_diagram_overview_form()

            If IsSomething(prApplication.WorkingPage) Then
                Select Case prApplication.WorkingPage.Language
                    Case Is = pcenumLanguage.ORMModel,
                              pcenumLanguage.EntityRelationshipDiagram,
                              pcenumLanguage.PropertyGraphSchema

                        Call zfrm_diagram_overview.SetDocument(prApplication.WorkingPage.DiagramView)
                End Select
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemKLTheoremWriter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemKLTheoremWriter.Click

        Call Me.LoadToolboxKLTheoremWriter()

    End Sub

    Private Sub ToolStripMenuItemRedo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRedo.Click

        Call prApplication.RedoLastAction()

    End Sub

    Private Function PerformDatabaseUpgrade() As Boolean

        Dim lsMessage As String

        Try
            PerformDatabaseUpgrade = False

            '----------------------------------------------------------
            'Database is not of the version expected by the application
            '----------------------------------------------------------
            If Not My.Settings.SilentDatabaseUpgrade Then

                lsMessage = "Boston has detected that a 'Boston Database Version' upgrade is required. "
                lsMessage = lsMessage & vbCrLf & vbCrLf
                lsMessage = lsMessage & "Your current Boston Database Version is: " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1).ToString & " (from database)"
                lsMessage = lsMessage & vbCrLf
                lsMessage = lsMessage & "The required Boston Database Version is: " & prApplication.DatabaseVersionNr
                lsMessage = lsMessage & vbCrLf & vbCrLf
                lsMessage = lsMessage & "Press [OK] to continue with the upgrade."
                lsMessage = lsMessage & vbCrLf & vbCrLf
                lsMessage = lsMessage & "Note: This message appears because you have recently installed an upgrade to Boston, or because there is an outstanding Boston database upgrade that is requried for the successful operation of Boston."
                lsMessage = lsMessage & vbCrLf & vbCrLf
                lsMessage = lsMessage & "Database Connection String = '" & My.Settings.DatabaseConnectionString & "'"
                lsMessage = lsMessage & vbCrLf & vbCrLf
                lsMessage = lsMessage & "Database Version Number (from database): " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1)

                Call MsgBox(lsMessage, vbExclamation)
            End If

            '---------------------------------------------------------------------------------------------------------------------------
            'Check to see if the rostersdatabaseupgrade.vdb file has been posted to the <AppPath./rostersdatbase/rostersdatabaseupgrade/
            '  directory. If this file exists, then load the contents into the 'upgrade' and 'upgrade_sql' tables within the Richmond database.
            '  In this manner, a new 'upgrade' can be delivered to the Richmond application, the sequential 'sql statements' to upgrade the
            '  database are loaded into the Roster database, and executed to effect the upgrade.
            '---------------------------------------------------------------------------------------------------------------------------        
            Dim lsUpgradeDatabaseFilePath As String = ""
            Dim lsDatabaseFilePath As String

            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
            lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

            lsDatabaseFilePath = lrSQLConnectionStringBuilder("Data Source")
            '-----------------------------------------------------------------------
            'The Upgrade Database is readonly, so can be in the application file path.
            lsUpgradeDatabaseFilePath = Path.GetDirectoryName(Boston.MyPath & "\database\") & "\databaseupgrade\bostondatabaseupgrade.vdb"
            If System.IO.File.Exists(lsUpgradeDatabaseFilePath) Then
                '----------------------------------------------------------------------
                'The rostersdatbaseupgrade.vdb file exists, so open the database
                '  and load the contents into the Richmond database.
                '----------------------------------------------------------------------
                pdbDatabaseUpgradeConnection.Open("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" & lsUpgradeDatabaseFilePath)

                '----------------------------------------------------------------------
                'Now repopulate the 'upgrade' and the 'upgrade_sql' tables.
                '----------------------------------------------------------------------            
                Call tableDatabaseUpgrade.LoadRichmondDatabaseUpgradeInformation()

                '----------------------------------------------------------
                'Check to see that there is actual 'upgrade' information
                '  in the Richmond database.
                '----------------------------------------------------------
                If GetRequiredUpgradeCount() > 0 Then

                    If My.Settings.SilentDatabaseUpgrade Then

#Region "Show FlashCard"
                        Dim lfrmFlashCard As New frmFlashCard
                        lfrmFlashCard.ziIntervalMilliseconds = 2600
                        lfrmFlashCard.BackColor = Color.LightGray
                        lsMessage = "Upgrading the database. This won't take too long."
                        lfrmFlashCard.zsText = lsMessage
                        Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)
#End Region

                        Dim lsSQLFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\TempFiles\"
                        System.IO.Directory.CreateDirectory(lsSQLFilePath)

                        Call prApplication.CloseDatabase()

                        Try
                            System.IO.File.Delete(lsSQLFilePath & "boston.ldb")
                        Catch ex As Exception
                        End Try
                        Boston.WaitForFile(lsSQLFilePath & "boston.mdb", IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.Read)
                        System.IO.File.Copy(prApplication.DatabaseLocationName, lsSQLFilePath & "boston.mdb", True)

                        Call Boston.OpenDatabase(lsSQLFilePath & "boston.mdb")

                        Dim lrDatabaseUpgrade As New DatabaseUpgrade.Upgrade
                        While tableDatabaseUpgrade.GetNextRequiredUpgrade(lrDatabaseUpgrade, True) IsNot Nothing

                            lfrmFlashCard.ziIntervalMilliseconds = 1600
                            lsMessage = "Upgrading the database from Version " & lrDatabaseUpgrade.FromVersionNr & " to Version " & lrDatabaseUpgrade.ToVersionNr
                            lfrmFlashCard.zsText = lsMessage
                            liDialogResult = lfrmFlashCard.ShowDialog(Me, "LightGray")

                            With New WaitCursor

                                If Database.Database.PerformNextRequiredDatabaseUpgrade(lrDatabaseUpgrade.UpgradeId, lrDatabaseUpgrade.FromVersionNr, lrDatabaseUpgrade.ToVersionNr) Then
                                    '------------------------------------------------
                                    'Update the Boston DatabaseVersionNr
                                    '------------------------------------------------
                                    Call Boston.UpdateDatabaseVersion(lrDatabaseUpgrade.ToVersionNr)
                                    Call tableDatabaseUpgrade.MarkUpgradeAsSuccessfulImplementation(lrDatabaseUpgrade.UpgradeId)

                                    'CodeSafe
                                    Call tableDatabaseUpgrade.MarkAllPreviousDatabaseUpgradesSuccessful(lrDatabaseUpgrade.UpgradeId)
                                Else
                                    Throw New Exception("Failed Database Upgrade: From version: " & lrDatabaseUpgrade.FromVersionNr & " to version: " & lrDatabaseUpgrade.ToVersionNr)
                                End If

                            End With

                        End While

                        prApplication.CloseDatabase()
                        Boston.WaitForFile(prApplication.DatabaseLocationName, IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                        System.IO.File.Copy(lsSQLFilePath & "boston.mdb", prApplication.DatabaseLocationName, True)
                        Boston.WaitForFile(lsSQLFilePath & "boston.mdb", IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)
                        System.IO.File.Delete(lsSQLFilePath & "boston.mdb")
                        System.IO.File.Delete(lsSQLFilePath & "boston.ldb")
                        Boston.OpenDatabase(prApplication.DatabaseLocationName)

                        lfrmFlashCard.ziIntervalMilliseconds = 5600
                        lfrmFlashCard.BackColor = Color.LightGray
                        lsMessage = "Successfully upgraded to database version: " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
                        lfrmFlashCard.zsText = lsMessage
                        liDialogResult = lfrmFlashCard.ShowDialog(Me)
                    Else
                        '------------------------------------------------
                        'Now, with the user, perform the actual Upgrade
                        '------------------------------------------------
                        frmDatabaseUpgrade.ShowDialog()
                    End If

                    '------------------------------------------------------------------------------------------------------
                    'Always replace the CoreMetaModel
                    '  This is because there is no safe way to replace the Core metamodel for skipped Database Upgrades.
                    '  E.g. If the user skips a Database Upgrade, and the Model structure changes (in the database),
                    '  then the code in Boston will be at the 'last' version of the model, not the earlier version
                    '  and the Model.Save function will fail.
                    '  Therefore the very last thing to do on a database upgrade is replace the Core Metamodel.
                    '------------------------------------------------------------------------------------------------------
                    With New WaitCursor
                        Call DatabaseUpgradeFunctions.ReplaceCoreModel()
                    End With

                    '------------------------------------------------
                    'Check to see if the user upgraded the database
                    '------------------------------------------------
                    Dim lsDatabaseVersionNumber As String = ""
                    lsDatabaseVersionNumber = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
                    If CDbl(prApplication.DatabaseVersionNr) = lsDatabaseVersionNumber Then
                        'If 1 = 2 Then 'See above. Previously... ps_application_database_version_nr = ps_current_database_version_nr Then
                        '----------------------------------------
                        'All good...Database is up to date
                        '----------------------------------------
                        PerformDatabaseUpgrade = True
                    Else
                        '--------------------------------------------------------------------------------
                        'Application will close because the RichmondDatabase is not at the correct version
                        '--------------------------------------------------------------------------------
                        lsMessage = "Boston will now close."
                        lsMessage = lsMessage & vbCrLf & vbCrLf
                        lsMessage = lsMessage & "The required database update must be performed to continue successfully using Boston."
                        lsMessage = lsMessage & vbCrLf & vbCrLf
                        lsMessage = lsMessage & "Your current Boston Database Version is: " & lsDatabaseVersionNumber
                        lsMessage = lsMessage & vbCrLf
                        lsMessage = lsMessage & "The required Boston Database Version is: " & prApplication.DatabaseVersionNr
                        lsMessage = lsMessage & vbCrLf & vbCrLf
                        lsMessage = lsMessage & "Please restart Boston and complete the required database update/s."
                        Call MsgBox(lsMessage, vbExclamation)
                        Me.Close()
                    End If
                Else
                    '------------------------------------------------------------------------------------------
                    'There is no sense proceding because there are no appropriate records in the 'upgrade' table.
                    '  i.e. where the 'successful_implementation' field is set to 'FALSE'
                    '------------------------------------------------------------------------------------------
                    lsMessage = "Please contact FactEngine Customer Support. The Boston database requires an upgrade, however the upgrade information is not available."
                    Call MsgBox(lsMessage, vbExclamation)
                End If
            Else
                '------------------------------------------------------------------------------------------
                'There is no sense proceding with the upgrade because the rostersdatabaseupgrade.vdb file
                '  does not exists. This file is required to perform the upgrade because it contains the
                '  sequential set of sql statements required to perform the upgrade.
                '  Inform the user that the file does not exist. Abort the upgrade, and close Boston.
                '------------------------------------------------------------------------------------------
                lsMessage = "The file required to upgrade the Boston database (bostondatabaseupgrade.vdb) is missing."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Reinstall the Boston upgrade, and if the problem still exists contact FactEngine."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Boston will now close."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "The required database update must be performed to continue successfully using Boston."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Please restart Boston after installing the upgrade to complete the required database update/s."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Expecting to find this file: " & lsUpgradeDatabaseFilePath
                Call MsgBox(lsMessage, vbExclamation)
                PerformDatabaseUpgrade = False
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Sub EmailSupportvievcomToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EmailSupportvievcomToolStripMenuItem.Click

        Try
            System.Diagnostics.Process.Start(String.Format("mailto:{0}", "support@factengine.ai"))
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: You might not have a default email application setup in Windows."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Email support@factengine.ai for support on Boston"
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripButtonCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonCopy.Click

        Dim loForm As Form

        loForm = Me.DockPanel.ActiveDocument

        If IsSomething(loForm) Then
            If loForm.Name = frmDiagramORM.Name Then
                Dim lrPage As New FBM.Page
                Dim lrORMForm As frmDiagramORM
                lrORMForm = loForm
                lrPage = lrORMForm.zrPage
                Call Me.CopySelectedObjectsToClipboard()
            End If
        End If

    End Sub

    Private Sub ToolStripButtonPaste_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButtonPaste.Click

        Dim loForm As Form

        loForm = Me.DockPanel.ActiveDocument

        If IsSomething(loForm) Then
            If loForm.Name = frmDiagramORM.Name Then
                Dim lrPage As New FBM.Page
                Dim lrORMForm As frmDiagramORM
                lrORMForm = loForm
                lrPage = lrORMForm.zrPage
                Call Me.PasteToCurrentPageFromClipboard()
            End If
        End If

    End Sub

    Private Sub ToolStripMenuItemEdit_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemEdit.DropDownOpening

        Dim lrPage As New FBM.Page
        Dim loForm As Form

        loForm = Me.DockPanel.ActiveDocument

        If Me.PageDataExistsInClipboard(lrPage) Then
            '------------------------------------
            'Page data exists in the clipboard.
            '------------------------------------
            If loForm.Name = Me.zrORMModel_view.Name Then
                '------------------------------------------------------------------------
                'Current form is an ORM Diagram form, so can at least paste to the Page
                '  if it isn't the same page the data was copied from
                '------------------------------------------------------------------------
                Dim loORMDiagramForm As frmDiagramORM
                loORMDiagramForm = loForm
                If IsSomething(loORMDiagramForm.zrPage) Then
                    If lrPage.CopiedPageId <> loORMDiagramForm.zrPage.PageId Then
                        Me.PasteToolStripMenuItem.Enabled = True
                    Else
                        Me.PasteToolStripMenuItem.Enabled = False
                    End If
                End If
            End If
        Else
            Me.PasteToolStripMenuItem.Enabled = False
        End If

    End Sub

    Private Sub RemoveUnneededConceptsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveUnneededConceptsToolStripMenuItem.Click

        frmCRUDRemoveUnneededConcepts.ShowDialog()

    End Sub

    Private Sub DiagramSpyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemDiagramSpy.Click

        '----------------------------------------------------------------------
        'Code safe
        '-----------
        If prApplication.WorkingModel Is Nothing Then
            MsgBox("Select a Model to work with before opening the DiagramSpy.")
            Exit Sub
        End If


        Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(prApplication.WorkingModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

        Call Me.LoadDiagramSpy(lrDiagramSpyPage, Nothing)

    End Sub

    Private Sub ToolStripMenuItemView_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemView.Click

        '---------------------------------
        'Code safe
        '-----------
        If prApplication.WorkingModel Is Nothing Then
            Me.ToolStripMenuItemDiagramSpy.Enabled = False
        Else
            Me.ToolStripMenuItemDiagramSpy.Enabled = True
        End If

    End Sub

    Private Sub PluginViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PluginViewerToolStripMenuItem.Click

        frmPluginViewer.ShowDialog()

    End Sub

    Private Sub InitializeClient()
        '20200903-VM-Tested on VM on Azure. Works fine. No configuration required. Just run the BostonServer on the VM first.
        'The 'Server: Connected' message can be seen in the bottom left corner of the Main form in the status bar.

        Try
            If prDuplexServiceClient IsNot Nothing Then
                Try
                    prDuplexServiceClient.Close()
                Catch
                    prDuplexServiceClient.Abort()
                Finally
                    prDuplexServiceClient = Nothing
                End Try
            End If

            Dim DuplexCallback As New DuplexCallback()

            Dim instanceContext As New InstanceContext(DuplexCallback)
            Dim dualHttpBinding As New WSDualHttpBinding(WSDualHttpSecurityMode.None)
            dualHttpBinding.OpenTimeout = New TimeSpan(0, 0, 5)
            Dim endpointAddress As New EndpointAddress(ServiceEndpointUri)
            prDuplexServiceClient = New DuplexServiceClient.DuplexServiceClient(instanceContext, dualHttpBinding, endpointAddress)
            prDuplexServiceClient.Open()
            prDuplexServiceClient.Connect()

            'AddHandler DuplexCallback.ServiceCallbackEvent, AddressOf prDuplexServiceClient.HandleServiceCallbackEvent
            AddHandler DuplexCallback.BroadcastEventReceived, AddressOf prDuplexServiceClient.HandleBroadcastReceived

            Me.ToolStripStatusLabelClientServer.Text = "Server: Connected"
            Me.ToolStripButtonProfile.Visible = True

        Catch ex As Exception
            MsgBox(ex.Message)
            Me.ToolStripStatusLabelClientServer.Text = "Server: Not Connected"
        End Try
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemLogIn.Click

        If frmLogin.ShowDialog() = Windows.Forms.DialogResult.OK Then

            '------------------------------------------------------------------
            'LogIn from populates prApplication.User
            Call Me.logInUser(prApplication.User)

        End If

    End Sub

    Private Sub ToolStripButtonProfile_Click(sender As Object, e As EventArgs) Handles ToolStripButtonProfile.Click

        Call Me.LoadCRUDEditUser(prApplication.User)

    End Sub

    Private Sub AddUserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddUser.Click

        Call Me.LoadCRUDAddUser()

    End Sub

    Private Sub EditUserToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditUserToolStripMenuItem.Click

        Dim lrGenericSelection As New tGenericSelection()
        Dim lrUser As New ClientServer.User

        Dim lsWhereClause As String = ""

        If prApplication.User.IsSuperuser Then
            lsWhereClause = ""
        Else
            lsWhereClause = " WHERE Id = '" & prApplication.User.Id & "'"
        End If

        If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "User",
                                               "ClientServerUser",
                                               "FirstName & ' ' & LastName AS Name, Username",
                                               "Username",
                                               lsWhereClause,
                                               Nothing,
                                               pcenumComboBoxStyle.DropdownList,
                                               "1,2",
                                               2,
                                               "120;120",
                                               "Name,Username") = Windows.Forms.DialogResult.OK Then


            lrUser.Username = lrGenericSelection.SelectValue
            Call tableClientServerUser.getUserDetailsByUsername(lrUser.Username, lrUser)

            Call Me.LoadCRUDEditUser(lrUser)
        End If


    End Sub

    Private Sub AddProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddProjectToolStripMenuItem.Click

        Dim lsMessage As String = ""

        Try
            If (prApplication.User Is Nothing) Then
                lsMessage = "You must log in before you can create a Project"
                Throw New Exception(lsMessage)
            ElseIf prApplication.User.IsLoggedIn = False Then
                lsMessage = "You must log in before you can create a Project"
                Throw New Exception(lsMessage)
            End If

            Call Me.LoadCRUDAddProject()

        Catch ex As Exception
            prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical)
        End Try

    End Sub

    Private Sub EditProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditProjectToolStripMenuItem.Click

        Dim lrGenericSelection As New tGenericSelection()
        Dim lrProject As New ClientServer.Project
        Dim lsWhereClause As String = ""
        Dim lsMessage As String = ""

        If prApplication.User.IsSuperuser Then
            'Can edit any Project
        Else
            If tableClientServerProject.GetProjectCountByCreatedByUser(prApplication.User) = 0 _
                And tableClientServerProjectUser.getProjectCountByAllocatedUser(prApplication.User) = 0 Then
                lsMessage = "You have not created any Projects to edit, or you are not included in any Projects.".AppendString(vbCrLf).AppendString(vbCrLf)
                lsMessage.AppendString("You can only edit/view Projects that you have created or in which you are included.")

                MsgBox(lsMessage)
                Exit Sub
            Else
                'Can only edit the Projects that the User has created, or that they are included in (ReadOnly)
                lsWhereClause = " WHERE CreatedByUserId = '" & Trim(prApplication.User.Id) & "'"
                If prApplication.User.Function.Contains(pcenumFunction.ReadProjectsIncludedIn) Then
                    lsWhereClause &= " OR Id IN (SELECT ProjectId"
                    lsWhereClause &= "              FROM ClientServerProjectUser"
                    lsWhereClause &= "             WHERE ProjectId = Id"
                    lsWhereClause &= "               AND UserId = '" & prApplication.User.Id & "')"
                End If
            End If
        End If

        If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "Projects",
                                               "ClientServerProject",
                                               "ProjectName",
                                               "Id",
                                               lsWhereClause) = Windows.Forms.DialogResult.OK Then

            lrProject.Id = lrGenericSelection.SelectIndex
            Call tableClientServerProject.getProjectDetailsById(lrProject.Id, lrProject)

            Call Me.LoadCRUDEditProject(lrProject)
        End If

    End Sub

    Private Sub AddNamespaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNamespaceToolStripMenuItem.Click

        Dim lsMessage As String = ""

        Try
            If prApplication.User Is Nothing Then
                lsMessage = "You must be logged in to add a Namespace."
                MsgBox(lsMessage)
            ElseIf (tableClientServerProject.getProjectCount = 0) Then
                lsMessage = "There must be at least one Project before you can create a Namespace"
                MsgBox(lsMessage)
            ElseIf Not prApplication.User.IsSuperuser Then
                If tableClientServerProjectUser.getProjectCountByAllocatedUser(prApplication.User) = 0 Then
                    lsMessage = "You are not allocated to any Projects"
                    lsMessage &= vbCrLf & vbCrLf
                    lsMessage &= "You must be allocated to at least one Project before adding a Namespace."
                    MsgBox(lsMessage)
                Else
                    Call Me.LoadCRUDAddNamespace()
                End If
            Else
                Call Me.LoadCRUDAddNamespace()
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub EditNamespaceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditNamespaceToolStripMenuItem.Click

        Dim lsMessage As String = ""

        Dim lrGenericSelection As New tGenericSelection()
        Dim lrNamespace As New ClientServer.Namespace

        Try
            If prApplication.User Is Nothing Then
                lsMessage = "You must log in to edit a Namespace"
                MsgBox(lsMessage)
            ElseIf Not prApplication.User.IsLoggedIn Then
                lsMessage = "You must log in to edit a Namespace"
                MsgBox(lsMessage)

            ElseIf tableClientServerNamespace.getNamespaceCount = 0 Then
                MsgBox("There are no Namespaces to edit.", MsgBoxStyle.Information)

            ElseIf (tableClientServerProjectUser.getProjectCountByAllocatedUser(prApplication.User) = 0) And
                   Not prApplication.User.IsSuperuser Then

                lsMessage = "You are not allocated to any Project."
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "You must be allocated to at least one Project before you can edit a Namespace."

                MsgBox(lsMessage, MsgBoxStyle.Information)
            Else

                Dim lsWhereClause As String = ""

                'A User can only edit a Namespace that is for a Project to which the User is related/allocated
                If prApplication.User.IsSuperuser Then
                    lsWhereClause = ""
                Else
                    lsWhereClause = " WHERE Id IN (SELECT Id FROM ClientServerNamespace WHERE ProjectId IN (SELECT ProjectId FROM ClientServerProjectUser WHERE UserId = '" & prApplication.User.Id & "'))"
                End If

                If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                           "Namespaces",
                                           "ClientServerNamespace",
                                           "Namespace",
                                           "Id",
                                           lsWhereClause) = Windows.Forms.DialogResult.OK Then

                    lrNamespace.Id = lrGenericSelection.SelectIndex
                    Call tableClientServerNamespace.getNamespaceDetailsById(lrNamespace.Id, lrNamespace)

                    Call Me.LoadCRUDEditNamespace(lrNamespace)
                End If
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemAddGroup_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddGroup.Click

        Call Me.LoadCRUDAddGroup()

    End Sub

    Private Sub ToolStripMenuItemEditGroup_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditGroup.Click

        Dim lrGenericSelection As New tGenericSelection()
        Dim lrGroup As New ClientServer.Group

        Dim lsWhereClause As String = ""

        If prApplication.User.IsSuperuser Then
            lsWhereClause = "" 'Superuser can edit any Group.
        Else
            lsWhereClause = "WHERE Id IN (SELECT Id " 'Created the Group
            lsWhereClause &= "              FROM ClientServerGroup"
            lsWhereClause &= "             WHERE CreatedByUserId = '" & prApplication.User.Id & "'"
            lsWhereClause &= "            )"
            lsWhereClause &= " OR Id IN (SELECT GroupId" 'Is a member of the Group
            lsWhereClause &= "             FROM ClientServerGroupUser"
            lsWhereClause &= "             WHERE UserId = '" & prApplication.User.Id & "'"
            lsWhereClause &= "            )"
        End If

        If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "Groups",
                                               "ClientServerGroup",
                                               "GroupName",
                                               "Id",
                                               lsWhereClause) = Windows.Forms.DialogResult.OK Then

            lrGroup.Id = lrGenericSelection.SelectIndex
            Call tableClientServerGroup.getGroupDetailsById(lrGroup.Id, lrGroup)

            Call Me.LoadCRUDEditGroup(lrGroup)
        End If

    End Sub

    Private Sub AddRoleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddRoleToolStripMenuItem.Click

        Call Me.LoadCRUDAddRole()

    End Sub

    Private Sub EditRoleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditRoleToolStripMenuItem.Click

        Dim lrGenericSelection As New tGenericSelection()
        Dim lrRole As New ClientServer.Role
        Dim lsMessage As String = ""

        Try
            '------------------------------------------------------------------
            'Check Credentials of the User
            If (prApplication.User Is Nothing) Then
                lsMessage = "You must log in before you can edit a Role."
                Throw New Exception(lsMessage)
            ElseIf prApplication.User.IsLoggedIn = False Then
                lsMessage = "You must log in before you can edit a Role."
                Throw New Exception(lsMessage)
            End If


            If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                                   "Roles",
                                                   "ClientServerRole",
                                                   "RoleName",
                                                   "Id") = Windows.Forms.DialogResult.OK Then

                lrRole.Id = lrGenericSelection.SelectIndex
                Call tableClientServerRole.getRoleDetailsById(lrRole.Id, lrRole)

                Call Me.LoadCRUDEditRole(lrRole)
            End If


        Catch ex As Exception
            prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical)
        End Try

    End Sub

    Private Sub LogOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogOutToolStripMenuItem.Click

        prApplication.User.IsLoggedIn = False

        Me.ToolStripMenuItemUser.Visible = True
        Me.ToolStripMenuItemProject.Visible = True

    End Sub

    Private Sub ToolStripMenuItemLogOut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemLogOut.Click

        Try
            '-------------------------------------------------------------------------------------------------------
            'Main Client/Server menu items
            If prApplication.User.IsSuperuser Or prApplication.User.Function.Contains(pcenumFunction.FullPermission) Then
                Me.ToolStripMenuItemUser.Visible = False
                Me.ToolStripMenuItemProject.Visible = False
            End If

            '------------------------------------------------------------------------------------------------------
            'Toggle LogIn/LogOut menu items
            Me.ToolStripMenuItemLogOut.Visible = False
            Me.ToolStripMenuItemLogIn.Visible = True

            prApplication.User.IsLoggedIn = False

            '-------------------------------------
            'Display the FlashCard to show that the User has logged out
            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.zsText = "Successfully logged out."
            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)

            If My.Settings.LoggingOutEndsSession Then

                If IsSomething(zfrmModelExplorer) Then
                    Me.zfrmModelExplorer.zoRecentNodes.Serialize(Me.zfrmModelExplorer.zsRecentNodesFileName)
                End If

                Me.Hide()
                Me.Close()
                Application.Exit()
                Environment.Exit(0)

            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub logInUser(ByRef arUser As ClientServer.User)

        Try
            If prApplication.User Is Nothing Then
                Call tableClientServerUser.getUserDetailsByUsername(arUser.Username, arUser)
                prApplication.User = New ClientServer.User
                prApplication.User = arUser
            End If

            Call publicModule.LoginUser(arUser)

            Dim lrLogEntry As New ClientServer.Log
            lrLogEntry.DateTime = Now
            lrLogEntry.LogType = pcenumLogType.LogIn
            lrLogEntry.User = arUser

            If My.Settings.UseVirtualUI Then
                If prThinfinity.BrowserInfo Is Nothing Then
                    Dim lsMessage = "Friendly message: 'UseVirtualUI' is configured to 'True', but it seems that you are not running Boston through a browser."
                    lsMessage &= vbCrLf & vbCrLf & "If you are running Boston through a browser, please contact FactEngine."
                    MsgBox(lsMessage)
                Else
                    Try
                        lrLogEntry.IPAddress = prThinfinity.BrowserInfo.IPAddress
                    Catch ex As Exception
                        Dim lsMessage = "Friendly message: 'UseVirtualUI' is configured to 'True', but it seems that you are not running Boston through a browser."
                        lsMessage &= vbCrLf & vbCrLf & "If you are running Boston through a browser, please contact FactEngine."
                        MsgBox(lsMessage)
                    End Try
                End If
            Else
                lrLogEntry.IPAddress = "NOTHING"
            End If

            Call tableClientServerLog.AddLogEntry(lrLogEntry)

            '-------------------------------------
            'Display the FlashCard to show that the User has logged in
            Dim lfrmFlashCard As New frmFlashCard
            lfrmFlashCard.zsText = "Welcome to Boston!"
            lfrmFlashCard.resizeToText()
            Dim liDialogResult As DialogResult = lfrmFlashCard.ShowDialog(Me)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub DoNotificationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DoNotificationToolStripMenuItem.Click

        Dim loNotification As New CharmNotification.Notification
        loNotification.Title = "Hellow World"
        loNotification.Text = "Testing this notofication stuff"
        loNotification.Duration = 5000
        loNotification.ShowNotification()

    End Sub

    Private Sub Button2_Click(ByVal asProjectId As String)

        MsgBox(asProjectId)

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButtonNotifications.Click

        If Me.zfrmNotifications Is Nothing Then
            Dim lfrmNotifications = New frmNotifications

            'lfrmNotifications.TopMost = True

            lfrmNotifications.Location = New Point(Me.ToolStripButtonNotifications.Bounds.Left, Me.ToolStrip_main.Bounds.Bottom + Me.ToolStrip_main.Height)

            lfrmNotifications.Owner = Me
            lfrmNotifications.Show()

            Me.zfrmNotifications = lfrmNotifications

            Call Me.LoadInvitationsGroupToJoinProject()
            Call Me.LoadNotificationsGeneral()
            Me.Focus()
        Else

            Me.zfrmNotifications.Hide()
            Me.zfrmNotifications.Dispose()
            Me.zfrmNotifications = Nothing
        End If

    End Sub

    Private Sub DeclineGroupToProjectInvitation(ByVal arInvitation As Object)

        Dim lrInvitation As New ClientServer.Invitation

        lrInvitation = CType(arInvitation, ClientServer.Invitation)

        lrInvitation.Accepted = False
        lrInvitation.Closed = True

        Call tableClientServerInvitation.closeInvitation(lrInvitation) 'Key: InvitationType, DateTime, InvitingUser, InvitedUser

    End Sub

    Private Sub TimerNotifications_Tick(sender As Object, e As EventArgs) Handles TimerNotifications.Tick

        If Me.TimerNotifications.Interval < 60000 Then
            Me.TimerNotifications.Interval = 60000
        End If

        If Not My.Settings.UseClientServer Then
            Me.TimerNotifications.Stop()
            Exit Sub
        End If

        If prApplication.User Is Nothing Then Exit Sub

        If Me.zfrmNotifications Is Nothing Then

            If Me.ToolStripButtonNotifications.Visible = True Then
                'The User already knows that they have Notifications.
                'When the User clicks on the [You have notifications] button, the Notifications will be loaded.
            Else
                'Get any Notifications and display them to the User if there is any.
                Dim lfrmNotifications = New frmNotifications

                'lfrmNotifications.TopMost = True

                lfrmNotifications.Owner = Me

                Me.zfrmNotifications = lfrmNotifications

                '=============================================================================
                'Load the Notifications
                Call Me.LoadInvitationsGroupToJoinProject()
                Call Me.LoadNotificationsGeneral()

                If lfrmNotifications.FlexibleListBox.Count > 0 Then
                    Me.ToolStripButtonNotifications.Visible = True
                    lfrmNotifications.Location = New Point(Me.ToolStripButtonNotifications.Bounds.Left, Me.ToolStrip_main.Bounds.Bottom + Me.ToolStrip_main.Height)
                    lfrmNotifications.Show()
                Else
                    Me.zfrmNotifications = Nothing
                    Me.ToolStripButtonNotifications.Visible = False
                    lfrmNotifications.Dispose()
                End If
            End If
        Else

            Me.zfrmNotifications.Hide()
            Me.zfrmNotifications.Dispose()
            Me.zfrmNotifications = Nothing

            Exit Sub
        End If

    End Sub

    Private Sub LoadInvitationsGroupToJoinProject()

        Dim larInvitations As New List(Of ClientServer.Invitation)
        Dim lrGroup As ClientServer.Group

        For Each lrGroup In tableClientServerGroupUser.GetGroupsCreatedByUser(prApplication.User)

            larInvitations = tableClientServerInvitation.getOpenInvitationsForGroupByType(lrGroup, pcenumInvitationType.GroupToJoinProject)

            For Each lrInvitation In larInvitations

                Dim loNotification As New NotificationBarGroupToProjectInvitation

                loNotification.LabelPromptDetails.Text = "As the creator of the Group, " & lrGroup.Name & ","
                loNotification.LabelPromptDetails.Text.AppendString(" you have been invited to include members of the '" & lrGroup.Name & "' Group")
                loNotification.LabelPromptDetails.Text.AppendString(" within the Project, '" & lrInvitation.InvitedToJoinProject.Name & "'. Do you accept this invitation on behalf of the Group?")

                loNotification.Tag = lrInvitation

                Me.zfrmNotifications.FlexibleListBox.Add(loNotification)

                Me.zfrmNotifications.Width = loNotification.Width + 10

                loNotification.Show()

                AddHandler loNotification.InvitationGroupToProjectDeclined, AddressOf HandleDeclineInvitationGroupToJoinProject
                AddHandler loNotification.InvitationGroupToProjectAccepted, AddressOf HandleAcceptInvitationGroupToJoinProject
            Next
        Next

    End Sub

    Private Sub LoadNotificationsGeneral()

        Dim larNotificationsGeneral As New List(Of ClientServer.NotificationGeneral)

        larNotificationsGeneral = tableClientServerNotifications.getOpenNotificationsByUserByType(prApplication.User, pcenumNotificationType.GeneralNotification)

        For Each lrNotification In larNotificationsGeneral

            Dim loNotification As New NotificationBarGeneralNotification

            loNotification.LabelNotificationText.Text = lrNotification.Text
            loNotification.Tag = lrNotification

            Me.zfrmNotifications.FlexibleListBox.Add(loNotification)
            Me.zfrmNotifications.Width = loNotification.Width + 10

            loNotification.Show()

            AddHandler loNotification.GotitClicked, AddressOf HandleNotificationGeneralGotit
        Next

    End Sub

    Public Sub HandleNotificationGeneralGotit(ByRef arNotification As ClientServer.NotificationGeneral)

        Call tableClientServerNotifications.closeNotification(arNotification)

        If Me.zfrmNotifications.FlexibleListBox.Count = 0 Then
            'Last Notification, so close the Notification form
            Me.zfrmNotifications.Hide()
            Me.zfrmNotifications.Dispose()
            Me.zfrmNotifications = Nothing
            Me.ToolStripButtonNotifications.Visible = False
        End If

    End Sub

    Private Sub HandleDeclineInvitationGroupToJoinProject(ByRef arObject As Object)

        Dim lrInvitation As ClientServer.Invitation

        lrInvitation = CType(arObject, ClientServer.Invitation)

        lrInvitation.Closed = True
        lrInvitation.Accepted = False

        Call tableClientServerInvitation.closeInvitation(lrInvitation)

    End Sub

    Private Sub HandleAcceptInvitationGroupToJoinProject(ByRef arObject As Object)

        Try
            Dim lrInvitation As ClientServer.Invitation

            lrInvitation = CType(arObject, ClientServer.Invitation)

            lrInvitation.Closed = True
            lrInvitation.Accepted = True

            pdbConnection.BeginTrans()
            Call tableClientServerProjectGroup.AddGroupToProject(lrInvitation.InvitedGroup, lrInvitation.InvitedToJoinProject)
            Call lrInvitation.InvitedGroup.IncludeAllUsersInProject(lrInvitation.InvitedToJoinProject)
            Call tableClientServerInvitation.closeInvitation(lrInvitation)

            Dim lrComboboxItem As tComboboxItem
            Dim larProject As New List(Of ClientServer.Project)
            If Me.zfrmModelExplorer IsNot Nothing Then
                For Each lrComboboxItem In Me.zfrmModelExplorer.ComboBoxProject.Items
                    larProject.Add(lrComboboxItem.Tag)
                Next
            End If

            If Not larProject.Contains(lrInvitation.InvitedToJoinProject) Then
                Me.zfrmModelExplorer.zbLoadingProjects = True
                lrComboboxItem = New tComboboxItem(lrInvitation.InvitedToJoinProject.Id,
                                                   lrInvitation.InvitedToJoinProject.Name,
                                                   lrInvitation.InvitedToJoinProject)
                Me.zfrmModelExplorer.ComboBoxProject.Items.Add(lrComboboxItem)
                Me.zfrmModelExplorer.zbLoadingProjects = False
            End If

            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ShowBroadcastEventMonitorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowBroadcastEventMonitorToolStripMenuItem.Click

        Try
            Dim lfrmBroadcastEventMonitor As New frmBroadcastEventMonitor
            lfrmBroadcastEventMonitor.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemEdit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEdit.Click

        '--------------------------------------------------------------
        'Check to see if there is a Page in the Clipboard for Pasting
        '--------------------------------------------------------------
        Dim lrClipboardPage As New FBM.Page 'Clipbrd.ClipboardPage
        Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")
        Try
            If Clipboard.ContainsData(RichmondPage.Name) Then
                Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()
                lrClipboardPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page) ' Clipbrd.ClipboardPage)
                If IsSomething(lrClipboardPage) Then
                    Me.PasteToolStripMenuItem.Enabled = True
                Else
                    Me.PasteToolStripMenuItem.Enabled = False
                End If
            End If
        Catch ex As Exception
            'Oh well, tried
        End Try

    End Sub

    Public Sub resizeModelExplorer()

        'Set the width of the form based on the widest node and as a proportion of the main form's width.
        Dim liMaxTextLength As Integer = 0
        Dim lsMaxString As String = ""
        If Me.zfrmModelExplorer.TreeView.Nodes(0).Nodes IsNot Nothing Then
            For Each lrNode As TreeNode In Me.zfrmModelExplorer.TreeView.Nodes(0).Nodes
                If lrNode.Text.Length > liMaxTextLength Then
                    liMaxTextLength = lrNode.Text.Length
                    lsMaxString = lrNode.Text
                End If
                liMaxTextLength = Math.Max(liMaxTextLength, lrNode.Text.Length)
            Next
        End If

        Dim G As Graphics
        G = Me.CreateGraphics

        Dim liNewSize As SizeF = G.MeasureString(lsMaxString, New Font("Microsoft Sans Serif", 8.25)) 'ptMe.zfrmModelExplorer.TreeView.Font)

        Me.DockPanel.DockLeftPortion = Me.Size.Width / 6 '((liNewSize.Width + 150) / Me.Size.Width)
    End Sub

    Private Sub NewModelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemNewModel.Click

        If Me.zfrmModelExplorer IsNot Nothing Then

            Call Me.zfrmModelExplorer.addNewModelToBoston()
        Else
            MsgBox("Please check that the Model Explorer view is open before adding a new model to Boston.")
        End If
    End Sub

    Private Sub ToolStripButtonNewModel_Click(sender As Object, e As EventArgs) Handles ToolStripButtonNewModel.Click

        If Me.zfrmModelExplorer IsNot Nothing Then
            Cursor.Current = Cursors.WaitCursor
            Call Me.zfrmModelExplorer.addNewModelToBoston()
            Cursor.Current = Cursors.Default
        End If

    End Sub

    Public Sub loadStartupPage()

        Dim lrChildForm As New frmStartup
        lrChildForm.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)
        Me.zfrmStartup = lrChildForm

    End Sub

    Private Sub ToolStripButton1_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        If Me.zfrmModelExplorer Is Nothing Then
            Call Me.LoadEnterpriseTreeViewer()
        End If

        If Me.zfrmStartup Is Nothing Then
            Call Me.loadStartupPage()
        Else
            Call Me.zfrmStartup.Show()
        End If

        Dim lsApplicationPath As String = Boston.MyPath
        Me.zfrmStartup.WebBrowser.Navigate("file:\\" & lsApplicationPath & "\startup\index.html")

    End Sub

    Private Sub CodeGeneratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemCodeGenerator.Click

        If prApplication.SoftwareCategory = pcenumSoftwareCategory.Student Then
            Dim lsMessage = "Please upgrade to Boston Professional for code generation."
            MsgBox(lsMessage)
        Else
            If Me.zfrmModelExplorer Is Nothing Then
                Call Me.LoadEnterpriseTreeViewer()
                Call Me.loadCodeGenerator()
            Else
                Call Me.loadCodeGenerator()
            End If
        End If

    End Sub

    Private Sub zfrmModelExplorer_Disposed(sender As Object, e As EventArgs) Handles zfrmModelExplorer.Disposed
        GC.Collect()
    End Sub

    Private Sub ToolStripMenuItemFactEngine_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFactEngine.Click

        If prApplication.WorkingModel Is Nothing Then
            MsgBox("Create and load a model before loading FactEngine.")
        Else
            Call Me.LoadFactEngine()
        End If


    End Sub

    Private Sub ConfigurationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigurationToolStripMenuItem.Click

        Call Me.LoadCRUDRichmondConfiguration()

    End Sub


    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

        Try

#Region "Open ModelExplorer"

            If Me.MenuItem_ShowEnterpriseTreeView.Checked Then
                If IsSomething(zfrmModelExplorer) Then
                    '---------------------------------------
                    'Enterprise TreeView is already loaded
                    '---------------------------------------
                Else
                    Call LoadEnterpriseTreeViewer()
                End If
            Else
                '------------------------------------------------
                'EnterpriseTreeViewer is not meant to be open
                '------------------------------------------------
                If IsSomething(zfrmModelExplorer) Then
                    'Nothing to do here.
                Else
                    Call LoadEnterpriseTreeViewer()
                End If
                Me.MenuItem_ShowEnterpriseTreeView.Checked = True
            End If
#End Region

            Call zfrmModelExplorer.ImportFBMXMLFile()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CompactAndRepairToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CompactAndRepairToolStripMenuItem.Click

        Try
            With New WaitCursor
                Try
                    pdbConnection.Close()
                    pdb_OLEDB_connection.Close()
                Catch
                End Try

                Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

                Dim lsDatabaseLocationName As String = lrSQLConnectionStringBuilder("Data Source")

                Dim lsCompactedDatabaseLocationName As String

                lsCompactedDatabaseLocationName = New FileInfo(lsDatabaseLocationName).DirectoryName & "\BostonCompacted.vdb"

                Call Boston.CompactAccessDB(lsDatabaseLocationName, lsCompactedDatabaseLocationName)

                Call Boston.OpenDatabase()
            End With

            MsgBox("The Boston database has successfully been compacted and repaired.")

        Catch ex As Exception
            MsgBox("Failed to compact and repair the Boston database. Please contact support.")
        End Try

    End Sub

    Public Sub SetGlobalsToNothing()

        Try
            Me.zfrmModelExplorer = Nothing
            Me.zfrm_toolbox = Nothing
            Me.zfrm_diagram_overview = Nothing
            Me.zfrm_orm_reading_editor = Nothing
            Me.zfrm_properties = Nothing
            Me.zfrm_KL_theorem_writer = Nothing
            Me.zrORMModel_view = Nothing
            Me.zfrm_Brain_box = Nothing
            Me.zfrmFactEngine = Nothing
            Me.zfrmStartup = Nothing
            Me.zfrmCRUDAddGroup = Nothing
            Me.zfrmCRUDAddNamespace = Nothing
            Me.zfrmCRUDAddProject = Nothing
            Me.zfrmCRUDAddRole = Nothing
            Me.zfrmCRUDAddUser = Nothing
            Me.zfrmCRUDBostonConfiguration = Nothing
            Me.zfrmCRUDEditGroup = Nothing
            Me.zfrmCRUDEditNamespace = Nothing
            Me.zfrmCRUDEditProject = Nothing
            Me.zfrmCRUDEditRole = Nothing
            Me.zfrmCRUDEditUser = Nothing
            Me.zfrm_ER_diagram_view = Nothing
            Me.zfrm_PGS_diagram_view = Nothing
            Me.zfrmStateTransitionDiagramView = Nothing
            Me.zfrmNotifications = Nothing
            Me.zfrmCodeGenerator = Nothing

            prApplication.DatabaseVersionNr = prApplication.DatabaseVersionNr

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemUnifiedOntologyBrowser_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemUnifiedOntologyBrowser.Click

        Try
            Dim lrGenericSelection As New tGenericSelection()
            Dim lrUnifiedOntology As New Ontology.UnifiedOntology
            Dim lsWhereClause As String = ""

            If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "Unified Ontology",
                                               "UnifiedOntology",
                                               "UnifiedOntologyName",
                                               "Id",
                                               lsWhereClause,
                                               Nothing,
                                               pcenumComboBoxStyle.DropdownList,
                                               "1",
                                               1,
                                               "120",
                                               "Unified Ontology Name") = Windows.Forms.DialogResult.OK Then


                With New WaitCursor


                    lrUnifiedOntology.Id = lrGenericSelection.SelectValue
                    Call TableUnifiedOntology.GetUnifiedOntologyDetails(lrUnifiedOntology)

                    Call Me.LoadUnifiedOntologyBrowser(lrUnifiedOntology, Nothing)
                End With

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub RegistrationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistrationToolStripMenuItem.Click

        frmRegistration.ShowDialog()

    End Sub

    Private Sub ShowClientServerBroadcastTesterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowClientServerBroadcastTesterToolStripMenuItem.Click

        Try
            Dim lfrmToolboxClientServerBroadcastTester As New frmToolboxClientServerBroadcastTester
            lfrmToolboxClientServerBroadcastTester.Show(Me.DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ThrowTestErrorMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThrowTestErrorMessageToolStripMenuItem.Click

        Try
            Dim lrModel As New FBM.Model
            lrModel.EntityType = Nothing
            lrModel.EntityType(0).Model = lrModel

            Throw New Exception("Test Error Message thrown by user in Superuser mode.")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemAddUnifiedOntology_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddUnifiedOntology.Click

        Try
            Call frmCRUDAddUnifiedOntology.ShowDialog()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemEditUnifiedOntology_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditUnifiedOntology.Click

        Try
            Dim lrGenericSelection As New tGenericSelection()

            If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "Unified Ontology",
                                               "UnifiedOntology",
                                               "UnifiedOntologyName",
                                               "Id",
                                               Nothing,
                                               Nothing,
                                               pcenumComboBoxStyle.DropdownList,
                                               "1",
                                               1,
                                               "120",
                                               "Unified Ontology Name") = Windows.Forms.DialogResult.OK Then


                Dim lfrmEditUnfiedOntology As New frmCRUDEditUnifiedOntology

                Dim lrUnifiedOntology As New Ontology.UnifiedOntology
                lrUnifiedOntology.Id = lrGenericSelection.SelectIndex
                Call TableUnifiedOntology.GetUnifiedOntologyDetails(lrUnifiedOntology)
                lfrmEditUnfiedOntology.moUnifiedOntology = lrUnifiedOntology
                Call lfrmEditUnfiedOntology.ShowDialog()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub EditConfigurationDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemEditConfigurationData.Click

        Try
            Dim lfrmCRUDReferenceData As New frmCRUDEditReferenceTable
            Call lfrmCRUDReferenceData.ShowDialog()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BroadcastMessageToUsersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BroadcastMessageToUsersToolStripMenuItem.Click

        Try
            Dim lrClientServerMessageBroadcaster As New frmClientServerMessageBroadcast
            Call lrClientServerMessageBroadcaster.ShowDialog()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub BackgroundWorkerStatusBar_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorkerStatusBar.ProgressChanged

        Try
            Call Boston.WriteToProgressBar(e.ProgressPercentage)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemLogInAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemLogInAs.Click

        Try
            Dim lrGenericSelection As New tGenericSelection()
            Dim lrUser As New ClientServer.User

            Dim lsWhereClause As String = ""

            If Boston.DisplayGenericSelectForm(lrGenericSelection,
                                               "User",
                                               "ClientServerUser",
                                               "FirstName & ' ' & LastName AS Name, Username",
                                               "Username",
                                               lsWhereClause,
                                               Nothing,
                                               pcenumComboBoxStyle.DropdownList,
                                               "1,2",
                                               2,
                                               "120;120",
                                               "Name,Username") = Windows.Forms.DialogResult.OK Then


                lrUser.Username = lrGenericSelection.SelectValue
                Call tableClientServerUser.getUserDetailsByUsername(lrUser.Username, lrUser)

                If frmLogin.ShowDialog() = Windows.Forms.DialogResult.OK Then

                    prApplication.User = lrUser

                    Call Me.SaveAllModels()

                    If Me.zfrmModelExplorer IsNot Nothing Then
                        Call Me.zfrmModelExplorer.Close()
                    End If

                    Call Me.logInUser(lrUser)
                    Call Me.SetupForm()

                End If



            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class
