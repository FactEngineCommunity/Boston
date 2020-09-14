Namespace FactEngine
    Public Interface iDatabaseConnection

        Function GO(ByVal asQuery As String) As ORMQL.Recordset
        Function GONonQuery(ByVal asQuery As String) As ORMQL.Recordset

    End Interface

End Namespace
