Imports System.Reflection

Public Class frmModelElementSelector

    Public Result As Interlink.Interlink

    Private Sub frmModelElementSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Call Me.SetupForm()
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub SetupForm()

        Try
            Call Me.LoadModels()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub LoadModels()

        Try
            Me.ComboBoxModel.Items.Clear()

            RemoveHandler Me.ComboBoxModel.SelectedIndexChanged, AddressOf Me.ComboBoxModel_SelectedIndexChanged
            For Each lrModel In prApplication.Models
                Dim lrComboboxItem As New tComboboxItem(lrModel.ModelId, lrModel.Name, lrModel)
                Me.ComboBoxModel.Items.Add(lrComboboxItem)
            Next
            AddHandler Me.ComboBoxModel.SelectedIndexChanged, AddressOf Me.ComboBoxModel_SelectedIndexChanged

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Loads the ModelElements for a Model.
    ''' </summary>
    ''' <param name="asModelId"></param>
    Private Sub LoadModelElementsForModel(ByVal asModelId As String)

        Try
            Dim lrModel As FBM.Model = prApplication.Models.Find(Function(x) x.ModelId = asModelId)

            If lrModel Is Nothing Then
                Throw New Exception("Model not found")
            Else
                If Not lrModel.Loaded Then Call lrModel.Load(False)
            End If

            Me.ComboBoxModelElement.Items.Clear()
            For Each lrModelElement In lrModel.getModelObjects
                Dim lrComboboxItem = New tComboboxItem(lrModelElement.Id, lrModelElement.Id, lrModelElement)
                Me.ComboBoxModelElement.Items.Add(lrComboboxItem)
            Next

            If Me.ComboBoxModelElement.Items.Count > 0 Then
                Me.ComboBoxModelElement.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ComboBoxModel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxModel.SelectedIndexChanged

        Try

            Call Me.LoadModelElementsForModel(Me.ComboBoxModel.SelectedItem.ItemData)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ButtonOkay_Click(sender As Object, e As EventArgs) Handles ButtonOkay.Click

        Try
            If Me.ComboBoxModel.SelectedIndex >= 0 And Me.ComboBoxModelElement.SelectedIndex >= 0 Then

                Me.Result = New Interlink.Interlink
                Me.Result.TargetModelId = Me.ComboBoxModel.SelectedItem.ItemData
                Me.Result.TargetModelElementId = Me.ComboBoxModelElement.SelectedItem.ItemData

                Me.DialogResult = DialogResult.OK

                Me.Hide()
                Me.Close()
                Me.Dispose()

            Else
                Me.Result = Nothing
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

End Class