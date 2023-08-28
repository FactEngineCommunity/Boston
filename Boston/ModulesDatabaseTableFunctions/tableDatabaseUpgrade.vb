Imports System.Reflection

Public Module tableDatabaseUpgrade

    ''' <summary>
    ''' Adds an 'Upgrade' record to the 'Rosters' database
    ''' '-----------------------------------------------------------------------------
    ''' Preconditions
    '''  * ar_upgrade.UpgradeId is set to the next free 'UpgradeId' in the upgrade table
    '''  * ar_upgrade.successfully_implemented should be equal to FALSE
    '''  * a record in the 'upgrade' table should NOT exist with ar_upgrade.FromVersion_nr, ar_upgrade.ToVersion_nr combination
    '''       NB This can be handled with a appropriate 'unique index' on 'FromVersion'/'ToVersion' within the 'upgrade' table.
    '''-----------------------------------------------------------------------------
    ''' </summary>
    ''' <param name="ar_upgrade"></param>
    ''' <remarks></remarks>
    Sub AddUpgrade(ByVal ar_upgrade As DatabaseUpgrade.Upgrade)

        Try
            Dim lsSQLQuery As String = ""

            lsSQLQuery = "INSERT INTO DatabaseUpgrade"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= ar_upgrade.UpgradeId & ","
            lsSQLQuery &= "'" & Trim(ar_upgrade.FromVersionNr) & "',"
            lsSQLQuery &= "'" & Trim(ar_upgrade.ToVersionNr) & "',"
            lsSQLQuery &= ar_upgrade.SuccessfulImplementation
            lsSQLQuery &= ")"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

            pdbConnection.RollbackTrans()
        End Try

    End Sub


    Function ExistsDatabaseUpgradeInRichmond(ByVal as_FromVersion_nr As String, ByVal as_ToVersion_nr As String) As Boolean

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New RecordsetProxy

        lrRecordset.ActiveConnection = pdbConnection
        lrRecordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT COUNT(*)"
        lsSQLQuery &= "  FROM DatabaseUpgrade"
        lsSQLQuery &= " WHERE FromVersion = '" & Trim(as_FromVersion_nr) & "'"
        lsSQLQuery &= "   AND ToVersion = '" & Trim(as_ToVersion_nr) & "'"

        lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

        If lrRecordset(0).Value = 0 Then
            ExistsDatabaseUpgradeInRichmond = False
        Else
            ExistsDatabaseUpgradeInRichmond = True
        End If

        lrRecordset.Close()

    End Function

    Sub DeleteDatabaseUpgradeData()

        '-----------------------------------------------------------------------------
        'This procedure removes all the existing records within the 'upgrade'
        '  table within the Rosters database.
        '
        '  The records are deleted so that the new batch arriving from the
        '     'RostersDatabaseUpgrade' database can simply be placed in an empty table.
        '-----------------------------------------------------------------------------

        Dim lsSQLQuery As String = ""

        lsSQLQuery = "DELETE FROM DatabaseUpgrade WHERE 1 = 1"

        pdbConnection.Execute(lsSQLQuery)

    End Sub

    Sub LoadRichmondDatabaseUpgradeInformation()

        Dim lrDatabaesUpgrade As DatabaseUpgrade.Upgrade
        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New RecordsetProxy

        Try
            Dim lsDatabaseVersionNumber As String = ""
            lsDatabaseVersionNumber = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)


            lrRecordset.ActiveConnection = pdbDatabaseUpgradeConnection
            lrRecordset.CursorType = pcOpenStatic

            '-----------------------------------------------------------------------------------
            'Load the records from the 'upgrade' table from the RostersDatabaseUpgrade database
            '  to the Rosters database
            '-----------------------------------------------------------------------------------
            lsSQLQuery = "SELECT * FROM Upgrade"

            lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

            While Not lrRecordset.EOF
                lrDatabaesUpgrade = New DatabaseUpgrade.Upgrade
                lrDatabaesUpgrade.UpgradeId = lrRecordset("UpgradeId").Value
                lrDatabaesUpgrade.FromVersionNr = lrRecordset("FromVersion").Value
                lrDatabaesUpgrade.ToVersionNr = lrRecordset("ToVersion").Value
                lrDatabaesUpgrade.SuccessfulImplementation = lrDatabaesUpgrade.ToVersionNr <= CDbl(lsDatabaseVersionNumber)

                If Not tableDatabaseUpgrade.ExistsDatabaseUpgradeInRichmond(lrDatabaesUpgrade.FromVersionNr, lrDatabaesUpgrade.ToVersionNr) Then
                    Call lrDatabaesUpgrade.Save()
                End If

                lrRecordset.MoveNext()
            End While

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try



    End Sub

    Function GetMinimumUpgradeVersionRequired() As String

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New RecordsetProxy

        lrRecordset.ActiveConnection = pdbConnection
        lrRecordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT MIN(cdbl(trim(ToVersion)))"
        lsSQLQuery &= " FROM DatabaseUpgrade"
        lsSQLQuery &= " WHERE SuccessfulImplementation = FALSE"
        lsSQLQuery &= "   AND cdbl(trim(ToVersion)) > " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1).ToString

        lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

        If (Not lrRecordset.EOF) And (Not IsDBNull(lrRecordset(0).Value)) Then
            GetMinimumUpgradeVersionRequired = lrRecordset(0).Value
            lrRecordset.Close()
        Else
            MsgBox("Error: GetMinimumUpgradeVersionRequired: no record returned")
            GetMinimumUpgradeVersionRequired = ""
        End If

    End Function

    Public Sub MarkAllPreviousDatabaseUpgradesSuccessful(ByVal aiLastSuccessfulUpgrade As Integer)


        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "UPDATE DatabaseUpgrade"
            lsSQLQuery &= " SET SuccessfulImplementation = TRUE"
            lsSQLQuery &= " WHERE UpgradeId < " & aiLastSuccessfulUpgrade.ToString

            Call pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub



    Public Function GetNextRequiredUpgrade(ByRef ar_upgrade As DatabaseUpgrade.Upgrade, Optional ByVal abSilent As Boolean = False) As DatabaseUpgrade.Upgrade

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New RecordsetProxy

        lrRecordset.ActiveConnection = pdbConnection
        lrRecordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= " FROM DatabaseUpgrade"
        lsSQLQuery &= " WHERE (csng(ToVersion) = ("
        lsSQLQuery &= "                     SELECT MAX(csng(upg2.ToVersion))"
        lsSQLQuery &= "                       FROM DatabaseUpgrade upg2"
        lsSQLQuery &= "                      WHERE SuccessfulImplementation = FALSE" 'i.e. Upgrade has not been performed yet
        lsSQLQuery &= "                        AND csng(ToVersion) <= " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
        lsSQLQuery &= "                     )"
        lsSQLQuery &= "   AND SuccessfulImplementation = FALSE)" 'i.e. Upgrade has not been performed yet
        lsSQLQuery &= "   OR"
        lsSQLQuery &= " (ToVersion = ("
        lsSQLQuery &= "                     SELECT MIN(csng((upg3.ToVersion))"
        lsSQLQuery &= "                       FROM DatabaseUpgrade upg3"
        lsSQLQuery &= "                      WHERE SuccessfulImplementation = FALSE" 'i.e. Upgrade has not been performed yet
        lsSQLQuery &= "                        AND csng(upg3.ToVersion) >= " & TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
        lsSQLQuery &= "                     )"
        lsSQLQuery &= "   AND SuccessfulImplementation = FALSE)" 'i.e. Upgrade has not been performed yet\
        lsSQLQuery &= " ORDER BY csng(ToVersion) DESC"


        lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

        If Not lrRecordset.EOF Then
            ar_upgrade.UpgradeId = lrRecordset("UpgradeId").Value
            ar_upgrade.FromVersionNr = Trim(lrRecordset("FromVersion").Value)
            ar_upgrade.ToVersionNr = Trim(lrRecordset("ToVersion").Value)
            ar_upgrade.SuccessfulImplementation = lrRecordset("SuccessfulImplementation").Value
            lrRecordset.Close()

            Return ar_upgrade
        Else
            If Not abSilent Then MsgBox("Error: GetNextRequiredUpgrade: no record returned")
            Return Nothing
        End If

    End Function



    Function GetRequiredUpgradeCount() As Integer

        Dim lsSQLQuery As String = ""
        Dim lrRecordset As New RecordsetProxy

        lrRecordset.ActiveConnection = pdbConnection
        lrRecordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT COUNT(*)"
        lsSQLQuery &= " FROM DatabaseUpgrade"
        lsSQLQuery &= " WHERE SuccessfulImplementation = FALSE"

        lrRecordset.Open(lsSQLQuery, , , pc_cmd_table)

        GetRequiredUpgradeCount = lrRecordset(0).Value

        lrRecordset.Close()

    End Function


    Sub MarkUpgradeAsSuccessfulImplementation(ByVal aiUpgradeId As Integer)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "UPDATE DatabaseUpgrade"
            lsSQLQuery &= " SET SuccessfulImplementation = TRUE"
            lsSQLQuery &= " WHERE UpgradeId = " & aiUpgradeId

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Module