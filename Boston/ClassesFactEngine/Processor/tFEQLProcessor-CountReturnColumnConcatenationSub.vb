Namespace FEQL
    Public Class CountReturnColumnConcatenationSub

        'COUNTRETURNCOLUMNCONCATENATIONSUB -> CONCATENATIONSYMBOL (QUOTEDSTRING | COUNTRETURNCOLUMN);

        Private _QUOTEDSTRING As String = Nothing
        Public Property QUOTEDSTRING As String
            Get
                Return Me._QUOTEDSTRING
            End Get
            Set(value As String)
                Me._QUOTEDSTRING = value
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

    End Class

End Namespace
