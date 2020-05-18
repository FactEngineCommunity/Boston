Imports System.Xml.Serialization

Namespace XMLModel16

    <Serializable()>
    Partial Public Class RoleReference

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

    End Class

End Namespace
