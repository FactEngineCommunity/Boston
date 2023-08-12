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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFEKLUploader))
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPageFEKLJSON = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonStopContinueProcessing = New System.Windows.Forms.Button()
        Me.LabelFEKLJSONErrorString = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LabelFEKLJSONErrorType = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelFEKLJSON = New System.Windows.Forms.Label()
        Me.LabelPromptFEKLJSONFileName = New System.Windows.Forms.Label()
        Me.DataGridViewFEKLStatements = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStripGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideProcessedFEKLStatementsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportErrorerFEKLStatementsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonOpenFEKLJSONFile = New System.Windows.Forms.Button()
        Me.TabPageFEKL = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ButtonFEKLStartStop = New System.Windows.Forms.Button()
        Me.RichTextBoxFEKLDocument = New System.Windows.Forms.RichTextBox()
        Me.LabelErrorMessage = New System.Windows.Forms.Label()
        Me.ButtonOpenFEKLFile = New System.Windows.Forms.Button()
        Me.LabelPromptErrorMessage = New System.Windows.Forms.Label()
        Me.LabelErrorType = New System.Windows.Forms.Label()
        Me.LabelPromptErrorType = New System.Windows.Forms.Label()
        Me.TabPageDDL2FEKL = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButtonDDLExtractFEKL = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripLabelPromptDatabaseType = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripComboBoxDatabaseType = New System.Windows.Forms.ToolStripComboBox()
        Me.TextBoxDDL = New System.Windows.Forms.TextBox()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.LabelPromptModelName = New System.Windows.Forms.Label()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.GroupBox.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPageFEKLJSON.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridViewFEKLStatements, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStripGrid.SuspendLayout()
        Me.TabPageFEKL.SuspendLayout()
        Me.Panel2.SuspendLayout()
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
        Me.GroupBox.Controls.Add(Me.LabelModelName)
        Me.GroupBox.Controls.Add(Me.LabelPromptModelName)
        Me.GroupBox.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(653, 440)
        Me.GroupBox.TabIndex = 0
        Me.GroupBox.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPageFEKLJSON)
        Me.TabControl1.Controls.Add(Me.TabPageFEKL)
        Me.TabControl1.Controls.Add(Me.TabPageDDL2FEKL)
        Me.TabControl1.Location = New System.Drawing.Point(6, 49)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(638, 381)
        Me.TabControl1.TabIndex = 10
        '
        'TabPageFEKLJSON
        '
        Me.TabPageFEKLJSON.Controls.Add(Me.Panel1)
        Me.TabPageFEKLJSON.Controls.Add(Me.LabelFEKLJSONErrorString)
        Me.TabPageFEKLJSON.Controls.Add(Me.Label2)
        Me.TabPageFEKLJSON.Controls.Add(Me.LabelFEKLJSONErrorType)
        Me.TabPageFEKLJSON.Controls.Add(Me.Label4)
        Me.TabPageFEKLJSON.Controls.Add(Me.LabelFEKLJSON)
        Me.TabPageFEKLJSON.Controls.Add(Me.LabelPromptFEKLJSONFileName)
        Me.TabPageFEKLJSON.Controls.Add(Me.DataGridViewFEKLStatements)
        Me.TabPageFEKLJSON.Controls.Add(Me.ButtonOpenFEKLJSONFile)
        Me.TabPageFEKLJSON.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFEKLJSON.Name = "TabPageFEKLJSON"
        Me.TabPageFEKLJSON.Size = New System.Drawing.Size(630, 355)
        Me.TabPageFEKLJSON.TabIndex = 2
        Me.TabPageFEKLJSON.Text = "FEKL JSON"
        Me.TabPageFEKLJSON.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.ButtonStopContinueProcessing)
        Me.Panel1.Location = New System.Drawing.Point(201, 15)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(301, 25)
        Me.Panel1.TabIndex = 15
        '
        'ButtonStopContinueProcessing
        '
        Me.ButtonStopContinueProcessing.AutoSize = True
        Me.ButtonStopContinueProcessing.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.ButtonStopContinueProcessing.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonStopContinueProcessing.ForeColor = System.Drawing.Color.White
        Me.ButtonStopContinueProcessing.Location = New System.Drawing.Point(0, 0)
        Me.ButtonStopContinueProcessing.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonStopContinueProcessing.Name = "ButtonStopContinueProcessing"
        Me.ButtonStopContinueProcessing.Size = New System.Drawing.Size(102, 25)
        Me.ButtonStopContinueProcessing.TabIndex = 14
        Me.ButtonStopContinueProcessing.Tag = "NotYetStarted"
        Me.ButtonStopContinueProcessing.Text = "&Start Processing"
        Me.ButtonStopContinueProcessing.UseVisualStyleBackColor = False
        Me.ButtonStopContinueProcessing.Visible = False
        '
        'LabelFEKLJSONErrorString
        '
        Me.LabelFEKLJSONErrorString.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelFEKLJSONErrorString.Location = New System.Drawing.Point(102, 313)
        Me.LabelFEKLJSONErrorString.Name = "LabelFEKLJSONErrorString"
        Me.LabelFEKLJSONErrorString.Size = New System.Drawing.Size(393, 36)
        Me.LabelFEKLJSONErrorString.TabIndex = 13
        Me.LabelFEKLJSONErrorString.Text = "N/A"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 313)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Error Message:"
        '
        'LabelFEKLJSONErrorType
        '
        Me.LabelFEKLJSONErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelFEKLJSONErrorType.AutoSize = True
        Me.LabelFEKLJSONErrorType.Location = New System.Drawing.Point(82, 290)
        Me.LabelFEKLJSONErrorType.Name = "LabelFEKLJSONErrorType"
        Me.LabelFEKLJSONErrorType.Size = New System.Drawing.Size(27, 13)
        Me.LabelFEKLJSONErrorType.TabIndex = 11
        Me.LabelFEKLJSONErrorType.Text = "N/A"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 290)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Error Type:"
        '
        'LabelFEKLJSON
        '
        Me.LabelFEKLJSON.AutoSize = True
        Me.LabelFEKLJSON.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelFEKLJSON.ForeColor = System.Drawing.Color.SkyBlue
        Me.LabelFEKLJSON.Location = New System.Drawing.Point(90, 49)
        Me.LabelFEKLJSON.Name = "LabelFEKLJSON"
        Me.LabelFEKLJSON.Size = New System.Drawing.Size(217, 16)
        Me.LabelFEKLJSON.TabIndex = 6
        Me.LabelFEKLJSON.Text = "<Filename - No File Selected>"
        '
        'LabelPromptFEKLJSONFileName
        '
        Me.LabelPromptFEKLJSONFileName.AutoSize = True
        Me.LabelPromptFEKLJSONFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPromptFEKLJSONFileName.Location = New System.Drawing.Point(17, 49)
        Me.LabelPromptFEKLJSONFileName.Name = "LabelPromptFEKLJSONFileName"
        Me.LabelPromptFEKLJSONFileName.Size = New System.Drawing.Size(67, 16)
        Me.LabelPromptFEKLJSONFileName.TabIndex = 5
        Me.LabelPromptFEKLJSONFileName.Text = "Filename:"
        '
        'DataGridViewFEKLStatements
        '
        Me.DataGridViewFEKLStatements.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewFEKLStatements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewFEKLStatements.ContextMenuStrip = Me.ContextMenuStripGrid
        Me.DataGridViewFEKLStatements.Location = New System.Drawing.Point(17, 82)
        Me.DataGridViewFEKLStatements.Name = "DataGridViewFEKLStatements"
        Me.DataGridViewFEKLStatements.Size = New System.Drawing.Size(595, 194)
        Me.DataGridViewFEKLStatements.TabIndex = 4
        '
        'ContextMenuStripGrid
        '
        Me.ContextMenuStripGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideProcessedFEKLStatementsToolStripMenuItem, Me.ExportErrorerFEKLStatementsToolStripMenuItem})
        Me.ContextMenuStripGrid.Name = "ContextMenuStripGrid"
        Me.ContextMenuStripGrid.Size = New System.Drawing.Size(246, 48)
        '
        'HideProcessedFEKLStatementsToolStripMenuItem
        '
        Me.HideProcessedFEKLStatementsToolStripMenuItem.Name = "HideProcessedFEKLStatementsToolStripMenuItem"
        Me.HideProcessedFEKLStatementsToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.HideProcessedFEKLStatementsToolStripMenuItem.Text = "&Hide Processed FEKL Statements"
        '
        'ExportErrorerFEKLStatementsToolStripMenuItem
        '
        Me.ExportErrorerFEKLStatementsToolStripMenuItem.Name = "ExportErrorerFEKLStatementsToolStripMenuItem"
        Me.ExportErrorerFEKLStatementsToolStripMenuItem.Size = New System.Drawing.Size(245, 22)
        Me.ExportErrorerFEKLStatementsToolStripMenuItem.Text = "&Export Errorer FEKL Statements"
        '
        'ButtonOpenFEKLJSONFile
        '
        Me.ButtonOpenFEKLJSONFile.Location = New System.Drawing.Point(17, 15)
        Me.ButtonOpenFEKLJSONFile.Name = "ButtonOpenFEKLJSONFile"
        Me.ButtonOpenFEKLJSONFile.Size = New System.Drawing.Size(123, 23)
        Me.ButtonOpenFEKLJSONFile.TabIndex = 2
        Me.ButtonOpenFEKLJSONFile.Text = "&Open FEKL JSON File"
        Me.ButtonOpenFEKLJSONFile.UseVisualStyleBackColor = True
        '
        'TabPageFEKL
        '
        Me.TabPageFEKL.Controls.Add(Me.Panel2)
        Me.TabPageFEKL.Controls.Add(Me.RichTextBoxFEKLDocument)
        Me.TabPageFEKL.Controls.Add(Me.LabelErrorMessage)
        Me.TabPageFEKL.Controls.Add(Me.ButtonOpenFEKLFile)
        Me.TabPageFEKL.Controls.Add(Me.LabelPromptErrorMessage)
        Me.TabPageFEKL.Controls.Add(Me.LabelErrorType)
        Me.TabPageFEKL.Controls.Add(Me.LabelPromptErrorType)
        Me.TabPageFEKL.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFEKL.Name = "TabPageFEKL"
        Me.TabPageFEKL.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageFEKL.Size = New System.Drawing.Size(630, 355)
        Me.TabPageFEKL.TabIndex = 0
        Me.TabPageFEKL.Text = "FEKL"
        Me.TabPageFEKL.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.ButtonFEKLStartStop)
        Me.Panel2.Location = New System.Drawing.Point(253, 15)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(301, 25)
        Me.Panel2.TabIndex = 16
        '
        'ButtonFEKLStartStop
        '
        Me.ButtonFEKLStartStop.AutoSize = True
        Me.ButtonFEKLStartStop.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.ButtonFEKLStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonFEKLStartStop.ForeColor = System.Drawing.Color.White
        Me.ButtonFEKLStartStop.Location = New System.Drawing.Point(0, 0)
        Me.ButtonFEKLStartStop.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonFEKLStartStop.Name = "ButtonFEKLStartStop"
        Me.ButtonFEKLStartStop.Size = New System.Drawing.Size(102, 25)
        Me.ButtonFEKLStartStop.TabIndex = 14
        Me.ButtonFEKLStartStop.Tag = "NotYetStarted"
        Me.ButtonFEKLStartStop.Text = "&Start Processing"
        Me.ButtonFEKLStartStop.UseVisualStyleBackColor = False
        '
        'RichTextBoxFEKLDocument
        '
        Me.RichTextBoxFEKLDocument.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxFEKLDocument.Location = New System.Drawing.Point(3, 60)
        Me.RichTextBoxFEKLDocument.Name = "RichTextBoxFEKLDocument"
        Me.RichTextBoxFEKLDocument.Size = New System.Drawing.Size(621, 220)
        Me.RichTextBoxFEKLDocument.TabIndex = 5
        Me.RichTextBoxFEKLDocument.Text = ""
        '
        'LabelErrorMessage
        '
        Me.LabelErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorMessage.Location = New System.Drawing.Point(86, 301)
        Me.LabelErrorMessage.Name = "LabelErrorMessage"
        Me.LabelErrorMessage.Size = New System.Drawing.Size(393, 36)
        Me.LabelErrorMessage.TabIndex = 9
        Me.LabelErrorMessage.Text = "N/A"
        '
        'ButtonOpenFEKLFile
        '
        Me.ButtonOpenFEKLFile.Location = New System.Drawing.Point(17, 15)
        Me.ButtonOpenFEKLFile.Name = "ButtonOpenFEKLFile"
        Me.ButtonOpenFEKLFile.Size = New System.Drawing.Size(95, 23)
        Me.ButtonOpenFEKLFile.TabIndex = 0
        Me.ButtonOpenFEKLFile.Text = "&Open FEKL File"
        Me.ButtonOpenFEKLFile.UseVisualStyleBackColor = True
        '
        'LabelPromptErrorMessage
        '
        Me.LabelPromptErrorMessage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorMessage.AutoSize = True
        Me.LabelPromptErrorMessage.Location = New System.Drawing.Point(2, 301)
        Me.LabelPromptErrorMessage.Name = "LabelPromptErrorMessage"
        Me.LabelPromptErrorMessage.Size = New System.Drawing.Size(78, 13)
        Me.LabelPromptErrorMessage.TabIndex = 8
        Me.LabelPromptErrorMessage.Text = "Error Message:"
        '
        'LabelErrorType
        '
        Me.LabelErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelErrorType.AutoSize = True
        Me.LabelErrorType.Location = New System.Drawing.Point(70, 283)
        Me.LabelErrorType.Name = "LabelErrorType"
        Me.LabelErrorType.Size = New System.Drawing.Size(27, 13)
        Me.LabelErrorType.TabIndex = 7
        Me.LabelErrorType.Text = "N/A"
        '
        'LabelPromptErrorType
        '
        Me.LabelPromptErrorType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelPromptErrorType.AutoSize = True
        Me.LabelPromptErrorType.Location = New System.Drawing.Point(5, 283)
        Me.LabelPromptErrorType.Name = "LabelPromptErrorType"
        Me.LabelPromptErrorType.Size = New System.Drawing.Size(59, 13)
        Me.LabelPromptErrorType.TabIndex = 6
        Me.LabelPromptErrorType.Text = "Error Type:"
        '
        'TabPageDDL2FEKL
        '
        Me.TabPageDDL2FEKL.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPageDDL2FEKL.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDDL2FEKL.Name = "TabPageDDL2FEKL"
        Me.TabPageDDL2FEKL.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageDDL2FEKL.Size = New System.Drawing.Size(630, 355)
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
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(624, 349)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButtonDDLExtractFEKL, Me.ToolStripLabelPromptDatabaseType, Me.ToolStripComboBoxDatabaseType})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(624, 25)
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
        Me.TextBoxDDL.Location = New System.Drawing.Point(3, 34)
        Me.TextBoxDDL.Multiline = True
        Me.TextBoxDDL.Name = "TextBoxDDL"
        Me.TextBoxDDL.Size = New System.Drawing.Size(618, 312)
        Me.TextBoxDDL.TabIndex = 0
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
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.Location = New System.Drawing.Point(675, 22)
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
        Me.ClientSize = New System.Drawing.Size(760, 454)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.GroupBox)
        Me.Name = "frmFEKLUploader"
        Me.Text = "FEKL Uploader"
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBox.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageFEKLJSON.ResumeLayout(False)
        Me.TabPageFEKLJSON.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.DataGridViewFEKLStatements, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStripGrid.ResumeLayout(False)
        Me.TabPageFEKL.ResumeLayout(False)
        Me.TabPageFEKL.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
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
    Friend WithEvents TabPageFEKLJSON As TabPage
    Friend WithEvents ButtonOpenFEKLJSONFile As Button
    Friend WithEvents DataGridViewFEKLStatements As DataGridView
    Friend WithEvents LabelFEKLJSON As Label
    Friend WithEvents LabelPromptFEKLJSONFileName As Label
    Friend WithEvents LabelFEKLJSONErrorString As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents LabelFEKLJSONErrorType As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ContextMenuStripGrid As ContextMenuStrip
    Friend WithEvents ExportErrorerFEKLStatementsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HideProcessedFEKLStatementsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ButtonStopContinueProcessing As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ButtonFEKLStartStop As Button
End Class
