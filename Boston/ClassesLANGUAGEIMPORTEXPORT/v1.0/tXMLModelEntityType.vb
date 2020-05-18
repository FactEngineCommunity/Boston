Imports System.Xml.Serialization

Namespace XMLModel1

    <Serializable()>
    Public Class EntityType

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
        Private _Instance As New List(Of String)
        Public Property Instance() As List(Of String)
            Get
                Return Me._Instance
            End Get
            Set(ByVal value As List(Of String))
                Me._Instance = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ReferenceModeValueTypeId As String
        <XmlAttribute()>
        Public Property ReferenceModeValueTypeId() As String
            Get
                Return Me._ReferenceModeValueTypeId
            End Get
            Set(ByVal value As String)
                Me._ReferenceModeValueTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ReferenceSchemeRoleConstraintId As String = ""
        <XmlAttribute()>
        Public Property ReferenceSchemeRoleConstraintId() As String
            Get
                Return Me._ReferenceSchemeRoleConstraintId
            End Get
            Set(ByVal value As String)
                Me._ReferenceSchemeRoleConstraintId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsObjectifyingEntityType As Boolean
        <XmlAttribute()>
        Public Property IsObjectifyingEntityType() As Boolean
            Get
                Return Me._IsObjectifyingEntityType
            End Get
            Set(ByVal value As Boolean)
                Me._IsObjectifyingEntityType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ReferenceMode As String
        <XmlAttribute()>
        Public Property ReferenceMode() As String
            Get
                Return Me._ReferenceMode
            End Get
            Set(ByVal value As String)
                Me._ReferenceMode = value
            End Set
        End Property

        Private _SubtypeRelationships As New List(Of XMLModel1.SubtypeRelationship)
        Public Property SubtypeRelationships() As List(Of XMLModel1.SubtypeRelationship)
            Get
                Return Me._SubtypeRelationships
            End Get
            Set(ByVal value As List(Of XMLModel1.SubtypeRelationship))
                Me._SubtypeRelationships = value
            End Set
        End Property


    End Class

End Namespace