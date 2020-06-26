Namespace PluginInterface.Sources

    Public Interface IManageSource

        Event ValueChanged(ByVal value As Object)
        Event Save()

        Sub Setup()

        Property ConnectionString() As String
        Property SingleResultApproach() As Boolean
        Property TableSchemaGeneric() As Boolean
        Property ColumnSchemaGeneric() As Boolean
        Property SchemaQuery() As String
        Property TableSchemaQuery() As String
        Property ColumnSchemaQuery() As String
        Property TableName() As String
        Property RoutineSchemaQuery() As String
        Property Transformations() As String

        Property BostonModel As FBM.Model

    End Interface

End Namespace