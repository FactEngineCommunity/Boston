Public Module publicConstantsModelValidator


    Public Enum pcenumModelErrors
        ObjectTypeRequiresPrimarySupertypeError = 99
        PopulationUniquenessError = 100
        TooManyRoleSequencesError = 101
        EqualityImpliedByMandatoryError = 102
        TooFewFactTypeRoleInstancesError = 103
        TooFewEntityTypeRoleInstancesError = 104
        EntityTypeRequiresReferenceSchemeError = 105
        FactTypeRequiresInternalUniquenessConstraintError = 106
        FrequencyConstraintContradictsInternalUniquenessConstraintError = 107
        ExternalConstraintRoleSequenceArityMismatchError = 108
        PreferredIdentifierRequiresMandatoryError = 109
        ImpliedInternalUniquenessConstraintError = 110
        CompatibleValueTypeInstanceValueError = 111
        RingConstraintTypeNotSpecifiedError = 112
        FrequencyConstraintMinMaxError = 113
        CompatibleRolePlayerTypeError = 114
        FactTypeRequiresReadingError = 115
        CompatibleSupertypesError = 116
        TooFewReadingRolesError = 117
        RolePlayerRequiredError = 118
        ValueMismatchError = 119
        ContradictionError = 120
        ImplicationError = 121
        NMinusOneError = 122
        DuplicateNameError = 123
        ValueRangeOverlapError = 124
        PopulationMandatoryError = 125
        TooFewRoleSequencesError = 126
        DataTypeNotSpecifiedError = 127
        TooManyReadingRolesError = 128
        RoleConstraintConflictError = 129
        ModelLoadingError = 300
        ModelElementAppearsOnNoPage = 130
        PopulationContainstNULLValueError = 131
        CMMLModelError = 140 'E.g. When there are errors within an RDS Table. E.g. When Columns have ActiveRole = Nothing etc.
    End Enum

    Public Enum pcenumModelSubErrorType
        'Start at 500 so as not to confuse with ModelErrors.
        None = 500
        RDSRelationOriginDestintaionColumnCountMismatch = 501

    End Enum


End Module
