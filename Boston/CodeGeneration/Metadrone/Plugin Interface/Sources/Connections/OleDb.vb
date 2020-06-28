Namespace PluginInterface.Sources

    Friend Class OleDb
        Implements IConnection

        Private mName As String = Nothing
        Private ConnStr As String = Nothing

        Private mSchemaQuery As String = Nothing
        Private mTableSchemaQuery As String = Nothing
        Private mColumnSchemaQuery As String = Nothing
        Private mTableNamePlaceHolder As String = Persistence.Source.Default_TableNamePlaceHolder
        Private mRoutineSchemaQuery As String = Nothing
        Private mTransforms As String = Nothing

        Private mIgnoreTableNames As New List(Of String)

        Public Sub New(ByVal Name As String, ByVal ConnectionString As String)
            Me.mName = Name
            Me.ConnStr = ConnectionString
        End Sub

        Public Property Name() As String Implements IConnection.Name
            Get
                Return Me.mName
            End Get
            Set(ByVal value As String)
                Me.mName = value
            End Set
        End Property

        Public _BostonModel As FBM.Model
        Public Property BostonModel As FBM.Model Implements IConnection.BostonModel
            Get
                Return Me._BostonModel
            End Get
            Set(value As FBM.Model)
                Me._BostonModel = value
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
            Dim Conn As New System.Data.OleDb.OleDbConnection(Me.ConnStr)
            Conn.Open()
            Conn.Close()
            Conn.Dispose()
        End Sub

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Using conn As New System.Data.OleDb.OleDbConnection(Me.ConnStr)
                conn.Open()

                Using cmd As New System.Data.OleDb.OleDbCommand()
                    cmd.Connection = conn
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = Query

                    Dim dt As New DataTable()
                    Using da As New System.Data.OleDb.OleDbDataAdapter(cmd)
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

            Using conn As New System.Data.OleDb.OleDbConnection(Me.ConnStr)
                conn.Open()

                'Using single result set
                If Not String.IsNullOrEmpty(Me.SchemaQuery) Then
                    Using cmd As New System.Data.OleDb.OleDbCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.SchemaQuery

                        Dim dtSchema As New DataTable()
                        Using da As New System.Data.OleDb.OleDbDataAdapter(cmd)
                            da.Fill(dtSchema)
                        End Using

                        conn.Close()

                        For Each dr As DataRow In dtSchema.Rows
                            Dim sr As SchemaRow = Me.SetColumnAttributes(dr.Item("TABLE_NAME").ToString, dr.Item("TABLE_TYPE").ToString, dr)

                            If dr.Item("IS_IDENTITY").ToString.Equals("0", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("1", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("1", StringComparison.OrdinalIgnoreCase), True, False))
                            ElseIf dr.Item("IS_IDENTITY").ToString.Equals("YES", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("NO", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("YES", StringComparison.OrdinalIgnoreCase), True, False))
                            ElseIf dr.Item("IS_IDENTITY").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("FALSE", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase), True, False))
                            End If

                            sr.IsPrimaryKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase)
                            sr.IsForeignKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase)

                            Schema.Add(sr)
                        Next

                        Return Schema
                    End Using
                End If

                'Way of the Table/Column

                'Retrieve table schema first
                Dim TableSchema As New List(Of SchemaRow)
                If Not String.IsNullOrEmpty(Me.TableSchemaQuery) Then
                    'Using table schema query
                    Using cmd As New System.Data.OleDb.OleDbCommand()
                        cmd.Connection = conn
                        cmd.CommandType = CommandType.Text
                        cmd.CommandText = Me.TableSchemaQuery

                        Dim dtTableSchema As New DataTable()
                        Using da As New System.Data.OleDb.OleDbDataAdapter(cmd)
                            da.Fill(dtTableSchema)
                        End Using

                        TableSchema = Me.GetInitialTables(dtTableSchema)
                    End Using

                Else
                    'Get by default using getschematable
                    Dim dtTableSchema As New DataTable()
                    dtTableSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, _
                                                             New String() {Nothing, Nothing, Nothing, Nothing})

                    TableSchema = Me.GetInitialTables(dtTableSchema)

                End If


                'Get columns for each table
                If Not String.IsNullOrEmpty(Me.ColumnSchemaQuery) Then
                    'Use column schema query
                    For Each tsr As SchemaRow In TableSchema
                        Dim dtColumnSchema As New DataTable()

                        Using cmd As New System.Data.OleDb.OleDbCommand()
                            cmd.Connection = conn
                            cmd.CommandType = CommandType.Text
                            cmd.CommandText = Me.ColumnSchemaQuery.Replace(Me.TableNamePlaceHolder, tsr.Name)

                            Using da As New System.Data.OleDb.OleDbDataAdapter(cmd)
                                da.Fill(dtColumnSchema)
                            End Using
                        End Using

                        'Get column schema
                        For Each dr As DataRow In dtColumnSchema.Rows
                            Dim sr As SchemaRow = Me.SetColumnAttributes(tsr.Name, tsr.Type, dr)

                            If dr.Item("IS_IDENTITY").ToString.Equals("0", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("1", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("1", StringComparison.OrdinalIgnoreCase), True, False))
                            ElseIf dr.Item("IS_IDENTITY").ToString.Equals("YES", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("NO", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("YES", StringComparison.OrdinalIgnoreCase), True, False))
                            ElseIf dr.Item("IS_IDENTITY").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_IDENTITY").ToString.Equals("FALSE", StringComparison.OrdinalIgnoreCase) Then
                                sr.IsIdentity = CBool(IIf(dr.Item("IS_IDENTITY").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase), True, False))
                            End If

                            sr.IsPrimaryKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase)
                            sr.IsForeignKey = dr.Item("CONSTRAINT_TYPE").ToString.Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase)

                            Schema.Add(sr)
                        Next
                    Next

                Else
                    'Get by default using getschematable
                    For Each tsr As SchemaRow In TableSchema
                        'First get indexes
                        Dim dtIndexes As DataTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Indexes, _
                                                                              New String() {Nothing, Nothing, Nothing, Nothing, tsr.Name})
                        Dim dtForeignKeys As DataTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Foreign_Keys, _
                                                                                  New String() {Nothing, Nothing, Nothing, Nothing, Nothing, tsr.Name})

                        'Get column schema
                        Dim ColSchema As New List(Of SchemaRow)
                        For Each dr As DataRow In conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, _
                                                                           New String() {Nothing, Nothing, tsr.Name, Nothing}).Rows
                            Dim sr As SchemaRow = Me.SetColumnAttributes(tsr.Name, tsr.Type, dr)

                            'See if is an index
                            'Primary keys/unique
                            For Each keyDr As DataRow In dtIndexes.Rows
                                If keyDr.Item("COLUMN_NAME").ToString.Equals(sr.Column_Name) Then
                                    sr.IsPrimaryKey = CBool(keyDr.Item("PRIMARY_KEY"))
                                    sr.IsIdentity = CBool(keyDr.Item("UNIQUE"))
                                End If
                            Next

                            'Foreign keys
                            For Each keyDr As DataRow In dtForeignKeys.Rows
                                If keyDr.Item("FK_COLUMN_NAME").ToString.Equals(sr.Column_Name) Then
                                    sr.IsForeignKey = True
                                End If
                            Next

                            ColSchema.Add(sr)
                        Next

                        'Sort by ordinal position (just in case)
                        Dim sorter As New Tools.ListSorter(Of SchemaRow)("Ordinal_Position asc")
                        ColSchema.Sort(sorter)

                        'Add to return
                        For Each sr In ColSchema
                            Schema.Add(sr)
                        Next
                    Next

                End If

                conn.Close()
            End Using

            Return Schema
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
            ''TODO implement
            Return New List(Of SchemaRow)
        End Function

        Public Function GetRoutineColumnSchema(ByVal RoutineName As String, _
                                               ByVal RoutineType As String, _
                                               ByVal IsProcedure As Boolean, _
                                               ByVal ParamList As List(Of String)) As List(Of SchemaRow) Implements IConnection.GetRoutineColumnSchema
            Return New List(Of SchemaRow)
        End Function

        Private Function GetInitialTables(ByVal dtTableSchema As DataTable) As List(Of SchemaRow)
            Dim TableSchema As New List(Of SchemaRow)
            For Each dr As DataRow In dtTableSchema.Rows
                If dr.Item("TABLE_TYPE").ToString.Equals("TABLE", StringComparison.OrdinalIgnoreCase) Or _
                   dr.Item("TABLE_TYPE").ToString.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase) Or _
                   dr.Item("TABLE_TYPE").ToString.Equals("VIEW", StringComparison.OrdinalIgnoreCase) Then
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
            Try
                sr.Data_Type = CType(dr.Item("DATA_TYPE").ToString, System.Data.OleDb.OleDbType).ToString
            Catch ex As Exception
                sr.Data_Type = dr.Item("DATA_TYPE").ToString
            End Try
            sr.Ordinal_Position = Convert.ToInt64(dr.Item("ORDINAL_POSITION"))
            sr.Length = Convert.ToInt64(IIf(dr.Item("CHARACTER_MAXIMUM_LENGTH") Is DBNull.Value, -1, dr.Item("CHARACTER_MAXIMUM_LENGTH")))
            sr.Precision = Convert.ToInt64(IIf(dr.Item("NUMERIC_PRECISION") Is DBNull.Value, -1, dr.Item("NUMERIC_PRECISION")))
            sr.Scale = Convert.ToInt64(IIf(dr.Item("NUMERIC_SCALE") Is DBNull.Value, -1, dr.Item("NUMERIC_SCALE")))
            If dr.Item("IS_NULLABLE").ToString.Equals("0", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_NULLABLE").ToString.Equals("1", StringComparison.OrdinalIgnoreCase) Then
                sr.Nullable = CBool(IIf(dr.Item("IS_NULLABLE").ToString.Equals("1", StringComparison.OrdinalIgnoreCase), True, False))
            ElseIf dr.Item("IS_NULLABLE").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_NULLABLE").ToString.Equals("FALSE", StringComparison.OrdinalIgnoreCase) Then
                sr.Nullable = CBool(IIf(dr.Item("IS_NULLABLE").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase), True, False))
            ElseIf dr.Item("IS_NULLABLE").ToString.Equals("TRUE", StringComparison.OrdinalIgnoreCase) Or dr.Item("IS_NULLABLE").ToString.Equals("FALSE", StringComparison.OrdinalIgnoreCase) Then
            Else
                Try
                    sr.Nullable = CBool(dr.Item("IS_NULLABLE"))
                Catch ex As Exception
                End Try
            End If

            sr.IsTable = sr.Type.Equals("TABLE", StringComparison.OrdinalIgnoreCase) Or sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
            sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)

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
            Dim copy As New OleDb(Name, Connectionstring)
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

                Using conn As New System.Data.OleDb.OleDbConnection(Me.ConnStr)
                    conn.Open()

                    Using cmd As New System.Data.OleDb.OleDbCommand()
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