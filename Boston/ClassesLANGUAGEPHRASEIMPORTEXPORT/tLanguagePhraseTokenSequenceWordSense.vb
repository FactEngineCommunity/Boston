Imports System.Xml.Serialization

Namespace LPIO

    <Serializable()> _
    Public Class LPTSWS
        Implements IEquatable(Of LPIO.LPTSWS)

        <XmlAttribute()> _
        Public LanguagePhraseId As Integer

        Private _Phrase As Language.LanguagePhrase
        Public Property Phrase() As Language.LanguagePhrase
            Get
                Return Me._Phrase
            End Get
            Set(ByVal value As Language.LanguagePhrase)
                Me._Phrase = value
                Me.LanguagePhraseId = Me._Phrase.PhraseId
            End Set
        End Property

        <XmlAttribute()> _
        Public Token As String = ""

        <XmlAttribute()> _
        Public SequenceNr As Integer

        <XmlAttribute()> _
        Public WordSense As pcenumWordSense

        Public Sub New()

        End Sub

        Public Overloads Function Equals(ByVal other As LPTSWS) As Boolean Implements System.IEquatable(Of LPTSWS).Equals

            Equals = True

            If (Me.SequenceNr = other.SequenceNr) And (Me.WordSense = other.WordSense) Then
                Equals = True
            Else
                Equals = False
            End If

        End Function
    End Class

End Namespace

