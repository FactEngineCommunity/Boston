Imports Viev.FBM.PublicConstants


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
            For Each lrFactType In Me.Model.FactType

                If lrFactType.Fact.Count > 1 Then


                    For Each lrInternalUniquenessConstraint In lrFactType.InternalUniquenessConstraint
                        '---------------------------------------------------------------------------
                        'Find the Count of Facts that have matching FactData for the span of Roles 
                        '  in the InternalUniquenessConstraint
                        '---------------------------------------------------------------------------

                        For Each lrFact In lrFactType.Fact

                            lrFactPredicate = New FBM.FactPredicate

                            For Each lrRole In lrInternalUniquenessConstraint.Role

                                Call lrFact.GetFactDataByRoleId(lrRole.Id).ClearModelErrors()
                                lsDataValue = lrFact.GetFactDataByRoleId(lrRole.Id).Data
                                lrFactData = New FBM.FactData(New FBM.Role(lrFactType, lrRole.Id, True), New FBM.Concept(lsDataValue))

                                lrFactPredicate.data.Add(lrFactData)
                            Next

                            '--------------------------------------------------------------------
                            'Retrieve all the Facts from the FactType that match the predicate.
                            '--------------------------------------------------------------------                
                            lrFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.EqualsByRoleIdData)

                            If lrFactList.Count > 1 Then

                                lsErrorMessage = "Population Uniqueness Error - "
                                lsErrorMessage &= "Multiple Facts violate Internal Uniqueness Constraint, '" & _
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

                                lrModelError = New FBM.ModelError(pcenumModelErrors.PopulationUniquenessError, _
                                                                  lsErrorMessage, _
                                                                  Nothing, _
                                                                  lrFactType)
                                For Each lrRole In lrInternalUniquenessConstraint.Role
                                    lrFact.GetFactDataByRoleId(lrRole.Id).ModelError.Add(lrModelError)
                                Next

                                lrFactType.ModelError.Add(lrModelError)
                                lrFact.ModelError.Add(lrModelError)
                                Me.Model.AddModelError(lrModelError)
                            End If

                        Next 'Fact in the FactType
                    Next 'InternalUniquenessConstraint
                End If

            Next

        End Sub

    End Class

End Namespace
