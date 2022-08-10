Imports System.Xml.Serialization

Namespace NORMA.Model

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class EntityType

        '''<remarks/>
        Public Property Notes() As EntityTypeNotes

        '''<remarks/>
        Public Property PlayedRoles() As EntityTypePlayedRoles

        '''<remarks/>
        Public Property PreferredIdentifier() As EntityTypePreferredIdentifier

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property _ReferenceMode() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class EntityTypeNotes

            '''<remarks/>
            Public Property Note() As EntityTypeNote

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class EntityTypeNote

                '''<remarks/>
                Public Property Text() As String

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class EntityTypePlayedRoles

            '''<remarks/>
            <XmlElement("Role", GetType(PlayedRolesRole)),
             XmlElement("SubtypeMetaRole", GetType(PlayedRolesSubtypeMetaRole)),
             XmlElement("SupertypeMetaRole", GetType(PlayedRolesSupertypeMetaRole))>
            Public Property Items() As Object()

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class PlayedRolesRole

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class PlayedRolesSubtypeMetaRole

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class PlayedRolesSupertypeMetaRole

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class EntityTypePreferredIdentifier

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class


End Namespace
