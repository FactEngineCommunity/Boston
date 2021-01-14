
Imports Boston.FBM.STM

Namespace STD

    Public Class Diagram

        Public ValueType As FBM.ValueType = Nothing
        Public State As New List(Of STD.State)
        Public StateTransition As New List(Of STD.StateTransition)

        Public StartBubble As STD.StartStateIndicator

        Public WithEvents STM As FBM.STM.Model

        Public Process As New List(Of CMML.Process)
        Public StateTransitionRelation As FBM.FactTypeInstance
        Public StateTransitionValueType As FBM.FactTypeInstance

        Private Sub STM_EndStateTransitionAdded(ByRef arEndStateTranstion As EndStateTransition) Handles STM.EndStateTransitionAdded

        End Sub

    End Class

End Namespace