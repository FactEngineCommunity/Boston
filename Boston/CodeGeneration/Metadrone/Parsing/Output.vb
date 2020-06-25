Imports System.IO

Namespace Parser.Output


#Region "Output Collection"

    Public Class OutputCollection
        Inherits Collections.CollectionBase

        Private BasePath As String = ""

        Friend Sub New(ByVal BasePath As String)
            If String.IsNullOrEmpty(BasePath) Then Exit Sub

            If Not IO.Path.GetExtension(BasePath).Length = 0 Then
                Me.BasePath = IO.Path.GetDirectoryName(BasePath)
            Else
                Me.BasePath = BasePath
            End If
        End Sub

        Friend Sub Add(ByVal OutputItem As OutputItem)
            OutputItem.BasePath = Me.BasePath
            Me.List.Add(OutputItem)
        End Sub

        Friend Sub AddKeepOriginalBasePath(ByVal OutputItem As OutputItem)
            Me.List.Add(OutputItem)
        End Sub

        Friend Sub Remove(ByVal index As Integer)
            Me.List.RemoveAt(index)
        End Sub

        Friend Function Item(ByVal index As Integer) As OutputItem
            Return CType(Me.List(index), OutputItem)
        End Function

        Public Sub WriteAll()
            For Each o As OutputItem In Me
                o.Write()
            Next
        End Sub

    End Class

#End Region

    'This class is for safe zone merging
    Friend Class SafeZone
        Public LineNumberStart As Integer = 0
        Public LineNumberEnd As Integer = 0
        Public Value As New System.Text.StringBuilder()
    End Class


    Public Class OutputItem
        Private sb As System.Text.StringBuilder = Nothing

        Private BaseWritePath As String = Nothing
        Private WritePath As String = Nothing

        Friend Sub New(ByVal Text As String, ByVal Path As String)
            Me.sb = New System.Text.StringBuilder(Text)
            Me.WritePath = Path
        End Sub

        Friend Property BasePath() As String
            Get
                Return Me.BaseWritePath
            End Get
            Set(ByVal value As String)
                Me.BaseWritePath = value
            End Set
        End Property

        Public ReadOnly Property Path() As String
            Get
                Return System.IO.Path.Combine(Me.BasePath, Me.WritePath)
            End Get
        End Property

        Public ReadOnly Property Text() As String
            Get
                'Return merged with existing output
                Return Me.Merge(Me.Path).ToString
            End Get
        End Property

        Public Sub Write()
            'Don't write if not intended to
            If Me.WritePath Is Nothing Then Exit Sub

            'Get path
            Dim outPath As String = Me.Path
            Dim DirPath = System.IO.Path.GetDirectoryName(outPath)

            'Create directory for path
            If Not DirPath.Length = 0 Then
                If Not Directory.Exists(DirPath) Then Directory.CreateDirectory(DirPath)
            End If

            'Get and merge content
            Dim content As System.Text.StringBuilder = Me.Merge(outPath)

            'Write
            Using sw As New StreamWriter(outPath, False)
                sw.Write(content.ToString)
                sw.Close()
                If Not File.Exists(outPath) Then Throw New System.Exception("File " & outPath & " could not be created.")
            End Using
        End Sub

        'Merge output with destination file (if safe zones are employed)
        Private Function Merge(ByVal FilePath As String) As System.Text.StringBuilder
            'No safe zones to check
            If String.IsNullOrEmpty(PackageBuilder.PreProc.Safe_Begin) Then Return Me.sb
            If String.IsNullOrEmpty(PackageBuilder.PreProc.Safe_End) Then Return Me.sb

            'No file to merge
            If Not File.Exists(FilePath) Then Return Me.sb

            'Ready new content
            Dim sbNew As New System.Text.StringBuilder(Me.sb.ToString)
            sbNew.Replace(vbCrLf, vbLf)
            sbNew.Replace(vbCr, vbLf)
            sbNew.Replace(vbLf, System.Environment.NewLine)

            'Get the existing file contents
            Dim sbOld As System.Text.StringBuilder = Nothing
            Using sw As New StreamReader(FilePath)
                sbOld = New System.Text.StringBuilder(sw.ReadToEnd())
                sw.Close()
            End Using
            sbOld.Replace(vbCrLf, vbLf)
            sbOld.Replace(vbCr, vbLf)
            sbOld.Replace(vbLf, System.Environment.NewLine)

            'Find safe zones in old content
            Dim safes As New List(Of SafeZone)
            Dim sr As System.IO.StringReader = Nothing
            Dim lineNo As Integer = 0
            Try
                Dim safe As SafeZone = Nothing
                sr = New System.IO.StringReader(sbOld.ToString)
                While sr.Peek > -1
                    Dim line As String = sr.ReadLine()
                    lineNo += 1

                    If safe Is Nothing Then
                        'Looking for the start of a safe zone
                        'Found begin of safe zone, now look for the end.
                        If line.Trim.IndexOf(PackageBuilder.PreProc.Safe_Begin) = 0 Then
                            safe = New SafeZone()
                            safe.LineNumberStart = lineNo
                            safe.Value.AppendLine(line)
                        End If
                    Else
                        safe.Value.AppendLine(line)
                        'Found end of safe zone, start to look for the next new one.
                        If line.Trim.IndexOf(PackageBuilder.PreProc.Safe_End) = 0 Then
                            safe.LineNumberEnd = lineNo
                            safes.Add(safe)
                            safe = Nothing
                        End If
                    End If
                End While

                'Tidy up, if safe zone has started then the rest is a safe zone
                If safe IsNot Nothing Then
                    safe.LineNumberEnd = lineNo
                    safes.Add(safe)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing
            End Try

            'No safe zones
            If safes.Count = 0 Then Return sbNew

            'Insert safe zones into new
            Dim sbMerged As New System.Text.StringBuilder()
            Dim safeIdx As Integer = 0
            lineNo = 0
            Try
                sr = New System.IO.StringReader(sbNew.ToString)
                While sr.Peek > -1
                    Dim line As String = sr.ReadLine()
                    lineNo += 1

                    'Insert safe zone at this position
                    If safeIdx < safes.Count AndAlso safes(safeIdx).LineNumberStart = lineNo Then
                        sbMerged.Append(safes(safeIdx).Value.ToString)
                        lineNo += (safes(safeIdx).LineNumberEnd - safes(safeIdx).LineNumberStart) + 1
                        safeIdx += 1
                    End If
                    sbMerged.AppendLine(line)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing
            End Try

            'Add remaining safe zones
            While safeIdx < safes.Count
                'First pad up until the next safe zone
                While lineNo < safes(safeIdx).LineNumberStart - 1
                    sbMerged.AppendLine()
                    lineNo += 1
                End While

                sbMerged.Append(safes(safeIdx).Value.ToString)
                lineNo += (safes(safeIdx).LineNumberEnd - safes(safeIdx).LineNumberStart) + 1
                safeIdx += 1
            End While

            sbMerged.Replace(vbCrLf, vbLf)
            sbMerged.Replace(vbCr, vbLf)
            sbMerged.Replace(vbLf, System.Environment.NewLine)
            Return sbMerged
        End Function

    End Class

End Namespace