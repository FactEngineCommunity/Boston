Imports System.Reflection

Public Class BPMNPopupToolElementChanger

    Public Type As pcenumBPMNProcessType
    Public Result As String

    Public mrCMMLModel As CMML.Model
    Public msLinkedProcessId As String
    Public moDiagram As MindFusion.Diagramming.Diagram

    Public mrPage As FBM.Page
    Public Node As MindFusion.Diagramming.WinForms.ControlNode

    Private Delegate Sub MyDelegate(ByRef arPage As FBM.Page, ByRef asInstructionType As String)

    Private Sub BPMNPopupToolSelector_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Public Sub SetupForm(Optional asFilter As String = Nothing)

        Try
            Select Case Me.Type
                Case Is = pcenumBPMNProcessType.Activity
                    Call Me.SetupForBPMNActivity(asFilter)
                Case Is = pcenumBPMNProcessType.Gateway
                    Call Me.SetupForBPMNGateway(asFilter)
            End Select

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try
    End Sub

    Private Sub SetupForBPMNActivity(Optional asFilter As String = Nothing)

        Try
            Dim lasList() As String = {"Task",
                                       "Send Task",
                                       "Receive Task",
                                       "User Task",
                                       "Manual Task",
                                       "Business Rule Task",
                                       "Service Task",
                                       "Script Task",
                                       "Call Activity"
                                        }

            Me.ListBox.Items.Clear()

            With Me.ListBox
                'Fill the list-box
                .UseCustomTabOffsets = True

                With .Items
                    For Each lsListItem In lasList
                        If asFilter IsNot Nothing Then
                            If lsListItem.IndexOf(asFilter, StringComparison.CurrentCultureIgnoreCase) = -1 Then
                                GoTo SkipItem
                            End If
                        End If
                        Select Case lsListItem
                            Case Is = "Task"
                                .Add("Task", My.Resources.BPMNElementDecorations.Task_25x25,,,)
                            Case Is = "Send Task"
                                .Add("Send Task", My.Resources.BPMNElementDecorations.TaskType_Send,,,)
                            Case Is = "Receive Task"
                                .Add("Receive Task", My.Resources.BPMNElementDecorations.TaskType_Receive,,,)
                            Case Is = "User Task"
                                .Add("User Task", My.Resources.BPMNElementDecorations.TaskType_User,,,)
                            Case Is = "Manual Task"
                                .Add("Manual Task", My.Resources.BPMNElementDecorations.TaskType_Manual,,,)
                            Case Is = "Business Rule Task"
                                .Add("Business Rule Task", My.Resources.BPMNElementDecorations.BusinessRule_25x25,,,)
                            Case Is = "Service Task"
                                .Add("Service Task", My.Resources.BPMNElementDecorations.TaskType_Service,,,)
                            Case Is = "Script Task"
                                .Add("Script Task", My.Resources.BPMNElementDecorations.TaskType_Script,,,)
                            Case Is = "Call Activity"
                                .Add("Call Activity", My.Resources.BPMNElementDecorations.CallActivity_25x25,,,)
                        End Select
SkipItem:
                    Next



                End With

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub SetupForBPMNGateway(Optional asFilter As String = Nothing)

        Try
            Dim lasList() As String = {"Parallel Gateway",
                                       "Inclusive Gateway",
                                       "Complex Gateway",
                                       "Event Based Gateway",
                                       "Exclusive Gateway"
                                        }

            Me.ListBox.Items.Clear()

            With Me.ListBox
                'Fill the list-box
                .UseCustomTabOffsets = True

                With .Items
                    For Each lsListItem In lasList
                        If asFilter IsNot Nothing Then
                            If lsListItem.IndexOf(asFilter, StringComparison.CurrentCultureIgnoreCase) = -1 Then
                                GoTo SkipItem
                            End If
                        End If
                        Select Case lsListItem
                            Case Is = "Parallel Gateway"
                                .Add("Parallel Gateway", My.Resources.BPMNElementDecorations.Gateway_Parallel,,,)
                            Case Is = "Inclusive Gateway"
                                .Add("Inclusive Gateway", My.Resources.BPMNElementDecorations.Gateway_Inclusive,,,)
                            Case Is = "Complex Gateway"
                                .Add("Complex Gateway", My.Resources.BPMNElementDecorations.Gateway_Complex,,,)
                            Case Is = "Event Based Gateway"
                                .Add("Event Based Gateway", My.Resources.BPMNElementDecorations.Gateway_EventBased,,,)
                            Case Is = "Exclusive Gateway"
                                .Add("Exclusive Gateway", My.Resources.BPMNElementDecorations.Gateway_Exclusive,,,)
                        End Select
SkipItem:
                    Next

                End With

            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub ListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox.SelectedIndexChanged

        Try
            Select Case Me.ListBox.SelectedItem
                Case Is = "Send Task"
                    Me.Result = "ChangeElementActivityTaskTypeSendTask"
                Case Is = "Receive Task"
                    Me.Result = "ChangeElementActivityTaskTypeReceiveTask"
                Case Is = "Manual Task"
                    Me.Result = "ChangeElementActivityTaskTypeManualTask"
                Case Is = "Business Rule Task"
                    Me.Result = "ChangeElementActivityTaskTypeBusinessRuleTask"
                Case Is = "Service Task"
                    Me.Result = "ChangeElementActivityTaskTypeServiceTask"
                Case Is = "Script Task"
                    Me.Result = "ChangeElementActivityTaskTypeScriptTask"
                Case Is = "User Task"
                    Me.Result = "ChangeElementActivityTaskTypeUserTask"
                Case Is = "Parallel Gateway"
                    Me.Result = "ChangeElementGatewayTypeParallel"
                Case Is = "Inclusive Gateway"
                    Me.Result = "ChangeElementGatewayTypeInclusive"
                Case Is = "Complex Gateway"
                    Me.Result = "ChangeElementGatewayTypeComplex"
                Case Is = "Event Based Gateway"
                    Me.Result = "ChangeElementGatewayTypeEventBased"
                Case Is = "Exclusive Gateway"
                    Me.Result = "ChangeElementGatewayTypeExlusive"
            End Select

            Me.moDiagram.Nodes.Remove(Me.Node)

            Dim delegate1 As MyDelegate = New MyDelegate(AddressOf frmDiagramBPMNCollaboration.ProcessPopupToolSelector)
            delegate1(Me.mrPage, Me.Result)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp

        Try
            If Trim(Me.TextBox1.Text).Length > 0 Then
                Call Me.SetupForm(Trim(Me.TextBox1.Text))
            Else
                Call SetupForm()
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub
End Class
