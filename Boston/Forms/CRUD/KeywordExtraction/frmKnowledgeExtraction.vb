Imports System.Reflection
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Collections
Imports edu.stanford.nlp.ling
Imports edu.stanford.nlp.pipeline
Imports edu.stanford.nlp.semgraph
Imports edu.stanford.nlp.semgraph.semgrex
Imports edu.stanford.nlp.trees
Imports edu.stanford.nlp.util
Imports edu.stanford.nlp.ling.CoreAnnotations
Imports edu.stanford.nlp.trees.GrammaticalRelation
Imports edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations
Imports edu.stanford.nlp.semgraph.SemanticGraph
Imports edu.stanford.nlp.semgraph.SemanticGraphEdge
Imports edu.stanford.nlp.ling.IndexedWord
Imports java.util
Imports Syn.WordNet
Imports Newtonsoft.Json

Public Class frmKnowledgeExtraction

	'===========================================================================================================
	'NB There used to be a [Save] button. See: 	Private Sub SaveButton_Click(sender As Object, e As EventArgs)
	'===========================================================================================================

	Private lvwColumnSorter As ListViewColumnSorter
	Public WithEvents mrModel As FBM.Model
	Private mfrmFindDialog As New frmTextboxFind()
	Private mbAbort As Boolean = False 'To abort a process
	Dim mrOpenAIAPI As OpenAI_API.OpenAIAPI

	Private wordNet As New WordNetEngine()

	Private FEKLGPTKnowledgeExtractor As New FEKL.FEKLGPTKnowledgeExtractor

	''' <summary>
	''' Changed with the user clicks on a keyword.
	''' </summary>
	Private zrSelectedModelElement As FBM.ModelObject = Nothing

#Region "Class - ListViewColumnSorter"
	''' <summary>
	''' This class is an implementation of the 'IComparer' interface.
	''' </summary>
	Public Class ListViewColumnSorter
		Implements IComparer
		''' <summary>
		''' Specifies the column to be sorted
		''' </summary>
		Private ColumnToSort As Integer
		''' <summary>
		''' Specifies the order in which to sort (i.e. 'Ascending').
		''' </summary>
		Private OrderOfSort As SortOrder
		''' <summary>
		''' Case insensitive comparer object
		''' </summary>
		Private ObjectCompare As CaseInsensitiveComparer

		''' <summary>
		''' Class constructor.  Initializes various elements
		''' </summary>
		Public Sub New()
			' Initialize the column to '0'
			ColumnToSort = 0

			' Initialize the sort order to 'none'
			OrderOfSort = SortOrder.None

			' Initialize the CaseInsensitiveComparer object
			ObjectCompare = New CaseInsensitiveComparer()
		End Sub



		''' <summary>
		''' This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
		''' </summary>
		''' <param name="x">First object to be compared</param>
		''' <param name="y">Second object to be compared</param>
		''' <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
		Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
			Dim compareResult As Integer
			Dim listviewX As ListViewItem, listviewY As ListViewItem

			' Cast the objects to be compared to ListViewItem objects
			listviewX = DirectCast(x, ListViewItem)
			listviewY = DirectCast(y, ListViewItem)

			Dim num As Decimal = 0
			Try
				' Suspect code  
				If Decimal.TryParse(listviewX.SubItems(ColumnToSort).Text, num) Then
					compareResult = Decimal.Compare(num, Convert.ToDecimal(listviewY.SubItems(ColumnToSort).Text))
				Else
					' Compare the two items
					compareResult = ObjectCompare.Compare(listviewX.SubItems(ColumnToSort).Text, listviewY.SubItems(ColumnToSort).Text)
				End If
			Catch e As Exception
				' Action after the exception is caught  
				compareResult = ObjectCompare.Compare(listviewX.SubItems(ColumnToSort).Text, listviewY.SubItems(ColumnToSort).Text)
			End Try

			' Calculate correct return value based on object comparison
			If OrderOfSort = SortOrder.Ascending Then
				' Ascending sort is selected, return normal result of compare operation
				Return compareResult
			ElseIf OrderOfSort = SortOrder.Descending Then
				' Descending sort is selected, return negative result of compare operation
				Return (-compareResult)
			Else
				' Return '0' to indicate they are equal
				Return 0
			End If
		End Function

		''' <summary>
		''' Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
		''' </summary>
		Public Property SortColumn() As Integer
			Get
				Return ColumnToSort
			End Get
			Set
				ColumnToSort = Value
			End Set
		End Property

		''' <summary>
		''' Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
		''' </summary>
		Public Property Order() As SortOrder
			Get
				Return OrderOfSort
			End Get
			Set
				OrderOfSort = Value
			End Set
		End Property

	End Class

#End Region
	Private Sub KeywordExtractionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Call Me.SetupForm()

		' Create an instance of a ListView column sorter and assign it to the ListView control.
		lvwColumnSorter = New ListViewColumnSorter()
		Me.ResultListView.ListViewItemSorter = lvwColumnSorter

		System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

		StandardizationButton.Enabled = False
		RemoveStopButton.Enabled = False
		KeywordExtractionMaxButton.Enabled = False
		KeywordExtractionNormalButton.Enabled = False
		StatusLabel.Text = "Welcome to keyword extraction software, which is based on the entropy difference."
		'Help Text
		'MessageBox.Show("The process of document keyword extraction:" & vbLf & vbLf & "Step 1: Open the document: Click ""Open"" button, select the target document. (File should be ""*. Txt "" file)" & vbLf & vbLf & "Step 2: Document standardization: Click ""Document Standardization"" button to remove the document punctuation, line breaks, and other useless symbols, and text replaces lowercase letters. (If the above operations have been completed, you can skip this step.)" & vbLf & vbLf & "Step 3: Stop word removal: Click ""Remove Stop"" button and follow the list of stop words, removing stop words in the document. (This step is optional, remove stop words, it may improve the accuracy of keyword extraction.)" & vbLf & vbLf & "Step 4: Here are two method to extract the keyword, you can choose one of them to finish the Keyword extraction work." & vbLf & "    4.1: Keyword extraction (normal entropy): Click ""Keyword Extraction (Entropy)"" button to extract keywords." & vbLf & "    4.2: Keyword extraction (maximum entropy): Click ""Keyword Extraction (Maximum Entropy)"" button and follow the maximum entropy method to extract keywords.", "Help")

		'Wordnet
		Call Me.wordNet.LoadFromDirectory(My.Settings.WordNetDictionaryEnglishPath)


#Region "FEKL Knowledge Extractor"

		'Load JSON schema
		Dim JsonContent As String = Boston.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "Boston.FEKL-GPT-KnowledgeExtractionPrompts.json") ' Path to your JSON schema
		'Deserialize JSON into RootObject
		Me.FEKLGPTKnowledgeExtractor = JsonConvert.DeserializeObject(Of FEKL.FEKLGPTKnowledgeExtractor)(JsonContent)

		For Each lrFEKLGPTKnowledgeExtractorPrompt In Me.FEKLGPTKnowledgeExtractor.Prompt

			Dim lrComboboxItem As New tComboboxItem(lrFEKLGPTKnowledgeExtractorPrompt.Description, lrFEKLGPTKnowledgeExtractorPrompt.Description, lrFEKLGPTKnowledgeExtractorPrompt)
			Me.ComboBoxFEKLGPTPromptType.Items.Add(lrComboboxItem)
		Next


#End Region

	End Sub

	Private Sub SetupForm()

		Try
			Me.LabelModelName.Text = Me.mrModel.Name

			'Status Tool Strip
			Me.ToolStripStatusLabel.Text = ""
			Me.ToolStripStatusLabelChunkCount.Text = ""

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub StandardizationButton_Click(sender As Object, e As EventArgs) Handles StandardizationButton.Click
		CloseButtons()
		MyData.TheDoc = MyFun.DocStandardization(MyData.TheDoc)
		StatusLabel.Text = "Document standardization process has been completed."
		OpenButton()
	End Sub

	Private Sub RemoveStopButton_Click(sender As Object, e As EventArgs) Handles RemoveStopButton.Click
		CloseButtons()
		'MessageBox.Show("Tip: If the document is longer, it may take you few minutes, please be patient ...", "Prompt", 0, MessageBoxIcon.Asterisk)

		Try
			With New WaitCursor
				MyData.TheDoc = MyFun.RemoveStop(MyData.TheDoc, Me.progressBar1)
				StatusLabel.Text = "Remove stop-words has been completed."
				OpenButton()
			End With

		Catch ex As Exception
			OpenButton()
		End Try

	End Sub

	Private Sub KeywordExtractionButton_Click(sender As Object, e As EventArgs) Handles KeywordExtractionMaxButton.Click
		Dim thread As New Thread(New ThreadStart(AddressOf DoWork))
		thread.IsBackground = True
		thread.Start()
	End Sub

	Private Sub DoWork()

		Try
			SyncLock Me
				CloseButtons()

				Dim dt1 As DateTime = DateTime.Now

				StatusLabel.Text = "Keyword extraction process is ongoing:0%"

				ResultListView.Items.Clear()

				MyData.WordsFre = MyFun.StatisticsWords(MyData.TheDoc)

				For i As Integer = 0 To MyData.WordsFre.Length - 1
					MyData.WordsFre(i).EntropyDifference_Max()
					progressBar1.Value = i * 100 \ MyData.WordsFre.Length
					StatusLabel.Text = "Keyword extraction process is ongoing: " & progressBar1.Value & "%"
				Next

				MyFun.QuickSort(MyData.WordsFre, 0, MyData.WordsFre.Length - 1)

				Dim WordsNum As Integer = 0
				For i As Integer = 0 To MyData.WordsFre.Length - 1
					If MyData.WordsFre(i).ED > 0 Then
						WordsNum += 1
					Else
						Exit For
					End If
				Next
				Dim lvi As ListViewItem() = New ListViewItem(WordsNum - 1) {}
				For i As Integer = 0 To WordsNum - 1
					lvi(i) = New ListViewItem()
					lvi(i).SubItems(0).Text = (i + 1).ToString()
					lvi(i).SubItems.Add(MyData.WordsFre(i).Word.ToString())

					If Me.mrModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(MyData.WordsFre(i).Word.ToString), True) IsNot Nothing Then
						MyData.WordsFre(i).IsInModel = True
						Dim loFont = New Font("Arial", 10, FontStyle.Bold)
						lvi(i).SubItems(1).Font = loFont
						lvi(i).ForeColor = Color.RoyalBlue
						'lvi(i).BackColor = Color.Beige
					End If

					lvi(i).SubItems.Add(MyData.WordsFre(i).ED.ToString())
					lvi(i).SubItems.Add(MyData.WordsFre(i).Frequency.ToString())
				Next
				ResultListView.Items.AddRange(lvi)

				Dim dt2 As DateTime = DateTime.Now

				progressBar1.Value = 100

				StatusLabel.Text = "Keyword extraction has been completed. The extraction spend " & (dt2 - dt1).ToString() & "."

				OpenButton()
			End SyncLock
		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try
	End Sub

	Private Sub SaveButton_Click(sender As Object, e As EventArgs)

		Dim save As String()
		Dim WordsNum As Integer = 0
		For i As Integer = 0 To MyData.WordsFre.Length - 1
			If MyData.WordsFre(i).ED > 0 Then
				WordsNum += 1
			Else
				Exit For
			End If
		Next
		save = New String(WordsNum - 1) {}
		For i As Integer = 0 To WordsNum - 1
			save(i) = (i + 1).ToString() & vbTab & MyData.WordsFre(i).Word.ToString() & vbTab & MyData.WordsFre(i).ED.ToString()
		Next
		Dim sfd As New SaveFileDialog()
		sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
		If sfd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			Dim encode As Encoding = Encoding.GetEncoding("GB2312")
			File.WriteAllLines(sfd.FileName, save)
		End If

	End Sub

	Private Sub OpenButton()
		SelectFileButton.Enabled = True
		StandardizationButton.Enabled = True
		RemoveStopButton.Enabled = True
		KeywordExtractionMaxButton.Enabled = True
		KeywordExtractionNormalButton.Enabled = True
	End Sub

	Private Sub CloseButtons()
		SelectFileButton.Enabled = False
		StandardizationButton.Enabled = False
		RemoveStopButton.Enabled = False
		KeywordExtractionMaxButton.Enabled = False
		KeywordExtractionNormalButton.Enabled = False
	End Sub

	Private Sub KeywordExtractionForm_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
		Dim path As String = DirectCast(e.Data.GetData(DataFormats.FileDrop), System.Array).GetValue(0).ToString()
		Dim regex As New Regex("^*.txt$")
		Dim ma As Match = regex.Match(path)
		If ma.Success Then
			PathTextBox.Text = path
			Dim encode As Encoding = Encoding.GetEncoding("GB2312")
			MyData.TheDoc = File.ReadAllText(path, encode)
			RichTextBoxText.Text = MyData.TheDoc

			ResultListView.Items.Clear()
			StandardizationButton.Enabled = True
			RemoveStopButton.Enabled = True
			KeywordExtractionMaxButton.Enabled = True
			KeywordExtractionNormalButton.Enabled = True
			StatusLabel.Text = path & " file open finish."
		Else
			MessageBox.Show("Sorry, your documents do not meet format, please select ""txt"" file.")
		End If
	End Sub

	Private Sub KeywordExtractionForm_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
		If e.Data.GetDataPresent(DataFormats.FileDrop) Then
			e.Effect = DragDropEffects.Link
		Else
			e.Effect = DragDropEffects.None
		End If
	End Sub

	Private Sub ResultListView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ResultListView.SelectedIndexChanged

		Try
			If ResultListView.SelectedIndices IsNot Nothing AndAlso ResultListView.SelectedIndices.Count > 0 Then
				Dim g As Graphics = Me.CreateGraphics()
				g.Clear(Me.BackColor)
				Dim word As String = ""
				Dim c As ListView.SelectedIndexCollection = ResultListView.SelectedIndices
				word = ResultListView.Items(c(0)).SubItems(1).Text

				Dim WordIndex As Integer = 0
				For WordIndex = 0 To MyData.WordsFre.Length - 1
					If word = MyData.WordsFre(WordIndex).Word Then
						Exit For
					End If
				Next

				Dim lrModelElement = Me.mrModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(word), True)
				If lrModelElement IsNot Nothing Then
					'-------------------------------------------------------
					'ORM Verbalisation
					'-------------------------------------------------------
					Dim lrToolboxForm As frmToolboxORMVerbalisation
					lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
					If IsSomething(lrToolboxForm) Then
						lrToolboxForm.zrModel = Me.mrModel
						Call lrToolboxForm.verbaliseModelElement(lrModelElement)
					End If

				End If
				Me.zrSelectedModelElement = lrModelElement

				Dim max As Integer = 0
				For Each length As Integer In MyData.WordsFre(WordIndex).Distance
					max += length
				Next

				Dim p As New Pen(Color.Black, 2)
				g.DrawRectangle(p, 38, Me.Size.Height - 80, 809, 60)
				Dim b1 As New SolidBrush(Color.Black)

				g.DrawString("0", New Font("Arial", 10), b1, New PointF(33, Me.Size.Height - 20))
				g.DrawString((max \ 10).ToString(), New Font("Arial", 10), b1, New PointF(113, Me.Size.Height - 20))
				g.DrawString((max * 2 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(193, Me.Size.Height - 20))
				g.DrawString((max * 3 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(273, Me.Size.Height - 20))
				g.DrawString((max * 4 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(353, Me.Size.Height - 20))
				g.DrawString((max * 5 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(433, Me.Size.Height - 20))
				g.DrawString((max * 6 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(513, Me.Size.Height - 20))
				g.DrawString((max * 7 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(593, Me.Size.Height - 20))
				g.DrawString((max * 8 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(673, Me.Size.Height - 20))
				g.DrawString((max * 9 \ 10).ToString(), New Font("Arial", 10), b1, New PointF(753, Me.Size.Height - 20))
				g.DrawString(max.ToString(), New Font("Arial", 10), b1, New PointF(833, Me.Size.Height - 20))
				For Each po As Integer In MyData.WordsFre(WordIndex).Position
					Dim x As Single = CSng(38 + 1.0 * po * 809 / max)
					g.DrawLine(p, x, Me.Size.Height - 80, x, Me.Size.Height - 20)
				Next
			End If

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,,,,, ex)
		End Try

	End Sub


	Private Sub DoWork_Normal()
		SyncLock Me
			CloseButtons()

			Try
				Dim dt1 As DateTime = DateTime.Now

				StatusLabel.Text = "Keyword extraction process is ongoing: 0%"

				ResultListView.Items.Clear()

				MyData.WordsFre = MyFun.StatisticsWords(MyData.TheDoc)

				For i As Integer = 0 To MyData.WordsFre.Length - 1
					MyData.WordsFre(i).EntropyDifference_Normal()
					progressBar1.Value = i * 100 \ MyData.WordsFre.Length
					StatusLabel.Text = "Keyword extraction process is ongoing: " & progressBar1.Value & "%"
				Next

				MyFun.QuickSort(MyData.WordsFre, 0, MyData.WordsFre.Length - 1)

				Dim WordsNum As Integer = 0
				For i As Integer = 0 To MyData.WordsFre.Length - 1
					If MyData.WordsFre(i).ED > 0 Then
						WordsNum += 1
					Else
						Exit For
					End If
				Next
				Dim lvi As ListViewItem() = New ListViewItem(WordsNum - 1) {}
				For i As Integer = 0 To WordsNum - 1
					lvi(i) = New ListViewItem()
					lvi(i).SubItems(0).Text = (i + 1).ToString()
					lvi(i).SubItems.Add(MyData.WordsFre(i).Word.ToString())

					If Me.mrModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(MyData.WordsFre(i).Word.ToString), True) IsNot Nothing Then
						MyData.WordsFre(i).IsInModel = True
						Dim loFont = New Font("Arial", 10, FontStyle.Bold)
						lvi(i).SubItems(1).Font = loFont
						lvi(i).ForeColor = Color.RoyalBlue
						'lvi(i).BackColor = Color.Beige
					End If
					lvi(i).SubItems.Add(MyData.WordsFre(i).ED.ToString())
					lvi(i).SubItems.Add(MyData.WordsFre(i).Frequency.ToString())
				Next
				ResultListView.Items.AddRange(lvi)

				Dim dt2 As DateTime = DateTime.Now

				progressBar1.Value = 100

				StatusLabel.Text = "Keyword extraction has been completed. The extraction spend " & (dt2 - dt1).ToString() & "."

				OpenButton()

			Catch ex As Exception
				OpenButton()
			End Try

		End SyncLock
	End Sub

	Private Sub HelpButton_Click(sender As Object, e As EventArgs) Handles HelpButton.Click
		MessageBox.Show("The process of document keyword extraction:" & vbLf & vbLf & "Step 1: Open the document: Click ""Open"" button, select the target document. (File should be ""*. Txt "" file)" & vbLf & vbLf & "Step 2: Document standardization: Click ""Document Standardization"" button to remove the document punctuation, line breaks, and other useless symbols, and text replaces lowercase letters. (If the above operations have been completed, you can skip this step.)" & vbLf & vbLf & "Step 3: Stop word removal: Click ""Remove Stop"" button and follow the list of stop words, removing stop words in the document. (This step is optional, remove stop words, it may improve the accuracy of keyword extraction.)" & vbLf & vbLf & "Step 4: Here are two method to extract the keyword, you can choose one of them to finish the Keyword extraction work." & vbLf & "    4.1: Keyword extraction (normal entropy): Click ""Keyword Extraction (Entropy)"" button to extract keywords." & vbLf & "    4.2: Keyword extraction (maximum entropy): Click ""Keyword Extraction (Maximum Entropy)"" button and follow the maximum entropy method to extract keywords.", "Help")
	End Sub

	Private Sub TextRichTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RichTextBoxText.KeyPress
		'e.Handled = True
	End Sub

	Private Sub ResultListView_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ResultListView.ColumnClick
		' Determine if clicked column is already the column that is being sorted.
		If e.Column = lvwColumnSorter.SortColumn Then
			' Reverse the current sort direction for this column.
			If lvwColumnSorter.Order = SortOrder.Ascending Then
				lvwColumnSorter.Order = SortOrder.Descending
			Else
				lvwColumnSorter.Order = SortOrder.Ascending
			End If
		Else
			' Set the column number that is to be sorted; default to ascending.
			lvwColumnSorter.SortColumn = e.Column
			lvwColumnSorter.Order = SortOrder.Ascending
		End If

		' Perform the sort with these new sort options.
		Me.ResultListView.Sort()
	End Sub

	Private Sub KeywordExtractionNormalButton_Click(sender As Object, e As EventArgs) Handles KeywordExtractionNormalButton.Click

		Dim thread As New Thread(New ThreadStart(AddressOf DoWork_Normal))
		thread.IsBackground = True
		thread.Start()

	End Sub

	Private Sub SelectFileButton_Click(sender As Object, e As EventArgs) Handles SelectFileButton.Click

		Dim ofd As New OpenFileDialog()

		Try
			ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
			If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				PathTextBox.Text = ofd.FileName
				Dim encode As Encoding = Encoding.GetEncoding("GB2312")
				MyData.TheDoc = File.ReadAllText(ofd.FileName, encode)
				RichTextBoxText.Text = MyData.TheDoc

				ResultListView.Items.Clear()
				StandardizationButton.Enabled = True
				RemoveStopButton.Enabled = True
				KeywordExtractionMaxButton.Enabled = True
				KeywordExtractionNormalButton.Enabled = True
				StatusLabel.Text = ofd.FileName & " file open finish."

				Call Me.HighlightText()

			End If

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Public Sub HighlightText(ByVal myRtb As RichTextBox, ByVal word As String, ByVal color As Color)

		If word = String.Empty Then Return
		Dim index As Integer = 0
		Dim startIndex As Integer = 0

		Try
			Dim lsAllText As String = LCase(myRtb.Text)
			While index < myRtb.TextLength

				index = lsAllText.IndexOf(LCase(word), startIndex)
				If index >= 0 And index < myRtb.TextLength - word.Length Then
					myRtb.[Select](index, word.Length)
					myRtb.SelectionColor = color
					startIndex = index + word.Length
				Else
					Exit While
				End If

			End While

			startIndex = 0
			lsAllText = myRtb.Text
			While index < myRtb.TextLength
				index = lsAllText.IndexOf(word, startIndex)
				If index >= 0 And index < myRtb.TextLength - word.Length Then
					myRtb.[Select](index, word.Length)
					myRtb.SelectionColor = color
					startIndex = index + word.Length
				Else
					Exit While
				End If

			End While

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub


	Private Sub ResultListView_MouseDown(sender As Object, e As MouseEventArgs) Handles ResultListView.MouseDown

		Try
			If e.Button = MouseButtons.Right Then
				Me.ResultListView.ContextMenuStrip = Me.ContextMenuStripKeyword
			End If
		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItemAddAsValueType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddAsValueType.Click

		Try
			With New WaitCursor
				If Me.zrSelectedModelElement IsNot Nothing Then
					MsgBox(Me.zrSelectedModelElement.Id & " is already within the Model.")
					Exit Sub
				End If

				Dim liIndex As Integer = Me.ResultListView.SelectedItems(0).Index
				Dim lsValueTypeName As String = Me.ResultListView.SelectedItems(0).SubItems(1).Text
				lsValueTypeName = Viev.Strings.MakeCapCamelCase(lsValueTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrValueType As FBM.ValueType
				lrValueType = Me.mrModel.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision, True)

				MyData.WordsFre(liIndex).IsInModel = True
				Dim loFont = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
				Me.ResultListView.SelectedItems(0).Font = loFont
				Me.ResultListView.SelectedItems(0).ForeColor = Color.RoyalBlue

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrValueType)
				End If
			End With


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItemViewInORMVerbaliser_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemViewInORMVerbaliser.Click

		Try
			With New WaitCursor
				If Me.zrSelectedModelElement Is Nothing Then
					MsgBox("The selected keyword is not in the Model.")
					Exit Sub
				End If

				Dim lrModelElement = Me.zrSelectedModelElement

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrModelElement)
				End If
			End With


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub TextRichTextBox_MouseDown(sender As Object, e As MouseEventArgs) Handles RichTextBoxText.MouseDown

		Try
			If Me.RichTextBoxText.SelectionLength > 0 Then
				Me.RichTextBoxText.ContextMenuStrip = Me.ContextMenuStripTextboxSelection
			Else
				Me.RichTextBoxText.ContextMenuStrip = Me.ContextMenuStripTextbox
			End If


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub FindToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindToolStripMenuItem.Click

		Try
			If mfrmFindDialog Is Nothing Then mfrmFindDialog = New frmTextboxFind()
			mfrmFindDialog.mRichTextBox = Me.RichTextBoxText
			Me.RichTextBoxText.HideSelection = False
			mfrmFindDialog.ShowDialog()
			Me.RichTextBoxText.Focus()
		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItemAddAsEntityType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemAddAsEntityType.Click

		Try
			With New WaitCursor
				If Me.zrSelectedModelElement IsNot Nothing Then
					MsgBox(Me.zrSelectedModelElement.Id & " is already within the Model.")
					Exit Sub
				End If

				Dim liIndex As Integer = Me.ResultListView.SelectedItems(0).Index
				Dim lsEntityTypeName As String = Me.ResultListView.SelectedItems(0).SubItems(1).Text
				lsEntityTypeName = Viev.Strings.MakeCapCamelCase(lsEntityTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrEntityType As FBM.EntityType
				lrEntityType = Me.mrModel.CreateEntityType(lsEntityTypeName, True, True)

				If My.Settings.UseDefaultReferenceModeNewEntityTypes Then

					Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode, False, Nothing, True, liDataType, False, False)

					If lrEntityType.getDataType = pcenumORMDataType.DataTypeNotSet Then
						Call lrEntityType.ReferenceModeValueType.SetDataType(liDataType)
					End If

					Call lrEntityType.ReferenceModeValueType.SetDataTypeLength(liDataTypeLength)
					Call lrEntityType.ReferenceModeValueType.SetDataTypePrecision(liDataTypePrecision)

				End If

				MyData.WordsFre(liIndex).IsInModel = True
				Dim loFont = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
				Me.ResultListView.SelectedItems(0).Font = loFont
				Me.ResultListView.SelectedItems(0).ForeColor = Color.RoyalBlue

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrEntityType)
				End If
			End With


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItemSelectionAddEntity_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSelectionAddEntity.Click

		Dim lsMessage As String

		Try
			If Me.RichTextBoxText.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxText.SelectedText)

				If Me.mrModel.GetModelObjectByName(lsModelElementName, True, False) IsNot Nothing Then
					lsMessage = lsModelElementName & " is already in the model."
					MsgBox(lsMessage)
					Exit Sub
				End If

				Dim lsEntityTypeName As String = Trim(Me.RichTextBoxText.SelectedText)
				lsEntityTypeName = Viev.Strings.MakeCapCamelCase(lsEntityTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrEntityType As FBM.EntityType
				lrEntityType = Me.mrModel.CreateEntityType(lsEntityTypeName, True, True)

				If My.Settings.UseDefaultReferenceModeNewEntityTypes Then

					Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode, False, Nothing, True, liDataType, False, False)

					If lrEntityType.getDataType = pcenumORMDataType.DataTypeNotSet Then
						Call lrEntityType.ReferenceModeValueType.SetDataType(liDataType)
					End If

					Call lrEntityType.ReferenceModeValueType.SetDataTypeLength(liDataTypeLength)
					Call lrEntityType.ReferenceModeValueType.SetDataTypePrecision(liDataTypePrecision)

				End If

				Me.RichTextBoxText.SelectionColor = Color.RoyalBlue

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrEntityType)
				End If


			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ToolStripMenuItemSelectionAddValueType_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSelectionAddValueType.Click

		Dim lsMessage As String

		Try
			If Me.RichTextBoxText.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxText.SelectedText)

				If Me.mrModel.GetModelObjectByName(lsModelElementName, True, False) IsNot Nothing Then
					lsMessage = lsModelElementName & " is already in the model."
					MsgBox(lsMessage)
					Exit Sub
				End If



				Dim lsValueTypeName As String = Trim(Me.RichTextBoxText.SelectedText)
				lsValueTypeName = Viev.Strings.MakeCapCamelCase(lsValueTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrValueType As FBM.ValueType
				lrValueType = Me.mrModel.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision, True)

				Me.RichTextBoxText.SelectionColor = Color.DarkSeaGreen

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrValueType)
				End If

			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub AsGeneralConceptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsGeneralConceptToolStripMenuItem.Click

		Dim lsMessage As String
		Try

			If Me.RichTextBoxText.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxText.SelectedText)

				Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.mrModel, lsModelElementName, pcenumConceptType.GeneralConcept)

				If Me.mrModel.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then

					lrDictionaryEntry = Me.mrModel.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

					If lrDictionaryEntry.isGeneralConcept Then
						MsgBox(lsModelElementName & " is already a General Concept within the Model.")
						Exit Sub
					Else
						lrDictionaryEntry.AddConceptType(pcenumConceptType.GeneralConcept)
						Me.mrModel.MakeDirty(False, False)
					End If
				Else
					lrDictionaryEntry = Me.mrModel.AddModelDictionaryEntry(lrDictionaryEntry, False, False,,,, True)
				End If

				Me.RichTextBoxText.SelectionColor = Color.DarkOrange

			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub SaveAsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsToolStripMenuItem.Click

		Try
			Dim sfd As New SaveFileDialog()
			sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
			If sfd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

				System.IO.File.WriteAllText(sfd.FileName, Me.RichTextBoxText.Text)

			End If


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub mrModel_ModelUpdated(ByRef arModelElement As FBM.ModelObject) Handles mrModel.ModelUpdated

		Try
			If arModelElement IsNot Nothing Then
				Select Case arModelElement.GetType
					Case Is = GetType(FBM.ValueType)

						Dim lrValueType As FBM.ValueType = arModelElement

						Call Me.HighlightText(Me.RichTextBoxText, lrValueType.Id, Color.DarkGreen)

						Dim lasValueConstraint = From ValueConstraint In lrValueType.ValueConstraint
												 Select ValueConstraint

						For Each lsValueConstraint In lasValueConstraint
							Call Me.HighlightText(Me.RichTextBoxText, lsValueConstraint, Color.Maroon)
						Next

				End Select
			End If

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Public Function ExtractFactTypeReadings(text As String) As List(Of String)

		Try
			Dim lasKeywords As New List(Of String)


			MyData.WordsFre = {}

			' Create a new StanfordCoreNLP pipeline with the required annotators
			Dim pipelineProps As New java.util.Properties()
			pipelineProps.setProperty("annotators", "tokenize, ssplit, pos, lemma, depparse")
			pipelineProps.setProperty("pos.model", "CoreNLP\models\english-caseless-left3words-distsim.tagger") ' Specify the path to the tagger properties file
			pipelineProps.setProperty("depparse.model", "CoreNLP\models\parser\nndep\english_UD.gz")


			Dim pipeline As New StanfordCoreNLP(pipelineProps)

			' Create an Annotation object with the input text
			Dim annotation As New Annotation(text)

			' Process the annotation through the pipeline
			pipeline.annotate(annotation)

			' Get the sentences from the annotation
			Dim sentences As java.util.ArrayList = CType(annotation.get(GetType(CoreAnnotations.SentencesAnnotation)), java.util.ArrayList)

			Dim lasFactTypeReading As New List(Of String)()

			'================Noun Phrases revised=====================================
			For Each sentence As CoreMap In sentences

				With New WaitCursor
					Dim tokens As java.util.ArrayList = sentence.get(GetType(CoreAnnotations.TokensAnnotation))
					Dim numTokens As Integer = tokens.size()

					Dim nounPhrase As StringBuilder = Nothing ' Variable to store the current noun phrase
					Dim subject As String = Nothing ' Variable to store the subject
					Dim verb As String = Nothing ' Variable to store the verb
					Dim [object] As String = Nothing ' Variable to store the object

					For i As Integer = 0 To numTokens - 1
						Dim token As CoreLabel = CType(tokens.get(i), CoreLabel)
						Dim pos As String = token.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

						If pos.StartsWith("NN") Then
							If nounPhrase Is Nothing Then
								nounPhrase = New StringBuilder()
							End If

							nounPhrase.Append(token.get(GetType(CoreAnnotations.TextAnnotation))).Append(" ")

							' Check if the next token is a verb
							If i + 1 < numTokens Then
								Dim nextToken As CoreLabel = CType(tokens.get(i + 1), CoreLabel)
								Dim nextPos As String = nextToken.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

								If nextPos.StartsWith("VB") Then
									verb = nextToken.get(GetType(CoreAnnotations.TextAnnotation)).ToString()
								End If
							End If
						ElseIf nounPhrase IsNot Nothing AndAlso Not String.IsNullOrEmpty(nounPhrase.ToString()) Then
							' A noun phrase has been captured, save it as the subject
							subject = nounPhrase.ToString().Trim()
							nounPhrase = Nothing ' Reset nounPhrase for the next noun phrase

							' Find the object related to the subject
							Dim objectNounPhrase As StringBuilder = Nothing ' Variable to store the current object noun phrase

							For j As Integer = i To numTokens - 1
								Dim nextToken As CoreLabel = CType(tokens.get(j), CoreLabel)
								Dim nextPos As String = nextToken.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

								If nextPos.StartsWith("NN") Then
									If objectNounPhrase Is Nothing Then
										objectNounPhrase = New StringBuilder()
									End If

									objectNounPhrase.Append(nextToken.get(GetType(CoreAnnotations.TextAnnotation))).Append(" ")
								ElseIf objectNounPhrase IsNot Nothing AndAlso Not String.IsNullOrEmpty(objectNounPhrase.ToString()) Then
									' A noun phrase has been captured, save it as the object
									[object] = objectNounPhrase.ToString().Trim()
									Exit For
								End If
							Next

							subject = Singularize(subject)
							[object] = Singularize([object])
							verb = ConjugateVerbWN(verb, subject)

							'CodeSafe
							If verb Is Nothing Then verb = "has"

							Dim lsSubjectKeyword As String = ""
							Dim lsObjectKeyword As String = ""

							lsSubjectKeyword = subject.ToPascalCase

							If Not String.IsNullOrEmpty([object]) Then
								' Add the SVO triple with noun phrase object
								lsObjectKeyword = [object].ToPascalCase
								lasFactTypeReading.Add(subject.ToPascalCase & " " & verb & " " & lsObjectKeyword)

							Else
								' No noun phrase found for the object, fallback to the singular word noun
								lsObjectKeyword = tokens.get(i).get(GetType(CoreAnnotations.TextAnnotation)).ToString().ToPascalCase
								lasFactTypeReading.Add(subject.ToPascalCase & " " & verb & " " & lsObjectKeyword)
							End If


#Region "Add Keyword to list"
							If Not lasKeywords.Contains(lsSubjectKeyword) Then

								Dim lvi As New ListViewItem()
								lvi.SubItems(0).Text = lsSubjectKeyword
								lvi.SubItems.Add(lsSubjectKeyword)

								Dim loWordFrequency As New WORDSFRE
								loWordFrequency.Position = {1}
								loWordFrequency.Distance = {1}
								loWordFrequency.Word = lsSubjectKeyword
								loWordFrequency.Frequency = 1
								loWordFrequency.Position(0) = 1
								MyData.WordsFre.Add(loWordFrequency)

								MyData.WordsFre.Add(New WORDSFRE())
								If Me.mrModel.GetModelObjectByName(lsSubjectKeyword, True) IsNot Nothing Then
									loWordFrequency.IsInModel = True
									Dim loFont = New Font("Arial", 10, FontStyle.Bold)
									lvi.SubItems(1).Font = loFont
									lvi.ForeColor = Color.RoyalBlue
									'lvi(i).BackColor = Color.Beige
								End If
								lvi.SubItems.Add(0.ToString)
								lvi.SubItems.Add((lasKeywords.FindAll(Function(x) x = subject.ToPascalCase).Count + 1).ToString)
								ResultListView.Items.Add(lvi)

							End If

							lasKeywords.Add(lsSubjectKeyword)

							If Not lasKeywords.Contains(lsObjectKeyword) Then

								Dim lvi As New ListViewItem()
								lvi.SubItems(0).Text = lsObjectKeyword
								lvi.SubItems.Add(lsObjectKeyword)

								Dim loWordFrequency As New WORDSFRE
								loWordFrequency.Position = {1}
								loWordFrequency.Distance = {1}
								loWordFrequency.Word = lsSubjectKeyword
								loWordFrequency.Frequency = 1
								loWordFrequency.Position(0) = 1
								MyData.WordsFre.Add(loWordFrequency)

								If Me.mrModel.GetModelObjectByName(lsObjectKeyword, True) IsNot Nothing Then
									loWordFrequency.IsInModel = True
									Dim loFont = New Font("Arial", 10, FontStyle.Bold)
									lvi.SubItems(1).Font = loFont
									lvi.ForeColor = Color.RoyalBlue
									'lvi(i).BackColor = Color.Beige
								End If
								lvi.SubItems.Add(0.ToString)
								lvi.SubItems.Add((lasKeywords.FindAll(Function(x) x = lsObjectKeyword).Count - 1).ToString)
								ResultListView.Items.Add(lvi)
							End If

							lasKeywords.Add(lsObjectKeyword)
#End Region
						End If

					Next
				End With
			Next            '=========================================



			'============Noun Phrases===============================
			For Each sentence As CoreMap In sentences
				Dim tokens As java.util.ArrayList = sentence.get(GetType(CoreAnnotations.TokensAnnotation))
				Dim numTokens As Integer = tokens.size()

				For i As Integer = 0 To numTokens - 1
					Dim token As CoreLabel = CType(tokens.get(i), CoreLabel)
					Dim pos As String = token.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

					' Check if the token is a noun
					If pos.StartsWith("NN") Then
						Dim nounPhrase As String = GetNounPhrase(tokens, i)
						Dim subject As String = nounPhrase
						Dim verb As String = Nothing
						Dim [object] As String = Nothing

						' Find the verb and object related to the subject
						For j As Integer = i + 1 To numTokens - 1
							Dim nextToken As CoreLabel = CType(tokens.get(j), CoreLabel)
							Dim nextPos As String = nextToken.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

							' Check if the next token is a verb
							If nextPos.StartsWith("VB") Then
								verb = nextToken.get(GetType(CoreAnnotations.TextAnnotation)).ToString()
							End If

							' Check if the next token is a noun
							If nextPos.StartsWith("NN") Then
								nounPhrase = GetNounPhrase(tokens, j)
								[object] = nounPhrase
								Exit For
							End If
						Next

						' Add the SVO triple if both verb and object are present
						If Not String.IsNullOrEmpty(verb) AndAlso Not String.IsNullOrEmpty([object]) Then
							lasFactTypeReading.Add(subject & " - " & verb & " - " & [object])
						End If
					End If
				Next
			Next
			'=============================================================================



			For Each sentence As CoreMap In sentences
				Dim tokens As java.util.ArrayList = sentence.get(GetType(CoreAnnotations.TokensAnnotation))
				Dim numTokens As Integer = tokens.size()

				For i As Integer = 0 To numTokens - 1
					Dim token As CoreLabel = CType(tokens.get(i), CoreLabel)
					Dim pos As String = token.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

					' Check if the token is a noun
					If pos.StartsWith("NN") Then
						Dim subject As String = token.get(GetType(CoreAnnotations.TextAnnotation)).ToString()
						Dim verb As String = Nothing
						Dim [object] As String = Nothing

						' Find the verb and object related to the subject
						For j As Integer = i + 1 To numTokens - 1
							Dim nextToken As CoreLabel = CType(tokens.get(j), CoreLabel)
							Dim nextPos As String = nextToken.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

							' Check if the next token is a verb
							If nextPos.StartsWith("VB") Then
								verb = nextToken.get(GetType(CoreAnnotations.TextAnnotation)).ToString()
							End If

							' Check if the next token is a noun
							If nextPos.StartsWith("NN") Then
								[object] = nextToken.get(GetType(CoreAnnotations.TextAnnotation)).ToString()
								Exit For
							End If
						Next

						' Add the SVO triple if both verb and object are present
						If Not String.IsNullOrEmpty(verb) AndAlso Not String.IsNullOrEmpty([object]) Then
							lasFactTypeReading.Add(subject & " - " & verb & " - " & [object])
						End If
					End If
				Next
			Next


			' Extract Fact Type Readings from each sentence			
			For Each sentence As CoreMap In sentences
				' Get the GrammaticalStructure from the sentence
				Dim gs As GrammaticalStructure = CType(sentence.get(GetType(edu.stanford.nlp.trees.TypedDependency)), GrammaticalStructure)

				Dim dependencies As SemanticGraph = sentence.get(GetType(SemanticGraphCoreAnnotations.BasicDependenciesAnnotation))
				Dim typedDependencies As java.util.ArrayList = CType(dependencies.typedDependencies(), java.util.ArrayList)

				' Create a map to store the named entities
				Dim namedEntities As New Dictionary(Of String, String)()


				For Each dependency As TypedDependency In typedDependencies
					Dim governor As String = dependency.gov().originalText()
					Dim dependent As String = dependency.dep().originalText()

					' Check if the governor is a noun and the dependent is a verb
					Dim governorPOS As String = dependency.gov().get(GetType(PartOfSpeechAnnotation))
					Dim dependentPOS As String = dependency.dep().get(GetType(PartOfSpeechAnnotation))
					If governorPOS IsNot Nothing AndAlso dependentPOS IsNot Nothing AndAlso governorPOS.StartsWith("NN") AndAlso dependentPOS.StartsWith("VB") Then
						Dim noun As String = FindNoun(dependency.gov().index(), typedDependencies)
						If noun IsNot Nothing Then
							Dim factTypeReading As String = $"{noun} {dependent} {governor}"
							lasFactTypeReading.Add(factTypeReading)
						End If
					End If
				Next
NextSentence:
			Next

			Return lasFactTypeReading

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

			Return New List(Of String)
		End Try
	End Function


	' Function to get the noun phrase by combining consecutive tokens with the same named entity tag
	Function GetNounPhrase(tokens As java.util.ArrayList, startIndex As Integer) As String

		Try
			Dim nounPhrase As New StringBuilder()
			Dim numTokens As Integer = tokens.size()
			Dim foundNounPhrase As Boolean = False

			For i As Integer = startIndex To numTokens - 1
				Dim token As CoreLabel = CType(tokens.get(i), CoreLabel)
				Dim pos As String = token.get(GetType(CoreAnnotations.PartOfSpeechAnnotation)).ToString()

				If token.get(GetType(CoreAnnotations.NamedEntityTagAnnotation)) IsNot Nothing Then
					Dim namedEntityTag As String = token.get(GetType(CoreAnnotations.NamedEntityTagAnnotation)).ToString()

					If pos.StartsWith("NN") AndAlso namedEntityTag <> "O" Then
						nounPhrase.Append(token.get(GetType(CoreAnnotations.TextAnnotation)).ToString()).Append(" ")
						foundNounPhrase = True
					ElseIf pos.StartsWith("NN") Then
						nounPhrase.Append(token.get(GetType(CoreAnnotations.TextAnnotation)).ToString()).Append(" ")
					Else
						Exit For
					End If
				Else
					' Handle case when NamedEntityTagAnnotation is null
					If pos.StartsWith("NN") Then
						nounPhrase.Append(token.get(GetType(CoreAnnotations.TextAnnotation)).ToString()).Append(" ")
						foundNounPhrase = True
					Else
						Exit For
					End If
				End If
			Next

			' If no noun phrase is found, use the singular word noun
			If Not foundNounPhrase AndAlso nounPhrase.Length > 0 Then
				nounPhrase.Length -= 1 ' Remove the trailing space
			End If

			Return nounPhrase.ToString().Trim()

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

			Return "Error"
		End Try
	End Function

#Region "Old ExtractFTRs"
	'	Public Function ExtractFactTypeReadingsOld(text As String) As List(Of String)

	'		Try
	'			' Create a new StanfordCoreNLP pipeline with the required annotators
	'			Dim pipelineProps As New java.util.Properties()
	'			pipelineProps.setProperty("annotators", "tokenize, ssplit, pos, lemma, depparse")
	'			pipelineProps.setProperty("pos.model", "CoreNLP\models\english-caseless-left3words-distsim.tagger") ' Specify the path to the tagger properties file
	'			pipelineProps.setProperty("depparse.model", "CoreNLP\models\parser\nndep\english_UD.gz")


	'			Dim pipeline As New StanfordCoreNLP(pipelineProps)

	'			' Create an Annotation object with the input text
	'			Dim annotation As New Annotation(text)

	'			' Process the annotation through the pipeline
	'			pipeline.annotate(annotation)

	'			' Get the sentences from the annotation
	'			Dim sentences As java.util.ArrayList = CType(annotation.get(GetType(CoreAnnotations.SentencesAnnotation)), java.util.ArrayList)

	'			' Extract Fact Type Readings from each sentence
	'			Dim factTypeReadings As New List(Of String)()
	'			For Each sentence As CoreMap In sentences
	'				' Get the GrammaticalStructure from the sentence
	'				Dim gs As GrammaticalStructure = CType(sentence.get(GetType(edu.stanford.nlp.trees.TypedDependency)), GrammaticalStructure)

	'				Dim dependencies As SemanticGraph = sentence.get(GetType(SemanticGraphCoreAnnotations.BasicDependenciesAnnotation))
	'				Dim typedDependencies As java.util.ArrayList = CType(dependencies.typedDependencies(), java.util.ArrayList)

	'				' Create a map to store the named entities
	'				Dim namedEntities As New Dictionary(Of String, String)()


	'				For Each dependency As TypedDependency In typedDependencies
	'					Dim governor As String = dependency.gov().originalText()
	'					Dim dependent As String = dependency.dep().originalText()

	'					' Check if the governor is a noun and the dependent is a verb
	'					Dim governorPOS As String = dependency.gov().get(GetType(PartOfSpeechAnnotation))
	'					Dim dependentPOS As String = dependency.dep().get(GetType(PartOfSpeechAnnotation))
	'					If governorPOS IsNot Nothing AndAlso dependentPOS IsNot Nothing AndAlso governorPOS.StartsWith("NN") AndAlso dependentPOS.StartsWith("VB") Then
	'						Dim noun As String = FindNoun(dependency.gov().index(), typedDependencies)
	'						If noun IsNot Nothing Then
	'							Dim factTypeReading As String = $"{noun} {dependent} {governor}"
	'							factTypeReadings.Add(factTypeReading)
	'						End If
	'					End If
	'				Next

	'				'For Each dependency As TypedDependency In typedDependencies
	'				'	Dim relation As String = dependency.reln().getShortName()
	'				'	Dim governor As String = dependency.gov().originalText()
	'				'	Dim dependent As String = dependency.dep().originalText()

	'				'	Dim governorPOS As String = dependency.gov().get(GetType(PartOfSpeechAnnotation))
	'				'	Dim dependentPOS As String = dependency.dep().get(GetType(PartOfSpeechAnnotation))

	'				'	If governorPOS IsNot Nothing AndAlso dependentPOS IsNot Nothing AndAlso governorPOS.StartsWith("NN") AndAlso dependentPOS.StartsWith("VB") Then
	'				'		Dim factTypeReading As String = $"{governor} {dependent}"
	'				'		factTypeReadings.Add(factTypeReading)
	'				'	End If


	'				'	'' Replace named entities with their types
	'				'	'If dependency.dep().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "PRP" Then
	'				'	'	Dim namedEntity As String = ""
	'				'	'	If namedEntities.ContainsKey(governor) Then
	'				'	'		namedEntity = namedEntities(governor)
	'				'	'	End If
	'				'	'	If namedEntity IsNot Nothing Then
	'				'	'		dependent = namedEntity
	'				'	'	End If
	'				'	'ElseIf dependency.gov().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "PRP" Then
	'				'	'	Dim namedEntity As String = ""
	'				'	'	If namedEntities.ContainsKey(dependent) Then
	'				'	'		namedEntity = namedEntities(dependent)
	'				'	'	End If
	'				'	'	If namedEntity IsNot Nothing Then
	'				'	'		governor = namedEntity
	'				'	'	End If
	'				'	'End If

	'				'	'' Store named entities
	'				'	'If dependency.dep().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "NNP" Then
	'				'	'	namedEntities(dependent) = governor
	'				'	'End If

	'				'	'' Extract binary, ternary, and n-ary relations
	'				'	'Select Case relation
	'				'	'	Case "nsubj", "nsubjpass", "dobj", "iobj", "pobj", "amod"
	'				'	'		Dim factTypeReading As String = $"{dependent} {relation} {governor}"
	'				'	'		factTypeReadings.Add(factTypeReading)
	'				'	'End Select
	'				'Next


	'				'' Process the dependencies to extract Fact Type Readings
	'				'For Each dependency As TypedDependency In typedDependencies

	'				'	Dim relation As String = dependency.reln().getShortName
	'				'	Dim governor As String = dependency.gov().originalText()
	'				'		Dim dependent As String = dependency.dep().originalText()

	'				'		' Replace named entities with their types
	'				'		If dependency.dep().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "PRP" Then
	'				'		Dim namedEntity As String = Nothing
	'				'		If namedEntities.ContainsKey(governor) Then
	'				'			namedEntity = namedEntities(governor)
	'				'		End If
	'				'		If namedEntity IsNot Nothing Then
	'				'			dependent = namedEntity
	'				'		End If
	'				'	ElseIf dependency.gov().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "PRP" Then
	'				'		Dim namedEntity As String = Nothing
	'				'		If namedEntities.ContainsKey(dependent) Then
	'				'			namedEntity = namedEntities(dependent)
	'				'		End If
	'				'		If namedEntity IsNot Nothing Then
	'				'			governor = namedEntity
	'				'		End If
	'				'	End If

	'				'		' Store named entities
	'				'		If dependency.dep().get(GetType(CoreAnnotations.PartOfSpeechAnnotation)) = "NNP" Then
	'				'			namedEntities(dependent) = governor
	'				'		End If

	'				'		' Extract binary, ternary, and n-ary relations
	'				'		If dependency.reln().getShortName() = "dep" Then
	'				'			Dim factTypeReading As String = $"{dependent} {relation} {governor}"
	'				'			factTypeReadings.Add(factTypeReading)
	'				'		ElseIf dependency.reln().getShortName() = "nsubjpass" Then
	'				'			Dim factTypeReading As String = $"{dependent} {relation} {governor}"
	'				'			factTypeReadings.Add(factTypeReading)
	'				'		End If
	'				'	Next
	'NextSentence:
	'			Next

	'			Return factTypeReadings

	'		Catch ex As Exception
	'			Dim lsMessage As String
	'			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

	'			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
	'			lsMessage &= vbCrLf & vbCrLf & ex.Message
	'			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

	'			Return New List(Of String)
	'		End Try
	'	End Function
#End Region
	Private Function FindNoun(governorIndex As Integer, dependencies As java.util.ArrayList) As String
		For Each dependency As TypedDependency In dependencies
			If dependency.reln().getShortName() = "nsubj" OrElse dependency.reln().getShortName() = "nsubjpass" Then
				If dependency.gov().index() = governorIndex Then
					Return dependency.dep().originalText()
				End If
			End If
		Next
		Return Nothing
	End Function

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonExtractFactTypeReadings.Click

		With New WaitCursor

			Me.TabPageResults.Show()
			Me.TabControl1.SelectedTab = Me.TabPageResults
			Me.TabControl1.SelectedTab.Refresh()
			Me.TabControl1.Refresh()

			With New WaitCursor

				Dim lasFactTypeReading = Me.ExtractFactTypeReadings(Me.RichTextBoxText.Text)

				Me.RichTextBoxResults.Text = String.Join(Environment.NewLine, lasFactTypeReading)
				Me.TabPageResults.Show()
			End With

		End With

	End Sub

	Function Singularize(ByVal asWord As String) As String

		Try
			Dim pipelineProps As New java.util.Properties()
			pipelineProps.setProperty("annotators", "tokenize, ssplit, pos, lemma, depparse")
			pipelineProps.setProperty("pos.model", "CoreNLP\models\english-caseless-left3words-distsim.tagger") ' Specify the path to the tagger properties file
			pipelineProps.setProperty("depparse.model", "CoreNLP\models\parser\nndep\english_UD.gz")

			Dim pipeline As New StanfordCoreNLP(pipelineProps)
			Dim document As New Annotation(asWord)
			pipeline.annotate(document)

			Dim sentences As java.util.ArrayList = CType(document.get(GetType(CoreAnnotations.SentencesAnnotation)), java.util.ArrayList)

			If sentences IsNot Nothing AndAlso Not sentences.isEmpty Then
				Dim tokens As java.util.ArrayList = sentences(0).get(GetType(TokensAnnotation))

				If tokens IsNot Nothing AndAlso Not tokens.isEmpty Then
					Dim singularWords As New List(Of String)()

					For Each token As CoreLabel In tokens
						Dim word As String = token.get(GetType(TextAnnotation))
						Dim lemma As String = token.get(GetType(LemmaAnnotation))

						If word IsNot Nothing AndAlso lemma IsNot Nothing Then
							If word.Trim().Length > 0 AndAlso lemma.Trim().Length > 0 Then
								Dim singularWord As String = lemma.ToLower()
								singularWords.Add(singularWord)
							End If
						End If
					Next

					Dim singularizedText As String = String.Join(" ", singularWords)
					Return singularizedText
				End If
			End If

			Return asWord

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True)

			Return asWord
		End Try
	End Function

	Function ConjugateVerb(ByVal asVerb As String, ByVal subject As String) As String

		Try

			If String.IsNullOrEmpty(asVerb) Then Return asVerb

			Dim normalizedSubject As String = subject.ToLower()
			Dim normalizedVerb As String = asVerb.ToLower()

			' Check for common subject-verb agreement patterns
			If normalizedSubject.EndsWith("s") AndAlso Not normalizedVerb.EndsWith("s") Then
				Return asVerb & "s" ' Third-person singular form
			ElseIf normalizedVerb = "have" AndAlso normalizedSubject.EndsWith("s") Then
				Return "has" ' "have" to "has" in third-person singular
			End If

			' Add more rules for subject-verb agreement here if needed

			Return asVerb ' Default case, return the original verb

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True)

			Return asVerb
		End Try

	End Function


	Function ConjugateVerbWN(ByVal asVerb As String, ByVal asSubject As String) As String

		Try
			'CodeSafe
			If asVerb Is Nothing Then Return asVerb

			Dim loPOS = New PartOfSpeech

			Dim synSets As IEnumerable(Of SynSet) = wordNet.GetSynSets(asVerb, PartOfSpeech.Verb)

			If synSets IsNot Nothing AndAlso synSets.Any() Then
				Dim normalizedSubject As String = asSubject.ToLower()

				' Check for irregular verbs
				Dim irregularVerbs As New Dictionary(Of String, String)()
				irregularVerbs.Add("have", "has")
				irregularVerbs.Add("be", "is")

				If irregularVerbs.ContainsKey(asVerb) Then
					Return irregularVerbs(asVerb)
				End If

				For Each synSet In synSets
					For Each wordForm In synSet.Words
						If wordForm = asVerb AndAlso normalizedSubject.EndsWith("s") Then
							Return asVerb
						End If
					Next
				Next
			End If

			Return asVerb ' Default case, return the original verb

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True)

			Return asVerb
		End Try

	End Function

	Private Sub ButtonExecuteLLMGenerativeAI_Click(sender As Object, e As EventArgs) Handles ButtonExecuteLLMGenerativeAI.Click

		'CodeSafe
		'If msSingleFilePath Is Nothing Then
		'	MsgBox("Please select a file to search.")
		'	Exit Sub
		'End If
		If Trim(Me.TextBoxAILLMPrompt.Text) = "" Then
			MsgBox("Please enter a LLM Prompt to operate over the document.")
			Exit Sub
		End If
		If Trim(My.Settings.FactEngineOpenAIAPIKey) = "" Then
			prApplication.ThrowErrorMessage("Set the OpenAI API Key in configuration", pcenumErrorType.Warning,, False,, True,, True)
			Exit Sub
		End If

		'Clear the Results textbox.
		Me.RichTextBoxResults.Clear()

		'Reset the Progress Bar
		Me.ProgressBar.Value = 0
		Me.ProgressBar.Visible = True

		'Show the Abort button
		Me.ButtonAbort.Visible = True
		Me.mbAbort = False

		Me.ButtonExecuteLLMGenerativeAI.Enabled = False

		Dim liChunkCounter As Integer = 1

		Try
			Dim lsDocumentText As String = ""

			lsDocumentText = MyData.TheDoc


			If lsDocumentText Is Nothing Then
				lsDocumentText = Me.RichTextBoxText.Text
			End If

			Dim words As String() = lsDocumentText.ToString.Split(" "c)

			Dim chunkSize As Integer = My.Settings.LLMChunkSize
			Dim overlapSize As Integer = 100
			Dim startIndex As Integer = 0

			ToolStripStatusLabel.Text = "Processing search"

			'Loop through each chunk of the current page         
			While startIndex < words.Count
				Dim endIndex As Integer = Math.Min(startIndex + chunkSize - 1, words.Length - 1)
				Dim chunkText As String = String.Join(" ", words, startIndex, endIndex - startIndex + 1)

				'Display the Chunk Number in the status bar.
				Me.ToolStripStatusLabelChunkCount.Text = "Chunk#: " & liChunkCounter

				'Do something with the current 500-word chunk, such as write it to a file or process it in some way             
				Me.mrOpenAIAPI = New OpenAI_API.OpenAIAPI(New OpenAI_API.APIAuthentication(My.Settings.FactEngineOpenAIAPIKey))

				If mbAbort Then Exit While

				Try

					Dim lsPrompt As String = ""

					'lsPrompt = My.Settings.PromptPreText
					lsPrompt &= Trim(Me.TextBoxAILLMPrompt.Text)
					'lsPrompt &= My.Settings.PromptPostText

					'---------------------------------------------------
					'ChatGPT - Tested, but not very good for this task.
					'---------------------------------------------------
					'Dim lrCompletionResult = Me.GetGPT3ChatResult(chunkText & vbCrLf & vbCrLf & lsPrompt)
					'Dim lsGPT3ReturnString = lrCompletionResult.Choices(0).Message.Content
					'---------------------------------------------------

					'=====================================================================================
					'Farm out to OpenAI via the OpenAI API
					Dim lsModifiedPrompt As String = chunkText & vbCrLf & vbCrLf & lsPrompt
					Dim lrCompletionResult = Boston.GetGPT3Result(Me.mrOpenAIAPI, lsModifiedPrompt)
					Dim lsGPT3ReturnString = lrCompletionResult.Completions(0).Text
					'=====================================================================================

					Dim liIndex As Integer
					Try
						liIndex = Math.Max(lsGPT3ReturnString.IndexOf(vbCrLf), lsGPT3ReturnString.Length)
					Catch ex As Exception
						liIndex = lsGPT3ReturnString.Length
					End Try

					Dim loColor As Color = Color.Black

					Dim start As Integer = Me.RichTextBoxResults.TextLength

					If Not (lsGPT3ReturnString.Replace(vbLf, "").Replace(vbCrLf, "").Replace(Chr(24), "") = "I don't know.") And Not lsGPT3ReturnString.Contains("I don't know") Then
						Me.RichTextBoxResults.AppendText(lsGPT3ReturnString.Substring(0, liIndex) & vbCrLf & "==========================" & vbCrLf)
					End If

					Dim li_end As Integer = Me.RichTextBoxResults.TextLength

					' Textbox may transform chars, so (end-start) != text.Length
					Me.RichTextBoxResults.Select(start, li_end - start)
					Me.RichTextBoxResults.SelectionColor = loColor
					Me.RichTextBoxResults.SelectionLength = 0 ' // clear                    

				Catch ex As Exception
					Throw New Exception(ex.Message)
				End Try

				'Move the start index forward by 400 words to create a 100-word overlap with the next chunk             
				startIndex += chunkSize - overlapSize

				If startIndex < words.Count Then
					Me.ProgressBar.Value = (startIndex / words.Count) * 100
				Else
					Me.ProgressBar.Value = 100
				End If

				Application.DoEvents()

				If Me.mbAbort Then
					Me.ProgressBar.Value = 0
					Me.ProgressBar.Visible = False
					Me.ButtonAbort.Visible = False
					Exit Sub
				End If

				liChunkCounter += 1
			End While

			'Hide the ProgressBar
			Me.ProgressBar.Visible = False
			'Hide the Abort button
			Me.ButtonAbort.Visible = False

			Me.ButtonExecuteLLMGenerativeAI.Enabled = True

			ToolStripStatusLabel.Text = "Completed search successfully"


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

			Me.ToolStripStatusLabel.Text = "An error occurred during the search process"
		Finally
			'Hide the ProgressBar
			Me.ProgressBar.Visible = False
			'Hide the Abort button
			Me.ButtonAbort.Visible = False

			Me.ButtonExecuteLLMGenerativeAI.Enabled = True

			'Clear the Chunk Number in the status bar.
			Me.ToolStripStatusLabelChunkCount.Text = ""
		End Try

	End Sub

	Private Sub PlaceInTheVirtualAnalystToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PlaceInTheVirtualAnalystToolStripMenuItem.Click

		Try
			With New WaitCursor

				If Me.RichTextBoxText.SelectionLength > 0 Then

					Dim lsSelectedText As String = Trim(Me.RichTextBoxText.SelectedText)

					'-------------------------------------------------------
					'ORM Verbalisation
					'-------------------------------------------------------
					Dim lrToolboxForm As frmToolboxBrainBox = Nothing
					lrToolboxForm = frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)

					If IsSomething(lrToolboxForm) Then
						lrToolboxForm.TextBoxInput.Text = lsSelectedText
					End If
				End If
			End With

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItem6_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem6.Click

		Try
			With New WaitCursor

				If Me.RichTextBoxResults.SelectionLength > 0 Then

					Dim lsSelectedText As String = Trim(Me.RichTextBoxResults.SelectedText)

					'-------------------------------------------------------
					'ORM Verbalisation
					'-------------------------------------------------------
					Dim lrToolboxForm As frmToolboxBrainBox = Nothing
					lrToolboxForm = frmMain.loadToolboxRichmondBrainBox(Nothing, Me.DockPanel.ActivePane)

					If IsSomething(lrToolboxForm) Then
						lrToolboxForm.TextBoxInput.Text = "NL: " & lsSelectedText
					End If
				End If
			End With

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click

		Dim lsMessage As String

		Try
			If Me.RichTextBoxResults.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxResults.SelectedText)

				If Me.mrModel.GetModelObjectByName(lsModelElementName, True, False) IsNot Nothing Then
					lsMessage = lsModelElementName & " is already in the model."
					MsgBox(lsMessage)
					Exit Sub
				End If

				Dim lsEntityTypeName As String = Trim(Me.RichTextBoxResults.SelectedText)
				lsEntityTypeName = Viev.Strings.MakeCapCamelCase(lsEntityTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrEntityType As FBM.EntityType
				lrEntityType = Me.mrModel.CreateEntityType(lsEntityTypeName, True, True)

				If My.Settings.UseDefaultReferenceModeNewEntityTypes Then

					Call lrEntityType.SetReferenceMode(My.Settings.DefaultReferenceMode, False, Nothing, True, liDataType, False, False)

					If lrEntityType.getDataType = pcenumORMDataType.DataTypeNotSet Then
						Call lrEntityType.ReferenceModeValueType.SetDataType(liDataType)
					End If

					Call lrEntityType.ReferenceModeValueType.SetDataTypeLength(liDataTypeLength)
					Call lrEntityType.ReferenceModeValueType.SetDataTypePrecision(liDataTypePrecision)

				End If

				Me.RichTextBoxResults.SelectionColor = Color.RoyalBlue

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrEntityType)
				End If

				Call Me.HighlightText()

			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click

		Dim lsMessage As String

		Try
			If Me.RichTextBoxResults.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxResults.SelectedText)

				If Me.mrModel.GetModelObjectByName(lsModelElementName, True, False) IsNot Nothing Then
					lsMessage = lsModelElementName & " is already in the model."
					MsgBox(lsMessage)
					Exit Sub
				End If



				Dim lsValueTypeName As String = Trim(Me.RichTextBoxResults.SelectedText)
				lsValueTypeName = Viev.Strings.MakeCapCamelCase(lsValueTypeName)

				Dim liDataType As pcenumORMDataType = pcenumORMDataType.TextFixedLength
				Dim liDataTypeLength As Integer = 50
				Dim liDataTypePrecision As Integer = 0

				Dim lrValueType As FBM.ValueType
				lrValueType = Me.mrModel.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision, True)

				Me.RichTextBoxResults.SelectionColor = Color.DarkSeaGreen

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.mrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.mrModel
					Call lrToolboxForm.verbaliseModelElement(lrValueType)
				End If

			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click

		Dim lsMessage As String
		Try

			If Me.RichTextBoxText.SelectionLength > 0 Then

				Dim lsModelElementName As String = Trim(Me.RichTextBoxText.SelectedText)

				Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.mrModel, lsModelElementName, pcenumConceptType.GeneralConcept)

				If Me.mrModel.ModelDictionary.Exists(AddressOf lrDictionaryEntry.Equals) Then

					lrDictionaryEntry = Me.mrModel.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

					If lrDictionaryEntry.isGeneralConcept Then
						MsgBox(lsModelElementName & " is already a General Concept within the Model.")
						Exit Sub
					Else
						lrDictionaryEntry.AddConceptType(pcenumConceptType.GeneralConcept)
						Me.mrModel.MakeDirty(False, False)
					End If
				Else
					lrDictionaryEntry = Me.mrModel.AddModelDictionaryEntry(lrDictionaryEntry, False, False,,,, True)
				End If

				Me.RichTextBoxText.SelectionColor = Color.DarkOrange

			End If

		Catch ex As Exception
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ButtonAbort_Click(sender As Object, e As EventArgs) Handles ButtonAbort.Click
		Me.mbAbort = True
	End Sub

	Private Sub RichTextBoxResults_MouseDown(sender As Object, e As MouseEventArgs) Handles RichTextBoxResults.MouseDown

		Try
			If Me.RichTextBoxResults.SelectionLength > 0 Then
				Me.RichTextBoxResults.ContextMenuStrip = Me.ContextMenuStripResultsSelection
			Else
				Me.RichTextBoxResults.ContextMenuStrip = Me.ContextMenuStripTextbox
			End If


		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
		End Try

	End Sub

	Private Sub HighlightText()

		Try

#Region "Highlight existing ModelElements"

			For Each lrFactType In Me.mrModel.FactType
				For Each lrFactTypeReading In lrFactType.FactTypeReading
					Dim lsFactTypeReading As String = lrFactTypeReading.GetReadingText
					Call Me.HighlightText(Me.RichTextBoxText, lsFactTypeReading, Color.Purple)
					Call Me.HighlightText(Me.RichTextBoxResults, lsFactTypeReading, Color.Purple)
				Next
			Next

			For Each lrModelElement In Me.mrModel.getModelObjects
				Call Me.HighlightText(Me.RichTextBoxText, lrModelElement.Id, Color.RoyalBlue)
				Call Me.HighlightText(Me.RichTextBoxResults, lrModelElement.Id, Color.RoyalBlue)
			Next

			For Each lrModelElement In Me.mrModel.ValueType
				Call Me.HighlightText(Me.RichTextBoxText, lrModelElement.Id, Color.DarkGreen)
				Call Me.HighlightText(Me.RichTextBoxResults, lrModelElement.Id, Color.DarkGreen)
			Next

			Dim lasValueConstraint = From ValueType In Me.mrModel.ValueType
									 From ValueConstraint In ValueType.ValueConstraint
									 Select ValueConstraint

			For Each lsValueConstraint In lasValueConstraint
				Call Me.HighlightText(Me.RichTextBoxText, lsValueConstraint, Color.Maroon)
				Call Me.HighlightText(Me.RichTextBoxResults, lsValueConstraint, Color.Maroon)
			Next

			For Each lrModelDictionaryEntry In Me.mrModel.ModelDictionary.FindAll(Function(x) x.isGeneralConcept)
				Call Me.HighlightText(Me.RichTextBoxText, lrModelDictionaryEntry.Symbol, Color.DarkOrange)
				Call Me.HighlightText(Me.RichTextBoxResults, lrModelDictionaryEntry.Symbol, Color.DarkOrange)
			Next
#End Region
		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ButtonRefereshResults_Click(sender As Object, e As EventArgs) Handles ButtonRefereshResults.Click

		Try
			Call Me.HighlightText()
		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ComboBoxFEKLGPTPromptType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxFEKLGPTPromptType.SelectedIndexChanged

		Try
			Me.TextBoxAILLMPrompt.Text = Me.ComboBoxFEKLGPTPromptType.SelectedItem.Tag.Prompt

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click

		Try
			Me.Hide()
			Me.Close()
			Me.Dispose()

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub RichTextBoxText_TextChanged(sender As Object, e As EventArgs) Handles RichTextBoxText.TextChanged

		Try
			If Clipboard.ContainsText() Then
				Dim clipboardText As String = Clipboard.GetText()
				Dim richTextBoxText As String = Me.RichTextBoxText.Text

				' Trim both clipboard and RichTextBox text before comparison
				clipboardText = clipboardText.Trim()
				richTextBoxText = richTextBoxText.Trim()

				Dim cleanedClipboardText As String = System.Text.RegularExpressions.Regex.Replace(clipboardText, "\s+", "")
				Dim cleanedRichTextBoxText As String = System.Text.RegularExpressions.Regex.Replace(richTextBoxText, "\s+", "")

				' Your handling code

				If cleanedClipboardText = cleanedRichTextBoxText Then
					' Handle the paste event
					' Your code to process the pasted text
					StandardizationButton.Enabled = True
					RemoveStopButton.Enabled = True
					KeywordExtractionMaxButton.Enabled = True
					KeywordExtractionNormalButton.Enabled = True
				End If
			End If

			MyData.TheDoc = Me.RichTextBoxText.Text.Trim

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

End Class