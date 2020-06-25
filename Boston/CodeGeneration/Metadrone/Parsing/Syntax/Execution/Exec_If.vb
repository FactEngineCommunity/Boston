Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Exec_Expr
Imports Boston.Parser.Output

Namespace Parser.Syntax

    Friend Class Exec_If
        Private ExecNode As SyntaxNode
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private WithEvents proc As Exec_Base = Nothing

        Friend UserReturnPath As String = Nothing
        Friend UserReturnPathSet As Boolean = False

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
        End Sub

        Public ReadOnly Property OutputList() As OutputCollection
            Get
                If Me.proc Is Nothing Then Return New OutputCollection(Me.BasePath)
                If Me.proc.OutputList Is Nothing Then Return New OutputCollection(Me.BasePath)
                Return Me.proc.OutputList
            End Get
        End Property

        Public Function Process() As System.Text.StringBuilder
            'Evaluate if condition
            Dim result As Object = EvalExpression(Me.ExecNode.Tokens, 1, Me.ExecNode.Tokens.Count - 1, Me.TemplateContext, Me.ScopeDepth)

            If CBool(result) Then
                'If condition satisfied, process child nodes up to else, elseif, or end

                'Determine which nodes to process
                Dim lower As Integer = 0
                Dim upper As Integer = Me.ExecNode.Nodes.Count - 1

                If Me.ExecNode.IfBranch.ElseIfNodeIndexes.Count > 0 Then
                    'Up to the first elseif
                    upper = Me.ExecNode.IfBranch.ElseIfNodeIndexes.Item(0)

                ElseIf Me.ExecNode.IfBranch.ElseNodeIndex > -1 Then
                    'Up to the else
                    upper = Me.ExecNode.IfBranch.ElseNodeIndex

                Else 'Up to the end
                    upper = Me.ExecNode.Nodes.Count - 1

                End If

                'process block
                Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                Dim sb As New System.Text.StringBuilder()
                sb.Append(Me.proc.ProcessBlock(lower, upper).ToString)

                'Clear variables inside this block
                PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

                'Return encounters
                Me.UserReturnPath = Me.proc.UserReturnPath
                Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                'Return text
                Return sb

            ElseIf Me.ExecNode.IfBranch.ElseIfNodeIndexes.Count > 0 Then
                'Asses elseif conditions
                For i As Integer = 0 To Me.ExecNode.IfBranch.ElseIfNodeIndexes.Count - 1
                    result = EvalExpression(Me.ExecNode.Nodes(Me.ExecNode.IfBranch.ElseIfNodeIndexes(i)).Tokens, _
                                            1, Me.ExecNode.Nodes(Me.ExecNode.IfBranch.ElseIfNodeIndexes(i)).Tokens.Count - 1, _
                                            Me.TemplateContext, Me.ScopeDepth)

                    If CBool(result) Then
                        'Determine which nodes to process
                        Dim lower As Integer = Me.ExecNode.IfBranch.ElseIfNodeIndexes(i)
                        Dim upper As Integer = Me.ExecNode.Nodes.Count - 1

                        If i < Me.ExecNode.IfBranch.ElseIfNodeIndexes.Count - 1 Then
                            'Up to the next elseif
                            upper = Me.ExecNode.IfBranch.ElseIfNodeIndexes.Item(i + 1)

                        ElseIf Me.ExecNode.IfBranch.ElseNodeIndex > -1 Then
                            'Up to the else
                            upper = Me.ExecNode.IfBranch.ElseNodeIndex

                        Else 'Up to the end
                            upper = Me.ExecNode.Nodes.Count - 1

                        End If

                        'process block
                        Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                        Dim sb As New System.Text.StringBuilder()
                        sb.Append(Me.proc.ProcessBlock(lower, upper).ToString)

                        'Clear variables inside this block
                        PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

                        Return sb
                    End If
                Next

                'None of the elseif conditions were met, go to else
                If Me.ExecNode.IfBranch.ElseNodeIndex > -1 Then
                    'Nodes to process are from else to end
                    Dim lower As Integer = Me.ExecNode.IfBranch.ElseNodeIndex
                    Dim upper As Integer = Me.ExecNode.Nodes.Count - 1

                    'process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                    Dim sb As New System.Text.StringBuilder()
                    sb.Append(Me.proc.ProcessBlock(lower, upper).ToString)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

                    'Return encounters
                    Me.UserReturnPath = Me.proc.UserReturnPath
                    Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                    'Return text
                    Return sb
                End If

            ElseIf Me.ExecNode.IfBranch.ElseNodeIndex > -1 Then
                'Do else

                'Nodes to process are from else to end
                Dim lower As Integer = Me.ExecNode.IfBranch.ElseNodeIndex
                Dim upper As Integer = Me.ExecNode.Nodes.Count - 1

                'process block
                Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                Dim sb As New System.Text.StringBuilder()
                sb.Append(Me.proc.ProcessBlock(lower, upper).ToString)

                'Clear variables inside this block
                PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

                'Return encounters
                Me.UserReturnPath = Me.proc.UserReturnPath
                Me.UserReturnPathSet = Me.proc.UserReturnPathSet

                'Return text
                Return sb

            End If

            'No processing to do
            Return New System.Text.StringBuilder()
        End Function

        Private Sub proc_Notify(ByVal Message As String) Handles proc.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub proc_OutputWritten(ByVal Path As String) Handles proc.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub proc_ConsoleOut(ByVal Message As String) Handles proc.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub proc_ConsoleClear() Handles proc.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace