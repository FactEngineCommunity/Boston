Imports System.Xml.Serialization

Namespace Language
    <Serializable()> _
    <XmlType(Namespace:="http://www.LANGUAGE.com", TypeName:="LanguagePhrase")> _
    Public Class LanguagePhrase
        Implements iObjectRelationalMap(Of Language.LanguagePhrase)

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
        Public Example As String

        Public TokenSequence As New List(Of Language.LanguagePhraseTokenSequence)

        Public Sub New()

        End Sub

        Public Function EqualsByTokens(ByVal arLanguagePhrase As Language.LanguagePhrase) As Boolean

            Dim lrTokenSequence As Language.LanguagePhraseTokenSequence
            Dim liInd As Integer = 0

            EqualsByTokens = True

            For Each lrTokenSequence In Me.TokenSequence

                If lrTokenSequence.Token = "*" Then
                    '--------------------------
                    'Guaranteed match, great!
                    '--------------------------
                Else
                    If lrTokenSequence.Token = arLanguagePhrase.TokenSequence(liInd).Token Then
                        '-------------------------
                        'Great, the Tokens match
                        '-------------------------
                    Else
                        EqualsByTokens = False
                    End If
                End If

                liInd += 1
            Next

        End Function

        Public Sub Create() Implements iObjectRelationalMap(Of LanguagePhrase).Create
            Call TableLanguagePhrase.AddLanguagePhrase(Me)
        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of LanguagePhrase).Delete
            Call TableLanguagePhrase.DeleteLanguagePhrase(Me)
        End Sub

        Public Function Load() As LanguagePhrase Implements iObjectRelationalMap(Of LanguagePhrase).Load
            Call TableLanguagePhrase.GetLanguagePhraseDetails(Me)
            Return Me
        End Function

        Public Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of LanguagePhrase).Save

            If abRapidSave Then
                Call TableLanguagePhrase.AddLanguagePhrase(Me)
            Else
                If TableLanguagePhrase.ExistsLanguagePhrase(Me) Then
                    Call TableLanguagePhrase.UpdateLanguagePhrase(Me)
                Else
                    Call TableLanguagePhrase.AddLanguagePhrase(Me)
                End If
            End If

        End Sub
    End Class

End Namespace

