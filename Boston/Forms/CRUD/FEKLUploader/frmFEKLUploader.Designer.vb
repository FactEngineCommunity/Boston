<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFEKLUploader
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFEKLUploader))
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageFEKL = New System.Windows.Forms.TabPage()
        Me.RichTextBoxFEKLDocument = New System.Windows.Forms.RichTextBox()
        Me.TabPageDDL2FEKL = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonDDLExtractFEKL = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabelPromptDatabaseType = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBoxDatabaseType = New System.Windows.Forms.ToolStripComboBox()
        Me.TextBoxDDL = New System.Windows.Forms.TextBox()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.LabelPromptErrorMessage = New System.Windows.Forms.Label()
        Me.LabelErrorType = New System.Windows.Forms.Label()
        Me.LabelPromptErrorType = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModelName = New System.Windows.Forms.Label()
        Me.ButtonLoadIntoModel = New System.Windows.Forms.Button()
        Me.ButtonOpenFEKLFile = New System.Windows.Forms.Button()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.GroupBox.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageFEKL.SuspendLayout()
        Me.TabPageDDL2FEKL.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox
        '
        Me.GroupBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox.Controls.Add(Me.TabControl1)
        Me.GroupBox.Controls.Add(Me.LabelErrorMessage)
        Me.GroupBox.Controls.Add(Me.LabelPromptErrorMessage)
        Me.GroupBox.Controls.Add(Me.LabelErrorType)
        Me.GroupBox.Controls.Add(Me.LabelPromptErrorType)
        Me.GroupBox.Controls.Add(Me.LabelModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelName)
        Me.GroupBox.Controls.Add(Me.ButtonLoadIntoModel)
        Me.GroupBox.Controls.Add(Me.ButtonOpenFEKLFile)
        Me.GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(713, 511)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageFEKL)
        Me.TabControl1.Controls.Add(Me.TabPageDDL2FEKL)
        Me.TabControl1.Location = New System.Drawing.Point(6, 80)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(698, 366)
        Me.TabControl1.TabIndex = 10
        '
        'TabPageFEKL
        '
        Me.TabPageFEKL.Controls.Add(Me.RichTextBoxFEKLDocument)
        Me.TabPageFEKL.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFEKL.Name = "TabPageFEKL"
        Me.TabPageFEKL.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageFEKL.Size = New System.Drawing.Size(690, 340)
        Me.TabPageFEKL.TabIndex = 0
        Me.TabPageFEKL.Text = "FEKL"
        Me.TabPageFEKL.UseVisualStyleBackColor = True
        '
        'RichTextBoxFEKLDocument
        '
        Me.RichTextBoxFEKLDocument.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxFEKLDocument.Location = New System.Drawing.Point(3, 3)
        Me.RichTextBoxFEKLDocument.Name = "RichTextBoxFEKLDocument"
        Me.RichTextBoxFEKLDocument.Size = New System.Drawing.Size(684, 334)
        Me.RichTextBoxFEKLDocument.TabIndex = 5
        Me.RichTextBoxFEKLDocument.Text = ""
        '
        'TabPageDDL2FEKL
        '
        Me.TabPageDDL2FEKL.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPageDDL2FEKL.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDDL2FEKL.Name = "TabPageDDL2FEKL"
        Me.TabPageDDL2FEKL.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageDDL2FEKL.Size = New System.Drawing.Size(471, 325)
        Me.TabPageDDL2FEKL.TabIndex = 1
        Me.TabPageDDL2FEKL.Text = "DDL-2-FEKL"
        Me.TabPageDDL2FEKL.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxDDL, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.90909!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(465, 319)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonDDLExtractFEKL, Me.ToolStripLabelPromptDatabaseType, Me.ToolStripComboBoxDatabaseType})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(465, 25)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButtonDDLExtractFEKL
        '
        Me.ToolStripButtonDDLExtractFEKL.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripButtonDDLExtractFEKL.Image = CType(resources.GetObject("ToolStripButtonDDLExtractFEKL.Image"), System.Drawing.Image)
        Me.ToolStripButtonDDLExtractFEKL.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonDDLExtractFEKL.Name = "ToolStripButtonDDLExtractFEKL"
        Me.ToolStripButtonDDLExtractFEKL.Size = New System.Drawing.Size(75, 22)
        Me.ToolStripButtonDDLExtractFEKL.Text = "&Extract FEKL"
        '
        'ToolStripLabelPromptDatabaseType
        '
        Me.ToolStripLabelPromptDatabaseType.Name = "ToolStripLabelPromptDatabaseType"
        Me.ToolStripLabelPromptDatabaseType.Size = New System.Drawing.Size(85, 22)
        Me.ToolStripLabelPromptDatabaseType.Text = "Database Type:"
        '
        'ToolStripComboBoxDatabaseType
        '
        Me.ToolStripComboBoxDatabaseType.Name = "ToolStripComboBoxDatabaseType"
        Me.ToolStripComboBoxDatabaseType.Size = New System.Drawing.Size(121, 25)
        '
        'TextBoxDDL
        '
        Me.TextBoxDDL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxDDL.Location = New System.Drawing.Point(3, 31)
        Me.TextBoxDDL.Multiline = True
        Me.TextBoxDDL.Name = "TextBoxDDL"
        Me.TextBoxDDL.Size = New System.Drawing.Size(459, 285)
        Me.TextBoxDDL.TabIndex = 0
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorMessage.Location = New System.Drawing.Point(92, 472)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(393, 36)
        Me.LabelErrorMessage.TabIndex = 9
        Me.LabelErrorMessage.Text = "N/A"
        '
        'LabelPromptErrorMessage
        '
        Me.LabelPromptErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorMessage.AutoSize = True
        Me.LabelPromptErrorMessage.Location = New System.Drawing.Point(7, 472)
        Me.LabelPromptErrorMessage.Name = "LabelPromptErrorMessage"
        Me.LabelPromptErrorMessage.Size = New System.Drawing.Size(78, 13)
        Me.LabelPromptErrorMessage.TabIndex = 8
        Me.LabelPromptErrorMessage.Text = "Error Message:"
        '
        'LabelErrorType
        '
        Me.LabelErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorType.AutoSize = True
        Me.LabelErrorType.Location = New System.Drawing.Point(72, 449)
        Me.LabelErrorType.Name = "LabelErrorType"
        Me.LabelErrorType.Size = New System.Drawing.Size(27, 13)
        Me.LabelErrorType.TabIndex = 7
        Me.LabelErrorType.Text = "N/A"
        '
        'LabelPromptErrorType
        '
        Me.LabelPromptErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorType.AutoSize = True
        Me.LabelPromptErrorType.Location = New System.Drawing.Point(7, 449)
        Me.LabelPromptErrorType.Name = "LabelPromptErrorType"
        Me.LabelPromptErrorType.Size = New System.Drawing.Size(59, 13)
        Me.LabelPromptErrorType.TabIndex = 6
        Me.LabelPromptErrorType.Text = "Error Type:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(55, 20)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(67, 13)
        Me.LabelModelName.TabIndex = 4
        Me.LabelModelName.Text = "Model Name"
        '
        'LabelPromptModelName
        '
        Me.LabelPromptModelName.AutoSize = True
        Me.LabelPromptModelName.Location = New System.Drawing.Point(9, 20)
        Me.LabelPromptModelName.Name = "LabelPromptModelName"
        Me.LabelPromptModelName.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModelName.TabIndex = 3
        Me.LabelPromptModelName.Text = "Model:"
        '
        'ButtonLoadIntoModel
        '
        Me.ButtonLoadIntoModel.Location = New System.Drawing.Point(107, 51)
        Me.ButtonLoadIntoModel.Name = "ButtonLoadIntoModel"
        Me.ButtonLoadIntoModel.Size = New System.Drawing.Size(99, 23)
        Me.ButtonLoadIntoModel.TabIndex = 1
        Me.ButtonLoadIntoModel.Text = "&Load into Model"
        Me.ButtonLoadIntoModel.UseVisualStyleBackColor = True
        '
        'ButtonOpenFEKLFile
        '
        Me.ButtonOpenFEKLFile.Location = New System.Drawing.Point(6, 51)
        Me.ButtonOpenFEKLFile.Name = "ButtonOpenFEKLFile"
        Me.ButtonOpenFEKLFile.Size = New System.Drawing.Size(95, 23)
        Me.ButtonOpenFEKLFile.TabIndex = 0
        Me.ButtonOpenFEKLFile.Text = "&Open FEKL File"
        Me.ButtonOpenFEKLFile.UseVisualStyleBackColor = True
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.Location = New System.Drawing.Point(735, 22)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(73, 23)
        Me.ButtonClose.TabIndex = 1
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmFEKLUploader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 525)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmFEKLUploader"
        Me.Text = "FEKL Uploader"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageFEKL.ResumeLayout(False)
        Me.TabPageDDL2FEKL.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents LabelModelName As Label
    Friend WithEvents LabelPromptModelName As Label
    Friend WithEvents ButtonLoadIntoModel As Button
    Friend WithEvents ButtonOpenFEKLFile As Button
    Friend WithEvents ButtonClose As Button
    Friend WithEvents RichTextBoxFEKLDocument As RichTextBox
    Friend WithEvents LabelErrorMessage As Label
    Friend WithEvents LabelPromptErrorMessage As Label
    Friend WithEvents LabelErrorType As Label
    Friend WithEvents LabelPromptErrorType As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPageFEKL As TabPage
    Friend WithEvents TabPageDDL2FEKL As TabPage
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButtonDDLExtractFEKL As ToolStripButton
    Friend WithEvents ToolStripLabelPromptDatabaseType As ToolStripLabel
    Friend WithEvents ToolStripComboBoxDatabaseType As ToolStripComboBox
    Friend WithEvents TextBoxDDL As TextBox
End Class
