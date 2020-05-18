Imports System.Xml.Serialization

Namespace XMLModel16

    <Serializable()>
    Public Class RoleConstraintArgument

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
        Private _SequenceNr As Integer = 1
        <XmlAttribute()>
        Public Property SequenceNr As Integer
            Get
                Return Me._SequenceNr
            End Get
            Set(value As Integer)
                Me._SequenceNr = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Role As New List(Of XMLModel16.RoleReference)
        Public Property Role As List(Of XMLModel16.RoleReference)
            Get
                Return Me._Role
            End Get
            Set(value As List(Of XMLModel16.RoleReference))
                Me._Role = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _JoinPath As New XMLModel16.JoinPath
        Public Property JoinPath As XMLModel16.JoinPath
            Get
                Return Me._JoinPath
            End Get
            Set(value As XMLModel16.JoinPath)
                Me._JoinPath = value
            End Set
        End Property

    End Class

End Namespace
