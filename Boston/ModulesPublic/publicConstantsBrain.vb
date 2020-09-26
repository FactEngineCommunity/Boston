Public Module publicConstantsBrain

    <Serializable()> _
    Public Enum pcenumBrainMode
        ORMQL
        NaturalLanguage
    End Enum

    Public Enum pcenumSentenceType
        Statement 'e.g. "All boys are good"
        Question 'e.g. "What time is it?"
        Directive 'e.g. "Run away"
        Salutation 'e.g. "Hello"
        Response 'Is a response to a question from the Brain.
        Unknown
    End Enum

    Public Enum pcenumPhraseType
        Resolving
        Resolved
    End Enum

    Public Enum pcenumWordSense
        Unknown 'Where it cannot be resolved/needs resolution
        Noun 'e.g. Person
        Verb 'e.g. Run
        Adjective 'e.g. Brown
        Adverb '
        Pronoun 'e.g. Peter
        Article 'e.g. a
        '---------------
        Preposition 'e.g. in, on, to, for, betwixt  and between
        Conjunction 'e.g. if, and, but
        Delimiter 'e.g. well, like, so, too, and sorta
        AlternativeAdditiveDeterminer 'another, other, somebody else
        CardinalNumber 'one, two, fifty, etc.
        DegreeDeterminer 'many, much, few, little...
        DemonstrativeDeterminer 'this, that, these, those, which
        DisjunctiveDeterminer 'either, neither
        DistributiveDeterminer  'each, every
        ElectiveDeterminer 'any, either, wichever
        EqualitiveDeterminer 'the(same)
        EvaluativeDeterminer 'such()
        ExclamativeDeterminer 'what(eyes!)
        ExistentialDeterminer 'some, any
        InterrogativeDeterminer 'which, what
        NegativeDeterminer 'no, neither
        PersonalDeterminer 'we teaches, you guys
        PostiveMultalDeterminer 'a lot of, many, several
        PositivePaucal 'a few, a little, some
        PossessiveDeterminer 'my, your, our etc
        QualitativeDeterminer 'that
        Quantifier ' all, few, many, several, some, every, each, any, no, etc.
        RelativeDeterminer 'whichever, whatever
        SubordinateConjunction
        SufficiencyDeterminer 'enough, sufficient
        UniquitiveDeterminer ' the(only)
        UniversalDeterminer
    End Enum

    Enum pcenumSelfTeachingMode
        [On]
        Off
    End Enum

    <Serializable()> _
    Enum pcenumQuestionType
        CopyFactType
        CheckWordTypeVerb
        CheckWordTypeNoun
        CheckWordTypeAdjective
        CreateConcept
        CreateEntityType
        CreateFactType
        CreateFactTypePredetermined
        CreateFactTypeReading
        CreateSubtypeRelationship
        CreateValueType
        ForgetAskedToAbortPlan 'i.e. If a user restates an aborted Sentence/Plan, the Brain asks if the user wants to forget that it was asked to abort that Sentence/Plan.
        OpenHelpFile
    End Enum

    Enum pcenumActionType
        CreateValueType
        CreateEntityType
        CreateFactType
        CreateFactTypeReading
        CreateRoleConstraint
        CreateSubtypeRelationship
        FindOrCreateEntityTypeORFactType 'e.g. In a Statement of type 'Person has AT MOST ONE FirstName' the first ModelElement is an EntityType or a FactType.
        FindOrCreateModelElement 'e.g. In a Statement of type 'Person has AT MOST ONE FirstName' the second ModelElement (FirstName) must either be found in the Model or a new ModelElement created (ValueType or EntityType)
        ForgetStatementAbortion 'Forget that the User asked the Brain to Abort a Statement/Sentence. i.e. So that the Statement/Sentence can be processed again.
        None 'i.e. e.g. An AlternateActionType for a Step may be pcenumActionType.None
    End Enum

    Enum pcenumBrainPlanStepStatus
        Unresolved
        Resolved
    End Enum

    Public Enum pcenumStepFactTypeAttributes
        None
        BinaryFactType
        ManyToOne
        MandatoryFirstRole
    End Enum

    Public Enum pcenumQuestionResolution
        Unanswered
        Aborted
        Answered
        SelfAnswered 'The Brain can answer its own Questions. e.g. If the Question's PlanStep is to FindOrCreateEntityTypeORFactType and the/an EntityType already exists in the Model, then the Brain can answer its own Question.
    End Enum

    Public Enum pcenumAcceptedResponce
        Unanswered
        Affirmative
        Nagative
    End Enum

    Public Enum pcenumPlanStatus        
        AbortedByUser
        Aborted
        Active
    End Enum

    Public Enum pcenumSentenceResolutionType
        Unresolved
        AbortedByUser
        SuccessfullyProcessed
    End Enum

End Module
