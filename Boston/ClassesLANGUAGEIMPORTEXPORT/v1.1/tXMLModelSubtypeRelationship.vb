Imports System.Xml.Serialization

Namespace XMLModel11
    <Serializable()>
    Public Class SubtypeRelationship

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ParentEntityTypeId As String
        <XmlAttribute()>
        Public Property ParentEntityTypeId() As String
            Get
                Return Me._ParentEntityTypeId
            End Get
            Set(ByVal value As String)
                Me._ParentEntityTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _SubtypingFactTypeId As String
        <XmlAttribute()>
        Public Property SubtypingFactTypeId() As String
            Get
                Return Me._SubtypingFactTypeId
            End Get
            Set(ByVal value As String)
                Me._SubtypingFactTypeId = value
            End Set
        End Property

    End Class

End Namespace
