Namespace Validation

    Public Class TooFewRoleSequencesError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.new(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError


            For Each lrRoleConstraint In Me.Model.RoleConstraint
                Select Case lrRoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.RingConstraint
                        If lrRoleConstraint.RoleConstraintRole.Count = 1 Then

                            lsErrorMessage = "Role Constraint has too few Role Sequences - "
                            lsErrorMessage &= "Role Constraint: '" & _
                                              lrRoleConstraint.Name & "'."

                            lrModelError = New FBM.ModelError(pcenumModelErrors.TooFewRoleSequencesError, _
                                                              lsErrorMessage, _
                                                              Nothing, _
                                                              lrRoleConstraint)

                            lrRoleConstraint.ModelError.Add(lrModelError)
                            Me.Model.AddModelError(lrModelError)
                        End If

                    Case Else
                        If lrRoleConstraint.RoleConstraintRole.Count = 0 Then

                            lsErrorMessage = "Role Constraint has too few Role Sequences - "
                            lsErrorMessage &= "Role Constraint: '" & _
                                              lrRoleConstraint.Name & "'."

                            lrModelError = New FBM.ModelError(pcenumModelErrors.TooFewRoleSequencesError, _
                                                              lsErrorMessage, _
                                                              Nothing, _
                                                              lrRoleConstraint)

                            lrRoleConstraint.ModelError.Add(lrModelError)
                            Me.Model.AddModelError(lrModelError)

                        End If
                End Select
            Next

        End Sub

    End Class

End Namespace
