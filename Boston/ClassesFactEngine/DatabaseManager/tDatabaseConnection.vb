Namespace FactEngine
    Public Class DatabaseConnection
        Public Overridable Function GO(ByVal asQuery As String) As ORMQL.Recordset
            Return New ORMQL.Recordset
        End Function
    End Class

End Namespace
