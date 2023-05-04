Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageCategory
        Implements IEquatable(Of DataLineageCategory)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public SequenceNr As Integer 'The SequenceNr for positioning on a Form/Page

        Public DataLineagaeCategoryPropertyType As New List(Of DataLineage.DataLineageCategoryPropertyType)

        'Parameterless Constructor
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByVal asName As String,
                       ByVal aiSequenceNr As Integer)
            Try
                Me.Model = arModel
                Me.Name = asName
                Me.SequenceNr = aiSequenceNr

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Shadows Function Equals(other As DataLineageCategory) As Boolean Implements IEquatable(Of DataLineageCategory).Equals
            Return Me.Model.ModelId = other.Model.ModelId And Me.Name = other.Name
        End Function


    End Class

End Namespace
