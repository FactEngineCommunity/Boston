Imports Boston.ORMQL
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Reflection

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
        End Function

        Public Overrides Function getRelationsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)

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
            '        Debugger.Break()
            '    End Try

            '    Try
            '        connection.Open()
            '    Catch ex As Exception
            '        Debugger.Break()
            '    End Try


            '    Dim restrictions As String() = New String() {Nothing}
            '    Dim schema As DataTable
            '    schema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, restrictions)

            '    For Each row As DataRow In schema.Rows
            '        Debugger.Break()
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

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getTables() As List(Of RDS.Table)
            Return New List(Of RDS.Table)
        End Function

        Public Overrides Function GO(asQuery As String) As Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting to database.", True)

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
    End Class

End Namespace
