Imports System.Reflection

Public Class frmToolboxDescriptions

    Public WithEvents mrModelElement As FBM.ModelObject
    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub setDescriptions(ByRef arModelElement As FBM.ModelObject)

        Me.mrModelElement = arModelElement

        If arModelElement Is Nothing Then
            Me.LabelModelElementName.Text = "<No Model Element Selected>"

            Me.TextBoxShortDescription.Text = ""
            Me.TextBoxLongDescription.Text = ""
        Else
            Me.LabelModelElementName.Text = arModelElement.Id

            Me.TextBoxShortDescription.Text = arModelElement.ShortDescription
            Me.TextBoxLongDescription.Text = arModelElement.LongDescription
        End If

    End Sub

    Public Sub setLongDescription(ByRef arModelElement As FBM.ModelObject)

        Try
            If arModelElement IsNot Nothing Then
                Me.TextBoxLongDescription.Text = arModelElement.LongDescription.Trim
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Public Sub setShortDescription(ByRef arModelElement As FBM.ModelObject)

        Try
            If arModelElement IsNot Nothing Then
                Me.TextBoxShortDescription.Text = arModelElement.ShortDescription.Trim
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub frmToolboxDescriptions_Leave(sender As Object, e As EventArgs) Handles Me.Leave

        If Me.mrModelElement IsNot Nothing Then
            If Me.mrModelElement.Model IsNot Nothing Then
                Call Me.mrModelElement.SetShortDescription(Trim(Me.TextBoxShortDescription.Text))
                Call Me.mrModelElement.SetLongDescription(Trim(Me.TextBoxLongDescription.Text))
            End If
        End If

    End Sub

    Private Sub frmToolboxDescriptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)

    End Sub

    Private Sub mrModelElement_RemovedFromModel() Handles mrModelElement.RemovedFromModel

        Try
            Me.mrModelElement = Nothing
            Me.LabelModelElementName.Text = "<No Model Element Selected>"
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub mrModelElement_LongDescriptionChanged(asLongDescription As String) Handles mrModelElement.LongDescriptionChanged

        Try
            Call Me.setLongDescription(Me.mrModelElement)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub mrModelElement_ShortDescriptionChanged(asShortDescription As String) Handles mrModelElement.ShortDescriptionChanged

        Try
            Call Me.setShortDescription(Me.mrModelElement)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxShortDescription_Leave(sender As Object, e As EventArgs) Handles TextBoxShortDescription.Leave

        Try
            If Me.mrModelElement IsNot Nothing Then
                If Me.mrModelElement.Model IsNot Nothing Then
                    Call Me.mrModelElement.SetShortDescription(Trim(Me.TextBoxShortDescription.Text))
                    Call Me.mrModelElement.SetLongDescription(Trim(Me.TextBoxLongDescription.Text))
                    Call prApplication.ResetPropertiesGrid()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub TextBoxLongDescription_Leave(sender As Object, e As EventArgs) Handles TextBoxLongDescription.Leave

        Try
            If Me.mrModelElement IsNot Nothing Then
                If Me.mrModelElement.Model IsNot Nothing Then
                    Call Me.mrModelElement.SetShortDescription(Trim(Me.TextBoxShortDescription.Text))
                    Call Me.mrModelElement.SetLongDescription(Trim(Me.TextBoxLongDescription.Text))
                    Call prApplication.ResetPropertiesGrid()
                End If
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