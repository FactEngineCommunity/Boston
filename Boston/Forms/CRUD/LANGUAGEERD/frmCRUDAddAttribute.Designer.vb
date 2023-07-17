<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDAddAttribute
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Dim NameLabel As System.Windows.Forms.Label
        Dim DataTypeLabel As System.Windows.Forms.Label
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCRUDAddAttribute))
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckBoxIsPrimaryIdentifier = New System.Windows.Forms.CheckBox
        Me.BindingSourceAttribute = New System.Windows.Forms.BindingSource(Me.components)
        Me.CheckBoxIsMandatory = New System.Windows.Forms.CheckBox
        Me.ComboBoxDataType = New System.Windows.Forms.ComboBox
        Me.BindingNavigatorAttribute = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.ComboBoxAttribute = New System.Windows.Forms.ComboBox
        Me.LabelPromptOfReferencedEntity = New System.Windows.Forms.Label
        Me.ComboBoxKeyAttributes = New System.Windows.Forms.ComboBox
        Me.LabelPromptLinksToAttribute = New System.Windows.Forms.Label
        Me.GroupBoxAttributeType = New System.Windows.Forms.GroupBox
        Me.GroupBoxAttributePicture = New System.Windows.Forms.GroupBox
        Me.PictureBoxAttribute = New System.Windows.Forms.PictureBox
        Me.GroupBoxRelationshipType = New System.Windows.Forms.GroupBox
        Me.LabelRelationshipVerbalisation = New System.Windows.Forms.Label
        Me.LabelPrompRelationDescription = New System.Windows.Forms.Label
        Me.PictureBoxRelationType = New System.Windows.Forms.PictureBox
        Me.CheckBoxRelationContributesToPrimaryKey = New System.Windows.Forms.CheckBox
        Me.CheckBoxRelationOriginMandatory = New System.Windows.Forms.CheckBox
        Me.CheckBoxRelationDestinationMany = New System.Windows.Forms.CheckBox
        Me.CheckBoxRelationOriginMany = New System.Windows.Forms.CheckBox
        Me.CheckBoxRelationDestinationMandatory = New System.Windows.Forms.CheckBox
        Me.GroupBoxReferences = New System.Windows.Forms.GroupBox
        Me.LabelValueTypeDescription = New System.Windows.Forms.Label
        Me.ComboBoxReferences = New System.Windows.Forms.ComboBox
        Me.LabelObjectType = New System.Windows.Forms.Label
        Me.ComboBoxObjectType = New System.Windows.Forms.ComboBox
        Me.GroupBoxFactTypeReading = New System.Windows.Forms.GroupBox
        Me.ListBox_EnterpriseAware = New System.Windows.Forms.ListBox
        Me.DataGrid_Readings = New System.Windows.Forms.DataGridView
        Me.TextboxReading = New System.Windows.Forms.RichTextBox
        Me.Button3 = New System.Windows.Forms.Button
        Me.LabelEntityTypeName = New System.Windows.Forms.Label
        Me.LabelPromptEntityTypeName = New System.Windows.Forms.Label
        Me.GroupBoxMultiplicityConstraint = New System.Windows.Forms.GroupBox
        Me.PictureBoxBracket = New System.Windows.Forms.PictureBox
        Me.GroupBoxMultiplicityVerbalisation = New System.Windows.Forms.GroupBox
        Me.LabelMultiplicityVerbalisation = New System.Windows.Forms.Label
        Me.RadioButtonOneToOne = New System.Windows.Forms.RadioButton
        Me.RadioButtonManyToOne = New System.Windows.Forms.RadioButton
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.LabelHelp = New System.Windows.Forms.Label
        Me.TimerStartTip = New System.Windows.Forms.Timer(Me.components)
        NameLabel = New System.Windows.Forms.Label
        DataTypeLabel = New System.Windows.Forms.Label
        Me.GroupBoxMain.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.BindingSourceAttribute, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingNavigatorAttribute, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigatorAttribute.SuspendLayout()
        Me.GroupBoxAttributeType.SuspendLayout()
        Me.GroupBoxAttributePicture.SuspendLayout()
        CType(Me.PictureBoxAttribute, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxRelationshipType.SuspendLayout()
        CType(Me.PictureBoxRelationType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxReferences.SuspendLayout()
        Me.GroupBoxFactTypeReading.SuspendLayout()
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxMultiplicityConstraint.SuspendLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxMultiplicityVerbalisation.SuspendLayout()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        NameLabel.AutoSize = True
        NameLabel.Location = New System.Drawing.Point(35, 32)
        NameLabel.Name = "NameLabel"
        NameLabel.Size = New System.Drawing.Size(38, 13)
        NameLabel.TabIndex = 19
        NameLabel.Text = "Name:"
        '
        'DataTypeLabel
        '
        DataTypeLabel.AutoSize = True
        DataTypeLabel.Location = New System.Drawing.Point(13, 64)
        DataTypeLabel.Name = "DataTypeLabel"
        DataTypeLabel.Size = New System.Drawing.Size(60, 13)
        DataTypeLabel.TabIndex = 20
        DataTypeLabel.Text = "Data Type:"
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Controls.Add(Me.GroupBox1)
        Me.GroupBoxMain.Controls.Add(Me.GroupBoxAttributeType)
        Me.GroupBoxMain.Controls.Add(Me.GroupBoxFactTypeReading)
        Me.GroupBoxMain.Controls.Add(Me.LabelEntityTypeName)
        Me.GroupBoxMain.Controls.Add(Me.LabelPromptEntityTypeName)
        Me.GroupBoxMain.Controls.Add(Me.GroupBoxMultiplicityConstraint)
        Me.GroupBoxMain.Location = New System.Drawing.Point(0, 0)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(739, 715)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.CheckBoxIsPrimaryIdentifier)
        Me.GroupBox1.Controls.Add(Me.CheckBoxIsMandatory)
        Me.GroupBox1.Controls.Add(DataTypeLabel)
        Me.GroupBox1.Controls.Add(Me.ComboBoxDataType)
        Me.GroupBox1.Controls.Add(Me.BindingNavigatorAttribute)
        Me.GroupBox1.Controls.Add(NameLabel)
        Me.GroupBox1.Controls.Add(Me.ComboBoxAttribute)
        Me.GroupBox1.Controls.Add(Me.LabelPromptOfReferencedEntity)
        Me.GroupBox1.Controls.Add(Me.ComboBoxKeyAttributes)
        Me.GroupBox1.Controls.Add(Me.LabelPromptLinksToAttribute)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 262)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(709, 166)
        Me.GroupBox1.TabIndex = 20
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Attribute Details :"
        '
        'CheckBoxIsPrimaryIdentifier
        '
        Me.CheckBoxIsPrimaryIdentifier.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.BindingSourceAttribute, "PartOfPrimaryKey", True))
        Me.CheckBoxIsPrimaryIdentifier.Location = New System.Drawing.Point(79, 110)
        Me.CheckBoxIsPrimaryIdentifier.Name = "CheckBoxIsPrimaryIdentifier"
        Me.CheckBoxIsPrimaryIdentifier.Size = New System.Drawing.Size(196, 24)
        Me.CheckBoxIsPrimaryIdentifier.TabIndex = 23
        Me.CheckBoxIsPrimaryIdentifier.Text = "Is &Primary Key / Part of Primary Key"
        Me.CheckBoxIsPrimaryIdentifier.UseVisualStyleBackColor = True
        '
        'BindingSourceAttribute
        '
        'Me.BindingSourceAttribute.DataSource = GetType(ERD.Attribute)
        '
        'CheckBoxIsMandatory
        '
        Me.CheckBoxIsMandatory.DataBindings.Add(New System.Windows.Forms.Binding("CheckState", Me.BindingSourceAttribute, "Mandatory", True))
        Me.CheckBoxIsMandatory.Location = New System.Drawing.Point(79, 88)
        Me.CheckBoxIsMandatory.Name = "CheckBoxIsMandatory"
        Me.CheckBoxIsMandatory.Size = New System.Drawing.Size(104, 24)
        Me.CheckBoxIsMandatory.TabIndex = 22
        Me.CheckBoxIsMandatory.Text = "Is &Mandatory"
        Me.CheckBoxIsMandatory.UseVisualStyleBackColor = True
        '
        'ComboBoxDataType
        '
        Me.ComboBoxDataType.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSourceAttribute, "DataType", True))
        Me.ComboBoxDataType.FormattingEnabled = True
        Me.ComboBoxDataType.Location = New System.Drawing.Point(79, 61)
        Me.ComboBoxDataType.Name = "ComboBoxDataType"
        Me.ComboBoxDataType.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxDataType.TabIndex = 21
        '
        'BindingNavigatorAttribute
        '
        Me.BindingNavigatorAttribute.AddNewItem = Nothing
        Me.BindingNavigatorAttribute.BackColor = System.Drawing.SystemColors.Control
        Me.BindingNavigatorAttribute.BindingSource = Me.BindingSourceAttribute
        Me.BindingNavigatorAttribute.CountItem = Me.BindingNavigatorCountItem
        Me.BindingNavigatorAttribute.DeleteItem = Nothing
        Me.BindingNavigatorAttribute.Dock = System.Windows.Forms.DockStyle.None
        Me.BindingNavigatorAttribute.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem})
        Me.BindingNavigatorAttribute.Location = New System.Drawing.Point(10, 137)
        Me.BindingNavigatorAttribute.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.BindingNavigatorAttribute.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.BindingNavigatorAttribute.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.BindingNavigatorAttribute.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.BindingNavigatorAttribute.Name = "BindingNavigatorAttribute"
        Me.BindingNavigatorAttribute.PositionItem = Me.BindingNavigatorPositionItem
        Me.BindingNavigatorAttribute.Size = New System.Drawing.Size(203, 25)
        Me.BindingNavigatorAttribute.TabIndex = 13
        Me.BindingNavigatorAttribute.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = My.Resources.MenuImages.Properties216x16
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = My.Resources.MenuImages.Paste16x16
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
        Me.BindingNavigatorMoveNextItem.Image = My.Resources.MenuImages.Properties216x16
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = My.Resources.MenuImages.Properties216x16
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'ComboBoxAttribute
        '
        Me.ComboBoxAttribute.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.BindingSourceAttribute, "Name", True))
        Me.ComboBoxAttribute.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxAttribute.ForeColor = System.Drawing.Color.SteelBlue
        Me.ComboBoxAttribute.FormattingEnabled = True
        Me.ComboBoxAttribute.Location = New System.Drawing.Point(79, 29)
        Me.ComboBoxAttribute.Name = "ComboBoxAttribute"
        Me.ComboBoxAttribute.Size = New System.Drawing.Size(240, 23)
        Me.ComboBoxAttribute.TabIndex = 20
        '
        'LabelPromptOfReferencedEntity
        '
        Me.LabelPromptOfReferencedEntity.AutoSize = True
        Me.LabelPromptOfReferencedEntity.Enabled = False
        Me.LabelPromptOfReferencedEntity.Location = New System.Drawing.Point(433, 61)
        Me.LabelPromptOfReferencedEntity.Name = "LabelPromptOfReferencedEntity"
        Me.LabelPromptOfReferencedEntity.Size = New System.Drawing.Size(126, 13)
        Me.LabelPromptOfReferencedEntity.TabIndex = 19
        Me.LabelPromptOfReferencedEntity.Text = "...of Entity <Entity Name>"
        '
        'ComboBoxKeyAttributes
        '
        Me.ComboBoxKeyAttributes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxKeyAttributes.Enabled = False
        Me.ComboBoxKeyAttributes.FormattingEnabled = True
        Me.ComboBoxKeyAttributes.Location = New System.Drawing.Point(436, 29)
        Me.ComboBoxKeyAttributes.Name = "ComboBoxKeyAttributes"
        Me.ComboBoxKeyAttributes.Size = New System.Drawing.Size(253, 21)
        Me.ComboBoxKeyAttributes.TabIndex = 18
        '
        'LabelPromptLinksToAttribute
        '
        Me.LabelPromptLinksToAttribute.AutoSize = True
        Me.LabelPromptLinksToAttribute.Enabled = False
        Me.LabelPromptLinksToAttribute.Location = New System.Drawing.Point(325, 32)
        Me.LabelPromptLinksToAttribute.Name = "LabelPromptLinksToAttribute"
        Me.LabelPromptLinksToAttribute.Size = New System.Drawing.Size(105, 13)
        Me.LabelPromptLinksToAttribute.TabIndex = 17
        Me.LabelPromptLinksToAttribute.Text = "...refers to Attribute..."
        '
        'GroupBoxAttributeType
        '
        Me.GroupBoxAttributeType.Controls.Add(Me.GroupBoxAttributePicture)
        Me.GroupBoxAttributeType.Controls.Add(Me.GroupBoxRelationshipType)
        Me.GroupBoxAttributeType.Controls.Add(Me.GroupBoxReferences)
        Me.GroupBoxAttributeType.Controls.Add(Me.LabelObjectType)
        Me.GroupBoxAttributeType.Controls.Add(Me.ComboBoxObjectType)
        Me.GroupBoxAttributeType.Location = New System.Drawing.Point(12, 37)
        Me.GroupBoxAttributeType.Name = "GroupBoxAttributeType"
        Me.GroupBoxAttributeType.Size = New System.Drawing.Size(709, 219)
        Me.GroupBoxAttributeType.TabIndex = 16
        Me.GroupBoxAttributeType.TabStop = False
        Me.GroupBoxAttributeType.Text = "Referential Integrity :"
        '
        'GroupBoxAttributePicture
        '
        Me.GroupBoxAttributePicture.Controls.Add(Me.PictureBoxAttribute)
        Me.GroupBoxAttributePicture.Location = New System.Drawing.Point(11, 68)
        Me.GroupBoxAttributePicture.Name = "GroupBoxAttributePicture"
        Me.GroupBoxAttributePicture.Size = New System.Drawing.Size(128, 145)
        Me.GroupBoxAttributePicture.TabIndex = 20
        Me.GroupBoxAttributePicture.TabStop = False
        Me.GroupBoxAttributePicture.Text = "Attribute View"
        '
        'PictureBoxAttribute
        '
        Me.PictureBoxAttribute.Image = CType(resources.GetObject("PictureBoxAttribute.Image"), System.Drawing.Image)
        Me.PictureBoxAttribute.Location = New System.Drawing.Point(23, 32)
        Me.PictureBoxAttribute.Name = "PictureBoxAttribute"
        Me.PictureBoxAttribute.Size = New System.Drawing.Size(90, 70)
        Me.PictureBoxAttribute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxAttribute.TabIndex = 0
        Me.PictureBoxAttribute.TabStop = False
        '
        'GroupBoxRelationshipType
        '
        Me.GroupBoxRelationshipType.Controls.Add(Me.LabelRelationshipVerbalisation)
        Me.GroupBoxRelationshipType.Controls.Add(Me.LabelPrompRelationDescription)
        Me.GroupBoxRelationshipType.Controls.Add(Me.PictureBoxRelationType)
        Me.GroupBoxRelationshipType.Controls.Add(Me.CheckBoxRelationContributesToPrimaryKey)
        Me.GroupBoxRelationshipType.Controls.Add(Me.CheckBoxRelationOriginMandatory)
        Me.GroupBoxRelationshipType.Controls.Add(Me.CheckBoxRelationDestinationMany)
        Me.GroupBoxRelationshipType.Controls.Add(Me.CheckBoxRelationOriginMany)
        Me.GroupBoxRelationshipType.Controls.Add(Me.CheckBoxRelationDestinationMandatory)
        Me.GroupBoxRelationshipType.Enabled = False
        Me.GroupBoxRelationshipType.Location = New System.Drawing.Point(145, 68)
        Me.GroupBoxRelationshipType.Name = "GroupBoxRelationshipType"
        Me.GroupBoxRelationshipType.Size = New System.Drawing.Size(548, 145)
        Me.GroupBoxRelationshipType.TabIndex = 19
        Me.GroupBoxRelationshipType.TabStop = False
        Me.GroupBoxRelationshipType.Text = "Relationship Type:"
        '
        'LabelRelationshipVerbalisation
        '
        Me.LabelRelationshipVerbalisation.Location = New System.Drawing.Point(13, 110)
        Me.LabelRelationshipVerbalisation.Name = "LabelRelationshipVerbalisation"
        Me.LabelRelationshipVerbalisation.Size = New System.Drawing.Size(529, 32)
        Me.LabelRelationshipVerbalisation.TabIndex = 20
        Me.LabelRelationshipVerbalisation.Text = "LabelRelationshipVerbalisation"
        '
        'LabelPrompRelationDescription
        '
        Me.LabelPrompRelationDescription.AutoSize = True
        Me.LabelPrompRelationDescription.ForeColor = System.Drawing.Color.Black
        Me.LabelPrompRelationDescription.Location = New System.Drawing.Point(18, 25)
        Me.LabelPrompRelationDescription.Name = "LabelPrompRelationDescription"
        Me.LabelPrompRelationDescription.Size = New System.Drawing.Size(455, 13)
        Me.LabelPrompRelationDescription.TabIndex = 19
        Me.LabelPrompRelationDescription.Text = "The relationship between Entity, <EntityName>, and Entity, <Entityname2>, has the" & _
            "se qualities :"
        '
        'PictureBoxRelationType
        '
        Me.PictureBoxRelationType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBoxRelationType.Location = New System.Drawing.Point(191, 52)
        Me.PictureBoxRelationType.Name = "PictureBoxRelationType"
        Me.PictureBoxRelationType.Size = New System.Drawing.Size(160, 48)
        Me.PictureBoxRelationType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxRelationType.TabIndex = 13
        Me.PictureBoxRelationType.TabStop = False
        '
        'CheckBoxRelationContributesToPrimaryKey
        '
        Me.CheckBoxRelationContributesToPrimaryKey.AutoSize = True
        Me.CheckBoxRelationContributesToPrimaryKey.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxRelationContributesToPrimaryKey.Location = New System.Drawing.Point(21, 86)
        Me.CheckBoxRelationContributesToPrimaryKey.Name = "CheckBoxRelationContributesToPrimaryKey"
        Me.CheckBoxRelationContributesToPrimaryKey.Size = New System.Drawing.Size(149, 17)
        Me.CheckBoxRelationContributesToPrimaryKey.TabIndex = 18
        Me.CheckBoxRelationContributesToPrimaryKey.Text = "Contributes to Primary Key"
        Me.CheckBoxRelationContributesToPrimaryKey.UseVisualStyleBackColor = True
        '
        'CheckBoxRelationOriginMandatory
        '
        Me.CheckBoxRelationOriginMandatory.AutoSize = True
        Me.CheckBoxRelationOriginMandatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxRelationOriginMandatory.Location = New System.Drawing.Point(94, 52)
        Me.CheckBoxRelationOriginMandatory.Name = "CheckBoxRelationOriginMandatory"
        Me.CheckBoxRelationOriginMandatory.Size = New System.Drawing.Size(76, 17)
        Me.CheckBoxRelationOriginMandatory.TabIndex = 14
        Me.CheckBoxRelationOriginMandatory.Text = "Mandatory"
        Me.CheckBoxRelationOriginMandatory.UseVisualStyleBackColor = True
        '
        'CheckBoxRelationDestinationMany
        '
        Me.CheckBoxRelationDestinationMany.AutoSize = True
        Me.CheckBoxRelationDestinationMany.Location = New System.Drawing.Point(372, 69)
        Me.CheckBoxRelationDestinationMany.Name = "CheckBoxRelationDestinationMany"
        Me.CheckBoxRelationDestinationMany.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxRelationDestinationMany.TabIndex = 17
        Me.CheckBoxRelationDestinationMany.Text = "Many"
        Me.CheckBoxRelationDestinationMany.UseVisualStyleBackColor = True
        '
        'CheckBoxRelationOriginMany
        '
        Me.CheckBoxRelationOriginMany.AutoSize = True
        Me.CheckBoxRelationOriginMany.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBoxRelationOriginMany.Location = New System.Drawing.Point(118, 69)
        Me.CheckBoxRelationOriginMany.Name = "CheckBoxRelationOriginMany"
        Me.CheckBoxRelationOriginMany.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxRelationOriginMany.TabIndex = 15
        Me.CheckBoxRelationOriginMany.Text = "Many"
        Me.CheckBoxRelationOriginMany.UseVisualStyleBackColor = True
        '
        'CheckBoxRelationDestinationMandatory
        '
        Me.CheckBoxRelationDestinationMandatory.AutoSize = True
        Me.CheckBoxRelationDestinationMandatory.Location = New System.Drawing.Point(372, 52)
        Me.CheckBoxRelationDestinationMandatory.Name = "CheckBoxRelationDestinationMandatory"
        Me.CheckBoxRelationDestinationMandatory.Size = New System.Drawing.Size(76, 17)
        Me.CheckBoxRelationDestinationMandatory.TabIndex = 16
        Me.CheckBoxRelationDestinationMandatory.Text = "Mandatory"
        Me.CheckBoxRelationDestinationMandatory.UseVisualStyleBackColor = True
        '
        'GroupBoxReferences
        '
        Me.GroupBoxReferences.Controls.Add(Me.LabelValueTypeDescription)
        Me.GroupBoxReferences.Controls.Add(Me.ComboBoxReferences)
        Me.GroupBoxReferences.Location = New System.Drawing.Point(292, 14)
        Me.GroupBoxReferences.Name = "GroupBoxReferences"
        Me.GroupBoxReferences.Size = New System.Drawing.Size(266, 54)
        Me.GroupBoxReferences.TabIndex = 11
        Me.GroupBoxReferences.TabStop = False
        Me.GroupBoxReferences.Text = "References Entity"
        '
        'LabelValueTypeDescription
        '
        Me.LabelValueTypeDescription.Location = New System.Drawing.Point(4, 22)
        Me.LabelValueTypeDescription.Name = "LabelValueTypeDescription"
        Me.LabelValueTypeDescription.Size = New System.Drawing.Size(257, 20)
        Me.LabelValueTypeDescription.TabIndex = 12
        Me.LabelValueTypeDescription.Text = "The Attribute stores a Value and references no Entity"
        '
        'ComboBoxReferences
        '
        Me.ComboBoxReferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxReferences.FormattingEnabled = True
        Me.ComboBoxReferences.Location = New System.Drawing.Point(11, 19)
        Me.ComboBoxReferences.Name = "ComboBoxReferences"
        Me.ComboBoxReferences.Size = New System.Drawing.Size(242, 21)
        Me.ComboBoxReferences.TabIndex = 9
        Me.ComboBoxReferences.Visible = False
        '
        'LabelObjectType
        '
        Me.LabelObjectType.AutoSize = True
        Me.LabelObjectType.Location = New System.Drawing.Point(5, 36)
        Me.LabelObjectType.Name = "LabelObjectType"
        Me.LabelObjectType.Size = New System.Drawing.Size(79, 13)
        Me.LabelObjectType.TabIndex = 3
        Me.LabelObjectType.Text = "Attribute Type :"
        '
        'ComboBoxObjectType
        '
        Me.ComboBoxObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxObjectType.FormattingEnabled = True
        Me.ComboBoxObjectType.Location = New System.Drawing.Point(90, 33)
        Me.ComboBoxObjectType.Name = "ComboBoxObjectType"
        Me.ComboBoxObjectType.Size = New System.Drawing.Size(196, 21)
        Me.ComboBoxObjectType.TabIndex = 4
        '
        'GroupBoxFactTypeReading
        '
        Me.GroupBoxFactTypeReading.Controls.Add(Me.ListBox_EnterpriseAware)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.DataGrid_Readings)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.TextboxReading)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.Button3)
        Me.GroupBoxFactTypeReading.Location = New System.Drawing.Point(12, 543)
        Me.GroupBoxFactTypeReading.Name = "GroupBoxFactTypeReading"
        Me.GroupBoxFactTypeReading.Size = New System.Drawing.Size(651, 166)
        Me.GroupBoxFactTypeReading.TabIndex = 3
        Me.GroupBoxFactTypeReading.TabStop = False
        Me.GroupBoxFactTypeReading.Text = "Attribute Reading:"
        '
        'ListBox_EnterpriseAware
        '
        Me.ListBox_EnterpriseAware.FormattingEnabled = True
        Me.ListBox_EnterpriseAware.Location = New System.Drawing.Point(64, 51)
        Me.ListBox_EnterpriseAware.Name = "ListBox_EnterpriseAware"
        Me.ListBox_EnterpriseAware.Size = New System.Drawing.Size(149, 121)
        Me.ListBox_EnterpriseAware.TabIndex = 11
        Me.ListBox_EnterpriseAware.Visible = False
        '
        'DataGrid_Readings
        '
        Me.DataGrid_Readings.AllowUserToAddRows = False
        Me.DataGrid_Readings.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGrid_Readings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid_Readings.Location = New System.Drawing.Point(13, 69)
        Me.DataGrid_Readings.Name = "DataGrid_Readings"
        Me.DataGrid_Readings.Size = New System.Drawing.Size(538, 90)
        Me.DataGrid_Readings.TabIndex = 10
        '
        'TextboxReading
        '
        Me.TextboxReading.AutoWordSelection = True
        Me.TextboxReading.Location = New System.Drawing.Point(13, 29)
        Me.TextboxReading.Multiline = False
        Me.TextboxReading.Name = "TextboxReading"
        Me.TextboxReading.Size = New System.Drawing.Size(539, 34)
        Me.TextboxReading.TabIndex = 8
        Me.TextboxReading.Text = ""
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(558, 29)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(88, 34)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "&Clear Reading"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'LabelEntityTypeName
        '
        Me.LabelEntityTypeName.AutoSize = True
        Me.LabelEntityTypeName.Location = New System.Drawing.Point(54, 18)
        Me.LabelEntityTypeName.Name = "LabelEntityTypeName"
        Me.LabelEntityTypeName.Size = New System.Drawing.Size(111, 13)
        Me.LabelEntityTypeName.TabIndex = 8
        Me.LabelEntityTypeName.Text = "LabelEntityTypeName"
        '
        'LabelPromptEntityTypeName
        '
        Me.LabelPromptEntityTypeName.AutoSize = True
        Me.LabelPromptEntityTypeName.Location = New System.Drawing.Point(9, 18)
        Me.LabelPromptEntityTypeName.Name = "LabelPromptEntityTypeName"
        Me.LabelPromptEntityTypeName.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptEntityTypeName.TabIndex = 7
        Me.LabelPromptEntityTypeName.Text = "Entity :"
        '
        'GroupBoxMultiplicityConstraint
        '
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.PictureBoxBracket)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.GroupBoxMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonOneToOne)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonManyToOne)
        Me.GroupBoxMultiplicityConstraint.Location = New System.Drawing.Point(12, 434)
        Me.GroupBoxMultiplicityConstraint.Name = "GroupBoxMultiplicityConstraint"
        Me.GroupBoxMultiplicityConstraint.Size = New System.Drawing.Size(709, 103)
        Me.GroupBoxMultiplicityConstraint.TabIndex = 2
        Me.GroupBoxMultiplicityConstraint.TabStop = False
        Me.GroupBoxMultiplicityConstraint.Text = "Value Multiplicity Constraint:"
        '
        'PictureBoxBracket
        '
        Me.PictureBoxBracket.Image = CType(resources.GetObject("PictureBoxBracket.Image"), System.Drawing.Image)
        Me.PictureBoxBracket.Location = New System.Drawing.Point(96, 26)
        Me.PictureBoxBracket.Name = "PictureBoxBracket"
        Me.PictureBoxBracket.Size = New System.Drawing.Size(21, 57)
        Me.PictureBoxBracket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxBracket.TabIndex = 8
        Me.PictureBoxBracket.TabStop = False
        '
        'GroupBoxMultiplicityVerbalisation
        '
        Me.GroupBoxMultiplicityVerbalisation.Controls.Add(Me.LabelMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxMultiplicityVerbalisation.Location = New System.Drawing.Point(119, 13)
        Me.GroupBoxMultiplicityVerbalisation.Name = "GroupBoxMultiplicityVerbalisation"
        Me.GroupBoxMultiplicityVerbalisation.Size = New System.Drawing.Size(584, 83)
        Me.GroupBoxMultiplicityVerbalisation.TabIndex = 4
        Me.GroupBoxMultiplicityVerbalisation.TabStop = False
        Me.GroupBoxMultiplicityVerbalisation.Text = "Multiplicity Verbalisation:"
        '
        'LabelMultiplicityVerbalisation
        '
        Me.LabelMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMultiplicityVerbalisation.Location = New System.Drawing.Point(9, 20)
        Me.LabelMultiplicityVerbalisation.Name = "LabelMultiplicityVerbalisation"
        Me.LabelMultiplicityVerbalisation.Size = New System.Drawing.Size(569, 52)
        Me.LabelMultiplicityVerbalisation.TabIndex = 3
        '
        'RadioButtonOneToOne
        '
        Me.RadioButtonOneToOne.AutoSize = True
        Me.RadioButtonOneToOne.Location = New System.Drawing.Point(15, 54)
        Me.RadioButtonOneToOne.Name = "RadioButtonOneToOne"
        Me.RadioButtonOneToOne.Size = New System.Drawing.Size(80, 17)
        Me.RadioButtonOneToOne.TabIndex = 2
        Me.RadioButtonOneToOne.Text = "One to One"
        Me.RadioButtonOneToOne.UseVisualStyleBackColor = True
        '
        'RadioButtonManyToOne
        '
        Me.RadioButtonManyToOne.AutoSize = True
        Me.RadioButtonManyToOne.Checked = True
        Me.RadioButtonManyToOne.Location = New System.Drawing.Point(15, 31)
        Me.RadioButtonManyToOne.Name = "RadioButtonManyToOne"
        Me.RadioButtonManyToOne.Size = New System.Drawing.Size(86, 17)
        Me.RadioButtonManyToOne.TabIndex = 1
        Me.RadioButtonManyToOne.TabStop = True
        Me.RadioButtonManyToOne.Text = "Many to One"
        Me.RadioButtonManyToOne.UseVisualStyleBackColor = True
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(750, 21)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 1
        Me.ButtonOK.Text = "&OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(750, 51)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'LabelHelp
        '
        Me.LabelHelp.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelp.Location = New System.Drawing.Point(0, 718)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(837, 83)
        Me.LabelHelp.TabIndex = 12
        '
        'TimerStartTip
        '
        Me.TimerStartTip.Enabled = True
        Me.TimerStartTip.Interval = 450
        '
        'frmCRUDAddAttribute
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(837, 801)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.LabelHelp)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "frmCRUDAddAttribute"
        Me.Text = "Add Attribute/s"
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.BindingSourceAttribute, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingNavigatorAttribute, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigatorAttribute.ResumeLayout(False)
        Me.BindingNavigatorAttribute.PerformLayout()
        Me.GroupBoxAttributeType.ResumeLayout(False)
        Me.GroupBoxAttributeType.PerformLayout()
        Me.GroupBoxAttributePicture.ResumeLayout(False)
        Me.GroupBoxAttributePicture.PerformLayout()
        CType(Me.PictureBoxAttribute, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxRelationshipType.ResumeLayout(False)
        Me.GroupBoxRelationshipType.PerformLayout()
        CType(Me.PictureBoxRelationType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxReferences.ResumeLayout(False)
        Me.GroupBoxFactTypeReading.ResumeLayout(False)
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxMultiplicityConstraint.ResumeLayout(False)
        Me.GroupBoxMultiplicityConstraint.PerformLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxMultiplicityVerbalisation.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBoxMain As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBoxMultiplicityConstraint As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButtonOneToOne As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonManyToOne As System.Windows.Forms.RadioButton
    Friend WithEvents LabelMultiplicityVerbalisation As System.Windows.Forms.Label
    Friend WithEvents GroupBoxMultiplicityVerbalisation As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxFactTypeReading As System.Windows.Forms.GroupBox
    Friend WithEvents DataGrid_Readings As System.Windows.Forms.DataGridView
    Friend WithEvents TextboxReading As System.Windows.Forms.RichTextBox
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ListBox_EnterpriseAware As System.Windows.Forms.ListBox
    Friend WithEvents LabelObjectType As System.Windows.Forms.Label
    Friend WithEvents ComboBoxObjectType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents ComboBoxReferences As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxReferences As System.Windows.Forms.GroupBox
    Friend WithEvents LabelValueTypeDescription As System.Windows.Forms.Label
    Friend WithEvents LabelHelp As System.Windows.Forms.Label
    Friend WithEvents PictureBoxRelationType As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxBracket As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBoxAttributeType As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBoxKeyAttributes As System.Windows.Forms.ComboBox
    Friend WithEvents LabelPromptLinksToAttribute As System.Windows.Forms.Label
    Friend WithEvents LabelPromptOfReferencedEntity As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxRelationDestinationMany As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRelationDestinationMandatory As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRelationOriginMany As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRelationOriginMandatory As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxRelationContributesToPrimaryKey As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBoxRelationshipType As System.Windows.Forms.GroupBox
    Friend WithEvents LabelRelationshipVerbalisation As System.Windows.Forms.Label
    Friend WithEvents LabelPrompRelationDescription As System.Windows.Forms.Label
    Friend WithEvents CheckBoxIsPrimaryIdentifier As System.Windows.Forms.CheckBox
    Friend WithEvents BindingSourceAttribute As System.Windows.Forms.BindingSource
    Friend WithEvents CheckBoxIsMandatory As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxDataType As System.Windows.Forms.ComboBox
    Friend WithEvents BindingNavigatorAttribute As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents ComboBoxAttribute As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBoxAttributePicture As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBoxAttribute As System.Windows.Forms.PictureBox
    Friend WithEvents TimerStartTip As System.Windows.Forms.Timer
End Class
