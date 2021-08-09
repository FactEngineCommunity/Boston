Public Class tQuestion
    Implements IEquatable(Of tQuestion)

    Public QuestionId As String = System.Guid.NewGuid.ToString
    Public Question As String = ""
    Public ExpectingYesNoResponse As Boolean = False
    Public Symbol As New List(Of String)
    Public ModelObject As New List(Of FBM.ModelObject)
    Public ValueType As New List(Of FBM.ValueType)
    Public QuestionType As pcenumQuestionType
    Public ObjectType As New FBM.ModelObject
    Public GeneralText As String = "" 'For general text that the question can use for anything (defined when the question is created).

    ''' <summary>
    ''' The Plan that this Question forms part of.
    ''' </summary>
    ''' <remarks></remarks>
    Public Plan As Brain.Plan

    Public Resolution As pcenumQuestionResolution = pcenumQuestionResolution.Unanswered
    Public AcceptedResponce As pcenumAcceptedResponce = pcenumAcceptedResponce.Unanswered

    ''' <summary>
    ''' The Step in the Plan for the Question that this Question assists in qualifying.
    '''   i.e. e.g. If a Plan has a Step that requires a Question to be asked of the user, this member (PlanStep) stores that Step,
    '''   and this Question is that Question that assists in qualifying/completing the Step.
    '''   If the Step requires an affirmative response from the user, and a negative response if given, then (generally) the Step in the Plan
    '''   fails, and the Plan may well then fail/be-aborted.
    ''' </summary>
    ''' <remarks></remarks>
    Public PlanStep As Brain.Step

    Public sentence As Language.Sentence = Nothing 'The sentence for which this Question relates. 
    'e.g. The user says something/types in something to the computer (as a Sentence), 
    'and the tBrain askes a Question about a word in the Sentence, or the entire Sentence

    ''' <summary>
    ''' Used if a reciprocal/reverse FactTypeReading needs to be made for a FactType. E.g. Contains one entry for a Binary Fact Type.
    ''' </summary>
    Public AdditionalSentence As List(Of Language.Sentence)


    Public FocalSymbol As New List(Of String)


    Public Sub New(ByVal asQuestion As String,
                   ByVal aiQuestionType As pcenumQuestionType,
                   ByVal abExpectingYesNoResponse As Boolean,
                   Optional ByVal aasSymbol As List(Of String) = Nothing,
                   Optional ByVal arSentence As Language.Sentence = Nothing,
                   Optional ByVal aoObjectType As FBM.ModelObject = Nothing,
                   Optional ByRef arPlan As Brain.Plan = Nothing,
                   Optional ByRef arStep As Brain.Step = Nothing,
                   Optional ByVal asGeneralText As String = "",
                   Optional ByVal aarAdditionalSentence As List(Of Language.Sentence) = Nothing)

        Dim lsString As String = ""

        Me.Question = asQuestion
        Me.ExpectingYesNoResponse = abExpectingYesNoResponse
        Me.QuestionType = aiQuestionType

        Select Case Me.QuestionType
            Case Is = pcenumQuestionType.CopyFactType
                Me.ObjectType = New FBM.FactType
                Me.ObjectType = aoObjectType
            Case Else
                Me.ObjectType = aoObjectType
        End Select

        For Each lsString In Me.Question.Split
            Me.Symbol.Add(lsString)
        Next

        Me.ModelObject = New List(Of FBM.ModelObject)

        If IsSomething(aasSymbol) Then
            For Each lsString In aasSymbol
                Select Case aiQuestionType
                    Case Is = pcenumQuestionType.CreateValueType
                        Dim lrDummyValueType As New FBM.ValueType(Nothing, pcenumLanguage.ORMModel, lsString, True)
                        lrDummyValueType.Id = lsString
                        Me.ValueType.Add(lrDummyValueType)
                    Case Is = pcenumQuestionType.CreateEntityType
                        Dim lrDummyEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, lsString, Nothing, True)
                        lrDummyEntityType.Id = lsString
                        Me.ModelObject.Add(lrDummyEntityType)
                End Select
            Next
        End If

        Me.FocalSymbol = aasSymbol

        If IsSomething(arSentence) Then
            Me.sentence = arSentence
        End If

        Me.Plan = arPlan
        Me.PlanStep = arStep
        Me.Plan.Step.AddUnique(arStep)
        If IsSomething(arStep) Then
            arStep.Question = Me
        End If

        Me.GeneralText = asGeneralText

        Me.AdditionalSentence = aarAdditionalSentence

    End Sub

    Public Overloads Function Equals(ByVal other As tQuestion) As Boolean Implements System.IEquatable(Of tQuestion).Equals

        Return Me.Question = other.Question

    End Function

End Class
