Imports System.Reflection

Module tableORMFactInstance
    '-----------------------------------------------------------------------
    'FactInstances are stored in the ConceptInstance table in this version
    '  of Richmond, but it is expedient to have this module seperate
    '  in case FactInstance ever becomes it's own Table
    '-----------------------------------------------------------------------

    Public Sub GetFactsForFactTypeInstance(ByRef arFactTypeInstance As FBM.FactTypeInstance)

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        lRecordset.ActiveConnection = pdbConnection
        lRecordset.CursorType = pcOpenStatic

        Try

            lsSQLQuery = "  SELECT f.Symbol, fci.x AS FCIX, fci.y AS FCIY, fd.RoleId, fd.ValueSymbol, fdci.x AS FDCIX, fdci.y AS FDCIY"
            lsSQLQuery &= "   FROM MetaModelFact f,"
            lsSQLQuery &= "        MetaModelFactData fd,"
            lsSQLQuery &= "        ModelConceptInstance fci," 'Fact
            lsSQLQuery &= "        ModelConceptInstance fdci"  'FactData
            lsSQLQuery &= "  WHERE f.ModelId = '" & Trim(arFactTypeInstance.Model.ModelId) & "'"
            lsSQLQuery &= "    AND f.FactTypeId = '" & Trim(CStr(arFactTypeInstance.Id)) & "'"
            lsSQLQuery &= "    AND fd.ModelId = '" & Trim(arFactTypeInstance.Model.ModelId) & "'"
            lsSQLQuery &= "    AND fd.FactSymbol = f.Symbol"
            lsSQLQuery &= "    AND fd.FactSymbol = fci.Symbol"
            lsSQLQuery &= "    AND fci.ModelId = '" & Trim(arFactTypeInstance.Model.ModelId) & "'"
            lsSQLQuery &= "    AND fci.PageId = '" & arFactTypeInstance.Page.PageId & "'"
            lsSQLQuery &= "    AND fci.ConceptType = '" & pcenumConceptType.Fact.ToString & "'"
            lsSQLQuery &= "    AND fdci.ModelId = '" & Trim(arFactTypeInstance.Model.ModelId) & "'"
            lsSQLQuery &= "    AND fdci.PageId = '" & arFactTypeInstance.Page.PageId & "'"
            lsSQLQuery &= "    AND fdci.Symbol = fd.ValueSymbol"
            lsSQLQuery &= "    AND fdci.ConceptType = '" & pcenumConceptType.Value.ToString & "'"
            lsSQLQuery &= "    AND fdci.RoleId = fd.RoleId"
            lsSQLQuery &= "  ORDER BY f.Symbol, fd.RoleId" 'Symbol equates to FactId

            lRecordset.Open(lsSQLQuery)

            Dim lsSuccessFactId As String = ""
            If Not lRecordset.EOF Then

                Dim liInd As Integer = 0
                Dim ls_Symbol_list As String = ""
                Dim ls_tuple_field_name As String = ""
                Dim ls_FactSymbol As String = Nothing 'The unique identifier for a Tuple (like an ORACLE 'RowId')
                Dim lrFactInstance As FBM.FactInstance
                Dim lrRoleInstance As FBM.RoleInstance
                Dim lsFactId As String = ""
                Dim lrFact As FBM.Fact

                lRecordset.MoveFirst()
                While Not lRecordset.EOF

                    While Not lRecordset.EOF
                        lsFactId = lRecordset("Symbol").Value
                        If lsSuccessFactId = lsFactId Then
                            lRecordset.MoveNext()
                        Else
                            Exit While
                        End If
                    End While
                    lrFactInstance = New FBM.FactInstance(lsFactId, arFactTypeInstance)
                    lsSuccessFactId = ""

                    lrFactInstance.X = lRecordset("FCIX").Value
                    lrFactInstance.Y = lRecordset("FCIY").Value
                    Dim lrFactDataInstance As FBM.FactDataInstance

                    '-----------------------------------
                    'Find the RoleInstance for the Data
                    '-----------------------------------
                    Dim liSuccessInd As Integer = 0
                    For liInd = 1 To arFactTypeInstance.Arity

                        If Not lRecordset.EOF Then
                            If lRecordset("Symbol").Value = lsFactId Then
                                Try

                                    lrRoleInstance = arFactTypeInstance.RoleGroup.Find(Function(x) x.Id = lRecordset("RoleId").Value) ' AddressOf lrRoleInstance.Equals)
                                    If lrRoleInstance Is Nothing Then
                                        Throw New ApplicationException("Error: FetFactsForFactTypeInstance: Could not find RoleInstance for RoleId: " & lRecordset("RoleId").Value & ", for FactType: " & arFactTypeInstance.Name)
                                    End If

                                    '-------------------------------
                                    'Create a Concept for the Data
                                    '-------------------------------
                                    Dim lrConcept As New FBM.Concept(lRecordset("ValueSymbol").Value)

                                    '----------------------------
                                    'Create the FactDataInstance
                                    '----------------------------
                                    lrFactDataInstance = New FBM.FactDataInstance(arFactTypeInstance.Page, lrFactInstance, lrRoleInstance, lrConcept)
                                    lrFactDataInstance.X = lRecordset("FDCIX").Value
                                    lrFactDataInstance.Y = lRecordset("FDCIY").Value

                                    '-------------------------------------
                                    'Add the FactDataInstance to the Role
                                    '-------------------------------------
                                    lrRoleInstance.Data.Add(lrFactDataInstance)

                                    '-------------------------------------
                                    'Add the FactDataInstance to the Fact
                                    '-------------------------------------
                                    lrFactInstance.Data.Add(lrFactDataInstance)
                                    '-------------------------------------
                                    'Add the FactDataInstance to the Page
                                    '  Most useful in XML Export
                                    '-------------------------------------
                                    'SyncLock arFactTypeInstance.Page.ValueInstance
                                    arFactTypeInstance.Page.ValueInstance.Add(lrFactDataInstance)
                                    'End SyncLock

                                    liSuccessInd += 1

                                    If Not lRecordset.EOF Then lRecordset.MoveNext()
                                Catch ex As Exception
#Region "Exception"
                                    'Probaly a EOF Error on lRecordset.MoveNext
                                    'Get the FactInstance from the Database
                                    Dim lrFact2 As FBM.Fact = arFactTypeInstance.FactType.Fact.Find(Function(x) x.Id = lsFactId).CloneInstance(arFactTypeInstance.Page)
                                    For Each lrFactData2 As FBM.FactData In lrFact2.Data
                                        '-------------------------------------
                                        'Add the FactDataInstance to the Role
                                        '-------------------------------------
                                        lrRoleInstance = arFactTypeInstance.RoleGroup.Find(Function(x) x.Id = lrFactData2.Role.Id)
                                        Dim lrFactDataInstance2 = lrFactData2.CloneInstance(arFactTypeInstance.Page, lrFactInstance)
                                        lrRoleInstance.Data.AddUnique(lrFactDataInstance2)

                                        '-------------------------------------
                                        'Add the FactDataInstance to the Fact
                                        '-------------------------------------
                                        lrFactInstance.Data.AddUnique(lrFactDataInstance2)
                                        '-------------------------------------
                                        'Add the FactDataInstance to the Page
                                        '  Most useful in XML Export
                                        '-------------------------------------
                                        'SyncLock arFactTypeInstance.Page.ValueInstance
                                        arFactTypeInstance.Page.ValueInstance.Add(lrFactDataInstance2)
                                    Next
                                    Dim lsMessage = "Not all Fact Data was found on the Page for:"
                                    lsMessage &= vbCrLf & vbCrLf
                                    lsMessage &= "Page: " & arFactTypeInstance.Page.Name
                                    lsMessage.AppendLine("Fact Type: " & arFactTypeInstance.Id)
                                    lsMessage.AppendLine("Fact :" & lsFactId)
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning,, False, False, True,, True)
#End Region
                                End Try
                            Else
                                Try
                                    lrFact = arFactTypeInstance.FactType.Fact.Find(Function(x) x.Id = lsFactId)
                                    lrFactInstance = lrFact.CloneInstance(arFactTypeInstance.Page, True)
                                    lrFactInstance.FactType = arFactTypeInstance
                                    lrFactInstance.isDirty = True
                                    arFactTypeInstance.isDirty = True
                                    arFactTypeInstance.Page.IsDirty = True
                                    For Each lrTempFactDataInstance In lrFactInstance.Data
                                        arFactTypeInstance.Page.ValueInstance.AddUnique(lrTempFactDataInstance)
                                        lrTempFactDataInstance.Role.Data.AddUnique(lrTempFactDataInstance)
                                    Next
                                    liSuccessInd = arFactTypeInstance.FactType.Arity
                                    lsSuccessFactId = lsFactId
                                    lRecordset.MoveNext()
                                    Exit For
                                Catch ex As Exception
                                    Dim lsMessage As String
                                    lsMessage = "Error: GetFactsForFactTypeInstance:"
                                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                                    lsMessage &= vbCrLf & vbCrLf & "PageId: " & arFactTypeInstance.Page.PageId & vbCrLf & ", FactTypeId: " & arFactTypeInstance.Id & vbCrLf & ", FactId:" & lsFactId
                                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                                End Try
                            End If
                        End If
                    Next liInd

                    If liSuccessInd < arFactTypeInstance.FactType.Arity Then
                        'Get the FactInstance from the Model level.
                        lrFact = arFactTypeInstance.FactType.Fact.Find(Function(x) x.Id = lsFactId)
                        lrFactInstance = lrFact.CloneInstance(arFactTypeInstance.Page, True)
                        lrFactInstance.isDirty = True
                        arFactTypeInstance.isDirty = True
                        arFactTypeInstance.Page.IsDirty = True
                        For Each lrFactDataInstance In lrFactInstance.Data
                            arFactTypeInstance.Page.ValueInstance.Add(lrFactDataInstance)
                            lrFactDataInstance.Role.Data.AddUnique(lrFactDataInstance)
                        Next
                    End If
                    lsSuccessFactId = lsFactId

                    'SyncLock arFactTypeInstance.Page.FactInstance
                    arFactTypeInstance.Page.FactInstance.Add(lrFactInstance)
                    'End SyncLock

                    'SyncLock arFactTypeInstance.Fact
                    arFactTypeInstance.Fact.Add(lrFactInstance)
                    'End SyncLock

                End While
            End If

            lRecordset.Close()

        Catch ex As Exception

            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage &= vbCrLf & vbCrLf & "PageId: " & arFactTypeInstance.Page.PageId & vbCrLf & ", FactTypeId: " & arFactTypeInstance.Id

            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,, True,, True)

            lRecordset.Close()
        End Try

    End Sub

End Module
