<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then
                    components.Dispose()
                End If

                If prDuplexServiceClient IsNot Nothing Then
                    Try
                        prDuplexServiceClient.Disconnect()
                        prDuplexServiceClient.Close()
                    Catch
                        prDuplexServiceClient.Abort()
                    End Try
                End If
            End If

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.MenuStrip_main = New System.Windows.Forms.MenuStrip()
        Me.mnu_Session = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemNewModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRecentNodes = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemLogIn = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemLogInAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemLogOut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOption_EndSession = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemUndo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRedo = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyAsImageToClipboardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemView = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItem_ShowEnterpriseTreeView = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCodeGenerator = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemFactEngine = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StandardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolboxesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemToolbox = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDiagramOverview = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemKLTheoremWriter = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConceptClassificationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VirtualAnalystToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DescriptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QueryEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelViewsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RelationalManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GraphSchemaManagerToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusBarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemDiagramSpy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemUnifiedOntologyBrowser = New System.Windows.Forms.ToolStripMenuItem()
        Me.JupyterNotebookToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChatOpenAI = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDocumentSearch = New System.Windows.Forms.ToolStripMenuItem()
        Me.DocumentDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MyTasksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemBoston = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.PluginViewerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.DatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CompactAndRepairToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveUnneededConceptsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteLogFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RegistrationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEditConfigurationData = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAddUser = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditUserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.GroupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAddGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEditGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRole = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddRoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditRoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.LogOutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemProject = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.NamespaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddNamespaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditNamespaceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemUnifiedOntology = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemAddUnifiedOntology = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEditUnifiedOntology = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemTestClientServer = New System.Windows.Forms.ToolStripMenuItem()
        Me.BroadcastMessageToUsersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowBroadcastEventMonitorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowClientServerBroadcastTesterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemOSM = New System.Windows.Forms.ToolStripMenuItem()
        Me.TaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddTaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditTaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteTaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImportExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportTaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportTaskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenAIFunctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddOpenAIFunctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditOpenAIFunctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteOpenAIFunctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TheBoxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestNotificationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DoNotificationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSuperuser = New System.Windows.Forms.ToolStripMenuItem()
        Me.ThrowTestErrorMessageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenericTestFormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEnterprise = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnterpriseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_AddEnterprise = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditEnterpriseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteEnterpriseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem15 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem16 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem17 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSubjectArea = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem18 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem19 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem20 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SolutionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddSolutionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditSolutionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteSolutionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemApplication = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteApplicationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ApplicationMenuEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplicationRunnerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemTesting = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestSetManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutRichmondToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemOpenLogFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.EmailSupportvievcomToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStrip_main = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton_Save = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonNewModel = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonNew = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonPrint = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonCopy = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonPaste = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripLabelPrompt_zoom = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBox_zoom = New System.Windows.Forms.ToolStripComboBox()
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonHelp = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonProfile = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButtonNotifications = New System.Windows.Forms.ToolStripButton()
        Me.StatusBar_main = New System.Windows.Forms.StatusStrip()
        Me.StatusLabelGeneralStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelUsername = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabelClientServer = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelPromptEvaluationTime = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelPromptEvaluationTimeSeconds = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ContextMenuStrip_Project = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SetProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DockPanel = New WeifenLuo.WinFormsUI.Docking.DockPanel()
        Me.HelpProvider = New System.Windows.Forms.HelpProvider()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TimerNotifications = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorkerStatusBar = New System.ComponentModel.BackgroundWorker()
        Me.TimerGFSBackup = New System.Windows.Forms.Timer(Me.components)
        Me.TimerDemo = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip_main.SuspendLayout()
        Me.ToolStrip_main.SuspendLayout()
        Me.StatusBar_main.SuspendLayout()
        Me.ContextMenuStrip_Project.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip_main
        '
        Me.MenuStrip_main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnu_Session, Me.ToolStripMenuItemPage, Me.ToolStripMenuItemEdit, Me.ToolStripMenuItemView, Me.ToolStripMenuItemBoston, Me.ToolStripMenuItemUser, Me.ToolStripMenuItemProject, Me.ToolStripMenuItemUnifiedOntology, Me.ToolStripMenuItemTestClientServer, Me.ToolStripMenuItemOSM, Me.TestNotificationToolStripMenuItem, Me.ToolStripMenuItemSuperuser, Me.ToolStripMenuItemEnterprise, Me.ToolStripMenuItemApplication, Me.ToolStripMenuItemTesting, Me.HelpToolStripMenuItem})
        Me.MenuStrip_main.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip_main.Name = "MenuStrip_main"
        Me.MenuStrip_main.Padding = New System.Windows.Forms.Padding(4, 1, 0, 1)
        Me.MenuStrip_main.Size = New System.Drawing.Size(1066, 24)
        Me.MenuStrip_main.TabIndex = 8
        Me.MenuStrip_main.Text = "MenuStrip1"
        '
        'mnu_Session
        '
        Me.mnu_Session.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.SaveAllToolStripMenuItem, Me.ToolStripSeparator10, Me.ToolStripMenuItemNewModel, Me.ToolStripMenuItem2, Me.ToolStripSeparator5, Me.ToolStripMenuItemRecentNodes, Me.ToolStripSeparator11, Me.ToolStripMenuItemLogIn, Me.ToolStripMenuItemLogInAs, Me.ToolStripMenuItemLogOut, Me.ToolStripSeparator6, Me.mnuOption_EndSession})
        Me.mnu_Session.Name = "mnu_Session"
        Me.mnu_Session.Size = New System.Drawing.Size(58, 22)
        Me.mnu_Session.Text = "&Session"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Image = CType(resources.GetObject("SaveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.SaveToolStripMenuItem.Text = "&Save"
        '
        'SaveAllToolStripMenuItem
        '
        Me.SaveAllToolStripMenuItem.Image = CType(resources.GetObject("SaveAllToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SaveAllToolStripMenuItem.Name = "SaveAllToolStripMenuItem"
        Me.SaveAllToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.SaveAllToolStripMenuItem.Text = "Save &All"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(191, 6)
        '
        'ToolStripMenuItemNewModel
        '
        Me.ToolStripMenuItemNewModel.Image = CType(resources.GetObject("ToolStripMenuItemNewModel.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemNewModel.Name = "ToolStripMenuItemNewModel"
        Me.ToolStripMenuItemNewModel.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItemNewModel.Text = "&New Model"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Image = CType(resources.GetObject("ToolStripMenuItem2.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItem2.Text = "&Import .fbm File"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(191, 6)
        '
        'ToolStripMenuItemRecentNodes
        '
        Me.ToolStripMenuItemRecentNodes.Name = "ToolStripMenuItemRecentNodes"
        Me.ToolStripMenuItemRecentNodes.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItemRecentNodes.Text = "Recent Models / Pages"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(191, 6)
        '
        'ToolStripMenuItemLogIn
        '
        Me.ToolStripMenuItemLogIn.Name = "ToolStripMenuItemLogIn"
        Me.ToolStripMenuItemLogIn.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItemLogIn.Text = "&Log in"
        '
        'ToolStripMenuItemLogInAs
        '
        Me.ToolStripMenuItemLogInAs.Name = "ToolStripMenuItemLogInAs"
        Me.ToolStripMenuItemLogInAs.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItemLogInAs.Text = "Log in a&s ..."
        Me.ToolStripMenuItemLogInAs.Visible = False
        '
        'ToolStripMenuItemLogOut
        '
        Me.ToolStripMenuItemLogOut.Name = "ToolStripMenuItemLogOut"
        Me.ToolStripMenuItemLogOut.Size = New System.Drawing.Size(194, 22)
        Me.ToolStripMenuItemLogOut.Text = "Log &out"
        Me.ToolStripMenuItemLogOut.Visible = False
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(191, 6)
        '
        'mnuOption_EndSession
        '
        Me.mnuOption_EndSession.Name = "mnuOption_EndSession"
        Me.mnuOption_EndSession.Size = New System.Drawing.Size(194, 22)
        Me.mnuOption_EndSession.Text = "&End Session"
        '
        'ToolStripMenuItemPage
        '
        Me.ToolStripMenuItemPage.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripMenuItem, Me.PrintToolStripMenuItem})
        Me.ToolStripMenuItemPage.Name = "ToolStripMenuItemPage"
        Me.ToolStripMenuItemPage.Size = New System.Drawing.Size(45, 22)
        Me.ToolStripMenuItemPage.Text = "&Page"
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Image = CType(resources.GetObject("PrintPreviewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Image = CType(resources.GetObject("PrintToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.PrintToolStripMenuItem.Text = "&Print"
        '
        'ToolStripMenuItemEdit
        '
        Me.ToolStripMenuItemEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemCopy, Me.PasteToolStripMenuItem, Me.ToolStripMenuItemUndo, Me.ToolStripMenuItemRedo, Me.SelectAllToolStripMenuItem, Me.CopyAsImageToClipboardToolStripMenuItem})
        Me.ToolStripMenuItemEdit.Name = "ToolStripMenuItemEdit"
        Me.ToolStripMenuItemEdit.Size = New System.Drawing.Size(39, 22)
        Me.ToolStripMenuItemEdit.Text = "&Edit"
        '
        'ToolStripMenuItemCopy
        '
        Me.ToolStripMenuItemCopy.Image = CType(resources.GetObject("ToolStripMenuItemCopy.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy"
        Me.ToolStripMenuItemCopy.Size = New System.Drawing.Size(221, 22)
        Me.ToolStripMenuItemCopy.Text = "&Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Enabled = False
        Me.PasteToolStripMenuItem.Image = CType(resources.GetObject("PasteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.PasteToolStripMenuItem.Text = "&Paste"
        '
        'ToolStripMenuItemUndo
        '
        Me.ToolStripMenuItemUndo.Enabled = False
        Me.ToolStripMenuItemUndo.Image = CType(resources.GetObject("ToolStripMenuItemUndo.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemUndo.Name = "ToolStripMenuItemUndo"
        Me.ToolStripMenuItemUndo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Z), System.Windows.Forms.Keys)
        Me.ToolStripMenuItemUndo.Size = New System.Drawing.Size(221, 22)
        Me.ToolStripMenuItemUndo.Text = "&Undo"
        '
        'ToolStripMenuItemRedo
        '
        Me.ToolStripMenuItemRedo.Enabled = False
        Me.ToolStripMenuItemRedo.Image = CType(resources.GetObject("ToolStripMenuItemRedo.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemRedo.Name = "ToolStripMenuItemRedo"
        Me.ToolStripMenuItemRedo.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.ToolStripMenuItemRedo.Size = New System.Drawing.Size(221, 22)
        Me.ToolStripMenuItemRedo.Text = "&Redo"
        '
        'SelectAllToolStripMenuItem
        '
        Me.SelectAllToolStripMenuItem.Image = CType(resources.GetObject("SelectAllToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        Me.SelectAllToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.SelectAllToolStripMenuItem.Text = "Select &All"
        '
        'CopyAsImageToClipboardToolStripMenuItem
        '
        Me.CopyAsImageToClipboardToolStripMenuItem.Image = CType(resources.GetObject("CopyAsImageToClipboardToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyAsImageToClipboardToolStripMenuItem.Name = "CopyAsImageToClipboardToolStripMenuItem"
        Me.CopyAsImageToClipboardToolStripMenuItem.Size = New System.Drawing.Size(221, 22)
        Me.CopyAsImageToClipboardToolStripMenuItem.Text = "Copy as &Image to Clipboard"
        '
        'ToolStripMenuItemView
        '
        Me.ToolStripMenuItemView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItem_ShowEnterpriseTreeView, Me.ToolStripMenuItemCodeGenerator, Me.ToolStripMenuItemFactEngine, Me.ToolStripSeparator8, Me.ToolStripMenuItem11, Me.ToolboxesToolStripMenuItem, Me.ModelViewsToolStripMenuItem, Me.StatusBarToolStripMenuItem, Me.ToolStripSeparator1, Me.ToolStripMenuItemDiagramSpy, Me.ToolStripMenuItemUnifiedOntologyBrowser, Me.JupyterNotebookToolStripMenuItem, Me.ToolStripMenuItemChatOpenAI, Me.ToolStripMenuItemDocumentSearch, Me.DocumentDatabaseToolStripMenuItem, Me.MyTasksToolStripMenuItem})
        Me.ToolStripMenuItemView.Name = "ToolStripMenuItemView"
        Me.ToolStripMenuItemView.Size = New System.Drawing.Size(44, 22)
        Me.ToolStripMenuItemView.Text = "&View"
        '
        'MenuItem_ShowEnterpriseTreeView
        '
        Me.MenuItem_ShowEnterpriseTreeView.Name = "MenuItem_ShowEnterpriseTreeView"
        Me.MenuItem_ShowEnterpriseTreeView.Size = New System.Drawing.Size(210, 22)
        Me.MenuItem_ShowEnterpriseTreeView.Text = "&Model Explorer"
        '
        'ToolStripMenuItemCodeGenerator
        '
        Me.ToolStripMenuItemCodeGenerator.AutoSize = False
        Me.ToolStripMenuItemCodeGenerator.Image = CType(resources.GetObject("ToolStripMenuItemCodeGenerator.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCodeGenerator.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripMenuItemCodeGenerator.Name = "ToolStripMenuItemCodeGenerator"
        Me.ToolStripMenuItemCodeGenerator.Size = New System.Drawing.Size(188, 30)
        Me.ToolStripMenuItemCodeGenerator.Text = "&Code Generator"
        '
        'ToolStripMenuItemFactEngine
        '
        Me.ToolStripMenuItemFactEngine.Image = CType(resources.GetObject("ToolStripMenuItemFactEngine.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemFactEngine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripMenuItemFactEngine.Name = "ToolStripMenuItemFactEngine"
        Me.ToolStripMenuItemFactEngine.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemFactEngine.Text = "Fact Engine"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StandardToolStripMenuItem})
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        Me.ToolStripMenuItem11.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItem11.Text = "Toolbars"
        '
        'StandardToolStripMenuItem
        '
        Me.StandardToolStripMenuItem.Name = "StandardToolStripMenuItem"
        Me.StandardToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.StandardToolStripMenuItem.Text = "&Standard"
        '
        'ToolboxesToolStripMenuItem
        '
        Me.ToolboxesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemToolbox, Me.ToolStripMenuItemDiagramOverview, Me.ToolStripMenuItemKLTheoremWriter, Me.TestToolStripMenuItem, Me.ConceptClassificationToolStripMenuItem, Me.VirtualAnalystToolStripMenuItem, Me.DescriptionsToolStripMenuItem, Me.QueryEditorToolStripMenuItem})
        Me.ToolboxesToolStripMenuItem.Name = "ToolboxesToolStripMenuItem"
        Me.ToolboxesToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.ToolboxesToolStripMenuItem.Text = "Tool&boxes"
        '
        'ToolStripMenuItemToolbox
        '
        Me.ToolStripMenuItemToolbox.Image = CType(resources.GetObject("ToolStripMenuItemToolbox.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemToolbox.Name = "ToolStripMenuItemToolbox"
        Me.ToolStripMenuItemToolbox.Size = New System.Drawing.Size(192, 22)
        Me.ToolStripMenuItemToolbox.Text = "&Toolbox"
        '
        'ToolStripMenuItemDiagramOverview
        '
        Me.ToolStripMenuItemDiagramOverview.Name = "ToolStripMenuItemDiagramOverview"
        Me.ToolStripMenuItemDiagramOverview.Size = New System.Drawing.Size(192, 22)
        Me.ToolStripMenuItemDiagramOverview.Text = "Diagram &Overview"
        '
        'ToolStripMenuItemKLTheoremWriter
        '
        Me.ToolStripMenuItemKLTheoremWriter.Name = "ToolStripMenuItemKLTheoremWriter"
        Me.ToolStripMenuItemKLTheoremWriter.Size = New System.Drawing.Size(192, 22)
        Me.ToolStripMenuItemKLTheoremWriter.Text = "&KL Theorem Writer"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.Image = CType(resources.GetObject("TestToolStripMenuItem.Image"), System.Drawing.Image)
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.TestToolStripMenuItem.Text = "&Properties"
        '
        'ConceptClassificationToolStripMenuItem
        '
        Me.ConceptClassificationToolStripMenuItem.Name = "ConceptClassificationToolStripMenuItem"
        Me.ConceptClassificationToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.ConceptClassificationToolStripMenuItem.Text = "&Concept Classification"
        '
        'VirtualAnalystToolStripMenuItem
        '
        Me.VirtualAnalystToolStripMenuItem.Name = "VirtualAnalystToolStripMenuItem"
        Me.VirtualAnalystToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.VirtualAnalystToolStripMenuItem.Text = "&Virtual Analyst"
        '
        'DescriptionsToolStripMenuItem
        '
        Me.DescriptionsToolStripMenuItem.Name = "DescriptionsToolStripMenuItem"
        Me.DescriptionsToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.DescriptionsToolStripMenuItem.Text = "&Descriptions"
        '
        'QueryEditorToolStripMenuItem
        '
        Me.QueryEditorToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.TextEdit16x16
        Me.QueryEditorToolStripMenuItem.Name = "QueryEditorToolStripMenuItem"
        Me.QueryEditorToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.QueryEditorToolStripMenuItem.Text = "&Query Editor"
        '
        'ModelViewsToolStripMenuItem
        '
        Me.ModelViewsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RelationalManagerToolStripMenuItem, Me.GraphSchemaManagerToolStripMenuItem1})
        Me.ModelViewsToolStripMenuItem.Name = "ModelViewsToolStripMenuItem"
        Me.ModelViewsToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.ModelViewsToolStripMenuItem.Text = "Model &Views"
        '
        'RelationalManagerToolStripMenuItem
        '
        Me.RelationalManagerToolStripMenuItem.Name = "RelationalManagerToolStripMenuItem"
        Me.RelationalManagerToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.RelationalManagerToolStripMenuItem.Text = "&Relational Manager"
        '
        'GraphSchemaManagerToolStripMenuItem1
        '
        Me.GraphSchemaManagerToolStripMenuItem1.Name = "GraphSchemaManagerToolStripMenuItem1"
        Me.GraphSchemaManagerToolStripMenuItem1.Size = New System.Drawing.Size(201, 22)
        Me.GraphSchemaManagerToolStripMenuItem1.Text = "&Graph Schema Manager"
        '
        'StatusBarToolStripMenuItem
        '
        Me.StatusBarToolStripMenuItem.Name = "StatusBarToolStripMenuItem"
        Me.StatusBarToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.StatusBarToolStripMenuItem.Text = "&Status Bar"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItemDiagramSpy
        '
        Me.ToolStripMenuItemDiagramSpy.Image = CType(resources.GetObject("ToolStripMenuItemDiagramSpy.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemDiagramSpy.Name = "ToolStripMenuItemDiagramSpy"
        Me.ToolStripMenuItemDiagramSpy.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemDiagramSpy.Text = "&Diagram Spy"
        '
        'ToolStripMenuItemUnifiedOntologyBrowser
        '
        Me.ToolStripMenuItemUnifiedOntologyBrowser.Image = CType(resources.GetObject("ToolStripMenuItemUnifiedOntologyBrowser.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemUnifiedOntologyBrowser.Name = "ToolStripMenuItemUnifiedOntologyBrowser"
        Me.ToolStripMenuItemUnifiedOntologyBrowser.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemUnifiedOntologyBrowser.Text = "&Unified Ontology Browser"
        '
        'JupyterNotebookToolStripMenuItem
        '
        Me.JupyterNotebookToolStripMenuItem.Name = "JupyterNotebookToolStripMenuItem"
        Me.JupyterNotebookToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.JupyterNotebookToolStripMenuItem.Text = "Jupyter Notebook"
        '
        'ToolStripMenuItemChatOpenAI
        '
        Me.ToolStripMenuItemChatOpenAI.Name = "ToolStripMenuItemChatOpenAI"
        Me.ToolStripMenuItemChatOpenAI.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemChatOpenAI.Text = "ORM and FEKL Expert"
        '
        'ToolStripMenuItemDocumentSearch
        '
        Me.ToolStripMenuItemDocumentSearch.Name = "ToolStripMenuItemDocumentSearch"
        Me.ToolStripMenuItemDocumentSearch.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemDocumentSearch.Text = "Document Search"
        '
        'DocumentDatabaseToolStripMenuItem
        '
        Me.DocumentDatabaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ManagerToolStripMenuItem})
        Me.DocumentDatabaseToolStripMenuItem.Name = "DocumentDatabaseToolStripMenuItem"
        Me.DocumentDatabaseToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.DocumentDatabaseToolStripMenuItem.Text = "Document Database"
        '
        'ManagerToolStripMenuItem
        '
        Me.ManagerToolStripMenuItem.Name = "ManagerToolStripMenuItem"
        Me.ManagerToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.ManagerToolStripMenuItem.Text = "Manager"
        '
        'MyTasksToolStripMenuItem
        '
        Me.MyTasksToolStripMenuItem.Name = "MyTasksToolStripMenuItem"
        Me.MyTasksToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.MyTasksToolStripMenuItem.Text = "My &Tasks"
        '
        'ToolStripMenuItemBoston
        '
        Me.ToolStripMenuItemBoston.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigurationToolStripMenuItem, Me.ToolStripSeparator14, Me.PluginViewerToolStripMenuItem, Me.ToolStripSeparator2, Me.DatabaseToolStripMenuItem, Me.LogFileToolStripMenuItem, Me.RegistrationToolStripMenuItem, Me.ToolStripMenuItemEditConfigurationData})
        Me.ToolStripMenuItemBoston.Name = "ToolStripMenuItemBoston"
        Me.ToolStripMenuItemBoston.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripMenuItemBoston.Text = "&Boston"
        '
        'ConfigurationToolStripMenuItem
        '
        Me.ConfigurationToolStripMenuItem.Image = CType(resources.GetObject("ConfigurationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ConfigurationToolStripMenuItem.Name = "ConfigurationToolStripMenuItem"
        Me.ConfigurationToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ConfigurationToolStripMenuItem.Text = "&Configuration"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(195, 6)
        '
        'PluginViewerToolStripMenuItem
        '
        Me.PluginViewerToolStripMenuItem.Name = "PluginViewerToolStripMenuItem"
        Me.PluginViewerToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.PluginViewerToolStripMenuItem.Text = "&Plugin Viewer"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(195, 6)
        '
        'DatabaseToolStripMenuItem
        '
        Me.DatabaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackupDatabaseToolStripMenuItem, Me.CompactAndRepairToolStripMenuItem, Me.RemoveUnneededConceptsToolStripMenuItem})
        Me.DatabaseToolStripMenuItem.Image = CType(resources.GetObject("DatabaseToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DatabaseToolStripMenuItem.Name = "DatabaseToolStripMenuItem"
        Me.DatabaseToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.DatabaseToolStripMenuItem.Text = "&Database"
        '
        'BackupDatabaseToolStripMenuItem
        '
        Me.BackupDatabaseToolStripMenuItem.Image = CType(resources.GetObject("BackupDatabaseToolStripMenuItem.Image"), System.Drawing.Image)
        Me.BackupDatabaseToolStripMenuItem.Name = "BackupDatabaseToolStripMenuItem"
        Me.BackupDatabaseToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.BackupDatabaseToolStripMenuItem.Text = "&Backup Database"
        '
        'CompactAndRepairToolStripMenuItem
        '
        Me.CompactAndRepairToolStripMenuItem.Name = "CompactAndRepairToolStripMenuItem"
        Me.CompactAndRepairToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.CompactAndRepairToolStripMenuItem.Text = "&Compact and repair"
        '
        'RemoveUnneededConceptsToolStripMenuItem
        '
        Me.RemoveUnneededConceptsToolStripMenuItem.Name = "RemoveUnneededConceptsToolStripMenuItem"
        Me.RemoveUnneededConceptsToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.RemoveUnneededConceptsToolStripMenuItem.Text = "&Remove unneeded Concepts"
        '
        'LogFileToolStripMenuItem
        '
        Me.LogFileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteLogFileToolStripMenuItem})
        Me.LogFileToolStripMenuItem.Name = "LogFileToolStripMenuItem"
        Me.LogFileToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.LogFileToolStripMenuItem.Text = "&Log File"
        '
        'DeleteLogFileToolStripMenuItem
        '
        Me.DeleteLogFileToolStripMenuItem.Image = CType(resources.GetObject("DeleteLogFileToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteLogFileToolStripMenuItem.Name = "DeleteLogFileToolStripMenuItem"
        Me.DeleteLogFileToolStripMenuItem.Size = New System.Drawing.Size(151, 22)
        Me.DeleteLogFileToolStripMenuItem.Text = "&Delete Log File"
        '
        'RegistrationToolStripMenuItem
        '
        Me.RegistrationToolStripMenuItem.Name = "RegistrationToolStripMenuItem"
        Me.RegistrationToolStripMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.RegistrationToolStripMenuItem.Text = "&Registration"
        '
        'ToolStripMenuItemEditConfigurationData
        '
        Me.ToolStripMenuItemEditConfigurationData.Name = "ToolStripMenuItemEditConfigurationData"
        Me.ToolStripMenuItemEditConfigurationData.Size = New System.Drawing.Size(198, 22)
        Me.ToolStripMenuItemEditConfigurationData.Text = "Edit Configuration Data"
        Me.ToolStripMenuItemEditConfigurationData.Visible = False
        '
        'ToolStripMenuItemUser
        '
        Me.ToolStripMenuItemUser.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddUser, Me.EditUserToolStripMenuItem, Me.ToolStripSeparator7, Me.GroupToolStripMenuItem, Me.ToolStripSeparator9, Me.ToolStripMenuItemRole, Me.ToolStripSeparator15, Me.LogOutToolStripMenuItem, Me.ToolStripMenuItem1})
        Me.ToolStripMenuItemUser.Name = "ToolStripMenuItemUser"
        Me.ToolStripMenuItemUser.Size = New System.Drawing.Size(42, 22)
        Me.ToolStripMenuItemUser.Text = "&User"
        '
        'ToolStripMenuItemAddUser
        '
        Me.ToolStripMenuItemAddUser.Name = "ToolStripMenuItemAddUser"
        Me.ToolStripMenuItemAddUser.Size = New System.Drawing.Size(122, 22)
        Me.ToolStripMenuItemAddUser.Text = "&Add User"
        '
        'EditUserToolStripMenuItem
        '
        Me.EditUserToolStripMenuItem.Name = "EditUserToolStripMenuItem"
        Me.EditUserToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.EditUserToolStripMenuItem.Text = "&Edit User"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(119, 6)
        '
        'GroupToolStripMenuItem
        '
        Me.GroupToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddGroup, Me.ToolStripMenuItemEditGroup})
        Me.GroupToolStripMenuItem.Name = "GroupToolStripMenuItem"
        Me.GroupToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.GroupToolStripMenuItem.Text = "&Group"
        '
        'ToolStripMenuItemAddGroup
        '
        Me.ToolStripMenuItemAddGroup.Name = "ToolStripMenuItemAddGroup"
        Me.ToolStripMenuItemAddGroup.Size = New System.Drawing.Size(132, 22)
        Me.ToolStripMenuItemAddGroup.Text = "&Add Group"
        '
        'ToolStripMenuItemEditGroup
        '
        Me.ToolStripMenuItemEditGroup.Name = "ToolStripMenuItemEditGroup"
        Me.ToolStripMenuItemEditGroup.Size = New System.Drawing.Size(132, 22)
        Me.ToolStripMenuItemEditGroup.Text = "&Edit Group"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(119, 6)
        '
        'ToolStripMenuItemRole
        '
        Me.ToolStripMenuItemRole.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddRoleToolStripMenuItem, Me.EditRoleToolStripMenuItem})
        Me.ToolStripMenuItemRole.Name = "ToolStripMenuItemRole"
        Me.ToolStripMenuItemRole.Size = New System.Drawing.Size(122, 22)
        Me.ToolStripMenuItemRole.Text = "&Role"
        '
        'AddRoleToolStripMenuItem
        '
        Me.AddRoleToolStripMenuItem.Name = "AddRoleToolStripMenuItem"
        Me.AddRoleToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.AddRoleToolStripMenuItem.Text = "&Add Role"
        '
        'EditRoleToolStripMenuItem
        '
        Me.EditRoleToolStripMenuItem.Name = "EditRoleToolStripMenuItem"
        Me.EditRoleToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.EditRoleToolStripMenuItem.Text = "&Edit Role"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(119, 6)
        '
        'LogOutToolStripMenuItem
        '
        Me.LogOutToolStripMenuItem.Name = "LogOutToolStripMenuItem"
        Me.LogOutToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.LogOutToolStripMenuItem.Text = "&Log Out"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(122, 22)
        '
        'ToolStripMenuItemProject
        '
        Me.ToolStripMenuItemProject.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddProjectToolStripMenuItem, Me.EditProjectToolStripMenuItem, Me.ToolStripSeparator16, Me.NamespaceToolStripMenuItem})
        Me.ToolStripMenuItemProject.Name = "ToolStripMenuItemProject"
        Me.ToolStripMenuItemProject.Size = New System.Drawing.Size(56, 22)
        Me.ToolStripMenuItemProject.Text = "P&roject"
        '
        'AddProjectToolStripMenuItem
        '
        Me.AddProjectToolStripMenuItem.Name = "AddProjectToolStripMenuItem"
        Me.AddProjectToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.AddProjectToolStripMenuItem.Text = "&Add Project"
        '
        'EditProjectToolStripMenuItem
        '
        Me.EditProjectToolStripMenuItem.Name = "EditProjectToolStripMenuItem"
        Me.EditProjectToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.EditProjectToolStripMenuItem.Text = "&Edit Project"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(133, 6)
        '
        'NamespaceToolStripMenuItem
        '
        Me.NamespaceToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNamespaceToolStripMenuItem, Me.EditNamespaceToolStripMenuItem})
        Me.NamespaceToolStripMenuItem.Name = "NamespaceToolStripMenuItem"
        Me.NamespaceToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.NamespaceToolStripMenuItem.Text = "&Namespace"
        '
        'AddNamespaceToolStripMenuItem
        '
        Me.AddNamespaceToolStripMenuItem.Name = "AddNamespaceToolStripMenuItem"
        Me.AddNamespaceToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.AddNamespaceToolStripMenuItem.Text = "&Add Namespace"
        '
        'EditNamespaceToolStripMenuItem
        '
        Me.EditNamespaceToolStripMenuItem.Name = "EditNamespaceToolStripMenuItem"
        Me.EditNamespaceToolStripMenuItem.Size = New System.Drawing.Size(161, 22)
        Me.EditNamespaceToolStripMenuItem.Text = "&Edit Namespace"
        '
        'ToolStripMenuItemUnifiedOntology
        '
        Me.ToolStripMenuItemUnifiedOntology.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem3, Me.ToolStripSeparator17, Me.ToolStripMenuItemAddUnifiedOntology, Me.ToolStripMenuItemEditUnifiedOntology})
        Me.ToolStripMenuItemUnifiedOntology.Name = "ToolStripMenuItemUnifiedOntology"
        Me.ToolStripMenuItemUnifiedOntology.Size = New System.Drawing.Size(110, 22)
        Me.ToolStripMenuItemUnifiedOntology.Text = "&Unified Ontology"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Image = CType(resources.GetObject("ToolStripMenuItem3.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItem3.Text = "&Unified Ontology Browser"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItemAddUnifiedOntology
        '
        Me.ToolStripMenuItemAddUnifiedOntology.Image = CType(resources.GetObject("ToolStripMenuItemAddUnifiedOntology.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemAddUnifiedOntology.Name = "ToolStripMenuItemAddUnifiedOntology"
        Me.ToolStripMenuItemAddUnifiedOntology.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemAddUnifiedOntology.Text = "&Add Unified Ontology"
        '
        'ToolStripMenuItemEditUnifiedOntology
        '
        Me.ToolStripMenuItemEditUnifiedOntology.Image = CType(resources.GetObject("ToolStripMenuItemEditUnifiedOntology.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemEditUnifiedOntology.Name = "ToolStripMenuItemEditUnifiedOntology"
        Me.ToolStripMenuItemEditUnifiedOntology.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemEditUnifiedOntology.Text = "&Edit Unified Ontology"
        '
        'ToolStripMenuItemTestClientServer
        '
        Me.ToolStripMenuItemTestClientServer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BroadcastMessageToUsersToolStripMenuItem, Me.ShowBroadcastEventMonitorToolStripMenuItem, Me.ShowClientServerBroadcastTesterToolStripMenuItem})
        Me.ToolStripMenuItemTestClientServer.Name = "ToolStripMenuItemTestClientServer"
        Me.ToolStripMenuItemTestClientServer.Size = New System.Drawing.Size(85, 22)
        Me.ToolStripMenuItemTestClientServer.Text = "Clie&nt Server"
        '
        'BroadcastMessageToUsersToolStripMenuItem
        '
        Me.BroadcastMessageToUsersToolStripMenuItem.Name = "BroadcastMessageToUsersToolStripMenuItem"
        Me.BroadcastMessageToUsersToolStripMenuItem.Size = New System.Drawing.Size(261, 22)
        Me.BroadcastMessageToUsersToolStripMenuItem.Text = "&Broadcast Message to Users"
        '
        'ShowBroadcastEventMonitorToolStripMenuItem
        '
        Me.ShowBroadcastEventMonitorToolStripMenuItem.Name = "ShowBroadcastEventMonitorToolStripMenuItem"
        Me.ShowBroadcastEventMonitorToolStripMenuItem.Size = New System.Drawing.Size(261, 22)
        Me.ShowBroadcastEventMonitorToolStripMenuItem.Text = "Show Broadcast Event Monitor"
        '
        'ShowClientServerBroadcastTesterToolStripMenuItem
        '
        Me.ShowClientServerBroadcastTesterToolStripMenuItem.Name = "ShowClientServerBroadcastTesterToolStripMenuItem"
        Me.ShowClientServerBroadcastTesterToolStripMenuItem.Size = New System.Drawing.Size(261, 22)
        Me.ShowClientServerBroadcastTesterToolStripMenuItem.Text = "Show Client Server Broadcast Tester"
        '
        'ToolStripMenuItemOSM
        '
        Me.ToolStripMenuItemOSM.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TaskToolStripMenuItem, Me.OpenAIFunctionToolStripMenuItem, Me.ViewToolStripMenuItem})
        Me.ToolStripMenuItemOSM.Name = "ToolStripMenuItemOSM"
        Me.ToolStripMenuItemOSM.Size = New System.Drawing.Size(45, 22)
        Me.ToolStripMenuItemOSM.Text = "&OSM"
        '
        'TaskToolStripMenuItem
        '
        Me.TaskToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddTaskToolStripMenuItem, Me.EditTaskToolStripMenuItem, Me.DeleteTaskToolStripMenuItem, Me.ToolStripSeparator18, Me.ImportExportToolStripMenuItem})
        Me.TaskToolStripMenuItem.Name = "TaskToolStripMenuItem"
        Me.TaskToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.TaskToolStripMenuItem.Text = "&Task"
        '
        'AddTaskToolStripMenuItem
        '
        Me.AddTaskToolStripMenuItem.Name = "AddTaskToolStripMenuItem"
        Me.AddTaskToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AddTaskToolStripMenuItem.Text = "&Add Task"
        '
        'EditTaskToolStripMenuItem
        '
        Me.EditTaskToolStripMenuItem.Name = "EditTaskToolStripMenuItem"
        Me.EditTaskToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EditTaskToolStripMenuItem.Text = "&Edit Task"
        '
        'DeleteTaskToolStripMenuItem
        '
        Me.DeleteTaskToolStripMenuItem.Name = "DeleteTaskToolStripMenuItem"
        Me.DeleteTaskToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DeleteTaskToolStripMenuItem.Text = "&Delete Task"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(149, 6)
        '
        'ImportExportToolStripMenuItem
        '
        Me.ImportExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportTaskToolStripMenuItem, Me.ImportTaskToolStripMenuItem})
        Me.ImportExportToolStripMenuItem.Name = "ImportExportToolStripMenuItem"
        Me.ImportExportToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ImportExportToolStripMenuItem.Text = "&Import | Export"
        '
        'ExportTaskToolStripMenuItem
        '
        Me.ExportTaskToolStripMenuItem.Name = "ExportTaskToolStripMenuItem"
        Me.ExportTaskToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.ExportTaskToolStripMenuItem.Text = "&Export Task"
        '
        'ImportTaskToolStripMenuItem
        '
        Me.ImportTaskToolStripMenuItem.Name = "ImportTaskToolStripMenuItem"
        Me.ImportTaskToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.ImportTaskToolStripMenuItem.Text = "&Import Task"
        '
        'OpenAIFunctionToolStripMenuItem
        '
        Me.OpenAIFunctionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddOpenAIFunctionToolStripMenuItem, Me.EditOpenAIFunctionToolStripMenuItem, Me.DeleteOpenAIFunctionToolStripMenuItem})
        Me.OpenAIFunctionToolStripMenuItem.Name = "OpenAIFunctionToolStripMenuItem"
        Me.OpenAIFunctionToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.OpenAIFunctionToolStripMenuItem.Text = "Function"
        '
        'AddOpenAIFunctionToolStripMenuItem
        '
        Me.AddOpenAIFunctionToolStripMenuItem.Name = "AddOpenAIFunctionToolStripMenuItem"
        Me.AddOpenAIFunctionToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.AddOpenAIFunctionToolStripMenuItem.Text = "&Add Function"
        '
        'EditOpenAIFunctionToolStripMenuItem
        '
        Me.EditOpenAIFunctionToolStripMenuItem.Name = "EditOpenAIFunctionToolStripMenuItem"
        Me.EditOpenAIFunctionToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.EditOpenAIFunctionToolStripMenuItem.Text = "&Edit Function"
        '
        'DeleteOpenAIFunctionToolStripMenuItem
        '
        Me.DeleteOpenAIFunctionToolStripMenuItem.Name = "DeleteOpenAIFunctionToolStripMenuItem"
        Me.DeleteOpenAIFunctionToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.DeleteOpenAIFunctionToolStripMenuItem.Text = "&Delete Function"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TheBoxToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(121, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'TheBoxToolStripMenuItem
        '
        Me.TheBoxToolStripMenuItem.Name = "TheBoxToolStripMenuItem"
        Me.TheBoxToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.TheBoxToolStripMenuItem.Text = "The &Box"
        '
        'TestNotificationToolStripMenuItem
        '
        Me.TestNotificationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DoNotificationToolStripMenuItem})
        Me.TestNotificationToolStripMenuItem.Name = "TestNotificationToolStripMenuItem"
        Me.TestNotificationToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.TestNotificationToolStripMenuItem.Text = "Test Notification"
        Me.TestNotificationToolStripMenuItem.Visible = False
        '
        'DoNotificationToolStripMenuItem
        '
        Me.DoNotificationToolStripMenuItem.Name = "DoNotificationToolStripMenuItem"
        Me.DoNotificationToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.DoNotificationToolStripMenuItem.Text = "Do Notification"
        '
        'ToolStripMenuItemSuperuser
        '
        Me.ToolStripMenuItemSuperuser.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ThrowTestErrorMessageToolStripMenuItem, Me.GenericTestFormToolStripMenuItem})
        Me.ToolStripMenuItemSuperuser.Name = "ToolStripMenuItemSuperuser"
        Me.ToolStripMenuItemSuperuser.Size = New System.Drawing.Size(71, 22)
        Me.ToolStripMenuItemSuperuser.Text = "Superuse&r"
        Me.ToolStripMenuItemSuperuser.Visible = False
        '
        'ThrowTestErrorMessageToolStripMenuItem
        '
        Me.ThrowTestErrorMessageToolStripMenuItem.Name = "ThrowTestErrorMessageToolStripMenuItem"
        Me.ThrowTestErrorMessageToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ThrowTestErrorMessageToolStripMenuItem.Text = "Throw test &Error Message"
        '
        'GenericTestFormToolStripMenuItem
        '
        Me.GenericTestFormToolStripMenuItem.Name = "GenericTestFormToolStripMenuItem"
        Me.GenericTestFormToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.GenericTestFormToolStripMenuItem.Text = "Generic Test Form"
        '
        'ToolStripMenuItemEnterprise
        '
        Me.ToolStripMenuItemEnterprise.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnterpriseToolStripMenuItem, Me.ToolStripMenuItem14, Me.ToolStripMenuItemSubjectArea, Me.SolutionToolStripMenuItem})
        Me.ToolStripMenuItemEnterprise.Name = "ToolStripMenuItemEnterprise"
        Me.ToolStripMenuItemEnterprise.Size = New System.Drawing.Size(71, 22)
        Me.ToolStripMenuItemEnterprise.Text = "&Enterprise"
        '
        'EnterpriseToolStripMenuItem
        '
        Me.EnterpriseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_AddEnterprise, Me.EditEnterpriseToolStripMenuItem, Me.DeleteEnterpriseToolStripMenuItem})
        Me.EnterpriseToolStripMenuItem.Name = "EnterpriseToolStripMenuItem"
        Me.EnterpriseToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.EnterpriseToolStripMenuItem.Text = "&Enterprise"
        '
        'mnuOption_AddEnterprise
        '
        Me.mnuOption_AddEnterprise.Image = CType(resources.GetObject("mnuOption_AddEnterprise.Image"), System.Drawing.Image)
        Me.mnuOption_AddEnterprise.Name = "mnuOption_AddEnterprise"
        Me.mnuOption_AddEnterprise.Size = New System.Drawing.Size(162, 22)
        Me.mnuOption_AddEnterprise.Text = "&Add Enterprise"
        '
        'EditEnterpriseToolStripMenuItem
        '
        Me.EditEnterpriseToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.edit16x16
        Me.EditEnterpriseToolStripMenuItem.Name = "EditEnterpriseToolStripMenuItem"
        Me.EditEnterpriseToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.EditEnterpriseToolStripMenuItem.Text = "&Edit Enterprise"
        '
        'DeleteEnterpriseToolStripMenuItem
        '
        Me.DeleteEnterpriseToolStripMenuItem.Image = CType(resources.GetObject("DeleteEnterpriseToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteEnterpriseToolStripMenuItem.Name = "DeleteEnterpriseToolStripMenuItem"
        Me.DeleteEnterpriseToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.DeleteEnterpriseToolStripMenuItem.Text = "&Delete Enterprise"
        '
        'ToolStripMenuItem14
        '
        Me.ToolStripMenuItem14.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem15, Me.ToolStripMenuItem16, Me.ToolStripMenuItem17})
        Me.ToolStripMenuItem14.Name = "ToolStripMenuItem14"
        Me.ToolStripMenuItem14.Size = New System.Drawing.Size(142, 22)
        Me.ToolStripMenuItem14.Text = "&Organisation"
        '
        'ToolStripMenuItem15
        '
        Me.ToolStripMenuItem15.Image = Global.Boston.My.Resources.Resources.Add16x16
        Me.ToolStripMenuItem15.Name = "ToolStripMenuItem15"
        Me.ToolStripMenuItem15.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem15.Text = "&Add Organisation"
        '
        'ToolStripMenuItem16
        '
        Me.ToolStripMenuItem16.Image = Global.Boston.My.Resources.Resources.edit16x16
        Me.ToolStripMenuItem16.Name = "ToolStripMenuItem16"
        Me.ToolStripMenuItem16.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem16.Text = "&Edit Organisation"
        '
        'ToolStripMenuItem17
        '
        Me.ToolStripMenuItem17.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.ToolStripMenuItem17.Name = "ToolStripMenuItem17"
        Me.ToolStripMenuItem17.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem17.Text = "&Delete Organisation"
        '
        'ToolStripMenuItemSubjectArea
        '
        Me.ToolStripMenuItemSubjectArea.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem18, Me.ToolStripMenuItem19, Me.ToolStripMenuItem20})
        Me.ToolStripMenuItemSubjectArea.Name = "ToolStripMenuItemSubjectArea"
        Me.ToolStripMenuItemSubjectArea.Size = New System.Drawing.Size(142, 22)
        Me.ToolStripMenuItemSubjectArea.Text = "Subject &Area"
        '
        'ToolStripMenuItem18
        '
        Me.ToolStripMenuItem18.Image = CType(resources.GetObject("ToolStripMenuItem18.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem18.Name = "ToolStripMenuItem18"
        Me.ToolStripMenuItem18.Size = New System.Drawing.Size(176, 22)
        Me.ToolStripMenuItem18.Text = "&Add Subject Area"
        '
        'ToolStripMenuItem19
        '
        Me.ToolStripMenuItem19.Image = Global.Boston.My.Resources.Resources.edit16x16
        Me.ToolStripMenuItem19.Name = "ToolStripMenuItem19"
        Me.ToolStripMenuItem19.Size = New System.Drawing.Size(176, 22)
        Me.ToolStripMenuItem19.Text = "&Edit Subject Area"
        '
        'ToolStripMenuItem20
        '
        Me.ToolStripMenuItem20.Image = CType(resources.GetObject("ToolStripMenuItem20.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem20.Name = "ToolStripMenuItem20"
        Me.ToolStripMenuItem20.Size = New System.Drawing.Size(176, 22)
        Me.ToolStripMenuItem20.Text = "&Delete Subject Area"
        '
        'SolutionToolStripMenuItem
        '
        Me.SolutionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddSolutionToolStripMenuItem, Me.EditSolutionToolStripMenuItem, Me.DeleteSolutionToolStripMenuItem})
        Me.SolutionToolStripMenuItem.Name = "SolutionToolStripMenuItem"
        Me.SolutionToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.SolutionToolStripMenuItem.Text = "&Solution"
        '
        'AddSolutionToolStripMenuItem
        '
        Me.AddSolutionToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Add16x16
        Me.AddSolutionToolStripMenuItem.Name = "AddSolutionToolStripMenuItem"
        Me.AddSolutionToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.AddSolutionToolStripMenuItem.Text = "&Add Solution"
        '
        'EditSolutionToolStripMenuItem
        '
        Me.EditSolutionToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.edit16x16
        Me.EditSolutionToolStripMenuItem.Name = "EditSolutionToolStripMenuItem"
        Me.EditSolutionToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.EditSolutionToolStripMenuItem.Text = "&Edit Solution"
        '
        'DeleteSolutionToolStripMenuItem
        '
        Me.DeleteSolutionToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.DeleteSolutionToolStripMenuItem.Name = "DeleteSolutionToolStripMenuItem"
        Me.DeleteSolutionToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DeleteSolutionToolStripMenuItem.Text = "&Delete Solution"
        '
        'ToolStripMenuItemApplication
        '
        Me.ToolStripMenuItemApplication.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddApplicationToolStripMenuItem, Me.EditApplicationToolStripMenuItem, Me.DeleteApplicationToolStripMenuItem, Me.ToolStripSeparator19, Me.ApplicationMenuEditorToolStripMenuItem, Me.ApplicationRunnerToolStripMenuItem})
        Me.ToolStripMenuItemApplication.Name = "ToolStripMenuItemApplication"
        Me.ToolStripMenuItemApplication.Size = New System.Drawing.Size(80, 22)
        Me.ToolStripMenuItemApplication.Text = "&Application"
        '
        'AddApplicationToolStripMenuItem
        '
        Me.AddApplicationToolStripMenuItem.Name = "AddApplicationToolStripMenuItem"
        Me.AddApplicationToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.AddApplicationToolStripMenuItem.Text = "&Add Application"
        '
        'EditApplicationToolStripMenuItem
        '
        Me.EditApplicationToolStripMenuItem.Name = "EditApplicationToolStripMenuItem"
        Me.EditApplicationToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.EditApplicationToolStripMenuItem.Text = "&Edit Application"
        '
        'DeleteApplicationToolStripMenuItem
        '
        Me.DeleteApplicationToolStripMenuItem.Name = "DeleteApplicationToolStripMenuItem"
        Me.DeleteApplicationToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.DeleteApplicationToolStripMenuItem.Text = "&Delete Application"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(200, 6)
        '
        'ApplicationMenuEditorToolStripMenuItem
        '
        Me.ApplicationMenuEditorToolStripMenuItem.Name = "ApplicationMenuEditorToolStripMenuItem"
        Me.ApplicationMenuEditorToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ApplicationMenuEditorToolStripMenuItem.Text = "Application &Menu Editor"
        '
        'ApplicationRunnerToolStripMenuItem
        '
        Me.ApplicationRunnerToolStripMenuItem.Name = "ApplicationRunnerToolStripMenuItem"
        Me.ApplicationRunnerToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ApplicationRunnerToolStripMenuItem.Text = "Application Runner"
        '
        'ToolStripMenuItemTesting
        '
        Me.ToolStripMenuItemTesting.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestSetManagerToolStripMenuItem})
        Me.ToolStripMenuItemTesting.Name = "ToolStripMenuItemTesting"
        Me.ToolStripMenuItemTesting.Size = New System.Drawing.Size(114, 22)
        Me.ToolStripMenuItemTesting.Text = "&Test Management"
        '
        'TestSetManagerToolStripMenuItem
        '
        Me.TestSetManagerToolStripMenuItem.Image = CType(resources.GetObject("TestSetManagerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.TestSetManagerToolStripMenuItem.Name = "TestSetManagerToolStripMenuItem"
        Me.TestSetManagerToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.TestSetManagerToolStripMenuItem.Text = "&Test Manager"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemHelp, Me.AboutRichmondToolStripMenuItem, Me.ToolStripSeparator13, Me.ToolStripMenuItemOpenLogFile, Me.EmailSupportvievcomToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'ToolStripMenuItemHelp
        '
        Me.ToolStripMenuItemHelp.Image = CType(resources.GetObject("ToolStripMenuItemHelp.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp"
        Me.ToolStripMenuItemHelp.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.ToolStripMenuItemHelp.Size = New System.Drawing.Size(226, 22)
        Me.ToolStripMenuItemHelp.Text = "&Help"
        '
        'AboutRichmondToolStripMenuItem
        '
        Me.AboutRichmondToolStripMenuItem.Image = CType(resources.GetObject("AboutRichmondToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AboutRichmondToolStripMenuItem.Name = "AboutRichmondToolStripMenuItem"
        Me.AboutRichmondToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.AboutRichmondToolStripMenuItem.Text = "&About Boston"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(223, 6)
        '
        'ToolStripMenuItemOpenLogFile
        '
        Me.ToolStripMenuItemOpenLogFile.Image = CType(resources.GetObject("ToolStripMenuItemOpenLogFile.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemOpenLogFile.Name = "ToolStripMenuItemOpenLogFile"
        Me.ToolStripMenuItemOpenLogFile.Size = New System.Drawing.Size(226, 22)
        Me.ToolStripMenuItemOpenLogFile.Text = "&Open Log File"
        '
        'EmailSupportvievcomToolStripMenuItem
        '
        Me.EmailSupportvievcomToolStripMenuItem.Image = CType(resources.GetObject("EmailSupportvievcomToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EmailSupportvievcomToolStripMenuItem.Name = "EmailSupportvievcomToolStripMenuItem"
        Me.EmailSupportvievcomToolStripMenuItem.Size = New System.Drawing.Size(226, 22)
        Me.EmailSupportvievcomToolStripMenuItem.Text = "&Email support@factengine.ai"
        '
        'ToolStrip_main
        '
        Me.ToolStrip_main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton_Save, Me.ToolStripSeparator4, Me.ToolStripButtonNewModel, Me.ToolStripButtonNew, Me.ToolStripButtonPrint, Me.ToolStripButtonCopy, Me.ToolStripButtonPaste, Me.toolStripSeparator12, Me.ToolStripLabelPrompt_zoom, Me.ToolStripComboBox_zoom, Me.toolStripSeparator, Me.ToolStripButtonHelp, Me.ToolStripSeparator3, Me.ToolStripButtonProfile, Me.ToolStripButtonNotifications})
        Me.ToolStrip_main.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip_main.Name = "ToolStrip_main"
        Me.ToolStrip_main.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStrip_main.Size = New System.Drawing.Size(1066, 32)
        Me.ToolStrip_main.TabIndex = 9
        Me.ToolStrip_main.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.AutoSize = False
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(28, 29)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripButton_Save
        '
        Me.ToolStripButton_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton_Save.Enabled = False
        Me.ToolStripButton_Save.Image = CType(resources.GetObject("ToolStripButton_Save.Image"), System.Drawing.Image)
        Me.ToolStripButton_Save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton_Save.Name = "ToolStripButton_Save"
        Me.ToolStripButton_Save.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButton_Save.Text = "Save"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 32)
        '
        'ToolStripButtonNewModel
        '
        Me.ToolStripButtonNewModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonNewModel.Image = CType(resources.GetObject("ToolStripButtonNewModel.Image"), System.Drawing.Image)
        Me.ToolStripButtonNewModel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonNewModel.Name = "ToolStripButtonNewModel"
        Me.ToolStripButtonNewModel.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonNewModel.Text = "New &Model"
        '
        'ToolStripButtonNew
        '
        Me.ToolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonNew.Enabled = False
        Me.ToolStripButtonNew.Image = CType(resources.GetObject("ToolStripButtonNew.Image"), System.Drawing.Image)
        Me.ToolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonNew.Name = "ToolStripButtonNew"
        Me.ToolStripButtonNew.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonNew.Text = "&New Page for Model"
        '
        'ToolStripButtonPrint
        '
        Me.ToolStripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonPrint.Enabled = False
        Me.ToolStripButtonPrint.Image = CType(resources.GetObject("ToolStripButtonPrint.Image"), System.Drawing.Image)
        Me.ToolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonPrint.Name = "ToolStripButtonPrint"
        Me.ToolStripButtonPrint.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonPrint.Text = "&Print"
        '
        'ToolStripButtonCopy
        '
        Me.ToolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonCopy.Enabled = False
        Me.ToolStripButtonCopy.Image = CType(resources.GetObject("ToolStripButtonCopy.Image"), System.Drawing.Image)
        Me.ToolStripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonCopy.Name = "ToolStripButtonCopy"
        Me.ToolStripButtonCopy.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonCopy.Text = "&Copy"
        '
        'ToolStripButtonPaste
        '
        Me.ToolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonPaste.Enabled = False
        Me.ToolStripButtonPaste.Image = CType(resources.GetObject("ToolStripButtonPaste.Image"), System.Drawing.Image)
        Me.ToolStripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonPaste.Name = "ToolStripButtonPaste"
        Me.ToolStripButtonPaste.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonPaste.Text = "&Paste"
        '
        'toolStripSeparator12
        '
        Me.toolStripSeparator12.Name = "toolStripSeparator12"
        Me.toolStripSeparator12.Size = New System.Drawing.Size(6, 32)
        '
        'ToolStripLabelPrompt_zoom
        '
        Me.ToolStripLabelPrompt_zoom.Name = "ToolStripLabelPrompt_zoom"
        Me.ToolStripLabelPrompt_zoom.Size = New System.Drawing.Size(45, 29)
        Me.ToolStripLabelPrompt_zoom.Text = "Zoom :"
        '
        'ToolStripComboBox_zoom
        '
        Me.ToolStripComboBox_zoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox_zoom.DropDownWidth = 75
        Me.ToolStripComboBox_zoom.Name = "ToolStripComboBox_zoom"
        Me.ToolStripComboBox_zoom.Size = New System.Drawing.Size(75, 32)
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(6, 32)
        '
        'ToolStripButtonHelp
        '
        Me.ToolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonHelp.Image = CType(resources.GetObject("ToolStripButtonHelp.Image"), System.Drawing.Image)
        Me.ToolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonHelp.Name = "ToolStripButtonHelp"
        Me.ToolStripButtonHelp.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonHelp.Text = "He&lp"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 32)
        '
        'ToolStripButtonProfile
        '
        Me.ToolStripButtonProfile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonProfile.Image = CType(resources.GetObject("ToolStripButtonProfile.Image"), System.Drawing.Image)
        Me.ToolStripButtonProfile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolStripButtonProfile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonProfile.Name = "ToolStripButtonProfile"
        Me.ToolStripButtonProfile.Size = New System.Drawing.Size(23, 29)
        Me.ToolStripButtonProfile.Text = "ToolStripButton1"
        Me.ToolStripButtonProfile.Visible = False
        '
        'ToolStripButtonNotifications
        '
        Me.ToolStripButtonNotifications.Image = CType(resources.GetObject("ToolStripButtonNotifications.Image"), System.Drawing.Image)
        Me.ToolStripButtonNotifications.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonNotifications.Name = "ToolStripButtonNotifications"
        Me.ToolStripButtonNotifications.Size = New System.Drawing.Size(144, 29)
        Me.ToolStripButtonNotifications.Text = "You have notifications"
        Me.ToolStripButtonNotifications.Visible = False
        '
        'StatusBar_main
        '
        Me.StatusBar_main.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusBar_main.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabelGeneralStatus, Me.ToolStripStatusLabelUsername, Me.ToolStripProgressBar, Me.ToolStripStatusLabelClientServer, Me.ToolStripStatusLabelPromptEvaluationTime, Me.ToolStripStatusLabelPromptEvaluationTimeSeconds})
        Me.StatusBar_main.Location = New System.Drawing.Point(0, 543)
        Me.StatusBar_main.Name = "StatusBar_main"
        Me.StatusBar_main.Size = New System.Drawing.Size(1066, 24)
        Me.StatusBar_main.TabIndex = 13
        Me.StatusBar_main.Text = "StatusStrip1"
        '
        'StatusLabelGeneralStatus
        '
        Me.StatusLabelGeneralStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left
        Me.StatusLabelGeneralStatus.Name = "StatusLabelGeneralStatus"
        Me.StatusLabelGeneralStatus.Size = New System.Drawing.Size(1051, 19)
        Me.StatusLabelGeneralStatus.Spring = True
        Me.StatusLabelGeneralStatus.Text = "    "
        Me.StatusLabelGeneralStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabelUsername
        '
        Me.ToolStripStatusLabelUsername.Name = "ToolStripStatusLabelUsername"
        Me.ToolStripStatusLabelUsername.Size = New System.Drawing.Size(0, 19)
        '
        'ToolStripProgressBar
        '
        Me.ToolStripProgressBar.Name = "ToolStripProgressBar"
        Me.ToolStripProgressBar.Size = New System.Drawing.Size(100, 19)
        Me.ToolStripProgressBar.Visible = False
        '
        'ToolStripStatusLabelClientServer
        '
        Me.ToolStripStatusLabelClientServer.Name = "ToolStripStatusLabelClientServer"
        Me.ToolStripStatusLabelClientServer.Size = New System.Drawing.Size(0, 19)
        '
        'ToolStripStatusLabelPromptEvaluationTime
        '
        Me.ToolStripStatusLabelPromptEvaluationTime.Name = "ToolStripStatusLabelPromptEvaluationTime"
        Me.ToolStripStatusLabelPromptEvaluationTime.Size = New System.Drawing.Size(95, 19)
        Me.ToolStripStatusLabelPromptEvaluationTime.Text = "Evaluation Time:"
        Me.ToolStripStatusLabelPromptEvaluationTime.Visible = False
        '
        'ToolStripStatusLabelPromptEvaluationTimeSeconds
        '
        Me.ToolStripStatusLabelPromptEvaluationTimeSeconds.Name = "ToolStripStatusLabelPromptEvaluationTimeSeconds"
        Me.ToolStripStatusLabelPromptEvaluationTimeSeconds.Size = New System.Drawing.Size(18, 19)
        Me.ToolStripStatusLabelPromptEvaluationTimeSeconds.Text = "0s"
        Me.ToolStripStatusLabelPromptEvaluationTimeSeconds.Visible = False
        '
        'ContextMenuStrip_Project
        '
        Me.ContextMenuStrip_Project.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_Project.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetProjectToolStripMenuItem})
        Me.ContextMenuStrip_Project.Name = "ContextMenuStrip_Project"
        Me.ContextMenuStrip_Project.Size = New System.Drawing.Size(131, 26)
        '
        'SetProjectToolStripMenuItem
        '
        Me.SetProjectToolStripMenuItem.Name = "SetProjectToolStripMenuItem"
        Me.SetProjectToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SetProjectToolStripMenuItem.Text = "&Set Project"
        '
        'DockPanel
        '
        Me.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DockPanel.Location = New System.Drawing.Point(0, 56)
        Me.DockPanel.Name = "DockPanel"
        Me.DockPanel.Size = New System.Drawing.Size(1066, 487)
        Me.DockPanel.TabIndex = 24
        '
        'HelpProvider
        '
        Me.HelpProvider.HelpNamespace = ".\richmondhelp\Boston.chm"
        '
        'NotifyIcon
        '
        Me.NotifyIcon.Text = "NotifyIcon1"
        Me.NotifyIcon.Visible = True
        '
        'TimerNotifications
        '
        Me.TimerNotifications.Interval = 5000
        '
        'BackgroundWorkerStatusBar
        '
        Me.BackgroundWorkerStatusBar.WorkerReportsProgress = True
        '
        'TimerGFSBackup
        '
        Me.TimerGFSBackup.Interval = 20000
        '
        'TimerDemo
        '
        Me.TimerDemo.Interval = 10000
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1066, 567)
        Me.Controls.Add(Me.DockPanel)
        Me.Controls.Add(Me.StatusBar_main)
        Me.Controls.Add(Me.ToolStrip_main)
        Me.Controls.Add(Me.MenuStrip_main)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "frmMain"
        Me.HelpProvider.SetShowHelp(Me, True)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Boston"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip_main.ResumeLayout(False)
        Me.MenuStrip_main.PerformLayout()
        Me.ToolStrip_main.ResumeLayout(False)
        Me.ToolStrip_main.PerformLayout()
        Me.StatusBar_main.ResumeLayout(False)
        Me.StatusBar_main.PerformLayout()
        Me.ContextMenuStrip_Project.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip_main As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStrip_main As System.Windows.Forms.ToolStrip
    Friend WithEvents mnu_Session As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_EndSession As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemBoston As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBar_main As System.Windows.Forms.StatusStrip
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem11 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutRichmondToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Project As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SetProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripButton_Save As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuItem_ShowEnterpriseTreeView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StandardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripLabelPrompt_zoom As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripComboBox_zoom As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripMenuItemUndo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigurationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DockPanel As WeifenLuo.WinFormsUI.Docking.DockPanel
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyAsImageToClipboardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemPage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRecentNodes As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider As System.Windows.Forms.HelpProvider
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonNew As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonCopy As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButtonPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonHelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripMenuItemHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusLabelGeneralStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemOpenLogFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackupDatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LogFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteLogFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents ToolboxesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemToolbox As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDiagramOverview As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemKLTheoremWriter As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRedo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents EmailSupportvievcomToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveUnneededConceptsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemDiagramSpy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PluginViewerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemTestClientServer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelClientServer As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButtonProfile As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripMenuItemLogIn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddUser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditUserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemProject As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelUsername As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemRole As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddRoleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditRoleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents LogOutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents NamespaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddNamespaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditNamespaceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemLogOut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestNotificationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DoNotificationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripButtonNotifications As System.Windows.Forms.ToolStripButton
    Friend WithEvents TimerNotifications As System.Windows.Forms.Timer
    Friend WithEvents ShowBroadcastEventMonitorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemNewModel As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripButtonNewModel As ToolStripButton
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripMenuItemCodeGenerator As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFactEngine As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents CompactAndRepairToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemUnifiedOntologyBrowser As ToolStripMenuItem
    Friend WithEvents RegistrationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowClientServerBroadcastTesterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSuperuser As ToolStripMenuItem
    Friend WithEvents ThrowTestErrorMessageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemUnifiedOntology As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddUnifiedOntology As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditUnifiedOntology As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditConfigurationData As ToolStripMenuItem
    Friend WithEvents BroadcastMessageToUsersToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackgroundWorkerStatusBar As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolStripMenuItemLogInAs As ToolStripMenuItem
    Friend WithEvents JupyterNotebookToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenericTestFormToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConceptClassificationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents VirtualAnalystToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DescriptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChatOpenAI As ToolStripMenuItem
    Friend WithEvents TimerGFSBackup As Timer
    Friend WithEvents ToolStripMenuItemOSM As ToolStripMenuItem
    Friend WithEvents TaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddTaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditTaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteTaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenAIFunctionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddOpenAIFunctionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditOpenAIFunctionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteOpenAIFunctionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TheBoxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents ImportExportToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExportTaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ImportTaskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDocumentSearch As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEnterprise As ToolStripMenuItem
    Friend WithEvents EnterpriseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuOption_AddEnterprise As ToolStripMenuItem
    Friend WithEvents EditEnterpriseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteEnterpriseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem14 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem15 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem16 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem17 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSubjectArea As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem18 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem19 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem20 As ToolStripMenuItem
    Friend WithEvents DocumentDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ManagerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TimerDemo As Timer
    Friend WithEvents QueryEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelPromptEvaluationTime As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelPromptEvaluationTimeSeconds As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItemApplication As ToolStripMenuItem
    Friend WithEvents AddApplicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditApplicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
    Friend WithEvents ApplicationMenuEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteApplicationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ApplicationRunnerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemTesting As ToolStripMenuItem
    Friend WithEvents TestSetManagerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MyTasksToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SolutionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddSolutionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditSolutionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteSolutionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ModelViewsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RelationalManagerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GraphSchemaManagerToolStripMenuItem1 As ToolStripMenuItem
End Class
