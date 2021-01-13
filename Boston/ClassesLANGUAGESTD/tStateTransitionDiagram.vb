
Namespace STD

    Public Class Diagram

        Public ValueType As FBM.ValueType = Nothing
        Public State As New List(Of STD.State)
        Public StateTransition As New List(Of STD.StateTransition)

        Public Process As New List(Of CMML.Process)
        Public StateTransitionRelation As FBM.FactTypeInstance
        Public StateTransitionValueType As FBM.FactTypeInstance

    End Class

End Namespace