<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditUser
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
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.button_okay = New System.Windows.Forms.Button()
        Me.groupbox_main = New System.Windows.Forms.GroupBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TextBoxLastName = New System.Windows.Forms.TextBox()
        Me.TextBoxFirstName = New System.Windows.Forms.TextBox()
        Me.LabelPromptLastName = New System.Windows.Forms.Label()
        Me.LabelPromptFirstName = New System.Windows.Forms.Label()
        Me.LabelPromptIsSuperuser = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.LabelPromptFunctions = New System.Windows.Forms.Label()
        Me.ListBoxFunctions = New System.Windows.Forms.ListBox()
        Me.ButtonExcludeRole = New System.Windows.Forms.Button()
        Me.ButtonIncludeRole = New System.Windows.Forms.Button()
        Me.ListBoxAvailableRoles = New System.Windows.Forms.ListBox()
        Me.ListBoxIncludedRoles = New System.Windows.Forms.ListBox()
        Me.labelprompt_role = New System.Windows.Forms.Label()
        Me.LabelPromptAvailableRoles = New System.Windows.Forms.Label()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.LabelPromptProjectsInGroup = New System.Windows.Forms.Label()
        Me.ListBoxProjectsInGroup = New System.Windows.Forms.ListBox()
        Me.ListBoxIncludedInGroups = New System.Windows.Forms.ListBox()
        Me.LabelPromptIncludedInGroups = New System.Windows.Forms.Label()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.CheckBoxUserPermissionCreate = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionAlter = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionRead = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionNoPermission = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionFullRights = New System.Windows.Forms.CheckBox()
        Me.ListBoxIncludedInProjects = New System.Windows.Forms.ListBox()
        Me.LabelPromptIncludedInProjects = New System.Windows.Forms.Label()
        Me.TabPageAdvanced = New System.Windows.Forms.TabPage()
        Me.checkboxIsActive = New System.Windows.Forms.CheckBox()
        Me.PictureBox_line2 = New System.Windows.Forms.PictureBox()
        Me.labelprompt_confirmation_password = New System.Windows.Forms.Label()
        Me.labelprompt_password = New System.Windows.Forms.Label()
        Me.textbox_confirmation_password = New System.Windows.Forms.TextBox()
        Me.textbox_password = New System.Windows.Forms.TextBox()
        Me.labelprompt_operator_name = New System.Windows.Forms.Label()
        Me.textbox_operator_name = New System.Windows.Forms.TextBox()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.ListBoxWhosInTheGroup = New System.Windows.Forms.ListBox()
        Me.groupbox_main.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPageAdvanced.SuspendLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(914, 65)
        Me.button_cancel.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(102, 40)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button_okay.Location = New System.Drawing.Point(914, 18)
        Me.button_okay.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(104, 37)
        Me.button_okay.TabIndex = 7
        Me.button_okay.Text = "&OK"
        Me.button_okay.UseVisualStyleBackColor = True
        '
        'groupbox_main
        '
        Me.groupbox_main.Controls.Add(Me.PictureBox1)
        Me.groupbox_main.Controls.Add(Me.TextBoxLastName)
        Me.groupbox_main.Controls.Add(Me.TextBoxFirstName)
        Me.groupbox_main.Controls.Add(Me.LabelPromptLastName)
        Me.groupbox_main.Controls.Add(Me.LabelPromptFirstName)
        Me.groupbox_main.Controls.Add(Me.LabelPromptIsSuperuser)
        Me.groupbox_main.Controls.Add(Me.TabControl1)
        Me.groupbox_main.Controls.Add(Me.PictureBox_line2)
        Me.groupbox_main.Controls.Add(Me.labelprompt_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.labelprompt_password)
        Me.groupbox_main.Controls.Add(Me.textbox_confirmation_password)
        Me.groupbox_main.Controls.Add(Me.textbox_password)
        Me.groupbox_main.Controls.Add(Me.labelprompt_operator_name)
        Me.groupbox_main.Controls.Add(Me.textbox_operator_name)
        Me.groupbox_main.Location = New System.Drawing.Point(14, 3)
        Me.groupbox_main.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.groupbox_main.Size = New System.Drawing.Size(879, 1049)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(27, 175)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(491, 0)
        Me.PictureBox1.TabIndex = 19
        Me.PictureBox1.TabStop = False
        '
        'TextBoxLastName
        '
        Me.TextBoxLastName.Location = New System.Drawing.Point(190, 135)
        Me.TextBoxLastName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxLastName.Name = "TextBoxLastName"
        Me.TextBoxLastName.Size = New System.Drawing.Size(289, 26)
        Me.TextBoxLastName.TabIndex = 18
        '
        'TextBoxFirstName
        '
        Me.TextBoxFirstName.Location = New System.Drawing.Point(190, 94)
        Me.TextBoxFirstName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxFirstName.Name = "TextBoxFirstName"
        Me.TextBoxFirstName.Size = New System.Drawing.Size(289, 26)
        Me.TextBoxFirstName.TabIndex = 17
        '
        'LabelPromptLastName
        '
        Me.LabelPromptLastName.AutoSize = True
        Me.LabelPromptLastName.Location = New System.Drawing.Point(86, 140)
        Me.LabelPromptLastName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptLastName.Name = "LabelPromptLastName"
        Me.LabelPromptLastName.Size = New System.Drawing.Size(90, 20)
        Me.LabelPromptLastName.TabIndex = 16
        Me.LabelPromptLastName.Text = "Last Name:"
        '
        'LabelPromptFirstName
        '
        Me.LabelPromptFirstName.AutoSize = True
        Me.LabelPromptFirstName.Location = New System.Drawing.Point(87, 98)
        Me.LabelPromptFirstName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptFirstName.Name = "LabelPromptFirstName"
        Me.LabelPromptFirstName.Size = New System.Drawing.Size(90, 20)
        Me.LabelPromptFirstName.TabIndex = 15
        Me.LabelPromptFirstName.Text = "First Name:"
        '
        'LabelPromptIsSuperuser
        '
        Me.LabelPromptIsSuperuser.AutoSize = True
        Me.LabelPromptIsSuperuser.Location = New System.Drawing.Point(490, 45)
        Me.LabelPromptIsSuperuser.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptIsSuperuser.Name = "LabelPromptIsSuperuser"
        Me.LabelPromptIsSuperuser.Size = New System.Drawing.Size(83, 20)
        Me.LabelPromptIsSuperuser.TabIndex = 14
        Me.LabelPromptIsSuperuser.Text = "Superuser"
        Me.LabelPromptIsSuperuser.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPageAdvanced)
        Me.TabControl1.Location = New System.Drawing.Point(27, 298)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(840, 722)
        Me.TabControl1.TabIndex = 13
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.LabelPromptFunctions)
        Me.TabPage1.Controls.Add(Me.ListBoxFunctions)
        Me.TabPage1.Controls.Add(Me.ButtonExcludeRole)
        Me.TabPage1.Controls.Add(Me.ButtonIncludeRole)
        Me.TabPage1.Controls.Add(Me.ListBoxAvailableRoles)
        Me.TabPage1.Controls.Add(Me.ListBoxIncludedRoles)
        Me.TabPage1.Controls.Add(Me.labelprompt_role)
        Me.TabPage1.Controls.Add(Me.LabelPromptAvailableRoles)
        Me.TabPage1.Location = New System.Drawing.Point(4, 29)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage1.Size = New System.Drawing.Size(832, 689)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Included Roles"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'LabelPromptFunctions
        '
        Me.LabelPromptFunctions.AutoSize = True
        Me.LabelPromptFunctions.Location = New System.Drawing.Point(9, 449)
        Me.LabelPromptFunctions.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptFunctions.Name = "LabelPromptFunctions"
        Me.LabelPromptFunctions.Size = New System.Drawing.Size(83, 20)
        Me.LabelPromptFunctions.TabIndex = 17
        Me.LabelPromptFunctions.Text = "Functions:"
        '
        'ListBoxFunctions
        '
        Me.ListBoxFunctions.FormattingEnabled = True
        Me.ListBoxFunctions.ItemHeight = 20
        Me.ListBoxFunctions.Location = New System.Drawing.Point(14, 474)
        Me.ListBoxFunctions.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxFunctions.Name = "ListBoxFunctions"
        Me.ListBoxFunctions.Size = New System.Drawing.Size(788, 184)
        Me.ListBoxFunctions.Sorted = True
        Me.ListBoxFunctions.TabIndex = 16
        '
        'ButtonExcludeRole
        '
        Me.ButtonExcludeRole.Location = New System.Drawing.Point(351, 103)
        Me.ButtonExcludeRole.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ButtonExcludeRole.Name = "ButtonExcludeRole"
        Me.ButtonExcludeRole.Size = New System.Drawing.Size(94, 38)
        Me.ButtonExcludeRole.TabIndex = 15
        Me.ButtonExcludeRole.Text = ">>"
        Me.ButtonExcludeRole.UseVisualStyleBackColor = True
        '
        'ButtonIncludeRole
        '
        Me.ButtonIncludeRole.Location = New System.Drawing.Point(351, 55)
        Me.ButtonIncludeRole.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ButtonIncludeRole.Name = "ButtonIncludeRole"
        Me.ButtonIncludeRole.Size = New System.Drawing.Size(96, 38)
        Me.ButtonIncludeRole.TabIndex = 14
        Me.ButtonIncludeRole.Text = "<<"
        Me.ButtonIncludeRole.UseVisualStyleBackColor = True
        '
        'ListBoxAvailableRoles
        '
        Me.ListBoxAvailableRoles.FormattingEnabled = True
        Me.ListBoxAvailableRoles.ItemHeight = 20
        Me.ListBoxAvailableRoles.Location = New System.Drawing.Point(460, 54)
        Me.ListBoxAvailableRoles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxAvailableRoles.Name = "ListBoxAvailableRoles"
        Me.ListBoxAvailableRoles.Size = New System.Drawing.Size(342, 384)
        Me.ListBoxAvailableRoles.Sorted = True
        Me.ListBoxAvailableRoles.TabIndex = 13
        '
        'ListBoxIncludedRoles
        '
        Me.ListBoxIncludedRoles.FormattingEnabled = True
        Me.ListBoxIncludedRoles.ItemHeight = 20
        Me.ListBoxIncludedRoles.Location = New System.Drawing.Point(14, 54)
        Me.ListBoxIncludedRoles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxIncludedRoles.Name = "ListBoxIncludedRoles"
        Me.ListBoxIncludedRoles.Size = New System.Drawing.Size(320, 384)
        Me.ListBoxIncludedRoles.Sorted = True
        Me.ListBoxIncludedRoles.TabIndex = 12
        '
        'labelprompt_role
        '
        Me.labelprompt_role.AutoSize = True
        Me.labelprompt_role.Location = New System.Drawing.Point(9, 23)
        Me.labelprompt_role.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelprompt_role.Name = "labelprompt_role"
        Me.labelprompt_role.Size = New System.Drawing.Size(119, 20)
        Me.labelprompt_role.TabIndex = 3
        Me.labelprompt_role.Text = "Included Roles:"
        '
        'LabelPromptAvailableRoles
        '
        Me.LabelPromptAvailableRoles.AutoSize = True
        Me.LabelPromptAvailableRoles.Location = New System.Drawing.Point(458, 23)
        Me.LabelPromptAvailableRoles.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptAvailableRoles.Name = "LabelPromptAvailableRoles"
        Me.LabelPromptAvailableRoles.Size = New System.Drawing.Size(121, 20)
        Me.LabelPromptAvailableRoles.TabIndex = 11
        Me.LabelPromptAvailableRoles.Text = "Available Roles:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TabControl2)
        Me.TabPage3.Controls.Add(Me.ListBoxIncludedInGroups)
        Me.TabPage3.Controls.Add(Me.LabelPromptIncludedInGroups)
        Me.TabPage3.Location = New System.Drawing.Point(4, 29)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(832, 689)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Groups"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'LabelPromptProjectsInGroup
        '
        Me.LabelPromptProjectsInGroup.AutoSize = True
        Me.LabelPromptProjectsInGroup.Location = New System.Drawing.Point(22, 22)
        Me.LabelPromptProjectsInGroup.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptProjectsInGroup.Name = "LabelPromptProjectsInGroup"
        Me.LabelPromptProjectsInGroup.Size = New System.Drawing.Size(233, 20)
        Me.LabelPromptProjectsInGroup.TabIndex = 3
        Me.LabelPromptProjectsInGroup.Text = "Projects for the selected Group:"
        '
        'ListBoxProjectsInGroup
        '
        Me.ListBoxProjectsInGroup.FormattingEnabled = True
        Me.ListBoxProjectsInGroup.ItemHeight = 20
        Me.ListBoxProjectsInGroup.Location = New System.Drawing.Point(27, 46)
        Me.ListBoxProjectsInGroup.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxProjectsInGroup.Name = "ListBoxProjectsInGroup"
        Me.ListBoxProjectsInGroup.Size = New System.Drawing.Size(709, 204)
        Me.ListBoxProjectsInGroup.TabIndex = 2
        '
        'ListBoxIncludedInGroups
        '
        Me.ListBoxIncludedInGroups.FormattingEnabled = True
        Me.ListBoxIncludedInGroups.ItemHeight = 20
        Me.ListBoxIncludedInGroups.Location = New System.Drawing.Point(33, 51)
        Me.ListBoxIncludedInGroups.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxIncludedInGroups.Name = "ListBoxIncludedInGroups"
        Me.ListBoxIncludedInGroups.Size = New System.Drawing.Size(763, 284)
        Me.ListBoxIncludedInGroups.Sorted = True
        Me.ListBoxIncludedInGroups.TabIndex = 1
        '
        'LabelPromptIncludedInGroups
        '
        Me.LabelPromptIncludedInGroups.AutoSize = True
        Me.LabelPromptIncludedInGroups.Location = New System.Drawing.Point(28, 26)
        Me.LabelPromptIncludedInGroups.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptIncludedInGroups.Name = "LabelPromptIncludedInGroups"
        Me.LabelPromptIncludedInGroups.Size = New System.Drawing.Size(151, 20)
        Me.LabelPromptIncludedInGroups.TabIndex = 0
        Me.LabelPromptIncludedInGroups.Text = "Included in Group/s:"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.CheckBoxUserPermissionCreate)
        Me.TabPage4.Controls.Add(Me.CheckBoxUserPermissionAlter)
        Me.TabPage4.Controls.Add(Me.CheckBoxUserPermissionRead)
        Me.TabPage4.Controls.Add(Me.CheckBoxUserPermissionNoPermission)
        Me.TabPage4.Controls.Add(Me.CheckBoxUserPermissionFullRights)
        Me.TabPage4.Controls.Add(Me.ListBoxIncludedInProjects)
        Me.TabPage4.Controls.Add(Me.LabelPromptIncludedInProjects)
        Me.TabPage4.Location = New System.Drawing.Point(4, 29)
        Me.TabPage4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(832, 689)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Projects"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionCreate
        '
        Me.CheckBoxUserPermissionCreate.AutoSize = True
        Me.CheckBoxUserPermissionCreate.Enabled = False
        Me.CheckBoxUserPermissionCreate.Location = New System.Drawing.Point(34, 575)
        Me.CheckBoxUserPermissionCreate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBoxUserPermissionCreate.Name = "CheckBoxUserPermissionCreate"
        Me.CheckBoxUserPermissionCreate.Size = New System.Drawing.Size(83, 24)
        Me.CheckBoxUserPermissionCreate.TabIndex = 11
        Me.CheckBoxUserPermissionCreate.Text = "Create"
        Me.CheckBoxUserPermissionCreate.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionAlter
        '
        Me.CheckBoxUserPermissionAlter.AutoSize = True
        Me.CheckBoxUserPermissionAlter.Enabled = False
        Me.CheckBoxUserPermissionAlter.Location = New System.Drawing.Point(34, 646)
        Me.CheckBoxUserPermissionAlter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBoxUserPermissionAlter.Name = "CheckBoxUserPermissionAlter"
        Me.CheckBoxUserPermissionAlter.Size = New System.Drawing.Size(68, 24)
        Me.CheckBoxUserPermissionAlter.TabIndex = 10
        Me.CheckBoxUserPermissionAlter.Text = "Alter"
        Me.CheckBoxUserPermissionAlter.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionRead
        '
        Me.CheckBoxUserPermissionRead.AutoSize = True
        Me.CheckBoxUserPermissionRead.Enabled = False
        Me.CheckBoxUserPermissionRead.Location = New System.Drawing.Point(34, 611)
        Me.CheckBoxUserPermissionRead.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBoxUserPermissionRead.Name = "CheckBoxUserPermissionRead"
        Me.CheckBoxUserPermissionRead.Size = New System.Drawing.Size(74, 24)
        Me.CheckBoxUserPermissionRead.TabIndex = 9
        Me.CheckBoxUserPermissionRead.Text = "Read"
        Me.CheckBoxUserPermissionRead.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionNoPermission
        '
        Me.CheckBoxUserPermissionNoPermission.AutoSize = True
        Me.CheckBoxUserPermissionNoPermission.Enabled = False
        Me.CheckBoxUserPermissionNoPermission.Location = New System.Drawing.Point(34, 540)
        Me.CheckBoxUserPermissionNoPermission.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBoxUserPermissionNoPermission.Name = "CheckBoxUserPermissionNoPermission"
        Me.CheckBoxUserPermissionNoPermission.Size = New System.Drawing.Size(136, 24)
        Me.CheckBoxUserPermissionNoPermission.TabIndex = 8
        Me.CheckBoxUserPermissionNoPermission.Text = "No Permission"
        Me.CheckBoxUserPermissionNoPermission.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionFullRights
        '
        Me.CheckBoxUserPermissionFullRights.AutoSize = True
        Me.CheckBoxUserPermissionFullRights.Enabled = False
        Me.CheckBoxUserPermissionFullRights.Location = New System.Drawing.Point(34, 505)
        Me.CheckBoxUserPermissionFullRights.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBoxUserPermissionFullRights.Name = "CheckBoxUserPermissionFullRights"
        Me.CheckBoxUserPermissionFullRights.Size = New System.Drawing.Size(110, 24)
        Me.CheckBoxUserPermissionFullRights.TabIndex = 7
        Me.CheckBoxUserPermissionFullRights.Text = "Full Rights"
        Me.CheckBoxUserPermissionFullRights.UseVisualStyleBackColor = True
        '
        'ListBoxIncludedInProjects
        '
        Me.ListBoxIncludedInProjects.FormattingEnabled = True
        Me.ListBoxIncludedInProjects.ItemHeight = 20
        Me.ListBoxIncludedInProjects.Location = New System.Drawing.Point(34, 49)
        Me.ListBoxIncludedInProjects.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBoxIncludedInProjects.Name = "ListBoxIncludedInProjects"
        Me.ListBoxIncludedInProjects.Size = New System.Drawing.Size(763, 444)
        Me.ListBoxIncludedInProjects.Sorted = True
        Me.ListBoxIncludedInProjects.TabIndex = 3
        '
        'LabelPromptIncludedInProjects
        '
        Me.LabelPromptIncludedInProjects.AutoSize = True
        Me.LabelPromptIncludedInProjects.Location = New System.Drawing.Point(30, 25)
        Me.LabelPromptIncludedInProjects.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LabelPromptIncludedInProjects.Name = "LabelPromptIncludedInProjects"
        Me.LabelPromptIncludedInProjects.Size = New System.Drawing.Size(155, 20)
        Me.LabelPromptIncludedInProjects.TabIndex = 2
        Me.LabelPromptIncludedInProjects.Text = "Included in Project/s:"
        '
        'TabPageAdvanced
        '
        Me.TabPageAdvanced.Controls.Add(Me.checkboxIsActive)
        Me.TabPageAdvanced.Location = New System.Drawing.Point(4, 29)
        Me.TabPageAdvanced.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageAdvanced.Name = "TabPageAdvanced"
        Me.TabPageAdvanced.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPageAdvanced.Size = New System.Drawing.Size(832, 689)
        Me.TabPageAdvanced.TabIndex = 1
        Me.TabPageAdvanced.Text = "Advanced"
        Me.TabPageAdvanced.UseVisualStyleBackColor = True
        '
        'checkboxIsActive
        '
        Me.checkboxIsActive.AutoSize = True
        Me.checkboxIsActive.Location = New System.Drawing.Point(27, 25)
        Me.checkboxIsActive.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.checkboxIsActive.Name = "checkboxIsActive"
        Me.checkboxIsActive.Size = New System.Drawing.Size(131, 24)
        Me.checkboxIsActive.TabIndex = 10
        Me.checkboxIsActive.Text = "User is Active"
        Me.checkboxIsActive.UseVisualStyleBackColor = True
        '
        'PictureBox_line2
        '
        Me.PictureBox_line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox_line2.Location = New System.Drawing.Point(30, 288)
        Me.PictureBox_line2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PictureBox_line2.Name = "PictureBox_line2"
        Me.PictureBox_line2.Size = New System.Drawing.Size(491, 0)
        Me.PictureBox_line2.TabIndex = 9
        Me.PictureBox_line2.TabStop = False
        '
        'labelprompt_confirmation_password
        '
        Me.labelprompt_confirmation_password.AutoSize = True
        Me.labelprompt_confirmation_password.Location = New System.Drawing.Point(33, 238)
        Me.labelprompt_confirmation_password.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelprompt_confirmation_password.Name = "labelprompt_confirmation_password"
        Me.labelprompt_confirmation_password.Size = New System.Drawing.Size(145, 20)
        Me.labelprompt_confirmation_password.TabIndex = 8
        Me.labelprompt_confirmation_password.Text = "Confirm Password :"
        Me.labelprompt_confirmation_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'labelprompt_password
        '
        Me.labelprompt_password.AutoSize = True
        Me.labelprompt_password.Location = New System.Drawing.Point(90, 198)
        Me.labelprompt_password.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelprompt_password.Name = "labelprompt_password"
        Me.labelprompt_password.Size = New System.Drawing.Size(86, 20)
        Me.labelprompt_password.TabIndex = 7
        Me.labelprompt_password.Text = "Password :"
        Me.labelprompt_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'textbox_confirmation_password
        '
        Me.textbox_confirmation_password.Location = New System.Drawing.Point(188, 234)
        Me.textbox_confirmation_password.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.textbox_confirmation_password.Name = "textbox_confirmation_password"
        Me.textbox_confirmation_password.Size = New System.Drawing.Size(290, 26)
        Me.textbox_confirmation_password.TabIndex = 6
        '
        'textbox_password
        '
        Me.textbox_password.Location = New System.Drawing.Point(188, 194)
        Me.textbox_password.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.textbox_password.Name = "textbox_password"
        Me.textbox_password.Size = New System.Drawing.Size(292, 26)
        Me.textbox_password.TabIndex = 5
        '
        'labelprompt_operator_name
        '
        Me.labelprompt_operator_name.AutoSize = True
        Me.labelprompt_operator_name.Location = New System.Drawing.Point(87, 45)
        Me.labelprompt_operator_name.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.labelprompt_operator_name.Name = "labelprompt_operator_name"
        Me.labelprompt_operator_name.Size = New System.Drawing.Size(91, 20)
        Me.labelprompt_operator_name.TabIndex = 1
        Me.labelprompt_operator_name.Text = "Username :"
        '
        'textbox_operator_name
        '
        Me.textbox_operator_name.Location = New System.Drawing.Point(188, 40)
        Me.textbox_operator_name.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.textbox_operator_name.Name = "textbox_operator_name"
        Me.textbox_operator_name.Size = New System.Drawing.Size(292, 26)
        Me.textbox_operator_name.TabIndex = 0
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage2)
        Me.TabControl2.Controls.Add(Me.TabPage5)
        Me.TabControl2.Location = New System.Drawing.Point(33, 356)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(763, 302)
        Me.TabControl2.TabIndex = 4
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ListBoxProjectsInGroup)
        Me.TabPage2.Controls.Add(Me.LabelPromptProjectsInGroup)
        Me.TabPage2.Location = New System.Drawing.Point(4, 29)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(755, 269)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Projects"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.ListBoxWhosInTheGroup)
        Me.TabPage5.Location = New System.Drawing.Point(4, 29)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(755, 269)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Who's in the Group?"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'ListBoxWhosInTheGroup
        '
        Me.ListBoxWhosInTheGroup.FormattingEnabled = True
        Me.ListBoxWhosInTheGroup.ItemHeight = 20
        Me.ListBoxWhosInTheGroup.Location = New System.Drawing.Point(13, 12)
        Me.ListBoxWhosInTheGroup.Name = "ListBoxWhosInTheGroup"
        Me.ListBoxWhosInTheGroup.Size = New System.Drawing.Size(729, 244)
        Me.ListBoxWhosInTheGroup.TabIndex = 5
        '
        'frmCRUDEditUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1095, 1050)
        Me.ControlBox = False
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmCRUDEditUser"
        Me.TabText = "Edit User Profile"
        Me.Text = "Edit User Profile"
        Me.groupbox_main.ResumeLayout(False)
        Me.groupbox_main.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPageAdvanced.ResumeLayout(False)
        Me.TabPageAdvanced.PerformLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_okay As System.Windows.Forms.Button
    Friend WithEvents groupbox_main As System.Windows.Forms.GroupBox
    Friend WithEvents textbox_confirmation_password As System.Windows.Forms.TextBox
    Friend WithEvents textbox_password As System.Windows.Forms.TextBox
    Friend WithEvents labelprompt_operator_name As System.Windows.Forms.Label
    Friend WithEvents textbox_operator_name As System.Windows.Forms.TextBox
    Friend WithEvents checkboxIsActive As System.Windows.Forms.CheckBox
    Friend WithEvents PictureBox_line2 As System.Windows.Forms.PictureBox
    Friend WithEvents labelprompt_confirmation_password As System.Windows.Forms.Label
    Friend WithEvents labelprompt_password As System.Windows.Forms.Label
    Friend WithEvents LabelPromptAvailableRoles As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPageAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents LabelPromptIsSuperuser As System.Windows.Forms.Label
    Friend WithEvents TextBoxLastName As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxFirstName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptLastName As System.Windows.Forms.Label
    Friend WithEvents LabelPromptFirstName As System.Windows.Forms.Label
    Friend WithEvents labelprompt_role As System.Windows.Forms.Label
    Friend WithEvents ButtonExcludeRole As System.Windows.Forms.Button
    Friend WithEvents ButtonIncludeRole As System.Windows.Forms.Button
    Friend WithEvents ListBoxAvailableRoles As System.Windows.Forms.ListBox
    Friend WithEvents ListBoxIncludedRoles As System.Windows.Forms.ListBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents ListBoxIncludedInGroups As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedInGroups As System.Windows.Forms.Label
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ListBoxIncludedInProjects As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedInProjects As System.Windows.Forms.Label
    Friend WithEvents LabelPromptFunctions As System.Windows.Forms.Label
    Friend WithEvents ListBoxFunctions As System.Windows.Forms.ListBox
    Friend WithEvents CheckBoxUserPermissionCreate As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionAlter As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionRead As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionNoPermission As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionFullRights As System.Windows.Forms.CheckBox
    Friend WithEvents LabelPromptProjectsInGroup As System.Windows.Forms.Label
    Friend WithEvents ListBoxProjectsInGroup As System.Windows.Forms.ListBox
    Friend WithEvents TabControl2 As TabControl
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage5 As TabPage
    Friend WithEvents ListBoxWhosInTheGroup As ListBox
End Class
