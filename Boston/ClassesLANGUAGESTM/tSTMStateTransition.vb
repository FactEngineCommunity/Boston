Imports System.Reflection

Namespace FBM.STM

    Public Class StateTransition

        Public Model As FBM.STM.Model

        ''' <summary>
        ''' Is the Id of the Fact (in the FBM/Core model) that is the StateTransition.
        ''' </summary>
        Public Id As String = ""

        Public FromState As FBM.STM.State = Nothing

        Public ToState As FBM.STM.State = Nothing

        Public [Event] As String = ""

        Public ValueType As FBM.ValueType = Nothing

        ''' <summary>
        ''' The Fact that represents this StateTransition in the FBM Model. I.e. Within the Core/MDA set of tables for State Transition Modelling.
        ''' </summary>
        Public Fact As FBM.Fact

        Public Event EventNameChanged(ByVal asNewEventName As String)
        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.STM.Model,
                       ByRef arValueType As FBM.ValueType,
                       ByRef arFromState As FBM.STM.State,
                       ByRef arToState As FBM.STM.State,
                       ByVal asEvent As String,
                       Optional ByRef arFact As FBM.Fact = Nothing)

            Me.Model = arModel
            Me.ValueType = arValueType
            Me.FromState = arFromState
            Me.ToState = arToState
            Me.Event = asEvent

            If arFact IsNot Nothing Then
                Me.Fact = arFact
                Me.Id = arFact.Id
            End If

        End Sub

        Public Sub removeFromModel()

            Try
                Call Me.Model.removeStateTransition(Me)

                RaiseEvent RemovedFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setEventName(ByVal asEventName As String)

            Me.Event = asEventName

            Me.Fact("Event").Data = asEventName
            Me.Fact.makeDirty()

            RaiseEvent EventNameChanged(asEventName)
        End Sub

    End Class

End Namespace
