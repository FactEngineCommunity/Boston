Imports System.Reflection

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
            Dim lbErrorFound As Boolean = False
            Dim lrTopMostSupertype As FBM.EntityType

            Try

                For Each lrEntityType In Me.Model.EntityType
                    lbErrorFound = False
                    If (lrEntityType.IsObjectifyingEntityType = False) And
                        (lrEntityType.HasCompoundReferenceMode = False) And (lrEntityType.HasSimpleReferenceScheme = False) Then
                        If lrEntityType.SubtypeRelationship.Count > 0 Then
                            lrTopMostSupertype = lrEntityType.GetTopmostSupertype
                            If (lrTopMostSupertype.HasCompoundReferenceMode = False) And (lrTopMostSupertype.HasSimpleReferenceScheme = False) Then
                                lbErrorFound = True
                            End If
                        Else
                            lbErrorFound = True
                        End If
                    End If

                    If lbErrorFound Then
                        lsErrorMessage = "Entity Type Requires Reference Scheme Error - "
                        lsErrorMessage &= "Entity Type: '" &
                                          lrEntityType.Name & "'."

                        lrModelError = New FBM.ModelError(pcenumModelErrors.EntityTypeRequiresReferenceSchemeError,
                                                          lsErrorMessage,
                                                          Nothing,
                                                          lrEntityType)

                        lrEntityType.ModelError.Add(lrModelError)

                        Me.Model.AddModelError(lrModelError)
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
