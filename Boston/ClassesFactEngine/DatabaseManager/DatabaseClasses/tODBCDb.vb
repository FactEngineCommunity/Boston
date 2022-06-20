Imports Boston.ORMQL
Imports Boston.RDS
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Reflection
Imports System.Threading.Tasks

Namespace FactEngine

    Public Class ODBCConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Public DatabaseConnectionString As String

        Public ODBCConnection As System.Data.Odbc.OdbcConnection

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer)

            Try
                Me.FBMModel = arFBMModel
                Me.DatabaseConnectionString = asDatabaseConnectionString
                Me.DefaultQueryLimit = aiDefaultQueryLimit

                Me.ODBCConnection = New System.Data.Odbc.OdbcConnection(Me.DatabaseConnectionString)
                Try
                    Me.ODBCConnection.Open()
                    Me.Connected = True
                Catch
                    MsgBox("Failed To open the MongoDB database connection.")
                End Try

            Catch ex As Exception

            End Try

        End Sub

        ''' <summary>
        ''' Returns a list of the Relations/ForeignKeys in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)
            Return New List(Of RDS.Relation)
        End Function

        ''' <summary>
        ''' Returns a list of the Indexes in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getIndexesByTable(ByRef arTable As RDS.Table) As List(Of RDS.Index)
            Return New List(Of RDS.Index)
            'Try
            '    Dim lrODBCTable As System.Data.DataTable
            '    Dim lrTable As RDS.Table
            '    Dim lrIndex As RDS.Index

            '    If Not Me.ODBCConnection.State = ConnectionState.Open Then
            '        Me.ODBCConnection.Open()
            '    End If

            '    For Each lrTable In Me.Model.RDS.Table
            '        'lrODBCTable = Me.ODBCConnection.GetSchema("Indexes", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, lrTable.Name, Nothing})

            '        'Call Me.DisplayData(lrODBCTable)

            '        For Each lrRow As DataRow In lrODBCTable.Rows
            '            lrIndex = lrTable.Index.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("INDEX_NAME")))

            '            If lrIndex Is Nothing Then
            '                lrIndex = New RDS.Index(lrTable, lrRow(lrODBCTable.Columns("INDEX_NAME")))
            '                lrTable.Index.AddUnique(lrIndex)
            '                lrIndex.NonUnique = CBool(lrRow(lrODBCTable.Columns("NON_UNIQUE")))
            '                lrIndex.IndexQualifier = Viev.NullVal(lrRow(lrODBCTable.Columns("INDEX_QUALIFIER")), "")

            '                Select Case lrRow(lrODBCTable.Columns("TYPE"))
            '                    Case Is = 1
            '                        lrIndex.IsPrimaryKey = True
            '                End Select

            '                If lrIndex.Name = "PRIMARY" Then
            '                    lrIndex.IsPrimaryKey = True
            '                End If

            '                If Me.Model.RDS.TargetDatabaseType = pcenumDatabaseType.MSJet Then
            '                    If lrIndex.Name = "PrimaryKey" Then
            '                        lrIndex.IsPrimaryKey = True
            '                    End If
            '                End If

            '                lrIndex.Type = lrRow(lrODBCTable.Columns("TYPE"))

            '                Select Case lrRow(lrODBCTable.Columns("ASC_OR_DESC"))
            '                    Case Is = "A"
            '                        lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Ascending
            '                    Case Is = "D"
            '                        lrIndex.AscendingOrDescending = pcenumODBCAscendingOrDescending.Descending
            '                End Select
            '                lrIndex.Cardinality = NullVal(lrRow(lrODBCTable.Columns("CARDINALITY")), 0)
            '                lrIndex.Pages = NullVal(lrRow(lrODBCTable.Columns("PAGES")), 0)
            '                lrIndex.FilterCondition = NullVal(lrRow(lrODBCTable.Columns("FILTER_CONDITION")), "")

            '                Me.Model.RDS.Index.AddUnique(lrIndex)

            '            End If

            '            lrIndex.Column.Add(lrTable.Column.Find(Function(x) x.Name = lrRow(lrODBCTable.Columns("COLUMN_NAME"))))
            '        Next
            '    Next

            'Catch ex As Exception
            '    Dim lsMessage As String
            '    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            '    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            '    lsMessage &= vbCrLf & vbCrLf & ex.Message
            '    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            'End Try


        End Function

        Public Overrides Function getRelationsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)

            'Could be important. Don't know what 'Restrictions' are.
            'lrODBCTable = lrODBCConnection.GetSchema("Restrictions")

            'OLEDB Connection String
            'Provider=MSDASQL;Persist Security Info=False;DSN=SQLiteTest

            'Try
            '    Dim lrSQLConnectionStringBuilder As System.Data.Common.DbConnectionStringBuilder = Nothing

            '    lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True) With {
            '               .ConnectionString = Me.Model.TargetDatabaseConnectionString
            '            }

            '    Dim lsDatabaseLocation = lrSQLConnectionStringBuilder("DSN")

            '    Dim lsConnectionString As String = ""
            '    lsConnectionString = "Provider=MSDATASHAPE;DRIVER={SQLite3 ODBC Driver};DSN=" & lsDatabaseLocation
            '    lsConnectionString = "Provider=MSDASQL;DRIVER={SQLite3 ODBC Driver};OPTION=3;FILEDSN=C:\Users\Victor\Desktop\Test.dsn"
            '    lsConnectionString = "Provider={SQLite3 ODBC Driver};Database=C:\Users\Victor\OneDrive\00-FactEngine\01-Marketing\02-Product\01-Products\00-FactEngine\Research\SemanticParsing\SpiderChallenge\databases\database\academic\academic.sqlite"

            '    'Have to use...'.Net Framework Data Provider for ODBC connection string
            '    Dim connection As New System.Data.OleDb.OleDbConnection
            '    Try
            '        connection = New System.Data.OleDb.OleDbConnection(lsConnectionString)
            '    Catch ex As Exception

            '    End Try

            '    Try
            '        connection.Open()
            '    Catch ex As Exception

            '    End Try


            '    Dim restrictions As String() = New String() {Nothing}
            '    Dim schema As DataTable
            '    schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, restrictions)

            '    For Each row As DataRow In schema.Rows

            '        'Dim dbForeignKey As ForeignKey = New ForeignKey()
            '        'dbForeignKey.Name = row("FK_NAME").ToString()
            '        'dbForeignKey.OriginalName = row("FK_NAME").ToString()
            '        'dbForeignKey.FKTableName = row("FK_TABLE_NAME").ToString()
            '        'Dim fkc As ForeignKeyColumn = New ForeignKeyColumn()
            '        'fkc.Name = row("FK_COLUMN_NAME").ToString()
            '        'dbForeignKey.FKColumns.Add(fkc)
            '        'dbForeignKey.FKTableSchema = schema.ToString()
            '        'dbForeignKey.PKTableName = row("PK_TABLE_NAME").ToString()
            '        'Dim pkc As ForeignKeyColumn = New ForeignKeyColumn()
            '        'pkc.Name = row("PK_COLUMN_NAME").ToString()
            '        'dbForeignKey.PKColumns.Add(pkc)
            '        'dbForeignKey.PKTableSchema = schema.ToString()
            '        'foreignKeys.Add(dbForeignKey)
            '    Next


            Return New List(Of RDS.Relation)
        End Function

        Public Overrides Function getColumnsByTable(ByRef arTable As Table) As List(Of RDS.Column)

            Return New List(Of Column)
            'Dim lrODBCTable As System.Data.DataTable
            'Dim lrTable As RDS.Table
            'Dim lrColumn As RDS.Column

            'If Not Me.ODBCConnection.State = ConnectionState.Open Then
            '    Me.ODBCConnection.Open()
            'End If

            ''lrODBCTable = Me.ODBCConnection.GetSchema("Columns", New String() {Nothing, Me.ComboBoxSchema.SelectedItem, Nothing, Nothing})

            'Dim lsTableName As String

            'For Each lrRow As DataRow In lrODBCTable.Rows
            '    lsTableName = Trim(lrRow(lrODBCTable.Columns("TABLE_NAME")))
            '    lrTable = Me.Model.RDS.Table.Find(Function(x) x.Name = lsTableName)
            '    If lrTable IsNot Nothing Then
            '        lrColumn = New RDS.Column(lrTable, lrRow(lrODBCTable.Columns("COLUMN_NAME")), Nothing, Nothing)

            '        lrColumn.OrdinalPosition = lrRow(lrODBCTable.Columns("ORDINAL_POSITION"))
            '        lrColumn.DataType = Me.Model.RDS.DataType.Find(Function(x) x.ProviderDBType = CInt(lrRow(lrODBCTable.Columns("DATA_TYPE"))))
            '        lrColumn.ODBCDataType = lrRow(lrODBCTable.Columns("DATA_TYPE"))
            '        lrColumn.DataTypeName = lrRow(lrODBCTable.Columns("TYPE_NAME"))
            '        lrColumn.Nullable = CBool(lrRow(lrODBCTable.Columns("NULLABLE")))
            '        lrColumn.IsNullable = lrRow(lrODBCTable.Columns("IS_NULLABLE")) = "YES"

            '        lrColumn.TableCategory = lrRow(lrODBCTable.Columns("TABLE_CAT"))
            '        lrColumn.TableSchema = NullVal(lrRow(lrODBCTable.Columns("TABLE_SCHEM")), "")
            '        lrColumn.ColumnSize = CInt(lrRow(lrODBCTable.Columns("COLUMN_SIZE")))
            '        lrColumn.BufferLength = CInt(NullVal(lrRow(lrODBCTable.Columns("BUFFER_LENGTH")), 0))
            '        lrColumn.DecimalDigits = CInt(NullVal(lrRow(lrODBCTable.Columns("DECIMAL_DIGITS")), 0))
            '        lrColumn.NumPrecRadix = CInt(NullVal(lrRow(lrODBCTable.Columns("NUM_PREC_RADIX")), 0))

            '        lrColumn.Remarks = NullVal(lrRow(lrODBCTable.Columns("REMARKS")), "")
            '        lrColumn.ColumnDef = NullVal(lrRow(lrODBCTable.Columns("COLUMN_DEF")), "")
            '        lrColumn.SQLDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)
            '        lrColumn.SQLDateTimeSub = NullVal(lrRow(lrODBCTable.Columns("SQL_DATETIME_SUB")), "")
            '        lrColumn.CharOctetLength = NullVal(lrRow(lrODBCTable.Columns("CHAR_OCTET_LENGTH")), 0)
            '        lrColumn.SSDataType = NullVal(lrRow(lrODBCTable.Columns("SQL_DATA_TYPE")), 0)

            '        lrTable.Column.Add(lrColumn)
            '    Else
            '        lrTable = New RDS.Table(Me.Model.RDS, lsTableName, Nothing)
            '        Me.Model.RDS.addTable(lrTable)
            '    End If
            'Next

        End Function

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getTables() As List(Of RDS.Table)
            Return New List(Of RDS.Table)

            'Dim lrODBCTable As System.Data.DataTable
            'Dim lrTable As RDS.Table
            'Dim lrPage As FBM.Page

            'Try
            '    lrODBCTable = Me.ODBCConnection.GetSchema("Tables")
            'Catch ex1 As Exception

            'End Try

            ''Dim blah = System.Data.Odbc.OdbcMetaDataCollectionNames.


            'Select Case Me.ODBCConnection.DataSource
            '    Case Is = "ACCESS"
            '        For Each lrRow As DataRow In lrODBCTable.Rows
            '            lrTable = New RDS.Table(Me.TempModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing)
            '            Select Case lrRow(lrODBCTable.Columns("TABLE_TYPE"))
            '                Case Is = "SYSTEM TABLE"
            '                    lrTable.IsSystemTable = True
            '                Case Else
            '                    lrTable.IsSystemTable = False
            '            End Select

            '            If Not lrTable.IsSystemTable Then
            '                Me.TempModel.RDS.Table.AddUnique(lrTable)

            '                If Not Me.CreatePagePerTable Then
            '                    lrPage = New FBM.Page(Me.Model, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel)
            '                    frmMain.zfrmModelExplorer.AddPageToModel(Me.Model.TreeNode, lrPage, True)
            '                End If
            '            End If
            '        Next
            '    Case Else
            '        For Each lrRow As DataRow In lrODBCTable.Rows
            '            'Create the Table into the TempModel
            '            Me.TempModel.RDS.Table.Add(New RDS.Table(Me.TempModel.RDS, lrRow(lrODBCTable.Columns("TABLE_NAME")), Nothing))

            '            If Not Me.CreatePagePerTable Then
            '                lrPage = New FBM.Page(Me.Model, Nothing, lrRow(lrODBCTable.Columns("TABLE_NAME")), pcenumLanguage.ORMModel) '(lrODBCTable.Columns("TABLE_NAME"))
            '                If frmMain.zfrmModelExplorer IsNot Nothing Then
            '                    frmMain.zfrmModelExplorer.AddPageToModel(Me.Model.TreeNode, lrPage, True)
            '                End If
            '            End If
            '            'End If
            '        Next
            'End Select

        End Function

        Public Overrides Function GO(asQuery As String) As Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Boston.WriteToStatusBar("Connecting to database.", True)

                Dim adapter As OdbcDataAdapter = New OdbcDataAdapter(asQuery, Me.ODBCConnection)
                Dim lrDataSet As New DataSet

                adapter.Fill(lrDataSet)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact

                '=====================================================
                'Column Names   

                For Each lrColumn In lrDataSet.Tables(0).Columns
                    Dim lrRole = New FBM.Role(lrFactType, lrColumn.ToString, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                    lrRecordset.Columns.Add(lrColumn.ToString)
                Next

                For Each lrRow As DataRow In lrDataSet.Tables(0).Rows

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    Dim liInd As Integer
                    For liInd = 0 To lrRow.ItemArray.Count - 1
                        loFieldValue = lrRow.Item(liInd).ToString

                        Try
                            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                            '=====================================================
                        Catch
                            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                        End Try
                    Next

                    larFact.Add(lrFact)

                    If larFact.Count = Me.DefaultQueryLimit Then
                        lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                        Exit For
                    End If

                Next

                lrRecordset.Facts = larFact

                'Run the SQL against the database
                Return lrRecordset
            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        Private Function iDatabaseConnection_GONonQuery(asQuery As String) As Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function

        Private Function iDatabaseConnection_GOAsync(asQuery As String) As Task(Of Recordset) Implements iDatabaseConnection.GOAsync
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
