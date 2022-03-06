Namespace Ontology

    Public Class UnifiedOntologyItem

        Public UnifiedOntology As Ontology.UnifiedOntology

        Public Model As FBM.Model

        Public ModelElement As FBM.ModelObject

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arUnifiedOntology As Ontology.UnifiedOntology,
                       ByRef arModel As FBM.Model,
                       ByRef arModelElement As FBM.ModelObject)

            Me.UnifiedOntology = arUnifiedOntology
            Me.Model = arModel
            Me.ModelElement = arModelElement

        End Sub


    End Class

End Namespace
