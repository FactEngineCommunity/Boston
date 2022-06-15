Namespace UML
    <Serializable()>
    Public Class ActorName
        Inherits FBM.PageObject

        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.ActorName

        Public Actor As UML.Actor

        ''' <summary>
        ''' Parameterless constructor.
        ''' </summary>
        Sub New()
        End Sub

        Public Sub New(ByRef arActor As UML.Actor)
            Me.Actor = arActor
        End Sub

    End Class

End Namespace