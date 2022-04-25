Imports System.Reflection

Partial Public Class tBrain

#Region "RESPONCEPROCESSING"

    Private Sub ResponceChecking()

        Try
            '-------------------------------------
            'Do processing for question response
            '-------------------------------------
            If Me.CurrentQuestion Is Nothing Then
                Exit Sub
            End If
            If Me.CurrentQuestion.ExpectingYesNoResponse Then
                Select Case LCase(Me.InputBuffer)
                    Case Is = "yes", "yes please"
                        '---------------------------------------------------------------
                        'Step out of the Brain and work on the WorkingModel/WorkingPage
                        '---------------------------------------------------------------
                        If Me.CurrentSentence IsNot Nothing Then
                            Me.CurrentSentence.SentenceType.Add(pcenumSentenceType.Response)
                            Me.CurrentSentence.SentenceType.Remove(pcenumSentenceType.Unknown)
                        End If

                        Select Case Me.CurrentQuestion.QuestionType
                            Case Is = pcenumQuestionType.CreateConcept
                                Dim lrConcept As New FBM.Concept(Me.CurrentQuestion.Symbol(0))
                                Me.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.GeneralConcept))

                            Case Is = pcenumQuestionType.CopyFactType
                                Call Me.ProcessStatementCopyFactType()

                            Case Is = pcenumQuestionType.CreateFactTypePredetermined
                                Call Me.executeStatementAddFactTypePredetermined(Me.CurrentQuestion)

                            Case Is = pcenumQuestionType.CreateEntityType
                                Call Me.ProcessStatementAddEntityType()

                            Case Is = pcenumQuestionType.CreateValueType
                                Call Me.ProcessStatementAddValueType(Me.CurrentQuestion)

                            Case Is = pcenumQuestionType.CreateFactType
                                If Me.executeStatementAddFactType() Then
                                    Call Me.ProcessedSentences.Add(Me.CurrentQuestion.sentence)
                                    Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)
                                End If

                            Case Is = pcenumQuestionType.CreateFactTypeReading
                                If Me.ProcessStatementAddFactTypeReading() Then
                                    Call Me.ProcessedSentences.Add(Me.CurrentQuestion.sentence)
                                    Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)
                                End If

                            Case Is = pcenumQuestionType.CreateSubtypeRelationship
                                Call Me.ProcessStatementCreateSubtypeRelationship(Me.CurrentQuestion)

                            Case Is = pcenumQuestionType.CheckWordTypeVerb
                                Dim lsORMQLQuery As String = ""

                            'lsORMQLQuery = "INSERT INTO IsVerb (Word) IN MODEL 'English' VALUES ('" & Me.CurrentQuestion.FocalSymbol & "')"
                            'Me.model.process_ORMQL_statement(lsORMQLQuery)

                            Case Is = pcenumQuestionType.CheckWordTypeNoun
                                Dim lsORMQLQuery As String = ""

                            'lsORMQLQuery = "INSERT INTO IsNoun (Word) IN MODEL 'English' VALUES ('" & Me.CurrentQuestion.FocalSymbol & "')"
                            'Me.model.process_ORMQL_statement(lsORMQLQuery)
                            Case Is = pcenumQuestionType.ForgetAskedToAbortPlan

                                Me.CurrentSentence = Me.CurrentQuestion.sentence
                                Call Me.AnalyseCurrentSentence()

                            Case Is = pcenumQuestionType.OpenHelpFile
                                Me.HelpProvider.HelpNamespace = My.Settings.HelpfileLocation
                                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, Me.CurrentQuestion.GeneralText)
                        End Select

                        Call Me.CurrentQuestionAnswered()
                        Call Me.send_data("Okay")
                    Case Is = "no"
                        If Me.CurrentSentence IsNot Nothing Then
                            Me.CurrentSentence.SentenceType.Add(pcenumSentenceType.Response)
                            Me.CurrentSentence.SentenceType.Remove(pcenumSentenceType.Unknown)
                        End If
                        Select Case Me.CurrentQuestion.QuestionType
                            Case Is = pcenumQuestionType.CreateValueType
                                Me.CurrentSentence = Me.CurrentQuestion.sentence
                                If Me.CurrentQuestion.PlanStep.AlternateActionType <> pcenumActionType.None Then
                                    If Me.CurrentQuestion.PlanStep.AlternateActionType = pcenumActionType.CreateEntityType Then
                                        '---------------------------------
                                        'Maybe the word is an EntityType
                                        '---------------------------------
                                        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, Me.CurrentQuestion.ValueType(0).Id, True)
                                        Me.CurrentQuestion.PlanStep.StepStatus = pcenumBrainPlanStepStatus.Unresolved
                                        Me.CurrentQuestion.PlanStep.AlternateActionType = pcenumActionType.None
                                        Call Me.AskQuestionCreateEntityType(lrEntityType, Me.CurrentQuestion.sentence, True)
                                    End If
                                Else
                                    Me.CurrentQuestion.PlanStep.StepStatus = pcenumBrainPlanStepStatus.Unresolved
                                    Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)

                                    If Me.CurrentQuestion.PlanStep.AbortPlanIfStepIsAborted Then
                                        Call Me.AbortCurrentPlan(True)
                                    End If
                                End If

                            Case Is = pcenumQuestionType.CreateEntityType

                                If Me.CurrentQuestion.PlanStep.AlternateActionType <> pcenumActionType.None Then
                                    If Me.CurrentQuestion.PlanStep.AlternateActionType = pcenumActionType.CreateValueType Then
                                        Dim lrValueType As New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, Me.CurrentQuestion.ModelObject(0).Id, True)
                                        Me.CurrentQuestion.PlanStep.StepStatus = pcenumBrainPlanStepStatus.Unresolved
                                        Me.CurrentQuestion.PlanStep.AlternateActionType = pcenumActionType.None
                                        Call Me.AskQuestionCreateValueType(lrValueType, Me.CurrentQuestion.sentence, True, Me.CurrentQuestion.Plan)
                                    End If
                                Else
                                    Me.CurrentQuestion.PlanStep.StepStatus = pcenumBrainPlanStepStatus.Unresolved
                                    Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)

                                    If Me.CurrentQuestion.PlanStep.AbortPlanIfStepIsAborted Then
                                        Call Me.AbortCurrentPlan(True)
                                    End If
                                End If

                            Case Is = pcenumQuestionType.CreateFactType
                                Call Me.ProcessedSentences.Add(Me.CurrentQuestion.sentence)
                                Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)

                            Case Is = pcenumQuestionType.CreateFactTypeReading
                                Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)
                                Call Me.ProcessedSentences.Add(Me.CurrentQuestion.sentence)

                                If Me.CurrentQuestion.PlanStep.AbortPlanIfStepIsAborted Then
                                    Call Me.AbortCurrentPlan(True)
                                End If

                            Case Is = pcenumQuestionType.CreateSubtypeRelationship
                                Me.CurrentQuestion.sentence.ResolutionType = pcenumSentenceResolutionType.AbortedByUser
                                Call Me.ProcessedSentences.Add(Me.CurrentQuestion.sentence)
                                Call Me.OutstandingSentences.Remove(Me.CurrentQuestion.sentence)
                            Case Is = pcenumQuestionType.CheckWordTypeVerb
                            '------------------------------------------------------
                            'The Word is not a Verb, check to see if it is a Noun
                            '------------------------------------------------------
                            'If (Not IsNoun(Me.CurrentQuestion.FocalSymbol)) Then
                            '    Dim lr_question As New tQuestion("I don't know the word/Symbol '" & Me.CurrentQuestion.FocalSymbol & "'. Is '" & Me.CurrentQuestion.FocalSymbol & "' a Noun?", pcenumQuestionType.check_word_type_noun, True, Me.CurrentQuestion.FocalSymbol, Me.sentence(0))
                            '    Me.question.Add(lr_question)
                            'End If
                            Case Is = pcenumQuestionType.CheckWordTypeNoun
                                '-----------------------------------------------------------
                                'The Word is not a Noun, check to see if it is an Adjective
                                '-----------------------------------------------------------
                                'If (Not IsAdjective(Me.currentQuestion.FocalSymbol)) Then
                                '    Dim lr_question As New tQuestion("I don't know the word/Symbol '" & Me.currentQuestion.FocalSymbol & "'. Is '" & Me.currentQuestion.FocalSymbol & "' an Adjective?", pcenumQuestionType.check_word_type_adjective, True, Me.currentQuestion.FocalSymbol, Me.sentence(0))
                                '    Me.question.Add(lr_question)
                                'End If
                        End Select
                        Me.CurrentQuestionAnswered()
                        Me.send_data("Okay")
                    Case Else
                        Me.send_data("...was expecting a 'Yes' or 'No' response.")
                        If Me.AskQuestions And Me.PressForAnswer Then
                            '------------------------
                            'Ask the Question again.
                            '------------------------
                            Me.send_data(Me.CurrentQuestion.Question)
                        Else
                            Me.send_data("Okay")
                        End If
                End Select
            End If


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

#End Region


End Class
