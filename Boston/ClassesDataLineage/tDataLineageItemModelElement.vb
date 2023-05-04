Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageItemModelElement
        Implements IEquatable(Of DataLineageItemModelElement)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public ModelElementType As String 'E.g. ValueType

        Public Symbol As String 'The Symbol of the Model Element. I.e. E.g. The Id of a Value Type. E.g. "Customer_Name"

        'Parameterless Constructor
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asDataLineageItemName">The name of the Data Lineage Item.</param>
        ''' <param name="asModelElementName">The Model Element that makes up the Data Lineage Item.
        '''   NB A Data Lineage Item may have more than one Model Element in its makeup. E.g. A RDS Column has FactType, Role, ValueType</param>
        ''' <param name="AsSymbol">The Symbol (Id/Name) of the Model Element that corresponds to the Data Lineage Item. E.g. "Customer" Id of Entity Type called, Customer. And where Id in Boston is the same as Name and is the Symbol of the Model Element.</param>
        Public Sub New(ByRef arModel As FBM.Model,
                       ByVal asDataLineageItemName As String,
                       ByVal asModelElementType As String,
                       ByVal AsSymbol As String)
            Try
                Me.Model = arModel
                Me.Name = asDataLineageItemName
                Me.ModelElementType = asModelElementType
                Me.Symbol = AsSymbol
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Shadows Function Equals(other As DataLineageItemModelElement) As Boolean Implements IEquatable(Of DataLineageItemModelElement).Equals
            Return Me.Name = other.Name And Me.ModelElementType = other.ModelElementType And Me.Symbol = other.Symbol
        End Function


    End Class

End Namespace
