Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class ProjectGroupPermission

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
    Private _GroupId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property GroupId() As String
        Get
            Return Me._GroupId
        End Get
        Set(ByVal value As String)
            Me._GroupId = value
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

