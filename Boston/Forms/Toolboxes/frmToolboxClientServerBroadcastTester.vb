Imports System.Reflection

Public Class frmToolboxClientServerBroadcastTester

    Private Sub frmToolboxClientServerBroadcastTester_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Try
            'BroadcastType combobox
            Me.ComboBoxBroadcastType.Items.Add(New tComboboxItem([Interface].pcenumBroadcastType.ModelGetModelIdByModelName,
                                                                 "ModelGetModelIdByModelName",
                                                                 [Interface].pcenumBroadcastType.ModelGetModelIdByModelName))

            Me.ComboBoxBroadcastType.Items.Add(New tComboboxItem([Interface].pcenumBroadcastType.FEKLStatement,
                                                                 "FEKLStatement",
                                                                 [Interface].pcenumBroadcastType.FEKLStatement))

            Me.ComboBoxBroadcastType.Items.Add(New tComboboxItem([Interface].pcenumBroadcastType.ModelCreate,
                                                                 "ModelCreate",
                                                                 [Interface].pcenumBroadcastType.ModelCreate))

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ButtonSendBroadcast_Click(sender As Object, e As EventArgs) Handles ButtonSendBroadcast.Click

        Try
            Call Me.ProcessSendBroadcast()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessSendBroadcast()

        Dim lsMessage As String

        Try
            If Me.ComboBoxBroadcastType.SelectedIndex < 0 Then
                lsMessage = "Select a Broadcast Type before clicking on [Send Broadast]"
                Me.TextBoxClientServerResponce.Text = lsMessage
                Exit Sub
            End If

            If My.Settings.UseClientServer And My.Settings.InitialiseClient Then

                Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                lrInterfaceModel.ModelId = Trim(Me.TextBoxModelId.Text)
                lrInterfaceModel.Name = Trim(Me.TextBoxModelName.Text)
                lrInterfaceModel.Namespace = ""
                lrInterfaceModel.ProjectId = ""
                lrInterfaceModel.StoreAsXML = False

                Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                lrBroadcast.Model = lrInterfaceModel

                Dim liInterfaceBroadcastType As [Interface].pcenumBroadcastType = Me.ComboBoxBroadcastType.SelectedItem.ItemData

                Select Case liInterfaceBroadcastType
                    Case Is = [Interface].pcenumBroadcastType.ModelGetModelIdByModelName
                    Case Is = [Interface].pcenumBroadcastType.FEKLStatement
                        Dim lrFEKLStatement As New [Interface].FEKLStatement
                        lrFEKLStatement.ModelId = lrInterfaceModel.ModelId
                        lrFEKLStatement.Statement = Trim(Me.TextBoxFEKLStatement.Text)
                        lrBroadcast.FEKLStatement = lrFEKLStatement
                End Select

                Call prDuplexServiceClient.SendBroadcast(liInterfaceBroadcastType, lrBroadcast)

                lsMessage = "Error Code:" & lrBroadcast.ErrorCode.ToString
                lsMessage.AppendLine("Error Message: " & lrBroadcast.ErrorMessage)

                Me.TextBoxClientServerResponce.Text = lsMessage

                Select Case liInterfaceBroadcastType
                    Case Is = [Interface].pcenumBroadcastType.ModelGetModelIdByModelName
                        Me.TextBoxModelId.Text = Trim(lrBroadcast.Model.ModelId)
                    Case Is = [Interface].pcenumBroadcastType.FEKLStatement
                        'Nothing at this stage
                End Select



            Else
                lsMessage = "Need at least the following to be set to True:"
                lsMessage.AppendDoubleLineBreak("My.Settings.UseClientServer And My.Settings.InitialiseClient")
                Me.TextBoxClientServerResponce.Text = lsMessage
            End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Class