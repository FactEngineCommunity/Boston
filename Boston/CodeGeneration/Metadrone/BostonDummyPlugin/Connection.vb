Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    Public Class Connection
        Implements PluginInterface.Sources.IConnection

        Private mName As String = ""
        Private ConnStr As String = ""
        Private mTransforms As String = ""

        Public Sub New()
            Me.mTransforms = Me.GetTransforms()
        End Sub

        Public Sub New(ByVal Name As String, ByVal ConnectionString As String)
            MyBase.New()
            Me.mName = Name
            Me.ConnStr = ConnectionString
        End Sub

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
                Return Me.mTransforms
            End Get
            Set(value As String)
                Me.mTransforms = value
            End Set
        End Property

        Public ReadOnly Property IgnoreTableNames As List(Of String) Implements IConnection.IgnoreTableNames
            Get
                Return New List(Of String)
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

        Public Function GetTransforms() As String
            Return Globals.ReadResource("Boston.BostonDummyPlugin.Transforms.txt")
        End Function

        Public Function GetRoutineSchema() As List(Of SchemaRow) Implements IConnection.GetRoutineSchema
            Throw New NotImplementedException()
        End Function

        Public Function GetRoutineColumnSchema(RoutineName As String, RoutineType As String, IsProcedure As Boolean, ParamList As List(Of String)) As List(Of SchemaRow) Implements IConnection.GetRoutineColumnSchema
            Throw New NotImplementedException()
        End Function

        Public Function CreateCopy(ByVal Name As String,
                                   ByVal Connectionstring As String,
                                   ByVal SchemaQuery As String,
                                   ByVal TableSchemaQuery As String,
                                   ByVal ColumnSchemaQuery As String,
                                   ByVal TableNamePlaceHolder As String,
                                   ByVal RoutineSchemaQuery As String,
                                   ByVal Transformations As String,
                                   ByVal IgnoreTableNames As List(Of String)) As IConnection Implements IConnection.CreateCopy
            Dim copy As Connection = New Connection(Name, Connectionstring)
            copy.SchemaQuery = SchemaQuery
            copy.TableSchemaQuery = TableSchemaQuery
            copy.ColumnSchemaQuery = ColumnSchemaQuery
            copy.TableNamePlaceHolder = TableNamePlaceHolder
            copy.RoutineSchemaQuery = RoutineSchemaQuery
            copy.Transformations = Transformations

            For Each tbl As String In IgnoreTableNames
                copy.IgnoreTableNames.Add(tbl)
            Next

            Return copy
        End Function

    End Class

End Namespace

