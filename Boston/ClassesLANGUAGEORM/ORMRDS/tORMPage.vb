Imports System.Reflection

Namespace FBM

    Partial Public Class Page

        Public WithEvents RDSModel As New RDS.Model

        Public Sub displayPGSRelationNodeLink(ByRef arOriginatingNode As PGS.Node, ByVal asRelationId As String)

            Dim lsSQLQuery As String
            Dim lrRecordset, lrRecordset1 As ORMQL.Recordset
            Dim lrFactInstance As New FBM.FactInstance

            Try
                Dim lsRelationId = asRelationId

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                lsSQLQuery &= " WHERE Entity = '" & arOriginatingNode.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.Facts.Count > 2 Then
                    'Ignore for now. Won't happen that much. Most relations of this type are binary.
                    'For ORM/PGS binary-manyToMany relations, the ORM relationship is actually a PGS relation, rather than a PGS Node.
                    'For ORM FactTypes that have 3 or more Roles, within the PGS diagram, the FT is a Node.
                    'NB See DiagramView.MouseDown for how the Predicates are shown for the Link of a Binary PGSRelationNode-come-Relation.
                    '  Must get the Predicates from the ResponsibleFactType rather than the actual Relations which are likely on/from the LinkFactTypes.
                Else
                    Dim liInd As Integer = 1
                    Dim lrNode1 As PGS.Node = Nothing
                    Dim lrNode2 As PGS.Node = Nothing

                    While Not lrRecordset.EOF
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If liInd = 1 Then
                            lrNode2 = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset1("Entity").Data)
                        Else
                            lrNode1 = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset1("Entity").Data)
                        End If

                        liInd += 1
                        lrRecordset.MoveNext()
                    End While

                    Dim lrRelation As New ERD.Relation(Me.Model,
                                                                Me,
                                                                asRelationId,
                                                                lrNode1,
                                                                pcenumCMMLMultiplicity.One,
                                                                False,
                                                                False,
                                                                lrNode2,
                                                                pcenumCMMLMultiplicity.One,
                                                                False,
                                                                arOriginatingNode.RDSTable)

                    lrRelation.IsPGSRelationNode = True
                    Dim lrOriginatingNode = arOriginatingNode
                    lrRelation.ActualPGSNode = Me.ERDiagram.Entity.Find(Function(x) x.Id = lrOriginatingNode.Id)
                    lrRelation.ActualPGSNode.PGSRelation = lrRelation

                    'NB Even though the RDSRelation is stored against the Link (below), the Predicates for the Link come from the ResponsibleFactType.
                    '  because the relation is actually a PGSRelationNode.
                    Dim lrRDSRelation As RDS.Relation = Me.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)
                    lrRelation.RelationFactType = lrRDSRelation.ResponsibleFactType

                    If lrRDSRelation.ResponsibleFactType.FactTypeReading.Count = 1 Then
                        Dim lrFactTypeReading = lrRDSRelation.ResponsibleFactType.FactTypeReading(0)
                        If Not lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = lrNode1.Name Then
                            'Swap the Origin and Desination nodes, for directed Graphs. i.e. The single FactTypeReading determines the direction.
                            Dim lrTempNode = lrNode1
                            lrNode1 = lrRelation.DestinationEntity
                            lrNode2 = lrTempNode
                        End If
                    End If

                    Dim lrLink As PGS.Link

                    lrLink = New PGS.Link(Me, lrFactInstance, lrNode1, lrNode2, Nothing, Nothing, lrRelation)
                    lrLink.RDSRelation = lrRDSRelation

                    lrLink.DisplayAndAssociate()

                    lrLink.Link.Text = lrRelation.ActualPGSNode.Id
                    lrLink.Relation.Link = lrLink
                    Me.ERDiagram.Relation.AddUnique(lrRelation)

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Function loadPGSNode(ByRef asTable As RDS.Table, Optional abDisplayAndAssociate As Boolean = True) As PGS.Node

            Try
                If Not Me.Language = pcenumLanguage.PropertyGraphSchema Then Throw New Exception("Only call this function for PGS Pages.")

                '------------------
                'Load the Node.
                '==================================================================================================================
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactInstance As FBM.FactInstance

                Dim lrNode As PGS.Node

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & asTable.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrNode = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).ClonePGSNode(Me)
                '===================================================================================================================
                lrNode.RDSTable = asTable 'IMPORTANT: Leave this at this point in the code.


                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                'lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"
                lsSQLQuery &= " WHERE IsPGSRelation = '" & lrNode.Name & "'"

                Dim lrRecordsetIsPGSRelation As ORMQL.Recordset
                lrRecordsetIsPGSRelation = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordsetIsPGSRelation("Count").Data) > 0 Then
                    lrNode.NodeType = pcenumPGSEntityType.Relationship
                End If

                Me.ERDiagram.Entity.AddUnique(lrNode)

                Dim loPointF As New PointF(0, 0)
                If abDisplayAndAssociate Then Call Me.DropExistingPGSNodeAtPoint(lrNode, loPointF)

                If abDisplayAndAssociate Then Call Me.loadRelationsForPGSNode(lrNode)
                If abDisplayAndAssociate Then Call Me.loadPropertyRelationsForPGSNode(lrNode)

                Return lrNode

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New PGS.Node
            End Try

        End Function

        Private Sub RDSModel_RelationAdded(ByRef arRelation As RDS.Relation) Handles RDSModel.RelationAdded

            Try
                Dim lrRelation As RDS.Relation = arRelation
                Dim lrOriginEntity, lrDestinationEntity As FBM.FactDataInstance
                'CodeSafe 
                If Me.IsCoreModelPage Then Exit Sub

                If Me.FactTypeInstance.Find(Function(x) x.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString) Is Nothing Then
                    Exit Sub
                End If

                If Me.isCMMLTableOnPage(lrRelation.OriginTable.Name) _
                    And Me.isCMMLTableOnPage(lrRelation.DestinationTable.Name) Then

                    Call Me.loadCMMLRelation(arRelation) 'Puts the relation on the Page
                End If

                lrOriginEntity = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRelation.OriginTable.Name)
                lrDestinationEntity = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRelation.DestinationTable.Name)



                If (lrOriginEntity IsNot Nothing) And (lrDestinationEntity IsNot Nothing) Then
                    'For when the Page is displayed

                    Dim lrERDRelation As New ERD.Relation(Me.Model,
                                                          Me,
                                                          arRelation.Id,
                                                          lrOriginEntity,
                                                          arRelation.OriginMultiplicity,
                                                          arRelation.RelationOriginIsMandatory,
                                                          arRelation.ContributesToPrimaryKey,
                                                          lrDestinationEntity,
                                                          arRelation.DestinationMultiplicity,
                                                          arRelation.RelationDestinationIsMandatory)

                    lrERDRelation.OriginPredicate = arRelation.OriginPredicate
                    lrERDRelation.DestinationPredicate = arRelation.DestinationPredicate
                    lrERDRelation.RelationFactType = arRelation.ResponsibleFactType
                    lrERDRelation.RDSRelation = arRelation

                    Me.ERDiagram.Relation.AddUnique(lrERDRelation)

                    If Me.Diagram IsNot Nothing Then

                        Select Case Me.Language
                            Case Is = pcenumLanguage.EntityRelationshipDiagram

                                Dim lrLink As ERD.Link
                                lrLink = New ERD.Link(Me, New FBM.FactInstance, lrOriginEntity, lrDestinationEntity, Nothing, Nothing, lrERDRelation)
                                lrLink.DisplayAndAssociate()
                                lrERDRelation.Link = lrLink

                            Case Is = pcenumLanguage.PropertyGraphSchema

                                Dim lrLink As PGS.Link
                                lrLink = New PGS.Link(Me, New FBM.FactInstance, lrOriginEntity, lrDestinationEntity, Nothing, Nothing, lrERDRelation)
                                lrLink.RDSRelation = arRelation
                                lrLink.DisplayAndAssociate()
                                'lrLink.Link.Text = arRelation.ActualPGSNode.Id

                        End Select

                    End If
                ElseIf lrOriginEntity Is Nothing And Not lrRelation.ResponsibleFactType.IsLinkFactType And Me.Language = pcenumLanguage.PropertyGraphSchema Then

                    Dim larDestinationModelObjects = lrRelation.ResponsibleFactType.getDesinationModelObjects

                    Dim lbAllFound As Boolean = True
                    For Each lrModelObject In larDestinationModelObjects
                        If Me.ERDiagram.Entity.Find(Function(x) x.Name = lrModelObject.Id) Is Nothing Then
                            lbAllFound = False
                        End If
                    Next

                    If lbAllFound Then
                        Dim lrNode = Me.loadPGSNode(lrRelation.OriginTable, False)
                        If lrNode.NodeType = pcenumPGSEntityType.Relationship Then
                            Call Me.displayPGSRelationNodeLink(lrNode, lrRelation.Id)
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSModel_RelationRemoved(ByVal arRelation As RDS.Relation) Handles RDSModel.RelationRemoved

            Try

                Dim larERDRelation = From ERDRelation In Me.ERDiagram.Relation.FindAll(Function(x) x.RDSRelation IsNot Nothing)
                                     Where ERDRelation.RDSRelation.Id = arRelation.Id
                                     Select ERDRelation

                For Each lrERDRelation In larERDRelation.ToList
                    Call lrERDRelation.removeFromPage()
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSModel_TableRemoved(ByRef arTable As RDS.Table) Handles RDSModel.TableRemoved

            Dim lrTable As RDS.Table = arTable

            Dim lrModelObject As FBM.ModelObject = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

            If lrModelObject IsNot Nothing Then
                'ERDEntity/PGSNode is on the Page.

                Select Case lrModelObject.ConceptType

                    Case Is = pcenumConceptType.Entity

                        Dim lrERDEntity As ERD.Entity = lrModelObject

                        For Each lrAttribute In lrERDEntity.Attribute
                            Call Me.ERDiagram.Attribute.Remove(lrAttribute)
                        Next

                        If Me.Diagram IsNot Nothing Then
                            Me.Diagram.Nodes.Remove(lrERDEntity.TableShape)
                        End If

                        Me.ERDiagram.Entity.Remove(lrERDEntity)

                    Case Is = pcenumConceptType.PGSNode

                        Dim lrPGSNode As PGS.Node = lrModelObject

                        If Me.Diagram IsNot Nothing Then
                            Me.Diagram.Nodes.Remove(lrPGSNode.Shape)
                        End If

                        Me.ERDiagram.Entity.Remove(lrPGSNode)

                End Select
            End If

        End Sub


    End Class

End Namespace
