Imports System.Xml.Serialization

Namespace NORMA.Model

#Region "Constraints"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ExclusionConstraint

        '''<remarks/>
        <XmlArrayItem("RoleSequence", IsNullable:=False)>
        Public Property RoleSequences() As RoleSequence()

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class RoleSequence

            '''<remarks/>
            <XmlElement("Role")>
            Public Property Role() As SequenceRole()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class SequenceRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class FrequencyConstraint

        '''<remarks/>
        Public Property RoleSequence() As FrequencyConstraintRoleSequence

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property MinFrequency() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property MaxFrequency() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FrequencyConstraintRoleSequence

            '''<remarks/>
            Public Property Role() As SequenceRole

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class SequenceRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class MandatoryConstraint

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property RoleSequence() As List(Of ConstraintRole)

        '''<remarks/>
        Public Property ImpliedByObjectType() As ConstraintImpliedByObjectType

        '''<remarks/>
        Public Property InherentForObjectType() As ConstraintInherentForObjectType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsSimple() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsImplied() As Boolean

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            Public Sub New()
                Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
            End Sub

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintImpliedByObjectType

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintInherentForObjectType

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class RingConstraint

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property RoleSequence() As ConstraintRole()

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Type() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/> 
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class EqualityConstraint

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlArrayItem("RoleSequence", IsNullable:=False)>
        Public Property RoleSequences() As RoleSequence()

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class RoleSequence

            '''<remarks/>
            <XmlElement("Role")>
            Public Property Role() As Role()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class Role

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class SubsetConstraint

        '''<remarks/>
        <XmlArrayItem("RoleSequence", IsNullable:=False)>
        Public Property RoleSequences() As ConstraintRoleSequence()

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintRoleSequence

            '''<remarks/>
            <XmlElement("Role")>
            Public Property Role() As SequenceRole()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class SequenceRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class UniquenessConstraint

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property RoleSequence() As ConstraintRole()

        '''<remarks/>
        Public Property PreferredIdentifierFor() As ConstraintPreferredIdentifierFor

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsInternal() As Boolean

        Public Sub New()
            Id = "_" & Guid.NewGuid().ToString.ToUpper
        End Sub

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            Public Sub New()
                Id = "_" & Guid.NewGuid().ToString.ToUpper
            End Sub

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ConstraintPreferredIdentifierFor

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

#End Region

End Namespace
