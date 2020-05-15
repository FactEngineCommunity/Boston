Namespace Validation

    Public Class RoleConstraintConflictError
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
                    Case Is = pcenumRoleConstraintType.EqualityConstraint
                        '-----------------------------------------------------------------------------------------------
                        'Check to see that there is not an ExclusionConstraint for the same set of RoleConstraintRoles
                        '-----------------------------------------------------------------------------------------------
                        Dim larRoleConstraint As New List(Of FBM.RoleConstraint)
                        larRoleConstraint = Me.Model.RoleConstraint.FindAll(AddressOf lrRoleConstraint.EqualsByRoleConstraintRoleGroup)
                        Dim lrOtherRoleConstraint As FBM.RoleConstraint
                        For Each lrOtherRoleConstraint In larRoleConstraint
                            If Not (lrOtherRoleConstraint Is lrRoleConstraint) Then
                                Dim larErrorSet As pcenumRoleConstraintType() = {pcenumRoleConstraintType.ExclusionConstraint, _
                                                                                 pcenumRoleConstraintType.SubsetConstraint, _
                                                                                 pcenumRoleConstraintType.ExclusiveORConstraint, _
                                                                                 pcenumRoleConstraintType.InclusiveORConstraint}

                                If larErrorSet.Contains(lrOtherRoleConstraint.RoleConstraintType) Then

                                    lsErrorMessage = "Role Constraint, '" & lrRoleConstraint.Name & _
                                                     "', conflicts with Role Constraint, '" & lrOtherRoleConstraint.Name & "'"

                                    lrModelError = New FBM.ModelError(pcenumModelErrors.RoleConstraintConflictError, _
                                                                      lsErrorMessage, _
                                                                      Nothing, _
                                                                      lrRoleConstraint)

                                    lrRoleConstraint.ModelError.Add(lrModelError)
                                    Me.Model.AddModelError(lrModelError)
                                End If
                            End If
                        Next

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
