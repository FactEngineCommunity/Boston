Imports DynamicClassLibrary.Factory
Imports System.Reflection

Namespace TableReferenceFieldValue

    Module ztable_reference_field_value

        Sub AddReferenceFieldValue(ByVal ar_reference_field_value As tReferenceFieldValue,
                                   Optional ByVal abIgnoreError As Boolean = False)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO ReferenceFieldValue"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= ar_reference_field_value.ReferenceTableId
                lsSQLQuery &= ", '" & Trim(ar_reference_field_value.RowId) & "'"
                lsSQLQuery &= " , " & ar_reference_field_value.ReferenceFieldId
                lsSQLQuery &= " , '" & Trim(ar_reference_field_value.Data) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableReferenceFieldValue.AddReferenceFieldValue"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                If Not abIgnoreError Then
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End If
            End Try

        End Sub

        Public Sub CreateReferenceFieldValueIfNotExists(ByRef lrReferenceFieldValue As tReferenceFieldValue)

            Try
                If Not TableReferenceFieldValue.ExistsReferenceFieldValue(lrReferenceFieldValue) Then
                    Call TableReferenceFieldValue.AddReferenceFieldValue(lrReferenceFieldValue)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Sub DeleteReferenceFieldValue(ByVal ar_reference_field_value As tReferenceFieldValue)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ReferenceFieldValue"
            lsSQLQuery &= " WHERE reference_table_id = " & ar_reference_field_value.ReferenceTableId
            lsSQLQuery &= "   AND row_id = '" & Trim(ar_reference_field_value.RowId) & "'"
            lsSQLQuery &= "   AND reference_field_id = " & ar_reference_field_value.ReferenceFieldId

            Call pdbConnection.Execute(lsSQLQuery)

        End Sub

        Public Sub DeleteReferenceFieldValuesForReferenceTableById(ByVal aiReferenceTableId As Integer)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM ReferenceFieldValue "
                lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId.ToString

                pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Function ExistsReferenceFieldValue(ByVal ar_reference_field_value As tReferenceFieldValue, Optional ByVal av_return_value As Object = Nothing) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT count(*)"
            lsSQLQuery &= "  FROM ReferenceFieldValue"
            lsSQLQuery &= " WHERE reference_table_id = " & ar_reference_field_value.ReferenceTableId
            lsSQLQuery &= "   AND reference_field_id = " & ar_reference_field_value.ReferenceFieldId
            lsSQLQuery &= "   AND row_id = '" & Trim(ar_reference_field_value.RowId) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                'FieldValue exists
                If Not (IsNothing(av_return_value)) Then
                    av_return_value = GetReferenceFieldValue(ar_reference_field_value.ReferenceTableId, ar_reference_field_value.ReferenceFieldId)
                End If
                ExistsReferenceFieldValue = True
            Else
                ExistsReferenceFieldValue = False
            End If

            lREcordset.Close()
            lREcordset = Nothing

        End Function

        Function GetReferenceFieldValue(ByVal aiReferenceTableId As Integer,
                                        ByVal aiReference_field_id As Integer,
                                        Optional ByVal abIgnoreErrors As Boolean = False) As String

            Dim lsSQLQuery As String
            '20230513-VM-Was - Dim lREcordset As New ADODB.Recordset 'Changed when moved to SQLite database
            Dim lRecordset As New RecordsetProxy()

            Try

                lRecordset.ActiveConnection = pdbConnection
                lRecordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT Data"
                lsSQLQuery &= " FROM ReferenceFieldValue"
                lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId
                lsSQLQuery &= " AND reference_field_Id = " & aiReference_field_id

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    GetReferenceFieldValue = lREcordset("Data").Value
                Else
                    If Not abIgnoreErrors Then
                        MsgBox("Error: get_reference_field_value_data: no record returned: parameter: aiReferenceTableId=" & aiReferenceTableId & ", aiReference_field_id=" & aiReference_field_id)
                    End If
                    GetReferenceFieldValue = ""
                End If

                lREcordset.Close()
                lREcordset = Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        ''' <summary>
        ''' Gets a list of ReferentTable tuples from the ReferenceFieldValue table.
        '''   Each Tuple is a dynamic object created at runtime. The returned Tuple objects have the Attributes of the Fields of the ReferenceTable.
        '''   aoWorkingClass is set to a sample of the makeup of the Tuples returned and can be viewed at debug time to see the name of the Attributes/Fields of each Tuple.
        ''' </summary>
        ''' <param name="aiReferenceTableId">The unique id of the ReferenceTable in the ReferenceTable table.</param>
        ''' <param name="aoWorkingClass">Is set to a sample of the makeup of the Tuples returned</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetReferenceFieldValueTuples(ByVal aiReferenceTableId As Integer,
                                              ByRef aoWorkingClass As Object,
                                              Optional ByRef arReferenceTable As ReferenceTable = Nothing) As List(Of Object)

            Dim liInd As Integer = 0
            Dim loField As New Object
            Dim loTuple As New tClass
            Dim loTupleObject As New Object
            Dim laaReferenceFieldList As New List(Of String)
            Dim lsFieldName As String = ""
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsOrderByClause As String = " ORDER BY "
            Dim liFieldCount As Integer

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = ADODB.CursorTypeEnum.adOpenStatic

                If arReferenceTable IsNot Nothing Then
                    arReferenceTable.ReferenceTuple.Clear()
                    arReferenceTable.Name = TableReferenceTable.getReferenceTableNameById(aiReferenceTableId)
                End If

                laaReferenceFieldList = GetReferenceFieldListByReferenceTableId(aiReferenceTableId)

                liFieldCount = laaReferenceFieldList.Count

#Region "Check if new field added"
                lsSQLQuery = "SELECT Max(RFV.reference_field_id) FROM ReferenceFieldValue RFV WHERE RFV.reference_table_id = " & aiReferenceTableId

                lREcordset.Open(lsSQLQuery)

                Try
                    If Not lREcordset.EOF Then
                        liFieldCount = lREcordset(0).Value
                    End If
                Catch ex As Exception
                Finally
                    lREcordset.Close()
                End Try


#End Region

                lsSQLQuery = "SELECT rfv1.row_id,"
                For liInd = 1 To liFieldCount
                    lsSQLQuery &= "rfv" & Trim(CStr(liInd)) & ".data"
                    If liInd < liFieldCount Then
                        lsSQLQuery &= ","
                    End If
                    lsOrderByClause &= liInd + 1
                    If liInd < liFieldCount Then
                        lsOrderByClause &= ","
                    End If
                Next

                lsSQLQuery &= " FROM "
                For liInd = 1 To liFieldCount
                    lsSQLQuery &= "ReferenceFieldValue rfv" & Trim(CStr(liInd))
                    If liInd < liFieldCount Then
                        lsSQLQuery &= ","
                    End If
                Next
                lsSQLQuery &= " WHERE "
                For liInd = 1 To liFieldCount
                    lsSQLQuery &= "rfv" & Trim(CStr(liInd)) & ".reference_table_id = " & aiReferenceTableId
                    lsSQLQuery &= " AND rfv" & Trim(CStr(liInd)) & ".reference_field_id = " & liInd
                    lsSQLQuery &= " AND rfv" & Trim(CStr(liInd)) & ".row_id = rfv1.row_id"
                    If liInd < liFieldCount Then
                        lsSQLQuery &= " AND "
                    End If
                Next
                lsSQLQuery &= lsOrderByClause

                lREcordset.Open(lsSQLQuery)

                '------------------------------------
                'Setup the DynamicObject
                '------------------------------------
                loTuple.add_attribute(New tAttribute("row_id", GetType(String)))
                For Each lsFieldName In laaReferenceFieldList
                    loTuple.add_attribute(New tAttribute(lsFieldName, GetType(String)))
                Next

                '---------------------------------------
                'Return a sample of the Tuple instance
                '---------------------------------------
                aoWorkingClass = loTuple.clone

                Dim loTuple_list As New List(Of Object)

                Dim lrReferenceTuple As New ReferenceTuple

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        loTupleObject = loTuple.clone
                        '-------------------
                        'Set the values
                        '-------------------
                        lrReferenceTuple = New ReferenceTuple(lREcordset("row_id").Value)
                        liInd = 0
                        For Each loField In lREcordset.Fields
                            Select Case liInd
                                Case Is = 0
                                    '------------------------------------------------------------
                                    'The first Column is always the unique RowId for the Tuple.
                                    '  NB This concept is consistent with models such as the
                                    '  ORACLE database, RowId, for unique tuples etc.
                                    '------------------------------------------------------------
                                    loTupleObject.row_id = lREcordset("row_id").Value
                                Case Else
                                    Dim pro As System.Reflection.PropertyInfo
                                    pro = loTupleObject.GetType.GetProperty(laaReferenceFieldList(liInd - 1))
                                    pro.SetValue(loTupleObject, CStr(Viev.NullVal(loField.value, " ")), Nothing)

                                    If arReferenceTable IsNot Nothing Then
                                        lrReferenceTuple.KeyValuePair.Add(New KeyValuePair(laaReferenceFieldList(liInd - 1), CStr(Viev.NullVal(loField.value, " "))))
                                    End If
                            End Select

                            liInd += 1
                        Next
                        loTuple_list.Add(loTupleObject)

                        If arReferenceTable IsNot Nothing Then
                            arReferenceTable.ReferenceTableId = CStr(aiReferenceTableId)
                            arReferenceTable.ReferenceTuple.Add(lrReferenceTuple)
                        End If

                        lREcordset.MoveNext()
                    End While
                    lREcordset.Close()
                Else
                    lREcordset.Close()
                End If

                Return loTuple_list

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak("ReferenceTableId:" & aiReferenceTableId)
                lsMessage.AppendLine("ReferenceFieldCount:" & liFieldCount)
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Sub save_reference_field_value(ByVal ar_reference_field_value As tReferenceFieldValue)


            Dim lsSQLQuery As String = ""

            lsSQLQuery = "INSERT INTO ReferenceFieldValue"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= ar_reference_field_value.ReferenceTableId & ","
            lsSQLQuery &= "'" & Trim(System.Guid.NewGuid.ToString) & "',"
            lsSQLQuery &= ar_reference_field_value.ReferenceFieldId & ","
            lsSQLQuery &= "'" & Trim(ar_reference_field_value.Data) & "'"

            pdbConnection.Execute(lsSQLQuery)


        End Sub


        Sub UpdateReferenceFieldValue(ByVal ar_reference_field_value As tReferenceFieldValue)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE ReferenceFieldValue"
            lsSQLQuery &= " SET data = '" & ar_reference_field_value.Data & "'"
            lsSQLQuery &= " WHERE reference_table_id = " & ar_reference_field_value.ReferenceTableId
            lsSQLQuery &= "   AND reference_field_id = " & ar_reference_field_value.ReferenceFieldId
            lsSQLQuery &= "   AND row_id = '" & Trim(ar_reference_field_value.RowId) & "'"

            pdbConnection.Execute(lsSQLQuery)

        End Sub


    End Module
End Namespace