Imports System.Reflection

Namespace FBM

    Partial Public Class Page

        Public WithEvents RDSModel As New RDS.Model

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
                                                          lrOriginEntity, _
                                                          arRelation.OriginMultiplicity, _
                                                          arRelation.RelationOriginIsMandatory, _
                                                          arRelation.ContributesToPrimaryKey, _
                                                          lrDestinationEntity, _
                                                          arRelation.DestinationMultiplicity, _
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
