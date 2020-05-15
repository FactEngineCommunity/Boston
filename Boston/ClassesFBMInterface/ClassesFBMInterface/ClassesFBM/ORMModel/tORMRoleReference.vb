Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Partial Public Class RoleReference

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
