Imports Boston.Parser.Output
Imports Boston.Parser.Syntax
Imports Boston.Parser.Syntax.Strings
Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Bin

Namespace Parser

    Friend Class PackageBuilder

#Region "Parser Status"
        Public Class ParserStatus
            Public Enum ProcessState
                Stopped = 1
                Running = 2
                Cancelled = 3
                ParseErrored = 4
                ExecErrored = 5
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

#Region "Parser Properties"

        Public Class Properties
            Public BeginTag As String = Syntax.Constants.TAG_BEGIN_DEFAULT
            Public EndTag As String = Syntax.Constants.TAG_END_DEFAULT
            Public SuperMain As String = ""
        End Class

#End Region

        Private PackageGUID As String
        Private MainSource As String
        Private Templates As New Parser.Templates()
        Private mParseException As Exception = Nothing
        Private Compilers As New List(Of Parser.Syntax.Compilation)
        Private ParseStatus As New ParserStatus()

        Private WithEvents Main As Parser.Syntax.Main
        Private WithEvents Exec As Parser.Syntax.Exec_Template = Nothing

        Friend BaseOutputPath As String

        Friend PreviewMode As Boolean = False
        Friend PreviewOutputRef As UI.PreviewOutput = Nothing

        Friend VBSource As System.Text.StringBuilder = Nothing
        Friend CSSource As System.Text.StringBuilder = Nothing
        Friend CompiledPackage As CompiledPackage = Nothing

        Friend Shared SuppressOutput As Boolean = False
        Friend Shared StopRequest As Boolean = False
        Friend Shared Sources As Source.Sources = Nothing
        Friend Shared CompiledTemplates As List(Of Parser.Syntax.SyntaxNode) = Nothing
        Friend Shared Variables As Parser.Syntax.Variables = Nothing
        Friend Shared Connections As Parser.Syntax.Connections = Nothing
        Friend Shared PreProc As PreProcessor = Nothing
        Friend Shared CodeDOM As Syntax.CodeDom = Nothing

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()

        Public Sub New(ByVal PackageGUID As String, ByVal BaseOutputPath As String)
            Me.PackageGUID = PackageGUID
            Me.BaseOutputPath = BaseOutputPath
        End Sub

        Friend Sub New(ByVal PackageGUID As String, ByVal BaseOutputPath As String, ByVal PreCompiledPackage As CompiledPackage)
            Me.PackageGUID = PackageGUID
            Me.BaseOutputPath = BaseOutputPath
            Me.CompiledPackage = PreCompiledPackage
        End Sub

        Public Sub SetProperties(ByVal Properties As Properties)
            With Properties
                Syntax.Constants.TAG_BEGIN = .BeginTag
                Syntax.Constants.TAG_END = .EndTag
            End With
        End Sub

        Public Shared Sub ClearSources()
            PackageBuilder.Sources = New Source.Sources()
        End Sub

        Public Shared Sub AddSource(ByVal Source As Source.Source)
            If PackageBuilder.Sources Is Nothing Then PackageBuilder.Sources = New Source.Sources()
            PackageBuilder.Sources.Add(Source)
        End Sub

        Public Sub SetMain(ByVal Source As String)
            Me.MainSource = Source
        End Sub

        Public Sub AddTemplate(ByVal TemplateName As String, ByVal Source As String)
            Me.Templates.Add(TemplateName, Source)
        End Sub

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

        Public ReadOnly Property Output() As OutputCollection
            Get
                If Me.Main Is Nothing Then Return New OutputCollection(Me.BaseOutputPath)

                Return Me.Main.OutputList
            End Get
        End Property

        Public Property Cancelled() As Boolean
            Get
                Return PackageBuilder.StopRequest
            End Get
            Set(ByVal value As Boolean)
                PackageBuilder.StopRequest = value
            End Set
        End Property

        Public Sub Start()
            Me.Status.SetParseState(ParserStatus.ProcessState.Running)
            Me.mParseException = Nothing
            Me.Cancelled = False

            'Compile templates in package
            RaiseEvent Notify(" Parsing.")
            Try
                If Me.CompiledPackage Is Nothing Then Call Me.Parse()

            Catch ex As Exception
                Me.mParseException = ex
                Me.Status.SetParseState(ParserStatus.ProcessState.ParseErrored)
                Exit Sub

            End Try

            'Compile .net code
            Try
                PackageBuilder.CodeDOM = New Syntax.CodeDom(Me.VBSource.ToString, Me.CSSource.ToString)

            Catch ex As Exception
                Me.mParseException = ex
                Me.Status.SetParseState(ParserStatus.ProcessState.ParseErrored)
                Exit Sub

            End Try

            'User aborted
            If PackageBuilder.StopRequest Then
                Me.CompiledPackage = Nothing
                Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                Exit Sub
            End If

            'Execute: parse main and execute templates called within there
            Try
                RaiseEvent Notify(System.Environment.NewLine)
                RaiseEvent Notify(" Executing.")

                'Initialise package level objects
                PackageBuilder.Variables = New Parser.Syntax.Variables()
                PackageBuilder.Connections = New Parser.Syntax.Connections()
                PackageBuilder.CompiledTemplates = New List(Of Syntax.SyntaxNode)
                PackageBuilder.PreProc = New PreProcessor()

                For Each comp In Me.CompiledPackage.CompiledTemplates
                    PackageBuilder.CompiledTemplates.Add(comp.BaseNode)
                Next
                Me.Main = New Parser.Syntax.Main(Me.MainSource, Me.BaseOutputPath, Me.PreviewMode)
                Call Me.Main.Run()

                'Execute in preview mode (main will ignore calls when in preview mode)
                If Me.PreviewMode Then
                    For Each ct In Me.CompiledPackage.CompiledTemplates
                        Me.Exec = New Parser.Syntax.Exec_Template(ct.BaseNode, Me.BaseOutputPath, Me.PreviewMode, "TEMPLATE", 1)
                        Me.Exec.PreviewOutputRef = Me.PreviewOutputRef
                        Me.Exec.Process()
                        
                        'Abort processing
                        If PackageBuilder.StopRequest Then
                            Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                            Exit Sub
                        End If

				        'Pass on output
			            For Each o As OutputItem In Me.Exec.OutputList
                            Me.Main.OutputList.Add(o)
			            Next
                    Next
                End If

                'Finished
                Me.Status.SetParseState(ParserStatus.ProcessState.Finished)

                'User aborted
                If PackageBuilder.StopRequest Then
                    Me.CompiledPackage = Nothing
                    Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                End If

            Catch ex As Exception
                Me.mParseException = ex
                Me.Status.SetParseState(ParserStatus.ProcessState.ExecErrored)

            End Try
        End Sub

        Private Sub Parse()
            'Gather template compilers
            Call Me.Tick()
            For i As Integer = 0 To Me.Templates.Count - 1
                Dim comp As New Parser.Syntax.Compilation(Me.Templates.Item(i).Name, Me.Templates.Item(i).Source, False, False)
                Me.Compilers.Add(comp)
            Next

            'Set tick delegates
            Call Me.Tick()
            For Each comp In Me.Compilers
                AddHandler comp.Tick, AddressOf Me.Tick
            Next

            'Compile templates
            Call Me.Tick()
            If My.Application.EXEC_ParseInThread Then
                Dim threads As New List(Of System.Threading.Thread)
                For Each comp In Me.Compilers
                    Dim t As New System.Threading.Thread(AddressOf comp.BuildForThreadedCompilation)
                    threads.Add(t)
                Next
                For Each t In threads
                    t.Start()
                Next

                'Wait for completion
                Dim compileCompleted As Boolean = False
                Dim stopRequest As Boolean = False
                While Not compileCompleted
                    Application.DoEvents()
                    'User aborted
                    If Not stopRequest And PackageBuilder.StopRequest Then
                        Me.Status.SetParseState(ParserStatus.ProcessState.Cancelled)
                        stopRequest = True
                        For Each comp In Me.Compilers
                            comp.StopRequest = True
                        Next
                    End If

                    'Check if completed
                    compileCompleted = True
                    For Each comp In Me.Compilers
                        If Not comp.Completed Then
                            compileCompleted = False
                            Exit For
                        End If
                    Next
                End While

            Else
                For Each comp In Me.Compilers
                    'Don't continue if aborted
                    If Me.Status.ParseState = ParserStatus.ProcessState.Cancelled Then Exit Sub

                    Call comp.BuildForThreadedCompilation()
                Next

            End If

            'Report exception
            Dim sbCompileError As New System.Text.StringBuilder()
            For Each comp In Me.Compilers
                If comp.CompileException IsNot Nothing Then
                    sbCompileError.AppendLine("  Compilation error in '" & comp.SourceName & "'. " & comp.CompileException.Message)
                End If
            Next
            If sbCompileError.Length > 0 Then Throw New Exception(System.Environment.NewLine & sbCompileError.ToString)

            'Don't continue if aborted
            If Me.Status.ParseState = ParserStatus.ProcessState.Cancelled Then Exit Sub

            'Gather compiled template collection
            Call Me.Tick()
            Me.CompiledPackage = New CompiledPackage
            Me.CompiledPackage.GUID = Me.PackageGUID
            For Each comp In Me.Compilers
                Me.CompiledPackage.CompiledTemplates.Add(New CompiledTemplate(comp.SourceName, comp.BaseSyntaxNode))
            Next

            'Evaluate templates
            Call Me.Tick()
            For i As Integer = 0 To Me.CompiledPackage.CompiledTemplates.Count - 1
                Try
                    'Check if template has not already been defined
                    For j As Integer = 0 To Me.CompiledPackage.CompiledTemplates.Count - 1
                        If j = i Then Continue For 'Don't compare the same
                        If Me.CompiledPackage.CompiledTemplates(j).BaseNode.TemplateName Is Nothing Then Continue For 'Header not evaluated yet

                        If StrEq(Me.CompiledPackage.CompiledTemplates(j).BaseNode.TemplateName, Me.CompiledPackage.CompiledTemplates(i).BaseNode.TemplateName) Then
                            Throw New Exception("Template '" & Me.CompiledPackage.CompiledTemplates(j).BaseNode.TemplateName & "' already defined.")
                        End If
                    Next
                Catch ex As Exception
                    Throw New Exception("Compilation error in '" & Me.CompiledPackage.CompiledTemplates(i).Name & "'. " & ex.Message)

                End Try
            Next
        End Sub

        Private Sub Tick()
            RaiseEvent Notify(".")
        End Sub

        Public Sub [Stop]()
            Me.Cancelled = True
        End Sub

        Private Sub Exec_Notify(ByVal Message As String) Handles Exec.Notify, Main.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub Exec_OutputWritten(ByVal Path As String) Handles Exec.OutputWritten, Main.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub Exec_ConsoleOut(ByVal Message As String) Handles Exec.ConsoleOut, Main.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub Exec_ConsoleClear() Handles Exec.ConsoleClear, Main.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace