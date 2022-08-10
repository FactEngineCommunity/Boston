Imports System.Xml.Serialization

Namespace NORMA.Model

#Region "Objects"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ObjectifiedType

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property PlayedRoles() As New List(Of ObjectifiedTypeRole)

        '''<remarks/>
        Public Property PreferredIdentifier() As ObjectifiedTypePreferredIdentifier

        '''<remarks/>
        Public Property NestedPredicate() As ObjectifiedTypeNestedPredicate

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property _ReferenceMode() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsIndependent() As Boolean

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ObjectifiedTypeRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ObjectifiedTypePreferredIdentifier

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            ''' <summary>
            ''' Parameterless Constructor
            ''' </summary>
            Public Sub New()
            End Sub

            Public Sub New(ByVal asRef As String)
                Me.Ref = asRef
            End Sub

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ObjectifiedTypeNestedPredicate

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property IsImplied() As Boolean

            ''' <summary>
            ''' Parameterless Constructor
            ''' </summary>
            Public Sub New()
                Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
            End Sub

            Public Sub New(ByVal asRef As String)
                Call Me.New
                Me.Ref = asRef
            End Sub
        End Class

    End Class

#End Region

End Namespace
