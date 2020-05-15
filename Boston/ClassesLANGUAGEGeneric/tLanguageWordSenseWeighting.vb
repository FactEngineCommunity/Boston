Namespace Language

    Public Class LanguageWordSenseWeighting

        Public Sense As pcenumWordSense
        Public Weighting As Integer = 0

        Public Sub New(ByVal aiSense As pcenumWordSense)

            Me.Sense = aiSense
            Me.Weighting = 0

        End Sub

        Public Overloads Function Equals(ByVal other As Language.LanguageWordSenseWeighting) As Boolean

            If Me.Sense = other.Sense Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function IsWeightingGreaterThan0() As Boolean

            Return Me.Weighting > 0

        End Function





    End Class

End Namespace

