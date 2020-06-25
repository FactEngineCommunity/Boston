Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class Exec_Expr

        Public Shared ExitBlock As Boolean = False

        Public Shared Function EvalExpression(ByVal Tokens As SyntaxTokenCollection, _
                                              ByVal TemplateContext As String, _
                                              ByVal ScopeDepth As Integer, _
                                              Optional ByRef ExitBlock As Boolean = False) As Object
            If Tokens Is Nothing Then Return Nothing
            If Tokens.Count = 0 Then Return Nothing

            Exec_Expr.ExitBlock = ExitBlock
            Dim evals As List(Of Object) = EvalExpressionIntoParameters(Tokens, TemplateContext, ScopeDepth, True, ExitBlock)
            'Expecting only one value
            If evals.Count = 0 Then
                Return Nothing
            ElseIf evals.Count = 1 Then
                Return evals(0)
            Else
                Throw New Exception("Syntax error.")
            End If
        End Function

        Public Shared Function EvalExpression(ByVal Tokens As SyntaxTokenCollection, _
                                              ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer, _
                                              ByVal TemplateContext As String, _
                                              ByVal ScopeDepth As Integer, _
                                              Optional ByRef ExitBlock As Boolean = False) As Object
            If Tokens Is Nothing Then Return Nothing
            If Tokens.Count = 0 Then Return Nothing

            Exec_Expr.ExitBlock = ExitBlock
            Dim evals As List(Of Object) = EvalExpressionIntoParameters(Tokens, TokenIdxStart, TokenIdxEnd, TemplateContext, ScopeDepth, True, ExitBlock)
            'Expecting only one value
            If evals.Count = 0 Then
                Return Nothing
            ElseIf evals.Count = 1 Then
                Return evals(0)
            Else
                Throw New Exception("Syntax error.")
            End If
        End Function

        'Evaluate expression in comma separated parameter values
        Public Shared Function EvalExpressionIntoParameters(ByVal Tokens As SyntaxTokenCollection, _
                                                            ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer, _
                                                            ByVal TemplateContext As String, _
                                                            ByVal ScopeDepth As Integer, _
                                                            ByVal ConvertVariablesIntoPrimitve As Boolean, _
                                                            Optional ByRef ExitBlock As Boolean = False) As List(Of Object)
            If Tokens Is Nothing Then Return Nothing
            If Tokens.Count = 0 Then Return Nothing

            Dim EvalTokens As New EvalTokens(Tokens, ConvertVariablesIntoPrimitve, TemplateContext, ScopeDepth)
            Dim ret As List(Of Object) = EvalTokens.GetValues(TokenIdxStart, TokenIdxEnd)
            ExitBlock = Exec_Expr.ExitBlock
            Return ret
        End Function

        'Evaluate expression in comma separated parameter values
        Public Shared Function EvalExpressionIntoParameters(ByVal Tokens As SyntaxTokenCollection, _
                                                            ByVal TemplateContext As String, _
                                                            ByVal ScopeDepth As Integer, _
                                                            ByVal ConvertVariablesIntoPrimitve As Boolean, _
                                                            Optional ByRef ExitBlock As Boolean = False) As List(Of Object)
            If Tokens Is Nothing Then Return Nothing
            If Tokens.Count = 0 Then Return Nothing

            Dim EvalTokens As New EvalTokens(Tokens, ConvertVariablesIntoPrimitve, TemplateContext, ScopeDepth)
            Dim ret As List(Of Object) = EvalTokens.GetValues()
            ExitBlock = Exec_Expr.ExitBlock
            Return ret
        End Function

    End Class

End Namespace