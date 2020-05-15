Imports System.Xml.Serialization

Namespace XMLModel14
    <Serializable()> _
    Public Class RoleConstraintRole

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RoleId As String
        <XmlAttribute()> _
        Public Property RoleId() As String
            Get
                Return Me._RoleId
            End Get
            Set(ByVal value As String)
                Me._RoleId = value
            End Set
        End Property

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
        Private _IsEntry As Boolean
        <XmlAttribute()> _
        Public Property IsEntry() As Boolean
            Get
                Return Me._IsEntry
            End Get
            Set(ByVal value As Boolean)
                Me._IsEntry = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsExit As Boolean
        <XmlAttribute()> _
        Public Property IsExit() As Boolean
            Get
                Return Me._IsExit
            End Get
            Set(ByVal value As Boolean)
                Me._IsExit = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ArgumentId As String = ""
        <XmlAttribute()> _
        Public Property ArgumentId As String
            Get
                Return Me._ArgumentId
            End Get
            Set(ByVal value As String)
                Me._ArgumentId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ArgumentSequenceNr As Integer = 0
        <XmlAttribute()> _
        Public Property ArgumentSequenceNr As Integer
            Get
                Return Me._ArgumentSequenceNr
            End Get
            Set(ByVal value As Integer)
                Me._ArgumentSequenceNr = value
            End Set
        End Property

    End Class

End Namespace
