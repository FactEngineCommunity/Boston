Imports Boston.ORMQL
Imports System.Data.Odbc

Namespace FactEngine

    Public Class ODBCConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Public DatabaseConnectionString As String

        Public ODBCConnection As System.Data.Odbc.OdbcConnection

        Public Sub New(ByRef arFBMModel As FBM.Model,
                       ByVal asDatabaseConnectionString As String,
                       ByVal aiDefaultQueryLimit As Integer)

            Try
                Me.FBMModel = arFBMModel
                Me.DatabaseConnectionString = asDatabaseConnectionString
                Me.DefaultQueryLimit = aiDefaultQueryLimit

                Me.ODBCConnection = New System.Data.Odbc.OdbcConnection(Me.DatabaseConnectionString)
                Try
                    Me.ODBCConnection.Open()
                    Me.Connected = True
                Catch
                    MsgBox("Failed To open the MongoDB database connection.")
                End Try

            Catch ex As Exception

            End Try

        End Sub

        ''' <summary>
        ''' Returns a list of the Relations/ForeignKeys in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)
            Return New List(Of RDS.Relation)
        End Function

        ''' <summary>
        ''' Returns a list of the Indexes in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getIndexesByTable(ByRef arTable As RDS.Table) As List(Of RDS.Index)
            Return New List(Of RDS.Index)
        End Function

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getTables() As List(Of RDS.Table)
            Return New List(Of RDS.Table)
        End Function

        Public Overrides Function GO(asQuery As String) As Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                '==========================================================
                'Populate the lrRecordset with results from the database
                'Richmond.WriteToStatusBar("Connecting to database.", True)

                Dim adapter As OdbcDataAdapter = New OdbcDataAdapter(asQuery, Me.ODBCConnection)
                Dim lrDataSet As New DataSet

                adapter.Fill(lrDataSet)

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact

                '=====================================================
                'Column Names   

                For Each lrColumn In lrDataSet.Tables(0).Columns
                    Dim lrRole = New FBM.Role(lrFactType, lrColumn.ToString, True, Nothing)
                    lrFactType.RoleGroup.AddUnique(lrRole)
                    lrRecordset.Columns.Add(lrColumn.ToString)
                Next

                For Each lrRow As DataRow In lrDataSet.Tables(0).Rows

                    lrFact = New FBM.Fact(lrFactType, False)
                    Dim loFieldValue As Object = Nothing
                    Dim liInd As Integer
                    For liInd = 0 To lrRow.ItemArray.Count - 1
                        loFieldValue = lrRow.Item(liInd).ToString

                        Try
                            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
                            '=====================================================
                        Catch
                            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                        End Try
                    Next

                    larFact.Add(lrFact)

                    If larFact.Count = Me.DefaultQueryLimit Then
                        lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                        Exit For
                    End If

                Next

                lrRecordset.Facts = larFact

                'Run the SQL against the database
                Return lrRecordset
            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        Private Function iDatabaseConnection_GONonQuery(asQuery As String) As Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
