Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class EvalTokens
        'Based upon code from http://www.vb-helper.com/howto_2005_evaluate_expressions.html

        Private Enum Precedence
            None = 11
            Unary = 10   ' Not actually used.
            Power = 9
            Times = 8
            Div = 7
            IntDiv = 6
            Modulus = 5
            Plus = 4
            Comparison = 3
            Logic = 2
            ParamSep = 1  ' Parameter separator
        End Enum

        Private Tokens As SyntaxTokenCollection = Nothing
        Private ConvertVariablesIntoPrimitve As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0


        Public Sub New(ByVal Tokens As SyntaxTokenCollection, ByVal ConvertVariablesIntoPrimitve As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.Tokens = Tokens
            Me.ConvertVariablesIntoPrimitve = ConvertVariablesIntoPrimitve
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
        End Sub

        Public Function GetValues() As List(Of Object)
            Return Me.Evaluate(0, Me.Tokens.Count - 1)
        End Function

        Public Function GetValues(ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer) As List(Of Object)
            Return Me.Evaluate(TokenIdxStart, TokenIdxEnd)
        End Function

        Private Function Evaluate(ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer) As List(Of Object)
            'Don't bother parsing if range invalid..
            If TokenIdxStart > Me.Tokens.Count - 1 Or _
               TokenIdxEnd > Me.Tokens.Count - 1 Or _
               TokenIdxStart > TokenIdxEnd Then
                Return New List(Of Object)
            End If

            'Don't bother parsing if only one token..
            If TokenIdxStart = TokenIdxEnd Then Return Me.GetLiteral(TokenIdxStart, TokenIdxEnd)

            ' If we find + or - now, it is a unary operator.
            Dim is_unary As Boolean = True

            ' So far we have nothing.
            Dim best_prec As Precedence = Precedence.None
            Dim best_pos As Integer = TokenIdxStart

            ' Find the operator with the lowest precedence.
            ' Look for places where there are no open
            ' parentheses.
            Dim parens As Integer = 0
            Dim next_unary As Boolean = False
            For tokenIdx As Integer = TokenIdxStart To TokenIdxEnd
                ' Assume we will not find an operator. In
                ' that case, the next operator will not
                ' be unary.
                next_unary = False

                ' Examine the next character.
                If Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.LeftParen Then
                    ' Increase the open parentheses count.
                    parens = parens + 1

                    ' A + or - after "(" is unary.
                    next_unary = True

                ElseIf Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.RightParen Then
                    ' Decrease the open parentheses count.
                    parens = parens - 1

                    ' An operator after ")" is not unary.
                    next_unary = False

                    ' If parens < 0, too many ')'s.
                    If parens < 0 Then
                        Throw New NotSupportedException("Too many " & ")s in expression '" & Strings.TokensToString(Me.Tokens, TokenIdxStart, TokenIdxEnd) & "'")
                    End If

                ElseIf parens = 0 Then
                    ' See if this is an operator.
                    If Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.Operator Then
                        ' An operator after an operator
                        ' is unary.
                        next_unary = True

                        ' See if this operator has higher
                        ' precedence than the current one.
                        Select Case Me.Tokens(tokenIdx).Text
                            Case "^"
                                If best_prec >= Precedence.Power Then
                                    best_prec = Precedence.Power
                                    best_pos = tokenIdx
                                End If

                            Case "*", "/"
                                If best_prec >= Precedence.Times Then
                                    best_prec = Precedence.Times
                                    best_pos = tokenIdx
                                End If

                            Case "\"
                                If best_prec >= Precedence.IntDiv Then
                                    best_prec = Precedence.IntDiv
                                    best_pos = tokenIdx
                                End If

                            Case "%"
                                If best_prec >= Precedence.Modulus Then
                                    best_prec = Precedence.Modulus
                                    best_pos = tokenIdx
                                End If

                            Case "+", "-"
                                ' Ignore unary operators
                                ' for now.
                                If (Not is_unary) And _
                                    best_prec >= Precedence.Plus Then
                                    best_prec = Precedence.Plus
                                    best_pos = tokenIdx
                                End If

                            Case "&" 'include string concats
                                If best_prec >= Precedence.Plus Then
                                    best_prec = Precedence.Plus
                                    best_pos = tokenIdx
                                End If

                        End Select

                    ElseIf Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.EqualSign Or _
                           Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.Comparison Then
                        If best_prec >= Precedence.Comparison Then
                            best_prec = Precedence.Comparison
                            best_pos = tokenIdx
                        End If

                    ElseIf Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.LogicOperator Then
                        If best_prec >= Precedence.Logic Then
                            best_prec = Precedence.Logic
                            best_pos = tokenIdx
                        End If

                    ElseIf Me.Tokens(tokenIdx).Type = SyntaxToken.ElementTypes.ParamSep Then
                        If best_prec >= Precedence.ParamSep Then
                            best_prec = Precedence.ParamSep
                            best_pos = tokenIdx
                        End If

                    End If

                End If

                is_unary = next_unary
            Next

            ' If the parentheses count is not zero,
            ' there's a ')' missing.
            If parens <> 0 Then
                Throw New NotSupportedException("Missing ) in " & "expression '" & Strings.TokensToString(Me.Tokens, TokenIdxStart, TokenIdxEnd) & "'")
            End If

            ' Hopefully we have the operator.
            If best_prec < Precedence.None Then
                Dim lexpr As List(Of Object) = Me.Evaluate(TokenIdxStart, best_pos - 1)
                Dim rexpr As List(Of Object) = Me.Evaluate(best_pos + 1, TokenIdxEnd)
                Dim ret As New List(Of Object)

                Select Case Me.Tokens(best_pos).Text.ToLower
                    Case "^"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CDbl(lexpr(0)) ^ CDbl(rexpr(0)))

                    Case "*"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CDbl(lexpr(0)) * CDbl(rexpr(0)))

                    Case "/"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CDbl(lexpr(0)) / CDbl(rexpr(0)))

                    Case "\"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CLng(lexpr(0)) \ CLng(rexpr(0)))

                    Case "%"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CDbl(lexpr(0)) Mod CDbl(rexpr(0)))

                    Case "+" ' Determine if concat string or addition on numerics
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")

                        If IsNumeric(lexpr(0)) And IsNumeric(rexpr(0)) Then
                            ret.Add(CDbl(lexpr(0)) + CDbl(rexpr(0)))
                        Else
                            ret.Add(lexpr(0).ToString & rexpr(0).ToString)
                        End If

                        'If TypeOf lexpr(0) Is String Or TypeOf rexpr(0) Is String Then
                        '    ret.Add(lexpr(0).ToString & rexpr(0).ToString)
                        'Else
                        '    ret.Add(CDbl(lexpr(0)) + CDbl(rexpr(0)))
                        'End If

                    Case "&" ' Concat string
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(lexpr(0).ToString & rexpr(0).ToString)

                    Case "-"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CDbl(lexpr(0)) - CDbl(rexpr(0)))

                    Case "="
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(StrEq(lexpr(0).ToString, rexpr(0).ToString))

                    Case ">"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        If TypeOf lexpr(0) Is String Or TypeOf rexpr(0) Is String Then
                            ret.Add(lexpr(0).ToString > rexpr(0).ToString)
                        Else
                            ret.Add(CDbl(lexpr(0)) > CDbl(rexpr(0)))
                        End If

                    Case ">="
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        If TypeOf lexpr(0) Is String Or TypeOf rexpr(0) Is String Then
                            ret.Add(lexpr(0).ToString >= rexpr(0).ToString)
                        Else
                            ret.Add(CDbl(lexpr(0)) >= CDbl(rexpr(0)))
                        End If

                    Case "<"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        If TypeOf lexpr(0) Is String Or TypeOf rexpr(0) Is String Then
                            ret.Add(lexpr(0).ToString < rexpr(0).ToString)
                        Else
                            ret.Add(CDbl(lexpr(0)) < CDbl(rexpr(0)))
                        End If

                    Case "<="
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        If TypeOf lexpr(0) Is String Or TypeOf rexpr(0) Is String Then
                            ret.Add(lexpr(0).ToString <= rexpr(0).ToString)
                        Else
                            ret.Add(CDbl(lexpr(0)) <= CDbl(rexpr(0)))
                        End If

                    Case "<>"
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(Not StrEq(lexpr(0).ToString, rexpr(0).ToString))

                    Case RESERVED_AND.ToLower
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CBool(lexpr(0)) And CBool(rexpr(0)))

                    Case RESERVED_OR.ToLower
                        If lexpr.Count <> 1 Or rexpr.Count <> 1 Then Throw New Exception("Syntax error.")
                        ret.Add(CBool(lexpr(0)) Or CBool(rexpr(0)))

                    Case "," ' Add to argument list
                        For i As Integer = 0 To lexpr.Count - 1
                            ret.Add(lexpr(i))
                        Next
                        For i As Integer = 0 To rexpr.Count - 1
                            ret.Add(rexpr(i))
                        Next

                    Case Else ' This shouldn't happen.
                        Throw New NotSupportedException("Error evaluating expression '" & Strings.TokensToString(Me.Tokens, TokenIdxStart, TokenIdxEnd) & "'")

                End Select
                Return ret

            End If

            ' If we do not yet have an operator, there
            ' are several possibilities:
            '
            ' 1. expr is (expr2) for some expr2.
            ' 2. expr is -expr2 or +expr2 for some expr2.
            ' 3. expr is not
            ' 4. expr is Fun(expr2) for a function Fun.
            ' 5. expr is a primitive.
            ' 6. It's a literal like "3.14159".

            ' Look for (expr2).
            If Me.Tokens(TokenIdxStart).Type = SyntaxToken.ElementTypes.LeftParen And Me.Tokens(TokenIdxEnd).Type = SyntaxToken.ElementTypes.RightParen Then
                ' Eval within parentheses.
                If TokenIdxStart + 1 > TokenIdxEnd - 1 Then
                    'Empty brackets 
                    Dim ret As New List(Of Object) : ret.Add(Nothing) : Return ret
                Else
                    Return Me.Evaluate(TokenIdxStart + 1, TokenIdxEnd - 1)
                End If
            End If

            ' Look for -expr2.
            If Me.Tokens(TokenIdxStart).Text.Equals("-") Then
                Dim ret As List(Of Object) = Me.Evaluate(TokenIdxStart + 1, TokenIdxEnd)
                If ret.Count <> 1 Then Throw New Exception("Syntax error.")
                ret(0) = -CDbl(ret(0))
                Return ret
            End If

            ' Look for +expr2.
            If Me.Tokens(TokenIdxStart).Text.Equals("+") Then
                Dim ret As List(Of Object) = Me.Evaluate(TokenIdxStart + 1, TokenIdxEnd)
                If ret.Count <> 1 Then Throw New Exception("Syntax error.")
                ret(0) = CDbl(ret(0))
                Return ret
            End If

            ' Look for not expr2.
            If StrEq(Me.Tokens(TokenIdxStart).Text, RESERVED_NOT) Then
                Dim ret As List(Of Object) = Me.Evaluate(TokenIdxStart + 1, TokenIdxEnd)
                If ret.Count <> 1 Then Throw New Exception("Syntax error.")
                ret(0) = Not CBool(ret(0))
                Return ret
            End If

            ' Look for Func(expr2).
            'TODO, may need to optimise text comparison later (enum to symbol table)
            If TokenIdxStart < TokenIdxEnd AndAlso _
               Me.Tokens(TokenIdxStart + 1).Type = SyntaxToken.ElementTypes.LeftParen And Me.Tokens(TokenIdxEnd).Type = SyntaxToken.ElementTypes.RightParen Then
                Dim args As List(Of Object) = Me.Evaluate(TokenIdxStart + 1, TokenIdxEnd)
                Dim ret As New List(Of Object)
                Select Case Me.Tokens(TokenIdxStart).Text.ToLower
                    Case SYS_MATHS_SIN.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(Math.Sin(CDbl(args(0))))

                    Case SYS_MATHS_COS.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(Math.Cos(CDbl(args(0))))

                    Case SYS_MATHS_TAN.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(Math.Tan(CDbl(args(0))))

                    Case SYS_MATHS_SQRT.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(Math.Sqrt(CDbl(args(0))))

                    Case SYS_CNUM.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(CDbl(args(0)))

                    Case SYS_RUNVB.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(PackageBuilder.CodeDOM.RunVB(args(0).ToString))

                    Case SYS_RUNCS.ToLower
                        If args.Count = 0 Then Throw New Exception("Expecting argument expression.")
                        If args.Count > 1 Then Throw New Exception("Too many arguments.")
                        ret.Add(PackageBuilder.CodeDOM.RunCS(args(0).ToString))

                    Case Else
                        'User defined
                        Return Me.GetVar(Me.Tokens(TokenIdxStart).Text, TokenIdxStart, args)

                End Select

                Return ret
            End If

            'Unknown syntax
            If TokenIdxStart <> TokenIdxEnd Then Throw New Exception("Syntax error.")

            ' It must be a literal like "2.71828".
            ' Try to return it and let any errors throw.
            Return Me.GetLiteral(TokenIdxStart, TokenIdxEnd)
        End Function

        Private Function GetLiteral(ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer) As List(Of Object)
            'Primitive, return the token value
            Select Case Me.Tokens(TokenIdxStart).Type
                Case SyntaxToken.ElementTypes.Boolean, SyntaxToken.ElementTypes.Number, SyntaxToken.ElementTypes.String
                    Dim ret As New List(Of Object)
                    ret.Add(Me.Tokens(TokenIdxStart).Value)
                    Return ret

            End Select

            'Evaluate variable
            Return Me.GetVar(Me.Tokens(TokenIdxStart).Text, TokenIdxStart, New List(Of Object)) '20200703-VM-Was Nothing for Params
        End Function

        Private Function GetVar(ByVal funcName As String, ByVal tokenIdx As Integer, ByVal params As List(Of Object)) As List(Of Object)
            Dim attrib As String = ""
            If Me.Tokens(tokenIdx).QualifiedExpressionTokens.Count <= 1 Then
                'Just use val (no qualifications)

            ElseIf Me.Tokens(tokenIdx).QualifiedExpressionTokens.Count = 2 Then
                'Val is first, attrib next
                funcName = Me.Tokens(tokenIdx).QualifiedExpressionTokens(0).Text
                attrib = Me.Tokens(tokenIdx).QualifiedExpressionTokens(1).Text
            End If
            If StrEq(funcName, GLOBALS_SOURCES) Then
                'System variable - source
                If attrib.Length = 0 Then Throw New Exception("Source expected.")

                'error if params
                If params IsNot Nothing Then
                    If params.Count = 1 Then
                        If params(0) IsNot Nothing Then Throw New Exception("Too many arguments in '" & funcName & "." & attrib & "'.")
                    ElseIf params.Count > 1 Then
                        Throw New Exception("Too many arguments in '" & funcName & "." & attrib & "'.")
                    End If
                End If

                'Get from project
                If PackageBuilder.Sources Is Nothing Then Throw New Exception("Sources not initialised.")
                Dim src As Source.Source = PackageBuilder.Sources.Item(attrib)
                If src Is Nothing Then Throw New Exception("Source '" & attrib & "' could not be found.")

                'Add to connections, set source object
                If PackageBuilder.Connections Is Nothing Then Throw New Exception("PackageBuilder not initialised.")
                PackageBuilder.Connections.Add(src, Connections.LoadModes.None)

                Dim ret As New List(Of Object)
                ret.Add(src)
                Return ret

            Else
                Return Me.GetVariable(PackageBuilder.Variables.Item(Me.TemplateContext, funcName, Me.ScopeDepth), attrib, params)

            End If
        End Function

        Private Function GetVariable(ByVal var As Variable, ByVal attrib As String, ByVal params As List(Of Object)) As List(Of Object)
            Select Case var.Type
                Case Variable.Types.Null
                    Throw New Exception("Reference to null value.")

                Case Variable.Types.Variable 'variable
                    Dim ret As New List(Of Object)

                    'If the variable value is a variable, drill down, keep trying
                    If TypeOf var.Value Is Variable Then
                        For Each o In Me.GetVariable(CType(var.Value, Variable), attrib, params)
                            ret.Add(o)
                        Next
                        Return ret
                    End If

                    'Just the actual variable
                    If attrib.Length = 0 And Not Me.ConvertVariablesIntoPrimitve Then
                        ret.Add(var)
                        Return ret
                    End If

                    'Handle null
                    If var.Value Is Nothing Then
                        ret.Add(Nothing)
                        Return ret
                    End If

                    'Eval variable value
                    If TypeOf var.Value Is IEntity Then
                        'Evaluate attribute
                        ret.Add(CType(var.Value, IEntity).GetAttributeValue(attrib, params, True, Exec_Expr.ExitBlock))
                    Else
                        ret.Add(var.Value)
                    End If

                    Return ret

                Case Else '(primitives, or sources)
                    'Just the variable value
                    Dim ret As New List(Of Object)
                    ret.Add(var.Value)
                    Return ret

            End Select
        End Function

    End Class

End Namespace