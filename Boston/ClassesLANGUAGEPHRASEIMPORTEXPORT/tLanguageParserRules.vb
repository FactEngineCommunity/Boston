Imports System.Xml.Serialization

Namespace LPIO

    <Serializable()> _
    Public Class LanguageParserRules '<XmlType(Namespace:="", TypeName:="LanguageParserRules")> _

        Public LanguagePhrases As New List(Of LPIO.LanguagePhrase)

        Public Sub New()

        End Sub
    End Class

End Namespace

