Namespace RDS

    <Serializable()> _
    Public Class DataType

        Public TypeName As String
        Public ProviderDBType As Integer
        Public ColumnSize As Integer
        Public CreateFormat As String = ""
        Public CreateParameters As String = ""
        Public DataType As String = ""
        Public IsAutoIncrementable As Boolean = False
        Public IsBestMatch As Boolean = False
        Public IsCaseSensitive As Boolean = False
        Public IsFixedLength As Boolean = False
        Public IsFixedPrecisionScale As Boolean = False
        Public IsLong As Boolean = False
        Public IsNullable As Boolean = False
        Public IsSearchable As Boolean = False
        Public IsSearchableWithLike As Boolean = False
        Public IsUnsigned As Boolean = False
        Public MaximumScale As Integer
        Public MinimumScale As Integer
        Public IsConcurrencyType As Boolean = False
        Public IsLiteralSupported As Boolean = False
        Public LiteralPrefix As String = ""
        Public LiteralSuffix As String = ""
        Public SQLtype As Integer

        ''' <summary>
        ''' Parameterless New.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Function EqualsByProviderDBType(ByVal other As RDS.DataType) As Boolean

            Return Me.ProviderDBType = other.ProviderDBType

        End Function

        Public Function EqualsBySQLType(ByVal other As RDS.DataType) As Boolean

            Return Me.SQLtype = other.SQLtype

        End Function

    End Class

End Namespace

