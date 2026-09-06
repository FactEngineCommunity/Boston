Imports System.Collections.Generic
Imports System.IO
Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS


    ' ─── Type Definition ───────────────────────────────────────────────────────────

    Public Class TypeDefinition
        Inherits UMS.ModelElement

        ''' <summary>
        ''' The name of the type e.g. Cinema, PersonLikesFilm
        ''' Maps to the YAML key "Type"
        ''' </summary>
        <YamlMember(Alias:="Type")>
        Public Property Name As String

        ''' <summary>
        ''' Graph labels for entity types e.g. [Cinema]
        ''' </summary>
        Public Property Labels As List(Of String)


        ''' <summary>
        ''' Single graph label for relationship types e.g. LIKES
        ''' </summary>
        Public Property Label As String = Nothing

        ''' <summary>
        ''' The Source type  (e.g. Person) if the Type is a Relationship. E.g. Is a Many-to-Many Table as for Person-LIKES->Film.
        ''' Otherwise Nothing/NULL.        
        ''' </summary>
        ''' <returns></returns>
        Public Property Source As String = Nothing

        ''' <summary>
        ''' The Target type  (e.g. Film) if the Type is a Relationship. E.g. Is a Many-to-Many Table as for Person-LIKES->Film.
        ''' Otherwise Nothing/NULL.
        ''' </summary>
        ''' <returns></returns>
        Public Property Target As String = Nothing

        ''' <summary>
        ''' For Document creation, when a relationship is embedded. E.g. "Person likes Film" (all of the Films, that are liked, are put in the Person object definition).
        ''' I.e. Embeds from/within the Source, and where 'Person' is the Source in the example above (and 'Film' the Target). See Source and Target above.
        ''' </summary>
        ''' <returns></returns>
        Public Property Embed As Boolean?

        ''' <summary>
        ''' Relationship annotation for relationship types e.g. "Person -> Film"
        ''' </summary>
        Public Property RelationshipAnnotation As String

        ''' <summary>
        ''' Ordered list of primary key property names
        ''' </summary>
        Public Property PrimaryKey As List(Of String)

        Public Property Properties As List(Of PropertyDefinition)

        Public Property Relationships As List(Of RelationshipDefinition)

        ''' <summary>
        ''' Fact-Based Modelling, Fact Type Readings. Populated if the Type is a Relationship Type. Set of Fact Type Readings by Language.
        ''' </summary>
        ''' <returns></returns>
        Public Property FactTypeReadings As List(Of UMS.FactTypeReadings)

        ''' <summary>
        ''' Fact Based Modelling, Facts. Populated if the Type is a Relationship Type and if there are any Facts stored in the modelling tool. Set of Facts for the Relationship.
        ''' E.g. "Part, 1, in Bin, 'H1', is stored in Warehouse, 'Sydney-01'.
        ''' </summary>
        ''' <returns></returns>
        Public Property Facts As List(Of UMS.Facts)

        Public Property UniquenessConstraints As List(Of UMS.UniquenessConstraint)

        ''' <summary>
        ''' Returns True if this type is a relationship type (has RelationshipAnnotation)
        ''' </summary>
        Public ReadOnly Property IsRelationshipType As Boolean
            Get
                Return Not String.IsNullOrEmpty(RelationshipAnnotation)
            End Get
        End Property

    End Class

End Namespace
