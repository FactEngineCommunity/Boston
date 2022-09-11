Imports System.Reflection

Module tableClientServerProjectGroupPermission

    Public Sub AddProjectGroupPermission(ByRef arProjectGroupPermission As ClientServer.ProjectGroupPermission)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerProjectGroupPermission"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arProjectGroupPermission.Project.Id & "'"
            lsSQLQuery &= " ,'" & arProjectGroupPermission.Group.Id & "'"
            lsSQLQuery &= " ,'" & arProjectGroupPermission.Permission.ToString & "'"
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

    Public Sub DeleteProjectGroupPermission(ByRef arProjectGroupPermission As ClientServer.ProjectGroupPermission)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "DELETE FROM ClientServerProjectGroupPermission"
            lsSQLQuery &= " WHERE ProjectId = '" & arProjectGroupPermission.Project.Id & "'"
            lsSQLQuery &= "   AND GroupId = '" & arProjectGroupPermission.Group.Id & "'"
            lsSQLQuery &= "   AND Permission = '" & arProjectGroupPermission.Permission.ToString & "'"

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

    Public Sub GetPermissionsForProjectGroup(ByRef arProject As ClientServer.Project,
                                                      ByRef arGroup As ClientServer.Group,
                                                      ByRef aarProjectGroupPermission As List(Of ClientServer.ProjectGroupPermission))

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrProjectGroupPermission As ClientServer.ProjectGroupPermission

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProjectGroupPermission"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND GroupId = '" & arGroup.Id & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrProjectGroupPermission = New ClientServer.ProjectGroupPermission
                    lrProjectGroupPermission.Project = arProject
                    lrProjectGroupPermission.Group = arGroup
                    lrProjectGroupPermission.Permission =
                        CType([Enum].Parse(GetType(pcenumPermission), lREcordset("Permission").Value), pcenumPermission)

                    aarProjectGroupPermission.Add(lrProjectGroupPermission)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub




End Module
