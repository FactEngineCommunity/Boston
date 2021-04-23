<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramERD
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiagramERD))
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_Entity = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEntityRelationshipDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemORMDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionUseCaseDiagramEntity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.DisplayDataIndexRelationInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEntityModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemReCreateDatabaseTable = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparatorReCreateTable = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.IndexEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMVerbalisationViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PageAsORMMetamodelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.AutoLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripAttribute = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemMoveUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemMoveDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.EditAttributeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDeleteAttribute = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemIsMandatory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemIsPartOfPrimaryKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Relation = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemEditRelation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDeleteRelation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllButThisPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAttributeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram1 = New MindFusion.Diagramming.Diagram()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram2 = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_Entity.SuspendLayout()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStripAttribute.SuspendLayout()
        Me.ContextMenuStrip_Relation.SuspendLayout()
        Me.ContextMenuStripTab.SuspendLayout()
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
        Me.ContextMenuStrip_Entity.AutoSize = False
        Me.ContextMenuStrip_Entity.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator2, Me.DisplayDataIndexRelationInformationToolStripMenuItem, Me.ToolStripMenuItemEntityModelErrors, Me.ToolStripSeparator8, Me.RemoveFromPageToolStripMenuItem, Me.ToolStripSeparator7, Me.ToolStripMenuItemReCreateDatabaseTable, Me.ToolStripSeparatorReCreateTable, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_Entity.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_Entity.Size = New System.Drawing.Size(323, 230)
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
        Me.MenuOptionUseCaseDiagramEntity.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.MenuOptionUseCaseDiagramEntity.Name = "MenuOptionUseCaseDiagramEntity"
        Me.MenuOptionUseCaseDiagramEntity.Size = New System.Drawing.Size(220, 22)
        Me.MenuOptionUseCaseDiagramEntity.Text = "&Use Case Diagram"
        Me.MenuOptionUseCaseDiagramEntity.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(319, 6)
        '
        'DisplayDataIndexRelationInformationToolStripMenuItem
        '
        Me.DisplayDataIndexRelationInformationToolStripMenuItem.Name = "DisplayDataIndexRelationInformationToolStripMenuItem"
        Me.DisplayDataIndexRelationInformationToolStripMenuItem.Size = New System.Drawing.Size(314, 22)
        Me.DisplayDataIndexRelationInformationToolStripMenuItem.Text = "&Display Index/Relation/Data Type Information"
        '
        'ToolStripMenuItemEntityModelErrors
        '
        Me.ToolStripMenuItemEntityModelErrors.Name = "ToolStripMenuItemEntityModelErrors"
        Me.ToolStripMenuItemEntityModelErrors.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemEntityModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(319, 6)
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
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(319, 6)
        '
        'ToolStripMenuItemReCreateDatabaseTable
        '
        Me.ToolStripMenuItemReCreateDatabaseTable.Name = "ToolStripMenuItemReCreateDatabaseTable"
        Me.ToolStripMenuItemReCreateDatabaseTable.Size = New System.Drawing.Size(314, 22)
        Me.ToolStripMenuItemReCreateDatabaseTable.Text = "Re/Create Database Table"
        '
        'ToolStripSeparatorReCreateTable
        '
        Me.ToolStripSeparatorReCreateTable.Name = "ToolStripSeparatorReCreateTable"
        Me.ToolStripSeparatorReCreateTable.Size = New System.Drawing.Size(319, 6)
        Me.ToolStripSeparatorReCreateTable.Visible = False
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
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
        Me.ContextMenuStrip_Diagram.AutoSize = False
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator5, Me.AutoLayoutToolStripMenuItem, Me.CopyToolStripMenuItem})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(216, 152)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator3, Me.IndexEditorToolStripMenuItem, Me.ORMVerbalisationViewToolStripMenuItem, Me.PageAsORMMetamodelToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(205, 6)
        '
        'IndexEditorToolStripMenuItem
        '
        Me.IndexEditorToolStripMenuItem.Image = CType(resources.GetObject("IndexEditorToolStripMenuItem.Image"), System.Drawing.Image)
        Me.IndexEditorToolStripMenuItem.Name = "IndexEditorToolStripMenuItem"
        Me.IndexEditorToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.IndexEditorToolStripMenuItem.Text = "&Index Editor"
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
        Me.PageAsORMMetamodelToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.ORM16x16
        Me.PageAsORMMetamodelToolStripMenuItem.Name = "PageAsORMMetamodelToolStripMenuItem"
        Me.PageAsORMMetamodelToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.PageAsORMMetamodelToolStripMenuItem.Text = "Page as &ORM Metamodel"
        Me.PageAsORMMetamodelToolStripMenuItem.Visible = False
        '
        'ShowHideToolStripMenuItem
        '
        Me.ShowHideToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_ViewGrid})
        Me.ShowHideToolStripMenuItem.Name = "ShowHideToolStripMenuItem"
        Me.ShowHideToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ShowHideToolStripMenuItem.Text = "&Show/Hide"
        '
        'mnuOption_ViewGrid
        '
        Me.mnuOption_ViewGrid.Image = Global.Boston.My.Resources.MenuImagesMain.Grid
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(96, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(212, 6)
        '
        'AutoLayoutToolStripMenuItem
        '
        Me.AutoLayoutToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.Layout16x16
        Me.AutoLayoutToolStripMenuItem.Name = "AutoLayoutToolStripMenuItem"
        Me.AutoLayoutToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.AutoLayoutToolStripMenuItem.Text = "&AutoLayout"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImagesMain.Camera16x16
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ContextMenuStripAttribute
        '
        Me.ContextMenuStripAttribute.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripSeparator4, Me.ToolStripMenuItemMoveUp, Me.ToolStripMenuItemMoveDown, Me.ToolStripSeparator6, Me.EditAttributeToolStripMenuItem, Me.ToolStripMenuItemDeleteAttribute, Me.ToolStripSeparator1, Me.ToolStripMenuItemIsMandatory, Me.ToolStripMenuItemIsPartOfPrimaryKey, Me.ToolStripMenuItemAttributeModelErrors})
        Me.ContextMenuStripAttribute.Name = "ContextMenuStripAttribute"
        Me.ContextMenuStripAttribute.Size = New System.Drawing.Size(187, 220)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem1.Text = "&Properties"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(183, 6)
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
        Me.ToolStripSeparator1.Visible = False
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
        'ContextMenuStrip_Relation
        '
        Me.ContextMenuStrip_Relation.AutoSize = False
        Me.ContextMenuStrip_Relation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemEditRelation, Me.ToolStripMenuItemDeleteRelation})
        Me.ContextMenuStrip_Relation.Name = "ContextMenuStrip_Relation"
        Me.ContextMenuStrip_Relation.Size = New System.Drawing.Size(189, 86)
        '
        'ToolStripMenuItemEditRelation
        '
        Me.ToolStripMenuItemEditRelation.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemEditRelation.Name = "ToolStripMenuItemEditRelation"
        Me.ToolStripMenuItemEditRelation.Size = New System.Drawing.Size(153, 22)
        Me.ToolStripMenuItemEditRelation.Text = "&Edit Relation"
        '
        'ToolStripMenuItemDeleteRelation
        '
        Me.ToolStripMenuItemDeleteRelation.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.ToolStripMenuItemDeleteRelation.Name = "ToolStripMenuItemDeleteRelation"
        Me.ToolStripMenuItemDeleteRelation.Size = New System.Drawing.Size(153, 22)
        Me.ToolStripMenuItemDeleteRelation.Text = "&Delete Relation"
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
        'ToolStripMenuItemAttributeModelErrors
        '
        Me.ToolStripMenuItemAttributeModelErrors.Name = "ToolStripMenuItemAttributeModelErrors"
        Me.ToolStripMenuItemAttributeModelErrors.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItemAttributeModelErrors.Text = "Model &Errors"
        '
        'DiagramView
        '
        Me.DiagramView.AllowDrop = True
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.Modify
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
        Me.DiagramView.Size = New System.Drawing.Size(938, 524)
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
        Me.HiddenDiagramView.Size = New System.Drawing.Size(938, 524)
        Me.HiddenDiagramView.TabIndex = 11
        Me.HiddenDiagramView.Text = "DiagramView1"
        '
        'frmDiagramERD
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 524)
        Me.Controls.Add(Me.DiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmDiagramERD"
        Me.TabPageContextMenuStrip = Me.ContextMenuStripTab
        Me.TabText = "Entity Relationship Diagram"
        Me.Text = "Entity Relationship Diagram"
        Me.ContextMenuStrip_Entity.ResumeLayout(False)
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStripAttribute.ResumeLayout(False)
        Me.ContextMenuStrip_Relation.ResumeLayout(False)
        Me.ContextMenuStripTab.ResumeLayout(False)
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
    Friend WithEvents ToolStripMenuItemEditRelation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDeleteRelation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEntityRelationshipDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PGSDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisplayDataIndexRelationInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEntityModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStripTab As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllButThisPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents IndexEditorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparatorReCreateTable As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemReCreateDatabaseTable As ToolStripMenuItem
    Friend WithEvents ORMVerbalisationViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemAttributeModelErrors As ToolStripMenuItem
    Friend WithEvents Diagram1 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram2 As MindFusion.Diagramming.Diagram
End Class
