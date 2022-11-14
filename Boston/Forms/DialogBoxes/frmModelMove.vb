Imports System.Reflection
Public Class frmModelMove

    Public mrModel As FBM.Model
    Public mrProject As ClientServer.Project
    Public mrNamespace As ClientServer.Namespace

    Private Sub frmModelMove_Load(sender As Object, e As EventArgs) Handles Me.Load

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

    Private Sub SetupForm()

        Try
            Me.LabelModelName.Text = Me.mrModel.Name

#Region "Current location"
            Dim lrProject As ClientServer.Project = Nothing
            lrProject = tableClientServerProject.getProjectDetailsById(Me.mrModel.ProjectId, lrProject, True)
            If lrProject IsNot Nothing Then
                Me.LabelCurrentProject.Text = lrProject.Name
            Else
                Me.LabelCurrentProject.Text = "MyPersonalModels"
            End If

            If Me.mrModel.Namespace IsNot Nothing Then
                Me.LabelCurrentNamespce.Text = Me.mrModel.Namespace.Name
            Else
                Me.LabelCurrentNamespce.Text = "Default"
            End If
#End Region

#Region "Load Projects and Namespaces for User"
            Call Me.loadProjects()
            Me.ComboBoxProject.SelectedIndex = 0
            Call Me.loadNamespacesForProject(Me.ComboBoxProject.SelectedItem.Tag)
#End Region

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub loadNamespacesForProject(ByRef arProject As ClientServer.Project)

        Try
            Dim lrNamespace As ClientServer.Namespace
            Dim larNamespace As New List(Of ClientServer.Namespace)

            larNamespace = tableClientServerNamespace.getNamespacesForProject(arProject)

            Dim lrComboboxItem As tComboboxItem

            Me.ComboBoxNamespace.Items.Clear()

            If arProject.Id = "MyPersonalModels" Then
                lrNamespace = New ClientServer.Namespace(0, "N/A", arProject)
                lrComboboxItem = New tComboboxItem(lrNamespace.Id, lrNamespace.Name, lrNamespace)
                Me.ComboBoxNamespace.Items.Add(lrComboboxItem)
            Else
                For Each lrNamespace In larNamespace
                    lrComboboxItem = New tComboboxItem(lrNamespace.Id, lrNamespace.Name, lrNamespace)
                    Me.ComboBoxNamespace.Items.Add(lrComboboxItem)
                Next
            End If

            If Me.ComboBoxNamespace.Items.Count > 0 Then
                Me.ComboBoxNamespace.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub loadProjects()

        Try
            For Each lrProject In tableClientServerProject.GetProjects(New List(Of ClientServer.Project), prApplication.User)

                Dim lrComoboxItem As New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
                Me.ComboBoxProject.Items.Add(lrComoboxItem)

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click

        Try
            Me.Hide()
            Me.Close()
            Me.Dispose()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonMoveModel_Click(sender As Object, e As EventArgs) Handles ButtonMoveModel.Click

        Try

            If MsgBox("Are you sure that you want to move the Model to the selected Project/Namespace?", MsgBoxStyle.YesNoCancel) = MsgBoxResult.Yes Then

                Me.mrProject = Me.ComboBoxProject.SelectedItem.Tag
                Me.mrNamespace = Me.ComboBoxNamespace.SelectedItem.Tag
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class