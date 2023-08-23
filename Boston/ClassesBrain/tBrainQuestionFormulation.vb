Imports System.Reflection

Partial Public Class tBrain

    ''' <summary>
    ''' Opposite of 'AT MOST ONE' Statement. E.g. MiddleName if of ANY NUMBER OF Person
    ''' </summary>
    ''' <param name="asOriginalSentence"></param>
    ''' <param name="abBroadcastInterfaceEvent"></param>
    ''' <param name="abStraightToActionProcessing"></param>
    ''' <param name="arDSCError"></param>
    Private Function FormulateQuestionsANYNUMBEROFStatement(ByVal asOriginalSentence As String,
                                                            Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                            Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                            Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                            Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Dim lsMessage As String

        Try
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT = New List(Of Object)
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME = New List(Of String)
            Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE = New List(Of Object)

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ATMOSTONEStatement, Me.VAQLParsetree.Nodes(0))

            '------------------------------
            'Create a Plan
            '------------------------------
#Region "Plan Creation"
            Dim lrPlan As New Brain.Plan
            Dim lrFirstStep As Brain.Step
            Dim lrSecondStep As Brain.Step
            Dim lrThirdStep As Brain.Step

            lrFirstStep = New Brain.Step(pcenumActionType.FindOrCreateEntityTypeORFactType, True, pcenumActionType.None, Nothing)
            lrSecondStep = New Brain.Step(pcenumActionType.FindOrCreateModelElement, True, pcenumActionType.None, Nothing)
            lrThirdStep = New Brain.Step(pcenumActionType.CreateFactType,
                                     True,
                                     pcenumActionType.None,
                                     Nothing,
                                     pcenumStepFactTypeAttributes.BinaryFactType,
                                     pcenumStepFactTypeAttributes.OneToMany)


            lrPlan.AddStep(lrFirstStep)
            lrPlan.AddStep(lrSecondStep)
            lrPlan.AddStep(lrThirdStep)
#End Region

            Dim lrQuestion As tQuestion
            Dim lasSymbol As New List(Of String)
            Dim lasModelElementNames As New List(Of String)
            Dim lrSentence As Language.Sentence
            Dim lsPredicatePartWord As String = ""
            Dim lsPredicatePart As String = ""

            lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            lrSentence.FrontText = Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT
            lrSentence.FollowingText = Me.VAQLProcessor.ATMOSTONEStatement.FOLLOWINGREADINGTEXT

            Dim lsModelElementName As String = ""

            Dim liInd As Integer = 1

            For Each lsModelElementName In Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME
                Me.send_data(lsModelElementName)
                lasModelElementNames.Add(Trim(lsModelElementName))
                If liInd = 1 Then

                    Dim lbIsLikelyValueType As Boolean = False

                    If Me.Model.ModelElementIsGeneralConceptOnly(Trim(lsModelElementName)) Then
                        If My.Settings.DefaultGeneralConceptToObjectTypeConversion = "Value Type" Then
                            lbIsLikelyValueType = True
                        End If
                    End If

                    If lbIsLikelyValueType Then
#Region "ValueType"
                        lasSymbol = New List(Of String)
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                 pcenumQuestionType.CreateValueType,
                                                 pcenumExpectedResponseType.YesNo,
                                                 lasSymbol,
                                                 lrSentence,
                                                 Nothing,
                                                 lrPlan,
                                                 lrSecondStep)

                        Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                        lrQuestion.ValueType(0).DataType = liDataType
                        lrQuestion.ValueType(0).DataTypeLength = 0
                        lrQuestion.ValueType(0).DataTypePrecision = 0

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region

                    Else
#Region "EntityType"
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the EntityType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lasSymbol(0) & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                     pcenumQuestionType.CreateEntityType,
                                                     pcenumExpectedResponseType.YesNo,
                                                     lasSymbol,
                                                     lrSentence,
                                                     Nothing,
                                                     lrPlan,
                                                     lrFirstStep)

                        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, Nothing, True)
                        lrQuestion.ModelObject.Add(lrEntityType)

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
                    End If
#End Region
                ElseIf liInd = 2 Then
#Region "ValueType"
                    lasSymbol = New List(Of String)
                    lasSymbol.Add(Trim(lsModelElementName))
                    'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                    lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                 pcenumQuestionType.CreateValueType,
                                                 pcenumExpectedResponseType.YesNo,
                                                 lasSymbol,
                                                 lrSentence,
                                                 Nothing,
                                                 lrPlan,
                                                 lrSecondStep)

                    Dim lsDataTypeName As String = ""
                    Dim liDataTypeLength As Integer = 0
                    Dim liDataTypePrecision As Integer = 0
                    Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                    If Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).KEYWDWRITTENAS IsNot Nothing Then

                        Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).VALUETYPEWRITTENASCLAUSE

                        If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                            liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                            liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        End If

                        lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                        If lsDataTypeName Is Nothing Then
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End If

                        Try
                            liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                        Catch ex As Exception
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End Try

                        lrQuestion.ValueType(0).DataType = liDataType
                        lrQuestion.ValueType(0).DataTypeLength = liDataTypeLength
                        lrQuestion.ValueType(0).DataTypePrecision = liDataTypePrecision

                    End If

                    If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                        Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                    ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                        Me.AddQuestion(lrQuestion)
                    End If
#End Region
                End If

                '=======================================================================================
                'Get the PredicatePartWords from the PredicateClause of the statement
                '----------------------------------------------------------------------
#Region "Sentense/Predicates"
                Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.MODELELEMENTClause, Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT(liInd - 1))

                Dim lrPredicatePart As New Language.PredicatePart

                lrPredicatePart.PreboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                lrPredicatePart.PostboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)
                lrPredicatePart.ObjectName = Trim(Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME)

                lsPredicatePart = ""

                If liInd = 1 Then
                    Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                    Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.PREDICATEPARTClause, Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE(liInd - 1))

                    For Each lsPredicatePartWord In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
                        lsPredicatePart = Trim(lsPredicatePart & " " & lsPredicatePartWord)
                    Next
                End If

                lrPredicatePart.PredicatePartText = lsPredicatePart
                lrPredicatePart.SequenceNr = liInd
                lrSentence.PredicatePart.Add(lrPredicatePart)
#End Region
                '=======================================================================================

                liInd += 1

            Next


            Dim lsEnumeratedFactTypeReading As String

            lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasModelElementNames, lrSentence)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?",
                                pcenumQuestionType.CreateFactTypePredetermined,
                                pcenumExpectedResponseType.YesNo,
                                lasModelElementNames,
                                lrSentence,
                                Nothing,
                                lrPlan,
                                lrThirdStep)

            If abStraightToActionProcessing Then
                If Not Me.executeStatementAddFactTypePredetermined(lrQuestion, abBroadcastInterfaceEvent, arDSCError) Then
                    Return False
                End If
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False

        End Try

    End Function

    ''' <summary>
    ''' Opposite of 'ONE' Statement. E.g. FirstName is of AT LEAST ONE Person
    ''' </summary>
    ''' <param name="asOriginalSentence"></param>
    ''' <param name="abBroadcastInterfaceEvent"></param>
    ''' <param name="abStraightToActionProcessing"></param>
    Private Function FormulateQuestionsATLEASTONEStatement(ByVal asOriginalSentence As String,
                                                           Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                           Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                           Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                           Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Dim lsMessage As String

        Try
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT = New List(Of Object)
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME = New List(Of String)
            Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE = New List(Of Object)

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ATMOSTONEStatement, Me.VAQLParsetree.Nodes(0))

            '------------------------------
            'Create a Plan
            '------------------------------
#Region "Plan Creation"
            Dim lrPlan As New Brain.Plan
            Dim lrFirstStep As Brain.Step
            Dim lrSecondStep As Brain.Step
            Dim lrThirdStep As Brain.Step

            lrFirstStep = New Brain.Step(pcenumActionType.FindOrCreateEntityTypeORFactType, True, pcenumActionType.None, Nothing)
            lrSecondStep = New Brain.Step(pcenumActionType.FindOrCreateModelElement, True, pcenumActionType.None, Nothing)
            lrThirdStep = New Brain.Step(pcenumActionType.CreateFactType,
                                         True,
                                         pcenumActionType.None,
                                         Nothing,
                                         pcenumStepFactTypeAttributes.BinaryFactType,
                                         pcenumStepFactTypeAttributes.OneToMany,
                                         pcenumStepFactTypeAttributes.MandatoryFirstRole)


            lrPlan.AddStep(lrFirstStep)
            lrPlan.AddStep(lrSecondStep)
            lrPlan.AddStep(lrThirdStep)
#End Region

            Dim lrQuestion As tQuestion
            Dim lasSymbol As New List(Of String)
            Dim lasModelElementNames As New List(Of String)
            Dim lrSentence As Language.Sentence
            Dim lsPredicatePartWord As String = ""
            Dim lsPredicatePart As String = ""

            lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            lrSentence.FrontText = Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT
            lrSentence.FollowingText = Me.VAQLProcessor.ATMOSTONEStatement.FOLLOWINGREADINGTEXT

            Dim lsModelElementName As String = ""

            Dim liInd As Integer = 1

            For Each lsModelElementName In Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME
                Me.send_data(lsModelElementName)
                lasModelElementNames.Add(Trim(lsModelElementName))
                If liInd = 1 Then
                    Dim lbIsLikelyValueType As Boolean = False

                    If Me.Model.ModelElementIsGeneralConceptOnly(Trim(lsModelElementName)) Then
                        If My.Settings.DefaultGeneralConceptToObjectTypeConversion = "Value Type" Then
                            lbIsLikelyValueType = True
                        End If
                    End If

                    If lbIsLikelyValueType Then
#Region "ValueType"
                        lasSymbol = New List(Of String)
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                 pcenumQuestionType.CreateValueType,
                                                 pcenumExpectedResponseType.YesNo,
                                                 lasSymbol,
                                                 lrSentence,
                                                 Nothing,
                                                 lrPlan,
                                                 lrSecondStep)

                        Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                        lrQuestion.ValueType(0).DataType = liDataType
                        lrQuestion.ValueType(0).DataTypeLength = 0
                        lrQuestion.ValueType(0).DataTypePrecision = 0

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region

                    Else
#Region "EntityType"
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the EntityType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lasSymbol(0) & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         lrSentence,
                                                         Nothing,
                                                         lrPlan,
                                                         lrFirstStep)

                        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, Nothing, True)
                        lrQuestion.ModelObject.Add(lrEntityType)

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region
                    End If
                ElseIf liInd = 2 Then
#Region "ValueType"
                    lasSymbol = New List(Of String)
                    lasSymbol.Add(Trim(lsModelElementName))
                    'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                    lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                     pcenumQuestionType.CreateValueType,
                                                     pcenumExpectedResponseType.YesNo,
                                                     lasSymbol,
                                                     lrSentence,
                                                     Nothing,
                                                     lrPlan,
                                                     lrSecondStep)

                    Dim lsDataTypeName As String = ""
                    Dim liDataTypeLength As Integer = 0
                    Dim liDataTypePrecision As Integer = 0
                    Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                    If Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).KEYWDWRITTENAS IsNot Nothing Then

                        Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).VALUETYPEWRITTENASCLAUSE

                        If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                            liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                            liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        End If

                        lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                        If lsDataTypeName Is Nothing Then
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End If

                        Try
                            liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                        Catch ex As Exception
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End Try

                        lrQuestion.ValueType(0).DataType = liDataType
                        lrQuestion.ValueType(0).DataTypeLength = liDataTypeLength
                        lrQuestion.ValueType(0).DataTypePrecision = liDataTypePrecision

                    End If

                    If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                        Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                    ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                        Me.AddQuestion(lrQuestion)
                    End If
#End Region
                End If

                '=======================================================================================
                'Get the PredicatePartWords from the PredicateClause of the statement
                '----------------------------------------------------------------------
#Region "Sentense/Predicates"
                Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.MODELELEMENTClause, Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT(liInd - 1))

                Dim lrPredicatePart As New Language.PredicatePart

                lrPredicatePart.PreboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                lrPredicatePart.PostboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)
                lrPredicatePart.ObjectName = Trim(Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME)

                lsPredicatePart = ""

                If liInd = 1 Then
                    Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                    Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.PREDICATEPARTClause, Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE(liInd - 1))

                    For Each lsPredicatePartWord In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
                        lsPredicatePart = Trim(lsPredicatePart & " " & lsPredicatePartWord)
                    Next
                End If

                lrPredicatePart.PredicatePartText = lsPredicatePart
                lrPredicatePart.SequenceNr = liInd
                lrSentence.PredicatePart.Add(lrPredicatePart)
#End Region
                '=======================================================================================

                liInd += 1

            Next


            Dim lsEnumeratedFactTypeReading As String

            lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasModelElementNames, lrSentence)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?",
                                    pcenumQuestionType.CreateFactTypePredetermined,
                                    pcenumExpectedResponseType.YesNo,
                                    lasModelElementNames,
                                    lrSentence,
                                    Nothing,
                                    lrPlan,
                                    lrThirdStep)

            If abStraightToActionProcessing Then
                If Not Me.executeStatementAddFactTypePredetermined(lrQuestion, abBroadcastInterfaceEvent, arDSCError) Then
                    Return False
                End If
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False

        End Try

    End Function

    Private Function FormulateQuestionsATMOSTONEStatement(ByVal asOriginalSentence As String,
                                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                          Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                          Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                          Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Dim lsMessage As String

        Try
            'CodeSafe
            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT = New List(Of Object)
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME = New List(Of String)
            Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE = New List(Of Object)

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ATMOSTONEStatement, Me.VAQLParsetree.Nodes(0))

            '------------------------------
            'Create a Plan
            '------------------------------
            Dim lrPlan As New Brain.Plan
            Dim lrFirstStep As Brain.Step
            Dim lrSecondStep As Brain.Step
            Dim lrThirdStep As Brain.Step

            lrFirstStep = New Brain.Step(pcenumActionType.FindOrCreateEntityTypeORFactType, True, pcenumActionType.None, Nothing)
            lrSecondStep = New Brain.Step(pcenumActionType.FindOrCreateModelElement, True, pcenumActionType.None, Nothing)
            lrThirdStep = New Brain.Step(pcenumActionType.CreateFactType,
                                         True,
                                         pcenumActionType.None,
                                         Nothing,
                                         pcenumStepFactTypeAttributes.BinaryFactType,
                                         pcenumStepFactTypeAttributes.ManyToOne)

            lrPlan.AddStep(lrFirstStep)
            lrPlan.AddStep(lrSecondStep)
            lrPlan.AddStep(lrThirdStep)

            Dim lrQuestion As tQuestion
            Dim lasSymbol As New List(Of String)
            Dim lasModelElementNames As New List(Of String)
            Dim lrSentence As Language.Sentence
            Dim lsPredicatePartWord As String = ""
            Dim lsPredicatePart As String = ""

            lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            lrSentence.FrontText = Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT
            lrSentence.FollowingText = Me.VAQLProcessor.ATMOSTONEStatement.FOLLOWINGREADINGTEXT

            Dim lsModelElementName As String = ""

            Dim liInd As Integer = 1


            For Each lsModelElementName In Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME
                Me.send_data(lsModelElementName)
                lasModelElementNames.Add(Trim(lsModelElementName))

                Dim lbIsLikelyValueType As Boolean = False
                Dim lbIsEntityType As Boolean = False

                If liInd = 1 Then

                    If Me.Model.ModelElementIsGeneralConceptOnly(Trim(lsModelElementName)) Then
                        If My.Settings.DefaultGeneralConceptToObjectTypeConversion = "Value Type" Then
                            lbIsLikelyValueType = True
                        End If
                    End If

                    If lbIsLikelyValueType Then
#Region "ValueType"
                        lasSymbol = New List(Of String)
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                 pcenumQuestionType.CreateValueType,
                                                 pcenumExpectedResponseType.YesNo,
                                                 lasSymbol,
                                                 lrSentence,
                                                 Nothing,
                                                 lrPlan,
                                                 lrSecondStep)

                        Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                        lrQuestion.ValueType(0).DataType = liDataType
                        lrQuestion.ValueType(0).DataTypeLength = 0
                        lrQuestion.ValueType(0).DataTypePrecision = 0

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region

                    Else
#Region "EntityType"
                        lasSymbol.Add(Trim(lsModelElementName))
                        'NB Brain can answer its own questions, so if the EntityType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lasSymbol(0) & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         lrSentence,
                                                         Nothing,
                                                         lrPlan,
                                                         lrFirstStep)

                        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, Nothing, True)
                        lrQuestion.ModelObject.Add(lrEntityType)

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
                    End If
#End Region
                ElseIf liInd = 2 Then
#Region "ValueType"
                    If Me.Model.ModelElementIsGeneralConceptOnly(Trim(lsModelElementName)) Then
                        Select Case My.Settings.DefaultGeneralConceptToObjectTypeConversion
                            Case Is = "Value Type"
                                lbIsLikelyValueType = True
                            Case Is = "Entity Type"
                                lbIsEntityType = True
                            Case Else
                                lbIsLikelyValueType = True
                        End Select
                    Else
                        lbIsLikelyValueType = True
                    End If

                    lasSymbol = New List(Of String)

                    If lbIsLikelyValueType Then
                        lasSymbol.Add(Trim(lsModelElementName))

                        'NB Brain can answer its own questions, so if the ValueType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lasSymbol(0) & "'?",
                                                     pcenumQuestionType.CreateValueType,
                                                     pcenumExpectedResponseType.YesNo,
                                                     lasSymbol,
                                                     lrSentence,
                                                     Nothing,
                                                     lrPlan,
                                                     lrSecondStep)

                        Dim lsDataTypeName As String = ""
                        Dim liDataTypeLength As Integer = 0
                        Dim liDataTypePrecision As Integer = 0
                        Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

                        If Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).KEYWDWRITTENAS IsNot Nothing Then

                            Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).VALUETYPEWRITTENASCLAUSE

                            If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                                lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                            ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                                lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                                liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                            ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                                lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                                liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                            End If

                            lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                            If lsDataTypeName Is Nothing Then
                                lsMessage = "That's not a valid Data Type."
                                If arDSCError IsNot Nothing Then
                                    arDSCError.Success = False
                                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                    arDSCError.ErrorString = lsMessage
                                End If
                                Me.send_data(lsMessage)
                                Return False
                            End If

                            Try
                                liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                            Catch ex As Exception
                                lsMessage = "That's not a valid Data Type."
                                If arDSCError IsNot Nothing Then
                                    arDSCError.Success = False
                                    arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                    arDSCError.ErrorString = lsMessage
                                End If
                                Me.send_data(lsMessage)
                                Return False
                            End Try

                            lrQuestion.ValueType(0).DataType = liDataType
                            lrQuestion.ValueType(0).DataTypeLength = liDataTypeLength
                            lrQuestion.ValueType(0).DataTypePrecision = liDataTypePrecision

                        End If

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region
                    Else
#Region "EntityType"
                        lasSymbol.Add(Trim(lsModelElementName))

                        'NB Brain can answer its own questions, so if the EntityType already exists, the following creates no problem.
                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lasSymbol(0) & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         lrSentence,
                                                         Nothing,
                                                         lrPlan,
                                                         lrFirstStep)

                        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, Nothing, True)
                        lrQuestion.ModelObject.Add(lrEntityType)

                        If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                            Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
                        ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                            Me.AddQuestion(lrQuestion)
                        End If
#End Region
                    End If

                End If

                '=======================================================================================
                'Get the PredicatePartWords from the PredicateClause of the statement
                '----------------------------------------------------------------------
                Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.MODELELEMENTClause, Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT(liInd - 1))


                Dim lrPredicatePart As New Language.PredicatePart

                lrPredicatePart.PreboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                lrPredicatePart.PostboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)
                lrPredicatePart.ObjectName = Trim(Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME)

                lsPredicatePart = ""

                If liInd = 1 Then
                    Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                    Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.PREDICATEPARTClause, Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE(liInd - 1))

                    For Each lsPredicatePartWord In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
                        lsPredicatePart = Trim(lsPredicatePart & " " & lsPredicatePartWord)
                    Next
                End If

                lrPredicatePart.PredicatePartText = lsPredicatePart
                lrPredicatePart.SequenceNr = liInd
                lrSentence.PredicatePart.Add(lrPredicatePart)
                '=======================================================================================

                liInd += 1
            Next

            Dim lsEnumeratedFactTypeReading As String

            lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasModelElementNames, lrSentence)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?",
                                    pcenumQuestionType.CreateFactTypePredetermined,
                                    pcenumExpectedResponseType.YesNo,
                                    lasModelElementNames,
                                    lrSentence,
                                    Nothing,
                                    lrPlan,
                                    lrThirdStep)

            If abStraightToActionProcessing Then
                If Not Me.executeStatementAddFactTypePredetermined(lrQuestion, abBroadcastInterfaceEvent, arDSCError) Then
                    Return False
                End If
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False

        End Try

    End Function

    Private Sub FormulateQuestionCreateInternalUniquenessConstraint(ByRef arFactType As FBM.FactType,
                                                                    ByRef arFactTypeReading As FBM.FactTypeReading,
                                                                    Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                                    Optional ByVal abStraightToActionProcessing As Boolean = False)

        '------------------------------
        'Create a Plan
        '------------------------------
        Dim lrPlan As New Brain.Plan
        Dim lrFirstStep As Brain.Step

        lrFirstStep = New Brain.Step(pcenumActionType.CreateInternalUniquenessConstraint, False, pcenumActionType.None, New List(Of FBM.ModelObject))

        lrPlan.AddStep(lrFirstStep)

        Dim lrQuestion As tQuestion

        Dim lsQuestionText As String = "" & vbCrLf

        lsQuestionText.AppendString(arFactType.RoleGroup(0).JoinedORMObject.Id & " " & arFactTypeReading.PredicatePart(0).PredicatePartText & " AT MOST ONE " & arFactType.RoleGroup(1).JoinedORMObject.Id & ";")
        lsQuestionText.AppendLine(arFactType.RoleGroup(0).JoinedORMObject.Id & " " & arFactTypeReading.PredicatePart(0).PredicatePartText & " ONE " & arFactType.RoleGroup(1).JoinedORMObject.Id & "; or")
        lsQuestionText.AppendLine("The relationship is Many-to-Many?")

        lrQuestion = New tQuestion(lsQuestionText,
                                   pcenumQuestionType.CreateInternalUniquenessConstraint,
                                   pcenumExpectedResponseType.ATMOSTONEONEMANYTOMANY, Nothing, Nothing, arFactType, lrPlan, lrPlan.Step(0),, Nothing)

        lrQuestion.ModelObject.Add(arFactType)

        'If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
        '    Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
        'Else
        If Not Me.QuestionHasBeenRaised(lrQuestion) Then
            Me.AddQuestion(lrQuestion)
        End If

        Me.Timeout.Start()

    End Sub


    Private Function FormulateQuestionsFACTTYPEStatement(ByVal asOriginalSentence As String,
                                                         Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                         Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                         Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                         Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Try
            Dim lrFactTypeReading As New FBM.FactTypeReading
            Dim lrPredicatePart As Language.PredicatePart
            Dim lrPlan As New Brain.Plan 'The Plan formulated to create the FactType.
            Dim lrStep As Brain.Step 'For Steps added to the Plan.
            Dim lrQuestion As tQuestion

            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT = ""
            Me.VAQLProcessor.FACTTYPEREADINGStatement.MODELELEMENT = New List(Of Object)
            Me.VAQLProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE = New List(Of Object)
            Me.VAQLProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = ""
            Me.VAQLProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT = ""

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.FACTTYPEREADINGStatement, Me.VAQLParsetree.Nodes(0))

            lrFactTypeReading.FrontText = Trim(NullVal(Me.VAQLProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT, ""))

            Dim lrModelElementNode As VAQL.ParseNode
            Dim lrPredicateClauseNode As VAQL.ParseNode
            Dim liInd As Integer = 0
            Dim lasModelObjectId As New List(Of String)

            Dim lrSentence As Language.Sentence
            Dim lrWordResolved As Language.WordResolved

            lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            lrSentence.FrontText = Me.VAQLProcessor.FACTTYPEREADINGStatement.FRONTREADINGTEXT
            lrSentence.FollowingText = Me.VAQLProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT

            For liInd = 1 To Me.VAQLProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count

                lrPredicatePart = New Language.PredicatePart()
                lrPredicatePart.SequenceNr = liInd

                lrModelElementNode = Me.VAQLProcessor.FACTTYPEREADINGStatement.MODELELEMENT(liInd - 1)
                Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.MODELELEMENTClause, lrModelElementNode)

                Dim lsModelObjectName As String = Trim(Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME)

                lrWordResolved = New Language.WordResolved(lsModelObjectName, pcenumWordSense.Noun)
                lrSentence.WordListResolved.Add(lrWordResolved)

                '------------------------------------------------------------------------------------------------------
                'Check to see whether the MODELELEMENTNAME is an Object Type that is actually linked by the FactType.
                '------------------------------------------------------------------------------------------------------
                Dim lbProceedToCreateObjectTypeQuestion As Boolean = False

                If Me.Model.ExistsModelElement(lsModelObjectName, True) Then

                    If Array.IndexOf({pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType},
                                  Me.Model.GetModelObjectByName(lsModelObjectName).ConceptType) >= 0 Then
                        '-----------------------------------------------------------------------------
                        'A ObjectType already exists within the Model for the name lsModelObjectName
                        '-----------------------------------------------------------------------------
                    Else
                        lbProceedToCreateObjectTypeQuestion = True
                    End If
                Else
                    lbProceedToCreateObjectTypeQuestion = True
                End If

                If lbProceedToCreateObjectTypeQuestion Then

                    Dim lbIsLikelyValueType As Boolean = False

                    If Me.Model.ModelElementIsGeneralConceptOnly(lsModelObjectName) Then
                        If My.Settings.DefaultGeneralConceptToObjectTypeConversion = "Value Type" Then
                            lbIsLikelyValueType = True
                        End If
                    End If

                    Dim items As Array
                    items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
                    Dim item As pcenumReferenceModeEndings
                    For Each item In items
                        If lsModelObjectName.EndsWith(GetEnumDescription(item)) Then
                            lbIsLikelyValueType = True
                            Exit For
                        ElseIf lsModelObjectName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                            lbIsLikelyValueType = True
                            Exit For
                        End If
                    Next

                    If lbIsLikelyValueType Then
                        lrStep = New Brain.Step(pcenumActionType.CreateValueType, True, pcenumActionType.None, Nothing)
                        lrPlan.AddStep(lrStep)

                        Dim lasSymbol As New List(Of String)
                        lasSymbol.Add(lsModelObjectName)

                        lrQuestion = New tQuestion("Would you like me to create an Value Type for '" & lsModelObjectName & "'?",
                                                         pcenumQuestionType.CreateValueType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         lrPlan,
                                                         lrStep)
                    Else
                        lrStep = New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.CreateValueType, Nothing)
                        lrPlan.AddStep(lrStep)

                        Dim lasSymbol As New List(Of String)
                        lasSymbol.Add(lsModelObjectName)

                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lsModelObjectName & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         pcenumExpectedResponseType.YesNo,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         lrPlan,
                                                         lrStep)
                    End If


                    If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Then
                        Select Case lrQuestion.QuestionType
                            Case Is = pcenumQuestionType.CreateValueType
                                Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent)
                            Case Is = pcenumQuestionType.CreateEntityType
                                Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent)
                        End Select

                    ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                        '------------------------------------------------------------
                        'Great, already asked the question and am awaiting responce
                        '------------------------------------------------------------
                        Me.AddQuestion(lrQuestion)
                    End If
                End If

                lrPredicatePart.PreboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                lrPredicatePart.PostboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)

                lrPredicatePart.ObjectName = Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME

                Dim lsPredicatePartText As String = ""

                If Me.VAQLProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART = "" Then
                    '----------------------------------------
                    'FactType is binary or greater in arity
                    '----------------------------------------
                    If liInd < Me.VAQLProcessor.FACTTYPEREADINGStatement.MODELELEMENT.Count Then
                        lrPredicateClauseNode = Me.VAQLProcessor.FACTTYPEREADINGStatement.PREDICATECLAUSE(liInd - 1)
                        Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                        Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.PREDICATEPARTClause, lrPredicateClauseNode)

                        For Each lsPredicatePartText In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
                            lrPredicatePart.PredicatePartText &= lsPredicatePartText
                        Next
                    End If

                    lrPredicatePart.PredicatePartText = Trim(lrPredicatePart.PredicatePartText)
                Else
                    '------------------------------
                    'FactType is a unary FactType
                    '------------------------------
                    lrPredicatePart.PredicatePartText = Trim(Me.VAQLProcessor.FACTTYPEREADINGStatement.UNARYPREDICATEPART)
                End If

                lrSentence.PredicatePart.Add(lrPredicatePart)
            Next 'ModelElement


            lrStep = New Brain.Step(pcenumActionType.CreateFactType, True, pcenumActionType.None, Nothing)
            lrPlan.AddStep(lrStep)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & Trim(asOriginalSentence) & "'?",
                                            pcenumQuestionType.CreateFactType,
                                            pcenumExpectedResponseType.YesNo,
                                            Nothing,
                                            lrSentence,
                                            Nothing,
                                            lrPlan,
                                            lrStep)

            If abStraightToActionProcessing Then
                Call Me.executeStatementAddFactType(lrQuestion, abBroadcastInterfaceEvent, abStraightToActionProcessing, arDSCError)
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            '-----------------------------------------------
            'Get the FollowingReadingText if there is one.
            '-----------------------------------------------
            lrFactTypeReading.FollowingText = Trim(Me.VAQLProcessor.FACTTYPEREADINGStatement.FOLLOWINGREADINGTEXT)

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function FormulateQuestionsONEStatement(ByVal asOriginalSentence As String,
                                                    Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                    Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                    Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                    Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Dim lsMessage As String

        Try
            Dim lrFactTypeReading As New FBM.FactTypeReading
            Dim lrPredicatePart As Language.PredicatePart
            Dim lrPlan As New Brain.Plan 'The Plan formulated to create the FactType.
            Dim lrStep As Brain.Step 'For Steps added to the Plan.
            Dim lrQuestion As tQuestion

            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT = ""
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME = New List(Of String)
            Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT = New List(Of Object)
            Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE = New List(Of Object)
            Me.VAQLProcessor.ATMOSTONEStatement.UNARYPREDICATEPART = ""
            Me.VAQLProcessor.ATMOSTONEStatement.FOLLOWINGREADINGTEXT = ""
            Me.VAQLProcessor.ATMOSTONEStatement.KEYWDWRITTENAS = Nothing
            Me.VAQLProcessor.ATMOSTONEStatement.VALUETYPEWRITTENASCLAUSE = Nothing

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ATMOSTONEStatement, Me.VAQLParsetree)

            lrFactTypeReading.FrontText = Trim(NullVal(Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT, ""))

            Dim lrModelElementNode As VAQL.ParseNode
            Dim lrPredicateClauseNode As VAQL.ParseNode
            Dim liInd As Integer = 0
            Dim lasModelObjectId As New List(Of String)

            Dim lrSentence As Language.Sentence
            Dim lrWordResolved As Language.WordResolved

            lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            lrSentence.FrontText = Me.VAQLProcessor.ATMOSTONEStatement.FRONTREADINGTEXT
            lrSentence.FollowingText = Me.VAQLProcessor.ATMOSTONEStatement.FOLLOWINGREADINGTEXT

            For liInd = 1 To Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT.Count

                lrPredicatePart = New Language.PredicatePart()
                lrPredicatePart.SequenceNr = liInd

                lrModelElementNode = Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT(liInd - 1)
                Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT = ""
                Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME = ""
                Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.MODELELEMENTClause, lrModelElementNode)

                Dim lsModelObjectName As String = Trim(Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME)

                lasModelObjectId.Add(lsModelObjectName)

                lrWordResolved = New Language.WordResolved(lsModelObjectName, pcenumWordSense.Noun)
                lrSentence.WordListResolved.Add(lrWordResolved)

                '------------------------------------------------------------------------------------------------------
                'Check to see whether the MODELELEMENTNAME is an Object Type that is actually linked by the FactType.
                '------------------------------------------------------------------------------------------------------
                Dim lbProceedToCreateObjectTypeQuestion As Boolean = False

                If Me.Model.ExistsModelElement(lsModelObjectName) Then
                    If Array.IndexOf({pcenumConceptType.ValueType,
                                        pcenumConceptType.EntityType,
                                        pcenumConceptType.FactType},
                                    Me.Model.GetModelObjectByName(lsModelObjectName).ConceptType) >= 0 Then
                        '-----------------------------------------------------------------------------
                        'A ObjectType already exists within the Model for the name lsModelObjectName
                        '-----------------------------------------------------------------------------
                    Else
                        lbProceedToCreateObjectTypeQuestion = True
                    End If
                Else
                    lbProceedToCreateObjectTypeQuestion = True
                End If

                If lbProceedToCreateObjectTypeQuestion Then

                    Dim lbIsLikelyValueType As Boolean = False
                    Dim items As Array
                    items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))

                    If Me.Model.ModelElementIsGeneralConceptOnly(lsModelObjectName) Then
                        If My.Settings.DefaultGeneralConceptToObjectTypeConversion = "Value Type" Then
                            lbIsLikelyValueType = True
                        End If
                    End If

                    If liInd <> 1 And Me.VAQLProcessor.ATMOSTONEStatement.KEYWDWRITTENAS IsNot Nothing Then
                        lbIsLikelyValueType = True
                    Else
                        For Each item In items
                            If lsModelObjectName.EndsWith(GetEnumDescription(item)) Then
                                lbIsLikelyValueType = True
                                Exit For
                            ElseIf lsModelObjectName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                                lbIsLikelyValueType = True
                                Exit For
                            End If
                        Next
                    End If

                    items = System.Enum.GetValues(GetType(pcenumValueTypeCandidates))
                    For Each item In items
                        If lsModelObjectName.EndsWith(GetEnumDescription(item)) Then
                            lbIsLikelyValueType = True
                            Exit For
                        End If
                    Next


                    Dim lrValueType As FBM.ValueType = Nothing
                    Dim lsDataTypeName As String = ""
                    Dim liDataTypeLength As Integer = 0
                    Dim liDataTypePrecision As Integer = 0
                    Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

                    If (liInd <> 1) And Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).KEYWDWRITTENAS IsNot Nothing Then

                        Me.VAQLProcessor.VALUETYPEWRITTENASClause = Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE(Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTTYPE.Count - 1).VALUETYPEWRITTENASCLAUSE

                        If Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPE.Nodes(0).Token.Text
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPELENGTH.Nodes(0).Token.Text
                            liDataTypeLength = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        ElseIf Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION IsNot Nothing Then
                            lsDataTypeName = Me.VAQLProcessor.VALUETYPEWRITTENASClause.DATATYPEPRECISION.Nodes(0).Token.Text
                            liDataTypePrecision = CInt(Me.VAQLProcessor.VALUETYPEWRITTENASClause.NUMBER)
                        End If

                        lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
                        If lsDataTypeName Is Nothing Then
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End If

                        Try
                            liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
                        Catch ex As Exception
                            lsMessage = "That's not a valid Data Type."
                            If arDSCError IsNot Nothing Then
                                arDSCError.Success = False
                                arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                                arDSCError.ErrorString = lsMessage
                            End If
                            Me.send_data(lsMessage)
                            Return False
                        End Try

                        lrValueType = Me.Model.CreateValueType(lsModelObjectName, False, liDataType, liDataTypeLength, liDataTypePrecision, abBroadcastInterfaceEvent)

                        lbIsLikelyValueType = True
                    End If

                    If lbIsLikelyValueType Then
                        lrStep = New Brain.Step(pcenumActionType.CreateValueType, True, pcenumActionType.None, Nothing)
                        lrPlan.AddStep(lrStep)

                        Dim lasSymbol As New List(Of String)
                        lasSymbol.Add(lsModelObjectName)

                        lrQuestion = New tQuestion("Would you like me to create an Value Type for '" & lsModelObjectName & "'?",
                                                            pcenumQuestionType.CreateValueType,
                                                            pcenumExpectedResponseType.YesNo,
                                                            lasSymbol,
                                                            Nothing,
                                                            lrValueType,
                                                            lrPlan,
                                                            lrStep)
                    Else
                        lrStep = New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.CreateValueType, Nothing)
                        lrPlan.AddStep(lrStep)

                        Dim lasSymbol As New List(Of String)
                        lasSymbol.Add(lsModelObjectName)

                        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lsModelObjectName & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                            pcenumQuestionType.CreateEntityType,
                                                            pcenumExpectedResponseType.YesNo,
                                                            lasSymbol,
                                                            Nothing,
                                                            Nothing,
                                                            lrPlan,
                                                            lrStep)
                    End If


                    If abStraightToActionProcessing And Not Me.QuestionIsResolved(lrQuestion) Or (liInd <> 1 And Me.VAQLProcessor.ATMOSTONEStatement.KEYWDWRITTENAS IsNot Nothing) Then
                        Select Case lrQuestion.QuestionType
                            Case Is = pcenumQuestionType.CreateValueType
                                Call Me.ProcessStatementAddValueType(lrQuestion, abBroadcastInterfaceEvent, arFEKLLineageObject)
                            Case Is = pcenumQuestionType.CreateEntityType
                                Call Me.ProcessStatementAddEntityType(lrQuestion, abBroadcastInterfaceEvent, arFEKLLineageObject)
                        End Select
                    ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                        Me.AddQuestion(lrQuestion)
                    End If
                End If

                lrPredicatePart.PreboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.PREBOUNDREADINGTEXT)
                lrPredicatePart.PostboundText = Trim(Me.VAQLProcessor.MODELELEMENTClause.POSTBOUNDREADINGTEXT)

                lrPredicatePart.ObjectName = Me.VAQLProcessor.MODELELEMENTClause.MODELELEMENTNAME

                Dim lsPredicatePartText As String = ""

                If Me.VAQLProcessor.ATMOSTONEStatement.UNARYPREDICATEPART = "" Then
                    '----------------------------------------
                    'FactType is binary or greater in arity
                    '----------------------------------------
                    If liInd < Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENT.Count Then
                        lrPredicateClauseNode = Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE(liInd - 1)
                        Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)
                        Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.PREDICATEPARTClause, lrPredicateClauseNode)

                        For Each lsPredicatePartText In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
                            lrPredicatePart.PredicatePartText &= lsPredicatePartText
                        Next
                    End If

                    lrPredicatePart.PredicatePartText = Trim(lrPredicatePart.PredicatePartText)
                Else
                    '------------------------------
                    'FactType is a unary FactType
                    '------------------------------
                    lrPredicatePart.PredicatePartText = Trim(Me.VAQLProcessor.ATMOSTONEStatement.UNARYPREDICATEPART)
                End If

                lrSentence.PredicatePart.Add(lrPredicatePart)
            Next 'ModelElement

            lrStep = New Brain.Step(pcenumActionType.CreateFactType,
                                    True,
                                    pcenumActionType.None,
                                    Nothing,
                                    pcenumStepFactTypeAttributes.BinaryFactType,
                                    pcenumStepFactTypeAttributes.ManyToOne,
                                    pcenumStepFactTypeAttributes.MandatoryFirstRole)


            lrPlan.AddStep(lrStep)

            Dim lsEnumeratedFactTypeReading As String

            lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasModelObjectId, lrSentence) ' Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART)

            lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?",
                                    pcenumQuestionType.CreateFactTypePredetermined,
                                    pcenumExpectedResponseType.YesNo,
                                    lasModelObjectId,
                                    lrSentence,
                                    Nothing,
                                    lrPlan,
                                    lrStep)

            If abStraightToActionProcessing Then
                If Not Me.executeStatementAddFactTypePredetermined(lrQuestion, abBroadcastInterfaceEvent, arDSCError, arFEKLLineageObject) Then
                    Return False
                End If
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            Return True

            '=====================================
            'Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME = New List(Of String)
            'Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE = New Object

            'Call Me.VAQLProcessor.GetParseTreeTokens(Me.VAQLProcessor.ATMOSTONEStatement, Me.VAQLParsetree)

            ''------------------------------
            ''Create a Plan
            ''------------------------------
            'Dim lrFirstStep As Brain.Step
            'Dim lrSecondStep As Brain.Step
            'Dim lrThirdStep As Brain.Step

            'lrFirstStep = New Brain.Step(pcenumActionType.FindOrCreateEntityTypeORFactType, True, pcenumActionType.None)
            'lrSecondStep = New Brain.Step(pcenumActionType.FindOrCreateModelElement, True, pcenumActionType.None)
            'lrThirdStep = New Brain.Step(pcenumActionType.CreateFactType, _
            '                             True, _
            '                             pcenumActionType.None, _
            '                             pcenumStepFactTypeAttributes.BinaryFactType, _
            '                             pcenumStepFactTypeAttributes.ManyToOne, _
            '                             pcenumStepFactTypeAttributes.MandatoryFirstRole)

            'lrPlan.AddStep(lrFirstStep)
            'lrPlan.AddStep(lrSecondStep)
            'lrPlan.AddStep(lrThirdStep)

            'Dim lrQuestion As tQuestion
            'Dim lasSymbol As New List(Of String)
            'Dim lasModelElementNames As New List(Of String)
            'Dim lrSentence As New Language.Sentence("")

            'Dim lsModelElementName As String = ""
            'Dim liInd As Integer = 1
            'For Each lsModelElementName In Me.VAQLProcessor.ATMOSTONEStatement.MODELELEMENTNAME
            '    Me.send_data(lsModelElementName)
            '    lasModelElementNames.Add(lsModelElementName)
            '    If liInd = 1 Then
            '        lasSymbol.Add(lsModelElementName)
            '        lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & lsModelElementName & "'?", _
            '                                         pcenumQuestionType.CreateEntityType, _
            '                                         True, _
            '                                         lasSymbol, _
            '                                         lrSentence, _
            '                                         Nothing, _
            '                                         lrPlan, _
            '                                         lrFirstStep)

            '        Dim lrEntityType As New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, Nothing, True)
            '        lrQuestion.EntityType.Add(lrEntityType)

            '        If Me.QuestionHasBeenRaised(lrQuestion) Then
            '            '------------------------------------------------------------
            '            'Great, already asked the question and am awaiting responce
            '            '------------------------------------------------------------
            '        Else
            '            Me.AddQuestion(lrQuestion)
            '        End If
            '    ElseIf liInd = 2 Then
            '        lasSymbol = New List(Of String)
            '        lasSymbol.Add(lsModelElementName)
            '        lrQuestion = New tQuestion("Would you like me to create a Value Type for '" & lsModelElementName & "'?", _
            '                                         pcenumQuestionType.CreateValueType, _
            '                                          True, _
            '                                         lasSymbol, _
            '                                         lrSentence, _
            '                                         Nothing, _
            '                                         lrPlan, _
            '                                         lrSecondStep)

            '        Dim lrValueType As New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lsModelElementName, True)
            '        lrQuestion.ValueType.Add(lrValueType)

            '        If Me.QuestionHasBeenRaised(lrQuestion) Then
            '            '------------------------------------------------------------
            '            'Great, already asked the question and am awaiting responce
            '            '------------------------------------------------------------
            '        Else
            '            Me.AddQuestion(lrQuestion)
            '        End If
            '    End If
            '    liInd += 1
            'Next

            ''=======================================================================================
            ''Get the PredicatePartWords from the PredicateClause of the statement
            ''----------------------------------------------------------------------
            'Dim lsPredicatePartWord As String = ""
            'Dim lsPredicatePart As String = ""

            'Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART = New List(Of String)

            'Call Me.VAQLProcessor.GetParseTreeTokens(Me.VAQLProcessor.PREDICATEPARTClause, Me.VAQLProcessor.ATMOSTONEStatement.PREDICATECLAUSE)

            'For Each lsPredicatePartWord In Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART
            '    lsPredicatePart = Trim(lsPredicatePart & " " & lsPredicatePartWord)
            'Next
            'Dim lrPredicatePart As New Language.PredicatePart
            'lrPredicatePart.PredicatePartText = lsPredicatePart

            'lrSentence.PredicatePart.Add(lrPredicatePart)
            ''=======================================================================================

            'Dim lsEnumeratedFactTypeReading As String

            'lsEnumeratedFactTypeReading = Me.CreateEnumeratedFactTypeReadingFromParts(lasModelElementNames, lrSentence.PredicatePart) ' Me.VAQLProcessor.PREDICATEPARTClause.PREDICATEPART)

            'lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & lsEnumeratedFactTypeReading & "'?", _
            '                        pcenumQuestionType.CreateFactTypePredetermined, _
            '                        True, _
            '                        lasModelElementNames, _
            '                        lrSentence, _
            '                        Nothing, _
            '                        lrPlan, _
            '                        lrThirdStep)

            'If Me.QuestionHasBeenRaised(lrQuestion) Then
            '    '------------------------------------------------------------
            '    'Great, already asked the question and am awaiting responce
            '    '------------------------------------------------------------
            'Else
            '    Me.AddQuestion(lrQuestion)
            'End If

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False

        End Try

    End Function

    Private Function FormulateQuestionsISWHEREStatement(ByVal asOriginalSentence As String,
                                                        Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                        Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                        Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                        Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Try
            Dim lsMessage As String = ""
            Dim lrPlan As New Brain.Plan 'The Plan formulated to create the FactType.
            Dim lrStep As Brain.Step 'For Steps added to the Plan.
            Dim lrPredicatePart As New Language.PredicatePart
            Dim lrQuestion As tQuestion
            Dim lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.ISWHEREStatement = New VAQL.IsWhereStatement

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ISWHEREStatement, Me.VAQLParsetree.Nodes(0))

            '===================================================================
            'Validate the IS WHERE Statement
            If Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT.Count > 1 Then
                For liInd = 1 To Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT.Count - 1
                    If Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(liInd).MODELELEMENT.Count <> Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(0).MODELELEMENT.Count Then

                        lsMessage = "That is an incorrect IS WHERE statement. Each Fact Type Reading must have the same number of Model Elements."
                        If arDSCError IsNot Nothing Then
                            arDSCError.Success = False
                            arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                            arDSCError.ErrorString = lsMessage
                        End If
                        Me.send_data(lsMessage)

                        lsMessage = "'" & Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(liInd).makeSentence & "' does not have the same number of Model Elements as '" & Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(0).makeSentence & "'."
                        Me.send_data(lsMessage)
                        Return False
                    End If
                Next
            End If
            '===================================================================

            Me.Timeout.Stop()

            'Check to see if the FactTypeName already exists
            Dim lsFactTypeName = Me.VAQLProcessor.ISWHEREStatement.MODELELEMENT(0).MODELELEMENTNAME

            If Me.Model.ExistsModelElement(lsFactTypeName) Then
                If Array.IndexOf({pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType},
                                  Me.Model.GetModelObjectByName(lsFactTypeName).ConceptType) >= 0 Then
                    '-----------------------------------------------------------------------------
                    'A ObjectType already exists within the Model for the name lsModelObjectName
                    '-----------------------------------------------------------------------------
                    lsMessage = "A Model Element already exists in the Model with the name, '" & lsFactTypeName & "'."
                    If arDSCError IsNot Nothing Then
                        arDSCError.Success = False
                        arDSCError.ErrorType = [Interface].publicConstants.pcenumErrorType.ModelElementAlreadyExists
                        arDSCError.ErrorString = lsMessage
                    End If
                    Me.send_data(lsMessage)
                    Return False

                End If
            Else
                'Get the ModelElements for the overall FactType. Only need to get them for the first FactTypeStatement.
                '  NB Subsequent FactTypeStatements in the ISWHERE Statement must have matching ModelElementNames.

                Dim lrFactTypeStatement = Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(0)
                Dim liInd = 1
                For Each lrModelElement In lrFactTypeStatement.MODELELEMENT

                    Call Me.createPlanStepForModelElement(lrModelElement.MODELELEMENTNAME, lrPlan)

                    Dim lrWordResolved = New Language.WordResolved(lrModelElement.MODELELEMENTNAME, pcenumWordSense.Noun)
                    lrSentence.WordListResolved.Add(lrWordResolved)

                    lrPredicatePart = New Language.PredicatePart
                    lrPredicatePart.PreboundText = Trim(lrModelElement.PREBOUNDREADINGTEXT)
                    lrPredicatePart.PostboundText = Trim(lrModelElement.POSTBOUNDREADINGTEXT)
                    lrPredicatePart.ObjectName = lrModelElement.MODELELEMENTNAME

                    If liInd < lrFactTypeStatement.MODELELEMENT.Count Then
                        For Each lsPredicatePartText In lrFactTypeStatement.PREDICATECLAUSE(liInd - 1).PREDICATEPART
                            lrPredicatePart.PredicatePartText &= lsPredicatePartText
                            lrPredicatePart.PredicatePartText = LTrim(lrPredicatePart.PredicatePartText)
                        Next
                    End If

                    lrSentence.PredicatePart.Add(lrPredicatePart)

                    liInd += 1
                Next

                lrSentence.FrontText = lrFactTypeStatement.FRONTREADINGTEXT
                lrSentence.FollowingText = lrFactTypeStatement.FOLLOWINGREADINGTEXT

                lrSentence.Sentence = lrFactTypeStatement.makeSentence
                lrSentence.OriginalSentence = lrFactTypeStatement.makeSentence

                lrStep = New Brain.Step(pcenumActionType.CreateFactType, True, pcenumActionType.None, Nothing)
                lrPlan.AddStep(lrStep)

                '===========================================================
                'Additional FactTypeReadings/Sentences
                Dim larAdditionalSentence As New List(Of Language.Sentence)
                If Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT.Count > 1 Then

                    For liFactTypeStatement = 1 To Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT.Count - 1

                        Dim lrAdditionalSentence = New Language.Sentence("", "")

                        lrFactTypeStatement = Me.VAQLProcessor.ISWHEREStatement.FACTTYPESTMT(liFactTypeStatement)

                        liInd = 1
                        For Each lrModelElement In lrFactTypeStatement.MODELELEMENT

                            lrPredicatePart = New Language.PredicatePart
                            lrPredicatePart.PreboundText = Trim(lrModelElement.PREBOUNDREADINGTEXT)
                            lrPredicatePart.PostboundText = Trim(lrModelElement.POSTBOUNDREADINGTEXT)
                            lrPredicatePart.ObjectName = lrModelElement.MODELELEMENTNAME

                            If liInd < lrFactTypeStatement.MODELELEMENT.Count Then
                                For Each lsPredicatePartText In lrFactTypeStatement.PREDICATECLAUSE(liInd - 1).PREDICATEPART
                                    lrPredicatePart.PredicatePartText &= lsPredicatePartText
                                    lrPredicatePart.PredicatePartText = LTrim(lrPredicatePart.PredicatePartText)
                                Next
                            End If
                            lrAdditionalSentence.PredicatePart.Add(lrPredicatePart)

                            liInd += 1
                        Next

                        larAdditionalSentence.Add(lrAdditionalSentence)
                    Next
                Else
                    larAdditionalSentence = Nothing
                End If

                lrQuestion = New tQuestion("Would you like me to create a Fact Type for '" & Trim(lrSentence.Sentence) & "'?",
                                            pcenumQuestionType.CreateFactType,
                                            pcenumExpectedResponseType.YesNo,
                                            Nothing,
                                            lrSentence,
                                            New FBM.ModelObject(lsFactTypeName, pcenumConceptType.FactType),
                                            lrPlan,
                                            lrStep,
                                            ,
                                            larAdditionalSentence)

                If abStraightToActionProcessing Then
                    Call Me.executeStatementAddFactType(lrQuestion, abBroadcastInterfaceEvent, abStraightToActionProcessing, arDSCError)
                ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                    Me.AddQuestion(lrQuestion)
                End If

                Me.send_data("Ok")
            End If

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

    Private Function FormulateQuestionsISAKINDOFStatement(ByVal asOriginalSentence As String,
                                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                                          Optional ByVal abStraightToActionProcessing As Boolean = False,
                                                          Optional ByRef arDSCError As DuplexServiceClient.DuplexServiceClientError = Nothing,
                                                          Optional ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject = Nothing) As Boolean

        Try
            Dim lsMessage As String = ""
            Dim lrPlan As New Brain.Plan 'The Plan formulated to create the SubtypeRelationship.

            Me.Model = prApplication.WorkingModel

            Me.VAQLProcessor.ISAKINDOFStatement = New VAQL.IsAKindOfStatement

            Call Me.VAQLProcessor.GetParseTreeTokensReflection(Me.VAQLProcessor.ISAKINDOFStatement, Me.VAQLParsetree.Nodes(0))


            For Each lsModelElementName In Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME

                If Me.Model.ExistsModelElement(lsModelElementName) Then
                    If Array.IndexOf({pcenumConceptType.EntityType},
                                      Me.Model.GetModelObjectByName(lsModelElementName).ConceptType) >= 0 Then
                        '-----------------------------------------------------------------------------
                        'A ObjectType already exists within the Model for the name lsModelElementName
                        '-----------------------------------------------------------------------------                        
                        Me.send_data("A Model Element already exists in the Model with the name, '" & lsModelElementName & "' of type " & Me.Model.GetModelObjectByName(lsModelElementName).ConceptType.ToString & ".")
                    End If
                Else
                    Call Me.createPlanStepForModelElement(lsModelElementName, lrPlan, True)
                End If
            Next

            Dim larModelObject As New List(Of FBM.ModelObject)
            Dim lrModelObject1 = Me.Model.GetModelObjectByName(Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(0))
            Dim lrModelObject2 = Me.Model.GetModelObjectByName(Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(1))
            larModelObject.Add(lrModelObject1)
            larModelObject.Add(lrModelObject2)

            Dim lrStep As New Brain.Step(pcenumActionType.CreateSubtypeRelationship,
                                         True,
                                         pcenumActionType.None,
                                         larModelObject,
                                         pcenumStepFactTypeAttributes.None
                                         )

            Dim lrSentence As New Language.Sentence(asOriginalSentence, asOriginalSentence)

            Dim lasSymbol As New List(Of String)
            lasSymbol.Add(Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(0))
            lasSymbol.Add(Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(1))

            Dim lrQuestion = New tQuestion("Would you like me to create the subtype relationship between " & Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(0) & " and " & Me.VAQLProcessor.ISAKINDOFStatement.MODELELEMENTNAME(1) & "'?",
                                           pcenumQuestionType.CreateSubtypeRelationship,
                                           pcenumExpectedResponseType.YesNo,
                                           lasSymbol,
                                           lrSentence,
                                           Nothing,
                                           lrPlan,
                                           lrStep,
                                           ,
                                           Nothing)

            lrQuestion.ModelObject = larModelObject

            If abStraightToActionProcessing Then
                Me.ProcessStatementCreateSubtypeRelationship(lrQuestion, abBroadcastInterfaceEvent)
            ElseIf Not Me.QuestionHasBeenRaised(lrQuestion) Then
                Me.AddQuestion(lrQuestion)
            End If

            Me.send_data("Ok.")

            Me.Timeout.Start()

            Return True

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return False
        End Try

    End Function

End Class
