Imports JupyterNetClient
Imports System.Windows.Forms.Integration
Imports System.Reflection
Imports CefSharp.WinForms
Imports CefSharp
Imports System.ComponentModel

Public Class frmOSMTheBox


    Private Sub frmToolboxJupyterForBoston_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Me.WebBrowser.Dock = DockStyle.Fill
            'Me.WebBrowser.Navigate("http://www.google.com")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub PopulateDocumentText(ByVal asHTML As String)

        Try
            If Me.WebBrowser.Document Is Nothing Then
                Me.WebBrowser.DocumentText = asHTML
            Else
                Me.WebBrowser.Document.OpenNew(True)
                Me.WebBrowser.Document.Write(asHTML)
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub OpenURL(ByVal asURL As String)

        Try
            Me.WebBrowser.Navigate(asURL)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmOSMTheBox_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Try
            frmMain.mfrmOSMTheBox = Nothing
            Call prApplication.TriggerFormTheBoxClosed

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try


    End Sub

End Class