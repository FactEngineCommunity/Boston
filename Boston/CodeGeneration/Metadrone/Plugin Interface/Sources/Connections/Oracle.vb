Imports System.Data.OracleClient.OracleConnection

Namespace PluginInterface.Sources

    Friend Class Oracle
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

        Friend Function GetQuery(ByVal Query As QueryEnum) As String
            Select Case Query
                Case QueryEnum.SchemaQuery : Return Globals.ReadResource("Metadrone.Queries.Oracle.SchemaQuery.sql")
                Case QueryEnum.TableQuery : Return Globals.ReadResource("Metadrone.Queries.Oracle.TableQuery.sql")
                Case QueryEnum.ColumnQuery : Return Globals.ReadResource("Metadrone.Queries.Oracle.ColumnQuery.sql")
                Case QueryEnum.RoutineSchemaQuery : Return Globals.ReadResource("Metadrone.Queries.Oracle.RoutineSchemaQuery.sql")
            End Select
            Return Nothing
        End Function

        Friend Function GetTransforms() As String
            Return Globals.ReadResource("Metadrone.Transforms.Oracle.txt")
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
            Dim Conn As New OracleClient.OracleConnection(Me.ConnStr)
            Conn.Open()
            Conn.Close()
            Conn.Dispose()
        End Sub

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Using conn As New OracleClient.OracleConnection(Me.ConnStr)
                conn.Open()

                Using cmd As New OracleClient.OracleCommand()
                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = Query

                    Dim dt As New DataTable()
                    Using da As New OracleClient.OracleDataAdapter(cmd)
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

            Using conn As New OracleClient.OracleConnection(Me.ConnStr)
                conn.Open()

                'Using single result set
                If Not String.IsNullOrEmpty(Me.SchemaQuery) Then
                    Using cmd As New OracleClient.OracleCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.SchemaQuery

                        Dim dtSchema As New DataTable()
                        Using da As New OracleClient.OracleDataAdapter(cmd)
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
                    Using cmd As New OracleClient.OracleCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.TableSchemaQuery

                        Dim dtTableSchema As New DataTable()
                        Using da As New OracleClient.OracleDataAdapter(cmd)
                            da.Fill(dtTableSchema)
                        End Using

                        TableSchema = Me.GetInitialTables(dtTableSchema)
                    End Using

                Else
                    'Get by default using GetSchema
                    Dim dtTableSchema As New DataTable()
                    dtTableSchema = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.Tables)
                    TableSchema = Me.GetInitialTablesGeneric(dtTableSchema)

                    dtTableSchema = conn.GetSchema(SqlClient.SqlClientMetaDataCollectionNames.Views)
                    Call Me.GetViewsGeneric(dtTableSchema, TableSchema)
                End If


                'Get columns for each table
                If Not String.IsNullOrEmpty(Me.ColumnSchemaQuery) Then
                    'Use column schema query
                    For Each tsr As SchemaRow In TableSchema
                        Dim dtColumnSchema As New DataTable()

                        Using cmd As New OracleClient.OracleCommand()
                            cmd.Connection = conn
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = Me.ColumnSchemaQuery.Replace(Me.TableNamePlaceHolder, tsr.Name)

                            Using da As New OracleClient.OracleDataAdapter(cmd)
                                da.Fill(dtColumnSchema)
                            End Using
                        End Using

                        'Get column schema
                        For Each dr As DataRow In dtColumnSchema.Rows
                            Schema.Add(Me.SetColumnAttributes(tsr.Name, tsr.Type, dr))
                        Next
                    Next

                End If

                conn.Close()
            End Using

            Return Schema
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
            If String.IsNullOrEmpty(Me.SchemaQuery) Then Return New List(Of SchemaRow)

            Using conn As New OracleClient.OracleConnection(Me.ConnStr)
                conn.Open()

                Dim Schema As New List(Of SchemaRow)
                Using cmd As New OracleClient.OracleCommand()
                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = Me.RoutineSchemaQuery

                    Dim dtSchema As New DataTable()
                    Using da As New OracleClient.OracleDataAdapter(cmd)
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

        Public Function GetRoutineColumnSchema(ByVal RoutineName As String,
                                              ByVal RoutineType As String,
                                              ByVal IsProcedure As Boolean,
                                              ByVal ParamList As List(Of String)) As List(Of SchemaRow) Implements IConnection.GetRoutineColumnSchema
            'TODO implement
            Return New List(Of SchemaRow)

            'Using conn As New OracleClient.OracleConnection(Me.ConnStr)
            '    conn.Open()

            '    Using cmd As New OracleClient.OracleCommand()
            '        Dim sb As New System.Text.StringBuilder("SET FMTONLY ON; ")
            '        If Parser.Syntax.Strings.StrEq(RoutineType, "PROCEDURE") Then
            '            sb.Append("EXEC " & RoutineName & " (")
            '            For i As Integer = 0 To ParamList.Count - 1
            '                sb.Append("NULL")
            '                If i < ParamList.Count - 1 Then sb.Append(", ")
            '            Next
            '            sb.Append(")")
            '        ElseIf Parser.Syntax.Strings.StrEq(RoutineType, "FUNCTION") Then
            '            sb.Append("SELECT * FROM " & RoutineName & "(")
            '            For i As Integer = 0 To ParamList.Count - 1
            '                sb.Append("NULL")
            '                If i < ParamList.Count - 1 Then sb.Append(", ")
            '            Next
            '            sb.Append(")")
            '        End If
            '        sb.Append("; SET FMTONLY OFF;")

            '        cmd.Connection = conn
            '        cmd.CommandType = CommandType.Text
            '        cmd.CommandText = sb.ToString

            '        Dim dtSchema As New DataTable()
            '        Using da As New OracleClient.OracleDataAdapter(cmd)
            '            da.Fill(dtSchema)
            '        End Using

            '        conn.Close()

            '        Return New Parser.Meta.Database.Routine().BuildColumns(RoutineName, RoutineType, IsProcedure, dtSchema)
            '    End Using

            '    conn.Close()
            'End Using
        End Function

        Private Function GetInitialTablesGeneric(ByVal dtTableSchema As DataTable) As List(Of SchemaRow)
            Dim TableSchema As New List(Of SchemaRow)

            'Filter by owner, which comes from connection string
            Dim ownerStartIdx As Integer = Me.ConnStr.IndexOf("USER ID=", StringComparison.OrdinalIgnoreCase)
            If ownerStartIdx = -1 Then Return TableSchema 'Don't bother if no owner

            ownerStartIdx += "USER ID=".Length
            Dim ownerEndIdx As Integer = Me.ConnStr.IndexOf(";", ownerStartIdx)
            If ownerEndIdx = -1 Then ownerEndIdx = Me.ConnStr.Length
            Dim owner As String = Me.ConnStr.Substring(ownerStartIdx, ownerEndIdx - ownerStartIdx).ToUpper.Trim

            For Each dr As DataRow In dtTableSchema.Rows
                If dr.Item("TYPE").ToString.Equals("USER", StringComparison.OrdinalIgnoreCase) And dr.Item("OWNER").ToString.Equals(owner, StringComparison.OrdinalIgnoreCase) Then
                    Dim tsr As New SchemaRow()
                    tsr.Name = dr.Item("TABLE_NAME").ToString
                    tsr.Type = "BASE TABLE"
                    TableSchema.Add(tsr)
                End If
            Next

            Return TableSchema
        End Function

        Private Sub GetViewsGeneric(ByVal dtTableSchema As DataTable, ByRef TableSchema As List(Of SchemaRow))
            'Filter by owner, which comes from connection string
            Dim ownerStartIdx As Integer = Me.ConnStr.IndexOf("USER ID=", StringComparison.OrdinalIgnoreCase)
            If ownerStartIdx = -1 Then Exit Sub 'Don't bother if no owner

            ownerStartIdx += "USER ID=".Length
            Dim ownerEndIdx As Integer = Me.ConnStr.IndexOf(";", ownerStartIdx)
            If ownerEndIdx = -1 Then ownerEndIdx = Me.ConnStr.Length
            Dim owner As String = Me.ConnStr.Substring(ownerStartIdx, ownerEndIdx - ownerStartIdx).ToUpper.Trim

            For Each dr As DataRow In dtTableSchema.Rows
                If dr.Item("OWNER").ToString.Equals(owner, StringComparison.OrdinalIgnoreCase) Then
                    Dim tsr As New SchemaRow()
                    tsr.Name = dr.Item("VIEW_NAME").ToString
                    tsr.Type = "VIEW"
                    TableSchema.Add(tsr)
                End If
            Next
        End Sub

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
            sr.Nullable = CBool(IIf(dr.Item("IS_NULLABLE").ToString.Equals("Y", StringComparison.OrdinalIgnoreCase), True, False))
            sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("UNIQUE"), True, False))

            sr.IsTable = sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
            sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)

            If dr.Item("CONSTRAINT_TYPE") Is DBNull.Value Then Return sr

            sr.IsPrimaryKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("P", StringComparison.OrdinalIgnoreCase)
            sr.IsForeignKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("R", StringComparison.OrdinalIgnoreCase)

            Return sr
        End Function

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

        Public Function CreateCopy(ByVal Name As String, ByVal Connectionstring As String,
                                   ByVal SchemaQuery As String, ByVal TableSchemaQuery As String,
                                   ByVal ColumnSchemaQuery As String, ByVal TableNamePlaceHolder As String,
                                   ByVal RoutineSchemaQuery As String, ByVal Transformations As String,
                                   ByVal IgnoreTableNames As List(Of String)) As IConnection Implements IConnection.CreateCopy
            Dim copy As New Oracle(Name, Connectionstring)
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

                Using conn As New OracleClient.OracleConnection(Me.ConnStr)
                    conn.Open()

                    Using cmd As New OracleClient.OracleCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = sr.ReadToEnd()

                        cmd.ExecuteNonQuery()
                    End Using

                    conn.Close()
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