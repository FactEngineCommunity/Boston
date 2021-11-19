Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddRoleConstraint(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Dim lrInterfaceRoleConstraint As Viev.FBM.Interface.RoleConstraint
            Dim lrRoleConstraint As New FBM.RoleConstraint


            lrInterfaceRoleConstraint = arInterfaceModel.RoleConstraint(0)

            lrRoleConstraint.Id = lrInterfaceRoleConstraint.Id
            lrRoleConstraint.Model = arModel
            lrRoleConstraint.Name = lrInterfaceRoleConstraint.Name
            lrRoleConstraint.IsMDAModelElement = lrInterfaceRoleConstraint.IsMDAModelElement
            lrRoleConstraint.IsPreferredIdentifier = lrInterfaceRoleConstraint.IsPreferredIdentifier
            lrRoleConstraint.ShortDescription = lrInterfaceRoleConstraint.ShortDescription
            lrRoleConstraint.LongDescription = lrInterfaceRoleConstraint.LongDescription
            lrRoleConstraint.MaximumFrequencyCount = lrInterfaceRoleConstraint.MaximumFrequencyCount
            lrRoleConstraint.MinimumFrequencyCount = lrInterfaceRoleConstraint.MinimumFrequencyCount
            lrRoleConstraint.RingConstraintType.GetByDescription(lrInterfaceRoleConstraint.RingConstraintType.ToString)
            lrRoleConstraint.RoleConstraintType.GetByDescription(lrInterfaceRoleConstraint.RoleConstraintType.ToString)
            lrRoleConstraint.IsDeontic = lrInterfaceRoleConstraint.IsDeontic
            lrRoleConstraint.Cardinality = lrInterfaceRoleConstraint.Cardinality
            lrRoleConstraint.CardinalityRangeType.GetByDescription(lrInterfaceRoleConstraint.CardinalityRangeType.ToString)

            For Each lrInterfaceRoleConstraintRole In lrInterfaceRoleConstraint.RoleConstraintRoles

                Dim lrRoleConstraintRole As New FBM.RoleConstraintRole
                Dim lrRole As FBM.Role

                lrRoleConstraintRole.Model = arModel

                lrRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceRoleConstraintRole.RoleId)

                lrRoleConstraint.Role.Add(lrRole)
                lrRoleConstraintRole.Role = lrRole
                lrRoleConstraintRole.RoleConstraint = lrRoleConstraint

                If lrInterfaceRoleConstraintRole.ArgumentId <> "" Then
                    lrRoleConstraintRole.RoleConstraintArgument = lrRoleConstraint.Argument.Find(Function(x) x.Id = lrInterfaceRoleConstraintRole.ArgumentId)
                    lrRoleConstraintRole.RoleConstraintArgument.Id = lrInterfaceRoleConstraintRole.ArgumentId
                    lrRoleConstraintRole.ArgumentSequenceNr = lrInterfaceRoleConstraintRole.ArgumentSequenceNr
                End If

                lrRoleConstraintRole.SequenceNr = lrInterfaceRoleConstraintRole.SequenceNr

                lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
            Next

            If lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                lrRoleConstraint.LevelNr = lrRoleConstraint.RoleConstraintRole(0).Role.FactType.InternalUniquenessConstraint.Count + 1
            End If

            arModel.AddRoleConstraint(lrRoleConstraint, True, False)

            If lrRoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then

                Dim lrFactType As FBM.FactType = lrRoleConstraint.RoleConstraintRole(0).Role.FactType

                Call lrFactType.AddInternalUniquenessConstraint(lrRoleConstraint)

            End If

            If arInterfaceModel.Page IsNot Nothing Then
                Dim lrPage As FBM.Page = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                Dim lrPointF As New PointF(arInterfaceModel.Page.ConceptInstance.X, arInterfaceModel.Page.ConceptInstance.Y)
                Select Case lrRoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        Call lrPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, lrPointF)
                    Case Else
                        Call lrPage.DropRoleConstraintAtPoint(lrRoleConstraint, lrPointF)
                End Select

            End If

            Call arModel.MakeDirty(True, True)

        End Sub

        Private Sub HandleModelDeleteRoleConstraint(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceRoleConstraint As Viev.FBM.Interface.RoleConstraint
                Dim lrRoleConstraint As New FBM.RoleConstraint


                lrInterfaceRoleConstraint = arInterfaceModel.RoleConstraint(0)

                lrRoleConstraint.Id = lrInterfaceRoleConstraint.Id
                lrRoleConstraint.Model = arModel
                lrRoleConstraint.Name = lrInterfaceRoleConstraint.Name
                lrRoleConstraint.RoleConstraintType.GetByDescription(lrInterfaceRoleConstraint.RoleConstraintType.ToString)

                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrRoleConstraint.Id)

                Dim lrRole As FBM.Role
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                For Each lrInterfaceRoleConstraintRole In lrInterfaceRoleConstraint.RoleConstraintRoles
                    lrRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceRoleConstraintRole.RoleId)
                    lrRoleConstraintRole = New FBM.RoleConstraintRole(lrRole, lrRoleConstraint)
                    lrRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr
                    lrRoleConstraint.RoleConstraintRole.Add(lrRoleConstraintRole)
                Next

                Call lrRoleConstraint.RemoveFromModel(True, True, False)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelAddRoleConstraintArgument(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceRoleConstraint As Viev.FBM.Interface.RoleConstraint
                Dim lrRoleConstraint As New FBM.RoleConstraint

                lrInterfaceRoleConstraint = arInterfaceModel.RoleConstraint(0)

                lrRoleConstraint.Id = lrInterfaceRoleConstraint.Id
                lrRoleConstraint.Model = arModel
                lrRoleConstraint.Name = lrInterfaceRoleConstraint.Name

                Dim lrInterfaceRCArgument As Viev.FBM.Interface.RoleConstraintArgument


                lrInterfaceRCArgument = arInterfaceModel.RoleConstraint(0).Argument(0)

                Dim lrRCArgument As New FBM.RoleConstraintArgument(lrRoleConstraint,
                                                                   lrInterfaceRCArgument.SequenceNr,
                                                                   lrInterfaceRCArgument.Id)

                lrRoleConstraint = arModel.RoleConstraint.Find(Function(x) x.Id = lrRoleConstraint.Id)

                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                For Each lrInterfaceRoleconstraintRole In lrInterfaceRCArgument.RoleConstraintRole

                    lrRoleConstraintRole = New FBM.RoleConstraintRole
                    lrRoleConstraintRole.RoleConstraintArgument = lrRCArgument
                    lrRoleConstraintRole.ArgumentSequenceNr = lrInterfaceRoleconstraintRole.ArgumentSequenceNr
                    lrRoleConstraintRole.Role = arModel.Role.Find(Function(x) x.Id = lrInterfaceRoleconstraintRole.RoleId)
                    lrRoleConstraintRole.SequenceNr = lrInterfaceRoleconstraintRole.SequenceNr
                    lrRoleConstraintRole.RoleConstraint = lrRoleConstraint
                    lrRoleConstraintRole.Model = arModel

                    lrRCArgument.RoleConstraintRole.Add(lrRoleConstraintRole)
                Next

                lrRCArgument.JoinPath = New FBM.JoinPath
                Dim lrRole As FBM.Role
                For Each lrInterfaceRoleReference In lrInterfaceRCArgument.JoinPath.RolePath
                    lrRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceRoleReference.RoleId)
                    lrRCArgument.JoinPath.RolePath.AddUnique(lrRole)
                Next

                lrRCArgument.JoinPath.JoinPathError.GetByDescription(lrInterfaceRCArgument.JoinPath.JoinPathError.ToString)

                Call lrRoleConstraint.AddArgument(lrRCArgument, False)

                For Each lrRoleConstraintRole In lrRCArgument.RoleConstraintRole
                    Call lrRoleConstraint.AddRoleConstraintRole(lrRoleConstraintRole)
                Next

                Call arModel.MakeDirty(False, True)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
