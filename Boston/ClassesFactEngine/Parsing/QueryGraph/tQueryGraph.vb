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

        Public Function FindPreviousQueryEdgeBaseNodeByTargetNodeName(ByVal asModelElementName As String) As FactEngine.QueryNode

            Try
                Dim liInd As Integer = Me.QueryEdges.Count - 1

                If Me.QueryEdges.Count = 0 Then
                    Return Nothing
                Else
                    For Each lrQueryEdge In Me.QueryEdges.ToArray.Reverse
                        If lrQueryEdge.TargetNode.Name = asModelElementName Then
                            Return lrQueryEdge.BaseNode
                        End If
                    Next
                End If

                Return Nothing

            Catch ex As Exception
                Return Nothing
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

        Public Function getNodeModelElementList(Optional abGetSupertypeModelElements As Boolean = False,
                                                Optional abIgnoreTargetNodes As Boolean = False) As List(Of FBM.ModelObject)

            Dim larModelElement As New List(Of FBM.ModelObject)

            Try
                If Me.HeadNode IsNot Nothing Then
                    larModelElement.AddUnique(Me.HeadNode.FBMModelObject)
                End If
                For Each lrQueryEdge In Me.QueryEdges
                    If lrQueryEdge.BaseNode.FBMModelObject IsNot Nothing Then
                        larModelElement.AddUnique(lrQueryEdge.BaseNode.FBMModelObject)
                    End If
                    If lrQueryEdge.TargetNode.FBMModelObject IsNot Nothing And Not abIgnoreTargetNodes Then
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
#Region "RETURNCLAUSE"
                    For Each lrReturnColumn In arWhichSelectStatement.RETURNCLAUSE.RETURNCOLUMN

                        If lrReturnColumn.MODELELEMENTNAME IsNot Nothing And lrReturnColumn.KEYWDCOUNTSTAR Is Nothing And lrReturnColumn.COUNTCLAUSE Is Nothing And lrReturnColumn.RETURNFUNCTION Is Nothing Then
#Region "ModelElementName"
                            If lrReturnColumn.COLUMNNAMESTR Is Nothing Then
#Region "ColumnNameStr is Nothing"
                                'Check if user forgot to put TableName as in Cinema.CinemaName
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
                                Dim lrTempColumn As RDS.Column = Nothing
                                Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrReturnColumn.MODELELEMENTNAME, True)
                                If lrModelElement IsNot Nothing Then
                                    Try
                                        Select Case lrModelElement.GetType
                                            Case Is = GetType(FBM.ValueType)
                                                GoTo MoveForward
                                            Case Is = GetType(FBM.EntityType),
                                                          GetType(FBM.FactType)
                                                For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                                                    lrTempColumn.IsDistinct = arWhichSelectStatement.RETURNCLAUSE.KEYWDDISTINCT IsNot Nothing
                                                    larColumn.Add(lrTempColumn)
                                                Next
                                                GoTo MoveForward
                                            Case Else
                                                Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & ".*, not found. Check your RETURN clause.")
                                        End Select
                                    Catch ex As Exception
                                        Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & ".*, not found. Check your RETURN clause.")
                                    End Try
                                End If

                                'Must be * (as in Lecturer.*)
                                Try
                                    Dim lrTable As RDS.Table = Me.Model.RDS.Table.Find(Function(x) x.Name = lrReturnColumn.MODELELEMENTNAME)
                                    For Each lrColumn In lrTable.Column
                                        larColumn.Add(lrColumn.Clone(Nothing, Nothing))
                                    Next
                                Catch ex As Exception
                                    Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & ".*, not found. Check your RETURN clause.")
                                End Try
#End Region
                            Else
#Region "Standard Table.ColumnName"
                                Dim larReturnColumn = From Table In Me.Model.RDS.Table
                                                      From Column In Table.Column
                                                      Where Table.Name = lrReturnColumn.MODELELEMENTNAME
                                                      Where Column.Name = lrReturnColumn.COLUMNNAMESTR
                                                      Select Column

                                Try
                                    Dim lrColumn As RDS.Column = larReturnColumn.First.Clone(Nothing, Nothing)
                                    lrColumn.NodeModifierFunction = lrReturnColumn.GetNodeModifierFunction
                                    lrColumn.TemporaryAlias = lrReturnColumn.MODELELEMENTSUFFIX
                                    If lrReturnColumn.ASCLAUSE IsNot Nothing Then
                                        lrColumn.AsName = lrReturnColumn.ASCLAUSE.COLUMNNAMESTR
                                    End If

                                    larColumn.Add(lrColumn)
                                Catch ex As Exception
                                    Throw New Exception("Column, " & lrReturnColumn.MODELELEMENTNAME & "." & lrReturnColumn.COLUMNNAMESTR & ", not found. Check your RETURN clause.")
                                End Try
#End Region
                            End If
#End Region
                        ElseIf lrReturnColumn.KEYWDCOUNTSTAR IsNot Nothing Then
                            'COUNT(*) is added in GenerateSQL. See main processing.
                        ElseIf lrReturnColumn.COUNTCLAUSE IsNot Nothing Then
                            'The COUNT(DISTINCT clause is added in the generated query. See main processing.
                        ElseIf lrReturnColumn.RETURNFUNCTION IsNot Nothing Then
                            Dim lrFunctionColumn As New RDS.Column(New RDS.Table(Me.Model.RDS, "DummyTable", Nothing), "ReturnFunction", Nothing, Nothing, False, System.Guid.NewGuid.ToString)
                            lrFunctionColumn.ColumnType = pcenumRDSColumnType.ReturnFunctionColumn
                            Dim lrDerivationProcessor = New FEQL.Processor(Me.Model)
                            lrFunctionColumn.TemporaryData = lrDerivationProcessor.walkReturnFunctionTree(lrReturnColumn.RETURNFUNCTION)
                            lrFunctionColumn.AsName = lrReturnColumn.ASCLAUSE.COLUMNNAMESTR
                            lrFunctionColumn.NodeModifierFunction = lrReturnColumn.GetNodeModifierFunction
                            larColumn.Add(lrFunctionColumn)
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
#End Region
                ElseIf abIsStraightDerivationClause Then
#Region "Straight Derivation Clause"
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
#End Region
                Else
                    'Head Column/s
                    Dim larHeadColumn As New List(Of RDS.Column)

                    Select Case Me.HeadNode.FBMModelObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrVTColumn As RDS.Column
                            If Me.QueryEdges(0).FBMFactType.IsLinkFactType Then
                                lrVTColumn = (From Column In Me.QueryEdges(0).FBMFactType.LinkFactTypeRole.FactType.getCorrespondingRDSTable.Column
                                              Where Column.Role Is Me.QueryEdges(0).FBMFactType.LinkFactTypeRole
                                              Select Column).First
                            ElseIf Me.QueryEdges(0).IsPartialFactTypeMatch Then
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
                            lrVTColumn.NodeModifierFunction = Me.HeadNode.ModifierFunction
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
                                lrTempColumn.IsDistinct = Me.HeadNode.IsDistinct
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
                            Try
                                lrRole = lrQueryEdge.FBMFactTypeReading.PredicatePart(lrQueryEdge.FBMFactTypeReading.PredicatePart.IndexOf(lrQueryEdge.FBMPredicatePart) + 1).Role
                            Catch ex As Exception
                                Throw New Exception("QueryGraph: Looking for predicate, '" & lrQueryEdge.FBMPredicatePart.PredicatePartText & "', in FactTypeReading:" & lrQueryEdge.FBMFactTypeReading.Id)
                            End Try

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

                        Dim lrTempColumn As RDS.Column = Nothing
                        Select Case lrRole.JoinedORMObject.ConceptType 'lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                Dim lrColumn As RDS.Column
                                If lrQueryEdge.IsPartialFactTypeMatch Then
                                    lrColumn = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.Role Is lrRole)
                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                                ElseIf lrRole.FactType.IsLinkFactType Then
                                    lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.Id Is lrRole.FactType.LinkFactTypeRole.Id)
                                Else
                                    lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                                    If lrQueryEdge.FBMFactType.IsManyTo1BinaryFactType And lrQueryEdge.FBMFactType.IsDerived And Not lrQueryEdge.FBMFactType.IsObjectified Then
                                        lrTempColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, lrQueryEdge.FBMFactType.Id, lrQueryEdge.FBMFactType), Nothing)
                                    ElseIf lrQueryEdge.BaseNode.FBMModelObject.IsDerived Then
                                        lrTempColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, lrQueryEdge.BaseNode.FBMModelObject.Id, lrQueryEdge.BaseNode.FBMModelObject), Nothing)
                                    Else
                                        lrTempColumn = lrColumn.Clone(New RDS.Table(Me.Model.RDS, lrQueryEdge.BaseNode.FBMModelObject.Id, lrQueryEdge.BaseNode.FBMModelObject), Nothing)
                                    End If
                                End If
                                If lrTempColumn Is Nothing And lrQueryEdge.FBMFactType.IsLinkFactType Then
                                    lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role Is lrQueryEdge.FBMFactType.LinkFactTypeRole).Clone(Nothing, Nothing)
                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing,,, lrQueryEdge.BaseNode.ModifierFunction)
                                    lrTempColumn.TemporaryAlias = lrQueryEdge.Alias
                                ElseIf lrTempColumn Is Nothing And lrQueryEdge.FBMFactType.IsBinaryFactType And lrQueryEdge.FBMFactType.HasTotalRoleConstraint Then
                                    lrColumn = lrQueryEdge.FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.ActiveRole Is lrQueryEdge.FBMFactType.RoleGroup(liRoleInd))
                                    lrTempColumn = lrColumn.Clone(Nothing, Nothing,,, lrQueryEdge.TargetNode.ModifierFunction)
                                    lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                                Else
                                    lrTempColumn.TemporaryAlias = lrQueryEdge.BaseNode.Alias
                                    If lrRole.JoinedORMObject.Id = lrTempColumn.Table.Name Then
                                        lrTempColumn.NodeModifierFunction = lrQueryEdge.BaseNode.ModifierFunction
                                    Else
                                        lrTempColumn.NodeModifierFunction = lrQueryEdge.TargetNode.ModifierFunction
                                        lrTempColumn.IsDistinct = lrQueryEdge.TargetNode.IsDistinct
                                    End If
                                End If
                                lrTempColumn.GraphNodeType = lrQueryEdge.BaseNode.Name '20230128-VM-Was lrColumn
                                lrTempColumn.QueryEdge = lrQueryEdge '20230128-VM-Was lrColumn
                                lrTempColumn.TemporaryAlias = Viev.NullVal(lrTempColumn.TemporaryAlias, "")
                                larColumn.AddUnique(lrTempColumn) '20230128-VM-Was lrColumn
                            Case Else
                                Dim larEdgeColumn As List(Of RDS.Column)
                                If lrRole IsNot Nothing Then
                                    larEdgeColumn = lrRole.JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition).ToList
                                Else
                                    larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition).ToList
                                End If

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
                                    lrTempColumn.NodeModifierFunction = lrQueryEdge.TargetNode.ModifierFunction
                                    lrTempColumn.IsDistinct = lrQueryEdge.TargetNode.IsDistinct
                                    larColumn.AddUnique(lrTempColumn)
                                Next
                        End Select
                        lrRole = Nothing
                    Next

                End If

                Return larColumn.Distinct.ToList
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
