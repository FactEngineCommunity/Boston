Imports System.Text
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Tools

    Friend Class Reflection

        Public Shared Function Serialise(ByVal Obj As Object, ByVal [Namespace] As String) As String
            If Obj Is Nothing Then Return Nothing

            Dim x As System.Xml.Serialization.XmlSerializer
            If [Namespace] IsNot Nothing AndAlso Not [Namespace].Length = 0 Then
                x = New System.Xml.Serialization.XmlSerializer(Obj.GetType, [Namespace])
            Else
                x = New System.Xml.Serialization.XmlSerializer(Obj.GetType)
            End If
            Dim sw As New IO.StringWriter()
            x.Serialize(sw, Obj)
            Return sw.ToString
        End Function

        Public Shared Function Deserialise(ByVal xml As String, ByVal type As System.Type, ByVal [Namespace] As String) As Object
            If xml Is Nothing Then Return Nothing
            If xml.Length = 0 Then Return Nothing

            Dim x As System.Xml.Serialization.XmlSerializer
            If [Namespace] IsNot Nothing AndAlso Not [Namespace].Length = 0 Then
                x = New System.Xml.Serialization.XmlSerializer(type, [Namespace])
            Else
                x = New System.Xml.Serialization.XmlSerializer(type)
            End If
            Dim sr As New IO.StringReader(xml)
            Return x.Deserialize(sr)
        End Function

        ''' <summary>
        ''' Converts an object to bytes and returns the byte array
        ''' </summary>
        ''' <param name="object">The object to convert into bytes</param>
        ''' <exception cref="Exception">Thrown in any event with the inner exception being the original exception thrown.</exception>
        Public Shared Function ConvertObjectToBytes(ByVal [object] As Object) As Byte()
            Dim byReturn As Byte() = Nothing
            Dim sStream As IO.MemoryStream = Nothing

            Try
                'Create stream
                sStream = New IO.MemoryStream()
                'Initialize BinaryFormater
                Dim bfTemp As New BinaryFormatter
                'Serializes any object into the memorystream
                bfTemp.Serialize(sStream, [object])
                'Returns the stream into an array of bytes
                byReturn = sStream.ToArray

            Catch ex As Exception
                Throw New Exception(String.Format("Exception occured in {0}:{1}{2}", _
                                                  System.Reflection.MethodBase.GetCurrentMethod().Name, _
                                                  System.Environment.NewLine & System.Environment.NewLine, ex.Message), ex)
            Finally
                If Not sStream Is Nothing Then sStream = Nothing
            End Try

            Return byReturn

        End Function

        ''' <summary>
        ''' Converts an array of bytes into an object
        ''' </summary>
        ''' <param name="bytes">The array of bytes to convert</param>
        ''' <exception cref="Exception">Thrown in any event with the inner exception being the original exception thrown.</exception>
        Public Shared Function ConvertBytesToObject(ByVal [bytes] As Byte()) As Object
            Dim oReturn As Object = Nothing
            Dim sStream As IO.MemoryStream = Nothing

            Try
                'Create stream
                sStream = New IO.MemoryStream(bytes)
                'Initialize BinaryFormater
                Dim bfTemp As New BinaryFormatter
                'Return stream to the beginning
                sStream.Position = 0
                'Deserialize stream into object of any type
                oReturn = bfTemp.Deserialize(sStream)

            Catch ex As Exception
                Throw New Exception(String.Format("Exception occured in {0}:{1}{2}", _
                                                  System.Reflection.MethodBase.GetCurrentMethod().Name, _
                                                  System.Environment.NewLine & System.Environment.NewLine, ex.Message), ex)
            Finally
                If Not sStream Is Nothing Then sStream = Nothing
            End Try

            Return oReturn
        End Function

    End Class



    Friend Class Compression

        'From http://social.msdn.microsoft.com/Forums/en-US/architecturegeneral/thread/bdec22aa-6d3a-41e3-8eb8-9aa68b993364/

        'Public Shared Function Compress(ByVal text As String) As String
        '    Dim buffer As Byte() = Encoding.UTF8.GetBytes(text)
        '    Dim ms As New MemoryStream()
        '    Using zip As New GZipStream(ms, CompressionMode.Compress, True)
        '        zip.Write(buffer, 0, buffer.Length)
        '    End Using

        '    ms.Position = 0
        '    Dim outStream As New MemoryStream()

        '    Dim compressed As Byte() = New Byte(CInt(ms.Length) - 1) {}
        '    ms.Read(compressed, 0, compressed.Length)

        '    Dim gzBuffer As Byte() = New Byte(compressed.Length + 3) {}
        '    System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length)
        '    System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4)
        '    Return Convert.ToBase64String(gzBuffer)
        'End Function

        'Public Shared Function Decompress(ByVal compressedText As String) As String
        '    Dim gzBuffer As Byte() = Convert.FromBase64String(compressedText)
        '    Using ms As New MemoryStream()
        '        Dim msgLength As Integer = BitConverter.ToInt32(gzBuffer, 0)
        '        ms.Write(gzBuffer, 4, gzBuffer.Length - 4)

        '        Dim buffer As Byte() = New Byte(msgLength - 1) {}

        '        ms.Position = 0
        '        Using zip As New GZipStream(ms, CompressionMode.Decompress)
        '            zip.Read(buffer, 0, buffer.Length)
        '        End Using

        '        Return Encoding.UTF8.GetString(buffer)
        '    End Using
        'End Function

        'From http://www.snippetware.com/2009/10/11/compressdecompress-byte-array/

        Public Shared Function Compress(ByVal data As Byte()) As Byte()
            Dim ms As New MemoryStream()
            'Dim ds As New GZipStream(ms, CompressionMode.Compress)
            Dim ds As New DeflateStream(ms, CompressionMode.Compress)
            ds.Write(data, 0, data.Length)
            ds.Flush()
            ds.Close()
            Dim dt() As Byte = ms.ToArray
            ms.Flush()
            ms.Close()
            Return dt
        End Function

        Public Shared Function Decompress(ByVal data As Byte()) As Byte()
            Const BUFFER_SIZE As Integer = 256
            Dim tempArray As Byte() = New Byte(BUFFER_SIZE - 1) {}
            Dim tempList As New List(Of Byte())()
            Dim count As Integer = 0, length As Integer = 0

            Dim ms As New MemoryStream(data)
            'Dim ds As New GZipStream(ms, CompressionMode.Decompress)
            Dim ds As New DeflateStream(ms, CompressionMode.Decompress)

            count = ds.Read(tempArray, 0, BUFFER_SIZE)
            While count > 0
                If count = BUFFER_SIZE Then
                    tempList.Add(tempArray)
                    tempArray = New Byte(BUFFER_SIZE - 1) {}
                Else
                    Dim temp As Byte() = New Byte(count - 1) {}
                    Array.Copy(tempArray, 0, temp, 0, count)
                    tempList.Add(temp)
                End If
                length += count
                count = ds.Read(tempArray, 0, BUFFER_SIZE)
            End While

            Dim retVal As Byte() = New Byte(length - 1) {}

            count = 0
            For Each temp As Byte() In tempList
                Array.Copy(temp, 0, retVal, count, temp.Length)
                count += temp.Length
            Next

            ds.Flush()
            ds.Close()
            ms.Flush()
            ms.Close()

            Return retVal
        End Function

    End Class

End Namespace