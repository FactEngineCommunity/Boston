Imports System.Reflection
Imports System.ComponentModel

Public Module publicConstants

    Public Enum pcenumBroadcastType
        AddModel
        DeleteModel
        FEKLStatement
        ModelAddPage
        ModelAddValueType
        ModelAddEntityType
        ModelAddFactType
        ModelAddFactTypeReading
        ModelAddRoleConstraint
        ModelAddRoleToFactType
        ModelAddModelNote
        ModelAddRoleConstraintArgument
        ModelDeleteValueType
        ModelDeleteEntityType
        ModelDeleteFactType
        ModelDeleteFactTypeReading
        ModelDeleteRoleConstraint
        ModelDeleteModelNote
        ModelGetModelIdByModelName
        ModelRoleConstraintAddRoleConstraintRole
        ModelRoleConstraintAddArgument
        ModelSaved
        ModelUpdateEntityType
        ModelUpdateFactType
        ModelUpdateFactTypeReading
        ModelUdateModelNote
        ModelUpdateRole
        ModelUpdateRoleConstraint
        ModelUpdateValueType
        PageDropModelElementAtPoint
        PageMovePageObject
        PageRemovePageObject
        ProjectAddNamespace
        ProjectDeleteNamespace
        RoleReassignJoinedModelObject
        SaveModel
        UserManagementAddFunctionToRole
        UserManagementAddGroupToProject
        UserManagementAddPermissionToGroupOnProject
        UserManagementAddPermissionToUserOnProject
        UserManagementAddRoleToUser
        UserManagementAddRoleToUserOnProject
        UserManagementAddUserToGroup
        UserManagementAddUserToProject
        UserManagementInviteUserToJoinProject
        UserManagementRemoveFunctionFromRole
        UserManagementRemoveGroupFromProject
        UserManagementRemovePermissionFromGroupOnProject
        UserManagementRemovePermissionFromUserOnProject
        UserManagementRemoveRoleFromUser
        UserManagementRemoveRoleFromUserOnProject
        UserManagementRemoveUserFromGroup
        UserManagementRemoveUserFromProject
    End Enum

    <Serializable()>
    Public Enum pcenumErrorType
        None = 0
        ModelDoesntExist = 100
        ModelElementAlreadyExists = 101
        UndocumentedError = 1000
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

    <Serializable()> _
    Public Enum pcenumJoinPathError
        None
        CircularPathFound
        AmbiguousJoinPath
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
        <DataType("Boolean")> <Description("Boolean")> [Boolean]
        <DataType("LogicalTrueFalse")> <Description("Logical: True | False.")> LogicalTrueFalse
        <DataType("LogicalYesNo")> <Description("Logical: Yes | No.")> LogicalYesNo
        <DataType("AutoCounter")> <Description("Numeric: Auto Counter")> NumericAutoCounter
        <DataType("Decimal")> <Description("Numeric: Decimal")> NumericDecimal
        <DataType("FloatCustomPrecision")> <Description("Numeric: Float (Custom Precision)")> NumericFloatCustomPrecision
        <DataType("FloatDoublePrecision")> <Description("Numeric: Float (Double Precision)")> NumericFloatDoublePrecision
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
    Public Enum pcenumCMMLMultiplicity
        One
        Many
    End Enum

    <Serializable()> _
    Public Enum pcenumDatabaseType
        MSJet = 101
        SQLServer = 102
        ORACLE = 103
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
        RoleValueConstraint
        SubsetConstraint
        ValueConstraint
        ValueComparisonConstraint
    End Enum

    <Serializable()> _
    Public Enum pcenumRingConstraintType
        None
        Acyclic
        AcyclicIntransitive
        AcyclicStronglyIntransitive
        Antisymmetric
        Asymmetric
        AsymmetricIntransitive
        Intransitive
        Irreflexive
        PurelyReflexive
        Symmetric
        SymmetricIntransitive
        SymmetricIrreflexive
        SymmetricTransitive
        Transitive
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

    <Serializable()>
    Public Enum pcenumCardinalityRangeType
        LessThanOrEqual
        Equal
        GreaterThanOrEqual
        Between
    End Enum

    <Serializable()>
    Public Enum pcenumValueRangeType
        None
        LessThan
        LessThanOrEqual
        GreaterThanOrEqual
        GreaterThan
    End Enum

End Module


