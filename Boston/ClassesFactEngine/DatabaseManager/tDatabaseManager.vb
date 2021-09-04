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

        Public Function GO(ByVal asQuery As String) As ORMQL.Recordset

            Dim lrRecordset As ORMQL.Recordset

            If Me.Connection Is Nothing Then Throw New Exception("The connection to the database has been lost. Close and reopen FactEngine.")

            lrRecordset = Me.Connection.GO(asQuery)

            Return lrRecordset

        End Function

        Public Function GONonQuery(ByVal asQuery As String) As ORMQL.Recordset

            Dim lrRecordset As ORMQL.Recordset

            If Me.Connection Is Nothing Then Throw New Exception("The connection to the database has been lost. Close and reopen FactEngine.")

            lrRecordset = Me.Connection.GONonQuery(asQuery)

            Return lrRecordset

        End Function

        Public Function establishConnection(ByVal aiDatabaseType As pcenumDatabaseType,
                                            ByVal asDatabaseConnectionString As String) As FactEngine.DatabaseConnection

            Try

                Select Case aiDatabaseType
                    Case Is = pcenumDatabaseType.SQLite
                        Me.Connection = New FactEngine.SQLiteConnection(Me.FBMModel, asDatabaseConnectionString, My.Settings.FactEngineDefaultQueryResultLimit)
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
                End Select

                Me.FBMModel.DatabaseConnection = Me.Connection

                Return Me.Connection

            Catch ex As Exception

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Class

End Namespace
