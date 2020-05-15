Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
    Public Class Role

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
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
    Private _Name As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Name() As String
        Get
            Return Me._Name
        End Get
        Set(ByVal value As String)
            Me._Name = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _SequenceNr As Integer = 1
    <DataMember()> _
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
    Private _Mandatory As Boolean = False
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Mandatory() As Boolean
        Get
            Return Me._Mandatory
        End Get
        Set(ByVal value As Boolean)
            Me._Mandatory = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _JoinedObjectTypeId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property JoinedObjectTypeId() As String
        Get
            Return Me._JoinedObjectTypeId
        End Get
        Set(ByVal value As String)
            Me._JoinedObjectTypeId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _ValueConstraint As New List(Of String)
    <DataMember()> _
    Public Property ValueConstraint() As List(Of String)
        Get
            Return Me._ValueConstraint
        End Get
        Set(ByVal value As List(Of String))
            Me._ValueConstraint = value
        End Set
    End Property

End Class
