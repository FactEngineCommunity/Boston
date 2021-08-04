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
        ' * Get the ODBC DataTypes for the database
        ' * Get the ODBC referenced tables...and put them into TempModel
        ' * 
        '=====================================================================================================================
        Try
            Me.TempModel.TargetDatabaseType = Me.Model.TargetDatabaseType
            Me.TempModel.TargetDatabaseConnectionString = Me.Model.TargetDatabaseConnectionString

            Me.Model.connectToDatabase()
            Me.TempModel.connectToDatabase()

            'Call Me.GetDataTypes()


            Call Me.getTables()

            Call Me.GetColumns()

            Call Me.GetIndexes()

            Call Me.getRelations()

            Call Me.TempModel.RDS.orderTablesByRelations()

            Call Me.createTablesForSingleColumnPKTables()


            Debugger.Break()

            '------------------------------------------------------------------------------
            'Create EntityTypes for each Table with a PrimaryKey with one Column.

        Catch ex As Exception
            Debugger.Break()
        End Try

        Return True


    End Function

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

                lrEntityType.SetReferenceMode(lsReferenceMode, False, lsValueTypeName)

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
