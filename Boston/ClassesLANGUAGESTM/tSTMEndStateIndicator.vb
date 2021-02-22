Imports System.Reflection

Namespace FBM.STM

    Public Class EndStateIndicator
        Implements IEquatable(Of EndStateIndicator)

        Public Model As STM.Model

        Public EndStateId As String = System.Guid.NewGuid.ToString

        Public Fact As FBM.Fact = Nothing 'The Fact that represents this EndStateIndicator in the CMML model (I.e. in the CoreElementHasElementType relation).

        Public ValueType As FBM.ValueType

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arSTModel As STM.Model, Optional ByVal asEndStateId As String = Nothing)

            Me.Model = arSTModel

            If asEndStateId IsNot Nothing Then
                Me.EndStateId = asEndStateId
            End If

        End Sub

        Public Shadows Function Equals(other As EndStateIndicator) As Boolean Implements IEquatable(Of EndStateIndicator).Equals
            Return Me.EndStateId = other.EndStateId
        End Function
    End Class

End Namespace
