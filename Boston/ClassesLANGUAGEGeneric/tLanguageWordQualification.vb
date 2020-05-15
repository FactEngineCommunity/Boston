
Namespace Language

    Public Class WordQualification

        Public Word As String 'e.g. "buildingname"
        Public OriginalWord As String 'e.g. "BuildingName". Is the actual word that the user typed into the Virtual Analyst.
        'Public Sense As New List(Of pcenumWordSense)
        Public Sense As New List(Of Language.LanguageWordSenseWeighting)

        Public Sub New(ByVal asWord As String)

            Me.Word = asWord

        End Sub

        Public Sub UpdateWeighting(ByVal lrLSTSWS As Language.LanguagePhraseTokenSequenceWordSense, Optional ByVal aiWeightingIncrease As Integer = 1)

            Dim lrLanguageWordSenseWeighting As New Language.LanguageWordSenseWeighting(lrLSTSWS.WordSense)

            Me.Sense.Find(AddressOf lrLanguageWordSenseWeighting.Equals).Weighting += aiWeightingIncrease

        End Sub

        Public Function GetHeighestWordSense() As pcenumWordSense

            Dim lrLanguageWordSenseWeighting As Language.LanguageWordSenseWeighting
            Dim lrWordSense As pcenumWordSense
            Dim liHighestWeighting As Integer = 0

            lrWordSense = pcenumWordSense.Unknown

            For Each lrLanguageWordSenseWeighting In Me.Sense
                If lrLanguageWordSenseWeighting.Weighting >= liHighestWeighting Then
                    lrWordSense = lrLanguageWordSenseWeighting.Sense
                    liHighestWeighting = lrLanguageWordSenseWeighting.Weighting
                End If
            Next

            Return lrWordSense

        End Function

        Public Function HasMultipleHeighestWeightings() As Boolean

            Dim lrWeightedWordSense As Language.LanguageWordSenseWeighting            
            Dim liHighestWeighting As Integer = 0
            Dim liHighestWeightingCount As Integer = 0

            HasMultipleHeighestWeightings = False

            If Me.Sense.Count <= 1 Then
                HasMultipleHeighestWeightings = False
                Exit Function
            End If

            For Each lrWeightedWordSense In Me.Sense
                If lrWeightedWordSense.Weighting = liHighestWeighting Then
                    liHighestWeighting = lrWeightedWordSense.Weighting
                    liHighestWeightingCount += 1
                ElseIf lrWeightedWordSense.Weighting > liHighestWeighting Then
                    liHighestWeightingCount = 1
                    liHighestWeighting = lrWeightedWordSense.Weighting
                End If
            Next

            If liHighestWeightingCount > 1 Then
                HasMultipleHeighestWeightings = True
            End If

        End Function

    End Class

End Namespace
