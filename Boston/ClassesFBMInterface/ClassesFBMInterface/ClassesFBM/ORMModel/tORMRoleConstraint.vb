Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
<Serializable()> _
    Public Class RoleConstraint

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
    Private _RoleConstraintType As pcenumRoleConstraintType
    <DataMember()> _
    <XmlAttribute()> _
    Public Property RoleConstraintType As pcenumRoleConstraintType
        Get
            Return Me._RoleConstraintType
        End Get
        Set(ByVal value As pcenumRoleConstraintType)
            Me._RoleConstraintType = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RingConstraintType As pcenumRingConstraintType
    <DataMember()> _
    <XmlAttribute()> _
    Public Property RingConstraintType As pcenumRingConstraintType
        Get
            Return Me._RingConstraintType
        End Get
        Set(ByVal value As pcenumRingConstraintType)
            Me._RingConstraintType = value
        End Set
    End Property

    '<DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    'Private _LevelNr As Integer
    '<XmlAttribute()> _
    'Public Property LevelNr() As Integer
    '    Get
    '        Return Me._LevelNr
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me._LevelNr = value
    '    End Set
    'End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _MDAModelElement As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsMDAModelElement As Boolean
        Get
            Return Me._MDAModelElement
        End Get
        Set(ByVal value As Boolean)
            Me._MDAModelElement = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsPreferredIdentifier As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsPreferredIdentifier() As Boolean
        Get
            Return Me._IsPreferredIdentifier
        End Get
        Set(ByVal value As Boolean)
            Me._IsPreferredIdentifier = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _IsDeontic As Boolean
    <DataMember()> _
    <XmlAttribute()> _
    Public Property IsDeontic() As Boolean
        Get
            Return Me._IsDeontic
        End Get
        Set(ByVal value As Boolean)
            Me._IsDeontic = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _MinimumFrequencyCount As Integer
    <DataMember()> _
    <XmlAttribute()> _
    Public Property MinimumFrequencyCount() As Integer
        Get
            Return Me._MinimumFrequencyCount
        End Get
        Set(ByVal value As Integer)
            Me._MinimumFrequencyCount = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _MaximumFrequencyCount As Integer
    <DataMember()> _
    <XmlAttribute()> _
    Public Property MaximumFrequencyCount() As Integer
        Get
            Return Me._MaximumFrequencyCount
        End Get
        Set(ByVal value As Integer)
            Me._MaximumFrequencyCount = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Cardinality As Integer
    <DataMember()> _
    <XmlAttribute()> _
    Public Property Cardinality() As Integer
        Get
            Return Me._Cardinality
        End Get
        Set(ByVal value As Integer)
            Me._Cardinality = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _CardinalityRangeType As pcenumCardinalityRangeType
    <DataMember()>
    <XmlAttribute()>
    Public Property CardinalityRangeType As pcenumCardinalityRangeType
        Get
            Return Me._CardinalityRangeType
        End Get
        Set(ByVal value As pcenumCardinalityRangeType)
            Me._CardinalityRangeType = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)>
    Private _ValueRangeType As pcenumValueRangeType
    <DataMember()>
    <XmlAttribute()>
    Public Property ValueRangeType As pcenumValueRangeType
        Get
            Return Me._ValueRangeType
        End Get
        Set(ByVal value As pcenumValueRangeType)
            Me._ValueRangeType = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _RoleConstraintRoles As New List(Of RoleConstraintRole)
    <DataMember()> _
    <XmlElement()> _
    Public Property RoleConstraintRoles() As List(Of RoleConstraintRole)
        Get
            Return Me._RoleConstraintRoles
        End Get
        Set(ByVal value As List(Of RoleConstraintRole))
            Me._RoleConstraintRoles = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _Argument As New List(Of RoleConstraintArgument)
    <DataMember()> _
    <XmlElement()> _
    Public Property Argument As List(Of RoleConstraintArgument)
        Get
            Return Me._Argument
        End Get
        Set(value As List(Of RoleConstraintArgument))
            Me._Argument = value
        End Set
    End Property

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private _LongDescription As String = ""
    <XmlAttribute()> _
    <DataMember()> _
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
    <XmlAttribute()> _
    <DataMember()> _
    Public Property ShortDescription() As String
        Get
            Return Me._ShortDescription
        End Get
        Set(ByVal value As String)
            Me._ShortDescription = value
        End Set
    End Property

End Class
