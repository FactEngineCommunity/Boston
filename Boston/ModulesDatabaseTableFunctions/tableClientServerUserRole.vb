Imports System.Reflection

Public Module tableClientServerUserRole

    Public Sub addRoleToUser(ByRef arRole As ClientServer.Role, ByRef arUser As ClientServer.User)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerUserRole"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arUser.Id & "'"
            lsSQLQuery &= "  ,'" & arRole.Id & "'"
            lsSQLQuery &= " )"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Function getRolesForUser(ByRef arUser As ClientServer.User, Optional abGetFunctionsForRole As Boolean = False) As List(Of ClientServer.Role)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrRole As ClientServer.Role
        Dim larRole As New List(Of ClientServer.Role)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerRole"
            lsSQLQuery &= " WHERE Id IN (SELECT RoleId FROM ClientServerUserRole WHERE UserId = '" & arUser.Id & "')"


            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrRole = New ClientServer.Role
                    lrRole.Id = lREcordset("Id").Value
                    lrRole.Name = lREcordset("RoleName").Value

                    If abGetFunctionsForRole Then
                        lrRole.FunctionT = tableClientServerRoleFunction.getFunctionsForRole(lrRole)
                    End If

                    larRole.Add(lrRole)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larRole

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return larRole
        End Try

    End Function

    Public Sub removeRoleFromUser(ByRef arRole As ClientServer.Role, ByRef arUser As ClientServer.User)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerUserRole"
            lsSQLQuery &= " WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "   AND RoleId = '" & arRole.Id & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Module
