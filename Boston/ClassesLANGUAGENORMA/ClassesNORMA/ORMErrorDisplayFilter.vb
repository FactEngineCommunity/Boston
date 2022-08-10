Imports System.Xml.Serialization

Namespace NORMA

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore"),
        XmlRoot([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore", IsNullable:=False)>
    Public Class ErrorDisplayFilter

        ''' <summary>
        ''' Get or Set the Catagories for Error Display Filter
        ''' </summary>
        ''' <returns></returns>
        Public Property Categories() As Category

        ''' <summary>
        ''' Get or Set the Id for Error Display filter
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        ''' <summary>
        ''' Get or Set the Ref property
        ''' </summary>
        <XmlAttribute(AttributeName:="ref")>
        Public Property Ref() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class Category

            '''<remarks/>
            Public Property ConstraintImplicationAndContradictionErrorCategory() As Object

            '''<remarks/>
            Public Property ConstraintStructureErrorCategory() As Object

            '''<remarks/>
            Public Property DataTypeAndValueErrorCategory() As Object

            '''<remarks/>
            Public Property FactTypeDefinitionErrorCategory() As Object

            '''<remarks/>
            Public Property ElementGroupingErrorCategory() As Object

            '''<remarks/>
            Public Property NameErrorCategory() As Object

            '''<remarks/>
            Public Property ReferenceSchemeErrorCategory() As Object

            '''<remarks/>
            Public Property PopulationErrorCategory() As Object

        End Class

    End Class

End Namespace

