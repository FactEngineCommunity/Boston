Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Exec_Base
        Private ExecNode As SyntaxNode
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private WithEvents execCall As Exec_Call = Nothing
        Private WithEvents execBase As Exec_Base = Nothing
        Private WithEvents execFor As Exec_For = Nothing
        Private WithEvents execIf As Exec_If = Nothing

        Friend UserReturnPath As String = Nothing
        Friend UserReturnPathSet As Boolean = False
        Friend UserExitLoop As Boolean = False

        Public OutputList As OutputCollection = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal BasePath As String, ByVal PreviewMode As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.ExecNode = ExecNode
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
            Me.OutputList = New OutputCollection(BasePath)
        End Sub

        Public Function ProcessBlock(Optional ByRef ExitIndex As Integer = -1) As System.Text.StringBuilder
            Return ProcessBlock(0, ExecNode.Nodes.Count - 1, ExitIndex)
        End Function

        Public Function ProcessBlock(ByVal NodeStart As Integer, ByVal NodeEnd As Integer, Optional ByRef ExitIndex As Integer = -1) As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            ExitIndex = -1
            For i As Integer = NodeStart To NodeEnd
                Me.execBase = New Exec_Base(Me.ExecNode.Nodes(i), Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)
                Dim ExitBlock As Boolean = False
                Try
                    sb.Append(Me.execBase.ProcessNode(ExitBlock).ToString)

                    'Pass on output
                    For Each o As OutputItem In Me.execBase.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'Finish on user return
                    If Me.execBase.UserReturnPathSet Then
                        Me.UserReturnPathSet = Me.execBase.UserReturnPathSet
                        Me.UserReturnPath = Me.execBase.UserReturnPath
                        Return sb
                    End If

                Catch ex As FindTemplateException
                    Throw ex
                Catch ex As CallTemplateException
                    Throw ex
                Catch ex As ExecException
                    Throw ex
                Catch ex As Exception
                    'Just throw an exec exception for the parent node
                    Throw New ExecException(ex.Message & " Line: " & Me.ExecNode.Nodes(i).LineNumber.ToString, Me.ExecNode.Nodes(i).LineNumber)

                End Try

                If ExitBlock Then
                    ExitIndex = i
                    Exit For
                End If

                If Me.execBase.UserExitLoop Then
                    Me.UserExitLoop = Me.execBase.UserExitLoop
                    ExitIndex = -1
                    Exit For
                End If
            Next

            Return sb
        End Function

        Private Function ProcessNode(ByRef ExitBlock As Boolean) As System.Text.StringBuilder
            Dim sb As New System.Text.StringBuilder()

            'Abort on cancel
            If PackageBuilder.StopRequest Then Return sb

            Select Case Me.ExecNode.Action
                Case SyntaxNode.ExecActions.PLAINTEXT
                    'Just add plain text
                    sb.Append(Me.ExecNode.Text)

                Case SyntaxNode.ExecActions.COMMENT
                    'Ignore comments

                Case SyntaxNode.ExecActions.PREPROC_IGNORECASE
                    'Setup ignore case preprocessor
                    If StrEq(Me.ExecNode.Tokens(2).Text, RESERVED_PREPROC_IGNORECASE_ON) Then
                        PackageBuilder.PreProc.IgnoreCase = True
                    ElseIf StrEq(Me.ExecNode.Tokens(2).Text, RESERVED_PREPROC_IGNORECASE_OFF) Then
                        PackageBuilder.PreProc.IgnoreCase = False
                    End If

                Case SyntaxNode.ExecActions.PREPROC_SAFEBEGIN
                    'Setup safe begin preprocessor
                    PackageBuilder.PreProc.Safe_Begin = _
                        EvalExpression(Me.ExecNode.Tokens, 3, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth).ToString

                Case SyntaxNode.ExecActions.PREPROC_SAFEEND
                    'Setup safe begin preprocessor
                    PackageBuilder.PreProc.Safe_End = _
                        EvalExpression(Me.ExecNode.Tokens, 3, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth).ToString

                Case SyntaxNode.ExecActions.ACTION_CALL
                    'Call template
                    Me.execCall = New Exec_Call(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)

                    'Eval args first
                    Call Me.execCall.EvalArgs()

                    Try
                        'Process
                        Me.execCall.Process()

                        'Output
                        For Each o As OutputItem In Me.execCall.OutputList
                            If PackageBuilder.SuppressOutput Then
                                'Pass on to collection for calling assembly
                                Me.OutputList.AddKeepOriginalBasePath(o)
                            Else
                                'Write to disk
                                Call o.Write()
                                RaiseEvent OutputWritten(o.Path)
                            End If
                        Next

                    Catch ex As FindTemplateException
                        Dim caller As String = "MAIN"
                        If Not String.IsNullOrEmpty(Me.TemplateContext) Then caller = "'" & Me.TemplateContext & "'"
                        Throw New CallTemplateException("Error in " & caller & " on line " & ExecNode.LineNumber.ToString & "; " & ex.Message)

                    Catch ex As CallTemplateException
                        Throw ex

                    Catch ex As ExecException
                        Throw New CallTemplateException("Error in '" & Me.execCall.TemplateCalled & "'; " & ex.Message)

                    Catch ex As Exception
                        Throw New CallTemplateException("Error in '" & Me.execCall.TemplateCalled & "'; " & ex.Message)

                    End Try

                Case SyntaxNode.ExecActions.ACTION_HEADER
                    'Ignore, headers are handled prior to the template being called in 'Parser.Syntax.Execution'

                Case SyntaxNode.ExecActions.ACTION_FOR
                    'For loop
                    Me.execFor = New Exec_For(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)
                    sb.Append(Me.execFor.Process().ToString)

                    'Pass on output
                    For Each o As OutputItem In Me.execFor.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'Return encounters
                    Me.UserReturnPath = Me.execFor.UserReturnPath
                    Me.UserReturnPathSet = Me.execFor.UserReturnPathSet

                Case SyntaxNode.ExecActions.ACTION_IF
                    'If block
                    Me.execIf = New Exec_If(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)
                    sb.Append(Me.execIf.Process().ToString)

                    'Pass on output
                    For Each o As OutputItem In Me.execIf.OutputList
                        Me.OutputList.Add(o)
                    Next

                    'Return encounters
                    Me.UserReturnPath = Me.execIf.UserReturnPath
                    Me.UserReturnPathSet = Me.execIf.UserReturnPathSet

                Case SyntaxNode.ExecActions.ACTION_ELSE, SyntaxNode.ExecActions.ACTION_ELSEIF, SyntaxNode.ExecActions.ACTION_END
                    'Ignore

                Case SyntaxNode.ExecActions.ACTION_SET
                    'Set variable
                    Dim proc As New Exec_Set(Me.ExecNode, Me.TemplateContext, Me.ScopeDepth)
                    Call proc.Process()

                Case SyntaxNode.ExecActions.ACTION_EXITWHEN
                    'Mark exit loop according to expression
                    Dim result As Object = EvalExpression(Me.ExecNode.Tokens, 2, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
                    Me.UserExitLoop = CBool(result)

                Case SyntaxNode.ExecActions.SYS_RETURN
                    'Return
                    Me.UserReturnPath = New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).Return()
                    Me.UserReturnPathSet = True
                    Return sb

                Case SyntaxNode.ExecActions.SYS_OUT
                    'Out
                    sb.Append(New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).Out())

                Case SyntaxNode.ExecActions.SYS_OUTLN
                    'OutLn
                    sb.Append(New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).OutLn())

                Case SyntaxNode.ExecActions.SYS_COUT
                    'Console Out
                    RaiseEvent ConsoleOut(New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).COut())

                Case SyntaxNode.ExecActions.SYS_COUTLN
                    'Console OutLn
                    RaiseEvent ConsoleOut(New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).COutLn())

                Case SyntaxNode.ExecActions.SYS_CLCON
                    'Clear console
                    RaiseEvent ConsoleClear()

                Case SyntaxNode.ExecActions.SYS_MAKEDIR
                    'Create directory
                    Call New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).MakeDir()

                Case SyntaxNode.ExecActions.SYS_FILECOPY
                    'File copy
                    Call New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).FileCopy()

                Case SyntaxNode.ExecActions.SYS_COMMAND
                    'Execute command line
                    Call New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).Command()

                Case SyntaxNode.ExecActions.SYS_RUNSCRIPT
                    'Execute script
                    Call New Exec_Sys(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth).RunScript()

                Case Else
                    'Attempt to evaluate expression
                    sb.Append(EvalExpression(Me.ExecNode.Tokens, Me.TemplateContext, Me.ScopeDepth, ExitBlock))

            End Select

            Return sb
        End Function

        Private Sub Exec_Notify(ByVal Message As String) Handles execCall.Notify, execBase.Notify, execFor.Notify, execIf.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub Exec_OutputWritten(ByVal Path As String) Handles execCall.OutputWritten, execBase.OutputWritten, execFor.OutputWritten, execIf.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub Exec_ConsoleOut(ByVal Message As String) Handles execCall.ConsoleOut, execBase.ConsoleOut, execFor.ConsoleOut, execIf.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub Exec_ConsoleClear() Handles execCall.ConsoleClear, execBase.ConsoleClear, execFor.ConsoleClear, execIf.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace