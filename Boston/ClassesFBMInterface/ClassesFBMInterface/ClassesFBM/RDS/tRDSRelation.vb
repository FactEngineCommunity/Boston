
Namespace RDS

    Public Class Relation

        Public Id As String = ""

        Public OriginEntity As RDS.Entity
        Public OriginAttribute As New List(Of RDS.Attribute)
        Public OriginMandatory As Boolean = False
        Public OriginMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public OriginContributesToPrimaryKey As Boolean = False
        Public OriginPredicate As String = ""

        Public DestinationEntity As RDS.Entity
        Public DestinationAttribute As List(Of RDS.Attribute)
        Public DestinationMandatory As Boolean = False
        Public DestinationMultiplicty As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public DestinationPredicate As String = ""

        Public WithEvents RelationFactType As FactType

    End Class

End Namespace
