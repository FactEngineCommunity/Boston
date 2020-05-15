Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class PredicatePart
        Inherits Viev.FBM.PredicatePart

        Public Overrides Sub Save()

            Try
                '-----------------------------------------
                'Saves the PredicatePart to the database
                '-----------------------------------------
                If tableORMPredicatePart.ExistsPredicatePart(Me) Then
                    Call tableORMPredicatePart.UpdatePredicatePart(Me)
                Else
                    Call tableORMPredicatePart.AddPredicatePart(Me)
                End If

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
