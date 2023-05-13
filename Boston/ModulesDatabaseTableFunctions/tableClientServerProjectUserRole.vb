Imports System.Reflection

Public Module tableClientServerProjectUserRole

    Public Sub AddUserRoleToProject(ByRef arUser As ClientServer.User,
                                    ByRef arRole As ClientServer.Role,
                                    ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerProjectUserRole"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arProject.Id & "'"
            lsSQLQuery &= "  ,'" & arUser.Id & "'"
            lsSQLQuery &= "  ,'" & arRole.Id & "'"
            lsSQLQuery &= " )"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function GetRolesForUserOnProject(ByRef arUser As ClientServer.User,
                                             ByRef arProject As ClientServer.Project,
                                             Optional ByVal abGetFunctions As Boolean = False) As List(Of ClientServer.Role)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larRole As New List(Of ClientServer.Role)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM ClientServerProjectUserRole"
            lsSQLQuery &= " WHERE ProjectId = '" & Trim(arProject.Id) & "'"
            lsSQLQuery &= "   AND UserId = '" & Trim(arUser.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrRole As ClientServer.Role

            While Not lREcordset.EOF
                lrRole = New ClientServer.Role
                lrRole.Id = lREcordset("RoleId").Value
                Call tableClientServerRole.getRoleDetailsById(lrRole.Id, lrRole)

                If abGetFunctions Then
                    lrRole.FunctionT = tableClientServerRoleFunction.getFunctionsForRole(lrRole)
                End If

                larRole.Add(lrRole)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larRole

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larRole
        End Try

    End Function

    Public Sub removeAllUserRolesFromProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerProjectUserRole"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arUser.Id & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub removeUserRoleFromProject(ByRef arUser As ClientServer.User,
                                         ByRef arRole As ClientServer.Role,
                                         ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerProjectUserRole"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "   AND RoleId = '" & arRole.Id & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Module
