Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic

Module MyMethodExtensions

    <Extension()> _
    Public Function AddUnique(Of T)(list As List(Of T), item As T)

        If Not list.Contains(item) Then list.Add(item)
        Return list
    End Function

    <Extension()> _
    Public Function MaximumValue(ByVal aiInt As Integer, ByVal aiMaximumValue As Integer) As Integer

        If aiInt > aiMaximumValue Then
            Return aiMaximumValue
        Else
            Return aiInt
        End If


    End Function

    <Extension()> _
    Public Function MaximumValue(ByVal aiDbl As Double, ByVal aiMaximumValue As Integer) As Double

        If aiDbl > aiMaximumValue Then
            Return aiMaximumValue
        Else
            Return aiDbl
        End If

    End Function

    <Extension()> _
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

    <Extension()> _
    Public Function CountSubstring(ByVal asString As String, ByVal asSubstring As String) As Integer

        Return asString.Split(asSubstring).Length - 1

    End Function

    <Extension()> _
    Public Function GetByDescription(ByRef aiEnum As [Enum], ByVal asDescription As String) As [Enum]

        aiEnum = CType([Enum].Parse(aiEnum.GetType, asDescription), [Enum])
        Return aiEnum

    End Function

    <Extension()> _
    Public Function isBetween(ByRef asglNumber As Single, ByVal aiLowerVal As Integer, ByVal aiUpperVal As Integer) As Boolean

        Return (asglNumber >= aiLowerVal) And (asglNumber <= aiUpperVal)

    End Function

    <Extension()> _
    Public Function AppendString(ByRef asString As String, ByVal asStringExtension As String) As String

        asString = asString & asStringExtension
        Return asString

    End Function

    <Extension()> _
    Public Function IsNumeric(ByRef asString As String) As Boolean

        Dim number As Integer
        Return Int32.TryParse(asString, number)

    End Function

End Module
