Namespace FEQL

    Public Class FACTREADINGClause

        Private _MODELELEMENTNAME As New List(Of String)
        Public Property MODELELEMENTNAME As List(Of String)
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As List(Of String))
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _PREDICATECLAUSE As New List(Of FEQL.PREDICATECLAUSE)
        Public Property PREDICATECLAUSE As List(Of FEQL.PREDICATECLAUSE)
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As List(Of FEQL.PREDICATECLAUSE))
                Me._PREDICATECLAUSE = value
            End Set
        End Property

        'Private _DERIVATIONCLAUSE
    End Class

End Namespace
