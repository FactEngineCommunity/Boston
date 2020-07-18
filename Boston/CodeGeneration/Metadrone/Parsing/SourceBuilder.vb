Imports Boston.Parser.Bin
Imports Boston.Parser.Syntax

Namespace Parser

    Friend Class SourceBuilder

#Region "Parser Status"
        Public Class ParserStatus
            Public Enum ProcessState
                Stopped = 1
                Running = 2
                Cancelled = 3
                ParseErrored = 4
                Finished = 6
            End Enum

            Private mParseState As ProcessState = ProcessState.Stopped

            Public ReadOnly Property ParseState() As ProcessState
                Get
                    Return Me.mParseState
                End Get
            End Property

            Friend Sub SetParseState(ByVal ParseState As ProcessState)
                Me.mParseState = ParseState
            End Sub
        End Class
#End Region

        Private mParseException As Exception = Nothing
        Private UnCompiledSources As New List(Of CompiledSource)

        Private StopBuildRequest As Boolean = False
        Private ParseStatus As New ParserStatus()

        Friend CompiledSources As New List(Of CompiledSource)

        Public Event Notify(ByVal Message As String)

        Public Sub New()

        End Sub

        Public Sub New(ByVal CompiledSources As List(Of CompiledSource))
            Me.CompiledSources = CompiledSources
        End Sub

        Public Sub AddSource(ByVal SourceName As String, ByVal Provider As String, ByVal Transforms As SourceTransforms)
            Me.UnCompiledSources.Add(New CompiledSource(SourceName, Provider, Transforms))
        End Sub

        Public Function GetCompiledSource(ByVal SourceName As String) As CompiledSource
            For Each src In Me.CompiledSources
                If src.Name.Equals(SourceName) Then Return src
            Next
            Return Nothing
        End Function

        Public ReadOnly Property Status() As ParserStatus
            Get
                Return Me.ParseStatus
            End Get
        End Property

        Public ReadOnly Property ParseException() As Exception
            Get
                Return Me.mParseException
            End Get
        End Property

        Public Property Cancelled() As Boolean
            Get
                Return Me.StopBuildRequest
            End Get
            Set(ByVal value As Boolean)
                Me.StopBuildRequest = value
            End Set
        End Property

        Public Sub Start()
            Me.Status.SetParseState(ParserStatus.ProcessState.Running)
            Me.mParseException = Nothing
            Me.Cancelled = False

            'Compile sources
            Try
                If Me.CompiledSources.Count = 0 Then Call Me.Parse()

            Catch ex As Exception
                Me.mParseException = ex
                Me.Status.SetParseState(ParserStatus.ProcessState.ParseErrored)
                Exit Sub

            Finally
                'Remove tick delegates (for persistence)
                For Each src In Me.UnCompiledSources
                    RemoveHandler src.Transforms.Tick, AddressOf Me.Tick
                Next
            End Try

            'User aborted
            If Me.StopBuildRequest Then
                Me.CompiledSources = Nothing
                Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                Exit Sub
            End If

            'Finished
            Me.Status.SetParseState(ParserStatus.ProcessState.Finished)
        End Sub

        Private Sub Parse()
            'Set tick delegates
            Call Me.Tick()
            For Each src In Me.UnCompiledSources
                AddHandler src.Transforms.Tick, AddressOf Me.Tick
            Next

            'Check plugins work
            Call Me.Tick()
            For Each src In Me.UnCompiledSources
                src.CheckPluginAvailable()
            Next

            'Compile souces
            Call Me.Tick()
            If My.Application.EXEC_ParseInThread Then
                Dim threads As New List(Of System.Threading.Thread)
                For Each src In Me.UnCompiledSources
                    Dim t As New System.Threading.Thread(AddressOf src.Transforms.Build)
                    threads.Add(t)
                Next
                For Each t In threads
                    t.Start()
                Next

                'Wait for completion
                Dim compileCompleted As Boolean = False
                Dim stopRequest As Boolean = False
                While Not compileCompleted
                    'Application.DoEvents()  '20200718-VM-Commented this out.
                    'User aborted
                    If Not stopRequest And Me.StopBuildRequest Then
                        Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                        stopRequest = True
                        For Each src In Me.UnCompiledSources
                            src.Transforms.StopRequest = True
                        Next
                    End If

                    'Check if completed
                    compileCompleted = True
                    For Each src In Me.UnCompiledSources
                        If Not src.Transforms.Completed Then
                            compileCompleted = False
                            Exit For
                        End If
                    Next
                End While

            Else
                For Each src In Me.UnCompiledSources
                    'Don't continue if aborted
                    If Me.Status.ParseState = ParserStatus.ProcessState.Cancelled Then Exit Sub

                    Call src.Transforms.Build()
                Next

            End If

            'Report exception
            Dim sbCompileError As New System.Text.StringBuilder()
            For Each src In Me.UnCompiledSources
                If src.Transforms.CompileException IsNot Nothing Then
                    sbCompileError.AppendLine("  Compilation error in '" & src.Name & "'. " & src.Transforms.CompileException.Message)
                End If
            Next
            If sbCompileError.Length > 0 Then Throw New Exception(System.Environment.NewLine & sbCompileError.ToString)

            'Don't continue if aborted
            If Me.Status.ParseState = ParserStatus.ProcessState.Cancelled Then Exit Sub

            'Just set uncompiled to the compiled collection
            Me.CompiledSources = Me.UnCompiledSources
        End Sub

        Private Sub Tick()
            RaiseEvent Notify(".")
        End Sub

        Public Sub [Stop]()
            Me.Cancelled = True
        End Sub

    End Class

End Namespace