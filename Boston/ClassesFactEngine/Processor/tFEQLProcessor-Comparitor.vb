Namespace FEQL

    Public Class Comparitor

        Public Function getFEQLMathComparitor() As FEQL.pcenumFEQLMathComparitor

            If Me._KEYWDEQUALS IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.Equals
            ElseIf Me._KEYWDLESSTHAN IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.LessThan
            ElseIf Me._KEYWDGREATERTHAN IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.GreaterThan
            Else
                Return FEQL.pcenumFEQLMathComparitor.None
            End If

        End Function

        Public Function getFEQLMathComparitorToken() As String

            If Me._KEYWDEQUALS IsNot Nothing Then
                Return "="
            ElseIf Me._KEYWDLESSTHAN IsNot Nothing Then
                Return "<"
            ElseIf Me._KEYWDGREATERTHAN IsNot Nothing Then
                Return ">"
            Else
                Return ""
            End If

        End Function


        Private _KEYWDEQUALS As String = Nothing
        Public Property KEYWDEQUALS As String
            Get
                Return Me._KEYWDEQUALS
            End Get
            Set(value As String)
                Me._KEYWDEQUALS = value
            End Set
        End Property

        Private _KEYWDLESSTHAN As String = Nothing
        Public Property KEYWDLESSTHAN As String
            Get
                Return Me._KEYWDLESSTHAN
            End Get
            Set(value As String)
                Me._KEYWDLESSTHAN = value
            End Set
        End Property

        Private _KEYWDGREATERTHAN As String = Nothing
        Public Property KEYWDGREATERTHAN As String
            Get
                Return Me._KEYWDGREATERTHAN
            End Get
            Set(value As String)
                Me._KEYWDGREATERTHAN = value
            End Set
        End Property

    End Class

End Namespace

