Imports System.Reflection

Public Class ODBCDatabaseReverseEngineer

    Dim Model As FBM.Model 'The model to which the database schema is to be mapped.

    Private ODBCConnection As System.Data.Odbc.OdbcConnection

    ''' <summary>
    ''' The RDS structure, from the database, is first mapped to this model.
    '''   NB Have to do this because creating the ModelElements in Me.Model will actually create the RDS structure for that Model.
    '''   The database structure is first loaded into TempModel, then the ModelElements created in Me.Model from TempModel.
    ''' </summary>
    Dim TempModel As New FBM.Model

    ''' <summary>
    ''' True if the user elects to create a Page in the Model for each Table.
    ''' </summary>
    Dim CreatePagePerTable As Boolean = False

    '=========================================================================================================================================================
    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="arModel"></param>
    Public Sub New(ByRef arModel As FBM.Model,
                   ByVal asDatabaseConnectionString As String,
                   ByVal abCreatePagePerTable As Boolean)
        Me.Model = arModel
        Me.ODBCConnection = New System.Data.Odbc.OdbcConnection(asDatabaseConnectionString)
        Me.CreatePagePerTable = abCreatePagePerTable
    End Sub

    ''' <summary>
    ''' Reverse engineers the Model from the database of that Model.
    ''' PRECONDITIONS: The Model has a linked database.
    ''' </summary>
    Public Function ReverseEngineerDatabase(ByRef asErrorMessage As String) As Boolean

        '=====================================================================================================================
        'PSEUDOCODE
        '
        ' * Connect to the ODBC database
        ' * Get the DataTypes for the database
        ' * Get the Tables...and put them into TempModel
        ' * Get the Columns for the TempModel Tables
        ' * Get the Indexes for the TempModel Tables
        ' * Get the Relations for the TempModel Tables
        ' * Order the TempModel Tables by their relations. Those that have no relations first.
        ' * Create EntityTypes for those Tables that have a Single Column PrimaryKey.
        ' * For each Table in TempModel Tables (by their sorted order)
        '     * Create the ValueTypes for Columns that that are ValueTypes (even if they are referenced by a Relation)
        '         Value Types are, in this instance,:
        '           1. Not the ValueTypes created for ReferenceShemes of EntityTypes created for Tables with single Column PrimaryKeys.
        '              a) Including those Columns/ValueTypes that are referenced by a Column that references an EntityType for a Table with a single column PrimaryKey.
        '           2. Are Columns/ValueTypes where the Column has no Relation/reference to another table.
        '     * If the Table's ModelElement has not been created, create it.
        '         Can be an EntityType or an ObjectifiedFactType.
        '     * If the Table is an EntityType that has a Compound Reference Scheme, create the ReferenceScheme for EntityTypee/Table.
        '         NB The referenced ModelElements should have already been created.
        '     * If the Table is an ObjectifiedFactType then create the ObjectifiedFactTyhpe.
        '         NB The referenced ModelElements should have already been created.
        '   Loop
        ' * For each Table in the TempModel Tables (by their sorted order)
        '    * Create the ExternalUniquenessConstraints for Indexes of the Table
        '   Loop
        '=====================================================================================================================
        Try
            Me.TempModel.TargetDatabaseType = Me.Model.TargetDatabaseType
            Me.TempModel.TargetDatabaseConnectionString = Me.Model.TargetDatabaseConnectionString

            Me.Model.connectToDatabase()
            Me.TempModel.connectToDatabase()

            'Load DatabaseTypes into memory.
            Call Me.Model.DatabaseConnection.getDatabaseTypes()

            'Call Me.GetDataTypes()

            Call Me.getTables()

            Call Me.GetColumns()

            Call Me.GetIndexes()

            Call Me.getRelations()

            Call Me.TempModel.RDS.orderTablesByRelations()

            '------------------------------------------------------------------------------
            'Create EntityTypes for each Table with a PrimaryKey with one Column.
            Call Me.createTablesForSingleColumnPKTables()

            For Each lrTable In Me.TempModel.RDS.Table

                'Create ValueTypes (that haven't already been created by virtue of being the ReferenceModeValueType of Simple Reference Scheme EntityTypes.
                Call Me.createValueTypesByTable(lrTable)
#Region "Create ObjectifiedFactTypes"
                If Me.Model.GetModelObjectByName(lrTable.Name) Is Nothing Then
                    'The Table has no ModelElement, so create it.
                    If lrTable.getPrimaryKeyColumns.Count = 1 Then
                        'Is an EntityType, and should already be a ModelElement in the Model.
                        Debugger.Break()
                    Else
                        'Create an ObjectifiedFactType
                        Dim lrFactType As FBM.FactType

                        Dim lrModelElement As FBM.ModelObject = Nothing
                        Dim larModelObject As New List(Of FBM.ModelObject)

                        For Each lrColumn In lrTable.getPrimaryKeyColumns
                            If lrColumn.hasOutboundRelation Then
                                Dim lrRelation = lrColumn.Relation.Find(Function(x) x.OriginTable IsNot Nothing)
                                Dim lrDestinationTable As RDS.Table = Me.Model.RDS.getTableByName(lrRelation.DestinationTable.Name)
                                lrModelElement = lrDestinationTable.FBMModelElement
                                If lrModelElement Is Nothing Then
                                    lrModelElement = Me.Model.GetModelObjectByName(lrColumn.Name)
                                End If
                            Else
                                lrModelElement = Me.Model.GetModelObjectByName(lrColumn.Name)
                            End If

                            If lrModelElement Is Nothing Then
                                Debugger.Break()
                            End If
                            If lrModelElement.isReferenceModeValueType Then
                                lrModelElement = Me.Model.getEntityTypeByReferenceModeValueType(lrModelElement)
                            End If
                            larModelObject.Add(lrModelElement)
                        Next

                        lrFactType = Me.Model.CreateFactType(lrTable.Name, larModelObject, False, True, , , False, )
                        Me.Model.AddFactType(lrFactType)
                        lrFactType.Objectify() 'Add to model first, so LinkFactTypes have something to join to.

                        'Create the internalUniquenessConstraint.
                        Dim larRole As New List(Of FBM.Role)
                        For Each lrRole In lrFactType.RoleGroup
                            larRole.Add(lrRole)
                        Next
                        Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                    End If
                End If
#End Region
            Next

            '-----------------------------------------------------------------------------
            'Create FactTypes for all other Relations.
            For Each lrRelation In Me.TempModel.RDS.Relation.FindAll(Function(x) Not x.isPrimaryKeyBasedRelation)
                'Relations to other Tables.
                Dim larModelElement As New List(Of FBM.ModelObject)
                Dim lrModelElement1 As FBM.ModelObject = Nothing
                Dim lrModelElement2 As FBM.ModelObject = Nothing

                lrModelElement1 = Me.Model.RDS.getTableByName(lrRelation.OriginTable.Name).FBMModelElement
                lrModelElement2 = Me.Model.RDS.getTableByName(lrRelation.DestinationTable.Name).FBMModelElement

                larModelElement.Add(lrModelElement1)
                larModelElement.Add(lrModelElement2)

                Dim lsFactTypeName As String = lrModelElement1.Id & lrModelElement2.Id
                lsFactTypeName = Me.Model.CreateUniqueFactTypeName(lsFactTypeName, 0)

                Dim lrFactType As FBM.FactType = Me.Model.CreateFactType(lsFactTypeName, larModelElement, False, True, , , False, )
                Me.Model.AddFactType(lrFactType)


                'Create the internalUniquenessConstraint.
                Dim larRole As New List(Of FBM.Role)
                larRole.Add(lrFactType.RoleGroup(0))

                Call lrFactType.CreateInternalUniquenessConstraint(larRole)

            Next

            '-----------------------------------------------------------------------------
            'Create FactTypes straight to ValueTypes.
            Dim lasNonReferenceModeValueTypeNames = From ValueType In Me.Model.ValueType
                                                    Where Not ValueType.isReferenceModeValueType
                                                    Select ValueType.Id

            For Each lrTable In Me.TempModel.RDS.Table

                Dim larValueTypeColumns = From Column In lrTable.Column
                                          Where lasNonReferenceModeValueTypeNames.Contains(Column.Name)
                                          Select Column

                For Each lrColumn In larValueTypeColumns
                    Dim larModelElement As New List(Of FBM.ModelObject)
                    Dim lrModelElement1 As FBM.ModelObject = Nothing
                    Dim lrModelElement2 As FBM.ModelObject = Nothing

                    lrModelElement1 = Me.Model.RDS.getTableByName(lrTable.Name).FBMModelElement
                    lrModelElement2 = Me.Model.GetModelObjectByName(lrColumn.Name)

                    larModelElement.Add(lrModelElement1)
                    larModelElement.Add(lrModelElement2)

                    Dim lsFactTypeName As String = lrModelElement1.Id & "Has" & lrModelElement2.Id
                    lsFactTypeName = Me.Model.CreateUniqueFactTypeName(lsFactTypeName, 0)

                    Dim lrFactType As FBM.FactType = Me.Model.CreateFactType(lsFactTypeName, larModelElement, False, True, , , False, )
                    Me.Model.AddFactType(lrFactType)

                    'Create the internalUniquenessConstraint.
                    Dim larRole As New List(Of FBM.Role)
                    larRole.Add(lrFactType.RoleGroup(0))

                    Call lrFactType.CreateInternalUniquenessConstraint(larRole)
                Next
            Next

                Debugger.Break()



        Catch ex As Exception
            Debugger.Break()
        End Try

        Return True


    End Function


    ''' <summary>
    ''' Creates the ValueTypes for Columns that that are ValueTypes (even if they are referenced by a Relation)
    '''   Value Types are, in this instance,:
    '''     1. Not the ValueTypes created for ReferenceShemes of EntityTypes created for Tables with single Column PrimaryKeys.
    '''        a) Including those Columns/ValueTypes that are referenced by a Column that references an EntityType for a Table with a single column PrimaryKey.
    '''     2. Are Columns/ValueTypes where the Column has no Relation/reference to another table.
    ''' </summary>
    Private Sub createValueTypesByTable(ByRef arTable As RDS.Table)

        Try
            For Each lrColumn In arTable.Column

                If Not (lrColumn.isPartOfPrimaryKey Or lrColumn.hasOutboundRelation) Or
                    (lrColumn.isPartOfPrimaryKey And Not lrColumn.hasOutboundRelation) Then

                    Dim lrModelElement = Me.Model.GetModelObjectByName(lrColumn.Name)

                    Dim lsUniqueValueTypeName As String
                    If lrModelElement IsNot Nothing Then
                        lsUniqueValueTypeName = lrColumn.Name
                        If lrModelElement.GetType <> GetType(FBM.ValueType) Then
                            lsUniqueValueTypeName = Me.Model.CreateUniqueValueTypeName(lrColumn.Name, 0)
                        End If
                    Else
                        lsUniqueValueTypeName = lrColumn.Name
                    End If
                    'Create the ValueType
                    Dim lrValueType As FBM.ValueType

                    lrValueType = New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, lrColumn.Name, True)
                    Try
                        lrValueType.DataType = Me.Model.DatabaseConnection.getBostonDataTypeByDatabaseDataType(lrColumn.DataType.DataType)
                    Catch ex As Exception
                        lrValueType.DataType = pcenumORMDataType.TextVariableLength
                    End Try

                    'Add the ValueType to the Model
                    Me.Model.AddValueType(lrValueType)
                End If

            Next

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DisplayData(ByRef table As DataTable)

        For Each row As DataRow In table.Rows
            For Each col As DataColumn In table.Columns
                MsgBox(col.ColumnName & " = " & row(col))
            Next
        Next

    End Sub

    Private Sub getRelations()

        Try
            For Each lrTable In Me.TempModel.RDS.Table
                For Each lrRelation In Me.TempModel.DatabaseConnection.getForeignKeyRelationshipsByTable(lrTable)
                    Me.TempModel.RDS.Relation.Add(lrRelation)
                Next
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Private Sub getTables()

        Dim lrTable As RDS.Table

        Try

            For Each lrTable In Me.Model.DatabaseConnection.getTables()
                Me.TempModel.RDS.Table.Add(lrTable)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub createTablesForSingleColumnPKTables()

        Dim lrPage As FBM.Page

        For Each lrTable In Me.TempModel.RDS.Table
            If lrTable.hasSingleColumnPrimaryKey Then
                Dim lrEntityType As FBM.EntityType
                lrEntityType = New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, lrTable.Name, lrTable.Name)
                Me.Model.AddEntityType(lrEntityType, True)

                Dim lsValueTypeName As String
                Dim lsReferenceMode As String = ""
                Dim lrPrimaryKeyColumn As RDS.Column
                lrPrimaryKeyColumn = lrTable.Index.Find(Function(x) x.IsPrimaryKey = True).Column(0)
                lsValueTypeName = lrPrimaryKeyColumn.Name

                Dim items As Array
                items = System.Enum.GetValues(GetType(pcenumReferenceModeEndings))
                Dim item As pcenumReferenceModeEndings
                For Each item In items
                    If lsValueTypeName.EndsWith(GetEnumDescription(item).Trim({"."c})) Then 'See https://msdn.microsoft.com/en-us/library/kxbw3kwc(v=vs.110).aspx
                        lsReferenceMode = GetEnumDescription(item).Trim({"."c})
                        Exit For
                    Else
                        lsReferenceMode = lsValueTypeName
                    End If
                Next

                '-----------------------------------------------------------------------------
                'Create an EntityTypeInstance for the new EntityType and put it on the Page.
                lrPage = Me.Model.Page.Find(Function(x) x.Name = lrEntityType.Name)
                If lrPage IsNot Nothing Then
                    Call lrPage.DropEntityTypeAtPoint(lrEntityType, New PointF(50, 50))
                End If

                Dim liBostonDataType As pcenumORMDataType = Me.Model.DatabaseConnection.getBostonDataTypeByDatabaseDataType(lrPrimaryKeyColumn.DataType.DataType)
                lrEntityType.SetReferenceMode(lsReferenceMode, False, lsValueTypeName, False, liBostonDataType)

                ''=================================================================================================================
                ''Create joined FactTypes.
                'Dim lrFactType As FBM.FactType
                'Dim lrColumn As RDS.Column
                'Dim larModelObject As New List(Of FBM.ModelObject)

                'For Each lrColumn In lrTable.Column.FindAll(Function(x) x.Name <> lrPrimaryKeyColumn.Name)
                '    larModelObject.Clear()
                '    larModelObject.Add(lrEntityType)

                '    Dim lrJoinedModelObject As FBM.ModelObject

                '    lrJoinedModelObject = Me.Model.FindEntityTypeByValueTypeId(lrColumn.Name)
                '    If lrJoinedModelObject Is Nothing Then
                '        lrJoinedModelObject = Me.Model.CreateValueType(lrColumn.Name)
                '    End If

                '    larModelObject.Add(lrJoinedModelObject)

                '    Dim lrModelObject As FBM.ModelObject
                '    Dim lsFactTypeName As String = ""

                '    For Each lrModelObject In larModelObject
                '        lsFactTypeName &= lrModelObject.Name
                '    Next
                '    lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False)

                '    Dim larRole As New List(Of FBM.Role)
                '    Dim lrRole As FBM.Role

                '    For Each lrRole In lrFactType.RoleGroup
                '        larRole.Add(lrRole)
                '    Next
                '    Dim lasPredicatePart As New List(Of String)
                '    lasPredicatePart.Add("has")
                '    lasPredicatePart.Add("")

                '    Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)

                '    Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, False)

                '    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrEntityType.Id)
                '    larRole.Clear()
                '    larRole.Add(lrRole)

                '    Dim lrRoleConstraint As FBM.RoleConstraint

                '    lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                '                                                     larRole,
                '                                                     "InternalUniquenessConstraint",
                '                                                     1)

                '    lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                '    Dim lrFactTypeInstance As FBM.FactTypeInstance
                '    lrFactTypeInstance = lrPage.DropFactTypeAtPoint(lrFactType, New PointF(100, 100), False)
                'Next 'Column
                '=================================================================================================================
            End If
        Next


    End Sub

    Private Sub GetIndexes()

        Dim lrTable As RDS.Table

        Try
            Dim larIndex As New List(Of RDS.Index)

            For Each lrTable In Me.TempModel.RDS.Table
                larIndex = Me.TempModel.DatabaseConnection.getIndexesByTable(lrTable)
                For Each lrIndex In larIndex
                    lrTable.Index.Add(lrIndex)
                Next

                If larIndex.FindAll(Function(x) x.IsPrimaryKey).Count = 0 Then
                    'Need an alternate route for SQLite where a PK can be created that has no index.
                    larIndex = Me.TempModel.DatabaseConnection.getIndexesByTableByAlternateMeans(lrTable)
                    For Each lrIndex In larIndex
                        lrTable.Index.Add(lrIndex)
                    Next
                End If

            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GetDataTypes()

        Try
            Dim lrODBCTable As System.Data.DataTable

            If Not Me.ODBCConnection.State = ConnectionState.Open Then
                Me.ODBCConnection.Open()
            End If

            lrODBCTable = Me.ODBCConnection.GetSchema("DataTypes")

            For Each lrRow As DataRow In lrODBCTable.Rows

                Dim lrDataType As New RDS.DataType

                lrDataType.TypeName = lrRow(lrODBCTable.Columns("TypeName"))
                lrDataType.ProviderDBType = lrRow(lrODBCTable.Columns("ProviderDbType"))
                lrDataType.ColumnSize = lrRow(lrODBCTable.Columns("ColumnSize"))
                lrDataType.CreateFormat = NullVal(lrRow(lrODBCTable.Columns("CreateFormat")), "")
                lrDataType.CreateParameters = NullVal(lrRow(lrODBCTable.Columns("CreateParameters")), "")
                lrDataType.DataType = lrRow(lrODBCTable.Columns("Datatype"))
                lrDataType.IsAutoIncrementable = NullVal(lrRow(lrODBCTable.Columns("IsAutoIncrementable")), False)
                lrDataType.IsBestMatch = NullVal(lrRow(lrODBCTable.Columns("IsBestMatch")), False)
                lrDataType.IsCaseSensitive = NullVal(lrRow(lrODBCTable.Columns("IsCaseSensitive")), False)
                lrDataType.IsFixedLength = NullVal(lrRow(lrODBCTable.Columns("IsFixedLength")), False)
                lrDataType.IsFixedPrecisionScale = NullVal(lrRow(lrODBCTable.Columns("IsFixedPrecisionScale")), False)
                lrDataType.IsLong = NullVal(lrRow(lrODBCTable.Columns("IsLong")), False)
                lrDataType.IsNullable = NullVal(lrRow(lrODBCTable.Columns("IsNullable")), False)
                lrDataType.IsSearchable = NullVal(lrRow(lrODBCTable.Columns("IsSearchable")), False)
                lrDataType.IsSearchableWithLike = NullVal(lrRow(lrODBCTable.Columns("IsSearchableWithLike")), False)
                lrDataType.IsUnsigned = NullVal(lrRow(lrODBCTable.Columns("IsUnsigned")), False)
                lrDataType.MaximumScale = NullVal(lrRow(lrODBCTable.Columns("MaximumScale")), 0)
                lrDataType.MinimumScale = NullVal(lrRow(lrODBCTable.Columns("MinimumScale")), 0)
                lrDataType.IsConcurrencyType = NullVal(lrRow(lrODBCTable.Columns("IsConcurrencyType")), False)
                lrDataType.IsLiteralSupported = NullVal(lrRow(lrODBCTable.Columns("IsLiteralSupported")), False)
                lrDataType.LiteralPrefix = NullVal(lrRow(lrODBCTable.Columns("LiteralPrefix")), "")
                lrDataType.LiteralSuffix = NullVal(lrRow(lrODBCTable.Columns("LiteralSuffix")), "")
                lrDataType.SQLtype = NullVal(lrRow(lrODBCTable.Columns("SQLType")), 0)

                Me.Model.RDS.DataType.Add(lrDataType)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub GetColumns()

        Try

            For Each lrTable In Me.TempModel.RDS.Table
                For Each lrColumn In Me.Model.DatabaseConnection.getColumnsByTable(lrTable)
                    lrTable.Column.Add(lrColumn)
                Next
            Next
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


End Class
