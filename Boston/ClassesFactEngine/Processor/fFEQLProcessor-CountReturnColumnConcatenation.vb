Namespace FEQL

    Public Class CountReturnColumnConcatenation

        'COUNTRETURNCOLUMNCONCATENATION -> (COUNTRETURNCOLUMN (COUNTRETURNCOLUMNCONCATENATIONSUB)+);

        Private _COUNTRETURNCOLUMN As FEQL.COUNTRETURNCOLUMN = Nothing
        Public Property COUNTRETURNCOLUMN As FEQL.COUNTRETURNCOLUMN
            Get
                Return Me._COUNTRETURNCOLUMN
            End Get
            Set(value As FEQL.COUNTRETURNCOLUMN)
                Me._COUNTRETURNCOLUMN = value
            End Set
        End Property

        Private _COUNTRETURNCOLUMNCONCATENATIONSUB As New List(Of FEQL.CountReturnColumnConcatenationSub)

        Public Property COUNTRETURNCOLUMNCONCATENATIONSUB As List(Of FEQL.CountReturnColumnConcatenationSub)
            Get
                Return Me._COUNTRETURNCOLUMNCONCATENATIONSUB
            End Get
            Set(value As List(Of FEQL.CountReturnColumnConcatenationSub))
                Me._COUNTRETURNCOLUMNCONCATENATIONSUB = value
            End Set
        End Property


    End Class

End Namespace
