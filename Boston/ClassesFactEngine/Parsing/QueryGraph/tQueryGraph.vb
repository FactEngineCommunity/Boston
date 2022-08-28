Namespace FactEngine
    Public Class QueryGraph

        Public Model As FBM.Model

        '============================================
        'Example Fact Engine Query
        '-----------------------------
        'WHICH Lecturer is located in WHICH Room 
        'AND holds WHICH Position
        'AND is in WHICH School
        'AND is in A School WHICH is in (Factulty:'IT') 
        'AND THAT Lecturer works for THAT Faculty 
        '------------------------------------------

        ''' <summary>
        ''' The HeadNode of the FactEngine WHICH Query, as in Lecturer in "WHICH Lecturer is located in WHICH Room"
        ''' </summary>
        Public HeadNode As New FactEngine.QueryNode()

        Public QueryEdges As New List(Of FactEngine.QueryEdge)

        Public Warning As New List(Of String)

        'The set of Nodes queried in a WHICH query statement
        Public Nodes As New List(Of FactEngine.QueryNode)

        ''' <summary>
        ''' The list of ProjectionColumns for the Query. Used to find and display Nodes/Links in QueryView in FactEngine form.
        ''' </summary>
        Public ProjectionColumn As New List(Of RDS.Column)

        Public Sub New(ByRef arFBMModel As FBM.Model)
            Me.Model = arFBMModel
        End Sub

        Public Function FindPreviousQueryEdgeBaseNodeByModelElementName(ByVal asModelElementName As String,
                                                                        ByRef arBaseNode As FactEngine.QueryNode) As FactEngine.QueryNode

            Try
                Dim liInd As Integer = Me.QueryEdges.Count - 1

                If Me.QueryEdges.Count = 0 Then
                    Return arBaseNode
                Else
                    For Each lrQueryEdge In Me.QueryEdges.ToArray.Reverse
                        If lrQueryEdge.BaseNode.Name = asModelElementName Then
                            Return lrQueryEdge.BaseNode
                        End If
                    Next
                End If

                Return Nothing

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

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

                Me.ProjectionColumn.RemoveAll(Function(x) x.Table.FBMModelElement.GetType = GetType(FBM.FactType) And x.Role.FactType.Id = x.Table.Name And x.Role.JoinedORMObject.GetType <> GetType(FBM.ValueType))
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
                    GoTo ReturnClause 'There is no WHERE Conditionals.
                Else
                    lsCypherQuery &= vbCrLf & "WHERE "
                End If


#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = Nothing
                Dim lbHasWhereClause As Boolean = False

#Region "WhereConditionals"
                If Me.HeadNode.HasIdentifier And Not Me.QueryEdges(0).IsRecursive Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrTargetTable.DatabaseName) & Viev.NullVal(Me.HeadNode.Alias, "") & "." & lrColumn.Name & " = '" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then
                            lsCypherQuery &= "AND "
                        End If
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
                End If

                For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))
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

                            End If

                            lbIntialWhere = "AND "
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
                                        lbIntialWhere = "AND "
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
                                            For Each lsIdentifier In lrQueryEdge.IdentifierList
                                                If liInd > 0 Then
                                                    lsCypherQuery &= "AND "
                                                    lbIntialWhere = ""
                                                End If
                                                lsCypherQuery &= Viev.NullVal(lbIntialWhere, "") & LCase(lrTargetTable.DatabaseName) & lsAlias & "." & larIndexColumns(liInd).Name & lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "'" & vbCrLf
                                                liInd += 1
                                            Next
                                        End If

                                        lbIntialWhere = "AND "
                                    End If
                            End Select
                    End Select
                    lbHasWhereClause = True
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
                    If Me.ProjectionColumn.Count > 0 Then
                        lsCypherQuery &= vbCrLf & "RETURN "
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
                            lsCypherQuery &= ", COUNT(*)"
                            If larCountStarColumn(0).ASCLAUSE IsNot Nothing Then
                                lsCypherQuery &= " AS " & larCountStarColumn(0).ASCLAUSE.COLUMNNAMESTR
                            End If
                            lbRequiresGroupByClause = True
                        End If
                    End If

                    lsCypherQuery &= ";"
                End If
#End Region

                Return lsCypherQuery

            Catch ex As Exception

                Throw New Exception("QueryGraph.generateTypeQL" & vbCrLf & ex.Message & vbCrLf & vbCrLf & lsCypherQuery)

            End Try

        End Function


        Public Function generateTypeQL(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement,
                                       Optional ByVal abIsCountStarSubQuery As Boolean = False,
                                       Optional ByVal abIsStraightDerivationClause As Boolean = False,
                                       Optional ByRef arDerivedModelElement As FBM.ModelObject = Nothing,
                                       Optional ByRef abIsSubQuery As Boolean = False) As String
            Dim lsTDBQuery As String
            If abIsSubQuery Then
                lsTDBQuery = " not {"
            Else
                lsTDBQuery = "match "
            End If

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

                Me.ProjectionColumn.RemoveAll(Function(x) x.Table.FBMModelElement.GetType = GetType(FBM.FactType) And x.Role.FactType.Id = x.Table.Name And x.Role.JoinedORMObject.GetType <> GetType(FBM.ValueType))
#End Region


#Region "Conditionals for Match from clause"
                Dim larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                             Where QueryEdge.TargetNode IsNot Nothing
                                             Select QueryEdge
                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None))
                '20210826-VM-Removed
                'And (Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

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

#Region "FromClause"
                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)

                'Circular/Recursive QueryEdges/TargetNodes are treated differently. See Recursive region below.
                Dim larNode As List(Of QueryNode) = (From Node In larFromNodes
                                                     Where Node.QueryEdge IsNot Nothing
                                                     Where Node.QueryEdge.IsRecursive
                                                     Where (Node.QueryEdge.IsCircular Or
                                                            Node.RDSTable.isCircularToTable(Node.QueryEdge.BaseNode.RDSTable))).ToList
                larFromNodes.RemoveAll(Function(x) larNode.Contains(x))

                Dim larSubQueryNodes As List(Of FactEngine.QueryNode) = (From Node In larFromNodes
                                                                         Where Node.QueryEdge IsNot Nothing
                                                                         Where (Node.QueryEdge.IsSubQueryLeader Or Node.QueryEdge.IsPartOfSubQuery)
                                                                         Where Not (Node.QueryEdge.BaseNode Is Node And Node.QueryEdge.IsSubQueryLeader)
                                                                         Select Node
                                                                        ).ToList

                larFromNodes.RemoveAll(Function(x) larSubQueryNodes.Contains(x))

                liInd = 0
                For Each lrQueryNode In larFromNodes.FindAll(Function(x) Not (x.FBMModelObject.IsDerived Or x.FBMModelObject.isUnaryFactType))

                    Select Case lrQueryNode.FBMModelObject.GetType
                        Case Is = GetType(FBM.FactType)

                            Dim larLinkFactTypeQueryEdge = From QueryEdge In Me.QueryEdges
                                                           Where QueryEdge.FBMFactType.IsLinkFactType
                                                           Where QueryEdge.FBMFactType.LinkFactTypeRole.FactType Is lrQueryNode.FBMModelObject
                                                           Select QueryEdge

                            lsTDBQuery &= "$" & lrQueryNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryNode.Alias, "")
                            lsTDBQuery &= " ("

                            Dim liInd2 As Integer = 0
                            For Each lrLinkFactTypeQueryEdge In larLinkFactTypeQueryEdge
                                If liInd2 > 0 Then lsTDBQuery &= ", "
                                Dim lrNodeToUse As FactEngine.QueryNode = lrLinkFactTypeQueryEdge.TargetNode
                                If lrNodeToUse Is lrQueryNode Then
                                    lrNodeToUse = lrLinkFactTypeQueryEdge.BaseNode
                                End If
                                lsTDBQuery &= lrLinkFactTypeQueryEdge.FBMFactType.LinkFactTypeRole.Name & ": $" & lrNodeToUse.DBVariableName & lrLinkFactTypeQueryEdge.TargetNode.Alias
                                liInd2 += 1
                            Next

                            lsTDBQuery &= ") isa " & lrQueryNode.RDSTable.DatabaseName

                        Case Else
                            lsTDBQuery &= "$" & lrQueryNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryNode.Alias, "") & " isa " & lrQueryNode.RDSTable.DatabaseName
                    End Select

                    '----------------------------------------------------
                    'Return Columns
                    Dim larReturnColumn = From Column In Me.ProjectionColumn
                                          Where (Column.Table.Name = lrQueryNode.RDSTable.Name Or
                                                 lrQueryNode.FBMModelObject.isSubtypeOfModelElement(Column.Table.FBMModelElement))
                                          Where Column.TemporaryAlias = lrQueryNode.Alias
                                          Select Column

                    For Each lrReturnColumn In larReturnColumn
                        lsTDBQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                    Next

                    'Variable Columns
                    For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) (x.TargetNode.Name = lrQueryNode.Name And x.TargetNode.Alias = lrQueryNode.Alias) Or
                                                                                        (x.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) And
                                                                                         x.BaseNode.Name = lrQueryNode.Name And
                                                                                         x.BaseNode.Alias = lrQueryNode.Alias))
                        Select Case lrQueryEdge.TargetNode.FBMModelObject.GetType
                            Case Is = GetType(FBM.ValueType)
                                Dim lrColumn As RDS.Column = lrQueryEdge.RDSColumn
                                If lrColumn.Table.Name <> lrQueryNode.Name Then Exit Select
                                If larReturnColumn.ToList.Find(AddressOf lrColumn.Equals) Is Nothing Then
                                    lsTDBQuery &= ", has " & lrColumn.Name & " $" & lrColumn.Table.DBVariableName & Viev.NullVal(lrQueryNode.Alias, "") & lrColumn.Name
                                End If
                            Case Is = GetType(FBM.EntityType)
                                Dim larConditionalColumn As List(Of RDS.Column) = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                                For Each lrColumn In larConditionalColumn
                                    If larReturnColumn.ToList.Find(AddressOf lrColumn.Equals) Is Nothing Then
                                        lsTDBQuery &= ", has " & lrColumn.Name & " $" & lrColumn.Table.DBVariableName & Viev.NullVal(lrQueryNode.Alias, "") & lrColumn.Name
                                    End If
                                Next
                        End Select
                    Next

                    'Columns on BaseNode
                    Dim larBaseNodeColumn = From QueryEdge In Me.QueryEdges
                                            Where QueryEdge.BaseNode Is lrQueryNode
                                            From Table In Me.Model.RDS.Table
                                            From Column In Table.Column
                                            Where QueryEdge.FBMFactType Is Column.Role.FactType
                                            Where Column.Table.Name = lrQueryNode.Name
                                            Where Not larConditionalQueryEdges.FindAll(Function(x) x.TargetNode.Name = lrQueryNode.Name).Contains(QueryEdge)
                                            Select Column

                    For Each lrColumn In larBaseNodeColumn
                        If larReturnColumn.ToList.Find(AddressOf lrColumn.Equals) Is Nothing Then
                            lsTDBQuery &= ", has " & lrColumn.Name & " $" & lrColumn.Table.DBVariableName & lrColumn.Name
                        End If
                    Next
                    '----------------------------------------------------

                    lsTDBQuery &= ";" & vbCrLf
                Next

                '=================================================================================================
                'PartialFactTypeReadingMatch - I.e. Joins on ManyToMany(..ToMany) tables
                Dim larPartialFTMatchNode = From Node In larFromNodes
                                            Where Node.QueryEdge IsNot Nothing
                                            Where Node.QueryEdge.IsPartialFactTypeMatch
                                            Select Node

                Dim larPartialFTMatchFT = From Node In larPartialFTMatchNode
                                          Where Not Node.QueryEdge.IsSubQueryLeader
                                          Group Node By Node.QueryEdge.FBMFactType, Node.QueryEdge.Alias Into grp = Group


                For Each lrFactTypeMatch In larPartialFTMatchFT

                    Dim lrFactType As FBM.FactType = lrFactTypeMatch.FBMFactType
                    '------------------------------------------------------------------------------
                    'Sample query
                    '(student: $Email,school: $School_Name,course: $Course_Code) isa studentship;

                    Dim larLinkedNodes = (From Node In larFromNodes
                                          Where Node.QueryEdge.FBMFactType Is lrFactType
                                          Where Node.QueryEdge.Alias = lrFactTypeMatch.Alias
                                          Select Node).ToList

                    'A Node may have been missed because it comes from earlier in the query.
                    Dim larQueryEdge = From QueryEdge In Me.QueryEdges
                                       Where QueryEdge.FBMFactType Is lrFactType
                                       Where QueryEdge.Alias = lrFactTypeMatch.Alias
                                       Select QueryEdge

                    Dim lrPrimaryQueryEdge As FactEngine.QueryEdge = Nothing 'The QueryEdge that we are talking about
                    For Each lrQueryEdge In larQueryEdge
                        lrPrimaryQueryEdge = lrQueryEdge
                        larLinkedNodes.AddUnique(lrQueryEdge.BaseNode)
                        larLinkedNodes.AddUnique(lrQueryEdge.TargetNode)
                    Next

                    lsTDBQuery &= "("
                    liInd = 0
                    Dim larUsedPredicatePart As New List(Of FBM.PredicatePart)
                    For Each lrNode In larLinkedNodes
                        If liInd > 0 Then lsTDBQuery.AppendString(",")

                        Dim larPredicatePart As New List(Of FBM.PredicatePart)
                        If lrNode.QueryEdge Is lrPrimaryQueryEdge Then
                            larPredicatePart = (From PredicatePart In lrNode.QueryEdge.FBMFactTypeReading.PredicatePart
                                                Where (PredicatePart.Role.JoinedORMObject.Id = lrNode.FBMModelObject.Id Or
                                                PredicatePart.Role.JoinedORMObject.isSubtypeOfModelElement(lrNode.FBMModelObject))
                                                Where Not larUsedPredicatePart.Contains(PredicatePart)
                                                Select PredicatePart).ToList
                        Else
                            'If lrPrimaryQueryEdge.FBMPredicatePart IsNot Nothing Then
                            'larPredicatePart.Insert(0, lrPrimaryQueryEdge.FBMPredicatePart)
                            'Else
                            larPredicatePart = (From PredicatePart In lrPrimaryQueryEdge.FBMFactTypeReading.PredicatePart
                                                Where (PredicatePart.Role.JoinedORMObject.Id = lrNode.FBMModelObject.Id Or
                                                       PredicatePart.Role.JoinedORMObject.isSubtypeOfModelElement(lrNode.FBMModelObject))
                                                Where Not larUsedPredicatePart.Contains(PredicatePart)
                                                Select PredicatePart).ToList
                            'End If
                        End If

                        Dim lrPredicatePart = larPredicatePart.First
                        larUsedPredicatePart.Add(lrPredicatePart)
                        Select Case lrNode.FBMModelObject.GetType
                            Case Is = GetType(FBM.ValueType)
                                lsTDBQuery &= lrPredicatePart.Role.Name & ": $" & lrFactType.getCorrespondingRDSTable.DatabaseName & lrNode.DBVariableName & lrNode.Alias
                            Case Else
                                lsTDBQuery &= lrPredicatePart.Role.Name & ": $" & lrNode.RDSTable.DatabaseName & lrNode.Alias
                        End Select


                        liInd += 1
                    Next
                    lsTDBQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName

                    '----------------------------------------------------
                    'Return Columns
                    Dim larReturnColumn = From Column In Me.ProjectionColumn
                                          Where (Column.Table.Name = lrFactType.getCorrespondingRDSTable.Name Or
                                                 lrFactType.isSubtypeOfModelElement(Column.Table.FBMModelElement))
                                          Where Column.TemporaryAlias = lrFactTypeMatch.Alias
                                          Where Not Column.isPartOfPrimaryKey
                                          Select Column

                    For Each lrReturnColumn In larReturnColumn
                        lsTDBQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                    Next

                    lsTDBQuery &= ";" & vbCrLf

                Next

#Region "Unary FactType Nodes"

                For Each lrNode In larFromNodes.FindAll(Function(x) x.FBMModelObject.isUnaryFactType)

                    lsTDBQuery &= "("

                    Dim lrFactType As FBM.FactType = lrNode.FBMModelObject
                    Dim lrRole As FBM.Role = lrFactType.RoleGroup(0)

                    'See if a QueryEdge exists that maps to the Table of the UnaryFactType.
                    Dim larUnaryFactTypeQueryEdge = From QueryEdge In Me.QueryEdges
                                                    Where QueryEdge.FBMFactType.IsLinkFactType
                                                    Where QueryEdge.TargetNode IsNot Nothing
                                                    Where QueryEdge.TargetNode.FBMModelObject Is lrRole.JoinedORMObject
                                                    Select QueryEdge

                    If larUnaryFactTypeQueryEdge.Count > 0 Then
                        lrNode = larUnaryFactTypeQueryEdge.First.TargetNode
                        lsTDBQuery &= lrRole.Name & ": $" & lrNode.DBVariableName & lrNode.Alias
                    Else
                        lsTDBQuery &= lrRole.Name & ": $" & lrRole.getCorrespondingRDSTable.DatabaseName
                    End If

                    lsTDBQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName

                    '----------------------------------------------------
                    'Return Columns
                    Dim larReturnColumn = From Column In Me.ProjectionColumn
                                          Where (Column.Table.Name = lrFactType.getCorrespondingRDSTable.Name Or
                                                 lrFactType.isSubtypeOfModelElement(Column.Table.FBMModelElement))
                                          Where Not Column.isPartOfPrimaryKey
                                          Select Column

                    For Each lrReturnColumn In larReturnColumn
                        lsTDBQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                    Next

                    lsTDBQuery &= ";" & vbCrLf
                Next
#End Region

                '=================================================================================================
                'Derived FactTypeReadingMatch - I.e. Joins on ManyToMany(..ToMany) tables
                Dim larCoveredFactTypes = (From FactTypeMatch In larPartialFTMatchFT
                                           Select FactTypeMatch.FBMFactType).ToList

                larPartialFTMatchNode = (From Node In larFromNodes
                                         Where Node.QueryEdge IsNot Nothing
                                         Where Node.QueryEdge.IsPartialFactTypeMatch
                                         Where Node.QueryEdge.FBMFactType.IsDerived
                                         Where Not larCoveredFactTypes.Contains(Node.QueryEdge.FBMFactType)
                                         Select Node).ToList

                Dim larPartialFTMatchFT2 = From Node In larPartialFTMatchNode
                                           Select Node.QueryEdge.FBMFactType Distinct

                For Each lrFactType In larPartialFTMatchFT2
                    '------------------------------------------------------------------------------
                    'Sample query
                    '(student: $Email,school: $School_Name,course: $Course_Code) isa studentship;

                    Dim larLinkedNodes = From Node In larFromNodes
                                         Where Node.QueryEdge.FBMFactType Is lrFactType
                                         Select Node

                    lsTDBQuery &= "("
                    liInd = 0
                    For Each lrNode In larLinkedNodes
                        If liInd > 0 Then lsTDBQuery.AppendString(",")

                        Dim larPredicatePart = From PredicatePart In lrNode.QueryEdge.FBMFactTypeReading.PredicatePart
                                               Where PredicatePart.Role.JoinedORMObject Is lrNode.FBMModelObject
                                               Select PredicatePart

                        lsTDBQuery &= larPredicatePart.First.Role.Name & ": $" & lrNode.RDSTable.DatabaseName
                        liInd += 1
                    Next
                    lsTDBQuery &= ") isa " & lrFactType.getCorrespondingRDSTable.DatabaseName & ";" & vbCrLf

                Next


#End Region


#Region "WHERE Clauses"

                'WHERE
                'WhereEdges are Where joins, rather than ConditionalQueryEdges which test for values by identifiers.
                Dim larWhereEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType) Or
                x.IsRDSTable Or x.IsDerived)

                'Remove Link Fact Types
                larWhereEdges.RemoveAll(Function(x) x.FBMFactType.IsLinkFactType)

                'Add special DerivedFactType WhereEdges
                Dim larSpecialDerivedQueryEdges = From QueryEdge In Me.QueryEdges
                                                  Where QueryEdge.FBMFactType IsNot Nothing
                                                  Where QueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count
                                                  Select QueryEdge

                larWhereEdges.AddRange(larSpecialDerivedQueryEdges.ToList)

                '20210926-VM-This might be better served by removing QueryEdges that have already been processed.
                larWhereEdges.RemoveAll(Function(x) x.BaseNode.FBMModelObject.isUnaryFactType)

                '20210911-VM-Moved to top so that variables can be put in From clause.
                'Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                'larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                '                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None))
                ''20210826-VM-Removed
                ''And (Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                ''BooleanPredicate edges. E.g. Protein is enzyme
                'Dim larExtraConditionalQueryEdges = From QueryEdge In Me.QueryEdges
                '                                    Where QueryEdge.FBMFactType IsNot Nothing
                '                                    Where QueryEdge.TargetNode Is Nothing And Not (QueryEdge.FBMFactType.IsDerived And QueryEdge.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))
                '                                    Select QueryEdge
                'larConditionalQueryEdges.AddRange(larExtraConditionalQueryEdges.ToList)

                ''ShortestPath conditionals are excluded.
                ''  E.g. For '(Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) '
                ''  the second QueryEdge conditional is taken care of in the FROM clause processing for the recursive query.
                'larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                ''Recursive NodePropertyIdentification conditionals are excluded.
                'larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

#Region "WHERE Joins"
                liInd = 1
                Dim lbHasWhereClause As Boolean = False

                For Each lrQueryEdge In larWhereEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))

                    Dim lrOriginTable As RDS.Table

                    If lrQueryEdge.IsPartialFactTypeMatch Then 'And lrQueryEdge.TargetNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
#Region "Partial FactType match"
                        '==================================================================================================
                        'Taken care of in From section (above). 20210909.
                        'Dim lrNaryTable As RDS.Table = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                        ''Dim lrOtherRole As FBM.Role = lrQueryEdge.FBMFactType.GetOtherRoleOfBinaryFactType(lrQueryEdge.FBMPredicatePart.Role.Id)
                        'lsTDBQuery &= "("
                        'liInd = 0
                        'For Each lrColumn In lrNaryTable.getPrimaryKeyColumns
                        '    If liInd > 0 Then lsTDBQuery.AppendString(",")
                        '    lsTDBQuery &= lrColumn.Role.Name & ": $" & lrColumn.Name
                        '    liInd += 1
                        'Next
                        'lsTDBQuery.AppendString(") isa " & lrQueryEdge.FBMFactType.getCorrespondingRDSTable.DatabaseName & ";" & vbCrLf)

#End Region
                    ElseIf lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatIdentityCompatitor Then
                        'E.g. Of the type "Person 1 Is Not Person 2" or "Person 1 Is Person 2"
#Region "AndThatIdentityComparitor. 'E.g. Of the type 'Person 1 Is Not Person 2' or 'Person 1 Is Person 2'"
                        lsTDBQuery &= "$" & lrQueryEdge.BaseNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                        If lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.ISClause Then
                            lsTDBQuery &= " = "
                        Else
                            lsTDBQuery &= " != "
                        End If
                        lsTDBQuery &= "$" & lrQueryEdge.TargetNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "")
                        lsTDBQuery &= ";" & vbCrLf
#End Region
                    ElseIf lrQueryEdge.IsDerived Then

                        '==================================================================================================
                        'Taken care of in From section (above). 20210909.

                        'lrOriginTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable(Nothing, True)

                        'If lrOriginTable Is Nothing Then
                        '    lrOriginTable = New RDS.Table(Me.Model.RDS, lrQueryEdge.FBMFactType.Id, lrQueryEdge.FBMFactType)
                        'End If
                        'liInd = 0
                        'For Each lrRole In lrQueryEdge.FBMFactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.GetType <> GetType(FBM.ValueType))
                        '    Dim lrDestinationTable As RDS.Table = lrRole.JoinedORMObject.getCorrespondingRDSTable

                        '    Dim liInd2 As Integer = 0
                        '    For Each lrColumn In lrDestinationTable.getPrimaryKeyColumns
                        '        Dim lrOriginColumn As RDS.Column = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                        '        If lrOriginColumn Is Nothing Then
                        '            lsTDBQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = "
                        '        Else
                        '            lsTDBQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrOriginColumn.Name & " = "
                        '        End If
                        '        lsTDBQuery &= lrDestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & vbCrLf
                        '        liInd2 += 1
                        '    Next
                        '    liInd += 1
                        'Next

                    ElseIf (lrQueryEdge.FBMFactType.isRDSTable And lrQueryEdge.FBMFactType.Arity = 2) Then

                        'RDSTable
#Region "PGSNodeTable/RDSTable"
                        Dim lrOtherRole As FBM.Role = lrQueryEdge.FBMFactType.GetOtherRoleOfBinaryFactType(lrQueryEdge.FBMPredicatePart.Role.Id)
                        lsTDBQuery &= "(" & lrQueryEdge.FBMPredicatePart.Role.Name & ": $" & lrQueryEdge.BaseNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & ","
                        Select Case lrQueryEdge.TargetNode.FBMModelObject.GetType
                            Case Is = GetType(FBM.ValueType)
                                lsTDBQuery &= lrOtherRole.Name & ": $" & lrQueryEdge.FBMFactType.DBName & lrQueryEdge.TargetNode.DBVariableName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & ") isa " & lrQueryEdge.FBMFactType.DBName
                            Case Else
                                lsTDBQuery &= lrOtherRole.Name & ": $" & lrQueryEdge.TargetNode.DBVariableName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & ") isa " & lrQueryEdge.FBMFactType.DBName
                        End Select

                        '----------------------------------------------------
                        'Return Columns
                        Dim larReturnColumn = From Column In Me.ProjectionColumn
                                              Where Column.Table.Name = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Name
                                              Where Column.TemporaryAlias = lrQueryEdge.Alias
                                              Where Not Column.isPartOfPrimaryKey
                                              Select Column

                        For Each lrReturnColumn In larReturnColumn
                            lsTDBQuery &= ", has " & lrReturnColumn.Name & " $" & lrReturnColumn.Table.DBVariableName & lrReturnColumn.TemporaryAlias & lrReturnColumn.Name
                        Next

                        lsTDBQuery &= ";" & vbCrLf

#End Region
                    ElseIf lrQueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count Then
#Region "DerivationType = COUNT"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        lrBaseNode = lrQueryEdge.BaseNode
                        lrTargetNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge)

                        lsTDBQuery &= lrBaseNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name & " = " & lrTargetNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name
#End Region
                    Else
#Region "Other/Else"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        If lrQueryEdge.IsReciprocal Then
                            lrBaseNode = lrQueryEdge.TargetNode
                            lrTargetNode = lrQueryEdge.BaseNode
                        ElseIf lrQueryEdge.IsPartialFactTypeMatch Then
                            lrBaseNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge, False)
                            lrTargetNode = lrQueryEdge.TargetNode
                        Else
                            lrBaseNode = lrQueryEdge.BaseNode
                            lrTargetNode = lrQueryEdge.TargetNode
                        End If

                        lrOriginTable = lrBaseNode.RDSTable
                        Dim larModelObject = New List(Of FBM.ModelObject)
                        larModelObject.Add(lrBaseNode.FBMModelObject)
                        larModelObject.Add(lrTargetNode.FBMModelObject)
                        Dim lrRelation = lrOriginTable.getRelationByFBMModelObjects(larModelObject, lrQueryEdge.FBMFactType, lrQueryEdge)

                        Dim liInd2 = 1
                        If lrRelation.OriginTable Is lrOriginTable Then
                            Dim larOriginColumn As New List(Of RDS.Column)
                            Dim larTargetColumn As New List(Of RDS.Column)
                            'was  larTargetColumn = lrQueryEdge.TargetNode.RDSTable.getPrimaryKeyColumns ' FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                            For Each lrColumn In lrRelation.OriginColumns
                                larOriginColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next

                            For Each lrColumn In lrRelation.DestinationColumns
                                larTargetColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next

                            lsTDBQuery &= "($" & lrQueryEdge.BaseNode.RDSTable.DBVariableName & Viev.NullVal(lrBaseNode.Alias, "") & ","
                            lsTDBQuery &= "$" & lrQueryEdge.TargetNode.RDSTable.DBVariableName & Viev.NullVal(lrTargetNode.Alias, "") & ") isa " & lrQueryEdge.FBMFactType.DatabaseName & ";" & vbCrLf

                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns

                            lsTDBQuery &= "($" & lrQueryEdge.TargetNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & ","
                            lsTDBQuery &= "$" & lrQueryEdge.BaseNode.RDSTable.DBVariableName & ") " & " isa " & lrQueryEdge.FBMFactType.Id


                        End If
#End Region
                    End If

                    'CodeSafe Remove wayward ANDs           
                    If Not lsTDBQuery.EndsWith(vbCrLf) Then lsTDBQuery &= vbCrLf

                    liInd += 1
                    lbHasWhereClause = True
                Next

#End Region

#Region "WhereConditionals"
                Dim lbFirstQueryEdgeIsRecursive As Boolean = False
                If Me.QueryEdges.Count > 0 Then
                    If Me.QueryEdges(0).IsRecursive Then lbFirstQueryEdgeIsRecursive = True
                End If
                If Me.HeadNode.HasIdentifier And Not lbFirstQueryEdgeIsRecursive Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsTDBQuery &= "$" & lrTargetTable.DatabaseName & Viev.NullVal(Me.HeadNode.Alias, "") & lrColumn.Name & " = '" & Me.HeadNode.IdentifierList(liInd) & "';" & vbCrLf
                        liInd += 1
                    Next
                End If

                For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))
                    Select Case lrQueryEdge.WhichClauseSubType
                        Case Is = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                            Dim lrFactType As FBM.FactType = Nothing
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

                            Dim lrPredicatePart As FBM.PredicatePart = Nothing

                            If lrQueryEdge.FBMPredicatePart IsNot Nothing Then
                                lrPredicatePart = lrQueryEdge.FBMPredicatePart
                            Else

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

                                lsTDBQuery &= "$" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsTDBQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select
                                lsTDBQuery &= Me.Model.DatabaseConnection.ComparitorOperator(lrQueryEdge.TargetNode.Comparitor)
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                        Dim loDateTime As DateTime = Nothing
                                        If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                            Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                        End If
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & ";" & vbCrLf
                                End Select

                            Else
                                lrTable = lrQueryEdge.BaseNode.RDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsTDBQuery &= "$" & lrQueryEdge.BaseNode.RDSTable.DBVariableName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsTDBQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select
                                lsTDBQuery &= Me.Model.DatabaseConnection.ComparitorOperator(lrQueryEdge.TargetNode.Comparitor)
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                        Dim loDateTime As DateTime = Nothing
                                        If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                            Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                        End If
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & ";" & vbCrLf
                                End Select

                            End If
                        Case Else

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsTDBQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & "."

                                    Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsTDBQuery &= lrTargetColumn.Name & " = True"

                                Case Else



                                    If lrQueryEdge.TargetNode.MathFunction <> pcenumMathFunction.None Then

                                        If lrQueryEdge.FBMFactType.IsDerived Then

                                            lsTDBQuery &= lrQueryEdge.FBMFactType.Id &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              CType(lrQueryEdge.TargetNode.PreboundText & lrQueryEdge.TargetNode.Name, String).Replace("-", "")

                                            lsTDBQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsTDBQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf

                                        Else


                                            'Math function
                                            Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                            lsTDBQuery &= "$" & lrTargetColumn.Table.DBVariableName & lrTargetColumn.Name & lrTargetColumn.TemporaryAlias

                                            'lrTargetTable.DatabaseName &
                                            '  Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                            '  "." &
                                            '  lrTargetColumn.Name

                                            lsTDBQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsTDBQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & ";" & vbCrLf

                                        End If
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
                                                lsTDBQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = "
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0))
                                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                End Select
                                            Else
                                                lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                                lsTDBQuery &= "$" & lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & lrColumn.Name
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDate,
                                                              pcenumORMDataType.TemporalDateAndTime
                                                        lsTDBQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                                End Select
                                                lsTDBQuery &= lrQueryEdge.getTargetSQLComparator
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0), True)
                                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsTDBQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & ";" & vbCrLf
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

                                            If larIndexColumns.Count = 0 Then
                                                larIndexColumns = lrTargetTable.getPrimaryKeyColumns
                                            End If

                                            liInd = 0
                                            For Each lsIdentifier In lrQueryEdge.IdentifierList
                                                lsTDBQuery &= "$" & lrTargetTable.DBVariableName & lsAlias & " has " & larIndexColumns(liInd).Name & lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "';" & vbCrLf
                                                liInd += 1
                                            Next
                                        End If

                                    End If
                            End Select
                    End Select
                    lbHasWhereClause = True
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
                    lsTDBQuery &= lrSubQueryGraph.generateTypeQL(arWhichSelectStatement, True,,, True)
                    lsTDBQuery &= "};"
                Next
                '=====================================================================================
#End Region


#Region "RETURN/GET Clause"

                If Not abIsSubQuery Then
                    If Me.ProjectionColumn.Count > 0 Then
                    lsTDBQuery &= " get "
                End If

                liInd = 1
                    For Each lrProjectColumn In Me.ProjectionColumn.FindAll(Function(x) x IsNot Nothing)

                        If lrProjectColumn.Role.FactType.IsDerived Then
                            If lrProjectColumn.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                                'for now
                                lsSelectClause &= "$" & lrProjectColumn.Table.DatabaseName & lrProjectColumn.Name
                            Else
                                lsSelectClause &= "$" & lrProjectColumn.Role.FactType.DBVariableName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                            End If

                        Else
                            lsSelectClause &= "$" & lrProjectColumn.Table.DBVariableName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & lrProjectColumn.Name
                            If lrProjectColumn.AsName IsNot Nothing Then
                                lsSelectClause &= " AS " & lrProjectColumn.AsName
                            End If
                        End If
                        If liInd < larProjectionColumn.Count Then lsSelectClause &= ","
                        liInd += 1
                    Next

                    lsTDBQuery &= lsSelectClause

                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        Dim larCountStarColumn = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                 Where ReturnColumn.KEYWDCOUNTSTAR IsNot Nothing
                                                 Select ReturnColumn

                        If larCountStarColumn.Count > 0 Then
                            lsTDBQuery &= ", COUNT(*)"
                            If larCountStarColumn(0).ASCLAUSE IsNot Nothing Then
                                lsTDBQuery &= " AS " & larCountStarColumn(0).ASCLAUSE.COLUMNNAMESTR
                            End If
                            lbRequiresGroupByClause = True
                        End If
                    End If

                    lsTDBQuery &= ";"
                End If
#End Region

                Return lsTDBQuery

            Catch ex As Exception

                Throw New Exception("QueryGraph.generateTypeQL" & vbCrLf & ex.Message & vbCrLf & vbCrLf & lsTDBQuery)

            End Try


        End Function

        ''' <summary>
        ''' Generates SQL to run against the database for this QueryGraph
        ''' </summary>
        ''' <param name="arWhichSelectStatement">The WhichSelectStatement from which the SQL is generated.
        ''' NB Subqueries are generated using this same function. For a subquery, contains the WhichSelectStatement if the subquery.</param>
        ''' <param name="abIsCountStarSubQuery">? TBA</param>
        ''' <param name="abIsStraightDerivationClause">TRUE if called to generate the SQL for a DerivedFactType. A specialised set of ProjectColumns is returned, using PKs rather than UCs.</param>
        ''' <param name="arDerivedModelElement">The DerivedModelElement if is called to generate the SQL for a DerivedModelElement.</param>
        ''' <returns></returns>
        Public Function generateSQL(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement,
                                    Optional ByVal abIsCountStarSubQuery As Boolean = False,
                                    Optional ByVal abIsStraightDerivationClause As Boolean = False,
                                    Optional ByRef arDerivedModelElement As FBM.ModelObject = Nothing,
                                    Optional ByRef abIsSubQuery As Boolean = False) As String

            Dim lsSQLQuery As String = ""
            Dim liInd As Integer
            Dim larColumn As New List(Of RDS.Column)
            Dim lbRequiresGroupByClause As Boolean = False
            Dim lsSelectClause As String = ""

            Try
                'Set the Node Aliases. E.g. If Lecturer occurs twice in the FROM clause, then Lecturer, Lecturer2 etc
                Call Me.setNodeAliases()
                Call Me.setQueryEdgeAliases()

                If Not abIsCountStarSubQuery Then
                    lsSQLQuery = "SELECT "
                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        If arWhichSelectStatement.RETURNCLAUSE.KEYWDDISTINCT IsNot Nothing Then
                            lsSQLQuery &= "DISTINCT "
                        End If
                    End If
#Region "ProjectionColums"
                    liInd = 1

                    Dim larProjectionColumn = Me.getProjectionColumns(arWhichSelectStatement, abIsStraightDerivationClause, arDerivedModelElement)
                    Me.ProjectionColumn = larProjectionColumn


                    For Each lrProjectColumn In larProjectionColumn.FindAll(Function(x) x IsNot Nothing)

                        If lrProjectColumn.Role.FactType.IsDerived Then
                            If lrProjectColumn.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                                'for now
                                lsSelectClause &= lrProjectColumn.Name
                            Else
                                If abIsStraightDerivationClause And Not lrProjectColumn.Role.FactType.IsObjectified Then
                                    lsSelectClause &= lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                                Else
                                    lsSelectClause &= lrProjectColumn.Role.FactType.Id & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                                End If

                            End If

                        Else
                            lsSelectClause &= lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                            If lrProjectColumn.AsName IsNot Nothing Then
                                lsSelectClause &= " AS " & lrProjectColumn.AsName
                            End If
                        End If
                        If liInd < larProjectionColumn.Count Then lsSelectClause &= ","
                        liInd += 1
                    Next
                    lsSQLQuery &= lsSelectClause

                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        Dim larCountStarColumn = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                 Where ReturnColumn.KEYWDCOUNTSTAR IsNot Nothing
                                                 Select ReturnColumn

                        If larCountStarColumn.Count > 0 Then
                            lsSQLQuery &= Boston.returnIfTrue(larProjectionColumn.Count > 0, ", ", "")
                            lsSQLQuery &= "COUNT(*)"
                            If larCountStarColumn(0).ASCLAUSE IsNot Nothing Then
                                lsSQLQuery &= " AS " & larCountStarColumn(0).ASCLAUSE.COLUMNNAMESTR
                            End If
                            If larProjectionColumn.Count > 0 Then
                                lbRequiresGroupByClause = True
                            End If
                        End If
                    End If
#End Region
                Else
                    lsSQLQuery = " 1 > (SELECT COUNT(*)"
                End If

                lsSQLQuery &= vbCrLf & "FROM "

#Region "FromClause"

                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)

                'Circular/Recursive QueryEdges/TargetNodes are treated differently. See Recursive region below.
                Dim larNode As List(Of QueryNode) = (From Node In larFromNodes
                                                     Where Node.QueryEdge IsNot Nothing
                                                     Where Node.QueryEdge.IsRecursive
                                                     Where (Node.QueryEdge.IsCircular Or
                                                            Node.RDSTable.isCircularToTable(Node.QueryEdge.BaseNode.RDSTable))).ToList
                larFromNodes.RemoveAll(Function(x) larNode.Contains(x))

                Dim larSubQueryNodes As List(Of FactEngine.QueryNode) = (From Node In larFromNodes
                                                                         Where Node.QueryEdge IsNot Nothing
                                                                         Where (Node.QueryEdge.IsSubQueryLeader Or Node.QueryEdge.IsPartOfSubQuery)
                                                                         Where Not (Node.QueryEdge.IsSubQueryLeader And Node Is Node.QueryEdge.BaseNode And Me.ProjectionColumn.Select(Function(x) x.Table.Name).Contains(Node.Name))
                                                                         Select Node
                                                                        ).ToList

                larFromNodes.RemoveAll(Function(x) larSubQueryNodes.Contains(x))

                'Derived Fact Type Parameters
                'The QueryEdge.FBMFactType is derived, the Column for the QueryNode IsDerivationParameter
                Dim larDerivationNodes = (From Node In larFromNodes
                                          Where Node.QueryEdge IsNot Nothing
                                          Where Node.QueryEdge.FBMFactType IsNot Nothing
                                          Where Node.QueryEdge.FBMFactType.IsDerived
                                          Select Node).ToList

                Dim larRemovedDerivationNode As New List(Of FactEngine.QueryNode)
                For Each lrDerivationNode In larDerivationNodes
                    'Get the Derivation Column
                    Dim lrDerivationTable As RDS.Table = Nothing
                    Dim larDerivationColumn As List(Of RDS.Column)
                    If lrDerivationNode.QueryEdge.FBMFactType.IsUnaryFactType Then
                        lrDerivationTable = lrDerivationNode.QueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable()

                        larDerivationColumn = (From Column In lrDerivationTable.Column
                                               Where Column.FactType.Id = lrDerivationNode.QueryEdge.FBMFactType.Id
                                               Select Column).ToList
                    Else
                        If lrDerivationNode.QueryEdge.FBMFactType.IsManyTo1BinaryFactType And Not lrDerivationNode.QueryEdge.FBMFactType.IsObjectified Then
                            lrDerivationTable = lrDerivationNode.FBMModelObject.getCorrespondingRDSTable
                        Else
                            lrDerivationTable = lrDerivationNode.QueryEdge.FBMFactType.getCorrespondingRDSTable
                        End If

                        larDerivationColumn = (From Column In lrDerivationTable.Column
                                               Where Column.Role.JoinedORMObject.Id = lrDerivationNode.Name
                                               Select Column).ToList
                    End If

                    If larDerivationColumn.Count > 0 Then
                        If larDerivationColumn.First.IsDerivationParameter Then
                            larFromNodes.Remove(lrDerivationNode)
                            larRemovedDerivationNode.Add(lrDerivationNode)
                        End If
                    End If
                Next

                liInd = 0
                For Each lrQueryNode In larFromNodes.FindAll(Function(x) Not x.FBMModelObject.IsDerived)
                    If liInd > 0 Then lsSQLQuery &= "," & vbCrLf
                    If lrQueryNode.Alias Is Nothing Then
                        lsSQLQuery &= lrQueryNode.RDSTable.DatabaseName  'FBMModelObject.getCorrespondingRDSTable.Name
                    Else
                        'FBMModelObject.getCorrespondingRDSTable.Name
                        lsSQLQuery &= lrQueryNode.RDSTable.DatabaseName & " " & lrQueryNode.Name & Viev.NullVal(lrQueryNode.Alias, "")
                    End If

                    liInd += 1
                Next

                'Derived EntityTypes
                Dim lrDerivationProcessor As FEQL.Processor = Nothing
                Dim lasAlias As New List(Of String)
#Region "Derived Entity Types"
                For Each lrQueryNode In larFromNodes.FindAll(Function(x) x.FBMModelObject.IsDerived)
                    lrDerivationProcessor = New FEQL.Processor(prApplication.WorkingModel)

                    If liInd > 0 Then lsSQLQuery &= "," & vbCrLf

                    If Not lasAlias.Contains(lrQueryNode.Id & NullVal(lrQueryNode.Alias, "")) Then

                        lsSQLQuery &= lrDerivationProcessor.processDerivationText((lrQueryNode.FBMModelObject.DerivationText).Replace(vbCr, " "),
                                                                                   lrQueryNode.FBMModelObject)

                        lasAlias.Add(lrQueryNode.Id & NullVal(lrQueryNode.Alias, ""))
                    End If
                    liInd += 1
                Next


#End Region


                liInd = 1
                Dim larQuerEdgeWithFBMFactType = Me.QueryEdges.FindAll(Function(x) x.FBMFactType IsNot Nothing Or (x.IsRecursive And x.IsCircular))
                Dim larRDSTableQueryEdge = larQuerEdgeWithFBMFactType.FindAll(Function(x) (x.FBMFactType.isRDSTable And
                                                                                           Not x.FBMFactType.IsDerived And
                                                                                           Not x.FBMFactType.IsUnaryFactType And
                                                                                           Not x.IsPartialFactTypeMatch) And
                                                                                           Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery) Or
                                                                                           (x.IsRecursive And (
                                                                                           x.IsCircular Or
                                                                                           x.HasTargetNodeCircularToTable())))

                Dim larRecursiveTableQueryEdge = From QueryEdge In Me.QueryEdges
                                                 Where QueryEdge.IsRecursive And QueryEdge.HasTargetNodeCircularToTable()
                                                 Select QueryEdge

                For Each lrQueryEdge In larRecursiveTableQueryEdge
                    larRDSTableQueryEdge.AddUnique(lrQueryEdge)
                Next


                'RDS Tables
                For Each lrQueryEdge In larRDSTableQueryEdge

                    If Not lrQueryEdge.IsRecursive Then
                        If Me.Nodes.FindAll(Function(x) x.Name = lrQueryEdge.FBMFactType.Id).Count = 0 Then
                            If Not larFromNodes.Count = 0 Then lsSQLQuery &= vbCrLf & ","
                            lsSQLQuery &= lrQueryEdge.FBMFactType.Id
                            If lrQueryEdge.Alias IsNot Nothing Then
                                lsSQLQuery &= " " & lrQueryEdge.FBMFactType.Id & Viev.NullVal(lrQueryEdge.Alias, "")
                            End If
                        End If

                    Else
#Region "Recursive"
                        If lrQueryEdge.IsCircular Then
#Region "Circular"
                            Dim lrPGSRelationTable = lrQueryEdge.TargetNode.RDSTable

                            Dim lasColumnName = From Column In lrPGSRelationTable.Column
                                                Order By Column.OrdinalPosition
                                                Select Column.Name

                            Dim larJoinColumn As List(Of RDS.Column)

                            larJoinColumn = (From Column In lrPGSRelationTable.Column.FindAll(Function(x) x.Relation.Count > 0)
                                             Where Column.Relation.Find(Function(x) x.OriginTable Is lrPGSRelationTable).DestinationTable.Name = lrQueryEdge.BaseNode.Name
                                             Select Column
                                             Order By Column.OrdinalPosition).ToList

                            If Not larJoinColumn(0).Role Is lrQueryEdge.FBMFactTypeReading.PredicatePart(1).Role Then
                                larJoinColumn.Reverse()
                            End If

                            Dim lsColumnNames = Strings.Join(lasColumnName.ToArray, ",")
                            lsSQLQuery &= vbCrLf & ", (WITH RECURSIVE nodes(" & lsColumnNames & ",depth) As ("
                            lsSQLQuery &= vbCrLf & " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",0"
                            lsSQLQuery &= vbCrLf & " FROM " & lrPGSRelationTable.DatabaseName
                            If lrQueryEdge.BaseNode.HasIdentifier Then
                                lsSQLQuery &= " WHERE "

                                Dim larPKColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns.OrderBy(Function(x) x.OrdinalPosition)
                                liInd = 0
                                For Each lrPKColumn In larPKColumn
                                    If liInd > 0 Then lsSQLQuery &= ","
                                    Dim lrPGSRelationColumn = From Column In lrPGSRelationTable.Column
                                                              From Relation In Column.Relation
                                                              Where Column.Relation.Count > 0
                                                              Where Relation.DestinationColumns(0).Id = lrPKColumn.Id
                                                              Select Column
                                                              Order By Column.OrdinalPosition

                                    lsSQLQuery &= lrPGSRelationTable.DatabaseName & "." & lrPGSRelationColumn.First.Name & " = " & lrQueryEdge.BaseNode.IdentifierList(liInd)
                                    liInd += 1
                                Next
                            End If
                            lsSQLQuery &= vbCrLf & " UNION"
                            lsSQLQuery &= " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",depth+1"
                            lsSQLQuery &= vbCrLf & " FROM nodes, " & lrPGSRelationTable.DatabaseName
                            lsSQLQuery &= vbCrLf & " WHERE nodes." & larJoinColumn(1).Name & " = " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(0).Name
                            lsSQLQuery &= vbCrLf & " LIMIT 100"
                            lsSQLQuery &= vbCrLf & ")"
                            lsSQLQuery &= vbCrLf & " SELECT " & Strings.Join(lasColumnName.ToArray, ",") & ",depth"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE depth <= (SELECT MIN(depth) FROM nodes N2 "
                            If lrQueryEdge.BaseNode.HasIdentifier Then
                                lsSQLQuery &= " WHERE " & "N2." & larJoinColumn(1).Name & " = " & lrQueryEdge.BaseNode.IdentifierList(0)
                            End If
                            lsSQLQuery &= "))" & lrPGSRelationTable.Name & lrQueryEdge.TargetNode.Alias
                            'MAX - 1 because is circular and could go on forever, and so LIMIT may stop half way through the last circular set of references.
#End Region
                        ElseIf lrQueryEdge.IsShortestPath Then
#Region "Shortest Path"
                            Dim lrPGSRelationTable = lrQueryEdge.TargetNode.RDSTable

                            Dim lasColumnName = From Column In lrPGSRelationTable.Column
                                                Order By Column.OrdinalPosition
                                                Select Column.Name

                            Dim larJoinColumn As List(Of RDS.Column)

                            larJoinColumn = (From Column In lrPGSRelationTable.Column.FindAll(Function(x) x.Relation.Count > 0)
                                             Where Column.Relation.Find(Function(x) x.OriginTable Is lrPGSRelationTable).DestinationTable.Name = lrQueryEdge.BaseNode.Name
                                             Select Column
                                             Order By Column.OrdinalPosition).ToList

                            If Not larJoinColumn(0).Role Is lrQueryEdge.FBMFactTypeReading.PredicatePart(1).Role Then
                                larJoinColumn.Reverse()
                            End If

                            Dim lsColumnNames = Strings.Join(lasColumnName.ToArray, ",")

                            lsSQLQuery &= vbCrLf & ", (WITH RECURSIVE nodes(" & lsColumnNames & ",level,path) As ("
                            lsSQLQuery &= vbCrLf & " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",1 as level,(" & larJoinColumn(0).Name & " || '->' || " & larJoinColumn(1).Name & ") AS path"
                            lsSQLQuery &= vbCrLf & " FROM " & lrPGSRelationTable.DatabaseName
                            If lrQueryEdge.BaseNode.HasIdentifier Then
                                lsSQLQuery &= " WHERE "

                                Dim larPKColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns.OrderBy(Function(x) x.OrdinalPosition)
                                liInd = 0
                                For Each lrPKColumn In larPKColumn
                                    If liInd > 0 Then lsSQLQuery &= ","
                                    Dim lrPGSRelationColumn = From Column In lrPGSRelationTable.Column
                                                              From Relation In Column.Relation
                                                              Where Column.Relation.Count > 0
                                                              Where Relation.DestinationColumns(0).Id = lrPKColumn.Id
                                                              Select Column
                                                              Order By Column.OrdinalPosition

                                    lsSQLQuery &= lrPGSRelationTable.DatabaseName & "." & lrPGSRelationColumn.First.Name & " = " & lrQueryEdge.BaseNode.IdentifierList(liInd)
                                    liInd += 1
                                Next
                            End If
                            lsSQLQuery &= vbCrLf & " UNION ALL"
                            lsSQLQuery &= " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",level+1, (nodes.path || '->' || " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(1).Name & ") AS path"
                            lsSQLQuery &= vbCrLf & " FROM nodes JOIN " & lrPGSRelationTable.DatabaseName
                            lsSQLQuery &= vbCrLf & " ON " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(0).Name & " = nodes." & larJoinColumn(1).Name
                            lsSQLQuery &= vbCrLf & " WHERE (" & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(1).Name & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0) & " OR level<10)"
                            lsSQLQuery &= vbCrLf & " AND NOT nodes.path LIKE '%->' || " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & ")"
                            lsSQLQuery &= vbCrLf & " SELECT DISTINCT nodes.*" ' & Strings.Join(lasColumnName.ToArray, ",") & ",depth"
                            lsSQLQuery &= vbCrLf & " FROM nodes,"
                            lsSQLQuery &= vbCrLf & " (SELECT path, level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).Name & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " AND level = (SELECT level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).Name & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " )"
                            lsSQLQuery &= vbCrLf & " ) pth"
                            lsSQLQuery &= vbCrLf & " WHERE nodes.level <= (SELECT level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).Name & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " )"
                            lsSQLQuery &= vbCrLf & " AND pth.path LIKE '%' || nodes.path || '%'"
                            lsSQLQuery &= vbCrLf & " ORDER BY level) " & lrPGSRelationTable.Name & lrQueryEdge.TargetNode.Alias

                            lrQueryEdge.GetNextQueryEdge.TargetNode.IsExcludedConditional = True

#End Region
                        Else
                            Dim lrRDSTable As RDS.Table
                            If lrQueryEdge.FBMFactType.isRDSTable Then
                                lrRDSTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable
                            Else
                                lrRDSTable = lrQueryEdge.TargetNode.RDSTable
                            End If

                            Dim larLeftColumn, larRightColumn As New List(Of RDS.Column)

                            If lrRDSTable.isPGSRelation Then

                                larLeftColumn = (From PredicatePart In lrQueryEdge.FBMFactTypeReading.PredicatePart
                                                 From Column In lrRDSTable.Column
                                                 Where PredicatePart.SequenceNr = 1
                                                 Where PredicatePart.Role.Id = Column.Role.Id
                                                 Select Column).ToList

                                larRightColumn = (From PredicatePart In lrQueryEdge.FBMFactTypeReading.PredicatePart
                                                  From Column In lrRDSTable.Column
                                                  Where PredicatePart.SequenceNr = 2
                                                  Where PredicatePart.Role.Id = Column.Role.Id
                                                  Select Column).ToList
                            Else
                                Dim larJoinColumn = (From Column In lrRDSTable.Column.FindAll(Function(x) x.Relation.Count > 0)
                                                     Where Column.Relation.Find(Function(x) x.OriginTable Is lrRDSTable).DestinationTable.Name = lrQueryEdge.BaseNode.Name
                                                     Select Column
                                                     Order By Column.OrdinalPosition).ToList
                                larLeftColumn.Add(larJoinColumn(0))
                                larRightColumn.Add(larJoinColumn(1))
                            End If

                            Dim lasColumnName = From Column In lrRDSTable.Column
                                                Order By Column.OrdinalPosition
                                                Select Column.Name
                            Dim lsColumnNames = Strings.Join(lasColumnName.ToArray, ",")
                            lsSQLQuery &= vbCrLf & ", (With RECURSIVE nodes(" & lsColumnNames & ",depth) As ("
                            lsSQLQuery &= vbCrLf & " Select " & lrRDSTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrRDSTable.DatabaseName & ".") & ",0"
                            lsSQLQuery &= vbCrLf & " FROM " & lrRDSTable.DatabaseName
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                lsSQLQuery &= vbCrLf & "," & "" & lrQueryEdge.TargetNode.RDSTable.DatabaseName
                            End If
                            If lrQueryEdge.BaseNode.IdentifierList.Count > 0 Then
                                Dim lrTargetTable As RDS.Table = larLeftColumn(0).Relation.Find(Function(x) x.OriginTable.Name = lrRDSTable.Name).DestinationTable
                                lsSQLQuery &= "," & lrTargetTable.Name & vbCrLf & " WHERE "
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= lrTargetTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = '"
                                    lsSQLQuery &= lrQueryEdge.BaseNode.IdentifierList(liInd) & "'" & vbCrLf
                                    If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then lsSQLQuery &= "AND "
                                    liInd += 1
                                Next
                                lsSQLQuery &= vbCrLf & " AND " & lrRDSTable.DatabaseName & "." & larLeftColumn(0).Name & " = " & lrTargetTable.DatabaseName & "." & lrTargetTable.getPrimaryKeyColumns(0).Name
                            End If
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                Dim lrTargetTable As RDS.Table = lrQueryEdge.TargetNode.RDSTable
                                lsSQLQuery &= vbCrLf & " WHERE " & lrRDSTable.DatabaseName & "." & larRightColumn(0).Name & " = " & lrTargetTable.DatabaseName & "." & lrTargetTable.getPrimaryKeyColumns(0).Name
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= vbCrLf & " AND "
                                    lsSQLQuery &= lrTargetTable.DatabaseName & "." & lrColumn.Name & " = '"
                                    lsSQLQuery &= lrQueryEdge.TargetNode.IdentifierList(liInd) & "'" & vbCrLf
                                    liInd += 1
                                Next
                                lrQueryEdge.TargetNode.IsExcludedConditional = True
                            End If
                            lsSQLQuery &= vbCrLf & " UNION "
                            lsSQLQuery &= vbCrLf & " SELECT " & lrRDSTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrRDSTable.DatabaseName & ".") & ",depth + 1"
                            lsSQLQuery &= vbCrLf & " FROM nodes, " & lrRDSTable.DatabaseName
                            lsSQLQuery &= vbCrLf & " WHERE "
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                lsSQLQuery &= "Nodes." & larLeftColumn(0).Name & " = " & lrRDSTable.DatabaseName & "." & larRightColumn(0).Name
                            Else
                                lsSQLQuery &= "Nodes." & larRightColumn(0).Name & " = " & lrRDSTable.DatabaseName & "." & larLeftColumn(0).Name
                            End If

                            If My.Settings.FactEngineDefaultQueryResultLimit > 0 Then
                                lsSQLQuery &= vbCrLf & " LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                            End If
                            lsSQLQuery &= vbCrLf & " )"
                            lsSQLQuery &= vbCrLf & " SELECT " & Strings.Join(lasColumnName.ToArray, ",") & ",depth"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            If lrQueryEdge.RecursiveNumber1 IsNot Nothing And lrQueryEdge.RecursiveNumber2 IsNot Nothing Then
                                lsSQLQuery &= vbCrLf & " WHERE depth BETWEEN " & lrQueryEdge.RecursiveNumber1 & " AND " & lrQueryEdge.RecursiveNumber2
                            ElseIf lrQueryEdge.RecursiveNumber2 Is Nothing Then
                                lsSQLQuery &= vbCrLf & " WHERE depth >= " & lrQueryEdge.RecursiveNumber1
                            End If
                            lsSQLQuery &= vbCrLf & ")"
                            If lrQueryEdge.FBMFactType.isRDSTable Then
                                lsSQLQuery &= " AS " & lrQueryEdge.FBMFactType.Id
                            ElseIf lrQueryEdge.TargetNode.RDSTable.isCircularToTable(lrQueryEdge.BaseNode.RDSTable) Then
                                lsSQLQuery &= " AS " & lrRDSTable.Name & lrQueryEdge.TargetNode.Alias
                            Else
                                lsSQLQuery &= " AS " & lrQueryEdge.FBMFactType.Id
                            End If
                        End If
                    End If
#End Region
                    'If liInd < larRDSTableQueryEdge.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                'Derived FactTypes
                lrDerivationProcessor = New FEQL.Processor(prApplication.WorkingModel)

                Dim larDerivedFactType = (From QueryEdge In Me.QueryEdges
                                          Where QueryEdge.FBMFactType.IsDerived
                                          Select New With {QueryEdge.FBMFactType, QueryEdge.Alias}).Distinct

                lasAlias = New List(Of String)
                For Each lrQueryEdge In larDerivedFactType 'Me.QueryEdges.FindAll(Function(x) x.FBMFactType.IsDerived)
                    If Not lasAlias.Contains(lrQueryEdge.FBMFactType.Id & NullVal(lrQueryEdge.Alias, "")) Then

                        Dim larQueryEdge = (From QueryEdge In Me.QueryEdges
                                            Where QueryEdge.FBMFactType Is lrQueryEdge.FBMFactType
                                            Where NullVal(QueryEdge.Alias, "") = NullVal(lrQueryEdge.Alias, "")
                                            Select QueryEdge).ToArray

                        lsSQLQuery &= "," & vbCrLf
                        lsSQLQuery &= lrDerivationProcessor.processDerivationText((lrQueryEdge.FBMFactType.DerivationText).Replace(vbCr, " ").Replace(vbCrLf, " "),
                                                                                   lrQueryEdge.FBMFactType,
                                                                                   larQueryEdge)

                        lasAlias.Add(lrQueryEdge.FBMFactType.Id & NullVal(lrQueryEdge.Alias, ""))
                    End If
                Next

                'PartialFactTypeMatch
                Dim lrPartialMatchFactType As FBM.FactType = Nothing
                For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery) And Not x.FBMFactType.IsDerived And Not x.FBMFactType.IsUnaryFactType)
                    If lrQueryEdge.IsPartialFactTypeMatch Then

                        Dim larExistingNode = From Node In larFromNodes
                                              Where Node.Name = lrQueryEdge.FBMFactType.Id
                                              Where Node.Alias = lrQueryEdge.Alias
                                              Select Node

                        If (lrQueryEdge.FBMFactType IsNot lrPartialMatchFactType) And larExistingNode.Count = 0 Then
                            If larFromNodes.Count > 0 Then lsSQLQuery &= vbCrLf & ","
                            lsSQLQuery &= lrQueryEdge.FBMFactType.Id
                            If lrQueryEdge.Alias IsNot Nothing Then
                                lsSQLQuery &= " " & lrQueryEdge.FBMFactType.Id & Viev.NullVal(lrQueryEdge.Alias, "")
                            End If
                        End If
                        lrPartialMatchFactType = lrQueryEdge.FBMFactType
                    Else
                        lrPartialMatchFactType = Nothing
                    End If
                Next
#End Region
                'WHERE
                Dim larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                             Where QueryEdge.TargetNode IsNot Nothing
                                             Select QueryEdge

                'WhereEdges are Where joins, rather than ConditionalQueryEdges which test for values by identifiers.
                Dim larWhereEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                                                                       x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType) Or
                                                                                       x.IsRDSTable Or x.IsDerived)

                Dim larEdgesWithDerivedFactType = (From QueryEdge In Me.QueryEdges
                                                   Where QueryEdge.FBMFactType.IsDerived
                                                   Select QueryEdge).ToList

                For Each lrQueryEdge In larEdgesWithDerivedFactType
                    Call larWhereEdges.AddUnique(lrQueryEdge)
                Next

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

                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None))
                '20210826-VM-Removed
                'And (Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                'BooleanPredicate edges. E.g. Protein is enzyme
                Dim larRange = From QueryEdge In Me.QueryEdges
                               Where QueryEdge.TargetNode Is Nothing
                '20220813-VM-Was
                'Me.QueryEdges.FindAll(Function(x) x.TargetNode Is Nothing And Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType)))

                larConditionalQueryEdges.AddRange(larRange.ToList)

                'ShortestPath conditionals are excluded.
                '  E.g. For '(Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) '
                '  the second QueryEdge conditional is taken care of in the FROM clause processing for the recursive query.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNodeIsExcludedConditional)

                If Not abIsStraightDerivationClause Then
                    larConditionalQueryEdges.RemoveAll(Function(x) x.FBMFactType.IsDerived)
                End If

                'Recursive NodePropertyIdentification conditionals are excluded.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNodeIsExcludedConditional)

                larRange = From QueryEdge In Me.QueryEdges
                           Where QueryEdge.TargetNode IsNot Nothing
                           Where QueryEdge.IsRDSTable
                           Where QueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType)
                           Where QueryEdge.IdentifierList.Count = 0
                           Where QueryEdge.TargetNode.IdentifierList.Count = 0
                           Select QueryEdge

                larWhereEdges.RemoveAll(Function(x) larRange.ToList.Contains(x))

                If larWhereEdges.Count = 0 And larConditionalQueryEdges.Count = 0 And (Not Me.HeadNode.HasIdentifier) Then
                    If NullVal(My.Settings.FactEngineDefaultQueryResultLimit, 0) > 0 Then
                        lsSQLQuery &= vbCrLf & "LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                    End If
                    Return lsSQLQuery
                End If

                lsSQLQuery &= vbCrLf & "WHERE "

#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = Nothing
                Dim lbHasWhereClause As Boolean = False

#Region "WhereJoins"
                For Each lrQueryEdge In larWhereEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))

                    If lbAddedAND Or liInd > 1 Then
                        lsSQLQuery &= "AND "
                        lbAddedAND = True
                    Else
                        lbAddedAND = False
                    End If

                    Dim lrOriginTable As RDS.Table

                    If lrQueryEdge.FBMFactType.IsUnaryFactType Then
                        'Must be derived, because normally would have no join for a Edge that has a FactType that is a UnaryFactType, it would be a conditional.
                        lrOriginTable = lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.getCorrespondingRDSTable

                        Dim liInd2 = 0
                        For Each lrColumn In lrOriginTable.getPrimaryKeyColumns
                            If liInd2 > 0 Then lsSQLQuery &= "AND "
                            lsSQLQuery &= lrOriginTable.DatabaseName & "." & lrColumn.Name & " = " & lrQueryEdge.FBMFactType.Id & "." & lrColumn.Name & vbCrLf
                            liInd2 += 1
                        Next

                    ElseIf lrQueryEdge.IsPartialFactTypeMatch Then 'And lrQueryEdge.TargetNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
#Region "Partial FactType match"
                        Dim liInd2 = 0
                        Dim lrNaryTable As RDS.Table = lrQueryEdge.FBMFactType.getCorrespondingRDSTable
                        If lrQueryEdge.BaseNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
                            For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                                If Not lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).IsDerivationParameter Then
                                    If liInd2 > 0 Then lsSQLQuery &= Boston.returnIfTrue(lbAddedAND, "", " AND ")
                                    lsSQLQuery &= lrNaryTable.DatabaseName & "." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                    lsSQLQuery &= "=" & lrColumn.Table.DatabaseName & "." & lrColumn.Name & vbCrLf
                                    liInd2 += 1
                                    lbAddedAND = False
                                End If
                            Next
                        End If
                        If lrQueryEdge.TargetNode.FBMModelObject.GetType <> GetType(FBM.ValueType) Then
                            If Not lbAddedAND Then lsSQLQuery &= Boston.returnIfTrue(lbAddedAND, "", " AND ")
                            liInd2 = 0
                            For Each lrColumn In lrQueryEdge.TargetNode.RDSTable.getPrimaryKeyColumns
                                If liInd2 > 0 Then lsSQLQuery &= " AND "
                                lsSQLQuery &= lrNaryTable.DatabaseName & "." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                lsSQLQuery &= "=" & lrColumn.Table.Name & "." & lrColumn.Name & vbCrLf
                                liInd2 += 1
                            Next
                        End If
#End Region
                    ElseIf lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatIdentityCompatitor Then
                        'E.g. Of the type "Person 1 Is Not Person 2" or "Person 1 Is Person 2"
#Region "AndThatIdentityComparitor. 'E.g. Of the type 'Person 1 Is Not Person 2' or 'Person 1 Is Person 2'"


                        lsSQLQuery &= "("
                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name
                            If lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.ISClause Then
                                lsSQLQuery &= " = "
                            Else
                                lsSQLQuery &= " <> "
                            End If
                            lsSQLQuery &= lrQueryEdge.TargetNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                        Next
                        lsSQLQuery &= ")"
#End Region
                    ElseIf lrQueryEdge.FBMFactType.IsDerived Then

                        lrOriginTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable(Nothing, True)

                        If lrOriginTable Is Nothing Then
                            lrOriginTable = New RDS.Table(Me.Model.RDS, lrQueryEdge.FBMFactType.Id, lrQueryEdge.FBMFactType)
                        End If
                        liInd = 0
                        For Each lrRole In lrQueryEdge.FBMFactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.GetType <> GetType(FBM.ValueType))
                            If liInd > 0 Then lsSQLQuery &= "AND "
                            Dim lrDestinationTable As RDS.Table = lrRole.JoinedORMObject.getCorrespondingRDSTable

                            Dim liInd2 As Integer = 0
                            For Each lrColumn In lrDestinationTable.getPrimaryKeyColumns
                                If liInd2 > 0 Then lsSQLQuery &= "AND "
                                Dim lrOriginColumn As RDS.Column = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                                If lrOriginColumn Is Nothing Then
                                    lsSQLQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = "
                                Else
                                    lsSQLQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrOriginColumn.Name & " = "
                                End If
                                lsSQLQuery &= lrDestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & vbCrLf
                                liInd2 += 1
                            Next
                            liInd += 1
                        Next

                    ElseIf (lrQueryEdge.FBMFactType.isRDSTable And lrQueryEdge.FBMFactType.Arity = 2) Then

                        'RDSTable
#Region "PGSNodeTable/RDSTable"
                        lrOriginTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                        Dim larRelation = lrOriginTable.getRelations

                        larRelation = larRelation.FindAll(Function(x) (x.DestinationTable.Name = lrQueryEdge.BaseNode.RelativeFBMModelObject.Id) Or
                                                                       (x.DestinationTable.Name = lrQueryEdge.TargetNode.Name)).OrderBy(Function(x) x.OriginColumns(0).OrdinalPosition).ToList

                        Dim liTempInd = 0
                        Dim liRelationCounter = 1

                        'Order the relations by the QueryEdge.FactTypeReading
                        '  This way 'Lecturer likes Lecturer' is different from 'Lecturer is liked by Lecturer'

                        Dim lrColumn = lrOriginTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactTypeReading.PredicatePart(0).Role)
                        If Not larRelation(0).OriginColumns.Contains(lrColumn) Then
                            larRelation.Reverse()
                        End If

                        For Each lrRelation In larRelation
                            Dim liColumnCounter = 0
                            For Each lrColumn In lrRelation.DestinationColumns
                                If liTempInd > 0 Then lsSQLQuery &= "AND "

                                Select Case liRelationCounter
                                    Case Is = 1
                                        Select Case lrQueryEdge.BaseNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsSQLQuery &= lrRelation.OriginTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= lrRelation.DestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrTargetColumn.Name & vbCrLf
                                        End Select
                                    Case Else
                                        Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsSQLQuery &= lrRelation.OriginTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= lrRelation.DestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrTargetColumn.Name & vbCrLf
                                        End Select
                                End Select

                                'lsSQLQuery &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = "
                                'lsSQLQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name & vbCrLf 'lrOriginTable.getColumnByOrdingalPosition(1).Name & vbCrLf
                                liTempInd += 1
                                liColumnCounter += 1
                            Next
                            liRelationCounter += 1
                        Next

                        'For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                        '    If liTempInd > 0 Then lsSQLQuery &= "AND "
                        '    Dim lrTargetColumn = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                        '    lsSQLQuery &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = "
                        '    lsSQLQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name & vbCrLf 'lrOriginTable.getColumnByOrdingalPosition(1).Name & vbCrLf
                        '    liTempInd += 1
                        'Next

                        'Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                        '    Case = pcenumConceptType.ValueType
                        '        'Nothing to do here
                        '    Case Else
                        '        liTempInd = 0
                        '        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                        '            Dim lrTargetColumn = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                        '            lsSQLQuery &= vbCrLf & "AND " & lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name & " = "
                        '            lsSQLQuery &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name 'lrOriginTable.getColumnByOrdingalPosition(2).Name
                        '        Next
                        'End Select
#End Region
                    ElseIf lrQueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count Then
#Region "DerivationType = COUNT"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        lrBaseNode = lrQueryEdge.BaseNode
                        lrTargetNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge)

                        lsSQLQuery &= lrBaseNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name & " = " & lrTargetNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name
#End Region
                    Else
#Region "Other/Else"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        If lrQueryEdge.IsReciprocal Then
                            lrBaseNode = lrQueryEdge.TargetNode
                            lrTargetNode = lrQueryEdge.BaseNode
                        ElseIf lrQueryEdge.IsPartialFactTypeMatch Then
                            lrBaseNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge, False)
                            lrTargetNode = lrQueryEdge.TargetNode
                        Else
                            lrBaseNode = lrQueryEdge.BaseNode
                            lrTargetNode = lrQueryEdge.TargetNode
                        End If

                        lrOriginTable = lrBaseNode.RDSTable
                        Dim larModelObject = New List(Of FBM.ModelObject)
                        larModelObject.Add(lrBaseNode.FBMModelObject)
                        larModelObject.Add(lrTargetNode.FBMModelObject)
                        Dim lrRelation As RDS.Relation
                        lrRelation = lrOriginTable.getRelationByFBMModelObjects(larModelObject, lrQueryEdge.FBMFactType, lrQueryEdge)

                        Dim liInd2 = 1
                        If lrRelation.OriginTable Is lrOriginTable Then
                            Dim larOriginColumn As New List(Of RDS.Column)
                            Dim larTargetColumn As New List(Of RDS.Column)
                            'was  larTargetColumn = lrQueryEdge.TargetNode.RDSTable.getPrimaryKeyColumns ' FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns

                            For Each lrColumn In lrRelation.OriginColumns
                                larOriginColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next

                            For Each lrColumn In lrRelation.DestinationColumns
                                larTargetColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next

                            For Each lrColumn In larOriginColumn
                                'was
                                'lsSQLQuery &= lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                                'lsSQLQuery &= " = " & lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                                lsSQLQuery &= lrColumn.Table.DatabaseName & Viev.NullVal(lrBaseNode.Alias, "") & "." & lrColumn.Name
                                lsSQLQuery &= " = " & larTargetColumn(liInd2 - 1).Table.DatabaseName & Viev.NullVal(lrTargetNode.Alias, "") & "." & larTargetColumn(liInd2 - 1).Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                Dim lrOriginColumn = lrRelation.OriginColumns.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                                lsSQLQuery &= lrQueryEdge.TargetNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrOriginColumn.Name
                                lsSQLQuery &= " = " & lrQueryEdge.BaseNode.RDSTable.DatabaseName & "." & lrColumn.Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        End If
#End Region
                    End If

                    'CodeSafe Remove wayward ANDs
                    If Trim(lsSQLQuery).EndsWith("AND") Then
                        lsSQLQuery = Trim(lsSQLQuery).Substring(0, lsSQLQuery.Length - 4)
                    End If

                    If Not lsSQLQuery.EndsWith(vbCrLf) Then lsSQLQuery &= vbCrLf

                    lbAddedAND = False
                    lbIntialWhere = Nothing

                    liInd += 1
                    lbHasWhereClause = True
                Next

#End Region
                Dim lbFirstQueryEdgeIsRecursive As Boolean = False
                If Me.QueryEdges.Count > 0 Then
                    lbFirstQueryEdgeIsRecursive = Me.QueryEdges(0).IsRecursive
                End If

                If (Not lbAddedAND And lbIntialWhere Is Nothing And
                    (larConditionalQueryEdges.Count > 0 And larWhereEdges.Count > 0)) Or
                    (Me.HeadNode.HasIdentifier And Not lbFirstQueryEdgeIsRecursive And larWhereEdges.Count > 0) Then
                    lsSQLQuery &= "AND " '20211008-VM-Was "AND " and Nothing below.
                    lbIntialWhere = Nothing
                End If

#Region "WhereConditionals"
                If Me.HeadNode.HasIdentifier And Not lbFirstQueryEdgeIsRecursive Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.DatabaseName & Viev.NullVal(Me.HeadNode.Alias, "") & "." & lrColumn.Name & " = '" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then
                            lsSQLQuery &= "AND "
                        End If
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
                End If

                For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))
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

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsSQLQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select
                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                        Dim loDateTime As DateTime = Nothing
                                        If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                            Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                        End If
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select

                            Else
                                lrTable = lrQueryEdge.BaseNode.RDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole IsNot Nothing
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsSQLQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                End Select
                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                        Dim loDateTime As DateTime = Nothing
                                        If Not DateTime.TryParse(lsUserDateTime, loDateTime) Then
                                            Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                        End If
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select

                            End If

                            lbIntialWhere = "AND "
                        Case Else

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable.DatabaseName & "."

                                    Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsSQLQuery &= lrTargetColumn.Name & " = True"

                                Case Else



                                    If lrQueryEdge.TargetNode.MathFunction <> pcenumMathFunction.None Then

                                        If lrQueryEdge.FBMFactType.IsDerived Then

                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrQueryEdge.FBMFactType.Id &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              CType(lrQueryEdge.TargetNode.PreboundText & lrQueryEdge.TargetNode.Name, String).Replace("-", "")

                                            lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf

                                        Else


                                            'Math function
                                            Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrTargetTable.DatabaseName &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              lrTargetColumn.Name

                                            lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf

                                        End If
                                        lbIntialWhere = "AND "
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
                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name & " = "
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0))
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                End Select
                                            Else
                                                lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.Name
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDate,
                                                              pcenumORMDataType.TemporalDateAndTime
                                                        lsSQLQuery &= prApplication.WorkingModel.DatabaseConnection.dateToTextOperator
                                                End Select
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0), True)
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
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
                                            For Each lsIdentifier In lrQueryEdge.IdentifierList
                                                If liInd > 0 Then
                                                    lsSQLQuery &= "AND "
                                                    lbIntialWhere = ""
                                                End If
                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.DatabaseName & lsAlias & "." & larIndexColumns(liInd).Name & lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "'" & vbCrLf
                                                liInd += 1
                                            Next
                                        End If

                                        lbIntialWhere = "AND "
                                    End If
                            End Select
                    End Select
                    lbHasWhereClause = True
                Next
#End Region
#End Region

#Region "Subqueries"
                'CodeSafe Remove wayward ANDs
                If Trim(lsSQLQuery).EndsWith("AND") Then
                    lsSQLQuery = Trim(lsSQLQuery).Substring(0, lsSQLQuery.Length - 4)
                    lbAddedAND = False
                End If
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

                    If lbIntialWhere IsNot Nothing Then lbHasWhereClause = True
                    lsSQLQuery &= Boston.returnIfTrue(lbAddedAND Or Not lbHasWhereClause, "", " AND ") & lrSubQueryGraph.generateSQL(arWhichSelectStatement,
                                                                                                                                       True,,, True)
                    lsSQLQuery &= ")"
                Next
                '=====================================================================================
#End Region

                '=====================================
                'Group By clause
                If lbRequiresGroupByClause Then
                    lsSQLQuery &= "GROUP BY " & lsSelectClause
                End If


                If Not abIsStraightDerivationClause And Not abIsSubQuery And NullVal(My.Settings.FactEngineDefaultQueryResultLimit, 0) > 0 Then
                    lsSQLQuery &= vbCrLf & "LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                End If

                Return lsSQLQuery
            Catch ex As Exception
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & lsSQLQuery)
            End Try


        End Function

        Public Function getProjectQueryEdges() As List(Of FactEngine.QueryEdge)

            Dim larQueryEdge As List(Of FactEngine.QueryEdge) = Me.QueryEdges.FindAll(Function(x) x.IsProjectColumn And Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery)).ToList

            'ShortestPath Targets
            Dim larShortestPathQueryEdge = Me.QueryEdges.FindAll(Function(x) x.IsShortestPath)

            For Each lrQueryEdge In larShortestPathQueryEdge
                larQueryEdge.AddUnique(lrQueryEdge.GetNextQueryEdge)
            Next

            '==============================================================================
            'SubQuery edges - Remove
            larQueryEdge.RemoveAll(Function(x) x.IsSubQueryLeader Or x.IsPartOfSubQuery)

            Return larQueryEdge

        End Function

        Public Function getNodeModelElementList(Optional abGetSupertypeModelElements As Boolean = False) As List(Of FBM.ModelObject)

            Dim larModelElement As New List(Of FBM.ModelObject)

            Try
                If Me.HeadNode IsNot Nothing Then
                    larModelElement.AddUnique(Me.HeadNode.FBMModelObject)
                End If
                For Each lrQueryEdge In Me.QueryEdges
                    If lrQueryEdge.BaseNode.FBMModelObject IsNot Nothing Then
                        larModelElement.AddUnique(lrQueryEdge.BaseNode.FBMModelObject)
                    End If
                    If lrQueryEdge.TargetNode.FBMModelObject IsNot Nothing Then
                        larModelElement.AddUnique(lrQueryEdge.TargetNode.FBMModelObject)
                    End If
                Next

                If abGetSupertypeModelElements Then
                    For Each lrModelElement In larModelElement.ToArray
                        larModelElement.AddRange(lrModelElement.getSupertypes)
                    Next
                End If

                Return larModelElement

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function getProjectionColumns(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement,
                                             Optional ByVal abIsStraightDerivationClause As Boolean = False,
                                             Optional ByVal arDerivedModelElement As FBM.ModelObject = Nothing) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)

            Try
#Region "Head Node"
                If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then

                    For Each lrReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN

                        If lrReturnColumn.MODELELEMENTNAME IsNot Nothing Then

                            If lrReturnColumn.COLUMNNAMESTR Is Nothing Then

                                'Check if use forgot to put TableName as in Cinema.CinemaName
                                Try
                                    Dim lrWhichSelectStatemet = arWhichSelectStatement
                                    Dim larTryColumn = From Table In Me.Model.RDS.Table
                                                       From Column In Table.Column
                                                       Where Me.Nodes.Select(Function(x) x.Name).Contains(Table.Name)
                                                       Where Column.Name = lrReturnColumn.MODELELEMENTNAME
                                                       Select Column

                                    If larTryColumn.Count = 1 Then
                                        larColumn.Add(larTryColumn.First)
                                        GoTo MoveForward
                                    End If

                                Catch ex As Exception

                                End Try

                                'Must be * (as in Lecturer.*)
                                Try
                                    Dim lrTable As RDS.Table = Me.Model.RDS.Table.Find(Function(x) x.Name = lrReturnColumn.MODELELEMENTNAME)
                                    For Each lrColumn In lrTable.Column
                                        larColumn.Add(lrColumn.Clone(Nothing, Nothing))
                                    Next
                                Catch ex As Exception
                                    Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & ".*, not found. Check your RETURN clause.")
                                End Try
                            Else
                                Dim larReturnColumn = From Table In Me.Model.RDS.Table
                                                      From Column In Table.Column
                                                      Where Table.Name = lrReturnColumn.MODELELEMENTNAME
                                                      Where Column.Name = lrReturnColumn.COLUMNNAMESTR
                                                      Select Column

                                Try
                                    Dim lrColumn As RDS.Column = larReturnColumn.First.Clone(Nothing, Nothing)
                                    lrColumn.TemporaryAlias = lrReturnColumn.MODELELEMENTSUFFIX
                                    If lrReturnColumn.ASCLAUSE IsNot Nothing Then
                                        lrColumn.AsName = lrReturnColumn.ASCLAUSE.COLUMNNAMESTR
                                    End If

                                    larColumn.Add(lrColumn)
                                Catch ex As Exception
                                    Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & "." & lrReturnColumn.COLUMNNAMESTR & ", not found. Check your RETURN clause.")
                                End Try
                            End If

                        ElseIf lrReturnColumn.KEYWDCOUNTSTAR IsNot Nothing Then
                            'COUNT(*) is added in GenerateSQL
                        Else
                            'Must be * (STAR)
                            For Each lrColumn In Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.Column
                                larColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next
                            For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) x.IsProjectColumn)

                            Next
                        End If
MoveForward:
                    Next
                ElseIf abIsStraightDerivationClause Then
                    Dim lrProjectionColumn As RDS.Column = Nothing

                    Select Case arDerivedModelElement.GetType
                        Case Is = GetType(FBM.FactType)
                            Dim lrFactType As FBM.FactType = CType(arDerivedModelElement, FBM.FactType)
                            If lrFactType.isRDSTable Then
                                'For Each lrColumn In arDerivedFactType.getCorrespondingRDSTable.Column
                                '    lrProjectionColumn = lrColumn.Clone(Nothing, Nothing)
                                '    larColumn.Add(lrProjectionColumn)
                                'Next

                                Dim larSubQueryNodes = (From QueryEdge In Me.QueryEdges
                                                        Where QueryEdge.IsPartOfSubQuery
                                                        Select QueryEdge.BaseNode).Union(
                                                        From QueryEdge In Me.QueryEdges
                                                        Where QueryEdge.IsSubQueryLeader Or QueryEdge.IsPartOfSubQuery
                                                        Where QueryEdge.TargetNode IsNot Nothing
                                                        Select QueryEdge.TargetNode)

                                Dim larSubQueryFBMModelElements = (From Node In larSubQueryNodes
                                                                   Select Node.FBMModelObject).ToList

                                For Each lrRole In lrFactType.RoleGroup

                                    If Not larSubQueryFBMModelElements.Contains(lrRole.JoinedORMObject) Then
                                        Select Case lrRole.JoinedORMObject.GetType
                                            Case Is = GetType(FBM.ValueType)
                                                Dim lrVTColumn As New RDS.Column(arDerivedModelElement.getCorrespondingRDSTable, lrRole.JoinedORMObject.Id, lrRole, lrRole, False)
                                                lrVTColumn = lrVTColumn.Clone(Nothing, Nothing)
                                                larColumn.Add(lrVTColumn)
                                            Case Else
                                                For Each lrColumn In lrRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                                                    lrProjectionColumn = lrColumn.Clone(Nothing, Nothing)
                                                    lrProjectionColumn.AsName = arDerivedModelElement.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                                    larColumn.Add(lrProjectionColumn)
                                                Next
                                        End Select
                                    End If
                                Next
                            Else
                                Dim lrDerivedFactType As FBM.FactType = CType(arDerivedModelElement, FBM.FactType)
                                For Each lrRole In lrDerivedFactType.RoleGroup
                                    Select Case lrRole.JoinedORMObject.GetType
                                        Case Is = GetType(FBM.ValueType)
                                            Dim lrVTColumn As New RDS.Column(arDerivedModelElement.getCorrespondingRDSTable, lrRole.JoinedORMObject.Id, lrRole, lrRole, False)
                                            lrVTColumn = lrVTColumn.Clone(Nothing, Nothing)
                                            larColumn.Add(lrVTColumn)
                                        Case Else
                                            If arDerivedModelElement.isUnaryFactType Then
                                                For Each lrColumn In lrRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                                                    lrProjectionColumn = lrColumn.Clone(Nothing, Nothing)
                                                    lrProjectionColumn.AsName = lrRole.JoinedORMObject.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                                    larColumn.Add(lrProjectionColumn)
                                                Next
                                            Else
                                                For Each lrColumn In lrRole.JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                                                    lrProjectionColumn = lrColumn.Clone(Nothing, Nothing)
                                                    lrProjectionColumn.AsName = arDerivedModelElement.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                                    larColumn.Add(lrProjectionColumn)
                                                Next
                                            End If


                                    End Select
                                Next
                            End If

                        Case Is = GetType(FBM.EntityType)
                            For Each lrColumn In arDerivedModelElement.getCorrespondingRDSTable.Column
                                lrProjectionColumn = lrColumn.Clone(Nothing, Nothing)
                                larColumn.Add(lrProjectionColumn)
                            Next
                    End Select

                Else
                    'Head Column/s
                    Dim larHeadColumn As New List(Of RDS.Column)

                    Select Case Me.HeadNode.FBMModelObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrVTColumn As RDS.Column
                            If Me.QueryEdges(0).IsPartialFactTypeMatch Then
                                lrVTColumn = (From Column In Me.QueryEdges(0).FBMFactType.getCorrespondingRDSTable.Column
                                              Where Column.Role Is Me.QueryEdges(0).FBMPredicatePart.Role
                                              Select Column).First
                            Else
                                lrVTColumn = (From Column In Me.QueryEdges(0).TargetNode.FBMModelObject.getCorrespondingRDSTable.Column
                                              Where Column.Role Is Me.QueryEdges(0).FBMFactType.RoleGroup(0)
                                              Where Column.ActiveRole Is Me.QueryEdges(0).FBMFactType.RoleGroup(1)
                                              Select Column).First
                            End If


                            lrVTColumn = lrVTColumn.Clone(Nothing, Nothing)
                            lrVTColumn.TemporaryAlias = Viev.NullVal(Me.HeadNode.QueryEdgeAlias, "")
                            lrVTColumn.GraphNodeType = Me.HeadNode.Name
                            larHeadColumn.Add(lrVTColumn)

                        Case Else
                            Dim lrTempColumn As RDS.Column = Nothing
                            For Each lrColumn In Me.HeadNode.RelativeFBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition)
                                If Me.HeadNode.FBMModelObject.GetType = GetType(FBM.EntityType) And Me.HeadNode.FBMModelObject.IsDerived Then
                                    lrTempColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, Me.HeadNode.FBMModelObject.Id, Me.HeadNode.FBMModelObject), Nothing)
                                Else
                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                                End If

                                lrTempColumn.TemporaryAlias = Viev.NullVal(Me.HeadNode.Alias, "")
                                lrTempColumn.GraphNodeType = Me.HeadNode.Name
                                lrTempColumn.IsPartOfUniqueIdentifier = True
                                If Me.QueryEdges.Count > 0 Then
                                    lrTempColumn.QueryEdge = Me.QueryEdges(0)
                                End If
                                larHeadColumn.Add(lrTempColumn)
                            Next
                    End Select

                    larColumn.AddRange(larHeadColumn.ToList)
#End Region
                    Dim liRoleInd As Integer
                    'Edge Column/s
                    Dim lrQueryEdge As FactEngine.QueryEdge
                    Dim lrRole As FBM.Role = Nothing
                    For Each lrQueryEdge In Me.getProjectQueryEdges()
                        If (lrQueryEdge.FBMFactType.IsManyTo1BinaryFactType Or lrQueryEdge.FBMFactType.Is1To1BinaryFactType) And lrQueryEdge.BaseNode.RelativeFBMModelObject.Id = lrQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).JoinedORMObject.Id Then
                            '20210724-VM-Was the below, which is wrong.
                            'If lrQueryEdge.FBMFactType.IsBinaryFactType And lrQueryEdge.BaseNode.RelativeFBMModelObject.Id = lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                            liRoleInd = 0 'TargetNode is for other side of ManyToOne BinaryFactType
                            lrRole = lrQueryEdge.FBMFactType.GetOtherRoleOfBinaryFactType(lrQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0).Id)

                        ElseIf lrQueryEdge.IsPartialFactTypeMatch Then
                            lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(lrQueryEdge.FBMFactTypeReading.PredicatePart.IndexOf(lrQueryEdge.FBMPredicatePart) + 1).Role
                            liRoleInd = lrQueryEdge.FBMFactType.RoleGroup.IndexOf(lrRole)
                        Else
                            liRoleInd = 1
                            If lrQueryEdge.FBMFactType.IsManyTo1BinaryFactType Then
                                'TargetNode is Many side of ManyToOne BinaryFactType
                                lrRole = lrQueryEdge.FBMFactType.InternalUniquenessConstraint(0).Role(0)
                            ElseIf lrQueryEdge.FBMFactType.HasTotalRoleConstraint And lrQueryEdge.FBMFactType.Arity = 2 Then
                                If lrQueryEdge.FBMFactTypeReading IsNot Nothing Then
                                    liRoleInd = lrQueryEdge.FBMFactTypeReading.PredicatePart.IndexOf(lrQueryEdge.FBMPredicatePart)
                                    lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(liRoleInd).Role
                                    If lrRole.JoinedORMObject.Id <> lrQueryEdge.TargetNode.Name And liRoleInd = 0 Then
                                        liRoleInd = 1
                                        lrRole = lrQueryEdge.FBMFactType.RoleGroup.Find(Function(x) x IsNot lrRole)
                                    End If
                                Else
                                    lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(1).Role
                                End If
                            ElseIf lrQueryEdge.FBMFactTypeReading IsNot Nothing Then
                                lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(lrQueryEdge.FBMFactTypeReading.PredicatePart.IndexOf(lrQueryEdge.FBMPredicatePart) + 1).Role
                            End If
                        End If

                        'CodeSafe
                        If lrRole Is Nothing Then
                            Throw New Exception("Could not find Role for Query Edge Fact Type: " & lrQueryEdge.FBMFactType.Id)
                        End If

                        Select Case lrRole.JoinedORMObject.ConceptType 'lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                Dim lrColumn As RDS.Column
                                If lrQueryEdge.IsPartialFactTypeMatch Then
                                    lrColumn = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.Role Is lrRole)
                                Else
                                    lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                    If lrQueryEdge.FBMFactType.IsManyTo1BinaryFactType And lrQueryEdge.FBMFactType.IsDerived And Not lrQueryEdge.FBMFactType.IsObjectified Then
                                        lrColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, lrQueryEdge.FBMFactType.Id, lrQueryEdge.FBMFactType), Nothing)
                                    ElseIf lrQueryEdge.BaseNode.FBMModelObject.IsDerived Then
                                        lrColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, lrQueryEdge.BaseNode.FBMModelObject.Id, lrQueryEdge.BaseNode.FBMModelObject), Nothing)
                                    End If
                                End If
                                If lrColumn Is Nothing And lrQueryEdge.FBMFactType.IsLinkFactType Then
                                    lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.LinkFactTypeRole).Clone(Nothing, Nothing)
                                    lrColumn.TemporaryAlias = lrQueryEdge.Alias
                                ElseIf lrColumn Is Nothing And lrQueryEdge.FBMFactType.IsBinaryFactType And lrQueryEdge.FBMFactType.HasTotalRoleConstraint Then
                                    lrColumn = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is lrQueryEdge.FBMFactType.RoleGroup(liRoleInd))
                                    lrColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                Else
                                    lrColumn.TemporaryAlias = lrQueryEdge.BaseNode.Alias
                                End If
                                lrColumn.GraphNodeType = lrQueryEdge.BaseNode.Name
                                lrColumn.QueryEdge = lrQueryEdge

                                larColumn.AddUnique(lrColumn)
                            Case Else
                                Dim larEdgeColumn As List(Of RDS.Column)
                                If lrRole IsNot Nothing Then
                                    larEdgeColumn = lrRole.JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition).ToList
                                Else
                                    larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition).ToList
                                End If
                                Dim lrTempColumn As RDS.Column
                                For Each lrColumn In larEdgeColumn
                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                                    If lrRole IsNot Nothing Then
                                        lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                    ElseIf liRoleInd = 1 Then
                                        lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                    ElseIf lrQueryEdge.IsCircular And lrQueryEdge.IsRecursive Then
                                        lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                    ElseIf lrQueryEdge.TargetNode.RDSTable.isCircularToTable(lrQueryEdge.BaseNode.RDSTable) And lrQueryEdge.IsRecursive Then
                                        lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                    Else
                                        lrTempColumn.TemporaryAlias = lrQueryEdge.Alias
                                    End If
                                    lrTempColumn.GraphNodeType = lrQueryEdge.TargetNode.Name
                                    lrTempColumn.IsPartOfUniqueIdentifier = True
                                    lrTempColumn.QueryEdge = lrQueryEdge
                                    larColumn.Add(lrTempColumn)
                                Next
                        End Select
                        lrRole = Nothing
                    Next

                End If

                Return larColumn
            Catch ex As Exception
                Throw New Exception("Error in QueryGraph.getProjectionColumns:" & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf & ex.StackTrace.ToString)
            End Try

        End Function

        Private Sub setNodeAliases()

            Dim liInd As Integer = 0

            For liInd = 0 To Me.Nodes.Count - 1
                If Me.Nodes.Count - 1 > 0 And (liInd + 1) <= Me.Nodes.Count - 1 Then
                    For liInd2 = liInd + 1 To Me.Nodes.Count - 1 'liInd - 1
                        If Me.Nodes(liInd2).Name = Me.Nodes(liInd).Name Then
                            If Me.Nodes(liInd2).Alias Is Nothing Then 'And
                                'Me.Nodes(liInd2).Alias IsNot Nothing Then
                                Me.Nodes(liInd2).Alias = Me.createUniqueNodeAlias(Me.Nodes(liInd2), 1)  'was (liInd + 1).ToString
                                'If Me.Nodes(liInd2).QueryEdge IsNot Nothing Then
                                '    Me.Nodes(liInd2).QueryEdge.Alias = Me.Nodes(liInd2).Alias
                                'End If
                            End If

                        End If
                    Next
                End If
            Next

        End Sub

        Public Sub checkNodeAliases()

            Dim larBaseNodes As New List(Of FactEngine.QueryNode)
            Dim larQueryNode As New List(Of FactEngine.QueryNode)

            Dim larBaseNode = From QueryEdge In Me.QueryEdges
                              Select QueryEdge.BaseNode

            larBaseNodes.AddRange(larBaseNode.ToList)

            Dim larWhichTargetNodes = From QueryEdge In Me.QueryEdges
                                      Where QueryEdge.WhichClause.KEYWDWHICH IsNot Nothing
                                      Select QueryEdge.TargetNode

            larBaseNodes.AddRange(larWhichTargetNodes.ToList)

            Dim larPropertyNodeTargets = From QueryEdge In Me.QueryEdges
                                         Where QueryEdge.WhichClause.NODEPROPERTYIDENTIFICATION IsNot Nothing
                                         Select QueryEdge.TargetNode

            larBaseNodes.AddRange(larPropertyNodeTargets)

            Dim larAANTargetNodes = From QueryEdge In Me.QueryEdges
                                    Where QueryEdge.WhichClause.KEYWDA IsNot Nothing Or QueryEdge.WhichClause.KEYWDAN IsNot Nothing
                                    Select QueryEdge.TargetNode

            larBaseNodes.AddRange(larAANTargetNodes)

            'Implied BaseNodes
            For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) x.IsPartialFactTypeMatch)
                Dim lrNewQueryNode As New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge, False)
                larBaseNodes.Add(lrNewQueryNode)
            Next


            For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) Not larBaseNodes.Contains(x.TargetNode))

                Select Case lrQueryEdge.WhichClauseType
                    Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement,
                              FactEngine.pcenumWhichClauseType.AndThatPredicateThatModelElement

                        Throw New Exception("Model Element, '" & lrQueryEdge.TargetNode.Name & " " & lrQueryEdge.TargetNode.Alias & "', is not referenced in the query.")
                    Case Is = FactEngine.pcenumWhichClauseType.AndThatModelElementPredicateModelElement
                        If lrQueryEdge.WhichClause.KEYWDTHAT.Count = 2 Then
                            Throw New Exception("Model Element, '" & lrQueryEdge.TargetNode.Name & " " & lrQueryEdge.TargetNode.Alias & "', is not referenced in the query.")
                        ElseIf lrQueryEdge.WhichClause.KEYWDTHAT.Count = 1 Then
                            If lrQueryEdge.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                lrQueryEdge.WhichClause.NODEPROPERTYIDENTIFICATION Is Nothing Then
                                Throw New Exception("Model Element, '" & lrQueryEdge.TargetNode.Name & " " & lrQueryEdge.TargetNode.Alias & "', is not referenced in the query.")
                            End If
                        End If
                End Select
            Next

        End Sub

        Private Function createUniqueNodeAlias(ByVal arNode As FactEngine.QueryNode, aiAlias As Integer) As String

            Dim lrTrialNode = Me.Nodes.Find(Function(x) x.Name = arNode.Name And x.Alias = aiAlias.ToString)

            If lrTrialNode IsNot Nothing Then
                Return Me.createUniqueNodeAlias(arNode, aiAlias + 1)
            End If

            Return aiAlias.ToString

        End Function

        Private Sub setQueryEdgeAliases()

            Dim larQuerEdgeWithFBMFactType = Me.QueryEdges.FindAll(Function(x) x.FBMFactType IsNot Nothing)
            Dim larRDSTableQueryEdge = larQuerEdgeWithFBMFactType.FindAll(Function(x) x.FBMFactType.isRDSTable)

            Dim liInd2 As Integer
            For liInd = 0 To larRDSTableQueryEdge.Count - 1
                If larRDSTableQueryEdge.Count - 1 > 0 And (liInd + 1) <= larRDSTableQueryEdge.Count - 1 Then
                    For liInd2 = liInd + 1 To larRDSTableQueryEdge.Count - 1
                        If larRDSTableQueryEdge(liInd2).FBMFactType Is larRDSTableQueryEdge(liInd).FBMFactType Then
                            If larRDSTableQueryEdge(liInd2).Alias Is Nothing Then
                                If larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge IsNot Nothing Then
                                    If larRDSTableQueryEdge(liInd2).IsPartialFactTypeMatch And
                                       larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge.FBMFactType Is larRDSTableQueryEdge(liInd2).FBMFactType And
                                       larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge.FBMPredicatePart IsNot larRDSTableQueryEdge(liInd2).FBMPredicatePart Then
                                        larRDSTableQueryEdge(liInd2).Alias = larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge.Alias
                                    Else
                                        Dim lrTemplQueryEdge = Me.QueryEdges.Find(Function(x) x.Id = larRDSTableQueryEdge(liInd2).Id)
                                        lrTemplQueryEdge.Alias = (liInd + 1).ToString
                                    End If
                                Else
                                    Dim lrTemplQueryEdge = Me.QueryEdges.Find(Function(x) x.Id = larRDSTableQueryEdge(liInd2).Id)
                                    lrTemplQueryEdge.Alias = (liInd + 1).ToString
                                End If
                            End If
                        End If
                    Next
                End If
            Next

        End Sub

    End Class

    Friend Class NewClass
        Public Property FBMFactType As FBM.FactType
        Public Property Item As Object

        Public Sub New(fBMFactType As FBM.FactType, item As Object)
            Me.FBMFactType = fBMFactType
            Me.Item = item
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim other = TryCast(obj, NewClass)
            Return other IsNot Nothing
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return 0
        End Function
    End Class
End Namespace
