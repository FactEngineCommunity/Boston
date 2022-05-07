Imports System.Reflection

Namespace Validation

    Public Class DataTypeNotSpecifiedError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.New(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrValueType As FBM.ValueType
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError

            Try

                For Each lrValueType In Me.Model.ValueType

                    If lrValueType.DataType = pcenumORMDataType.DataTypeNotSet Then

                        lsErrorMessage = "Data Type Not Specified Error - "
                        lsErrorMessage &= "Value Type: '" &
                                          lrValueType.Name & "'."

                        lrModelError = New FBM.ModelError(pcenumModelErrors.DataTypeNotSpecifiedError,
                                                          lsErrorMessage,
                                                          Nothing,
                                                          lrValueType)

                        lrValueType.ModelError.Add(lrModelError)
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
