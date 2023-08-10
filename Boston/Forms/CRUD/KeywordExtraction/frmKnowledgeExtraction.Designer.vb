<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKnowledgeExtraction
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
        Me.HelpButton = New System.Windows.Forms.Button()
        Me.KeywordExtractionNormalButton = New System.Windows.Forms.Button()
        Me.progressBar1 = New System.Windows.Forms.ProgressBar()
        Me.KeywordExtractionMaxButton = New System.Windows.Forms.Button()
        Me.RemoveStopButton = New System.Windows.Forms.Button()
        Me.StandardizationButton = New System.Windows.Forms.Button()
        Me.ResultListView = New System.Windows.Forms.ListView()
        Me.columnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.columnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SelectFileButton = New System.Windows.Forms.Button()
        Me.RichTextBoxText = New System.Windows.Forms.RichTextBox()
        Me.PathTextBox = New System.Windows.Forms.TextBox()
        Me.PathLabel = New System.Windows.Forms.Label()
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.ContextMenuStripKeyword = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddToModelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAddAsEntityType = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemAddAsValueType = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemViewInORMVerbaliser = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStripTextbox = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelPromptModel = New System.Windows.Forms.Label()
        Me.LabelModelName = New System.Windows.Forms.Label()
        Me.ContextMenuStripTextboxSelection = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSelectionAddEntity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSelectionAddValueType = New System.Windows.Forms.ToolStripMenuItem()
        Me.AsGeneralConceptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlaceInTheVirtualAnalystToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ButtonExtractFactTypeReadings = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabDocumentText = New System.Windows.Forms.TabPage()
        Me.TabPageResults = New System.Windows.Forms.TabPage()
        Me.ButtonRefereshResults = New System.Windows.Forms.Button()
        Me.ButtonAbort = New System.Windows.Forms.Button()
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.RichTextBoxResults = New System.Windows.Forms.RichTextBox()
        Me.ContextMenuStripResultsSelection = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPageAIExtraction = New System.Windows.Forms.TabPage()
        Me.GroupBoxAILLMPrompt = New System.Windows.Forms.GroupBox()
        Me.LabelPromptPrompt = New System.Windows.Forms.Label()
        Me.ComboBoxFEKLGPTPromptType = New System.Windows.Forms.ComboBox()
        Me.TextBoxAILLMPrompt = New System.Windows.Forms.TextBox()
        Me.ButtonExecuteLLMGenerativeAI = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelChunkCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.ContextMenuStripKeyword.SuspendLayout()
        Me.ContextMenuStripTextbox.SuspendLayout()
        Me.ContextMenuStripTextboxSelection.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabDocumentText.SuspendLayout()
        Me.TabPageResults.SuspendLayout()
        Me.ContextMenuStripResultsSelection.SuspendLayout()
        Me.TabPageAIExtraction.SuspendLayout()
        Me.GroupBoxAILLMPrompt.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpButton
        '
        Me.HelpButton.Location = New System.Drawing.Point(742, 36)
        Me.HelpButton.Name = "HelpButton"
        Me.HelpButton.Size = New System.Drawing.Size(75, 25)
        Me.HelpButton.TabIndex = 50
        Me.HelpButton.Text = "Help"
        Me.HelpButton.UseVisualStyleBackColor = True
        '
        'KeywordExtractionNormalButton
        '
        Me.KeywordExtractionNormalButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeywordExtractionNormalButton.Location = New System.Drawing.Point(889, 199)
        Me.KeywordExtractionNormalButton.Name = "KeywordExtractionNormalButton"
        Me.KeywordExtractionNormalButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionNormalButton.TabIndex = 49
        Me.KeywordExtractionNormalButton.Text = "Keyword Extraction (Entropy)"
        Me.KeywordExtractionNormalButton.UseVisualStyleBackColor = True
        '
        'progressBar1
        '
        Me.progressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.progressBar1.Location = New System.Drawing.Point(12, 385)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(809, 25)
        Me.progressBar1.TabIndex = 47
        '
        'KeywordExtractionMaxButton
        '
        Me.KeywordExtractionMaxButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeywordExtractionMaxButton.Location = New System.Drawing.Point(889, 258)
        Me.KeywordExtractionMaxButton.Name = "KeywordExtractionMaxButton"
        Me.KeywordExtractionMaxButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionMaxButton.TabIndex = 45
        Me.KeywordExtractionMaxButton.Text = "Keyword Extraction (Maximum Entropy)"
        Me.KeywordExtractionMaxButton.UseVisualStyleBackColor = True
        '
        'RemoveStopButton
        '
        Me.RemoveStopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveStopButton.Location = New System.Drawing.Point(889, 140)
        Me.RemoveStopButton.Name = "RemoveStopButton"
        Me.RemoveStopButton.Size = New System.Drawing.Size(115, 53)
        Me.RemoveStopButton.TabIndex = 44
        Me.RemoveStopButton.Text = "Remove Stop"
        Me.RemoveStopButton.UseVisualStyleBackColor = True
        '
        'StandardizationButton
        '
        Me.StandardizationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StandardizationButton.Location = New System.Drawing.Point(889, 81)
        Me.StandardizationButton.Name = "StandardizationButton"
        Me.StandardizationButton.Size = New System.Drawing.Size(115, 53)
        Me.StandardizationButton.TabIndex = 43
        Me.StandardizationButton.Text = "Document Standardization"
        Me.StandardizationButton.UseVisualStyleBackColor = True
        '
        'ResultListView
        '
        Me.ResultListView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResultListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.columnHeader1, Me.columnHeader2, Me.columnHeader3, Me.columnHeader4})
        Me.ResultListView.FullRowSelect = True
        Me.ResultListView.GridLines = True
        Me.ResultListView.HideSelection = False
        Me.ResultListView.Location = New System.Drawing.Point(566, 81)
        Me.ResultListView.Name = "ResultListView"
        Me.ResultListView.Size = New System.Drawing.Size(303, 229)
        Me.ResultListView.TabIndex = 48
        Me.ResultListView.UseCompatibleStateImageBehavior = False
        Me.ResultListView.View = System.Windows.Forms.View.Details
        '
        'columnHeader1
        '
        Me.columnHeader1.Text = "Rank"
        Me.columnHeader1.Width = 45
        '
        'columnHeader2
        '
        Me.columnHeader2.Text = "Word"
        Me.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.columnHeader2.Width = 80
        '
        'columnHeader3
        '
        Me.columnHeader3.Text = "Entropy Difference"
        Me.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.columnHeader3.Width = 100
        '
        'columnHeader4
        '
        Me.columnHeader4.Text = "Frequency"
        '
        'SelectFileButton
        '
        Me.SelectFileButton.Location = New System.Drawing.Point(594, 36)
        Me.SelectFileButton.Margin = New System.Windows.Forms.Padding(2)
        Me.SelectFileButton.Name = "SelectFileButton"
        Me.SelectFileButton.Size = New System.Drawing.Size(62, 25)
        Me.SelectFileButton.TabIndex = 52
        Me.SelectFileButton.Text = "&Open"
        Me.SelectFileButton.UseVisualStyleBackColor = True
        '
        'RichTextBoxText
        '
        Me.RichTextBoxText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxText.Location = New System.Drawing.Point(6, 6)
        Me.RichTextBoxText.Name = "RichTextBoxText"
        Me.RichTextBoxText.Size = New System.Drawing.Size(509, 304)
        Me.RichTextBoxText.TabIndex = 46
        Me.RichTextBoxText.Text = ""
        '
        'PathTextBox
        '
        Me.PathTextBox.Location = New System.Drawing.Point(143, 38)
        Me.PathTextBox.Name = "PathTextBox"
        Me.PathTextBox.Size = New System.Drawing.Size(437, 20)
        Me.PathTextBox.TabIndex = 41
        '
        'PathLabel
        '
        Me.PathLabel.AutoSize = True
        Me.PathLabel.Location = New System.Drawing.Point(13, 41)
        Me.PathLabel.Name = "PathLabel"
        Me.PathLabel.Size = New System.Drawing.Size(124, 13)
        Me.PathLabel.TabIndex = 40
        Me.PathLabel.Text = "Please Enter A File Path:"
        '
        'StatusLabel
        '
        Me.StatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.Location = New System.Drawing.Point(12, 369)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(35, 13)
        Me.StatusLabel.TabIndex = 51
        Me.StatusLabel.Text = "label1"
        '
        'ContextMenuStripKeyword
        '
        Me.ContextMenuStripKeyword.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToModelToolStripMenuItem, Me.ToolStripMenuItemViewInORMVerbaliser})
        Me.ContextMenuStripKeyword.Name = "ContextMenuStripKeyword"
        Me.ContextMenuStripKeyword.Size = New System.Drawing.Size(196, 48)
        '
        'AddToModelToolStripMenuItem
        '
        Me.AddToModelToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemAddAsEntityType, Me.ToolStripMenuItemAddAsValueType})
        Me.AddToModelToolStripMenuItem.Name = "AddToModelToolStripMenuItem"
        Me.AddToModelToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.AddToModelToolStripMenuItem.Text = "&Add to Model"
        '
        'ToolStripMenuItemAddAsEntityType
        '
        Me.ToolStripMenuItemAddAsEntityType.Name = "ToolStripMenuItemAddAsEntityType"
        Me.ToolStripMenuItemAddAsEntityType.Size = New System.Drawing.Size(154, 22)
        Me.ToolStripMenuItemAddAsEntityType.Text = "...as Entity Type"
        '
        'ToolStripMenuItemAddAsValueType
        '
        Me.ToolStripMenuItemAddAsValueType.Name = "ToolStripMenuItemAddAsValueType"
        Me.ToolStripMenuItemAddAsValueType.Size = New System.Drawing.Size(154, 22)
        Me.ToolStripMenuItemAddAsValueType.Text = "...as Value Type"
        '
        'ToolStripMenuItemViewInORMVerbaliser
        '
        Me.ToolStripMenuItemViewInORMVerbaliser.Name = "ToolStripMenuItemViewInORMVerbaliser"
        Me.ToolStripMenuItemViewInORMVerbaliser.Size = New System.Drawing.Size(195, 22)
        Me.ToolStripMenuItemViewInORMVerbaliser.Text = "View in ORM &Verbaliser"
        '
        'ContextMenuStripTextbox
        '
        Me.ContextMenuStripTextbox.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindToolStripMenuItem, Me.SaveAsToolStripMenuItem})
        Me.ContextMenuStripTextbox.Name = "ContextMenuStripTextbox"
        Me.ContextMenuStripTextbox.Size = New System.Drawing.Size(138, 48)
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.FindToolStripMenuItem.Text = "&Find"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save as..."
        '
        'LabelPromptModel
        '
        Me.LabelPromptModel.AutoSize = True
        Me.LabelPromptModel.Location = New System.Drawing.Point(16, 13)
        Me.LabelPromptModel.Name = "LabelPromptModel"
        Me.LabelPromptModel.Size = New System.Drawing.Size(39, 13)
        Me.LabelPromptModel.TabIndex = 53
        Me.LabelPromptModel.Text = "Model:"
        '
        'LabelModelName
        '
        Me.LabelModelName.AutoSize = True
        Me.LabelModelName.Location = New System.Drawing.Point(62, 12)
        Me.LabelModelName.Name = "LabelModelName"
        Me.LabelModelName.Size = New System.Drawing.Size(90, 13)
        Me.LabelModelName.TabIndex = 54
        Me.LabelModelName.Text = "LabelModelName"
        '
        'ContextMenuStripTextboxSelection
        '
        Me.ContextMenuStripTextboxSelection.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.PlaceInTheVirtualAnalystToolStripMenuItem})
        Me.ContextMenuStripTextboxSelection.Name = "ContextMenuStripTextboxSelection"
        Me.ContextMenuStripTextboxSelection.Size = New System.Drawing.Size(215, 48)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemSelectionAddEntity, Me.ToolStripMenuItemSelectionAddValueType, Me.AsGeneralConceptToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(214, 22)
        Me.ToolStripMenuItem1.Text = "&Add to Model"
        '
        'ToolStripMenuItemSelectionAddEntity
        '
        Me.ToolStripMenuItemSelectionAddEntity.Name = "ToolStripMenuItemSelectionAddEntity"
        Me.ToolStripMenuItemSelectionAddEntity.Size = New System.Drawing.Size(185, 22)
        Me.ToolStripMenuItemSelectionAddEntity.Text = "...as &Entity Type"
        '
        'ToolStripMenuItemSelectionAddValueType
        '
        Me.ToolStripMenuItemSelectionAddValueType.Name = "ToolStripMenuItemSelectionAddValueType"
        Me.ToolStripMenuItemSelectionAddValueType.Size = New System.Drawing.Size(185, 22)
        Me.ToolStripMenuItemSelectionAddValueType.Text = "...as &Value Type"
        '
        'AsGeneralConceptToolStripMenuItem
        '
        Me.AsGeneralConceptToolStripMenuItem.Name = "AsGeneralConceptToolStripMenuItem"
        Me.AsGeneralConceptToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AsGeneralConceptToolStripMenuItem.Text = "...as &General Concept"
        '
        'PlaceInTheVirtualAnalystToolStripMenuItem
        '
        Me.PlaceInTheVirtualAnalystToolStripMenuItem.Name = "PlaceInTheVirtualAnalystToolStripMenuItem"
        Me.PlaceInTheVirtualAnalystToolStripMenuItem.Size = New System.Drawing.Size(214, 22)
        Me.PlaceInTheVirtualAnalystToolStripMenuItem.Text = "Place in the Virtual Analyst"
        '
        'ButtonExtractFactTypeReadings
        '
        Me.ButtonExtractFactTypeReadings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExtractFactTypeReadings.Location = New System.Drawing.Point(889, 327)
        Me.ButtonExtractFactTypeReadings.Name = "ButtonExtractFactTypeReadings"
        Me.ButtonExtractFactTypeReadings.Size = New System.Drawing.Size(115, 40)
        Me.ButtonExtractFactTypeReadings.TabIndex = 55
        Me.ButtonExtractFactTypeReadings.Text = "Extract &Fact Type Readings"
        Me.ButtonExtractFactTypeReadings.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabDocumentText)
        Me.TabControl1.Controls.Add(Me.TabPageResults)
        Me.TabControl1.Controls.Add(Me.TabPageAIExtraction)
        Me.TabControl1.Location = New System.Drawing.Point(12, 67)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(548, 299)
        Me.TabControl1.TabIndex = 56
        '
        'TabDocumentText
        '
        Me.TabDocumentText.Controls.Add(Me.RichTextBoxText)
        Me.TabDocumentText.Location = New System.Drawing.Point(4, 22)
        Me.TabDocumentText.Name = "TabDocumentText"
        Me.TabDocumentText.Padding = New System.Windows.Forms.Padding(3)
        Me.TabDocumentText.Size = New System.Drawing.Size(503, 273)
        Me.TabDocumentText.TabIndex = 0
        Me.TabDocumentText.Text = "Document Text"
        Me.TabDocumentText.UseVisualStyleBackColor = True
        '
        'TabPageResults
        '
        Me.TabPageResults.Controls.Add(Me.ButtonRefereshResults)
        Me.TabPageResults.Controls.Add(Me.ButtonAbort)
        Me.TabPageResults.Controls.Add(Me.ProgressBar)
        Me.TabPageResults.Controls.Add(Me.RichTextBoxResults)
        Me.TabPageResults.Location = New System.Drawing.Point(4, 22)
        Me.TabPageResults.Name = "TabPageResults"
        Me.TabPageResults.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageResults.Size = New System.Drawing.Size(503, 273)
        Me.TabPageResults.TabIndex = 1
        Me.TabPageResults.Text = "Results"
        Me.TabPageResults.UseVisualStyleBackColor = True
        '
        'ButtonRefereshResults
        '
        Me.ButtonRefereshResults.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefereshResults.Image = Global.Boston.My.Resources.Resources.Refresh_16x16
        Me.ButtonRefereshResults.Location = New System.Drawing.Point(490, 8)
        Me.ButtonRefereshResults.Name = "ButtonRefereshResults"
        Me.ButtonRefereshResults.Size = New System.Drawing.Size(25, 23)
        Me.ButtonRefereshResults.TabIndex = 11
        Me.ButtonRefereshResults.UseVisualStyleBackColor = True
        '
        'ButtonAbort
        '
        Me.ButtonAbort.Location = New System.Drawing.Point(6, 8)
        Me.ButtonAbort.Name = "ButtonAbort"
        Me.ButtonAbort.Size = New System.Drawing.Size(43, 23)
        Me.ButtonAbort.TabIndex = 10
        Me.ButtonAbort.Text = "&Abort"
        Me.ButtonAbort.UseVisualStyleBackColor = True
        Me.ButtonAbort.Visible = False
        '
        'ProgressBar
        '
        Me.ProgressBar.Location = New System.Drawing.Point(55, 17)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(100, 14)
        Me.ProgressBar.TabIndex = 9
        Me.ProgressBar.Visible = False
        '
        'RichTextBoxResults
        '
        Me.RichTextBoxResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxResults.ContextMenuStrip = Me.ContextMenuStripResultsSelection
        Me.RichTextBoxResults.Location = New System.Drawing.Point(6, 37)
        Me.RichTextBoxResults.Name = "RichTextBoxResults"
        Me.RichTextBoxResults.Size = New System.Drawing.Size(509, 273)
        Me.RichTextBoxResults.TabIndex = 0
        Me.RichTextBoxResults.Text = ""
        '
        'ContextMenuStripResultsSelection
        '
        Me.ContextMenuStripResultsSelection.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem2, Me.ToolStripMenuItem6})
        Me.ContextMenuStripResultsSelection.Name = "ContextMenuStripTextboxSelection"
        Me.ContextMenuStripResultsSelection.Size = New System.Drawing.Size(215, 48)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem3, Me.ToolStripMenuItem4, Me.ToolStripMenuItem5})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(214, 22)
        Me.ToolStripMenuItem2.Text = "&Add to Model"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(185, 22)
        Me.ToolStripMenuItem3.Text = "...as &Entity Type"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(185, 22)
        Me.ToolStripMenuItem4.Text = "...as &Value Type"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(185, 22)
        Me.ToolStripMenuItem5.Text = "...as &General Concept"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(214, 22)
        Me.ToolStripMenuItem6.Text = "Place in the Virtual Analyst"
        '
        'TabPageAIExtraction
        '
        Me.TabPageAIExtraction.Controls.Add(Me.GroupBoxAILLMPrompt)
        Me.TabPageAIExtraction.Location = New System.Drawing.Point(4, 22)
        Me.TabPageAIExtraction.Name = "TabPageAIExtraction"
        Me.TabPageAIExtraction.Size = New System.Drawing.Size(540, 273)
        Me.TabPageAIExtraction.TabIndex = 2
        Me.TabPageAIExtraction.Text = "AI Extraction"
        Me.TabPageAIExtraction.UseVisualStyleBackColor = True
        '
        'GroupBoxAILLMPrompt
        '
        Me.GroupBoxAILLMPrompt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxAILLMPrompt.Controls.Add(Me.LabelPromptPrompt)
        Me.GroupBoxAILLMPrompt.Controls.Add(Me.ComboBoxFEKLGPTPromptType)
        Me.GroupBoxAILLMPrompt.Controls.Add(Me.TextBoxAILLMPrompt)
        Me.GroupBoxAILLMPrompt.Controls.Add(Me.ButtonExecuteLLMGenerativeAI)
        Me.GroupBoxAILLMPrompt.Location = New System.Drawing.Point(12, 13)
        Me.GroupBoxAILLMPrompt.Name = "GroupBoxAILLMPrompt"
        Me.GroupBoxAILLMPrompt.Size = New System.Drawing.Size(521, 242)
        Me.GroupBoxAILLMPrompt.TabIndex = 0
        Me.GroupBoxAILLMPrompt.TabStop = False
        Me.GroupBoxAILLMPrompt.Text = "LLM Prompt:"
        '
        'LabelPromptPrompt
        '
        Me.LabelPromptPrompt.AutoSize = True
        Me.LabelPromptPrompt.Location = New System.Drawing.Point(6, 31)
        Me.LabelPromptPrompt.Name = "LabelPromptPrompt"
        Me.LabelPromptPrompt.Size = New System.Drawing.Size(70, 13)
        Me.LabelPromptPrompt.TabIndex = 2
        Me.LabelPromptPrompt.Text = "Prompt Type:"
        '
        'ComboBoxFEKLGPTPromptType
        '
        Me.ComboBoxFEKLGPTPromptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFEKLGPTPromptType.FormattingEnabled = True
        Me.ComboBoxFEKLGPTPromptType.Location = New System.Drawing.Point(82, 28)
        Me.ComboBoxFEKLGPTPromptType.Name = "ComboBoxFEKLGPTPromptType"
        Me.ComboBoxFEKLGPTPromptType.Size = New System.Drawing.Size(306, 21)
        Me.ComboBoxFEKLGPTPromptType.TabIndex = 1
        '
        'TextBoxAILLMPrompt
        '
        Me.TextBoxAILLMPrompt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxAILLMPrompt.Location = New System.Drawing.Point(6, 55)
        Me.TextBoxAILLMPrompt.Multiline = True
        Me.TextBoxAILLMPrompt.Name = "TextBoxAILLMPrompt"
        Me.TextBoxAILLMPrompt.Size = New System.Drawing.Size(506, 181)
        Me.TextBoxAILLMPrompt.TabIndex = 0
        '
        'ButtonExecuteLLMGenerativeAI
        '
        Me.ButtonExecuteLLMGenerativeAI.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExecuteLLMGenerativeAI.Location = New System.Drawing.Point(406, 28)
        Me.ButtonExecuteLLMGenerativeAI.Name = "ButtonExecuteLLMGenerativeAI"
        Me.ButtonExecuteLLMGenerativeAI.Size = New System.Drawing.Size(71, 21)
        Me.ButtonExecuteLLMGenerativeAI.TabIndex = 57
        Me.ButtonExecuteLLMGenerativeAI.Text = "E&xecute"
        Me.ButtonExecuteLLMGenerativeAI.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel, Me.ToolStripStatusLabelChunkCount})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 450)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1026, 22)
        Me.StatusStrip1.TabIndex = 58
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(113, 17)
        Me.ToolStripStatusLabel.Text = "ToolStripStatusLabel"
        '
        'ToolStripStatusLabelChunkCount
        '
        Me.ToolStripStatusLabelChunkCount.Name = "ToolStripStatusLabelChunkCount"
        Me.ToolStripStatusLabelChunkCount.Size = New System.Drawing.Size(181, 17)
        Me.ToolStripStatusLabelChunkCount.Text = "ToolStripStatusLabelChunkCount"
        '
        'ButtonClose
        '
        Me.ButtonClose.Location = New System.Drawing.Point(892, 38)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 59
        Me.ButtonClose.Text = "&Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'frmKnowledgeExtraction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1026, 472)
        Me.Controls.Add(Me.ButtonClose)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ButtonExtractFactTypeReadings)
        Me.Controls.Add(Me.LabelModelName)
        Me.Controls.Add(Me.LabelPromptModel)
        Me.Controls.Add(Me.HelpButton)
        Me.Controls.Add(Me.KeywordExtractionNormalButton)
        Me.Controls.Add(Me.progressBar1)
        Me.Controls.Add(Me.KeywordExtractionMaxButton)
        Me.Controls.Add(Me.RemoveStopButton)
        Me.Controls.Add(Me.StandardizationButton)
        Me.Controls.Add(Me.ResultListView)
        Me.Controls.Add(Me.SelectFileButton)
        Me.Controls.Add(Me.PathTextBox)
        Me.Controls.Add(Me.PathLabel)
        Me.Controls.Add(Me.StatusLabel)
        Me.Name = "frmKnowledgeExtraction"
        Me.Text = "Knowledge Extraction (beta)"
        Me.ContextMenuStripKeyword.ResumeLayout(False)
        Me.ContextMenuStripTextbox.ResumeLayout(False)
        Me.ContextMenuStripTextboxSelection.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabDocumentText.ResumeLayout(False)
        Me.TabPageResults.ResumeLayout(False)
        Me.ContextMenuStripResultsSelection.ResumeLayout(False)
        Me.TabPageAIExtraction.ResumeLayout(False)
        Me.GroupBoxAILLMPrompt.ResumeLayout(False)
        Me.GroupBoxAILLMPrompt.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Shadows WithEvents HelpButton As Button
    Private WithEvents KeywordExtractionNormalButton As Button
    Private WithEvents progressBar1 As ProgressBar
    Private WithEvents KeywordExtractionMaxButton As Button
    Private WithEvents RemoveStopButton As Button
    Private WithEvents StandardizationButton As Button
    Private WithEvents ResultListView As ListView
    Private WithEvents columnHeader1 As ColumnHeader
    Private WithEvents columnHeader2 As ColumnHeader
    Private WithEvents columnHeader3 As ColumnHeader
    Private WithEvents columnHeader4 As ColumnHeader
    Friend WithEvents SelectFileButton As Button
    Private WithEvents RichTextBoxText As RichTextBox
    Private WithEvents PathTextBox As TextBox
    Private WithEvents PathLabel As Label
    Private WithEvents StatusLabel As Label
    Friend WithEvents ContextMenuStripKeyword As ContextMenuStrip
    Friend WithEvents AddToModelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddAsEntityType As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemAddAsValueType As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemViewInORMVerbaliser As ToolStripMenuItem
    Friend WithEvents ContextMenuStripTextbox As ContextMenuStrip
    Friend WithEvents FindToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LabelPromptModel As Label
    Friend WithEvents LabelModelName As Label
    Friend WithEvents ContextMenuStripTextboxSelection As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSelectionAddEntity As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSelectionAddValueType As ToolStripMenuItem
    Friend WithEvents AsGeneralConceptToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ButtonExtractFactTypeReadings As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabDocumentText As TabPage
    Friend WithEvents TabPageResults As TabPage
    Friend WithEvents RichTextBoxResults As RichTextBox
    Friend WithEvents TabPageAIExtraction As TabPage
    Friend WithEvents GroupBoxAILLMPrompt As GroupBox
    Friend WithEvents TextBoxAILLMPrompt As TextBox
    Friend WithEvents ButtonExecuteLLMGenerativeAI As Button
    Friend WithEvents ButtonAbort As Button
    Friend WithEvents ProgressBar As ProgressBar
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabelChunkCount As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents PlaceInTheVirtualAnalystToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ContextMenuStripResultsSelection As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripMenuItem
    Friend WithEvents ButtonRefereshResults As Button
    Friend WithEvents LabelPromptPrompt As Label
    Friend WithEvents ComboBoxFEKLGPTPromptType As ComboBox
    Friend WithEvents ButtonClose As Button
End Class
