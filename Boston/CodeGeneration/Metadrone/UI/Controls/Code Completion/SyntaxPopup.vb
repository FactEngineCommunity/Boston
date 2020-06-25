Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings

Namespace UI

    Friend Class SyntaxPopup

        Private ListItems As New SyntaxPopupList()
        Private origWidth As Integer = 600
        Private lstWidth As Integer = 170

        Public Event Cancel()


        Public Event UserSelected()
        Public Event InsertTag(ByVal TagVal As String)

        Private Class ListItem
            Private strText As String
            Private strTag As String

            Public Sub New(ByVal Text As String, ByVal Tag As String)
                Me.strText = Text
                Me.strTag = Tag
            End Sub

            Public Function Tag() As String
                Return Me.strTag
            End Function

            Public Overrides Function ToString() As String
                Return Me.strText
            End Function
        End Class

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.Init(False)

            'Fill with system items
            Call Me.ListItems.Fill(Nothing, SyntaxPopupList.Contexts.DontShow, Nothing)
            Me.origWidth = Me.Width
            Me.lstWidth = Me.lst.Width
        End Sub

        Public ReadOnly Property SelectedItem() As String
            Get
                If Me.lst.SelectedItem Is Nothing Then Return ""

                For Each item In Me.ListItems.Items
                    If Me.lst.SelectedItem.ToString.Equals(item.Item) Then
                        If Not String.IsNullOrEmpty(item.DefaultCompletion) Then
                            Return item.DefaultCompletion
                        Else
                            Return item.Item
                        End If
                    End If
                Next

                Return Me.lst.SelectedItem.ToString
            End Get
        End Property

        Public Property SelectedIndex() As Integer
            Get
                Return Me.lst.SelectedIndex
            End Get
            Set(ByVal value As Integer)
                Me.lst.SelectedIndex = value
            End Set
        End Property

        Public Sub MatchItem(ByVal value As String)
            'Attempt to match values in list
            Me.lst.SelectedIndex = -1
            If String.IsNullOrEmpty(value) Then Exit Sub

            For i As Integer = 0 To Me.lst.Items.Count - 1
                If Me.lst.Items(i).ToString.Length < value.Length Then Continue For

                If StrEq(Me.lst.Items(i).ToString, value) Then
                    Me.lst.SelectedIndex = i
                    Exit Sub
                ElseIf StrEq(Me.lst.Items(i).ToString.Substring(0, value.Length), value) Then
                    Me.lst.SelectedIndex = i
                    Exit Sub
                End If
            Next
        End Sub

        Public Sub Init(ByVal EnableCancel As Boolean)
            Me.lst.Sorted = False
            Me.lst.Items.Clear()
            Me.lst.Width = Me.lstWidth
            Me.Width = Me.origWidth

            If EnableCancel Then
                Me.lblTitle.Text = "Loading"
                Me.lblDesc.Text = "Parsing code and loading variable information..."
                Me.lnkCancel.Visible = True
            Else
                Me.lblTitle.Text = ""
                Me.lblDesc.Text = ""
                Me.lnkCancel.Visible = False
            End If
        End Sub

        Public Function Fill(ByVal lineWords As txtBox.LineWords, _
                             ByVal context As SyntaxPopupList.Contexts, ByVal analysis As Parser.CodeCompletion.Analyser) As Boolean
            Me.Init(False)

            'Set title description
            Select Case context
                Case SyntaxPopupList.Contexts.StartInMain, SyntaxPopupList.Contexts.StartInTemplate
                    Me.lblTitle.Text = "Action, function, or variable reference"

                Case SyntaxPopupList.Contexts.For
                    Me.lblTitle.Text = "Subject for this FOR action"

                Case SyntaxPopupList.Contexts.Expression
                    Me.lblTitle.Text = "Expression"

                Case SyntaxPopupList.Contexts.Call
                    Me.lblTitle.Text = "Call a template"

                Case SyntaxPopupList.Contexts.Set
                    Me.lblTitle.Text = "Set value to variable or variable attribute"

                Case SyntaxPopupList.Contexts.InSources
                    Me.lblTitle.Text = "Source to loop tables/views/routines through"

                Case SyntaxPopupList.Contexts.InTable_View_Routine
                    Me.lblTitle.Text = "Table/view/routine variable to loop columns through"

                Case SyntaxPopupList.Contexts.InRoutineOnly
                    Me.lblTitle.Text = "Routine variable to loop parameters through"

                Case SyntaxPopupList.Contexts.Sources
                    Me.lblTitle.Text = "Select a source"

                Case SyntaxPopupList.Contexts.Member
                    Me.lblTitle.Text = "Select a variable attribute or function"

            End Select

            'Fill list items for the listbox
            Me.ListItems.Fill(lineWords, context, analysis)

            'Get graphics for text size
            Dim g As Graphics = Me.lst.CreateGraphics
            Dim maxSize As Integer = Me.lstWidth 'Original should be 170

            'Fill list box
            For Each item In Me.ListItems.Items
                Me.lst.Items.Add(New ImageListBox.ImageListBoxItem(item.Icon, item.Item))
                Dim size As Integer = CInt(g.MeasureString(Me.lst.Items(Me.lst.Items.Count - 1).ToString, Me.lst.Font).Width)
                If size > maxSize Then maxSize = size
            Next

            'Re-adjust size of list box
            If maxSize > Me.lst.Width Then
                Me.lst.Width = maxSize + 16
                Me.Width = (Me.origWidth - Me.lstWidth) + maxSize + 16
            End If

            'Reset description, and selection
            Me.lblDesc.Text = ""

            If Me.lst.Items.Count = 0 Then
                Me.lst.SelectedIndex = -1
                Return False
            Else
                Me.lst.SelectedIndex = 0
                Return True
            End If
        End Function

        Private Sub lst_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst.DoubleClick
            RaiseEvent UserSelected()
        End Sub

        Private Sub lst_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lst.KeyDown
            Select Case e.KeyCode
                Case Keys.Tab, Keys.Enter
                    'use selected value
                    RaiseEvent UserSelected()
                    e.Handled = True
            End Select
        End Sub

        Private Sub lst_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lst.KeyPress
            If e.KeyChar = Chr(Keys.Escape) Then Me.Hide()
        End Sub

        Private Sub lst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst.SelectedIndexChanged
            Me.lblDesc.Text = ""
            If Me.SelectedIndex = -1 Then Exit Sub

            For Each item In Me.ListItems.Items
                If Me.lst.SelectedItem.ToString.Equals(item.Item) Then
                    Me.lblDesc.Text = item.Description
                End If
            Next
        End Sub

        Private Sub lnkCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCancel.Click
            RaiseEvent Cancel()
        End Sub

    End Class

End Namespace