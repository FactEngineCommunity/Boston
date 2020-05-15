<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditAttribute
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
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox
        Me.GroupBoxMultiplicityConstraint = New System.Windows.Forms.GroupBox
        Me.GroupBoxMultiplicityVerbalisation = New System.Windows.Forms.GroupBox
        Me.LabelMultiplicityVerbalisation = New System.Windows.Forms.Label
        Me.RadioButtonOneToOne = New System.Windows.Forms.RadioButton
        Me.RadioButtonManyToOne = New System.Windows.Forms.RadioButton
        Me.ComboBoxAttribute = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.GroupBoxFactTypeReading = New System.Windows.Forms.GroupBox
        Me.ListBox_EnterpriseAware = New System.Windows.Forms.ListBox
        Me.DataGrid_Readings = New System.Windows.Forms.DataGridView
        Me.TextboxReading = New System.Windows.Forms.RichTextBox
        Me.Button3 = New System.Windows.Forms.Button
        Me.LabelObjectType = New System.Windows.Forms.Label
        Me.ComboBoxObjectType = New System.Windows.Forms.ComboBox
        Me.GroupBoxMain.SuspendLayout()
        Me.GroupBoxMultiplicityConstraint.SuspendLayout()
        Me.GroupBoxMultiplicityVerbalisation.SuspendLayout()
        Me.GroupBoxFactTypeReading.SuspendLayout()
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBoxMain
        '
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxObjectType)
        Me.GroupBoxMain.Controls.Add(Me.LabelObjectType)
        Me.GroupBoxMain.Controls.Add(Me.GroupBoxMultiplicityConstraint)
        Me.GroupBoxMain.Controls.Add(Me.ComboBoxAttribute)
        Me.GroupBoxMain.Controls.Add(Me.Label1)
        Me.GroupBoxMain.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(471, 185)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'GroupBoxMultiplicityConstraint
        '
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.GroupBoxMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonOneToOne)
        Me.GroupBoxMultiplicityConstraint.Controls.Add(Me.RadioButtonManyToOne)
        Me.GroupBoxMultiplicityConstraint.Location = New System.Drawing.Point(10, 78)
        Me.GroupBoxMultiplicityConstraint.Name = "GroupBoxMultiplicityConstraint"
        Me.GroupBoxMultiplicityConstraint.Size = New System.Drawing.Size(450, 90)
        Me.GroupBoxMultiplicityConstraint.TabIndex = 2
        Me.GroupBoxMultiplicityConstraint.TabStop = False
        Me.GroupBoxMultiplicityConstraint.Text = "Multiplicity Constraint:"
        '
        'GroupBoxMultiplicityVerbalisation
        '
        Me.GroupBoxMultiplicityVerbalisation.Controls.Add(Me.LabelMultiplicityVerbalisation)
        Me.GroupBoxMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxMultiplicityVerbalisation.Location = New System.Drawing.Point(119, 11)
        Me.GroupBoxMultiplicityVerbalisation.Name = "GroupBoxMultiplicityVerbalisation"
        Me.GroupBoxMultiplicityVerbalisation.Size = New System.Drawing.Size(325, 73)
        Me.GroupBoxMultiplicityVerbalisation.TabIndex = 4
        Me.GroupBoxMultiplicityVerbalisation.TabStop = False
        Me.GroupBoxMultiplicityVerbalisation.Text = "Multiplicity Verbalisation:"
        '
        'LabelMultiplicityVerbalisation
        '
        Me.LabelMultiplicityVerbalisation.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMultiplicityVerbalisation.Location = New System.Drawing.Point(9, 20)
        Me.LabelMultiplicityVerbalisation.Name = "LabelMultiplicityVerbalisation"
        Me.LabelMultiplicityVerbalisation.Size = New System.Drawing.Size(307, 41)
        Me.LabelMultiplicityVerbalisation.TabIndex = 3
        '
        'RadioButtonOneToOne
        '
        Me.RadioButtonOneToOne.AutoSize = True
        Me.RadioButtonOneToOne.Location = New System.Drawing.Point(15, 55)
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
        'ComboBoxAttribute
        '
        Me.ComboBoxAttribute.FormattingEnabled = True
        Me.ComboBoxAttribute.Location = New System.Drawing.Point(96, 19)
        Me.ComboBoxAttribute.Name = "ComboBoxAttribute"
        Me.ComboBoxAttribute.Size = New System.Drawing.Size(289, 21)
        Me.ComboBoxAttribute.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Attribute Name :"
        '
        'ButtonOK
        '
        Me.ButtonOK.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(505, 23)
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
        Me.ButtonCancel.Location = New System.Drawing.Point(505, 52)
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
        Me.GroupBoxFactTypeReading.Location = New System.Drawing.Point(11, 224)
        Me.GroupBoxFactTypeReading.Name = "GroupBoxFactTypeReading"
        Me.GroupBoxFactTypeReading.Size = New System.Drawing.Size(569, 212)
        Me.GroupBoxFactTypeReading.TabIndex = 3
        Me.GroupBoxFactTypeReading.TabStop = False
        Me.GroupBoxFactTypeReading.Text = "Fact Type Reading:"
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
        Me.DataGrid_Readings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid_Readings.Location = New System.Drawing.Point(13, 69)
        Me.DataGrid_Readings.Name = "DataGrid_Readings"
        Me.DataGrid_Readings.Size = New System.Drawing.Size(447, 130)
        Me.DataGrid_Readings.TabIndex = 10
        '
        'TextboxReading
        '
        Me.TextboxReading.AutoWordSelection = True
        Me.TextboxReading.Location = New System.Drawing.Point(13, 29)
        Me.TextboxReading.Multiline = False
        Me.TextboxReading.Name = "TextboxReading"
        Me.TextboxReading.Size = New System.Drawing.Size(447, 34)
        Me.TextboxReading.TabIndex = 8
        Me.TextboxReading.Text = ""
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(466, 29)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(88, 34)
        Me.Button3.TabIndex = 9
        Me.Button3.Text = "&Clear Reading"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'LabelObjectType
        '
        Me.LabelObjectType.AutoSize = True
        Me.LabelObjectType.Location = New System.Drawing.Point(12, 50)
        Me.LabelObjectType.Name = "LabelObjectType"
        Me.LabelObjectType.Size = New System.Drawing.Size(71, 13)
        Me.LabelObjectType.TabIndex = 3
        Me.LabelObjectType.Text = "Object Type :"
        '
        'ComboBoxObjectType
        '
        Me.ComboBoxObjectType.FormattingEnabled = True
        Me.ComboBoxObjectType.Location = New System.Drawing.Point(96, 50)
        Me.ComboBoxObjectType.Name = "ComboBoxObjectType"
        Me.ComboBoxObjectType.Size = New System.Drawing.Size(196, 21)
        Me.ComboBoxObjectType.TabIndex = 4
        '
        'frmCRUDAddAttribute
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 448)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBoxFactTypeReading)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.Controls.Add(Me.ButtonOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "frmCRUDAddAttribute"
        Me.Text = "Add Attribute"
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.GroupBoxMultiplicityConstraint.ResumeLayout(False)
        Me.GroupBoxMultiplicityConstraint.PerformLayout()
        Me.GroupBoxMultiplicityVerbalisation.ResumeLayout(False)
        Me.GroupBoxFactTypeReading.ResumeLayout(False)
        CType(Me.DataGrid_Readings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBoxMain As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ComboBoxAttribute As System.Windows.Forms.ComboBox
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
End Class
