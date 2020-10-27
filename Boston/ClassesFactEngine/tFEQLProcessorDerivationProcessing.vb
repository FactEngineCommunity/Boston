Namespace FEQL

    Partial Public Class Processor

        Public Sub processDerivationText(ByVal asDerivationText As String,
                                         ByRef arFactType As FBM.FactType)

            Try

                Me.Parsetree = Me.Parser.Parse(asDerivationText)

                Dim lrDerivationClause As New FEQL.DERIVATIONCLAUSE


                If Me.Parsetree.Errors.Count > 0 Then
                    Throw New Exception("Error in Derivation Text for Fact Type, '" & arFactType.Id & "'.")
                End If

                Call Me.GetParseTreeTokensReflection(lrDerivationClause, Me.Parsetree.Nodes(0))

                'PSEUDOCODE
                'Get the FactTypes for the DerivationSubClauses
                'Analyse the type of Derivation 

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

    End Class

End Namespace

