Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class ProjectUserRole

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _ProjectId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property ProjectId() As String
        Get
            Return Me._ProjectId
        End Get
        Set(ByVal value As String)
            Me._ProjectId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _UserId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property UserId() As String
        Get
            Return Me._UserId
        End Get
        Set(ByVal value As String)
            Me._UserId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RoleId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property RoleId() As String
        Get
            Return Me._RoleId
        End Get
        Set(ByVal value As String)
            Me._RoleId = value
        End Set
    End Property

End Class
