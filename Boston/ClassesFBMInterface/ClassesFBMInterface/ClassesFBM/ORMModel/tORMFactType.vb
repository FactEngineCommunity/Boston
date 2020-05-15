Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
    Public Class FactType
    Implements IEquatable(Of FactType)

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
    Private _Name As String
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Name() As String
        Get
            Return Me._Name
        End Get
        Set(ByVal value As String)
            Me._Name = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _ObjectifyingEntityTypeId As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property ObjectifyingEntityTypeId As String
        Get
            Return Me._ObjectifyingEntityTypeId
        End Get
        Set(ByVal value As String)
            Me._ObjectifyingEntityTypeId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RoleGroup As New List(Of Role)
    <DataMember()> _
    Public Property RoleGroup() As List(Of Role)
        Get
            Return Me._RoleGroup
        End Get
        Set(ByVal value As List(Of Role))
            Me._RoleGroup = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Facts As New List(Of Fact)
    <DataMember()> _
    Public Property Facts() As List(Of Fact)
        Get
            Return Me._Facts
        End Get
        Set(ByVal value As List(Of Fact))
            Me._Facts = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _FactTypeReadings As New List(Of FactTypeReading)
    <DataMember()> _
    Public Property FactTypeReadings() As List(Of FactTypeReading)
        Get
            Return Me._FactTypeReadings
        End Get
        Set(ByVal value As List(Of FactTypeReading))
            Me._FactTypeReadings = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsSubtypeRelationshipFactType As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsSubtypeRelationshipFactType() As Boolean
        Get
            Return Me._IsSubtypeRelationshipFactType
        End Get
        Set(ByVal value As Boolean)
            Me._IsSubtypeRelationshipFactType = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsObjectified As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsObjectified() As Boolean
        Get
            Return Me._IsObjectified
        End Get
        Set(ByVal value As Boolean)
            Me._IsObjectified = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsPreferredReferenceSchemeFT As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsPreferredReferenceSchemeFT() As Boolean
        Get
            Return Me._IsPreferredReferenceSchemeFT
        End Get
        Set(ByVal value As Boolean)
            Me._IsPreferredReferenceSchemeFT = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsIndependent As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsIndependent As Boolean
        Get
            Return Me._IsIndependent
        End Get
        Set(ByVal value As Boolean)
            Me._IsIndependent = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsLinkFactType As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsLinkFactType As Boolean
        Get
            Return Me._IsLinkFactType
        End Get
        Set(ByVal value As Boolean)
            Me._IsLinkFactType = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _LinkFactTypeRoleId As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property LinkFactTypeRoleId As String
        Get
            Return Me._LinkFactTypeRoleId
        End Get
        Set(ByVal value As String)
            Me._LinkFactTypeRoleId = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsMDAModelElement As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsMDAModelElement As Boolean
        Get
            Return Me._IsMDAModelElement
        End Get
        Set(ByVal value As Boolean)
            Me._IsMDAModelElement = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _ShowFactTypeName As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property ShowFactTypeName As Boolean
        Get
            Return Me._IsObjectified
            'Return Me._ShowFactTypeName
        End Get
        Set(ByVal value As Boolean)
            Me._ShowFactTypeName = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsDerived As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsDerived As Boolean
        Get
            Return Me._IsDerived
        End Get
        Set(ByVal value As Boolean)
            Me._IsDerived = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsStored As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsStored As Boolean
        Get
            Return Me._IsStored
        End Get
        Set(ByVal value As Boolean)
            Me._IsStored = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _DerivationText As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property DerivationText As String
        Get
            Return Me._DerivationText
        End Get
        Set(ByVal value As String)
            Me._DerivationText = value
        End Set
    End Property


    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _LongDescription As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property LongDescription() As String
        Get
            Return Me._LongDescription
        End Get
        Set(ByVal value As String)
            Me._LongDescription = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _ShortDescription As String = ""
    <DataMember()> _
    <XmlAttribute()> _
    Public Property ShortDescription() As String
        Get
            Return Me._ShortDescription
        End Get
        Set(ByVal value As String)
            Me._ShortDescription = value
        End Set
    End Property

    Public Shadows Function Equals(ByVal other As FactType) As Boolean Implements System.IEquatable(Of FactType).Equals

        If Me.Id = other.Id Then
            Return True
        Else
            Return False
        End If

    End Function
End Class
