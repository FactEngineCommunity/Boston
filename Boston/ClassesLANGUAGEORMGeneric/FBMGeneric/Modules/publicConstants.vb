Imports System.ComponentModel
Imports System.Reflection

Namespace FBM

    Public Module PublicConstants

        <Serializable()> _
        Public Enum pcenumConceptType
            [Class] 'Represents a Class in a UML Class Diagram
            Comment
            Actor 'Represents an Actor in a UML Use Case Diagram
            ActorName
            ActorProcessLink
            AssociationEndRole
            'CardinalityConstraint
            Concept
            DataStore
            Decision
            Enterprise
            Entity
            EntityType
            EntityTypeName
            EnumerationSet 'Represents an Enumeration Set in a UML Class Diagram
            'EqualityConstraint
            [Event]
            'ExclusionConstraint
            'ExclusiveORConstraint
            'ExternalRoleConstraint
            'ExternalUniquenessConstraint
            [Function]
            FactTypeReading
            Fact
            FactType
            FactTypeDerivationText
            FactTypeName
            FactTable
            GeneralConcept
            Generalisation
            'FrequencyConstraint
            'InclusiveORConstraint
            'InternalRoleConstraint
            Link
            ModelNote
            Model
            ObjectInstance
            Page
            Procedure
            Process
            Role
            'RingConstraint
            RoleGroupSelectNode
            RoleData
            RoleName
            RoleConstraint
            RoleConstraintRole
            Solution
            State
            StrategicGoal
            SubjectArea
            SubtypeConstraint
            SubtypeLink
            SystemBoundary
            TotalSubtype
            Value
            ValueConstraint
            ValueType
        End Enum

        <Serializable()> _
        Public Enum pcenumFactTypeOrientation
            Horizontal
            Vertical
        End Enum

        <Serializable()> _
        Public Enum pcenumRoleJoinType 'NB Stored as Integer within the database.
            EntityType = 1
            ValueType = 2
            FactType = 3
        End Enum

        <Serializable()> _
        Public Enum pcenumJoinPathError
            None
            CircularPathFound
            AmbiguousJoinPath
        End Enum

        Public Enum pcenumBinaryRelationMultiplicityType
            OneToOne
            OneToMany
            ManyToOne
            ManyToMany
        End Enum

        Public Enum pcenumModelErrors
            ObjectTypeRequiresPrimarySupertypeError = 99
            PopulationUniquenessError = 100
            TooManyRoleSequencesError = 101
            EqualityImpliedByMandatoryError = 102
            TooFewFactTypeRoleInstancesError = 103
            TooFewEntityTypeRoleInstancesError = 104
            EntityTypeRequiresReferenceSchemeError = 105
            FactTypeRequiresInternalUniquenessConstraintError = 106
            FrequencyConstraintContradictsInternalUniquenessConstraintError = 107
            ExternalConstraintRoleSequenceArityMismatchError = 108
            PreferredIdentifierRequiresMandatoryError = 109
            ImpliedInternalUniquenessConstraintError = 110
            CompatibleValueTypeInstanceValueError = 111
            RingConstraintTypeNotSpecifiedError = 112
            FrequencyConstraintMinMaxError = 113
            CompatibleRolePlayerTypeError = 114
            FactTypeRequiresReadingError = 115
            CompatibleSupertypesError = 116
            TooFewReadingRolesError = 117
            RolePlayerRequiredError = 118
            ValueMismatchError = 119
            ContradictionError = 120
            ImplicationError = 121
            NMinusOneError = 122
            DuplicateNameError = 123
            ValueRangeOverlapError = 124
            PopulationMandatoryError = 125
            TooFewRoleSequencesError = 126
            DataTypeNotSpecifiedError = 127
            TooManyReadingRolesError = 128
            RoleConstraintConflictError = 129
        End Enum

        Public Enum pcenumKLDataLetter
            a
            b
            c
            d
            e
            f
            g
            h
            i
            j
            k
            l
            m
            n
            o
            p
            q
            r
            s
            t
            u
            v
            w
            x
            y
            z
        End Enum

        <Serializable()> _
        Public Enum pcenumLanguage
            ORMModel = 1
            UseCaseDiagram = 2
            BusinessRulesNaturalLanguage = 3 'English (see Italian below...may need to change this. We ClassBrain for more information about natural language processing).
            ZNotation = 4
            FlowChart = 6
            DataFlowDiagram = 8
            EntityRelationshipDiagram = 9
            PropertyGraphSchema = 22
        End Enum

        Public Enum pcenumReferenceMode
            <Description(".#")> DotHash
            <Description(".code")> DotLowercaseCode
            <Description(".Code")> DotUppercaseCode
            <Description(".Id")> DotCamelcaseId
            <Description(".id")> DotLowercaseId
            <Description(".ID")> DotALLCAPSID
            <Description(".name")> DotLowercaseName
            <Description(".Name")> DotUppercaseName
            <Description(".Nr")> DotUppwercaseNr
            <Description(".nr")> DotLowercaseNr
            <Description(".Title")> DotUppercaseTitle
            <Description(".title")> DotLowercaseTitle
            <Description("AUD:")> AUD
            <Description("CE:")> CE
            <Description("Celcius:")> Celsius
            <Description("cm:")> cm
            <Description("EUR:")> EUR
            <Description("Fahrenheit:")> Fahrenheit
            <Description("kg:")> kg
            <Description("km:")> km
            <Description("mile:")> mile
            <Description("mm:")> mm
            <Description("USD:")> USD
        End Enum

        <Serializable()> _
        Public Enum pcenumRoleConstraintType
            CardinalityConstraint
            EqualityConstraint
            ExclusionConstraint
            ExclusiveORConstraint
            ExternalUniquenessConstraint
            FrequencyConstraint
            InclusiveORConstraint
            InternalUniquenessConstraint
            RingConstraint
            SubsetConstraint
        End Enum

        <AttributeUsageAttribute(AttributeTargets.Field)> _
        Public Class DataTypeAttribute
            Inherits Attribute
            ' etc
            Public m_name As String

            Public Sub New(ByVal asDataTypeName As String)
                Me.m_name = asDataTypeName
            End Sub

            Public Shared Function [Get](ByVal tp As Type, ByVal name As String) As String

                Dim attr As DataTypeAttribute

                Dim mi As MemberInfo
                Dim mai As MemberInfo() = tp.GetMembers()

                For Each mi In mai
                    attr = TryCast(Attribute.GetCustomAttribute(mi, GetType(DataTypeAttribute)), DataTypeAttribute)
                    If attr IsNot Nothing Then
                        If attr.m_name = name Then
                            Return mi.Name
                            Exit For
                        End If
                    End If
                Next

                Return Nothing

            End Function
        End Class


        ''' <summary>
        ''' See DataTypeAttribute Class (above) for how to get the name of an Enum member from its corresponding 
        '''   DataType attribute 'name'. Used when converting VAQL DataType tokens to ORMDataType enum.
        ''' </summary>
        ''' <remarks></remarks>
        <Serializable()> _
        Public Enum pcenumORMDataType
            <DataType("DataTypeNotSet")> <Description("<Data Type Not Set>")> DataTypeNotSet
            <DataType("LogicalTrueFalse")> <Description("Logical: True | False.")> LogicalTrueFalse
            <DataType("LogicalYesNo")> <Description("Logical: Yes | No.")> LogicalYesNo
            <DataType("AutoCounter")> <Description("Numeric: Auto Counter")> NumericAutoCounter
            <DataType("Decimal")> <Description("Numeric: Decimal")> NumericDecimal
            <DataType("FloatCustomPrecision")> <Description("Numeric: Float (Custom Precision)")> NumericFloatCustomPrecision
            <DataType("FloatDoublePrecision")> <Description("Numeric: Float (Double Precision)")> NumericFlotDoublePrecision
            <DataType("FloatSinglePrecistion")> <Description("Numeric: Float (Single Precision)")> NumericFloatSinglePrecision
            <DataType("Money")> <Description("Numeric: Money")> NumericMoney
            <DataType("SignedBigInteger")> <Description("Numeric: Signed Big Integer")> NumericSignedBigInteger
            <DataType("SignedInteger")> <Description("Numeric: Signed Integer")> NumericSignedInteger
            <DataType("SignedSmallInteger")> <Description("Numeric: Signed Small Integer")> NumericSignedSmallInteger
            <DataType("UnsignedBigInteger")> <Description("Numeric: Unsigned Big Integer")> NumericUnsignedBigInteger
            <DataType("UnsignedInteger")> <Description("Numeric: Unsigned Integer")> NumericUnsignedInteger
            <DataType("UnsignedSmallInteger")> <Description("Numeric: Unsigned Small Integer")> NumericUnsignedSmallInteger
            <DataType("UnsignedTinyInteger")> <Description("Numeric: Unsigned Tiny Integer")> NumericUnsignedTinyInteger
            <DataType("ObjectID")> <Description("Other: Object ID")> OtherObjectID
            <DataType("RowID")> <Description("Other: Row ID")> OtherRowID
            <DataType("RawDataFixedLength")> <Description("Raw Data: Fixed Length")> RawDataFixedLength
            <DataType("RawDataLargeLength")> <Description("Raw Data: Large Length")> RawDataLargeLength
            <DataType("RawDataOLEObject")> <Description("Raw Data: OLE Object")> RawDataOLEObject
            <DataType("RawDataPicture")> <Description("Raw Data: Picture")> RawDataPicture
            <DataType("VariableLength")> <Description("Raw Data: Variable Length")> RawDataVariableLength
            <DataType("AutoTimestamp")> <Description("Temporal: Auto Timestamp")> TemporalAutoTimestamp
            <DataType("Date")> <Description("Temporal: Date")> TemporalDate
            <DataType("DateTime")> <Description("Temporal: Date & Time")> TemporalDateAndTime
            <DataType("Time")> <Description("Temporal: Time")> TemporalTime
            <DataType("StringFixedLength")> <Description("Text: Fixed Length")> TextFixedLength
            <DataType("StringLargeLength")> <Description("Text: Large Length")> TextLargeLength
            <DataType("StringVariableLength")> <Description("Text: Variable Length")> TextVariableLength
        End Enum

        <Serializable()> _
        Public Enum pcenumCardinalityRangeType
            LessThanOREqual
            Equal
            GreaterThanOREqual
            Between
        End Enum

        <Serializable()> _
        Public Enum pcenumRingConstraintType
            None
            Irreflexive
            Asymmetric
            Intransitive
            Antisymmetric
            Acyclic
            AsymmetricIntransitive
            AcyclicIntransitive
            Symmetric
            SymmetricIrreflexive
            SymmetricIntransitive
            PurelyReflexive
            DeonticIrreflexive
            DeonticAssymmetric
            DeonticIntransitive
            DeonticAntisymmetric
            DeonticAcyclic
            DeonticAssymmetricIntransitive
            DeonticAcyclicIntransitive
            DeonticSymmetric
            DeonticSymmetricIrreflexive
            DeonticSymmetricIntransitive
            DeonticPurelyReflexive
        End Enum

        Public Enum pcenumDataType
            [String]
            [Integer]
            [Boolean]
            [Single]
            [Double]
            [Char]
            [Byte]
            [Date]
            [Long]
            [Short]
            [UInteger]
            [ULong]
            [UShort]
        End Enum

        <Serializable()> _
        Public Enum pcenumValueRangeType
            None
            LessThan
            LessThanOREqual
            GreaterThanOREqual
            GreaterThan
        End Enum

    End Module

End Namespace