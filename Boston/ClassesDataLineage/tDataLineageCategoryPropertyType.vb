Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageCategoryPropertyType
        Implements IEquatable(Of DataLineageItem)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public PropertyType As String 'E.g. "Document Name", "Page Nr"

        Public SequenceNr As Integer 'The sequence within the Category as displayed within a page.

        Public Hidden As Boolean 'Whether the user can see the Property Type or not.

        'Parameterless Constructor
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByVal asName As String,
                       ByVal asPropertyType As String,
                       ByVal aiSequenceNr As Integer)
            Try
                Me.Model = arModel
                Me.Name = asName
                Me.PropertyType = asPropertyType
                Me.SequenceNr = aiSequenceNr

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Shadows Function Equals(other As DataLineageItem) As Boolean Implements IEquatable(Of DataLineageItem).Equals
            Return Me.Name = other.Name
        End Function


    End Class

End Namespace
