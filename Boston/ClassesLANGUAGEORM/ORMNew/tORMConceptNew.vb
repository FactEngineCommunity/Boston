Imports System.Xml.Serialization

Namespace FBM

    <Serializable()> _
    Public Class Concept
        Inherits Viev.FBM.Concept

        Public Overrides Sub Save()

            If TableConcept.ExistsConcept(Me) Then
                'TableConcept.UpdateConcept(Me)
                '20150106-Can't update the primary-key of the Concept because of the ReferentialIntegrity constraint on MetaModelModelDictionary to MetaModelConcept.
            Else
                TableConcept.AddConcept(Me)
            End If

        End Sub

    End Class

End Namespace
