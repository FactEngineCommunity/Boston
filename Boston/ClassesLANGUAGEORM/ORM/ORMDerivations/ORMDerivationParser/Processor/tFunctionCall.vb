Namespace Derivations

    ''' <summary>
    ''' Object representation of the TinyPG FUNCTION_CALL production.
    ''' </summary>
    Public Class FunctionCall

        Public Property FUNCTION_NAME As String = String.Empty
        Public Property OPEN_PARENTHESIS As String = String.Empty
        Public Property FUNCTION_ARGUMENT_LIST As FunctionArgumentList
        Public Property CLOSE_PARENTHESIS As String = String.Empty

    End Class

End Namespace
