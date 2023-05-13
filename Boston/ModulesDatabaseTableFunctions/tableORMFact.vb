Imports System.Reflection

Namespace TableFact

    Module TableFact

        Public Sub AddFact(ByVal arFact As FBM.Fact)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFact"
                lsSQLQuery &= " VALUES("
                lsSQLQuery &= pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= "," & pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= " ,'" & arFact.Model.ModelId & "'"
                lsSQLQuery &= " ,'" & arFact.Symbol & "'"
                lsSQLQuery &= " ,'" & arFact.FactType.Id & "'"
                lsSQLQuery &= " )"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFact.AddFact"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

        ''' <summary>
        ''' Deletes a Fact from the database.
        ''' </summary>
        ''' <param name="arFact"></param>
        ''' <param name="abIgnoreErrors">In DuplexClient Client/Server mode can raise 'Record is deleted' error. Set to True if this persists</param>
        Public Sub DeleteFact(ByVal arFact As FBM.Fact, Optional ByVal abIgnoreErrors As Boolean = False)

            Try

                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM MetaModelFact"
                lsSQLQuery &= " WHERE Symbol = '" & arFact.Symbol & "'" 'Symbol is FactId
                lsSQLQuery &= " AND ModelId = '" & arFact.Model.ModelId & "'"

                pdbConnection.BeginTrans()

                '----------------------------------------------
                'Delete the FactData associated with the Fact
                '----------------------------------------------
                Call TableFactData.DeleteFactDataByFact(arFact)

                pdbConnection.Execute(lsSQLQuery)

                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message

                pdbConnection.RollbackTrans()
                If Not abIgnoreErrors Then prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ExistsFact(ByVal arFact As FBM.Fact) As Boolean

            '------------------------------------------------------------------
            'The FactTypeId or RowId is the Symbol attribute in the Fact table
            '------------------------------------------------------------------
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelFact"
                lsSQLQuery &= " WHERE Symbol = '" & Trim(arFact.Symbol) & "'"
                lsSQLQuery &= " AND ModelId = '" & arFact.Model.ModelId & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsFact = True
                Else
                    ExistsFact = False
                End If

                lREcordset.Close()
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub GetFactsForFactType(ByRef arFactType As FBM.FactType) 'As List(Of FBM.Fact)

            Dim lrFact As FBM.Fact
            Dim lsMessage As String

            Dim lsSQLQuery As String
            Dim lRecordset As New RecordsetProxy

            Try

                lRecordset.ActiveConnection = pdbConnection
                lRecordset.CursorType = pcOpenStatic

                lsSQLQuery = "  SELECT f.Symbol, fd.RoleId, fd.ValueSymbol"
                lsSQLQuery &= "   FROM MetaModelFact f,"
                lsSQLQuery &= "        MetaModelFactData fd"
                lsSQLQuery &= "  WHERE f.FactTypeId = '" & Trim(CStr(arFactType.Id)) & "'"
                lsSQLQuery &= "    AND f.ModelId = '" & arFactType.Model.ModelId & "'"
                lsSQLQuery &= "    AND fd.FactSymbol = f.Symbol"
                lsSQLQuery &= "    AND fd.ModelId = f.ModelId"
                lsSQLQuery &= "  ORDER BY f.Symbol, fd.RoleId"

                lRecordset.Open(lsSQLQuery)

                Dim lrRoleDictionary As New Dictionary(Of String, Integer)
                For liInd = 1 To arFactType.Arity
                    lrRoleDictionary.Add(arFactType.RoleGroup(liInd - 1).Id, liInd - 1)
                Next

                If Not lRecordset.EOF Then

                    Dim lrDictionaryEntry As FBM.DictionaryEntry
                    Dim lrFactDictionaryEntry As FBM.DictionaryEntry '20220129-VM-Removed. = New FBM.DictionaryEntry(arFactType.Model, "DummyFactId", pcenumConceptType.Fact)                
                    Dim liInd As Integer
                    Dim lrRole As FBM.Role
                    Dim larRoleGroup As List(Of FBM.Role) = arFactType.RoleGroup

                    While Not lRecordset.EOF
                        lrFact = New FBM.Fact(lRecordset("Symbol").Value, arFactType, False)
                        For liInd = 1 To arFactType.Arity

                            lrRole = larRoleGroup(lrRoleDictionary(lRecordset("RoleId").Value)) '.Find(Function(x) x.Id = lRecordset("RoleId").Value)

                            '--------------------------------------------------------------------------------------------------
                            'Get the Concept from the ModelDictionary so that FactData objects are linked directly to the Concept/Value in the ModelDictionary
                            '--------------------------------------------------------------------------------------------------
                            lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(arFactType.Model, lRecordset("ValueSymbol").Value, pcenumConceptType.Value),
                                                                                         , False,,, True, True)

                            Dim lrFactData As New FBM.FactData(lrRole, lrDictionaryEntry.Concept, lrFact, False)
                            '-----------------------------
                            'Add the RoleData to the Fact
                            '-----------------------------
                            lrFact.Data.Add(lrFactData)
                            '-------------------------------------
                            'Add the RoleData to the Role as well
                            '-------------------------------------
                            lrRole.Data.Add(lrFactData)

                            lrRole.JoinedORMObject.Instance.AddUnique(lrFactData.Data)

                            lRecordset.MoveNext()
                        Next liInd

                        '--------------------------------------------------------------------------------------------------
                        'Get the Concept from the ModelDictionary so that Facts are linked directly to the Concept/Value.
                        '--------------------------------------------------------------------------------------------------
                        lrFactDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(New FBM.DictionaryEntry(arFactType.Model, lrFact.Id, pcenumConceptType.Fact),
                                                                                         True,
                                                                                         False,
                                                                                         False,
                                                                                         False,'Not straight save. Because should have been loaded with the ModelDictionary
                                                                                         True,
                                                                                         True)
                        lrFactDictionaryEntry.LongDescription = lrFact.LongDescription


                        '----------------------------------------------------------------------------------------------------------
                        'If the FactType of the Fact is Objectified, add the Fact.Id as an instance of the ObjectifyingEntityType
                        '----------------------------------------------------------------------------------------------------------
                        If arFactType.IsObjectified And arFactType.ObjectifyingEntityType IsNot Nothing Then
                            arFactType.ObjectifyingEntityType.Instance.Add(lrFact.Id)
                        End If

                        'SyncLock arFactType.Fact
                        arFactType.Fact.Add(lrFact)
                        'End SyncLock

                    End While
                End If

                lRecordset.Close()

            Catch ex As Exception
                lsMessage = "Error: TableFact.GetFactsForFactType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Loading Facts for FactType: '" & arFactType.Id & "'"
                If lrFact IsNot Nothing Then
                    lsMessage.AppendDoubleLineBreak("Fact.Id: " & lrFact.Id)
                End If
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                lRecordset.Close()
                'GetFactsForFactType = Nothing
            End Try

        End Sub

        Public Sub UpdateFact(ByVal arFact As FBM.Fact)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelFact"
                lsSQLQuery &= "   SET FactTypeId = '" & Trim(arFact.FactType.Id) & "'"
                lsSQLQuery &= " WHERE Symbol = '" & Trim(arFact.Symbol) & "'"


                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFact.UpdateFact"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace