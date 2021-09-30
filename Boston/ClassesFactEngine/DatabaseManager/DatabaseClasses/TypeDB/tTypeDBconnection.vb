Imports Boston.FactEngine
Imports Boston.ORMQL
Imports GrpcServer
Imports TypeDBCustom
Imports System.Reflection

Namespace FactEngine.TypeDB


    Public Class TypeDBConnection
        Inherits FactEngine.DatabaseConnection
        Implements FactEngine.iDatabaseConnection

        Private FBMModel As FBM.Model

        Private client As TypeDBCustom.CoreClient = Nothing

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
                client = New CoreClient(Me.FBMModel.Server, CInt(Me.FBMModel.Port))
                Call client.OpenDatabase(Trim(Me.FBMModel.Database))
                Me.Connected = True

            Catch ex As Exception
                Me.Connected = False
                Throw New Exception("Could not connect to the database. Check the Model Configuration's Connection String.")
            End Try

        End Sub

        Public Overrides Function ComparitorOperator(ByVal aiFEQLComparitor As FEQL.pcenumFEQLComparitor) As String

            Select Case aiFEQLComparitor
                Case Is = FEQL.pcenumFEQLComparitor.Bang
                    Return " <> "
                Case Is = FEQL.pcenumFEQLComparitor.Colon,
                          FEQL.pcenumFEQLComparitor.Carret
                    Return " = "
                Case Is = FEQL.pcenumFEQLComparitor.LikeComparitor
                    Return " like "
            End Select

            Return " = "

        End Function

        Public Overrides Function getColumnsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)
            Try
                Dim attributes = client.getAttributes(arTable.Name)

                Dim lsColumnName As String
                Dim lbIsMandatory As Boolean
                Dim lrColumn As New RDS.Column

                '-------------------------------------------------
                'Get the key for the table.
                Dim larKeyAttributesArray = client.getAttributes(arTable.Name, True)
                Dim lasKeyAttribute As New List(Of String)
                For Each attribute In larKeyAttributesArray
                    lasKeyAttribute.Add(attribute.Label)
                Next


                For Each attribute In attributes
                    lsColumnName = attribute.Label
                    lbIsMandatory = lasKeyAttribute.Contains(attribute.Label)
                    lrColumn = New RDS.Column(arTable, lsColumnName, Nothing, Nothing, lbIsMandatory)
                    lrColumn.DataType = New RDS.DataType
                    lrColumn.DataType.DataType = attribute.ValueType
                    lrColumn.DatabaseName = lrColumn.Name

                    larColumn.Add(lrColumn)
                Next

                Return larColumn

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        ''' <summary>
        ''' Returns a list of the Indexes in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overrides Function getIndexesByTable(ByRef arTable As RDS.Table) As List(Of RDS.Index)

            Dim larIndex As New List(Of RDS.Index)

            Try
                Dim larKeyAttributesArray = client.getAttributes(arTable.Name, True)

                Dim lsIndexName As String = ""
                Dim lbIsUnique As Boolean = False
                Dim lbIsPrimaryKey As Boolean = False
                Dim lrIndex As New RDS.Index
                Dim lsQualifier As String = ""
                Dim liIndexSequence As Integer = 1
                Dim lbIgnoreNulls As Boolean = False
                Dim lsColumnName As String = ""
                Dim larColumn As New List(Of RDS.Column)
                Dim lrColumn As RDS.Column = Nothing

                Dim lasIndexNames As New List(Of String)

                For Each lrAttribute In larKeyAttributesArray
                    larColumn.Clear()

                    'Will only be one Column in Key for TypeDB at this stage.                    
                    lsIndexName = arTable.Name & "_PK"
                    lbIsUnique = True
                    lsQualifier = "PK"
                    lbIsPrimaryKey = True
                    liIndexSequence = 1
                    lbIgnoreNulls = False
                    lsColumnName = lrAttribute.Label

                    lrColumn = arTable.Column.Find(Function(x) x.Name = lsColumnName)
                    larColumn.Add(lrColumn)

                Next

                lrIndex = New RDS.Index(arTable,
                                        lsIndexName,
                                        lsQualifier,
                                        pcenumODBCAscendingOrDescending.Ascending,
                                        lbIsPrimaryKey,
                                        lbIsUnique,
                                        lbIgnoreNulls,
                                        larColumn,
                                        True,
                                        False,
                                        True)

                larIndex.Add(lrIndex)

                Return larIndex

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Index)
            End Try

        End Function

        Public Overrides Function getBostonDataTypeByDatabaseDataType(ByVal asDatabaseDataType As String) As pcenumORMDataType

            Try
                asDatabaseDataType = Trim(asDatabaseDataType)
                Dim liIndex As Integer = asDatabaseDataType.IndexOf("(")
                If (liIndex > 0) Then
                    asDatabaseDataType = asDatabaseDataType.Substring(0, liIndex)
                End If

                Dim larDBDataType = From DatabaseDataType In Me.FBMModel.RDS.DatabaseDataType
                                    Where UCase(DatabaseDataType.DataType) = UCase(asDatabaseDataType)
                                    Where Me.FBMModel.TargetDatabaseType.ToString = DatabaseDataType.Database.ToString
                                    Select DatabaseDataType.BostonDataType


                If larDBDataType.Count > 0 Then
                    Return larDBDataType.First
                Else
                    Return pcenumORMDataType.TextVariableLength
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return pcenumORMDataType.TextVariableLength
            End Try

        End Function


        Public Overrides Sub getDatabaseTypes()

            Try
                Dim lsPath = Richmond.MyPath & "\database\databasedatatypes\bostondatabasedatattypes.csv"
                Dim reader As System.IO.TextReader = New System.IO.StreamReader(lsPath)

                Dim csvReader = New CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture)
                Me.FBMModel.RDS.DatabaseDataType = csvReader.GetRecords(Of DatabaseDataType).ToList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)

            Dim larRelation As New List(Of RDS.Relation)
            Try
                Dim lrRecordset As ORMQL.Recordset

                Dim Relations = client.getRelations()

                Dim lrRelation As RDS.Relation = Nothing
                Dim lrDestinationTable As RDS.Table = Nothing
                Dim lrOriginColumn As RDS.Column = Nothing
                Dim lrDestinationColumn As RDS.Column = Nothing
                Dim lasToTableNames As New List(Of String)

                While Not lrRecordset.EOF
                    'https://stackoverflow.com/questions/48508140/how-do-i-get-information-about-foreign-keys-in-sqlite
                    'Columns
                    '====================
                    'id
                    'seq (0 based by table)
                    'table  (To Table)
                    'from   (Column)
                    'to     (To Column)
                    'on_update  ('NO ACTION')
                    'on_delete
                    'match
                    lrDestinationTable = Me.FBMModel.RDS.getTableByName(lrRecordset("table").Data)

                    While Not lrRecordset.EOF

                        lrOriginColumn = arTable.Column.Find(Function(x) x.Name = lrRecordset("from").Data)
                        lrDestinationColumn = lrDestinationTable.Column.Find(Function(x) x.Name = lrRecordset("to").Data)
                        If lrDestinationColumn Is Nothing Then
                            'Try and find the DestinationColumn another way.
                            If lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey) IsNot Nothing Then
                                If lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey).Column.Count = 1 Then
                                    lrDestinationColumn = lrDestinationTable.Index.Find(Function(x) x.IsPrimaryKey).Column.First
                                Else
                                    Throw New Exception("Foreign key from Table, '" & arTable.Name & "', to table, '" & lrDestinationTable.Name & "', has a Column that can not be found in the referenced table. Try making the Column in '" & lrDestinationTable.Name & "' match those in table, " & arTable.Name)
                                End If
                            End If

                        End If

                        If Not lasToTableNames.Contains(lrRecordset("table").Data) Then

                            lrRelation = New RDS.Relation(System.Guid.NewGuid.ToString,
                                                  arTable,
                                                  pcenumCMMLMultiplicity.Many,
                                                  True,
                                                  lrOriginColumn.isPartOfPrimaryKey,
                                                  "involves",
                                                  lrDestinationTable,
                                                  pcenumCMMLMultiplicity.One,
                                                  lrDestinationColumn.IsMandatory,
                                                  "is involed in",
                                                  Nothing)
                            larRelation.Add(lrRelation)
                        End If

                        lrOriginColumn.Relation.Add(lrRelation)
                        lrRelation.OriginColumns.Add(lrOriginColumn)
                        lrRelation.DestinationColumns.Add(lrDestinationColumn)

                        lrRecordset.MoveNext()

                        If Not lrRecordset.EOF Then
                            If lrRecordset("table").Data <> lrDestinationTable.Name Then
                                lrRecordset.CurrentFactIndex -= 1
                                Exit While
                            End If
                            lasToTableNames.AddUnique(lrDestinationTable.Name)
                        End If

                    End While

                    lrRecordset.MoveNext()
                End While


                Return larRelation

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & arTable.Name & ":" & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message & ex.StackTrace
                'prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                'Return New List(Of RDS.Relation)
                Throw New Exception(lsMessage)
            End Try
        End Function

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getTables() As List(Of RDS.Table)

            Dim larTable As New List(Of RDS.Table)
            Try
                Dim lsTableName As String
                Dim lrTable As RDS.Table = Nothing

                Dim Entities = client.getEntities()

                For Each lrEntity In Entities

                    If Not lrEntity.Label = "entity" Then
                        lsTableName = Trim(lrEntity.Label)
                        lrTable = New RDS.Table(Me.FBMModel.RDS, lsTableName, Nothing)

                        Dim larRoleAttributes = client.getPlays(lsTableName)
                        For Each lrRoleAttribute In larRoleAttributes
                            Dim lrPlays As New RDS.Plays(lrRoleAttribute.Scope, lrRoleAttribute.Label)
                        Next

                        larTable.Add(lrTable)
                    End If
                Next

                Dim Relations = client.getRelations()

                For Each lrRelation In Relations

                    Dim larAttributes = client.getAttributes(lrRelation.Label)
                    If larAttributes.Count > 0 Then
                        lsTableName = Trim(lrRelation.Label)
                        lrTable = New RDS.Table(Me.FBMModel.RDS, lsTableName, Nothing)

                        larTable.Add(lrTable)
                    End If
                Next

                Return larTable

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Table)
            End Try


        End Function

        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                lrRecordset.Query = asQuery

                'From Previous TypeDB DLL
                'Dim task As System.Threading.Tasks.Task(Of ORMQL.Recordset) = Me.GOAsync(asQuery)
                'lrRecordset = task.Run(Async Function() Await Me.GOAsync(asQuery)).Result

                Dim rsult = client.ExecuteQuery(lrRecordset.Query)
                Try

                    Dim larFact As New List(Of FBM.Fact)
                    Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                    Dim lrFact As FBM.Fact
                    Dim lsColumnName As String

                    For Each conc In rsult

                        lrFact = New FBM.Fact(lrFactType, False)
                        Dim cncpts As Concept() = New Concept(conc.Map.Count - 1) {}
                        'For i = 0 To conc.Map.Count - 1
                        'conc.Map.TryGetValue(conc.Map.Keys(i), cncpts(i)) 'you can get the maping key from your query

                        If lrRecordset.Columns.Count = 0 Then
                                For Each loValue In conc.Map.Values
                                    Try
                                        lsColumnName = lrFactType.CreateUniqueRoleName(loValue.Thing.[Type].Label.ToString, 0)
                                    Catch ex As Exception
                                        lsColumnName = lrFactType.CreateUniqueRoleName(loValue.[Type].Label.ToString, 0)
                                    End Try

                                    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                                    lrFactType.RoleGroup.AddUnique(lrRole)
                                    lrRecordset.Columns.Add(lsColumnName)
                                Next
                            End If

                            Dim loFieldValue As Object = Nothing
                            Dim liInd As Integer = 0

                            For Each loValue In conc.Map.Values

                                Select Case loValue.ConceptCase
                                    Case Is = Concept.ConceptOneofCase.Thing

                                        Select Case loValue.Thing.Type.ValueType.ToString
                                            Case Is = "Long"
                                                loFieldValue = loValue.Thing.Value.Long
                                            Case Is = "Datetime"
                                                loFieldValue = DateTimeOffset.FromUnixTimeMilliseconds(loValue.Thing.Value.DateTime).ToString
                                            Case Else
                                                Try
                                                    loFieldValue = loValue.Thing.Value.String
                                                Catch ex As Exception
                                                    loFieldValue = loValue.Thing.ToString
                                                    'Not a biggie at this stage.
                                                End Try
                                        End Select
                                    Case Is = Concept.ConceptOneofCase.Type
                                        loFieldValue = loValue.Type.Label
                                        'Dim loObject As Object = GrpcServer.Transaction.
                                        Try
                                            Dim loEncoding As String = loValue.Type.Encoding.ToString
                                            Dim loValueType As String = loValue.Type.ValueType.ToString
                                            If Not (loEncoding.StartsWith("Attribute") Or loEncoding.StartsWith("Relation")) Then
                                                'Debugger.Break()
                                            End If

                                        Catch ex As Exception
                                            'Not a biggie at this stage.
                                        End Try
                                    Case Else
                                        'Debugger.Break()
                                End Select

                                Try
                                    lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(Viev.NullVal(loFieldValue, "")), lrFact))
                                Catch
                                    Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                                End Try
                                liInd += 1
                            Next 'Column Value

                            larFact.Add(lrFact)

                            If larFact.Count = Me.DefaultQueryLimit Then
                                lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                                Exit For
                            End If

                        Next
                    'Erase cncpts
                    'Next

                    lrRecordset.Facts = larFact
                    Return lrRecordset

                Catch ex As Exception
                    lrRecordset.ErrorString = ex.Message
                    Return lrRecordset
                End Try

                Return lrRecordset
            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset
            End Try

        End Function

        Public Overrides Async Function GOAsync(asQuery As String) As Threading.Tasks.Task(Of ORMQL.Recordset) Implements iDatabaseConnection.GOAsync

            Dim lrRecordset As New ORMQL.Recordset

            'this is the client we build in C#             

            Try
                lrRecordset.Query = asQuery

                Dim larFact As New List(Of FBM.Fact)
                Dim lrFactType = New FBM.FactType(Me.FBMModel, "DummyFactType", True)
                Dim lrFact As FBM.Fact = Nothing

                Dim lsColumnName As String = ""

                '=================================================================================================================
                'TypeDB specific.
                'make connection to server
                client = New TypeDBCustom.CoreClient(Me.FBMModel.Server, CInt(Me.FBMModel.Port))

                'Create Session. Connect to the database and create session + pulse automatically.
                Call client.OpenDatabase(Trim(Me.FBMModel.Database))

                'this is how we setup a match query, for different query type you have to use different property of query object
                Dim query As New QueryManager.Types.Req()
                query.MatchReq = New QueryManager.Types.Match.Types.Req() With {.Query = Trim(asQuery)}
                query.Options = New Options() With {.Parallel = True}

                'clear the existing transactions if there are any.
                client.transactionClient.Reqs.Clear()
                'you can add multiple transaction queries at once
                client.transactionClient.Reqs.Add(New Transaction.Types.Req() With {.QueryManagerReq = query, .ReqId = client.SessionID})
                'write the transaction to bi-directional stream    

                '20210930-VM-Old DLL
                'Await client.Transactions.RequestStream.WriteAsync(client.transactionClient)

                'Dim ServerResp As Transaction.Types.Server = Nothing
                ''this is like an enumrator, you have to call MoveNext for every chunk of data you will receive
                'Do While Await client.Transactions.ResponseStream.MoveNext(Threading.CancellationToken.None)
                '    ServerResp = client.Transactions.ResponseStream.Current 'set the current enumrator object to local so can access it shortly

                '    'this is check if the stream have done sending the data and we exit the loop,
                '    'if this miss you will stuck on MoveNext 
                '    If ServerResp.ResPart.ResCase = Transaction.Types.ResPart.ResOneofCase.StreamResPart AndAlso
                '        ServerResp.ResPart.StreamResPart.State = Transaction.Types.Stream.Types.State.Done Then
                '        Exit Do
                '    End If

                '    'this will be different according to your scenario. you need to use breakpoint to see data
                '    'to implement better logic here. "Answers" below is the array of ConceptMap
                '    Try
                '        Dim laoMap As List(Of ConceptMap) = ServerResp.ResPart.QueryManagerResPart.MatchResPart.Answers.ToList
                '        For Each itm In laoMap
                '            Dim cncpt As Concept = Nothing 'this will be used to get the concept from conceptMap so you can access values.
                '            itm.Map.TryGetValue("gen", cncpt) 'you can get the maping key from your query

                '            If lrRecordset.Columns.Count = 0 Then
                '                For Each loValue In itm.Map.Values
                '                    Try
                '                        lsColumnName = lrFactType.CreateUniqueRoleName(loValue.Thing.[Type].Label.ToString, 0)
                '                    Catch ex As Exception
                '                        lsColumnName = lrFactType.CreateUniqueRoleName(loValue.[Type].Label.ToString, 0)
                '                    End Try

                '                    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                '                    lrFactType.RoleGroup.AddUnique(lrRole)
                '                    lrRecordset.Columns.Add(lsColumnName)
                '                Next
                '            End If

                '            lrFact = New FBM.Fact(lrFactType, False)
                '            Dim loFieldValue As Object = Nothing
                '            Dim liInd As Integer = 0

                '            For Each loValue In itm.Map.Values

                '                Select Case loValue.ConceptCase
                '                    Case Is = Concept.ConceptOneofCase.Thing

                '                        Select Case loValue.Thing.Type.ValueType.ToString
                '                            Case Is = "Long"
                '                                loFieldValue = loValue.Thing.Value.Long
                '                            Case Is = "Datetime"
                '                                loFieldValue = DateTimeOffset.FromUnixTimeMilliseconds(loValue.Thing.Value.DateTime).ToString
                '                            Case Else
                '                                Try
                '                                    loFieldValue = loValue.Thing.Value.String
                '                                Catch ex As Exception
                '                                    loFieldValue = loValue.Thing.ToString
                '                                    'Not a biggie at this stage.
                '                                End Try
                '                        End Select
                '                    Case Is = Concept.ConceptOneofCase.Type
                '                        loFieldValue = loValue.Type.Label
                '                        'Dim loObject As Object = GrpcServer.Transaction.
                '                        Try
                '                        Dim loEncoding As String = loValue.Type.Encoding.ToString
                '                            Dim loValueType As String = loValue.Type.ValueType.ToString
                '                            If Not (loEncoding.StartsWith("Attribute") Or loEncoding.StartsWith("Relation")) Then
                '                                'Debugger.Break()
                '                            End If

                '                        Catch ex As Exception
                '                            'Not a biggie at this stage.
                '                        End Try
                '                    Case Else
                '                        'Debugger.Break()
                '                End Select

                '                Try
                '                    lrFact.Data.Add(New FBM.FactData(lrFactType.RoleGroup(liInd), New FBM.Concept(Viev.NullVal(loFieldValue, "")), lrFact))
                '                Catch
                '                    Throw New Exception("Tried to add a recordset Column that is not in the Project Columns. Column Index: " & liInd)
                '                End Try
                '                liInd += 1
                '            Next 'Column Value

                '            larFact.Add(lrFact)

                '            If larFact.Count = Me.DefaultQueryLimit Then
                '                lrRecordset.Warning.Add("Query limit of " & Me.DefaultQueryLimit.ToString & " reached.")
                '                Exit For
                '            End If
                '        Next
                '    Catch ex As Exception
                '        Exit Do 'Not a biggie at this stage.
                '    End Try
                'Loop

                lrRecordset.Facts = larFact
                Return lrRecordset

            Catch ex As Exception
                lrRecordset.ErrorString = ex.Message
                Return lrRecordset

            End Try

        End Function

        Public Overrides Function GONonQuery(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GONonQuery
            Throw New NotImplementedException()
        End Function

    End Class

End Namespace
