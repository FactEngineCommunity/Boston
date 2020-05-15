Imports System.Reflection

Public Module publicModule

    'Client/Server using WCF
    Public prDuplexServiceClient As DuplexServiceClient.DuplexServiceClient

    Public Sub LoginUser(ByRef arUser As ClientServer.User)

        Try
            frmMain.ToolStripButtonProfile.Visible = True
            frmMain.ToolStripStatusLabelUsername.Text = "Username: " & arUser.Username

            arUser.IsLoggedIn = True

            Call tableClientServerUser.getAvailableFunctions(arUser)

            If arUser.IsSuperuser Then
                arUser.Function.AddUnique(pcenumFunction.FullPermission)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Module
