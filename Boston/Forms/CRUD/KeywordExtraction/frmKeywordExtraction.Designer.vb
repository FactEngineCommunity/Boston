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
        Me.ContextMenuStripKeyword.SuspendLayout()
        Me.ContextMenuStripTextbox.SuspendLayout()
        Me.SuspendLayout()
        '
        'HelpButton
        '
        Me.HelpButton.Location = New System.Drawing.Point(743, 30)
        Me.HelpButton.Name = "HelpButton"
        Me.HelpButton.Size = New System.Drawing.Size(75, 25)
        Me.HelpButton.TabIndex = 50
        Me.HelpButton.Text = "Help"
        Me.HelpButton.UseVisualStyleBackColor = True
        '
        'KeywordExtractionNormalButton
        '
        Me.KeywordExtractionNormalButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeywordExtractionNormalButton.Location = New System.Drawing.Point(885, 199)
        Me.KeywordExtractionNormalButton.Name = "KeywordExtractionNormalButton"
        Me.KeywordExtractionNormalButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionNormalButton.TabIndex = 49
        Me.KeywordExtractionNormalButton.Text = "Keyword Extraction (Entropy)"
        Me.KeywordExtractionNormalButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(662, 30)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 25)
        Me.SaveButton.TabIndex = 42
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'progressBar1
        '
        Me.progressBar1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.progressBar1.Location = New System.Drawing.Point(30, 446)
        Me.progressBar1.Name = "progressBar1"
        Me.progressBar1.Size = New System.Drawing.Size(809, 25)
        Me.progressBar1.TabIndex = 47
        '
        'KeywordExtractionMaxButton
        '
        Me.KeywordExtractionMaxButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KeywordExtractionMaxButton.Location = New System.Drawing.Point(885, 258)
        Me.KeywordExtractionMaxButton.Name = "KeywordExtractionMaxButton"
        Me.KeywordExtractionMaxButton.Size = New System.Drawing.Size(115, 53)
        Me.KeywordExtractionMaxButton.TabIndex = 45
        Me.KeywordExtractionMaxButton.Text = "Keyword Extraction (Maximum Entropy)"
        Me.KeywordExtractionMaxButton.UseVisualStyleBackColor = True
        '
        'RemoveStopButton
        '
        Me.RemoveStopButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveStopButton.Location = New System.Drawing.Point(885, 140)
        Me.RemoveStopButton.Name = "RemoveStopButton"
        Me.RemoveStopButton.Size = New System.Drawing.Size(115, 53)
        Me.RemoveStopButton.TabIndex = 44
        Me.RemoveStopButton.Text = "Remove Stop"
        Me.RemoveStopButton.UseVisualStyleBackColor = True
        '
        'StandardizationButton
        '
        Me.StandardizationButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StandardizationButton.Location = New System.Drawing.Point(885, 81)
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
        Me.ResultListView.Location = New System.Drawing.Point(562, 81)
        Me.ResultListView.Name = "ResultListView"
        Me.ResultListView.Size = New System.Drawing.Size(303, 321)
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
        Me.SelectFileButton.Location = New System.Drawing.Point(595, 30)
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
        Me.TextRichTextBox.Location = New System.Drawing.Point(30, 81)
        Me.TextRichTextBox.Name = "TextRichTextBox"
        Me.TextRichTextBox.Size = New System.Drawing.Size(523, 321)
        Me.TextRichTextBox.TabIndex = 46
        Me.TextRichTextBox.Text = ""
        '
        'PathTextBox
        '
        Me.PathTextBox.Location = New System.Drawing.Point(144, 32)
        Me.PathTextBox.Name = "PathTextBox"
        Me.PathTextBox.Size = New System.Drawing.Size(437, 20)
        Me.PathTextBox.TabIndex = 41
        '
        'PathLabel
        '
        Me.PathLabel.AutoSize = True
        Me.PathLabel.Location = New System.Drawing.Point(14, 35)
        Me.PathLabel.Name = "PathLabel"
        Me.PathLabel.Size = New System.Drawing.Size(124, 13)
        Me.PathLabel.TabIndex = 40
        Me.PathLabel.Text = "Please Enter A File Path:"
        '
        'StatusLabel
        '
        Me.StatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.Location = New System.Drawing.Point(27, 422)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(35, 13)
        Me.StatusLabel.TabIndex = 51
        Me.StatusLabel.Text = "label1"
        '
        'ContextMenuStripKeyword
        '
        Me.ContextMenuStripKeyword.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToModelToolStripMenuItem, Me.ToolStripMenuItemViewInORMVerbaliser})
        Me.ContextMenuStripKeyword.Name = "ContextMenuStripKeyword"
        Me.ContextMenuStripKeyword.Size = New System.Drawing.Size(196, 70)
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
        Me.ToolStripMenuItemAddAsEntityType.Size = New System.Drawing.Size(180, 22)
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
        Me.ContextMenuStripTextbox.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindToolStripMenuItem})
        Me.ContextMenuStripTextbox.Name = "ContextMenuStripTextbox"
        Me.ContextMenuStripTextbox.Size = New System.Drawing.Size(138, 26)
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FindToolStripMenuItem.Text = "&Find"
        '
        'frmKeywordExtraction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 564)
        Me.Controls.Add(Me.HelpButton)
        Me.Controls.Add(Me.KeywordExtractionNormalButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.progressBar1)
        Me.Controls.Add(Me.KeywordExtractionMaxButton)
        Me.Controls.Add(Me.RemoveStopButton)
        Me.Controls.Add(Me.StandardizationButton)
        Me.Controls.Add(Me.ResultListView)
        Me.Controls.Add(Me.SelectFileButton)
        Me.Controls.Add(Me.TextRichTextBox)
        Me.Controls.Add(Me.PathTextBox)
        Me.Controls.Add(Me.PathLabel)
        Me.Controls.Add(Me.StatusLabel)
        Me.Name = "frmKeywordExtraction"
        Me.Text = "Keyword Extraction"
        Me.ContextMenuStripKeyword.ResumeLayout(False)
        Me.ContextMenuStripTextbox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents HelpButton As Button
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
End Class
