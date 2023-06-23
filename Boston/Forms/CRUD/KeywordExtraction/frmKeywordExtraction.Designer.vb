<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKeywordExtraction
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
        Me.SaveButton = New System.Windows.Forms.Button()
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
        Me.TextRichTextBox = New System.Windows.Forms.RichTextBox()
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
        Me.ButtonExtractFactTypeReadings = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabDocumentText = New System.Windows.Forms.TabPage()
        Me.TabPageResults = New System.Windows.Forms.TabPage()
        Me.RichTextBoxResults = New System.Windows.Forms.RichTextBox()
        Me.ContextMenuStripKeyword.SuspendLayout()
        Me.ContextMenuStripTextbox.SuspendLayout()
        Me.ContextMenuStripTextboxSelection.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabDocumentText.SuspendLayout()
        Me.TabPageResults.SuspendLayout()
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
        Me.KeywordExtractionNormalButton.Location = New System.Drawing.Point(875, 199)
        Me.KeywordExtractionNormalButton.Name = "KeywordExtractionNormalButton"
        Me.KeywordExtractionNormalButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionNormalButton.TabIndex = 49
        Me.KeywordExtractionNormalButton.Text = "Keyword Extraction (Entropy)"
        Me.KeywordExtractionNormalButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(661, 36)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 25)
        Me.SaveButton.TabIndex = 42
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'progressBar1
        '
        Me.progressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.progressBar1.Location = New System.Drawing.Point(12, 462)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(809, 25)
        Me.progressBar1.TabIndex = 47
        '
        'KeywordExtractionMaxButton
        '
        Me.KeywordExtractionMaxButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeywordExtractionMaxButton.Location = New System.Drawing.Point(875, 258)
        Me.KeywordExtractionMaxButton.Name = "KeywordExtractionMaxButton"
        Me.KeywordExtractionMaxButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionMaxButton.TabIndex = 45
        Me.KeywordExtractionMaxButton.Text = "Keyword Extraction (Maximum Entropy)"
        Me.KeywordExtractionMaxButton.UseVisualStyleBackColor = True
        '
        'RemoveStopButton
        '
        Me.RemoveStopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveStopButton.Location = New System.Drawing.Point(875, 140)
        Me.RemoveStopButton.Name = "RemoveStopButton"
        Me.RemoveStopButton.Size = New System.Drawing.Size(115, 53)
        Me.RemoveStopButton.TabIndex = 44
        Me.RemoveStopButton.Text = "Remove Stop"
        Me.RemoveStopButton.UseVisualStyleBackColor = True
        '
        'StandardizationButton
        '
        Me.StandardizationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StandardizationButton.Location = New System.Drawing.Point(875, 81)
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
        Me.ResultListView.Location = New System.Drawing.Point(552, 81)
        Me.ResultListView.Name = "ResultListView"
        Me.ResultListView.Size = New System.Drawing.Size(303, 306)
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
        'TextRichTextBox
        '
        Me.TextRichTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextRichTextBox.Location = New System.Drawing.Point(6, 6)
        Me.TextRichTextBox.Name = "TextRichTextBox"
        Me.TextRichTextBox.Size = New System.Drawing.Size(514, 338)
        Me.TextRichTextBox.TabIndex = 46
        Me.TextRichTextBox.Text = ""
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
        Me.StatusLabel.Location = New System.Drawing.Point(12, 446)
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
        Me.ContextMenuStripTextboxSelection.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.ContextMenuStripTextboxSelection.Name = "ContextMenuStripTextboxSelection"
        Me.ContextMenuStripTextboxSelection.Size = New System.Drawing.Size(148, 26)
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemSelectionAddEntity, Me.ToolStripMenuItemSelectionAddValueType, Me.AsGeneralConceptToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(147, 22)
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
        'ButtonExtractFactTypeReadings
        '
        Me.ButtonExtractFactTypeReadings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonExtractFactTypeReadings.Location = New System.Drawing.Point(875, 327)
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
        Me.TabControl1.Location = New System.Drawing.Point(12, 67)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(534, 376)
        Me.TabControl1.TabIndex = 56
        '
        'TabDocumentText
        '
        Me.TabDocumentText.Controls.Add(Me.TextRichTextBox)
        Me.TabDocumentText.Location = New System.Drawing.Point(4, 22)
        Me.TabDocumentText.Name = "TabDocumentText"
        Me.TabDocumentText.Padding = New System.Windows.Forms.Padding(3)
        Me.TabDocumentText.Size = New System.Drawing.Size(526, 350)
        Me.TabDocumentText.TabIndex = 0
        Me.TabDocumentText.Text = "Document Text"
        Me.TabDocumentText.UseVisualStyleBackColor = True
        '
        'TabPageResults
        '
        Me.TabPageResults.Controls.Add(Me.RichTextBoxResults)
        Me.TabPageResults.Location = New System.Drawing.Point(4, 22)
        Me.TabPageResults.Name = "TabPageResults"
        Me.TabPageResults.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageResults.Size = New System.Drawing.Size(483, 365)
        Me.TabPageResults.TabIndex = 1
        Me.TabPageResults.Text = "Results"
        Me.TabPageResults.UseVisualStyleBackColor = True
        '
        'RichTextBoxResults
        '
        Me.RichTextBoxResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxResults.Location = New System.Drawing.Point(6, 6)
        Me.RichTextBoxResults.Name = "RichTextBoxResults"
        Me.RichTextBoxResults.Size = New System.Drawing.Size(471, 353)
        Me.RichTextBoxResults.TabIndex = 0
        Me.RichTextBoxResults.Text = ""
        '
        'frmKeywordExtraction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1012, 549)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ButtonExtractFactTypeReadings)
        Me.Controls.Add(Me.LabelModelName)
        Me.Controls.Add(Me.LabelPromptModel)
        Me.Controls.Add(Me.HelpButton)
        Me.Controls.Add(Me.KeywordExtractionNormalButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.progressBar1)
        Me.Controls.Add(Me.KeywordExtractionMaxButton)
        Me.Controls.Add(Me.RemoveStopButton)
        Me.Controls.Add(Me.StandardizationButton)
        Me.Controls.Add(Me.ResultListView)
        Me.Controls.Add(Me.SelectFileButton)
        Me.Controls.Add(Me.PathTextBox)
        Me.Controls.Add(Me.PathLabel)
        Me.Controls.Add(Me.StatusLabel)
        Me.Name = "frmKeywordExtraction"
        Me.Text = "Keyword Extraction (beta)"
        Me.ContextMenuStripKeyword.ResumeLayout(False)
        Me.ContextMenuStripTextbox.ResumeLayout(False)
        Me.ContextMenuStripTextboxSelection.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabDocumentText.ResumeLayout(False)
        Me.TabPageResults.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Shadows WithEvents HelpButton As Button
    Private WithEvents KeywordExtractionNormalButton As Button
    Private WithEvents SaveButton As Button
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
    Private WithEvents TextRichTextBox As RichTextBox
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
End Class
