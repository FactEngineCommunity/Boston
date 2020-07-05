Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr

Namespace Parser.Syntax

    <Serializable()> Friend Class SyntaxNode

        Private AsMain As Boolean = False

        <Serializable()> Public Class IfBranchDef
            Public ElseNodeIndex As Integer = -1
            Public ElseIfNodeIndexes As New List(Of Integer)

            Public Sub New()
                Me.ElseIfNodeIndexes = New List(Of Integer)
            End Sub

            Public Function GetCopy() As IfBranchDef
                Dim cbc As New IfBranchDef()
                cbc.ElseNodeIndex = Me.ElseNodeIndex
                For Each i In Me.ElseIfNodeIndexes
                    cbc.ElseIfNodeIndexes.Add(i)
                Next
                Return cbc
            End Function
        End Class

        <Serializable()> Public Class Parameter
            Public Name As String
            Public Value As Object

            Public Sub New()

            End Sub

            Public Sub New(ByVal Name As String, ByVal Value As Object)
                Me.Name = Name : Me.Value = Value
            End Sub
        End Class


        Public Text As String
        Public NotStrict As Boolean = False

        Public TemplateName As String
        Public TemplateParameters As New List(Of Parameter)
        Public Tokens As New SyntaxTokenCollection()

        Public Action As ExecActions = ExecActions.PLAINTEXT
        Public ForEntity As ExecForEntities = ExecForEntities.NULL
        Public SetSubj As String = ""
        Public SetSubjAttrib As String = ""

        Public LineNumber As Integer
        Public Nodes As New List(Of SyntaxNode)

        Public Owner_TemplateParamNames As New List(Of String)
        Public Owner_TemplateParamValues As New List(Of Object)

        Public IfBranch As New IfBranchDef()

        Public Sub New()

        End Sub

        Friend Sub New(ByVal Owner As SyntaxNode, ByVal AsMain As Boolean, Optional ByVal NotStrict As Boolean = False)
            Me.New(ACTION_ROOT, False, -1, AsMain, Owner, NotStrict)
        End Sub

        Friend Sub New(ByVal Text As String, ByVal isPlainText As Boolean, ByVal LineNumber As Integer, _
                       ByVal AsMain As Boolean, ByVal Owner As SyntaxNode, _
                       Optional ByVal NotStrict As Boolean = False)
            If isPlainText Then
                Me.Tokens.Add(New SyntaxToken(Text, LineNumber))
                Me.Text = Text
            Else
                Me.Tokens = Tokenise(Text, LineNumber)
                Me.Text = TokensToString(Me.Tokens, 0, Me.Tokens.Count - 1)
            End If
            Me.LineNumber = LineNumber

            'All done if just plain text
            If isPlainText Then Exit Sub

            'Determine action
            Me.AsMain = AsMain
            Me.NotStrict = NotStrict
            Call Me.SetAction()

            'Set owner values
            Me.SetOwner(Owner)
        End Sub

        Public Sub SetOwner(ByVal Owner As SyntaxNode)
            If Owner IsNot Nothing Then
                For Each p In Owner.TemplateParameters
                    Me.Owner_TemplateParamNames.Add(p.Name)
                    Me.Owner_TemplateParamValues.Add(p.Value)
                Next
            End If
        End Sub

        'Remove any comment tokens after code
        Private Sub StripCommentTokensAfterCode()
            For i As Integer = 1 To Me.Tokens.Count - 1
                If StrEq(Me.Tokens(i).Text, RESERVED_COMMENT_LINE) Then
                    While i < Me.Tokens.Count
                        Me.Tokens.Remove(i)
                    End While
                    Exit Sub
                End If
            Next
        End Sub

        Private Sub SetAction()
            If Me.Tokens.Count = 0 Then Exit Sub

            Call Me.StripCommentTokensAfterCode()
            Dim tok As String = Me.Tokens(0).Text
            If StrEq(tok, ACTION_ROOT) Then
                Me.Action = ExecActions.ACTION_ROOT

            ElseIf tok.IndexOf(RESERVED_COMMENT_LINE) = 0 Then
                Me.Action = ExecActions.COMMENT

            ElseIf Me.Tokens(0).Type = SyntaxToken.ElementTypes.Preproc Then
                'Only parse preprocessors if main
                If Not Me.AsMain Then Throw New Exception("Statement only valid in main. Line: " & Me.LineNumber.ToString)

                'Need statement
                If Me.Tokens.Count < 2 Then Throw New Exception("Preprocessor statement expected.")

                tok = Me.Tokens(1).Text
                If StrEq(tok, RESERVED_PREPROC_IGNORECASE) Then
                    '#ignorecase
                    Dim sbErr As New System.Text.StringBuilder("Syntax error. Use '" & RESERVED_PREPROC_IGNORECASE & " " & RESERVED_PREPROC_IGNORECASE_ON & "'")
                    sbErr.Append(" or '" & RESERVED_PREPROC_IGNORECASE & " " & RESERVED_PREPROC_IGNORECASE_OFF & "'. Line: " & Me.LineNumber.ToString)
                    If Me.Tokens.Count < 3 Then Throw New Exception(sbErr.ToString)
                    If Me.Tokens.Count > 3 AndAlso Not StrEq(Me.Tokens(3).Text, RESERVED_COMMENT_LINE) Then Throw New Exception(sbErr.ToString)

                    If StrEq(Me.Tokens(2).Text, RESERVED_PREPROC_IGNORECASE_ON) Then
                        'Ok
                    ElseIf StrEq(Me.Tokens(2).Text, RESERVED_PREPROC_IGNORECASE_OFF) Then
                        'Ok
                    Else
                        Throw New Exception(sbErr.ToString)
                    End If

                    'Preprocessor ok
                    Me.Action = ExecActions.PREPROC_IGNORECASE

                ElseIf StrEq(tok, RESERVED_PREPROC_SAFEBEGIN) Then
                    '#safebegin
                    Dim sbErr As New System.Text.StringBuilder("Syntax error. Use '" & RESERVED_PREPROC_SAFEBEGIN & " = ""string value""")
                    sbErr.Append("'. Line: " & Me.LineNumber.ToString)
                    If Me.Tokens.Count < 4 Then Throw New Exception(sbErr.ToString)
                    If Me.Tokens.Count > 4 AndAlso Not StrEq(Me.Tokens(4).Text, RESERVED_COMMENT_LINE) Then Throw New Exception(sbErr.ToString)
                    If Me.Tokens.IndexOf("=") <> 2 Then Throw New Exception(sbErr.ToString)

                    'Preprocessor ok
                    Me.Action = ExecActions.PREPROC_SAFEBEGIN

                ElseIf StrEq(tok, RESERVED_PREPROC_SAFEEND) Then
                    '#safeend
                    Dim sbErr As New System.Text.StringBuilder("Syntax error. Use '" & RESERVED_PREPROC_SAFEEND & " = ""string value""")
                    sbErr.Append("'. Line: " & Me.LineNumber.ToString)
                    If Me.Tokens.Count < 4 Then Throw New Exception(sbErr.ToString)
                    If Me.Tokens.Count > 4 AndAlso Not StrEq(Me.Tokens(4).Text, RESERVED_COMMENT_LINE) Then Throw New Exception(sbErr.ToString)
                    If Me.Tokens.IndexOf("=") <> 2 Then Throw New Exception(sbErr.ToString)

                    'Preprocessor ok
                    Me.Action = ExecActions.PREPROC_SAFEEND

                Else
                    Throw New Exception("Unknown preprocessor '" & tok & "'. Line: " & Me.LineNumber.ToString)

                End If

            ElseIf StrEq(tok, ACTION_HEADER) Then
                'Header only for templates
                If Me.AsMain Then Throw New Exception("Statement only valid in templates. Line: " & Me.LineNumber.ToString)

                If Not Me.NotStrict Then
                    If Me.Tokens.Count > 1 Then
                        Throw New Exception("Syntax error. Line: " & Me.LineNumber.ToString)
                    End If
                End If

                Me.Action = ExecActions.ACTION_HEADER

            ElseIf StrEq(tok, HEADER_IS) Then
                'Header only for templates
                If Me.AsMain Then Throw New Exception("Statement only valid in templates. Line: " & Me.LineNumber.ToString)

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 2 Then Throw New Exception("Syntax error. Expecting template identifier. Line: " & Me.LineNumber.ToString)
                End If

                Me.Action = ExecActions.HEADER_IS

            ElseIf StrEq(tok, ACTION_CALL) Then
                Me.Action = ExecActions.ACTION_CALL

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 2 Then Throw New Exception("Name of object to call expected. Line: " & Me.LineNumber.ToString)
                End If

            ElseIf StrEq(tok, ACTION_FOR) Then
                Me.Action = ExecActions.ACTION_FOR

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 5 Then Throw New Exception("Syntax error in for construct. Line: " & Me.LineNumber.ToString)
                    If Not StrEq(Me.Tokens(3).Text, ACTION_IN) Then Throw New Exception("Syntax error in for construct. Line: " & Me.LineNumber.ToString)
                    Select Case Me.Tokens(2).Type
                        Case SyntaxToken.ElementTypes.Variable
                            If ContainsOperator(Me.Tokens(2).Text) Then
                                Throw New Exception("'" & Me.Tokens(2).Text & "' is not a valid identifier name. Line: " & Me.LineNumber.ToString)
                            End If
                        Case Else
                            Throw New Exception("'" & Me.Tokens(2).Text & "' is not a valid identifier name. Line: " & Me.LineNumber.ToString)
                    End Select
                End If

                If Me.Tokens.Count > 1 Then
                    Dim tokObject As String = Me.Tokens(1).Text
                    If StrEq(tokObject, OBJECT_TABLE) Then
                        Me.ForEntity = ExecForEntities.OBJECT_TABLE

                    ElseIf StrEq(tokObject, OBJECT_VIEW) Then
                        Me.ForEntity = ExecForEntities.OBJECT_VIEW

                    ElseIf StrEq(tokObject, OBJECT_RELATIONS) Then 'Boston specific. Not part of original Metadrone.
                        Me.ForEntity = ExecForEntities.OBJECT_RELATIONS

                    ElseIf StrEq(tokObject, OBJECT_RELATION) Then 'Boston specific. Not part of original Metadrone.
                        Me.ForEntity = ExecForEntities.OBJECT_RELATION

                    ElseIf StrEq(tokObject, OBJECT_COLUMN) Then
                        Me.ForEntity = ExecForEntities.OBJECT_COLUMN

                    ElseIf StrEq(tokObject, OBJECT_PKCOLUMN) Then
                        Me.ForEntity = ExecForEntities.OBJECT_PKCOLUMN

                    ElseIf StrEq(tokObject, OBJECT_FKCOLUMN) Then
                        Me.ForEntity = ExecForEntities.OBJECT_FKCOLUMN

                    ElseIf StrEq(tokObject, OBJECT_IDCOLUMN) Then
                        Me.ForEntity = ExecForEntities.OBJECT_IDCOLUMN

                    ElseIf StrEq(tokObject, OBJECT_FILE) Then
                        Me.ForEntity = ExecForEntities.OBJECT_FILE

                    ElseIf StrEq(tokObject, OBJECT_PROCEDURE) Then
                        Me.ForEntity = ExecForEntities.OBJECT_PROCEDURE

                    ElseIf StrEq(tokObject, OBJECT_FUNCTION) Then
                        Me.ForEntity = ExecForEntities.OBJECT_FUNCTION

                    ElseIf StrEq(tokObject, OBJECT_PARAMETER) Then
                        Me.ForEntity = ExecForEntities.OBJECT_PARAMETER

                    ElseIf StrEq(tokObject, OBJECT_INPARAMETER) Then
                        Me.ForEntity = ExecForEntities.OBJECT_INPARAMETER

                    ElseIf StrEq(tokObject, OBJECT_OUTPARAMETER) Then
                        Me.ForEntity = ExecForEntities.OBJECT_OUTPARAMETER

                    ElseIf StrEq(tokObject, OBJECT_INOUTPARAMETER) Then
                        Me.ForEntity = ExecForEntities.OBJECT_INOUTPARAMETER

                    Else
                        If Not Me.NotStrict Then Throw New Exception("Unknown entity '" & tokObject & "'. Line: " & Me.LineNumber.ToString)

                    End If
                End If

            ElseIf StrEq(tok, ACTION_IF) Then
                Me.Action = ExecActions.ACTION_IF

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 2 Then Throw New Exception("Condition expected. Line: " & Me.LineNumber.ToString)
                End If

            ElseIf StrEq(tok, ACTION_ELSE) Then
                Me.Action = ExecActions.ACTION_ELSE

                If Not Me.NotStrict Then
                    If Me.Tokens.Count > 1 Then Throw New Exception("Conditional expressions not valid here. Line: " & Me.LineNumber.ToString)
                End If

            ElseIf StrEq(tok, ACTION_ELSEIF) Then
                Me.Action = ExecActions.ACTION_ELSEIF

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 2 Then Throw New Exception("Condition expected. Line: " & Me.LineNumber.ToString)
                End If

            ElseIf StrEq(tok, ACTION_END) Then
                Me.Action = ExecActions.ACTION_END

                If Not Me.NotStrict Then
                    If Me.Tokens.Count > 1 Then
                        Throw New Exception("Syntax error. Line: " & Me.LineNumber.ToString)
                    End If
                End If

            ElseIf StrEq(tok, ACTION_SET) Then
                Me.Action = ExecActions.ACTION_SET

                'Check syntax
                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 4 Then Throw New Exception("Syntax error in assignment operation. Line: " & Me.LineNumber.ToString)
                    If Me.Tokens.IndexOf("=") <> 2 Then Throw New Exception("Syntax error in assignment operation. Line: " & Me.LineNumber.ToString)
                    Select Case Me.Tokens(1).Type
                        Case SyntaxToken.ElementTypes.Variable
                            If ContainsOperator_ExceptPeriod(Me.Tokens(1).Text) Then
                                Throw New Exception("'" & Me.Tokens(1).Text & "' is not a valid identifier name. Line: " & Me.LineNumber.ToString)
                            End If
                            'Check that the qualified tokens are valid
                            'ie no ..... successive dots
                            If Me.Tokens(1).QualifiedExpressionTokens.Count > 0 Then
                                For i As Integer = 0 To Me.Tokens(1).QualifiedExpressionTokens.Count - 1
                                    If Me.Tokens(1).QualifiedExpressionTokens(i).Text.Length = 0 Then
                                        Throw New Exception("Syntax error. Line: " & Me.LineNumber.ToString)
                                    End If
                                Next
                            End If
                        Case Else
                            Throw New Exception("'" & Me.Tokens(1).Text & "' is not a valid identifier name. Line: " & Me.LineNumber.ToString)
                    End Select
                End If

                'Determine subject
                If Me.Tokens.Count > 1 AndAlso Me.Tokens(1).QualifiedExpressionTokens.Count > 0 Then
                    'This shouldn't happen yet (syntax spec not doesn't support any scenarios like this).
                    'But an exception should occur on this attempt
                    If Me.Tokens(1).QualifiedExpressionTokens.Count > 2 Then
                        'We'll just throw an exception for now..
                        If Not Me.NotStrict Then
                            Dim sb As New System.Text.StringBuilder("'" & Me.Tokens(1).QualifiedExpressionTokens(2).Text)
                            sb.Append("' is not a member of '" & Me.Tokens(1).QualifiedExpressionTokens(0).Text)
                            sb.Append("." & Me.Tokens(1).QualifiedExpressionTokens(1).Text & "'. Line: " & Me.LineNumber.ToString)
                            Throw New Exception(sb.ToString)
                        End If
                    End If

                    Me.SetSubj = Me.Tokens(1).QualifiedExpressionTokens(0).Text
                    Me.SetSubjAttrib = Me.Tokens(1).QualifiedExpressionTokens(1).Text
                End If

            ElseIf StrEq(tok, ACTION_EXIT) Then
                Me.Action = ExecActions.ACTION_EXITWHEN

                If Not Me.NotStrict Then
                    If Me.Tokens.Count < 3 Then Throw New Exception("Syntax error. Line: " & Me.LineNumber.ToString)
                    If Not StrEq(Me.Tokens(1).Text, ACTION_WHEN) Then
                        Throw New Exception("Syntax error. Use '" & ACTION_EXIT & " " & ACTION_WHEN & _
                                            "' combination. Line: " & Me.LineNumber.ToString)
                    End If
                End If

            ElseIf StrEq(tok, SYS_RETURN) Then
                'Header only for templates
                If Me.AsMain Then Throw New Exception("Statement only valid in templates. Line: " & Me.LineNumber.ToString)

                Me.Action = ExecActions.SYS_RETURN

            ElseIf StrEq(tok, SYS_OUT) Then
                'Only for templates
                If Me.AsMain Then Throw New Exception("Statement only valid in templates. Line: " & Me.LineNumber.ToString)

                Me.Action = ExecActions.SYS_OUT

            ElseIf StrEq(tok, SYS_OUTLN) Then
                'Only for templates
                If Me.AsMain Then Throw New Exception("Statement only valid in templates. Line: " & Me.LineNumber.ToString)

                Me.Action = ExecActions.SYS_OUTLN

            ElseIf StrEq(tok, SYS_COUT) Then
                Me.Action = ExecActions.SYS_COUT

            ElseIf StrEq(tok, SYS_COUTLN) Then
                Me.Action = ExecActions.SYS_COUTLN

            ElseIf StrEq(tok, SYS_CLCON) Then
                Me.Action = ExecActions.SYS_CLCON

            ElseIf StrEq(tok, SYS_MAKEDIR) Then
                Me.Action = ExecActions.SYS_MAKEDIR

            ElseIf StrEq(tok, SYS_FILECOPY) Then
                Me.Action = ExecActions.SYS_FILECOPY

            ElseIf StrEq(tok, SYS_COMMAND) Then
                Me.Action = ExecActions.SYS_COMMAND

            ElseIf StrEq(tok, SYS_RUNSCRIPT) Then
                Me.Action = ExecActions.SYS_RUNSCRIPT

            Else
                Me.Action = ExecActions.IDENTIFIER

            End If
        End Sub

        Public Enum ExecActions
            PLAINTEXT = 0

            ACTION_ROOT = 1

            PREPROC_IGNORECASE = 2
            PREPROC_SAFEBEGIN = 3
            PREPROC_SAFEEND = 4

            COMMENT = 5

            HEADER_IS = 6

            ACTION_HEADER = 7
            ACTION_CALL = 8
            ACTION_FOR = 9
            ACTION_IF = 10
            ACTION_ELSE = 11
            ACTION_ELSEIF = 12
            ACTION_END = 13
            ACTION_SET = 14
            ACTION_EXITWHEN = 15

            SYS_RETURN = 16
            SYS_OUT = 17
            SYS_OUTLN = 18
            SYS_COUT = 19
            SYS_COUTLN = 20
            SYS_CLCON = 21
            SYS_MAKEDIR = 22
            SYS_FILECOPY = 23
            SYS_COMMAND = 24
            SYS_RUNSCRIPT = 25

            IDENTIFIER = 26
        End Enum

        Public Enum ExecForEntities
            NULL = 0
            OBJECT_TABLE = 1
            OBJECT_VIEW = 2
            OBJECT_COLUMN = 3
            OBJECT_PKCOLUMN = 4
            OBJECT_FKCOLUMN = 5
            OBJECT_IDCOLUMN = 6
            OBJECT_FILE = 7
            OBJECT_PROCEDURE = 8
            OBJECT_FUNCTION = 9
            OBJECT_PARAMETER = 10
            OBJECT_INPARAMETER = 11
            OBJECT_OUTPARAMETER = 12
            OBJECT_INOUTPARAMETER = 13
            OBJECT_RELATION = 14 'Boston specific. Not part of original Metadrone. Used for RDS.Relations as Objects.
            OBJECT_RELATIONS = 15 'Boston specific. Not part of original Metadrone. Used for RDS.Relations as Objects.
        End Enum

        Public Function GetCopy() As SyntaxNode
            Dim sn As New SyntaxNode()

            sn.Text = Me.Text
            sn.NotStrict = Me.NotStrict

            sn.TemplateName = Me.TemplateName
            sn.TemplateParameters = Me.TemplateParameters

            sn.Action = Me.Action
            sn.ForEntity = Me.ForEntity
            sn.SetSubj = Me.SetSubj
            sn.SetSubjAttrib = Me.SetSubjAttrib

            sn.LineNumber = Me.LineNumber

            For Each s In Me.Owner_TemplateParamNames
                Me.Owner_TemplateParamNames.Add(s)
            Next

            For Each tmpv In Me.Owner_TemplateParamValues
                Me.Owner_TemplateParamValues.Add(tmpv)
            Next

            sn.Tokens = Me.Tokens.GetCopy()

            For Each n In Me.Nodes
                sn.Nodes.Add(n.GetCopy())
            Next

            Me.IfBranch = Me.IfBranch.GetCopy()

            Return sn
        End Function

    End Class

End Namespace