Namespace Persistence.Beta10

    Public Class Source : Implements IMDPersistenceItem
        Private Dirty As Boolean = False
        Private mGUID As String = System.Guid.NewGuid.ToString()

        Public Const Default_TableNamePlaceHolder As String = "@TABLE_NAME"
        Public Const Default_RoutineNamePlaceHolder As String = "@ROUTINE_NAME"

        Public Name As String
        Public Provider As String = PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName

        Public ConnectionString As String
        Public SchemaQuery As String
        Public TableSchemaQuery As String
        Public ColumnSchemaQuery As String
        Public TableNamePlaceHolder As String
        Public RoutineSchemaQuery As String
        Public Transformations As String

        Public Sub New()

        End Sub

        Public Property GUID() As String
            Get
                Return Me.mGUID
            End Get
            Set(ByVal value As String)
                Me.mGUID = value
            End Set
        End Property

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New Source()
            Copy.Name = Me.Name
            Copy.Provider = Me.Provider

            Copy.ConnectionString = Me.ConnectionString
            Copy.SchemaQuery = Me.SchemaQuery
            Copy.TableSchemaQuery = Me.TableSchemaQuery
            Copy.ColumnSchemaQuery = Me.ColumnSchemaQuery
            Copy.TableNamePlaceHolder = Me.TableNamePlaceHolder
            Copy.RoutineSchemaQuery = Me.RoutineSchemaQuery
            Copy.Transformations = Me.Transformations

            Copy.GUID = Me.GUID

            Return Copy
        End Function

        Public Function GetDirty() As Boolean
            Return Me.Dirty
        End Function

        Public Sub SetDirty(ByVal value As Boolean)
            Me.Dirty = value
        End Sub

        Friend Function ToParserSource() As Parser.Source.Source
            Dim src As New Parser.Source.Source()
            src.Name = Me.Name
            src.Provider = Me.Provider
            src.ConnectionString = Me.ConnectionString
            src.SchemaQuery = Me.SchemaQuery
            src.TableSchemaQuery = Me.TableSchemaQuery
            src.ColumnSchemaQuery = Me.ColumnSchemaQuery
            src.TableNamePlaceHolder = Me.TableNamePlaceHolder
            src.RoutineSchemaQuery = Me.RoutineSchemaQuery
            src.Transformations = Me.Transformations

            Return src
        End Function

    End Class

End Namespace