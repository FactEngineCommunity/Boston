Imports Boston.ORMQL
Imports System.Reflection

Namespace FactEngine
    Public Class DatabaseManager

        Public DatabaseType As pcenumDatabaseType = pcenumDatabaseType.None

        Public FBMModel As FBM.Model

        Public Connection As FactEngine.DatabaseConnection = Nothing

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arFBMModel As FBM.Model)

            Me.FBMModel = arFBMModel

        End Sub

        Public Function GOAsync(ByVal asQuery As String) As ORMQL.Recordset

            Dim lrRecordset As ORMQL.Recordset

            If Me.Connection Is Nothing Then Throw New Exception("The connection to the database has been lost. Close and reopen FactEngine.")

            Me.Connection.GOAsync(asQuery)
            'lrRecordset = Me.Connection.GOAsync(asQuery).Result

            Return lrRecordset

        End Function

        Public Function GO(ByVal asQuery As String) As ORMQL.Recordset

            Dim lrRecordset As ORMQL.Recordset

            If Me.Connection Is Nothing Then Throw New Exception("The connection to the database has been lost. Close and reopen FactEngine.")

            Select Case Me.FBMModel.TargetDatabaseType
                Case Is = pcenumDatabaseType.FactEngineSemanticLayer
                    lrRecordset = Me.Connection.GOAbstractionLayer(asQuery)
                Case Else
                    lrRecordset = Me.Connection.GO(asQuery)
            End Select

            Return lrRecordset

        End Function

        Public Function GONonQuery(ByVal asQuery As String) As ORMQL.Recordset

            Dim lrRecordset As ORMQL.Recordset

            If Me.Connection Is Nothing Then Throw New Exception("The connection to the database has been lost. Close and reopen FactEngine.")

            lrRecordset = Me.Connection.GONonQuery(asQuery)

            Return lrRecordset

        End Function

        Public Function establishConnection(ByVal aiDatabaseType As pcenumDatabaseType,
                                            ByVal asDatabaseConnectionString As String,
                                            Optional ByVal abThrowError As Boolean = True) As FactEngine.DatabaseConnection

            Try

                Select Case aiDatabaseType
                    Case Is = pcenumDatabaseType.MSJet
                        Me.Connection = New FactEngine.MSJet(Me.FBMModel, asDatabaseConnectionString)
                    Case Is = pcenumDatabaseType.SQLite
                        Me.Connection = New FactEngine.SQLiteConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit, False)
                    Case Is = pcenumDatabaseType.MongoDB
                        Me.Connection = New FactEngine.MongoDbConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.PostgreSQL
                        Me.Connection = New FactEngine.PostgreSQLConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.Snowflake
                        Me.Connection = New FactEngine.SnowflakeConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.TypeDB
                        Me.Connection = New FactEngine.TypeDB.TypeDBConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.ODBC
                        Me.Connection = New FactEngine.ODBCConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.Neo4j
                        Me.Connection = New FactEngine.Neo4jConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.RelationalAI
                        'Me.Connection = New FactEngine.RelationalAIConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.KuzuDB
                        Me.Connection = New FactEngine.KuzuDBConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.EdgeDB
                        Me.Connection = New FactEngine.EdgeDBConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                    Case Is = pcenumDatabaseType.FactEngineSemanticLayer
                        Me.Connection = New FactEngine.Interlink(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
                End Select

                Me.FBMModel.DatabaseConnection = Me.Connection

                Return Me.Connection

            Catch ex As Exception

                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak(ex.Message)

                If abThrowError Then
                    prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, abUseFlashCard:=True)
                End If

                Return Nothing
            End Try

        End Function

    End Class

End Namespace
