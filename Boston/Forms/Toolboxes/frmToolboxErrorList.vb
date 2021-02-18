Imports System.Reflection
Imports Boston.FBM

Public Class frmToolboxErrorList

    Private cManager As CurrencyManager
    Public WithEvents zrModel As FBM.Model

    Private WithEvents HelpProvider As New System.Windows.Forms.HelpProvider
    Private ziSelectedErrorNumber As Integer = 0

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
            DataGrid_ErrorList.DataSource = Me.zrModel.ModelError
            Me.cManager = CType(DataGrid_ErrorList.BindingContext(Me.zrModel.ModelError), CurrencyManager)
            Me.DataGrid_ErrorList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Me.DataGrid_ErrorList.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try


    End Sub

    Sub UpdateErrorList()

        Try
            Call Me.SetupForm()
            cManager.Refresh()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                lrModelError = Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedRows(0).Index)
            ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
                ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
                lrModelError = Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex)
            Else
                ziSelectedErrorNumber = 0
            End If

            '==========================================================================================================
            If lrModelError IsNot Nothing Then

                Dim larPage As New List(Of FBM.Page)
                Dim loMenuOption As ToolStripItem

                '--------------------------------------------------------
                'Load the ORMDiagrams that relate to the ValueType
                '  as selectable menuOptions
                '--------------------------------------------------------        
                Select Case lrModelError.ModelObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        larPage = prApplication.CMML.get_orm_diagram_pages_for_value_type(lrModelError.ModelObject)
                    Case Is = pcenumConceptType.EntityType
                        larPage = prApplication.CMML.getORMDiagramPagesForEntityType(lrModelError.ModelObject)
                    Case Is = pcenumConceptType.FactType
                        larPage = prApplication.CMML.get_orm_diagram_pages_for_FactType(lrModelError.ModelObject)
                    Case Is = pcenumConceptType.RoleConstraint
                        larPage = prApplication.CMML.GetORMDiagramPagesForRoleConstraint(lrModelError.ModelObject)
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

                        If IsSomething(lrEnterpriseView) Then
                            '---------------------------------------------------
                            'Add the Page(Name) to the MenuOption.DropDownItems
                            '---------------------------------------------------
                            loMenuOption = Me.ToolStripMenuItemShowInDiagram.DropDownItems.Add(lrPage.Name)
                            loMenuOption.Tag = prPageNodes.Find(AddressOf lrEnterpriseView.Equals)
                            AddHandler loMenuOption.Click, AddressOf Me.OpenModelPage
                        End If
                    Next
                End If

            End If
            '==========================================================================================================

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

        Call Me.zrModel.checkForErrors()

    End Sub

    Private Sub DataGrid_ErrorList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGrid_ErrorList.CellClick

        Dim lrModelError As FBM.ModelError = Nothing

        If Me.DataGrid_ErrorList.SelectedRows.Count = 1 Then
            ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedRows(0).Index).Cells(0).Value
            lrModelError = Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedRows(0).Index)
        ElseIf Me.DataGrid_ErrorList.SelectedCells.Count = 1 Then
            ziSelectedErrorNumber = Me.DataGrid_ErrorList.Rows(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex).Cells(0).Value
            lrModelError = Me.zrModel.ModelError(Me.DataGrid_ErrorList.SelectedCells(0).RowIndex)
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
                If IsSomething(lrPropertyGridForm) Then
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

        End If
        '==========================================================================================================


    End Sub

End Class