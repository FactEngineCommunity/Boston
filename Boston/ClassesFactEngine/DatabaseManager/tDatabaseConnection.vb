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

        ''' <summary>
        ''' Adds a new Column to a Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overridable Sub addColumn(ByRef arColumn As RDS.Column)
        End Sub

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

        ''' <summary>
        ''' Removes the Column from its Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overridable Sub removeColumn(ByRef arColumn As RDS.Column)
        End Sub

        ''' <summary>
        ''' Renames the given Column to the new column name.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="asNewColumnName"></param>
        Public Overridable Sub renameColumn(ByRef arColumn As RDS.Column, ByVal asNewColumnName As String)
        End Sub

    End Class

End Namespace
