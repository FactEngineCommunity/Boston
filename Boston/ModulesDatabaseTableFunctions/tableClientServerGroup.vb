Imports System.Reflection

Module tableClientServerGroup

    Public Sub addGroup(ByRef arGroup As ClientServer.Group)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerGroup"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arGroup.Id & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arGroup.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arGroup.CreatedByUser.Id, "'", "`")) & "'"
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

    Public Sub getGroupDetailsById(ByVal asGroupId As String, ByRef arGroup As ClientServer.Group)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id = '" & Trim(asGroupId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arGroup.Id = lREcordset("Id").Value
                arGroup.Name = Trim(lREcordset("GroupName").Value)
                arGroup.CreatedByUser = New ClientServer.User
                arGroup.CreatedByUser.Id = Trim(lREcordset("CreatedByUserId").Value)
                Call tableClientServerUser.getUserDetailsById(arGroup.CreatedByUser.Id, arGroup.CreatedByUser)
            Else
                Dim lsMessage As String = "Error: getGroupDetailsById: No Group returned for Id: " & asGroupId
                Throw New Exception(lsMessage)
            End If

            lREcordset.Close()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub updateGroup(ByRef arGroup As ClientServer.Group)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE ClientServerGroup"
            lsSQLQuery &= "   SET GroupName = '" & Trim(Replace(arGroup.Name, "'", "`")) & "'"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arGroup.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
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
