Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class FactData

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

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Data As String
    <DataMember()> _
    Public Property Data() As String
        Get
            Return Me._Data
        End Get
        Set(ByVal value As String)
            Me._Data = value
        End Set
    End Property

End Class
