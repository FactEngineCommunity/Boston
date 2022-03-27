Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Text

Public Class frmMain

    ' Step 1 Create a URI to serve as the base address.
    ' Ex. "http://localhost:9001/WCFServices/"
    Dim baseAddress As New Uri("http://localhost:9001/WCFServices/")

    ' Step 2 Create a ServiceHost instance
    Dim selfHost As New ServiceHost(GetType(BostonWCFServiceLibrary.DuplexService), baseAddress)

    Public Class ConsoleWriterEventArgs
        Inherits EventArgs

        Public Property ValueString As String

        Public Sub New(ByVal value As String)
            ValueString = value
        End Sub
    End Class

    Public Class ConsoleWriter
        Inherits TextWriter

        Public Overrides ReadOnly Property Encoding As Encoding
            Get
                Return Encoding.UTF8
            End Get
        End Property

        Public Overrides Sub Write(ByVal value As String)
            RaiseEvent WriteEvent(Me, New ConsoleWriterEventArgs(value))
            MyBase.Write(value)
        End Sub

        Public Overrides Sub WriteLine(ByVal value As String)
            RaiseEvent WriteLineEvent(Me, New ConsoleWriterEventArgs(value))
            MyBase.WriteLine(value)
        End Sub

        Public Event WriteEvent As EventHandler(Of ConsoleWriterEventArgs)

        Public Event WriteLineEvent As EventHandler(Of ConsoleWriterEventArgs)
    End Class

    Public WithEvents zrConsoleWriter As New ConsoleWriter

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        If (Me.WindowState = FormWindowState.Minimized) Then
            NotifyIcon.Visible = True
            Me.Hide()
        End If

    End Sub

    Private Sub MaximiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaximiseToolStripMenuItem.Click

        Me.Show()
        Me.WindowState = FormWindowState.Normal

    End Sub


    Private Sub QuitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles QuitToolStripMenuItem.Click

        'Close the ServiceHostBase to shutdown the service.
        selfHost.Close()

        Me.Close()
        Me.Dispose()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Console.SetOut(zrConsoleWriter)

            ' Step 3 Add a service endpoint.
            Dim loBinding As New WSDualHttpBinding(WSDualHttpSecurityMode.None)
            'loBinding.OpenTimeout = New TimeSpan(0, 1, 30)
            'loBinding.CloseTimeout = New TimeSpan(0, 0, 10)
            loBinding.SendTimeout = New TimeSpan(0, 0, 10)
            loBinding.ReceiveTimeout = New TimeSpan(0, 30, 0)


            selfHost.AddServiceEndpoint(GetType(BostonWCFServiceLibrary.IDuplexService), loBinding, "DuplexService")

            ' Step 4 Enable Metadata Exchange and Add MEX endpoint
            Dim smb As New ServiceMetadataBehavior()
            smb.HttpGetEnabled = True

            selfHost.Description.Behaviors.Add(smb)
            selfHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), Convert.ToString(Me.baseAddress) & "mex")

            ' Step 5 Start the service.
            selfHost.Open()
            Me.ListBox.Items.Insert(0, "Press <ENTER> to terminate service.")
            Me.ListBox.Items.Insert(0, "Listening at: " & baseAddress.ToString)
            Me.ListBox.Items.Insert(0, "The service is ready.")
            Me.ListBox.Items.Add("")

        Catch ce As CommunicationException
            MsgBox("An exception occurred: " & ce.Message)
            selfHost.Abort()
            Call Me.Close()
            Call Me.Dispose()
        End Try

    End Sub


    Private Sub EndSessionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EndSessionToolStripMenuItem.Click

        Dim lsMessage As String = ""

        lsMessage = "Are you absolutely sure you would like to terminate the Boston Server?"
        lsMessage &= vbCrLf & vbCrLf
        lsMessage &= "First check to see that there are no instances of Boston open."

        If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            'Close the ServiceHostBase to shutdown the service.
            selfHost.Close()

            Me.Close()
            Me.Dispose()

        End If

    End Sub


    Private Sub zrConsoleWriter_WriteLineEvent(sender As Object, e As ConsoleWriterEventArgs) Handles zrConsoleWriter.WriteLineEvent

        If e.ValueString IsNot Nothing Then
            Me.ListBox.Items.Insert(0, Now.ToString & "  :" & e.ValueString)
        End If

        If Me.ListBox.Items.Count > 50 Then
            Me.ListBox.Items.RemoveAt(49)
        End If

    End Sub

    Private Sub NotifyIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseClick

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Show()
            Me.WindowState = FormWindowState.Normal
        End If

    End Sub

    Private Sub NotifyIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon.MouseDoubleClick

        Me.Show()
        Me.WindowState = FormWindowState.Normal

    End Sub

    Private Sub MinimiseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimiseToolStripMenuItem.Click

        Me.WindowState = FormWindowState.Minimized

    End Sub

    Private Sub MaximiseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MaximiseToolStripMenuItem1.Click

        Me.WindowState = FormWindowState.Maximized

    End Sub

    Private Sub NormalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NormalToolStripMenuItem.Click

        Me.WindowState = FormWindowState.Normal

    End Sub
End Class
