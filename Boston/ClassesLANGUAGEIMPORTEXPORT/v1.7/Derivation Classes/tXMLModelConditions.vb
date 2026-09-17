Imports System.Xml.Serialization

Namespace XMLModel

    ''' <summary>
    ''' Container for condition expression nodes in a RolePath.
    '''
    ''' Conditions restrict the values bound by SubPaths.
    '''
    ''' Examples (conceptual):
    '''   - NOT(IsNull(City))
    '''   - Role1 &lt;&gt; Role2
    '''   - Position(RoleX) = 1
    '''
    ''' The exact XML shapes vary. We capture raw nodes first and later convert them
    ''' into a typed expression tree.
    ''' </summary>
    <Serializable>
    Public Class Conditions

        ''' <summary>
        ''' Raw XML nodes representing the condition expressions.
        '''
        ''' Often the top-level nodes are logical operators such as:
        '''   &lt;And&gt; ... &lt;/And&gt;
        '''   &lt;Or&gt;  ... &lt;/Or&gt;
        '''   &lt;Not&gt; ... &lt;/Not&gt;
        '''
        ''' Or direct comparisons:
        '''   &lt;Equals&gt; ... &lt;/Equals&gt;
        '''   &lt;NotEquals&gt; ... &lt;/NotEquals&gt;
        '''
        ''' Boston typically renders these as a trailing:
        '''   "where X and Y"
        ''' and uses NOT(...) only when it improves readability.
        ''' 
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
    <XmlRoot("And")>
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
    <XmlRoot("Or")>
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
    <XmlRoot("Not")>
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
    <XmlRoot("Equals")>
    Public Class ConditionEquals
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("NotEquals")>
    Public Class ConditionNotEquals
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("GreaterThan")>
    Public Class ConditionGreaterThan
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("GreaterThanOrEqual")>
    Public Class ConditionGreaterThanOrEqual
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("LessThan")>
    Public Class ConditionLessThan
        Inherits ConditionBinaryComparison
    End Class


    <Serializable>
    <XmlRoot("LessThanOrEqual")>
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