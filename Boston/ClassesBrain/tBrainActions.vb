Imports System.Reflection

Partial Public Class tBrain

    Private Sub ProcessENTITYTYPEISIDENTIFIEDBYITSStatement()

        Try

            Me.Model = prApplication.WorkingModel


            Dim lrEntityType As FBM.EntityType
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance

            Me.VAQL.ENTITYTYPEISIDENTIFIEDBYITSStatement.MODELELEMENTNAME = ""
            Me.VAQL.ENTITYTYPEISIDENTIFIEDBYITSStatement.REFERENCEMODE = ""

            Call Me.VAQL.GetParseTreeTokensReflection(Me.VAQL.ENTITYTYPEISIDENTIFIEDBYITSStatement, Me.VAQLParsetree.Nodes(0))

            Dim lsEntityTypeName As String = Trim(Me.VAQL.ENTITYTYPEISIDENTIFIEDBYITSStatement.MODELELEMENTNAME)
            Dim lsActualModelElementName As String = ""

            If Me.Model.GetConceptTypeByNameFuzzy(lsEntityTypeName, lsActualModelElementName) = pcenumConceptType.EntityType Then
                '---------------------------------------------------------
                'Great! The name identified by the user is an EntityType
                '---------------------------------------------------------

                lsEntityTypeName = lsActualModelElementName

                Dim lrModelObject = Me.Model.GetModelObjectByName(lsEntityTypeName)
                lrEntityType = lrModelObject
            Else
                If Me.Model.ExistsModelElement(lsEntityTypeName) Then
                    Dim lrModelObject = Me.Model.GetModelObjectByName(lsEntityTypeName)
                    If lrModelObject.GetType IsNot GetType(FBM.EntityType) Then
                        Me.send_data(lsEntityTypeName & " is not an Entity Type")
                        Exit Sub
                    End If
                End If
                lrEntityType = Me.Model.CreateEntityType(lsEntityTypeName, True)

                If Me.Page IsNot Nothing Then
                    lrEntityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 100))
                End If

                'Call lrEntityTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                'Call lrEntityTypeInstance.Move(lrEntityTypeInstance.X, lrEntityTypeInstance.Y, True)
            End If

                Dim lsReferenceMode As String = Me.VAQL.ENTITYTYPEISIDENTIFIEDBYITSStatement.REFERENCEMODE

            Dim items As Array
            items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
            Dim item As pcenumReferenceModeEndings
            For Each item In items
                If lsReferenceMode.EndsWith(GetEnumDescription(item)) Then
                    lsReferenceMode = "." & lsReferenceMode
                    Exit For
                ElseIf lsReferenceMode.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                    lsReferenceMode = "." & lsReferenceMode
                    Exit For
                End If
            Next

            Call lrEntityType.SetReferenceMode(lsReferenceMode, False, Nothing, True)

            If lrEntityType.ReferenceModeValueType IsNot Nothing And lrEntityType.ReferenceModeFactType IsNot Nothing Then
                Call lrEntityType.ReferenceModeValueType.Save()
                Call lrEntityType.ReferenceModeFactType.Save()

                For Each lrInternalUniquenessConstraint In lrEntityType.ReferenceModeFactType.InternalUniquenessConstraint
                    Call lrInternalUniquenessConstraint.Save()
                Next
            End If

            Call lrEntityType.Save()

            Me.send_data("Ok")

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessStatementAddEntityType()

        Me.Model = prApplication.WorkingModel

        Dim lrEnityTypeInstance As FBM.EntityTypeInstance

        Dim lsOldEntityTypeName As String = ""
        Dim lsEntityTypeName As String = ""

        Me.Timeout.Stop()

        lsOldEntityTypeName = Me.CurrentQuestion.EntityType(0).Id
        lsEntityTypeName = Viev.Strings.MakeCapCamelCase(Me.CurrentQuestion.EntityType(0).Id)

        If IsSomething(Me.CurrentQuestion.sentence) Then
            Me.CurrentQuestion.sentence.Sentence = Me.CurrentQuestion.sentence.Sentence.Replace(lsOldEntityTypeName, lsEntityTypeName)
            Me.CurrentQuestion.sentence.ResetSentence()

            Call Language.AnalyseSentence(Me.CurrentQuestion.sentence, Me.Model)
            Call Language.ProcessSentence(Me.CurrentQuestion.sentence)
            If Me.CurrentQuestion.sentence.AreAllWordsResolved Then
                Call Language.ResolveSentence(Me.CurrentQuestion.sentence)
            End If

        End If

        Dim lrEntityType As FBM.EntityType

        lrEntityType = Me.Model.CreateEntityType(lsEntityTypeName, True)

        If Me.Page IsNot Nothing Then
            lrEnityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 10)) 'VM-20180329-Me.Page.Form.CreateEntityType(lsEntityTypeName, True)

            Call lrEnityTypeInstance.RepellFromNeighbouringPageObjects(1, False)
            Call lrEnityTypeInstance.Move(lrEnityTypeInstance.X, lrEnityTypeInstance.Y, True)

            If Me.AutoLayoutOn Then
                Me.Page.Form.AutoLayout()
            End If
        End If

        Me.Timeout.Start()

    End Sub

    Private Function executeStatementAddFactType() As Boolean

        Dim lrFactType As FBM.FactType
        Dim lrResolvedWord As Language.WordResolved
        Dim lrEntityType As FBM.EntityType
        Dim lrValueType As FBM.ValueType = Nothing
        Dim lrJoinedFactType As FBM.FactType = Nothing
        Dim larModelObject As New List(Of FBM.ModelObject)

        Try
            Me.Model = prApplication.WorkingModel

            executeStatementAddFactType = True

            For Each lrResolvedWord In Me.CurrentQuestion.sentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)
                lrEntityType = New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, Nothing, True)
                lrEntityType = Me.Model.EntityType.Find(AddressOf lrEntityType.Equals)

                If lrEntityType Is Nothing Then
                    lrValueType = New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrResolvedWord.Word, True)
                    lrValueType = Me.Model.ValueType.Find(AddressOf lrValueType.Equals)
                    If lrValueType Is Nothing Then
                        lrJoinedFactType = New FBM.FactType(Me.Model, lrResolvedWord.Word, True)
                        lrJoinedFactType = Me.Model.FactType.Find(AddressOf lrJoinedFactType.Equals)

                        If lrJoinedFactType.IsObjectified Then
                            larModelObject.Add(lrJoinedFactType)
                        Else
                            Dim lsMessage As String = ""
                            lsMessage = "The Fact Type, '" & lrJoinedFactType.Name & "', must be objectified before it can be involved in other Fact Types."
                            lsMessage &= vbCrLf & "Objectify the Fact Type and try again."
                            Me.OutputBuffer = lsMessage
                            Me.OutputChannel.BeginInvoke(New SendDataDelegate(AddressOf Me.send_data), Me.OutputBuffer)
                            Return False
                            Exit Function
                        End If
                    Else
                        larModelObject.Add(lrValueType)
                    End If
                Else
                    larModelObject.Add(lrEntityType)
                End If
            Next

            Dim lrModelObject As FBM.ModelObject
            Dim lsFactTypeName As String = ""

            For Each lrModelObject In larModelObject
                lsFactTypeName &= lrModelObject.Name
            Next

            '==========================================================================
            'Adding the FactType to the Model is done in the 'DropFactTypeAtPoint' stage, 
            '  which also broadcasts the event if in Client/Server mode. The below just creates the FactType ready for adding to the Model.
            lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False, True, , , False, )

            Dim larRole As New List(Of FBM.Role)

            For Each lrRole In lrFactType.RoleGroup
                larRole.Add(lrRole)
            Next

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim loPointF As PointF

            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, Me.CurrentQuestion.sentence)
            lrFactTypeReading.IsPreferred = True

            '====================================================================
            'Check to see that the FactType doesn't clash with another FactType
            Dim lrClashFactType = Me.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)
            If lrClashFactType IsNot Nothing Then
                Call Me.send_data("A Fact Type already exists with the Fact Type Reading, '" & lrFactTypeReading.GetReadingText & "'.")
                Return False
            End If

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, True)

            'Additional FactTypeReadings
            For Each lrSentence In Me.CurrentQuestion.AdditionalSentence

                larRole = New List(Of FBM.Role)
                For Each lrPredicatePart In lrSentence.PredicatePart
                    Dim lrPredicateRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrPredicatePart.ObjectName)
                    larRole.Add(lrPredicateRole)
                Next
                lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)
                Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, True)
            Next

            If lrFactType.MakeNameFromFactTypeReadings <> lrFactType.Id Then
                Call lrFactType.setName(lrFactType.MakeNameFromFactTypeReadings, False)
            End If

            If Me.Page Is Nothing Then
                Me.Model.AddFactType(lrFactType, True, True, Nothing)
            Else
                Dim lbAllObjectsOnPage As Boolean = True
                For Each lrRole In lrFactType.RoleGroup
                    If Not Me.Page.ContainsModelElement(lrRole.JoinedORMObject) Then
                        lbAllObjectsOnPage = False
                    End If
                Next

                If lbAllObjectsOnPage Then
                    loPointF = Me.Page.GetMidPointOfModelObjects(lrFactType.GetModelObjects)
                Else
                    loPointF = New PointF(100, 100)
                End If

                lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, loPointF, False, False, True)

                Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(1, False)

                Dim loPoint As New PointF(100, 100)
                If lrFactTypeInstance.Arity = 2 Then
                    Dim lrFirstModelObject, lrSecondModelObject As Object

                    lrFirstModelObject = lrFactTypeInstance.RoleGroup(0).JoinedORMObject
                    lrSecondModelObject = lrFactTypeInstance.RoleGroup(1).JoinedORMObject

                    loPoint.X = (lrFirstModelObject.X + lrSecondModelObject.X) / 2
                    loPoint.Y = (lrFirstModelObject.y + lrSecondModelObject.Y) / 2

                    lrFactTypeInstance.Move(loPoint.X, loPoint.Y, True)
                End If

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If

                Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                Call lrFactTypeInstance.Move(lrFactTypeInstance.X, lrFactTypeInstance.Y, True)

            End If

            Me.OutputChannel.Focus()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    ''' <summary>
    ''' Used when the ModelElements are already identified
    '''   e.g. When a Sentence of type 'Person has AT MOST ONE FirstName' is processed, the ModelElements are identified by the VAQL Parser.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function executeStatementAddFactTypePredetermined(ByRef arQuestion As tQuestion) As Boolean

        Dim lrFactType As FBM.FactType
        Dim lsResolvedModelElementName As String
        Dim lrModelObject As FBM.ModelObject
        Dim larModelObject As New List(Of FBM.ModelObject)

        Try
            Me.Model = prApplication.WorkingModel

            executeStatementAddFactTypePredetermined = True

            For Each lsResolvedModelElementName In arQuestion.FocalSymbol
                lrModelObject = Me.Model.GetModelObjectByName(lsResolvedModelElementName)
                If IsSomething(lrModelObject) Then
                    larModelObject.Add(lrModelObject)
                Else
                    executeStatementAddFactTypePredetermined = False
                    Exit Function
                End If
            Next

            Dim lsFactTypeName As String = ""

            For Each lrModelObject In larModelObject
                lsFactTypeName &= lrModelObject.Name
            Next

            lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False, True, , , True,  )

            Dim lrRole As FBM.Role
            Dim larRole As New List(Of FBM.Role)

            If arQuestion.PlanStep.FactTypeAttributes.Count > 0 Then

                If arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.MandatoryFirstRole) Then
                    Dim lsModelElementId As String = arQuestion.FocalSymbol(0)

                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementId)
                    lrRole.SetMandatory(True, True)
                End If

            End If

            '==========================================================================================
            'FactTypeReading            
            larRole.Clear()
            For Each lrRole In lrFactType.RoleGroup
                larRole.Add(lrRole)
            Next

            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, arQuestion.sentence)

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, True, True)

            If lrFactType.MakeNameFromFactTypeReadings <> lrFactType.Id Then
                lrFactType.setName(lrFactType.MakeNameFromFactTypeReadings)
            End If
            '===========================================================

            '===========================
            'RoleConstraint
            larRole = New List(Of FBM.Role)
            If arQuestion.PlanStep.FactTypeAttributes.Contains(pcenumStepFactTypeAttributes.ManyToOne) Then
                Dim lrRoleConstraint As FBM.RoleConstraint

                Dim lsModelElementId As String = arQuestion.FocalSymbol(0)

                lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lsModelElementId)
                larRole.Add(lrRole)

                lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                 larRole,
                                                                 "InternalUniquenessConstraint",
                                                                 1,
                                                                 False,
                                                                 False)

                lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                Me.Model.AddRoleConstraint(lrRoleConstraint, True, True)
            End If
            '===========================

            If Me.Page IsNot Nothing Then
                Dim lrFactTypeInstance As FBM.FactTypeInstance
                Dim loPointF As PointF

                Dim lbAllObjectsOnPage As Boolean = True
                For Each lrRole In lrFactType.RoleGroup
                    If Not Me.Page.ContainsModelElement(lrRole.JoinedORMObject) Then
                        lbAllObjectsOnPage = False
                    End If
                Next

                If lbAllObjectsOnPage Then
                    loPointF = Me.Page.GetMidPointOfModelObjects(lrFactType.GetModelObjects)
                Else
                    loPointF = New PointF(100, 100)
                End If

                lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, loPointF, False, False)

                Call lrFactTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                Call lrFactTypeInstance.Move(lrFactTypeInstance.X, lrFactTypeInstance.Y, True)

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Private Function ProcessStatementAddFactTypeReading() As Boolean

        Dim larModelObject As New List(Of FBM.ModelObject)
        Dim lrModelObject As FBM.ModelObject
        Dim lrFactType As FBM.FactType
        Dim lrResolvedWord As Language.WordResolved
        Dim lrRole As FBM.Role
        Dim larRole As New List(Of FBM.Role)

        Try
            Me.Model = prApplication.WorkingModel

            ProcessStatementAddFactTypeReading = True

            lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)

            For Each lrResolvedWord In Me.CurrentQuestion.sentence.WordListResolved.FindAll(Function(x) x.Sense = pcenumWordSense.Noun)

                lrModelObject = New FBM.ModelObject(lrResolvedWord.Word, pcenumConceptType.ValueType)
                lrRole = New FBM.Role(lrFactType, New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrModelObject.Id, True))
                lrFactType.RoleGroup.Add(lrRole)
                larModelObject.Add(lrModelObject)
            Next

            lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.EqualsByModelElements)

            Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, Me.CurrentQuestion.sentence)

            Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, True)

            If Me.Page IsNot Nothing Then

                Dim lrFactTypeInstance As New FBM.FactTypeInstance
                lrFactTypeInstance = lrFactType.CloneInstance(Me.Page, False)
                lrFactTypeInstance = Me.Page.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)

                Call lrFactTypeInstance.SetSuitableFactTypeReading()
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Private Sub ProcessStatementAddValueType()

        Dim lrValueTypeInstance As FBM.ValueTypeInstance

        Dim lsOldValueTypeName As String = ""
        Dim lsValueTypeName As String = ""

        Me.Timeout.Stop()

        Me.Model = prApplication.WorkingModel

        lsOldValueTypeName = Me.CurrentQuestion.ValueType(0).Id
        lsValueTypeName = Viev.Strings.MakeCapCamelCase(Me.CurrentQuestion.ValueType(0).Id)

        If Me.CurrentQuestion.sentence IsNot Nothing Then
            Me.CurrentQuestion.sentence.Sentence = Me.CurrentQuestion.sentence.Sentence.Replace(lsOldValueTypeName, lsValueTypeName)
            Me.CurrentQuestion.sentence.ResetSentence()

            Call Language.AnalyseSentence(Me.CurrentQuestion.sentence, Me.Model)
            Call Language.ProcessSentence(Me.CurrentQuestion.sentence)
            If Me.CurrentQuestion.sentence.AreAllWordsResolved Then
                Call Language.ResolveSentence(Me.CurrentQuestion.sentence)
            End If
        End If

        Dim lrValueType As FBM.ValueType

        lrValueType = Me.Model.CreateValueType(lsValueTypeName, False)

        If Me.Page IsNot Nothing Then

            lrValueTypeInstance = Me.Page.DropValueTypeAtPoint(lrValueType, New PointF(100, 100)) 'VM-20181329-Remove this commented-out code, if all okay. Me.Page.Form.CreateValueType(lsValueTypeName, True)

            Call lrValueTypeInstance.RepellFromNeighbouringPageObjects(1, False)
            Call lrValueTypeInstance.Move(lrValueTypeInstance.X, lrValueTypeInstance.Y, True)

            If Me.AutoLayoutOn Then
                Me.Page.Form.AutoLayout()
            End If
        Else
            Me.Model.AddValueType(lrValueType, True, True, Nothing)
        End If
        Me.Timeout.Start()

    End Sub

    Private Sub ProcessISANENTITYTYPECLAUSE()

        Try
            With New WaitCursor
                Me.Model = prApplication.WorkingModel

                Me.VAQL.ISANENTITYTYPEStatement.KEYWDISANENTITYTYPE = ""
                Me.VAQL.ISANENTITYTYPEStatement.MODELELEMENTNAME = ""

                Call Me.VAQL.GetParseTreeTokensReflection(Me.VAQL.ISANENTITYTYPEStatement, Me.VAQLParsetree.Nodes(0))

                Me.Timeout.Stop()

                Dim lsEntityTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQL.ISANENTITYTYPEStatement.MODELELEMENTNAME))

                If Me.Model.ExistsModelElement(lsEntityTypeName) Then
                    Me.send_data("There is already a Model Element with the name, '" & lsEntityTypeName & "'. Try another name")
                    Exit Sub
                End If

                If Me.Model.ModelDictionary.Find(Function(x) x.Symbol = lsEntityTypeName And x.isEntityType) IsNot Nothing Then
                    Me.send_data("I know.")
                    Exit Sub
                End If

                'Have already checked to see wither it is okay to create the EntityType above.
                Dim lrEntityType = Me.Model.CreateEntityType(Trim(lsEntityTypeName), True)

                If Me.Page IsNot Nothing Then
                    Dim lrEnityTypeInstance = Me.Page.DropEntityTypeAtPoint(lrEntityType, New PointF(100, 10)) 'VM-20180329-Me.Page.Form.CreateEntityType(lsEntityTypeName, True)

                    Call lrEnityTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                    Call lrEnityTypeInstance.Move(lrEnityTypeInstance.X, lrEnityTypeInstance.Y, True)

                    If Me.AutoLayoutOn Then
                        Me.Page.Form.AutoLayout()
                    End If
                End If

                Me.send_data("Ok")

                Me.Timeout.Start()
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessISWHEREStatement(ByVal asOriginalSentence As String)

        Try
            Dim lsMessage As String = ""
            Dim lrPlan As New Brain.Plan 'The Plan formulated to create the FactType.
            Dim lrStep As Brain.Step 'For Steps added to the Plan.
            Dim lrPredicatePart As New Language.PredicatePart
            Dim lrQuestion As tQuestion
            Dim lrSentence = New Language.Sentence(asOriginalSentence, asOriginalSentence)

            Me.Model = prApplication.WorkingModel

            Me.VAQL.ISWHEREStatement = New VAQL.IsWhereStatement

            Call Me.VAQL.GetParseTreeTokensReflection(Me.VAQL.ISWHEREStatement, Me.VAQLParsetree.Nodes(0))

            '===================================================================
            'Validate the IS WHERE Statement
            If Me.VAQL.ISWHEREStatement.FACTTYPESTMT.Count > 1 Then
                For liInd = 1 To Me.VAQL.ISWHEREStatement.FACTTYPESTMT.Count - 1
                    If Me.VAQL.ISWHEREStatement.FACTTYPESTMT(liInd).MODELELEMENT.Count <> Me.VAQL.ISWHEREStatement.FACTTYPESTMT(0).MODELELEMENT.Count Then

                        lsMessage = "That is an incorrect IS WHERE statement. Each Fact Type Reading must have the same number of Model Elements."
                        Me.send_data(lsMessage)
                        lsMessage = "'" & Me.VAQL.ISWHEREStatement.FACTTYPESTMT(liInd).makeSentence & "' does not have the same number of Model Elements as '" & Me.VAQL.ISWHEREStatement.FACTTYPESTMT(0).makeSentence & "'."
                        Me.send_data(lsMessage)
                        Exit Sub
                    End If
                Next
            End If
            '===================================================================

            Me.Timeout.Stop()

            'Check to see if the FactTypeName already exists
            Dim lsFactTypeName = Me.VAQL.ISWHEREStatement.MODELELEMENT(0).MODELELEMENTNAME

            If Me.Model.ExistsModelElement(lsFactTypeName) Then
                If Array.IndexOf({pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType},
                                  Me.Model.GetModelObjectByName(lsFactTypeName).ConceptType) >= 0 Then
                    '-----------------------------------------------------------------------------
                    'A ObjectType already exists within the Model for the name lsModelObjectName
                    '-----------------------------------------------------------------------------
                    Me.send_data("A Model Element already exists in the Model with the name, '" & lsFactTypeName & "'.")
                Else

                End If
            Else
                'Get the ModelElements for the overall FactType. Only need to get them for the first FactTypeStatement.
                '  NB Subsequent FactTypeStatements in the ISWHERE Statement must have matching ModelElementNames.

                Dim lrFactTypeStatement = Me.VAQL.ISWHEREStatement.FACTTYPESTMT(0)
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

                lrStep = New Brain.Step(pcenumActionType.CreateFactType, True, pcenumActionType.None)
                lrPlan.AddStep(lrStep)

                '===========================================================
                'Additional FactTypeReadings/Sentences
                Dim larAdditionalSentence As New List(Of Language.Sentence)
                If Me.VAQL.ISWHEREStatement.FACTTYPESTMT.Count > 1 Then

                    For liFactTypeStatement = 1 To Me.VAQL.ISWHEREStatement.FACTTYPESTMT.Count - 1

                        Dim lrAdditionalSentence = New Language.Sentence("", "")

                        lrFactTypeStatement = Me.VAQL.ISWHEREStatement.FACTTYPESTMT(liFactTypeStatement)

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
                                            True,
                                            Nothing,
                                            lrSentence,
                                            Nothing,
                                            lrPlan,
                                            lrStep,
                                            ,
                                            larAdditionalSentence)

                If Not Me.QuestionHasBeenRaised(lrQuestion) Then
                    Me.AddQuestion(lrQuestion)
                End If

                Me.send_data("Ok")
            End If

            Me.Timeout.Start()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub createPlanStepForModelElement(ByVal asModelObjectName As String, ByRef arPlan As Brain.Plan)

        Dim lrQuestion As tQuestion
        Dim lrStep As Brain.Step 'For Steps added to the Plan.

        Try
            Dim lbProceedToCreateObjectTypeQuestion As Boolean = False

            If Me.Model.ExistsModelElement(asModelObjectName) Then
                If Array.IndexOf({pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType},
                                  Me.Model.GetModelObjectByName(asModelObjectName).ConceptType) >= 0 Then
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
                Dim item As pcenumReferenceModeEndings
                For Each item In items
                    If asModelObjectName.EndsWith(GetEnumDescription(item)) Then
                        lbIsLikelyValueType = True
                        Exit For
                    ElseIf asModelObjectName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                        lbIsLikelyValueType = True
                        Exit For
                    End If
                Next

                If lbIsLikelyValueType Then
                    lrStep = New Brain.Step(pcenumActionType.CreateValueType, True, pcenumActionType.None)
                    arPlan.AddStep(lrStep)

                    Dim lasSymbol As New List(Of String)
                    lasSymbol.Add(asModelObjectName)

                    lrQuestion = New tQuestion("Would you like me to create an Value Type for '" & asModelObjectName & "'?",
                                                         pcenumQuestionType.CreateValueType,
                                                         True,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         arPlan,
                                                         lrStep)
                Else
                    lrStep = New Brain.Step(pcenumActionType.CreateEntityType, True, pcenumActionType.CreateValueType)
                    arPlan.AddStep(lrStep)

                    Dim lasSymbol As New List(Of String)
                    lasSymbol.Add(asModelObjectName)

                    lrQuestion = New tQuestion("Would you like me to create an Entity Type for '" & asModelObjectName & "'? (Answer 'No' and I'll ask you if you want a Value Type)",
                                                         pcenumQuestionType.CreateEntityType,
                                                         True,
                                                         lasSymbol,
                                                         Nothing,
                                                         Nothing,
                                                         arPlan,
                                                         lrStep)
                End If


                If Me.QuestionHasBeenRaised(lrQuestion) Then
                    '------------------------------------------------------------
                    'Great, already asked the question and am awaiting responce
                    '------------------------------------------------------------
                Else
                    Me.AddQuestion(lrQuestion)
                End If

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub ProcessVALUETYPEISWRITTENASStatement()

        Me.Model = prApplication.WorkingModel

        Me.VAQL.VALUETYPEISWRITTENASStatement.MODELELEMENTNAME = ""
        Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPE = New Object
        Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPELENGTH = New Object
        Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION = New Object
        Me.VAQL.VALUETYPEISWRITTENASStatement.NUMBER = ""

        Call Me.VAQL.GetParseTreeTokensReflection(Me.VAQL.VALUETYPEISWRITTENASStatement, Me.VAQLParsetree.Nodes(0))

        Dim lrValueTypeInstance As FBM.ValueTypeInstance
        Dim lsValueTypeName As String = ""
        Dim lsDataTypeName As String = ""
        Dim liDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet

        lsValueTypeName = Trim(Viev.Strings.MakeCapCamelCase(Me.VAQL.VALUETYPEISWRITTENASStatement.MODELELEMENTNAME))

        Dim liDataTypeLength As Integer = 0
        Dim liDataTypePrecision As Integer = 0

        If Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPE.GetType Is GetType(VAQL.ParseNode) Then
            lsDataTypeName = Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPE.Nodes(0).Token.Text
        ElseIf Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPELENGTH.GetType Is GetType(VAQL.ParseNode) Then
            lsDataTypeName = Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPELENGTH.Nodes(0).Token.Text
            liDataTypeLength = CInt(Me.VAQL.VALUETYPEISWRITTENASStatement.NUMBER)
        ElseIf Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION.GetType Is GetType(VAQL.ParseNode) Then
            lsDataTypeName = Me.VAQL.VALUETYPEISWRITTENASStatement.DATATYPEPRECISION.Nodes(0).Token.Text
            liDataTypePrecision = CInt(Me.VAQL.VALUETYPEISWRITTENASStatement.NUMBER)
        End If

        lsDataTypeName = DataTypeAttribute.Get(GetType(pcenumORMDataType), lsDataTypeName)
        If lsDataTypeName Is Nothing Then
            Me.send_data("That's not a valid Data Type.")
            Exit Sub
        End If

        Try
            liDataType = DirectCast([Enum].Parse(GetType(pcenumORMDataType), lsDataTypeName), pcenumORMDataType)
        Catch ex As Exception
            Me.send_data("That's not a valid Data Type.")
            Exit Sub
        End Try

        Dim lsActualModelElementName As String = ""

        If Me.Model.GetConceptTypeByNameFuzzy(lsValueTypeName, lsActualModelElementName) = pcenumConceptType.ValueType Then

            Dim lrValueType As FBM.ValueType
            Dim liInitialDataType As pcenumORMDataType

            lrValueType = Me.Model.GetModelObjectByName(lsActualModelElementName)
            liInitialDataType = lrValueType.DataType

            Dim lsInitialDataType = liInitialDataType.ToString
            If lrValueType.DataTypeLength > 0 Then
                lsInitialDataType &= "(" & lrValueType.DataTypeLength.ToString
                If lrValueType.DataTypePrecision > 0 Then
                    lsInitialDataType &= "," & lrValueType.DataTypePrecision.ToString
                End If
                lsInitialDataType &= ")"
            End If

            Call lrValueType.SetDataType(liDataType, liDataTypeLength, liDataTypePrecision, True)

            Dim lsNewDataType = liDataType.ToString
            If liDataTypeLength > 0 Then
                lsNewDataType &= "(" & liDataTypeLength.ToString
                If liDataTypePrecision > 0 Then
                    lsNewDataType &= "," & liDataTypePrecision.ToString
                End If
                lsNewDataType &= ")"
            End If

            If Me.AutoLayoutOn Then
                Me.Page.Form.AutoLayout()
            End If

            Me.send_data("Thank you. I changed the DataType of Value Type, '" & lsActualModelElementName & "' from " & lsInitialDataType & " to " & lsNewDataType & ".", False)
        Else

            Me.Timeout.Stop()

            Dim lrValueType As FBM.ValueType
            lrValueType = Me.Model.CreateValueType(lsValueTypeName, True, liDataType, liDataTypeLength, liDataTypePrecision)

            If Me.Page IsNot Nothing Then

                lrValueTypeInstance = Me.Page.DropValueTypeAtPoint(lrValueType, New PointF(100, 100))
                Call lrValueTypeInstance.RepellFromNeighbouringPageObjects(1, False)
                Call lrValueTypeInstance.Move(lrValueTypeInstance.X, lrValueTypeInstance.Y, True)

                If Me.AutoLayoutOn Then
                    Me.Page.Form.AutoLayout()
                End If
            End If
            Me.Timeout.Start()

        End If

    End Sub

End Class
