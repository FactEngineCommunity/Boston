Namespace Validation

    Public Class FactTypeRequiresIUCError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.new(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrFactType As FBM.FactType
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError


            For Each lrFactType In Me.Model.FactType

                If (lrFactType.Arity > 1) And (lrFactType.InternalUniquenessConstraint.Count = 0) Then

                    lsErrorMessage = "Fact Type requires Internal Uniqueness Constraint Error - "
                    lsErrorMessage &= "Fact Type: '" & _
                                      lrFactType.Name & "'."

                    lrModelError = New FBM.ModelError(pcenumModelErrors.FactTypeRequiresInternalUniquenessConstraintError, _
                                                      lsErrorMessage, _
                                                      Nothing, _
                                                      lrFactType)

                    lrFactType.ModelError.Add(lrModelError)
                    Me.Model.AddModelError(lrModelError)

                End If
            Next

        End Sub

    End Class

End Namespace
