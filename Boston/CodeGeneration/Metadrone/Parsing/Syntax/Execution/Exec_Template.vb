Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Exec_Template
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private SyntaxTree As SyntaxNode

        Private WithEvents BaseProc As Exec_Base = Nothing
        Private WithEvents BigForProc As Exec_BigFor = Nothing

        Friend PreviewOutputRef As UI.PreviewOutput = Nothing

        Public OutputList As OutputCollection = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal CompiledTemplate As SyntaxNode, ByVal BasePath As String, ByVal PreviewMode As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
            Me.SyntaxTree = CompiledTemplate
            Me.OutputList = New OutputCollection(BasePath)
        End Sub

        Public Sub Process()
            'Get header
            Dim Header As Exec_Header = Nothing
            For Each node In Me.SyntaxTree.Nodes
                'Process header
                If node.Action <> SyntaxNode.ExecActions.ACTION_HEADER Then Continue For

                'Exception if declared more than once
                If Header IsNot Nothing Then Throw New Exception("Only one template header allowed. Line: " & node.LineNumber.ToString)

                Try
                    Header = New Exec_Header(node, Me.BasePath, Me.TemplateContext, Me.ScopeDepth)

                Catch ex As Exec_Header_NullParams_Exception
                    'Don't handle when not previewing
                    If Not Me.PreviewMode Then Throw

                    'Just throw if can't reconcile parameters
                    If Not ex.ParamNames.Count.Equals(node.Owner_TemplateParamNames.Count) Then
                        Throw New Exception("System error: Failure to reconcile parameter count.")
                    End If

                    'Prompt to set parameters
                    Dim f As New UI.PreviewOutputInit()
                    For Each p In ex.ParamNames
                        f.ParamNames.Add(p)
                    Next
                    If f.ShowDialog(Me.PreviewOutputRef) = DialogResult.Cancel Then Throw New Exception("User aborted operation.")

                    'Assign values to template parameters
                    For i As Integer = 0 To node.Owner_TemplateParamValues.Count - 1
                        node.Owner_TemplateParamValues(i) = f.ParamVals(i)
                    Next

                    'And the owner's
                    For i As Integer = 0 To Me.SyntaxTree.TemplateParameters.Count - 1
                        Me.SyntaxTree.TemplateParameters(i).Value = node.Owner_TemplateParamValues(i)
                    Next

                    'Try again
                    Header = New Exec_Header(node, Me.BasePath, Me.TemplateContext, Me.ScopeDepth)

                Catch ex As Exception
                    Throw New Exception("Header construct invalid: " & ex.Message)

                End Try
            Next

            'Don't continue without header
            If Header Is Nothing Then Throw New Exception("No template header defined.")

            'Process template
            Select Case Header.Method
                Case Exec_Header.MethodType.Single
                    'Use path specified in header
                    Dim Path As String = Header.Path

                    'Process template
                    Me.BaseProc = New Exec_Base(Me.SyntaxTree, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)
                    Dim sb As System.Text.StringBuilder = Me.BaseProc.ProcessBlock()

                    'Pass on output
                    For Each o As OutputItem In Me.BaseProc.OutputList
                    	Me.OutputList.Add(o)
                    Next

                    'Use path specified in return statement
                    If Me.BaseProc.UserReturnPathSet Then Path = Me.BaseProc.UserReturnPath

                    'If path is set, add to outputs
                    If Not String.IsNullOrEmpty(Path) Then Me.OutputList.Add(New OutputItem(sb.ToString, Path))
                    
                Case Else
                    Me.BigForProc = New Exec_BigFor(Me.SyntaxTree, Header, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth)
                    Me.BigForProc.Process()

                    'Pass on output
                    For Each o As OutputItem In Me.BigForProc.OutputList
                        Me.OutputList.Add(o)
                    Next
            End Select
        End Sub

        Private Sub Proc_Notify(ByVal Message As String) Handles BaseProc.Notify, BigForProc.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub Proc_OutputWritten(ByVal Path As String) Handles BaseProc.OutputWritten, BigForProc.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub Proc_ConsoleOut(ByVal Message As String) Handles BaseProc.ConsoleOut, BigForProc.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub Proc_ConsoleClear() Handles BaseProc.ConsoleClear, BigForProc.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace
