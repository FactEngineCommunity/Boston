Imports System.Xml.Serialization

Namespace LPIO

    <Serializable()> _
    Public Class LanguagePhrase '<XmlType(Namespace:="", TypeName:="LanguagePhrase")> _

        <XmlAttribute()> _
        Public PhraseId As Integer

        <XmlAttribute()> _
        Public PhraseType As pcenumPhraseType

        <XmlAttribute()> _
        Public ResolvesToLanguagePhraseId As String
        Public _ResolvesToPhrase As Language.LanguagePhrase
        Public Property ResolvesToPhrase() As Language.LanguagePhrase
            Get
                Return Me._ResolvesToPhrase
            End Get
            Set(ByVal value As Language.LanguagePhrase)
                Me._ResolvesToPhrase = value
                Me.ResolvesToLanguagePhraseId = Me._ResolvesToPhrase.PhraseId.ToString
            End Set
        End Property

        <XmlAttribute()> _
        Public Example As String = ""

        Public TokenSequence As New List(Of LPIO.LPTSWS)

        Private HelloWorld()() As String

    End Class

End Namespace

