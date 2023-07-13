Imports System.Reflection

Public Module tableClientServerProject

    Public Sub addProject(ByRef arProject As ClientServer.Project)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerProject"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arProject.Id & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arProject.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arProject.CreatedByUserId, "'", "`")) & "'"
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

    Public Function getProjectCount() As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerProject"

            lREcordset.Open(lsSQLQuery)

            getProjectCount = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Function GetProjectCountByCreatedByUser(ByVal arUser As ClientServer.User) As Integer

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerProject"
            lsSQLQuery &= " WHERE CreatedByUserId = '" & Trim(arUser.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            GetProjectCountByCreatedByUser = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return 0
        End Try

    End Function

    Public Function getProjectDetailsById(ByVal asProjectId As String,
                                          ByRef arProject As ClientServer.Project,
                                          Optional abIgnoreErrors As Boolean = False) As ClientServer.Project

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerProject"
            lsSQLQuery &= " WHERE Id = '" & Trim(asProjectId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                If arProject Is Nothing Then
                    arProject = New ClientServer.Project
                End If

                arProject.Id = lREcordset("Id").Value
                arProject.Name = lREcordset("Projectname").Value
                arProject.CreatedByUser = New ClientServer.User
                Dim lsCreatedByUserId As String = lREcordset("CreatedByUserId").Value
                Call tableClientServerUser.getUserDetailsById(lsCreatedByUserId, arProject.CreatedByUser)
            Else
                If Not abIgnoreErrors Then
                    Dim lsMessage As String = "Error: getProjectDetailsById: No Project returned for Id: " & asProjectId
                    Throw New Exception(lsMessage)
                End If
            End If

            lREcordset.Close()

            Return arProject

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function GetProjects(ByRef aarProject As List(Of ClientServer.Project),
                                ByRef arUser As ClientServer.User) As List(Of ClientServer.Project)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim lrProject As ClientServer.Project

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProject"
            If arUser IsNot Nothing Then
                lsSQLQuery &= " WHERE Id IN (SELECT ProjectId FROM ClientServerProjectUser WHERE UserId = '" & arUser.Id & "')"
            End If

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then

                If aarProject Is Nothing Then
                    aarProject = New List(Of ClientServer.Project)
                End If

                While Not lREcordset.EOF
                    lrProject = New ClientServer.Project
                    lrProject.Id = lREcordset("Id").Value
                    lrProject.Name = lREcordset("ProjectName").Value
                    lrProject.CreatedByUserId = lREcordset("CreatedByUserId").Value

                    aarProject.Add(lrProject)
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return aarProject

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return New List(Of ClientServer.Project)
        End Try

    End Function

    Public Function IsProjectAllocatedToUser(ByRef arProject As ClientServer.Project, ByRef arUser As ClientServer.User) As Boolean

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerProjectUser"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"
            lsSQLQuery &= "   AND UserId = '" & arUser.Id & "'"

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

    Public Sub UpdateProject(ByRef arProject As ClientServer.Project)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE ClientServerProject"
            lsSQLQuery &= "   SET ProjectName = '" & Trim(Replace(arProject.Name, "'", "`")) & "'"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arProject.Id, "'", "`")) & "'"

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
