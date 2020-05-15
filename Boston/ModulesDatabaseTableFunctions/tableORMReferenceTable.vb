Namespace TableReferenceTable
    Module TableReferenceTable

        Public Class t_reference_table_type
            Public reference_table_id As Integer
            Public name As String
        End Class


        Sub add_reference_table(ByVal l_reference_table As t_reference_table_type)


            Dim lsSQLQuery As String = ""
            Dim l_reference_table_id As Integer


            l_reference_table_id = get_next_reference_table_id()

            lsSQLQuery = "INSERT INTO ReferenceTable"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= l_reference_table_id & ","
            lsSQLQuery &= "'" & l_reference_table.name & "'" & ","

            pdbConnection.Execute(lsSQLQuery)

        End Sub


        Sub delete_reference_table(ByVal l_reference_table_id As Integer)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ReferenceTable "
            lsSQLQuery &= " WHERE Confguration_Item_Type_Id = " & l_reference_table_id

            pdbConnection.Execute(lsSQLQuery)

        End Sub

        Function exists_reference_table_by_name(ByVal as_reference_table_name As String, Optional ByRef av_return_value As Integer = 0) As Boolean

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
                exists_reference_table_by_name = True
            Else
                exists_reference_table_by_name = False
            End If

            lREcordset.Close()
            lREcordset = Nothing

        End Function

        Function get_next_reference_table_id() As Integer

            'Returns the next reference_field_id in sequence
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "SELECT Max(reference_table_Id) + 1 FROM ReferenceTable"

            lREcordset.Open(lsSQLQuery, , , pc_cmd_table)  ' Create Snapshot.

            If Not IsDBNull(lREcordset(0)) Then
                get_next_reference_table_id = lREcordset(0).Value
            Else
                get_next_reference_table_id = 1
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


        Sub update_reference_table(ByVal ar_reference_table As t_reference_table_type)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "UPDATE ReferenceTable"
            lsSQLQuery &= " SET reference_table_name = " & ar_reference_table.name & ","
            lsSQLQuery &= " WHERE reference_table_Id = " & ar_reference_table.reference_table_id

            pdbConnection.Execute(lsSQLQuery)

        End Sub


    End Module
End Namespace