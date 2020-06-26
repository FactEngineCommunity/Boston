Namespace UI

    Partial Friend Class MainForm

        Private Sub Initialise()
            'Load plugins
            Call Me.LoadPlugins()


            Me.lblStatus.Text = "Initialising..."
            Me.Refresh()

            'Load settings
            Call Me.settings.Load()
            Call CType(Me.tcMain.TabPages(0).Controls(0), StartPage).LoadRecents(Me.settings)

            'Clean up update folders
            For Each s In System.IO.Directory.GetDirectories(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location), _
                                                             "update-????-??-??")
                System.IO.Directory.Delete(s, True)
            Next s
        End Sub


        Private Sub LoadPlugins()
            If Globals.SourcePlugins.Plugins.Count > 0 Then Exit Sub

            Dim di As New IO.DirectoryInfo(My.Application.Info.DirectoryPath & "\codegenerationplugins")
            Dim aryFi As IO.FileInfo() = di.GetFiles("*plugins*.dll")
            For Each fi As IO.FileInfo In aryFi
                Me.lblStatus.Text = "Scanning for plugins: '" & fi.FullName & "'. "
                Me.Refresh()
                Try
                    If Globals.SourcePlugins.LoadAssembly(fi.FullName) Then
                        Me.lblStatus.Text &= "Loaded."
                    End If

                Catch ex As Exception
                    'Ignore exception
                    MsgBox(ex.InnerException.Message)
                End Try
            Next

            Call Globals.SourcePlugins.loadBostonCodeGenerationPlugin() 'DummyPlugin that references a Boston Model in memory rather than a database

            Me.lblStatus.Text = ""
            Me.Refresh()
        End Sub

    End Class

End Namespace
