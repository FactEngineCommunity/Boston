Imports System.Text.RegularExpressions
Imports System.IO
Imports NAudio.Wave.SampleProviders
Imports NAudio.Wave
Imports NAudio.Lame

Namespace BackCast
    Public Class Dialog

        Public Utterance As New List(Of BackCast.Utterance)

        Public ReadOnly Property CombinedBytes As Byte()
            Get
                Return Me.Utterance.Select(Function(utterance) utterance.Bytes).Aggregate(Function(a, b) Enumerable.Concat(a, b).ToArray())
            End Get
        End Property


        Public Sub PopulateDialog(aarVoice As List(Of Voice), asDialogText As String)

            Dim utterances As New List(Of Utterance)

            ' Split the dialog into individual lines
            Dim lines As String() = Regex.Split(asDialogText, "\r\n|\r|\n")

            ' Regular expression patterns to match speaker and their text            

            ' Iterate over each line and process it
            For Each line As String In lines

                Dim utterance As New Utterance()

                ' Match Victor's utterance
                For Each lrVoice In aarVoice
                    Dim lsMatchPattern As String = "^" & lrVoice.name.Trim & ": \s*(.*)$"

                    Dim lrMatch As Match = Regex.Match(line, lsMatchPattern)
                    If lrMatch.Groups.Count = 0 Then GoTo NextUtterance
                    If lrMatch.Success Then
                        utterance.Voice = lrVoice
                        utterance.Text = lrMatch.Groups(1).Value.Trim()
                        Exit For
                    End If
                Next

                ' Add the utterance to the list
                If Not Trim(utterance.Text) = "" Then
                    utterances.Add(utterance)
                End If
NextUtterance:
            Next

            ' Set the utterances in the dialog
            Me.Utterance = utterances
        End Sub

        Public Sub SaveAsMP3(speed As Single)
            ' Create an instance of the OpenFileDialog class
            Dim saveFileDialog As New SaveFileDialog()

            ' Set the default file extension and filter
            saveFileDialog.DefaultExt = ".mp3"
            saveFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3"
            saveFileDialog.FileName = "MyBackCastDialog"

            ' Show the file selection dialog
            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                '============was=============================
                '' Get the selected file path
                'Dim filePath As String = saveFileDialog.FileName

                '' Get the combined audio bytes from the Dialog class property
                'Dim combinedBytes As Byte() = Me.CombinedBytes

                '' Save the combined bytes as an MP3 file
                'File.WriteAllBytes(filePath, combinedBytes)
                '=============================================
                ' Get the selected file path
                Dim filePath As String = saveFileDialog.FileName

                ' Get the combined audio bytes from the Me.CombinedBytes variable
                Dim combinedBytes As Byte() = Me.CombinedBytes

                ' Create a MemoryStream from the combined audio bytes
                Using inputStream As New MemoryStream(combinedBytes)
                    ' Create an instance of Mp3FileReader to read the audio data
                    Dim audioReader As New Mp3FileReader(inputStream)

                    ' Calculate the adjusted length based on the speed
                    Dim adjustedLengthSeconds As Double = audioReader.TotalTime.TotalSeconds / speed
                    Dim adjustedLength As TimeSpan = TimeSpan.FromSeconds(adjustedLengthSeconds)

                    ' Create an instance of WaveFormatConversionStream to convert the audio format
                    Dim formatConversionStream As New WaveFormatConversionStream(New WaveFormat(44100, 2), audioReader)

                    ' Create an instance of MediaFoundationResampler to resample the audio
                    Dim resampler As New MediaFoundationResampler(formatConversionStream, New WaveFormat(44100, 2))

                    ' Calculate the resampling ratio based on the desired speed
                    Dim resampledSampleRate As Integer = CInt(resampler.WaveFormat.SampleRate * speed)
                    Dim resampledWaveFormat As New WaveFormat(resampledSampleRate, resampler.WaveFormat.Channels)

                    ' Create an instance of WaveFileWriter to write the slowed down audio as an MP3 file
                    Using mp3Writer As New LameMP3FileWriter(filePath, resampledWaveFormat, 128)
                        ' Read the resampled audio samples and write them to the MP3 file
                        Dim buffer(4096) As Byte
                        Dim bytesRead As Integer
                        Dim totalBytesToWrite As Integer = CInt(resampledWaveFormat.AverageBytesPerSecond * adjustedLength.TotalSeconds)
                        Dim bytesWritten As Integer = 0

                        Do
                            bytesRead = resampler.Read(buffer, 0, buffer.Length)
                            If bytesRead > 0 Then
                                Dim bytesToWrite As Integer = Math.Min(bytesRead, totalBytesToWrite - bytesWritten)
                                mp3Writer.Write(buffer, 0, bytesToWrite)
                                bytesWritten += bytesToWrite
                            End If
                        Loop While bytesRead > 0 AndAlso bytesWritten < totalBytesToWrite
                    End Using
                End Using
            End If

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <param name="speed">A value of 1.0 represents normal speed, 2.0 represents double speed (twice as fast), and 0.5 represents half speed (half as fast)</param>
        Private Sub PlayMP3FileWithSpeed(ByVal filePath As String, ByVal speed As Single)
            ' Create an instance of AudioFileReader to read the MP3 file
            Dim audioFileReader As New AudioFileReader(filePath)

            ' Adjust the playback speed by changing the sample rate
            Dim adjustedSampleRate As Integer = CInt(audioFileReader.WaveFormat.SampleRate * speed)
            Dim adjustedWaveFormat As New WaveFormat(adjustedSampleRate, audioFileReader.WaveFormat.Channels)

            ' Create an instance of ResamplerDmoStream to resample the audio data
            Dim resampler As New ResamplerDmoStream(audioFileReader, adjustedWaveFormat)

            ' Create an instance of WaveOutEvent for audio playback
            Dim waveOut As New WaveOutEvent()

            ' Set the resampler as the audio source for the wave output
            waveOut.Init(resampler)

            ' Start playback
            waveOut.Play()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="mp3Bytes"></param>
        ''' <param name="speed">A value of 1.0 represents normal speed, 2.0 represents double speed (twice as fast), and 0.5 represents half speed (half as fast)</param>
        Private Sub PlayMP3ByteArrayWithSpeed(ByVal mp3Bytes As Byte(), ByVal speed As Single)
            ' Create a MemoryStream from the MP3 byte array
            Dim memStream As New MemoryStream(mp3Bytes)

            ' Create an instance of CustomStream to read audio data from the MemoryStream
            Dim audioStream As New CustomStream(memStream)

            ' Create an instance of WaveOutEvent for audio playback
            Dim waveOut As New WaveOutEvent()

            ' Adjust the playback speed by changing the sample rate
            audioStream.SetSpeed(speed)

            ' Set the audio stream as the audio source for the wave output
            waveOut.Init(audioStream)

            ' Start playback
            waveOut.Play()
        End Sub

        ' CustomStream implementation to read audio data from MemoryStream
        Private Class CustomStream
            Inherits WaveStream

            Private memStream As MemoryStream
            Private speed As Single

            Private _Length As Long
            Public Overrides ReadOnly Property Length As Long
                Get
                    Return memStream.Length
                End Get
            End Property

            Private _Position As Long
            Public Overrides Property Position As Long
                Get
                    Return memStream.Position
                End Get
                Set(value As Long)
                    memStream.Position = value
                End Set
            End Property


            Public Sub New(ByVal stream As MemoryStream)
                Me.memStream = stream
                Me.speed = 1.0F
            End Sub

            Public Sub SetSpeed(ByVal speed As Single)
                Me.speed = speed
            End Sub

            Public Overrides ReadOnly Property WaveFormat As WaveFormat
                Get
                    ' Return the wave format of the MP3 audio (e.g., 44.1kHz stereo with a block size of 1152 and a bitrate of 128 kbps)
                    Return New Mp3WaveFormat(44100, 2, 1152, 128)
                End Get
            End Property


            Public Overrides Function Seek(ByVal offset As Long, ByVal origin As SeekOrigin) As Long
                Return memStream.Seek(offset, origin)
            End Function

            Public Overrides Function Read(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer) As Integer
                Dim bytesRead As Integer = memStream.Read(buffer, offset, count)

                ' Adjust the playback speed by modifying the number of bytes read
                bytesRead = CInt(bytesRead * speed)

                Return bytesRead
            End Function
        End Class

    End Class

    Public Class Utterance

        Public SequenceNr As Integer = 1

        Public Voice As BackCast.Voice

        Public Text As String

        Public Bytes As Byte()

        Public Sub PlayIt()

            Using inputStream As New MemoryStream(Me.Bytes)
                Using waveStream As WaveStream = New Mp3FileReader(inputStream)
                    Using outputDevice As IWavePlayer = New WaveOutEvent()
                        outputDevice.Init(waveStream)
                        outputDevice.Play()
                        While outputDevice.PlaybackState = PlaybackState.Playing
                            System.Threading.Thread.Sleep(100)
                        End While
                    End Using
                End Using
            End Using

        End Sub

    End Class

End Namespace
