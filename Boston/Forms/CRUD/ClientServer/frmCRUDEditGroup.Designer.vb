<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCRUDEditGroup
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
        Me.ButtonCancel = New System.Windows.Forms.Button()
        Me.ButtonOkay = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LabelCreatedByUser = New System.Windows.Forms.Label()
        Me.LabelPromptCreatedByUser = New System.Windows.Forms.Label()
        Me.GroupBoxIncludedUsers = New System.Windows.Forms.GroupBox()
        Me.ButtonExcludeUser = New System.Windows.Forms.Button()
        Me.ButtonIncludeUser = New System.Windows.Forms.Button()
        Me.LabelPromptAvailableUsers = New System.Windows.Forms.Label()
        Me.LabelPromptIncludedUsers = New System.Windows.Forms.Label()
        Me.ListBoxIncludedUsers = New System.Windows.Forms.ListBox()
        Me.PanelInviteUser = New System.Windows.Forms.Panel()
        Me.TextBoxUserName = New System.Windows.Forms.TextBox()
        Me.FlexibleListBox = New FlexibleListBox()
        Me.ListBoxAvailableUsers = New System.Windows.Forms.ListBox()
        Me.TextBoxGroupName = New System.Windows.Forms.TextBox()
        Me.LabelPromptGroupName = New System.Windows.Forms.Label()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBoxIncludedUsers.SuspendLayout()
        Me.PanelInviteUser.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(614, 49)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(69, 24)
        Me.ButtonCancel.TabIndex = 5
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(614, 19)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(69, 24)
        Me.ButtonOkay.TabIndex = 4
        Me.ButtonOkay.Text = "&OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.LabelPromptCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.GroupBoxIncludedUsers)
        Me.GroupBox1.Controls.Add(Me.TextBoxGroupName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptGroupName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(589, 539)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        '
        'LabelCreatedByUser
        '
        Me.LabelCreatedByUser.AutoSize = True
        Me.LabelCreatedByUser.Location = New System.Drawing.Point(79, 48)
        Me.LabelCreatedByUser.Name = "LabelCreatedByUser"
        Me.LabelCreatedByUser.Size = New System.Drawing.Size(104, 13)
        Me.LabelCreatedByUser.TabIndex = 4
        Me.LabelCreatedByUser.Text = "LabelCreatedByUser"
        '
        'LabelPromptCreatedByUser
        '
        Me.LabelPromptCreatedByUser.AutoSize = True
        Me.LabelPromptCreatedByUser.Location = New System.Drawing.Point(15, 48)
        Me.LabelPromptCreatedByUser.Name = "LabelPromptCreatedByUser"
        Me.LabelPromptCreatedByUser.Size = New System.Drawing.Size(61, 13)
        Me.LabelPromptCreatedByUser.TabIndex = 3
        Me.LabelPromptCreatedByUser.Text = "Created by:"
        '
        'GroupBoxIncludedUsers
        '
        Me.GroupBoxIncludedUsers.BackColor = System.Drawing.Color.White
        Me.GroupBoxIncludedUsers.Controls.Add(Me.ButtonExcludeUser)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.ButtonIncludeUser)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.LabelPromptAvailableUsers)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.LabelPromptIncludedUsers)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.ListBoxIncludedUsers)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.PanelInviteUser)
        Me.GroupBoxIncludedUsers.Controls.Add(Me.ListBoxAvailableUsers)
        Me.GroupBoxIncludedUsers.Location = New System.Drawing.Point(9, 77)
        Me.GroupBoxIncludedUsers.Name = "GroupBoxIncludedUsers"
        Me.GroupBoxIncludedUsers.Size = New System.Drawing.Size(563, 446)
        Me.GroupBoxIncludedUsers.TabIndex = 2
        Me.GroupBoxIncludedUsers.TabStop = False
        Me.GroupBoxIncludedUsers.Text = "Included Users"
        '
        'ButtonExcludeUser
        '
        Me.ButtonExcludeUser.Location = New System.Drawing.Point(240, 73)
        Me.ButtonExcludeUser.Name = "ButtonExcludeUser"
        Me.ButtonExcludeUser.Size = New System.Drawing.Size(63, 25)
        Me.ButtonExcludeUser.TabIndex = 11
        Me.ButtonExcludeUser.Text = ">>"
        Me.ButtonExcludeUser.UseVisualStyleBackColor = True
        '
        'ButtonIncludeUser
        '
        Me.ButtonIncludeUser.Location = New System.Drawing.Point(240, 42)
        Me.ButtonIncludeUser.Name = "ButtonIncludeUser"
        Me.ButtonIncludeUser.Size = New System.Drawing.Size(64, 25)
        Me.ButtonIncludeUser.TabIndex = 10
        Me.ButtonIncludeUser.Text = "<<"
        Me.ButtonIncludeUser.UseVisualStyleBackColor = True
        '
        'LabelPromptAvailableUsers
        '
        Me.LabelPromptAvailableUsers.AutoSize = True
        Me.LabelPromptAvailableUsers.Location = New System.Drawing.Point(310, 25)
        Me.LabelPromptAvailableUsers.Name = "LabelPromptAvailableUsers"
        Me.LabelPromptAvailableUsers.Size = New System.Drawing.Size(80, 13)
        Me.LabelPromptAvailableUsers.TabIndex = 9
        Me.LabelPromptAvailableUsers.Text = "Available Users"
        '
        'LabelPromptIncludedUsers
        '
        Me.LabelPromptIncludedUsers.AutoSize = True
        Me.LabelPromptIncludedUsers.Location = New System.Drawing.Point(15, 25)
        Me.LabelPromptIncludedUsers.Name = "LabelPromptIncludedUsers"
        Me.LabelPromptIncludedUsers.Size = New System.Drawing.Size(78, 13)
        Me.LabelPromptIncludedUsers.TabIndex = 7
        Me.LabelPromptIncludedUsers.Text = "Included Users"
        '
        'ListBoxIncludedUsers
        '
        Me.ListBoxIncludedUsers.FormattingEnabled = True
        Me.ListBoxIncludedUsers.Location = New System.Drawing.Point(15, 41)
        Me.ListBoxIncludedUsers.Name = "ListBoxIncludedUsers"
        Me.ListBoxIncludedUsers.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxIncludedUsers.Sorted = True
        Me.ListBoxIncludedUsers.TabIndex = 6
        '
        'PanelInviteUser
        '
        Me.PanelInviteUser.BackColor = System.Drawing.Color.White
        Me.PanelInviteUser.Controls.Add(Me.TextBoxUserName)
        Me.PanelInviteUser.Controls.Add(Me.FlexibleListBox)
        Me.PanelInviteUser.Location = New System.Drawing.Point(313, 41)
        Me.PanelInviteUser.Name = "PanelInviteUser"
        Me.PanelInviteUser.Size = New System.Drawing.Size(215, 394)
        Me.PanelInviteUser.TabIndex = 12
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
        Me.FlexibleListBox.BackColor = System.Drawing.Color.Transparent
        Me.FlexibleListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlexibleListBox.Location = New System.Drawing.Point(0, 31)
        Me.FlexibleListBox.Name = "FlexibleListBox"
        Me.FlexibleListBox.Size = New System.Drawing.Size(215, 362)
        Me.FlexibleListBox.TabIndex = 1
        '
        'ListBoxAvailableUsers
        '
        Me.ListBoxAvailableUsers.FormattingEnabled = True
        Me.ListBoxAvailableUsers.Location = New System.Drawing.Point(313, 41)
        Me.ListBoxAvailableUsers.Name = "ListBoxAvailableUsers"
        Me.ListBoxAvailableUsers.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxAvailableUsers.Sorted = True
        Me.ListBoxAvailableUsers.TabIndex = 8
        '
        'TextBoxGroupName
        '
        Me.TextBoxGroupName.Location = New System.Drawing.Point(82, 18)
        Me.TextBoxGroupName.Name = "TextBoxGroupName"
        Me.TextBoxGroupName.Size = New System.Drawing.Size(262, 20)
        Me.TextBoxGroupName.TabIndex = 1
        '
        'LabelPromptGroupName
        '
        Me.LabelPromptGroupName.AutoSize = True
        Me.LabelPromptGroupName.Location = New System.Drawing.Point(6, 21)
        Me.LabelPromptGroupName.Name = "LabelPromptGroupName"
        Me.LabelPromptGroupName.Size = New System.Drawing.Size(70, 13)
        Me.LabelPromptGroupName.TabIndex = 0
        Me.LabelPromptGroupName.Text = "Group Name:"
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'frmCRUDEditGroup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(761, 563)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDEditGroup"
        Me.TabText = "Edit Group"
        Me.Text = "Edit Group"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBoxIncludedUsers.ResumeLayout(False)
        Me.GroupBoxIncludedUsers.PerformLayout()
        Me.PanelInviteUser.ResumeLayout(False)
        Me.PanelInviteUser.PerformLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonOkay As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxIncludedUsers As System.Windows.Forms.GroupBox
    Friend WithEvents TextBoxGroupName As System.Windows.Forms.TextBox
    Friend WithEvents LabelPromptGroupName As System.Windows.Forms.Label
    Friend WithEvents ButtonExcludeUser As System.Windows.Forms.Button
    Friend WithEvents ButtonIncludeUser As System.Windows.Forms.Button
    Friend WithEvents LabelPromptAvailableUsers As System.Windows.Forms.Label
    Friend WithEvents ListBoxAvailableUsers As System.Windows.Forms.ListBox
    Friend WithEvents LabelPromptIncludedUsers As System.Windows.Forms.Label
    Friend WithEvents ListBoxIncludedUsers As System.Windows.Forms.ListBox
    Friend WithEvents ErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents PanelInviteUser As System.Windows.Forms.Panel
    Friend WithEvents TextBoxUserName As System.Windows.Forms.TextBox
    Friend WithEvents FlexibleListBox As FlexibleListBox
    Friend WithEvents LabelCreatedByUser As System.Windows.Forms.Label
    Friend WithEvents LabelPromptCreatedByUser As System.Windows.Forms.Label
End Class
