Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class ModelNote

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
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

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _JoinedObjectTypeId As String
        <XmlAttribute()> _
        Public Property JoinedObjectTypeId() As String
            Get
                Return Me._JoinedObjectTypeId
            End Get
            Set(ByVal value As String)
                Me._JoinedObjectTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Note As String
        <XmlElement()> _
        Public Property Note() As String
            Get
                Return Me._Note
            End Get
            Set(ByVal value As String)
                Me._Note = value
            End Set
        End Property


    End Class

End Namespace

