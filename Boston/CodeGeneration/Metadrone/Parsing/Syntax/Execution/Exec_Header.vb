Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class Exec_Header_NullParams_Exception
        Inherits Exception
        Public ParamNames As New List(Of String)
        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
    End Class

    Friend Class Exec_Header
        Private mMethod As MethodType = MethodType.Single
        Private PathExpressionTokens As New SyntaxTokenCollection()
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Public Enum MethodType
            [Single] = 0
            ForTable = 1
            ForView = 2
            ForProcedure = 3
            ForFunction = 4
            ForFile = 5
        End Enum

        Public TemplateName As String = ""
        Public LoopVarName As String = ""
        Public LoopSourceName As String = ""
        Public LoopDirInfo As IO.DirectoryInfo = Nothing

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal BasePath As String, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth

            For Each node In ExecNode.Nodes
                If node.Tokens.Count = 0 Then Continue For

                Select Case node.Action
                    Case SyntaxNode.ExecActions.PLAINTEXT, SyntaxNode.ExecActions.COMMENT, SyntaxNode.ExecActions.ACTION_END
                        'Ignore plaintext, comments and block enders

                    Case SyntaxNode.ExecActions.HEADER_IS
                        'Template name and parameters already parsed in package builder.
                        'Template parameters set when parsing main and calling this template.

                        'Check parameters have been assigned values from caller.
                        Dim nullParams As New List(Of String)
                        For i As Integer = 0 To ExecNode.Owner_TemplateParamNames.Count - 1
                            If ExecNode.Owner_TemplateParamValues(i) Is Nothing Then nullParams.Add(ExecNode.Owner_TemplateParamNames(i))
                        Next
                        'Report null parameters
                        If nullParams.Count > 0 Then
                            Dim sb As New System.Text.StringBuilder("The following parameters are null: ")
                            For i As Integer = 0 To nullParams.Count - 1
                                sb.Append(nullParams(i))
                                If i < nullParams.Count - 1 Then sb.Append(", ")
                            Next
                            Dim nullException As New Exec_Header_NullParams_Exception(sb.ToString)
                            For Each np In nullParams
                                nullException.ParamNames.Add(np)
                            Next
                            Throw nullException
                        End If

                        'Add arguments to variables
                        For i As Integer = 0 To ExecNode.Owner_TemplateParamNames.Count - 1
                            PackageBuilder.Variables.Add(Me.TemplateContext, _
                                                         ExecNode.Owner_TemplateParamNames(i), _
                                                         Me.ScopeDepth, _
                                                         New Variable(ExecNode.Owner_TemplateParamValues(i), Variable.Types.Variable))
                        Next

                    Case SyntaxNode.ExecActions.ACTION_FOR
                        'Set method
                        Select Case node.ForEntity
                            Case SyntaxNode.ExecForEntities.OBJECT_TABLE
                                Me.mMethod = MethodType.ForTable
                            Case SyntaxNode.ExecForEntities.OBJECT_VIEW
                                Me.mMethod = MethodType.ForView
                            Case SyntaxNode.ExecForEntities.OBJECT_PROCEDURE
                                Me.mMethod = MethodType.ForProcedure
                            Case SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                                Me.mMethod = MethodType.ForFunction
                            Case SyntaxNode.ExecForEntities.OBJECT_FILE
                                Me.mMethod = MethodType.ForFile
                            Case Else
                                Throw New Exception("Unsupported entity '" & node.Tokens(1).Text & "'. Line: " & node.LineNumber.ToString)
                        End Select

                        'Add variable
                        Me.LoopVarName = node.Tokens(2).Text
                        PackageBuilder.Variables.Add(Me.TemplateContext, Me.LoopVarName, Me.ScopeDepth, New Variable(Nothing, Variable.Types.Variable))

                        'Add connection
                        Me.LoopSourceName = ""
                        If Me.mMethod = MethodType.ForTable Or Me.mMethod = MethodType.ForView Or _
                           Me.mMethod = MethodType.ForProcedure Or Me.mMethod = MethodType.ForFunction Then
                            Try
                                'Get connection
                                Dim LoopConn As Object = EvalExpression(node.Tokens, 4, node.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
                                If LoopConn IsNot Nothing AndAlso TypeOf LoopConn Is Source.Source Then
                                    'Add to connections
                                    Me.LoopSourceName = CType(LoopConn, Source.Source).Name
                                    Select Case Me.mMethod
                                        Case MethodType.ForTable, MethodType.ForView
                                            PackageBuilder.Connections.Add(CType(LoopConn, Source.Source), Connections.LoadModes.Schema)

                                        Case MethodType.ForProcedure, MethodType.ForFunction
                                            PackageBuilder.Connections.Add(CType(LoopConn, Source.Source), Connections.LoadModes.Routines)

                                    End Select

                                Else
                                    Dim sb As New System.Text.StringBuilder()
                                    For i As Integer = 4 To node.Tokens.Count - 1
                                        sb.Append(node.Tokens(i).Text)
                                    Next
                                    Throw New Exception("Could not evaluate connection source: " & sb.ToString)

                                End If

                            Catch ex As Exception
                                Throw New Exception(ex.Message & " Line: " & node.LineNumber.ToString)
                            End Try

                        ElseIf Me.mMethod = MethodType.ForFile Then
                            Try
                                'Get directory source
                                Dim LoopConn As Object = EvalExpression(node.Tokens, 4, node.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
                                If LoopConn IsNot Nothing AndAlso TypeOf LoopConn Is String Then
                                    'Get directory
                                    Dim dirPath As String = IO.Path.Combine(BasePath, LoopConn.ToString)
                                    Me.LoopDirInfo = New IO.DirectoryInfo(dirPath)

                                Else
                                    Dim sb As New System.Text.StringBuilder()
                                    For i As Integer = 4 To node.Tokens.Count - 1
                                        sb.Append(node.Tokens(i).Text)
                                    Next
                                    Throw New Exception("Could not evaluate directory: " & sb.ToString)

                                End If

                            Catch ex As Exception
                                Throw New Exception(ex.Message & " Line: " & node.LineNumber.ToString)
                            End Try

                        End If

                    Case SyntaxNode.ExecActions.SYS_RETURN
                        'Get tokens for path expression
                        For i As Integer = 1 To node.Tokens.Count - 1
                            Me.PathExpressionTokens.Add(New SyntaxToken(node.Tokens(i).Text, node.LineNumber, SyntaxToken.ElementTypes.NotSet))
                        Next

                    Case SyntaxNode.ExecActions.ACTION_SET
                        'Set variable
                        Dim proc As New Exec_Set(node, Me.TemplateContext, Me.ScopeDepth)
                        Call proc.Process()

                    Case Else
                        Throw New Exception("Syntax error. Line: " & node.LineNumber.ToString)

                End Select
            Next

            'Confirm
            If PathExpressionTokens.Count = 0 Then Throw New Exception("Invalid or missing return expression.")
        End Sub

        Public ReadOnly Property Method() As MethodType
            Get
                Return Me.mMethod
            End Get
        End Property

        Public ReadOnly Property Path() As String
            Get
                Try
                    Return EvalExpression(Me.PathExpressionTokens, Me.TemplateContext, Me.ScopeDepth).ToString

                Catch ex As Exception
                    Throw New Exception("Failed to evaluate return expression: " & ex.Message)

                End Try
            End Get
        End Property
    End Class

End Namespace
