Imports System.Reflection

Namespace FBM

    Partial Public Class Model 'See also the folder ORMRDS/tORMModel.vb

        Public Event StateTransitionAdded(ByRef lrFact As FBM.Fact)

        Public Sub addCMMLColumnToRelationOrigin(ByRef arRelation As RDS.Relation,
                                                 ByRef arColumn As RDS.Column,
                                                 ByVal aiOrdinalPosition As Integer)

            Dim lsSQLQuery As String
            Dim lrFact As FBM.Fact

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
            lsSQLQuery &= " (Attribute, Relation)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & arColumn.Id & "'"
            lsSQLQuery &= ",'" & arRelation.Id & "'"
            lsSQLQuery &= " )"

            lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
            lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lrFact.Id & "'"
            lsSQLQuery &= ",'" & aiOrdinalPosition & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub

        Public Sub addCMMLColumnToIndex(ByRef arIndex As RDS.Index,
                                        ByRef arColumn As RDS.Column)

            Try

                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString & " (Index, Property) VALUES ("
                lsSQLQuery &= "'" & arIndex.Name & "','" & arColumn.Id & "'"
                lsSQLQuery &= ")"
                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addCMMLColumnToRelationDestination(ByRef arRelation As RDS.Relation,
                                                      ByRef arColumn As RDS.Column,
                                                      ByVal aiOrdinalPosition As Integer)

            Dim lsSQLQuery As String
            Dim lrFact As FBM.Fact

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
            lsSQLQuery &= " (Attribute, Relation)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & arColumn.Id & "'"
            lsSQLQuery &= ",'" & arRelation.Id & "'"
            lsSQLQuery &= " )"

            lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
            lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= "'" & lrFact.Id & "'"
            lsSQLQuery &= ",'" & aiOrdinalPosition & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


        End Sub

        Public Sub addCMMLIsPGSRelation(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " (IsPGSRelation)"
                lsSQLQuery &= " VALUES ('" & arTable.Name & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addCMMLIsDerivedFactTypeParameter(ByRef arColumn As RDS.Column)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                lsSQLQuery &= " (IsDerivedFactTypeParameter)"
                lsSQLQuery &= " VALUES ('" & arColumn.Id & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStartState">The State for a ValueType that is a start of a STM.</param>
        Public Function addCMMLStartState(ByRef arStartState As FBM.STM.State) As FBM.Fact

            Try

                Dim lsSQLQuery As String

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
                lsSQLQuery &= " (ValueType, CoreElement, Event)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStartState.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStartState.Id & "'"
                lsSQLQuery &= ",''"
                lsSQLQuery &= " )"

                Return Me.ORMQL.ProcessORMQLStatement(lsSQLQuery) 'As Fact

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
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStateTransition"></param>
        Public Sub addCMMLStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Try

                Dim lsSQLQuery As String
                Dim lrFact As FBM.Fact

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " (ValueType, Concept1, Concept2, Event)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStateTransition.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.FromState.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.ToState.Id & "'"
                lsSQLQuery &= ",'" & arStateTransition.Event & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arStateTransition.Id = lrFact.Id

                arStateTransition.Fact = lrFact

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' For the STM (State Transition Model), for Value Type/Value Constraints within the Model.
        ''' </summary>
        ''' <param name="arStopState">The State for a ValueType that is a Stop of a STM.</param>
        Public Sub addCMMLStopState(ByRef arStopState As FBM.STM.State)

            Try

                Dim lsSQLQuery As String
                Dim lrFact As FBM.Fact

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " (ValueType, CoreElement)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arStopState.ValueType.Id & "'"
                lsSQLQuery &= ",'" & arStopState.Id & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Changes the name of an Attribute in the RDS
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Sub changeCMMLAttributeName(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
            lsSQLQuery &= " SET PropertyName = '" & arColumn.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub changeCMMLAttributeEntityForColumn(ByRef arColumn As RDS.Column, ByRef arTable As RDS.Table)

            Try

                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreERDAttribute.ToString
                lsSQLQuery &= " SET ModelObject = '" & arTable.Name & "'"
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= "   AND ModelObject = '" & arColumn.Table.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub changeCMMLStateName(ByRef arState As STM.State, ByVal asOldStateName As String)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreStateHasName.ToString
                lsSQLQuery &= " SET StateName = '" & arState.Name & "'"
                lsSQLQuery &= " WHERE State = '" & arState.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Connects to the database if it is not already connected
        ''' </summary>
        Public Function connectToDatabase(Optional abForceConnection As Boolean = False) As Boolean

            Try
                If abForceConnection Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseConnection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseManager.Connection Is Nothing Then
                    'Try and establish a connection
                    Call Me.DatabaseManager.establishConnection(Me.TargetDatabaseType, Me.TargetDatabaseConnectionString)
                    If Me.DatabaseConnection Is Nothing Then
                        Throw New Exception("No database connection has been established. Please check the database connection settings for the Model in the Model Configuration Form.")
                    End If
                ElseIf Me.DatabaseConnection.Connected = False Then
                    Throw New Exception("The database is not connected.")
                End If

                If Me.IsDatabaseSynchronised And Me.RDS.DatabaseDataType.Count = 0 Then
                    Dim lsPath = Richmond.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                    Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                    Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                    Me.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList
                End If

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, ex.StackTrace, False,, True)

                Return False
            End Try
        End Function

        Public Sub createCMMLAttribute(ByVal asEntityName As String,
                                       ByVal asAttributeName As String,
                                       ByRef arRole As FBM.Role,
                                       ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrFact As New FBM.Fact
            Dim lrTable As RDS.Table
            Dim lrModelElement As FBM.ModelObject

            Try
                '---------------------------------------------------------------------
                'Check to see that the Entity (already) exists in the ERD MetaModel.
                '  If the Entity doesn't exist, create it.
                '---------------------------------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & asEntityName & "'"

                Dim lrORMRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrORMRecordset.EOF Then
                    '---------------------------------------------------------------
                    'The Entity does not exist in the ERD MetaModel, so create it.
                    '---------------------------------------------------------------
                    Richmond.WriteToStatusBar("Creating Entity, '" & asEntityName & "'")

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " (Element, ElementType)"
                    lsSQLQuery &= " VALUES ('" & asEntityName & "', 'Entity')"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '---------------------------------------------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(asEntityName)

                    lrTable = New RDS.Table(Me.RDS, asEntityName, lrModelElement)
                    Me.RDS.Table.AddUnique(lrTable)
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
                If arColumn Is Nothing Then
                    lsPropertyInstanceId = System.Guid.NewGuid.ToString
                Else
                    lsPropertyInstanceId = arColumn.Id
                End If

                '---------------------------------------------
                'Create the Attribute against the Entity
                '---------------------------------------------
                lsSQLQuery = "INSERT INTO CoreERDAttribute (ModelObject, Attribute)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & asEntityName & "'"
                lsSQLQuery &= " ,'" & lsPropertyInstanceId & "'"
                lsSQLQuery &= " )"

                lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " WHERE Property = '" & lsPropertyInstanceId & "'"

                Dim lrRecordsetCount = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


                '====================================================================================================
                Dim lbForceStorePropertyInformation As Boolean = False
                If arColumn.Role.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    If arColumn.Role.JoinsEntityType.IsObjectifyingEntityType Then
                        lbForceStorePropertyInformation = True
                    End If
                End If
                If asEntityName = arColumn.Table.Name And asEntityName = arColumn.Role.JoinedORMObject.GetTopmostNonAbsorbedSupertype(True).Id Then
                    'Column may be for Supertype of absorbed Subtype.
                    lbForceStorePropertyInformation = True
                End If
                If lbForceStorePropertyInformation Or (asEntityName = arColumn.Role.JoinedORMObject.Id Or asEntityName = arColumn.Role.FactType.Id) And lrRecordsetCount("Count").Data = 0 Then
                    'Columns can be reused on Subtype Entities, and don't need their definition twice,
                    '  just their relationship with the ERD Entity (above).

                    lsSQLQuery = "INSERT INTO CorePropertyHasPropertyName (Property, PropertyName)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & asAttributeName & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


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

                    lsSQLQuery = "INSERT INTO CorePropertyHasActiveRole (Property, Role)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & lsPropertyInstanceId & "'"
                    lsSQLQuery &= " ,'" & arColumn.ActiveRole.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '--------------------------------------------------
                    'Set the Ordinal Position of the Attribute
                    '--------------------------------------------------
                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " WHERE ModelObject = '" & asEntityName & "'"

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
                    If arColumn.Nullable = False Then

                        Richmond.WriteToStatusBar("Creating MandatoryConstraint for Attribute, '" & asAttributeName & "', for Entity, '" & asEntityName & "'", True)

                        lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= " '" & lsPropertyInstanceId & "'" 'lsAttributeName & "'"
                        lsSQLQuery &= " )"

                        lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
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

        Public Sub createCMMLAttributeIsMandatory(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & arColumn.Id & "'"
            lsSQLQuery &= " )"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLEntityByRDSTable(ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String

            For Each lrColumn In arTable.Column

                lsSQLQuery = "REMOVE INSTANCE '" & lrColumn.Id & "' FROM CoreElement"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Next

            lsSQLQuery = "REMOVE INSTANCE '" & arTable.Name & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE Element = '" & arTable.Name & "'"
            lsSQLQuery &= " AND ElementType = 'Entity'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub


        Public Sub createCMMLIndex(ByVal asIndexName As String,
                                   ByVal asEntityName As String,
                                   ByVal asQualifier As String,
                                   ByVal aiIndexDirection As pcenumCMMLIndexDirection,
                                   ByVal abIsPrimaryKey As Boolean,
                                   ByVal abRestrainsToUniqueValues As Boolean,
                                   ByVal abIndexIgnoresNulls As Boolean,
                                   ByVal aarColumn As List(Of RDS.Column))

            Try
                Dim lsSQLQuery As String = ""

                '-------------------------------------------------
                'Must create a Primary Identifier for the Entity
                '-------------------------------------------------
                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                lsSQLQuery &= " (Entity, Index)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asEntityName & "'"
                lsSQLQuery &= ",'" & asIndexName & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                For Each lrColumn In aarColumn
                    '-------------------------------------
                    'Add the Attribute to the PrimaryKey
                    '-------------------------------------
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                    lsSQLQuery &= " (Index, Property)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ",'" & lrColumn.Id & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                Next

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexRestrainsToUniqueValues.ToString
                lsSQLQuery &= " (Index)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If abIsPrimaryKey Then
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " (Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If abIndexIgnoresNulls Then
                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIgnoresNulls.ToString
                    lsSQLQuery &= " (Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & asIndexName & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexHasDirection.ToString
                lsSQLQuery &= " (Index, IndexDirection)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ",'" & aiIndexDirection.ToString & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO "
                lsSQLQuery &= pcenumCMMLRelations.CoreIndexHasQualifier.ToString
                lsSQLQuery &= " (Index, Qualifier)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & asIndexName & "'"
                lsSQLQuery &= ",'" & asQualifier & "'"
                lsSQLQuery &= ")"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub createCMMLRelation(ByRef arRelation As RDS.Relation)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO CoreRelationIsForEntity (Entity, Relation)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.OriginTable.Name & "'"
                lsSQLQuery &= " ,'" & arRelation.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreRelationHasDestinationEntity (Entity, Relation)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.DestinationTable.Name & "'"
                lsSQLQuery &= " ,'" & arRelation.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreOriginMultiplicity (ERDRelation, Multiplicity)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " ,'" & arRelation.OriginMultiplicity.ToString & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO CoreDestinationMultiplicity (ERDRelation, Multiplicity)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " ,'" & arRelation.DestinationMultiplicity.ToString & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString & " (Relation, FactType)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arRelation.Id & "'"
                lsSQLQuery &= " , '" & arRelation.ResponsibleFactType.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If arRelation.RelationOriginIsMandatory Then
                    lsSQLQuery = "INSERT INTO CoreOriginIsMandatory (OriginIsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                If arRelation.RelationDestinationIsMandatory Then
                    lsSQLQuery = "INSERT INTO CoreDestinationIsMandatory (DestinationIsMandatory)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= " '" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                lsSQLQuery &= " (Relation, Predicate)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arRelation.Id & "'"
                lsSQLQuery &= ",'" & arRelation.OriginPredicate & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                lsSQLQuery &= " (Relation, Predicate)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arRelation.Id & "'"
                lsSQLQuery &= ",'" & arRelation.DestinationPredicate & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim liInd As Integer = 1
                Dim lrFact As FBM.Fact

                For Each lrOriginColumn In arRelation.OriginColumns

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                    lsSQLQuery &= " (Attribute, Relation)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrOriginColumn.Id & "'"
                    lsSQLQuery &= ",'" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                    lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrFact.Id & "'"
                    lsSQLQuery &= ",'" & liInd & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    liInd += 1
                Next

                liInd = 1
                For Each lrDestinationColumn In arRelation.DestinationColumns

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                    lsSQLQuery &= " (Attribute, Relation)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrDestinationColumn.Id & "'"
                    lsSQLQuery &= ",'" & arRelation.Id & "'"
                    lsSQLQuery &= " )"

                    lrFact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition.ToString
                    lsSQLQuery &= " (RelationAttribute, OrdinalPosition)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & lrFact.Id & "'"
                    lsSQLQuery &= ",'" & liInd & "'"
                    lsSQLQuery &= " )"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setCMMLProcessToProcessRelationIsExtends(ByRef arProcessToProcessRelation As CMML.ProcessProcessRelation, ByVal abIsExtends As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsExtends Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreP2PIsExtends.ToString
                    lsSQLQuery &= " (CoreP2PIsExtends)"
                    lsSQLQuery &= " VALUES ('" & arProcessToProcessRelation.Fact.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreP2PIsExtends.ToString
                    lsSQLQuery &= " WHERE CoreP2PIsExtends = '" & arProcessToProcessRelation.Fact.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub setCMMLProcessToProcessRelationIsIncludes(ByRef arProcessToProcessRelation As CMML.ProcessProcessRelation, ByVal abIsIncludes As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsIncludes Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreP2PIsIncludes.ToString
                    lsSQLQuery &= " (CoreP2PIsIncludes)"
                    lsSQLQuery &= " VALUES ('" & arProcessToProcessRelation.Fact.Id & "')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreP2PIsIncludes.ToString
                    lsSQLQuery &= " WHERE CoreP2PIsIncludes = '" & arProcessToProcessRelation.Fact.Id & "'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub setCoreElementIsActor(ByRef arModelElement As FBM.ModelObject, ByVal abIsActor As Boolean)

            Dim lsSQLQuery As String

            Try

                If abIsActor Then
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " (Element, ElementType)"
                    lsSQLQuery &= " VALUES ('" & arModelElement.Id & "', 'Actor')"
                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " WHERE Element = '" & arModelElement.Id & "'"
                    lsSQLQuery &= " AND ElementType = 'Actor'"
                End If

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub createCMMLActor(ByRef arActor As CMML.Actor)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & arActor.Name & "', 'Actor')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)


            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementName.ToString
            lsSQLQuery &= " (Element, ElementName)"
            lsSQLQuery &= " VALUES ('" & arActor.Name & "','" & arActor.Name & "')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Function createCMMLActorProcessRelation(ByRef arCMMLActorProcessRelation As CMML.ActorProcessRelation) As FBM.Fact

            Try
                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Dim lsSQLString As String = ""
                lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                lsSQLString &= " (Actor, Process, Data)"
                lsSQLString &= " VALUES ("
                lsSQLString &= "'" & arCMMLActorProcessRelation.Actor.Name & "'"
                lsSQLString &= ",'" & arCMMLActorProcessRelation.Process.Id & "'"
                lsSQLString &= ",''"
                lsSQLString &= ")"

                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Return Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Function createCMMLProcessProcessRelation(ByRef arCMMLProcessProcessRelation As CMML.ProcessProcessRelation) As FBM.Fact

            Try
                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Dim lsSQLString As String = ""
                lsSQLString = "INSERT INTO " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                lsSQLString &= " (Process1, Process2, Data)"
                lsSQLString &= " VALUES ("
                lsSQLString &= "'" & arCMMLProcessProcessRelation.Process1.Id & "'"
                lsSQLString &= ",'" & arCMMLProcessProcessRelation.Process2.Id & "'"
                lsSQLString &= ",''"
                lsSQLString &= ")"

                '----------------------------------
                'Create the Fact within the Model
                '----------------------------------
                Return Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Sub createCMMLTable(ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & arTable.Name & "', 'Entity')"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If arTable.isPGSRelation Then
                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " (IsPGSRelation)"
                lsSQLQuery &= " VALUES ('" & arTable.Name & "')"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub createColumnForUnaryFactType(ByRef arFactType As FBM.FactType)

            Try
                'RDS
                If arFactType.Arity = 1 Then
                    'Unary FactType, so add boolean Column to the corresponding RDS Table.

                    Dim lrFactType = arFactType

                    Dim lrTable As RDS.Table = Me.RDS.Table.Find(Function(x) x.Name = lrFactType.RoleGroup(0).JoinedORMObject.Id)

                    If lrTable Is Nothing Then
                        lrTable = Me.RDS.Table.Find(Function(x) x.Name = lrFactType.RoleGroup(0).JoinedORMObject.GetTopmostNonAbsorbedSupertype.Id)
                    End If

                    Dim lsColumnName = "DummyFactTypeReadingRequired"

                    If lrFactType.FactTypeReading.Count > 0 Then
                        lsColumnName = Viev.Strings.MakeCapCamelCase(lrFactType.FactTypeReading(0).PredicatePart(0).PredicatePartText, True)
                    End If

                    lsColumnName = lrTable.createUniqueColumnName(lsColumnName, Nothing, 0)

                    Dim lrColumn As New RDS.Column(lrTable, lsColumnName, arFactType.RoleGroup(0), arFactType.RoleGroup(0), False)
                    lrColumn.FactType = arFactType

                    Call lrTable.addColumn(lrColumn)

                Else
                    Throw New Exception("FactType is not Unary")
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub



        Public Function CreateUniqueEntityName(ByVal asEntityName As String, Optional ByVal aiStartingInd As Integer = 0) As String

            Dim lsUniqueEntityName As String = ""
            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniqueEntityName = asEntityName
            Else
                lsUniqueEntityName = asEntityName & aiStartingInd.ToString
            End If


            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE " & pcenumCMML.Element.ToString & " = '" & lsUniqueEntityName & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then

                lsUniqueEntityName = Me.CreateUniqueEntityName(asEntityName, aiStartingInd + 1)

            End If

            Return lsUniqueEntityName

        End Function

        Public Function CreateUniquePropertyName(ByRef arAttribute As ERD.Attribute, ByRef aiStartingInd As Integer) As String

            Dim lsUniquePropertyName As String = ""

            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniquePropertyName = arAttribute.AttributeName
            Else
                lsUniquePropertyName = arAttribute.AttributeName & aiStartingInd.ToString
            End If

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            lsSQLQuery &= " WHERE ModelObject = '" & arAttribute.Entity.Name & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If Not lrRecordset.EOF Then
                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"
                lsSQLQuery &= "   AND PropertyName = '" & lsUniquePropertyName & "'"

                lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset1("Count").Data) > 0 Then
                    lsUniquePropertyName = Me.CreateUniquePropertyName(arAttribute, aiStartingInd + 1)
                End If
            End If

            Return lsUniquePropertyName

        End Function

        ''' <summary>
        ''' Sets the ResponsibleFactType of existing Relations for the FactType to the relations respective LinkFactType.
        ''' This method called when a suitable FactType is objectified.
        ''' NB See also the converse moveRelationsOfLinkFactTypesToRespectiveFactType
        ''' </summary>
        ''' <param name="arFactType"></param>
        ''' <remarks></remarks>
        Public Sub moveRelationsOfFactTypeToRespectiveLinkFactTypes(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset
            Dim lasRelationId As New List(Of String)

            Try

                lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                lsSQLQuery &= " WHERE FactType = '" & arFactType.Id & "'"

                lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrRecordset.EOF

                    lasRelationId.Add(lrRecordset("Relation").Data)

                    lrRecordset.MoveNext()
                End While

                Dim larLinkFactType = From FactType In Me.FactType
                                      Where FactType.IsLinkFactType = True _
                                      And arFactType.RoleGroup.Contains(FactType.LinkFactTypeRole)
                                      Select FactType

                For Each lrLinkFactType In larLinkFactType

                    'For Each lsRelationId In lasRelationId.ToArray ''20211005-VM-Removed because step through them below.
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
                    lsSQLQuery &= " WHERE Entity = '" & lrLinkFactType.LinkFactTypeRole.JoinedORMObject.GetTopmostNonAbsorbedSupertype.Id & "'"
                    'lsSQLQuery &= " AND Relation = '" & lsRelationId & "'" '20211005-VM-Removed because couldn't possibly know this.

                    lrRecordset1 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset1.EOF
                        If lasRelationId.Contains(lrRecordset1("Relation").Data) Then
                            lasRelationId.Remove(lrRecordset1("Relation").Data)
                            Exit While
                        End If
                        lrRecordset1.MoveNext()
                    End While

                    If Not lrRecordset1.EOF Then
                        lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " SET FactType = '" & lrLinkFactType.Id & "'"
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset1("Relation").Data & "'"

                        '============================================================================
                        'RDS
                        Dim lrRDSRelation As RDS.Relation = Me.RDS.Relation.Find(Function(x) x.Id = lrRecordset1("Relation").Data) 'lsRelationId)
                        Try
                            Call lrRDSRelation.changeResponsibleFactType(lrLinkFactType)
                        Catch ex As Exception
                            '20200726-VM-Unusual occurence, consider throwing an Exception.
                            'Debugger.Break()
                        End Try

                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

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

        Public Sub moveRelationsOfLinkFactTypesToRespectiveFactType(ByVal arFactType As FBM.FactType)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try

                Dim larFactType = From FactType In Me.FactType
                                  Where FactType.IsLinkFactType = True _
                                  And arFactType.RoleGroup.Contains(FactType.LinkFactTypeRole)
                                  Select FactType

                For Each lrFactType In larFactType

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                    lsSQLQuery &= " WHERE FactType = '" & lrFactType.Id & "'"

                    lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    While Not lrRecordset.EOF
                        lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForFactType.ToString
                        lsSQLQuery &= " SET FactType = '" & arFactType.Id & "'"
                        lsSQLQuery &= " WHERE Relation = '" & lrRecordset("Relation").Data & "'"

                        '============================================================================
                        'RDS
                        Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        Dim lrRDSRelation As RDS.Relation = Me.RDS.Relation.Find(Function(x) x.Id = lrRecordset("Relation").Data)
                        Try
                            Call lrRDSRelation.changeResponsibleFactType(arFactType)
                        Catch ex As Exception
                            '20200726-VM-Unusual occurence, consider throwing an Exception.
                            'Debugger.Break()
                        End Try

                        lrRecordset.MoveNext()
                    End While

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
        ''' Called when a FactType is removed from a Model and that Model contains a Language within the Core that references that FactType.
        ''' </summary>
        ''' <param name="arFactType"></param>
        ''' <remarks></remarks>
        Public Sub RemoveFactTypeReferencesFromCore(ByRef arFactType As FBM.FactType)

            Dim lsSQLQuery As String
            'Dim lrColumn As RDS.Column
            'Dim lrTable As RDS.Table

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString
            'lsSQLQuery &= " WHERE ModelObject = '" & lrAttribute.Entity.Id & "'"
            'lsSQLQuery &= " AND Attribute = '" & lrAttribute.Name & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
            'lsSQLQuery &= " WHERE Property = '" & lrAttribute.Name & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLAttribute(ByVal asEntityName As String, ByVal asAttributeId As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "REMOVE INSTANCE '" & asAttributeId & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            '----------------------------------------------------------------------------------
            'Reset the OrdinalPosition of the Attributes above the Attribute that was deleted
            '  is done at the RDS Level.
            '----------------------------------------------------------------------------------
            '20180606-VM-ToDo: Maybe already done. Check this
        End Sub

        Public Sub removeCMMLAttributeFromTableOnly(ByRef arColumn As RDS.Column, arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreERDAttribute.ToString  '" & asAttributeId & "' FROM CoreElement"
                lsSQLQuery &= " WHERE ModelObject = '" & arTable.Name & "'"
                lsSQLQuery &= " AND Attribute = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub removeCMMLAttributeIsMandatory(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM CoreIsMandatory"
            lsSQLQuery &= " WHERE IsMandatory = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLIndex(ByRef arIndex As RDS.Index)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "REMOVE INSTANCE '" & arIndex.Name & "' FROM CoreIndex"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLIsDerivedFactTypeParameter(ByRef arColumn As RDS.Column)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsDerivedFactTypeParameter.ToString
                lsSQLQuery &= " WHERE IsDerivedFactTypeParameter = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub removeCMMLIsPGSRelation(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIsPGSRelation.ToString
                lsSQLQuery &= " WHERE IsPGSRelation = '" & arTable.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLPropertyFromIndexByRDSIndexColumn(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
            lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"
            lsSQLQuery &= " AND Property = '" & arColumn.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLActor(ByRef arCMMLActor As CMML.Actor)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLString &= " WHERE Element = '" & arCMMLActor.Name & "'"
                lsSQLString &= "   AND ElementType = 'Actor'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLProcess(ByRef arCMMLProcess As CMML.Process)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLString &= " WHERE Element = '" & arCMMLProcess.Id & "'"
                lsSQLString &= "   AND ElementType = 'Process'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLActorProcessRelation(ByRef arCMMLActorProcessRelation As CMML.ActorProcessRelation)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString
                lsSQLString &= " WHERE Actor = '" & arCMMLActorProcessRelation.Actor.Name & "'"
                lsSQLString &= "   AND Process = '" & arCMMLActorProcessRelation.Process.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLProcessProcessRelation(ByRef arCMMLProcesProcessRelation As CMML.ProcessProcessRelation)

            Try
                Dim lsSQLString As String = ""

                lsSQLString = "DELETE FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString
                lsSQLString &= " WHERE Process1 = '" & arCMMLProcesProcessRelation.Process1.Id & "'"
                lsSQLString &= "   AND Process2 = '" & arCMMLProcesProcessRelation.Process2.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLString)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLRelation(ByRef arRelation As RDS.Relation)

            Dim lsSQLQuery As String

            lsSQLQuery = "REMOVE INSTANCE '" & arRelation.Id & "' FROM CoreElement"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLRelationDestinationColumn(ByRef arRelation As RDS.Relation,
                                                  ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationDestination.ToString
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= " AND Relation = '" & arRelation.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                'lsSQLQuery &= " WHERE RelationAttribute = '" & lrFact.Id & "'"

                'Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLRelationOriginColumn(ByRef arRelation As RDS.Relation,
                                                  ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOrigin.ToString
                lsSQLQuery &= " WHERE Attribute = '" & arColumn.Id & "'"
                lsSQLQuery &= " AND Relation = '" & arRelation.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                'lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreAttributeIsPartOfRelationOriginHasOrdinalPosition.ToString
                'lsSQLQuery &= " WHERE RelationAttribute = '" & lrFact.Id & "'"

                'Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStartState"></param>
        Public Sub removeCMMLStartState(ByRef arStartState As FBM.STM.State)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStartState.ValueType.Id & "'"
                lsSQLQuery &= " AND CoreElement = '" & arStartState.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeCMMLState(ByRef arState As STM.State)

            Dim lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasState.ToString
            lsSQLQuery &= " WHERE ValueType = '" & arState.ValueType.Id & "'"
            lsSQLQuery &= " AND State = '" & arState.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreStateHasName.ToString
            lsSQLQuery &= " WHERE State = '" & arState.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "REMOVE INSTANCE '" & arState.Id & "' FROM CoreElement"
            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStateTransition">The StateTransition to be removed from the STM within the FBM.</param>
        Public Sub removeCMMLStateTransition(ByRef arStateTransition As FBM.STM.StateTransition)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStateTransition.ValueType.Id & "'"
                lsSQLQuery &= " AND Concept1 = '" & arStateTransition.FromState.Id & "'"
                lsSQLQuery &= " AND Concept2 = '" & arStateTransition.ToState.Id & "'"
                lsSQLQuery &= " AND Event = '" & arStateTransition.Event & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' For the State Transition Model (STM) of the FBM Model...for ValueType/ValueConstraint state transitions
        ''' </summary>
        ''' <param name="arStopState">The StopState being removed from the State Transition Model.</param>
        Public Sub removeCMMLStopState(ByRef arStopState As FBM.STM.State)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
                lsSQLQuery &= " WHERE ValueType = '" & arStopState.ValueType.Id & "'"
                lsSQLQuery &= " AND CoreElement = '" & arStopState.Name & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub setCMMLAttributeOrdinalPosition(ByVal asAttributeId As String, ByVal aiOrdinalPosition As Integer)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                lsSQLQuery &= " SET Position = '" & aiOrdinalPosition.ToString & "'"
                lsSQLQuery &= " WHERE Property = '" & asAttributeId & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                'lrFactInstance = lrRecordset.CurrentFact
                'lrFactInstance.GetFactDataInstanceByRoleName("Position").Data = aiOrdinalPosition.ToString

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub updateCMMLIndexIsPrimaryKey(ByRef arIndex As RDS.Index, ByVal abIsPrimaryKey As Boolean)

            Try
                Dim lsSQLQuery As String = ""

                If abIsPrimaryKey Then

                    lsSQLQuery = "INSERT INTO "
                    lsSQLQuery &= pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " (Index)"
                    lsSQLQuery &= " VALUES ("
                    lsSQLQuery &= "'" & arIndex.Name & "'"
                    lsSQLQuery &= ")"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Else
                    lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreIndexIsPrimaryKey.ToString
                    lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"

                    Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            End Try

        End Sub

        Public Sub updateCMMLIndexName(ByVal asOriginalName As String, ByVal asNewName As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "RENAME INSTANCE '" & asOriginalName & "'"
            lsSQLQuery &= " IN CoreIndex"
            lsSQLQuery &= " TO '" & asNewName & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLIndexQualifier(ByRef arIndex As RDS.Index, ByVal asNewQualifier As String)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreIndexHasQualifier.ToString
            lsSQLQuery &= " SET Qualifier = '" & asNewQualifier & "'"
            lsSQLQuery &= " WHERE Index = '" & arIndex.Name & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLRelationDestinationMultiplicity(ByRef arRelation As RDS.Relation, ByVal aiMultiplicity As pcenumCMMLMultiplicity)

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreDestinationMultiplicity.ToString
            lsSQLQuery &= " SET Multiplicity = '" & aiMultiplicity.ToString & "'"
            lsSQLQuery &= " WHERE ERDRelation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLRelationOriginMultiplicity(ByRef arRelation As RDS.Relation, ByVal aiMultiplicity As pcenumCMMLMultiplicity)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreOriginMultiplicity.ToString
            lsSQLQuery &= " SET Multiplicity = '" & aiMultiplicity.ToString & "'"
            lsSQLQuery &= " WHERE ERDRelation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateCMMLActorName(ByVal asOldName As String, ByVal asNewName As String)
            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " SET Element = '" & asNewName & "'"
                lsSQLQuery &= " WHERE Element '" & asOldName & "'"
                lsSQLQuery &= " AND ElementType = 'Actor'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub updateCMMLTableName(ByVal asOldName As String, ByVal asNewName As String)
            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "RENAME INSTANCE '" & asOldName & "' IN " & pcenumCMMLCoreModel.CoreElement.ToString & " TO '" & asNewName & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub updateORSetCMMLPropertyActiveRole(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset.EOF Then
                lsSQLQuery = "INSERT INTO CorePropertyHasActiveRole (Property, Role)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arColumn.Id & "'"
                lsSQLQuery &= " ,'" & arColumn.ActiveRole.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Else
                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                lsSQLQuery &= " SET Role = '" & arColumn.ActiveRole.Id & "'"
                lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub updateORSetCMMLPropertyRole(ByRef arColumn As RDS.Column)

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
            lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset.EOF Then
                lsSQLQuery = "INSERT INTO CorePropertyIsForRole (Property, Role)"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & arColumn.Id & "'"
                lsSQLQuery &= " ,'" & arColumn.Role.Id & "'"
                lsSQLQuery &= " )"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Else
                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                lsSQLQuery &= " SET Role = '" & arColumn.Role.Id & "'"
                lsSQLQuery &= " WHERE Property = '" & arColumn.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

        End Sub

        Public Sub updateRelationDestinationPredicate(ByRef arRelation As RDS.Relation, ByVal asDestinationPredicate As String)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                lsSQLQuery &= " SET Predicate = '" & asDestinationPredicate & "'"
                lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

                Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub updateRelationOriginPredicate(ByRef arRelation As RDS.Relation, ByVal asDestinationPredicate As String)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreOriginPredicate.ToString
            lsSQLQuery &= " SET Predicate = '" & asDestinationPredicate & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateRelationDestinationTable(ByRef arRelation As RDS.Relation, ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationHasDestinationEntity.ToString
            lsSQLQuery &= " SET Entity = '" & arTable.Name & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub updateRelationOriginTable(ByRef arRelation As RDS.Relation, ByRef arTable As RDS.Table)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreRelationIsForEntity.ToString
            lsSQLQuery &= " SET Entity = '" & arTable.Name & "'"
            lsSQLQuery &= " WHERE Relation= '" & arRelation.Id & "'"

            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub PopulateCMMLStructureFromCoreMDAElements(Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing)

            Try
                Me.RDSLoading = True
                Dim lsMessage As String

                Dim lsSQLQuery As String = ""
                Dim lrActor As CMML.Actor
                Dim lrProcess, lrProcess1, lrProcess2 As CMML.Process
                Dim lrActorProcessRelation As CMML.ActorProcessRelation
                Dim lrProcessProcessRelation As CMML.ProcessProcessRelation

#Region "Actors"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Actor'"

                Dim lrORMRecordset, lrORMRecordset2 As ORMQL.Recordset

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrModelElement As FBM.ModelObject
                Dim lsColumnName As String = ""
                Dim liInd As Integer = 0 'For reporting progress on Actors loaded.

                While Not lrORMRecordset.EOF
                    '-----------------------------------------------------
                    'Get the underlying ModelElement
                    lrModelElement = Me.GetModelObjectByName(lrORMRecordset("Element").Data)

                    If lrModelElement Is Nothing Then
                        'This is dire. Create a dummy FBMEntityType for the Actor, and add the EntityType to the Model. 
                        '  The user can then elect to delete the EntityType/Actor if it shouldn't be in the model
                        Dim lrEntityType = Me.CreateEntityType(lrORMRecordset("Element").Data, True)
                        lrModelElement = lrEntityType

                        'Let the user know what happened.
                        lsMessage = "The Actor, '" & lrModelElement.Name & "', within the relational model had no corresponding Object-Role Model model element."
                        lsMessage &= vbCrLf & vbCrLf & "An Entity Type has been created in the model to cater for this. If you no longer need this model element remove it from the model."
                        Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical)
                    End If

                    lrActor = New CMML.Actor(Me.UML, lrORMRecordset("Element").Data, lrModelElement)
                    Me.UML.Actor.Add(lrActor)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actors
#End Region

#Region "Processes"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'Process'"

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lsProcessText As String = ""

                While Not lrORMRecordset.EOF

#Region "Process Text"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                    lsSQLQuery &= " WHERE Process = '" & lrORMRecordset("Element").Data & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsProcessText = "Error-No Process Text"
                    If Not lrORMRecordset2.EOF Then
                        lsProcessText = lrORMRecordset2("ProcessText").Data
                    End If
#End Region

                    lrProcess = New CMML.Process(Me.UML, lrORMRecordset("Element").Data, lsProcessText)
                    Me.UML.Process.Add(lrProcess)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actors
#End Region

#Region "Actor to Process Relations"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreActorToProcessParticipationRelation.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrActor = Me.UML.Actor.Find(Function(x) x.Name = lrORMRecordset("Actor").Data)
                    lrProcess = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process").Data)

                    lrActorProcessRelation = New CMML.ActorProcessRelation(Me.UML, lrActor, lrProcess)
                    lrActorProcessRelation.Fact = lrORMRecordset.CurrentFact

                    Me.UML.ActorProcessRelation.Add(lrActorProcessRelation)

                    lrActor.Process.Add(lrProcess)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actors
#End Region

#Region "Process to Process Relations"
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreProcessToProcessParticipationRelation.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrProcess1 = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process1").Data)
                    lrProcess2 = Me.UML.Process.Find(Function(x) x.Id = lrORMRecordset("Process2").Data)

                    lrProcessProcessRelation = New CMML.ProcessProcessRelation(Me.UML, lrProcess1, lrProcess2)
                    lrProcessProcessRelation.Fact = lrORMRecordset.CurrentFact

#Region "Is Extends"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreP2PIsExtends"
                    lsSQLQuery &= " WHERE CoreP2PIsExtends = '" & lrProcessProcessRelation.Fact.Id & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcessProcessRelation.IsExtends = True
                    End If
#End Region

#Region "Is Includes"
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreP2PIsIncludes"
                    lsSQLQuery &= " WHERE CoreP2PIsIncludes = '" & lrProcessProcessRelation.Fact.Id & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If Not lrORMRecordset2.EOF Then
                        lrProcessProcessRelation.IsIncludes = True
                    End If
#End Region

                    Me.UML.ProcessProcessRelation.Add(lrProcessProcessRelation)

                    'lrProcess1.Process.Add(lrProcess2)

                    lrORMRecordset.MoveNext()

                End While 'Stepping through Actors
#End Region

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Me.RDSLoading = False
            End Try

        End Sub


    End Class

End Namespace
