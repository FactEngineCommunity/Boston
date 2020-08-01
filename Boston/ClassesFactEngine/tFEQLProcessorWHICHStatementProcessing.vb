Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessWHICHSELECTStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset
            lrRecordset.StatementType = FactEngine.Constants.pcenumFEQLStatementType.WHICHSELECTStatement

            Try
                'Richmond.WriteToStatusBar("Processsing WHICH Statement.", True)
                Me.WHICHSELECTStatement = New FEQL.WHICHSELECTStatement

                Call Me.GetParseTreeTokensReflection(Me.WHICHSELECTStatement, Me.Parsetree.Nodes(0))

                Dim lrQueryGraph As New FactEngine.QueryGraph(Me.Model)

                '----------------------------------------
                'Create the HeadNode for the QueryGraph
                Dim lrFBMModelObject As FBM.ModelObject = Me.Model.GetModelObjectByName(Me.WHICHSELECTStatement.MODELELEMENTNAME(0))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHSELECTStatement.MODELELEMENTNAME(0) & "'.")
                lrQueryGraph.HeadNode = New FactEngine.QueryNode(lrFBMModelObject)
                lrQueryGraph.Nodes.Add(lrQueryGraph.HeadNode)

                '----------------------------------
                'Get the Edges for the QueryGraph
                Dim lrPreviousTargetNode As FactEngine.QueryNode = Nothing
                For Each loWhichClause In Me.WHICHSELECTStatement.WHICHCLAUSE

                    Me.WHICHCLAUSE = New FEQL.WHICHCLAUSE
                    Call Me.GetParseTreeTokensReflection(Me.WHICHCLAUSE, loWhichClause)

                    '==============================
                    'Create the QueryEdge
                    Dim lrQueryEdge As New FactEngine.QueryEdge(lrQueryGraph)

                    '============================================
                    'Example Fact Engine Query
                    '--------------------------
                    'WHICH Lecturer is located in WHICH Room 
                    'AND holds WHICH Position
                    'AND is in WHICH School
                    'AND is in A School WHICH is in (Factulty:'IT') 
                    'AND THAT Lecturer works for THAT Faculty 
                    '------------------------------------------

                    'KEYWDAND As String
                    'KEYWDTHAT As List(Of String)
                    'PREDICATE As List(Of String)
                    'KEYWDWHICH As String
                    'MODELELEMENTNAME As List(Of String)
                    'NODEPROPERTYIDENTIFICATION As Object

                    'If Me.WHICHCLAUSE.KEYWDIS IsNot Nothing And
                    '   Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then

                    '    'E.g. 'IS in (Semester:'1') as in when queryinging 'WHICH TimetableBooking IS in (Semester:'1')
                    '    lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification

                    '    If lrPreviousTargetNode Is Nothing Then
                    '        lrQueryEdge.BaseNode = lrQueryGraph.HeadNode
                    '    Else
                    '        lrQueryEdge.BaseNode = lrPreviousTargetNode
                    '    End If

                    '    'Get the TargetNode
                    '    Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                    '    Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                    '    lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    '    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                    '    lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
                    '    'lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)

                    '    '---------------------------------------------------------
                    '    'Set the Identification
                    '    For Each lsIdentifier In Me.NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    '        lrQueryEdge.IdentifierList.Add(lsIdentifier)
                    '    Next

                    '    '---------------------------------------------------------
                    '    'Get the Predicate
                    '    For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                    '        lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                    '    Next

                    'Else
                    If Me.WHICHCLAUSE.KEYWDAND Is Nothing And Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then


                        'E.g. "WHICH is in (Factulty:'IT') "
                        lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.WhichPredicateNodePropertyIdentification

                        If lrPreviousTargetNode Is Nothing Then
                            lrQueryEdge.BaseNode = lrQueryGraph.HeadNode
                        Else
                            lrQueryEdge.BaseNode = lrPreviousTargetNode
                        End If

                        'Get the TargetNode
                        Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                        Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                        lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                        If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                        lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)

                        If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                            lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                        Else
                            lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)
                        End If

                        '---------------------------------------------------------
                        'Set the Identification
                        For Each lsIdentifier In Me.NODEPROPERTYIDENTIFICATION.IDENTIFIER
                            lrQueryEdge.IdentifierList.Add(lsIdentifier)
                        Next

                        '---------------------------------------------------------
                        'Get the Predicate
                        For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                            lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                        Next

                        '-----------------------------------------
                        'Get the relevant FBM.FactType
                        Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)

                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                               Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Then
                        'E.G. "AND holds WHICH Position"
                        lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateAModelElement
                        lrQueryEdge.IsProjectColumn = True

                        'Set the BaseNode
                        lrQueryEdge.BaseNode = lrQueryGraph.HeadNode

                        'Get the TargetNode                        
                        lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                        If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                        lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
                        lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)

                        '---------------------------------------------------------
                        'Get the Predicate
                        For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                            lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                        Next

                        '-----------------------------------------
                        'Get the relevant FBM.FactType
                        Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)

                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing Then
                        'E.g. "is in WHICH Room" in the above example
                        '  NB Also caters for "that is in A SemesterStructure" in "WHICH Room is in A Faculty THAT is in A SemesterStucture"
                        lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.PredicateWhichModelElement
                        lrQueryEdge.IsProjectColumn = True

                        'Set the BaseNode
                        If Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                               (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then
                            lrQueryEdge.BaseNode = lrPreviousTargetNode
                        Else
                            lrQueryEdge.BaseNode = lrQueryGraph.HeadNode
                        End If


                        'Get the TargetNode                        
                        lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                            lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
                            lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)


                            '---------------------------------------------------------
                            'Get the Predicate
                            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                                lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                            Next

                            '-----------------------------------------
                            'Get the relevant FBM.FactType
                            Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)

                        ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                               Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing And
                               Me.WHICHCLAUSE.KEYWDTHAT.Count = 0 Then
                            'E.g. "AND holds WHICH Position"
                            lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement
                            lrQueryEdge.IsProjectColumn = True

                            'Set the BaseNode
                            lrQueryEdge.BaseNode = lrQueryGraph.HeadNode

                            'Get the TargetNode                        
                            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                            lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
                            lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)

                            '---------------------------------------------------------
                            'Get the Predicate
                            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                                lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                            Next

                            '-----------------------------------------
                            'Get the relevant FBM.FactType
                            Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)

                        ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                               Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                               (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then

                            Call Me.analyseANDTHATWHICHClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                               Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 Then

                            Call Me.analyseANDTHATClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                               Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 Then
                            'E.g. "AND THAT Lecturer works for THAT Faculty"
                            lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement

                            'Set the BaseNode
                            Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                            If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                            lrQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)

                            'Get the TargetNode                        
                            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(1))
                            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(1) & "'.")
                            lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
                            lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)

                            '---------------------------------------------------------
                            'Get the Predicate
                            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                                lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                            Next

                            '-----------------------------------------
                            'Get the relevant FBM.FactType
                            Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)

                        ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And Me.WHICHCLAUSE.MODELELEMENTNAME IsNot Nothing Then
                            'E.g. "AND is in A School"
                            lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement

                            'Set the BaseNode
                            lrQueryEdge.BaseNode = lrQueryGraph.HeadNode

                            'Get the TargetNode                        
                            If Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                                Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                                Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                                '---------------------------------------------------------
                                'Set the Identification
                                For Each lsIdentifier In Me.NODEPROPERTYIDENTIFICATION.IDENTIFIER
                                    lrQueryEdge.IdentifierList.Add(lsIdentifier)
                                Next
                            Else
                                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                            End If

                            lrQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
                            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                                lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                            Else
                                lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)
                            End If


                            '---------------------------------------------------------
                            'Get the Predicate
                            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                                lrQueryEdge.Predicate = Trim(lrQueryEdge.Predicate & " " & lsPredicatePart)
                            Next

                            '-----------------------------------------
                            'Get the relevant FBM.FactType
                            Call lrQueryEdge.getAndSetFBMFactType(lrQueryEdge.BaseNode,
                                                                  lrQueryEdge.TargetNode,
                                                                  lrQueryEdge.Predicate)
                            'lrRecordset.Error = New ORMQL.Error("Which subclause not recognised")
                        Else
                            Throw New Exception("Unknown WHICH Clause.")
                    End If

                    '-------------------------------------------------
                    'Add the QueryEdge to the QueryGraph
                    lrQueryGraph.QueryEdges.Add(lrQueryEdge)

                    lrPreviousTargetNode = lrQueryEdge.TargetNode

                Next

                'Richmond.WriteToStatusBar("Generating SQL", True)

                '==========================================================================
                'Get the records
                Dim lsSQLQuery = lrQueryGraph.generateSQL

                lrRecordset.Query = lsSQLQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting to database.", True)
                Dim lrSQLiteConnection = Database.CreateConnection(Me.Model.TargetDatabaseConnectionString)
                Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, lsSQLQuery)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)
                Dim lrFact As FBM.Fact
                'Richmond.WriteToStatusBar("Reading results.", True)

                '=====================================================
                'Column Names        
                For Each lrProjectColumn In lrQueryGraph.getProjectionColumns
                    lrRecordset.Columns.Add(lrProjectColumn.Name)
                    Dim lrRole = New FBM.Role(lrFactType, lrProjectColumn.Name, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                Next

                While lrSQLiteDataReader.Read()

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                        Select Case lrSQLiteDataReader.GetFieldType(liInd)
                            Case Is = GetType(String)
                                loFieldValue = lrSQLiteDataReader.GetString(liInd)
                            Case Else
                                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                        End Select

                        lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                        '=====================================================
                    Next

                    larFact.Add(lrFact)

                End While
                lrRecordset.Facts = larFact
                lrSQLiteConnection.Close()

                'Run the SQL against the database
                Return lrRecordset

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = ex.Message
                Else
                    lrRecordset.ErrorString = ex.InnerException.Message
                End If

                Return lrRecordset
            End Try

        End Function

        Private Sub analyseANDTHATClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                         ByRef arQueryGraph As FactEngine.QueryGraph,
                                         ByRef arQueryEdge As FactEngine.QueryEdge)

            'E.g. AND THAT School is in (Faculty:'IT')

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicatetModelElement

            'Set the BaseNode
            If arWHICHCLAUSE.KEYWDTHAT.Count = 1 And CType(arWHICHCLAUSE.WHICHTHATCLAUSE, FEQL.ParseNode).Nodes(0).Token.Type <> TokenType.KEYWDTHAT Then
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            Else
                Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)
            End If


            'Get the TargetNode                        
            Dim lrFBMModelObject As FBM.ModelObject

            'Get the TargetNode                        
            If Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                '---------------------------------------------------------
                'Set the Identification
                For Each lsIdentifier In Me.NODEPROPERTYIDENTIFICATION.IDENTIFIER
                    arQueryEdge.IdentifierList.Add(lsIdentifier)
                Next
            Else
                Dim liModelElementInd = 0
                If arWHICHCLAUSE.KEYWDTHAT.Count = 2 Then liModelElementInd = 1
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(liModelElementInd))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            End If
            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)


            '---------------------------------------------------------
            'Get the Predicate
            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                arQueryEdge.Predicate = Trim(arQueryEdge.Predicate & " " & lsPredicatePart)
            Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

        End Sub

        Private Sub analyseANDTHATWHICHClause(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                              ByRef arQueryGraph As FactEngine.QueryGraph,
                                              ByRef arQueryEdge As FactEngine.QueryEdge,
                                              ByRef arPreviousTargetNode As FactEngine.QueryNode)

            'E.g. AND THAT School is in WHICH Faculty

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicatetModelElement
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
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)
            Else
                Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
                arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)
            End If

            'Get the TargetNode                        
            Dim lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(1))
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

            '---------------------------------------------------------
            'Get the Predicate
            For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
                arQueryEdge.Predicate = Trim(arQueryEdge.Predicate & " " & lsPredicatePart)
            Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

        End Sub

        Public Function getWHICHClauseType(ByRef arWHICHClause As FEQL.WHICHCLAUSE) As FactEngine.pcenumWhichClauseType

            Dim lrFBMModelObject As FBM.ModelObject

            If arWHICHClause.KEYWDAND Is Nothing And arWHICHClause.NODEPROPERTYIDENTIFICATION IsNot Nothing Then

                'E.g. "WHICH is in (Factulty:'IT') "
                'Get the TargetNode
                Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, arWHICHClause.NODEPROPERTYIDENTIFICATION)
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")

                If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                    Return FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                Else
                    Return FactEngine.Constants.pcenumWhichClauseType.WhichPredicateNodePropertyIdentification
                End If

            ElseIf arWHICHClause.KEYWDAND Is Nothing And
                               arWHICHClause.KEYWDWHICH IsNot Nothing Then
                'E.G. "AND holds WHICH Position"
                Return FactEngine.Constants.pcenumWhichClauseType.AndPredicateAModelElement

            ElseIf arWHICHClause.KEYWDAND Is Nothing Then
                'E.g. "is in WHICH Room" in the above example
                Return FactEngine.Constants.pcenumWhichClauseType.PredicateWhichModelElement

            ElseIf arWHICHClause.KEYWDAND IsNot Nothing And
                               arWHICHClause.KEYWDWHICH IsNot Nothing And
                               arWHICHClause.KEYWDTHAT.Count = 0 Then
                'E.g. "AND holds WHICH Position"
                Return FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement

            ElseIf arWHICHClause.KEYWDAND IsNot Nothing And
                               arWHICHClause.KEYWDTHAT.Count = 1 And
                               (arWHICHClause.KEYWDWHICH IsNot Nothing Or arWHICHClause.KEYWDA IsNot Nothing) Then

                'Get the TargetNode                        
                If arWHICHClause.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                    Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                    Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, arWHICHClause.NODEPROPERTYIDENTIFICATION)
                    lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                Else
                    Dim liModelElementInd = 0
                    If arWHICHClause.KEYWDTHAT.Count = 2 Then liModelElementInd = 1
                    lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHClause.MODELELEMENTNAME(liModelElementInd))
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHClause.MODELELEMENTNAME(0) & "'.")
                End If
                If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                    Return FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                Else
                    Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicatetModelElement
                End If

            ElseIf arWHICHClause.KEYWDAND IsNot Nothing And
                               arWHICHClause.KEYWDTHAT.Count = 1 Then

                'Get the TargetNode                        
                If arWHICHClause.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                    Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                    Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, arWHICHClause.NODEPROPERTYIDENTIFICATION)
                    lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                Else
                    Dim liModelElementInd = 0
                    If arWHICHClause.KEYWDTHAT.Count = 2 Then liModelElementInd = 1
                    lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHClause.MODELELEMENTNAME(liModelElementInd))
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHClause.MODELELEMENTNAME(0) & "'.")
                End If
                If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                    Return FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                Else
                    Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicatetModelElement
                End If

            ElseIf arWHICHClause.KEYWDAND IsNot Nothing And
                               arWHICHClause.KEYWDTHAT.Count = 2 Then
                'E.g. "AND THAT Lecturer works for THAT Faculty"

            ElseIf arWHICHClause.KEYWDAND IsNot Nothing And arWHICHClause.MODELELEMENTNAME IsNot Nothing Then
                'E.g. "AND is in A School"

                'Get the TargetNode                        
                If arWHICHClause.NODEPROPERTYIDENTIFICATION IsNot Nothing Then
                    Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
                    Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, arWHICHClause.NODEPROPERTYIDENTIFICATION)
                    lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
                Else
                    lrFBMModelObject = Me.Model.GetModelObjectByName(arWHICHClause.MODELELEMENTNAME(0))
                    If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & arWHICHClause.MODELELEMENTNAME(0) & "'.")
                End If

                If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                    Return FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                Else
                    Return FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement
                End If

            End If
        End Function

    End Class
End Namespace
