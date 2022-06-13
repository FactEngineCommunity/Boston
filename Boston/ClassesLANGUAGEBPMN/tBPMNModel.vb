Namespace BPMN
    Public Class Model
        'Inherits FBM.Model '20220528-VM-Was. Commented out. Can't work out why it would be there.

        Public Actor As New List(Of CMML.Actor)
        Public Process As New List(Of CMML.Process)
        Public ActorToProcessParticipationRelation As FBM.FactTypeInstance
        Public PocessToProcessParticipationRelation As FBM.FactTypeInstance

        Public Sub New()

            Call MyBase.New()

        End Sub

    End Class
End Namespace
