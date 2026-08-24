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
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ListBoxProjectsInGroup = New System.Windows.Forms.ListBox()
        Me.LabelPromptProjectsInGroup = New System.Windows.Forms.Label()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.ListBoxWhosInTheGroup = New System.Windows.Forms.ListBox()
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
        Me.TabPagePesonalisation = New System.Windows.Forms.TabPage()
        Me.CheckBoxShowTheBox = New System.Windows.Forms.CheckBox()
        Me.CheckBoxShowVirtualAnalyst = New System.Windows.Forms.CheckBox()
        Me.CheckBoxShowEnterpriseExplorer = New System.Windows.Forms.CheckBox()
        Me.ComboBoxDefaultTask = New System.Windows.Forms.ComboBox()
        Me.LabelPromptDefaultTask = New System.Windows.Forms.Label()
        Me.TextBoxLogoFileLocation = New System.Windows.Forms.TextBox()
        Me.LabelPromptLogoFileLocation = New System.Windows.Forms.Label()
        Me.TabPageAuthentication = New System.Windows.Forms.TabPage()
        Me.TextBoxCallInPIN = New System.Windows.Forms.TextBox()
        Me.LabelPromptCallInPIN = New System.Windows.Forms.Label()
        Me.PictureBox_line2 = New System.Windows.Forms.PictureBox()
        Me.labelprompt_confirmation_password = New System.Windows.Forms.Label()
        Me.labelprompt_password = New System.Windows.Forms.Label()
        Me.textbox_confirmation_password = New System.Windows.Forms.TextBox()
        Me.textbox_password = New System.Windows.Forms.TextBox()
        Me.labelprompt_operator_name = New System.Windows.Forms.Label()
        Me.textbox_operator_name = New System.Windows.Forms.TextBox()
        Me.ButtonMakeDemoUser = New System.Windows.Forms.Button()
        Me.LabelPrompDefaultBostonApplication = New System.Windows.Forms.Label()
        Me.ComboBoxDefaultBostonApplication = New System.Windows.Forms.ComboBox()
        Me.groupbox_main.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPageAdvanced.SuspendLayout()
        Me.TabPagePesonalisation.SuspendLayout()
        Me.TabPageAuthentication.SuspendLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(609, 42)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_okay
        '
        Me.button_okay.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.button_okay.Location = New System.Drawing.Point(609, 12)
        Me.button_okay.Name = "button_okay"
        Me.button_okay.Size = New System.Drawing.Size(69, 24)
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
        Me.groupbox_main.Location = New System.Drawing.Point(9, 2)
        Me.groupbox_main.Name = "groupbox_main"
        Me.groupbox_main.Size = New System.Drawing.Size(586, 682)
        Me.groupbox_main.TabIndex = 6
        Me.groupbox_main.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(18, 114)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox1.TabIndex = 19
        Me.PictureBox1.TabStop = False
        '
        'TextBoxLastName
        '
        Me.TextBoxLastName.Location = New System.Drawing.Point(127, 88)
        Me.TextBoxLastName.Name = "TextBoxLastName"
        Me.TextBoxLastName.Size = New System.Drawing.Size(194, 20)
        Me.TextBoxLastName.TabIndex = 18
        '
        'TextBoxFirstName
        '
        Me.TextBoxFirstName.Location = New System.Drawing.Point(127, 61)
        Me.TextBoxFirstName.Name = "TextBoxFirstName"
        Me.TextBoxFirstName.Size = New System.Drawing.Size(194, 20)
        Me.TextBoxFirstName.TabIndex = 17
        '
        'LabelPromptLastName
        '
        Me.LabelPromptLastName.AutoSize = True
        Me.LabelPromptLastName.Location = New System.Drawing.Point(57, 91)
        Me.LabelPromptLastName.Name = "LabelPromptLastName"
        Me.LabelPromptLastName.Size = New System.Drawing.Size(61, 13)
        Me.LabelPromptLastName.TabIndex = 16
        Me.LabelPromptLastName.Text = "Last Name:"
        '
        'LabelPromptFirstName
        '
        Me.LabelPromptFirstName.AutoSize = True
        Me.LabelPromptFirstName.Location = New System.Drawing.Point(58, 64)
        Me.LabelPromptFirstName.Name = "LabelPromptFirstName"
        Me.LabelPromptFirstName.Size = New System.Drawing.Size(60, 13)
        Me.LabelPromptFirstName.TabIndex = 15
        Me.LabelPromptFirstName.Text = "First Name:"
        '
        'LabelPromptIsSuperuser
        '
        Me.LabelPromptIsSuperuser.AutoSize = True
        Me.LabelPromptIsSuperuser.Location = New System.Drawing.Point(327, 29)
        Me.LabelPromptIsSuperuser.Name = "LabelPromptIsSuperuser"
        Me.LabelPromptIsSuperuser.Size = New System.Drawing.Size(55, 13)
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
        Me.TabControl1.Controls.Add(Me.TabPagePesonalisation)
        Me.TabControl1.Controls.Add(Me.TabPageAuthentication)
        Me.TabControl1.Location = New System.Drawing.Point(18, 194)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(560, 469)
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
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(552, 443)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Included Roles"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'LabelPromptFunctions
        '
        Me.LabelPromptFunctions.AutoSize = True
        Me.LabelPromptFunctions.Location = New System.Drawing.Point(6, 292)
        Me.LabelPromptFunctions.Name = "LabelPromptFunctions"
        Me.LabelPromptFunctions.Size = New System.Drawing.Size(56, 13)
        Me.LabelPromptFunctions.TabIndex = 17
        Me.LabelPromptFunctions.Text = "Functions:"
        '
        'ListBoxFunctions
        '
        Me.ListBoxFunctions.FormattingEnabled = True
        Me.ListBoxFunctions.Location = New System.Drawing.Point(9, 308)
        Me.ListBoxFunctions.Name = "ListBoxFunctions"
        Me.ListBoxFunctions.Size = New System.Drawing.Size(527, 121)
        Me.ListBoxFunctions.Sorted = True
        Me.ListBoxFunctions.TabIndex = 16
        '
        'ButtonExcludeRole
        '
        Me.ButtonExcludeRole.Location = New System.Drawing.Point(234, 67)
        Me.ButtonExcludeRole.Name = "ButtonExcludeRole"
        Me.ButtonExcludeRole.Size = New System.Drawing.Size(63, 25)
        Me.ButtonExcludeRole.TabIndex = 15
        Me.ButtonExcludeRole.Text = ">>"
        Me.ButtonExcludeRole.UseVisualStyleBackColor = True
        '
        'ButtonIncludeRole
        '
        Me.ButtonIncludeRole.Location = New System.Drawing.Point(234, 36)
        Me.ButtonIncludeRole.Name = "ButtonIncludeRole"
        Me.ButtonIncludeRole.Size = New System.Drawing.Size(64, 25)
        Me.ButtonIncludeRole.TabIndex = 14
        Me.ButtonIncludeRole.Text = "<<"
        Me.ButtonIncludeRole.UseVisualStyleBackColor = True
        '
        'ListBoxAvailableRoles
        '
        Me.ListBoxAvailableRoles.FormattingEnabled = True
        Me.ListBoxAvailableRoles.Location = New System.Drawing.Point(307, 35)
        Me.ListBoxAvailableRoles.Name = "ListBoxAvailableRoles"
        Me.ListBoxAvailableRoles.Size = New System.Drawing.Size(229, 251)
        Me.ListBoxAvailableRoles.Sorted = True
        Me.ListBoxAvailableRoles.TabIndex = 13
        '
        'ListBoxIncludedRoles
        '
        Me.ListBoxIncludedRoles.FormattingEnabled = True
        Me.ListBoxIncludedRoles.Location = New System.Drawing.Point(9, 35)
        Me.ListBoxIncludedRoles.Name = "ListBoxIncludedRoles"
        Me.ListBoxIncludedRoles.Size = New System.Drawing.Size(215, 251)
        Me.ListBoxIncludedRoles.Sorted = True
        Me.ListBoxIncludedRoles.TabIndex = 12
        '
        'labelprompt_role
        '
        Me.labelprompt_role.AutoSize = True
        Me.labelprompt_role.Location = New System.Drawing.Point(6, 15)
        Me.labelprompt_role.Name = "labelprompt_role"
        Me.labelprompt_role.Size = New System.Drawing.Size(81, 13)
        Me.labelprompt_role.TabIndex = 3
        Me.labelprompt_role.Text = "Included Roles:"
        '
        'LabelPromptAvailableRoles
        '
        Me.LabelPromptAvailableRoles.AutoSize = True
        Me.LabelPromptAvailableRoles.Location = New System.Drawing.Point(305, 15)
        Me.LabelPromptAvailableRoles.Name = "LabelPromptAvailableRoles"
        Me.LabelPromptAvailableRoles.Size = New System.Drawing.Size(83, 13)
        Me.LabelPromptAvailableRoles.TabIndex = 11
        Me.LabelPromptAvailableRoles.Text = "Available Roles:"
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.TabControl2)
        Me.TabPage3.Controls.Add(Me.ListBoxIncludedInGroups)
        Me.TabPage3.Controls.Add(Me.LabelPromptIncludedInGroups)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(552, 443)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Groups"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage2)
        Me.TabControl2.Controls.Add(Me.TabPage5)
        Me.TabControl2.Location = New System.Drawing.Point(22, 231)
        Me.TabControl2.Margin = New System.Windows.Forms.Padding(2)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(509, 196)
        Me.TabControl2.TabIndex = 4
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ListBoxProjectsInGroup)
        Me.TabPage2.Controls.Add(Me.LabelPromptProjectsInGroup)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage2.Size = New System.Drawing.Size(501, 170)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Projects"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ListBoxProjectsInGroup
        '
        Me.ListBoxProjectsInGroup.FormattingEnabled = True
        Me.ListBoxProjectsInGroup.Location = New System.Drawing.Point(18, 30)
        Me.ListBoxProjectsInGroup.Name = "ListBoxProjectsInGroup"
        Me.ListBoxProjectsInGroup.Size = New System.Drawing.Size(474, 134)
        Me.ListBoxProjectsInGroup.TabIndex = 2
        '
        'LabelPromptProjectsInGroup
        '
        Me.LabelPromptProjectsInGroup.AutoSize = True
        Me.LabelPromptProjectsInGroup.Location = New System.Drawing.Point(15, 14)
        Me.LabelPromptProjectsInGroup.Name = "LabelPromptProjectsInGroup"
        Me.LabelPromptProjectsInGroup.Size = New System.Drawing.Size(156, 13)
        Me.LabelPromptProjectsInGroup.TabIndex = 3
        Me.LabelPromptProjectsInGroup.Text = "Projects for the selected Group:"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.ListBoxWhosInTheGroup)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Margin = New System.Windows.Forms.Padding(2)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(2)
        Me.TabPage5.Size = New System.Drawing.Size(501, 170)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Who's in the Group?"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'ListBoxWhosInTheGroup
        '
        Me.ListBoxWhosInTheGroup.FormattingEnabled = True
        Me.ListBoxWhosInTheGroup.Location = New System.Drawing.Point(9, 8)
        Me.ListBoxWhosInTheGroup.Margin = New System.Windows.Forms.Padding(2)
        Me.ListBoxWhosInTheGroup.Name = "ListBoxWhosInTheGroup"
        Me.ListBoxWhosInTheGroup.Size = New System.Drawing.Size(487, 160)
        Me.ListBoxWhosInTheGroup.TabIndex = 5
        '
        'ListBoxIncludedInGroups
        '
        Me.ListBoxIncludedInGroups.FormattingEnabled = True
        Me.ListBoxIncludedInGroups.Location = New System.Drawing.Point(22, 33)
        Me.ListBoxIncludedInGroups.Name = "ListBoxIncludedInGroups"
        Me.ListBoxIncludedInGroups.Size = New System.Drawing.Size(510, 186)
        Me.ListBoxIncludedInGroups.Sorted = True
        Me.ListBoxIncludedInGroups.TabIndex = 1
        '
        'LabelPromptIncludedInGroups
        '
        Me.LabelPromptIncludedInGroups.AutoSize = True
        Me.LabelPromptIncludedInGroups.Location = New System.Drawing.Point(19, 17)
        Me.LabelPromptIncludedInGroups.Name = "LabelPromptIncludedInGroups"
        Me.LabelPromptIncludedInGroups.Size = New System.Drawing.Size(104, 13)
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
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(552, 443)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Projects"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionCreate
        '
        Me.CheckBoxUserPermissionCreate.AutoSize = True
        Me.CheckBoxUserPermissionCreate.Enabled = False
        Me.CheckBoxUserPermissionCreate.Location = New System.Drawing.Point(23, 374)
        Me.CheckBoxUserPermissionCreate.Name = "CheckBoxUserPermissionCreate"
        Me.CheckBoxUserPermissionCreate.Size = New System.Drawing.Size(57, 17)
        Me.CheckBoxUserPermissionCreate.TabIndex = 11
        Me.CheckBoxUserPermissionCreate.Text = "Create"
        Me.CheckBoxUserPermissionCreate.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionAlter
        '
        Me.CheckBoxUserPermissionAlter.AutoSize = True
        Me.CheckBoxUserPermissionAlter.Enabled = False
        Me.CheckBoxUserPermissionAlter.Location = New System.Drawing.Point(23, 420)
        Me.CheckBoxUserPermissionAlter.Name = "CheckBoxUserPermissionAlter"
        Me.CheckBoxUserPermissionAlter.Size = New System.Drawing.Size(47, 17)
        Me.CheckBoxUserPermissionAlter.TabIndex = 10
        Me.CheckBoxUserPermissionAlter.Text = "Alter"
        Me.CheckBoxUserPermissionAlter.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionRead
        '
        Me.CheckBoxUserPermissionRead.AutoSize = True
        Me.CheckBoxUserPermissionRead.Enabled = False
        Me.CheckBoxUserPermissionRead.Location = New System.Drawing.Point(23, 397)
        Me.CheckBoxUserPermissionRead.Name = "CheckBoxUserPermissionRead"
        Me.CheckBoxUserPermissionRead.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxUserPermissionRead.TabIndex = 9
        Me.CheckBoxUserPermissionRead.Text = "Read"
        Me.CheckBoxUserPermissionRead.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionNoPermission
        '
        Me.CheckBoxUserPermissionNoPermission.AutoSize = True
        Me.CheckBoxUserPermissionNoPermission.Enabled = False
        Me.CheckBoxUserPermissionNoPermission.Location = New System.Drawing.Point(23, 351)
        Me.CheckBoxUserPermissionNoPermission.Name = "CheckBoxUserPermissionNoPermission"
        Me.CheckBoxUserPermissionNoPermission.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxUserPermissionNoPermission.TabIndex = 8
        Me.CheckBoxUserPermissionNoPermission.Text = "No Permission"
        Me.CheckBoxUserPermissionNoPermission.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionFullRights
        '
        Me.CheckBoxUserPermissionFullRights.AutoSize = True
        Me.CheckBoxUserPermissionFullRights.Enabled = False
        Me.CheckBoxUserPermissionFullRights.Location = New System.Drawing.Point(23, 328)
        Me.CheckBoxUserPermissionFullRights.Name = "CheckBoxUserPermissionFullRights"
        Me.CheckBoxUserPermissionFullRights.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxUserPermissionFullRights.TabIndex = 7
        Me.CheckBoxUserPermissionFullRights.Text = "Full Rights"
        Me.CheckBoxUserPermissionFullRights.UseVisualStyleBackColor = True
        '
        'ListBoxIncludedInProjects
        '
        Me.ListBoxIncludedInProjects.FormattingEnabled = True
        Me.ListBoxIncludedInProjects.Location = New System.Drawing.Point(23, 32)
        Me.ListBoxIncludedInProjects.Name = "ListBoxIncludedInProjects"
        Me.ListBoxIncludedInProjects.Size = New System.Drawing.Size(510, 290)
        Me.ListBoxIncludedInProjects.Sorted = True
        Me.ListBoxIncludedInProjects.TabIndex = 3
        '
        'LabelPromptIncludedInProjects
        '
        Me.LabelPromptIncludedInProjects.AutoSize = True
        Me.LabelPromptIncludedInProjects.Location = New System.Drawing.Point(20, 16)
        Me.LabelPromptIncludedInProjects.Name = "LabelPromptIncludedInProjects"
        Me.LabelPromptIncludedInProjects.Size = New System.Drawing.Size(108, 13)
        Me.LabelPromptIncludedInProjects.TabIndex = 2
        Me.LabelPromptIncludedInProjects.Text = "Included in Project/s:"
        '
        'TabPageAdvanced
        '
        Me.TabPageAdvanced.Controls.Add(Me.checkboxIsActive)
        Me.TabPageAdvanced.Location = New System.Drawing.Point(4, 22)
        Me.TabPageAdvanced.Name = "TabPageAdvanced"
        Me.TabPageAdvanced.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageAdvanced.Size = New System.Drawing.Size(552, 443)
        Me.TabPageAdvanced.TabIndex = 1
        Me.TabPageAdvanced.Text = "Advanced"
        Me.TabPageAdvanced.UseVisualStyleBackColor = True
        '
        'checkboxIsActive
        '
        Me.checkboxIsActive.AutoSize = True
        Me.checkboxIsActive.Location = New System.Drawing.Point(18, 16)
        Me.checkboxIsActive.Name = "checkboxIsActive"
        Me.checkboxIsActive.Size = New System.Drawing.Size(91, 17)
        Me.checkboxIsActive.TabIndex = 10
        Me.checkboxIsActive.Text = "User is Active"
        Me.checkboxIsActive.UseVisualStyleBackColor = True
        '
        'TabPagePesonalisation
        '
        Me.TabPagePesonalisation.Controls.Add(Me.ComboBoxDefaultBostonApplication)
        Me.TabPagePesonalisation.Controls.Add(Me.LabelPrompDefaultBostonApplication)
        Me.TabPagePesonalisation.Controls.Add(Me.CheckBoxShowTheBox)
        Me.TabPagePesonalisation.Controls.Add(Me.CheckBoxShowVirtualAnalyst)
        Me.TabPagePesonalisation.Controls.Add(Me.CheckBoxShowEnterpriseExplorer)
        Me.TabPagePesonalisation.Controls.Add(Me.ComboBoxDefaultTask)
        Me.TabPagePesonalisation.Controls.Add(Me.LabelPromptDefaultTask)
        Me.TabPagePesonalisation.Controls.Add(Me.TextBoxLogoFileLocation)
        Me.TabPagePesonalisation.Controls.Add(Me.LabelPromptLogoFileLocation)
        Me.TabPagePesonalisation.Location = New System.Drawing.Point(4, 22)
        Me.TabPagePesonalisation.Name = "TabPagePesonalisation"
        Me.TabPagePesonalisation.Size = New System.Drawing.Size(552, 443)
        Me.TabPagePesonalisation.TabIndex = 4
        Me.TabPagePesonalisation.Text = "Pesonalisation"
        Me.TabPagePesonalisation.UseVisualStyleBackColor = True
        '
        'CheckBoxShowTheBox
        '
        Me.CheckBoxShowTheBox.AutoSize = True
        Me.CheckBoxShowTheBox.Location = New System.Drawing.Point(26, 136)
        Me.CheckBoxShowTheBox.Name = "CheckBoxShowTheBox"
        Me.CheckBoxShowTheBox.Size = New System.Drawing.Size(96, 17)
        Me.CheckBoxShowTheBox.TabIndex = 6
        Me.CheckBoxShowTheBox.Text = "Show The Box"
        Me.CheckBoxShowTheBox.UseVisualStyleBackColor = True
        '
        'CheckBoxShowVirtualAnalyst
        '
        Me.CheckBoxShowVirtualAnalyst.AutoSize = True
        Me.CheckBoxShowVirtualAnalyst.Location = New System.Drawing.Point(26, 112)
        Me.CheckBoxShowVirtualAnalyst.Name = "CheckBoxShowVirtualAnalyst"
        Me.CheckBoxShowVirtualAnalyst.Size = New System.Drawing.Size(122, 17)
        Me.CheckBoxShowVirtualAnalyst.TabIndex = 5
        Me.CheckBoxShowVirtualAnalyst.Text = "Show Virtual Analyst"
        Me.CheckBoxShowVirtualAnalyst.UseVisualStyleBackColor = True
        '
        'CheckBoxShowEnterpriseExplorer
        '
        Me.CheckBoxShowEnterpriseExplorer.AutoSize = True
        Me.CheckBoxShowEnterpriseExplorer.Location = New System.Drawing.Point(26, 88)
        Me.CheckBoxShowEnterpriseExplorer.Name = "CheckBoxShowEnterpriseExplorer"
        Me.CheckBoxShowEnterpriseExplorer.Size = New System.Drawing.Size(126, 17)
        Me.CheckBoxShowEnterpriseExplorer.TabIndex = 4
        Me.CheckBoxShowEnterpriseExplorer.Text = "Show Model Explorer"
        Me.CheckBoxShowEnterpriseExplorer.UseVisualStyleBackColor = True
        '
        'ComboBoxDefaultTask
        '
        Me.ComboBoxDefaultTask.FormattingEnabled = True
        Me.ComboBoxDefaultTask.Location = New System.Drawing.Point(123, 48)
        Me.ComboBoxDefaultTask.Name = "ComboBoxDefaultTask"
        Me.ComboBoxDefaultTask.Size = New System.Drawing.Size(228, 21)
        Me.ComboBoxDefaultTask.TabIndex = 3
        '
        'LabelPromptDefaultTask
        '
        Me.LabelPromptDefaultTask.AutoSize = True
        Me.LabelPromptDefaultTask.Location = New System.Drawing.Point(23, 54)
        Me.LabelPromptDefaultTask.Name = "LabelPromptDefaultTask"
        Me.LabelPromptDefaultTask.Size = New System.Drawing.Size(71, 13)
        Me.LabelPromptDefaultTask.TabIndex = 2
        Me.LabelPromptDefaultTask.Text = "Default Task:"
        '
        'TextBoxLogoFileLocation
        '
        Me.TextBoxLogoFileLocation.Location = New System.Drawing.Point(123, 22)
        Me.TextBoxLogoFileLocation.Name = "TextBoxLogoFileLocation"
        Me.TextBoxLogoFileLocation.Size = New System.Drawing.Size(403, 20)
        Me.TextBoxLogoFileLocation.TabIndex = 1
        '
        'LabelPromptLogoFileLocation
        '
        Me.LabelPromptLogoFileLocation.AutoSize = True
        Me.LabelPromptLogoFileLocation.Location = New System.Drawing.Point(20, 25)
        Me.LabelPromptLogoFileLocation.Name = "LabelPromptLogoFileLocation"
        Me.LabelPromptLogoFileLocation.Size = New System.Drawing.Size(97, 13)
        Me.LabelPromptLogoFileLocation.TabIndex = 0
        Me.LabelPromptLogoFileLocation.Text = "Logo File Location:"
        '
        'TabPageAuthentication
        '
        Me.TabPageAuthentication.Controls.Add(Me.TextBoxCallInPIN)
        Me.TabPageAuthentication.Controls.Add(Me.LabelPromptCallInPIN)
        Me.TabPageAuthentication.Location = New System.Drawing.Point(4, 22)
        Me.TabPageAuthentication.Name = "TabPageAuthentication"
        Me.TabPageAuthentication.Size = New System.Drawing.Size(552, 443)
        Me.TabPageAuthentication.TabIndex = 5
        Me.TabPageAuthentication.Text = "Authentication"
        Me.TabPageAuthentication.UseVisualStyleBackColor = True
        '
        'TextBoxCallInPIN
        '
        Me.TextBoxCallInPIN.Location = New System.Drawing.Point(83, 21)
        Me.TextBoxCallInPIN.MaxLength = 4
        Me.TextBoxCallInPIN.Name = "TextBoxCallInPIN"
        Me.TextBoxCallInPIN.Size = New System.Drawing.Size(65, 20)
        Me.TextBoxCallInPIN.TabIndex = 1
        '
        'LabelPromptCallInPIN
        '
        Me.LabelPromptCallInPIN.AutoSize = True
        Me.LabelPromptCallInPIN.Location = New System.Drawing.Point(17, 24)
        Me.LabelPromptCallInPIN.Name = "LabelPromptCallInPIN"
        Me.LabelPromptCallInPIN.Size = New System.Drawing.Size(60, 13)
        Me.LabelPromptCallInPIN.TabIndex = 0
        Me.LabelPromptCallInPIN.Text = "Call-In PIN:"
        '
        'PictureBox_line2
        '
        Me.PictureBox_line2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox_line2.Location = New System.Drawing.Point(20, 187)
        Me.PictureBox_line2.Name = "PictureBox_line2"
        Me.PictureBox_line2.Size = New System.Drawing.Size(328, 1)
        Me.PictureBox_line2.TabIndex = 9
        Me.PictureBox_line2.TabStop = False
        '
        'labelprompt_confirmation_password
        '
        Me.labelprompt_confirmation_password.AutoSize = True
        Me.labelprompt_confirmation_password.Location = New System.Drawing.Point(22, 155)
        Me.labelprompt_confirmation_password.Name = "labelprompt_confirmation_password"
        Me.labelprompt_confirmation_password.Size = New System.Drawing.Size(97, 13)
        Me.labelprompt_confirmation_password.TabIndex = 8
        Me.labelprompt_confirmation_password.Text = "Confirm Password :"
        Me.labelprompt_confirmation_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'labelprompt_password
        '
        Me.labelprompt_password.AutoSize = True
        Me.labelprompt_password.Location = New System.Drawing.Point(60, 129)
        Me.labelprompt_password.Name = "labelprompt_password"
        Me.labelprompt_password.Size = New System.Drawing.Size(59, 13)
        Me.labelprompt_password.TabIndex = 7
        Me.labelprompt_password.Text = "Password :"
        Me.labelprompt_password.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'textbox_confirmation_password
        '
        Me.textbox_confirmation_password.Location = New System.Drawing.Point(125, 152)
        Me.textbox_confirmation_password.Name = "textbox_confirmation_password"
        Me.textbox_confirmation_password.Size = New System.Drawing.Size(195, 20)
        Me.textbox_confirmation_password.TabIndex = 6
        '
        'textbox_password
        '
        Me.textbox_password.Location = New System.Drawing.Point(125, 126)
        Me.textbox_password.Name = "textbox_password"
        Me.textbox_password.Size = New System.Drawing.Size(196, 20)
        Me.textbox_password.TabIndex = 5
        '
        'labelprompt_operator_name
        '
        Me.labelprompt_operator_name.AutoSize = True
        Me.labelprompt_operator_name.Location = New System.Drawing.Point(58, 29)
        Me.labelprompt_operator_name.Name = "labelprompt_operator_name"
        Me.labelprompt_operator_name.Size = New System.Drawing.Size(61, 13)
        Me.labelprompt_operator_name.TabIndex = 1
        Me.labelprompt_operator_name.Text = "Username :"
        '
        'textbox_operator_name
        '
        Me.textbox_operator_name.Location = New System.Drawing.Point(125, 26)
        Me.textbox_operator_name.Name = "textbox_operator_name"
        Me.textbox_operator_name.Size = New System.Drawing.Size(196, 20)
        Me.textbox_operator_name.TabIndex = 0
        '
        'ButtonMakeDemoUser
        '
        Me.ButtonMakeDemoUser.Location = New System.Drawing.Point(609, 94)
        Me.ButtonMakeDemoUser.Name = "ButtonMakeDemoUser"
        Me.ButtonMakeDemoUser.Size = New System.Drawing.Size(109, 23)
        Me.ButtonMakeDemoUser.TabIndex = 9
        Me.ButtonMakeDemoUser.Text = "Make Demo User"
        Me.ButtonMakeDemoUser.UseVisualStyleBackColor = True
        Me.ButtonMakeDemoUser.Visible = False
        '
        'LabelPrompDefaultBostonApplication
        '
        Me.LabelPrompDefaultBostonApplication.AutoSize = True
        Me.LabelPrompDefaultBostonApplication.Location = New System.Drawing.Point(26, 176)
        Me.LabelPrompDefaultBostonApplication.Name = "LabelPrompDefaultBostonApplication"
        Me.LabelPrompDefaultBostonApplication.Size = New System.Drawing.Size(98, 13)
        Me.LabelPrompDefaultBostonApplication.TabIndex = 7
        Me.LabelPrompDefaultBostonApplication.Text = "Boston Application:"
        '
        'ComboBoxDefaultBostonApplication
        '
        Me.ComboBoxDefaultBostonApplication.FormattingEnabled = True
        Me.ComboBoxDefaultBostonApplication.Location = New System.Drawing.Point(130, 173)
        Me.ComboBoxDefaultBostonApplication.Name = "ComboBoxDefaultBostonApplication"
        Me.ComboBoxDefaultBostonApplication.Size = New System.Drawing.Size(396, 21)
        Me.ComboBoxDefaultBostonApplication.TabIndex = 8
        '
        'frmCRUDEditUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 682)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonMakeDemoUser)
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.button_okay)
        Me.Controls.Add(Me.groupbox_main)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
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
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPageAdvanced.ResumeLayout(False)
        Me.TabPageAdvanced.PerformLayout()
        Me.TabPagePesonalisation.ResumeLayout(False)
        Me.TabPagePesonalisation.PerformLayout()
        Me.TabPageAuthentication.ResumeLayout(False)
        Me.TabPageAuthentication.PerformLayout()
        CType(Me.PictureBox_line2, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents TabPagePesonalisation As TabPage
    Friend WithEvents ComboBoxDefaultTask As ComboBox
    Friend WithEvents LabelPromptDefaultTask As Label
    Friend WithEvents TextBoxLogoFileLocation As TextBox
    Friend WithEvents LabelPromptLogoFileLocation As Label
    Friend WithEvents CheckBoxShowTheBox As CheckBox
    Friend WithEvents CheckBoxShowVirtualAnalyst As CheckBox
    Friend WithEvents CheckBoxShowEnterpriseExplorer As CheckBox
    Friend WithEvents TabPageAuthentication As TabPage
    Friend WithEvents TextBoxCallInPIN As TextBox
    Friend WithEvents LabelPromptCallInPIN As Label
    Friend WithEvents ButtonMakeDemoUser As Button
    Friend WithEvents ComboBoxDefaultBostonApplication As ComboBox
    Friend WithEvents LabelPrompDefaultBostonApplication As Label
End Class
