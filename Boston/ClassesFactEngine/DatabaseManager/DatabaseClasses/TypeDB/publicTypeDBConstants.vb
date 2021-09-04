
Namespace FactEngine.TypeDB

    Module TypeDBConstants

        Public Enum RPCType
            TxType
            TxRequest
            DeleteRequest
            DeleteResponse
            Open
            Commit
            ExecQuery
            RunConceptMethod
            PutAttributeType
            PutRule
            Infer
            TxResponse
            QueryResult
            Done
            Answer
            Keyspace
            Query
        End Enum


        Public Enum BaseType
            Entity = 0
            Relationship = 1
            Attribute = 2
            EntityType = 3
            RelationshipType = 4
            AttributeType = 5
            Role = 6
            Rule = 7
            MetaType = 8
        End Enum

        Public Enum DataType
            [String] = 0
            [Boolean] = 1
            [Integer] = 2
            [Long] = 3
            Float = 4
            [Double] = 5
            [Date] = 6
        End Enum

        Public Enum ConceptMethods
            None = 0
            Delete = 16
            GetLabel = 3
            SetLabel = 18
            IsImplicit = 4
            GetSubConcepts = 19
            GetSuperConcepts = 53
            GetDirectSuperConcept = 14
            SetDirectSuperConcept = 17
            GetWhen = 7
            GetThen = 8
            GetRelationshipTypesThatRelateRole = 20
            GetTypesThatPlayRole = 21
            GetInstances = 30
            GetAttributeTypes = 11
            SetAttributeType = 25
            UnsetAttributeType = 26
            GetKeyTypes = 12
            SetKeyType = 27
            UnsetKeyType = 28
            IsAbstract = 6
            SetAbstract = 22
            GetRolesPlayedByType = 29
            SetRolePlayedByType = 23
            UnsetRolePlayedByType = 24
            AddEntity = 34
            GetRelatedRoles = 36
            SetRelatedRole = 37
            UnsetRelatedRole = 38
            PutAttribute = 32
            GetAttribute = 33
            GetDataTypeOfType = 2
            GetDataTypeOfAttribute = 54
            GetRegex = 9
            SetRegex = 31
            IsInferred = 5
            GetDirectType = 13
            GetRelationships = 39
            GetRelationshipsByRoles = 48
            GetRolesPlayedByThing = 40
            GetAttributes = 41
            GetAttributesByTypes = 49
            GetKeys = 42
            GetKeysByTypes = 50
            SetAttribute = 43
            UnsetAttribute = 44
            AddRelationship = 35
            GetRolePlayers = 10
            GetRolePlayersByRoles = 51
            SetRolePlayer = 46
            UnsetRolePlayer = 15
            GetValue = 1
            GetOwners = 47
        End Enum

        Public Enum RequestOneofCase
            None = 0
            Open = 1
            Commit = 2
            ExecQuery = 3
            [Next] = 4
            [Stop] = 5
            RunConceptMethod = 6
            GetConcept = 7
            GetSchemaConcept = 8
            GetAttributesByValue = 9
            PutEntityType = 10
            PutRelationshipType = 11
            PutAttributeType = 12
            PutRole = 13
            PutRule = 14
        End Enum


    End Module

End Namespace
