Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Meta
Imports Boston.Parser.Meta.Database
Imports Boston.Parser.Output
Imports Boston.Parser.Syntax.Exec_Expr

Namespace Parser.Syntax

    Friend Class Exec_BigFor
        Private ExecNode As SyntaxNode
        Private Header As Exec_Header
        Private BasePath As String = Nothing
        Private PreviewMode As Boolean = False
        Private TemplateContext As String = Nothing
        Private ScopeDepth As Integer = 0

        Private WithEvents proc As Exec_Base = Nothing

        Public OutputList As OutputCollection = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal ExecNode As SyntaxNode, ByVal Header As Exec_Header, ByVal BasePath As String, ByVal PreviewMode As Boolean, _
                       ByVal TemplateContext As String, ByVal ScopeDepth As Integer)
            Me.ExecNode = ExecNode
            Me.Header = Header
            Me.BasePath = BasePath
            Me.PreviewMode = PreviewMode
            Me.TemplateContext = TemplateContext
            Me.ScopeDepth = ScopeDepth
            Me.OutputList = New OutputCollection(BasePath)
        End Sub

        Public Sub Process()
            'Process file for
            If Me.Header.Method = Exec_Header.MethodType.ForFile Then
            	Me.ProcessFileFor()
            	Exit Sub
            End If

            'Determine which objects to fetch
            Dim ForEntity As SyntaxNode.ExecForEntities = SyntaxNode.ExecForEntities.NULL
            Select Case Me.Header.Method
                Case Exec_Header.MethodType.ForTable
                    ForEntity = SyntaxNode.ExecForEntities.OBJECT_TABLE
                Case Exec_Header.MethodType.ForView
                    ForEntity = SyntaxNode.ExecForEntities.OBJECT_VIEW
                Case Exec_Header.MethodType.ForProcedure
                    ForEntity = SyntaxNode.ExecForEntities.OBJECT_PROCEDURE
                Case Exec_Header.MethodType.ForFunction
                    ForEntity = SyntaxNode.ExecForEntities.OBJECT_FUNCTION
                Case Exec_Header.MethodType.ForRelation
                    ForEntity = SyntaxNode.ExecForEntities.OBJECT_RELATIONS
            End Select

            'Set up entities first
            Call PackageBuilder.Connections.Item(Me.Header.LoopSourceName).InitEntities()

            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0

            Do
                'Repeat if had to exit

                'Prevent infinite loop
                If CurrentLoopIdx >= PackageBuilder.Connections.Item(Me.Header.LoopSourceName).GetEntities(ForEntity).Count Then ExitIndex = -1

                'Perform loop on block
                For i As Integer = CurrentLoopIdx To PackageBuilder.Connections.Item(Me.Header.LoopSourceName).GetEntities(ForEntity).Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Exit Sub
                    
                    'Check that any filtering has not made this index go past the array range
                    If i > PackageBuilder.Connections.Item(Me.Header.LoopSourceName).GetEntities(ForEntity).Count - 1 Then Exit For

                    'Set variable's value
                    With PackageBuilder.Variables.Item(Me.TemplateContext, Me.Header.LoopVarName, Me.ScopeDepth)
                        .Value = PackageBuilder.Connections.Item(Me.Header.LoopSourceName).GetEntities(ForEntity).Item(i)
                    End With

                    'process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                    Dim sb As System.Text.StringBuilder = proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex)

                    'Clear variables inside this block
                    PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth + 1)

                    'If return encountered, finish processing
                    If Me.proc.UserReturnPathSet Then ExitIndex = -1

                    If ExitIndex = -1 Then
                        'Table processed

                        'Use path specified in header
                        Dim Path As String = Header.Path

                        'Use path specified in return statement
                        If Me.proc.UserReturnPathSet Then Path = Me.proc.UserReturnPath

                        'If path is set
                        If Not String.IsNullOrEmpty(Path) Then
                            'Add to outputs
                            Me.OutputList.Add(New OutputItem(sb.ToString, Path))
                            RaiseEvent Notify(".")
                        End If
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear variables inside this block
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth)
        End Sub

        Private Function ProcessFileFor() As List(Of OutputItem)
            Dim outputs As New List(Of OutputItem)

            Dim ExitIndex As Integer = -1
            Dim CurrentLoopIdx As Integer = 0
            Dim CurrentBlockIdx As Integer = 0

            'Loop through files in directory
            Do
                'Repeat if had to exit

                Dim files() As IO.FileInfo = Me.Header.LoopDirInfo.GetFiles()

                'Perform loop on block
                If files Is Nothing Then Throw New Exception("Failed to get files from directory.")
                For i As Integer = CurrentLoopIdx To files.Count - 1
                    'Abort on cancel
                    If PackageBuilder.StopRequest Then Return outputs

                    'Set variable's value
                    PackageBuilder.Variables.Item(Me.Header.LoopVarName, Me.TemplateContext, Me.ScopeDepth).Value = files(i).Name

                    'process block
                    Me.proc = New Exec_Base(Me.ExecNode, Me.BasePath, Me.PreviewMode, Me.TemplateContext, Me.ScopeDepth + 1)
                    Dim sb As System.Text.StringBuilder = Me.proc.ProcessBlock(CurrentBlockIdx, Me.ExecNode.Nodes.Count - 1, ExitIndex)

                    If Me.proc.UserReturnPathSet Then ExitIndex = -1

                    If ExitIndex = -1 Then
                        'Table processed

                        'Use path specified in header
                        Dim Path As String = Header.Path

                        'Use path specified in return statement
                        If proc.UserReturnPathSet Then Path = Me.proc.UserReturnPath

                        'If path is set
                        If Not String.IsNullOrEmpty(Path) Then
                            'Add to outputs
                            outputs.Add(New OutputItem(sb.ToString, Path))
                            RaiseEvent Notify(".")
                        End If
                    Else
                        'Redo because of exit
                        CurrentLoopIdx = i
                        CurrentBlockIdx = ExitIndex + 1
                        Exit For
                    End If
                Next
            Loop Until ExitIndex = -1

            'Clear variables inside this block
            PackageBuilder.Variables.Decommission(Me.TemplateContext, Me.ScopeDepth)

            Return outputs
        End Function

        Private Sub proc_Notify(ByVal Message As String) Handles proc.Notify
            'We don't want to show the ticks for any table loops within this big for.
            'RaiseEvent Notify(Message)
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