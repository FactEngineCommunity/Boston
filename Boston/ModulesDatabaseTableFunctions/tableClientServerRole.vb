Imports System.Reflection

Public Module tableClientServerRole

    Public Sub addRole(ByRef arRole As ClientServer.Role)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerRole"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arRole.Id & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arRole.Name, "'", "`")) & "'"
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Sub getRoleDetailsById(ByVal asRoleId As String, ByRef arRole As ClientServer.Role)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerRole"
            lsSQLQuery &= " WHERE Id = '" & Trim(asRoleId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arRole.Id = lREcordset("Id").Value
                arRole.Name = lREcordset("RoleName").Value
            Else
                Dim lsMessage As String = "Error: getRoleDetailsById: No Role returned for Id: " & asRoleId
                Throw New Exception(lsMessage)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub getRoleDetailsByName(ByVal asRoleName As String, ByRef arRole As ClientServer.Role)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerRole"
            lsSQLQuery &= " WHERE RoleName = '" & Trim(asRoleName) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arRole.Id = lREcordset("Id").Value
                arRole.Name = lREcordset("RoleName").Value
            Else
                Dim lsMessage As String = "Error: getRoleDetailsById: No Role returned for RoleName: " & asRoleName
                Throw New Exception(lsMessage)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub updateRole(ByRef arRole As ClientServer.Role)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = " UPDATE ClientServerRole"
            lsSQLQuery &= "   SET Rolename = '" & Trim(Replace(arRole.Name, "'", "`")) & "'"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arRole.Id, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception

            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

End Module
