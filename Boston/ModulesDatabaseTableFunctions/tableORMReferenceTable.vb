Imports System.Reflection
Imports System.Dynamic

Namespace TableReferenceTable
    Module TableReferenceTable

        Sub AddReferenceTable(ByVal l_reference_table As ReferenceTable)

            Dim lsSQLQuery As String = ""
            Dim lrReferenceTableId As Integer


            lrReferenceTableId = GetNextReferenceTableId()

            If l_reference_table IsNot Nothing Then
                lrReferenceTableId = l_reference_table.ReferenceTableId
            End If

            lsSQLQuery = "INSERT INTO ReferenceTable"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= lrReferenceTableId & ","
            lsSQLQuery &= "'" & l_reference_table.name & "'" & ",0)"

            pdbConnection.Execute(lsSQLQuery)

        End Sub

        Public Sub CreateReferenceTableIfNotExists(ByVal arReferenceTable As ReferenceTable)

            Try
                If Not TableReferenceTable.ExistsReferenceTableByName(arReferenceTable.name) Then
                    Call TableReferenceTable.AddReferenceTable(arReferenceTable)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Sub DeleteReferenceTable(ByVal lrReferenceTableId As Integer)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM ReferenceTable "
                lsSQLQuery &= " WHERE reference_table_id = " & lrReferenceTableId

                pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Function ExistsReferenceTableByName(ByVal as_reference_table_name As String, Optional ByRef av_return_value As Integer = 0) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic


            lsSQLQuery = "SELECT count(*)"
            lsSQLQuery &= "  FROM ReferenceTable"
            lsSQLQuery &= " WHERE reference_table_name = '" & Trim(as_reference_table_name) & "'"

            lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

            If lREcordset(0).Value > 0 Then
                'ReferenceTable exists
                If Not (IsNothing(av_return_value)) Then
                    av_return_value = GetReferenceTableIdByName(as_reference_table_name)
                End If
                ExistsReferenceTableByName = True
            Else
                ExistsReferenceTableByName = False
            End If

            lREcordset.Close()
            lREcordset = Nothing

        End Function

        Function GetNextReferenceTableId() As Integer

            'Returns the next reference_field_id in sequence
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "SELECT Max(reference_table_id) + 1 FROM ReferenceTable"

            lREcordset.Open(lsSQLQuery, , , pc_cmd_table)  ' Create Snapshot.

            If Not IsDBNull(lREcordset(0)) Then
                GetNextReferenceTableId = lREcordset(0).Value
            Else
                GetNextReferenceTableId = 1
            End If

            lREcordset.Close()
            lREcordset = Nothing

        End Function

        Function GetReferenceTableIdByName(ByVal as_table_name As String) As Integer

            'Returns the TableId of a ReferenceTable given the as_table_name

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM ReferenceTable "
            lsSQLQuery &= " WHERE reference_table_name = '" & Trim(as_table_name) & "'"

            lREcordset.Open(lsSQLQuery, , , pc_cmd_table)  ' Create Snapshot.

            If Not lREcordset.EOF Then
                GetReferenceTableIdByName = lREcordset("reference_table_id").Value
                lREcordset.Close()
            Else
                MsgBox($"Error: GetReferenceTableIdByName: No row returned for Reference Table name, '{as_table_name}'.")
            End If


            lREcordset = Nothing

        End Function

        Function GetReferenceTableNameById(ByVal aiReferenceTableId As Integer) As String

            'Returns the TableId of a ReferenceTable given the as_table_name

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT * "
            lsSQLQuery &= " FROM ReferenceTable "
            lsSQLQuery &= " WHERE reference_table_id = " & CStr(aiReferenceTableId)

            lREcordset.Open(lsSQLQuery, , , pc_cmd_table)  ' Create Snapshot.

            If Not lREcordset.EOF Then
                GetReferenceTableNameById = lREcordset("reference_table_name").Value
                lREcordset.Close()
            Else
                MsgBox($"Error: GetReferenceTableIdById: No Row Returned for ReferenceTableId, {aiReferenceTableId}.")
                Return "Error"
            End If

            lREcordset = Nothing

        End Function

        Public Function GetReferenceTableTupleObject(ByRef aiReferenceTableId As Integer) As Object

            Try
                Dim liInd As Integer = 0
                Dim loField As New Object
                Dim laaReferenceFieldList As New List(Of String)
                Dim lsFieldName As String = ""
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New RecordsetProxy
                Dim lsOrderByClause As String = " ORDER BY "
                Dim liFieldCount As Integer

                'Dynamic Object
                Dim loTuple As Object = New ExpandoObject '20231105-Was New tClass
                Dim loTupleObject As Object = New ExpandoObject '20231105-Was New tClass New Object
                Dim loTupleDictionary As IDictionary(Of String, Object) = DirectCast(loTupleObject, IDictionary(Of String, Object))


                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = ADODB.CursorTypeEnum.adOpenStatic

                laaReferenceFieldList = GetReferenceFieldListByReferenceTableId(aiReferenceTableId)

                liFieldCount = laaReferenceFieldList.Count

                Dim loTuple_list As New List(Of Object)

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

                'CodeSafe
                If liFieldCount = 0 Then
                    Return loTuple_list
                End If
#End Region

                '------------------------------------
                'Setup the DynamicObject
                '------------------------------------
                loTupleDictionary.Add("RowId", Nothing)

                For Each lsFieldName In laaReferenceFieldList
                    loTupleDictionary.Add(lsFieldName, Nothing)
                Next

                '---------------------------------------
                'Return a sample of the Tuple instance
                '---------------------------------------
                Return loTupleObject


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function


        Sub UdateReferenceTable(ByVal ar_reference_table As ReferenceTable)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE ReferenceTable"
            lsSQLQuery &= " SET reference_table_name = " & ar_reference_table.name & ","
            lsSQLQuery &= " WHERE reference_table_id = " & ar_reference_table.ReferenceTableId

            pdbConnection.Execute(lsSQLQuery)

        End Sub

        Public Sub UpSert(aiReferenceTableId As Integer, aoObject As Object, aarKeyFields() As Object)

            Try
                Dim larKeyValuePairs() As Object = {}

                Dim objectType As Type = aoObject.GetType()

                ' Get all properties of the object
                Dim properties As PropertyInfo() = objectType.GetProperties()

                For Each propertyInfo As PropertyInfo In properties


                    Dim fieldName As String = propertyInfo.Name
                    Dim value As Object = propertyInfo.GetValue(aoObject)
                    Dim liFieldId = tableReferenceField.GetReferenceTableFieldIdByLabel(aiReferenceTableId, fieldName, True)

                    Dim lbIsKey As Boolean = aarKeyFields.Where(Function(x) x.FieldName = fieldName).Count > 0

                    Dim lrFieldKeyValue = New With {.FieldId = liFieldId, .FieldName = fieldName, .IsKey = lbIsKey, .Value = value}

                    larKeyValuePairs.Add(lrFieldKeyValue)
                Next

                ' Check if the virtual row already exists based on the key fields
                Dim larObject = TableReferenceFieldValue.GetReferenceFieldValueTuples(aiReferenceTableId,, aarKeyFields) ', Nothing
                If larObject.Count = 0 Then
                    ' Insert the reference_field_value rows to represent the virtual row
                    InsertVirtualRow(aiReferenceTableId, larKeyValuePairs)
                Else
                    larKeyValuePairs = larKeyValuePairs.Where(Function(x) x.IsKey = False).ToArray
                    For Each lrKeyValue In larKeyValuePairs
                        Dim lrReferenceFieldValue = New tReferenceFieldValue(aiReferenceTableId, lrKeyValue.FieldId, larObject(0).RowId, lrKeyValue.Value)
                        Call TableReferenceFieldValue.UpdateReferenceFieldValue(lrReferenceFieldValue)
                    Next
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak("Contact FactEngine. ReferenceTable with Id, " & aiReferenceTableId & ", may not be configured in your database.")
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub


        Private Sub InsertVirtualRow(ByVal aiReferenceTableId As Integer, aarKeyValuePairs() As Object)

            Try

                Dim lrReferenceFieldValue As New tReferenceFieldValue
                lrReferenceFieldValue.ReferenceTableId = aiReferenceTableId
                lrReferenceFieldValue.RowId = System.Guid.NewGuid.ToString

                For Each lrKeyValuePair In aarKeyValuePairs
                    lrReferenceFieldValue.ReferenceFieldId = lrKeyValuePair.FieldId
                    lrReferenceFieldValue.Data = lrKeyValuePair.Value
                    lrReferenceFieldValue.Save()
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Module
End Namespace