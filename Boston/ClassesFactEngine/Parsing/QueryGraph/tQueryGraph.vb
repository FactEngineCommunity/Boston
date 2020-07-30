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
            For Each lrQueryEdge In Me.getProjectQueryEdges()
                'larColumn = lrQueryEdge.FBMFactType.RoleGroup(1).JoinedORMObject.getCorrespondingRDSTable.getPrimaryKeyColumns
                Dim liRoleInd As Integer
                If lrQueryEdge.FBMFactType.RoleGroup(0).JoinedORMObject.Id = lrQueryEdge.BaseNode.FBMModelObject.Id Then
                    liRoleInd = 1
                Else
                    liRoleInd = 0
                End If
                larColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                Dim liInd2 = 1
                For Each lrColumn In larColumn
                    lsSQLQuery &= lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.Name & "." _
                                  & lrColumn.Name
                    If liInd2 < larColumn.Count Then lsSQLQuery &= ","
                    liInd2 += 1
                Next

                If liInd < Me.getProjectQueryEdges.Count Then lsSQLQuery &= ","
                liInd += 1
            Next

            lsSQLQuery &= vbCrLf & "FROM "

            liInd = 1
            For Each lrQueryNode In Me.Nodes
                lsSQLQuery &= lrQueryNode.FBMModelObject.getCorrespondingRDSTable.Name
                If liInd < Me.Nodes.Count Then lsSQLQuery &= "," & vbCrLf
                liInd += 1
            Next

            lsSQLQuery &= vbCrLf & "WHERE "

            liInd = 1
            Dim lbIntialWhere = Nothing
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
                lbIntialWhere = "AND "
            Next

            'WHERE Conditional
            Dim larConditionalQueryEdges As New List(Of FactEngine.QueryEdge)
            larConditionalQueryEdges = Me.QueryEdges.FindAll(Function(x) x.IdentifierList.Count > 0)

            For Each lrQueryEdge In larConditionalQueryEdges
                Select Case lrQueryEdge.WhichClauseType
                    Case Is = FactEngine.Constants.pcenumWhichClauseType.IsPredicateNodePropertyIdentification
                        Dim lrFactType = CType(lrQueryEdge.BaseNode.FBMModelObject, FBM.FactType)

                        Dim lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                               From PredicatePart In FactTypeReading.PredicatePart
                                               Where PredicatePart.PredicatePartText = Trim(lrQueryEdge.Predicate)
                                               Select PredicatePart).First

                        Dim lrResponsibleRole As FBM.Role

                        If Not lrPredicatePart.Role.JoinedORMObject Is lrQueryEdge.TargetNode.FBMModelObject Then
                            lrQueryEdge.Predicate = "is " & lrQueryEdge.Predicate

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

                    Case Else
                        Dim lrTargetTable = lrQueryEdge.TargetNode.FBMModelObject.getCorrespondingRDSTable
                        Dim larIndexColumns = lrTargetTable.getFirstUniquenessConstraintColumns
                        liInd = 0
                        For Each lsIdentifier In lrQueryEdge.IdentifierList
                            lsSQLQuery &= "AND " & lrTargetTable.Name & "." & larIndexColumns(liInd).Name & " = '" & lsIdentifier & "'" & vbCrLf
                            liInd += 1
                        Next
                End Select

            Next

            Return lsSQLQuery

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
                Dim larEdgeColumn = lrQueryEdge.FBMFactType.RoleGroup(liRoleInd).JoinedORMObject.getCorrespondingRDSTable.getFirstUniquenessConstraintColumns
                larColumn.AddRange(larEdgeColumn.ToList)
            Next

            Return larColumn

        End Function

    End Class

End Namespace
