<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditProject
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCRUDEditProject))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LabelCreatedByUser = New System.Windows.Forms.Label()
        Me.LabelPromptCreatedByUser = New System.Windows.Forms.Label()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.LabelHelpTipsUser = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.FlexibleListBoxIncludedUser = New Boston.FlexibleListBox()
        Me.PanelInviteUser = New System.Windows.Forms.Panel()
        Me.TextBoxUserName = New System.Windows.Forms.TextBox()
        Me.FlexibleListBox = New Boston.FlexibleListBox()
        Me.ListBoxAvailableUsers = New System.Windows.Forms.ListBox()
        Me.ButtonIncludeUser = New System.Windows.Forms.Button()
        Me.LabelPromptAvailableUsers = New System.Windows.Forms.Label()
        Me.LabelPromptIncludedUsers = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.CheckBoxUserPermissionCreate = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionAlter = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionRead = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionNoPermission = New System.Windows.Forms.CheckBox()
        Me.CheckBoxUserPermissionFullRights = New System.Windows.Forms.CheckBox()
        Me.LabelPromptUser = New System.Windows.Forms.Label()
        Me.ComboBoxUserPermissions = New MTGCComboBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.LabelHelpTipsGroup = New System.Windows.Forms.Label()
        Me.TabControl3 = New System.Windows.Forms.TabControl()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.LabelPromptGroupUsersInProject = New System.Windows.Forms.Label()
        Me.ListBoxGroupUsers = New System.Windows.Forms.ListBox()
        Me.ButtonExcludeGroup = New System.Windows.Forms.Button()
        Me.ButtonIncludeGroup = New System.Windows.Forms.Button()
        Me.LabelPromptAvailableGroups = New System.Windows.Forms.Label()
        Me.LabelPromptIncludedGroups = New System.Windows.Forms.Label()
        Me.ListBoxIncludedGroups = New System.Windows.Forms.ListBox()
        Me.PanelInviteGroup = New System.Windows.Forms.Panel()
        Me.TextBoxGroupInvite = New System.Windows.Forms.TextBox()
        Me.FlexibleListBoxGroupInvite = New Boston.FlexibleListBox()
        Me.ListBoxAvailableGroups = New System.Windows.Forms.ListBox()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.ComboBoxGroupPermissions = New System.Windows.Forms.ComboBox()
        Me.CheckBoxGroupPermissionCreate = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGroupPermissionAlter = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGroupPermissionRead = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGroupPermissionNoPermission = New System.Windows.Forms.CheckBox()
        Me.CheckBoxGroupPermissionFullRights = New System.Windows.Forms.CheckBox()
        Me.LabelPromptGroup = New System.Windows.Forms.Label()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.ListBoxIncludedNamespaces = New System.Windows.Forms.ListBox()
        Me.LabelPromptIncludedNamespaces = New System.Windows.Forms.Label()
        Me.TextBoxProjectName = New System.Windows.Forms.TextBox()
        Me.LabelPromptGroupName = New System.Windows.Forms.Label()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CheckBoxLeastRestrictiveCreate = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLeastRestrictiveAlter = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLeastRestrictiveRead = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLeastRestrictivenoPermission = New System.Windows.Forms.CheckBox()
        Me.CheckBoxLeastRestrictiveFullPermissions = New System.Windows.Forms.CheckBox()
        Me.LabelPromptLeastRestrictivePermissions = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.PanelInviteUser.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabControl3.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.PanelInviteGroup.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.LabelPromptCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.TabControl2)
        Me.GroupBox1.Controls.Add(Me.TextBoxProjectName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptGroupName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(672, 709)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'LabelCreatedByUser
        '
        Me.LabelCreatedByUser.AutoSize = True
        Me.LabelCreatedByUser.Location = New System.Drawing.Point(86, 51)
        Me.LabelCreatedByUser.Name = "LabelCreatedByUser"
        Me.LabelCreatedByUser.Size = New System.Drawing.Size(104, 13)
        Me.LabelCreatedByUser.TabIndex = 5
        Me.LabelCreatedByUser.Text = "LabelCreatedByUser"
        '
        'LabelPromptCreatedByUser
        '
        Me.LabelPromptCreatedByUser.AutoSize = True
        Me.LabelPromptCreatedByUser.Location = New System.Drawing.Point(19, 51)
        Me.LabelPromptCreatedByUser.Name = "LabelPromptCreatedByUser"
        Me.LabelPromptCreatedByUser.Size = New System.Drawing.Size(61, 13)
        Me.LabelPromptCreatedByUser.TabIndex = 4
        Me.LabelPromptCreatedByUser.Text = "Created by:"
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage3)
        Me.TabControl2.Controls.Add(Me.TabPage4)
        Me.TabControl2.Controls.Add(Me.TabPage7)
        Me.TabControl2.Location = New System.Drawing.Point(9, 81)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(657, 622)
        Me.TabControl2.TabIndex = 3
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.LabelHelpTipsUser)
        Me.TabPage3.Controls.Add(Me.TabControl1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(649, 596)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "Users"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'LabelHelpTipsUser
        '
        Me.LabelHelpTipsUser.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelpTipsUser.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelpTipsUser.Location = New System.Drawing.Point(3, 546)
        Me.LabelHelpTipsUser.Name = "LabelHelpTipsUser"
        Me.LabelHelpTipsUser.Size = New System.Drawing.Size(643, 47)
        Me.LabelHelpTipsUser.TabIndex = 13
        Me.LabelHelpTipsUser.Text = "LabelHelpTipsUser"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(6, 6)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(637, 537)
        Me.TabControl1.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.LabelPromptLeastRestrictivePermissions)
        Me.TabPage1.Controls.Add(Me.CheckBoxLeastRestrictiveCreate)
        Me.TabPage1.Controls.Add(Me.CheckBoxLeastRestrictiveAlter)
        Me.TabPage1.Controls.Add(Me.CheckBoxLeastRestrictiveRead)
        Me.TabPage1.Controls.Add(Me.CheckBoxLeastRestrictivenoPermission)
        Me.TabPage1.Controls.Add(Me.CheckBoxLeastRestrictiveFullPermissions)
        Me.TabPage1.Controls.Add(Me.FlexibleListBoxIncludedUser)
        Me.TabPage1.Controls.Add(Me.PanelInviteUser)
        Me.TabPage1.Controls.Add(Me.ButtonIncludeUser)
        Me.TabPage1.Controls.Add(Me.LabelPromptAvailableUsers)
        Me.TabPage1.Controls.Add(Me.LabelPromptIncludedUsers)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(629, 511)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Included Users"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'FlexibleListBoxIncludedUser
        '
        Me.FlexibleListBoxIncludedUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlexibleListBoxIncludedUser.Location = New System.Drawing.Point(15, 29)
        Me.FlexibleListBoxIncludedUser.Margin = New System.Windows.Forms.Padding(0)
        Me.FlexibleListBoxIncludedUser.Name = "FlexibleListBoxIncludedUser"
        Me.FlexibleListBoxIncludedUser.Size = New System.Drawing.Size(300, 393)
        Me.FlexibleListBoxIncludedUser.TabIndex = 7
        '
        'PanelInviteUser
        '
        Me.PanelInviteUser.Controls.Add(Me.TextBoxUserName)
        Me.PanelInviteUser.Controls.Add(Me.FlexibleListBox)
        Me.PanelInviteUser.Controls.Add(Me.ListBoxAvailableUsers)
        Me.PanelInviteUser.Location = New System.Drawing.Point(390, 29)
        Me.PanelInviteUser.Name = "PanelInviteUser"
        Me.PanelInviteUser.Size = New System.Drawing.Size(225, 394)
        Me.PanelInviteUser.TabIndex = 6
        '
        'TextBoxUserName
        '
        Me.TextBoxUserName.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxUserName.Name = "TextBoxUserName"
        Me.TextBoxUserName.Size = New System.Drawing.Size(171, 20)
        Me.TextBoxUserName.TabIndex = 3
        Me.TextBoxUserName.Tag = "Type a name here"
        '
        'FlexibleListBox
        '
        Me.FlexibleListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlexibleListBox.Location = New System.Drawing.Point(0, 31)
        Me.FlexibleListBox.Name = "FlexibleListBox"
        Me.FlexibleListBox.Size = New System.Drawing.Size(215, 362)
        Me.FlexibleListBox.TabIndex = 1
        '
        'ListBoxAvailableUsers
        '
        Me.ListBoxAvailableUsers.FormattingEnabled = True
        Me.ListBoxAvailableUsers.Location = New System.Drawing.Point(0, 0)
        Me.ListBoxAvailableUsers.Name = "ListBoxAvailableUsers"
        Me.ListBoxAvailableUsers.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxAvailableUsers.Sorted = True
        Me.ListBoxAvailableUsers.TabIndex = 2
        '
        'ButtonIncludeUser
        '
        Me.ButtonIncludeUser.Location = New System.Drawing.Point(321, 29)
        Me.ButtonIncludeUser.Name = "ButtonIncludeUser"
        Me.ButtonIncludeUser.Size = New System.Drawing.Size(64, 25)
        Me.ButtonIncludeUser.TabIndex = 4
        Me.ButtonIncludeUser.Text = "<<"
        Me.ButtonIncludeUser.UseVisualStyleBackColor = True
        '
        'LabelPromptAvailableUsers
        '
        Me.LabelPromptAvailableUsers.AutoSize = True
        Me.LabelPromptAvailableUsers.Location = New System.Drawing.Point(387, 12)
        Me.LabelPromptAvailableUsers.Name = "LabelPromptAvailableUsers"
        Me.LabelPromptAvailableUsers.Size = New System.Drawing.Size(80, 13)
        Me.LabelPromptAvailableUsers.TabIndex = 3
        Me.LabelPromptAvailableUsers.Text = "Available Users"
        '
        'LabelPromptIncludedUsers
        '
        Me.LabelPromptIncludedUsers.AutoSize = True
        Me.LabelPromptIncludedUsers.Location = New System.Drawing.Point(15, 12)
        Me.LabelPromptIncludedUsers.Name = "LabelPromptIncludedUsers"
        Me.LabelPromptIncludedUsers.Size = New System.Drawing.Size(78, 13)
        Me.LabelPromptIncludedUsers.TabIndex = 1
        Me.LabelPromptIncludedUsers.Text = "Included Users"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.CheckBoxUserPermissionCreate)
        Me.TabPage2.Controls.Add(Me.CheckBoxUserPermissionAlter)
        Me.TabPage2.Controls.Add(Me.CheckBoxUserPermissionRead)
        Me.TabPage2.Controls.Add(Me.CheckBoxUserPermissionNoPermission)
        Me.TabPage2.Controls.Add(Me.CheckBoxUserPermissionFullRights)
        Me.TabPage2.Controls.Add(Me.LabelPromptUser)
        Me.TabPage2.Controls.Add(Me.ComboBoxUserPermissions)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(629, 428)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "User Permissions"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionCreate
        '
        Me.CheckBoxUserPermissionCreate.AutoSize = True
        Me.CheckBoxUserPermissionCreate.Location = New System.Drawing.Point(70, 120)
        Me.CheckBoxUserPermissionCreate.Name = "CheckBoxUserPermissionCreate"
        Me.CheckBoxUserPermissionCreate.Size = New System.Drawing.Size(57, 17)
        Me.CheckBoxUserPermissionCreate.TabIndex = 6
        Me.CheckBoxUserPermissionCreate.Text = "Create"
        Me.CheckBoxUserPermissionCreate.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionAlter
        '
        Me.CheckBoxUserPermissionAlter.AutoSize = True
        Me.CheckBoxUserPermissionAlter.Location = New System.Drawing.Point(70, 166)
        Me.CheckBoxUserPermissionAlter.Name = "CheckBoxUserPermissionAlter"
        Me.CheckBoxUserPermissionAlter.Size = New System.Drawing.Size(47, 17)
        Me.CheckBoxUserPermissionAlter.TabIndex = 5
        Me.CheckBoxUserPermissionAlter.Text = "Alter"
        Me.CheckBoxUserPermissionAlter.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionRead
        '
        Me.CheckBoxUserPermissionRead.AutoSize = True
        Me.CheckBoxUserPermissionRead.Location = New System.Drawing.Point(70, 143)
        Me.CheckBoxUserPermissionRead.Name = "CheckBoxUserPermissionRead"
        Me.CheckBoxUserPermissionRead.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxUserPermissionRead.TabIndex = 4
        Me.CheckBoxUserPermissionRead.Text = "Read"
        Me.CheckBoxUserPermissionRead.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionNoPermission
        '
        Me.CheckBoxUserPermissionNoPermission.AutoSize = True
        Me.CheckBoxUserPermissionNoPermission.Location = New System.Drawing.Point(70, 97)
        Me.CheckBoxUserPermissionNoPermission.Name = "CheckBoxUserPermissionNoPermission"
        Me.CheckBoxUserPermissionNoPermission.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxUserPermissionNoPermission.TabIndex = 3
        Me.CheckBoxUserPermissionNoPermission.Text = "No Permission"
        Me.CheckBoxUserPermissionNoPermission.UseVisualStyleBackColor = True
        '
        'CheckBoxUserPermissionFullRights
        '
        Me.CheckBoxUserPermissionFullRights.AutoSize = True
        Me.CheckBoxUserPermissionFullRights.Location = New System.Drawing.Point(70, 74)
        Me.CheckBoxUserPermissionFullRights.Name = "CheckBoxUserPermissionFullRights"
        Me.CheckBoxUserPermissionFullRights.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxUserPermissionFullRights.TabIndex = 2
        Me.CheckBoxUserPermissionFullRights.Text = "Full Rights"
        Me.CheckBoxUserPermissionFullRights.UseVisualStyleBackColor = True
        '
        'LabelPromptUser
        '
        Me.LabelPromptUser.AutoSize = True
        Me.LabelPromptUser.Location = New System.Drawing.Point(31, 44)
        Me.LabelPromptUser.Name = "LabelPromptUser"
        Me.LabelPromptUser.Size = New System.Drawing.Size(32, 13)
        Me.LabelPromptUser.TabIndex = 1
        Me.LabelPromptUser.Text = "User:"
        '
        'ComboBoxUserPermissions
        '
        Me.ComboBoxUserPermissions.ArrowBoxColor = System.Drawing.SystemColors.Control
        Me.ComboBoxUserPermissions.ArrowColor = System.Drawing.Color.Black
        Me.ComboBoxUserPermissions.BindedControl = CType(resources.GetObject("ComboBoxUserPermissions.BindedControl"), MTGCComboBox.ControlloAssociato)
        Me.ComboBoxUserPermissions.BorderStyle = MTGCComboBox.TipiBordi.FlatXP
        Me.ComboBoxUserPermissions.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Me.ComboBoxUserPermissions.ColumnNum = 1
        Me.ComboBoxUserPermissions.ColumnWidth = "121"
        Me.ComboBoxUserPermissions.DisabledArrowBoxColor = System.Drawing.SystemColors.Control
        Me.ComboBoxUserPermissions.DisabledArrowColor = System.Drawing.Color.LightGray
        Me.ComboBoxUserPermissions.DisabledBackColor = System.Drawing.SystemColors.Control
        Me.ComboBoxUserPermissions.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder
        Me.ComboBoxUserPermissions.DisabledForeColor = System.Drawing.SystemColors.GrayText
        Me.ComboBoxUserPermissions.DisplayMember = "Text"
        Me.ComboBoxUserPermissions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.ComboBoxUserPermissions.DropDownArrowBackColor = System.Drawing.Color.FromArgb(CType(CType(136, Byte), Integer), CType(CType(169, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.ComboBoxUserPermissions.DropDownBackColor = System.Drawing.Color.FromArgb(CType(CType(193, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.ComboBoxUserPermissions.DropDownForeColor = System.Drawing.Color.Black
        Me.ComboBoxUserPermissions.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown
        Me.ComboBoxUserPermissions.DropDownWidth = 141
        Me.ComboBoxUserPermissions.GridLineColor = System.Drawing.Color.LightGray
        Me.ComboBoxUserPermissions.GridLineHorizontal = False
        Me.ComboBoxUserPermissions.GridLineVertical = False
        Me.ComboBoxUserPermissions.HighlightBorderColor = System.Drawing.Color.Blue
        Me.ComboBoxUserPermissions.HighlightBorderOnMouseEvents = True
        Me.ComboBoxUserPermissions.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem
        Me.ComboBoxUserPermissions.Location = New System.Drawing.Point(70, 38)
        Me.ComboBoxUserPermissions.ManagingFastMouseMoving = True
        Me.ComboBoxUserPermissions.ManagingFastMouseMovingInterval = 30
        Me.ComboBoxUserPermissions.Name = "ComboBoxUserPermissions"
        Me.ComboBoxUserPermissions.NormalBorderColor = System.Drawing.Color.Black
        Me.ComboBoxUserPermissions.SelectedItem = Nothing
        Me.ComboBoxUserPermissions.SelectedValue = Nothing
        Me.ComboBoxUserPermissions.Size = New System.Drawing.Size(416, 21)
        Me.ComboBoxUserPermissions.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.LabelHelpTipsGroup)
        Me.TabPage4.Controls.Add(Me.TabControl3)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(649, 596)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Text = "Groups"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'LabelHelpTipsGroup
        '
        Me.LabelHelpTipsGroup.BackColor = System.Drawing.SystemColors.Info
        Me.LabelHelpTipsGroup.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelHelpTipsGroup.Location = New System.Drawing.Point(3, 546)
        Me.LabelHelpTipsGroup.Name = "LabelHelpTipsGroup"
        Me.LabelHelpTipsGroup.Size = New System.Drawing.Size(643, 47)
        Me.LabelHelpTipsGroup.TabIndex = 13
        Me.LabelHelpTipsGroup.Text = "LabelHelpTipsGroup"
        '
        'TabControl3
        '
        Me.TabControl3.Controls.Add(Me.TabPage5)
        Me.TabControl3.Controls.Add(Me.TabPage6)
        Me.TabControl3.Location = New System.Drawing.Point(6, 6)
        Me.TabControl3.Name = "TabControl3"
        Me.TabControl3.SelectedIndex = 0
        Me.TabControl3.Size = New System.Drawing.Size(578, 455)
        Me.TabControl3.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.LabelPromptGroupUsersInProject)
        Me.TabPage5.Controls.Add(Me.ListBoxGroupUsers)
        Me.TabPage5.Controls.Add(Me.ButtonExcludeGroup)
        Me.TabPage5.Controls.Add(Me.ButtonIncludeGroup)
        Me.TabPage5.Controls.Add(Me.LabelPromptAvailableGroups)
        Me.TabPage5.Controls.Add(Me.LabelPromptIncludedGroups)
        Me.TabPage5.Controls.Add(Me.ListBoxIncludedGroups)
        Me.TabPage5.Controls.Add(Me.PanelInviteGroup)
        Me.TabPage5.Controls.Add(Me.ListBoxAvailableGroups)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(570, 429)
        Me.TabPage5.TabIndex = 0
        Me.TabPage5.Text = "Included Groups"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'LabelPromptGroupUsersInProject
        '
        Me.LabelPromptGroupUsersInProject.AutoSize = True
        Me.LabelPromptGroupUsersInProject.Location = New System.Drawing.Point(12, 267)
        Me.LabelPromptGroupUsersInProject.Name = "LabelPromptGroupUsersInProject"
        Me.LabelPromptGroupUsersInProject.Size = New System.Drawing.Size(135, 13)
        Me.LabelPromptGroupUsersInProject.TabIndex = 14
        Me.LabelPromptGroupUsersInProject.Text = "Group Users in this Project:"
        '
        'ListBoxGroupUsers
        '
        Me.ListBoxGroupUsers.FormattingEnabled = True
        Me.ListBoxGroupUsers.Location = New System.Drawing.Point(15, 285)
        Me.ListBoxGroupUsers.Name = "ListBoxGroupUsers"
        Me.ListBoxGroupUsers.Size = New System.Drawing.Size(215, 134)
        Me.ListBoxGroupUsers.TabIndex = 13
        '
        'ButtonExcludeGroup
        '
        Me.ButtonExcludeGroup.Location = New System.Drawing.Point(240, 61)
        Me.ButtonExcludeGroup.Name = "ButtonExcludeGroup"
        Me.ButtonExcludeGroup.Size = New System.Drawing.Size(63, 25)
        Me.ButtonExcludeGroup.TabIndex = 11
        Me.ButtonExcludeGroup.Text = ">>"
        Me.ButtonExcludeGroup.UseVisualStyleBackColor = True
        '
        'ButtonIncludeGroup
        '
        Me.ButtonIncludeGroup.Location = New System.Drawing.Point(240, 30)
        Me.ButtonIncludeGroup.Name = "ButtonIncludeGroup"
        Me.ButtonIncludeGroup.Size = New System.Drawing.Size(64, 25)
        Me.ButtonIncludeGroup.TabIndex = 10
        Me.ButtonIncludeGroup.Text = "<<"
        Me.ButtonIncludeGroup.UseVisualStyleBackColor = True
        '
        'LabelPromptAvailableGroups
        '
        Me.LabelPromptAvailableGroups.AutoSize = True
        Me.LabelPromptAvailableGroups.Location = New System.Drawing.Point(310, 13)
        Me.LabelPromptAvailableGroups.Name = "LabelPromptAvailableGroups"
        Me.LabelPromptAvailableGroups.Size = New System.Drawing.Size(87, 13)
        Me.LabelPromptAvailableGroups.TabIndex = 9
        Me.LabelPromptAvailableGroups.Text = "Available Groups"
        '
        'LabelPromptIncludedGroups
        '
        Me.LabelPromptIncludedGroups.AutoSize = True
        Me.LabelPromptIncludedGroups.Location = New System.Drawing.Point(15, 12)
        Me.LabelPromptIncludedGroups.Name = "LabelPromptIncludedGroups"
        Me.LabelPromptIncludedGroups.Size = New System.Drawing.Size(85, 13)
        Me.LabelPromptIncludedGroups.TabIndex = 7
        Me.LabelPromptIncludedGroups.Text = "Included Groups"
        '
        'ListBoxIncludedGroups
        '
        Me.ListBoxIncludedGroups.FormattingEnabled = True
        Me.ListBoxIncludedGroups.Location = New System.Drawing.Point(15, 29)
        Me.ListBoxIncludedGroups.Name = "ListBoxIncludedGroups"
        Me.ListBoxIncludedGroups.Size = New System.Drawing.Size(215, 225)
        Me.ListBoxIncludedGroups.Sorted = True
        Me.ListBoxIncludedGroups.TabIndex = 6
        '
        'PanelInviteGroup
        '
        Me.PanelInviteGroup.Controls.Add(Me.TextBoxGroupInvite)
        Me.PanelInviteGroup.Controls.Add(Me.FlexibleListBoxGroupInvite)
        Me.PanelInviteGroup.Location = New System.Drawing.Point(313, 29)
        Me.PanelInviteGroup.Name = "PanelInviteGroup"
        Me.PanelInviteGroup.Size = New System.Drawing.Size(225, 225)
        Me.PanelInviteGroup.TabIndex = 12
        '
        'TextBoxGroupInvite
        '
        Me.TextBoxGroupInvite.Location = New System.Drawing.Point(0, 0)
        Me.TextBoxGroupInvite.Name = "TextBoxGroupInvite"
        Me.TextBoxGroupInvite.Size = New System.Drawing.Size(171, 20)
        Me.TextBoxGroupInvite.TabIndex = 3
        Me.TextBoxGroupInvite.Tag = "Type a Group name here"
        '
        'FlexibleListBoxGroupInvite
        '
        Me.FlexibleListBoxGroupInvite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlexibleListBoxGroupInvite.Location = New System.Drawing.Point(0, 31)
        Me.FlexibleListBoxGroupInvite.Name = "FlexibleListBoxGroupInvite"
        Me.FlexibleListBoxGroupInvite.Size = New System.Drawing.Size(215, 194)
        Me.FlexibleListBoxGroupInvite.TabIndex = 1
        '
        'ListBoxAvailableGroups
        '
        Me.ListBoxAvailableGroups.FormattingEnabled = True
        Me.ListBoxAvailableGroups.Location = New System.Drawing.Point(313, 29)
        Me.ListBoxAvailableGroups.Name = "ListBoxAvailableGroups"
        Me.ListBoxAvailableGroups.Size = New System.Drawing.Size(229, 225)
        Me.ListBoxAvailableGroups.Sorted = True
        Me.ListBoxAvailableGroups.TabIndex = 8
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.ComboBoxGroupPermissions)
        Me.TabPage6.Controls.Add(Me.CheckBoxGroupPermissionCreate)
        Me.TabPage6.Controls.Add(Me.CheckBoxGroupPermissionAlter)
        Me.TabPage6.Controls.Add(Me.CheckBoxGroupPermissionRead)
        Me.TabPage6.Controls.Add(Me.CheckBoxGroupPermissionNoPermission)
        Me.TabPage6.Controls.Add(Me.CheckBoxGroupPermissionFullRights)
        Me.TabPage6.Controls.Add(Me.LabelPromptGroup)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(570, 429)
        Me.TabPage6.TabIndex = 1
        Me.TabPage6.Text = "Group Permissions"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'ComboBoxGroupPermissions
        '
        Me.ComboBoxGroupPermissions.FormattingEnabled = True
        Me.ComboBoxGroupPermissions.Location = New System.Drawing.Point(70, 36)
        Me.ComboBoxGroupPermissions.Name = "ComboBoxGroupPermissions"
        Me.ComboBoxGroupPermissions.Size = New System.Drawing.Size(205, 21)
        Me.ComboBoxGroupPermissions.TabIndex = 13
        '
        'CheckBoxGroupPermissionCreate
        '
        Me.CheckBoxGroupPermissionCreate.AutoSize = True
        Me.CheckBoxGroupPermissionCreate.Location = New System.Drawing.Point(70, 120)
        Me.CheckBoxGroupPermissionCreate.Name = "CheckBoxGroupPermissionCreate"
        Me.CheckBoxGroupPermissionCreate.Size = New System.Drawing.Size(57, 17)
        Me.CheckBoxGroupPermissionCreate.TabIndex = 12
        Me.CheckBoxGroupPermissionCreate.Text = "Create"
        Me.CheckBoxGroupPermissionCreate.UseVisualStyleBackColor = True
        '
        'CheckBoxGroupPermissionAlter
        '
        Me.CheckBoxGroupPermissionAlter.AutoSize = True
        Me.CheckBoxGroupPermissionAlter.Location = New System.Drawing.Point(70, 166)
        Me.CheckBoxGroupPermissionAlter.Name = "CheckBoxGroupPermissionAlter"
        Me.CheckBoxGroupPermissionAlter.Size = New System.Drawing.Size(47, 17)
        Me.CheckBoxGroupPermissionAlter.TabIndex = 11
        Me.CheckBoxGroupPermissionAlter.Text = "Alter"
        Me.CheckBoxGroupPermissionAlter.UseVisualStyleBackColor = True
        '
        'CheckBoxGroupPermissionRead
        '
        Me.CheckBoxGroupPermissionRead.AutoSize = True
        Me.CheckBoxGroupPermissionRead.Location = New System.Drawing.Point(70, 143)
        Me.CheckBoxGroupPermissionRead.Name = "CheckBoxGroupPermissionRead"
        Me.CheckBoxGroupPermissionRead.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxGroupPermissionRead.TabIndex = 10
        Me.CheckBoxGroupPermissionRead.Text = "Read"
        Me.CheckBoxGroupPermissionRead.UseVisualStyleBackColor = True
        '
        'CheckBoxGroupPermissionNoPermission
        '
        Me.CheckBoxGroupPermissionNoPermission.AutoSize = True
        Me.CheckBoxGroupPermissionNoPermission.Location = New System.Drawing.Point(70, 97)
        Me.CheckBoxGroupPermissionNoPermission.Name = "CheckBoxGroupPermissionNoPermission"
        Me.CheckBoxGroupPermissionNoPermission.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxGroupPermissionNoPermission.TabIndex = 9
        Me.CheckBoxGroupPermissionNoPermission.Text = "No Permission"
        Me.CheckBoxGroupPermissionNoPermission.UseVisualStyleBackColor = True
        '
        'CheckBoxGroupPermissionFullRights
        '
        Me.CheckBoxGroupPermissionFullRights.AutoSize = True
        Me.CheckBoxGroupPermissionFullRights.Location = New System.Drawing.Point(70, 74)
        Me.CheckBoxGroupPermissionFullRights.Name = "CheckBoxGroupPermissionFullRights"
        Me.CheckBoxGroupPermissionFullRights.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxGroupPermissionFullRights.TabIndex = 8
        Me.CheckBoxGroupPermissionFullRights.Text = "Full Rights"
        Me.CheckBoxGroupPermissionFullRights.UseVisualStyleBackColor = True
        '
        'LabelPromptGroup
        '
        Me.LabelPromptGroup.AutoSize = True
        Me.LabelPromptGroup.Location = New System.Drawing.Point(31, 44)
        Me.LabelPromptGroup.Name = "LabelPromptGroup"
        Me.LabelPromptGroup.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptGroup.TabIndex = 7
        Me.LabelPromptGroup.Text = "Group:"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.ListBoxIncludedNamespaces)
        Me.TabPage7.Controls.Add(Me.LabelPromptIncludedNamespaces)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(649, 596)
        Me.TabPage7.TabIndex = 2
        Me.TabPage7.Text = "Namespaces"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'ListBoxIncludedNamespaces
        '
        Me.ListBoxIncludedNamespaces.FormattingEnabled = True
        Me.ListBoxIncludedNamespaces.Location = New System.Drawing.Point(19, 31)
        Me.ListBoxIncludedNamespaces.Name = "ListBoxIncludedNamespaces"
        Me.ListBoxIncludedNamespaces.Size = New System.Drawing.Size(550, 394)
        Me.ListBoxIncludedNamespaces.TabIndex = 5
        '
        'LabelPromptIncludedNamespaces
        '
        Me.LabelPromptIncludedNamespaces.AutoSize = True
        Me.LabelPromptIncludedNamespaces.Location = New System.Drawing.Point(16, 15)
        Me.LabelPromptIncludedNamespaces.Name = "LabelPromptIncludedNamespaces"
        Me.LabelPromptIncludedNamespaces.Size = New System.Drawing.Size(121, 13)
        Me.LabelPromptIncludedNamespaces.TabIndex = 4
        Me.LabelPromptIncludedNamespaces.Text = "Included Namespace/s:"
        '
        'TextBoxProjectName
        '
        Me.TextBoxProjectName.Location = New System.Drawing.Point(86, 18)
        Me.TextBoxProjectName.Name = "TextBoxProjectName"
        Me.TextBoxProjectName.Size = New System.Drawing.Size(262, 20)
        Me.TextBoxProjectName.TabIndex = 1
        '
        'LabelPromptGroupName
        '
        Me.LabelPromptGroupName.AutoSize = True
        Me.LabelPromptGroupName.Location = New System.Drawing.Point(6, 21)
        Me.LabelPromptGroupName.Name = "LabelPromptGroupName"
        Me.LabelPromptGroupName.Size = New System.Drawing.Size(74, 13)
        Me.LabelPromptGroupName.TabIndex = 0
        Me.LabelPromptGroupName.Text = "Project Name:"
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(690, 22)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(69, 24)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "&OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(690, 52)
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
        'CheckBoxLeastRestrictiveCreate
        '
        Me.CheckBoxLeastRestrictiveCreate.AutoSize = True
        Me.CheckBoxLeastRestrictiveCreate.Location = New System.Drawing.Point(96, 452)
        Me.CheckBoxLeastRestrictiveCreate.Name = "CheckBoxLeastRestrictiveCreate"
        Me.CheckBoxLeastRestrictiveCreate.Size = New System.Drawing.Size(57, 17)
        Me.CheckBoxLeastRestrictiveCreate.TabIndex = 12
        Me.CheckBoxLeastRestrictiveCreate.Text = "Create"
        Me.CheckBoxLeastRestrictiveCreate.UseVisualStyleBackColor = True
        '
        'CheckBoxLeastRestrictiveAlter
        '
        Me.CheckBoxLeastRestrictiveAlter.AutoSize = True
        Me.CheckBoxLeastRestrictiveAlter.Location = New System.Drawing.Point(212, 452)
        Me.CheckBoxLeastRestrictiveAlter.Name = "CheckBoxLeastRestrictiveAlter"
        Me.CheckBoxLeastRestrictiveAlter.Size = New System.Drawing.Size(47, 17)
        Me.CheckBoxLeastRestrictiveAlter.TabIndex = 11
        Me.CheckBoxLeastRestrictiveAlter.Text = "Alter"
        Me.CheckBoxLeastRestrictiveAlter.UseVisualStyleBackColor = True
        '
        'CheckBoxLeastRestrictiveRead
        '
        Me.CheckBoxLeastRestrictiveRead.AutoSize = True
        Me.CheckBoxLeastRestrictiveRead.Location = New System.Drawing.Point(159, 452)
        Me.CheckBoxLeastRestrictiveRead.Name = "CheckBoxLeastRestrictiveRead"
        Me.CheckBoxLeastRestrictiveRead.Size = New System.Drawing.Size(52, 17)
        Me.CheckBoxLeastRestrictiveRead.TabIndex = 10
        Me.CheckBoxLeastRestrictiveRead.Text = "Read"
        Me.CheckBoxLeastRestrictiveRead.UseVisualStyleBackColor = True
        '
        'CheckBoxLeastRestrictivenoPermission
        '
        Me.CheckBoxLeastRestrictivenoPermission.AutoSize = True
        Me.CheckBoxLeastRestrictivenoPermission.Location = New System.Drawing.Point(15, 475)
        Me.CheckBoxLeastRestrictivenoPermission.Name = "CheckBoxLeastRestrictivenoPermission"
        Me.CheckBoxLeastRestrictivenoPermission.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxLeastRestrictivenoPermission.TabIndex = 9
        Me.CheckBoxLeastRestrictivenoPermission.Text = "No Permission"
        Me.CheckBoxLeastRestrictivenoPermission.UseVisualStyleBackColor = True
        '
        'CheckBoxLeastRestrictiveFullPermissions
        '
        Me.CheckBoxLeastRestrictiveFullPermissions.AutoSize = True
        Me.CheckBoxLeastRestrictiveFullPermissions.Location = New System.Drawing.Point(15, 452)
        Me.CheckBoxLeastRestrictiveFullPermissions.Name = "CheckBoxLeastRestrictiveFullPermissions"
        Me.CheckBoxLeastRestrictiveFullPermissions.Size = New System.Drawing.Size(75, 17)
        Me.CheckBoxLeastRestrictiveFullPermissions.TabIndex = 8
        Me.CheckBoxLeastRestrictiveFullPermissions.Text = "Full Rights"
        Me.CheckBoxLeastRestrictiveFullPermissions.UseVisualStyleBackColor = True
        '
        'LabelPromptLeastRestrictivePermissions
        '
        Me.LabelPromptLeastRestrictivePermissions.AutoSize = True
        Me.LabelPromptLeastRestrictivePermissions.Location = New System.Drawing.Point(12, 432)
        Me.LabelPromptLeastRestrictivePermissions.Name = "LabelPromptLeastRestrictivePermissions"
        Me.LabelPromptLeastRestrictivePermissions.Size = New System.Drawing.Size(273, 13)
        Me.LabelPromptLeastRestrictivePermissions.TabIndex = 13
        Me.LabelPromptLeastRestrictivePermissions.Text = "Least Restrictive Permissions (Between User and Group)"
        '
        'frmCRUDEditProject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(879, 733)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDEditProject"
        Me.TabText = "Edit Project"
        Me.Text = "Edit Project"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.PanelInviteUser.ResumeLayout(False)
        Me.PanelInviteUser.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabControl3.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.PanelInviteGroup.ResumeLayout(False)
        Me.PanelInviteGroup.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxProjectName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptGroupName As System.Windows.Forms.Label
    Friend WithEvents ButtonOkay As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents ButtonIncludeUser As System.Windows.Forms.Button
    Friend WithEvents LabelPromptAvailableUsers As System.Windows.Forms.Label
    Friend WithEvents ListBoxAvailableUsers As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedUsers As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents LabelPromptUser As System.Windows.Forms.Label
    Friend WithEvents ComboBoxUserPermissions As MTGCComboBox
    Friend WithEvents CheckBoxUserPermissionCreate As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionAlter As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionRead As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionNoPermission As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxUserPermissionFullRights As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabControl3 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents ButtonExcludeGroup As System.Windows.Forms.Button
    Friend WithEvents ButtonIncludeGroup As System.Windows.Forms.Button
    Friend WithEvents LabelPromptAvailableGroups As System.Windows.Forms.Label
    Friend WithEvents ListBoxAvailableGroups As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedGroups As System.Windows.Forms.Label
    Friend WithEvents ListBoxIncludedGroups As System.Windows.Forms.ListBox
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents ComboBoxGroupPermissions As System.Windows.Forms.ComboBox
    Friend WithEvents CheckBoxGroupPermissionCreate As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxGroupPermissionAlter As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxGroupPermissionRead As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxGroupPermissionNoPermission As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxGroupPermissionFullRights As System.Windows.Forms.CheckBox
    Friend WithEvents LabelPromptGroup As System.Windows.Forms.Label
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents LabelCreatedByUser As System.Windows.Forms.Label
    Friend WithEvents LabelPromptCreatedByUser As System.Windows.Forms.Label
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents ListBoxIncludedNamespaces As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedNamespaces As System.Windows.Forms.Label
    Friend WithEvents PanelInviteUser As System.Windows.Forms.Panel
    Friend WithEvents FlexibleListBox As Boston.FlexibleListBox
    Friend WithEvents TextBoxUserName As System.Windows.Forms.TextBox
    Friend WithEvents PanelInviteGroup As System.Windows.Forms.Panel
    Friend WithEvents TextBoxGroupInvite As System.Windows.Forms.TextBox
    Friend WithEvents FlexibleListBoxGroupInvite As Boston.FlexibleListBox
    Friend WithEvents FlexibleListBoxIncludedUser As Boston.FlexibleListBox
    Friend WithEvents LabelHelpTipsUser As System.Windows.Forms.Label
    Friend WithEvents LabelHelpTipsGroup As System.Windows.Forms.Label
    Friend WithEvents LabelPromptGroupUsersInProject As System.Windows.Forms.Label
    Friend WithEvents ListBoxGroupUsers As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptLeastRestrictivePermissions As System.Windows.Forms.Label
    Friend WithEvents CheckBoxLeastRestrictiveCreate As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLeastRestrictiveAlter As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLeastRestrictiveRead As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLeastRestrictivenoPermission As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxLeastRestrictiveFullPermissions As System.Windows.Forms.CheckBox
End Class
