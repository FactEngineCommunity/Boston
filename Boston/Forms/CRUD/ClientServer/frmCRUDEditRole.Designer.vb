<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditRole
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBoxRoleFunction = New System.Windows.Forms.GroupBox()
        Me.ButtonExcludeFunction = New System.Windows.Forms.Button()
        Me.ButtonIncludeFunction = New System.Windows.Forms.Button()
        Me.LabelPromptAvailableFunctions = New System.Windows.Forms.Label()
        Me.ListBoxAvailableFunctions = New System.Windows.Forms.ListBox()
        Me.LabelPromptIncludedFunctions = New System.Windows.Forms.Label()
        Me.ListBoxIncludedFunctions = New System.Windows.Forms.ListBox()
        Me.LabelPromptRoleNumber = New System.Windows.Forms.Label()
        Me.TextBoxRoleName = New System.Windows.Forms.TextBox()
        Me.LabelPromptGroupName = New System.Windows.Forms.Label()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.LabelHelpTips = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxRoleFunction.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.GroupBoxRoleFunction)
        Me.GroupBox1.Controls.Add(Me.LabelPromptRoleNumber)
        Me.GroupBox1.Controls.Add(Me.TextBoxRoleName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptGroupName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(570, 554)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'GroupBoxRoleFunction
        '
        Me.GroupBoxRoleFunction.Controls.Add(Me.LabelHelpTips)
        Me.GroupBoxRoleFunction.Controls.Add(Me.ButtonExcludeFunction)
        Me.GroupBoxRoleFunction.Controls.Add(Me.ButtonIncludeFunction)
        Me.GroupBoxRoleFunction.Controls.Add(Me.LabelPromptAvailableFunctions)
        Me.GroupBoxRoleFunction.Controls.Add(Me.ListBoxAvailableFunctions)
        Me.GroupBoxRoleFunction.Controls.Add(Me.LabelPromptIncludedFunctions)
        Me.GroupBoxRoleFunction.Controls.Add(Me.ListBoxIncludedFunctions)
        Me.GroupBoxRoleFunction.Location = New System.Drawing.Point(12, 51)
        Me.GroupBoxRoleFunction.Name = "GroupBoxRoleFunction"
        Me.GroupBoxRoleFunction.Size = New System.Drawing.Size(541, 497)
        Me.GroupBoxRoleFunction.TabIndex = 3
        Me.GroupBoxRoleFunction.TabStop = False
        Me.GroupBoxRoleFunction.Text = "Included Functions"
        '
        'ButtonExcludeFunction
        '
        Me.ButtonExcludeFunction.Location = New System.Drawing.Point(231, 71)
        Me.ButtonExcludeFunction.Name = "ButtonExcludeFunction"
        Me.ButtonExcludeFunction.Size = New System.Drawing.Size(63, 25)
        Me.ButtonExcludeFunction.TabIndex = 11
        Me.ButtonExcludeFunction.Text = ">>"
        Me.ButtonExcludeFunction.UseVisualStyleBackColor = True
        '
        'ButtonIncludeFunction
        '
        Me.ButtonIncludeFunction.Location = New System.Drawing.Point(231, 40)
        Me.ButtonIncludeFunction.Name = "ButtonIncludeFunction"
        Me.ButtonIncludeFunction.Size = New System.Drawing.Size(64, 25)
        Me.ButtonIncludeFunction.TabIndex = 10
        Me.ButtonIncludeFunction.Text = "<<"
        Me.ButtonIncludeFunction.UseVisualStyleBackColor = True
        '
        'LabelPromptAvailableFunctions
        '
        Me.LabelPromptAvailableFunctions.AutoSize = True
        Me.LabelPromptAvailableFunctions.Location = New System.Drawing.Point(301, 23)
        Me.LabelPromptAvailableFunctions.Name = "LabelPromptAvailableFunctions"
        Me.LabelPromptAvailableFunctions.Size = New System.Drawing.Size(99, 13)
        Me.LabelPromptAvailableFunctions.TabIndex = 9
        Me.LabelPromptAvailableFunctions.Text = "Available Functions"
        '
        'ListBoxAvailableFunctions
        '
        Me.ListBoxAvailableFunctions.FormattingEnabled = True
        Me.ListBoxAvailableFunctions.Location = New System.Drawing.Point(304, 39)
        Me.ListBoxAvailableFunctions.Name = "ListBoxAvailableFunctions"
        Me.ListBoxAvailableFunctions.Size = New System.Drawing.Size(229, 394)
        Me.ListBoxAvailableFunctions.Sorted = True
        Me.ListBoxAvailableFunctions.TabIndex = 8
        '
        'LabelPromptIncludedFunctions
        '
        Me.LabelPromptIncludedFunctions.AutoSize = True
        Me.LabelPromptIncludedFunctions.Location = New System.Drawing.Point(6, 23)
        Me.LabelPromptIncludedFunctions.Name = "LabelPromptIncludedFunctions"
        Me.LabelPromptIncludedFunctions.Size = New System.Drawing.Size(97, 13)
        Me.LabelPromptIncludedFunctions.TabIndex = 7
        Me.LabelPromptIncludedFunctions.Text = "Included Functions"
        '
        'ListBoxIncludedFunctions
        '
        Me.ListBoxIncludedFunctions.FormattingEnabled = True
        Me.ListBoxIncludedFunctions.Location = New System.Drawing.Point(6, 39)
        Me.ListBoxIncludedFunctions.Name = "ListBoxIncludedFunctions"
        Me.ListBoxIncludedFunctions.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxIncludedFunctions.Sorted = True
        Me.ListBoxIncludedFunctions.TabIndex = 6
        '
        'LabelPromptRoleNumber
        '
        Me.LabelPromptRoleNumber.AutoSize = True
        Me.LabelPromptRoleNumber.Location = New System.Drawing.Point(6, 51)
        Me.LabelPromptRoleNumber.Name = "LabelPromptRoleNumber"
        Me.LabelPromptRoleNumber.Size = New System.Drawing.Size(0, 13)
        Me.LabelPromptRoleNumber.TabIndex = 2
        '
        'TextBoxRoleName
        '
        Me.TextBoxRoleName.Location = New System.Drawing.Point(75, 18)
        Me.TextBoxRoleName.Name = "TextBoxRoleName"
        Me.TextBoxRoleName.Size = New System.Drawing.Size(262, 20)
        Me.TextBoxRoleName.TabIndex = 1
        '
        'LabelPromptGroupName
        '
        Me.LabelPromptGroupName.AutoSize = True
        Me.LabelPromptGroupName.Location = New System.Drawing.Point(6, 21)
        Me.LabelPromptGroupName.Name = "LabelPromptGroupName"
        Me.LabelPromptGroupName.Size = New System.Drawing.Size(63, 13)
        Me.LabelPromptGroupName.TabIndex = 0
        Me.LabelPromptGroupName.Text = "Role Name:"
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(588, 22)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(69, 24)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "&OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(588, 52)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(69, 24)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'LabelHelpTips
        '
        Me.LabelHelpTips.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelpTips.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelpTips.Location = New System.Drawing.Point(3, 447)
        Me.LabelHelpTips.Name = "LabelHelpTips"
        Me.LabelHelpTips.Size = New System.Drawing.Size(535, 47)
        Me.LabelHelpTips.TabIndex = 12
        Me.LabelHelpTips.Text = "Label1"
        '
        'frmCRUDEditRole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(768, 587)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDEditRole"
        Me.TabText = "Edit Role"
        Me.Text = "Edit Role"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBoxRoleFunction.ResumeLayout(False)
        Me.GroupBoxRoleFunction.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxRoleName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptGroupName As System.Windows.Forms.Label
    Friend WithEvents ButtonOkay As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents LabelPromptRoleNumber As System.Windows.Forms.Label
    Friend WithEvents GroupBoxRoleFunction As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonExcludeFunction As System.Windows.Forms.Button
    Friend WithEvents ButtonIncludeFunction As System.Windows.Forms.Button
    Friend WithEvents LabelPromptAvailableFunctions As System.Windows.Forms.Label
    Friend WithEvents ListBoxAvailableFunctions As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedFunctions As System.Windows.Forms.Label
    Friend WithEvents ListBoxIncludedFunctions As System.Windows.Forms.ListBox
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents LabelHelpTips As System.Windows.Forms.Label
End Class
