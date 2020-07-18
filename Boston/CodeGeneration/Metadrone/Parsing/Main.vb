Imports Boston.Parser.Output

Namespace Parser

    Public Class Main
        Private WithEvents sourceParser As SourceBuilder = Nothing
        Private WithEvents packageParser As Parser.PackageBuilder = Nothing
        
        Private mdProject As Persistence.MDProject = Nothing
        Private frmMain As UI.MainForm = Nothing
        Private StopBuildRequest As Boolean = False
        Private DontUpdatePackage As Boolean = True
        Private Output As Parser.Output.OutputCollection = Nothing

        Friend ProjectPath As String = Nothing
        Friend BuildError As Exception = Nothing        

        Public Event Notify(ByVal Message As String)
        Public Event OutputWritten(ByVal Path As String)
        Public Event ConsoleOut(ByVal Message As String)
        Public Event ConsoleClear()
        Public Event BuildCompleted()

        Public Sub New()

        End Sub

        Public Sub New(ByVal LoadProjectPath As String)
            Call Me.LoadProject(LoadProjectPath)
        End Sub

#Region "Usable friends: OutputSummary, BeginBuild"

        Friend Shared Function OutputSummary(ByVal StartTime As Date) As String
            Dim secs As Long = DateDiff(DateInterval.Second, StartTime, Now)
            Dim mins As Long = CLng((secs - (secs Mod 60)) / 60)
            secs = secs Mod 60

            If secs > 59 Then
                mins = secs Mod 60
                secs -= (mins * 60)
            End If

            Dim strSecs As String = ""
            If secs = 1 Then
                strSecs = "1 second"
            Else
                strSecs = secs.ToString & " seconds"
            End If

            If mins > 0 Then
                Dim strMins As String = ""
                If mins = 1 Then
                    strMins = "1 minute"
                Else
                    strMins = mins.ToString & " minutes"
                End If
                Return ("========== Finished in " & strMins & ", " & strSecs & ". ==========" & System.Environment.NewLine)
            Else
                Return ("========== Finished in " & strSecs & ". ==========" & System.Environment.NewLine)
            End If
        End Function

        Friend Shared Function BeginBuild(ByVal projectName As String, ByVal timeStarted As Date) As String
            Return ("========== Building '" & projectName & "' " & timeStarted.ToString() & " ==========" & System.Environment.NewLine)
        End Function

#End Region

        Friend Sub SetInteractive(ByVal frm As UI.MainForm)
            Me.frmMain = frm
        End Sub


        Public Sub LoadProject(ByVal LoadProjectPath As String)
            Me.ProjectPath = LoadProjectPath
            Me.mdProject = Persistence.ProjectManager.Load(LoadProjectPath)

            'Copy packages and sources in folders into project root
            For Each fld In Me.mdProject.Folders
                Call Me.AddProjectFolder("", fld)
            Next

            PackageBuilder.SuppressOutput = False
        End Sub

        Public Sub LoadProject(ByVal Project As Persistence.MDProject)
            Me.mdProject = Project

            'Copy packages and sources in folders into project root
            For Each fld In Me.mdProject.Folders
                Call Me.AddProjectFolder("", fld)
            Next

        	PackageBuilder.SuppressOutput = False
        End Sub

        'Copy packages and sources in folders into project root
        'Parser will just look at project root for sources and packages
        Private Sub AddProjectFolder(ByVal path As String, ByVal fld As Persistence.ProjectFolder)
            For Each pkg In fld.Packages
                pkg.Path = path & fld.Name & "/"
                Me.mdProject.Packages.Add(pkg)
            Next
            For Each src In fld.Sources
                Me.mdProject.Sources.Add(src)
            Next
            For Each f In fld.Folders
                Call Me.AddProjectFolder(path & fld.Name & "/", f)
            Next
        End Sub

        Public Sub BuildProject()
            Call Me.BuildProject(False)
        End Sub

		Public Function BuildProjectToOutput() As OutputCollection
			Call Me.BuildProject(True)
			Return Me.Output
		End Function
		
        Private Sub BuildProject(ByVal suppressOutput As Boolean)
        	PackageBuilder.SuppressOutput = suppressOutput

            'Ready output
            Me.Output = New Parser.Output.OutputCollection(Nothing)

            Try
                'Prepare
                Dim timeStarted As Date = Now
                RaiseEvent Notify(BeginBuild(Me.mdProject.Name, timeStarted))
                Me.BuildError = Nothing
                Me.StopBuildRequest = False

                'Eval sources
                Call Me.CompileSources()

                'Compile list of packages in project that need to be built
                RaiseEvent Notify("Parsing SUPER MAIN...")
                Dim sbld As New Parser.SuperBuilder(Me.mdProject)
                Try
                    sbld.Parse()
                    RaiseEvent Notify(".")
                Catch ex As Exception
                    sbld.PackagesToBuild.Clear()
                    RaiseEvent Notify(ex.Message)
                Finally
                    RaiseEvent Notify(System.Environment.NewLine)
                End Try

                'Build packages
                For Each pkg In sbld.PackagesToBuild
                    If Me.StopBuildRequest Then Exit For

                    'Cancelled (either during source build phase, or a package), don't continue with build
                    If Me.packageParser IsNot Nothing AndAlso Me.packageParser.Cancelled Then
                        RaiseEvent Notify(System.Environment.NewLine & "User aborted operation." & System.Environment.NewLine)
                        RaiseEvent Notify(System.Environment.NewLine)
                        Exit For
                    End If

                    'Build package
                    Me.BuildPackage(Me.mdProject.Properties, pkg, Me.mdProject.Bin.GetCompiledPackage(pkg.TagVal.GUID))

                    'Abort/error/output
                    Select Case Me.packageParser.Status.ParseState
                        Case PackageBuilder.ParserStatus.ProcessState.Cancelled
                            'Update compiled object data
                            If Not Me.DontUpdatePackage Then Me.mdProject.Bin.AddPackage(Me.packageParser.CompiledPackage)

                            'Notify and exit
                            RaiseEvent Notify(System.Environment.NewLine & "User aborted operation." & System.Environment.NewLine)
                            RaiseEvent Notify(System.Environment.NewLine)
                            Exit For

                        Case PackageBuilder.ParserStatus.ProcessState.ParseErrored
                            'Don't update compiled object data
                            Me.mdProject.Bin = Nothing

                            'Notify and continue to next
                            RaiseEvent Notify(System.Environment.NewLine)
                            If Me.packageParser.ParseException IsNot Nothing Then
                                RaiseEvent Notify(" Error: " & Me.packageParser.ParseException.Message & System.Environment.NewLine)
                            Else
                                RaiseEvent Notify(" Error encountered." & System.Environment.NewLine)
                            End If
                            RaiseEvent Notify(System.Environment.NewLine)
                            Continue For

                        Case PackageBuilder.ParserStatus.ProcessState.ExecErrored
                            'Update compiled object data
                            If Not Me.DontUpdatePackage Then Me.mdProject.Bin.AddPackage(Me.packageParser.CompiledPackage)

                            'Notify and continue to next
                            RaiseEvent Notify(System.Environment.NewLine)
                            If Me.packageParser.ParseException IsNot Nothing Then
                                RaiseEvent Notify(" Error: " & Me.packageParser.ParseException.Message & System.Environment.NewLine)
                            Else
                                RaiseEvent Notify(" Error encountered." & System.Environment.NewLine)
                            End If
                            RaiseEvent Notify(System.Environment.NewLine)
                            Continue For

                        Case PackageBuilder.ParserStatus.ProcessState.Finished
                            'Update compiled object data
                            If Not Me.DontUpdatePackage Then Me.mdProject.Bin.AddPackage(Me.packageParser.CompiledPackage)

                            'Return output
                            If PackageBuilder.SuppressOutput Then
                                For Each o As Parser.Output.OutputItem In Me.packageParser.Output
                                    If Me.packageParser.Cancelled Then
                                        RaiseEvent Notify("User aborted operation." & System.Environment.NewLine)
                                        Exit For
                                    End If
                                    Me.Output.AddKeepOriginalBasePath(o)
                                Next
                            End If

                    End Select

                    RaiseEvent Notify(System.Environment.NewLine)
                Next

                'Write compiled object data
                If My.Application.EXEC_SaveBin Then
                    Dim writeObj As Boolean = Not Me.DontUpdatePackage
                    If Me.sourceParser IsNot Nothing AndAlso Me.sourceParser.Cancelled Then writeObj = False
                    If Me.packageParser IsNot Nothing AndAlso Me.packageParser.Cancelled Then writeObj = False
                    If Me.packageParser IsNot Nothing AndAlso _
                       Me.packageParser.Status.ParseState = PackageBuilder.ParserStatus.ProcessState.Cancelled Then
                        writeObj = False
                    End If
                    If Me.packageParser IsNot Nothing AndAlso _
                       Me.packageParser.Status.ParseState = PackageBuilder.ParserStatus.ProcessState.ParseErrored Then
                        writeObj = False
                    End If
                    If writeObj Then
                        RaiseEvent Notify("Saving object data..." & System.Environment.NewLine & System.Environment.NewLine)
                        Dim prj As Persistence.MDProject = Persistence.ProjectManager.Load(Me.ProjectPath)
                        prj.Bin = Me.mdProject.Bin
                        Persistence.ProjectManager.Save(prj, Me.ProjectPath)
                    End If
                End If

                'Summary
                Call Me.ShowOutputSummary(timeStarted)

            Catch ex As Exception
                Me.BuildError = ex

            Finally
            	PackageBuilder.SuppressOutput = False

            End Try

            RaiseEvent BuildCompleted()
        End Sub

        Private Sub CompileSources()
            'Retrieve compiled sources
            RaiseEvent Notify("Evaluating sources.")
            Me.sourceParser = Nothing
            Me.sourceParser = New SourceBuilder(Me.mdProject.Bin.Bin.srces)
            If Me.sourceParser.CompiledSources.Count = 0 Then
                'If not pre-compiled get sources to compile
                For Each src In Me.mdProject.Sources

                    If Me.StopBuildRequest Then Exit For

                    If Me.frmMain IsNot Nothing Then
                        Dim idx As Integer = Me.frmMain.TagFoundInTabPages(src)
                        If idx > -1 Then
                            'If in editor, use values there
                            If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.ManageSource Then
                                Dim tag As Object = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.ManageSource).Tag
                                Dim s As Parser.Source.Source = CType(tag, Boston.Persistence.Source).ToParserSource
                                Me.sourceParser.AddSource(s.Name, s.Provider, New Syntax.SourceTransforms(s.Name, s.Transformations))
                            End If
                        Else
                            'Just from loaded
                            Me.sourceParser.AddSource(src.Name, src.Provider, New Syntax.SourceTransforms(src.Name, src.Transformations))
                        End If
                    Else
                        Me.sourceParser.AddSource(src.Name, src.Provider, New Syntax.SourceTransforms(src.Name, src.Transformations))
                    End If
                Next

                'Compile sources
                If My.Application.EXEC_ParseInThread Then
                    'Set status to running first before starting, because when during during the thread, it wasn't set yet,
                    'so it exited from the while loop prematurely
                    Me.sourceParser.Status.SetParseState(SourceBuilder.ParserStatus.ProcessState.Running)
                    Dim thrd As New Threading.Thread(AddressOf Me.sourceParser.Start)
                    thrd.Start()
                    While Me.sourceParser.Status.ParseState = SourceBuilder.ParserStatus.ProcessState.Running
                        If Me.StopBuildRequest Then Me.sourceParser.Stop()
                        Application.DoEvents() 'This somehow gives breathing room and improves speed in release mode. weird.
                    End While

                Else
                    Me.sourceParser.Start()

                End If

                'Set to persistence
                Me.mdProject.Bin.SetCompiledSources(Me.sourceParser.CompiledSources)
            End If

            Select Case Me.sourceParser.Status.ParseState
                Case SourceBuilder.ParserStatus.ProcessState.Cancelled
                    'Cancelled, notify (build won't continue in next for loop)
                    RaiseEvent Notify(System.Environment.NewLine & "User aborted operation." & System.Environment.NewLine)
                Case SourceBuilder.ParserStatus.ProcessState.ParseErrored
                    'Notify and ready to abort
                    RaiseEvent Notify(System.Environment.NewLine)
                    If Me.sourceParser.ParseException IsNot Nothing Then
                        RaiseEvent Notify(" Error: " & Me.sourceParser.ParseException.Message & System.Environment.NewLine)
                    Else
                        RaiseEvent Notify(" Error encountered." & System.Environment.NewLine)
                    End If
                    Me.StopBuildRequest = True
            End Select

            RaiseEvent Notify(System.Environment.NewLine)
            Application.DoEvents()
        End Sub


        Friend Sub BuildPackage(ByVal ProjProperties As Boston.Persistence.Properties,
                                ByVal Package As Boston.Persistence.MDPackage,
                                ByVal PreCompiledPackage As Bin.CompiledPackage)
            'Get base output path
            Dim BasePath As String = ""
            If Not String.IsNullOrEmpty(Me.ProjectPath) Then
                BasePath = System.IO.Path.GetDirectoryName(Me.ProjectPath)
            End If

            Me.DontUpdatePackage = PreCompiledPackage IsNot Nothing
            Me.packageParser = Nothing
            Me.packageParser = New Parser.PackageBuilder(Package.TagVal.GUID, BasePath, PreCompiledPackage)

            'Set up sources
            PackageBuilder.ClearSources()
            For Each src In Me.mdProject.Sources
                If Me.frmMain IsNot Nothing Then
                    Dim idx As Integer = Me.frmMain.TagFoundInTabPages(src)
                    If idx > -1 Then
                        'If in editor, use values there
                        If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.ManageSource Then
                            Dim tag As Object = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.ManageSource).Tag
                            Dim s As Parser.Source.Source = CType(tag, Boston.Persistence.Source).ToParserSource
                            s.Transforms = Me.sourceParser.GetCompiledSource(s.Name).Transforms 'Pass on compiled transforms
                            PackageBuilder.AddSource(s)
                        End If
                    Else
                        'Just add from loaded
                        Dim s As Parser.Source.Source = src.ToParserSource
                        s.Transforms = Me.sourceParser.GetCompiledSource(s.Name).Transforms 'Pass on compiled transforms
                        PackageBuilder.AddSource(s)
                    End If

                Else
                    Dim s As Parser.Source.Source = src.ToParserSource
                    s.Transforms = Me.sourceParser.GetCompiledSource(s.Name).Transforms 'Pass on compiled transforms
                    PackageBuilder.AddSource(s)

                End If
            Next

            'Set up package to parse

            'Main - Don't continue on error
            If Me.frmMain IsNot Nothing Then
                Dim idx As Integer = Me.frmMain.TagFoundInTabPages(Package.Main)
                If idx > -1 Then
                    'If in editor, use code there
                    If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                        Dim tag As Object = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.CodeEditor).Tag
                        Me.packageParser.SetMain(CType(tag, Boston.Persistence.Main).Text)
                    End If
                Else
                    'Just add from loaded
                    Me.packageParser.SetMain(Package.Main.Text)
                End If

            Else
                Me.packageParser.SetMain(Package.Main.Text)

            End If

            'Properties
            If Me.frmMain IsNot Nothing Then
                Dim idx As Integer = Me.frmMain.TagFoundInTabPages(ProjProperties)
                If idx > -1 Then
                    'If in editor, use properties there
                    If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.ManageProjectProperties Then
                        Dim tag As Object = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.ManageProjectProperties).Tag
                        Dim props As New Parser.PackageBuilder.Properties()
                        props.BeginTag = CType(tag, Boston.Persistence.Properties).BeginTag
                        props.EndTag = CType(tag, Boston.Persistence.Properties).EndTag
                        props.SuperMain = CType(tag, Boston.Persistence.Properties).SuperMain
                        Me.packageParser.SetProperties(props)
                    End If
                Else
                    'Just add from loaded
                    Dim props As New Parser.PackageBuilder.Properties()
                    props.BeginTag = ProjProperties.BeginTag
                    props.EndTag = ProjProperties.EndTag
                    props.SuperMain = ProjProperties.SuperMain
                    Me.packageParser.SetProperties(props)
                End If

            Else
                Dim props As New Parser.PackageBuilder.Properties()
                props.BeginTag = ProjProperties.BeginTag
                props.EndTag = ProjProperties.EndTag
                props.SuperMain = ProjProperties.SuperMain
                Me.packageParser.SetProperties(props)

            End If

            'Templates in folders
            For Each f As Boston.Persistence.Folder In Package.Folders
                Me.AddFolder(f)
            Next

            'Templates
            If Me.frmMain IsNot Nothing Then
                For Each t As Boston.Persistence.Template In Package.Templates
                    Dim idx As Integer = Me.frmMain.TagFoundInTabPages(t)
                    If idx > -1 Then
                        'If in editor, use code there
                        If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                            Dim editor As UI.CodeEditor = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.CodeEditor)
                            Dim tag As Boston.Persistence.Template = CType(editor.Tag, Boston.Persistence.Template)
                            Me.packageParser.AddTemplate(tag.Name, tag.Text)

                            'If user has modified we therefore need to re-compile, not updating that compilation in the saved object data
                            If tag.GetDirty Then
                                Me.packageParser.CompiledPackage = Nothing
                                Me.DontUpdatePackage = False
                            End If
                        End If
                    Else
                        'Just add from loaded
                        Me.packageParser.AddTemplate(t.Name, t.Text)
                    End If
                Next

            Else
                For Each t As Boston.Persistence.Template In Package.Templates
                    Me.packageParser.AddTemplate(t.Name, t.Text)
                Next

            End If

            '.net source
            Me.packageParser.VBSource = New System.Text.StringBuilder()
            Me.packageParser.CSSource = New System.Text.StringBuilder()
            If Me.frmMain IsNot Nothing Then
                For Each vb In Package.VBCode
                    Dim idx As Integer = Me.frmMain.TagFoundInTabPages(vb)
                    If idx > -1 Then
                        'If in editor, use code there
                        If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                            Dim editor As UI.CodeEditor = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.CodeEditor)
                            Dim tag As Boston.Persistence.CodeDOM_VB = CType(editor.Tag, Boston.Persistence.CodeDOM_VB)
                            Me.packageParser.VBSource.AppendLine(tag.Text)

                            'If user has modified we therefore need to re-compile, not updating that compilation in the saved object data
                            If tag.GetDirty Then
                                Me.packageParser.CompiledPackage = Nothing
                                Me.DontUpdatePackage = False
                            End If
                        End If
                    Else
                        'Just add from loaded
                        Me.packageParser.VBSource.AppendLine(vb.Text)
                    End If
                Next

                For Each cs In Package.CSCode
                    Dim idx As Integer = Me.frmMain.TagFoundInTabPages(cs)
                    If idx > -1 Then
                        'If in editor, use code there
                        If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                            Dim editor As UI.CodeEditor = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.CodeEditor)
                            Dim tag As Boston.Persistence.CodeDOM_CS = CType(editor.Tag, Boston.Persistence.CodeDOM_CS)
                            Me.packageParser.CSSource.AppendLine(tag.Text)

                            'If user has modified we therefore need to re-compile, not updating that compilation in the saved object data
                            If tag.GetDirty Then
                                Me.packageParser.CompiledPackage = Nothing
                                Me.DontUpdatePackage = False
                            End If
                        End If
                    Else
                        'Just add from loaded
                        Me.packageParser.CSSource.AppendLine(cs.Text)
                    End If
                Next

            Else
                For Each vb In Package.VBCode
                    Me.packageParser.VBSource.AppendLine(vb.Text)
                Next
                For Each cs In Package.CSCode
                    Me.packageParser.CSSource.AppendLine(cs.Text)
                Next

            End If

            'Starting text
            RaiseEvent Notify("'" & Package.Path & Package.Name & "'. " & Package.TemplateCount.ToString & " template(s)." & System.Environment.NewLine)

            'Run
            If My.Application.EXEC_ParseInThread Then
                'Set status to running first before starting, because when during during the thread, it wasn't set yet,
                'so it exited from the while loop prematurely
                Me.packageParser.Status.SetParseState(PackageBuilder.ParserStatus.ProcessState.Running)
                Dim thrd As New Threading.Thread(AddressOf Me.packageParser.Start)
                thrd.Start()
                While Me.packageParser.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
                    If Me.StopBuildRequest Then Me.packageParser.Stop()
                    Application.DoEvents() 'This somehow gives breathing room and improves speed in release mode. weird.
                End While

            Else
                Me.packageParser.Start()

            End If
        End Sub

        Friend Sub CancelBuild()
            Me.StopBuildRequest = True
        End Sub

        Private Sub ShowOutputSummary(ByVal StartTime As Date)
            RaiseEvent Notify(OutputSummary(StartTime))
        End Sub

        Private Sub AddFolder(ByVal f As Boston.Persistence.Folder)
            'Add any sub folders
            For Each folder As Boston.Persistence.Folder In f.Folders
                Me.AddFolder(folder)
            Next

            'Add templates in this folder
            For Each t As Boston.Persistence.Template In f.Templates
                If Me.frmMain IsNot Nothing Then
                    Dim idx As Integer = Me.frmMain.TagFoundInTabPages(t)
                    If idx > -1 Then
                        'If in editor, use code there
                        If TypeOf Me.frmMain.tcMain.TabPages(idx).Controls(0) Is UI.CodeEditor Then
                            Dim tag As Object = CType(Me.frmMain.tcMain.TabPages(idx).Controls(0), UI.CodeEditor).Tag
                            Me.packageParser.AddTemplate(CType(tag, Boston.Persistence.Template).Name, CType(tag, Boston.Persistence.Template).Text)
                        End If
                    Else
                        'Just add from loaded
                        Me.packageParser.AddTemplate(t.Name, t.Text)
                    End If

                Else
                    Me.packageParser.AddTemplate(t.Name, t.Text)

                End If
            Next
        End Sub

        Private Sub packageParser_Notify(ByVal Message As String) Handles packageParser.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub sourceParser_Notify(ByVal Message As String) Handles sourceParser.Notify
            RaiseEvent Notify(Message)
        End Sub

        Private Sub packageParser_OutputWritten(ByVal Path As String) Handles packageParser.OutputWritten
            RaiseEvent OutputWritten(Path)
        End Sub

        Private Sub packageParser_ConsoleOut(ByVal Message As String) Handles packageParser.ConsoleOut
            RaiseEvent ConsoleOut(Message)
        End Sub

        Private Sub packageParser_ConsoleClear() Handles packageParser.ConsoleClear
            RaiseEvent ConsoleClear()
        End Sub

    End Class

End Namespace