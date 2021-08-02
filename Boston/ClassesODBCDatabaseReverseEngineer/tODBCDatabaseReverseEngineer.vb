Imports System.Reflection
Imports System.Data.OleDb

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
            If Not Me.ODBCConnection.State = ConnectionState.Open Then
                Me.ODBCConnection.Open()
            End If

            Call Me.GetDataTypes()


            'Could be important. Don't know what 'Restrictions' are.
            'lrODBCTable = lrODBCConnection.GetSchema("Restrictions")

            Call Me.getTables()

            Try
                Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

                lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
                           .ConnectionString = Me.Model.TargetDatabaseConnectionString
                        }

                Dim lsDatabaseLocation = lrSQLConnectionStringBuilder("DSN")

                Dim lsConnectionString As String = ""
                lsConnectionString = "Provider=MSDATASHAPE;DRIVER={SQLite3 ODBC Driver};DSN=" & lsDatabaseLocation
                lsConnectionString = "Provider=MSDASQL;DRIVER={SQLite3 ODBC Driver};OPTION=3;FILEDSN=C:\Users\Victor\Desktop\Test.dsn"
                lsConnectionString = "Provider={SQLite3 ODBC Driver};Database=C:\Users\Victor\OneDrive\00-FactEngine\01-Marketing\02-Product\01-Products\00-FactEngine\Research\SemanticParsing\SpiderChallenge\databases\database\academic\academic.sqlite"

                'Have to use...'.Net Framework Data Provider for ODBC connection string
                Dim connection As New System.Data.OleDb.OleDbConnection
                Try
                    connection = New System.Data.OleDb.OleDbConnection(lsConnectionString)
                Catch ex As Exception
                    Debugger.Break()
                End Try

                Try
                    connection.Open()
                Catch ex As Exception
                    Debugger.Break()
                End Try


                Dim restrictions As String() = New String() {Nothing}
                Dim schema As DataTable
                schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, restrictions)

                For Each row As DataRow In schema.Rows
                    Debugger.Break()
                    'Dim dbForeignKey As ForeignKey = New ForeignKey()
                    'dbForeignKey.Name = row("FK_NAME").ToString()
                    'dbForeignKey.OriginalName = row("FK_NAME").ToString()
                    'dbForeignKey.FKTableName = row("FK_TABLE_NAME").ToString()
                    'Dim fkc As ForeignKeyColumn = New ForeignKeyColumn()
                    'fkc.Name = row("FK_COLUMN_NAME").ToString()
                    'dbForeignKey.FKColumns.Add(fkc)
                    'dbForeignKey.FKTableSchema = schema.ToString()
                    'dbForeignKey.PKTableName = row("PK_TABLE_NAME").ToString()
                    'Dim pkc As ForeignKeyColumn = New ForeignKeyColumn()
                    'pkc.Name = row("PK_COLUMN_NAME").ToString()
                    'dbForeignKey.PKColumns.Add(pkc)
                    'dbForeignKey.PKTableSchema = schema.ToString()
                    'foreignKeys.Add(dbForeignKey)
                Next

            Catch ex As Exception
                Debugger.Break()
            End Try



            'OLEDB Connection String
            'Provider=MSDASQL;Persist Security Info=False;DSN=SQLiteTest

            ''------------------------------------------------------------------------------
            ''Get the Columns for the tables.
            'Call Me.GetColumns()

            ''------------------------------------------------------------------------------
            ''Get the Indexes for the tables.
            'Call Me.GetIndexes()

            '------------------------------------------------------------------------------
            'Create EntityTypes for each Table with a PrimaryKey with one Column.

            Return True

            Catch ex As Exception
                Debugger.Break()

            Return False
        End Try

    End Function

    Private Sub DisplayData(ByRef table As DataTable)

        For Each row As DataRow In table.Rows
            For Each col As DataColumn In table.Columns
                MsgBox(col.ColumnName & " = " & row(col))
            Next
        Next

    End Sub

    Private Sub getTables()

        Dim lrODBCTable As System.Data.DataTable
        Dim lrTable As RDS.Table
        Dim lrPage As FBM.Page

        Try
            Try
                lrODBCTable = Me.ODBCConnection.GetSchema("Tables")
            Catch ex1 As Exception
                Debugger.Break()
            End Try

            'Dim blah = System.Data.Odbc.OdbcMetaDataCollectionNames.


            Select Case Me.ODBCConnection.DataSource
                    Case Is = "ACCESS"
                        For Each lrRow As DataRow In lrODBCTable.Rows
                            lrTable = New RDS.Table(Me.TempModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing)
                            Select Case lrRow(lrODBCTable.Columns("TABLE_TYPE"))
                                Case Is = "SYSTEM TABLE"
                                    lrTable.IsSystemTable = True
                                Case Else
                                    lrTable.IsSystemTable = False
                            End Select

                            If Not lrTable.IsSystemTable Then
                                Me.TempModel.RDS.Table.AddUnique(lrTable)

                            If Not Me.CreatePagePerTable Then
                                lrPage = New FBM.Page(Me.Model, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel)
                                frmMain.zfrmModelExplorer.AddPageToModel(Me.Model.TreeNode, lrPage, True)
                            End If
                        End If
                        Next
                    Case Else
                        For Each lrRow As DataRow In lrODBCTable.Rows
                            'Create the Table into the TempModel
                            Me.TempModel.RDS.Table.Add(New RDS.Table(Me.TempModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing))

                        If Not Me.CreatePagePerTable Then
                            lrPage = New FBM.Page(Me.Model, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel) '(lrODBCTable.Columns("TABLE_NAME"))
                            If frmMain.zfrmModelExplorer IsNot Nothing Then
                                frmMain.zfrmModelExplorer.AddPageToModel(Me.Model.TreeNode, lrPage, True)
                            End If
                        End If
                        'End If
                    Next
                End Select

            Catch ex As Exception
                Debugger.Break()
        End Try

    End Sub

    Private Sub createTablesForSingleColumnPKODBCTables()

        Dim lrPage As FBM.Page

        For Each lrTable In Me.Model.RDS.Table
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
                Call lrPage.DropEntityTypeAtPoint(lrEntityType, New PointF(50, 50))

                lrEntityType.SetReferenceMode(lsReferenceMode, False, lsValueTypeName)

                '=================================================================================================================
                'Create joined FactTypes.
                Dim lrFactType As FBM.FactType
                Dim lrColumn As RDS.Column
                Dim larModelObject As New List(Of FBM.ModelObject)

                For Each lrColumn In lrTable.Column.FindAll(Function(x) x.Name <> lrPrimaryKeyColumn.Name)
                    larModelObject.Clear()
                    larModelObject.Add(lrEntityType)

                    Dim lrJoinedModelObject As FBM.ModelObject

                    lrJoinedModelObject = Me.Model.FindEntityTypeByValueTypeId(lrColumn.Name)
                    If lrJoinedModelObject Is Nothing Then
                        lrJoinedModelObject = Me.Model.CreateValueType(lrColumn.Name)
                    End If

                    larModelObject.Add(lrJoinedModelObject)

                    Dim lrModelObject As FBM.ModelObject
                    Dim lsFactTypeName As String = ""

                    For Each lrModelObject In larModelObject
                        lsFactTypeName &= lrModelObject.Name
                    Next
                    lrFactType = Me.Model.CreateFactType(lsFactTypeName, larModelObject, False)

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrRole As FBM.Role

                    For Each lrRole In lrFactType.RoleGroup
                        larRole.Add(lrRole)
                    Next
                    Dim lasPredicatePart As New List(Of String)
                    lasPredicatePart.Add("has")
                    lasPredicatePart.Add("")

                    Dim lrFactTypeReading As New FBM.FactTypeReading(lrFactType, larRole, lasPredicatePart)

                    Call lrFactType.AddFactTypeReading(lrFactTypeReading, False, False)

                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = lrEntityType.Id)
                    larRole.Clear()
                    larRole.Add(lrRole)

                    Dim lrRoleConstraint As FBM.RoleConstraint

                    lrRoleConstraint = Me.Model.CreateRoleConstraint(pcenumRoleConstraintType.InternalUniquenessConstraint,
                                                                         larRole,
                                                                         "InternalUniquenessConstraint",
                                                                         1)

                    lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

                    Dim lrFactTypeInstance As FBM.FactTypeInstance
                    lrFactTypeInstance = lrPage.DropFactTypeAtPoint(lrFactType, New PointF(100, 100), False)
                Next 'Column
                '=================================================================================================================
            End If
        Next


    End Sub


    Private Sub GetIndexes()
        Try
            Dim lrODBCTable As System.Data.DataTable
            Dim lrTable As RDS.Table
            Dim lrIndex As RDS.Index

            If Not Me.ODBCConnection.State = ConnectionState.Open Then
                Me.ODBCConnection.Open()
            End If

            For Each lrTable In Me.Model.RDS.Table
                'lrODBCTable = Me.ODBCConnection.GetSchema("Indexes", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, lrTable.Name, Nothing})

                'Call Me.DisplayData(lrODBCTable)

                For Each lrRow As DataRow In lrODBCTable.Rows
                    lrIndex = lrTable.Index.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("INDEX_NAME")))

                    If lrIndex Is Nothing Then
                        lrIndex = New RDS.Index(lrTable, lrRow(lrODBCTable.Columns("INDEX_NAME")))
                        lrTable.Index.AddUnique(lrIndex)
                        lrIndex.NonUnique = CBool(lrRow(lrODBCTable.Columns("NON_UNIQUE")))
                        lrIndex.IndexQualifier = Viev.NullVal(lrRow(lrODBCTable.Columns("INDEX_QUALIFIER")), "")

                        Select Case lrRow(lrODBCTable.Columns("TYPE"))
                            Case Is = 1
                                lrIndex.IsPrimaryKey = True
                        End Select

                        If lrIndex.Name = "PRIMARY" Then
                            lrIndex.IsPrimaryKey = True
                        End If

                        If Me.Model.RDS.TargetDatabaseType = pcenumDatabaseType.MSJet Then
                            If lrIndex.Name = "PrimaryKey" Then
                                lrIndex.IsPrimaryKey = True
                            End If
                        End If

                        lrIndex.Type = lrRow(lrODBCTable.Columns("TYPE"))

                        Select Case lrRow(lrODBCTable.Columns("ASC_OR_DESC"))
                            Case Is = "A"
                                lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Ascending
                            Case Is = "D"
                                lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Descending
                        End Select
                        lrIndex.Cardinality = NullVal(lrRow(lrODBCTable.Columns("CARDINALITY")), 0)
                        lrIndex.Pages = NullVal(lrRow(lrODBCTable.Columns("PAGES")), 0)
                        lrIndex.FilterCondition = NullVal(lrRow(lrODBCTable.Columns("FILTER_CONDITION")), "")

                        Me.Model.RDS.Index.AddUnique(lrIndex)

                    End If

                    lrIndex.Column.Add(lrTable.Column.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("COLUMN_NAME"))))
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
            Dim lrODBCTable As System.Data.DataTable
            Dim lrTable As RDS.Table
            Dim lrColumn As RDS.Column

            If Not Me.ODBCConnection.State = ConnectionState.Open Then
                Me.ODBCConnection.Open()
            End If

            'lrODBCTable = Me.ODBCConnection.GetSchema("Columns", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, Nothing, Nothing})

            Dim lsTableName As String

            For Each lrRow As DataRow In lrODBCTable.Rows
                lsTableName = Trim(lrRow(lrODBCTable.Columns("TABLE_NAME")))
                lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = lsTableName)
                If lrTable IsNot Nothing Then
                    lrColumn = New RDS.Column(lrTable, lrRow(lrODBCTable.Columns("COLUMN_NAME")), Nothing, Nothing)

                    lrColumn.OrdinalPosition = lrRow(lrODBCTable.Columns("ORDINAL_POSITION"))
                    lrColumn.DataType = Me.Model.RDS.DataType.Find(Function(x) x.ProviderDBType = CInt(lrRow(lrODBCTable.Columns("DATA_TYPE"))))
                    lrColumn.ODBCDataType = lrRow(lrODBCTable.Columns("DATA_TYPE"))
                    lrColumn.DataTypeName = lrRow(lrODBCTable.Columns("TYPE_NAME"))
                    lrColumn.Nullable = CBool(lrRow(lrODBCTable.Columns("NULLABLE")))
                    lrColumn.IsNullable = lrRow(lrODBCTable.Columns("IS_NULLABLE")) = "YES"

                    lrColumn.TableCategory = lrRow(lrODBCTable.Columns("TABLE_CAT"))
                    lrColumn.TableSchema = NullVal(lrRow(lrODBCTable.Columns("TABLE_SCHEM")), "")
                    lrColumn.ColumnSize = CInt(lrRow(lrODBCTable.Columns("COLUMN_SIZE")))
                    lrColumn.BufferLength = CInt(NullVal(lrRow(lrODBCTable.Columns("BUFFER_LENGTH")), 0))
                    lrColumn.DecimalDigits = CInt(NullVal(lrRow(lrODBCTable.Columns("DECIMAL_DIGITS")), 0))
                    lrColumn.NumPrecRadix = CInt(NullVal(lrRow(lrODBCTable.Columns("NUM_PREC_RADIX")), 0))

                    lrColumn.Remarks = NullVal(lrRow(lrODBCTable.Columns("REMARKS")), "")
                    lrColumn.ColumnDef = NullVal(lrRow(lrODBCTable.Columns("COLUMN_DEF")), "")
                    lrColumn.SQLDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)
                    lrColumn.SQLDateTimeSub = NullVal(lrRow(lrODBCTable.Columns("SQL_DATETIME_SUB")), "")
                    lrColumn.CharOctetLength = NullVal(lrRow(lrODBCTable.Columns("CHAR_OCTET_LENGTH")), 0)
                    lrColumn.SSDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)

                    lrTable.Column.Add(lrColumn)
                Else
                    lrTable = New RDS.Table(Me.Model.RDS, lsTableName, Nothing)
                    Me.Model.RDS.addTable(lrTable)
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


End Class
