Imports System.Xml.Serialization

Namespace XMLModel11
    <Serializable()>
    Public Class FactType
        Implements IEquatable(Of XMLModel11.FactType)

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
        Private _RoleGroup As New List(Of XMLModel11.Role)
        Public Property RoleGroup() As List(Of XMLModel11.Role)
            Get
                Return Me._RoleGroup
            End Get
            Set(ByVal value As List(Of XMLModel11.Role))
                Me._RoleGroup = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Facts As New List(Of XMLModel11.Fact)
        Public Property Facts() As List(Of XMLModel11.Fact)
            Get
                Return Me._Facts
            End Get
            Set(ByVal value As List(Of XMLModel11.Fact))
                Me._Facts = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _FactTypeReadings As New List(Of XMLModel11.FactTypeReading)
        Public Property FactTypeReadings() As List(Of XMLModel11.FactTypeReading)
            Get
                Return Me._FactTypeReadings
            End Get
            Set(ByVal value As List(Of XMLModel11.FactTypeReading))
                Me._FactTypeReadings = value
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

        Public Shadows Function Equals(ByVal other As FactType) As Boolean Implements System.IEquatable(Of FactType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

End Namespace
