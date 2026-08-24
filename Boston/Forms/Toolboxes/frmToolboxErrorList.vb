Imports System.Reflection
Imports Boston.FBM

Public Class frmToolboxErrorList

    Private cManager As CurrencyManager
    Public WithEvents zrModel As FBM.Model

    Public WithEvents mrApplication As tApplication

    Private WithEvents HelpProvider As New System.Windows.Forms.HelpProvider
    Private ziSelectedErrorNumber As Integer = 0
    Private mbShowCoreModelErrors As Boolean = False

    Private Sub frmToolboxErrorList_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed

        Me.zrModel = Nothing

    End Sub

    Private Sub frm_error_list_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)
        Me.zrModel = Nothing

    End Sub

    Private Sub frm_error_list_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call SetupForm()

    End Sub

    Sub SetupForm()

        Try
            Me.LabelModelName.Text = "<Select a Model in the Model Explorer>"

            If Me.zrModel IsNot Nothing Then
                Me.LabelModelName.Text = Me.zrModel.Name
            End If

            Call Me.DisplayErrorList

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub DisplayErrorList()

        Try
            'CodeSafe
            If Me.IsDisposed Then Exit Sub

            If mbShowCoreModelErrors Then
                DataGrid_ErrorList.DataSource = Me.zrModel.ModelError
            Else
                Dim larModelError = From ModelError In Me.zrModel.ModelError
                                    Where ModelError.ModelObject IsNot Nothing
                                    Where ModelError.ModelObject.IsMDAModelElement = False
                                    Where ModelError.ErrorId <> pcenumModelErrors.CMMLModelError
                                    Select ModelError

                Dim larCMMLModelError = From ModelError In Me.zrModel.ModelError
                                        Where ModelError.ErrorId = pcenumModelErrors.CMMLModelError
                                        Select ModelError

                Dim larAllErrors As New List(Of FBM.ModelError)

                larAllErrors.AddRange(larModelError.ToList)
                larAllErrors.AddRange(larCMMLModelError.ToList)

                DataGrid_ErrorList.DataSource = larAllErrors 'See CellFormatting for showing the ErrorId rather than its Text (for the Enum)
            End If

            Me.cManager = CType(DataGrid_ErrorList.BindingContext(Me.zrModel.ModelError), CurrencyManager)
            Me.DataGrid_ErrorList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Me.DataGrid_ErrorList.Columns("ErrorId").DefaultCellStyle.Format = "D"
            Me.DataGrid_ErrorList.Columns("SubErrorId").DefaultCellStyle.Format = "D"
            Me.DataGrid_ErrorList.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub UpdateErrorList()

        Try
            Call Me.SetupForm()
            cManager.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub


    Private Sub frm_error_list_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        DataGrid_ErrorList.Width = Me.Width

    End Sub

    Private Sub zrModel_FinishedErrorChecking() Handles zrModel.FinishedErrorChecking

        Call Me.UpdateErrorList()

    End Sub

    Private Sub zrModel_ModelErrorsCleared() Handles zrModel.ModelErrorsCleared

        Call Me.UpdateErrorList()

    End Sub

    Private Sub HandleModelErrorsUpdated() Handles zrModel.ModelErrorsUpdated

        Call Me.UpdateErrorList()

    End Sub

    Private Sub DataGrid_ErrorList_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGrid_ErrorList.DataError
        '-----------------------------------------
        'Keep - stops problems
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click

        Dim loNavigator As HelpNavigator = HelpNavigator.Topic

        Me.HelpProvider.HelpNamespace = My.Settings.HelpfileLocation
        Select Case ziSelectedErrorNumber
            Case Is = 100
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_100.htm")
            Case Is = 106
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_106.htm")
            Case Is = 113
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_113.htm")
            Case Is = 115
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_115.htm")
            Case Is = 126
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_126.htm")
            Case Is = 127
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_127.htm")
            Case Is = 129
                Help.ShowHelp(frmMain, Me.HelpProvider.HelpNamespace, loNavigator, "Model_Error_129.htm")
            Case Else

        End Select

    End Sub

    Private Sub ContextMenuStripHelp_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStripHelp.Opening

        Dim lrModelError As FBM.ModelError = Nothing

        Try

            If Me.DataGrid_ErrorList.SelectedRows.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedRows(0).Index).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.SelectedRows(0).DataBoundItem 'Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedRows(0).Index)
            ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.CurrentRow.DataBoundItem '.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex)
            Else
                ziSelectedErrorNumber = 0
            End If

            '==========================================================================================================
            If lrModelError IsNot Nothing Then

#Region "Load Pages that the ModelElement is on."
                Dim larPage As New List(Of FBM.Page)
                Dim loMenuOption As ToolStripItem

                '--------------------------------------------------------
                'Load the ORMDiagrams that relate to the ValueType
                '  as selectable menuOptions
                '--------------------------------------------------------        
                Select Case lrModelError.ErrorId
                    Case Is = pcenumModelErrors.CMMLModelError
                        Select Case lrModelError.SubErrorId
                            Case Is = pcenumModelSubErrorType.RDSRelationOriginDestintaionColumnCountMismatch
                                larPage = prApplication.CMML.getERDiagramPagesForModelElementName(Me.zrModel, CType(lrModelError.CMMLModelElement, RDS.Relation).OriginTable.Name)
                            Case Else

                        End Select

                    Case Else
                        Select Case lrModelError.ModelObject.ConceptType
                            Case Is = pcenumConceptType.ValueType
                                larPage = prApplication.CMML.getORMDiagramPagesForValueType(lrModelError.ModelObject)
                            Case Is = pcenumConceptType.EntityType
                                larPage = prApplication.CMML.getORMDiagramPagesForEntityType(lrModelError.ModelObject)
                            Case Is = pcenumConceptType.FactType
                                larPage = prApplication.CMML.getORMDiagramPagesForFactType(lrModelError.ModelObject)
                            Case Is = pcenumConceptType.RoleConstraint
                                larPage = prApplication.CMML.GetORMDiagramPagesForRoleConstraint(lrModelError.ModelObject)
                        End Select
                End Select


                Me.ToolStripMenuItemShowInDiagram.DropDownItems.Clear()

                If larPage.Count = 0 Then
                    loMenuOption = Me.ToolStripMenuItemShowInDiagram.DropDownItems.Add("The Model Object related to this error is not on any Page in the Model.")
                Else
                    For Each lrPage In larPage
                        '----------------------------------------------------------
                        'Try and find the Page within the EnterpriseView.TreeView
                        '  NB If 'Core' Pages are not shown for the model, 
                        '  they will not be in the TreeView and so a menuOption
                        '  is now added for those hidden Pages.
                        '----------------------------------------------------------
                        Dim lrEnterpriseView As tEnterpriseEnterpriseView
                        lrEnterpriseView = New tEnterpriseEnterpriseView(pcenumMenuType.pageORMModel,
                                                                   lrPage,
                                                                   Me.zrModel.ModelId,
                                                                   pcenumLanguage.ORMModel,
                                                                   Nothing,
                                                                   lrPage.PageId)

                        lrEnterpriseView = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)

                        lrEnterpriseView.FocusModelElement = lrModelError.ModelObject

                        If lrEnterpriseView IsNot Nothing Then
                            '---------------------------------------------------
                            'Add the Page(Name) to the MenuOption.DropDownItems
                            '---------------------------------------------------
                            loMenuOption = Me.ToolStripMenuItemShowInDiagram.DropDownItems.Add(lrPage.Name)
                            loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                            AddHandler loMenuOption.Click, AddressOf Me.OpenModelPage
                        End If
                    Next
                End If
#End Region

#Region "Set the Fix (if any) for the ModelError"
                Select Case lrModelError.ErrorId
                    Case Is = pcenumModelErrors.RDSRelationsWithMismatchedOriginAndDestinationColumnCount

                        Me.ToolStripMenuItemApplyFix.Tag = New With {.ModelElement = lrModelError.CMMLModelElement, .FixType = pcenumModelFixType.RDSRelationsWhereOriginColumnCountNotEqualDestinationColumnCount}
                        Me.ToolStripMenuItemApplyFix.Enabled = True

                    Case Is = pcenumModelErrors.RDSTableWithSimpleReferenceSchemeButMultiplePrimaryKeyColumns

                        Me.ToolStripMenuItemApplyFix.Tag = New With {.ModelElement = lrModelError.CMMLModelElement, .FixType = pcenumModelFixType.RDSTablesWithSimpleReferenceSchemeAndMultiplePrimaryKeyColumns}
                        Me.ToolStripMenuItemApplyFix.Enabled = True

                    Case Else
                        Me.ToolStripMenuItemApplyFix.Enabled = False
                End Select
#End Region
            End If
            '==========================================================================================================

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub OpenModelPage(ByVal sender As Object, ByVal e As EventArgs)

        Dim lrEnterpriseView As tEnterpriseEnterpriseView
        Dim item As ToolStripItem = CType(sender, ToolStripItem)
        lrEnterpriseView = item.Tag
        prApplication.WorkingPage = lrEnterpriseView.Tag


        frmMain.zfrmModelExplorer.TreeView.SelectedNode = lrEnterpriseView.TreeNode
        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub

    Private Sub zrModel_ModelErrorRemoved(arModelError As ModelError) Handles zrModel.ModelErrorRemoved

        Call Me.UpdateErrorList()

    End Sub

    Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs) Handles ButtonRefresh.Click

        With New WaitCursor

            RemoveHandler zrModel.ModelErrorAdded, AddressOf Me.zrModel_ModelErrorAdded
            Call Me.zrModel.checkForErrors(Me.CheckBoxShowDatabaseMappingErrors.Checked, Me.CheckBoxShowRelationalModelErrors.Checked)
            AddHandler zrModel.ModelErrorAdded, AddressOf Me.zrModel_ModelErrorAdded
        End With
    End Sub

    Private Sub DataGrid_ErrorList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGrid_ErrorList.CellClick

        Dim lrModelError As FBM.ModelError = Nothing

        If Me.DataGrid_ErrorList.SelectedRows.Count = 1 Then
            ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedRows(0).Index).Cells(0).Value
            lrModelError = Me.DataGrid_ErrorList.SelectedRows(0).DataBoundItem
        ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
            ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
            lrModelError = Me.DataGrid_ErrorList.CurrentRow.DataBoundItem
        Else
            ziSelectedErrorNumber = 0
        End If

        '==========================================================================================================
        If lrModelError IsNot Nothing Then

            Dim larPage As New List(Of FBM.Page)


            Dim lrPropertyGridForm As frmToolboxProperties
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If lrPropertyGridForm IsNot Nothing Then
                Dim loMiscFilterAttribute As Attribute = New System.ComponentModel.CategoryAttribute("Misc")
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {loMiscFilterAttribute})
                If lrPropertyGridForm IsNot Nothing Then
                    Dim lrModelElementInstance As FBM.ModelObject = Nothing
                    Dim lrPage As New FBM.Page
                    Select Case lrModelError.ModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrModelElementInstance = CType(lrModelError.ModelObject, FBM.EntityType).CloneInstance(lrPage, False)
                        Case Is = pcenumConceptType.ValueType
                            lrModelElementInstance = CType(lrModelError.ModelObject, FBM.ValueType).CloneInstance(lrPage, False)
                        Case Is = pcenumConceptType.FactType
                            lrModelElementInstance = CType(lrModelError.ModelObject, FBM.FactType).CloneInstance(lrPage, False)
                        Case Is = pcenumConceptType.RoleConstraint
                            lrModelElementInstance = CType(lrModelError.ModelObject, FBM.RoleConstraint).CloneInstance(lrPage, False)
                    End Select
                    If lrModelElementInstance IsNot Nothing Then
                        lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelElementInstance
                        lrPropertyGridForm.BringToFront()
                        lrPropertyGridForm.Show()
                    End If
                End If
            End If

#Region "Verbalisation"
            Dim lrORMToolboxVerbalisation As frmToolboxORMVerbalisation
            lrORMToolboxVerbalisation = prApplication.GetToolboxForm(frmToolboxORMVerbalisation.Name)

            If lrORMToolboxVerbalisation IsNot Nothing Then

                Select Case lrModelError.ErrorId
                    Case Is = pcenumModelErrors.RDSRelationsWithMismatchedOriginAndDestinationColumnCount
                        Dim lrRelation As RDS.Relation = lrModelError.CMMLModelElement
                        Call lrORMToolboxVerbalisation.VerbaliseRelation(lrRelation.ResponsibleFactType, lrRelation, False)
                    Case Else

                End Select

            End If
#End Region

        End If
        '==========================================================================================================


    End Sub

    'Private Sub zrModel_ModelErrorAdded() Handles zrModel.ModelErrorAdded

    'End Sub

    Private Sub zrModel_ModelErrorAdded(ByRef abUpdateErrorList As Boolean) Handles zrModel.ModelErrorAdded

        If Me.IsDisposed Then Exit Sub

        If abUpdateErrorList Then Call Me.UpdateErrorList()

    End Sub

    Private Sub ShowCoreModelErrorsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemShowCoreModelErrors.Click

        Me.ToolStripMenuItemShowCoreModelErrors.Checked = Not Me.ToolStripMenuItemShowCoreModelErrors.Checked

        Me.mbShowCoreModelErrors = Me.ToolStripMenuItemShowCoreModelErrors.Checked

    End Sub

    Private Sub frmToolboxErrorList_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        Try
            If e.Button = MouseButtons.Right Then
                Me.ContextMenuStrip = ContextMenuStripShowCoreModelErrors
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Private Sub ShowTheModelElementInTheModelDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowTheModelElementInTheModelDictionaryToolStripMenuItem.Click

        Dim lrModelError As FBM.ModelError = Nothing

        Try

            If Me.DataGrid_ErrorList.SelectedRows.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedRows(0).Index).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.SelectedRows(0).DataBoundItem 'Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedRows(0).Index)
            ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.CurrentRow.DataBoundItem '.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex)
            Else
                ziSelectedErrorNumber = 0
            End If

            '==========================================================================================================
            If lrModelError IsNot Nothing Then

                Dim lfrmModelDictionary As New frmToolboxModelDictionary
                Dim lrModelObject As FBM.ModelObject

                lrModelObject = lrModelError.ModelObject

                'CodeSafe 
                If lrModelObject Is Nothing Then Exit Sub

                Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

                Try
                    If frmMain.IsDiagramSpyFormLoaded Then
                        Dim larDiagramSpyPage = From ActivePage In prApplication.ActivePages.ToArray
                                                Where ActivePage.Tag IsNot Nothing
                                                Where ActivePage.Tag.GetType Is GetType(FBM.DiagramSpyPage)
                                                Select ActivePage

                        For Each lfrmDiagramSpyPage In larDiagramSpyPage.ToArray
                            Call lfrmDiagramSpyPage.Close()
                        Next
                    End If

                    lrModelObject = frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelObject)
                Catch ex As Exception
                    Exit Sub
                End Try

                If prApplication.RightToolboxForms.FindAll(AddressOf lfrmModelDictionary.EqualsByName).Count = 0 Then
                    Call frmMain.LoadToolboxModelDictionary()
                End If

                lfrmModelDictionary = prApplication.RightToolboxForms.Find(AddressOf lfrmModelDictionary.EqualsByName)
                lfrmModelDictionary.Show()

                Call lfrmModelDictionary.setLanguage(pcenumLanguage.ORMModel)
                Call lfrmModelDictionary.FindTreeNode(lrModelObject.Id)


            End If
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Private Sub mrApplication_WorkingModelChanged() Handles mrApplication.WorkingModelChanged

        Try
            Me.zrModel = prApplication.WorkingModel
            Me.LabelModelName.Text = Me.zrModel.Name

            Call Me.DisplayErrorList()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Sub ToolStripMenuItemApplyFix_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemApplyFix.Click

        Try
            With New WaitCursor
                Dim laiFixType As New List(Of pcenumModelFixType)
                laiFixType.Add(Me.ToolStripMenuItemApplyFix.Tag.FixType)
                Call prApplication.WorkingModel.FixErrors(laiFixType, Me.ToolStripMenuItemApplyFix.Tag.ModelElement)
            End With

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
        End Try

    End Sub

    Private Sub ShowInDiagramSpyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowInDiagramSpyToolStripMenuItem.Click

        Try
            Dim lrModelError As FBM.ModelError = Nothing

            If Me.DataGrid_ErrorList.SelectedRows.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedRows(0).Index).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.SelectedRows(0).DataBoundItem 'Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedRows(0).Index)
            ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
                lrModelError = Me.DataGrid_ErrorList.CurrentRow.DataBoundItem '.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex)
            Else
                ziSelectedErrorNumber = 0
            End If

            '==========================================================================================================
            If lrModelError IsNot Nothing Then

                Select Case lrModelError.ErrorId
                    Case pcenumModelErrors.RDSTableWithSimpleReferenceSchemeButMultiplePrimaryKeyColumns,
                         pcenumModelErrors.RDSRelationsWithMismatchedOriginAndDestinationColumnCount

                        Dim lrTable = lrModelError.ModelObject.getCorrespondingRDSTable()
                        Call frmMain.LoadERDDiagramSpy(lrTable)
                    Case Else
                        Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)
                        Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelError.ModelObject, Control.ModifierKeys = Keys.Control)

                End Select

            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
        End Try

    End Sub

End Class