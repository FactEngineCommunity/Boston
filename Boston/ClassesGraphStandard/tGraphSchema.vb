Imports Newtonsoft.Json
Imports System.Collections.Generic

'https://neo4j.com/developer-blog/describing-property-graph-data-model/
'https://medium.com/neo4j/describing-a-property-graph-data-model-d203c389546e

Namespace GraphStandard
    Public Class GraphSchemaRepresentation
        Public Property GraphSchema As GraphSchema
    End Class

    Public Class GraphSchema
        Public Property NodeLabels As New List(Of NodeLabel)
        Public Property RelationshipTypes As New List(Of RelationshipType)
        Public Property NodeObjectTypes As New List(Of NodeObjectType)
        Public Property RelationshipObjectTypes As New List(Of RelationshipObjectType)
    End Class

    'GraphSchemaRepresentation
    '    └─ GraphSchema
    '        ├─ NodeLabels
    '        │   └─ NodeLabel
    '        │       ├─ id
    '        │       └─ Token
    '        ├─ RelationshipTypes
    '        │   └─ RelationshipType
    '        │       ├─ id
    '        │       └─ Token
    '        ├─ NodeObjectTypes
    '        │   └─ NodeObjectType
    '        │       ├─ id
    '        │       ├─ Labels
    '        │       │   └─ NodeLabelRef
    '        │       │       └─ ref
    '        │       └─ Properties
    '        │           └─ NodeProperty
    '        │               ├─ Token
    '        │               ├─ Type
    '        │               │   └─ NodeType
    '        │               │       └─ Type
    '        │               └─ Nullable
    '        └─ RelationshipObjectTypes
    '            └─ RelationshipObjectType
    '                ├─ id
    '                ├─ Type
    '                │   └─ RelationshipTypeRef
    '                │       └─ ref
    '                ├─ From
    '                │   └─ RelationshipNodeRef
    '                │       └─ ref
    '                ├─ To
    '                │   └─ RelationshipNodeRef
    '                │       └─ ref
    '                └─ Properties
    '                    └─ RelationshipProperty
    '                        ├─ Token
    '                        ├─ Type
    '                        │   ├─ Type
    '                        │   └─ Items
    '                        │       └─ RelationshipPropertyItemType
    '                        │           └─ Type
    '                        └─ Nullable

    '
    ' {
    ' "graphSchemaRepresentation": {
    ' "graphSchema": {
    ' "nodeLabels": [
    ' { "$id": "nl:Person", "token": "Person" },
    ' { "$id": "nl:Actor", "token": "Actor" },
    ' { "$id": "nl:Director", "token": "Director" },
    ' { "$id": "nl:Movie", "token": "Movie" }
    ' ],
    ' "relationshipTypes": [
    ' { "$id": "rt:ACTED_IN", "token": "ACTED_IN" },
    ' { "$id": "rt:DIRECTED", "token": "DIRECTED" }
    ' ],    ' "nodeObjectTypes": [
    ' {
    ' "$id": "n:Person",
    ' "labels": [{ "$ref": "#nl:Person" }],
    ' "properties": [
    ' {
    ' "token": "born",
    ' "type": { "type": "integer" },
    ' "nullable": false
    ' },
    ' { "token": "name", "type": { "type": "string" }, "nullable": false }
    ' ]
    ' },
    ' {
    ' "$id": "n:Movie",
    ' "labels": [{ "$ref": "#nl:Movie" }],
    ' "properties": [
    ' {
    ' "token": "title",
    ' "type": { "type": "string" },
    ' "nullable": false
    ' },
    ' {
    ' "token": "release",
    ' "type": { "type": "date" },
    ' "nullable": false
    ' }
    ' ]
    ' }
    ' ],
    ' "relationshipObjectTypes": [
    ' {
    ' "$id": "r:ACTED_IN",
    ' "type": { "$ref": "#rt:ACTED_IN" },
    ' "from": { "$ref": "#n:Actor:Person" },
    ' "to": { "$ref": "#n:Movie" },
    ' "properties": [
    ' {
    ' "token": "roles",
    ' "type": { "type": "array", "items": { "type": "string" } },
    ' "nullable": false
    ' }
    ' ]
    ' }
    ' ]
    ' }
    ' }
    ' }

End Namespace