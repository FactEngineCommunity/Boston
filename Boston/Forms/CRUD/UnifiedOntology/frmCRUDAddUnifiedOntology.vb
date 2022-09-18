Imports System.Reflection
Imports System.IO
Imports System.ComponentModel

Public Class frmCRUDAddUnifiedOntology

    Private Sub frmCRUDAddUnifiedOntology_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Call Me.SetupForm

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
            Me.TextBoxUnifiedOntologyName.Text = Boston.CreateUniqueDatabaseIdentifier("UnifiedOntology", "UnifiedOntologyName", "Unified Ontology", False)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub


    Private Sub ButtonFileSelect_Click(sender As Object, e As EventArgs) Handles ButtonFileSelect.Click

        Try
            RemoveHandler Me.TextBoxImageFileLocation.Validating, AddressOf Me.TextBoxImageFileLocation_Validating
            Using lrOpenFileDialog As New OpenFileDialog

                If lrOpenFileDialog.ShowDialog = DialogResult.OK Then

                    If File.Exists(lrOpenFileDialog.FileName) Then
                        Me.TextBoxImageFileLocation.Text = lrOpenFileDialog.FileName
                    Else
                        MsgBox("No file exists at the given location/name.")
                    End If
                End If

            End Using
            AddHandler Me.TextBoxImageFileLocation.Validating, AddressOf Me.TextBoxImageFileLocation_Validating

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function CheckFields() As Boolean

        Try
            If Trim(Me.TextBoxUnifiedOntologyName.Text) = "" Then
                Me.ErrorProvider.SetError(Me.TextBoxUnifiedOntologyName, "Enter a valid Unified Ontology Name.")
                Return False
            End If

            If Trim(Me.TextBoxImageFileLocation.Text) <> "" Then
                If Not File.Exists(Trim(Me.TextBoxImageFileLocation.Text)) Then
                    Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "Enter a valid file location/name.")
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        Try
            If Me.CheckFields Then

                Dim lsUnifiedOntologyId As String = Trim(Me.TextBoxUnifiedOntologyName.Text)
                lsUnifiedOntologyId = Boston.CreateUniqueDatabaseIdentifier("UnifiedOntology", "Id", lsUnifiedOntologyId, False)

                Dim lsUnifiedOntologyName As String = Database.MakeStringSafe(Trim(Me.TextBoxUnifiedOntologyName.Text))


                Dim lrUnifiedOntology = New Ontology.UnifiedOntology(lsUnifiedOntologyId, lsUnifiedOntologyName)
                lrUnifiedOntology.ImageFileLocationName = Trim(Me.TextBoxImageFileLocation.Text)

                Call TableUnifiedOntology.AddUnifiedOntology(lrUnifiedOntology)
                Me.Close()
                Me.Dispose()

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxUnifiedOntologyName_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxUnifiedOntologyName.Validating

        Try
            If Trim(Me.TextBoxUnifiedOntologyName.Text) = "" Then
                Me.ErrorProvider.SetError(Me.TextBoxUnifiedOntologyName, "Enter a valid Unified Ontology Name")
                e.Cancel = True
            Else
                Me.ErrorProvider.SetError(Me.TextBoxUnifiedOntologyName, "")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxUnifiedOntologyName_Validated(sender As Object, e As EventArgs) Handles TextBoxUnifiedOntologyName.Validated

        Try
            Me.ErrorProvider.SetError(Me.TextBoxUnifiedOntologyName, "")
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxImageFileLocation_Validated(sender As Object, e As EventArgs) Handles TextBoxImageFileLocation.Validated

        Try
            Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "")
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxImageFileLocation_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxImageFileLocation.Validating

        Try

            If Trim(Me.TextBoxImageFileLocation.Text) <> "" Then
                If Not File.Exists(Trim(Me.TextBoxImageFileLocation.Text)) Then
                    Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "Enter a valid image location/file name.")
                    e.Cancel = True
                Else
                    Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "")
                End If
            End If

            Dim ImageExtensions As New List(Of String) From {".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG"}

            If Not ImageExtensions.Contains(Path.GetExtension(Trim(Me.TextBoxImageFileLocation.Text)).ToUpperInvariant()) Then
                Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "Enter a valid image file location/name.")
                e.Cancel = True
            Else
                Me.ErrorProvider.SetError(Me.TextBoxImageFileLocation, "")
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click

        Try
            Me.Hide()
            Me.Close()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBoxImageFileLocation_Leave(sender As Object, e As EventArgs) Handles TextBoxImageFileLocation.Leave

        Try
            If Me.ActiveControl.Name.Equals("ButtonFileSelect") Then
                RemoveHandler Me.TextBoxImageFileLocation.Validating, AddressOf Me.TextBoxImageFileLocation_Validating
                Call Me.ButtonFileSelect_Click(sender, e)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxImageFileLocation_GotFocus(sender As Object, e As EventArgs) Handles TextBoxImageFileLocation.GotFocus

        Try
            AddHandler Me.TextBoxImageFileLocation.Validating, AddressOf Me.TextBoxImageFileLocation_Validating
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class