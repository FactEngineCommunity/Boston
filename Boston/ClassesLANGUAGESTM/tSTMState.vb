
Namespace FBM.STM

    ''' <summary>
    ''' NB Each State is actually a ValueConstraint on the ValueType, but are stored redundently within the STDModel attribute of a ValueType.
    ''' </summary>
    Public Class State
        Implements IEquatable(Of FBM.STM.State)

        Public Name As String = ""

        Public ValueType As FBM.ValueType = Nothing

        Public IsStart As Boolean = False
        Public IsStop As Boolean = False

        ''' <summary>
        ''' Parameterless new
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arValueType As FBM.ValueType, ByVal asStateName As String)

            Me.ValueType = arValueType
            Me.Name = asStateName

        End Sub

        Public Shadows Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals
            Return (Me.Name = other.Name) And (Me.ValueType.Id = other.ValueType.Id)
        End Function

    End Class

End Namespace
