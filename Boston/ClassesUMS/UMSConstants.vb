Namespace UMS

' ─── Enumerations ─────────────────────────────────────────────────────────────

Public Enum DataType
        [Integer]
        DateTime
        TextFixedLength
        TextVariableLength
End Enum

    Public Enum Constraint
        NotNull
        Unique
    End Enum

    Public Enum Cardinality
        One
        Many 'May be one. E.g. At least one.
        ZeroOrOne
        ZeroOrMore
    End Enum

End Namespace