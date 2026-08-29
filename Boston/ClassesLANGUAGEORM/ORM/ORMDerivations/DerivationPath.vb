Imports System.Xml.Serialization

Namespace FBM

    ''' <summary>
    ''' A derivation path for a derived Fact Type.
    '''
    ''' NORMA names and ids these paths so they can be referenced/reused internally.
    '''
    ''' Key components:
    '''   - PathComponents.RolePath: describes the traversal and constraints.
    '''   - Projections: describes what is returned to fill the derived fact's roles.
    '''
    ''' Example (high-level):
    '''   Derived: Person lives in Country
    '''   Body:
    '''     Person lives in City
    '''     City is in Country
    '''   Projection:
    '''     project Person and Country (so the derived fact has those two roles)
    ''' </summary>
    Public Class FactTypeDerivationPath

        ''' <summary>
        ''' Unique id of this derivation path within the .orm file.
        ''' Useful for debugging, caching and cross-referencing.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Optional name of the derivation path.
        ''' NORMA may supply a name when the user names the derivation.
        ''' </summary>
        <XmlAttribute("Name")>
        Public Property Name As String

        ''' <summary>
        ''' The core "plan" of the derivation: RolePath, which contains SubPaths, Conditions, etc.
        ''' </summary>
        <XmlElement("PathComponents")>
        Public Property PathComponents As PathComponents

        ''' <summary>
        ''' Projection list describing what values are output from the derivation.
        '''
        ''' This is important when the derivation internally traverses several fact types and
        ''' binds many variables; the projections specify which variables correspond to the
        ''' roles of the derived Fact Type.
        ''' 
        ''' Container for derivation projections.
        '''
        ''' Projections describe which bound values are output from the derivation.
        ''' They map the derivation's internal variable bindings back to the roles
        ''' of the derived Fact Type.
        '''
        ''' Example:
        '''   Derived: Person lives in Country
        '''   Internal bindings: Person, City, Country
        '''   Projection chooses: Person, Country
        '''
        ''' Example:
        '''   Derived fact: Person lives in Country (IS WHERE Person lives in City THAT is in Country)
        '''   Derivation binds: Person, City, Country
        '''   Projection refs will identify "Person" and "Country" (and not "City").
        ''' </summary>
        <XmlArray("DerivationProjections")>
        <XmlArrayItem("DerivationProjection")>
        Public Property Projection As New List(Of DerivationProjection)

    End Class

End Namespace