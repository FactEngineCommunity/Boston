Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.Reflection 

Public Module publicConstants

    '---------------------------------
    'Database ADO Constants
    '---------------------------------
    Public Const pc_open_forward_only As Integer = 0
    Public Const pcOpenStatic As Integer = 3
    Public Const pcOpenDynamic As Integer = 2
    Public Const pc_cmd_table As Integer = 2

    ''' <summary>
    ''' FactEngine uses this to set the Color of each Node based on its OrdinalPostion in the ProjectedColumns/Nodes
    ''' </summary>
    Public pcColorValues() As Integer = CType([Enum].GetValues(GetType(pcenumColourWheel)), Integer())

    ''' <summary>
    ''' This software can be released in either one of two Software Categories, "Student" or "Professional".
    ''' Features of the software are either enabled or disabled depending on the Software Category assigned to the software.
    ''' NB Currently stored in RichmondApplication class (default is "Professional").
    ''' NB Currently set in frmMain.Load method. Must be set before building the software for release..
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum pcenumSoftwareCategory
        Student
        Professional
    End Enum

    Public Enum pcenumRDSColumnType
        StandardRDSColumn
        ReturnFunctionColumn 'As used in Functions returned as a Column in a SQL Query etc.
    End Enum

    Public Enum pcenumTargetMorphShape
        RoundRectangle
        Circle
    End Enum

    Public Enum pcenumNaturalLanguage
        English
        Italian
        French
        German
        Russian
        Japanese
        Chinese
        Spanish
    End Enum

    Public Enum pcenumColourWheel
        LihtGreen = -7278960
        LightOrange = -404827
        LightYellow = -32
        LightPurple = -5399563
        LightBlue = -5909255
        LightGray = -2894893
    End Enum


    Public Enum pcenumDataType
        [String]
        [Integer]
        [Boolean]
        [Single]
        [Double]
        [Char]
        [Byte]
        [Date]
        [Long]
        [Short]
        [UInteger]
        [ULong]
        [UShort]
    End Enum

    Public Enum pcenumComboBoxStyle
        Dropdown
        DropdownList
        Simple
    End Enum


    <Serializable()>
    Public Enum pcenumDatabaseType
        None = 100
        MSJet = 101
        SQLServer = 102
        ORACLE = 103
        SQLite = 104
        MongoDB = 105
        ODBC = 106
        PostgreSQL = 107
        Snowflake = 108
        TypeDB = 109
        NORMA = 110
        Neo4j = 111
        RelationalAI = 112
        KuzuDB = 113
        EdgeDB = 114
    End Enum

    Public Enum pcenumDebugMode
        NoLogging
        Debug
        DebugCriticalErrorsOnly
    End Enum

    Public Enum pcenumErrorType
        Critical
        Information
        Warning
    End Enum

    <Serializable()>
    Public Enum pcenumRoleJoinType 'NB Stored as Integer within the database.
        None = 0
        EntityType = 1
        ValueType = 2
        FactType = 3
    End Enum

    Public Enum pcenumFollowingThatOrSome
        That
        Some
        Either
        None
        TheSame
    End Enum

    <Serializable()>
    Public Enum pcenumValueRangeType
        <Description("is equal to")> Equal
        <Description("none")> None
        <Description("is not equal to")> NotEqual
        <Description("is less than")> LessThan
        <Description("is less than or equal to")> LessThanOrEqual
        <Description("is greater than or equal to")> GreaterThanOrEqual
        <Description("is greater than")> GreaterThan
    End Enum

    <Serializable()> _
    Public Enum pcenumCardinalityRangeType
        LessThanOrEqual
        Equal
        GreaterThanOrEqual
        Between
    End Enum

    <Serializable()> _
    Public Enum pcenumConceptType
        [Class] 'Represents a Class in a UML Class Diagram
        Comment
        Actor 'Represents an Actor in a UML Use Case Diagram
        ActorName
        ActorProcessLink
        AssociationEndRole
        'CardinalityConstraint
        Concept
        DataStore
        Decision
        DerivationText
        EndStateIndicator
        EndStateTransition
        Enterprise
        Entity
        EntityType
        EntityTypeDerivationText
        EntityTypeName
        EnumerationSet 'Represents an Enumeration Set in a UML Class Diagram
        'EqualityConstraint
        [Event]
        'ExclusionConstraint
        'ExclusiveORConstraint
        'ExternalRoleConstraint
        'ExternalUniquenessConstraint
        [Function]
        FactTypeReading
        Fact
        FactType
        FactTypeDerivationText
        FactTypeName
        FactTable
        GeneralConcept  'Concepts that are not yet formalised.
        Generalisation
        'FrequencyConstraint
        'InclusiveORConstraint
        'InternalRoleConstraint
        Link
        ModelNote
        Model
        ObjectInstance
        Page
        PGSNode
        Procedure
        Process
        ProcessProcessLink
        Relation
        Role
        'RingConstraint
        RoleGroupSelectNode
        RoleData
        RoleName
        RoleConstraint
        RoleConstraintRole
        Solution
        StartStateIndicator
        StartStateTransition
        State
        StateTransition
        StrategicGoal        
        SubjectArea
        SubtypeRelationship
        'SubtypeLink
        SystemBoundary
        TotalSubtype
        Value
        ValueConstraint
        ValueType
        Unknown  'E.g. When importing a NORMA file, may need to check what type a "ref" reference is (i.e. e.g. to an EntityType, FactType, or (in error) Unknown).
    End Enum

    Public Enum pcenumDiagramType
        ORMModel
    End Enum

    <Serializable()> _
    Public Enum pcenumSubjectAreaType
        business_area = 1
        subject_area = 2
    End Enum

    <Serializable()> _
    Public Enum pcenumShapeDropTarget
        Canvas
        OtherShapeObject
    End Enum

    Public Enum pcenumReferenceMode
        <Description(".#")> DotHash
        <Description(".code")> DotLowercaseCode
        <Description(".Code")> DotUppercaseCode
        <Description(".Id")> DotCamelcaseId
        <Description(".id")> DotLowercaseId
        <Description(".ID")> DotALLCAPSID
        <Description(".name")> DotLowercaseName
        <Description(".Name")> DotUppercaseName
        <Description(".Nr")> DotUppwercaseNr
        <Description(".nr")> DotLowercaseNr
        <Description(".Title")> DotUppercaseTitle
        <Description(".title")> DotLowercaseTitle
        <Description("AUD:")> AUD
        <Description("CE:")> CE
        <Description("Celcius:")> Celsius
        <Description("cm:")> cm
        <Description("EUR:")> EUR
        <Description("Fahrenheit:")> Fahrenheit
        <Description("kg:")> kg
        <Description("km:")> km
        <Description("mile:")> mile
        <Description("mm:")> mm
        <Description("USD:")> USD
    End Enum

    Public Enum pcenumReferenceModeEndings
        <Description(".#")> DotHash
        <Description(".code")> DotLowercaseCode
        <Description(".Code")> DotUppercaseCode
        <Description(".Id")> DotCamelcaseId
        <Description("Id")> NoDotCamelcaseId
        <Description(".id")> DotLowercaseId
        <Description(".ID")> DotALLCAPSID
        <Description(".name")> DotLowercaseName
        <Description(".Name")> DotUppercaseName
        <Description(".Nr")> DotUppwercaseNr
        <Description(".nr")> DotLowercaseNr
        <Description(".quantity")> DotLowercaseQuantity
        <Description(".Quantity")> DotUppercaseQuantity
        <Description(".Title")> DotUppercaseTitle
        <Description(".title")> DotLowercaseTitle
        <Description(".Date")> UppercaseDate
        <Description(".date")> DotLowercaseDate
        <Description(".Time")> DotUppercaseTime
        <Description(".time")> DotLowercaseTime
    End Enum

    Public Enum pcenumValueTypeCandidates
        <Description("Value")> Value
        <Description("Number")> Number
        <Description("Status")> Status
        <Description("Year")> Year
        <Description("Month")> Month
        <Description("Reason")> Reason
        <Description("Quantity")> Quantity
    End Enum

    <Serializable()> _
    Public Enum pcenumRoleConstraintType
        CardinalityConstraint
        EqualityConstraint
        ExclusionConstraint
        ExclusiveORConstraint        
        ExternalUniquenessConstraint
        FrequencyConstraint
        InclusiveORConstraint
        InternalUniquenessConstraint
        RingConstraint
        RoleValueConstraint
        SubsetConstraint
        ValueConstraint
        ValueComparisonConstraint
    End Enum

    <Serializable()> _
    Public Enum pcenumRingConstraintType
        None
        Acyclic
        AcyclicIntransitive
        AcyclicStronglyIntransitive
        Antisymmetric
        Asymmetric
        AsymmetricIntransitive
        Intransitive
        Irreflexive
        PurelyReflexive
        Symmetric
        SymmetricIntransitive
        SymmetricIrreflexive
        SymmetricTransitive
        Transitive
        DeonticIrreflexive
        DeonticAssymmetric
        DeonticIntransitive
        DeonticAntisymmetric
        DeonticAcyclic
        DeonticAssymmetricIntransitive
        DeonticAcyclicIntransitive
        DeonticSymmetric
        DeonticSymmetricIrreflexive
        DeonticSymmetricIntransitive
        DeonticPurelyReflexive
    End Enum

    <Serializable()> _
    Public Enum pcenumRingConstraintTypeSelection
        Irreflexive
        Asymmetric
        Intransitive
        Antisymmetric
        Acyclic
        AsymmetricIntransitive
        AcyclicIntransitive
        Symmetric
        SymmetricIrreflexive
        SymmetricIntransitive
        PurelyReflexive
        DeonticIrreflexive
        DeonticAssymmetric
        DeonticIntransitive
        DeonticAntisymmetric
        DeonticAcyclic
        DeonticAssymmetricIntransitive
        DeonticAcyclicIntransitive
        DeonticSymmetric
        DeonticSymmetricIrreflexive
        DeonticSymmetricIntransitive
        DeonticPurelyReflexive
    End Enum

    <Serializable()> _
    Public Enum pcenumMenuType
        menuBoston
        modelORMModel
        pageBPMNCollaborationDiagram
        pageBPMNConversationDiagram
        pageBPMNProcessDiagram
        pageBPMNChoreographyDiagram
        pageDataFlowDiagram
        pageORMModel
        pagePGSDiagram
        pageERD
        pageSTD
        pageUMLUseCaseDiagram
    End Enum

    <Serializable()> _
    Public Enum pcenumNavigationIcons
        iconORMDiagram = 0
        iconDatabase = 1
        iconPage = 2
        iconPGSPage = 3 'Property Graph Schema Page
        iconERDPage = 4
        iconSTDPage = 5
        iconUCDPage = 15
        iconBPMNChoreorgraphDiagram = 16
        iconBPMNCollaborationDiagram = 17
        iconBPMNConversationDiagram = 18
        iconBPMNProcessDiagram = 19
    End Enum

    <Serializable()> _
    Public Enum pcenumORMReferenceMode
        id = 100
    End Enum

    ''' <summary>
    ''' Used within a Form/Page to flag when a Form/Page is in a special DragMode based on what the user has dragged to the form.
    '''   e.g. If the User drags a SupertypeConnector, then the Form/Page is put into the ORMSupertypeConnector DragMode, and
    '''   the cursor of the ORMModelPage Form/Page is changed to the SupertypeConnnector icon (for the duration of the drag operation).
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum pcenumSpecialDragMode
        None
        ORMSubtypeConnector
        ORMNoteConnector
    End Enum

    Public Enum pcenumBinaryRelationMultiplicityType
        OneToOne
        OneToMany
        ManyToOne
        ManyToMany
    End Enum

    <Serializable()> _
    Public Enum pcenumSecurityFacilityType
        menu_access = 100
        function_access = 110
    End Enum

    <Serializable()> _
    Public Enum pcenumCoreEntityType
        Process = 100
        Actor = 110
    End Enum

    <Serializable()> _
    Public Enum pcenumKL
        ThereExists = 214
        ForAll = 213
        ImpliesThat = 167
    End Enum

    <Serializable()> _
    Public Enum pcenumKLFreeVariable
        x = 1
        y = 2
        z = 3
        p = 4
        q = 5
        r = 6
    End Enum

    <Serializable()> _
    Public Enum pcenumKLFunction
        '--------------------------------------------------------------------------------------------------------------------
        'In general, you can just use the first character of the FactType isomorphically mapped to a Kl function.
        '  If that is not available, step through the enum values below until you find a function label that is free for use
        '--------------------------------------------------------------------------------------------------------------------
        A = 1
        B = 2
        C = 3
        D = 4
        E = 5
        F = 6
        G = 7
        H = 8
        I = 9
        J = 10
        K = 11
        L = 12
        M = 13
        N = 14
        O = 15
        P = 16
        Q = 17
        R = 18
        S = 19
        T = 20
        U = 21
        V = 22
        W = 23
        X = 24
        Y = 25
        Z = 26
    End Enum

    <Serializable()> _
    Enum pcenumKLProofRule
        P = 100 'Premise
        C = 101 'Conclusion
        NC = 102 'Negated Conclusion
        PC = 103 'Propositional Calculus tree rule
        QN = 104 'Quantifier Negation
        EI = 105 'Existential Instantiation
        UI = 106 'Universal Instantiation
        SI = 107 'Substituity of Identicals
        DN = 108 'Double Negation
        AA = 109 'Afirm the Antecenent (i.e. Modus Ponens)
        AB = 110 'Affirm one side of a Biconditional
    End Enum

    Enum pcenumKLDataLetter
        a
        b
        c
        d
        e
        f
        g
        h
        i
        j
        k
        l
        m
        n
        o
        p
        q
        r
        s
        t
        u
        v
        w
        x
        y
        z
    End Enum

    <Serializable()> _
    Enum pcenumFactTypeOrientation
        Horizontal
        Vertical
    End Enum

    Enum pcenumToolboxShapeFunction
        SubtypeConnectorFunction
    End Enum

    Enum pcenumUserAction
        MoveModelObject
        RemovedPageObjectFromPage
        AddPageObjectToPage 'e.g. PageObject dragged/dropped from ModelDictionaryViewer (ModelObject already exists at the Model level).
        AddNewPageObjectToPage 'When new ModelObject is created at the Model level as well.
    End Enum

End Module
