Imports System.Reflection

Namespace Validation

    Public Class ModelElementAppearsOnNoPageError
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

                Dim larPageInstance = (From Page In Me.Model.Page
                                       From ModelElementInstance In Page.GetAllPageObjects
                                       Select ModelElementInstance.Id).Distinct

                For Each lrModelElement In Me.Model.getModelObjects

                    lbErrorFound = False

                    If lrModelElement.GetType = GetType(FBM.ValueType) Then
                        If CType(lrModelElement, FBM.ValueType).isReferenceModeValueType Then
                            GoTo SkipModelElement
                        End If
                    End If

                    lbErrorFound = Not larPageInstance.Contains(lrModelElement.Id)

                    If lbErrorFound Then
                        lsErrorMessage = "Model Element appears on no Page -"
                        lsErrorMessage &= "Model element: '" &
                                          lrModelElement.Id & "'."

                        lrModelError = New FBM.ModelError(pcenumModelErrors.ModelElementAppearsOnNoPage,
                                                          lsErrorMessage,
                                                          Nothing,
                                                          lrModelElement)

                        lrModelElement._ModelError.AddUnique(lrModelError)

                        Me.Model.AddModelError(lrModelError)
                    Else
SkipModelElement:
                        'CodeSafe
                        lrModelElement._ModelError.RemoveAll(Function(x) x.ErrorId = pcenumModelErrors.ModelElementAppearsOnNoPage)
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
