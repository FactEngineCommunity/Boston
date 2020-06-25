Namespace PluginInterface.Sources

    Friend Class Access
        Implements IConnection

        Private mName As String = Nothing
        Private ConnStr As String = Nothing

        Friend Enum QueryEnum
            TableQuery = 1
            RoutineSchemaQuery = 4
        End Enum

        Private mTableSchemaQuery As String = Me.GetQuery(QueryEnum.TableQuery)
        Private mRoutineSchemaQuery As String = Nothing
        Private mTransforms As String = Me.GetTransforms()

        Private mIgnoreTableNames As New List(Of String)

        Public Sub New(ByVal Name As String, ByVal ConnectionString As String)
            Me.mName = Name
            Me.ConnStr = ConnectionString
        End Sub

        Friend Function GetQuery(ByVal Query As QueryEnum) As String
            Select Case Query
                Case QueryEnum.TableQuery : Return Globals.ReadResource("Metadrone.Queries.Access.TableQuery.sql")
            End Select
            Return Nothing
        End Function

        Friend Function GetTransforms() As String
            Return Globals.ReadResource("Metadrone.Transforms.Access.txt")
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
                Return Nothing
            End Get
            Set(ByVal value As String)

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
            Dim Conn As New Data.OleDb.OleDbConnection(Me.ConnStr)
            Conn.Open()
            Conn.Close()
            Conn.Dispose()
        End Sub

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Dim oledb As New OleDb(Me.Name, Me.ConnectionString)
            Return oledb.TestQuery(Query)
        End Function

        Public Function GetSchema() As List(Of SchemaRow) Implements IConnection.GetSchema
            Dim oledb As New OleDb(Me.Name, Me.ConnectionString)
            oledb.TableSchemaQuery = Me.TableSchemaQuery
            Return oledb.GetSchema()
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
            Dim copy As New Access(Name, Connectionstring)
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
            Dim oledb As New OleDb(Me.Name, Me.ConnectionString)
            Call oledb.RunScriptFile(ScriptFile)
        End Sub

    End Class

End Namespace