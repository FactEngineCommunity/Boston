Imports System.Xml.Serialization

Namespace XMLModelv081
    <Serializable()> _
    Public Class FactType
        Implements IEquatable(Of XMLModelv081.FactType)

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
        Private _ObjectifyingEntityTypeId As String = ""
        <XmlAttribute()> _
        Public Property ObjectifyingEntityTypeId() As String
            Get
                Return Me._ObjectifyingEntityTypeId
            End Get
            Set(ByVal value As String)
                Me._ObjectifyingEntityTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RoleGroup As New List(Of XMLModelv081.Role)
        Public Property RoleGroup() As List(Of XMLModelv081.Role)
            Get
                Return Me._RoleGroup
            End Get
            Set(ByVal value As List(Of XMLModelv081.Role))
                Me._RoleGroup = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Facts As New List(Of XMLModelv081.Fact)
        Public Property Facts() As List(Of XMLModelv081.Fact)
            Get
                Return Me._Facts
            End Get
            Set(ByVal value As List(Of XMLModelv081.Fact))
                Me._Facts = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _FactTypeReadings As New List(Of XMLModelv081.FactTypeReading)
        Public Property FactTypeReadings() As List(Of XMLModelv081.FactTypeReading)
            Get
                Return Me._FactTypeReadings
            End Get
            Set(ByVal value As List(Of XMLModelv081.FactTypeReading))
                Me._FactTypeReadings = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsSubtypeRelationshipFactType As Boolean
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
        Private _IsPreferredReferenceModeFT As Boolean
        <XmlAttribute()> _
        Public Property IsPreferredReferenceModeFT() As Boolean
            Get
                Return Me._IsPreferredReferenceModeFT
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredReferenceModeFT = value
            End Set
        End Property

        Public Shadows Function Equals(ByVal other As FactType) As Boolean Implements System.IEquatable(Of XMLModelv081.FactType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

End Namespace
