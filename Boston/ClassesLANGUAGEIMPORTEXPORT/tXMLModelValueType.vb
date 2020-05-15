Imports System.Xml.Serialization

Namespace XMLModel

    <Serializable()> _
    Partial Public Class ValueType

        Private _Id As String
        <XmlAttribute()> _
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        Private _Name As String
        <XmlAttribute()> _
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        Private _DataType As String
        <XmlAttribute()> _
        Public Property DataType() As String
            Get
                Return Me._DataType
            End Get
            Set(ByVal value As String)
                Me._DataType = value
            End Set
        End Property

        Private _DataTypePrecision As Integer
        Public Property DataTypePrecision() As Integer
            Get
                Return Me._DataTypePrecision
            End Get
            Set(ByVal value As Integer)
                Me._DataTypePrecision = value
            End Set
        End Property

        Private _DataTypeLength As Integer
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

    End Class

End Namespace
