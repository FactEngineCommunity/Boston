Imports System.Xml.Serialization

Namespace NORMA.ORMDiagram

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class ExternalConstraintShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class FactTypeShape

        '''<remarks/>
        Public Property RelativeShapes() As RelativeShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property Roles() As FactTypeShapeRole()

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property DisplayOrientation() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property DisplayRoleNames() As String

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
        Partial Public Class RelativeShape

            '''<remarks/>
            Public Property ReadingShape() As FactTypeReadingShape

            '''<remarks/>
            Public Property ValueConstraintShape() As FactTypeValueConstraintShape

            '''<remarks/>
            Public Property ObjectifiedFactTypeNameShape() As FactTypeNameShape

            '''<remarks/> 
            <XmlElement("RoleNameShape")>
            Public Property RoleNameShapes() As FactTypeRoleNameShape()

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
            Partial Public Class FactTypeReadingShape

                '''<remarks/>
                Public Property Subject() As Subject

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property IsExpanded() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property AbsoluteBounds() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
            Partial Public Class FactTypeValueConstraintShape

                '''<remarks/>
                Public Property Subject() As Subject

                '''<remarks/>
                <XmlAttribute()>
                Public Property id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property IsExpanded() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property AbsoluteBounds() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
            Partial Public Class FactTypeNameShape

                '''<remarks/>
                Public Property Subject() As Subject

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property IsExpanded() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property AbsoluteBounds() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
            Partial Public Class FactTypeRoleNameShape

                '''<remarks/>
                Public Property Subject() As Subject

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property IsExpanded() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property AbsoluteBounds() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
        Partial Public Class FactTypeShapeRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class FrequencyConstraintShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class ModelNoteShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class ObjectTypeShape

        '''<remarks/>
        Public Property RelativeShapes() As ObjectTypeRelativeShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property ExpandRefMode() As Boolean

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
        Partial Public Class ObjectTypeRelativeShape

            '''<remarks/>
            Public Property ValueConstraintShape() As RelativeShapeValueConstraintShape

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
            Partial Public Class RelativeShapeValueConstraintShape

                '''<remarks/>
                Public Property Subject() As Subject

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property IsExpanded() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property AbsoluteBounds() As String

            End Class

        End Class

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class RingConstraintShape

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/> 
        <XmlAttribute()>
        Public Property IsExpanded() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property AbsoluteBounds() As String

    End Class

End Namespace