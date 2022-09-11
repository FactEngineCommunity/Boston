Imports System.Reflection

Public Module tableClientServerLog

    Public Sub AddLogEntry(ByRef arLogEntry As ClientServer.Log)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerLog"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " #" & arLogEntry.DateTime & "#"
            lsSQLQuery &= " ,'" & Trim(Replace(arLogEntry.User.Id, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(arLogEntry.LogType.ToString) & "'"
            lsSQLQuery &= " ,'" & Trim(arLogEntry.IPAddress) & "'"
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

End Module
