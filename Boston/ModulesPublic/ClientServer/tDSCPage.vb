Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddPage(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Dim lrPage As FBM.Page
            lrPage = New FBM.Page(arModel, arInterfaceModel.Page.Id, arInterfaceModel.Page.Name, pcenumLanguage.ORMModel)

            arModel.Page.AddUnique(lrPage)

            arModel.IsDirty = True
            lrPage.IsDirty = True

            If prApplication.WorkingProject Is Nothing Or prApplication.WorkingNamespace Is Nothing Then
                Exit Sub
            End If

            If prApplication.WorkingProject.Id = arInterfaceModel.ProjectId _
                And prApplication.WorkingNamespace.Name = arInterfaceModel.Namespace Then

                If frmMain.zfrmModelExplorer IsNot Nothing Then

                    Dim lrModelNode As TreeNode
                    lrModelNode = frmMain.zfrmModelExplorer.TreeView.Nodes.Find(arModel.ModelId, True)(0)
                    Call frmMain.zfrmModelExplorer.AddPageToModel(lrModelNode, lrPage, False)

                End If
            End If


        End Sub

        Private Sub HandlePageDropModelElementAtPoint(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                'CodeSafe
                If Not arModel.Loaded Then Exit Sub

                Dim lrPage As FBM.Page
                lrPage = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                Dim lrInterfaceConceptInstance As Viev.FBM.Interface.ConceptInstance
                lrInterfaceConceptInstance = arInterfaceModel.Page.ConceptInstance

                Dim lrModelElement As FBM.ModelObject = arModel.GetModelObjectByName(lrInterfaceConceptInstance.ModelElementId)

                'CodeSafe
                If lrModelElement Is Nothing Then Exit Sub
                If Not lrPage.Loaded Then Exit Sub

                Select Case lrModelElement.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Call lrPage.DropValueTypeAtPoint(lrModelElement, New PointF(lrInterfaceConceptInstance.X, lrInterfaceConceptInstance.Y))
                    Case Is = pcenumConceptType.EntityType
                        Call lrPage.DropEntityTypeAtPoint(lrModelElement, New PointF(lrInterfaceConceptInstance.X, lrInterfaceConceptInstance.Y))
                    Case Is = pcenumConceptType.FactType
                        Call lrPage.DropFactTypeAtPoint(lrModelElement, New PointF(lrInterfaceConceptInstance.X, lrInterfaceConceptInstance.Y), False, False)
                    Case Is = pcenumConceptType.RoleConstraint
                        Dim lrRoleConstraint As FBM.RoleConstraint = lrModelElement
                        Select Case lrRoleConstraint.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                Call lrPage.DropFrequencyConstraintAtPoint(lrRoleConstraint, New PointF(lrInterfaceConceptInstance.X, lrInterfaceConceptInstance.Y))
                            Case Else
                                Call lrPage.DropRoleConstraintAtPoint(lrRoleConstraint, New PointF(lrInterfaceConceptInstance.X, lrInterfaceConceptInstance.Y))
                        End Select
                    Case Is = pcenumConceptType.ModelNote
                        'N/A At this stage.
                End Select

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub HandlePageMovePageObject(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrPage As FBM.Page
                lrPage = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                Call lrPage.moveModelElement(arInterfaceModel.Page.ConceptInstance)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandlePageRemovePageObject(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrPage As FBM.Page
                lrPage = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                Dim lrInterfaceConceptInstance As Viev.FBM.Interface.ConceptInstance
                lrInterfaceConceptInstance = arInterfaceModel.Page.ConceptInstance

                Dim lrModelObject As FBM.ModelObject

                lrModelObject = lrPage.getModelElementById(lrInterfaceConceptInstance.ModelElementId)

                If lrModelObject IsNot Nothing Then
                    Select Case lrModelObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Call lrPage.RemoveValueTypeInstance(lrModelObject, False)
                        Case Is = pcenumConceptType.EntityType
                            Call lrPage.RemoveEntityTypeInstance(lrModelObject, False)
                        Case Is = pcenumConceptType.FactType
                            Call lrPage.RemoveFactTypeInstance(lrModelObject, False)
                        Case Is = pcenumConceptType.RoleConstraint
                            Call lrPage.RemoveRoleConstraintInstance(lrModelObject, False)
                        Case Is = pcenumConceptType.ModelNote
                            'VM-20180330-ToDo
                    End Select
                End If

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