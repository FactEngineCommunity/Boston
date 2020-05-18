Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class FactData

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _RoleId As String
        <XmlAttribute()>
        Public Property RoleId() As String
            Get
                Return Me._RoleId
            End Get
            Set(ByVal value As String)
                Me._RoleId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Data As String
        Public Property Data() As String
            Get
                Return Me._Data
            End Get
            Set(ByVal value As String)
                Me._Data = value
            End Set
        End Property

    End Class

End Namespace
