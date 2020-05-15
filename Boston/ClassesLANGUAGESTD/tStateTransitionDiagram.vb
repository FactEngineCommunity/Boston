Public Class tStateTransitionDiagram

    Public ValueTypeId As String
    Public State As New List(Of CMML.State)
    Public StateTransition As New List(Of CMML.StateTransition)

    Public Process As New List(Of CMML.Process)
    Public StateTransitionRelation As FBM.FactTypeInstance
    Public StateTransitionValueType As FBM.FactTypeInstance

End Class
