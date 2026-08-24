Imports System.Globalization
Imports System.Reflection

Namespace FactEngine
    Partial Public Class QueryGraph

        'THE MOST
        'WHICH Customer placed THE MOST Orders
        'Can extend to WHICH Customer placed THE MOST Orders THAT has (ShippedDate.YEAR:'2016')
        '
        'Select Case c.*
        'From customers c
        'Where c.customerId In
        '(select customerId
        ' from orders
        ' group by customerId
        ' having count(*) =
        '(select count(*)
        ' from orders ord
        ' group by ord.customerId
        ' order by count(*) desc 
        'LIMIT 1))


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
                                    Optional ByRef abIsSubQuery As Boolean = False,
                                    Optional ByVal asFEQLQuery As String = "") As String

#Region "Setup"
            Dim lsSQLQuery As String = ""
            Dim liInd As Integer
            Dim larColumn As New List(Of RDS.Column)
            Dim lbRequiresGroupByClause As Boolean = False
            Dim lsSelectClause As String = ""
            Dim lbHasDistinctClause As Boolean = False

            Dim lsNameStartWrapperCharacter As String = "["
            Dim lsNameEndWrapperCharacter As String = "]"
            Select Case Me.Model.TargetDatabaseType
                Case pcenumDatabaseType.Snowflake
                    lsNameStartWrapperCharacter = """"
                    lsNameEndWrapperCharacter = """"
                Case Else
                    lsNameStartWrapperCharacter = "["
                    lsNameEndWrapperCharacter = "]"
            End Select

            'Special variables. Used later in code after set.
            Dim larCountStarColumn As IEnumerable(Of FEQL.RETURNCOLUMN)
#End Region

            Try
                'Set the Node Aliases. E.g. If Lecturer occurs twice in the FROM clause, then Lecturer, Lecturer2 etc
                Call Me.setNodeAliases()
                Call Me.setQueryEdgeAliases()

#Region "SELECT Clause"
                Dim larProjectionColumn As New List(Of RDS.Column)

                lbRequiresGroupByClause = arWhichSelectStatement.GROUPBYCLAUSE IsNot Nothing

                If Not abIsCountStarSubQuery Then
                    lsSQLQuery = "SELECT "
                    If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                        If arWhichSelectStatement.RETURNCLAUSE.KEYWDDISTINCT IsNot Nothing And Not arWhichSelectStatement.RETURNCLAUSE.ContainsCountDistinctClause Then
                            lsSQLQuery &= "DISTINCT "
                            lbHasDistinctClause = True
                        End If
                    ElseIf Me.QueryEdges.FindAll(Function(x) x.BaseNode.IsDistinct).Count > 0 Or Me.QueryEdges.FindAll(Function(x) x.TargetNode IsNot Nothing AndAlso x.TargetNode.IsDistinct).Count > 0 Then
                        lsSQLQuery &= "DISTINCT "
                        lbHasDistinctClause = True
                    End If
#Region "ProjectionColums"
                    liInd = 1

                    larProjectionColumn = Me.getProjectionColumns(arWhichSelectStatement, abIsStraightDerivationClause, arDerivedModelElement)
                    Me.ProjectionColumn = larProjectionColumn

                    'Find Nodes with DISTINCT
                    Dim larDISTINCTNode As New List(Of FactEngine.QueryNode)
                    larDISTINCTNode.AddRange(Me.QueryEdges.Where(Function(x) x.TargetNode IsNot Nothing AndAlso x.TargetNode.IsDistinct).Select(Function(x) x.TargetNode))
                    larDISTINCTNode.AddRange(Me.QueryEdges.Where(Function(x) x.BaseNode.IsDistinct).Select(Function(x) x.BaseNode))

                    If lbHasDistinctClause And larDISTINCTNode.Count > 0 Then
                        larProjectionColumn.RemoveAll(Function(x) Not x.IsDistinct And Not larDISTINCTNode.Select(Function(y) y.Name).Contains(x.Table.Name))
                    End If

                    If larProjectionColumn.Count = 0 And arWhichSelectStatement.RETURNCLAUSE Is Nothing Then
                        lsSQLQuery &= "*"
                    Else
                        For Each lrProjectColumn In larProjectionColumn.FindAll(Function(x) x IsNot Nothing)

                            Dim lbColumnIsDerived As Boolean = False
                            Try
                                lbColumnIsDerived = lrProjectColumn.Role.FactType.IsDerived
                            Catch
                                'Defaults to False
                            End Try

#Region "Modifier Function"
                            Select Case lrProjectColumn.NodeModifierFunction
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                    Select Case Me.Model.TargetDatabaseType
                                        Case Is = pcenumDatabaseType.Snowflake
                                            lsSelectClause &= "DATE("
                                        Case Else
                                            lsSelectClause &= "DATE("
                                    End Select
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
#Region "Month"
                                    Select Case Me.Model.TargetDatabaseType
                                        Case Is = pcenumDatabaseType.Snowflake
                                            lsSelectClause &= "DATE_PART(MONTH,"
                                        Case Else
                                            lsSelectClause &= "strftime('%m',"
                                    End Select
#End Region
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
#Region "Year"
                                    Select Case Me.Model.TargetDatabaseType
                                        Case Is = pcenumDatabaseType.Snowflake
                                            lsSelectClause &= "DATE_PART(YEAR,"
                                        Case Else
                                            lsSelectClause &= "strftime('%Y',"
                                    End Select
#End Region
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Hour
#Region "Hour"
                                    Select Case Me.Model.TargetDatabaseType
                                        Case Is = pcenumDatabaseType.Snowflake
                                            lsSelectClause &= "DATE_PART(HOUR,"
                                        Case Else
                                            lsSelectClause &= "strftime('%H',"
                                    End Select
#End Region
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                    lsSelectClause &= "TIME("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                    lsSelectClause &= "LOWER("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                    lsSelectClause &= "UPPER("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Sum
                                    lsSelectClause &= "SUM("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Average
                                    lsSelectClause &= "AVG("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Max
                                    lsSelectClause &= "MAX("
                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Min
                                    lsSelectClause &= "MIN("
                                Case Else
                                    lsSelectClause &= ""
                            End Select
#End Region

                            If lbColumnIsDerived Then
                                If lrProjectColumn.Role.JoinedORMObject.GetType = GetType(FBM.ValueType) Then
                                    'for now
                                    lsSelectClause &= lrProjectColumn.DBName
                                Else
                                    If abIsStraightDerivationClause And lrProjectColumn.FactType.IsDerived Then
                                        lsSelectClause &= lrProjectColumn.Role.FactType.Id & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.DBName
                                    ElseIf abIsStraightDerivationClause And Not lrProjectColumn.Role.FactType.IsObjectified Then
                                        lsSelectClause &= lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.DBName
                                    Else
                                        lsSelectClause &= lrProjectColumn.Role.FactType.Id & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.DBName
                                    End If

                                End If

                            Else
                                Select Case lrProjectColumn.ColumnType
                                    Case Is = pcenumRDSColumnType.StandardRDSColumn
                                        lsSelectClause &= lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lsNameStartWrapperCharacter & lrProjectColumn.DBName & lsNameEndWrapperCharacter
                                    Case Is = pcenumRDSColumnType.ReturnFunctionColumn,
                                              pcenumRDSColumnType.FunctionCall
                                        lsSelectClause &= lrProjectColumn.TemporaryData
                                    Case Else
                                        Throw New NotImplementedException("Not sure what type of Column for ProjectionColumn.")
                                End Select

                                '20230416-was here lsSelectClause &= Boston.returnIfTrue(lrProjectColumn.NodeModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None, "", ")")
                            End If

                            lsSelectClause &= Boston.returnIfTrue(lrProjectColumn.NodeModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None, "", ")")

                            If lrProjectColumn.AsName IsNot Nothing Then
                                lsSelectClause &= " AS " & lrProjectColumn.AsName '20231108-VM-Was lsNameStartWrapperCharacter & lrProjectColumn.AsName & lsNameEndWrapperCharacter
                            End If

                            If liInd < larProjectionColumn.Count Then lsSelectClause &= ","
                            liInd += 1
                        Next
                        lsSQLQuery &= lsSelectClause

                        If arWhichSelectStatement.RETURNCLAUSE IsNot Nothing Then
                            larCountStarColumn = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                 Where ReturnColumn.KEYWDCOUNTSTAR IsNot Nothing
                                                 Select ReturnColumn

                            Dim larCountModifierFunction = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                           Where ReturnColumn.NODEMODIFIERFUNCTION IsNot Nothing
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
                            ElseIf larCountModifierFunction.Count > 0 Then
                                If larProjectionColumn.Count > 0 Then
                                    lbRequiresGroupByClause = True
                                End If
                            End If

                            Dim larCountClauseColumn = From ReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN
                                                       Where ReturnColumn.COUNTCLAUSE IsNot Nothing
                                                       Where ReturnColumn.KEYWDCOUNTSTAR Is Nothing
                                                       Select ReturnColumn.COUNTCLAUSE

                            If larCountClauseColumn.Count > 0 Then

                                Dim lrModelElement As FBM.ModelObject
                                For Each lrCountClause In larCountClauseColumn
                                    lsSQLQuery &= Boston.returnIfTrue(larProjectionColumn.Count > 0, ", ", "")
                                    lsSQLQuery &= "COUNT(DISTINCT "
                                    If lrCountClause.COUNTRETURNCOLUMNCONCATENATION IsNot Nothing Then

                                        lrModelElement = Me.Model.GetModelObjectByName(lrCountClause.COUNTRETURNCOLUMNCONCATENATION.COUNTRETURNCOLUMN.MODELELEMENTNAME, True)
                                        If lrModelElement Is Nothing Then
                                            Throw New Exception(lrCountClause.COUNTRETURNCOLUMNCONCATENATION.COUNTRETURNCOLUMN.MODELELEMENTNAME & " is unknown in the model.")
                                        Else
                                            lsSQLQuery &= lrModelElement.DatabaseName & lrCountClause.COUNTRETURNCOLUMNCONCATENATION.COUNTRETURNCOLUMN.MODELELEMENTSUFFIX & "." & lrCountClause.COUNTRETURNCOLUMNCONCATENATION.COUNTRETURNCOLUMN.COLUMNNAMESTR
                                        End If

                                        For Each lrConcatColumn In lrCountClause.COUNTRETURNCOLUMNCONCATENATION.COUNTRETURNCOLUMNCONCATENATIONSUB

                                            lsSQLQuery &= " || "

                                            If lrConcatColumn.COUNTRETURNCOLUMN IsNot Nothing Then
                                                lrModelElement = Me.Model.GetModelObjectByName(lrConcatColumn.COUNTRETURNCOLUMN.MODELELEMENTNAME, True)
                                                If lrModelElement Is Nothing Then
                                                    Throw New Exception(lrConcatColumn.COUNTRETURNCOLUMN.MODELELEMENTNAME & " is unknown in the model.")
                                                Else
                                                    lsSQLQuery &= lrModelElement.DatabaseName & lrConcatColumn.COUNTRETURNCOLUMN.MODELELEMENTSUFFIX & "." & lrConcatColumn.COUNTRETURNCOLUMN.COLUMNNAMESTR
                                                End If
                                            Else
                                                lsSQLQuery &= lrConcatColumn.QUOTEDSTRING
                                            End If
                                        Next
                                    ElseIf lrCountClause.COUNTRETURNCOLUMN IsNot Nothing Then

                                        lrModelElement = Me.Model.GetModelObjectByName(lrCountClause.COUNTRETURNCOLUMN.MODELELEMENTNAME, True)
                                        If lrModelElement Is Nothing Then
                                            Throw New Exception(lrCountClause.COUNTRETURNCOLUMN.MODELELEMENTNAME & " is unknown in the model.")
                                        Else
                                            lsSQLQuery &= lrModelElement.DatabaseName & lrCountClause.COUNTRETURNCOLUMN.MODELELEMENTSUFFIX & "." & lrCountClause.COUNTRETURNCOLUMN.COLUMNNAMESTR
                                        End If

                                    ElseIf lrCountClause.MODELELEMENTNAME IsNot Nothing Then
                                        'VM....to get PK cOlumns for ModelElement and produce DISTINCT clause
                                        lrModelElement = Me.Model.GetModelObjectByName(lrCountClause.MODELELEMENTNAME, True)
                                        If lrModelElement Is Nothing Then
                                            Throw New Exception(lrCountClause.COUNTRETURNCOLUMN.MODELELEMENTNAME & " is unknown in the model.")
                                        Else
                                            liInd = 0
                                            For Each lrPKColumn In lrModelElement.getCorrespondingRDSTable.getPrimaryKeyColumns
                                                If liInd > 0 Then lsSQLQuery &= " || '-' || "
                                                lsSQLQuery &= lrModelElement.DatabaseName & "." & lrPKColumn.DBName
                                                liInd += 1
                                            Next
                                        End If
                                    End If
                                    lsSQLQuery &= ")"
                                Next
                            End If

                        End If
                    End If
#End Region
                Else
                    lsSQLQuery = " 1 > (SELECT COUNT(*)"
                End If
#End Region

FromClauseSection:
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
                '20230115-VM-Added the following. May need to check.
                Dim larOriginalFromNodes = larFromNodes.ToList

                For Each lrQueryNode In larFromNodes.FindAll(Function(x) Not x.FBMModelObject.IsDerived)
                    If liInd > 0 Then lsSQLQuery &= "," & vbCrLf
                    If lrQueryNode.Alias Is Nothing Then
                        lsSQLQuery &= lrQueryNode.RDSTable.DatabaseName  'FBMModelObject.getCorrespondingRDSTable.Name
                    Else
                        'FBMModelObject.getCorrespondingRDSTable.Name
                        lsSQLQuery &= lrQueryNode.RDSTable.DatabaseName & " " & lrQueryNode.FBMModelObject.DatabaseName & Viev.NullVal(lrQueryNode.Alias, "")
                    End If

                    liInd += 1
                Next

                'Derived EntityTypes
                Dim lrDerivationProcessor As FEQL.Processor = Nothing
                Dim lasAlias As New List(Of String)
#Region "Derived Entity Types"
                For Each lrQueryNode In larFromNodes.FindAll(Function(x) x.FBMModelObject.IsDerived)
                    lrDerivationProcessor = New FEQL.Processor(Me.Model)

                    If liInd > 0 Then lsSQLQuery &= "," & vbCrLf

                    If Not lasAlias.Contains(lrQueryNode.Id & NullVal(lrQueryNode.Alias, "")) Then

                        lsSQLQuery &= lrDerivationProcessor.processDerivationText((lrQueryNode.FBMModelObject.DerivationText).Replace(vbCr, " "), lrQueryNode.FBMModelObject)

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

                Dim lrTargetTable As RDS.Table = Nothing
                Dim lsAlias As String = ""

                'RDS Tables
                For Each lrQueryEdge In larRDSTableQueryEdge

                    If Not lrQueryEdge.IsRecursive Then
                        If Me.Nodes.FindAll(Function(x) x.Name = lrQueryEdge.FBMFactType.Id).Count = 0 Then
                            If Not larFromNodes.Count = 0 Then lsSQLQuery &= vbCrLf & ","
                            lsSQLQuery &= lrQueryEdge.FBMFactType.getCorrespondingRDSTable.DatabaseName
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
                            lsSQLQuery &= vbCrLf & " WHERE nodes." & larJoinColumn(1).DBName & " = " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(0).DBName
                            lsSQLQuery &= vbCrLf & " LIMIT 100"
                            lsSQLQuery &= vbCrLf & ")"
                            lsSQLQuery &= vbCrLf & " SELECT " & Strings.Join(lasColumnName.ToArray, ",") & ",depth"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE depth <= (SELECT MIN(depth) FROM nodes N2 "
                            If lrQueryEdge.BaseNode.HasIdentifier Then
                                lsSQLQuery &= " WHERE " & "N2." & larJoinColumn(1).DBName & " = " & lrQueryEdge.BaseNode.IdentifierList(0)
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
                            lsSQLQuery &= vbCrLf & " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",1 as level,(" & larJoinColumn(0).DBName & " || '->' || " & larJoinColumn(1).DBName & ") AS path"
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

                                    lsSQLQuery &= lrPGSRelationTable.DatabaseName & "." & lrPGSRelationColumn.First.DBName & " = " & lrQueryEdge.BaseNode.IdentifierList(liInd)
                                    liInd += 1
                                Next
                            End If
                            lsSQLQuery &= vbCrLf & " UNION ALL"
                            lsSQLQuery &= " SELECT " & lrPGSRelationTable.DatabaseName & "." & Strings.Join(lasColumnName.ToArray, "," & lrPGSRelationTable.DatabaseName & ".") & ",level+1, (nodes.path || '->' || " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(1).DBName & ") AS path"
                            lsSQLQuery &= vbCrLf & " FROM nodes JOIN " & lrPGSRelationTable.DatabaseName
                            lsSQLQuery &= vbCrLf & " ON " & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(0).DBName & " = nodes." & larJoinColumn(1).DBName
                            lsSQLQuery &= vbCrLf & " WHERE (" & lrPGSRelationTable.DatabaseName & "." & larJoinColumn(1).DBName & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0) & " OR level<10)"
                            lsSQLQuery &= vbCrLf & " AND NOT nodes.path LIKE '%->' || " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & ")"
                            lsSQLQuery &= vbCrLf & " SELECT DISTINCT nodes.*" ' & Strings.Join(lasColumnName.ToArray, ",") & ",depth"
                            lsSQLQuery &= vbCrLf & " FROM nodes,"
                            lsSQLQuery &= vbCrLf & " (SELECT path, level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).DBName & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " AND level = (SELECT level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).DBName & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " )"
                            lsSQLQuery &= vbCrLf & " ) pth"
                            lsSQLQuery &= vbCrLf & " WHERE nodes.level <= (SELECT level"
                            lsSQLQuery &= vbCrLf & " FROM nodes"
                            lsSQLQuery &= vbCrLf & " WHERE " & larJoinColumn(1).DBName & " = " & lrQueryEdge.GetNextQueryEdge.IdentifierList(0)
                            lsSQLQuery &= vbCrLf & " )"
                            lsSQLQuery &= vbCrLf & " AND pth.path LIKE '%' || nodes.path || '%'"
                            lsSQLQuery &= vbCrLf & " ORDER BY level) " & lrPGSRelationTable.DBName & lrQueryEdge.TargetNode.Alias

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
                                lrTargetTable = larLeftColumn(0).Relation.Find(Function(x) x.OriginTable.Name = lrRDSTable.Name).DestinationTable
                                lsSQLQuery &= "," & lrTargetTable.Name & vbCrLf & " WHERE "
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= lrTargetTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & " = '"
                                    lsSQLQuery &= lrQueryEdge.BaseNode.IdentifierList(liInd) & "'" & vbCrLf
                                    If liInd < Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns.Count - 1 Then lsSQLQuery &= "AND "
                                    liInd += 1
                                Next
                                lsSQLQuery &= vbCrLf & " AND " & lrRDSTable.DatabaseName & "." & larLeftColumn(0).DBName & " = " & lrTargetTable.DatabaseName & "." & lrTargetTable.getPrimaryKeyColumns(0).DBName
                            End If
                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                lrTargetTable = lrQueryEdge.TargetNode.RDSTable
                                lsSQLQuery &= vbCrLf & " WHERE " & lrRDSTable.DatabaseName & "." & larRightColumn(0).DBName & " = " & lrTargetTable.DatabaseName & "." & lrTargetTable.getPrimaryKeyColumns(0).DBName
                                liInd = 0
                                For Each lrColumn In lrTargetTable.getFirstUniquenessConstraintColumns
                                    lsSQLQuery &= vbCrLf & " AND "
                                    lsSQLQuery &= lrTargetTable.DatabaseName & "." & lrColumn.DBName & " = '"
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
                lrDerivationProcessor = New FEQL.Processor(Me.Model)

                Dim larDerivedFactType = (From QueryEdge In Me.QueryEdges
                                          Where QueryEdge.FBMFactType IsNot Nothing
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
                Dim larPotentialPartialFactTypeMatchEdges = From QueryEdge In Me.QueryEdges
                                                            Where QueryEdge.FBMFactType IsNot Nothing
                                                            Where Not (QueryEdge.IsSubQueryLeader Or QueryEdge.IsPartOfSubQuery)
                                                            Where Not QueryEdge.FBMFactType.IsDerived
                                                            Where Not QueryEdge.FBMFactType.IsUnaryFactType
                                                            Select QueryEdge

                For Each lrQueryEdge In larPotentialPartialFactTypeMatchEdges '20230126-VM-Was Me.QueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery) And Not x.FBMFactType.IsDerived And Not x.FBMFactType.IsUnaryFactType)
                    If lrQueryEdge.IsPartialFactTypeMatch Then

                        Dim larExistingNode = From Node In larOriginalFromNodes
                                              Where (Node.Name = lrQueryEdge.FBMFactType.Id) Or (Node.RelativeFBMModelObject.Id = lrQueryEdge.FBMFactType.Id)
                                              Where (Node.Alias = lrQueryEdge.Alias) Or (Node.Alias Is lrQueryEdge.Alias)
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
#Region "WHERE Precursors"
                Dim larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                             Where QueryEdge.TargetNode IsNot Nothing
                                             Select QueryEdge
                'Where QueryEdge.FBMFactType.IsRDSRelation = False '20240115-VM-Added to stop Many-to-Many tables joining to themselves.

                'WhereEdges are Where joins, rather than ConditionalQueryEdges which test for values by identifiers.
                Dim larWhereEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) ((x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                                                                       x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType) Or
                                                                                       x.IsRDSTable Or
                                                                                       x.IsDerived) And
                                                                                       Not (x.BaseNode.RelativeFBMModelObject.Id = x.TargetNode.RelativeFBMModelObject.Id And
                                                                                            x.BaseNode.Alias = x.TargetNode.Alias))

                Dim larEdgesWithDerivedFactType = (From QueryEdge In Me.QueryEdges
                                                   Where QueryEdge.FBMFactType IsNot Nothing
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
                '20230130-VM-Moved to new QueryEdge.Formula regime.
                'Removed: 'x.TargetNode.MathFunction <> pcenumMathFunction.None Or (below)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.IdentifierList.Count > 0 Or
                                                                                              x.MathComparitor <> FEQL.tFEQLConstants.pcenumFEQLMathComparitor.None Or
                                                                                              x.TargetNode.ModifierFunction <> FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None Or
                                                                                              x.MathClause IsNot Nothing))

                'Derived FactTypes (e.g. Function Columns, may have a BaseNode that has an identifier)
                larConditionalQueryEdges.AddRange(larEdgesWithTargetNode.ToList.FindAll(Function(x) abIsStraightDerivationClause And x.BaseNode.IdentifierList.Count > 0))
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
                    larConditionalQueryEdges.RemoveAll(Function(x) x.IsDerived)
                End If

                'Recursive NodePropertyIdentification conditionals are excluded.
                larConditionalQueryEdges.RemoveAll(Function(x) x.TargetNodeIsExcludedConditional)

                'PartialJoin on the same Table, or simply a Column on the table.
                larRange = From QueryEdge In Me.QueryEdges
                           Where QueryEdge.TargetNode IsNot Nothing
                           Where QueryEdge.IsRDSTable
                           Where QueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType)
                           Where QueryEdge.IdentifierList.Count = 0
                           Where QueryEdge.TargetNode.IdentifierList.Count = 0
                           Where QueryEdge.BaseNode.RDSTable Is QueryEdge.TargetNode
                           Select QueryEdge

                larWhereEdges.RemoveAll(Function(x) larRange.ToList.Contains(x))

                If larWhereEdges.Count = 0 And larConditionalQueryEdges.Count = 0 And (Not Me.HeadNode.HasIdentifier) Then

                    If lbRequiresGroupByClause Then GoTo GroupByClause

                    If NullVal(My.Settings.FactEngineDefaultQueryResultLimit, 0) > 0 Then
                        lsSQLQuery &= vbCrLf & "LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                    End If
                    Return lsSQLQuery
                End If
#End Region

#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = Nothing
                Dim lbHasWhereClause As Boolean = False

                'SQL Just for the WHERE clause joins
                Dim lsWHEREClauseJoinsSQL As String = " WHERE "

#Region "WhereJoins"
                Dim lbWhereJoinAdded As Boolean = False

                For Each lrQueryEdge In larWhereEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))

                    lbWhereJoinAdded = True

                    If lbAddedAND Or liInd > 1 Then
                        lsWHEREClauseJoinsSQL &= "AND "
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
                            If liInd2 > 0 Then lsWHEREClauseJoinsSQL &= "AND "
                            lsWHEREClauseJoinsSQL &= lrOriginTable.DatabaseName & "." & lrColumn.DBName & " = " & lrQueryEdge.FBMFactType.Id & "." & lrColumn.DBName & vbCrLf
                            liInd2 += 1
                        Next

                    ElseIf lrQueryEdge.IsPartialFactTypeMatch Then 'And lrQueryEdge.TargetNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
#Region "Partial FactType match"
                        Dim liInd2 = 0
                        Dim lrNaryTable As RDS.Table = lrQueryEdge.FBMFactType.getCorrespondingRDSTable
                        If lrQueryEdge.BaseNode.FBMModelObject.GetType IsNot GetType(FBM.ValueType) Then
                            For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                                If Not lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).IsDerivationParameter Then
                                    If liInd2 > 0 Then lsWHEREClauseJoinsSQL &= Boston.returnIfTrue(lbAddedAND, "", " AND ")
                                    lsWHEREClauseJoinsSQL &= lrNaryTable.DatabaseName & "." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                    lsWHEREClauseJoinsSQL &= "=" & lrColumn.Table.DatabaseName & "." & lrColumn.DBName & vbCrLf
                                    liInd2 += 1
                                    lbAddedAND = False
                                End If
                            Next
                        End If
                        If lrQueryEdge.TargetNode.FBMModelObject.GetType <> GetType(FBM.ValueType) Then
                            If Not lbAddedAND And (liInd > 1 Or liInd2 > 0) Then lsWHEREClauseJoinsSQL &= Boston.returnIfTrue(lbAddedAND, "", " AND ")
                            liInd2 = 0
                            For Each lrColumn In lrQueryEdge.TargetNode.RDSTable.getPrimaryKeyColumns
                                If liInd2 > 0 Then lsWHEREClauseJoinsSQL &= " AND "
                                lsWHEREClauseJoinsSQL &= lrNaryTable.DatabaseName & "." & lrNaryTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole).Name
                                lsWHEREClauseJoinsSQL &= "=" & lrColumn.Table.Name & "." & lrColumn.DBName & vbCrLf
                                liInd2 += 1
                            Next
                        End If
#End Region
                    ElseIf lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatIdentityCompatitor Then
                        'E.g. Of the type "Person 1 Is Not Person 2" or "Person 1 Is Person 2"
#Region "AndThatIdentityComparitor. 'E.g. Of the type 'Person 1 Is Not Person 2' or 'Person 1 Is Person 2'"

                        lsWHEREClauseJoinsSQL &= "("
                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsWHEREClauseJoinsSQL &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.DBName
                            If lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.ISClause Then
                                lsWHEREClauseJoinsSQL &= " = "
                            Else
                                lsWHEREClauseJoinsSQL &= " <> "
                            End If
                            lsWHEREClauseJoinsSQL &= lrQueryEdge.TargetNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.DBName
                        Next
                        lsWHEREClauseJoinsSQL &= ")"
#End Region
                    ElseIf lrQueryEdge.FBMFactType.IsDerived Then
#Region "Derived"
                        Dim lrDerivationTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable(Nothing, True)

                        If lrDerivationTable Is Nothing Then
                            lrDerivationTable = New RDS.Table(Me.Model.RDS, lrQueryEdge.FBMFactType.Id, lrQueryEdge.FBMFactType)
                        End If

                        liInd = 0
                        Dim larProcessedColumn As New List(Of RDS.Column)
                        For Each lrRole In lrQueryEdge.FBMFactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.GetType <> GetType(FBM.ValueType))
                            'Each Role in Derivation FactType

                            If liInd > 0 Then lsWHEREClauseJoinsSQL &= "AND "

                            'Table joined by Derivation Table/FactType
                            Dim lrDestinationTable As RDS.Table = lrRole.JoinedORMObject.getCorrespondingRDSTable

                            Dim lsDestinationTableAlias = lrQueryEdge.BaseNode.Alias
                            If liInd > 0 And lrQueryEdge.TargetNode IsNot Nothing Then
                                lsDestinationTableAlias = lrQueryEdge.TargetNode.Alias
                            End If

                            Dim liInd2 As Integer = 0
                            For Each lrColumn In lrDestinationTable.getPrimaryKeyColumns

                                If liInd2 > 0 Then
                                    lsWHEREClauseJoinsSQL &= "AND "
                                End If

                                Dim lrDerivationColumn As RDS.Column = lrDerivationTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole And Not larProcessedColumn.Contains(x))
                                If lrDerivationColumn Is Nothing Then
                                    lsWHEREClauseJoinsSQL &= lrDerivationTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & " = "
                                Else
                                    lsWHEREClauseJoinsSQL &= lrDerivationTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrDerivationColumn.DBName & " = "
                                End If
                                lsWHEREClauseJoinsSQL &= lrDestinationTable.DatabaseName & Viev.NullVal(lsDestinationTableAlias, "") & "." & lrColumn.DBName & vbCrLf

                                larProcessedColumn.Add(lrDerivationColumn)

                                liInd2 += 1
                            Next
                            liInd += 1
                        Next
#End Region
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
                                If liTempInd > 0 Then lsWHEREClauseJoinsSQL &= "AND "

                                Select Case liRelationCounter
                                    Case Is = 1
                                        Select Case lrQueryEdge.BaseNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsWHEREClauseJoinsSQL &= lrRelation.OriginTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).DBName & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsWHEREClauseJoinsSQL &= lrRelation.DestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrTargetColumn.DBName & vbCrLf
                                        End Select
                                    Case Else
                                        Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsWHEREClauseJoinsSQL &= lrRelation.OriginTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).DBName & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsWHEREClauseJoinsSQL &= lrRelation.DestinationTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrTargetColumn.DBName & vbCrLf
                                        End Select
                                End Select

                                'lsWHEREClauseJoinsSQL &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = "
                                'lsWHEREClauseJoinsSQL &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name & vbCrLf 'lrOriginTable.getColumnByOrdingalPosition(1).Name & vbCrLf
                                liTempInd += 1
                                liColumnCounter += 1
                            Next
                            liRelationCounter += 1
                        Next

                        'For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                        '    If liTempInd > 0 Then lsWHEREClauseJoinsSQL &= "AND "
                        '    Dim lrTargetColumn = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                        '    lsWHEREClauseJoinsSQL &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = "
                        '    lsWHEREClauseJoinsSQL &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name & vbCrLf 'lrOriginTable.getColumnByOrdingalPosition(1).Name & vbCrLf
                        '    liTempInd += 1
                        'Next

                        'Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                        '    Case = pcenumConceptType.ValueType
                        '        'Nothing to do here
                        '    Case Else
                        '        liTempInd = 0
                        '        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                        '            Dim lrTargetColumn = lrOriginTable.Column.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                        '            lsWHEREClauseJoinsSQL &= vbCrLf & "AND " & lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name & " = "
                        '            lsWHEREClauseJoinsSQL &= lrOriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrTargetColumn.Name 'lrOriginTable.getColumnByOrdingalPosition(2).Name
                        '        Next
                        'End Select
#End Region
                    ElseIf lrQueryEdge.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count Then
#Region "DerivationType = COUNT"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        lrBaseNode = lrQueryEdge.BaseNode
                        lrTargetNode = New FactEngine.QueryNode(lrQueryEdge.FBMFactType, lrQueryEdge)

                        lsWHEREClauseJoinsSQL &= lrBaseNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.DBName & " = " & lrTargetNode.RDSTable.DatabaseName & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.DBName
#End Region
                    Else
#Region "Other/Else"
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        If lrQueryEdge.IsReciprocal Then
                            If lrQueryEdge.FBMFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = lrQueryEdge.BaseNode.Id Then
                                lrBaseNode = lrQueryEdge.TargetNode
                                lrTargetNode = lrQueryEdge.BaseNode
                            Else
                                lrBaseNode = lrQueryEdge.BaseNode
                                lrTargetNode = lrQueryEdge.TargetNode
                            End If

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
                                larOriginColumn.Add(lrColumn.Clone(Nothing, lrRelation))
                            Next

                            For Each lrColumn In lrRelation.DestinationColumns
                                larTargetColumn.Add(lrColumn.Clone(Nothing, Nothing))
                            Next

                            For Each lrColumn In larOriginColumn
                                'was
                                'lsWHEREClauseJoinsSQL &= lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                                'lsWHEREClauseJoinsSQL &= " = " & lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                                lsWHEREClauseJoinsSQL &= lrColumn.Table.DatabaseName & Viev.NullVal(lrBaseNode.Alias, "") & "." & lrColumn.DBName
                                lsWHEREClauseJoinsSQL &= " = " & larTargetColumn(liInd2 - 1).Table.DatabaseName & Viev.NullVal(lrTargetNode.Alias, "") & "." & larTargetColumn(liInd2 - 1).DBName
                                If liInd2 < larTargetColumn.Count Then lsWHEREClauseJoinsSQL &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                Dim lrOriginColumn = lrRelation.OriginColumns.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                                lsWHEREClauseJoinsSQL &= lrQueryEdge.TargetNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrOriginColumn.DBName
                                lsWHEREClauseJoinsSQL &= " = " & lrQueryEdge.BaseNode.RDSTable.DatabaseName & "." & lrColumn.DBName
                                If liInd2 < larTargetColumn.Count Then lsWHEREClauseJoinsSQL &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        End If
#End Region
                    End If

                    'CodeSafe Remove wayward ANDs
                    If Trim(lsWHEREClauseJoinsSQL).EndsWith("AND") Then
                        lsWHEREClauseJoinsSQL = Trim(lsWHEREClauseJoinsSQL).Substring(0, lsWHEREClauseJoinsSQL.Length - 4)
                    End If

                    If Not lsWHEREClauseJoinsSQL.EndsWith(vbCrLf) Then lsWHEREClauseJoinsSQL &= vbCrLf

                    lbAddedAND = False
                    lbIntialWhere = Nothing

                    liInd += 1
                    lbHasWhereClause = True
                Next

#End Region

#Region "WHERE intermediate"
                'Initial WHERE where required
                If lbHasWhereClause Then
                    lsSQLQuery &= lsWHEREClauseJoinsSQL
                End If

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
#End Region

#Region "WhereConditionals"

#Region "HeadNode processing"
                If Me.HeadNode.HasIdentifier And Not lbFirstQueryEdgeIsRecursive Then
                    lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0

                    If Not lbWhereJoinAdded Then
                        lsSQLQuery.AppendLine(lsWHEREClauseJoinsSQL)
                    End If

                    If Me.HeadNode.FBMModelObject.GetType = GetType(FBM.ValueType) And Me.HeadNode.QueryEdge IsNot Nothing AndAlso Me.HeadNode.QueryEdge.FBMFactType.getCorrespondingRDSTable(, True) IsNot Nothing Then
                        'E.g. (Pizza Name:'Margherita') has WHICH Topping
                        larColumn = New List(Of RDS.Column)
                        Dim lrTable = Me.HeadNode.QueryEdge.FBMFactType.getCorrespondingRDSTable
                        Dim lrColumn = lrTable.Column.Find(Function(x) x.Role.Id = Me.HeadNode.QueryEdge.FBMPredicatePart.Role.Id)
                        larColumn.Add(lrColumn)
                    Else
                        Select Case Me.HeadNode.Comparitor
                            Case Is = FEQL.pcenumFEQLComparitor.Colon, FEQL.pcenumFEQLComparitor.LikeComparitor
                                larColumn = Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                            Case Is = FEQL.pcenumFEQLComparitor.Carret
                                larColumn = Me.HeadNode.RDSTable.getPrimaryKeyColumns
                        End Select
                    End If

                    For Each lrColumn In larColumn
                        If liInd > Me.HeadNode.IdentifierList.Count - 1 Then Exit For

                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.DatabaseName & Viev.NullVal(Me.HeadNode.Alias, "") & "." & lrColumn.DBName & Me.HeadNode.getTargetSQLComparator & " '" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If (liInd < larColumn.Count - 1) And (liInd < Me.HeadNode.IdentifierList.Count - 1) Then
                            lsSQLQuery &= "AND "
                        End If
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
                End If
#End Region


                For Each lrQueryEdge In larConditionalQueryEdges.FindAll(Function(x) Not (x.IsSubQueryLeader Or x.IsPartOfSubQuery))

                    If Not lbWhereJoinAdded Then
                        lsSQLQuery.AppendLine(" WHERE ")
                        lbWhereJoinAdded = True
                    End If

                    Select Case True
                        Case lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.AndThatValueComparitor
                            'E.g. "AND THAT Quantity > UnitsInStock"
#Region "AndThatValueComparitor. 'E.g. AND THAT Quantity > UnitsInStock"

                            If {GetType(FBM.EntityType), GetType(FBM.FactType)}.Contains(lrQueryEdge.BaseNode.FBMModelObject.GetType) And
                                    lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) Then

                                'E.g. AND THAT Session is on DateTime >= TODAY
                                '20251108-Was...added ModifierFunction code below.
                                'lsSQLQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrQueryEdge.TargetNode.Name
#Region "Modifier Function"
                                Dim lrTable = lrQueryEdge.BaseNode.RDSTable
                                Dim lrColumn = lrTable.Column.Find(Function(x) x.Name = lrQueryEdge.TargetNode.Name)
                                Select Case lrQueryEdge.TargetNode.ModifierFunction
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                        lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                        lsSQLQuery &= "time(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
#Region "Month"
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSelectClause &= "DATE_PART(MONTH," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                            Case Else
                                                lsSQLQuery &= "strftime('%m'," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                        End Select
#End Region
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
#Region "Year"
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSelectClause &= "DATE_PART(YEAR," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                            Case Else
                                                lsSQLQuery &= "strftime('%Y'," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                        End Select
#End Region
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                        lsSQLQuery &= "lower(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"

                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                        lsSQLQuery &= "upper(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"

                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next

                                        If lrQueryEdge.TargetNode.NEXTCLAUSE.DAYOFTHEWEEK IsNot Nothing Then
                                            lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                        Else
                                            Throw New NotImplementedException("Next Clause without correct Literal/Reserved-Word.")
                                        End If

                                    Case Else
                                        lsSQLQuery &= lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName
                                End Select
#End Region

                                lsSQLQuery &= lrQueryEdge.MathClause.ReturnFormulaAsString(Me.Model.TargetDatabaseType)

                            Else

                                lsSQLQuery &= lrQueryEdge.MathClause.ReturnFormulaAsString(Me.Model.TargetDatabaseType)
                                lsSQLQuery &= lrQueryEdge.TargetNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrQueryEdge.TargetNode.Name

                            End If

#End Region

                        Case lrQueryEdge.WhichClauseSubType = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
#Region "PredicateNodePropertyIdentification"

                            Dim lrFactType As FBM.FactType = Nothing
                            Dim lrPredicatePart As FBM.PredicatePart = Nothing

                            If lrQueryEdge.FBMPredicatePart IsNot Nothing Then
                                lrPredicatePart = lrQueryEdge.FBMPredicatePart
                                lrFactType = lrQueryEdge.FBMFactType
                            Else
#Region "QueryEdge.FBMPrediatePart is NOTHING"
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
#End Region
                            End If
                            Dim lrResponsibleRole As FBM.Role

                            If lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.BaseNode.FBMModelObject Then
                                'Nothing to do here, because is the Predicate joined to the BaseNode that we want the Table for
                            ElseIf Not lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject Then
#Region "Code probably not needed"
                                '20200808-VM-Leave this breakpoint here. If hasn't been hit in years, get rid of this ElseIf
                                lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                                   Select PredicatePart).First
#End Region
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
#Region "lrQueryEdge.IsPartialFactTypeMatch Or lrQueryEdge.FBMFactType.isRDSTable"
                                lrTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                                Dim lrColumn As RDS.Column
                                Try
                                    lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                Catch ex As Exception
                                    Dim lsMessage As String
                                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                                    lsMessage.AppendDoubleLineBreak("Table: " & lrTable.Name)
                                    lsMessage.AppendLine("Column Count: " & lrTable.Column.Count)
                                    lsMessage.AppendLine("QueryEdge.TargetNode.FBMModelElement.Id: " & lrQueryEdge.TargetNode.FBMModelObject.Id)
                                    lsMessage.AppendLine("ResponsibleRole.Id" & lrResponsibleRole.Id)

                                    Throw New ApplicationException(lsMessage)

                                End Try

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "")
#Region "LHS"

#Region "Modifier Function"
                                Select Case lrQueryEdge.TargetNode.ModifierFunction
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                        lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                        lsSQLQuery &= "time(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
#Region "Month"
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSelectClause &= "DATE_PART(MONTH," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                            Case Else
                                                lsSQLQuery &= "strftime('%m'," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                        End Select
#End Region
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
#Region "Year"
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSelectClause &= "DATE_PART(YEAR," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                            Case Else
                                                lsSQLQuery &= "strftime('%Y'," & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                        End Select
#End Region
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                        lsSQLQuery &= "lower(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                        lsSQLQuery &= "upper(" & lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName & ")"
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next
#Region "Next"
                                        If lrQueryEdge.TargetNode.NEXTCLAUSE.DAYOFTHEWEEK IsNot Nothing Then
                                            lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                        Else
                                            Throw New NotImplementedException("Next Clause without correct Literal/Reserved-Word.")
                                        End If
#End Region
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.This
#Region "This"
                                        If lrQueryEdge.TargetNode.THISCLAUSE.DAYOFTHEWEEK IsNot Nothing Then
                                            lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                        Else
                                            Throw New NotImplementedException("Next Clause without correct Literal/Reserved-Word.")
                                        End If
#End Region
                                    Case Else

                                        lsSQLQuery &= lrTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName

                                End Select

#End Region
#End Region

#Region "RHS"
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsSQLQuery &= Me.Model.DatabaseConnection.dateToTextOperator
                                End Select

                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime
                                        Dim lsUserDateTime = If(lrQueryEdge.IdentifierList.Count > 0, lrQueryEdge.IdentifierList(0), "")

                                        Dim lasKeywords = {"TODAY", "TOMORROW"}
                                        If lasKeywords.Contains(lsUserDateTime) Then
                                            Select Case lsUserDateTime
                                                Case Is = "TOMORROW"
                                                    Dim tomorrowDate As Date = Date.Today.AddDays(1)
                                                    lsUserDateTime = tomorrowDate.ToString("yyyy-MM-dd")
                                            End Select
                                        End If

                                        Dim loDateTime As DateTime
                                        Dim lsDateTime As String = lsUserDateTime
                                        Dim format As String = My.Settings.FactEngineUserDateTimeFormat
                                        Select Case lrQueryEdge.TargetNode.ModifierFunction
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                format = My.Settings.FactEngineUserDateFormat
                                                'DateTime.TryParse(lsUserDateTime, loDateTime)
                                                If Not DateTime.TryParseExact(lsUserDateTime, format, Nothing, DateTimeStyles.None, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                End If
                                                lsDateTime = Me.Model.DatabaseConnection.FormatDate(lsUserDateTime)
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year,
                                                      FEQL.pcenumFEQLNodeModifierFunction.Month
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                lsDateTime = lsUserDateTime
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                'Do Nothing
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Between
                                                lsDateTime = lsUserDateTime & " BETWEEN " & lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(0).GetFunctionParameterString & " AND " & lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(1).GetFunctionParameterString & vbCrLf
                                                lsSQLQuery &= lsDateTime
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next
                                                lsDateTime = lsUserDateTime & " = " & lrQueryEdge.TargetNode.NEXTCLAUSE.GetQueryExtension(Me.Model) & vbCrLf
                                                lsSQLQuery &= lsDateTime
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.This
                                                lsDateTime = lsUserDateTime & " = " & lrQueryEdge.TargetNode.THISCLAUSE.GetQueryExtension(Me.Model) & vbCrLf
                                                lsSQLQuery &= lsDateTime
                                            Case Else
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                'Was DateTime.TryParse(lsUserDateTime, loDateTime)
                                                If Not DateTime.TryParseExact(lsUserDateTime, format, Nothing, DateTimeStyles.None, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                End If
                                                lsDateTime = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                        End Select

                                    Case Else
                                        lsSQLQuery &= " = " & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select
#End Region

#End Region
                            Else
                                lrTable = lrQueryEdge.BaseNode.RDSTable

                                Dim lrColumn = (From Column In lrTable.Column
                                                Where Column.Role Is lrResponsibleRole
                                                Where Column.ActiveRole IsNot Nothing
                                                Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                                Select Column).First

                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "")

#Region "LHS"

#Region "ModifierFunction"
                                Dim laiPrefixFunction As New List(Of FEQL.pcenumFEQLNodeModifierFunction) From {}
                                Select Case lrQueryEdge.TargetNode.ModifierFunction
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date,
                                              FEQL.pcenumFEQLNodeModifierFunction.Today,
                                              FEQL.pcenumFEQLNodeModifierFunction.Tomorrow
                                        lsSQLQuery &= "date("
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Date)
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Today)
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Tomorrow)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSQLQuery &= "DATE_PART(MONTH,"
                                            Case Else
                                                lsSQLQuery &= "strftime('%m',"
                                        End Select
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Month)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
                                        Select Case Me.Model.TargetDatabaseType
                                            Case Is = pcenumDatabaseType.Snowflake
                                                lsSQLQuery &= "DATE_PART(YEAR,"
                                            Case Else
                                                lsSQLQuery &= "strftime('%Y',"
                                        End Select
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Year)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                        lsSQLQuery &= "time("
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.Time)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                        lsSQLQuery &= "lower("
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.ToLower)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                        lsSQLQuery &= "upper("
                                        laiPrefixFunction.Add(FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.ToUpper)
                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next
                                        If lrQueryEdge.TargetNode.NEXTCLAUSE.DAYOFTHEWEEK IsNot Nothing Then
                                            lsSQLQuery &= $"date({lrTable.DatabaseName}{Viev.NullVal(lrQueryEdge.Alias, "")}.{lrColumn.DBName})"
                                        Else
                                            Throw New NotImplementedException("Next Clause without correct Literal/Reserved-Word.")
                                        End If
                                    Case Else
                                        lsSQLQuery &= ""
                                End Select
#End Region

                                lsSQLQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.DBName

                                lsSQLQuery &= If(laiPrefixFunction.Contains(lrQueryEdge.TargetNode.ModifierFunction), ")", "")

#End Region

#Region "RHS"
                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDate,
                                              pcenumORMDataType.TemporalDateAndTime
                                        lsSQLQuery &= Me.Model.DatabaseConnection.dateToTextOperator
                                End Select

                                Select Case lrColumn.getMetamodelDataType
                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                              pcenumORMDataType.TemporalDate
                                        Dim lsUserDateTime = If(lrQueryEdge.IdentifierList.Count > 0, lrQueryEdge.IdentifierList(0), "")

                                        Dim lasKeywords = {"TODAY", "TOMORROW"}
                                        If lasKeywords.Contains(lsUserDateTime) Then
                                            Select Case lsUserDateTime
                                                Case Is = "TOMORROW"
                                                    Dim tomorrowDate As Date = Date.Today.AddDays(1)
                                                    lsUserDateTime = tomorrowDate.ToString("yyyy-MM-dd")
                                            End Select
                                        End If

                                        Dim loDateTime As DateTime
                                        Dim lsDateTime As String = lsUserDateTime
                                        Select Case lrQueryEdge.TargetNode.ModifierFunction
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                End If
                                                lsDateTime = Me.Model.DatabaseConnection.FormatDate(lsUserDateTime)
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                'Do Nothing
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year,
                                                      FEQL.pcenumFEQLNodeModifierFunction.Month
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                lsDateTime = lsUserDateTime
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Between
                                                lsDateTime = lsUserDateTime & " BETWEEN " & lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(0).GetFunctionParameterString & " AND " & lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(1).GetFunctionParameterString
                                                lsSQLQuery &= lsDateTime
                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Tomorrow
                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                lsSQLQuery &= $"date('now', '+1 day') "

                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next
                                                lsSQLQuery &= " = " & lrQueryEdge.TargetNode.NEXTCLAUSE.GetQueryExtension(Me.Model)

                                            Case Else

                                                lsSQLQuery &= lrQueryEdge.getTargetSQLComparator
                                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                End If
                                                lsDateTime = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                                lsSQLQuery &= If(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & If(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                        End Select
                                    Case Else
                                        lsSQLQuery &= lrQueryEdge.getTargetSQLComparator & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                End Select

                            End If

#End Region

                            lbIntialWhere = "AND "
#End Region
                        Case lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatModelElementPredicateThatModelElement
#Region "AndThatModelElementPredicateThatModelElement"
                            If lrQueryEdge.MathComparitor <> FEQL.tFEQLConstants.pcenumFEQLMathComparitor.None Then

                                lsSQLQuery &= lbIntialWhere & lrQueryEdge.BaseNode.FBMModelObject.DBName & " " & lrQueryEdge.WhichClause.COMPARITOR.getFEQLMathComparitorToken & " " & lrQueryEdge.TargetNode.FBMModelObject.DBName

                            Else
                                GoTo ConditionalCaseElse
                            End If
#End Region
                        Case Else
ConditionalCaseElse:
#Region "ConditionalCaseElse:"
                            'Sometimes QueryEdges just nominate a ValueType
                            If lrQueryEdge.TargetNode IsNot Nothing Then
                                'Rem Unary QueryEdges/FactTypes do not have a TargetNode.
                                If lrQueryEdge.FBMFactType.IsDerived And
                                       lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) And
                                       lrQueryEdge.IdentifierList.Count = 0 Then

                                    GoTo SkipQueryEdge
                                ElseIf lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) And
                                       lrQueryEdge.IdentifierList.Count = 0 And
                                       lrQueryEdge.MathClause Is Nothing And
                                       lrQueryEdge.BaseNode.HasIdentifier = False And
                                       lrQueryEdge.WhichClause.COMPARITOR Is Nothing And
                                       lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None Then
                                    GoTo SkipQueryEdge
                                End If
                            End If

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable.DatabaseName & lrQueryEdge.BaseNode.Alias & "."

                                    lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsSQLQuery &= lrTargetColumn.DBName & " = True " & vbCrLf

                                    lbIntialWhere = "AND "

                                Case Else

                                    If lrQueryEdge.MathClause IsNot Nothing Then '20230130-VM-new regime: Was: TargetNode.MathFunction <> pcenumMathFunction.None Then
#Region "MathClause"
                                        If lrQueryEdge.FBMFactType.IsDerived Then

                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrQueryEdge.FBMFactType.Id &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              CType(lrQueryEdge.TargetNode.PreboundText & lrQueryEdge.TargetNode.Name, String).Replace("-", "")

                                            '20230130-VM-New Regime. QueryEdge.Formula: Removed below
                                            'lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            'lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf
                                            lsSQLQuery &= lrQueryEdge.FormulaText

                                        Else


                                            'Math function
                                            lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrTargetTable.DatabaseName &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              lrTargetColumn.DBName

                                            '20230130-VM-New Regime. QueryEdge.Formula: Removed below
                                            'lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                            'lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString & vbCrLf
                                            lsSQLQuery &= lrQueryEdge.FormulaText

                                        End If
                                        lbIntialWhere = "AND "
#End Region
                                    Else
                                        lrTargetTable = Nothing
                                        lsAlias = ""

                                        'Check for reciprocal reading. As in WHICH Person was armed by (Person 2:'David') rather than WHICH Person armed (Person 2:'Saul')
                                        If lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) Then
#Region "Target Node is Value Type"
                                            lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                            lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                                            Dim lrColumn As RDS.Column
                                            If lrQueryEdge.FBMFactType.IsLinkFactType Then
#Region "Link Fact Type"
                                                lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.LinkFactTypeRole)

                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "")
#Region "ModifierFunction"
                                                Select Case lrQueryEdge.TargetNode.ModifierFunction
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                        lsSQLQuery &= "date("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
                                                        lsSQLQuery &= "strftime('%m',"
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
                                                        lsSQLQuery &= "strftime('%Y',"
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                        lsSQLQuery &= "time("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                                        lsSQLQuery &= "lower("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                                        lsSQLQuery &= "upper("
                                                    Case Else
                                                        lsSQLQuery &= ""
                                                End Select
#End Region
                                                lsSQLQuery &= lrQueryEdge.BaseNode.RDSTable.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName
                                                lsSQLQuery &= Boston.returnIfTrue(lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None, "", ")")
                                                lsSQLQuery &= " = "

                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate
                                                        Dim lsUserDateTime = If(lrQueryEdge.IdentifierList.Count > 0, lrQueryEdge.IdentifierList(0), "")
                                                        Dim loDateTime As DateTime
                                                        Dim lsDateTime As String = lsUserDateTime
                                                        Select Case lrQueryEdge.TargetNode.ModifierFunction
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                                End If
                                                                lsDateTime = Me.Model.DatabaseConnection.FormatDate(lsUserDateTime)
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                                'Do Nothing
                                                            Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year,
                                                                      FEQL.pcenumFEQLNodeModifierFunction.Month
                                                                lsDateTime = lsUserDateTime
                                                            Case Else
                                                                If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                                    Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                                End If
                                                                lsDateTime = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                                        End Select
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                    Case Else
                                                        lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                End Select
#End Region
                                            Else
#Region "LHS"
                                                If lrQueryEdge.IsPartialFactTypeMatch Then
                                                    lrColumn = lrQueryEdge.RDSColumn
                                                Else
                                                    lrColumn = lrQueryEdge.BaseNode.RDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                                End If

                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "")
#Region "ModifierFunction"
                                                Select Case lrQueryEdge.TargetNode.ModifierFunction
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                        lsSQLQuery &= "date("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Month
                                                        Select Case Me.Model.TargetDatabaseType
                                                            Case Is = pcenumDatabaseType.Snowflake
                                                                lsSQLQuery &= "DATE_PART(MONTH,"
                                                            Case Else
                                                                lsSQLQuery &= "strftime('%m',"
                                                        End Select
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year
                                                        Select Case Me.Model.TargetDatabaseType
                                                            Case Is = pcenumDatabaseType.Snowflake
                                                                lsSQLQuery &= "DATE_PART(YEAR,"
                                                            Case Else
                                                                lsSQLQuery &= "strftime('%Y',"
                                                        End Select
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Hour
                                                        Select Case Me.Model.TargetDatabaseType
                                                            Case Is = pcenumDatabaseType.Snowflake
                                                                lsSQLQuery &= "DATE_PART(HOUR,"
                                                            Case Else
                                                                lsSQLQuery &= "strftime('%H',"
                                                        End Select
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                        lsSQLQuery &= "time("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToLower
                                                        lsSQLQuery &= "lower("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.ToUpper
                                                        lsSQLQuery &= "upper("
                                                    Case Is = FEQL.pcenumFEQLNodeModifierFunction.Next

                                                        If lrQueryEdge.TargetNode.NEXTCLAUSE.DAYOFTHEWEEK IsNot Nothing Then
                                                            lsSQLQuery &= $"date("
                                                        Else
                                                            Throw New NotImplementedException("Next Clause without correct Literal/Reserved-Word.")
                                                        End If
                                                    Case Else
                                                        lsSQLQuery &= ""
                                                End Select
#End Region
                                                '20230119-VM-Was Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.RDSTable
                                                lsSQLQuery &= lrColumn.Table.DatabaseName & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrColumn.DBName

                                                lsSQLQuery &= Boston.returnIfTrue(lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None, "", ")")

                                                If lrQueryEdge.TargetNode.BETWEENCLAUSE IsNot Nothing Then
                                                    lsSQLQuery &= $" BETWEEN '{lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(0).QUOTEDIDENTIFIER.IDENTIFIER}' AND '{lrQueryEdge.TargetNode.BETWEENCLAUSE.FUNCTIONPARAMETER(1).QUOTEDIDENTIFIER.IDENTIFIER}'"
                                                End If

                                                Select Case lrColumn.getMetamodelDataType
                                                    Case Is = pcenumORMDataType.TemporalDate,
                                                              pcenumORMDataType.TemporalDateAndTime
                                                        lsSQLQuery &= Me.Model.DatabaseConnection.dateToTextOperator
                                                End Select
#End Region

#Region "Comparitor and RHS"
                                                If lrQueryEdge.IdentifierList.Count > 0 Then

                                                    'Comparitor
                                                    lsSQLQuery &= lrQueryEdge.getTargetSQLComparator

                                                    Select Case lrColumn.getMetamodelDataType
                                                        Case Is = pcenumORMDataType.TemporalDateAndTime,
                                                              pcenumORMDataType.TemporalDate

                                                            Dim lsUserDateTime = lrQueryEdge.IdentifierList(0)
                                                            Dim loDateTime As DateTime = Nothing
                                                            Dim lsDateTime As String = lsUserDateTime
                                                            Select Case lrQueryEdge.TargetNode.ModifierFunction
                                                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Date
                                                                    If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                                        Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                                    End If
                                                                    lsDateTime = Me.Model.DatabaseConnection.FormatDate(lsUserDateTime)
                                                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Year,
                                                                      FEQL.pcenumFEQLNodeModifierFunction.Month
                                                                    lsDateTime = lsUserDateTime
                                                                Case Is = FEQL.pcenumFEQLNodeModifierFunction.Time
                                                                    'Do nothing 
                                                                Case Else
                                                                    If Not DateTime.TryParse(lsUserDateTime, loDateTime) And (lrQueryEdge.TargetNode.ModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None) Then
                                                                        Throw New Exception(lsUserDateTime & " is not a valid DateTime. Try entering a DateTime value in the FactEngine configuration format: " & My.Settings.FactEngineUserDateTimeFormat)
                                                                    End If
                                                                    lsDateTime = Me.Model.DatabaseConnection.FormatDateTime(lsUserDateTime)
                                                            End Select

                                                            lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lsDateTime & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                        Case Else

                                                            If lrQueryEdge.FBMFactType.IsDerived And lrQueryEdge.TargetNode.FBMModelObject.GetType = GetType(FBM.ValueType) Then
                                                                lsSQLQuery &= lrQueryEdge.FBMFactType.Name & lrQueryEdge.Alias & lrColumn.Name
                                                            Else
                                                                lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & lrQueryEdge.IdentifierList(0) & Boston.returnIfTrue(lrColumn.DataTypeIsNumeric, "", "'") & vbCrLf
                                                            End If

                                                    End Select
                                                End If
#End Region
                                            End If

#End Region
                                                Else
#Region "Target Node is NOT Value Type"
                                            Dim lrQueryNode As FactEngine.QueryNode
                                            If lrQueryEdge.TargetNode.HasIdentifier Then
                                                lrTargetTable = lrQueryEdge.TargetNode.RDSTable
                                                lrQueryNode = lrQueryEdge.TargetNode
                                                lsAlias = Viev.NullVal(lrQueryEdge.TargetNode.Alias, "")
                                            Else
                                                lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                                                lrQueryNode = lrQueryEdge.BaseNode
                                                lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                                            End If

                                            Dim larIndexColumns As New List(Of RDS.Column)
                                            Select Case lrQueryNode.Comparitor
                                                Case Is = FEQL.pcenumFEQLComparitor.Colon
                                                    larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns
                                                Case Is = FEQL.pcenumFEQLComparitor.Carret
                                                    larIndexColumns = lrTargetTable.getPrimaryKeyColumns
                                            End Select

                                            'CodeSafe
                                            If larIndexColumns.Count = 0 Then
                                                Throw New Exception("No Index Columns found for Target Table: " & lrTargetTable.Name)
                                            End If

                                            liInd = 0
                                            For Each lsIdentifier In lrQueryEdge.IdentifierList
                                                If liInd > 0 Then
                                                    lsSQLQuery &= "AND "
                                                    lbIntialWhere = ""
                                                End If
                                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.DatabaseName & lsAlias & "." & larIndexColumns(liInd).DBName & lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "'" & vbCrLf
                                                liInd += 1
                                            Next
                                        End If
#End Region
                                        lbIntialWhere = "AND "
                                    End If
                            End Select
#End Region
                    End Select

                    '=====================================
                    If abIsStraightDerivationClause And lrQueryEdge.BaseNode.IdentifierList.Count > 0 Then
#Region "abIsStraightDerivationClause And lrQueryEdge.BaseNode.IdentifierList.Count > 0"
                        lrTargetTable = lrQueryEdge.BaseNode.RDSTable
                        lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")

                        Dim larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns

                        liInd = 0
                        For Each lsIdentifier In lrQueryEdge.BaseNode.IdentifierList
                            If liInd > 0 And Not lsSQLQuery.EndsWith("AND ") Then
                                lsSQLQuery &= vbCrLf & "AND "
                                lbIntialWhere = ""
                            ElseIf lsSQLQuery.EndsWith("AND ") Then
                                lbIntialWhere = ""
                            End If
                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.DatabaseName & lsAlias & "." & larIndexColumns(liInd).Name & lrQueryEdge.getTargetSQLComparator & "'" & lsIdentifier & "'" & vbCrLf
                            liInd += 1
                        Next

                        lbIntialWhere = "AND "
#End Region
                    End If
                    '======================================

                    lbHasWhereClause = True
SkipQueryEdge:
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
GroupByClause:
#Region "GROUP BY Clause"
                If lbRequiresGroupByClause And larProjectionColumn.Count > 1 Or
                    (lbRequiresGroupByClause AndAlso larProjectionColumn.Count >= 1 AndAlso larCountStarColumn IsNot Nothing AndAlso larCountStarColumn.Count > 0) Or
                    arWhichSelectStatement.GROUPBYCLAUSE IsNot Nothing Then

                    If Not lsSQLQuery.EndsWith(vbCrLf) Then lsSQLQuery &= vbCrLf
                    lsSQLQuery &= "GROUP BY "
                    liInd = 0
                    If arWhichSelectStatement.GROUPBYCLAUSE IsNot Nothing AndAlso arWhichSelectStatement.GROUPBYCLAUSE.KEYWDGROUPBY IsNot Nothing Then

                        For Each lrReturnColumn As FEQL.RETURNCOLUMN In arWhichSelectStatement.GROUPBYCLAUSE.RETURNCOLUMN
                            If liInd > 0 Then lsSQLQuery &= ", "

                            Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrReturnColumn.MODELELEMENTNAME)
                            Dim lrColumn As RDS.Column = Nothing

                            lsSQLQuery &= lrReturnColumn.GETMODELELEMENTSTRING(Me.Model) 'NAME)

                            'If lrModelElement IsNot Nothing Then
                            '    Try
                            '        lrColumn = Me.Model.RDS.Table.Find(Function(x) x.Name = lrModelElement.Id).Column.Find(Function(x) x.Name = lrReturnColumn.COLUMNNAMESTR)
                            '        If lrColumn IsNot Nothing Then
                            '            lsSQLQuery &= If(lrReturnColumn.ASCLAUSE IsNot Nothing, lrReturnColumn.ASCLAUSE.COLUMNNAMESTR, lrColumn.Table.DBName & "." & lrColumn.DBName)
                            '        End If
                            '    Catch
                            '    End Try
                            'Else
                            '    lsSQLQuery &= If(lrReturnColumn.ASCLAUSE IsNot Nothing, lrReturnColumn.ASCLAUSE.COLUMNNAMESTR, lrReturnColumn.GETMODELELEMENTSTRING(Me.Model)) 'NAME)
                            'End If


                            liInd += 1
                        Next
                        GoTo HavingClause
                    End If
                    For Each lrColumn In larProjectionColumn.FindAll(Function(x) x.NodeModifierFunction = FEQL.tFEQLConstants.pcenumFEQLNodeModifierFunction.None)
                        If liInd > 0 Then lsSQLQuery &= ", "
                        Select Case Me.Model.TargetDatabaseType
                            Case Is = pcenumDatabaseType.Snowflake
                                lsSQLQuery &= lsNameStartWrapperCharacter & Boston.returnIfTrue(lrColumn.AsName Is Nothing, lrColumn.DBName, lrColumn.AsName) & lsNameEndWrapperCharacter
                            Case Else
                                lsSQLQuery &= lrColumn.Table.DatabaseName & "." & lsNameStartWrapperCharacter & Boston.returnIfTrue(lrColumn.AsName Is Nothing, lrColumn.DBName, lrColumn.AsName) & lsNameEndWrapperCharacter
                        End Select

                        liInd += 1
                    Next
                End If
#End Region

                '=====================================
                'Having clause
HavingClause:
#Region "HAVING Clause"
                If arWhichSelectStatement.HAVINGCLAUSE IsNot Nothing Then
                    If Not lsSQLQuery.EndsWith(vbCrLf) Then lsSQLQuery &= vbCrLf
                    lsSQLQuery &= "HAVING "

                    liInd = 0
                    For Each lrHavingCondition In arWhichSelectStatement.HAVINGCLAUSE.HAVINGCONDITION

                        If liInd > 0 Then lsSQLQuery &= ","

                        If lrHavingCondition.RETURNCOLUMN.KEYWDCOUNTSTAR IsNot Nothing Then
                            lsSQLQuery &= " COUNT(*)"
                        ElseIf lrHavingCondition.RETURNCOLUMN.NUMBER IsNot Nothing Then
                            lsSQLQuery &= lrHavingCondition.RETURNCOLUMN.NUMBER
                        ElseIf lrHavingCondition.RETURNCOLUMN.QUOTEDSTRING IsNot Nothing Then
                            lsSQLQuery &= lrHavingCondition.RETURNCOLUMN.QUOTEDSTRING
                        ElseIf lrHavingCondition.RETURNCOLUMN.COLUMNNAMESTR IsNot Nothing Then
                            lsSQLQuery &= lrHavingCondition.RETURNCOLUMN.MODELELEMENTNAME & "." & lrHavingCondition.RETURNCOLUMN.COLUMNNAMESTR
                            lsSQLQuery &= " " & lrHavingCondition.MATHCLAUSE.ReturnFormulaAsString
                        Else
                            Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrHavingCondition.RETURNCOLUMN.MODELELEMENTNAME, True)
                            If lrModelElement Is Nothing Then
                                Throw New Exception("Could not find Model Element, " & lrHavingCondition.RETURNCOLUMN.MODELELEMENTNAME & ", in ORDER BY Clause.")
                            Else
                                Dim liInd2 = 0
                                Dim lrTable As RDS.Table = lrModelElement.getCorrespondingRDSTable()
                                If lrTable Is Nothing Then
                                    Throw New Exception("Could not find a Table for Model Element name, " & lrHavingCondition.RETURNCOLUMN.MODELELEMENTNAME & ", in ORDER BY Clause.")
                                End If
                                For Each lrColumn In lrTable.getFirstUniquenessConstraintColumns
                                    If liInd2 > 0 Then lsSQLQuery &= ","
                                    lsSQLQuery &= lrColumn.Table.DatabaseName & "." & lrColumn.DatabaseName
                                    liInd2 += 1
                                Next
                            End If
                        End If

                    Next

                End If
#End Region

OrderByClause:
#Region "ORDER BY Clause"
                If arWhichSelectStatement.ORDERBYCLAUSE IsNot Nothing Then
                    If Not lsSQLQuery.EndsWith(vbCrLf) Then lsSQLQuery &= vbCrLf
                    lsSQLQuery.AppendLine("ORDER BY ")
                    liInd = 0
                    For Each lrOrderByColum In arWhichSelectStatement.ORDERBYCLAUSE.ORDERBYCOLUMN
                        If liInd > 0 Then lsSQLQuery &= ","

                        If lrOrderByColum.RETURNCOLUMN.KEYWDCOUNTSTAR IsNot Nothing Then
                            lsSQLQuery &= " COUNT(*)"
                        ElseIf lrOrderByColum.RETURNCOLUMN.NUMBER IsNot Nothing Then
                            lsSQLQuery &= lrOrderByColum.RETURNCOLUMN.NUMBER
                        ElseIf lrOrderByColum.RETURNCOLUMN.COLUMNNAMESTR IsNot Nothing Then
                            lsSQLQuery &= lrOrderByColum.RETURNCOLUMN.MODELELEMENTNAME & "." & lrOrderByColum.RETURNCOLUMN.COLUMNNAMESTR
                        Else
                            Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrOrderByColum.RETURNCOLUMN.MODELELEMENTNAME, True)
                            If lrModelElement Is Nothing Then
                                Throw New Exception("Could not find Model Element, " & lrOrderByColum.RETURNCOLUMN.MODELELEMENTNAME & ", in ORDER BY Clause.")
                            Else
                                Dim liInd2 = 0
                                Dim lrTable As RDS.Table = lrModelElement.getCorrespondingRDSTable()
                                If lrTable Is Nothing Then
                                    Throw New Exception("Could not find a Table for Model Element name, " & lrOrderByColum.RETURNCOLUMN.MODELELEMENTNAME & ", in ORDER BY Clause.")
                                End If
                                For Each lrColumn In lrTable.getFirstUniquenessConstraintColumns
                                    If liInd2 > 0 Then lsSQLQuery &= ","
                                    lsSQLQuery &= lrColumn.Table.DatabaseName & "." & lrColumn.DatabaseName
                                    liInd2 += 1
                                Next
                            End If
                        End If

                        Select Case lrOrderByColum.OrderByDirection
                            Case Is = FEQL.pcenumFEQLOrderByDirection.None
                            Case Is = FEQL.pcenumFEQLOrderByDirection.Ascending
                                lsSQLQuery &= " ASC"
                            Case Is = FEQL.pcenumFEQLOrderByDirection.Descending
                                lsSQLQuery &= " DESC"
                        End Select

                        liInd += 1
                    Next

                End If
#End Region

#Region "LIMIT Clause"
                If arWhichSelectStatement.LIMITCLAUSE IsNot Nothing Then
                    lsSQLQuery.AppendLine("LIMIT " & arWhichSelectStatement.LIMITCLAUSE.NUMBER)
                ElseIf Not abIsStraightDerivationClause And Not abIsSubQuery And NullVal(My.Settings.FactEngineDefaultQueryResultLimit, 0) > 0 Then
                    lsSQLQuery &= vbCrLf & "LIMIT " & My.Settings.FactEngineDefaultQueryResultLimit
                End If
#End Region

                Return lsSQLQuery

            Catch ex As Exception
                Dim lsMessage As String = ""
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error in: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendLine(ex.Message)
                lsMessage.AppendLine(asFEQLQuery)
                lsMessage.AppendDoubleLineBreak(ex.Message & vbCrLf & vbCrLf & lsSQLQuery & vbCrLf & "At line: " & Boston.ExtractExceptionLine(ex))
                lsMessage.AppendDoubleLineBreak(ex.StackTrace)
                Throw New Exception(lsMessage)
            End Try

        End Function

    End Class

End Namespace

