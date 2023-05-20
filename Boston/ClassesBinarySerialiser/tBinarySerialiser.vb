Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Reflection

Public Class BinarySerialiser
    Private Shared formatter As New BinaryFormatter()


    ''' <summary>
    ''' Standard Binary Serializer in .Net
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <param name="obj"></param>
    Public Shared Sub SerializeObject(ByVal filePath As String, ByVal obj As Object)

        Using fileStream As New FileStream(filePath, FileMode.Create)
            formatter.Serialize(fileStream, obj) 'Standard Binary Serialiser                        
        End Using

    End Sub

    ''' <summary>
    ''' Standard binary Serializer
    ''' </summary>
    ''' <param name="filePath"></param>
    ''' <returns></returns>
    Public Shared Function DeserializeObject(ByVal filePath As String) As Object
        Using fileStream As New FileStream(filePath, FileMode.Open)
            Return formatter.Deserialize(fileStream) 'Standard Binary Serialiser            
        End Using
    End Function

    ''' <summary>
    ''' Using Newtonsoft.JSON
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="fileName"></param>
    ''' <param name="obj"></param>
    Public Shared Sub Serialize(Of T)(fileName As String, obj As T)


        Dim loJSONSerialiserSettings As New Newtonsoft.Json.JsonSerializerSettings() With {
                                                                    .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                                    .ReferenceLoopHandling = ReferenceLoopHandling.Serialize, 'Ignore,
                                                                    .TypeNameHandling = TypeNameHandling.All,
                                                                    .NullValueHandling = NullValueHandling.Ignore,
                                                                    .Error = Sub(sender As Object, e As Newtonsoft.Json.Serialization.ErrorEventArgs)
                                                                                 ' Handle serialization error if needed
                                                                                 If e.ErrorContext.Error.InnerException.Message = "The method or operation is not implemented." Then

                                                                                     Console.WriteLine("Serialization Error: NotImplementedException occurred.")
                                                                                     e.ErrorContext.Handled = True

                                                                                 ElseIf e.ErrorContext.Error.InnerException.Message = "Object reference not set to an instance of an object." Then

                                                                                     Console.WriteLine("Serialization Error: Object reference Not set to an instance of an object.")
                                                                                     e.ErrorContext.Handled = True
                                                                                 Else
                                                                                     ' Handle other exceptions
                                                                                     Console.WriteLine($"Serialization Error: {e.ErrorContext.Error.Message}")
                                                                                 End If

                                                                                 ' Set the error context handled to suppress the default exception throwing behavior

                                                                             End Sub
                                                                    }
        Try
            'Original, Works
            'Dim lsJSON As String = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, loJSONSerialiserSettings)

            'Using loStreamWriter As New StreamWriter(fileName)
            '    loStreamWriter.Write(lsJSON)
            'End Using
            '========================================          

            Using fileStream As FileStream = File.Create(fileName)
                Using sw As New StreamWriter(fileStream)
                    Using writer As New JsonTextWriter(sw)
                        Dim serializer As JsonSerializer = JsonSerializer.Create(loJSONSerialiserSettings)

                        ' Serialize the object to JSON
                        serializer.Serialize(writer, obj)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Using Newtonsoft.JSON
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="fileName"></param>
    ''' <returns></returns>
    Public Shared Function Deserialize(Of T)(fileName As String) As T

        Try
            Dim loJSONSerialiserSettings As New Newtonsoft.Json.JsonSerializerSettings() With {
                                                            .PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                            .TypeNameHandling = TypeNameHandling.Auto,
                                                            .NullValueHandling = NullValueHandling.Ignore,
                                                            .MaxDepth = 128,
                                                            .Error = Sub(sender As Object, e As Newtonsoft.Json.Serialization.ErrorEventArgs)
                                                                         ' Handle serialization error if needed
                                                                         If e.ErrorContext.Error.InnerException.Message = "The method or operation is not implemented." Then
                                                                             ' Handle the NotImplementedException
                                                                             Console.WriteLine("Serialization Error: NotImplementedException occurred.")
                                                                             ' Additional error handling or logging can be performed here
                                                                             e.ErrorContext.Handled = True
                                                                         Else
                                                                             ' Handle other exceptions
                                                                             Console.WriteLine($"Serialization Error: {e.ErrorContext.Error.Message}")
                                                                         End If

                                                                         ' Set the error context handled to suppress the default exception throwing behavior

                                                                     End Sub
                                                            }
            'Original Works
            'Dim lsJSON As String = System.IO.File.ReadAllText(fileName)
            'Dim lrModel As T = JsonConvert.DeserializeObject(Of T)(lsJSON, loJSONSerialiserSettings)

            Dim lrModel As T
            Using sr As New StreamReader(fileName)
                Using reader As New JsonTextReader(sr)

                    Dim serializer As JsonSerializer = JsonSerializer.Create(loJSONSerialiserSettings)

                    ' Deserialize the JSON from the file
                    lrModel = serializer.Deserialize(Of T)(reader)
                End Using
            End Using

            Return lrModel

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

End Class

