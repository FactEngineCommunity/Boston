Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class ProjectUserPermission

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
    Private _Permission As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Permission() As String
        Get
            Return Me._Permission
        End Get
        Set(ByVal value As String)
            Me._Permission = value
        End Set
    End Property

End Class
