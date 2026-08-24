Namespace FEQL

    Public Class Comparitor

        Public Function getFEQLMathComparitor() As FEQL.pcenumFEQLMathComparitor


            If Me._KEYWDEQUALS IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.Equals
            ElseIf Me._EQUALS IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.Equals
            ElseIf Me._KEYWDLESSTHAN IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.LessThan
            ElseIf Me._KEYWDLESSTHANEQUALS IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.LessThanOrEquals
            ElseIf Me._KEYWDGREATERTHAN IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.GreaterThan
            ElseIf Me._KEYWDGREATERTHANEQUALS IsNot Nothing Then
                Return FEQL.pcenumFEQLMathComparitor.GreaterThanOrEquals
            Else
                Return FEQL.pcenumFEQLMathComparitor.None
            End If

        End Function

        Public Function getFEQLMathComparitorToken() As String

            If Me._KEYWDEQUALS IsNot Nothing Then
                Return "="
            ElseIf Me._EQUALS IsNot Nothing Then
                Return "="
            ElseIf Me._KEYWDLESSTHAN IsNot Nothing Then
                Return "<"
            ElseIf Me._KEYWDLESSTHANEQUALS IsNot Nothing Then
                Return "<="
            ElseIf Me._KEYWDGREATERTHAN IsNot Nothing Then
                Return ">"
            ElseIf Me._KEYWDGREATERTHANEQUALS IsNot Nothing Then
                Return ">="
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

        Private _EQUALS As String = Nothing

        Public Shadows Property [EQUALS] As String
            Get
                Return Me._EQUALS
            End Get
            Set(value As String)
                Me._EQUALS = value
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

        Private _KEYWDLESSTHANEQUALS As String = Nothing
        Public Property KEYWDLESSTHANEQUALS As String
            Get
                Return Me._KEYWDLESSTHANEQUALS
            End Get
            Set(value As String)
                Me._KEYWDLESSTHANEQUALS = value
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

        Private _KEYWDGREATERTHANEQUALS As String = Nothing
        Public Property KEYWDGREATERTHANEQUALS As String
            Get
                Return Me._KEYWDGREATERTHANEQUALS
            End Get
            Set(value As String)
                Me._KEYWDGREATERTHANEQUALS = value
            End Set
        End Property

    End Class

End Namespace

