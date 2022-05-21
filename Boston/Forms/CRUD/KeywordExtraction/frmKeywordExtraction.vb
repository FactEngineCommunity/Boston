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

Public Class frmKeywordExtraction

	Private lvwColumnSorter As ListViewColumnSorter
	Public zrModel As FBM.Model
	''' <summary>
	''' Changed with the user clicks on a keyword.
	''' </summary>
	Private zrSelectedModelElement As FBM.ModelObject = Nothing

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


	Private Sub KeywordExtractionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		' Create an instance of a ListView column sorter and assign it to the ListView control.
		lvwColumnSorter = New ListViewColumnSorter()
		Me.ResultListView.ListViewItemSorter = lvwColumnSorter

		System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
		SaveButton.Enabled = False
		StandardizationButton.Enabled = False
		RemoveStopButton.Enabled = False
		KeywordExtractionMaxButton.Enabled = False
		KeywordExtractionNormalButton.Enabled = False
		StatusLabel.Text = "Welcome to keyword extraction software, which is based on the entropy difference."
		MessageBox.Show("The process of document keyword extraction:" & vbLf & vbLf & "Step 1: Open the document: Click ""Open"" button, select the target document. (File should be ""*. Txt "" file)" & vbLf & vbLf & "Step 2: Document standardization: Click ""Document Standardization"" button to remove the document punctuation, line breaks, and other useless symbols, and text replaces lowercase letters. (If the above operations have been completed, you can skip this step.)" & vbLf & vbLf & "Step 3: Stop word removal: Click ""Remove Stop"" button and follow the list of stop words, removing stop words in the document. (This step is optional, remove stop words, it may improve the accuracy of keyword extraction.)" & vbLf & vbLf & "Step 4: Here are two method to extract the keyword, you can choose one of them to finish the Keyword extraction work." & vbLf & "    4.1: Keyword extraction (normal entropy): Click ""Keyword Extraction (Entropy)"" button to extract keywords." & vbLf & "    4.2: Keyword extraction (maximum entropy): Click ""Keyword Extraction (Maximum Entropy)"" button and follow the maximum entropy method to extract keywords.", "Help")
	End Sub


	Private Sub StandardizationButton_Click(sender As Object, e As EventArgs) Handles StandardizationButton.Click
		CloseButton()
		MyData.TheDoc = MyFun.DocStandardization(MyData.TheDoc)
		StatusLabel.Text = "Document standardization process has been completed."
		OpenButton()
	End Sub

	Private Sub RemoveStopButton_Click(sender As Object, e As EventArgs) Handles RemoveStopButton.Click
		CloseButton()
		MessageBox.Show("Tip: If the document is longer, it may take you few minutes, please be patient ...", "Prompt", 0, MessageBoxIcon.Asterisk)
		MyData.TheDoc = MyFun.RemoveStop(MyData.TheDoc)
		StatusLabel.Text = "Remove stop-words has been completed."
		OpenButton()
	End Sub

	Private Sub KeywordExtractionButton_Click(sender As Object, e As EventArgs) Handles KeywordExtractionMaxButton.Click
		Dim thread As New Thread(New ThreadStart(AddressOf DoWork))
		thread.IsBackground = True
		thread.Start()
	End Sub

	Private Sub DoWork()
		SyncLock Me
			CloseButton()

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
				lvi(i).SubItems.Add(MyData.WordsFre(i).ED.ToString())
				lvi(i).SubItems.Add(MyData.WordsFre(i).Frequency.ToString())
			Next
			ResultListView.Items.AddRange(lvi)

			Dim dt2 As DateTime = DateTime.Now
			SaveButton.Enabled = True
			progressBar1.Value = 100

			StatusLabel.Text = "Keyword extraction has been completed. The extraction spend " & (dt2 - dt1).ToString() & "."

			OpenButton()
		End SyncLock
	End Sub

	Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
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
		SaveButton.Enabled = True
		StandardizationButton.Enabled = True
		RemoveStopButton.Enabled = True
		KeywordExtractionMaxButton.Enabled = True
		KeywordExtractionNormalButton.Enabled = True
	End Sub

	Private Sub CloseButton()
		SelectFileButton.Enabled = False
		SaveButton.Enabled = False
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
			TextRichTextBox.Text = MyData.TheDoc
			SaveButton.Enabled = False
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

			Dim lrModelElement = Me.zrModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(word), True)
			If lrModelElement IsNot Nothing Then
				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation
				lrToolboxForm = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)
				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.zrModel
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
    End Sub


	Private Sub DoWork_Normal()
		SyncLock Me
			CloseButton()

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

				If Me.zrModel.GetModelObjectByName(Viev.Strings.MakeCapCamelCase(MyData.WordsFre(i).Word.ToString), True) IsNot Nothing Then
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
			SaveButton.Enabled = True
			progressBar1.Value = 100

			StatusLabel.Text = "Keyword extraction has been completed. The extraction spend " & (dt2 - dt1).ToString() & "."

			OpenButton()
		End SyncLock
	End Sub

	Private Sub HelpButton_Click(sender As Object, e As EventArgs) Handles HelpButton.Click
		MessageBox.Show("The process of document keyword extraction:" & vbLf & vbLf & "Step 1: Open the document: Click ""Open"" button, select the target document. (File should be ""*. Txt "" file)" & vbLf & vbLf & "Step 2: Document standardization: Click ""Document Standardization"" button to remove the document punctuation, line breaks, and other useless symbols, and text replaces lowercase letters. (If the above operations have been completed, you can skip this step.)" & vbLf & vbLf & "Step 3: Stop word removal: Click ""Remove Stop"" button and follow the list of stop words, removing stop words in the document. (This step is optional, remove stop words, it may improve the accuracy of keyword extraction.)" & vbLf & vbLf & "Step 4: Here are two method to extract the keyword, you can choose one of them to finish the Keyword extraction work." & vbLf & "    4.1: Keyword extraction (normal entropy): Click ""Keyword Extraction (Entropy)"" button to extract keywords." & vbLf & "    4.2: Keyword extraction (maximum entropy): Click ""Keyword Extraction (Maximum Entropy)"" button and follow the maximum entropy method to extract keywords.", "Help")
	End Sub

	Private Sub TextRichTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextRichTextBox.KeyPress
		e.Handled = True
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
		ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
		If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			PathTextBox.Text = ofd.FileName
			Dim encode As Encoding = Encoding.GetEncoding("GB2312")
			MyData.TheDoc = File.ReadAllText(ofd.FileName, encode)
			TextRichTextBox.Text = MyData.TheDoc
			SaveButton.Enabled = False
			ResultListView.Items.Clear()
			StandardizationButton.Enabled = True
			RemoveStopButton.Enabled = True
			KeywordExtractionMaxButton.Enabled = True
			KeywordExtractionNormalButton.Enabled = True
			StatusLabel.Text = ofd.FileName & " file open finish."
		End If

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
				lrValueType = Me.zrModel.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision, True)

				MyData.WordsFre(liIndex).IsInModel = True
				Dim loFont = New Font("Microsoft Sans Serif", 8.25, FontStyle.Bold)
				Me.ResultListView.SelectedItems(0).Font = loFont
				Me.ResultListView.SelectedItems(0).ForeColor = Color.RoyalBlue

				'-------------------------------------------------------
				'ORM Verbalisation
				'-------------------------------------------------------
				Dim lrToolboxForm As frmToolboxORMVerbalisation = Nothing
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.zrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.zrModel
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
				lrToolboxForm = frmMain.loadToolboxORMVerbalisationForm(Me.zrModel, Me.DockPanel.ActivePane)

				If IsSomething(lrToolboxForm) Then
					lrToolboxForm.zrModel = Me.zrModel
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
End Class