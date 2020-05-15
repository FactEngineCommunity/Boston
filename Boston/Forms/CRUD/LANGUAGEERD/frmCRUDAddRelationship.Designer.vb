<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDAddRelationship
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCRUDAddRelationship))
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox
        Me.CheckBoxIsMandatory = New System.Windows.Forms.CheckBox
        Me.LabelEntityTypeName = New System.Windows.Forms.Label
        Me.LabelPromptEntityTypeName = New System.Windows.Forms.Label
        Me.GroupBoxMultiplicityConstraint = New System.Windows.Forms.GroupBox
        Me.PictureBoxBracket = New System.Windows.Forms.PictureBox
        Me.GroupBoxMultiplicityVerbalisation = New System.Windows.Forms.GroupBox
        Me.PictureBoxValueTypeAttribute = New System.Windows.Forms.PictureBox
        Me.PictureBoxMultiplicity = New System.Windows.Forms.PictureBox
        Me.LabelMultiplicityVerbalisation = New System.Windows.Forms.Label
        Me.RadioButtonOneToOne = New System.Windows.Forms.RadioButton
        Me.RadioButtonManyToOne = New System.Windows.Forms.RadioButton
        Me.ComboBoxReferences = New System.Windows.Forms.ComboBox
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.GroupBoxFactTypeReading = New System.Windows.Forms.GroupBox
        Me.ListBox_EnterpriseAware = New System.Windows.Forms.ListBox
        Me.DataGrid_Readings = New System.Windows.Forms.DataGridView
        Me.TextboxReading = New System.Windows.Forms.RichTextBox
        Me.Button3 = New System.Windows.Forms.Button
        Me.LabelHelp = New System.Windows.Forms.Label
        Me.GroupBoxMain.SuspendLayout()
        Me.GroupBoxMultiplicityConstraint.SuspendLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxMultiplicityVerbalisation.SuspendLayout()
        CType(Me.PictureBoxValueTypeAttribute, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxMultiplicity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxFactTypeReading.SuspendLayout()
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxReferences)
        Me.GroupBoxMain.Controls.Add(Me.CheckBoxIsMandatory)
        Me.GroupBoxMain.Controls.Add(Me.LabelEntityTypeName)
        Me.GroupBoxMain.Controls.Add(Me.LabelPromptEntityTypeName)
        Me.GroupBoxMain.Controls.Add(Me.GroupBoxMultiplicityConstraint)
        Me.GroupBoxMain.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(571, 257)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'CheckBoxIsMandatory
        '
        Me.CheckBoxIsMandatory.AutoSize = True
        Me.CheckBoxIsMandatory.Location = New System.Drawing.Point(97, 69)
        Me.CheckBoxIsMandatory.Name = "CheckBoxIsMandatory"
        Me.CheckBoxIsMandatory.Size = New System.Drawing.Size(87, 17)
        Me.CheckBoxIsMandatory.TabIndex = 15
        Me.CheckBoxIsMandatory.Text = "Is Mandatory"
        Me.CheckBoxIsMandatory.UseVisualStyleBackColor = True
        '
        'LabelEntityTypeName
        '
        Me.LabelEntityTypeName.AutoSize = True
        Me.LabelEntityTypeName.Location = New System.Drawing.Point(94, 21)
        Me.LabelEntityTypeName.Name = "LabelEntityTypeName"
        Me.LabelEntityTypeName.Size = New System.Drawing.Size(111, 13)
        Me.LabelEntityTypeName.TabIndex = 8
        Me.LabelEntityTypeName.Text = "LabelEntityTypeName"
        '
        'LabelPromptEntityTypeName
        '
        Me.LabelPromptEntityTypeName.AutoSize = True
        Me.LabelPromptEntityTypeName.Location = New System.Drawing.Point(19, 21)
        Me.LabelPromptEntityTypeName.Name = "LabelPromptEntityTypeName"
        Me.LabelPromptEntityTypeName.Size = New System.Drawing.Size(72, 13)
        Me.LabelPromptEntityTypeName.TabIndex = 7
        Me.LabelPromptEntityTypeName.Text = "Entity (Type) :"
        '
        'GroupBoxMultiplicityConstraint
        '
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.PictureBoxBracket)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.GroupBoxMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonOneToOne)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonManyToOne)
        Me.GroupBoxMultiplicityConstraint.Location = New System.Drawing.Point(6, 92)
        Me.GroupBoxMultiplicityConstraint.Name = "GroupBoxMultiplicityConstraint"
        Me.GroupBoxMultiplicityConstraint.Size = New System.Drawing.Size(559, 159)
        Me.GroupBoxMultiplicityConstraint.TabIndex = 2
        Me.GroupBoxMultiplicityConstraint.TabStop = False
        Me.GroupBoxMultiplicityConstraint.Text = "Multiplicity Constraint:"
        '
        'PictureBoxBracket
        '
        Me.PictureBoxBracket.Image = CType(resources.GetObject("PictureBoxBracket.Image"), System.Drawing.Image)
        Me.PictureBoxBracket.Location = New System.Drawing.Point(96, 30)
        Me.PictureBoxBracket.Name = "PictureBoxBracket"
        Me.PictureBoxBracket.Size = New System.Drawing.Size(21, 45)
        Me.PictureBoxBracket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxBracket.TabIndex = 8
        Me.PictureBoxBracket.TabStop = False
        '
        'GroupBoxMultiplicityVerbalisation
        '
        Me.GroupBoxMultiplicityVerbalisation.Controls.Add(Me.PictureBoxValueTypeAttribute)
        Me.GroupBoxMultiplicityVerbalisation.Controls.Add(Me.PictureBoxMultiplicity)
        Me.GroupBoxMultiplicityVerbalisation.Controls.Add(Me.LabelMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxMultiplicityVerbalisation.Location = New System.Drawing.Point(119, 11)
        Me.GroupBoxMultiplicityVerbalisation.Name = "GroupBoxMultiplicityVerbalisation"
        Me.GroupBoxMultiplicityVerbalisation.Size = New System.Drawing.Size(434, 142)
        Me.GroupBoxMultiplicityVerbalisation.TabIndex = 4
        Me.GroupBoxMultiplicityVerbalisation.TabStop = False
        Me.GroupBoxMultiplicityVerbalisation.Text = "Multiplicity Verbalisation:"
        '
        'PictureBoxValueTypeAttribute
        '
        Me.PictureBoxValueTypeAttribute.Image = CType(resources.GetObject("PictureBoxValueTypeAttribute.Image"), System.Drawing.Image)
        Me.PictureBoxValueTypeAttribute.Location = New System.Drawing.Point(6, 53)
        Me.PictureBoxValueTypeAttribute.Name = "PictureBoxValueTypeAttribute"
        Me.PictureBoxValueTypeAttribute.Size = New System.Drawing.Size(82, 70)
        Me.PictureBoxValueTypeAttribute.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBoxValueTypeAttribute.TabIndex = 14
        Me.PictureBoxValueTypeAttribute.TabStop = False
        Me.PictureBoxValueTypeAttribute.Visible = False
        '
        'PictureBoxMultiplicity
        '
        Me.PictureBoxMultiplicity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBoxMultiplicity.Location = New System.Drawing.Point(6, 53)
        Me.PictureBoxMultiplicity.Name = "PictureBoxMultiplicity"
        Me.PictureBoxMultiplicity.Size = New System.Drawing.Size(160, 48)
        Me.PictureBoxMultiplicity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxMultiplicity.TabIndex = 13
        Me.PictureBoxMultiplicity.TabStop = False
        '
        'LabelMultiplicityVerbalisation
        '
        Me.LabelMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMultiplicityVerbalisation.Location = New System.Drawing.Point(9, 20)
        Me.LabelMultiplicityVerbalisation.Name = "LabelMultiplicityVerbalisation"
        Me.LabelMultiplicityVerbalisation.Size = New System.Drawing.Size(318, 52)
        Me.LabelMultiplicityVerbalisation.TabIndex = 3
        '
        'RadioButtonOneToOne
        '
        Me.RadioButtonOneToOne.AutoSize = True
        Me.RadioButtonOneToOne.Location = New System.Drawing.Point(16, 54)
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
        'ComboBoxReferences
        '
        Me.ComboBoxReferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxReferences.FormattingEnabled = True
        Me.ComboBoxReferences.Location = New System.Drawing.Point(97, 42)
        Me.ComboBoxReferences.Name = "ComboBoxReferences"
        Me.ComboBoxReferences.Size = New System.Drawing.Size(242, 21)
        Me.ComboBoxReferences.TabIndex = 9
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(593, 23)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 1
        Me.ButtonOK.Text = "&OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(593, 52)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'GroupBoxFactTypeReading
        '
        Me.GroupBoxFactTypeReading.Controls.Add(Me.ListBox_EnterpriseAware)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.DataGrid_Readings)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.TextboxReading)
        Me.GroupBoxFactTypeReading.Controls.Add(Me.Button3)
        Me.GroupBoxFactTypeReading.Location = New System.Drawing.Point(12, 275)
        Me.GroupBoxFactTypeReading.Name = "GroupBoxFactTypeReading"
        Me.GroupBoxFactTypeReading.Size = New System.Drawing.Size(652, 179)
        Me.GroupBoxFactTypeReading.TabIndex = 3
        Me.GroupBoxFactTypeReading.TabStop = False
        Me.GroupBoxFactTypeReading.Text = "Attribute (Fact Type) Reading:"
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
        Me.DataGrid_Readings.Size = New System.Drawing.Size(539, 90)
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
        'LabelHelp
        '
        Me.LabelHelp.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelp.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelp.Location = New System.Drawing.Point(0, 464)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(685, 83)
        Me.LabelHelp.TabIndex = 12
        '
        'frmCRUDAddRelationship
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(685, 547)
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelHelp)
        Me.Controls.Add(Me.GroupBoxFactTypeReading)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Controls.Add(Me.ButtonOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "frmCRUDAddRelationship"
        Me.Text = "Add Attribute"
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.GroupBoxMultiplicityConstraint.ResumeLayout(False)
        Me.GroupBoxMultiplicityConstraint.PerformLayout()
        CType(Me.PictureBoxBracket, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxMultiplicityVerbalisation.ResumeLayout(False)
        Me.GroupBoxMultiplicityVerbalisation.PerformLayout()
        CType(Me.PictureBoxValueTypeAttribute, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxMultiplicity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxFactTypeReading.ResumeLayout(False)
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents LabelEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents ComboBoxReferences As System.Windows.Forms.ComboBox
    Friend WithEvents LabelHelp As System.Windows.Forms.Label
    Friend WithEvents PictureBoxMultiplicity As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxBracket As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxValueTypeAttribute As System.Windows.Forms.PictureBox
    Friend WithEvents CheckBoxIsMandatory As System.Windows.Forms.CheckBox
End Class
