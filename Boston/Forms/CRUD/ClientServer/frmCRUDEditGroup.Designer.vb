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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageUsers = New System.Windows.Forms.TabPage()
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
        Me.TabPageOntologies = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ButtonRemoveOntology = New System.Windows.Forms.Button()
        Me.ButtonAddOntology = New System.Windows.Forms.Button()
        Me.LabelPromptAvailableOntologies = New System.Windows.Forms.Label()
        Me.LabelPromptIncludedOntologies = New System.Windows.Forms.Label()
        Me.ListBoxIncludedOntologies = New System.Windows.Forms.ListBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ListBoxAvailableOntologies = New System.Windows.Forms.ListBox()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.LabelCreatedByUser = New System.Windows.Forms.Label()
        Me.LabelPromptCreatedByUser = New System.Windows.Forms.Label()
        Me.TextBoxGroupName = New System.Windows.Forms.TextBox()
        Me.LabelPromptGroupName = New System.Windows.Forms.Label()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageUsers.SuspendLayout()
        Me.GroupBoxIncludedUsers.SuspendLayout()
        Me.PanelInviteUser.SuspendLayout()
        Me.TabPageOntologies.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(620, 49)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(69, 24)
        Me.ButtonCancel.TabIndex = 5
        Me.ButtonCancel.Text = "&Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Location = New System.Drawing.Point(620, 19)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(69, 24)
        Me.ButtonOkay.TabIndex = 4
        Me.ButtonOkay.Text = "&OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.TabControl1)
        Me.GroupBox1.Controls.Add(Me.LabelCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.LabelPromptCreatedByUser)
        Me.GroupBox1.Controls.Add(Me.TextBoxGroupName)
        Me.GroupBox1.Controls.Add(Me.LabelPromptGroupName)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(602, 580)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageUsers)
        Me.TabControl1.Controls.Add(Me.TabPageOntologies)
        Me.TabControl1.Location = New System.Drawing.Point(9, 83)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(580, 491)
        Me.TabControl1.TabIndex = 5
        '
        'TabPageUsers
        '
        Me.TabPageUsers.Controls.Add(Me.GroupBoxIncludedUsers)
        Me.TabPageUsers.Location = New System.Drawing.Point(4, 22)
        Me.TabPageUsers.Name = "TabPageUsers"
        Me.TabPageUsers.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageUsers.Size = New System.Drawing.Size(572, 465)
        Me.TabPageUsers.TabIndex = 0
        Me.TabPageUsers.Text = "Users"
        Me.TabPageUsers.UseVisualStyleBackColor = True
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
        Me.GroupBoxIncludedUsers.Location = New System.Drawing.Point(6, 6)
        Me.GroupBoxIncludedUsers.Name = "GroupBoxIncludedUsers"
        Me.GroupBoxIncludedUsers.Size = New System.Drawing.Size(551, 449)
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
        'TabPageOntologies
        '
        Me.TabPageOntologies.Controls.Add(Me.GroupBox2)
        Me.TabPageOntologies.Location = New System.Drawing.Point(4, 22)
        Me.TabPageOntologies.Name = "TabPageOntologies"
        Me.TabPageOntologies.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageOntologies.Size = New System.Drawing.Size(572, 465)
        Me.TabPageOntologies.TabIndex = 1
        Me.TabPageOntologies.Text = "Ontologies"
        Me.TabPageOntologies.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.White
        Me.GroupBox2.Controls.Add(Me.ButtonRemoveOntology)
        Me.GroupBox2.Controls.Add(Me.ButtonAddOntology)
        Me.GroupBox2.Controls.Add(Me.LabelPromptAvailableOntologies)
        Me.GroupBox2.Controls.Add(Me.LabelPromptIncludedOntologies)
        Me.GroupBox2.Controls.Add(Me.ListBoxIncludedOntologies)
        Me.GroupBox2.Controls.Add(Me.Panel1)
        Me.GroupBox2.Controls.Add(Me.ListBox2)
        Me.GroupBox2.Location = New System.Drawing.Point(11, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(551, 449)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Included Ontologies"
        '
        'ButtonRemoveOntology
        '
        Me.ButtonRemoveOntology.Location = New System.Drawing.Point(240, 73)
        Me.ButtonRemoveOntology.Name = "ButtonRemoveOntology"
        Me.ButtonRemoveOntology.Size = New System.Drawing.Size(63, 25)
        Me.ButtonRemoveOntology.TabIndex = 11
        Me.ButtonRemoveOntology.Text = ">>"
        Me.ButtonRemoveOntology.UseVisualStyleBackColor = True
        '
        'ButtonAddOntology
        '
        Me.ButtonAddOntology.Location = New System.Drawing.Point(240, 42)
        Me.ButtonAddOntology.Name = "ButtonAddOntology"
        Me.ButtonAddOntology.Size = New System.Drawing.Size(64, 25)
        Me.ButtonAddOntology.TabIndex = 10
        Me.ButtonAddOntology.Text = "<<"
        Me.ButtonAddOntology.UseVisualStyleBackColor = True
        '
        'LabelPromptAvailableOntologies
        '
        Me.LabelPromptAvailableOntologies.AutoSize = True
        Me.LabelPromptAvailableOntologies.Location = New System.Drawing.Point(310, 25)
        Me.LabelPromptAvailableOntologies.Name = "LabelPromptAvailableOntologies"
        Me.LabelPromptAvailableOntologies.Size = New System.Drawing.Size(103, 13)
        Me.LabelPromptAvailableOntologies.TabIndex = 9
        Me.LabelPromptAvailableOntologies.Text = "Available Ontologies"
        '
        'LabelPromptIncludedOntologies
        '
        Me.LabelPromptIncludedOntologies.AutoSize = True
        Me.LabelPromptIncludedOntologies.Location = New System.Drawing.Point(15, 25)
        Me.LabelPromptIncludedOntologies.Name = "LabelPromptIncludedOntologies"
        Me.LabelPromptIncludedOntologies.Size = New System.Drawing.Size(101, 13)
        Me.LabelPromptIncludedOntologies.TabIndex = 7
        Me.LabelPromptIncludedOntologies.Text = "Included Ontologies"
        '
        'ListBoxIncludedOntologies
        '
        Me.ListBoxIncludedOntologies.FormattingEnabled = True
        Me.ListBoxIncludedOntologies.Location = New System.Drawing.Point(15, 41)
        Me.ListBoxIncludedOntologies.Name = "ListBoxIncludedOntologies"
        Me.ListBoxIncludedOntologies.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxIncludedOntologies.Sorted = True
        Me.ListBoxIncludedOntologies.TabIndex = 6
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.ListBoxAvailableOntologies)
        Me.Panel1.Location = New System.Drawing.Point(313, 41)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(215, 394)
        Me.Panel1.TabIndex = 12
        '
        'ListBoxAvailableOntologies
        '
        Me.ListBoxAvailableOntologies.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBoxAvailableOntologies.FormattingEnabled = True
        Me.ListBoxAvailableOntologies.Location = New System.Drawing.Point(0, 0)
        Me.ListBoxAvailableOntologies.Name = "ListBoxAvailableOntologies"
        Me.ListBoxAvailableOntologies.Size = New System.Drawing.Size(215, 394)
        Me.ListBoxAvailableOntologies.TabIndex = 0
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(313, 41)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(215, 394)
        Me.ListBox2.Sorted = True
        Me.ListBox2.TabIndex = 8
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
        Me.ClientSize = New System.Drawing.Size(761, 604)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCRUDEditGroup"
        Me.TabText = "Edit Group"
        Me.Text = "Edit Group"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageUsers.ResumeLayout(False)
        Me.GroupBoxIncludedUsers.ResumeLayout(False)
        Me.GroupBoxIncludedUsers.PerformLayout()
        Me.PanelInviteUser.ResumeLayout(False)
        Me.PanelInviteUser.PerformLayout()
        Me.TabPageOntologies.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
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
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageUsers As TabPage
    Friend WithEvents TabPageOntologies As TabPage
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents ButtonRemoveOntology As Button
    Friend WithEvents ButtonAddOntology As Button
    Friend WithEvents LabelPromptAvailableOntologies As Label
    Friend WithEvents LabelPromptIncludedOntologies As Label
    Friend WithEvents ListBoxIncludedOntologies As ListBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ListBoxAvailableOntologies As ListBox
    Friend WithEvents ListBox2 As ListBox
End Class
