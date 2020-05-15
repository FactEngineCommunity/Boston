Namespace Database

    Public Module publicDatabase

        Public Function MakeStringSafe(ByVal asString As String) As String

            Dim lsReturnString As String = ""

            lsReturnString = asString.Replace("'", "''")

            Return lsReturnString

        End Function

        Public Function RevertString(ByVal asString As String) As String

            Dim lsReturnString As String = ""

            lsReturnString = asString.Replace("''", "`")

            Return lsReturnString

        End Function


    End Module

End Namespace
