Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddFactType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType = arInterfaceModel.FactType(0)

                Dim lrFactType As New FBM.FactType(arModel,
                                                   lrInterfaceFactType.Name,
                                                   True)

                lrFactType.ShortDescription = lrInterfaceFactType.ShortDescription
                lrFactType.LongDescription = lrInterfaceFactType.LongDescription
                lrFactType.IsIndependent = lrInterfaceFactType.IsIndependent
                lrFactType.IsObjectified = lrInterfaceFactType.IsObjectified
                lrFactType.IsStored = lrInterfaceFactType.IsStored
                lrFactType.IsPreferredReferenceMode = lrInterfaceFactType.IsPreferredReferenceSchemeFT
                lrFactType.IsSubtypeRelationshipFactType = lrInterfaceFactType.IsSubtypeRelationshipFactType
                lrFactType.DerivationText = lrInterfaceFactType.DerivationText
                lrFactType.IsDerived = lrInterfaceFactType.IsDerived
                lrFactType.IsLinkFactType = lrInterfaceFactType.IsLinkFactType
                lrFactType.IsMDAModelElement = lrInterfaceFactType.IsMDAModelElement
                lrFactType.ShowFactTypeName = lrInterfaceFactType.ShowFactTypeName

                If lrInterfaceFactType.LinkFactTypeRoleId <> "" Then
                    lrFactType.LinkFactTypeRole = arModel.Role.Find(Function(x) x.Id = lrInterfaceFactType.LinkFactTypeRoleId)
                End If

                Dim lrRole As FBM.Role
                For Each lrInterfaceRole In lrInterfaceFactType.RoleGroup
                    lrRole = New FBM.Role(lrFactType, lrInterfaceRole.Id)
                    lrRole.Name = lrInterfaceRole.Name
                    lrRole.SequenceNr = lrInterfaceRole.SequenceNr
                    lrRole.Mandatory = lrInterfaceRole.Mandatory
                    If lrInterfaceRole.JoinedObjectTypeId Is Nothing Then
                        lrRole.JoinedORMObject = Nothing
                    Else
                        'VM-20180318-Add code here to join the Role to the appropriate ModelElement
                        Dim lrJoinedModelObject As FBM.ModelObject
                        lrJoinedModelObject = arModel.GetModelObjectByName(lrInterfaceRole.JoinedObjectTypeId)
                        lrRole.ReassignJoinedModelObject(lrJoinedModelObject, False, Nothing)
                    End If
                    lrFactType.RoleGroup.Add(lrRole)
                    'lrInterfaceRole.ValueConstraint   'N/A At this stage.                            
                Next

                arModel.AddFactType(lrFactType, True, False, Nothing)

                If arInterfaceModel.Page IsNot Nothing Then

                    Dim lrPage As FBM.Page = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                    Dim lrPointF As New PointF(arInterfaceModel.Page.ConceptInstance.X, arInterfaceModel.Page.ConceptInstance.Y)
                    Call lrPage.DropFactTypeAtPoint(lrFactType, lrPointF, False, False)

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelAddFactTypeReading(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType
                lrInterfaceFactType = arInterfaceModel.FactType(0)

                Dim lrFactType As FBM.FactType
                lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType to add a FactTypeReading to.
                If lrFactType Is Nothing Then Exit Sub

                Dim lrFactTypeReading As New FBM.FactTypeReading
                Dim lrInterfaceFactTypeReading As Viev.FBM.Interface.FactTypeReading = arInterfaceModel.FactType(0).FactTypeReadings(0)

                lrFactTypeReading.Id = lrInterfaceFactTypeReading.Id
                lrFactTypeReading.FactType = lrFactType
                lrFactTypeReading.Model = arModel
                lrFactTypeReading.FrontText = lrInterfaceFactTypeReading.FrontReadingText
                lrFactTypeReading.FollowingText = lrInterfaceFactTypeReading.FollowingReadingText

                Dim lrInterfacePredicatePart As Viev.FBM.Interface.PredicatePart
                Dim lrPredicatePart As FBM.PredicatePart
                For Each lrInterfacePredicatePart In lrInterfaceFactTypeReading.PredicatePart
                    lrPredicatePart = New FBM.PredicatePart
                    lrPredicatePart.Model = arModel
                    lrPredicatePart.SequenceNr = lrInterfacePredicatePart.SequenceNr
                    lrPredicatePart.FactTypeReading = lrFactTypeReading

                    lrPredicatePart.Role = arModel.Role.Find(Function(x) x.Id = lrInterfacePredicatePart.Role_Id)
                    lrPredicatePart.PreBoundText = lrInterfacePredicatePart.PreboundReadingText
                    lrPredicatePart.PostBoundText = lrInterfacePredicatePart.PostboundReadingText
                    lrPredicatePart.PredicatePartText = lrInterfacePredicatePart.PredicatePartText

                    lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                Call lrFactType.AddFactTypeReading(lrFactTypeReading, True, False)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelDeleteFactType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType = arInterfaceModel.FactType(0)

                Dim lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType to add a FactTypeReading to.
                If lrFactType Is Nothing Then Exit Sub

                Call lrFactType.RemoveFromModel(False, True, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelDeleteFactTypeReading(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType
                lrInterfaceFactType = arInterfaceModel.FactType(0)

                Dim lrFactType As FBM.FactType
                lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType.
                If lrFactType Is Nothing Then Exit Sub

                Dim lrFactTypeReading As New FBM.FactTypeReading
                Dim lrInterfaceFactTypeReading As Viev.FBM.Interface.FactTypeReading = arInterfaceModel.FactType(0).FactTypeReadings(0)

                lrFactTypeReading.Id = lrInterfaceFactTypeReading.Id

                lrFactTypeReading = lrFactType.FactTypeReading.Find(Function(x) x.Id = lrFactTypeReading.Id)

                'CodeSafe: Exit sub if there is no FactTypeReading.
                If lrFactTypeReading Is Nothing Then Exit Sub

                Call lrFactTypeReading.RemoveFromModel(False, True, False)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelUpdateFactType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType
                lrInterfaceFactType = arInterfaceModel.FactType(0)

                Dim lrFactType As FBM.FactType
                lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType.
                If lrFactType Is Nothing Then Exit Sub

                If lrInterfaceFactType.Name <> lrFactType.Name Then
                    Call lrFactType.setName(lrInterfaceFactType.Name, False)
                ElseIf lrInterfaceFactType.IsDerived <> lrFactType.IsDerived Then
                    Call lrFactType.SetIsDerived(lrInterfaceFactType.IsDerived, False)
                ElseIf lrInterfaceFactType.IsIndependent <> lrFactType.IsIndependent Then
                    Call lrFactType.SetIsIndependent(lrInterfaceFactType.IsIndependent, False)
                ElseIf lrInterfaceFactType.IsObjectified <> lrFactType.IsObjectified Then
                    If lrInterfaceFactType.IsObjectified Then

                        lrFactType.ObjectifyingEntityType = lrFactType.Model.EntityType.Find(Function(x) x.Id = lrInterfaceFactType.ObjectifyingEntityTypeId)

                        Call lrFactType.SetIsObjectified(True, False)
                    Else
                        Call lrFactType.RemoveObjectification(False)
                    End If
                    Call lrFactType.SetIsObjectified(lrInterfaceFactType.IsObjectified, False)
                ElseIf lrInterfaceFactType.DerivationText <> lrFactType.DerivationText Then
                    Call lrFactType.SetDerivationText(lrInterfaceFactType.DerivationText, False)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelUpdateFactTypeReading(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceFactType As Viev.FBM.Interface.FactType
                lrInterfaceFactType = arInterfaceModel.FactType(0)

                Dim lrFactType As FBM.FactType
                lrFactType = arModel.FactType.Find(Function(x) x.Id = lrInterfaceFactType.Id)

                'CodeSafe: Exit sub if there is no FactType.
                If lrFactType Is Nothing Then Exit Sub

                Dim lrFactTypeReading As New FBM.FactTypeReading
                Dim lrInterfaceFactTypeReading As Viev.FBM.Interface.FactTypeReading = arInterfaceModel.FactType(0).FactTypeReadings(0)

                lrFactTypeReading.Id = lrInterfaceFactTypeReading.Id
                lrFactTypeReading.FactType = lrFactType
                lrFactTypeReading.Model = arModel
                lrFactTypeReading.FrontText = lrInterfaceFactTypeReading.FrontReadingText
                lrFactTypeReading.FollowingText = lrInterfaceFactTypeReading.FollowingReadingText

                Dim lrInterfacePredicatePart As Viev.FBM.Interface.PredicatePart
                Dim lrPredicatePart As FBM.PredicatePart
                For Each lrInterfacePredicatePart In lrInterfaceFactTypeReading.PredicatePart
                    lrPredicatePart = New FBM.PredicatePart
                    lrPredicatePart.Model = arModel
                    lrPredicatePart.FactTypeReading = lrFactTypeReading
                    lrPredicatePart.SequenceNr = lrInterfacePredicatePart.SequenceNr

                    lrPredicatePart.Role = arModel.Role.Find(Function(x) x.Id = lrInterfacePredicatePart.Role_Id)
                    lrPredicatePart.PreBoundText = lrInterfacePredicatePart.PreboundReadingText
                    lrPredicatePart.PostBoundText = lrInterfacePredicatePart.PostboundReadingText
                    lrPredicatePart.PredicatePartText = lrInterfacePredicatePart.PredicatePartText

                    lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                Call lrFactType.SetFactTypeReading(lrFactTypeReading, False)

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
