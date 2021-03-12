Namespace FactEngine
    Public Class DatabaseConnection

        Public Connected As Boolean = False

        Public DefaultQueryLimit As Integer = 100

        Public Overridable Function GO(ByVal asQuery As String) As ORMQL.Recordset
            Return New ORMQL.Recordset
        End Function

        Public Overridable Function GONonQuery(ByVal asQuery As String) As ORMQL.Recordset
            Return New ORMQL.Recordset
        End Function

        Public Overridable Function createDatabase(ByVal asDatabaseLocationName As String) As ORMQL.Recordset
            Return New ORMQL.Recordset
        End Function

        ''' <summary>
        ''' Creates a new table in the database. Relational tablles must have at least one one column.
        ''' </summary>
        ''' <param name="arTable">The table to be created.</param>
        ''' <param name="arColumn">The column to be created for the new table.</param>
        Public Overridable Sub createTable(ByRef arTable As RDS.Table, ByRef arColumn As RDS.Column)
        End Sub

    End Class

End Namespace
