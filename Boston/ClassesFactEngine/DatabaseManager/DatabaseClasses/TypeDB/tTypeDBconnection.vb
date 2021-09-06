Imports Boston.FactEngine
Imports Boston.ORMQL

Namespace FactEngine.TypeDB


    Public Class TypeDBConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Public DatabaseConnectionString As String

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer,
                       Optional ByVal abCreatingNewDatabase As Boolean = False)

            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString
            Me.DefaultQueryLimit = aiDefaultQueryLimit

            If abCreatingNewDatabase Then Exit Sub

            Try
                '20210901-VM-For now. Until the gRPC client has been built.
                Me.Connected = True

            Catch ex As Exception
                Me.Connected = False
                Throw New Exception("Could not connect to the database. Check the Model Configuration's Connection String.")
            End Try

        End Sub

        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            lrRecordset.Query = asQuery

            Return lrRecordset

            'Test Grpc            
            Dim lrTypeDBCallInvoker As New TypeDBGrpcCallInvoker()
            Dim lrConnection As New GrpcServer.TypeDB.TypeDBClient(lrTypeDBCallInvoker)


        End Function

        Public Overrides Function GONonQuery(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function


    End Class

End Namespace
