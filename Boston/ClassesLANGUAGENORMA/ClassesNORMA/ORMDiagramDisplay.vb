Imports System.Xml.Serialization

Namespace NORMA

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay"),
        XmlRoot([Namespace]:="http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay", IsNullable:=False)>
    Public Class DiagramDisplay

        ''' <summary>
        ''' Get or set the Id for Display
        ''' </summary>
        <XmlElement("Diagram")>
        Public Property Diagrams() As Diagram()

        ''' <summary>
        ''' Get or set the Id for DiagramDisplay
        ''' </summary>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        ''' <summary>
        ''' Get or set save indicating Diagram position should be saved or not
        ''' </summary>
        <XmlAttribute()>
        Public Property SaveDiagramPosition() As Boolean

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2008-11/DiagramDisplay")>
        Partial Public Class Diagram

            ''' <summary>
            ''' Get or set the Id for Display
            ''' </summary>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            ''' <summary>
            ''' Get or set the Ref for Display
            ''' </summary>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            ''' <summary>
            ''' Get or set the CenterPoint for Display
            ''' </summary>
            <XmlAttribute()>
            Public Property CenterPoint() As String

            ''' <summary>
            ''' Get or set for Display is active or not
            ''' </summary>
            <XmlAttribute()>
            Public Property IsActive() As Boolean

        End Class

    End Class

End Namespace
