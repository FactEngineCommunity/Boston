Namespace CQL

    Module publicFunctions

        Public Function GetCommentCQL(ByVal asCommentLine As String) As String

            Dim lsCommentCQL As String = ""

            lsCommentCQL = "/*".AppendString(vbCrLf)
            lsCommentCQL.AppendString(" * " & asCommentLine & vbCrLf)
            lsCommentCQL.AppendString(" */" & vbCrLf)

            Return lsCommentCQL

        End Function


    End Module

End Namespace
