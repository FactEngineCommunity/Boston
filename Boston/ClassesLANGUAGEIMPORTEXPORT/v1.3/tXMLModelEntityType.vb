Imports System.Xml.Serialization

Namespace XMLModel13

    <Serializable()> _
    Public Class EntityType

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
        Private _GUID As String
        <XmlAttribute()> _
        Public Property GUID As String
            Get
                Return Me._GUID
            End Get
            Set(ByVal value As String)
                Me._GUID = value
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
        Private _Instance As New List(Of String)
        Public Property Instance() As List(Of String)
            Get
                Return Me._Instance
            End Get
            Set(ByVal value As List(Of String))
                Me._Instance = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ReferenceModeValueTypeId As String
        <XmlAttribute()> _
        Public Property ReferenceModeValueTypeId() As String
            Get
                Return Me._ReferenceModeValueTypeId
            End Get
            Set(ByVal value As String)
                Me._ReferenceModeValueTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ReferenceSchemeRoleConstraintId As String = ""
        <XmlAttribute()> _
        Public Property ReferenceSchemeRoleConstraintId() As String
            Get
                Return Me._ReferenceSchemeRoleConstraintId
            End Get
            Set(ByVal value As String)
                Me._ReferenceSchemeRoleConstraintId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsObjectifyingEntityType As Boolean
        <XmlAttribute()> _
        Public Property IsObjectifyingEntityType() As Boolean
            Get
                Return Me._IsObjectifyingEntityType
            End Get
            Set(ByVal value As Boolean)
                Me._IsObjectifyingEntityType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ReferenceMode As String
        <XmlAttribute()> _
        Public Property ReferenceMode() As String
            Get
                Return Me._ReferenceMode
            End Get
            Set(ByVal value As String)
                Me._ReferenceMode = value
            End Set
        End Property

        Private _SubtypeRelationships As New List(Of XMLModel13.SubtypeRelationship)
        Public Property SubtypeRelationships() As List(Of XMLModel13.SubtypeRelationship)
            Get
                Return Me._SubtypeRelationships
            End Get
            Set(ByVal value As List(Of XMLModel13.SubtypeRelationship))
                Me._SubtypeRelationships = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsIndependent As Boolean
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
        Private _IsPersonal As Boolean
        <XmlAttribute()> _
        Public Property IsPersonal As Boolean
            Get
                Return Me._IsPersonal
            End Get
            Set(ByVal value As Boolean)
                Me._IsPersonal = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _LongDescription As String = ""
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
        Private _IsAbsorbed As Boolean
        <XmlAttribute()>
        Public Property IsAbsorbed As Boolean
            Get
                Return Me._IsAbsorbed
            End Get
            Set(ByVal value As Boolean)
                Me._IsAbsorbed = value
            End Set
        End Property


    End Class

End Namespace