Imports System.Reflection
Imports System.Threading.Tasks

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
                    Richmond.WriteToStatusBar("Creating Entity, '" & lsEntityName & "'")

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

                    Richmond.WriteToStatusBar("Creating MandatoryConstraint for Attribute, '" & lsAttributeName & "', for Entity, '" & lsEntityName & "'", True)

                    lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'" 'lsAttributeName & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                '=========================================================================
                'Check if the Role represents the PK of an Entity with a SimpleReferenceScheme
                Dim lsIndexName As String = ""
                lsIndexName = Viev.Strings.RemoveWhiteSpace(lrAttribute.Entity.Id & "PK")

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                Richmond.WriteToStatusBar("Moving Primary Keys to the top of each Enitity.", True)
                If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(95)
                For Each lrTable In Me.RDS.Table
                    Call lrTable.movePrimaryKeyColumnsToTopOrdinalPosition()
                Next

                Richmond.WriteToStatusBar("Completed creating the Entity Relationship Diagram Artifacts", True)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                                        lrColumn.Name = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lrColumn.Name))
                                        lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
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
                                lrColumn.Name = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lrColumn.Name))
                                lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

                            Richmond.WriteToStatusBar("Creating Entity, '" & lrFactType.Name & "'")

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

                    '            Richmond.WriteToStatusBar("Creating Entity, '" & lrFactType.Name & "'")

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub generateERDIndexes()

            Richmond.WriteToStatusBar("Generating the Indexes.", True)

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
                Dim larEntityType = From EntityType In Me.EntityType _
                                    Where EntityType.IsMDAModelElement = False _
                                    And EntityType.HasCompoundReferenceMode _
                                    Select EntityType

                For Each lrEntityType In larEntityType

                    Dim lrRoleConstraint As FBM.RoleConstraint = lrEntityType.ReferenceModeRoleConstraint

                    larColumn = New List(Of RDS.Column)
                    For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                        Dim lrNearestRole As FBM.Role = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id)

                        Dim larColumns = From Table In Me.RDS.Table _
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub generateIndexesForSimpleReferenceSchemes()

            Dim lrColumn As RDS.Column
            Dim larColumn As List(Of RDS.Column)

            '====================================================
            'Generate the Indexes for SimpleReferenceSchemes
            '=================================================
            Try
                Dim larRole = From RoleConstraint In Me.RoleConstraint _
                              Where RoleConstraint.IsMDAModelElement = False _
                              And RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                              And RoleConstraint.RoleConstraintRole.Count = 1 _
                              And RoleConstraint.IsPreferredIdentifier = True _
                              Select RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(RoleConstraint.RoleConstraintRole(0).Role.Id)

                For Each lrRole In larRole

                    Dim larColumns = From Table In Me.RDS.Table _
                                    From Column In Table.Column
                                    Where Column.Role.Id = lrRole.Id _
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub generateERDRelationships()

            Try
                Dim lrFactType As FBM.FactType

                Richmond.WriteToStatusBar("Creating Relationships.", True)

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub generateRelationForReassignedRole(ByRef arRole As FBM.Role)

            Try
                Dim lrRole As FBM.Role = arRole

                Dim larRelation = From Relation In Me.RDS.Relation _
                                  Where Relation.ResponsibleFactType.Id = lrRole.FactType.Id _
                                  Select Relation

                Dim lrResponsibleRole As FBM.Role

                If (lrRole.FactType.HasTotalRoleConstraint Or lrRole.FactType.HasPartialButMultiRoleConstraint) And lrRole.FactType.InternalUniquenessConstraint.Count > 0 Then

                    lrResponsibleRole = lrRole

                    If lrRole.FactType.IsObjectified Then
                        Dim larLinkFactTypeRole = From FactType In lrRole.Model.FactType _
                                                  Where FactType.IsLinkFactType = True _
                                                  And FactType.LinkFactTypeRole Is lrResponsibleRole _
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
                            Case Is = pcenumConceptType.EntityType, _
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

                        Richmond.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

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

                        'Reciprocal relations for 'Destination' Table's Columns
                        larDestinationColumn = New List(Of RDS.Column)
                        larDestinationColumn = lrOriginTable.Column.FindAll(Function(x) x.isPartOfPrimaryKey = True)

                        For Each lrOriginColumn In larDestinationColumn
                            For Each lrColumn In lrDestinationTable.Column.FindAll(Function(x) x.ActiveRole.Id = lrOriginColumn.ActiveRole.Id)
                                lrColumn.Relation.Add(lrRelation)
                            Next
                        Next


                End Select 'GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject.ConceptType

                '==================================================================

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                        Richmond.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub generateRelationForManyTo1BinaryFactType(ByRef arRole As FBM.Role)

            Try
                '====================
                'RDS
                Dim lrOriginTable As RDS.Table = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lbContributesToPrimaryKey As Boolean = False

                Dim lrRole As FBM.Role = arRole

                Dim lrOtherRole As FBM.Role = arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id)

                Select Case lrOtherRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        'We don't generate Relations for ManyToOneFactTypes that join to a ValueType
                    Case Is = pcenumConceptType.EntityType, _
                              pcenumConceptType.FactType

                        lrOriginTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.JoinedORMObject.Id)
                        lrDestinationTable = Me.RDS.Table.Find(Function(x) x.Name = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject.Id)

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
                        Richmond.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                        Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                           lrOriginTable, _
                                                           pcenumCMMLMultiplicity.Many, _
                                                           arRole.Mandatory, _
                                                           lbContributesToPrimaryKey, _
                                                           lsOriginPredicate, _
                                                           lrDestinationTable, _
                                                            pcenumCMMLMultiplicity.One, _
                                                           arRole.FactType.GetOtherRoleOfBinaryFactType(arRole.Id).Mandatory, _
                                                           lsDestinationPredicate, _
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

                        Select Case lrOtherRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                For Each lrColumn In lrRelation.DestinationColumns
                                    If arRole.FactType.IsLinkFactType Then
                                        larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.FactType.LinkFactTypeRole.Id _
                                                                                          And x.ActiveRole.Id = lrColumn.ActiveRole.Id))
                                    Else
                                        If lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                                                                     And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Is Nothing Then
                                            '=====================================================
                                            'The Column doesn't exist in the Table yet.
                                            Dim lrNewColumn = New RDS.Column(lrOriginTable, lrColumn.ActiveRole.JoinedORMObject.Id, lrRole, lrColumn.ActiveRole, lrRole.Mandatory)
                                            lrOriginTable.addColumn(lrNewColumn)
                                        End If
                                        larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.Role.Id = lrRole.Id _
                                                                                      And x.ActiveRole.Id = lrColumn.ActiveRole.Id))

                                    End If
                                Next
                            Case Is = pcenumConceptType.FactType
                                For Each lrColumn In lrDestinationTable.Column
                                    larOriginColumn.Add(lrOriginTable.Column.Find(Function(x) x.ActiveRole.Id = lrColumn.ActiveRole.Id _
                                                                                      And Not x.Role.FactType.IsPreferredReferenceMode))
                                Next
                        End Select

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
                            Try
                                Dim lrColumn = arRole.JoinedORMObject.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.Id = lrRole.Id)
                                If lrColumn IsNot Nothing Then
                                    lrColumn.Relation.Add(lrRelation)
                                    lrRelation.OriginColumns.Add(lrColumn)
                                End If
                            Catch ex As Exception
                                'Not a good place to be. Can only create the OriginColumns once the PrimaryKey is created for the DestinationTable.
                            End Try

                        End If

                        Me.RDS.addRelation(lrRelation)

                End Select 'GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject.ConceptType


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

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

                Dim larRelation = From Relation In Me.RDS.Relation _
                                  Where Relation.ResponsibleFactType.Id = arResponsibleRole.FactType.Id _
                                  And Relation.OriginTable Is lrOriginTable _
                                  And Relation.DestinationTable Is lrDestinationTable _
                                  And Relation.OriginColumns.FindAll(Function(x) x.Role Is arResponsibleRole).Count > 0
                                  Select Relation

                If larRelation.Count > 0 Then
                    Exit Sub
                End If

                Richmond.WriteToStatusBar("Creating Relationship between Entities, '" & lrOriginTable.Name & "', and , '" & lrDestinationTable.Name & "'", True)

                '--------------------------------------------------------------------
                'Create the Relation
                '--------------------------------------------------------------------                        
                Dim lrRelation As New RDS.Relation(System.Guid.NewGuid.ToString,
                                                   lrOriginTable, _
                                                   pcenumCMMLMultiplicity.Many, _
                                                   True, _
                                                   lbContributesToPrimaryKey, _
                                                   "Is involved in", _
                                                   lrDestinationTable, _
                                                    pcenumCMMLMultiplicity.One, _
                                                   arResponsibleRole.Mandatory, _
                                                   "Involves", _
                                                   arResponsibleRole.FactType)

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

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

            If Me.FactType.FindAll(Function(x) x.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString).Count > 0 And _
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

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lrIndex = New RDS.Index(arTable, lrRecordset("Index").Data)

                    'Columns 
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString '(Index, Property)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

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

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.Unique = Not lrRecordset1.EOF


                    'Is PrimaryKey
                    lsSQLQuery = "SELECT * "
                    lsSQLQuery &= "FROM " & pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.IsPrimaryKey = Not lrRecordset1.EOF
                    '20210505-VM-Not needed because IsPartOfPrimaryKey is a function of Table Indexes
                    'For Each lrColumn In lrIndex.Column
                    '    lrColumn.ContributesToPrimaryKey = Not lrRecordset1.EOF
                    'Next

                    'Ignore Nulls
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIgnoresNulls.ToString
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.IgnoresNulls = Not lrRecordset1.EOF

                    'Direction
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasDirection.ToString '(Index, IndexDirection)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrIndex.AscendingOrDescending.GetByDescription(lrRecordset1("IndexDirection").Data)

                    'Qualifier
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexHasQualifier.ToString '(Index, Qualifier)
                    lsSQLQuery &= " WHERE Index = '" & lrIndex.Name & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="aoBackgroundWorker">To report progress. Start at 80%.</param>
        Public Sub PopulateRDSStructureFromCoreMDAElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Try
                Me.RDSLoading = True
                Dim lsMessage As String

                Dim lsSQLQuery As String = ""
                Dim lrTable As RDS.Table
                Dim lrColumn As RDS.Column

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Entity'"

                Dim lrORMRecordset,
                    lrORMRecordset2,
                    lrORMRecordset3 As ORMQL.Recordset

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrModelElement As FBM.ModelObject
                Dim lsColumnName As String = ""
                Dim lrResponsibleRole As FBM.Role
                Dim lrActiveRole As FBM.Role
                Dim liInd As Integer = 0 'For reporting progress on Tables loaded.

                While Not lrORMRecordset.EOF
#Region "Tables"
                    '-----------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(lrORMRecordset("Element").Data)

                    If lrModelElement Is Nothing Then
                        'This is dire. Create a dummy FBMEntityType for the Table, and add the EntityType to the Model. 
                        '  The user can then elect to delete the EntityType/Table if it shouldn't be in the model
                        Dim lrEntityType = Me.CreateEntityType(lrORMRecordset("Element").Data, True)
                        lrModelElement = lrEntityType

                        'Let the user know what happened.
                        lsMessage = "The Table, '" & lrModelElement.Name & "', within the relational model had no corresponding Object-Role Model model element."
                        lsMessage &= vbCrLf & vbCrLf & "An Entity Type has been created in the model to cater for this. If you no longer need this model element remove it from the model."
                        Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical)
                    End If


                    lrTable = New RDS.Table(Me.RDS, lrORMRecordset("Element").Data, lrModelElement)
                    Me.RDS.Table.Add(lrTable)

                    'PGS Relation
                    lsSQLQuery = " SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                    lsSQLQuery &= " WHERE IsPGSRelation = '" & lrTable.Name & "'"

                    lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrORMRecordset3(0).Data > 0 Then
                        lrTable.isPGSRelation = True
                    End If

                    lrORMRecordset.MoveNext()
#End Region
                End While 'Stepping through Tables

                Dim larSortedTables = Me.RDS.Table.OrderBy(Function(x) x.getSupertypeTables.Count)

                For Each lrTable In larSortedTables

                    '==========================================================================================================
                    'Columns
                    lsSQLQuery = " SELECT *"
                    lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " WHERE ModelObject = '" & lrTable.Name & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsColumnId As String = "" 'Used also for Debugging/ErrorThrowing.
                    Dim lrSupertypeColumn As RDS.Column = Nothing

                    While Not lrORMRecordset2.EOF

                        Try
                            lsColumnId = lrORMRecordset2("Attribute").Data

                            'Responsible Role
                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                            lsSQLQuery &= " WHERE Property = '" & lsColumnId & "'"

                            lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                            lrResponsibleRole = Me.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)

                            Dim lrResponsibleRoleTable As RDS.Table = lrResponsibleRole.getCorrespondingRDSTable
                            If lrTable IsNot lrResponsibleRoleTable And lrResponsibleRoleTable.Column.Count > 0 Then

                                lrSupertypeColumn = lrResponsibleRoleTable.Column.Find(Function(x) x.Id = lsColumnId)
                                If lrSupertypeColumn Is Nothing Then
                                    'CodeSafe...fall back to ResponsibleRole
                                    lrSupertypeColumn = lrResponsibleRoleTable.Column.Find(Function(x) x.Role.Id = lrResponsibleRole.Id)
                                End If

                                Dim lrNewColumn = lrSupertypeColumn.Clone(lrTable, Nothing, True)

                                Dim lrExistingColumn = lrTable.Column.Find(Function(x) x.Role Is lrNewColumn.Role And x.ActiveRole Is lrNewColumn.ActiveRole)
                                If lrExistingColumn Is Nothing Then
                                    Call lrTable.Column.Add(lrNewColumn)
                                End If
                            Else
                                'Column Name
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM CorePropertyHasPropertyName" '(Property, PropertyName)
                                lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lsColumnName = lrORMRecordset3("PropertyName").Data

                                'Active Role
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                                lsSQLQuery &= " WHERE Property = '" & lrORMRecordset2("Attribute").Data & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lrActiveRole = Me.Role.Find(Function(x) x.Id = lrORMRecordset3("Role").Data)

                                'New Column
                                lrColumn = New RDS.Column(lrTable, lsColumnName, lrResponsibleRole, lrActiveRole)
                                lrColumn.Id = lsColumnId

                                'IsMandatory
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIsMandatory.ToString
                                lsSQLQuery &= " WHERE IsMandatory = '" & lrColumn.Id & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lrColumn.IsMandatory = Not lrORMRecordset3.EOF

                                'Fact Type
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lrColumn.FactType = Me.FactType.Find(Function(x) x.Id = lrORMRecordset3("FactType").Data)

                                'Ordinal Position
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                                lsSQLQuery &= " WHERE Property = '" & lrColumn.Id & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                                lrColumn.OrdinalPosition = lrORMRecordset3("Position").Data

                                'Is Derived Fact Type Parameter
                                lsSQLQuery = " SELECT COUNT(*)"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                                lsSQLQuery &= " WHERE IsDerivedFactTypeParameter = '" & lrColumn.Id & "'"

                                lrORMRecordset3 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                                If lrORMRecordset3(0).Data > 0 Then
                                    lrColumn.IsDerivationParameter = True
                                End If

                                lrTable.Column.Add(lrColumn)
                            End If

                        Catch ex As Exception

                            If My.Settings.AutomaticallyDeleteTroublesomeColumns Then
                                Call Me.removeCMMLAttribute(lrTable.Name, lsColumnId)
                            Else

                                Dim lsErrorMessage As String = "Trouble loading Column for Table, " & lrTable.Name & ". Column.Id = " & lsColumnId
                                lsErrorMessage &= vbCrLf & vbCrLf & "Skipping loading of the Column from the ORM model (only) as a precaution."
                                If Me.IsDatabaseSynchronised Then
                                    lsErrorMessage &= " Contact support for instructions how to fix this problem."
                                End If
                                lsErrorMessage.AppendDoubleLineBreak("Optionally you can delete the Column from the database altogether. Click [Yes] to delete the Column, or [No] to keep the Column.")
                                lsErrorMessage.AppendString(" You should only delete the Column if you know what you are doing or as advised by support. Backup your database before deleting core elements.")

                                'Dim lbDatabaseSynchronisation As Boolean = Me.IsDatabaseSynchronised
                                'Me.IsDatabaseSynchronised = False
                                'Call Me.removeCMMLAttribute(lrTable.Name, lsColumnId)
                                'Me.IsDatabaseSynchronised = lbDatabaseSynchronisation

                                lsErrorMessage &= vbCrLf & vbCrLf & ex.StackTrace
                                If prApplication.ThrowErrorMessage(lsErrorMessage, pcenumErrorType.Information,
                                                                        ex.StackTrace,
                                                                        False,
                                                                        False,
                                                                        True,
                                                                        MessageBoxButtons.YesNo) = DialogResult.Yes Then
                                    '20210813-VM-If this gets out of hand, remove this functionality.
                                    Call Me.removeCMMLAttribute(lrTable.Name, lsColumnId)
                                End If
                            End If
                        End Try

                        lrORMRecordset2.MoveNext()
                    End While
                    '==========================================================================================================

                    '===========================
                    'Indexes                    
                    Call Me.loadIndexesForTable(lrTable)

                    Dim larNullActiveRoles = From Column In lrTable.Column
                                             Where Column.ActiveRole Is Nothing
                                             Select Column

                    If larNullActiveRoles.Count > 0 Then
                        'CodeSafe
                        For Each lrColumn In larNullActiveRoles.ToArray
                            Try
                                If lrColumn.Role.FactType.Id = lrColumn.Table.Name And
                                    lrColumn.Role.FactType.IsObjectified Then
                                    If lrColumn.Role.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                                        lrColumn.ActiveRole = lrColumn.Role
                                        Call Me.updateORSetCMMLPropertyActiveRole(lrColumn)
                                    End If
                                End If
                            Catch ex As Exception
                                'CodeSafe
                                If lrColumn.ActiveRole Is Nothing And lrColumn.Role Is Nothing And lrColumn.FactType Is Nothing Then
                                    Call lrTable.removeColumn(lrColumn)
                                End If
                            End Try
                        Next
                    End If

                    If aoBackgroundWorker IsNot Nothing Then aoBackgroundWorker.ReportProgress(Viev.Lesser(99, 80 + CInt(19 * (liInd / larSortedTables.Count))))
                    liInd += 1 'For reporting progress on Tables loaded.
                Next

                '==========================================================================================================
                'Relations                

                Call Me.populateRDSRelationsFromCoreMDAElements()

                '==========================================================
                'State Transition Model
                '  NB Is called from within this thread so as to not clash on the ORMQL Parser.
                If CDbl(Viev.NullVal(Me.CoreVersionNumber, 0)) >= 2.1 Then
                    Call Me.PopulateSTMStructureFromCoreMDAElements()
                End If

                Me.RDSLoading = False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Me.RDSLoading = False
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

                Dim larFactType = From FactType In Me.FactType _
                                  From Role In FactType.RoleGroup _
                                  Where Role.JoinedORMObject.Id = arRole.FactType.Id _
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function getTableForResponsibleRole(ByVal arRole As FBM.Role) As RDS.Table

            Dim larTable = From Table In Me.RDS.Table _
                           From Column In Table.Column _
                           Where Column.Role.Id = arRole.Id _
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
                        Dim larColumn = From Table In Me.RDS.Table _
                                        From Column In Table.Column _
                                        Where Table.Name = arTargetTable.Name _
                                        And Column.ActiveRole.Id = lrOFTRole.Id _
                                        And Column.ActiveRole.Id <> Column.Role.Id _
                                        Select Column

                        For Each lrColumn In larColumn.ToArray

                            Dim larIndex = From Index In Me.RDS.Index _
                                           From Column In Index.Column _
                                           Where Column.Id = lrColumn.Id _
                                           And Column.ActiveRole.Id <> Column.Role.Id _
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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

                While Not lrRecordset.EOF

                    '------------------------
                    'Find the Origin Entity
                    '------------------------

                    lrOriginTable = New RDS.Table
                    lrOriginTable.Name = lrRecordset("Entity").Data
                    lsRelationId = lrRecordset("Relation").Data

                    lrOriginTable = Me.RDS.Table.Find(AddressOf lrOriginTable.Equals)

                    Call Me.populateRDSRelationForOriginTableRelation(lrOriginTable, lsRelationId)

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
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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

                lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Try
                    lrDestinationTable.Name = lrRecordset1("Entity").Data

                    lrDestinationTable = Me.RDS.Table.Find(AddressOf lrDestinationTable.Equals)
                Catch ex As Exception
                    lsMessage = "Relation with Origin Entity, " & arOriginTable.Name & ", has no Destination Entity"
                    lsMessage.AppendDoubleLineBreak("Boston will remove this Relation from the Relational View as a precaution. Determine which Relation is missing and try and remake the Relation from the Object-Role Model view.")
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True)
                    Dim lrRelation As New RDS.Relation
                    lrRelation.Id = asRelationId
                    Me.RDS.removeRelation(lrRelation)
                End Try

                If lrDestinationTable IsNot Nothing Then

                    '================================
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreOriginMultiplicity"
                    lsSQLQuery &= " WHERE ERDRelation = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lsOriginMultiplicity As String = lrRecordset1("Multiplicity").Data

                    Dim liOriginMultiplicity As pcenumCMMLMultiplicity
                    Select Case lsOriginMultiplicity
                        Case Is = pcenumCMMLMultiplicity.One.ToString
                            liOriginMultiplicity = pcenumCMMLMultiplicity.One
                        Case Is = pcenumCMMLMultiplicity.Many.ToString
                            liOriginMultiplicity = pcenumCMMLMultiplicity.Many
                    End Select

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

                    '-------------------------------------------------------------------------------
                    'Check to see whether the Relation contributes to the PrimaryKey of the Entity
                    '-------------------------------------------------------------------------------
                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString
                    lsSQLQuery &= " WHERE " & pcenumCMMLRelations.CoreContributesToPrimaryKey.ToString & " = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lbContributesToPrimaryKey As Boolean = False

                    If CInt(lrRecordset1("Count").Data) > 0 Then
                        lbContributesToPrimaryKey = True
                    End If

                    '-----------------------------------
                    'Get the FactType for the Relation
                    '-----------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                    lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

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

                    'Predicates
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                    lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    If Not lrRecordset1.EOF Then
                        lrRelation.OriginPredicate = lrRecordset1("Predicate").Data
                    End If


                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                    lsSQLQuery &= " WHERE Relation = '" & asRelationId & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    If Not lrRecordset1.EOF Then
                        lrRelation.DestinationPredicate = lrRecordset1("Predicate").Data
                    End If

                    '==============================================================
                    'Get the Columns and their OrdinalPositions for the Relation.
                    Dim larColumn As New List(Of RDS.Column)
                    Dim lrColumn As RDS.Column
                    Dim lrDictionary As New SortedDictionary(Of Integer, String)

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                    lsSQLQuery &= " WHERE Relation = '" & lrRelation.Id & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset1.EOF

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                        lsSQLQuery &= " WHERE RelationAttribute = '" & lrRecordset1.CurrentFact.Id & "'"

                        lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

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
                            lrColumn.Relation.Add(lrRelation)
                            lrRelation.OriginColumns.Add(lrColumn)
                        Else
                            prApplication.ThrowErrorMessage("Relation: " & lrRelation.Id & ", had no origin column with " & lrDictionaryEntry.Value, pcenumErrorType.Warning, Nothing, False, False, False)
                        End If
                    Next

                    'DestinationColumns
                    lrDictionary.Clear()

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                    lsSQLQuery &= " WHERE Relation = '" & lrRelation.Id & "'"

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset1.EOF

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
                        lsSQLQuery &= " WHERE RelationAttribute = '" & lrRecordset1.CurrentFact.Id & "'"

                        lrRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrDictionary.Add(CInt(lrRecordset2("OrdinalPosition").Data), lrRecordset1("Attribute").Data)

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

                            lrColumn.Relation.Add(lrRelation)
                            lrRelation.DestinationColumns.Add(lrColumn)
                        Catch ex As Exception
                            'CodeSafe
                            lsMessage = "Could not find Column in Destination Table, " & lrRelation.DestinationTable.Name
                            lsMessage.AppendLine("Origin Table: " & lrRelation.OriginTable.Name)
                            lsMessage.AppendLine("Column Id: " & lrDictionaryEntry.Value)
                            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True)
                        End Try
                    Next

                    If lrRelation.is1To1BinaryRelation Then
                        Call lrRelation.establishReverseColumns()
                    End If

                    Me.RDS.Relation.Add(lrRelation)

                End If 'DestinationTable IsNot Nothing

            Catch AppEx As ApplicationException

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & AppEx.Message
                Try
                    lsMessage.AppendLine("Origin Table: " & arOriginTable.Name)
                    lsMessage.AppendLine("Destination Table: " & lrDestinationTable.Name)

                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, AppEx.StackTrace)
                Catch ex As Exception
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, AppEx.StackTrace)
                End Try

            Catch ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class

End Namespace
