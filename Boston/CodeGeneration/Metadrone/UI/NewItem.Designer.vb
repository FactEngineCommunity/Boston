Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Friend Class NewItem
        Inherits System.Windows.Forms.Form

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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewItem))
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
            Me.OK_Button = New System.Windows.Forms.Button
            Me.Cancel_Button = New System.Windows.Forms.Button
            Me.lblProjectName = New System.Windows.Forms.Label
            Me.txtProjectName = New System.Windows.Forms.TextBox
            Me.txtPackageName = New System.Windows.Forms.TextBox
            Me.lblPackageName = New System.Windows.Forms.Label
            Me.rbPackage = New System.Windows.Forms.RadioButton
            Me.rbTemplate = New System.Windows.Forms.RadioButton
            Me.pnlProject = New System.Windows.Forms.Panel
            Me.chkNewProject = New System.Windows.Forms.CheckBox
            Me.picPackage = New System.Windows.Forms.PictureBox
            Me.picTemplate = New System.Windows.Forms.PictureBox
            Me.pnlTemplate = New System.Windows.Forms.Panel
            Me.Panel5 = New System.Windows.Forms.Panel
            Me.lblTemplateName = New System.Windows.Forms.Label
            Me.txtTemplate = New System.Windows.Forms.TextBox
            Me.pnlDBSource = New System.Windows.Forms.Panel
            Me.lblDBSourceName = New System.Windows.Forms.Label
            Me.txtDBSourceName = New System.Windows.Forms.TextBox
            Me.picDBSource = New System.Windows.Forms.PictureBox
            Me.rbDBSource = New System.Windows.Forms.RadioButton
            Me.lblMsg = New System.Windows.Forms.Label
            Me.picVB = New System.Windows.Forms.PictureBox
            Me.rbVB = New System.Windows.Forms.RadioButton
            Me.picCS = New System.Windows.Forms.PictureBox
            Me.rbCS = New System.Windows.Forms.RadioButton
            Me.pnlCodeFile = New System.Windows.Forms.Panel
            Me.lblCodeFileName = New System.Windows.Forms.Label
            Me.txtCodeFileName = New System.Windows.Forms.TextBox
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.lblDescription = New System.Windows.Forms.Label
            Me.Panel3 = New System.Windows.Forms.Panel
            Me.Panel4 = New System.Windows.Forms.Panel
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.picProject = New System.Windows.Forms.PictureBox
            Me.ttFast = New System.Windows.Forms.ToolTip(Me.components)
            Me.pnlContainer = New System.Windows.Forms.Panel
            Me.TableLayoutPanel1.SuspendLayout()
            Me.pnlProject.SuspendLayout()
            CType(Me.picPackage, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.Panel5.SuspendLayout()
            Me.pnlDBSource.SuspendLayout()
            CType(Me.picDBSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picVB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picCS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlCodeFile.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.Panel2.SuspendLayout()
            CType(Me.picProject, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlContainer.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(726, 184)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 1
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'OK_Button
            '
            Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.OK_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.OK_Button.Location = New System.Drawing.Point(3, 3)
            Me.OK_Button.Name = "OK_Button"
            Me.OK_Button.Size = New System.Drawing.Size(67, 23)
            Me.OK_Button.TabIndex = 0
            Me.OK_Button.Text = "OK"
            '
            'Cancel_Button
            '
            Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
            Me.Cancel_Button.Name = "Cancel_Button"
            Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
            Me.Cancel_Button.TabIndex = 1
            Me.Cancel_Button.Text = "Cancel"
            '
            'lblProjectName
            '
            Me.lblProjectName.AutoSize = True
            Me.lblProjectName.Location = New System.Drawing.Point(12, 85)
            Me.lblProjectName.Name = "lblProjectName"
            Me.lblProjectName.Size = New System.Drawing.Size(74, 13)
            Me.lblProjectName.TabIndex = 3
            Me.lblProjectName.Text = "Project Name:"
            '
            'txtProjectName
            '
            Me.txtProjectName.Location = New System.Drawing.Point(15, 101)
            Me.txtProjectName.Name = "txtProjectName"
            Me.txtProjectName.Size = New System.Drawing.Size(201, 20)
            Me.txtProjectName.TabIndex = 4
            '
            'txtPackageName
            '
            Me.txtPackageName.Location = New System.Drawing.Point(15, 29)
            Me.txtPackageName.Name = "txtPackageName"
            Me.txtPackageName.Size = New System.Drawing.Size(201, 20)
            Me.txtPackageName.TabIndex = 1
            '
            'lblPackageName
            '
            Me.lblPackageName.AutoSize = True
            Me.lblPackageName.Location = New System.Drawing.Point(12, 13)
            Me.lblPackageName.Name = "lblPackageName"
            Me.lblPackageName.Size = New System.Drawing.Size(84, 13)
            Me.lblPackageName.TabIndex = 0
            Me.lblPackageName.Text = "Package Name:"
            '
            'rbPackage
            '
            Me.rbPackage.BackColor = System.Drawing.Color.Transparent
            Me.rbPackage.Checked = True
            Me.rbPackage.Location = New System.Drawing.Point(61, 27)
            Me.rbPackage.Name = "rbPackage"
            Me.rbPackage.Size = New System.Drawing.Size(108, 17)
            Me.rbPackage.TabIndex = 0
            Me.rbPackage.TabStop = True
            Me.rbPackage.Text = "Package"
            Me.rbPackage.UseVisualStyleBackColor = False
            '
            'rbTemplate
            '
            Me.rbTemplate.BackColor = System.Drawing.Color.Transparent
            Me.rbTemplate.Location = New System.Drawing.Point(61, 103)
            Me.rbTemplate.Name = "rbTemplate"
            Me.rbTemplate.Size = New System.Drawing.Size(108, 17)
            Me.rbTemplate.TabIndex = 2
            Me.rbTemplate.Text = "Template"
            Me.rbTemplate.UseVisualStyleBackColor = False
            '
            'pnlProject
            '
            Me.pnlProject.Controls.Add(Me.chkNewProject)
            Me.pnlProject.Controls.Add(Me.lblProjectName)
            Me.pnlProject.Controls.Add(Me.txtProjectName)
            Me.pnlProject.Controls.Add(Me.lblPackageName)
            Me.pnlProject.Controls.Add(Me.txtPackageName)
            Me.pnlProject.Location = New System.Drawing.Point(6, 3)
            Me.pnlProject.Name = "pnlProject"
            Me.pnlProject.Size = New System.Drawing.Size(342, 218)
            Me.pnlProject.TabIndex = 1
            Me.pnlProject.Visible = False
            '
            'chkNewProject
            '
            Me.chkNewProject.AutoSize = True
            Me.chkNewProject.Location = New System.Drawing.Point(15, 65)
            Me.chkNewProject.Name = "chkNewProject"
            Me.chkNewProject.Size = New System.Drawing.Size(115, 17)
            Me.chkNewProject.TabIndex = 2
            Me.chkNewProject.Text = "Create new project"
            Me.chkNewProject.UseVisualStyleBackColor = True
            '
            'picPackage
            '
            Me.picPackage.BackColor = System.Drawing.Color.Transparent
            Me.picPackage.Image = CType(resources.GetObject("picPackage.Image"), System.Drawing.Image)
            Me.picPackage.Location = New System.Drawing.Point(23, 16)
            Me.picPackage.Name = "picPackage"
            Me.picPackage.Size = New System.Drawing.Size(32, 32)
            Me.picPackage.TabIndex = 10
            Me.picPackage.TabStop = False
            '
            'picTemplate
            '
            Me.picTemplate.BackColor = System.Drawing.Color.Transparent
            Me.picTemplate.Image = CType(resources.GetObject("picTemplate.Image"), System.Drawing.Image)
            Me.picTemplate.Location = New System.Drawing.Point(23, 92)
            Me.picTemplate.Name = "picTemplate"
            Me.picTemplate.Size = New System.Drawing.Size(32, 32)
            Me.picTemplate.TabIndex = 11
            Me.picTemplate.TabStop = False
            '
            'pnlTemplate
            '
            Me.pnlTemplate.Location = New System.Drawing.Point(0, 0)
            Me.pnlTemplate.Name = "pnlTemplate"
            Me.pnlTemplate.Size = New System.Drawing.Size(200, 100)
            Me.pnlTemplate.TabIndex = 5
            '
            'Panel5
            '
            Me.Panel5.Controls.Add(Me.lblTemplateName)
            Me.Panel5.Controls.Add(Me.txtTemplate)
            Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
            Me.Panel5.Location = New System.Drawing.Point(0, 0)
            Me.Panel5.Name = "Panel5"
            Me.Panel5.Size = New System.Drawing.Size(342, 67)
            Me.Panel5.TabIndex = 3
            '
            'lblTemplateName
            '
            Me.lblTemplateName.AutoSize = True
            Me.lblTemplateName.Location = New System.Drawing.Point(12, 13)
            Me.lblTemplateName.Name = "lblTemplateName"
            Me.lblTemplateName.Size = New System.Drawing.Size(85, 13)
            Me.lblTemplateName.TabIndex = 0
            Me.lblTemplateName.Text = "Template Name:"
            '
            'txtTemplate
            '
            Me.txtTemplate.Location = New System.Drawing.Point(15, 29)
            Me.txtTemplate.Name = "txtTemplate"
            Me.txtTemplate.Size = New System.Drawing.Size(201, 20)
            Me.txtTemplate.TabIndex = 1
            '
            'pnlDBSource
            '
            Me.pnlDBSource.Controls.Add(Me.lblDBSourceName)
            Me.pnlDBSource.Controls.Add(Me.txtDBSourceName)
            Me.pnlDBSource.Location = New System.Drawing.Point(357, 3)
            Me.pnlDBSource.Name = "pnlDBSource"
            Me.pnlDBSource.Size = New System.Drawing.Size(342, 218)
            Me.pnlDBSource.TabIndex = 2
            Me.pnlDBSource.Visible = False
            '
            'lblDBSourceName
            '
            Me.lblDBSourceName.AutoSize = True
            Me.lblDBSourceName.Location = New System.Drawing.Point(12, 13)
            Me.lblDBSourceName.Name = "lblDBSourceName"
            Me.lblDBSourceName.Size = New System.Drawing.Size(75, 13)
            Me.lblDBSourceName.TabIndex = 19
            Me.lblDBSourceName.Text = "Source Name:"
            '
            'txtDBSourceName
            '
            Me.txtDBSourceName.Location = New System.Drawing.Point(15, 29)
            Me.txtDBSourceName.Name = "txtDBSourceName"
            Me.txtDBSourceName.Size = New System.Drawing.Size(201, 20)
            Me.txtDBSourceName.TabIndex = 20
            '
            'picDBSource
            '
            Me.picDBSource.BackColor = System.Drawing.Color.Transparent
            Me.picDBSource.Image = CType(resources.GetObject("picDBSource.Image"), System.Drawing.Image)
            Me.picDBSource.Location = New System.Drawing.Point(23, 54)
            Me.picDBSource.Name = "picDBSource"
            Me.picDBSource.Size = New System.Drawing.Size(32, 32)
            Me.picDBSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
            Me.picDBSource.TabIndex = 14
            Me.picDBSource.TabStop = False
            '
            'rbDBSource
            '
            Me.rbDBSource.BackColor = System.Drawing.Color.Transparent
            Me.rbDBSource.Location = New System.Drawing.Point(61, 65)
            Me.rbDBSource.Name = "rbDBSource"
            Me.rbDBSource.Size = New System.Drawing.Size(108, 17)
            Me.rbDBSource.TabIndex = 1
            Me.rbDBSource.Text = "Source"
            Me.rbDBSource.UseVisualStyleBackColor = False
            '
            'lblMsg
            '
            Me.lblMsg.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.lblMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblMsg.Location = New System.Drawing.Point(182, 324)
            Me.lblMsg.Name = "lblMsg"
            Me.lblMsg.Size = New System.Drawing.Size(702, 13)
            Me.lblMsg.TabIndex = 10
            '
            'picVB
            '
            Me.picVB.BackColor = System.Drawing.Color.Transparent
            Me.picVB.Image = CType(resources.GetObject("picVB.Image"), System.Drawing.Image)
            Me.picVB.Location = New System.Drawing.Point(23, 130)
            Me.picVB.Name = "picVB"
            Me.picVB.Size = New System.Drawing.Size(32, 32)
            Me.picVB.TabIndex = 17
            Me.picVB.TabStop = False
            '
            'rbVB
            '
            Me.rbVB.BackColor = System.Drawing.Color.Transparent
            Me.rbVB.Location = New System.Drawing.Point(61, 141)
            Me.rbVB.Name = "rbVB"
            Me.rbVB.Size = New System.Drawing.Size(108, 17)
            Me.rbVB.TabIndex = 3
            Me.rbVB.Text = "VB Code"
            Me.rbVB.UseVisualStyleBackColor = False
            '
            'picCS
            '
            Me.picCS.BackColor = System.Drawing.Color.Transparent
            Me.picCS.Image = CType(resources.GetObject("picCS.Image"), System.Drawing.Image)
            Me.picCS.Location = New System.Drawing.Point(23, 168)
            Me.picCS.Name = "picCS"
            Me.picCS.Size = New System.Drawing.Size(32, 32)
            Me.picCS.TabIndex = 19
            Me.picCS.TabStop = False
            '
            'rbCS
            '
            Me.rbCS.BackColor = System.Drawing.Color.Transparent
            Me.rbCS.Location = New System.Drawing.Point(61, 179)
            Me.rbCS.Name = "rbCS"
            Me.rbCS.Size = New System.Drawing.Size(108, 17)
            Me.rbCS.TabIndex = 4
            Me.rbCS.Text = "C Sharp Code"
            Me.rbCS.UseVisualStyleBackColor = False
            '
            'pnlCodeFile
            '
            Me.pnlCodeFile.Controls.Add(Me.lblCodeFileName)
            Me.pnlCodeFile.Controls.Add(Me.txtCodeFileName)
            Me.pnlCodeFile.Location = New System.Drawing.Point(357, 227)
            Me.pnlCodeFile.Name = "pnlCodeFile"
            Me.pnlCodeFile.Size = New System.Drawing.Size(342, 218)
            Me.pnlCodeFile.TabIndex = 4
            Me.pnlCodeFile.Visible = False
            '
            'lblCodeFileName
            '
            Me.lblCodeFileName.AutoSize = True
            Me.lblCodeFileName.Location = New System.Drawing.Point(12, 13)
            Me.lblCodeFileName.Name = "lblCodeFileName"
            Me.lblCodeFileName.Size = New System.Drawing.Size(85, 13)
            Me.lblCodeFileName.TabIndex = 0
            Me.lblCodeFileName.Text = "Code File Name:"
            '
            'txtCodeFileName
            '
            Me.txtCodeFileName.Location = New System.Drawing.Point(15, 29)
            Me.txtCodeFileName.Name = "txtCodeFileName"
            Me.txtCodeFileName.Size = New System.Drawing.Size(201, 20)
            Me.txtCodeFileName.TabIndex = 1
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.lblDescription)
            Me.Panel1.Controls.Add(Me.Panel3)
            Me.Panel1.Controls.Add(Me.Panel4)
            Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.Panel1.Location = New System.Drawing.Point(0, 337)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(884, 225)
            Me.Panel1.TabIndex = 5
            '
            'lblDescription
            '
            Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDescription.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            Me.lblDescription.Location = New System.Drawing.Point(203, 23)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(452, 158)
            Me.lblDescription.TabIndex = 2
            '
            'Panel3
            '
            Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Panel3.BackColor = System.Drawing.Color.Transparent
            Me.Panel3.BackgroundImage = CType(resources.GetObject("Panel3.BackgroundImage"), System.Drawing.Image)
            Me.Panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.Panel3.ForeColor = System.Drawing.Color.Transparent
            Me.Panel3.Location = New System.Drawing.Point(6, 11)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(978, 1)
            Me.Panel3.TabIndex = 0
            '
            'Panel4
            '
            Me.Panel4.BackgroundImage = Global.Metadrone.My.Resources.Resources.DialogLeft
            Me.Panel4.Dock = System.Windows.Forms.DockStyle.Left
            Me.Panel4.Location = New System.Drawing.Point(0, 0)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(182, 225)
            Me.Panel4.TabIndex = 0
            '
            'Panel2
            '
            Me.Panel2.BackgroundImage = Global.Metadrone.My.Resources.Resources.DialogLeft
            Me.Panel2.Controls.Add(Me.picPackage)
            Me.Panel2.Controls.Add(Me.rbPackage)
            Me.Panel2.Controls.Add(Me.rbTemplate)
            Me.Panel2.Controls.Add(Me.picCS)
            Me.Panel2.Controls.Add(Me.picTemplate)
            Me.Panel2.Controls.Add(Me.rbCS)
            Me.Panel2.Controls.Add(Me.rbDBSource)
            Me.Panel2.Controls.Add(Me.picVB)
            Me.Panel2.Controls.Add(Me.picDBSource)
            Me.Panel2.Controls.Add(Me.rbVB)
            Me.Panel2.Controls.Add(Me.picProject)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
            Me.Panel2.Location = New System.Drawing.Point(0, 0)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(182, 337)
            Me.Panel2.TabIndex = 0
            '
            'picProject
            '
            Me.picProject.BackColor = System.Drawing.Color.Transparent
            Me.picProject.Image = CType(resources.GetObject("picProject.Image"), System.Drawing.Image)
            Me.picProject.Location = New System.Drawing.Point(23, 16)
            Me.picProject.Name = "picProject"
            Me.picProject.Size = New System.Drawing.Size(32, 32)
            Me.picProject.TabIndex = 20
            Me.picProject.TabStop = False
            '
            'ttFast
            '
            Me.ttFast.AutoPopDelay = 5000
            Me.ttFast.InitialDelay = 100
            Me.ttFast.IsBalloon = True
            Me.ttFast.ReshowDelay = 100
            '
            'pnlContainer
            '
            Me.pnlContainer.Controls.Add(Me.pnlCodeFile)
            Me.pnlContainer.Controls.Add(Me.pnlDBSource)
            Me.pnlContainer.Controls.Add(Me.pnlProject)
            Me.pnlContainer.Controls.Add(Me.pnlTemplate)
            Me.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlContainer.Location = New System.Drawing.Point(182, 0)
            Me.pnlContainer.Name = "pnlContainer"
            Me.pnlContainer.Padding = New System.Windows.Forms.Padding(0, 0, 12, 0)
            Me.pnlContainer.Size = New System.Drawing.Size(702, 324)
            Me.pnlContainer.TabIndex = 11
            '
            'NewItem
            '
            Me.AcceptButton = Me.OK_Button
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.CancelButton = Me.Cancel_Button
            Me.ClientSize = New System.Drawing.Size(884, 562)
            Me.Controls.Add(Me.pnlContainer)
            Me.Controls.Add(Me.lblMsg)
            Me.Controls.Add(Me.Panel2)
            Me.Controls.Add(Me.Panel1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(703, 469)
            Me.Name = "NewItem"
            Me.Opacity = 0.94
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "New Item"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.pnlProject.ResumeLayout(False)
            Me.pnlProject.PerformLayout()
            CType(Me.picPackage, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            Me.Panel5.ResumeLayout(False)
            Me.Panel5.PerformLayout()
            Me.pnlDBSource.ResumeLayout(False)
            Me.pnlDBSource.PerformLayout()
            CType(Me.picDBSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picVB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picCS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlCodeFile.ResumeLayout(False)
            Me.pnlCodeFile.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel2.ResumeLayout(False)
            CType(Me.picProject, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlContainer.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents OK_Button As System.Windows.Forms.Button
        Friend WithEvents Cancel_Button As System.Windows.Forms.Button
        Friend WithEvents lblProjectName As System.Windows.Forms.Label
        Friend WithEvents txtProjectName As System.Windows.Forms.TextBox
        Friend WithEvents txtPackageName As System.Windows.Forms.TextBox
        Friend WithEvents lblPackageName As System.Windows.Forms.Label
        Friend WithEvents rbPackage As System.Windows.Forms.RadioButton
        Friend WithEvents rbTemplate As System.Windows.Forms.RadioButton
        Friend WithEvents picPackage As System.Windows.Forms.PictureBox
        Friend WithEvents picTemplate As System.Windows.Forms.PictureBox
        Friend WithEvents pnlProject As System.Windows.Forms.Panel
        Friend WithEvents pnlTemplate As System.Windows.Forms.Panel
        Friend WithEvents lblTemplateName As System.Windows.Forms.Label
        Friend WithEvents txtTemplate As System.Windows.Forms.TextBox
        Friend WithEvents chkNewProject As System.Windows.Forms.CheckBox
        Friend WithEvents pnlDBSource As System.Windows.Forms.Panel
        Friend WithEvents picDBSource As System.Windows.Forms.PictureBox
        Friend WithEvents rbDBSource As System.Windows.Forms.RadioButton
        Friend WithEvents lblDBSourceName As System.Windows.Forms.Label
        Friend WithEvents txtDBSourceName As System.Windows.Forms.TextBox
        Friend WithEvents lblMsg As System.Windows.Forms.Label
        Friend WithEvents picVB As System.Windows.Forms.PictureBox
        Friend WithEvents rbVB As System.Windows.Forms.RadioButton
        Friend WithEvents picCS As System.Windows.Forms.PictureBox
        Friend WithEvents rbCS As System.Windows.Forms.RadioButton
        Friend WithEvents pnlCodeFile As System.Windows.Forms.Panel
        Friend WithEvents lblCodeFileName As System.Windows.Forms.Label
        Friend WithEvents txtCodeFileName As System.Windows.Forms.TextBox
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents ttFast As System.Windows.Forms.ToolTip
        Friend WithEvents picProject As System.Windows.Forms.PictureBox
        Friend WithEvents Panel5 As System.Windows.Forms.Panel
        Friend WithEvents pnlContainer As System.Windows.Forms.Panel

    End Class

End Namespace