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

        Public DBDataTypes As New List(Of DatabaseDataType)

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

            Call Me.getDatabaseDataTypes

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

                Return Nothing
            End Try

        End Function

        Public Shadows Function RollbackTrans() As Object
            Try
                Me.Connection.RollbackTrans()
                Return Nothing
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning,, False,, True,, True, ex)

                Return Nothing
            End Try

        End Function

        Public Sub CreateLinkTable(odbcDSN As String, tableName As String)

            Try
                Dim loCatalog As New ADOX.Catalog
                loCatalog.ActiveConnection = Me.Connection

                ' Create the link
                Dim tbl As New ADOX.Table
                tbl.Name = tableName
                tbl.ParentCatalog = loCatalog

                ' Set the properties you found
                tbl.Properties.Item("Jet OLEDB:Create Link").Value = True
                tbl.Properties.Item("Jet OLEDB:Link Provider String").Value = "ODBC;DSN=" & odbcDSN & ";"
                tbl.Properties.Item("Jet OLEDB:Remote Table Name").Value = tableName

                ' Append the table to the catalog (this creates the link)
                loCatalog.Tables.Append(tbl)

                loCatalog = Nothing
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Overloads Function Execute(ByVal asQuery As String, Optional ByVal abIgnoreErrors As Boolean = False) As ORMQL.Recordset

            Dim lrRecordset As New ORMQL.Recordset

            Try
                Me.Connection.Execute(asQuery)

                Return lrRecordset
            Catch ex As Exception

                lrRecordset.ErrorString = ex.Message

                If abIgnoreErrors Then Return lrRecordset

                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                Dim lbUseFlashcard As Boolean = (My.Settings.DebugMode <> "Debug")

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,, lbUseFlashcard,, lbUseFlashcard, ex)

                Return New ORMQL.Recordset

            End Try
        End Function

        ''' <summary>
        ''' Generates a CREATE TABLE Statement for the given Table, specific to the database type.
        ''' </summary>
        ''' <param name="arTable">The RDS Table for which the SQL CREATE statement is to be generated.</param>
        ''' <param name="asTableName">Optional table name for the table in the CREATE statement.</param>
        ''' <returns></returns>
        Public Overrides Function generateCREATETABLEStatement(ByRef arTable As RDS.Table,
                                                               Optional asTableName As String = Nothing,
                                                               Optional abIgnoreColumnISNOTNULL As Boolean = False) As String

            Try
                Dim lsSQLCommand As String = ""

                If asTableName Is Nothing Then
                    lsSQLCommand = "CREATE TABLE [" & arTable.Name & "]"
                Else
                    lsSQLCommand = "CREATE TABLE [" & asTableName & "]"
                End If
                lsSQLCommand &= " ("
                'Column defs
                Dim liInd = 0
                For Each lrColumn In arTable.Column
                    If liInd > 0 Then lsSQLCommand &= ","
                    lsSQLCommand &= Me.generateSQLColumnDefinition(lrColumn) & vbCrLf
                    liInd += 1
                Next
                'Primary Key
                If arTable.getPrimaryKeyColumns.Count > 0 Then
                    lsSQLCommand &= ", CONSTRAINT [" & arTable.Name.RemoveWhitespace & "_PK] PRIMARY KEY ("
                    liInd = 0
                    For Each lrColumn In arTable.getPrimaryKeyColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.Name & "]"
                        liInd += 1
                    Next
                    lsSQLCommand &= ")"
                    If arTable.isPGSRelation Then
                        lsSQLCommand &= " /* { Label:""" & arTable.DBName & """} */"
                    End If
                    lsSQLCommand &= vbCrLf
                End If
                'Unique Indexes
                If arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey).Count > 0 Then
                    liInd = 1
                    For Each lrIndex In arTable.Index.FindAll(Function(x) Not x.IsPrimaryKey)
                        lsSQLCommand &= ", CONSTRAINT " & lrIndex.Name & " UNIQUE ("
                        Dim liInd2 = 0
                        For Each lrColumn In lrIndex.Column
                            If liInd2 > 0 Then lsSQLCommand &= ","
                            lsSQLCommand &= "[" & lrColumn.Name & "]"
                            liInd2 += 1
                        Next
                        lsSQLCommand &= ")" & vbCrLf
                    Next
                End If
                'Foreign Keys
                For Each lrRelation In arTable.getOutgoingRelations '.FindAll(Function(x) x.OriginColumns.Count > 1)
                    lsSQLCommand &= ", FOREIGN KEY ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.Name & "]"
                        liInd += 1
                    Next
                    lsSQLCommand &= ") REFERENCES [" & lrRelation.DestinationTable.Name & "] ("
                    liInd = 0
                    For Each lrColumn In lrRelation.OriginColumns
                        If liInd > 0 Then lsSQLCommand &= ","
                        lsSQLCommand &= "[" & lrColumn.getReferencedColumn.Name & "]"
                        liInd += 1
                    Next
                    lsSQLCommand &= ")"
                    lsSQLCommand &= " ON DELETE CASCADE ON UPDATE CASCADE"
                    lsSQLCommand &= " /* { Label:""" & lrRelation.ResponsibleFactType.DBName & """} */" & vbCrLf
                    lsSQLCommand &= vbCrLf
                Next
                lsSQLCommand &= ")"

                Return lsSQLCommand
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

        Public Overrides Function generateSQLColumnDefinition(ByRef arColumn As RDS.Column,
                                                              Optional abIgnoreColumnISNOTNULL As Boolean = False) As String
            Try

                Dim lsSQLColumnDefinition As String
                Dim lrColumn As RDS.Column = arColumn


                lsSQLColumnDefinition = arColumn.Name

                If arColumn.ActiveRole.FactType.RoleGroup.Count = 1 Then
                    lsSQLColumnDefinition &= " INTEGER"
                Else
                    lsSQLColumnDefinition &= " " & Me.getDBDataType(arColumn.ActiveRole.JoinsValueType.DataType)
                    If arColumn.ActiveRole.JoinsValueType.DataTypeLength > 0 Then
                        If arColumn.DataTypeIsText Then
                            lsSQLColumnDefinition &= "(" & arColumn.ActiveRole.JoinsValueType.DataTypeLength.ToString
                            If arColumn.ActiveRole.JoinsValueType.DataTypePrecision > 0 Then
                                lsSQLColumnDefinition &= arColumn.ActiveRole.JoinsValueType.DataTypePrecision.ToString
                            End If
                            lsSQLColumnDefinition &= ")"
                        End If
                    End If

                    Dim larOutgoingRelation = arColumn.Relation.FindAll(Function(x) x.OriginTable Is lrColumn.Table)
                    If larOutgoingRelation.Count > 0 Then
                        If larOutgoingRelation(0).OriginColumns.Count = 1 Then
                            lsSQLColumnDefinition &= " REFERENCES [" & larOutgoingRelation(0).DestinationTable.Name & "]"
                        End If
                        If arColumn.Role.Mandatory Then lsSQLColumnDefinition &= " NOT NULL"
                    ElseIf arColumn.Role.Mandatory Then
                        lsSQLColumnDefinition &= " NOT NULL"
                    End If

                End If

                Return lsSQLColumnDefinition

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        ''' <summary>
        ''' Returns a list of the Relations/ForeignKeys in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)
            Return New List(Of RDS.Relation)
        End Function

        Public Function getDBDataType(ByVal aiORMDataType As pcenumORMDataType) As String

            Try

                Dim larDBDataType = From DatabaseDataType In Me.DBDataTypes
                                    Where DatabaseDataType.BostonDataType = aiORMDataType
                                    Where DatabaseDataType.Database = pcenumDatabaseType.MSJet
                                    Select DatabaseDataType

                Return larDBDataType.First.DataType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return "Error"
            End Try

        End Function

        Public Overrides Sub getDatabaseDataTypes()

            Try
                Dim lsPath = Boston.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)

                Me.DBDataTypes = csvReader.GetRecords(Of DatabaseDataType).ToList

                If Me.FBMModel IsNot Nothing Then
                    Me.FBMModel.RDS.DatabaseDataType = Me.DBDataTypes
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


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

        Public Overrides Function Open(Optional ByVal asDatabaseConnectionString As String = Nothing) As Boolean

            Try
                Call Me._DbConnection.Open(asDatabaseConnectionString)
                Return True
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
                Return False
            End Try

        End Function

        ''' <summary>
        ''' Creates or Recreates the Table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overrides Sub recreateTable(ByRef arTable As RDS.Table)

            Try
                Dim lsSQLQuery As String = ""
                Dim lasColumnNames = From Column In arTable.Column
                                     Select Column.Name

                Dim lsColumnList = String.Join(",", lasColumnNames)


                Dim lrSQLiteConnection = Database.CreateConnection(Me.DatabaseConnectionString)


                Try

                    Try
                        lsSQLQuery = Me.generateCREATETABLEStatement(arTable, arTable.Name)
                        Me.Execute(lsSQLQuery)
                    Catch ex As Exception
                        Throw New Exception("Couldn't create Table: " & arTable.Name)
                    End Try

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Function iDatabaseConnection_GOAbstractionLayer(asQuery As String) As Recordset Implements iDatabaseConnection.GOAbstractionLayer
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace
