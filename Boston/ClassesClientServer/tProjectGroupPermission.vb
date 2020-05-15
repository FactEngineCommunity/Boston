Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class ProjectGroupPermission

        Public Project As ClientServer.Project
        Public Group As ClientServer.Group
        Public Permission As pcenumPermission

    End Class

End Namespace