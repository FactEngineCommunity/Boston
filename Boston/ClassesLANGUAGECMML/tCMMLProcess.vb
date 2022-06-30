Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace CMML
    <Serializable()>
    Public Class Process
        Inherits CMML.Element

        Public CMMLModel As CMML.Model

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public ProcessType As pcenumBPMNProcessType

        Public Id As String

        ''' <summary>
        ''' The text of the Process
        ''' </summary>
        Public Text As String

        Public Name As String

#Region "Activity - BPMN"
        Public ActivityType As pcenumBPMNActivityType

        Public ActivityMarker As pcenumBPMNActivityMarker

        Public ActivityTaskType As pcenumBPMNActivityTaskType
#End Region

#Region "Conversation - BPMN"
        Public ConversationType As pcenumBPMNConversationType
#End Region

#Region "Events - BPMN"

        Public EventPosition As pcenumBPMNEventPosition

        Public EventType As pcenumBPMNEventType

        Public EventSubType As pcenumBPMNEventSubType
#End Region

#Region "Gateway - BPMN"

        Public GatewayType As pcenumBPMNGatewayType
#End Region

        '20220617-VM-Can probably make these redundant/derived.
        '''' <summary>
        '''' Processes participated with by the Process
        '''' </summary>
        'Public Process As New List(Of CMML.Process)
        'Public Extended_by_process As List(Of CMML.Process)

        ''' <summary>
        ''' The SequenceNr assigned to the Process in a sequence of Processes in (say) a FlowChart or EventTraceDiagram
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        Public Shadows IsDecision As Boolean = False
        Public IsStart As Boolean = False
        Public IsStop As Boolean = False

        ''' <summary>
        ''' The Actor responsible for the process, as in (say) an EventTraceDiagram
        ''' </summary>
        ''' <remarks></remarks>
        Public ResponsibleActor As CMML.Actor

        <CategoryAttribute("Process"),
             DefaultValueAttribute(GetType(String), ""),
             DescriptionAttribute("Name of the Process.")>
        Public Property ProcessName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        Public Event FactChanged(ByRef arFact As FBM.Fact)
        Public Event RemovedFromModel()
        Public Event TextChanged(ByVal asNewText As String)
        Public Event ActivityTypeChanged(aiBPMNActityType As pcenumBPMNActivityType)
        Public Event ActivityMarkerChanged(aiBPMNActivityMarker As pcenumBPMNActivityMarker)
        Public Event ActivityTaskTypeChanged(ByVal aiBPMNActivityTaskType As pcenumBPMNActivityTaskType)
        Public Event ConversationTypeChanged(ByVal aiBPMNConversationType As pcenumBPMNConversationType)
        Public Event EventPositionChanged(ByVal aiBPMNEventPosition As pcenumBPMNEventPosition)
        Public Event EventTypeChanged(ByVal aiBPMNEventType As pcenumBPMNEventType)
        Public Event EventSubTypeChanged(ByVal aiBPMNSubType As pcenumBPMNEventSubType)
        Public Event GatewayTypeChanged(ByVal aiBPMNGatewayType As pcenumBPMNGatewayType)

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arModel As CMML.Model, ByVal asProcessId As String, ByVal asProcessText As String)

            Call MyBase.New

            Me.Id = asProcessId
            Me.CMMLModel = arModel
            Me.Text = asProcessText

        End Sub

        Public Shadows Function EqualsByName(ByVal other As CMML.Process) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareSequenceNrs(ByVal aoA As CMML.Process, ByVal aoB As CMML.Process) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'SortRoleGroup'
            '------------------------------------------------------
            Dim loa As New Object
            Dim lob As New Object

            Try
                Return aoA.SequenceNr - aoB.SequenceNr

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Shared Function CompareText(x As CMML.Process, y As CMML.Process) As Integer

            Return String.Compare(x.Text, y.Text)

        End Function

        Public Function getParticipatingActors() As List(Of CMML.Actor)

            Try
                Dim larParticipatingActor = From ActorProcessRelation In Me.CMMLModel.ActorProcessRelation
                                            Where ActorProcessRelation.Process.Id = Me.Id
                                            Select ActorProcessRelation.Actor

                Return larParticipatingActor.ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of CMML.Actor)
            End Try
        End Function

        Public Sub RemoveFromModel()

            Try
                Call Me.CMMLModel.RemoveProcess(Me)

                RaiseEvent RemovedFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetProcessText(ByVal asNewProcessText As String)

            Try
                Me.Text = asNewProcessText

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessText(Me)

                RaiseEvent TextChanged(Me.Text)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub SetActivityType(aiBPMNActivityType As pcenumBPMNActivityType)

            Try
                Me.ActivityType = aiBPMNActivityType

                RaiseEvent ActivityTypeChanged(aiBPMNActivityType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetActivityMarker(aiBPMNActivityMarker As pcenumBPMNActivityMarker)

            Try
                Me.ActivityMarker = aiBPMNActivityMarker

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessActivityMarker(Me, aiBPMNActivityMarker)

                RaiseEvent ActivityMarkerChanged(aiBPMNActivityMarker)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetActivityTaskType(aiBPMNActivityTaskType As pcenumBPMNActivityTaskType)

            Try
                Me.ActivityTaskType = aiBPMNActivityTaskType

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessActivityTaskType(Me, aiBPMNActivityTaskType)

                RaiseEvent ActivityTaskTypeChanged(aiBPMNActivityTaskType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetConversationType(aiBPMNConversationType As pcenumBPMNConversationType)

            Try
                Me.ConversationType = aiBPMNConversationType

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessConversationType(Me, aiBPMNConversationType)

                RaiseEvent ConversationTypeChanged(aiBPMNConversationType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetEventPosition(aiBPMNEventPosition As pcenumBPMNEventPosition)

            Try
                Me.EventPosition = aiBPMNEventPosition

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessEventPosition(Me, aiBPMNEventPosition)

                RaiseEvent EventPositionChanged(aiBPMNEventPosition)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetEventType(aiBPMNEventType As pcenumBPMNEventType)

            Try
                Me.EventType = aiBPMNEventType

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessEventType(Me, aiBPMNEventType)

                RaiseEvent EventTypeChanged(aiBPMNEventType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetEventSubType(aiBPMNEventSubType As pcenumBPMNEventSubType)

            Try
                Me.EventSubType = aiBPMNEventSubType

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessEventSubType(Me, aiBPMNEventSubType)

                RaiseEvent EventSubTypeChanged(aiBPMNEventSubType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetGatewayType(aiBPMNGatewayType As pcenumBPMNGatewayType)

            Try
                Me.GatewayType = aiBPMNGatewayType

                'CMML
                Call Me.CMMLModel.Model.updateCMMLProcessGatewayType(Me, aiBPMNGatewayType)

                RaiseEvent GatewayTypeChanged(aiBPMNGatewayType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


    End Class

End Namespace