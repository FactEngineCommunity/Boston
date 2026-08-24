Public Module PublicConstantsODBC

    <Serializable()> _
    Public Enum pcenumODBCAscendingOrDescending
        Ascending
        Descending
    End Enum

    Public Enum pcenumODBCIndexType
        Unknown = 0
        PrimaryKey = 1
        Unique = 2
    End Enum

    Public Enum pcenumODBCTableType
        Table
        SystemTable
    End Enum

End Module
