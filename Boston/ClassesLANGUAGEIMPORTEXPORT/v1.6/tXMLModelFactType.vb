Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class FactType
        Implements IEquatable(Of XMLModel16.FactType)

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
        Private _ObjectifyingEntityTypeId As String = ""
        <XmlAttribute()>
        Public Property ObjectifyingEntityTypeId() As String
            Get
                Return Me._ObjectifyingEntityTypeId
            End Get
            Set(ByVal value As String)
                Me._ObjectifyingEntityTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleGroup As New List(Of XMLModel16.Role)
        Public Property RoleGroup() As List(Of XMLModel16.Role)
            Get
                Return Me._RoleGroup
            End Get
            Set(ByVal value As List(Of XMLModel16.Role))
                Me._RoleGroup = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Facts As New List(Of XMLModel16.Fact)
        Public Property Facts() As List(Of XMLModel16.Fact)
            Get
                Return Me._Facts
            End Get
            Set(ByVal value As List(Of XMLModel16.Fact))
                Me._Facts = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _FactTypeReadings As New List(Of XMLModel16.FactTypeReading)
        Public Property FactTypeReadings() As List(Of XMLModel16.FactTypeReading)
            Get
                Return Me._FactTypeReadings
            End Get
            Set(ByVal value As List(Of XMLModel16.FactTypeReading))
                Me._FactTypeReadings = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsLinkFactType As Boolean
        <XmlAttribute()>
        Public Property IsLinkFactType As Boolean
            Get
                Return Me._IsLinkFactType
            End Get
            Set(ByVal value As Boolean)
                Me._IsLinkFactType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _LinkFactTypeRoleId As String
        <XmlAttribute()>
        Public Property LinkFactTypeRoleId As String
            Get
                Return Me._LinkFactTypeRoleId
            End Get
            Set(ByVal value As String)
                Me._LinkFactTypeRoleId = value
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
        Private _IsSubtypeRelationshipFactType As Boolean
        <XmlAttribute()>
        Public Property IsSubtypeRelationshipFactType() As Boolean
            Get
                Return Me._IsSubtypeRelationshipFactType
            End Get
            Set(ByVal value As Boolean)
                Me._IsSubtypeRelationshipFactType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsObjectified As Boolean
        <XmlAttribute()>
        Public Property IsObjectified() As Boolean
            Get
                Return Me._IsObjectified
            End Get
            Set(ByVal value As Boolean)
                Me._IsObjectified = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsPreferredReferenceSchemeFT As Boolean
        <XmlAttribute()>
        Public Property IsPreferredReferenceSchemeFT() As Boolean
            Get
                Return Me._IsPreferredReferenceSchemeFT
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredReferenceSchemeFT = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsDerived As Boolean
        <XmlAttribute()>
        Public Property IsDerived As Boolean
            Get
                Return Me._IsDerived
            End Get
            Set(ByVal value As Boolean)
                Me._IsDerived = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsStored As Boolean
        <XmlAttribute()>
        Public Property IsStored As Boolean
            Get
                Return Me._IsStored
            End Get
            Set(ByVal value As Boolean)
                Me._IsStored = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _DerivationText As String
        <XmlAttribute()>
        Public Property DerivationText As String
            Get
                Return Me._DerivationText
            End Get
            Set(ByVal value As String)
                Me._DerivationText = value
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

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsIndependent As Boolean
        <XmlAttribute()>
        Public Property IsIndependent As Boolean
            Get
                Return Me._IsIndependent
            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
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

End Namespace
