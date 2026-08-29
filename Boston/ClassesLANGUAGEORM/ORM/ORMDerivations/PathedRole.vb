Imports System.Xml.Serialization
Imports System.Collections.Generic

Namespace FBM

    ''' <summary>
    ''' Reference to a role participating in a SubPath.
    '''
    ''' PathedRole is intentionally tiny:
    '''   - Ref points to the role id in the .orm file (which becomes an FBM.Role in Boston).
    '''   - Purpose is metadata used by NORMA; Boston can initially treat it as advisory.
    '''
    ''' Purpose is often used by NORMA to describe how a role is used in the path:
    '''   - join semantics
    '''   - identification / reference roles
    '''   - "input" vs "output" usage
    '''
    ''' Boston can use Purpose later to improve English rendering and variable binding,
    ''' but it is not required for the first functional importer.
    ''' </summary>
    Public Class PathedRole

        <XmlAttribute("id")>
        Public Property id As String

        ''' <summary>
        ''' Reference id of the role in the NORMA model.
        '''
        ''' Boston resolves this to an FBM.Role using the model's role index.
        ''' From that role, Boston can obtain:
        '''   - owning FactType (role.FactType)
        '''   - joined object type (role.JoinedORMObject)
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

        <XmlAttribute("IsNegated")>
        Public Property IsNegated As Boolean

        ''' <summary>
        ''' NORMA metadata describing the "purpose" of the role in this SubPath.
        '''
        ''' Examples (varies across NORMA versions):
        '''   - "Input"
        '''   - "Output"
        '''   - "Join"
        '''   - "Predicate"
        '''
        ''' Keep as a string because NORMA may emit new values across versions.
        ''' </summary>
        <XmlAttribute("Purpose")>
        Public Property Purpose As String

        <XmlElement("ValueRestriction")>
        Public Property ValueRestriction As PathedRoleValueRestriction

    End Class

    Public Class PathedRoleValueRestriction

        <XmlElement("PathedRoleConditionValueConstraint")>
        Public Property PathedRoleConditionValueConstraint As PathedRoleConditionValueConstraint

    End Class

    Public Class PathedRoleConditionValueConstraint

        <XmlAttribute("id")>
        Public Property id As String

        <XmlArray("ValueRanges")>
        <XmlArrayItem("ValueRange")>
        Public Property ValueRange As New List(Of PathedRoleValueRange)

    End Class

    Public Class PathedRoleValueRange

        <XmlAttribute("id")>
        Public Property id As String

        <XmlAttribute("MinValue")>
        Public Property MinValue As String

        <XmlAttribute("InvariantMinValue")>
        Public Property InvariantMinValue As String

        <XmlAttribute("MaxValue")>
        Public Property MaxValue As String

        <XmlAttribute("InvariantMaxValue")>
        Public Property InvariantMaxValue As String

        <XmlAttribute("MinInclusion")>
        Public Property MinInclusion As String

        <XmlAttribute("MaxInclusion")>
        Public Property MaxInclusion As String

    End Class

End Namespace
