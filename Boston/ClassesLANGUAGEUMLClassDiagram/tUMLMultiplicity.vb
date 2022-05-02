Namespace UML

    <Serializable()>
    Public Class Multiplicity

        Public Id As String = ""
        Public Name As String = ""
        Public Attribute As String = ""
        Public LowerBound As String = "0"
        Public UpperBound As String = "1"

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
            Me.Name = Me.Id
        End Sub

        Public Sub New(ByVal asMultiplicityId As String, ByVal asAttributeId As String, Optional ByVal asLowerBound As String = "0", Optional ByVal asUpperBound As String = "1")

            Me.Id = asMultiplicityId
            Me.Name = Me.Id
            Me.Attribute = asAttributeId
            Me.LowerBound = asLowerBound
            Me.UpperBound = asUpperBound

        End Sub

        Public Function EqualsByAttributeId(ByVal other As UML.Multiplicity) As Boolean

            If other.Attribute = Me.Attribute Then
                Return True
            Else
                Return False
            End If

        End Function


    End Class

End Namespace
