Namespace Language

    Public Class LanguagePhraseTokenSequenceWordSense
        Implements IEquatable(Of Language.LanguagePhraseTokenSequenceWordSense)
        Implements iObjectRelationalMap(Of Language.LanguagePhraseTokenSequenceWordSense)

        Public Phrase As Language.LanguagePhrase
        Public Token As String
        Public SequenceNr As Integer
        Public WordSense As pcenumWordSense

        Public Overloads Function Equals(ByVal other As LanguagePhraseTokenSequenceWordSense) As Boolean Implements System.IEquatable(Of LanguagePhraseTokenSequenceWordSense).Equals

            Equals = True

            If (Me.SequenceNr = other.SequenceNr) And (Me.WordSense = other.WordSense) Then
                Equals = True
            Else
                Equals = False
            End If


        End Function

        Public Sub Create() Implements iObjectRelationalMap(Of LanguagePhraseTokenSequenceWordSense).Create
            Call TableLanguagePhraseTokenSequenceWordSense.AddLPTSWS(Me)
        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of LanguagePhraseTokenSequenceWordSense).Delete
            Call TableLanguagePhraseTokenSequenceWordSense.DeleteValueType(Me)
        End Sub

        Public Function Load() As LanguagePhraseTokenSequenceWordSense Implements iObjectRelationalMap(Of LanguagePhraseTokenSequenceWordSense).Load

            Return Me
        End Function

        Public Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of LanguagePhraseTokenSequenceWordSense).Save

            If abRapidSave Then
                TableLanguagePhraseTokenSequenceWordSense.AddLPTSWS(Me)
            Else
                If TableLanguagePhraseTokenSequenceWordSense.ExistsLPTSWS(Me) Then
                    '------------------------------------------------------------
                    'Nothing to do here. PrimaryKey is all fields in the table.
                    '------------------------------------------------------------
                Else
                    TableLanguagePhraseTokenSequenceWordSense.AddLPTSWS(Me)
                End If
            End If

        End Sub

    End Class

End Namespace

