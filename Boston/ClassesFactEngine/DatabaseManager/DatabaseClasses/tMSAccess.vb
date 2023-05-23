Imports System.Threading.Tasks
Imports Boston.ORMQL
Imports System.Reflection

Namespace FactEngine

    Public Class MSAccessConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Private _DbConnection As New ADODB.Connection

        Public Property Connection As ADODB.Connection
            Get
                Return Me._DbConnection
            End Get
            Set(value As ADODB.Connection)
                Me._DbConnection = value
            End Set
        End Property

        Public Overrides Property State As Integer
            Get
                Return Me.Connection.State
            End Get
            Set(value As Integer)
                Me._State = value
            End Set
        End Property

        Public Sub New(ByRef arFBMModel As FBM.Model, ByVal asDatabaseConnectionString As String)
            Me.FBMModel = arFBMModel
            Me.DatabaseConnectionString = asDatabaseConnectionString

            Dim lsDatabaseLocation As String
            Dim lsDataProvider As String
            Dim lsMessage As String

            If asDatabaseConnectionString IsNot Nothing Then
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
                Try
                    Me._DbConnection.Open(Me.DatabaseConnectionString)
                    Me.Connected = True
                Catch
                    MsgBox("Failed To open the Microsoft Access database connection.")
                End Try
            End If

        End Sub

        Public Shadows Function BeginTrans() As Object
            Try
                Me.Connection.BeginTrans()
                Return Nothing
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

                Return Nothing
            End Try

        End Function

        Public Shadows Function CommitTrans() As Object
            Try
                Me.Connection.CommitTrans()
                Return Nothing
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

                Return Nothing
            End Try

        End Function

        Public Overrides Sub Execute(asQuery As String)

            Try
                Call Me.Connection.Execute(asQuery)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

            lrRecordset.Query = asQuery

            '==========================================================
            'Populate the lrRecordset with results from the database
            'Boston.WriteToStatusBar("Connecting to database.", True)

            'Dim lrMSAccessRecordset = Database.getReaderForSQL(lrSQLiteConnection, asQuery)

            'Dim larFact As New List(Of FBM.Fact)
            'Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
            'Dim lrFact As FBM.Fact
            ''Boston.WriteToStatusBar("Reading results.", True)

            ''=====================================================
            ''Column Names   
            ''20200805-VM-To Do
            ''Dim larProjectColumn = lrQueryGraph.getProjectionColumns
            'Dim lsColumnName As String

            'For Each lrProjectColumn In larProjectColumn
            '    lrRecordset.Columns.Add(lrProjectColumn.Name)
            '    lsColumnName = lrFactType.CreateUniqueRoleName(lrProjectColumn.Name, 0)
            '    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
            '    lrFactType.RoleGroup.AddUnique(lrRole)
            'Next

            'For liFieldInd = 0 To lrSQLiteDataReader.FieldCount - 1
            '    lsColumnName = lrFactType.CreateUniqueRoleName(lrSQLiteDataReader.GetName(liFieldInd), 0)
            '    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
            '    lrFactType.RoleGroup.AddUnique(lrRole)
            '    lrRecordset.Columns.Add(lsColumnName)
            'Next

            'While lrSQLiteDataReader.Read()

            '    lrFact = New FBM.Fact(lrFactType, False)
            '    Dim loFieldValue As Object = Nothing
            '    Dim liInd As Integer
            '    For liInd = 0 To lrSQLiteDataReader.FieldCount - 1
            '        Select Case lrSQLiteDataReader.GetFieldType(liInd)
            '            Case Is = GetType(String)
            '                loFieldValue = lrSQLiteDataReader.GetString(liInd)
            '            Case Else
            '                loFieldValue = lrSQLiteDataReader.GetValue(liInd)
            '        End Select

            '        Try
            '            lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(loFieldValue), lrFact))
            '            '=====================================================
            '        Catch
            '            Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
            '        End Try
            '    Next

            '    larFact.Add(lrFact)

            'End While
            'lrRecordset.Facts = larFact

            'lrMSAccessRecordset.Close()

            'Return the ORMQLRecordset
            Return lrRecordset

        End Function

        Private Function iDatabaseConnection_GONonQuery(asQuery As String) As Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function

        Private Function iDatabaseConnection_GOAsync(asQuery As String) As Task(Of Recordset) Implements iDatabaseConnection.GOAsync
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub Open(Optional ByVal asDatabaseConnectionString As String = Nothing)

            Try
                Call Me._DbConnection.Open(asDatabaseConnectionString)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
