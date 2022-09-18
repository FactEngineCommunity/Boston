Imports System.Reflection

Public Class frmCRUDAddProject

    Private zrProject As New ClientServer.Project


    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click


        If Me.checkFields() Then
            If Me.getFields(Me.zrProject) Then

                Call tableClientServerProject.addProject(Me.zrProject)
                Call tableClientServerProjectUser.AddUserToProject(prApplication.User, Me.zrProject)
                Dim lrProjectUserPermission As New ClientServer.ProjectUserPermission()
                lrProjectUserPermission.User = prApplication.User
                lrProjectUserPermission.Project = Me.zrProject
                lrProjectUserPermission.Permission = pcenumPermission.FullRights
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                lrProjectUserPermission.Permission = pcenumPermission.Create
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                lrProjectUserPermission.Permission = pcenumPermission.Read
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)
                lrProjectUserPermission.Permission = pcenumPermission.Alter
                Call tableClientServerProjectUserPermission.AddProjectUserPermission(lrProjectUserPermission)

                Dim lrNamespace As New ClientServer.Namespace
                lrNamespace.Id = System.Guid.NewGuid.ToString
                lrNamespace.Name = "Default"
                lrNamespace.Number = ""
                lrNamespace.Project = Me.zrProject

                Call tableClientServerNamespace.addNamespace(lrNamespace)

                Dim lrRole As ClientServer.Role

                For Each lrRole In prApplication.User.Role
                    Call tableClientServerProjectUserRole.AddUserRoleToProject(prApplication.User, lrRole, Me.zrProject)
                Next

                Me.Hide()
                Me.Close()
                Me.Dispose()
            End If
        End If

    End Sub

    Private Function checkFields() As Boolean

        checkFields = True

        If Trim(Me.TextBoxProjectName.Text) = "" Then
            checkFields = False
        End If

    End Function

    ''' <summary>
    ''' PRECONDITION: A User is logged into Boston.
    ''' </summary>
    ''' <param name="arProject"></param>
    ''' <remarks></remarks>
    Private Function getFields(ByRef arProject As ClientServer.Project) As Boolean

        Try
            If prApplication.User Is Nothing Then
                Throw New Exception("You must log in before creating a Project.")
            End If

            arProject.Name = Trim(Me.TextBoxProjectName.Text)
            arProject.CreatedByUserId = prApplication.User.Id
            arProject.CreatedByUser = prApplication.User

            Return True

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub
End Class