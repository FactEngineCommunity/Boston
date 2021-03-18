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

        ''' <summary>
        ''' Changes the data type of the nominated column.
        ''' </summary>
        ''' <param name="arColumn">The Column to have its data type changed.</param>
        ''' <param name="asDataType">The new data type.</param>
        ''' <param name="asLength">The length of the data type. 0 is nothing.</param>
        ''' <param name="arPrecision">The precision of the data type. 0 is nothing.</param>
        Public Overrides Sub columnChangeDatatype(ByRef arColumn As RDS.Column,
                                                    ByVal asDataType As pcenumORMDataType,
                                                    ByVal asLength As Integer,
                                                    ByRef arPrecision As Integer)
            Try
                Call Me.FBMModel.connectToDatabase()

                Dim columnDefinition = New List(Of String)()
                Dim mSql = "SELECT type, sql FROM sqlite_master WHERE tbl_name='" & arColumn.Table.Name & "'"
                Dim sqlScript As String = ""

                Dim lrRecordset = Me.GO(mSql)
                sqlScript = lrRecordset("sql").Data

                If Not String.IsNullOrEmpty(sqlScript) Then
                    Dim firstIndex As Integer = sqlScript.IndexOf("(")
                    Dim lastIndex As Integer = sqlScript.LastIndexOf(")")

                    If firstIndex >= 0 AndAlso lastIndex <= sqlScript.Length - 1 Then
                        sqlScript = sqlScript.Substring(firstIndex + 1, lastIndex - firstIndex - 1)
                    End If

                    Dim scriptParts As String() = sqlScript.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

                    For Each s As String In scriptParts
                        If s.Contains(arColumn.Name) Then
                            s = arColumn.Name
                            s &= " " & arColumn.ActiveRole.JoinsValueType.DBDataType
                            If arColumn.ActiveRole.JoinsValueType.DataTypeLength > 0 Then
                                If arColumn.DataTypeIsText Then
                                    s &= "(" & arColumn.ActiveRole.JoinsValueType.DataTypeLength.ToString
                                    If arColumn.ActiveRole.JoinsValueType.DataTypePrecision > 0 Then
                                        s &= arColumn.ActiveRole.JoinsValueType.DataTypePrecision.ToString
                                    End If
                                    s &= ")"
                                End If
                            End If
                            columnDefinition.Add(s)
                        Else
                            columnDefinition.Add(s)
                        End If
                    Next
                End If

                Dim columnDefinitionString As String = String.Join(",", columnDefinition)
                Dim columns As List(Of String) = New List(Of String)()

                mSql = "PRAGMA table_info(" & arColumn.Table.Name & ")"
                lrRecordset = Me.GO(mSql)

                While Not lrRecordset.EOF
                    columns.Add(lrRecordset("name").Data)
                    lrRecordset.MoveNext()
                End While

                Dim columnString As String = String.Join(",", columns)

                mSql = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(mSql)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            If columns.Count > 0 Then
                                Dim query As String = "CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & columnDefinitionString & ")"
                                cmd.CommandText = query
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_backup SELECT " & columnString & " FROM " & arColumn.Table.Name
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "CREATE TABLE " & arColumn.Table.Name & "(" & columnDefinitionString & ")"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & " SELECT " & columnString & " FROM " & arColumn.Table.Name & "_backup;"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & "_backup"
                                cmd.ExecuteNonQuery()
                            Else
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                                cmd.ExecuteNonQuery()
                            End If
                        End Using

                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    Finally
                        tr.Commit()
                    End Try
                End Using

                mSql = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(mSql)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub

        ''' <summary>
        ''' Sets whether the specified Column is mandatory or not, in the database.
        ''' </summary>
        ''' <param name="arColumn">The Column to have its schema definition changed.</param>
        ''' <param name="abIsMandatory">True if the Column is mandatory for its Table.</param>
        Public Overrides Sub columnSetMandatory(ByRef arColumn As RDS.Column,
                                                  ByVal abIsMandatory As Boolean)
            Try
                Call Me.FBMModel.connectToDatabase()

                Dim columnDefinition = New List(Of String)()
                Dim mSql = "SELECT type, sql FROM sqlite_master WHERE tbl_name='" & arColumn.Table.Name & "'"
                Dim lsSQLScript As String = ""

                Dim lrRecordset = Me.GO(mSql)
                lsSQLScript = lrRecordset("sql").Data

                If Not String.IsNullOrEmpty(lsSQLScript) Then
                    Dim firstIndex As Integer = lsSQLScript.IndexOf("(")
                    Dim lastIndex As Integer = lsSQLScript.LastIndexOf(")")

                    If firstIndex >= 0 AndAlso lastIndex <= lsSQLScript.Length - 1 Then
                        lsSQLScript = lsSQLScript.Substring(firstIndex + 1, lastIndex - firstIndex - 1)
                    End If

                    Dim scriptParts As String() = lsSQLScript.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

                    For Each s As String In scriptParts
                        If s.Contains(arColumn.Name) Then
                            s = arColumn.makeSQLColumnDefinition
                            columnDefinition.Add(s)
                        Else
                            columnDefinition.Add(s)
                        End If
                    Next
                End If

                Dim lsColumnDefinitions As String = String.Join(",", columnDefinition)
                Dim larColumnName As List(Of String) = New List(Of String)()

                mSql = "PRAGMA table_info(" & arColumn.Table.Name & ")"
                lrRecordset = Me.GO(mSql)

                While Not lrRecordset.EOF
                    larColumnName.Add(lrRecordset("name").Data)
                    lrRecordset.MoveNext()
                End While

                Dim lsColumnList As String = String.Join(",", larColumnName)

                mSql = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(mSql)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            If larColumnName.Count > 0 Then
                                cmd.CommandText = "CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_backup SELECT " & lsColumnList & " FROM " & arColumn.Table.Name
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "CREATE TABLE " & arColumn.Table.Name & "(" & lsColumnDefinitions & ")"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & " SELECT " & lsColumnList & " FROM " & arColumn.Table.Name & "_backup;"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & "_backup"
                                cmd.ExecuteNonQuery()
                            Else
                                cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                                cmd.ExecuteNonQuery()
                            End If
                        End Using

                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    Finally
                        tr.Commit()
                    End Try
                End Using

                mSql = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(mSql)

            Catch ex As Exception
                Debugger.Break()
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
                Dim columnDefinition = New List(Of String)()
                Dim mSql = "SELECT type, sql FROM sqlite_master WHERE tbl_name='" & arColumn.Table.Name & "'"
                Dim sqlScript As String = ""

                Dim lrRecordset = Me.GO(mSql)
                sqlScript = lrRecordset("sql").Data

                If Not String.IsNullOrEmpty(sqlScript) Then
                    Dim firstIndex As Integer = sqlScript.IndexOf("(")
                    Dim lastIndex As Integer = sqlScript.LastIndexOf(")")

                    If firstIndex >= 0 AndAlso lastIndex <= sqlScript.Length - 1 Then
                        sqlScript = sqlScript.Substring(firstIndex + 1, lastIndex - firstIndex - 1)
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
                lrRecordset = Me.GO(mSql)

                While Not lrRecordset.EOF
                    columns.Add(lrRecordset("name").Data)
                    lrRecordset.MoveNext()
                End While

                columns.Remove(arColumn.Name)

                Dim columnString As String = String.Join(",", columns)

                mSql = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(mSql)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                        cmd.Transaction = tr
                        If columns.Count > 0 Then
                            Dim query As String = "CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & columnDefinitionString & ")"
                            cmd.CommandText = query
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_backup SELECT " & columnString & " FROM " & arColumn.Table.Name
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "CREATE TABLE " & arColumn.Table.Name & "(" & columnDefinitionString & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & " SELECT " & columnString & " FROM " & arColumn.Table.Name & "_backup;"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & "_backup"
                            cmd.ExecuteNonQuery()
                        Else
                            cmd.CommandText = "DROP TABLE " & arColumn.Table.Name & ""
                            cmd.ExecuteNonQuery()
                        End If
                    End Using

                    tr.Commit()
                End Using

                mSql = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(mSql)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub

        ''' <summary>
        ''' Removes/Drops the Table from the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overrides Sub removeTable(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLCommand = "DROP TABLE " & arTable.Name

                Me.GONonQuery(lsSQLCommand)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub

        ''' <summary>
        ''' Renames the given Column to the new column name.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="asNewColumnName"></param>
        Public Overrides Sub renameColumn(ByRef arColumn As RDS.Column, ByVal asNewColumnName As String)

            Try
                Dim lsSQLCommmand = "ALTER TABLE " & arColumn.Table.Name
                lsSQLCommmand &= " RENAME COLUMN " & arColumn.Name & " TO " & asNewColumnName

                Me.GONonQuery(lsSQLCommmand)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub

        ''' <summary>
        ''' Renames a table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <param name="asNewName"></param>
        Public Overrides Sub RenameTable(ByRef arTable As RDS.Table, ByVal asNewName As String)

            Try
                Dim lsSQLCommmand = "ALTER TABLE " & arTable.Name
                lsSQLCommmand &= " RENAME TO " & asNewName

                Me.GONonQuery(lsSQLCommmand)

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub


    End Class


End Namespace
