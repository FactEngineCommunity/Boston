Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class RoleConstraint

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Id As String
        <XmlAttribute()>
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _GUID As String
        <XmlAttribute()>
        Public Property GUID As String
            Get
                Return Me._GUID
            End Get
            Set(ByVal value As String)
                Me._GUID = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Name As String
        <XmlAttribute()>
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleConstraintType As String
        <XmlAttribute()>
        Public Property RoleConstraintType() As String
            Get
                Return Me._RoleConstraintType
            End Get
            Set(ByVal value As String)
                Me._RoleConstraintType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RingConstraintType As String
        <XmlAttribute()>
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

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsPreferredUniqueness As Boolean
        <XmlAttribute()>
        Public Property IsPreferredUniqueness() As Boolean
            Get
                Return Me._IsPreferredUniqueness
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredUniqueness = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsDeontic As Boolean
        <XmlAttribute()>
        Public Property IsDeontic() As Boolean
            Get
                Return Me._IsDeontic
            End Get
            Set(ByVal value As Boolean)
                Me._IsDeontic = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsMDAModelElement As Boolean
        <XmlAttribute()>
        Public Property IsMDAModelElement As Boolean
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MinimumFrequencyCount As Integer
        <XmlAttribute()>
        Public Property MinimumFrequencyCount() As Integer
            Get
                Return Me._MinimumFrequencyCount
            End Get
            Set(ByVal value As Integer)
                Me._MinimumFrequencyCount = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MaximumFrequencyCount As Integer
        <XmlAttribute()>
        Public Property MaximumFrequencyCount() As Integer
            Get
                Return Me._MaximumFrequencyCount
            End Get
            Set(ByVal value As Integer)
                Me._MaximumFrequencyCount = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Cardinality As Integer
        <XmlAttribute()>
        Public Property Cardinality() As Integer
            Get
                Return Me._Cardinality
            End Get
            Set(ByVal value As Integer)
                Me._Cardinality = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _CardinalityRangeType As String
        <XmlAttribute()>
        Public Property CardinalityRangeType() As String
            Get
                Return Me._CardinalityRangeType
            End Get
            Set(ByVal value As String)
                Me._CardinalityRangeType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleConstraintRoles As New List(Of XMLModel16.RoleConstraintRole)
        Public Property RoleConstraintRoles() As List(Of XMLModel16.RoleConstraintRole)
            Get
                Return Me._RoleConstraintRoles
            End Get
            Set(ByVal value As List(Of XMLModel16.RoleConstraintRole))
                Me._RoleConstraintRoles = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Argument As New List(Of XMLModel16.RoleConstraintArgument)
        Public Property Argument As List(Of XMLModel16.RoleConstraintArgument)
            Get
                Return Me._Argument
            End Get
            Set(value As List(Of XMLModel16.RoleConstraintArgument))
                Me._Argument = value
            End Set
        End Property


        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _LongDescription As String = ""
        <XmlAttribute()>
        Public Property LongDescription() As String
            Get
                Return Me._LongDescription
            End Get
            Set(ByVal value As String)
                Me._LongDescription = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ShortDescription As String = ""
        <XmlAttribute()>
        Public Property ShortDescription() As String
            Get
                Return Me._ShortDescription
            End Get
            Set(ByVal value As String)
                Me._ShortDescription = value
            End Set
        End Property


    End Class

End Namespace
