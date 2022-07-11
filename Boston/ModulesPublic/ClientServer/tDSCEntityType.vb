Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddEntityType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceEntityType As Viev.FBM.Interface.EntityType = arInterfaceModel.EntityType(0)

                Dim lrEntityType As New FBM.EntityType(arModel,
                                           pcenumLanguage.ORMModel,
                                           lrInterfaceEntityType.Id,
                                           lrInterfaceEntityType.Id,
                                           Nothing)

                Call arModel.AddEntityType(lrEntityType, True, False)

                If arInterfaceModel.Page IsNot Nothing Then
                    Dim lrPage As FBM.Page = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                    Dim lrPointF As New PointF(arInterfaceModel.Page.ConceptInstance.X, arInterfaceModel.Page.ConceptInstance.Y)
                    Call lrPage.DropEntityTypeAtPoint(lrEntityType, lrPointF)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelDeleteEntityType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceEntityType As Viev.FBM.Interface.EntityType
                lrInterfaceEntityType = arInterfaceModel.EntityType(0)

                Dim lrEntityType As FBM.EntityType
                lrEntityType = arModel.EntityType.Find(Function(x) x.Id = lrInterfaceEntityType.Id)

                'CodeSafe: Exit sub if nothing to remove.
                If lrEntityType Is Nothing Then Exit Sub

                Call lrEntityType.RemoveFromModel(True, True, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelUpdateEntityType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try

                Dim lrInterfaceEntityType As Viev.FBM.Interface.EntityType
                lrInterfaceEntityType = arInterfaceModel.EntityType(0)

                Dim lrEntityType As FBM.EntityType
                lrEntityType = arModel.EntityType.Find(Function(x) x.Id = lrInterfaceEntityType.Id)

                'CodeSafe: Exit sub if nothing to remove.
                If lrEntityType Is Nothing Then Exit Sub

                lrEntityType.PreferredIdentifierRCId = lrInterfaceEntityType.ReferenceSchemeRoleConstraintId
                If lrEntityType.PreferredIdentifierRCId <> "" Then
                    lrEntityType.ReferenceModeRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrEntityType.PreferredIdentifierRCId)
                End If

                If lrInterfaceEntityType.Name <> lrEntityType.Name Then
                    Call lrEntityType.SetName(lrInterfaceEntityType.Name, False)
                ElseIf lrInterfaceEntityType.IsIndependent <> lrEntityType.IsIndependent Then
                    Call lrEntityType.SetIsIndependent(lrInterfaceEntityType.IsIndependent, False)
                ElseIf lrInterfaceEntityType.IsPersonal <> lrEntityType.IsPersonal Then
                    Call lrEntityType.SetIsPersonal(lrInterfaceEntityType.IsPersonal, False)
                ElseIf lrInterfaceEntityType.IsAbsorbed <> lrEntityType.IsAbsorbed Then
                    Call lrEntityType.SetIsAbsorbed(lrInterfaceEntityType.IsAbsorbed, False)
                ElseIf lrInterfaceEntityType.ReferenceMode <> lrEntityType.ReferenceMode Then
                    Call lrEntityType.SetReferenceMode(lrInterfaceEntityType.ReferenceMode, True, Nothing, False)
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
