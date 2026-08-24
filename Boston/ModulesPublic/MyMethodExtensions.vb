Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Dynamic
Imports System.Reflection

Module MyMethodExtensions

    <Extension()>
    Public Function Between(value As Integer, min As Integer, max As Integer) As Boolean
        Return value >= min AndAlso value <= max
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

#Region "Richtextbox"
    ' Constants for Win32 API calls
    Private Const EM_GETFIRSTVISIBLELINE As Integer = &HCE
    Private Const EM_LINEINDEX As Integer = &HBB
    Private Const EM_LINESCROLL As Integer = &HB6
    Private Const EM_GETRECT As Integer = &HB2

    ' Win32 API declarations
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (
        ByVal hWnd As IntPtr, ByVal wMsg As Integer,
        ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

    Private Declare Function GetClientRect Lib "user32.dll" (
        ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean


    ' Extension method to get the first visible character index
    <Runtime.CompilerServices.Extension()>
    Public Function GetFirstVisibleCharIndex(ByVal richTextBox As RichTextBox) As Integer
        Dim rect As RECT
        GetClientRect(richTextBox.Handle, rect)

        Dim firstVisibleLine As Integer = SendMessage(richTextBox.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero)
        Dim firstVisibleCharIndex As Integer = SendMessage(richTextBox.Handle, EM_LINEINDEX, CType(firstVisibleLine, IntPtr), IntPtr.Zero)
        Dim charWidth As Double = rect.Right / richTextBox.TextLength
        Dim charOffset As Double = (rect.Left / charWidth)
        Return CInt(firstVisibleCharIndex + charOffset)
    End Function

    ' Extension method to get the last visible character index
    <Runtime.CompilerServices.Extension()>
    Public Function GetLastVisibleCharIndex(ByVal richTextBox As RichTextBox) As Integer
        Dim rect As RECT
        GetClientRect(richTextBox.Handle, rect)

        ' Calculate the number of visible lines based on the control's height and font height
        Dim visibleLines As Integer = CInt(Math.Ceiling((richTextBox.Height - 2) / (richTextBox.Font.Height + 2)))
        Dim visibleCharactersPerLine As Integer = CInt(Math.Ceiling((richTextBox.Width - 2) / (richTextBox.Font.Height / 2 + 2)))

        ' Calculate the last visible line index
        Return richTextBox.GetFirstVisibleCharIndex + (visibleLines * visibleCharactersPerLine)

    End Function

    ''' <summary>
    ''' RichTextBox: Move cursor and scroll to index.
    ''' </summary>
    ''' <param name="richTextBox"></param>
    ''' <param name="index"></param>
    <Extension()>
    Public Sub MoveCursorAndScrollToIndex(ByVal richTextBox As RichTextBox, ByVal index As Integer)
        If index >= 0 AndAlso index <= richTextBox.TextLength Then
            richTextBox.SelectionStart = index
            richTextBox.SelectionLength = 0
            richTextBox.ScrollToCaret()
        Else
            ' Handle the case where the index is out of bounds.
        End If
    End Sub

#End Region

#Region "Earhart"
    ''' <summary>
    ''' This extension will be used to get all the descendants of a tree node
    ''' </summary>
    <Extension()>
    Friend Iterator Function AllDescendants(ByVal c As TreeNodeCollection) As IEnumerable(Of TreeNode)
        For Each node In c.OfType(Of TreeNode)()
            Yield node

            For Each child In node.Nodes.AllDescendants()
                Yield child
            Next
        Next
    End Function

    '''' <summary>
    '''' This extension will be used to get all nested of a <see cref="Earhart.TreeFolder"/>, Including self
    '''' </summary>
    '<Extension()>
    'Friend Iterator Function All(ByVal arTreeModel As Earhart.TreeFolder) As IEnumerable(Of Earhart.TreeFolder)

    '    Yield arTreeModel

    '    For Each lrTreeModel In arTreeModel.NestedFolders
    '        For Each child In lrTreeModel.All()
    '            Yield child
    '        Next
    '    Next

    'End Function
    '''' <summary>
    '''' This extension will be used to get all the nested of a <see cref="Earhart.TreeFolder"/>
    '''' </summary>
    '<Extension()>
    'Friend Iterator Function AllNested(ByVal arTreeModel As Earhart.TreeFolder) As IEnumerable(Of Earhart.TreeFolder)

    '    For Each lrTreeModel In arTreeModel.NestedFolders
    '        Yield lrTreeModel

    '        For Each child In lrTreeModel.AllNested()
    '            Yield child
    '        Next
    '    Next

    'End Function
#End Region

    <Extension()>
    Public Function GetReversed(Of T)(ByVal list As List(Of T)) As List(Of T)
        Return list.AsEnumerable().Reverse().ToList()
    End Function

    ''' <summary>
    ''' Append to an array.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="original"></param>
    ''' <param name="rangeToAppend"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function AppendRange(ByVal original As Array, ByVal rangeToAppend As Array) As Array

        If original.GetType() IsNot rangeToAppend.GetType() Then
            Throw New ArgumentException("The arrays must be of the same type.")
        End If

        Dim resultArray = Array.CreateInstance(original.GetType().GetElementType(), original.Length + rangeToAppend.Length)

        Array.Copy(original, resultArray, original.Length)
        Array.Copy(rangeToAppend, 0, resultArray, original.Length, rangeToAppend.Length)

        Return resultArray
    End Function


    ''' <summary>
    ''' RichTextBox: FindAndReplaceAll
    ''' </summary>
    ''' <param name="richTextBox"></param>
    ''' <param name="findText"></param>
    ''' <param name="replaceText"></param>
    <Extension()>
    Public Sub FindAndReplaceAll(ByVal richTextBox As RichTextBox, ByVal findText As String, ByVal replaceText As String)
        Dim index As Integer = richTextBox.Text.IndexOf(findText)
        While index <> -1
            richTextBox.SelectionStart = index
            richTextBox.SelectionLength = findText.Length
            richTextBox.SelectedText = replaceText
            index = richTextBox.Text.IndexOf(findText, index + replaceText.Length)
        End While
    End Sub


    ''' <summary>
    ''' Richtextbox: FindReplaceNext
    ''' </summary>
    ''' <param name="richTextBox"></param>
    ''' <param name="findText"></param>
    ''' <param name="replaceText"></param>
    ''' <param name="startIndex"></param>
    <Extension()>
    Public Sub FindReplaceNext(ByVal richTextBox As RichTextBox,
                               ByVal findText As String,
                               ByVal replaceText As String,
                               ByVal startIndex As Integer,
                               ByRef aiFoundIndex As Integer)

        If startIndex < 0 OrElse startIndex > richTextBox.TextLength Then
            ' Handle the case where the startIndex is out of bounds
            Return
        End If

        Dim index As Integer = richTextBox.Text.IndexOf(findText, startIndex)
        If index <> -1 Then
            richTextBox.SelectionStart = index
            richTextBox.SelectionLength = findText.Length
            richTextBox.SelectedText = replaceText
            richTextBox.SelectionStart = index ' Optional: set the cursor after the replaced text
            richTextBox.ScrollToCaret() ' Optional: scroll to the replaced text
            aiFoundIndex = index
        Else
            ' Handle the case where the text is not found
        End If
    End Sub

    ''' <summary>
    ''' <see cref="RichTextBox"/> doesn't support the <see cref="vbCrLf"/> characters. <br />
    ''' This will replace all the line carriage return characters with line feed characters.
    ''' </summary>
    ''' <param name="asData">The string data you want check for characters</param>
    ''' <returns></returns>
    <Extension()>
    Friend Function FixLineCharacters(ByRef asData As String) As String
        Try

            ' replace the characters in the data
            asData = asData.
                Replace(vbCr, vbLf).
                Replace(vbCrLf, vbLf)

            ' return the data
            Return asData

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            ' return the original data
            Return asData
        End Try

    End Function

    ''' <summary>
    ''' RichTextBox: Highlight Text
    ''' </summary>
    ''' <param name="richTextBox"></param>
    ''' <param name="startIndex"></param>
    ''' <param name="wordLength"></param>
    <Extension()>
    Public Sub HighlightText(ByVal richTextBox As RichTextBox, ByVal startIndex As Integer, ByVal wordLength As Integer)
        If startIndex < 0 OrElse startIndex + wordLength > richTextBox.TextLength Then
            ' Handle the case where the startIndex or wordLength is out of bounds
            Return
        End If

        richTextBox.SelectionStart = startIndex
        richTextBox.SelectionLength = wordLength

        ' Set the selection background color to blue and the text color to white
        richTextBox.SelectionBackColor = Color.Blue
        richTextBox.SelectionColor = Color.White
    End Sub

    ''' <summary>
    ''' RichTextBox: Reset highlighting
    ''' </summary>
    ''' <param name="richTextBox"></param>
    <Extension()>
    Public Sub ResetHighlighting(ByVal richTextBox As RichTextBox)
        richTextBox.SelectAll()
        richTextBox.SelectionBackColor = Color.White ' Reset background color
        richTextBox.SelectionColor = Color.Black ' Reset text color
        richTextBox.SelectionLength = 0 ' Deselect everything
    End Sub

    <Extension()>
    Public Function CondenseString(ByVal input As String, ByVal startLength As Integer, ByVal endLength As Integer, ByVal ellipsisCount As Integer) As String
        If input.Length <= startLength + endLength + ellipsisCount Then
            Return input
        End If

        Dim condensedString As String = input.Substring(0, startLength)
        condensedString += New String("."c, ellipsisCount)
        condensedString += input.Substring(input.Length - endLength, endLength)

        Return condensedString
    End Function

    <Extension()>
    Public Function ToSnakeCase(ByVal aInput As String) As String
        If String.IsNullOrWhiteSpace(aInput) Then
            Return String.Empty
        End If

        ' Replace spaces with underscores and make lowercase
        Return aInput.Trim().Replace(" "c, "_"c).ToLowerInvariant()
    End Function

    <Extension()>
    Function ToPascalCase(ByVal input As String) As String
        Dim words As String() = input.Split({" "c}, StringSplitOptions.None) '20250831-Did contain , "_"c

        For i As Integer = 0 To words.Length - 1
            words(i) = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words(i).ToLower())
        Next

        Dim pascalCase As String = String.Join(" ", words)

        Return pascalCase
    End Function


    <Extension()>
    Public Function ToPascalCaseWithSpaces(ByVal input As String) As String
        Try
            Dim pascalWithSpaces As String = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2")
            Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(pascalWithSpaces)
        Catch ex As Exception
            Return input
        End Try

    End Function

    ''' <summary>
    ''' Used to truncate/concatenate a string.
    ''' E.g. 'Test Set 1 - Error Conditions', becomes 'TS1EC', for Test Case 'Codes' etc. E.g. 'TS1EC-01'
    ''' </summary>
    ''' <param name="aText"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function ToInitialCode(ByVal aText As String) As String
        If String.IsNullOrWhiteSpace(aText) Then Return String.Empty

        Dim lResult As New Text.StringBuilder
        ' Match words or numbers
        For Each lMatch As Match In Regex.Matches(aText, "\w+")
            Dim lToken As String = lMatch.Value
            ' If token starts with digit, keep the whole number; else take first letter
            If Char.IsDigit(lToken(0)) Then
                lResult.Append(lToken)
            Else
                lResult.Append(Char.ToUpper(lToken(0)))
            End If
        Next

        Return lResult.ToString()
    End Function

    <Extension()>
    Public Function ToPaddedCode(ByVal aNumber As Integer) As String
        If aNumber < 1000 Then
            Return aNumber.ToString("D3")   ' pad with zeros up to 3 digits
        Else
            Return aNumber.ToString()       ' no padding beyond 999
        End If
    End Function

    ''' <summary>
    ''' Limit to use with ExpandoObjects please.
    ''' </summary>
    ''' <param name="expando"></param>
    ''' <param name="fieldName"></param>
    ''' <returns></returns>
    <System.Runtime.CompilerServices.Extension()>
    Public Function HasField(expando As Object, fieldName As String) As Boolean
        Try
            Dim value As Object = CallByName(expando, fieldName, CallType.Get)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function GetAttributeValue(Of TAttribute As Attribute, TValue)(source As [Enum]) As TValue
        Dim memberInfo As MemberInfo = source.GetType().GetMember(source.ToString()).FirstOrDefault()
        If memberInfo IsNot Nothing Then
            Dim attribute As TAttribute = memberInfo.GetCustomAttribute(Of TAttribute)()
            If attribute IsNot Nothing Then
                Dim valueProperty As PropertyInfo = attribute.GetType().GetProperty("Value")
                If valueProperty IsNot Nothing Then
                    Return DirectCast(valueProperty.GetValue(attribute), TValue)
                End If
            End If
        End If
        Return Nothing
    End Function

    <Extension()>
    Public Sub ReplaceWith(Of T As Class)(ByRef obj As T, other As T)
        Dim size = Marshal.SizeOf(GetType(T))
        Dim ptr = Marshal.AllocHGlobal(size)
        Marshal.StructureToPtr(other, ptr, False)
        Marshal.PtrToStructure(ptr, obj)
        Marshal.FreeHGlobal(ptr)
    End Sub

    ''' <summary>
    ''' Add/Append and item to an array.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="arr"></param>
    ''' <param name="item"></param>
    <Extension()>
    Public Sub Add(Of T)(ByRef arr As T(), item As T)
        Array.Resize(arr, arr.Length + 1)
        arr(arr.Length - 1) = item
    End Sub

    ''' <summary>
    ''' Richtextbox GotoLine
    ''' </summary>
    ''' <param name="wantedLine_zero_based"></param>
    <Extension()>
    Public Sub GotoLine(ByVal richTextBox As RichTextBox, ByVal wantedLine_zero_based As Integer)
        Dim index As Integer = richTextBox.GetFirstCharIndexFromLine(wantedLine_zero_based)
        richTextBox.[Select](index, 0)
        richTextBox.ScrollToCaret()
    End Sub

    ''' <summary>
    ''' Richtextbox Highlight Line
    ''' </summary>
    ''' <param name="richTextBox"></param>
    ''' <param name="index"></param>
    ''' <param name="color"></param>
    <Extension()>
    Public Sub HighlightLine(ByVal richTextBox As RichTextBox, ByVal index As Integer, ByVal color As Color)
        richTextBox.SelectAll()
        richTextBox.SelectionBackColor = richTextBox.BackColor
        Dim lines = richTextBox.Lines
        If index < 0 OrElse index >= lines.Length Then Return
        Dim start = richTextBox.GetFirstCharIndexFromLine(index)
        Dim length = lines(index).Length
        richTextBox.[Select](start, length)
        richTextBox.SelectionBackColor = color
    End Sub

    ''' <summary>
    ''' Truncates a string to a max number of characters.
    ''' NB Can also use Strings.Left(str,int)
    ''' </summary>
    ''' <param name="value">The String to truncate</param>
    ''' <param name="maxLength">The maximum length of the string from the left.</param>
    ''' <returns></returns>
    <Extension()>
    Public Function Truncate(ByVal value As String, ByVal maxLength As Integer) As String
        If String.IsNullOrEmpty(value) Then Return value
        Return If(value.Length <= maxLength, value, value.Substring(0, maxLength))
    End Function

    <Extension()>
    Public Function LCase(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then Return value
        Return Strings.LCase(value)
    End Function

    ''' <summary>
    ''' For the ErrorProvider. Determines if there is an invalid control...a control with a validation error
    ''' </summary>
    ''' <param name="arErrorProvider"></param>
    ''' <param name="arControl"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function IsValid(ByRef arErrorProvider As ErrorProvider,
                            Optional ByRef arControl As Control = Nothing) As Boolean

        If arControl IsNot Nothing Then
            For Each Control As Control In arControl.Controls
                If arErrorProvider.GetError(Control) <> "" Then Return False
                If IsValid(arErrorProvider, Control) = False Then Return False
            Next
        Else
            For Each c As Control In arErrorProvider.ContainerControl.Controls
                If arErrorProvider.GetError(c) <> "" Then Return False

                For Each SubControl As Control In c.Controls
                    If arErrorProvider.GetError(SubControl) <> "" Then Return False

                    For Each SubSubControl In SubControl.Controls
                        If IsValid(arErrorProvider, SubSubControl) = False Then Return False
                    Next
                Next
            Next

        End If

        Return True
    End Function

    <Extension()>
    Public Sub RenameKey(Of TKey, TValue)(ByVal dic As IDictionary(Of TKey, TValue), ByVal fromKey As TKey, ByVal toKey As TKey)
        Dim value As TValue = dic(fromKey)
        dic.Remove(fromKey)
        dic(toKey) = value
    End Sub

    ''' <summary>
    ''' For the ErrorProvider. Returns control with a validation error or Nothing
    ''' </summary>
    ''' <param name="arErrorProvider"></param>
    ''' <param name="arControl"></param>
    ''' <returns></returns>
    <Extension()>
    Public Function getInvalidControl(ByRef arErrorProvider As ErrorProvider,
                            Optional ByRef arControl As Control = Nothing) As Control

        If arControl IsNot Nothing Then
            For Each Control As Control In arControl.Controls
                If arErrorProvider.GetError(Control) <> "" Then Return Control
                If getInvalidControl(arErrorProvider, Control) IsNot Nothing Then
                    Return getInvalidControl(arErrorProvider, Control)
                End If
            Next
        Else
            For Each c As Control In arErrorProvider.ContainerControl.Controls
                If arErrorProvider.GetError(c) <> "" Then Return c

                For Each SubControl As Control In c.Controls
                    If arErrorProvider.GetError(SubControl) <> "" Then Return SubControl

                    For Each SubSubControl In SubControl.Controls
                        If getInvalidControl(arErrorProvider, SubSubControl) IsNot Nothing Then
                            Return getInvalidControl(arErrorProvider, SubSubControl)
                        End If
                    Next
                Next
            Next

        End If

        Return Nothing
    End Function

    <Extension()>
    Public Function AddUnique(Of T)(list As List(Of T), item As T)

        If Not list.Contains(item) Then list.Add(item)
        Return list
    End Function

    <Extension()>
    Public Function MaximumValue(ByVal aiInt As Integer, ByVal aiMaximumValue As Integer) As Integer

        If aiInt > aiMaximumValue Then
            Return aiMaximumValue
        Else
            Return aiInt
        End If


    End Function

    <Extension()>
    Public Function MaximumValue(ByVal aiDbl As Double, ByVal aiMaximumValue As Integer) As Double

        If aiDbl > aiMaximumValue Then
            Return aiMaximumValue
        Else
            Return aiDbl
        End If

    End Function

    <Extension()>
    Public Function CompareWith(aarList1 As List(Of RDS.Column), aarList2 As List(Of RDS.Column)) As Integer

        If aarList1.Count <> aarList2.Count Then
            Return 1
        End If

        For Each lrElement In aarList2
            If Not aarList1.Contains(lrElement) Then
                Return 1
            End If
        Next

        Return 0

    End Function

    <Extension()>
    Public Function CountSubstring(ByVal asString As String, ByVal asSubstring As String) As Integer

        Return asString.Split(asSubstring).Length - 1

    End Function

    <Extension()>
    Public Function GetByDescription(ByRef aiEnum As [Enum], ByVal asDescription As String) As [Enum]

        aiEnum = CType([Enum].Parse(aiEnum.GetType, asDescription), [Enum])
        Return aiEnum

    End Function

    <Extension()>
    Public Function GetEnumValue(Of TEnum)(ByVal value As Integer) As TEnum

        Return CType(System.Enum.ToObject(GetType(TEnum), value), TEnum)
    End Function

    <Extension()>
    Public Function DescriptionAttr(Of T)(ByVal source As T) As String
        Dim fi As FieldInfo = source.[GetType]().GetField(source.ToString())
        Dim attributes As DescriptionAttribute() = CType(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())

        If attributes IsNot Nothing AndAlso attributes.Length > 0 Then
            Return attributes(0).Description
        Else
            Return source.ToString()
        End If
    End Function


    <Extension()>
    Public Function isBetween(ByRef asglNumber As Single, ByVal aiLowerVal As Integer, ByVal aiUpperVal As Integer) As Boolean

        Return (asglNumber >= aiLowerVal) And (asglNumber <= aiUpperVal)

    End Function

    <Extension()>
    Public Function RemoveDoubleWhiteSpace(ByRef asString As String) As String

        While asString.Contains("  ")
            asString = asString.Replace("  ", " ")
        End While

        Return asString

    End Function

    <Extension()>
    Public Function AppendString(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function RemoveWhitespace(ByRef asString As String) As String

        Return Regex.Replace(asString, "\s+", "")

    End Function

    <Extension()>
    Public Function AppendStringInColor(ByRef aoRichtextbox As RichTextBox,
                                        ByVal asStringExtension As String,
                                        ByVal aiColor As Color,
                                        Optional ByVal abInBold As Boolean = False) As RichTextBox

        Dim liOriginalColor As Color = aoRichtextbox.SelectionColor
        aoRichtextbox.Select(aoRichtextbox.TextLength, 0)
        aoRichtextbox.SelectionColor = aiColor

        If abInBold Then
            aoRichtextbox.SelectionFont = New Font(aoRichtextbox.Font, FontStyle.Bold)
        Else
            aoRichtextbox.SelectionFont = New Font(aoRichtextbox.Font, FontStyle.Regular)
        End If
        aoRichtextbox.AppendText(asStringExtension)

        'Return back to original color
        aoRichtextbox.Select(aoRichtextbox.TextLength, 0)
        aoRichtextbox.SelectionColor = liOriginalColor

        Return aoRichtextbox

    End Function

    <Extension()>
    Public Function AppendLine(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & vbCrLf & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function ReplaceFirst(ByRef asString As String, ByVal asFirstString As String, ByVal asReplaceString As String)

        Dim pos As Integer = asString.IndexOf(asFirstString)

        If pos < 0 Then
            Return asString
        Else
            asString = asString.Substring(0, pos) + asReplaceString + asString.Substring(pos + asFirstString.Length)
            Return asString
        End If

    End Function

    <Extension()>
    Public Function AppendDoubleLineBreak(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & vbCrLf & vbCrLf & asStringExtension
        Return asString

    End Function



    <Extension()>
    Public Function IsNumeric(ByRef asString As String) As Boolean

        Dim number As Integer
        Return Int32.TryParse(asString, number)

    End Function

    <Extension()>
    Public Iterator Function Permute(Of T)(ByVal sequence As IEnumerable(Of T)) As IEnumerable(Of IEnumerable(Of T))
        If sequence Is Nothing Then
            Return
        End If

        Dim list = sequence.ToList()

        If Not list.Any() Then
            Yield Enumerable.Empty(Of T)()
        Else
            Dim startingElementIndex = 0

            For Each startingElement In list
                Dim index = startingElementIndex
                Dim remainingItems = list.Where(Function(e, i) i <> index)

                For Each permutationOfRemainder In remainingItems.Permute()
                    Yield permutationOfRemainder.Prepend(startingElement)
                Next

                startingElementIndex += 1
            Next
        End If
    End Function

End Module
