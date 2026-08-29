Imports System.Xml.Serialization

Namespace FBM

    '==========================================================
    '  core:RolePath  (the query-plan core)
    '==========================================================
    ''' <summary>
    ''' The core query-plan node for a NORMA derivation.
    '''
    ''' A RolePath should be read as:
    '''   "Start from RootObjectType, traverse a set of SubPaths (role sequences),
    '''    compute CalculatedValues, and enforce Conditions."
    '''
    ''' In Boston, RolePath is interpreted by:
    '''   1) Resolving PathedRole.Ref -> FBM.Role
    '''   2) Deriving FactType from roles
    '''   3) Calling FactType.getFactTypeReadingByRoleSequence(roles)
    '''   4) Rendering the reading in natural language
    '''   5) Rendering conditions as trailing where ... / NOT(...)
    '''
    ''' Example interpretation:
    '''   RootObjectType: Person
    '''   SubPath #1 roles: (Person role, City role) => FactTypeReading "Person lives in City"
    '''   SubPath #2 roles: (City role, Country role) => FactTypeReading "City is in Country"
    '''   Condition: City != Null
    '''   Projection: Person, Country
    ''' </summary>
    Public Class RolePath

        ''' <summary>
        ''' Unique id of the role path within the derivation.
        ''' Useful for debugging and for correlating calculated values / projections.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Operator used when combining multiple SubPaths.
        '''
        ''' Typical values:
        '''   - "And" : SubPaths are conjunctive (all must hold)
        '''   - "Or"  : alternative SubPaths (any may hold)
        '''
        ''' Practical note:
        '''   NORMA derivations commonly act like conjunctions even when this field is not present,
        '''   so treat it as advisory and default to AND behaviour unless you see explicit OR usage.
        ''' </summary>
        <XmlAttribute("SplitCombinationOperator")>
        Public Property SplitCombinationOperator As String

        ''' <summary>
        ''' The "starting object type" for the derivation.
        '''
        ''' In NORMA XML this is normally a reference to an object type (entity type/value type).
        '''
        ''' In Boston, you generally resolve this to an FBM.ModelObject and use it when generating English:
        '''   - "that Person ..."
        '''   - "that Customer ..."
        '''
        ''' The binder logic typically introduces RootObjectType as definite ("that X") to keep the
        ''' derivation sounding natural.
        ''' </summary>
        <XmlElement("RootObjectType")>
        Public Property RootObjectType As FBM.RootObjectType '20260306-VM-Was FBM.ModelObject

        <XmlArray("PathedRoles")>
        <XmlArrayItem("PathedRole")>
        Public Property PathedRole As New List(Of PathedRole)

        ''' <summary>
        ''' The traversals that make up the derivation.
        '''
        ''' Each SubPath is a sequence of PathedRoles. In practice, a SubPath usually corresponds to
        ''' exactly one Fact Type traversal in the derivation body.
        '''
        ''' Boston interpretation:
        '''   SubPath -> roles -> fact type -> getFactTypeReadingByRoleSequence -> render reading sentence.
        ''' </summary>
        <XmlArray("SubPaths")>
        <XmlArrayItem("SubPath")>
        Public Property SubPath As New List(Of RoleSubPath)

        <XmlArray("ObjectUnifiers")>
        <XmlArrayItem("ObjectUnifier")>
        Public Property ObjectUnifier As New List(Of ObjectUnifier)

        ''' <summary>
        ''' Intermediate values computed inside the derivation.
        '''
        ''' NORMA introduces calculated values for many reasons, including:
        '''   - "Position" within a role sequence
        '''   - constant folding / re-use of expression fragments
        '''   - calls to Functions
        '''
        ''' In the minimal importer, these are captured as raw XML (XmlAnyElement) so that we can:
        '''   - deserialize without knowing every schema shape upfront
        '''   - later add a typed expression tree once patterns are confirmed
        '''
        ''' During English generation, CalculatedValues are typically referenced by Conditions or Projections.
        ''' 
        ''' Container for calculated values defined within a RolePath.
        '''
        ''' Calculated values are intermediate expressions which may be referenced by:
        '''   - Conditions
        '''   - Projections
        '''   - other CalculatedValues
        '''
        ''' In NORMA XML, these values can be nested expressions of many shapes.
        ''' We capture them raw (XmlAnyElement) so the importer remains stable while we
        ''' progressively type the expression language.
        ''' </summary>
        <XmlArray("CalculatedValues")>
        <XmlArrayItem("CalculatedValue")>
        Public Property CalculatedValues As New List(Of CalculatedValue)

        ''' <summary>
        ''' Constraint expression nodes for the derivation.
        '''
        ''' Conditions can include:
        '''   - logical operators: And, Or, Not
        '''   - comparisons: Equals, NotEquals, GreaterThan, etc.
        '''   - function calls: IsNull(x), Position(role), Count(...), etc.
        '''   - references to CalculatedValues by id
        '''
        ''' In the minimal importer we capture raw XML nodes so that importing does not depend on
        ''' fully typing NORMA's expression language immediately.
        '''
        ''' English rendering strategy:
        '''   - Prefer compact "where ..." suffix for comparisons.
        '''   - Render NOT(...) inline when it expresses negation.
        '''   - Keep parentheses only when needed.
        ''' </summary>
        <XmlElement("Conditions")>
        Public Property Conditions As Conditions

    End Class

    Public Class RootObjectType

        <XmlAttribute("id")>
        Public Property id As String

        ''' <summary>
        ''' Used only when loading from a .orm NORMA file. Is not stored in Boston. See 'BostonModelElement' below.
        ''' ref = pointer to ObjectType in NORMA .orm file. Boston does not store .ref Ids for ObjectTypes, but uses First-Order Logic Id = Name, where Names are unique across all ObjectTypes in a Model.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("ref")>
        Public Property ref As String

        <XmlIgnore>
        Public BostonModelElement As FBM.ModelObject

        ''' <summary>
        ''' See, "DomainObjectTypeCanBeIndependent", FactType, in FBM1002WD08_with model_20180717.orm
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("IsNegated")>
        Public Property IsNegated As Boolean

    End Class

End Namespace