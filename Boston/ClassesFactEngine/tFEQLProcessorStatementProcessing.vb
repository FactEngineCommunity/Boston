Namespace FEQL
    Partial Public Class Processor

        Public Function ProcessWHICHSELECTStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset

            Try
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
                        lrQueryGraph.Nodes.AddUnique(lrQueryEdge.TargetNode)

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

                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Then
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
                        lrQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.PredicateWhichModelElement
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

                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Then
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
                        'lrRecordset.Error = New ORMQL.Error("Which subclause not recognised")
                    Else
                        Throw New Exception("Unknown WHICH Clause.")
                    End If

                    '-------------------------------------------------
                    'Add the QueryEdge to the QueryGraph
                    lrQueryGraph.QueryEdges.Add(lrQueryEdge)

                    lrPreviousTargetNode = lrQueryEdge.TargetNode

                Next

                '==========================================================================
                'Get the records
                Dim lsSQLQuery = lrQueryGraph.generateSQL

                '==========================================================
                'Populate the lrRecordset with results from the database
                Dim lrSQLiteConnection = Database.CreateConnection(Me.Model.TargetDatabaseConnectionString)
                Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, lsSQLQuery)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)
                Dim lrFact As FBM.Fact
                While lrSQLiteDataReader.Read()

                    lrFact = New FBM.Fact(lrFactType, False)

                    For liInd = 1 To lrSQLiteDataReader.FieldCount
                        Dim myreader As String = lrSQLiteDataReader.GetString(0)
                        '=====================================================
                        'Column Names
                        lrRecordset.Columns.Add(liInd.ToString)


                        Dim lrRole = New FBM.Role(lrFactType, liInd.ToString, True, Nothing)
                        lrFactType.RoleGroup.Add(lrRole)

                        lrFact.Data.Add(New FBM.FactData(lrRole, New FBM.Concept(lrSQLiteDataReader.GetString(liInd - 1)), lrFact))
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

    End Class
End Namespace
