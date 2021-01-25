
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

    End Class

End Namespace
