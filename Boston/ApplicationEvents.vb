Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.    

    Partial Friend Class MyApplication

        Private Sub MyApplication_Shutdown(sender As Object, e As EventArgs) Handles Me.Shutdown

            '===========================================================================================================
            'Client/Server
            If My.Settings.UseClientServer And (prDuplexServiceClient IsNot Nothing) Then
                Try
                    If prDuplexServiceClient.State = ServiceModel.CommunicationState.Opened Then
                        prDuplexServiceClient.Disconnect()
                        prDuplexServiceClient.Abort()
                        '_proxy.Close() 'Close sometimes just hang there. Abort seems to work better/just as well.
                    End If
                Catch
                    prDuplexServiceClient.Abort()
                End Try
            End If

        End Sub

        'Dim liThinfinity As New Cybele.Thinfinity.VirtualUI

        <STAThread()> _
        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            If My.Settings.UseVirtualUI Then
                Call prThinfinity.Start(15)
                prThinfinity.AllowExecute("vbc.exe")
                'liThinfinity.DevMode = False

                If My.Settings.UseWindowsAuthenticationVirtualUI Then
                    Dim lrUser As New ClientServer.User
                    Dim lasUsername() As String = prThinfinity.BrowserInfo.Username.Split("\")
                    lrUser.Username = lasUsername(1)
                    prUser = lrUser
                End If

            End If

            If e.CommandLine.Count > 0 Then
                If e.CommandLine(0).EndsWith(".fbm") Then
                    psStartupFBMFile = e.CommandLine(0)
                End If
            End If

        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            '===========================================================================================================
            'Client/Server
            If My.Settings.UseClientServer And (prDuplexServiceClient IsNot Nothing) Then
                Try
                        prDuplexServiceClient.Disconnect()
                        prDuplexServiceClient.Abort()
                        '_proxy.Close() 'Close sometimes just hang there. Abort seems to work better/just as well.
                Catch
                    prDuplexServiceClient.Abort()
                End Try
            End If

            Dim lsMessage As String

            lsMessage = "An unhandled exception has occured in Boston"
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "If this problem reoccurs, please contact FactEngine support."
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= e.Exception.Message
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= e.Exception.InnerException.Message

            MsgBox(lsMessage)

            lsMessage &= e.ExitApplication

        End Sub

    End Class


End Namespace

