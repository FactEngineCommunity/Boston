Imports System.Reflection

Module tableClientServerProjectUserPermission

    Public Sub AddProjectUserPermission(ByRef arProjectUserPermission As ClientServer.ProjectUserPermission)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerProjectUserPermission"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arProjectUserPermission.Project.Id & "'"
            lsSQLQuery &= " ,'" & arProjectUserPermission.User.Id & "'"
            lsSQLQuery &= " ,'" & arProjectUserPermission.Permission.ToString & "'"
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

    Public Sub DeleteProjectUserPermission(ByRef arProjectUserPermission As ClientServer.ProjectUserPermission)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "DELETE FROM ClientServerProjectUserPermission"
            lsSQLQuery &= " WHERE ProjectId = '" & arProjectUserPermission.Project.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arProjectUserPermission.User.Id & "'"
            lsSQLQuery &= "   AND Permission = '" & arProjectUserPermission.Permission.ToString & "'"

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

    Public Sub GetPermissionsForProjectUser(ByRef arProject As ClientServer.Project,
                                                      ByRef arUser As ClientServer.User,
                                                      ByRef aarProjectUserPermission As List(Of ClientServer.ProjectUserPermission))

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrProjectUserPermission As ClientServer.ProjectUserPermission

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProjectUserPermission"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arUser.Id & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrProjectUserPermission = New ClientServer.ProjectUserPermission
                    lrProjectUserPermission.Project = arProject
                    lrProjectUserPermission.User = arUser
                    lrProjectUserPermission.Permission = _
                        CType([Enum].Parse(GetType(pcenumPermission), lREcordset("Permission").Value), pcenumPermission)

                    aarProjectUserPermission.Add(lrProjectUserPermission)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub removeAllPermissionsForUserForProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "DELETE FROM ClientServerProjectUserPermission"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arUser.Id & "'"            

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

End Module
