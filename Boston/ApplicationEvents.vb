Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            '20260811-VM-Not currently using, because double-clicking on a .fbm file reloads that file as a new model (regardless of whether it was loaded once before).
            'If e.CommandLine.Count > 0 Then

            '    Dim lsFile As String = e.CommandLine(0)

            '    If lsFile.EndsWith(".fbm", StringComparison.OrdinalIgnoreCase) Then

            '        psStartupFBMFile = lsFile

            '    End If

            'End If

        End Sub


    End Class
End Namespace
