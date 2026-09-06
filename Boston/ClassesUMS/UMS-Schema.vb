Imports System.Collections.Generic
Imports System.IO
Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

' ─── Enumerations ─────────────────────────────────────────────────────────────

Public Enum DataType
    Integer
    DateTime
    TextFixedLength
    TextVariableLength
End Enum

Public Enum Constraint
    NotNull
    Unique
End Enum

' ─── Domain Classes ───────────────────────────────────────────────────────────

Public Class TypeDefinition

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
    Public Property Label As String

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

    Public Property TypeReadings As List(Of String)

    ''' <summary>
    ''' Returns True if this type is a relationship type (has RelationshipAnnotation)
    ''' </summary>
    Public ReadOnly Property IsRelationshipType As Boolean
        Get
            Return Not String.IsNullOrEmpty(RelationshipAnnotation)
        End Get
    End Property

End Class


Public Class PropertyDefinition

    ''' <summary>
    ''' The property name e.g. Id, Name, CinemaId
    ''' </summary>
    Public Property Name As String

    ''' <summary>
    ''' Optional alias / role name e.g. "Cinema Name", "Film Title"
    ''' </summary>
    Public Property Alias As String

    ''' <summary>
    ''' Data type: Integer, DateTime, TextFixedLength, TextVariableLength
    ''' </summary>
    Public Property DataType As DataType

    ''' <summary>
    ''' Length for TextFixedLength properties e.g. 100, 200, 1
    ''' Null for all other data types
    ''' </summary>
    Public Property Length As Integer?

    ''' <summary>
    ''' Constraints e.g. [NOT NULL, UNIQUE]
    ''' </summary>
    Public Property Constraints As List(Of Constraint)

    Public Property Readings As List(Of String)

End Class


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

    ''' <summary>
    ''' Source key columns e.g. [CinemaId] or [CinemaId, FilmId, DateTime]
    ''' </summary>
    Public Property From As List(Of String)

    ''' <summary>
    ''' Target reference string e.g. "Cinema [Id]" or "Row [CinemaId, RowNr]"
    ''' </summary>
    <YamlMember(Alias:="To")>
    Public Property [To] As String

    Public Property Readings As List(Of String)

End Class


' ─── Custom Type Converters ───────────────────────────────────────────────────

Public Class DataTypeConverter
    Implements IYamlTypeConverter

    Public Function Accepts(type As Type) As Boolean Implements IYamlTypeConverter.Accepts
        Return type = GetType(DataType)
    End Function

    Public Function ReadYaml(parser As IParser, type As Type) As Object Implements IYamlTypeConverter.ReadYaml
        Dim scalar = parser.Consume(Of YamlDotNet.Core.Events.Scalar)()
        Select Case scalar.Value
            Case "Integer"           : Return DataType.Integer
            Case "DateTime"          : Return DataType.DateTime
            Case "TextFixedLength"   : Return DataType.TextFixedLength
            Case "TextVariableLength": Return DataType.TextVariableLength
            Case Else
                Throw New YamlDotNet.Core.YamlException($"Unknown DataType: '{scalar.Value}'")
        End Select
    End Function

    Public Sub WriteYaml(emitter As IEmitter, value As Object, type As Type) Implements IYamlTypeConverter.WriteYaml
        Dim dt = CType(value, DataType)
        Dim text = dt.ToString()
        emitter.Emit(New YamlDotNet.Core.Events.Scalar(text))
    End Sub

End Class


Public Class ConstraintConverter
    Implements IYamlTypeConverter

    Public Function Accepts(type As Type) As Boolean Implements IYamlTypeConverter.Accepts
        Return type = GetType(Constraint)
    End Function

    Public Function ReadYaml(parser As IParser, type As Type) As Object Implements IYamlTypeConverter.ReadYaml
        Dim scalar = parser.Consume(Of YamlDotNet.Core.Events.Scalar)()
        Select Case scalar.Value
            Case "NOT NULL": Return Constraint.NotNull
            Case "UNIQUE"  : Return Constraint.Unique
            Case Else
                Throw New YamlDotNet.Core.YamlException($"Unknown Constraint: '{scalar.Value}'")
        End Select
    End Function

    Public Sub WriteYaml(emitter As IEmitter, value As Object, type As Type) Implements IYamlTypeConverter.WriteYaml
        Dim c = CType(value, Constraint)
        Dim text = If(c = Constraint.NotNull, "NOT NULL", "UNIQUE")
        emitter.Emit(New YamlDotNet.Core.Events.Scalar(text))
    End Sub

End Class


' ─── Serialiser Factory ───────────────────────────────────────────────────────

Public Class UmsSchemaSerializer

    Public Shared Function BuildDeserializer() As IDeserializer
        Return New DeserializerBuilder() _
            .WithNamingConvention(PascalCaseNamingConvention.Instance) _
            .WithTypeConverter(New DataTypeConverter()) _
            .WithTypeConverter(New ConstraintConverter()) _
            .IgnoreUnmatchedProperties() _
            .Build()
    End Function

    Public Shared Function BuildSerializer() As ISerializer
        Return New SerializerBuilder() _
            .WithNamingConvention(PascalCaseNamingConvention.Instance) _
            .WithTypeConverter(New DataTypeConverter()) _
            .WithTypeConverter(New ConstraintConverter()) _
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull Or
                DefaultValuesHandling.OmitEmptyCollections) _
            .Build()
    End Function

    ''' <summary>
    ''' Load a UMS schema from a YAML file.
    ''' </summary>
    Public Shared Function LoadFromFile(filePath As String) As List(Of TypeDefinition)
        Dim yaml As String = File.ReadAllText(filePath)
        Return LoadFromString(yaml)
    End Function

    ''' <summary>
    ''' Load a UMS schema from a YAML string.
    ''' </summary>
    Public Shared Function LoadFromString(yaml As String) As List(Of TypeDefinition)
        Return BuildDeserializer().Deserialize(Of List(Of TypeDefinition))(yaml)
    End Function

    ''' <summary>
    ''' Serialise a UMS schema to a YAML string.
    ''' </summary>
    Public Shared Function SaveToString(schema As List(Of TypeDefinition)) As String
        Return BuildSerializer().Serialize(schema)
    End Function

    ''' <summary>
    ''' Serialise a UMS schema to a YAML file.
    ''' </summary>
    Public Shared Sub SaveToFile(schema As List(Of TypeDefinition), filePath As String)
        File.WriteAllText(filePath, SaveToString(schema))
    End Sub

End Class
