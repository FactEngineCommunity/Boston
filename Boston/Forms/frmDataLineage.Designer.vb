<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDataLineage
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDataLineage))
        Me.GroupBoxCategories = New System.Windows.Forms.GroupBox()
        Me.LabelPromtLineageItem = New System.Windows.Forms.Label()
        Me.LabelLineageItem = New System.Windows.Forms.Label()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.BindingNavigatorLineageProperty = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorAddNewItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingSourceLineageProperty = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ButtonOpenDocument = New System.Windows.Forms.Button()
        Me.GroupBoxNavigator = New System.Windows.Forms.GroupBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonClose = New System.Windows.Forms.ToolStripButton()
        Me.Diagram = New MindFusion.Diagramming.Diagram()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.DiagramView = New MindFusion.Diagramming.WinForms.DiagramView()
        Me.ContextMenuStripDocuments = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ClearFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ContextMenuStripDocument = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FilterByThisDocumentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.BindingNavigatorLineageProperty, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorLineageProperty.SuspendLayout()
        CType(Me.BindingSourceLineageProperty, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxNavigator.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.ContextMenuStripDocuments.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStripDocument.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxCategories
        '
        Me.GroupBoxCategories.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBoxCategories.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxCategories.ForeColor = System.Drawing.Color.Gray
        Me.GroupBoxCategories.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxCategories.Name = "GroupBoxCategories"
        Me.GroupBoxCategories.Size = New System.Drawing.Size(498, 310)
        Me.GroupBoxCategories.TabIndex = 0
        Me.GroupBoxCategories.TabStop = False
        Me.GroupBoxCategories.Text = "Lineage Categories:"
        '
        'LabelPromtLineageItem
        '
        Me.LabelPromtLineageItem.AutoSize = True
        Me.LabelPromtLineageItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromtLineageItem.ForeColor = System.Drawing.Color.Gray
        Me.LabelPromtLineageItem.Location = New System.Drawing.Point(12, 30)
        Me.LabelPromtLineageItem.Name = "LabelPromtLineageItem"
        Me.LabelPromtLineageItem.Size = New System.Drawing.Size(88, 16)
        Me.LabelPromtLineageItem.TabIndex = 1
        Me.LabelPromtLineageItem.Text = "Lineage Item:"
        '
        'LabelLineageItem
        '
        Me.LabelLineageItem.AutoSize = True
        Me.LabelLineageItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelLineageItem.Location = New System.Drawing.Point(106, 24)
        Me.LabelLineageItem.Name = "LabelLineageItem"
        Me.LabelLineageItem.Size = New System.Drawing.Size(100, 24)
        Me.LabelLineageItem.TabIndex = 2
        Me.LabelLineageItem.Text = "LabelItem"
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(15, 67)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 3
        Me.ButtonSave.Text = "&Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(96, 67)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 4
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'BindingNavigatorLineageProperty
        '
        Me.BindingNavigatorLineageProperty.AddNewItem = Me.BindingNavigatorAddNewItem
        Me.BindingNavigatorLineageProperty.BindingSource = Me.BindingSourceLineageProperty
        Me.BindingNavigatorLineageProperty.CountItem = Me.BindingNavigatorCountItem
        Me.BindingNavigatorLineageProperty.DeleteItem = Me.BindingNavigatorDeleteItem
        Me.BindingNavigatorLineageProperty.Dock = System.Windows.Forms.DockStyle.None
        Me.BindingNavigatorLineageProperty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorAddNewItem, Me.BindingNavigatorDeleteItem})
        Me.BindingNavigatorLineageProperty.Location = New System.Drawing.Point(3, 9)
        Me.BindingNavigatorLineageProperty.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.BindingNavigatorLineageProperty.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.BindingNavigatorLineageProperty.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.BindingNavigatorLineageProperty.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.BindingNavigatorLineageProperty.Name = "BindingNavigatorLineageProperty"
        Me.BindingNavigatorLineageProperty.PositionItem = Me.BindingNavigatorPositionItem
        Me.BindingNavigatorLineageProperty.Size = New System.Drawing.Size(255, 25)
        Me.BindingNavigatorLineageProperty.TabIndex = 5
        Me.BindingNavigatorLineageProperty.Text = "BindingNavigator1"
        '
        'BindingNavigatorAddNewItem
        '
        Me.BindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorAddNewItem.Image = Global.Boston.My.Resources.Resources.Add16x16
        Me.BindingNavigatorAddNewItem.Name = "BindingNavigatorAddNewItem"
        Me.BindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorAddNewItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorAddNewItem.Text = "Add new"
        '
        'BindingSourceLineageProperty
        '
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Delete"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 23)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'ButtonOpenDocument
        '
        Me.ButtonOpenDocument.Location = New System.Drawing.Point(481, 67)
        Me.ButtonOpenDocument.Name = "ButtonOpenDocument"
        Me.ButtonOpenDocument.Size = New System.Drawing.Size(99, 23)
        Me.ButtonOpenDocument.TabIndex = 6
        Me.ButtonOpenDocument.Text = "&Open Document"
        Me.ButtonOpenDocument.UseVisualStyleBackColor = True
        '
        'GroupBoxNavigator
        '
        Me.GroupBoxNavigator.Controls.Add(Me.BindingNavigatorLineageProperty)
        Me.GroupBoxNavigator.Location = New System.Drawing.Point(207, 61)
        Me.GroupBoxNavigator.Name = "GroupBoxNavigator"
        Me.GroupBoxNavigator.Size = New System.Drawing.Size(268, 36)
        Me.GroupBoxNavigator.TabIndex = 7
        Me.GroupBoxNavigator.TabStop = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonClose})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(741, 25)
        Me.ToolStrip1.TabIndex = 8
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonClose
        '
        Me.ToolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButtonClose.Image = CType(resources.GetObject("ToolStripButtonClose.Image"), System.Drawing.Image)
        Me.ToolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonClose.Name = "ToolStripButtonClose"
        Me.ToolStripButtonClose.Size = New System.Drawing.Size(40, 22)
        Me.ToolStripButtonClose.Text = "&Close"
        '
        'Diagram
        '
        Me.Diagram.BackBrush = New MindFusion.Drawing.SolidBrush("#FFFFFFFF")
        '
        'SplitContainer
        '
        Me.SplitContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer.Location = New System.Drawing.Point(15, 118)
        Me.SplitContainer.Name = "SplitContainer"
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.DiagramView)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.GroupBoxCategories)
        Me.SplitContainer.Size = New System.Drawing.Size(715, 310)
        Me.SplitContainer.SplitterDistance = 213
        Me.SplitContainer.TabIndex = 10
        '
        'DiagramView
        '
        Me.DiagramView.Behavior = MindFusion.Diagramming.Behavior.LinkShapes
        Me.DiagramView.ContextMenuStrip = Me.ContextMenuStripDocuments
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
        Me.DiagramView.Size = New System.Drawing.Size(213, 310)
        Me.DiagramView.TabIndex = 0
        Me.DiagramView.Text = "DiagramView1"
        '
        'ContextMenuStripDocuments
        '
        Me.ContextMenuStripDocuments.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearFilterToolStripMenuItem})
        Me.ContextMenuStripDocuments.Name = "ContextMenuStripDocuments"
        Me.ContextMenuStripDocuments.Size = New System.Drawing.Size(129, 26)
        '
        'ClearFilterToolStripMenuItem
        '
        Me.ClearFilterToolStripMenuItem.Name = "ClearFilterToolStripMenuItem"
        Me.ClearFilterToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ClearFilterToolStripMenuItem.Text = "&Clear filter"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Location = New System.Drawing.Point(15, 103)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Size = New System.Drawing.Size(793, 130)
        Me.SplitContainer1.SplitterDistance = 264
        Me.SplitContainer1.TabIndex = 10
        '
        'ContextMenuStripDocument
        '
        Me.ContextMenuStripDocument.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FilterByThisDocumentToolStripMenuItem})
        Me.ContextMenuStripDocument.Name = "ContextMenuStripDocument"
        Me.ContextMenuStripDocument.Size = New System.Drawing.Size(197, 26)
        '
        'FilterByThisDocumentToolStripMenuItem
        '
        Me.FilterByThisDocumentToolStripMenuItem.Name = "FilterByThisDocumentToolStripMenuItem"
        Me.FilterByThisDocumentToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.FilterByThisDocumentToolStripMenuItem.Text = "&Filter by this document"
        '
        'frmDataLineage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(741, 439)
        Me.Controls.Add(Me.SplitContainer)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.GroupBoxNavigator)
        Me.Controls.Add(Me.ButtonOpenDocument)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.LabelLineageItem)
        Me.Controls.Add(Me.LabelPromtLineageItem)
        Me.Name = "frmDataLineage"
        Me.Text = "Metadata Lineage"
        CType(Me.BindingNavigatorLineageProperty, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorLineageProperty.ResumeLayout(False)
        Me.BindingNavigatorLineageProperty.PerformLayout()
        CType(Me.BindingSourceLineageProperty, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxNavigator.ResumeLayout(False)
        Me.GroupBoxNavigator.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        Me.ContextMenuStripDocuments.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStripDocument.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBoxCategories As GroupBox
    Friend WithEvents LabelPromtLineageItem As Label
    Friend WithEvents LabelLineageItem As Label
    Friend WithEvents ButtonSave As Button
    Friend WithEvents ButtonClose As Button
    Friend WithEvents BindingNavigatorLineageProperty As BindingNavigator
    Friend WithEvents BindingNavigatorAddNewItem As ToolStripButton
    Friend WithEvents BindingSourceLineageProperty As BindingSource
    Friend WithEvents BindingNavigatorCountItem As ToolStripLabel
    Friend WithEvents BindingNavigatorDeleteItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveFirstItem As ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As ToolStripSeparator
    Friend WithEvents ButtonOpenDocument As Button
    Friend WithEvents GroupBoxNavigator As GroupBox
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButtonClose As ToolStripButton
    Friend WithEvents DiagramView As MindFusion.Diagramming.WinForms.DiagramView
    Friend WithEvents Diagram As MindFusion.Diagramming.Diagram
    Friend WithEvents SplitContainer As SplitContainer
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ContextMenuStripDocuments As ContextMenuStrip
    Friend WithEvents ClearFilterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripDocument As ContextMenuStrip
    Friend WithEvents FilterByThisDocumentToolStripMenuItem As ToolStripMenuItem
End Class
