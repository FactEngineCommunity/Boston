Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Exec_Call
        Private ExecNode As SyntaxNode
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private args As New List(Of Object)
        Private _templateCalled As String = ""

        Private WithEvents Exec As Boston.Parser.Syntax.Exec_Template = Nothing

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

        Public ReadOnly Property TemplateToCall() As String
            Get
                Return Me.ExecNode.Tokens(1).Text
            End Get
        End Property

        Public ReadOnly Property TemplateCalled() As String
            Get
                Return Me._templateCalled
            End Get
        End Property


        Public Sub EvalArgs()
            'When calling, we want to make some guess-work as to what is being passed regarding variables.
            'If a variable is passed on its own don't convert it to its primitive value
            'If a variable is used within an expression being passed in, automatically convert it so it can be used in the expression
            'eg:
            'call template(tblvar + "append text", colvar) //tblvar is converted to string value "my_table_nameappend text"
            'call template(tblvar, colvar) //tblvar is not converted

            'Split out between param seps
            '(arg...range,arg...range)
            Dim argIdxRanges As New List(Of List(Of Integer))
            Dim argIdxRange As New List(Of Integer)
            For i As Integer = 3 To Me.ExecNode.Tokens.Count - 2
                Select Case Me.ExecNode.Tokens(i).Type
                    Case SyntaxToken.ElementTypes.ParamSep
                        argIdxRanges.Add(argIdxRange)
                        argIdxRange = New List(Of Integer)
                    Case Else
                        argIdxRange.Add(i)
                End Select
            Next
            If argIdxRange.Count > 0 Then argIdxRanges.Add(argIdxRange)

            Dim argsToUse As List(Of Object)
            For Each ag In argIdxRanges
                If ag.Count > 1 Then
                    'Conversion required
                    argsToUse = EvalExpressionIntoParameters(Me.ExecNode.Tokens, ag(0), ag(ag.Count - 1), Me.TemplateContext, Me.ScopeDepth, True)
                Else
                    argsToUse = EvalExpressionIntoParameters(Me.ExecNode.Tokens, ag(0), ag(0), Me.TemplateContext, Me.ScopeDepth, False)
                End If
                For i As Integer = 0 To argsToUse.Count - 1
                    Me.args.Add(argsToUse(i))
                Next
            Next
        End Sub

        Public Sub Process()
            'Retrieve and setup template
            Dim n As Parser.Syntax.SyntaxNode = Nothing
            Try
                RaiseEvent Notify(".")
                n = Me.GetTemplate()

            Catch ex As FindTemplateException
                'Ignore the find error if previewing.
                If Not Me.PreviewMode Then Throw ex

            Catch ex As Exception
                Throw ex

            End Try

            'If preview mode, don't execute. It will be executed in the preview window.
            If Me.PreviewMode Then Exit Sub

            'Execute template
            Me.Exec = New Boston.Parser.Syntax.Exec_Template(n, Me.BasePath, Me.PreviewMode, Me.TemplateCalled, Me.ScopeDepth + 1)
            Me.Exec.Process()
			
	        'Pass on output
            For Each o As OutputItem In Me.Exec.OutputList
            	Me.OutputList.Add(o)
            Next

            'Clear variables inside this call
            PackageBuilder.Variables.Decommission(Me.TemplateCalled, Me.ScopeDepth + 1)
        End Sub

        Private Function GetTemplate() As Parser.Syntax.SyntaxNode
            'Get template
            Me._templateCalled = ""
            For Each TemplateNode In PackageBuilder.CompiledTemplates
                If StrEq(TemplateNode.TemplateName, Me.TemplateToCall) Then
                    'null single arg () is ok here
                    If TemplateNode.TemplateParameters.Count = 0 And Me.args.Count = 1 AndAlso Me.args(0) Is Nothing Then Return TemplateNode

                    'Check args match
                    If Me.args.Count > TemplateNode.TemplateParameters.Count Then
                        Throw New Exception("Too many arguments for template '" & Me.TemplateToCall & "'.")
                    ElseIf Me.args.Count < TemplateNode.TemplateParameters.Count Then
                        Throw New Exception("Not enough arguments for template '" & Me.TemplateToCall & "'.")
                    End If

                    'Assign values to template parameters
                    For i As Integer = 0 To Me.args.Count - 1
                        TemplateNode.TemplateParameters(i).Value = Me.args(i)

                        'And sub nodes
                        For Each node In TemplateNode.Nodes
                            If node.Owner_TemplateParamValues.Count > i Then
                                node.Owner_TemplateParamValues(i) = TemplateNode.TemplateParameters(i).Value
                            End If
                        Next
                    Next

                    Me._templateCalled = TemplateNode.TemplateName
                    Return TemplateNode
                End If
            Next

            Throw New FindTemplateException("Template '" & Me.TemplateToCall & "' not declared.")
        End Function

        Private Sub Exec_Notify(ByVal Message As String) Handles Exec.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub Exec_OutputWritten(ByVal Path As String) Handles Exec.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub Exec_ConsoleOut(ByVal Message As String) Handles Exec.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub Exec_ConsoleClear() Handles Exec.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace