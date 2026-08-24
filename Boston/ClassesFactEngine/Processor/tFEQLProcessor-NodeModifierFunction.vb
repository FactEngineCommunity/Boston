Namespace FEQL

    Public Class NodeModifierFunction

        Public Function GetNodeModifierFunction() As FEQL.pcenumFEQLNodeModifierFunction
            If Me.KEYWDDATE IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Date
            ElseIf Me.KEYWDMONTH IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Month
            ElseIf Me.KEYWDYEAR IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Year
            ElseIf Me.KEYWDHOUR IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Hour
            ElseIf Me.KEYWDTIME IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Time
            ElseIf Me.KEYWDTOLOWER IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.ToLower
            ElseIf Me.KEYWDTOUPPER IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.ToUpper
            ElseIf Me.KEYWDSUM IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Sum
            ElseIf Me.KEYWDAVG IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Average
            ElseIf Me.KEYWDMAX IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Max
            ElseIf Me.KEYWDMIN IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Min
            ElseIf Me.KEYWDBETWEEN IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Between
            ElseIf Me.KEYWDNEXT IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Next
            ElseIf Me.KEYWDTHIS IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.This
            ElseIf Me.KEYWDTODAY IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Today
            ElseIf Me.KEYWDTOMORROW IsNot Nothing Then
                Return FEQL.pcenumFEQLNodeModifierFunction.Tomorrow
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

        Private _KEYWDHOUR As String = Nothing
        Public Property KEYWDHOUR As String
            Get
                Return Me._KEYWDHOUR
            End Get
            Set(value As String)
                Me._KEYWDHOUR = value
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

        Private _KEYWDSUM As String = Nothing
        Public Property KEYWDSUM As String
            Get
                Return Me._KEYWDSUM
            End Get
            Set(value As String)
                Me._KEYWDSUM = value
            End Set
        End Property

        Private _KEYWDAVG As String = Nothing
        Public Property KEYWDAVG As String
            Get
                Return Me._KEYWDAVG
            End Get
            Set(value As String)
                Me._KEYWDAVG = value
            End Set
        End Property

        Private _KEYWDMAX As String = Nothing
        Public Property KEYWDMAX As String
            Get
                Return Me._KEYWDMAX
            End Get
            Set(value As String)
                Me._KEYWDMAX = value
            End Set
        End Property

        Private _KEYWDMIN As String = Nothing
        Public Property KEYWDMIN As String
            Get
                Return Me._KEYWDMIN
            End Get
            Set(value As String)
                Me._KEYWDMIN = value
            End Set
        End Property

        Private _KEYWDBETWEEN As String = Nothing
        Public Property KEYWDBETWEEN As String
            Get
                Return Me._KEYWDBETWEEN
            End Get
            Set(value As String)
                Me._KEYWDBETWEEN = value
            End Set
        End Property

        Private _BETWEENCLAUSE As FEQL.BETWEENClause = Nothing
        Public Property BETWEENCLAUSE As FEQL.BETWEENClause
            Get
                Return Me._BETWEENCLAUSE
            End Get
            Set(value As FEQL.BETWEENClause)
                Me._BETWEENCLAUSE = value
            End Set
        End Property

        Private _KEYWDNEXT As String = Nothing
        Public Property KEYWDNEXT As String
            Get
                Return Me._KEYWDNEXT
            End Get
            Set(value As String)
                Me._KEYWDNEXT = value
            End Set
        End Property

        Private _NEXTCLAUSE As FEQL.NEXTClause = Nothing
        Public Property NEXTCLAUSE As FEQL.NEXTClause
            Get
                Return Me._NEXTCLAUSE
            End Get
            Set(value As FEQL.NEXTClause)
                Me._NEXTCLAUSE = value
            End Set
        End Property

        Private _KEYWDTHIS As String = Nothing
        Public Property KEYWDTHIS As String
            Get
                Return Me._KEYWDTHIS
            End Get
            Set(value As String)
                Me._KEYWDTHIS = value
            End Set
        End Property

        Private _THISCLAUSE As FEQL.THISClause = Nothing
        Public Property THISCLAUSE As FEQL.THISClause
            Get
                Return Me._THISCLAUSE
            End Get
            Set(value As FEQL.THISClause)
                Me._THISCLAUSE = value
            End Set
        End Property

        Private _KEYWDTODAY As String = Nothing
        Public Property KEYWDTODAY As String
            Get
                Return Me._KEYWDTODAY
            End Get
            Set(value As String)
                Me._KEYWDTODAY = value
            End Set
        End Property

        Private _KEYWDTOMORROW As String = Nothing
        Public Property KEYWDTOMORROW As String
            Get
                Return Me._KEYWDTOMORROW
            End Get
            Set(value As String)
                Me._KEYWDTOMORROW = value
            End Set
        End Property

    End Class

End Namespace

