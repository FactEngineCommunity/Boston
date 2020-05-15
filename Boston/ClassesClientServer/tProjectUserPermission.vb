Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class ProjectUserPermission

        Public Project As ClientServer.Project
        Public User As ClientServer.User
        Public Permission As pcenumPermission

    End Class

End Namespace
