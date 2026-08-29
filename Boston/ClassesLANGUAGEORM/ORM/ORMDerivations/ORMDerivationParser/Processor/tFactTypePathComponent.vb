Namespace Derivations

    ''' <summary>
    ''' Object representation of one TinyPG FACT_TYPE_PATH_COMPONENT choice.
    ''' Exactly one alternative is populated for a valid parse node.
    ''' </summary>
    Public Class FactTypePathComponent

        Public Property KEYWD_FOR_SOME As String = String.Empty
        Public Property KEYWD_SOME As String = String.Empty
        Public Property KEYWD_EACH As String = String.Empty
        Public Property KEYWD_NO As String = String.Empty
        Public Property KEYWD_THAT As String = String.Empty
        Public Property KEYWD_POSSIBLE_VALUES_OF As String = String.Empty
        Public Property KEYWD_BY_DEFINITION As String = String.Empty
        Public Property KEYWD_AT_LEAST As String = String.Empty
        Public Property KEYWD_AT_MOST As String = String.Empty
        Public Property FUNCTION_CALL As FunctionCall
        Public Property PARENTHESISED_DERIVATION_EXPRESSION As ParenthesisedDerivationExpression
        Public Property COMPARISON_OPERATOR As String = String.Empty
        Public Property ARITHMETIC_OPERATOR As String = String.Empty
        Public Property NUMBER_LITERAL As String = String.Empty
        Public Property TEXT_LITERAL As String = String.Empty
        Public Property COMMA_SEPARATOR As String = String.Empty
        Public Property MODEL_ELEMENT_REFERENCE As ModelElementReference
        Public Property PREDICATE_WORD As String = String.Empty

    End Class

End Namespace
