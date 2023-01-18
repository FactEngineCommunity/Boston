Namespace FEQL
    Public Class WHICHCLAUSE

        Private _WHICHCLAUSEBROPEN As New List(Of String)
        Public Property WHICHCLAUSEBROPEN As List(Of String)
            Get
                Return Me._WHICHCLAUSEBROPEN
            End Get
            Set(value As List(Of String))
                Me._WHICHCLAUSEBROPEN = value
            End Set
        End Property

        Private _WHICHCLAUSEBRCLOSE As New List(Of String)
        Public Property WHICHCLAUSEBRCLOSE As List(Of String)
            Get
                Return Me._WHICHCLAUSEBRCLOSE
            End Get
            Set(value As List(Of String))
                Me._WHICHCLAUSEBRCLOSE = value
            End Set
        End Property

        Private _KEYWDIS As String = Nothing
        Public Property KEYWDIS As String
            Get
                Return Me._KEYWDIS
            End Get
            Set(value As String)
                Me._KEYWDIS = value
            End Set
        End Property

        Private _KEYWDISNOT As String = Nothing
        Public Property KEYWDISNOT As String
            Get
                Return Me._KEYWDISNOT
            End Get
            Set(value As String)
                Me._KEYWDISNOT = value
            End Set
        End Property

        Private _KEYWDA As String = Nothing
        Public Property KEYWDA As String
            Get
                Return Me._KEYWDA
            End Get
            Set(value As String)
                Me._KEYWDA = value
            End Set
        End Property

        Private _KEYWDAN As String = Nothing
        Public Property KEYWDAN As String
            Get
                Return Me._KEYWDAN
            End Get
            Set(value As String)
                Me._KEYWDAN = value
            End Set
        End Property

        Private _KEYWDNO As String = Nothing
        Public Property KEYWDNO As String
            Get
                Return Me._KEYWDNO
            End Get
            Set(value As String)
                Me._KEYWDNO = value
            End Set
        End Property

        Public Function getAndOr(Optional abDefault As String = Nothing) As String

            If Me._KEYWDAND IsNot Nothing Then
                Return "AND"
            ElseIf Me._KEYWDOR IsNot Nothing Then
                Return "OR"
            Else
                'CodeSafe
                Return abDefault
            End If
        End Function

        Private _KEYWDAND As String = Nothing
        Public Property KEYWDAND As String
            Get
                Return Me._KEYWDAND
            End Get
            Set(value As String)
                Me._KEYWDAND = value
            End Set
        End Property

        Private _KEYWDOR As String = Nothing
        Public Property KEYWDOR As String
            Get
                Return Me._KEYWDOR
            End Get
            Set(value As String)
                Me._KEYWDOR = value
            End Set
        End Property

        Private _KEYWDWHEREALSO As String = Nothing
        Public Property KEYWDWHEREALSO As String
            Get
                Return Me._KEYWDWHEREALSO
            End Get
            Set(value As String)
                Me._KEYWDWHEREALSO = value
                Me.KEYWDAND = "AND" 'Because the FEQL processing is based on KEYWDAND, and does not include checks for KEYWDALSO.
            End Set
        End Property

        Private _WHICHTHATCLAUSE As Object = Nothing 'NB Is used to disambiguate where the THAT is in the WHICHCLAUSE
        Public Property WHICHTHATCLAUSE As Object
            Get
                Return Me._WHICHTHATCLAUSE
            End Get
            Set(value As Object)
                Me._WHICHTHATCLAUSE = value
            End Set
        End Property

        Private _KEYWDTHAT As New List(Of String)
        Public Property KEYWDTHAT As List(Of String)
            Get
                Return Me._KEYWDTHAT
            End Get
            Set(value As List(Of String))
                Me._KEYWDTHAT = value
            End Set
        End Property

        Private _PREDICATE As New List(Of String)
        Public Property PREDICATE As List(Of String)
            Get
                Return Me._PREDICATE
            End Get
            Set(value As List(Of String))
                Me._PREDICATE = value
            End Set
        End Property

        Private _KEYWDWHICH As String = Nothing
        Public Property KEYWDWHICH As String
            Get
                Return Me._KEYWDWHICH
            End Get
            Set(value As String)
                Me._KEYWDWHICH = value
            End Set
        End Property

        Private _MODELELEMENT As New List(Of Object)
        Public Property MODELELEMENT As List(Of Object)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of Object))
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _MODELELEMENTNAME As New List(Of String)
        Public Property MODELELEMENTNAME As List(Of String)
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As List(Of String))
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _NODE As New List(Of NODE)

        Public Property NODE As List(Of NODE)
            Get
                Return Me._NODE
            End Get
            Set(value As List(Of NODE))
                Me._NODE = value
            End Set
        End Property


        Private _NODEPROPERTYIDENTIFICATION As Object = Nothing
        Public Property NODEPROPERTYIDENTIFICATION As Object
            Get
                Return Me._NODEPROPERTYIDENTIFICATION
            End Get
            Set(value As Object)
                Me._NODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

        Private _WITHCLAUSE As Object = Nothing 'NB Is used to disambiguate where the THAT is in the WHICHCLAUSE
        Public Property WITHCLAUSE As Object
            Get
                Return Me._WITHCLAUSE
            End Get
            Set(value As Object)
                Me._WITHCLAUSE = value
            End Set
        End Property

        Private _MATHCLAUSE As MATHCLAUSE = Nothing
        Public Property MATHCLAUSE As MATHCLAUSE
            Get
                Return Me._MATHCLAUSE
            End Get
            Set(value As MATHCLAUSE)
                Me._MATHCLAUSE = value
            End Set
        End Property

        Private _RECURSIVECLAUSE As RECURSIVECLAUSE = Nothing
        Public Property RECURSIVECLAUSE As RECURSIVECLAUSE
            Get
                Return Me._RECURSIVECLAUSE
            End Get
            Set(value As RECURSIVECLAUSE)
                Me._RECURSIVECLAUSE = value
            End Set
        End Property

    End Class

End Namespace
