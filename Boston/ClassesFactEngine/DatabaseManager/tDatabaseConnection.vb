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

        ''' <summary>
        ''' Adds the referenced Index to the database. Table is within Index definition.
        ''' </summary>
        ''' <param name="arIndex">The Index to be added to the database.</param>
        Public Overridable Sub addIndex(ByRef arIndex As RDS.Index)
        End Sub

        ''' <summary>
        ''' Adds the given Relation/ForeignKey to the database. Relation holds relative Tables.
        ''' </summary>
        ''' <param name="arRelation"></param>
        Public Overridable Sub AddForeignKey(ByRef arRelation As RDS.Relation)
        End Sub

        ''' <summary>
        ''' Changes the data type of the nominated column.
        ''' </summary>
        ''' <param name="arColumn">The Column to have its data type changed.</param>
        ''' <param name="asDataType">The new data type.</param>
        ''' <param name="aiLength">The length of the data type. 0 is nothing.</param>
        ''' <param name="aiPrecision">The precision of the data type. 0 is nothing.</param>
        Public Overridable Sub columnChangeDatatype(ByRef arColumn As RDS.Column,
                                                    ByVal asDataType As pcenumORMDataType,
                                                    ByVal aiLength As Integer,
                                                    ByRef aiPrecision As Integer)
        End Sub

        ''' <summary>
        ''' Sets whether the specified Column is mandatory or not, in the database.
        ''' </summary>
        ''' <param name="arColumn">The Column to have its schema definition changed.</param>
        ''' <param name="abIsMandatory">True if the Column is mandatory for its Table.</param>
        Public Overridable Sub columnSetMandatory(ByRef arColumn As RDS.Column,
                                                  ByVal abIsMandatory As Boolean)
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
        ''' Some databases, like PostgreSQL use a date to string operator for use in LIKE clauses.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function dateToTextOperator() As String
            Return ""
        End Function

        Public Overridable Function FormatDateTime(ByVal asOriginalDate As String) As String
            Return ""
        End Function

        Public Overridable Function generateSQLColumnDefinition(ByRef arColumn As RDS.Column) As String
            Return ""
        End Function

        ''' <summary>
        ''' Generates a CREATE TABLE Statement for the given Table, specific to the database type.
        ''' </summary>
        ''' <param name="arTable">The RDS Table for which the SQL CREATE statement is to be generated.</param>
        ''' <param name="asTableName">Optional table name for the table in the CREATE statement.</param>
        ''' <returns></returns>
        Public Overridable Function generateSQLCREATETABLEStatement(ByRef arTable As RDS.Table,
                                                                     Optional asTableName As String = Nothing) As String
            Return ""
        End Function

        Public Overridable Function getColumnsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Column)
            Return New List(Of RDS.Column)
        End Function

        Public Overridable Sub getDatabaseTypes()
        End Sub

        Public Overridable Function getBostonDataTypeByDatabaseDataType(ByVal asDatabaseDataType As String) As pcenumORMDataType
            Return pcenumORMDataType.TextVariableLength
        End Function

        ''' <summary>
        ''' Returns a list of the Relations/ForeignKeys in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overridable Function getForeignKeyRelationshipsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)
            Return New List(Of RDS.Relation)
        End Function

        ''' <summary>
        ''' Returns a list of the Indexes in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overridable Function getIndexesByTable(ByRef arTable As RDS.Table) As List(Of RDS.Index)
            Return New List(Of RDS.Index)
        End Function

        ''' <summary>
        ''' Gets PK Index by other means if primary GetIndexesByTable doesn't return PK Indexes.
        '''   E.g. In SQLite you can create a Table with a PK and without an Index.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        Public Overridable Function getIndexesByTableByAlternateMeans(ByRef arTable As RDS.Table) As List(Of RDS.Index)
            Return New List(Of RDS.Index)
        End Function

        Public Overridable Function getRelationsByTable(ByRef arTable As RDS.Table) As List(Of RDS.Relation)
            Return New List(Of RDS.Relation)
        End Function

        ''' <summary>
        ''' Returns a list of the Tables in the database. As used in Reverse Engineering a database.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function getTables() As List(Of RDS.Table)
            Return New List(Of RDS.Table)
        End Function

        ''' <summary>
        ''' Adds the nominated Column to the nominated Index.
        ''' </summary>
        ''' <param name="arIndex">The Index to add the nominated Column to.</param>
        ''' <param name="arColumn">The Column to add to the nominated Index.</param>
        Public Overridable Sub IndexAddColumn(ByRef arIndex As RDS.Index, ByRef arColumn As RDS.Column)
        End Sub

        ''' <summary>
        ''' Updates the Index in the database. E.g. Changing a Unique Index to Primary Key.
        ''' </summary>
        ''' <param name="arIndex">The Index to be updated.</param>
        Public Overridable Sub IndexUpdate(ByRef arIndex As RDS.Index)
        End Sub

        ''' <summary>
        ''' Creates or Recreates the Table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overridable Sub recreateTable(ByRef arTable As RDS.Table)
        End Sub

        ''' <summary>
        ''' Removes the Column from its Table.
        ''' </summary>
        ''' <param name="arColumn"></param>
        Public Overridable Sub removeColumn(ByRef arColumn As RDS.Column)
        End Sub

        ''' <summary>
        ''' Removes/Drops the Table from the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        Public Overridable Sub removeTable(ByRef arTable As RDS.Table)
        End Sub

        ''' <summary>
        ''' Renames the given Column to the new column name.
        ''' </summary>
        ''' <param name="arColumn"></param>
        ''' <param name="asNewColumnName"></param>
        Public Overridable Sub renameColumn(ByRef arColumn As RDS.Column, ByVal asNewColumnName As String)
        End Sub

        ''' <summary>
        ''' Renames a table in the database.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <param name="asNewName"></param>
        Public Overridable Sub RenameTable(ByRef arTable As RDS.Table, ByVal asNewName As String)
        End Sub

        ''' <summary>
        ''' Returns True if a Table with the given name exists in the database, else returns False.
        ''' </summary>
        ''' <param name="asTableName"></param>
        ''' <returns></returns>
        Public Overridable Function TableExists(ByVal asTableName As String) As Boolean
        End Function

        ''' <summary>
        ''' Updates the value of a Column in the database.
        ''' </summary>
        ''' <param name="asTableName">The name of the Table for which the Attribute/Column value is to be updated.</param>
        ''' <param name="arColumn">The Column/Attribute for which the value is to be updated.</param>
        ''' <param name="asNewValue">The new value for the Attribute/Column.</param>
        ''' <param name="aarPKColumn">A list of the Primary Key Columns/Attributes for the record to be updated. TemporaryValue of Column is existing/old value of the Primary Key Column/Attribute.</param>
        Public Overridable Function UpdateAttributeValue(ByVal asTableName As String,
                                                         ByVal arColumn As RDS.Column,
                                                         ByVal asNewValue As String,
                                                         ByVal aarPKColumn As List(Of RDS.Column)) As ORMQL.Recordset
            Return New ORMQL.Recordset
        End Function

    End Class

End Namespace
