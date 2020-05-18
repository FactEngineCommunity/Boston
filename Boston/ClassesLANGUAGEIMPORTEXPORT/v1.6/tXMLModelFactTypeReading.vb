Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class FactTypeReading

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
        Private _FrontReadingText As String = ""
        <XmlAttribute()>
        Public Property FrontReadingText As String
            Get
                Return Me._FrontReadingText
            End Get
            Set(value As String)
                Me._FrontReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _FollowingReadingText As String = ""
        <XmlAttribute()>
        Public Property FollowingReadingText As String
            Get
                Return Me._FollowingReadingText
            End Get
            Set(value As String)
                Me._FollowingReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _PredicateParts As New List(Of XMLModel16.PredicatePart)
        Public Property PredicateParts() As List(Of XMLModel16.PredicatePart)
            Get
                Return Me._PredicateParts
            End Get
            Set(ByVal value As List(Of XMLModel16.PredicatePart))
                Me._PredicateParts = value
            End Set
        End Property

    End Class

End Namespace
