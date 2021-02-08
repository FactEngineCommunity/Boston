Imports System.Reflection

Namespace FBM.STM

    Public Class EndStateIndicator

        Public Model As STM.Model

        Public EndStateId As String = System.Guid.NewGuid.ToString

        Public Fact As FBM.Fact = Nothing 'The Fact that represents this EndStateIndicator in the CMML model (I.e. in the CoreElementHasElementType relation).

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arSTModel As STM.Model)
            Me.Model = arSTModel
        End Sub

    End Class

End Namespace
