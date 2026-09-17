Imports System.Xml.Serialization

Namespace XMLModel

    ''' <summary>
    ''' Represents a single NORMA-style derivation projection within a Fact Type derivation path.
    '''
    ''' A DerivationProjection maps the outputs of one internal derivation path
    ''' back to the roles of the derived Fact Type.
    '''
    ''' In NORMA's .orm XML, this element typically:
    '''   - has its own unique id
    '''   - references the RolePath it projects from
    '''   - contains one or more RoleProjection elements
    '''
    ''' Boston usage:
    '''   - persist imported derivation projections in .fbm files
    '''   - support correct rendering of imported derivations
    '''   - preserve role-to-source bindings (PathRoot, CalculatedValue, etc.)
    ''' </summary>
    <Serializable>
    Public Class DerivationProjection

        ''' <summary>
        ''' Unique id of this derivation projection.
        ''' Useful for debugging, tracing and cross-referencing.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Reference to the path component being projected.
        '''
        ''' In NORMA this commonly points to the RolePath id that produced
        ''' the internal bindings used by this projection.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

        ''' <summary>
        ''' The individual role mappings for this projection.
        '''
        ''' Each RoleProjection maps one role of the derived Fact Type
        ''' to its derivation source.
        ''' </summary>
        <XmlElement("RoleProjection")>
        Public Property RoleProjection As New List(Of RoleProjection)

    End Class


    ''' <summary>
    ''' Represents one projected role within a DerivationProjection.
    '''
    ''' A RoleProjection maps a specific role of the derived Fact Type
    ''' to the source that supplies its value.
    '''
    ''' In NORMA:
    '''   - @ref points to the derived Fact Type Role id
    '''   - DerivationSource indicates where the role's value comes from
    ''' </summary>
    <Serializable>
    Public Class RoleProjection

        ''' <summary>
        ''' Unique id of this role projection.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Reference to the derived Fact Type Role id being populated.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

        ''' <summary>
        ''' Wrapper node that contains the actual source element
        ''' used to populate the projected role.
        ''' </summary>
        <XmlElement("DerivationSource")>
        Public Property DerivationSource As DerivationSource

    End Class


    ''' <summary>
    ''' Represents the source expression used to populate a projected role.
    '''
    ''' NORMA wraps the actual source inside a DerivationSource element.
    ''' The contained source is usually one of:
    '''   - PathRoot
    '''   - CalculatedValue
    '''
    ''' Boston usage:
    '''   - preserve which source populates a derived role
    '''   - enable faithful round-tripping of derivations
    ''' </summary>
    <Serializable>
    Public Class DerivationSource

        ''' <summary>
        ''' Optional PathRoot source.
        '''
        ''' When present, the projected role is populated from the derivation's root binding.
        ''' The ref points to the RootObjectType id within the RolePath.
        ''' </summary>
        <XmlElement("PathRoot")>
        Public Property PathRoot As PathRootReference

        ''' <summary>
        ''' Optional PathedRole source.
        '''
        ''' When present, the projected role is populated from a PathedRole
        ''' binding within the derivation path.
        ''' The ref points to a PathedRole id.
        ''' </summary>
        <XmlElement("PathedRole")>
        Public Property PathedRole As PathedRoleReference

        ''' <summary>
        ''' Optional CalculatedValue source.
        '''
        ''' When present, the projected role is populated from a calculated value
        ''' defined within the derivation path.
        ''' The ref points to a CalculatedValue id.
        ''' </summary>
        <XmlElement("CalculatedValue")>
        Public Property CalculatedValue As CalculatedValueReference

        ''' <summary>
        ''' Convenience: returns the ref of the populated source node, if any.
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property SourceRef As String
            Get
                If PathRoot IsNot Nothing Then Return PathRoot.Ref
                If CalculatedValue IsNot Nothing Then Return CalculatedValue.Ref
                If PathedRole IsNot Nothing Then Return PathedRole.Ref
                Return Nothing
            End Get
        End Property

    End Class

    '20260306-VM-See inside CalculatedValue
    '''' <summary>
    '''' Represents a PathRoot reference inside a DerivationSource.
    '''' </summary>
    '<Serializable>
    'Public Class PathRootReference

    '    ''' <summary>
    '    ''' Reference to the RootObjectType id within the derivation path.
    '    ''' </summary>
    '    <XmlAttribute("ref")>
    '    Public Property Ref As String

    'End Class

    ''' <summary>
    ''' Represents a CalculatedValue reference inside a DerivationSource.
    ''' </summary>
    <Serializable>
    Public Class CalculatedValueReference

        ''' <summary>
        ''' Reference to the CalculatedValue id within the derivation path.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class

End Namespace