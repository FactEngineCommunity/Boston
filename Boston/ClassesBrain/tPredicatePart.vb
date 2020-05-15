Namespace Language

    Public Class PredicatePart

        Public SequenceNr As Integer = 0 'The respective position of a PredicatePart within a Sentence.

        Public PreboundText As String 'Prebound text, normally in the form "SSSS-", preceding the ModelElement(Name) referenced by the referenced Role.
        Public PostboundText As String 'Postbound text, normally in the form "-SSSS", following the ModelElement(Name) referenced by the referenced Role.        

        Public ObjectName As String 'The ModelObject to which the PredicatePart relates.

        Public PredicatePartText As String 'The text of the PredicatePart.

        Public Sub New()

        End Sub

        Public Sub New(ByVal asPredicatePartText As String)

            Me.PredicatePartText = asPredicatePartText
            Me.PreboundText = ""
            Me.PostboundText = ""

        End Sub

    End Class

End Namespace
