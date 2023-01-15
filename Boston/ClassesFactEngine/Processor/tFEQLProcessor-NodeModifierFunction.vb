Namespace FEQL

    Public Class NodeModifierFunction

        Public Function GetNodeModifierFunction() As FEQL.pcenumFEQLComparitor

        End Function

        Private _KEYWDDATE As String = Nothing
        Public Property KEYWDDATE As String
            Get
                Return Me._KEYWDDATE
            End Get
            Set(value As String)
                Me._KEYWDDATE = value
            End Set
        End Property

        Private _KEYWDTIME As String = Nothing
        Public Property KEYWDTIME As String
            Get
                Return Me._KEYWDTIME
            End Get
            Set(value As String)
                Me._KEYWDTIME = value
            End Set
        End Property

    End Class

End Namespace

