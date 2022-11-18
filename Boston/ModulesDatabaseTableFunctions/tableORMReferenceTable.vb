Imports System.Reflection

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Function ExistsReferenceTableByName(ByVal as_reference_table_name As String, Optional ByRef av_return_value As Integer = 0) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

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
            Dim lREcordset As New ADODB.Recordset

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
            Dim lREcordset As New ADODB.Recordset

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
                MsgBox("Error: GetReferenceTableIdByName: No Row Returned.")
            End If


            lREcordset = Nothing

        End Function

        Function GetReferenceTableNameById(ByVal aiReferenceTableId As Integer) As String

            'Returns the TableId of a ReferenceTable given the as_table_name

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

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
                MsgBox("Error: GetReferenceTableIdByName: No Row Returned.")
            End If

            lREcordset = Nothing

        End Function


        Sub UdateReferenceTable(ByVal ar_reference_table As ReferenceTable)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE ReferenceTable"
            lsSQLQuery &= " SET reference_table_name = " & ar_reference_table.name & ","
            lsSQLQuery &= " WHERE reference_table_id = " & ar_reference_table.ReferenceTableId

            pdbConnection.Execute(lsSQLQuery)

        End Sub


    End Module
End Namespace