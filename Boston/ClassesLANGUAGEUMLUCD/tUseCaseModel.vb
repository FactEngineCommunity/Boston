Public Class tUseCaseModel
    Inherits FBM.Model

    Public Actor As New List(Of CMML.tActor)
    Public Process As New List(Of CMML.Process)
    Public ActorToProcessParticipationRelation As FBM.FactTypeInstance
    Public PocessToProcessRelation As FBM.FactTypeInstance

End Class
