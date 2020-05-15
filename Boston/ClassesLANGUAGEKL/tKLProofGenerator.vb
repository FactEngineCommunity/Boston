Imports System.Collections.Specialized

Public Class tKLProofGenerator

    Public FreeVariable As New StringCollection
    Public [Function] As New StringCollection
    Public ProofStep As List(Of tKLProofStep)


    Sub New()

    End Sub

    Function is_free_variable_reserved(ByVal as_free_variable As String) As Boolean

        is_free_variable_reserved = False

        If Me.FreeVariable.Contains(as_free_variable) Then
            '---------------------------------------
            'The Function Label is already reserved
            '---------------------------------------
            is_free_variable_reserved = True
        Else
            is_free_variable_reserved = False
        End If

    End Function


    Function is_function_label_reserved(ByVal as_function_label As String) As Boolean

        is_function_label_reserved = False

        If Me.function.Contains(as_function_label) Then
            '---------------------------------------
            'The Function Label is already reserved
            '---------------------------------------
            is_function_label_reserved = True
        Else
            is_function_label_reserved = False
        End If

    End Function

    Sub reserve_free_variable(ByVal as_free_variable As String)

        If IsSomething(Me.FreeVariable.Contains(as_free_variable)) Then
            '---------------------------------------
            'The Free Variable is already reserved
            '---------------------------------------
        Else
            Me.FreeVariable.Add(as_free_variable)
        End If

    End Sub

    Sub reserve_function_label(ByVal as_function_label As String)

        If IsSomething(Me.function.Contains(as_function_label)) Then
            '---------------------------------------
            'The Function Label is already reserved
            '---------------------------------------            
        Else
            Me.function.Add(as_function_label)
        End If

    End Sub

End Class
