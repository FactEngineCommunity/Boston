Imports System.Reflection

Public Class frmToolboxDatabaseSchemaViewer

    Public WithEvents mrModel As FBM.Model

    Private Sub frmToolboxDatabaseSchemaViewer_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function EqualsByName(ByVal other As Form) As Boolean
        Return Me.Name = other.Name
    End Function

    Public Sub SetupForm()

        Try
            Call Me.LoadTableNames()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub LoadTableNames()

        Try
            Me.TreeViewSchema.Nodes.Add("Tables")

            Dim larTable As List(Of RDS.Table)

            If Me.mrModel.DatabaseConnection Is Nothing Then
                If Not Me.mrModel.connectToDatabase() Then
                    prApplication.ThrowErrorMessage("Couldn't connect to the database.", pcenumErrorType.Warning, abThrowtoMSGBox:=True, abUseFlashCard:=True)
                End If
            End If

            larTable = Me.mrModel.DatabaseConnection.getTables

            For Each lrTable In larTable
                Dim lrNode = Me.TreeViewSchema.Nodes(0).Nodes.Add(lrTable.Name)

                For Each lrColumn In Me.mrModel.DatabaseConnection.getColumnsByTable(lrTable)
                    lrNode.Nodes.Add(lrColumn.Name & "    " & lrColumn.DataType.DataType & "     " & lrColumn.TemporaryData)
                Next

            Next

            Dim lasRelationLabel = Me.mrModel.DatabaseConnection.getRelationLabels

            For Each lsRelationLabel In lasRelationLabel
                Dim lrNode = Me.TreeViewSchema.Nodes(0).Nodes.Add(lsRelationLabel)
            Next

            Me.TreeViewSchema.Nodes(0).Expand()


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        Try
            Me.TreeViewSchema.Nodes.Clear()
            Call Me.LoadTableNames()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub frmToolboxDatabaseSchemaViewer_Closed(sender As Object, e As EventArgs) Handles Me.Closed

        Try
            prApplication.RightToolboxForms.Remove(Me)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class