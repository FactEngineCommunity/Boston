Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class ProjectNamespace

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
    Private _NamespaceId As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property NamespaceId() As String
        Get
            Return Me._NamespaceId
        End Get
        Set(ByVal value As String)
            Me._NamespaceId = value
        End Set
    End Property

End Class


