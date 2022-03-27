Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports BostonWCFServiceLibrary.DuplexService
Imports System.Reflection

Namespace DuplexServiceClient

    Public Class DuplexServiceClient
        Inherits DuplexClientBase(Of BostonWCFServiceLibrary.IDuplexService)
        Implements BostonWCFServiceLibrary.IDuplexService

        Public Tube As New Tube(10)

        Public Sub New(callbackInstance As InstanceContext, binding As WSDualHttpBinding, endpointAddress As EndpointAddress)
            MyBase.New(callbackInstance, binding, endpointAddress)
        End Sub

        Public Sub Connect() Implements BostonWCFServiceLibrary.IDuplexService.Connect
            Channel.Connect()
        End Sub

        Public Sub Disconnect() Implements BostonWCFServiceLibrary.IDuplexService.Disconnect
            Channel.Disconnect()
        End Sub

        Public Sub SendBroadcast(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                 ByRef arBroadcast As Viev.FBM.Interface.Broadcast) Implements BostonWCFServiceLibrary.IDuplexService.SendBroadcast
            Try

                Channel.SendBroadcast(aiBroadcastType, arBroadcast)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage1 = "Error sending Client/Server Broadcast. The session may have timed out."
                lsMessage1 &= "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub BroadcastInvitationToDuplexService(ByRef aiInterfaceBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                                      ByRef arBroadcast As Viev.FBM.Interface.Broadcast)

            Call prDuplexServiceClient.SendBroadcast(aiInterfaceBroadcastType, arBroadcast)

        End Sub

        '====================================================
        Public Sub BroadcastToDuplexService(ByRef aiInterfaceBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                            ByRef arModelElement As FBM.ModelObject,
                                            ByRef arConceptInstance As FBM.ConceptInstance)

            Try
                '=====================================================================================
                'Broadcast the addition to the DuplexServer
                If My.Settings.UseClientServer _
                    And My.Settings.InitialiseClient Then

                    Dim lrInterfaceModel As New Viev.FBM.Interface.Model
                    lrInterfaceModel.ModelId = arModelElement.Model.ModelId
                    lrInterfaceModel.Name = arModelElement.Model.Name
                    lrInterfaceModel.Namespace = prApplication.WorkingNamespace.Id
                    lrInterfaceModel.ProjectId = prApplication.WorkingProject.Id
                    lrInterfaceModel.StoreAsXML = arModelElement.Model.StoreAsXML

                    '=============================================================================
                    If arConceptInstance IsNot Nothing Then
                        Dim lrInterfacePage As New Viev.FBM.Interface.Page
                        lrInterfacePage.Id = arConceptInstance.PageId

                        Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance
                        lrInterfaceConceptInstance.ModelElementId = arModelElement.Id
                        lrInterfaceConceptInstance.X = arConceptInstance.X
                        lrInterfaceConceptInstance.Y = arConceptInstance.Y

                        lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                        lrInterfaceModel.Page = lrInterfacePage
                    End If
                    '=============================================================================

                    Select Case aiInterfaceBroadcastType
                        Case Is = [Interface].pcenumBroadcastType.PageRemovePageObject

                            Dim lrInterfacePage As New Viev.FBM.Interface.Page
                            Dim lrInterfaceConceptInstance As New Viev.FBM.Interface.ConceptInstance

                            lrInterfacePage.Id = arConceptInstance.PageId

                            lrInterfaceConceptInstance.ModelElementId = arModelElement.Id

                            lrInterfacePage.ConceptInstance = lrInterfaceConceptInstance
                            lrInterfaceModel.Page = lrInterfacePage

                        Case Is = [Interface].pcenumBroadcastType.ModelAddFactTypeReading,
                                  [Interface].pcenumBroadcastType.ModelUpdateFactTypeReading,
                                  [Interface].pcenumBroadcastType.ModelDeleteFactTypeReading

                            Dim lrFactTypeReading As FBM.FactTypeReading = CType(arModelElement, FBM.FactTypeReading)
                            Dim lrFactType = lrFactTypeReading.FactType

                            Dim lrInterfaceFactTypeReading As New Viev.FBM.Interface.FactTypeReading

                            lrInterfaceFactTypeReading.Id = lrFactTypeReading.Id
                            lrInterfaceFactTypeReading.FrontReadingText = lrFactTypeReading.FrontText
                            lrInterfaceFactTypeReading.FollowingReadingText = lrFactTypeReading.FollowingText

                            Dim lrInterfacePredicatePart As Viev.FBM.Interface.PredicatePart
                            For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                                lrInterfacePredicatePart = New Viev.FBM.Interface.PredicatePart
                                lrInterfacePredicatePart.SequenceNr = lrPredicatePart.SequenceNr
                                lrInterfacePredicatePart.Role_Id = lrPredicatePart.Role.Id
                                lrInterfacePredicatePart.PreboundReadingText = lrPredicatePart.PreBoundText
                                lrInterfacePredicatePart.PostboundReadingText = lrPredicatePart.PostBoundText
                                lrInterfacePredicatePart.PredicatePartText = lrPredicatePart.PredicatePartText

                                lrInterfaceFactTypeReading.PredicatePart.Add(lrInterfacePredicatePart)
                            Next

                            Dim lrInterfaceFactType As New Viev.FBM.Interface.FactType
                            lrInterfaceFactType.Id = lrFactType.Id

                            lrInterfaceFactType.FactTypeReadings.Add(lrInterfaceFactTypeReading)

                            lrInterfaceModel.FactType.Add(lrInterfaceFactType)

                        Case Is = [Interface].pcenumBroadcastType.ModelAddRoleToFactType

                            Dim lrRole As FBM.Role = CType(arModelElement, FBM.Role)
                            Dim lrFactType = lrRole.FactType

                            Dim lrInterfaceFactType As New Viev.FBM.Interface.FactType
                            lrInterfaceFactType.Id = lrFactType.Id
                            lrInterfaceFactType.Name = lrFactType.Name
                            lrInterfaceFactType.IsIndependent = lrFactType.IsIndependent
                            lrInterfaceFactType.IsStored = lrFactType.IsStored

                            Dim lrInterfaceRole As Viev.FBM.Interface.Role

                            lrInterfaceRole = New Viev.FBM.Interface.Role
                            lrInterfaceRole.Id = lrRole.Id
                            lrInterfaceRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                            lrInterfaceRole.Mandatory = lrRole.Mandatory
                            lrInterfaceRole.Name = lrRole.Name
                            lrInterfaceRole.SequenceNr = lrRole.SequenceNr

                            lrInterfaceFactType.RoleGroup.Add(lrInterfaceRole)

                            lrInterfaceModel.FactType.Add(lrInterfaceFactType)

                        Case Is = [Interface].pcenumBroadcastType.ModelAddRoleConstraintArgument

                            Dim lrRCArgument As FBM.RoleConstraintArgument = arModelElement

                            Dim lrInterfaceRoleConstraint As New Viev.FBM.Interface.RoleConstraint
                            lrInterfaceRoleConstraint.Id = lrRCArgument.RoleConstraint.Id
                            lrInterfaceRoleConstraint.Name = lrRCArgument.RoleConstraint.Name
                            lrInterfaceRoleConstraint.RingConstraintType.GetByDescription(lrRCArgument.RoleConstraint.RingConstraintType.ToString)
                            lrInterfaceRoleConstraint.RoleConstraintType.GetByDescription(lrRCArgument.RoleConstraint.RoleConstraintType.ToString)

                            Dim lrInterfaceArgument As New Viev.FBM.Interface.RoleConstraintArgument
                            lrInterfaceArgument.Id = lrRCArgument.Id
                            lrInterfaceArgument.SequenceNr = lrRCArgument.SequenceNr

                            For Each lrRoleConstraintRole In lrRCArgument.RoleConstraintRole

                                Dim lrInterfaceRoleConstraintRole As New Viev.FBM.Interface.RoleConstraintRole

                                If lrRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                                    lrInterfaceRoleConstraintRole.ArgumentId = lrRoleConstraintRole.RoleConstraintArgument.Id
                                    lrInterfaceRoleConstraintRole.ArgumentSequenceNr = lrRoleConstraintRole.ArgumentSequenceNr
                                End If

                                lrInterfaceRoleConstraintRole.RoleId = lrRoleConstraintRole.Role.Id
                                lrInterfaceRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr

                                lrInterfaceRoleConstraint.RoleConstraintRoles.Add(lrInterfaceRoleConstraintRole)

                                lrInterfaceArgument.RoleConstraintRole.Add(lrInterfaceRoleConstraintRole)
                            Next

                            lrInterfaceArgument.JoinPath = New Viev.FBM.Interface.JoinPath
                            Dim lrInterfaceRoleReference As Viev.FBM.Interface.RoleReference
                            For Each lrRole In lrRCArgument.JoinPath.RolePath
                                lrInterfaceRoleReference = New Viev.FBM.Interface.RoleReference
                                lrInterfaceRoleReference.RoleId = lrRole.Id

                                lrInterfaceArgument.JoinPath.RolePath.AddUnique(lrInterfaceRoleReference)
                            Next

                            lrInterfaceArgument.JoinPath.JoinPathError.GetByDescription(lrRCArgument.JoinPath.JoinPathError.ToString)

                            '--------------------------------------------------------------------
                            'Add the InterfaceArgument to the InterfaceRoleConstraint
                            lrInterfaceRoleConstraint.Argument.Add(lrInterfaceArgument)

                            '--------------------------------------------------------------------
                            'Add the InterfaceRoleConstraint to the InterfaceModel
                            lrInterfaceModel.RoleConstraint.Add(lrInterfaceRoleConstraint)


                        Case Else

                            Select Case arModelElement.ConceptType
                                Case Is = pcenumConceptType.Role

                                    Dim lrRole = CType(arModelElement, FBM.Role)
                                    'lrRole = arModelElement

                                    Dim lrInterfaceFactType As New Viev.FBM.Interface.FactType
                                    Dim lrInterfaceRole As New Viev.FBM.Interface.Role
                                    lrInterfaceRole.Id = lrRole.Id
                                    lrInterfaceRole.Name = lrRole.Name
                                    lrInterfaceRole.SequenceNr = lrRole.SequenceNr
                                    lrInterfaceRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                                    lrInterfaceRole.Mandatory = lrRole.Mandatory

                                    lrInterfaceFactType.RoleGroup.Add(lrInterfaceRole)
                                    lrInterfaceModel.FactType.Add(lrInterfaceFactType)

                                Case Is = pcenumConceptType.ValueType

                                    Dim lrValueType = CType(arModelElement, FBM.ValueType)

                                    Dim lrInterfaceValueType As New Viev.FBM.Interface.ValueType
                                    lrInterfaceValueType.Id = lrValueType.Id
                                    lrInterfaceValueType.Name = lrValueType.Name
                                    lrInterfaceValueType.DataType.GetByDescription(lrValueType.DataType.ToString)
                                    lrInterfaceValueType.DataTypeLength = lrValueType.DataTypeLength
                                    lrInterfaceValueType.DataTypePrecision = lrValueType.DataTypePrecision

                                    lrInterfaceModel.ValueType.Add(lrInterfaceValueType)

                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityType = CType(arModelElement, FBM.EntityType)

                                    Dim lrInterfaceEntityType As New Viev.FBM.Interface.EntityType
                                    lrInterfaceEntityType.Id = lrEntityType.Id
                                    lrInterfaceEntityType.Name = lrEntityType.Name
                                    lrInterfaceEntityType.IsIndependent = lrEntityType.IsIndependent
                                    lrInterfaceEntityType.IsPersonal = lrEntityType.IsPersonal
                                    lrInterfaceEntityType.IsAbsorbed = lrEntityType.IsAbsorbed
                                    lrInterfaceEntityType.ReferenceMode = lrEntityType.ReferenceMode
                                    lrInterfaceEntityType.ReferenceSchemeRoleConstraintId = lrEntityType.PreferredIdentifierRCId

                                    lrInterfaceModel.EntityType.Add(lrInterfaceEntityType)

                                Case Is = pcenumConceptType.FactType

                                    Dim lrFactType = CType(arModelElement, FBM.FactType)

                                    Dim lrInterfaceFactType As New Viev.FBM.Interface.FactType
                                    lrInterfaceFactType.Id = lrFactType.Id
                                    lrInterfaceFactType.Name = lrFactType.Name
                                    lrInterfaceFactType.IsIndependent = lrFactType.IsIndependent
                                    lrInterfaceFactType.IsStored = lrFactType.IsStored
                                    lrInterfaceFactType.IsDerived = lrFactType.IsDerived
                                    lrInterfaceFactType.DerivationText = lrFactType.DerivationText
                                    lrInterfaceFactType.IsObjectified = lrFactType.IsObjectified
                                    If lrFactType.ObjectifyingEntityType IsNot Nothing Then
                                        lrInterfaceFactType.ObjectifyingEntityTypeId = lrFactType.ObjectifyingEntityType.Id
                                    End If
                                    If lrFactType.LinkFactTypeRole IsNot Nothing Then
                                        lrInterfaceFactType.LinkFactTypeRoleId = lrFactType.LinkFactTypeRole.Id
                                    End If

                                    Dim lrInterfaceRole As Viev.FBM.Interface.Role
                                    For Each lrRole In lrFactType.RoleGroup
                                        lrInterfaceRole = New Viev.FBM.Interface.Role
                                        lrInterfaceRole.Id = lrRole.Id
                                        lrInterfaceRole.JoinedObjectTypeId = lrRole.JoinedORMObject.Id
                                        lrInterfaceRole.Mandatory = lrRole.Mandatory
                                        lrInterfaceRole.Name = lrRole.Name
                                        lrInterfaceRole.SequenceNr = lrRole.SequenceNr

                                        lrInterfaceFactType.RoleGroup.Add(lrInterfaceRole)
                                    Next

                                    lrInterfaceModel.FactType.Add(lrInterfaceFactType)

                                Case Is = pcenumConceptType.RoleConstraint

                                    Dim lrRoleConstraint As FBM.RoleConstraint = arModelElement

                                    Dim lrInterfaceRoleConstraint As New Viev.FBM.Interface.RoleConstraint
                                    lrInterfaceRoleConstraint.Id = lrRoleConstraint.Id
                                    lrInterfaceRoleConstraint.Name = lrRoleConstraint.Name
                                    lrInterfaceRoleConstraint.RoleConstraintType.GetByDescription(lrRoleConstraint.RoleConstraintType.ToString)
                                    lrInterfaceRoleConstraint.IsMDAModelElement = lrRoleConstraint.IsMDAModelElement
                                    lrInterfaceRoleConstraint.IsPreferredIdentifier = lrRoleConstraint.IsPreferredIdentifier
                                    lrInterfaceRoleConstraint.ShortDescription = lrRoleConstraint.ShortDescription
                                    lrInterfaceRoleConstraint.LongDescription = lrRoleConstraint.LongDescription
                                    lrInterfaceRoleConstraint.MaximumFrequencyCount = lrRoleConstraint.MaximumFrequencyCount
                                    lrInterfaceRoleConstraint.MinimumFrequencyCount = lrRoleConstraint.MinimumFrequencyCount
                                    lrInterfaceRoleConstraint.RingConstraintType.GetByDescription(lrRoleConstraint.RingConstraintType.ToString)
                                    lrInterfaceRoleConstraint.RoleConstraintType.GetByDescription(lrRoleConstraint.RoleConstraintType.ToString)
                                    lrInterfaceRoleConstraint.IsDeontic = lrRoleConstraint.IsDeontic
                                    lrInterfaceRoleConstraint.Cardinality = lrRoleConstraint.Cardinality
                                    lrInterfaceRoleConstraint.CardinalityRangeType.GetByDescription(lrRoleConstraint.CardinalityRangeType.ToString)

                                    For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole

                                        Dim lrInterfaceRoleConstraintRole As New Viev.FBM.Interface.RoleConstraintRole

                                        If lrRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                                            lrInterfaceRoleConstraintRole.ArgumentId = lrRoleConstraintRole.RoleConstraintArgument.Id
                                            lrInterfaceRoleConstraintRole.ArgumentSequenceNr = lrRoleConstraintRole.ArgumentSequenceNr
                                        End If

                                        lrInterfaceRoleConstraintRole.RoleId = lrRoleConstraintRole.Role.Id
                                        lrInterfaceRoleConstraintRole.SequenceNr = lrRoleConstraintRole.SequenceNr

                                        lrInterfaceRoleConstraint.RoleConstraintRoles.Add(lrInterfaceRoleConstraintRole)
                                    Next

                                    lrInterfaceModel.RoleConstraint.Add(lrInterfaceRoleConstraint)

                            End Select

                    End Select

                    Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                    lrBroadcast.Model = lrInterfaceModel

                    Call prDuplexServiceClient.SendBroadcast(aiInterfaceBroadcastType, lrBroadcast)

                End If
                '=====================================================================================

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        '====================================================
        Public Sub HandleBroadcastReceived(sender As Object, e As Broadcast)

            Try

                prDuplexServiceClient.Tube.Add(e.BroadcastType) 'For displaying in frmBroadcastEventMonitor

                Dim lrInterfaceBroadcast As Viev.FBM.Interface.Broadcast
                lrInterfaceBroadcast = CType(e.Broadcast, Viev.FBM.Interface.Broadcast)

                If lrInterfaceBroadcast.Model IsNot Nothing Then
                    Dim lrInterfaceModel As Viev.FBM.Interface.Model
                    lrInterfaceModel = CType(e.Broadcast.Model, Viev.FBM.Interface.Model)

                    Dim lrModel As FBM.Model
                    lrModel = prApplication.Models.Find(Function(x) x.ModelId = lrInterfaceModel.ModelId)

                    'Model save method may have changed.
                    lrModel.StoreAsXML = lrInterfaceModel.StoreAsXML

                    If lrModel Is Nothing And
                        Not e.BroadcastType = [Interface].pcenumBroadcastType.AddModel Then Exit Sub 'Nothing more to do here.

                    Call Me.HandleModelBroadcastReceived(e.BroadcastType, lrModel, lrInterfaceModel)

                ElseIf lrInterfaceBroadcast.Invitation IsNot Nothing Then

                    Dim lrInterfaceInvitation As Viev.FBM.Interface.Invitation
                    lrInterfaceInvitation = CType(e.Broadcast.Invitation, Viev.FBM.Interface.Invitation)

                    If lrInterfaceInvitation.InvitedUserId <> prApplication.User.Id Then Exit Sub

                    Call Me.HandleInvitationBroadcastReceived(lrInterfaceInvitation)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub HandleInvitationBroadcastReceived(ByRef arInterfaceInvitation As Viev.FBM.Interface.Invitation)

            If frmMain.zfrmModelExplorer Is Nothing Then
                Exit Sub
            Else
                Select Case arInterfaceInvitation.InvitationType
                    Case Is = [Interface].pcenumInvitationType.UserToJoinGroup
                        Call frmMain.zfrmModelExplorer.loadUserToJoinGroupInvitations()
                    Case Is = [Interface].pcenumInvitationType.UserToJoinProject
                        Call frmMain.zfrmModelExplorer.loadUserToJoinProjectInvitations(arInterfaceInvitation.InvitedToJoinProjectId)
                End Select

            End If

        End Sub


        Public Sub HandleModelBroadcastReceived(ByVal aiBroadcastType As Viev.FBM.Interface.pcenumBroadcastType,
                                                ByRef arModel As FBM.Model,
                                                ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Select Case aiBroadcastType
                Case Is = [Interface].pcenumBroadcastType.ModelSaved
                    Call Me.HandleModelSaved(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.DeleteModel
                    Call Me.HandleDeleteModel(arModel)
                Case Is = [Interface].pcenumBroadcastType.AddModel
                    Call Me.HandleAddModel(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddPage
                    Call Me.HandleModelAddPage(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.SaveModel
                    Call Me.HandleSaveModel(arModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddFactTypeReading
                    Call Me.HandleModelAddFactTypeReading(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddRoleToFactType
                    Call Me.HandleModelAddRoleToFactType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddRoleConstraint
                    Call Me.HandleModelAddRoleConstraint(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddRoleConstraintArgument
                    Call Me.HandleModelAddRoleConstraintArgument(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddValueType
                    Call Me.HandleModelAddValueType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddEntityType
                    Call Me.HandleModelAddEntityType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelAddFactType
                    Call Me.HandleModelAddFactType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelDeleteEntityType
                    Call Me.HandleModelDeleteEntityType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelDeleteFactType
                    Call Me.HandleModelDeleteFactType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelDeleteFactTypeReading
                    Call Me.HandleModelDeleteFactTypeReading(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelDeleteRoleConstraint
                    Call Me.HandleModelDeleteRoleConstraint(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelDeleteValueType
                    Call Me.HandleModelDeleteValueType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.PageMovePageObject
                    Call Me.HandlePageMovePageObject(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelUpdateValueType
                    Call Me.HandleModelUpdateValueType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelUpdateEntityType
                    Call Me.HandleModelUpdateEntityType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelUpdateFactType
                    Call Me.HandleModelUpdateFactType(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelUpdateFactTypeReading
                    Call Me.HandleModelUpdateFactTypeReading(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.ModelUpdateRole
                    Call Me.HandleModelUpdateRole(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.PageDropModelElementAtPoint
                    Call Me.HandlePageDropModelElementAtPoint(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.PageRemovePageObject
                    Call Me.HandlePageRemovePageObject(arModel, arInterfaceModel)
                Case Is = [Interface].pcenumBroadcastType.RoleReassignJoinedModelObject
                    Call Me.HandleModelRoleReassignJoinedModelObject(arModel, arInterfaceModel)
            End Select

        End Sub

    End Class
End Namespace
