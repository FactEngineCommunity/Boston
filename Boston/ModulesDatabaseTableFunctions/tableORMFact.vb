Imports System.Reflection

Namespace TableFact

    Module TableFact

        Public Sub AddFact(ByVal arFact As FBM.Fact)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelFact"
                lsSQLQuery &= " VALUES("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
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

        Public Sub DeleteFact(ByVal arFact As FBM.Fact)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelFact"
            lsSQLQuery &= " WHERE Symbol = '" & arFact.Symbol & "'" 'Symbol is FactId
            lsSQLQuery &= " AND ModelId = '" & arFact.Model.ModelId & "'"

            pdbConnection.BeginTrans()

            pdbConnection.Execute(lsSQLQuery)

            '----------------------------------------------
            'Delete the FactData associated with the Fact
            '----------------------------------------------
            Call TableFactData.DeleteFactDataByFact(arFact)

            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsFact(ByVal arFact As FBM.Fact) As Boolean

            '------------------------------------------------------------------
            'The FactTypeId or RowId is the Symbol attribute in the Fact table
            '------------------------------------------------------------------
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

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

            Dim lrDictionaryEntry As FBM.DictionaryEntry
            Dim ls_Symbol_list As String = ""
            Dim ls_tuple_field_name As String = ""
            Dim ls_FactSymbol As String = Nothing 'The unique identifier for a Tuple (like an ORACLE 'RowId')
            Dim lrFact As FBM.Fact
            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role
            Dim lsSQLQuery As String = ""
            Dim lRecordset As New ADODB.Recordset
            Dim lsMessage As String = ""
            Dim lsId As String

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
                'lsSQLQuery &= "  ORDER BY f.Symbol"

                lRecordset.Open(lsSQLQuery)

                If Not lRecordset.EOF Then
                    While Not lRecordset.EOF
                        lrFact = New FBM.Fact(lRecordset("Symbol").Value, arFactType)
                        lrFact.isDirty = False
                        For liInd = 1 To arFactType.Arity
                            lsId = lRecordset("RoleId").Value
                            lrRole = arFactType.RoleGroup.Find(Function(x) x.Id = lsId)
                            If lrRole Is Nothing Then
                                Throw New Exception("Error: GetFactsForFactType: " & vbCrLf & "Could not find Role with RoleId: " & lRecordset("RoleId").Value & vbCrLf & " for Fact with FactId/Symbol: " & lrFact.Symbol & vbCrLf & " for FactType with FactTypeId: " & arFactType.Id & vbCrLf & " in table MetaModelFactData.")
                            End If

                            '--------------------------------------------------------------------------------------------------
                            'Get the Concept from the ModelDictionary so that FactData objects are linked directly to the Concept/Value in the ModelDictionary
                            '--------------------------------------------------------------------------------------------------
                            lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model, lRecordset("ValueSymbol").Value, pcenumConceptType.Value)
                            lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry, , False, False)

                            Dim lrConcept = lrDictionaryEntry.Concept

                            If lrDictionaryEntry Is Nothing Then
                                lsMessage = "Missing ModelDictionary (IsValue) entry for:"
                                lsMessage &= vbCrLf & " FactData: " & lRecordset("ValueSymbol").Value
                                lsMessage &= vbCrLf & " FactId: " & lrFact.Symbol
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= vbCrLf & " Creating a new Dictionary Entry for the Concept in the Model and database."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= vbCrLf & " Reload the Model for the changes to make effect."

                                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(arFactType.Model, lRecordset("ValueSymbol").Value, pcenumConceptType.Value)
                                lrNewDictionaryEntry.Save()

                                lrConcept = lrNewDictionaryEntry.Concept

                                Throw New Exception(lsMessage)
                            End If

                            Dim lrFactData As New FBM.FactData(lrRole, lrConcept, lrFact)

                            lrFactData.isDirty = False
                            lrFactData.Model = arFactType.Model
                            '-----------------------------
                            'Add the RoleData to the Fact
                            '-----------------------------
                            lrFact.Data.Add(lrFactData)
                            '-------------------------------------
                            'Add the RoleData to the Role as well
                            '-------------------------------------
                            lrRole.Data.Add(lrFactData)
                            'If Not lrRole.JoinedORMObject.Instance.Exists(Function(x) x = lrFactData.Data) Then
                            lrRole.JoinedORMObject.Instance.AddUnique(lrFactData.Data)
                            'End If
                            lRecordset.MoveNext()
                        Next liInd


                        '20200514-VM-This requires review and might not be necessary, as can be generaed on the fly in the verbalisation toolbox
                        ''-----------------------------------------------------------------
                        ''Set the LongDescription to the FactTypeReading for the FactType
                        ''-----------------------------------------------------------------
                        'If arFactType.FactTypeReading.Count > 0 Then

                        '    Dim lrPredictaePart As FBM.PredicatePart
                        '    liInd = 0
                        '    Dim lsFactReading As String = ""
                        '    Dim lrFactTypeReading As New FBM.FactTypeReading
                        '    lrFactTypeReading = arFactType.FactTypeReading.Find(AddressOf lrFactTypeReading.MatchesByRoles)
                        '    'arFactType.RoleGroup.Sort(
                        '    If IsSomething(lrFactTypeReading) Then
                        '        For Each lrPredictaePart In lrFactTypeReading.PredicatePart
                        '            'lsFactReading &= arFactType.RoleGroup(liInd).JoinedORMObject.Id & "(" & lrFact.Data(liInd).Data & ") "
                        '            lsFactReading &= arFactType.RoleGroup(liInd).JoinedORMObject.Id & "(" & lrFact.Data(liInd).Data & ") "
                        '            lsFactReading &= lrPredictaePart.PredicatePartText & " "
                        '            liInd += 1
                        '            If (liInd = arFactType.FactTypeReading(0).PredicatePart.Count) And _
                        '               arFactType.FactTypeReading(0).PredicatePart.Count > 1 Then
                        '                'lsFactReading &= arFactType.RoleGroup(liInd).JoinedORMObject.Id & "(" & lrFact.Data(liInd).Data & ") "
                        '                lsFactReading &= arFactType.RoleGroup(liInd).JoinedORMObject.Id & "(" & lrFact.Data(liInd).Data & ") "
                        '            End If
                        '        Next
                        '        lrFact.LongDescription = lsFactReading
                        '    End If
                        'End If

                        '--------------------------------------------------------------------------------------------------
                        'Get the Concept from the ModelDictionary so that Facts are linked directly to the Concept/Value.
                        '--------------------------------------------------------------------------------------------------
                        lrDictionaryEntry = New FBM.DictionaryEntry(arFactType.Model, lrFact.Id, pcenumConceptType.Fact)
                        lrDictionaryEntry = arFactType.Model.AddModelDictionaryEntry(lrDictionaryEntry, True, False)

                        If lrDictionaryEntry IsNot Nothing Then
                            lrDictionaryEntry.LongDescription = lrFact.LongDescription
                        Else
                            lsMessage = "TableFact.getFactsForFactType:" & vbCrLf
                            lsMessage &= "FactType.Id: " & arFactType.Id & vbCrLf
                            lsMessage &= "Can't find Dictionary Entry for Fact with Fact.Id: " & lrFact.Id

                            Dim lsFixMessage As String = "" 'How to fix the problem
                            lsFixMessage = "INSERT INTO MetaModelModelDictionary VALUES "
                            lsFixMessage &= "(Now, Now,"
                            lsFixMessage &= "'" & arFactType.Model.ModelId & "',"
                            lsFixMessage &= "'" & lrFact.Id & "',"
                            lsFixMessage &= "'',"
                            lsFixMessage &= "'',"
                            lsFixMessage &= "False, False, False, True, False, False) "
                            lsFixMessage &= vbCrLf & vbCrLf & "Attempting to fix the problem"
                            prApplication.ThrowErrorMessage(lsMessage & vbCrLf & "Fix to apply:" & lsFixMessage, pcenumErrorType.Critical)
                        End If

                        '----------------------------------------------------------------------------------------------------------
                        'If the FactType of the Fact is Objectified, add the Fact.Id as an instance of the ObjectifyingEntityType
                        '----------------------------------------------------------------------------------------------------------
                        If arFactType.IsObjectified And arFactType.ObjectifyingEntityType IsNot Nothing Then
                            arFactType.ObjectifyingEntityType.Instance.Add(lrFact.Id)
                        End If

                        'SyncLock arFactType.Fact
                        arFactType.Fact.Add(lrFact)
                        'End SyncLock

                        '-------------------------------------------------------------
                        'CodeSafe: If there are no Roles for the FactType then abort
                        '-------------------------------------------------------------
                        If arFactType.Arity = 0 Then
                            Exit While
                        End If

                    End While
                End If

                lRecordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                lsMessage1 = "Error: TableFact.GetFactsForFactType"
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                lsMessage1 &= vbCrLf & vbCrLf & "Loading Facts for FactType: '" & arFactType.Id & "'"
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

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