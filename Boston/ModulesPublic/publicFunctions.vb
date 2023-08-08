Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Text
Imports System.Xml.Serialization
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Threading.Tasks
Imports OpenAI_API
Imports OpenAI_API.Completions
Imports OpenAI_API.Models
Imports System.Net
Imports NAudio.Wave
Imports Newtonsoft.Json

Namespace Boston

    Public Module publicFunctions

        Private ReadOnly random As Random = New Random()

        ''' <summary>
        ''' This Function Will Extract The Exception Line Number and Method In Which
        ''' Exception Occurred And The Exception Message
        ''' </summary>
        ''' <param name="arException">Provide Exception Object</param>
        ''' <returns>Tab Separated String Of Extracted Exception</returns>
        Public Function ExtractLineAndMethod(ByRef arException As Exception, Optional abShowStackTrace As Boolean = False) As String

            Dim lrStackTrace As New StackTrace(arException, True)
            Dim lrMethodBase As MethodBase = lrStackTrace.GetFrame(lrStackTrace.FrameCount - 1).GetMethod()
            Try
                Dim lsMethodName As String = $"{lrMethodBase.ReflectedType.Name}.{lrMethodBase.Name}"
                Dim liLineNumber As Integer = lrStackTrace.GetFrame(lrStackTrace.FrameCount - 1).GetFileLineNumber()
                Dim lsMessage As String = $"Error: {lsMethodName} - {liLineNumber}" & vbCrLf & vbCrLf & arException.Message
                If abShowStackTrace Then
                    lsMessage &= vbCrLf & vbCrLf & arException.StackTrace.ToString()
                End If
                Return lsMessage
            Finally
                lrStackTrace = Nothing
                lrMethodBase = Nothing
            End Try

        End Function

        Public Function ListsCommonElementCount(ByVal list1 As List(Of String), ByVal list2 As List(Of String)) As Integer
            Dim commonCount As Integer = list1.Intersect(list2).Count()
            Return commonCount
        End Function

        Public Function RandomString(ByVal length As Integer) As String
            Const chars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
            Return New String(Enumerable.Repeat(chars, length).[Select](Function(s) s(random.[Next](s.Length))).ToArray())
        End Function

        Public Function RandomInteger(ByVal aiMin As Integer, aiMax As Integer) As Integer

            Return CInt(Int((aiMax * Rnd()) + aiMin))

        End Function

        Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As System.Windows.Forms.Keys) As Integer 'was vKey As Long

        <Extension()>
        Public Sub Add(Of T)(ByRef arr As T(), ByVal item As T)
            Array.Resize(arr, arr.Length + 1)
            arr(arr.Length - 1) = item
        End Sub

        ''' <summary>
        ''' Returns where the last word/s is/are an Adjective or Compound Adjective,
        '''   based on the word/s ending in: "-ful," "-ous," "-ing," "-ed," "-ive," "-ic," and "-ice".
        ''' </summary>
        ''' <param name="sentence"></param>
        ''' <param name="aiIndex"></param>
        ''' <returns></returns>
        Function ExtractLastAdjectiveFromSentence(ByVal sentence As String, ByRef aiIndex As Integer) As String

            Dim adjectiveEndingsPattern As String = "\b(\w+(?:ful|ous|ing|ed|ive|ic|ice))\b(?=(?:\s*\w+(?:ful|ous|ing|ed|ive|ic|ice)\b)*\s*$)" ''"(?:\w+ful\b|\w+ous\b|\w+ing\b|\w+ed\b|\w+ive\b|\w+ic\b|\w+ice\b)"
            Dim compoundAdjectivePattern As String = "(?:\b\w+\s+)*(?:" & adjectiveEndingsPattern & ")*\b"

            Dim lastAdjective As String = Nothing

            Dim matches As MatchCollection = Regex.Matches(sentence, adjectiveEndingsPattern)
            If matches.Count > 0 Then
                Dim lasMatches = matches.Cast(Of Match)().Select(Function(m) m.Value).ToArray
                lastAdjective = Strings.Join(lasMatches, " ")
                aiIndex = matches(0).Index
            Else
                Dim words As String() = sentence.Split(" "c)
                For i As Integer = words.Length - 1 To 0 Step -1
                    Dim currentWord As String = words(i)
                    If Regex.IsMatch(currentWord, adjectiveEndingsPattern) Then
                        lastAdjective = currentWord & If(String.IsNullOrEmpty(lastAdjective), "", " " & lastAdjective)
                    Else
                        Exit For
                    End If
                Next
            End If

            Return lastAdjective

        End Function

        Public Function GetAdjustedFont(ByVal g As Graphics, ByVal graphicString As String, ByVal originalFont As Font, ByVal containerWidth As Integer, ByVal maxFontSize As Integer, ByVal minFontSize As Integer, ByVal smallestOnFail As Boolean) As Font
            Dim testFont As Font = Nothing

            Try
                testFont = New Font(originalFont.Name, minFontSize, originalFont.Style)

                For adjustedSize As Integer = maxFontSize To minFontSize Step -1

                    testFont = New Font(originalFont.Name, adjustedSize + 1, originalFont.Style)

                    Dim adjustedSizeNew As SizeF = g.MeasureString(graphicString, testFont)

                    If containerWidth > Convert.ToInt32(adjustedSizeNew.Width) Then
                        Return testFont
                    End If
                Next

                If smallestOnFail Then
                    Return testFont
                Else
                    Return originalFont
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return originalFont
            End Try
        End Function

        Public Function GetConfigFileLocation() As String

            Return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString

        End Function

        Public Sub WaitForFile(ByVal fullPath As String, ByVal mode As FileMode, ByVal access As FileAccess, ByVal share As FileShare)
            Try
                For numTries As Integer = 0 To 10 - 1
                    Dim fs As FileStream = Nothing

                    Try
                        fs = New FileStream(fullPath, mode, access, share)
                        fs.Dispose()
                    Catch __unusedIOException1__ As IOException

                        If fs IsNot Nothing Then
                            fs.Dispose()
                        End If

                        System.Threading.Thread.Sleep(50)
                    End Try
                Next

            Catch ex As Exception
            Finally
                System.Threading.Thread.Sleep(50)
            End Try
        End Sub

        Public Function ResizeImage(ByVal image As Image, ByVal width As Integer, ByVal height As Integer) As Bitmap
            Dim destRect = New Rectangle(0, 0, width, height)
            Dim destImage = New Bitmap(width, height)
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution)

            Using lrgraphics = Graphics.FromImage(destImage)
                lrgraphics.CompositingMode = CompositingMode.SourceCopy
                lrgraphics.CompositingQuality = CompositingQuality.HighQuality
                lrgraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                lrgraphics.SmoothingMode = SmoothingMode.HighQuality
                lrgraphics.PixelOffsetMode = PixelOffsetMode.HighQuality

                Using wrapMode = New ImageAttributes()
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipX)
                    lrgraphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode)
                End Using
            End Using

            Return destImage

        End Function

        Public Function returnIfTrue(ByVal abBoolValue As Boolean,
                                     ByVal asReturnString As Object,
                                     ByVal asAlternateString As Object) As Object

            If abBoolValue Then
                Return asReturnString
            Else
                Return asAlternateString
            End If

        End Function

        Public Function ObjectHasProperty(obj As Object, propertyName As String) As Boolean
            Dim type As Type = obj.GetType()
            Dim propertyInfo As PropertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase Or BindingFlags.Public Or BindingFlags.Instance)
            Return propertyInfo IsNot Nothing
        End Function

        Public Function OpenDatabase(Optional ByVal asDatabaseLocationFile As String = Nothing) As Boolean

            Dim lsMessage As String = ""
            Dim lsConnectionString As String = ""
            Dim lsDatabaseLocation As String = ""
            Dim lsDataProvider As String = ""

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
                        Call prApplication.ThrowErrorMessage("pdbConnection IsNot Nothing: " & (pdbConnection IsNot Nothing).ToString, pcenumErrorType.Warning)
                        Call prApplication.ThrowErrorMessage("pdbConnection.Connection IsNot Nothing: " & (pdbConnection.Connection IsNot Nothing).ToString, pcenumErrorType.Warning)
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
                regKey = My.Computer.Registry.CurrentUser.OpenSubKey("SOFTWARE\Boston", True)
                regKey.SetValue("DatabaseLocation", lsDatabaseLocation)

                Dim loConfiguration As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal)
                regKey.SetValue("ConfigurationFileLocation", loConfiguration.FilePath)
                'Below Failed...because no permissions.
                'My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Boston\Variables", "DatabaseLocation", lsDatabaseLocation)

                '------------------------------------------------
                'Open the (database) connection
                '------------------------------------------------
                lsConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                If pbLogStartup Then prApplication.ThrowErrorMessage("pdbConnection Opening", pcenumErrorType.Warning)

                If pdbConnection.Open(lsConnectionString) Then

                    If pbLogStartup Then
                        prApplication.ThrowErrorMessage("pdbConnection Opened", pcenumErrorType.Warning)
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
                Return True

            Catch lo_ex As Exception

                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & lo_ex.Message
                lsMessage.AppendDoubleLineBreak("Error: There was an error opening Boston database: ")
                lsMessage.AppendDoubleLineBreak(lsConnectionString)
                lsMessage.AppendDoubleLineBreak("'" & Trim(lo_ex.Message) & "'" & vbCrLf & lo_ex.StackTrace)
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                Return False
            End Try

        End Function

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

        Public Function PageDataExistsInClipboard(ByRef arPage As FBM.Page) As Boolean

            Dim RichmondPage As DataFormats.Format = DataFormats.GetFormat("RichmondPage")

            Try
                If Clipboard.ContainsData(RichmondPage.Name) Then
                    Return True
                Else
                    Return False
                End If
                ''----------------------------------------
                '' Retrieve the data from the clipboard.
                ''----------------------------------------
                'Dim myRetrievedObject As IDataObject = Clipboard.GetDataObject()

                ''----------------------------------------------------
                '' Convert the IDataObject type to MyNewObject type. 
                ''----------------------------------------------------
                'Dim lrPage As FBM.Page 'Clipbrd.ClipboardPage

                'lrPage = CType(myRetrievedObject.GetData(RichmondPage.Name), FBM.Page) 'Clipbrd.ClipboardPage)

                'If lrPage Is Nothing Then
                '    Return False
                'Else
                '    arPage = lrPage
                '    Return True
                'End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        ''' <summary>
        ''' Displays the Generic Selection Form
        ''' </summary>
        ''' <param name="arGenericSelection"></param>
        ''' <param name="as_form_title"></param>
        ''' <param name="as_select_object"></param>
        ''' <param name="as_select_field"></param>
        ''' <param name="as_index_field"></param>
        ''' <param name="as_where_clause">SQL WHERE Clause. Include the 'WHERE' token.</param>
        ''' <param name="ao_combobox_item"></param>
        ''' <param name="aiComboboxStyle"></param>
        ''' <param name="asOrderByFields"></param>
        ''' <param name="aiSelectColumn"></param>
        ''' <param name="asColumnWidthString"></param>
        ''' <param name="asFieldList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DisplayGenericSelectForm(ByRef arGenericSelection As tGenericSelection,
                                                 ByVal as_form_title As String,
                                                 ByVal as_select_object As String,
                                                 ByVal as_select_field As String,
                                                 ByVal as_index_field As String,
                                                 Optional ByVal as_where_clause As String = "",
                                                 Optional ByVal ao_combobox_item As tComboboxItem = Nothing,
                                                 Optional ByVal aiComboboxStyle As pcenumComboBoxStyle = pcenumComboBoxStyle.DropdownList,
                                                 Optional ByVal asOrderByFields As String = Nothing,
                                                 Optional ByVal aiSelectColumn As Integer = 1,
                                                 Optional ByVal asColumnWidthString As String = "100",
                                                 Optional ByVal asFieldList As String = ""
                                                 ) As DialogResult

            arGenericSelection.SelectField = as_select_field
            arGenericSelection.IndexField = as_index_field
            arGenericSelection.TableName = as_select_object
            arGenericSelection.FormTitle = as_form_title
            arGenericSelection.SelectColumn = aiSelectColumn
            arGenericSelection.ColumnWidthString = asColumnWidthString
            arGenericSelection.FieldList = asFieldList

            Try

                If IsSomething(asOrderByFields) Then
                    arGenericSelection.OrderByFields = asOrderByFields
                Else
                    arGenericSelection.OrderByFields = as_select_field
                End If
                If Not IsNothing(as_where_clause) Then
                    arGenericSelection.WhereClause = as_where_clause
                Else
                    arGenericSelection.WhereClause = ""
                End If

                If IsSomething(ao_combobox_item) Then
                    arGenericSelection.TupleList.Add(ao_combobox_item)
                End If

                Select Case aiSelectColumn
                    Case Is = 1
                        Dim lfrm_generic_select_frm As New frmGenericSelect
                        If IsSomething(aiComboboxStyle) Then
                            Select Case aiComboboxStyle
                                Case Is = pcenumComboBoxStyle.Dropdown
                                    lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDown
                                Case Is = pcenumComboBoxStyle.DropdownList
                                    lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDownList
                                Case Is = pcenumComboBoxStyle.Simple
                                    lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.Simple
                            End Select
                        Else
                            lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDown
                        End If

                        lfrm_generic_select_frm.zoGenericSelection = arGenericSelection
                        DisplayGenericSelectForm = lfrm_generic_select_frm.ShowDialog()
                    Case Is > 1
                        Dim lfrm_generic_select_frm As New frmGenericSelectMultiColumn
                        If IsSomething(aiComboboxStyle) Then
                            Select Case aiComboboxStyle
                                Case Is = pcenumComboBoxStyle.Dropdown
                                    lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDown
                                Case Is = pcenumComboBoxStyle.DropdownList
                                    lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDownList
                                Case Is = pcenumComboBoxStyle.Simple
                                    lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.Simple
                            End Select
                        Else
                            lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDown
                        End If

                        lfrm_generic_select_frm.zoGenericSelection = arGenericSelection
                        DisplayGenericSelectForm = lfrm_generic_select_frm.ShowDialog()
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return DialogResult.Abort
            End Try

        End Function

        Function CloneObject(ByVal obj As Object) As Object
            ' Create a memory stream and a formatter.
            Dim ms As New MemoryStream(5000)
            Dim bf As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            ' Serialize the object into the stream.

            bf.Serialize(ms, obj)
            ' Position streem pointer back to first byte.        
            ms.Seek(0, SeekOrigin.Begin)
            ' Deserialize into another object.
            CloneObject = bf.Deserialize(ms)
            ' Release memory.
            ms.Close()

        End Function

        ''' <summary>
        ''' Displays a FlashCard on the screen.
        ''' </summary>
        ''' <param name="asText">Text to display.</param>
        ''' <param name="aoColor">The color of the FlashCard</param>
        ''' <param name="aiInterval">The interval, in milliseconds, to display the FlashCard for.</param>
        Public Sub ShowFlashCard(ByVal asText As String,
                                 ByVal aoColor? As Color,
                                 ByVal aiInterval As Integer,
                                 Optional ByVal aiFontSize As Single = 8.25)

            Try
                Dim lfrmFlashCard As New frmFlashCard
                lfrmFlashCard.ziIntervalMilliseconds = aiInterval
                lfrmFlashCard.zsText = asText
                lfrmFlashCard.Show(frmMain, aoColor)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        ''' <summary>
        '''  Encrypts/decrypts the passed string using a simple ASCII value-swapping algorithm
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SimpleCrypt(ByVal Text As String) As String
            Dim strTempChar As String = ""
            Dim i As Integer
            For i = 1 To Len(Text)
                If Asc(Mid$(Text, i, 1)) < 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
                ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
                End If
                Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
            Next i
            Return Text
        End Function

        Public Function IsValidEmailAddress(ByVal asString As String) As Boolean

            IsValidEmailAddress = True

            Try
                Dim loEmailAddress As New System.Net.Mail.MailAddress(asString)

            Catch ex As Exception

                IsValidEmailAddress = False

            End Try

        End Function

        ''' <summary>
        ''' Writes to the status bar of the Main form (frmMain).
        ''' </summary>
        ''' <param name="asMessage"></param>
        ''' <param name="abRefreshForm">Refreshes the foorm (frmMain)</param>
        ''' <param name="aiProgressPercent"></param>
        ''' <param name="abAppendMessageOnly">As when adding dots (.) to a message that already exists.</param>
        Public Sub WriteToStatusBar(ByVal asMessage As String,
                                    Optional ByVal abRefreshForm As Boolean = False,
                                    Optional ByVal aiProgressPercent As Integer = 0,
                                    Optional abAppendMessageOnly As Boolean = False)

            If abAppendMessageOnly Then
                frmMain.StatusLabelGeneralStatus.Text = frmMain.StatusLabelGeneralStatus.Text & asMessage
            Else
                frmMain.StatusLabelGeneralStatus.Text = asMessage
            End If


            If aiProgressPercent > 0 Then
                frmMain.ToolStripProgressBar.Visible = True
                frmMain.ToolStripProgressBar.Value = aiProgressPercent
            Else
                frmMain.ToolStripProgressBar.Visible = False
            End If

            If abRefreshForm Then
                frmMain.Refresh()
            End If
            frmMain.Invalidate()

        End Sub

        Public Sub WriteToProgressBar(ByVal aiProgressPercent As Integer)

            Try
                'frmMain.zfrmModelExplorer.CircularProgressBar.Value = aiProgressPercent
                'frmMain.zfrmModelExplorer.Invalidate()
                frmMain.ToolStripProgressBar.Visible = True
                frmMain.ToolStripProgressBar.Value = aiProgressPercent
                frmMain.Refresh()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function IsSerializable(ByVal obj As Object) As Boolean

            Dim mem As System.IO.MemoryStream = New System.IO.MemoryStream()
            Dim bin As BinaryFormatter = New BinaryFormatter()
            'Dim bin = New XmlSerializer(obj.GetType)
            Try

                bin.Serialize(mem, obj)
                Return True

            Catch Serex As System.Runtime.Serialization.SerializationException

                MsgBox("Your object cannot be serialized. The reason is: " & Serex.ToString() & Serex.GetType.ToString)
                Return False

            Catch ex As Exception

                MsgBox("Your object cannot be serialized. The reason is: " & ex.ToString() & ex.GetType.ToString)
                Return False
            End Try
        End Function

        Public Function ConvertNumberToLetters(ByVal aiNumber As Integer) As String

            Dim lsNumber As String = ""
            Dim lsString As String = ""
            Dim liNumber As Integer
            Dim lsCharacter As String = ""
            Dim liInd As Integer = 0
            Dim KLIdentityLetters As Array = System.[Enum].GetValues(GetType(pcenumKLDataLetter))

            Try
                lsNumber = aiNumber.ToString

                For liInd = 1 To lsNumber.Length
                    lsCharacter = Mid$(lsNumber, liInd, 1)
                    liNumber = Convert.ToInt32(lsCharacter)
                    lsString &= KLIdentityLetters(liNumber).ToString
                Next

                Return lsString

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: publicFunctions.ConvertNumberToLetters"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

        Private Sub ReadLicenceFile()
            ' load the license text file
            Dim myFile As System.IO.StreamReader = New System.IO.StreamReader(Boston.MyPath() & "\license.txt")
            Dim mystring As String = myFile.ReadToEnd()

            myFile.Close()

        End Sub

        Public Function MyPath() As String
            'get the app path
            Dim fullAppName As String = [Assembly].GetExecutingAssembly().GetName().CodeBase
            'This strips off the exe name
            Dim FullAppPath As String = Path.GetDirectoryName(fullAppName)

            FullAppPath = Mid(FullAppPath, Len("file:\\"))

            Return FullAppPath
        End Function

        Public Sub UpdateDatabaseVersion(ByVal asDatabaseVersionNr As String)

            Try
                Dim lrReferenceFieldValue As New tReferenceFieldValue()
                lrReferenceFieldValue.ReferenceTableId = 1
                lrReferenceFieldValue.ReferenceFieldId = 1
                lrReferenceFieldValue.Data = asDatabaseVersionNr
                lrReferenceFieldValue.RowId = "1"

                Call TableReferenceFieldValue.UpdateReferenceFieldValue(lrReferenceFieldValue)

                My.Settings.DatabaseVersionNumber = asDatabaseVersionNr
                My.Settings.Save()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        '20220620-VM-REdundant in Namespace Boston
        'Public Function CropImage(ByVal img As Image, ByVal backgroundColor As Color, Optional ByVal margin As Integer = 0) As Image

        '    Dim minX As Integer = img.Width
        '    Dim minY As Integer = img.Height
        '    Dim maxX As Integer = 0
        '    Dim maxY As Integer = 0

        '    Using bmp As New Bitmap(img)

        '        For y As Integer = 0 To bmp.Height - 1
        '            For x As Integer = 0 To bmp.Width - 1
        '                If bmp.GetPixel(x, y).ToArgb <> backgroundColor.ToArgb Then
        '                    If x < minX Then
        '                        minX = x
        '                    ElseIf x > maxX Then
        '                        maxX = x
        '                    End If
        '                    If y < minY Then
        '                        minY = y
        '                    ElseIf y > maxY Then
        '                        maxY = y
        '                    End If
        '                End If
        '            Next
        '        Next

        '        Dim rect As New Rectangle(minX - margin, minY - margin, maxX - minX + 2 * margin + 1, maxY - minY + 2 * margin + 1)
        '        Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)

        '        Return cropped

        '    End Using

        'End Function

        'Public Function CreateFramedImage(ByVal Source As Image, ByVal BorderColor As Color, ByVal BorderThickness As Integer) As Image

        '    Dim b As New Bitmap(Source.Width + BorderThickness * 2, Source.Height + BorderThickness * 2)
        '    Dim g As Graphics = Graphics.FromImage(b)
        '    g.Clear(BorderColor)
        '    g.DrawImage(Source, BorderThickness, BorderThickness)
        '    g.Dispose()
        '    Return b

        'End Function

        Public Sub CreateDirectoryIfItDoesntExist(ByVal asDirectoryPath As String)

            'Dim file As System.IO.FileInfo = New System.IO.FileInfo(asDirectoryPath)
            'file.Directory.Create()

            Directory.CreateDirectory(asDirectoryPath)

        End Sub

        Public Function UnicodeToAscii(ByVal unicodeString As String) As String


            Dim ascii As Encoding = Encoding.ASCII
            Dim unicode As Encoding = Encoding.Unicode
            ' Convert the string into a byte array. 
            Dim unicodeBytes As Byte() = unicode.GetBytes(unicodeString)

            ' Perform the conversion from one encoding to the other. 
            Dim asciiBytes As Byte() = Encoding.Convert(unicode, ascii, unicodeBytes)

            ' Convert the new byte array into a char array and then into a string. 
            Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length) - 1) As Char
            ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
            Dim asciiString As New String(asciiChars)
            Return asciiString
        End Function

        Public Function GetEnumFromDescriptionAttribute(Of T)(description As String) As T
            Dim type = GetType(T)
            If Not type.IsEnum Then
                Throw New InvalidOperationException()
            End If
            For Each fieldInfo In type.GetFields()
                Dim descriptionAttribute = TryCast(Attribute.GetCustomAttribute(fieldInfo, GetType(DescriptionAttribute)), DescriptionAttribute)
                If descriptionAttribute IsNot Nothing Then
                    If descriptionAttribute.Description <> description Then
                        Continue For
                    End If
                    Return DirectCast(fieldInfo.GetValue(Nothing), T)
                End If
                If fieldInfo.Name <> description Then
                    Continue For
                End If
                Return DirectCast(fieldInfo.GetValue(Nothing), T)
            Next
            Return Nothing
        End Function

        Public Function ReadEmbeddedRessourceToString(assembly As Assembly, searchPattern As String) As String

            Dim resourceName = assembly.GetManifestResourceNames().FirstOrDefault(Function(x) x.Contains(searchPattern))
            Using stream = assembly.GetManifestResourceStream(resourceName)
                If stream IsNot Nothing Then
                    Using reader = New StreamReader(stream, Encoding.Default)
                        Return reader.ReadToEnd()
                    End Using
                End If
            End Using
            Return String.Empty

        End Function

        Public Function Soundex(ByVal Word As String, ByVal Length As Integer) As String
            ' Value to return
            Dim Value As String = ""
            ' Size of the word to process
            Dim Size As Integer = Word.Length
            ' Make sure the word is at least two characters in length
            If (Size > 1) Then
                ' Convert the word to all uppercase
                Word = Word.ToUpper()
                ' Conver to the word to a character array for faster processing
                Dim Chars() As Char = Word.ToCharArray()
                ' Buffer to build up with character codes
                Dim Buffer As New System.Text.StringBuilder
                Buffer.Length = 0
                ' The current and previous character codes
                Dim PrevCode As Integer = 0
                Dim CurrCode As Integer = 0
                ' Append the first character to the buffer
                Buffer.Append(Chars(0))
                ' Prepare variables for loop
                Dim i As Integer
                Dim LoopLimit As Integer = Size - 1
                ' Loop through all the characters and convert them to the proper character code
                For i = 1 To LoopLimit
                    Select Case Chars(i)
                        Case "A", "E", "I", "O", "U", "H", "W", "Y"
                            CurrCode = 0
                        Case "B", "F", "P", "V"
                            CurrCode = 1
                        Case "C", "G", "J", "K", "Q", "S", "X", "Z"
                            CurrCode = 2
                        Case "D", "T"
                            CurrCode = 3
                        Case "L"
                            CurrCode = 4
                        Case "M", "N"
                            CurrCode = 5
                        Case "R"
                            CurrCode = 6
                    End Select
                    ' Check to see if the current code is the same as the last one
                    If (CurrCode <> PrevCode) Then
                        ' Check to see if the current code is 0 (a vowel); do not proceed
                        If (CurrCode <> 0) Then
                            Buffer.Append(CurrCode)
                        End If
                    End If
                    ' If the buffer size meets the length limit, then exit the loop
                    If (Buffer.Length = Length) Then
                        Exit For
                    End If
                Next
                ' Padd the buffer if required
                Size = Buffer.Length
                If (Size < Length) Then
                    Buffer.Append("0", (Length - Size))
                End If
                ' Set the return value
                Value = Buffer.ToString()
            End If
            ' Return the computed soundex
            Return Value
        End Function

        Public Function ToRadians(adblDegrees As Double) As Double

            Return adblDegrees * (Math.PI / 180)

        End Function

        Function ConvertToPascalCase(ByVal input As String) As String
            ' Split the input string into words
            Dim words As String() = input.Split({" "c, "_"c}, StringSplitOptions.RemoveEmptyEntries)

            ' Capitalize the first letter of each word
            For i As Integer = 0 To words.Length - 1
                words(i) = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words(i).ToLower())
            Next

            ' Join the words together
            Dim pascalCase As String = String.Concat(words)

            Return pascalCase
        End Function

        Public Function GetGPT3Result(ByRef arOpenAIAPI As OpenAI_API.OpenAIAPI, ByVal asNLQuery As String, Optional ByVal asActualNLQuery As String = "") As OpenAI_API.Completions.CompletionResult

            Try
                Dim liMaxTokens As Integer = Math.Max(2000, CInt(asActualNLQuery.Split(" ").Length * 3.5))

                Dim _timeout As Integer = 5000 ' 5 seconds
                Dim _cancellationTokenSource As New System.Threading.CancellationTokenSource

                Dim lrOpenAIAPI = arOpenAIAPI

                Return Task.Run(Function() lrOpenAIAPI.Completions.CreateCompletionAsync(
                                           New OpenAI_API.Completions.CompletionRequest(prompt:=asNLQuery,
                                                                                         model:=OpenAI_API.Models.Model.DavinciText,
                                                                                         temperature:=0,
                                                                                         max_tokens:=liMaxTokens)
                                ), _cancellationTokenSource.Token).Result

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

        End Function

        Public Function GetGPTChatResponse(ByVal arOpenAIAPI As OpenAI_API.OpenAIAPI, ByVal asPrompt As String, Optional ByVal asActualNLQuery As String = "") As OpenAI_API.Chat.ChatResult
            Try
                Dim _timeout As Integer = 30000 ' 5 seconds

                ' Task to execute the main operation
                Dim chatTask = Task.Run(Function() arOpenAIAPI.Chat.CreateChatCompletionAsync(
            New OpenAI_API.Chat.ChatRequest() With {
                .Model = Model.GPT4,
                .Temperature = 0,
                .MaxTokens = 1500,
                .TopP = 1,
                .Messages = New OpenAI_API.Chat.ChatMessage() {New OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, asPrompt)}
            }
        ))

                ' Wait for either the main task to complete or the timeout task to complete
                Dim completedTask = Task.WhenAny(chatTask, Task.Delay(_timeout)).Result

                ' Check if the main task completed or timed out
                If completedTask Is chatTask Then
                    ' Main task completed successfully
                    Return chatTask.Result
                Else
                    ' Main task timed out, handle this scenario (e.g., log an error or return a default result)
                    Return New OpenAI_API.Chat.ChatResult()
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage.AppendDoubleLineBreak(ex.InnerException.Message)
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try
        End Function

        'Public Function GetGPTChatResponse(ByVal arOpenAIAPI As OpenAI_API.OpenAIAPI, ByVal asPrompt As String, Optional ByVal asActualNLQuery As String = "") As OpenAI_API.Chat.ChatResult

        '    Try
        '        Dim _timeout As Integer = 5000 ' 5 seconds
        '        Dim _cancellationTokenSource As New System.Threading.CancellationTokenSource

        '        Try

        '            'Return Task.Run(Function() Me.mrOpenAIAPI.Chat.CreateChatCompletionAsync(ByVal request As ChatRequest) As Task(Of ChatResult)
        '            Return Task.Run(Function() arOpenAIAPI.Chat.CreateChatCompletionAsync(
        '                            New OpenAI_API.Chat.ChatRequest() With {.Model = Model.GPT4,
        '                                                                    .Temperature = 0,
        '                                                                    .MaxTokens = 1500,
        '                                                                    .TopP = 1,
        '                                                                    .Messages = New OpenAI_API.Chat.ChatMessage() {New OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, asPrompt)}})).Result

        '        Catch ex As Exception
        '            Return New OpenAI_API.Chat.ChatResult()
        '        End Try

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

        '        Return Nothing
        '    End Try

        'End Function

        Public Sub GetElevenLabsSpeech(ByVal asTextToSpeak As String,
                                       ByVal arVoice As BackCast.Voice,
                                       Optional ByRef arUtterance As BackCast.Utterance = Nothing,
                                       Optional ByVal abSuppressPlaying As Boolean = False)

            Try

                Dim CHUNK_SIZE As Integer = 1024
                Dim url As String = "https://api.elevenlabs.io/v1/text-to-speech/" & arVoice.VoiceId

                Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
                request.Method = "POST"
                request.ContentType = "application/json"
                request.Headers("xi-api-key") = "5bf9e5dd50a7c5a519ecb8249cd097b3"
                request.Accept = "audio/mpeg"

                Dim data As String = "{""text"": """ & asTextToSpeak & """, ""model_id"": ""eleven_monolingual_v1"", ""voice_settings"": {""stability"": " & arVoice.Stability & ", ""similarity_boost"": " & arVoice.SimilarityBoost & "}}"

                Dim byteArray As Byte() = Encoding.UTF8.GetBytes(data)
                request.ContentLength = byteArray.Length

                Using dataStream As Stream = request.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                End Using

                Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Dim responseStream As Stream = response.GetResponseStream()

                Using memoryStream As New MemoryStream()
                    Dim buffer As Byte() = New Byte(CHUNK_SIZE - 1) {}
                    Dim bytesRead As Integer = responseStream.Read(buffer, 0, CHUNK_SIZE)
                    While bytesRead > 0
                        memoryStream.Write(buffer, 0, bytesRead)
                        bytesRead = responseStream.Read(buffer, 0, CHUNK_SIZE)
                    End While

                    ' Convert the memory stream to byte array
                    Dim lrTTSResult As Byte() = memoryStream.ToArray()

                    ' Save the byte array to a file if needed
                    'File.WriteAllBytes("output.mp3", lrTTSResult)

                    If arUtterance IsNot Nothing Then
                        arUtterance.Bytes = lrTTSResult
                    End If

                    ' Play the audio from the byte array
                    If Not abSuppressPlaying Then
                        Using inputStream As New MemoryStream(lrTTSResult)
                            Using waveStream As WaveStream = New Mp3FileReader(inputStream)
                                Using outputDevice As IWavePlayer = New WaveOutEvent()
                                    outputDevice.Init(waveStream)
                                    outputDevice.Play()
                                    While outputDevice.PlaybackState = PlaybackState.Playing
                                        System.Threading.Thread.Sleep(100)
                                    End While
                                End Using
                            End Using
                        End Using
                    End If
                End Using

                responseStream.Close()
                response.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Function GetElevenLabsVoices(xiApiKey As String) As List(Of BackCast.Voice)

            Dim Voices As New List(Of BackCast.Voice)()

            Dim url As String = "https://api.elevenlabs.io/v1/voices"
            Dim headers As New WebHeaderCollection()
            headers.Add("Accept", "application/json")
            headers.Add("xi-api-key", xiApiKey)

            Using client As New WebClient()
                client.Headers = headers
                Dim responseJson As String = client.DownloadString(url)
                Dim wrapper As BackCast.RootObjectWrapper = JsonConvert.DeserializeObject(Of BackCast.RootObjectWrapper)(responseJson)
                Voices = wrapper?.Voices
            End Using


            Return Voices
        End Function

    End Module

End Namespace