<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenericSelectMultiColumn
    Inherits WeifenLuo.WinFormsUI.Docking.DockContent

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGenericSelectMultiColumn))
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.groupbox_main = New System.Windows.Forms.GroupBox()
        Me.comboboxSelection = New MTGCComboBox()
        Me.ButtonOK = New System.Windows.Forms.Button()
        Me.groupbox_main.SuspendLayout()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(341, 42)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 2
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.comboboxSelection)
        Me.groupbox_main.Location = New System.Drawing.Point(7, 2)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(319, 68)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'comboboxSelection
        '
        Me.comboboxSelection.ArrowBoxColor = System.Drawing.SystemColors.Control
        Me.comboboxSelection.ArrowColor = System.Drawing.Color.Black
        Me.comboboxSelection.BindedControl = CType(resources.GetObject("comboboxSelection.BindedControl"), MTGCComboBox.ControlloAssociato)
        Me.comboboxSelection.BorderStyle = MTGCComboBox.TipiBordi.FlatXP
        Me.comboboxSelection.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.comboboxSelection.ColumnNum = 3
        Me.comboboxSelection.ColumnWidth = "100;100;100"
        Me.comboboxSelection.DisabledArrowBoxColor = System.Drawing.SystemColors.Control
        Me.comboboxSelection.DisabledArrowColor = System.Drawing.Color.LightGray
        Me.comboboxSelection.DisabledBackColor = System.Drawing.SystemColors.Control
        Me.comboboxSelection.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder
        Me.comboboxSelection.DisabledForeColor = System.Drawing.SystemColors.GrayText
        Me.comboboxSelection.DisplayMember = "Text"
        Me.comboboxSelection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.comboboxSelection.DropDownArrowBackColor = System.Drawing.Color.FromArgb(CType(CType(136, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.comboboxSelection.DropDownBackColor = System.Drawing.Color.FromArgb(CType(CType(193, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.comboboxSelection.DropDownForeColor = System.Drawing.Color.Black
        Me.comboboxSelection.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown
        Me.comboboxSelection.DropDownWidth = 320
        Me.comboboxSelection.GridLineColor = System.Drawing.Color.LightGray
        Me.comboboxSelection.GridLineHorizontal = False
        Me.comboboxSelection.GridLineVertical = False
        Me.comboboxSelection.HighlightBorderColor = System.Drawing.Color.Blue
        Me.comboboxSelection.HighlightBorderOnMouseEvents = True
        Me.comboboxSelection.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem
        Me.comboboxSelection.Location = New System.Drawing.Point(15, 28)
        Me.comboboxSelection.ManagingFastMouseMoving = True
        Me.comboboxSelection.ManagingFastMouseMovingInterval = 30
        Me.comboboxSelection.Name = "comboboxSelection"
        Me.comboboxSelection.NormalBorderColor = System.Drawing.Color.Black
        Me.comboboxSelection.SelectedItem = Nothing
        Me.comboboxSelection.SelectedValue = Nothing
        Me.comboboxSelection.Size = New System.Drawing.Size(288, 21)
        Me.comboboxSelection.TabIndex = 0
        '
        'ButtonOK
        '
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(341, 13)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(68, 23)
        Me.ButtonOK.TabIndex = 1
        Me.ButtonOK.Text = "&OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'frmGenericSelectMultiColumn
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.button_cancel
        Me.ClientSize = New System.Drawing.Size(421, 79)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGenericSelectMultiColumn"
        Me.TabText = "Select"
        Me.Text = "Select"
        Me.groupbox_main.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents comboboxSelection As MTGCComboBox
End Class
