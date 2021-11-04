Public Module publicCMML

    <Serializable()> _
Public Enum pcenumCMMLRelations
        'CMML = Common Meta-Modeling Language

        '-----------------------------------------------------------------------------------------------------------------
        'Unary Relations
        '[VM-20080517-It is most likely that Unary Relations will be stored against the 'ModelDictionary' table itself,
        '  howeverm, if need be Unary Relations may be stored within the 'FactTable'/'Fact' table as per binary relations
        '-----------------------------------------------------------------------------------------------------------------
        CoreIsProcess
        CoreIsActor
        CoreIsDecision
        CoreIsERTable

        '---------------------------------------------
        'STDs (State Transition Diagrams)
        '---------------------------------------------
        CoreStateHasName
        CoreStateTransition
        CoreValueTypeIsStateTransitionBased
        CoreValueTypeHasStartCoreElementState
        CoreValueTypeHasState
        CoreValueTypeHasEndCoreElementState

        '---------------------------------------------
        'DFDs (Data Flow Diagrams
        '---------------------------------------------
        CoreActorIsReceivingParty
        CoreProcessToProcessDataTransferRelation '(ActorSymbol,ProcessSymbol,DataStructSymbol)
        CoreProcessToProcessControlTransferRelation
        CoreProcessWritesToDataStore
        CoreProcessReadsFromDataStore

        '-----------------------------------------------
        'ETDs (Event Trace Diagrams)
        '-----------------------------------------------
        CoreElementSequenceNr

        '------------------------------------------
        'UML Use Case Diagrams
        '------------------------------------------
        CoreActorToProcessParticipationRelation  ' 200(ActorSymbol,ProcessSymbol)
        CoreProcessToProcessRelation

        '-----------------------------------------------------
        'ERDs (EntityRelationshipDiagrams)
        '-----------------------------------------------------
        CorePropertyHasOrdinalPosition
        CoreAttributeIsPartOfRelation
        CoreAttributeIsPartOfRelationDestination
        CoreAttributeIsPartOfRelationDestinationHasOrdinalPosition
        CoreAttributeIsPartOfRelationOrigin
        CoreAttributeIsPartOfRelationOriginHasOrdinalPosition
        CoreContributesToPrimaryKey
        CoreDestinationIsMandatory
        CoreDestinationMultiplicity
        CoreDestinationPredicate
        CoreERDEntity
        CoreERDAttribute
        CoreIndexHasDirection
        CoreIndexHasQualifier
        CoreIndexIgnoresNulls
        CoreIndexIsPrimaryKey
        CoreIndexRestrainsToUniqueValues
        CoreRelationIsForEntity
        CoreRelationHasDestinationEntity
        CoreIsMandatory
        CoreIsPGSRelation
        CoreOriginIsMandatory
        CoreOriginMultiplicity
        CoreOriginPredicate
        CorePropertyIsForFactType
        CorePropertyIsForRole
        CorePropertyHasActiveRole
        CoreRelationIsForFactType

        '---------------------------------------
        'Binary Relations - UML Class Diagrams 
        '---------------------------------------        
        CoreAssociationEndHasAggregationKind
        CoreAssociationEndIsAttachedToClass
        CoreAssociationHasAssociationEnd
        CoreAssociationHasAssociationName
        CoreAssociationIsDerived
        CoreAssociationIsForAttribute
        CoreAttributeHasType
        CoreClassHasClassName
        CoreClassHasAttribute
        CoreIndexIsForEntity
        CoreElementHasComment
        CoreElementHasElementType '{Element, ElementType}        
        CoreEnumerationHasEnumerationName
        CoreEnumerationHasEnumerationLiteral
        CoreEnumerationLiteralHasEnumerationLiteralName
        CoreEnumerationLiteralHasTypeInstance
        CoreGeneralisation
        CoreGeneralisationEnds
        CoreGeneralisationIsPartOfGeneralisationSet
        CoreGeneralisationSetHasGeneralisationSetName
        CoreGeneralisationSetIsDisjoint
        CoreGeneralisationSetIsCovering
        CoreIndexHasIndexName
        CoreIndexMakesUseOfProperty        
        CoreMultiplicityHasLowerBound
        CoreMultiplicityHasUpperBound
        CoreMultiplicityIsUnbounded
        CorePropertyHasType
        CorePropertyHasMultiplicity
        CorePropertyHasPropertyName
        CorePropertyIsDerived

    End Enum

    Enum pcenumCMMLIndexDirection
        ASC
        DESC
    End Enum

    <Serializable()> _
    Enum pcenumCMML
        Actor
        Concept 'State/Value (e.g. 'Value' of ValueType). Concept is used ubiquetously though, because every ObjectType and Fact in Richmond is a Concept
        DataStore 'Used in DataFlowDiagrams
        Entity 'Used in ER Diagrams
        ERDEntity 'ERDEntity
        ERDAttributeRole1 'ERDAttribute
        ERDAttributeRole2 'ERDAttribute
        Entity1 'ERDRelation
        Entity2 'ERDRelation
        Multiplicity 'The name of the ValueType used within the OriginMultiplicity and DestinationMultiplicity FactTypes.
        ObjectType 'ERDEntity
        Process
        Process1 'ProcessToProcessRelation FactType
        Process2 'ProcessToProcessRelation FactType
        Value1 'StartState within a StateTransitionRelation
        Value2 'EndState within a StateTransitionRelation
        ValueType
        Element 'ElementHasElementType
        ElementType 'ElementHasElementType
    End Enum

    Enum pcenumCMMLCorePage
        CoreActorEntityTypes
        CoreDataFlowDiagram
        CoreDerivations
        CoreEntityRelationshipDiagram
        CoreEventTraceDiagram
        CoreFlowchart
        CorePropertyGraphSchema
        CoreStateTransitionDiagram
        CoreUMLSuperstructure
        CoreUMLUseCaseDiagram
    End Enum

    <Serializable()> _
    Enum pcenumCMMLMultiplicity
        One
        Many
    End Enum

    Enum pcenumCMMLCoreModel
        Core
        CoreElement
    End Enum

End Module
