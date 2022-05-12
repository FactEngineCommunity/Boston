<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramPGS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiagramPGS))
        Me.ContextMenuStrip_Node = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ERDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ViewPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAttributeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_RemoveFromPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
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
        Me.CypherToolboxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemEdgeReadingEditor = New System.Windows.Forms.ToolStripMenuItem()
        Me.PageAsORMMetamodelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.AutoLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripAttribute = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditAttributeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteAttributeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemIsMandatory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemIsPartOfPrimaryKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.MoveUpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveDownToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Relation = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemERDDiagram1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRelationRemoveFromPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemEditRelation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemDeleteRelation = New System.Windows.Forms.ToolStripMenuItem()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStripTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllButThisPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram1 = New MindFusion.Diagramming.Diagram()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram2 = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_Node.SuspendLayout()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStripAttribute.SuspendLayout()
        Me.ContextMenuStrip_Relation.SuspendLayout()
        Me.ContextMenuStripTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip_Node
        '
        Me.ContextMenuStrip_Node.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator4, Me.ViewPropertiesToolStripMenuItem, Me.AddAttributeToolStripMenuItem, Me.ToolStripSeparator2, Me.ToolStripMenuItem_RemoveFromPage, Me.ToolStripSeparator9, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_Node.Name = "ContextMenuStrip_Actor"
        Me.ContextMenuStrip_Node.Size = New System.Drawing.Size(176, 132)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PGSDiagramToolStripMenuItem, Me.ORMDiagramToolStripMenuItem, Me.ERDiagramToolStripMenuItem})
        Me.MorphToToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To ..."
        '
        'PGSDiagramToolStripMenuItem
        '
        Me.PGSDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.PGS16x16
        Me.PGSDiagramToolStripMenuItem.Name = "PGSDiagramToolStripMenuItem"
        Me.PGSDiagramToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.PGSDiagramToolStripMenuItem.Text = "PGS Diagram"
        '
        'ORMDiagramToolStripMenuItem
        '
        Me.ORMDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ORM16x16
        Me.ORMDiagramToolStripMenuItem.Name = "ORMDiagramToolStripMenuItem"
        Me.ORMDiagramToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ORMDiagramToolStripMenuItem.Text = "&ORM Diagram"
        '
        'ERDiagramToolStripMenuItem
        '
        Me.ERDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ERD16x16
        Me.ERDiagramToolStripMenuItem.Name = "ERDiagramToolStripMenuItem"
        Me.ERDiagramToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ERDiagramToolStripMenuItem.Text = "&ER Diagram"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(172, 6)
        '
        'ViewPropertiesToolStripMenuItem
        '
        Me.ViewPropertiesToolStripMenuItem.Name = "ViewPropertiesToolStripMenuItem"
        Me.ViewPropertiesToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.ViewPropertiesToolStripMenuItem.Text = "View &Properties"
        '
        'AddAttributeToolStripMenuItem
        '
        Me.AddAttributeToolStripMenuItem.Image = CType(resources.GetObject("AddAttributeToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AddAttributeToolStripMenuItem.Name = "AddAttributeToolStripMenuItem"
        Me.AddAttributeToolStripMenuItem.Size = New System.Drawing.Size(175, 22)
        Me.AddAttributeToolStripMenuItem.Text = "Add &Attribute/s"
        Me.AddAttributeToolStripMenuItem.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(172, 6)
        Me.ToolStripSeparator2.Visible = False
        '
        'ToolStripMenuItem_RemoveFromPage
        '
        Me.ToolStripMenuItem_RemoveFromPage.Name = "ToolStripMenuItem_RemoveFromPage"
        Me.ToolStripMenuItem_RemoveFromPage.Size = New System.Drawing.Size(175, 22)
        Me.ToolStripMenuItem_RemoveFromPage.Text = "&Remove from Page"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(172, 6)
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(175, 22)
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
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator5, Me.AutoLayoutToolStripMenuItem, Me.CopyToolStripMenuItem})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 120)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolboxToolStripMenuItem, Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator3, Me.CypherToolboxToolStripMenuItem, Me.ToolStripSeparator7, Me.ToolStripMenuItemEdgeReadingEditor, Me.PageAsORMMetamodelToolStripMenuItem})
        Me.ViewToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ToolboxToolStripMenuItem
        '
        Me.ToolboxToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Toolbox16x16B_W
        Me.ToolboxToolStripMenuItem.Name = "ToolboxToolStripMenuItem"
        Me.ToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ToolboxToolStripMenuItem.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.dictionary16x16
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
        'CypherToolboxToolStripMenuItem
        '
        Me.CypherToolboxToolStripMenuItem.Name = "CypherToolboxToolStripMenuItem"
        Me.CypherToolboxToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.CypherToolboxToolStripMenuItem.Text = "&Cypher Toolbox"
        Me.CypherToolboxToolStripMenuItem.Visible = False
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(205, 6)
        Me.ToolStripSeparator7.Visible = False
        '
        'ToolStripMenuItemEdgeReadingEditor
        '
        Me.ToolStripMenuItemEdgeReadingEditor.Image = Global.Boston.My.Resources.MenuImagesMain.FactTypeReading16x16
        Me.ToolStripMenuItemEdgeReadingEditor.Name = "ToolStripMenuItemEdgeReadingEditor"
        Me.ToolStripMenuItemEdgeReadingEditor.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItemEdgeReadingEditor.Text = "ORM &Reading Editor"
        '
        'PageAsORMMetamodelToolStripMenuItem
        '
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
        Me.mnuOption_ViewGrid.Image = Global.Boston.My.Resources.MenuImages.Paste16x16
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(96, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(204, 6)
        '
        'AutoLayoutToolStripMenuItem
        '
        Me.AutoLayoutToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.AutoLayoutToolStripMenuItem.Name = "AutoLayoutToolStripMenuItem"
        Me.AutoLayoutToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.AutoLayoutToolStripMenuItem.Text = "&AutoLayout"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Camera16x16
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy Image to Clipboard"
        '
        'ContextMenuStripAttribute
        '
        Me.ContextMenuStripAttribute.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripAttribute.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditAttributeToolStripMenuItem, Me.DeleteAttributeToolStripMenuItem, Me.ToolStripSeparator1, Me.ToolStripMenuItemIsMandatory, Me.ToolStripMenuItemIsPartOfPrimaryKey, Me.ToolStripSeparator6, Me.MoveUpToolStripMenuItem, Me.MoveDownToolStripMenuItem})
        Me.ContextMenuStripAttribute.Name = "ContextMenuStripAttribute"
        Me.ContextMenuStripAttribute.Size = New System.Drawing.Size(195, 196)
        '
        'EditAttributeToolStripMenuItem
        '
        Me.EditAttributeToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.edit16x16
        Me.EditAttributeToolStripMenuItem.Name = "EditAttributeToolStripMenuItem"
        Me.EditAttributeToolStripMenuItem.Size = New System.Drawing.Size(194, 30)
        Me.EditAttributeToolStripMenuItem.Text = "&Edit Attribute"
        '
        'DeleteAttributeToolStripMenuItem
        '
        Me.DeleteAttributeToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.deleteround16x16
        Me.DeleteAttributeToolStripMenuItem.Name = "DeleteAttributeToolStripMenuItem"
        Me.DeleteAttributeToolStripMenuItem.Size = New System.Drawing.Size(194, 30)
        Me.DeleteAttributeToolStripMenuItem.Text = "D&elete Attribute"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(191, 6)
        '
        'ToolStripMenuItemIsMandatory
        '
        Me.ToolStripMenuItemIsMandatory.Image = CType(resources.GetObject("ToolStripMenuItemIsMandatory.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemIsMandatory.Name = "ToolStripMenuItemIsMandatory"
        Me.ToolStripMenuItemIsMandatory.Size = New System.Drawing.Size(194, 30)
        Me.ToolStripMenuItemIsMandatory.Text = "&Is Mandatory"
        '
        'ToolStripMenuItemIsPartOfPrimaryKey
        '
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Image = CType(resources.GetObject("ToolStripMenuItemIsPartOfPrimaryKey.Image"), System.Drawing.Image)
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Name = "ToolStripMenuItemIsPartOfPrimaryKey"
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Size = New System.Drawing.Size(194, 30)
        Me.ToolStripMenuItemIsPartOfPrimaryKey.Text = "&Is Part of Primary Key"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(191, 6)
        '
        'MoveUpToolStripMenuItem
        '
        Me.MoveUpToolStripMenuItem.Image = CType(resources.GetObject("MoveUpToolStripMenuItem.Image"), System.Drawing.Image)
        Me.MoveUpToolStripMenuItem.Name = "MoveUpToolStripMenuItem"
        Me.MoveUpToolStripMenuItem.Size = New System.Drawing.Size(194, 30)
        Me.MoveUpToolStripMenuItem.Text = "Move &Up"
        '
        'MoveDownToolStripMenuItem
        '
        Me.MoveDownToolStripMenuItem.Image = CType(resources.GetObject("MoveDownToolStripMenuItem.Image"), System.Drawing.Image)
        Me.MoveDownToolStripMenuItem.Name = "MoveDownToolStripMenuItem"
        Me.MoveDownToolStripMenuItem.Size = New System.Drawing.Size(194, 30)
        Me.MoveDownToolStripMenuItem.Text = "Move &Down"
        '
        'ContextMenuStrip_Relation
        '
        Me.ContextMenuStrip_Relation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem1, Me.ToolStripSeparator8, Me.ToolStripMenuItemRelationRemoveFromPage, Me.ToolStripMenuItemEditRelation, Me.ToolStripMenuItemDeleteRelation})
        Me.ContextMenuStrip_Relation.Name = "ContextMenuStrip_Relation"
        Me.ContextMenuStrip_Relation.Size = New System.Drawing.Size(176, 98)
        '
        'MorphToToolStripMenuItem1
        '
        Me.MorphToToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PGSDiagramToolStripMenuItem1, Me.ORMDiagramToolStripMenuItem1, Me.ToolStripMenuItemERDDiagram1})
        Me.MorphToToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem1.Name = "MorphToToolStripMenuItem1"
        Me.MorphToToolStripMenuItem1.Size = New System.Drawing.Size(175, 22)
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
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(172, 6)
        '
        'ToolStripMenuItemRelationRemoveFromPage
        '
        Me.ToolStripMenuItemRelationRemoveFromPage.Name = "ToolStripMenuItemRelationRemoveFromPage"
        Me.ToolStripMenuItemRelationRemoveFromPage.Size = New System.Drawing.Size(175, 22)
        Me.ToolStripMenuItemRelationRemoveFromPage.Text = "&Remove from Page"
        '
        'ToolStripMenuItemEditRelation
        '
        Me.ToolStripMenuItemEditRelation.Image = Global.Boston.My.Resources.MenuImages.edit16x16
        Me.ToolStripMenuItemEditRelation.Name = "ToolStripMenuItemEditRelation"
        Me.ToolStripMenuItemEditRelation.Size = New System.Drawing.Size(175, 22)
        Me.ToolStripMenuItemEditRelation.Text = "&Edit Relation"
        '
        'ToolStripMenuItemDeleteRelation
        '
        Me.ToolStripMenuItemDeleteRelation.Image = Global.Boston.My.Resources.MenuImages.deleteround16x16
        Me.ToolStripMenuItemDeleteRelation.Name = "ToolStripMenuItemDeleteRelation"
        Me.ToolStripMenuItemDeleteRelation.Size = New System.Drawing.Size(175, 22)
        Me.ToolStripMenuItemDeleteRelation.Text = "&Delete Relation"
        '
        'Diagram
        '
        Me.Diagram.AllowUnanchoredLinks = False
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.DefaultShape = MindFusion.Diagramming.Shape.FromId("Ellipse")
        Me.Diagram.DynamicLinks = True
        Me.Diagram.LinkBaseShapeSize = 4.0!
        Me.Diagram.LinkHeadShape = MindFusion.Diagramming.ArrowHead.PointerArrow
        Me.Diagram.LinkHeadShapeSize = 4.0!
        Me.Diagram.LinksSnapToBorders = True
        Me.Diagram.LinkStyle = MindFusion.Diagramming.LinkStyle.Bezier
        Me.Diagram.LinkTextStyle = MindFusion.Diagramming.LinkTextStyle.Rotate
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.TableBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFF0")
        Me.Diagram.TextColor = System.Drawing.Color.DarkGray
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
        Me.DiagramView.TabIndex = 12
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
        'frmDiagramPGS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(938, 524)
        Me.Controls.Add(Me.DiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Name = "frmDiagramPGS"
        Me.TabPageContextMenuStrip = Me.ContextMenuStripTab
        Me.TabText = "Property Graph Schema Diagram"
        Me.Text = "Property Graph Schema Diagram"
        Me.ContextMenuStrip_Node.ResumeLayout(False)
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStripAttribute.ResumeLayout(False)
        Me.ContextMenuStrip_Relation.ResumeLayout(False)
        Me.ContextMenuStripTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip_Node As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddAttributeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents MoveUpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveDownToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteAttributeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Relation As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemEditRelation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemDeleteRelation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CypherToolboxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents PGSDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ERDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphToToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PGSDiagramToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMDiagramToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemERDDiagram1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStripTab As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllButThisPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Diagram1 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram2 As MindFusion.Diagramming.Diagram
    Friend WithEvents ToolStripMenuItem_RemoveFromPage As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEdgeReadingEditor As ToolStripMenuItem
    Friend WithEvents ViewPropertiesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRelationRemoveFromPage As ToolStripMenuItem
End Class
