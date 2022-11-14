Namespace FEQL
    Partial Public Class Processor

#Region "ProcessWHICHSELECTStatementNew"
        Public Function ProcessWHICHSELECTStatementNew(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ORMQL.Recordset
            lrRecordset.StatementType = FactEngine.Constants.pcenumFEQLStatementType.WHICHSELECTStatement

            Try
                Dim lrQueryGraph As New FactEngine.QueryGraph(Me.Model)

                lrQueryGraph = Me.getQueryGraph()

                '==========================================================================
                'Get the records
                'CodeSafe 
                If Me.DatabaseManager.Connection Is Nothing Then
                    Call Me.Model.connectToDatabase()
                End If

                Select Case Me.Model.TargetDatabaseType
                    Case Is = pcenumDatabaseType.TypeDB
                        lsSQLQuery = lrQueryGraph.generateTypeQL(Me.WHICHSELECTStatement)
                    Case Is = pcenumDatabaseType.Neo4j
                        lsSQLQuery = lrQueryGraph.generateCypher(Me.WHICHSELECTStatement)
                    Case Else
                        lsSQLQuery = lrQueryGraph.generateSQL(Me.WHICHSELECTStatement)
                End Select


                If Me.DatabaseManager.Connection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(prApplication.WorkingModel.TargetDatabaseType, prApplication.WorkingModel.TargetDatabaseConnectionString)
                    If Me.DatabaseManager.Connection Is Nothing Then
                        Throw New Exception("No database connection has been established.")
                    End If
                ElseIf Me.DatabaseManager.Connection.Connected = False Then
                    Throw New Exception("The database is not connected.")
                End If

                Dim lrTestRecordset = Me.DatabaseManager.GO(lsSQLQuery)
                lrTestRecordset.Warning = lrQueryGraph.Warning
                lrTestRecordset.QueryGraph = lrQueryGraph

                If Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDDID) Then
                    lrTestRecordset.StatementType = FactEngine.Constants.pcenumFEQLStatementType.DIDStatement
                End If

                Return lrTestRecordset


            Catch appex As ApplicationException

                If appex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = appex.Message
                    lrRecordset.Query = lsSQLQuery
                Else
                    lrRecordset.ErrorString = appex.InnerException.Message
                End If
                lrRecordset.ApplicationException = appex
                Return lrRecordset

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = ex.Message
                    lrRecordset.Query = lsSQLQuery
                Else
                    lrRecordset.ErrorString = ex.InnerException.Message
                End If

                Return lrRecordset
            End Try

        End Function

#End Region

#Region "getQueryGraph"

        Public Function getQueryGraph(Optional ByRef arWHICHSelectStatement As FEQL.WHICHSELECTStatement = Nothing) As FactEngine.QueryGraph

            Dim lrQueryGraph As New FactEngine.QueryGraph(Me.Model)

            Try
                If arWHICHSelectStatement Is Nothing Then
                    Me.WHICHSELECTStatement = New FEQL.WHICHSELECTStatement
                    Call Me.GetParseTreeTokensReflection(Me.WHICHSELECTStatement, Me.Parsetree.Nodes(0))
                Else
                    Me.WHICHSELECTStatement = arWHICHSelectStatement
                End If

                '----------------------------------------
                'Create the HeadNode for the QueryGraph
                Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause

                'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHSELECTStatement.MODELELEMENT(0)) '
                Dim lrFBMModelObject As FBM.ModelObject
                If Me.Parsetree.Nodes(0).Nodes(0).Nodes(0).Token.Type = TokenType.NODEPROPERTYIDENTIFICATION Then
                    If Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION(0).MODELELEMENTNAME = Me.WHICHSELECTStatement.MODELELEMENTNAME(0) Then
                        lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION(0).MODELELEMENTNAME) 'MODELELEMENTNAME
                    Else
                        lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHSELECTStatement.MODELELEMENTNAME(0))
                    End If
                Else
                    lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHSELECTStatement.NODE(0).MODELELEMENTNAME) 'MODELELEMENTNAME

                    If lrFBMModelObject Is Nothing Then
                        Dim lsModelObjectName As String = Me.WHICHSELECTStatement.NODE(0).MODELELEMENTNAME
                        Dim larModelObject = From ModelObject In Me.Model.getModelObjects
                                             Select New With {ModelObject, .Lev = Fastenshtein.Levenshtein.Distance(ModelObject.Id, lsModelObjectName)}

                        lrFBMModelObject = larModelObject.OrderBy(Function(X) X.Lev).First.ModelObject
                    End If

                End If

                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHSELECTStatement.MODELELEMENTNAME(0) & "'.")
                lrQueryGraph.HeadNode = New FactEngine.QueryNode(lrFBMModelObject)

                If Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION.Count > 0 Then
                    If Me.WHICHSELECTStatement.MODELELEMENTNAME.Count > 1 Then
                        Me.WHICHCLAUSE = New FEQL.WHICHCLAUSE
                        Call Me.GetParseTreeTokensReflection(Me.WHICHCLAUSE, Me.WHICHSELECTStatement.WHICHCLAUSE(0))
                        Select Case Me.getWHICHClauseType(Me.WHICHCLAUSE, Me.WHICHSELECTStatement.WHICHCLAUSE(0))
                            Case Is = FactEngine.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                            Case Else
                                lrQueryGraph.HeadNode.Alias = Me.WHICHSELECTStatement.NODE(0).MODELELEMENTSUFFIX
                        End Select

                    Else
                        lrQueryGraph.HeadNode.Alias = Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION(0).MODELELEMENTSUFFIX
                    End If

                Else
                    lrQueryGraph.HeadNode.Alias = Me.WHICHSELECTStatement.NODE(0).MODELELEMENTSUFFIX
                End If

                If Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION.Count > 0 Then
                    If Me.WHICHSELECTStatement.NODE.Count <> Me.WHICHSELECTStatement.MODELELEMENTNAME.Count Then
                        If Me.Parsetree.Nodes(0).Nodes(0).Nodes(0).Token.Type = TokenType.NODEPROPERTYIDENTIFICATION Then
                            For Each lsIdentifier In Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION(0).IDENTIFIER
                                lrQueryGraph.HeadNode.IdentifierList.Add(lsIdentifier)
                            Next
                            lrQueryGraph.HeadNode.HasIdentifier = True
                        End If
                        If lrQueryGraph.HeadNode.Alias Is Nothing And Me.WHICHSELECTStatement.MODELELEMENTNAME.Count = 1 Then
                            lrQueryGraph.HeadNode.Alias = Me.WHICHSELECTStatement.NODEPROPERTYIDENTIFICATION(0).MODELELEMENTSUFFIX
                        End If
                    End If
                End If

                If Me.WHICHSELECTStatement.NODE.Count > 0 Then
                    If Me.WHICHSELECTStatement.NODE(0).NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                        lrQueryGraph.HeadNode.HasIdentifier = True
                        For Each lsIdentifier In Me.WHICHSELECTStatement.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                            lrQueryGraph.HeadNode.IdentifierList.Add(lsIdentifier)
                        Next
                    End If
                End If

                lrQueryGraph.Nodes.Add(lrQueryGraph.HeadNode)

                '----------------------------------
                'Get the Edges for the QueryGraph
                Dim lrPreviousTargetNode As FactEngine.QueryNode = Nothing
                Dim lrPreviousTopicNode As FactEngine.QueryNode = Nothing

                For Each loWhichClause In Me.WHICHSELECTStatement.WHICHCLAUSE

                    Me.WHICHCLAUSE = New FEQL.WHICHCLAUSE
                    Call Me.GetParseTreeTokensReflection(Me.WHICHCLAUSE, loWhichClause)

                    '==============================
                    'Create the QueryEdge
                    Dim lrQueryEdge As New FactEngine.QueryEdge(lrQueryGraph, Me.WHICHCLAUSE)

                    '---------------------------------------------------------
                    'Get the Predicate. Every which clause has a Predicate.
                    For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                        lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                    Next

                    '============================================
                    'Example Fact Engine Query
                    '--------------------------
                    'WHICH Lecturer is located in WHICH Room 
                    'AND holds WHICH Position
                    'AND is in WHICH School
                    'AND is in A School WHICH is in (Factulty:'IT') 
                    'AND THAT Lecturer works for THAT Faculty 
                    '------------------------------------------

                    'CodeSafe
                    If lrQueryGraph.QueryEdges.Count = 0 Then
                        lrPreviousTargetNode = lrQueryGraph.HeadNode
                    End If

                    Select Case Me.getWHICHClauseType(Me.WHICHCLAUSE, loWhichClause)
                        Case Is = FactEngine.pcenumWhichClauseType.None
                            Throw New Exception("Unknown WhichClauseType.")
                        Case Is = FactEngine.pcenumWhichClauseType.WhichPredicateNodePropertyIdentification '     1. E.g. WHICH has (emailAddress:'hamish.bentley@qut.com.au)
                            Call Me.analyseWhichPredicateNodePropertyIdentification(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.PredicateWHICHModelElement  '                  2. E.g. occupies WHICH Room
                            Call Me.AnlysePredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatPredicateThatModelElement  '            3. E.g. AND THAT Faculty has THAT School
                            Call Me.analyseANDThatPredicateThatModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement  '4. E.g. AND THAT School is in THAT Faculty
                            Call Me.analyseAndThatModelElementPredicateThatModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.UnkownPredicateWhichModelElement  '            5. E.g. has WHICH RoomName
                            Call Me.analysePredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndPredicateWhichModelElement  '               6. E.g. AND has WHICH RoomName
                            Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTopicNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndModelElementPredicateWhichModelElement '6.1 E.g. AND Employee 2 reports to WHICH Employee 3
                            Call Me.analyseANDModelElementWHICHCModelElementlause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateWhichModelElement '7. E.g. AND THAT Faculty has FacultyName
                            Call Me.analyseANDTHATWHICHClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateModelElement '     8. E.g. AND THAT Employee 2 reports to Employee 3. NB Can be ambiguous because could be AND THAT Employee 2 reports to THAT Employee 3
                            Call Me.analyseANDTHATClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        Case Is = FactEngine.pcenumWhichClauseType.AndWhichPredicateNodePropertyIdentification  ' 9. E.g. AND WHICH is in (Faculty:IT')
                            Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.WhichPredicateThatModelElement ' 10. E.g. WHICH houses THAT Lecturer
                            Call Me.analyseWhichPredicateThatClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.ThatPredicateWhichModelElement '11. E.g. THAT houses WHICH Lecturer
                            Call Me.analyseThatPredicateWhichClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.WithClause  '12. E.g. WITH WHAT Rating
                            Call Me.analystWITHClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatIdentityCompatitor  '13. E.g. "Person 1 IS NOT Person 2" or "Person 1 IS Person 2"
                            Call Me.analystISNOTClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        Case Is = FactEngine.pcenumWhichClauseType.BooleanPredicate  '14. E.g. Protein is enzyme
                            Call Me.analystBooleanPredicateClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.ThatModelElementPredicate  '15. E.g. THAT James Dean Played in (as in "SHOW ME Movies THAT James Dean played in")
                            Call Me.analyseThatModelElementPredicate(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Else 'CodeSafe
                            Throw New Exception("Unknown WhichClauseType.")

                    End Select

                    If lrQueryEdge.BaseNode.QueryEdge Is Nothing Then
                        lrQueryEdge.BaseNode.QueryEdge = lrQueryEdge
                    End If

                    '-------------------------------------------------
                    'Add the QueryEdge to the QueryGraph
                    lrQueryGraph.QueryEdges.Add(lrQueryEdge)
                    If lrQueryEdge.InjectsQueryEdge IsNot Nothing Then
                        If lrQueryEdge.InjectsQueryEdge.TargetNode.HasIdentifier Then
                            lrQueryEdge.InjectsQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                        Else
                            lrQueryEdge.InjectsQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
                            lrQueryEdge.IsProjectColumn = True
                        End If
                        Select Case lrQueryEdge.InjectsQueryEdge.WhichClauseType
                            Case Is = FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
                                lrQueryEdge.InjectsQueryEdge.IsProjectColumn = True
                        End Select

                        If Me.WHICHCLAUSE.MATHCLAUSE IsNot Nothing Then
                            lrQueryEdge.InjectsQueryEdge.TargetNode.MathFunction = Boston.GetEnumFromDescriptionAttribute(Of pcenumMathFunction)(Me.WHICHCLAUSE.MATHCLAUSE.MATHFUNCTION)
                            If Me.WHICHCLAUSE.MATHCLAUSE.NUMBER IsNot Nothing Then
                                lrQueryEdge.InjectsQueryEdge.TargetNode.MathNumber = CDbl(Me.WHICHCLAUSE.MATHCLAUSE.NUMBER)
                            End If
                        End If

                        lrQueryGraph.QueryEdges.Add(lrQueryEdge.InjectsQueryEdge)
                        lrQueryGraph.Nodes.AddUnique(lrQueryEdge.InjectsQueryEdge.BaseNode)

                        lrPreviousTargetNode = lrQueryEdge.InjectsQueryEdge.TargetNode
                        lrPreviousTopicNode = lrQueryEdge.InjectsQueryEdge.BaseNode

                    Else
                        lrPreviousTargetNode = lrQueryEdge.TargetNode
                        lrPreviousTopicNode = lrQueryEdge.BaseNode
                    End If



                    '-------------------------------------------------
                    'PartialFactTypeMatch processing
                    If lrQueryGraph.QueryEdges.Count > 1 And lrQueryEdge.IsPartialFactTypeMatch Then
                        If Not lrQueryEdge.AmbiguousFactTypeMatches.Count > 0 Then
                            Dim lrPreviousQueryEdge = lrQueryEdge.GetPreviousQueryEdge
                            If lrPreviousQueryEdge.AmbiguousFactTypeMatches.Contains(lrQueryEdge.FBMFactType) Then
                                lrPreviousQueryEdge.FBMFactType = lrQueryEdge.FBMFactType
                                lrPreviousQueryEdge.FBMFactTypeReading = lrQueryEdge.FBMFactType.FactTypeReading.Find(AddressOf lrPreviousQueryEdge.AmbiguousFactTypeReading.EqualsPartiallyByPredicatePartText)
                                If lrQueryEdge.FBMFactTypeReading Is Nothing Then
                                    If lrPreviousQueryEdge.FBMFactTypeReading IsNot Nothing Then
                                        lrQueryEdge.FBMFactTypeReading = lrPreviousQueryEdge.FBMFactTypeReading
                                        lrPreviousQueryEdge.ErrorMessage = Nothing
                                    Else
                                        Throw New Exception(lrPreviousQueryEdge.ErrorMessage)
                                    End If
                                Else
                                    'PredicatePart
                                    lrPreviousQueryEdge.FBMPredicatePart = (From PredicatePart In lrPreviousQueryEdge.FBMFactTypeReading.PredicatePart
                                                                            Where PredicatePart.Role.JoinedORMObject.Id = lrPreviousQueryEdge.BaseNode.Name
                                                                            Where PredicatePart.PredicatePartText = lrPreviousQueryEdge.Predicate
                                                                            Select PredicatePart
                                                                            ).First

                                    lrPreviousQueryEdge.ErrorMessage = Nothing
                                End If
                            Else
                                If lrQueryEdge.FBMFactTypeReading Is Nothing And lrPreviousQueryEdge.IsPartialFactTypeMatch Then
                                    lrQueryEdge.FBMFactTypeReading = lrPreviousQueryEdge.FBMFactTypeReading
                                End If
                            End If
                        End If
                    End If

                Next

                'Boston.WriteToStatusBar("Generating SQL", True)                
                Dim larQueryEdgeTypes = {FactEngine.pcenumWhichClauseType.ISClause, FactEngine.pcenumWhichClauseType.ISNOTClause}

                'Try and fix ambiguities for PartialFactTypeMatch queries.
                For liInd = lrQueryGraph.QueryEdges.Count - 1 To 0 Step -1
                    If liInd > 0 Then
                        Dim lrPreviousQueryEdge As FactEngine.QueryEdge = lrQueryGraph.QueryEdges(liInd).GetPreviousQueryEdge
                        If lrPreviousQueryEdge.IsPartialFactTypeMatch And lrPreviousQueryEdge.FBMFactType Is lrQueryGraph.QueryEdges(liInd).FBMFactType Then
                            If lrPreviousQueryEdge.AmbiguousFactTypeMatches.Count > 0 Then
                                lrPreviousQueryEdge.AmbiguousFactTypeMatches.Clear()
                                lrPreviousQueryEdge.ErrorMessage = Nothing

                                If lrPreviousQueryEdge.FBMPredicatePart Is Nothing Then
                                    Call lrPreviousQueryEdge.getAndSetPredicatePart()
                                End If
                            End If

                        End If
                    End If
                Next

                Dim larErroredQueryEdges = From QueryEdge In lrQueryGraph.QueryEdges
                                           Where QueryEdge.ErrorMessage IsNot Nothing Or (QueryEdge.FBMFactType Is Nothing And Not larQueryEdgeTypes.Contains(QueryEdge.WhichClauseSubType))
                                           Select QueryEdge

                If larErroredQueryEdges.Count > 0 Then
                    Dim lsMessage = "Error: " & larErroredQueryEdges.First.ErrorMessage
                    Throw New Exception(lsMessage)
                End If


                Call lrQueryGraph.checkNodeAliases()

                Return lrQueryGraph

            Catch appex As ApplicationException
                Dim lsMessage As String = appex.Message
                If My.Settings.ShowStackTraceFactEngineQuery Then lsMessage.AppendDoubleLineBreak(appex.StackTrace)
                Dim lrApplicationException As New ApplicationException(lsMessage)
                If appex.Data.Contains("QueryEdgeGetFBMFactTypeFail") Then
                    lrApplicationException.Data.Add("QueryEdgeGetFBMFactTypeFail", appex.Data.Item("QueryEdgeGetFBMFactTypeFail"))
                End If
                Throw lrApplicationException
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function
#End Region

#Region "getModelElmentByBaseNodePredicate"
        Private Function getModelElmentByBaseNodePredicate(ByVal arBaseNode As FactEngine.QueryNode,
                                                           ByVal asPredicate As String) As FBM.ModelObject
            Try

                Dim larPRedicatePart = From FactType In Me.Model.FactType
                                       From FactTypeReading In FactType.FactTypeReading
                                       From PredicatePart In FactTypeReading.PredicatePart
                                       Where PredicatePart.Role.JoinedORMObject.Id = arBaseNode.Name
                                       Where PredicatePart.PredicatePartText = asPredicate
                                       Select PredicatePart

                If larPRedicatePart.Count = 1 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = larPRedicatePart.First.FactTypeReading
                    Try
                        Dim lrPredicatePart As FBM.PredicatePart = lrFactTypeReading.PredicatePart(larPRedicatePart.First.SequenceNr)
                        Return lrPredicatePart.Role.JoinedORMObject
                    Catch
                        Return Nothing
                    End Try
                Else
                    Try
                        Dim larReducedPredicatePart = From PredicatePart In larPRedicatePart
                                                      Where PredicatePart.FactTypeReading.PredicatePart(1).Role.JoinedORMObject.GetType <> GetType(FBM.FactType)
                                                      Select PredicatePart

                        If larReducedPredicatePart.Count = 1 Then
                            Dim lrFactTypeReading As FBM.FactTypeReading = larReducedPredicatePart.First.FactTypeReading
                            Try
                                Dim lrPredicatePart As FBM.PredicatePart = lrFactTypeReading.PredicatePart(larReducedPredicatePart.First.SequenceNr)
                                Return lrPredicatePart.Role.JoinedORMObject
                            Catch
                                Return Nothing
                            End Try
                        Else
                            Return Nothing
                        End If
                    Catch ex As Exception
                        Return Nothing
                    End Try
                End If

                Return Nothing

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

#End Region

        '1
#Region "analysePredicatePropertyNodeIdentification"
        Private Sub analyseWhichPredicateNodePropertyIdentification(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                                    ByRef arQueryGraph As FactEngine.QueryGraph,
                                                                    ByRef arQueryEdge As FactEngine.QueryEdge,
                                                                    ByRef arPreviousTargetNode As FactEngine.QueryNode)

            Dim lrFBMModelObject As FBM.ModelObject

            'E.g. "WHICH is in (Factulty:'IT') "
            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.WhichPredicateNodePropertyIdentification

            If arWHICHCLAUSE.KEYWDWHICH IsNot Nothing Then arQueryEdge.IsProjectColumn = True

            If arPreviousTargetNode Is Nothing Then
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            Else
                arQueryEdge.BaseNode = arPreviousTargetNode
            End If

            'Get the TargetNode
            'Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
            '20200822-VM-Change back if doesn't work
            'Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.NODEPROPERTYIDENTIFICATION.MODELELEMENT)
            lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME) '  Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
            arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            arQueryEdge.TargetNode.Alias = arWHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryEdge.TargetNode.Comparitor = arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.getComparitorType

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            If arWHICHCLAUSE.RECURSIVECLAUSE IsNot Nothing Then
                If arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDCIRCULAR IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsCircular = True
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDSHORTESTPATH IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsShortestPath = True
                ElseIf arQueryEdge.TargetNode.RDSTable.isCircularToTable(arQueryEdge.BaseNode.RDSTable) Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                End If

                arQueryEdge.IsRecursive = True
                If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER1 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                    If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                        arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(1)
                    End If
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = "0"
                    arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                End If
            End If

            '---------------------------------------------------------
            'Set the Identification
            For Each lsIdentifier In Me.WHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                arQueryEdge.IdentifierList.Add(lsIdentifier)
                arQueryEdge.TargetNode.HasIdentifier = True
                arQueryEdge.TargetNode.IdentifierList.Add(lsIdentifier)
            Next

            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate,
                                                  arPreviousTargetNode)

            If arQueryEdge.FBMFactType IsNot Nothing Then
                If arQueryEdge.FBMFactType.Arity = 2 Then
                    If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                        If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                            If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id Then
                                If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                                    arQueryEdge.IsReciprocal = True
                                End If
                            ElseIf arQueryEdge.BaseNode.Name = arQueryEdge.TargetNode.Name Then
                                arQueryEdge.IsReciprocal = True
                            End If
                        End If
                    End If
                End If
            End If


        End Sub
#End Region

        '2
#Region "analysePredicateWhichModelElement"
        Public Sub AnlysePredicateWhichModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                   ByRef arQueryGraph As FactEngine.QueryGraph,
                                                   ByRef arQueryEdge As FactEngine.QueryEdge,
                                                   ByRef arPreviousTargetNode As FactEngine.QueryNode)

            Dim lrFBMModelObject As FBM.ModelObject

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.PredicateWHICHModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            arQueryEdge.BaseNode = arQueryGraph.HeadNode

            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges(arQueryGraph.QueryEdges.Count - 1).IsPartialFactTypeMatch Then
                    arQueryEdge.BaseNode = arPreviousTargetNode
                End If
            End If

            'Get the TargetNode                        
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge, True)
            arQueryEdge.TargetNode.PreboundText = Me.WHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX

            If arWHICHCLAUSE.RECURSIVECLAUSE IsNot Nothing Then
                If arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDCIRCULAR IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsCircular = True
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDSHORTESTPATH IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsShortestPath = True
                ElseIf arQueryEdge.TargetNode.RDSTable.isCircularToTable(arQueryEdge.BaseNode.RDSTable) Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                End If
            End If

            arQueryGraph.Nodes.Add(arQueryEdge.TargetNode)

            ''---------------------------------------------------------
            ''Get the Predicate
            'For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
            '    lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
            'Next

            'Recursion
            If arWHICHCLAUSE.RECURSIVECLAUSE IsNot Nothing Then
                arQueryEdge.IsRecursive = True
                If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER1 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                    If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                        arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(1)
                    End If
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = "0"
                    arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                End If
            End If

            If Me.WHICHCLAUSE.MATHCLAUSE IsNot Nothing Then
                arQueryEdge.TargetNode.MathFunction = Boston.GetEnumFromDescriptionAttribute(Of pcenumMathFunction)(Me.WHICHCLAUSE.MATHCLAUSE.MATHFUNCTION)
                If Me.WHICHCLAUSE.MATHCLAUSE.NUMBER IsNot Nothing Then
                    arQueryEdge.TargetNode.MathNumber = CDbl(Me.WHICHCLAUSE.MATHCLAUSE.NUMBER)
                End If
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate,
                                                  arPreviousTargetNode)

            If Not arQueryEdge.AmbiguousFactTypeMatches.Count > 0 Then
                If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                    If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id Then
                        arQueryEdge.IsReciprocal = True
                    End If
                End If
            End If

        End Sub
#End Region

        '3
#Region "analyseThatPredicateThatModelElement"
        Public Sub analyseANDThatPredicateThatModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                           ByRef arQueryGraph As FactEngine.QueryGraph,
                                                           ByRef arQueryEdge As FactEngine.QueryEdge,
                                                           ByRef arPreviousTargetNode As FactEngine.QueryNode)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatPredicateThatModelElement

            'Set the BaseNode
            arQueryEdge.BaseNode = arPreviousTargetNode

            'Get the TargetNode                        
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject, arQueryEdge, True)
            arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

        End Sub
#End Region

        '4
#Region "analyseAndThatModelElementPredicateThatModelElement"

        Public Sub analyseAndThatModelElementPredicateThatModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                                       ByRef arQueryGraph As FactEngine.QueryGraph,
                                                                       ByRef arQueryEdge As FactEngine.QueryEdge,
                                                                       ByRef arPreviousTargetNode As FactEngine.QueryNode)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement

            'Set the BaseNode
            Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))

            Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, arQueryEdge)
            arQueryEdge.BaseNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX

            'Get the TargetNode                        
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(1))
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            ''---------------------------------------------------------
            ''Get the Predicate
            'For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
            '    lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
            'Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)
        End Sub
#End Region

        '5
#Region "analysePredicateWhichModelElement"
        Public Sub analysePredicateWhichModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                     ByRef arQueryGraph As FactEngine.QueryGraph,
                                                     ByRef arQueryEdge As FactEngine.QueryEdge,
                                                     ByRef arPreviousTargetNode As FactEngine.QueryNode)

            Dim lrFBMModelObject As FBM.ModelObject
            Dim lbIdentityCreatedTargetNode As Boolean = False

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.UnkownPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            If (arPreviousTargetNode IsNot Nothing) And Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                               (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then
                arQueryEdge.BaseNode = arPreviousTargetNode
            ElseIf (arPreviousTargetNode IsNot Nothing) And Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                               (Me.WHICHCLAUSE.KEYWDWHICH Is Nothing And Me.WHICHCLAUSE.KEYWDA Is Nothing) Then
                arQueryEdge.BaseNode = arPreviousTargetNode
            ElseIf (arPreviousTargetNode IsNot Nothing) And arWHICHCLAUSE.KEYWDA IsNot Nothing Then
                arQueryEdge.BaseNode = arPreviousTargetNode
            Else
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            End If

            'CodeSafe
            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartialFactTypeMatch Then
                    arQueryEdge.BaseNode = arPreviousTargetNode
                End If
            End If

            'Get the TargetNode                        
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrFBMModelObject Is Nothing Then
                lrFBMModelObject = Me.getModelElmentByBaseNodePredicate(arQueryEdge.BaseNode, arQueryEdge.Predicate)
                If lrFBMModelObject IsNot Nothing Then lbIdentityCreatedTargetNode = True
            End If
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            If lbIdentityCreatedTargetNode Then
                arQueryEdge.TargetNode.IdentifierList.Add(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                arQueryEdge.IdentifierList.Add(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                arQueryEdge.TargetNode.HasIdentifier = True
            End If
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode) '20200808-VM-Was AddUnique

            If arWHICHCLAUSE.RECURSIVECLAUSE IsNot Nothing Then
                If arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDCIRCULAR IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsCircular = True
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.KEYWDSHORTESTPATH IsNot Nothing Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                    arQueryEdge.IsShortestPath = True
                ElseIf arQueryEdge.TargetNode.RDSTable.isCircularToTable(arQueryEdge.BaseNode.RDSTable) Then
                    arQueryEdge.TargetNode.Alias = "RND" & 100.ToString & New Random().Next(10).ToString
                End If
                If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER1 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                    If arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                        arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(1)
                    End If
                ElseIf arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER2 IsNot Nothing Then
                    arQueryEdge.RecursiveNumber1 = "0"
                    arQueryEdge.RecursiveNumber2 = arWHICHCLAUSE.RECURSIVECLAUSE.NUMBER(0)
                End If
                arQueryEdge.IsRecursive = True
            End If

            If arWHICHCLAUSE.MATHCLAUSE IsNot Nothing Then
                arQueryEdge.TargetNode.MathFunction = Boston.GetEnumFromDescriptionAttribute(Of pcenumMathFunction)(arWHICHCLAUSE.MATHCLAUSE.MATHFUNCTION)
                If arWHICHCLAUSE.MATHCLAUSE.NUMBER IsNot Nothing Then
                    arQueryEdge.TargetNode.MathNumber = CDbl(arWHICHCLAUSE.MATHCLAUSE.NUMBER)
                End If
            End If

            If arWHICHCLAUSE.KEYWDNO IsNot Nothing Then
                arQueryEdge.IsSubQueryLeader = True
                arQueryEdge.SubQueryAlias = Boston.RandomString(5)
            ElseIf arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate,
                                                  arPreviousTargetNode)

            If arQueryEdge.FBMFactType IsNot Nothing Then 'May be PartialFactTypeMatch and hasn't found the FactType yet.
                If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                    If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType And arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id Then
                        If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                            arQueryEdge.IsReciprocal = True
                        End If
                    End If
                ElseIf arQueryEdge.FBMFactType.FactTypeReading.Find(Function(x) x.IsPreferred) Is Nothing And
                       arQueryEdge.FBMFactType.IsManyTo1BinaryFactType And
                       arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                    If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                        arQueryEdge.IsReciprocal = True
                    End If
                End If
            End If

            If arQueryEdge.FBMFactType IsNot Nothing Then
                If arQueryEdge.FBMFactType.Arity = 2 Then
                    If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                        If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                            If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                                If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                                    arQueryEdge.IsReciprocal = True
                                End If
                            ElseIf arQueryEdge.BaseNode.Name = arQueryEdge.TargetNode.Name Then
                                arQueryEdge.IsReciprocal = True
                            End If
                        End If
                    End If
                End If
            End If

        End Sub
#End Region

        '6
#Region "analyseAndPredicateWhichModelElement"

        Public Sub analyseAndPredicateWhichModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                        ByRef arQueryGraph As FactEngine.QueryGraph,
                                                        ByRef arQueryEdge As FactEngine.QueryEdge,
                                                        ByRef arPreviousTopicNode As FactEngine.QueryNode)

            Dim lrFBMModelObject As FBM.ModelObject
            Dim lbIdentityCreatedTargetNode As Boolean = False

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode            
            If arWHICHCLAUSE.MODELELEMENTNAME.Count = 2 Then
                Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
                Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.MODELELEMENTCLAUSE.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME & "'.")
                Dim lrQueryNode = New FactEngine.QueryNode(lrFBMModelObject)
                lrQueryNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX
                arQueryGraph.Nodes.AddUnique(lrQueryNode)
                arQueryEdge.BaseNode = lrQueryNode
            Else
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            End If

            'Get the TargetNode                        
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
            If lrFBMModelObject Is Nothing Then
                lrFBMModelObject = Me.getModelElmentByBaseNodePredicate(arQueryEdge.BaseNode, arQueryEdge.Predicate)
                If lrFBMModelObject IsNot Nothing Then lbIdentityCreatedTargetNode = True
            End If
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            If lbIdentityCreatedTargetNode Then
                arQueryEdge.TargetNode.IdentifierList.Add(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                arQueryEdge.IdentifierList.Add(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                arQueryEdge.TargetNode.HasIdentifier = True
            End If
            Try
                arQueryEdge.TargetNode.Comparitor = arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.getComparitorType
            Catch 'Not a biggie if not exists.
            End Try
            arQueryGraph.Nodes.Add(arQueryEdge.TargetNode) 'Was AddUnique, but WHICH implies that we are talking of a different TargetNode, as where TargeNode has been referenced before

            '---------------------------------------------------------
            'Set the Identification if there is any
            If Me.WHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                For Each lsIdentifier In Me.WHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                Next
                arQueryEdge.TargetNode.HasIdentifier = True
            End If

            '==========================================================================
            '20201007-VM-Maybe ned the following
            'If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
            '    arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            'Else
            '    arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            'End If
            '==========================================================================

            'Subquery
            If arWHICHCLAUSE.KEYWDNO IsNot Nothing Then
                arQueryEdge.IsSubQueryLeader = True
                arQueryEdge.SubQueryAlias = Boston.RandomString(5)
            ElseIf arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Try
                Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)
            Catch
                'This isn't elegant, but if the previous fails it is because the FactType is not outgoing to the QueryGraph.BaseNode, but may well be for the last Node queried over.
                '  can raise a warning if we like, such as "Warning: Better to use "AND THAT <PreviousTargetNode> <Predicate> WHICH <TargetNode>"
                Try
                    arQueryEdge.BaseNode = arPreviousTopicNode
                    Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    arQueryEdge.QueryGraph.Warning.Add("Recommendation: Use 'AND THAT " & arPreviousTopicNode.Name & " " & arQueryEdge.Predicate & " WHICH " & arQueryEdge.TargetNode.Name)
                End Try
            End Try

            If arQueryEdge.FBMFactType IsNot Nothing Then
                If arQueryEdge.FBMFactType.Arity = 2 Then
                    If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                        If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                            If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id Then
                                If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                                    arQueryEdge.IsReciprocal = True
                                End If
                            ElseIf arQueryEdge.BaseNode.Name = arQueryEdge.TargetNode.Name Then
                                arQueryEdge.IsReciprocal = True
                            End If
                        End If
                    End If
                End If
            End If

        End Sub
#End Region

        '6.1
#Region "analyseANDModelElementWHICHCModelElementlause"
        '6.1
        Private Sub analyseANDModelElementWHICHCModelElementlause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                                  ByRef arQueryGraph As FactEngine.QueryGraph,
                                                                  ByRef arQueryEdge As FactEngine.QueryEdge,
                                                                  ByRef arPreviousTargetNode As FactEngine.QueryNode)

            'E.g. AND Employee 2 reports to WHICH Employee 3

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateWhichModelElement
            If arWHICHCLAUSE.KEYWDA Is Nothing Then
                arQueryEdge.IsProjectColumn = True
            End If

            'Set the BaseNode

            Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")

            Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))

            arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)
            arQueryEdge.BaseNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX

            'Get the TargetNode                        
            Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(1))
            Dim lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(1))
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
            arQueryEdge.TargetNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX

            If arQueryEdge.TargetNode.Alias Is Nothing Then '20200810-VM-May need to revise this
                arQueryGraph.Nodes.Add(arQueryEdge.TargetNode) 'was simply addUnique 20200807-VM
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

            arPreviousTargetNode = arQueryEdge.BaseNode

        End Sub

#End Region

        '7
#Region "analyseANDTHATWHICHClause"
        '7
        Private Sub analyseANDTHATWHICHClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                              ByRef arQueryGraph As FactEngine.QueryGraph,
                                              ByRef arQueryEdge As FactEngine.QueryEdge,
                                              ByRef arPreviousTargetNode As FactEngine.QueryNode)

            'E.g. AND THAT School is in WHICH Faculty

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateWhichModelElement
            If arWHICHCLAUSE.KEYWDA Is Nothing Then
                arQueryEdge.IsProjectColumn = True
            End If

            'Set the BaseNode
            If arWHICHCLAUSE.KEYWDTHAT.Count = 1 And CType(arWHICHCLAUSE.WHICHTHATCLAUSE, FEQL.ParseNode).Nodes(0).Token.Type <> TokenType.KEYWDTHAT Then
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            ElseIf arWHICHCLAUSE.KEYWDTHAT.Count = 1 And
                   CType(arWHICHCLAUSE.WHICHTHATCLAUSE, FEQL.ParseNode).Nodes(0).Token.Type = TokenType.KEYWDTHAT And
                   arWHICHCLAUSE.KEYWDWHICH IsNot Nothing Then
                Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")

                Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
                Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))

                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, arQueryEdge)
                arQueryEdge.BaseNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX
            Else
                Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, arQueryEdge)
            End If

            'Get the TargetNode                        
            'Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            'Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(1))
            Dim lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            If arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                arQueryEdge.TargetNode.Comparitor = arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.getComparitorType

                '---------------------------------------------------------
                'Set the Identification
                For Each lsIdentifier In Me.WHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                    arQueryEdge.TargetNode.HasIdentifier = True
                    arQueryEdge.TargetNode.IdentifierList.Add(lsIdentifier)
                Next
            End If

            If arQueryEdge.TargetNode.Alias Is Nothing Then '20200810-VM-May need to revise this
                arQueryGraph.Nodes.Add(arQueryEdge.TargetNode) 'was simply addUnique 20200807-VM
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

            arPreviousTargetNode = arQueryEdge.BaseNode

            If arQueryEdge.FBMFactType IsNot Nothing Then
                If arQueryEdge.FBMFactType.Arity = 2 Then
                    If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                        If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                            If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id Then
                                If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                                    arQueryEdge.IsReciprocal = True
                                End If
                            ElseIf arQueryEdge.BaseNode.Name = arQueryEdge.TargetNode.Name Then
                                arQueryEdge.IsReciprocal = True
                            End If
                        End If
                    End If
                End If
            End If

        End Sub

#End Region

        '8
#Region "analyseANDTHATWHICHClause"
        '8
        Private Sub analyseANDTHATClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                         ByRef arQueryGraph As FactEngine.QueryGraph,
                                         ByRef arQueryEdge As FactEngine.QueryEdge)

            'E.g. AND THAT School is in (Faculty:'IT')

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateModelElement

            'Set the BaseNode
            If arWHICHCLAUSE.KEYWDTHAT.Count = 1 And CType(arWHICHCLAUSE.WHICHTHATCLAUSE, FEQL.ParseNode).Nodes(0).Token.Type <> TokenType.KEYWDTHAT Then
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            Else
                Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
                Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
                Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, arQueryEdge)
                arQueryEdge.BaseNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX
            End If


            'Get the TargetNode                        
            Dim lrFBMModelObject As FBM.ModelObject

            'Get the TargetNode                        
            If Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                'Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                '20200822-VM-Change back if doesn't work
                'Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                '---------------------------------------------------------
                'Set the Identification
                For Each lsIdentifier In arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                Next
            Else
                Dim liModelElementInd = 0
                Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
                If Me.WHICHCLAUSE.MODELELEMENTNAME.Count = 1 And Me.WHICHCLAUSE.NODE.Count = 0 Then
                    Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
                Else
                    Me.MODELELEMENTCLAUSE.MODELELEMENTNAME = Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME
                    Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
                    liModelElementInd = 1
                End If
                If arWHICHCLAUSE.KEYWDTHAT.Count = 2 Then liModelElementInd = 1
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(liModelElementInd))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            End If

            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
            arQueryEdge.TargetNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX
            If arQueryEdge.IdentifierList.Count > 0 Then arQueryEdge.TargetNode.HasIdentifier = True
            If arWHICHCLAUSE.NODE.Count > 0 Then
                arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
                arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
                arQueryEdge.TargetNode.Alias = arWHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            End If
            If arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                arQueryEdge.TargetNode.Comparitor = arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.getComparitorType
            End If

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

        End Sub

#End Region

        '9
#Region "analyseAndPredicateWhichModelElement"

        Public Sub analyseAndPredicateWhichModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                        ByRef arQueryGraph As FactEngine.QueryGraph,
                                                        ByRef arQueryEdge As FactEngine.QueryEdge,
                                                        ByRef aoNODEPROPERTYIDENTIFICATION As FEQL.NODEPROPERTYIDENTIFICATION)

            Dim lrFBMModelObject As FBM.ModelObject

            'E.g. "AND is in A School"
            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement


            'Set the BaseNode
            arQueryEdge.BaseNode = arQueryGraph.HeadNode


            'Get the TargetNode                        
            If aoNODEPROPERTYIDENTIFICATION IsNot Nothing Then
                'Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                '20200822-VM-Change back if doesn't work
                'Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                '---------------------------------------------------------
                'Set the Identification
                For Each lsIdentifier In arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                Next
            Else
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            End If

            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
            If aoNODEPROPERTYIDENTIFICATION IsNot Nothing Then
                arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
                arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            End If

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            ''---------------------------------------------------------
            ''Get the Predicate
            'For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
            '    lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
            'Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                      arQueryEdge.TargetNode,
                                                      arQueryEdge.Predicate)

        End Sub

#End Region

        '10
#Region "analyseWhichPredicateThatClause"

        Public Sub analyseWhichPredicateThatClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                              ByRef arQueryGraph As FactEngine.QueryGraph,
                                              ByRef arQueryEdge As FactEngine.QueryEdge,
                                              ByRef arPreviousTargetNode As FactEngine.QueryNode)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.WhichPredicateThatModelElement

            'Set the BaseNode
            arQueryEdge.BaseNode = arPreviousTargetNode

            'Get the TargetNode                        
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)
        End Sub
#End Region

        '11
#Region "analyseThatPredicateWhichClause"

        Public Sub analyseThatPredicateWhichClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                              ByRef arQueryGraph As FactEngine.QueryGraph,
                                              ByRef arQueryEdge As FactEngine.QueryEdge,
                                              ByRef arPreviousTargetNode As FactEngine.QueryNode)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            arQueryEdge.BaseNode = arPreviousTargetNode

            'Get the TargetNode                        
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject, arQueryEdge, True)
            arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate,
                                                  arPreviousTargetNode)

            If arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

            If Me.WHICHCLAUSE.MATHCLAUSE IsNot Nothing Then
                arQueryEdge.TargetNode.MathFunction = Boston.GetEnumFromDescriptionAttribute(Of pcenumMathFunction)(Me.WHICHCLAUSE.MATHCLAUSE.MATHFUNCTION)
                If Me.WHICHCLAUSE.MATHCLAUSE.NUMBER IsNot Nothing Then
                    arQueryEdge.TargetNode.MathNumber = CDbl(Me.WHICHCLAUSE.MATHCLAUSE.NUMBER)
                End If
            End If

            If arQueryEdge.FBMFactType IsNot Nothing Then
                If arQueryEdge.FBMFactType.Arity = 2 Then
                    If Not arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).PredicatePartText = arQueryEdge.Predicate Then
                        If arQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                            If arQueryEdge.BaseNode.Name <> arQueryEdge.FBMFactType.getPrimaryFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id Then
                                If arQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id <> arQueryEdge.BaseNode.Name Then
                                    arQueryEdge.IsReciprocal = True
                                End If
                            ElseIf arQueryEdge.BaseNode.Name = arQueryEdge.TargetNode.Name Then
                                arQueryEdge.IsReciprocal = True
                            End If
                        End If
                    End If
                End If
            End If

            If arWHICHCLAUSE.KEYWDNO IsNot Nothing Then
                arQueryEdge.IsSubQueryLeader = True
                arQueryEdge.SubQueryAlias = Boston.RandomString(5)
            ElseIf arQueryGraph.QueryEdges.Count > 0 Then
                If arQueryGraph.QueryEdges.Last.IsPartOfSubQuery Or arQueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                    arQueryEdge.IsPartOfSubQuery = True
                    arQueryEdge.SubQueryAlias = arQueryGraph.QueryEdges.Last.SubQueryAlias
                End If
            End If

        End Sub
#End Region

        '12
#Region "analystWITHClause"

        Private Sub analystWITHClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                      ByRef arQueryGraph As FactEngine.QueryGraph,
                                      ByRef arQueryEdge As FactEngine.QueryEdge,
                                      ByRef arPreviousTargetNode As FactEngine.QueryNode
                                      )

            'E.g. WITH WHAT Rating (as in "WHICH Person likes WHICH City WITH WHAT Rating"), can also be WITH (Rating:'10') for instance.
            Dim lrFBMModelObject As FBM.ModelObject

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.WithClause
            arQueryEdge.IsProjectColumn = True

            Call Me.GetParseTreeTokensReflection(Me.WITHCLAUSE, Me.WHICHCLAUSE.WITHCLAUSE)

            'Get the TargetNode                        
            If Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                'Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                '20200822-VM-Change back if doesn't work
                'Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                '---------------------------------------------------------
                'Set the Identification
                For Each lsIdentifier In arWHICHCLAUSE.NODE(0).NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                Next
            Else
                Dim liModelElementInd = 0
                If arWHICHCLAUSE.KEYWDTHAT.Count = 2 Then liModelElementInd = 1
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(liModelElementInd))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            End If

            Dim lrBaseObject As FBM.ModelObject = Nothing
            lrBaseObject = arQueryGraph.QueryEdges(arQueryGraph.QueryEdges.Count - 1).FBMFactType

            If Not lrBaseObject.IsObjectified Then
                For liInd = arQueryGraph.QueryEdges.Count - 1 To 0 Step -1
                    If arQueryGraph.QueryEdges(liInd).FBMFactType.IsObjectified Then
                        lrBaseObject = arQueryGraph.QueryEdges(liInd).FBMFactType
                        Exit For
                    ElseIf liInd = 0 Then
                        Select Case arPreviousTargetNode.FBMModelObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                Dim lsMessage = "WITH WHAT clauses cannot be used for Value Types. Make sure the previous target model element is an Entity Type or an Objectified Fact Type."
                                lsMessage &= " For 'WITH WHAT " & lrFBMModelObject.Id & "'"
                                Throw New Exception(lsMessage)
                            Case Is = pcenumConceptType.EntityType
                                lrBaseObject = arPreviousTargetNode.FBMModelObject
                            Case Is = pcenumConceptType.FactType
                                If arQueryGraph.QueryEdges(liInd).TargetNode.FBMModelObject.IsObjectified Then
                                    lrBaseObject = arQueryGraph.QueryEdges(liInd).TargetNode.FBMModelObject
                                Else
                                    Throw New Exception("Couldn't find an Objectified Fact Type / Property Edge for WITH WHAT Clause for Model Element, '" & lrFBMModelObject.Id & "'.")
                                End If
                        End Select
                    End If
                Next
            End If

            If lrBaseObject.ConceptType = pcenumConceptType.FactType Then
                If Not lrBaseObject.IsObjectified Then
                    Throw New Exception("The Model Element, '" & lrBaseObject.Id & "', is not an Objectified Fact Type for WITH WHAT clause for Model Element, '" & lrFBMModelObject.Id & "'.")
                End If
            End If

            arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseObject)

            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            '-----------------------------------------
            'Get the relevant FBM.FactType
            '  NB Predicate is nothing because the WITH clause is of the form "WITH WHAT Rating", so need to find the LinkFactType without using a Predicate
            '20200807-VM-Will need to fix this because might have more than one LinkFactType referencing the same ModelObject. Need to use PreboundReadingText etc and through FactTypeReadings.
            Try
                arQueryEdge.FBMFactType = arQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.ActiveRole.JoinedORMObject.Id = lrFBMModelObject.Id).Role.FactType
            Catch ex As Exception
                Dim lrTable As RDS.Table = arPreviousTargetNode.FBMModelObject.getCorrespondingRDSTable
                arQueryEdge.BaseNode = New FactEngine.QueryNode(arPreviousTargetNode.FBMModelObject, arQueryEdge)
                arQueryEdge.FBMFactType = lrTable.Column.Find(Function(x) x.ActiveRole.JoinedORMObject Is lrFBMModelObject).FactType

                Dim larModelElement As New List(Of FBM.ModelObject)
                larModelElement.Add(arQueryEdge.BaseNode.FBMModelObject)
                larModelElement.Add(arQueryEdge.TargetNode.FBMModelObject)
                arQueryEdge.FBMFactTypeReading = arQueryEdge.FBMFactType.getFactTypeReadingByModelElementOrder(larModelElement)

            End Try


            'Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
            '                                      arQueryEdge.TargetNode,
            '                                      arQueryEdge.Predicate)

        End Sub
#End Region

        '13
#Region "analystISNOTClause"

        Public Sub analystISNOTClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                      ByRef arQueryGraph As FactEngine.QueryGraph,
                                      ByRef arQueryEdge As FactEngine.QueryEdge)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatIdentityCompatitor

            If arWHICHCLAUSE.KEYWDIS IsNot Nothing Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.ISClause
            ElseIf arWHICHCLAUSE.KEYWDISNOT IsNot Nothing Then
                arQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.ISNOTClause
            End If

            'Set the BaseNode
            Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            Call Me.GetParseTreeTokensReflection(Me.MODELELEMENTCLAUSE, Me.WHICHCLAUSE.MODELELEMENT(0))
            If Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX Is Nothing Then
                arQueryEdge.BaseNode = arQueryGraph.Nodes.Find(Function(x) x.Name = Me.MODELELEMENTCLAUSE.MODELELEMENTNAME)
            Else
                Dim lrFBMModelObject = Me.Model.GetModelObjectByName(Me.MODELELEMENTCLAUSE.MODELELEMENTNAME)
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
                arQueryEdge.BaseNode.Alias = Me.MODELELEMENTCLAUSE.MODELELEMENTSUFFIX
            End If
            arQueryGraph.Nodes.AddUnique(arQueryEdge.BaseNode)

            'Set the TargetNode
            Me.MODELELEMENTCLAUSE = New FEQL.MODELELEMENTClause
            If Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX Is Nothing Then
                arQueryEdge.TargetNode = arQueryGraph.Nodes.Find(Function(x) x.Name = Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
            Else
                Dim lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.NODE(0).MODELELEMENTNAME)
                arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject, arQueryEdge)
                arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            End If
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            If arQueryEdge.BaseNode.Name <> arQueryEdge.TargetNode.Name Then
                Throw New Exception("Error: IS NOT clauses must reference the same type of Model Element.")
            End If

            'NB There is no Predicate or FBMFactType for this type of QueryEdge

        End Sub

#End Region

        '14
#Region "analystBooleanPredicateClause"

        Public Sub analystBooleanPredicateClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                 ByRef arQueryGraph As FactEngine.QueryGraph,
                                                 ByRef arQueryEdge As FactEngine.QueryEdge,
                                                 ByRef arPreviousTargetNode As FactEngine.QueryNode)

            'E.g. Protein is enzyme

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.BooleanPredicate

            '------------------------------------------------------
            'Set the BaseNode
            arQueryEdge.BaseNode = arQueryGraph.HeadNode
            If arPreviousTargetNode IsNot Nothing Then
                arQueryEdge.BaseNode = arPreviousTargetNode
            End If

            '------------------------------------------------------
            'No need to set the TargetNode, because there is none.
            arQueryEdge.TargetNode = Nothing

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

        End Sub

#End Region

        '15
#Region "analyseThatModelElementPredicate"

        Public Sub analyseThatModelElementPredicate(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                              ByRef arQueryGraph As FactEngine.QueryGraph,
                                              ByRef arQueryEdge As FactEngine.QueryEdge,
                                              ByRef arPreviousTargetNode As FactEngine.QueryNode)

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            arQueryEdge.BaseNode = Nothing

            'Get the TargetNode                        
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(arWHICHCLAUSE.MODELELEMENTNAME(0))
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject, arQueryEdge, True)
            'arQueryEdge.TargetNode.Alias = Me.WHICHCLAUSE.NODE(0).MODELELEMENTSUFFIX
            'arQueryEdge.TargetNode.PreboundText = arWHICHCLAUSE.NODE(0).PREBOUNDREADINGTEXT
            'arQueryEdge.TargetNode.PostboundText = arWHICHCLAUSE.NODE(0).POSTBOUNDREADINGTEXT
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            arQueryEdge.FBMFactType = Me.Model.getFactTypeByPredicateFarSideModelElement(arQueryEdge.Predicate, arQueryEdge.TargetNode.FBMModelObject, True)

            If arQueryEdge.FBMFactType Is Nothing Then
                Throw New Exception("Couldn't find a Fact Type that ends '" & arQueryEdge.Predicate & " " & arWHICHCLAUSE.MODELELEMENTNAME(0) & "'")
            End If
            Dim lsTargetNodeName As String = arQueryEdge.TargetNode.Name
            Dim larRole = From Role In arQueryEdge.FBMFactType.RoleGroup
                          Where Role.JoinedORMObject.Id <> lsTargetNodeName
                          Select Role


            Dim lrBaseFBMModelObject = larRole.First.JoinedORMObject
            If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")

            arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject, arQueryEdge)


            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)
        End Sub
#End Region


#Region "getWHICHClauseType"
        Public Function getWHICHClauseType(ByRef arWHICHClause As FEQL.WHICHCLAUSE,
                                           ByRef arWhichClauseNode As FEQL.ParseNode) As FactEngine.pcenumWhichClauseType

            'Preparsing
            '14
            If arWHICHClause.PREDICATE.Count > 0 And
               arWHICHClause.MODELELEMENTNAME.Count = 0 And
               arWHICHClause.NODE.Count = 0 And
               arWHICHClause.MODELELEMENT.Count = 0 Then
                '14
                Return FactEngine.Constants.pcenumWhichClauseType.BooleanPredicate
            End If

            If arWhichClauseNode.Nodes.Count >= 3 Then
                If Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                   arWhichClauseNode.Nodes(0).Token.Type = FEQL.TokenType.KEYWDWHICH And
                   arWhichClauseNode.Nodes(2).Token.Type = FEQL.TokenType.KEYWDTHAT Then
                    'E.g. WHICH involves THAT Lecturer
                    Return FactEngine.Constants.pcenumWhichClauseType.WhichPredicateThatModelElement
                Else
                    If arWhichClauseNode.Nodes(0).Nodes.Count > 0 Then
                        If Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                           arWhichClauseNode.Nodes(0).Nodes(0).Token.Type = FEQL.TokenType.KEYWDTHAT And
                           {FEQL.TokenType.KEYWDWHICH, FEQL.TokenType.KEYWDA, FEQL.TokenType.NODE, FEQL.TokenType.KEYWDNO}.Contains(arWhichClauseNode.Nodes(1).Token.Type) Then
                            'E.g. WHICH involves THAT Lecturer
                            Return FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
                        End If
                    End If
                End If
            ElseIf arWhichClauseNode.Nodes.count = 2 Then
                If arWhichClauseNode.Nodes(0).Nodes.Count = 2 Then
                    If Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                   arWhichClauseNode.Nodes(0).Nodes(0).Token.Type = FEQL.TokenType.KEYWDTHAT And
                   arWhichClauseNode.Nodes(0).Nodes(1).Token.Type = FEQL.TokenType.PREDICATECLAUSE And
                   arWhichClauseNode.Nodes(1).Nodes(0).Token.Type = FEQL.TokenType.MODELELEMENTNAME Then
                        'E.g. "THAT is in Cinema" (E.g. As in a Derived Fact Type Rule "IS WHERE Seat is in A Row THAT is in Cinema")
                        Return FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
                    End If
                End If
            End If

                '13
                If Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                (Me.WHICHCLAUSE.KEYWDISNOT IsNot Nothing Or
                Me.WHICHCLAUSE.KEYWDIS IsNot Nothing) Then

                'E.g. Person 1 IS NOT Person 2
                Return FactEngine.Constants.pcenumWhichClauseType.AndThatIdentityCompatitor

                '12
            ElseIf Me.WHICHCLAUSE.WITHCLAUSE IsNot Nothing Then

                'E.g. WITH WHAT Rating (as in "WHICH Person likes WHICH City WITH WHAT Rating"), can also be WITH (Rating:'10') for instance.
                Return FactEngine.Constants.pcenumWhichClauseType.WithClause

                '1
            ElseIf arWHICHClause.KEYWDAND Is Nothing And
                   arWHICHClause.NODEPROPERTYIDENTIFICATION IsNot Nothing Then

                'E.g. "WHICH is in (Factulty:'IT') "
                Return FactEngine.Constants.pcenumWhichClauseType.WhichPredicateNodePropertyIdentification

                '2
            ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                   Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Then

                'E.G. "holds WHICH Position"
                Return FactEngine.Constants.pcenumWhichClauseType.PredicateWHICHModelElement

                '3.
            ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 Then

                'E.g. ...TimetableBooking "THAT involves THAT Lecturer"
                'E.g. "AND THAT Lecturer works for THAT Faculty"
                Return FactEngine.Constants.pcenumWhichClauseType.AndThatPredicateThatModelElement

                '4
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 And '> 0 And '20200820-VM-Was  = 2
                   Me.WHICHCLAUSE.KEYWDWHICH Is Nothing Then

                'E.g. "AND THAT Lecturer works for THAT Faculty"
                Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement

                '15
            ElseIf Me.WHICHCLAUSE.KEYWDTHAT.Count > 0 And
                   Me.WHICHCLAUSE.MODELELEMENTNAME.Count = 1 And
                   Me.WHICHCLAUSE.PREDICATE.Count > 0 And
                   (Me.WHICHCLAUSE.KEYWDWHICH Is Nothing Or Me.WHICHCLAUSE.KEYWDA Is Nothing) Then

                'E.g. "AND is in A School"            
                Return FactEngine.Constants.pcenumWhichClauseType.ThatModelElementPredicate

                '5
            ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing Then

                'E.g. "is in WHICH Room" in the above example
                '  NB Also caters for "that is in A SemesterStructure" in "WHICH Room is in A Faculty THAT is in A SemesterStucture"
                Return FactEngine.Constants.pcenumWhichClauseType.UnkownPredicateWhichModelElement

                '6.1
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 0 And
                   Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing And
                   Me.WHICHCLAUSE.MODELELEMENT.Count = 2 Then

                'E.g. "AND holds WHICH Position"
                Return FactEngine.Constants.pcenumWhichClauseType.AndModelElementPredicateWhichModelElement

                '6
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   ((Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing) Or (Me.WHICHCLAUSE.KEYWDA IsNot Nothing)) And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 0 Then

                'E.g. "AND holds WHICH Position"
                Return FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement

                '7
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                   (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then

                Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateWhichModelElement

                '8
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 Then

                Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateModelElement

                '9
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.MODELELEMENTNAME IsNot Nothing And
                   Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then

                'E.g. "AND is in A School"                
                Return FactEngine.Constants.pcenumWhichClauseType.AndWhichPredicateNodePropertyIdentification

                '9
            ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                   Me.WHICHCLAUSE.MODELELEMENTNAME IsNot Nothing Then

                'E.g. "AND is in A School"            
                Return FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement
                '20210724-VM-Was below for some reason
                'Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement

            End If
        End Function
#End Region

    End Class
End Namespace
