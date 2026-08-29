Imports System.Collections.Generic

Namespace Derivations

    ''' <summary>
    ''' Object representation of the TinyPG DERIVATION_EXPRESSION production.
    ''' LOGICAL_CONNECTIVE item n joins DERIVATION_CLAUSE item n + 1.
    ''' </summary>
    Public Class DerivationExpression

        Public Property DERIVATION_CLAUSE As New List(Of DerivationClause)
        Public Property LOGICAL_CONNECTIVE As New List(Of LogicalConnective)

    End Class

End Namespace
