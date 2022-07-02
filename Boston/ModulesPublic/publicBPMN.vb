Public Module publicBPMN

    Public Enum pcenumBPMNPopupToolType
        Changer
        Adder
    End Enum
    Public Enum pcenumBPMNProcessType
        Activity
        Flow
        Conversation
        [Event]
        Gateway
    End Enum

    'Public Enum pcenumBPMNElementType
    '    Activity
    '    Flow
    '    Conversation
    '    [Event]
    '    Gateway
    'End Enum

#Region "Activity - BPMN"
    Public Enum pcenumBPMNActivityType
        Task
        Transaction
        EventSubProcess
        CallActivity
    End Enum

    Public Enum pcenumBPMNActivityMarker
        SubProcessMarker
        LoopMarker
        ParallelMIMarker
        SequentialMIMarker
        AdHocMarker
        CompensationMarker
    End Enum

    Public Enum pcenumBPMNActivityTaskType
        SendTask
        ReceiveTask
        UserTask
        ManualTask
        BusinessRuleTask
        ServiceTask
        ScriptTask
    End Enum
#End Region

#Region "Flow/Link - BPMN"
    Public Enum pcenumBPMNFlowType
        DefaultFlow
        SequenceFlow
        ConditionalFlow
    End Enum
#End Region

#Region "Conversation - BPMN"
    Public Enum pcenumBPMNConversationType
        Conversation
        CallConversation
    End Enum
#End Region

#Region "Events - BPMN"
    Public Enum pcenumBPMNEventPosition
        Start
        Intermediate
        [End]
    End Enum

    Public Enum pcenumBPMNEventType
        None
        Message
        Timer
        Escalation
        Conditional
        Link
        [Error]
        Cancel
        Compensation
        Signal
        Multiple
        ParallelMultiple
        Terminate
    End Enum

    Public Enum pcenumBPMNEventSubType
        Standard
        SubprocessInterupting
        SubprocessNonInterupting
        Catching
        BoundaryInterupting
        BoundaryNonInterupting
        Throwing
    End Enum

#End Region

#Region "Gateway - BPMN"
    Public Enum pcenumBPMNGatewayType
        ExclusiveGateway
        EventBasedGateway
        ParallelGateway
        InclusiveGateway
        ComplexGateway
        ExclusiveEventBasedGateway
        ParallelEventBasedGateway
    End Enum
#End Region

End Module
