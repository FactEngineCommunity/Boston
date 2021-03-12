Imports Boston.ORMQL
Imports System.Data.SQLite

Namespace FactEngine

    Public Class SQLiteConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Public DatabaseConnectionString As String

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer,
                       Optional ByVal abCreatingNewDatabase As Boolean = False)

            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString
            Me.DefaultQueryLimit = aiDefaultQueryLimit

            If abCreatingNewDatabase Then Exit Sub

            Try
                Dim lrSQLiteConnection = Boston.Database.CreateConnection(Me.DatabaseConnectionString)
                Me.Connected = True 'Connections are actually made for each Query.
                lrSQLiteConnection.Close()
            Catch ex As Exception
                Me.Connected = False
                Throw New Exception("Could not connect to the database. Check the Model Configuration's Connection String.")
            End Try

        End Sub

        ''' <summary>
        ''' Adds a new Column to a Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overrides Sub addColumn(ByRef arColumn As RDS.Column)

            Dim lsSQLCommand As String

            Try
                lsSQLCommand = "ALTER TABLE " & arColumn.Table.Name
                lsSQLCommand &= " ADD COLUMN "
                lsSQLCommand &= arColumn.Name & " " & "TEXT(100)" '& arColumn.DataTypeName

                Me.GONonQuery(lsSQLCommand)

            Catch ex As Exception

            End Try

        End Sub

        Public Overrides Function createDatabase(ByVal asDatabaseLocationName As String) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset

            Try
                Call System.Data.SQLite.SQLiteConnection.CreateFile(asDatabaseLocationName)

                Return lrRecordset

            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        ''' Creates a new table in the database. Relational tablles must have at least one one column.
        ''' <summary>
        ''' <param name="arTable">The table to be created.</param>
        ''' <param name="arColumn">The column to be created for the new table.</param>
        ''' </summary>
        Public Overrides Sub createTable(ByRef arTable As RDS.Table, ByRef arColumn As RDS.Column)

            Dim lsSQLCommand As String

            Try
                lsSQLCommand = "CREATE TABLE " & arTable.Name
                lsSQLCommand &= " ("
                lsSQLCommand &= arColumn.Name & " " & "TEXT(100)" '& arColumn.DataTypeName
                lsSQLCommand &= ")"

                Me.GONonQuery(lsSQLCommand)

            Catch ex As Exception

            End Try


        End Sub

        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting to database.", True)
                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                If lrSQLiteConnection Is Nothing Then
                    Throw New Exception("SQLite Adaptor: Could not create SQLite database connection to execute the query.")
                End If

                Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, asQuery)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact
                'Richmond.WriteToStatusBar("Reading results.", True)

                '=====================================================
                'Column Names   
                '20200805-VM-To Do
                'Dim larProjectColumn = lrQueryGraph.getProjectionColumns
                Dim lsColumnName As String

                'For Each lrProjectColumn In larProjectColumn
                '    lrRecordset.Columns.Add(lrProjectColumn.Name)
                '    lsColumnName = lrFactType.CreateUniqueRoleName(lrProjectColumn.Name, 0)
                '    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                '    lrFactType.RoleGroup.AddUnique(lrRole)
                'Next

                For liFieldInd = 0 To lrSQLiteDataReader.FieldCount - 1
                    lsColumnName = lrFactType.CreateUniqueRoleName(lrSQLiteDataReader.GetName(liFieldInd), 0)
                    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                    lrRecordset.Columns.Add(lsColumnName)
                Next

                While lrSQLiteDataReader.Read()

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    Dim liInd As Integer
                    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                        Select Case lrSQLiteDataReader.GetFieldType(liInd)
                            Case Is = GetType(String)
                                If Not Viev.NullVal(lrSQLiteDataReader.GetFieldValue(Of Object)(liInd), "") = "" Then
                                    loFieldValue = lrSQLiteDataReader.GetString(liInd)
                                Else
                                    loFieldValue = ""
                                End If

                            Case Else
                                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                        End Select

                        Try
                            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(Viev.NullVal(loFieldValue, "")), lrFact))
                            '=====================================================
                        Catch
                            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                        End Try
                    Next

                    larFact.Add(lrFact)

                    If larFact.Count = Me.DefaultQueryLimit Then
                        lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                        Exit While
                    End If

                End While

                lrRecordset.Facts = larFact
                lrSQLiteConnection.Close()

                'Run the SQL against the database
                Return lrRecordset
            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        Public Overrides Function GONonQuery(ByVal asSQLQuery As String) As Recordset Implements iDatabaseConnection.GONonQuery

            Dim result As Integer = -1

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asSQLQuery

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                If lrSQLiteConnection Is Nothing Then
                    Throw New Exception("SQLite Adaptor: Could not create SQLite database connection to execute the query.")
                End If


                Using cmd As New System.Data.SQLite.SQLiteCommand(lrSQLiteConnection)
                    cmd.CommandText = asSQLQuery
                    cmd.Prepare()

                    Try
                        result = cmd.ExecuteNonQuery()
                    Catch SQLiteException As System.Data.SQLite.SQLiteException
                        Throw New Exception(SQLiteException.Message)
                    End Try
                End Using

                lrSQLiteConnection.Close()

                Return lrRecordset

            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        ''' <summary>
        ''' Removes the Column from its Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overrides Sub removeColumn(ByRef arColumn As RDS.Column)

            Try

                'Dim mSqliteDbConnection = New SQLiteConnection("Data Source=db_folder\MySqliteBasedApp.db;Version=3;Page Size=1024;")
                'SqliteDbConnection.Open()
                Dim columnDefinition = New List(Of String)()
                Dim mSql = "SELECT type, sql FROM sqlite_master WHERE tbl_name='" & arColumn.Table.Name & "'"
                'Dim mSqliteCommand = New SQLiteCommand(mSql, mSqliteDbConnection)
                Dim sqlScript As String = ""

                Dim lrRecordset = Me.GO(mSql)
                'Using CSharpImpl.__Assign(mSqliteReader, mSqliteCommand.ExecuteReader())
                While Not lrRecordset.EOF
                    sqlScript = lrRecordset("sql").Data
                    Exit While
                    lrRecordset.MoveNext()
                End While
                'End Using

                If Not String.IsNullOrEmpty(sqlScript) Then
                    Dim firstIndex As Integer = sqlScript.IndexOf("(")
                    Dim lastIndex As Integer = sqlScript.LastIndexOf(")")

                    If firstIndex >= 0 AndAlso lastIndex <= sqlScript.Length - 1 Then
                        sqlScript = sqlScript.Substring(firstIndex, lastIndex - firstIndex + 1)
                    End If

                    Dim scriptParts As String() = sqlScript.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

                    For Each s As String In scriptParts
                        If Not s.Contains(arColumn.Name) Then
                            columnDefinition.Add(s)
                        End If
                    Next
                End If

                Dim columnDefinitionString As String = String.Join(",", columnDefinition)
                Dim columns As List(Of String) = New List(Of String)()
                mSql = "PRAGMA table_info(" & arColumn.Table.Name & ")"
                'mSqliteCommand = New SQLiteCommand(mSql, mSqliteDbConnection)
                lrRecordset = Me.GO(mSql)
                'Using CSharpImpl.__Assign(mSqliteReader, mSqliteCommand.ExecuteReader())

                While Not lrRecordset.EOF
                    columns.Add(lrRecordset("name").Data)
                    lrRecordset.MoveNext()
                End While
                '    End Using

                columns.Remove(arColumn.Name)
                Dim columnString As String = String.Join(",", columns)

                mSql = "PRAGMA foreign_keys=OFF"
                '    mSqliteCommand = New SQLiteCommand(mSql, mSqliteDbConnection)
                Me.GONonQuery(mSql)
                'Dim n As Integer = mSqliteCommand.ExecuteNonQuery()

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                        cmd.Transaction = tr
                        Dim query As String = "CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup " & columnDefinitionString & ")"
                        cmd.CommandText = query
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_backup SELECT " & columnString & " FROM " & arColumn.Table.Name
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "CREATE TABLE " & arColumn.Table.Name & columnDefinitionString & ")"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & " SELECT " & columnString & " FROM " & arColumn.Table.Name & "_backup;"
                        cmd.ExecuteNonQuery()
                        cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & "_backup"
                        cmd.ExecuteNonQuery()
                    End Using

                    tr.Commit()
                End Using

                mSql = "PRAGMA foreign_keys=ON"
                'mSqliteCommand = New SQLiteCommand(mSql, mSqliteDbConnection)
                'n = mSqliteCommand.ExecuteNonQuery()
                Me.GONonQuery(mSql)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub


    End Class


End Namespace
