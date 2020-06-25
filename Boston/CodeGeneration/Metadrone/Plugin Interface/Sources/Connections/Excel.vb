Namespace PluginInterface.Sources

    Friend Class Excel
        Implements IConnection

        Private mName As String = Nothing
        Private ConnStr As String = Nothing

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
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property TableSchemaQuery() As String Implements IConnection.TableSchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property ColumnSchemaQuery() As String Implements IConnection.ColumnSchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property TableNamePlaceHolder() As String Implements IConnection.TableNamePlaceHolder
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property RoutineSchemaQuery() As String Implements IConnection.RoutineSchemaQuery
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Property Transformations() As String Implements IConnection.Transformations
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Friend ReadOnly Property IgnoreTableNames() As List(Of String) Implements IConnection.IgnoreTableNames
            Get
                Return Me.mIgnoreTableNames
            End Get
        End Property

        Public Sub TestConnection() Implements IConnection.TestConnection
            Dim Conn As New Data.OleDb.OleDbConnection(Me.ConnStr)
            Conn.Open()
            Conn.Close()
            Conn.Dispose()
        End Sub

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Return New DataTable()
        End Function

        Public Function GetSchema() As List(Of SchemaRow) Implements IConnection.GetSchema
            Dim Schema As New List(Of SchemaRow)

            Using conn As New System.Data.OleDb.OleDbConnection(Me.ConnStr)
                conn.Open()

                'Retrieve table schema first
                Dim TableSchema As New List(Of SchemaRow)
                Dim dtTableSchema As New DataTable()
                dtTableSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, _
                                                         New String() {Nothing, Nothing, Nothing, Nothing})

                TableSchema = Me.GetInitialTables(dtTableSchema)

                'Get columns for each table
                For Each tsr As SchemaRow In TableSchema
                    'Get columns
                    Dim ColSchema As New List(Of SchemaRow)
                    For Each dr As DataRow In conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, _
                                                                       New String() {Nothing, Nothing, tsr.Name, Nothing}).Rows
                        ColSchema.Add(Me.SetColumnAttributes(tsr.Name, tsr.Type, dr))
                    Next

                    'Fix table names
                    For Each sr In ColSchema
                        'Remove quotes
                        If sr.Name.IndexOf("'") = 0 And sr.Name.LastIndexOf("'") = sr.Name.Length - 1 Then
                            sr.Name = sr.Name.Substring(1, sr.Name.Length - 2).Trim
                        End If
                        'Remove dollar sign
                        If sr.Name.IndexOf("$") = sr.Name.Length - 1 Then sr.Name = sr.Name.Substring(0, sr.Name.Length - 1).Trim
                    Next

                    'Sort by ordinal position
                    Dim sorter As New Tools.ListSorter(Of SchemaRow)("Ordinal_Position asc")
                    ColSchema.Sort(sorter)

                    'Add to return
                    For Each sr In ColSchema
                        Schema.Add(sr)
                    Next
                Next

                conn.Close()
            End Using

            Return Schema
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
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
            sr.IsView = False

            sr.IsPrimaryKey = False
            sr.IsIdentity = False
            sr.IsIdentity = False

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
            Dim copy As New Excel(Name, Connectionstring)
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
            Throw New Exception("Cannot run script against Microsoft Excel.")
        End Sub

    End Class

End Namespace