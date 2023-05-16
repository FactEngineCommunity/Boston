Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class BinarySerialiser
    Private Shared formatter As New BinaryFormatter()

    Public Shared Sub SerializeObject(ByVal filePath As String, ByVal obj As Object)
        Using fileStream As New FileStream(filePath, FileMode.Create)
            formatter.Serialize(fileStream, obj)
        End Using
    End Sub

    Public Shared Function DeserializeObject(ByVal filePath As String) As Object
        Using fileStream As New FileStream(filePath, FileMode.Open)
            Return formatter.Deserialize(fileStream)
        End Using
    End Function

End Class