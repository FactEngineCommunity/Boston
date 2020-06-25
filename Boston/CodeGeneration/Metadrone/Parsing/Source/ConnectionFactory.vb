Imports Boston.PluginInterface.Sources
Imports Boston.Parser.Syntax.Strings

Namespace Parser.Source

    Friend Class ConnectionFactory

        Public Shared Function EvaluateProvider(ByVal Source As Parser.Source.Source) As IConnection
            If StrEq(Source.Provider, PluginInterface.Sources.Descriptions.SQLSERVER.ProviderName) Then
                'SQL Server
                Dim sql As New SQLServer(Source.Name, Source.ConnectionString)
                sql.SchemaQuery = Source.SchemaQuery
                sql.TableSchemaQuery = Source.TableSchemaQuery
                sql.ColumnSchemaQuery = Source.ColumnSchemaQuery
                sql.TableNamePlaceHolder = Source.TableNamePlaceHolder
                sql.RoutineSchemaQuery = Source.RoutineSchemaQuery
                sql.Transformations = Source.Transformations
                Return sql

            ElseIf StrEq(Source.Provider, PluginInterface.Sources.Descriptions.ORACLE.ProviderName) Then
                'Oracle
                Dim sql As New Oracle(Source.Name, Source.ConnectionString)
                sql.SchemaQuery = Source.SchemaQuery
                sql.TableSchemaQuery = Source.TableSchemaQuery
                sql.ColumnSchemaQuery = Source.ColumnSchemaQuery
                sql.TableNamePlaceHolder = Source.TableNamePlaceHolder
                sql.RoutineSchemaQuery = Source.RoutineSchemaQuery
                sql.Transformations = Source.Transformations
                Return sql

            ElseIf StrEq(Source.Provider, PluginInterface.Sources.Descriptions.ACCESS.ProviderName) Or _
                   StrEq(Source.Provider, PluginInterface.Sources.Descriptions.ACCESS2K.ProviderName) Then
                'Access
                Dim sql As New Access(Source.Name, Source.ConnectionString)
                sql.TableSchemaQuery = Source.TableSchemaQuery
                sql.Transformations = Source.Transformations
                Return sql

            ElseIf StrEq(Source.Provider, PluginInterface.Sources.Descriptions.EXCEL.ProviderName) Then
                'Excel
                Dim sql As New Excel(Source.Name, Source.ConnectionString)
                sql.Transformations = Source.Transformations
                Return sql

            ElseIf StrEq(Source.Provider, PluginInterface.Sources.Descriptions.OLEDB.ProviderName) Then
                'Generic OLEDB
                Dim sql As New OleDb(Source.Name, Source.ConnectionString)
                sql.SchemaQuery = Source.SchemaQuery
                sql.TableSchemaQuery = Source.TableSchemaQuery
                sql.ColumnSchemaQuery = Source.ColumnSchemaQuery
                sql.TableNamePlaceHolder = Source.TableNamePlaceHolder
                sql.RoutineSchemaQuery = Source.RoutineSchemaQuery
                sql.Transformations = Source.Transformations
                Return sql

            ElseIf StrEq(Source.Provider, PluginInterface.Sources.Descriptions.ODBC.ProviderName) Then
                'Generic ODBC
                Dim sql As New ODBC(Source.Name, Source.ConnectionString)
                sql.SchemaQuery = Source.SchemaQuery
                sql.TableSchemaQuery = Source.TableSchemaQuery
                sql.ColumnSchemaQuery = Source.ColumnSchemaQuery
                sql.TableNamePlaceHolder = Source.TableNamePlaceHolder
                sql.RoutineSchemaQuery = Source.RoutineSchemaQuery
                sql.Transformations = Source.Transformations
                Return sql

            Else
                'Match with plugins
                For Each pi In Globals.SourcePlugins.Plugins
                    If StrEq(Source.Provider, pi.SourceDescription.ProviderName) Then
                        Return pi.Connection.CreateCopy(Source.Name, Source.ConnectionString, _
                                                        Source.SchemaQuery, Source.TableSchemaQuery, _
                                                        Source.ColumnSchemaQuery, Source.TableNamePlaceHolder, _
                                                        Source.RoutineSchemaQuery, Source.Transformations, _
                                                        pi.Connection.IgnoreTableNames)
                    End If
                Next

                'Unknown
                Return New NullConnection(Source.Name)

            End If
        End Function

    End Class

End Namespace
