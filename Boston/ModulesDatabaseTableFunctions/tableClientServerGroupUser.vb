Imports System.Reflection

Public Module tableClientServerGroupUser

    Public Sub AddUserToGroup(ByRef arUser As ClientServer.User, ByRef arGroup As ClientServer.Group)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerGroupUser"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arGroup.Id & "'"
            lsSQLQuery &= "  ,'" & arUser.Id & "'"
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

    Public Function GetGroupsCreatedByUser(ByRef arUser As ClientServer.User) As List(Of ClientServer.Group)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larGroup As New List(Of ClientServer.Group)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerGroup"
            lsSQLQuery &= " WHERE CreatedByUserId = '" & Trim(arUser.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrGroup As ClientServer.Group

            While Not lREcordset.EOF
                lrGroup = New ClientServer.Group
                lrGroup.Id = lREcordset("Id").Value
                Call tableClientServerGroup.getGroupDetailsById(lrGroup.Id, lrGroup)
                larGroup.Add(lrGroup)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larGroup

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larGroup
        End Try

    End Function

    Public Function GetGroupsForUser(ByRef arUser As ClientServer.User) As List(Of ClientServer.Group)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larGroup As New List(Of ClientServer.Group)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerGroupUser"
            lsSQLQuery &= " WHERE UserId = '" & Trim(arUser.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrGroup As ClientServer.Group

            While Not lREcordset.EOF
                lrGroup = New ClientServer.Group
                lrGroup.Id = lREcordset("GroupId").Value
                Call tableClientServerGroup.getGroupDetailsById(lrGroup.Id, lrGroup)
                larGroup.Add(lrGroup)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larGroup

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larGroup
        End Try

    End Function

    Public Function GetUsersForGroup(ByRef arGroup As ClientServer.Group) As List(Of ClientServer.User)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larUser As New List(Of ClientServer.User)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerGroupUser"
            lsSQLQuery &= " WHERE GroupId = '" & Trim(arGroup.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            Dim lrUser As ClientServer.User

            While Not lREcordset.EOF
                lrUser = New ClientServer.User
                lrUser.Id = lREcordset("UserId").Value
                Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser)
                larUser.Add(lrUser)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larUser

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larUser
        End Try

    End Function

    ''' <summary>
    ''' PRECONDITION: Group is known to be included on the Project
    ''' </summary>
    ''' <param name="arGroup"></param>
    ''' <param name="arProject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUsersForGroupOnProject(ByRef arGroup As ClientServer.Group, ByRef arProject As ClientServer.Project) As List(Of ClientServer.User)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim larUser As New List(Of ClientServer.User)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM ClientServerUser"
            lsSQLQuery &= " WHERE Id IN (SELECT UserId"
            lsSQLQuery &= "                FROM ClientServerGroupUser"
            lsSQLQuery &= "               WHERE GroupId = '" & Trim(arGroup.Id) & "'"
            lsSQLQuery &= "              )"
            lsSQLQuery &= "   AND Id IN (SELECT UserId"
            lsSQLQuery &= "                FROM ClientServerProjectUser"
            lsSQLQuery &= "               WHERE ProjectId = '" & Trim(arProject.Id) & "'"
            lsSQLQuery &= "              )"

            lREcordset.Open(lsSQLQuery)

            Dim lrUser As ClientServer.User

            While Not lREcordset.EOF
                lrUser = New ClientServer.User
                lrUser.Id = lREcordset("Id").Value
                Call tableClientServerUser.getUserDetailsById(lrUser.Id, lrUser, False)
                larUser.Add(lrUser)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larUser

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larUser
        End Try

    End Function

    Public Sub removeUserFromGroup(ByRef arUser As ClientServer.User, ByRef arGroup As ClientServer.Group)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerGroupUser"
            lsSQLQuery &= " WHERE GroupId = '" & arGroup.Id & "'"
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

End Module
