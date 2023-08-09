Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageItem
        Implements IEquatable(Of DataLineageItem)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public DataLineageCategory As New DataLineage.DataLineageCategory

        Public DataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)

        'Parameterless Constructor
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asName As String)
            Try
                Me.Model = arModel
                Me.Name = asName
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
