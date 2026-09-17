Imports System.Xml.Serialization
Imports Boston.FBM

Namespace XMLModel

    ''' <summary>
    ''' A single calculated value definition inside a derivation RolePath.
    '''
    ''' NORMA shape:
    '''
    '''   <CalculatedValue id=".">
    '''     <Function ref="functionId" />
    '''     <AggregationContext>
    '''       <PathRoot ref="rootObjectTypeId" />
    '''     </AggregationContext>
    '''     <Inputs>
    '''       <Input id=".">
    '''         <Parameter ref="parameterId" />
    '''         <Source>
    '''           <PathedRole ref="roleId" />
    '''           OR
    '''           <CalculatedValue ref="calcId" />
    '''           OR
    '''           <Constant id=".">
    '''             <Value>...</Value>
    '''           </Constant>
    '''         </Source>
    '''       </Input>
    '''     </Inputs>
    '''   </CalculatedValue>
    '''
    ''' Boston usage:
    '''   - preserve NORMA's expression-tree structure in .fbm files
    '''   - support nested function calls
    '''   - preserve aggregation scope for rendering and round-tripping
    ''' </summary>
    <Serializable()>
    Public Class CalculatedValue

        ''' <summary>
        ''' Identifier for the calculated value.
        '''
        ''' This is the stable handle referenced elsewhere in the derivation,
        ''' such as from projections and from other calculated values.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Reference to the function being called by this calculated value.
        ''' </summary>
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
        ''' </summary>
        <XmlElement("AggregationContext")>
        Public Property AggregationContext As AggregationContext

        ''' <summary>
        ''' The ordered input arguments supplied to the function.
        ''' </summary>
        <XmlElement("Inputs")>
        Public Property Inputs As Inputs

    End Class

    '==========================================================
    '  core:Function ref inside CalculatedValue
    '==========================================================

    ''' <summary>
    ''' Reference to a Function definition used by a CalculatedValue.
    ''' </summary>
    <Serializable>
    <XmlRoot("Function", Namespace:="orm")>
    Public Class FunctionRef

        ''' <summary>
        ''' Reference to the Function id.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    '==========================================================
    '  core:AggregationContext inside CalculatedValue
    '==========================================================

    ''' <summary>
    ''' Represents the optional aggregation context for a CalculatedValue.
    '''
    ''' NORMA uses this to indicate the binding scope for an aggregate.
    ''' In the examples we have seen, this contains a PathRoot reference.
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


    ''' <summary>
    ''' Represents a PathRoot reference inside an AggregationContext.
    '''
    ''' Example:
    '''   <PathRoot ref="_B9972D2C-2A88-4BA9-8278-53AAA219A65A" />
    ''' </summary>
    <Serializable>
    <XmlRoot("PathRoot", Namespace:="orm")>
    Public Class PathRootReference

        ''' <summary>
        ''' Reference to the RootObjectType id within the derivation path.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    '==========================================================
    '  core:Inputs / Input / Parameter / Source
    '==========================================================

    ''' <summary>
    ''' Container for the ordered arguments supplied to a CalculatedValue.
    ''' </summary>
    <Serializable>
    <XmlRoot("Inputs", Namespace:="orm")>
    Public Class Inputs

        ''' <summary>
        ''' Ordered list of input arguments.
        ''' </summary>
        <XmlElement("Input")>
        Public Property Input As New List(Of Input)

    End Class


    ''' <summary>
    ''' A single input argument to a CalculatedValue function call.
    ''' </summary>
    <Serializable>
    <XmlRoot("Input", Namespace:="orm")>
    Public Class Input

        ''' <summary>
        ''' Unique id of this input node.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Reference to the function parameter this input satisfies.
        ''' </summary>
        <XmlElement("Parameter")>
        Public Property Parameter As ParameterRef

        ''' <summary>
        ''' Source expression supplying the value for this parameter.
        ''' </summary>
        <XmlElement("Source")>
        Public Property Source As Source

    End Class


    ''' <summary>
    ''' Reference to a function parameter.
    ''' </summary>
    <Serializable>
    <XmlRoot("Parameter", Namespace:="orm")>
    Public Class ParameterRef

        ''' <summary>
        ''' Reference to the Parameter id.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    ''' <summary>
    ''' Source is a choice node: exactly one of:
    '''   - PathedRole ref
    '''   - CalculatedValue ref
    '''   - Constant
    ''' </summary>
    <Serializable>
    <XmlRoot("Source", Namespace:="orm")>
    Public Class Source

        ''' <summary>
        ''' The single selected source expression node.
        ''' </summary>
        <XmlElement("PathRoot", GetType(PathRootReference))>
        <XmlElement("PathedRole", GetType(PathedRoleRef))>
        <XmlElement("CalculatedValue", GetType(CalculatedValueRef))>
        <XmlElement("Constant", GetType(Constant))>
        Public Property Item As Object

    End Class


    ''' <summary>
    ''' Reference to a PathedRole used as an input source.
    ''' </summary>
    <Serializable>
    <XmlRoot("PathedRole", Namespace:="orm")>
    Public Class PathedRoleRef

        ''' <summary>
        ''' Reference to the PathedRole id.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    ''' <summary>
    ''' Reference to another CalculatedValue used as an input source.
    ''' </summary>
    <Serializable>
    <XmlRoot("CalculatedValue", Namespace:="orm")>
    Public Class CalculatedValueRef

        ''' <summary>
        ''' Reference to the CalculatedValue id.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    ''' <summary>
    ''' Literal constant value used as an input source.
    ''' </summary>
    <Serializable>
    <XmlRoot("Constant", Namespace:="orm")>
    Public Class Constant

        ''' <summary>
        ''' Unique id of the constant node.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Literal value of the constant.
        ''' </summary>
        <XmlElement("Value")>
        Public Property Value As String

    End Class

End Namespace