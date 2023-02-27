Imports System.Reflection
Imports System.ComponentModel

Namespace FBM

    Partial Public Class Page

        Public Function CreateEntityRelationshipDiagramFromPropertyGraphSchema(ByRef aoBackgroundWorker As BackgroundWorker) As FBM.Page

            Try

                Dim lrModelElement As FBM.ModelObject = Nothing
                Dim lrPage As FBM.Page
                Dim lsSQLQuery As String
                Dim lrORMRecordset As ORMQL.Recordset
                Dim lrORMRecordset1 As ORMQL.Recordset

                Boston.WriteToStatusBar("Loading the MetaModel for Entity Relationship Diagrams")
                aoBackgroundWorker.ReportProgress(15)

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the EntityRelationshipDiagram.
                '----------------------------------------------------
                lrPage = lrCorePage.Clone(Me.Model, False, True)
                lrPage.IsDirty = True
                lrPage.Name = "ERD-" & Trim(Me.Name) '"NewEntityRelationshipDiagram"
                lrPage.Name = Me.Model.CreateUniquePageName(lrPage.Name, 0)
                lrPage.Language = pcenumLanguage.EntityRelationshipDiagram

                Me.Model.Page.Add(lrPage)

                aoBackgroundWorker.ReportProgress(20)

                '=========================================================================================
                'Model Level first.
                '=========================================================================================
                'Create the Entities, Attributes and Relations at the Model level first
                'This is because there may be Attributes identified at the Model level that aren't available on the Page being processed.
                If Not Me.Model.HasCoreModel Then
                    Call Me.Model.createEntityRelationshipArtifacts()
                End If

                '---------------------------------------------------
                'Drop the Node Types onto the Page as Object Types
                '---------------------------------------------------
                Boston.WriteToStatusBar("Creating Entities")

                '=========================================================================================

                aoBackgroundWorker.ReportProgress(40)

                Dim lrFactInstance As FBM.FactInstance = Nothing

                For Each lrPGSNode As PGS.Node In Me.ERDiagram.Entity
                    '===============================================================
                    'Check if is a FactType with TotalInternalUniquenessConstraint
                    '===============================================================
                    '---------------------------------------------------------------
                    'Check to see if the Entity already exists in the ERD MetaModel
                    '---------------------------------------------------------------
                    lsSQLQuery = " SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                    lsSQLQuery &= " WHERE Element = '" & lrPGSNode.Name & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrORMRecordset("Count").Data = 0 Then

                        Boston.WriteToStatusBar("Creating Entity, '" & lrPGSNode.Name & "'")

                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " WHERE Element = '" & lrPGSNode.Name & "'"

                        lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If Not lrORMRecordset1.EOF Then

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrFactInstance.Data(0).X = lrPGSNode.X
                            lrFactInstance.Data(0).Y = lrPGSNode.Y
                            lrFactInstance.FactType.isDirty = True
                            lrFactInstance.isDirty = True
                            Me.IsDirty = True

                        End If

                    End If
                Next


                Boston.WriteToStatusBar("Completed creating the Entity Relationship Diagram, '" & lrPage.Name & "'")

                '---------------------------------
                'Save the new Page to the database
                '---------------------------------
                Me.MakeDirty()
                lrPage.MakeDirty()
                Call Me.Form.EnableSaveButton()

                Return lrPage

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CreatePropertyGraphSchemaFromEntityRelationshipDiagram(ByRef aoBackgroundWorker As BackgroundWorker) As FBM.Page

            Try

                Dim lrModelElement As FBM.ModelObject = Nothing
                Dim lrPage As FBM.Page
                Dim lsSQLQuery As String
                Dim lrORMRecordset As ORMQL.Recordset
                Dim lrORMRecordset1 As ORMQL.Recordset

                Boston.WriteToStatusBar("Loading the MetaModel for Property Graph Schema")
                aoBackgroundWorker.ReportProgress(15)

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CorePropertyGraphSchema.ToString,
                                               pcenumCMMLCorePage.CorePropertyGraphSchema.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.EqualsByName)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CorePropertyGraphSchema.ToString & "', in the Core Model.")
                End If

                '----------------------------------------------------
                'Create the Page for the EntityRelationshipDiagram.
                '----------------------------------------------------
                lrPage = lrCorePage.Clone(Me.Model, False, True)
                lrPage.IsDirty = True
                lrPage.Name = "PGS-" & Trim(Me.Name) '"NewEntityRelationshipDiagram"
                lrPage.Name = Me.Model.CreateUniquePageName(lrPage.Name, 0)
                lrPage.Language = pcenumLanguage.EntityRelationshipDiagram

                Me.Model.Page.Add(lrPage)

                aoBackgroundWorker.ReportProgress(20)

                '=========================================================================================
                'Model Level first.
                '=========================================================================================
                'Create the Entities, Attributes and Relations at the Model level first
                'This is because there may be Attributes identified at the Model level that aren't available on the Page being processed.
                If Not Me.Model.HasCoreModel Then
                    Call Me.Model.createEntityRelationshipArtifacts()
                End If

                '---------------------------------------------------
                'Drop the Node Types onto the Page as Object Types
                '---------------------------------------------------
                Boston.WriteToStatusBar("Creating Entities")

                '=========================================================================================

                aoBackgroundWorker.ReportProgress(40)

                Dim lrFactInstance As FBM.FactInstance = Nothing

                For Each lrPGSNode As ERD.Entity In Me.ERDiagram.Entity
                    '===============================================================
                    'Check if is a FactType with TotalInternalUniquenessConstraint
                    '===============================================================
                    '---------------------------------------------------------------
                    'Check to see if the Entity already exists in the ERD MetaModel
                    '---------------------------------------------------------------
                    lsSQLQuery = " SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                    lsSQLQuery &= " WHERE Element = '" & lrPGSNode.Name & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrORMRecordset("Count").Data = 0 Then

                        Boston.WriteToStatusBar("Creating Node Type, '" & lrPGSNode.Name & "'")

                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " WHERE Element = '" & lrPGSNode.Name & "'"

                        lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If Not lrORMRecordset1.EOF Then

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrFactInstance.Data(0).X = lrPGSNode.X
                            lrFactInstance.Data(0).Y = lrPGSNode.Y
                            lrFactInstance.FactType.isDirty = True
                            lrFactInstance.isDirty = True
                            Me.IsDirty = True

                        End If

                    End If
                Next

                Boston.WriteToStatusBar("Completed creating the Property Graph Schema, '" & lrPage.Name & "'")

                '---------------------------------
                'Save the new Page to the database
                '---------------------------------
                Me.MakeDirty()
                lrPage.MakeDirty()
                Call Me.Form.EnableSaveButton()

                Return lrPage

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Function CreateObjectRoleModelFromPropertyGraphSchema(ByRef aoBackgroundWorker As BackgroundWorker) As FBM.Page

            Try

                Dim lrModelElement As FBM.ModelObject = Nothing
                Dim lrPage As FBM.Page

                Dim lsPageName = Me.Model.CreateUniquePageName("NewObjectRoleModel", 0)
                lrPage = New FBM.Page(Me.Model, Nothing, lsPageName, pcenumLanguage.ORMModel)
                lrPage.IsDirty = True

                Me.Model.Page.Add(lrPage)

                '---------------------------------------------------
                'Drop the Node Types onto the Page as Object Types
                '---------------------------------------------------
                Boston.WriteToStatusBar("Creating Object Types")

                Dim lrEntityType As FBM.EntityType = Nothing
                Dim lrFactType As FBM.FactType = Nothing
                Dim lrFactTypeInstance As FBM.FactTypeInstance = Nothing

                For Each lrNodeType As PGS.Node In Me.ERDiagram.Entity.FindAll(Function(x) x.getCorrespondingRDSTable.FBMModelElement.GetType = GetType(FBM.EntityType))

                    lrEntityType = lrNodeType.RDSTable.FBMModelElement
                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                    lrEntityTypeInstance = lrPage.DropEntityTypeAtPoint(lrEntityType, New PointF(lrNodeType.X, lrNodeType.Y), False)

                    If lrEntityType.HasCompoundReferenceMode Then

                        Dim loPointF As PointF = Nothing
                        Dim larModelElement = New List(Of FBM.ModelObject)

                        For Each lrRole In lrEntityType.ReferenceModeRoleConstraint.Role

                            Dim lbEmptySpaceFound As Boolean = False
                            loPointF = lrPage.FindBlankSpaceInRelationToModelObject(lrEntityTypeInstance, lbEmptySpaceFound)
                            lrFactTypeInstance = lrPage.DropFactTypeAtPoint(lrRole.FactType, loPointF, False,,,,, False)

                            lrModelElement = lrFactTypeInstance.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject
                            larModelElement.Add(lrModelElement)
                        Next

                        loPointF = Me.GetMidPointOfModelObjects(larModelElement)
                        Call lrPage.DropRoleConstraintAtPoint(lrEntityType.ReferenceSchemeRoleConstraint, loPointF, True)

                    End If

                Next

                lrFactType = Nothing
                For Each lrNodeType As PGS.Node In Me.ERDiagram.Entity.FindAll(Function(x) x.getCorrespondingRDSTable.FBMModelElement.GetType = GetType(FBM.FactType))

                    lrFactType = lrNodeType.RDSTable.FBMModelElement

                    Call lrPage.DropFactTypeAtPoint(lrFactType, New PointF(lrNodeType.X, lrNodeType.Y), False,,,,, False, True)

                    Dim larFactType As New List(Of FBM.FactType)
                    If lrFactType.hasAssociatedFactTypes(larFactType) Then

                        For Each lrAssociatedFactType In larFactType

                            Dim larRole = From Role In lrAssociatedFactType.RoleGroup
                                          Where Role.JoinedORMObject.Id = lrFactType.Id
                                          Where Role.HasInternalUniquenessConstraint
                                          Select Role

                            If larRole.Count > 0 Then
                                If larRole(0).FactType.IsManyTo1BinaryFactType Then
                                    Call lrPage.DropFactTypeAtPoint(lrAssociatedFactType, New PointF(lrNodeType.X, lrNodeType.Y), False,,,,, False)
                                End If
                            End If

                        Next

                    End If
                Next

                Dim larFactTypeInstance = From FactTypeInstance In lrPage.FactTypeInstance
                                          Where FactTypeInstance.IsObjectified
                                          Select FactTypeInstance

                For Each lrFactTypeInstance In larFactTypeInstance
                    Dim loPointF = lrPage.GetMidPointOfModelObjects(lrFactTypeInstance.FactType.ModelObjects)
                    Call lrFactTypeInstance.Move(loPointF.X, loPointF.Y, True)
                Next

                aoBackgroundWorker.ReportProgress(40)

                '---------------------------------------------------
                'Drop the Relations onto the Page as Fact Types
                '---------------------------------------------------
                Boston.WriteToStatusBar("Creating Object Types")

                Dim larERDRelation = From ERDRelation In Me.ERDiagram.Relation
                                     Where ERDRelation.RDSRelation IsNot Nothing
                                     Where ERDRelation.RDSRelation.ResponsibleFactType IsNot Nothing
                                     Select ERDRelation

                For Each lrRelation As ERD.Relation In larERDRelation

                    If lrRelation.RDSRelation.ResponsibleFactType.IsObjectified Or lrRelation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        If lrRelation.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                            lrFactType = lrRelation.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                        Else
                            lrFactType = lrRelation.RDSRelation.ResponsibleFactType
                        End If
                    Else
                        lrFactType = lrRelation.RDSRelation.ResponsibleFactType
                    End If

                    If Not lrFactType.IsLinkFactType Then
                        Call lrPage.DropFactTypeIfFoundObjects(lrFactType)
                    End If
                Next

                '============================================================================================================================================
                'Bring in Attributes from the Model level that are not at the Page level                
                aoBackgroundWorker.ReportProgress(100)

                Boston.WriteToStatusBar("Completed creating the Object Role Model, '" & lrPage.Name & "'")

                '---------------------------------
                'Save the new Page to the database
                '---------------------------------
                Me.MakeDirty()
                lrPage.MakeDirty()
                Call Me.Form.EnableSaveButton()

                Return lrPage

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Private Sub DropFactTypeIfFoundObjects(ByRef arFactType As FBM.FactType)

            Try
                Dim lrFactType = arFactType
                For Each lrModelElement In lrFactType.RoleGroup.Select(Function(x) x.JoinedORMObject)
                    If Me.GetAllPageObjects.Find(Function(x) x.Id = lrModelElement.Id) Is Nothing Then
                        Call Me.DropFactTypeIfFoundObjects(lrModelElement)
                    End If
                Next

                Dim lbFoundObjects As Boolean = True
                Dim loPointF As PointF = Me.GetMidPointOfModelObjects(lrFactType.GetModelObjects, lbFoundObjects)

                Call Me.DropFactTypeAtPoint(lrFactType, loPointF, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Function LoadERDEntityFromRDSTable(ByRef arTable As RDS.Table,
                                                  ByRef aoPointF As PointF) As ERD.Entity

            Dim lrERDEntity As ERD.Entity = Nothing
            Dim lsSQLQuery As String = Nothing
            Dim lrRecordset As ORMQL.Recordset = Nothing
            Dim lsMessage As String
            Dim lrFactInstance As FBM.FactInstance

            Try

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arTable.Name & "'"
                lsSQLQuery &= " AND ElementType = 'Entity'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrRecordset.EOF Then
                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrERDEntity = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneEntity(Me)
                    '===================================================================================================================
                    With New WaitCursor
                        lrERDEntity.RDSTable = arTable 'IMPORTANT: Leave this at this point in the code. Otherwise (somehow) lrEntity ends up with no TableShape.

                        Call Me.DropExistingEntityAtPoint(lrERDEntity, aoPointF, False)
                        Call Me.loadRelationsForEntity(lrERDEntity, False)
                        Call lrERDEntity.Move(aoPointF.X, aoPointF.Y, True)
                        Call Me.Save(False, False)
                    End With

                End If

                Return lrERDEntity

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrERDEntity
            End Try

        End Function

        Public Function LoadPGSNodeTypeFromRDSTable(ByRef arTable As RDS.Table,
                                                    ByRef aoPointF As PointF,
                                                    Optional ByVal abDisplayAnyway As Boolean = False) As PGS.Node

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactInstance As FBM.FactInstance
                Dim lrNode As PGS.Node

                'CodeSafe
                If arTable Is Nothing Then Return Nothing

                'CodeSafe
                Dim lsTableName = arTable.Name
                lrNode = Me.ERDiagram.Entity.Find(Function(x) x.Name = lsTableName)
                If lrNode IsNot Nothing Then
                    If abDisplayAnyway Then GoTo DisplayAnyway
                    Return lrNode  'Already on the Page.
                End If

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arTable.Name & "'"
                lsSQLQuery &= " AND ElementType = 'Entity'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrNode = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).ClonePGSNodeType(Me)
DisplayAnyway:
                '===================================================================================================================
                lrNode.RDSTable = arTable 'IMPORTANT: Leave this at this point in the code.
                Call Me.DropExistingPGSNodeAtPoint(lrNode, aoPointF)

                If lrNode.RDSTable.isPGSRelation And lrNode.RDSTable.Arity < 3 Then
                    'Need to load the relation for the joined Nodes, not the PGSRelation.
                    'E.g. If 'Person likes Person WITH Rating'...then need to load that relation

                    Call Me.loadRelationsForPGSNode(lrNode, False)

                    Dim lrFactType As FBM.FactType = lrNode.RDSTable.FBMModelElement

                    Dim larDestinationModelObjects = lrFactType.getDestinationModelObjects

                    Dim lrRDSRelation = Me.Model.RDS.Relation.Find(Function(x) x.OriginTable.Name = lrNode.Name And
                                                                               larDestinationModelObjects.Select(Function(y) y.Id).ToList.Contains(x.DestinationTable.Name))

                    Dim lbAllFound As Boolean = True
                    For Each lrModelObject In larDestinationModelObjects
                        If Me.ERDiagram.Entity.Find(Function(x) x.Name = lrModelObject.Id) Is Nothing Then
                            lbAllFound = False
                        End If
                    Next

                    If lbAllFound Then
                        If lrNode.RDSTable.isPGSRelation Then
                            Call Me.DisplayPGSRelationNodeLink(lrNode, lrRDSRelation)
                            lrNode.Shape.Visible = False
                            For Each lrEdge As MindFusion.Diagramming.DiagramLink In lrNode.Shape.OutgoingLinks
                                lrEdge.Visible = False
                            Next
                        End If
                    End If
                Else
                    Call Me.loadRelationsForPGSNode(lrNode, False)
                    Call Me.loadPropertyRelationsForPGSNode(lrNode, False)
                End If

                Call lrNode.RefreshShape()

                Return lrNode

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try
        End Function

    End Class

End Namespace

