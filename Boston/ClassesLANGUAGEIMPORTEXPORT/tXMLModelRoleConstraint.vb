Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class RoleConstraint

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Id As String
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
        Private _RoleConstraintType As String
        <XmlAttribute()> _
        Public Property RoleConstraintType() As String
            Get
                Return Me._RoleConstraintType
            End Get
            Set(ByVal value As String)
                Me._RoleConstraintType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RingConstraintType As String
        <XmlAttribute()> _
        Public Property RingConstraintType() As String
            Get
                Return Me._RingConstraintType
            End Get
            Set(ByVal value As String)
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
        Private _IsPreferredUniqueness As Boolean
        <XmlAttribute()> _
        Public Property IsPreferredUniqueness() As Boolean
            Get
                Return Me._IsPreferredUniqueness
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredUniqueness = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsDeontic As Boolean
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
        Private _CardinalityRangeType As String
        <XmlAttribute()> _
        Public Property CardinalityRangeType() As String
            Get
                Return Me._CardinalityRangeType
            End Get
            Set(ByVal value As String)
                Me._CardinalityRangeType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RoleConstraintRoles As New List(Of XMLModel.RoleConstraintRole)
        Public Property RoleConstraintRoles() As List(Of XMLModel.RoleConstraintRole)
            Get
                Return Me._RoleConstraintRoles
            End Get
            Set(ByVal value As List(Of XMLModel.RoleConstraintRole))
                Me._RoleConstraintRoles = value
            End Set
        End Property


    End Class

End Namespace
