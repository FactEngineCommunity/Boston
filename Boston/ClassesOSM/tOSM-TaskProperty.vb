Imports System.Xml.Serialization
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Reflection


Namespace OSM

    <Serializable>
    Public Class TaskProperty

        <XmlIgnore>
        <NonSerialized>
        Private _TaskId As String = System.Guid.NewGuid.ToString
        <XmlAttribute>
        Public Property TaskId As String
            Get
                Return Me._TaskId
            End Get
            Set(value As String)
                Me._TaskId = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _OrdinalPosition As String
        <XmlAttribute>
        Public Property OrdinalPosition As String
            Get
                Return Me._OrdinalPosition
            End Get
            Set(value As String)
                Me._OrdinalPosition = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _Name As String
        <XmlAttribute>
        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _DataType As String
        <XmlAttribute>
        Public Property DataType As String
            Get
                Return Me._DataType
            End Get
            Set(value As String)
                Me._DataType = value
            End Set
        End Property

        <XmlIgnore>
        <NonSerialized>
        Private _Description As String = System.Guid.NewGuid.ToString
        <XmlAttribute>
        Public Property Description As String
            Get
                Return Me._Description
            End Get
            Set(value As String)
                Me._Description = value
            End Set
        End Property

    End Class

End Namespace
