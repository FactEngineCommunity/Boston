Imports System.Xml.Serialization

Namespace XMLModel12

    <Serializable()> _
    Public Class RoleConstraintArgument

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
        Private _SequenceNr As Integer = 1
        <XmlAttribute()> _
        Public Property SequenceNr As Integer
            Get
                Return Me._SequenceNr
            End Get
            Set(value As Integer)
                Me._SequenceNr = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Role As New List(Of XMLModel12.RoleReference)
        Public Property Role As List(Of XMLModel12.RoleReference)
            Get
                Return Me._Role
            End Get
            Set(value As List(Of XMLModel12.RoleReference))
                Me._Role = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _JoinPath As New XMLModel12.JoinPath
        Public Property JoinPath As XMLModel12.JoinPath
            Get
                Return Me._JoinPath
            End Get
            Set(value As XMLModel12.JoinPath)
                Me._JoinPath = value
            End Set
        End Property

    End Class

End Namespace
