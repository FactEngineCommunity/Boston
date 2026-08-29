Imports System.Xml.Serialization

Namespace FBM

    '==========================================================
    '  core:DerivationRule
    '==========================================================
    ''' <summary>
    ''' The root container for a NORMA derivation, attached to a Fact Type.
    '''
    ''' A DerivationRule typically contains a single FactTypeDerivationPath.
    ''' That path is the "query plan" describing how NORMA derives the fact.
    '''
    ''' In narrative terms, the rule says:
    '''   "This Fact Type holds if and only if there exists a walk through other Fact Types
    '''    (SubPaths) satisfying the Conditions, and projecting results to the derived roles."
    '''    
    '''   FactType.DerivationRule
    '''     -> FactTypeDerivationPath (the "query plan" describing how the fact is derived)
    '''
    ''' A derived Fact Fype is typically rendered as:
    '''   "*[Head Fact Type Reading] if and only if [body readings] where [conditions]"
    '''
    ''' Example target output (from multiple SubPaths + conditions):
    '''   *Person lives in Country if and only if
    '''     that Person lives in some City
    '''     and that City is in that Country
    '''   where City is not Null
    ''' </summary>
    <Serializable>
    Public Class DerivationRule

        ''' <summary>
        ''' The main derivation path for this derived fact.
        '''
        ''' This is the object you walk to find:
        '''   - RolePath.RootObjectType
        '''   - RolePath.SubPaths (Fact traversals)
        '''   - RolePath.CalculatedValues (intermediate values)
        '''   - RolePath.Conditions (constraints)
        '''   - Projections (which values map back to the derived roles)
        ''' </summary>
        <XmlElement("FactTypeDerivationPath")>
        Public Property FactTypeDerivationPath As FactTypeDerivationPath

    End Class

End Namespace
