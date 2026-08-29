Imports System.Xml.Serialization

Namespace FBM

    ''' <summary>
    ''' Represents a single NORMA derivation projection within a Fact Type derivation path.
    '''
    ''' A DerivationProjection maps the outputs of one internal derivation path
    ''' back to the roles of the derived Fact Type.
    '''
    ''' In NORMA's .orm XML, this element typically:
    '''   - has its own unique id
    '''   - references the RolePath it projects from
    '''   - contains one or more RoleProjection elements
    '''
    ''' Example:
    '''   Derived Fact Type: Predicate has Arity
    '''   Internal derivation path starts at Predicate and calculates Arity
    '''   The DerivationProjection maps:
    '''       Predicate role  - PathRoot
    '''       Arity role      - CalculatedValue  
    ''' Boston usage:
    '''   - preserve the imported NORMA derivation structure faithfully
    '''   - support rendering of derived Fact Types imported from .orm files
    '''   - resolve which internal bound values populate which derived roles
    ''' </summary>
    Public Class DerivationProjection

        ''' <summary>
        ''' Unique id of this derivation projection within the .orm file.
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
    ''' In NORMA's .orm XML:
    '''   - @id identifies this role projection node
    '''   - @ref points to the derived Fact Type's Role id
    '''   - DerivationSource indicates where the role's value comes from
    '''
    ''' Example:
    '''   For the derived Fact Type:
    '''       Predicate has Arity
    '''
    '''   One RoleProjection may map:
    '''       Predicate role - PathRoot
    '''
    '''   Another RoleProjection may map:
    '''       Arity role - CalculatedValue
    ''' </summary>
    Public Class RoleProjection

        ''' <summary>
        ''' Unique id of this role projection within the .orm file.
        ''' Useful for debugging, tracing and cross-referencing.
        ''' </summary>
        <XmlAttribute("id")>
        Public Property Id As String

        ''' <summary>
        ''' Reference to the derived Fact Type role being populated.
        '''
        ''' This should point to a Role id in the derived Fact Type's FactRoles collection.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

        ''' <summary>
        ''' The source of the value assigned to the referenced derived role.
        '''
        ''' NORMA commonly uses:
        '''   - PathRoot
        '''   - CalculatedValue
        '''
        ''' This structure is important because the derived Fact Type role
        ''' is not always populated directly from a traversed role.
        ''' It may come from an aggregate or other computed expression.
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
    ''' This allows a RoleProjection to say where its output value comes from.
    '''
    ''' Example:
    '''   <DerivationSource>
    '''       <PathRoot ref="..." />
    '''   </DerivationSource>
    '''
    ''' Example:
    '''   <DerivationSource>
    '''       <CalculatedValue ref="..." />
    '''   </DerivationSource>
    '''
    ''' Boston usage:
    '''   - determine whether a projected role is bound from the derivation root
    '''   - determine whether a projected role is bound from a calculated value
    '''   - preserve NORMA's structure faithfully for rendering and round-tripping
    ''' </summary>
    Public Class DerivationSource

        ''' <summary>
        ''' Optional PathRoot source.
        '''
        ''' When present, the projected role is populated from the derivation's root binding.
        ''' The ref points to the RootObjectType id within the RolePath.
        ''' </summary>
        <XmlElement("PathRoot")>
        Public Property PathRoot As PathRootReference

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
        ''' Returns True when this DerivationSource has no recognised source node populated.
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property IsEmpty As Boolean
            Get
                Return PathRoot Is Nothing AndAlso CalculatedValue Is Nothing
            End Get
        End Property

        ''' <summary>
        ''' Returns the reference id of the populated source node, if any.
        '''
        ''' This is a convenience property for importer and renderer code.
        ''' </summary>
        <XmlIgnore>
        Public ReadOnly Property SourceRef As String
            Get
                If PathRoot IsNot Nothing Then
                    Return PathRoot.Ref
                End If


                If PathedRole IsNot Nothing Then
                    Return PathedRole.Ref
                End If

                If CalculatedValue IsNot Nothing Then
                    Return CalculatedValue.Ref
                End If

                Return Nothing
            End Get
        End Property

    End Class


    ''' <summary>
    ''' Represents a NORMA PathRoot reference inside a DerivationSource.
    '''
    ''' This is used when a projected role is populated directly from the root object
    ''' of the derivation path.
    '''
    ''' Example:
    '''   <PathRoot ref="_B9972D2C-2A88-4BA9-8278-53AAA219A65A" />
    ''' </summary>
    Public Class PathRootReference

        ''' <summary>
        ''' Reference to the RootObjectType id within the derivation path.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class


    ''' <summary>
    ''' Represents a NORMA CalculatedValue reference inside a DerivationSource.
    '''
    ''' This is used when a projected role is populated from a calculated value
    ''' produced by the derivation path.
    '''
    ''' Example:
    '''   <CalculatedValue ref="_FA2CB72D-FDC8-4D85-B4EC-973431AC4BB7" />
    ''' </summary>
    Public Class CalculatedValueReference

        ''' <summary>
        ''' Reference to the CalculatedValue id within the derivation path.
        ''' </summary>
        <XmlAttribute("ref")>
        Public Property Ref As String

    End Class

End Namespace