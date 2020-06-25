Imports Boston.Parser.Syntax.Constants

Namespace UI

    Friend Class txtBox
        Private WithEvents txtMain As ICSharpCode.TextEditor.TextEditorControl = Nothing

        Private strHighlightMode As String = "MetadroneTemplate"

        Friend Class LineWords
            Public node As Parser.Syntax.SyntaxNode = Nothing
            Public lastWord As String = ""
            Public SpaceCountBeforeCaret As Integer = 0
        End Class


        Public Shadows Event Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        Public Shadows Event TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Shadows Event KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Public Shadows Event KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Public Shadows Event MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)


        Public Enum HighlightModes
            MetadroneTemplate = 0
            MetadroneMain = 1
            MetadroneSuperMain = 2
            SQL = 3
            Transformations = 4
            VB = 5
            CS = 6
        End Enum

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call Me.Setup()
        End Sub

        Public Sub New(ByVal HighlightMode As HighlightModes)
            MyBase.New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.HightlightMode = HighlightMode

            Call Me.Setup()
        End Sub

        Private Sub Setup()
            Try
                Me.txtMain = New ICSharpCode.TextEditor.TextEditorControl()
                Me.txtMain.ConvertTabsToSpaces = True
                Me.txtMain.ShowMatchingBracket = False
                Me.txtMain.TabIndent = 4

                AddHandler Me.txtMain.ActiveTextAreaControl.TextArea.KeyDown, AddressOf txtMain_KeyDown
                AddHandler Me.txtMain.ActiveTextAreaControl.TextArea.KeyPress, AddressOf txtMain_KeyPress
                AddHandler Me.txtMain.ActiveTextAreaControl.TextArea.MouseDown, AddressOf txtMain_MouseDown

            Catch ex As Exception

            End Try
        End Sub

        Public Shadows ReadOnly Property Focused() As Boolean
            Get
                Return Me.txtMain.ActiveTextAreaControl.TextArea.Focused
            End Get
        End Property

        Public Shadows Sub Focus()
            Me.txtMain.Focus()
            'Move caret to selection
            Try
                If Me.txtMain.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0 Then
                    Me.txtMain.ActiveTextAreaControl.TextArea.Caret.Position = _
                        Me.txtMain.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).EndPosition
                    Me.txtMain.ActiveTextAreaControl.ScrollToCaret()
                    Me.txtMain.ActiveTextAreaControl.TextArea.Caret.UpdateCaretPosition()
                    If Me.txtMain.ActiveControl IsNot Nothing Then Me.txtMain.ActiveControl.Refresh()
                End If
            Catch ex As Exception
                'Just ignore
            End Try
        End Sub

        Public Shadows Sub Refresh()
            Me.txtMain.ActiveTextAreaControl.TextArea.Refresh()
            MyBase.Refresh()
        End Sub

        Public ReadOnly Property Offset() As Integer
            Get
                Return Me.txtMain.ActiveTextAreaControl.Caret.Offset
            End Get
        End Property

        Public Sub ClearSelection()
            Me.txtMain.ActiveTextAreaControl.SelectionManager.ClearSelection()
        End Sub

        Private Sub txtBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.txtMain.Dock = DockStyle.Fill
            Me.txtMain.SetHighlighting(Me.strHighlightMode)
            Me.txtMain.ShowVRuler = False
            Me.Controls.Add(Me.txtMain)
        End Sub

        Friend Class CaretPopupAble
            Public InCode As Boolean = False
            Public InString As Boolean = False
            Public LastTagBeginOffset As Integer = -1
            Public Sub New(ByVal inCode As Boolean, ByVal inString As Boolean, ByVal lastTagBeginOffset As Integer)
                Me.InCode = inCode : Me.InString = inString : Me.LastTagBeginOffset = lastTagBeginOffset
            End Sub
        End Class

        Friend Function IsCaretInMainCode() As CaretPopupAble
            'Check if entering string
            Dim s() As String = Me.txtMain.Text.Substring(0, Me.txtMain.ActiveTextAreaControl.Caret.Offset).Split("""".ToCharArray)
            Return New CaretPopupAble(True, s.Count Mod 2 = 0, -1)
        End Function


        Friend Function IsCaretInTemplateCode() As CaretPopupAble
            'Get text up until caret and look for last tag begin
            Dim sr As New System.IO.StringReader(Me.txtMain.Text.Substring(0, Me.txtMain.ActiveTextAreaControl.Caret.Offset))
            Try
                Dim sb As New System.Text.StringBuilder()  'Read buffer
                Dim inTag As Boolean = False
                Dim inStr As Boolean = False
                Dim tagOffset As Integer = -1
                Dim currOffset As Integer = 0
                Dim tagBeginFirstChar As Char = TAG_BEGIN.Substring(0, 1).ToCharArray()(0)
                Dim tagEndFirstChar As Char = TAG_END.Substring(0, 1).ToCharArray()(0)
                While sr.Peek > -1
                    If Not inStr Then
                        'Not in string literals, so look out for tag defs
                        If Not inTag Then
                            'Not in a tag, peek ahead to see if we are entering a tag
                            If Convert.ToChar(sr.Peek).Equals(tagBeginFirstChar) Then
                                'Look for begin tag
                                Dim ch(TAG_BEGIN.Length - 1) As Char
                                Dim chLen As Integer = sr.Read(ch, 0, TAG_BEGIN.Length)
                                inTag = chLen = TAG_BEGIN.Length And CStr(ch).Equals(TAG_BEGIN)

                                If inTag Then
                                    'Entered a tag

                                    'Reset buffer
                                    If sb.Length > 0 Then sb = New System.Text.StringBuilder()

                                    'Update latest tag offset
                                    tagOffset = currOffset

                                    'Check if entering string
                                    Dim s() As String = CStr(ch).Split("""".ToCharArray)
                                    inStr = s.Count Mod 2 = 0
                                Else
                                    'Otherwise, just add to buffer
                                    sb.Append(CStr(ch))
                                End If

                                currOffset += TAG_BEGIN.Length
                            Else
                                'Nothing to check here, just add to buffer
                                sb.Append(Convert.ToChar(sr.Read()).ToString)
                                currOffset += 1
                            End If

                        Else
                            'In a tag, peek ahead to see if we are getting to an end tag
                            If Convert.ToChar(sr.Peek).Equals(tagEndFirstChar) Then
                                'Look for end tag
                                Dim ch(TAG_END.Length - 1) As Char
                                Dim chLen As Integer = sr.Read(ch, 0, TAG_END.Length)
                                inTag = Not (chLen = TAG_END.Length And CStr(ch).Equals(TAG_END))

                                If Not inTag Then
                                    'Left the tag
                                    sb = New System.Text.StringBuilder(sb.ToString.Trim)
                                    If sb.Length > 0 Then
                                        sb = New System.Text.StringBuilder() 'Reset buffer
                                    End If
                                Else
                                    'Otherwise, just add to buffer
                                    sb.Append(CStr(ch))

                                    'Check if entering string
                                    Dim s() As String = CStr(ch).Split("""".ToCharArray)
                                    inStr = s.Count Mod 2 = 0
                                End If

                                currOffset += TAG_BEGIN.Length
                            Else
                                'Nothing to check here, just add to buffer
                                sb.Append(Convert.ToChar(sr.Read()).ToString)
                                currOffset += 1
                            End If

                        End If

                    Else
                        'In string literals, look for literal termination
                        Dim ch As String = Convert.ToChar(sr.Read()).ToString
                        sb.Append(ch)
                        inStr = Not ch.Equals("""")
                        currOffset += 1

                    End If

                End While

                'Check instr for last fragment
                If sb.Length > 0 And inTag Then
                    Dim s() As String = sb.ToString.Split("""".ToCharArray)
                    inStr = s.Count Mod 2 = 0
                End If

                Return New CaretPopupAble(inTag, inStr, tagOffset)

            Catch ex As Exception
                'Ignore
                Return New CaretPopupAble(False, False, -1)

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try
        End Function

        Friend Function GetPopupPoint() As Point
            With Me.txtMain.ActiveTextAreaControl.Caret.ScreenPosition
                Dim leftToBe As Integer = Me.ParentForm.Left + Me.Left + .X + 24
                Dim topToBe As Integer = Me.ParentForm.Top + Me.Top + .Y + 92

                Return New Point(leftToBe, topToBe)
            End With
        End Function

        'Get words on current line
        Friend Function GetCurrentLine(ByVal keyChar As Char, ByVal ppAble As CaretPopupAble) As LineWords
            Try
                Dim lineWords As New LineWords()

                Select Case Me.HightlightMode
                    Case UI.txtBox.HighlightModes.SQL
                        'No popup
                        Return lineWords

                    Case UI.txtBox.HighlightModes.MetadroneMain, HighlightModes.MetadroneSuperMain
                        'not in a string
                        If Me.IsCaretInMainCode().InString Then Return lineWords

                    Case UI.txtBox.HighlightModes.MetadroneTemplate
                        'only in code, not in a string
                        If Not ppAble.InCode Then Return lineWords
                        If ppAble.InString Then Return lineWords

                End Select

                With Me.txtMain.ActiveTextAreaControl
                    'line index from start up to caret
                    Dim startIndex As Integer = .Document.GetLineSegment(.Caret.Line).Offset
                    Dim length As Integer = .Caret.Column

                    'Quirk for template
                    Select Case Me.HightlightMode
                        Case UI.txtBox.HighlightModes.MetadroneTemplate
                            'Get index/length past last begin tag up to caret
                            Dim fromTagStartIndex = ppAble.LastTagBeginOffset + TAG_BEGIN.Length

                            If fromTagStartIndex < startIndex Then
                                'We're inside a begin tag with new lines (the tag is above the start index of the current line)
                                'So we do nothing, since we are only working on the current line
                            Else
                                'tag is on this current line, start from the tag only ignore anything before it
                                startIndex = fromTagStartIndex
                                length = .Caret.Offset - startIndex
                            End If
                    End Select

                    'Get the line
                    Dim line As New System.Text.StringBuilder(Me.txtMain.Text.Substring(startIndex, length))
                    'plus the key stroke
                    If keyChar <> Nothing Then
                        If keyChar = ControlChars.Back Then
                            line.Remove(line.Length - 1, 1)
                        Else
                            line.Append(keyChar)
                        End If
                    End If

                    'Get last statement (if; any; seperated)
                    Dim lines() As String = line.ToString.Split(RESERVED_SEPERATOR.ToCharArray)
                    If lines.Count > 1 Then line = New System.Text.StringBuilder(lines(lines.Count - 1))

                    'Compile as a syntax node
                    Select Case Me.HightlightMode
                        Case UI.txtBox.HighlightModes.MetadroneMain, HighlightModes.MetadroneSuperMain
                            lineWords.node = New Parser.Syntax.SyntaxNode(line.ToString, False, 0, True, Nothing, True)
                        Case UI.txtBox.HighlightModes.MetadroneTemplate
                            lineWords.node = New Parser.Syntax.SyntaxNode(line.ToString, False, 0, False, Nothing, True)
                    End Select
                    If lineWords.node.Tokens.Count > 0 Then lineWords.lastWord = lineWords.node.Tokens(lineWords.node.Tokens.Count - 1).Text

                    'Check for spaces just before the caret
                    lineWords.SpaceCountBeforeCaret = 0
                    Dim sr As System.IO.StringReader = Nothing
                    Try
                        sr = New System.IO.StringReader(CStr(line.ToString.Reverse.ToArray))
                        While sr.Peek > -1
                            If Not Convert.ToChar(sr.Read()).ToString.Equals(" ") Then Exit While
                            lineWords.SpaceCountBeforeCaret += 1
                        End While
                    Catch ex As Exception
                        Throw
                    Finally
                        sr.Close()
                        sr.Dispose()
                        sr = Nothing
                    End Try

                    Return lineWords
                End With
            Catch ex As Exception
                Return New LineWords()
            End Try
        End Function

        Public Sub FocusText()
            Me.txtMain.Focus()
        End Sub

        Public Property SuppressCursorMove() As Boolean
            Get
                Return Me.txtMain.ActiveTextAreaControl.TextArea.SuppressCursorMove
            End Get
            Set(ByVal value As Boolean)
                Me.txtMain.ActiveTextAreaControl.TextArea.SuppressCursorMove = value
            End Set
        End Property

        Public Property HightlightMode() As HighlightModes
            Get
                If Me.strHighlightMode.Equals("MetadroneTemplate") Then
                    Return HighlightModes.MetadroneTemplate

                ElseIf Me.strHighlightMode.Equals("MetadroneMain") Then
                    Return HighlightModes.MetadroneMain

                ElseIf Me.strHighlightMode.Equals("MetadroneSuperMain") Then
                    Return HighlightModes.MetadroneSuperMain

                ElseIf Me.strHighlightMode.Equals("SQL") Then
                    Return HighlightModes.SQL

                ElseIf Me.strHighlightMode.Equals("MetadroneTransformations") Then
                    Return HighlightModes.Transformations

                ElseIf Me.strHighlightMode.Equals("VBNET") Then
                    Return HighlightModes.VB

                ElseIf Me.strHighlightMode.Equals("C#") Then
                    Return HighlightModes.CS

                End If
            End Get
            Set(ByVal value As HighlightModes)
                Select Case value
                    Case HighlightModes.MetadroneTemplate : Me.strHighlightMode = "MetadroneTemplate"
                    Case HighlightModes.MetadroneMain : Me.strHighlightMode = "MetadroneMain"
                    Case HighlightModes.MetadroneSuperMain : Me.strHighlightMode = "MetadroneSuperMain"
                    Case HighlightModes.SQL : Me.strHighlightMode = "SQL"
                    Case HighlightModes.Transformations : Me.strHighlightMode = "MetadroneTransformations"
                    Case HighlightModes.VB : Me.strHighlightMode = "VBNET"
                    Case HighlightModes.CS : Me.strHighlightMode = "C#"
                End Select
            End Set
        End Property

        Public Shadows Property Text() As String
            Get
                Return Me.txtMain.Text
            End Get
            Set(ByVal value As String)
                Me.txtMain.Text = value
            End Set
        End Property

        Public Property ShowLineNumbers() As Boolean
            Get
                Return Me.txtMain.ShowLineNumbers
            End Get
            Set(ByVal value As Boolean)
                Me.txtMain.ShowLineNumbers = value
            End Set
        End Property

        Public Property SelectedText() As String
            Get
                Return Me.txtMain.ActiveTextAreaControl.SelectionManager.SelectedText
            End Get
            Set(ByVal value As String)
                Me.txtMain.ActiveTextAreaControl.TextArea.InsertString(value)
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Return Me.txtMain.ActiveTextAreaControl.Caret.Offset
            End Get
            Set(ByVal value As Integer)

            End Set
        End Property

        Public Property SelectionLength() As Integer
            Get
                With Me.txtMain.ActiveTextAreaControl.SelectionManager.GetSelectionAtLine(Me.txtMain.ActiveTextAreaControl.Caret.Position.Line)
                    Return .EndColumn - .StartColumn
                End With
            End Get
            Set(ByVal value As Integer)
                
            End Set
        End Property

        Public Property CaretColumn() As Integer
            Get
                Return Me.txtMain.ActiveTextAreaControl.Caret.Column
            End Get
            Set(ByVal value As Integer)
                Me.txtMain.ActiveTextAreaControl.Caret.Column = value
            End Set
        End Property

        Public Property CaretLine() As Integer
            Get
                Return Me.txtMain.ActiveTextAreaControl.Caret.Line
            End Get
            Set(ByVal value As Integer)
                Me.txtMain.ActiveTextAreaControl.Caret.Line = value
            End Set
        End Property

        Public Sub SetSelection(ByVal startCol As Integer, ByVal startLine As Integer, _
                                ByVal endCol As Integer, ByVal endLine As Integer)
            Dim startLoc As New ICSharpCode.TextEditor.TextLocation(startCol, startLine)
            Dim endLoc As New ICSharpCode.TextEditor.TextLocation(endCol, endLine)
            Me.txtMain.ActiveTextAreaControl.SelectionManager.SetSelection(startLoc, endLoc)
        End Sub

        Public Sub SetSelection(ByVal offsetStartChars As Integer, ByVal offsetCharLength As Integer)
            Dim startPos As ICSharpCode.TextEditor.TextLocation = Me.txtMain.ActiveTextAreaControl.Caret.Position
            Dim endPos As ICSharpCode.TextEditor.TextLocation = startPos
            startPos.Column -= offsetStartChars
            endPos.Column -= offsetStartChars : endPos.Column += offsetCharLength
            Me.txtMain.ActiveTextAreaControl.SelectionManager.SetSelection(startPos, endPos)
        End Sub

        Public Sub RemoveSelectedText()
            Me.txtMain.ActiveTextAreaControl.SelectionManager.RemoveSelectedText()
        End Sub

        Public Shadows Property Font() As Font
            Get
                Return Me.txtMain.Font
            End Get
            Set(ByVal value As Font)
                Me.txtMain.Font = value
            End Set
        End Property

        Public Property [ReadOnly]() As Boolean
            Get
                Return Me.txtMain.IsReadOnly
            End Get
            Set(ByVal value As Boolean)
                Me.txtMain.IsReadOnly = value
            End Set
        End Property

        Public Property DontHilight() As Boolean
            Get
                Return False
            End Get
            Set(ByVal value As Boolean)

            End Set
        End Property

        Private Sub txtMain_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtMain.MouseWheel
            If e.Delta > 0 Then
                RaiseEvent Scroll(Me, New System.Windows.Forms.ScrollEventArgs(ScrollEventType.SmallIncrement, e.Delta))
            Else
                RaiseEvent Scroll(Me, New System.Windows.Forms.ScrollEventArgs(ScrollEventType.SmallDecrement, e.Delta))
            End If
        End Sub

        Private Sub txtMain_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles txtMain.Scroll
            RaiseEvent Scroll(Me, e)
        End Sub

        Private Sub txtMain_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMain.TextChanged
            RaiseEvent TextChanged(Me, e)
        End Sub

        Private Sub txtMain_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
            RaiseEvent KeyPress(Me, e)
            e.Handled = True
        End Sub

        Private Sub txtMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
            RaiseEvent KeyDown(Me, e)
        End Sub

        Private Sub txtMain_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
            RaiseEvent MouseDown(Me, e)
        End Sub



        'Highlight next search match in text editor
        'Return:
        '0:   No matches
        '-1:  End of search
        'int: Number of matches
        Public Function FindNext(ByVal searchDoc As MainForm.SearchDocument) As Integer
            'No matches
            If searchDoc.Matches.Count = 0 Then Return 0

            'Get offset to search from
            Dim searchFromOffset As Integer = Me.Offset
            If Me.txtMain.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0 Then
                'use selection if set
                searchFromOffset = Me.txtMain.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
            End If
            'Was at end of search, reset
            If searchDoc.Matches.Count < searchDoc.CurrentMatchIndex + 1 Then searchFromOffset = 0

            'Move match index past offset
            searchDoc.CurrentMatchIndex = 0
            While searchDoc.CurrentMatchIndex < searchDoc.Matches.Count AndAlso _
                      searchDoc.Matches.Item(searchDoc.CurrentMatchIndex).Index <= searchFromOffset
                searchDoc.CurrentMatchIndex += 1
            End While

            'End of search
            If searchDoc.Matches.Count < searchDoc.CurrentMatchIndex + 1 Then Return -1


            'Hilight match
            With searchDoc.Matches.Item(searchDoc.CurrentMatchIndex)
                Dim start As ICSharpCode.TextEditor.TextLocation = Me.txtMain.Document.OffsetToPosition(.Index)
                Dim line As Integer = Me.txtMain.Document.GetLineNumberForOffset(.Index)

                Dim finish As ICSharpCode.TextEditor.TextLocation = Me.txtMain.Document.OffsetToPosition(.Index + .Length)
                Me.txtMain.ActiveTextAreaControl.SelectionManager.SetSelection(start, finish)

                Me.txtMain.ActiveTextAreaControl.TextArea.ScrollTo(line)
                Me.Refresh()
            End With

            'Return match count
            Return searchDoc.Matches.Count
        End Function

    End Class

End Namespace