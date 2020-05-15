Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class GroupUser

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

End Class

