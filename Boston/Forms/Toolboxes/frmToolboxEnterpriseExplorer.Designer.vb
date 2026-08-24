Imports Boston

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmToolboxEnterpriseExplorer
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            Me.TreeView.Nodes.Clear()
            Me.TreeView.Dispose()
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolboxEnterpriseExplorer))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelProjectControls = New System.Windows.Forms.Panel()
        Me.ComboBoxProject = New System.Windows.Forms.ComboBox()
        Me.LabelPromptProject = New System.Windows.Forms.Label()
        Me.LabelPromptNamespace = New System.Windows.Forms.Label()
        Me.ComboBoxNamespace = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LabelHelpTips = New System.Windows.Forms.Label()
        Me.SearchTextbox = New CustomSearchTextbox()
        Me.ButtonNewModel = New System.Windows.Forms.Button()
        Me.TreeView = New Boston.BostonTreeView()
        Me.ContextMenuStrip_ORMModels = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemAddModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportormNORMAFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromRDFOWLTurtlettlFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromfbmStandardFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromXMLDocumentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnhideHiddenModelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnhideASelectedModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.CircularProgressBar = New CircularProgressBar.CircularProgressBar()
        Me.Timer_FormSetup = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Page = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeletePageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEditPageAsORMDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_ORMModel = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewGlossaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemViewDatabaseSchema = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenerateDocumentationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemCodeGenerator = New System.Windows.Forms.ToolStripMenuItem()
        Me.FactEngineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemTaxonomyTree = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemKeywordExtractionTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.FEKLUploaderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLIFGeneratorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RDFGeneratorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UMSGeneratorProcessorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OssieGeneratorProcessorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditAITrainingDataEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VirtualBusinessAnalystToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemLanguage = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddORMPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertyGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddPGSPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EntityRelationshipDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddERDPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StateTransitionDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddSTDPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BusinessProcessModellingNotationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddBPMNPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddBPMNChoreographyDiagramPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddBPMNCollaborationDiagramPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddProcessDiagramPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddUseCaseDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StructureChartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FlowchartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemPastePage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddXSDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemEmptyModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideAllotherModelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem12 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromORMCMMLFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportTestingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TocqlFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemExportToNORMAormFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToFEKLtxtFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RelationalDataStructureToXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RDFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToRDFTurtlettlFilebetaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToRDFOWLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToRDSTriGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToRDSTriGWithSHACLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TofbmExchangeMetaModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TofigFactInterchangeGrammarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemMoveModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemShareModelWithProject = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemModelConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemFixModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.HelpProvider = New System.Windows.Forms.HelpProvider()
        Me.BackgroundWorkerModelLoader = New System.ComponentModel.BackgroundWorker()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStripSolution = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CreateATicketToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripXSD = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.PanelProjectControls.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip_ORMModels.SuspendLayout()
        Me.ContextMenuStrip_Page.SuspendLayout()
        Me.ContextMenuStrip_ORMModel.SuspendLayout()
        Me.ContextMenuStripSolution.SuspendLayout()
        Me.ContextMenuStripXSD.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PanelProjectControls, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(290, 559)
        Me.TableLayoutPanel1.TabIndex = 4
        '
        'PanelProjectControls
        '
        Me.PanelProjectControls.AutoSize = True
        Me.PanelProjectControls.Controls.Add(Me.ComboBoxProject)
        Me.PanelProjectControls.Controls.Add(Me.LabelPromptProject)
        Me.PanelProjectControls.Controls.Add(Me.LabelPromptNamespace)
        Me.PanelProjectControls.Controls.Add(Me.ComboBoxNamespace)
        Me.PanelProjectControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelProjectControls.Location = New System.Drawing.Point(0, 0)
        Me.PanelProjectControls.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelProjectControls.Name = "PanelProjectControls"
        Me.PanelProjectControls.Size = New System.Drawing.Size(290, 31)
        Me.PanelProjectControls.TabIndex = 12
        '
        'ComboBoxProject
        '
        Me.ComboBoxProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxProject.FormattingEnabled = True
        Me.ComboBoxProject.Location = New System.Drawing.Point(47, 7)
        Me.ComboBoxProject.Name = "ComboBoxProject"
        Me.ComboBoxProject.Size = New System.Drawing.Size(146, 21)
        Me.ComboBoxProject.Sorted = True
        Me.ComboBoxProject.TabIndex = 7
        '
        'LabelPromptProject
        '
        Me.LabelPromptProject.AutoSize = True
        Me.LabelPromptProject.Location = New System.Drawing.Point(3, 10)
        Me.LabelPromptProject.Name = "LabelPromptProject"
        Me.LabelPromptProject.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptProject.TabIndex = 6
        Me.LabelPromptProject.Text = "Project:"
        '
        'LabelPromptNamespace
        '
        Me.LabelPromptNamespace.AutoSize = True
        Me.LabelPromptNamespace.Location = New System.Drawing.Point(199, 10)
        Me.LabelPromptNamespace.Name = "LabelPromptNamespace"
        Me.LabelPromptNamespace.Size = New System.Drawing.Size(49, 13)
        Me.LabelPromptNamespace.TabIndex = 8
        Me.LabelPromptNamespace.Text = "NSpace:"
        Me.LabelPromptNamespace.Visible = False
        '
        'ComboBoxNamespace
        '
        Me.ComboBoxNamespace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNamespace.FormattingEnabled = True
        Me.ComboBoxNamespace.Location = New System.Drawing.Point(250, 7)
        Me.ComboBoxNamespace.Name = "ComboBoxNamespace"
        Me.ComboBoxNamespace.Size = New System.Drawing.Size(137, 21)
        Me.ComboBoxNamespace.TabIndex = 9
        Me.ComboBoxNamespace.Visible = False
        '
        'Panel1
        '
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.Controls.Add(Me.LabelHelpTips)
        Me.Panel1.Controls.Add(Me.SearchTextbox)
        Me.Panel1.Controls.Add(Me.ButtonNewModel)
        Me.Panel1.Controls.Add(Me.TreeView)
        Me.Panel1.Controls.Add(Me.CircularProgressBar)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 34)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(284, 522)
        Me.Panel1.TabIndex = 11
        '
        'LabelHelpTips
        '
        Me.LabelHelpTips.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelHelpTips.BackColor = System.Drawing.Color.LightGoldenrodYellow
        Me.LabelHelpTips.Location = New System.Drawing.Point(3, 446)
        Me.LabelHelpTips.Name = "LabelHelpTips"
        Me.LabelHelpTips.Size = New System.Drawing.Size(278, 76)
        Me.LabelHelpTips.TabIndex = 12
        '
        'SearchTextbox
        '
        Me.SearchTextbox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SearchTextbox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SearchTextbox.Location = New System.Drawing.Point(3, 5)
        Me.SearchTextbox.Name = "SearchTextbox"
        Me.SearchTextbox.Size = New System.Drawing.Size(252, 30)
        Me.SearchTextbox.TabIndex = 11
        '
        'ButtonNewModel
        '
        Me.ButtonNewModel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonNewModel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ButtonNewModel.BackgroundImage = Global.Boston.My.Resources.Resources.ModelAdd16x16
        Me.ButtonNewModel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ButtonNewModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNewModel.ForeColor = System.Drawing.Color.White
        Me.ButtonNewModel.Location = New System.Drawing.Point(256, 10)
        Me.ButtonNewModel.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonNewModel.Name = "ButtonNewModel"
        Me.ButtonNewModel.Size = New System.Drawing.Size(18, 18)
        Me.ButtonNewModel.TabIndex = 10
        Me.ButtonNewModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip.SetToolTip(Me.ButtonNewModel, "Add a Model to Boston")
        Me.ButtonNewModel.UseVisualStyleBackColor = True
        '
        'TreeView
        '
        Me.TreeView.AllowDrop = True
        Me.TreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView.BackColor = System.Drawing.SystemColors.Window
        Me.TreeView.ContextMenuStrip = Me.ContextMenuStrip_ORMModels
        Me.TreeView.ForeColor = System.Drawing.Color.Black
        Me.TreeView.HideSelection = False
        Me.TreeView.ImageIndex = 0
        Me.TreeView.ImageList = Me.ImageList
        Me.TreeView.LabelEdit = True
        Me.TreeView.Location = New System.Drawing.Point(3, 35)
        Me.TreeView.MinimumSize = New System.Drawing.Size(190, 4)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.SelectedImageKey = "blank.ico"
        Me.TreeView.SelectedNode = Nothing
        Me.TreeView.Size = New System.Drawing.Size(278, 408)
        Me.TreeView.TabIndex = 0
        '
        'ContextMenuStrip_ORMModels
        '
        Me.ContextMenuStrip_ORMModels.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip_ORMModels.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddModel, Me.ToolStripSeparator2, Me.ToolStripMenuItem3, Me.UnhideHiddenModelsToolStripMenuItem, Me.UnhideASelectedModelToolStripMenuItem})
        Me.ContextMenuStrip_ORMModels.Name = "ContextMenuStrip_ORMModels"
        Me.ContextMenuStrip_ORMModels.Size = New System.Drawing.Size(209, 114)
        '
        'ToolStripMenuItemAddModel
        '
        Me.ToolStripMenuItemAddModel.Image = CType(resources.GetObject("ToolStripMenuItemAddModel.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemAddModel.Name = "ToolStripMenuItemAddModel"
        Me.ToolStripMenuItemAddModel.Size = New System.Drawing.Size(208, 26)
        Me.ToolStripMenuItemAddModel.Text = "&Add Model"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(205, 6)
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4, Me.ImportormNORMAFileToolStripMenuItem, Me.FromRDFOWLTurtlettlFileToolStripMenuItem, Me.FromfbmStandardFileToolStripMenuItem, Me.FromXMLDocumentToolStripMenuItem})
        Me.ToolStripMenuItem3.Image = CType(resources.GetObject("ToolStripMenuItem3.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(208, 26)
        Me.ToolStripMenuItem3.Text = "&Import"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Image = Global.Boston.My.Resources.Resources.XML16x16
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(227, 22)
        Me.ToolStripMenuItem4.Text = "From .fbm file"
        '
        'ImportormNORMAFileToolStripMenuItem
        '
        Me.ImportormNORMAFileToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.NORMA16x16
        Me.ImportormNORMAFileToolStripMenuItem.Name = "ImportormNORMAFileToolStripMenuItem"
        Me.ImportormNORMAFileToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.ImportormNORMAFileToolStripMenuItem.Text = "Import .orm NORMA File"
        '
        'FromRDFOWLTurtlettlFileToolStripMenuItem
        '
        Me.FromRDFOWLTurtlettlFileToolStripMenuItem.Name = "FromRDFOWLTurtlettlFileToolStripMenuItem"
        Me.FromRDFOWLTurtlettlFileToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.FromRDFOWLTurtlettlFileToolStripMenuItem.Text = "From RDF/OWL Turtle .ttl file"
        '
        'FromfbmStandardFileToolStripMenuItem
        '
        Me.FromfbmStandardFileToolStripMenuItem.Name = "FromfbmStandardFileToolStripMenuItem"
        Me.FromfbmStandardFileToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.FromfbmStandardFileToolStripMenuItem.Text = "From .fbm Standard file"
        '
        'FromXMLDocumentToolStripMenuItem
        '
        Me.FromXMLDocumentToolStripMenuItem.Name = "FromXMLDocumentToolStripMenuItem"
        Me.FromXMLDocumentToolStripMenuItem.Size = New System.Drawing.Size(227, 22)
        Me.FromXMLDocumentToolStripMenuItem.Text = "From .XML document"
        '
        'UnhideHiddenModelsToolStripMenuItem
        '
        Me.UnhideHiddenModelsToolStripMenuItem.Name = "UnhideHiddenModelsToolStripMenuItem"
        Me.UnhideHiddenModelsToolStripMenuItem.Size = New System.Drawing.Size(208, 26)
        Me.UnhideHiddenModelsToolStripMenuItem.Text = "&Unhide Hidden Models"
        '
        'UnhideASelectedModelToolStripMenuItem
        '
        Me.UnhideASelectedModelToolStripMenuItem.Name = "UnhideASelectedModelToolStripMenuItem"
        Me.UnhideASelectedModelToolStripMenuItem.Size = New System.Drawing.Size(208, 26)
        Me.UnhideASelectedModelToolStripMenuItem.Text = "&Unhide a selected Model"
        '
        'ImageList
        '
        Me.ImageList.ImageStream = CType(resources.GetObject("ImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList.Images.SetKeyName(0, "orm_icon.ico")
        Me.ImageList.Images.SetKeyName(1, "database-16x16.png")
        Me.ImageList.Images.SetKeyName(2, "Page16x16.png")
        Me.ImageList.Images.SetKeyName(3, "PagePGS16x16.png")
        Me.ImageList.Images.SetKeyName(4, "ERD-16-16.png")
        Me.ImageList.Images.SetKeyName(5, "StateTransitionDiagram-16x16.png")
        Me.ImageList.Images.SetKeyName(6, "MongoDB.png")
        Me.ImageList.Images.SetKeyName(7, "MSAccessLogo.png")
        Me.ImageList.Images.SetKeyName(8, "SQLiteLogo.png")
        Me.ImageList.Images.SetKeyName(9, "SQLServerLogo.png")
        Me.ImageList.Images.SetKeyName(10, "databaseODBC.jpg")
        Me.ImageList.Images.SetKeyName(11, "PostgeSQL.png")
        Me.ImageList.Images.SetKeyName(12, "Snowflake16x16.png")
        Me.ImageList.Images.SetKeyName(13, "TypeDB16x16.png")
        Me.ImageList.Images.SetKeyName(14, "Neo4j16x16.png")
        Me.ImageList.Images.SetKeyName(15, "UML-UseCase16x16.png")
        Me.ImageList.Images.SetKeyName(16, "BPMN-ChoreographyDiagram-16x16.png")
        Me.ImageList.Images.SetKeyName(17, "BPMN-CollaborationDiagram-16x16.png")
        Me.ImageList.Images.SetKeyName(18, "BPMN-ConversationDiagram-16x16.png")
        Me.ImageList.Images.SetKeyName(19, "BPMN-ProcessDiagram-16x16.png")
        Me.ImageList.Images.SetKeyName(20, "DatabaseXML16x16.jpg")
        Me.ImageList.Images.SetKeyName(21, "RelationalAI16x16.png")
        Me.ImageList.Images.SetKeyName(22, "Kuzu16x16.png")
        Me.ImageList.Images.SetKeyName(23, "EdgeDB16x16.png")
        Me.ImageList.Images.SetKeyName(24, "Enterprise16x16.png")
        Me.ImageList.Images.SetKeyName(25, "Solution16x16.png")
        Me.ImageList.Images.SetKeyName(26, "XSD16x16.png")
        Me.ImageList.Images.SetKeyName(27, "FactEngine16x16.png")
        '
        'CircularProgressBar
        '
        Me.CircularProgressBar.AnimationFunction = Nothing
        Me.CircularProgressBar.AnimationSpeed = 100
        Me.CircularProgressBar.BackColor = System.Drawing.Color.White
        Me.CircularProgressBar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CircularProgressBar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.CircularProgressBar.InnerColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.CircularProgressBar.InnerMargin = 2
        Me.CircularProgressBar.InnerWidth = -1
        Me.CircularProgressBar.Location = New System.Drawing.Point(49, 74)
        Me.CircularProgressBar.MarqueeAnimationSpeed = 1000
        Me.CircularProgressBar.Name = "CircularProgressBar"
        Me.CircularProgressBar.OuterColor = System.Drawing.Color.LightGray
        Me.CircularProgressBar.OuterMargin = -25
        Me.CircularProgressBar.OuterWidth = 26
        Me.CircularProgressBar.ProgressColor = System.Drawing.Color.SteelBlue
        Me.CircularProgressBar.ProgressWidth = 6
        Me.CircularProgressBar.SecondaryFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CircularProgressBar.Size = New System.Drawing.Size(55, 55)
        Me.CircularProgressBar.StartAngle = 270
        Me.CircularProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.CircularProgressBar.SubscriptMargin = New System.Windows.Forms.Padding(-4, -35, 0, 0)
        Me.CircularProgressBar.SubscriptText = ""
        Me.CircularProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(CType(CType(166, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.CircularProgressBar.SuperscriptMargin = New System.Windows.Forms.Padding(15, 30, 0, 0)
        Me.CircularProgressBar.SuperscriptText = ""
        Me.CircularProgressBar.TabIndex = 5
        Me.CircularProgressBar.Text = "0%"
        Me.CircularProgressBar.TextMargin = New System.Windows.Forms.Padding(0, 1, 0, 0)
        Me.CircularProgressBar.Value = 68
        '
        'Timer_FormSetup
        '
        Me.Timer_FormSetup.Enabled = True
        '
        'ContextMenuStrip_Page
        '
        Me.ContextMenuStrip_Page.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_Page.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditPageToolStripMenuItem, Me.DeletePageToolStripMenuItem, Me.ToolStripMenuItemEditPageAsORMDiagram, Me.ToolStripMenuItem2, Me.ToolStripSeparator4, Me.CopyPageToolStripMenuItem})
        Me.ContextMenuStrip_Page.Name = "ContextMenuStrip_Page"
        Me.ContextMenuStrip_Page.Size = New System.Drawing.Size(224, 160)
        '
        'EditPageToolStripMenuItem
        '
        Me.EditPageToolStripMenuItem.Image = CType(resources.GetObject("EditPageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EditPageToolStripMenuItem.Name = "EditPageToolStripMenuItem"
        Me.EditPageToolStripMenuItem.Size = New System.Drawing.Size(223, 30)
        Me.EditPageToolStripMenuItem.Text = "&Edit Page"
        '
        'DeletePageToolStripMenuItem
        '
        Me.DeletePageToolStripMenuItem.Image = CType(resources.GetObject("DeletePageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeletePageToolStripMenuItem.Name = "DeletePageToolStripMenuItem"
        Me.DeletePageToolStripMenuItem.Size = New System.Drawing.Size(223, 30)
        Me.DeletePageToolStripMenuItem.Text = "&Delete Page"
        '
        'ToolStripMenuItemEditPageAsORMDiagram
        '
        Me.ToolStripMenuItemEditPageAsORMDiagram.Name = "ToolStripMenuItemEditPageAsORMDiagram"
        Me.ToolStripMenuItemEditPageAsORMDiagram.Size = New System.Drawing.Size(223, 30)
        Me.ToolStripMenuItemEditPageAsORMDiagram.Text = "Edit Page as &ORM Diagram"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(223, 30)
        Me.ToolStripMenuItem2.Text = "&Rename"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(220, 6)
        '
        'CopyPageToolStripMenuItem
        '
        Me.CopyPageToolStripMenuItem.Image = CType(resources.GetObject("CopyPageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyPageToolStripMenuItem.Name = "CopyPageToolStripMenuItem"
        Me.CopyPageToolStripMenuItem.Size = New System.Drawing.Size(223, 30)
        Me.CopyPageToolStripMenuItem.Text = "&Copy Page"
        '
        'ContextMenuStrip_ORMModel
        '
        Me.ContextMenuStrip_ORMModel.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip_ORMModel.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewModelDictionaryToolStripMenuItem, Me.ViewGlossaryToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.ToolStripSeparator7, Me.ToolStripMenuItemLanguage, Me.ToolStripMenuItemPastePage, Me.ToolStripSeparator1, Me.AddXSDToolStripMenuItem, Me.ToolStripSeparator6, Me.ToolStripMenuItemEmptyModel, Me.HideToolStripMenuItem, Me.HideAllotherModelsToolStripMenuItem, Me.ImportExportToolStripMenuItem, Me.RenameToolStripMenuItem, Me.ToolStripMenuItemMoveModel, Me.ToolStripMenuItemShareModelWithProject, Me.DeleteModelToolStripMenuItem, Me.ToolStripSeparator3, Me.ToolStripMenuItemModelConfiguration, Me.ToolStripSeparator5, Me.ToolStripMenuItemFixModelErrors})
        Me.ContextMenuStrip_ORMModel.Name = "ContextMenuStrip_ORMModel"
        Me.ContextMenuStrip_ORMModel.Size = New System.Drawing.Size(231, 472)
        '
        'ViewModelDictionaryToolStripMenuItem
        '
        Me.ViewModelDictionaryToolStripMenuItem.Image = CType(resources.GetObject("ViewModelDictionaryToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ViewModelDictionaryToolStripMenuItem.Name = "ViewModelDictionaryToolStripMenuItem"
        Me.ViewModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.ViewModelDictionaryToolStripMenuItem.Text = "Model Di&ctionary"
        '
        'ViewGlossaryToolStripMenuItem
        '
        Me.ViewGlossaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Glossary16x16
        Me.ViewGlossaryToolStripMenuItem.Name = "ViewGlossaryToolStripMenuItem"
        Me.ViewGlossaryToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.ViewGlossaryToolStripMenuItem.Text = "&Glossary"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemViewDatabaseSchema, Me.GenerateDocumentationToolStripMenuItem, Me.ToolStripMenuItemCodeGenerator, Me.FactEngineToolStripMenuItem, Me.ToolStripMenuItemTaxonomyTree, Me.ToolStripMenuItemKeywordExtractionTool, Me.FEKLUploaderToolStripMenuItem, Me.CLIFGeneratorToolStripMenuItem, Me.RDFGeneratorToolStripMenuItem, Me.UMSGeneratorProcessorToolStripMenuItem, Me.OssieGeneratorProcessorToolStripMenuItem, Me.EditAITrainingDataEditorToolStripMenuItem, Me.VirtualBusinessAnalystToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'ToolStripMenuItemViewDatabaseSchema
        '
        Me.ToolStripMenuItemViewDatabaseSchema.Image = Global.Boston.My.Resources.Resources.DatabaseSchema
        Me.ToolStripMenuItemViewDatabaseSchema.Name = "ToolStripMenuItemViewDatabaseSchema"
        Me.ToolStripMenuItemViewDatabaseSchema.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItemViewDatabaseSchema.Text = "Database Schema"
        '
        'GenerateDocumentationToolStripMenuItem
        '
        Me.GenerateDocumentationToolStripMenuItem.Image = CType(resources.GetObject("GenerateDocumentationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.GenerateDocumentationToolStripMenuItem.Name = "GenerateDocumentationToolStripMenuItem"
        Me.GenerateDocumentationToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.GenerateDocumentationToolStripMenuItem.Text = "Generate D&ocumentation"
        '
        'ToolStripMenuItemCodeGenerator
        '
        Me.ToolStripMenuItemCodeGenerator.Image = CType(resources.GetObject("ToolStripMenuItemCodeGenerator.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemCodeGenerator.Name = "ToolStripMenuItemCodeGenerator"
        Me.ToolStripMenuItemCodeGenerator.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItemCodeGenerator.Text = "Code &Generator"
        '
        'FactEngineToolStripMenuItem
        '
        Me.FactEngineToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.FactEngine16x16
        Me.FactEngineToolStripMenuItem.Name = "FactEngineToolStripMenuItem"
        Me.FactEngineToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.FactEngineToolStripMenuItem.Text = "&Fact Engine"
        '
        'ToolStripMenuItemTaxonomyTree
        '
        Me.ToolStripMenuItemTaxonomyTree.Image = Global.Boston.My.Resources.Resources.TaxonomyModel16x16
        Me.ToolStripMenuItemTaxonomyTree.Name = "ToolStripMenuItemTaxonomyTree"
        Me.ToolStripMenuItemTaxonomyTree.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItemTaxonomyTree.Text = "Taxonomy Tree"
        '
        'ToolStripMenuItemKeywordExtractionTool
        '
        Me.ToolStripMenuItemKeywordExtractionTool.Image = CType(resources.GetObject("ToolStripMenuItemKeywordExtractionTool.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemKeywordExtractionTool.Name = "ToolStripMenuItemKeywordExtractionTool"
        Me.ToolStripMenuItemKeywordExtractionTool.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItemKeywordExtractionTool.Text = "&Knowledge Extraction Tools"
        '
        'FEKLUploaderToolStripMenuItem
        '
        Me.FEKLUploaderToolStripMenuItem.Image = CType(resources.GetObject("FEKLUploaderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.FEKLUploaderToolStripMenuItem.Name = "FEKLUploaderToolStripMenuItem"
        Me.FEKLUploaderToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.FEKLUploaderToolStripMenuItem.Text = "FEKL Uploader"
        '
        'CLIFGeneratorToolStripMenuItem
        '
        Me.CLIFGeneratorToolStripMenuItem.Name = "CLIFGeneratorToolStripMenuItem"
        Me.CLIFGeneratorToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.CLIFGeneratorToolStripMenuItem.Text = "&CLIF Generator"
        '
        'RDFGeneratorToolStripMenuItem
        '
        Me.RDFGeneratorToolStripMenuItem.Name = "RDFGeneratorToolStripMenuItem"
        Me.RDFGeneratorToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.RDFGeneratorToolStripMenuItem.Text = "&RDF Generator"
        '
        'UMSGeneratorProcessorToolStripMenuItem
        '
        Me.UMSGeneratorProcessorToolStripMenuItem.Name = "UMSGeneratorProcessorToolStripMenuItem"
        Me.UMSGeneratorProcessorToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.UMSGeneratorProcessorToolStripMenuItem.Text = "&UMS Generator / Processor"
        '
        'OssieGeneratorProcessorToolStripMenuItem
        '
        Me.OssieGeneratorProcessorToolStripMenuItem.Name = "OssieGeneratorProcessorToolStripMenuItem"
        Me.OssieGeneratorProcessorToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.OssieGeneratorProcessorToolStripMenuItem.Text = "&Ossie Generator / Processor"
        '
        'EditAITrainingDataEditorToolStripMenuItem
        '
        Me.EditAITrainingDataEditorToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.AI16x16
        Me.EditAITrainingDataEditorToolStripMenuItem.Name = "EditAITrainingDataEditorToolStripMenuItem"
        Me.EditAITrainingDataEditorToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.EditAITrainingDataEditorToolStripMenuItem.Text = "Edit AI &Training Data"
        '
        'VirtualBusinessAnalystToolStripMenuItem
        '
        Me.VirtualBusinessAnalystToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.VirtualAnalyst16x16
        Me.VirtualBusinessAnalystToolStripMenuItem.Name = "VirtualBusinessAnalystToolStripMenuItem"
        Me.VirtualBusinessAnalystToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.VirtualBusinessAnalystToolStripMenuItem.Text = "&Virtual Business Analyst"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripMenuItemLanguage
        '
        Me.ToolStripMenuItemLanguage.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddORMPageToolStripMenuItem, Me.PropertyGraphSchemaToolStripMenuItem, Me.EntityRelationshipDiagramToolStripMenuItem, Me.StateTransitionDiagramToolStripMenuItem, Me.BusinessProcessModellingNotationToolStripMenuItem, Me.UMLToolStripMenuItem, Me.StructureChartToolStripMenuItem, Me.FlowchartToolStripMenuItem})
        Me.ToolStripMenuItemLanguage.Image = CType(resources.GetObject("ToolStripMenuItemLanguage.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemLanguage.Name = "ToolStripMenuItemLanguage"
        Me.ToolStripMenuItemLanguage.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemLanguage.Text = "Add Page (Other &Languages)"
        '
        'AddORMPageToolStripMenuItem
        '
        Me.AddORMPageToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.PageAdd16x16
        Me.AddORMPageToolStripMenuItem.Name = "AddORMPageToolStripMenuItem"
        Me.AddORMPageToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.AddORMPageToolStripMenuItem.Text = "Add &ORM Page"
        '
        'PropertyGraphSchemaToolStripMenuItem
        '
        Me.PropertyGraphSchemaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPGSPageToolStripMenuItem})
        Me.PropertyGraphSchemaToolStripMenuItem.Image = CType(resources.GetObject("PropertyGraphSchemaToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PropertyGraphSchemaToolStripMenuItem.Name = "PropertyGraphSchemaToolStripMenuItem"
        Me.PropertyGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.PropertyGraphSchemaToolStripMenuItem.Text = "&Property Graph Schema"
        '
        'AddPGSPageToolStripMenuItem
        '
        Me.AddPGSPageToolStripMenuItem.Image = CType(resources.GetObject("AddPGSPageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddPGSPageToolStripMenuItem.Name = "AddPGSPageToolStripMenuItem"
        Me.AddPGSPageToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.AddPGSPageToolStripMenuItem.Text = "&Add PGS Page"
        '
        'EntityRelationshipDiagramToolStripMenuItem
        '
        Me.EntityRelationshipDiagramToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddERDPageToolStripMenuItem})
        Me.EntityRelationshipDiagramToolStripMenuItem.Image = CType(resources.GetObject("EntityRelationshipDiagramToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EntityRelationshipDiagramToolStripMenuItem.Name = "EntityRelationshipDiagramToolStripMenuItem"
        Me.EntityRelationshipDiagramToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.EntityRelationshipDiagramToolStripMenuItem.Text = "&Entity Relationship Diagram"
        '
        'AddERDPageToolStripMenuItem
        '
        Me.AddERDPageToolStripMenuItem.Name = "AddERDPageToolStripMenuItem"
        Me.AddERDPageToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.AddERDPageToolStripMenuItem.Text = "&Add ERD Page"
        '
        'StateTransitionDiagramToolStripMenuItem
        '
        Me.StateTransitionDiagramToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddSTDPageToolStripMenuItem})
        Me.StateTransitionDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.StateTransitionDiagram_16x16
        Me.StateTransitionDiagramToolStripMenuItem.Name = "StateTransitionDiagramToolStripMenuItem"
        Me.StateTransitionDiagramToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.StateTransitionDiagramToolStripMenuItem.Text = "&State Transition Diagram"
        '
        'AddSTDPageToolStripMenuItem
        '
        Me.AddSTDPageToolStripMenuItem.Name = "AddSTDPageToolStripMenuItem"
        Me.AddSTDPageToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.AddSTDPageToolStripMenuItem.Text = "&Add STD Page"
        '
        'BusinessProcessModellingNotationToolStripMenuItem
        '
        Me.BusinessProcessModellingNotationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddBPMNPageToolStripMenuItem, Me.AddBPMNChoreographyDiagramPageToolStripMenuItem, Me.AddBPMNCollaborationDiagramPageToolStripMenuItem, Me.AddProcessDiagramPageToolStripMenuItem})
        Me.BusinessProcessModellingNotationToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.BPMN16x16
        Me.BusinessProcessModellingNotationToolStripMenuItem.Name = "BusinessProcessModellingNotationToolStripMenuItem"
        Me.BusinessProcessModellingNotationToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.BusinessProcessModellingNotationToolStripMenuItem.Text = "&Business Process Modelling Notation"
        Me.BusinessProcessModellingNotationToolStripMenuItem.Visible = False
        '
        'AddBPMNPageToolStripMenuItem
        '
        Me.AddBPMNPageToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.BPMN_ConversationDiagram_16x16
        Me.AddBPMNPageToolStripMenuItem.Name = "AddBPMNPageToolStripMenuItem"
        Me.AddBPMNPageToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.AddBPMNPageToolStripMenuItem.Text = "&Add Conversation Diagram Page"
        '
        'AddBPMNChoreographyDiagramPageToolStripMenuItem
        '
        Me.AddBPMNChoreographyDiagramPageToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.BPMN_ChoreographyDiagram_16x16
        Me.AddBPMNChoreographyDiagramPageToolStripMenuItem.Name = "AddBPMNChoreographyDiagramPageToolStripMenuItem"
        Me.AddBPMNChoreographyDiagramPageToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.AddBPMNChoreographyDiagramPageToolStripMenuItem.Text = "Add Choreography Diagram Page"
        '
        'AddBPMNCollaborationDiagramPageToolStripMenuItem
        '
        Me.AddBPMNCollaborationDiagramPageToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.BPMN_CollaborationDiagram_16x16
        Me.AddBPMNCollaborationDiagramPageToolStripMenuItem.Name = "AddBPMNCollaborationDiagramPageToolStripMenuItem"
        Me.AddBPMNCollaborationDiagramPageToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.AddBPMNCollaborationDiagramPageToolStripMenuItem.Text = "Add Collaboration Diagram Page"
        '
        'AddProcessDiagramPageToolStripMenuItem
        '
        Me.AddProcessDiagramPageToolStripMenuItem.Image = CType(resources.GetObject("AddProcessDiagramPageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddProcessDiagramPageToolStripMenuItem.Name = "AddProcessDiagramPageToolStripMenuItem"
        Me.AddProcessDiagramPageToolStripMenuItem.Size = New System.Drawing.Size(252, 22)
        Me.AddProcessDiagramPageToolStripMenuItem.Text = "Add &Process Diagram Page"
        '
        'UMLToolStripMenuItem
        '
        Me.UMLToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddUseCaseDiagramToolStripMenuItem})
        Me.UMLToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.UML16x16
        Me.UMLToolStripMenuItem.Name = "UMLToolStripMenuItem"
        Me.UMLToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.UMLToolStripMenuItem.Text = "UML"
        '
        'AddUseCaseDiagramToolStripMenuItem
        '
        Me.AddUseCaseDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.UML_UseCase16x16
        Me.AddUseCaseDiagramToolStripMenuItem.Name = "AddUseCaseDiagramToolStripMenuItem"
        Me.AddUseCaseDiagramToolStripMenuItem.Size = New System.Drawing.Size(194, 22)
        Me.AddUseCaseDiagramToolStripMenuItem.Text = "Add &Use Case Diagram"
        '
        'StructureChartToolStripMenuItem
        '
        Me.StructureChartToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.StructureChart
        Me.StructureChartToolStripMenuItem.Name = "StructureChartToolStripMenuItem"
        Me.StructureChartToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.StructureChartToolStripMenuItem.Text = "Structure Chart"
        '
        'FlowchartToolStripMenuItem
        '
        Me.FlowchartToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Flowchart
        Me.FlowchartToolStripMenuItem.Name = "FlowchartToolStripMenuItem"
        Me.FlowchartToolStripMenuItem.Size = New System.Drawing.Size(273, 26)
        Me.FlowchartToolStripMenuItem.Text = "Flowchart"
        '
        'ToolStripMenuItemPastePage
        '
        Me.ToolStripMenuItemPastePage.Image = CType(resources.GetObject("ToolStripMenuItemPastePage.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemPastePage.Name = "ToolStripMenuItemPastePage"
        Me.ToolStripMenuItemPastePage.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemPastePage.Text = "&Paste Page"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(227, 6)
        '
        'AddXSDToolStripMenuItem
        '
        Me.AddXSDToolStripMenuItem.Image = CType(resources.GetObject("AddXSDToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddXSDToolStripMenuItem.Name = "AddXSDToolStripMenuItem"
        Me.AddXSDToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.AddXSDToolStripMenuItem.Text = "Add &XSD"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripMenuItemEmptyModel
        '
        Me.ToolStripMenuItemEmptyModel.Image = CType(resources.GetObject("ToolStripMenuItemEmptyModel.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemEmptyModel.Name = "ToolStripMenuItemEmptyModel"
        Me.ToolStripMenuItemEmptyModel.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemEmptyModel.Text = "E&mpty Model"
        Me.ToolStripMenuItemEmptyModel.Visible = False
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.HideToolStripMenuItem.Text = "&Hide"
        '
        'HideAllotherModelsToolStripMenuItem
        '
        Me.HideAllotherModelsToolStripMenuItem.Name = "HideAllotherModelsToolStripMenuItem"
        Me.HideAllotherModelsToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.HideAllotherModelsToolStripMenuItem.Text = "Hide all &other models"
        '
        'ImportExportToolStripMenuItem
        '
        Me.ImportExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem12, Me.ExportTestingToolStripMenuItem, Me.TocqlFileToolStripMenuItem, Me.ToolStripMenuItemExportToNORMAormFile, Me.ToFEKLtxtFileToolStripMenuItem, Me.RelationalDataStructureToXMLToolStripMenuItem, Me.RDFToolStripMenuItem, Me.TofbmExchangeMetaModelToolStripMenuItem, Me.TofigFactInterchangeGrammarToolStripMenuItem})
        Me.ImportExportToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Export16x16
        Me.ImportExportToolStripMenuItem.Name = "ImportExportToolStripMenuItem"
        Me.ImportExportToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.ImportExportToolStripMenuItem.Text = "&Export"
        '
        'ToolStripMenuItem12
        '
        Me.ToolStripMenuItem12.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FromORMCMMLFileToolStripMenuItem})
        Me.ToolStripMenuItem12.Image = CType(resources.GetObject("ToolStripMenuItem12.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem12.Name = "ToolStripMenuItem12"
        Me.ToolStripMenuItem12.Size = New System.Drawing.Size(259, 22)
        Me.ToolStripMenuItem12.Text = "&Import"
        Me.ToolStripMenuItem12.Visible = False
        '
        'FromORMCMMLFileToolStripMenuItem
        '
        Me.FromORMCMMLFileToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.XML16x16
        Me.FromORMCMMLFileToolStripMenuItem.Name = "FromORMCMMLFileToolStripMenuItem"
        Me.FromORMCMMLFileToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.FromORMCMMLFileToolStripMenuItem.Text = "From .fbm file"
        '
        'ExportTestingToolStripMenuItem
        '
        Me.ExportTestingToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.XML16x16
        Me.ExportTestingToolStripMenuItem.Name = "ExportTestingToolStripMenuItem"
        Me.ExportTestingToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.ExportTestingToolStripMenuItem.Text = "To .&fbm File"
        '
        'TocqlFileToolStripMenuItem
        '
        Me.TocqlFileToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.CQL16x16
        Me.TocqlFileToolStripMenuItem.Name = "TocqlFileToolStripMenuItem"
        Me.TocqlFileToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.TocqlFileToolStripMenuItem.Text = "To .&cql File"
        Me.TocqlFileToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItemExportToNORMAormFile
        '
        Me.ToolStripMenuItemExportToNORMAormFile.Image = Global.Boston.My.Resources.Resources.NORMA16x16
        Me.ToolStripMenuItemExportToNORMAormFile.Name = "ToolStripMenuItemExportToNORMAormFile"
        Me.ToolStripMenuItemExportToNORMAormFile.Size = New System.Drawing.Size(259, 22)
        Me.ToolStripMenuItemExportToNORMAormFile.Text = "To NORMA .orm File (beta)"
        '
        'ToFEKLtxtFileToolStripMenuItem
        '
        Me.ToFEKLtxtFileToolStripMenuItem.Name = "ToFEKLtxtFileToolStripMenuItem"
        Me.ToFEKLtxtFileToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.ToFEKLtxtFileToolStripMenuItem.Text = "To FEKL .txt File (beta)"
        '
        'RelationalDataStructureToXMLToolStripMenuItem
        '
        Me.RelationalDataStructureToXMLToolStripMenuItem.Name = "RelationalDataStructureToXMLToolStripMenuItem"
        Me.RelationalDataStructureToXMLToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.RelationalDataStructureToXMLToolStripMenuItem.Text = "Relational Data Structure to XML"
        '
        'RDFToolStripMenuItem
        '
        Me.RDFToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToRDFTurtlettlFilebetaToolStripMenuItem, Me.ToRDFOWLToolStripMenuItem, Me.ToRDSTriGToolStripMenuItem, Me.ToRDSTriGWithSHACLToolStripMenuItem})
        Me.RDFToolStripMenuItem.Name = "RDFToolStripMenuItem"
        Me.RDFToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.RDFToolStripMenuItem.Text = "&RDF"
        '
        'ToRDFTurtlettlFilebetaToolStripMenuItem
        '
        Me.ToRDFTurtlettlFilebetaToolStripMenuItem.Name = "ToRDFTurtlettlFilebetaToolStripMenuItem"
        Me.ToRDFTurtlettlFilebetaToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.ToRDFTurtlettlFilebetaToolStripMenuItem.Text = "To RDF Turtle .ttl File (beta)"
        '
        'ToRDFOWLToolStripMenuItem
        '
        Me.ToRDFOWLToolStripMenuItem.Name = "ToRDFOWLToolStripMenuItem"
        Me.ToRDFOWLToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.ToRDFOWLToolStripMenuItem.Text = "to RDF OWL"
        '
        'ToRDSTriGToolStripMenuItem
        '
        Me.ToRDSTriGToolStripMenuItem.Name = "ToRDSTriGToolStripMenuItem"
        Me.ToRDSTriGToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.ToRDSTriGToolStripMenuItem.Text = "to RDS TriG"
        '
        'ToRDSTriGWithSHACLToolStripMenuItem
        '
        Me.ToRDSTriGWithSHACLToolStripMenuItem.Name = "ToRDSTriGWithSHACLToolStripMenuItem"
        Me.ToRDSTriGWithSHACLToolStripMenuItem.Size = New System.Drawing.Size(217, 22)
        Me.ToRDSTriGWithSHACLToolStripMenuItem.Text = "to RDS TriG with SHACL"
        '
        'TofbmExchangeMetaModelToolStripMenuItem
        '
        Me.TofbmExchangeMetaModelToolStripMenuItem.Name = "TofbmExchangeMetaModelToolStripMenuItem"
        Me.TofbmExchangeMetaModelToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.TofbmExchangeMetaModelToolStripMenuItem.Text = "To .fbm E&xchange MetaModel"
        '
        'TofigFactInterchangeGrammarToolStripMenuItem
        '
        Me.TofigFactInterchangeGrammarToolStripMenuItem.Name = "TofigFactInterchangeGrammarToolStripMenuItem"
        Me.TofigFactInterchangeGrammarToolStripMenuItem.Size = New System.Drawing.Size(259, 22)
        Me.TofigFactInterchangeGrammarToolStripMenuItem.Text = "To .fig (Fact Interchange &Grammar)"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.RenameToolStripMenuItem.Text = "&Rename"
        '
        'ToolStripMenuItemMoveModel
        '
        Me.ToolStripMenuItemMoveModel.Name = "ToolStripMenuItemMoveModel"
        Me.ToolStripMenuItemMoveModel.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemMoveModel.Text = "&Move Model"
        '
        'ToolStripMenuItemShareModelWithProject
        '
        Me.ToolStripMenuItemShareModelWithProject.Name = "ToolStripMenuItemShareModelWithProject"
        Me.ToolStripMenuItemShareModelWithProject.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemShareModelWithProject.Text = "Share with Project..."
        '
        'DeleteModelToolStripMenuItem
        '
        Me.DeleteModelToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.DeleteModelToolStripMenuItem.Name = "DeleteModelToolStripMenuItem"
        Me.DeleteModelToolStripMenuItem.Size = New System.Drawing.Size(230, 26)
        Me.DeleteModelToolStripMenuItem.Text = "&Delete Model"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripMenuItemModelConfiguration
        '
        Me.ToolStripMenuItemModelConfiguration.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemModelConfiguration.Name = "ToolStripMenuItemModelConfiguration"
        Me.ToolStripMenuItemModelConfiguration.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemModelConfiguration.Text = "Model Con&figuration"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(227, 6)
        '
        'ToolStripMenuItemFixModelErrors
        '
        Me.ToolStripMenuItemFixModelErrors.Name = "ToolStripMenuItemFixModelErrors"
        Me.ToolStripMenuItemFixModelErrors.Size = New System.Drawing.Size(230, 26)
        Me.ToolStripMenuItemFixModelErrors.Text = "Fi&x Model Errors"
        Me.ToolStripMenuItemFixModelErrors.Visible = False
        '
        'HelpProvider
        '
        Me.HelpProvider.HelpNamespace = ".\richmondhelp\Boston.chm"
        '
        'BackgroundWorkerModelLoader
        '
        Me.BackgroundWorkerModelLoader.WorkerReportsProgress = True
        '
        'ContextMenuStripSolution
        '
        Me.ContextMenuStripSolution.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CreateATicketToolStripMenuItem})
        Me.ContextMenuStripSolution.Name = "ContextMenuStripSolution"
        Me.ContextMenuStripSolution.Size = New System.Drawing.Size(153, 26)
        '
        'CreateATicketToolStripMenuItem
        '
        Me.CreateATicketToolStripMenuItem.Name = "CreateATicketToolStripMenuItem"
        Me.CreateATicketToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CreateATicketToolStripMenuItem.Text = "Create a &Ticket"
        '
        'ContextMenuStripXSD
        '
        Me.ContextMenuStripXSD.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem, Me.DeleteToolStripMenuItem})
        Me.ContextMenuStripXSD.Name = "ContextMenuStripXSD"
        Me.ContextMenuStripXSD.Size = New System.Drawing.Size(108, 48)
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.DeleteToolStripMenuItem.Text = "&Delete"
        '
        'frmToolboxEnterpriseExplorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(290, 559)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxEnterpriseExplorer"
        Me.TabText = "Model Explorer"
        Me.Text = "Model Explorer"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.PanelProjectControls.ResumeLayout(False)
        Me.PanelProjectControls.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ContextMenuStrip_ORMModels.ResumeLayout(False)
        Me.ContextMenuStrip_Page.ResumeLayout(False)
        Me.ContextMenuStrip_ORMModel.ResumeLayout(False)
        Me.ContextMenuStripSolution.ResumeLayout(False)
        Me.ContextMenuStripXSD.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer_FormSetup As System.Windows.Forms.Timer
    Friend WithEvents ImageList As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip_Page As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeletePageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ORMModel As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ViewModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DialogFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents DialogOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ContextMenuStrip_ORMModels As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemAddModel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider As System.Windows.Forms.HelpProvider
    Friend WithEvents ImportExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem12 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEmptyModel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemPastePage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FromORMCMMLFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemLanguage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertyGraphSchemaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddPGSPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEditPageAsORMDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackgroundWorkerModelLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolStripMenuItemModelConfiguration As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CircularProgressBar As CircularProgressBar.CircularProgressBar
    Friend WithEvents EntityRelationshipDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddERDPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewGlossaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComboBoxProject As System.Windows.Forms.ComboBox
    Friend WithEvents LabelPromptProject As System.Windows.Forms.Label
    Friend WithEvents ComboBoxNamespace As System.Windows.Forms.ComboBox
    Friend WithEvents LabelPromptNamespace As System.Windows.Forms.Label
    Friend WithEvents GenerateDocumentationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents StateTransitionDiagramToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddSTDPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ButtonNewModel As Button
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStripMenuItemCodeGenerator As ToolStripMenuItem
    Friend WithEvents TreeView As Boston.BostonTreeView
    Friend WithEvents FactEngineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UnhideHiddenModelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ExportTestingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TocqlFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideAllotherModelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFixModelErrors As ToolStripMenuItem
    Friend WithEvents UnhideASelectedModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemKeywordExtractionTool As ToolStripMenuItem
    Friend WithEvents BusinessProcessModellingNotationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddBPMNPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemTaxonomyTree As ToolStripMenuItem
    Friend WithEvents AddBPMNChoreographyDiagramPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddBPMNCollaborationDiagramPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UMLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddUseCaseDiagramToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddProcessDiagramPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemExportToNORMAormFile As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemMoveModel As ToolStripMenuItem
    Friend WithEvents FEKLUploaderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditAITrainingDataEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SearchTextbox As CustomSearchTextbox
    Friend WithEvents ToolStripMenuItemViewDatabaseSchema As ToolStripMenuItem
    Friend WithEvents VirtualBusinessAnalystToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToFEKLtxtFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
    Friend WithEvents FromRDFOWLTurtlettlFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemShareModelWithProject As ToolStripMenuItem
    Friend WithEvents ImportormNORMAFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PanelProjectControls As Panel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents LabelHelpTips As Label
    Friend WithEvents RelationalDataStructureToXMLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StructureChartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RDFToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToRDFTurtlettlFilebetaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToRDFOWLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToRDSTriGToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToRDSTriGWithSHACLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FlowchartToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CLIFGeneratorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RDFGeneratorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripSolution As ContextMenuStrip
    Friend WithEvents CreateATicketToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddXSDToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ContextMenuStripXSD As ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TofbmExchangeMetaModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FromfbmStandardFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FromXMLDocumentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TofigFactInterchangeGrammarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UMSGeneratorProcessorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OssieGeneratorProcessorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddORMPageToolStripMenuItem As ToolStripMenuItem
End Class
