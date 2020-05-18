Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class ModelNote

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
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
        Private _JoinedObjectTypeId As String
        <XmlAttribute()>
        Public Property JoinedObjectTypeId() As String
            Get
                Return Me._JoinedObjectTypeId
            End Get
            Set(ByVal value As String)
                Me._JoinedObjectTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Note As String
        <XmlElement()>
        Public Property Note() As String
            Get
                Return Me._Note
            End Get
            Set(ByVal value As String)
                Me._Note = value
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

