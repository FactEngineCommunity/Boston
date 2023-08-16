Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports Boston.FBM

Namespace BPMN
    <Serializable()>
    Public Class Process
        Inherits UML.Process
        Implements FBM.iPageObject
        Implements BPMN.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public Shadows WithEvents CMMLProcess As CMML.Process

        Public TaskTypeShape As ShapeNode

        <XmlIgnore()>
        <CategoryAttribute("Process"),
        Browsable(True),
        [ReadOnly](True),
        DescriptionAttribute("The unique Process Id of the Process.")>
        Public Overloads ReadOnly Property ProcessId As String
            Get
                Return Me.Id
            End Get
        End Property

        Private _IsCreatingLink As Boolean = False

        Public Property IsCreatingLink As Boolean Implements iPageObject.IsCreatingLink
            Get
                Return Me._IsCreatingLink
            End Get
            Set(value As Boolean)
                Me._IsCreatingLink = value
            End Set
        End Property

        Public ReadOnly Property BPMNPage As Page Implements iPageObject.BPMNPage
            Get
                Return Me.Page
            End Get
        End Property

        Public Shadows Shape As BPMN.ShapeNode

        '20220617-VM-Remove if not needed.
        '''' <summary>
        '''' The text of the Process
        '''' </summary>
        'Public Text As String

        'Public Shadows Page As FBM.Page

        'Public include_process As List(Of CMML.Process)
        'Public included_by_process As List(Of CMML.Process)
        'Public extend_to_process As List(Of CMML.Process)
        'Public extended_by_process As List(Of CMML.Process)

        '''' <summary>
        '''' The SequenceNr assigned to the Process in a sequence of Processes in (say) a FlowChart or EventTraceDiagram
        '''' </summary>
        '''' <remarks></remarks>
        'Public SequenceNr As Single = 1

        'Public Shadows IsDecision As Boolean = False
        'Public IsStart As Boolean = False
        'Public IsStop As Boolean = False

        '''' <summary>
        '''' The Actor responsible for the process, as in (say) an EventTraceDiagram
        '''' </summary>
        '''' <remarks></remarks>
        'Public ResponsibleActor As CMML.Actor

        '20220617-VM-Not really used.
        '<CategoryAttribute("Process"),
        '     DefaultValueAttribute(GetType(String), ""),
        '     DescriptionAttribute("Name of the Process.")>
        'Public Property ProcessName() As String
        '    Get
        '        Return Me.Name
        '    End Get
        '    Set(ByVal Value As String)
        '        Me.Name = Value
        '    End Set
        'End Property

        Public Shadows Event FactChanged(ByRef arFact As FBM.Fact)

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asGUID As String, ByVal asProcessId As String, ByVal asProcessText As String)
            Call MyBase.New

            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.Text = asProcessText

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Me.Page = arPage
            Me.Role = arRoleInstance
            Me.Concept = ar_concept

            '------------------------------------
            'link the RoleData back to the Model
            '------------------------------------
            Dim lrRole As FBM.Role = arRoleInstance.Role
            Dim lrRole_data As New FBM.FactData(arRoleInstance.Role, ar_concept)
            lrRole_data = lrRole.Data.Find(AddressOf lrRole_data.Equals)
            Me.FactData = lrRole_data

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept, ByVal aiX As Integer, ByVal aiY As Integer)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Call Me.New(arPage, arRoleInstance, ar_concept)
            Me.X = aiX
            Me.Y = aiY

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

        Public Overrides Sub DisplayAndAssociate(Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Dim loDroppedNode As New ShapeNode

            Try
                '--------------------------------------------------------------------
                'Create a Shape for the EntityTypeInstance on the DiagramView object
                '--------------------------------------------------------------------            
                loDroppedNode = New BPMN.ShapeNode 'Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 20, 15)                
                loDroppedNode.SetRect(New RectangleF(0, 0, 20, 15), False) '20220624-VM-New
                loDroppedNode.Move(Me.X, Me.Y)
                loDroppedNode.TextFormat = New StringFormat(StringFormatFlags.NoFontFallback)
                loDroppedNode.TextFormat.Alignment = StringAlignment.Center
                loDroppedNode.TextFormat.LineAlignment = StringAlignment.Center
                Me.Page.Diagram.Nodes.Add(loDroppedNode)  '20220624-VM-New
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove ''HatchHandles3 is a very professional look, or SquareHandles2
                loDroppedNode.ToolTip = "process"
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.ShadowColor = Color.White
                loDroppedNode.Text = Me.Text 'arProcessInstance.Name            
                loDroppedNode.Tag = New FBM.EntityTypeInstance 'loDroppedNode.Tag = New Object
                loDroppedNode.Tag = Me
                loDroppedNode.Obstacle = True
                loDroppedNode.Font = New Font("Arial", 7)
                loDroppedNode.Pen.Width = 5
                Me.Shape = loDroppedNode
                Me.Shape.BPMNElement = Me  '20220624-VM-New                

                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.BPMNCollaborationDiagram
                        Select Case Me.CMMLProcess.ProcessType
                            Case Is = pcenumBPMNProcessType.Event
#Region "Event"
                                loDroppedNode.Shape = Shapes.Ellipse
                                loDroppedNode.Resize(12, 12)
                                loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                                loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                                loDroppedNode.Text = ""
                                loDroppedNode.Pen.Width = 0.0
                                loDroppedNode.ImageAlign = MindFusion.Drawing.ImageAlign.BottomCenter

                                Select Case Me.CMMLProcess.EventPosition
#Region "Position of Event"
                                    Case Is = pcenumBPMNEventPosition.End
#Region "End - EventPosition"
                                        Select Case Me.CMMLProcess.EventType
#Region "Event Type"
                                            Case Is = pcenumBPMNEventType.None
#Region "None - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_None_Standard
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"

#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Cancel
#Region "Cancel - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Cancel_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Compensation
#Region "Compensation - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Compensation_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Error
#Region "Error - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Error_Standard
#End Region

                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Escalation
#Region "Escaltion - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Escalation_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Link
#Region "Link - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Link_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Message
#Region "Message - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Message_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Multiple
#Region "Multiple - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Multiple_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Signal
#Region "Signal - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Signal_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Terminate
#Region "Terminate - Event Type"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_End_Terminate_Standard
#End Region
                                                End Select
#End Region
                                        End Select
#End Region 'Event Type

#End Region
                                    Case Is = pcenumBPMNEventPosition.Intermediate
#Region "Intermediate - EventPosition"
                                        Select Case Me.CMMLProcess.EventType
#Region "EventType"
                                            Case Is = pcenumBPMNEventType.None
#Region "None - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_None_Throwing
#End Region
                                                End Select
#End Region
#End Region
                                            Case Is = pcenumBPMNEventType.Cancel
#Region "Cancel - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Cancel_BoundaryInterupting
#End Region
                                                End Select
#End Region
#End Region
                                            Case Is = pcenumBPMNEventType.Compensation
#Region "Compensation - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Compensation_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "SUbprocess Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Compensation_SubprocessInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Compensation_Throwing
#End Region
                                                End Select
#End Region
#End Region
                                            Case Is = pcenumBPMNEventType.Conditional
#Region "Conditional - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Conditional_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Conditional_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Conditional_Catching
#End Region
                                                End Select
#End Region
#End Region
                                            Case Is = pcenumBPMNEventType.Error
#Region "Error - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Error_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Error_SubprocessInterupting
#End Region
                                                End Select
#End Region
#End Region
                                            Case Is = pcenumBPMNEventType.Escalation
#Region "Escalation - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Escalation_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Escalation_SubprocessNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Escalation_Throwing
#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Link
#Region "Link - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Link_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Link_Throwing
#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Message
#Region "Message - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Message_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Message_BoundaryNonInterupting___Copy
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Message_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"


                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Message_SubprocessNonInterupting





#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Message_Throwing
#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Multiple
#Region "Multiple - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"


                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryInterupting





#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Multiple_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Multiple_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Multiple_SubprocessNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Multiple_Throwing
#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.ParallelMultiple
#Region "ParallelMultiple - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_ParallelMultiple_SubprocessNonInterupting
#End Region

#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Signal
#Region "Signal - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_SubprocessInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_Subprocess_NonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Throwing
#Region "Throwing"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Signal_Throwing
#End Region
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Timer
#Region "Timer - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
#Region "SubType of Event"
                                                    Case Is = pcenumBPMNEventSubType.BoundaryInterupting
#Region "Boundary Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Timer_BoundaryInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.BoundaryNonInterupting
#Region "Boundary NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Timer_BoundaryNonInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.Catching
#Region "Catching"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Timer_Catching
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Timer_SubprocessInterupting
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessNonInterupting
#Region "Subprocess NonInterupting"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Intermediate_Timer_SubprocessNonInterupting
#End Region
#End Region
                                                End Select
#End Region
                                        End Select
#End Region
#End Region
                                    Case Is = pcenumBPMNEventPosition.Start
#Region "Start - Event Position"
                                        Select Case Me.CMMLProcess.EventType
#Region "Event Type"
                                            Case Is = pcenumBPMNEventType.None
#Region "None - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_None_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Conditional
#Region "Conditional - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Conditional_Standard
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "SubprocessInterupting - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Conditional_SubprocessInterupting
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Message
#Region "Message - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Message_Standard
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Message_SubprocessInterupting
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Multiple
#Region "Multiple - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Multiple_Standard
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Multiple_SubprocessInterupting
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.ParallelMultiple
#Region "ParallelMultiple - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_ParallelMultiple_Standard
#End Region
                                                    Case Is = pcenumBPMNEventSubType.SubprocessInterupting
#Region "Subprocess Interupting - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_ParallelMultiple_SubprocessInterupting
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Signal
#Region "Signal - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Signal_Standard
#End Region
                                                End Select
#End Region
                                            Case Is = pcenumBPMNEventType.Timer
#Region "Timer - EventType"
                                                Select Case Me.CMMLProcess.EventSubType
                                                    Case Is = pcenumBPMNEventSubType.Standard
#Region "Standard - Event SubType"
                                                        loDroppedNode.Image = My.Resources.BPMN.Event_Start_Timer_Standard
#End Region
                                                End Select
#End Region
#End Region
                                        End Select
#End Region
                                End Select

#End Region 'Event Position

#End Region 'Event

                            Case Is = pcenumBPMNProcessType.Gateway
#Region "Gateway"
                                loDroppedNode.Shape = Shapes.Ellipse
                                loDroppedNode.Resize(12, 12)
                                loDroppedNode.Image = My.Resources.BPMN.Gateway_Exclusive
                                loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                                loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                                loDroppedNode.Text = ""
                                loDroppedNode.Pen.Width = 0.0
#End Region
                            Case Else
#Region "Activity/Else"
                                loDroppedNode.Shape = Shapes.RoundRect
                                loDroppedNode.Resize(20, 15)
                                loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                                loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.Black)
                                loDroppedNode.Pen.Width = 0.5

                                Dim loTaskTypeNode = New BPMN.ShapeNode 'Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 20, 15)                
                                loTaskTypeNode.SetRect(New RectangleF(0, 0, 4, 4), False) '20220624-VM-New
                                loTaskTypeNode.Move(Me.X + 1, Me.Y + 1)
                                loTaskTypeNode.Pen = New MindFusion.Drawing.Pen(Color.White, 0.0002)
                                loTaskTypeNode.Transparent = True
                                loTaskTypeNode.Visible = False
                                loTaskTypeNode.Locked = True
                                Me.Page.Diagram.Nodes.Add(loTaskTypeNode)
                                loTaskTypeNode.AttachTo(loDroppedNode, AttachToNode.TopCenter)
                                loTaskTypeNode.ImageAlign = MindFusion.Drawing.ImageAlign.Fit
                                Me.TaskTypeShape = loTaskTypeNode
#End Region
                        End Select

                    Case Is = pcenumLanguage.ORMModel
                        'Can delete this later. At present, if the MetaModel is shown as an ORM diagram, the Page.Language changes to ORMModel
                        ' then if you drop a Process on the Page, the Process has no default shape
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.Resize(20, 15)
                        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.Beige)
                    Case Is = pcenumLanguage.DataFlowDiagram
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.Resize(20, 15)
                        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.Beige)
                    Case Is = pcenumLanguage.UMLUseCaseDiagram
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                        loDroppedNode.Resize(40, 12)
                    Case Is = pcenumLanguage.FlowChart
                        If Me.IsDecision Then
                            loDroppedNode.Shape = Shapes.Decision
                        ElseIf Me.IsStart Then
                            loDroppedNode.Shape = Shapes.Ellipse
                        ElseIf Me.IsStop Then
                            loDroppedNode.Shape = Shapes.Ellipse
                        End If
                        loDroppedNode.Resize(20, 15)
                End Select



#Region "Snap to Grid"
                Me.Page.Diagram.AlignToGrid = True
                Dim ptCenter As PointF = loDroppedNode.GetCenter()
                Dim ptAlignedCenter As PointF = Me.Page.Diagram.AlignPointToGrid(ptCenter)
                Dim fDeltaX As Double = ptAlignedCenter.X - ptCenter.X
                Dim fDeltaY As Double = ptAlignedCenter.Y - ptCenter.Y
                Me.Page.Diagram.AlignToGrid = False

                Dim rectBounds As RectangleF = loDroppedNode.Bounds
                rectBounds.Offset(fDeltaX, fDeltaY)
                loDroppedNode.Bounds = rectBounds
#End Region

                If IsSomething(aoContainerNode) Then
                    aoContainerNode.Add(loDroppedNode)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean,
                                    Optional ByVal abMakeDirty As Boolean = True) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Me.FactDataInstance.X = aiNewX
            Me.FactDataInstance.Y = aiNewY

            Me.FactDataInstance.Fact.FactType.isDirty = True
            Me.FactDataInstance.Fact.isDirty = True
            Me.FactDataInstance.isDirty = True

            Try
                Me.FactDataInstance.Page.MakeDirty()
            Catch ex As Exception

            End Try

        End Sub

        Public Overrides Function RemoveFromPage() As Boolean

            Try
                Dim lsSQLQuery As String

                '----------------------------------------------------------------------------------------------------------
                'Remove the Process that represents the Process from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Page.UMLDiagram.Process.Remove(Me)

                Dim larLinkToRemove As New List(Of DiagramLink)

                If Me.Page.Diagram IsNot Nothing Then

                    Me.Page.Diagram.Nodes.Remove(Me.Shape)

                    For Each lrLink In Me.Shape.IncomingLinks
                        larLinkToRemove.Add(lrLink)
                    Next

                    For Each lrLink In Me.Shape.OutgoingLinks
                        larLinkToRemove.Add(lrLink)
                    Next

                    For Each lrLink In larLinkToRemove
                        Me.Page.Diagram.Links.Remove(lrLink)
                    Next
                End If

                '-------------------------------------------------------------------------
                'Remove the Process from the Page
                '---------------------------------
#Region "CMML"
                'Likely already deleted when deleted at the Model level.
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & Me.Id & "'"
                lsSQLQuery &= "   AND ElementType = 'Process'"

                Call Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function


        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                          Optional ByVal asSelectedGridItemLabel As String = "")

            Dim lsMessage As String = ""

            Try
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Text"

                            If Me.Model.UML.Process.Find(Function(x) x.Id <> Me.Id And Trim(x.Text) = Trim(Me.Text)) IsNot Nothing Then
                                lsMessage = "You cannot set the name of a Process as the same as the name of another Process in the model."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "A Process with the tex, '" & Me.Text & "', already exists in the model."

                                Me.Text = Me.CMMLProcess.Text

                                MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                            Else
                                Call Me.CMMLProcess.SetProcessText(Me.Text)
                            End If
                    End Select
                End If

                If Me.Shape IsNot Nothing Then
                    Me.Shape.Text = Me.Text
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Shadows Sub SetProcessText(ByVal asNewProcessText As String)

            Try
                Me.Text = asNewProcessText

                Call Me.CMMLProcess.SetProcessText(asNewProcessText)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the size of the Process proportional to the number of incomming Links.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetSizeProportionalToInputs(Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Select Case Me.Shape.IncomingLinks.Count
                Case Is < 2
                    Me.Shape.Resize(20, 15)
                Case 3 To 5
                    Me.Shape.Resize(30, 20)
                Case 6 To 10
                    Me.Shape.Resize(40, 30)
                Case Is >= 10
                    Me.Shape.Resize(60, 60)
            End Select

            If IsSomething(aoContainerNode) Then
                aoContainerNode.AutoShrink = True
                aoContainerNode.Resize(Viev.Greater(Me.Shape.Bounds.Width + 20, aoContainerNode.Bounds.Width), Viev.Greater(Me.Shape.Bounds.Height + 20, aoContainerNode.Bounds.Height))
            End If

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated
            Try
                Call Me.UpdateGUIFromModel()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Friend Shadows Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.Shape) Then
                        If Me.Shape.Text <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.Shape.Text = Trim(Me.FactData.Data)
                            Call Me.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
                        End If
                    End If
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setFact(ByRef arFact As FBM.Fact)

            Try

                Me.Fact = arFact

                RaiseEvent FactChanged(arFact)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Overloads Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

            Try
                If IsSomething(Me.Shape) Then
                    If Me.Shape.Selected Then
                        Me.Shape.Pen.Color = Color.Blue
                    Else
                        Me.Shape.Pen.Color = Color.Black
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub CMMLProcess_RemovedFromModel() Handles CMMLProcess.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub CMMLProcess_TextChanged(asNewText As String) Handles CMMLProcess.TextChanged

            Try
                Me.Text = asNewText

                Call Me.RefreshShape()

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