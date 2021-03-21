Public Class DatabaseDataType

    Private _Database As pcenumDatabaseType
    Public Property Database As pcenumDatabaseType
        Get
            Return Me._Database
        End Get
        Set(value As pcenumDatabaseType)
            Me._Database = value
        End Set
    End Property


    Private _BostonDataType As pcenumORMDataType
    Public Property BostonDataType As pcenumORMDataType
        Get
            Return Me._BostonDataType
        End Get
        Set(value As pcenumORMDataType)
            Me._BostonDataType = value
        End Set
    End Property

    Private _DataType As String
    Public Property DataType As String
        Get
            Return Me._DataType
        End Get
        Set(value As String)
            Me._DataType = value
        End Set
    End Property

End Class
