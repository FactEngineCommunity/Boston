Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleAddModel(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            If prApplication.WorkingProject Is Nothing Or prApplication.WorkingNamespace Is Nothing Then
                Exit Sub
            End If

            If prApplication.WorkingProject.Id = arInterfaceModel.ProjectId _
                And prApplication.WorkingNamespace.Name = arInterfaceModel.Namespace Then

                If frmMain.zfrmModelExplorer IsNot Nothing Then

                    arModel = New FBM.Model(pcenumLanguage.ORMModel,
                                            arInterfaceModel.Name,
                                             arInterfaceModel.ModelId,
                                             prApplication.WorkingNamespace)

                    arModel.IsDirty = True

                    Call frmMain.zfrmModelExplorer.AddModelToModelExplorer(arModel, True)

                End If

            End If

        End Sub

        Private Sub HandleModelSaved(ByRef arModel As FBM.Model, ByRef arInterfactModel As Viev.FBM.Interface.Model)

            arModel.IsDirty = False

            If IsSomething(frmMain.zfrmModelExplorer) Then
                Select Case frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.MenuType
                    Case Is = pcenumMenuType.modelORMModel
                        If arModel Is frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag Then
                            frmMain.ToolStripButton_Save.Enabled = False
                        End If
                        arModel.IsDirty = False
                        Call arModel.makeAllPagesClean()
                    Case Is = pcenumMenuType.pageORMModel
                        Dim lrPage As FBM.Page
                        lrPage = frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag
                        If lrPage.Model Is arModel Then
                            frmMain.ToolStripButton_Save.Enabled = False
                        End If
                        lrPage.Model.IsDirty = False
                        Call lrPage.Model.makeAllPagesClean()
                End Select
            End If

        End Sub

        Private Sub HandleDeleteModel(ByRef arModel As FBM.Model)
            Call arModel.RemoveFromDatabase()

            If frmMain.zfrmModelExplorer IsNot Nothing Then
                Call frmMain.zfrmModelExplorer.DeleteModelTreeNode(arModel)
            End If

        End Sub

        Private Sub HandleSaveModel(ByRef arModel As FBM.Model)

            Call arModel.Save()

            If IsSomething(frmMain.zfrmModelExplorer) Then
                If frmMain.zfrmModelExplorer.TreeView.SelectedNode IsNot Nothing Then
                    Select Case frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.MenuType
                        Case Is = pcenumMenuType.modelORMModel
                            If arModel Is frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag Then
                                frmMain.ToolStripButton_Save.Enabled = False
                            End If
                        Case Is = pcenumMenuType.pageORMModel
                            Dim lrPage As FBM.Page
                            lrPage = frmMain.zfrmModelExplorer.TreeView.SelectedNode.Tag.Tag
                            If lrPage.Model Is arModel Then
                                frmMain.ToolStripButton_Save.Enabled = False
                            End If
                    End Select
                End If
            End If

        End Sub

    End Class

End Namespace
