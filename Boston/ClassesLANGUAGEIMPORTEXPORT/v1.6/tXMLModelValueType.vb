Imports System.Xml.Serialization

Namespace XMLModel16

    <Serializable()>
    Partial Public Class ValueType

        Private _Id As String
        <XmlAttribute()>
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _GUID As String
        <XmlAttribute()>
        Public Property GUID As String
            Get
                Return Me._GUID
            End Get
            Set(ByVal value As String)
                Me._GUID = value
            End Set
        End Property

        Private _Name As String
        <XmlAttribute()>
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Private _DataType As pcenumORMDataType
        <XmlAttribute()>
        Public Property DataType() As pcenumORMDataType
            Get
                Return Me._DataType
            End Get
            Set(ByVal value As pcenumORMDataType)
                Me._DataType = value
            End Set
        End Property

        Private _DataTypePrecision As Integer
        <XmlAttribute()>
        Public Property DataTypePrecision() As Integer
            Get
                Return Me._DataTypePrecision
            End Get
            Set(ByVal value As Integer)
                Me._DataTypePrecision = value
            End Set
        End Property

        Private _DataTypeLength As Integer
        <XmlAttribute()>
        Public Property DataTypeLength() As Integer
            Get
                Return Me._DataTypeLength
            End Get
            Set(ByVal value As Integer)
                Me._DataTypeLength = value
            End Set
        End Property

        Private _Instance As New List(Of String)
        Public Property Instance() As List(Of String)
            Get
                Return Me._Instance
            End Get
            Set(ByVal value As List(Of String))
                Me._Instance = value
            End Set
        End Property

        Private _ValueConstraint As New List(Of String)
        Public Property ValueConstraint() As List(Of String)
            Get
                Return Me._ValueConstraint
            End Get
            Set(ByVal value As List(Of String))
                Me._ValueConstraint = value
            End Set
        End Property


        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _LongDescription As String = ""
        <XmlAttribute()>
        Public Property LongDescription() As String
            Get
                Return Me._LongDescription
            End Get
            Set(ByVal value As String)
                Me._LongDescription = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ShortDescription As String = ""
        <XmlAttribute()>
        Public Property ShortDescription() As String
            Get
                Return Me._ShortDescription
            End Get
            Set(ByVal value As String)
                Me._ShortDescription = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsIndependent As Boolean
        <XmlAttribute()>
        Public Property IsIndependent As Boolean
            Get
                Return Me._IsIndependent
            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsMDAModelElement As Boolean
        <XmlAttribute()>
        Public Property IsMDAModelElement As Boolean
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

    End Class

End Namespace
