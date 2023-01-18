Namespace FactEngine
    Partial Public Class QueryGraph

        Public Function generateCypher(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement,
                                       Optional ByVal abIsCountStarSubQuery As Boolean = False,
                                       Optional ByVal abIsStraightDerivationClause As Boolean = False,
                                       Optional ByRef arDerivedModelElement As FBM.ModelObject = Nothing,
                                       Optional ByRef abIsSubQuery As Boolean = False) As String
            Dim lsCypherQuery As String = ""

            Dim liInd As Integer
            Dim larColumn As New List(Of RDS.Column)
            Dim lbRequiresGroupByClause As Boolean = False
            Dim lsSelectClause As String = ""

            Try
                'Set the Node Aliases. E.g. If Lecturer occurs twice in the FROM clause, then Lecturer, Lecturer2 etc
                Call Me.setNodeAliases()
                Call Me.setQueryEdgeAliases()

#Region "Get ProjectionColums"
                liInd = 1
                Dim larProjectionColumn = Me.getProjectionColumns(arWhichSelectStatement, abIsStraightDerivationClause, arDerivedModelElement)
                Me.ProjectionColumn = larProjectionColumn

                'Remove UnaryFactTypes from ProjectionColumns, because UnaryFactTypes in TypeDB store the OID rather than a field per se.
                Me.ProjectionColumn.RemoveAll(Function(x) x.Table.FBMModelElement.isUnaryFactType And x.isPartOfPrimaryKey)

                '20221014-Removed. Need for Objectified Fact Types.
                'Me.ProjectionColumn.RemoveAll(Function(x) x.Table.FBMModelElement.GetType = GetType(FBM.FactType) And x.Role.FactType.Id = x.Table.Name And x.Role.JoinedORMObject.GetType <> GetType(FBM.ValueType))
#End Region


#Region "Conditionals for Match from clause"
                Dim larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                             Where QueryEdge.TargetNode IsNot Nothing
                                             Select QueryEdge
                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None))
                'BooleanPredicate edges. E.g. Protein is enzyme
                Dim larExtraConditionalQueryEdges = From QueryEdge In Me.QueryEdges
                                                    Where QueryEdge.FBMFactType IsNot Nothing
                                                    Where QueryEdge.TargetNode Is Nothing And Not (QueryEdge.FBMFactType.IsDerived And QueryEdge.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))
                                                    Select QueryEdge
                larConditionalQueryEdges.AddRange(larExtraConditionalQueryEdges.ToList)

                'ShortestPath conditionals are excluded.
                '  E.g. For '(Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) '
                '  the second QueryEdge conditional is taken care of in the FROM clause processing for the recursive query.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                'Recursive NodePropertyIdentification conditionals are excluded.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)
#End Region

#Region "MATCH clause"
                Dim larMatchEdges = Me.QueryEdges.FindAll(Function(x) x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType).ToList

                'Circular/Recursive QueryEdges/TargetNodes are treated differently. See Recursive region below.
                'Dim larNode As List(Of QueryNode) = (From Node In larFromNodes
                '                                     Where Node.QueryEdge IsNot Nothing
                '                                     Where Node.QueryEdge.IsRecursive
                '                                     Where (Node.QueryEdge.IsCircular Or
                '                                            Node.RDSTable.isCircularToTable(Node.QueryEdge.BaseNode.RDSTable))).ToList

                'larFromNodes.RemoveAll(Function(x) larNode.Contains(x))

                Dim larSubQueryEdges As List(Of FactEngine.QueryEdge) = (From QueryEdge In larMatchEdges
                                                                         Where (QueryEdge.IsSubQueryLeader Or QueryEdge.IsPartOfSubQuery)
                                                                         Select QueryEdge
                                                                        ).ToList

                larMatchEdges.RemoveAll(Function(x) larSubQueryEdges.Contains(x))

                'Derived Fact Type Parameters
                'The QueryEdge.FBMFactType is derived, the Column for the QueryNode IsDerivationParameter
                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)

                'Circular/Recursive QueryEdges/TargetNodes are treated differently. See Recursive region below.
                Dim larNode As List(Of QueryNode) = (From Node In larFromNodes
                                                     Where Node.QueryEdge IsNot Nothing
                                                     Where Node.QueryEdge.IsRecursive
                                                     Where (Node.QueryEdge.IsCircular Or
                                                            Node.RDSTable.isCircularToTable(Node.QueryEdge.BaseNode.RDSTable))).ToList
                larFromNodes.RemoveAll(Function(x) larNode.Contains(x))


                Dim larDerivationNodes = (From Node In larFromNodes
                                          Where Node.QueryEdge IsNot Nothing
                                          Where Node.QueryEdge.FBMFactType IsNot Nothing
                                          Where Node.QueryEdge.FBMFactType.IsDerived
                                          Select Node).ToList

                Dim larRemovedDerivationNode As New List(Of FactEngine.QueryNode)
                For Each lrDerivationNode In larDerivationNodes
                    'Get the Derivation Column
                    Dim lrDerivationTable As RDS.Table = lrDerivationNode.QueryEdge.FBMFactType.getCorrespondingRDSTable
                    Dim larDerivationColumn = From Column In lrDerivationTable.Column
                                              Where Column.Role.JoinedORMObject.Id = lrDerivationNode.Name
                                              Select Column

                    If larDerivationColumn.Count > 0 Then
                        If larDerivationColumn.First.IsDerivationParameter Then
                            larFromNodes.Remove(lrDerivationNode)
                            larRemovedDerivationNode.Add(lrDerivationNode)
                        End If
                    End If
                Next


                '======================================================================================================
                'Develop the MATCH clause
                Dim larLocalMatchEdges As List(Of FactEngine.QueryEdge) = larMatchEdges.ToList

                If abIsSubQuery Then
                    lsCypherQuery &= " not {"
                Else
StartMatch:
                    'Because a Cypher query can have multiple MATCH clauses. As in:
                    'MATCH (person:Person)-[acted_in:ACTED_IN]-(movie:Movie)
                    'MATCH (person:Person)-[directed:DIRECTED]-(movie:Movie)
                    'RETURN person.name, movie.title
                    lsCypherQuery &= "MATCH "
                End If
                liInd = 0
                For Each lrQueryEdge In larLocalMatchEdges.FindAll(Function(x) Not (x.FBMFactType.IsDerived Or x.FBMFactType.IsUnaryFactType)).ToArray

                    'BaseNode
                    Dim lrPreviousQueryEdge As FactEngine.QueryEdge = lrQueryEdge.GetPreviousQueryEdge

                    If lrPreviousQueryEdge Is Nothing Or liInd = 0 Then
                        'First QueryEdge
                        lsCypherQuery &= "(" & LCase(lrQueryEdge.BaseNode.Name) & lrQueryEdge.BaseNode.Alias & ":" & lrQueryEdge.BaseNode.Name & ")"
                    Else
                        If lrPreviousQueryEdge.TargetNode.Name = lrQueryEdge.BaseNode.Name And lrPreviousQueryEdge.Alias = lrQueryEdge.Alias Then
                            'Add Nothing
                        Else
                            lsCypherQuery &= vbCrLf
                            GoTo StartMatch
                        End If
                    End If
                    'Predicate/Edge
                    lsCypherQuery &= "-[" & LCase(lrQueryEdge.FBMFactType.DBName) & lrQueryEdge.Alias & ":" & lrQueryEdge.FBMFactType.DBName & "]-"
                    'TargetNode
                    lsCypherQuery &= "(" & LCase(lrQueryEdge.TargetNode.Name) & lrQueryEdge.TargetNode.Alias & ":" & lrQueryEdge.TargetNode.Name & ")"

                    'Remove the Edge because it has been processed. This so that we can achieve the following:
                    'MATCH (person:Person)-[acted_in:ACTED_IN]-(movie:Movie)
                    'MATCH (person:Person)-[directed:DIRECTED]-(movie:Movie)
                    'RETURN person.name, movie.title
                    larLocalMatchEdges.Remove(lrQueryEdge)
                    liInd += 1
                Next
#Region "old unused code/legacy SQL/TypeQL"
                ''=================================================================================================
                ''PartialFactTypeReadingMatch - I.e. Joins on ManyToMany(..ToMany) tables
                'Dim larPartialFTMatchNode = From Node In larFromNodes
                '                            Where Node.QueryEdge IsNot Nothing
                '                            Where Node.QueryEdge.IsPartialFactTypeMatch
                '                            Select Node

                'Dim larPartialFTMatchFT = From Node In larPartialFTMatchNode
                '                          Where Not Node.QueryEdge.IsSubQueryLeader
                '                          Group Node By Node.QueryEdge.FBMFactType, Node.QueryEdge.Alias Into grp = Group


                'For Each lrFactTypeMatch In larPartialFTMatchFT

                '    Dim lrFactType As FBM.FactType = lrFactTypeMatch.FBMFactType
                '    '------------------------------------------------------------------------------
                '    'Sample query
                '    '(student: $Email,school: $School_Name,course: $Course_Code) isa studentship;

                '    Dim larLinkedNodes = (From Node In larFromNodes
                '                          Where Node.QueryEdge.FBMFactType Is lrFactType
                '                          Where Node.QueryEdge.Alias = lrFactTypeMatch.Alias
                '                          Select Node).ToList

                '    'A Node may have been missed because it comes from earlier in the query.
                '    Dim larQueryEdge = From QueryEdge In Me.QueryEdges
                '                       Where QueryEdge.FBMFactType Is lrFactType
                '                       Where QueryEdge.Alias = lrFactTypeMatch.Alias
                '                       Select QueryEdge

                '    Dim lrPrimaryQueryEdge As FactEngine.QueryEdge = Nothing 'The QueryEdge that we are talking about
                '    For Each lrQueryEdge In larQueryEdge
                '        lrPrimaryQueryEdge = lrQueryEdge
                '        larLinkedNodes.AddUnique(lrQueryEdge.BaseNode)
                '        larLinkedNodes.AddUnique(lrQueryEdge.TargetNode)
                '    Next

                '    lsCypherQuery &= "("
                '    liInd = 0
                '    Dim larUsedPredicatePart As New List(Of FBM.PredicatePart)
                '    For Each lrNode In larLinkedNodes
                '        If liInd > 0 Then lsCypherQuery.AppendString(",")

                '        Dim larPredicatePart As New List(Of FBM.PredicatePart)
                '        If lrNode.QueryEdge Is lrPrimaryQueryEdge Then
                '            larPredicatePart = (From PredicatePart In lrNode.QueryEdge.FBMFactTypeReading.PredicatePart
                '                                Where (PredicatePart.Role.JoinedORMObject.Id = lrNode.FBMModelObject.Id Or
                '                                PredicatePart.Role.JoinedORMObject.isSubtypeOfModelElement(lrNode.FBMModelObject))
                '                                Where Not larUsedPredicatePart.Contains(PredicatePart)
                '                                Select PredicatePart).ToList
                '        Else
                '            'If lrPrimaryQueryEdge.FBMPredicatePart IsNot Nothing Then
                '            'larPredicatePart.Insert(0, lrPrimaryQueryEdge.FBMPredicatePart)
                '            'Else
                '            larPredicatePart = (From PredicatePart In lrPrimaryQueryEdge.FBMFactTypeReading.PredicatePart
                '                                Where (PredicatePart.Role.JoinedORMObject.Id = lrNode.FBMModelObject.Id Or
                '                                       PredicatePart.Role.JoinedORMObject.isSubtypeOfModelElement(lrNode.FBMModelObject))
                '                                Where Not larUsedPredicatePart.Contains(PredicatePart)
                '                                Select PredicatePart).ToList
                '            'End If
                '        End If

                '        Dim lrPredicatePart = larPredicatePart.First
                '        larUsedPredicatePart.Add(lrPredicatePart)
                '        Select Case lrNode.FBMModelObject.GetType
                '            Case Is = GetType(FBM.ValueType)
                '                lsCypherQuery &= lrPredicatePart.Role.Name & ": $" & lrFactType.getCorrespondingRDSTable.DatabaseName & lrNode.DBVariableName & lrNode.Alias
                '            Case Else
                '                lsCypherQuery &= lrPredicatePart.Role.Name & ": $" & lrNode.RDSTable.DatabaseName & lrNode.Alias
                '        End Select


                '        liInd += 1
                '    Next
                '    lsCypherQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName

                '    '----------------------------------------------------
                '    'Return Columns
                '    Dim larReturnColumn = From Column In Me.ProjectionColumn
                '                          Where (Column.Table.Name = lrFactType.getCorrespondingRDSTable.Name Or
                '                                 lrFactType.isSubtypeOfModelElement(Column.Table.FBMModelElement))
                '                          Where Column.TemporaryAlias = lrFactTypeMatch.Alias
                '                          Where Not Column.isPartOfPrimaryKey
                '                          Select Column

                '    For Each lrReturnColumn In larReturnColumn
                '        lsCypherQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                '    Next

                '    lsCypherQuery &= ";" & vbCrLf
                'Next
#End Region

#Region "Unary FactType Nodes"

                For Each lrEdge In larMatchEdges.FindAll(Function(x) x.FBMFactType.IsUnaryFactType)

                    'lsCypherQuery &= "("

                    'Dim lrFactType As FBM.FactType = lrEdge.FBMFactType
                    'Dim lrRole As FBM.Role = lrFactType.RoleGroup(0)

                    ''See if a QueryEdge exists that maps to the Table of the UnaryFactType.
                    'Dim larUnaryFactTypeQueryEdge = From QueryEdge In Me.QueryEdges
                    '                                Where QueryEdge.FBMFactType.IsLinkFactType
                    '                                Where QueryEdge.TargetNode IsNot Nothing
                    '                                Where QueryEdge.TargetNode.FBMModelObject Is lrRole.JoinedORMObject
                    '                                Select QueryEdge

                    'If larUnaryFactTypeQueryEdge.Count > 0 Then
                    '    lrEdge = larUnaryFactTypeQueryEdge.First.TargetNode
                    '    lsCypherQuery &= lrRole.Name & ": $" & lrEdge.FBMFactType.DBVariableName & lrEdge.Alias
                    'Else
                    '    lsCypherQuery &= lrRole.Name & ": $" & lrRole.getCorrespondingRDSTable.DatabaseName
                    'End If

                    'lsCypherQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName

                    ''----------------------------------------------------
                    ''Return Columns
                    'Dim larReturnColumn = From Column In Me.ProjectionColumn
                    '                      Where (Column.Table.Name = lrFactType.getCorrespondingRDSTable.Name Or
                    '                             lrFactType.isSubtypeOfModelElement(Column.Table.FBMModelElement))
                    '                      Where Not Column.isPartOfPrimaryKey
                    '                      Select Column

                    'For Each lrReturnColumn In larReturnColumn
                    '    lsCypherQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                    'Next

                    'lsCypherQuery &= ";" & vbCrLf
                Next
#End Region

                ''=================================================================================================
                ''Derived FactTypeReadingMatch - I.e. Joins on ManyToMany(..ToMany) tables
                'Dim larCoveredFactTypes = (From FactTypeMatch In larPartialFTMatchFT
                '                           Select FactTypeMatch.FBMFactType).ToList

                'larPartialFTMatchNode = (From Node In larFromNodes
                '                         Where Node.QueryEdge IsNot Nothing
                '                         Where Node.QueryEdge.IsPartialFactTypeMatch
                '                         Where Node.QueryEdge.FBMFactType.IsDerived
                '                         Where Not larCoveredFactTypes.Contains(Node.QueryEdge.FBMFactType)
                '                         Select Node).ToList

                'Dim larPartialFTMatchFT2 = From Node In larPartialFTMatchNode
                '                           Select Node.QueryEdge.FBMFactType Distinct

                'For Each lrFactType In larPartialFTMatchFT2
                '    '------------------------------------------------------------------------------
                '    'Sample query
                '    '(student: $Email,school: $School_Name,course: $Course_Code) isa studentship;

                '    Dim larLinkedNodes = From Node In larFromNodes
                '                         Where Node.QueryEdge.FBMFactType Is lrFactType
                '                         Select Node

                '    lsCypherQuery &= "("
                '    liInd = 0
                '    For Each lrNode In larLinkedNodes
                '        If liInd > 0 Then lsCypherQuery.AppendString(",")

                '        Dim larPredicatePart = From PredicatePart In lrNode.QueryEdge.FBMFactTypeReading.PredicatePart
                '                               Where PredicatePart.Role.JoinedORMObject Is lrNode.FBMModelObject
                '                               Select PredicatePart

                '        lsCypherQuery &= larPredicatePart.First.Role.Name & ": $" & lrNode.RDSTable.DatabaseName
                '        liInd += 1
                '    Next
                '    lsCypherQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName & ";" & vbCrLf

                'Next


#End Region
                'WHERE
                larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                         Where QueryEdge.TargetNode IsNot Nothing
                                         Select QueryEdge

                'WhereEdges are Where joins, rather than ConditionalQueryEdges which test for values by identifiers.
                Dim larWhereEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                                                                       x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType) Or
                                                                                       x.IsRDSTable Or x.IsDerived)
                'And
                'Not (x.IsPartialFactTypeMatch And
                'x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))
                ')

                'Add special DerivedFactType WhereEdges
                Dim larSpecialDerivedQueryEdges = From QueryEdge In Me.QueryEdges
                                                  Where QueryEdge.FBMFactType IsNot Nothing
                                                  Where QueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count
                                                  Select QueryEdge

                larWhereEdges.AddRange(larSpecialDerivedQueryEdges.ToList)

                'Remove DerivationParameter Nodes
                For Each lrDerivationParameterNode In larRemovedDerivationNode
                    larWhereEdges.Remove(lrDerivationParameterNode.QueryEdge)
                Next

                larConditionalQueryEdges = New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None))
                '20210826-VM-Removed
                'And (Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                'BooleanPredicate edges. E.g. Protein is enzyme
                larConditionalQueryEdges.AddRange(Me.QueryEdges.FindAll(Function(x) x.TargetNode Is Nothing And Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                'ShortestPath conditionals are excluded.
                '  E.g. For '(Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) '
                '  the second QueryEdge conditional is taken care of in the FROM clause processing for the recursive query.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                If Not abIsStraightDerivationClause Then
                    larConditionalQueryEdges.RemoveAll(Function(x) x.FBMFactType.IsDerived)
                End If

                'Recursive NodePropertyIdentification conditionals are excluded.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                larWhereEdges.RemoveAll(Function(x) (Not x.IsRDSTable And x.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType)) And (x.IdentifierList.Count = 0) And (x.TargetNode.IdentifierList.Count = 0))

                If larConditionalQueryEdges.Count = 0 And (Not Me.HeadNode.HasIdentifier) Then 'HeadNode.HasIdentifier is where "(Person:'Peter') went to WHICH School"

                    If Me.QueryEdges.Count = 0 And Me.HeadNode IsNot Nothing Then
                        lsCypherQuery &= "(" & Me.HeadNode.Name.LCase & ":" & Me.HeadNode.Name & ")"
                    End If

                    GoTo ReturnClause 'There is no WHERE Conditionals.
                ElseIf larConditionalQueryEdges.Count = 1 And (Not Me.HeadNode.HasIdentifier) Then 'HeadNode.HasIdentifier is where "(Person:'Peter') went to WHICH School"

                    If Me.QueryEdges.Count = 1 And Me.HeadNode IsNot Nothing And Trim(lsCypherQuery = "MATCH") Then
                        lsCypherQuery &= "(" & Me.HeadNode.Name.LCase & ":" & Me.HeadNode.Name & ")"
                    ElseIf Trim(lsCypherQuery) = "MATCH" Then
                        lsCypherQuery &= "(" & Me.HeadNode.Name.LCase & ":" & Me.HeadNode.Name & ")" & vbCrLf & "WHERE "
                    Else
                        lsCypherQuery &= vbCrLf & "WHERE "
                    End If

                    'GoTo ReturnClause
                ElseIf larConditionalQueryEdges.Count = 0 And Me.HeadNode.HasIdentifier And (larMatchEdges.Count = 0 And larLocalMatchEdges.Count = 0) Then
                    If Me.QueryEdges.Count <= 1 And Me.HeadNode IsNot Nothing Then
                        lsCypherQuery &= "(" & Me.HeadNode.Name.LCase & ":" & Me.HeadNode.Name & ")" & vbCrLf & "WHERE "
                    End If
                Else
                    lsCypherQuery &= vbCrLf & "WHERE "
                End If


#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = Nothing
                Dim lbHasWhereClause As Boolean = False

#Region "WhereConditionals"
                Dim lbFirstEdgeIsRecursive As Boolean = False
                If Me.QueryEdges.Count > 0 Then
                    lbFirstEdgeIsRecursive = Me.QueryEdges(0).IsRecursive
                End If

                If Me.HeadNode.HasIdentifier And Not lbFirstEdgeIsRecursive Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrTargetTable.DatabaseName) & Viev.NullVal(Me.HeadNode.Alias, "") & "." & lrColumn.Name & Me.HeadNode.getTargetSQLComparator & "'" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then
                            lsCypherQuery &= "AND "
                        End If
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
                End If

                liInd = 0
                For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))

                    'lsCypherQuery &= String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBROPEN.ToArray)
                    lbIntialWhere = lrQueryEdge.WhichClause.getAndOr(Boston.returnIfTrue(liInd = 0, "", "AND")) & " " & String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBROPEN.ToArray)

                    Select Case lrQueryEdge.WhichClauseSubType
                        Case Is = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                            Dim lrFactType As FBM.FactType = Nothing
                            Dim lrPredicatePart As FBM.PredicatePart = Nothing

                            If lrQueryEdge.FBMPredicatePart IsNot Nothing Then
                                lrPredicatePart = lrQueryEdge.FBMPredicatePart
                                lrFactType = lrQueryEdge.FBMFactType
                            Else

                                Select Case lrQueryEdge.BaseNode.FBMModelObject.GetType
                                    Case GetType(FBM.FactType)
                                        If lrQueryEdge.WhichClauseType = pcenumWhichClauseType.WithClause Then
                                            lrFactType = lrQueryEdge.FBMFactType
                                        Else
                                            lrFactType = CType(lrQueryEdge.BaseNode.FBMModelObject, FBM.FactType)
                                        End If

                                    Case GetType(FBM.EntityType)
                                        lrFactType = lrQueryEdge.FBMFactType
                                    Case GetType(FBM.ValueType)
                                        If lrQueryEdge.IsPartialFactTypeMatch Then
                                            lrFactType = lrQueryEdge.FBMFactType
                                        Else
                                            Throw New NotImplementedException("Unknown Conditional type in query. Contact support.")
                                        End If
                                End Select


                                Dim larPredicatePart As List(Of FBM.PredicatePart)
                                If lrQueryEdge.Predicate = "" Then
                                    'Likely a "WITH WHAT Rating" or "WITH (Rating:'8')" as in "WHICH Lecturer likes WHICH Lecturer WITH WHAT RATING"
                                    larPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                                        Select FactTypeReading.PredicatePart(0)).ToList
                                Else
                                    larPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                                        From PredicatePart In FactTypeReading.PredicatePart
                                                        Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                                        Select PredicatePart).ToList
                                End If

                                If larPredicatePart.Count = 0 Then

                                    larPredicatePart = (From FactType In Me.Model.FactType
                                                        From FactTypeReading In FactType.FactTypeReading
                                                        From PredicatePart In FactTypeReading.PredicatePart
                                                        Where FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrQueryEdge.BaseNode.Name _
                                                           Or x.JoinedORMObject.Id = lrQueryEdge.TargetNode.Name).Count = 2
                                                        Where PredicatePart.PredicatePartText = lrQueryEdge.Predicate
                                                        Where lrQueryEdge.Predicate <> ""
                                                        Select PredicatePart).ToList

                                    If larPredicatePart.Count > 0 Then
                                        lrPredicatePart = larPredicatePart.First
                                        lrFactType = lrPredicatePart.FactTypeReading.FactType
                                    Else
                                        If lrFactType.IsObjectified Then
                                            Dim larFactTypeReading = From FactType In lrFactType.getLinkFactTypes
                                                                     From FactTypeReading In FactType.FactTypeReading
                                                                     Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = lrQueryEdge.BaseNode.Name
                                                                     Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = lrQueryEdge.TargetNode.Name
                                                                     Select FactTypeReading
                                            If larFactTypeReading.Count > 0 Then
                                                lrFactType = larFactTypeReading.First.FactType
                                            End If
                                        End If
                                    End If
                                Else
                                    If lrQueryEdge.FBMFactTypeReading IsNot Nothing Then
                                        lrPredicatePart = larPredicatePart.Find(Function(x) x.FactTypeReading Is lrQueryEdge.FBMFactTypeReading)
                                    Else
                                        lrPredicatePart = larPredicatePart.First 'For now...need to consider PreboundReadingText/s
                                    End If
                                End If
                            End If
                            Dim lrResponsibleRole As FBM.Role

                            If lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.BaseNode.FBMModelObject Then
                                'Nothing to do here, because is the Predicate joined to the BaseNode that we want the Table for
                            ElseIf Not lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject Then

                                'lrQueryEdge.Predicate = "is " & lrQueryEdge.Predicate
                                '20200808-VM-Leave this breakpoint here. If hasn't been hit in years, get rid of this ElseIf
                                lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                                   Select PredicatePart).First
                            End If

                            If lrPredicatePart Is Nothing Then
                                Throw New Exception("There is no Predicate (Part) of Fact Type, '" & lrQueryEdge.FBMFactType.Id & "', that is '" & lrQueryEdge.Predicate & "'.")
                            Else
                                If lrFactType.IsLinkFactType Then
                                    'Want the Role from the actual FactType
                                    lrResponsibleRole = lrFactType.LinkFactTypeRole
                                ElseIf lrQueryEdge.IsPartialFactTypeMatch Or lrQueryEdge.FBMFactType.isRDSTable Then
                                    lrResponsibleRole = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr).Role
                                Else
                                    lrResponsibleRole = lrPredicatePart.Role
                                End If
                            End If

                            Dim lrTable As RDS.Table
                            If lrQueryEdge.IsPartialFactTypeMatch Or lrQueryEdge.FBMFactType.isRDSTable Then
                                lrTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrTable.DatabaseName) & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsCypherQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select

                                lsCypherQuery &= lrQueryEdge.getTargetSQLComparator

                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                        Dim loDateTime As DateTime = Nothing
                                        If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                            Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                        End If
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select
                            Else
                                lrTable = lrQueryEdge.BaseNode.RDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole IsNot Nothing
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrQueryEdge.BaseNode.RDSTable.DatabaseName) & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsCypherQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select

                                Select Case lrQueryEdge.TargetNode.Comparitor
                                    Case Is = FEQL.pcenumFEQLComparitor.InComparitor
                                        lsCypherQuery &= " IN [" & String.Join(",", lrQueryEdge.TargetNode.IdentifierList) & "]"
                                    Case Else
                                        lsCypherQuery &= lrQueryEdge.getTargetSQLComparator
                                        Select Case lrColumn.getMetamodelDataType
                                            Case Is = pcenumORMDataType.TemporalDateAndTime
                                                Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                                Dim loDateTime As DateTime = Nothing
                                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                End If
                                                Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                                lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                            Case Else
                                                lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                        End Select
                                End Select
                            End If

                            lbIntialWhere = lrQueryEdge.WhichClause.getAndOr & String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBROPEN.ToArray)
                        Case Else

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrQueryEdge.BaseNode.RDSTable.DatabaseName) & "."

                                    Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsCypherQuery &= lrTargetColumn.Name & " = True"

                                Case Else



                                    If lrQueryEdge.TargetNode.MathFunction <> pcenumMathFunction.None Then

                                        If lrQueryEdge.FBMFactType.IsDerived Then

                                            lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrQueryEdge.FBMFactType.Id &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              CType(lrQueryEdge.TargetNode.PreboundText & lrQueryEdge.TargetNode.Name, String).Replace("-", "")

                                            lsCypherQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsCypherQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf

                                        Else


                                            'Math function
                                            Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                            lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              LCase(lrTargetTable.DatabaseName) &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              lrTargetColumn.Name

                                            lsCypherQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsCypherQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf

                                        End If
                                        lbIntialWhere = lrQueryEdge.WhichClause.getAndOr & String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBROPEN.ToArray)
                                    Else
                                        Dim lrTargetTable As RDS.Table = Nothing
                                        Dim lsAlias As String = ""

                                        'Check for reciprocal reading. As in WHICH Person was armed by (Person 2:'David') rather than WHICH Person armed (Person 2:'Saul')
                                        If lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) Then
                                            lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                                            Dim lrColumn As RDS.Column
                                            If lrQueryEdge.FBMFactType.IsLinkFactType Then
                                                lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.LinkFactTypeRole)
                                                '20210820-VM-Added below. Was not hear for some reason.
                                                lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrQueryEdge.BaseNode.RDSTable.DatabaseName) & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = "
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0))
                                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                End Select
                                            Else
                                                lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                                lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrQueryEdge.BaseNode.RDSTable.DatabaseName) & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDate,
                                                              pcenumORMDataType.TemporalDateAndTime
                                                        lsCypherQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                                End Select
                                                lsCypherQuery &= lrQueryEdge.getTargetSQLComparator
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0), True)
                                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsCypherQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                End Select
                                            End If

                                        Else
                                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                                lrTargetTable = lrQueryEdge.TargetNode.RDSTable
                                                lsAlias = Viev.NullVal(lrQueryEdge.TargetNode.Alias, "")
                                            Else
                                                lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                                lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                                            End If

                                            Dim larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns

                                            liInd = 0

                                            Select Case lrQueryEdge.TargetNode.Comparitor
                                                Case Is = FEQL.pcenumFEQLComparitor.InComparitor
                                                    If larIndexColumns.Count > 1 Then Throw New Exception(lrTargetTable.Name & " has a multipart identifier. Cannot use in IN clause.")
                                                    Dim lsIdentifierList = ""
                                                    Dim lsJoinString As String = Boston.returnIfTrue(larIndexColumns(0).DataTypeIsNumeric, ",", "','")
                                                    lsIdentifierList = Boston.returnIfTrue(larIndexColumns(0).DataTypeIsNumeric, ",", "'") & String.Join(lsJoinString, lrQueryEdge.TargetNode.IdentifierList) & Boston.returnIfTrue(larIndexColumns(0).DataTypeIsNumeric, ",", "'")

                                                    lsCypherQuery &= LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(0).Name & " IN [" & lsIdentifierList & "]"
                                                Case Else
                                                    lsCypherQuery &= Viev.NullVal(lbIntialWhere, "")

                                                    For Each lsIdentifier In lrQueryEdge.IdentifierList
                                                        If liInd > 0 Then
                                                            lsCypherQuery &= "AND "
                                                            lbIntialWhere = ""
                                                        End If

                                                        Select Case lrQueryEdge.TargetNode.ModifierFunction
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                                lsCypherQuery &= "Date(" & LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(liInd).Name & ")"
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                                                lsCypherQuery &= "ToLower(" & LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(liInd).Name & ")"
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                                                lsCypherQuery &= "ToUpper(" & LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(liInd).Name & ")"
                                                            Case Else
                                                                lsCypherQuery &= LCase(lrTargetTable.DatabaseName) & Viev.NullVal(lrQueryEdge.Alias, "") & "." & larIndexColumns(liInd).Name
                                                        End Select

                                                        'lsCypherQuery &= LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(liInd).Name
                                                        lsCypherQuery &= lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "'" & vbCrLf
                                                        liInd += 1
                                                    Next
                                            End Select
                                        End If

                                        lbIntialWhere = lrQueryEdge.WhichClause.getAndOr("AND") & " " & String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBROPEN.ToArray)
                                    End If
                            End Select
                    End Select
                    lbHasWhereClause = True

                    lsCypherQuery &= String.Join("", lrQueryEdge.WhichClause.WHICHCLAUSEBRCLOSE.ToArray)
                    liInd += 1
                Next
#End Region
#End Region

#Region "Subqueries"
                '=====================================================================================
                'SubQueries
                Dim lasSubQueryAlias = From QueryEdge In Me.QueryEdges
                                       Where QueryEdge.IsSubQueryLeader Or QueryEdge.IsPartOfSubQuery
                                       Select QueryEdge.SubQueryAlias Distinct

                For Each lsSubQueryAlias In lasSubQueryAlias
                    Dim larSubQueryEdge = Me.QueryEdges.FindAll(Function(x) x.SubQueryAlias = lsSubQueryAlias)

                    Dim lrSubQueryGraph = New FactEngine.QueryGraph(Me.Model)

                    lrSubQueryGraph.HeadNode = larSubQueryEdge.First.BaseNode
                    lrSubQueryGraph.QueryEdges.AddRange(larSubQueryEdge)
                    lrSubQueryGraph.QueryEdges.ForEach(Sub(x) x.IsSubQueryLeader = False)
                    lrSubQueryGraph.QueryEdges.ForEach(Sub(x) x.IsPartOfSubQuery = False)

                    For Each lrQueryEdge In lrSubQueryGraph.QueryEdges
                        lrSubQueryGraph.Nodes.Add(lrQueryEdge.TargetNode)
                    Next

                    '----------------------------------------------------------------------------------------------
                    'Remove Nodes that clearly are correlated. E.g. Session in 
                    '"WHICH Cinema is showing (Film:'Rocky') at (DateTime:'1/5/2021 10:00') 
                    '"AND contains WHICH Row THAT contains A Seat THAT has NO Booking THAT Is for THAT Session "
                    Dim lrNode As FactEngine.QueryNode
                    Dim lbKeep As Boolean = False
                    For liInd = 0 To lrSubQueryGraph.Nodes.Count - 1
                        lrNode = lrSubQueryGraph.Nodes(liInd)
                        If lrNode.IsThatReferencedTargetNode And liInd > 0 Then
                            For liInd2 = 0 To liInd - 1
                                If lrSubQueryGraph.Nodes(liInd2).Name = lrNode.Name And lrSubQueryGraph.Nodes(liInd2).Alias = lrNode.Alias Then
                                    lbKeep = True
                                End If
                            Next
                            If Not lbKeep Then lrSubQueryGraph.Nodes.Remove(lrNode)
                        End If
                    Next
                    'lrSubQueryGraph.Nodes.RemoveAll(Function(x) x.IsThatReferencedTargetNode)
                    lsCypherQuery &= lrSubQueryGraph.generateTypeQL(arWhichSelectStatement, True,,, True)
                    lsCypherQuery &= "};"
                Next
                '=====================================================================================
#End Region

#Region "RETURN/GET Clause"
ReturnClause:
                If Not abIsSubQuery Then
                    If Me.ProjectionColumn.Count > 0 Or arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        lsCypherQuery &= vbCrLf & "RETURN "
                    End If

                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        lsCypherQuery &= Viev.NullVal(arWhichSelectStatement.RETURNCLAUSE.KEYWDDISTINCT, "") & " "
                    End If


                    liInd = 1

                    Dim larProjectColumn = (From Column In Me.ProjectionColumn
                                            Where Column IsNot Nothing).GroupBy(Function(d) New With {Key d.Table.Name, Key d.TemporaryAlias, Key .ColumnName = d.Name}).Select(Function(d) d.FirstOrDefault()).ToList()
                    'Group New With {.TableName = Column.Table.Name,
                    '                 .TableDatabaseName = Column.Table.DatabaseName,
                    '                 .TableDBVariableName = Column.Table.DBVariableName,
                    '                 .ColumnName = Column.Name,
                    '                 .ColumnTemporaryAlias = Column.TemporaryAlias,
                    '                 .ColumnAsName = Column.AsName,
                    '                 .ColumnRoleFactTypeIsDerived = Column.Role.FactType.IsDerived,
                    '                 .ColumnRoleJoinedORMObjectGetType = Column.Role.JoinedORMObject.GetType,
                    '                 .ColumnRoleFactTypeDBVariableName = Column.Role.FactType.DBVariableName} By newGroup Into Group
                    'Select newGroup
                    ' ).Distinct.ToList

                    Me.ProjectionColumn = larProjectColumn

                    For Each lrProjectColumn In larProjectColumn

                        If lrProjectColumn.Role.FactType.IsDerived Then
                            If lrProjectColumn.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                                'for now
                                lsSelectClause &= "$" & lrProjectColumn.Table.DatabaseName & lrProjectColumn.Name
                            Else
                                lsSelectClause &= "$" & lrProjectColumn.Role.FactType.DBVariableName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                            End If

                        Else
                            lsSelectClause &= LCase(lrProjectColumn.Table.DBVariableName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "")) & "." & lrProjectColumn.Name
                            If lrProjectColumn.AsName IsNot Nothing Then
                                lsSelectClause &= " AS " & lrProjectColumn.AsName
                            End If
                        End If
                        If liInd < larProjectColumn.Count Then lsSelectClause &= ","
                        liInd += 1
                    Next

                    lsCypherQuery &= lsSelectClause

                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        Dim larCountStarColumn = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                 Where ReturnColumn.KEYWDCOUNTSTAR IsNot Nothing
                                                 Select ReturnColumn

                        If larCountStarColumn.Count > 0 Then
                            If arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN.Count > 1 Then
                                lsCypherQuery &= ","
                            End If
                            lsCypherQuery &= " COUNT(*)"
                            If larCountStarColumn(0).ASCLAUSE IsNot Nothing Then
                                lsCypherQuery &= " AS " & larCountStarColumn(0).ASCLAUSE.COLUMNNAMESTR
                            End If
                            lbRequiresGroupByClause = True
                        End If
                    End If

                    If Not abIsStraightDerivationClause And NullVal(My.Settings.FactEngineDefaultQueryResultLimit, 0) > 0 Then
                        lsCypherQuery &= vbCrLf & "LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                    End If

                    lsCypherQuery &= ";"
                End If
#End Region

                Return lsCypherQuery

            Catch ex As Exception

                Throw New Exception("QueryGraph.generateTypeQL" & vbCrLf & ex.Message & vbCrLf & vbCrLf & lsCypherQuery)

            End Try

        End Function

    End Class

End Namespace
