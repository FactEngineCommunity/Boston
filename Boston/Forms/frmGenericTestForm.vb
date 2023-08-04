Imports System.Linq.Expressions
Imports System.Reflection

Public Class frmGenericTestForm
    Private Sub Get_Click(sender As Object, e As EventArgs) Handles Button1.Click


        Try
            ' Define the LINQ expression for the condition
            Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"

            Dim lrDataStore As New DataStore.Store

            Dim larDictionaryEntry As List(Of FBM.EntityType) = lrDataStore.Get(Of FBM.EntityType)(whereClause)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Try
            Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)

            Dim lrDataStore As New DataStore.Store
            Call lrDataStore.Add(lrEntityType)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Update_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Try
            Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
            Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"

            lrEntityType.Symbol = "Testaddb"

            Dim lrDataStore As New DataStore.Store
            Call lrDataStore.Update(Of FBM.EntityType)(lrEntityType, whereClause)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub Delete_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Try
            Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
            Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"

            Dim lrDataStore As New DataStore.Store
            Call lrDataStore.Delete(Of FBM.EntityType)(whereClause)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub
End Class