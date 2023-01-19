Namespace FEQL
    Public Class NODEPROPERTYIDENTIFICATION

        Public Function getComparitorType() As pcenumFEQLComparitor

            If Me.BANG IsNot Nothing Then
                Return pcenumFEQLComparitor.Bang
            ElseIf Me.CARRET IsNot Nothing Then
                Return pcenumFEQLComparitor.Carret
            ElseIf Me.COLON IsNot Nothing Then
                Return pcenumFEQLComparitor.Colon
            ElseIf Me.INCOMPARITOR IsNot Nothing Then
                Return pcenumFEQLComparitor.InComparitor
            ElseIf Me.LIKECOMPARITOR IsNot Nothing Then
                Return pcenumFEQLComparitor.LikeComparitor
            End If

        End Function

        Public Function GetNodeModifierFunction() As FEQL.pcenumFEQLNodeModifierFunction

            If Me.NODEMODIFIERFUNCTION IsNot Nothing Then
                Return Me.NODEMODIFIERFUNCTION.GetNodeModifierFunction
            Else
                Return FEQL.pcenumFEQLNodeModifierFunction.None
            End If

        End Function

        Private _BANG As String = Nothing
        Public Property BANG As String
            Get
                Return Me._BANG
            End Get
            Set(value As String)
                Me._BANG = value
            End Set
        End Property

        Private _CARRET As String = Nothing
        Public Property CARRET As String
            Get
                Return Me._CARRET
            End Get
            Set(value As String)
                Me._CARRET = value
            End Set
        End Property

        Private _COLON As String = Nothing
        Public Property COLON As String
            Get
                Return Me._COLON
            End Get
            Set(value As String)
                Me._COLON = value
            End Set
        End Property

        Private _INCOMPARITOR As String = Nothing
        Public Property INCOMPARITOR As String
            Get
                Return Me._INCOMPARITOR
            End Get
            Set(value As String)
                Me._INCOMPARITOR = value
            End Set
        End Property

        Private _LIKECOMPARITOR As String = Nothing
        Public Property LIKECOMPARITOR As String
            Get
                Return Me._LIKECOMPARITOR
            End Get
            Set(value As String)
                Me._LIKECOMPARITOR = value
            End Set
        End Property


        Private _MODELELEMENT As New Object
        Public Property MODELELEMENT As Object
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As Object)
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _IDENTIFIER As New List(Of String)
        Public Property IDENTIFIER As List(Of String)
            Get
                '20210911-VM-If this causes problems revert to just returning Me._IDENTIFIER
                Dim lasIdentifier As New List(Of String)
                lasIdentifier.AddRange(Me._IDENTIFIER)
                lasIdentifier.AddRange(Me._EMAILADDRESS)
                Return lasIdentifier
            End Get
            Set(value As List(Of String))
                Me._IDENTIFIER = value
            End Set
        End Property

        Private _EMAILADDRESS As New List(Of String)
        Public Property EMAILADDRESS As List(Of String)
            Get
                Return Me._EMAILADDRESS
            End Get
            Set(value As List(Of String))
                Me._EMAILADDRESS = value
            End Set
        End Property

        Private _QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
        Public Property QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
            Get
                Return Me._QUOTEDIDENTIFIERLIST
            End Get
            Set(value As FEQL.QuotedIdentifierList)
                Me._QUOTEDIDENTIFIERLIST = value
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

        Private _NODEMODIFIERFUNCTION As FEQL.NodeModifierFunction = Nothing
        Public Property NODEMODIFIERFUNCTION As FEQL.NodeModifierFunction
            Get
                Return Me._NODEMODIFIERFUNCTION
            End Get
            Set(value As FEQL.NodeModifierFunction)
                Me._NODEMODIFIERFUNCTION = value
            End Set
        End Property

    End Class

End Namespace
