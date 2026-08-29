Imports System.Collections.Generic

Namespace Derivations

    ''' <summary>
    ''' Object representation of the TinyPG FUNCTION_ARGUMENT_LIST production.
    ''' COMMA_SEPARATOR item n separates expression n from expression n + 1.
    ''' </summary>
    Public Class FunctionArgumentList

        Public Property DERIVATION_EXPRESSION As New List(Of DerivationExpression)
        Public Property COMMA_SEPARATOR As New List(Of String)

    End Class

End Namespace
