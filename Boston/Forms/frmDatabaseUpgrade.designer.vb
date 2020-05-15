<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDatabaseUpgrade
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDatabaseUpgrade))
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.picturebox_viev_logo = New System.Windows.Forms.PictureBox()
        Me.panel_main = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.labelprompt_upgrade_version_required = New System.Windows.Forms.Label()
        Me.label_upgrade_version_required = New System.Windows.Forms.Label()
        Me.LabelUpgradeCompletionStatus = New System.Windows.Forms.Label()
        Me.progressbar = New System.Windows.Forms.ProgressBar()
        Me.button_upgrade = New System.Windows.Forms.Button()
        Me.label_current_upgrade_status = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.labelprompt_upgrades_required_to_perform = New System.Windows.Forms.Label()
        Me.listbox_upgrades_performed = New System.Windows.Forms.ListBox()
        Me.listbox_upgrades_required = New System.Windows.Forms.ListBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.label_application_version_nr = New System.Windows.Forms.Label()
        Me.LabelCurrentDatabaseVersionNr = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.labelprompt_current_database_version_nr = New System.Windows.Forms.Label()
        Me.labelprompt_database_upgrade_facility = New System.Windows.Forms.Label()
        Me.labelprompt_rosters_logo = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.picturebox_viev_logo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panel_main.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button_cancel
        '
        Me.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.button_cancel.Location = New System.Drawing.Point(553, 11)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(68, 26)
        Me.button_cancel.TabIndex = 8
        Me.button_cancel.Text = "&Close"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'picturebox_viev_logo
        '
        Me.picturebox_viev_logo.Image = CType(resources.GetObject("picturebox_viev_logo.Image"), System.Drawing.Image)
        Me.picturebox_viev_logo.Location = New System.Drawing.Point(12, 13)
        Me.picturebox_viev_logo.Name = "picturebox_viev_logo"
        Me.picturebox_viev_logo.Size = New System.Drawing.Size(95, 40)
        Me.picturebox_viev_logo.TabIndex = 0
        Me.picturebox_viev_logo.TabStop = False
        '
        'panel_main
        '
        Me.panel_main.BackColor = System.Drawing.Color.White
        Me.panel_main.Controls.Add(Me.TabControl1)
        Me.panel_main.Controls.Add(Me.label_application_version_nr)
        Me.panel_main.Controls.Add(Me.LabelCurrentDatabaseVersionNr)
        Me.panel_main.Controls.Add(Me.Label1)
        Me.panel_main.Controls.Add(Me.labelprompt_current_database_version_nr)
        Me.panel_main.Controls.Add(Me.labelprompt_database_upgrade_facility)
        Me.panel_main.Controls.Add(Me.labelprompt_rosters_logo)
        Me.panel_main.Controls.Add(Me.PictureBox1)
        Me.panel_main.Controls.Add(Me.picturebox_viev_logo)
        Me.panel_main.Location = New System.Drawing.Point(10, 11)
        Me.panel_main.Name = "panel_main"
        Me.panel_main.Size = New System.Drawing.Size(534, 317)
        Me.panel_main.TabIndex = 9
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(16, 138)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(501, 161)
        Me.TabControl1.TabIndex = 10
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.labelprompt_upgrade_version_required)
        Me.TabPage1.Controls.Add(Me.label_upgrade_version_required)
        Me.TabPage1.Controls.Add(Me.LabelUpgradeCompletionStatus)
        Me.TabPage1.Controls.Add(Me.progressbar)
        Me.TabPage1.Controls.Add(Me.button_upgrade)
        Me.TabPage1.Controls.Add(Me.label_current_upgrade_status)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(493, 135)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Current Upgrade Required"
        '
        'labelprompt_upgrade_version_required
        '
        Me.labelprompt_upgrade_version_required.Location = New System.Drawing.Point(19, 28)
        Me.labelprompt_upgrade_version_required.Name = "labelprompt_upgrade_version_required"
        Me.labelprompt_upgrade_version_required.Size = New System.Drawing.Size(160, 26)
        Me.labelprompt_upgrade_version_required.TabIndex = 5
        Me.labelprompt_upgrade_version_required.Text = "Upgrade Database Version Nr (this upgrade is required) :"
        Me.labelprompt_upgrade_version_required.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'label_upgrade_version_required
        '
        Me.label_upgrade_version_required.Location = New System.Drawing.Point(190, 29)
        Me.label_upgrade_version_required.Name = "label_upgrade_version_required"
        Me.label_upgrade_version_required.Size = New System.Drawing.Size(48, 22)
        Me.label_upgrade_version_required.TabIndex = 4
        Me.label_upgrade_version_required.Text = "label_upgrade_version_required"
        '
        'LabelUpgradeCompletionStatus
        '
        Me.LabelUpgradeCompletionStatus.AutoSize = True
        Me.LabelUpgradeCompletionStatus.Location = New System.Drawing.Point(19, 108)
        Me.LabelUpgradeCompletionStatus.Name = "LabelUpgradeCompletionStatus"
        Me.LabelUpgradeCompletionStatus.Size = New System.Drawing.Size(165, 13)
        Me.LabelUpgradeCompletionStatus.TabIndex = 3
        Me.LabelUpgradeCompletionStatus.Text = "label_upgrade_completion_status"
        '
        'progressbar
        '
        Me.progressbar.Location = New System.Drawing.Point(17, 80)
        Me.progressbar.Name = "progressbar"
        Me.progressbar.Size = New System.Drawing.Size(459, 16)
        Me.progressbar.TabIndex = 2
        '
        'button_upgrade
        '
        Me.button_upgrade.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.button_upgrade.ForeColor = System.Drawing.SystemColors.ControlText
        Me.button_upgrade.Location = New System.Drawing.Point(245, 29)
        Me.button_upgrade.Name = "button_upgrade"
        Me.button_upgrade.Size = New System.Drawing.Size(118, 25)
        Me.button_upgrade.TabIndex = 1
        Me.button_upgrade.Text = "&Upgrade Now"
        Me.button_upgrade.UseVisualStyleBackColor = False
        '
        'label_current_upgrade_status
        '
        Me.label_current_upgrade_status.BackColor = System.Drawing.SystemColors.Info
        Me.label_current_upgrade_status.Location = New System.Drawing.Point(14, 28)
        Me.label_current_upgrade_status.Name = "label_current_upgrade_status"
        Me.label_current_upgrade_status.Size = New System.Drawing.Size(459, 42)
        Me.label_current_upgrade_status.TabIndex = 0
        Me.label_current_upgrade_status.Text = "Your Boston database is now at the correct version. Please click [Close] to start" & _
    " using Boston."
        Me.label_current_upgrade_status.Visible = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.labelprompt_upgrades_required_to_perform)
        Me.TabPage2.Controls.Add(Me.listbox_upgrades_performed)
        Me.TabPage2.Controls.Add(Me.listbox_upgrades_required)
        Me.TabPage2.Controls.Add(Me.PictureBox2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(493, 135)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Database Upgrade History"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(260, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(172, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Upgrades Successfully Performed :"
        '
        'labelprompt_upgrades_required_to_perform
        '
        Me.labelprompt_upgrades_required_to_perform.AutoSize = True
        Me.labelprompt_upgrades_required_to_perform.Location = New System.Drawing.Point(13, 23)
        Me.labelprompt_upgrades_required_to_perform.Name = "labelprompt_upgrades_required_to_perform"
        Me.labelprompt_upgrades_required_to_perform.Size = New System.Drawing.Size(156, 13)
        Me.labelprompt_upgrades_required_to_perform.TabIndex = 3
        Me.labelprompt_upgrades_required_to_perform.Text = "Upgrades Required to Perform :"
        '
        'listbox_upgrades_performed
        '
        Me.listbox_upgrades_performed.FormattingEnabled = True
        Me.listbox_upgrades_performed.Location = New System.Drawing.Point(263, 41)
        Me.listbox_upgrades_performed.Name = "listbox_upgrades_performed"
        Me.listbox_upgrades_performed.Size = New System.Drawing.Size(217, 82)
        Me.listbox_upgrades_performed.TabIndex = 2
        '
        'listbox_upgrades_required
        '
        Me.listbox_upgrades_required.FormattingEnabled = True
        Me.listbox_upgrades_required.Location = New System.Drawing.Point(15, 41)
        Me.listbox_upgrades_required.Name = "listbox_upgrades_required"
        Me.listbox_upgrades_required.Size = New System.Drawing.Size(217, 82)
        Me.listbox_upgrades_required.TabIndex = 1
        '
        'PictureBox2
        '
        Me.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox2.Location = New System.Drawing.Point(246, 17)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(1, 163)
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'label_application_version_nr
        '
        Me.label_application_version_nr.AutoSize = True
        Me.label_application_version_nr.ForeColor = System.Drawing.Color.Gray
        Me.label_application_version_nr.Location = New System.Drawing.Point(210, 106)
        Me.label_application_version_nr.Name = "label_application_version_nr"
        Me.label_application_version_nr.Size = New System.Drawing.Size(141, 13)
        Me.label_application_version_nr.TabIndex = 8
        Me.label_application_version_nr.Text = "label_application_version_nr"
        '
        'LabelCurrentDatabaseVersionNr
        '
        Me.LabelCurrentDatabaseVersionNr.AutoSize = True
        Me.LabelCurrentDatabaseVersionNr.Location = New System.Drawing.Point(210, 84)
        Me.LabelCurrentDatabaseVersionNr.Name = "LabelCurrentDatabaseVersionNr"
        Me.LabelCurrentDatabaseVersionNr.Size = New System.Drawing.Size(159, 13)
        Me.LabelCurrentDatabaseVersionNr.TabIndex = 7
        Me.LabelCurrentDatabaseVersionNr.Text = "LabelCurrentDatabaseVersionNr"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Gray
        Me.Label1.Location = New System.Drawing.Point(13, 106)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Current Boston Application Version :"
        '
        'labelprompt_current_database_version_nr
        '
        Me.labelprompt_current_database_version_nr.AutoSize = True
        Me.labelprompt_current_database_version_nr.Location = New System.Drawing.Point(56, 84)
        Me.labelprompt_current_database_version_nr.Name = "labelprompt_current_database_version_nr"
        Me.labelprompt_current_database_version_nr.Size = New System.Drawing.Size(148, 13)
        Me.labelprompt_current_database_version_nr.TabIndex = 4
        Me.labelprompt_current_database_version_nr.Text = "Current Database Version Nr :"
        '
        'labelprompt_database_upgrade_facility
        '
        Me.labelprompt_database_upgrade_facility.AutoSize = True
        Me.labelprompt_database_upgrade_facility.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelprompt_database_upgrade_facility.Location = New System.Drawing.Point(317, 33)
        Me.labelprompt_database_upgrade_facility.Name = "labelprompt_database_upgrade_facility"
        Me.labelprompt_database_upgrade_facility.Size = New System.Drawing.Size(170, 16)
        Me.labelprompt_database_upgrade_facility.TabIndex = 3
        Me.labelprompt_database_upgrade_facility.Text = "Database Upgrade Facility"
        '
        'labelprompt_rosters_logo
        '
        Me.labelprompt_rosters_logo.AutoSize = True
        Me.labelprompt_rosters_logo.Font = New System.Drawing.Font("Arial Narrow", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelprompt_rosters_logo.Location = New System.Drawing.Point(127, 13)
        Me.labelprompt_rosters_logo.Name = "labelprompt_rosters_logo"
        Me.labelprompt_rosters_logo.Size = New System.Drawing.Size(119, 42)
        Me.labelprompt_rosters_logo.TabIndex = 2
        Me.labelprompt_rosters_logo.Text = "Boston"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(11, 66)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(495, 1)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'frmDatabaseUpgrade
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 342)
        Me.ControlBox = False
        Me.Controls.Add(Me.panel_main)
        Me.Controls.Add(Me.button_cancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDatabaseUpgrade"
        Me.Text = "Boston Database Version Upgrade Facility"
        CType(Me.picturebox_viev_logo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panel_main.ResumeLayout(False)
        Me.panel_main.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents picturebox_viev_logo As System.Windows.Forms.PictureBox
    Friend WithEvents panel_main As System.Windows.Forms.Panel
    Friend WithEvents labelprompt_database_upgrade_facility As System.Windows.Forms.Label
    Friend WithEvents labelprompt_rosters_logo As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents labelprompt_current_database_version_nr As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents label_application_version_nr As System.Windows.Forms.Label
    Friend WithEvents LabelCurrentDatabaseVersionNr As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents label_current_upgrade_status As System.Windows.Forms.Label
    Friend WithEvents button_upgrade As System.Windows.Forms.Button
    Friend WithEvents LabelUpgradeCompletionStatus As System.Windows.Forms.Label
    Friend WithEvents progressbar As System.Windows.Forms.ProgressBar
    Friend WithEvents labelprompt_upgrades_required_to_perform As System.Windows.Forms.Label
    Friend WithEvents listbox_upgrades_performed As System.Windows.Forms.ListBox
    Friend WithEvents listbox_upgrades_required As System.Windows.Forms.ListBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents labelprompt_upgrade_version_required As System.Windows.Forms.Label
    Friend WithEvents label_upgrade_version_required As System.Windows.Forms.Label
End Class
