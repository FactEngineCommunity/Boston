Imports System.Runtime.InteropServices

Namespace UI

    Friend Class PreviewOutput
        Private WithEvents sourceParser As Parser.SourceBuilder
        Private WithEvents packageParser As Parser.PackageBuilder

        Private BasePath As String = ""
        Private Properties As New Persistence.Properties()
        Private Main As String
        Private Sources As New List(Of Persistence.Source)
        Private Template As New Persistence.Template()
        Private VBSource As New System.Text.StringBuilder()
        Private CSSource As New System.Text.StringBuilder()

        <DllImport("user32.dll")> _
        Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
        End Function


        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

        Public Sub New(ByVal BasePath As String, _
                       ByVal Properties As Persistence.Properties, ByVal MainSource As String, _
                       ByVal Sources As List(Of Persistence.Source), ByVal Template As Persistence.Template, _
                       ByVal VBSource As System.Text.StringBuilder, ByVal CSSource As System.Text.StringBuilder)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.BasePath = BasePath
            Me.Properties = Properties
            Me.Main = MainSource
            Me.Sources = Sources
            Me.Template.Text = Template.Text
            Me.Template.Name = Template.Name
            Me.VBSource = VBSource
            Me.CSSource = CSSource
        End Sub

        Private Sub PreviewOutput_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
            'Init
            Me.Refresh()
            Me.grdPaths.Rows.Clear()
            Me.txtOut.Text = ""
            Me.txtOutput.Text = "Generating preview..." & System.Environment.NewLine
            Me.txtOutput.Refresh()


            'Set up sources
            Me.WriteOut("Evaluating sources.")
            Me.sourceParser = Nothing
            Me.sourceParser = New Parser.SourceBuilder()
            'If not pre-compiled get sources to compile
            For Each src In Me.Sources
                Dim idx As Integer = MainForm.TagFoundInTabPages(src)
                If idx > -1 Then
                    'If in editor, use values there
                    If TypeOf MainForm.tcMain.TabPages(idx).Controls(0) Is UI.ManageSource Then
                        Dim tag As Object = CType(MainForm.tcMain.TabPages(idx).Controls(0), UI.ManageSource).Tag
                        Dim s As Parser.Source.Source = CType(tag, Boston.Persistence.Source).ToParserSource
                        Me.sourceParser.AddSource(s.Name, s.Provider, New Parser.Syntax.SourceTransforms(s.Name, s.Transformations))
                    End If
                Else
                    'Just from loaded
                    Me.sourceParser.AddSource(src.Name, src.Provider, New Parser.Syntax.SourceTransforms(src.Name, src.Transformations))
                End If
            Next

            'Compile sources
            If My.Application.EXEC_ParseInThread Then
                'Set status to running first before starting, because when during during the thread, it wasn't set yet,
                'so it exited from the while loop prematurely
                Me.sourceParser.Status.SetParseState(Parser.SourceBuilder.ParserStatus.ProcessState.Running)
                Dim thrd As New Threading.Thread(AddressOf Me.sourceParser.Start)
                thrd.Start()
                While Me.sourceParser.Status.ParseState = Parser.SourceBuilder.ParserStatus.ProcessState.Running
                    Application.DoEvents()
                End While

            Else
                Me.sourceParser.Start()

            End If
            Me.WriteOut(System.Environment.NewLine)

            Select Case Me.sourceParser.Status.ParseState
                Case Parser.SourceBuilder.ParserStatus.ProcessState.Cancelled
                    'Cancelled, notify (build won't continue in next for loop)
                    Me.WriteOut(System.Environment.NewLine & "User aborted operation." & System.Environment.NewLine)
                    Exit Sub
                Case Parser.SourceBuilder.ParserStatus.ProcessState.ParseErrored
                    'Notify and ready to abort
                    Me.WriteOut(System.Environment.NewLine)
                    If Me.sourceParser.ParseException IsNot Nothing Then
                        Me.WriteOut(" Error: " & Me.sourceParser.ParseException.Message & System.Environment.NewLine)
                    Else
                        Me.WriteOut(" Error encountered." & System.Environment.NewLine)
                    End If
                    Exit Sub
            End Select


            'Set up parser
            Try
                Me.packageParser = New Parser.PackageBuilder("", Me.BasePath, Nothing)
                Me.packageParser.PreviewOutputRef = Me

                'Set up properties
                Dim props As New Parser.PackageBuilder.Properties()
                props.BeginTag = Me.Properties.BeginTag
                props.EndTag = Me.Properties.EndTag
                props.SuperMain = Me.Properties.SuperMain
                Me.packageParser.SetProperties(props)
                Me.packageParser.SetMain(Me.Main)

                'Set up sources
                Parser.PackageBuilder.ClearSources()
                For Each src In Me.Sources
                    Dim idx As Integer = MainForm.TagFoundInTabPages(src)
                    If idx > -1 Then
                        'If in editor, use values there
                        If TypeOf MainForm.tcMain.TabPages(idx).Controls(0) Is UI.ManageSource Then
                            Dim tag As Object = CType(MainForm.tcMain.TabPages(idx).Controls(0), UI.ManageSource).Tag
                            Dim s As Parser.Source.Source = CType(tag, Boston.Persistence.Source).ToParserSource
                            s.Transforms = Me.sourceParser.GetCompiledSource(s.Name).Transforms 'Pass on compiled transforms
                            Parser.PackageBuilder.AddSource(s)
                        End If
                    Else
                        'Just add from loaded
                        Dim s As Parser.Source.Source = src.ToParserSource
                        s.Transforms = Me.sourceParser.GetCompiledSource(s.Name).Transforms 'Pass on compiled transforms
                        Parser.PackageBuilder.AddSource(s)
                    End If
                Next

                'Add the template
                Me.packageParser.AddTemplate(Me.Template.Name, Me.Template.Text)

                'Set .net source
                Me.packageParser.VBSource = Me.VBSource
                Me.packageParser.CSSource = Me.CSSource

            Catch ex As Exception
                Me.WriteOut("Error: " & ex.Message & System.Environment.NewLine)

                Exit Sub
            End Try

            'Parse
            Me.packageParser.PreviewMode = True

            If My.Application.EXEC_ParseInThread Then
                'Set status to running first before starting, because when during during the thread, it wasn't set yet,
                'so it exited from the while loop prematurely
                Me.packageParser.Status.SetParseState(Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running)

                Dim t As New Threading.Thread(AddressOf Me.packageParser.Start)
                t.Start()
                Application.DoEvents()

                While Me.packageParser.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
                    Application.DoEvents()
                End While
            Else
                Call Me.packageParser.Start()
            End If

            Select Case Me.packageParser.Status.ParseState
                Case Parser.PackageBuilder.ParserStatus.ProcessState.Cancelled
                    Me.WriteOut(System.Environment.NewLine & "User aborted operation." & System.Environment.NewLine)
                    Me.WriteOut(System.Environment.NewLine)

                Case Parser.PackageBuilder.ParserStatus.ProcessState.ParseErrored, Parser.PackageBuilder.ParserStatus.ProcessState.ExecErrored
                    Me.WriteOut(System.Environment.NewLine)
                    If Me.packageParser.ParseException IsNot Nothing Then
                        Me.WriteOut(" Error encountered: " & Me.packageParser.ParseException.Message & System.Environment.NewLine)
                    Else
                        Me.WriteOut(" Error encountered." & System.Environment.NewLine)
                    End If
                    Me.WriteOut(System.Environment.NewLine)

                Case Parser.PackageBuilder.ParserStatus.ProcessState.Finished
                    For Each o As Parser.Output.OutputItem In Me.packageParser.Output
                        Dim str(0) As String
                        str(0) = o.Path
                        Me.grdPaths.Rows.Add(str)
                    Next

                    Me.btnSave.Enabled = Me.grdPaths.SelectedRows.Count = 1
                    Me.btnSaveAll.Enabled = Me.grdPaths.Rows.Count > 0
                    Me.WriteOut(System.Environment.NewLine & "Completed.")

            End Select
        End Sub

        Private Sub PreviewOutput_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If Me.packageParser.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running Then
                If MessageBox.Show("Cancel preview parsing?", "Cancel",
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.OK Then
                    Me.packageParser.Stop()
                    Application.DoEvents()

                    While Me.packageParser.Status.ParseState = Boston.Parser.PackageBuilder.ParserStatus.ProcessState.Running
                        Application.DoEvents()
                    End While
                Else
                    e.Cancel = True
                End If
            End If
        End Sub

        Private Sub grdPaths_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdPaths.SelectionChanged
            Me.btnSave.Enabled = Me.grdPaths.SelectedRows.Count = 1
            Me.txtOut.Text = Me.packageParser.Output.Item(Me.grdPaths.SelectedRows(0).Index).Text()
        End Sub

        Private Sub sourceParser_Notify(ByVal Message As String) Handles sourceParser.Notify
            Me.WriteOut(Message)
        End Sub

        Private Sub packageParser_Notify(ByVal Message As String) Handles packageParser.Notify
            Me.WriteOut(Message)
        End Sub

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                Me.WriteOut(System.Environment.NewLine & " Writing output...")
                Me.packageParser.Output.Item(Me.grdPaths.SelectedRows(0).Index).Write()
                Me.WriteOut("finished. " & System.Environment.NewLine)
            Catch ex As Exception
                Me.WriteOut("failed. " & ex.Message & System.Environment.NewLine)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnSaveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAll.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                Me.WriteOut(System.Environment.NewLine & " Writing output...")
                For Each o As Parser.Output.OutputItem In Me.packageParser.Output
                    o.Write()
                Next
                Me.WriteOut("finished. " & System.Environment.NewLine)
            Catch ex As Exception
                Me.WriteOut("failed. " & ex.Message & System.Environment.NewLine)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub WriteOut(ByVal text As String)
            Try
                LockWindowUpdate(Me.txtOutput.Handle)
                Me.txtOutput.AppendText(text)
                Me.txtOutput.ScrollToCaret()
            Finally
                LockWindowUpdate(IntPtr.Zero)
            End Try
        End Sub

    End Class

End Namespace