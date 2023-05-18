Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Newtonsoft.Json
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
                                                                    .ReferenceLoopHandling = ReferenceLoopHandling.Serialize, ' or ReferenceLoopHandling.Ignore
                                                                    .TypeNameHandling = TypeNameHandling.All,
                                                                    .NullValueHandling = NullValueHandling.Ignore,
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
        Try
            Dim lsJSON As String = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, loJSONSerialiserSettings)

            Using loStreamWriter As New StreamWriter(fileName)
                loStreamWriter.Write(lsJSON)
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

        Dim lsJSON As String = System.IO.File.ReadAllText(fileName)
        Dim lrModel As FBM.Model = JsonConvert.DeserializeObject(Of FBM.Model)(lsJSON, New JsonSerializerSettings With {.PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                                                                                                .TypeNameHandling = TypeNameHandling.Auto})
    End Function

End Class