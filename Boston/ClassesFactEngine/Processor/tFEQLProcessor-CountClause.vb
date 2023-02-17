Namespace FEQL
    Public Class CountClause

        'KEYWDCOUNT (BROPEN (STAR | KEYWDDISTINCT (COLUMNSTRINGCONCATENATION | COUNTRETURNCOLUMN | MODELELEMENTNAME)) BRCLOSE )

        Private _KEYWDCOUNT As String = Nothing
        Public Property KEYWDCOUNT As String
            Get
                Return Me._KEYWDCOUNT
            End Get
            Set(value As String)
                Me._KEYWDCOUNT = value
            End Set
        End Property

        Private _STAR As String = Nothing
        Public Property STAR As String
            Get
                Return Me._STAR
            End Get
            Set(value As String)
                Me._STAR = value
            End Set
        End Property

        Private _KEYWDDISTINCT As String = Nothing
        Public Property KEYWDDISTINCT As String
            Get
                Return Me._KEYWDDISTINCT
            End Get
            Set(value As String)
                Me._KEYWDDISTINCT = value
            End Set
        End Property

        Private _MODELELEMENTNAME As String = Nothing
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _COUNTRETURNCOLUMN As FEQL.COUNTRETURNCOLUMN = Nothing
        Public Property COUNTRETURNCOLUMN As FEQL.COUNTRETURNCOLUMN
            Get
                Return Me._COUNTRETURNCOLUMN
            End Get
            Set(value As FEQL.COUNTRETURNCOLUMN)
                Me._COUNTRETURNCOLUMN = value
            End Set
        End Property

        Private _COUNTRETURNCOLUMNCONCATENATION As CountReturnColumnConcatenation = Nothing
        Public Property COUNTRETURNCOLUMNCONCATENATION As CountReturnColumnConcatenation
            Get
                Return Me._COUNTRETURNCOLUMNCONCATENATION
            End Get
            Set(value As CountReturnColumnConcatenation)
                Me._COUNTRETURNCOLUMNCONCATENATION = value
            End Set
        End Property

    End Class

End Namespace
