Namespace FEQL


    Public Class COUNTRETURNCOLUMN

        ''' <summary>
        ''' NB Is useless if the database name is not the same as the modelelementname. This best processed at the graph level.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Extrapolation As String
            Get
                Return NullVal(Me.MODELELEMENTNAME, "") & NullVal(Me.MODELELEMENTSUFFIX, "") & "." & NullVal(Me.COLUMNNAMESTR, "")
            End Get
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

        Private _MODELELEMENTSUFFIX As String = Nothing
        Public Property MODELELEMENTSUFFIX As String
            Get
                Return Me._MODELELEMENTSUFFIX
            End Get
            Set(value As String)
                Me._MODELELEMENTSUFFIX = value
            End Set
        End Property

        Private _COLUMNNAMESTR As String = Nothing
        Public Property COLUMNNAMESTR As String
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As String)
                Me._COLUMNNAMESTR = value
            End Set
        End Property

    End Class


End Namespace