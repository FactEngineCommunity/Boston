Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
    Public Class PredicatePart

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _SequenceNr As Integer
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
    Private _RoleId As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Role_Id As String
        Get
            Return Me._RoleId
        End Get
        Set(ByVal value As String)
            Me._RoleId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _PreboundReadingText As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property PreboundReadingText As String
        Get
            Return Me._PreboundReadingText
        End Get
        Set(value As String)
            Me._PreboundReadingText = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _PostboundReadingText As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property PostboundReadingText As String
        Get
            Return Me._PostboundReadingText
        End Get
        Set(value As String)
            Me._PostboundReadingText = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _PredicatePartText As String
    <DataMember()> _
    Public Property PredicatePartText() As String
        Get
            Return Me._PredicatePartText
        End Get
        Set(ByVal value As String)
            Me._PredicatePartText = value
        End Set
    End Property

End Class
