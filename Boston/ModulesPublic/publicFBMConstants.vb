Imports System.ComponentModel
Imports System.Reflection

Public Module publicFBMConstants

    <AttributeUsageAttribute(AttributeTargets.Field)>
    Public Class AlternateDataTypeAttribute
        Inherits Attribute
        ' etc
        Public m_name As String

        Public Sub New(ByVal asDataTypeName As String)
            Me.m_name = asDataTypeName
        End Sub

        ''' <summary>
        ''' 20230121-VM-Implemented to transition from StringFixedLength (etc) to TextFixedLength.
        ''' </summary>
        ''' <param name="tp"></param>
        ''' <param name="name"></param>
        ''' <returns></returns>
        Public Shared Function [Get](ByVal tp As Type, ByVal name As String) As String

            Dim attr As AlternateDataTypeAttribute

            Dim mi As MemberInfo
            Dim mai As MemberInfo() = tp.GetMembers()

            For Each mi In mai
                attr = TryCast(Attribute.GetCustomAttribute(mi, GetType(AlternateDataTypeAttribute)), AlternateDataTypeAttribute)
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

            '20230121-VM-Implemented to transition from StringFixedLength (etc) to TextFixedLength.
            Return AlternateDataTypeAttribute.Get(GetType(pcenumORMDataType), name)

        End Function
    End Class

    <AttributeUsageAttribute(AttributeTargets.Field)> _
    Public Class AbsorptionChoiceAttribute
        Inherits Attribute
        ' etc
        Public m_name As String

        Public Sub New(ByVal asAbsorptionChoiceName As String)
            Me.m_name = asAbsorptionChoiceName
        End Sub

        Public Shared Function [Get](ByVal tp As Type, ByVal name As String) As String

            Dim attr As AbsorptionChoiceAttribute

            Dim mi As MemberInfo
            Dim mai As MemberInfo() = tp.GetMembers()

            For Each mi In mai
                attr = TryCast(Attribute.GetCustomAttribute(mi, GetType(AbsorptionChoiceAttribute)), AbsorptionChoiceAttribute)
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
        <DataType("SignedBigInteger")> <Description("Numeric: Signed Big Integer")> NumericSignedBigInteger 'NB 'Big' is 'Large' In NORMA .orm XML file
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
        <DataType("TemporalDate")> <Description("Temporal: Date")> TemporalDate
        <DataType("TemporalDateTime")> <Description("Temporal: Date & Time")> TemporalDateAndTime
        <DataType("Time")> <Description("Temporal: Time")> TemporalTime
        '<DataType("StringFixedLength")> <Description("Text: Fixed Length")> TextFixedLengthLegacy
        '<DataType("StringLargeLength")> <Description("Text: Large Length")> TextLargeLengthLegacy
        '<DataType("StringVariableLength")> <Description("Text: Variable Length")> TextVariableLengthLegacy
        <DataType("TextFixedLength")> <AlternateDataType("StringFixedLength")> <Description("Text: Fixed Length")> TextFixedLength
        <DataType("TextLargeLength")> <AlternateDataType("StringLargeLength")> <Description("Text: Large Length")> TextLargeLength
        <DataType("TextVariableLength")> <AlternateDataType("StringVariableLength")> <Description("Text: Variable Length")> TextVariableLength
    End Enum

    <Serializable()> _
    Public Enum pcenumAbsorptionChoice
        <AbsorptionChoice("AbsorbedAllPaths")> <Description("Absorbed (All Paths)")> AbsorbedAllPaths
        <AbsorptionChoice("Separate")> <Description("Separate")> Separate
    End Enum


End Module
