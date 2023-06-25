Imports Boston.ORMQL
Imports System.Data.SQLite
Imports System.Reflection
Imports System.Threading.Tasks
Imports kuzunet
Imports System.Text.RegularExpressions

Namespace FactEngine

    Public Class KuzuDBConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Public Shadows DatabaseConnectionString As String = ""

        Private db As kuzu_database
        Private conn As kuzu_connection

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer,
                       Optional ByVal abCreatingNewDatabase As Boolean = False)

            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString
            Me.DefaultQueryLimit = aiDefaultQueryLimit

            'Connection String: Idea to use
            'Data Source=Data Source:<Path to Folder for KuzuDB>;User Id=admin;Password=admin;            

            If abCreatingNewDatabase Then Exit Sub

            Try
                Dim lrSQLConnectionStringBuilder As New System.Data.SqlClient.SqlConnectionStringBuilder(asDatabaseConnectionString)
                lrSQLConnectionStringBuilder.ConnectionString = asDatabaseConnectionString

                Dim lsDBFolder As String = ""
                Dim lsUsername As String = ""
                Dim lsPassword As String = ""

                lsDBFolder = lrSQLConnectionStringBuilder("Data Source")
                lsUsername = lrSQLConnectionStringBuilder.UserID
                lsPassword = lrSQLConnectionStringBuilder.Password

                Me.db = kuzu_database_init(lsDBFolder, 0)
                Me.conn = kuzu_connection_init(db)

                Me.Connected = True 'Connections are actually made for each Query.                

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
            Dim lrRecordset As ORMQL.Recordset
            Try
                lsSQLCommand = "ALTER TABLE " & arColumn.Table.Name
                lsSQLCommand &= " ADD "
                lsSQLCommand &= Me.generateSQLColumnDefinition(arColumn)

                lrRecordset = Me.GONonQuery(lsSQLCommand)

                If lrRecordset.ErrorString IsNot Nothing Then
                    Throw New Exception(lrRecordset.ErrorString)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Adds the referenced Index to the database. Table is within Index definition.
        ''' </summary>
        ''' <param name="arIndex">The Index to be added to the database.</param>
        Public Overrides Sub addIndex(ByRef arIndex As RDS.Index)

            Try
                Dim lsSQL As String = ""


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
                Dim lsSQL As String = ""
                Dim lrRecordset As ORMQL.Recordset

                Try
                    lsSQL = "ALTER TABLE `" & arColumn.Table.Name & "` DROP " & arColumn.Name

                    lrRecordset = Me.GONonQuery(lsSQL)

                    If lrRecordset.ErrorString IsNot Nothing Then
                        Throw New Exception(lrRecordset.ErrorString)
                    End If
                Catch ex As Exception
                    'Not a biggie. Didn't want the Column anyway.
                End Try


                Dim lrColumn = arColumn
                Call Me.addColumn(arColumn)

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
                Dim lsSQL As String = ""


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
            Dim lrTable As RDS.Table = arTable
            Try
                Dim larColumn = From Column In lrTable.Column
                                Where Not Column.isPartOfPrimaryKey
                                Select Column

                For Each lrColumn In larColumn
                    '=========E.g. From Kuzu website==========================================================
                    '// create the schema
                    'connection.query("CREATE NODE TABLE User(name STRING, age INT64, PRIMARY KEY (name))");
                    '=========================================================================================
                    lsSQLCommand = "CREATE NODE TABLE `" & arTable.Name & "`"
                    lsSQLCommand &= " ("
                    lsSQLCommand &= lrColumn.Name & " " & lrColumn.DBDataType & If(lrColumn.Index.Count > 0, ", PRIMARY KEY (" & lrColumn.Name & ")", "")
                    lsSQLCommand &= ")"

                    Me.GONonQuery(lsSQLCommand)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        Public Overrides Function DataTypeWrapper(ByVal aiDataType As pcenumORMDataType) As String
            Try
                Select Case aiDataType
                    Case Is = pcenumORMDataType.TextFixedLength,
                              pcenumORMDataType.TextLargeLength,
                              pcenumORMDataType.TextVariableLength
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True, False, True)

                Return ""
            End Try

        End Function

        Public Overrides Function DateTimeFormat() As String
            Return "yyyy-MM-dd HH:mm:ss"
        End Function

        Public Overrides Function FormatDateTime(ByVal asOriginalDate As String,
                                                 Optional ByVal abIgnoreError As Boolean = False,
                                                 Optional ByVal abJustDate As Boolean = False) As String

            Try
                Dim lsPattern As String

                If abJustDate Then
                    lsPattern = "yyyy-MM-dd"
                Else
                    lsPattern = "yyyy-MM-dd HH:mm:ss"
                End If

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

                '20230623-VM-ToDo: Check if KuzuDB has Length and/or Precision
                'If arColumn.ActiveRole.JoinsValueType.DataTypeLength > 0 Then
                '    If arColumn.DataTypeIsText Then
                '        lsSQLColumnDefinition &= "(" & arColumn.ActiveRole.JoinsValueType.DataTypeLength.ToString
                '        If arColumn.ActiveRole.JoinsValueType.DataTypePrecision > 0 Then
                '            lsSQLColumnDefinition &= arColumn.ActiveRole.JoinsValueType.DataTypePrecision.ToString
                '        End If
                '        lsSQLColumnDefinition &= ")"
                '    End If
                'End If

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
                Dim loColumns = kuzu_connection_get_node_property_names(Me.conn, arTable.Name)
                Dim lasColumnDescription As List(Of String) = loColumns.Split(vbLf).ToList.Select(Function(s) s.Replace(vbTab, "")).ToList
                lasColumnDescription.RemoveAt(0)
                lasColumnDescription.RemoveAt(lasColumnDescription.Count - 1)

                '=========NB=============
                'Dim input As String = "Hello" & vbCrLf & "World" & vbTab & "Test"
                'Dim separators As String() = {vbCrLf, vbTab}
                'Dim result As String() = input.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                '=========NB=============

                Dim lsColumnName As String
                Dim lsDataType As String
                Dim lbIsMandatory As Boolean
                Dim lbIsPrimaryKey As Boolean
                Dim lrColumn As New RDS.Column

                For Each lsColumnDescription In lasColumnDescription

                    '==================================
                    'E.g. For/From Kuzu database: kuzu_connection_get_node_property_names
                    '"name STRING(PRIMARY KEY)"
                    '"population INT64"
                    '==================================

                    Dim regexPattern As String = "^(.*?)\s+(\w+)(?:\s*\((PRIMARY KEY)\))?$"

                    Dim regex As New Regex(regexPattern)

                    'Extracting information from the description
                    Dim match1 As Match = regex.Match(lsColumnDescription)

                    If match1.Success Then
                        lsColumnName = match1.Groups(1).Value.Trim()
                        lsDataType = match1.Groups(2).Value.Trim()
                        lbIsPrimaryKey = match1.Groups(3).Success
                        lbIsMandatory = lbIsPrimaryKey

                        lrColumn = New RDS.Column(arTable, lsColumnName, Nothing, Nothing, lbIsMandatory)
                        lrColumn.DataType = New RDS.DataType
                        lrColumn.DataType.DataType = lsDataType
                        lrColumn.DatabaseName = lrColumn.Name

                        larColumn.Add(lrColumn)
                    Else
                        GoTo SkipColumn
                    End If

SkipColumn:
                Next

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
                Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
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

                Dim lsTableNames = kuzu_connection_get_node_table_names(Me.conn)
                Dim lasTableNames As List(Of String) = lsTableNames.Split(vbLf).ToList.Select(Function(s) s.Replace(vbTab, "")).ToList
                lasTableNames.RemoveAt(0)
                lasTableNames.RemoveAt(lasTableNames.Count - 1)

                Dim lsTableName As String
                Dim lrTable As New RDS.Table

                For Each lsTableName In lasTableNames

                    lrTable = New RDS.Table(Me.FBMModel.RDS, lsTableName, Nothing)
                    larTable.Add(lrTable)
                Next

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

        Public Overrides Async Function GOAsync(asQuery As String) As Threading.Tasks.Task(Of ORMQL.Recordset) Implements iDatabaseConnection.GOAsync

            Throw New NotImplementedException
            Return Nothing

        End Function

        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact

#Region "New Code"

                '=====================================================
                'Works for: MATCH (p:Person)-[r:ACTED_IN]-(m:Movie {title:"The Matrix"}) RETURN p.name, m.title
                'NB Kuzu is case sensitive.

                Dim loResult As kuzu_query_result
                Dim lsErrorMessage As String = Nothing

                Try
                    loResult = kuzu_connection_query(Me.conn, asQuery)

                    lsErrorMessage = kuzu_query_result_get_error_message(loResult)
                Catch ex As Exception
                    Throw New Exception(ex.Message.AppendDoubleLineBreak(lsErrorMessage))
                End Try

                If lsErrorMessage IsNot Nothing Then

                    Throw New Exception(lsErrorMessage)
                    '    Try
                    '        Dim preparedStatement As kuzu_prepared_statement = kuzu_connection_prepare(Me.conn, asQuery)

                    '        loResult = kuzu_connection_execute(Me.conn, preparedStatement)
                    '    Catch ave As AccessViolationException
                    '        Throw New Exception("Access violation trying to run: " & asQuery)
                    '    Catch ex As Exception

                    '    End Try

                End If

#Region "Notes"
                '                Hey, I want to ask about what are the default values of the prepared statements. The queries work when we dont bind them values.
                'output:
                '                [RESULT] 1
                'code:
                '                Int main()
                '{
                '    kuzu_database * db = kuzu_database_init("test_me" /* fill db path */, 0);
                '    kuzu_connection * connection = kuzu_connection_init(db);

                '    kuzu_query_result * result = kuzu_connection_query(
                '        connection,
                '        "CREATE NODE TABLE Person(name STRING, age INT64, isStudent BOOLEAN, PRIMARY KEY(name));");

                '    If (result == NULL) Then
                '                        {
                '        printf("[NULL]: CREATE NODE TABLE\n");
                '        Return 1;
                '    }

                '/* ---------------- HERE -------------
                '    Char * query = "MATCH (a:Person) WHERE a.isStudent = $1 AND a.age > $2 RETURN COUNT(*)";

                '    kuzu_prepared_statement * preparedStatement = kuzu_connection_prepare(connection, query);

                '   ---------------- HERE ------------- */
                '    If (query == NULL) Then
                '                            {
                '        printf("[NULL]: MATCH (a:Person) WHERE\n");
                '        Return 1;
                '    }

                '    kuzu_query_result * c_result = kuzu_connection_execute(connection, preparedStatement);

                '    If (c_result == NULL) Then
                '                                {
                '        printf("[NULL] kuzu_connection_execute\n");
                '        Return 1;
                '    }

                '    printf("[RESULT] %d\n", kuzu_query_result_is_success(c_result));

                '    Return 0;
                '}
                '(edited)

                '1 reply

                'Xiyang
                '5 days ago
                'Hi Margerette, the default value of a parameter Is NULL . So the query Is executed as MATCH (a: Person) WHERE a.isStudent = NULL And a.age > NULL RETURN COUNT(*) which leads to success execution but empty output
#End Region

                ' Just peek to check if any value available.
                ' Getting count here will consume all the records
                ' Use loResult.ToList() if you need all the record in separate list
                If kuzu_query_result_has_next(loResult) Then

                    Dim liFieldCount As Integer = kuzu_query_result_get_num_columns(loResult)

                    '==================================================================
                    'Column Names   
                    Dim lsColumnName As String
                    For liInd = 0 To liFieldCount - 1
                        lsColumnName = kuzu_query_result_get_column_name(loResult, liInd)
                        '                        lsColumnName = lrFactType.CreateUniqueRoleName(loResult(0).Keys(liFieldInd), 0)
                        Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                        lrFactType.RoleGroup.AddUnique(lrRole)
                        lrRecordset.Columns.Add(lsColumnName)
                    Next
                    '==================================================================

                    ' looping each record will consume the record from list
                    ' consumed record will be removed from the list
                    While (kuzu_query_result_has_next(loResult))

                        lrFact = New FBM.Fact(lrFactType, False)
                        Dim loFieldValue As Object = Nothing
                        Dim liInd As Integer

                        '===================================
                        'Create a Tuple for the Result/Row
                        Dim tuple As kuzu_flat_tuple = kuzu_query_result_get_next(loResult)

                        For liInd = 0 To liFieldCount - 1

                            Dim value As kuzu_value = kuzu_flat_tuple_get_value(tuple, liInd)

                            If value Is Nothing Then
                                loFieldValue = "NULL"
                                GoTo AddFactData
                            End If

                            Select Case kuzu_data_type_get_id(kuzu_value_get_data_type(value)).ToString

                                Case Is = "KUZU_NODE"

                                    Dim loNodeValue As kuzu_node_val = kuzu_value_get_node_val(value)

                                    loFieldValue = kuzu_node_val_get_id(loNodeValue)
                                    loFieldValue = kuzu_node_val_to_string(loNodeValue)

                                Case Is = "KUZU_STRING"

                                    Dim lsTempValue = kuzu_value_get_string(value)

                                    If Not NullVal(lsTempValue, "") = "" Then
                                        loFieldValue = lsTempValue
                                    Else
                                        loFieldValue = ""
                                    End If
                                Case Is = "KUZU_INT16"

                                    Dim llTempValue As Long = kuzu_value_get_int16(value)
                                    loFieldValue = llTempValue
                                Case Is = "KUZU_INT64"

                                    Dim llTempValue As Long = kuzu_value_get_int64(value)
                                    loFieldValue = llTempValue

                                    '============================================
                                    'Case TypeOf loFieldValue Is Neo4j.Driver.IRelationship
                                    '    ' cast the object to relationship interface
                                    '    Dim loRelationship As Neo4j.Driver.IRelationship = loFieldValue
                                    '    ' find the relational nodes
                                    '    Dim lsFirst = lrRecord.Values.First(Function(x) x.Value.ElementId = loRelationship.StartNodeId).Value.Labels(0) ' StartNodeElementId
                                    '    Dim lsLast = lrRecord.Values.First(Function(x) x.Value.ElementId = loRelationship.EndNodeId).Value.Labels(0) 'EndNodeElementId
                                    '    ' build relationship string
                                    '    loFieldValue = $"{lsFirst}-{loRelationship.Type}->{lsLast}"
                                    '    Exit Select

                                    'Case TypeOf loFieldValue Is INode
                                    '    ' cast the object to Node interface
                                    '    Dim loNode As INode = loFieldValue
                                    '    loFieldValue = loNode.ToString()
                                    '    Exit Select

                                Case Else
                                    '    Try
                                    '        loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                                    '    Catch ex As Exception
                                    '        'Sometimes DateTime values are not in the correct format. None the less they are stored in SQLite.
                                    '        loFieldValue = lrSQLiteDataReader.GetString(liInd)
                                    '    End Try
                                    loFieldValue = value.ToString
                            End Select

                            ' check 
AddFactData:
                            Try
                                lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(Viev.NullVal(loFieldValue, "")), lrFact))
                                '=====================================================
                            Catch ex As Exception
                                Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                            End Try
                        Next

                        larFact.Add(lrFact)

                        If larFact.Count = Me.DefaultQueryLimit Then
                            lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                            GoTo FinishedProcessing
                        End If

                    End While
                End If
FinishedProcessing:

                lrRecordset.Facts = larFact
#End Region

#Region "Old Code"
                '                '=====================================================
                '                'Works for: MATCH (p:Person)-[r:ACTED_IN]-(m:Movie {title:"The Matrix"}) RETURN p.name, m.title
                '                'NB Neo4j is case sensitive.
                '                Dim loResult = Me._driver.Session.ReadTransaction(Function(tx)
                '                                                                      Dim result = tx.Run(asQuery)
                '                                                                      Return result.ToList
                '                                                                  End Function)



                '                If loResult.Count > 0 Then

                '                    '==================================================================
                '                    'Column Names   
                '                    Dim lsColumnName As String
                '                    For liFieldInd = 0 To loResult(0).Keys.Count - 1
                '                        lsColumnName = lrFactType.CreateUniqueRoleName(loResult(0).Keys(liFieldInd), 0)
                '                        Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                '                        lrFactType.RoleGroup.AddUnique(lrRole)
                '                        lrRecordset.Columns.Add(lsColumnName)
                '                    Next
                '                    '==================================================================
                '                    For Each lrResult In loResult

                '                        lrFact = New FBM.Fact(lrFactType, False)
                '                        Dim loFieldValue As Object = Nothing
                '                        Dim liInd As Integer
                '                        For liInd = 0 To loResult(0).Keys.Count - 1

                '                            If lrResult.Values(loResult(0).Keys(liInd)) Is Nothing Then
                '                                loFieldValue = "NULL"
                '                                GoTo AddFactData
                '                            End If
                '                            Select Case lrResult.Values(loResult(0).Keys(liInd)).GetType '.GetFieldType(liInd)
                '                                Case Is = GetType(String)
                '                                    If Not Viev.NullVal(lrResult.Values(loResult(0).Keys(liInd)), "") = "" Then
                '                                        loFieldValue = lrResult.Values(loResult(0).Keys(liInd))
                '                                    Else
                '                                        loFieldValue = ""
                '                                    End If
                '                                Case Is = GetType(DateTime)
                '                                    '                Try
                '                                    '                    loFieldValue = lrSQLiteDataReader.GetDateTime(liInd).ToString(Me.DateTimeFormat)
                '                                    '                Catch ex As Exception
                '                                    '                    Try
                '                                    '                        loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                '                                    '                    Catch ex1 As Exception
                '                                    '                        'Sometimes DateTime values are not in the correct format. None the less they are stored in SQLite.
                '                                    '                        loFieldValue = lrSQLiteDataReader.GetString(liInd)
                '                                    '                    End Try
                '                                    '                End Try
                '                                Case Else
                '                                    '                Try
                '                                    '                    loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                '                                    '                Catch ex As Exception
                '                                    '                    'Sometimes DateTime values are not in the correct format. None the less they are stored in SQLite.
                '                                    '                    loFieldValue = lrSQLiteDataReader.GetString(liInd)
                '                                    '                End Try
                '                                    loFieldValue = lrResult.Values(loResult(0).Keys(liInd))
                '                            End Select

                'AddFactData:
                '                            Try
                '                                lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(Viev.NullVal(loFieldValue, "")), lrFact))
                '                                '=====================================================
                '                            Catch
                '                                Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                '                            End Try
                '                        Next

                '                        larFact.Add(lrFact)

                '                        If larFact.Count = Me.DefaultQueryLimit Then
                '                            lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                '                            GoTo FinishedProcessing
                '                        End If

                '                    Next
                '                End If
                'FinishedProcessing:

                '                lrRecordset.Facts = larFact
#End Region

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
                lrRecordset = Me.GO(asSQLQuery)

                If lrRecordset.ErrorString IsNot Nothing Then
                    Throw New Exception(lrRecordset.ErrorString)
                End If

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
                Dim lsSQL As String = ""

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

                lsSQL = "ALTER TABLE " & arColumn.Table.Name & " DROP " & arColumn.Name

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
                Dim lsSQLCommand = "DROP TABLE " & arTable.Name

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
                Dim lrRecordset As ORMQL.Recordset

                Dim lsSQLCommmand = "ALTER TABLE " & arColumn.Table.Name
                lsSQLCommmand &= " RENAME " & arColumn.Name & " TO " & asNewColumnName

                lrRecordset = Me.GONonQuery(lsSQLCommmand)

                If lrRecordset.ErrorString IsNot Nothing Then
                    Throw New Exception(lrRecordset.ErrorString)
                End If

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
                Dim lsSQLCommmand = "ALTER TABLE " & arTable.Name
                lsSQLCommmand &= " RENAME TO " & asNewName

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

                lsSQLQuery = "MATCH (a:" & asTableName & ") RETURN ID(a) AS ID LIMIT 1"

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

            lsSQLQuery = "MATCH (" & LCase(asTableName) & ":" & asTableName & "{" & vbCrLf

            For Each lrColumn In aarPKColumn
                lsSQLQuery &= lrColumn.Name & ": "
                lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "")
                lsSQLQuery &= lrColumn.TemporaryData
                lsSQLQuery &= Boston.returnIfTrue(lrColumn.DataTypeIsText, "'", "") & vbCrLf
            Next
            lsSQLQuery &= " })"

            lsSQLQuery &= " SET " & LCase(asTableName) & "." & arColumn.Name & " = " & vbCrLf
            lsSQLQuery &= Boston.returnIfTrue(arColumn.DataTypeIsText, "'", "")
            lsSQLQuery &= asNewValue
            lsSQLQuery &= Boston.returnIfTrue(arColumn.DataTypeIsText, "'", "") & vbCrLf

            lsSQLQuery &= "RETURN " & LCase(asTableName)

            Dim lrRecordset = Me.GO(lsSQLQuery)

            Return lrRecordset

        End Function

    End Class

End Namespace
