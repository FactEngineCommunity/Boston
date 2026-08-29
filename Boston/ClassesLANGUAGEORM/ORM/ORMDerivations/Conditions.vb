Imports System
Imports System.Collections.Generic
Imports System.Xml.Serialization

Namespace FBM

    '==========================================================
    '  core:Conditions  (typed)
    '==========================================================
    ''' <summary>
    ''' Container for condition expression nodes in a RolePath.
    '''
    ''' NORMA commonly emits logical operators (And/Or/Not) and comparisons
    ''' (Equals/NotEquals/GreaterThan/LessThan/etc), whose operands refer to:
    '''   - PathedRole ref
    '''   - CalculatedValue ref
    '''   - Constant literal
    ''' </summary>
    <Serializable>
    <XmlRoot("Conditions", Namespace:="orm")>
    Public Class Conditions

        ''' <summary>
        ''' The list of top-level condition nodes inside &lt;Conditions&gt;.
        ''' In many models there is only one (often an And/Or wrapper).
        ''' </summary>
        <XmlElement("And", GetType(ConditionAnd))>
        <XmlElement("Or", GetType(ConditionOr))>
        <XmlElement("Not", GetType(ConditionNot))>
        <XmlElement("Equals", GetType(ConditionEquals))>
        <XmlElement("NotEquals", GetType(ConditionNotEquals))>
        <XmlElement("GreaterThan", GetType(ConditionGreaterThan))>
        <XmlElement("GreaterThanOrEqual", GetType(ConditionGreaterThanOrEqual))>
        <XmlElement("LessThan", GetType(ConditionLessThan))>
        <XmlElement("LessThanOrEqual", GetType(ConditionLessThanOrEqual))>
        <XmlElement("CalculatedCondition", GetType(CalculatedCondition))>
        Public Property Items As New List(Of ConditionNode)

    End Class


    '==========================================================
    '  Condition nodes (recursive)
    '==========================================================
    <Serializable>
    Public MustInherit Class ConditionNode
    End Class


    <Serializable>
    <XmlRoot("And", Namespace:="orm")>
    Public Class ConditionAnd
        Inherits ConditionNode

        <XmlElement("And", GetType(ConditionAnd))>
        <XmlElement("Or", GetType(ConditionOr))>
        <XmlElement("Not", GetType(ConditionNot))>
        <XmlElement("Equals", GetType(ConditionEquals))>
        <XmlElement("NotEquals", GetType(ConditionNotEquals))>
        <XmlElement("GreaterThan", GetType(ConditionGreaterThan))>
        <XmlElement("GreaterThanOrEqual", GetType(ConditionGreaterThanOrEqual))>
        <XmlElement("LessThan", GetType(ConditionLessThan))>
        <XmlElement("LessThanOrEqual", GetType(ConditionLessThanOrEqual))>
        <XmlElement("CalculatedCondition", GetType(CalculatedCondition))>
        Public Property Items As New List(Of ConditionNode)
    End Class


    <Serializable>
    <XmlRoot("Or", Namespace:="orm")>
    Public Class ConditionOr
        Inherits ConditionNode

        <XmlElement("And", GetType(ConditionAnd))>
        <XmlElement("Or", GetType(ConditionOr))>
        <XmlElement("Not", GetType(ConditionNot))>
        <XmlElement("Equals", GetType(ConditionEquals))>
        <XmlElement("NotEquals", GetType(ConditionNotEquals))>
        <XmlElement("GreaterThan", GetType(ConditionGreaterThan))>
        <XmlElement("GreaterThanOrEqual", GetType(ConditionGreaterThanOrEqual))>
        <XmlElement("LessThan", GetType(ConditionLessThan))>
        <XmlElement("LessThanOrEqual", GetType(ConditionLessThanOrEqual))>
        <XmlElement("CalculatedCondition", GetType(CalculatedCondition))>
        Public Property Items As New List(Of ConditionNode)
    End Class


    <Serializable>
    <XmlRoot("Not", Namespace:="orm")>
    Public Class ConditionNot
        Inherits ConditionNode

        <XmlElement("And", GetType(ConditionAnd))>
        <XmlElement("Or", GetType(ConditionOr))>
        <XmlElement("Not", GetType(ConditionNot))>
        <XmlElement("Equals", GetType(ConditionEquals))>
        <XmlElement("NotEquals", GetType(ConditionNotEquals))>
        <XmlElement("GreaterThan", GetType(ConditionGreaterThan))>
        <XmlElement("GreaterThanOrEqual", GetType(ConditionGreaterThanOrEqual))>
        <XmlElement("LessThan", GetType(ConditionLessThan))>
        <XmlElement("LessThanOrEqual", GetType(ConditionLessThanOrEqual))>
        <XmlElement("CalculatedCondition", GetType(CalculatedCondition))>
        Public Property Item As ConditionNode
    End Class


    '==========================================================
    '  Comparisons (binary)
    '==========================================================
    <Serializable>
    Public MustInherit Class ConditionBinaryComparison
        Inherits ConditionNode

        <XmlElement("Left")>
        Public Property Left As ConditionValueContainer

        <XmlElement("Right")>
        Public Property Right As ConditionValueContainer
    End Class


    <Serializable>
    <XmlRoot("Equals", Namespace:="orm")>
    Public Class ConditionEquals
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("NotEquals", Namespace:="orm")>
    Public Class ConditionNotEquals
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("GreaterThan", Namespace:="orm")>
    Public Class ConditionGreaterThan
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("GreaterThanOrEqual", Namespace:="orm")>
    Public Class ConditionGreaterThanOrEqual
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("LessThan", Namespace:="orm")>
    Public Class ConditionLessThan
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("LessThanOrEqual", Namespace:="orm")>
    Public Class ConditionLessThanOrEqual
        Inherits ConditionBinaryComparison
    End Class


    '==========================================================
    '  Operand containers + operand types
    '==========================================================
    ''' <summary>
    ''' NORMA frequently wraps an operand inside a named container element
    ''' (Left/Right or Argument/Value etc.). We keep a single container type
    ''' with a choice payload.
    ''' </summary>
    <Serializable>
    Public Class ConditionValueContainer

        <XmlElement("PathedRole", GetType(PathedRoleRef))>
        <XmlElement("CalculatedValue", GetType(CalculatedValueRef))>
        <XmlElement("Constant", GetType(Constant))>
        Public Property Item As Object

    End Class

    <Serializable>
    <XmlRoot("CalculatedCondition", Namespace:="orm")>
    Public Class CalculatedCondition
        Inherits ConditionNode

        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class

End Namespace