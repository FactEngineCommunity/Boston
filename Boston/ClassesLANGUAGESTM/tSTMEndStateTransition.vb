Imports System.Reflection
Imports System.Xml.Serialization


Namespace FBM.STM

    <Serializable()>
    Public Class EndStateTransition
        Implements IEquatable(Of FBM.STM.EndStateTransition)

        <XmlIgnore()>
        <NonSerialized()>
        Public Model As STM.Model

        Public ValueType As FBM.ValueType

        Public EndStateId As String = "" 'The Id of the EndStateIndicator for this transition.

        Public [Event] As String = ""

        Public State As STM.State

        ''' <summary>
        ''' The Fact that represents this EndStateTransition in the FBM Model. I.e. Within the Core/MDA set of tables for State Transition Modelling.
        ''' </summary>
        Public Fact As FBM.Fact

        Public Event EventNameChanged(ByVal asNewEventName As String)
        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arSTModel As STM.Model,
                       ByRef arValueType As FBM.ValueType,
                       ByRef arState As STM.State,
                       ByVal asEvent As String)

            Me.Model = arSTModel
            Me.ValueType = arValueType
            Me.State = arState
            Me.Event = asEvent

        End Sub

        Public Shadows Function Equals(other As EndStateTransition) As Boolean Implements IEquatable(Of EndStateTransition).Equals

            Return (Me.ValueType.Id = other.ValueType.Id) And (Me.State.Name = other.State.Name) And (Me.Event = other.Event) And (Me.EndStateId = other.EndStateId)

        End Function

        Public Function EqualsByFact(other As STM.EndStateTransition) As Boolean

            Return Me.Fact.Id = other.Fact.Id
        End Function

        Public Shadows Function RemoveFromModel() As Boolean

            Call Me.Model.removeEndStateTransition(Me)

            RaiseEvent RemovedFromModel()

            Return True

        End Function

        Public Sub setEventName(ByVal asEventName As String)

            Me.Event = asEventName

            Me.Fact("Event").Data = asEventName
            Me.Fact.makeDirty()

            RaiseEvent EventNameChanged(asEventName)
        End Sub

    End Class

End Namespace
