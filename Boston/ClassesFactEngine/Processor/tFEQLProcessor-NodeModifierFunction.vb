Namespace FEQL

    Public Class NodeModifierFunction

        Public Function GetNodeModifierFunction() As FEQL.pcenumFEQLNodeModifierFunction
            If Me.KEYWDDATE IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Date
            ElseIf Me.KEYWDMONTH IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Month
            ElseIf Me.KEYWDYEAR IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Year
            ElseIf Me.KEYWDTIME IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Time
            ElseIf Me.KEYWDTOLOWER IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.ToLower
            ElseIf Me.KEYWDTOUPPER IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.ToUpper
            Else
                Return pcenumFEQLNodeModifierFunction.None
            End If
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

        Private _KEYWDMONTH As String = Nothing
        Public Property KEYWDMONTH As String
            Get
                Return Me._KEYWDMONTH
            End Get
            Set(value As String)
                Me._KEYWDMONTH = value
            End Set
        End Property

        Private _KEYWDYEAR As String = Nothing
        Public Property KEYWDYEAR As String
            Get
                Return Me._KEYWDYEAR
            End Get
            Set(value As String)
                Me._KEYWDYEAR = value
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

        Private _KEYWDTOLOWER As String = Nothing
        Public Property KEYWDTOLOWER As String
            Get
                Return Me._KEYWDTOLOWER
            End Get
            Set(value As String)
                Me._KEYWDTOLOWER = value
            End Set
        End Property

        Private _KEYWDTOUPPER As String = Nothing
        Public Property KEYWDTOUPPER As String
            Get
                Return Me._KEYWDTOUPPER
            End Get
            Set(value As String)
                Me._KEYWDTOUPPER = value
            End Set
        End Property

    End Class

End Namespace

