Imports System.Reflection

Public Module tableClientServerProjectUser

    Public Sub AddUserToProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerProjectUser"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arProject.Id & "'"
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

    ''' <summary>
    ''' Returns the count of Projects in which the nominated User is included.
    ''' </summary>
    ''' <param name="arUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getProjectCountByAllocatedUser(ByRef arUser As ClientServer.User) As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerProjectUser"
            lsSQLQuery &= "  WHERE UserId = '" & arUser.Id & "'"

            lREcordset.Open(lsSQLQuery)

            Return lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Function isUserOnProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project) As Boolean

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerProjectUser"
            lsSQLQuery &= " WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= "   AND ProjectId = '" & arProject.Id & "'"

            lREcordset.Open(lsSQLQuery)

            Return lREcordset(0).Value > 0

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Sub removeUserFromProject(ByRef arUser As ClientServer.User, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerProjectUser"
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

End Module
