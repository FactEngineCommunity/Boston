<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProjectUserDetails
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim CheckBoxProperties1 As PresentationControls.CheckBoxProperties = New PresentationControls.CheckBoxProperties()
        Me.LabelIncludedUserFullName = New System.Windows.Forms.Label()
        Me.CheckedComboBoxRole = New PresentationControls.CheckBoxComboBox()
        Me.ButtonRemoveUserFromProject = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LabelIncludedUserFullName
        '
        Me.LabelIncludedUserFullName.AutoSize = True
        Me.LabelIncludedUserFullName.BackColor = System.Drawing.Color.Transparent
        Me.LabelIncludedUserFullName.Location = New System.Drawing.Point(2, 3)
        Me.LabelIncludedUserFullName.MaximumSize = New System.Drawing.Size(100, 13)
        Me.LabelIncludedUserFullName.Name = "LabelIncludedUserFullName"
        Me.LabelIncludedUserFullName.Size = New System.Drawing.Size(96, 13)
        Me.LabelIncludedUserFullName.TabIndex = 0
        Me.LabelIncludedUserFullName.Text = "LabelIncludedUserFullName"
        '
        'CheckedComboBoxRole
        '
        CheckBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CheckedComboBoxRole.CheckBoxProperties = CheckBoxProperties1
        Me.CheckedComboBoxRole.DisplayMemberSingleItem = ""
        Me.CheckedComboBoxRole.FormattingEnabled = True
        Me.CheckedComboBoxRole.Location = New System.Drawing.Point(98, 0)
        Me.CheckedComboBoxRole.Name = "CheckedComboBoxRole"
        Me.CheckedComboBoxRole.Size = New System.Drawing.Size(173, 21)
        Me.CheckedComboBoxRole.TabIndex = 2
        '
        'ButtonRemoveUserFromProject
        '
        Me.ButtonRemoveUserFromProject.Image = Global.Boston.My.Resources.Resources.deleteround16x16
        Me.ButtonRemoveUserFromProject.Location = New System.Drawing.Point(273, 1)
        Me.ButtonRemoveUserFromProject.Name = "ButtonRemoveUserFromProject"
        Me.ButtonRemoveUserFromProject.Size = New System.Drawing.Size(18, 18)
        Me.ButtonRemoveUserFromProject.TabIndex = 3
        Me.ButtonRemoveUserFromProject.UseVisualStyleBackColor = True
        '
        'ProjectUserDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.ButtonRemoveUserFromProject)
        Me.Controls.Add(Me.LabelIncludedUserFullName)
        Me.Controls.Add(Me.CheckedComboBoxRole)
        Me.Name = "ProjectUserDetails"
        Me.Size = New System.Drawing.Size(292, 21)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelIncludedUserFullName As System.Windows.Forms.Label
    Friend WithEvents CheckedComboBoxRole As PresentationControls.CheckBoxComboBox
    Friend WithEvents ButtonRemoveUserFromProject As System.Windows.Forms.Button

End Class
