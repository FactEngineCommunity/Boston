Namespace My

    ' The following events are available for MyApplication:
    '
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private LogPath As String = Nothing

        Friend EXEC_ParseInThread As Boolean = False
        'Break up parsing jobs into threads and execute them concurrently.
        'Set to false when debugging and to parse synchronously.

        Friend EXEC_ErrorSensitivity As ErrorSensitivity = ErrorSensitivity.Standard
        'Set to 'Noisy' when wanting to report certain errors for debugging.

        Friend EXEC_SaveBin As Boolean = False
        'Enable this the save pre-compilation data (should compilation ever become sufficiently slow).


        Friend Enum ErrorSensitivity
            Standard = 0
            Noisy = 1
        End Enum

        Friend Enum ExecModes
            Standard = 0
            Open = 1
            Build = 2
        End Enum

        Friend Mode As ExecModes = ExecModes.Standard
        Friend ProjectPath As String = Nothing


        'Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
        '    'No arguments
        '    If e.CommandLine.Count = 0 Then Exit Sub

        '    'Check for help
        '    'Metadrone.exe /?
        '    If e.CommandLine.Count = 1 Then
        '        If e.CommandLine(0).Trim.Equals("/?", StringComparison.OrdinalIgnoreCase) Then
        '            Call Me.InstructionsOut(Nothing)
        '            System.Environment.Exit(0)
        '            End
        '        End If
        '    End If

        '    Dim i As Integer = -1
        '    While i < e.CommandLine.Count - 1
        '        i += 1

        '        Try
        '            'Help only on its own
        '            If e.CommandLine(i).Trim.Equals("/?", StringComparison.OrdinalIgnoreCase) Then
        '                Throw New Exception("Help parameter should be on its own. Eg Metadrone.exe /?")
        '            End If

        '            'Parse synchronously (internal use)
        '            'Metadrone.exe /ParseNoThread
        '            If e.CommandLine(i).Trim.Equals("/PARSENOTHREAD", StringComparison.OrdinalIgnoreCase) Then
        '                Me.EXEC_ParseInThread = False
        '                Continue While
        '            End If

        '            'Noisy error sensitivity for debugging (internal use)
        '            'Metadrone.exe /NOISY
        '            If e.CommandLine(i).Trim.Equals("/NOISY", StringComparison.OrdinalIgnoreCase) Then
        '                Me.EXEC_ErrorSensitivity = ErrorSensitivity.Noisy
        '                Continue While
        '            End If

        '            'Get log path
        '            If e.CommandLine(i).Trim.Equals("/LOG", StringComparison.OrdinalIgnoreCase) Then
        '                If i = e.CommandLine.Count - 1 Then Throw New Exception("Log path expected.")
        '                i += 1
        '                Me.LogPath = e.CommandLine(i).Trim
        '                Continue While
        '            End If

        '            'Build
        '            If e.CommandLine(i).Trim.Equals("/BUILD", StringComparison.OrdinalIgnoreCase) Then
        '                Me.Mode = ExecModes.Build
        '                If i = e.CommandLine.Count - 1 Then Throw New Exception("Project path expected.")
        '                i += 1
        '                Me.ProjectPath = e.CommandLine(i).Trim
        '                Continue While
        '            End If

        '            'Assume open file
        '            If String.IsNullOrEmpty(Me.ProjectPath) Then
        '                Me.ProjectPath = e.CommandLine(i).Trim
        '                Me.Mode = ExecModes.Open
        '            Else
        '                Throw New Exception("Unknown parameter '" & e.CommandLine(i).Trim & "'.")
        '            End If

        '        Catch ex As Exception
        '            Call Me.InstructionsOut(ex)

        '            'Exit app
        '            System.Environment.Exit(0)
        '            End

        '        End Try

        '    End While


        '    'Implement mode
        '    Select Case Me.Mode
        '        Case ExecModes.Standard, ExecModes.Open
        '            'Form shown event will check this mode and open
        '            Exit Sub

        '        Case ExecModes.Build
        '            Dim exec As New ConsoleExec(Me.LogPath)
        '            Try
        '                ConsoleExec.AttachConsole()
        '                exec.WriteLine(Nothing)
        '                exec.WriteLine(Globals.ASSEMBLY_NAME_METADRONE & " " & "version " & Globals.ASSEMBLY_VERSION_METADRONE)
        '                exec.WriteLine("www.metadrone.com")
        '                exec.WriteLine(UI.About.GetCopyright())
        '                exec.BuildProject(Me.ProjectPath)
        '            Catch ex As Exception
        '                exec.WriteLine("Error encountered: " & ex.Message)
        '            Finally
        '                ConsoleExec.FreeConsole()
        '            End Try
        '    End Select

        '    'Finished building
        '    System.Environment.Exit(0)
        '    End
        'End Sub

        Private Sub InstructionsOut(ByVal ex As Exception)
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine()

            If ex IsNot Nothing Then
                sb.AppendLine("Invalid command line argument(s).")
                sb.AppendLine(ex.Message)
                sb.AppendLine()
                sb.AppendLine()
            End If

            sb.AppendLine("Usage:")
            sb.AppendLine()
            sb.AppendLine("Open a project")
            sb.AppendLine("Metadrone.exe ""Project\Path.ext""")
            sb.AppendLine()
            sb.AppendLine("Build a project")
            sb.AppendLine("Metadrone.exe /build ""Project\Path.ext""")
            sb.AppendLine()
            sb.AppendLine("Build a project and log to a text file (new or append to existing)")
            sb.AppendLine("Metadrone.exe /build ""Project\Path.ext"" /log ""logfile.txt""")

            Dim exec As New ConsoleExec(Nothing)
            ConsoleExec.AttachConsole()
            exec.WriteLine(Nothing)
            exec.WriteLine(Globals.ASSEMBLY_NAME_METADRONE & " " & "version " & Globals.ASSEMBLY_VERSION_METADRONE)
            exec.WriteLine("www.metadrone.com")
            exec.WriteLine(UI.About.GetCopyright())
            exec.WriteLine(sb.ToString)
            ConsoleExec.FreeConsole()
        End Sub

    End Class

End Namespace

