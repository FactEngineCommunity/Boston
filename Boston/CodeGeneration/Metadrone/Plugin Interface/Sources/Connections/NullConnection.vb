Namespace PluginInterface.Sources

    Friend Class NullConnection
        Implements IConnection

        Private mName As String = Nothing

        Public Sub New(ByVal Name As String)
            Me.Name = Name
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
                Return Nothing
            End Get
            Set(ByVal value As String)
                Dim tmp As String = value
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

        Public Property RoutineSchemaQuery() As String Implements IConnection.RoutineSchemaQuery
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

        Public Property Transformations() As String Implements IConnection.Transformations
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Friend ReadOnly Property IgnoreTableNames() As List(Of String) Implements IConnection.IgnoreTableNames
            Get
                Return New List(Of String)
            End Get
        End Property

        Public Function TestQuery(ByVal Query As String) As DataTable Implements IConnection.TestQuery
            Return New DataTable()
        End Function

        Public Function GetSchema() As List(Of SchemaRow) Implements IConnection.GetSchema
            Return New List(Of SchemaRow)
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
            Return New List(Of String)
        End Function

        Public Sub TestConnection() Implements IConnection.TestConnection

        End Sub

        Public Function CreateCopy(ByVal Name As String, ByVal Connectionstring As String, _
                                   ByVal SchemaQuery As String, ByVal TableSchemaQuery As String, _
                                   ByVal ColumnSchemaQuery As String, ByVal TableNamePlaceHolder As String, _
                                   ByVal RoutineSchemaQuery As String, ByVal Transformations As String, _
                                   ByVal IgnoreTableNames As List(Of String)) As IConnection Implements IConnection.CreateCopy
            Dim copy As New NullConnection(Name)
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

        End Sub

    End Class

End Namespace