Imports System.Xml.Serialization

Namespace XMLModelv081

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
        Private _ReferenceModeRoleConstraintId As String = ""
        <XmlAttribute()> _
        Public Property ReferenceModeRoleConstraintId() As String
            Get
                Return Me._ReferenceModeRoleConstraintId
            End Get
            Set(ByVal value As String)
                Me._ReferenceModeRoleConstraintId = value
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

        Private _SubtypeRelationships As New List(Of XMLModelv081.SubtypeRelationship)
        Public Property SubtypeRelationships() As List(Of XMLModelv081.SubtypeRelationship)
            Get
                Return Me._SubtypeRelationships
            End Get
            Set(ByVal value As List(Of XMLModelv081.SubtypeRelationship))
                Me._SubtypeRelationships = value
            End Set
        End Property


    End Class

End Namespace