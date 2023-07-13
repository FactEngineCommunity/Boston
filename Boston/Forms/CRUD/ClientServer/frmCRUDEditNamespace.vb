Imports System.Reflection

Public Class frmCRUDEditNamespace

    Public zrNamespace As ClientServer.Namespace

    Private Sub frmCRUDAddNamespace_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call Me.setupForm()

    End Sub

    Private Sub setupForm()

        Call Me.loadProjects(Me.zrNamespace.Project)

        Me.TextBoxNamespaceName.Text = Me.zrNamespace.Name

    End Sub

    Private Sub loadProjects(ByRef arProject As ClientServer.Project)

        Dim lsMessage As String
        Dim lrProject As ClientServer.Project

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            '---------------------------------------------
            'First get EntityTypes with no ParentEntityId
            '---------------------------------------------
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM ClientServerProject"
            If Not prApplication.User.IsSuperuser Then
                lsSQLQuery &= " WHERE Id IN (SELECT ProjectId"
                lsSQLQuery &= "                FROM ClientServerProjectUser"
                lsSQLQuery &= "               WHERE UserId = '" & prApplication.User.Id & "'"
                lsSQLQuery &= "             )"
            End If

            lREcordset.Open(lsSQLQuery)

            Dim lrNamespaceProjectItem As tComboboxItem = Nothing

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrProject = New ClientServer.Project
                    lrProject.Id = lREcordset("Id").Value
                    lrProject.Name = lREcordset("ProjectName").Value

                    Dim lrComboboxItem As New tComboboxItem(lrProject.Id, lrProject.Name, lrProject)
                    Me.ComboBoxProject.Items.Add(lrComboboxItem)

                    If lrProject.Id = arProject.Id Then lrNamespaceProjectItem = lrComboboxItem

                    lREcordset.MoveNext()
                End While
            End If

            Me.ComboBoxProject.SelectedIndex = Me.ComboBoxProject.Items.IndexOf(lrNamespaceProjectItem)

            lREcordset.Close()

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function checkFields() As Boolean

        checkFields = True

        If Trim(Me.TextBoxNamespaceName.Text) = "" Then
            Return False
        End If

        If Me.ComboBoxProject.SelectedIndex = -1 Then
            Return False
        End If

    End Function

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Me.Hide()
        Me.Close()
        Me.Dispose()

    End Sub


    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        If Me.checkFields Then
            If Me.getFields(Me.zrNamespace) Then

                Call tableClientServerNamespace.updateNamespace(Me.zrNamespace)

                Me.Hide()
                Me.Close()
                Me.Dispose()
            End If
        End If

    End Sub

    Private Function getFields(ByRef arNamespace As ClientServer.Namespace)

        Try
            arNamespace.Name = Trim(Me.TextBoxNamespaceName.Text)
            arNamespace.Project = Me.ComboBoxProject.SelectedItem.Tag

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

End Class