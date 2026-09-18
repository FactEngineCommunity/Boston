Imports System.Xml.Serialization

Namespace XMLModel

    <Serializable()>
    Partial Public Class [Alias]

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _AliasType As pcenumORMAliasType
        <XmlAttribute()>
        Public Property AliasType As pcenumORMAliasType
            Get
                Return Me._AliasType
            End Get
            Set(ByVal value As pcenumORMAliasType)
                Me._AliasType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Alias As String
        <XmlAttribute()>
        Public Property Aliaas As String
            Get
                Return Me._Alias
            End Get
            Set(ByVal value As String)
                Me._Alias = value
            End Set
        End Property


    End Class

End Namespace
