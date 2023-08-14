Imports System.Reflection

Public Module tableClientServerProjectModelShare

    Public Sub AddModelToProject(ByRef arModel As FBM.Model, ByRef arProject As ClientServer.Project)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO ClientServerProjectModelShare"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arModel.ModelId & "'"
            lsSQLQuery &= "  ,'" & arProject.Id & "'"
            lsSQLQuery &= " )"

            Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)

            If lrRecordset.ErrorReturned Then
                Throw New Exception(lrRecordset.ErrorString)
            End If

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function getModelsForProjectForUser(ByRef arProject As ClientServer.Project, ByRef arUser As ClientServer.User) As List(Of FBM.Model)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim lrModel As FBM.Model
        Dim larModel As New List(Of FBM.Model)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProjectModelShare"
            lsSQLQuery &= " WHERE ProjectId IN (SELECT ProjectId"
            lsSQLQuery &= "                       FROM ClientServerProjectUser"
            lsSQLQuery &= "                      WHERE UserId = '" & arUser.Id & "'"
            lsSQLQuery &= ")"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrModel = New FBM.Model
                    lrModel.ModelId = lREcordset("ModelId").Value

                    If TableModel.GetModelDetails(lrModel) Then
                        larModel.Add(lrModel)
                    End If

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larModel

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larModel
        End Try

    End Function

    Public Function getModelsForProject(ByRef arProject As ClientServer.Project) As List(Of FBM.Model)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim lrModel As FBM.Model
        Dim larModel As New List(Of FBM.Model)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProjectModelShare"
            lsSQLQuery &= " WHERE ProjectId = '" & arProject.Id & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrModel = New FBM.Model
                    lrModel.ModelId = lREcordset("ModelId").Value

                    lrModel = prApplication.Models.Find(Function(x) x.ModelId = lrModel.ModelId)

                    If lrModel IsNot Nothing Then
                        larModel.Add(lrModel)
                    End If
                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larModel

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return larModel
        End Try

    End Function


    Public Sub removeModelFromProject(ByRef arProject As ClientServer.Project, ByRef arModel As FBM.Model)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM ClientServerProjectModelshare"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= "   AND ProjectId = '" & arProject.Id & "'"

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
