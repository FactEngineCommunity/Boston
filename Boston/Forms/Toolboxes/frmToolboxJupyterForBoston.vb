Imports JupyterNetClient
Imports System.Windows.Forms.Integration
Imports System.Reflection


Public Class frmToolboxJupyterForBoston
    Private Sub frmToolboxJupyterForBoston_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try

            '===========OLD================================
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


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class