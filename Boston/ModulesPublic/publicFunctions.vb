Imports System.IO
Imports System.Runtime.InteropServices
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
Imports Azure
Imports Azure.AI.OpenAI
Imports OpenAI_API
Imports OpenAI_API.Completions
Imports OpenAI_API.Models
Imports System.Net
Imports NAudio.Wave
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Boston
    Public Module publicFunctions

        Private ReadOnly random As Random = New Random()

        ''' <summary>
        ''' This Function Will Extract The Exception Line Number and Method In Which
        ''' Exception Occurred And The Exception Message
        ''' </summary>
        ''' <param name="arException">Provide Exception Object</param>
        ''' <returns>Tab Separated String Of Extracted Exception</returns>
        Public Function ExtractExceptionLine(ByRef arException As Exception) As String

            Try
                Dim lrStackTrace As New StackTrace(arException, True)
                Dim lrMethodBase As MethodBase = lrStackTrace.GetFrame(lrStackTrace.FrameCount - 1).GetMethod()
                Try
                    Dim liLineNumber As Integer = lrStackTrace.GetFrame(lrStackTrace.FrameCount - 1).GetFileLineNumber()
                    If liLineNumber = 0 Then

                        Dim keyword As String = ":line"
                        Dim index As Integer = arException.StackTrace.IndexOf(keyword)

                        If index <> -1 Then
                            ' Add the length of the keyword to the index to start after it.
                            Dim result As String = arException.StackTrace.Substring(index + keyword.Length)
                            result = Environment.StackTrace
                            ' Trim any leading spaces.
                            result = result.Trim()

                            Console.WriteLine(result)
                        Else
                            Return 0.ToString
                        End If

                    End If
                    Return liLineNumber.ToString
                Catch
                    Return "Unkown"
                Finally
                    lrStackTrace = Nothing
                    lrMethodBase = Nothing
                End Try

            Catch ex As Exception
                Return "Errror Retrieving Line Number"
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

        <DllImport("user32.dll")>
        Public Function GetKeyState(ByVal nVirtKey As Integer) As Short
        End Function
        Public Function IsCtrlPressed() As Boolean
            ' The key code for the Control key
            Const VK_CONTROL As Integer = &H11
            ' Check the high-order bit
            Return (GetKeyState(VK_CONTROL) And &H8000) = &H8000
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return originalFont
            End Try
        End Function

        Public Function GetConfigFileLocation() As String

            Return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString

        End Function

        Public Function WaitForFile(ByVal fullPath As String, ByVal mode As FileMode, ByVal access As FileAccess, ByVal share As FileShare) As Boolean
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
                Return False
            Finally
                System.Threading.Thread.Sleep(50)
            End Try

            Return True

        End Function

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
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        ''' <summary>
        ''' Displays the Generic Selection Form
        ''' </summary>
        ''' <param name="arGenericSelection"></param>
        ''' <param name="as_form_title"></param>
        ''' <param name="aiSelectColumn"></param>
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
                                                 ByVal asTableName As String,
                                                 ByVal as_select_field As String,
                                                 ByVal as_index_field As String,
                                                 Optional ByVal as_where_clause As String = "",
                                                 Optional ByVal ao_combobox_item As tComboboxItem = Nothing,
                                                 Optional ByVal aiComboboxStyle As pcenumComboBoxStyle = pcenumComboBoxStyle.DropdownList,
                                                 Optional ByVal asOrderByFields As String = Nothing,
                                                 Optional ByVal aiSelectColumn As Integer = 1,
                                                 Optional ByVal asColumnWidthString As String = "100",
                                                 Optional ByVal asFieldList As String = "",
                                                 Optional ByVal abUseDataStore As Boolean = False,
                                                 Optional ByVal atDataStoreType As Type = Nothing,
                                                 Optional ByVal asObjectName As String = Nothing
                                                 ) As DialogResult

            arGenericSelection.SelectField = as_select_field
            arGenericSelection.IndexField = as_index_field
            arGenericSelection.TableName = asTableName
            arGenericSelection.FormTitle = as_form_title
            arGenericSelection.SelectColumn = aiSelectColumn
            arGenericSelection.ColumnWidthString = asColumnWidthString
            arGenericSelection.FieldList = asFieldList
            arGenericSelection.UseDataStore = abUseDataStore
            arGenericSelection.DataStoreType = atDataStoreType
            arGenericSelection.ObjectName = asObjectName

            Try

                If asOrderByFields IsNot Nothing Then
                    arGenericSelection.OrderByFields = asOrderByFields
                Else
                    arGenericSelection.OrderByFields = as_select_field
                End If
                If Not IsNothing(as_where_clause) Then
                    arGenericSelection.WhereClause = as_where_clause
                Else
                    arGenericSelection.WhereClause = ""
                End If

                If ao_combobox_item IsNot Nothing Then
                    arGenericSelection.TupleList.Add(ao_combobox_item)
                End If

                Select Case aiSelectColumn
                    Case Is = 1
                        Dim lfrm_generic_select_frm As New frmGenericSelect

                        Select Case aiComboboxStyle
                            Case Is = pcenumComboBoxStyle.Dropdown
                                lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDown
                            Case Is = pcenumComboBoxStyle.DropdownList
                                lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDownList
                            Case Is = pcenumComboBoxStyle.Simple
                                lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.Simple
                            Case Else
                                lfrm_generic_select_frm.combobox_selection.DropDownStyle = ComboBoxStyle.DropDown
                        End Select

                        lfrm_generic_select_frm.zoGenericSelection = arGenericSelection
                        DisplayGenericSelectForm = lfrm_generic_select_frm.ShowDialog()
                    Case Is > 1
                        Dim lfrm_generic_select_frm As New frmGenericSelectMultiColumn
                        Select Case aiComboboxStyle
                            Case Is = pcenumComboBoxStyle.Dropdown
                                lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDown
                            Case Is = pcenumComboBoxStyle.DropdownList
                                lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDownList
                            Case Is = pcenumComboBoxStyle.Simple
                                lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.Simple
                            Case Else
                                lfrm_generic_select_frm.comboboxSelection.DropDownStyle = ComboBoxStyle.DropDown
                        End Select

                        lfrm_generic_select_frm.zoGenericSelection = arGenericSelection
                        DisplayGenericSelectForm = lfrm_generic_select_frm.ShowDialog()
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

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
                                 Optional ByVal aiInterval As Integer = 2500,
                                 Optional ByVal aiFontSize As Single = 10)

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub MakeRoundedCorners(ByRef aoForm As Form)

            Dim liArcRadius As Integer = 35

            aoForm.FormBorderStyle = FormBorderStyle.None
            Dim p As New Drawing2D.GraphicsPath()
            p.StartFigure()
            p.AddArc(New Rectangle(0, 0, liArcRadius, liArcRadius), 180, 90)
            p.AddLine(liArcRadius, 0, aoForm.Width - liArcRadius, 0)
            p.AddArc(New Rectangle(aoForm.Width - liArcRadius, 0, liArcRadius, liArcRadius), -90, 90)
            p.AddLine(aoForm.Width, liArcRadius, aoForm.Width, aoForm.Height - liArcRadius)
            p.AddArc(New Rectangle(aoForm.Width - liArcRadius, aoForm.Height - liArcRadius, liArcRadius, liArcRadius), 0, 90)
            p.AddLine(aoForm.Width - liArcRadius, aoForm.Height, liArcRadius, aoForm.Height)
            p.AddArc(New Rectangle(0, aoForm.Height - liArcRadius, liArcRadius, liArcRadius), 90, 90)
            p.CloseFigure()
            aoForm.Region = New Region(p)

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

            ' CodeSafe
            If prApplication Is Nothing OrElse prApplication.MainForm Is Nothing Then Exit Sub

            ' Check if this method was called from a different thread than the UI thread
            If prApplication.MainForm.InvokeRequired Then
                ' Call this same method but on the UI thread
                prApplication.MainForm.Invoke(New Action(Sub()
                                                             WriteToStatusBar(asMessage, abRefreshForm, aiProgressPercent, abAppendMessageOnly)
                                                         End Sub))
                Return
            End If

            ' The following code is guaranteed to run on the UI thread
            If abAppendMessageOnly Then
                prApplication.MainForm.StatusLabelGeneralStatus.Text &= asMessage
            Else
                prApplication.MainForm.StatusLabelGeneralStatus.Text = asMessage
            End If

            If aiProgressPercent > 0 Then
                prApplication.MainForm.ToolStripProgressBar.Visible = True
                prApplication.MainForm.ToolStripProgressBar.Value = aiProgressPercent
            Else
                prApplication.MainForm.ToolStripProgressBar.Visible = False
            End If

            If abRefreshForm Then
                prApplication.MainForm.StatusLabelGeneralStatus.Invalidate()
                prApplication.MainForm.ToolStripProgressBar.Invalidate()
                prApplication.MainForm.Update()  ' Force immediate update
            End If

        End Sub

        Public Sub WriteToProgressBar(ByVal aiProgressPercent As Integer)

            Try
                'frmMain.zfrmModelExplorer.CircularProgressBar.Value = aiProgressPercent
                'frmMain.zfrmModelExplorer.Invalidate()
                frmMain.ToolStripProgressBar.Visible = True
                Try
                    frmMain.ToolStripProgressBar.Value = aiProgressPercent
                Catch ex As Exception
                    'You never know.
                End Try


                ' Assuming your main form has a unique name, replace "YourMainFormName" with the actual name.
                Dim mainForm As frmMain = Application.OpenForms.OfType(Of frmMain)().FirstOrDefault()

                If mainForm IsNot Nothing Then
                    ' Now you have a reference to the main form, and you can call Invalidate on its controls.
                    mainForm.ToolStripProgressBar.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function IsSerializable(ByVal obj As Object) As Boolean
            Try
                Return True
                '20251122-VM-Use the following in the future.
                'Public Function IsSerializableWithContract(Of TRoot)(obj As TRoot, knownTypes As IEnumerable(Of Type)) As Boolean
                'Try
                'Dim serializer As New DataContractSerializer(GetType(TRoot), knownTypes)


                Dim serializer As New DataContractSerializer(obj.GetType())
                Using mem As New MemoryStream()
                    serializer.WriteObject(mem, obj)
                End Using
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

#Region "ENUMS"

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

        Public Function GetEnumDescriptions(Of TEnum As Structure)() As List(Of String)
            Dim descriptions As New List(Of String)
            For Each value In [Enum].GetValues(GetType(TEnum))
                Dim fieldInfo = GetType(TEnum).GetField(value.ToString())
                Dim attributes = CType(fieldInfo.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
                If attributes.Length > 0 Then
                    descriptions.Add(attributes(0).Description)
                Else
                    descriptions.Add(value.ToString())
                End If
            Next
            Return descriptions
        End Function

#End Region

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

        Function GetGroqResponse(jsonData As String) As JObject
            Dim url As String = "https://api.groq.com/openai/v1/chat/completions"
            Dim apiKey As String = My.Settings.GroqAPIKey

            Dim request As WebRequest = WebRequest.Create(url)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Headers.Add("Authorization", "Bearer " & apiKey)

            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(jsonData)
            request.ContentLength = bytes.Length

            Dim jsonResult As JObject = Nothing

            Try
                Using stream As Stream = request.GetRequestStream()
                    stream.Write(bytes, 0, bytes.Length)
                End Using

                Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                    Using streamReader As New StreamReader(response.GetResponseStream())
                        Dim result As String = streamReader.ReadToEnd()
                        jsonResult = JObject.Parse(result)
                        Return jsonResult
                    End Using
                End Using
            Catch exWeb As WebException
                If exWeb IsNot Nothing Then
                    Using errorResponse As HttpWebResponse = CType(exWeb.Response, HttpWebResponse)
                        Using reader As New StreamReader(errorResponse.GetResponseStream())
                            Dim errorText As String = reader.ReadToEnd()
                            prApplication.ThrowMessage(errorText, pcenumErrorType.Critical)
                        End Using
                    End Using
                End If
            Catch ex As Exception
                ' Handle or log the exception as needed
                Console.WriteLine("Error: " & ex.Message)
                Return Nothing
            End Try
        End Function

        'Function GetGroqResponse(userMessage As String) As Object
        '    Dim url As String = "https://api.groq.com/openai/v1" '"https://api.groq.com/openai/v1/chat/completions"
        '    Dim apiKey As String = My.Settings.GroqAPIKey
        '    Dim data As String = "{""messages"": [{""role"": ""user"", ""content"": """ & "Hello World" & """}], ""model"": ""gpt-4-32k-0613""}" '""mixtral-8x7b-32768""}"

        '    Dim request As WebRequest = WebRequest.Create(url)
        '    request.Method = "POST"
        '    request.Headers.Add("Authorization", "Bearer " & apiKey)
        '    request.ContentType = "application/json"

        '    Using streamWriter As New StreamWriter(request.GetRequestStream())
        '        streamWriter.Write(data)
        '        streamWriter.Flush()
        '        streamWriter.Close()
        '    End Using

        '    Dim response As WebResponse = request.GetResponse()
        '    Using streamReader As New StreamReader(response.GetResponseStream())
        '        Dim result As String = streamReader.ReadToEnd()
        '        Dim jsonResult As JObject = JObject.Parse(result)
        '        Return jsonResult
        '    End Using

        'End Function

        Public Function GetGPT3Result(ByRef arOpenAIAPI As OpenAI_API.OpenAIAPI, ByVal asNLQuery As String, Optional ByVal asActualNLQuery As String = "") As OpenAI_API.Completions.CompletionResult

            Try
                Dim liMaxTokens As Integer = Math.Max(2000, CInt(asActualNLQuery.Split(" ").Length * 3.5))

                Dim _timeout As Integer = 5000 ' 5 seconds
                Dim _cancellationTokenSource As New System.Threading.CancellationTokenSource

                Dim lrOpenAIAPI = arOpenAIAPI

                Return Task.Run(Function() lrOpenAIAPI.Completions.CreateCompletionAsync(
                                           New OpenAI_API.Completions.CompletionRequest(prompt:=asNLQuery,
                                                                                         model:=OpenAI_API.Models.Model.GPT4,
                                                                                         temperature:=0,
                                                                                         max_tokens:=liMaxTokens)
                                ), _cancellationTokenSource.Token).Result

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak(ex.InnerException.Message)
                lsMessage.AppendDoubleLineBreak(ex.Message)
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, False,,,,, ex)

                Return Nothing
            End Try

        End Function

        Public Function GetGPTChatResponse(ByVal asPrompt As String, Optional ByVal asActualNLQuery As String = "") As NullableResponse(Of ChatCompletions)

            Try
                Dim apiKey As String = My.Settings.FactEngineOpenAIAPIKey

                ' Create the client
                Dim lrOpenAIClient = New OpenAIClient(apiKey)  'Azure: New OpenAIClient(New Uri(apiUrl), New AzureKeyCredential(apiKey))

                Dim lrChatCompletionsOptions As New ChatCompletionsOptions() With {
                            .User = "Bot",
                            .MaxTokens = 4000,
                            .Functions = {},
                            .Temperature = Single.Parse("0.2")
                    }

                Dim lrChatMessage = New ChatMessage(ChatRole.User, asPrompt)

                lrChatCompletionsOptions.Messages.Clear()
                lrChatCompletionsOptions.Messages.Add(lrChatMessage)
                lrChatCompletionsOptions.ChoiceCount = 1 '20240113-VM-Was 10, then 5....now reduced to 1. See KickStartTimer.
                lrChatCompletionsOptions.FrequencyPenalty = 0
                lrChatCompletionsOptions.Temperature = 1
                lrChatCompletionsOptions.MaxTokens = 2000

                Dim response As NullableResponse(Of ChatCompletions) = lrOpenAIClient.GetChatCompletions(My.Settings.FactEngineModelName, lrChatCompletionsOptions)

                Return response

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak(ex.InnerException.Message)
                lsMessage.AppendDoubleLineBreak(ex.Message)
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, False,,,,, ex)

                Return Nothing
            End Try

        End Function


        ''' <summary>
        ''' 20250825-VM-Write a version of this function that uses the Azure OpenAI client.
        '''   This way we don't have to use the Model enum, but rather can use the text name of the model. E.g. "ChatGPTTurbo" rather than Model.ChatGPTTurbo.
        '''   See above
        ''' </summary>
        ''' <param name="arOpenAIAPI"></param>
        ''' <param name="asPrompt"></param>
        ''' <param name="asActualNLQuery"></param>
        ''' <returns></returns>
        Public Function GetGPTChatResponse(arOpenAIAPI As OpenAI_API.OpenAIAPI, ByVal asPrompt As String, Optional ByVal asActualNLQuery As String = "") As OpenAI_API.Chat.ChatResult
            Try
                Dim _timeout As Integer = 5000 ' 5 seconds
                Dim _cancellationTokenSource As New System.Threading.CancellationTokenSource

                Dim lrOpenAIAPI = arOpenAIAPI

                Return Task.Run(Function() lrOpenAIAPI.Chat.CreateChatCompletionAsync(
                                                      New OpenAI_API.Chat.ChatRequest() With {
                                                                                     .Model = Model.ChatGPTTurbo,
                                                                                     .Temperature = 0,
                                                                                     .MaxTokens = 2000,
                                                                                     .Messages = New OpenAI_API.Chat.ChatMessage() {New OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, asPrompt)}
                                                                                     }
                                                ), _cancellationTokenSource.Token
                                        ).Result

                ' Task to execute the main operation
                'Dim chatTask = Task.Run(Function() lrOpenAIAPI.Chat.CreateChatCompletionAsync(
                '                              New OpenAI_API.Chat.ChatRequest() With {
                '                                                                     .Model = Model.ChatGPTTurbo,
                '                                                                     .Temperature = 0,
                '                                                                     .MaxTokens = 500,
                '                                                                     .Messages = New OpenAI_API.Chat.ChatMessage() {New OpenAI_API.Chat.ChatMessage(OpenAI_API.Chat.ChatMessageRole.User, asPrompt)}
                '                                                                     }
                '                                )
                '                        )

                '' Wait for either the main task to complete or the timeout task to complete
                'Dim completedTask = Task.WhenAny(chatTask, Task.Delay(_timeout)).Result

                '' Check if the main task completed or timed out
                'If completedTask Is chatTask Then
                '    ' Main task completed successfully
                '    Return chatTask.Result
                'Else
                '    ' Main task timed out, handle this scenario (e.g., log an error or return a default result)
                '    Return New OpenAI_API.Chat.ChatResult()
                'End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage.AppendDoubleLineBreak(ex.InnerException.Message)
                lsMessage.AppendDoubleLineBreak(ex.Message)
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical,,,,,,, ex)

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
        '        prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

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
                                    Call prApplication.Brain.TriggerSpeaking()
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
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, abUseFlashCard:=True)
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