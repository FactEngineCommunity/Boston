Imports System
Imports System.Collections.Generic
Imports System.Xml.Serialization

Namespace FBM

    '==========================================================
    '  core:CalculatedValues  (typed)
    '==========================================================

    ''' <summary>
    ''' A single calculated value definition inside a RolePath.
    '''
    ''' NORMA shape (as seen in our working tests):
    '''
    '''   <CalculatedValue id=".">
    '''     <Function ref="functionId"/>
    '''     <AggregationContext>
    '''       <PathRoot ref="rootObjectTypeId"/>
    '''     </AggregationContext>
    '''     <Inputs>
    '''       <Input id=".">
    '''         <Parameter ref="parameterId"/>
    '''         <Source>
    '''           <PathedRole ref="roleId"/>
    '''           OR <CalculatedValue ref="calcId"/>
    '''           OR <Constant id="."><Value>100</Value></Constant>
    '''         </Source>
    '''       </Input>
    '''     </Inputs>
    '''   </CalculatedValue>
    '''
    ''' Boston usage:
    '''   - preserve NORMA's expression-tree structure faithfully
    '''   - support nested function calls
    '''   - preserve aggregation scope for rendering and round-tripping
    ''' </summary>
    <Serializable>
    <XmlRoot("CalculatedValue", Namespace:="orm")>
    Public Class CalculatedValue

        <XmlAttribute("id")>
        Public Property Id As String

        <XmlElement("Function")>
        Public Property [Function] As FunctionRef

        ''' <summary>
        ''' Optional aggregation context for aggregate functions such as count, sum or average.
        '''
        ''' In NORMA this commonly wraps a PathRoot reference, identifying the binding
        ''' relative to which the aggregate is evaluated.
        '''
        ''' Example:
        '''   <AggregationContext>
        '''       <PathRoot ref="_B9972D2C-2A88-4BA9-8278-53AAA219A65A" />
        '''   </AggregationContext>
        '''
        ''' Boston usage:
        '''   - preserve NORMA's aggregate scope faithfully
        '''   - support rendering phrases such as:
        '''         count(each Role for that Predicate)
        ''' </summary>
        <XmlElement("AggregationContext")>
        Public Property AggregationContext As AggregationContext

        <XmlElement("Inputs")>
        Public Property Inputs As Inputs

    End Class


    '==========================================================
    '  core:Function ref inside CalculatedValue
    '==========================================================

    <Serializable>
    <XmlRoot("Function", Namespace:="orm")>
    Public Class FunctionRef

        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class

    ''' <summary>
    ''' Represents the optional aggregation context for a CalculatedValue.
    '''
    ''' NORMA uses this to indicate the binding scope for an aggregate.
    ''' In the examples we have seen, this contains a PathRoot reference.
    ''' 
    '''==========================================================
    '''core:AggregationContext inside CalculatedValue
    '''==========================================================
    '''
    ''' Example:
    '''   <AggregationContext>
    '''       <PathRoot ref="..." />
    '''   </AggregationContext>
    ''' </summary>
    <Serializable>
    <XmlRoot("AggregationContext", Namespace:="orm")>
    Public Class AggregationContext

        ''' <summary>
        ''' Optional PathRoot reference identifying the root binding
        ''' relative to which the aggregation is evaluated.
        ''' </summary>
        <XmlElement("PathRoot")>
        Public Property PathRoot As PathRootReference

    End Class


    '==========================================================
    '  core:Inputs / Input / Parameter / Source
    '==========================================================

    <Serializable>
    <XmlRoot("Inputs", Namespace:="orm")>
    Public Class Inputs

        <XmlElement("Input")>
        Public Property Input As New List(Of Input)

    End Class


    <Serializable>
    <XmlRoot("Input", Namespace:="orm")>
    Public Class Input

        <XmlAttribute("id")>
        Public Property Id As String

        <XmlElement("Parameter")>
        Public Property Parameter As ParameterRef

        <XmlElement("Source")>
        Public Property Source As Source

    End Class


    <Serializable>
    <XmlRoot("Parameter", Namespace:="orm")>
    Public Class ParameterRef

        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    ''' <summary>
    ''' Source is a choice node: exactly one of:
    '''   - PathedRole ref
    '''   - CalculatedValue ref (dependency)
    '''   - Constant (literal)
    ''' </summary>
    <Serializable>
    <XmlRoot("Source", Namespace:="orm")>
    Public Class Source

        <XmlElement("PathedRole", GetType(PathedRoleRef))>
        <XmlElement("CalculatedValue", GetType(CalculatedValueRef))>
        <XmlElement("Constant", GetType(Constant))>
        Public Property Item As Object

    End Class


    <Serializable>
    <XmlRoot("PathedRole", Namespace:="orm")>
    Public Class PathedRoleRef

        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    <Serializable>
    <XmlRoot("CalculatedValue", Namespace:="orm")>
    Public Class CalculatedValueRef

        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    <Serializable>
    <XmlRoot("Constant", Namespace:="orm")>
    Public Class Constant

        <XmlAttribute("id")>
        Public Property Id As String

        <XmlElement("Value")>
        Public Property Value As String

    End Class

End Namespace