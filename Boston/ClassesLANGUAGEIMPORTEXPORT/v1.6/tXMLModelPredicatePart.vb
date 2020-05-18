Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class PredicatePart

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _SequenceNr As Integer
        <XmlAttribute()>
        Public Property SequenceNr() As Integer
            Get
                Return Me._SequenceNr
            End Get
            Set(ByVal value As Integer)
                Me._SequenceNr = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleId As String = ""
        <XmlAttribute()>
        Public Property Role_Id As String
            Get
                Return Me._RoleId
            End Get
            Set(ByVal value As String)
                Me._RoleId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _PreboundReadingText As String = ""
        <XmlAttribute()>
        Public Property PreboundReadingText As String
            Get
                Return Me._PreboundReadingText
            End Get
            Set(value As String)
                Me._PreboundReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _PostboundReadingText As String = ""
        <XmlAttribute()>
        Public Property PostboundReadingText As String
            Get
                Return Me._PostboundReadingText
            End Get
            Set(value As String)
                Me._PostboundReadingText = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _PredicatePartText As String
        Public Property PredicatePartText() As String
            Get
                Return Me._PredicatePartText
            End Get
            Set(ByVal value As String)
                Me._PredicatePartText = value
            End Set
        End Property

    End Class

End Namespace
