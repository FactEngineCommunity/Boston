Imports System.Xml.Serialization

Namespace XMLModelv1
    <Serializable()> _
    Public Class PredicatePart

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _SequenceNr As Integer
        <XmlAttribute()> _
        Public Property SequenceNr() As Integer
            Get
                Return Me._SequenceNr
            End Get
            Set(ByVal value As Integer)
                Me._SequenceNr = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ObjectType1Id As String
        <XmlAttribute()> _
        Public Property ObjectType1Id() As String
            Get
                Return Me._ObjectType1Id
            End Get
            Set(ByVal value As String)
                Me._ObjectType1Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ObjectType2Id As String
        <XmlAttribute()> _
        Public Property ObjectType2Id() As String
            Get
                Return Me._ObjectType2Id
            End Get
            Set(ByVal value As String)
                Me._ObjectType2Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
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
