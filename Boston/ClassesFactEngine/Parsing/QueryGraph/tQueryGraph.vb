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

        Public Function FindPreviousQueryEdgeBaseNodeByModelElementName(ByVal asModelElementName As String) As FactEngine.QueryNode

            Try
                Dim liInd As Integer = Me.QueryEdges.Count - 1

                For Each lrQueryEdge In Me.QueryEdges.ToArray.Reverse
                    If lrQueryEdge.BaseNode.Name = asModelElementName Then
                        Return lrQueryEdge.BaseNode
                    End If
                Next

                Return Nothing

            Catch ex As Exception
                Debugger.Break()
                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Generates SQL to run against the database for this QueryGraph
        ''' </summary>
        ''' <returns></returns>
        Public Function generateSQL(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement,
                                    Optional ByVal abIsCountStarSubQuery As Boolean = False) As String

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
                    Dim larProjectionColumn = Me.getProjectionColumns(arWhichSelectStatement)
                    Me.ProjectionColumn = larProjectionColumn


                    For Each lrProjectColumn In larProjectionColumn.FindAll(Function(x) x IsNot Nothing)

                        If lrProjectColumn.Role.FactType.IsDerived Then
                            lsSelectClause &= "[" & lrProjectColumn.Role.FactType.Id & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "]." & lrProjectColumn.Name
                        Else
                            lsSelectClause &= "[" & lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "]." & lrProjectColumn.Name
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
                            lsSQLQuery &= ", COUNT(*)"
                            lbRequiresGroupByClause = True
                        End If
                    End If

                    '20201015-VM-If all seems fine, remove the following.
                    'Select Case (Me.HeadNode.FBMModelObject).GetType
                    '    Case GetType(FBM.ValueType)
                    '        'lsSQLQuery &= Me.HeadNode. Me.HeadNode.FBMModelObject.Id
                    '    Case Else
                    '        larColumn = Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                    '        liInd = 1
                    '        For Each lrColumn In larColumn
                    '            lsSQLQuery &= lrColumn.Table.Name & "." & lrColumn.Name
                    '            If liInd < larColumn.Count Then lsSQLQuery &= ","
                    '            liInd += 1
                    '        Next
                    'End Select

                    'If Me.getProjectQueryEdges.Count > 0 And larColumn.Count > 0 Then lsSQLQuery &= ", "

                    'liInd = 1
                    'Dim larProjectQueryEdge = Me.getProjectQueryEdges()
                    'For Each lrQueryEdge In larProjectQueryEdge
                    '    'larColumn = lrQueryEdge.FBMFactType.RoleGroup(1).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                    '    Dim liRoleInd As Integer
                    '    If lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id Then
                    '        liRoleInd = 1
                    '    Else
                    '        liRoleInd = 0
                    '    End If
                    '    larColumn = New List(Of RDS.Column)
                    '    If lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                    '        larColumn.Add(lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType))
                    '    Else
                    '        larColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                    '    End If

                    '    Dim liInd2 = 1
                    '    For Each lrColumn In larColumn
                    '        If lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                    '            lsSQLQuery &= lrColumn.Table.Name & "." _
                    '                      & lrColumn.Name
                    '        Else
                    '            lsSQLQuery &= lrColumn.Table.Name & "." _ 'lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.Name & "." _
                    '                      & lrColumn.Name
                    '        End If

                    '        If liInd2 < larColumn.Count Then lsSQLQuery &= ","
                    '        liInd2 += 1
                    '    Next

                    '    If liInd < Me.getProjectQueryEdges.Count Then lsSQLQuery &= ","
                    '    liInd += 1
                    'Next
#End Region
                Else
                    lsSQLQuery = " 1 > (SELECT COUNT(*)"
                End If

                lsSQLQuery &= vbCrLf & "FROM "

#Region "FromClause"
                liInd = 1
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
                                                                         Select Node
                                                                        ).ToList

                larFromNodes.RemoveAll(Function(x) larSubQueryNodes.Contains(x))

                For Each lrQueryNode In larFromNodes
                    If lrQueryNode.Alias Is Nothing Then
                        lsSQLQuery &= "[" & lrQueryNode.RDSTable.Name & "]" 'FBMModelObject.getCorrespondingRDSTable.Name
                    Else
                        'FBMModelObject.getCorrespondingRDSTable.Name
                        lsSQLQuery &= lrQueryNode.Name & " " & lrQueryNode.Name & Viev.NullVal(lrQueryNode.Alias, "")
                    End If

                    If liInd < larFromNodes.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                liInd = 1
                Dim larQuerEdgeWithFBMFactType = Me.QueryEdges.FindAll(Function(x) x.FBMFactType IsNot Nothing Or (x.IsRecursive And x.IsCircular))
                Dim larRDSTableQueryEdge = larQuerEdgeWithFBMFactType.FindAll(Function(x) (x.FBMFactType.isRDSTable And
                                                                                           Not x.FBMFactType.IsDerived And
                                                                                           Not x.IsPartialFactTypeMatch) And
                                                                                           Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery) Or
                                                                                           (x.IsRecursive And (
                                                                                           x.IsCircular Or
                                                                                           x.TargetNode.RDSTable.isCircularToTable(x.BaseNode.RDSTable))))

                Dim larRecursiveTableQueryEdge = From QueryEdge In Me.QueryEdges
                                                 Where QueryEdge.IsRecursive And QueryEdge.TargetNode.RDSTable.isCircularToTable(QueryEdge.BaseNode.RDSTable)
                                                 Select QueryEdge

                For Each lrQueryEdge In larRecursiveTableQueryEdge
                    larRDSTableQueryEdge.AddUnique(lrQueryEdge)
                Next


                'RDS Tables
                For Each lrQueryEdge In larRDSTableQueryEdge

                    If Not lrQueryEdge.IsRecursive Then
                        If Me.Nodes.FindAll(Function(x) x.Name = lrQueryEdge.FBMFactType.Id).Count = 0 Then
                            lsSQLQuery &= vbCrLf & "," & lrQueryEdge.FBMFactType.Id
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
                            lsSQLQuery &= vbCrLf & " SELECT [" & lrPGSRelationTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrPGSRelationTable.Name & "].") & ",0"
                            lsSQLQuery &= vbCrLf & " FROM [" & lrPGSRelationTable.Name & "]"
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

                                    lsSQLQuery &= "[" & lrPGSRelationTable.Name & "]." & lrPGSRelationColumn.First.Name & " = " & lrQueryEdge.BaseNode.IdentifierList(liInd)
                                    liInd += 1
                                Next
                            End If
                            lsSQLQuery &= vbCrLf & " UNION"
                            lsSQLQuery &= " SELECT [" & lrPGSRelationTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrPGSRelationTable.Name & "].") & ",depth+1"
                            lsSQLQuery &= vbCrLf & " FROM nodes, [" & lrPGSRelationTable.Name & "]"
                            lsSQLQuery &= vbCrLf & " WHERE nodes." & larJoinColumn(1).Name & " = [" & lrPGSRelationTable.Name & "]." & larJoinColumn(0).Name
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
                            lsSQLQuery &= vbCrLf & " SELECT [" & lrPGSRelationTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrPGSRelationTable.Name & "].") & ",1 as level,(" & larJoinColumn(0).Name & " || '->' || " & larJoinColumn(1).Name & ") AS path"
                            lsSQLQuery &= vbCrLf & " FROM [" & lrPGSRelationTable.Name & "]"
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

                                    lsSQLQuery &= "[" & lrPGSRelationTable.Name & "]." & lrPGSRelationColumn.First.Name & " = " & lrQueryEdge.BaseNode.IdentifierList(liInd)
                                    liInd += 1
                                Next
                            End If
                            lsSQLQuery &= vbCrLf & " UNION ALL"
                            lsSQLQuery &= " SELECT [" & lrPGSRelationTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrPGSRelationTable.Name & "].") & ",level+1, (nodes.path || '->' || [" & lrPGSRelationTable.Name & "]." & larJoinColumn(1).Name & ") AS path"
                            lsSQLQuery &= vbCrLf & " FROM nodes JOIN [" & lrPGSRelationTable.Name & "]"
                            lsSQLQuery &= vbCrLf & " ON [" & lrPGSRelationTable.Name & "]." & larJoinColumn(0).Name & " = nodes." & larJoinColumn(1).Name
                            lsSQLQuery &= vbCrLf & " WHERE ([" & lrPGSRelationTable.Name & "]." & larJoinColumn(1).Name & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0) & " OR level<10)"
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
                            lsSQLQuery &= vbCrLf & " Select [" & lrRDSTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrRDSTable.Name & "].") & ",0"
                            lsSQLQuery &= vbCrLf & " FROM [" & lrRDSTable.Name & "]"
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                lsSQLQuery &= vbCrLf & "," & "[" & lrQueryEdge.TargetNode.RDSTable.Name & "]"
                            End If
                            If lrQueryEdge.BaseNode.IdentifierList.Count > 0 Then
                                Dim lrTargetTable As RDS.Table = larLeftColumn(0).Relation.Find(Function(x) x.OriginTable.Name = lrRDSTable.Name).DestinationTable
                                lsSQLQuery &= "," & lrTargetTable.Name & vbCrLf & " WHERE "
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= "[" & lrTargetTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "]." & lrColumn.Name & " = '"
                                    lsSQLQuery &= lrQueryEdge.BaseNode.IdentifierList(liInd) & "'" & vbCrLf
                                    If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then lsSQLQuery &= "AND "
                                    liInd += 1
                                Next
                                lsSQLQuery &= vbCrLf & " AND [" & lrRDSTable.Name & "]." & larLeftColumn(0).Name & " = [" & lrTargetTable.Name & "]." & lrTargetTable.getPrimaryKeyColumns(0).Name
                            End If
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                Dim lrTargetTable As RDS.Table = lrQueryEdge.TargetNode.RDSTable
                                lsSQLQuery &= vbCrLf & " WHERE [" & lrRDSTable.Name & "]." & larRightColumn(0).Name & " = [" & lrTargetTable.Name & "]." & lrTargetTable.getPrimaryKeyColumns(0).Name
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= vbCrLf & " AND "
                                    lsSQLQuery &= "[" & lrTargetTable.Name & "]." & lrColumn.Name & " = '"
                                    lsSQLQuery &= lrQueryEdge.TargetNode.IdentifierList(liInd) & "'" & vbCrLf
                                    liInd += 1
                                Next
                                lrQueryEdge.TargetNode.IsExcludedConditional = True
                            End If
                            lsSQLQuery &= vbCrLf & " UNION "
                            lsSQLQuery &= vbCrLf & " SELECT [" & lrRDSTable.Name & "]." & Strings.Join(lasColumnName.ToArray, ",[" & lrRDSTable.Name & "].") & ",depth + 1"
                            lsSQLQuery &= vbCrLf & " FROM nodes, [" & lrRDSTable.Name & "]"
                            lsSQLQuery &= vbCrLf & " WHERE "
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                lsSQLQuery &= "Nodes." & larLeftColumn(0).Name & " = [" & lrRDSTable.Name & "]." & larRightColumn(0).Name
                            Else
                                lsSQLQuery &= "Nodes." & larRightColumn(0).Name & " = [" & lrRDSTable.Name & "]." & larLeftColumn(0).Name
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
                Dim lrDerivationProcessor As New FEQL.Processor(prApplication.WorkingModel)

                Dim larDerivedFactType = From QueryEdge In Me.QueryEdges
                                         Where QueryEdge.FBMFactType.IsDerived
                                         Select New With {QueryEdge.FBMFactType, QueryEdge.Alias}

                Dim lasAlias As New List(Of String)
                For Each lrQueryEdge In larDerivedFactType 'Me.QueryEdges.FindAll(Function(x) x.FBMFactType.IsDerived)
                    If Not lasAlias.Contains(lrQueryEdge.FBMFactType.Id & NullVal(lrQueryEdge.Alias, "")) Then

                        Dim larQueryEdge = (From QueryEdge In Me.QueryEdges
                                            Where QueryEdge.FBMFactType Is lrQueryEdge.FBMFactType
                                            Where NullVal(QueryEdge.Alias, "") = NullVal(lrQueryEdge.Alias, "")
                                            Select QueryEdge).ToArray

                        lsSQLQuery &= "," & vbCrLf
                        lsSQLQuery &= lrDerivationProcessor.processDerivationText((lrQueryEdge.FBMFactType.DerivationText).Replace(vbCr, " "),
                                                                                   lrQueryEdge.FBMFactType,
                                                                                   larQueryEdge)

                        lasAlias.Add(lrQueryEdge.FBMFactType.Id & NullVal(lrQueryEdge.Alias, ""))
                    End If
                Next

                'PartialFactTypeMatch
                Dim lrPartialMatchFactType As FBM.FactType = Nothing
                For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery) And Not x.FBMFactType.IsDerived)
                    If lrQueryEdge.IsPartialFactTypeMatch Then

                        Dim larExistingNode = From Node In larFromNodes
                                              Where Node.Name = lrQueryEdge.FBMFactType.Id
                                              Where Node.Alias = lrQueryEdge.Alias
                                              Select Node

                        If (lrQueryEdge.FBMFactType IsNot lrPartialMatchFactType) And larExistingNode.Count = 0 Then
                            lsSQLQuery &= vbCrLf & "," & lrQueryEdge.FBMFactType.Id
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
                                                                                       x.FBMFactType.isRDSTable)
                'And
                'Not (x.IsPartialFactTypeMatch And
                'x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))
                ')

                'Add special DerivedFactType WhereEdges
                larWhereEdges.AddRange(Me.QueryEdges.FindAll(Function(x) x.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count))


                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.TargetNode.MathFunction <> pcenumMathFunction.None) And
                                                                                              (Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                'BooleanPredicate edges. E.g. Protein is enzyme
                larConditionalQueryEdges.AddRange(Me.QueryEdges.FindAll(Function(x) x.TargetNode Is Nothing And Not (x.FBMFactType.IsDerived And x.TargetNode.FBMModelObject.GetType Is GetType(FBM.ValueType))))

                'ShortestPath conditionals are excluded.
                '  E.g. For '(Account:1) made [SHORTEST PATH 0..10] WHICH Transaction THAT was made to (Account 2:4) '
                '  the second QueryEdge conditional is taken care of in the FROM clause processing for the recursive query.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                'Recursive NodePropertyIdentification conditionals are excluded.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNode.IsExcludedConditional)

                If larWhereEdges.Count = 0 And larConditionalQueryEdges.Count = 0 And (Not Me.HeadNode.HasIdentifier) Then
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

                    If lrQueryEdge.IsPartialFactTypeMatch Then 'And lrQueryEdge.TargetNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
                        Dim liInd2 = 0
                        Dim lrNaryTable As RDS.Table = lrQueryEdge.FBMFactType.getCorrespondingRDSTable
                        If lrQueryEdge.BaseNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
                            For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                                If liInd2 > 0 Then lsSQLQuery &= Richmond.returnIfTrue(lbAddedAND, "", " AND ")
                                lsSQLQuery &= "[" & lrNaryTable.Name & "]." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                lsSQLQuery &= "=" & "[" & lrColumn.Table.Name & "]." & lrColumn.Name & vbCrLf
                                liInd2 += 1
                                lbAddedAND = False
                            Next
                        End If
                        If lrQueryEdge.TargetNode.FBMModelObject.GetType <> GetType(FBM.ValueType) Then
                            If Not lbAddedAND Then lsSQLQuery &= Richmond.returnIfTrue(lbAddedAND, "", " AND ")
                            liInd2 = 0
                            For Each lrColumn In lrQueryEdge.TargetNode.RDSTable.getPrimaryKeyColumns
                                If liInd2 > 0 Then lsSQLQuery &= " AND "
                                lsSQLQuery &= "[" & lrNaryTable.Name & "]." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                lsSQLQuery &= "=" & "[" & lrColumn.Table.Name & "]." & lrColumn.Name & vbCrLf
                                liInd2 += 1
                            Next
                        End If
                    ElseIf lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatIdentityCompatitor Then
                        'E.g. Of the type "Person 1 Is Not Person 2" or "Person 1 Is Person 2"

                        lsSQLQuery &= "("
                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= "[" & lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "]." & lrColumn.Name
                            If lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.ISClause Then
                                lsSQLQuery &= " = "
                            Else
                                lsSQLQuery &= " <> "
                            End If
                            lsSQLQuery &= "[" & lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "]." & lrColumn.Name
                        Next
                        lsSQLQuery &= ")"

                    ElseIf lrQueryEdge.FBMFactType.isRDSTable And lrQueryEdge.FBMFactType.Arity = 2 Then
                        'RDSTable
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
                                                lsSQLQuery &= "[" & lrRelation.OriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "]." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= "[" & lrRelation.DestinationTable.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "]." & lrTargetColumn.Name & vbCrLf
                                        End Select
                                    Case Else
                                        Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsSQLQuery &= "[" & lrRelation.OriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "]." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= "[" & lrRelation.DestinationTable.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "]." & lrTargetColumn.Name & vbCrLf
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

                    ElseIf lrQueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count Then

                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        lrBaseNode = lrQueryEdge.BaseNode
                        lrTargetNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge)

                        lsSQLQuery &= "[" & lrBaseNode.Name & "]." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name & " = " & "[" & lrTargetNode.Name & "]." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name
                    Else
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

                            For Each lrColumn In larOriginColumn
                                'was
                                'lsSQLQuery &= lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                                'lsSQLQuery &= " = " & lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                                lsSQLQuery &= "[" & lrColumn.Table.Name & Viev.NullVal(lrBaseNode.Alias, "") & "]." & lrColumn.Name
                                lsSQLQuery &= " = [" & larTargetColumn(liInd2 - 1).Table.Name & Viev.NullVal(lrTargetNode.Alias, "") & "]." & larTargetColumn(liInd2 - 1).Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                Dim lrOriginColumn = lrRelation.OriginColumns.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                                lsSQLQuery &= "[" & lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "]." & lrOriginColumn.Name
                                lsSQLQuery &= " = " & "[" & lrQueryEdge.BaseNode.Name & "]." & lrColumn.Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        End If
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
                If (Not lbAddedAND And lbIntialWhere Is Nothing And
                    (larConditionalQueryEdges.Count > 0 And larWhereEdges.Count > 0)) Or
                    (Me.HeadNode.HasIdentifier And Not Me.QueryEdges(0).IsRecursive And larWhereEdges.Count > 0) Then
                    lsSQLQuery &= "AND "
                    lbIntialWhere = Nothing
                End If

#Region "WhereConditionals"
                If Me.HeadNode.HasIdentifier And Not Me.QueryEdges(0).IsRecursive Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrTargetTable.Name & Viev.NullVal(Me.HeadNode.Alias, "") & "]." & lrColumn.Name & " = '" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then lsSQLQuery &= "AND "
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
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


                            Dim lrPredicatePart As FBM.PredicatePart = Nothing

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
                                ElseIf lrQueryEdge.IsPartialFactTypeMatch Then
                                    lrResponsibleRole = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr).Role
                                Else
                                    lrResponsibleRole = lrPredicatePart.Role
                                End If

                            End If

                            Dim lrTable As RDS.Table
                            If lrQueryEdge.IsPartialFactTypeMatch Then
                                lrTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "]." & lrColumn.Name & " = "
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0))
                                        lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select

                            Else
                                lrTable = lrQueryEdge.BaseNode.RDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrQueryEdge.BaseNode.RDSTable.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "]." & lrColumn.Name & " = "
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsDateTime As String = Me.Model.DatabaseConnection.FormatDateTime(lrQueryEdge.IdentifierList(0))
                                        lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                    Case Else
                                        lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Richmond.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select

                            End If

                            lbIntialWhere = "AND "
                        Case Else

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrQueryEdge.BaseNode.Name & "]."

                                    Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsSQLQuery &= lrTargetColumn.Name & " = True"

                                Case Else



                                    If lrQueryEdge.TargetNode.MathFunction <> pcenumMathFunction.None Then

                                        'Math function
                                        Dim lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                        Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              "[" & lrTargetTable.Name &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "]." &
                                              lrTargetColumn.Name

                                        lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                        lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString


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
                                            Else
                                                Throw New NotImplementedException("Unkown condition in query. Contact support.")
                                            End If
                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrTargetTable.Name & lsAlias & "]." & lrColumn.Name & " = '" & lrQueryEdge.IdentifierList(0) & "'" & vbCrLf
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
                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrTargetTable.Name & lsAlias & "]." & larIndexColumns(liInd).Name & " = '" & lsIdentifier & "'" & vbCrLf
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

                    If lbIntialWhere IsNot Nothing Then lbHasWhereClause = True
                    lsSQLQuery &= Richmond.returnIfTrue(lbAddedAND Or Not lbHasWhereClause, "", " AND ") & lrSubQueryGraph.generateSQL(arWhichSelectStatement, True)

                    lsSQLQuery &= ")"
                Next
                '=====================================================================================

                '=====================================
                'Group By clause
                If lbRequiresGroupByClause Then
                    lsSQLQuery &= "GROUP BY" & lsSelectClause
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

            Return larQueryEdge

        End Function

        Public Function getProjectionColumns(ByRef arWhichSelectStatement As FEQL.WHICHSELECTStatement) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)

            Try

                If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then

                    For Each lrReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN

                        If lrReturnColumn.MODELELEMENTNAME IsNot Nothing Then

                            If lrReturnColumn.COLUMNNAMESTR Is Nothing Then
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
                    Next

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
                                lrTempColumn = lrColumn.Clone(Nothing, Nothing)
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

                    Dim liRoleInd As Integer
                    'Edge Column/s
                    Dim lrQueryEdge As FactEngine.QueryEdge
                    Dim lrRole As FBM.Role = Nothing
                    For Each lrQueryEdge In Me.getProjectQueryEdges()
                        If lrQueryEdge.FBMFactType.IsBinaryFactType And lrQueryEdge.BaseNode.RelativeFBMModelObject.Id = lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                            liRoleInd = 1 'Other side of a BinaryFactType
                        ElseIf lrQueryEdge.IsPartialFactTypeMatch Then
                            lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(lrQueryEdge.FBMFactTypeReading.PredicatePart.IndexOf(lrQueryEdge.FBMPredicatePart) + 1).Role
                            liRoleInd = lrQueryEdge.FBMFactType.RoleGroup.IndexOf(lrRole)
                        Else
                            liRoleInd = 0 'First Role of a BinaryFactType
                        End If
                        Select Case lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                Dim lrColumn As RDS.Column
                                If lrQueryEdge.IsPartialFactTypeMatch Then
                                    lrColumn = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.Role Is lrRole)
                                Else
                                    lrColumn = lrQueryEdge.BaseNode.RelativeFBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
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
                                       larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge.FBMFactType Is larRDSTableQueryEdge(liInd2).FBMFactType Then
                                        larRDSTableQueryEdge(liInd2).Alias = larRDSTableQueryEdge(liInd2).GetPreviousQueryEdge.Alias
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
