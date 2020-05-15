Namespace Language

    Module tLanguageAnalysis

        ''' <summary>
        ''' NB If a Word is an EntityType or ValueType in the Model, but the case of a letter in the word is wrong, this method will correct the case.
        ''' </summary>
        ''' <param name="arSentence"></param>
        ''' <param name="arModel"></param>
        ''' <remarks></remarks>
        Public Sub AnalyseSentence(ByRef arSentence As Language.Sentence, Optional ByRef arModel As FBM.Model = Nothing)

            Dim lrWordQualification As Language.WordQualification
            '-------------------------------------------------------------------------------------
            'Check to see if all the words in the sentence are known within the Language ontology
            '-------------------------------------------------------------------------------------
            For Each lrWordQualification In arSentence.WordListQualification

                If lrWordQualification.Word.Contains("'") Then
                    '-----------------------------------
                    'Filter out words that won't parse
                    '-----------------------------------
                    lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Unknown))
                Else
                    If prApplication.Language.WordIsNoun(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Noun))
                    End If

                    'Pronoun 'e.g. Peter

                    If prApplication.Language.WordIsVerb(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Verb))
                    End If

                    If prApplication.Language.WordIsAdjective(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Adjective))
                    End If

                    If prApplication.Language.WordIsAdverb(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Adverb))
                    End If

                    If prApplication.Language.WordIsArticle(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Article))
                    End If

                    If prApplication.Language.WordIsPreposition(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Preposition))
                    End If

                    If prApplication.Language.WordIsConjunction(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Conjunction))
                    End If

                    If prApplication.Language.WordIsDelimeter(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Delimiter))
                    End If

                    If prApplication.Language.WordIsAlternativeAdditiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.AlternativeAdditiveDeterminer))
                    End If

                    If prApplication.Language.WordIsCardinalNumber(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.CardinalNumber))
                    End If

                    If prApplication.Language.WordIsDegreeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.DegreeDeterminer))
                    End If

                    If prApplication.Language.WordIsDemonstrativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.DemonstrativeDeterminer))
                    End If

                    If prApplication.Language.WordIsDisjunctiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.DisjunctiveDeterminer))
                    End If

                    If prApplication.Language.WordIsDistributiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.DistributiveDeterminer))
                    End If

                    If prApplication.Language.WordIsElectiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.ElectiveDeterminer))
                    End If

                    If prApplication.Language.WordIsEqualitiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.EqualitiveDeterminer))
                    End If

                    If prApplication.Language.WordIsEvaluativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.EvaluativeDeterminer))
                    End If

                    If prApplication.Language.WordIsExclamativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.ExclamativeDeterminer))
                    End If

                    If prApplication.Language.WordIsExistentialDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.ExistentialDeterminer))
                    End If

                    If prApplication.Language.WordIsInterrogativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.InterrogativeDeterminer))
                    End If

                    If prApplication.Language.WordIsNegativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.NegativeDeterminer))
                    End If

                    If prApplication.Language.WordIsPersonalDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.PersonalDeterminer))
                    End If

                    If prApplication.Language.WordIsPositiveMultalDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.PostiveMultalDeterminer))
                    End If

                    If prApplication.Language.WordIsPositivePaucal(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.PositivePaucal))
                    End If

                    If prApplication.Language.WordIsPossessiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.PossessiveDeterminer))
                    End If

                    If prApplication.Language.WordIsQualitativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.QualitativeDeterminer))
                    End If

                    If prApplication.Language.WordIsQuantifier(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.Quantifier))
                    End If

                    If prApplication.Language.WordIsRelativeDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.RelativeDeterminer))
                    End If

                    If prApplication.Language.WordIsSufficiencyDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.SufficiencyDeterminer))
                    End If

                    If prApplication.Language.WordIsUniquitiveDeterminer(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.UniquitiveDeterminer))
                    End If

                    If prApplication.Language.WordIsSubordinateConjunction(lrWordQualification.Word) Then
                        lrWordQualification.Sense.Add(New Language.LanguageWordSenseWeighting(pcenumWordSense.SubordinateConjunction))
                    End If

                    'UniversalDeterminer

                    '------------------------------------------------------------------------------
                    'Check if the World is a ModelElement within the Model.
                    '-------------------------------------------------------
                    Dim lsActualModelElementName As String = ""

                    If IsSomething(arModel) Then
                        Dim lrValueType As New FBM.ValueType(arModel, pcenumLanguage.ORMModel, lrWordQualification.Word, True)
                        Dim lrNoun As New Language.LanguageWordSenseWeighting(pcenumWordSense.Noun)
                        If arModel.ValueType.Contains(lrValueType) Then
                            If lrWordQualification.Sense.Contains(lrNoun) Then
                            Else
                                lrWordQualification.Sense.Add(lrNoun)
                            End If
                        ElseIf arModel.GetConceptTypeByNameFuzzy(lrWordQualification.Word, lsActualModelElementName) = pcenumConceptType.ValueType Then
                            lrWordQualification.Word = lsActualModelElementName
                            lrWordQualification.Sense.Add(lrNoun)
                        End If
                    End If

                    If IsSomething(arModel) Then
                        Dim lrEntityType As New FBM.EntityType(arModel, pcenumLanguage.ORMModel, lrWordQualification.Word, Nothing, True)
                        Dim lrNoun As New Language.LanguageWordSenseWeighting(pcenumWordSense.Noun)
                        If arModel.EntityType.Contains(lrEntityType) Then
                            If lrWordQualification.Sense.Contains(lrNoun) Then
                            Else
                                lrWordQualification.Sense.Add(lrNoun)
                            End If
                        ElseIf arModel.GetConceptTypeByNameFuzzy(lrWordQualification.Word, lsActualModelElementName) = pcenumConceptType.EntityType Then
                            lrWordQualification.Word = lsActualModelElementName
                            lrWordQualification.Sense.Add(lrNoun)
                        End If
                    End If

                End If
            Next

        End Sub

        Public Sub ProcessSentence(ByVal arSentence As Language.Sentence)

            Dim liInd As Integer = 0
            Dim liInd2 As Integer = 0
            Dim liInd3 As Integer = 0
            Dim lrLanguagePhrase As Language.LanguagePhrase
            Dim lrTokenSequence As Language.LanguagePhraseTokenSequence

            For liInd = 0 To arSentence.WordListQualification.Count - 1

                For Each lrLanguagePhrase In prApplication.Language.LanguagePhrase

                    If lrLanguagePhrase.PhraseType = pcenumPhraseType.Resolving Then

                        If lrLanguagePhrase.TokenSequence.Count > (arSentence.WordList.Count - liInd) Then
                            '-------------------------
                            'Skip the LanguagePhrase
                            '-------------------------
                        Else

                            '------------------------------
                            'Create the SentenceSubPhrase
                            '------------------------------
                            Dim lrSentenceSubPhrase As New Language.LanguagePhrase

                            For liInd2 = 0 To lrLanguagePhrase.TokenSequence.Count - 1
                                lrSentenceSubPhrase.TokenSequence.Add(New Language.LanguagePhraseTokenSequence(arSentence.WordList(liInd + liInd2), liInd2 + 1))
                                Dim lrLanguageWordSenseWeighting As Language.LanguageWordSenseWeighting

                                For Each lrLanguageWordSenseWeighting In arSentence.WordListQualification(liInd + liInd2).Sense
                                    Dim lrLPTSWS As New Language.LanguagePhraseTokenSequenceWordSense
                                    lrLPTSWS.SequenceNr = liInd2 + 1
                                    lrLPTSWS.WordSense = lrLanguageWordSenseWeighting.Sense
                                    lrLPTSWS.Token = arSentence.WordListQualification(liInd + liInd2).Word
                                    lrSentenceSubPhrase.TokenSequence(liInd2).LPTSWS.Add(lrLPTSWS)
                                Next
                            Next

                            '--------------------------------------------------------------------------------------
                            'Check that the Tokens match between the lrSentenceSubPhrase and the lrLanguagePhrase
                            '--------------------------------------------------------------------------------------
                            If lrLanguagePhrase.EqualsByTokens(lrSentenceSubPhrase) Then
                                '-----------------------------------------------------------
                                'The Tokens match, so progress to the Senses of each Token
                                '-----------------------------------------------------------
                                Dim lbMatch As Boolean = True
                                liInd3 = 0
                                For Each lrTokenSequence In lrLanguagePhrase.TokenSequence

                                    If lrTokenSequence.Equals(lrSentenceSubPhrase.TokenSequence(liInd3)) Then
                                    Else
                                        lbMatch = False
                                        Exit For
                                    End If
                                    liInd3 += 1
                                Next
                                If lbMatch Then
                                    '--------------------------------------------------------
                                    'Have found a TokenSequenceNrWordSense set that matches
                                    '--------------------------------------------------------
                                    Dim lrResolvedLanguagePhrase As Language.LanguagePhrase
                                    lrResolvedLanguagePhrase = lrLanguagePhrase.ResolvesToPhrase
                                    liInd3 = 0
                                    For Each lrTokenSequence In lrLanguagePhrase.TokenSequence
                                        Dim liWeightingIncreaseValue As Integer
                                        Select Case lrResolvedLanguagePhrase.TokenSequence.Count
                                            Case Is >= 5
                                                liWeightingIncreaseValue = 10
                                            Case Is = 4
                                                liWeightingIncreaseValue = 1
                                            Case Is = 3
                                                liWeightingIncreaseValue = 1
                                            Case Else
                                                liWeightingIncreaseValue = 1
                                        End Select

                                        Call arSentence.WordListQualification(liInd + liInd3).UpdateWeighting(lrResolvedLanguagePhrase.TokenSequence(liInd3).LPTSWS(0), liWeightingIncreaseValue)

                                        liInd3 += 1
                                    Next

                                End If
                            End If

                        End If
                    End If
                Next

            Next

        End Sub

        Public Sub ResolveSentence(ByVal arSentence As Language.Sentence)

            Dim lrWord As Language.WordQualification
            Dim lrWordResolved As Language.WordResolved

            arSentence.POStaggingResolved = True
            arSentence.WordListResolved.Clear()

            For Each lrWord In arSentence.WordListQualification
                Dim lrWordSense As pcenumWordSense

                lrWordSense = lrWord.GetHeighestWordSense

                lrWordResolved = New Language.WordResolved(lrWord.Word, lrWordSense)

                If lrWordResolved.Sense = pcenumWordSense.Noun Then
                    lrWordResolved.Word = Viev.Strings.MakeCapCamelCase(lrWordResolved.Word)
                End If

                arSentence.WordListResolved.Add(lrWordResolved)
            Next

            arSentence.SentenceResolved = ""
            For Each lrWordResolved In arSentence.WordListResolved
                arSentence.SentenceResolved &= lrWordResolved.Word & " "
            Next
            arSentence.SentenceResolved = Trim(arSentence.SentenceResolved)
        End Sub


    End Module

End Namespace
