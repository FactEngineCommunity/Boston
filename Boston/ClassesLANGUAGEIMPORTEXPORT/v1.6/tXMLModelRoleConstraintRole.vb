Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class RoleConstraintRole

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleId As String
        <XmlAttribute()>
        Public Property RoleId() As String
            Get
                Return Me._RoleId
            End Get
            Set(ByVal value As String)
                Me._RoleId = value
            End Set
        End Property

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
        Private _IsEntry As Boolean
        <XmlAttribute()>
        Public Property IsEntry() As Boolean
            Get
                Return Me._IsEntry
            End Get
            Set(ByVal value As Boolean)
                Me._IsEntry = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsExit As Boolean
        <XmlAttribute()>
        Public Property IsExit() As Boolean
            Get
                Return Me._IsExit
            End Get
            Set(ByVal value As Boolean)
                Me._IsExit = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ArgumentId As String = ""
        <XmlAttribute()>
        Public Property ArgumentId As String
            Get
                Return Me._ArgumentId
            End Get
            Set(ByVal value As String)
                Me._ArgumentId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ArgumentSequenceNr As Integer = 0
        <XmlAttribute()>
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
