Imports System.Reflection
Imports System.Runtime.InteropServices


Public Class frmStartup

    Private WithEvents zrWebBrowser As New WebBrowser

    <ComVisible(True)>
    Public Class ScriptingHelper

        Public Sub NavigateTo(url As String)

            If MsgBox("Are you okay to leave Boston and open the site in your default browser", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then
                Process.Start(url)
            End If

        End Sub

    End Class

    Private Sub frmStartup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Dim lsApplicationPath As String = Boston.MyPath

            If My.Settings.UseClientServer Then
                Me.WebBrowser.Navigate("file:\\" & lsApplicationPath & "\startup\indexClientServer.html")
            Else
                Me.WebBrowser.Navigate("file:\\" & lsApplicationPath & "\startup\index.html")
            End If


            Me.zrWebBrowser = Me.WebBrowser

            Dim loHelper As New ScriptingHelper
            Me.zrWebBrowser.ObjectForScripting = loHelper

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


    Private Sub zrWebBrowser_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles zrWebBrowser.DocumentCompleted

        Me.Cursor = Cursors.Default
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub zrWebBrowser_Navigated(sender As Object, e As WebBrowserNavigatedEventArgs) Handles zrWebBrowser.Navigated

        Cursor.Current = Cursors.Default

    End Sub


    Private Sub zrWebBrowser_Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles zrWebBrowser.Navigating

        Me.Cursor = Cursors.WaitCursor

        If Me.zrWebBrowser.ReadyState = WebBrowserReadyState.Loading Then
            Cursor.Current = Cursors.WaitCursor
        Else
            Cursor.Current = Cursors.Default
        End If

        Dim lasURLArgument() As String
        Dim lsModelObjectName As String

        lasURLArgument = e.Url.ToString.Split(":")

        If lasURLArgument(0) = "function" Then

            lsModelObjectName = lasURLArgument(1)

            '------------------------------------------------------------------------------------------
            'Cancel the Navigation so that the new verbalisation isn't wiped out.
            '  i.e. Because the Navication (e.URL) isn't to an actual URL, an error WebPage is shown,
            '  rather than the new Verbalisation. Cancelling the Navigation fixes this.
            '------------------------------------------------------------------------------------------
            e.Cancel = True

            Select Case lasURLArgument(1)
                Case Is = "NavigateTo"
                    Dim liIndex = e.Url.ToString.IndexOf("NavigateTo:")
                    Dim urlPart As String = e.Url.ToString.Substring(liIndex + "NavigateTo:".Length)
                    Call Me.zrWebBrowser.ObjectForScripting.navigateTo(urlPart)
                Case Is = "AddNewModel"
                    If frmMain.zfrmModelExplorer IsNot Nothing Then
                        Me.Cursor = Cursors.WaitCursor
                        Call frmMain.zfrmModelExplorer.addNewModelToBoston()
                        Me.Cursor = Cursors.Default
                    Else
                        MsgBox("Load the Model Explorer first. Select the [View]->[Model Explorer] menu option.")
                    End If
            End Select


        End If

        'e.Cancel = True


    End Sub

    Private Sub frmStartup_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

        frmMain.zfrmStartup = Nothing

    End Sub
End Class