Imports Boston.ORMQL
Imports System.Data.SQLite
Imports System.Reflection

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
                lsSQLCommand = "ALTER TABLE [" & arColumn.Table.Name & "]"
                lsSQLCommand &= " ADD COLUMN "
                lsSQLCommand &= Me.generateSQLColumnDefinition(arColumn)

                Me.GONonQuery(lsSQLCommand)

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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arRelation.OriginTable, arRelation.OriginTable.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arRelation.OriginTable.Name & "_temp SELECT " & lsColumnList & " FROM [" & arRelation.OriginTable.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arRelation.OriginTable.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE " & arRelation.OriginTable.Name & "_temp RENAME TO [" & arRelation.OriginTable.Name & "]"
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                Dim lasColumnNames = From Column In lrColumn.Table.Column
                                     Select Column.Name
                Dim lsColumnList = String.Join(",", lasColumnNames)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)
                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arColumn.Table, arColumn.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_temp SELECT " & lsColumnList & " FROM [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE " & arColumn.Table.Name & "_temp RENAME TO [" & arColumn.Table.Name & "]"
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arColumn.Table, arColumn.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_temp SELECT " & lsColumnList & " FROM [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE " & arColumn.Table.Name & "_temp RENAME TO [" & arColumn.Table.Name & "]"
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

        Public Overrides Function FormatDateTime(ByVal asOriginalDate As String) As String

            Try

                Dim lsPattern As String = "yyyy-MM-dd HH:mm:ss"

                Return Convert.ToDateTime(asOriginalDate, System.Threading.Thread.CurrentThread.CurrentUICulture).ToString(lsPattern)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        Public Overrides Function generateSQLColumnDefinition(ByRef arColumn As RDS.Column) As String
            Try

                Dim lsSQLColumnDefinition As String
                Dim lrColumn As RDS.Column = arColumn


                lsSQLColumnDefinition = arColumn.Name
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
                        lsSQLColumnDefinition &= " REFERENCES [" & larOutgoingRelation(0).DestinationTable.Name & "]"
                    End If
                    If arColumn.Role.Mandatory Then lsSQLColumnDefinition &= " NOT NULL"
                ElseIf arColumn.Role.Mandatory Then
                    lsSQLColumnDefinition &= " NOT NULL"
                End If

                Return lsSQLColumnDefinition

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Generates a CREATE TABLE Statement for the given Table, specific to the database type.
        ''' </summary>
        ''' <param name="arTable">The RDS Table for which the SQL CREATE statement is to be generated.</param>
        ''' <param name="asTableName">Optional table name for the table in the CREATE statement.</param>
        ''' <returns></returns>
        Public Overrides Function generateSQLCREATETABLEStatement(ByRef arTable As RDS.Table,
                                                                  Optional asTableName As String = Nothing) As String

            Try
                Dim lsSQLCommand As String

                If asTableName Is Nothing Then
                    lsSQLCommand = "CREATE TABLE [" & arTable.Name & "]"
                Else
                    lsSQLCommand = "CREATE TABLE [" & asTableName & "]"
                End If
                lsSQLCommand &= " ("
                'Column defs
                Dim liInd = 0
                For Each lrColumn In arTable.Column
                    If liInd > 0 Then lsSQLCommand &= ","
                    lsSQLCommand &= Me.generateSQLColumnDefinition(lrColumn) & vbCrLf
                    liInd += 1
                Next
                'Primary Key
                If arTable.getPrimaryKeyColumns.Count > 0 Then
                    lsSQLCommand &= ", CONSTRAINT " & arTable.Name & "_PK PRIMARY KEY ("
                    liInd = 0
                    For Each lrColumn In arTable.getPrimaryKeyColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= lrColumn.Name
                        liInd += 1
                    Next
                    lsSQLCommand &= ")" & vbCrLf
                End If
                'Unique Indexes
                If arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey).Count > 0 Then
                    liInd = 1
                    For Each lrIndex In arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey)
                        lsSQLCommand &= ", CONSTRAINT " & lrIndex.Name & " UNIQUE ("
                        Dim liInd2 = 0
                        For Each lrColumn In lrIndex.Column
                            If liInd2 > 0 Then lsSQLCommand &= ","
                            lsSQLCommand &= lrColumn.Name
                            liInd2 += 1
                        Next
                        lsSQLCommand &= ")" & vbCrLf
                    Next
                End If
                'Foreign Keys
                For Each lrRelation In arTable.getOutgoingRelations '.FindAll(Function(x) x.OriginColumns.Count > 1)
                    lsSQLCommand &= ", FOREIGN KEY ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= lrColumn.Name
                        liInd += 1
                    Next
                    lsSQLCommand &= ") REFERENCES [" & lrRelation.DestinationTable.Name & "] ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= lrColumn.getReferencedColumn.Name
                        liInd += 1
                    Next
                    lsSQLCommand &= ")"
                    lsSQLCommand &= " ON DELETE CASCADE ON UPDATE CASCADE" & vbCrLf
                Next
                lsSQLCommand &= ")"

                Return lsSQLCommand
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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

                    If lrDestinationTable Is Nothing Then debugger.break
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
                                lasToTableNames.AddUnique(lrDestinationTable.Name)
                                lrRecordset.CurrentFactIndex -= 1
                                Exit While
                            End If
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
                'prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                'Return New List(Of RDS.Relation)
                Throw New Exception(lsMessage)
            End Try
        End Function

        Public Overrides Function getColumnsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)
            Try
                Dim lsSQL As String = "PRAGMA table_info('" & arTable.Name & "')"
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        Public Overrides Sub getDatabaseTypes()

            Try
                Dim lsPath = Richmond.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                Me.FBMModel.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Index)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Relation)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Table)
            End Try


        End Function



        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting To database.", True)
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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arIndex.Table, arIndex.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Creates or Recreates the Table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overrides Sub recreateTable(ByRef arTable As RDS.Table)

            Try

                Dim lsSQL As String

                lsSQL = "PRAGMA foreign_keys=OFF"
                Me.GONonQuery(lsSQL)

                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)

                Using tr As SQLiteTransaction = lrSQLiteConnection.BeginTransaction()

                    Try
                        Using cmd As SQLiteCommand = lrSQLiteConnection.CreateCommand()
                            cmd.Transaction = tr
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arTable, arTable.Name)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
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
                            cmd.CommandText = Me.generateSQLCREATETABLEStatement(arColumn.Table, arColumn.Table.Name & "_temp") ''"CREATE TEMPORARY TABLE " & arColumn.Table.Name & "_backup (" & lsColumnDefinitions & ")"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "INSERT INTO " & arColumn.Table.Name & "_temp SELECT " & lsColumnList & " FROM [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "DROP TABLE [" & arColumn.Table.Name & "]"
                            cmd.ExecuteNonQuery()
                            cmd.CommandText = "ALTER TABLE " & arColumn.Table.Name & "_temp RENAME TO [" & arColumn.Table.Name & "]"
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                lsSQLCommmand &= " RENAME COLUMN " & arColumn.Name & " TO " & asNewColumnName

                Me.GONonQuery(lsSQLCommmand)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
            lsSQLQuery &= " SET " & arColumn.Name & " = " & vbCrLf
            lsSQLQuery &= Richmond.returnIfTrue(arColumn.DataTypeIsText, "'", "")
            lsSQLQuery &= asNewValue
            lsSQLQuery &= Richmond.returnIfTrue(arColumn.DataTypeIsText, "'", "") & vbCrLf
            lsSQLQuery &= " WHERE "
            For Each lrColumn In aarPKColumn
                lsSQLQuery &= lrColumn.Name & " = "
                lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                lsSQLQuery &= lrColumn.TemporaryData
                lsSQLQuery &= Richmond.returnIfTrue(lrColumn.DataTypeIsText, "'", "") & vbCrLf
            Next

            Dim lrRecordset = Me.GO(lsSQLQuery)

            Return lrRecordset

        End Function

    End Class

End Namespace
