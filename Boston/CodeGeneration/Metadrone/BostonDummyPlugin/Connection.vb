Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Public Class Connection
        Implements PluginInterface.Sources.IConnection

        Public Property Name As String Implements IConnection.Name
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property ConnectionString As String Implements IConnection.ConnectionString
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property SchemaQuery As String Implements IConnection.SchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property TableSchemaQuery As String Implements IConnection.TableSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property ColumnSchemaQuery As String Implements IConnection.ColumnSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property TableNamePlaceHolder As String Implements IConnection.TableNamePlaceHolder
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property RoutineSchemaQuery As String Implements IConnection.RoutineSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Transformations As String Implements IConnection.Transformations
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public ReadOnly Property IgnoreTableNames As List(Of String) Implements IConnection.IgnoreTableNames
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public Sub TestConnection() Implements IConnection.TestConnection
            'Nothing to do here. Is part of Boston and directly references the currently loaded Model.
        End Sub

        Public Sub RunScriptFile(ScriptFile As String) Implements IConnection.RunScriptFile
            Throw New NotImplementedException()
        End Sub

        Public Function TestQuery(Query As String) As DataTable Implements IConnection.TestQuery
            Throw New NotImplementedException()
        End Function

        Public Function GetSchema() As List(Of SchemaRow) Implements IConnection.GetSchema
            Throw New NotImplementedException()
        End Function

        Public Function GetTables() As List(Of String) Implements IConnection.GetTables
            Throw New NotImplementedException()
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
            Throw New NotImplementedException()
        End Function

        Public Function GetRoutineColumnSchema(RoutineName As String, RoutineType As String, IsProcedure As Boolean, ParamList As List(Of String)) As List(Of SchemaRow) Implements IConnection.GetRoutineColumnSchema
            Throw New NotImplementedException()
        End Function

        Public Function CreateCopy(Name As String, Connectionstring As String, SchemaQuery As String, TableSchemaQuery As String, ColumnSchemaQuery As String, TableNamePlaceHolder As String, RoutineSchemaQuery As String, Transformations As String, IgnoreTableNames As List(Of String)) As IConnection Implements IConnection.CreateCopy
            Throw New NotImplementedException()
        End Function

    End Class

End Namespace

