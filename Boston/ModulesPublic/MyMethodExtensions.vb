Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic

Module MyMethodExtensions

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
    Public Function isBetween(ByRef asglNumber As Single, ByVal aiLowerVal As Integer, ByVal aiUpperVal As Integer) As Boolean

        Return (asglNumber >= aiLowerVal) And (asglNumber <= aiUpperVal)

    End Function

    <Extension()>
    Public Function AppendString(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & asStringExtension
        Return asString

    End Function

    <Extension()>
    Public Function AppendLine(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & asStringExtension
        Return asString

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
