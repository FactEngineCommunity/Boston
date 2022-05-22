<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCRUDAddAttributeNew
    Inherits System.Windows.Forms.Form

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
        Dim NameLabel As System.Windows.Forms.Label
        Dim DataTypeLabel As System.Windows.Forms.Label
        Me.GroupBoxMain = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LabelPromptAttributeOk = New System.Windows.Forms.Label()
        Me.TextBoxDataTypePrecision = New System.Windows.Forms.TextBox()
        Me.LabelPromptPrecision = New System.Windows.Forms.Label()
        Me.TextBoxDataTypeLength = New System.Windows.Forms.TextBox()
        Me.LabelPromptLength = New System.Windows.Forms.Label()
        Me.CheckBoxIsMandatory = New System.Windows.Forms.CheckBox()
        Me.ComboBoxDataType = New System.Windows.Forms.ComboBox()
        Me.ComboBoxAttribute = New System.Windows.Forms.ComboBox()
        Me.LabelEntityTypeName = New System.Windows.Forms.Label()
        Me.LabelPromptEntityTypeName = New System.Windows.Forms.Label()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.LabelHelp = New System.Windows.Forms.Label()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        NameLabel = New System.Windows.Forms.Label()
        DataTypeLabel = New System.Windows.Forms.Label()
        Me.GroupBoxMain.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.GroupBoxMain.Controls.Add(Me.LabelEntityTypeName)
        Me.GroupBoxMain.Controls.Add(Me.LabelPromptEntityTypeName)
        Me.GroupBoxMain.Location = New System.Drawing.Point(3, 2)
        Me.GroupBoxMain.Name = "GroupBoxMain"
        Me.GroupBoxMain.Size = New System.Drawing.Size(739, 239)
        Me.GroupBoxMain.TabIndex = 0
        Me.GroupBoxMain.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.LabelPromptAttributeOk)
        Me.GroupBox1.Controls.Add(Me.TextBoxDataTypePrecision)
        Me.GroupBox1.Controls.Add(Me.LabelPromptPrecision)
        Me.GroupBox1.Controls.Add(Me.TextBoxDataTypeLength)
        Me.GroupBox1.Controls.Add(Me.LabelPromptLength)
        Me.GroupBox1.Controls.Add(Me.CheckBoxIsMandatory)
        Me.GroupBox1.Controls.Add(DataTypeLabel)
        Me.GroupBox1.Controls.Add(Me.ComboBoxDataType)
        Me.GroupBox1.Controls.Add(NameLabel)
        Me.GroupBox1.Controls.Add(Me.ComboBoxAttribute)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 51)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(709, 182)
        Me.GroupBox1.TabIndex = 20
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Attribute Details :"
        '
        'LabelPromptAttributeOk
        '
        Me.LabelPromptAttributeOk.AutoSize = True
        Me.LabelPromptAttributeOk.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptAttributeOk.Location = New System.Drawing.Point(325, 33)
        Me.LabelPromptAttributeOk.MaximumSize = New System.Drawing.Size(300, 0)
        Me.LabelPromptAttributeOk.Name = "LabelPromptAttributeOk"
        Me.LabelPromptAttributeOk.Size = New System.Drawing.Size(122, 15)
        Me.LabelPromptAttributeOk.TabIndex = 27
        Me.LabelPromptAttributeOk.Text = "LabelPromptAttributeOk"
        '
        'TextBoxDataTypePrecision
        '
        Me.TextBoxDataTypePrecision.Location = New System.Drawing.Point(79, 144)
        Me.TextBoxDataTypePrecision.Name = "TextBoxDataTypePrecision"
        Me.TextBoxDataTypePrecision.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDataTypePrecision.TabIndex = 26
        '
        'LabelPromptPrecision
        '
        Me.LabelPromptPrecision.AutoSize = True
        Me.LabelPromptPrecision.Location = New System.Drawing.Point(20, 147)
        Me.LabelPromptPrecision.Name = "LabelPromptPrecision"
        Me.LabelPromptPrecision.Size = New System.Drawing.Size(53, 13)
        Me.LabelPromptPrecision.TabIndex = 25
        Me.LabelPromptPrecision.Text = "Precision:"
        '
        'TextBoxDataTypeLength
        '
        Me.TextBoxDataTypeLength.Location = New System.Drawing.Point(79, 116)
        Me.TextBoxDataTypeLength.Name = "TextBoxDataTypeLength"
        Me.TextBoxDataTypeLength.Size = New System.Drawing.Size(100, 20)
        Me.TextBoxDataTypeLength.TabIndex = 24
        '
        'LabelPromptLength
        '
        Me.LabelPromptLength.AutoSize = True
        Me.LabelPromptLength.Location = New System.Drawing.Point(30, 119)
        Me.LabelPromptLength.Name = "LabelPromptLength"
        Me.LabelPromptLength.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptLength.TabIndex = 23
        Me.LabelPromptLength.Text = "Length:"
        '
        'CheckBoxIsMandatory
        '
        Me.CheckBoxIsMandatory.Location = New System.Drawing.Point(79, 88)
        Me.CheckBoxIsMandatory.Name = "CheckBoxIsMandatory"
        Me.CheckBoxIsMandatory.Size = New System.Drawing.Size(104, 24)
        Me.CheckBoxIsMandatory.TabIndex = 22
        Me.CheckBoxIsMandatory.Text = "Is &Mandatory"
        Me.CheckBoxIsMandatory.UseVisualStyleBackColor = True
        '
        'ComboBoxDataType
        '
        Me.ComboBoxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDataType.FormattingEnabled = True
        Me.ComboBoxDataType.Location = New System.Drawing.Point(79, 61)
        Me.ComboBoxDataType.Name = "ComboBoxDataType"
        Me.ComboBoxDataType.Size = New System.Drawing.Size(182, 21)
        Me.ComboBoxDataType.TabIndex = 21
        '
        'ComboBoxAttribute
        '
        Me.ComboBoxAttribute.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxAttribute.ForeColor = System.Drawing.Color.SteelBlue
        Me.ComboBoxAttribute.FormattingEnabled = True
        Me.ComboBoxAttribute.Location = New System.Drawing.Point(79, 29)
        Me.ComboBoxAttribute.Name = "ComboBoxAttribute"
        Me.ComboBoxAttribute.Size = New System.Drawing.Size(240, 23)
        Me.ComboBoxAttribute.TabIndex = 20
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
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(769, 51)
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
        Me.LabelHelp.Location = New System.Drawing.Point(0, 244)
        Me.LabelHelp.Name = "LabelHelp"
        Me.LabelHelp.Size = New System.Drawing.Size(856, 66)
        Me.LabelHelp.TabIndex = 12
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(769, 20)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOkay.TabIndex = 13
        Me.ButtonOkay.Text = "&Okay"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'frmCRUDAddAttributeNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(856, 310)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.LabelHelp)
        Me.Controls.Add(Me.GroupBoxMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Name = "frmCRUDAddAttributeNew"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Attribute/s"
        Me.GroupBoxMain.ResumeLayout(False)
        Me.GroupBoxMain.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBoxMain As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents LabelEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptEntityTypeName As System.Windows.Forms.Label
    Friend WithEvents LabelHelp As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBoxIsMandatory As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxDataType As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxAttribute As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxDataTypeLength As TextBox
    Friend WithEvents LabelPromptLength As Label
    Friend WithEvents TextBoxDataTypePrecision As TextBox
    Friend WithEvents LabelPromptPrecision As Label
    Friend WithEvents LabelPromptAttributeOk As Label
    Friend WithEvents ButtonOkay As Button
End Class
