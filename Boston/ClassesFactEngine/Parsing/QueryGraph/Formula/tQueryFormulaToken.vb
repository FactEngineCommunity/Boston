Namespace FactEngine

    ''' <summary>
    ''' A QueryEdge may have a Formula, which is a List(Of tQueryFormulaToken).
    '''   NB The classes QueryFormulaToken and QueryNode both inherit from tQueryFormulaToken.
    ''' </summary>
    Public MustInherit Class tQueryFormulaToken
    End Class

    Public Class QueryFormulaToken
        Inherits tQueryFormulaToken

        ''' <summary>
        ''' A Token can be +,-,*,/,[Number],[ReservedWord],(,),=,<,>,<=,>=
        ''' NB A QueryNode can also be a QueryFormulaToken and inherits tQueryFormulaToken
        ''' </summary>
        Private _TOKEN As String = Nothing
        Public Property TOKEN As String
            Get
                Return Me._TOKEN
            End Get
            Set(value As String)
                Me._TOKEN = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asToken As String)
            Me.TOKEN = asToken
        End Sub

    End Class

End Namespace
