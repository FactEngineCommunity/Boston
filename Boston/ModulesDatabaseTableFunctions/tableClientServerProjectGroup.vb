Imports System.Reflection

Public Module tableClientServerProjectGroup

    Public Sub AddGroupToProject(ByRef arGroup As ClientServer.Group, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerProjectGroup"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arProject.Id & "'"
            lsSQLQuery &= "  ,'" & arGroup.Id & "'"
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

    Public Function getGroupsForUserForProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrGroup As ClientServer.Group
        Dim larGroup As New List(Of ClientServer.Group)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerGroup"
            lsSQLQuery &= " WHERE Id IN (SELECT GroupId"
            lsSQLQuery &= "                FROM ClientServerGroupUser"
            lsSQLQuery &= "               WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "                 AND GroupId IN (SELECT GroupId "
            lsSQLQuery &= "                                   FROM ClientServerProjectGroup WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "                                )"
            lsSQLQuery &= ")"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrGroup = New ClientServer.Group
                    lrGroup.Id = lREcordset("Id").Value
                    lrGroup.Name = lREcordset("GroupName").Value

                    larGroup.Add(lrGroup)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larGroup

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larGroup
        End Try

    End Function

    Public Function getProjectsForGroup(ByRef arGroup As ClientServer.Group) As List(Of ClientServer.Project)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrProject As ClientServer.Project
        Dim larProject As New List(Of ClientServer.Project)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProject"
            lsSQLQuery &= " WHERE Id IN (SELECT ProjectId FROM ClientServerProjectGroup WHERE GroupId = '" & arGroup.Id & "')"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrProject = New ClientServer.Project
                    lrProject.Id = lREcordset("Id").Value
                    lrProject.Name = lREcordset("ProjectName").Value

                    larProject.Add(lrProject)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larProject

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larProject
        End Try

    End Function


    Public Sub removeGroupFromProject(ByRef arGroup As ClientServer.Group, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerProjectGroup"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND GroupId = '" & arGroup.Id & "'"

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
