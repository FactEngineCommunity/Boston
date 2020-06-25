Friend Class ConsoleExec

    Private WithEvents mdMain As New Parser.Main()

    Private SuccessCount As Integer = 0
    Private FailCount As Integer = 0

    Private LogFile As String = Nothing

    Private Class Console
        Declare Function AttachConsole Lib "kernel32.dll" (ByVal dwProcessId As Int32) As Boolean
        Declare Function FreeConsole Lib "kernel32.dll" () As Boolean
    End Class

    Public Sub New(ByVal LogFile As String)
        Me.LogFile = LogFile
    End Sub

    Friend Sub BuildProject(ByVal ProjectPath As String)
        'Parse synchronously
        My.Application.EXEC_ParseInThread = False

        'Open project
        Me.WriteLine(Nothing)
        Me.WriteLine("Opening " & ProjectPath & "...")
        Me.mdMain.LoadProject(ProjectPath)
        Me.WriteLine(Nothing)

        'Build
        Me.mdMain.BuildProject()
    End Sub

    Friend Shared Sub AttachConsole()
        Console.AttachConsole(-1)
    End Sub

    Friend Shared Sub FreeConsole()
        Console.FreeConsole()
    End Sub

    Friend Sub Write(ByVal message As String)
        System.Console.Write(message)

        If Not String.IsNullOrEmpty(Me.LogFile) Then
            Dim sw As New System.IO.StreamWriter(LogFile, True)
            sw.Write(message)
            sw.Close()
        End If
    End Sub

    Friend Sub WriteLine(ByVal message As String)
        If message Is Nothing Then
            System.Console.WriteLine()
        Else
            System.Console.WriteLine(message)
        End If

        If Not String.IsNullOrEmpty(Me.LogFile) Then
            Dim sw As New System.IO.StreamWriter(LogFile, True)
            sw.WriteLine(message)
            sw.Close()
        End If
    End Sub

    Private Sub mdMain_Notify(ByVal Message As String) Handles mdMain.Notify
        Me.Write(Message)
    End Sub

End Class
