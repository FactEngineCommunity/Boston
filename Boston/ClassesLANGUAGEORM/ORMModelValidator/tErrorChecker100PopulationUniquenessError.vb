Imports System.Reflection

Namespace Validation

    Public Class ErrorCheckerPopulationUniquenessError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.New(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrFactType As FBM.FactType
            Dim lrInternalUniquenessConstraint As FBM.RoleConstraint
            Dim lrFact As FBM.Fact
            Dim lrRole As FBM.Role
            Dim lrFactData As FBM.FactData
            Dim lsDataValue As String = ""
            Dim lrFactPredicate As New FBM.FactPredicate
            Dim lrFactList As New List(Of FBM.Fact)
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError

            Try

                For Each lrFactType In Me.Model.FactType

                    If lrFactType.Fact.Count > 1 Then

                        If lrFactType.InternalUniquenessConstraint.Count = 0 Then
                            Dim larModelErrorFactData = (From Fact In lrFactType.Fact
                                                         From FactData In Fact.Data
                                                         Where FactData.ModelError IsNot Nothing
                                                         Where FactData.ModelError.Count > 0
                                                         Select FactData).ToList

                            For Each lrFactData In larModelErrorFactData
                                Call lrFactData.ClearModelErrors()
                            Next
                        End If

                        For Each lrRole In lrFactType.RoleGroup
                            If Not lrRole.HasInternalUniquenessConstraint Then
                                For Each lrFactData In lrRole.Data
                                    Call lrFactData.ClearModelErrors()
                                Next
                            End If
                        Next

                        For Each lrInternalUniquenessConstraint In lrFactType.InternalUniquenessConstraint
                            '---------------------------------------------------------------------------
                            'Find the Count of Facts that have matching FactData for the span of Roles 
                            '  in the InternalUniquenessConstraint
                            '---------------------------------------------------------------------------

                            For Each lrFact In lrFactType.Fact

                                lrFactPredicate = New FBM.FactPredicate

                                For Each lrRole In lrInternalUniquenessConstraint.Role

                                    lrFactData = lrFact.GetFactDataByRoleId(lrRole.Id)
                                    If lrFactData Is Nothing Then
                                        Dim lsMessage = "No FactData found for Role:"
                                        lsMessage &= vbCrLf & vbCrLf & "InternalUniquenessConstraint: " & lrInternalUniquenessConstraint.Id
                                        lsMessage &= vbCrLf & "Role.Id: " & lrRole.Id
                                        lsMessage &= vbCrLf & vbCrLf & "Fact Type's Roles: " & lrInternalUniquenessConstraint.Id
                                        For Each lrFTRole In lrFactType.RoleGroup
                                            lsMessage &= vbCrLf & "Role.Id: " & lrFTRole.Id
                                        Next
                                        MsgBox(lsMessage)
                                        'No point continuing, abort
                                        Exit Sub
                                    Else
                                        Call lrFactData.ClearModelErrors()
                                        lsDataValue = lrFact.GetFactDataByRoleId(lrRole.Id).Data
                                        lrFactData = New FBM.FactData(New FBM.Role(lrFactType, lrRole.Id, True), New FBM.Concept(lsDataValue))

                                        lrFactPredicate.data.Add(lrFactData)
                                    End If
                                Next

                                '--------------------------------------------------------------------
                                'Retrieve all the Facts from the FactType that match the predicate.
                                '--------------------------------------------------------------------                
                                lrFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.EqualsByRoleIdData)

                                If lrFactList.Count > 1 Then

                                    lsErrorMessage = "Population Uniqueness Error - "
                                    lsErrorMessage &= "Multiple Facts violate Internal Uniqueness Constraint, '" &
                                                      lrInternalUniquenessConstraint.Name & "'."
                                    lsErrorMessage &= " Values include, {"
                                    Dim liInd As Integer
                                    For liInd = 1 To lrFactPredicate.data.Count
                                        lsErrorMessage &= lrFactPredicate.data(liInd - 1).Data
                                        If liInd < lrFactPredicate.data.Count Then
                                            lsErrorMessage &= ", "
                                        End If
                                    Next
                                    lsErrorMessage &= "}"

                                    lrModelError = New FBM.ModelError(pcenumModelErrors.PopulationUniquenessError,
                                                                      lsErrorMessage,
                                                                      Nothing,
                                                                      lrFactType)
                                    For Each lrRole In lrInternalUniquenessConstraint.Role
                                        lrFact.GetFactDataByRoleId(lrRole.Id)._ModelError.Add(lrModelError)
                                    Next

                                    lrFactType._ModelError.Add(lrModelError)
                                    lrFact.AddModelError(lrModelError)
                                    Me.Model.AddModelError(lrModelError)
                                End If

                            Next 'Fact in the FactType
                        Next 'InternalUniquenessConstraint
                    End If

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
