Imports System.Reflection

Module tableReferenceField

    Public Sub AddReferenceField(ByRef arReferenceField As tReferenceField)

        Try
            Dim lsSQLQuery As String = ""

            lsSQLQuery = "INSERT INTO ReferenceField"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= arReferenceField.ReferenceTableId & ","
            lsSQLQuery &= arReferenceField.reference_field_id & ","
            lsSQLQuery &= "'" & arReferenceField.label & "'" & ","
            lsSQLQuery &= arReferenceField.data_type & ","
            lsSQLQuery &= arReferenceField.cardinality & ","
            lsSQLQuery &= arReferenceField.required & ","
            lsSQLQuery &= arReferenceField.system & ")"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub CreateReferenceFieldIfNotExists(ByRef arReferenceField As tReferenceField)

        Try
            Dim lbReturnValue As Boolean
            If Not tableReferenceField.ExistsReferenceTableFieldByLabel(arReferenceField.ReferenceTableId,
                                                                        arReferenceField.label,
                                                                        lbReturnValue) Then
                Call tableReferenceField.AddReferenceField(arReferenceField)
            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub DeleteReferenceFieldsForReferenceTableById(ByVal aiReferenceTableId As Integer)

        Try
            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ReferenceField "
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

    Function ExistsReferenceTableFieldByLabel(ByVal aiReferenceTableId As Integer, ByVal as_reference_field_label As String, Optional ByRef av_return_value As Integer = 0) As Boolean

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic


        lsSQLQuery = "SELECT count(*)"
        lsSQLQuery &= "  FROM ReferenceField"
        lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId
        lsSQLQuery &= "   AND reference_field_label = '" & Trim(as_reference_field_label) & "'"

        lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

        If lREcordset(0).Value > 0 Then
            'FieldValue exists
            If Not (IsNothing(av_return_value)) Then
                av_return_value = GetReferenceTableFieldIdByLabel(aiReferenceTableId, as_reference_field_label)
            End If
            ExistsReferenceTableFieldByLabel = True
        Else
            ExistsReferenceTableFieldByLabel = False
        End If

        lREcordset.Close()
        lREcordset = Nothing

    End Function

    Function GetReferenceFieldLabel(ByVal aiReferenceFieldId As Integer) As String

        Dim lsSQLQuery As String
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic


        ' Create the Snapshot
        lsSQLQuery = "SELECT * FROM ReferenceField WHERE reference_field_Id = " & aiReferenceFieldId

        lREcordset.Open(lsSQLQuery)  ' Create Snapshot.

        If Not lREcordset.EOF Then
            GetReferenceFieldLabel = lREcordset("reference_field_Label").Value
            lREcordset.Close()
        Else
            MsgBox("ERROR: No reference_field_Id exists for the reference_field_Id : " & Str$(aiReferenceFieldId) & ". Error in function call 'get_configuration_label'")
            GetReferenceFieldLabel = ""
        End If

        lREcordset = Nothing

    End Function

    Function GetReferenceFieldCount() As Integer

        'returns the number of reference_fields
        Dim lsSQLQuery As String
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic


        ' Create the recordset
        lsSQLQuery = "SELECT COUNT(*) FROM ReferenceField"
        lREcordset.Open(lsSQLQuery)

        GetReferenceFieldCount = lREcordset(0).Value

        lREcordset.Close()
        lREcordset = Nothing


    End Function

    Function GetReferenceFieldListByReferenceTableId(ByVal aiReferenceTableId As Integer) As List(Of String)

        Dim laa_reference_field As New List(Of String)
        Dim lsSQLQuery As String
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic


        ' Create the recordset
        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM ReferenceField"
        lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId
        lsSQLQuery &= " ORDER BY reference_field_id"

        lREcordset.Open(lsSQLQuery)

        If Not lREcordset.EOF Then
            While Not lREcordset.EOF
                laa_reference_field.Add(lREcordset("reference_field_label").Value)
                lREcordset.MoveNext()
            End While
            lREcordset.Close()
        End If

        Return laa_reference_field

    End Function


    Function GetReferenceFieldDetailsById(ByVal aiReferenceTableId As Integer, ByVal aiReferenceFieldId As Integer, ByRef arReferenceField As tReferenceField) As Boolean

        'Returns the values for a reference_field
        'given a reference_field_Id

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        '-------------------------
        'Set default return value
        '-------------------------
        GetReferenceFieldDetailsById = False

        lsSQLQuery = "SELECT * FROM ReferenceField"
        lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId
        lsSQLQuery &= " AND reference_field_Id = " & aiReferenceFieldId

        lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

        If Not lREcordset.EOF Then
            arReferenceField.reference_field_id = aiReferenceFieldId
            arReferenceField.label = lREcordset("reference_field_Label").Value
            arReferenceField.ReferenceTableId = lREcordset("reference_table_id").Value
            arReferenceField.data_type = lREcordset("reference_data_type_Id").Value
            arReferenceField.cardinality = lREcordset("Cardinality").Value
            arReferenceField.required = lREcordset("Required").Value
            GetReferenceFieldDetailsById = True
        Else
            MsgBox("Cannot location Confguration_Item for reference_field_Id - '" & aiReferenceFieldId & "'")
            GetReferenceFieldDetailsById = False
        End If

        lREcordset.Close()
        lREcordset = Nothing

    End Function


    Function GetReferenceTableFieldIdByLabel(ByVal aiReferenceTableId As Integer, ByVal as_reference_field_label As String) As Integer

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT * "
        lsSQLQuery &= "  FROM ReferenceField"
        lsSQLQuery &= " WHERE reference_table_id = " & aiReferenceTableId
        lsSQLQuery &= "   AND reference_field_label = '" & Trim(as_reference_field_label) & "'"

        lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

        If Not lREcordset.EOF Then
            GetReferenceTableFieldIdByLabel = lREcordset("reference_field_id").Value
        Else
            MsgBox("Error: get_reference_table_field_by_label: No row returned")
        End If

        lREcordset.Close()
        lREcordset = Nothing

    End Function


    Function IsReferenceFieldRequired(ByVal l_reference_field_id As Integer) As Integer

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT Count(*)"
        lsSQLQuery &= " FROM ReferenceField"
        lsSQLQuery &= " WHERE reference_field_Id = " & l_reference_field_id
        lsSQLQuery &= " AND Required = TRUE"

        lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

        If lREcordset(0).Value > 0 Then
            IsReferenceFieldRequired = True
        Else
            IsReferenceFieldRequired = False
        End If

        lREcordset.Close()
        lREcordset = Nothing


    End Function



    Function IsUniqueReferenceFieldLabel(ByVal l_reference_field_id As Integer, ByVal l_reference_field_label As String) As Boolean

        'Returns True if Label is unique and l_reference_field_id = 0
        'Returns True if Label is unique and l_reference_field_id > 0
        ' ie if reference_field_id > 0 then there must be only one occurance of label in database
        '    if l_reference_field_id = 0 then user is adding reference_field and there
        '       must be no previous instances of label in the database
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        ' Create the recordset
        lsSQLQuery = "SELECT COUNT(*)"
        lsSQLQuery &= " FROM ReferenceField"
        lsSQLQuery &= " WHERE UCASE(reference_field_Label) = '" & UCase(l_reference_field_label) & "'"
        If l_reference_field_id > 0 Then
            lsSQLQuery &= " AND reference_field_Id <> " & l_reference_field_id
        End If

        lREcordset.Open(lsSQLQuery, , , pc_cmd_table)

        If lREcordset(0).Value = 0 Then
            IsUniqueReferenceFieldLabel = True
        Else
            IsUniqueReferenceFieldLabel = False
        End If

        lREcordset.Close()
        lREcordset = Nothing

    End Function


End Module