Imports JupyterNetClient
Imports System.Windows.Forms.Integration
Imports System.Reflection
Imports CefSharp.WinForms
Imports CefSharp

Public Class frmToolboxJupyterForBoston

    Private WithEvents moCefBrowser As New ChromiumWebBrowser

    Private Sub frmToolboxJupyterForBoston_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            '===========OLD================================
#Region "trying to host a WCF app"
            ''Create the ElementHost control for hosting the
            ''WPF UserControl.
            'Dim host As New ElementHost()
            'host.Dock = DockStyle.Fill

            ''Create the WPF UserControl.
            'Dim uc As New JupiterNet.View.MainWindow

            ''Assign the WPF UserControl to the ElementHost control's
            ''Child property.
            'host.Child = uc

            ''Add the ElementHost control to the form's
            ''collection of child controls.
            'Me.Controls.Add(host)            
#End Region

            'Use this CommandLine to start Jupyter.
            'jupyter notebook  --NotebookApp.token=abcd --notebook-dir="D:\Temp"

            Dim loCefSettings As New CefSettings
            If Not Cef.IsInitialized Then
                Cef.Initialize(loCefSettings)
            End If

            Me.Panel.Controls.Add(moCefBrowser)
            moCefBrowser.Dock = DockStyle.Fill

            moCefBrowser.LoadUrl("http://127.0.0.1:8888\tree")

            'Test ChatBase.co chatbot.
            'Dim lsApplicationPath As String = Boston.MyPath
            'If My.Settings.UseClientServer Then
            '    moCefBrowser.LoadUrl("file:\\" & lsApplicationPath & "\startup\indexClientServer.html")
            'Else
            '    moCefBrowser.LoadUrl("file:\\" & lsApplicationPath & "\startup\index.html")
            'End If



        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click

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

End Class