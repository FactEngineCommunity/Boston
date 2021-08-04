<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.GroupBox_main = New System.Windows.Forms.GroupBox()
        Me.ButtonNewModel = New System.Windows.Forms.Button()
        Me.ComboBoxNamespace = New System.Windows.Forms.ComboBox()
        Me.LabelPromptNamespace = New System.Windows.Forms.Label()
        Me.ComboBoxProject = New System.Windows.Forms.ComboBox()
        Me.LabelPromptProject = New System.Windows.Forms.Label()
        Me.LabelHelpTips = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
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
        Me.GenerateDocumentationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CodeGenerationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FactEngineToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddPageToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemPastePage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemLanguage = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertyGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddPGSPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EntityRelationshipDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddERDPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StateTransitionDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddSTDPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.EmptyModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideAllotherModelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemModelConfiguration = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImportExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem12 = New System.Windows.Forms.ToolStripMenuItem()
        Me.FromORMCMMLFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportTestingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TocqlFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemFixModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.DialogFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.DialogOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.ContextMenuStrip_ORMModels = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemAddModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnhideHiddenModelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpProvider = New System.Windows.Forms.HelpProvider()
        Me.BackgroundWorkerModelLoader = New System.ComponentModel.BackgroundWorker()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TreeView = New Boston.BostonTreeView()
        Me.GroupBox_main.SuspendLayout()
        Me.ContextMenuStrip_Page.SuspendLayout()
        Me.ContextMenuStrip_ORMModel.SuspendLayout()
        Me.ContextMenuStrip_ORMModels.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox_main
        '
        Me.GroupBox_main.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox_main.Controls.Add(Me.ButtonNewModel)
        Me.GroupBox_main.Controls.Add(Me.ComboBoxNamespace)
        Me.GroupBox_main.Controls.Add(Me.LabelPromptNamespace)
        Me.GroupBox_main.Controls.Add(Me.ComboBoxProject)
        Me.GroupBox_main.Controls.Add(Me.LabelPromptProject)
        Me.GroupBox_main.Controls.Add(Me.LabelHelpTips)
        Me.GroupBox_main.Controls.Add(Me.Button1)
        Me.GroupBox_main.Controls.Add(Me.TextBox1)
        Me.GroupBox_main.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_main.Name = "GroupBox_main"
        Me.GroupBox_main.Size = New System.Drawing.Size(383, 588)
        Me.GroupBox_main.TabIndex = 0
        Me.GroupBox_main.TabStop = False
        '
        'ButtonNewModel
        '
        Me.ButtonNewModel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonNewModel.Image = Global.Boston.My.Resources.MenuImagesMain.ModelAdd16x16
        Me.ButtonNewModel.Location = New System.Drawing.Point(355, 22)
        Me.ButtonNewModel.Name = "ButtonNewModel"
        Me.ButtonNewModel.Size = New System.Drawing.Size(22, 23)
        Me.ButtonNewModel.TabIndex = 10
        Me.ToolTip.SetToolTip(Me.ButtonNewModel, "Add a Model to Boston")
        Me.ButtonNewModel.UseVisualStyleBackColor = True
        '
        'ComboBoxNamespace
        '
        Me.ComboBoxNamespace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNamespace.FormattingEnabled = True
        Me.ComboBoxNamespace.Location = New System.Drawing.Point(246, 0)
        Me.ComboBoxNamespace.Name = "ComboBoxNamespace"
        Me.ComboBoxNamespace.Size = New System.Drawing.Size(137, 21)
        Me.ComboBoxNamespace.TabIndex = 9
        Me.ComboBoxNamespace.Visible = False
        '
        'LabelPromptNamespace
        '
        Me.LabelPromptNamespace.AutoSize = True
        Me.LabelPromptNamespace.Location = New System.Drawing.Point(191, 3)
        Me.LabelPromptNamespace.Name = "LabelPromptNamespace"
        Me.LabelPromptNamespace.Size = New System.Drawing.Size(49, 13)
        Me.LabelPromptNamespace.TabIndex = 8
        Me.LabelPromptNamespace.Text = "NSpace:"
        Me.LabelPromptNamespace.Visible = False
        '
        'ComboBoxProject
        '
        Me.ComboBoxProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxProject.FormattingEnabled = True
        Me.ComboBoxProject.Location = New System.Drawing.Point(49, 0)
        Me.ComboBoxProject.Name = "ComboBoxProject"
        Me.ComboBoxProject.Size = New System.Drawing.Size(136, 21)
        Me.ComboBoxProject.Sorted = True
        Me.ComboBoxProject.TabIndex = 7
        '
        'LabelPromptProject
        '
        Me.LabelPromptProject.AutoSize = True
        Me.LabelPromptProject.Location = New System.Drawing.Point(0, 3)
        Me.LabelPromptProject.Name = "LabelPromptProject"
        Me.LabelPromptProject.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptProject.TabIndex = 6
        Me.LabelPromptProject.Text = "Project:"
        '
        'LabelHelpTips
        '
        Me.LabelHelpTips.BackColor = System.Drawing.Color.LemonChiffon
        Me.LabelHelpTips.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelpTips.Location = New System.Drawing.Point(3, 545)
        Me.LabelHelpTips.Name = "LabelHelpTips"
        Me.LabelHelpTips.Size = New System.Drawing.Size(377, 40)
        Me.LabelHelpTips.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(296, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(54, 22)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "S&earch"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(3, 24)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(289, 20)
        Me.TextBox1.TabIndex = 1
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
        Me.EditPageToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.edit16x16
        Me.EditPageToolStripMenuItem.Name = "EditPageToolStripMenuItem"
        Me.EditPageToolStripMenuItem.Size = New System.Drawing.Size(223, 30)
        Me.EditPageToolStripMenuItem.Text = "&Edit Page"
        '
        'DeletePageToolStripMenuItem
        '
        Me.DeletePageToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.deleteround16x16
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
        Me.CopyPageToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Copy16x16
        Me.CopyPageToolStripMenuItem.Name = "CopyPageToolStripMenuItem"
        Me.CopyPageToolStripMenuItem.Size = New System.Drawing.Size(223, 30)
        Me.CopyPageToolStripMenuItem.Text = "&Copy Page"
        '
        'ContextMenuStrip_ORMModel
        '
        Me.ContextMenuStrip_ORMModel.AutoSize = False
        Me.ContextMenuStrip_ORMModel.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewModelDictionaryToolStripMenuItem, Me.ViewGlossaryToolStripMenuItem, Me.GenerateDocumentationToolStripMenuItem, Me.CodeGenerationToolStripMenuItem, Me.FactEngineToolStripMenuItem, Me.ToolStripSeparator7, Me.AddPageToolStripMenuItem1, Me.ToolStripMenuItemPastePage, Me.ToolStripMenuItemLanguage, Me.ToolStripSeparator1, Me.EmptyModelToolStripMenuItem, Me.HideToolStripMenuItem, Me.HideAllotherModelsToolStripMenuItem, Me.RenameToolStripMenuItem, Me.DeleteModelToolStripMenuItem, Me.ToolStripSeparator3, Me.ToolStripMenuItemModelConfiguration, Me.ToolStripSeparator5, Me.ImportExportToolStripMenuItem, Me.ToolStripMenuItemFixModelErrors})
        Me.ContextMenuStrip_ORMModel.Name = "ContextMenuStrip_ORMModel"
        Me.ContextMenuStrip_ORMModel.Size = New System.Drawing.Size(216, 530)
        '
        'ViewModelDictionaryToolStripMenuItem
        '
        Me.ViewModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.dictionary16x16
        Me.ViewModelDictionaryToolStripMenuItem.Name = "ViewModelDictionaryToolStripMenuItem"
        Me.ViewModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewModelDictionaryToolStripMenuItem.Text = "View Model Di&ctionary"
        '
        'ViewGlossaryToolStripMenuItem
        '
        Me.ViewGlossaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Glossary16x16
        Me.ViewGlossaryToolStripMenuItem.Name = "ViewGlossaryToolStripMenuItem"
        Me.ViewGlossaryToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewGlossaryToolStripMenuItem.Text = "View &Glossary"
        '
        'GenerateDocumentationToolStripMenuItem
        '
        Me.GenerateDocumentationToolStripMenuItem.Image = CType(resources.GetObject("GenerateDocumentationToolStripMenuItem.Image"), System.Drawing.Image)
        Me.GenerateDocumentationToolStripMenuItem.Name = "GenerateDocumentationToolStripMenuItem"
        Me.GenerateDocumentationToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.GenerateDocumentationToolStripMenuItem.Text = "Generate D&ocumentation"
        '
        'CodeGenerationToolStripMenuItem
        '
        Me.CodeGenerationToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.Project16x16
        Me.CodeGenerationToolStripMenuItem.Name = "CodeGenerationToolStripMenuItem"
        Me.CodeGenerationToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CodeGenerationToolStripMenuItem.Text = "Code &Generation"
        '
        'FactEngineToolStripMenuItem
        '
        Me.FactEngineToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.FactEngine16x16
        Me.FactEngineToolStripMenuItem.Name = "FactEngineToolStripMenuItem"
        Me.FactEngineToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.FactEngineToolStripMenuItem.Text = "&Fact Engine"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(212, 6)
        '
        'AddPageToolStripMenuItem1
        '
        Me.AddPageToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.PageAdd16x16
        Me.AddPageToolStripMenuItem1.Name = "AddPageToolStripMenuItem1"
        Me.AddPageToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.AddPageToolStripMenuItem1.Text = "&Add Page"
        '
        'ToolStripMenuItemPastePage
        '
        Me.ToolStripMenuItemPastePage.Image = Global.Boston.My.Resources.MenuImages.Paste16x16
        Me.ToolStripMenuItemPastePage.Name = "ToolStripMenuItemPastePage"
        Me.ToolStripMenuItemPastePage.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemPastePage.Text = "&Paste Page"
        '
        'ToolStripMenuItemLanguage
        '
        Me.ToolStripMenuItemLanguage.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertyGraphSchemaToolStripMenuItem, Me.EntityRelationshipDiagramToolStripMenuItem, Me.StateTransitionDiagramToolStripMenuItem})
        Me.ToolStripMenuItemLanguage.Image = CType(resources.GetObject("ToolStripMenuItemLanguage.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemLanguage.Name = "ToolStripMenuItemLanguage"
        Me.ToolStripMenuItemLanguage.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemLanguage.Text = "&Language"
        '
        'PropertyGraphSchemaToolStripMenuItem
        '
        Me.PropertyGraphSchemaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddPGSPageToolStripMenuItem})
        Me.PropertyGraphSchemaToolStripMenuItem.Image = CType(resources.GetObject("PropertyGraphSchemaToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PropertyGraphSchemaToolStripMenuItem.Name = "PropertyGraphSchemaToolStripMenuItem"
        Me.PropertyGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.PropertyGraphSchemaToolStripMenuItem.Text = "Property Graph Schema"
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
        Me.EntityRelationshipDiagramToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
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
        Me.StateTransitionDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.StateTransitionDiagram_16x16
        Me.StateTransitionDiagramToolStripMenuItem.Name = "StateTransitionDiagramToolStripMenuItem"
        Me.StateTransitionDiagramToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.StateTransitionDiagramToolStripMenuItem.Text = "&State Transition Diagram"
        '
        'AddSTDPageToolStripMenuItem
        '
        Me.AddSTDPageToolStripMenuItem.Name = "AddSTDPageToolStripMenuItem"
        Me.AddSTDPageToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.AddSTDPageToolStripMenuItem.Text = "&Add STD Page"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(212, 6)
        '
        'EmptyModelToolStripMenuItem
        '
        Me.EmptyModelToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.empty16x16
        Me.EmptyModelToolStripMenuItem.Name = "EmptyModelToolStripMenuItem"
        Me.EmptyModelToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.EmptyModelToolStripMenuItem.Text = "E&mpty Model"
        Me.EmptyModelToolStripMenuItem.Visible = False
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.HideToolStripMenuItem.Text = "&Hide"
        '
        'HideAllotherModelsToolStripMenuItem
        '
        Me.HideAllotherModelsToolStripMenuItem.Name = "HideAllotherModelsToolStripMenuItem"
        Me.HideAllotherModelsToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.HideAllotherModelsToolStripMenuItem.Text = "Hide all &other models"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.RenameToolStripMenuItem.Text = "&Rename"
        '
        'DeleteModelToolStripMenuItem
        '
        Me.DeleteModelToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.DeleteModelToolStripMenuItem.Name = "DeleteModelToolStripMenuItem"
        Me.DeleteModelToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.DeleteModelToolStripMenuItem.Text = "&Delete Model"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(212, 6)
        '
        'ToolStripMenuItemModelConfiguration
        '
        Me.ToolStripMenuItemModelConfiguration.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemModelConfiguration.Name = "ToolStripMenuItemModelConfiguration"
        Me.ToolStripMenuItemModelConfiguration.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemModelConfiguration.Text = "Model Con&figuration"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(212, 6)
        '
        'ImportExportToolStripMenuItem
        '
        Me.ImportExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem12, Me.ExportTestingToolStripMenuItem, Me.TocqlFileToolStripMenuItem})
        Me.ImportExportToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.ImportExport16x16
        Me.ImportExportToolStripMenuItem.Name = "ImportExportToolStripMenuItem"
        Me.ImportExportToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ImportExportToolStripMenuItem.Text = "&Export"
        '
        'ToolStripMenuItem12
        '
        Me.ToolStripMenuItem12.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FromORMCMMLFileToolStripMenuItem})
        Me.ToolStripMenuItem12.Image = Global.Boston.My.Resources.MenuImages.Import16x16
        Me.ToolStripMenuItem12.Name = "ToolStripMenuItem12"
        Me.ToolStripMenuItem12.Size = New System.Drawing.Size(135, 22)
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
        Me.ExportTestingToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ExportTestingToolStripMenuItem.Text = "To .&fbm File"
        '
        'TocqlFileToolStripMenuItem
        '
        Me.TocqlFileToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.CQL16x16
        Me.TocqlFileToolStripMenuItem.Name = "TocqlFileToolStripMenuItem"
        Me.TocqlFileToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.TocqlFileToolStripMenuItem.Text = "To .&cql File"
        Me.TocqlFileToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItemFixModelErrors
        '
        Me.ToolStripMenuItemFixModelErrors.Name = "ToolStripMenuItemFixModelErrors"
        Me.ToolStripMenuItemFixModelErrors.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemFixModelErrors.Text = "Fi&x Model Errors"
        Me.ToolStripMenuItemFixModelErrors.Visible = False
        '
        'ContextMenuStrip_ORMModels
        '
        Me.ContextMenuStrip_ORMModels.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_ORMModels.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddModel, Me.ToolStripSeparator2, Me.ToolStripMenuItem1, Me.UnhideHiddenModelsToolStripMenuItem})
        Me.ContextMenuStrip_ORMModels.Name = "ContextMenuStrip_ORMModels"
        Me.ContextMenuStrip_ORMModels.Size = New System.Drawing.Size(205, 100)
        '
        'ToolStripMenuItemAddModel
        '
        Me.ToolStripMenuItemAddModel.Image = Global.Boston.My.Resources.MenuImages.DatabaseAdd16x16
        Me.ToolStripMenuItemAddModel.Name = "ToolStripMenuItemAddModel"
        Me.ToolStripMenuItemAddModel.Size = New System.Drawing.Size(204, 30)
        Me.ToolStripMenuItemAddModel.Text = "&Add Model"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(201, 6)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.XML16x16
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(204, 30)
        Me.ToolStripMenuItem1.Text = "&Import .fbm File"
        '
        'UnhideHiddenModelsToolStripMenuItem
        '
        Me.UnhideHiddenModelsToolStripMenuItem.Name = "UnhideHiddenModelsToolStripMenuItem"
        Me.UnhideHiddenModelsToolStripMenuItem.Size = New System.Drawing.Size(204, 30)
        Me.UnhideHiddenModelsToolStripMenuItem.Text = "&Unhide Hidden Models"
        '
        'HelpProvider
        '
        Me.HelpProvider.HelpNamespace = ".\richmondhelp\Richmond.chm"
        '
        'BackgroundWorkerModelLoader
        '
        Me.BackgroundWorkerModelLoader.WorkerReportsProgress = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.TreeView)
        Me.Panel1.Controls.Add(Me.CircularProgressBar)
        Me.Panel1.Location = New System.Drawing.Point(12, 62)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(380, 482)
        Me.Panel1.TabIndex = 11
        '
        'TreeView
        '
        Me.TreeView.AllowDrop = True
        Me.TreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer), CType(CType(5, Byte), Integer))
        Me.TreeView.HideSelection = False
        Me.TreeView.ImageIndex = 0
        Me.TreeView.ImageList = Me.ImageList
        Me.TreeView.LabelEdit = True
        Me.TreeView.Location = New System.Drawing.Point(0, 0)
        Me.TreeView.MinimumSize = New System.Drawing.Size(190, 4)
        Me.TreeView.Name = "TreeView"
        Me.TreeView.SelectedImageKey = "blank.ico"
        Me.TreeView.SelectedNode = Nothing
        Me.TreeView.Size = New System.Drawing.Size(380, 482)
        Me.TreeView.TabIndex = 0
        '
        'frmToolboxEnterpriseExplorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(407, 612)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.GroupBox_main)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmToolboxEnterpriseExplorer"
        Me.TabText = "Model Explorer"
        Me.Text = "Model Explorer"
        Me.GroupBox_main.ResumeLayout(False)
        Me.GroupBox_main.PerformLayout()
        Me.ContextMenuStrip_Page.ResumeLayout(False)
        Me.ContextMenuStrip_ORMModel.ResumeLayout(False)
        Me.ContextMenuStrip_ORMModels.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_main As System.Windows.Forms.GroupBox
    Friend WithEvents Timer_FormSetup As System.Windows.Forms.Timer
    Friend WithEvents ImageList As System.Windows.Forms.ImageList
    Friend WithEvents ContextMenuStrip_Page As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeletePageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ORMModel As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AddPageToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ViewModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DialogFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents DialogOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ContextMenuStrip_ORMModels As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemAddModel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider As System.Windows.Forms.HelpProvider
    Friend WithEvents ImportExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem12 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents LabelHelpTips As System.Windows.Forms.Label
    Friend WithEvents DeleteModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EmptyModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemPastePage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FromORMCMMLFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents CodeGenerationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TreeView As BostonTreeView
    Friend WithEvents FactEngineToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UnhideHiddenModelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ExportTestingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TocqlFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideAllotherModelsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFixModelErrors As ToolStripMenuItem
End Class
