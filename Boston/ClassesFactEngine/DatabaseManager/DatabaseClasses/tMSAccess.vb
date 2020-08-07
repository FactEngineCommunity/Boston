Imports Boston.ORMQL

Namespace FactEngine

    Public Class MSAccessConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Private DbConnection As New ADODB.Connection

        Public DatabaseConnectionString As String

        Public Sub New(ByRef arFBMModel As FBM.Model, ByVal asDatabaseConnectionString As String)
            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString

            Dim lsDatabaseLocation As String
            Dim lsDataProvider As String
            Dim lsMessage As String

            Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
            lrSQLConnectionStringBuilder.ConnectionString = Me.DatabaseConnectionString

            lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")
            lsDataProvider = lrSQLConnectionStringBuilder("Provider")

            If Not System.IO.File.Exists(lsDatabaseLocation) Then

                lsMessage = "Cannot find the database at the configured location:"
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= lsDatabaseLocation
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Manage the Database ConnectionString for the datase in the Model's configuration"

                MsgBox(lsMessage)

            End If

            '------------------------------------------------
            'Open the (database) connection
            '------------------------------------------------
            Me.DbConnection.Open(Me.DatabaseConnectionString)

        End Sub

        Public Overrides Function GO(asQuery As String) As Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            lrRecordset.Query = asQuery

            '==========================================================
            'Populate the lrRecordset with results from the database
            'Richmond.WriteToStatusBar("Connecting to database.", True)
            Dim lrSQLiteDataReader = Database.getReaderForSQL(lrSQLiteConnection, asQuery)

            Dim larFact As New List(Of FBM.Fact)
            Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
            Dim lrFact As FBM.Fact
            'Richmond.WriteToStatusBar("Reading results.", True)

            '=====================================================
            'Column Names   
            '20200805-VM-To Do
            'Dim larProjectColumn = lrQueryGraph.getProjectionColumns
            Dim lsColumnName As String

            'For Each lrProjectColumn In larProjectColumn
            '    lrRecordset.Columns.Add(lrProjectColumn.Name)
            '    lsColumnName = lrFactType.CreateUniqueRoleName(lrProjectColumn.Name, 0)
            '    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
            '    lrFactType.RoleGroup.AddUnique(lrRole)
            'Next

            For liFieldInd = 0 To lrSQLiteDataReader.FieldCount - 1
                lsColumnName = lrFactType.CreateUniqueRoleName(lrSQLiteDataReader.GetName(liFieldInd), 0)
                Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                lrFactType.RoleGroup.AddUnique(lrRole)
                lrRecordset.Columns.Add(lsColumnName)
            Next

            While lrSQLiteDataReader.Read()

                lrFact = New FBM.Fact(lrFactType, False)
                Dim loFieldValue As Object = Nothing
                Dim liInd As Integer
                For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
                    Select Case lrSQLiteDataReader.GetFieldType(liInd)
                        Case Is = GetType(String)
                            loFieldValue = lrSQLiteDataReader.GetString(liInd)
                        Case Else
                            loFieldValue = lrSQLiteDataReader.GetValue(liInd)
                    End Select

                    Try
                        lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                        '=====================================================
                    Catch
                        Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                    End Try
                Next

                larFact.Add(lrFact)

            End While
            lrRecordset.Facts = larFact
            lrSQLiteConnection.Close()

            'Run the SQL against the database
            Return lrRecordset

        End Function

    End Class


End Namespace
