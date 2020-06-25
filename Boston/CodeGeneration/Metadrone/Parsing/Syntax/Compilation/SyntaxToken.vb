Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr

Namespace Parser.Syntax

    <Serializable()> Friend Class SyntaxToken
        Public Enum ElementTypes
            [NotSet] = 0
            [Operator] = 1
            [Comparison] = 2
            [EqualSign] = 3
            [LogicOperator] = 4
            [LeftParen] = 5
            [RightParen] = 6
            [ParamSep] = 7
            [String] = 8
            [Number] = 9
            [Boolean] = 10
            [Reserved] = 11
            [Variable] = 12
            [Preproc] = 13
        End Enum

        Public Enum TransformTargets
            NotSet = 0
            Table = 1
            Routine = 2
            Column = 3
            Param = 4
        End Enum

        Public Text As String = Nothing
        Public CodeLineNumber As Integer = -1
        Public QualifiedExpressionTokens As New SyntaxTokenCollection()

        Public TransformTarget As TransformTargets = TransformTargets.NotSet

        Public Value As Object = Nothing
        Public Type As ElementTypes = ElementTypes.NotSet

        Public Sub New()

        End Sub

        'For plain text tokens
        Public Sub New(ByVal plainTextTokenText As String, ByVal nodeLineNumber As Integer)
            If plainTextTokenText Is Nothing Then plainTextTokenText = "" 'No null ref issues
            Me.Text = plainTextTokenText
            Me.CodeLineNumber = nodeLineNumber
        End Sub

        'For code tokens
        Public Sub New(ByVal tokenText As String, ByVal nodeLineNumber As Integer, ByVal overrideType As ElementTypes)
            Call Me.New(tokenText, nodeLineNumber)

            'Determine type
            If overrideType = ElementTypes.NotSet Then
                Me.Type = Me.GetValueType(Me.Text)
            Else
                Me.Type = overrideType
            End If

            'Don't bother getting type if zero length
            If Me.Text.Length = 0 Then
                'Enforce empty string if overridden as a string
                If Me.Type = ElementTypes.String Then Me.Value = ""
                Exit Sub
            End If

            Select Case Me.Type
                Case ElementTypes.Operator : Me.Value = Me.Text
                Case ElementTypes.Comparison : Me.Value = Me.Text
                Case ElementTypes.EqualSign : Me.Value = Me.Text
                Case ElementTypes.LogicOperator : Me.Value = Me.Text
                Case ElementTypes.LeftParen : Me.Value = Me.Text
                Case ElementTypes.RightParen : Me.Value = Me.Text
                Case ElementTypes.ParamSep : Me.Value = Me.Text
                Case ElementTypes.Number : Me.Value = CDbl(Me.Text)

                Case ElementTypes.String
                    If overrideType = ElementTypes.NotSet Then
                        Me.Value = RemQuotes(Me.Text)
                    Else
                        Me.Value = Me.Text
                    End If

                Case ElementTypes.Boolean
                    If StrEq(Me.Text, RESERVED_TRUE) Then
                        Me.Value = True
                    Else
                        Me.Value = False
                    End If

                Case ElementTypes.Variable
                    Me.Value = Nothing 'Evaluate later

                    'Split member qualifiers
                    If Me.Text.Equals(".") Then Exit Sub
                    Dim spl As List(Of String) = Me.Text.Split(".".ToCharArray).ToList
                    If spl.Count > 1 Then
                        For i As Integer = 0 To spl.Count - 1
                            Me.QualifiedExpressionTokens.Add(New SyntaxToken(spl(i), nodeLineNumber, ElementTypes.NotSet))
                        Next
                    End If

            End Select
        End Sub

        Private Function GetValueType(ByVal Expression As String) As ElementTypes
            If Expression.Equals("^") Or Expression.Equals("*") Or Expression.Equals("/") Or Expression.Equals("\") Or _
               Expression.Equals("%") Or Expression.Equals("+") Or Expression.Equals("-") Or Expression.Equals("&") Then
                'Operator
                Return ElementTypes.Operator

            ElseIf Expression.Equals("<>") Or Expression.Equals("<") Or Expression.Equals("<=") Or _
                   Expression.Equals(">") Or Expression.Equals(">=") Then
                'Comparison
                Return ElementTypes.Comparison

            ElseIf Expression.Equals("=") Then
                'Assignment (or equivalence) equal sign
                Return ElementTypes.EqualSign

            ElseIf StrEq(Expression, RESERVED_AND) Or StrEq(Expression, RESERVED_OR) Then
                'Logic operator
                Return ElementTypes.LogicOperator

            ElseIf Expression.Equals("(") Then
                Return ElementTypes.LeftParen

            ElseIf Expression.Equals(")") Then
                Return ElementTypes.RightParen

            ElseIf Expression.Equals(",") Then
                'Parameter separator
                Return ElementTypes.ParamSep

            ElseIf StrEq(Expression, RESERVED_TRUE) Or StrEq(Expression, RESERVED_FALSE) Then
                'Boolean
                Return ElementTypes.Boolean

            ElseIf Microsoft.VisualBasic.IsNumeric(Expression) Then
                'Numeric value
                Return ElementTypes.Number

            ElseIf Expression.IndexOf("""") = 0 And Expression.LastIndexOf("""") = Expression.Length - 1 Then
                'String
                Return ElementTypes.String

            ElseIf IsReservedWord(Expression) Then
                Return ElementTypes.Reserved

            ElseIf StrEq(Expression, RESERVED_PREPROC) Then
                Return ElementTypes.Preproc

            Else
                'Check for funky chars
                If IsOperator(Expression) Then
                    Throw New Exception("Invalid character. Line: " & Me.CodeLineNumber.ToString & ".")
                End If

                'Variable
                Return ElementTypes.Variable

            End If
        End Function

        Public Sub SetTransformTarget()
            If Me.QualifiedExpressionTokens.Count <> 2 Then Exit Sub

            Dim exprLeft As String = Me.QualifiedExpressionTokens(0).Text
            If StrEq(exprLeft, SyntaxToken.TransformTargets.Table.ToString) Then
                Me.TransformTarget = TransformTargets.Table

            ElseIf StrEq(exprLeft, SyntaxToken.TransformTargets.Routine.ToString) Then
                Me.TransformTarget = TransformTargets.Routine

            ElseIf StrEq(exprLeft, SyntaxToken.TransformTargets.Column.ToString) Then
                Me.TransformTarget = TransformTargets.Column

            ElseIf StrEq(exprLeft, SyntaxToken.TransformTargets.Param.ToString) Then
                Me.TransformTarget = TransformTargets.Param

            Else
                Dim sb As New System.Text.StringBuilder("Use '" & SyntaxToken.TransformTargets.Table.ToString.ToLower & "', ")
                sb.Append("'" & SyntaxToken.TransformTargets.Routine.ToString.ToLower & "', ")
                sb.Append("'" & SyntaxToken.TransformTargets.Column.ToString.ToLower & "', ")
                sb.Append("or '" & SyntaxToken.TransformTargets.Param.ToString.ToLower & "'.")
                Throw New Exception(sb.ToString)

            End If
        End Sub
    End Class





#Region "SyntaxTokenCollection"

    <Serializable()> Friend Class SyntaxTokenCollection
        Inherits Collections.CollectionBase

        Public Sub New()

        End Sub

        Public Sub Add(ByVal item As SyntaxToken)
            Me.List.Add(item)
        End Sub

        Public Sub Remove(ByVal index As Integer)
            Me.List.RemoveAt(index)
        End Sub

        Public ReadOnly Property ItemList() As ArrayList
            Get
                Return MyBase.InnerList
            End Get
        End Property

        Default Public Overloads ReadOnly Property Item(ByVal index As Integer) As SyntaxToken
            Get
                Return CType(Me.List(index), SyntaxToken)
            End Get
        End Property

        Public Function IndexOf(ByVal item As String) As Integer
            For i As Integer = 0 To Me.List.Count - 1
                If StrEq(CType(Me.List(i), SyntaxToken).Text, item) Then Return i
            Next
            Return -1
        End Function

        Public Function GetCopy() As SyntaxTokenCollection
            Dim ret As New SyntaxTokenCollection()
            For Each t As SyntaxToken In Me
                Dim tok As New SyntaxToken(t.Text, t.CodeLineNumber)
                tok.QualifiedExpressionTokens = t.QualifiedExpressionTokens.GetCopy()
                tok.TransformTarget = t.TransformTarget
                tok.Type = t.Type
                tok.Value = t.Value

                ret.Add(tok)
            Next
            Return ret
        End Function

    End Class

#End Region


End Namespace