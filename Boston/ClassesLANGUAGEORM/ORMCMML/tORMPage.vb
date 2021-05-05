Imports System.Reflection

Namespace FBM

    Partial Public Class Page

        Public Sub AddAttributeToEntity(ByRef arRDSColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrORMRecordset As ORMQL.Recordset
            Dim lrORMRecordset2 As ORMQL.Recordset
            Dim lsPropertyInstanceId As String = ""

            Try
                lsPropertyInstanceId = arRDSColumn.Id

                Richmond.WriteToStatusBar("Adding Property to Page: '" & lsPropertyInstanceId & "'")

                '--------------------------------------------------------------------------------------------------
                'The Attribute is not on the Page.
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " WHERE ModelObject = '" & arRDSColumn.Table.Name & "'"
                lsSQLQuery &= "   AND Attribute = '" & arRDSColumn.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '======================================================================================================================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyIsForFactType" '(Property, FactType)"
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                'Responsible Role
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyIsForRole" '(Property, Role)
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'Active Role
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyHasActiveRole" '(Property, Role)
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString '(Attribute, Position)
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Check to see if the Attribute is Mandatory
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString '(Attribute, Position)
                lsSQLQuery &= " WHERE IsMandatory = '" & lsPropertyInstanceId & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrORMRecordset2.EOF Then
                    lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                Call Me.MakeDirty()
                Call Me.Save()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addRDSRelation(ByRef arRelation As RDS.Relation)

            Try
                Dim lsSQLQuery As String = ""
                Dim lrORMRecordset As ORMQL.Recordset

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                lsSQLQuery &= " WHERE Relation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString '(Entity, Relation)
                lsSQLQuery &= " WHERE Relation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString  '(ERDEntity, Multiplicity)
                lsSQLQuery &= " WHERE ERDRelation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Dim liOriginMultiplicity As pcenumCMMLMultiplicity
                Call liOriginMultiplicity.GetByDescription(lrORMRecordset("Multiplicity").Data)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString '(ERDEntity, Multiplicity)
                lsSQLQuery &= " WHERE ERDRelation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
                Call liDestinationMultiplicity.GetByDescription(lrORMRecordset("Multiplicity").Data)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '-----------------------------------
                'Get the FactType for the Relation
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString '(Relation, FactType)
                lsSQLQuery &= " WHERE Relation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Dim lrFactType As New FBM.FactType(Me.Model, lrORMRecordset("FactType").Data, True)
                lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.Equals)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '=================================================================
                'Origin/Destination Predicates
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString '(Relation, Predicate)
                lsSQLQuery &= " WHERE Relation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString  '(Relation, Predicate)
                lsSQLQuery &= " WHERE Relation = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrORMRecordset.Facts.Count > 0 Then
                    lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                '===================================================================
                'Origin/Destination Mandatory requirements
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString  '(Relation, Predicate)
                lsSQLQuery &= " WHERE OriginIsMandatory = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Dim lbRelationOriginIsMandatory As Boolean = lrORMRecordset.Facts.Count > 0

                While Not lrORMRecordset.EOF
                    lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrORMRecordset.MoveNext()
                End While

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString  '(Relation, Predicate)
                lsSQLQuery &= " WHERE DestinationIsMandatory = '" & arRelation.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Dim lbRelationDestinationIsMandatory As Boolean = lrORMRecordset.Facts.Count > 0

                While Not lrORMRecordset.EOF
                    lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrORMRecordset.MoveNext()
                End While

                Call Me.MakeDirty()
                Call Me.Save()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addRDSTableToPage(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arTable.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrRecordset.EOF Then

                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                    lsSQLQuery &= " WHERE IsPGSRelation = '" & arTable.Name & "'"

                    Dim lrRecordsetIsPGSRelation As ORMQL.Recordset
                    lrRecordsetIsPGSRelation = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrRecordsetIsPGSRelation.EOF Then
                        lsSQLQuery = "ADD FACT '" & lrRecordsetIsPGSRelation.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    For Each lrRelation In arTable.getOutgoingRelations
                        Call Me.addRDSRelation(lrRelation)
                    Next

                    Call Me.MakeDirty()
                    Call Me.Save()

                Else
                    Throw New Exception("Couldn't find CoreElement for, '" & arTable.Name & "'.")
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Creates an Entity Relationship Diagram from an ORM-Diagram Page.
        ''' Puts the ERD on a Page under the same Richmond.Model as the ORM-Diagram of the Page,
        '''   and under the same Node/Section on the Model Tree.
        ''' </summary>
        Public Function CreateEntityRelationshipDiagram(ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker) As FBM.Page

            'SeeAlso(Model.IsObjectAnAttribute)

            '-----------------------------------------------------------------------------
            'Pseudocode:
            '
            '  * Create a new Page for the Model of Language "Entity-Relationship Diagram"
            '  * Follow Halpin's steps to create an ERD from an ORM Diagram
            '      to create the Entities, Relations
            '------------------------------------------------------------------------------
            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance
            Dim lrFact As New FBM.Fact
            Dim lsEntityName As String = ""
            Dim lrPage As FBM.Page
            Dim lsSQLQuery As String = ""

            Dim lrORMRecordset As ORMQL.Recordset
            Dim lrORMRecordset1 As ORMQL.Recordset

            Try
                '==============================================================
                'Get the Core Metamodel.Page for an EntityRelationshipDiagram
                '==============================================================
                '-------------------------------------------
                'Get the EntityRelationshipModel Core Page
                '-------------------------------------------
                Richmond.WriteToStatusBar("Loading the MetaModel for Entity Relationship Diagrams")
                aoBackgroundWorker.ReportProgress(5)

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core, _
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString, _
                                               pcenumCMMLCorePage.CoreEntityRelationshipDiagram.ToString, _
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

                aoBackgroundWorker.ReportProgress(10)

                '=========================================================================================
                'Model Level first.
                '=========================================================================================
                'Create the Entities, Attributes and Relations at the Model level first
                'This is because there may be Attributes identified at the Model level that aren't available on the Page being processed.
                If Not Me.Model.HasCoreModel Then
                    Call Me.Model.createEntityRelationshipArtifacts()
                End If

                '=========================================================================================

                aoBackgroundWorker.ReportProgress(20)

                '=======================================================
                'Start creating the Entities, Attributes and Relations
                '=======================================================

                '--------------------------------------------------------------------------------------------------------------
                'Create an Entity for each FactType with a TotalInternalUniquenessConstraint or PartialButMultiRoleConstraint
                '--------------------------------------------------------------------------------------------------------------
                Richmond.WriteToStatusBar("Creating Entities and Relations")

                Dim lasEntity As New List(Of String)

                For Each lrFactTypeInstance In Me.FactTypeInstance
                    '===============================================================
                    'Check if is a FactType with TotalInternalUniquenessConstraint
                    '===============================================================
                    If lrFactTypeInstance.HasTotalRoleConstraint Or lrFactTypeInstance.HasPartialButMultiRoleConstraint Then
                        '-----------------------------------------------------
                        'Is a FactType with TotalInternalUniquenessConstraint
                        '-----------------------------------------------------                        

                        '---------------------------------------------------------------
                        'Check to see if the Entity already exists in the ERD MetaModel
                        '---------------------------------------------------------------
                        lsSQLQuery = " SELECT COUNT(*)"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        lsSQLQuery &= " WHERE Element = '" & lrFactTypeInstance.Name & "'"

                        lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset("Count").Data = 0 Then

                            Richmond.WriteToStatusBar("Creating Entity, '" & lrFactTypeInstance.Name & "'")

                            lsSQLQuery = " SELECT *"
                            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " WHERE Element = '" & lrFactTypeInstance.Name & "'"

                            lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrORMRecordset1.EOF Then

                                lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                lasEntity.AddUnique(lrFactTypeInstance.Name)
                            End If

                        End If
                    End If
                Next

                aoBackgroundWorker.ReportProgress(40)

                '===============================================================
                'Double check that Entities are on the Page for each Attribute
                '===============================================================
                Dim lsPropertyInstanceId As String
                Dim lrEntity As New ERD.Entity(lrPage, "")
                Dim lrAttribute As New ERD.Attribute("DummyId", lrEntity)
                For Each lrRoleInstance In Me.RoleInstance

                    If lrRoleInstance.IsERDPropertyRole Then

                        lsEntityName = lrRoleInstance.BelongsToTable

                        lrEntity.Name = lsEntityName

                        lasEntity.AddUnique(lsEntityName)

                        '---------------------------------------------------------------------
                        'Check to see that the Entity (already) exists in the ERD MetaModel.
                        '  If the Entity doesn't exist, create it.
                        '---------------------------------------------------------------------
                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                        lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset.EOF Then
                            '---------------------------------------------------------------
                            'The Entity does not exist in the ERD MetaModel, so create it.
                            '---------------------------------------------------------------
                            lsSQLQuery = " SELECT *"
                            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                            lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrORMRecordset1.EOF Then
                                Richmond.WriteToStatusBar("Creating Entity, '" & lsEntityName & "'")
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            End If
                        End If
                    End If 'Is Property
                Next 'RoleInstance - Creating Attributes

                '======================================================================================================
                'Add each EntityType that is on the Page as an Entity on the ERD Page, just for good measure.
                '  The user can remove the EntityType if it is not required.
                '  The reason for this is because some EntityTypes on the Page don't necessarily have any RoleInstances
                '  that would have been captured (above) as PropertyRoles.
                '------------------------------------------------------------------------------------------------------
                For Each lrEntityTypeInstance In Me.EntityTypeInstance
                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                    lsSQLQuery &= " WHERE Element = '" & lrEntityTypeInstance.Id & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrORMRecordset.EOF And Not lasEntity.Contains(lrEntityTypeInstance.Id) Then
                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString                       
                        lsSQLQuery &= " WHERE Element = '" & lrEntityTypeInstance.Id & "'"

                        lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If Not lrORMRecordset1.EOF Then
                            lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lasEntity.AddUnique(lrEntityTypeInstance.Id)
                        End If
                    End If
                Next
                '======================================================================================================


                '============================================================================================================================================
                'Bring in Attributes from the Model level that are not at the Page level                

                aoBackgroundWorker.ReportProgress(50)

                Dim lrORMRecordset2 As ORMQL.Recordset

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim liAttributeCount As Integer = 0
                liAttributeCount = lrORMRecordset("Count").Data
                Dim liAttributeCounter As Integer = 0

                For Each lsEntityName In lasEntity

                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " WHERE ModelObject = '" & lsEntityName & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Richmond.WriteToStatusBar("Adding Properties for Entity, '" & lsEntityName & "' to Page", True)

                    While Not lrORMRecordset.EOF

                        liAttributeCounter += 1
                        aoBackgroundWorker.ReportProgress(((CInt(liAttributeCounter / liAttributeCount) * 25) + 50).MaximumValue(100))

                        lsPropertyInstanceId = lrORMRecordset("Attribute").Data

                        Richmond.WriteToStatusBar("Adding Property to Page: '" & lsPropertyInstanceId & "'")

                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        lsSQLQuery &= " WHERE Attribute = '" & lrORMRecordset("Attribute").Data & "'"

                        lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset1.EOF Then
                            '--------------------------------------------------------------------------------------------------
                            'The Attribute is not on the Page.

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            '======================================================================================================================
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyIsForFactType" '(Property, FactType)"
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString 'CorePropertyHasPropertyName.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyIsForRole" '(Property, Role)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyHasActiveRole" '(Property, Role)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString '(Attribute, Position)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            '--------------------------------------------
                            'Check to see if the Attribute is Mandatory
                            '--------------------------------------------
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString '(Attribute, Position)
                            lsSQLQuery &= " WHERE IsMandatory = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrORMRecordset2.EOF Then
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            End If
                            '======================================================================================================================
                        End If
                        lrORMRecordset.MoveNext()
                    End While
                Next 'lsEntityName
                '============================================================================================================================================


                '============================================================================================================================================
                'Bring the Relationships in from the Model level.
                ''===========================================
                ''Relationships between Entities
                ''===========================================
                Dim liEntityCounter As Integer = 0
                For Each lsEntityName In lasEntity

                    liEntityCounter += 1
                    aoBackgroundWorker.ReportProgress(((liEntityCounter / lasEntity.Count) * 25) + 75)

                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                    lsSQLQuery &= " WHERE Entity = '" & lsEntityName & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Richmond.WriteToStatusBar("Adding Relations for Entity, '" & lsEntityName & "' to Page", True)

                    While Not lrORMRecordset.EOF

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString '(Entity, Relation)
                        lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lasEntity.Contains(lrORMRecordset2("Entity").Data) Then
                            'Destination Entity is on the Page.

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString  '(ERDEntity, Multiplicity)
                            lsSQLQuery &= " WHERE ERDRelation = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString '(ERDEntity, Multiplicity)
                            lsSQLQuery &= " WHERE ERDRelation = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString '(Relation, FactType)
                            lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            '        lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                            '        lsSQLQuery &= " (OriginIsMandatory)"
                            '        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                            '        lsSQLQuery &= " VALUES ("
                            '        lsSQLQuery &= "'" & lsRelationId & "'"
                            '        lsSQLQuery &= " )"

                            '        'lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                            '        'lsSQLQuery &= " (" & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString & ")"
                            '        'lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                            '        'lsSQLQuery &= " VALUES ("
                            '        'lsSQLQuery &= "'" & lsRelationId & "'"
                            '        'lsSQLQuery &= " )"


                            '=================================================================
                            'Origin/Destination Predicates
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString '(Relation, Predicate)
                            lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString  '(Relation, Predicate)
                            lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If lrORMRecordset2.Facts.Count > 0 Then
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            End If

                            '===================================================================
                            'Origin/Destination Mandatory requirements
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString  '(Relation, Predicate)
                            lsSQLQuery &= " WHERE OriginIsMandatory = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            While Not lrORMRecordset2.EOF
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                lrORMRecordset2.MoveNext()
                            End While

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString  '(Relation, Predicate)
                            lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lrORMRecordset("Relation").Data & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            While Not lrORMRecordset2.EOF
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                lrORMRecordset2.MoveNext()
                            End While

                        End If 'Destination Entity is on the Page.

                        lrORMRecordset.MoveNext()
                    End While

                Next 'lsEntityName
                '============================================================================================================================================

                Richmond.WriteToStatusBar("Completed creating the Entity Relationship Diagram, '" & lrPage.Name & "'")

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

        End Function 'CreateEntityRelationshipDiagram

        ''' <summary>
        ''' Creates an PropertGraphSchema Diagram from an ORM-Diagram Page.
        ''' Puts the PGS-Diagram on a Page under the same Richmond.Model as the ORM-Diagram
        '''   and under the same Node/Section on the Enterprise Model Tree.
        ''' </summary>    
        Public Function CreatePropertyGraphSchema(ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker) As FBM.Page

            'SeeAlso(Model.IsObjectAnAttribute)

            '-----------------------------------------------------------------------------
            'Pseudocode:
            '
            '  * Create a new Page for the Model of Language "Entity-Relationship Diagram"
            '  * Follow Halpin's steps to create an ERD from an ORM Diagram
            '      to create the Entities, Relations
            '------------------------------------------------------------------------------

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance
            Dim lsEntityName As String = ""
            Dim lrPage As FBM.Page
            Dim lsSQLQuery As String = ""

            Dim lrORMRecordset As ORMQL.Recordset
            Dim lrORMRecordset1 As ORMQL.Recordset

            Try
                '==============================================================
                'Get the Core Metamodel.Page for an EntityRelationshipDiagram
                '==============================================================

                '----------------------------------------------
                'CodeSafe: Remove Tables with no Columns.
                Call Me.Model.RDS.RemoveTablesWithNoColumns()

                '-------------------------------------------
                'Get the EntityRelationshipModel Core Page
                '-------------------------------------------
                Richmond.WriteToStatusBar("Loading the MetaModel for Entity Relationship Diagrams")

                Dim lrCorePage As New FBM.Page(prApplication.CMML.Core,
                                               pcenumCMMLCorePage.CorePropertyGraphSchema.ToString,
                                               pcenumCMMLCorePage.CorePropertyGraphSchema.ToString,
                                               pcenumLanguage.ORMModel)

                lrCorePage = prApplication.CMML.Core.Page.Find(AddressOf lrCorePage.Equals)

                If lrCorePage Is Nothing Then
                    Throw New Exception("Couldn't find Page, '" & pcenumCMMLCorePage.CorePropertyGraphSchema.ToString & "', in the Core Model.")
                End If

                lrPage = lrCorePage.Clone(Me.Model, False, True)
                lrPage.IsDirty = True
                lrPage.Name = "PGS-" & Trim(Me.Name)
                lrPage.Name = Me.Model.CreateUniquePageName(lrPage.Name, 0)
                lrPage.Language = pcenumLanguage.PropertyGraphSchema

                Me.Model.Page.Add(lrPage)

                '=========================================================================================
                'Model Level first.
                '  Create the Entities, Attributes and Relations at the Model level first
                '  This is because there may be Attributes identified at the Model level that aren't available on the Page being processed.
                If Not Me.Model.HasCoreModel Then
                    Call Me.Model.createEntityRelationshipArtifacts()
                End If                
                '=========================================================================================

                '----------------------------------------------------------------------------
                'Create an Entity for each FactType with a TotalInternalUniquenessConstraint
                '----------------------------------------------------------------------------
                Richmond.WriteToStatusBar("Creating Nodes and Relations")

                '----------------------------------------------------------------------------
                'Create an Entity for each FactType with a TotalInternalUniquenessConstraint
                '----------------------------------------------------------------------------
                Richmond.WriteToStatusBar("Creating Entities and Relations")

                Dim lasEntity As New List(Of String)

                For Each lrFactTypeInstance In Me.FactTypeInstance
                    '===============================================================
                    'Check if is a FactType with TotalInternalUniquenessConstraint
                    '===============================================================
                    If lrFactTypeInstance.HasTotalRoleConstraint Or lrFactTypeInstance.HasPartialButMultiRoleConstraint Then
                        '-----------------------------------------------------
                        'Is a FactType with TotalInternalUniquenessConstraint
                        '-----------------------------------------------------                        

                        '---------------------------------------------------------------
                        'Check to see if the Entity already exists in the ERD MetaModel
                        '---------------------------------------------------------------
                        lsSQLQuery = " SELECT COUNT(*)"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        lsSQLQuery &= " WHERE Element = '" & lrFactTypeInstance.Name & "'"

                        lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset("Count").Data = 0 Then

                            Richmond.WriteToStatusBar("Creating Entity, '" & lrFactTypeInstance.Name & "'")

                            lsSQLQuery = " SELECT *"
                            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " WHERE Element = '" & lrFactTypeInstance.Name & "'"

                            lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrORMRecordset1.EOF Then

                                lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                lasEntity.AddUnique(lrFactTypeInstance.Name)
                            End If

                        End If
                    End If
                Next

                aoBackgroundWorker.ReportProgress(40)

                '=======================
                'Create the Attributes
                '=======================
                Dim lsPropertyInstanceId As String
                Dim lrEntity As New ERD.Entity(lrPage, "")
                Dim lrAttribute As New ERD.Attribute("DummyId", lrEntity)

                'For Each lrRoleInstance In Me.RoleInstance

                '    If lrRoleInstance.IsERDPropertyRole Then

                '        lsEntityName = lrRoleInstance.BelongsToTable
                '        lsAttributeName = lrRoleInstance.GetAttributeName

                '        lrEntity.Name = lsEntityName

                '        lasEntity.AddUnique(lsEntityName)

                '        lrAttribute.AttributeName = lsAttributeName

                '        lsAttributeName = lrCMMLModel.CreateUniquePropertyName(lrAttribute, 0)

                '        '---------------------------------------------------------------------
                '        'Check to see that the Entity (already) exists in the ERD MetaModel.
                '        '  If the Entity doesn't exist, create it.
                '        '---------------------------------------------------------------------
                '        lsSQLQuery = " SELECT *"
                '        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                '        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                '        lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                '        lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '        If lrORMRecordset.EOF Then
                '            '---------------------------------------------------------------
                '            'The Entity does not exist in the ERD MetaModel, so create it.
                '            '---------------------------------------------------------------
                '            lsSQLQuery = " SELECT *"
                '            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                '            lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                '            lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '            If Not lrORMRecordset1.EOF Then
                '                Richmond.WriteToStatusBar("Creating Entity, '" & lsEntityName & "'")
                '                lsSQLQuery = "ADD FACT '" & lrORMRecordset1.CurrentFact.Id & "'"
                '                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                '                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                '                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                '            End If
                '        End If
                '    End If 'Is Property
                'Next 'RoleInstance - Creating Attributes
                aoBackgroundWorker.ReportProgress(40)

                '======================================================
                'Create the Nodes for Attributes that are on the Page
                '======================================================
                For Each lrRoleInstance In Me.RoleInstance

                    If lrRoleInstance.IsERDPropertyRole Then

                        lsEntityName = lrRoleInstance.BelongsToTable

                        lrEntity.Name = lsEntityName

                        lasEntity.AddUnique(lsEntityName)

                        '---------------------------------------------------------------------
                        'Check to see that the Entity (already) exists in the ERD MetaModel.
                        '  If the Entity doesn't exist, create it.
                        '---------------------------------------------------------------------
                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                        lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset.EOF Then
                            'Nothing to do here.
                            'Table may not exist because it IsAbsorbed (for its FBMModelObject).

                            '20200422-VM-Below redacted. ORM Pages take care of creating Tables in the RDS.
                            '---------------------------------------------------------------
                            'The Entity does not exist in the ERD MetaModel, so create it.
                            '---------------------------------------------------------------
                            'Richmond.WriteToStatusBar("Creating Entity, '" & lsEntityName & "'")

                            'lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            'lsSQLQuery &= " (Element, ElementType)"
                            'lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                            'lsSQLQuery &= " VALUES ('" & lsEntityName & "', 'Entity')"
                            'Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)                            
                        Else
                            '---------------------------------------------------------------
                            'The Entity may already exist in the Model but not on the Page
                            '---------------------------------------------------------------
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                            lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                            lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If lrORMRecordset1.EOF Then
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lasEntity.AddUnique(lsEntityName)
                            Else
                                '-----------------------------------
                                'The Entity is already on the Page
                                '-----------------------------------
                            End If

                        End If
                    End If 'Is Property
                Next 'RoleInstance - Creating Attributes

                '==============================================
                'PGS Relations
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                lsSQLQuery &= " WHERE ElementType = 'Entity'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                    lsSQLQuery &= " WHERE IsPGSRelation = '" & lrORMRecordset("Element").Data & "'"

                    Dim lrRecordsetIsPGSRelation As ORMQL.Recordset
                    lrRecordsetIsPGSRelation = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrRecordsetIsPGSRelation.EOF Then
                        lsSQLQuery = "ADD FACT '" & lrRecordsetIsPGSRelation.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lrORMRecordset.MoveNext()
                End While


                '======================================================================================================
                'Add each EntityType that is on the Page as an Entity on the ERD Page, just for good measure.
                '  The user can remove the EntityType if it is not required.
                '  The reason for this is because some EntityTypes on the Page don't necessarily have any RoleInstances
                '  that would have been captured (above) as PropertyRoles.
                '------------------------------------------------------------------------------------------------------
                For Each lrEntityTypeInstance In Me.EntityTypeInstance
                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " WHERE Element = '" & lrEntityTypeInstance.Id & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset.EOF And Not lasEntity.Contains(lrEntityTypeInstance.Id) Then
                        lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lasEntity.AddUnique(lrEntityTypeInstance.Id)
                    End If
                Next
                '======================================================================================================


                '============================================================================================================================================
                'Bring in Attributes from the Model level that are not at the Page level                

                aoBackgroundWorker.ReportProgress(50)

                Dim lrORMRecordset2 As ORMQL.Recordset

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim liAttributeCount As Integer = 0
                liAttributeCount = lrORMRecordset("Count").Data
                Dim liAttributeCounter As Integer = 0

                For Each lsEntityName In lasEntity

                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " WHERE ModelObject = '" & lsEntityName & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Richmond.WriteToStatusBar("Adding Properties for Entity, '" & lsEntityName & "' to Page", True)

                    While Not lrORMRecordset.EOF

                        liAttributeCounter += 1
                        aoBackgroundWorker.ReportProgress((((liAttributeCounter / liAttributeCount) * 25) + 50).MaximumValue(100))

                        lsPropertyInstanceId = lrORMRecordset("Attribute").Data

                        Richmond.WriteToStatusBar("Adding Property to Page: '" & lsPropertyInstanceId & "'")

                        lsSQLQuery = " SELECT *"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        lsSQLQuery &= " WHERE Attribute = '" & lrORMRecordset("Attribute").Data & "'"

                        lrORMRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset1.EOF Then
                            '--------------------------------------------------------------------------------------------------
                            'The Attribute is not on the Page.

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            '======================================================================================================================
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyIsForFactType" '(Property, FactType)"
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM CorePropertyIsForRole" '(Property, Role)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString '(Attribute, Position)
                            lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            '--------------------------------------------
                            'Check to see if the Attribute is Mandatory
                            '--------------------------------------------
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString '(Attribute, Position)
                            lsSQLQuery &= " WHERE IsMandatory = '" & lsPropertyInstanceId & "'"

                            lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If Not lrORMRecordset2.EOF Then
                                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                                lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            End If
                            '======================================================================================================================
                        End If
                        lrORMRecordset.MoveNext()
                    End While
                Next 'lsEntityName
                '============================================================================================================================================


                '============================================================================================================================================
                'Bring the Relationships in from the Model level.
                ''===========================================
                ''Relationships between Entities
                ''===========================================
                Dim liEntityCounter As Integer = 0
                For Each lsEntityName In lasEntity

                    liEntityCounter += 1
                    aoBackgroundWorker.ReportProgress(((liEntityCounter / lasEntity.Count) * 25) + 75)

                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                    lsSQLQuery &= " WHERE Entity = '" & lsEntityName & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Richmond.WriteToStatusBar("Adding Relations for Entity, '" & lsEntityName & "' to Page", True)

                    While Not lrORMRecordset.EOF

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString '(Entity, Relation)
                        lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString  '(ERDEntity, Multiplicity)
                        lsSQLQuery &= " WHERE ERDRelation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString '(ERDEntity, Multiplicity)
                        lsSQLQuery &= " WHERE ERDRelation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString '(Relation, FactType)
                        lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        '        lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                        '        lsSQLQuery &= " (OriginIsMandatory)"
                        '        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        '        lsSQLQuery &= " VALUES ("
                        '        lsSQLQuery &= "'" & lsRelationId & "'"
                        '        lsSQLQuery &= " )"

                        '        'lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                        '        'lsSQLQuery &= " (" & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString & ")"
                        '        'lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
                        '        'lsSQLQuery &= " VALUES ("
                        '        'lsSQLQuery &= "'" & lsRelationId & "'"
                        '        'lsSQLQuery &= " )"


                        '=================================================================
                        'Origin/Destination Predicates
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString '(Relation, Predicate)
                        lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE Relation = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset2.Facts.Count > 0 Then
                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        End If

                        '===================================================================
                        'Origin/Destination Mandatory requirements
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE OriginIsMandatory = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        While Not lrORMRecordset2.EOF
                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrORMRecordset2.MoveNext()
                        End While

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lrORMRecordset("Relation").Data & "'"

                        lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        While Not lrORMRecordset2.EOF
                            lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrORMRecordset2.MoveNext()
                        End While

                        lrORMRecordset.MoveNext()
                    End While

                Next 'lsEntityName
                '============================================================================================================================================

                Richmond.WriteToStatusBar("Completed creating the Entity Relationship Diagram, '" & lrPage.Name & "'")

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

        ''' <summary>
        ''' Removes the CMML Facts relating to the Attribute that is being deleted from the Page.
        ''' </summary>
        ''' <param name="asAttributeId"></param>
        ''' <remarks></remarks>
        Public Sub DeleteERDElementsByAttributeId(ByVal asAttributeId As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            lsSQLQuery &= " WHERE Attribute = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM CorePropertyHasPropertyName" '(Property, PropertyName)
            lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM CorePropertyIsForFactType" '(Property, FactType)"
            lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM CorePropertyIsForRole" '(Property, Role)
            lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString '(Attribute, Position)
            lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '--------------------------------------------
            'Check to see if the Attribute is Mandatory
            '--------------------------------------------
            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString '(Attribute, Position)
            lsSQLQuery &= " WHERE IsMandatory = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelation.ToString '(Attribute, Position)
            lsSQLQuery &= " WHERE Attribute = '" & asAttributeId & "'"

            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Sub DropEntityAtPoint(ByRef arEntityInstance As ERD.Entity, ByVal aoPointF As PointF)

            Dim lsSQLQuery As String
            Dim lrFactInstance As FBM.FactInstance

            Try

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " (" & pcenumCMML.Element.ToString
                lsSQLQuery &= " , " & pcenumCMML.ElementType.ToString & ")"
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arEntityInstance.Name & "'"
                lsSQLQuery &= ",'" & pcenumCMML.Entity.ToString & "'"
                lsSQLQuery &= ")"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arEntityInstance = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneEntity(Me)

                arEntityInstance.X = aoPointF.X
                arEntityInstance.Y = aoPointF.Y

                Call arEntityInstance.DisplayAndAssociate()

                Call Me.MakeDirty()
                Call Me.Save()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DropExistingEntityAtPoint(ByRef arEntityInstance As ERD.Entity, ByVal aoPointF As PointF, Optional ByVal abSavePage As Boolean = True)

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactInstance As FBM.FactInstance
                Dim lrRDSTable As RDS.Table

                lrRDSTable = arEntityInstance.RDSTable
                '==================================================================================================================

                Dim lrFactDataInstance As FBM.FactDataInstance
                arEntityInstance.X = aoPointF.X
                arEntityInstance.Y = aoPointF.Y

                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                arEntityInstance.DisplayAndAssociate()

                Me.ERDiagram.Entity.AddUnique(arEntityInstance)

                '=====================
                'Load the Attributes
                '=====================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreERDAttribute"
                lsSQLQuery &= " WHERE ModelObject = '" & arEntityInstance.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrERAttribute As ERD.Attribute
                Dim lrRecordset1 As ORMQL.Recordset

                While Not lrRecordset.EOF

                    Dim lsMandatory As String = ""

                    lrERAttribute = New ERD.Attribute With {
                        .Id = lrRecordset("Attribute").Data
                    }

                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery) 'lrRecordset("Attribute")

                    lrFactDataInstance = lrFactInstance.GetFactDataInstanceByRoleName("Attribute")
                    lrERAttribute = lrFactDataInstance.CloneAttribute(Me)

                    '-------------------------------
                    'Get the Name of the Attribute
                    '-------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrERAttribute.AttributeName = lrRecordset1("PropertyName").Data

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '-------------------------------------------------
                    'Check to see whether the Attribute is Mandatory
                    '-------------------------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreIsMandatory"
                    lsSQLQuery &= " WHERE IsMandatory = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset1.Facts.Count = 1 Then
                        lrERAttribute.Mandatory = True
                        lsMandatory = "*"

                        lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    'lrERAttribute.OrdinalPosition = CInt(lrRecordset1("Position").Data)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '=============
                    'Role
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    '=============
                    'FactType
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    '---------------------------------------------------
                    'Add the Attribute to the ER Entity
                    '---------------------------------------------------
                    lrERAttribute.Entity = arEntityInstance

                    arEntityInstance.Attribute.AddUnique(lrERAttribute)
                    Me.ERDiagram.Attribute.AddUnique(lrERAttribute)

                    Dim lrColumn As RDS.Column = arEntityInstance.RDSTable.Column.Find(Function(x) x.Id = lrERAttribute.Id)
                    lrERAttribute.Column = lrColumn

                    lrERAttribute.ActiveRole = lrColumn.ActiveRole
                    lrERAttribute.ResponsibleRole = lrColumn.Role

                    lrRecordset.MoveNext()
                End While

                '-------------------------------------------------------------------
                'Paint the sorted Attributes (By Ordinal Position) for each Entity
                '-------------------------------------------------------------------
                arEntityInstance.Attribute.Sort(AddressOf ERD.Attribute.ComparerOrdinalPosition)

                For Each lrERAttribute In arEntityInstance.Attribute
                    arEntityInstance.TableShape.RowCount += 1
                    lrERAttribute.Cell = arEntityInstance.TableShape.Item(0, arEntityInstance.TableShape.RowCount - 1)
                    arEntityInstance.TableShape.Item(0, arEntityInstance.TableShape.RowCount - 1).Tag = lrERAttribute
                    Call lrERAttribute.RefreshShape()
                Next

                arEntityInstance.TableShape.ResizeToFitText(False)

                Call Me.MakeDirty()
                If abSavePage Then
                    Call Me.Save()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DropExistingPGSNodeAtPoint(ByRef arPGSNode As PGS.Node, ByVal aoPointF As PointF)

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactInstance As FBM.FactInstance
                Dim lrRDSTable As RDS.Table

                lrRDSTable = arPGSNode.RDSTable
                '==================================================================================================================

                Dim lrFactDataInstance As FBM.FactDataInstance
                arPGSNode.X = aoPointF.X
                arPGSNode.Y = aoPointF.Y

                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                arPGSNode.DisplayAndAssociate()

                Me.ERDiagram.Entity.AddUnique(arPGSNode)

                '=====================
                'Load the Attributes
                '=====================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreERDAttribute"
                lsSQLQuery &= " WHERE ModelObject = '" & arPGSNode.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrRecordset1 As ORMQL.Recordset

                While Not lrRecordset.EOF

                    Dim lsMandatory As String = ""

                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery) 'lrRecordset("Attribute")

                    lrFactDataInstance = lrFactInstance.GetFactDataInstanceByRoleName("Attribute")

                    '-------------------------------
                    'Get the Name of the Attribute
                    '-------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '-------------------------------------------------
                    'Check to see whether the Attribute is Mandatory
                    '-------------------------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreIsMandatory"
                    lsSQLQuery &= " WHERE IsMandatory = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset1.Facts.Count = 1 Then
                        lsMandatory = "*"

                        lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '=============
                    'Role
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    '=============
                    'FactType
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    lrRecordset.MoveNext()
                End While

                Call Me.MakeDirty()
                Call Me.Save()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ExistsDataStore(ByVal asProcessName As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            ExistsDataStore = False

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE Element = '" & asProcessName & "'"
            lsSQLQuery &= "   AND ElementType = 'DataStore'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then
                ExistsDataStore = True
            End If

        End Function

        Public Function isCMMLTableOnPage(ByVal asTableName As String) As Boolean

            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE ElementType = 'Entity'"
            lsSQLQuery &= " AND Element = '" & asTableName & "'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Return Not lrRecordset.EOF


        End Function

        Public Function loadCMMLRelation(ByRef arRelation As RDS.Relation,
                                         Optional ByRef arOriginEntity As FBM.FactDataInstance = Nothing,
                                         Optional ByRef arDestinationEntity As FBM.FactDataInstance = Nothing) As ERD.Relation

            Try
                Call Me.addRDSRelation(arRelation)

                Dim lrERDRelation As New ERD.Relation(Me.Model,
                                                      Me,
                                                      arRelation.Id,
                                                      arOriginEntity,
                                                      arRelation.OriginMultiplicity,
                                                      arRelation.RelationOriginIsMandatory,
                                                      False,
                                                      arDestinationEntity,
                                                      arRelation.DestinationMultiplicity,
                                                      arRelation.RelationDestinationIsMandatory)

                lrERDRelation.RDSRelation = arRelation 'Me.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)
                lrERDRelation.RelationFactType = arRelation.ResponsibleFactType

                Return lrERDRelation

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' PRECONDITION: Relations are already created in the RDS Model of the FBM.Model. i.e. Is not used to create relationships, but load them onto a Page.
        ''' </summary>
        ''' <param name="arEntity"></param>
        ''' <remarks></remarks>
        Public Sub loadRelationsForEntity(ByRef arEntity As ERD.Entity, Optional abSavePage As Boolean = True)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset
            Dim lrRecordset2 As ORMQL.Recordset

            Dim lrFactInstance As New FBM.FactInstance

            Dim lasRelationId As New List(Of String) 'List of the Relations loaded onto the Page. Used so that a relation that links an Entity to itself is not loaded twice.

            '20180609-VM-ToDo-lrFactInstance is used for a Link (below) but set to nothing in particular (above).

            For liInd As Integer = 1 To 2

                lsSQLQuery = "SELECT *"
                If liInd = 1 Then
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                Else
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                End If
                lsSQLQuery &= " WHERE Entity = '" & arEntity.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrOrigingEREntity As ERD.Entity
                Dim lrDestinationgEREntity As ERD.Entity
                Dim lsRelationId As String = ""

                While Not lrRecordset.EOF
                    '------------------------
                    'Find the Origin Entity
                    '------------------------
                    lsRelationId = lrRecordset("Relation").Data

                    If liInd = 1 Then
                        lrOrigingEREntity = New ERD.Entity
                        lrDestinationgEREntity = New ERD.Entity
                        lrOrigingEREntity.Symbol = lrRecordset("Entity").Data
                        lrOrigingEREntity = Me.ERDiagram.Entity.Find(AddressOf lrOrigingEREntity.EqualsBySymbol)

                        '-----------------------------
                        'Find the Destination Entity
                        '-----------------------------
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrDestinationgEREntity.Symbol = lrRecordset1("Entity").Data

                        lrDestinationgEREntity = Me.ERDiagram.Entity.Find(AddressOf lrDestinationgEREntity.EqualsBySymbol)
                    Else '=2
                        lrOrigingEREntity = New ERD.Entity
                        lrDestinationgEREntity = New ERD.Entity
                        lrDestinationgEREntity.Symbol = lrRecordset("Entity").Data
                        lrDestinationgEREntity = Me.ERDiagram.Entity.Find(AddressOf lrDestinationgEREntity.EqualsBySymbol)

                        '-----------------------------
                        'Find the Destination Entity
                        '-----------------------------
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrOrigingEREntity.Symbol = lrRecordset1("Entity").Data
                        lrOrigingEREntity = Me.ERDiagram.Entity.Find(AddressOf lrOrigingEREntity.EqualsBySymbol)
                    End If

                    If (lrOrigingEREntity IsNot Nothing) And (lrDestinationgEREntity IsNot Nothing) And Not lasRelationId.Contains(lsRelationId) Then

                        lasRelationId.Add(lsRelationId)

                        Dim loOriginTableNode As ERD.TableNode = lrOrigingEREntity.TableShape
                        Dim loDestinationTableNode As ERD.TableNode = lrDestinationgEREntity.TableShape

                        lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                        If liInd = 1 Then
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                        Else
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                        End If
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                        If liInd = 1 Then
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString

                        Else
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                        End If
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString  '(ERDEntity, Multiplicity)
                        lsSQLQuery &= " WHERE ERDRelation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        Dim liOriginMultiplicity As pcenumCMMLMultiplicity
                        Call liOriginMultiplicity.GetByDescription(lrRecordset2("Multiplicity").Data)

                        lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString '(ERDEntity, Multiplicity)
                        lsSQLQuery &= " WHERE ERDRelation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
                        Call liDestinationMultiplicity.GetByDescription(lrRecordset2("Multiplicity").Data)

                        lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString '(Relation, FactType)
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        '===================================================================
                        'Origin/Destination Mandatory requirements
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE OriginIsMandatory = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        Dim lbRelationOriginIsMandatory As Boolean = lrRecordset2.Facts.Count > 0

                        While Not lrRecordset2.EOF
                            lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrRecordset2.MoveNext()
                        End While

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        Dim lbRelationDestinationIsMandatory As Boolean = lrRecordset2.Facts.Count > 0

                        While Not lrRecordset2.EOF
                            lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrRecordset2.MoveNext()
                        End While

                        '----------------------------------------------------------------------------
                        'Create the Relation for the ERD
                        'ToDo: Needs ' lbcontributestoprimarykey
                        Dim lrRelation As New ERD.Relation(Me.Model,
                                                           Me,
                                                           lsRelationId,
                                                           lrOrigingEREntity,
                                                           liOriginMultiplicity,
                                                           lbRelationOriginIsMandatory,
                                                           False,
                                                           lrDestinationgEREntity,
                                                           liDestinationMultiplicity,
                                                           lbRelationDestinationIsMandatory)

                        '=================================================================

                        'Origin/Destination Predicates
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString '(Relation, Predicate)
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRelation.OriginPredicate = lrRecordset2("Predicate").Data


                        lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString  '(Relation, Predicate)
                        lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRelation.DestinationPredicate = lrRecordset2("Predicate").Data

                        If lrRecordset2.Facts.Count > 0 Then
                            lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        End If

                        '-----------------------------------
                        'Get the FactType for the Relation
                        '-----------------------------------
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        Dim lrFactType As New FBM.FactType(Me.Model, lrRecordset2("FactType").Data, True)
                        lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.Equals)
                        lrRelation.RelationFactType = lrFactType

                        lsSQLQuery = "ADD FACT '" & lrRecordset2.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        lrRelation.RDSRelation = Me.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)

                        Me.ERDiagram.Relation.Add(lrRelation)

                        Dim lrLink As New ERD.Link(arEntity.Page, lrFactInstance, lrOrigingEREntity, lrDestinationgEREntity, Nothing, Nothing, lrRelation)
                        lrLink.DisplayAndAssociate()

                        '==========================================================================================================================
                        '==========================================================================================================================
                        'Dim loOriginTableNode As ERD.TableNode = lrOrigingEREntity.TableShape
                        'Dim loDestinationTableNode As ERD.TableNode = lrDestinationgEREntity.TableShape

                        ''================================
                        'lsSQLQuery = "SELECT *"
                        'lsSQLQuery &= " FROM CoreOriginMultiplicity"
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lsOriginMultiplicity As String = lrRecordset1("Multiplicity").Data

                        'Dim liOriginMultiplicity As pcenumCMMLMultiplicity
                        'Select Case lsOriginMultiplicity
                        '    Case Is = pcenumCMMLMultiplicity.One.ToString
                        '        liOriginMultiplicity = pcenumCMMLMultiplicity.One
                        '    Case Is = pcenumCMMLMultiplicity.Many.ToString
                        '        liOriginMultiplicity = pcenumCMMLMultiplicity.Many
                        'End Select

                        'lsSQLQuery = "SELECT COUNT(*)"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE OriginIsMandatory = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lbRelationOriginIsMandatory As Boolean = False
                        'If CInt(lrRecordset1("Count").Data) > 0 Then
                        '    lbRelationOriginIsMandatory = True
                        'End If

                        ''================================
                        'lsSQLQuery = "SELECT *"
                        'lsSQLQuery &= " FROM CoreDestinationMultiplicity"
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE ERDRelation = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lsDestinationMultiplicity As String = lrRecordset1("Multiplicity").Data

                        'Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
                        'Select Case lsDestinationMultiplicity
                        '    Case Is = pcenumCMMLMultiplicity.One.ToString
                        '        liDestinationMultiplicity = pcenumCMMLMultiplicity.One
                        '    Case Is = pcenumCMMLMultiplicity.Many.ToString
                        '        liDestinationMultiplicity = pcenumCMMLMultiplicity.Many
                        'End Select

                        'lsSQLQuery = "SELECT COUNT(*)"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE DestinationIsMandatory = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lbRelationDestinationIsMandatory As Boolean = False
                        'If CInt(lrRecordset1("Count").Data) > 0 Then
                        '    lbRelationDestinationIsMandatory = True
                        'End If

                        ''-------------------------------------------------------------------------------
                        ''Check to see whether the Relation contributes to the PrimaryKey of the Entity
                        ''-------------------------------------------------------------------------------
                        'lsSQLQuery = "SELECT COUNT(*)"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & " = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lbContributesToPrimaryKey As Boolean = False

                        'If CInt(lrRecordset1("Count").Data) > 0 Then
                        '    lbContributesToPrimaryKey = True
                        'End If

                        'Dim lrRelation As New ERD.Relation(Me.Model,
                        '                                   lrRecordset("Relation").Data,
                        '                                   lrOrigingEREntity, _
                        '                                   liOriginMultiplicity, _
                        '                                   lbRelationOriginIsMandatory, _
                        '                                   lbContributesToPrimaryKey, _
                        '                                   lrDestinationgEREntity, _
                        '                                   liDestinationMultiplicity, _
                        '                                   lbRelationDestinationIsMandatory)

                        ''-------------------------------------
                        ''Get the Predicates for the Relation
                        ''-------------------------------------
                        'lsSQLQuery = "SELECT *"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        'If Not lrRecordset1.EOF Then
                        '    lrRelation.OriginPredicate = lrRecordset1("Predicate").Data
                        'End If


                        'lsSQLQuery = "SELECT *"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        'If Not lrRecordset1.EOF Then
                        '    lrRelation.DestinationPredicate = lrRecordset1("Predicate").Data
                        'End If

                        ''-----------------------------------
                        ''Get the FactType for the Relation
                        ''-----------------------------------
                        'lsSQLQuery = "SELECT *"
                        'lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        'lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                        'lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        'lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        'Dim lrFactType As New FBM.FactType(Me.Model, lrRecordset1("FactType").Data, True)
                        'lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.Equals)
                        'lrRelation.RelationFactType = lrFactType

                        'Me.ERDiagram.Relation.Add(lrRelation)

                        'Dim lrLink As New ERD.Link(arEntity.Page, lrFactInstance, lrOrigingEREntity, lrDestinationgEREntity, Nothing, Nothing, lrRelation)
                        'lrLink.DisplayAndAssociate()
                    End If

                    lrRecordset.MoveNext()
                End While

            Next 'From/To arEntity

            Call Me.MakeDirty()
            If abSavePage Then
                Call Me.Save()
            End If

        End Sub

        Public Sub loadPropertyRelationsForPGSNode(ByRef arPGSNode As PGS.Node,
                                                   Optional ByVal abAddToPage As Boolean = False)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset2 As ORMQL.Recordset
            Dim lrOriginatingNode As PGS.Node
            Dim lsRelationId As String = Nothing

            Try
                Dim larTable = From Table In Me.Model.RDS.Table
                               Where Table.isPGSRelation = True
                               Select Table

                For Each lrTable In larTable

                    lrOriginatingNode = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

                    If lrOriginatingNode Is Nothing Then

                        'Need to add the RelationNode to the Page

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                        lsSQLQuery &= " WHERE Entity = '" & lrTable.Name & "'"

                        lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        Dim lasLoadedRelations As New List(Of String)
                        Dim liInd As Integer = 1
                        Dim lrNode1 As PGS.Node = Nothing
                        Dim lrNode2 As PGS.Node = Nothing

                        While Not lrRecordset.EOF

                            lsRelationId = lrRecordset("Relation").Data

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                            lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                            lrRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            If liInd = 1 Then
                                lrNode2 = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
                            Else
                                lrNode1 = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrRecordset2("Entity").Data)
                            End If

                            liInd += 1

                            lrRecordset.MoveNext()
                        End While

                        If (lrNode1 IsNot Nothing) And (lrNode2 IsNot Nothing) Then

                            Call Me.addRDSTableToPage(lrTable) 'Because we want this Table on this Page going forward.

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                            lsSQLQuery &= " WHERE ElementType = 'Entity'"
                            lsSQLQuery &= " AND Element = '" & lrTable.Name & "'"

                            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            Dim lrFactDataInstance As FBM.FactDataInstance = lrRecordset("Element")
                            lrOriginatingNode = lrFactDataInstance.ClonePGSNode(Me)
                            lrOriginatingNode.RDSTable = lrTable

                            Me.ERDiagram.Entity.Add(lrOriginatingNode)

                            Dim lrRelation As New ERD.Relation(Me.Model,
                                                               Me,
                                                               lsRelationId,
                                                               lrNode1,
                                                               pcenumCMMLMultiplicity.One,
                                                               False,
                                                               False,
                                                               lrNode2,
                                                               pcenumCMMLMultiplicity.One,
                                                               False)

                            lrRelation.IsPGSRelationNode = True
                            lrRelation.ActualPGSNode = Me.ERDiagram.Entity.Find(Function(x) x.Id = lrOriginatingNode.Id)

                            'NB Even though the RDSRelation is stored against the Link (below), the Predicates for the Link come from the ResponsibleFactType.
                            '  because the relation is actually a PGSRelationNode.
                            Dim lrRDSRelation As RDS.Relation = Me.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)
                            lrRelation.RelationFactType = lrRDSRelation.ResponsibleFactType

                            If abAddToPage Then Call Me.addRDSRelation(lrRDSRelation)

                            Dim lrLink As PGS.Link
                            lrLink = New PGS.Link(Me, New FBM.FactInstance, lrNode1, lrNode2, Nothing, Nothing, lrRelation)
                            lrLink.RDSRelation = lrRDSRelation
                            lrLink.DisplayAndAssociate()
                            lrLink.Link.Text = lrRelation.ActualPGSNode.Id

                            Me.ERDiagram.Relation.Add(lrRelation)

                        End If

                    End If

                Next 'Table that is a PGSRelationNode


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub loadRelationsForPGSNode(ByRef arPGSNode As PGS.Node,
                                           Optional ByVal abAddToPage As Boolean = False)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            Dim lrFactInstance As New FBM.FactInstance

            Try
                Dim lasRelationId As New List(Of String) 'List of the Relations loaded onto the Page. Used so that a relation that links an Entity to itself is not loaded twice.

                '20180609-VM-ToDo-lrFactInstance is used for a Link (below) but set to nothing in particular (above).

                For liInd As Integer = 1 To 2

                    lsSQLQuery = "SELECT *"
                    If liInd = 1 Then
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                    Else
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                    End If
                    lsSQLQuery &= " WHERE Entity = '" & arPGSNode.Name & "'"

                    lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrOrigingEREntity As PGS.Node
                    Dim lrDestinationgEREntity As PGS.Node
                    Dim lsRelationId As String = ""

                    While Not lrRecordset.EOF
                        '------------------------
                        'Find the Origin Entity
                        '------------------------
                        lsRelationId = lrRecordset("Relation").Data

                        If liInd = 1 Then
                            lrOrigingEREntity = New PGS.Node
                            lrDestinationgEREntity = New PGS.Node
                            lrOrigingEREntity.Symbol = lrRecordset("Entity").Data
                            lrOrigingEREntity = Me.ERDiagram.Entity.Find(AddressOf lrOrigingEREntity.EqualsBySymbol)

                            '-----------------------------
                            'Find the Destination Entity
                            '-----------------------------
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                            lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                            lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrDestinationgEREntity.Symbol = lrRecordset1("Entity").Data

                            lrDestinationgEREntity = Me.ERDiagram.Entity.Find(AddressOf lrDestinationgEREntity.EqualsBySymbol)
                        Else '=2
                            lrOrigingEREntity = New PGS.Node
                            lrDestinationgEREntity = New PGS.Node
                            lrDestinationgEREntity.Symbol = lrRecordset("Entity").Data
                            lrDestinationgEREntity = Me.ERDiagram.Entity.Find(AddressOf lrDestinationgEREntity.EqualsBySymbol)

                            '-----------------------------
                            'Find the Destination Entity
                            '-----------------------------
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                            lsSQLQuery &= " WHERE Relation = '" & lsRelationId & "'"

                            lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrOrigingEREntity.Symbol = lrRecordset1("Entity").Data
                            lrOrigingEREntity = Me.ERDiagram.Entity.Find(AddressOf lrOrigingEREntity.EqualsBySymbol)
                        End If

                        If (lrOrigingEREntity IsNot Nothing) And (lrDestinationgEREntity IsNot Nothing) And Not lasRelationId.Contains(lsRelationId) Then

                            lasRelationId.Add(lsRelationId)

                            Dim lrRDSRelation As RDS.Relation = Me.Model.RDS.Relation.Find(Function(x) x.Id = lsRelationId)

                            Dim lrRelation As ERD.Relation
                            If Me.ERDiagram.Relation.FindAll(Function(x) x.Id = lsRelationId).Count = 0 Then
                                lrRelation = Me.loadCMMLRelation(lrRDSRelation, lrOrigingEREntity, lrDestinationgEREntity)
                                Me.ERDiagram.Relation.Add(lrRelation)
                            Else
                                lrRelation = Me.ERDiagram.Relation.Find(Function(x) x.Id = lsRelationId)
                            End If

                            If abAddToPage Then Me.addRDSRelation(lrRDSRelation)

                            Dim lrLink As PGS.Link
                            lrLink = New PGS.Link(Me, lrFactInstance, lrOrigingEREntity, lrDestinationgEREntity, Nothing, Nothing, lrRelation)
                            lrLink.DisplayAndAssociate()

                        End If

                        lrRecordset.MoveNext()
                    End While

                Next 'From/To arPGSNode

                Call Me.MakeDirty()
                Call Me.Save()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            End Try

        End Sub

        Public Sub removeCMMLAttribute(ByRef arAttribute As ERD.Attribute)

            '-------------------------------------------------------------------------------------------------------------------
            'Remove the Attributes of the Entity
            '-------------------------------------
            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM CoreIsMandatory"
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE IsMandatory = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Attribute = '" & arAttribute.Id & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub

        ''' <summary>
        ''' NB Not used, because Index information for ERDs/PGSs are retrieved from the RDS level (in memory).
        ''' i.e. Indexes are removed at the CMML Model level, but no need at the Page level.
        ''' 20180714-VM-This code is for reference only and can probably be removed at a later stage.
        ''' </summary>
        ''' <param name="arEntity"></param>
        ''' <remarks></remarks>
        Public Sub removeCMMLIndexesForEntity(ByRef arEntity As ERD.Entity)

            Dim lrRecordset As ORMQL.Recordset
            Dim lsSQLQuery As String

            '================================
            '----------------------------------------------
            'Remove any unique identifiers for the Entity
            '----------------------------------------------
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Entity = '" & arEntity.Name & "'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            While Not lrRecordset.EOF
                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " WHERE Index = '" & lrRecordset("Index").Data & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrRecordset.MoveNext()
            End While

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
            lsSQLQuery &= " WHERE Entity = '" & arEntity.Name & "'"
            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            '================================

        End Sub

        Public Sub removeRelationsForEntity(ByRef arEntity As FBM.FactDataInstance)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lsRelationId As String = ""

            Try
                Dim lrCoreRelationEntityTypeInstance As FBM.EntityTypeInstance = Me.EntityTypeInstance.Find(Function(x) x.Id = "CoreRelation")

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " WHERE Entity = '" & arEntity.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF
                    lsRelationId = lrRecordset("Relation").Data

                    Call lrCoreRelationEntityTypeInstance.removeInstance(lsRelationId)
                    lrRecordset.MoveNext()
                End While


                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " WHERE Entity = '" & arEntity.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF
                    lsRelationId = lrRecordset("Relation").Data
                    Call lrCoreRelationEntityTypeInstance.removeInstance(lsRelationId)

                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Private Sub _Model_RDSColumnAdded(ByRef arColumn As RDS.Column) Handles _Model.RDSColumnAdded

            Dim lsSQLQuery As String = ""
            Dim lrORMRecordset As ORMQL.Recordset
            Dim lrORMRecordset2 As ORMQL.Recordset
            Dim lrColumn As RDS.Column
            Dim lrERAttribute As ERD.Attribute
            Dim lrEREntity As ERD.Entity
            Dim lrFactDataInstance As FBM.FactDataInstance

            If Me.Language = pcenumLanguage.ORMModel Then
                Exit Sub
            End If

            lrColumn = arColumn

            lrEREntity = Me.ERDiagram.Entity.Find(Function(x) x.Name = lrColumn.Table.Name)

            If lrEREntity IsNot Nothing Then
                '------------------------------------------------------------------------------------------------
                'The Entity for the Column is on the Page.

                '=====================
                'Load the Attribute
                '=====================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreERDAttribute"
                lsSQLQuery &= " WHERE Attribute = '" & lrColumn.Id & "'"

                lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '=====================================================================================================================================
                '=====================================================================================================================================
                Dim lrFactInstance As FBM.FactInstance

                lsSQLQuery = "ADD FACT '" & lrORMRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrERAttribute = New ERD.Attribute

                lrFactDataInstance = lrFactInstance.GetFactDataInstanceByRoleName("Attribute")
                lrERAttribute = lrFactDataInstance.CloneAttribute(Me)

                '-------------------------------
                'Get the Name of the Attribute
                '-------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrERAttribute.AttributeName = lrORMRecordset2("PropertyName").Data

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Get the FactType for the Attribute
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyIsForFactType" '(Property, FactType)"
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Get the Role for the Attribute
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CorePropertyIsForRole" '(Property, Role)
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrERAttribute.ResponsibleRole = Me.Model.Role.Find(Function(x) x.Id = lrORMRecordset2("Role").Data)
                lrERAttribute.ResponsibleFactType = lrERAttribute.ResponsibleRole.FactType

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Get the OrdinalPosition for the Attribute
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString '(Attribute, Position)
                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'lrERAttribute.OrdinalPosition = lrORMRecordset2("Position").Data

                lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Check to see if the Attribute is Mandatory
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString '(Attribute, Position)
                lsSQLQuery &= " WHERE IsMandatory = '" & lrColumn.Id & "'"

                lrORMRecordset2 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If Not lrORMRecordset2.EOF Then
                    lsSQLQuery = "ADD FACT '" & lrORMRecordset2.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrERAttribute.Mandatory = True
                End If

                '--------------------------------------------------------
                'Check to see whether the Entity has a PrimaryKey
                '--------------------------------------------------------
                'lrERAttribute.PartOfPrimaryKey = True
                'lrEREntity.PrimaryKey.Add(lrERAttribute)
                '=====================================================================================================================================
                '====================================================================================================================================

                '---------------------------------------------------
                'Add the Attribute to the Entity
                '---------------------------------------------------
                lrERAttribute.Entity = New ERD.Entity
                lrERAttribute.Entity = lrEREntity

                lrEREntity.Attribute.Add(lrERAttribute)
                Me.ERDiagram.Attribute.Add(lrERAttribute)

                '=============================================================================================
                'Paint the sorted Attributes (By Ordinal Position) for each Entity
                '-------------------------------------------------------------------
                lrEREntity.Attribute.Sort(AddressOf ERD.Attribute.ComparerOrdinalPosition)


                lrEREntity.TableShape.RowCount += 1
                lrERAttribute.Cell = lrEREntity.TableShape.Item(0, lrEREntity.TableShape.RowCount - 1)
                lrEREntity.TableShape.Item(0, lrEREntity.TableShape.RowCount - 1).Tag = lrERAttribute
                Call lrERAttribute.RefreshShape()

                lrEREntity.TableShape.ResizeToFitText(False)
                '=============================================================================================


            End If 'The Entity for the Column is on the Page.

        End Sub '_Model_RDSColumnAdded

    End Class

End Namespace
