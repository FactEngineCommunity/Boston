Imports System.IO
Imports System.Reflection
Imports System.Data.OleDb
Imports System.Configuration
Imports ADOX
Imports System.Data.SQLite
Imports System.IO.Path
Imports System.Globalization
Imports System.Data.Common

Namespace Database

    Public Module DatabaseModule

        ' Constants for the backup intervals
        Private Const DaysInWeek As Integer = 7
        Private Const DaysInMonth As Integer = 30 ' Approximation for a month, adjust as needed

        Public Function OpenDatabase(Optional ByVal asDatabaseLocationFile As String = Nothing,
                                     Optional abSaveToRegistry As Boolean = True) As Boolean

            Dim lsMessage As String = ""
            Dim lsConnectionString As String = ""
            Dim lsDatabaseLocation As String = ""
            Dim lsDataProvider As String = ""
            Dim lbUserDatabaseIntervention As Boolean = False 'If True then need to ignore Registry setting for database.

            Try
                '------------------------------------------------
                'Define the location of your database
                'as follows if you want to use a relative folder.
                '------------------------------------------------
                lsConnectionString = Trim(My.Settings.DatabaseConnectionString)

                '-------------------------------------------
                'Sample Database ConnectionStrings
                '-------------------------------------------
#Region "Exapmle Database ConnectionStrings"
                'SQL Server Connection String Sample
                'DRIVER=SQL Server;UID=s2\vmorgante;PWD=cisco1;Trusted_Connection=;DATABASE=PreviewDialler;WSID=SIS2WKVM;APP=Microsoft Office 2003;SERVER=SIS2DB02;Description=PreviewDialler
                'lrSQLConnectionStringBuilder used to interogate the connection string.
                'Dim lrSQLConnectionStringBuilder As New SqlConnectionStringBuilder
                'lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString
                '-----------------------------------
                'MDB Connection String Sample
                'Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Program Files\PreviewDialler\database\PREVIEWDIALLER.mdb
                '-------------------------------------------
#End Region
                '------------------------------------------------
                'Construct the connection string
                '------------------------------------------------                 
                Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                Dim lsDatabaseType As String = My.Settings.DatabaseType

#Region "User Intervention on Startup"
                If My.Computer.Keyboard.AltKeyDown Then
                    Dim lfrmCRUDBostonConfiguration As New frmCRUDBostonConfiguration
                    Call lfrmCRUDBostonConfiguration.ShowDialog()
                    My.Settings.DatabaseConnectionString = lfrmCRUDBostonConfiguration.msConnectionString
                    My.Settings.DatabaseType = lfrmCRUDBostonConfiguration.msDatabaseType
                    lfrmCRUDBostonConfiguration.Dispose()
                    lsConnectionString = My.Settings.DatabaseConnectionString
                    lsDatabaseType = My.Settings.DatabaseType
                    prApplication.ThrowMessage("User intervened/set database location:" & lsConnectionString, pcenumErrorType.Information)
                    lbUserDatabaseIntervention = True 'So an ignore getting Registry database file location.
                End If
#End Region

TestDatabase:
                If lsDatabaseType = pcenumDatabaseType.PostgreSQL.ToString Then
#Region "Postgres"
                    lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                    pdbConnection = New FactEngine.PostgreSQLConnection(Nothing, lsConnectionString, 10000, False)
                    pdb_OLEDB_connection = New FactEngine.PostgreSQLConnection(Nothing, lsConnectionString, 10000, False) '2023-For Now. Will Fail for SQLite databases when doing database upgrades.
#End Region
                ElseIf My.Settings.DatabaseType = pcenumDatabaseType.SQLite.ToString Then
#Region "SQLite"
                    lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                    pdbConnection = New FactEngine.SQLiteConnection(Nothing, lsConnectionString, 1000, False)
                    pdb_OLEDB_connection = New FactEngine.SQLiteConnection(Nothing, lsConnectionString, 1000, False) '2023-For Now. Will Fail for SQLite databases when doing database upgrades.

                    If pbLogStartup Then
                        Call prApplication.ThrowMessage("pdbConnection IsNot Nothing: " & (pdbConnection IsNot Nothing).ToString, pcenumErrorType.Warning)
                        Call prApplication.ThrowMessage("pdbConnection.Connection IsNot Nothing: " & (pdbConnection.Connection IsNot Nothing).ToString, pcenumErrorType.Warning)
                    End If

#Region "SHIFT KEY DOWN - User Points to database"
                    If My.Computer.Keyboard.ShiftKeyDown Then
UserSelectedDatabaseSQLite:
                        Using lrOpenFileDialog As New OpenFileDialog

                            lrOpenFileDialog.Filter = "Boston Database (*.db)|*.db; *.db|All Files|*.*"

                            If System.IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\database") Then
                                lrOpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\database"
                            End If

                            If lrOpenFileDialog.ShowDialog = DialogResult.OK Then

                                If File.Exists(lrOpenFileDialog.FileName) Then
                                    lsDatabaseLocation = lrOpenFileDialog.FileName
                                    'Reaffirm/Set My.Settings.DatabaseConnectionString (for upgrades etc).
                                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation
                                    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                                    My.Settings.Save()
                                    GoTo CheckExistsDatabaseLocation
                                Else
                                    MsgBox("No file exists at the given location/name.")
                                End If
                            End If

                        End Using

                    End If
#End Region

                    lsDatabaseLocation = Boston.returnIfTrue(asDatabaseLocationFile IsNot Nothing, asDatabaseLocationFile, lrSQLConnectionStringBuilder("Data Source"))

                    'Compare with Registry DatabaseConnectionString
                    Dim lsRegistryDatabaseLocation = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Boston", "DatabaseLocation", Nothing)
                    If Not lbUserDatabaseIntervention And lsRegistryDatabaseLocation IsNot Nothing Then
                        If lsDatabaseLocation <> lsRegistryDatabaseLocation Then
                            lsDatabaseLocation = lsRegistryDatabaseLocation
                        End If
                    End If

CheckExistsDatabaseLocationSQLite:
                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation


                    If Not System.IO.File.Exists(lsDatabaseLocation) Then
                        '-----------------------------------
                        'Try and find the database locally
                        '-----------------------------------
                        Try
                            lsDatabaseLocation = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Boston", "DatabaseLocation", Nothing)
                            If lsDatabaseLocation IsNot Nothing Then
                                If System.IO.File.Exists(lsDatabaseLocation) Then
                                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation

                                    If Not File.Exists(lsDatabaseLocation) Then GoTo StillCannotFindTheDatabase

                                    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                                    lsConnectionString = My.Settings.DatabaseConnectionString
                                    GoTo OpenConnection
                                End If
                            End If
                        Catch ex As Exception
                            'Not a biggie.
                        End Try

StillCannotFindTheDatabaseSQLite:
                        If Not My.Settings.SilentPreConfiguration Then

                            lsMessage = "Cannot find the Boston database at the default/configured location:"
                            lsMessage.AppendDoubleLineBreak(lsDatabaseLocation)
                            lsMessage.AppendDoubleLineBreak("If this is a not a new installation of Boston, contact FactEngine support.")
                            lsMessage.AppendDoubleLineBreak("Click [Yes] to locate the database yourself or [No] to close Boston.")
                            lsMessage.AppendDoubleLineBreak("The default name for the Boston database is boston.vdb")


                            If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                GoTo UserSelectedDatabaseSQLite
                            Else
                                Return False
                            End If

                        End If
                    End If
#End Region
                ElseIf My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then
#Region "MS Jet"
                    lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                    pdbConnection = New FactEngine.MSAccessConnection(Nothing, Nothing) ' ADODB.Connection 'New FactEngine.SQLiteConnection(Nothing, "", 1000, False)
                    pdb_OLEDB_connection = New OleDb.OleDbConnection 'New FactEngine.SQLiteConnection(Nothing, "", 1000, False) '2023-For Now. Will Fail for SQLite databases when doing database upgrades.

#Region "SHIFT KEY DOWN - User Points to database"
                    If My.Computer.Keyboard.ShiftKeyDown Then
UserSelectedDatabase:
                        Using lrOpenFileDialog As New OpenFileDialog

                            lrOpenFileDialog.Filter = "Boston Database (*.vdb, *.mdb)|*.vdb; *.mdb|All Files|*.*"

                            If System.IO.Directory.Exists(My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\database") Then
                                lrOpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\database"
                            End If

                            If lrOpenFileDialog.ShowDialog = DialogResult.OK Then

                                If File.Exists(lrOpenFileDialog.FileName) Then
                                    lsDatabaseLocation = lrOpenFileDialog.FileName
                                    'Reaffirm/Set My.Settings.DatabaseConnectionString (for upgrades etc).
                                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation
                                    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                                    My.Settings.Save()
                                    GoTo CheckExistsDatabaseLocation
                                Else
                                    MsgBox("No file exists at the given location/name.")
                                End If
                            End If

                        End Using
#End Region
                    End If

                    lsDatabaseLocation = Boston.returnIfTrue(asDatabaseLocationFile IsNot Nothing, asDatabaseLocationFile, lrSQLConnectionStringBuilder("Data Source"))
                    lsDataProvider = lrSQLConnectionStringBuilder("Provider")

CheckExistsDatabaseLocation:
                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation


                    If Not System.IO.File.Exists(lsDatabaseLocation) Then
#Region "Database can't be found"
                        '-----------------------------------
                        'Try and find the database locally
                        '-----------------------------------
                        Try
                            lsDatabaseLocation = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Boston", "DatabaseLocation", Nothing)
                            If lsDatabaseLocation IsNot Nothing Then
                                If My.Computer.Keyboard.CtrlKeyDown Then GoTo LastDitch : 
                                If System.IO.File.Exists(lsDatabaseLocation) Then
                                    lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation

                                    If Not File.Exists(lsDatabaseLocation) Then GoTo StillCannotFindTheDatabase

                                    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                                    lsConnectionString = My.Settings.DatabaseConnectionString
                                    GoTo OpenConnection
                                Else
LastDitch:
                                    MsgBox("Cannot find the Boston Database. Set the Database Type and Connection String and restart Boston.")
                                    Dim lfrmCRUDBostonConfiguration As New frmCRUDBostonConfiguration
                                    Call lfrmCRUDBostonConfiguration.ShowDialog()
                                    My.Settings.DatabaseConnectionString = lfrmCRUDBostonConfiguration.msConnectionString
                                    My.Settings.DatabaseType = lfrmCRUDBostonConfiguration.msDatabaseType
                                    lsConnectionString = My.Settings.DatabaseConnectionString
                                    lsDatabaseType = My.Settings.DatabaseType
                                    GoTo TestDatabase
                                End If
                            End If
                        Catch ex As Exception
                            'Not a biggie.
                        End Try

StillCannotFindTheDatabase:
                        If Not My.Settings.SilentPreConfiguration Then

                            lsMessage = "Cannot find the Boston database at the default/configured location:"
                            lsMessage.AppendDoubleLineBreak(lsDatabaseLocation)
                            lsMessage.AppendDoubleLineBreak("If this is a not a new installation of Boston, contact FactEngine support.")
                            lsMessage.AppendDoubleLineBreak("Click [Yes] to locate the database yourself or [No] to close Boston.")
                            lsMessage.AppendDoubleLineBreak("The default name for the Boston database is boston.vdb")


                            If MsgBox(lsMessage, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                                GoTo UserSelectedDatabase
                            Else
                                Return False
                            End If

                        End If

#Region "Abandoned 20220830"
                        'If System.IO.File.Exists(lsLocalDatabaseLocation) Then
                        '    lrSQLConnectionStringBuilder("Data Source") = lsLocalDatabaseLocation
                        '    My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                        '    lsMessage = "Saving the following as the Database Connection String for your installation of Boston."
                        '    lsMessage.AppendDoubleLineBreak(lrSQLConnectionStringBuilder.ConnectionString)
                        '    lsMessage.AppendDoubleLineBreak("Boston will start but you won't be able to use this database. Contact FactEngine to find out how to connect to the correct database.")
                        '    lsMessage.AppendDoubleLineBreak("To make changes to the Boston database connection string, go to [Boston]->[Configuration]")

                        '    If Not My.Settings.SilentPreConfiguration Then
                        '        MsgBox(lsMessage)
                        '    End If

                        '    My.Settings.Save()
                        '    lsConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                        'Else
                        '    lsMessage = "Cannot find the Boston database at:"
                        '    lsMessage &= vbCrLf & vbCrLf
                        '    lsMessage &= lsDatabaseLocation
                        '    lsMessage &= vbCrLf
                        '    lsMessage &= "or..."
                        '    lsMessage &= vbCrLf
                        '    lsMessage &= lsLocalDatabaseLocation
                        '    lsMessage &= vbCrLf & vbCrLf
                        '    'lsMessage &= "Adjust the setting, 'database_connection_str', in the 'Boston.exe.config' file and restart Boston."
                        '    lsMessage &= "Adjust the Database Connection String for your database and restart Boston."
                        '    MsgBox(lsMessage)
                        '    frmCRUDBostonConfiguration.ShowDialog()
                        '    Return False
                        'End If
#End Region
#End Region
                    End If
#End Region
                End If

OpenConnection:
                Dim regKey As Microsoft.Win32.RegistryKey
                regKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE", True)
                regKey.CreateSubKey("Boston")
                regKey.Close()
                If abSaveToRegistry Then
                    'Not saved to registry if opening Temp database for Database Upgrade.
                    regKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Boston", True)
                    regKey.SetValue("DatabaseLocation", lsDatabaseLocation)
                End If

                regKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Boston", True)
                Dim loConfiguration As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
                regKey.SetValue("ConfigurationFileLocation", loConfiguration.FilePath)

                'Below Failed...because no permissions.
                'My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Boston\Variables", "DatabaseLocation", lsDatabaseLocation)

                '------------------------------------------------
                'Open the (database) connection
                '------------------------------------------------
                lsConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                If pbLogStartup Then prApplication.ThrowMessage("pdbConnection Opening", pcenumErrorType.Warning)

                If pdbConnection.Open(lsConnectionString) Then

                    If pbLogStartup Then
                        prApplication.ThrowMessage("pdbConnection Opened: " & lsConnectionString, pcenumErrorType.Warning)
                    End If
                Else
                    Throw New Exception("Failed to Open database. Method: Open.")
                End If

#Region "OLEDB"
                Select Case My.Settings.DatabaseType
                    Case Is = pcenumDatabaseType.SQLite.ToString, pcenumDatabaseType.MSJet.ToString

                        'lsDatabaseLocation
                        lrSQLConnectionStringBuilder = New System.Data.Common.DbConnectionStringBuilder(True)
                        lrSQLConnectionStringBuilder("Data Source") = lsDatabaseLocation
                        lrSQLConnectionStringBuilder("Provider") = "Microsoft.ACE.OLEDB.12.0" ' "Microsoft.Jet.OLEDB.4.0"

                        pdb_OLEDB_connection.ConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                        pdb_OLEDB_connection.Open()
                End Select
#End Region

                'Database Version Number. Get because user may have intervened and changed database.
                prApplication.DatabaseVersionNr = TableReferenceFieldValue.GetReferenceFieldValue(1, 1)

                Return True

            Catch lo_ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & lo_ex.Message
                lsMessage.AppendDoubleLineBreak("Error: There was an error opening Boston database: ")
                lsMessage.AppendDoubleLineBreak(lsConnectionString)
                lsMessage.AppendDoubleLineBreak("'" & Trim(lo_ex.Message) & "'" & vbCrLf & lo_ex.StackTrace)
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning)
                Return False
            End Try

        End Function

#Region "Grandfather Father Son Regime"

        Public Sub PerformGrandfatherFatherSonBackup()

            Try
                Dim loSetting As Object = New System.Dynamic.ExpandoObject
                Dim larExpandoFields() As Object = {}

                larExpandoFields.Add(New With {.FieldName = "GFSBackupType", .Value = "LastGrandfatherBackupDate"})
                Dim larSettingTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(40,, larExpandoFields) ', loSetting
                Dim grandfatherBackupDate As DateTime = DateTime.ParseExact(larSettingTuples(0).LastBackupDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)

                larExpandoFields = {}
                larExpandoFields.Add(New With {.FieldName = "GFSBackupType", .Value = "LastFatherBackupDate"})
                larSettingTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(40,, larExpandoFields) ', loSetting
                Dim fatherBackupDate As DateTime = DateTime.ParseExact(larSettingTuples(0).LastBackupDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)

                larExpandoFields = {}
                larExpandoFields.Add(New With {.FieldName = "GFSBackupType", .Value = "LastSonBackupDate"})
                larSettingTuples = TableReferenceFieldValue.GetReferenceFieldValueTuples(40,, larExpandoFields) ', loSetting
                Dim sonBackupDate As DateTime = DateTime.ParseExact(larSettingTuples(0).LastBackupDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)

                ' Calculate the backup intervals
                Dim currentDate As DateTime = DateTime.Now
                Dim daysSinceGrandfatherBackup As Integer = (currentDate - grandfatherBackupDate).Days
                Dim daysSinceFatherBackup As Integer = (currentDate - fatherBackupDate).Days
                Dim daysSinceSonBackup As Integer = (currentDate - sonBackupDate).Days

                ' Perform backups as necessary
                If daysSinceGrandfatherBackup >= DaysInMonth Then
                    BackupDatabaseAndXml("Grandfather")
                    ' Update the last backup date in the database
                    loSetting = New With {.GFSBackupType = "LastGrandfatherBackupDate", .LastBackupDate = currentDate.ToString("yyyy-MM-dd")}
                    Dim larKeyFields() As Object = {}
                    larKeyFields.Add(New With {.FieldId = 1, .FieldName = "GFSBackupType", .Value = "LastGrandfatherBackupDate"})
                    Call TableReferenceTable.UpSert(40, loSetting, larKeyFields)
                End If
                If daysSinceFatherBackup >= DaysInWeek Then
                    BackupDatabaseAndXml("Father")
                    ' Update the last backup date in the database
                    loSetting = New With {.GFSBackupType = "LastFatherBackupDate", .LastBackupDate = currentDate.ToString("yyyy-MM-dd")}
                    Dim larKeyFields() As Object = {}
                    larKeyFields.Add(New With {.FieldId = 1, .FieldName = "GFSBackupType", .Value = "LastFatherBackupDate"})
                    Call TableReferenceTable.UpSert(40, loSetting, larKeyFields)
                End If
                If daysSinceSonBackup >= 1 Then
                    BackupDatabaseAndXml("Son")
                    'Update the last backup date in the database
                    loSetting = New With {.GFSBackupType = "LastSonBackupDate", .LastBackupDate = currentDate.ToString("yyyy-MM-dd")}
                    Dim larKeyFields() As Object = {}
                    larKeyFields.Add(New With {.FieldId = 1, .FieldName = "GFSBackupType", .Value = "LastSonBackupDate"})
                    Call TableReferenceTable.UpSert(40, loSetting, larKeyFields)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        ''' <summary>
        ''' Backs up the Database Location and XML Folder under that to a \Grandfather, \Father, or \Son location.
        ''' </summary>
        ''' <param name="asBackupType">Grandfather, Father, Son</param>
        Private Sub BackupDatabaseAndXml(asBackupType As String)

            Try
                Dim databaseLocation As String = ""
                Dim backupFolder As String = ""
                Dim databaseFile As String = ""
                Dim xmlSourceFolder As String = ""
                Dim xmlBackupFolder As String = ""

                databaseLocation = GetDatabaseDirectoryPathFromConnectionString(My.Settings.DatabaseConnectionString)
                backupFolder = System.IO.Path.Combine(databaseLocation, "Backup", asBackupType)
                'Database File
                databaseFile = GetDatabaseFileNameFromConnectionString(My.Settings.DatabaseConnectionString)

                'XML folder
                xmlSourceFolder = Path.Combine(databaseLocation, "XML")
                xmlBackupFolder = Path.Combine(backupFolder, "XML")


                Directory.CreateDirectory(backupFolder)
                Directory.CreateDirectory(xmlSourceFolder)
                Directory.CreateDirectory(xmlBackupFolder)
                File.Copy(Path.Combine(databaseLocation, databaseFile), Path.Combine(backupFolder, Path.GetFileName(databaseFile)), overwrite:=True)
                CopyDirectory(xmlSourceFolder, xmlBackupFolder)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Private Function GetDatabaseDirectoryPathFromConnectionString(connectionString As String) As String
            Try
                Select Case My.Settings.DatabaseType
                    Case Is = "MSJet"
                        ' Parse the connection string using DbConnectionStringBuilder.
                        Dim builder As New DbConnectionStringBuilder With {
                            .connectionString = connectionString
                        }
                        ' Extract the database file path from the connection string.
                        ' Assumes "Data Source" is the correct key in the connection string.
                        If builder.ContainsKey("Data Source") Then
                            Dim databaseFilePath As String = builder("Data Source").ToString()
                            ' Return the directory path without the database file name.
                            Return Path.GetDirectoryName(databaseFilePath)
                        Else
                            Throw New InvalidOperationException("The connection string does not contain a Data Source key.")
                        End If
                    Case Is = "SQLite"
                        ' Parse the connection string using DbConnectionStringBuilder.
                        Dim builder As New DbConnectionStringBuilder With {
                            .ConnectionString = connectionString
                        }
                        ' Extract the database file path from the connection string.
                        ' Assumes "Data Source" is the correct key in the connection string.
                        If builder.ContainsKey("Data Source") Then
                            Dim databaseFilePath As String = builder("Data Source").ToString()
                            ' Return the directory path without the database file name.
                            Return Path.GetDirectoryName(databaseFilePath)
                        Else
                            Throw New InvalidOperationException("The connection string does not contain a Data Source key.")
                        End If
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodBase.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                ' Replace the following line with your application's error handling
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return "<Error Getting Database Location from Connection String>"
            End Try
        End Function

        Private Function GetDatabaseFileNameFromConnectionString(connectionString As String) As String
            Try
                Select Case My.Settings.DatabaseType
                    Case Is = "MSJet"
                        ' Parse the connection string using DbConnectionStringBuilder.
                        Dim builder As New System.Data.Common.DbConnectionStringBuilder With {
                            .ConnectionString = connectionString
                        }
                        ' Extract the database file path from the connection string.
                        ' Assumes "Data Source" is the correct key in the connection string.
                        If builder.ContainsKey("Data Source") Then
                            Dim databaseFilePath As String = builder("Data Source").ToString()
                            ' Return the database file name with extension.
                            Return Path.GetFileName(databaseFilePath)
                        Else
                            Throw New InvalidOperationException("The connection string does not contain a Data Source key.")
                        End If
                    Case Is = "SQLite"
                        ' Parse the connection string using DbConnectionStringBuilder.
                        Dim builder As New System.Data.Common.DbConnectionStringBuilder With {
                            .ConnectionString = connectionString
                        }
                        ' Extract the database file path from the connection string.
                        ' Assumes "Data Source" is the correct key in the connection string.
                        If builder.ContainsKey("Data Source") Then
                            Dim databaseFilePath As String = builder("Data Source").ToString()
                            ' Return the database file name with extension.
                            Return Path.GetFileName(databaseFilePath)
                        Else
                            Throw New InvalidOperationException("The connection string does not contain a Data Source key.")
                        End If
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return "<Error Getting Database Location from Connection String>"
            End Try
        End Function

        Private Sub CopyDirectory(sourceDir As String, destDir As String)

            Try

                If Not Directory.Exists(destDir) Then
                    Directory.CreateDirectory(destDir)
                End If

                For Each file As String In System.IO.Directory.GetFiles(sourceDir)
                    System.IO.File.Copy(file, System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(file)), overwrite:=True)
                Next

                For Each directory As String In System.IO.Directory.GetDirectories(sourceDir)
                    CopyDirectory(directory, System.IO.Path.Combine(destDir, System.IO.Path.GetFileName(directory)))
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub


#End Region

        Public Sub CompactAccessDB(ByVal sFilePath As String, ByVal sNewFilePath As String)
            Try
                Dim sCompactError As String = ""

                Dim lsSourceConnectionString, lsNewConnectionString As String
                lsSourceConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", sFilePath)
                lsNewConnectionString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5", sNewFilePath)
                Dim jro As New JRO.JetEngine 'new instance of the jet engine
                jro.CompactDatabase(lsSourceConnectionString, lsNewConnectionString)

                System.IO.File.Delete(sFilePath)
                System.IO.File.Move(sNewFilePath, sFilePath)

            Catch ex As System.Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub


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
                    Call Database.CompactAccessDB(lsDatabaseLocationName, lsCompactedDatabaseLocationName)
                Catch ex As Exception
                    prApplication.ThrowMessage("Failed to compact the database. Check to see if any other application has the database open.", pcenumErrorType.Warning)
                End Try

                Call Database.OpenDatabase()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Function MakeStringSafe(ByVal asString As String) As String

            Dim lsReturnString As String = ""

            lsReturnString = asString.Replace("""", """")
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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
        Public Function PerformNextRequiredDatabaseUpgrade(ByVal aiUpgradeId As String,
                                                           ByVal asFromVersionNr As String,
                                                           ByVal asToVersionNr As String,
                                                           ByVal asTempDatabaseLocationFileName As String) As Boolean

            Dim lsUpgradeSQL As String
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

#Region "Prepare Error Message if required"
                        lsErrorMessage = lrRecordset("SequenceNr").Value
                        lsErrorMessage &= vbCrLf
                        lsErrorMessage &= lrDatabaseUpgradeSQL.UpgradeType
                        lsErrorMessage &= vbCrLf
                        lsErrorMessage &= lrDatabaseUpgradeSQL.TableName
                        lsErrorMessage &= vbCrLf
                        lsErrorMessage &= lrDatabaseUpgradeSQL.FieldName
                        lsErrorMessage &= vbCrLf
                        lsErrorMessage &= lrDatabaseUpgradeSQL.OrdinalPosition
                        lsErrorMessage &= vbCrLf
                        lsErrorMessage &= lsUpgradeSQL
#End Region


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
                            '                        prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex1.StackTrace)

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
                                    Select Case My.Settings.DatabaseType
                                        Case Is = "MSJet"
#Region "MSJet"
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
#End Region
                                        Case Is = "SQLite"
#Region "SQLite"
                                            Dim lrUpgradeRecordset As ORMQL.Recordset = pdbConnection.Execute(lsUpgradeSQL.Replace(vbCrLf, ""))

                                            If lrUpgradeRecordset.ErrorReturned Then
                                                Boston.ShowFlashCard("Error performing upgrade step".AppendDoubleLineBreak(lsErrorMessage), Color.Salmon)
                                                Return False
                                            End If
#End Region
                                    End Select

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

                Dim lsMessage As String = ""
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

                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()

                Return False

            End Try

error_handler:

            pdbConnection.RollbackTrans()
            'Call transaction.Rollback()

            lsErrorMessage = Err.Number & ", " & Err.Source & ", " & Err.Description
            lsErrorMessage &= "Error Message: " & lsErrorMessage
            MsgBox(lsErrorMessage)

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




