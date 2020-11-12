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

        ''' <summary>
        ''' The list of ProjectionColumns for the Query. Used to find and display Nodes/Links in QueryView in FactEngine form.
        ''' </summary>
        Public ProjectionColumn As New List(Of RDS.Column)

        Public Sub New(ByRef arFBMModel As FBM.Model)
            Me.Model = arFBMModel
        End Sub

        Public Function generateCREATESQL() As String

        End Function

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
                Call Me.setQueryEdgeAliases()

                lsSQLQuery = "SELECT "

#Region "ProjectionColums"
                liInd = 1
                Dim larProjectionColumn = Me.getProjectionColumns
                Me.ProjectionColumn = larProjectionColumn
                For Each lrProjectColumn In larProjectionColumn.FindAll(Function(x) x IsNot Nothing)

                    If lrProjectColumn.Role.FactType.IsDerived Then
                        lsSQLQuery &= "[" & lrProjectColumn.Role.FactType.Id & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "]." & lrProjectColumn.Name
                    Else
                        lsSQLQuery &= "[" & lrProjectColumn.Table.DatabaseName & Viev.NullVal(lrProjectColumn.TemporaryAlias, "") & "]." & lrProjectColumn.Name
                    End If
                    If liInd < larProjectionColumn.Count Then lsSQLQuery &= ","
                    liInd += 1
                Next

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

                lsSQLQuery &= vbCrLf & "FROM "

#Region "FromClause"
                liInd = 1
                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)
                For Each lrQueryNode In larFromNodes
                    If lrQueryNode.Alias Is Nothing Then
                        lsSQLQuery &= lrQueryNode.Name 'FBMModelObject.getCorrespondingRDSTable.Name
                    Else
                        'FBMModelObject.getCorrespondingRDSTable.Name
                        lsSQLQuery &= lrQueryNode.Name & " " & lrQueryNode.Name & Viev.NullVal(lrQueryNode.Alias, "")
                    End If

                    If liInd < larFromNodes.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                liInd = 1
                Dim larQuerEdgeWithFBMFactType = Me.QueryEdges.FindAll(Function(x) x.FBMFactType IsNot Nothing)
                Dim larRDSTableQueryEdge = larQuerEdgeWithFBMFactType.FindAll(Function(x) x.FBMFactType.isRDSTable And Not x.FBMFactType.IsDerived)

                'RDS Tables
                For Each lrQueryEdge In larRDSTableQueryEdge

                    If Me.Nodes.FindAll(Function(x) x.Name = lrQueryEdge.FBMFactType.Id).Count = 0 Then
                        lsSQLQuery &= vbCrLf & "," & lrQueryEdge.FBMFactType.Id
                        If lrQueryEdge.Alias IsNot Nothing Then
                            lsSQLQuery &= " " & lrQueryEdge.FBMFactType.Id & Viev.NullVal(lrQueryEdge.Alias, "")
                        End If
                    End If

                    'If liInd < larRDSTableQueryEdge.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                'Derived FactTypes
                Dim lrDerivationProcessor As New FEQL.Processor(prApplication.WorkingModel)
                For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) x.FBMFactType.IsDerived)

                    lsSQLQuery &= vbCrLf & ","

                    lsSQLQuery &= lrDerivationProcessor.processDerivationText(lrQueryEdge.FBMFactType.DerivationText, lrQueryEdge.FBMFactType)

                Next

#End Region

                'WHERE
                Dim larEdgesWithTargetNode = From QueryEdge In Me.QueryEdges
                                             Where QueryEdge.TargetNode IsNot Nothing
                                             Select QueryEdge

                'WhereEdges are Where joins, rather than ConditionalQueryEdges which test for values by identifiers.
                Dim larWhereEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) (x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType And
                                                                       x.BaseNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType) Or
                                                                       x.FBMFactType.isRDSTable
                                                                       )

                'Add special DerivedFactType WhereEdges
                larWhereEdges.AddRange(Me.QueryEdges.FindAll(Function(x) x.FBMFactType.DerivationType = pcenumFEQLDerivationType.Count))


                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = larEdgesWithTargetNode.ToList.FindAll(Function(x) x.IdentifierList.Count > 0 Or
                                                                             x.TargetNode.MathFunction <> pcenumMathFunction.None)

                'BooleanPredicate edges. E.g. Protein is enzyme
                larConditionalQueryEdges.AddRange(Me.QueryEdges.FindAll(Function(x) x.TargetNode Is Nothing))

                If larWhereEdges.Count = 0 And larConditionalQueryEdges.Count = 0 And (Not Me.HeadNode.HasIdentifier) Then
                    Return lsSQLQuery
                End If

                lsSQLQuery &= vbCrLf & "WHERE "

#Region "WhereClauses"
                liInd = 1
                Dim lbAddedAND = False
                Dim lbIntialWhere = Nothing

#Region "WhereJoins"
                For Each lrQueryEdge In larWhereEdges

                    If lbAddedAND Or liInd > 1 Then
                        lsSQLQuery &= "AND "
                        lbAddedAND = True
                    End If
                    lbAddedAND = False

                    Dim lrOriginTable As RDS.Table

                    If lrQueryEdge.WhichClauseType = pcenumWhichClauseType.AndThatIdentityCompatitor Then
                        'E.g. Of the type "Person 1 IS NOT Person 2" or "Person 1 IS Person 2"

                        lsSQLQuery &= "("
                        For Each lrColumn In lrQueryEdge.BaseNode.RDSTable.getPrimaryKeyColumns
                            lsSQLQuery &= lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name
                            If lrQueryEdge.WhichClauseSubType = pcenumWhichClauseType.ISClause Then
                                lsSQLQuery &= " = "
                            Else
                                lsSQLQuery &= " <> "
                            End If
                            lsSQLQuery &= lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrColumn.Name
                        Next
                        lsSQLQuery &= ")"

                    ElseIf lrQueryEdge.FBMFactType.isRDSTable And lrQueryEdge.FBMFactType.Arity = 2 Then

                        lrOriginTable = lrQueryEdge.FBMFactType.getCorrespondingRDSTable

                        Dim larRelation = lrOriginTable.getRelations

                        larRelation = larRelation.FindAll(Function(x) (x.DestinationTable.Name = lrQueryEdge.BaseNode.Name) Or
                                                                       (x.DestinationTable.Name = lrQueryEdge.TargetNode.Name))
                        Dim liTempInd = 0
                        Dim liRelationCounter = 1
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
                                                lsSQLQuery &= lrRelation.OriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= lrRelation.DestinationTable.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrTargetColumn.Name & vbCrLf
                                        End Select
                                    Case Else
                                        Select Case lrQueryEdge.TargetNode.FBMModelObject.ConceptType
                                            Case = pcenumConceptType.ValueType
                                                'Nothing to do here
                                            Case Else
                                                lsSQLQuery &= lrRelation.OriginTable.Name & Viev.NullVal(lrQueryEdge.Alias, "") & "." & lrRelation.OriginColumns(liColumnCounter).Name & " = "
                                                Dim lrTargetColumn = lrRelation.DestinationColumns.Find(Function(x) x.ActiveRole Is lrRelation.OriginColumns(liColumnCounter).ActiveRole)
                                                lsSQLQuery &= lrRelation.DestinationTable.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrTargetColumn.Name & vbCrLf
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

                        lsSQLQuery &= lrBaseNode.Name & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name & " = " & lrTargetNode.Name & "." & lrBaseNode.RDSTable.getPrimaryKeyColumns.First.Name

                    Else
                        Dim lrBaseNode, lrTargetNode As FactEngine.QueryNode
                        If lrQueryEdge.IsReciprocal Then
                            lrBaseNode = lrQueryEdge.TargetNode
                            lrTargetNode = lrQueryEdge.BaseNode
                        Else
                            lrBaseNode = lrQueryEdge.BaseNode
                            lrTargetNode = lrQueryEdge.TargetNode
                        End If

                        lrOriginTable = lrBaseNode.FBMModelObject.getCorrespondingRDSTable
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
                                lsSQLQuery &= lrColumn.Table.Name & Viev.NullVal(lrBaseNode.Alias, "") & "." & lrColumn.Name
                                lsSQLQuery &= " = " & larTargetColumn(liInd2 - 1).Table.Name & Viev.NullVal(lrTargetNode.Alias, "") & "." & larTargetColumn(liInd2 - 1).Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        Else
                            Dim larTargetColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                            For Each lrColumn In larTargetColumn
                                Dim lrOriginColumn = lrRelation.OriginColumns.Find(Function(x) x.ActiveRole Is lrColumn.ActiveRole)
                                lsSQLQuery &= lrQueryEdge.TargetNode.Name & Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") & "." & lrOriginColumn.Name
                                lsSQLQuery &= " = " & lrQueryEdge.BaseNode.Name & "." & lrColumn.Name
                                If liInd2 < larTargetColumn.Count Then lsSQLQuery &= vbCrLf & "AND "
                                liInd2 += 1
                            Next
                        End If
                    End If
                    If Not lsSQLQuery.EndsWith(vbCrLf) Then lsSQLQuery &= vbCrLf
                    'If liInd < Me.QueryEdges.Count Then
                    '    lbAddedAND = True
                    'End If

                    liInd += 1
                    lbIntialWhere = Nothing
                Next

#End Region
                If (Not lbAddedAND And lbIntialWhere Is Nothing And
                    larConditionalQueryEdges.Count > 0) Or
                    (Me.HeadNode.HasIdentifier And larWhereEdges.Count > 0) Then
                    lsSQLQuery &= "AND "
                    lbIntialWhere = Nothing
                End If

#Region "WhereConditionals"
                If Me.HeadNode.HasIdentifier Then
                    Dim lrTargetTable = Me.HeadNode.RDSTable
                    liInd = 0
                    For Each lrColumn In Me.HeadNode.RDSTable.getFirstUniquenessConstraintColumns
                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & "[" & lrTargetTable.Name & Viev.NullVal(Me.HeadNode.Alias, "") & "]." & lrColumn.Name & " = '" & Me.HeadNode.IdentifierList(liInd) & "'" & vbCrLf
                        If liInd < Me.HeadNode.IdentifierList.Count - 1 Then lsSQLQuery &= "AND "
                        liInd += 1
                    Next
                    lbIntialWhere = "AND "
                End If


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

                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.Name & Viev.NullVal(lrQueryEdge.BaseNode.Alias, "") & "." & lrColumn.Name & " = '" & lrQueryEdge.IdentifierList(0) & "'" & vbCrLf
                            lbIntialWhere = "AND "
                        Case Else

                            Select Case lrQueryEdge.WhichClauseType
                                Case Is = pcenumWhichClauseType.BooleanPredicate

                                    lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrQueryEdge.BaseNode.Name & "."

                                    Dim lrTargetTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable
                                    Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)

                                    lsSQLQuery &= lrTargetColumn.Name & " = True"

                                Case Else



                                    If lrQueryEdge.TargetNode.MathFunction <> pcenumMathFunction.None Then

                                        'Math function
                                        Dim lrTargetTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable
                                        Dim lrTargetColumn = lrTargetTable.Column.Find(Function(x) x.FactType Is lrQueryEdge.FBMFactType)
                                        lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") &
                                              lrTargetTable.Name &
                                              Viev.NullVal(lrQueryEdge.TargetNode.Alias, "") &
                                              "." &
                                              lrTargetColumn.Name

                                        lsSQLQuery &= " " & Viev.GetEnumDescription(lrQueryEdge.TargetNode.MathFunction)
                                        lsSQLQuery &= " " & lrQueryEdge.TargetNode.MathNumber.ToString


                                        lbIntialWhere = "AND "
                                    Else
                                        Dim lrTargetTable As RDS.Table = Nothing
                                        Dim lsAlias As String = ""

                                        'Check for reciprocal reading. As in WHICH Person was armed by (Person 2:'David') rather than WHICH Person armed (Person 2:'Saul')
                                        If lrQueryEdge.TargetNode.HasIdentifier Then
                                            lrTargetTable = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable
                                            lsAlias = Viev.NullVal(lrQueryEdge.TargetNode.Alias, "")
                                        Else
                                            lrTargetTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable
                                            lsAlias = Viev.NullVal(lrQueryEdge.BaseNode.Alias, "")
                                        End If

                                        Dim larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns

                                        liInd = 0
                                        For Each lsIdentifier In lrQueryEdge.IdentifierList
                                            lsSQLQuery &= Viev.NullVal(lbIntialWhere, "") & lrTargetTable.Name & lsAlias & "." & larIndexColumns(liInd).Name & " = '" & lsIdentifier & "'" & vbCrLf
                                            If liInd < lrQueryEdge.IdentifierList.Count - 1 Then lsSQLQuery &= "AND "
                                            liInd += 1
                                        Next
                                        lbIntialWhere = "AND "
                                    End If
                            End Select
                    End Select

                Next
#End Region
#End Region

                Return lsSQLQuery
            Catch ex As Exception
                Throw New Exception(ex.Message & vbCrLf & vbCrLf & lsSQLQuery)
            End Try


        End Function

        Public Function getProjectQueryEdges() As List(Of FactEngine.QueryEdge)

            Return Me.QueryEdges.FindAll(Function(x) x.IsProjectColumn).ToList

        End Function

        Public Function getProjectionColumns() As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)

            Try



                'Head Column/s
                Dim larHeadColumn As New List(Of RDS.Column)
                Select Case Me.HeadNode.FBMModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Dim lrVTColumn = (From Column In Me.QueryEdges(0).TargetNode.FBMModelObject.getCorrespondingRDSTable.Column
                                          Where Column.Role Is Me.QueryEdges(0).FBMFactType.RoleGroup(0)
                                          Where Column.ActiveRole Is Me.QueryEdges(0).FBMFactType.RoleGroup(1)
                                          Select Column).First

                        lrVTColumn = lrVTColumn.Clone(Nothing, Nothing)
                        lrVTColumn.TemporaryAlias = Viev.NullVal(Me.HeadNode.QueryEdgeAlias, "")
                        lrVTColumn.GraphNodeType = Me.HeadNode.Name
                        larHeadColumn.Add(lrVTColumn)

                    Case Else
                        Dim lrTempColumn As RDS.Column = Nothing
                        For Each lrColumn In Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition)
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
                For Each lrQueryEdge In Me.getProjectQueryEdges()
                    If lrQueryEdge.BaseNode.FBMModelObject.Id = lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id Then
                        liRoleInd = 1 'Other side of a BinaryFactType
                    Else
                        liRoleInd = 0 'First Role of a BinaryFactType
                    End If
                    Select Case lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType)
                            If lrColumn Is Nothing And lrQueryEdge.FBMFactType.IsLinkFactType Then
                                lrColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType.LinkFactTypeRole.FactType).Clone(Nothing, Nothing)
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
                            Dim larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns.OrderBy(Function(x) x.OrdinalPosition)
                            Dim lrTempColumn As RDS.Column
                            For Each lrColumn In larEdgeColumn
                                lrTempColumn = lrColumn.Clone(Nothing, Nothing)
                                If liRoleInd = 1 Then
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

                Next

                Return larColumn
            Catch ex As Exception
                Throw New Exception("Error in QueryGraph.getProjectionColumns:" & vbCrLf & vbCrLf & ex.StackTrace.ToString)
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
                                Dim lrTemplQueryEdge = Me.QueryEdges.Find(Function(x) x.Id = larRDSTableQueryEdge(liInd2).Id)
                                lrTemplQueryEdge.Alias = (liInd + 1).ToString
                            End If
                        End If
                    Next
                End If
            Next

        End Sub

    End Class

End Namespace
