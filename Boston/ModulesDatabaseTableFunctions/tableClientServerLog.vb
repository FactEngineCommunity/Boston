Imports System.Reflection

Public Module tableClientServerLog

    Public Sub AddLogEntry(ByRef arLogEntry As ClientServer.Log)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerLog"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= pdbConnection.DateTimeWrap(arLogEntry.DateTime.ToString("yyyy/MM/dd HH:mm:ss")) '" #" & arLogEntry.DateTime & "#"
            lsSQLQuery &= " ,'" & Trim(Replace(arLogEntry.User.Id, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(arLogEntry.LogType.ToString) & "'"
            lsSQLQuery &= " ,'" & Trim(arLogEntry.IPAddress) & "'"
            lsSQLQuery &= " ,'" & Trim(arLogEntry.BrowserId) & "'"
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Function GetLastLogonUsernamePasswordFromBrowserId(ByVal asBrowserId As String) As ClientServer.User

        Dim lrUser As New ClientServer.User

        Try

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM ClientServerLog"
            lsSQLQuery &= " WHERE BrowserId = '" & asBrowserId.Trim & "'"
            lsSQLQuery &= " ORDER BY DateTime DESC"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                Dim lsUserId = lREcordset("UserId").Value
                lREcordset.Close()
                getUserDetailsById(lsUserId, lrUser, True)
                Return lrUser
            Else
                lREcordset.Close()
                Return Nothing
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function GetLastLogonUsernamePasswordFromIPAddress(ByVal asIPAddress As String) As ClientServer.User

        Dim lrUser As New ClientServer.User

        Try

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM ClientServerLog"
            lsSQLQuery &= " WHERE IPAddress = '" & asIPAddress.Trim & "'"
            lsSQLQuery &= " ORDER BY DateTime DESC"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                Dim lsUserId = lREcordset("UserId").Value
                lREcordset.Close()
                getUserDetailsById(lsUserId, lrUser, True)
                Return lrUser
            Else
                lREcordset.Close()
                Return Nothing
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

End Module
