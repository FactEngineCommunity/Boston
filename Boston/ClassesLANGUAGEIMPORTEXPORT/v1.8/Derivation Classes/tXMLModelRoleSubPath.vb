Imports System.Xml.Serialization

Namespace XMLModel

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
    <Serializable>
    Public Class RoleSubPath

        ''' <summary>
        ''' Unique id for this SubPath. Useful for tracing back conditions or debugging.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String


        <XmlAttribute("SplitCombinationOperator")>
        Public Property SplitCombinationOperator As String


        <XmlElement>
        Public Property RootObjectType As XMLModel.RootObjectType

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