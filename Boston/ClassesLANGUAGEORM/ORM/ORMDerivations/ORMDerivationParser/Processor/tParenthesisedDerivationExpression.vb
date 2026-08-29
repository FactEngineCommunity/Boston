Namespace Derivations

    ''' <summary>
    ''' Object representation of the TinyPG PARENTHESISED_DERIVATION_EXPRESSION production.
    ''' </summary>
    Public Class ParenthesisedDerivationExpression

        Public Property OPEN_PARENTHESIS As String = String.Empty
        Public Property DERIVATION_EXPRESSION As DerivationExpression
        Public Property CLOSE_PARENTHESIS As String = String.Empty

    End Class

End Namespace
