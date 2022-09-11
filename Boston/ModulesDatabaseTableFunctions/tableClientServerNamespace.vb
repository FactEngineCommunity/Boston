Imports System.Reflection

Public Module tableClientServerNamespace
    Public Sub addNamespace(ByRef arNamespace As ClientServer.Namespace)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO ClientServerNamespace"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arNamespace.Id & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arNamespace.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arNamespace.Number, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arNamespace.Project.Id, "'", "`")) & "'"
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

    Public Function getNamespaceCount() As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ClientServerNamespace"

            lREcordset.Open(lsSQLQuery)

            getNamespaceCount = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Sub getNamespaceDetailsById(ByVal asNamespaceId As String, ByRef arNamespace As ClientServer.Namespace)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerNamespace"
            lsSQLQuery &= " WHERE Id = '" & Trim(asNamespaceId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                arNamespace.Id = lREcordset("Id").Value
                arNamespace.Name = lREcordset("Namespace").Value
                arNamespace.Number = lREcordset("Number").Value
                Dim lrProject As New ClientServer.Project
                Call tableClientServerProject.getProjectDetailsById(lREcordset("ProjectId").Value, lrProject)
                arNamespace.Project = lrProject
            Else
                Dim lsMessage As String = "Error: getNamespaceDetailsById: No Namespace returned for Id: " & asNamespaceId
                Throw New Exception(lsMessage)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function getNamespacesForProject(ByRef arProject As ClientServer.Project) As List(Of ClientServer.Namespace)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrNamespace As ClientServer.Namespace
        Dim larNamespace As New List(Of ClientServer.Namespace)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM ClientServerNamespace"
            lsSQLQuery &= " WHERE ProjectId = '" & Trim(arProject.Id) & "'"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF
                lrNamespace = New ClientServer.Namespace
                lrNamespace.Id = lREcordset("Id").Value
                lrNamespace.Name = lREcordset("Namespace").Value
                lrNamespace.Number = lREcordset("Number").Value
                lrNamespace.Project = arProject

                larNamespace.Add(lrNamespace)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larNamespace
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub updateNamespace(ByRef arNamespace As ClientServer.Namespace)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE ClientServerNamespace"
            lsSQLQuery &= "   SET Namespace = '" & Trim(Replace(arNamespace.Name, "'", "`")) & "'"
            lsSQLQuery &= "       , [Number] = '" & Trim(arNamespace.Number) & "'"
            lsSQLQuery &= "       , ProjectId = '" & arNamespace.Project.Id & "'"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arNamespace.Id, "'", "`")) & "'"

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
