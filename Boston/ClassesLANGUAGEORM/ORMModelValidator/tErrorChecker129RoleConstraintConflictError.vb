Imports System.Reflection

Namespace Validation

    Public Class RoleConstraintConflictError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.New(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrRoleConstraint As FBM.RoleConstraint
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError

            Try

                For Each lrRoleConstraint In Me.Model.RoleConstraint
                    Select Case lrRoleConstraint.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.EqualityConstraint
                            '-----------------------------------------------------------------------------------------------
                            'Check to see that there is not an ExclusionConstraint etc for the same set of RoleConstraintRoles
                            '-----------------------------------------------------------------------------------------------
                            Dim larRoleConstraint As New List(Of FBM.RoleConstraint)
                            larRoleConstraint = Me.Model.RoleConstraint.FindAll(AddressOf lrRoleConstraint.EqualsByRoleConstraintRoleGroup)
                            Dim lrOtherRoleConstraint As FBM.RoleConstraint
                            For Each lrOtherRoleConstraint In larRoleConstraint
                                If Not (lrOtherRoleConstraint Is lrRoleConstraint) Then
                                    Dim larErrorSet As pcenumRoleConstraintType() = {pcenumRoleConstraintType.ExclusionConstraint,
                                                                                     pcenumRoleConstraintType.SubsetConstraint,
                                                                                     pcenumRoleConstraintType.ExclusiveORConstraint,
                                                                                     pcenumRoleConstraintType.InclusiveORConstraint}

                                    If larErrorSet.Contains(lrOtherRoleConstraint.RoleConstraintType) Then

                                        lsErrorMessage = "Role Constraint, '" & lrRoleConstraint.Name &
                                                         "', conflicts with Role Constraint, '" & lrOtherRoleConstraint.Name & "'"

                                        lrModelError = New FBM.ModelError(pcenumModelErrors.RoleConstraintConflictError,
                                                                          lsErrorMessage,
                                                                          Nothing,
                                                                          lrRoleConstraint)

                                        lrRoleConstraint.ModelError.Add(lrModelError)
                                        Me.Model.AddModelError(lrModelError)
                                    End If
                                End If
                            Next

                        Case Is = pcenumRoleConstraintType.RingConstraint
                            If lrRoleConstraint.RoleConstraintRole.Count = 1 Then

                                lsErrorMessage = "Role Constraint has too few Role Sequences - "
                                lsErrorMessage &= "Role Constraint: '" &
                                                  lrRoleConstraint.Name & "'."

                                lrModelError = New FBM.ModelError(pcenumModelErrors.TooFewRoleSequencesError,
                                                                  lsErrorMessage,
                                                                  Nothing,
                                                                  lrRoleConstraint)

                                lrRoleConstraint.ModelError.Add(lrModelError)
                                Me.Model.AddModelError(lrModelError)
                            End If

                        Case Else
                            If lrRoleConstraint.RoleConstraintRole.Count = 0 Then

                                lsErrorMessage = "Role Constraint has too few Role Sequences - "
                                lsErrorMessage &= "Role Constraint: '" &
                                                  lrRoleConstraint.Name & "'."

                                lrModelError = New FBM.ModelError(pcenumModelErrors.TooFewRoleSequencesError,
                                                                  lsErrorMessage,
                                                                  Nothing,
                                                                  lrRoleConstraint)

                                lrRoleConstraint.ModelError.Add(lrModelError)
                                Me.Model.AddModelError(lrModelError)

                            End If
                    End Select
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class

End Namespace
