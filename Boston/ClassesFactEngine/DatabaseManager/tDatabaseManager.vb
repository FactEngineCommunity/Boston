Imports Boston.ORMQL

Namespace FactEngine
    Public Class DatabaseManager

        Public DatabaseType As pcenumDatabaseType = pcenumDatabaseType.None

        Public FBMModel As FBM.Model

        Private Connection As FactEngine.DatabaseConnection = Nothing

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

            lrRecordset = Me.Connection.GO(asQuery)

            Return lrRecordset

        End Function

        Public Sub establishConnection(ByVal aiDatabaseType As pcenumDatabaseType, ByVal asDatabaseConnectionString As String)

            Select Case aiDatabaseType
                Case Is = pcenumDatabaseType.SQLite
                    Me.Connection = New FactEngine.SQLiteConnection(Me.FBMModel, asDatabaseConnectionString)
            End Select

        End Sub

    End Class

End Namespace
