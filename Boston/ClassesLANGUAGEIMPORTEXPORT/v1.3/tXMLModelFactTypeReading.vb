Imports System.Xml.Serialization

Namespace XMLModel13
    <Serializable()> _
    Public Class FactTypeReading

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
        Private _FrontReadingText As String = ""
        <XmlAttribute()> _
        Public Property FrontReadingText As String
            Get
                Return Me._FrontReadingText
            End Get
            Set(value As String)
                Me._FrontReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _FollowingReadingText As String = ""
        <XmlAttribute()> _
        Public Property FollowingReadingText As String
            Get
                Return Me._FollowingReadingText
            End Get
            Set(value As String)
                Me._FollowingReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _PredicateParts As New List(Of XMLModel.PredicatePart)
        Public Property PredicateParts() As List(Of XMLModel.PredicatePart)
            Get
                Return Me._PredicateParts
            End Get
            Set(ByVal value As List(Of XMLModel.PredicatePart))
                Me._PredicateParts = value
            End Set
        End Property

    End Class

End Namespace
