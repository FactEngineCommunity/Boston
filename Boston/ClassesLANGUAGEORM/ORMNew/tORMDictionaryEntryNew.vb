Imports System.Xml.Serialization

Namespace FBM

    <Serializable()> _
    Public Class DictionaryEntry
        Inherits Viev.FBM.DictionaryEntry

        Public Overrides Sub Concept_Updated() Handles Concept.ConceptSymbolUpdated

            Call TableModelDictionary.ModifySymbol(Me.Model, Me, Me.Concept.Symbol, pcenumConceptType.EntityType)

            Me.Symbol = Me.Concept.Symbol

        End Sub

        Public Overloads Overrides Sub Save()

            '-------------------------------------------
            'Saves the DictionaryEntry to the database
            '-------------------------------------------
            If TableModelDictionary.ExistsModelDictionaryEntry(Me) Then
                Call TableModelDictionary.UpdateModelDictionaryEntry(Me)
            Else
                '---------------------------------------------------------
                'Make sure an entry exists in the MetaModelConcept table
                '---------------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.Symbol)
                lrConcept.Save()
                Call TableModelDictionary.AddModelDictionaryEntry(Me)
            End If

        End Sub


    End Class

End Namespace
