Imports System
Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports NAudio.Wave
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Linq
Imports System.Reflection

Public Class AssemblyAIRealtimeClient
    Implements IDisposable

    Private _webSocket As ClientWebSocket
    Private _waveIn As WaveInEvent
    Private _bufferedWaveProvider As BufferedWaveProvider
    Private OutputTextbox As RichTextBox

    Private IsDisposed As Boolean = False
    Private IsDisposing As Boolean = False

    Public Sub New(ByRef aoTextbox As RichTextBox)

        Try
            If Me.IsDisposing Then Exit Sub
            If Me.IsDisposed Then Exit Sub

            Me.OutputTextbox = aoTextbox

            _webSocket = New ClientWebSocket()

            ' Setup NAudio WaveInEvent
            _waveIn = New WaveInEvent() With {
                    .WaveFormat = New WaveFormat(16000, 1)
                }

            _bufferedWaveProvider = New BufferedWaveProvider(_waveIn.WaveFormat)

            _bufferedWaveProvider.DiscardOnBufferOverflow = True

            ' Handling the DataAvailable event
            If _bufferedWaveProvider IsNot Nothing Then
                If Me.IsDisposed Then Exit Sub
                AddHandler _waveIn.DataAvailable, Sub(s, e) _bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded)
            End If

            '        Sub(s, e)
            '    ' Check if adding new samples exceeds the buffer length
            '    If _bufferedWaveProvider.BufferedBytes + e.BytesRecorded > _bufferedWaveProvider.BufferLength Then
            '        ' If yes, discard the data (or handle it as needed)
            '        ' Optionally, you can log this event or take other actions
            '        Debugger.Break()
            '    Else
            '        ' If no, add the samples to the buffer
            '        _bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded)
            '    End If
            'End Sub

            _waveIn.StartRecording()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Public Async Function StartTranscriptionAsync() As Task

        Try
            Dim apiKey As String = My.Settings.OSMAssemblyAIAPIKey

            Try
                _webSocket.Options.SetRequestHeader("Authorization", apiKey)
                Await _webSocket.ConnectAsync(New Uri("wss://api.assemblyai.com/v2/realtime/ws?sample_rate=16000"), CancellationToken.None)
            Catch ex As Exception
                'Probably cannot connect to the remote server.
                Return
            End Try

            poCancellationTokenSource = New CancellationTokenSource
            Dim loCancellationToken As CancellationToken = poCancellationTokenSource.Token

            Await Task.WhenAll(SendAudioDataAsync(), ReceiveMessagesAsync(loCancellationToken))

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Async Function SendAudioDataAsync() As Task

        Try
            If Me.IsDisposed Then Return
            Dim buffer(4095) As Byte
            While True
                If _bufferedWaveProvider.BufferedBytes > buffer.Length Then
                    Dim bytesRead = _bufferedWaveProvider.Read(buffer, 0, buffer.Length)
                    If Me.IsDisposed Then Return
                    Await _webSocket.SendAsync(New ArraySegment(Of Byte)(buffer, 0, bytesRead), WebSocketMessageType.Binary, True, CancellationToken.None)
                Else
                    Await Task.Delay(50) ' Reduce CPU usage
                End If
            End While

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            '20240109-Errors, but have no solution at this stage.
            'prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, abUseFlashCard:=True)
        End Try
    End Function

    Private lastIndex As Integer = -1 ' Member variable to keep track of the last index processed
    Private Timeout As Integer = 0
    Private Async Function ReceiveMessagesAsync(aoCancellationToken As CancellationToken) As Task
        Dim buffer(4095) As Byte
        Dim result As WebSocketReceiveResult

        Try
            If aoCancellationToken.IsCancellationRequested Or poCancellationTokenSource.IsCancellationRequested Then
                result = Await _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), aoCancellationToken)
                Call Me.Dispose()
                Return
            End If
            Do
                ' Check for cancellation
                If aoCancellationToken.IsCancellationRequested Then
                    Exit Do
                End If

                '    result = Await _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), CancellationToken.None)
                '    Dim message = Encoding.UTF8.GetString(buffer, 0, result.Count)
                '    OutputTextbox.Invoke(Sub()
                '                             Dim json As JObject = JObject.Parse(message)
                '                             Dim wordsArray As JArray = json("words")

                '                             If wordsArray IsNot Nothing AndAlso wordsArray.Count > lastIndex + 1 Then
                '                                 ' Process only new words
                '                                 Dim newWords = wordsArray _
                '                                                 .Skip(lastIndex + 1) _
                '                                                 .Select(Function(w) w("text").ToString())
                '                                 Dim words As String = String.Join(" ", newWords)
                '                                 OutputTextbox.AppendText(" " & words)

                '                                 ' Update the lastIndex
                '                                 lastIndex = wordsArray.Count - 1
                '                             ElseIf wordsArray IsNot Nothing AndAlso wordsArray.Count = 0 Then
                '                                 Timeout += 1
                '                             End If

                '                             If Timeout > 2 Then
                '                                 lastIndex = -1
                '                                 Timeout = 0
                '                             End If
                '                         End Sub)
                'Loop While Not result.CloseStatus.HasValue

#Region "Old Duplex Code"
                result = Await _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), aoCancellationToken)
                Dim message = Encoding.UTF8.GetString(buffer, 0, result.Count)
                If message Is Nothing Or message = "" Then Exit Do
                OutputTextbox.Invoke(Sub()
                                         Dim json As JObject = JObject.Parse(message)
                                         Dim wordsArray As JArray = json("words")

                                     If wordsArray IsNot Nothing AndAlso wordsArray.Count > lastIndex + 1 Then
                                         ' Process only new words
                                         Dim newWords = wordsArray _
                                                     .Skip(lastIndex + 1) _
                                                     .Select(Function(w) w("text").ToString())
                                         Dim words As String = String.Join(" ", newWords)
                                         OutputTextbox.AppendText(" " & words)

                                         ' Update the lastIndex
                                         lastIndex = wordsArray.Count - 1
                                     ElseIf wordsArray IsNot Nothing AndAlso wordsArray.Count = 0 Then
                                         Timeout += 1
                                     End If

                                     If Timeout > 2 Then
                                         lastIndex = -1
                                         Timeout = 0
                                     End If
                                 End Sub)
            Loop While Not result.CloseStatus.HasValue AndAlso Not aoCancellationToken.IsCancellationRequested And Not Me.IsDisposing Or Me.IsDisposed
#End Region

        Catch ex As OperationCanceledException
            ' Handle the cancellation
            ' No need to call poCancellationTokenSource.Cancel() here as the token is already cancelled
        Catch ex As Exception

        Finally
            Me.Dispose()
        End Try
    End Function

    'Private Async Function ReceiveMessagesAsync() As Task
    '    Dim buffer(4095) As Byte
    '    Dim result As WebSocketReceiveResult
    '    Do
    '        result = Await _webSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), CancellationToken.None)
    '        Dim message = Encoding.UTF8.GetString(buffer, 0, result.Count)
    '        OutputTextbox.Invoke(Sub()
    '                                 Dim json As JObject = JObject.Parse(message)
    '                                 Dim wordsArray As JArray = json("words")

    '                                 ' Assuming each element in the words array is a string
    '                                 If wordsArray IsNot Nothing Then
    '                                     Dim words As String = String.Join(" ", wordsArray.Select(Function(w) w("text").ToString()))
    '                                     OutputTextbox.AppendText(words)
    '                                 End If
    '                             End Sub)
    '    Loop While Not result.CloseStatus.HasValue
    'End Function

    Public Sub Dispose() Implements IDisposable.Dispose

        If Me.IsDisposed Then Return ' Prevent double disposal.

        Try
            Me.IsDisposing = True

            If _webSocket IsNot Nothing Then
                Try
                    _webSocket.Dispose()
                Catch ex As ObjectDisposedException
                    ' Object was already disposed.
                End Try
                _webSocket = Nothing
            End If

            If _waveIn IsNot Nothing Then
                Try
                    _waveIn.StopRecording()
                    '_waveIn.Dispose() Was trying to access memory not availabel etc
                Catch ex As ObjectDisposedException
                    ' Object was already disposed.
                End Try
                _waveIn = Nothing
            End If

            _bufferedWaveProvider = Nothing
        Catch ex As Exception
            ' Log the exception or handle it as needed.
        Finally
            Me.IsDisposing = False
            Me.IsDisposed = True
            GC.SuppressFinalize(Me)
        End Try

        'Try
        '    If Me.IsDisposed Then Exit Sub

        '    Me.IsDisposing = True
        '    _webSocket?.Dispose()
        '    _waveIn?.StopRecording()
        '    _waveIn?.Dispose()
        '    _bufferedWaveProvider = Nothing
        '    Me.IsDisposed = True
        '    Me.IsDisposing = False
        'Catch
        '    'Probably already disposed.
        'End Try
    End Sub
End Class

