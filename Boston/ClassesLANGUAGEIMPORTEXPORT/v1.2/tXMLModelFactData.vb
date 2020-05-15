Imports System.Xml.Serialization

Namespace XMLModel12
    <Serializable()> _
    Public Class FactData

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RoleId As String
        <XmlAttribute()> _
        Public Property RoleId() As String
            Get
                Return Me._RoleId
            End Get
            Set(ByVal value As String)
                Me._RoleId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
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
