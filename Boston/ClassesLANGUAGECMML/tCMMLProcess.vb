Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace CMML
    <Serializable()>
    Public Class Process

        Public Model As CMML.Model

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public Id As String

        ''' <summary>
        ''' The text of the Process
        ''' </summary>
        Public Text As String

        Public Name As String

        Public include_process As List(Of CMML.Process)
        Public included_by_process As List(Of CMML.Process)
        Public extend_to_process As List(Of CMML.Process)
        Public extended_by_process As List(Of CMML.Process)

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

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arModel As CMML.Model, ByVal asProcessId As String, ByVal asProcessText As String)

            Call MyBase.New

            Me.Id = asProcessId
            Me.Model = arModel
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

        Public Sub SetProcessText(ByVal asNewProcessText As String)

            Try
                Me.Text = asNewProcessText

                'CMML
                Call Me.Model.Model.updateCMMLProcessText(Me)

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