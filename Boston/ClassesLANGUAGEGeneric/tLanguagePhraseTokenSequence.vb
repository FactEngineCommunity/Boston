Namespace Language

    Public Class LanguagePhraseTokenSequence

        Public Token As String = ""
        Public SequenceNr As Integer = 0
        Public LPTSWS As New List(Of Language.LanguagePhraseTokenSequenceWordSense)

        Public Sub New()

        End Sub

        Public Sub New(ByVal asToken As String, ByVal aiSequenceNr As Integer)

            Me.Token = asToken
            Me.SequenceNr = aiSequenceNr

        End Sub

        Public Overloads Function Equals(ByVal arLanguagePhraseTokenSequence As Language.LanguagePhraseTokenSequence) As Boolean

            Dim lbEquals As Boolean = True
            Dim liInd As Integer = 0
            Dim lrLPTSWS As Language.LanguagePhraseTokenSequenceWordSense


            If (Me.SequenceNr = arLanguagePhraseTokenSequence.SequenceNr) Then

                liInd = 0
                For Each lrLPTSWS In Me.LPTSWS
                    If arLanguagePhraseTokenSequence.LPTSWS.Contains(lrLPTSWS) Then

                    Else
                        lbEquals = False
                    End If
                    liInd += 1
                Next

            Else
                lbEquals = False
            End If

            Return lbEquals

        End Function

    End Class

End Namespace

