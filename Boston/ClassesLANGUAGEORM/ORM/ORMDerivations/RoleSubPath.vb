Imports System.Xml.Serialization

Namespace FBM

    ''' <summary>
    ''' A single path segment inside a RolePath.
    '''
    ''' A SubPath contains PathedRoles. Those roles identify:
    '''   - the FactType traversed
    '''   - the role order needed to select the correct FactTypeReading
    '''
    ''' Typical SubPath usage:
    '''   - A SubPath for a binary fact will have two PathedRoles.
    '''   - A SubPath for a ternary fact will have three PathedRoles.
    '''   - SubPaths can also represent joins through objectified facts and subtype paths.
    ''' </summary>
    Public Class RoleSubPath

        ''' <summary>
        ''' Unique id for this SubPath. Useful for tracing back conditions or debugging.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String


        <XmlAttribute("SplitCombinationOperator")>
        Public Property SplitCombinationOperator As String


        ''' <summary>
        ''' Defines the starting Object Type for a SubPath within a derivation RolePath.
        '''
        ''' Example scenario (NORMA derivation):
        '''
        ''' Derived Fact Type
        ''' -----------------
        ''' Person lives in birth Country
        '''
        ''' Reading
        ''' -------
        ''' Person lives in birth Country
        ''' IS WHERE
        ''' Person lives in City
        ''' THAT is in Country1
        ''' AND
        ''' Person born in Country2
        ''' AND Country1 = Country2
        '''
        ''' Why a SubPath RootObjectType is needed
        ''' --------------------------------------
        ''' There are two logical Country variables in the derivation:
        '''
        '''     Country1  ← reached through the City path
        '''     Country2  ← reached through the Birth path
        '''
        ''' NORMA introduces a SubPath with its own RootObjectType in order to
        ''' begin a second traversal of the model that represents the second
        ''' logical variable.
        '''
        ''' Conceptual Path Tree
        ''' --------------------
        ''' RootObjectType: Person
        '''
        ''' RolePath
        '''  ├─ Person lives in City
        '''  │   └─ City is in Country (Country1)
        '''
        '''  └─ SubPath
        '''      RootObjectType: Person
        '''      └─ Person born in Country (Country2)
        '''
        ''' The two Country variables are later unified through an ObjectUnifier
        ''' which expresses the constraint:
        '''
        '''     Country1 = Country2
        '''
        ''' In query terms, the SubPath.RootObjectType functions like introducing
        ''' a new table alias in SQL so that a second logical instance of the same
        ''' object type can participate in the derivation.
        ''' </summary>
        <XmlElement("RootObjectType")>
        Public Property RootObjectType As FBM.RootObjectType

        ''' <summary>
        ''' The ordered list of roles that are traversed in this SubPath.
        '''
        ''' Boston uses these refs to fetch FBM.Role objects from the model.
        ''' Then it calls:
        '''    FactType.getFactTypeReadingByRoleSequence(larRole)
        ''' to obtain the reading whose predicate parts match this role order.
        ''' 
        ''' Ordered role references comprising this SubPath.
        '''
        ''' Ordering matters because it selects the correct TypedPredicate / FactTypeReading.
        ''' For example, a binary fact may have both:
        '''   - "Person drives Car"
        '''   - "Car is driven by Person"
        ''' These have the same underlying FactType but different role sequences/readings.
        ''' </summary>
        <XmlArray("PathedRoles")>
        <XmlArrayItem("PathedRole")>
        Public Property PathedRole As New List(Of PathedRole)

        <XmlArray("SubPaths")>
        <XmlArrayItem("SubPath")>
        Public Property SubPath As New List(Of RoleSubPath)

    End Class

End Namespace