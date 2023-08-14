<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmModelShareWithProject
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModelShareWithProject))
        Me.GroupBoxMoveModel = New System.Windows.Forms.GroupBox()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.ButtonShareModel = New System.Windows.Forms.Button()
        Me.GroupBoxCurrentLocation = New System.Windows.Forms.GroupBox()
        Me.LabelCurrentNamespce = New System.Windows.Forms.Label()
        Me.LabelCurrentProject = New System.Windows.Forms.Label()
        Me.LabelPromptCurrentNamespace = New System.Windows.Forms.Label()
        Me.LabelPromptCurrentProject = New System.Windows.Forms.Label()
        Me.ComboBoxNamespace = New System.Windows.Forms.ComboBox()
        Me.LabelPromptNamespace = New System.Windows.Forms.Label()
        Me.ComboBoxProject = New System.Windows.Forms.ComboBox()
        Me.LabelPromptProject = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.GroupBoxMoveModel.SuspendLayout()
        Me.GroupBoxCurrentLocation.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxMoveModel
        '
        Me.GroupBoxMoveModel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxMoveModel.Controls.Add(Me.ButtonClose)
        Me.GroupBoxMoveModel.Controls.Add(Me.ButtonShareModel)
        Me.GroupBoxMoveModel.Controls.Add(Me.GroupBoxCurrentLocation)
        Me.GroupBoxMoveModel.Controls.Add(Me.ComboBoxNamespace)
        Me.GroupBoxMoveModel.Controls.Add(Me.LabelPromptNamespace)
        Me.GroupBoxMoveModel.Controls.Add(Me.ComboBoxProject)
        Me.GroupBoxMoveModel.Controls.Add(Me.LabelPromptProject)
        Me.GroupBoxMoveModel.Controls.Add(Me.LabelModelName)
        Me.GroupBoxMoveModel.Controls.Add(Me.LabelPromptModel)
        Me.GroupBoxMoveModel.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxMoveModel.Name = "GroupBoxMoveModel"
        Me.GroupBoxMoveModel.Size = New System.Drawing.Size(461, 247)
        Me.GroupBoxMoveModel.TabIndex = 0
        Me.GroupBoxMoveModel.TabStop = False
        Me.GroupBoxMoveModel.Text = "Move Model:"
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Location = New System.Drawing.Point(237, 205)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(59, 25)
        Me.ButtonClose.TabIndex = 8
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ButtonShareModel
        '
        Me.ButtonShareModel.Location = New System.Drawing.Point(149, 205)
        Me.ButtonShareModel.Name = "ButtonShareModel"
        Me.ButtonShareModel.Size = New System.Drawing.Size(82, 25)
        Me.ButtonShareModel.TabIndex = 7
        Me.ButtonShareModel.Text = "&Share Model"
        Me.ButtonShareModel.UseVisualStyleBackColor = True
        '
        'GroupBoxCurrentLocation
        '
        Me.GroupBoxCurrentLocation.Controls.Add(Me.LabelCurrentNamespce)
        Me.GroupBoxCurrentLocation.Controls.Add(Me.LabelCurrentProject)
        Me.GroupBoxCurrentLocation.Controls.Add(Me.LabelPromptCurrentNamespace)
        Me.GroupBoxCurrentLocation.Controls.Add(Me.LabelPromptCurrentProject)
        Me.GroupBoxCurrentLocation.Location = New System.Drawing.Point(9, 108)
        Me.GroupBoxCurrentLocation.Name = "GroupBoxCurrentLocation"
        Me.GroupBoxCurrentLocation.Size = New System.Drawing.Size(437, 81)
        Me.GroupBoxCurrentLocation.TabIndex = 6
        Me.GroupBoxCurrentLocation.TabStop = False
        Me.GroupBoxCurrentLocation.Text = "Current Location:"
        '
        'LabelCurrentNamespce
        '
        Me.LabelCurrentNamespce.AutoSize = True
        Me.LabelCurrentNamespce.Location = New System.Drawing.Point(79, 54)
        Me.LabelCurrentNamespce.Name = "LabelCurrentNamespce"
        Me.LabelCurrentNamespce.Size = New System.Drawing.Size(118, 13)
        Me.LabelCurrentNamespce.TabIndex = 8
        Me.LabelCurrentNamespce.Text = "LabelCurrentNamespce"
        '
        'LabelCurrentProject
        '
        Me.LabelCurrentProject.AutoSize = True
        Me.LabelCurrentProject.Location = New System.Drawing.Point(55, 27)
        Me.LabelCurrentProject.Name = "LabelCurrentProject"
        Me.LabelCurrentProject.Size = New System.Drawing.Size(100, 13)
        Me.LabelCurrentProject.TabIndex = 7
        Me.LabelCurrentProject.Text = "LabelCurrentProject"
        '
        'LabelPromptCurrentNamespace
        '
        Me.LabelPromptCurrentNamespace.AutoSize = True
        Me.LabelPromptCurrentNamespace.Location = New System.Drawing.Point(6, 54)
        Me.LabelPromptCurrentNamespace.Name = "LabelPromptCurrentNamespace"
        Me.LabelPromptCurrentNamespace.Size = New System.Drawing.Size(67, 13)
        Me.LabelPromptCurrentNamespace.TabIndex = 6
        Me.LabelPromptCurrentNamespace.Text = "Namespace:"
        '
        'LabelPromptCurrentProject
        '
        Me.LabelPromptCurrentProject.AutoSize = True
        Me.LabelPromptCurrentProject.Location = New System.Drawing.Point(6, 27)
        Me.LabelPromptCurrentProject.Name = "LabelPromptCurrentProject"
        Me.LabelPromptCurrentProject.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptCurrentProject.TabIndex = 5
        Me.LabelPromptCurrentProject.Text = "Project:"
        '
        'ComboBoxNamespace
        '
        Me.ComboBoxNamespace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxNamespace.FormattingEnabled = True
        Me.ComboBoxNamespace.Location = New System.Drawing.Point(79, 72)
        Me.ComboBoxNamespace.Name = "ComboBoxNamespace"
        Me.ComboBoxNamespace.Size = New System.Drawing.Size(367, 21)
        Me.ComboBoxNamespace.TabIndex = 5
        '
        'LabelPromptNamespace
        '
        Me.LabelPromptNamespace.AutoSize = True
        Me.LabelPromptNamespace.Location = New System.Drawing.Point(6, 75)
        Me.LabelPromptNamespace.Name = "LabelPromptNamespace"
        Me.LabelPromptNamespace.Size = New System.Drawing.Size(67, 13)
        Me.LabelPromptNamespace.TabIndex = 4
        Me.LabelPromptNamespace.Text = "Namespace:"
        '
        'ComboBoxProject
        '
        Me.ComboBoxProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxProject.FormattingEnabled = True
        Me.ComboBoxProject.Location = New System.Drawing.Point(54, 45)
        Me.ComboBoxProject.Name = "ComboBoxProject"
        Me.ComboBoxProject.Size = New System.Drawing.Size(392, 21)
        Me.ComboBoxProject.TabIndex = 3
        '
        'LabelPromptProject
        '
        Me.LabelPromptProject.AutoSize = True
        Me.LabelPromptProject.Location = New System.Drawing.Point(6, 48)
        Me.LabelPromptProject.Name = "LabelPromptProject"
        Me.LabelPromptProject.Size = New System.Drawing.Size(43, 13)
        Me.LabelPromptProject.TabIndex = 2
        Me.LabelPromptProject.Text = "Project:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(51, 23)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 1
        Me.LabelModelName.Text = "LabelModelName"
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(6, 23)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 0
        Me.LabelPromptModel.Text = "Model:"
        '
        'frmModelShareWithProject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(487, 271)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBoxMoveModel)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmModelShareWithProject"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Move Model"
        Me.GroupBoxMoveModel.ResumeLayout(False)
        Me.GroupBoxMoveModel.PerformLayout()
        Me.GroupBoxCurrentLocation.ResumeLayout(False)
        Me.GroupBoxCurrentLocation.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBoxMoveModel As GroupBox
    Friend WithEvents ButtonClose As Button
    Friend WithEvents ButtonShareModel As Button
    Friend WithEvents GroupBoxCurrentLocation As GroupBox
    Friend WithEvents LabelCurrentNamespce As Label
    Friend WithEvents LabelCurrentProject As Label
    Friend WithEvents LabelPromptCurrentNamespace As Label
    Friend WithEvents LabelPromptCurrentProject As Label
    Friend WithEvents ComboBoxNamespace As ComboBox
    Friend WithEvents LabelPromptNamespace As Label
    Friend WithEvents ComboBoxProject As ComboBox
    Friend WithEvents LabelPromptProject As Label
    Friend WithEvents LabelModelName As Label
    Friend WithEvents LabelPromptModel As Label
End Class
