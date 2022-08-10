Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace NORMA.Model

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ValueType

        '''<remarks/>
        <XmlArrayItem("Role", IsNullable:=False)>
        Public Property PlayedRoles() As List(Of ValueTypeRole)

        '''<remarks/>
        Public Property ConceptualDataType() As ValueTypeConceptualDataType

        '''<remarks/>
        Public Property ValueRestriction() As ValueTypeValueRestriction

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsImplicitBooleanValue() As Boolean

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ValueTypeRole

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ValueTypeConceptualDataType

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Scale() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Length() As String

            ''' <summary>
            ''' Parameterless Constructor
            ''' </summary>
            Public Sub New()
            End Sub

            Public Sub New(ByVal asDataTypeReference As String)

                Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
                Me.Ref = asDataTypeReference
                Me.Scale = "0"
                Me.Length = "0"

            End Sub

            Public Sub New(ByVal asDataTypeReference As String, ByVal asScale As String, ByVal asLength As String)

                Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
                Me.Ref = asDataTypeReference
                Me.Scale = asScale
                Me.Length = asLength

            End Sub

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ValueTypeValueRestriction

            '''<remarks/>
            Public Property ValueConstraint() As ValueTypeRestrictionValueConstraint

        End Class

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ValueTypeRestrictionValueConstraint

            '''<remarks/>
            <XmlArrayItem("ValueRange", IsNullable:=False)>
            Public Property ValueRanges() As RestrictionValueConstraintValueRange()

            '''<remarks/>
            <XmlAttribute(AttributeName:="id")>
            Public Property Id() As String

            '''<remarks/>
            <XmlAttribute()>
            Public Property Name() As String

            <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
            Partial Public Class RestrictionValueConstraintValueRange

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

End Namespace
