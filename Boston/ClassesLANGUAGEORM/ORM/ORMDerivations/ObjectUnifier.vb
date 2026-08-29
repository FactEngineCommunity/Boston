Imports System.Xml.Serialization

Namespace FBM
    Public Class ObjectUnifier

        <XmlAttribute("id")>
        Public Property id As String

        ''' <summary>
        ''' PathedRole references that must resolve to the same object instance.
        ''' </summary>
        <XmlElement("PathedRole")>
        Public Property PathedRoles As New List(Of PathedRoleReference)

        ''' <summary>
        ''' Root object references that must resolve to the same object instance.
        ''' </summary>
        <XmlElement("PathRoot")>
        Public Property PathRoots As New List(Of PathRootReference)

    End Class


    ''' <summary>
    ''' Lightweight reference wrapper for PathedRole references inside ObjectUnifier.
    ''' </summary>
    Public Class PathedRoleReference

        ''' <summary>
        ''' Reference to a PathedRole id.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


End Namespace
