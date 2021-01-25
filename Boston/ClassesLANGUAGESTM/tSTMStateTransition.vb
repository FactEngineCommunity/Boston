
Namespace FBM.STM

    Public Class StateTransition

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

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arValueType As FBM.ValueType,
                        ByRef arFromState As FBM.STM.State,
                        ByRef arToState As FBM.STM.State,
                        ByVal asEvent As String,
                        Optional ByRef arFact As FBM.Fact = Nothing)

            Me.ValueType = arValueType
            Me.FromState = arFromState
            Me.ToState = arToState
            Me.Event = asEvent

            If arFact IsNot Nothing Then
                Me.Fact = arFact
                Me.Id = arFact.Id
            End If

        End Sub

    End Class

End Namespace
