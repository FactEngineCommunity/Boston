Namespace FactEngine
    Public Class QueryGraph

        Public Model As FBM.Model

        '============================================
        'Example Fact Engine Query
        '--------------------------
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

        Public Sub New(ByRef arFBMModel As FBM.Model)
            Me.Model = arFBMModel
        End Sub

        ''' <summary>
        ''' Generates SQL to run against the database for this QueryGraph
        ''' </summary>
        ''' <returns></returns>
        Public Function generateSQL() As String

            Dim lsSQLQuery As String = ""
            Dim liInd As Integer
            Dim larColumn As New List(Of RDS.Column)

            Try
                'Set the Node Aliases. E.g. If Lecturer occurs twice in the FROM clause, then Lecturer, Lecturer2 etc
                Call Me.setNodeAliases()

                lsSQLQuery = "SELECT "

#Region "ProjectionColums"
                liInd = 1
                Dim larProjectionColumn = Me.getProjectionColumns
                For Each lrProjectColumn In larProjectionColumn.FindAll(Function(x) x IsNot Nothing)
                    lsSQLQuery &= lrProjectColumn.Table.Name & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "." & lrProjectColumn.Name
                    If liInd < larProjectionColumn.Count Then lsSQLQuery &= ","
                    liInd += 1
                Next

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

                lsSQLQuery &= vbCrLf & "FROM "

#Region "FromClause"
                liInd = 1
                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)
                For Each lrQueryNode In larFromNodes
                    If lrQueryNode.Alias Is Nothing Then
                        lsSQLQuery &= lrQueryNode.FBMModelObject.getCorrespondingRDSTable.Name
                    Else
                        lsSQLQuery &= lrQueryNode.FBMModelObject.getCorrespondingRDSTable.Name & " " & lrQueryNode.FBMModelObject.getCorrespondingRDSTable.Name & Viev.NullVal(lrQueryNode.Alias, "")
                    End If

                    If liInd < larFromNodes.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                liInd = 1
                Dim larQuerEdgeWithFBMFactType = Me.QueryEdges.FindAll(Function(x) x.FBMFactType IsNot Nothing)
                Dim larRDSTableQueryEdge = larQuerEdgeWithFBMFactType.FindAll(Function(x) x.FBMFactType.isRDSTable)

                For Each lrQueryEdge In larRDSTableQueryEdge

                    If Me.Nodes.FindAll(Function(x) x.Name = lrQueryEdge.FBMFactType.Id).Count = 0 Then
                        lsSQLQuery &= vbCrLf & "," & lrQueryEdge.FBMFactType.Id & Viev.NullVal(lrQueryEdge.Alias, "")
                    End If

                    'If liInd < larRDSTableQueryEdge.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

#End Region

                'WHERE
                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = Me.QueryEdges.FindAll(Function(x) x.IdentifierList.Count > 0)

                Dim larWhereEdges = Me.QueryEdges.FindAll(Function(x) x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                                                          x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)

                If larWhereEdges.Count = 0 And larConditionalQueryEdges.Count = 0 Then
                    Return lsSQLQuery
                End If

                lsSQLQuery &= vbCrLf & "WHERE "

#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = ""

#Region "WhereJoins"
                For Each lrQueryEdge In larWhereEdges

                    If lbAddedAND Or liInd > 1 Then
                        lsSQLQuery &= "AND "
                        lbAddedAND = True
                    End If
                    lbAddedAND = False

                    Dim lrOriginTable As RDS.Table

                    If lrQueryEdge.WhichClauseType = pcenumWhichClauseType.ISNOTClause Then
                        'E.g. Of the type "Person 1 IS NOT Person 2'

                        lsSQLQuery &= "("
                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " <> "
                            lsSQLQuery &= lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                        Next
                        lsSQLQuery &= ")"

                    ElseIf lrQueryEdge.FBMFactType.isRDSTable And lrQueryEdge.FBMFactType.Arity = 2 Then

                        lrOriginTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = "
                            lsSQLQuery &= lrOriginTable.Name & "." & lrOriginTable.getColumnByOrdingalPosition(1).Name
                        Next

                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= vbCrLf & "AND " & lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name & " = "
                            lsSQLQuery &= lrOriginTable.Name & "." & lrOriginTable.getColumnByOrdingalPosition(2).Name
                        Next

                    Else
                        lrOriginTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable
                        Dim larModelObject = New List(Of FBM.ModelObject)
                        larModelObject.Add(lrQueryEdge.BaseNode.FBMModelObject)
                        larModelObject.Add(lrQueryEdge.TargetNode.FBMModelObject)
                        Dim lrRelation = lrOriginTable.getRelationByFBMModelObjects(larModelObject)

                        Dim liInd2 = 1
                        If lrRelation.OriginTable Is lrOriginTable Then
                            Dim larTargetColumn = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                lsSQLQuery &= lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                                lsSQLQuery &= " = " & lrQueryEdge.TargetNode.FBMModelObject.Id & "." & lrColumn.Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                lsSQLQuery &= lrQueryEdge.TargetNode.FBMModelObject.Id & "." & lrColumn.Name
                                lsSQLQuery &= " = " & lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        End If
                    End If
                    lsSQLQuery &= vbCrLf
                    'If liInd < Me.QueryEdges.Count Then
                    '    lbAddedAND = True
                    'End If

                    liInd += 1
                    lbIntialWhere = Nothing
                Next

#End Region
                If Not lbAddedAND And lbIntialWhere Is Nothing And larConditionalQueryEdges.Count > 0 Then lsSQLQuery &= " AND "

#Region "WhereConditionals"
                For Each lrQueryEdge In larConditionalQueryEdges
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
                                                    Where FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id _
                                                       Or x.JoinedORMObject.Id = lrQueryEdge.TargetNode.FBMModelObject.Id).Count = 2
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
                                                                 Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id
                                                                 Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = lrQueryEdge.TargetNode.FBMModelObject.Id
                                                                 Select FactTypeReading
                                        If larFactTypeReading.Count > 0 Then
                                            lrFactType = larFactTypeReading.First.FactType
                                        End If
                                    End If
                                End If
                            Else
                                lrPredicatePart = larPredicatePart.First 'For now...need to consider PreboundReadingText/s
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
                                Else
                                    lrResponsibleRole = lrPredicatePart.Role
                                End If

                            End If

                            Dim lrTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable

                            Dim lrColumn = (From Column In lrTable.Column
                                            Where Column.Role Is lrResponsibleRole
                                            Where Column.ActiveRole.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject
                                            Select Column).First

                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name & " = '" & lrQueryEdge.IdentifierList(0) & "'" & vbCrLf
                            lbIntialWhere = "AND "
                        Case Else
                            Dim lrTargetTable = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable
                            Dim larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns
                            liInd = 0
                            For Each lsIdentifier In lrQueryEdge.IdentifierList
                                lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.Name & "." & larIndexColumns(liInd).Name & " = '" & lsIdentifier & "'" & vbCrLf
                                If liInd < lrQueryEdge.IdentifierList.Count - 1 Then lsSQLQuery &= "AND "
                                liInd += 1
                            Next
                            lbIntialWhere = "AND "
                    End Select

                Next
#End Region
#End Region

                Return lsSQLQuery
            Catch ex As Exception
                Return lsSQLQuery
            End Try


        End Function

        Public Function getProjectQueryEdges() As List(Of FactEngine.QueryEdge)

            Return Me.QueryEdges.FindAll(Function(x) x.IsProjectColumn).ToList

        End Function

        Public Function getProjectionColumns() As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)

            'Head Column/s
            Dim larHeadColumn As New List(Of RDS.Column)
            Select Case Me.HeadNode.FBMModelObject.ConceptType
                Case Is = pcenumConceptType.ValueType
                    Dim larVTColumn = (From Column In Me.QueryEdges(0).TargetNode.FBMModelObject.getCorrespondingRDSTable.Column
                                       Where Column.Role Is Me.QueryEdges(0).FBMFactType.RoleGroup(0)
                                       Where Column.ActiveRole Is Me.QueryEdges(0).FBMFactType.RoleGroup(1)
                                       Select Column).First

                    larVTColumn = larVTColumn.Clone(Nothing, Nothing)
                    larVTColumn.TemporaryAlias = Viev.NullVal(Me.HeadNode.QueryEdgeAlias, "")
                    larHeadColumn.Add(larVTColumn)

                Case Else
                    Dim lrTempColumn As RDS.Column = Nothing
                    For Each lrColumn In Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                        lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                        lrTempColumn.TemporaryAlias = Viev.NullVal(Me.HeadNode.Alias, "")
                        larHeadColumn.Add(lrTempColumn)
                    Next
            End Select

            larColumn.AddRange(larHeadColumn.ToList)

            Dim liRoleInd As Integer
            'Edge Column/s
            For Each lrQueryEdge In Me.getProjectQueryEdges()
                If lrQueryEdge.BaseNode.FBMModelObject.Id = lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                    liRoleInd = 1
                Else
                    liRoleInd = 0
                End If
                Select Case lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Dim lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                        If lrColumn Is Nothing And lrQueryEdge.FBMFactType.IsLinkFactType Then
                            lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType.LinkFactTypeRole.FactType).Clone(Nothing, Nothing)
                            lrColumn.TemporaryAlias = lrQueryEdge.Alias
                        End If
                        larColumn.AddUnique(lrColumn)
                    Case Else
                        Dim larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                        Dim lrTempColumn As RDS.Column
                        For Each lrColumn In larEdgeColumn
                            lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                            If liRoleInd = 1 Then
                                lrTempColumn.TemporaryAlias = lrQueryEdge.TargetNode.Alias
                            Else
                                lrTempColumn.TemporaryAlias = lrQueryEdge.Alias
                            End If
                            larColumn.Add(lrTempColumn)
                        Next

                End Select

            Next

            Return larColumn

        End Function

        Private Sub setNodeAliases()

            Dim liInd As Integer = 0

            For liInd = 0 To Me.Nodes.Count - 1
                If liInd > 0 Then
                    For liInd2 = 0 To liInd - 1
                        If Me.Nodes(liInd).Name = Me.Nodes(liInd2).Name Then
                            If Me.Nodes(liInd).Alias Is Nothing And
                               Me.Nodes(liInd2).Alias IsNot Nothing Then
                                Me.Nodes(liInd2).Alias = liInd.ToString
                                If Me.Nodes(liInd2).QueryEdge IsNot Nothing Then
                                    Me.Nodes(liInd2).QueryEdge.Alias = Me.Nodes(liInd2).Alias
                                End If
                            End If

                        End If
                    Next
                End If
            Next

        End Sub

    End Class

End Namespace
