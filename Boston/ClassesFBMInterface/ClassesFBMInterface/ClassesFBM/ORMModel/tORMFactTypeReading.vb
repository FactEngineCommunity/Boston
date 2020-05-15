Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
Public Class FactTypeReading

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
    Private _FrontReadingText As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property FrontReadingText As String
        Get
            Return Me._FrontReadingText
        End Get
        Set(value As String)
            Me._FrontReadingText = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _FollowingReadingText As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property FollowingReadingText As String
        Get
            Return Me._FollowingReadingText
        End Get
        Set(value As String)
            Me._FollowingReadingText = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _PredicatePart As New List(Of PredicatePart)
    <DataMember()> _
    Public Property PredicatePart() As List(Of PredicatePart)
        Get
            Return Me._PredicatePart
        End Get
        Set(ByVal value As List(Of PredicatePart))
            Me._PredicatePart = value
        End Set
    End Property

End Class
