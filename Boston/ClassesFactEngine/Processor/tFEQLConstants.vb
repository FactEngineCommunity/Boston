Namespace FEQL
    Public Module tFEQLConstants

        Public Enum pcenumFEQLMathComparitor
            None
            Equals
            LessThan
            GreaterThan
        End Enum

        Public Enum pcenumFEQLComparitor
            Bang
            Carret
            Colon
            InComparitor
            LikeComparitor
        End Enum

        Public Enum pcenumFEQLNodeModifierFunction
            None
            [Date]
            Month
            Year
            Time
            ToLower
            ToUpper
            Sum
            Average
            Max
            Min
        End Enum

        Public Enum pcenumFEQLOrderByDirection
            None
            Ascending
            Descending
        End Enum

    End Module

End Namespace
