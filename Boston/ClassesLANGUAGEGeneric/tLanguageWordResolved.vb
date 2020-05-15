
Namespace Language

    Public Class WordResolved

        Public Word As String
        Public Sense As pcenumWordSense


        Public Sub New(ByVal asWord As String, ByVal aiWordSense As pcenumWordSense)

            Me.Word = asWord
            Me.Sense = aiWordSense

        End Sub

    End Class

End Namespace
