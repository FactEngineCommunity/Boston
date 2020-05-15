Imports System.Xml.Serialization

Namespace RDS

    <Serializable()> _
    Public Class DataType

        <XmlAttribute()> _
        Public TypeName As String

        <XmlAttribute()> _
        Public ProviderDBType As Integer

        <XmlAttribute()> _
        Public ColumnSize As Integer

        <XmlAttribute()> _
        Public CreateFormat As String = ""

        <XmlAttribute()> _
        Public CreateParameters As String = ""

        <XmlAttribute()> _
        Public DataType As String = ""

        <XmlAttribute()> _
        Public IsAutoIncrementable As Boolean = False

        <XmlAttribute()> _
        Public IsBestMatch As Boolean = False

        <XmlAttribute()> _
        Public IsCaseSensitive As Boolean = False

        <XmlAttribute()> _
        Public IsFixedLength As Boolean = False

        <XmlAttribute()> _
        Public IsFixedPrecisionScale As Boolean = False

        <XmlAttribute()> _
        Public IsLong As Boolean = False

        <XmlAttribute()> _
        Public IsNullable As Boolean = False

        <XmlAttribute()> _
        Public IsSearchable As Boolean = False

        <XmlAttribute()> _
        Public IsSearchableWithLike As Boolean = False

        <XmlAttribute()> _
        Public IsUnsigned As Boolean = False

        <XmlAttribute()> _
        Public MaximumScale As Integer

        <XmlAttribute()> _
        Public MinimumScale As Integer

        <XmlAttribute()> _
        Public IsConcurrencyType As Boolean = False

        <XmlAttribute()> _
        Public IsLiteralSupported As Boolean = False

        <XmlAttribute()> _
        Public LiteralPrefix As String = ""

        <XmlAttribute()> _
        Public LiteralSuffix As String = ""

        <XmlAttribute()> _
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
