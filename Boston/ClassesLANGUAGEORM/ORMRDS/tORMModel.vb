Imports System.Reflection
Imports System.Threading.Tasks
Imports System.Threading

Namespace FBM

    Partial Public Class Model


        Public RDSCreated As Boolean = False 'True after the RDF has been created for the Model. e.g. Once the first ERD has been created for the Model.

        ''' <summary>
        ''' Adds the required Core CMML Elements for an ERD Attribute based on the given Role.
        ''' PreConditions: It is already established that the Role is an ERD Property Role.
        ''' </summary>
        ''' <param name="arRole"></param>
        ''' <remarks></remarks>
        Public Sub aaAddERDAttributeForRole(ByRef arRole As FBM.Role)

            '20251018-VM-This method isn't called from anywhere. Delete this method after 6 months if not missed.

            'Testing SVN. Should pick up this line added to the file.
            '=======================
            'Create the Attributes
            '=======================
            Dim lrEntity As New ERD.Entity()
            Dim lrAttribute As New ERD.Attribute("DummyId", lrEntity)
            Dim lsEntityName As String = ""
            Dim lsAttributeName As String = ""
            Dim lsSQLQuery As String = ""
            Dim lrTable As RDS.Table
            Dim lrColumn As New RDS.Column

            Try
                lsEntityName = arRole.BelongsToTable
                lsAttributeName = arRole.GetAttributeName

                '--------------------------------------------------------------
                'Get the underlying ModelElement
                Dim lrModelElement As FBM.ModelObject = Me.GetModelObjectByName(lsEntityName)

                lrEntity.Name = lsEntityName
                lrAttribute.AttributeName = lsAttributeName

                lsAttributeName = Me.CreateUniquePropertyName(lrAttribute, 0)

                lrColumn.Name = lsAttributeName

                '---------------------------------------------------------------------
                'Check to see that the Entity (already) exists in the ERD MetaModel.
                '  If the Entity doesn't exist, create it.
                '---------------------------------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & lsEntityName & "'"

                Dim lrORMRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrORMRecordset.EOF Then
                    '---------------------------------------------------------------
                    'The Entity does not exist in the ERD MetaModel, so create it.
                    '---------------------------------------------------------------
                    Boston.WriteToStatusBar("Creating Entity, '" & lsEntityName & "'")

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " (Element, ElementType)"
                    lsSQLQuery &= " VALUES ('" & lsEntityName & "', 'Entity')"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrTable = New RDS.Table(Me.RDS, lsEntityName, lrModelElement)
                    Me.RDS.Table.AddUnique(lrTable)
                Else
                    lrColumn.Table = Me.RDS.Table.Find(Function(x) x.Name = lsEntityName)
                End If

                '======================
                'Create the Attribute
                '======================
                '----------------------------------------------------------------------------------------------------
                'First create the Property for the Attribute.
                '  Properties themselves are GUIDs with a PropertyName.
                '  The reason why Properties are unique and non-SelfDescriptive (i.e. The name of the Property is not
                '  the Id of the Property) is because multiple Entities may have Attributes with the same name.
                '  So each Entity links to its Properties/Attributes by their Id (i.e. EntityType Instance) and by
                '  default have a Property/Attribute Name.
                '----------------------------------------------------------------------------------------------------
                Dim lsPropertyInstanceId As String

                lsPropertyInstanceId = System.Guid.NewGuid.ToString

                lrColumn.Id = lsPropertyInstanceId

                lsSQLQuery = "INSERT INTO CorePropertyHasPropertyName (Property, PropertyName)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " ,'" & lsAttributeName & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                lsSQLQuery = "INSERT INTO CorePropertyIsForFactType (Property, FactType)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " ,'" & arRole.FactType.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CorePropertyIsForRole (Property, Role)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " ,'" & arRole.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '---------------------------------------------
                'Create the Attribute against the Entity
                '---------------------------------------------
                lsSQLQuery = "INSERT INTO CoreERDAttribute (ModelObject, Attribute)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & lsEntityName & "'"
                lsSQLQuery &= " ,'" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------------
                'Set the Ordinal Position of the Attribute
                '--------------------------------------------------
                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " WHERE ModelObject = '" & lsEntityName & "'"

                Dim lrCountRecordset As New ORMQL.Recordset

                lrCountRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " (Property, Position)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & lsPropertyInstanceId & "'"
                lsSQLQuery &= ",'" & lrCountRecordset("Count").Data & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '--------------------------------------------
                'Check to see if the Attribute is Mandatory
                '--------------------------------------------
                If arRole.Mandatory Then

                    Boston.WriteToStatusBar("Creating MandatoryConstraint for Attribute, '" & lsAttributeName & "', for Entity, '" & lsEntityName & "'", True)

                    lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'" 'lsAttributeName & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                '=========================================================================
                'Check if the Role represents the PK of an Entity with a SimpleReferenceScheme
                Dim lsIndexName As String = ""
                lsIndexName = FEStrings.ProperSpace(lrAttribute.Entity.Id & "PK")

                If arRole.IsERDSimpleReferenceSchemePKRole _
                    Or arRole.IsERDPKRoleOfObjectifiedFactType Then
                    '-------------------------------------------------
                    'Must create a Primary Identifier for the Entity
                    '-------------------------------------------------
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                    lsSQLQuery &= " (Entity, Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrAttribute.Entity.Id & "'"
                    lsSQLQuery &= ",'" & lsIndexName & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '-------------------------------------
                    'Add the Attribute to the PrimaryKey
                    '-------------------------------------
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                    lsSQLQuery &= " (Index, Property)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lsIndexName & "'"
                    lsSQLQuery &= ",'" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If
                '=========================================================================

                RaiseEvent RDSColumnAdded(lrColumn)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        ''' <summary>
        ''' Creates the Entity Relationship Diagram fact/data artifacts within the (existing) ERD (Meta)Model.
        ''' Preconditions: The 'Core' set of ERD/PGS ModelObjects have been injected within the Model from the Model called, 'Core'.
        ''' </summary>
        Public Sub createEntityRelationshipArtifacts(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)
            '--------------------------------------------------------------------------------------------------------------------------
            'Pseudocode:
            '
            '  * Follow Halpin's steps to create an ERD from an ORM Diagram to create the Entities, Attributes, Indexes and Relations
            '--------------------------------------------------------------------------------------------------------------------------
            Dim lrFact As New FBM.Fact
            Dim lsEntityName As String = ""
            Dim lsAttributeName As String = ""
            Dim lsSQLQuery As String = ""
            Dim lsTableName As String = ""

            Try
                If Me.ContainsLanguage.Contains(pcenumLanguage.EntityRelationshipDiagram) Then Exit Sub 'Because the Relational model has already been created for the Model.

                '=======================================================
                'Start creating the Entities, Attributes and Relations
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(10)
                Call Me.generateEntityArtifacts()

                '=======================
                'Create the Attributes
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(30)
                Call Me.generateAttributeArtifacts()

                '=============================
                'Create the Indexes               
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(60)
                Call Me.generateERDIndexes()

                '===========================================
                'Create the Relationships between Entities                
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(90)
                Call Me.generateERDRelationships()

                '===========================================================================
                'Move the PrimaryKey Columns of each Table to the topmost OrdinalPositions
                Boston.WriteToStatusBar("Moving Primary Keys to the top of each Enitity.", True)
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(95)
                For Each lrTable In Me.RDS.Table
                    Call lrTable.movePrimaryKeyColumnsToTopOrdinalPosition()
                Next

                Boston.WriteToStatusBar("Completed creating the Entity Relationship Diagram Artifacts", True)
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(100)

                '-------------------------------------------------------------------------------------------------
                'Let the Model know that it now contains the language, EntityRelationshipDiagram.
                '  This is so that automatic processing of ERDs can happen if the ORM Model changes.
                Me.ContainsLanguage.AddUnique(pcenumLanguage.EntityRelationshipDiagram)

                Me.RDSCreated = True

                '---------------------------------
                'Save the new Page to the database
                '---------------------------------
                Me.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub 'CreateEntityRelationshipArtifacts


        Public Sub generateAttributeArtifacts()

            Try
                For Each lrRoleConstraint In Me.RoleConstraint.FindAll(Function(x) x.IsMDAModelElement = False _
                                                                        And x.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                                                                        And Not x.isSubtypeRelationshipFactTypeIUConstraint)

                    Call Me.generateAttributesForRoleConstraint(lrRoleConstraint)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateAttributesForRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            'If lrRole.IsERDPropertyRole Then
            'lsEntityName = lrRole.BelongsToTable

            '--------------------------------------------------------------------------------------------------
            'NB A PropertyRole may actually be for more than one Property/Attribute of the Entity/Table.
            '  The reason is that the Role may be part of a larger RoleGroup and where the Role
            '  references an ObjectifiedFactType that is not Independant/Separate. In that case the referenced
            '  ObjectifiedFactType becomes 'absorbed' by the referencing Role/FactType and the effective 
            '  Properties/Attributes of each of the ObjectifiedFactType's Roles become Properties/Attributes of
            '  the initially referencing Role/FactType/Entity/Table.
            '  i.e. The Role is still a PropertyRole, but has more than on Attribute that must be added to the Entity.
            '--------------------------------------------------------------------------------------------------
            Dim lrRole As FBM.Role
            Dim lrTable As RDS.Table
            Dim lrColumn As RDS.Column
            Dim lsTableName As String = ""
            Dim lrFactType As FBM.FactType

            Try
                If arRoleConstraint.Role(0).FactType.IsLinkFactType Then
                    'Throw New Exception("RoleConstraint, '" & arRoleConstraint.Id & "' not catered for when generating Attribute artifacts.")
                    Exit Sub
                ElseIf arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject.ConceptType = pcenumConceptType.ValueType _
                    And arRoleConstraint.RoleConstraintRole(0).Role.FactType.Is1To1BinaryFactType Then
                    'Do nothing...because is most likely a RoleConstraint on a 1:1 BinaryFactType that represents the Reference Scheme for an EntityType
                    '  and is on the Role that links to the ReferenceModeValueType of an EntityType.
                    Exit Sub
                ElseIf arRoleConstraint.impliesSingleColumnForRDSTable Then

                    If arRoleConstraint.RoleConstraintRole.Count = 1 Then

                        lrRole = arRoleConstraint.RoleConstraintRole(0).Role

                        If lrRole.IsERDPropertyRole Then

                            lsTableName = lrRole.BelongsToTable
                            lrTable = Me.RDS.getTableByName(lsTableName)

                            If lrTable Is Nothing Then
                                'Table not created yet
                                Dim lrModelElement As FBM.ModelObject = Me.GetModelObjectByName(lsTableName)
                                lrTable = New RDS.Table(Me.RDS, lsTableName, lrModelElement)
                                Me.RDS.Table.AddUnique(lrTable)
                            End If

                            lrColumn = lrRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)

                            If lrRole.Mandatory Then
                                lrColumn.IsMandatory = True
                            End If

                            lrTable.addColumn(lrColumn)

                        End If
                    Else
                        Throw New Exception("RoleConstraint can't both imply a single Column and have more than one RoleConstraintRole.")
                    End If

                ElseIf (arRoleConstraint.RoleConstraintRole.Count <> 1) _
                    Or (arRoleConstraint.RoleConstraintRole.Count = 1 And arRoleConstraint.RoleConstraintRole(0).Role.FactType.IsObjectified) Then 'And (arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject.ConceptType <> pcenumConceptType.ValueType) Then
                    'Make Columns for the FactType of the RoleConstraint (InternalUniquenessConstraint)
                    lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType

                    'Must have a column for all of the Roles of the FactType
                    For Each lrRole In lrFactType.RoleGroup

                        lsTableName = lrFactType.Name
                        lrTable = Me.RDS.getTableByName(lsTableName)

                        If lrTable Is Nothing Then
                            'Table not created yet
                            lrTable = New RDS.Table(Me.RDS, lsTableName, lrFactType)
                            Me.RDS.Table.AddUnique(lrTable)
                        End If

                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType

                                If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                    'There is no Column in the Table for the Role.

                                    lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)
                                    '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                                    'If arRoleConstraint.Role.Contains(lrRole) And lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                    '    lrColumn.ContributesToPrimaryKey = True
                                    'End If
                                    If arRoleConstraint.Role.Contains(lrRole) Then
                                        lrColumn.IsMandatory = True
                                    End If
                                    lrTable.addColumn(lrColumn)
                                End If

                            Case Is = pcenumConceptType.EntityType

                                If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                                    'There is no Column in the Table for the Role.
                                    Dim lrEntityType As FBM.EntityType = lrRole.JoinedORMObject

                                    If lrEntityType.HasCompoundReferenceMode Then

                                        Dim larColumn As New List(Of RDS.Column)
                                        Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrRole, larColumn)

                                        For Each lrColumn In larColumn
                                            If arRoleConstraint.Role.Contains(lrRole) Then
                                                lrColumn.IsMandatory = True
                                                '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                                                'If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                                '    lrColumn.ContributesToPrimaryKey = True
                                                'End If
                                            End If
                                            lrTable.addColumn(lrColumn)
                                        Next
                                    Else
                                        lrColumn = lrRole.GetCorrespondingFactTypeColumn(lrTable)
                                        If arRoleConstraint.Role.Contains(lrRole) Then
                                            lrColumn.IsMandatory = True
                                            '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                                            'If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                            '    lrColumn.ContributesToPrimaryKey = True
                                            'End If
                                        End If
                                        lrTable.addColumn(lrColumn)
                                    End If

                                End If

                            Case Else

                                Dim larColumn As New List(Of RDS.Column)

                                larColumn = lrRole.getColumns(lrTable, lrRole)

                                For Each lrColumn In larColumn
                                    If Not lrTable.Column.Exists(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id) Then
                                        'There is no Column in the Table for the Role.
                                        lrColumn.Name = FEStrings.MakeCapCamelCase(FEStrings.ProperSpace(lrColumn.Name))
                                        lrColumn.Name = lrTable.createUniqueColumnName(lrColumn.Name, lrColumn, 0)
                                        If arRoleConstraint.Role.Contains(lrRole) Then
                                            lrColumn.IsMandatory = True
                                            '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                                            'If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                            '    lrColumn.ContributesToPrimaryKey = True
                                            'End If
                                        End If
                                        lrTable.addColumn(lrColumn)
                                    End If
                                Next

                        End Select
                        Dim lrModelElement As FBM.ModelObject = lrRole.JoinedORMObject
                    Next
                ElseIf arRoleConstraint.Role(0).FactType.IsManyTo1BinaryFactType Then
                    '------------------------------------------------------------------------------------------------------------------------
                    'The RoleConstraint's Role is likely on a FactType that references an EntityType that has a CompoundReferenceScheme

                    lsTableName = arRoleConstraint.Role(0).BelongsToTable
                    lrTable = Me.RDS.getTableByName(lsTableName)

                    Dim lrModelElement As FBM.ModelObject

                    If lrTable Is Nothing Then
                        'Table not created yet
                        lrModelElement = Me.GetModelObjectByName(lsTableName)
                        lrTable = New RDS.Table(Me.RDS, lsTableName, lrModelElement)
                        Me.RDS.Table.AddUnique(lrTable)
                    End If

                    lrRole = arRoleConstraint.Role(0)
                    lrModelElement = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject

                    If lrModelElement.ConceptType = pcenumConceptType.EntityType Then

                        Dim lrEntityType As FBM.EntityType = lrModelElement

                        If lrEntityType.HasCompoundReferenceMode Then
                            'Good. We found it
                            Dim larColumn As New List(Of RDS.Column)

                            Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrRole, larColumn)
                            For Each lrColumn In larColumn
                                lrColumn.IsMandatory = lrRole.Mandatory
                                lrTable.addColumn(lrColumn)
                            Next
                        Else
                            'If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                            '    'There is no Column in the Table for the Role.

                            '    lrColumn = lrRole.GetCorrespondingColumn(lrTable)
                            '    lrTable.addColumn(lrColumn)
                            'End If
                            'Why would you get here...should be caught above as a Role that returns a single Column.
                            Throw New Exception("RoleConstraint, '" & arRoleConstraint.Id & "' not catered for when generating Attribute artifacts.")
                        End If
                    ElseIf lrModelElement.ConceptType = pcenumConceptType.ValueType Then
                        'If Not lrTable.Column.Exists(Function(x) x.Role.Id = lrRole.Id) Then
                        '    'There is no Column in the Table for the Role.

                        '    lrColumn = lrRole.GetCorrespondingColumn(lrTable)
                        '    lrTable.addColumn(lrColumn)
                        'End If
                        Throw New Exception("RoleConstraint, '" & arRoleConstraint.Id & "' not catered for when generating Attribute artifacts.")
                    ElseIf lrModelElement.ConceptType = pcenumConceptType.FactType Then
                        Dim larColumn As New List(Of RDS.Column)

                        larColumn = lrRole.getColumns(lrTable, lrRole)

                        For Each lrColumn In larColumn
                            If Not lrTable.Column.Exists(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id) Then
                                'There is no Column in the Table for the Role.
                                lrColumn.Name = FEStrings.MakeCapCamelCase(FEStrings.ProperSpace(lrColumn.Name))
                                lrColumn.Name = lrTable.createUniqueColumnName(lrColumn.Name, lrColumn, 0)
                                lrColumn.IsMandatory = lrRole.Mandatory
                                lrTable.addColumn(lrColumn)
                            End If
                        Next
                    Else
                        Throw New Exception("RoleConstraint, '" & arRoleConstraint.Id & "' not catered for when generating Attribute artifacts.")
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Genreates the CMML Entity artifacts. Called from Me.createEntityRelationshipArtifacts.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub generateEntityArtifacts()

            Dim lsSQLQuery As String
            Dim lrTable As RDS.Table

            Try
                '----------------------------------------------------------------------------
                'Create an Entity for each FactType with a TotalInternalUniquenessConstraint
                '----------------------------------------------------------------------------
                For Each lrFactType In Me.FactType.FindAll(Function(x) x.IsMDAModelElement = False)
                    '===============================================================
                    'Check if is a FactType with TotalInternalUniquenessConstraint
                    '===============================================================

                    If lrFactType.HasTotalRoleConstraint Or lrFactType.HasPartialButMultiRoleConstraint Then
                        '---------------------------------------------------------------
                        'Check to see if the Entity already exists in the ERD MetaModel
                        '---------------------------------------------------------------
                        lsSQLQuery = " SELECT COUNT(*)"
                        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                        lsSQLQuery &= " WHERE Element = '" & lrFactType.Name & "'"

                        Dim lrORMRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrORMRecordset.Facts(0).DictionarySet("Count") = 0 Then

                            Boston.WriteToStatusBar("Creating Entity, '" & lrFactType.Name & "'")

                            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                            lsSQLQuery &= " (Element, ElementType)"
                            lsSQLQuery &= " VALUES ('" & lrFactType.Name & "', 'Entity')"
                            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                            lrTable = New RDS.Table(Me.RDS, lrFactType.Name, lrFactType)

                            If lrFactType.Arity > 2 Then
                                'Couldn't possibly be a PGSRelation.
                            ElseIf lrFactType.Arity = 2 _
                                And Not lrFactType.atLeasOneRoleJoinsAValueType Then

                                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                                lsSQLQuery &= " (IsPGSRelation)"
                                lsSQLQuery &= " VALUES ('" & lrFactType.Id & "')"
                                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                lrTable.isPGSRelation = True
                            End If

                            Me.RDS.Table.AddUnique(lrTable)

                        End If
                    End If

                    ''======================================================================================
                    ''Check if is a FactType with InternalUniquenessConstraint spanning more than one Role
                    ''======================================================================================
                    'Dim lrRoleConstraint As FBM.RoleConstraint

                    'For Each lrRoleConstraint In lrFactType.InternalUniquenessConstraint
                    '    If (lrFactType.Arity > 2) And (lrRoleConstraint.RoleConstraintRole.Count >= 2) Then
                    '        '-----------------------------------------------------------------------------
                    '        'Is a FactType with InternalUniquenessConstraint spanning more than one Role
                    '        '-----------------------------------------------------------------------------
                    '        lsSQLQuery = " SELECT COUNT(*)"
                    '        lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    '        lsSQLQuery &= " WHERE Element = '" & lrFactType.Name & "'"

                    '        Dim lrORMRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '        If lrORMRecordset.Facts(0).DictionarySet("Count") = 0 Then

                    '            Boston.WriteToStatusBar("Creating Entity, '" & lrFactType.Name & "'")

                    '            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    '            lsSQLQuery &= " (Element, ElementType)"
                    '            lsSQLQuery &= " VALUES ('" & lrFactType.Name & "', 'Entity')"

                    '            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '            lrTable = New RDS.Table(Me.RDS, lrFactType.Name, lrFactType)
                    '            Me.RDS.Table.AddUnique(lrTable)
                    '        End If
                    '        Exit For
                    '    End If
                    'Next
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateERDIndexes()

            Boston.WriteToStatusBar("Generating the Indexes.", True)

            Call Me.generateIndexesForSimpleReferenceSchemes()

            Call Me.generateIndexesForCompoundReferenceSchemes()

            Call Me.generateIndexesForTernaryOrGreaterFactTypes()

        End Sub

        Public Sub generateIndexesForCompoundReferenceSchemes()

            Dim lrColumn As RDS.Column
            Dim larColumn As List(Of RDS.Column)

            Try
                '=====================================================================
                'Generate the Indexes for EntityTypes with CompoundReferenceSchemes
                Dim larEntityType = From EntityType In Me.EntityType
                                    Where EntityType.IsMDAModelElement = False _
                                    And EntityType.HasCompoundReferenceMode
                                    Select EntityType

                For Each lrEntityType In larEntityType

                    Dim lrRoleConstraint As FBM.RoleConstraint = lrEntityType.ReferenceModeRoleConstraint

                    larColumn = New List(Of RDS.Column)
                    For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                        Dim lrNearestRole As FBM.Role = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id)

                        Dim larColumns = From Table In Me.RDS.Table
                                         From Column In Table.Column
                                         Where Column.Role.Id = lrNearestRole.Id
                                         Select Column Distinct ' 'ActiveRole.Id = lrRoleConstraintRole.Role.Id _

                        For Each lrColumn In larColumns
                            '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                            'lrColumn.ContributesToPrimaryKey = True
                            larColumn.Add(lrColumn)
                        Next
                    Next

                    Dim lrIndex As New RDS.Index(larColumn(0).Table,
                                                 larColumn(0).Table.Name & "_PK",
                                                 "PK",
                                                 pcenumCMMLIndexDirection.ASC,
                                                 True,
                                                 True,
                                                 True,
                                                 larColumn,
                                                 False,
                                                 True)

                    larColumn(0).Table.Index.AddUnique(lrIndex)

                    Call Me.createCMMLIndex(lrEntityType.Name & "_PK",
                        lrEntityType.Name,
                        "PK",
                        pcenumCMMLIndexDirection.ASC,
                        True,
                        True,
                        True,
                        larColumn)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateIndexesForSimpleReferenceSchemes()

            Dim lrColumn As RDS.Column
            Dim larColumn As List(Of RDS.Column)

            '====================================================
            'Generate the Indexes for SimpleReferenceSchemes
            '=================================================
            Try
                Dim larRole = From RoleConstraint In Me.RoleConstraint
                              Where RoleConstraint.IsMDAModelElement = False _
                              And RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                              And RoleConstraint.RoleConstraintRole.Count = 1 _
                              And RoleConstraint.IsPreferredIdentifier = True
                              Select RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(RoleConstraint.RoleConstraintRole(0).Role.Id)

                For Each lrRole In larRole

                    Dim larColumns = From Table In Me.RDS.Table
                                     From Column In Table.Column
                                     Where Column.Role.Id = lrRole.Id
                                     Select Column Distinct

                    lrColumn = larColumns(0)

                    '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                    'lrColumn.ContributesToPrimaryKey = True

                    larColumn = New List(Of RDS.Column)
                    larColumn.Add(lrColumn)

                    Dim lrIndex As New RDS.Index(lrColumn.Table,
                                                 lrColumn.Table.Name & "_PK",
                                                 "PK",
                                                 pcenumCMMLIndexDirection.ASC,
                                                 True,
                                                 True,
                                                 True,
                                                 larColumn,
                                                 False,
                                                 True)

                    lrColumn.Table.Index.AddUnique(lrIndex)

                    Call Me.createCMMLIndex(lrColumn.Table.Name & "_PK",
                                            lrColumn.Table.Name,
                                            "PK",
                                            pcenumCMMLIndexDirection.ASC,
                                            True,
                                            True,
                                            True,
                                            larColumn)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateIndexesForTernaryOrGreaterFactTypes()

            Dim lrColumn As RDS.Column
            Dim larColumn As List(Of RDS.Column)

            '====================================================
            'Generate the Indexes for SimpleReferenceSchemes
            '=================================================
            Try
                Dim larRoleConstraint = From RoleConstraint In Me.RoleConstraint
                                        Where RoleConstraint.IsMDAModelElement = False _
                                        And RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                                        And RoleConstraint.RoleConstraintRole.Count > 1
                                        Select RoleConstraint

                Dim lbIsPK As Boolean = False

                For Each lrRoleConstraint In larRoleConstraint

                    If Me.RDS.Table.Find(Function(x) x.Name = lrRoleConstraint.RoleConstraintRole(0).Role.FactType.Id) IsNot Nothing Then
                        'Table exists for the RoleConstraint

                        larColumn = New List(Of RDS.Column)



                        For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                            Dim larIndexColumn = From Table In Me.RDS.Table
                                                 From Column In Table.Column
                                                 Where Column.Role.Id = lrRoleConstraintRole.Role.Id
                                                 Select Column Distinct

                            For Each lrColumn In larIndexColumn

                                If lrRoleConstraint.RoleConstraintRole(0).Role.FactType.InternalUniquenessConstraint.Count = 1 Then
                                    lbIsPK = True
                                    Exit For
                                    'lrColumn.ContributesToPrimaryKey = True
                                End If

                                If lrRoleConstraint.IsPreferredIdentifier = True Then
                                    lbIsPK = True
                                    Exit For
                                    'lrColumn.ContributesToPrimaryKey = True
                                End If

                                'larColumn.AddUnique(lrColumn) 'See below ToList
                            Next
                            larColumn = larIndexColumn.ToList
                        Next

                        Dim lsIndexName As String = ""
                        Dim lsQualifier As String = ""

                        If larColumn.Count = 0 Then
                            Throw New Exception("No Columns found for Table covered by Role Constraint, '" & lrRoleConstraint.Id & "'")
                        End If

                        Dim lbIsPrimaryKey As Boolean = False
                        If lbIsPK Then 'larColumn(0).ContributesToPrimaryKey Then
                            lsIndexName = larColumn(0).Table.Name & "_PK"
                            lsQualifier = "PK"
                            lbIsPrimaryKey = True
                        Else
                            lsQualifier = larColumn(0).Table.generateUniqueQualifier("UC")
                            lsIndexName = larColumn(0).Table.Name & "_" & Trim(lsQualifier)
                        End If

                        Dim lrIndex As New RDS.Index(larColumn(0).Table,
                                                     lsIndexName,
                                                     lsQualifier,
                                                     pcenumCMMLIndexDirection.ASC,
                                                     lbIsPrimaryKey,
                                                     True,
                                                     True,
                                                     larColumn,
                                                     False,
                                                     True)

                        larColumn(0).Table.Index.AddUnique(lrIndex)

                        Call Me.createCMMLIndex(lsIndexName,
                                                larColumn(0).Table.Name,
                                                lsQualifier,
                                                pcenumCMMLIndexDirection.ASC,
                                                lbIsPrimaryKey,
                                                True,
                                                True,
                                                larColumn)

                    End If 'Found Table for Index
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateERDRelationships()

            Try
                Dim lrFactType As FBM.FactType

                Boston.WriteToStatusBar("Creating Relationships.", True)

                '----------------------------------------------------------------------------------
                'Role is for Many-to-One Binary FactType and Role has InternalUniquenessConstraint.
                '----------------------------------------------------------------------------------
                For Each lrRole In Me.Role.FindAll(Function(x) x.FactType.IsMDAModelElement = False _
                                                           And x.HasInternalUniquenessConstraint _
                                                           And x.FactType.IsManyTo1BinaryFactType)
                    If lrRole.FactType.IsManyTo1BinaryFactType _
                        And Not (lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).ConceptType = pcenumConceptType.ValueType) Then
                        Call Me.generateRelationForManyTo1BinaryFactType(lrRole)
                    End If 'IsManyTo1BinaryFactType
                Next 'RoleInstance

                For Each lrFactType In Me.FactType.FindAll(Function(x) x.IsMDAModelElement = False _
                                                                   And x.InternalUniquenessConstraint.Count > 0 _
                                                                   And x.Is1To1BinaryFactType _
                                                                   And Not x.IsPreferredReferenceMode)

                    Call Me.generateRelationFor1To1BinaryFactType(lrFactType.RoleGroup(0))
                Next 'RoleInstance

                For Each lrFactType In Me.FactType.FindAll(Function(x) (x.IsMDAModelElement = False _
                                                                        And (x.HasTotalRoleConstraint _
                                                                        Or x.HasPartialButMultiRoleConstraint) _
                                                                        And Not x.hasLinkFactTypes))

                    '----------------------------------------------------------------------------------------
                    'NB LinkFactTypes are taken care of in generateRelationForManyTo1BinaryFactType (above)

                    '---------------------
                    'Create the Relation
                    '---------------------
                    For Each lrRole In lrFactType.RoleGroup
                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                'Role represents a Column/Attribute/Property rather than a ForeignKey
                            Case Else
                                Call Me.generateRelationForManyToManyFactTypeRole(lrRole)
                        End Select

                    Next 'Role in FactType
                Next 'FactType with TotalInternalUniquenessConstraint

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function GenerateFEKL(Optional ByVal abUseEntityGrouping As Boolean = False,
                                     Optional ByVal abGenerateFacts As Boolean = False,
                                     Optional ByRef arModelElement As FBM.ModelObject = Nothing) As String

            Dim lsFEKL As String = ""

            Try
                'CodeSafe | Organic Computing. Load the model if it isn't already loaded.
                If Not Me.Loaded Then Call Me.Load(True, False, Nothing, True, False)

                If abUseEntityGrouping Then
#Region "Entity Grouping"
                    Dim larEntity As New List(Of FBM.ModelObject)

                    If arModelElement Is Nothing Then

                        larEntity = (From Entity In Me.getModelObjects(True)
                                     Select Entity).ToList
                    Else
                        larEntity.Add(arModelElement)
                    End If

                    For Each lrEntity In larEntity

                        lsFEKL &= CType(lrEntity, FBM.ModelObject).GenerateFEKLLine(, abUseEntityGrouping)

                    Next
#End Region
                Else
#Region "Straight FEKL (no Entity Grouping)"
                    If arModelElement Is Nothing Then
#Region "All Model Elements"
                        For Each lrValueType In Me.ValueType.FindAll(Function(x) Not x.IsMDAModelElement)
                            lsFEKL &= lrValueType.GenerateFEKLLine
                        Next

                        For Each lrEntityType In Me.EntityType.FindAll(Function(x) Not x.IsMDAModelElement And Not x.IsObjectifyingEntityType)
                            lsFEKL &= lrEntityType.GenerateFEKLLine
                        Next

                        For Each lrFactType In Me.FactType.FindAll(Function(x) Not x.IsMDAModelElement And Not x.IsPreferredReferenceMode And Not x.IsLinkFactType)
                            lsFEKL &= lrFactType.GenerateFEKLLine

                            If abGenerateFacts Then
                                For Each lrFact In lrFactType.Fact
                                    lsFEKL &= lrFact.GenerateFEKLLine
                                Next
                            End If
                        Next

                        For Each lrRoleConstraint In Me.RoleConstraint.FindAll(Function(x) Not x.IsMDAModelElement)
                            lsFEKL &= lrRoleConstraint.GenerateFEKLLine
                        Next
#End Region
                    Else
#Region "Particular Model Element"
                        Select Case arModelElement.GetType
                            Case Is = GetType(FBM.ValueType)
                                lsFEKL &= CType(arModelElement, FBM.ValueType).GenerateFEKLLine
                            Case Is = GetType(FBM.EntityType)

                                Dim lrTable As RDS.Table = arModelElement.getCorrespondingRDSTable

#Region "Value Types (for Columns of the RDS.Table for the Entity Type"
                                If lrTable IsNot Nothing Then
                                    For Each lrColumn In lrTable.Column
                                        Dim lrValueType = lrColumn.ActiveRole.JoinsValueType

                                        lsFEKL &= lrValueType.GenerateFEKLLine
                                    Next
                                    lsFEKL.AppendLine("")
                                End If
#End Region

                                lsFEKL &= CType(arModelElement, FBM.EntityType).GenerateFEKLLine
                                lsFEKL.AppendLine("")

#Region "Fact Type (Attributes/Columns/Properties) for the Entity Type"
                                If lrTable IsNot Nothing Then
                                    For Each lrColumn In lrTable.Column
                                        lsFEKL &= lrColumn.FactType.GenerateFEKLLine()
                                    Next
                                    lsFEKL.AppendLine("")
                                End If
#End Region

                            Case Is = GetType(FBM.FactType)
                                lsFEKL &= CType(arModelElement, FBM.FactType).GenerateFEKLLine
                        End Select
#End Region
                    End If

#End Region
                End If

                Return lsFEKL

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return lsFEKL
            End Try

        End Function

        Public Sub generateRelationForReassignedRole(ByRef arRole As FBM.Role)

            Try
                Dim lrRole As FBM.Role = arRole

                Dim larRelation = From Relation In Me.RDS.Relation
                                  Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id
                                  Select Relation

                Dim lrResponsibleRole As FBM.Role

                If (lrRole.FactType.HasTotalRoleConstraint Or lrRole.FactType.HasPartialButMultiRoleConstraint) And lrRole.FactType.InternalUniquenessConstraint.Count > 0 Then

                    lrResponsibleRole = lrRole

                    If lrRole.FactType.IsObjectified Then
                        Dim larLinkFactTypeRole = From FactType In lrRole.Model.FactType
                                                  Where FactType.IsLinkFactType = True _
                                                  And FactType.LinkFactTypeRole Is lrResponsibleRole
                                                  Select FactType.RoleGroup(0)

                        For Each lrLinkFactTypeRole In larLinkFactTypeRole
                            Call lrRole.Model.generateRelationForManyTo1BinaryFactType(lrLinkFactTypeRole)
                        Next
                    Else
                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType
                                Call lrRole.Model.generateRelationForManyToManyFactTypeRole(lrResponsibleRole)
                        End Select

                    End If
                ElseIf lrRole.FactType.Is1To1BinaryFactType Then
                    'ToDo:To test
                    Call Me.generateRelationFor1To1BinaryFactType(lrRole)
                ElseIf lrRole.FactType.Arity = 2 Then

                    If lrRole.HasInternalUniquenessConstraint Then
                        lrResponsibleRole = lrRole
                    ElseIf lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).HasInternalUniquenessConstraint Then
                        lrResponsibleRole = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id)
                    Else
                        lrResponsibleRole = Nothing
                    End If

                    If lrResponsibleRole IsNot Nothing Then
                        Select Case lrResponsibleRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType

                                Call lrRole.Model.generateRelationForManyTo1BinaryFactType(lrResponsibleRole)
                        End Select
                    End If

                End If
                '======================================================================================================================

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        Public Sub generateRelationFor1To1BinaryFactType(ByRef arRole As FBM.Role)

            Try
                '====================
                'RDS
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lbContributesToPrimaryKey As Boolean = False

                Dim lrRole As FBM.Role = arRole
                Dim lrOtherRole As FBM.Role = Nothing

                lrOtherRole = arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id)

                Select Case lrOtherRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        'We don't generate Relations for ManyToOneFactTypes that join to a ValueType
                    Case Is = pcenumConceptType.EntityType,
                              pcenumConceptType.FactType

                        lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.JoinedORMObject.Id)
                        lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrOtherRole.JoinedORMObject.Id)

                        If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then
                            Throw New Exception("Could not find Origin table or Destination Table when creating relation.")
                        End If

                        '------------------------------------------------------------------------------------------
                        'If the OriginEntity is a Subtype of another Entity, then need to set the OriginEntity to
                        '  the topmost Supertype.
                        '------------------------------------------------------------------------------------------
                        Dim lrModelObject As FBM.ModelObject
                        lrModelObject = Me.GetModelObjectByName(lrOriginTable.Name)
                        Dim lrEntityType As FBM.EntityType
                        Select Case lrModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrEntityType = lrModelObject
                                If lrEntityType.IsAbsorbed Then
                                    lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                                Else
                                    'Leave the lsOriginEntityName the same
                                End If
                            Case Else
                                '20171104-VM-Keep the lsOriginEntityName the same for now.
                                'Need to update this code when FactTypes can have supertypes in Boston.
                        End Select


                        '---------------------------------------------------------------------------------------------------
                        'If the DestinationEntity is a Subtype of another Entity, then need to set the DestinationTable to
                        '  the topmost Supertype.
                        '---------------------------------------------------------------------------------------------------                                
                        lrModelObject = Me.GetModelObjectByName(lrDestinationTable.Name)
                        lrEntityType = New FBM.EntityType
                        Select Case lrModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrEntityType = lrModelObject
                                If lrEntityType.IsAbsorbed Then
                                    lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                                Else
                                    'Leave the lsOriginEntityName the same
                                End If
                            Case Else
                                '20171104-VM-Keep the lsOriginEntityName the same for now.
                                'Need to update this code when FactTypes can have supertypes in Boston.
                        End Select

                        '=================================================================
                        'Origin/Destination Predicates
                        Dim lsOriginPredicate As String = ""
                        Dim lsDestinationPredicate As String = ""

                        Dim larRole As New List(Of FBM.Role)
                        Dim lrFactTypeReading As FBM.FactTypeReading
                        larRole.Add(arRole.FactType.RoleGroup(1))
                        larRole.Add(arRole.FactType.RoleGroup(0))

                        lrFactTypeReading = arRole.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                        If lrFactTypeReading IsNot Nothing Then
                            lsOriginPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                        Else
                            lsOriginPredicate = "unknown predicate"
                        End If

                        larRole.Clear()
                        larRole.Add(arRole.FactType.RoleGroup(0))
                        larRole.Add(arRole.FactType.RoleGroup(1))

                        lrFactTypeReading = arRole.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                        If lrFactTypeReading IsNot Nothing Then
                            lsDestinationPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                        Else
                            lsDestinationPredicate = "unknown predicate"
                        End If

                        Boston.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                        '--------------------------------------------------------------------
                        'Create the Relation
                        '--------------------------------------------------------------------                        
                        Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                           lrOriginTable,
                                                           pcenumCMMLMultiplicity.One,
                                                           arRole.Mandatory,
                                                           lbContributesToPrimaryKey,
                                                           lsOriginPredicate,
                                                           lrDestinationTable,
                                                            pcenumCMMLMultiplicity.One,
                                                           arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id).Mandatory,
                                                           lsDestinationPredicate,
                                                           arRole.FactType)

                        '--------------------------------------------------------------------------
                        'Get the Origin/Destination Columns
                        Dim larDestinationColumn As New List(Of RDS.Column)
                        larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)
                        lrRelation.DestinationColumns = larDestinationColumn

                        Dim larOriginColumn As New List(Of RDS.Column)
                        For Each lrColumn In larDestinationColumn
                            larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id))
                        Next
                        lrRelation.OriginColumns = larOriginColumn

                        lrRelation.OriginColumns = larOriginColumn
                        For Each lrColumn In lrRelation.OriginColumns
                            Try
                                lrColumn.Relation.Add(lrRelation) 'Only need to do this on the Origin side. The Origin Column 'has' the Relation.
                            Catch ex As Exception
                                Throw New Exception("Failed to create Relation between Entities " & lrOriginTable.Name & " and " & lrDestinationTable.Name)
                            End Try

                        Next

                        Me.RDS.addRelation(lrRelation)

                        '20240223-VM-Only need to do this on the Origin side. The Origin Column 'has' the Relation.
                        'Reciprocal relations for 'Destination' Table's Columns
                        'larDestinationColumn = New List(Of RDS.Column)
                        'larDestinationColumn = lrOriginTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)

                        'For Each lrOriginColumn In larDestinationColumn
                        '    For Each lrColumn In lrDestinationTable.Column.FindAll(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                        '        lrColumn.Relation.Add(lrRelation)
                        '    Next
                        'Next


                End Select 'GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject.ConceptType

                '==================================================================

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub generateRelationManyTo1ForUnaryFactType(ByRef arRole As FBM.Role)

            Try
                '====================
                'RDS
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lbContributesToPrimaryKey As Boolean = False

                Dim lrRole As FBM.Role = arRole

                Select Case lrRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        'We don't generate Relations for ManyToOneFactTypes that join to a ValueType
                    Case Is = pcenumConceptType.EntityType,
                              pcenumConceptType.FactType

                        Dim larTable As List(Of RDS.Table)
                        If lrRole.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                            If lrRole.JoinsEntityType.HasSimpleReferenceScheme Then
                                larTable = (From Table In Me.RDS.Table
                                            From Column In Table.Column
                                            Where Column.Role Is lrRole
                                            Where Column.ActiveRole Is CType(lrRole.JoinsEntityType.GetTopmostNonAbsorbedSupertype(True), FBM.EntityType).ReferenceModeRoleConstraint.Role(0)
                                            Where Column.Role.JoinedORMObject.Id <> Table.Name
                                            Where Table.FBMModelElement Is lrRole.FactType
                                            Select Table).ToList
                            Else
                                larTable = (From Table In Me.RDS.Table
                                            From Column In Table.Column
                                            Where Column.Role Is lrRole
                                            Where Column.ActiveRole Is lrRole
                                            Where Column.Role.JoinedORMObject.Id <> Table.Name
                                            Where Table.FBMModelElement Is lrRole.FactType
                                            Select Table).ToList
                            End If
                        Else
                            larTable = (From Table In Me.RDS.Table
                                        From Column In Table.Column
                                        Where Column.Role Is lrRole
                                        Where Column.ActiveRole Is lrRole
                                        Where Column.Role.JoinedORMObject.Id <> Table.Name
                                        Where Table.FBMModelElement Is lrRole.FactType
                                        Select Table).ToList
                        End If

                        lrOriginTable = larTable.First
                        lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.JoinedORMObject.GetTopmostNonAbsorbedSupertype.Id)

                        If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Exit Sub

                        '------------------------------------------------------------------------------------------
                        'If the OriginEntity is a Subtype of another Entity, then need to set the OriginEntity to
                        '  the topmost Supertype.
                        '------------------------------------------------------------------------------------------
                        Dim lrOriginModelObject As FBM.ModelObject
                        lrOriginModelObject = Me.GetModelObjectByName(lrOriginTable.Name)
                        Dim lrEntityType As FBM.EntityType
                        Select Case lrOriginModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrEntityType = lrOriginModelObject
                                If lrEntityType.IsAbsorbed Then
                                    lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                                Else
                                    'Leave the lsOriginEntityName the same
                                End If
                            Case Else
                                '20171104-VM-Keep the lsOriginEntityName the same for now.
                                'Need to update this code when FactTypes can have supertypes in Boston.
                        End Select


                        '---------------------------------------------------------------------------------------------------
                        'If the DestinationEntity is a Subtype of another Entity, then need to set the DestinationTable to
                        '  the topmost Supertype.
                        '---------------------------------------------------------------------------------------------------                                
                        Dim lrDestinationModelObject As FBM.ModelObject = Me.GetModelObjectByName(lrDestinationTable.Name)
                        lrEntityType = New FBM.EntityType
                        Select Case lrDestinationModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrEntityType = lrDestinationModelObject
                                If lrEntityType.IsAbsorbed Then
                                    lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                                Else
                                    'Leave the lsOriginEntityName the same
                                End If
                            Case Else
                                '20171104-VM-Keep the lsOriginEntityName the same for now.
                                'Need to update this code when FactTypes can have supertypes in Boston.
                        End Select

                        If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Exit Sub

                        Dim larRelation = From Relation In Me.RDS.Relation
                                          Where Relation.ResponsibleFactType IsNot Nothing
                                          Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id _
                                          And Relation.OriginTable Is lrOriginTable _
                                          And Relation.DestinationTable Is lrDestinationTable
                                          Select Relation

                        If larRelation.Count > 0 Then
                            Exit Sub
                        End If


                        '=================================================================
                        'Origin/Destination Predicates
                        Dim lsOriginPredicate As String = "Involves"
                        Dim lsDestinationPredicate As String = "Is involved in"


                        '--------------------------------------------------------------------
                        'Create the Relation
                        '--------------------------------------------------------------------                        
                        Boston.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                        Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                           lrOriginTable,
                                                           pcenumCMMLMultiplicity.Many,
                                                           arRole.Mandatory,
                                                           lbContributesToPrimaryKey,
                                                           lsOriginPredicate,
                                                           lrDestinationTable,
                                                            pcenumCMMLMultiplicity.One,
                                                           True,
                                                           lsDestinationPredicate,
                                                           arRole.FactType)

                        '--------------------------------------------------------------------------
                        'Get the Origin/Destination Columns
                        Dim larDestinationColumn As New List(Of RDS.Column)
                        Select Case lrDestinationModelObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)
                            Case Else 'FactType by enlcosing IF
                                larDestinationColumn = lrDestinationTable.Column
                        End Select

                        lrRelation.DestinationColumns = larDestinationColumn

                        Dim larOriginColumn As New List(Of RDS.Column)

                        larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.Role Is lrRole))

                        'CodeSafe
                        'Get rid of Columns that are Nothing
                        larOriginColumn.RemoveAll(Function(x) x Is Nothing)

                        'Origin Columns
                        lrRelation.OriginColumns = larOriginColumn

                        For Each lrColumn In lrRelation.OriginColumns
                            lrColumn.Relation.Add(lrRelation) 'Only need to do this on the Origin side. The Origin Column 'has' the Relation.
                        Next

                        If lrRelation.OriginColumns.Count = 0 Then
                            'CodeSafe. Destination Table has no PrimaryKey most likely,
                            '  but can get Origin Column from the arRole
                            Dim lrColumn = arRole.JoinedORMObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.Id = lrRole.Id)
                            lrColumn.Relation.Add(lrRelation)
                            lrRelation.OriginColumns.Add(lrColumn)
                        End If

                        Me.RDS.addRelation(lrRelation)

                End Select 'GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject.ConceptType


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function generateRelationForManyTo1BinaryFactType(ByRef arRole As FBM.Role,
                                                                 Optional ByVal abAddToModel As Boolean = True) As RDS.Relation

            Dim lrRelation As RDS.Relation = Nothing

            Try
                '====================
                'RDS
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lbContributesToPrimaryKey As Boolean = False

                Dim lbProcessTheRelationship As Boolean = False

                Dim lrRole As FBM.Role = arRole

                Dim lrOtherRole As FBM.Role = arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id)

                lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.JoinedORMObject.Id)

                Select Case lrOtherRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        'We don't generate Relations for ManyToOneFactTypes that join to a ValueType
                        If CType(lrOtherRole.JoinedORMObject, FBM.ValueType).IsIndependent Then
                            'Is Effectively to an Entity Type for RDS processing. Idependent Value Types have their own Entity/Table, with one Primary Key column...with the same name as the Value Type.
                            lbProcessTheRelationship = True
                        End If
                    Case Is = pcenumConceptType.EntityType,
                              pcenumConceptType.FactType

                        lbProcessTheRelationship = True
                End Select

                If lbProcessTheRelationship Then
                    lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.JoinedORMObject.Id)
                    lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject.Id)

                    If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Return Nothing

                    '------------------------------------------------------------------------------------------
                    'If the OriginEntity is a Subtype of another Entity, then need to set the OriginEntity to
                    '  the topmost Supertype.
                    '------------------------------------------------------------------------------------------
                    Dim lrOriginModelObject As FBM.ModelObject
                    lrOriginModelObject = Me.GetModelObjectByName(lrOriginTable.Name)
                    Dim lrEntityType As FBM.EntityType
                    Select Case lrOriginModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrEntityType = lrOriginModelObject
                            If lrEntityType.IsAbsorbed Then
                                lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                            Else
                                'Leave the lsOriginEntityName the same
                            End If
                        Case Else
                            '20171104-VM-Keep the lsOriginEntityName the same for now.
                            'Need to update this code when FactTypes can have supertypes in Boston.
                    End Select

                    '---------------------------------------------------------------------------------------------------
                    'If the DestinationEntity is a Subtype of another Entity, then need to set the DestinationTable to
                    '  the topmost Supertype.
                    '---------------------------------------------------------------------------------------------------                                
                    Dim lrDestinationModelObject As FBM.ModelObject = Me.GetModelObjectByName(lrDestinationTable.Name)
                    If lrDestinationModelObject Is Nothing Then
                        lrDestinationModelObject = lrDestinationTable.FBMModelElement
                        If lrDestinationModelObject Is Nothing Then
                            Throw New ApplicationException("Could not find Object Type for Destination Table: " & lrDestinationTable.Name)
                        End If
                    End If

                    lrEntityType = New FBM.EntityType
                    Select Case lrDestinationModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrEntityType = lrDestinationModelObject
                            If lrEntityType.IsAbsorbed Then
                                lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                            Else
                                'Leave the lsOriginEntityName the same
                            End If
                        Case Else
                            '20171104-VM-Keep the lsOriginEntityName the same for now.
                            'Need to update this code when FactTypes can have supertypes in Boston.
                    End Select

                    If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Return Nothing

                    Dim larRelation = From Relation In Me.RDS.Relation
                                      Where Relation.ResponsibleFactType IsNot Nothing
                                      Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id _
                                      And Relation.OriginTable Is lrOriginTable _
                                      And Relation.DestinationTable Is lrDestinationTable
                                      Select Relation

                    If larRelation.Count > 0 Then
                        Return Nothing
                    End If

                    '=================================================================
                    'Origin/Destination Predicates
                    Dim lsOriginPredicate As String = ""
                    Dim lsDestinationPredicate As String = ""

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    larRole.Add(arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(arRole)

                    lrFactTypeReading = arRole.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsOriginPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsOriginPredicate = "unknown predicate"
                    End If

                    larRole.Clear()
                    larRole.Add(arRole) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id))

                    lrFactTypeReading = arRole.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsDestinationPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsDestinationPredicate = "unknown predicate"
                    End If


                    '--------------------------------------------------------------------
                    'Create the Relation
                    '--------------------------------------------------------------------                        
                    Boston.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                    lrRelation = New RDS.Relation(System.Guid.NewGuid.ToString,
                                                  lrOriginTable,
                                                  pcenumCMMLMultiplicity.Many,
                                                  arRole.Mandatory,
                                                  lbContributesToPrimaryKey,
                                                  lsOriginPredicate,
                                                  lrDestinationTable,
                                                  pcenumCMMLMultiplicity.One,
                                                  arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id).Mandatory,
                                                  lsDestinationPredicate,
                                                  arRole.FactType)

                    '--------------------------------------------------------------------------
                    'Get the Origin/Destination Columns
                    Dim larDestinationColumn As New List(Of RDS.Column)
                    Select Case lrDestinationModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)
                        Case Else 'FactType by enlcosing IF
                            larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey)
                    End Select

                    lrRelation.DestinationColumns = larDestinationColumn

                    Dim larOriginColumn As New List(Of RDS.Column)

                    Dim lsColumnName As String
                    Dim lsOriginalColumnName As String = "Error-Contact FactEngine"
                    Dim lrColumn As RDS.Column = Nothing
                    Select Case lrOtherRole.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            For Each lrColumn In lrRelation.DestinationColumns
                                If arRole.FactType.IsLinkFactType Then
                                    lrColumn = lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.FactType.LinkFactTypeRole.Id _
                                                                                      And x.ActiveRole.Id = lrColumn.ActiveRole.Id)
                                    lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.Id
                                    larOriginColumn.Add(lrColumn)
                                Else
                                    If lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                                                                 And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                                        '=====================================================
                                        'The Column doesn't exist in the Table yet.
                                        lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.Id
                                        lsColumnName = lrOriginTable.createUniqueColumnName(lsOriginalColumnName)
                                        Dim lrNewColumn = New RDS.Column(lrOriginTable, lsColumnName, lrRole, lrColumn.ActiveRole, lrRole.Mandatory)
                                        lrOriginTable.addColumn(lrNewColumn)
                                    End If
                                    lrColumn = lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                                                                                     And x.ActiveRole.Id = lrColumn.ActiveRole.Id)
                                    lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.Id
                                    larOriginColumn.Add(lrColumn)

                                End If
                            Next
                        Case Is = pcenumConceptType.FactType
                            For Each lrColumn In lrDestinationTable.Column
                                lrColumn = lrOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id _
                                                                                 And Not x.Role.FactType.IsPreferredReferenceMode _
                                                                                 And x.Relation.Count = 0
                                                                                 )
                                lsOriginalColumnName = lrColumn.ActiveRole.JoinedORMObject.Id
                                larOriginColumn.Add(lrColumn)
                            Next
                    End Select

                    'CodeSafe
                    'Get rid of Columns that are Nothing
                    larOriginColumn.RemoveAll(Function(x) x Is Nothing)

                    'Origin Columns
                    lrRelation.OriginColumns = larOriginColumn

                    If abAddToModel Then 'Default is TRue

                        For Each lrColumn In lrRelation.OriginColumns
                            lrColumn.Relation.Add(lrRelation) 'Only need to do this on the Origin side. The Origin Column 'has' the Relation.
                        Next

                        If lrRelation.OriginColumns.Count = 0 Then
                            'CodeSafe. Destination Table has no PrimaryKey most likely,
                            '  but can get Origin Column from the arRole
                            Try
                                lrColumn = arRole.JoinedORMObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.Id = lrRole.Id)
                                lsOriginalColumnName = lrColumn.Name
                                If lrColumn IsNot Nothing Then
                                    lrColumn.Relation.Add(lrRelation)
                                    lrRelation.OriginColumns.Add(lrColumn)
                                End If
                            Catch ex As Exception
                                'Not a good place to be. Can only create the OriginColumns once the PrimaryKey is created for the DestinationTable.
                            End Try

                        End If

#Region "Recursive to Subtypes that need the Relation also"
                        'Subtypes - Inline recursive loop
                        Dim lrSubtypeRelation As RDS.Relation = lrRelation
                        Dim processSubtypes As Action(Of RDS.Table) = Nothing
                        Dim liInd = 0
                        processSubtypes = Sub(arTable As RDS.Table)

                                              If arTable IsNot lrRelation.OriginTable Then
                                                  lrSubtypeRelation = lrRelation.Clone
                                              End If

                                              lrSubtypeRelation.OriginTable = arTable
                                              Me.RDS.addRelation(lrSubtypeRelation)

                                              If liInd = 0 Then GoTo GenerateRelationManyTo1GetSubtypes

                                              'Renaming of OriginColumns for aesthetics. Rather than say Asset_Id primarykey from Supertype on say Floor,
                                              ' and Asset_Id1 where pointing to another subtype of the first, e.g Building,
                                              ' well create BuildingAsset_Id.
                                              For Each lrColumn In lrRelation.OriginColumns.FindAll(Function(x) Not x.isPartOfPrimaryKey)

                                                  Dim larFactTypeReading = From FactTypeReading In lrColumn.FactType.FactTypeReading
                                                                           Where lrColumn.Role.Id = FactTypeReading.RoleList(0).Id
                                                                           Select FactTypeReading

                                                  lsDestinationPredicate = ""

                                                  If larFactTypeReading.Count > 0 Then
                                                      lsDestinationPredicate = lrRelation.DestinationPredicate.ToPascalCase
                                                  End If

                                                  lsColumnName = lrOriginTable.createUniqueColumnName(lsDestinationPredicate & lsOriginalColumnName)
                                                  lrColumn.setName(lsColumnName)

                                              Next
GenerateRelationManyTo1GetSubtypes:
                                              For Each lrSubtypeTable In arTable.getSubtypeTables()

                                                  processSubtypes(lrSubtypeTable) ' Recursive call for each subtype

                                                  liInd += 1
                                              Next

                                          End Sub

                        ' Start the recursion
                        processSubtypes(lrRelation.OriginTable)
#End Region

                    End If

                End If 'GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject.ConceptType

                Return lrRelation

            Catch appEx As ApplicationException

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & appEx.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Warning, Nothing, False, False, True)

                Return Nothing

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function

        Public Sub generateRelationForManyToManyFactTypeRole(ByVal arResponsibleRole As FBM.Role)

            Try
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lbContributesToPrimaryKey As Boolean = False
                Dim lrModelObject As FBM.ModelObject
                Dim lrEntityType As FBM.EntityType

                lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = arResponsibleRole.FactType.Id)

                '---------------------------------------------------------------------------------------------------
                'If the DestinationEntity is a Subtype of another Entity, then need to set the DestinationTable to
                '  the topmost Supertype.
                '---------------------------------------------------------------------------------------------------                                
                lrModelObject = Me.GetModelObjectByName(arResponsibleRole.JoinedORMObject.Id)
                Select Case lrModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        lrEntityType = lrModelObject
                        If lrEntityType.IsAbsorbed Then
                            lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.GetTopmostSupertype.Id)
                        Else
                            'Leave the lsDestinationTable the same
                            lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrEntityType.Id)
                        End If
                    Case Else
                        lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrModelObject.Id)
                        '20171104-VM-Keep the lsOriginEntityName the same for now.
                        'Need to update this code when FactTypes can have supertypes in Boston.
                End Select

                If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Exit Sub

                Dim larRelation = From Relation In Me.RDS.Relation
                                  Where Relation.ResponsibleFactType IsNot Nothing
                                  Where Relation.ResponsibleFactType.Id = arResponsibleRole.FactType.Id _
                                  And Relation.OriginTable Is lrOriginTable _
                                  And Relation.DestinationTable Is lrDestinationTable _
                                  And Relation.OriginColumns.FindAll(Function(x) x.Role Is arResponsibleRole).Count > 0
                                  Select Relation

                If larRelation.Count > 0 Then
                    Exit Sub
                End If

                Boston.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                '--------------------------------------------------------------------
                'Create the Relation
                '--------------------------------------------------------------------                        
                Dim lrResponsibleRole = arResponsibleRole
                Dim lrLinkFactType = arResponsibleRole.FactType.getLinkFactTypes.Find(Function(x) x.LinkFactTypeRole.Id = lrResponsibleRole.Id)

                'CodeSafe: 20260306-Not the best, but use for now.
                If lrLinkFactType Is Nothing Then lrLinkFactType = arResponsibleRole.FactType


                Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                   lrOriginTable,
                                                   pcenumCMMLMultiplicity.Many,
                                                   True,
                                                   lbContributesToPrimaryKey,
                                                   "Is involved in",
                                                   lrDestinationTable,
                                                    pcenumCMMLMultiplicity.One,
                                                   arResponsibleRole.Mandatory,
                                                   "Involves",
                                                   lrLinkFactType) '20251030-VM-Was arResponsibleRole.FactType (which was the Objectified Fact Type, or the Fact Type with a MultiRoleInternalUniquenessConstraint.

                '--------------------------------------------------------------------------
                'Get the Origin/Destination Columns
                Dim larDestinationColumn As New List(Of RDS.Column)
                larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)
                lrRelation.DestinationColumns = larDestinationColumn

                Dim larOriginColumn As New List(Of RDS.Column)
                '20200701-VM-Remove the following if all seems okay. Found commented out on this date.
                'For Each lrColumn In larDestinationColumn
                '    If lrOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                '        'For when a Column in a ManyToManyFactType wasn't previously joined to a EntityType with a ReferenceScheme
                '        Dim lrColumnToModify As RDS.Column = lrOriginTable.Column.Find(Function(x) x.Role.JoinedORMObject.Id = lrColumn.Role.JoinedORMObject.Id And x.ActiveRole Is x.Role)
                '        If lrColumnToModify IsNot Nothing Then
                '            lrColumnToModify.setActiveRole(lrColumn.ActiveRole)
                '            larOriginColumn.Add(lrColumnToModify)
                '        End If
                '    Else
                '        larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id))
                '    End If
                'Next

                larOriginColumn = lrOriginTable.Column.FindAll(Function(x) x.Role.Id = arResponsibleRole.Id)

                lrRelation.OriginColumns = larOriginColumn
                For Each lrColumn In lrRelation.OriginColumns
                    lrColumn.Relation.Add(lrRelation)
                Next

                Me.RDS.addRelation(lrRelation)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Creates an RDS.Relation for PGSRelationNodes (for appropriate FactTypes).
        ''' PRECONDITION: The Fact Type is a Candidate for PGSRelationNodes.
        ''' </summary>
        ''' <param name="arResponsibleFactType"></param>
        Public Function generateRDSRelationForPGSRelationNode(ByRef arResponsibleFactType As FBM.FactType,
                                                              Optional ByVal abAddToRDSModel As Boolean = False) As RDS.Relation

            Try
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing

                Dim larRole = arResponsibleFactType.RoleGroup.ToList

                Dim larNodeJoiningRoles = From Role In larRole
                                          Where Role.JoinsValueType Is Nothing
                                          Select Role

                'CodeSafe
                If Not larNodeJoiningRoles.Count.Between(1, 2) Then Throw New Exception($"FactType, {arResponsibleFactType.Id}, is not a Candidate PGSRelation Node Fact Type.")

                lrOriginTable = larNodeJoiningRoles(0).JoinedORMObject.getCorrespondingRDSTable()
                lrDestinationTable = larNodeJoiningRoles(1).JoinedORMObject.getCorrespondingRDSTable()

                If (lrOriginTable Is Nothing) Or (lrDestinationTable Is Nothing) Then Throw New Exception($"FactType, {arResponsibleFactType.Id}, is not a Candidate PGSRelation Node Fact Type.")

                Dim lrResponsibleFactType = arResponsibleFactType

                Dim larRelation = From Relation In Me.RDS.Relation
                                  Where Relation.ResponsibleFactType IsNot Nothing
                                  Where Relation.ResponsibleFactType.Id = lrResponsibleFactType.Id _
                                  And Relation.OriginTable Is lrOriginTable _
                                  And Relation.DestinationTable Is lrDestinationTable _
                                  And Relation.OriginColumns.Any(Function(x) lrResponsibleFactType.RoleGroup.Contains(x.Role))
                                  Select Relation

                If larRelation.Count > 0 Then
                    Throw New Exception($"An RDS.Relation of type PGS Relation Node, already exists for FactType, {arResponsibleFactType.Id}.")
                End If

                '--------------------------------------------------------------------
                'Create the Relation
                '--------------------------------------------------------------------                        
                Dim lrRDSRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                       lrOriginTable,
                                                       pcenumCMMLMultiplicity.Many,
                                                       True,
                                                       True,
                                                       "has",
                                                       lrDestinationTable,
                                                       pcenumCMMLMultiplicity.Many,
                                                       True,
                                                       "has",
                                                       arResponsibleFactType)

                '--------------------------------------
                'Get the Origin/Destination Columns
                Dim larDestinationColumn As New List(Of RDS.Column)
                larDestinationColumn = lrDestinationTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey)
                lrRDSRelation.DestinationColumns = larDestinationColumn

                Dim larOriginColumn As New List(Of RDS.Column)

                larOriginColumn = lrOriginTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey)

                lrRDSRelation.OriginColumns = larOriginColumn
                For Each lrColumn In lrRDSRelation.OriginColumns
                    lrColumn.Relation.Add(lrRDSRelation)
                Next

                If abAddToRDSModel Then Me.RDS.addRelation(lrRDSRelation)

                Return lrRDSRelation

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Sub generateRelationshipForRole(ByRef arRole As FBM.Role)

            If arRole.FactType.IsMDAModelElement Then Exit Sub

            If (arRole.FactType.HasTotalRoleConstraint Or arRole.FactType.HasPartialButMultiRoleConstraint) _
                 And Not arRole.FactType.hasLinkFactTypes Then
                '----------------------------------------------------------------------------------------
                'NB LinkFactTypes are taken care of in generateRelationForManyTo1BinaryFactType (above)

                For Each lrRole In arRole.FactType.RoleGroup
                    Select Case lrRole.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            'Role represents a Column/Attribute/Property rather than a ForeignKey
                        Case Else
                            Call Me.generateRelationForManyToManyFactTypeRole(lrRole)
                    End Select
                Next
            End If

        End Sub


        Public Function HasCoreModel() As Boolean

            If Me.FactType.FindAll(Function(x) x.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString).Count > 0 And
                Not Me.Name = "Core" Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub loadIndexesForTable(ByRef arTable As RDS.Table)

            Dim lrIndex As RDS.Index
            Dim lrColumn As RDS.Column
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            Try
                '-------------------------------------------------
                'Must create a Primary Identifier for the Entity
                '-------------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                lsSQLQuery &= " WHERE Entity = '" & arTable.Name & "'"

                'lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrRecordset = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexIsForEntity, "Entity", arTable.Name)

                While Not lrRecordset.EOF

                    lrIndex = New RDS.Index(arTable, lrRecordset("Index").Data)

                    'Columns 
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString '(Index, Property)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexMakesUseOfProperty, "Index", lrIndex.Name)

                    While Not lrRecordset1.EOF

                        lrColumn = New RDS.Column
                        lrColumn = arTable.Column.Find(Function(x) x.Id = lrRecordset1("Property").Data)

                        'Code Safe. If Column doesn't exist, need to remove it from the Index.
                        If lrColumn Is Nothing Then
                            lrColumn = New RDS.Column(arTable, "Dummy", Nothing, Nothing)
                            lrColumn.Id = lrRecordset1("Property").Data
                            lrIndex.removeColumn(lrColumn)
                        Else
                            lrColumn.Index.AddUnique(lrIndex)

                            lrIndex.Column.Add(lrColumn)
                        End If

                        lrRecordset1.MoveNext()
                    End While

                    lrIndex.ResponsibleRoleConstraint = lrIndex.getResponsibleRoleConstraintFromORMModel()

                    'Restrains to Unique Values
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexRestrainsToUniqueValues.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexRestrainsToUniqueValues, "Index", lrIndex.Name)
                    lrIndex.Unique = Not lrRecordset1.EOF


                    'Is PrimaryKey
                    lsSQLQuery = "SELECT * "
                    lsSQLQuery &= "FROM " & pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexIsPrimaryKey, "Index", lrIndex.Name)
                    lrIndex.IsPrimaryKey = Not lrRecordset1.EOF
                    '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                    'For Each lrColumn In lrIndex.Column
                    '    lrColumn.ContributesToPrimaryKey = Not lrRecordset1.EOF
                    'Next

                    'Ignore Nulls
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIgnoresNulls.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexIgnoresNulls, "Index", lrIndex.Name)
                    lrIndex.IgnoresNulls = Not lrRecordset1.EOF

                    'Direction
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasDirection.ToString '(Index, IndexDirection)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexHasDirection, "Index", lrIndex.Name)
                    lrIndex.AscendingOrDescending.GetByDescription(lrRecordset1("IndexDirection").Data)

                    'Qualifier
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasQualifier.ToString '(Index, Qualifier)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreIndexHasQualifier, "Index", lrIndex.Name)
                    lrIndex.IndexQualifier = lrRecordset1("Qualifier").Data

                    '==========================
                    'Add the Index to the RDS
                    Me.RDS.Index.AddUnique(lrIndex)

                    arTable.Index.AddUnique(lrIndex)

                    lrRecordset.MoveNext()
                End While 'Indexes in the Model for the arTable.

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        ''' <summary>
        ''' PRECONDITION: The Role hasn't already been removed from the FactType.
        ''' </summary>
        ''' <param name="arRole"></param>
        ''' <remarks></remarks>
        Public Sub removeColumnsIndexColumnsForRole(ByRef arRole As FBM.Role)

            Try
                Dim lrRole As FBM.Role = arRole

                '----------------------------------------------------------
                'Columns where the Column's ResponsibleRole is the arRole
                Dim larColumn = From Table In Me.RDS.Table
                                From Column In Table.Column
                                Where Column.Role.Id = lrRole.Id
                                Select Column

                For Each lrColumn In larColumn.ToArray

                    Dim larIndex = From Index In Me.RDS.Index
                                   From Column In Index.Column
                                   Where Column.Role.Id = lrRole.Id
                                   Select Index

                    For Each lrIndex In larIndex.ToList
                        Call lrIndex.removeColumn(lrColumn)
                    Next

                    Call lrColumn.Table.removeColumn(lrColumn)
                Next


                '----------------------------------------------------------
                'Columns where the Column's ActiveRole is the arRole
                larColumn = From Table In Me.RDS.Table
                            From Column In Table.Column
                            Where Column.ActiveRole IsNot Nothing
                            Where Column.ActiveRole.Id = lrRole.Id
                            Select Column

                For Each lrColumn In larColumn.ToArray

                    'Index
                    Dim larIndex = From Index In Me.RDS.Index
                                   From Column In Index.Column
                                   Where Column.Id = lrColumn.Id
                                   Select Index

                    For Each lrIndex In larIndex.ToArray
                        Call lrIndex.removeColumn(lrColumn)
                    Next

                    'Table
                    Call lrColumn.Table.removeColumn(lrColumn)
                Next

                '-------------------------------------------------------
                'Columns for nested/multi-nested ObjectifiedFactTypes.
                Dim lrTable As RDS.Table

                'Code Safe-A Role that references nothing will have no IndexColumns to remove.
                If arRole.JoinedORMObject Is Nothing Then Exit Sub


                If arRole.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then
                    If arRole.FactType.Is1To1BinaryFactType Then
                        lrTable = Me.getTableForResponsibleRole(arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id))

                        If lrTable IsNot Nothing Then
                            Call Me.removeColumnsForObjectifiedFactType(lrTable, arRole.JoinedORMObject)
                        End If
                    Else
                        If arRole.FactType.IsObjectified Then
                            Dim larTable As New List(Of RDS.Table)
                            Dim larProcessedRoles As New List(Of FBM.Role)

                            Me.getTablesWithColumnsThatTransgressRole(arRole, larTable, larProcessedRoles)

                            For Each lrTable In larTable
                                Call Me.removeColumnsForObjectifiedFactType(lrTable, arRole.JoinedORMObject)
                            Next
                        End If
                    End If
                End If

                'All other ResponsibleColumns
                If arRole.hasResponsibleColumns Then
                    For Each lrColumn In arRole.getResponsibleColumns.ToArray
                        Call lrColumn.Table.removeColumn(lrColumn)
                    Next
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub removeRDSRelationsForLinkFactTypesForFactType(ByRef arFactType As FBM.FactType)

            Try
                For Each lrLinkFactType In arFactType.getLinkFactTypes
                    For Each lrRelation In Me.RDS.Relation.FindAll(Function(x) x.ResponsibleFactType Is lrLinkFactType)
                        Call Me.RDS.removeRelation(lrRelation)
                    Next
                Next
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Searches recursively upwards to find tables with Columns that transgress the give Role.
        ''' </summary>
        ''' <param name="arRole">The Role that is transgressed by Columns in a Table.</param>
        ''' <param name="aarTable">The returned list of Tables that transgress the arRole.</param>
        ''' <remarks>Transgression means that the Role is neither the Responsible or ActiveRole of a Column, but is necessary in the formation of the Column.</remarks>
        Public Sub getTablesWithColumnsThatTransgressRole(ByVal arRole As FBM.Role, ByRef aarTable As List(Of RDS.Table), ByRef aarProcessedRoles As List(Of FBM.Role))

            Try

                Dim lrTable As RDS.Table

                Dim larFactType = From FactType In Me.FactType
                                  From Role In FactType.RoleGroup
                                  Where Role.JoinedORMObject.Id = arRole.FactType.Id
                                  Select FactType

                For Each lrFactType In larFactType

                    For Each lrRole In lrFactType.RoleGroup
                        lrTable = Me.getTableForResponsibleRole(lrRole)

                        If lrTable IsNot Nothing Then
                            aarTable.AddUnique(lrTable)
                        End If

                        If Not aarProcessedRoles.Contains(lrRole) Then
                            aarProcessedRoles.Add(lrRole)
                            If lrRole.FactType.IsObjectified Then
                                Call Me.getTablesWithColumnsThatTransgressRole(lrRole, aarTable, aarProcessedRoles)
                            End If
                        End If
                    Next
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Function getDatabaseDataTypeFromORMDataType(ByVal aiTargetDatabaseType As pcenumDatabaseType, ByVal aiORMDataType As pcenumORMDataType) As String

            Try
                'CodeSafe
                If Me.RDS.DatabaseDataType.Count = 0 Then
                    Me.getDatabaseDataTypes(aiTargetDatabaseType)
                End If



                Dim larDBDataType = From DatabaseDataType In Me.RDS.DatabaseDataType
                                    Where DatabaseDataType.BostonDataType = aiORMDataType
                                    Where DatabaseDataType.Database = aiTargetDatabaseType
                                    Select DatabaseDataType

                If larDBDataType.Count = 0 Then
                    Return "<Error>"
                Else
                    Return larDBDataType.First.DataType
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return "<Error>"
            End Try

        End Function

        Public Function getDatabaseDataTypes(ByVal aiTargetDatabaseType As pcenumDatabaseType) As List(Of String)

            Try
                If Me.RDS.DatabaseDataType.Count = 0 Then
                    Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                    Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                    Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                    Me.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList
                End If

                Dim larDBDataType = (From DatabaseDataType In Me.RDS.DatabaseDataType
                                     Where DatabaseDataType.Database = aiTargetDatabaseType
                                     Select DatabaseDataType.DataType).Distinct.ToList

                Return larDBDataType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New List(Of String)
            End Try

        End Function

        Public Function getTableForResponsibleRole(ByVal arRole As FBM.Role) As RDS.Table

            Dim larTable = From Table In Me.RDS.Table
                           From Column In Table.Column
                           Where Column.Role.Id = arRole.Id
                           Select Table

            If larTable.Count > 0 Then
                Return larTable.First
            Else
                Return Nothing
            End If

        End Function

        Public Sub removeColumnsForObjectifiedFactType(ByVal arTargetTable As RDS.Table, ByRef arFactType As FBM.FactType)

            Try
                Dim larRole As New List(Of FBM.Role)
                larRole = arFactType.RoleGroup.ToList

                For Each lrOFTRole In larRole

                    If lrOFTRole.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then
                        '-------------------------------------------------
                        'Recursive, down the nested ObjectifiedFactTypes
                        Call Me.removeColumnsForObjectifiedFactType(arTargetTable, lrOFTRole.FactType)
                    Else
                        Dim larColumn = From Table In Me.RDS.Table
                                        From Column In Table.Column
                                        Where Table.Name = arTargetTable.Name _
                                        And Column.ActiveRole.Id = lrOFTRole.Id _
                                        And Column.ActiveRole.Id <> Column.Role.Id
                                        Select Column

                        For Each lrColumn In larColumn.ToArray

                            Dim larIndex = From Index In Me.RDS.Index
                                           From Column In Index.Column
                                           Where Column.Id = lrColumn.Id _
                                           And Column.ActiveRole.Id <> Column.Role.Id
                                           Select Index

                            For Each lrIndex In larIndex.ToList
                                Me.RDS.Index.Find(Function(x) x.Name = lrIndex.Name).removeColumn(lrColumn)
                            Next

                            Call lrColumn.Table.removeColumn(lrColumn)
                        Next
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub populateRDSRelationsFromCoreMDAElements()

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrOriginTable As RDS.Table
                Dim lsRelationId As String = ""

                '=======================================
                'Map the Relations - Link the Entities 
                '=======================================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim liInd = 1
                While Not lrRecordset.EOF

                    '------------------------
                    'Find the Origin Entity
                    '------------------------

                    lrOriginTable = New RDS.Table
                    lrOriginTable.Name = lrRecordset("Entity").Data
                    lsRelationId = lrRecordset("Relation").Data

                    lrOriginTable = Me.RDS.Table.Find(AddressOf lrOriginTable.Equals)

                    Call Me.populateRDSRelationForOriginTableRelation(lrOriginTable, lsRelationId)

                    Boston.WriteToStatusBar("Populating Relations for Entity: " & lrOriginTable.Name, True, If(liInd < 1, 1, If(liInd > 100, 100, liInd)))
                    liInd += 1

                    lrRecordset.MoveNext()
                End While

                '==================================================================
                'CodeSafe
                Dim larRelation = From Relation In Me.RDS.Relation
                                  Where Relation.OriginColumns.FindAll(Function(x) x.Role Is Nothing).Count > 0
                                  Select Relation

                For Each lrRelation In larRelation.ToArray
                    MsgBox("Removing one redundant relation")
                    Call Me.RDS.removeRelation(lrRelation)
                Next
                '==================================================================

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arBinaryManyToManyTable">The Binary Many-to-Many Table being deleted and the data transferred to the arTargetTable originally linked by the binary table.</param>
        ''' <param name="arTargetTable">That Target Table for the Foreign Key Reference, and as originally linked by arBinaryManyToManyTable.</param>
        Public Sub MigrateManyToOneBinaryTableToForeignKeyOnTargetTable(ByRef arBinaryManyToManyTable As RDS.Table, ByRef arTargetTable As RDS.Table)

            Try
                'PSEUDOCODE
                '* Make sure we can connect to the database;
                '* Check to see that the Target Table is actually linked by the Binary Many-to-Many Table,
                ''     and throw error if not;
                '* Create the New Column on the Target Table;
                '* Create the Foreign Key Reference for and on the new Column created in the previous step;

                Dim lrFactType As FBM.FactType = Nothing

                If Not Me.connectToDatabase(True, False) Then
                    Throw New Exception("Cannot connect to the database.")
                End If

#Region "Make sure is a Binary Many-to-Many Join Table"
                If Not arBinaryManyToManyTable.FBMModelElement.GetType = GetType(FBM.FactType) Then
                    Throw New Exception("Table is not a Binary Many-to-Many Fact Type. FBM Model Element for the Table is not a Fact Type.")
                End If

                lrFactType = arBinaryManyToManyTable.FBMModelElement

                If lrFactType.Arity <> 2 Then
                    Throw New Exception("Table is not a Binary Many-to-Many Fact Type. The Fact Type for the Table is not a Binary Fact Type.")
                End If

                If lrFactType.IsManyTo1BinaryFactType Then
                    Throw New Exception("Table is not a Binary Many-to-Many Fact Type. The Fact Type for the Table is a Binary Fact Type, but is not a Many-to-One Binary Fact Type.")
                End If
#End Region
                Dim lrBinaryFactTypeModelElement = arBinaryManyToManyTable.FBMModelElement
                Dim lrResponsibleRole = lrFactType.RoleGroup.Find(Function(x) Not x.JoinedORMObject Is lrBinaryFactTypeModelElement)
                Dim lrOtherRole = lrFactType.RoleGroup.FindAll(Function(x) x IsNot lrResponsibleRole).First

                Dim larActiveRole As New List(Of FBM.Role)

                Select Case lrOtherRole.JoinedORMObject.GetType
                    Case Is = GetType(FBM.ValueType)
                        larActiveRole.Add(lrOtherRole)

                    Case Is = GetType(FBM.EntityType)
                        If lrOtherRole.JoinsEntityType.HasSimpleReferenceScheme Then
                            larActiveRole.Add(lrOtherRole.JoinsEntityType.ReferenceModeRoleConstraint.Role(0))
                        Else
                            'Compound reference scheme. To do.
                        End If
                End Select

                Dim lrTargetForeignKeyTable = lrResponsibleRole.JoinedORMObject.getCorrespondingRDSTable

                If larActiveRole.Count = 1 Then

                    Dim lrOriginalColumn = arBinaryManyToManyTable.Column.Find(Function(x) x.ActiveRole Is larActiveRole(0))
                    Dim lrOtherOriginalColumn = arBinaryManyToManyTable.Column.Find(Function(x) x.ActiveRole IsNot larActiveRole(0))

                    Dim lrColumn As New RDS.Column(lrTargetForeignKeyTable, lrOriginalColumn.Name, lrResponsibleRole, larActiveRole(0))

                    Call lrTargetForeignKeyTable.addColumn(lrColumn, True, True, True)

#Region "Copy Data from the Binary Many-to-One Table to the Target Foreign Key Table"

                    Dim lrPrimaryKeyColumn As RDS.Column
                    Try
                        lrPrimaryKeyColumn = lrTargetForeignKeyTable.getPrimaryKeyColumns.First
                    Catch ex As Exception
                        Throw New Exception($"There is no Primary Key on the Target Table, {lrTargetForeignKeyTable.Name}.")
                    End Try

                    Dim lsSQLQuery As String = $"UPDATE [{lrTargetForeignKeyTable.Name}] SET {lrColumn.Name} = (SELECT {lrOriginalColumn.Name} FROM [{arBinaryManyToManyTable.Name}] WHERE [{arBinaryManyToManyTable.Name}].{lrOtherOriginalColumn.Name} = {lrTargetForeignKeyTable.Name}.{lrPrimaryKeyColumn.Name} LIMIT 1)"

                    Dim lrRecordset = Me.DatabaseConnection.GONonQuery(lsSQLQuery)
#End Region
                    If lrRecordset.ErrorReturned Then
                        Throw New Exception("Error inserting data from Binary Many-to-One Table into new Foreign Key".AppendDoubleLineBreak(lrRecordset.ErrorString).AppendDoubleLineBreak(lsSQLQuery))
                    Else
                        'Remove the existing Binary Many-to-One Table
                        Me.DatabaseConnection.removeTable(arBinaryManyToManyTable)
                    End If

                Else

                End If




            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub populateRDSRelationForOriginTableRelation(ByRef arOriginTable As RDS.Table, asRelationId As String)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset1 As ORMQL.Recordset
            Dim lrRecordset2 As ORMQL.Recordset
            Dim lrDestinationTable As RDS.Table = Nothing
            Dim lrFactType As FBM.FactType
            Dim lsMessage As String

            Try
                '-----------------------------
                'Find the Destination Entity
                '-----------------------------
                lrDestinationTable = New RDS.Table 'So the name can be set.

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreRelationHasDestinationEntity, "Relation", asRelationId)

                Try
                    lrDestinationTable.Name = lrRecordset1("Entity").Data

                    lrDestinationTable = Me.RDS.Table.Find(AddressOf lrDestinationTable.Equals)
                Catch ex As Exception
                    lsMessage = "Relation with Origin Entity, " & arOriginTable.Name & ", has no Destination Entity"
                    lsMessage.AppendDoubleLineBreak("Boston will remove this Relation from the Relational View as a precaution. Determine which Relation is missing and try and remake the Relation from the Object-Role Model view.")
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True)
                    Dim lrRelation As New RDS.Relation
                    lrRelation.Model = Me.RDS
                    lrRelation.Id = asRelationId
                    Me.RDS.removeRelation(lrRelation)
                End Try

                If lrDestinationTable IsNot Nothing Then

#Region "Multiplicity and Mandatory"
                    '================================
                    Dim liOriginMultiplicity As pcenumCMMLMultiplicity

                    Try
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM CoreOriginMultiplicity"
                        lsSQLQuery &= " WHERE ERDRelation = '" & asRelationId & "'"

                        lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        Dim lsOriginMultiplicity As String = lrRecordset1("Multiplicity").Data

                        Select Case lsOriginMultiplicity
                            Case Is = pcenumCMMLMultiplicity.One.ToString
                                liOriginMultiplicity = pcenumCMMLMultiplicity.One
                            Case Is = pcenumCMMLMultiplicity.Many.ToString
                                liOriginMultiplicity = pcenumCMMLMultiplicity.Many
                        End Select
                    Catch
                        prApplication.ThrowMessage("Error retrieving OriginMultiplicity for RDS.Relation with Origin Table, " & arOriginTable.Name & ", and Destination Table, " & lrDestinationTable.Name,
                                                        pcenumErrorType.Warning,
                                                        abUseFlashCard:=True)
                    End Try

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginIsMandatory.ToString
                    lsSQLQuery &= " WHERE OriginIsMandatory = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbRelationOriginIsMandatory As Boolean = False
                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbRelationOriginIsMandatory = True
                    End If

                    '================================
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreDestinationMultiplicity"
                    lsSQLQuery &= " WHERE ERDRelation = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsDestinationMultiplicity As String = lrRecordset1("Multiplicity").Data

                    Dim liDestinationMultiplicity As pcenumCMMLMultiplicity
                    Select Case lsDestinationMultiplicity
                        Case Is = pcenumCMMLMultiplicity.One.ToString
                            liDestinationMultiplicity = pcenumCMMLMultiplicity.One
                        Case Is = pcenumCMMLMultiplicity.Many.ToString
                            liDestinationMultiplicity = pcenumCMMLMultiplicity.Many
                    End Select

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationIsMandatory.ToString
                    lsSQLQuery &= " WHERE DestinationIsMandatory = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbRelationDestinationIsMandatory As Boolean = False
                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbRelationDestinationIsMandatory = True
                    End If
#End Region

                    '-------------------------------------------------------------------------------
                    'Check to see whether the Relation contributes to the PrimaryKey of the Entity
                    '-------------------------------------------------------------------------------
                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
                    lsSQLQuery &= " WHERE " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & " = '" & asRelationId & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreContributesToPrimaryKey, "CoreContributesToPrimaryKey", asRelationId)

                    Dim lbContributesToPrimaryKey As Boolean = False

                    If lrRecordset1.Facts.Count > 0 Then 'Not using COUNT(*) 'CInt(lrRecordset1("Count").Data) > 0 Then
                        lbContributesToPrimaryKey = True
                    End If

                    '-----------------------------------
                    'Get the FactType for the Relation
                    '-----------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                    lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                    'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreRelationIsForFactType, "Relation", asRelationId)

                    Try
                        lrFactType = New FBM.FactType(Me, lrRecordset1("FactType").Data, True)
                        lrFactType = Me.FactType.Find(AddressOf lrFactType.Equals)
                    Catch ex As Exception
                        Throw New ApplicationException("Can't find Fact Type for Relation  with RelationId: " & asRelationId)
                    End Try

                    Dim lrRelation As New RDS.Relation(asRelationId,
                                                           arOriginTable,
                                                           liOriginMultiplicity,
                                                           lbRelationOriginIsMandatory,
                                                           lbContributesToPrimaryKey,
                                                           "",
                                                           lrDestinationTable,
                                                           liDestinationMultiplicity,
                                                           lbRelationDestinationIsMandatory,
                                                           "",
                                                           lrFactType)


                    'EnforcesOnCascadeUpdate
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreRelationEnforcesOnCascadeUpdate, "Relation", asRelationId)
                    lrRelation.EnforcesOnCascadeUpdate = lrRecordset1.Facts.Count > 0

                    'EnforcesOnCascadeDelete
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreRelationEnforcesOnCascadeDelete, "Relation", asRelationId)
                    lrRelation.EnforcesOnCascadeDelete = lrRecordset1.Facts.Count > 0

                    'EnforcesReferentialIntegrity
                    lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreRelationEnforcesReferentialIntegrity, "Relation", asRelationId)
                    lrRelation.EnforcesReferentialIntegrity = lrRecordset1.Facts.Count > 0

                    'Predicates
#Region "Predicates"
                    lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                        'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreOriginPredicate, "Relation", asRelationId)
                        If Not lrRecordset1.EOF Then
                            lrRelation.OriginPredicate = lrRecordset1("Predicate").Data
                        End If


                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                        lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                        'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreDestinationPredicate, "Relation", asRelationId)
                        If Not lrRecordset1.EOF Then
                            lrRelation.DestinationPredicate = lrRecordset1("Predicate").Data
                        End If
#End Region

                        '==============================================================
                        'Get the Columns and their OrdinalPositions for the Relation.
                        Dim larColumn As New List(Of RDS.Column)
                        Dim lrColumn As RDS.Column
                        Dim lrDictionary As New SortedDictionary(Of Integer, String)

#Region "Origin Columns"
                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lrRelation.Id & "'"

                        'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin, "Relation", lrRelation.Id)

                        While Not lrRecordset1.EOF

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                            lsSQLQuery &= " WHERE RelationAttribute = '" & lrRecordset1.CurrentFact.Id & "'"

                            'lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrRecordset2 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition, "RelationAttribute", lrRecordset1.CurrentFact.Id)

                            While lrDictionary.ContainsKey(lrRecordset2("OrdinalPosition").Data)
                                lrRecordset2("OrdinalPosition").Data += 1
                                lrRecordset2("OrdinalPosition").makeDirty()
                            End While

                            lrDictionary.Add(CInt(lrRecordset2("OrdinalPosition").Data), lrRecordset1("Attribute").Data)

                            lrRecordset1.MoveNext()
                        End While

                        For Each lrDictionaryEntry In lrDictionary

                            lrColumn = New RDS.Column
                            Dim larOriginColumn = From Table In Me.RDS.Table
                                                  From Column In Table.Column
                                                  Where Column.Table.Name = lrRelation.OriginTable.Name
                                                  Where Column.Id = lrDictionaryEntry.Value
                                                  Select Column

                            If larOriginColumn.Count > 0 Then
                                lrColumn = larOriginColumn.First
                                lrColumn.Relation.AddUnique(lrRelation)
                                lrRelation.OriginColumns.AddUnique(lrColumn)
                            Else
                                prApplication.ThrowMessage("Relation: " & lrRelation.Id & ", had no origin column with " & lrDictionaryEntry.Value, pcenumErrorType.Warning, Nothing, False, False, False)
                            End If
                        Next
#End Region

#Region "Destination Columns"
                        'DestinationColumns
                        lrDictionary.Clear()

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                        lsSQLQuery &= " WHERE Relation = '" & lrRelation.Id & "'"

                        'lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        lrRecordset1 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination, "Relation", lrRelation.Id)

                        While Not lrRecordset1.EOF

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
                            lsSQLQuery &= " WHERE RelationAttribute = '" & lrRecordset1.CurrentFact.Id & "'"

                            'lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrRecordset2 = Me.GetCoreKeyValue(pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition, "RelationAttribute", lrRecordset1.CurrentFact.Id)

                            Try
                                lrDictionary.Add(CInt(lrRecordset2("OrdinalPosition").Data), lrRecordset1("Attribute").Data)
                            Catch ex As Exception
                                'not ideal, but don't stop here
                                prApplication.ThrowMessage(ex.Message, pcenumErrorType.Warning, ex.StackTrace, True, False, False)
                            End Try

                            lrRecordset1.MoveNext()
                        End While

                        For Each lrDictionaryEntry In lrDictionary

                            lrColumn = New RDS.Column
                        Try
                            lrColumn = (From Table In Me.RDS.Table
                                        From Column In Table.Column
                                        Where Column.Table.Name = lrRelation.DestinationTable.Name
                                        Where Column.Id = lrDictionaryEntry.Value
                                        Select Column).First

                            '20240223-VM-Only need to do this on the Origin side. The Origin Column 'has' the Relation.
                            'lrColumn.Relation.Add(lrRelation)
                            lrRelation.DestinationColumns.Add(lrColumn)
                        Catch ex As Exception
                            'CodeSafe
                            Dim lrGhostColumn = From Table In Me.RDS.Table
                                                From Column In Table.Column
                                                Where Column.Id = lrDictionaryEntry.Value
                                                Select Column

                            If lrGhostColumn.Count = 0 Then
                                'The Column doesn't exist anywhere in the RDS model, so get rid of it.
                                Call lrRelation.RemoveDestinationColumn(New RDS.Column(lrRelation.DestinationTable, "DummyColumn", Nothing, Nothing, , lrDictionaryEntry.Value))
                            Else
                                lsMessage = "Could not find Column in Destination Table, " & lrRelation.DestinationTable.Name
                                lsMessage.AppendDoubleLineBreak("Origin Table: " & lrRelation.OriginTable.Name)
                                lsMessage.AppendLine("Origin Column count: " & lrRelation.OriginColumns.Count)
                                lsMessage.AppendLine("Destination Column count: " & lrRelation.DestinationColumns.Count)
                                lsMessage.AppendDoubleLineBreak("Column Id: " & lrDictionaryEntry.Value)

                                Dim lrModelError = New FBM.ModelError(pcenumModelErrors.CMMLModelError,
                                                                lsMessage,
                                                                Nothing,
                                                                Nothing,
                                                                False,
                                                                pcenumModelSubErrorType.RDSRelationOriginDestintaionColumnCountMismatch,
                                                                lrRelation)
                                Me.AddModelError(lrModelError)

                                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True, , True)
                            End If
                        End Try
                    Next

                    If lrRelation.is1To1BinaryRelation Then
                        Call lrRelation.establishReverseColumns()
                    End If

                    Me.RDS.Relation.Add(lrRelation)

                End If 'DestinationTable IsNot Nothing

#End Region

            Catch AppEx As ApplicationException

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & AppEx.Message
                Try
                    lsMessage.AppendLine("Origin Table: " & arOriginTable.Name)
                    lsMessage.AppendLine("Destination Table: " & lrDestinationTable.Name)

                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, AppEx.StackTrace)
                Catch ex As Exception
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, AppEx.StackTrace)
                End Try

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

    End Class

End Namespace
