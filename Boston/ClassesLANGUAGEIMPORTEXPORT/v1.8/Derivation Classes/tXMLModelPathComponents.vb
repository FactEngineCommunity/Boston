Imports System.Xml.Serialization

Namespace XMLModel

    ''' <summary>
    ''' Wrapper for the set of path components that make up a derivation.
    '''
    ''' In practice this minimal importer only needs RolePath.
    ''' NORMA may extend PathComponents in future schema versions; this wrapper keeps
    ''' deserialization stable.
    ''' </summary>
    <Serializable>
    Public Class PathComponents

        ''' <summary>
        ''' The RolePath is the heart of the derivation plan: a constrained walk through roles.
        ''' </summary>
        <XmlElement("RolePath")>
        Public Property RolePath As New List(Of RolePath)

    End Class

End Namespace
