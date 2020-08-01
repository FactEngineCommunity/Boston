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
            Try
                lsSQLQuery = "SELECT "
                'Dim larColumn = Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                Dim larColumn = Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                Dim liInd = 1
                For Each lrColumn In larColumn
                    lsSQLQuery &= lrColumn.Table.Name & "." & lrColumn.Name
                    If liInd < larColumn.Count Then lsSQLQuery &= ","
                    liInd += 1
                Next

                If Me.getProjectQueryEdges.Count > 0 Then lsSQLQuery &= ","

                liInd = 1
                Dim larProjectQueryEdge = Me.getProjectQueryEdges()
                For Each lrQueryEdge In larProjectQueryEdge
                    'larColumn = lrQueryEdge.FBMFactType.RoleGroup(1).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                    Dim liRoleInd As Integer
                    If lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id Then
                        liRoleInd = 1
                    Else
                        liRoleInd = 0
                    End If
                    larColumn = New List(Of RDS.Column)
                    If lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                        larColumn.Add(lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType))
                    Else
                        larColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                    End If

                    Dim liInd2 = 1
                    For Each lrColumn In larColumn
                        If lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                            lsSQLQuery &= lrColumn.Table.Name & "." _
                                      & lrColumn.Name
                        Else
                            lsSQLQuery &= lrColumn.Table.Name & "." _ 'lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.Name & "." _
                                      & lrColumn.Name
                        End If

                        If liInd2 < larColumn.Count Then lsSQLQuery &= ","
                        liInd2 += 1
                    Next

                    If liInd < Me.getProjectQueryEdges.Count Then lsSQLQuery &= ","
                    liInd += 1
                Next

                lsSQLQuery &= vbCrLf & "FROM "

                liInd = 1
                Dim larFromNodes = Me.Nodes.FindAll(Function(x) x.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)
                For Each lrQueryNode In larFromNodes
                    lsSQLQuery &= lrQueryNode.FBMModelObject.getCorrespondingRDSTable.Name

                    If liInd < larFromNodes.Count Then lsSQLQuery &= "," & vbCrLf
                    liInd += 1
                Next

                lsSQLQuery &= vbCrLf & "WHERE "

                liInd = 1
                Dim lbIntialWhere = ""
                For Each lrQueryEdge In Me.QueryEdges.FindAll(Function(x) x.TargetNode.FBMModelObject.ConceptType <> pcenumConceptType.ValueType)

                    Dim lrOriginTable = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable
                    Dim larModelObject = New List(Of FBM.ModelObject)
                    larModelObject.Add(lrQueryEdge.BaseNode.FBMModelObject)
                    larModelObject.Add(lrQueryEdge.TargetNode.FBMModelObject)
                    Dim lrRelation = lrOriginTable.getRelationByFBMModelObjects(larModelObject)

                    If lrRelation.OriginTable Is lrOriginTable Then
                        Dim larTargetColumn = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                        For Each lrColumn In larTargetColumn
                            lsSQLQuery &= lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                            lsSQLQuery &= " = " & lrQueryEdge.TargetNode.FBMModelObject.Id & "." & lrColumn.Name
                        Next
                    Else
                        Dim larTargetColumn = lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                        For Each lrColumn In larTargetColumn
                            lsSQLQuery &= lrQueryEdge.TargetNode.FBMModelObject.Id & "." & lrColumn.Name
                            lsSQLQuery &= " = " & lrQueryEdge.BaseNode.FBMModelObject.Id & "." & lrColumn.Name
                        Next
                    End If
                    lsSQLQuery &= vbCrLf
                    If liInd < Me.QueryEdges.Count Then lsSQLQuery &= "AND "
                    liInd += 1
                    lbIntialWhere = Nothing
                Next

                'WHERE Conditional
                Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
                larConditionalQueryEdges = Me.QueryEdges.FindAll(Function(x) x.IdentifierList.Count > 0)

                For Each lrQueryEdge In larConditionalQueryEdges
                    Select Case lrQueryEdge.WhichClauseType
                        Case Is = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                            Dim lrFactType = CType(lrQueryEdge.BaseNode.FBMModelObject, FBM.FactType)

                            Dim larPredicatePart = From FactTypeReading In lrFactType.FactTypeReading
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                                   Select PredicatePart

                            Dim lrPredicatePart As FBM.PredicatePart
                            If larPredicatePart.Count = 0 Then

                                larPredicatePart = From FactType In Me.Model.FactType
                                                   From FactTypeReading In FactType.FactTypeReading
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id _
                                                  Or x.JoinedORMObject.Id = lrQueryEdge.TargetNode.FBMModelObject.Id).Count = 2
                                                   Where PredicatePart.PredicatePartText = lrQueryEdge.Predicate
                                                   Select PredicatePart


                                lrPredicatePart = larPredicatePart.First
                                lrFactType = lrPredicatePart.FactTypeReading.FactType
                            Else
                                lrPredicatePart = larPredicatePart.First
                            End If

                            Dim lrResponsibleRole As FBM.Role

                            If Not lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject Then

                                'lrQueryEdge.Predicate = "is " & lrQueryEdge.Predicate

                                lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                                   From PredicatePart In FactTypeReading.PredicatePart
                                                   Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                                   Select PredicatePart).First
                            End If

                            If lrPredicatePart Is Nothing Then
                                Throw New Exception("There is no Predicate (Part) of Fact Type, '" & lrQueryEdge.FBMFactType.Id & "', that is '" & lrQueryEdge.Predicate & "'.")
                            Else
                                lrResponsibleRole = lrPredicatePart.Role
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
                                liInd += 1
                            Next
                            lbIntialWhere = "AND "
                    End Select

                Next

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
            Dim larHeadColumn = Me.HeadNode.FBMModelObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
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
                        larColumn.Add(lrQueryEdge.BaseNode.FBMModelObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.FactType Is lrQueryEdge.FBMFactType))
                    Case Else
                        Dim larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                        larColumn.AddRange(larEdgeColumn.ToList)
                End Select

            Next

            Return larColumn

        End Function

    End Class

End Namespace
