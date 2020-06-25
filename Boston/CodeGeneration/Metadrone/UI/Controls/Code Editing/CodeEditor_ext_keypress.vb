Imports Boston.Parser.Syntax.Constants

Namespace UI

    Partial Friend Class CodeEditor

        Private lineWords As txtBox.LineWords = Nothing
        Private popupAble As txtBox.CaretPopupAble = Nothing

#Region "Events"

        Private Sub txtBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.TextChanged
            RaiseEvent TextChanged(sender, e)
        End Sub

        Private Sub txtBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBox.Leave
            Me.HidePopup(False)
        End Sub

        Private Sub txtBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBox.KeyPress
            Call Me.HandleKeyPress(e)
        End Sub

        Private Sub txtBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBox.KeyDown
            Call Me.HandleKeyDown(e)
        End Sub

        Private Sub txtBox_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles txtBox.Scroll
            Call Me.HidePopup(False)
        End Sub

#End Region


        Private Function NoPopupHere() As Boolean
            'Check if go ahead with handling a keypress
            Me.popupAble = Nothing
            If Me.ReadOnly Then Return True
            If Me.NoPopup Then Return True
            Select Case Me.txtBox.HightlightMode
                Case UI.txtBox.HighlightModes.SQL
                    'No popup
                    Return True

                Case UI.txtBox.HighlightModes.MetadroneMain, UI.txtBox.HighlightModes.MetadroneSuperMain
                    If Me.txtBox.IsCaretInMainCode().InString Then Return True

                Case UI.txtBox.HighlightModes.MetadroneTemplate
                    'Popup in tag
                    Me.popupAble = Me.txtBox.IsCaretInTemplateCode()
                    If Not Me.popupAble.InCode Then Return True
                    If Me.popupAble.InString Then Return True

            End Select

            Return False
        End Function

        Private Sub HandleKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
            'Check if go ahead with handling a keypress
            If Me.NoPopupHere Then Exit Sub

            'Act on key char
            Select Case e.KeyChar
                Case ControlChars.Tab, ControlChars.Cr, ControlChars.CrLf.ToCharArray, ControlChars.Lf, ControlChars.NewLine.ToCharArray
                    'use popup's selected value
                    Call Me.popup_UserSelected()
                    e.Handled = True

                Case " ".ToCharArray
                    'use popup's selected value
                    Call Me.popup_UserSelected()
                    e.Handled = True

                    'Insert 'in' automatically
                    If Me.lineWords IsNot Nothing AndAlso _
                       Me.lineWords.node IsNot Nothing AndAlso _
                       Me.lineWords.node.Action = Parser.Syntax.SyntaxNode.ExecActions.ACTION_FOR AndAlso _
                       Me.lineWords.node.Tokens.Count = 3 Then
                        Me.SelectedText = " " & ACTION_IN
                    End If

                    'Fill, show, match popup
                    Me.lineWords = Me.txtBox.GetCurrentLine(e.KeyChar, Me.popupAble)
                    Call Me.FillShowMatchPopup(False)

                Case Else
                    'Fill, show, match popup
                    Me.lineWords = Me.txtBox.GetCurrentLine(e.KeyChar, Me.popupAble)
                    Call Me.FillShowMatchPopup(False)

            End Select
        End Sub

        Private Sub HandleKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
            If Me.ReadOnly Then Exit Sub

            Select Case e.KeyCode
                Case Keys.Space
                    'CRTL-Space shows popup
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True

                        'Check if go ahead with handling a keypress
                        If Me.NoPopupHere Then Exit Sub

                        'Fill..
                        Me.lineWords = Me.txtBox.GetCurrentLine(Nothing, Me.popupAble)
                        Dim context As SyntaxPopupList.Contexts = SyntaxPopupList.Contexts.DontShow
                        Select Case Me.txtBox.HightlightMode
                            Case UI.txtBox.HighlightModes.MetadroneMain, UI.txtBox.HighlightModes.MetadroneSuperMain
                                Dim popupAble As txtBox.CaretPopupAble = Me.txtBox.IsCaretInMainCode()
                                If Not popupAble.InString Then
                                    context = Me.GetPopupContext()
                                    If context = SyntaxPopupList.Contexts.DontShow Then
                                        Call Me.HidePopup(False)
                                        Exit Sub
                                    End If
                                    Call Me.FillPopup(context, True)
                                End If
                            Case UI.txtBox.HighlightModes.MetadroneTemplate
                                Dim popupAble As txtBox.CaretPopupAble = Me.txtBox.IsCaretInTemplateCode()
                                If popupAble.InCode And Not popupAble.InString Then
                                    context = Me.GetPopupContext()
                                    If context = SyntaxPopupList.Contexts.DontShow Then
                                        Call Me.HidePopup(False)
                                        Exit Sub
                                    End If
                                    Call Me.FillPopup(context, True)
                                End If
                            Case Else
                                Exit Sub
                        End Select

                        If Me.popup.lst.Items.Count = 0 Then
                            Call Me.HidePopup(False)
                            Exit Sub
                        End If

                        If Me.lineWords.SpaceCountBeforeCaret = 0 Then
                            If context = SyntaxPopupList.Contexts.Member Then
                                'Only get after the dot
                                Dim s() As String = Me.lineWords.lastWord.Split(".".ToCharArray)
                                If s.Count < 2 Then Exit Sub
                                'Match if in this member
                                Me.popup.MatchItem(s(1))
                            Else
                                'Match if in a word
                                Me.popup.MatchItem(Me.lineWords.lastWord)
                            End If
                        End If

                        Call Me.ShowPopup(context, True)
                    End If

                Case Keys.Left, Keys.Right, Keys.Home, Keys.End
                    'exit popup, user wants to keep editing
                    Me.HidePopup(False)

                Case Keys.Escape
                    'Escape, hide popup with escape key
                    Me.HidePopup(True)
                    e.Handled = True
                    e.SuppressKeyPress = True

                Case Keys.Back
                    'Check if go ahead with handling a keypress
                    If Me.NoPopupHere Then Exit Sub

                    'Get current line
                    Me.lineWords = Me.txtBox.GetCurrentLine(ControlChars.Back, Me.popupAble)

                    'hide on no word
                    If Me.lineWords.lastWord.Length = 0 Then
                        Call Me.HidePopup(False)
                        Exit Sub
                    End If

                    Call Me.FillShowMatchPopup(True)

                Case Keys.F
                    'CTRL-F find
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                        RaiseEvent SearchRequested()
                    End If

                Case Keys.S
                    'CRTL-S save
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                        RaiseEvent Save()
                    End If

                Case Keys.Y
                    'CRTL-Y redo
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                Case Keys.Z
                    'CRTL-Z undo
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

                Case Keys.Up
                    'move up the popup list
                    If Me.popup.Visible Then
                        If Me.popup.SelectedIndex > 0 Then Me.popup.SelectedIndex -= 1
                        e.Handled = True
                    End If

                Case Keys.Down
                    'move down the popup list
                    If Me.popup.Visible Then
                        If Me.popup.SelectedIndex < Me.popup.lst.Items.Count - 1 Then Me.popup.SelectedIndex += 1
                        e.Handled = True
                    End If

                Case Keys.PageUp
                    'move up the popup list a page
                    If Me.popup.Visible Then
                        Dim idx As Integer = Me.popup.SelectedIndex
                        idx -= 8
                        If idx < 0 Then idx = 0
                        Me.popup.SelectedIndex = idx
                        e.Handled = True
                    End If

                Case Keys.PageDown
                    'move down the popup list a page
                    If Me.popup.Visible Then
                        Dim idx As Integer = Me.popup.SelectedIndex
                        idx += 8
                        If idx > Me.popup.lst.Items.Count - 1 Then idx = Me.popup.lst.Items.Count - 1
                        Me.popup.SelectedIndex = idx
                        e.Handled = True
                    End If

                Case Keys.F5
                    'F5 - run
                    e.Handled = True
                    e.SuppressKeyPress = True
                    RaiseEvent Run()

                Case Keys.F6
                    'F6 - run preview
                    Select Case Me.txtBox.HightlightMode
                        Case UI.txtBox.HighlightModes.MetadroneTemplate
                            e.Handled = True
                            e.SuppressKeyPress = True
                            Me.btnPreview_Click(Nothing, Nothing)

                    End Select

                Case Else
                    If e.Modifiers = Keys.Control Then
                        e.Handled = True
                        e.SuppressKeyPress = True
                    End If

            End Select
        End Sub

    End Class

End Namespace