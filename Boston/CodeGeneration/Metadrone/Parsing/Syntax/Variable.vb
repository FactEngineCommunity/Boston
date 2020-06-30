Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Syntax.Strings

Namespace Parser.Syntax

    Friend Class Variable
        Public Enum Types
            [Null] = 0
            [Primitive] = 1
            [SourceRef] = 2
            [Variable] = 3
        End Enum

        Public Value As Object = Nothing
        Public Type As Types = Types.Null

        Public Sub New(ByVal Value As Object, ByVal Type As Types)
            If Type = Types.Primitive Then
                'Validate primitive type
                If Value Is Nothing Then
                    Throw New Exception("Reference to a null.")
                ElseIf TypeOf Value Is String Then
                    'ok
                ElseIf TypeOf Value Is Boolean Then
                    'ok
                ElseIf IsNumeric(Value) Then
                    'ok
                Else
                    Throw New Exception("Variables of type '" & Value.GetType.Name & "' not supported.")
                End If
            End If

            Me.Value = Value
            Me.Type = Type
        End Sub

    End Class

End Namespace