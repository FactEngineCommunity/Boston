Namespace Parser.Syntax

    Friend Class FindTemplateException
        Inherits Exception

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
    End Class



    Friend Class CallTemplateException
        Inherits Exception

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub
    End Class



    Friend Class ExecException
        Inherits Exception

        Public LineNumber As Integer = 0

        Public Sub New(ByVal message As String, ByVal LineNumber As Integer)
            MyBase.New(message)
            Me.LineNumber = LineNumber
        End Sub
    End Class

End Namespace