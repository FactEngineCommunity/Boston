Imports System.Reflection
Imports adox

Namespace Database

    Public Module DatabaseModule

        Public Sub CompactAndRepairDatabase()

            Try

                pdbConnection.Close()
                pdb_OLEDB_connection.Close()

                Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                lrSQLConnectionStringBuilder.ConnectionString = My.Settings.DatabaseConnectionString

                Dim lsDatabaseLocationName As String = lrSQLConnectionStringBuilder("Data Source")
                Dim lsCompactedDatabaseLocationName As String
                lsCompactedDatabaseLocationName = New System.IO.FileInfo(lsDatabaseLocationName).DirectoryName & "\BostonCompacted.vdb"

                Try
                    Call Boston.CompactAccessDB(lsDatabaseLocationName, lsCompactedDatabaseLocationName)
                Catch ex As Exception
                    prApplication.ThrowErrorMessage("Failed to compact the database. Check to see if any other application has the database open.", pcenumErrorType.Warning)
                End Try

                Call Boston.OpenDatabase()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Function MakeStringSafe(ByVal asString As String) As String

            Dim lsReturnString As String = ""

            lsReturnString = asString.Replace("""", "''")
            lsReturnString = asString.Replace("'", "''")


            Return lsReturnString

        End Function

        Public Function RevertString(ByVal asString As String) As String

            Dim lsReturnString As String = ""

            lsReturnString = asString.Replace("''", "`")

            Return lsReturnString

        End Function

        Public Function IsLastRequiredUpgradeById(ByVal aiUpgradeId As Integer) As Boolean

            Try
                Dim lsSQLQuery As String
                Dim lrRecordset As New ADODB.Recordset

                lrRecordset.ActiveConnection = pdbDatabaseUpgradeConnection
                lrRecordset.CursorType = pcOpenStatic

                '-----------------------------------------------------------------
                'Get the set of SQLStatements that need to be performed.
                '-----------------------------------------------------------------
                lsSQLQuery = "SELECT MAX(UpgradeId) AS MaxUpgradeId"
                lsSQLQuery &= "  FROM Upgrade"

                lrRecordset.Open(lsSQLQuery)

                If aiUpgradeId >= CInt(lrRecordset("MaxUpgradeId").Value) Then
                    Return True
                End If

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Returns the wildcard operator for a database type. I.e. e.g. * or %
        ''' </summary>
        ''' <param name="aiDatabaseType"></param>
        ''' <returns></returns>
        Public Function getLikeWildcardOperator(ByVal aiDatabaseType As pcenumDatabaseType) As String

            Select Case aiDatabaseType
                Case Is = pcenumDatabaseType.MSJet
                    Return "*"
                Case Is = pcenumDatabaseType.TypeDB,
                          pcenumDatabaseType.Neo4j,
                          pcenumDatabaseType.KuzuDB
                    Return ".*"
                Case Else
                    Return "%"
            End Select

        End Function

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
            Dim lsErrorMessage As String
            Dim lrDatabaseUpgradeSQL As DatabaseUpgrade.UpgradeSQL
            Dim lsSQLQuery As String
            Dim lrRecordset As New ADODB.Recordset

            Dim transaction As Object '20230513-VM-Was OleDb.OleDbTransaction = Nothing '(Changed when moved to SQLite database for Boston)
            Dim locker As Object = New Object()

            Call Boston.WriteToStatusBar("Performing database upgrade from Version " & asFromVersionNr & " to Version " & asToVersionNr, True)

            Try
                Dim lsSQLFilePath = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\TempFiles\"

                lrRecordset.ActiveConnection = pdbDatabaseUpgradeConnection
                lrRecordset.CursorType = pcOpenStatic

                '-----------------------------------------------------------------
                'Get the set of SQLStatements that need to be performed.
                '-----------------------------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= "  FROM UpgradeSQL"
                lsSQLQuery &= " WHERE UpgradeId = " & aiUpgradeId
                lsSQLQuery &= " ORDER BY SequenceNr"

                lrRecordset.Open(lsSQLQuery)

                If Not lrRecordset.EOF Then

                    pdbConnection.BeginTrans()
                    'transaction = pdb_OLEDB_connection.BeginTransaction()

                    Dim liStepCount As Integer = lrRecordset.RecordCount
                    Dim liStep = 1
                    While Not lrRecordset.EOF

                        Call prApplication.WriteToStatusBar(".", True, Math.Min(100, (liStep / liStepCount) * 100), True)

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
                        lrDatabaseUpgradeSQL.AllowFail = CBool(lrRecordset("AllowFail").Value)
                        lsUpgradeSQL = Trim(Viev.NullVal(lrRecordset("SQLString").Value, ""))
                        lrDatabaseUpgradeSQL.CodeToExecute = Trim(Viev.NullVal(lrRecordset("CodeToExecute").Value, ""))

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
#Region "Old 1"
                            'Case Is = 1 'STRAIGHT SQL STATEMENT

                            '    Dim lsUpgradeSQLCommands() = lsUpgradeSQL.Split(";")

                            '    For Each lsCommand In lsUpgradeSQLCommands
                            '        If Trim(lsCommand) <> "" Then
                            '            Try
                            '                Call pdbConnection.Execute(lsCommand)

                            '            Catch ex As Exception

                            '                Try

                            '                    Dim command As New OleDb.OleDbCommand(lsCommand, pdb_OLEDB_connection)
                            '                    Call command.ExecuteNonQuery()

                            '                Catch ex1 As Exception

                            '                    If lrDatabaseUpgradeSQL.AllowFail Then
                            '                        'Nothing to do here. E.g. An INSERT need not fail if the data already exists.
                            '                    Else
                            '                        Dim lsMessage1 As String
                            '                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                            '                        lsMessage1 = "There was an error upgrading the Boston database."
                            '                        lsMessage1.AppendDoubleLineBreak("Error: " & mb.ReflectedType.Name & "." & mb.Name)
                            '                        lsMessage1 &= vbCrLf & lsCommand
                            '                        lsMessage1 &= vbCrLf & vbCrLf & ex1.Message
                            '                        prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex1.StackTrace)

                            '                        GoTo error_handler
                            '                    End If
                            '                End Try
                            '            End Try


                            '        End If
                            '    Next
#End Region
#Region "Old 2"
                            'Case Is = 2 'SQL ADD COLUMN WITH ORDINAL POSITION OF FIELD

                            '    Try
                            '        pdbConnection.Execute(lsUpgradeSQL)
                            '    Catch ex As Exception

                            '        If lrDatabaseUpgradeSQL.AllowFail Then
                            '            'Nothing to do here. E.g. Some INSERT statements are okay to fail because the data already exists in the database.
                            '        Else
                            '            Throw New Exception(ex.Message, ex.InnerException)
                            '        End If
                            '    End Try


                            '    lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully executed SQL"
                            '    lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully refreshed TableDef"
                            '    lsStallMessage = lsStallMessage & vbCrLf & "Stall: Attempting to set OrdinalPosition: " & lrDatabaseUpgradeSQL.OrdinalPosition

                            ''----------------------------------------------------------------------------------------------
                            ''VM-20160518-Drop this code. ADO.Net does not support TableDefs. This was old DAO code, which
                            ''  is no longer supported. There is no way to change the ordinal position of a field in a table with ADO.Net
                            ''pdbConnection.TableDefs(Trim(lrDatabaseUpgradeSQL.TableName)).Fields(Trim(lrDatabaseUpgradeSQL.FieldName)).OrdinalPosition = lrDatabaseUpgradeSQL.OrdinalPosition
                            ''lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully changed ordinal position"
                            ''lsStallMessage = lsStallMessage & vbCrLf & "Stall: Successfully refreshed TableDefs"
#End Region
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
#Region "7"
                                '-------------------------------------------------------------
                                'Because it is known in advance and on release which code
                                '  must be executed to fulfill a release...then it must be
                                '  hard-coded, and may as well be referenced from right here.
                                '-------------------------------------------------------------
                                Try
                                    Call transaction.Commit()
                                Catch ex As Exception
                                    'Not a biggie. transaction reset below.
                                End Try

                                Dim lbIsLastRequiredUpgrade As Boolean = Database.IsLastRequiredUpgradeById(CInt(aiUpgradeId))

                                If Not Database.ExecuteUpgradeCode(lrDatabaseUpgradeSQL.CodeToExecute, lbIsLastRequiredUpgrade) Then
                                    GoTo error_handler
                                End If
                                transaction = pdb_OLEDB_connection.BeginTransaction()
#End Region
                            Case Is = 8 'Create Proceedure
#Region "8"
                                'NB Can use
                                'CREATE VIEW MDB AS
                                'Select *
                                'From MetaModelModelDictionary

                                '20220307-VM-To Do. Not yet implemented
                                Throw New NotImplementedException("Not yet implemented. Type 8 upgrade type.")
                                Dim cat As New ADOX.Catalog
                                cat.ActiveConnection = pdbConnection
                                'Delete existing procedure
                                cat.Procedures.Delete("strProcedureName")

                                'Create new procedure
                                Dim cmd As New ADODB.Command
                                cmd.CommandText = "strSQLStatement"
                                cat.Procedures.Append("ProcedureName", cmd)
#End Region
                            Case Is = 1, 2, 9
                                'Use DBWConsole

                                Try
                                    SyncLock locker

                                        Threading.Thread.Sleep(3500)

                                        Try
                                            Boston.WaitForFile(lsSQLFilePath & "\script.sql", IO.FileMode.Open, IO.FileAccess.Write, IO.FileShare.ReadWrite)
                                            System.IO.File.Delete(lsSQLFilePath & "\script.sql")
                                        Catch ex As Exception
                                            'Not a biggie.
                                        End Try

                                        Using streamWriter As New System.IO.StreamWriter(lsSQLFilePath & "\script.sql", False)

                                            streamWriter.WriteLine(lsUpgradeSQL & ";")
                                            streamWriter.Close()
                                        End Using

                                        Dim lsShellCommand As String = Boston.MyPath & "\dbwconsole\DBWConsole.exe " & lsSQLFilePath & "script.sql " & lsSQLFilePath & "boston.mdb /e"

                                        Boston.WaitForFile(lsSQLFilePath & "boston.mdb", IO.FileMode.Open, IO.FileAccess.ReadWrite, IO.FileShare.ReadWrite)


                                        Shell(lsShellCommand, lrDatabaseUpgradeSQL.AllowFail, True, 5000)

                                    End SyncLock

                                Catch ex As Exception

                                    If lrDatabaseUpgradeSQL.AllowFail Then
                                        'Nothing to do here. E.g. Some INSERT statements are okay to fail because the data already exists in the database.
                                    Else
                                        Throw New Exception(ex.Message & vbCrLf & vbCrLf & ex.StackTrace, ex.InnerException)
                                    End If
                                End Try


                        End Select

                        liStep += 1
                        lrRecordset.MoveNext()
                    End While

                    pdbConnection.CommitTrans()
                    'Call transaction.Commit()

                    PerformNextRequiredDatabaseUpgrade = True
                    lrRecordset.Close()

                Else
                    MsgBox("Error: PerformNextRequiredDatabaseUpgrade: No set of SQLStatements returned for UpgradeId: " & aiUpgradeId)
                    PerformNextRequiredDatabaseUpgrade = False
                End If

                Return True

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf
                lsMessage.AppendDoubleLineBreak(ex.Message)
                lsMessage.AppendDoubleLineBreak("The following step in the upgrade failed:")
                'lsMessage &= vbCrLf & vbCrLf
                'lsMessage &= "Upgrade Type: " & lrDatabaseUpgradeSQL.UpgradeType
                lsMessage.AppendLine("Stall Message/s: " & lsStallMessage)
                Try
                    lsMessage &= vbCrLf
                    lsMessage &= "Sequence#: " & lrRecordset("SequenceNr").Value

                    lsMessage &= vbCrLf
                    lsMessage &= "SQL: " & lrRecordset("SQLString").Value
                Catch ex1 As Exception
                    'Tried to get as much information as possible.
                End Try

                lsMessage &= vbCrLf & vbCrLf & ex.Message

                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()

                Return False

            End Try

error_handler:

            pdbConnection.RollbackTrans()
            'Call transaction.Rollback()

            lsErrorMessage = Err.Number & ", " & Err.Source & ", " & Err.Description
            lsMessage &= "Error Message: " & lsErrorMessage
            MsgBox(lsMessage)

            Return False

        End Function

        ''' <summary>
        ''' Used when the Upgrade requires Boston to run code in Boston.
        ''' </summary>
        ''' <param name="asUpgradeCodeName"></param>
        ''' <param name="abIsLastRequiredUpgrade">Used to determine whether to run the code or not.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ExecuteUpgradeCode(ByVal asUpgradeCodeName As String,
                                    ByVal abIsLastRequiredUpgrade As Boolean) As Boolean

            ExecuteUpgradeCode = True

            Select Case Trim(asUpgradeCodeName)
            '----------------------------------
                'Put calls to code execution here
                '----------------------------------
                Case Is = "CommitTransaction"
                    Call DatabaseUpgradeFunctions.CommitTransaction(True)
                Case Is = "CreateApplicationKey"
                    Call DatabaseUpgradeFunctions.CreateApplicationKey()
                Case Is = "DoNothingDummyFunction"
                    Call DatabaseUpgradeFunctions.DoNothingDummyFunction()
                Case Is = "SetInitialFactTypeReadingTypedPredicateIds"
                    Call DatabaseUpgradeFunctions.SetInitialFactTypeReadingTypedPredicateIds()
                Case Is = "UpgradePredicatePartRoleIds"
                    Call DatabaseUpgradeFunctions.UpgradePredicatePartRoleIds()
                Case Is = "InsertNewPredicatePartRecordsForRoleIds"
                    Call DatabaseUpgradeFunctions.InsertNewPredicatePartRecordsForRoleIds()
                Case Is = "AddValueTypeGUIDs"
                    Call DatabaseUpgradeFunctions.AddValueTypeGUIDs()
                Case Is = "AddFactTypeGUIDs"
                    Call DatabaseUpgradeFunctions.AddFactTypeGUIDs()
                Case Is = "AddRoleConstraintGUIDs"
                    Call DatabaseUpgradeFunctions.AddRoleConstraintGUIDs()
                Case Is = "ReplaceCoreModel"
                    If abIsLastRequiredUpgrade Then
                        'Only replace the Core metamodel if lbIsLastRequiredUpgrade, because otherwise the Model. Save might fail if the Model data structure has changed.
                        '  This basically requires that the last thing that is done each release is to update the Core metamodel, so that 
                        '  customers who delay doing an upgrade have the Core metamodel updated as and when required.
                        Call DatabaseUpgradeFunctions.ReplaceCoreModel()
                    End If
                Case Is = "ReplaceUniversityModel"
                    Call DatabaseUpgradeFunctions.ReplaceUniversityModel()
                Case Else
                    MsgBox("Error: ExecuteUpgradeCode: Ability to process code '" & asUpgradeCodeName & "' does not exist. Please contact your vendor with this error message.")
                    ExecuteUpgradeCode = False
            End Select

        End Function

    End Module

End Namespace




