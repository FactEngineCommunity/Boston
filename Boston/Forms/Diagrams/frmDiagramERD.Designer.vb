<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDiagramERD
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiagramERD))
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_Entity = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEntityRelationshipDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemORMDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionUseCaseDiagramEntity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddRelatedEntitiesToThisPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDisplayDataIndexRelationInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEntityIndexEditor = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEntityModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.MakeAllColumnNamesDBNameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MakeAllDBNamesColumnNamesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInDiagramSpyToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewInGlossaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewInModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemAddAttribute = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemIndexManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemReCreateDatabaseTable = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.ConvertToFactTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ViewTableDataToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparatorReCreateTable = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.DatabaseSchemaViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemDiagramIndexEditor = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMVerbalisationViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PageAsORMMetamodelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.DatabaseMappingShadingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemConvert = New System.Windows.Forms.ToolStripMenuItem()
        Me.LanguageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertyGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConvertToPropertyGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.AutoLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LayeredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrthogonalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpringToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripAttribute = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemMoveUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemMoveDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInDiagramSpyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.EditAttributeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDeleteAttribute = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemAttributeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemIsMandatory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemIsPartOfPrimaryKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Relation = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemERDDiagram1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemDeleteRelation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRelationShowInDiagramSpy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRelationShowInModelDictionary = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllButThisPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.CircularProgressBar = New CircularProgressBar.CircularProgressBar()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelMain = New System.Windows.Forms.Panel()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripLabelPromptModel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabelModel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabelPromptTableIsInDatabase = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripLabelTableIsInDatabase = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSplitDatabaseButton = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolStripMenuItemChangesAlterDatabase = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Entity.SuspendLayout()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStripAttribute.SuspendLayout()
        Me.ContextMenuStrip_Relation.SuspendLayout()
        Me.ContextMenuStripTab.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.PanelMain.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Diagram
        '
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.LinkCustomDraw = MindFusion.Diagramming.CustomDraw.Full
        Me.Diagram.LinksSnapToBorders = True
        Me.Diagram.LinkTextStyle = MindFusion.Diagramming.LinkTextStyle.Follow
        '
        'ContextMenuStrip_Entity
        '
        Me.ContextMenuStrip_Entity.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator2, Me.AddRelatedEntitiesToThisPageToolStripMenuItem, Me.ToolStripMenuItemDisplayDataIndexRelationInformation, Me.ToolStripMenuItemEntityIndexEditor, Me.ToolStripMenuItemEntityModelErrors, Me.ToolStripSeparator14, Me.MakeAllColumnNamesDBNameToolStripMenuItem, Me.MakeAllDBNamesColumnNamesToolStripMenuItem, Me.ToolStripSeparator8, Me.ShowInDiagramSpyToolStripMenuItem1, Me.ViewInGlossaryToolStripMenuItem, Me.ViewInModelDictionaryToolStripMenuItem, Me.ToolStripSeparator11, Me.ToolStripMenuItemAddAttribute, Me.ToolStripMenuItemIndexManager, Me.ToolStripMenuItemReCreateDatabaseTable, Me.ToolStripSeparator9, Me.ConvertToFactTypeToolStripMenuItem, Me.ToolStripSeparator10, Me.RemoveFromPageToolStripMenuItem, Me.ToolStripSeparator7, Me.ViewTableDataToolStripMenuItem, Me.ToolStripSeparatorReCreateTable, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_Entity.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_Entity.Size = New System.Drawing.Size(315, 448)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemEntityRelationshipDiagram, Me.ToolStripMenuItemORMDiagram, Me.PGSDiagramToolStripMenuItem, Me.MenuOptionUseCaseDiagramEntity})
        Me.MorphToToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To"
        '
        'ToolStripMenuItemEntityRelationshipDiagram
        '
        Me.ToolStripMenuItemEntityRelationshipDiagram.Image = Global.Boston.My.Resources.Resources.ERD16x16
        Me.ToolStripMenuItemEntityRelationshipDiagram.Name = "ToolStripMenuItemEntityRelationshipDiagram"
        Me.ToolStripMenuItemEntityRelationshipDiagram.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemEntityRelationshipDiagram.Text = "&Entity Relationship Diagram"
        '
        'ToolStripMenuItemORMDiagram
        '
        Me.ToolStripMenuItemORMDiagram.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.ToolStripMenuItemORMDiagram.Name = "ToolStripMenuItemORMDiagram"
        Me.ToolStripMenuItemORMDiagram.Size = New System.Drawing.Size(220, 22)
        Me.ToolStripMenuItemORMDiagram.Text = "&ORM Diagram"
        '
        'PGSDiagramToolStripMenuItem
        '
        Me.PGSDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.PGS16x16
        Me.PGSDiagramToolStripMenuItem.Name = "PGSDiagramToolStripMenuItem"
        Me.PGSDiagramToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.PGSDiagramToolStripMenuItem.Text = "&PGS Diagram"
        '
        'MenuOptionUseCaseDiagramEntity
        '
        Me.MenuOptionUseCaseDiagramEntity.Image = CType(resources.GetObject("MenuOptionUseCaseDiagramEntity.Image"), System.Drawing.Image)
        Me.MenuOptionUseCaseDiagramEntity.Name = "MenuOptionUseCaseDiagramEntity"
        Me.MenuOptionUseCaseDiagramEntity.Size = New System.Drawing.Size(220, 22)
        Me.MenuOptionUseCaseDiagramEntity.Text = "&Use Case Diagram"
        Me.MenuOptionUseCaseDiagramEntity.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(311, 6)
        '
        'AddRelatedEntitiesToThisPageToolStripMenuItem
        '
        Me.AddRelatedEntitiesToThisPageToolStripMenuItem.Name = "AddRelatedEntitiesToThisPageToolStripMenuItem"
        Me.AddRelatedEntitiesToThisPageToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.AddRelatedEntitiesToThisPageToolStripMenuItem.Text = "&Add related Entities to this Page"
        '
        'ToolStripMenuItemDisplayDataIndexRelationInformation
        '
        Me.ToolStripMenuItemDisplayDataIndexRelationInformation.Image = CType(resources.GetObject("ToolStripMenuItemDisplayDataIndexRelationInformation.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemDisplayDataIndexRelationInformation.Name = "ToolStripMenuItemDisplayDataIndexRelationInformation"
        Me.ToolStripMenuItemDisplayDataIndexRelationInformation.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemDisplayDataIndexRelationInformation.Text = "&Display Index/Relation/Data Type Information"
        '
        'ToolStripMenuItemEntityIndexEditor
        '
        Me.ToolStripMenuItemEntityIndexEditor.Image = CType(resources.GetObject("ToolStripMenuItemEntityIndexEditor.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemEntityIndexEditor.Name = "ToolStripMenuItemEntityIndexEditor"
        Me.ToolStripMenuItemEntityIndexEditor.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemEntityIndexEditor.Text = "&Index Editor (Superuser)"
        Me.ToolStripMenuItemEntityIndexEditor.Visible = False
        '
        'ToolStripMenuItemEntityModelErrors
        '
        Me.ToolStripMenuItemEntityModelErrors.Name = "ToolStripMenuItemEntityModelErrors"
        Me.ToolStripMenuItemEntityModelErrors.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemEntityModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(311, 6)
        '
        'MakeAllColumnNamesDBNameToolStripMenuItem
        '
        Me.MakeAllColumnNamesDBNameToolStripMenuItem.Name = "MakeAllColumnNamesDBNameToolStripMenuItem"
        Me.MakeAllColumnNamesDBNameToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.MakeAllColumnNamesDBNameToolStripMenuItem.Text = "Make all Column Names = DBName"
        '
        'MakeAllDBNamesColumnNamesToolStripMenuItem
        '
        Me.MakeAllDBNamesColumnNamesToolStripMenuItem.Name = "MakeAllDBNamesColumnNamesToolStripMenuItem"
        Me.MakeAllDBNamesColumnNamesToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.MakeAllDBNamesColumnNamesToolStripMenuItem.Text = "Make all DBNames = Column Names"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(311, 6)
        '
        'ShowInDiagramSpyToolStripMenuItem1
        '
        Me.ShowInDiagramSpyToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.spyglass_icon
        Me.ShowInDiagramSpyToolStripMenuItem1.Name = "ShowInDiagramSpyToolStripMenuItem1"
        Me.ShowInDiagramSpyToolStripMenuItem1.Size = New System.Drawing.Size(314, 22)
        Me.ShowInDiagramSpyToolStripMenuItem1.Text = "Show in Diagram Spy"
        '
        'ViewInGlossaryToolStripMenuItem
        '
        Me.ViewInGlossaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Glossary16x16
        Me.ViewInGlossaryToolStripMenuItem.Name = "ViewInGlossaryToolStripMenuItem"
        Me.ViewInGlossaryToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ViewInGlossaryToolStripMenuItem.Text = "Show in Glossary"
        '
        'ViewInModelDictionaryToolStripMenuItem
        '
        Me.ViewInModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ViewInModelDictionaryToolStripMenuItem.Name = "ViewInModelDictionaryToolStripMenuItem"
        Me.ViewInModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ViewInModelDictionaryToolStripMenuItem.Text = "Show in Model Dictionary"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(311, 6)
        '
        'ToolStripMenuItemAddAttribute
        '
        Me.ToolStripMenuItemAddAttribute.Name = "ToolStripMenuItemAddAttribute"
        Me.ToolStripMenuItemAddAttribute.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemAddAttribute.Text = "Add &Attribute"
        '
        'ToolStripMenuItemIndexManager
        '
        Me.ToolStripMenuItemIndexManager.Image = Global.Boston.My.Resources.Resources.Index
        Me.ToolStripMenuItemIndexManager.Name = "ToolStripMenuItemIndexManager"
        Me.ToolStripMenuItemIndexManager.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemIndexManager.Text = "&Index Manager"
        '
        'ToolStripMenuItemReCreateDatabaseTable
        '
        Me.ToolStripMenuItemReCreateDatabaseTable.Image = Global.Boston.My.Resources.Resources.Recycle16x16
        Me.ToolStripMenuItemReCreateDatabaseTable.Name = "ToolStripMenuItemReCreateDatabaseTable"
        Me.ToolStripMenuItemReCreateDatabaseTable.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemReCreateDatabaseTable.Text = "Re/Create Database Table"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(311, 6)
        '
        'ConvertToFactTypeToolStripMenuItem
        '
        Me.ConvertToFactTypeToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Convert16x16
        Me.ConvertToFactTypeToolStripMenuItem.Name = "ConvertToFactTypeToolStripMenuItem"
        Me.ConvertToFactTypeToolStripMenuItem.ShowShortcutKeys = False
        Me.ConvertToFactTypeToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ConvertToFactTypeToolStripMenuItem.Text = "&Convert to Fact Type"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(311, 6)
        '
        'RemoveFromPageToolStripMenuItem
        '
        Me.RemoveFromPageToolStripMenuItem.Name = "RemoveFromPageToolStripMenuItem"
        Me.RemoveFromPageToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.RemoveFromPageToolStripMenuItem.Text = "Remove from &Page"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(311, 6)
        '
        'ViewTableDataToolStripMenuItem
        '
        Me.ViewTableDataToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Table
        Me.ViewTableDataToolStripMenuItem.Name = "ViewTableDataToolStripMenuItem"
        Me.ViewTableDataToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.ViewTableDataToolStripMenuItem.Text = "&Table Data"
        '
        'ToolStripSeparatorReCreateTable
        '
        Me.ToolStripSeparatorReCreateTable.Name = "ToolStripSeparatorReCreateTable"
        Me.ToolStripSeparatorReCreateTable.Size = New System.Drawing.Size(311, 6)
        Me.ToolStripSeparatorReCreateTable.Visible = False
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = CType(resources.GetObject("PropertiesToolStripMenuItem1.Image"), System.Drawing.Image)
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(314, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'HiddenDiagram
        '
        Me.HiddenDiagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'MorphTimer
        '
        Me.MorphTimer.Interval = 1500
        '
        'MorphStepTimer
        '
        Me.MorphStepTimer.Interval = 25
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator5, Me.ToolStripMenuItemConvert, Me.ToolStripSeparator12, Me.AutoLayoutToolStripMenuItem, Me.CopyToolStripMenuItem})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 126)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolboxToolStripMenuItem, Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator3, Me.DatabaseSchemaViewToolStripMenuItem, Me.ToolStripSeparator16, Me.ToolStripMenuItemDiagramIndexEditor, Me.ToolStripMenuItem2, Me.ORMVerbalisationViewToolStripMenuItem, Me.PageAsORMMetamodelToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ToolboxToolStripMenuItem
        '
        Me.ToolboxToolStripMenuItem.Image = CType(resources.GetObject("ToolboxToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ToolboxToolStripMenuItem.Name = "ToolboxToolStripMenuItem"
        Me.ToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ToolboxToolStripMenuItem.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = CType(resources.GetObject("ModelDictionaryToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = CType(resources.GetObject("PropertiesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(205, 6)
        '
        'DatabaseSchemaViewToolStripMenuItem
        '
        Me.DatabaseSchemaViewToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.DatabaseSchema
        Me.DatabaseSchemaViewToolStripMenuItem.Name = "DatabaseSchemaViewToolStripMenuItem"
        Me.DatabaseSchemaViewToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.DatabaseSchemaViewToolStripMenuItem.Text = "Database Schema View"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(205, 6)
        '
        'ToolStripMenuItemDiagramIndexEditor
        '
        Me.ToolStripMenuItemDiagramIndexEditor.Image = CType(resources.GetObject("ToolStripMenuItemDiagramIndexEditor.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemDiagramIndexEditor.Name = "ToolStripMenuItemDiagramIndexEditor"
        Me.ToolStripMenuItemDiagramIndexEditor.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItemDiagramIndexEditor.Text = "&Index Editor"
        Me.ToolStripMenuItemDiagramIndexEditor.Visible = False
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Image = CType(resources.GetObject("ToolStripMenuItem2.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItem2.Text = "Fact Type &Reading Editor"
        '
        'ORMVerbalisationViewToolStripMenuItem
        '
        Me.ORMVerbalisationViewToolStripMenuItem.Image = CType(resources.GetObject("ORMVerbalisationViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMVerbalisationViewToolStripMenuItem.Name = "ORMVerbalisationViewToolStripMenuItem"
        Me.ORMVerbalisationViewToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ORMVerbalisationViewToolStripMenuItem.Text = "&Verbalisation View"
        '
        'PageAsORMMetamodelToolStripMenuItem
        '
        Me.PageAsORMMetamodelToolStripMenuItem.Image = CType(resources.GetObject("PageAsORMMetamodelToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PageAsORMMetamodelToolStripMenuItem.Name = "PageAsORMMetamodelToolStripMenuItem"
        Me.PageAsORMMetamodelToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PageAsORMMetamodelToolStripMenuItem.Text = "Page as &ORM Metamodel"
        Me.PageAsORMMetamodelToolStripMenuItem.Visible = False
        '
        'ShowHideToolStripMenuItem
        '
        Me.ShowHideToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_ViewGrid, Me.DatabaseMappingShadingToolStripMenuItem})
        Me.ShowHideToolStripMenuItem.Name = "ShowHideToolStripMenuItem"
        Me.ShowHideToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ShowHideToolStripMenuItem.Text = "&Show/Hide"
        '
        'mnuOption_ViewGrid
        '
        Me.mnuOption_ViewGrid.Image = CType(resources.GetObject("mnuOption_ViewGrid.Image"), System.Drawing.Image)
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(219, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'DatabaseMappingShadingToolStripMenuItem
        '
        Me.DatabaseMappingShadingToolStripMenuItem.CheckOnClick = True
        Me.DatabaseMappingShadingToolStripMenuItem.Name = "DatabaseMappingShadingToolStripMenuItem"
        Me.DatabaseMappingShadingToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.DatabaseMappingShadingToolStripMenuItem.Text = "&Database Mapping Shading"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(204, 6)
        '
        'ToolStripMenuItemConvert
        '
        Me.ToolStripMenuItemConvert.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LanguageToolStripMenuItem})
        Me.ToolStripMenuItemConvert.Image = CType(resources.GetObject("ToolStripMenuItemConvert.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemConvert.Name = "ToolStripMenuItemConvert"
        Me.ToolStripMenuItemConvert.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemConvert.Text = "Con&vert Page..."
        '
        'LanguageToolStripMenuItem
        '
        Me.LanguageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertyGraphSchemaToolStripMenuItem, Me.ConvertToPropertyGraphSchemaToolStripMenuItem})
        Me.LanguageToolStripMenuItem.Image = CType(resources.GetObject("LanguageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem"
        Me.LanguageToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.LanguageToolStripMenuItem.Text = "To &Language..."
        '
        'PropertyGraphSchemaToolStripMenuItem
        '
        Me.PropertyGraphSchemaToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.PropertyGraphSchemaToolStripMenuItem.Name = "PropertyGraphSchemaToolStripMenuItem"
        Me.PropertyGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.PropertyGraphSchemaToolStripMenuItem.Text = "Object-Role Model"
        '
        'ConvertToPropertyGraphSchemaToolStripMenuItem
        '
        Me.ConvertToPropertyGraphSchemaToolStripMenuItem.Image = CType(resources.GetObject("ConvertToPropertyGraphSchemaToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ConvertToPropertyGraphSchemaToolStripMenuItem.Name = "ConvertToPropertyGraphSchemaToolStripMenuItem"
        Me.ConvertToPropertyGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ConvertToPropertyGraphSchemaToolStripMenuItem.Text = "&Property Graph Schema"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(204, 6)
        '
        'AutoLayoutToolStripMenuItem
        '
        Me.AutoLayoutToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LayeredToolStripMenuItem, Me.OrthogonalToolStripMenuItem, Me.SpringToolStripMenuItem, Me.TreeToolStripMenuItem})
        Me.AutoLayoutToolStripMenuItem.Image = CType(resources.GetObject("AutoLayoutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AutoLayoutToolStripMenuItem.Name = "AutoLayoutToolStripMenuItem"
        Me.AutoLayoutToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.AutoLayoutToolStripMenuItem.Text = "&AutoLayout"
        '
        'LayeredToolStripMenuItem
        '
        Me.LayeredToolStripMenuItem.Name = "LayeredToolStripMenuItem"
        Me.LayeredToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.LayeredToolStripMenuItem.Text = "&Layered"
        '
        'OrthogonalToolStripMenuItem
        '
        Me.OrthogonalToolStripMenuItem.Name = "OrthogonalToolStripMenuItem"
        Me.OrthogonalToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.OrthogonalToolStripMenuItem.Text = "&Orthogonal"
        '
        'SpringToolStripMenuItem
        '
        Me.SpringToolStripMenuItem.Name = "SpringToolStripMenuItem"
        Me.SpringToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.SpringToolStripMenuItem.Text = "&Spring"
        '
        'TreeToolStripMenuItem
        '
        Me.TreeToolStripMenuItem.Name = "TreeToolStripMenuItem"
        Me.TreeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.TreeToolStripMenuItem.Text = "&Tree"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = CType(resources.GetObject("CopyToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ContextMenuStripAttribute
        '
        Me.ContextMenuStripAttribute.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemMoveUp, Me.ToolStripMenuItemMoveDown, Me.ToolStripSeparator4, Me.ShowInDiagramSpyToolStripMenuItem, Me.ToolStripSeparator6, Me.EditAttributeToolStripMenuItem, Me.ToolStripMenuItemDeleteAttribute, Me.ToolStripSeparator1, Me.ToolStripMenuItemAttributeModelErrors, Me.ToolStripSeparator17, Me.ToolStripMenuItemIsMandatory, Me.ToolStripMenuItemIsPartOfPrimaryKey, Me.ToolStripMenuItem1})
        Me.ContextMenuStripAttribute.Name = "ContextMenuStripAttribute"
        Me.ContextMenuStripAttribute.Size = New System.Drawing.Size(187, 226)
        '
        'ToolStripMenuItemMoveUp
        '
        Me.ToolStripMenuItemMoveUp.Image = Global.Boston.My.Resources.Resources.Up16x16
        Me.ToolStripMenuItemMoveUp.Name = "ToolStripMenuItemMoveUp"
        Me.ToolStripMenuItemMoveUp.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemMoveUp.Text = "Move &Up"
        '
        'ToolStripMenuItemMoveDown
        '
        Me.ToolStripMenuItemMoveDown.Image = Global.Boston.My.Resources.Resources.Down16x16
        Me.ToolStripMenuItemMoveDown.Name = "ToolStripMenuItemMoveDown"
        Me.ToolStripMenuItemMoveDown.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemMoveDown.Text = "Move &Down"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(183, 6)
        '
        'ShowInDiagramSpyToolStripMenuItem
        '
        Me.ShowInDiagramSpyToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Spyglass16x16
        Me.ShowInDiagramSpyToolStripMenuItem.Name = "ShowInDiagramSpyToolStripMenuItem"
        Me.ShowInDiagramSpyToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ShowInDiagramSpyToolStripMenuItem.Text = "Show in Diagram &Spy"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(183, 6)
        Me.ToolStripSeparator6.Visible = False
        '
        'EditAttributeToolStripMenuItem
        '
        Me.EditAttributeToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.edit16x16
        Me.EditAttributeToolStripMenuItem.Name = "EditAttributeToolStripMenuItem"
        Me.EditAttributeToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.EditAttributeToolStripMenuItem.Text = "&Edit Attribute"
        Me.EditAttributeToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItemDeleteAttribute
        '
        Me.ToolStripMenuItemDeleteAttribute.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.ToolStripMenuItemDeleteAttribute.Name = "ToolStripMenuItemDeleteAttribute"
        Me.ToolStripMenuItemDeleteAttribute.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemDeleteAttribute.Text = "D&elete Attribute"
        Me.ToolStripMenuItemDeleteAttribute.Visible = False
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(183, 6)
        '
        'ToolStripMenuItemAttributeModelErrors
        '
        Me.ToolStripMenuItemAttributeModelErrors.Name = "ToolStripMenuItemAttributeModelErrors"
        Me.ToolStripMenuItemAttributeModelErrors.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemAttributeModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(183, 6)
        '
        'ToolStripMenuItemIsMandatory
        '
        Me.ToolStripMenuItemIsMandatory.Image = Global.Boston.My.Resources.Resources.StarIcon16x16
        Me.ToolStripMenuItemIsMandatory.Name = "ToolStripMenuItemIsMandatory"
        Me.ToolStripMenuItemIsMandatory.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemIsMandatory.Text = "&Is Mandatory"
        Me.ToolStripMenuItemIsMandatory.Visible = False
        '
        'ToolStripMenuItemIsPartOfPrimaryKey
        '
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Name = "ToolStripMenuItemIsPartOfPrimaryKey"
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Text = "&Is Part of Primary Key"
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Visible = False
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = CType(resources.GetObject("ToolStripMenuItem1.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem1.Text = "&Properties"
        '
        'ContextMenuStrip_Relation
        '
        Me.ContextMenuStrip_Relation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem1, Me.ToolStripSeparator15, Me.ToolStripMenuItemDeleteRelation, Me.ToolStripMenuItem3, Me.ToolStripSeparator13, Me.ToolStripMenuItemRelationShowInDiagramSpy, Me.ToolStripMenuItemRelationShowInModelDictionary, Me.ToolStripSeparator18, Me.ToolStripMenuItem4})
        Me.ContextMenuStrip_Relation.Name = "ContextMenuStrip_Relation"
        Me.ContextMenuStrip_Relation.Size = New System.Drawing.Size(211, 154)
        '
        'MorphToToolStripMenuItem1
        '
        Me.MorphToToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PGSDiagramToolStripMenuItem1, Me.ORMDiagramToolStripMenuItem1, Me.ToolStripMenuItemERDDiagram1})
        Me.MorphToToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem1.Name = "MorphToToolStripMenuItem1"
        Me.MorphToToolStripMenuItem1.Size = New System.Drawing.Size(210, 22)
        Me.MorphToToolStripMenuItem1.Text = "&Morph to ..."
        '
        'PGSDiagramToolStripMenuItem1
        '
        Me.PGSDiagramToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.PGS16x16
        Me.PGSDiagramToolStripMenuItem1.Name = "PGSDiagramToolStripMenuItem1"
        Me.PGSDiagramToolStripMenuItem1.Size = New System.Drawing.Size(149, 22)
        Me.PGSDiagramToolStripMenuItem1.Text = "&PGS Diagram"
        '
        'ORMDiagramToolStripMenuItem1
        '
        Me.ORMDiagramToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.ORMDiagramToolStripMenuItem1.Name = "ORMDiagramToolStripMenuItem1"
        Me.ORMDiagramToolStripMenuItem1.Size = New System.Drawing.Size(149, 22)
        Me.ORMDiagramToolStripMenuItem1.Text = "&ORM Diagram"
        '
        'ToolStripMenuItemERDDiagram1
        '
        Me.ToolStripMenuItemERDDiagram1.Image = Global.Boston.My.Resources.Resources.ERD16x16
        Me.ToolStripMenuItemERDDiagram1.Name = "ToolStripMenuItemERDDiagram1"
        Me.ToolStripMenuItemERDDiagram1.Size = New System.Drawing.Size(149, 22)
        Me.ToolStripMenuItemERDDiagram1.Text = "&ERD Diagram"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItemDeleteRelation
        '
        Me.ToolStripMenuItemDeleteRelation.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemDeleteRelation.Name = "ToolStripMenuItemDeleteRelation"
        Me.ToolStripMenuItemDeleteRelation.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemDeleteRelation.Text = "&Delete Relation"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Image = Global.Boston.My.Resources.Resources.Table
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItem3.Text = "View &Table Data"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItemRelationShowInDiagramSpy
        '
        Me.ToolStripMenuItemRelationShowInDiagramSpy.Image = Global.Boston.My.Resources.Resources.spyglass_icon
        Me.ToolStripMenuItemRelationShowInDiagramSpy.Name = "ToolStripMenuItemRelationShowInDiagramSpy"
        Me.ToolStripMenuItemRelationShowInDiagramSpy.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemRelationShowInDiagramSpy.Text = "Show in Diagram Spy"
        '
        'ToolStripMenuItemRelationShowInModelDictionary
        '
        Me.ToolStripMenuItemRelationShowInModelDictionary.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ToolStripMenuItemRelationShowInModelDictionary.Name = "ToolStripMenuItemRelationShowInModelDictionary"
        Me.ToolStripMenuItemRelationShowInModelDictionary.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItemRelationShowInModelDictionary.Text = "Show in Model Dictionary"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(207, 6)
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Image = Global.Boston.My.Resources.Resources.Recycle16x16
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(210, 22)
        Me.ToolStripMenuItem4.Text = "Re/Create Database Table"
        '
        'ContextMenuStripTab
        '
        Me.ContextMenuStripTab.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripTab.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem, Me.CloseAllButThisPageToolStripMenuItem})
        Me.ContextMenuStripTab.Name = "ContextMenuStripTab"
        Me.ContextMenuStripTab.Size = New System.Drawing.Size(191, 48)
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.CloseToolStripMenuItem.Text = "&Close"
        '
        'CloseAllButThisPageToolStripMenuItem
        '
        Me.CloseAllButThisPageToolStripMenuItem.Name = "CloseAllButThisPageToolStripMenuItem"
        Me.CloseAllButThisPageToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.CloseAllButThisPageToolStripMenuItem.Text = "Close all but this &Page"
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
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
        Me.CircularProgressBar.Location = New System.Drawing.Point(418, 228)
        Me.CircularProgressBar.MarqueeAnimationSpeed = 1000
        Me.CircularProgressBar.Name = "CircularProgressBar"
        Me.CircularProgressBar.OuterColor = System.Drawing.Color.LightGray
        Me.CircularProgressBar.OuterMargin = -25
        Me.CircularProgressBar.OuterWidth = 26
        Me.CircularProgressBar.ProgressColor = System.Drawing.Color.SteelBlue
        Me.CircularProgressBar.ProgressWidth = 6
        Me.CircularProgressBar.SecondaryFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CircularProgressBar.Size = New System.Drawing.Size(50, 50)
        Me.CircularProgressBar.StartAngle = 270
        Me.CircularProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.CircularProgressBar.SubscriptMargin = New System.Windows.Forms.Padding(-5, -27, 0, 0)
        Me.CircularProgressBar.SubscriptText = ""
        Me.CircularProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(CType(CType(166, Byte), Integer), CType(CType(166, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.CircularProgressBar.SuperscriptMargin = New System.Windows.Forms.Padding(12, 25, 0, 0)
        Me.CircularProgressBar.SuperscriptText = ""
        Me.CircularProgressBar.TabIndex = 17
        Me.CircularProgressBar.Text = "0"
        Me.CircularProgressBar.TextMargin = New System.Windows.Forms.Padding(2, 2, 0, 0)
        Me.CircularProgressBar.Value = 68
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PanelMain, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(938, 524)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'PanelMain
        '
        Me.PanelMain.Controls.Add(Me.DiagramView)
        Me.PanelMain.Controls.Add(Me.HiddenDiagramView)
        Me.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelMain.Location = New System.Drawing.Point(3, 3)
        Me.PanelMain.Name = "PanelMain"
        Me.PanelMain.Size = New System.Drawing.Size(932, 488)
        Me.PanelMain.TabIndex = 19
        '
        'DiagramView
        '
        Me.DiagramView.AllowDrop = True
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.DrawLinks
        Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Diagram
        Me.DiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.DiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.DiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.DiagramView.Diagram = Me.Diagram
        Me.DiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiagramView.Location = New System.Drawing.Point(0, 0)
        Me.DiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.DiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.DiagramView.Name = "DiagramView"
        Me.DiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.DiagramView.Size = New System.Drawing.Size(932, 488)
        Me.DiagramView.TabIndex = 0
        Me.DiagramView.Text = "DiagramView1"
        '
        'HiddenDiagramView
        '
        Me.HiddenDiagramView.BackColor = System.Drawing.Color.White
        Me.HiddenDiagramView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.HiddenDiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.HiddenDiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.HiddenDiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.HiddenDiagramView.Diagram = Me.HiddenDiagram
        Me.HiddenDiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HiddenDiagramView.Location = New System.Drawing.Point(0, 0)
        Me.HiddenDiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.None
        Me.HiddenDiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.HiddenDiagramView.Name = "HiddenDiagramView"
        Me.HiddenDiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.HiddenDiagramView.Size = New System.Drawing.Size(932, 488)
        Me.HiddenDiagramView.TabIndex = 11
        Me.HiddenDiagramView.Text = "DiagramView1"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabelPromptModel, Me.ToolStripLabelModel, Me.ToolStripLabelPromptTableIsInDatabase, Me.ToolStripLabelTableIsInDatabase, Me.ToolStripSplitDatabaseButton})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 494)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(938, 30)
        Me.ToolStrip1.TabIndex = 20
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripLabelPromptModel
        '
        Me.ToolStripLabelPromptModel.ForeColor = System.Drawing.Color.LightSteelBlue
        Me.ToolStripLabelPromptModel.Name = "ToolStripLabelPromptModel"
        Me.ToolStripLabelPromptModel.Size = New System.Drawing.Size(44, 27)
        Me.ToolStripLabelPromptModel.Text = "Model:"
        '
        'ToolStripLabelModel
        '
        Me.ToolStripLabelModel.ForeColor = System.Drawing.Color.Gray
        Me.ToolStripLabelModel.Name = "ToolStripLabelModel"
        Me.ToolStripLabelModel.Size = New System.Drawing.Size(0, 27)
        '
        'ToolStripLabelPromptTableIsInDatabase
        '
        Me.ToolStripLabelPromptTableIsInDatabase.ForeColor = System.Drawing.Color.LightSteelBlue
        Me.ToolStripLabelPromptTableIsInDatabase.Name = "ToolStripLabelPromptTableIsInDatabase"
        Me.ToolStripLabelPromptTableIsInDatabase.Size = New System.Drawing.Size(111, 27)
        Me.ToolStripLabelPromptTableIsInDatabase.Text = "Table is in database:"
        Me.ToolStripLabelPromptTableIsInDatabase.Visible = False
        '
        'ToolStripLabelTableIsInDatabase
        '
        Me.ToolStripLabelTableIsInDatabase.ForeColor = System.Drawing.Color.Gray
        Me.ToolStripLabelTableIsInDatabase.Name = "ToolStripLabelTableIsInDatabase"
        Me.ToolStripLabelTableIsInDatabase.Size = New System.Drawing.Size(33, 27)
        Me.ToolStripLabelTableIsInDatabase.Text = "False"
        Me.ToolStripLabelTableIsInDatabase.Visible = False
        '
        'ToolStripSplitDatabaseButton
        '
        Me.ToolStripSplitDatabaseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripSplitDatabaseButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemChangesAlterDatabase})
        Me.ToolStripSplitDatabaseButton.Image = Global.Boston.My.Resources.Resources.Database16x16
        Me.ToolStripSplitDatabaseButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitDatabaseButton.Name = "ToolStripSplitDatabaseButton"
        Me.ToolStripSplitDatabaseButton.Size = New System.Drawing.Size(32, 27)
        Me.ToolStripSplitDatabaseButton.Text = "ToolStripSplitButton1"
        '
        'ToolStripMenuItemChangesAlterDatabase
        '
        Me.ToolStripMenuItemChangesAlterDatabase.Name = "ToolStripMenuItemChangesAlterDatabase"
        Me.ToolStripMenuItemChangesAlterDatabase.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItemChangesAlterDatabase.Text = "Changes Alter Database"
        '
        'frmDiagramERD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 524)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.CircularProgressBar)
        Me.Name = "frmDiagramERD"
        Me.TabPageContextMenuStrip = Me.ContextMenuStripTab
        Me.TabText = "Entity Relationship Diagram"
        Me.Text = "Entity Relationship Diagram"
        Me.ContextMenuStrip_Entity.ResumeLayout(False)
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStripAttribute.ResumeLayout(False)
        Me.ContextMenuStrip_Relation.ResumeLayout(False)
        Me.ContextMenuStripTab.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.PanelMain.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents ContextMenuStrip_Entity As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOptionUseCaseDiagramEntity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemORMDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStripAttribute As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditAttributeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PageAsORMMetamodelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AutoLayoutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemIsMandatory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemIsPartOfPrimaryKey As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemMoveUp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemMoveDown As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDeleteAttribute As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Relation As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemDeleteRelation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEntityRelationshipDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PGSDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDisplayDataIndexRelationInformation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEntityModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStripTab As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllButThisPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDiagramIndexEditor As ToolStripMenuItem
    Friend WithEvents ToolStripSeparatorReCreateTable As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemReCreateDatabaseTable As ToolStripMenuItem
    Friend WithEvents ORMVerbalisationViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemAttributeModelErrors As ToolStripMenuItem
    Friend WithEvents ViewTableDataToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddRelatedEntitiesToThisPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ShowInDiagramSpyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemEntityIndexEditor As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemIndexManager As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddAttribute As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolboxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ConvertToFactTypeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemConvert As ToolStripMenuItem
    Friend WithEvents LanguageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PropertyGraphSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConvertToPropertyGraphSchemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents CircularProgressBar As CircularProgressBar.CircularProgressBar
    Friend WithEvents ViewInGlossaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewInModelDictionaryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents ShowInDiagramSpyToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
    Friend WithEvents MorphToToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents PGSDiagramToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ORMDiagramToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemERDDiagram1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents MakeAllColumnNamesDBNameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MakeAllDBNamesColumnNamesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LayeredToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrthogonalToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpringToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TreeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents PanelMain As Panel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripLabelPromptModel As ToolStripLabel
    Friend WithEvents ToolStripLabelModel As ToolStripLabel
    Friend WithEvents ToolStripLabelPromptTableIsInDatabase As ToolStripLabel
    Friend WithEvents ToolStripLabelTableIsInDatabase As ToolStripLabel
    Friend WithEvents DatabaseMappingShadingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DatabaseSchemaViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As ToolStripSeparator
    Friend WithEvents ToolStripSplitDatabaseButton As ToolStripSplitButton
    Friend WithEvents ToolStripMenuItemChangesAlterDatabase As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemRelationShowInModelDictionary As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRelationShowInDiagramSpy As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
End Class
