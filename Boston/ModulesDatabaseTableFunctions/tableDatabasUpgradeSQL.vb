Imports System.Reflection

Public Module tableDatabasUpgradeSQL

    ''' <summary>
    ''' NB Operates on the richmonddatabaseupgrade connection rather than the richmonddatabase connection.
    ''' </summary>
    ''' <param name="ai_upgrade_id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetUpgradeSequenceStepCount(ByVal ai_upgrade_id As Integer) As Integer

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New ADODB.Recordset

        Try
            lrRecordset.ActiveConnection = pdbDatabaseUpgradeConnection
            lrRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM UpgradeSQL"
            lsSQLQuery &= " WHERE UpgradeId = " & ai_upgrade_id

            lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

            If Not lrRecordset.EOF Then
                GetUpgradeSequenceStepCount = lrRecordset(0).Value
                lrRecordset.Close()
            Else
                MsgBox("Error: GetUpgradeSequenceStepCount: 0 value returned: Id = " & ai_upgrade_id)
                GetUpgradeSequenceStepCount = 0
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Function


End Module