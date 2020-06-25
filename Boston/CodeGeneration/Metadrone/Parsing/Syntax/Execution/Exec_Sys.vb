Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr

Namespace Parser.Syntax

    Friend Class Exec_Sys
        Private ExecNode As SyntaxNode
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Public Event Notify(ByVal Message As String)

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal BasePath As String, ByVal PreviewMode As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.ExecNode = ExecNode
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
        End Sub

        Public Function [Return]() As String
            'Evaluate expression, return value
            Dim retVal As Object = Exec_Expr.EvalExpression(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)
            If retVal IsNot Nothing Then
                If TypeOf retVal Is Parser.Source.Source Then Throw New Exception("Source cannot be evaluated into a string.")
                Return retVal.ToString
            End If

            'Return nothing otherwise
            Return Nothing
        End Function

        Public Function Out() As String
            Try
                'Get parameters.
                Dim paramVals As List(Of Object) = _
                    EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

                'Check params ok
                If paramVals.Count = 0 Then Throw New Exception("Expecting argument text value.")
                If paramVals.Count > 1 Then Throw New Exception("Too many arguments.")

                'Return value
                If paramVals(0) Is Nothing Then Return Nothing
                Return paramVals(0).ToString

            Catch ex As Exception
                Throw New Exception("Invalid output value. " & ex.Message)
            End Try
        End Function

        Public Function OutLn() As String
            Return Me.Out() & System.Environment.NewLine
        End Function

        Public Function COut() As String
            Try
                'Get parameters.
                Dim paramVals As List(Of Object) = _
                    EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

                'Check params ok
                If paramVals.Count = 0 Then Throw New Exception("Expecting argument text value.")
                If paramVals.Count > 1 Then Throw New Exception("Too many arguments.")

                'Return value
                If paramVals(0) Is Nothing Then Return Nothing
                Return paramVals(0).ToString

            Catch ex As Exception
                Throw New Exception("Invalid value. " & ex.Message)
            End Try
        End Function

        Public Function COutLn() As String
            Return Me.Out() & System.Environment.NewLine
        End Function

        Public Sub MakeDir()
            'Don't process in preview mode
            If Me.PreviewMode Then Exit Sub

            'Get parameters.
            Dim paramVals As List(Of Object) = _
                EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

            'Check params ok
            If paramVals.Count = 0 Then Throw New Exception("Expecting argument path.")
            If paramVals.Count > 1 Then Throw New Exception("Too many arguments.")

            If paramVals(0) Is Nothing Then Throw New Exception("Directory path cannot be null.")

            'Combine paths if not rooted
            Dim strDir As String = paramVals(0).ToString
            If Not IO.Path.IsPathRooted(strDir) Then strDir = IO.Path.Combine(Me.BasePath, strDir)

            'Create directory
            Try
                IO.Directory.CreateDirectory(strDir)
            Catch ex As Exception
                Throw New Exception("Failed to create directory. " & ex.Message)
            End Try
        End Sub

        Public Sub FileCopy()
            'Don't process in preview mode
            If Me.PreviewMode Then Exit Sub

            'Get parameters.
            Dim paramVals As List(Of Object) = _
                EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

            'Check params ok
            If paramVals.Count = 0 Then Throw New Exception("Expecting argument source.")
            If paramVals.Count = 1 Then Throw New Exception("Expecting argument target.")
            If paramVals.Count > 2 Then Throw New Exception("Too many arguments.")

            'Combine paths if not rooted
            If Not IO.Path.IsPathRooted(paramVals(0).ToString) Then paramVals(0) = IO.Path.Combine(Me.BasePath, paramVals(0).ToString)
            If Not IO.Path.IsPathRooted(paramVals(1).ToString) Then paramVals(1) = IO.Path.Combine(Me.BasePath, paramVals(1).ToString)

            'Check file exists
            If Me.PreviewMode Then
                If Not IO.File.Exists(paramVals(0).ToString) Then Throw New Exception("The system cannot find the file specified.")
            End If

            'Copy file
            Try
                IO.File.Copy(paramVals(0).ToString, paramVals(1).ToString, True)
            Catch ex As Exception
                Throw New Exception("Failed to copy file. " & ex.Message)
            End Try
        End Sub

        Public Sub Command()
            'Don't process in preview mode
            If Me.PreviewMode Then Exit Sub

            'Get parameters.
            Dim paramVals As List(Of Object) = _
                EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

            'Check params ok
            If paramVals.Count = 0 Then Throw New Exception("Expecting argument filename.")
            If paramVals.Count = 1 Then Throw New Exception("Expecting argument arguments.")
            If paramVals.Count > 2 Then Throw New Exception("Too many arguments.")

            'Execute command
            Process.Start(paramVals(0).ToString, paramVals(1).ToString)
        End Sub

        Public Sub RunScript()
            'Don't process in preview mode
            If Me.PreviewMode Then Exit Sub

            'Get parameters.
            Dim paramVals As List(Of Object) = _
                EvalExpressionIntoParameters(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth, True)

            'Check params ok
            If paramVals.Count = 0 Then Throw New Exception("Expecting argument source.")
            If paramVals.Count = 1 Then Throw New Exception("Expecting argument filename.")
            If paramVals.Count > 2 Then Throw New Exception("Too many arguments.")

            If paramVals(0) Is Nothing Then Throw New Exception("Invalid argument for source")
            If Not TypeOf paramVals(0) Is Parser.Source.Source Then Throw New Exception("Invalid argument for source")

            If String.IsNullOrEmpty(paramVals(1).ToString) Then Throw New Exception("Invalid argument for filename")

            'Combine paths if not rooted
            Dim strPath As String = paramVals(1).ToString
            If Not IO.Path.IsPathRooted(strPath) Then strPath = IO.Path.Combine(Me.BasePath, strPath)

            'Run script file
            Call PackageBuilder.Connections.Item(CType(paramVals(0), Parser.Source.Source).Name).RunScriptFile(strPath)
        End Sub

    End Class

End Namespace