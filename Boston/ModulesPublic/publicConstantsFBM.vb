Public Module publicConstantsFBM

    <Serializable()> _
    Public Enum pcenumLanguage
        ORMModel = 1
        UMLUseCaseDiagram = 2
        BusinessRulesNaturalLanguage = 3 'English (see Italian below...may need to change this. We ClassBrain for more information about natural language processing).
        ZNotation = 4
        FlowChart = 6
        DataFlowDiagram = 8
        EntityRelationshipDiagram = 9
        PropertyGraphSchema = 22
        StateTransitionDiagram = 10
        BPMNChoreographDiagram = 11
        CMML = 12 'Used for cloning UML.Process objects to UCD.Process objects etc.
        BPMNConversationDiagram = 13
        BPMNCollaborationDiagram = 14
        BPMNProcessDigram = 15
    End Enum

    <Serializable()>
    Public Enum pcenumJoinPathError
        None
        CircularPathFound
        AmbiguousJoinPath
        RidiculousDepth
        DoubledBackToExistingArgumentRole
        DoubledBackToCoveredRole
    End Enum

    Public Enum pcenumModelFixType
        RolesWithoutJoinedORMObject
        RelationsInvalidActiveRoleOnOriginColumns
        ColumnsWhereActiveRoleIsNothingTryAndFix
        ColumnsWhereActiveRoleIsNothingRemoveTheColumn
        ColumnsWhereNoLongerPartOfSupertypeHierarchyRemoveColumn
        InternalUniquenessConstraintsWhereLevelNumbersAreNotCorrect
        ColumnOrdinalPositionsResetWhereOutOfSynchronousOrder
        RDSTablesWithNoColumnsRemoveThoseTables
        RDSColumnsThatShouldBeMandatoryMakeMandatory
        RemoveFactTypeInstancesFromPageWhereFactTypeIntanceHasRoleInstanceThatJoinsNothing
        RDSTablesWhereTheNumberOfPrimaryKeyColumnsDoesNotMatchTheNumberOfRolesInThePreferredIdentifierFixThat
        DuplicateFactsRemoveDuplicates
        RDSTablesAndPGSNodesThatAreMissingRelationsAddTheRelations
        RDSTablesWithMoreThanOneRelationForTheSameFactTypeJoinPruneExtraRelations
        RDSRelationsThatHaveNoOriginColumnsRemoveRelation
        RDSRelationsThatHaveOriginTableButNoDestinationTableAndViceVersa
        RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount
    End Enum

End Module
