Imports System.Reflection

Namespace FEQL
    Partial Public Class Processor

        Public Function doesQueryEdgeHaveAnRingConstraint(ByRef arQueryEdge As FactEngine.QueryEdge) As Boolean

            Try
                If arQueryEdge.BaseNode.FBMModelObject IsNot arQueryEdge.TargetNode.FBMModelObject Then
                    Return False
                Else
                    For Each lrRingConstraint In Me.Model.RoleConstraint.FindAll(Function(x) x.RoleConstraintType = pcenumRoleConstraintType.RingConstraint)
                        If lrRingConstraint.Role(0).JoinedORMObject Is arQueryEdge.BaseNode.FBMModelObject Then
                            Return True
                        End If
                    Next
                End If

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

    End Class
End Namespace
