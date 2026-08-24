Imports Boston.ORMQL
Imports System.Data.SQLite
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Reflection
Imports Boston.FactEngine.DatabaseConnection

Namespace FactEngine

    Public Class SQLiteConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Private ReadOnly _activeTransactions As List(Of IDbTransaction) = New List(Of IDbTransaction)()

        Private _Connection As Data.SQLite.SQLiteConnection = Nothing
        Public Property Connection As Data.SQLite.SQLiteConnection
            Get
                Return Me._Connection
            End Get
            Set(value As Data.SQLite.SQLiteConnection)
                Me._Connection = value
            End Set
        End Property

        ' Stopwatch for profiling
        Private stopwatch As New Stopwatch()

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Call Me.RegisterCustomFunctions()
        End Sub

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer,
                       Optional ByVal abCreatingNewDatabase As Boolean = False)

            Call Me.New 'Registers Custom (vb.net) functions.

            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString
            Me.DefaultQueryLimit = aiDefaultQueryLimit

            If abCreatingNewDatabase Then Exit Sub

            Try
                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                If lrSQLiteConnection Is Nothing Then Throw New Exception("Failed to create SQLiteConnection")

                Me.Connected = True 'Connections can actually be made for each Query. Keep open for Boston. E.g. When SQLite is the database type for Boston itself.
                Me._Connection = Nothing '20231029-VM-Was lrSQLiteConnection

                lrSQLiteConnection.Close()

                'Call Me.GONonQuery("PRAGMA foreign_keys = ON;") '20231029-Test without

                Me.State = 1

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
                Dim lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                lsSQLCommand = "ALTER TABLE [" & arColumn.Table.DBName & "]"
                lsSQLCommand &= " ADD COLUMN "
                lsSQLCommand &= Me.generateSQLColumnDefinition(arColumn)

                Dim lrRecordset As ORMQL.Recordset = Nothing

                Try
                    lrRecordset = Me.GONonQuery(lsSQLCommand)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                If lrRecordset.ErrorReturned Then
                    MsgBox(lrRecordset.ErrorString)
                End If

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)

            Catch ex As Exception

            End Try

        End Sub

        ''' <summary>
        ''' Adds the referenced Index to the database. Table is within Index definition.
        ''' </summary>
        ''' <param name="arIndex">The Index to be added to the database.</param>
        Public Overrides Sub addIndex(ByRef arIndex As RDS.Index)

            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrIndex As RDS.Index = arIndex
                Dim lasColumnNames = From Column In lrIndex.Table.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()

#Region "Special Indexes"
                            'When an Index in SQLite has a Column that may be NULL, need to add another special index.                            
                            Dim larIndexWithNullableColumn = (From Index In lrIndex.Table.Index
                                                              From Column In Index.Column
                                                              Where Not Column.IsMandatory
                                                              Select Index).Distinct

                            Dim liInd = 1
                            For Each lrIndex In larIndexWithNullableColumn
                                Dim lsSQLCommand As String = ""

                                cmd.CommandText = "DROP INDEX uc_" & arIndex.Table.Name & "special" & liInd.ToString
                                Try
                                    cmd.ExecuteNonQuery()
                                Catch ex As Exception
                                    'Tried
                                End Try

                                lsSQLCommand = "CREATE UNIQUE INDEX uc_" & lrIndex.Table.Name & "special" & liInd.ToString & " ON " & lrIndex.Table.Name & "_temp" & "("
                                Dim liInd2 = 0
                                For Each lrColumn In lrIndex.Column.FindAll(Function(x) x.IsMandatory)
                                    If liInd2 > 0 Then lsSQLCommand &= ","
                                    lsSQLCommand &= "[" & lrColumn.Name & "]"
                                    liInd2 += 1
                                Next
                                lsSQLCommand &= ")"
                                liInd2 = 0
                                lsSQLCommand &= " WHERE "
                                For Each lrColumn In lrIndex.Column.FindAll(Function(x) Not x.IsMandatory)
                                    If liInd2 > 0 Then
                                        lsSQLCommand &= " AND "
                                    End If
                                    lsSQLCommand &= lrColumn.Name
                                    lsSQLCommand &= " IS NULL"
                                    liInd2 += 1
                                Next
                                cmd.CommandText = lsSQLCommand
                                cmd.ExecuteNonQuery()
                                liInd += 1
                            Next
#End Region


                            cmd.CommandText = "INSERT INTO [" & arIndex.Table.Name & "_temp] SELECT " & lsColumnList & " FROM [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arIndex.Table.Name & "_temp] RENAME TO [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Appends a String data type Column with a string, with a line break before hand if required.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="asAppendString"></param>
        ''' <param name="jsonWhereString">JSON that has the Key/Value pairs for the WHERE clause</param>
        ''' <param name="abWithLineBreak"></param>
        Public Overrides Function AppendStringColumn(ByVal asTableName As String,
                                                     ByVal asColumnName As String,
                                                     ByVal asAppendString As String,
                                                     ByVal jsonWhereString As String,
                                                     Optional ByVal abWithLineBreak As Boolean = False) As Boolean

            Try
                Dim lsSQLQuery As String = ""
                Dim lsNewlinePart As String = "'\n' || "

                If Not abWithLineBreak Then
                    lsNewlinePart = ""
                End If

#Region "TableName | WHERE CLAUSE etc"
                ' Parse the JSON string
                Dim jsonObject As JObject = JObject.Parse(jsonWhereString)

                ' Determine the table name and columns/values
                Dim tableName As String = Nothing
                Dim columns As New List(Of String)
                Dim values As New List(Of String)
                Dim parameters As New List(Of SQLiteParameter)

                Dim lrTable As RDS.Table = Nothing

                lrTable = Me.FBMModel.RDS.Table.Find(Function(x) x.DBName = asTableName)
                If lrTable Is Nothing Then
                    Return False
                End If

                ' Assuming there's only one table name key in the JSON


                For Each column As KeyValuePair(Of String, JToken) In jsonObject

                    Dim lrColumn As RDS.Column = lrTable.Column.Find(Function(x) x.DBName = column.Key)

                    If lrColumn IsNot Nothing Then
                        columns.Add(column.Key)
                        Dim lsColumnValue As String = "<Error>"
                        Select Case lrColumn.getMetamodelDataType
                            'Case Is = pcenumORMDataType.AutoUUID
                            '    lsColumnValue = System.Guid.NewGuid.ToString
                            Case Else
                                lsColumnValue = column.Value.ToString
                        End Select
                        values.Add(Me.DataTypeWrapper(lrColumn.getMetamodelDataType) & lsColumnValue & Me.DataTypeWrapper(lrColumn.getMetamodelDataType))
                    End If
                Next

                ' Construct the query
                lsSQLQuery = "UPDATE " & asTableName
                asAppendString = asAppendString.Replace("'", "''")
                lsSQLQuery &= " SET " & asColumnName & " = " & asColumnName & " || " & lsNewlinePart & "'" & asAppendString & "'"


                Dim lrRecordset = Me.GONonQuery(lsSQLQuery)

                Dim lsWhereClause = " WHERE "

                Dim liInd = 0
                Dim liInd2 = 0
                For Each lrColumn In lrTable.getPrimaryKeyColumns
                    liInd = columns.IndexOf(lrColumn.DBName)
                    If liInd2 > 0 Then lsWhereClause.AppendString(" AND ")
                    lsWhereClause.AppendLine(columns(liInd) & " = " & values(liInd))
                    columns.RemoveAt(liInd)
                    values.RemoveAt(liInd)
                    liInd2 += 1
                Next

                liInd = 0
                For Each lsColumnName In columns
                    If liInd > 0 Then lsSQLQuery &= ","
                    lsSQLQuery.AppendLine(columns(liInd) & " = " & values(liInd))
                    liInd += 1
                Next

                lsSQLQuery.AppendLine(lsWhereClause)

                lrRecordset = Me.GONonQuery(lsSQLQuery)

                Return lrRecordset.ErrorReturned
#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Adds the given Relation/ForeignKey to the database. Relation holds relative Tables.
        ''' </summary>
        ''' <param name="arRelation"></param>
        Public Overrides Sub AddForeignKey(ByRef arRelation As RDS.Relation)

            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrRelation As RDS.Relation = arRelation
                Dim lasColumnNames = From Column In lrRelation.OriginTable.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arRelation.OriginTable, arRelation.OriginTable.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO [" & arRelation.OriginTable.Name & "_temp] SELECT " & lsColumnList & " FROM [" & arRelation.OriginTable.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arRelation.OriginTable.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arRelation.OriginTable.Name & "_temp] RENAME TO [" & arRelation.OriginTable.Name & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Shadows Function BeginTrans() As SQLiteTransaction
            Try
                Return Nothing '20231029-VM-Trying this.
                Dim transaction As SQLiteTransaction = Nothing
                If Me._Connection IsNot Nothing Then
                    transaction = Me._Connection.BeginTransaction
                    _activeTransactions.Add(transaction)
                End If
                Return transaction
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

                Return Nothing
            End Try

        End Function

        Public Shadows Function BeginTransaction() As SQLiteTransaction
            Return Nothing '20231029-VM-Trying without.
            Return Me.BeginTrans
        End Function

        Public Overrides Sub Close()
            Try
                If Me._Connection IsNot Nothing Then
                    Me._Connection.Close()
                End If
                Me._Connection = Nothing
                Me.State = 0
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrColumn As RDS.Column = arColumn
                Dim lasColumnNamesFinal As New List(Of String)

                Dim lasColumnNames = (From Column In arColumn.Table.Column
                                      Select $"[{Column.DBName}]").ToList

                For Each lsColumnName In lasColumnNames.ToArray
                    If Not Me.getColumnsByTable(arColumn.Table).Select(Function(x) $"[{x.DBName}]").Contains(lsColumnName) Then
                        lsColumnName = "NULL"
                    End If
                    lasColumnNamesFinal.Add(lsColumnName)
                Next

                Dim lsColumnList = String.Join(",", lasColumnNamesFinal.Select(Function(col) col))

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arColumn.Table, arColumn.Table.DBName & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO [" & arColumn.Table.DBName & "_temp] SELECT " & lsColumnList & " FROM [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arColumn.Table.DBName & "_temp] RENAME TO [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Returns True if a Column with the given name exists in the database for the Column's Table, else returns False.
        ''' </summary>
        ''' <param name="asTableName">The Table testing for</param>
        ''' <param name="asColumnName">The Column testing for</param>
        ''' <returns></returns>
        Public Overrides Function ColumnExists(ByVal asTableName As String, ByVal asColumnName As String) As Boolean

            Try
                Return Me.getColumnsByTable(New RDS.Table(Nothing, asTableName, Nothing)).FindAll(Function(x) x.Name = asColumnName).Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        ''' <summary>
        ''' Sets whether the specified Column is mandatory or not, in the database.
        ''' </summary>
        ''' <param name="arColumn">The Column to have its schema definition changed.</param>
        ''' <param name="abIsMandatory">True if the Column is mandatory for its Table.</param>
        Public Overrides Sub columnSetMandatory(ByRef arColumn As RDS.Column,
                                                  ByVal abIsMandatory As Boolean)
            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrColumn As RDS.Column = arColumn
                Dim lasColumnNamesFinal As New List(Of String)

                Dim lasColumnNames = (From Column In arColumn.Table.Column
                                      Select $"[{Column.DBName}]").ToList

                For Each lsColumnName In lasColumnNames.ToArray
                    If Not Me.getColumnsByTable(arColumn.Table).Select(Function(x) $"[{x.DBName}]").Contains(lsColumnName) Then
                        lsColumnName = "NULL"
                    End If
                    lasColumnNamesFinal.Add(lsColumnName)
                Next

                Dim lsColumnList = String.Join(",", lasColumnNamesFinal)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arColumn.Table, arColumn.Table.DBName & "_temp", Not arColumn.IsMandatory) ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO [" & arColumn.Table.DBName & "_temp] SELECT " & lsColumnList & " FROM [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arColumn.Table.DBName & "_temp] RENAME TO [" & arColumn.Table.DBName & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub CommitTrans()

            Try
                Exit Sub '20231029-VM-Testing without.
                If _activeTransactions.Count = 0 Then
                    Throw New InvalidOperationException("There are no active transactions to commit.")
                End If

                Dim transaction = _activeTransactions(_activeTransactions.Count - 1)
                _activeTransactions.RemoveAt(_activeTransactions.Count - 1)
                transaction.Commit()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)
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
                lsSQLCommand = "CREATE TABLE [" & arTable.Name & "]"
                lsSQLCommand &= " ("
                lsSQLCommand &= arColumn.Name & " " & "TEXT(100)" '& arColumn.DataTypeName
                lsSQLCommand &= ")"

                Me.GONonQuery(lsSQLCommand)

            Catch ex As Exception

            End Try


        End Sub

        Public Overrides Function CreateTableInstance(ByVal jsonString As String, ByRef asErrorMessage As String) As Boolean

            Try

                ' Parse the JSON string
                Dim jsonObject As JObject = JObject.Parse(jsonString)

                ' Determine the table name and columns/values
                Dim tableName As String = Nothing
                Dim columns As New List(Of String)
                Dim values As New List(Of String)
                Dim parameters As New List(Of SQLiteParameter)

                ' Assuming there's only one table name key in the JSON
                Dim lrTable As RDS.Table = Nothing
                For Each prop As KeyValuePair(Of String, JToken) In jsonObject
                    tableName = "[" & prop.Key & "]"

                    lrTable = Me.FBMModel.RDS.Table.Find(Function(x) x.DBName = prop.Key)
                    If lrTable Is Nothing Then
                        Return False
                    End If

                    Dim columnData As JObject = prop.Value

                    For Each column As KeyValuePair(Of String, JToken) In columnData

                        Dim lrColumn As RDS.Column = lrTable.Column.Find(Function(x) x.DBName = column.Key)

                        If lrColumn IsNot Nothing Then
                            columns.Add(column.Key)
                            Dim lsColumnValue As String = "<Error>"
                            Select Case lrColumn.getMetamodelDataType
                                'Case Is = pcenumORMDataType.AutoUUID
                                '    lsColumnValue = System.Guid.NewGuid.ToString
                                Case Else
                                    lsColumnValue = column.Value.ToString
                            End Select
                            values.Add(Me.DataTypeWrapper(lrColumn.getMetamodelDataType) & lsColumnValue & Me.DataTypeWrapper(lrColumn.getMetamodelDataType))
                        End If
                    Next
                Next

                ' Construct the query
                Dim lsQuery As String = $"INSERT INTO {tableName} ({String.Join(", ", columns)}) VALUES ({String.Join(", ", values)})"

                Dim lrRecordset = Me.GONonQuery(lsQuery)

                If lrRecordset.ErrorReturned Then
                    asErrorMessage = lrRecordset.ErrorString
                End If

                If lrRecordset.ErrorReturned Then
                    If {"UNIQUE constraint failed", "NOT NULL constraint failed"}.Any(Function(substring) lrRecordset.ErrorString.Contains(substring)) Then
                        lsQuery = $"UPDATE {tableName} SET " '({String.Join(", ", columns)}) VALUES ({String.Join(", ", values)})"                        
                        Dim lsWhereClause = " WHERE "
                        Dim liInd = 0
                        For Each lrColumn In lrTable.getPrimaryKeyColumns
                            liInd = columns.IndexOf(lrColumn.DBName)
                            lsWhereClause.AppendLine(columns(liInd) & " = " & values(liInd))
                            columns.RemoveAt(liInd)
                            values.RemoveAt(liInd)
                        Next
                        liInd = 0
                        For Each lsColumnName In columns
                            If liInd > 0 Then lsQuery &= ","
                            lsQuery.AppendLine(columns(liInd) & " = " & values(liInd))
                            liInd += 1
                        Next
                        lsQuery.AppendLine(lsWhereClause)
                        lrRecordset = Me.GONonQuery(lsQuery)

                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Public Overrides Function DataTypeWrapper(ByVal aiDataType As pcenumORMDataType) As String
            Try
                Select Case aiDataType
                    Case Is = pcenumORMDataType.TextFixedLength,
                              pcenumORMDataType.TextLargeLength,
                              pcenumORMDataType.TextVariableLength,
                              pcenumORMDataType.AutoUUID
                        Return "'"
                    Case Is = pcenumORMDataType.TemporalDate,
                              pcenumORMDataType.TemporalDateAndTime
                        Return "'"
                    Case Else
                        Return ""
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, True)

                Return ""
            End Try

        End Function

        Public Overrides Function DateTimeFormat() As String
            Return "yyyy-MM-dd HH:mm:ss"
        End Function

        ''' <summary>
        ''' Deletes a Table Instance (Row in a Table) given JSON in the format: {"Order" : {"Order_Id":123, "Customer_Id":456}}
        ''' </summary>
        ''' <param name="jsonString"></param>
        Public Overrides Function DeleteTableInstance(ByVal jsonString As String) As Boolean

            Try
                ' Parse the JSON string
                Dim jsonObject As JObject = JObject.Parse(jsonString)

                ' Determine the table name and columns/values
                Dim tableName As String = Nothing
                Dim columns As New List(Of String)
                Dim values As New List(Of String)
                Dim parameters As New List(Of SQLiteParameter)

                ' Parse the JSON string
                ' Determine the table name
                Dim lrTable As RDS.Table = Nothing
                Dim whereConditions As New List(Of String)

                For Each prop As KeyValuePair(Of String, JToken) In jsonObject
                    tableName = "[" & prop.Key & "]"
                    lrTable = Me.FBMModel.RDS.Table.Find(Function(x) x.DBName = prop.Key)
                    If lrTable Is Nothing Then
                        Return False
                    End If

                    ' Construct the WHERE clause                    
                    Dim columnData As JObject = prop.Value

                    For Each column As KeyValuePair(Of String, JToken) In columnData
                        Dim columnName As String = column.Key
                        Dim columnValue As String = column.Value.ToString()
                        Dim lrColumn As RDS.Column = lrTable.Column.Find(Function(x) x.DBName = columnName)
                        If lrColumn IsNot Nothing Then
                            columnValue = $"{Me.DataTypeWrapper(lrColumn.getMetamodelDataType)}{columnValue}{Me.DataTypeWrapper(lrColumn.getMetamodelDataType)}"
                            Dim condition As String = $"{columnName} = {columnValue}"
                            whereConditions.Add(condition)
                        End If
                    Next
                Next

                ' Construct the query
                Dim lsQuery As String = $"DELETE FROM {tableName} WHERE {String.Join(" AND ", whereConditions)}"

                Dim lrRecordset = Me.GONonQuery(lsQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Overloads Function Execute(ByVal asQuery As String, Optional ByVal abIgnoreErrors As Boolean = False) As Recordset

            Try
                Return Me.GONonQuery(asQuery)
            Catch ex As Exception
                If abIgnoreErrors Then Return New ORMQL.Recordset
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Function


        Public Overrides Function FormatDate(ByVal asOriginalDate As String,
                                                 Optional ByVal abIgnoreError As Boolean = False) As String

            Try

                'Dim lsPattern As String = "yyyy-MM-dd"

                'Return Convert.ToDateTime(asOriginalDate, System.Threading.Thread.CurrentThread.CurrentUICulture).ToString(lsPattern)

                Dim lsPattern As String = "yyyy-MM-dd"
                Dim datePatterns() As String = {
                            "dd/MM/yyyy",
                            "M/d/yyyy h:mm:ss tt",
                            "M/d/yyyy h:mm tt",
                            "M/d/yyyy HH:mm:ss",
                            "M/d/yyyy",
                            "MM/dd/yyyy",
                            "yyyy-MM-dd",
                            "yyyy/MM/dd"
                }

                Dim parsedDate As DateTime

                ' Attempt to parse the date string using the array of patterns
                For Each pattern In datePatterns
                    If DateTime.TryParseExact(asOriginalDate, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, parsedDate) Then
                        Return parsedDate.ToString(lsPattern)
                    End If
                Next

                ' If no pattern matched and error handling is not ignored
                If Not abIgnoreError Then
                    Throw New FormatException("Date format not recognized.")
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        Public Overrides Function FormatDateTime(ByVal asOriginalDate As String,
                                                 Optional ByVal abIgnoreError As Boolean = False,
                                                 Optional ByVal abJustDate As Boolean = False) As String

            Try


                Dim lsPattern As String = "yyyy-MM-dd HH:mm:ss"

                Try
                    Return Convert.ToDateTime(asOriginalDate, System.Threading.Thread.CurrentThread.CurrentUICulture).ToString(lsPattern)
                Catch ex As Exception
                    Return ""
                End Try


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        Public Overrides Function generateSQLColumnDefinition(ByRef arColumn As RDS.Column,
                                                              Optional abIgnoreColumnISNOTNULL As Boolean = False) As String
            Try

                Dim lsSQLColumnDefinition As String
                Dim lrColumn As RDS.Column = arColumn

                'CodeSafe
#Region "Code Safe: Mandatory Role Constraint"
                If Not (arColumn.IsMandatory = arColumn.Role.Mandatory) Then
                    'Default to the Column
                    If Not arColumn.Table.FBMModelElement Is arColumn.Role.FactType Then
                        arColumn.Role.Mandatory = arColumn.IsMandatory
                        arColumn.Role.makeDirty()
                    End If
                End If
#End Region


                lsSQLColumnDefinition = $"[{arColumn.DBName}]"

                If arColumn.ActiveRole.FactType.RoleGroup.Count = 1 Then
                    lsSQLColumnDefinition &= " INTEGER"
                Else
                    lsSQLColumnDefinition &= " " & arColumn.ActiveRole.JoinsValueType.DBDataType
                    If arColumn.ActiveRole.JoinsValueType.DataTypeLength > 0 Then
                        If arColumn.DataTypeIsText Then
                            lsSQLColumnDefinition &= "(" & arColumn.ActiveRole.JoinsValueType.DataTypeLength.ToString
                            If arColumn.ActiveRole.JoinsValueType.DataTypePrecision > 0 Then
                                lsSQLColumnDefinition &= arColumn.ActiveRole.JoinsValueType.DataTypePrecision.ToString
                            End If
                            lsSQLColumnDefinition &= ")"
                        End If
                    End If

                    Dim larOutgoingRelation = arColumn.Relation.FindAll(Function(x) x.OriginTable Is lrColumn.Table)
                    If larOutgoingRelation.Count > 0 Then
                        If larOutgoingRelation(0).OriginColumns.Count = 1 Then
                            lsSQLColumnDefinition &= " REFERENCES [" & larOutgoingRelation(0).DestinationTable.DBName & "]"
                        End If
                        If arColumn.Role.Mandatory And Not abIgnoreColumnISNOTNULL Then lsSQLColumnDefinition &= " NOT NULL"
                    ElseIf arColumn.Role.Mandatory And Not abIgnoreColumnISNOTNULL Then
                        lsSQLColumnDefinition &= " NOT NULL"
                    End If

                End If

                Return lsSQLColumnDefinition

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Generates a CREATE TABLE Statement for the given Table, specific to the database type.
        ''' </summary>
        ''' <param name="arTable">The RDS Table for which the SQL CREATE statement is to be generated.</param>
        ''' <param name="asTableName">Optional table name for the table in the CREATE statement.</param>
        ''' <returns></returns>
        Public Overrides Function generateCREATETABLEStatement(ByRef arTable As RDS.Table,
                                                               Optional asTableName As String = Nothing,
                                                               Optional abIgnoreColumnISNOTNULL As Boolean = False) As String

            Try
                Dim lsSQLCommand As String = ""

                If asTableName Is Nothing Then
                    lsSQLCommand = "CREATE TABLE [" & arTable.DBName & "]"
                Else
                    lsSQLCommand = "CREATE TABLE [" & asTableName & "]"
                End If
                lsSQLCommand &= " ("
                'Column defs
                Dim liInd = 0
                For Each lrColumn In arTable.Column
                    If liInd > 0 Then lsSQLCommand &= ","
                    lsSQLCommand &= Me.generateSQLColumnDefinition(lrColumn, abIgnoreColumnISNOTNULL) & vbCrLf
                    liInd += 1
                Next
                'Primary Key
                If arTable.getPrimaryKeyColumns.Count > 0 Then
                    lsSQLCommand &= ", CONSTRAINT [" & arTable.Name.RemoveWhitespace & "_PK] PRIMARY KEY ("
                    liInd = 0
                    For Each lrColumn In arTable.getPrimaryKeyColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.DBName & "]"
                        liInd += 1
                    Next
                    lsSQLCommand &= ")"
                    If arTable.isPGSRelation Then
                        lsSQLCommand &= " /* { Label:""" & arTable.DBName & """} */"
                    End If
                    lsSQLCommand &= vbCrLf
                End If
                'Unique Indexes
                If arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey).Count > 0 Then
                    liInd = 1
                    For Each lrIndex In arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey)
                        lsSQLCommand &= ", CONSTRAINT " & lrIndex.Name & " UNIQUE ("
                        Dim liInd2 = 0
                        For Each lrColumn In lrIndex.Column
                            If liInd2 > 0 Then lsSQLCommand &= ","
                            lsSQLCommand &= "[" & lrColumn.Name & "]"
                            liInd2 += 1
                        Next
                        lsSQLCommand &= ")" & vbCrLf
                    Next
                End If
                'Foreign Keys
                For Each lrRelation In arTable.getOutgoingRelations '.FindAll(Function(x) x.OriginColumns.Count > 1)

                    'CodeSafe: Make sure OriginColumns count = DestinationColumns count
#Region "CodeSafe"
                    If lrRelation.OriginColumns.Count <> lrRelation.DestinationColumns.Count Then
                        Call lrRelation.Model.Model.FixErrors(New List(Of pcenumModelFixType) From {pcenumModelFixType.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount},
                                                              lrRelation)
                    End If
#End Region

                    lsSQLCommand &= ", FOREIGN KEY ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.DBName & "]"
                        liInd += 1
                    Next
                    lsSQLCommand &= ") REFERENCES [" & lrRelation.DestinationTable.DBName & "] ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.getReferencedColumn.DBName & "]"
                        liInd += 1
                    Next

                    'Graph Label for Relational Knowledge Graph/Graph Database procesing/queries
                    Dim lsGraphLabel = ""
                    Try
                        lsGraphLabel = lrRelation.ResponsibleFactType.PropertyGraphLabel
                    Catch ex As Exception
                        'No loss
                    End Try

                    lsSQLCommand &= ")"
                    lsSQLCommand &= " ON DELETE CASCADE ON UPDATE CASCADE"
                    lsSQLCommand &= " /* { Label:""" & lsGraphLabel & """} */" & vbCrLf
                    lsSQLCommand &= vbCrLf
                Next
                lsSQLCommand &= ")"

                Return lsSQLCommand
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        Public Overrides Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)

            Dim larRelation As New List(Of RDS.Relation)
            Try
                Dim lsSQL As String
                Dim lrRecordset As ORMQL.Recordset

                lsSQL = "PRAGMA foreign_key_list(" & arTable.Name & ")"

                lrRecordset = Me.GO(lsSQL)

                Dim lrRelation As RDS.Relation = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lrOriginColumn As RDS.Column = Nothing
                Dim lrDestinationColumn As RDS.Column = Nothing
                Dim lasToTableNames As New List(Of String)

                While Not lrRecordset.EOF
                    'https://stackoverflow.com/questions/48508140/how-do-i-get-information-about-foreign-keys-in-sqlite
                    'Columns
                    '====================
                    'id
                    'seq (0 based by table)
                    'table  (To Table)
                    'from   (Column)
                    'to     (To Column)
                    'on_update  ('NO ACTION')
                    'on_delete
                    'match
                    lrDestinationTable = Me.FBMModel.RDS.getTableByName(lrRecordset("table").Data)

                    While Not lrRecordset.EOF

                        lrOriginColumn = arTable.Column.Find(Function(x) x.Name = lrRecordset("from").Data)
                        lrDestinationColumn = lrDestinationTable.Column.Find(Function(x) x.Name = lrRecordset("to").Data)
                        If lrDestinationColumn Is Nothing Then
                            'Try and find the DestinationColumn another way.
                            If lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey) IsNot Nothing Then
                                If lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey).Column.Count = 1 Then
                                    lrDestinationColumn = lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey).Column.First
                                Else
                                    Throw New Exception("Foreign key from Table, '" & arTable.Name & "', to table, '" & lrDestinationTable.Name & "', has a Column that can not be found in the referenced table. Try making the Column in '" & lrDestinationTable.Name & "' match those in table, " & arTable.Name)
                                End If
                            End If

                        End If

                        If Not lasToTableNames.Contains(lrRecordset("table").Data) Then

                            lrRelation = New RDS.Relation(System.Guid.NewGuid.ToString,
                                                  arTable,
                                                  pcenumCMMLMultiplicity.Many,
                                                  True,
                                                  lrOriginColumn.isPartOfPrimaryKey,
                                                  "involves",
                                                  lrDestinationTable,
                                                  pcenumCMMLMultiplicity.One,
                                                  lrDestinationColumn.IsMandatory,
                                                  "is involed in",
                                                  Nothing)
                            larRelation.Add(lrRelation)
                        End If

                        lrOriginColumn.Relation.Add(lrRelation)
                        lrRelation.OriginColumns.Add(lrOriginColumn)
                        lrRelation.DestinationColumns.Add(lrDestinationColumn)

                        lrRecordset.MoveNext()

                        If Not lrRecordset.EOF Then
                            If lrRecordset("table").Data <> lrDestinationTable.Name Then
                                lrRecordset.CurrentFactIndex -= 1
                                Exit While
                            End If
                            lasToTableNames.AddUnique(lrDestinationTable.Name)
                        End If

                        If lrDestinationTable.getPrimaryKeyColumns.Count = 1 Then
                            lasToTableNames.Remove(lrDestinationTable.Name)
                        End If

                    End While

                    lrRecordset.MoveNext()
                End While


                Return larRelation

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & arTable.Name & ":" & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message & ex.StackTrace
                'prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                'Return New List(Of RDS.Relation)
                Throw New Exception(lsMessage)
            End Try
        End Function

        Public Overrides Function getColumnsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)
            Try
                Dim lsSQL As String = "PRAGMA table_info('" & arTable.DBName & "')"
                Dim lrRecordset As ORMQL.Recordset = Me.GO(lsSQL)

                Dim lsColumnName As String
                Dim lbIsMandatory As Boolean
                Dim lrColumn As New RDS.Column

                While Not lrRecordset.EOF
                    'name
                    'type
                    'notnull
                    'dflt_value
                    'pk  (0 if not part of the PK, else integer value)
                    lsColumnName = lrRecordset("name").Data
                    lbIsMandatory = CInt(NullVal(lrRecordset("notnull").Data, 0)) > 0
                    lrColumn = New RDS.Column(arTable, lsColumnName, Nothing, Nothing, lbIsMandatory)
                    lrColumn.DataType = New RDS.DataType
                    lrColumn.DataType.DataType = lrRecordset("type").Data
                    lrColumn.DatabaseName = lrColumn.Name

                    larColumn.Add(lrColumn)

                    lrRecordset.MoveNext()
                End While

                Return larColumn

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        Public Overrides Sub getDatabaseDataTypes()

            Try
                Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                Me.FBMModel.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function GetDatabaseName(connectionString As String) As String
            ' Split the connection string to find the "Data Source" part
            Dim parts As String() = connectionString.Split(";"c)

            For Each part As String In parts
                If part.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase) Then
                    ' Extract the database file path
                    Dim dataSource As String = part.Substring(12).Trim()
                    ' Get the file name without the path and extension
                    Dim dbName As String = System.IO.Path.GetFileNameWithoutExtension(dataSource)
                    Return dbName
                End If
            Next

            ' If no "Data Source" is found, return an empty string or handle it accordingly
            Return String.Empty

        End Function

        Public Overrides Function getBostonDataTypeByDatabaseDataType(ByVal asDatabaseDataType As String) As pcenumORMDataType

            Try
                asDatabaseDataType = Trim(asDatabaseDataType)
                Dim liIndex As Integer = asDatabaseDataType.IndexOf("(")
                If (liIndex > 0) Then
                    asDatabaseDataType = asDatabaseDataType.Substring(0, liIndex)
                End If

                Dim larDBDataType = From DatabaseDataType In Me.FBMModel.RDS.DatabaseDataType
                                    Where UCase(DatabaseDataType.DataType) = UCase(asDatabaseDataType)
                                    Where Me.FBMModel.TargetDatabaseType.ToString = DatabaseDataType.Database.ToString
                                    Select DatabaseDataType.BostonDataType


                If larDBDataType.Count > 0 Then
                    Return larDBDataType.First
                Else
                    Return pcenumORMDataType.TextVariableLength
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return pcenumORMDataType.TextVariableLength
            End Try

        End Function

        ''' <summary>
        ''' Returns a list of the Indexes in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getIndexesByTable(ByRef arTable As RDS.Table) As List(Of RDS.Index)

            Dim larIndex As New List(Of RDS.Index)
            Try
                Dim lsSQL As String = "SELECT DISTINCT m.name AS 'table', ii.name AS 'column', ii.cid + 1 AS 'sequencenr', il.*
                                         FROM sqlite_master AS m,
                                              pragma_index_list(m.name) AS il,
                                              pragma_index_info(il.name) AS ii
                                        WHERE m.type = 'table'
                                          AND m.name = '" & arTable.Name & "'
                                        ORDER BY 1;"

                Dim lrRecordset As ORMQL.Recordset = Me.GO(lsSQL)

                If lrRecordset.EOF Then
                    lsSQL = "SELECT '" & arTable.Name & "_PK' as 'name', l.name as 'column', 1 as 'unique', 'pk' as 'origin', 1 as 'partial' FROM pragma_table_info('" & arTable.Name & "') as l WHERE l.pk = 1;"
                    lrRecordset = Me.GO(lsSQL)
                End If

                Dim lsIndexName As String = ""
                Dim lbIsUnique As Boolean = False
                Dim lbIsPrimaryKey As Boolean = False
                Dim lrIndex As New RDS.Index
                Dim lsQualifier As String = ""
                Dim liIndexSequence As Integer = 1
                Dim lbIgnoreNulls As Boolean = False
                Dim lsColumnName As String = ""
                Dim larColumn As New List(Of RDS.Column)
                Dim lrColumn As RDS.Column = Nothing

                Dim lasIndexNames As New List(Of String)

                While Not lrRecordset.EOF
                    'table
                    'column
                    'sequencenr (of Column in Index)
                    'seq (sequence of Index in Table, I think)
                    'name
                    'unique (1 if unique)
                    'origin (pk if PrimaryKey)
                    'partial
                    larColumn.Clear()

                    While Not lrRecordset.EOF

                        lsIndexName = lrRecordset("name").Data
                        lbIsUnique = CInt(NullVal(lrRecordset("unique").Data, 0)) > 0
                        If lrRecordset("origin").Data = "pk" Then
                            lsQualifier = "PK"
                            lbIsPrimaryKey = True
                        Else
                            lsQualifier = "UC"
                            lbIsPrimaryKey = False
                        End If

                        liIndexSequence = CInt(NullVal(lrRecordset("unique").Data, 1))
                        lbIgnoreNulls = CInt(NullVal(lrRecordset("partial").Data, 0)) = 0
                        lsColumnName = lrRecordset("column").Data

                        lrColumn = arTable.Column.Find(Function(x) x.Name = lsColumnName)
                        larColumn.Add(lrColumn)

                        lasIndexNames.AddUnique(lsIndexName)

                        lrRecordset.MoveNext()

                        If Not lrRecordset.EOF Then
                            If lrRecordset("name").Data <> lsIndexName Then
                                lasIndexNames.AddUnique(lsIndexName)
                                lrRecordset.CurrentFactIndex -= 1
                                Exit While
                            End If
                        End If

                    End While

                    lrIndex = New RDS.Index(arTable,
                                            lsIndexName,
                                            lsQualifier,
                                            pcenumODBCAscendingOrDescending.Ascending,
                                            lbIsPrimaryKey,
                                            lbIsUnique,
                                            lbIgnoreNulls,
                                            larColumn,
                                            True,
                                            False,
                                            True)

                    larIndex.Add(lrIndex)

                    lrRecordset.MoveNext()
                End While

                Return larIndex

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Index)
            End Try

        End Function

        ''' <summary>
        ''' Gets PK Index by other means if primary GetIndexesByTable doesn't return PK Indexes.
        '''   E.g. In SQLite you can create a Table with a PK and without an Index.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getIndexesByTableByAlternateMeans(ByRef arTable As RDS.Table) As List(Of RDS.Index)

            Dim larIndex As New List(Of RDS.Index)

            Try
                Dim lsSQL As String = "SELECT * FROM pragma_table_info('" & arTable.Name & "') as l WHERE l.pk = 1;"

                Dim lrRecordset As ORMQL.Recordset = Me.GO(lsSQL)

                Dim lsIndexName As String = ""
                Dim lbIsUnique As Boolean = False
                Dim lbIsPrimaryKey As Boolean = False
                Dim lrIndex As New RDS.Index
                Dim lsQualifier As String = ""
                Dim liIndexSequence As Integer = 1
                Dim lbIgnoreNulls As Boolean = False
                Dim lsColumnName As String = ""
                Dim larColumn As New List(Of RDS.Column)
                Dim lrColumn As RDS.Column = Nothing

                Dim lasIndexNames As New List(Of String)

                While Not lrRecordset.EOF
                    'name (name of the Column)
                    'type
                    'notnull 
                    'dflt value
                    'pk (will be 1)
                    larColumn.Clear()

                    While Not lrRecordset.EOF

                        lsIndexName = arTable.Name & "_PK"
                        lbIsUnique = CInt(NullVal(lrRecordset("notnull").Data, 0)) > 0
                        lsQualifier = "PK"
                        lbIsPrimaryKey = True
                        liIndexSequence = 1
                        lbIgnoreNulls = CInt(NullVal(lrRecordset("notnull").Data, 0)) = 0
                        lsColumnName = lrRecordset("name").Data

                        lrColumn = arTable.Column.Find(Function(x) x.Name = lsColumnName)
                        larColumn.Add(lrColumn)

                        lrRecordset.MoveNext()
                    End While

                    lrIndex = New RDS.Index(arTable,
                                            lsIndexName,
                                            lsQualifier,
                                            pcenumODBCAscendingOrDescending.Ascending,
                                            lbIsPrimaryKey,
                                            lbIsUnique,
                                            lbIgnoreNulls,
                                            larColumn,
                                            True,
                                            False,
                                            True)

                    larIndex.Add(lrIndex)

                    lrRecordset.MoveNext()
                End While

                Return larIndex
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Index)
            End Try
        End Function

        Public Overrides Function GetNodeDetails(ByVal asTableName As String, ByVal aasIdentiferList As List(Of String), ByVal abUseUniqueIndex As Boolean) As List(Of KeyValuePair)

            Dim larKeyValuePair As New List(Of KeyValuePair)

            Try
                Dim lsSQLQuery As String = ""

                If abUseUniqueIndex Then

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery.AppendLine("FROM " & asTableName)
                    lsSQLQuery.AppendLine("WHERE ")

                    Dim lrIndex As RDS.Index = Me.getIndexesByTable(New RDS.Table(Nothing, asTableName, Nothing)).Find(Function(x) x.Unique)

                    Dim liInd = 0
                    For Each lrColumn In lrIndex.Column
                        If liInd > 0 Then lsSQLQuery &= vbCrLf & "AND "
                        lsSQLQuery.AppendString(lrColumn.Name & aasIdentiferList(liInd))
                        liInd += 1
                    Next

                    Dim lrRecordset As ORMQL.Recordset

                    lrRecordset = Me.GO(lsSQLQuery)

                    For Each lsColumnName In lrRecordset.ColumnNames
                        Dim lrKeyValuePair As New KeyValuePair(lsColumnName, lrRecordset(lsColumnName).Data)
                        larKeyValuePair.Add(lrKeyValuePair)
                    Next

                Else
                    'Use PrimaryKey
                End If

                Return larKeyValuePair

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return New List(Of KeyValuePair)
            End Try

        End Function

        Public Overrides Function getRelationsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)

            Dim larRelation As New List(Of RDS.Relation)

            Try
                Return larRelation
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Relation)
            End Try

        End Function

        Public Overrides Function getTableRowCount(ByRef arTable As RDS.Table) As Integer

            Try
                Dim lsSQLQuery As String = "SELECT COUNT(*) AS RowCount FROM " & arTable.DatabaseName

                Dim lrRecordset = Me.GO(lsSQLQuery)

                Return lrRecordset("RowCount").Data

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getTables() As List(Of RDS.Table)

            Dim larTable As New List(Of RDS.Table)
            Try
                Dim lsSQL As String = "SELECT name FROM sqlite_master WHERE type='table'"
                Dim lrRecordset As ORMQL.Recordset = Me.GO(lsSQL)

                If lrRecordset.ErrorReturned Then
                    Throw New Exception(lrRecordset.ErrorString)
                End If

                Dim lsTableName As String
                Dim lrTable As New RDS.Table

                While Not lrRecordset.EOF

                    lsTableName = lrRecordset("name").Data
                    lrTable = New RDS.Table(Me.FBMModel.RDS, lsTableName, Nothing)

                    larTable.Add(lrTable)

                    lrRecordset.MoveNext()
                End While

                Return larTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Table)
            End Try


        End Function


        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                'CodeSafe: ExecuteNonQuery if not a SELECT statement.
                Dim firstWord As String = asQuery.Trim().Split(" "c)(0).ToUpper()

                Select Case firstWord
                    Case "SELECT"
                        'Nothing to do here
                    Case "INSERT", "UPDATE", "DELETE", "CREATE", "DROP", "ALTER", "PRAGMA"
                        Call Me.GONonQuery(asQuery)
                    Case "BEGIN", "COMMIT", "ROLLBACK"
                        Call Me.GONonQuery(asQuery)
                    Case Else
                        'Handle unknown or unsupported commands
                End Select


                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Boston.WriteToStatusBar("Connecting To database.", True)
                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                lrSQLiteConnection.EnableExtensions(True)
                lrSQLiteConnection.LoadExtension("SQLite.Interop.dll", "sqlite3_fts5_init") ' "sqlite3_json_init")

                If lrSQLiteConnection Is Nothing Then
                    Throw New Exception("SQLite Adaptor: Could not create SQLite database connection to execute the query.")
                End If

                Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, asQuery)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact

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
                    lrRecordset.ColumnNames.Add(lsColumnName)
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
                            Case Is = GetType(DateTime)
                                Try
                                    loFieldValue = lrSQLiteDataReader.GetDateTime(liInd).ToString(Me.DateTimeFormat)
                                Catch ex As Exception
                                    Try
                                        loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                                    Catch ex1 As Exception
                                        'Sometimes DateTime values are not in the correct format. None the less they are stored in SQLite.
                                        loFieldValue = lrSQLiteDataReader.GetString(liInd)
                                    End Try
                                End Try
                            Case Else
                                Try
                                    loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                                Catch ex As Exception
                                    'Sometimes DateTime values are not in the correct format. None the less they are stored in SQLite.
                                    loFieldValue = lrSQLiteDataReader.GetString(liInd)
                                End Try

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
                lrRecordset.Reset()


                'If Me.Connection Is Nothing Then 'Was screwing up saving/committing to the db file
                lrSQLiteConnection.Close()
                'End If



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

            Dim lrSQLiteConnection As Data.SQLite.SQLiteConnection = Nothing
            Dim transaction As System.Data.SQLite.SQLiteTransaction = Nothing

            Try
                lrRecordset.Query = asSQLQuery

                lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                If lrSQLiteConnection.State <> ConnectionState.Open Then
                    lrSQLiteConnection.Open()
                End If

                If lrSQLiteConnection Is Nothing Then
                    Throw New Exception("SQLite Adaptor: Could not create SQLite database connection to execute the query.")
                End If

                Dim liIterationSequenceNr = 1

                ' Enable foreign key constraints
                Using pragmaCommand As SQLiteCommand = New System.Data.SQLite.SQLiteCommand(lrSQLiteConnection)
                    pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;"
                    pragmaCommand.ExecuteNonQuery()
                End Using


ExecuteTheQuery:
                Using cmd As SQLiteCommand = New System.Data.SQLite.SQLiteCommand(lrSQLiteConnection)
                    ' Set PRAGMA foreign_keys = ON within the transaction
                    cmd.CommandText = asSQLQuery '20231202-VM-Was "PRAGMA foreign_keys = ON;" & asSQLQuery
                    cmd.Prepare()

                    Try
                        result = cmd.ExecuteNonQuery()
                    Catch SQLiteException As System.Data.SQLite.SQLiteException
#Region "SQLite Exception"
                        If SQLiteException.Message.Contains("FOREIGN KEY constraint failed") Then
                            'Not using Transactions with SQLite because will have the _Connection always open, which stops subsequent writes,
                            '  and too difficult, at this stage, to rewrite Boston to maintain Transaction state.
                            Try
                                Using pragmaCommand As SQLiteCommand = New System.Data.SQLite.SQLiteCommand(lrSQLiteConnection)
                                    pragmaCommand.CommandText = "PRAGMA foreign_keys = OFF;"
                                    pragmaCommand.ExecuteNonQuery()
                                End Using

                                GoTo ExecuteTheQuery
                            Catch SQLiteException2 As System.Data.SQLite.SQLiteException
                                Throw New Exception(SQLiteException2.Message)
                            End Try
                        Else
                            Throw New Exception(SQLiteException.Message)
                        End If
#End Region
                    End Try
                End Using

                Using pragmaCommand As SQLiteCommand = New System.Data.SQLite.SQLiteCommand(lrSQLiteConnection)
                    pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;"
                    pragmaCommand.ExecuteNonQuery()

#Region "Integrity Check"
                    Dim lsFKIntegrityResultFail As String = ""
                    Using command As New SQLiteCommand("PRAGMA foreign_key_check;", lrSQLiteConnection)
                        Using reader As SQLiteDataReader = command.ExecuteReader()
                            If Not reader.HasRows Then
                                lsFKIntegrityResultFail = "No foreign key violations found."
                            Else
                                '20250214-Was returning rows unrelated to query.
                                'While reader.Read()
                                '    lsFKIntegrityResultFail &= $"Table: {reader.GetString(0)}, Row ID: {reader.GetInt64(1)}, Parent Table: {reader.GetString(2)}, Foreign Key Index: {reader.GetInt32(3)}" & vbNewLine
                                'End While

                                'Throw New Exception(lsFKIntegrityResultFail)
                            End If
                        End Using
                    End Using
                    ' Verify that foreign keys are enabled
                    pragmaCommand.CommandText = "PRAGMA foreign_keys;"
                    Dim loPragmaResult As Object = pragmaCommand.ExecuteScalar()

                    If loPragmaResult = 0 Then
#Region "Foreign Keys not turned on"
                        pragmaCommand.CommandText = "PRAGMA integrity_check;"
                        loPragmaResult = pragmaCommand.ExecuteScalar()

                        If loPragmaResult <> "ok" Then
                            Throw New Exception(loPragmaResult)
                        End If

                        'pragmaCommand.CommandText = "PRAGMA foreign_key_list('Booking');"
                        'loPragmaResult = pragmaCommand.ExecuteScalar()
                        Using command As New SQLiteCommand("PRAGMA foreign_key_check;", lrSQLiteConnection)
                            Using reader As SQLiteDataReader = command.ExecuteReader()
                                If Not reader.HasRows Then
                                    lsFKIntegrityResultFail = "No foreign key violations found."
                                Else
                                    While reader.Read()
                                        lsFKIntegrityResultFail &= $"Table: {reader.GetString(0)}, Row ID: {reader.GetInt64(1)}, Parent Table: {reader.GetString(2)}, Foreign Key Index: {reader.GetInt32(3)}" & vbNewLine
                                    End While
                                End If
                            End Using
                        End Using
#End Region
                        Throw New Exception("Failed to enable foreign key constraints.".AppendDoubleLineBreak(lsFKIntegrityResultFail))
                    End If
#End Region
                End Using


                Return lrRecordset

            Catch ex As Exception
                ' Roll back the transaction if an exception occurs
                If transaction IsNot Nothing Then
                    transaction.Rollback()
                End If

                lrRecordset.ErrorString = ex.Message

                If My.Settings.DebugMode = "Debug" Then
                    Call prApplication.ThrowMessage(ex.Message.AppendDoubleLineBreak(asSQLQuery), pcenumErrorType.Critical, ex.StackTrace, False, False, True, , True, ex, False)
                    Call Me.RollbackTrans()
                End If

                Return lrRecordset

            Finally
                ' Close the connection
                If lrSQLiteConnection IsNot Nothing Then
                    lrSQLiteConnection.Close()
                End If
            End Try


        End Function

        ''' <summary>
        ''' Adds the nominated Column to the nominated Index.
        ''' </summary>
        ''' <param name="arIndex">The Index to add the nominated Column to.</param>
        ''' <param name="arColumn">The Column to add to the nominated Index.</param>
        Public Overrides Sub IndexAddColumn(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)

            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrIndex As RDS.Index = arIndex
                Dim lasColumnNames = From Column In lrIndex.Table.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()

#Region "Special Indexes"
                            'When an Index in SQLite has a Column that may be NULL, need to add another special index.                            
                            Dim larIndexWithNullableColumn = From Index In lrIndex.Table.Index
                                                             From Column In Index.Column
                                                             Where Not Column.IsMandatory
                                                             Select Index

                            Dim liInd = 1
                            For Each lrIndex In larIndexWithNullableColumn
                                Dim lsSQLCommand As String = ""

                                Dim larMandatoryColumn = lrIndex.Column.FindAll(Function(x) x.IsMandatory)
                                'CodeSafe
                                If larMandatoryColumn.Count = 0 Then GoTo NextIndexWithNullableColum

                                cmd.CommandText = "DROP INDEX uc_" & arIndex.Table.Name & "special" & liInd.ToString
                                Try
                                    cmd.ExecuteNonQuery()
                                Catch ex As Exception
                                    'Tried
                                End Try

                                lsSQLCommand = "CREATE UNIQUE INDEX uc_" & lrIndex.Table.Name & "special" & liInd.ToString & " ON " & lrIndex.Table.Name & "_temp" & "("
                                Dim liInd2 = 0
                                For Each lrColumn In lrIndex.Column.FindAll(Function(x) x.IsMandatory)
                                    If liInd2 > 0 Then lsSQLCommand &= ","
                                    lsSQLCommand &= "[" & lrColumn.Name & "]"
                                    liInd2 += 1
                                Next
                                lsSQLCommand &= ")" & vbCrLf
                                liInd2 = 0
                                lsSQLCommand &= " WHERE "
                                For Each lrColumn In larMandatoryColumn
                                    If liInd2 > 0 Then
                                        lsSQLCommand &= ", "
                                    End If
                                    lsSQLCommand &= lrColumn.Name
                                    liInd2 += 1
                                Next
                                lsSQLCommand &= " IS NULL"
                                cmd.CommandText = lsSQLCommand
                                cmd.ExecuteNonQuery()
                                liInd += 1
NextIndexWithNullableColum:
                            Next
#End Region

                            cmd.CommandText = "INSERT INTO " & arIndex.Table.Name & "_temp SELECT " & lsColumnList & " FROM [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE " & arIndex.Table.Name & "_temp RENAME TO [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Updates the Index in the database. E.g. Changing a Unique Index to Primary Key.
        ''' </summary>
        ''' <param name="arIndex">The Index to be updated.</param>
        Public Overrides Sub IndexUpdate(ByRef arIndex As RDS.Index)

            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrIndex As RDS.Index = arIndex
                Dim lasColumnNames = From Column In lrIndex.Table.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO [" & arIndex.Table.Name & "_temp] SELECT " & lsColumnList & " FROM [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arIndex.Table.Name & "_temp] RENAME TO [" & arIndex.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function Open(Optional ByVal asDatabaseConnectionString As String = Nothing) As Boolean

            Try
                If asDatabaseConnectionString IsNot Nothing Then
                    Me.DatabaseConnectionString = asDatabaseConnectionString
                End If

                If Me.DatabaseConnectionString Is Nothing Then
                    Throw New Exception("Must have Database Connection String for the database")
                End If

                Try
                    If Me.Connection IsNot Nothing Then
                        Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                        Me.Connected = True 'Connections are actually made for each Query.
                        Me._Connection = lrSQLiteConnection
                        Me.State = 1
                    End If
                Catch ex As Exception
                    Me.Connected = False
                    Throw New Exception("Could not connect to the database. Check the Model Configuration's Connection String.")
                End Try

                'Me.Connection.EnableExtensions(True)
                'Me.Connection.LoadExtension("SQLite.Interop.dll", "sqlite3_fts5_init") ' "sqlite3_json_init")

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Public Overrides Function ParseDDL(ByVal asDDL As String) As List(Of RDS.Table)

            Dim larTable As New List(Of RDS.Table)()
            Dim ddlStatements() As String = asDDL.Split(New String() {";"}, StringSplitOptions.RemoveEmptyEntries)

            For Each statement As String In ddlStatements

                Dim lrTable As RDS.Table = ParseTableDefinition(larTable, statement)
                If lrTable IsNot Nothing Then
                    larTable.Add(lrTable)
                End If

            Next

            Return larTable
        End Function

        Private Function ParseTableDefinition(ByRef aarTable As List(Of RDS.Table), ByVal asDDL As String) As RDS.Table

            Dim lrTable As New RDS.Table

            Dim tablePattern As String = "CREATE\s+TABLE\s+\[([^\]]+)\]"
            Dim tableName As String = String.Empty
            Dim columns As New List(Of RDS.Column)()
            Dim primaryKeys As New List(Of String)()
            Dim foreignKeys As New List(Of RDS.Relation)()

            Dim match As Match = Regex.Match(asDDL, tablePattern, RegexOptions.IgnoreCase)

            If match.Success Then
                tableName = match.Groups(1).Value.Trim()
                Dim columnStatements() As String = asDDL.Substring(match.Length).Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

                For Each columnStatement As String In columnStatements
                    Dim columnDefinition As RDS.Column = ParseColumnDefinition(columnStatement)
                    If columnDefinition IsNot Nothing Then
                        columns.Add(columnDefinition)
                    End If

                    Dim primaryKey As String = ParsePrimaryKey(columnStatement)
                    If primaryKey IsNot Nothing Then
                        primaryKeys.Add(primaryKey)
                    End If

                    Dim foreignKey As RDS.Relation = ParseForeignKey(lrTable, aarTable, columnStatement)
                    If foreignKey IsNot Nothing Then
                        foreignKeys.Add(foreignKey)
                    End If
                Next
            End If

            If tableName <> String.Empty AndAlso columns.Count > 0 Then
                lrTable.Name = tableName
                lrTable.Column = columns
                'tableDefinition.PrimaryKeys = primaryKeys
                'tableDefinition.ForeignKeys = foreignKeys
                Return lrTable
            End If

            Return Nothing
        End Function

        Private Function ParseColumnDefinition(ByVal columnStatement As String) As RDS.Column

            Dim columnPattern As String = "\s*([^\s]+)\s+([^\s]+)\s*.*"

            Dim match As Match = Regex.Match(columnStatement, columnPattern, RegexOptions.IgnoreCase)

            If match.Success Then
                Dim columnName As String = match.Groups(1).Value.Trim()
                Dim dataType As String = match.Groups(2).Value.Trim()

                Dim lrColumn As New RDS.Column
                lrColumn.Name = columnName
                lrColumn._DBDataType = dataType

                Return lrColumn
            End If

            Return Nothing
        End Function

        Private Function ParsePrimaryKey(ByVal columnStatement As String) As String

            Dim primaryKeyPattern As String = "PRIMARY KEY\s*\((.+)\)"
            Dim match As Match = Regex.Match(columnStatement, primaryKeyPattern, RegexOptions.IgnoreCase)

            If match.Success Then
                Return match.Groups(1).Value.Trim()
            End If

            Return Nothing

        End Function

        Private Function ParseForeignKey(ByRef arOriginTable As RDS.Table, ByRef aarTable As List(Of RDS.Table), ByVal columnStatement As String) As RDS.Relation

            Dim foreignKeyPattern As String = "FOREIGN KEY\s*\((.+)\)\s*REFERENCES\s*([^\(]+)\s*\((.+)\)"
            Dim match As Match = Regex.Match(columnStatement, foreignKeyPattern, RegexOptions.IgnoreCase)

            If match.Success Then
                Dim columnName As String = match.Groups(1).Value.Trim()
                Dim referencedTable As String = match.Groups(2).Value.Trim()
                Dim referencedColumn As String = match.Groups(3).Value.Trim()

                Dim foreignKeyDefinition As New RDS.Relation

                foreignKeyDefinition.OriginTable = arOriginTable
                'foreignKeyDefinition.ColumnName = columnName
                foreignKeyDefinition.DestinationTable = aarTable.Find(Function(x) x.Name = referencedTable)
                'foreignKeyDefinition.ReferencedColumn = referencedColumn

                Return foreignKeyDefinition
            End If

            Return Nothing

        End Function


        ''' <summary>
        ''' Creates or Recreates the Table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overrides Sub recreateTable(ByRef arTable As RDS.Table)

            Try

                Dim lsSQL As String

                'CodeSafe
                arTable.Column = arTable.Column.OrderBy(Function(x) x.OrdinalPosition).ToList

                Dim lasColumnNames = (From Column In arTable.Column
                                      Select $"[{Column.DBName}]").ToList

                Dim lasColumnNamesFinal As New List(Of String)
                For Each lsColumnName In lasColumnNames.ToArray
                    If Not Me.getColumnsByTable(arTable).Select(Function(x) $"[{x.DBName}]").Contains(lsColumnName) Then
                        lsColumnName = "NULL"
                    End If
                    lasColumnNamesFinal.Add(lsColumnName)
                Next

                Dim lsColumnList = String.Join(",", lasColumnNamesFinal)

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            Try
                                cmd.Transaction = tr
                                cmd.CommandText = "DROP TABLE IF EXISTS [" & arTable.DBName & "_temp]" 'Temp Table. Drop.
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = Me.generateCREATETABLEStatement(arTable, arTable.DBName & "_temp")
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = $"INSERT INTO [{arTable.DBName}_temp] SELECT {lsColumnList} FROM [{arTable.DBName}]"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "DROP TABLE [" & arTable.DBName & "]"
                                cmd.ExecuteNonQuery()
                                cmd.CommandText = "ALTER TABLE [" & arTable.DBName & "_temp] RENAME TO [" & arTable.DBName & "]"
                                cmd.ExecuteNonQuery()
                            Catch ex As Exception
                                Boston.ShowFlashCard(ex.Message, Color.Salmon, 4000)
                                cmd.CommandText = Me.generateCREATETABLEStatement(arTable, arTable.DBName)
                                cmd.ExecuteNonQuery()
                            End Try

                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try

                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Register a custom function with database
        ''' </summary>
        Public Overrides Sub RegisterCustomFunctions()

            ' Register the custom function using the provided delegate
            SQLiteFunction.RegisterFunction(GetType(VectorDB.HammingDistanceFunction))
        End Sub

        ''' <summary>
        ''' Removes the Column from its Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overrides Sub removeColumn(ByRef arColumn As RDS.Column)

            Try
                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrColumn As RDS.Column = arColumn
                Dim lasColumnNames = From Column In lrColumn.Table.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateCREATETABLEStatement(arColumn.Table, arColumn.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO [" & arColumn.Table.Name & "_temp] SELECT " & lsColumnList & " FROM [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE [" & arColumn.Table.Name & "_temp] RENAME TO [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                        End Using

                        tr.Commit()
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        tr.Rollback()
                    End Try
                End Using

                lsSQL = "PRAGMA foreign_keys=ON"
                Me.GONonQuery(lsSQL)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        ''' <summary>
        ''' Removes/Drops the Table from the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overrides Sub removeTable(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLCommand = "DROP TABLE [" & arTable.Name & "]"

                Me.GONonQuery(lsSQLCommand)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Renames the given Column to the new column name.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="asNewColumnName"></param>
        Public Overrides Sub renameColumn(ByRef arColumn As RDS.Column, ByVal asNewColumnName As String)

            Try
                Dim lsSQLCommmand = "ALTER TABLE [" & arColumn.Table.Name & "]"
                lsSQLCommmand &= " RENAME COLUMN [" & arColumn.Name & "] TO " & "[" & asNewColumnName & "]"

                Me.GONonQuery(lsSQLCommmand)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Renames a table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <param name="asNewName"></param>
        Public Overrides Sub RenameTable(ByRef arTable As RDS.Table, ByVal asNewName As String)

            Try
                Dim lsSQLCommmand = "ALTER TABLE [" & arTable.Name & "]"
                lsSQLCommmand &= " RENAME TO [" & asNewName & "]"

                Me.GONonQuery(lsSQLCommmand)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Overrides Sub RollbackTrans()

            Try
                If _activeTransactions.Count = 0 Then
                    Throw New InvalidOperationException("There are no active transactions to rollback.")
                End If

                Dim transaction = _activeTransactions(_activeTransactions.Count - 1)
                _activeTransactions.RemoveAt(_activeTransactions.Count - 1)
                transaction.Rollback()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Returns True if a Table with the given name exists in the database, else returns False.
        ''' </summary>
        ''' <param name="asTableName"></param>
        ''' <returns></returns>
        Public Overrides Function TableExists(ByVal asTableName As String) As Boolean

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "SELECT * FROM sqlite_master"
                lsSQLQuery &= " WHERE Type ='table'"
                lsSQLQuery &= " AND name ='" & asTableName & "'"

                Dim lrRecordset As ORMQL.Recordset

                lrRecordset = Me.GO(lsSQLQuery)

                Return lrRecordset.Facts.Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Updates the value of a Column in the database.
        ''' </summary>
        ''' <param name="asTableName">The name of the Table for which the Attribute/Column value is to be updated.</param>
        ''' <param name="arColumn">The Column/Attribute for which the value is to be updated.</param>
        ''' <param name="asNewValue">The new value for the Attribute/Column.</param>
        ''' <param name="aarPKColumn">A list of the Primary Key Columns/Attributes for the record to be updated. TemporaryValue of Column is existing/old value of the Primary Key Column/Attribute.</param>
        Public Overrides Function UpdateAttributeValue(ByVal asTableName As String,
                                                       ByVal arColumn As RDS.Column,
                                                       ByVal asNewValue As String,
                                                       ByVal aarPKColumn As List(Of RDS.Column)) As ORMQL.Recordset

            Dim lsSQLQuery As String

            lsSQLQuery = "UPDATE " & asTableName & vbCrLf
            lsSQLQuery &= " SET " & arColumn.Name & " = "
            lsSQLQuery &= Boston.returnIfTrue(arColumn.DataTypeIsTextOrDate, "'", "")
            lsSQLQuery &= asNewValue
            lsSQLQuery &= Boston.returnIfTrue(arColumn.DataTypeIsTextOrDate, "'", "") & vbCrLf
            lsSQLQuery &= " WHERE "
            Dim liInd = 0
            For Each lrColumn In aarPKColumn
                lsSQLQuery &= Boston.returnIfTrue(liInd > 0, " AND ", "")
                lsSQLQuery &= lrColumn.Name & " = "
                lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsTextOrDate, "'", "")
                lsSQLQuery &= lrColumn.TemporaryData
                lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsTextOrDate, "'", "") & vbCrLf
                liInd += 1
            Next

            Dim lrRecordset = Me.GONonQuery(lsSQLQuery)

            Return lrRecordset

        End Function

        Public Overrides Function UpdateTableInstance(ByVal jsonString As String) As Boolean

            Try

                ' Parse the JSON string
                Dim jsonObject As JObject = JObject.Parse(jsonString)

                ' Determine the table name and columns/values
                Dim tableName As String = Nothing
                Dim columns As New List(Of String)
                Dim values As New List(Of String)
                Dim parameters As New List(Of SQLiteParameter)

                ' Assuming there's only one table name key in the JSON
                Dim lrTable As RDS.Table = Nothing
                For Each prop As KeyValuePair(Of String, JToken) In jsonObject
                    tableName = "[" & prop.Key & "]"

                    lrTable = Me.FBMModel.RDS.Table.Find(Function(x) x.DBName = prop.Key)
                    If lrTable Is Nothing Then
                        Return False
                    End If

                    Dim columnData As JObject = prop.Value

                    For Each column As KeyValuePair(Of String, JToken) In columnData

                        Dim lrColumn As RDS.Column = lrTable.Column.Find(Function(x) x.DBName = column.Key)

                        If lrColumn IsNot Nothing Then
                            columns.Add(column.Key)
                            Dim lsColumnValue As String = "<Error>"
                            Select Case lrColumn.getMetamodelDataType
                                'Case Is = pcenumORMDataType.AutoUUID
                                '    lsColumnValue = System.Guid.NewGuid.ToString
                                Case Else
                                    lsColumnValue = column.Value.ToString
                            End Select
                            values.Add(Me.DataTypeWrapper(lrColumn.getMetamodelDataType) & lsColumnValue & Me.DataTypeWrapper(lrColumn.getMetamodelDataType))
                        End If
                    Next
                Next

                Dim lsQuery As String

                lsQuery = $"UPDATE {tableName} SET " '({String.Join(", ", columns)}) VALUES ({String.Join(", ", values)})"                        
                Dim lsWhereClause = " WHERE "
                Dim liInd = 0
                For Each lrColumn In lrTable.getPrimaryKeyColumns
                    liInd = columns.IndexOf(lrColumn.DBName)
                    lsWhereClause.AppendLine(columns(liInd) & " = " & values(liInd))
                    columns.RemoveAt(liInd)
                    values.RemoveAt(liInd)
                Next
                liInd = 0
                For Each lsColumnName In columns
                    If liInd > 0 Then lsQuery &= ","
                    lsQuery.AppendLine(columns(liInd) & " = " & values(liInd))
                    liInd += 1
                Next
                lsQuery.AppendLine(lsWhereClause)
                Dim lrRecordset = Me.GONonQuery(lsQuery)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Private Function iDatabaseConnection_GOAsync(asQuery As String) As Task(Of Recordset) Implements iDatabaseConnection.GOAsync
            Throw New NotImplementedException()
        End Function

        Private Function iDatabaseConnection_GOAbstractionLayer(asQuery As String) As Recordset Implements iDatabaseConnection.GOAbstractionLayer
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
