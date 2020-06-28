Imports Boston.FBM
Imports Boston.PluginInterface.Sources

Namespace SourcePlugins.Boston
    ''' <summary>
    ''' See Loader.vb for how the Plugin is created
    ''' </summary>
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
                Return Me.mName
            End Get
            Set(value As String)
                Me.mName = value
            End Set
        End Property

        Public _BostonModel As FBM.Model
        Public Property BostonModel As Model Implements IConnection.BostonModel
            Get
                Return Me._BostonModel
            End Get
            Set(value As Model)
                Me._BostonModel = value
            End Set
        End Property

        Public Property ConnectionString As String Implements IConnection.ConnectionString
            Get
                Return Nothing 'Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property SchemaQuery As String Implements IConnection.SchemaQuery
            Get
                Return Nothing
            End Get
            Set(value As String)
            End Set
        End Property

        Public Property TableSchemaQuery As String Implements IConnection.TableSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                ' Throw New NotImplementedException()
            End Set
        End Property

        Public Property ColumnSchemaQuery As String Implements IConnection.ColumnSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                'Throw New NotImplementedException()
            End Set
        End Property

        Public Property TableNamePlaceHolder As String Implements IConnection.TableNamePlaceHolder
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                'Throw New NotImplementedException()
            End Set
        End Property

        Public Property RoutineSchemaQuery As String Implements IConnection.RoutineSchemaQuery
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                'Throw New NotImplementedException()
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

            Dim larSchema As New List(Of SchemaRow)

            If Me.BostonModel Is Nothing Then Return larSchema

            Dim sr As SchemaRow

            For Each lrTable In Me.BostonModel.RDS.Table
                For Each lrColumn In lrTable.Column
                    sr = New SchemaRow()
                    sr.Name = lrTable.Name
                    sr.Type = "TABLE"
                    sr.Column_Name = lrColumn.Name
                    sr.Data_Type = lrColumn.getMetamodelDataType.ToString
                    sr.Ordinal_Position = lrColumn.OrdinalPosition
                    sr.Length = lrColumn.getMetamodelDataTypeLength
                    sr.Precision = lrColumn.getMetamodelDataTypePrecision
                    sr.Scale = 0
                    sr.Nullable = Not lrColumn.IsMandatory
                    sr.IsIdentity = False
                    sr.IsTable = True
                    sr.IsView = False
                    sr.IsPrimaryKey = lrColumn.ContributesToPrimaryKey
                    sr.IsForeignKey = False
                    larSchema.Add(sr)
                Next
            Next

            Return larSchema

        End Function

        Public Function GetTables() As List(Of String) Implements IConnection.GetTables

            Dim lasTableName As New List(Of String)

            For Each lrTable In Me.BostonModel.RDS.Table
                lasTableName.Add(lrTable.Name)
            Next

            Return lasTableName

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

        Private Function SetColumnAttributes(ByVal Table_Name As String, ByVal Table_Type As String, ByVal dr As DataRow) As SchemaRow
            Dim sr As SchemaRow = New SchemaRow()
            sr.Name = Table_Name.Trim()
            sr.Type = Table_Type.Trim()
            sr.Column_Name = dr("COLUMN_NAME").ToString().Trim()
            sr.Data_Type = dr("DATA_TYPE").ToString().Trim()
            sr.Ordinal_Position = Convert.ToInt64(dr("ORDINAL_POSITION"))
            sr.Length = Convert.ToInt64((If(Object.ReferenceEquals(dr("CHARACTER_MAXIMUM_LENGTH"), DBNull.Value), -1, dr("CHARACTER_MAXIMUM_LENGTH"))))
            sr.Precision = Convert.ToInt64((If(Object.ReferenceEquals(dr("NUMERIC_PRECISION"), DBNull.Value), -1, dr("NUMERIC_PRECISION"))))
            sr.Scale = Convert.ToInt64((If(Object.ReferenceEquals(dr("NUMERIC_SCALE"), DBNull.Value), -1, dr("NUMERIC_SCALE"))))
            sr.Nullable = Convert.ToBoolean((If(dr("IS_NULLABLE").ToString().Trim().Equals("YES", StringComparison.OrdinalIgnoreCase), True, False)))
            sr.IsIdentity = Convert.ToBoolean((If(dr("IS_IDENTITY").ToString().Trim().Equals("1"), True, False)))
            sr.IsTable = sr.Type.Equals("BASE TABLE", StringComparison.OrdinalIgnoreCase)
            sr.IsView = sr.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase)
            sr.IsPrimaryKey = dr("CONSTRAINT_TYPE").ToString().Trim().Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase)
            sr.IsForeignKey = dr("CONSTRAINT_TYPE").ToString().Trim().Equals("FOREIGN KEY", StringComparison.OrdinalIgnoreCase)
            Return sr
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
            copy.BostonModel = BostonModel

            For Each tbl As String In IgnoreTableNames
                copy.IgnoreTableNames.Add(tbl)
            Next

            Return copy
        End Function

    End Class

End Namespace

