Namespace FEQL
    Partial Public Class Processor

#Region "ProcessWHICHSELECTStatement"
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

                    '1
                    If Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                       Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION IsNot Nothing Then

                        Call Me.analyseWhichPredicateNodePropertyIdentification(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '2
                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                           Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Then
                        'E.G. "holds WHICH Position"  as in WHICH Lecturer holds WHICH Position
                        Call Me.AnlysePredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '3
                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing And
                           Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 Then
                        'E.g. ...TimetableBooking "THAT involves THAT Lecturer"
                        'E.g. "AND THAT Lecturer works for THAT Faculty"

                        Call Me.analyseANDThatPredicateThatModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '4
                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                           Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 Then
                        'E.g. "AND THAT Lecturer works for THAT Faculty"

                        Call Me.analyseAndThatModelElementPredicateThatModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '5
                    ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing Then
                        'E.g. "is located in WHICH Room" in the above example 'WHICH Lecturer is located in WHICH Room 
                        '  NB Also caters for "that is in A SemesterStructure" in "WHICH Room is in A Faculty THAT is in A SemesterStucture"

                        Call Me.analysePredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '6
                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                           Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing And
                           Me.WHICHCLAUSE.KEYWDTHAT.Count = 0 Then
                        'E.g. "AND holds WHICH Position"

                        Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        '7
                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                           Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                           (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then

                        Call Me.analyseANDTHATWHICHClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        '8
                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                           Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 Then

                        Call Me.analyseANDTHATClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        '9
                    ElseIf Me.WHICHCLAUSE.KEYWDAND IsNot Nothing And
                           Me.WHICHCLAUSE.MODELELEMENTNAME IsNot Nothing Then

                        Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, Me.NODEPROPERTYIDENTIFICATION)

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
                Dim larProjectColumn = lrQueryGraph.getProjectionColumns
                Dim lsColumnName As String

                For Each lrProjectColumn In larProjectColumn
                    lrRecordset.Columns.Add(lrProjectColumn.Name)
                    lsColumnName = lrFactType.CreateUniqueRoleName(lrProjectColumn.Name, 0)
                    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                Next

                While lrSQLiteDataReader.Read()

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    Dim liInd As Integer
                    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                        Select Case lrSQLiteDataReader.GetFieldType(liInd)
                            Case Is = GetType(String)
                                loFieldValue = lrSQLiteDataReader.GetString(liInd)
                            Case Else
                                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                        End Select

                        Try
                            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                            '=====================================================
                        Catch
                            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                        End Try
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

#End Region


#Region "ProcessWHICHSELECTStatementNew"
        Public Function ProcessWHICHSELECTStatementNew(ByVal asFEQLStatement As String) As ORMQL.Recordset

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
                            Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateWhichModelElement '7. E.g. AND THAT Faculty has FacultyName
                            Call Me.analyseANDTHATWHICHClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateModelElement '     8.
                            Call Me.analyseANDTHATClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge)

                        Case Is = FactEngine.pcenumWhichClauseType.AndWhichPredicateNodePropertyIdentification  ' 9. E.g. AND WHICH is in (Faculty:IT')
                            Call Me.analyseAndPredicateWhichModelElement(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, Me.NODEPROPERTYIDENTIFICATION)

                        Case Is = FactEngine.pcenumWhichClauseType.WhichPredicateThatModelElement ' 10. E.g. WHICH houses THAT Lecturer
                            Call Me.analyseWhichPredicateThatClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                        Case Is = FactEngine.pcenumWhichClauseType.ThatPredicateWhichModelElement '11. E.g. THAT houses WHICH Lecturer
                            Call Me.analyseThatPredicateWhichClause(Me.WHICHCLAUSE, lrQueryGraph, lrQueryEdge, lrPreviousTargetNode)

                    End Select

                    '-------------------------------------------------
                    'Add the QueryEdge to the QueryGraph
                    lrQueryGraph.QueryEdges.Add(lrQueryEdge)

                    lrPreviousTargetNode = lrQueryEdge.TargetNode

                Next

                'Richmond.WriteToStatusBar("Generating SQL", True)

                '==========================================================================
                'Get the records
                Dim lsSQLQuery = lrQueryGraph.generateSQL

                Dim lrTestRecordset = Me.DatabaseManager.GO(lsSQLQuery)

                'lrRecordset.Query = lsSQLQuery

                ''==========================================================
                ''Populate the lrRecordset with results from the database
                ''Richmond.WriteToStatusBar("Connecting to database.", True)
                'Dim lrSQLiteConnection = Database.CreateConnection(Me.Model.TargetDatabaseConnectionString)
                'Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, lsSQLQuery)

                'Dim larFact As New List(Of FBM.Fact)
                'Dim lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)
                'Dim lrFact As FBM.Fact
                ''Richmond.WriteToStatusBar("Reading results.", True)

                ''=====================================================
                ''Column Names        
                'Dim larProjectColumn = lrQueryGraph.getProjectionColumns
                'Dim lsColumnName As String

                'For Each lrProjectColumn In larProjectColumn
                '    lrRecordset.Columns.Add(lrProjectColumn.Name)
                '    lsColumnName = lrFactType.CreateUniqueRoleName(lrProjectColumn.Name, 0)
                '    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                '    lrFactType.RoleGroup.AddUnique(lrRole)
                'Next

                'While lrSQLiteDataReader.Read()

                '    lrFact = New FBM.Fact(lrFactType, False)
                '    Dim loFieldValue As Object = Nothing
                '    Dim liInd As Integer
                '    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                '        Select Case lrSQLiteDataReader.GetFieldType(liInd)
                '            Case Is = GetType(String)
                '                loFieldValue = lrSQLiteDataReader.GetString(liInd)
                '            Case Else
                '                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                '        End Select

                '        Try
                '            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                '            '=====================================================
                '        Catch
                '            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                '        End Try
                '    Next

                '    larFact.Add(lrFact)

                'End While
                'lrRecordset.Facts = larFact
                'lrSQLiteConnection.Close()

                'Run the SQL against the database
                Return lrTestRecordset

            Catch ex As Exception
                If ex.InnerException Is Nothing Then
                    lrRecordset.ErrorString = ex.Message
                Else
                    lrRecordset.ErrorString = ex.InnerException.Message
                End If

                Return lrRecordset
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

            If arPreviousTargetNode Is Nothing Then
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            Else
                arQueryEdge.BaseNode = arPreviousTargetNode
            End If

            'Get the TargetNode
            Me.NODEPROPERTYIDENTIFICATION = New FEQL.NODEPROPERTYIDENTIFICATION
            Call Me.GetParseTreeTokensReflection(Me.NODEPROPERTYIDENTIFICATION, Me.WHICHCLAUSE.NODEPROPERTYIDENTIFICATION)
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME)
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.NODEPROPERTYIDENTIFICATION.MODELELEMENTNAME & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If

            '---------------------------------------------------------
            'Set the Identification
            For Each lsIdentifier In Me.NODEPROPERTYIDENTIFICATION.IDENTIFIER
                arQueryEdge.IdentifierList.Add(lsIdentifier)
            Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                                  arQueryEdge.TargetNode,
                                                                  arQueryEdge.Predicate)


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

            'Get the TargetNode                        
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
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
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)


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
            Dim lrBaseFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrBaseFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.BaseNode = New FactEngine.QueryNode(lrBaseFBMModelObject)

            'Get the TargetNode                        
            Dim lrTargetFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(1))
            If lrTargetFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(1) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
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

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.UnkownPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            If Me.WHICHCLAUSE.KEYWDTHAT.Count = 1 And
                               (Me.WHICHCLAUSE.KEYWDWHICH IsNot Nothing Or Me.WHICHCLAUSE.KEYWDA IsNot Nothing) Then
                arQueryEdge.BaseNode = arPreviousTargetNode
            Else
                arQueryEdge.BaseNode = arQueryGraph.HeadNode
            End If


            'Get the TargetNode                        
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
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

            'If lrQueryEdge.BaseNode.FBMModelObject.ConceptType = pcenumConceptType.ValueType Then
            '    Dim lrTempNode = lrQueryEdge.BaseNode
            '    lrQueryEdge.BaseNode = lrQueryEdge.TargetNode
            '    lrQueryEdge.TargetNode = lrTempNode
            'End If

        End Sub
#End Region

        '6
#Region "analyseAndPredicateWhichModelElement"

        Public Sub analyseAndPredicateWhichModelElement(ByRef arWHICHCLAUSE As FEQL.WHICHCLAUSE,
                                                        ByRef arQueryGraph As FactEngine.QueryGraph,
                                                        ByRef arQueryEdge As FactEngine.QueryEdge)

            Dim lrFBMModelObject As FBM.ModelObject

            arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.AndPredicateWhichModelElement
            arQueryEdge.IsProjectColumn = True

            'Set the BaseNode
            arQueryEdge.BaseNode = arQueryGraph.HeadNode

            'Get the TargetNode                        
            lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
            If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)
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

            ''---------------------------------------------------------
            ''Get the Predicate
            'For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
            '    arQueryEdge.Predicate = Trim(arQueryEdge.Predicate & " " & lsPredicatePart)
            'Next

            '-----------------------------------------
            'Get the relevant FBM.FactType
            Call arQueryEdge.getAndSetFBMFactType(arQueryEdge.BaseNode,
                                                  arQueryEdge.TargetNode,
                                                  arQueryEdge.Predicate)

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

            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
            Else
                arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)
            End If


            ''---------------------------------------------------------
            ''Get the Predicate
            'For Each lsPredicatePart In Me.WHICHCLAUSE.PREDICATE
            '    arQueryEdge.Predicate = Trim(arQueryEdge.Predicate & " " & lsPredicatePart)
            'Next

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
                lrFBMModelObject = Me.Model.GetModelObjectByName(Me.WHICHCLAUSE.MODELELEMENTNAME(0))
                If lrFBMModelObject Is Nothing Then Throw New Exception("The Model does not contain a Model Element called, '" & Me.WHICHCLAUSE.MODELELEMENTNAME(0) & "'.")
            End If

            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrFBMModelObject)

            If lrFBMModelObject.ConceptType = pcenumConceptType.ValueType Then
                arQueryEdge.WhichClauseType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
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
            arQueryEdge.TargetNode = New FactEngine.QueryNode(lrTargetFBMModelObject)
            arQueryGraph.Nodes.AddUnique(arQueryEdge.TargetNode)

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
                           arWhichClauseNode.Nodes(1).Token.Type = FEQL.TokenType.KEYWDWHICH Then
                            'E.g. WHICH involves THAT Lecturer
                            Return FactEngine.Constants.pcenumWhichClauseType.ThatPredicateWhichModelElement
                        End If
                    End If
                End If
            End If
            '1
            If arWHICHClause.KEYWDAND Is Nothing And
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
                   Me.WHICHCLAUSE.KEYWDTHAT.Count = 2 Then

                'E.g. "AND THAT Lecturer works for THAT Faculty"
                Return FactEngine.Constants.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement

                '5
            ElseIf Me.WHICHCLAUSE.KEYWDAND Is Nothing Then

                'E.g. "is in WHICH Room" in the above example
                '  NB Also caters for "that is in A SemesterStructure" in "WHICH Room is in A Faculty THAT is in A SemesterStucture"
                Return FactEngine.Constants.pcenumWhichClauseType.UnkownPredicateWhichModelElement

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
                   Me.WHICHCLAUSE.MODELELEMENTNAME IsNot Nothing Then

                'E.g. "AND is in A School"                
                Return FactEngine.Constants.pcenumWhichClauseType.AndWhichPredicateNodePropertyIdentification

            End If
        End Function
#End Region

    End Class
End Namespace
