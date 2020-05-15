Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class Fact

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
    Private _Data As New List(Of FactData)
    <DataMember()> _
    Public Property Data() As List(Of FactData)
        Get
            Return Me._Data
        End Get
        Set(ByVal value As List(Of FactData))
            Me._Data = value
        End Set
    End Property

End Class


