Imports System.Data

Namespace PluginInterface.Sources

    Friend Class SQLServer
        Implements IConnection

        Private mName As String = Nothing
        Private ConnStr As String = Nothing

        Friend Enum QueryEnum
            SchemaQuery = 1
            TableQuery = 2
            ColumnQuery = 3
            RoutineSchemaQuery = 4
        End Enum

        Private mSchemaQuery As String = Me.GetQuery(QueryEnum.SchemaQuery)
        Private mTableSchemaQuery As String = Me.GetQuery(QueryEnum.TableQuery)
        Private mColumnSchemaQuery As String = Me.GetQuery(QueryEnum.ColumnQuery)
        Private mTableNamePlaceHolder As String = Persistence.Source.Default_TableNamePlaceHolder
        Private mRoutineSchemaQuery As String = Me.GetQuery(QueryEnum.RoutineSchemaQuery)
        Private mTransforms As String = Me.GetTransforms()

        Private mIgnoreTableNames As New List(Of String)

        Public Sub New(ByVal Name As String, ByVal ConnectionString As String)
            Me.mName = Name
            Me.ConnStr = ConnectionString
        End Sub

        Public _BostonModel As FBM.Model
        Public Property BostonModel As FBM.Model Implements IConnection.BostonModel
            Get
                Return Me._BostonModel
            End Get
            Set(value As FBM.Model)
                Me._BostonModel = value
            End Set
        End Property

        Friend Function GetQuery(ByVal Query As QueryEnum) As String
            Select Case Query
                Case QueryEnum.SchemaQuery : Return Globals.ReadResource("Boston.Queries.SQLServer.SchemaQuery.sql")
                Case QueryEnum.TableQuery : Return Globals.ReadResource("Boston.Queries.SQLServer.TableQuery.sql")
                Case QueryEnum.ColumnQuery : Return Globals.ReadResource("Boston.Queries.SQLServer.ColumnQuery.sql")
                Case QueryEnum.RoutineSchemaQuery : Return Globals.ReadResource("Boston.Queries.SQLServer.RoutineSchemaQuery.sql")
            End Select
            Return Nothing
        End Function

        Friend Function GetTransforms() As String
            Return Globals.ReadResource("Boston.Transforms.SQLServer.txt")
        End Function

        Public Property Name() As String Implements IConnection.Name
            Get
                Return Me.mName
            End Get
            Set(ByVal value As String)
                Me.mName = value
            End Set
        End Property

        Public Property ConnectionString() As String Implements IConnection.ConnectionString
            Get
                Return Me.ConnStr
            End Get
            Set(ByVal value As String)
                Me.ConnStr = value
            End Set
        End Property

        Public Property SchemaQuery() As String Implements IConnection.SchemaQuery
            Get
                Return Me.mSchemaQuery
            End Get
            Set(ByVal value As String)
                Me.mSchemaQuery = value
            End Set
        End Property

        Public Property TableSchemaQuery() As String Implements IConnection.TableSchemaQuery
            Get
                Return Me.mTableSchemaQuery
            End Get
            Set(ByVal value As String)
                Me.mTableSchemaQuery = value
            End Set
        End Property

        Public Property ColumnSchemaQuery() As String Implements IConnection.ColumnSchemaQuery
            Get
                Return Me.mColumnSchemaQuery
            End Get
            Set(ByVal value As String)
                Me.mColumnSchemaQuery = value
            End Set
        End Property

        Public Property TableNamePlaceHolder() As String Implements IConnection.TableNamePlaceHolder
            Get
                Return Me.mTableNamePlaceHolder
            End Get
            Set(ByVal value As String)
                Me.mTableNamePlaceHolder = value
            End Set
        End Property

        Public Property RoutineSchemaQuery() As String Implements IConnection.RoutineSchemaQuery
            Get
                Return Me.mRoutineSchemaQuery
            End Get
            Set(ByVal value As String)
                Me.mRoutineSchemaQuery = value
            End Set
        End Property

        Public Property Transformations() As String Implements IConnection.Transformations
            Get
                Return Me.mTransforms
            End Get
            Set(ByVal value As String)
                Me.mTransforms = value
            End Set
        End Property

        Friend ReadOnly Property IgnoreTableNames() As List(Of String) Implements IConnection.IgnoreTableNames
            Get
                Return Me.mIgnoreTableNames
            End Get
        End Property

        Public Sub TestConnection() Implements IConnection.TestConnection
            Dim Conn As New SqlClient.SqlConnection(Me.ConnStr)
            Conn.Open()
            Conn.Close()
            Conn.Dispose()
        End Sub

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Using conn As New SqlClient.SqlConnection(Me.ConnStr)
                conn.Open()

                Using cmd As New SqlClient.SqlCommand()
                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = Query

                    Dim dt As New DataTable()
                    Using da As New SqlClient.SqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using

                    Return dt
                End Using

                conn.Close()
            End Using
        End Function

        Public Function GetSchema() As List(Of SchemaRow) Implements IConnection.GetSchema
            If Not String.IsNullOrEmpty(Me.ColumnSchemaQuery) Then
                If Me.ColumnSchemaQuery.IndexOf(Me.TableNamePlaceHolder) = -1 Then
                    Throw New Exception("Required placeholder for table name: '" & Me.TableNamePlaceHolder & "'.")
                End If
            End If

            Dim Schema As New List(Of SchemaRow)

            Using conn As New SqlClient.SqlConnection(Me.ConnStr)
                conn.Open()

                'Using single result set
                If Not String.IsNullOrEmpty(Me.SchemaQuery) Then
                    Using cmd As New SqlClient.SqlCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.SchemaQuery

                        Dim dtSchema As New DataTable()
                        Using da As New SqlClient.SqlDataAdapter(cmd)
                            da.Fill(dtSchema)
                        End Using

                        conn.Close()

                        For Each dr As DataRow In dtSchema.Rows
                            Schema.Add(Me.SetColumnAttributes(dr.Item("TABLE_NAME").ToString, dr.Item("TABLE_TYPE").ToString, dr))
                        Next

                        Return Schema
                    End Using
                End If

                'Way of the Table/Column

                'Retrieve table schema first
                Dim TableSchema As New List(Of SchemaRow)
                If Not String.IsNullOrEmpty(Me.TableSchemaQuery) Then
                    'Using table schema query
                    Using cmd As New SqlClient.SqlCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.TableSchemaQuery

                        Dim dtTableSchema As New DataTable()
                        Using da As New SqlClient.SqlDataAdapter(cmd)
                            da.Fill(dtTableSchema)
                        End Using

                        TableSchema = Me.GetInitialTables(dtTableSchema)
                    End Using

                Else
                    'Get by default using GetSchema
                    Dim dtTableSchema As New DataTable()
                    dtTableSchema = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.Tables)
                    TableSchema = Me.GetInitialTables(dtTableSchema)

                End If


                'Get columns for each table
                If Not String.IsNullOrEmpty(Me.ColumnSchemaQuery) Then
                    'Use column schema query
                    For Each tsr As SchemaRow In TableSchema
                        Dim dtColumnSchema As New DataTable()

                        Using cmd As New SqlClient.SqlCommand()
                            cmd.Connection = conn
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = Me.ColumnSchemaQuery.Replace(Me.TableNamePlaceHolder, tsr.Name)

                            Using da As New SqlClient.SqlDataAdapter(cmd)
                                da.Fill(dtColumnSchema)
                            End Using
                        End Using

                        'Get column schema
                        For Each dr As DataRow In dtColumnSchema.Rows
                            Schema.Add(Me.SetColumnAttributes(tsr.Name, tsr.Type, dr))
                        Next
                    Next

                Else
                    'Get by default using DataReader GetSchemaTable
                    'Foreign key retrieval is shit-house. We will not use generic get schema.
                    'For Each tsr As SchemaRow In TableSchema
                    '    Dim dtAllCols As DataTable = conn.GetSchema("Columns", _
                    '                                                New String() {Nothing, Nothing, tsr.Table_Name, Nothing})

                    '    Dim dtIndexes As DataTable = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.IndexColumns, _
                    '                                                New String() {Nothing, Nothing, tsr.Table_Name, Nothing, Nothing})

                    '    Dim dtAllIndexes As DataTable = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.Indexes, _
                    '                                                   New String() {Nothing, Nothing, tsr.Table_Name, Nothing})

                    '    Dim dtForeignKeys As DataTable = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.ForeignKeys, _
                    '                                                    New String() {Nothing, Nothing, tsr.Table_Name, Nothing})

                    '    Using cmd As New SqlClient.SqlCommand()
                    '        cmd.Connection = conn
                    '        cmd.CommandType = CommandType.Text
                    '        cmd.CommandText = "SELECT * FROM [" & tsr.Table_Name & "]"

                    '        Dim reader As SqlClient.SqlDataReader = cmd.ExecuteReader(CommandBehavior.KeyInfo)
                    '        For Each dr As DataRow In reader.GetSchemaTable().Rows
                    '            Dim sr As SchemaRow = Me.SetColumnAttributesUsingDataReader(tsr, dr)

                    '            'Foreign keys - not supported. stupid.
                    '            For Each keyDr As DataRow In dtForeignKeys.Rows
                    '                If keyDr.Item("FK_COLUMN_NAME").ToString.Equals(sr.Column_Name) Then
                    '                    sr.IsForeignKey = True
                    '                End If
                    '            Next

                    '            Schema.Add(sr)
                    '        Next
                    '        reader.Close()
                    '    End Using
                    'Next

                End If

                conn.Close()
            End Using

            Return Schema
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
            If String.IsNullOrEmpty(Me.SchemaQuery) Then Return New List(Of SchemaRow)

            Using conn As New SqlClient.SqlConnection(Me.ConnStr)
                conn.Open()

                Dim Schema As New List(Of SchemaRow)
                Using cmd As New SqlClient.SqlCommand()
                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = Me.RoutineSchemaQuery

                    Dim dtSchema As New DataTable()
                    Using da As New SqlClient.SqlDataAdapter(cmd)
                        da.Fill(dtSchema)
                    End Using

                    conn.Close()

                    For Each dr As DataRow In dtSchema.Rows
                        Schema.Add(Me.SetParameterAttributes(dr.Item("ROUTINE_NAME").ToString, dr.Item("ROUTINE_TYPE").ToString, dr))
                    Next

                    Return Schema
                End Using

                conn.Close()
            End Using
        End Function

        Public Function GetRoutineColumnSchema(ByVal RoutineName As String, _
                                               ByVal RoutineType As String, _
                                               ByVal IsProcedure As Boolean, _
                                               ByVal ParamList As List(Of String)) As List(Of SchemaRow) Implements IConnection.GetRoutineColumnSchema
            Using conn As New SqlClient.SqlConnection(Me.ConnStr)
                conn.Open()

                Using cmd As New SqlClient.SqlCommand()
                    Dim sb As New System.Text.StringBuilder("SET FMTONLY ON; ")
                    If Parser.Syntax.Strings.StrEq(RoutineType, "PROCEDURE") Then
                        sb.Append("EXEC [" & RoutineName & "]")
                        For i As Integer = 0 To ParamList.Count - 1
                            sb.Append(" NULL")
                            If i < ParamList.Count - 1 Then sb.Append(",")
                        Next
                    ElseIf Parser.Syntax.Strings.StrEq(RoutineType, "FUNCTION") Then
                        sb.Append("SELECT * FROM [" & RoutineName & "](")
                        For i As Integer = 0 To ParamList.Count - 1
                            sb.Append("NULL")
                            If i < ParamList.Count - 1 Then sb.Append(", ")
                        Next
                        sb.Append(")")
                    End If
                    sb.Append("; SET FMTONLY OFF;")

                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = sb.ToString

                    Dim dtSchema As New DataTable()
                    Using da As New SqlClient.SqlDataAdapter(cmd)
                        da.FillSchema(dtSchema, SchemaType.Source)
                    End Using

                    conn.Close()

                    Return Me.BuildColumns(RoutineName, RoutineType, IsProcedure, dtSchema)
                End Using

                conn.Close()
            End Using
        End Function

        Private Function TypeToSqlDbType(ByVal type As Type) As SqlDbType
            Dim p1 As New SqlClient.SqlParameter()
            Dim tc As System.ComponentModel.TypeConverter
            tc = System.ComponentModel.TypeDescriptor.GetConverter(p1.DbType)

            If tc.CanConvertFrom(type) Then
                p1.DbType = DirectCast(tc.ConvertFrom(type.Name), DbType)
            Else
                Try
                    'Custom
                    Dim typeName As String = type.Name
                    typeName = typeName.Replace("[]", "()")
                    If Parser.Syntax.Strings.StrEq(typeName, "byte()") Then
                        Return SqlDbType.Binary
                    Else
                        p1.DbType = DirectCast(tc.ConvertFrom(typeName), DbType)
                    End If
                Catch ex As Exception
                    If My.Application.EXEC_ErrorSensitivity > My.MyApplication.ErrorSensitivity.Standard Then Throw
                End Try
            End If

            Return p1.SqlDbType
        End Function

        Public Function BuildColumns(ByVal RoutineName As String, ByVal RoutineType As String, _
                                     ByVal IsProcedure As Boolean, ByVal dtSchema As DataTable) As List(Of SchemaRow)
            Dim Schema As New List(Of SchemaRow)
            For i As Integer = 0 To dtSchema.Columns.Count - 1
                Dim sr As New SchemaRow()
                sr.Name = RoutineName
                sr.Type = RoutineType

                sr.Column_Name = dtSchema.Columns(i).ColumnName
                sr.Data_Type = Me.TypeToSqlDbType(dtSchema.Columns(i).DataType).ToString
                sr.Ordinal_Position = dtSchema.Columns(i).Ordinal
                sr.Length = dtSchema.Columns(i).MaxLength
                sr.Precision = -1
                sr.Scale = -1
                sr.Nullable = dtSchema.Columns(i).AllowDBNull
                sr.IsIdentity = dtSchema.Columns(i).AutoIncrement

                sr.IsTable = False
                sr.IsView = False

                sr.IsPrimaryKey = dtSchema.Columns(i).AutoIncrement
                sr.IsForeignKey = False

                sr.IsProcedure = IsProcedure
                sr.IsFunction = Not IsProcedure

                Schema.Add(sr)
            Next
            Return Schema
        End Function

        Private Function GetInitialTables(ByVal dtTableSchema As DataTable) As List(Of SchemaRow)
            Dim TableSchema As New List(Of SchemaRow)
            For Each dr As DataRow In dtTableSchema.Rows
                If dr.Item("TABLE_TYPE").ToString.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase) Or dr.Item("TABLE_TYPE").ToString.Equals("VIEW", StringComparison.OrdinalIgnoreCase) Then
                    Dim tsr As New SchemaRow()
                    tsr.Name = dr.Item("TABLE_NAME").ToString
                    tsr.Type = dr.Item("TABLE_TYPE").ToString
                    TableSchema.Add(tsr)
                End If
            Next

            Return TableSchema
        End Function

        Private Function SetColumnAttributes(ByVal Table_Name As String, ByVal Table_Type As String, ByVal dr As DataRow) As SchemaRow
            Dim sr As New SchemaRow()
            sr.Name = Table_Name
            sr.Type = Table_Type

            sr.Column_Name = dr.Item("COLUMN_NAME").ToString
            sr.Data_Type = dr.Item("DATA_TYPE").ToString
            sr.Ordinal_Position = Convert.ToInt64(dr.Item("ORDINAL_POSITION"))
            sr.Length = Convert.ToInt64(IIf(dr.Item("CHARACTER_MAXIMUM_LENGTH") Is DBNull.Value, -1, dr.Item("CHARACTER_MAXIMUM_LENGTH")))
            sr.Precision = Convert.ToInt64(IIf(dr.Item("NUMERIC_PRECISION") Is DBNull.Value, -1, dr.Item("NUMERIC_PRECISION")))
            sr.Scale = Convert.ToInt64(IIf(dr.Item("NUMERIC_SCALE") Is DBNull.Value, -1, dr.Item("NUMERIC_SCALE")))
            sr.Nullable = CBool(IIf(dr.Item("IS_NULLABLE").ToString.Equals("YES", StringComparison.OrdinalIgnoreCase), True, False))
            sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("1"), True, False))

            sr.IsTable = sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
            sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)

            sr.IsPrimaryKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase)
            sr.IsForeignKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase)

            Return sr
        End Function

        'Private Function SetColumnAttributesUsingDataReader(ByVal tableSchemaRow As SchemaRow, ByVal dr As DataRow) As SchemaRow
        '    Dim sr As New SchemaRow()
        '    sr.Table_Name = tableSchemaRow.Table_Name
        '    sr.Table_Type = tableSchemaRow.Table_Type

        '    sr.Column_Name = dr.Item("ColumnName").ToString
        '    sr.Data_Type = dr.Item("DataTypeName").ToString
        '    sr.Ordinal_Position = CInt(dr.Item("ColumnOrdinal"))
        '    sr.Length = CInt(IIf(dr.Item("ColumnSize") Is DBNull.Value, -1, dr.Item("ColumnSize")))
        '    sr.Precision = CInt(IIf(dr.Item("NumericPrecision") Is DBNull.Value, -1, dr.Item("NumericPrecision")))
        '    sr.Scale = CInt(IIf(dr.Item("NumericScale") Is DBNull.Value, -1, dr.Item("NumericScale")))
        '    sr.Nullable = CBool(dr.Item("AllowDBNull").ToString)

        '    sr.IsTable = sr.Table_Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
        '    sr.IsView = sr.Table_Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)

        '    sr.IsPrimaryKey = CBool(dr.Item("IsKey").ToString)
        '    sr.IsIdentity = CBool(dr.Item("IsIdentity").ToString)

        '    Return sr
        'End Function

        Private Function SetParameterAttributes(ByVal Routine_Name As String, ByVal Routine_Type As String, ByVal dr As DataRow) As SchemaRow
            Dim sr As New SchemaRow()
            sr.Name = Routine_Name
            sr.Type = Routine_Type

            sr.Parameter_Name = Tools.Conv.ToString(dr.Item("PARAMETER_NAME"), True)
            sr.Parameter_Mode = Tools.Conv.ToString(dr.Item("PARAMETER_MODE"), True)
            sr.IsInMode = sr.Parameter_Mode.Equals("IN")
            sr.IsOutMode = sr.Parameter_Mode.Equals("OUT")
            sr.IsInOutMode = sr.Parameter_Mode.Equals("INOUT")

            sr.Data_Type = Tools.Conv.ToString(dr.Item("DATA_TYPE"), True)
            sr.Ordinal_Position = Tools.Conv.ToLong(dr.Item("ORDINAL_POSITION"))
            sr.Length = Convert.ToInt64(IIf(dr.Item("CHARACTER_MAXIMUM_LENGTH") Is DBNull.Value, -1, dr.Item("CHARACTER_MAXIMUM_LENGTH")))
            sr.Precision = Convert.ToInt64(IIf(dr.Item("NUMERIC_PRECISION") Is DBNull.Value, -1, dr.Item("NUMERIC_PRECISION")))
            sr.Scale = Convert.ToInt64(IIf(dr.Item("NUMERIC_SCALE") Is DBNull.Value, -1, dr.Item("NUMERIC_SCALE")))

            sr.IsProcedure = sr.Type.Equals("PROCEDURE", StringComparison.OrdinalIgnoreCase)
            sr.IsFunction = sr.Type.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase)

            Return sr
        End Function

        Public Function GetTables() As List(Of String) Implements IConnection.GetTables
            Dim schema As List(Of SchemaRow) = Me.GetSchema()
            Dim tables As New List(Of String)
            Dim lastTable As String = ""
            For Each sr As SchemaRow In schema
                If Not sr.Name.Equals(lastTable) And sr.IsTable Then
                    lastTable = sr.Name
                    tables.Add(sr.Name)
                End If
            Next
            Return tables
        End Function

        Public Function CreateCopy(ByVal Name As String, ByVal Connectionstring As String, _
                                   ByVal SchemaQuery As String, ByVal TableSchemaQuery As String, _
                                   ByVal ColumnSchemaQuery As String, ByVal TableNamePlaceHolder As String, _
                                   ByVal RoutineSchemaQuery As String, ByVal Transformations As String, _
                                   ByVal IgnoreTableNames As List(Of String)) As IConnection Implements IConnection.CreateCopy
            Dim copy As New SQLServer(Name, Connectionstring)
            copy.SchemaQuery = SchemaQuery
            copy.TableSchemaQuery = TableSchemaQuery
            copy.ColumnSchemaQuery = ColumnSchemaQuery
            copy.TableNamePlaceHolder = TableNamePlaceHolder
            copy.RoutineSchemaQuery = RoutineSchemaQuery
            copy.Transformations = Transformations
            For Each tbl In IgnoreTableNames
                copy.IgnoreTableNames.Add(tbl)
            Next
            Return copy
        End Function

        Public Sub RunScriptFile(ByVal ScriptFile As String) Implements IConnection.RunScriptFile
            If Not System.IO.File.Exists(ScriptFile) Then Throw New Exception("File '" & ScriptFile & "' does not exist.")

            Dim sr As System.IO.StreamReader = Nothing
            Try
                sr = New System.IO.StreamReader(ScriptFile, False)

                Using conn As New SqlClient.SqlConnection(Me.ConnStr)
                    Dim server As New Microsoft.SqlServer.Management.Smo.Server(New Microsoft.SqlServer.Management.Common.ServerConnection(conn))
                    server.ConnectionContext.Connect()
                    server.ConnectionContext.ExecuteNonQuery(sr.ReadToEnd())
                    server.ConnectionContext.Disconnect()
                End Using

            Catch ex As Exception
                Throw ex

            Finally
                sr.Close()
                sr.Dispose()

            End Try
        End Sub

    End Class

End Namespace