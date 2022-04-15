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

Namespace Richmond

    Public Module publicFunctions

        Private ReadOnly random As Random = New Random()

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

        Public Function GetConfigFileLocation() As String

            Return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString

        End Function

        Public Function returnIfTrue(ByVal abBoolValue As Boolean,
                                     ByVal asReturnString As String,
                                     ByVal asAlternateString As String) As String

            If abBoolValue Then
                Return asReturnString
            Else
                Return asAlternateString
            End If

        End Function

        Public Function OpenDatabase() As Boolean

            Dim lsMessage As String = ""
            Dim lsDBLocation As String
            Dim lsConnectionString As String
            Dim lsDatabaseLocation As String = ""
            Dim lsLocalDatabaseLocation As String = ""
            Dim lsDataProvider As String = ""

            Try
                '------------------------------------------------
                'Define the location of your database
                'as follows if you want to use a relative folder.
                '------------------------------------------------
                lsDBLocation = Trim(My.Settings.DatabaseConnectionString)

                '-------------------------------------------
                'Sample Database ConnectionStrings
                '-------------------------------------------
                'SQL Server Connection String Sample
                'DRIVER=SQL Server;UID=s2\vmorgante;PWD=cisco1;Trusted_Connection=;DATABASE=PreviewDialler;WSID=SIS2WKVM;APP=Microsoft Office 2003;SERVER=SIS2DB02;Description=PreviewDialler
                'lrSQLConnectionStringBuilder used to interogate the connection string.
                'Dim lrSQLConnectionStringBuilder As New SqlConnectionStringBuilder
                'lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString
                '-----------------------------------
                'MDB Connection String Sample
                'Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Program Files\PreviewDialler\database\PREVIEWDIALLER.mdb
                '-------------------------------------------

                '------------------------------------------------
                'Construct the connection string
                '------------------------------------------------
                lsConnectionString = lsDBLocation

                If My.Settings.DatabaseType = pcenumDatabaseType.MSJet.ToString Then

                    Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                    lrSQLConnectionStringBuilder.ConnectionString = lsConnectionString

                    lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")
                    lsDataProvider = lrSQLConnectionStringBuilder("Provider")

                    If Not System.IO.File.Exists(lsDatabaseLocation) Then

                        '-----------------------------------
                        'Try and find the database locally
                        '-----------------------------------
                        lsLocalDatabaseLocation = Richmond.MyPath & "\database\boston.vdb"

                        lsMessage = "Cannot find the ORM Stuio database at the default/configured location:"
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= lsDatabaseLocation
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "If this is a new installation of Boston, Boston will try and locate the database at:"
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= lsLocalDatabaseLocation
                        lsMessage &= vbCrLf & vbCrLf
                        lsMessage &= "If this is a not a new installation of Boston, contact FactEngine support."

                        If My.Settings.SilentPreConfiguration Then
                            '-------------------------------------
                            'Don't display anything to the user.
                            '-------------------------------------
                        Else
                            MsgBox(lsMessage)
                        End If

                        If System.IO.File.Exists(lsLocalDatabaseLocation) Then
                            lrSQLConnectionStringBuilder("Data Source") = lsLocalDatabaseLocation
                            My.Settings.DatabaseConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                            lsMessage = "Saving the following as the Database Connection String for your installation of Boston."
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage &= lrSQLConnectionStringBuilder.ConnectionString
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage.AppendLine("Boston will start but you won't be able to use this database. Contact FactEngine to find out how to connect to the correct database.")
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage &= "To make changes to the Boston database connection string, go to [Boston]->[Configuration]"

                            If Not My.Settings.SilentPreConfiguration Then
                                MsgBox(lsMessage)
                            End If

                            My.Settings.Save()
                            lsConnectionString = lrSQLConnectionStringBuilder.ConnectionString
                        Else
                            lsMessage = "Cannot find the Boston database at:"
                            lsMessage &= vbCrLf & vbCrLf
                            lsMessage &= lsDatabaseLocation
                            lsMessage &= vbCrLf
                            lsMessage &= "or..."
                            lsMessage &= vbCrLf
                            lsMessage &= lsLocalDatabaseLocation
                            lsMessage &= vbCrLf & vbCrLf
                            'lsMessage &= "Adjust the setting, 'database_connection_str', in the 'Richmond.exe.config' file and restart Richmond."
                            lsMessage &= "Adjust the Database Connection String for your database and restart Boston."
                            MsgBox(lsMessage)
                            frmCRUDBostonConfiguration.ShowDialog()
                            Return False
                        End If

                    End If
                End If

                '------------------------------------------------
                'Open the (database) connection
                '------------------------------------------------
                pdbConnection.Open(lsConnectionString)
                pdb_OLEDB_connection.ConnectionString = lsConnectionString
                pdb_OLEDB_connection.Open()
                Return True

            Catch lo_ex As Exception
                lsMessage = "Error: There was an error opening Richmond database: "
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "'" & Trim(lo_ex.Message) & "'" & vbCrLf & lo_ex.StackTrace
                MsgBox(lsMessage)
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

        Public Sub WriteToStatusBar(ByVal asMessage As String, Optional ByVal abRefreshForm As Boolean = False, Optional ByVal aiProgressPercent As Integer = 0)

            frmMain.StatusLabelGeneralStatus.Text = asMessage

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
            Dim myFile As System.IO.StreamReader = New System.IO.StreamReader(Richmond.MyPath() & "\license.txt")
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

        Public Function CropImage(ByVal img As Image, ByVal backgroundColor As Color, Optional ByVal margin As Integer = 0) As Image

            Dim minX As Integer = img.Width
            Dim minY As Integer = img.Height
            Dim maxX As Integer = 0
            Dim maxY As Integer = 0

            Using bmp As New Bitmap(img)

                For y As Integer = 0 To bmp.Height - 1
                    For x As Integer = 0 To bmp.Width - 1
                        If bmp.GetPixel(x, y).ToArgb <> backgroundColor.ToArgb Then
                            If x < minX Then
                                minX = x
                            ElseIf x > maxX Then
                                maxX = x
                            End If
                            If y < minY Then
                                minY = y
                            ElseIf y > maxY Then
                                maxY = y
                            End If
                        End If
                    Next
                Next

                Dim rect As New Rectangle(minX - margin, minY - margin, maxX - minX + 2 * margin + 1, maxY - minY + 2 * margin + 1)
                Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)

                Return cropped

            End Using

        End Function

        Public Function CreateFramedImage(ByVal Source As Image, ByVal BorderColor As Color, ByVal BorderThickness As Integer) As Image

            Dim b As New Bitmap(Source.Width + BorderThickness * 2, Source.Height + BorderThickness * 2)
            Dim g As Graphics = Graphics.FromImage(b)
            g.Clear(BorderColor)
            g.DrawImage(Source, BorderThickness, BorderThickness)
            g.Dispose()
            Return b

        End Function

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

    End Module

End Namespace