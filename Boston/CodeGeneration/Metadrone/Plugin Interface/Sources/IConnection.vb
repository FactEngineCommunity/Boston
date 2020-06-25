Namespace PluginInterface.Sources

    Public Interface IConnection

        Property Name() As String
        Property ConnectionString() As String

        Property SchemaQuery() As String
        Property TableSchemaQuery() As String
        Property ColumnSchemaQuery() As String
        Property TableNamePlaceHolder() As String
        Property RoutineSchemaQuery() As String
        Property Transformations() As String

        ReadOnly Property IgnoreTableNames() As List(Of String)

        Sub TestConnection()
        Function TestQuery(ByVal Query As String) As DataTable
        Function GetSchema() As List(Of SchemaRow)
        Function GetTables() As List(Of String)
        Function GetRoutineSchema() As List(Of SchemaRow)
        Function GetRoutineColumnSchema(ByVal RoutineName As String, ByVal RoutineType As String, _
                                        ByVal IsProcedure As Boolean, ByVal ParamList As List(Of String)) As List(Of SchemaRow)

        Function CreateCopy(ByVal Name As String, ByVal Connectionstring As String, _
                            ByVal SchemaQuery As String, ByVal TableSchemaQuery As String, _
                            ByVal ColumnSchemaQuery As String, ByVal TableNamePlaceHolder As String, _
                            ByVal RoutineSchemaQuery As String, ByVal Transformations As String, _
                            ByVal IgnoreTableNames As List(Of String)) As IConnection

        Sub RunScriptFile(ByVal ScriptFile As String)

    End Interface

End Namespace
