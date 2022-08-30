Imports System.Xml.Serialization

Namespace NORMA

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore"),
        XmlRoot([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore", IsNullable:=False)>
    Partial Public Class ORMModel

        '''<remarks/>
        Public Property Objects() As ORMModelObjects

        '''<remarks/>
        Public Property Facts() As ORMModelFacts

        '''<remarks/>
        Public Property Constraints() As ORMModelConstraints

        '''<remarks/>
        Public Property DataTypes() As ORMModelDataTypes

        '''<remarks/>
        <XmlArrayItem("ModelNote", IsNullable:=False)>
        Public Property ModelNotes() As ORMModelModelNote()

        '''<remarks/>
        Public Property ModelErrors() As ORMModelModelErrors

        '''<remarks/>
        <XmlArrayItem("ReferenceModeKind", IsNullable:=False)>
        Public Property ReferenceModeKinds() As ORMModelReferenceModeKind()

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String


        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()

            Me.Objects = New ORMModelObjects

        End Sub

        Public Sub New(ByVal aiId As Integer, ByVal asName As String)
            Call Me.New

            Me.Id = "_" & aiId
            Me.Name = asName

        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelObjects

        '''<remarks/>
        <XmlElement("EntityType", GetType(Model.EntityType)),
         XmlElement("ObjectifiedType", GetType(Model.ObjectifiedType)),
         XmlElement("ValueType", GetType(Model.ValueType))>
        Public Property Items() As Object() = {}

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelFacts

        '''<remarks/>
        <XmlElement("Fact", GetType(Model.Fact)),
         XmlElement("ImpliedFact", GetType(Model.FactsImpliedFact)),
         XmlElement("SubtypeFact", GetType(Model.ORMModelFactsSubtypeFact))>
        Public Property Items() As Object()

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelConstraints

        '''<remarks/>

        <XmlElement("ExclusionConstraint", GetType(Model.ExclusionConstraint)),
            XmlElement("EqualityConstraint", GetType(Model.EqualityConstraintType)),
            XmlElement("FrequencyConstraint", GetType(Model.FrequencyConstraint)),
            XmlElement("MandatoryConstraint", GetType(Model.MandatoryConstraint)),
            XmlElement("RingConstraint", GetType(Model.RingConstraintType)),
            XmlElement("SubsetConstraint", GetType(Model.SubsetConstraint)),
            XmlElement("UniquenessConstraint", GetType(Model.UniquenessConstraint))>
        Public Property Items() As List(Of Object)

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelDataTypes

        '''<remarks/>
        Public Property UnspecifiedDataType() As Model.UnspecifiedDataType

        '''<remarks/>
        Public Property AutoCounterNumericDataType() As Model.AutoCounterNumericDataType

        '''<remarks/>
        Public Property VariableLengthTextDataType() As Model.VariableLengthTextDataType

        '''<remarks/>
        Public Property FixedLengthTextDataType() As Model.FixedLengthTextDataType

        '''<remarks/>
        Public Property LargeLengthTextDataType() As Model.LargeLengthTextDataType

        '''<remarks/>
        Public Property SignedIntegerNumericDataType() As Model.SignedIntegerNumericDataType

        '''<remarks/>
        Public Property UnsignedIntegerNumericDataType() As Model.UnsignedIntegerNumericDataType

        '''<remarks/>
        Public Property VariableLengthRawDataDataType() As Model.VariableLengthRawDataDataType

        '''<remarks/>
        Public Property TrueOrFalseLogicalDataType() As Model.TrueOrFalseLogicalDataType

        Public Property FloatingPointNumericDataType() As Model.FloatingPointNumericDataType

        Public Property DateTemporalDataType() As Model.DateTemporalDataType

        Public Property SignedSmallIntegerNumericDataType() As Model.SignedSmallIntegerNumericDataType

        Public Property SignedLargeIntegerNumericDataType() As Model.SignedLargeIntegerNumericDataType

        Public Property UnsignedTinyIntegerNumericDataType() As Model.UnsignedTinyIntegerNumericDataType

        Public Property UnsignedSmallIntegerNumericDataType() As Model.UnsignedSmallIntegerNumericDataType

        Public Property SinglePrecisionFloatingPointNumericDataType() As Model.SinglePrecisionFloatingPointNumericDataType

        Public Property DoublePrecisionFloatingPointNumericDataType() As Model.DoublePrecisionFloatingPointNumericDataType

        Public Property DecimalNumericDataType() As Model.DecimalNumericDataType

        Public Property MoneyNumericDataType() As Model.MoneyNumericDataType

        Public Property FixedLengthRawDataDataType() As Model.FixedLengthRawDataDataType

        Public Property LargeLengthRawDataDataType() As Model.LargeLengthRawDataDataType

        Public Property PictureRawDataDataType() As Model.PictureRawDataDataType

        Public Property OleObjectRawDataDataType() As Model.OleObjectRawDataDataType

        Public Property AutoTimestampTemporalDataType() As Model.AutoTimestampTemporalDataType

        Public Property TimeTemporalDataType() As Model.TimeTemporalDataType

        Public Property DateAndTimeTemporalDataType() As Model.DateAndTimeTemporalDataType

        Public Property YesOrNoLogicalDataType() As Model.YesOrNoLogicalDataType

        Public Property RowIdOtherDataType() As Model.RowIdOtherDataType

        Public Property ObjectIdOtherDataType() As Model.ObjectIdOtherDataType

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelModelNote

        '''<remarks/>
        Public Property Text() As String

        '''<remarks/>
        Public Property ReferencedBy() As Model.NoteReferencedBy

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelModelErrors

        '''<remarks/>
        <XmlElement("EntityTypeRequiresReferenceSchemeError", GetType(Model.EntityTypeRequiresReferenceSchemeError)),
            XmlElement("FactTypeRequiresInternalUniquenessConstraintError", GetType(Model.FactTypeRequiresInternalUniquenessConstraintError)),
            XmlElement("FactTypeRequiresReadingError", GetType(Model.FactTypeRequiresReadingError))>
        Public Property Items() As Object()

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class ORMModelReferenceModeKind

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property FormatString() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property ReferenceModeType() As String

    End Class

End Namespace

Namespace NORMA.Model

#Region "Model Note"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class NoteReferencedBy

        '''<remarks/>
        Public Property ObjectType() As RefObjectType

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class RefObjectType

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

#End Region

#Region "Model Errors"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class EntityTypeRequiresReferenceSchemeError

        '''<remarks/>
        Public Property EntityType() As ErrorEntityType

        '''<remarks/>
        <XmlAttribute()>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ErrorEntityType

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class FactTypeRequiresInternalUniquenessConstraintError

        '''<remarks/>
        Public Property Fact() As ErrorFact

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ErrorFact

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class FactTypeRequiresReadingError

        '''<remarks/>
        Public Property Fact() As ErrorFact

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
        Partial Public Class ErrorFact

            '''<remarks/>
            <XmlAttribute(AttributeName:="ref")>
            Public Property Ref() As String

        End Class

    End Class

#End Region

End Namespace