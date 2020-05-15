Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class RoleConstraintArgument

    Private _Id As String
    <DataMember()> _
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
    <DataMember()> _
    <XmlAttribute()> _
    Public Property SequenceNr As Integer
        Get
            Return Me._SequenceNr
        End Get
        Set(value As Integer)
            Me._SequenceNr = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RoleConstraintRole As New List(Of RoleConstraintRole)
    <DataMember()> _
    <XmlElement()> _
    Public Property RoleConstraintRole As List(Of RoleConstraintRole)
        Get
            Return Me._RoleConstraintRole
        End Get
        Set(value As List(Of RoleConstraintRole))
            Me._RoleConstraintRole = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _JoinPath As New JoinPath
    <DataMember()> _
    <XmlElement()> _
    Public Property JoinPath As JoinPath
        Get
            Return Me._JoinPath
        End Get
        Set(value As JoinPath)
            Me._JoinPath = value
        End Set
    End Property

End Class
