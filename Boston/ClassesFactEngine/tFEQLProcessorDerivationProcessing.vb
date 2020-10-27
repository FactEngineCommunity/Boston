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
                'Analyse/Get the type of Derivation
                'Return the SQL for the Derivation
                For Each lrDerivationSubClause In lrDerivationClause.DERIVATIONSUBCLAUSE

                    If lrDerivationSubClause.isFactTypeOnly Then
                        'Get the FactType for the DerivationSubClause
                        Dim larModelObject As New List(Of FBM.ModelObject)
                        Dim larRole As New List(Of FBM.Role)
                        Dim lrFactTypeReading As New FBM.FactTypeReading
                        Dim lrFactType As New FBM.FactType("DummyFactType")

                        Dim lrSentence As New Language.Sentence("DummySentence")

                        Dim liInd = 0
                        For Each lsModelElementName In lrDerivationSubClause.MODELELEMENTNAME
                            Dim lrModelObject = Me.Model.GetModelObjectByName(lsModelElementName)
                            larModelObject.Add(lrModelObject)

                            Dim lrRole = New FBM.Role(lrFactType, lrModelObject)
                            larRole.Add(lrRole)

                            Dim lrPredicatePart = New Language.PredicatePart()
                            Dim lsPredicatePartText = ""
                            If liInd < lrDerivationSubClause.MODELELEMENTNAME.Count - 1 Then
                                For Each lsPredicatePart In lrDerivationSubClause.PREDICATECLAUSE(liInd).PREDICATE
                                    lsPredicatePartText &= lsPredicatePart & " "
                                Next
                                lrPredicatePart.PredicatePartText = Trim(lsPredicatePartText)
                            Else
                                lrPredicatePart.PredicatePartText = ""
                            End If
                            lrSentence.PredicatePart.Add(lrPredicatePart)

                            liInd += 1
                        Next

                        lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)

                        lrDerivationSubClause.FBMFactType = Me.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)

                    End If

                Next

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

    End Class

End Namespace

