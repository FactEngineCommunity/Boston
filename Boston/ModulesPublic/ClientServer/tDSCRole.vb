Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddRoleToFactType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Dim lrInterfaceFactType As Viev.FBM.Interface.FactType
            Dim lrInterfaceRole As Viev.FBM.Interface.Role
            Dim lrFactType As FBM.FactType
            Dim lrRole As FBM.Role

            Try
                lrInterfaceFactType = arInterfaceModel.FactType(0)
                lrInterfaceRole = lrInterfaceFactType.RoleGroup(0)

                lrFactType = New FBM.FactType(arModel, lrInterfaceFactType.Name, True)
                lrFactType.IsIndependent = lrInterfaceFactType.IsIndependent
                lrFactType.IsStored = lrInterfaceFactType.IsStored

                Dim lrJoinedModelElement As New FBM.ModelObject

                lrJoinedModelElement = arModel.GetModelObjectByName(lrInterfaceRole.JoinedObjectTypeId)

                'CodeSafe: Exit sub if there is no ModelElement
                If lrJoinedModelElement Is Nothing Then Exit Sub

                lrRole = New FBM.Role(lrFactType,
                                      lrInterfaceRole.Id,
                                      False,
                                      lrJoinedModelElement)

                lrInterfaceRole.Mandatory = lrRole.Mandatory
                lrInterfaceRole.Name = lrRole.Name

                lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType.
                If lrFactType Is Nothing Then Exit Sub

                Call lrFactType.AddRole(lrRole, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelRoleReassignJoinedModelObject(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceRole As Viev.FBM.Interface.Role
                lrInterfaceRole = arInterfaceModel.FactType(0).RoleGroup(0)

                Dim lrRole As FBM.Role
                lrRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceRole.Id)

                'CodeSafe: Exit sub if there is no Role.
                If lrRole Is Nothing Then Exit Sub

                Dim lrModelObject As FBM.ModelObject
                lrModelObject = arModel.GetModelObjectByName(lrInterfaceRole.JoinedObjectTypeId)

                'CodeSafe: Exit sub if there is no ModelElement
                If lrModelObject Is Nothing Then Exit Sub

                Call lrRole.ReassignJoinedModelObject(lrModelObject, False, Nothing)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelUpdateRole(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceRole As Viev.FBM.Interface.Role
                lrInterfaceRole = arInterfaceModel.FactType(0).RoleGroup(0)

                Dim lrRole As FBM.Role
                lrRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceRole.Id)

                'CodeSafe: Exit sub if there is no Role
                If lrRole Is Nothing Then Exit Sub

                If lrInterfaceRole.Name <> lrRole.Name Then
                    Call lrRole.setName(lrInterfaceRole.Name, False)
                ElseIf lrInterfaceRole.Mandatory <> lrRole.Mandatory Then
                    Call lrRole.SetMandatory(lrInterfaceRole.Mandatory, False)
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
