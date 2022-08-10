Imports System.Xml.Serialization

Namespace NORMA.Model

#Region "Facts"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class Fact

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property FactRoles() As List(Of FactRole)

        '''<remarks/>
        <XmlArrayItem("ReadingOrder", IsNullable:=False)>
        Public Property ReadingOrders() As New List(Of FactReadingOrder)

        '''<remarks/>
        Public Property InternalConstraints() As FactInternalConstraints

        '''<remarks/>
        Public Property DerivationRule() As FactDerivationRule

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property _Name() As String

        Public Sub New()
        End Sub

        Public Sub New(ByVal aiId As String, ByVal asName As String)
            Id = aiId
            _Name = asName
        End Sub

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactRole

            '''<remarks/>
            Public Property RolePlayer() As FactRoleRolePlayer

            '''<remarks/>
            Public Property ValueRestriction() As FactRoleValueRestriction

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property _IsMandatory() As Boolean

            '''<remarks/>
            <XmlAttribute()>
            Public Property _Multiplicity() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Name() As String

            Public Sub New()
                Id = "_" & System.Guid.NewGuid.ToString.ToUpper
            End Sub

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class FactRoleRolePlayer

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class FactRoleValueRestriction

                '''<remarks/>
                Public Property RoleValueConstraint() As RestrictionRoleValueConstraint

                <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
                Partial Public Class RestrictionRoleValueConstraint

                    '''<remarks/>
                    Public Property ValueRanges() As ConstraintValueRanges

                    '''<remarks/>
                    <XmlAttribute(AttributeName:="id")>
                    Public Property Id() As String

                    '''<remarks/>
                    <XmlAttribute()>
                    Public Property Name() As String

                End Class

                <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
                Partial Public Class ConstraintValueRanges

                    '''<remarks/>
                    Public Property ValueRange() As ConstraintValueRangesValueRange

                End Class

                <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
                Partial Public Class ConstraintValueRangesValueRange

                    '''<remarks/>
                    <XmlAttribute(AttributeName:="id")>
                    Public Property Id() As String

                    '''<remarks/>
                    <XmlAttribute()>
                    Public Property MinValue() As String

                    '''<remarks/>
                    <XmlAttribute()>
                    Public Property MaxValue() As String

                    '''<remarks/>
                    <XmlAttribute()>
                    Public Property MinInclusion() As String

                    '''<remarks/>
                    <XmlAttribute()>
                    Public Property MaxInclusion() As String

                End Class

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactReadingOrder

            '''<remarks/>
            <XmlArrayItem("Reading", IsNullable:=False)>
            Public Property Readings() As Reading()

            '''<remarks/>
            <XmlArrayItem("Role", IsNullable:=False)>
            Public Property RoleSequence() As Role()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            Public Sub New()
                Id = "_" & System.Guid.NewGuid.ToString.ToUpper
            End Sub

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class Reading

                '''<remarks/>
                Public Property Data() As String

                Public Property ExpandedData() As ExpandedData

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String


                Public Sub New()

                    Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper

                End Sub

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class Role

                '''<remarks/> 
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class ExpandedData

                Public Property RoleText As New RoleText

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class RoleText

                <XmlAttribute>
                Public Property RoleIndex As Integer

                <XmlAttribute>
                Public Property FollowingText As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactInternalConstraints

            '''<remarks/>
            <XmlElement("MandatoryConstraint", GetType(MandatoryConstraint)),
             XmlElement("UniquenessConstraint", GetType(UniquenessConstraint))>
            Public Property Items() As List(Of Object)

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class MandatoryConstraint

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            '''<remarks/>
            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class UniquenessConstraint

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactDerivationRule

            '''<remarks/>
            Public Property DerivationExpression() As RuleDerivationExpression

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class RuleDerivationExpression

                '''<remarks/>
                Public Property Body() As String

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property DerivationStorage() As String

            End Class

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class FactsImpliedFact

        '''<remarks/>
        Public Property FactRoles() As ImpliedFactRoles

        '''<remarks/>
        <XmlArrayItem("ReadingOrder", IsNullable:=False)>
        Public Property ReadingOrders() As ReadingOrder()

        '''<remarks/>
        Public Property InternalConstraints() As FactInternalConstraints

        '''<remarks/>
        Public Property ImpliedByObjectification() As FactImpliedByObjectification

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property _Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ImpliedFactRoles

            '''<remarks/>
            Public Property RoleProxy() As ImpliedFactRoleProxy

            '''<remarks/>
            Public Property Role() As ImpliedFactRole

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class ImpliedFactRoleProxy

                '''<remarks/>
                Public Property Role() As ProxyRole

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
                Partial Public Class ProxyRole

                    '''<remarks/>
                    <XmlAttribute(AttributeName:="ref")>
                    Public Property Ref() As String

                End Class

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class ImpliedFactRole

                '''<remarks/>
                Public Property RolePlayer() As ImpliedFactRolePlayer

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property _IsMandatory() As Boolean

                '''<remarks/>
                <XmlAttribute()>
                Public Property _Multiplicity() As String

                '''<remarks/>
                <XmlAttribute()>
                Public Property Name() As String

                <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
                Partial Public Class ImpliedFactRolePlayer

                    '''<remarks/>
                    <XmlAttribute(AttributeName:="ref")>
                    Public Property Ref() As String

                End Class

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ReadingOrder

            '''<remarks/>
            Public Property Readings() As OrderReadings

            '''<remarks/>
            <XmlArrayItem("Role", IsNullable:=False)>
            Public Property RoleSequence() As OrderRole()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class OrderReadings

                '''<remarks/>
                Public Property Reading() As OrderReading

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class OrderReading

                '''<remarks/>
                Public Property Data() As String

                '''<remarks/>
                <XmlAttribute(AttributeName:="id")>
                Public Property Id() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class OrderRole

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactInternalConstraints

            '''<remarks/>
            Public Property MandatoryConstraint() As ConstraintsMandatoryConstraint

            '''<remarks/>
            Public Property UniquenessConstraint() As ConstraintsUniquenessConstraint

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class ConstraintsMandatoryConstraint

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class ConstraintsUniquenessConstraint

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class FactImpliedByObjectification

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelFactsSubtypeFact

        '''<remarks/>
        Public Property FactRoles() As SubtypeFactRoles

        '''<remarks/>
        Public Property InternalConstraints() As SubtypeFactInternalConstraints

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property _Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property PreferredIdentificationPath() As Boolean

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class SubtypeFactRoles

            '''<remarks/>
            Public Property SubtypeMetaRole() As RolesSubtypeMetaRole

            '''<remarks/>
            Public Property SupertypeMetaRole() As RolesSupertypeMetaRole

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class RolesSubtypeMetaRole

            '''<remarks/>
            Public Property RolePlayer() As MetaRolePlayer

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property _IsMandatory() As Boolean

            '''<remarks/>
            <XmlAttribute()>
            Public Property _Multiplicity() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Name() As String

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class MetaRolePlayer

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class RolesSupertypeMetaRole

            '''<remarks/>
            Public Property RolePlayer() As MetaRolePlayer

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property _IsMandatory() As Boolean

            '''<remarks/>
            <XmlAttribute()>
            Public Property _Multiplicity() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Name() As String

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class MetaRolePlayer

                '''<remarks/>
                <XmlAttribute(AttributeName:="ref")>
                Public Property Ref() As String

            End Class

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class SubtypeFactInternalConstraints

            '''<remarks/>
            Public Property MandatoryConstraint() As InternalConstraintsMandatoryConstraint

            '''<remarks/>
            <XmlElement("UniquenessConstraint")>
            Public Property UniquenessConstraint() As InternalConstraintsUniquenessConstraint()

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class InternalConstraintsMandatoryConstraint

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class InternalConstraintsUniquenessConstraint

            '''<remarks/> 
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

#End Region

End Namespace
