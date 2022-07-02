Imports System.Reflection

Public Class BPMNPopupToolElementChanger

    Public ViewType As pcenumBPMNPopupToolType = pcenumBPMNPopupToolType.Changer
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
                Case Is = pcenumBPMNProcessType.Event
                    Call Me.SetupForBPMNEvent(asFilter)
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

    Private Sub SetupForBPMNEvent(Optional asFilter As String = Nothing)

        Try
            Dim lasList() As String

            Me.ListBox.Items.Clear()

            'If asFilter IsNot Nothing Then
#Region "All"
            lasList = {"Event End Cancel Standard",
                            "Event End Compensation Standard",
                            "Event End Error Standard",
                            "Event End Escalation Standard",
                            "Event End Link Standard",
                            "Event End Message Standard",
                            "Event End Multiple Standard",
                            "Event End None Standard",
                            "Event End Signal Standard",
                            "Event End Terminate Standard",
                            "Event Intermediate Cancel Boundary Interupting",
                            "Event Intermediate Compensation Boundary Interupting",
                            "Event Intermediate Compensation Subprocess Interupting",
                            "Event Intermediate Compensation Throwing",
                            "Event Intermediate Conditioal Boundary NonInterupting",
                            "Event Intermediate Conditional Boundary Interupting",
                            "Event Intermediate Conditional Catching",
                            "Event Intermediate Error Boundary Interupting",
                            "Event Intermediate Error Subprocess Interupting",
                            "Event Intermediate Escalation Boundary Interupting",
                            "Event Intermediate Escalation Boundary NonInterupting",
                            "Event Intermediate Escalation Subprocess Interupting",
                            "Event Intermediate Escalation Subprocess NonInterupting",
                            "Event Intermediate Escalation Throwing",
                            "Event Intermediate Link Catching",
                            "Event Intermediate Link Throwing",
                            "Event Intermediate Message Boundary Interupting",
                            "Event Intermediate Message Boundary NonInterupting",
                            "Event Intermediate Message Catching",
                            "Event Intermediate Message Subprocess NonInterupting",
                            "Event Intermediate Message Throwing",
                            "Event Intermediate Multiple Boundary Interupting",
                            "Event Intermediate Multiple Boundary NonInterupting",
                            "Event Intermediate Multiple Catching",
                            "Event Intermediate Multiple Subprocess NonInterupting",
                            "Event Intermediate Multiple Throwing",
                            "Event Intermediate None Throwing",
                            "Event Intermediate Parallel Subprocess NonInterupting",
                            "Event Intermediate Parallel Multiple Boundary Interupting",
                            "Event Intermediate Parallel Multiple Boundary NonInterupting",
                            "Event Intermediate Parallel Multiple Catching",
                            "Event Intermediate Signal Boundary Interupting",
                            "Event Intermediate Signal Boundary NonInterupting",
                            "Event Intermediate Signal Catching",
                            "Event Intermediate Signal Subprocess NonInterupting",
                            "Event Intermediate Signal Subprocess Interupting",
                            "Event Intermediate Signal Throwing",
                            "Event Intermediate Timer Boundary Interupting",
                            "Event Intermediate Timer Boundary NonInterupting",
                            "Event Intermediate Timer Subprocess Interupting",
                            "Event Intermediate Timer Subprocess NonInterupting",
                            "Event Intermediate Timer Catching",
                            "Event Start Conditional Standard",
                            "Event Start Conditional Subprocess Interupting",
                            "Event Start Message Standard",
                            "Event Start Message Subprocess Interupting",
                            "Event Start Multiple Standard",
                            "Event Start Multiple Subprocess Interupting",
                            "Event Start None Standard",
                            "Event Start Parallel Multiple Standard",
                            "Event Start ParallelMultiple Subprocess Interupting",
                            "Event Start Signal Standard",
                            "Event Start Timer Standard"
                                        }
#End Region
            '            Else
            '#Region "Short"
            '                lasList = {"Start Event",
            '                            "End Event",
            '                            "Message Intermediate Catch Event",
            '                            "Message Intermediate Throw Event",
            '                            "Timer Intermediate Catch Event",
            '                            "Escalation Intermediate Throw Event",
            '                            "Conditional Intermediate Catch Event",
            '                            "Link Intermediate Catch Event",
            '                            "Link Intermediate Throw Event",
            '                            "Compensation Intermediate Throw Event",
            '                            "Signal Intermediate Catch Event",
            '                            "Signal Intermediate Throw Event"
            '                            }
            '#End Region
            '            End If


            With Me.ListBox
                'Fill the list-box
                .UseCustomTabOffsets = True

                With .Items
                    For Each lsListItem In lasList

                        If asFilter IsNot Nothing Then
                            For Each subString In asFilter.Split(" ")
                                If lsListItem.IndexOf(subString, StringComparison.CurrentCultureIgnoreCase) = -1 Then
                                    GoTo SkipItem
                                End If
                            Next
                        End If

#Region "Older"
                        '                        Select Case lsListItem

                        '                            Case Is = "Start Event"
                        '                                .Add("Start Event", My.Resources.BPMN.Event_Start_None_Standard,,,)

                        '                            Case Is = "End Event"
                        '                                .Add("End Event", My.Resources.BPMN.Event_End_None_Standard,,,)

                        '                            Case Is = "Message Intermediate Catch Event"
                        '                                .Add("Message Intermediate Catch Event", My.Resources.BPMN.Event_Intermediate_Message_Catching,,,)

                        '                            Case Is = "Message Intermediate Throw Event"
                        '                                .Add("Message Intermediate Throw Event", My.Resources.BPMN.Event_Intermediate_Message_Throwing,,,)

                        '                            Case Is = "Timer Intermediate Catch Event"
                        '                                .Add("Timer Intermediate Catch Event", My.Resources.BPMN.Event_Intermediate_Timer_Catching,,,)

                        '                            Case Is = "Escalation Intermediate Throw Event"
                        '                                .Add("Escalation Intermediate Throw Event", My.Resources.BPMN.Event_Intermediate_Escalation_Throwing,,,)

                        '                            Case Is = "Conditional Intermediate Catch Event"
                        '                                .Add("Conditional Intermediate Catch Event", My.Resources.BPMN.Event_Intermediate_Conditional_Catching,,,)

                        '                            Case Is = "Link Intermediate Catch Event"
                        '                                .Add("Link Intermediate Catch Event", My.Resources.BPMN.Event_Intermediate_Link_Catching,,,)

                        '                            Case Is = "Link Intermediate Throw Event"
                        '                                .Add("Link Intermediate Throw Event", My.Resources.BPMN.Event_Intermediate_Link_Throwing,,,)

                        '                            Case Is = "Compensation Intermediate Throw Event"
                        '                                .Add("Compensation Intermediate Throw Event", My.Resources.BPMN.Event_Intermediate_Compensation_Throwing,,,)

                        '                            Case Is = "Signal Intermediate Catch Event"
                        '                                .Add("Signal Intermediate Catch Event", My.Resources.BPMN.Event_Start_Signal_Standard,,,)

                        '                            Case Is = "Signal Intermediate Throw Event"
                        '                                .Add("Signal Intermediate Throw Event", My.Resources.BPMN.Event_Intermediate_None_Throwing,,,)

                        '                        End Select
#End Region

                        Select Case lsListItem
                            Case Is = "Event End Cancel Standard"
                                .Add("Event End Cancel Standard", My.Resources.BPMN.Event_End_Cancel_Standard,,,)
                            Case Is = "Event End Compensation Standard"
                                .Add("Event End Compensation Standard", My.Resources.BPMN.Event_End_Compensation_Standard,,,)
                            Case Is = "Event End Error Standard"
                                .Add("Event End Error Standard", My.Resources.BPMN.Event_End_Error_Standard,,,)
                            Case Is = "Event End Escalation Standard"
                                .Add("Event End Escalation Standard", My.Resources.BPMN.Event_End_Escalation_Standard,,,)
                            Case Is = "Event End Link Standard"
                                .Add("Event End Link Standard", My.Resources.BPMN.Event_End_Link_Standard,,,)
                            Case Is = "Event End Message Standard"
                                .Add("Event End Message Standard", My.Resources.BPMN.Event_End_Message_Standard,,,)
                            Case Is = "Event End Multiple Standard"
                                .Add("Event End Multiple Standard", My.Resources.BPMN.Event_End_Multiple_Standard,,,)
                            Case Is = "Event End None Standard"
                                .Add("Event End None Standard", My.Resources.BPMN.Event_End_None_Standard,,,)
                            Case Is = "Event End Signal Standard"
                                .Add("Event End Signal Standard", My.Resources.BPMN.Event_End_Signal_Standard,,,)
                            Case Is = "Event End Terminate Standard"
                                .Add("Event End Terminate Standard", My.Resources.BPMN.Event_End_Terminate_Standard,,,)
                            Case Is = "Event Intermediate Cancel Boundary Interupting"
                                .Add("Event Intermediate Cancel Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Cancel_BoundaryInterupting,,,)
                            Case Is = "Event Intermediate Compensation Boundary Interupting"
                                .Add("Event Intermediate Compensation Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Compensation_BoundaryInterupting,,,)
                            Case Is = "Event Intermediate Compensation Subprocess Interupting"
                                .Add("Event Intermediate Compensation Subprocess Interupting", My.Resources.BPMN.Event_Intermediate_Compensation_SubprocessInterupting,,,)

                            Case Is = "Event Intermediate Compensation Throwing"
                                .Add("Event Intermediate Compensation Throwing", My.Resources.BPMN.Event_Intermediate_Compensation_Throwing,,,)

                            Case Is = "Event Intermediate Conditioal Boundary NonInterupting"
                                .Add("Event Intermediate Conditioal Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Conditioal_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Conditional Boundary Interupting"
                                .Add("Event Intermediate Conditional Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Conditional_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Conditional Catching"
                                .Add("Event Intermediate Conditional Catching", My.Resources.BPMN.Event_Intermediate_Conditional_Catching,,,)

                            Case Is = "Event Intermediate Error Boundary Interupting"
                                .Add("Event Intermediate Error Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Error_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Error Subprocess Interupting"
                                .Add("Event Intermediate Error Subprocess Interupting", My.Resources.BPMN.Event_Intermediate_Error_SubprocessInterupting,,,)

                            Case Is = "Event Intermediate Escalation Boundary Interupting"
                                .Add("Event Intermediate Escalation Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Escalation Boundary NonInterupting"
                                .Add("Event Intermediate Escalation Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Escalation Subprocess Interupting"
                                .Add("Event Intermediate Escalation Subprocess Interupting", My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessInterupting,,,)

                            Case Is = "Event Intermediate Escalation Subprocess NonInterupting"
                                .Add("Event Intermediate Escalation Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessNonInterupting,,,)

                            Case Is = "Event Intermediate Escalation Throwing"
                                .Add("Event Intermediate Escalation Throwing", My.Resources.BPMN.Event_Intermediate_Escalation_Throwing,,,)

                            Case Is = "Event Intermediate Link Catching"
                                .Add("Event Intermediate Link Catching", My.Resources.BPMN.Event_Intermediate_Link_Catching,,,)

                            Case Is = "Event Intermediate Link Throwing"
                                .Add("Event Intermediate Link Throwing", My.Resources.BPMN.Event_Intermediate_Link_Throwing,,,)

                            Case Is = "Event Intermediate Message Boundary Interupting"
                                .Add("Event Intermediate Message Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Message_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Message Boundary NonInterupting"
                                .Add("Event Intermediate Message Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Message_BoundaryNonInterupting___Copy,,,)

                            Case Is = "Event Intermediate Message Catching"
                                .Add("Event Intermediate Message Catching", My.Resources.BPMN.Event_Intermediate_Message_Catching,,,)

                            Case Is = "Event Intermediate Message Subprocess NonInterupting"
                                .Add("Event Intermediate Message Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Message_SubprocessNonInterupting,,,)

                            Case Is = "Event Intermediate Message Throwing"
                                .Add("Event Intermediate Message Throwing", My.Resources.BPMN.Event_Intermediate_Message_Throwing,,,)

                            Case Is = "Event Intermediate Multiple Boundary Interupting"
                                .Add("Event Intermediate Multiple Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Multiple Boundary NonInterupting"
                                .Add("Event Intermediate Multiple Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Multiple Catching"
                                .Add("Event Intermediate Multiple Catching", My.Resources.BPMN.Event_Intermediate_Multiple_Catching,,,)

                            Case Is = "Event Intermediate Multiple Subprocess NonInterupting"
                                .Add("Event Intermediate Multiple Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Multiple_SubprocessNonInterupting,,,)

                            Case Is = "Event Intermediate Multiple Throwing"
                                .Add("Event Intermediate Multiple Throwing", My.Resources.BPMN.Event_Intermediate_Multiple_Throwing,,,)

                            Case Is = "Event Intermediate None Throwing"
                                .Add("Event Intermediate None Throwing", My.Resources.BPMN.Event_Intermediate_None_Throwing,,,)

                            Case Is = "Event Intermediate Parallel Subprocess NonInterupting"
                                .Add("Event Intermediate Parallel Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Parallel_SubprocessNonInterupting,,,)

                            Case Is = "Event Intermediate Parallel Multiple Boundary Interupting"
                                .Add("Event Intermediate Parallel Multiple Boundary Interupting", My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Parallel Multiple Boundary NonInterupting"
                                .Add("Event Intermediate Parallel Multiple Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Parallel Multiple Catching"
                                .Add("Event Intermediate Parallel Multiple Catching", My.Resources.BPMN.Event_Intermediate_ParallelMultiple_Catching,,,)

                            Case Is = "Event Intermediate Signal Boundary Interupting"
                                .Add("Event Intermediate Signal Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Signal_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Signal Boundary NonInterupting"
                                .Add("Event Intermediate Signal Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Signal_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Signal Catching"
                                .Add("Event Intermediate Signal Catching", My.Resources.BPMN.Event_Intermediate_Signal_Catching,,,)

                            Case Is = "Event Intermediate Signal Subprocess NonInterupting"
                                .Add("Event Intermediate Signal Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Signal_Subprocess_NonInterupting,,,)

                            Case Is = "Event Intermediate Signal Subprocess Interupting"
                                .Add("Event Intermediate Signal Subprocess Interupting", My.Resources.BPMN.Event_Intermediate_Signal_SubprocessInterupting,,,)

                            Case Is = "Event Intermediate Signal Throwing"
                                .Add("Event Intermediate Signal Throwing", My.Resources.BPMN.Event_Intermediate_Signal_Throwing,,,)

                            Case Is = "Event Intermediate Timer Boundary Interupting"
                                .Add("Event Intermediate Timer Boundary Interupting", My.Resources.BPMN.Event_Intermediate_Timer_BoundaryInterupting,,,)

                            Case Is = "Event Intermediate Timer Boundary NonInterupting"
                                .Add("Event Intermediate Timer Boundary NonInterupting", My.Resources.BPMN.Event_Intermediate_Timer_BoundaryNonInterupting,,,)

                            Case Is = "Event Intermediate Timer Subprocess Interupting"
                                .Add("Event Intermediate Timer Subprocess Interupting", My.Resources.BPMN.Event_Intermediate_Timer_SubprocessInterupting,,,)

                            Case Is = "Event Intermediate Timer Subprocess NonInterupting"
                                .Add("Event Intermediate Timer Subprocess NonInterupting", My.Resources.BPMN.Event_Intermediate_Timer_SubprocessNonInterupting,,,)

                            Case Is = "Event Intermediate Timer Catching"
                                .Add("Event Intermediate Timer Catching", My.Resources.BPMN.Event_Intermediate_Timer_Catching,,,)

                            Case Is = "Event Start Conditional Standard"
                                .Add("Event Start Conditional Standard", My.Resources.BPMN.Event_Start_Conditional_Standard,,,)

                            Case Is = "Event Start Conditional Subprocess Interupting"
                                .Add("Event Start Conditional Subprocess Interupting", My.Resources.BPMN.Event_Start_Conditional_SubprocessInterupting,,,)

                            Case Is = "Event Start Message Standard"
                                .Add("Event Start Message Standard", My.Resources.BPMN.Event_Start_Message_Standard,,,)

                            Case Is = "Event Start Message Subprocess Interupting"
                                .Add("Event Start Message Subprocess Interupting", My.Resources.BPMN.Event_Start_Message_SubprocessInterupting,,,)

                            Case Is = "Event Start Multiple Standard"
                                .Add("Event Start Multiple Standard", My.Resources.BPMN.Event_Start_Multiple_Standard,,,)

                            Case Is = "Event Start Multiple Subprocess Interupting"
                                .Add("Event Start Multiple Subprocess Interupting", My.Resources.BPMN.Event_Start_Multiple_SubprocessInterupting,,,)

                            Case Is = "Event Start None Standard"
                                .Add("Event Start None Standard", My.Resources.BPMN.Event_Start_None_Standard,,,)

                            Case Is = "Event Start Parallel Multiple Standard"
                                .Add("Event Start Parallel Multiple Standard", My.Resources.BPMN.Event_Start_ParallelMultiple_Standard,,,)

                            Case Is = "Event Start Parallel Multiple Subprocess Interupting"
                                .Add("Event Start Parallel Multiple Subprocess Interupting", My.Resources.BPMN.Event_Start_ParallelMultiple_SubprocessInterupting,,,)

                            Case Is = "Event Start Signal Standard"
                                .Add("Event Start Signal Standard", My.Resources.BPMN.Event_Start_Signal_Standard,,,)

                            Case Is = "Event Start Timer Standard"
                                .Add("Event Start Timer Standard", My.Resources.BPMN.Event_Start_Timer_Standard,,,)

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
#Region "Tasks"
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
#End Region

#Region "Gateways"
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
#End Region

#Region "Events"
                Case Is = "Event End Cancel Standard"
                    Me.Result = "ChangeElementEventEndCancelStandard"

                Case Is = "Event End Compensation Standard"
                    Me.Result = "ChangeElementEventEndCompensationStandard"

                Case Is = "Event End Error Standard"
                    Me.Result = "ChangeElementEventEndErrorStandard"

                Case Is = "Event End Escalation Standard"
                    Me.Result = "ChangeElementEventEndEscalationStandard"

                Case Is = "Event End Link Standard"
                    Me.Result = "ChangeElementEventEndLinkStandard"

                Case Is = "Event End Message Standard"
                    Me.Result = "ChangeElementEventEndMessageStandard"

                Case Is = "Event End Multiple Standard"
                    Me.Result = "ChangeElementEventEndMultipleStandard"

                Case Is = "Event End None Standard"
                    Me.Result = "ChangeElementEventEndNoneStandard"

                Case Is = "Event End Signal Standard"
                    Me.Result = "ChangeElementEventEndSignalStandard"

                Case Is = "Event End Terminate Standard"
                    Me.Result = "ChangeElementEventEndTerminateStandard"

                Case Is = "Event Intermediate Cancel Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateCancelBoundaryInterupting"

                Case Is = "Event Intermediate Compensation Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateCompensationBoundaryInterupting"

                Case Is = "Event Intermediate Compensation Subprocess Interupting"
                    Me.Result = "ChangeElementEventIntermediateCompensationSubprocessInterupting"

                Case Is = "Event Intermediate Compensation Throwing"
                    Me.Result = "ChangeElementEventIntermediateCompensationThrowing"

                Case Is = "Event Intermediate Conditioal Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateConditioalBoundaryNonInterupting"

                Case Is = "Event Intermediate Conditional Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateConditionalBoundaryInterupting"

                Case Is = "Event Intermediate Conditional Catching"
                    Me.Result = "ChangeElementEventIntermediateConditionalCatching"

                Case Is = "Event Intermediate Error Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateErrorBoundaryInterupting"

                Case Is = "Event Intermediate Error Subprocess Interupting"
                    Me.Result = "ChangeElementEventIntermediateErrorSubprocessInterupting"

                Case Is = "Event Intermediate Escalation Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateEscalationBoundaryInterupting"

                Case Is = "Event Intermediate Escalation Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateEscalationBoundaryNonInterupting"

                Case Is = "Event Intermediate Escalation Subprocess Interupting"
                    Me.Result = "ChangeElementEventIntermediateEscalationSubprocessInterupting"

                Case Is = "Event Intermediate Escalation Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateEscalationSubprocessNonInterupting"

                Case Is = "Event Intermediate Escalation Throwing"
                    Me.Result = "ChangeElementEventIntermediateEscalationThrowing"

                Case Is = "Event Intermediate Link Catching"
                    Me.Result = "ChangeElementEventIntermediateLinkCatching"

                Case Is = "Event Intermediate Link Throwing"
                    Me.Result = "ChangeElementEventIntermediateLinkThrowing"

                Case Is = "Event Intermediate Message Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateMessageBoundaryInterupting"

                Case Is = "Event Intermediate Message Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateMessageBoundaryNonInterupting"

                Case Is = "Event Intermediate Message Catching"
                    Me.Result = "ChangeElementEventIntermediateMessageCatching"

                Case Is = "Event Intermediate Message Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateMessageSubprocessNonInterupting"

                Case Is = "Event Intermediate Message Throwing"
                    Me.Result = "ChangeElementEventIntermediateMessageThrowing"

                Case Is = "Event Intermediate Multiple Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateMultipleBoundaryInterupting"

                Case Is = "Event Intermediate Multiple Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateMultipleBoundaryNonInterupting"

                Case Is = "Event Intermediate Multiple Catching"
                    Me.Result = "ChangeElementEventIntermediateMultipleCatching"

                Case Is = "Event Intermediate Multiple Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateMultipleSubprocessNonInterupting"

                Case Is = "Event Intermediate Multiple Throwing"
                    Me.Result = "ChangeElementEventIntermediateMultipleThrowing"

                Case Is = "Event Intermediate None Throwing"
                    Me.Result = "ChangeElementEventIntermediateNoneThrowing"

                Case Is = "Event Intermediate Parallel Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateParallelSubprocessNonInterupting"

                Case Is = "Event Intermediate Parallel Multiple Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateParallelMultipleBoundaryInterupting"

                Case Is = "Event Intermediate Parallel Multiple Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateParallelMultipleBoundaryNonInterupting"

                Case Is = "Event Intermediate Parallel Multiple Catching"
                    Me.Result = "ChangeElementEventIntermediateParallelMultipleCatching"

                Case Is = "Event Intermediate Signal Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateSignalBoundaryInterupting"

                Case Is = "Event Intermediate Signal Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateSignalBoundaryNonInterupting"

                Case Is = "Event Intermediate Signal Catching"
                    Me.Result = "ChangeElementEventIntermediateSignalCatching"

                Case Is = "Event Intermediate Signal Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateSignalSubprocessNonInterupting"

                Case Is = "Event Intermediate Signal Subprocess Interupting"
                    Me.Result = "ChangeElementEventIntermediateSignalSubprocessInterupting"

                Case Is = "Event Intermediate Signal Throwing"
                    Me.Result = "ChangeElementEventIntermediateSignalThrowing"

                Case Is = "Event Intermediate Timer Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateTimerBoundaryInterupting"

                Case Is = "Event Intermediate Timer Boundary NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateTimerBoundaryNonInterupting"

                Case Is = "Event Intermediate Timer Subprocess Interupting"
                    Me.Result = "ChangeElementEventIntermediateTimerSubprocessInterupting"

                Case Is = "Event Intermediate Timer Subprocess NonInterupting"
                    Me.Result = "ChangeElementEventIntermediateTimerSubprocessNonInterupting"

                Case Is = "Event Intermediate Timer Boundary Interupting"
                    Me.Result = "ChangeElementEventIntermediateTimerBoundaryInterupting"

                Case Is = "Event Intermediate Timer Catching"
                    Me.Result = "ChangeElementEventIntermediateTimerCatching"

                Case Is = "Event Start Conditional Standard"
                    Me.Result = "ChangeElementEventStartConditionalStandard"

                Case Is = "Event Start Conditional Subprocess Interupting"
                    Me.Result = "ChangeElementEventStartConditionalSubprocessInterupting"

                Case Is = "Event Start Message Standard"
                    Me.Result = "ChangeElementEventStartMessageStandard"

                Case Is = "Event Start Message Subprocess Interupting"
                    Me.Result = "ChangeElementEventStartMessageSubprocessInterupting"

                Case Is = "Event Start Multiple Standard"
                    Me.Result = "ChangeElementEventStartMultipleStandard"

                Case Is = "Event Start Multiple Subprocess Interupting"
                    Me.Result = "ChangeElementEventStartMultipleSubprocessInterupting"

                Case Is = "Event Start None Standard"
                    Me.Result = "ChangeElementEventStartNoneStandard"

                Case Is = "Event Start Parallel Multiple Standard"
                    Me.Result = "ChangeElementEventStartParallelMultipleStandard"

                Case Is = "Event Start Parallel Multiple Subprocess Interupting"
                    Me.Result = "ChangeElementEventStartParallelMultipleSubprocessInterupting"

                Case Is = "Event Start Signal Standard"
                    Me.Result = "ChangeElementEventStartSignalStandard"

                Case Is = "Event Start Timer Standard"
                    Me.Result = "ChangeElementEventStartTimerStandard"
#End Region

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
