
Namespace FBM.STM

    ''' <summary>
    ''' NB Each State is actually a ValueConstraint on the ValueType, but are stored redundently within the STDModel attribute of a ValueType.
    ''' </summary>
    Public Class State
        Implements IEquatable(Of FBM.STM.State)

        Public Model As FBM.Model = Nothing

        Public Name As String = ""

        Public ValueType As FBM.ValueType = Nothing

        Public IsStart As Boolean = False
        Public IsStop As Boolean = False

        ''' <summary>
        ''' Parameterless new
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByRef arValueType As FBM.ValueType, ByVal asStateName As String)

            Me.Model = arModel
            Me.ValueType = arValueType
            Me.Name = asStateName

        End Sub

        Public Shadows Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals
            Return (Me.Name = other.Name) And (Me.ValueType.Id = other.ValueType.Id)
        End Function

        ''' <summary>
        ''' Sets the State as a StartState for the STM
        ''' </summary>
        Public Function makeStartState() As FBM.Fact

            Me.IsStart = True

            'CMML
            Return Me.Model.addCMMLStartState(Me)

        End Function

    End Class

End Namespace
