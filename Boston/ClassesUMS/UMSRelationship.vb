Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS

    Public Class RelationshipDefinition

        ''' <summary>
        ''' FK or relationship name e.g. FK_Session_Cinema, PersonLikesFilm_Person
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' Graph label e.g. IS_AT, IS_FOR, IS_IN
        ''' Optional — relationship types may omit the label on their FK entries
        ''' </summary>
        Public Property Label As String


        Public Property Source As String 'E.g. Person in Person-LIKES->Film

        Public Property Target As String 'E.g. Film in Person-LIKES->Film

        ''' <summary>
        ''' Source key columns e.g. [CinemaId] or [CinemaId, FilmId, DateTime]
        ''' </summary>
        Public Property From As New List(Of String)

        ''' <summary>
        ''' Target reference string e.g. "Cinema [Id]" or "Row [CinemaId, RowNr]"
        ''' </summary>
        <YamlMember(Alias:="To")>
        Public Property [To] As New List(Of String)

        ''' <summary>
        ''' For Document creation, when a relationship is embedded. E.g. Cinema contains Row (all of the Rows are put in the Cinema object definition).
        ''' I.e. Embeds from/within the Source, and where 'Cinema' is the Source in the example above (and Row the Target).
        ''' </summary>
        ''' <returns></returns>
        Public Property Embed As Boolean?

        ''' <summary>
        ''' For Embedded (above) relationships. Is the Cardinality of the effective list (One, Many, ZeroOrMore).
        ''' </summary>
        ''' <returns></returns>
        Public Property Cardinality As UMS.Cardinality?

        Public Property Readings As New List(Of String)

        Public Property Facts As List(Of UMS.Facts)

    End Class

End Namespace