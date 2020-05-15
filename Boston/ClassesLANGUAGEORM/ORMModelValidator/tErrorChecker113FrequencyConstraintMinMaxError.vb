Namespace Validation

    Public Class FrequencyConstraintMinMaxError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.new(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lrFactData As FBM.FactData
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError

            For Each lrRoleConstraint In Me.Model.RoleConstraint.FindAll(Function(x) x.RoleConstraintType = pcenumRoleConstraintType.FrequencyConstraint)

                For Each lrFactData In lrRoleConstraint.RoleConstraintRole(0).Role.Data

                    Dim liFactDataCount = From fd In lrRoleConstraint.RoleConstraintRole(0).Role.Data _
                                          Where fd.Data = lrFactData.Data _
                                          Select fd Distinct.Count

                    If liFactDataCount > lrRoleConstraint.MaximumFrequencyCount Then

                        lsErrorMessage = "Fact Data value, '" & lrFactData.Data & "', "
                        lsErrorMessage &= " exists in " & liFactDataCount.ToString & " Facts"
                        lsErrorMessage &= " where the Frequency Constraint, '" & lrRoleConstraint.Name & "',"
                        lsErrorMessage &= " has a Maximum Frequency for the Role of <=" & lrRoleConstraint.MaximumFrequencyCount.ToString

                        lrModelError = New FBM.ModelError(pcenumModelErrors.FrequencyConstraintMinMaxError, _
                                  lsErrorMessage, _
                                  Nothing, _
                                  lrRoleConstraint)

                        lrRoleConstraint.AddModelError(lrModelError)
                        Me.Model.AddModelError(lrModelError)

                    End If
                Next

                For Each lsInstance In lrRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject.Instance

                    Dim liFactDataCount = From fd In lrRoleConstraint.RoleConstraintRole(0).Role.Data _
                                          Where fd.Data = lsInstance _
                                          Select fd Distinct.Count

                    If liFactDataCount < lrRoleConstraint.MinimumFrequencyCount Then

                        lsErrorMessage = "Fact Data value, '" & lsInstance & "', "
                        lsErrorMessage &= " exists in " & liFactDataCount.ToString & " Facts"
                        lsErrorMessage &= " where the Frequency Constraint, '" & lrRoleConstraint.Name & "',"
                        lsErrorMessage &= " has a Minimum Frequency for the Role of " & lrRoleConstraint.MinimumFrequencyCount.ToString

                        lrModelError = New FBM.ModelError(pcenumModelErrors.FrequencyConstraintMinMaxError, _
                                  lsErrorMessage, _
                                  Nothing, _
                                  lrRoleConstraint)

                        lrRoleConstraint.AddModelError(lrModelError)
                        Me.Model.AddModelError(lrModelError)
                    End If
                Next

            Next

        End Sub

    End Class

End Namespace
