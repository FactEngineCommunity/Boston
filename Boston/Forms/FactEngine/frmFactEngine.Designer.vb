<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFactEngine
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFactEngine))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelHelp = New System.Windows.Forms.Label()
        Me.ContextMenuStripHelpLabel = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripNaturalLanguage = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabelPromptNaturalLanguage = New System.Windows.Forms.ToolStripLabel()
        Me.TextBoxNaturalLanguage = New System.Windows.Forms.ToolStripTextBox()
        Me.TextBoxInput = New System.Windows.Forms.RichTextBox()
        Me.ContextMenuStripFactEngine = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ErrorListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMVerbalisationViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VirtualAnalystToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BackgroundColourToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemLightBackground = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDarkBackground = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemHelpTips = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultAfterQueryToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDefaultToResultsTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDefaultToQueryTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAutoCapitalise = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemNaturalLanguageAnswers = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemNaturalLanguage = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelPrompt = New System.Windows.Forms.Label()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonGO = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButtonOptions = New System.Windows.Forms.ToolStripDropDownButton()
        Me.SuspendGraphViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelWorkingModelName = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelLookingFor = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelCurrentProduction = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelRequiresConnectionString = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelGOPrompt = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelError = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelDefaultQueryLimit = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabPageResults = New System.Windows.Forms.TabPage()
        Me.StatusStrip3 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelResults = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelError = New System.Windows.Forms.TextBox()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripDropDownButtonData = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ExportAsCSVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPageQuery = New System.Windows.Forms.TabPage()
        Me.StatusStrip4 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelPromptLineNumber = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelLineNumber = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TextBoxQuery = New System.Windows.Forms.TextBox()
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonQueryGO = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripComboBoxQueryLanguage = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButtonQueryOptions = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ToolStripMenuItemUseThisQueryLanguage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemGenerateQueryOnly = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPageGraph = New System.Windows.Forms.TabPage()
        Me.GraphDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ContextMenuStripGraph = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.LayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.TabPageChart = New System.Windows.Forms.TabPage()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.Diagram1 = New MindFusion.Diagramming.Diagram()
        Me.Diagram2 = New MindFusion.Diagramming.Diagram()
        Me.Diagram3 = New MindFusion.Diagramming.Diagram()
        Me.Diagram4 = New MindFusion.Diagramming.Diagram()
        Me.Diagram5 = New MindFusion.Diagramming.Diagram()
        Me.Diagram6 = New MindFusion.Diagramming.Diagram()
        Me.Diagram7 = New MindFusion.Diagramming.Diagram()
        Me.Diagram8 = New MindFusion.Diagramming.Diagram()
        Me.Diagram9 = New MindFusion.Diagramming.Diagram()
        Me.Diagram10 = New MindFusion.Diagramming.Diagram()
        Me.Diagram11 = New MindFusion.Diagramming.Diagram()
        Me.Diagram12 = New MindFusion.Diagramming.Diagram()
        Me.TabPageActualQuery = New System.Windows.Forms.TabPage()
        Me.StatusStripActualQuery = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TextBoxActualQuery = New System.Windows.Forms.TextBox()
        Me.ToolStripActualQuery = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripComboBox1 = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ContextMenuStripHelpLabel.SuspendLayout()
        Me.ToolStripNaturalLanguage.SuspendLayout()
        Me.ContextMenuStripFactEngine.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip2.SuspendLayout()
        Me.TabControl.SuspendLayout()
        Me.TabPageResults.SuspendLayout()
        Me.StatusStrip3.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.TabPageQuery.SuspendLayout()
        Me.StatusStrip4.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.TabPageGraph.SuspendLayout()
        Me.ContextMenuStripGraph.SuspendLayout()
        Me.TabPageActualQuery.SuspendLayout()
        Me.StatusStripActualQuery.SuspendLayout()
        Me.ToolStripActualQuery.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(2)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ToolStrip1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.StatusStrip2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.StatusStrip1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TabControl)
        Me.SplitContainer1.Size = New System.Drawing.Size(1095, 542)
        Me.SplitContainer1.SplitterDistance = 288
        Me.SplitContainer1.SplitterWidth = 3
        Me.SplitContainer1.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelHelp, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStripNaturalLanguage, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxInput, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelPrompt, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 31)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1091, 209)
        Me.TableLayoutPanel1.TabIndex = 14
        '
        'LabelHelp
        '
        Me.LabelHelp.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelp.ContextMenuStrip = Me.ContextMenuStripHelpLabel
        Me.LabelHelp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelHelp.ForeColor = System.Drawing.Color.DarkGray
        Me.LabelHelp.Location = New System.Drawing.Point(3, 135)
        Me.LabelHelp.MaximumSize = New System.Drawing.Size(0, 74)
        Me.LabelHelp.MinimumSize = New System.Drawing.Size(0, 74)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(1085, 74)
        Me.LabelHelp.TabIndex = 12
        '
        'ContextMenuStripHelpLabel
        '
        Me.ContextMenuStripHelpLabel.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripHelpLabel.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideToolStripMenuItem})
        Me.ContextMenuStripHelpLabel.Name = "ContextMenuStripHelpLabel"
        Me.ContextMenuStripHelpLabel.Size = New System.Drawing.Size(100, 26)
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
        Me.HideToolStripMenuItem.Text = "&Hide"
        '
        'ToolStripNaturalLanguage
        '
        Me.ToolStripNaturalLanguage.AutoSize = False
        Me.ToolStripNaturalLanguage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripNaturalLanguage.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabelPromptNaturalLanguage, Me.TextBoxNaturalLanguage})
        Me.ToolStripNaturalLanguage.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripNaturalLanguage.Name = "ToolStripNaturalLanguage"
        Me.ToolStripNaturalLanguage.Size = New System.Drawing.Size(1091, 28)
        Me.ToolStripNaturalLanguage.TabIndex = 0
        Me.ToolStripNaturalLanguage.TabStop = True
        Me.ToolStripNaturalLanguage.Text = "ToolStrip4"
        '
        'ToolStripLabelPromptNaturalLanguage
        '
        Me.ToolStripLabelPromptNaturalLanguage.Name = "ToolStripLabelPromptNaturalLanguage"
        Me.ToolStripLabelPromptNaturalLanguage.Size = New System.Drawing.Size(104, 25)
        Me.ToolStripLabelPromptNaturalLanguage.Text = "Natural &Language:"
        '
        'TextBoxNaturalLanguage
        '
        Me.TextBoxNaturalLanguage.AcceptsTab = True
        Me.TextBoxNaturalLanguage.AutoSize = False
        Me.TextBoxNaturalLanguage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNaturalLanguage.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TextBoxNaturalLanguage.Name = "TextBoxNaturalLanguage"
        Me.TextBoxNaturalLanguage.Size = New System.Drawing.Size(850, 23)
        '
        'TextBoxInput
        '
        Me.TextBoxInput.AcceptsTab = True
        Me.TextBoxInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.TextBoxInput.ContextMenuStrip = Me.ContextMenuStripFactEngine
        Me.TextBoxInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxInput.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxInput.ForeColor = System.Drawing.Color.Wheat
        Me.TextBoxInput.Location = New System.Drawing.Point(2, 46)
        Me.TextBoxInput.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxInput.Name = "TextBoxInput"
        Me.TextBoxInput.Size = New System.Drawing.Size(1087, 87)
        Me.TextBoxInput.TabIndex = 1
        Me.TextBoxInput.Text = ""
        '
        'ContextMenuStripFactEngine
        '
        Me.ContextMenuStripFactEngine.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ToolStripSeparator1, Me.BackgroundColourToolStripMenuItem, Me.ToolStripMenuItemHelpTips, Me.DefaultAfterQueryToToolStripMenuItem, Me.ToolStripMenuItemAutoCapitalise, Me.ToolStripMenuItemNaturalLanguageAnswers, Me.ToolStripMenuItemNaturalLanguage})
        Me.ContextMenuStripFactEngine.Name = "ContextMenuStripFactEngine"
        Me.ContextMenuStripFactEngine.Size = New System.Drawing.Size(221, 172)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.AutoSize = False
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertiesToolStripMenuItem, Me.ModelDictionaryToolStripMenuItem, Me.ErrorListToolStripMenuItem, Me.ToolStripMenuItem8, Me.ORMVerbalisationViewToolStripMenuItem, Me.VirtualAnalystToolStripMenuItem})
        Me.ViewToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(240, 30)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'ErrorListToolStripMenuItem
        '
        Me.ErrorListToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.ErrorList
        Me.ErrorListToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ErrorListToolStripMenuItem.Name = "ErrorListToolStripMenuItem"
        Me.ErrorListToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ErrorListToolStripMenuItem.Text = "&Error List"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Image = Global.Boston.My.Resources.MenuImages.FactTypeReading16x16
        Me.ToolStripMenuItem8.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(203, 22)
        Me.ToolStripMenuItem8.Text = "Fact Type &Reading Editor"
        '
        'ORMVerbalisationViewToolStripMenuItem
        '
        Me.ORMVerbalisationViewToolStripMenuItem.Image = CType(resources.GetObject("ORMVerbalisationViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMVerbalisationViewToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ORMVerbalisationViewToolStripMenuItem.Name = "ORMVerbalisationViewToolStripMenuItem"
        Me.ORMVerbalisationViewToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ORMVerbalisationViewToolStripMenuItem.Text = "ORM &Verbalisation View"
        '
        'VirtualAnalystToolStripMenuItem
        '
        Me.VirtualAnalystToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.VirtualAnalyst16x16
        Me.VirtualAnalystToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.VirtualAnalystToolStripMenuItem.Name = "VirtualAnalystToolStripMenuItem"
        Me.VirtualAnalystToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.VirtualAnalystToolStripMenuItem.Text = "Virtual Analyst"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(217, 6)
        '
        'BackgroundColourToolStripMenuItem
        '
        Me.BackgroundColourToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemLightBackground, Me.ToolStripMenuItemDarkBackground})
        Me.BackgroundColourToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.BackgroundColourToolStripMenuItem.Name = "BackgroundColourToolStripMenuItem"
        Me.BackgroundColourToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.BackgroundColourToolStripMenuItem.Text = "&Background Colour"
        '
        'ToolStripMenuItemLightBackground
        '
        Me.ToolStripMenuItemLightBackground.Name = "ToolStripMenuItemLightBackground"
        Me.ToolStripMenuItemLightBackground.Size = New System.Drawing.Size(101, 22)
        Me.ToolStripMenuItemLightBackground.Text = "&Light"
        '
        'ToolStripMenuItemDarkBackground
        '
        Me.ToolStripMenuItemDarkBackground.Name = "ToolStripMenuItemDarkBackground"
        Me.ToolStripMenuItemDarkBackground.Size = New System.Drawing.Size(101, 22)
        Me.ToolStripMenuItemDarkBackground.Text = "&Dark"
        '
        'ToolStripMenuItemHelpTips
        '
        Me.ToolStripMenuItemHelpTips.Checked = True
        Me.ToolStripMenuItemHelpTips.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemHelpTips.Image = Global.Boston.My.Resources.Resources.HelpTips16x16
        Me.ToolStripMenuItemHelpTips.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ToolStripMenuItemHelpTips.Name = "ToolStripMenuItemHelpTips"
        Me.ToolStripMenuItemHelpTips.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemHelpTips.Text = "&Help Tips"
        '
        'DefaultAfterQueryToToolStripMenuItem
        '
        Me.DefaultAfterQueryToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemDefaultToResultsTab, Me.ToolStripMenuItemDefaultToQueryTab})
        Me.DefaultAfterQueryToToolStripMenuItem.Name = "DefaultAfterQueryToToolStripMenuItem"
        Me.DefaultAfterQueryToToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.DefaultAfterQueryToToolStripMenuItem.Text = "&Default tab after query..."
        '
        'ToolStripMenuItemDefaultToResultsTab
        '
        Me.ToolStripMenuItemDefaultToResultsTab.Checked = True
        Me.ToolStripMenuItemDefaultToResultsTab.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemDefaultToResultsTab.Name = "ToolStripMenuItemDefaultToResultsTab"
        Me.ToolStripMenuItemDefaultToResultsTab.Size = New System.Drawing.Size(131, 22)
        Me.ToolStripMenuItemDefaultToResultsTab.Text = "&Results tab"
        '
        'ToolStripMenuItemDefaultToQueryTab
        '
        Me.ToolStripMenuItemDefaultToQueryTab.Name = "ToolStripMenuItemDefaultToQueryTab"
        Me.ToolStripMenuItemDefaultToQueryTab.Size = New System.Drawing.Size(131, 22)
        Me.ToolStripMenuItemDefaultToQueryTab.Text = "&Query tab"
        '
        'ToolStripMenuItemAutoCapitalise
        '
        Me.ToolStripMenuItemAutoCapitalise.Checked = True
        Me.ToolStripMenuItemAutoCapitalise.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemAutoCapitalise.Name = "ToolStripMenuItemAutoCapitalise"
        Me.ToolStripMenuItemAutoCapitalise.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemAutoCapitalise.Text = "&Auto Capitalise"
        '
        'ToolStripMenuItemNaturalLanguageAnswers
        '
        Me.ToolStripMenuItemNaturalLanguageAnswers.CheckOnClick = True
        Me.ToolStripMenuItemNaturalLanguageAnswers.Name = "ToolStripMenuItemNaturalLanguageAnswers"
        Me.ToolStripMenuItemNaturalLanguageAnswers.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemNaturalLanguageAnswers.Text = "Natural Language Answers"
        '
        'ToolStripMenuItemNaturalLanguage
        '
        Me.ToolStripMenuItemNaturalLanguage.Checked = True
        Me.ToolStripMenuItemNaturalLanguage.CheckOnClick = True
        Me.ToolStripMenuItemNaturalLanguage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemNaturalLanguage.Name = "ToolStripMenuItemNaturalLanguage"
        Me.ToolStripMenuItemNaturalLanguage.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemNaturalLanguage.Text = "Natural Language Tool Strip"
        '
        'LabelPrompt
        '
        Me.LabelPrompt.AutoSize = True
        Me.LabelPrompt.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelPrompt.Location = New System.Drawing.Point(3, 31)
        Me.LabelPrompt.Name = "LabelPrompt"
        Me.LabelPrompt.Size = New System.Drawing.Size(1085, 13)
        Me.LabelPrompt.TabIndex = 13
        Me.LabelPrompt.Text = "FactEngine Query Language:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonGO, Me.ToolStripDropDownButtonOptions})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStrip1.Size = New System.Drawing.Size(1091, 31)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonGO
        '
        Me.ToolStripButtonGO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonGO.Image = Global.Boston.My.Resources.MenuImages.GO16x16
        Me.ToolStripButtonGO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonGO.Name = "ToolStripButtonGO"
        Me.ToolStripButtonGO.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButtonGO.Text = "ToolStripButton1"
        '
        'ToolStripDropDownButtonOptions
        '
        Me.ToolStripDropDownButtonOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButtonOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SuspendGraphViewToolStripMenuItem})
        Me.ToolStripDropDownButtonOptions.Image = CType(resources.GetObject("ToolStripDropDownButtonOptions.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButtonOptions.Name = "ToolStripDropDownButtonOptions"
        Me.ToolStripDropDownButtonOptions.Size = New System.Drawing.Size(62, 28)
        Me.ToolStripDropDownButtonOptions.Text = "&Options"
        '
        'SuspendGraphViewToolStripMenuItem
        '
        Me.SuspendGraphViewToolStripMenuItem.Checked = True
        Me.SuspendGraphViewToolStripMenuItem.CheckOnClick = True
        Me.SuspendGraphViewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.SuspendGraphViewToolStripMenuItem.Name = "SuspendGraphViewToolStripMenuItem"
        Me.SuspendGraphViewToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.SuspendGraphViewToolStripMenuItem.Text = "Suspend Graph View"
        '
        'StatusStrip2
        '
        Me.StatusStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelWorkingModelName, Me.ToolStripStatusLabelLookingFor, Me.ToolStripStatusLabelCurrentProduction, Me.ToolStripStatusLabelRequiresConnectionString, Me.ToolStripStatusLabelGOPrompt, Me.ToolStripStatusLabelError, Me.ToolStripStatusLabelDefaultQueryLimit})
        Me.StatusStrip2.Location = New System.Drawing.Point(0, 240)
        Me.StatusStrip2.Name = "StatusStrip2"
        Me.StatusStrip2.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.StatusStrip2.Size = New System.Drawing.Size(1091, 22)
        Me.StatusStrip2.TabIndex = 1
        Me.StatusStrip2.Text = "StatusStrip2"
        '
        'ToolStripStatusLabelWorkingModelName
        '
        Me.ToolStripStatusLabelWorkingModelName.Name = "ToolStripStatusLabelWorkingModelName"
        Me.ToolStripStatusLabelWorkingModelName.Size = New System.Drawing.Size(118, 17)
        Me.ToolStripStatusLabelWorkingModelName.Text = "WorkingModelName"
        '
        'ToolStripStatusLabelLookingFor
        '
        Me.ToolStripStatusLabelLookingFor.Name = "ToolStripStatusLabelLookingFor"
        Me.ToolStripStatusLabelLookingFor.Size = New System.Drawing.Size(62, 17)
        Me.ToolStripStatusLabelLookingFor.Text = "Expecting:"
        '
        'ToolStripStatusLabelCurrentProduction
        '
        Me.ToolStripStatusLabelCurrentProduction.Name = "ToolStripStatusLabelCurrentProduction"
        Me.ToolStripStatusLabelCurrentProduction.Size = New System.Drawing.Size(106, 17)
        Me.ToolStripStatusLabelCurrentProduction.Text = "CurrentProduction"
        '
        'ToolStripStatusLabelRequiresConnectionString
        '
        Me.ToolStripStatusLabelRequiresConnectionString.Name = "ToolStripStatusLabelRequiresConnectionString"
        Me.ToolStripStatusLabelRequiresConnectionString.Size = New System.Drawing.Size(145, 17)
        Me.ToolStripStatusLabelRequiresConnectionString.Text = "RequiresConnectionString"
        '
        'ToolStripStatusLabelGOPrompt
        '
        Me.ToolStripStatusLabelGOPrompt.Name = "ToolStripStatusLabelGOPrompt"
        Me.ToolStripStatusLabelGOPrompt.Size = New System.Drawing.Size(64, 17)
        Me.ToolStripStatusLabelGOPrompt.Text = "GOPrompt"
        Me.ToolStripStatusLabelGOPrompt.Visible = False
        '
        'ToolStripStatusLabelError
        '
        Me.ToolStripStatusLabelError.ForeColor = System.Drawing.Color.Coral
        Me.ToolStripStatusLabelError.Name = "ToolStripStatusLabelError"
        Me.ToolStripStatusLabelError.Size = New System.Drawing.Size(138, 17)
        Me.ToolStripStatusLabelError.Text = "ToolStripStatusLabelError"
        '
        'ToolStripStatusLabelDefaultQueryLimit
        '
        Me.ToolStripStatusLabelDefaultQueryLimit.Name = "ToolStripStatusLabelDefaultQueryLimit"
        Me.ToolStripStatusLabelDefaultQueryLimit.Size = New System.Drawing.Size(210, 17)
        Me.ToolStripStatusLabelDefaultQueryLimit.Text = "ToolStripStatusLabelDefaultQueryLimit"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 262)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 9, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(1091, 22)
        Me.StatusStrip1.TabIndex = 0
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabPageResults)
        Me.TabControl.Controls.Add(Me.TabPageQuery)
        Me.TabControl.Controls.Add(Me.TabPageGraph)
        Me.TabControl.Controls.Add(Me.TabPageChart)
        Me.TabControl.Controls.Add(Me.TabPageActualQuery)
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1091, 247)
        Me.TabControl.TabIndex = 2
        '
        'TabPageResults
        '
        Me.TabPageResults.Controls.Add(Me.StatusStrip3)
        Me.TabPageResults.Controls.Add(Me.LabelError)
        Me.TabPageResults.Controls.Add(Me.ToolStrip2)
        Me.TabPageResults.Location = New System.Drawing.Point(4, 22)
        Me.TabPageResults.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPageResults.Name = "TabPageResults"
        Me.TabPageResults.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPageResults.Size = New System.Drawing.Size(1083, 221)
        Me.TabPageResults.TabIndex = 0
        Me.TabPageResults.Text = "Results"
        Me.TabPageResults.UseVisualStyleBackColor = True
        '
        'StatusStrip3
        '
        Me.StatusStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelResults})
        Me.StatusStrip3.Location = New System.Drawing.Point(2, 197)
        Me.StatusStrip3.Name = "StatusStrip3"
        Me.StatusStrip3.Size = New System.Drawing.Size(1079, 22)
        Me.StatusStrip3.TabIndex = 3
        Me.StatusStrip3.Text = "StatusStrip3"
        '
        'ToolStripStatusLabelResults
        '
        Me.ToolStripStatusLabelResults.Name = "ToolStripStatusLabelResults"
        Me.ToolStripStatusLabelResults.Size = New System.Drawing.Size(150, 17)
        Me.ToolStripStatusLabelResults.Text = "ToolStripStatusLabelResults"
        '
        'LabelError
        '
        Me.LabelError.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelError.BackColor = System.Drawing.SystemColors.Control
        Me.LabelError.Location = New System.Drawing.Point(2, 27)
        Me.LabelError.Margin = New System.Windows.Forms.Padding(2)
        Me.LabelError.Multiline = True
        Me.LabelError.Name = "LabelError"
        Me.LabelError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.LabelError.Size = New System.Drawing.Size(1049, 204)
        Me.LabelError.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripDropDownButtonData})
        Me.ToolStrip2.Location = New System.Drawing.Point(2, 2)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStrip2.Size = New System.Drawing.Size(1079, 25)
        Me.ToolStrip2.TabIndex = 2
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripDropDownButtonData
        '
        Me.ToolStripDropDownButtonData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButtonData.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportAsCSVToolStripMenuItem})
        Me.ToolStripDropDownButtonData.Image = CType(resources.GetObject("ToolStripDropDownButtonData.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButtonData.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButtonData.Name = "ToolStripDropDownButtonData"
        Me.ToolStripDropDownButtonData.Size = New System.Drawing.Size(44, 22)
        Me.ToolStripDropDownButtonData.Text = "&Data"
        '
        'ExportAsCSVToolStripMenuItem
        '
        Me.ExportAsCSVToolStripMenuItem.Name = "ExportAsCSVToolStripMenuItem"
        Me.ExportAsCSVToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.ExportAsCSVToolStripMenuItem.Text = "Export as &CSV"
        '
        'TabPageQuery
        '
        Me.TabPageQuery.Controls.Add(Me.StatusStrip4)
        Me.TabPageQuery.Controls.Add(Me.TextBoxQuery)
        Me.TabPageQuery.Controls.Add(Me.ToolStrip3)
        Me.TabPageQuery.Location = New System.Drawing.Point(4, 22)
        Me.TabPageQuery.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPageQuery.Name = "TabPageQuery"
        Me.TabPageQuery.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPageQuery.Size = New System.Drawing.Size(1083, 221)
        Me.TabPageQuery.TabIndex = 1
        Me.TabPageQuery.Text = "Query"
        Me.TabPageQuery.UseVisualStyleBackColor = True
        '
        'StatusStrip4
        '
        Me.StatusStrip4.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelPromptLineNumber, Me.ToolStripStatusLabelLineNumber})
        Me.StatusStrip4.Location = New System.Drawing.Point(2, 197)
        Me.StatusStrip4.Name = "StatusStrip4"
        Me.StatusStrip4.Size = New System.Drawing.Size(1079, 22)
        Me.StatusStrip4.TabIndex = 4
        Me.StatusStrip4.Text = "StatusStrip4"
        '
        'ToolStripStatusLabelPromptLineNumber
        '
        Me.ToolStripStatusLabelPromptLineNumber.Name = "ToolStripStatusLabelPromptLineNumber"
        Me.ToolStripStatusLabelPromptLineNumber.Size = New System.Drawing.Size(79, 17)
        Me.ToolStripStatusLabelPromptLineNumber.Text = "Line Number:"
        '
        'ToolStripStatusLabelLineNumber
        '
        Me.ToolStripStatusLabelLineNumber.Name = "ToolStripStatusLabelLineNumber"
        Me.ToolStripStatusLabelLineNumber.Size = New System.Drawing.Size(179, 17)
        Me.ToolStripStatusLabelLineNumber.Text = "ToolStripStatusLabelLineNumber"
        '
        'TextBoxQuery
        '
        Me.TextBoxQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxQuery.Location = New System.Drawing.Point(1, 34)
        Me.TextBoxQuery.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxQuery.Multiline = True
        Me.TextBoxQuery.Name = "TextBoxQuery"
        Me.TextBoxQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxQuery.Size = New System.Drawing.Size(1079, 162)
        Me.TextBoxQuery.TabIndex = 2
        '
        'ToolStrip3
        '
        Me.ToolStrip3.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonQueryGO, Me.ToolStripComboBoxQueryLanguage, Me.ToolStripButton1, Me.ToolStripDropDownButtonQueryOptions})
        Me.ToolStrip3.Location = New System.Drawing.Point(2, 2)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStrip3.Size = New System.Drawing.Size(1079, 31)
        Me.ToolStrip3.TabIndex = 3
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'ToolStripButtonQueryGO
        '
        Me.ToolStripButtonQueryGO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButtonQueryGO.Image = Global.Boston.My.Resources.MenuImages.GO16x16
        Me.ToolStripButtonQueryGO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonQueryGO.Name = "ToolStripButtonQueryGO"
        Me.ToolStripButtonQueryGO.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButtonQueryGO.Text = "ToolStripButton1"
        '
        'ToolStripComboBoxQueryLanguage
        '
        Me.ToolStripComboBoxQueryLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBoxQueryLanguage.Items.AddRange(New Object() {"SQL", "TypeQL", "Cypher", "openCypher"})
        Me.ToolStripComboBoxQueryLanguage.MergeIndex = 0
        Me.ToolStripComboBoxQueryLanguage.Name = "ToolStripComboBoxQueryLanguage"
        Me.ToolStripComboBoxQueryLanguage.Size = New System.Drawing.Size(121, 31)
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButton1.Text = "ToolStripButton1"
        '
        'ToolStripDropDownButtonQueryOptions
        '
        Me.ToolStripDropDownButtonQueryOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButtonQueryOptions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemUseThisQueryLanguage, Me.ToolStripMenuItemGenerateQueryOnly})
        Me.ToolStripDropDownButtonQueryOptions.Image = CType(resources.GetObject("ToolStripDropDownButtonQueryOptions.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButtonQueryOptions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButtonQueryOptions.Name = "ToolStripDropDownButtonQueryOptions"
        Me.ToolStripDropDownButtonQueryOptions.Size = New System.Drawing.Size(62, 28)
        Me.ToolStripDropDownButtonQueryOptions.Text = "&Options"
        '
        'ToolStripMenuItemUseThisQueryLanguage
        '
        Me.ToolStripMenuItemUseThisQueryLanguage.Checked = True
        Me.ToolStripMenuItemUseThisQueryLanguage.CheckOnClick = True
        Me.ToolStripMenuItemUseThisQueryLanguage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemUseThisQueryLanguage.Name = "ToolStripMenuItemUseThisQueryLanguage"
        Me.ToolStripMenuItemUseThisQueryLanguage.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItemUseThisQueryLanguage.Text = "Use this query language"
        '
        'ToolStripMenuItemGenerateQueryOnly
        '
        Me.ToolStripMenuItemGenerateQueryOnly.CheckOnClick = True
        Me.ToolStripMenuItemGenerateQueryOnly.Name = "ToolStripMenuItemGenerateQueryOnly"
        Me.ToolStripMenuItemGenerateQueryOnly.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItemGenerateQueryOnly.Text = "Generate query only"
        '
        'TabPageGraph
        '
        Me.TabPageGraph.Controls.Add(Me.GraphDiagramView)
        Me.TabPageGraph.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGraph.Name = "TabPageGraph"
        Me.TabPageGraph.Size = New System.Drawing.Size(1083, 221)
        Me.TabPageGraph.TabIndex = 2
        Me.TabPageGraph.Text = "Graph"
        Me.TabPageGraph.UseVisualStyleBackColor = True
        '
        'GraphDiagramView
        '
        Me.GraphDiagramView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.GraphDiagramView.ContextMenuStrip = Me.ContextMenuStripGraph
        Me.GraphDiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.GraphDiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.GraphDiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.GraphDiagramView.Diagram = Me.Diagram
        Me.GraphDiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GraphDiagramView.Location = New System.Drawing.Point(0, 0)
        Me.GraphDiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.GraphDiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.GraphDiagramView.Name = "GraphDiagramView"
        Me.GraphDiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.GraphDiagramView.Size = New System.Drawing.Size(1083, 221)
        Me.GraphDiagramView.TabIndex = 0
        Me.GraphDiagramView.Text = "DiagramView1"
        '
        'ContextMenuStripGraph
        '
        Me.ContextMenuStripGraph.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripGraph.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LayoutToolStripMenuItem, Me.ClearToolStripMenuItem})
        Me.ContextMenuStripGraph.Name = "ContextMenuStripGraph"
        Me.ContextMenuStripGraph.Size = New System.Drawing.Size(111, 48)
        '
        'LayoutToolStripMenuItem
        '
        Me.LayoutToolStripMenuItem.Name = "LayoutToolStripMenuItem"
        Me.LayoutToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.LayoutToolStripMenuItem.Text = "&Layout"
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(110, 22)
        Me.ClearToolStripMenuItem.Text = "&Clear"
        '
        'Diagram
        '
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Diagram.LinkStyle = MindFusion.Diagramming.LinkStyle.Bezier
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'TabPageChart
        '
        Me.TabPageChart.Location = New System.Drawing.Point(4, 22)
        Me.TabPageChart.Name = "TabPageChart"
        Me.TabPageChart.Size = New System.Drawing.Size(1083, 221)
        Me.TabPageChart.TabIndex = 3
        Me.TabPageChart.Text = "Chart"
        Me.TabPageChart.UseVisualStyleBackColor = True
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
        '
        'TabPageActualQuery
        '
        Me.TabPageActualQuery.Controls.Add(Me.StatusStripActualQuery)
        Me.TabPageActualQuery.Controls.Add(Me.TextBoxActualQuery)
        Me.TabPageActualQuery.Controls.Add(Me.ToolStripActualQuery)
        Me.TabPageActualQuery.Location = New System.Drawing.Point(4, 22)
        Me.TabPageActualQuery.Name = "TabPageActualQuery"
        Me.TabPageActualQuery.Size = New System.Drawing.Size(1083, 221)
        Me.TabPageActualQuery.TabIndex = 4
        Me.TabPageActualQuery.Text = "Actual Query"
        Me.TabPageActualQuery.UseVisualStyleBackColor = True
        '
        'StatusStripActualQuery
        '
        Me.StatusStripActualQuery.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripStatusLabel2})
        Me.StatusStripActualQuery.Location = New System.Drawing.Point(0, 199)
        Me.StatusStripActualQuery.Name = "StatusStripActualQuery"
        Me.StatusStripActualQuery.Size = New System.Drawing.Size(1083, 22)
        Me.StatusStripActualQuery.TabIndex = 7
        Me.StatusStripActualQuery.Text = "StatusStrip5"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(79, 17)
        Me.ToolStripStatusLabel1.Text = "Line Number:"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(179, 17)
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabelLineNumber"
        '
        'TextBoxActualQuery
        '
        Me.TextBoxActualQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxActualQuery.Location = New System.Drawing.Point(1, 33)
        Me.TextBoxActualQuery.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBoxActualQuery.Multiline = True
        Me.TextBoxActualQuery.Name = "TextBoxActualQuery"
        Me.TextBoxActualQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxActualQuery.Size = New System.Drawing.Size(1079, 162)
        Me.TextBoxActualQuery.TabIndex = 5
        '
        'ToolStripActualQuery
        '
        Me.ToolStripActualQuery.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStripActualQuery.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton2, Me.ToolStripComboBox1, Me.ToolStripButton3, Me.ToolStripDropDownButton1})
        Me.ToolStripActualQuery.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripActualQuery.Name = "ToolStripActualQuery"
        Me.ToolStripActualQuery.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.ToolStripActualQuery.Size = New System.Drawing.Size(1083, 31)
        Me.ToolStripActualQuery.TabIndex = 6
        Me.ToolStripActualQuery.Text = "ToolStrip4"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.Boston.My.Resources.MenuImages.GO16x16
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButton2.Text = "ToolStripButton1"
        '
        'ToolStripComboBox1
        '
        Me.ToolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox1.Items.AddRange(New Object() {"SQL", "TypeQL", "Cypher", "openCypher"})
        Me.ToolStripComboBox1.MergeIndex = 0
        Me.ToolStripComboBox1.Name = "ToolStripComboBox1"
        Me.ToolStripComboBox1.Size = New System.Drawing.Size(121, 31)
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(28, 28)
        Me.ToolStripButton3.Text = "ToolStripButton1"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem2})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(62, 28)
        Me.ToolStripDropDownButton1.Text = "&Options"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Checked = True
        Me.ToolStripMenuItem1.CheckOnClick = True
        Me.ToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItem1.Text = "Use this query language"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.CheckOnClick = True
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(200, 22)
        Me.ToolStripMenuItem2.Text = "Generate query only"
        '
        'frmFactEngine
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1095, 542)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmFactEngine"
        Me.Text = "Fact Engine"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ContextMenuStripHelpLabel.ResumeLayout(False)
        Me.ToolStripNaturalLanguage.ResumeLayout(False)
        Me.ToolStripNaturalLanguage.PerformLayout()
        Me.ContextMenuStripFactEngine.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip2.ResumeLayout(False)
        Me.StatusStrip2.PerformLayout()
        Me.TabControl.ResumeLayout(False)
        Me.TabPageResults.ResumeLayout(False)
        Me.TabPageResults.PerformLayout()
        Me.StatusStrip3.ResumeLayout(False)
        Me.StatusStrip3.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.TabPageQuery.ResumeLayout(False)
        Me.TabPageQuery.PerformLayout()
        Me.StatusStrip4.ResumeLayout(False)
        Me.StatusStrip4.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.TabPageGraph.ResumeLayout(False)
        Me.ContextMenuStripGraph.ResumeLayout(False)
        Me.TabPageActualQuery.ResumeLayout(False)
        Me.TabPageActualQuery.PerformLayout()
        Me.StatusStripActualQuery.ResumeLayout(False)
        Me.StatusStripActualQuery.PerformLayout()
        Me.ToolStripActualQuery.ResumeLayout(False)
        Me.ToolStripActualQuery.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TextBoxNaturalLanguage As ToolStripTextBox
    Friend WithEvents ToolStripMenuItemNaturalLanguage As ToolStripMenuItem
    Friend WithEvents TextBoxInput As RichTextBox
    Friend WithEvents StatusStrip2 As StatusStrip
    Friend WithEvents ToolStripStatusLabelWorkingModelName As ToolStripStatusLabel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButtonGO As ToolStripButton
    Friend WithEvents ToolStripStatusLabelCurrentProduction As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelLookingFor As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelRequiresConnectionString As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelGOPrompt As ToolStripStatusLabel
    Friend WithEvents LabelError As TextBox
    Friend WithEvents TabControl As TabControl
    Friend WithEvents TabPageQuery As TabPage
    Friend WithEvents TextBoxQuery As TextBox
    Friend WithEvents TabPageResults As TabPage
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents ToolStrip3 As ToolStrip
    Friend WithEvents ToolStripButtonQueryGO As ToolStripButton
    Friend WithEvents LabelHelp As Label
    Friend WithEvents ContextMenuStripHelpLabel As ContextMenuStrip
    Friend WithEvents HideToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripFactEngine As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemHelpTips As ToolStripMenuItem
    Friend WithEvents TabPageGraph As TabPage
    Friend WithEvents GraphDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStripGraph As ContextMenuStrip
    Friend WithEvents LayoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VirtualAnalystToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ErrorListToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As ToolStripMenuItem
    Friend WithEvents ORMVerbalisationViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackgroundColourToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemLightBackground As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDarkBackground As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ClearToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolStripStatusLabelError As ToolStripStatusLabel
    Friend WithEvents DefaultAfterQueryToToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDefaultToResultsTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDefaultToQueryTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAutoCapitalise As ToolStripMenuItem
    Friend WithEvents ToolStripNaturalLanguage As ToolStrip
    Friend WithEvents ToolStripLabelPromptNaturalLanguage As ToolStripLabel
    Friend WithEvents Diagram1 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram2 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram3 As MindFusion.Diagramming.Diagram
    Friend WithEvents ToolStripComboBoxQueryLanguage As ToolStripComboBox
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents Diagram4 As MindFusion.Diagramming.Diagram
    Friend WithEvents ToolStripMenuItemNaturalLanguageAnswers As ToolStripMenuItem
    Friend WithEvents Diagram5 As MindFusion.Diagramming.Diagram
    Friend WithEvents LabelPrompt As Label
    Friend WithEvents ToolStripDropDownButtonOptions As ToolStripDropDownButton
    Friend WithEvents SuspendGraphViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip3 As StatusStrip
    Friend WithEvents ToolStripStatusLabelResults As ToolStripStatusLabel
    Friend WithEvents Diagram6 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram7 As MindFusion.Diagramming.Diagram
    Friend WithEvents ToolStripDropDownButtonData As ToolStripDropDownButton
    Friend WithEvents ExportAsCSVToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Diagram8 As MindFusion.Diagramming.Diagram
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Diagram9 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram10 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram11 As MindFusion.Diagramming.Diagram
    Friend WithEvents TabPageChart As TabPage
    Friend WithEvents ToolStripDropDownButtonQueryOptions As ToolStripDropDownButton
    Friend WithEvents ToolStripMenuItemUseThisQueryLanguage As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemGenerateQueryOnly As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelDefaultQueryLimit As ToolStripStatusLabel
    Friend WithEvents Diagram12 As MindFusion.Diagramming.Diagram
    Friend WithEvents StatusStrip4 As StatusStrip
    Friend WithEvents ToolStripStatusLabelPromptLineNumber As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelLineNumber As ToolStripStatusLabel
    Friend WithEvents TabPageActualQuery As TabPage
    Friend WithEvents StatusStripActualQuery As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents TextBoxActualQuery As TextBox
    Friend WithEvents ToolStripActualQuery As ToolStrip
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents ToolStripComboBox1 As ToolStripComboBox
    Friend WithEvents ToolStripButton3 As ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
End Class
