Imports System.Reflection

Public Class frmDatabaseUpgrade

    Private Sub DatabaseUpgradeFrmLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------
        'Centre the form
        '-------------------------------
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (Me.Width / 2)
        Me.Top = (2 * (Screen.PrimaryScreen.WorkingArea.Height / 5)) - (Me.Height / 2)

        Call Me.SetupForm()
        Call Me.PrepareForUpgrade(prApplication.DatabaseVersionNr)

    End Sub

    Private Sub SetupForm()

        Me.progressbar.Value = 0
        Me.LabelUpgradeCompletionStatus.Text = ""

        label_application_version_nr.Text = prApplication.ApplicationVersionNr
        LabelCurrentDatabaseVersionNr.Text = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
        label_upgrade_version_required.Text = tableDatabaseUpgrade.GetMinimumUpgradeVersionRequired()

        Call Me.LoadDatabaseUpgradeHistory()

    End Sub

    Sub LoadDatabaseUpgradeHistory()

        Dim lsListItem As String
        Dim lsSQLQuery As String
        Dim lrRecordset As New ADODB.Recordset

        lrRecordset.ActiveConnection = pdbConnection
        lrRecordset.CursorType = pcOpenStatic

        '---------------------------------------------
        'First clear the lists
        '---------------------------------------------
        listbox_upgrades_performed.Items.Clear()
        listbox_upgrades_required.Items.Clear()

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= "  FROM DatabaseUpgrade"
        lsSQLQuery &= " WHERE csng(ToVersion) > " & My.Settings.DatabaseVersionNumber.ToString
        lsSQLQuery &= "    OR "
        lsSQLQuery &= "       (csng(ToVersion) <= " & My.Settings.DatabaseVersionNumber.ToString
        lsSQLQuery &= "        AND SuccessfulImplementation = TRUE)"
        lsSQLQuery &= " ORDER BY csng(ToVersion)"

        lrRecordset.Open(lsSQLQuery)


        While Not lrRecordset.EOF
            If lrRecordset("SuccessfulImplementation").Value Then
                lsListItem = lrRecordset("FromVersion").Value & " to " & lrRecordset("ToVersion").Value & " : Success"
                listbox_upgrades_performed.Items.Add(New tComboboxItem(0, lsListItem))
            Else
                lsListItem = lrRecordset("FromVersion").Value & " to " & lrRecordset("ToVersion").Value & " : Required"
                listbox_upgrades_required.Items.Add(New tComboboxItem(0, lsListItem))
            End If
            lrRecordset.MoveNext()
        End While
        lrRecordset.Close()


    End Sub

    ''' <summary>
    ''' RETURNS TRUE if SuccessfulImplementation
    '''  ELSE FALSE if NOT SuccessfulImplementation
    '''             OR No RequiredUpgrade exists
    ''' </summary>
    ''' <param name="aiUpgradeId">The UpgradeId of the DatabaseUpgrade to be performed</param>
    ''' <param name="asFromVersionNr">The database VersionNr from which the Upgrade will be performed
    ''' Must be the current VersionNr of the Boston installation database.</param>
    ''' <param name="asToVersionNr">The database VersionNr that the installation datbase will be upgraded to.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PerformNextRequiredDatabaseUpgrade(ByVal aiUpgradeId As String, ByVal asFromVersionNr As String, ByVal asToVersionNr As String) As Boolean

        Dim lsUpgradeSQL As String
        Dim lsMessage As String
        Dim lsStallMessage As String = ""
        Dim liProgressBarValue As Integer
        Dim lsErrorMessage As String
        Dim lrDatabaseUpgradeSQL As DatabaseUpgrade.UpgradeSQL
        Dim lsSQLQuery As String
        Dim lrRecordset As New ADODB.Recordset

        Dim transaction As OleDb.OleDbTransaction = Nothing

        Try

            lrRecordset.ActiveConnection = pdbDatabaseUpgradeConnection
            lrRecordset.CursorType = pcOpenStatic

            '-----------------------------------------------------------------
            'In expectation of success...setup the ProgressBar
            '-----------------------------------------------------------------
            LabelUpgradeCompletionStatus.Visible = False 'Initially
            progressbar.Maximum = tableDatabasUpgradeSQL.GetUpgradeSequenceStepCount(aiUpgradeId)
            liProgressBarValue = 0

            '-----------------------------------------------------------------
            'Get the set of SQLStatements that need to be performed.
            '-----------------------------------------------------------------
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM UpgradeSQL"
            lsSQLQuery &= " WHERE UpgradeId = " & aiUpgradeId
            lsSQLQuery &= " ORDER BY SequenceNr"

            lrRecordset.Open(lsSQLQuery)

            If Not lrRecordset.EOF Then
                'pdbConnection.BeginTrans()

                transaction = pdb_OLEDB_connection.BeginTransaction()

                While Not lrRecordset.EOF

                    lsStallMessage = ""
                    '---------------------------------------------------
                    'Step through the set of required SQL statements
                    '  that need to be executed to perform the upgrade
                    '---------------------------------------------------
                    lrDatabaseUpgradeSQL = New DatabaseUpgrade.UpgradeSQL
                    lrDatabaseUpgradeSQL.UpgradeType = lrRecordset("UpgradeType").Value
                    lrDatabaseUpgradeSQL.TableName = Trim(Viev.NullVal(lrRecordset("TableName").Value, ""))
                    lrDatabaseUpgradeSQL.FieldName = Trim(Viev.NullVal(lrRecordset("FieldName").Value, ""))
                    lrDatabaseUpgradeSQL.OrdinalPosition = Viev.NullVal(lrRecordset("OrdinalPosition").Value, 0)
                    lrDatabaseUpgradeSQL.CodeToExecute = Trim(Viev.NullVal(lrRecordset("CodeToExecute").Value, ""))
                    lrDatabaseUpgradeSQL.AllowFail = CBool(lrRecordset("AllowFal").Value)

                    lsUpgradeSQL = Trim(Viev.NullVal(lrRecordset("SQLString").Value, ""))


                    lsMessage = lrRecordset("SequenceNr").Value
                    lsMessage &= vbCrLf
                    lsMessage &= lrDatabaseUpgradeSQL.UpgradeType
                    lsMessage &= vbCrLf
                    lsMessage &= lrDatabaseUpgradeSQL.TableName
                    lsMessage &= vbCrLf
                    lsMessage &= lrDatabaseUpgradeSQL.FieldName
                    lsMessage &= vbCrLf
                    lsMessage &= lrDatabaseUpgradeSQL.OrdinalPosition
                    lsMessage &= vbCrLf
                    lsMessage &= lsUpgradeSQL


                    Select Case lrDatabaseUpgradeSQL.UpgradeType
                        Case Is = 1 'STRAIGHT SQL STATEMENT

                            Dim lsUpgradeSQLCommands() = lsUpgradeSQL.Split(";")

                            For Each lsCommand In lsUpgradeSQLCommands
                                If Trim(lsCommand) <> "" Then
                                    Try
                                        'Call pdbConnection.Execute(lsCommand)
                                        Dim command As New OleDb.OleDbCommand(lsCommand)
                                        command.Connection = pdb_OLEDB_connection
                                        command.Transaction = transaction
                                        Call command.ExecuteNonQuery()
                                    Catch ex As Exception

                                        If lrDatabaseUpgradeSQL.AllowFail Then
                                            'Okay, nothing to do. Some INSERT statements, for instance, are allowed to fail because the data is already in the database.
                                        Else
                                            Dim lsMessage1 As String
                                            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                                            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                                            lsMessage1 &= vbCrLf & lsCommand
                                            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                                            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                                            GoTo error_handler
                                        End If
                                    End Try
                                End If
                            Next

                            'pdbConnection.Execute(lsUpgradeSQL)

                        Case Is = 2 'SQL ADD COLUMN WITH ORDINAL POSITION OF FIELD
                            pdbConnection.Execute(lsUpgradeSQL)

                            lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully executed SQL"
                            lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully refreshed TableDef"
                            lsStallMessage = lsStallMessage & vbCrLf & "Stall: Attempting to set OrdinalPosition: " & lrDatabaseUpgradeSQL.OrdinalPosition

                            '----------------------------------------------------------------------------------------------
                            'VM-20160518-Drop this code. ADO.Net does not support TableDefs. This was old DAO code, which
                            '  is no longer supported. There is no way to change the ordinal position of a field in a table with ADO.Net
                            'pdbConnection.TableDefs(Trim(lrDatabaseUpgradeSQL.TableName)).Fields(Trim(lrDatabaseUpgradeSQL.FieldName)).OrdinalPosition = lrDatabaseUpgradeSQL.OrdinalPosition
                            'lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully changed ordinal position"
                            'lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully refreshed TableDefs"

                        Case Is = 3 'SQL CREATE TABLE
                            pdbConnection.Execute(lsUpgradeSQL)

                        Case Is = 4 'SQL UPDATE INTRODUCING NEW CONSTRAINT
                            pdbConnection.Execute(lsUpgradeSQL)

                        Case Is = 5 'TABLE NAME CHANGE
                            pdbConnection.Execute(lsUpgradeSQL)
                            '----------------------------------------------------------------------------------------------
                            'VM-20171013-Drop this code. ADO.Net does not support TableDefs. This was old DAO code, which
                            '  is no longer supported. There is no way to change the ordinal position of a field in a table with ADO.Net
                            'pdbConnection.TableDefs(lsUpgradeSQL).name = Trim(lrDatabaseUpgradeSQL.TableName)

                        Case Is = 6 'ALTER TABLE DROP COLUMN
                            pdbConnection.Execute(lsUpgradeSQL)

                        Case Is = 7 'EXECUTE CODE WITHIN Boston
                            '-------------------------------------------------------------
                            'Because it is known in advance and on release which code
                            '  must be executed to fulfill a release...then it must be
                            '  hard-coded, and may as well be referenced from right here.
                            '-------------------------------------------------------------
                            Call transaction.Commit()
                            If Not Database.Database.ExecuteUpgradeCode(lrDatabaseUpgradeSQL.CodeToExecute) Then
                                GoTo error_handler
                            End If
                            transaction = pdb_OLEDB_connection.BeginTransaction()
                    End Select

                    '--------------------------------------
                    'Increment the ProgressBarValue
                    '--------------------------------------
                    liProgressBarValue += 1
                    progressbar.Value = liProgressBarValue
                    lrRecordset.MoveNext()
                End While
                'pdbConnection.CommitTrans() '(dbForceOSFlush)
                Call transaction.Commit()


                PerformNextRequiredDatabaseUpgrade = True
                LabelUpgradeCompletionStatus.Text = "Upgrade from database version '" & asFromVersionNr & "' to databse version '" & asToVersionNr & "' completed successfully!"
                LabelUpgradeCompletionStatus.Visible = True
                lrRecordset.Close()
            Else
                MsgBox("Error: PerformNextRequiredDatabaseUpgrade: No set of SQLStatements returned for UpgradeId: " & aiUpgradeId)
                PerformNextRequiredDatabaseUpgrade = False
            End If

            Me.Cursor = Cursors.Default

            Return True

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            PerformNextRequiredDatabaseUpgrade = False

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "The following step in the upgrade failed:"
            'lsMessage &= vbCrLf & vbCrLf
            'lsMessage &= "Upgrade Type: " & lrDatabaseUpgradeSQL.UpgradeType
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Sequence#: " & lrRecordset("SequenceNr").Value
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "Stall Message/s: " & lsStallMessage
            lsMessage &= vbCrLf & vbCrLf
            lsMessage &= "SQL: " & lrRecordset("SQLString").Value
            lsMessage &= vbCrLf & vbCrLf

            lsMessage &= vbCrLf & vbCrLf & ex.Message

            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            pdbConnection.RollbackTrans()

        End Try

error_handler:
        Cursor = Cursors.Default
        'pdbConnection.Rollback()
        Call transaction.Rollback()

        lsErrorMessage = Err.Number & ", " & Err.Source & ", " & Err.Description
        lsMessage &= "Error Message: " & lsErrorMessage
        MsgBox(lsMessage)

        PerformNextRequiredDatabaseUpgrade = False

    End Function

    Sub PrepareForUpgrade(ByVal asRequiredApplicationDatabaseVersionNr As String)

        '---------------------------------------------------
        'Check to see if the database is the correct version
        '---------------------------------------------------
        If TableReferenceFieldValue.GetReferenceFieldValue(1, 1) = asRequiredApplicationDatabaseVersionNr Then
            '-------------------------------------------
            'Database is up to date.
            '  i.e. The databaseVersion requried by the Application and the current DatabaseVersion are the same
            '-------------------------------------------
            label_current_upgrade_status.Visible = True
            '-------------------------------------------
            'Blank out those controls not needed
            '-------------------------------------------
            labelprompt_upgrade_version_required.Visible = False
            label_upgrade_version_required.Visible = False
            button_upgrade.Enabled = False
            button_upgrade.Visible = False
        Else
            '----------------------------------------------------------
            'Database is not of the version expected by the application
            '...so definitely needs upgrade
            '----------------------------------------------------------
            If GetRequiredUpgradeCount() > 0 Then
                '-----------------------------------------------------------------------------
                'The 'upgrade' table is suitably populated with upgrades not yet performed
                '  successfully.
                '-----------------------------------------------------------------------------
                label_upgrade_version_required.Text = GetMinimumUpgradeVersionRequired()
                labelprompt_current_database_version_nr.Visible = True
                LabelCurrentDatabaseVersionNr.Visible = True
                label_upgrade_version_required.Visible = True
                button_upgrade.Enabled = True
                button_upgrade.Visible = True
            End If
        End If


    End Sub


    Private Sub button_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_cancel.Click

        Try
            pdbConnection.RollbackTrans()
        Catch ex As Exception

        End Try
        Me.Close()

    End Sub


    Private Sub button_upgrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_upgrade.Click

        Dim lrDatabaesUpgrade As New DatabaseUpgrade.Upgrade

        Try
            '-----------------------------------------------------------------
            'Get the UpgradeId of the next Upgrade that needs to be performed
            '-----------------------------------------------------------------
            Call tableDatabaseUpgrade.GetNextRequiredUpgrade(lrDatabaesUpgrade)

            If Me.PerformNextRequiredDatabaseUpgrade(lrDatabaesUpgrade.UpgradeId, lrDatabaesUpgrade.FromVersionNr, lrDatabaesUpgrade.ToVersionNr) Then
                '------------------------------------------------
                'Update the Boston DatabaseVersionNr
                '------------------------------------------------
                Call Richmond.UpdateDatabaseVersion(lrDatabaesUpgrade.ToVersionNr)
                Call tableDatabaseUpgrade.MarkUpgradeAsSuccessfulImplementation(lrDatabaesUpgrade.UpgradeId)
                Call Me.LoadDatabaseUpgradeHistory()
                '------------------------------------------------
                'Update the form so that the user knows that the
                '  upgrade has been successful
                '------------------------------------------------
                LabelCurrentDatabaseVersionNr.Text = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)
            End If

            Call Me.PrepareForUpgrade(My.Settings.DatabaseVersionNumber.ToString)

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


End Class