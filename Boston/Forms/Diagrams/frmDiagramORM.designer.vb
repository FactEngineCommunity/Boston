<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiagramORM
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiagramORM))
        Me.SubjectAreaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintPreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportVisioDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToSVGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToDXFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToXMLFieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UndoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_OverviewTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_AddEntityType = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RoleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DomainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddDomainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditDomainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteDomainToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.SaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ContextMenuStrip_Diagram = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_Toolbox = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ErrorListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMVerbalisationViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RichmondBrainBoxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowHideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FactTypesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator31 = New System.Windows.Forms.ToolStripSeparator()
        Me.ViewFactTablesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRoleNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemFactTypeReadings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemFactTypeNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.FacToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSubtypeConstraints = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRoleConstraints = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemHelpTips = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewGrid = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemConvert = New System.Windows.Forms.ToolStripMenuItem()
        Me.LanguageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertyGraphSchemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EntityRelationshipDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19Convert = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator34 = New System.Windows.Forms.ToolStripSeparator()
        Me.AutoLayoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_CopyImageToClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.HiddenDiagram = New MindFusion.Diagramming.Diagram()
        Me.ContextMenuStrip_shape_list = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DockingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_DockRight = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_DockLeft = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_DockTop = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Role = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOption_Mandatory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_AddUniquenessConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeonticToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.SetNameFromHostingObjectTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemovefromPageAndModelToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator25 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemShowLinkFactType = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_ViewReadingEditor = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertiesToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_EntityType = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuOption_EntityTypeMorphTo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ERDiagramToolStripMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemPropertyGraphSchema = New System.Windows.Forms.ToolStripMenuItem()
        Me.UseCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuOptionDataFlowDiagramEntityType = New System.Windows.Forms.ToolStripMenuItem()
        Me.UMLClassDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemEntityTypeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.LockToThisPositionOnPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowInModelDictionaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInDiagramSpyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator30 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator40 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExpandTheReferenceSchemeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideTheReferenceSchemeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator32 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuOption_EntityTypeProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_ExternalRoleConstraint = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemModelErrorsExternalRoleConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator27 = New System.Windows.Forms.ToolStripSeparator()
        Me.ChangeToToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToUniquenessConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticUniqueness = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToValueComparison = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.GreaterThanOREqualToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LessThanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LessThanOREqualToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeonticGreaterThanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeonticGreaterThanOREqualToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeonticLessThanToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeonticLessThanOREqualToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToExclusiveConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToInclusiveOrConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToExclusiveOrConstranit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToEqualityConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemChangeToDeonticEqualityConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemRemoveAllArguments = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRemoveArgument = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem17 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveLinksToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RedoJoinPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInModelDictionaryToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator37 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveFromPageToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageModelToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_FactTable = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemAddFact = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteRowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteRowFactFromPageAndModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemFactModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.ResizeToFitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImportFactFromModelLevelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MorphTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MorphStepTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_ValueType = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemStateTransitionDiagram = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemValueTypeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ShowInModelDictionaryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator33 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem12 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpProvider = New System.Windows.Forms.HelpProvider()
        Me.ContextMenuStrip_RingConstraint = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemModelErrorsRingConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator28 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem24 = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintIrreflexiveToolStrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConsgtraintAsymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRnigConstraintIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintAntisymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintAcyclic = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintAsymmetricIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintAcyclicIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintSymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintSymmetricIrreflexive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintSymmetricIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintPurelyReflexive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticIrreflexive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticAsymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticAntisymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemDeonticAcyclic = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticAsymmetricIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticAcyclicIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticSymmetric = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticSymmetricIrreflexive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticSymmetricIntransitive = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuItemRingConstraintDeonticPurelyReflexive = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem13 = New System.Windows.Forms.ToolStripMenuItem()
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.ComboBoxFact = New System.Windows.Forms.ComboBox()
        Me.LabelHelp = New System.Windows.Forms.Label()
        Me.ContextMenuStrip_HelpTips = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComboBoxEntityType = New System.Windows.Forms.ComboBox()
        Me.ComboBoxValueType = New System.Windows.Forms.ComboBox()
        Me.ContextMenuStrip_SubtypeRelationship = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemSubtypeShowCorrespondingFactType = New System.Windows.Forms.ToolStripMenuItem()
        Me.HideCorrespondingFactTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator36 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_FrequencyConstraint = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemModelErrorsFrequencyConstraint = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator26 = New System.Windows.Forms.ToolStripSeparator()
        Me.ReoveFromPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageAndModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_InternalUniquenessConstraint = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowInModelDictionaryToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator38 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveFromPageModelToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator39 = New System.Windows.Forms.ToolStripSeparator()
        Me.PropertiesToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Animator1 = New Viev.Animator.Animator()
        Me.ContextMenuStripVirtualAnalyst = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideMeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CircularProgressBar = New CircularProgressBar.CircularProgressBar()
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.ContextMenuStripTab = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllButThisPageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemFactTypeModelErrors = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripSeparator35 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemFactTypeRemoveFromPageModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ContextMenuStrip_FactType = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MorphToToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ORMFromFactTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ERDiagramFromFactTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PGSDiagramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuOption_IsObjectified = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemViewFactTable = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAddRole = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowInModelDictionaryToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PropertieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripModelNote = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemModelNoteRemoveFromPageAndModel = New System.Windows.Forms.ToolStripMenuItem()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram1 = New MindFusion.Diagramming.Diagram()
        Me.HiddenDiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.Diagram2 = New MindFusion.Diagramming.Diagram()
        Me.ToolStripSeparator41 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem14 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator42 = New System.Windows.Forms.ToolStripSeparator()
        Me.ContextMenuStrip_Diagram.SuspendLayout()
        Me.ContextMenuStrip_shape_list.SuspendLayout()
        Me.ContextMenuStrip_Role.SuspendLayout()
        Me.ContextMenuStrip_EntityType.SuspendLayout()
        Me.ContextMenuStrip_ExternalRoleConstraint.SuspendLayout()
        Me.ContextMenuStrip_FactTable.SuspendLayout()
        Me.ContextMenuStrip_ValueType.SuspendLayout()
        Me.ContextMenuStrip_RingConstraint.SuspendLayout()
        Me.ContextMenuStrip_HelpTips.SuspendLayout()
        Me.ContextMenuStrip_SubtypeRelationship.SuspendLayout()
        Me.ContextMenuStrip_FrequencyConstraint.SuspendLayout()
        Me.ContextMenuStrip_InternalUniquenessConstraint.SuspendLayout()
        Me.ContextMenuStripVirtualAnalyst.SuspendLayout()
        Me.ContextMenuStripTab.SuspendLayout()
        Me.ContextMenuStrip_FactType.SuspendLayout()
        Me.ContextMenuStripModelNote.SuspendLayout()
        Me.SuspendLayout()
        '
        'SubjectAreaToolStripMenuItem
        '
        Me.SubjectAreaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintPreviewToolStripMenuItem, Me.PrintToolStripMenuItem, Me.ToolStripSeparator1, Me.ImportToolStripMenuItem, Me.ExportToolStripMenuItem, Me.SaveToXMLFieToolStripMenuItem})
        Me.SubjectAreaToolStripMenuItem.Name = "SubjectAreaToolStripMenuItem"
        Me.SubjectAreaToolStripMenuItem.Size = New System.Drawing.Size(81, 20)
        Me.SubjectAreaToolStripMenuItem.Text = "S&ubject Area"
        '
        'PrintPreviewToolStripMenuItem
        '
        Me.PrintPreviewToolStripMenuItem.Name = "PrintPreviewToolStripMenuItem"
        Me.PrintPreviewToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.PrintPreviewToolStripMenuItem.Text = "Print Pre&view"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.PrintToolStripMenuItem.Text = "&Print"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(152, 6)
        '
        'ImportToolStripMenuItem
        '
        Me.ImportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImportVisioDiagramToolStripMenuItem})
        Me.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem"
        Me.ImportToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.ImportToolStripMenuItem.Text = "&Import"
        '
        'ImportVisioDiagramToolStripMenuItem
        '
        Me.ImportVisioDiagramToolStripMenuItem.Name = "ImportVisioDiagramToolStripMenuItem"
        Me.ImportVisioDiagramToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.ImportVisioDiagramToolStripMenuItem.Text = "Import &Visio Diagram"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportToToolStripMenuItem, Me.ExportToSVGToolStripMenuItem, Me.ExportToToolStripMenuItem1, Me.ExportToToolStripMenuItem2, Me.ExportToDXFToolStripMenuItem})
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.ExportToolStripMenuItem.Text = "&Export"
        '
        'ExportToToolStripMenuItem
        '
        Me.ExportToToolStripMenuItem.Name = "ExportToToolStripMenuItem"
        Me.ExportToToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.ExportToToolStripMenuItem.Text = "Export to &PDF"
        '
        'ExportToSVGToolStripMenuItem
        '
        Me.ExportToSVGToolStripMenuItem.Name = "ExportToSVGToolStripMenuItem"
        Me.ExportToSVGToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.ExportToSVGToolStripMenuItem.Text = "Export to &SVG"
        '
        'ExportToToolStripMenuItem1
        '
        Me.ExportToToolStripMenuItem1.Name = "ExportToToolStripMenuItem1"
        Me.ExportToToolStripMenuItem1.Size = New System.Drawing.Size(150, 22)
        Me.ExportToToolStripMenuItem1.Text = "Export to P&NG"
        '
        'ExportToToolStripMenuItem2
        '
        Me.ExportToToolStripMenuItem2.Name = "ExportToToolStripMenuItem2"
        Me.ExportToToolStripMenuItem2.Size = New System.Drawing.Size(150, 22)
        Me.ExportToToolStripMenuItem2.Text = "Export to &Visio"
        '
        'ExportToDXFToolStripMenuItem
        '
        Me.ExportToDXFToolStripMenuItem.Name = "ExportToDXFToolStripMenuItem"
        Me.ExportToDXFToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.ExportToDXFToolStripMenuItem.Text = "Export to D&XF"
        '
        'SaveToXMLFieToolStripMenuItem
        '
        Me.SaveToXMLFieToolStripMenuItem.Name = "SaveToXMLFieToolStripMenuItem"
        Me.SaveToXMLFieToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.SaveToXMLFieToolStripMenuItem.Text = "Save to &XML fie"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UndoToolStripMenuItem, Me.ToolStripSeparator2, Me.CopyToolStripMenuItem, Me.PasteToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'UndoToolStripMenuItem
        '
        Me.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem"
        Me.UndoToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.UndoToolStripMenuItem.Text = "&Undo"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(100, 6)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.CopyToolStripMenuItem.Text = "&Copy"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.PasteToolStripMenuItem.Text = "&Paste"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_OverviewTool})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'mnuOption_OverviewTool
        '
        Me.mnuOption_OverviewTool.CheckOnClick = True
        Me.mnuOption_OverviewTool.Name = "mnuOption_OverviewTool"
        Me.mnuOption_OverviewTool.Size = New System.Drawing.Size(148, 22)
        Me.mnuOption_OverviewTool.Text = "&Overview Tool"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem3, Me.RoleToolStripMenuItem, Me.DomainToolStripMenuItem})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(42, 20)
        Me.ToolStripMenuItem2.Text = "ORM"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_AddEntityType, Me.ToolStripMenuItem6, Me.ToolStripMenuItem7})
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(116, 22)
        Me.ToolStripMenuItem3.Text = "&Entity"
        '
        'mnuOption_AddEntityType
        '
        Me.mnuOption_AddEntityType.Name = "mnuOption_AddEntityType"
        Me.mnuOption_AddEntityType.Size = New System.Drawing.Size(167, 22)
        Me.mnuOption_AddEntityType.Text = "&Add Entity Type"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem6.Text = "&Edit Entity Type"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem7.Text = "&Delete Entity Type"
        '
        'RoleToolStripMenuItem
        '
        Me.RoleToolStripMenuItem.Name = "RoleToolStripMenuItem"
        Me.RoleToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.RoleToolStripMenuItem.Text = "&Role"
        '
        'DomainToolStripMenuItem
        '
        Me.DomainToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDomainToolStripMenuItem, Me.EditDomainToolStripMenuItem, Me.DeleteDomainToolStripMenuItem})
        Me.DomainToolStripMenuItem.Name = "DomainToolStripMenuItem"
        Me.DomainToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.DomainToolStripMenuItem.Text = "&Domain"
        '
        'AddDomainToolStripMenuItem
        '
        Me.AddDomainToolStripMenuItem.Name = "AddDomainToolStripMenuItem"
        Me.AddDomainToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AddDomainToolStripMenuItem.Text = "&Add Domain"
        '
        'EditDomainToolStripMenuItem
        '
        Me.EditDomainToolStripMenuItem.Name = "EditDomainToolStripMenuItem"
        Me.EditDomainToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EditDomainToolStripMenuItem.Text = "&Edit Domain"
        '
        'DeleteDomainToolStripMenuItem
        '
        Me.DeleteDomainToolStripMenuItem.Name = "DeleteDomainToolStripMenuItem"
        Me.DeleteDomainToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DeleteDomainToolStripMenuItem.Text = "&Delete Domain"
        '
        'Diagram
        '
        Me.Diagram.AdjustmentHandlesSize = 1.5!
        Me.Diagram.AlignToGrid = False
        Me.Diagram.AllowSelfLoops = False
        Me.Diagram.AllowUnconnectedLinks = True
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.DynamicLinks = True
        Me.Diagram.ExpandButtonPosition = MindFusion.Diagramming.ExpandButtonPosition.OuterLeft
        Me.Diagram.LinkBaseShapeSize = 2.0!
        Me.Diagram.LinkBrush = New MindFusion.Drawing.SolidBrush("#FF790079")
        Me.Diagram.LinkHeadShape = MindFusion.Diagramming.ArrowHead.None
        Me.Diagram.LinkHeadShapeSize = 2.0!
        Me.Diagram.LinksSnapToBorders = True
        Me.Diagram.SelectAfterCreate = False
        Me.Diagram.ShadowColor = System.Drawing.Color.Black
        Me.Diagram.ShadowOffsetX = 0!
        Me.Diagram.ShadowOffsetY = 0!
        Me.Diagram.ShapeBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        Me.Diagram.ShapeCustomDraw = MindFusion.Diagramming.CustomDraw.Additional
        Me.Diagram.ShapeHandlesStyle = MindFusion.Diagramming.HandlesStyle.InvisibleMove
        Me.Diagram.ShapePen = New MindFusion.Drawing.Pen("0/#FF000000/0.01/0/0//0/0/10/")
        Me.Diagram.ShowAnchors = MindFusion.Diagramming.ShowAnchors.Never
        Me.Diagram.ShowHandlesOnDrag = False
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.FileName = "OpenFileDialog1"
        '
        'ContextMenuStrip_Diagram
        '
        Me.ContextMenuStrip_Diagram.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem1, Me.ShowHideToolStripMenuItem, Me.ToolStripSeparator3, Me.ToolStripMenuItemConvert, Me.ToolStripSeparator19Convert, Me.ToolStripMenuItemPaste, Me.ToolStripSeparator34, Me.AutoLayoutToolStripMenuItem, Me.mnuOption_CopyImageToClipboard})
        Me.ContextMenuStrip_Diagram.Name = "ContextMenuStrip_Diagram"
        Me.ContextMenuStrip_Diagram.Size = New System.Drawing.Size(208, 154)
        '
        'ViewToolStripMenuItem1
        '
        Me.ViewToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_Toolbox, Me.ModelDictionaryToolStripMenuItem, Me.PropertiesToolStripMenuItem, Me.ToolStripSeparator18, Me.ErrorListToolStripMenuItem, Me.ToolStripMenuItem8, Me.ORMVerbalisationViewToolStripMenuItem, Me.RichmondBrainBoxToolStripMenuItem})
        Me.ViewToolStripMenuItem1.Name = "ViewToolStripMenuItem1"
        Me.ViewToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.ViewToolStripMenuItem1.Text = "&View"
        '
        'mnuOption_Toolbox
        '
        Me.mnuOption_Toolbox.Image = Global.Boston.My.Resources.MenuImages.Toolbox16x16B_W
        Me.mnuOption_Toolbox.Name = "mnuOption_Toolbox"
        Me.mnuOption_Toolbox.Size = New System.Drawing.Size(203, 22)
        Me.mnuOption_Toolbox.Text = "&Toolbox"
        '
        'ModelDictionaryToolStripMenuItem
        '
        Me.ModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.dictionary16x16
        Me.ModelDictionaryToolStripMenuItem.Name = "ModelDictionaryToolStripMenuItem"
        Me.ModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ModelDictionaryToolStripMenuItem.Text = "Model &Dictionary"
        '
        'PropertiesToolStripMenuItem
        '
        Me.PropertiesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem.Name = "PropertiesToolStripMenuItem"
        Me.PropertiesToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.PropertiesToolStripMenuItem.Text = "&Properties"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(200, 6)
        '
        'ErrorListToolStripMenuItem
        '
        Me.ErrorListToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.ErrorList
        Me.ErrorListToolStripMenuItem.Name = "ErrorListToolStripMenuItem"
        Me.ErrorListToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ErrorListToolStripMenuItem.Text = "&Error List"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Image = Global.Boston.My.Resources.MenuImages.FactTypeReading16x16
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(203, 22)
        Me.ToolStripMenuItem8.Text = "Fact Type &Reading Editor"
        '
        'ORMVerbalisationViewToolStripMenuItem
        '
        Me.ORMVerbalisationViewToolStripMenuItem.Image = CType(resources.GetObject("ORMVerbalisationViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMVerbalisationViewToolStripMenuItem.Name = "ORMVerbalisationViewToolStripMenuItem"
        Me.ORMVerbalisationViewToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.ORMVerbalisationViewToolStripMenuItem.Text = "ORM &Verbalisation View"
        '
        'RichmondBrainBoxToolStripMenuItem
        '
        Me.RichmondBrainBoxToolStripMenuItem.Image = CType(resources.GetObject("RichmondBrainBoxToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RichmondBrainBoxToolStripMenuItem.Name = "RichmondBrainBoxToolStripMenuItem"
        Me.RichmondBrainBoxToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.RichmondBrainBoxToolStripMenuItem.Text = "&Virtual Analyst"
        '
        'ShowHideToolStripMenuItem
        '
        Me.ShowHideToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FactTypesToolStripMenuItem, Me.FacToolStripMenuItem, Me.MenuItemHelpTips, Me.mnuOption_ViewGrid})
        Me.ShowHideToolStripMenuItem.Name = "ShowHideToolStripMenuItem"
        Me.ShowHideToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.ShowHideToolStripMenuItem.Text = "&Show/Hide"
        '
        'FactTypesToolStripMenuItem
        '
        Me.FactTypesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem, Me.ToolStripSeparator31, Me.ViewFactTablesToolStripMenuItem, Me.ToolStripMenuItemRoleNames, Me.ToolStripMenuItemFactTypeReadings, Me.ToolStripMenuItemFactTypeNames})
        Me.FactTypesToolStripMenuItem.Name = "FactTypesToolStripMenuItem"
        Me.FactTypesToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.FactTypesToolStripMenuItem.Text = "&Fact Types"
        '
        'HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem
        '
        Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Image = CType(resources.GetObject("HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Name = "HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem"
        Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Size = New System.Drawing.Size(376, 22)
        Me.HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem.Text = "&Hide Fact Type Names, Role Names && Fact Type Readings"
        '
        'ToolStripSeparator31
        '
        Me.ToolStripSeparator31.Name = "ToolStripSeparator31"
        Me.ToolStripSeparator31.Size = New System.Drawing.Size(373, 6)
        '
        'ViewFactTablesToolStripMenuItem
        '
        Me.ViewFactTablesToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.FactTables16x16
        Me.ViewFactTablesToolStripMenuItem.Name = "ViewFactTablesToolStripMenuItem"
        Me.ViewFactTablesToolStripMenuItem.Size = New System.Drawing.Size(376, 22)
        Me.ViewFactTablesToolStripMenuItem.Text = "&Fact Tables"
        '
        'ToolStripMenuItemRoleNames
        '
        Me.ToolStripMenuItemRoleNames.Checked = True
        Me.ToolStripMenuItemRoleNames.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemRoleNames.Name = "ToolStripMenuItemRoleNames"
        Me.ToolStripMenuItemRoleNames.Size = New System.Drawing.Size(376, 22)
        Me.ToolStripMenuItemRoleNames.Text = "&Role Names"
        '
        'ToolStripMenuItemFactTypeReadings
        '
        Me.ToolStripMenuItemFactTypeReadings.Checked = True
        Me.ToolStripMenuItemFactTypeReadings.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemFactTypeReadings.Name = "ToolStripMenuItemFactTypeReadings"
        Me.ToolStripMenuItemFactTypeReadings.Size = New System.Drawing.Size(376, 22)
        Me.ToolStripMenuItemFactTypeReadings.Text = "Fact Type Rea&dings"
        '
        'ToolStripMenuItemFactTypeNames
        '
        Me.ToolStripMenuItemFactTypeNames.Checked = True
        Me.ToolStripMenuItemFactTypeNames.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemFactTypeNames.Name = "ToolStripMenuItemFactTypeNames"
        Me.ToolStripMenuItemFactTypeNames.Size = New System.Drawing.Size(376, 22)
        Me.ToolStripMenuItemFactTypeNames.Text = "Fact Type &Names"
        '
        'FacToolStripMenuItem
        '
        Me.FacToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemSubtypeConstraints, Me.ToolStripMenuItemRoleConstraints})
        Me.FacToolStripMenuItem.Name = "FacToolStripMenuItem"
        Me.FacToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.FacToolStripMenuItem.Text = "&Constraints"
        '
        'ToolStripMenuItemSubtypeConstraints
        '
        Me.ToolStripMenuItemSubtypeConstraints.Checked = True
        Me.ToolStripMenuItemSubtypeConstraints.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemSubtypeConstraints.Name = "ToolStripMenuItemSubtypeConstraints"
        Me.ToolStripMenuItemSubtypeConstraints.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItemSubtypeConstraints.Text = "&Subtype Constraints"
        '
        'ToolStripMenuItemRoleConstraints
        '
        Me.ToolStripMenuItemRoleConstraints.Checked = True
        Me.ToolStripMenuItemRoleConstraints.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItemRoleConstraints.Name = "ToolStripMenuItemRoleConstraints"
        Me.ToolStripMenuItemRoleConstraints.Size = New System.Drawing.Size(180, 22)
        Me.ToolStripMenuItemRoleConstraints.Text = "&Role Constraints"
        '
        'MenuItemHelpTips
        '
        Me.MenuItemHelpTips.Image = Global.Boston.My.Resources.MenuImages.HelpTips16x16
        Me.MenuItemHelpTips.Name = "MenuItemHelpTips"
        Me.MenuItemHelpTips.Size = New System.Drawing.Size(134, 22)
        Me.MenuItemHelpTips.Text = "Help Tips"
        '
        'mnuOption_ViewGrid
        '
        Me.mnuOption_ViewGrid.Image = Global.Boston.My.Resources.MenuImages.Grid
        Me.mnuOption_ViewGrid.Name = "mnuOption_ViewGrid"
        Me.mnuOption_ViewGrid.Size = New System.Drawing.Size(134, 22)
        Me.mnuOption_ViewGrid.Text = "&Grid"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(204, 6)
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
        Me.LanguageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PropertyGraphSchemaToolStripMenuItem, Me.EntityRelationshipDiagramToolStripMenuItem})
        Me.LanguageToolStripMenuItem.Image = CType(resources.GetObject("LanguageToolStripMenuItem.Image"), System.Drawing.Image)
        Me.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem"
        Me.LanguageToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.LanguageToolStripMenuItem.Text = "To &Language..."
        '
        'PropertyGraphSchemaToolStripMenuItem
        '
        Me.PropertyGraphSchemaToolStripMenuItem.Image = CType(resources.GetObject("PropertyGraphSchemaToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PropertyGraphSchemaToolStripMenuItem.Name = "PropertyGraphSchemaToolStripMenuItem"
        Me.PropertyGraphSchemaToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.PropertyGraphSchemaToolStripMenuItem.Text = "Property &Graph Schema"
        '
        'EntityRelationshipDiagramToolStripMenuItem
        '
        Me.EntityRelationshipDiagramToolStripMenuItem.Image = CType(resources.GetObject("EntityRelationshipDiagramToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EntityRelationshipDiagramToolStripMenuItem.Name = "EntityRelationshipDiagramToolStripMenuItem"
        Me.EntityRelationshipDiagramToolStripMenuItem.Size = New System.Drawing.Size(220, 22)
        Me.EntityRelationshipDiagramToolStripMenuItem.Text = "&Entity Relationship Diagram"
        '
        'ToolStripSeparator19Convert
        '
        Me.ToolStripSeparator19Convert.Name = "ToolStripSeparator19Convert"
        Me.ToolStripSeparator19Convert.Size = New System.Drawing.Size(204, 6)
        '
        'ToolStripMenuItemPaste
        '
        Me.ToolStripMenuItemPaste.Enabled = False
        Me.ToolStripMenuItemPaste.Name = "ToolStripMenuItemPaste"
        Me.ToolStripMenuItemPaste.Size = New System.Drawing.Size(207, 22)
        Me.ToolStripMenuItemPaste.Text = "&Paste"
        '
        'ToolStripSeparator34
        '
        Me.ToolStripSeparator34.Name = "ToolStripSeparator34"
        Me.ToolStripSeparator34.Size = New System.Drawing.Size(204, 6)
        '
        'AutoLayoutToolStripMenuItem
        '
        Me.AutoLayoutToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Layout16x16
        Me.AutoLayoutToolStripMenuItem.Name = "AutoLayoutToolStripMenuItem"
        Me.AutoLayoutToolStripMenuItem.Size = New System.Drawing.Size(207, 22)
        Me.AutoLayoutToolStripMenuItem.Text = "&AutoLayout"
        '
        'mnuOption_CopyImageToClipboard
        '
        Me.mnuOption_CopyImageToClipboard.Image = Global.Boston.My.Resources.MenuImages.Camera16x16
        Me.mnuOption_CopyImageToClipboard.Name = "mnuOption_CopyImageToClipboard"
        Me.mnuOption_CopyImageToClipboard.Size = New System.Drawing.Size(207, 22)
        Me.mnuOption_CopyImageToClipboard.Text = "&Copy Image to Clipboard"
        '
        'HiddenDiagram
        '
        Me.HiddenDiagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'ContextMenuStrip_shape_list
        '
        Me.ContextMenuStrip_shape_list.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_shape_list.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DockingToolStripMenuItem})
        Me.ContextMenuStrip_shape_list.Name = "ContextMenuStrip_shape_list"
        Me.ContextMenuStrip_shape_list.Size = New System.Drawing.Size(119, 26)
        '
        'DockingToolStripMenuItem
        '
        Me.DockingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_DockRight, Me.mnuOption_DockLeft, Me.mnuOption_DockTop})
        Me.DockingToolStripMenuItem.Name = "DockingToolStripMenuItem"
        Me.DockingToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.DockingToolStripMenuItem.Text = "&Docking"
        '
        'mnuOption_DockRight
        '
        Me.mnuOption_DockRight.Name = "mnuOption_DockRight"
        Me.mnuOption_DockRight.Size = New System.Drawing.Size(102, 22)
        Me.mnuOption_DockRight.Text = "&Right"
        '
        'mnuOption_DockLeft
        '
        Me.mnuOption_DockLeft.Name = "mnuOption_DockLeft"
        Me.mnuOption_DockLeft.Size = New System.Drawing.Size(102, 22)
        Me.mnuOption_DockLeft.Text = "&Left"
        '
        'mnuOption_DockTop
        '
        Me.mnuOption_DockTop.Name = "mnuOption_DockTop"
        Me.mnuOption_DockTop.Size = New System.Drawing.Size(102, 22)
        Me.mnuOption_DockTop.Text = "&Top"
        '
        'ContextMenuStrip_Role
        '
        Me.ContextMenuStrip_Role.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_Mandatory, Me.mnuOption_AddUniquenessConstraint, Me.DeonticToolStripMenuItem, Me.ToolStripSeparator8, Me.SetNameFromHostingObjectTypeToolStripMenuItem, Me.ToolStripSeparator23, Me.RemovefromPageAndModelToolStripMenuItem1, Me.ToolStripSeparator25, Me.ToolStripMenuItemShowLinkFactType, Me.mnuOption_ViewReadingEditor, Me.PropertiesToolStripMenuItem1})
        Me.ContextMenuStrip_Role.Name = "ContextMenuStrip_Role"
        Me.ContextMenuStrip_Role.Size = New System.Drawing.Size(265, 198)
        '
        'mnuOption_Mandatory
        '
        Me.mnuOption_Mandatory.Image = Global.Boston.My.Resources.ORMShapes.mandatory
        Me.mnuOption_Mandatory.Name = "mnuOption_Mandatory"
        Me.mnuOption_Mandatory.Size = New System.Drawing.Size(264, 22)
        Me.mnuOption_Mandatory.Text = "&Mandatory"
        '
        'mnuOption_AddUniquenessConstraint
        '
        Me.mnuOption_AddUniquenessConstraint.Image = Global.Boston.My.Resources.ORMShapes.uniqueness
        Me.mnuOption_AddUniquenessConstraint.Name = "mnuOption_AddUniquenessConstraint"
        Me.mnuOption_AddUniquenessConstraint.Size = New System.Drawing.Size(264, 22)
        Me.mnuOption_AddUniquenessConstraint.Text = "Add &Uniqueness Constraint"
        '
        'DeonticToolStripMenuItem
        '
        Me.DeonticToolStripMenuItem.Name = "DeonticToolStripMenuItem"
        Me.DeonticToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.DeonticToolStripMenuItem.Text = "&Deontic"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(261, 6)
        '
        'SetNameFromHostingObjectTypeToolStripMenuItem
        '
        Me.SetNameFromHostingObjectTypeToolStripMenuItem.Name = "SetNameFromHostingObjectTypeToolStripMenuItem"
        Me.SetNameFromHostingObjectTypeToolStripMenuItem.Size = New System.Drawing.Size(264, 22)
        Me.SetNameFromHostingObjectTypeToolStripMenuItem.Text = "&Set Name from Hosting Object Type"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(261, 6)
        '
        'RemovefromPageAndModelToolStripMenuItem1
        '
        Me.RemovefromPageAndModelToolStripMenuItem1.Name = "RemovefromPageAndModelToolStripMenuItem1"
        Me.RemovefromPageAndModelToolStripMenuItem1.Size = New System.Drawing.Size(264, 22)
        Me.RemovefromPageAndModelToolStripMenuItem1.Text = "Remove &from Page and Model"
        '
        'ToolStripSeparator25
        '
        Me.ToolStripSeparator25.Name = "ToolStripSeparator25"
        Me.ToolStripSeparator25.Size = New System.Drawing.Size(261, 6)
        '
        'ToolStripMenuItemShowLinkFactType
        '
        Me.ToolStripMenuItemShowLinkFactType.Name = "ToolStripMenuItemShowLinkFactType"
        Me.ToolStripMenuItemShowLinkFactType.Size = New System.Drawing.Size(264, 22)
        Me.ToolStripMenuItemShowLinkFactType.Text = "Show &Link Fact Type"
        '
        'mnuOption_ViewReadingEditor
        '
        Me.mnuOption_ViewReadingEditor.Image = Global.Boston.My.Resources.MenuImages.FactTypeReading16x16
        Me.mnuOption_ViewReadingEditor.Name = "mnuOption_ViewReadingEditor"
        Me.mnuOption_ViewReadingEditor.Size = New System.Drawing.Size(264, 22)
        Me.mnuOption_ViewReadingEditor.Text = "&View Reading Editor"
        '
        'PropertiesToolStripMenuItem1
        '
        Me.PropertiesToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertiesToolStripMenuItem1.Name = "PropertiesToolStripMenuItem1"
        Me.PropertiesToolStripMenuItem1.Size = New System.Drawing.Size(264, 22)
        Me.PropertiesToolStripMenuItem1.Text = "&Properties"
        '
        'ContextMenuStrip_EntityType
        '
        Me.ContextMenuStrip_EntityType.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOption_EntityTypeMorphTo, Me.ToolStripSeparator5, Me.ToolStripMenuItemEntityTypeModelErrors, Me.ToolStripSeparator20, Me.LockToThisPositionOnPageToolStripMenuItem, Me.AddAllAssociatedFactTypesToPageToolStripMenuItem, Me.ShowInModelDictionaryToolStripMenuItem, Me.ToolStripSeparator12, Me.ShowInDiagramSpyToolStripMenuItem, Me.ToolStripSeparator30, Me.ToolStripMenuItemCopy, Me.ToolStripSeparator40, Me.ExpandTheReferenceSchemeToolStripMenuItem, Me.HideTheReferenceSchemeToolStripMenuItem, Me.ToolStripSeparator32, Me.RemoveFromPageToolStripMenuItem, Me.RemoveFromPageModelToolStripMenuItem, Me.ToolStripSeparator15, Me.mnuOption_EntityTypeProperties})
        Me.ContextMenuStrip_EntityType.Name = "ContextMenuStrip_EntityType"
        Me.ContextMenuStrip_EntityType.Size = New System.Drawing.Size(270, 310)
        '
        'mnuOption_EntityTypeMorphTo
        '
        Me.mnuOption_EntityTypeMorphTo.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ORMDiagramToolStripMenuItem, Me.ERDiagramToolStripMenu, Me.ToolStripMenuItemPropertyGraphSchema, Me.UseCToolStripMenuItem, Me.MenuOptionDataFlowDiagramEntityType, Me.UMLClassDiagramToolStripMenuItem})
        Me.mnuOption_EntityTypeMorphTo.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.mnuOption_EntityTypeMorphTo.Name = "mnuOption_EntityTypeMorphTo"
        Me.mnuOption_EntityTypeMorphTo.Size = New System.Drawing.Size(269, 22)
        Me.mnuOption_EntityTypeMorphTo.Text = "&Morph to..."
        '
        'ORMDiagramToolStripMenuItem
        '
        Me.ORMDiagramToolStripMenuItem.Image = CType(resources.GetObject("ORMDiagramToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMDiagramToolStripMenuItem.Name = "ORMDiagramToolStripMenuItem"
        Me.ORMDiagramToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ORMDiagramToolStripMenuItem.Text = "&ORM Diagram"
        '
        'ERDiagramToolStripMenu
        '
        Me.ERDiagramToolStripMenu.Image = Global.Boston.My.Resources.Resources.ERD16x16
        Me.ERDiagramToolStripMenu.Name = "ERDiagramToolStripMenu"
        Me.ERDiagramToolStripMenu.Size = New System.Drawing.Size(199, 22)
        Me.ERDiagramToolStripMenu.Text = "&ER Diagram"
        Me.ERDiagramToolStripMenu.Visible = False
        '
        'ToolStripMenuItemPropertyGraphSchema
        '
        Me.ToolStripMenuItemPropertyGraphSchema.Image = Global.Boston.My.Resources.Resources.PGS16x16
        Me.ToolStripMenuItemPropertyGraphSchema.Name = "ToolStripMenuItemPropertyGraphSchema"
        Me.ToolStripMenuItemPropertyGraphSchema.Size = New System.Drawing.Size(199, 22)
        Me.ToolStripMenuItemPropertyGraphSchema.Text = "&Property Graph Schema"
        '
        'UseCToolStripMenuItem
        '
        Me.UseCToolStripMenuItem.Name = "UseCToolStripMenuItem"
        Me.UseCToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.UseCToolStripMenuItem.Text = "&Use Case Diagram"
        Me.UseCToolStripMenuItem.Visible = False
        '
        'MenuOptionDataFlowDiagramEntityType
        '
        Me.MenuOptionDataFlowDiagramEntityType.Name = "MenuOptionDataFlowDiagramEntityType"
        Me.MenuOptionDataFlowDiagramEntityType.Size = New System.Drawing.Size(199, 22)
        Me.MenuOptionDataFlowDiagramEntityType.Text = "&Data Flow Diagram"
        Me.MenuOptionDataFlowDiagramEntityType.Visible = False
        '
        'UMLClassDiagramToolStripMenuItem
        '
        Me.UMLClassDiagramToolStripMenuItem.Name = "UMLClassDiagramToolStripMenuItem"
        Me.UMLClassDiagramToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.UMLClassDiagramToolStripMenuItem.Text = "UML &Class Diagram"
        Me.UMLClassDiagramToolStripMenuItem.Visible = False
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(266, 6)
        '
        'ToolStripMenuItemEntityTypeModelErrors
        '
        Me.ToolStripMenuItemEntityTypeModelErrors.Name = "ToolStripMenuItemEntityTypeModelErrors"
        Me.ToolStripMenuItemEntityTypeModelErrors.Size = New System.Drawing.Size(269, 22)
        Me.ToolStripMenuItemEntityTypeModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(266, 6)
        '
        'LockToThisPositionOnPageToolStripMenuItem
        '
        Me.LockToThisPositionOnPageToolStripMenuItem.Name = "LockToThisPositionOnPageToolStripMenuItem"
        Me.LockToThisPositionOnPageToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.LockToThisPositionOnPageToolStripMenuItem.Text = "&Lock to this position on Page"
        Me.LockToThisPositionOnPageToolStripMenuItem.Visible = False
        '
        'AddAllAssociatedFactTypesToPageToolStripMenuItem
        '
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem.Name = "AddAllAssociatedFactTypesToPageToolStripMenuItem"
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem.Text = "&Add all associated Fact Types to Page"
        '
        'ShowInModelDictionaryToolStripMenuItem
        '
        Me.ShowInModelDictionaryToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem.Name = "ShowInModelDictionaryToolStripMenuItem"
        Me.ShowInModelDictionaryToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.ShowInModelDictionaryToolStripMenuItem.Text = "Show in Model &Dictionary"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(266, 6)
        '
        'ShowInDiagramSpyToolStripMenuItem
        '
        Me.ShowInDiagramSpyToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Spyglass16x16
        Me.ShowInDiagramSpyToolStripMenuItem.Name = "ShowInDiagramSpyToolStripMenuItem"
        Me.ShowInDiagramSpyToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.ShowInDiagramSpyToolStripMenuItem.Text = "Show in Diagram &Spy"
        '
        'ToolStripSeparator30
        '
        Me.ToolStripSeparator30.Name = "ToolStripSeparator30"
        Me.ToolStripSeparator30.Size = New System.Drawing.Size(266, 6)
        '
        'ToolStripMenuItemCopy
        '
        Me.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy"
        Me.ToolStripMenuItemCopy.Size = New System.Drawing.Size(269, 22)
        Me.ToolStripMenuItemCopy.Text = "&Copy"
        '
        'ToolStripSeparator40
        '
        Me.ToolStripSeparator40.Name = "ToolStripSeparator40"
        Me.ToolStripSeparator40.Size = New System.Drawing.Size(266, 6)
        '
        'ExpandTheReferenceSchemeToolStripMenuItem
        '
        Me.ExpandTheReferenceSchemeToolStripMenuItem.Name = "ExpandTheReferenceSchemeToolStripMenuItem"
        Me.ExpandTheReferenceSchemeToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.ExpandTheReferenceSchemeToolStripMenuItem.Text = "&Expand the Reference Scheme"
        '
        'HideTheReferenceSchemeToolStripMenuItem
        '
        Me.HideTheReferenceSchemeToolStripMenuItem.Name = "HideTheReferenceSchemeToolStripMenuItem"
        Me.HideTheReferenceSchemeToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.HideTheReferenceSchemeToolStripMenuItem.Text = "&Hide the Reference Scheme"
        Me.HideTheReferenceSchemeToolStripMenuItem.Visible = False
        '
        'ToolStripSeparator32
        '
        Me.ToolStripSeparator32.Name = "ToolStripSeparator32"
        Me.ToolStripSeparator32.Size = New System.Drawing.Size(266, 6)
        '
        'RemoveFromPageToolStripMenuItem
        '
        Me.RemoveFromPageToolStripMenuItem.Name = "RemoveFromPageToolStripMenuItem"
        Me.RemoveFromPageToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.RemoveFromPageToolStripMenuItem.Text = "&Remove from Page"
        '
        'RemoveFromPageModelToolStripMenuItem
        '
        Me.RemoveFromPageModelToolStripMenuItem.Name = "RemoveFromPageModelToolStripMenuItem"
        Me.RemoveFromPageModelToolStripMenuItem.Size = New System.Drawing.Size(269, 22)
        Me.RemoveFromPageModelToolStripMenuItem.Text = "&Remove from Page && Model"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(266, 6)
        '
        'mnuOption_EntityTypeProperties
        '
        Me.mnuOption_EntityTypeProperties.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.mnuOption_EntityTypeProperties.Name = "mnuOption_EntityTypeProperties"
        Me.mnuOption_EntityTypeProperties.Size = New System.Drawing.Size(269, 22)
        Me.mnuOption_EntityTypeProperties.Text = "&Properties"
        '
        'ContextMenuStrip_ExternalRoleConstraint
        '
        Me.ContextMenuStrip_ExternalRoleConstraint.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_ExternalRoleConstraint.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemModelErrorsExternalRoleConstraint, Me.ToolStripSeparator27, Me.ChangeToToolStripMenuItem1, Me.ToolStripSeparator7, Me.ToolStripMenuItemRemoveAllArguments, Me.ToolStripMenuItemRemoveArgument, Me.RemoveLinksToolStripMenuItem, Me.RedoJoinPathToolStripMenuItem, Me.ToolStripSeparator24, Me.ShowInModelDictionaryToolStripMenuItem3, Me.ToolStripSeparator37, Me.RemoveFromPageToolStripMenuItem1, Me.RemoveFromPageModelToolStripMenuItem2, Me.ToolStripSeparator14, Me.PropertiesToolStripMenuItem2})
        Me.ContextMenuStrip_ExternalRoleConstraint.Name = "ContextMenuStrip_ExternalRoleConstraint"
        Me.ContextMenuStrip_ExternalRoleConstraint.Size = New System.Drawing.Size(234, 334)
        '
        'ToolStripMenuItemModelErrorsExternalRoleConstraint
        '
        Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.Name = "ToolStripMenuItemModelErrorsExternalRoleConstraint"
        Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItemModelErrorsExternalRoleConstraint.Text = "Model &Errors"
        '
        'ToolStripSeparator27
        '
        Me.ToolStripSeparator27.Name = "ToolStripSeparator27"
        Me.ToolStripSeparator27.Size = New System.Drawing.Size(230, 6)
        '
        'ChangeToToolStripMenuItem1
        '
        Me.ChangeToToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemChangeToUniquenessConstraint, Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint, Me.ToolStripMenuItemChangeToDeonticUniqueness, Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint, Me.ToolStripMenuItemChangeToValueComparison, Me.ToolStripMenuItemChangeToExclusiveConstraint, Me.ToolStripMenuItemChangeToInclusiveOrConstraint, Me.ToolStripMenuItemChangeToExclusiveOrConstranit, Me.ToolStripMenuItemChangeToEqualityConstraint, Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint, Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint, Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint, Me.ToolStripMenuItemChangeToDeonticEqualityConstraint})
        Me.ChangeToToolStripMenuItem1.Name = "ChangeToToolStripMenuItem1"
        Me.ChangeToToolStripMenuItem1.Size = New System.Drawing.Size(233, 30)
        Me.ChangeToToolStripMenuItem1.Text = "&Change to..."
        '
        'ToolStripMenuItemChangeToUniquenessConstraint
        '
        Me.ToolStripMenuItemChangeToUniquenessConstraint.Image = Global.Boston.My.Resources.Resources.externalUniqueness
        Me.ToolStripMenuItemChangeToUniquenessConstraint.Name = "ToolStripMenuItemChangeToUniquenessConstraint"
        Me.ToolStripMenuItemChangeToUniquenessConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToUniquenessConstraint.Text = "&Uniqueness Constraint"
        '
        'ToolStripMenuItemChangeToUniquenessPreferredConstraint
        '
        Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Image = Global.Boston.My.Resources.Resources.preferred_uniqueness
        Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Name = "ToolStripMenuItemChangeToUniquenessPreferredConstraint"
        Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToUniquenessPreferredConstraint.Text = "&Preferred Uniqueness Constraint"
        '
        'ToolStripMenuItemChangeToDeonticUniqueness
        '
        Me.ToolStripMenuItemChangeToDeonticUniqueness.Image = Global.Boston.My.Resources.Resources.deontic_external_uniqueness
        Me.ToolStripMenuItemChangeToDeonticUniqueness.Name = "ToolStripMenuItemChangeToDeonticUniqueness"
        Me.ToolStripMenuItemChangeToDeonticUniqueness.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticUniqueness.Text = "Deontic Uniqueness Constraint"
        '
        'ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint
        '
        Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Image = Global.Boston.My.Resources.Resources.deontic_external_preferred_uniqueness
        Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Name = "ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint"
        Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint.Text = "Deontic Preferred Uniqueness Constraint"
        '
        'ToolStripMenuItemChangeToValueComparison
        '
        Me.ToolStripMenuItemChangeToValueComparison.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem4, Me.GreaterThanOREqualToToolStripMenuItem, Me.LessThanToolStripMenuItem, Me.LessThanOREqualToToolStripMenuItem, Me.DeonticGreaterThanToolStripMenuItem, Me.DeonticGreaterThanOREqualToToolStripMenuItem, Me.DeonticLessThanToolStripMenuItem, Me.DeonticLessThanOREqualToToolStripMenuItem})
        Me.ToolStripMenuItemChangeToValueComparison.Name = "ToolStripMenuItemChangeToValueComparison"
        Me.ToolStripMenuItemChangeToValueComparison.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToValueComparison.Text = "&Value Comparison"
        Me.ToolStripMenuItemChangeToValueComparison.Visible = False
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Image = Global.Boston.My.Resources.ORMShapes.value_comparison_greater
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(251, 22)
        Me.ToolStripMenuItem4.Text = "&Greater Than"
        '
        'GreaterThanOREqualToToolStripMenuItem
        '
        Me.GreaterThanOREqualToToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.value_comparison_greater_equal
        Me.GreaterThanOREqualToToolStripMenuItem.Name = "GreaterThanOREqualToToolStripMenuItem"
        Me.GreaterThanOREqualToToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.GreaterThanOREqualToToolStripMenuItem.Text = "&Greater Than OR Equal To"
        '
        'LessThanToolStripMenuItem
        '
        Me.LessThanToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.value_comparrison_less
        Me.LessThanToolStripMenuItem.Name = "LessThanToolStripMenuItem"
        Me.LessThanToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.LessThanToolStripMenuItem.Text = "&Less Than"
        '
        'LessThanOREqualToToolStripMenuItem
        '
        Me.LessThanOREqualToToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.value_comparrison_less_than
        Me.LessThanOREqualToToolStripMenuItem.Name = "LessThanOREqualToToolStripMenuItem"
        Me.LessThanOREqualToToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.LessThanOREqualToToolStripMenuItem.Text = "Less Than OR Equal To"
        '
        'DeonticGreaterThanToolStripMenuItem
        '
        Me.DeonticGreaterThanToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.deontic_value_comparison_greater
        Me.DeonticGreaterThanToolStripMenuItem.Name = "DeonticGreaterThanToolStripMenuItem"
        Me.DeonticGreaterThanToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DeonticGreaterThanToolStripMenuItem.Text = "Deontic Greater Than"
        '
        'DeonticGreaterThanOREqualToToolStripMenuItem
        '
        Me.DeonticGreaterThanOREqualToToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.deontic_value_comparison_greater_equal
        Me.DeonticGreaterThanOREqualToToolStripMenuItem.Name = "DeonticGreaterThanOREqualToToolStripMenuItem"
        Me.DeonticGreaterThanOREqualToToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DeonticGreaterThanOREqualToToolStripMenuItem.Text = "Deontic Greater Than OR Equal To"
        '
        'DeonticLessThanToolStripMenuItem
        '
        Me.DeonticLessThanToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.deontic_value_comparrison_less
        Me.DeonticLessThanToolStripMenuItem.Name = "DeonticLessThanToolStripMenuItem"
        Me.DeonticLessThanToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DeonticLessThanToolStripMenuItem.Text = "Deontic Less Than"
        '
        'DeonticLessThanOREqualToToolStripMenuItem
        '
        Me.DeonticLessThanOREqualToToolStripMenuItem.Image = Global.Boston.My.Resources.ORMShapes.deontic_value_comparrison_less_than
        Me.DeonticLessThanOREqualToToolStripMenuItem.Name = "DeonticLessThanOREqualToToolStripMenuItem"
        Me.DeonticLessThanOREqualToToolStripMenuItem.Size = New System.Drawing.Size(251, 22)
        Me.DeonticLessThanOREqualToToolStripMenuItem.Text = "Deontic Less Than OR Equal To"
        '
        'ToolStripMenuItemChangeToExclusiveConstraint
        '
        Me.ToolStripMenuItemChangeToExclusiveConstraint.Image = Global.Boston.My.Resources.ORMShapes.exclusion
        Me.ToolStripMenuItemChangeToExclusiveConstraint.Name = "ToolStripMenuItemChangeToExclusiveConstraint"
        Me.ToolStripMenuItemChangeToExclusiveConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToExclusiveConstraint.Text = "Exclusion Constraint"
        '
        'ToolStripMenuItemChangeToInclusiveOrConstraint
        '
        Me.ToolStripMenuItemChangeToInclusiveOrConstraint.Image = Global.Boston.My.Resources.ORMShapes.inclusive_or
        Me.ToolStripMenuItemChangeToInclusiveOrConstraint.Name = "ToolStripMenuItemChangeToInclusiveOrConstraint"
        Me.ToolStripMenuItemChangeToInclusiveOrConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToInclusiveOrConstraint.Text = "Inclusive Or Constraint"
        '
        'ToolStripMenuItemChangeToExclusiveOrConstranit
        '
        Me.ToolStripMenuItemChangeToExclusiveOrConstranit.Image = Global.Boston.My.Resources.ORMShapes.exclusiveOr
        Me.ToolStripMenuItemChangeToExclusiveOrConstranit.Name = "ToolStripMenuItemChangeToExclusiveOrConstranit"
        Me.ToolStripMenuItemChangeToExclusiveOrConstranit.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToExclusiveOrConstranit.Text = "Exclusive Or Constraint"
        '
        'ToolStripMenuItemChangeToEqualityConstraint
        '
        Me.ToolStripMenuItemChangeToEqualityConstraint.Image = Global.Boston.My.Resources.ORMShapes.equality
        Me.ToolStripMenuItemChangeToEqualityConstraint.Name = "ToolStripMenuItemChangeToEqualityConstraint"
        Me.ToolStripMenuItemChangeToEqualityConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToEqualityConstraint.Text = "Equality Constraint"
        '
        'ToolStripMenuItemChangeToDeonticExclusiveConstraint
        '
        Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint.Image = Global.Boston.My.Resources.ORMShapes.deontic_exclusion
        Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint.Name = "ToolStripMenuItemChangeToDeonticExclusiveConstraint"
        Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticExclusiveConstraint.Text = "Deontic Exclusion Constraint"
        '
        'ToolStripMenuItemChangeToDeonticInclusiveOrConstraint
        '
        Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Image = Global.Boston.My.Resources.ORMShapes.deontic_inclusive_or
        Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Name = "ToolStripMenuItemChangeToDeonticInclusiveOrConstraint"
        Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticInclusiveOrConstraint.Text = "Deontic Inclusive Or Constraint"
        '
        'ToolStripMenuItemChangeToDeonticExclusiveOrConstraint
        '
        Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Image = Global.Boston.My.Resources.ORMShapes.deontic_exclusiveOr
        Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Name = "ToolStripMenuItemChangeToDeonticExclusiveOrConstraint"
        Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticExclusiveOrConstraint.Text = "Deontic Exclusive Or Constraint"
        '
        'ToolStripMenuItemChangeToDeonticEqualityConstraint
        '
        Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Image = Global.Boston.My.Resources.ORMShapes.deontic_equality
        Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Name = "ToolStripMenuItemChangeToDeonticEqualityConstraint"
        Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Size = New System.Drawing.Size(288, 22)
        Me.ToolStripMenuItemChangeToDeonticEqualityConstraint.Text = "Deontic Equality Constraint"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(230, 6)
        '
        'ToolStripMenuItemRemoveAllArguments
        '
        Me.ToolStripMenuItemRemoveAllArguments.Name = "ToolStripMenuItemRemoveAllArguments"
        Me.ToolStripMenuItemRemoveAllArguments.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItemRemoveAllArguments.Text = "Remove all Argument&s"
        '
        'ToolStripMenuItemRemoveArgument
        '
        Me.ToolStripMenuItemRemoveArgument.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem17})
        Me.ToolStripMenuItemRemoveArgument.Name = "ToolStripMenuItemRemoveArgument"
        Me.ToolStripMenuItemRemoveArgument.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItemRemoveArgument.Text = "Remove &Argument #..."
        '
        'ToolStripMenuItem17
        '
        Me.ToolStripMenuItem17.Name = "ToolStripMenuItem17"
        Me.ToolStripMenuItem17.Size = New System.Drawing.Size(80, 22)
        Me.ToolStripMenuItem17.Text = "&1"
        '
        'RemoveLinksToolStripMenuItem
        '
        Me.RemoveLinksToolStripMenuItem.Name = "RemoveLinksToolStripMenuItem"
        Me.RemoveLinksToolStripMenuItem.Size = New System.Drawing.Size(233, 30)
        Me.RemoveLinksToolStripMenuItem.Text = "Remove &Links"
        '
        'RedoJoinPathToolStripMenuItem
        '
        Me.RedoJoinPathToolStripMenuItem.Name = "RedoJoinPathToolStripMenuItem"
        Me.RedoJoinPathToolStripMenuItem.Size = New System.Drawing.Size(233, 30)
        Me.RedoJoinPathToolStripMenuItem.Text = "Redo &JoinPath"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(230, 6)
        '
        'ShowInModelDictionaryToolStripMenuItem3
        '
        Me.ShowInModelDictionaryToolStripMenuItem3.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem3.Name = "ShowInModelDictionaryToolStripMenuItem3"
        Me.ShowInModelDictionaryToolStripMenuItem3.Size = New System.Drawing.Size(233, 30)
        Me.ShowInModelDictionaryToolStripMenuItem3.Text = "Show in Model &Dictionary"
        '
        'ToolStripSeparator37
        '
        Me.ToolStripSeparator37.Name = "ToolStripSeparator37"
        Me.ToolStripSeparator37.Size = New System.Drawing.Size(230, 6)
        '
        'RemoveFromPageToolStripMenuItem1
        '
        Me.RemoveFromPageToolStripMenuItem1.Name = "RemoveFromPageToolStripMenuItem1"
        Me.RemoveFromPageToolStripMenuItem1.Size = New System.Drawing.Size(233, 30)
        Me.RemoveFromPageToolStripMenuItem1.Text = "&Remove from Page"
        '
        'RemoveFromPageModelToolStripMenuItem2
        '
        Me.RemoveFromPageModelToolStripMenuItem2.Name = "RemoveFromPageModelToolStripMenuItem2"
        Me.RemoveFromPageModelToolStripMenuItem2.Size = New System.Drawing.Size(233, 30)
        Me.RemoveFromPageModelToolStripMenuItem2.Text = "Remove from Page && &Model"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(230, 6)
        '
        'PropertiesToolStripMenuItem2
        '
        Me.PropertiesToolStripMenuItem2.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem2.Name = "PropertiesToolStripMenuItem2"
        Me.PropertiesToolStripMenuItem2.Size = New System.Drawing.Size(233, 30)
        Me.PropertiesToolStripMenuItem2.Text = "&Properties"
        '
        'ChangeToToolStripMenuItem
        '
        Me.ChangeToToolStripMenuItem.Name = "ChangeToToolStripMenuItem"
        Me.ChangeToToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ChangeToToolStripMenuItem.Text = "&Change to..."
        '
        'ContextMenuStrip_FactTable
        '
        Me.ContextMenuStrip_FactTable.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_FactTable.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddFact, Me.DeleteRowToolStripMenuItem, Me.DeleteRowFactFromPageAndModelToolStripMenuItem, Me.ToolStripMenuItem15, Me.ToolStripMenuItemFactModelErrors, Me.ToolStripSeparator21, Me.ResizeToFitToolStripMenuItem1, Me.HideToolStripMenuItem, Me.ToolStripSeparator9, Me.ImportFactFromModelLevelToolStripMenuItem})
        Me.ContextMenuStrip_FactTable.Name = "ContextMenuStrip_FactTable"
        Me.ContextMenuStrip_FactTable.Size = New System.Drawing.Size(279, 176)
        '
        'ToolStripMenuItemAddFact
        '
        Me.ToolStripMenuItemAddFact.Name = "ToolStripMenuItemAddFact"
        Me.ToolStripMenuItemAddFact.Size = New System.Drawing.Size(278, 22)
        Me.ToolStripMenuItemAddFact.Text = "&Add Row/Fact"
        '
        'DeleteRowToolStripMenuItem
        '
        Me.DeleteRowToolStripMenuItem.Name = "DeleteRowToolStripMenuItem"
        Me.DeleteRowToolStripMenuItem.Size = New System.Drawing.Size(278, 22)
        Me.DeleteRowToolStripMenuItem.Text = "&Delete Row/Fact from Page"
        '
        'DeleteRowFactFromPageAndModelToolStripMenuItem
        '
        Me.DeleteRowFactFromPageAndModelToolStripMenuItem.Name = "DeleteRowFactFromPageAndModelToolStripMenuItem"
        Me.DeleteRowFactFromPageAndModelToolStripMenuItem.Size = New System.Drawing.Size(278, 22)
        Me.DeleteRowFactFromPageAndModelToolStripMenuItem.Text = "Delete Row/Fact from &Page and Model"
        '
        'ToolStripMenuItem15
        '
        Me.ToolStripMenuItem15.Name = "ToolStripMenuItem15"
        Me.ToolStripMenuItem15.Size = New System.Drawing.Size(275, 6)
        '
        'ToolStripMenuItemFactModelErrors
        '
        Me.ToolStripMenuItemFactModelErrors.Name = "ToolStripMenuItemFactModelErrors"
        Me.ToolStripMenuItemFactModelErrors.Size = New System.Drawing.Size(278, 22)
        Me.ToolStripMenuItemFactModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(275, 6)
        '
        'ResizeToFitToolStripMenuItem1
        '
        Me.ResizeToFitToolStripMenuItem1.Name = "ResizeToFitToolStripMenuItem1"
        Me.ResizeToFitToolStripMenuItem1.Size = New System.Drawing.Size(278, 22)
        Me.ResizeToFitToolStripMenuItem1.Text = "&Resize to fit text"
        '
        'HideToolStripMenuItem
        '
        Me.HideToolStripMenuItem.Name = "HideToolStripMenuItem"
        Me.HideToolStripMenuItem.Size = New System.Drawing.Size(278, 22)
        Me.HideToolStripMenuItem.Text = "&Hide"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(275, 6)
        '
        'ImportFactFromModelLevelToolStripMenuItem
        '
        Me.ImportFactFromModelLevelToolStripMenuItem.Name = "ImportFactFromModelLevelToolStripMenuItem"
        Me.ImportFactFromModelLevelToolStripMenuItem.Size = New System.Drawing.Size(278, 22)
        Me.ImportFactFromModelLevelToolStripMenuItem.Text = "&Import Fact from Model level"
        '
        'MorphTimer
        '
        Me.MorphTimer.Interval = 3500
        '
        'MorphStepTimer
        '
        Me.MorphStepTimer.Interval = 17
        '
        'ContextMenuStrip_ValueType
        '
        Me.ContextMenuStrip_ValueType.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_ValueType.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem5, Me.ToolStripSeparator6, Me.ToolStripMenuItemValueTypeModelErrors, Me.ToolStripSeparator19, Me.ShowInModelDictionaryToolStripMenuItem1, Me.ToolStripSeparator33, Me.ToolStripMenuItem11, Me.ToolStripMenuItem12, Me.ToolStripSeparator16, Me.ToolStripMenuItem10})
        Me.ContextMenuStrip_ValueType.Name = "ContextMenuStrip_ValueType"
        Me.ContextMenuStrip_ValueType.Size = New System.Drawing.Size(234, 208)
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram, Me.ToolStripMenuItemStateTransitionDiagram})
        Me.ToolStripMenuItem5.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItem5.Text = "&Morph to..."
        '
        'ToolStripMenuItem_ValueTypeMorph_ORMDiagram
        '
        Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.Image = CType(resources.GetObject("ToolStripMenuItem_ValueTypeMorph_ORMDiagram.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.Name = "ToolStripMenuItem_ValueTypeMorph_ORMDiagram"
        Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.Size = New System.Drawing.Size(202, 22)
        Me.ToolStripMenuItem_ValueTypeMorph_ORMDiagram.Text = "&ORM Diagram"
        '
        'ToolStripMenuItemStateTransitionDiagram
        '
        Me.ToolStripMenuItemStateTransitionDiagram.Image = Global.Boston.My.Resources.Resources.StateTransitionDiagram_16x16
        Me.ToolStripMenuItemStateTransitionDiagram.Name = "ToolStripMenuItemStateTransitionDiagram"
        Me.ToolStripMenuItemStateTransitionDiagram.Size = New System.Drawing.Size(202, 22)
        Me.ToolStripMenuItemStateTransitionDiagram.Text = "&State Transition Diagram"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(230, 6)
        '
        'ToolStripMenuItemValueTypeModelErrors
        '
        Me.ToolStripMenuItemValueTypeModelErrors.Name = "ToolStripMenuItemValueTypeModelErrors"
        Me.ToolStripMenuItemValueTypeModelErrors.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItemValueTypeModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(230, 6)
        '
        'ShowInModelDictionaryToolStripMenuItem1
        '
        Me.ShowInModelDictionaryToolStripMenuItem1.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem1.Name = "ShowInModelDictionaryToolStripMenuItem1"
        Me.ShowInModelDictionaryToolStripMenuItem1.Size = New System.Drawing.Size(233, 30)
        Me.ShowInModelDictionaryToolStripMenuItem1.Text = "Show in Model &Dictionary"
        '
        'ToolStripSeparator33
        '
        Me.ToolStripSeparator33.Name = "ToolStripSeparator33"
        Me.ToolStripSeparator33.Size = New System.Drawing.Size(230, 6)
        '
        'ToolStripMenuItem11
        '
        Me.ToolStripMenuItem11.Name = "ToolStripMenuItem11"
        Me.ToolStripMenuItem11.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItem11.Text = "&Remove from Page"
        '
        'ToolStripMenuItem12
        '
        Me.ToolStripMenuItem12.Name = "ToolStripMenuItem12"
        Me.ToolStripMenuItem12.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItem12.Text = "&Remove from Page && Model"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(230, 6)
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(233, 30)
        Me.ToolStripMenuItem10.Text = "&Properties"
        '
        'ContextMenuStrip_RingConstraint
        '
        Me.ContextMenuStrip_RingConstraint.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_RingConstraint.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemModelErrorsRingConstraint, Me.ToolStripSeparator28, Me.ToolStripMenuItem24, Me.ToolStripSeparator13, Me.ToolStripMenuItem9, Me.ToolStripMenuItem13})
        Me.ContextMenuStrip_RingConstraint.Name = "ContextMenuStrip_RingConstraint"
        Me.ContextMenuStrip_RingConstraint.Size = New System.Drawing.Size(226, 104)
        '
        'ToolStripMenuItemModelErrorsRingConstraint
        '
        Me.ToolStripMenuItemModelErrorsRingConstraint.Name = "ToolStripMenuItemModelErrorsRingConstraint"
        Me.ToolStripMenuItemModelErrorsRingConstraint.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItemModelErrorsRingConstraint.Text = "Model &Errors"
        '
        'ToolStripSeparator28
        '
        Me.ToolStripSeparator28.Name = "ToolStripSeparator28"
        Me.ToolStripSeparator28.Size = New System.Drawing.Size(222, 6)
        '
        'ToolStripMenuItem24
        '
        Me.ToolStripMenuItem24.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemRingConstraintIrreflexiveToolStrip, Me.MenuItemRingConsgtraintAsymmetric, Me.MenuItemRnigConstraintIntransitive, Me.MenuItemRingConstraintAntisymmetric, Me.MenuItemRingConstraintAcyclic, Me.MenuItemRingConstraintAsymmetricIntransitive, Me.MenuItemRingConstraintAcyclicIntransitive, Me.MenuItemRingConstraintSymmetric, Me.MenuItemRingConstraintSymmetricIrreflexive, Me.MenuItemRingConstraintSymmetricIntransitive, Me.MenuItemRingConstraintPurelyReflexive, Me.MenuItemRingConstraintDeonticIrreflexive, Me.MenuItemRingConstraintDeonticAsymmetric, Me.MenuItemRingConstraintDeonticIntransitive, Me.MenuItemRingConstraintDeonticAntisymmetric, Me.MenuItemDeonticAcyclic, Me.MenuItemRingConstraintDeonticAsymmetricIntransitive, Me.MenuItemRingConstraintDeonticAcyclicIntransitive, Me.MenuItemRingConstraintDeonticSymmetric, Me.MenuItemRingConstraintDeonticSymmetricIrreflexive, Me.MenuItemRingConstraintDeonticSymmetricIntransitive, Me.MenuItemRingConstraintDeonticPurelyReflexive})
        Me.ToolStripMenuItem24.Name = "ToolStripMenuItem24"
        Me.ToolStripMenuItem24.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem24.Text = "&Change to..."
        '
        'MenuItemRingConstraintIrreflexiveToolStrip
        '
        Me.MenuItemRingConstraintIrreflexiveToolStrip.Image = Global.Boston.My.Resources.ORMShapes.irreflexive
        Me.MenuItemRingConstraintIrreflexiveToolStrip.Name = "MenuItemRingConstraintIrreflexiveToolStrip"
        Me.MenuItemRingConstraintIrreflexiveToolStrip.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintIrreflexiveToolStrip.Text = "Irreflexive"
        '
        'MenuItemRingConsgtraintAsymmetric
        '
        Me.MenuItemRingConsgtraintAsymmetric.Image = Global.Boston.My.Resources.ORMShapes.Asymmetric
        Me.MenuItemRingConsgtraintAsymmetric.Name = "MenuItemRingConsgtraintAsymmetric"
        Me.MenuItemRingConsgtraintAsymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConsgtraintAsymmetric.Text = "Asymmetric"
        '
        'MenuItemRnigConstraintIntransitive
        '
        Me.MenuItemRnigConstraintIntransitive.Image = Global.Boston.My.Resources.ORMShapes.intransitive
        Me.MenuItemRnigConstraintIntransitive.Name = "MenuItemRnigConstraintIntransitive"
        Me.MenuItemRnigConstraintIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRnigConstraintIntransitive.Text = "Intransitive"
        '
        'MenuItemRingConstraintAntisymmetric
        '
        Me.MenuItemRingConstraintAntisymmetric.Image = Global.Boston.My.Resources.ORMShapes.Antisymmetric
        Me.MenuItemRingConstraintAntisymmetric.Name = "MenuItemRingConstraintAntisymmetric"
        Me.MenuItemRingConstraintAntisymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintAntisymmetric.Text = "Antisymmetric"
        '
        'MenuItemRingConstraintAcyclic
        '
        Me.MenuItemRingConstraintAcyclic.Image = Global.Boston.My.Resources.ORMShapes.acyclic
        Me.MenuItemRingConstraintAcyclic.Name = "MenuItemRingConstraintAcyclic"
        Me.MenuItemRingConstraintAcyclic.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintAcyclic.Text = "Acyclic"
        '
        'MenuItemRingConstraintAsymmetricIntransitive
        '
        Me.MenuItemRingConstraintAsymmetricIntransitive.Image = Global.Boston.My.Resources.ORMShapes.asymmetric_intransitive
        Me.MenuItemRingConstraintAsymmetricIntransitive.Name = "MenuItemRingConstraintAsymmetricIntransitive"
        Me.MenuItemRingConstraintAsymmetricIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintAsymmetricIntransitive.Text = "Asymmetric + Intransitive"
        '
        'MenuItemRingConstraintAcyclicIntransitive
        '
        Me.MenuItemRingConstraintAcyclicIntransitive.Image = Global.Boston.My.Resources.ORMShapes.acyclic_intransitive
        Me.MenuItemRingConstraintAcyclicIntransitive.Name = "MenuItemRingConstraintAcyclicIntransitive"
        Me.MenuItemRingConstraintAcyclicIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintAcyclicIntransitive.Text = "Acyclic + Intransitive"
        '
        'MenuItemRingConstraintSymmetric
        '
        Me.MenuItemRingConstraintSymmetric.Image = Global.Boston.My.Resources.ORMShapes.symmetric
        Me.MenuItemRingConstraintSymmetric.Name = "MenuItemRingConstraintSymmetric"
        Me.MenuItemRingConstraintSymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintSymmetric.Text = "Symmetric"
        '
        'MenuItemRingConstraintSymmetricIrreflexive
        '
        Me.MenuItemRingConstraintSymmetricIrreflexive.Image = Global.Boston.My.Resources.ORMShapes.symmetric_irreflexive
        Me.MenuItemRingConstraintSymmetricIrreflexive.Name = "MenuItemRingConstraintSymmetricIrreflexive"
        Me.MenuItemRingConstraintSymmetricIrreflexive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintSymmetricIrreflexive.Text = "Symmetric + Irreflexive"
        '
        'MenuItemRingConstraintSymmetricIntransitive
        '
        Me.MenuItemRingConstraintSymmetricIntransitive.Image = Global.Boston.My.Resources.ORMShapes.symmetric_intransitive
        Me.MenuItemRingConstraintSymmetricIntransitive.Name = "MenuItemRingConstraintSymmetricIntransitive"
        Me.MenuItemRingConstraintSymmetricIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintSymmetricIntransitive.Text = "Symmetric + Intransitive"
        '
        'MenuItemRingConstraintPurelyReflexive
        '
        Me.MenuItemRingConstraintPurelyReflexive.Image = Global.Boston.My.Resources.ORMShapes.purely_reflexive
        Me.MenuItemRingConstraintPurelyReflexive.Name = "MenuItemRingConstraintPurelyReflexive"
        Me.MenuItemRingConstraintPurelyReflexive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintPurelyReflexive.Text = "Purely Reflexive"
        '
        'MenuItemRingConstraintDeonticIrreflexive
        '
        Me.MenuItemRingConstraintDeonticIrreflexive.Image = Global.Boston.My.Resources.ORMShapes.deontic_irreflexive
        Me.MenuItemRingConstraintDeonticIrreflexive.Name = "MenuItemRingConstraintDeonticIrreflexive"
        Me.MenuItemRingConstraintDeonticIrreflexive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticIrreflexive.Text = "Deontic Irreflexive"
        '
        'MenuItemRingConstraintDeonticAsymmetric
        '
        Me.MenuItemRingConstraintDeonticAsymmetric.Image = Global.Boston.My.Resources.ORMShapes.deontic_asymmetric
        Me.MenuItemRingConstraintDeonticAsymmetric.Name = "MenuItemRingConstraintDeonticAsymmetric"
        Me.MenuItemRingConstraintDeonticAsymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticAsymmetric.Text = "Deontic Asymmetric"
        '
        'MenuItemRingConstraintDeonticIntransitive
        '
        Me.MenuItemRingConstraintDeonticIntransitive.Image = Global.Boston.My.Resources.ORMShapes.deontic_intransitive
        Me.MenuItemRingConstraintDeonticIntransitive.Name = "MenuItemRingConstraintDeonticIntransitive"
        Me.MenuItemRingConstraintDeonticIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticIntransitive.Text = "Deontic Intransitive"
        '
        'MenuItemRingConstraintDeonticAntisymmetric
        '
        Me.MenuItemRingConstraintDeonticAntisymmetric.Image = Global.Boston.My.Resources.ORMShapes.deontic_antisymmetric
        Me.MenuItemRingConstraintDeonticAntisymmetric.Name = "MenuItemRingConstraintDeonticAntisymmetric"
        Me.MenuItemRingConstraintDeonticAntisymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticAntisymmetric.Text = "Deontic Antisymmetric"
        '
        'MenuItemDeonticAcyclic
        '
        Me.MenuItemDeonticAcyclic.Image = Global.Boston.My.Resources.ORMShapes.deontic_acyclic
        Me.MenuItemDeonticAcyclic.Name = "MenuItemDeonticAcyclic"
        Me.MenuItemDeonticAcyclic.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemDeonticAcyclic.Text = "Deontic Acyclic"
        '
        'MenuItemRingConstraintDeonticAsymmetricIntransitive
        '
        Me.MenuItemRingConstraintDeonticAsymmetricIntransitive.Image = Global.Boston.My.Resources.ORMShapes.deontic_asymmetric_intransitive
        Me.MenuItemRingConstraintDeonticAsymmetricIntransitive.Name = "MenuItemRingConstraintDeonticAsymmetricIntransitive"
        Me.MenuItemRingConstraintDeonticAsymmetricIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticAsymmetricIntransitive.Text = "Deontic Asymmetric + Intransitive"
        '
        'MenuItemRingConstraintDeonticAcyclicIntransitive
        '
        Me.MenuItemRingConstraintDeonticAcyclicIntransitive.Image = Global.Boston.My.Resources.ORMShapes.deontic_acyclic_intransitive
        Me.MenuItemRingConstraintDeonticAcyclicIntransitive.Name = "MenuItemRingConstraintDeonticAcyclicIntransitive"
        Me.MenuItemRingConstraintDeonticAcyclicIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticAcyclicIntransitive.Text = "Deontic Acyclic + Intransitive"
        '
        'MenuItemRingConstraintDeonticSymmetric
        '
        Me.MenuItemRingConstraintDeonticSymmetric.Image = Global.Boston.My.Resources.ORMShapes.deontic_symmetric
        Me.MenuItemRingConstraintDeonticSymmetric.Name = "MenuItemRingConstraintDeonticSymmetric"
        Me.MenuItemRingConstraintDeonticSymmetric.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticSymmetric.Text = "Deontic Symmetric"
        '
        'MenuItemRingConstraintDeonticSymmetricIrreflexive
        '
        Me.MenuItemRingConstraintDeonticSymmetricIrreflexive.Image = Global.Boston.My.Resources.ORMShapes.deontic_symmetric_irreflexive
        Me.MenuItemRingConstraintDeonticSymmetricIrreflexive.Name = "MenuItemRingConstraintDeonticSymmetricIrreflexive"
        Me.MenuItemRingConstraintDeonticSymmetricIrreflexive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticSymmetricIrreflexive.Text = "Deontic Symmetric + Irreflexive"
        '
        'MenuItemRingConstraintDeonticSymmetricIntransitive
        '
        Me.MenuItemRingConstraintDeonticSymmetricIntransitive.Image = Global.Boston.My.Resources.ORMShapes.deontic_symmetric_intransitive
        Me.MenuItemRingConstraintDeonticSymmetricIntransitive.Name = "MenuItemRingConstraintDeonticSymmetricIntransitive"
        Me.MenuItemRingConstraintDeonticSymmetricIntransitive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticSymmetricIntransitive.Text = "Deontic Symmetric + Intransitive"
        '
        'MenuItemRingConstraintDeonticPurelyReflexive
        '
        Me.MenuItemRingConstraintDeonticPurelyReflexive.Image = Global.Boston.My.Resources.ORMShapes.deontic_purely_reflexive
        Me.MenuItemRingConstraintDeonticPurelyReflexive.Name = "MenuItemRingConstraintDeonticPurelyReflexive"
        Me.MenuItemRingConstraintDeonticPurelyReflexive.Size = New System.Drawing.Size(254, 22)
        Me.MenuItemRingConstraintDeonticPurelyReflexive.Text = "Deontic Purely Reflexive"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(222, 6)
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem9.Text = "&Remove from Page"
        '
        'ToolStripMenuItem13
        '
        Me.ToolStripMenuItem13.Name = "ToolStripMenuItem13"
        Me.ToolStripMenuItem13.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem13.Text = "Remove from Page && &Model"
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(890, 524)
        '
        'ComboBoxFact
        '
        Me.ComboBoxFact.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFact.FormattingEnabled = True
        Me.ComboBoxFact.Items.AddRange(New Object() {"", "Hello", "World"})
        Me.ComboBoxFact.Location = New System.Drawing.Point(208, 0)
        Me.ComboBoxFact.Name = "ComboBoxFact"
        Me.ComboBoxFact.Size = New System.Drawing.Size(178, 21)
        Me.ComboBoxFact.TabIndex = 10
        Me.ComboBoxFact.Visible = False
        '
        'LabelHelp
        '
        Me.LabelHelp.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelp.ContextMenuStrip = Me.ContextMenuStrip_HelpTips
        Me.LabelHelp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelp.Location = New System.Drawing.Point(0, 466)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(890, 83)
        Me.LabelHelp.TabIndex = 11
        '
        'ContextMenuStrip_HelpTips
        '
        Me.ContextMenuStrip_HelpTips.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_HelpTips.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideToolStripMenuItem1})
        Me.ContextMenuStrip_HelpTips.Name = "ContextMenuStrip_HelpTips"
        Me.ContextMenuStrip_HelpTips.Size = New System.Drawing.Size(100, 26)
        '
        'HideToolStripMenuItem1
        '
        Me.HideToolStripMenuItem1.Name = "HideToolStripMenuItem1"
        Me.HideToolStripMenuItem1.Size = New System.Drawing.Size(99, 22)
        Me.HideToolStripMenuItem1.Text = "&Hide"
        '
        'ComboBoxEntityType
        '
        Me.ComboBoxEntityType.FormattingEnabled = True
        Me.ComboBoxEntityType.Location = New System.Drawing.Point(415, 0)
        Me.ComboBoxEntityType.Name = "ComboBoxEntityType"
        Me.ComboBoxEntityType.Size = New System.Drawing.Size(176, 21)
        Me.ComboBoxEntityType.TabIndex = 12
        Me.ComboBoxEntityType.Visible = False
        '
        'ComboBoxValueType
        '
        Me.ComboBoxValueType.FormattingEnabled = True
        Me.ComboBoxValueType.Location = New System.Drawing.Point(0, 0)
        Me.ComboBoxValueType.Name = "ComboBoxValueType"
        Me.ComboBoxValueType.Size = New System.Drawing.Size(178, 21)
        Me.ComboBoxValueType.TabIndex = 13
        Me.ComboBoxValueType.Visible = False
        '
        'ContextMenuStrip_SubtypeRelationship
        '
        Me.ContextMenuStrip_SubtypeRelationship.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemSubtypeShowCorrespondingFactType, Me.HideCorrespondingFactTypeToolStripMenuItem, Me.ToolStripSeparator29, Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem, Me.ToolStripSeparator36, Me.PropertiesToolStripMenuItem4})
        Me.ContextMenuStrip_SubtypeRelationship.Name = "ContextMenuStrip_SubtypeRelationship"
        Me.ContextMenuStrip_SubtypeRelationship.Size = New System.Drawing.Size(236, 104)
        '
        'ToolStripMenuItemSubtypeShowCorrespondingFactType
        '
        Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Name = "ToolStripMenuItemSubtypeShowCorrespondingFactType"
        Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Size = New System.Drawing.Size(235, 22)
        Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Text = "&Show corresponding Fact Type"
        Me.ToolStripMenuItemSubtypeShowCorrespondingFactType.Visible = False
        '
        'HideCorrespondingFactTypeToolStripMenuItem
        '
        Me.HideCorrespondingFactTypeToolStripMenuItem.Name = "HideCorrespondingFactTypeToolStripMenuItem"
        Me.HideCorrespondingFactTypeToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.HideCorrespondingFactTypeToolStripMenuItem.Text = "&Hide corresponding Fact Type"
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(232, 6)
        '
        'RemoveSubtypeRelationshipFromTheModelToolStripMenuItem
        '
        Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem.Name = "RemoveSubtypeRelationshipFromTheModelToolStripMenuItem"
        Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveSubtypeRelationshipFromTheModelToolStripMenuItem.Text = "&Remove from Page && Model"
        '
        'ToolStripSeparator36
        '
        Me.ToolStripSeparator36.Name = "ToolStripSeparator36"
        Me.ToolStripSeparator36.Size = New System.Drawing.Size(232, 6)
        '
        'PropertiesToolStripMenuItem4
        '
        Me.PropertiesToolStripMenuItem4.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem4.Name = "PropertiesToolStripMenuItem4"
        Me.PropertiesToolStripMenuItem4.Size = New System.Drawing.Size(235, 22)
        Me.PropertiesToolStripMenuItem4.Text = "&Properties"
        '
        'ContextMenuStrip_FrequencyConstraint
        '
        Me.ContextMenuStrip_FrequencyConstraint.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_FrequencyConstraint.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemModelErrorsFrequencyConstraint, Me.ToolStripSeparator26, Me.ReoveFromPageToolStripMenuItem, Me.RemoveFromPageAndModelToolStripMenuItem})
        Me.ContextMenuStrip_FrequencyConstraint.Name = "ContextMenuStrip_FrequencyConstraint"
        Me.ContextMenuStrip_FrequencyConstraint.Size = New System.Drawing.Size(236, 76)
        '
        'ToolStripMenuItemModelErrorsFrequencyConstraint
        '
        Me.ToolStripMenuItemModelErrorsFrequencyConstraint.Name = "ToolStripMenuItemModelErrorsFrequencyConstraint"
        Me.ToolStripMenuItemModelErrorsFrequencyConstraint.Size = New System.Drawing.Size(235, 22)
        Me.ToolStripMenuItemModelErrorsFrequencyConstraint.Text = "Model &Errors"
        '
        'ToolStripSeparator26
        '
        Me.ToolStripSeparator26.Name = "ToolStripSeparator26"
        Me.ToolStripSeparator26.Size = New System.Drawing.Size(232, 6)
        '
        'ReoveFromPageToolStripMenuItem
        '
        Me.ReoveFromPageToolStripMenuItem.Name = "ReoveFromPageToolStripMenuItem"
        Me.ReoveFromPageToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ReoveFromPageToolStripMenuItem.Text = "Remove from &Page"
        '
        'RemoveFromPageAndModelToolStripMenuItem
        '
        Me.RemoveFromPageAndModelToolStripMenuItem.Name = "RemoveFromPageAndModelToolStripMenuItem"
        Me.RemoveFromPageAndModelToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.RemoveFromPageAndModelToolStripMenuItem.Text = "&Remove from Page and Model"
        '
        'ContextMenuStrip_InternalUniquenessConstraint
        '
        Me.ContextMenuStrip_InternalUniquenessConstraint.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStrip_InternalUniquenessConstraint.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowInModelDictionaryToolStripMenuItem4, Me.ToolStripSeparator38, Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType, Me.RemoveFromPageModelToolStripMenuItem1, Me.ToolStripSeparator39, Me.PropertiesToolStripMenuItem3})
        Me.ContextMenuStrip_InternalUniquenessConstraint.Name = "ContextMenuStrip_InternalRoleConstraint"
        Me.ContextMenuStrip_InternalUniquenessConstraint.Size = New System.Drawing.Size(296, 136)
        '
        'ShowInModelDictionaryToolStripMenuItem4
        '
        Me.ShowInModelDictionaryToolStripMenuItem4.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem4.Name = "ShowInModelDictionaryToolStripMenuItem4"
        Me.ShowInModelDictionaryToolStripMenuItem4.Size = New System.Drawing.Size(295, 30)
        Me.ShowInModelDictionaryToolStripMenuItem4.Text = "Show in Model &Dictionary"
        '
        'ToolStripSeparator38
        '
        Me.ToolStripSeparator38.Name = "ToolStripSeparator38"
        Me.ToolStripSeparator38.Size = New System.Drawing.Size(292, 6)
        '
        'ToolStripMenuItemExtendToCoverAllRolesInTheFactType
        '
        Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Name = "ToolStripMenuItemExtendToCoverAllRolesInTheFactType"
        Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Size = New System.Drawing.Size(295, 30)
        Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Text = "&Extend to cover all Roles in the Fact Type"
        Me.ToolStripMenuItemExtendToCoverAllRolesInTheFactType.Visible = False
        '
        'RemoveFromPageModelToolStripMenuItem1
        '
        Me.RemoveFromPageModelToolStripMenuItem1.Name = "RemoveFromPageModelToolStripMenuItem1"
        Me.RemoveFromPageModelToolStripMenuItem1.Size = New System.Drawing.Size(295, 30)
        Me.RemoveFromPageModelToolStripMenuItem1.Text = "&Remove from Page && Model"
        '
        'ToolStripSeparator39
        '
        Me.ToolStripSeparator39.Name = "ToolStripSeparator39"
        Me.ToolStripSeparator39.Size = New System.Drawing.Size(292, 6)
        '
        'PropertiesToolStripMenuItem3
        '
        Me.PropertiesToolStripMenuItem3.Image = Global.Boston.My.Resources.Resources.Properties216x16
        Me.PropertiesToolStripMenuItem3.Name = "PropertiesToolStripMenuItem3"
        Me.PropertiesToolStripMenuItem3.Size = New System.Drawing.Size(295, 30)
        Me.PropertiesToolStripMenuItem3.Text = "&Properties"
        '
        'Animator1
        '
        Me.Animator1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Animator1.ContextMenuStrip = Me.ContextMenuStripVirtualAnalyst
        Me.Animator1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Animator1.Location = New System.Drawing.Point(0, 424)
        Me.Animator1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Animator1.Name = "Animator1"
        Me.Animator1.Size = New System.Drawing.Size(110, 125)
        Me.Animator1.TabIndex = 14
        Me.Animator1.Visible = False
        '
        'ContextMenuStripVirtualAnalyst
        '
        Me.ContextMenuStripVirtualAnalyst.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripVirtualAnalyst.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideMeToolStripMenuItem})
        Me.ContextMenuStripVirtualAnalyst.Name = "ContextMenuStripVirtualAnalyst"
        Me.ContextMenuStripVirtualAnalyst.Size = New System.Drawing.Size(120, 26)
        '
        'HideMeToolStripMenuItem
        '
        Me.HideMeToolStripMenuItem.Name = "HideMeToolStripMenuItem"
        Me.HideMeToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.HideMeToolStripMenuItem.Text = "&Hide me"
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
        Me.CircularProgressBar.Location = New System.Drawing.Point(402, 231)
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
        Me.CircularProgressBar.TabIndex = 15
        Me.CircularProgressBar.Text = "0"
        Me.CircularProgressBar.TextMargin = New System.Windows.Forms.Padding(2, 2, 0, 0)
        Me.CircularProgressBar.Value = 68
        '
        'BackgroundWorker
        '
        Me.BackgroundWorker.WorkerReportsProgress = True
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
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripMenuItemFactTypeModelErrors
        '
        Me.ToolStripMenuItemFactTypeModelErrors.Name = "ToolStripMenuItemFactTypeModelErrors"
        Me.ToolStripMenuItemFactTypeModelErrors.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItemFactTypeModelErrors.Text = "Model &Errors"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripSeparator35
        '
        Me.ToolStripSeparator35.Name = "ToolStripSeparator35"
        Me.ToolStripSeparator35.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripMenuItemFactTypeInstanceRemoveFromPage
        '
        Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage.Name = "ToolStripMenuItemFactTypeInstanceRemoveFromPage"
        Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage.Text = "&Remove from Page"
        '
        'ToolStripMenuItemFactTypeRemoveFromPageModel
        '
        Me.ToolStripMenuItemFactTypeRemoveFromPageModel.Name = "ToolStripMenuItemFactTypeRemoveFromPageModel"
        Me.ToolStripMenuItemFactTypeRemoveFromPageModel.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItemFactTypeRemoveFromPageModel.Text = "Remove &from Page && Model"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(307, 6)
        '
        'ContextMenuStrip_FactType
        '
        Me.ContextMenuStrip_FactType.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphToToolStripMenuItem, Me.ToolStripSeparator11, Me.ToolStripMenuItemFactTypeModelErrors, Me.ToolStripSeparator22, Me.mnuOption_IsObjectified, Me.ToolStripSeparator41, Me.ToolStripMenuItem14, Me.ToolStripSeparator42, Me.ToolStripMenuItemViewFactTable, Me.ToolStripMenuItemAddRole, Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem, Me.ToolStripSeparator4, Me.ToolStripMenuItem1, Me.ToolStripSeparator10, Me.ShowInModelDictionaryToolStripMenuItem2, Me.ToolStripSeparator35, Me.ToolStripMenuItemFactTypeInstanceRemoveFromPage, Me.ToolStripMenuItemFactTypeRemoveFromPageModel, Me.ToolStripSeparator17, Me.PropertieToolStripMenuItem, Me.AddAllAssociatedFactTypesToPageToolStripMenuItem1})
        Me.ContextMenuStrip_FactType.Name = "ContextMenuStrip_FactType"
        Me.ContextMenuStrip_FactType.Size = New System.Drawing.Size(311, 360)
        '
        'MorphToToolStripMenuItem
        '
        Me.MorphToToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ORMFromFactTypeToolStripMenuItem, Me.ERDiagramFromFactTypeToolStripMenuItem, Me.PGSDiagramToolStripMenuItem})
        Me.MorphToToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.Morphing16x16
        Me.MorphToToolStripMenuItem.Name = "MorphToToolStripMenuItem"
        Me.MorphToToolStripMenuItem.Size = New System.Drawing.Size(310, 22)
        Me.MorphToToolStripMenuItem.Text = "&Morph To"
        '
        'ORMFromFactTypeToolStripMenuItem
        '
        Me.ORMFromFactTypeToolStripMenuItem.Image = CType(resources.GetObject("ORMFromFactTypeToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ORMFromFactTypeToolStripMenuItem.Name = "ORMFromFactTypeToolStripMenuItem"
        Me.ORMFromFactTypeToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ORMFromFactTypeToolStripMenuItem.Text = "&ORM Diagram"
        '
        'ERDiagramFromFactTypeToolStripMenuItem
        '
        Me.ERDiagramFromFactTypeToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.ERD16x16
        Me.ERDiagramFromFactTypeToolStripMenuItem.Name = "ERDiagramFromFactTypeToolStripMenuItem"
        Me.ERDiagramFromFactTypeToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.ERDiagramFromFactTypeToolStripMenuItem.Text = "&ER Diagram"
        '
        'PGSDiagramToolStripMenuItem
        '
        Me.PGSDiagramToolStripMenuItem.Image = Global.Boston.My.Resources.Resources.PGS16x16
        Me.PGSDiagramToolStripMenuItem.Name = "PGSDiagramToolStripMenuItem"
        Me.PGSDiagramToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.PGSDiagramToolStripMenuItem.Text = "PGS Diagram"
        '
        'mnuOption_IsObjectified
        '
        Me.mnuOption_IsObjectified.Image = Global.Boston.My.Resources.MenuImages.FactTypeObjectified16x16
        Me.mnuOption_IsObjectified.Name = "mnuOption_IsObjectified"
        Me.mnuOption_IsObjectified.Size = New System.Drawing.Size(310, 22)
        Me.mnuOption_IsObjectified.Text = "&Is Objectified"
        '
        'ToolStripMenuItemViewFactTable
        '
        Me.ToolStripMenuItemViewFactTable.Image = Global.Boston.My.Resources.MenuImages.FactTables16x16
        Me.ToolStripMenuItemViewFactTable.Name = "ToolStripMenuItemViewFactTable"
        Me.ToolStripMenuItemViewFactTable.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItemViewFactTable.Text = "View &Fact Table"
        '
        'ToolStripMenuItemAddRole
        '
        Me.ToolStripMenuItemAddRole.Name = "ToolStripMenuItemAddRole"
        Me.ToolStripMenuItemAddRole.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItemAddRole.Text = "&Add Role"
        '
        'RemoveallInternalUniquenessConstraintsToolStripMenuItem
        '
        Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.FactTypeRemoveUniquenessConstraints16x16
        Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem.Name = "RemoveallInternalUniquenessConstraintsToolStripMenuItem"
        Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem.Size = New System.Drawing.Size(310, 22)
        Me.RemoveallInternalUniquenessConstraintsToolStripMenuItem.Text = "&Remove [all] Internal Uniqueness Constraints"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Image = Global.Boston.My.Resources.MenuImages.FactTypeReading16x16
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItem1.Text = "&View Reading Editor"
        '
        'ShowInModelDictionaryToolStripMenuItem2
        '
        Me.ShowInModelDictionaryToolStripMenuItem2.Image = Global.Boston.My.Resources.Resources.dictionary16x16
        Me.ShowInModelDictionaryToolStripMenuItem2.Name = "ShowInModelDictionaryToolStripMenuItem2"
        Me.ShowInModelDictionaryToolStripMenuItem2.Size = New System.Drawing.Size(310, 22)
        Me.ShowInModelDictionaryToolStripMenuItem2.Text = "Show in Model &Dictionary"
        '
        'PropertieToolStripMenuItem
        '
        Me.PropertieToolStripMenuItem.Image = Global.Boston.My.Resources.MenuImages.Properties216x16
        Me.PropertieToolStripMenuItem.Name = "PropertieToolStripMenuItem"
        Me.PropertieToolStripMenuItem.Size = New System.Drawing.Size(310, 22)
        Me.PropertieToolStripMenuItem.Text = "&Properties"
        '
        'AddAllAssociatedFactTypesToPageToolStripMenuItem1
        '
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem1.Name = "AddAllAssociatedFactTypesToPageToolStripMenuItem1"
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem1.Size = New System.Drawing.Size(310, 22)
        Me.AddAllAssociatedFactTypesToPageToolStripMenuItem1.Text = "&Add all associated Fact Types to Page"
        '
        'ContextMenuStripModelNote
        '
        Me.ContextMenuStripModelNote.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuStripModelNote.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemModelNoteRemoveFromPageAndModel})
        Me.ContextMenuStripModelNote.Name = "ContextMenuStripModelNote"
        Me.ContextMenuStripModelNote.Size = New System.Drawing.Size(226, 26)
        '
        'ToolStripMenuItemModelNoteRemoveFromPageAndModel
        '
        Me.ToolStripMenuItemModelNoteRemoveFromPageAndModel.Name = "ToolStripMenuItemModelNoteRemoveFromPageAndModel"
        Me.ToolStripMenuItemModelNoteRemoveFromPageAndModel.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItemModelNoteRemoveFromPageAndModel.Text = "&Remove from Page && Model"
        '
        'DiagramView
        '
        Me.DiagramView.AllowDrop = True
        Me.DiagramView.AllowInplaceEdit = True
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.DrawLinks
        Me.DiagramView.ContextMenuStrip = Me.ContextMenuStrip_Diagram
        Me.DiagramView.ControlHandlesStyle = MindFusion.Diagramming.HandlesStyle.HatchHandles
        Me.DiagramView.ControlMouseAction = MindFusion.Diagramming.ControlMouseAction.SelectNode
        Me.DiagramView.DelKeyAction = MindFusion.Diagramming.DelKeyAction.DeleteSelectedItems
        Me.DiagramView.Diagram = Me.Diagram
        Me.DiagramView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DiagramView.InplaceEditCancelOnEsc = False
        Me.DiagramView.Location = New System.Drawing.Point(0, 0)
        Me.DiagramView.MiddleButtonActions = MindFusion.Diagramming.MouseButtonActions.Pan
        Me.DiagramView.ModificationStart = MindFusion.Diagramming.ModificationStart.SelectedOnly
        Me.DiagramView.MoveCursor = System.Windows.Forms.Cursors.Hand
        Me.DiagramView.Name = "DiagramView"
        Me.DiagramView.RightButtonActions = MindFusion.Diagramming.MouseButtonActions.Cancel
        Me.DiagramView.Size = New System.Drawing.Size(890, 549)
        Me.DiagramView.TabIndex = 8
        '
        'HiddenDiagramView
        '
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
        Me.HiddenDiagramView.Size = New System.Drawing.Size(890, 549)
        Me.HiddenDiagramView.TabIndex = 9
        Me.HiddenDiagramView.Text = "DiagramView1"
        '
        'ToolStripSeparator41
        '
        Me.ToolStripSeparator41.Name = "ToolStripSeparator41"
        Me.ToolStripSeparator41.Size = New System.Drawing.Size(307, 6)
        '
        'ToolStripMenuItem14
        '
        Me.ToolStripMenuItem14.Image = Global.Boston.My.Resources.Resources.Spyglass16x16
        Me.ToolStripMenuItem14.Name = "ToolStripMenuItem14"
        Me.ToolStripMenuItem14.Size = New System.Drawing.Size(310, 22)
        Me.ToolStripMenuItem14.Text = "Show in Diagram &Spy"
        '
        'ToolStripSeparator42
        '
        Me.ToolStripSeparator42.Name = "ToolStripSeparator42"
        Me.ToolStripSeparator42.Size = New System.Drawing.Size(307, 6)
        '
        'frmDiagramORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(890, 549)
        Me.Controls.Add(Me.LabelHelp)
        Me.Controls.Add(Me.ComboBoxFact)
        Me.Controls.Add(Me.ComboBoxEntityType)
        Me.Controls.Add(Me.ComboBoxValueType)
        Me.Controls.Add(Me.Animator1)
        Me.Controls.Add(Me.DiagramView)
        Me.Controls.Add(Me.HiddenDiagramView)
        Me.Controls.Add(Me.CircularProgressBar)
        Me.HelpProvider.SetHelpKeyword(Me, "")
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmDiagramORM"
        Me.HelpProvider.SetShowHelp(Me, True)
        Me.TabPageContextMenuStrip = Me.ContextMenuStripTab
        Me.TabText = "ORM Model"
        Me.Text = "ORM Model"
        Me.ContextMenuStrip_Diagram.ResumeLayout(False)
        Me.ContextMenuStrip_shape_list.ResumeLayout(False)
        Me.ContextMenuStrip_Role.ResumeLayout(False)
        Me.ContextMenuStrip_EntityType.ResumeLayout(False)
        Me.ContextMenuStrip_ExternalRoleConstraint.ResumeLayout(False)
        Me.ContextMenuStrip_FactTable.ResumeLayout(False)
        Me.ContextMenuStrip_ValueType.ResumeLayout(False)
        Me.ContextMenuStrip_RingConstraint.ResumeLayout(False)
        Me.ContextMenuStrip_HelpTips.ResumeLayout(False)
        Me.ContextMenuStrip_SubtypeRelationship.ResumeLayout(False)
        Me.ContextMenuStrip_FrequencyConstraint.ResumeLayout(False)
        Me.ContextMenuStrip_InternalUniquenessConstraint.ResumeLayout(False)
        Me.ContextMenuStripVirtualAnalyst.ResumeLayout(False)
        Me.ContextMenuStripTab.ResumeLayout(False)
        Me.ContextMenuStrip_FactType.ResumeLayout(False)
        Me.ContextMenuStripModelNote.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_AddEntityType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RoleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DomainToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddDomainToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditDomainToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteDomainToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UndoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_OverviewTool As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SubjectAreaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintPreviewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ExportToSVGToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToDXFToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportVisioDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToXMLFieToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Diagram As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuStrip_shape_list As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents DockingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_DockRight As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_DockLeft As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_DockTop As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Role As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOption_ViewReadingEditor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_Mandatory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ContextMenuStrip_EntityType As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuOption_EntityTypeMorphTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_EntityTypeProperties As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseCToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutoLayoutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_AddUniquenessConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_CopyImageToClipboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ExternalRoleConstraint As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ChangeToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeonticToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeToToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToUniquenessConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToUniquenessPreferredConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToValueComparison As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GreaterThanOREqualToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeonticGreaterThanToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeonticGreaterThanOREqualToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LessThanToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LessThanOREqualToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeonticLessThanToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeonticLessThanOREqualToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticUniqueness As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticPreferredUniquenessConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToExclusiveConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToInclusiveOrConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToEqualityConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticExclusiveConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticInclusiveOrConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticEqualityConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToExclusiveOrConstranit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemChangeToDeonticExclusiveOrConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ErrorListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMVerbalisationViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RichmondBrainBoxToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_FactTable As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemAddFact As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ResizeToFitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_Toolbox As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HiddenDiagram As MindFusion.Diagramming.Diagram
    Friend WithEvents MorphTimer As System.Windows.Forms.Timer
    Friend WithEvents MorphStepTimer As System.Windows.Forms.Timer
    Friend WithEvents ORMDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_ValueType As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ValueTypeMorph_ORMDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemStateTransitionDiagram As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ERDiagramToolStripMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuOptionDataFlowDiagramEntityType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LockToThisPositionOnPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider As System.Windows.Forms.HelpProvider
    Friend WithEvents ContextMenuStrip_RingConstraint As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem24 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintIrreflexiveToolStrip As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConsgtraintAsymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRnigConstraintIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintAntisymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintAcyclic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintAsymmetricIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintAcyclicIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintSymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintSymmetricIrreflexive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintSymmetricIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintPurelyReflexive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticIrreflexive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticAsymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticAntisymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemDeonticAcyclic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticAsymmetricIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticAcyclicIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticSymmetric As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticSymmetricIrreflexive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticSymmetricIntransitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemRingConstraintDeonticPurelyReflexive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromPageModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem11 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem12 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteRowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowHideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemHelpTips As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOption_ViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FacToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSubtypeConstraints As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRoleConstraints As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FactTypesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRoleNames As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFactTypeReadings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFactTypeNames As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewFactTablesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImportFactFromModelLevelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UMLClassDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteRowFactFromPageAndModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HiddenDiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents ComboBoxFact As System.Windows.Forms.ComboBox
    Friend WithEvents LabelHelp As System.Windows.Forms.Label
    Friend WithEvents ComboBoxEntityType As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxValueType As System.Windows.Forms.ComboBox
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents ContextMenuStrip_SubtypeRelationship As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveSubtypeRelationshipFromTheModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_FrequencyConstraint As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveFromPageAndModelToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReoveFromPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_InternalUniquenessConstraint As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveFromPageModelToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RemoveFromPageToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromPageModelToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem13 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19Convert As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemConvert As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LanguageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PropertyGraphSchemaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemValueTypeModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemEntityTypeModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemFactModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator21 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SetNameFromHostingObjectTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator23 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Animator1 As Viev.Animator.Animator
    Friend WithEvents ContextMenuStripVirtualAnalyst As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HideMeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EntityRelationshipDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideFactTypeNamesRoleNamesFactTypeReadingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveLinksToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator24 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RemovefromPageAndModelToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator25 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemModelErrorsFrequencyConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator26 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemModelErrorsExternalRoleConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator27 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemModelErrorsRingConstraint As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator28 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemRemoveArgument As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem17 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CircularProgressBar As CircularProgressBar.CircularProgressBar
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents RedoJoinPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSubtypeShowCorrespondingFactType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_HelpTips As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HideToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemRemoveAllArguments As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator29 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowInDiagramSpyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator30 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemPropertyGraphSchema As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator31 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HideCorrespondingFactTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExpandTheReferenceSchemeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HideTheReferenceSchemeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator32 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator33 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator37 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator38 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator39 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemExtendToCoverAllRolesInTheFactType As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStripTab As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CloseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllButThisPageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphToToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ORMFromFactTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ERDiagramFromFactTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PGSDiagramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemFactTypeModelErrors As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator22 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuOption_IsObjectified As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemViewFactTable As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveallInternalUniquenessConstraintsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowInModelDictionaryToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator35 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItemFactTypeInstanceRemoveFromPage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemFactTypeRemoveFromPageModel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PropertieToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_FactType As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ContextMenuStripModelNote As ContextMenuStrip
    Friend WithEvents ToolStripMenuItemModelNoteRemoveFromPageAndModel As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemPaste As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator34 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemCopy As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator40 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItemShowLinkFactType As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddRole As ToolStripMenuItem
    Friend WithEvents Diagram1 As MindFusion.Diagramming.Diagram
    Friend WithEvents Diagram2 As MindFusion.Diagramming.Diagram
    Friend WithEvents AddAllAssociatedFactTypesToPageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddAllAssociatedFactTypesToPageToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator36 As ToolStripSeparator
    Friend WithEvents PropertiesToolStripMenuItem4 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator41 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem14 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator42 As ToolStripSeparator
End Class
