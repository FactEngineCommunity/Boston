Imports Boston.Parser.Syntax.Constants

Namespace UI

    Partial Friend Class CodeEditor

        Private Const POPUP_OPACITY As Integer = 85
        Private Const POPUP_FADE_STEP_RATE As Integer = 10
        Private Const POPUP_DISABLE_FADE As Boolean = True
        Private Const POPUP_SPACEACROSS_SIZE As Integer = 7

        Private WithEvents popup As New SyntaxPopup()

        Private ccAnalyser As New Parser.CodeCompletion.Analyser()
        Private ccContext As SyntaxPopupList.Contexts = SyntaxPopupList.Contexts.DontShow

        Private Function EnteringTag(ByVal newChar As String) As Boolean
            Dim idx As Integer = Me.txtBox.SelectionStart + 1 - TAG_BEGIN.Length
            If idx < 0 Then idx = 0
            Dim str As String = Me.txtBox.Text.Substring(idx, TAG_BEGIN.Length - 1) & newChar
            Return Parser.Syntax.Strings.StrEq(str, TAG_BEGIN)
        End Function

        Private Function LeavingTag(ByVal newChar As String) As Boolean
            Dim idx As Integer = Me.txtBox.SelectionStart - TAG_END.Length
            If idx < 0 Then idx = 0
            Dim str As String = Me.txtBox.Text.Substring(idx + 1, TAG_END.Length - 1) & newChar
            Return Parser.Syntax.Strings.StrEq(str, TAG_END)
        End Function

        Private Function GetPopupContext(Optional ByVal defaultAs As SyntaxPopupList.Contexts = SyntaxPopupList.Contexts.DontShow) As SyntaxPopupList.Contexts
            Try
                If Me.lineWords.node Is Nothing Then Return SyntaxPopupList.Contexts.DontShow

                'Don't show on comments
                If Me.lineWords.node.Action = Parser.Syntax.SyntaxNode.ExecActions.COMMENT Then Return SyntaxPopupList.Contexts.DontShow

                Dim ret As SyntaxPopupList.Contexts = defaultAs

                'Default to base first if a dont show
                If ret = SyntaxPopupList.Contexts.DontShow Then
                    Select Case Me.txtBox.HightlightMode
                        Case UI.txtBox.HighlightModes.MetadroneMain, UI.txtBox.HighlightModes.MetadroneSuperMain
                            ret = SyntaxPopupList.Contexts.StartInMain
                        Case UI.txtBox.HighlightModes.MetadroneTemplate
                            ret = SyntaxPopupList.Contexts.StartInTemplate
                    End Select
                End If

                'No code is base
                If Me.lineWords.node.Tokens.Count = 0 Then Return ret

                'Evalute from action
                Select Case Me.lineWords.node.Action
                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_FOR
                        'Still in the base
                        If Me.lineWords.node.Tokens.Count = 1 And Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                        Select Case Me.lineWords.node.ForEntity
                            Case Parser.Syntax.SyntaxNode.ExecForEntities.NULL
                                If Me.lineWords.node.Tokens.Count > 2 Then Return SyntaxPopupList.Contexts.DontShow
                                Return SyntaxPopupList.Contexts.For

                            Case Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_TABLE, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_VIEW, _
                                 Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_PROCEDURE, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                                'show only after 'in' and during the source token
                                Select Case Me.lineWords.node.Tokens.Count
                                    Case 4
                                        Return SyntaxPopupList.Contexts.InSources
                                    Case 5
                                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow

                                        If Me.lineWords.node.Tokens(4).Text.IndexOf(".") > -1 Then
                                            'Check variable reference against reserved - remember name is up to the dot
                                            If Me.lineWords.node.Tokens(4).Text.Split(".".ToCharArray).Count > 2 Then
                                                Return SyntaxPopupList.Contexts.DontShow
                                            End If
                                            Dim varName As String = Me.lineWords.node.Tokens(4).Text.Substring(0, Me.lineWords.node.Tokens(4).Text.IndexOf("."))

                                            If Parser.Syntax.Strings.StrEq(varName, GLOBALS_SOURCES) Then
                                                Return SyntaxPopupList.Contexts.Sources
                                            End If
                                        End If

                                        Return SyntaxPopupList.Contexts.InSources
                                    Case Else
                                        Return SyntaxPopupList.Contexts.DontShow
                                End Select

                            Case Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_COLUMN, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_FKCOLUMN, _
                                 Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_IDCOLUMN, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_PKCOLUMN
                                'show only after 'in' and during the source token
                                Select Case Me.lineWords.node.Tokens.Count
                                    Case 4
                                        Return SyntaxPopupList.Contexts.InTable_View_Routine
                                    Case 5
                                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow
                                        Return SyntaxPopupList.Contexts.InTable_View_Routine
                                    Case Else
                                        Return SyntaxPopupList.Contexts.DontShow
                                End Select

                            Case Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_PARAMETER, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_INOUTPARAMETER, _
                                 Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_INPARAMETER, Parser.Syntax.SyntaxNode.ExecForEntities.OBJECT_OUTPARAMETER
                                'show only after 'in' and during the source token
                                Select Case Me.lineWords.node.Tokens.Count
                                    Case 4
                                        Return SyntaxPopupList.Contexts.InRoutineOnly
                                    Case 5
                                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow
                                        Return SyntaxPopupList.Contexts.InRoutineOnly
                                    Case Else
                                        Return SyntaxPopupList.Contexts.DontShow
                                End Select

                            Case Else
                                'don't show anymore for now
                                Return SyntaxPopupList.Contexts.DontShow

                        End Select

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_CALL
                        If Me.lineWords.node.Tokens.Count = 1 Then
                            'Still in the base
                            If Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                            'expression for this statement
                            Return SyntaxPopupList.Contexts.Call
                        End If

                        'In the middle of an expression

                        'Dont need to see on white space
                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow

                        Return SyntaxPopupList.Contexts.Call

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_SET
                        If Me.lineWords.node.Tokens.Count = 1 Then
                            'Still in the base
                            If Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                            'expression for this statement
                            Return SyntaxPopupList.Contexts.Set
                        End If

                        'In the middle of an expression

                        'Dont need to see on white space
                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow

                        'Re-eval context for last word
                        Call Me.ReEvalLastLineword()
                        Return Me.GetPopupContext(SyntaxPopupList.Contexts.Expression)

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_IF, _
                         Parser.Syntax.SyntaxNode.ExecActions.ACTION_ELSEIF, _
                         Parser.Syntax.SyntaxNode.ExecActions.ACTION_EXITWHEN
                        If Me.lineWords.node.Tokens.Count = 1 Then
                            'Still in the base
                            If Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                            'expression for this statement
                            Return SyntaxPopupList.Contexts.Expression
                        End If

                        'In the middle of an expression

                        'Dont need to see on white space
                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow

                        'Re-eval context for last word
                        Call Me.ReEvalLastLineword()
                        Return Me.GetPopupContext(SyntaxPopupList.Contexts.Expression)

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_ELSE
                        'Still in the base
                        If Me.lineWords.node.Tokens.Count = 1 And Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                        Return SyntaxPopupList.Contexts.DontShow

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_END
                        'Still in the base
                        If Me.lineWords.node.Tokens.Count = 1 And Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                        Return SyntaxPopupList.Contexts.DontShow

                    Case Parser.Syntax.SyntaxNode.ExecActions.ACTION_HEADER
                        'Still in the base
                        If Me.lineWords.node.Tokens.Count = 1 And Me.lineWords.SpaceCountBeforeCaret = 0 Then Return ret

                        Return SyntaxPopupList.Contexts.DontShow

                    Case Parser.Syntax.SyntaxNode.ExecActions.IDENTIFIER
                        'Dont need to see on white space
                        If Me.lineWords.SpaceCountBeforeCaret > 0 Then Return SyntaxPopupList.Contexts.DontShow

                        If Me.lineWords.lastWord.IndexOf(".") > -1 Then
                            'member of variable

                            'invalid expression
                            If Me.lineWords.node.Tokens.Count > 1 Then Return SyntaxPopupList.Contexts.DontShow

                            'not if start with dot
                            If Me.lineWords.lastWord.Substring(0, 1).Equals(".") Then Return SyntaxPopupList.Contexts.DontShow

                            'not on ... successive dots
                            If Me.lineWords.lastWord.Length > 1 AndAlso _
                               Me.lineWords.lastWord.Substring(Me.lineWords.lastWord.Length - 2, 2).Equals("..") Then
                                Return SyntaxPopupList.Contexts.DontShow
                            End If

                            'finally, this is crude, but we don't support nested members
                            If Me.lineWords.lastWord.Split(".".ToCharArray).Length > 2 Then Return SyntaxPopupList.Contexts.DontShow

                            'Check variable reference against reserved - remember name is up to the dot
                            Dim varName As String = Me.lineWords.lastWord.Substring(0, Me.lineWords.lastWord.IndexOf("."))

                            If Parser.Syntax.Strings.StrEq(varName, GLOBALS_SOURCES) Then
                                Return SyntaxPopupList.Contexts.Sources
                            End If

                            'Member otherwise
                            Return SyntaxPopupList.Contexts.Member

                        ElseIf Me.lineWords.node.Tokens.Count = 1 Then
                            'Still in the default
                            Return ret

                        End If

                        Return SyntaxPopupList.Contexts.Expression

                    Case Else
                        Return SyntaxPopupList.Contexts.DontShow

                End Select

                Return ret

            Catch ex As Exception
                Return SyntaxPopupList.Contexts.DontShow

            End Try
        End Function

        Private Sub ReEvalLastLineword()
            Dim lastword = Me.lineWords.lastWord
            Me.lineWords = New UI.txtBox.LineWords()
            Select Case Me.txtBox.HightlightMode
                Case UI.txtBox.HighlightModes.MetadroneMain, UI.txtBox.HighlightModes.MetadroneSuperMain
                    Me.lineWords.node = New Parser.Syntax.SyntaxNode(lastword, False, 0, True, Nothing, True)
                Case UI.txtBox.HighlightModes.MetadroneTemplate
                    lineWords.node = New Parser.Syntax.SyntaxNode(lastword, False, 0, False, Nothing, True)
            End Select
            If Me.lineWords.node.Tokens.Count > 0 Then Me.lineWords.lastWord = Me.lineWords.node.Tokens(Me.lineWords.node.Tokens.Count - 1).Text
        End Sub

        Private Sub FillShowMatchPopup(ByVal MoveBoxLeft As Boolean)
            'Get context
            Dim Context As SyntaxPopupList.Contexts = Me.GetPopupContext()
            Dim dontRefresh As Boolean = Me.ccContext = Context
            Me.ccContext = Context
            If Context = SyntaxPopupList.Contexts.Member Then dontRefresh = False 'we always want to refresh on a member

            'Hide on don't show
            If Context = SyntaxPopupList.Contexts.DontShow Then
                Call Me.HidePopup(False)
                Exit Sub
            End If

            'When refreshing clear first to fill again
            If Not dontRefresh Then
                Me.popup.Init(True)
                Application.DoEvents()
            End If

            'Make sure popped up
            'If Not Me.popup.Visible Then Me.ShowPopup(Context, True)

            'Fill popup list first to check against
            If Not dontRefresh Then Call Me.FillPopup(Context, False)

            'Don't continue if cancelled and popup hidden
            'If Not Me.popup.Visible Then Exit Sub


            'if the context is a member, now that we've parsed and filled
            'check this identifier is not a primitive (no members in primitives)
            If Context = SyntaxPopupList.Contexts.Member Then
                Dim varname As String = Me.lineWords.lastWord
                Dim matched As Boolean = False
                'name is up to the dot
                Dim dotIdx As Integer = Me.lineWords.lastWord.IndexOf(".")
                varname = Me.lineWords.lastWord.Substring(0, dotIdx)

                'Check against reserved
                If Parser.Syntax.Strings.StrEq(varname, GLOBALS_SOURCES) Then
                    'Assume matched
                    matched = True
                    Context = SyntaxPopupList.Contexts.Sources

                Else
                    For Each var As Parser.CodeCompletion.Variable In Me.ccAnalyser.MainVars
                        'Primitives dont' count
                        If var.Type = Parser.CodeCompletion.Variable.Types.Primitive Then Continue For
                        If Parser.Syntax.Strings.StrEq(varname, var.Name) Then
                            matched = True
                            Exit For
                        End If
                    Next
                    If Not matched Then
                        For Each var As Parser.CodeCompletion.Variable In Me.ccAnalyser.TemplateVars
                            'Primitives dont' count
                            If var.Type = Parser.CodeCompletion.Variable.Types.Primitive Then Continue For
                            If Parser.Syntax.Strings.StrEq(varname, var.Name) Then
                                matched = True
                                Exit For
                            End If
                        Next
                    End If

                End If

                'Don't continue if it's not a valid variable
                If Not matched Then
                    Call Me.HidePopup(False)
                    Exit Sub
                End If
            End If


            'Match..
            Me.popup.lst.SelectedIndex = -1
            If Me.lineWords.SpaceCountBeforeCaret = 0 Then
                Select Case Context
                    Case SyntaxPopupList.Contexts.Member, SyntaxPopupList.Contexts.Sources
                        'Match if in a word (only after the period)
                        Dim dotIdx As Integer = Me.lineWords.lastWord.LastIndexOf(".")
                        If dotIdx = -1 Then
                            Me.popup.MatchItem(Me.lineWords.lastWord)
                        Else
                            Me.popup.MatchItem(Me.lineWords.lastWord.Substring(dotIdx + 1, Me.lineWords.lastWord.Length - (dotIdx + 1)))
                        End If
                    Case Else
                        'Match if in a word
                        Me.popup.MatchItem(Me.lineWords.lastWord)

                        'Box not relevant if nothing matches current word
                        If Me.lineWords.lastWord.Length > 0 And Me.popup.lst.SelectedIndex = -1 Then
                            Call Me.HidePopup(False)
                            Exit Sub
                        End If
                End Select
            End If

            'Move over box
            If MoveBoxLeft Then
                Me.popup.Left -= POPUP_SPACEACROSS_SIZE
            Else
                Me.popup.Left += POPUP_SPACEACROSS_SIZE
            End If

            'Pop up lastly for now
            If Not Me.popup.Visible Then Me.ShowPopup(Context, True)
        End Sub

        Private Sub FillPopup(ByVal Context As SyntaxPopupList.Contexts, ByVal NoEvents As Boolean)
            'Get explorer control
            Dim ex As Explorer = Me.GetExplorerControl(Me.ParentForm)
            If ex Is Nothing Then Throw New Exception("Failure to find project explorer control")

            'Avoid issues
            If ex.tvwMain.Nodes.Count = 0 Then Exit Sub

            'Update code com to fill
            Select Case Me.txtBox.HightlightMode
                Case UI.txtBox.HighlightModes.MetadroneMain, UI.txtBox.HighlightModes.MetadroneSuperMain
                    Me.ccAnalyser.Init(ex.Transform(ex.tvwMain.Nodes(0)), _
                                                    CType(Me.Tag, Persistence.IEditorItem).OwnerGUID, _
                                                    Me.Text.Substring(0, Me.txtBox.Offset), "")
                Case UI.txtBox.HighlightModes.MetadroneTemplate
                    Me.ccAnalyser.Init(ex.Transform(ex.tvwMain.Nodes(0)), _
                                                    CType(Me.Tag, Persistence.IEditorItem).OwnerGUID, _
                                                    Me.GetMain().Text, Me.Text.Substring(0, Me.txtBox.Offset))
            End Select

            ''Wait until update has completed
            'If My.Application.EXEC_ParseInThread Then
            '    Me.ccAnalyser.Completed = False
            '    Dim t As New System.Threading.Thread(AddressOf Me.ccAnalyser.Begin)
            '    t.Start()
            '    Dim stopInfiniteLoop As Integer = 0
            '    While Not Me.ccAnalyser.Completed And stopInfiniteLoop < 5000
            '        stopInfiniteLoop += 1
            '        Application.DoEvents()
            '        System.Threading.Thread.Sleep(1)
            '    End While
            'Else
            '    Me.ccAnalyser.Begin()
            'End If

            ''Hide on cancel
            'If Me.ccAnalyser.CancelRequest Then
            '    Me.HidePopup(False)
            '    Exit Sub
            'End If

            'Non-threaded for now
            Me.ccAnalyser.Begin()

            'Fill
            Call Me.popup.Fill(Me.lineWords, Context, Me.ccAnalyser)
            Me.popup.SelectedIndex = -1
        End Sub

        Private Sub ShowPopup(ByVal Context As SyntaxPopupList.Contexts, ByVal Fade As Boolean)
            If Me.NoPopup Then Exit Sub

            If Context = SyntaxPopupList.Contexts.DontShow Then Exit Sub

            'Get popup location
            Dim pt As Point = Me.txtBox.GetPopupPoint

            If TypeOf Me.Parent Is TabPage Then
                If TypeOf Me.Parent.Parent Is CustTabControl Then
                    If TypeOf Me.Parent.Parent.Parent Is SplitterPanel Then
                        pt.X += CType(Me.Parent.Parent.Parent.Parent, SplitContainer).Panel1.Width - 20
                        pt.Y += CType(Me.Parent.Parent.Parent.Parent, SplitContainer).Top + 25
                    End If
                End If
            End If

            'Show
            Me.popup.Left = pt.X
            Me.popup.Top = pt.Y

            If Me.popup.Top + Me.popup.Height > Screen.PrimaryScreen.Bounds.Height Then
                Me.popup.Top -= Me.popup.Height + 25
            End If

            If Not Me.popup.Visible Then
                Me.popup.TopLevel = True
                If Not POPUP_DISABLE_FADE And Fade Then
                    Me.popup.Opacity = 0
                    Me.popup.Show(Me.ParentForm)
                    For i = 0 To POPUP_OPACITY Step POPUP_FADE_STEP_RATE
                        Me.popup.Opacity = i / 100
                        Me.popup.Refresh()
                    Next
                    Me.popup.Opacity = POPUP_OPACITY / 100
                Else
                    Me.popup.Opacity = POPUP_OPACITY / 100
                    Me.popup.Show(Me.ParentForm)
                End If
            End If
            Me.txtBox.SuppressCursorMove = True
            Me.FocusText()
        End Sub

        Friend Sub HidePopup(ByVal Fade As Boolean)
            Me.ccAnalyser.CancelRequest = True
            If Not Me.popup.Visible Then Exit Sub
            If Me.NoPopup Then Exit Sub

            Me.txtBox.SuppressCursorMove = False
            Me.popup.TopMost = False
            If Fade And Not POPUP_DISABLE_FADE Then
                For i = POPUP_OPACITY To 0 Step -POPUP_FADE_STEP_RATE
                    Me.popup.Opacity = i / 100
                    Me.popup.Refresh()
                Next
                Me.popup.Opacity = 0
            End If
            Me.popup.Hide()
            Me.FocusText()
        End Sub

        Private Sub popup_Cancel() Handles popup.Cancel
            Me.ccAnalyser.CancelRequest = True
        End Sub

        Private Sub popup_UserSelected() Handles popup.UserSelected
            'use popup's selected value
            If Not Me.popup.Visible Then Exit Sub
            If String.IsNullOrEmpty(Me.popup.SelectedItem) Then Exit Sub

            'Get current line
            Me.lineWords = Me.txtBox.GetCurrentLine(Nothing, Me.popupAble)

            If Not String.IsNullOrEmpty(Me.lineWords.lastWord) Then
                If Me.lineWords.SpaceCountBeforeCaret = 0 Then
                    Dim caretCol As Integer = Me.txtBox.CaretColumn
                    Dim caretLin As Integer = Me.txtBox.CaretLine
                    Dim idx As Integer = Me.lineWords.lastWord.LastIndexOf(".")
                    If idx > -1 Then
                        'Remove current after period word to replace
                        Me.txtBox.SetSelection(caretCol - Me.lineWords.lastWord.Length + idx + 1, caretLin, caretCol, caretLin)
                        Me.txtBox.RemoveSelectedText()
                        'reset selection for insert
                        Me.txtBox.SetSelection(caretCol - Me.lineWords.lastWord.Length + idx + 1, caretLin, _
                                               caretCol - Me.lineWords.lastWord.Length + idx + 1, caretLin)
                    Else
                        'Remove current word to replace
                        Me.txtBox.SetSelection(caretCol - Me.lineWords.lastWord.Length, caretLin, caretCol, caretLin)
                        Me.txtBox.RemoveSelectedText()
                        'reset selection for insert
                        Me.txtBox.SetSelection(caretCol - Me.lineWords.lastWord.Length, caretLin, _
                                               caretCol - Me.lineWords.lastWord.Length, caretLin)
                    End If
                End If
            End If

            'now insert
            Me.txtBox.SelectedText = Me.popup.SelectedItem

            Me.HidePopup(True)
        End Sub

    End Class

End Namespace