Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports System.Reflection

<CLSCompliant(True)> _
Public Class tErrorLogger

    Private ErrorLogFilePath As String = ""

    Private syncObject As New Object

    ''' <summary>
    ''' Parameterless Constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        Try
            ErrorLogFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\Errors\"

            If Not System.IO.Directory.Exists(ErrorLogFilePath) Then
                System.IO.Directory.CreateDirectory(ErrorLogFilePath)
            End If

            '--------------------------
            'Check if the file exists
            '--------------------------
            If File.Exists(Me.ErrorLogFilePath & "errlog.txt") Then
                '-----------------------------------
                'All good. The file already exists
                '-----------------------------------
                Dim fs As FileStream = New FileStream(Me.ErrorLogFilePath & "errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
                fs.Close()
            Else
                Dim fs As FileStream = New FileStream(Me.ErrorLogFilePath & "errlog.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
                Dim s As StreamWriter = New StreamWriter(fs)
                s.Close()
                fs.Close()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            MsgBox(lsMessage)
        End Try

    End Sub

    ''' <summary>
    ''' Open or create an error log and submit error message.
    ''' </summary>
    ''' <param name="msg">message to be written to error file</param>
    ''' <param name="stkTrace">stack trace from error message</param>
    ''' <param name="title">title of the error file entry</param>
    ''' <remarks>RETURNS: Nothing</remarks>
    Public Sub WriteToErrorLog(ByVal msg As String, ByVal stkTrace As String, ByVal title As String)

        'check and make the directory if necessary; this is set to look in the application
        'folder, you may wish to place the error log in another location depending upon the
        'the user's role and write access to different areas of the file system        

        Try
            Select Case My.Settings.DebugMode
                Case Is = pcenumDebugMode.Debug.ToString, _
                          pcenumDebugMode.DebugCriticalErrorsOnly.ToString

                    SyncLock Me.syncObject
                        'log it
                        Dim fs1 As FileStream = New FileStream(Me.ErrorLogFilePath & "errlog.txt", FileMode.Append, FileAccess.Write)
                        Dim s1 As StreamWriter = New StreamWriter(fs1)
                        s1.Write("Title: " & title & vbCrLf)
                        s1.Write("Message: " & msg & vbCrLf)
                        s1.Write("StackTrace: " & stkTrace & vbCrLf)
                        s1.Write("Date/Time: " & DateTime.Now.ToString() & ":" & DateTime.UtcNow.Millisecond & vbCrLf)
                        s1.Write("===========================================================================================" & vbCrLf)
                        s1.Close()
                        fs1.Close()
                    End SyncLock

                Case Is = pcenumDebugMode.NoLogging.ToString
                    '------------
                    'No logging
                    '------------
            End Select

            'Public Sub Logit(ByVal Message As String, Optional ByVal AddToSystemApplicationLog As Boolean = False)
            '    Static LogLines As New List(Of String)
            '    If LogLines.Count = 0 Then
            '        If Me.LogFileName.Exists Then
            '            Try
            '                LogLines = IO.File.ReadAllLines(Me.LogFileName.FullName).ToList
            '            Catch ex As Exception
            '                Try
            '                    MakeEventlogEntry("Cannot read log file!")
            '                Catch exx As Exception
            '                    Exit Sub 'There's no place to log anything
            '                End Try
            '                Exit Sub  'there's nothing we can do really...
            '            End Try
            '        End If
            '    End If

            '    'clean up the message
            '    If Message.EndsWith(Environment.NewLine) Then
            '        Message = Message.Substring(0, Message.Length - 2)
            '    End If
            '    Message = System.DateTime.Now & "     " & Message
            '    LogLines.Add(Message)

            '    'make sure we keep only MaxEntries entries
            '    Do While LogLines.Count > Me.MaxLogLines
            '        LogLines.RemoveAt(0)
            '    Loop

            '    Try
            '        IO.File.WriteAllLines(Me.LogFileName.FullName, LogLines)
            '        If AddToSystemApplicationLog Then
            '            MakeEventlogEntry(Message)
            '        End If
            '    Catch ex As Exception
            '        Try
            '            MakeEventlogEntry("Cannot write to log file!")
            '        Catch exx As Exception
            '            Exit Sub  'There's no place to log anything
            '        End Try
            '        Exit Sub 'There's no where to log it so just ignore error
            '    End Try

            'End Sub

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            MsgBox(lsMessage)            

        End Try

    End Sub

End Class
