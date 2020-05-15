Namespace Validation

    Public Class EntityTypeRequiresReferenceSchemeError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.New(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrEntityType As FBM.EntityType
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError


            For Each lrEntityType In Me.Model.EntityType

                If (lrEntityType.HasCompoundReferenceMode = False) And (lrEntityType.HasSimpleReferenceMode = False) Then

                    lsErrorMessage = "Entity Type Requires Reference Scheme Error - "
                    lsErrorMessage &= "Entity Type: '" & _
                                      lrEntityType.Name & "'."

                    lrModelError = New FBM.ModelError(pcenumModelErrors.EntityTypeRequiresReferenceSchemeError, _
                                                      lsErrorMessage, _
                                                      Nothing, _
                                                      lrEntityType)

                    lrEntityType.ModelError.Add(lrModelError)

                    Me.Model.AddModelError(lrModelError)

                End If
            Next

        End Sub

    End Class

End Namespace
