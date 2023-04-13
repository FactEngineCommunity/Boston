Imports System.Reflection

Namespace Validation

    Public Class ModelValidator

        ''' <summary>
        ''' The Model being validated
        ''' </summary>
        ''' <remarks></remarks>
        Public Model As FBM.Model = Nothing

        ''' <summary>
        ''' List of ErrorCheker inherited classes/objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public ErrorChecker As New List(Of Validation.ErrorChecker)

        Public Sub New(ByRef arModel As FBM.Model)

            Me.Model = arModel

            Call Me.AddErrorCheckers()

        End Sub

        ''' <summary>
        ''' Adds an ErrorChecker object to the list of ErrorChecker objects.
        ''' </summary>
        ''' <param name="arErrorChecker"></param>
        ''' <remarks></remarks>
        Public Sub AddErrorChecker(ByRef arErrorChecker As Validation.ErrorChecker)

            Me.ErrorChecker.Add(arErrorChecker)

        End Sub

        ''' <summary>
        ''' Creates the initial list of ErrorChecker objects
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddErrorCheckers()

            Try
                If Me.Model Is Nothing Then
                    Throw New Exception("Method called where no Model exists for the ModelValidator")
                End If

                Me.AddErrorChecker(New Validation.ErrorCheckerPopulationUniquenessError(Me.Model)) '100
                'TooManyRoleSequencesError
                'EqualityImpliedByMandatoryError
                'TooFewFactTypeRoleInstancesError
                'TooFewEntityTypeRoleInstancesError
                Me.AddErrorChecker(New Validation.EntityTypeRequiresReferenceSchemeError(Me.Model)) '105               
                'ObjectTypeRequiresPrimarySupertypeError
                Me.AddErrorChecker(New Validation.FactTypeRequiresIUCError(Me.Model)) '106
                'FrequencyConstraintContradictsInternalUniquenessConstraintError
                'ExternalConstraintRoleSequenceArityMismatchError
                'PreferredIdentifierRequiresMandatoryError
                'ImpliedInternalUniquenessConstraintError
                'CompatibleValueTypeInstanceValueError
                'RingConstraintTypeNotSpecifiedError
                Me.AddErrorChecker(New Validation.FrequencyConstraintMinMaxError(Me.Model)) '113
                'CompatibleRolePlayerTypeError
                Me.AddErrorChecker(New Validation.FactTypeRequiresReadingError(Me.Model)) '115
                'CompatibleSupertypesError
                'TooFewReadingRolesError
                'RolePlayerRequiredError
                'ValueMismatchError
                'ContradictionError
                'ImplicationError
                'NMinusOneError
                'DuplicateNameError
                'ValueRangeOverlapError
                'PopulationMandatoryError
                Me.AddErrorChecker(New Validation.TooFewRoleSequencesError(Me.Model)) '126
                Me.AddErrorChecker(New Validation.DataTypeNotSpecifiedError(Me.Model)) '127
                'TooManyReadingRolesError
                Me.AddErrorChecker(New Validation.RoleConstraintConflictError(Me.Model)) '129
                'CompoundReferenceSchemeForEntityTypeWithReferenceMode
                'Me.AddErrorChecker(New Validation.ModelElementAppearsOnNoPageError(Me.Model)) '130
                Me.AddErrorChecker(New Validation.ErrorCheckerPopulationHasNULLValueError(Me.Model)) '131
                Me.AddErrorChecker(New Validation.ErrorCheckerCMMLModelError(Me.Model)) '140

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub CheckForErrors()

            Try

                Dim lrErrorChecker As Validation.ErrorChecker

                Call Me.Model.ClearModelErrors()

                For Each lrErrorChecker In Me.ErrorChecker
                    Call lrErrorChecker.CheckForErrors()
                Next

                Me.Model.TriggerFinishedErrorChecking()

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
