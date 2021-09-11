Imports Boston.FactEngine
Imports Boston.ORMQL
Imports GrpcServer
Imports TypeDBCustom

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
                '20210901-VM-For now. Until the gRPC client has been built.
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

        Public Overrides Function GO(asQuery As String) As ORMQL.Recordset Implements iDatabaseConnection.GO

            Dim lrRecordset As New ORMQL.Recordset

            Try
                Dim task As System.Threading.Tasks.Task(Of ORMQL.Recordset) = Me.GOAsync(asQuery)
                'task.Wait()
                'lrRecordset = task.Result

                lrRecordset = task.Run(Async Function() Await Me.GOAsync(asQuery)).Result

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
                Await client.Transactions.RequestStream.WriteAsync(client.transactionClient)

                Dim ServerResp As Transaction.Types.Server = Nothing
                'this is like an enumrator, you have to call MoveNext for every chunk of data you will receive
                Do While Await client.Transactions.ResponseStream.MoveNext(Threading.CancellationToken.None)
                    ServerResp = client.Transactions.ResponseStream.Current 'set the current enumrator object to local so can access it shortly

                    'this is check if the stream have done sending the data and we exit the loop,
                    'if this miss you will stuck on MoveNext 
                    If ServerResp.ResPart.ResCase = Transaction.Types.ResPart.ResOneofCase.StreamResPart AndAlso
                        ServerResp.ResPart.StreamResPart.State = Transaction.Types.Stream.Types.State.Done Then
                        Exit Do
                    End If

                    'this will be different according to your scenario. you need to use breakpoint to see data
                    'to implement better logic here. "Answers" below is the array of ConceptMap
                    Try
                        For Each itm In ServerResp.ResPart.QueryManagerResPart.MatchResPart.Answers.ToArray()
                            Dim cncpt As Concept = Nothing 'this will be used to get the concept from conceptMap so you can access values.
                            itm.Map.TryGetValue("gen", cncpt) 'you can get the maping key from your query

                            If lrRecordset.Columns.Count = 0 Then
                                For Each loValue In itm.Map.Values
                                    lsColumnName = lrFactType.CreateUniqueRoleName(loValue.Thing.[Type].Label.ToString, 0)
                                    Dim lrRole = New FBM.Role(lrFactType, lsColumnName, True, Nothing)
                                    lrFactType.RoleGroup.AddUnique(lrRole)
                                    lrRecordset.Columns.Add(lsColumnName)
                                Next
                            End If

                            lrFact = New FBM.Fact(lrFactType, False)
                            Dim loFieldValue As Object = Nothing
                            Dim liInd As Integer = 0

                            For Each loValue In itm.Map.Values

                                Select Case loValue.Thing.Type.ValueType.ToString
                                    Case Is = "Long"
                                        loFieldValue = loValue.Thing.Value.Long
                                    Case Else
                                        loFieldValue = loValue.Thing.Value.String
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
                    Catch ex As Exception
                        Exit Do 'Not a biggie at this stage.
                    End Try
                Loop

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
