Public Class tKLProofStep

    Public StepNr As Integer = 0
    Public ProofRule As pcenumKLProofRule
    Public ModelObject As FBM.ModelObject

    Sub New()

    End Sub

    Sub New(ByVal aiStep_nr As Integer, ByVal aiProof_rule As pcenumKLProofRule, Optional ByVal ao_model_object As FBM.ModelObject = Nothing)

        Me.StepNr = aiStep_nr
        Me.ProofRule = aiProof_rule
        If ao_model_object IsNot Nothing Then
            Me.ModelObject = ao_model_object
        End If

    End Sub

End Class
